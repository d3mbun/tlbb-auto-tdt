using System.Security;

namespace System.Threading.Tasks
{
	/// <summary>
	/// Internal helper class to keep track of stack depth and decide whether we should inline or not.
	/// </summary>
	internal class StackGuard
	{
		private const int s_maxUncheckedInliningDepth = 20;

		private int m_inliningDepth;

		/// <summary>
		/// This method needs to be called before attempting inline execution on the current thread. 
		/// If false is returned, it means we are too close to the end of the stack and should give up inlining.
		/// Each call to TryBeginInliningScope() that returns true must be matched with a 
		/// call to EndInliningScope() regardless of whether inlining actually took place.
		/// </summary>
		[SecuritySafeCritical]
		internal bool TryBeginInliningScope()
		{
			if (m_inliningDepth < 20 || CheckForSufficientStack())
			{
				m_inliningDepth++;
				return true;
			}
			return false;
		}

		/// <summary>
		/// This needs to be called once for each previous successful TryBeginInliningScope() call after
		/// inlining related logic runs.
		/// </summary>
		[SecuritySafeCritical]
		internal void EndInliningScope()
		{
			m_inliningDepth--;
			if (m_inliningDepth < 0)
			{
				m_inliningDepth = 0;
			}
		}

		[SecurityCritical]
		private bool CheckForSufficientStack()
		{
			return true;
		}
	}
}
