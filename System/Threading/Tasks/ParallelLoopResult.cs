namespace System.Threading.Tasks
{
	/// <summary>
	/// Provides completion status on the execution of a <see cref="T:System.Threading.Tasks.Parallel" /> loop.
	/// </summary>
	/// <remarks>
	/// If <see cref="P:System.Threading.Tasks.ParallelLoopResult.IsCompleted" /> returns true, then the loop ran to completion, such that all iterations
	/// of the loop were executed. If <see cref="P:System.Threading.Tasks.ParallelLoopResult.IsCompleted" /> returns false and <see cref="P:System.Threading.Tasks.ParallelLoopResult.LowestBreakIteration" /> returns null, a call to <see cref="M:System.Threading.Tasks.ParallelLoopState.Stop" /> was used to end the loop prematurely. If <see cref="P:System.Threading.Tasks.ParallelLoopResult.IsCompleted" /> returns false and <see cref="P:System.Threading.Tasks.ParallelLoopResult.LowestBreakIteration" /> returns a non-null integral
	/// value, <see cref="M:System.Threading.Tasks.ParallelLoopState.Break" /> was used to end the loop prematurely.
	/// </remarks>
	public struct ParallelLoopResult
	{
		internal bool m_completed;

		internal long? m_lowestBreakIteration;

		/// <summary>
		/// Gets whether the loop ran to completion, such that all iterations of the loop were executed
		/// and the loop didn't receive a request to end prematurely.
		/// </summary>
		public bool IsCompleted => m_completed;

		/// <summary>
		/// Gets the index of the lowest iteration from which <see cref="M:System.Threading.Tasks.ParallelLoopState.Break" />
		/// was called.
		/// </summary>
		/// <remarks>
		/// If <see cref="M:System.Threading.Tasks.ParallelLoopState.Break" /> was not employed, this property will
		/// return null.
		/// </remarks>
		public long? LowestBreakIteration => m_lowestBreakIteration;
	}
}
