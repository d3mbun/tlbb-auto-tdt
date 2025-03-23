using System.Collections;
using System.Collections.Generic;

namespace System.Linq.Parallel
{
	/// <summary>
	/// A linked list of array chunks. Allows direct access to its arrays.
	/// </summary>
	/// <typeparam name="TInputOutput">The elements held within.</typeparam>
	internal class ListChunk<TInputOutput> : IEnumerable<TInputOutput>, IEnumerable
	{
		internal TInputOutput[] m_chunk;

		private int m_chunkCount;

		private ListChunk<TInputOutput> m_nextChunk;

		private ListChunk<TInputOutput> m_tailChunk;

		/// <summary>
		/// The next chunk in the linked chain.
		/// </summary>
		internal ListChunk<TInputOutput> Next => m_nextChunk;

		/// <summary>
		/// The number of elements contained within this particular chunk.
		/// </summary>
		internal int Count => m_chunkCount;

		/// <summary>
		/// Allocates a new root chunk of a particular size.
		/// </summary>
		internal ListChunk(int size)
		{
			m_chunk = new TInputOutput[size];
			m_chunkCount = 0;
			m_tailChunk = this;
		}

		/// <summary>
		/// Adds an element to this chunk.  Only ever called on the root.
		/// </summary>
		/// <param name="e">The new element.</param>
		internal void Add(TInputOutput e)
		{
			ListChunk<TInputOutput> listChunk = m_tailChunk;
			if (listChunk.m_chunkCount == listChunk.m_chunk.Length)
			{
				m_tailChunk = new ListChunk<TInputOutput>(listChunk.m_chunkCount * 2);
				listChunk = (listChunk.m_nextChunk = m_tailChunk);
			}
			listChunk.m_chunk[listChunk.m_chunkCount++] = e;
		}

		/// <summary>
		/// Fetches an enumerator to walk the elements in all chunks rooted from this one.
		/// </summary>
		public IEnumerator<TInputOutput> GetEnumerator()
		{
			for (ListChunk<TInputOutput> curr = this; curr != null; curr = curr.m_nextChunk)
			{
				for (int i = 0; i < curr.m_chunkCount; i++)
				{
					yield return curr.m_chunk[i];
				}
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<TInputOutput>)this).GetEnumerator();
		}
	}
}
