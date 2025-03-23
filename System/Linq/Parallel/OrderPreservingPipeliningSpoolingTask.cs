using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq.Parallel
{
	internal class OrderPreservingPipeliningSpoolingTask<TOutput> : SpoolingTaskBase
	{
		/// <summary>
		/// The number of elements to accumulate on the producer before copying the elements to the 
		/// producer-consumer buffer. This constant is only used in the AutoBuffered mode.
		///
		/// Experimentally, 16 appears to be sufficient buffer size to compensate for the synchronization
		/// cost.
		/// </summary>
		private const int PRODUCER_BUFFER_AUTO_SIZE = 16;

		private readonly QueryTaskGroupState m_taskGroupState;

		private readonly TaskScheduler m_taskScheduler;

		private readonly QueryOperatorEnumerator<TOutput, int> m_partition;

		private readonly bool[] m_consumerWaiting;

		private readonly bool[] m_producerWaiting;

		private readonly bool[] m_producerDone;

		private readonly int m_partitionIndex;

		private readonly Queue<Pair<int, TOutput>>[] m_buffers;

		private readonly object m_bufferLock;

		/// <summary>
		/// Whether the producer is allowed to buffer up elements before handing a chunk to the consumer.
		/// If false, the producer will make each result available to the consumer immediately after it is
		/// produced.
		/// </summary>
		private readonly bool m_autoBuffered;

		/// <summary>
		/// Constructor
		/// </summary>
		internal OrderPreservingPipeliningSpoolingTask(QueryOperatorEnumerator<TOutput, int> partition, QueryTaskGroupState taskGroupState, bool[] consumerWaiting, bool[] producerWaiting, bool[] producerDone, int partitionIndex, Queue<Pair<int, TOutput>>[] buffers, object bufferLock, TaskScheduler taskScheduler, bool autoBuffered)
			: base(partitionIndex, taskGroupState)
		{
			m_partition = partition;
			m_taskGroupState = taskGroupState;
			m_producerDone = producerDone;
			m_consumerWaiting = consumerWaiting;
			m_producerWaiting = producerWaiting;
			m_partitionIndex = partitionIndex;
			m_buffers = buffers;
			m_bufferLock = bufferLock;
			m_taskScheduler = taskScheduler;
			m_autoBuffered = autoBuffered;
		}

		/// <summary>
		/// This method is responsible for enumerating results and enqueueing them to
		/// the output buffer as appropriate.  Each base class implements its own.
		/// </summary>
		protected override void SpoolingWork()
		{
			TOutput currentElement = default(TOutput);
			int currentKey = 0;
			int num = ((!m_autoBuffered) ? 1 : 16);
			Pair<int, TOutput>[] array = new Pair<int, TOutput>[num];
			QueryOperatorEnumerator<TOutput, int> partition = m_partition;
			CancellationToken mergedCancellationToken = m_taskGroupState.CancellationState.MergedCancellationToken;
			int i;
			do
			{
				for (i = 0; i < num; i++)
				{
					if (!partition.MoveNext(ref currentElement, ref currentKey))
					{
						break;
					}
					ref Pair<int, TOutput> reference = ref array[i];
					reference = new Pair<int, TOutput>(currentKey, currentElement);
				}
				if (i == 0)
				{
					break;
				}
				lock (m_bufferLock)
				{
					if (mergedCancellationToken.IsCancellationRequested)
					{
						return;
					}
					for (int j = 0; j < i; j++)
					{
						m_buffers[m_partitionIndex].Enqueue(array[j]);
					}
					if (m_consumerWaiting[m_partitionIndex])
					{
						Monitor.Pulse(m_bufferLock);
						m_consumerWaiting[m_partitionIndex] = false;
					}
					if (m_buffers[m_partitionIndex].Count >= 8192)
					{
						m_producerWaiting[m_partitionIndex] = true;
						Monitor.Wait(m_bufferLock);
					}
				}
			}
			while (i == num);
		}

		/// <summary>
		/// Creates and begins execution of a new set of spooling tasks.
		/// </summary>
		public static void Spool(QueryTaskGroupState groupState, PartitionedStream<TOutput, int> partitions, bool[] consumerWaiting, bool[] producerWaiting, bool[] producerDone, Queue<Pair<int, TOutput>>[] buffers, object[] bufferLocks, TaskScheduler taskScheduler, bool autoBuffered)
		{
			int degreeOfParallelism = partitions.PartitionCount;
			for (int i = 0; i < degreeOfParallelism; i++)
			{
				buffers[i] = new Queue<Pair<int, TOutput>>(128);
				bufferLocks[i] = new object();
			}
			Task task = new Task(delegate
			{
				for (int j = 0; j < degreeOfParallelism; j++)
				{
					QueryTask queryTask = new OrderPreservingPipeliningSpoolingTask<TOutput>(partitions[j], groupState, consumerWaiting, producerWaiting, producerDone, j, buffers, bufferLocks[j], taskScheduler, autoBuffered);
					queryTask.RunAsynchronously(taskScheduler);
				}
			});
			groupState.QueryBegin(task);
			task.Start(taskScheduler);
		}

		/// <summary>
		/// Dispose the underlying enumerator and wake up the consumer if necessary.
		/// </summary>
		protected override void SpoolingFinally()
		{
			lock (m_bufferLock)
			{
				m_producerDone[m_partitionIndex] = true;
				if (m_consumerWaiting[m_partitionIndex])
				{
					Monitor.Pulse(m_bufferLock);
					m_consumerWaiting[m_partitionIndex] = false;
				}
			}
			base.SpoolingFinally();
			m_partition.Dispose();
		}
	}
}
