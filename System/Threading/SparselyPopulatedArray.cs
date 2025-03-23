namespace System.Threading
{
	/// <summary>
	/// A sparsely populated array.  Elements can be sparse and some null, but this allows for
	/// lock-free additions and growth, and also for constant time removal (by nulling out).
	/// </summary>
	/// <typeparam name="T">The kind of elements contained within.</typeparam>
	internal class SparselyPopulatedArray<T> where T : class
	{
		private readonly SparselyPopulatedArrayFragment<T> m_head;

		private volatile SparselyPopulatedArrayFragment<T> m_tail;

		/// <summary>
		/// The head of the doubly linked list.
		/// </summary>
		internal SparselyPopulatedArrayFragment<T> Head => m_head;

		/// <summary>
		/// The tail of the doubly linked list.
		/// </summary>
		internal SparselyPopulatedArrayFragment<T> Tail => m_tail;

		/// <summary>
		/// Allocates a new array with the given initial size.
		/// </summary>
		/// <param name="initialSize">How many array slots to pre-allocate.</param>
		internal SparselyPopulatedArray(int initialSize)
		{
			m_head = (m_tail = new SparselyPopulatedArrayFragment<T>(initialSize));
		}

		/// <summary>
		/// Adds an element in the first available slot, beginning the search from the tail-to-head.
		/// If no slots are available, the array is grown.  The method doesn't return until successful.
		/// </summary>
		/// <param name="element">The element to add.</param>
		/// <returns>Information about where the add happened, to enable O(1) deregistration.</returns>
		internal SparselyPopulatedArrayAddInfo<T> Add(T element)
		{
			while (true)
			{
				SparselyPopulatedArrayFragment<T> sparselyPopulatedArrayFragment = m_tail;
				while (sparselyPopulatedArrayFragment.m_next != null)
				{
					sparselyPopulatedArrayFragment = (m_tail = sparselyPopulatedArrayFragment.m_next);
				}
				for (SparselyPopulatedArrayFragment<T> sparselyPopulatedArrayFragment2 = sparselyPopulatedArrayFragment; sparselyPopulatedArrayFragment2 != null; sparselyPopulatedArrayFragment2 = sparselyPopulatedArrayFragment2.m_prev)
				{
					if (sparselyPopulatedArrayFragment2.m_freeCount < 1)
					{
						sparselyPopulatedArrayFragment2.m_freeCount--;
					}
					if (sparselyPopulatedArrayFragment2.m_freeCount > 0 || sparselyPopulatedArrayFragment2.m_freeCount < -10)
					{
						int length = sparselyPopulatedArrayFragment2.Length;
						int num = (length - sparselyPopulatedArrayFragment2.m_freeCount) % length;
						if (num < 0)
						{
							num = 0;
							sparselyPopulatedArrayFragment2.m_freeCount--;
						}
						for (int i = 0; i < length; i++)
						{
							int num2 = (num + i) % length;
							if (sparselyPopulatedArrayFragment2.m_elements[num2] == null && Interlocked.CompareExchange(ref sparselyPopulatedArrayFragment2.m_elements[num2], element, null) == null)
							{
								int num3 = sparselyPopulatedArrayFragment2.m_freeCount - 1;
								sparselyPopulatedArrayFragment2.m_freeCount = ((num3 > 0) ? num3 : 0);
								return new SparselyPopulatedArrayAddInfo<T>(sparselyPopulatedArrayFragment2, num2);
							}
						}
					}
				}
				SparselyPopulatedArrayFragment<T> sparselyPopulatedArrayFragment3 = new SparselyPopulatedArrayFragment<T>((sparselyPopulatedArrayFragment.m_elements.Length == 4096) ? 4096 : (sparselyPopulatedArrayFragment.m_elements.Length * 2), sparselyPopulatedArrayFragment);
				if (Interlocked.CompareExchange(ref sparselyPopulatedArrayFragment.m_next, sparselyPopulatedArrayFragment3, null) == null)
				{
					m_tail = sparselyPopulatedArrayFragment3;
				}
			}
		}
	}
}
