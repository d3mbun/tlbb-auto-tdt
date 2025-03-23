namespace System.Threading.Tasks
{
	/// <summary>
	/// Specifies flags that control optional behavior for the creation and execution of continuation tasks.
	/// </summary>
	[Serializable]
	[Flags]
	public enum TaskContinuationOptions
	{
		/// <summary>
		/// Default = "Continue on any, no task options, run asynchronously"
		/// Specifies that the default behavior should be used.  Continuations, by default, will
		/// be scheduled when the antecedent task completes, regardless of the task's final <see cref="T:System.Threading.Tasks.TaskStatus">TaskStatus</see>.
		/// </summary>
		None = 0x0,
		/// <summary>
		/// A hint to a <see cref="T:System.Threading.Tasks.TaskScheduler">TaskScheduler</see> to schedule a
		/// task in as fair a manner as possible, meaning that tasks scheduled sooner will be more likely to
		/// be run sooner, and tasks scheduled later will be more likely to be run later.
		/// </summary>
		PreferFairness = 0x1,
		/// <summary>
		/// Specifies that a task will be a long-running, course-grained operation.  It provides
		/// a hint to the <see cref="T:System.Threading.Tasks.TaskScheduler">TaskScheduler</see> that
		/// oversubscription may be warranted.
		/// </summary>
		LongRunning = 0x2,
		/// <summary>
		/// Specifies that a task is attached to a parent in the task hierarchy.
		/// </summary>
		AttachedToParent = 0x4,
		/// <summary>
		/// Specifies that the continuation task should not be scheduled if its antecedent ran to completion.
		/// This option is not valid for multi-task continuations.
		/// </summary>
		NotOnRanToCompletion = 0x10000,
		/// <summary>
		/// Specifies that the continuation task should not be scheduled if its antecedent threw an unhandled
		/// exception. This option is not valid for multi-task continuations.
		/// </summary>
		NotOnFaulted = 0x20000,
		/// <summary>
		/// Specifies that the continuation task should not be scheduled if its antecedent was canceled. This
		/// option is not valid for multi-task continuations.
		/// </summary>
		NotOnCanceled = 0x40000,
		/// <summary>
		/// Specifies that the continuation task should be scheduled only if its antecedent ran to
		/// completion. This option is not valid for multi-task continuations.
		/// </summary>
		OnlyOnRanToCompletion = 0x60000,
		/// <summary>
		/// Specifies that the continuation task should be scheduled only if its antecedent threw an
		/// unhandled exception. This option is not valid for multi-task continuations.
		/// </summary>
		OnlyOnFaulted = 0x50000,
		/// <summary>
		/// Specifies that the continuation task should be scheduled only if its antecedent was canceled.
		/// This option is not valid for multi-task continuations.
		/// </summary>
		OnlyOnCanceled = 0x30000,
		/// <summary>
		/// Specifies that the continuation task should be executed synchronously. With this option
		/// specified, the continuation will be run on the same thread that causes the antecedent task to
		/// transition into its final state. If the antecedent is already complete when the continuation is
		/// created, the continuation will run on the thread creating the continuation.  Only very
		/// short-running continuations should be executed synchronously.
		/// </summary>
		ExecuteSynchronously = 0x80000
	}
}
