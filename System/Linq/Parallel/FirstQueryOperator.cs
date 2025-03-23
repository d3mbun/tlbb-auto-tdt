using System.Collections.Generic;
using System.Threading;

namespace System.Linq.Parallel
{
	/// <summary>
	/// First tries to discover the first element in the source, optionally matching a
	/// predicate.  All partitions search in parallel, publish the lowest index for a
	/// candidate match, and reach a barrier.  Only the partition that "wins" the race,
	/// i.e. who found the candidate with the smallest index, will yield an element.
	/// </summary>
	/// <typeparam name="TSource"></typeparam>
	internal sealed class FirstQueryOperator<TSource> : UnaryQueryOperator<TSource, TSource>
	{
		private class FirstQueryOperatorEnumerator : QueryOperatorEnumerator<TSource, int>
		{
			private QueryOperatorEnumerator<TSource, int> m_source;

			private Func<TSource, bool> m_predicate;

			private bool m_alreadySearched;

			private Shared<int> m_sharedFirstCandidate;

			private CountdownEvent m_sharedBarrier;

			private CancellationToken m_cancellationToken;

			internal FirstQueryOperatorEnumerator(QueryOperatorEnumerator<TSource, int> source, Func<TSource, bool> predicate, Shared<int> sharedFirstCandidate, CountdownEvent sharedBarrier, CancellationToken cancellationToken)
			{
				m_source = source;
				m_predicate = predicate;
				m_sharedFirstCandidate = sharedFirstCandidate;
				m_sharedBarrier = sharedBarrier;
				m_cancellationToken = cancellationToken;
			}

			internal override bool MoveNext(ref TSource currentElement, ref int currentKey)
			{
				if (m_alreadySearched)
				{
					return false;
				}
				TSource currentElement2 = default(TSource);
				int num = -1;
				try
				{
					int currentKey2 = 0;
					int num2 = 0;
					while (m_source.MoveNext(ref currentElement2, ref currentKey2))
					{
						if ((num2++ & 0x3F) == 0)
						{
							CancellationState.ThrowIfCanceled(m_cancellationToken);
						}
						if (m_predicate == null || m_predicate(currentElement2))
						{
							num = currentKey2;
							int value;
							do
							{
								value = m_sharedFirstCandidate.Value;
							}
							while ((value == -1 || num < value) && Interlocked.CompareExchange(ref m_sharedFirstCandidate.Value, num, value) != value);
							break;
						}
						if (m_sharedFirstCandidate.Value != -1 && currentKey2 > m_sharedFirstCandidate.Value)
						{
							break;
						}
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
					if (m_sharedFirstCandidate.Value == num)
					{
						currentElement = currentElement2;
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

		internal FirstQueryOperator(IEnumerable<TSource> child, Func<TSource, bool> predicate)
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
			_ = inputStream.OrdinalIndexState;
			int partitionCount = inputStream.PartitionCount;
			PartitionedStream<TSource, int> partitionedStream;
			if (m_prematureMergeNeeded)
			{
				ListQueryResults<TSource> listQueryResults = QueryOperator<TSource>.ExecuteAndCollectResults(inputStream, partitionCount, base.Child.OutputOrdered, preferStriping, settings);
				partitionedStream = listQueryResults.GetPartitionedStream();
			}
			else
			{
				partitionedStream = (PartitionedStream<TSource, int>)(object)inputStream;
			}
			Shared<int> sharedFirstCandidate = new Shared<int>(-1);
			CountdownEvent sharedBarrier = new CountdownEvent(partitionCount);
			PartitionedStream<TSource, int> partitionedStream2 = new PartitionedStream<TSource, int>(partitionCount, Util.GetDefaultComparer<int>(), OrdinalIndexState.Shuffled);
			for (int i = 0; i < partitionCount; i++)
			{
				partitionedStream2[i] = new FirstQueryOperatorEnumerator(partitionedStream[i], m_predicate, sharedFirstCandidate, sharedBarrier, settings.CancellationState.MergedCancellationToken);
			}
			recipient.Receive(partitionedStream2);
		}

		internal override IEnumerable<TSource> AsSequentialQuery(CancellationToken token)
		{
			throw new NotSupportedException();
		}
	}
}
