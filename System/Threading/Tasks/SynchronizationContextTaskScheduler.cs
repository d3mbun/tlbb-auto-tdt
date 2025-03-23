using System.Collections.Generic;
using System.Security;

namespace System.Threading.Tasks
{
	/// <summary>
	/// A TaskScheduler implementation that executes all tasks queued to it through a call to 
	/// <see cref="M:System.Threading.SynchronizationContext.Post(System.Threading.SendOrPostCallback,System.Object)" /> on the <see cref="T:System.Threading.SynchronizationContext" /> 
	/// that its associated with. The default constructor for this class binds to the current <see cref="T:System.Threading.SynchronizationContext" /> 
	/// </summary>
	internal sealed class SynchronizationContextTaskScheduler : TaskScheduler
	{
		private SynchronizationContext m_synchronizationContext;

		private static SendOrPostCallback s_postCallback = PostCallback;

		/// <summary>
		/// Implementes the <see cref="T:System.Threading.Tasks.TaskScheduler.MaximumConcurrencyLevel" /> property for
		/// this scheduler class.
		///
		/// By default it returns 1, because a <see cref="T:System.Threading.SynchronizationContext" /> based
		/// scheduler only supports execution on a single thread.
		/// </summary>
		public override int MaximumConcurrencyLevel => 1;

		/// <summary>
		/// Constructs a SynchronizationContextTaskScheduler associated with <see cref="T:System.Threading.SynchronizationContext.Current" /> 
		/// </summary>
		/// <exception cref="T:System.InvalidOperationException">This constructor expects <see cref="T:System.Threading.SynchronizationContext.Current" /> to be set.</exception>
		internal SynchronizationContextTaskScheduler()
		{
			SynchronizationContext current = SynchronizationContext.Current;
			if (current == null)
			{
				throw new InvalidOperationException(Environment2.GetResourceString("TaskScheduler_FromCurrentSynchronizationContext_NoCurrent"));
			}
			m_synchronizationContext = current;
		}

		/// <summary>
		/// Implemetation of <see cref="T:System.Threading.Tasks.TaskScheduler.QueueTask" /> for this scheduler class.
		///
		/// Simply posts the tasks to be executed on the associated <see cref="T:System.Threading.SynchronizationContext" />.
		/// </summary>
		/// <param name="task"></param>
		[SecurityCritical]
		protected internal override void QueueTask(Task task)
		{
			m_synchronizationContext.Post(s_postCallback, task);
		}

		/// <summary>
		/// Implementation of <see cref="T:System.Threading.Tasks.TaskScheduler.TryExecuteTaskInline" />  for this scheduler class.
		///
		/// The task will be executed inline only if the call happens within 
		/// the associated <see cref="T:System.Threading.SynchronizationContext" />.
		/// </summary>
		/// <param name="task"></param>
		/// <param name="taskWasPreviouslyQueued"></param>
		[SecurityCritical]
		protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
		{
			if (SynchronizationContext.Current == m_synchronizationContext)
			{
				return TryExecuteTask(task);
			}
			return false;
		}

		[SecurityCritical]
		protected override IEnumerable<Task> GetScheduledTasks()
		{
			return null;
		}

		private static void PostCallback(object obj)
		{
			Task task = (Task)obj;
			task.ExecuteEntry(bPreventDoubleExecution: true);
		}
	}
}
