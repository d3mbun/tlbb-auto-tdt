using System.Collections;
using System.Collections.Generic;
using System.Linq.Parallel;

namespace System.Linq
{
	/// <summary>
	/// Represents a parallel sequence.
	/// </summary>
	public class ParallelQuery : IEnumerable
	{
		private QuerySettings m_specifiedSettings;

		internal QuerySettings SpecifiedQuerySettings => m_specifiedSettings;

		internal ParallelQuery(QuerySettings specifiedSettings)
		{
			m_specifiedSettings = specifiedSettings;
		}

		internal virtual ParallelQuery<TCastTo> Cast<TCastTo>()
		{
			throw new NotSupportedException();
		}

		internal virtual ParallelQuery<TCastTo> OfType<TCastTo>()
		{
			throw new NotSupportedException();
		}

		internal virtual IEnumerator GetEnumeratorUntyped()
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Returns an enumerator that iterates through the sequence.
		/// </summary>
		/// <returns>An enumerator that iterates through the sequence.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumeratorUntyped();
		}
	}
	/// <summary>
	/// Represents a parallel sequence.
	/// </summary>
	public class ParallelQuery<TSource> : ParallelQuery, IEnumerable<TSource>, IEnumerable
	{
		internal ParallelQuery(QuerySettings settings)
			: base(settings)
		{
		}

		internal sealed override ParallelQuery<TCastTo> Cast<TCastTo>()
		{
			return this.Select((TSource elem) => (TCastTo)(object)elem);
		}

		internal sealed override ParallelQuery<TCastTo> OfType<TCastTo>()
		{
			return from elem in this
				where elem is TCastTo
				select (TCastTo)(object)elem;
		}

		internal override IEnumerator GetEnumeratorUntyped()
		{
			return ((IEnumerable<TSource>)this).GetEnumerator();
		}

		/// <summary>
		/// Returns an enumerator that iterates through the sequence.
		/// </summary>
		/// <returns>An enumerator that iterates through the sequence.</returns>
		public virtual IEnumerator<TSource> GetEnumerator()
		{
			throw new NotSupportedException();
		}
	}
}
