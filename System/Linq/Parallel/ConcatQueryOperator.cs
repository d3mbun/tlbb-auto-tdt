using System.Collections.Generic;
using System.Threading;

namespace System.Linq.Parallel
{
	/// <summary>
	/// Concatenates one data source with another.  Order preservation is used to ensure
	/// the output is actually a concatenation -- i.e. one after the other.  The only
	/// special synchronization required is to find the largest index N in the first data
	/// source so that the indices of elements in the second data source can be offset
	/// by adding N+1.  This makes it appear to the order preservation infrastructure as
	/// though all elements in the second came after all elements in the first, which is
	/// precisely what we want.
	/// </summary>
	/// <typeparam name="TSource"></typeparam>
	internal sealed class ConcatQueryOperator<TSource> : BinaryQueryOperator<TSource, TSource, TSource>
	{
		private class ConcatQueryOperatorEnumerator<TLeftKey, TRightKey> : QueryOperatorEnumerator<TSource, ConcatKey<TLeftKey, TRightKey>>
		{
			private QueryOperatorEnumerator<TSource, TLeftKey> m_firstSource;

			private QueryOperatorEnumerator<TSource, TRightKey> m_secondSource;

			private bool m_begunSecond;

			internal ConcatQueryOperatorEnumerator(QueryOperatorEnumerator<TSource, TLeftKey> firstSource, QueryOperatorEnumerator<TSource, TRightKey> secondSource)
			{
				m_firstSource = firstSource;
				m_secondSource = secondSource;
			}

			internal override bool MoveNext(ref TSource currentElement, ref ConcatKey<TLeftKey, TRightKey> currentKey)
			{
				if (!m_begunSecond)
				{
					TLeftKey currentKey2 = default(TLeftKey);
					if (m_firstSource.MoveNext(ref currentElement, ref currentKey2))
					{
						currentKey = ConcatKey<TLeftKey, TRightKey>.MakeLeft(currentKey2);
						return true;
					}
					m_begunSecond = true;
				}
				TRightKey currentKey3 = default(TRightKey);
				if (m_secondSource.MoveNext(ref currentElement, ref currentKey3))
				{
					currentKey = ConcatKey<TLeftKey, TRightKey>.MakeRight(currentKey3);
					return true;
				}
				return false;
			}

			protected override void Dispose(bool disposing)
			{
				m_firstSource.Dispose();
				m_secondSource.Dispose();
			}
		}

		private class ConcatQueryOperatorResults : BinaryQueryOperatorResults
		{
			private ConcatQueryOperator<TSource> m_concatOp;

			private int m_leftChildCount;

			private int m_rightChildCount;

			internal override bool IsIndexible => true;

			internal override int ElementsCount => m_leftChildCount + m_rightChildCount;

			public static QueryResults<TSource> NewResults(QueryResults<TSource> leftChildQueryResults, QueryResults<TSource> rightChildQueryResults, ConcatQueryOperator<TSource> op, QuerySettings settings, bool preferStriping)
			{
				if (leftChildQueryResults.IsIndexible && rightChildQueryResults.IsIndexible)
				{
					return new ConcatQueryOperatorResults(leftChildQueryResults, rightChildQueryResults, op, settings, preferStriping);
				}
				return new BinaryQueryOperatorResults(leftChildQueryResults, rightChildQueryResults, op, settings, preferStriping);
			}

			private ConcatQueryOperatorResults(QueryResults<TSource> leftChildQueryResults, QueryResults<TSource> rightChildQueryResults, ConcatQueryOperator<TSource> concatOp, QuerySettings settings, bool preferStriping)
				: base(leftChildQueryResults, rightChildQueryResults, (BinaryQueryOperator<TSource, TSource, TSource>)concatOp, settings, preferStriping)
			{
				m_concatOp = concatOp;
				m_leftChildCount = leftChildQueryResults.ElementsCount;
				m_rightChildCount = rightChildQueryResults.ElementsCount;
			}

