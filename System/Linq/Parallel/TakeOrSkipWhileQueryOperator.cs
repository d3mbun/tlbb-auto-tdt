using System.Collections.Generic;
using System.Threading;

namespace System.Linq.Parallel
{
	/// <summary>
	/// Take- and SkipWhile work similarly. Execution is broken into two phases: Search
	/// and Yield.
	///
	/// During the Search phase, many partitions at once search for the first occurrence
	/// of a false element.  As they search, any time a partition finds a false element
	/// whose index is lesser than the current lowest-known false element, the new index
	/// will be published, so other partitions can stop the search.  The search stops
	/// as soon as (1) a partition exhausts its input, (2) the predicate yields false for
	/// one of the partition's elements, or (3) its input index passes the current lowest-
	/// known index (sufficient since a given partition's indices are always strictly
	/// incrementing -- asserted below).  Elements are buffered during this process.
	///
	/// Partitions use a barrier after Search and before moving on to Yield.  Once all
	/// have passed the barrier, Yielding begins.  At this point, the lowest-known false
	/// index will be accurate for the entire set, since all partitions have finished
	/// scanning.  This is where TakeWhile and SkipWhile differ.  TakeWhile will start at
	/// the beginning of its buffer and yield all elements whose indices are less than
	/// the lowest-known false index.  SkipWhile, on the other hand, will skipp any such
	/// elements in the buffer, yielding those whose index is greater than or equal to
	/// the lowest-known false index, and then finish yielding any remaining elements in
	/// its data source (since it may have stopped prematurely due to (3) above).
	/// </summary>
	/// <typeparam name="TResult"></typeparam>
	internal sealed class TakeOrSkipWhileQueryOperator<TResult> : UnaryQueryOperator<TResult, TResult>
	{
		private class TakeOrSkipWhileQueryOperatorEnumerator : QueryOperatorEnumerator<TResult, int>
		{
			private readonly QueryOperatorEnumerator<TResult, int> m_source;

			private readonly Func<TResult, bool> m_predicate;

			private readonly Func<TResult, int, bool> m_indexedPredicate;

			private readonly bool m_take;

			private readonly Shared<int> m_sharedLowFalse;

			private readonly CountdownEvent m_sharedBarrier;

			private readonly CancellationToken m_cancellationToken;

			private List<Pair<TResult, int>> m_buffer;

			private Shared<int> m_bufferIndex;

			internal TakeOrSkipWhileQueryOperatorEnumerator(QueryOperatorEnumerator<TResult, int> source, Func<TResult, bool> predicate, Func<TResult, int, bool> indexedPredicate, bool take, Shared<int> sharedLowFalse, CountdownEvent sharedBarrier, CancellationToken cancelToken)
			{
				m_source = source;
				m_predicate = predicate;
				m_indexedPredicate = indexedPredicate;
				m_take = take;
				m_sharedLowFalse = sharedLowFalse;
				m_sharedBarrier = sharedBarrier;
				m_cancellationToken = cancelToken;
			}

