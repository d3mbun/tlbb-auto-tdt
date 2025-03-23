using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq.Parallel
{
	/// <summary>
	/// A merge helper that yields results in a streaming fashion, while still ensuring correct output
	/// ordering. This merge only works if each producer task generates outputs in the correct order,
	/// i.e. with an Increasing (or Correct) order index.
	///
	/// The merge creates DOP producer tasks, each of which will be  writing results into a separate
	/// buffer.
	///
	/// The consumer always waits until each producer buffer contains at least one element. If we don't
	/// have one element from each producer, we cannot yield the next element. (If the order index is 
	/// Correct, or in some special cases with the Increasing order, we could yield sooner. The
	/// current algorithm does not take advantage of this.)
	///
	/// The consumer maintains a producer heap, and uses it to decide which producer should yield the next output
	/// result. After yielding an element from a particular producer, the consumer will take another element
	/// from the same producer. However, if the producer buffer exceeded a particular threshold, the consumer
	/// will take the entire buffer, and give the producer an empty buffer to fill.
	///
	/// Finally, if the producer notices that its buffer has exceeded an even greater threshold, it will
	/// go to sleep and wait until the consumer takes the entire buffer.
	/// </summary>
	internal class OrderPreservingPipeliningMergeHelper<TOutput> : IMergeHelper<TOutput>
	{
		/// <summary>
		/// A structure to represent a producer in the producer heap.
		/// </summary>
		private struct Producer
		{
			internal readonly int MaxKey;

			internal readonly int ProducerIndex;

			internal Producer(int maxKey, int producerIndex)
			{
				MaxKey = maxKey;
				ProducerIndex = producerIndex;
			}
		}

		/// <summary>
		/// A comparer used by FixedMaxHeap(Of Producer)
		///
		/// This comparer will be used by max-heap. We want the producer with the smallest MaxKey to
		/// end up in the root of the heap.
		///
		///     x.MaxKey GREATER_THAN y.MaxKey  =&gt;  x LESS_THAN y     =&gt; return -
		///     x.MaxKey EQUALS y.MaxKey        =&gt;  x EQUALS y        =&gt; return 0
		///     x.MaxKey LESS_THAN y.MaxKey     =&gt;  x GREATER_THAN y  =&gt; return +
		/// </summary>
		private class ProducerComparer : IComparer<Producer>
		{
			public int Compare(Producer x, Producer y)
			{
				return y.MaxKey - x.MaxKey;
			}
		}

		/// <summary>
		/// Enumerator over the results of an order-preserving pipelining merge.
		/// </summary>
		private class OrderedPipeliningMergeEnumerator : MergeEnumerator<TOutput>
		{
			/// <summary>
			/// Merge helper associated with this enumerator
			/// </summary>
			private OrderPreservingPipeliningMergeHelper<TOutput> m_mergeHelper;

			/// <summary>
			/// Heap used to efficiently locate the producer whose result should be consumed next.
			/// For each producer, stores the order index for the next element to be yielded.
			///
			/// Read and written by the consumer only.
			/// </summary>
			private readonly FixedMaxHeap<Producer> m_producerHeap;

			/// <summary>
			/// Stores the next element to be yielded from each producer. We use a separate array
			/// rather than storing this information in the producer heap to keep the Producer struct 
			/// small.
			///
			/// Read and written by the consumer only.
			/// </summary>
			private readonly TOutput[] m_producerNextElement;

			/// <summary>
			/// A private buffer for the consumer. When the size of a producer buffer exceeds a threshold 
			/// (STEAL_BUFFER_SIZE), the consumer will take ownership of the entire buffer, and give the
			/// producer a new empty buffer to place results into.
			///
			/// Read and written by the consumer only.
			/// </summary>
			private readonly Queue<Pair<int, TOutput>>[] m_privateBuffer;

			/// <summary>
			/// Tracks whether MoveNext() has already been called previously.
			/// </summary>
			private bool m_initialized;

			/// <summary>
			/// Returns the current result
			/// </summary>
			public override TOutput Current
			{
				get
				{
					int producerIndex = m_producerHeap.MaxValue.ProducerIndex;
					return m_producerNextElement[producerIndex];
				}
			}

			/// <summary>
			/// Constructor
			/// </summary>
			internal OrderedPipeliningMergeEnumerator(OrderPreservingPipeliningMergeHelper<TOutput> mergeHelper)
				: base(mergeHelper.m_taskGroupState)
			{
				int partitionCount = mergeHelper.m_partitions.PartitionCount;
				m_mergeHelper = mergeHelper;
				m_producerHeap = new FixedMaxHeap<Producer>(partitionCount, OrderPreservingPipeliningMergeHelper<TOutput>.s_producerComparer);
				m_privateBuffer = new Queue<Pair<int, TOutput>>[partitionCount];
				m_producerNextElement = new TOutput[partitionCount];
			}

			/// <summary>
			/// Moves the enumerator to the next result, or returns false if there are no more results to yield.
			/// </summary>
			public override bool MoveNext()
			{
				if (!m_initialized)
				{
					m_initialized = true;
					for (int i = 0; i < m_mergeHelper.m_partitions.PartitionCount; i++)
					{
						Pair<int, TOutput> element = default(Pair<int, TOutput>);
						if (TryWaitForElement(i, ref element))
						{
							m_producerHeap.Insert(new Producer(element.First, i));
							m_producerNextElement[i] = element.Second;
						}
						else
						{
							ThrowIfInTearDown();
						}
					}
				}
				else
				{
					if (m_producerHeap.Count == 0)
					{
						return false;
					}
					int producerIndex = m_producerHeap.MaxValue.ProducerIndex;
					Pair<int, TOutput> element2 = default(Pair<int, TOutput>);
					if (TryGetPrivateElement(producerIndex, ref element2) || TryWaitForElement(producerIndex, ref element2))
					{
						m_producerHeap.ReplaceMax(new Producer(element2.First, producerIndex));
						m_producerNextElement[producerIndex] = element2.Second;
					}
					else
					{
						ThrowIfInTearDown();
						m_producerHeap.RemoveMax();
					}
				}
				return m_producerHeap.Count > 0;
			}

			/// <summary>
			/// If the cancellation of the query has been initiated (because one or more producers
			/// encountered exceptions, or because external cancellation token has been set), the method 
			/// will tear down the query and rethrow the exception.
			/// </summary>
			private void ThrowIfInTearDown()
			{
				if (!m_mergeHelper.m_taskGroupState.CancellationState.MergedCancellationToken.IsCancellationRequested)
				{
					return;
				}
				try
				{
					object[] bufferLocks = m_mergeHelper.m_bufferLocks;
					for (int i = 0; i < bufferLocks.Length; i++)
					{
						lock (bufferLocks[i])
						{
							Monitor.Pulse(bufferLocks[i]);
						}
					}
					m_taskGroupState.QueryEnd(userInitiatedDispose: false);
				}
				finally
				{
					m_producerHeap.Clear();
				}
			}

			/// <summary>
			/// Wait until a producer's buffer is non-empty, or until that producer is done.
			/// </summary>
			/// <returns>false if there is no element to yield because the producer is done, true otherwise</returns>
			private bool TryWaitForElement(int producer, ref Pair<int, TOutput> element)
			{
				Queue<Pair<int, TOutput>> queue = m_mergeHelper.m_buffers[producer];
				object obj = m_mergeHelper.m_bufferLocks[producer];
				lock (obj)
				{
					if (queue.Count == 0)
					{
						if (m_mergeHelper.m_producerDone[producer])
						{
							element = default(Pair<int, TOutput>);
							return false;
						}
						m_mergeHelper.m_consumerWaiting[producer] = true;
						Monitor.Wait(obj);
						if (queue.Count == 0)
						{
							element = default(Pair<int, TOutput>);
							return false;
						}
					}
					if (m_mergeHelper.m_producerWaiting[producer])
					{
						Monitor.Pulse(obj);
						m_mergeHelper.m_producerWaiting[producer] = false;
					}
					if (queue.Count < 1024)
					{
						element = queue.Dequeue();
						return true;
					}
					m_privateBuffer[producer] = m_mergeHelper.m_buffers[producer];
					m_mergeHelper.m_buffers[producer] = new Queue<Pair<int, TOutput>>(128);
				}
				TryGetPrivateElement(producer, ref element);
				return true;
			}

			/// <summary>
			/// Looks for an element from a particular producer in the consumer's private buffer.
			/// </summary>
			private bool TryGetPrivateElement(int producer, ref Pair<int, TOutput> element)
			{
				Queue<Pair<int, TOutput>> queue = m_privateBuffer[producer];
				if (queue != null)
				{
					if (queue.Count > 0)
					{
						element = queue.Dequeue();
						return true;
					}
					m_privateBuffer[producer] = null;
				}
				return false;
			}

			public override void Dispose()
			{
				int num = m_mergeHelper.m_buffers.Length;
				for (int i = 0; i < num; i++)
				{
					object obj = m_mergeHelper.m_bufferLocks[i];
					lock (obj)
					{
						if (m_mergeHelper.m_producerWaiting[i])
						{
							Monitor.Pulse(obj);
						}
					}
				}
				base.Dispose();
			}
		}

		/// <summary>
		/// The initial capacity of the buffer queue. The value was chosen experimentally.
		/// </summary>
		internal const int INITIAL_BUFFER_SIZE = 128;

		/// <summary>
		/// If the consumer notices that the queue reached this limit, it will take the entire buffer from
		/// the producer, instead of just popping off one result. The value was chosen experimentally.
		/// </summary>
		internal const int STEAL_BUFFER_SIZE = 1024;

		/// <summary>
		/// If the producer notices that the queue reached this limit, it will go to sleep until woken up
		/// by the consumer. Chosen experimentally.
		/// </summary>
		internal const int MAX_BUFFER_SIZE = 8192;

		private readonly QueryTaskGroupState m_taskGroupState;

		private readonly PartitionedStream<TOutput, int> m_partitions;

		private readonly TaskScheduler m_taskScheduler;

		/// <summary>
		/// Whether the producer is allowed to buffer up elements before handing a chunk to the consumer.
		/// If false, the producer will make each result available to the consumer immediately after it is
		/// produced.
		/// </summary>
		private readonly bool m_autoBuffered;

		/// <summary>
		/// Buffers for the results. Each buffer has elements added by one producer, and removed
		/// by the consumer.
		/// </summary>
		private readonly Queue<Pair<int, TOutput>>[] m_buffers;

		/// <summary>
		/// Whether each producer is done producing. Set to true by individual producers, read by consumer.
		/// </summary>
		private readonly bool[] m_producerDone;

		/// <summary>
		/// Whether a particular producer is waiting on the consumer. Read by the consumer, set to true
		/// by producers, set to false by the consumer.
		/// </summary>
		private readonly bool[] m_producerWaiting;

		/// <summary>
		///  Whether the consumer is waiting on a particular producer. Read by producers, set to true
		///  by consumer, set to false by producer.
		/// </summary>
		private readonly bool[] m_consumerWaiting;

		/// <summary>
		/// Each object is a lock protecting the corresponding elements in m_buffers, m_producerDone, 
		/// m_producerWaiting and m_consumerWaiting.
		/// </summary>
		private readonly object[] m_bufferLocks;

		/// <summary>
		/// A singleton instance of the comparer used by the producer heap. Eager allocation is OK 
		/// because if the static constructor runs, we will be using this merge.
		/// </summary>
		private static ProducerComparer s_producerComparer = new ProducerComparer();

		internal OrderPreservingPipeliningMergeHelper(PartitionedStream<TOutput, int> partitions, TaskScheduler taskScheduler, CancellationState cancellationState, bool autoBuffered, int queryId)
		{
			m_taskGroupState = new QueryTaskGroupState(cancellationState, queryId);
			m_partitions = partitions;
			m_taskScheduler = taskScheduler;
			m_autoBuffered = autoBuffered;
			int partitionCount = m_partitions.PartitionCount;
			m_buffers = new Queue<Pair<int, TOutput>>[partitionCount];
			m_producerDone = new bool[partitionCount];
			m_consumerWaiting = new bool[partitionCount];
			m_producerWaiting = new bool[partitionCount];
			m_bufferLocks = new object[partitionCount];
		}

		void IMergeHelper<TOutput>.Execute()
		{
			OrderPreservingPipeliningSpoolingTask<TOutput>.Spool(m_taskGroupState, m_partitions, m_consumerWaiting, m_producerWaiting, m_producerDone, m_buffers, m_bufferLocks, m_taskScheduler, m_autoBuffered);
		}

		IEnumerator<TOutput> IMergeHelper<TOutput>.GetEnumerator()
		{
			return new OrderedPipeliningMergeEnumerator(this);
		}

		public TOutput[] GetResultsAsArray()
		{
			throw new InvalidOperationException();
		}
	}
}
