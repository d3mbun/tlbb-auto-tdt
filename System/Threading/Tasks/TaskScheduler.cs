using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security;
using System.Security.Permissions;

namespace System.Threading.Tasks
{
	/// <summary>
	/// Represents an abstract scheduler for tasks.
	/// </summary>
	/// <remarks>
	/// <para>
	/// <see cref="T:System.Threading.Tasks.TaskScheduler">TaskScheduler</see> acts as the extension point for all 
	/// pluggable scheduling logic.  This includes mechanisms such as how to schedule a task for execution, and
	/// how scheduled tasks should be exposed to debuggers.
	/// </para>
	/// <para>
	/// All members of the abstract <see cref="T:System.Threading.Tasks.TaskScheduler" /> type are thread-safe
	/// and may be used from multiple threads concurrently.
	/// </para>
	/// </remarks>
	[DebuggerTypeProxy(typeof(SystemThreadingTasks_TaskSchedulerDebugView))]
	[DebuggerDisplay("Id={Id}")]
	[PermissionSet(SecurityAction.InheritanceDemand, Unrestricted = true)]
	[HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
	public abstract class TaskScheduler
	{
		/// <summary>
		/// Nested class that provides debugger view for TaskScheduler
		/// </summary>
		internal sealed class SystemThreadingTasks_TaskSchedulerDebugView
		{
			private readonly TaskScheduler m_taskScheduler;

			public int Id => m_taskScheduler.Id;

			public IEnumerable<Task> ScheduledTasks
			{
				[SecurityCritical]
				get
				{
					return m_taskScheduler.GetScheduledTasks();
				}
			}

			public SystemThreadingTasks_TaskSchedulerDebugView(TaskScheduler scheduler)
			{
				m_taskScheduler = scheduler;
			}
		}

		private static TaskScheduler s_defaultTaskScheduler = new ThreadPoolTaskScheduler();

		internal static int s_taskSchedulerIdCounter;

		private int m_taskSchedulerId;

		internal WeakReference m_weakReferenceToSelf;

		private static ConcurrentDictionary<WeakReference, object> s_activeTaskSchedulers;

		private static object _unobservedTaskExceptionLockObject = new object();

		/// <summary>
		/// Indicates the maximum concurrency level this 
		/// <see cref="T:System.Threading.Tasks.TaskScheduler" />  is able to support.
		/// </summary>
		public virtual int MaximumConcurrencyLevel => int.MaxValue;

		/// <summary>
		/// Indicates whether this is a custom scheduler, in which case the safe code paths will be taken upon task entry
		/// using a CAS to transition from queued state to executing.
		/// </summary>
		internal virtual bool RequiresAtomicStartTransition => true;

		/// <summary>
		/// Gets the default <see cref="T:System.Threading.Tasks.TaskScheduler">TaskScheduler</see> instance.
		/// </summary>
		public static TaskScheduler Default => s_defaultTaskScheduler;

		/// <summary>
		/// Gets the <see cref="T:System.Threading.Tasks.TaskScheduler">TaskScheduler</see>
		/// associated with the currently executing task.
		/// </summary>
		/// <remarks>
		/// When not called from within a task, <see cref="P:System.Threading.Tasks.TaskScheduler.Current" /> will return the <see cref="P:System.Threading.Tasks.TaskScheduler.Default" /> scheduler.
		/// </remarks>
		public static TaskScheduler Current
		{
			get
			{
				Task internalCurrent = Task.InternalCurrent;
				if (internalCurrent != null)
				{
					return internalCurrent.ExecutingTaskScheduler;
				}
				return Default;
			}
		}

		/// <summary>
		/// Gets the unique ID for this <see cref="T:System.Threading.Tasks.TaskScheduler" />.
		/// </summary>
		public int Id
		{
			get
			{
				if (m_taskSchedulerId == 0)
				{
					int num = 0;
					do
					{
						num = Interlocked.Increment(ref s_taskSchedulerIdCounter);
					}
					while (num == 0);
					Interlocked.CompareExchange(ref m_taskSchedulerId, num, 0);
				}
				return m_taskSchedulerId;
			}
		}

		private static event EventHandler<UnobservedTaskExceptionEventArgs> _unobservedTaskException;

		/// <summary>
		/// Occurs when a faulted <see cref="T:System.Threading.Tasks.Task" />'s unobserved exception is about to trigger exception escalation
		/// policy, which, by default, would terminate the process.
		/// </summary>
		/// <remarks>
		/// This AppDomain-wide event provides a mechanism to prevent exception
		/// escalation policy (which, by default, terminates the process) from triggering. 
		/// Each handler is passed a <see cref="T:System.Threading.Tasks.UnobservedTaskExceptionEventArgs" />
		/// instance, which may be used to examine the exception and to mark it as observed.
		/// </remarks>
		public static event EventHandler<UnobservedTaskExceptionEventArgs> UnobservedTaskException
		{
			[SecurityCritical]
			add
			{
				if (value != null)
				{
					lock (_unobservedTaskExceptionLockObject)
					{
						_unobservedTaskException += value;
					}
				}
			}
			[SecurityCritical]
			remove
			{
				lock (_unobservedTaskExceptionLockObject)
				{
					_unobservedTaskException -= value;
				}
			}
		}

		/// <summary>
		/// Queues a <see cref="T:System.Threading.Tasks.Task">Task</see> to the scheduler.
		/// </summary>
		/// <remarks>
		/// <para>
		/// A class derived from <see cref="T:System.Threading.Tasks.TaskScheduler">TaskScheduler</see>  
		/// implements this method to accept tasks being scheduled on the scheduler.
		/// A typical implementation would store the task in an internal data structure, which would
		/// be serviced by threads that would execute those tasks at some time in the future.
		/// </para>
		/// <para>
		/// This method is only meant to be called by the .NET Framework and
		/// should not be called directly by the derived class. This is necessary 
		/// for maintaining the consistency of the system.
		/// </para>
		/// </remarks>
		/// <param name="task">The <see cref="T:System.Threading.Tasks.Task">Task</see> to be queued.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="task" /> argument is null.</exception>
		[SecurityCritical]
		protected internal abstract void QueueTask(Task task);

		/// <summary>
		/// Determines whether the provided <see cref="T:System.Threading.Tasks.Task">Task</see>
		/// can be executed synchronously in this call, and if it can, executes it.
		/// </summary>
		/// <remarks>
		/// <para>
		/// A class derived from <see cref="T:System.Threading.Tasks.TaskScheduler">TaskScheduler</see> implements this function to
		/// support inline execution of a task on a thread that initiates a wait on that task object. Inline
		/// execution is optional, and the request may be rejected by returning false. However, better
		/// scalability typically results the more tasks that can be inlined, and in fact a scheduler that
		/// inlines too little may be prone to deadlocks. A proper implementation should ensure that a
		/// request executing under the policies guaranteed by the scheduler can successfully inline. For
		/// example, if a scheduler uses a dedicated thread to execute tasks, any inlining requests from that
		/// thread should succeed.
		/// </para>
		/// <para>
		/// If a scheduler decides to perform the inline execution, it should do so by calling to the base
		/// TaskScheduler's
		/// <see cref="M:System.Threading.Tasks.TaskScheduler.TryExecuteTask(System.Threading.Tasks.Task)">TryExecuteTask</see> method with the provided task object, propagating
		/// the return value. It may also be appropriate for the scheduler to remove an inlined task from its
		/// internal data structures if it decides to honor the inlining request. Note, however, that under
		/// some circumstances a scheduler may be asked to inline a task that was not previously provided to
		/// it with the <see cref="M:System.Threading.Tasks.TaskScheduler.QueueTask(System.Threading.Tasks.Task)" /> method.
		/// </para>
		/// <para>
		/// The derived scheduler is responsible for making sure that the calling thread is suitable for
		/// executing the given task as far as its own scheduling and execution policies are concerned.
		/// </para>
		/// </remarks>
		/// <param name="task">The <see cref="T:System.Threading.Tasks.Task">Task</see> to be
		/// executed.</param>
		/// <param name="taskWasPreviouslyQueued">A Boolean denoting whether or not task has previously been
		/// queued. If this parameter is True, then the task may have been previously queued (scheduled); if
		/// False, then the task is known not to have been queued, and this call is being made in order to
		/// execute the task inline without queueing it.</param>
		/// <returns>A Boolean value indicating whether the task was executed inline.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="task" /> argument is
		/// null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The <paramref name="task" /> was already
		/// executed.</exception>
		[SecurityCritical]
		protected abstract bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued);

		/// <summary>
		/// Generates an enumerable of <see cref="T:System.Threading.Tasks.Task">Task</see> instances
		/// currently queued to the scheduler waiting to be executed.
		/// </summary>
		/// <remarks>
		/// <para>
		/// A class derived from <see cref="T:System.Threading.Tasks.TaskScheduler" /> implements this method in order to support
		/// integration with debuggers. This method will only be invoked by the .NET Framework when the
		/// debugger requests access to the data. The enumerable returned will be traversed by debugging
		/// utilities to access the tasks currently queued to this scheduler, enabling the debugger to
		/// provide a representation of this information in the user interface.
		/// </para>
		/// <para>
		/// It is important to note that, when this method is called, all other threads in the process will
		/// be frozen. Therefore, it's important to avoid synchronization with other threads that may lead to
		/// blocking. If synchronization is necessary, the method should prefer to throw a <see cref="T:System.NotSupportedException" />
		/// than to block, which could cause a debugger to experience delays. Additionally, this method and
		/// the enumerable returned must not modify any globally visible state.
		/// </para>
		/// <para>
		/// The returned enumerable should never be null. If there are currently no queued tasks, an empty
		/// enumerable should be returned instead.
		/// </para>
		/// <para>
		/// For developers implementing a custom debugger, this method shouldn't be called directly, but
		/// rather this functionality should be accessed through the internal wrapper method
		/// GetScheduledTasksForDebugger:
		/// <c>internal Task[] GetScheduledTasksForDebugger()</c>. This method returns an array of tasks,
		/// rather than an enumerable. In order to retrieve a list of active schedulers, a debugger may use
		/// another internal method: <c>internal static TaskScheduler[] GetTaskSchedulersForDebugger()</c>.
		/// This static method returns an array of all active TaskScheduler instances.
		/// GetScheduledTasksForDebugger then may be used on each of these scheduler instances to retrieve
		/// the list of scheduled tasks for each.
		/// </para>
		/// </remarks>
		/// <returns>An enumerable that allows traversal of tasks currently queued to this scheduler.
		/// </returns>
		/// <exception cref="T:System.NotSupportedException">
		/// This scheduler is unable to generate a list of queued tasks at this time.
		/// </exception>
		[SecurityCritical]
		protected abstract IEnumerable<Task> GetScheduledTasks();

		/// <summary>
		/// Retrieves some thread static state that can be cached and passed to multiple
		/// TryRunInline calls, avoiding superflous TLS fetches.
		/// </summary>
		/// <returns>A bag of TLS state (or null if none exists).</returns>
		[SecuritySafeCritical]
		internal virtual object GetThreadStatics()
		{
			return null;
		}

		/// <summary>
		/// Attempts to execute the target task synchronously.
		/// </summary>
		/// <param name="task">The task to run.</param>
		/// <param name="taskWasPreviouslyQueued">True if the task may have been previously queued,
		/// false if the task was absolutely not previously queued.</param>
		/// <param name="threadStatics">The state retrieved from GetThreadStatics</param>
		/// <returns>True if it ran, false otherwise.</returns>
		[SecuritySafeCritical]
		internal bool TryRunInline(Task task, bool taskWasPreviouslyQueued, object threadStatics)
		{
			TaskScheduler executingTaskScheduler = task.ExecutingTaskScheduler;
			if (executingTaskScheduler != this && executingTaskScheduler != null)
			{
				return executingTaskScheduler.TryRunInline(task, taskWasPreviouslyQueued);
			}
			if (executingTaskScheduler == null || task.m_action == null || task.IsDelegateInvoked || task.IsCanceled || !Task.CurrentStackGuard.TryBeginInliningScope())
			{
				return false;
			}
			bool flag = false;
			try
			{
				flag = TryExecuteTaskInline(task, taskWasPreviouslyQueued);
			}
			finally
			{
				Task.CurrentStackGuard.EndInliningScope();
			}
			if (flag && !task.IsDelegateInvoked && !task.IsCanceled)
			{
				throw new InvalidOperationException(Environment2.GetResourceString("TaskScheduler_InconsistentStateAfterTryExecuteTaskInline"));
			}
			return flag;
		}

		[SecuritySafeCritical]
		internal bool TryRunInline(Task task, bool taskWasPreviouslyQueued)
		{
			return TryRunInline(task, taskWasPreviouslyQueued, GetThreadStatics());
		}

		/// <summary>
		/// Attempts to dequeue a <see cref="T:System.Threading.Tasks.Task">Task</see> that was previously queued to
		/// this scheduler.
		/// </summary>
		/// <param name="task">The <see cref="T:System.Threading.Tasks.Task">Task</see> to be dequeued.</param>
		/// <returns>A Boolean denoting whether the <paramref name="task" /> argument was successfully dequeued.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="task" /> argument is null.</exception>
		[SecurityCritical]
		protected internal virtual bool TryDequeue(Task task)
		{
			return false;
		}

		/// <summary>
		/// Notifies the scheduler that a work item has made progress.
		/// </summary>
		internal virtual void NotifyWorkItemProgress()
		{
		}

		/// <summary>
		/// Initializes the <see cref="T:System.Threading.Tasks.TaskScheduler" />.
		/// </summary>
		protected TaskScheduler()
		{
			m_weakReferenceToSelf = new WeakReference(this);
			RegisterTaskScheduler(this);
		}

		/// <summary>
		/// Frees all resources associated with this scheduler.
		/// </summary>
		~TaskScheduler()
		{
			UnregisterTaskScheduler(this);
		}

		/// <summary>
		/// Creates a <see cref="T:System.Threading.Tasks.TaskScheduler" />
		/// associated with the current <see cref="T:System.Threading.SynchronizationContext" />.
		/// </summary>
		/// <remarks>
		/// All <see cref="T:System.Threading.Tasks.Task">Task</see> instances queued to 
		/// the returned scheduler will be executed through a call to the
		/// <see cref="M:System.Threading.SynchronizationContext.Post(System.Threading.SendOrPostCallback,System.Object)">Post</see> method
		/// on that context.
		/// </remarks>
		/// <returns>
		/// A <see cref="T:System.Threading.Tasks.TaskScheduler" /> associated with 
		/// the current <see cref="T:System.Threading.SynchronizationContext">SynchronizationContext</see>, as
		/// determined by <see cref="P:System.Threading.SynchronizationContext.Current">SynchronizationContext.Current</see>.
		/// </returns>
		/// <exception cref="T:System.InvalidOperationException">
		/// The current SynchronizationContext may not be used as a TaskScheduler.
		/// </exception>
		public static TaskScheduler FromCurrentSynchronizationContext()
		{
			return new SynchronizationContextTaskScheduler();
		}

		/// <summary>
		/// Attempts to execute the provided <see cref="T:System.Threading.Tasks.Task">Task</see>
		/// on this scheduler.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Scheduler implementations are provided with <see cref="T:System.Threading.Tasks.Task">Task</see>
		/// instances to be executed through either the <see cref="M:System.Threading.Tasks.TaskScheduler.QueueTask(System.Threading.Tasks.Task)" /> method or the
		/// <see cref="M:System.Threading.Tasks.TaskScheduler.TryExecuteTaskInline(System.Threading.Tasks.Task,System.Boolean)" /> method. When the scheduler deems it appropriate to run the
		/// provided task, <see cref="M:System.Threading.Tasks.TaskScheduler.TryExecuteTask(System.Threading.Tasks.Task)" /> should be used to do so. TryExecuteTask handles all
		/// aspects of executing a task, including action invocation, exception handling, state management,
		/// and lifecycle control.
		/// </para>
		/// <para>
		/// <see cref="M:System.Threading.Tasks.TaskScheduler.TryExecuteTask(System.Threading.Tasks.Task)" /> must only be used for tasks provided to this scheduler by the .NET
		/// Framework infrastructure. It should not be used to execute arbitrary tasks obtained through
		/// custom mechanisms.
		/// </para>
		/// </remarks>
		/// <param name="task">
		/// A <see cref="T:System.Threading.Tasks.Task">Task</see> object to be executed.</param>
		/// <exception cref="T:System.InvalidOperationException">
		/// The <paramref name="task" /> is not associated with this scheduler.
		/// </exception>
		/// <returns>A Boolean that is true if <paramref name="task" /> was successfully executed, false if it
		/// was not. A common reason for execution failure is that the task had previously been executed or
		/// is in the process of being executed by another thread.</returns>
		[SecurityCritical]
		protected bool TryExecuteTask(Task task)
		{
			if (task.ExecutingTaskScheduler != this)
			{
				throw new InvalidOperationException(Environment2.GetResourceString("TaskScheduler_ExecuteTask_WrongTaskScheduler"));
			}
			return task.ExecuteEntry(bPreventDoubleExecution: true);
		}

		internal static void PublishUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs ueea)
		{
			lock (_unobservedTaskExceptionLockObject)
			{
				TaskScheduler._unobservedTaskException?.Invoke(sender, ueea);
			}
		}

