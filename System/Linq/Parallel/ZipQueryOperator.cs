using System.Collections.Generic;
using System.Threading;

namespace System.Linq.Parallel
{
	/// <summary>
	/// A Zip operator combines two input data sources into a single output stream,
	/// using a pairwise element matching algorithm. For example, the result of zipping
	/// two vectors a = {0, 1, 2, 3} and b = {9, 8, 7, 6} is the vector of pairs,
	/// c = {(0,9), (1,8), (2,7), (3,6)}. Because the expectation is that each element
	/// is matched with the element in the other data source at the same ordinal
	/// position, the zip operator requires order preservation. 
	/// </summary>
	/// <typeparam name="TLeftInput"></typeparam>
	/// <typeparam name="TRightInput"></typeparam>
	/// <typeparam name="TOutput"></typeparam>
	internal sealed class ZipQueryOperator<TLeftInput, TRightInput, TOutput> : QueryOperator<TOutput>
	{
		internal class ZipQueryOperatorResults : QueryResults<TOutput>
		{
			private readonly QueryResults<TLeftInput> m_leftChildResults;

			private readonly QueryResults<TRightInput> m_rightChildResults;

			private readonly Func<TLeftInput, TRightInput, TOutput> m_resultSelector;

			private readonly int m_count;

			private readonly int m_partitionCount;

			private readonly bool m_preferStriping;

			internal override int ElementsCount => m_count;

			internal override bool IsIndexible => true;

			internal ZipQueryOperatorResults(QueryResults<TLeftInput> leftChildResults, QueryResults<TRightInput> rightChildResults, Func<TLeftInput, TRightInput, TOutput> resultSelector, int partitionCount, bool preferStriping)
			{
				m_leftChildResults = leftChildResults;
				m_rightChildResults = rightChildResults;
				m_resultSelector = resultSelector;
				m_partitionCount = partitionCount;
				m_preferStriping = preferStriping;
				m_count = Math.Min(m_leftChildResults.Count, m_rightChildResults.Count);
			}

			internal override TOutput GetElement(int index)
			{
				return m_resultSelector(m_leftChildResults.GetElement(index), m_rightChildResults.GetElement(index));
			}

			internal override void GivePartitionedStream(IPartitionedStreamRecipient<TOutput> recipient)
			{
				PartitionedStream<TOutput, int> partitionedStream = ExchangeUtilities.PartitionDataSource(this, m_partitionCount, m_preferStriping);
				recipient.Receive(partitionedStream);
			}
		}

		private readonly Func<TLeftInput, TRightInput, TOutput> m_resultSelector;

		private readonly QueryOperator<TLeftInput> m_leftChild;

		private readonly QueryOperator<TRightInput> m_rightChild;

		private readonly bool m_prematureMergeLeft;

		private readonly bool m_prematureMergeRight;

		internal override OrdinalIndexState OrdinalIndexState => OrdinalIndexState.Indexible;

		internal override bool LimitsParallelism
		{
			get
			{
				if (!m_prematureMergeLeft)
				{
					return m_prematureMergeRight;
				}
				return true;
			}
		}

		internal ZipQueryOperator(ParallelQuery<TLeftInput> leftChildSource, IEnumerable<TRightInput> rightChildSource, Func<TLeftInput, TRightInput, TOutput> resultSelector)
			: this(QueryOperator<TLeftInput>.AsQueryOperator(leftChildSource), QueryOperator<TRightInput>.AsQueryOperator(rightChildSource), resultSelector)
		{
		}

		private ZipQueryOperator(QueryOperator<TLeftInput> left, QueryOperator<TRightInput> right, Func<TLeftInput, TRightInput, TOutput> resultSelector)
			: base(left.SpecifiedQuerySettings.Merge(right.SpecifiedQuerySettings))
		{
			m_leftChild = left;
			m_rightChild = right;
			m_resultSelector = resultSelector;
			m_outputOrdered = m_leftChild.OutputOrdered || m_rightChild.OutputOrdered;
			m_prematureMergeLeft = m_leftChild.OrdinalIndexState != OrdinalIndexState.Indexible;
			m_prematureMergeRight = m_rightChild.OrdinalIndexState != OrdinalIndexState.Indexible;
		}

		internal override QueryResults<TOutput> Open(QuerySettings settings, bool preferStriping)
		{
			QueryResults<TLeftInput> queryResults = m_leftChild.Open(settings, preferStriping);
			QueryResults<TRightInput> queryResults2 = m_rightChild.Open(settings, preferStriping);
			int value = settings.DegreeOfParallelism.Value;
			if (m_prematureMergeLeft)
			{
				PartitionedStreamMerger<TLeftInput> partitionedStreamMerger = new PartitionedStreamMerger<TLeftInput>(forEffectMerge: false, ParallelMergeOptions.FullyBuffered, settings.TaskScheduler, m_leftChild.OutputOrdered, settings.CancellationState, settings.QueryId);
				queryResults.GivePartitionedStream(partitionedStreamMerger);
				queryResults = new ListQueryResults<TLeftInput>(partitionedStreamMerger.MergeExecutor.GetResultsAsArray(), value, preferStriping);
			}
			if (m_prematureMergeRight)
			{
				PartitionedStreamMerger<TRightInput> partitionedStreamMerger2 = new PartitionedStreamMerger<TRightInput>(forEffectMerge: false, ParallelMergeOptions.FullyBuffered, settings.TaskScheduler, m_rightChild.OutputOrdered, settings.CancellationState, settings.QueryId);
				queryResults2.GivePartitionedStream(partitionedStreamMerger2);
				queryResults2 = new ListQueryResults<TRightInput>(partitionedStreamMerger2.MergeExecutor.GetResultsAsArray(), value, preferStriping);
			}
			return new ZipQueryOperatorResults(queryResults, queryResults2, m_resultSelector, value, preferStriping);
		}

		internal override IEnumerable<TOutput> AsSequentialQuery(CancellationToken token)
		{
			using (IEnumerator<TLeftInput> leftEnumerator = m_leftChild.AsSequentialQuery(token).GetEnumerator())
			{
				using (IEnumerator<TRightInput> rightEnumerator = m_rightChild.AsSequentialQuery(token).GetEnumerator())
				{
					while (leftEnumerator.MoveNext() && rightEnumerator.MoveNext())
					{
						yield return m_resultSelector(leftEnumerator.Current, rightEnumerator.Current);
					}
				}
			}
		}
	}
}
