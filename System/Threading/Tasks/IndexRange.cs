namespace System.Threading.Tasks
{
	/// <summary>
	/// Represents an index range
	/// </summary>
	internal struct IndexRange
	{
		internal long m_nFromInclusive;

		internal long m_nToExclusive;

		internal Shared<long> m_nSharedCurrentIndexOffset;

		internal int m_bRangeFinished;
	}
}