		/// <summary>
		/// Provides an array of all queued <see cref="T:System.Threading.Tasks.Task">Task</see> instances
		/// for the debugger.
		/// </summary>
		/// <remarks>
		/// The returned array is populated through a call to <see cref="M:System.Threading.Tasks.TaskScheduler.GetScheduledTasks" />.
		/// Note that this function is only meant to be invoked by a debugger remotely. 
		/// It should not be called by any other codepaths.
		/// </remarks>
		/// <returns>An array of <see cref="T:System.Threading.Tasks.Task">Task</see> instances.</returns> 
		/// <exception cref="T:System.NotSupportedException">
		/// This scheduler is unable to generate a list of queued tasks at this time.
		/// </exception>
		[SecurityCritical]
		internal Task[] GetScheduledTasksForDebugger()
		{
			IEnumerable<Task> scheduledTasks = GetScheduledTasks();
			if (scheduledTasks == null)
			{
				return null;
			}
			Task[] array = scheduledTasks as Task[];
			if (array == null)
			{
				array = new List<Task>(scheduledTasks).ToArray();
			}
			Task[] array2 = array;
			foreach (Task task in array2)
			{
				_ = task.Id;
			}
			return array;
		}

		/// <summary>
		/// Provides an array of all active <see cref="T:System.Threading.Tasks.TaskScheduler">TaskScheduler</see> 
		/// instances for the debugger.
		/// </summary>
		/// <remarks>
		/// This function is only meant to be invoked by a debugger remotely. 
		/// It should not be called by any other codepaths.
		/// </remarks>
		/// <returns>An array of <see cref="T:System.Threading.Tasks.TaskScheduler">TaskScheduler</see> instances.</returns> 
		[SecurityCritical]
		internal static TaskScheduler[] GetTaskSchedulersForDebugger()
		{
			TaskScheduler[] array = new TaskScheduler[s_activeTaskSchedulers.Count];
			IEnumerator<KeyValuePair<WeakReference, object>> enumerator = s_activeTaskSchedulers.GetEnumerator();
			int num = 0;
			while (enumerator.MoveNext())
			{
				TaskScheduler taskScheduler = enumerator.Current.Key.Target as TaskScheduler;
				if (taskScheduler != null)
				{
					array[num++] = taskScheduler;
					_ = taskScheduler.Id;
				}
			}
			return array;
		}

		/// <summary>
		/// Registers a new TaskScheduler instance in the global collection of schedulers.
		/// </summary>
		internal static void RegisterTaskScheduler(TaskScheduler ts)
		{
			LazyInitializer.EnsureInitialized(ref s_activeTaskSchedulers);
			s_activeTaskSchedulers.TryAdd(ts.m_weakReferenceToSelf, null);
		}

		/// <summary>
		/// Removes a TaskScheduler instance from the global collection of schedulers.
		/// </summary>
		internal static void UnregisterTaskScheduler(TaskScheduler ts)
		{
			s_activeTaskSchedulers.TryRemove(ts.m_weakReferenceToSelf, out var _);
		}
	}
}