			internal override bool MoveNext(ref TResult currentElement, ref int currentKey)
			{
				if (m_buffer == null)
				{
					List<Pair<TResult, int>> list = new List<Pair<TResult, int>>();
					try
					{
						TResult currentElement2 = default(TResult);
						int currentKey2 = 0;
						int num = 0;
						while (m_source.MoveNext(ref currentElement2, ref currentKey2))
						{
							if ((num++ & 0x3F) == 0)
							{
								CancellationState.ThrowIfCanceled(m_cancellationToken);
							}
							list.Add(new Pair<TResult, int>(currentElement2, currentKey2));
							int value = m_sharedLowFalse.Value;
							if (value != -1 && currentKey2 > value)
							{
								break;
							}
							if ((m_predicate == null) ? m_indexedPredicate(currentElement2, currentKey2) : m_predicate(currentElement2))
							{
								continue;
							}
							SpinWait spinWait = default(SpinWait);
							while (true)
							{
								int num2 = Thread.VolatileRead(ref m_sharedLowFalse.Value);
								if ((num2 == -1 || num2 >= currentKey2) && Interlocked.CompareExchange(ref m_sharedLowFalse.Value, currentKey2, num2) != num2)
								{
									spinWait.SpinOnce();
									continue;
								}
								break;
							}
							break;
						}
					}
					finally
					{
						m_sharedBarrier.Signal();
					}
					m_sharedBarrier.Wait(m_cancellationToken);
					m_buffer = list;
					m_bufferIndex = new Shared<int>(-1);
				}
				if (m_take)
				{
					if (m_bufferIndex.Value >= m_buffer.Count - 1)
					{
						return false;
					}
					m_bufferIndex.Value++;
					currentElement = m_buffer[m_bufferIndex.Value].First;
					currentKey = m_buffer[m_bufferIndex.Value].Second;
					if (m_sharedLowFalse.Value != -1)
					{
						return m_sharedLowFalse.Value > m_buffer[m_bufferIndex.Value].Second;
					}
					return true;
				}
				if (m_sharedLowFalse.Value == -1)
				{
					return false;
				}
				if (m_bufferIndex.Value < m_buffer.Count - 1)
				{
					m_bufferIndex.Value++;
					while (m_bufferIndex.Value < m_buffer.Count)
					{
						if (m_buffer[m_bufferIndex.Value].Second >= m_sharedLowFalse.Value)
						{
							currentElement = m_buffer[m_bufferIndex.Value].First;
							currentKey = m_buffer[m_bufferIndex.Value].Second;
							return true;
						}
						m_bufferIndex.Value++;
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

		private Func<TResult, bool> m_predicate;

		private Func<TResult, int, bool> m_indexedPredicate;

		private readonly bool m_take;

		private bool m_prematureMerge;

		internal override bool LimitsParallelism => true;

		internal TakeOrSkipWhileQueryOperator(IEnumerable<TResult> child, Func<TResult, bool> predicate, Func<TResult, int, bool> indexedPredicate, bool take)
			: base(child)
		{
			m_predicate = predicate;
			m_indexedPredicate = indexedPredicate;
			m_take = take;
			SetOrdinalIndexState(OutputOrderIndexState());
		}

		/// <summary>
		/// Determines the order index state for the output operator
		/// </summary>
		private OrdinalIndexState OutputOrderIndexState()
		{
			OrdinalIndexState state = OrdinalIndexState.Increasing;
			if (m_indexedPredicate != null)
			{
				state = OrdinalIndexState.Correct;
			}
			OrdinalIndexState ordinalIndexState = base.Child.OrdinalIndexState.Worse(OrdinalIndexState.Correct);
			if (ordinalIndexState.IsWorseThan(state))
			{
				m_prematureMerge = true;
			}
			if (!m_take)
			{
				ordinalIndexState = ordinalIndexState.Worse(OrdinalIndexState.Increasing);
			}
			return ordinalIndexState;
		}

		internal override void WrapPartitionedStream<TKey>(PartitionedStream<TResult, TKey> inputStream, IPartitionedStreamRecipient<TResult> recipient, bool preferStriping, QuerySettings settings)
		{
			int partitionCount = inputStream.PartitionCount;
			PartitionedStream<TResult, int> partitionedStream;
			if (m_prematureMerge)
			{
				ListQueryResults<TResult> listQueryResults = QueryOperator<TResult>.ExecuteAndCollectResults(inputStream, partitionCount, base.Child.OutputOrdered, preferStriping, settings);
				partitionedStream = listQueryResults.GetPartitionedStream();
			}
			else
			{
				partitionedStream = (PartitionedStream<TResult, int>)(object)inputStream;
			}
			Shared<int> sharedLowFalse = new Shared<int>(-1);
			CountdownEvent sharedBarrier = new CountdownEvent(partitionCount);
			PartitionedStream<TResult, int> partitionedStream2 = new PartitionedStream<TResult, int>(partitionCount, Util.GetDefaultComparer<int>(), OrdinalIndexState);
			for (int i = 0; i < partitionCount; i++)
			{
				partitionedStream2[i] = new TakeOrSkipWhileQueryOperatorEnumerator(partitionedStream[i], m_predicate, m_indexedPredicate, m_take, sharedLowFalse, sharedBarrier, settings.CancellationState.MergedCancellationToken);
			}
			recipient.Receive(partitionedStream2);
		}

		internal override QueryResults<TResult> Open(QuerySettings settings, bool preferStriping)
		{
			QueryResults<TResult> childQueryResults = base.Child.Open(settings, preferStriping: true);
			return new UnaryQueryOperatorResults(childQueryResults, this, settings, preferStriping);
		}

		internal override IEnumerable<TResult> AsSequentialQuery(CancellationToken token)
		{
			if (m_take)
			{
				if (m_indexedPredicate != null)
				{
					return base.Child.AsSequentialQuery(token).TakeWhile(m_indexedPredicate);
				}
				return base.Child.AsSequentialQuery(token).TakeWhile(m_predicate);
			}
			if (m_indexedPredicate != null)
			{
				IEnumerable<TResult> source = CancellableEnumerable.Wrap(base.Child.AsSequentialQuery(token), token);
				return source.SkipWhile(m_indexedPredicate);
			}
			IEnumerable<TResult> source2 = CancellableEnumerable.Wrap(base.Child.AsSequentialQuery(token), token);
			return source2.SkipWhile(m_predicate);
		}
	}
}
