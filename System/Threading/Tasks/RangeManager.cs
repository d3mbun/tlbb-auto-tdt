namespace System.Threading.Tasks
{
	/// <summary>
	/// Represents the entire loop operation, keeping track of workers and ranges.
	/// </summary>
	///
	/// The usage pattern is:
	///    1) The Parallel loop entry function (ForWorker) creates an instance of this class
	///    2) Every thread joining to service the parallel loop calls RegisterWorker to grab a 
	///       RangeWorker struct to wrap the state it will need to find and execute work, 
	///       and they keep interacting with that struct until the end of the loop
	internal class RangeManager
	{
		internal readonly IndexRange[] m_indexRanges;

		internal int m_nCurrentIndexRangeToAssign;

		internal long m_nStep;

		/// <summary>
		/// Initializes a RangeManager with the given loop parameters, and the desired number of outer ranges
		/// </summary>
		internal RangeManager(long nFromInclusive, long nToExclusive, long nStep, int nNumExpectedWorkers)
		{
			m_nCurrentIndexRangeToAssign = 0;
			m_nStep = nStep;
			if (nNumExpectedWorkers == 1)
			{
				nNumExpectedWorkers = 2;
			}
			ulong num = (ulong)(nToExclusive - nFromInclusive);
			ulong num2 = num / (ulong)nNumExpectedWorkers;
			num2 -= num2 % (ulong)nStep;
			if (num2 == 0)
			{
				num2 = (ulong)nStep;
			}
			int num3 = (int)(num / num2);
			if (num % num2 != 0)
			{
				num3++;
			}
			long num4 = (long)num2;
			m_indexRanges = new IndexRange[num3];
			long num5 = nFromInclusive;
			for (int i = 0; i < num3; i++)
			{
				m_indexRanges[i].m_nFromInclusive = num5;
				m_indexRanges[i].m_nSharedCurrentIndexOffset = null;
				m_indexRanges[i].m_bRangeFinished = 0;
				num5 += num4;
				if (num5 < num5 - num4 || num5 > nToExclusive)
				{
					num5 = nToExclusive;
				}
				m_indexRanges[i].m_nToExclusive = num5;
			}
		}

		/// <summary>
		/// The function that needs to be called by each new worker thread servicing the parallel loop
		/// in order to get a RangeWorker struct that wraps the state for finding and executing indices
		/// </summary>
		internal RangeWorker RegisterNewWorker()
		{
			int nInitialRange = (Interlocked.Increment(ref m_nCurrentIndexRangeToAssign) - 1) % m_indexRanges.Length;
			return new RangeWorker(m_indexRanges, nInitialRange, m_nStep);
		}
	}
}
