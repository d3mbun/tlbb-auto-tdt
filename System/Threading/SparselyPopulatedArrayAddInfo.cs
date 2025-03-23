namespace System.Threading
{
	/// <summary>
	/// A struct to hold a link to the exact spot in an array an element was inserted, enabling
	/// constant time removal later on.
	/// </summary>
	internal struct SparselyPopulatedArrayAddInfo<T> where T : class
	{
		private SparselyPopulatedArrayFragment<T> m_source;

		private int m_index;

		internal SparselyPopulatedArrayFragment<T> Source => m_source;

		internal int Index => m_index;

		internal SparselyPopulatedArrayAddInfo(SparselyPopulatedArrayFragment<T> source, int index)
		{
			m_source = source;
			m_index = index;
		}
	}
}
