using System.Collections.Generic;
using System.Linq.Parallel;

namespace System.Linq
{
	/// <summary>
	/// Represents a sorted, parallel sequence.
	/// </summary>
	public class OrderedParallelQuery<TSource> : ParallelQuery<TSource>
	{
		private QueryOperator<TSource> m_sortOp;

		internal QueryOperator<TSource> SortOperator => m_sortOp;

		internal IOrderedEnumerable<TSource> OrderedEnumerable => (IOrderedEnumerable<TSource>)m_sortOp;

		internal OrderedParallelQuery(QueryOperator<TSource> sortOp)
			: base(sortOp.SpecifiedQuerySettings)
		{
			m_sortOp = sortOp;
		}

		/// <summary>
		/// Returns an enumerator that iterates through the sequence.
		/// </summary>
		/// <returns>An enumerator that iterates through the sequence.</returns>
		public override IEnumerator<TSource> GetEnumerator()
		{
			return m_sortOp.GetEnumerator();
		}
	}
}
