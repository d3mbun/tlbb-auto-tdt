namespace System.Threading
{
	/// <summary>
	/// A fragment of a sparsely populated array, doubly linked.
	/// </summary>
	/// <typeparam name="T">The kind of elements contained within.</typeparam>
	internal class SparselyPopulatedArrayFragment<T> where T : class
	{
		internal readonly T[] m_elements;

		internal volatile int m_freeCount;

		internal volatile SparselyPopulatedArrayFragment<T> m_next;

		internal volatile SparselyPopulatedArrayFragment<T> m_prev;

		internal T this[int index] => m_elements[index];

		internal int Length => m_elements.Length;

		internal SparselyPopulatedArrayFragment<T> Next => m_next;

		internal SparselyPopulatedArrayFragment<T> Prev => m_prev;

		internal SparselyPopulatedArrayFragment(int size)
			: this(size, (SparselyPopulatedArrayFragment<T>)null)
		{
		}

		internal SparselyPopulatedArrayFragment(int size, SparselyPopulatedArrayFragment<T> prev)
		{
			m_elements = new T[size];
			m_freeCount = size;
			m_prev = prev;
		}

		internal T SafeAtomicRemove(int index, T expectedElement)
		{
			T val = Interlocked.CompareExchange(ref m_elements[index], null, expectedElement);
			if (val != null)
			{
				m_freeCount++;
			}
			return val;
		}
	}
}
