using System.Collections.Generic;
using System.Threading;

namespace System.Linq.Parallel
{
	/// <summary>
	/// Take and Skip either take or skip a specified number of elements, captured in the
	/// count argument.  These will work a little bit like TakeWhile and SkipWhile: there
	/// are two phases, (1) Search and (2) Yield.  In the search phase, our goal is to
	/// find the 'count'th index from the input.  We do this in parallel by sharing a count-
	/// sized array.  Each thread races to populate the array with indices in ascending
	/// order.  This requires synchronization for inserts.  We use a simple heap, for decent
	/// worst case performance.  After a thread has scanned ‘count’ elements, or its current
	/// index is greater than or equal to the maximum index in the array (and the array is
	/// fully populated), the thread can stop searching.  All threads issue a barrier before
	/// moving to the Yield phase.  When the Yield phase is entered, the count-1th element
	/// of the array contains: in the case of Take, the maximum index (exclusive) to be
	/// returned; or in the case of Skip, the minimum index (inclusive) to be returned.  The
	/// Yield phase simply consists of yielding these elements as output.
	/// </summary>
	/// <typeparam name="TResult"></typeparam>
	internal sealed class TakeOrSkipQueryOperator<TResult> : UnaryQueryOperator<TResult, TResult>
	{
		private class TakeOrSkipQueryOperatorEnumerator : QueryOperatorEnumerator<TResult, int>
		{
			private readonly QueryOperatorEnumerator<TResult, int> m_source;

			private readonly int m_count;

			private readonly bool m_take;

			private readonly FixedMaxHeap<int> m_sharedIndices;

			private readonly CountdownEvent m_sharedBarrier;

			private readonly CancellationToken m_cancellationToken;

			private List<Pair<TResult, int>> m_buffer;

			private Shared<int> m_bufferIndex;

			internal TakeOrSkipQueryOperatorEnumerator(QueryOperatorEnumerator<TResult, int> source, int count, bool take, FixedMaxHeap<int> sharedIndices, CountdownEvent sharedBarrier, CancellationToken cancellationToken)
			{
				m_source = source;
				m_count = count;
				m_take = take;
				m_sharedIndices = sharedIndices;
				m_sharedBarrier = sharedBarrier;
				m_cancellationToken = cancellationToken;
			}

			internal override bool MoveNext(ref TResult currentElement, ref int currentKey)
			{
				if (m_buffer == null && m_count > 0)
				{
					List<Pair<TResult, int>> list = new List<Pair<TResult, int>>();
					TResult currentElement2 = default(TResult);
					int currentKey2 = 0;
					int num = 0;
					while (list.Count < m_count && m_source.MoveNext(ref currentElement2, ref currentKey2))
					{
						if ((num++ & 0x3F) == 0)
						{
							CancellationState.ThrowIfCanceled(m_cancellationToken);
						}
						list.Add(new Pair<TResult, int>(currentElement2, currentKey2));
						lock (m_sharedIndices)
						{
							if (!m_sharedIndices.Insert(currentKey2))
							{
								break;
							}
						}
					}
					m_sharedBarrier.Signal();
					m_sharedBarrier.Wait(m_cancellationToken);
					m_buffer = list;
					m_bufferIndex = new Shared<int>(-1);
				}
				if (m_take)
				{
					if (m_count == 0 || m_bufferIndex.Value >= m_buffer.Count - 1)
					{
						return false;
					}
					m_bufferIndex.Value++;
					currentElement = m_buffer[m_bufferIndex.Value].First;
					currentKey = m_buffer[m_bufferIndex.Value].Second;
					int maxValue = m_sharedIndices.MaxValue;
					if (maxValue != -1)
					{
						return m_buffer[m_bufferIndex.Value].Second <= maxValue;
					}
					return true;
				}
				int num2 = -1;
				if (m_count > 0)
				{
					if (m_sharedIndices.Count < m_count)
					{
						return false;
					}
					num2 = m_sharedIndices.MaxValue;
					if (m_bufferIndex.Value < m_buffer.Count - 1)
					{
						m_bufferIndex.Value++;
						while (m_bufferIndex.Value < m_buffer.Count)
						{
							if (m_buffer[m_bufferIndex.Value].Second > num2)
							{
								currentElement = m_buffer[m_bufferIndex.Value].First;
								currentKey = m_buffer[m_bufferIndex.Value].Second;
								return true;
							}
							m_bufferIndex.Value++;
						}
					}
				}
				if (m_source.MoveNext(ref currentElement, ref currentKey))
				{
					return true;
				}
				return false;
			}

			protected override void Dispose(bool disposing)
			{
				m_source.Dispose();
			}
		}

		private class TakeOrSkipQueryOperatorResults : UnaryQueryOperatorResults
		{
			private TakeOrSkipQueryOperator<TResult> m_takeOrSkipOp;

			private int m_childCount;

