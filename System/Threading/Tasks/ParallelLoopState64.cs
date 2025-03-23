namespace System.Threading.Tasks
{
	/// <summary>
	/// Allows independent iterations of a parallel loop to interact with other iterations.
	/// </summary>
	internal class ParallelLoopState64 : ParallelLoopState
	{
		private ParallelLoopStateFlags64 m_sharedParallelStateFlags;

		private long m_currentIteration;

		/// <summary>
		/// Tracks the current loop iteration for the owning task.
		/// This is used to compute whether or not the task should
		/// terminate early due to a Break() call.
		/// </summary>
		internal long CurrentIteration
		{
			get
			{
				return m_currentIteration;
			}
			set
			{
				m_currentIteration = value;
			}
		}

		/// <summary>
		/// Returns true if we should be exiting from the current iteration
		/// due to Stop(), Break() or exception.
		/// </summary>
		internal override bool InternalShouldExitCurrentIteration => m_sharedParallelStateFlags.ShouldExitLoop(CurrentIteration);

		/// <summary>
		/// Returns the lowest iteration at which Break() has been called, or
		/// null if Break() has not yet been called.
		/// </summary>
		internal override long? InternalLowestBreakIteration => m_sharedParallelStateFlags.NullableLowestBreakIteration;

		/// <summary>
		/// Internal constructor to ensure an instance isn't created by users.
		/// </summary>
		/// <param name="sharedParallelStateFlags">A flag shared among all threads participating
		/// in the execution of a certain loop.</param>
		internal ParallelLoopState64(ParallelLoopStateFlags64 sharedParallelStateFlags)
			: base(sharedParallelStateFlags)
		{
			m_sharedParallelStateFlags = sharedParallelStateFlags;
		}

		/// <summary>
		/// Communicates that parallel tasks should stop when they reach a specified iteration element.
		/// (which is CurrentIteration of the caller).
		/// </summary>
		/// <exception cref="T:System.InvalidOperationException">Break() called after Stop().</exception>
		/// <remarks>
		/// Atomically sets shared StoppedBroken flag to BROKEN, then atomically sets shared 
		/// LowestBreakIteration to CurrentIteration, but only if CurrentIteration is less than 
		/// LowestBreakIteration.
		/// </remarks>
		internal override void InternalBreak()
		{
			ParallelLoopState.Break(CurrentIteration, m_sharedParallelStateFlags);
		}
	}
}
