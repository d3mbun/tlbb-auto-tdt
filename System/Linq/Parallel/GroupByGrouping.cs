using System.Collections;
using System.Collections.Generic;

namespace System.Linq.Parallel
{
	internal class GroupByGrouping<TGroupKey, TElement> : IGrouping<TGroupKey, TElement>, IEnumerable<TElement>, IEnumerable
	{
		private KeyValuePair<Wrapper<TGroupKey>, ListChunk<TElement>> m_keyValues;

		TGroupKey IGrouping<TGroupKey, TElement>.Key => m_keyValues.Key.Value;

		internal GroupByGrouping(TGroupKey key)
		{
			m_keyValues = new KeyValuePair<Wrapper<TGroupKey>, ListChunk<TElement>>(new Wrapper<TGroupKey>(key), new ListChunk<TElement>(2));
		}

		internal GroupByGrouping(KeyValuePair<Wrapper<TGroupKey>, ListChunk<TElement>> keyValues)
		{
			m_keyValues = keyValues;
		}

		internal void Add(TElement element)
		{
			m_keyValues.Value.Add(element);
		}

		IEnumerator<TElement> IEnumerable<TElement>.GetEnumerator()
		{
			return m_keyValues.Value.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<TElement>)this).GetEnumerator();
		}
	}
}
