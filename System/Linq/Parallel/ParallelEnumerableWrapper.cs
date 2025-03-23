using System.Collections;
using System.Collections.Generic;

namespace System.Linq.Parallel
{
	/// <summary>
	/// A simple implementation of the ParallelQuery{object} interface which wraps an
	/// underlying IEnumerable, such that it can be used in parallel queries.
	/// </summary>
	internal class ParallelEnumerableWrapper : ParallelQuery<object>
	{
		private readonly IEnumerable m_source;

		internal ParallelEnumerableWrapper(IEnumerable source)
			: base(QuerySettings.Empty)
		{
			m_source = source;
		}

		internal override IEnumerator GetEnumeratorUntyped()
		{
			return m_source.GetEnumerator();
		}

		public override IEnumerator<object> GetEnumerator()
		{
			return new EnumerableWrapperWeakToStrong(m_source).GetEnumerator();
		}
	}
	/// <summary>
	/// A simple implementation of the ParallelQuery{T} interface which wraps an
	/// underlying IEnumerable{T}, such that it can be used in parallel queries.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	internal class ParallelEnumerableWrapper<T> : ParallelQuery<T>
	{
		private readonly IEnumerable<T> m_wrappedEnumerable;

		internal IEnumerable<T> WrappedEnumerable => m_wrappedEnumerable;

		internal ParallelEnumerableWrapper(IEnumerable<T> wrappedEnumerable)
			: base(QuerySettings.Empty)
		{
			m_wrappedEnumerable = wrappedEnumerable;
		}

		public override IEnumerator<T> GetEnumerator()
		{
			return m_wrappedEnumerable.GetEnumerator();
		}
	}
}