			internal override TSource GetElement(int index)
			{
				if (index < m_leftChildCount)
				{
					return m_leftChildQueryResults.GetElement(index);
				}
				return m_rightChildQueryResults.GetElement(index - m_leftChildCount);
			}
		}

		private readonly bool m_prematureMergeLeft;

		private readonly bool m_prematureMergeRight;

		internal override bool LimitsParallelism
		{
			get
			{
				if (!m_prematureMergeLeft)
				{
					return m_prematureMergeLeft;
				}
				return true;
			}
		}

		internal ConcatQueryOperator(ParallelQuery<TSource> firstChild, ParallelQuery<TSource> secondChild)
			: base(firstChild, secondChild)
		{
			m_outputOrdered = base.LeftChild.OutputOrdered || base.RightChild.OutputOrdered;
			m_prematureMergeLeft = base.LeftChild.OrdinalIndexState.IsWorseThan(OrdinalIndexState.Increasing);
			m_prematureMergeRight = base.RightChild.OrdinalIndexState.IsWorseThan(OrdinalIndexState.Increasing);
			if (base.LeftChild.OrdinalIndexState == OrdinalIndexState.Indexible && base.RightChild.OrdinalIndexState == OrdinalIndexState.Indexible)
			{
				SetOrdinalIndex(OrdinalIndexState.Indexible);
			}
			else
			{
				SetOrdinalIndex(OrdinalIndexState.Shuffled);
			}
		}

		internal override QueryResults<TSource> Open(QuerySettings settings, bool preferStriping)
		{
			QueryResults<TSource> leftChildQueryResults = base.LeftChild.Open(settings, preferStriping);
			QueryResults<TSource> rightChildQueryResults = base.RightChild.Open(settings, preferStriping);
			return ConcatQueryOperatorResults.NewResults(leftChildQueryResults, rightChildQueryResults, this, settings, preferStriping);
		}

		public override void WrapPartitionedStream<TLeftKey, TRightKey>(PartitionedStream<TSource, TLeftKey> leftStream, PartitionedStream<TSource, TRightKey> rightStream, IPartitionedStreamRecipient<TSource> outputRecipient, bool preferStriping, QuerySettings settings)
		{
			_ = leftStream.OrdinalIndexState;
			int partitionCount = leftStream.PartitionCount;
			PartitionedStream<TSource, int> partitionedStream;
			if (m_prematureMergeLeft)
			{
				ListQueryResults<TSource> listQueryResults = QueryOperator<TSource>.ExecuteAndCollectResults(leftStream, partitionCount, base.LeftChild.OutputOrdered, preferStriping, settings);
				partitionedStream = listQueryResults.GetPartitionedStream();
			}
			else
			{
				partitionedStream = (PartitionedStream<TSource, int>)(object)leftStream;
			}
			PartitionedStream<TSource, int> partitionedStream2;
			if (m_prematureMergeRight)
			{
				ListQueryResults<TSource> listQueryResults2 = QueryOperator<TSource>.ExecuteAndCollectResults(rightStream, partitionCount, base.LeftChild.OutputOrdered, preferStriping, settings);
				partitionedStream2 = listQueryResults2.GetPartitionedStream();
			}
			else
			{
				partitionedStream2 = (PartitionedStream<TSource, int>)(object)rightStream;
			}
			IComparer<ConcatKey<int, int>> keyComparer = ConcatKey<int, int>.MakeComparer(partitionedStream.KeyComparer, partitionedStream2.KeyComparer);
			PartitionedStream<TSource, ConcatKey<int, int>> partitionedStream3 = new PartitionedStream<TSource, ConcatKey<int, int>>(partitionCount, keyComparer, OrdinalIndexState);
			for (int i = 0; i < partitionCount; i++)
			{
				partitionedStream3[i] = new ConcatQueryOperatorEnumerator<int, int>(partitionedStream[i], partitionedStream2[i]);
			}
			outputRecipient.Receive(partitionedStream3);
		}

		internal override IEnumerable<TSource> AsSequentialQuery(CancellationToken token)
		{
			return base.LeftChild.AsSequentialQuery(token).Concat(base.RightChild.AsSequentialQuery(token));
		}
	}
}
