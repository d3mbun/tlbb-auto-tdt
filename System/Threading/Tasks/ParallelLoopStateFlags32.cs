namespace System.Threading.Tasks
{
	/// <summary>
	/// An internal class used to share accounting information in 32-bit versions
	/// of For()/ForEach() loops.
	/// </summary>
	internal class ParallelLoopStateFlags32 : ParallelLoopStateFlags
	{
		internal volatile int m_lowestBreakIteration = int.MaxValue;

		internal int LowestBreakIteration => m_lowestBreakIteration;

		internal long? NullableLowestBreakIteration
		{
			get
			{
				if (m_lowestBreakIteration == int.MaxValue)
				{
					return null;
				}
				long location = m_lowestBreakIteration;
				if (IntPtr.Size >= 8)
				{
					return location;
				}
				return Interlocked.Read(ref location);
			}
		}

		/// <summary>
		/// Lets the caller know whether or not to prematurely exit the For/ForEach loop.
		/// If this returns true, then exit the loop.  Otherwise, keep going.
		/// </summary>
		/// <param name="CallerIteration">The caller's current iteration point
		/// in the loop.</param>
		/// <remarks>
		/// The loop should exit on any one of the following conditions:
		///   (1) Stop() has been called by one or more tasks.
		///   (2) An exception has been raised by one or more tasks.
		///   (3) Break() has been called by one or more tasks, and
		///       CallerIteration exceeds the (lowest) iteration at which 
		///       Break() was called.
		///   (4) The loop was canceled.
		/// </remarks>
		internal bool ShouldExitLoop(int CallerIteration)
		{
			int loopStateFlags = base.LoopStateFlags;
			if (loopStateFlags != ParallelLoopStateFlags.PLS_NONE)
			{
				if ((loopStateFlags & (ParallelLoopStateFlags.PLS_EXCEPTIONAL | ParallelLoopStateFlags.PLS_STOPPED | ParallelLoopStateFlags.PLS_CANCELED)) == 0)
				{
					if ((loopStateFlags & ParallelLoopStateFlags.PLS_BROKEN) != 0)
					{
						return CallerIteration > LowestBreakIteration;
					}
					return false;
				}
				return true;
			}
			return false;
		}

		internal bool ShouldExitLoop()
		{
			int loopStateFlags = base.LoopStateFlags;
			if (loopStateFlags != ParallelLoopStateFlags.PLS_NONE)
			{
				return (loopStateFlags & (ParallelLoopStateFlags.PLS_EXCEPTIONAL | ParallelLoopStateFlags.PLS_CANCELED)) != 0;
			}
			return false;
		}
	}
}
