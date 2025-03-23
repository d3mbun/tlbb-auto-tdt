using System.Threading;

namespace System.Linq.Parallel
{
	/// <summary>
	/// A class with some shared implementation between all aggregation enumerators. 
	/// </summary>
	/// <typeparam name="TIntermediate"></typeparam>
	internal abstract class InlinedAggregationOperatorEnumerator<TIntermediate> : QueryOperatorEnumerator<TIntermediate, int>
	{
		private int m_partitionIndex;

		protected CancellationToken m_cancellationToken;

		internal InlinedAggregationOperatorEnumerator(int partitionIndex, CancellationToken cancellationToken)
		{
			m_partitionIndex = partitionIndex;
			m_cancellationToken = cancellationToken;
		}

		internal sealed override bool MoveNext(ref TIntermediate currentElement, ref int currentKey)
		{
			if (MoveNextCore(ref currentElement))
			{
				currentKey = m_partitionIndex;
				return true;
			}
			return false;
		}

		protected abstract bool MoveNextCore(ref TIntermediate currentElement);
	}
}
