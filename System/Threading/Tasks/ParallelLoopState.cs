using System.Diagnostics;
using System.Security.Permissions;

namespace System.Threading.Tasks
{
	/// <summary>
	/// Enables iterations of <see cref="T:System.Threading.Tasks.Parallel" /> loops to interact with
	/// other iterations.
	/// </summary>
	[DebuggerDisplay("ShouldExitCurrentIteration = {ShouldExitCurrentIteration}")]
	[HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
	public class ParallelLoopState
	{
		private ParallelLoopStateFlags m_flagsBase;

		/// <summary>
		/// Internal/virtual support for ShouldExitCurrentIteration.
		/// </summary>
		internal virtual bool InternalShouldExitCurrentIteration
		{
			get
			{
				throw new NotSupportedException(Environment2.GetResourceString("ParallelState_NotSupportedException_UnsupportedMethod"));
			}
		}

		/// <summary>
		/// Gets whether the current iteration of the loop should exit based
		/// on requests made by this or other iterations.
		/// </summary>
		/// <remarks>
		/// When an iteration of a loop calls <see cref="M:System.Threading.Tasks.ParallelLoopState.Break" /> or <see cref="M:System.Threading.Tasks.ParallelLoopState.Stop" />, or
		/// when one throws an exception, or when the loop is canceled, the <see cref="T:System.Threading.Tasks.Parallel" /> class will proactively
		/// attempt to prohibit additional iterations of the loop from starting execution.
		/// However, there may be cases where it is unable to prevent additional iterations from starting.
		/// It may also be the case that a long-running iteration has already begun execution.  In such
		/// cases, iterations may explicitly check the <see cref="P:System.Threading.Tasks.ParallelLoopState.ShouldExitCurrentIteration" /> property and
		/// cease execution if the property returns true.
		/// </remarks>
		public bool ShouldExitCurrentIteration => InternalShouldExitCurrentIteration;

		/// <summary>
		/// Gets whether any iteration of the loop has called <see cref="M:System.Threading.Tasks.ParallelLoopState.Stop" />.
		/// </summary>
		public bool IsStopped => (m_flagsBase.LoopStateFlags & ParallelLoopStateFlags.PLS_STOPPED) != 0;

		/// <summary>
		/// Gets whether any iteration of the loop has thrown an exception that went unhandled by that
		/// iteration.
		/// </summary>
		public bool IsExceptional => (m_flagsBase.LoopStateFlags & ParallelLoopStateFlags.PLS_EXCEPTIONAL) != 0;

		/// <summary>
		/// Internal/virtual support for LowestBreakIteration.
		/// </summary>
		internal virtual long? InternalLowestBreakIteration
		{
			get
			{
				throw new NotSupportedException(Environment2.GetResourceString("ParallelState_NotSupportedException_UnsupportedMethod"));
			}
		}

		/// <summary>
		/// Gets the lowest iteration of the loop from which <see cref="M:System.Threading.Tasks.ParallelLoopState.Break" /> was called.
		/// </summary>
		/// <remarks>
		/// If no iteration of the loop called <see cref="M:System.Threading.Tasks.ParallelLoopState.Break" />, this property will return null.
		/// </remarks>
		public long? LowestBreakIteration => InternalLowestBreakIteration;

		internal ParallelLoopState(ParallelLoopStateFlags fbase)
		{
			m_flagsBase = fbase;
		}

		/// <summary>
		/// Communicates that the <see cref="T:System.Threading.Tasks.Parallel" /> loop should cease execution at the system's earliest
		/// convenience.
		/// </summary>
		/// <exception cref="T:System.InvalidOperationException">
		/// The <see cref="M:System.Threading.Tasks.ParallelLoopState.Break" /> method was previously called.  <see cref="M:System.Threading.Tasks.ParallelLoopState.Break" /> and <see cref="M:System.Threading.Tasks.ParallelLoopState.Stop" /> may not be used in combination by iterations of the same loop.
		/// </exception>
		/// <remarks>
		/// <para>
		/// <see cref="M:System.Threading.Tasks.ParallelLoopState.Stop" /> may be used to communicate to the loop that no other iterations need be run.
		/// For long-running iterations that may already be executing, <see cref="M:System.Threading.Tasks.ParallelLoopState.Stop" /> causes <see cref="P:System.Threading.Tasks.ParallelLoopState.IsStopped" />
		/// to return true for all other iterations of the loop, such that another iteration may check <see cref="P:System.Threading.Tasks.ParallelLoopState.IsStopped" /> and exit early if it's observed to be true.
		/// </para>
		/// <para>
		/// <see cref="M:System.Threading.Tasks.ParallelLoopState.Stop" /> is typically employed in search-based algorithms, where once a result is found,
		/// no other iterations need be executed.
		/// </para>
		/// </remarks>
		public void Stop()
		{
			m_flagsBase.Stop();
		}

		internal virtual void InternalBreak()
		{
			throw new NotSupportedException(Environment2.GetResourceString("ParallelState_NotSupportedException_UnsupportedMethod"));
		}

		/// <summary>
		/// Communicates that the <see cref="T:System.Threading.Tasks.Parallel" /> loop should cease execution at the system's earliest
		/// convenience of iterations beyond the current iteration.
		/// </summary>
		/// <exception cref="T:System.InvalidOperationException">
		/// The <see cref="M:System.Threading.Tasks.ParallelLoopState.Stop" /> method was previously called. <see cref="M:System.Threading.Tasks.ParallelLoopState.Break" /> and <see cref="M:System.Threading.Tasks.ParallelLoopState.Stop" />
		/// may not be used in combination by iterations of the same loop.
		/// </exception>
		/// <remarks>
		/// <para>
		/// <see cref="M:System.Threading.Tasks.ParallelLoopState.Break" /> may be used to communicate to the loop that no other iterations after the
		/// current iteration need be run. For example, if <see cref="M:System.Threading.Tasks.ParallelLoopState.Break" /> is called from the 100th
		/// iteration of a for loop iterating in parallel from 0 to 1000, all iterations less than 100 should
		/// still be run, but the iterations from 101 through to 1000 are not necessary.
		/// </para>
		/// <para>
		/// For long-running iterations that may already be executing, <see cref="M:System.Threading.Tasks.ParallelLoopState.Break" /> causes <see cref="P:System.Threading.Tasks.ParallelLoopState.LowestBreakIteration" />
		/// to be set to the current iteration's index if the current index is less than the current value of
		/// <see cref="P:System.Threading.Tasks.ParallelLoopState.LowestBreakIteration" />.
		/// </para>
		/// <para>
		/// <see cref="M:System.Threading.Tasks.ParallelLoopState.Break" /> is typically employed in search-based algorithms where an ordering is
		/// present in the data source.
		/// </para>
		/// </remarks>
		public void Break()
		{
			InternalBreak();
		}

		internal static void Break(int iteration, ParallelLoopStateFlags32 pflags)
		{
			int oldState = ParallelLoopStateFlags.PLS_NONE;
			if (!pflags.AtomicLoopStateUpdate(ParallelLoopStateFlags.PLS_BROKEN, ParallelLoopStateFlags.PLS_STOPPED | ParallelLoopStateFlags.PLS_EXCEPTIONAL | ParallelLoopStateFlags.PLS_CANCELED, ref oldState))
			{
				if ((oldState & ParallelLoopStateFlags.PLS_STOPPED) != 0)
				{
					throw new InvalidOperationException(Environment2.GetResourceString("ParallelState_Break_InvalidOperationException_BreakAfterStop"));
				}
				return;
			}
			int lowestBreakIteration = pflags.m_lowestBreakIteration;
			if (iteration >= lowestBreakIteration)
			{
				return;
			}
			SpinWait spinWait = default(SpinWait);
			while (Interlocked.CompareExchange(ref pflags.m_lowestBreakIteration, iteration, lowestBreakIteration) != lowestBreakIteration)
			{
				spinWait.SpinOnce();
				lowestBreakIteration = pflags.m_lowestBreakIteration;
				if (iteration > lowestBreakIteration)
				{
					break;
				}
			}
		}

		internal static void Break(long iteration, ParallelLoopStateFlags64 pflags)
		{
			int oldState = ParallelLoopStateFlags.PLS_NONE;
			if (!pflags.AtomicLoopStateUpdate(ParallelLoopStateFlags.PLS_BROKEN, ParallelLoopStateFlags.PLS_STOPPED | ParallelLoopStateFlags.PLS_EXCEPTIONAL | ParallelLoopStateFlags.PLS_CANCELED, ref oldState))
			{
				if ((oldState & ParallelLoopStateFlags.PLS_STOPPED) != 0)
				{
					throw new InvalidOperationException(Environment2.GetResourceString("ParallelState_Break_InvalidOperationException_BreakAfterStop"));
				}
				return;
			}
			long lowestBreakIteration = pflags.LowestBreakIteration;
			if (iteration >= lowestBreakIteration)
			{
				return;
			}
			SpinWait spinWait = default(SpinWait);
			while (Interlocked.CompareExchange(ref pflags.m_lowestBreakIteration, iteration, lowestBreakIteration) != lowestBreakIteration)
			{
				spinWait.SpinOnce();
				lowestBreakIteration = pflags.LowestBreakIteration;
				if (iteration > lowestBreakIteration)
				{
					break;
				}
			}
		}
	}
}
