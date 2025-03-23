using System.Collections;
using System.Collections.Generic;

namespace System.Linq.Parallel
{
	/// <summary>
	/// An ordered version of the grouping data structure. Represents an ordered group of elements that
	/// have the same grouping key.
	/// </summary>
	internal class OrderedGroupByGrouping<TGroupKey, TOrderKey, TElement> : IGrouping<TGroupKey, TElement>, IEnumerable<TElement>, IEnumerable
	{
		private TGroupKey m_groupKey;

		private GrowingArray<TElement> m_values;

		private GrowingArray<TOrderKey> m_orderKeys;

		private IComparer<TOrderKey> m_orderComparer;

		TGroupKey IGrouping<TGroupKey, TElement>.Key => m_groupKey;

		/// <summary>
		/// Constructs a new grouping
		/// </summary>
		internal OrderedGroupByGrouping(TGroupKey groupKey, IComparer<TOrderKey> orderComparer)
		{
			m_groupKey = groupKey;
			m_values = new GrowingArray<TElement>();
			m_orderKeys = new GrowingArray<TOrderKey>();
			m_orderComparer = orderComparer;
		}

		IEnumerator<TElement> IEnumerable<TElement>.GetEnumerator()
		{
			int valueCount = m_values.Count;
			TElement[] valueArray = m_values.InternalArray;
			for (int i = 0; i < valueCount; i++)
			{
				yield return valueArray[i];
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<TElement>)this).GetEnumerator();
		}

		/// <summary>
		/// Add an element
		/// </summary>
		internal void Add(TElement value, TOrderKey orderKey)
		{
			m_values.Add(value);
			m_orderKeys.Add(orderKey);
		}

		/// <summary>
		/// No more elements will be added, so we can sort the group now.
		/// </summary>
		internal void DoneAdding()
		{
			Array.Sort(m_orderKeys.InternalArray, m_values.InternalArray, 0, m_values.Count, m_orderComparer);
		}
	}
}
