namespace System.Threading.Tasks
{
	/// <summary>
	/// The RangeWorker struct wraps the state needed by a task that services the parallel loop
	/// </summary>
	internal struct RangeWorker
	{
		internal readonly IndexRange[] m_indexRanges;

		internal int m_nCurrentIndexRange;

		internal long m_nStep;

		internal long m_nIncrementValue;

		internal readonly long m_nMaxIncrementValue;

		/// <summary>
		/// Initializes a RangeWorker struct
		/// </summary>
		internal RangeWorker(IndexRange[] ranges, int nInitialRange, long nStep)
		{
			m_indexRanges = ranges;
			m_nCurrentIndexRange = nInitialRange;
			m_nStep = nStep;
			m_nIncrementValue = nStep;
			m_nMaxIncrementValue = 16 * nStep;
		}

		/// <summary>
		/// Implements the core work search algorithm that will be used for this range worker. 
		/// </summary> 
		///
		/// Usage pattern is:
		///    1) the thread associated with this rangeworker calls FindNewWork
		///    2) if we return true, the worker uses the nFromInclusiveLocal and nToExclusiveLocal values
		///       to execute the sequential loop
		///    3) if we return false it means there is no more work left. It's time to quit.        
		internal bool FindNewWork(out long nFromInclusiveLocal, out long nToExclusiveLocal)
		{
			int num = m_indexRanges.Length;
			do
			{
				IndexRange indexRange = m_indexRanges[m_nCurrentIndexRange];
				if (indexRange.m_bRangeFinished == 0)
				{
					if (m_indexRanges[m_nCurrentIndexRange].m_nSharedCurrentIndexOffset == null)
					{
						Interlocked.CompareExchange(ref m_indexRanges[m_nCurrentIndexRange].m_nSharedCurrentIndexOffset, new Shared<long>(0L), null);
					}
					long num2 = Interlocked.Add(ref m_indexRanges[m_nCurrentIndexRange].m_nSharedCurrentIndexOffset.Value, m_nIncrementValue) - m_nIncrementValue;
					if (indexRange.m_nToExclusive - indexRange.m_nFromInclusive > num2)
					{
						nFromInclusiveLocal = indexRange.m_nFromInclusive + num2;
						nToExclusiveLocal = nFromInclusiveLocal + m_nIncrementValue;
						if (nToExclusiveLocal > indexRange.m_nToExclusive || nToExclusiveLocal < indexRange.m_nFromInclusive)
						{
							nToExclusiveLocal = indexRange.m_nToExclusive;
						}
						if (m_nIncrementValue < m_nMaxIncrementValue)
						{
							m_nIncrementValue *= 2L;
							if (m_nIncrementValue > m_nMaxIncrementValue)
							{
								m_nIncrementValue = m_nMaxIncrementValue;
							}
						}
						return true;
					}
					Interlocked.Exchange(ref m_indexRanges[m_nCurrentIndexRange].m_bRangeFinished, 1);
				}
				m_nCurrentIndexRange = (m_nCurrentIndexRange + 1) % m_indexRanges.Length;
				num--;
			}
			while (num > 0);
			nFromInclusiveLocal = 0L;
			nToExclusiveLocal = 0L;
			return false;
		}

		/// <summary>
		/// 32 bit integer version of FindNewWork. Assumes the ranges were initialized with 32 bit values.
		/// </summary> 
		internal bool FindNewWork32(out int nFromInclusiveLocal32, out int nToExclusiveLocal32)
		{
			long nFromInclusiveLocal33;
			long nToExclusiveLocal33;
			bool result = FindNewWork(out nFromInclusiveLocal33, out nToExclusiveLocal33);
			nFromInclusiveLocal32 = (int)nFromInclusiveLocal33;
			nToExclusiveLocal32 = (int)nToExclusiveLocal33;
			return result;
		}
	}
}
