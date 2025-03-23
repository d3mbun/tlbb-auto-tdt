using System.Collections.Generic;
using System.Security;

namespace System.Threading.Tasks
{
	/// <summary>
	/// An implementation of TaskScheduler that uses the ThreadPool scheduler
	/// </summary>
	internal sealed class ThreadPoolTaskScheduler : TaskScheduler
	{
		private static ParameterizedThreadStart s_longRunningThreadWork = LongRunningThreadWork;

		private static WaitCallback s_taskExecuteWaitCallback = TaskExecuteWaitCallback;

		/// <summary>
		/// This is the only scheduler that returns false for this property, indicating that the task entry codepath is unsafe (CAS free)
		/// since we know that the underlying scheduler already takes care of atomic transitions from queued to non-queued.
		/// </summary>
		internal override bool RequiresAtomicStartTransition => true;

		/// <summary>
		/// Constructs a new ThreadPool task scheduler object
		/// </summary>
		internal ThreadPoolTaskScheduler()
		{
		}

		private static void LongRunningThreadWork(object obj)
		{
			Task task = obj as Task;
			task.ExecuteEntry(bPreventDoubleExecution: true);
		}

		/// <summary>
		/// Schedules a task to the ThreadPool.
		/// </summary>
		/// <param name="task">The task to schedule.</param>
		[SecurityCritical]
		protected internal override void QueueTask(Task task)
		{
			if ((task.Options & TaskCreationOptions.LongRunning) != 0)
			{
				Thread thread = new Thread(s_longRunningThreadWork);
				thread.IsBackground = true;
				thread.Start(task);
			}
			else
			{
				ThreadPool.QueueUserWorkItem(s_taskExecuteWaitCallback, task);
			}
		}

		private static void TaskExecuteWaitCallback(object obj)
		{
			Task task = (Task)obj;
			task.ExecuteEntry(bPreventDoubleExecution: true);
		}

		/// <summary>
		/// This internal function will do this:
		///   (1) If the task had previously been queued, attempt to pop it and return false if that fails.
		///   (2) Propagate the return value from Task.ExecuteEntry() back to the caller.
		///
		/// IMPORTANT NOTE: TryExecuteTaskInline will NOT throw task exceptions itself. Any wait code path using this function needs
		/// to account for exceptions that need to be propagated, and throw themselves accordingly.
		/// </summary>
		[SecurityCritical]
		protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
		{
			try
			{
				return task.ExecuteEntry(bPreventDoubleExecution: true);
			}
			finally
			{
				if (taskWasPreviouslyQueued)
				{
					NotifyWorkItemProgress();
				}
			}
		}

		[SecurityCritical]
		protected internal override bool TryDequeue(Task task)
		{
			return false;
		}

		[SecurityCritical]
		protected override IEnumerable<Task> GetScheduledTasks()
		{
			yield return null;
		}

		private IEnumerable<Task> FilterTasksFromWorkItems(IEnumerable<IThreadPoolWorkItem> tpwItems)
		{
			foreach (IThreadPoolWorkItem tpwi in tpwItems)
			{
				if (tpwi is Task)
				{
					yield return (Task)tpwi;
				}
			}
		}

		/// <summary>
		/// Notifies the scheduler that work is progressing (no-op).
		/// </summary>
		internal override void NotifyWorkItemProgress()
		{
		}
	}
}
