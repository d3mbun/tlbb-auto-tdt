using System.Collections.Generic;
using System.Threading;

namespace System.Linq.Parallel
{
	/// <summary>
	/// A join operator takes a left query tree and a right query tree, and then yields the
	/// matching pairs between the two. LINQ supports equi-key-based joins. Hence, a key-
	/// selection function for the left and right data types will yield keys of the same
	/// type for both. We then merely have to match elements from the left with elements from
	/// the right that have the same exact key. Note that this is an inner join. In other
	/// words, outer elements with no matching inner elements do not appear in the output.
	///
	/// @TODO: @BUG#528: Currently we implement only a hash-join algorithm. Furthermore, we always
	///     choose the inner data source for the hash-table creation. There is room for
	///     optimization and different algorithm choices eventually.
	///
	/// Hash-joins work in two phases:
	///
	///    (1) Building - we build a hash-table from one of the data sources. In the case
	///            of this specific operator, the table is built from the hash-codes of
	///            keys selected via the key selector function. Because elements may share
	///            the same key, the table must support one-key-to-many-values.
	///    (2) Probing - for each element in the data source not used for building, we
	///            use its key to look into the hash-table. If we find elements under this
	///            key, we just enumerate all of them, yielding them as join matches.
	///
	/// Because hash-tables exhibit on average O(1) lookup, we turn what would have been
	/// an O(n*m) algorithm -- in the case of nested loops joins -- into an O(n) algorithm.
	/// We of course require some additional storage to do so, but in general this pays.
	/// </summary>
	/// <typeparam name="TLeftInput"></typeparam>
	/// <typeparam name="TRightInput"></typeparam>
	/// <typeparam name="TKey"></typeparam>
	/// <typeparam name="TOutput"></typeparam>
	internal sealed class JoinQueryOperator<TLeftInput, TRightInput, TKey, TOutput> : BinaryQueryOperator<TLeftInput, TRightInput, TOutput>
	{
		private readonly Func<TLeftInput, TKey> m_leftKeySelector;

		private readonly Func<TRightInput, TKey> m_rightKeySelector;

		private readonly Func<TLeftInput, TRightInput, TOutput> m_resultSelector;

		private readonly IEqualityComparer<TKey> m_keyComparer;

		internal override bool LimitsParallelism => false;

		internal JoinQueryOperator(ParallelQuery<TLeftInput> left, ParallelQuery<TRightInput> right, Func<TLeftInput, TKey> leftKeySelector, Func<TRightInput, TKey> rightKeySelector, Func<TLeftInput, TRightInput, TOutput> resultSelector, IEqualityComparer<TKey> keyComparer)
			: base(left, right)
		{
			m_leftKeySelector = leftKeySelector;
			m_rightKeySelector = rightKeySelector;
			m_resultSelector = resultSelector;
			m_keyComparer = keyComparer;
			m_outputOrdered = base.LeftChild.OutputOrdered;
			SetOrdinalIndex(OrdinalIndexState.Shuffled);
		}

		public override void WrapPartitionedStream<TLeftKey, TRightKey>(PartitionedStream<TLeftInput, TLeftKey> leftStream, PartitionedStream<TRightInput, TRightKey> rightStream, IPartitionedStreamRecipient<TOutput> outputRecipient, bool preferStriping, QuerySettings settings)
		{
			if (base.LeftChild.OutputOrdered)
			{
				WrapPartitionedStreamHelper(ExchangeUtilities.HashRepartitionOrdered(leftStream, m_leftKeySelector, m_keyComparer, null, settings.CancellationState.MergedCancellationToken), rightStream, outputRecipient, settings.CancellationState.MergedCancellationToken);
			}
			else
			{
				WrapPartitionedStreamHelper(ExchangeUtilities.HashRepartition(leftStream, m_leftKeySelector, m_keyComparer, null, settings.CancellationState.MergedCancellationToken), rightStream, outputRecipient, settings.CancellationState.MergedCancellationToken);
			}
		}

		private void WrapPartitionedStreamHelper<TLeftKey, TRightKey>(PartitionedStream<Pair<TLeftInput, TKey>, TLeftKey> leftHashStream, PartitionedStream<TRightInput, TRightKey> rightPartitionedStream, IPartitionedStreamRecipient<TOutput> outputRecipient, CancellationToken cancellationToken)
		{
			int partitionCount = leftHashStream.PartitionCount;
			PartitionedStream<Pair<TRightInput, TKey>, int> partitionedStream = ExchangeUtilities.HashRepartition(rightPartitionedStream, m_rightKeySelector, m_keyComparer, null, cancellationToken);
			PartitionedStream<TOutput, TLeftKey> partitionedStream2 = new PartitionedStream<TOutput, TLeftKey>(partitionCount, leftHashStream.KeyComparer, OrdinalIndexState);
			for (int i = 0; i < partitionCount; i++)
			{
				partitionedStream2[i] = new HashJoinQueryOperatorEnumerator<TLeftInput, TLeftKey, TRightInput, TKey, TOutput>(leftHashStream[i], partitionedStream[i], m_resultSelector, null, m_keyComparer, cancellationToken);
			}
			outputRecipient.Receive(partitionedStream2);
		}

		internal override QueryResults<TOutput> Open(QuerySettings settings, bool preferStriping)
		{
			QueryResults<TLeftInput> leftChildQueryResults = base.LeftChild.Open(settings, preferStriping: false);
			QueryResults<TRightInput> rightChildQueryResults = base.RightChild.Open(settings, preferStriping: false);
			return new BinaryQueryOperatorResults(leftChildQueryResults, rightChildQueryResults, this, settings, preferStriping: false);
		}

		internal override IEnumerable<TOutput> AsSequentialQuery(CancellationToken token)
		{
			IEnumerable<TLeftInput> outer = CancellableEnumerable.Wrap(base.LeftChild.AsSequentialQuery(token), token);
			IEnumerable<TRightInput> inner = CancellableEnumerable.Wrap(base.RightChild.AsSequentialQuery(token), token);
			return outer.Join(inner, m_leftKeySelector, m_rightKeySelector, m_resultSelector, m_keyComparer);
		}
	}
}