			internal override bool IsIndexible => m_childCount >= 0;

			internal override int ElementsCount
			{
				get
				{
					if (m_takeOrSkipOp.m_take)
					{
						return Math.Min(m_childCount, m_takeOrSkipOp.m_count);
					}
					return Math.Max(m_childCount - m_takeOrSkipOp.m_count, 0);
				}
			}

			public static QueryResults<TResult> NewResults(QueryResults<TResult> childQueryResults, TakeOrSkipQueryOperator<TResult> op, QuerySettings settings, bool preferStriping)
			{
				if (childQueryResults.IsIndexible)
				{
					return new TakeOrSkipQueryOperatorResults(childQueryResults, op, settings, preferStriping);
				}
				return new UnaryQueryOperatorResults(childQueryResults, op, settings, preferStriping);
			}

			private TakeOrSkipQueryOperatorResults(QueryResults<TResult> childQueryResults, TakeOrSkipQueryOperator<TResult> takeOrSkipOp, QuerySettings settings, bool preferStriping)
				: base(childQueryResults, (UnaryQueryOperator<TResult, TResult>)takeOrSkipOp, settings, preferStriping)
			{
				m_takeOrSkipOp = takeOrSkipOp;
				m_childCount = m_childQueryResults.ElementsCount;
			}

			internal override TResult GetElement(int index)
			{
				if (m_takeOrSkipOp.m_take)
				{
					return m_childQueryResults.GetElement(index);
				}
				return m_childQueryResults.GetElement(m_takeOrSkipOp.m_count + index);
			}
		}

		private readonly int m_count;

		private readonly bool m_take;

		private bool m_prematureMerge;

		internal override bool LimitsParallelism => OrdinalIndexState != OrdinalIndexState.Indexible;

		internal TakeOrSkipQueryOperator(IEnumerable<TResult> child, int count, bool take)
			: base(child)
		{
			m_count = count;
			m_take = take;
			SetOrdinalIndexState(OutputOrdinalIndexState());
		}

		/// <summary>
		/// Determines the order index state for the output operator
		/// </summary>
		private OrdinalIndexState OutputOrdinalIndexState()
		{
			OrdinalIndexState ordinalIndexState = base.Child.OrdinalIndexState;
			if (ordinalIndexState == OrdinalIndexState.Indexible)
			{
				return OrdinalIndexState.Indexible;
			}
			if (ordinalIndexState.IsWorseThan(OrdinalIndexState.Increasing))
			{
				m_prematureMerge = true;
				ordinalIndexState = OrdinalIndexState.Correct;
			}
			if (!m_take && ordinalIndexState == OrdinalIndexState.Correct)
			{
				ordinalIndexState = OrdinalIndexState.Increasing;
			}
			return ordinalIndexState;
		}

		internal override void WrapPartitionedStream<TKey>(PartitionedStream<TResult, TKey> inputStream, IPartitionedStreamRecipient<TResult> recipient, bool preferStriping, QuerySettings settings)
		{
			PartitionedStream<TResult, int> partitionedStream;
			if (m_prematureMerge)
			{
				ListQueryResults<TResult> listQueryResults = QueryOperator<TResult>.ExecuteAndCollectResults(inputStream, inputStream.PartitionCount, base.Child.OutputOrdered, preferStriping, settings);
				partitionedStream = listQueryResults.GetPartitionedStream();
			}
			else
			{
				partitionedStream = (PartitionedStream<TResult, int>)(object)inputStream;
			}
			int partitionCount = inputStream.PartitionCount;
			FixedMaxHeap<int> sharedIndices = new FixedMaxHeap<int>(m_count);
			CountdownEvent sharedBarrier = new CountdownEvent(partitionCount);
			PartitionedStream<TResult, int> partitionedStream2 = new PartitionedStream<TResult, int>(partitionCount, Util.GetDefaultComparer<int>(), OrdinalIndexState);
			for (int i = 0; i < partitionCount; i++)
			{
				partitionedStream2[i] = new TakeOrSkipQueryOperatorEnumerator(partitionedStream[i], m_count, m_take, sharedIndices, sharedBarrier, settings.CancellationState.MergedCancellationToken);
			}
			recipient.Receive(partitionedStream2);
		}

		internal override QueryResults<TResult> Open(QuerySettings settings, bool preferStriping)
		{
			QueryResults<TResult> childQueryResults = base.Child.Open(settings, preferStriping: true);
			return TakeOrSkipQueryOperatorResults.NewResults(childQueryResults, this, settings, preferStriping);
		}

		internal override IEnumerable<TResult> AsSequentialQuery(CancellationToken token)
		{
			if (m_take)
			{
				return base.Child.AsSequentialQuery(token).Take(m_count);
			}
			IEnumerable<TResult> source = CancellableEnumerable.Wrap(base.Child.AsSequentialQuery(token), token);
			return source.Skip(m_count);
		}
	}
}
