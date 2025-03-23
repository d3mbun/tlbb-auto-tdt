using System.Collections.Generic;
using System.Threading;

namespace System.Linq.Parallel
{
	/// <summary>
	/// Last tries to discover the last element in the source, optionally matching a
	/// predicate.  All partitions search in parallel, publish the greatest index for a
	/// candidate match, and reach a barrier.  Only the partition that "wins" the race,
	/// i.e. who found the candidate with the largest index, will yield an element.
	///
	/// @TODO: @PERF: @BUG#414: this traverses the data source in forward-order.  In the future, we
	///     will want to traverse in reverse order, since this allows partitions to stop
	///     the search sooner (by watching if the current index passes below the current best).
	///
	/// </summary>
	/// <typeparam name="TSource"></typeparam>
	internal sealed class LastQueryOperator<TSource> : UnaryQueryOperator<TSource, TSource>
	{
		private class LastQueryOperatorEnumerator<TKey> : QueryOperatorEnumerator<TSource, int>
		{
			private QueryOperatorEnumerator<TSource, int> m_source;

			private Func<TSource, bool> m_predicate;

			private bool m_alreadySearched;

			private Shared<int> m_sharedLastCandidate;

			private CountdownEvent m_sharedBarrier;

			private CancellationToken m_cancellationToken;

			internal LastQueryOperatorEnumerator(QueryOperatorEnumerator<TSource, int> source, Func<TSource, bool> predicate, Shared<int> sharedLastCandidate, CountdownEvent sharedBarrier, CancellationToken cancelToken)
			{
				m_source = source;
				m_predicate = predicate;
				m_sharedLastCandidate = sharedLastCandidate;
				m_sharedBarrier = sharedBarrier;
				m_cancellationToken = cancelToken;
			}

			internal override bool MoveNext(ref TSource currentElement, ref int currentKey)
			{
				if (m_alreadySearched)
				{
					return false;
				}
				TSource val = default(TSource);
				int num = -1;
				try
				{
					TSource currentElement2 = default(TSource);
					int currentKey2 = 0;
					int num2 = 0;
					while (m_source.MoveNext(ref currentElement2, ref currentKey2))
					{
						if ((num2 & 0x3F) == 0)
						{
							CancellationState.ThrowIfCanceled(m_cancellationToken);
						}
						if (m_predicate == null || m_predicate(currentElement2))
						{
							val = currentElement2;
							num = currentKey2;
						}
						num2++;
					}
					if (num != -1)
					{
						int value;
						do
						{
							value = m_sharedLastCandidate.Value;
						}
						while ((value == -1 || num > value) && Interlocked.CompareExchange(ref m_sharedLastCandidate.Value, num, value) != value);
					}
				}
				finally
				{
					m_sharedBarrier.Signal();
				}
				m_alreadySearched = true;
				if (num != -1)
				{
					m_sharedBarrier.Wait(m_cancellationToken);
					if (m_sharedLastCandidate.Value == num)
					{
						currentElement = val;
						currentKey = 0;
						return true;
					}
				}
				return false;
			}

			protected override void Dispose(bool disposing)
			{
				m_source.Dispose();
			}
		}

		private readonly Func<TSource, bool> m_predicate;

		private readonly bool m_prematureMergeNeeded;

		internal override bool LimitsParallelism => m_prematureMergeNeeded;

		internal LastQueryOperator(IEnumerable<TSource> child, Func<TSource, bool> predicate)
			: base(child)
		{
			m_predicate = predicate;
			m_prematureMergeNeeded = base.Child.OrdinalIndexState.IsWorseThan(OrdinalIndexState.Increasing);
		}

		internal override QueryResults<TSource> Open(QuerySettings settings, bool preferStriping)
		{
			QueryResults<TSource> childQueryResults = base.Child.Open(settings, preferStriping: false);
			return new UnaryQueryOperatorResults(childQueryResults, this, settings, preferStriping);
		}

		internal override void WrapPartitionedStream<TKey>(PartitionedStream<TSource, TKey> inputStream, IPartitionedStreamRecipient<TSource> recipient, bool preferStriping, QuerySettings settings)
		{
			int partitionCount = inputStream.PartitionCount;
			PartitionedStream<TSource, int> partitionedStream = ((!m_prematureMergeNeeded) ? ((PartitionedStream<TSource, int>)(object)inputStream) : QueryOperator<TSource>.ExecuteAndCollectResults(inputStream, partitionCount, base.Child.OutputOrdered, preferStriping, settings).GetPartitionedStream());
			Shared<int> sharedLastCandidate = new Shared<int>(-1);
			CountdownEvent sharedBarrier = new CountdownEvent(partitionCount);
			PartitionedStream<TSource, int> partitionedStream2 = new PartitionedStream<TSource, int>(partitionCount, partitionedStream.KeyComparer, OrdinalIndexState.Shuffled);
			for (int i = 0; i < partitionCount; i++)
			{
				partitionedStream2[i] = new LastQueryOperatorEnumerator<TKey>(partitionedStream[i], m_predicate, sharedLastCandidate, sharedBarrier, settings.CancellationState.MergedCancellationToken);
			}
			recipient.Receive(partitionedStream2);
		}

		internal override IEnumerable<TSource> AsSequentialQuery(CancellationToken token)
		{
			throw new NotSupportedException();
		}
	}
}
