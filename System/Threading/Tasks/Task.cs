using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security;
using System.Security.Permissions;

namespace System.Threading.Tasks
{
	/// <summary>
	/// Represents an asynchronous operation.
	/// </summary>
	/// <remarks>
	/// <para>
	/// <see cref="T:System.Threading.Tasks.Task" /> instances may be created in a variety of ways. The most common approach is by
	/// using the Task type's <see cref="P:System.Threading.Tasks.Task.Factory" /> property to retrieve a <see cref="T:System.Threading.Tasks.TaskFactory" /> instance that can be used to create tasks for several
	/// purposes. For example, to create a <see cref="T:System.Threading.Tasks.Task" /> that runs an action, the factory's StartNew
	/// method may be used:
	/// <code>
	/// // C# 
	/// var t = Task.Factory.StartNew(() =&gt; DoAction());
	///
	/// ' Visual Basic 
	/// Dim t = Task.Factory.StartNew(Function() DoAction())
	/// </code>
	/// </para>
	/// <para>
	/// The <see cref="T:System.Threading.Tasks.Task" /> class also provides constructors that initialize the Task but that do not
	/// schedule it for execution. For performance reasons, TaskFactory's StartNew method should be the
	/// preferred mechanism for creating and scheduling computational tasks, but for scenarios where creation
	/// and scheduling must be separated, the constructors may be used, and the task's <see cref="M:System.Threading.Tasks.Task.Start" />
	/// method may then be used to schedule the task for execution at a later time.
	/// </para>
	/// <para>
	/// All members of <see cref="T:System.Threading.Tasks.Task" />, except for <see cref="M:System.Threading.Tasks.Task.Dispose" />, are thread-safe
	/// and may be used from multiple threads concurrently.
	/// </para>
	/// <para>
	/// For operations that return values, the <see cref="T:System.Threading.Tasks.Task`1" /> class
	/// should be used.
	/// </para>
	/// <para>
	/// For developers implementing custom debuggers, several internal and private members of Task may be
	/// useful (these may change from release to release). The Int32 m_taskId field serves as the backing
	/// store for the <see cref="P:System.Threading.Tasks.Task.Id" /> property, however accessing this field directly from a debugger may be
	/// more efficient than accessing the same value through the property's getter method (the
	/// s_taskIdCounter Int32 counter is used to retrieve the next available ID for a Task). Similarly, the
	/// Int32 m_stateFlags field stores information about the current lifecycle stage of the Task,
	/// information also accessible through the <see cref="P:System.Threading.Tasks.Task.Status" /> property. The m_action System.Object
	/// field stores a reference to the Task's delegate, and the m_stateObject System.Object field stores the
	/// async state passed to the Task by the developer. Finally, for debuggers that parse stack frames, the
	/// InternalWait method serves a potential marker for when a Task is entering a wait operation.
	/// </para>
	/// </remarks>
	[DebuggerTypeProxy(typeof(SystemThreadingTasks_TaskDebugView))]
	[DebuggerDisplay("Id = {Id}, Status = {Status}, Method = {DebuggerDisplayMethodDescription}")]
	[HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
	public class Task : IThreadPoolWorkItem, IAsyncResult, IDisposable
	{
		private static class ThreadLocals
		{
			[ThreadStatic]
			internal static Task s_currentTask;

			[ThreadStatic]
			internal static StackGuard s_stackGuard;
		}

		public static void Run(Action a)
        {
			Task.Factory.StartNew(a);
        }

		internal class ContingentProperties
		{
			public volatile int m_internalCancellationRequested;

			internal volatile int m_completionCountdown = 1;

			public volatile TaskExceptionHolder m_exceptionsHolder;

			public volatile List<Task> m_exceptionalChildren;

			public volatile List<TaskContinuation> m_continuations;

			public CancellationToken m_cancellationToken;

			public Shared<CancellationTokenRegistration> m_cancellationRegistration;
		}

		/// <summary>
		/// A structure to hold continuation information.
		/// </summary>
		internal struct TaskContinuation
		{
			internal object m_task;

			internal TaskScheduler m_taskScheduler;

			internal TaskContinuationOptions m_options;

			/// <summary>
			/// Constructs a new continuation structure.
			/// </summary>
			/// <param name="task">The task to be activated.</param>
			/// <param name="options">The continuation options.</param>
			/// <param name="scheduler">The scheduler to use for the continuation.</param>
			internal TaskContinuation(Task task, TaskScheduler scheduler, TaskContinuationOptions options)
			{
				m_task = task;
				m_taskScheduler = scheduler;
				m_options = options;
			}

			internal TaskContinuation(Action<Task> action)
			{
				m_task = action;
				m_taskScheduler = null;
				m_options = TaskContinuationOptions.None;
			}

			/// <summary>
			/// Invokes the continuation for the target completion task.
			/// </summary>
			/// <param name="completedTask">The completed task.</param>
			/// <param name="bCanInlineContinuationTask">Whether the continuation can be inlined.</param>
			[SecuritySafeCritical]
			internal void Run(Task completedTask, bool bCanInlineContinuationTask)
			{
				Task task = m_task as Task;
				if (task != null)
				{
					if (completedTask.ContinueWithIsRightKind(m_options))
					{
						task.m_taskScheduler = m_taskScheduler;
						if (bCanInlineContinuationTask && (m_options & TaskContinuationOptions.ExecuteSynchronously) != 0)
						{
							if (!task.MarkStarted())
							{
								return;
							}
							try
							{
								if (!m_taskScheduler.TryRunInline(task, taskWasPreviouslyQueued: false))
								{
									m_taskScheduler.QueueTask(task);
								}
							}
							catch (Exception ex)
							{
								if (!(ex is ThreadAbortException) || (task.m_stateFlags & 0x8000000) == 0)
								{
									TaskSchedulerException exceptionObject = new TaskSchedulerException(ex);
									task.AddException(exceptionObject);
									task.Finish(bUserDelegateExecuted: false);
								}
							}
						}
						else
						{
							try
							{
								task.ScheduleAndStart(needsProtection: true);
							}
							catch (TaskSchedulerException)
							{
							}
						}
					}
					else
					{
						task.InternalCancel(bCancelNonExecutingOnly: false);
					}
				}
				else
				{
					Action<Task> action = m_task as Action<Task>;
					action(completedTask);
				}
			}
		}

		private const int OptionsMask = 65535;

		internal const int TASK_STATE_STARTED = 65536;

		internal const int TASK_STATE_DELEGATE_INVOKED = 131072;

		internal const int TASK_STATE_DISPOSED = 262144;

		internal const int TASK_STATE_EXCEPTIONOBSERVEDBYPARENT = 524288;

		internal const int TASK_STATE_CANCELLATIONACKNOWLEDGED = 1048576;

		internal const int TASK_STATE_FAULTED = 2097152;

		internal const int TASK_STATE_CANCELED = 4194304;

		internal const int TASK_STATE_WAITING_ON_CHILDREN = 8388608;

		internal const int TASK_STATE_RAN_TO_COMPLETION = 16777216;

		internal const int TASK_STATE_WAITINGFORACTIVATION = 33554432;

		internal const int TASK_STATE_COMPLETION_RESERVED = 67108864;

		internal const int TASK_STATE_THREAD_WAS_ABORTED = 134217728;

		internal static int s_taskIdCounter;

		private static TaskFactory s_factory;

		private int m_taskId;

		internal object m_action;

		internal object m_stateObject;

		internal TaskScheduler m_taskScheduler;

		internal readonly Task m_parent;

		internal ExecutionContext m_capturedContext;

		internal int m_stateFlags;

		internal static int CANCELLATION_REQUESTED;

		private ManualResetEventSlim m_completionEvent;

		internal ContingentProperties m_contingentProperties;

		internal static Action<object> s_taskCancelCallback;

		internal static Func<ContingentProperties> s_contingentPropertyCreator;

		private static Predicate<Task> s_IsExceptionObservedByParentPredicate;

		[SecurityCritical]
		private static ContextCallback s_ecCallback;

		private string DebuggerDisplayMethodDescription
		{
			get
			{
				Delegate @delegate = (Delegate)m_action;
				if ((object)@delegate == null)
				{
					return "{null}";
				}
				return @delegate.Method.ToString();
			}
		}

		internal TaskCreationOptions Options => (TaskCreationOptions)(m_stateFlags & 0xFFFF);

		/// <summary>
		/// Gets a unique ID for this <see cref="T:System.Threading.Tasks.Task">Task</see> instance.
		/// </summary>
		/// <remarks>
		/// Task IDs are assigned on-demand and do not necessarily represent the order in the which Task
		/// instances were created.
		/// </remarks>
		public int Id
		{
			get
			{
				if (m_taskId == 0)
				{
					int num = 0;
					do
					{
						num = Interlocked.Increment(ref s_taskIdCounter);
					}
					while (num == 0);
					Interlocked.CompareExchange(ref m_taskId, num, 0);
				}
				return m_taskId;
			}
		}

		/// <summary>
		/// Returns the unique ID of the currently executing <see cref="T:System.Threading.Tasks.Task">Task</see>.
		/// </summary>
		public static int? CurrentId => InternalCurrent?.Id;

		/// <summary>
		/// Gets the <see cref="T:System.Threading.Tasks.Task">Task</see> instance currently executing, or
		/// null if none exists.
		/// </summary>
		internal static Task InternalCurrent => ThreadLocals.s_currentTask;

		/// <summary>
		/// Gets the StackGuard object assigned to the current thread.
		/// </summary>
		internal static StackGuard CurrentStackGuard
		{
			get
			{
				StackGuard stackGuard = ThreadLocals.s_stackGuard;
				if (stackGuard == null)
				{
					stackGuard = (ThreadLocals.s_stackGuard = new StackGuard());
				}
				return stackGuard;
			}
		}

		/// <summary>
		/// Gets the <see cref="T:System.AggregateException">Exception</see> that caused the <see cref="T:System.Threading.Tasks.Task">Task</see> to end prematurely. If the <see cref="T:System.Threading.Tasks.Task">Task</see> completed successfully or has not yet thrown any
		/// exceptions, this will return null.
		/// </summary>
		/// <remarks>
		/// Tasks that throw unhandled exceptions store the resulting exception and propagate it wrapped in a
		/// <see cref="T:System.AggregateException" /> in calls to <see cref="M:System.Threading.Tasks.Task.Wait">Wait</see>
		/// or in accesses to the <see cref="P:System.Threading.Tasks.Task.Exception" /> property.  Any exceptions not observed by the time
		/// the Task instance is garbage collected will be propagated on the finalizer thread.
		/// </remarks>
		/// <exception cref="T:System.ObjectDisposedException">
		/// The <see cref="T:System.Threading.Tasks.Task">Task</see>
		/// has been disposed.
		/// </exception>
		public AggregateException Exception
		{
			get
			{
				AggregateException result = null;
				if (IsFaulted)
				{
					result = GetExceptions(includeTaskCanceledExceptions: false);
				}
				return result;
			}
		}

		/// <summary>
		/// Gets the <see cref="T:System.Threading.Tasks.TaskStatus">TaskStatus</see> of this Task. 
		/// </summary>
		public TaskStatus Status
		{
			get
			{
				int stateFlags = m_stateFlags;
				if (((uint)stateFlags & 0x200000u) != 0)
				{
					return TaskStatus.Faulted;
				}
				if (((uint)stateFlags & 0x400000u) != 0)
				{
					return TaskStatus.Canceled;
				}
				if (((uint)stateFlags & 0x1000000u) != 0)
				{
					return TaskStatus.RanToCompletion;
				}
				if (((uint)stateFlags & 0x800000u) != 0)
				{
					return TaskStatus.WaitingForChildrenToComplete;
				}
				if (((uint)stateFlags & 0x20000u) != 0)
				{
					return TaskStatus.Running;
				}
				if (((uint)stateFlags & 0x10000u) != 0)
				{
					return TaskStatus.WaitingToRun;
				}
				if (((uint)stateFlags & 0x2000000u) != 0)
				{
					return TaskStatus.WaitingForActivation;
				}
				return TaskStatus.Created;
			}
		}

		/// <summary>
		/// Gets whether this <see cref="T:System.Threading.Tasks.Task">Task</see> instance has completed
		/// execution due to being canceled.
		/// </summary>
		/// <remarks>
		/// A <see cref="T:System.Threading.Tasks.Task">Task</see> will complete in Canceled state either if its <see cref="P:System.Threading.Tasks.Task.CancellationToken">CancellationToken</see> 
		/// was marked for cancellation before the task started executing, or if the task acknowledged the cancellation request on 
		/// its already signaled CancellationToken by throwing an 
		/// <see cref="T:System.OperationCanceledException">OperationCanceledException</see> that bears the same 
		/// <see cref="T:System.Threading.CancellationToken">CancellationToken</see>.
		/// </remarks>
		public bool IsCanceled => (m_stateFlags & 0x600000) == 4194304;

		/// <summary>
		/// Returns true if this task has a cancellation token and it was signaled.
		/// To be used internally in execute entry codepaths.
		/// </summary>
		internal bool IsCancellationRequested
		{
			get
			{
				if (m_contingentProperties == null || m_contingentProperties.m_internalCancellationRequested != CANCELLATION_REQUESTED)
				{
					return CancellationToken.IsCancellationRequested;
				}
				return true;
			}
		}

		/// <summary>
		/// This internal property provides access to the CancellationToken that was set on the task 
		/// when it was constructed.
		/// </summary>
		internal CancellationToken CancellationToken
		{
			get
			{
				if (m_contingentProperties != null)
				{
					return m_contingentProperties.m_cancellationToken;
				}
				return CancellationToken.None;
			}
		}

		/// <summary>
		/// Gets whether this <see cref="T:System.Threading.Tasks.Task" /> threw an OperationCanceledException while its CancellationToken was signaled.
		/// </summary>
		internal bool IsCancellationAcknowledged => (m_stateFlags & 0x100000) != 0;

		/// <summary>
		/// Gets whether this <see cref="T:System.Threading.Tasks.Task">Task</see> has completed.
		/// </summary>
		/// <remarks>
		/// <see cref="P:System.Threading.Tasks.Task.IsCompleted" /> will return true when the Task is in one of the three
		/// final states: <see cref="F:System.Threading.Tasks.TaskStatus.RanToCompletion">RanToCompletion</see>,
		/// <see cref="F:System.Threading.Tasks.TaskStatus.Faulted">Faulted</see>, or
		/// <see cref="F:System.Threading.Tasks.TaskStatus.Canceled">Canceled</see>.
		/// </remarks>
		public bool IsCompleted => (m_stateFlags & 0x1600000) != 0;

		internal bool CompletedSuccessfully
		{
			get
			{
				int num = 23068672;
				return (m_stateFlags & num) == 16777216;
			}
		}

		/// <summary>
		/// Checks whether this task has been disposed.
		/// </summary>
		internal bool IsDisposed => (m_stateFlags & 0x40000) != 0;

		/// <summary>
		/// Gets the <see cref="T:System.Threading.Tasks.TaskCreationOptions">TaskCreationOptions</see> used
		/// to create this task.
		/// </summary>
		public TaskCreationOptions CreationOptions => Options & (TaskCreationOptions)(-65281);

		/// <summary>
		/// Gets a <see cref="T:System.Threading.WaitHandle" /> that can be used to wait for the task to
		/// complete.
		/// </summary>
		/// <remarks>
		/// Using the wait functionality provided by <see cref="M:System.Threading.Tasks.Task.Wait" />
		/// should be preferred over using <see cref="P:System.IAsyncResult.AsyncWaitHandle" /> for similar
		/// functionality.
		/// </remarks>
		/// <exception cref="T:System.ObjectDisposedException">
		/// The <see cref="T:System.Threading.Tasks.Task" /> has been disposed.
		/// </exception>
		WaitHandle IAsyncResult.AsyncWaitHandle
		{
			get
			{
				ThrowIfDisposed();
				return CompletedEvent.WaitHandle;
			}
		}

		internal virtual object InternalAsyncState => m_stateObject;

		/// <summary>
		/// Gets the state object supplied when the <see cref="T:System.Threading.Tasks.Task">Task</see> was created,
		/// or null if none was supplied.
		/// </summary>
		public object AsyncState => InternalAsyncState;

		/// <summary>
		/// Gets an indication of whether the asynchronous operation completed synchronously.
		/// </summary>
		/// <value>true if the asynchronous operation completed synchronously; otherwise, false.</value>
		bool IAsyncResult.CompletedSynchronously => false;

		/// <summary>
		/// Provides access to the TaskScheduler responsible for executing this Task.
		/// </summary>
		internal TaskScheduler ExecutingTaskScheduler => m_taskScheduler;

		/// <summary>
		/// Provides access to factory methods for creating <see cref="T:System.Threading.Tasks.Task" /> and <see cref="T:System.Threading.Tasks.Task`1" /> instances.
		/// </summary>
		/// <remarks>
		/// The factory returned from <see cref="P:System.Threading.Tasks.Task.Factory" /> is a default instance
		/// of <see cref="T:System.Threading.Tasks.TaskFactory" />, as would result from using
		/// the default constructor on TaskFactory.
		/// </remarks>
		public static TaskFactory Factory => s_factory;

		/// <summary>
		/// Provides an event that can be used to wait for completion.
		/// Only called by Wait*(), which means that we really do need to instantiate a completion event.
		/// </summary>
		internal ManualResetEventSlim CompletedEvent
		{
			get
			{
				if (m_completionEvent == null)
				{
					bool isCompleted = IsCompleted;
					ManualResetEventSlim manualResetEventSlim = new ManualResetEventSlim(isCompleted);
					if (Interlocked.CompareExchange(ref m_completionEvent, manualResetEventSlim, null) != null)
					{
						manualResetEventSlim.Dispose();
					}
					else if (!isCompleted && IsCompleted)
					{
						manualResetEventSlim.Set();
					}
				}
				return m_completionEvent;
			}
		}

		/// <summary>
		/// Determines whether this is the root task of a self replicating group.
		/// </summary>
		internal bool IsSelfReplicatingRoot
		{
			get
			{
				if ((Options & (TaskCreationOptions)2048) != 0)
				{
					return (Options & (TaskCreationOptions)256) == 0;
				}
				return false;
			}
		}

		/// <summary>
		/// Determines whether the task is a replica itself.
		/// </summary>
		internal bool IsChildReplica => (Options & (TaskCreationOptions)256) != 0;

		internal int ActiveChildCount
		{
			get
			{
				if (m_contingentProperties == null)
				{
					return 0;
				}
				return m_contingentProperties.m_completionCountdown - 1;
			}
		}

		/// <summary>
		/// The property formerly known as IsFaulted.
		/// </summary>
		internal bool ExceptionRecorded
		{
			get
			{
				if (m_contingentProperties != null)
				{
					return m_contingentProperties.m_exceptionsHolder != null;
				}
				return false;
			}
		}

		/// <summary>
		/// Gets whether the <see cref="T:System.Threading.Tasks.Task" /> completed due to an unhandled exception.
		/// </summary>
		/// <remarks>
		/// If <see cref="P:System.Threading.Tasks.Task.IsFaulted" /> is true, the Task's <see cref="P:System.Threading.Tasks.Task.Status" /> will be equal to
		/// <see cref="F:System.Threading.Tasks.TaskStatus.Faulted">TaskStatus.Faulted</see>, and its
		/// <see cref="P:System.Threading.Tasks.Task.Exception" /> property will be non-null.
		/// </remarks>
		public bool IsFaulted => (m_stateFlags & 0x200000) != 0;

		/// <summary>
		/// Checks whether the TASK_STATE_EXCEPTIONOBSERVEDBYPARENT status flag is set,
		/// This will only be used by the implicit wait to prevent double throws
		///
		/// </summary>
		internal bool IsExceptionObservedByParent => (m_stateFlags & 0x80000) != 0;

		/// <summary>
		/// Checks whether the body was ever invoked. Used by task scheduler code to verify custom schedulers actually ran the task.
		/// </summary>
		internal bool IsDelegateInvoked => (m_stateFlags & 0x20000) != 0;

		internal virtual object SavedStateForNextReplica
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		internal virtual object SavedStateFromPreviousReplica
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		internal virtual Task HandedOverChildReplica
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		/// <summary>
		/// A type initializer that runs with the appropriate permissions.
		/// </summary>
		[SecuritySafeCritical]
		static Task()
		{
			s_factory = new TaskFactory();
			CANCELLATION_REQUESTED = 1;
			s_taskCancelCallback = TaskCancelCallback;
			s_contingentPropertyCreator = ContingentPropertyCreator;
			s_IsExceptionObservedByParentPredicate = (Task t) => t.IsExceptionObservedByParent;
			s_ecCallback = ExecutionContextCallback;
		}

		internal Task(bool canceled, TaskCreationOptions creationOptions)
		{
			if (canceled)
			{
				m_stateFlags = (int)((TaskCreationOptions)5242880 | creationOptions);
			}
			else
			{
				m_stateFlags = (int)((TaskCreationOptions)16777216 | creationOptions);
			}
		}

		internal Task(object state, CancellationToken cancelationToken, TaskCreationOptions creationOptions, InternalTaskOptions internalOptions, bool promiseStyle)
		{
			if (((uint)creationOptions & 0xFFFFFFFBu) != 0)
			{
				throw new ArgumentOutOfRangeException("creationOptions");
			}
			if (((uint)internalOptions & 0xFFFFFBFFu) != 0)
			{
				throw new ArgumentOutOfRangeException("internalOptions", Environment2.GetResourceString("Task_PromiseCtor_IllegalInternalOptions"));
			}
			if ((creationOptions & TaskCreationOptions.AttachedToParent) != 0)
			{
				m_parent = InternalCurrent;
			}
			TaskConstructorCore(null, state, cancelationToken, creationOptions, internalOptions, TaskScheduler.Current);
		}

		/// <summary>
		/// Initializes a new <see cref="T:System.Threading.Tasks.Task" /> with the specified action.
		/// </summary>
		/// <param name="action">The delegate that represents the code to execute in the Task.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="action" /> argument is null.</exception>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task(Action action)
			: this(action, null, InternalCurrent, CancellationToken.None, TaskCreationOptions.None, InternalTaskOptions.None, null)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			PossiblyCaptureContext(ref stackMark);
		}

		/// <summary>
		/// Initializes a new <see cref="T:System.Threading.Tasks.Task" /> with the specified action and <see cref="T:System.Threading.CancellationToken">CancellationToken</see>.
		/// </summary>
		/// <param name="action">The delegate that represents the code to execute in the Task.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken">CancellationToken</see>
		/// that will be assigned to the new Task.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="action" /> argument is null.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken">CancellationToken</see>
		/// has already been disposed.
		/// </exception>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task(Action action, CancellationToken cancellationToken)
			: this(action, null, InternalCurrent, cancellationToken, TaskCreationOptions.None, InternalTaskOptions.None, null)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			PossiblyCaptureContext(ref stackMark);
		}

		/// <summary>
		/// Initializes a new <see cref="T:System.Threading.Tasks.Task" /> with the specified action and creation options.
		/// </summary>
		/// <param name="action">The delegate that represents the code to execute in the task.</param>
		/// <param name="creationOptions">
		/// The <see cref="T:System.Threading.Tasks.TaskCreationOptions">TaskCreationOptions</see> used to
		/// customize the Task's behavior.
		/// </param>
		/// <exception cref="T:System.ArgumentNullException">
		/// The <paramref name="action" /> argument is null.
		/// </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// The <paramref name="creationOptions" /> argument specifies an invalid value for <see cref="T:System.Threading.Tasks.TaskCreationOptions" />.
		/// </exception>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task(Action action, TaskCreationOptions creationOptions)
			: this(action, null, InternalCurrent, CancellationToken.None, creationOptions, InternalTaskOptions.None, null)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			PossiblyCaptureContext(ref stackMark);
		}

		/// <summary>
		/// Initializes a new <see cref="T:System.Threading.Tasks.Task" /> with the specified action and creation options.
		/// </summary>
		/// <param name="action">The delegate that represents the code to execute in the task.</param>
		/// <param name="cancellationToken">The <see cref="P:System.Threading.Tasks.Task.CancellationToken" /> that will be assigned to the new task.</param>
		/// <param name="creationOptions">
		/// The <see cref="T:System.Threading.Tasks.TaskCreationOptions">TaskCreationOptions</see> used to
		/// customize the Task's behavior.
		/// </param>
		/// <exception cref="T:System.ArgumentNullException">
		/// The <paramref name="action" /> argument is null.
		/// </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// The <paramref name="creationOptions" /> argument specifies an invalid value for <see cref="T:System.Threading.Tasks.TaskCreationOptions" />.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken">CancellationToken</see>
		/// has already been disposed.
		/// </exception>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task(Action action, CancellationToken cancellationToken, TaskCreationOptions creationOptions)
			: this(action, null, InternalCurrent, cancellationToken, creationOptions, InternalTaskOptions.None, null)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			PossiblyCaptureContext(ref stackMark);
		}

		/// <summary>
		/// Initializes a new <see cref="T:System.Threading.Tasks.Task" /> with the specified action and state.
		/// </summary>
		/// <param name="action">The delegate that represents the code to execute in the task.</param>
		/// <param name="state">An object representing data to be used by the action.</param>
		/// <exception cref="T:System.ArgumentNullException">
		/// The <paramref name="action" /> argument is null.
		/// </exception>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task(Action<object> action, object state)
			: this(action, state, InternalCurrent, CancellationToken.None, TaskCreationOptions.None, InternalTaskOptions.None, null)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			PossiblyCaptureContext(ref stackMark);
		}

		/// <summary>
		/// Initializes a new <see cref="T:System.Threading.Tasks.Task" /> with the specified action, state, snd options.
		/// </summary>
		/// <param name="action">The delegate that represents the code to execute in the task.</param>
		/// <param name="state">An object representing data to be used by the action.</param>
		/// <param name="cancellationToken">The <see cref="P:System.Threading.Tasks.Task.CancellationToken" /> that will be assigned to the new task.</param>
		/// <exception cref="T:System.ArgumentNullException">
		/// The <paramref name="action" /> argument is null.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken">CancellationToken</see>
		/// has already been disposed.
		/// </exception>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task(Action<object> action, object state, CancellationToken cancellationToken)
			: this(action, state, InternalCurrent, cancellationToken, TaskCreationOptions.None, InternalTaskOptions.None, null)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			PossiblyCaptureContext(ref stackMark);
		}

		/// <summary>
		/// Initializes a new <see cref="T:System.Threading.Tasks.Task" /> with the specified action, state, snd options.
		/// </summary>
		/// <param name="action">The delegate that represents the code to execute in the task.</param>
		/// <param name="state">An object representing data to be used by the action.</param>
		/// <param name="creationOptions">
		/// The <see cref="T:System.Threading.Tasks.TaskCreationOptions">TaskCreationOptions</see> used to
		/// customize the Task's behavior.
		/// </param>
		/// <exception cref="T:System.ArgumentNullException">
		/// The <paramref name="action" /> argument is null.
		/// </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// The <paramref name="creationOptions" /> argument specifies an invalid value for <see cref="T:System.Threading.Tasks.TaskCreationOptions" />.
		/// </exception>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task(Action<object> action, object state, TaskCreationOptions creationOptions)
			: this(action, state, InternalCurrent, CancellationToken.None, creationOptions, InternalTaskOptions.None, null)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			PossiblyCaptureContext(ref stackMark);
		}

		/// <summary>
		/// Initializes a new <see cref="T:System.Threading.Tasks.Task" /> with the specified action, state, snd options.
		/// </summary>
		/// <param name="action">The delegate that represents the code to execute in the task.</param>
		/// <param name="state">An object representing data to be used by the action.</param>
		/// <param name="cancellationToken">The <see cref="P:System.Threading.Tasks.Task.CancellationToken" /> that will be assigned to the new task.</param>
		/// <param name="creationOptions">
		/// The <see cref="T:System.Threading.Tasks.TaskCreationOptions">TaskCreationOptions</see> used to
		/// customize the Task's behavior.
		/// </param>
		/// <exception cref="T:System.ArgumentNullException">
		/// The <paramref name="action" /> argument is null.
		/// </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// The <paramref name="creationOptions" /> argument specifies an invalid value for <see cref="T:System.Threading.Tasks.TaskCreationOptions" />.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken">CancellationToken</see>
		/// has already been disposed.
		/// </exception>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task(Action<object> action, object state, CancellationToken cancellationToken, TaskCreationOptions creationOptions)
			: this(action, state, InternalCurrent, cancellationToken, creationOptions, InternalTaskOptions.None, null)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			PossiblyCaptureContext(ref stackMark);
		}

		internal Task(Action<object> action, object state, Task parent, CancellationToken cancellationToken, TaskCreationOptions creationOptions, InternalTaskOptions internalOptions, TaskScheduler scheduler, ref StackCrawlMark2 stackMark)
			: this(action, state, parent, cancellationToken, creationOptions, internalOptions, scheduler)
		{
			PossiblyCaptureContext(ref stackMark);
		}

		/// <summary>
		/// An internal constructor used by the factory methods on task and its descendent(s).
		/// This variant does not capture the ExecutionContext; it is up to the caller to do that.
		/// </summary>
		/// <param name="action">An action to execute.</param>
		/// <param name="state">Optional state to pass to the action.</param>
		/// <param name="parent">Parent of Task.</param>
		/// <param name="cancellationToken">A CancellationToken for the task.</param>
		/// <param name="scheduler">A task scheduler under which the task will run.</param>
		/// <param name="creationOptions">Options to control its execution.</param>
		/// <param name="internalOptions">Internal options to control its execution</param>
		internal Task(object action, object state, Task parent, CancellationToken cancellationToken, TaskCreationOptions creationOptions, InternalTaskOptions internalOptions, TaskScheduler scheduler)
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			if ((creationOptions & TaskCreationOptions.AttachedToParent) != 0 || (internalOptions & InternalTaskOptions.SelfReplicating) != 0)
			{
				m_parent = parent;
			}
			TaskConstructorCore(action, state, cancellationToken, creationOptions, internalOptions, scheduler);
		}

		/// <summary>
		/// Common logic used by the following internal ctors:
		///     Task()
		///     Task(object action, object state, Task parent, TaskCreationOptions options, TaskScheduler taskScheduler)
		///
		/// ASSUMES THAT m_creatingTask IS ALREADY SET.
		///
		/// </summary>
		/// <param name="action">Action for task to execute.</param>
		/// <param name="state">Object to which to pass to action (may be null)</param>
		/// <param name="scheduler">Task scheduler on which to run thread (only used by continuation tasks).</param>
		/// <param name="cancellationToken">A CancellationToken for the Task.</param>
		/// <param name="creationOptions">Options to customize behavior of Task.</param>
		/// <param name="internalOptions">Internal options to customize behavior of Task.</param>
		internal void TaskConstructorCore(object action, object state, CancellationToken cancellationToken, TaskCreationOptions creationOptions, InternalTaskOptions internalOptions, TaskScheduler scheduler)
		{
			m_action = action;
			m_stateObject = state;
			m_taskScheduler = scheduler;
			if (((uint)creationOptions & 0xFFFFFFF8u) != 0)
			{
				throw new ArgumentOutOfRangeException("creationOptions");
			}
			if (((uint)internalOptions & 0xFFFFD0FFu) != 0)
			{
				throw new ArgumentOutOfRangeException("internalOptions", Environment2.GetResourceString("Task_ctor_IllegalInternalOptions"));
			}
			if ((creationOptions & TaskCreationOptions.LongRunning) != 0 && (internalOptions & InternalTaskOptions.SelfReplicating) != 0)
			{
				throw new InvalidOperationException(Environment2.GetResourceString("Task_ctor_LRandSR"));
			}
			m_stateFlags = (int)creationOptions | (int)internalOptions;
			if (m_action == null || (internalOptions & InternalTaskOptions.ContinuationTask) != 0)
			{
				m_stateFlags |= 33554432;
			}
			if (m_parent != null && (creationOptions & TaskCreationOptions.AttachedToParent) != 0)
			{
				m_parent.AddNewChild();
			}
			if (!cancellationToken.CanBeCanceled)
			{
				return;
			}
			LazyInitializer.EnsureInitialized(ref m_contingentProperties, s_contingentPropertyCreator);
			m_contingentProperties.m_cancellationToken = cancellationToken;
			try
			{
				cancellationToken.ThrowIfSourceDisposed();
				if ((internalOptions & (InternalTaskOptions.PromiseTask | InternalTaskOptions.QueuedByRuntime)) == 0)
				{
					CancellationTokenRegistration value = cancellationToken.InternalRegisterWithoutEC(s_taskCancelCallback, this);
					m_contingentProperties.m_cancellationRegistration = new Shared<CancellationTokenRegistration>(value);
				}
			}
			catch
			{
				if (m_parent != null && (creationOptions & TaskCreationOptions.AttachedToParent) != 0)
				{
					m_parent.DisregardChild();
				}
				throw;
			}
		}

		/// <summary>
		/// Checks if we registered a CT callback during construction, and deregisters it. 
		/// This should be called when we know the registration isn't useful anymore. Specifically from Finish() if the task has completed
		/// successfully or with an exception.
		/// </summary>
		internal void DeregisterCancellationCallback()
		{
			if (m_contingentProperties != null && m_contingentProperties.m_cancellationRegistration != null)
			{
				try
				{
					m_contingentProperties.m_cancellationRegistration.Value.Dispose();
				}
				catch (ObjectDisposedException)
				{
				}
				m_contingentProperties.m_cancellationRegistration = null;
			}
		}

		private static void TaskCancelCallback(object o)
		{
			Task task = (Task)o;
			task.InternalCancel(bCancelNonExecutingOnly: false);
		}

		/// <summary>
		/// Captures the ExecutionContext so long as flow isn't suppressed.
		/// </summary>
		/// <param name="stackMark">A stack crawl mark pointing to the frame of the caller.</param>
		[SecuritySafeCritical]
		internal void PossiblyCaptureContext(ref StackCrawlMark2 stackMark)
		{
			if (!ExecutionContext.IsFlowSuppressed())
			{
				m_capturedContext = ExecutionContext.Capture();
			}
		}

		internal bool AtomicStateUpdate(int newBits, int illegalBits)
		{
			int oldFlags = 0;
			return AtomicStateUpdate(newBits, illegalBits, ref oldFlags);
		}

		internal bool AtomicStateUpdate(int newBits, int illegalBits, ref int oldFlags)
		{
			SpinWait spinWait = default(SpinWait);
			while (true)
			{
				oldFlags = m_stateFlags;
				if ((oldFlags & illegalBits) != 0)
				{
					return false;
				}
				if (Interlocked.CompareExchange(ref m_stateFlags, oldFlags | newBits, oldFlags) == oldFlags)
				{
					break;
				}
				spinWait.SpinOnce();
			}
			return true;
		}

		internal bool MarkStarted()
		{
			return AtomicStateUpdate(65536, 4259840);
		}

		/// <summary>
		/// Internal function that will be called by a new child task to add itself to 
		/// the children list of the parent (this).
		///
		/// Since a child task can only be created from the thread executing the action delegate
		/// of this task, reentrancy is neither required nor supported. This should not be called from
		/// anywhere other than the task construction/initialization codepaths.
		/// </summary>
		internal void AddNewChild()
		{
            _ = LazyInitializer.EnsureInitialized(ref m_contingentProperties, s_contingentPropertyCreator);
			if (m_contingentProperties.m_completionCountdown == 1 && !IsSelfReplicatingRoot)
			{
				m_contingentProperties.m_completionCountdown++;
			}
			else
			{
				Interlocked.Increment(ref m_contingentProperties.m_completionCountdown);
			}
		}

		internal void DisregardChild()
		{
			Interlocked.Decrement(ref m_contingentProperties.m_completionCountdown);
		}

		/// <summary>
		/// Starts the <see cref="T:System.Threading.Tasks.Task" />, scheduling it for execution to the current <see cref="T:System.Threading.Tasks.TaskScheduler">TaskScheduler</see>.
		/// </summary>
		/// <remarks>
		/// A task may only be started and run only once.  Any attempts to schedule a task a second time
		/// will result in an exception.
		/// </remarks>
		/// <exception cref="T:System.InvalidOperationException">
		/// The <see cref="T:System.Threading.Tasks.Task" /> is not in a valid state to be started. It may have already been started,
		/// executed, or canceled, or it may have been created in a manner that doesn't support direct
		/// scheduling.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">
		/// The <see cref="T:System.Threading.Tasks.Task" /> instance has been disposed.
		/// </exception>
		public void Start()
		{
			Start(TaskScheduler.Current);
		}

		/// <summary>
		/// Starts the <see cref="T:System.Threading.Tasks.Task" />, scheduling it for execution to the specified <see cref="T:System.Threading.Tasks.TaskScheduler">TaskScheduler</see>.
		/// </summary>
		/// <remarks>
		/// A task may only be started and run only once. Any attempts to schedule a task a second time will
		/// result in an exception.
		/// </remarks>
		/// <param name="scheduler">
		/// The <see cref="T:System.Threading.Tasks.TaskScheduler">TaskScheduler</see> with which to associate
		/// and execute this task.
		/// </param>
		/// <exception cref="T:System.ArgumentNullException">
		/// The <paramref name="scheduler" /> argument is null.
		/// </exception>
		/// <exception cref="T:System.InvalidOperationException">
		/// The <see cref="T:System.Threading.Tasks.Task" /> is not in a valid state to be started. It may have already been started,
		/// executed, or canceled, or it may have been created in a manner that doesn't support direct
		/// scheduling.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">
		/// The <see cref="T:System.Threading.Tasks.Task" /> instance has been disposed.
		/// </exception>
		public void Start(TaskScheduler scheduler)
		{
			ThrowIfDisposed();
			if (IsCompleted)
			{
				throw new InvalidOperationException(Environment2.GetResourceString("Task_Start_TaskCompleted"));
			}
			if (m_action == null)
			{
				throw new InvalidOperationException(Environment2.GetResourceString("Task_Start_NullAction"));
			}
			if (scheduler == null)
			{
				throw new ArgumentNullException("scheduler");
			}
			if ((Options & (TaskCreationOptions)512) != 0)
			{
				throw new InvalidOperationException(Environment2.GetResourceString("Task_Start_ContinuationTask"));
			}
			if (Interlocked.CompareExchange(ref m_taskScheduler, scheduler, null) != null)
			{
				throw new InvalidOperationException(Environment2.GetResourceString("Task_Start_AlreadyStarted"));
			}
			ScheduleAndStart(needsProtection: true);
		}

		/// <summary>
		/// Runs the <see cref="T:System.Threading.Tasks.Task" /> synchronously on the current <see cref="T:System.Threading.Tasks.TaskScheduler">TaskScheduler</see>.
		/// </summary>
		/// <remarks>
		/// <para>
		/// A task may only be started and run only once. Any attempts to schedule a task a second time will
		/// result in an exception.
		/// </para>
		/// <para>
		/// Tasks executed with <see cref="M:System.Threading.Tasks.Task.RunSynchronously" /> will be associated with the current <see cref="T:System.Threading.Tasks.TaskScheduler">TaskScheduler</see>.
		/// </para>
		/// <para>
		/// If the target scheduler does not support running this Task on the current thread, the Task will
		/// be scheduled for execution on the scheduler, and the current thread will block until the
		/// Task has completed execution.
		/// </para>
		/// </remarks>
		/// <exception cref="T:System.InvalidOperationException">
		/// The <see cref="T:System.Threading.Tasks.Task" /> is not in a valid state to be started. It may have already been started,
		/// executed, or canceled, or it may have been created in a manner that doesn't support direct
		/// scheduling.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">
		/// The <see cref="T:System.Threading.Tasks.Task" /> instance has been disposed.
		/// </exception>
		public void RunSynchronously()
		{
			InternalRunSynchronously(TaskScheduler.Current);
		}

		/// <summary>
		/// Runs the <see cref="T:System.Threading.Tasks.Task" /> synchronously on the <see cref="T:System.Threading.Tasks.TaskScheduler">scheduler</see> provided.
		/// </summary>
		/// <remarks>
		/// <para>
		/// A task may only be started and run only once. Any attempts to schedule a task a second time will
		/// result in an exception.
		/// </para>
		/// <para>
		/// If the target scheduler does not support running this Task on the current thread, the Task will
		/// be scheduled for execution on the scheduler, and the current thread will block until the
		/// Task has completed execution.
		/// </para>
		/// </remarks>
		/// <exception cref="T:System.InvalidOperationException">
		/// The <see cref="T:System.Threading.Tasks.Task" /> is not in a valid state to be started. It may have already been started,
		/// executed, or canceled, or it may have been created in a manner that doesn't support direct
		/// scheduling.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">
		/// The <see cref="T:System.Threading.Tasks.Task" /> instance has been disposed.
		/// </exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="scheduler" /> parameter
		/// is null.</exception>
		/// <param name="scheduler">The scheduler on which to attempt to run this task inline.</param>
		public void RunSynchronously(TaskScheduler scheduler)
		{
			if (scheduler == null)
			{
				throw new ArgumentNullException("scheduler");
			}
			InternalRunSynchronously(scheduler);
		}

		[SecuritySafeCritical]
		internal void InternalRunSynchronously(TaskScheduler scheduler)
		{
			ThrowIfDisposed();
			if ((Options & (TaskCreationOptions)512) != 0)
			{
				throw new InvalidOperationException(Environment2.GetResourceString("Task_RunSynchronously_Continuation"));
			}
			if (IsCompleted)
			{
				throw new InvalidOperationException(Environment2.GetResourceString("Task_RunSynchronously_TaskCompleted"));
			}
			if (m_action == null)
			{
				throw new InvalidOperationException(Environment2.GetResourceString("Task_RunSynchronously_Promise"));
			}
			if (Interlocked.CompareExchange(ref m_taskScheduler, scheduler, null) != null)
			{
				throw new InvalidOperationException(Environment2.GetResourceString("Task_RunSynchronously_AlreadyStarted"));
			}
			if (MarkStarted())
			{
				bool flag = false;
				try
				{
					if (!scheduler.TryRunInline(this, taskWasPreviouslyQueued: false))
					{
						scheduler.QueueTask(this);
						flag = true;
					}
					if (!IsCompleted)
					{
						CompletedEvent.Wait();
					}
				}
				catch (Exception ex)
				{
					if (!flag && !(ex is ThreadAbortException))
					{
						TaskSchedulerException ex2 = new TaskSchedulerException(ex);
						AddException(ex2);
						Finish(bUserDelegateExecuted: false);
						m_contingentProperties.m_exceptionsHolder.MarkAsHandled(calledFromFinalizer: false);
						throw ex2;
					}
					throw;
				}
				return;
			}
			throw new InvalidOperationException(Environment2.GetResourceString("Task_RunSynchronously_TaskCompleted"));
		}

		internal static Task InternalStartNew(Task creatingTask, object action, object state, CancellationToken cancellationToken, TaskScheduler scheduler, TaskCreationOptions options, InternalTaskOptions internalOptions, ref StackCrawlMark2 stackMark)
		{
			if (scheduler == null)
			{
				throw new ArgumentNullException("scheduler");
			}
			Task task = new Task(action, state, creatingTask, cancellationToken, options, internalOptions | InternalTaskOptions.QueuedByRuntime, scheduler);
			task.PossiblyCaptureContext(ref stackMark);
			task.ScheduleAndStart(needsProtection: false);
			return task;
		}

		internal static Task InternalStartNew(Task creatingTask, object action, object state, TaskScheduler scheduler, TaskCreationOptions options, InternalTaskOptions internalOptions, ExecutionContext context)
		{
			if (scheduler == null)
			{
				throw new ArgumentNullException("scheduler");
			}
			Task task = new Task(action, state, creatingTask, CancellationToken.None, options, internalOptions | InternalTaskOptions.QueuedByRuntime, scheduler);
			task.m_capturedContext = context;
			task.ScheduleAndStart(needsProtection: false);
			return task;
		}

		private static ContingentProperties ContingentPropertyCreator()
		{
			return new ContingentProperties();
		}

		/// <summary>
		/// Throws an exception if the task has been disposed, and hence can no longer be accessed.
		/// </summary>
		/// <exception cref="T:System.ObjectDisposedException">The task has been disposed.</exception>
		internal void ThrowIfDisposed()
		{
			if (IsDisposed)
			{
				throw new ObjectDisposedException(null, Environment2.GetResourceString("Task_ThrowIfDisposed"));
			}
		}

		/// <summary>
		/// Sets the internal completion event.
		/// </summary>
		private void SetCompleted()
		{
			m_completionEvent?.Set();
		}

		/// <summary>
		/// Disposes the <see cref="T:System.Threading.Tasks.Task" />, releasing all of its unmanaged resources.  
		/// </summary>
		/// <remarks>
		/// Unlike most of the members of <see cref="T:System.Threading.Tasks.Task" />, this method is not thread-safe.
		/// Also, <see cref="M:System.Threading.Tasks.Task.Dispose" /> may only be called on a <see cref="T:System.Threading.Tasks.Task" /> that is in one of
		/// the final states: <see cref="F:System.Threading.Tasks.TaskStatus.RanToCompletion">RanToCompletion</see>,
		/// <see cref="F:System.Threading.Tasks.TaskStatus.Faulted">Faulted</see>, or
		/// <see cref="F:System.Threading.Tasks.TaskStatus.Canceled">Canceled</see>.
		/// </remarks>
		/// <exception cref="T:System.InvalidOperationException">
		/// The exception that is thrown if the <see cref="T:System.Threading.Tasks.Task" /> is not in 
		/// one of the final states: <see cref="F:System.Threading.Tasks.TaskStatus.RanToCompletion">RanToCompletion</see>,
		/// <see cref="F:System.Threading.Tasks.TaskStatus.Faulted">Faulted</see>, or
		/// <see cref="F:System.Threading.Tasks.TaskStatus.Canceled">Canceled</see>.
		/// </exception>        
		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Disposes the <see cref="T:System.Threading.Tasks.Task" />, releasing all of its unmanaged resources.  
		/// </summary>
		/// <param name="disposing">
		/// A Boolean value that indicates whether this method is being called due to a call to <see cref="M:System.Threading.Tasks.Task.Dispose" />.
		/// </param>
		/// <remarks>
		/// Unlike most of the members of <see cref="T:System.Threading.Tasks.Task" />, this method is not thread-safe.
		/// </remarks>
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (!IsCompleted)
				{
					throw new InvalidOperationException(Environment2.GetResourceString("Task_Dispose_NotCompleted"));
				}
				ManualResetEventSlim completionEvent = m_completionEvent;
				if (completionEvent != null)
				{
					if (!completionEvent.IsSet)
					{
						completionEvent.Set();
					}
					completionEvent.Dispose();
					m_completionEvent = null;
				}
			}
			m_stateFlags |= 262144;
		}

		/// <summary>
		/// Schedules the task for execution.
		/// </summary>
		/// <param name="needsProtection">If true, TASK_STATE_STARTED bit is turned on in
		/// an atomic fashion, making sure that TASK_STATE_CANCELED does not get set
		/// underneath us.  If false, TASK_STATE_STARTED bit is OR-ed right in.  This
		/// allows us to streamline things a bit for StartNew(), where competing cancellations
		/// are not a problem.</param>
		[SecuritySafeCritical]
		internal void ScheduleAndStart(bool needsProtection)
		{
			if (needsProtection)
			{
				if (!MarkStarted())
				{
					return;
				}
			}
			else
			{
				m_stateFlags |= 65536;
			}
			try
			{
				m_taskScheduler.QueueTask(this);
			}
			catch (ThreadAbortException exceptionObject)
			{
				AddException(exceptionObject);
				FinishThreadAbortedTask(bTAEAddedToExceptionHolder: true, delegateRan: false);
			}
			catch (Exception innerException)
			{
				TaskSchedulerException ex = new TaskSchedulerException(innerException);
				AddException(ex);
				Finish(bUserDelegateExecuted: false);
				if ((Options & (TaskCreationOptions)512) == 0)
				{
					m_contingentProperties.m_exceptionsHolder.MarkAsHandled(calledFromFinalizer: false);
				}
				throw ex;
			}
		}

		/// <summary>
		/// Adds an exception to the list of exceptions this task has thrown.
		/// </summary>
		/// <param name="exceptionObject">An object representing either an Exception or a collection of Exceptions.</param>
		internal void AddException(object exceptionObject)
		{
            _ = LazyInitializer.EnsureInitialized(ref m_contingentProperties, s_contingentPropertyCreator);
			if (m_contingentProperties.m_exceptionsHolder == null)
			{
				TaskExceptionHolder taskExceptionHolder = new TaskExceptionHolder(this);
				if (Interlocked.CompareExchange(ref m_contingentProperties.m_exceptionsHolder, taskExceptionHolder, null) != null)
				{
					taskExceptionHolder.MarkAsHandled(calledFromFinalizer: false);
				}
			}
			lock (m_contingentProperties)
			{
				m_contingentProperties.m_exceptionsHolder.Add(exceptionObject);
			}
		}

		/// <summary>
		/// Returns a list of exceptions by aggregating the holder's contents. Or null if
		/// no exceptions have been thrown.
		/// </summary>
		/// <param name="includeTaskCanceledExceptions">Whether to include a TCE if cancelled.</param>
		/// <returns>An aggregate exception, or null if no exceptions have been caught.</returns>
		private AggregateException GetExceptions(bool includeTaskCanceledExceptions)
		{
			Exception ex = null;
			if (includeTaskCanceledExceptions && IsCanceled)
			{
				ex = new TaskCanceledException(this);
			}
			if (ExceptionRecorded)
			{
				return m_contingentProperties.m_exceptionsHolder.CreateExceptionObject(calledFromFinalizer: false, ex);
			}
			if (ex != null)
			{
				return new AggregateException(ex);
			}
			return null;
		}

		/// <summary>
		/// Throws an aggregate exception if the task contains exceptions. 
		/// </summary>
		internal void ThrowIfExceptional(bool includeTaskCanceledExceptions)
		{
			Exception exceptions = GetExceptions(includeTaskCanceledExceptions);
			if (exceptions != null)
			{
				UpdateExceptionObservedStatus();
				throw exceptions;
			}
		}

		/// <summary>
		/// Checks whether this is an attached task, and whether we are being called by the parent task.
		/// And sets the TASK_STATE_EXCEPTIONOBSERVEDBYPARENT status flag based on that.
		///
		/// This is meant to be used internally when throwing an exception, and when WaitAll is gathering 
		/// exceptions for tasks it waited on. If this flag gets set, the implicit wait on children 
		/// will skip exceptions to prevent duplication.
		///
		/// This should only be called when this task has completed with an exception
		///
		/// </summary>
		internal void UpdateExceptionObservedStatus()
		{
			if ((Options & TaskCreationOptions.AttachedToParent) != 0 && InternalCurrent == m_parent)
			{
				m_stateFlags |= 524288;
			}
		}

		/// <summary>
		/// Signals completion of this particular task.
		///
		/// The bUserDelegateExecuted parameter indicates whether this Finish() call comes following the
		/// full execution of the user delegate. 
		///
		/// If bUserDelegateExecuted is false, it mean user delegate wasn't invoked at all (either due to
		/// a cancellation request, or because this task is a promise style Task). In this case, the steps
		/// involving child tasks (i.e. WaitForChildren) will be skipped.
		///
		/// </summary>
		internal void Finish(bool bUserDelegateExecuted)
		{
			if (!bUserDelegateExecuted)
			{
				FinishStageTwo();
			}
			else
			{
				ContingentProperties contingentProperties = m_contingentProperties;
				if (contingentProperties == null || (contingentProperties.m_completionCountdown == 1 && !IsSelfReplicatingRoot) || Interlocked.Decrement(ref contingentProperties.m_completionCountdown) == 0)
				{
					FinishStageTwo();
				}
				else
				{
					AtomicStateUpdate(8388608, 23068672);
				}
			}
			List<Task> list = ((m_contingentProperties != null) ? m_contingentProperties.m_exceptionalChildren : null);
			if (list != null)
			{
				lock (list)
				{
					list.RemoveAll(s_IsExceptionObservedByParentPredicate);
				}
			}
		}

		/// <summary>
		/// FinishStageTwo is to be executed as soon as we known there are no more children to complete. 
		/// It can happen i) either on the thread that originally executed this task (if no children were spawned, or they all completed by the time this task's delegate quit)
		///              ii) or on the thread that executed the last child.
		/// </summary>
		internal void FinishStageTwo()
		{
			AddExceptionsFromChildren();
			int num = (ExceptionRecorded ? 2097152 : ((!IsCancellationRequested || !IsCancellationAcknowledged) ? 16777216 : 4194304));
			Interlocked.Exchange(ref m_stateFlags, m_stateFlags | num);
			SetCompleted();
			DeregisterCancellationCallback();
			FinishStageThree();
		}

		/// <summary>
		/// Final stage of the task completion code path. Notifies the parent (if any) that another of its childre are done, and runs continuations.
		/// This function is only separated out from FinishStageTwo because these two operations are also needed to be called from CancellationCleanupLogic()
		/// </summary>
		private void FinishStageThree()
		{
			if (m_parent != null && ((uint)m_stateFlags & 0xFFFFu & 4u) != 0)
			{
				m_parent.ProcessChildCompletion(this);
			}
			FinishContinuations();
			m_action = null;
		}

		/// <summary>
		/// This is called by children of this task when they are completed.
		/// </summary>
		internal void ProcessChildCompletion(Task childTask)
		{
			if (childTask.IsFaulted && !childTask.IsExceptionObservedByParent)
			{
				if (m_contingentProperties.m_exceptionalChildren == null)
				{
					Interlocked.CompareExchange(ref m_contingentProperties.m_exceptionalChildren, new List<Task>(), null);
				}
				List<Task> exceptionalChildren = m_contingentProperties.m_exceptionalChildren;
				if (exceptionalChildren != null)
				{
					lock (exceptionalChildren)
					{
						exceptionalChildren.Add(childTask);
					}
				}
			}
			if (Interlocked.Decrement(ref m_contingentProperties.m_completionCountdown) == 0)
			{
				FinishStageTwo();
			}
		}

		/// <summary>
		/// This is to be called just before the task does its final state transition. 
		/// It traverses the list of exceptional children, and appends their aggregate exceptions into this one's exception list
		/// </summary>
		internal void AddExceptionsFromChildren()
		{
			List<Task> list = ((m_contingentProperties != null) ? m_contingentProperties.m_exceptionalChildren : null);
			if (list == null)
			{
				return;
			}
			lock (list)
			{
				foreach (Task item in list)
				{
					if (item.IsFaulted && !item.IsExceptionObservedByParent)
					{
						TaskExceptionHolder exceptionsHolder = item.m_contingentProperties.m_exceptionsHolder;
						AddException(exceptionsHolder.CreateExceptionObject(calledFromFinalizer: false, null));
					}
				}
			}
			m_contingentProperties.m_exceptionalChildren = null;
		}

		/// <summary>
		/// Special purpose Finish() entry point to be used when the task delegate throws a ThreadAbortedException
		/// This makes a note in the state flags so that we avoid any costly synchronous operations in the finish codepath
		/// such as inlined continuations
		/// </summary>
		/// <param name="bTAEAddedToExceptionHolder">
		/// Indicates whether the ThreadAbortException was added to this task's exception holder. 
		/// This should always be true except for the case of non-root self replicating task copies.
		/// </param>
		/// <param name="delegateRan">Whether the delegate was executed.</param>
		internal void FinishThreadAbortedTask(bool bTAEAddedToExceptionHolder, bool delegateRan)
		{
			if (bTAEAddedToExceptionHolder)
			{
				m_contingentProperties.m_exceptionsHolder.MarkAsHandled(calledFromFinalizer: false);
			}
			if (AtomicStateUpdate(134217728, 157286400))
			{
				Finish(delegateRan);
			}
		}

		/// <summary>
		/// Executes the task. This method will only be called once, and handles bookeeping associated with
		/// self-replicating tasks, in addition to performing necessary exception marshaling.
		/// </summary>
		/// <exception cref="T:System.ObjectDisposedException">The task has already been disposed.</exception>
		private void Execute()
		{
			if (IsSelfReplicatingRoot)
			{
				ExecuteSelfReplicating(this);
				return;
			}
			try
			{
				InnerInvoke();
			}
			catch (ThreadAbortException unhandledException)
			{
				if (!IsChildReplica)
				{
					HandleException(unhandledException);
					FinishThreadAbortedTask(bTAEAddedToExceptionHolder: true, delegateRan: true);
				}
			}
			catch (Exception unhandledException2)
			{
				HandleException(unhandledException2);
			}
		}

		internal virtual bool ShouldReplicate()
		{
			return true;
		}

		internal virtual Task CreateReplicaTask(Action<object> taskReplicaDelegate, object stateObject, Task parentTask, TaskScheduler taskScheduler, TaskCreationOptions creationOptionsForReplica, InternalTaskOptions internalOptionsForReplica)
		{
			return new Task(taskReplicaDelegate, stateObject, parentTask, CancellationToken.None, creationOptionsForReplica, internalOptionsForReplica, parentTask.ExecutingTaskScheduler);
		}

		private static void ExecuteSelfReplicating(Task root)
		{
			TaskCreationOptions creationOptionsForReplicas = root.CreationOptions | TaskCreationOptions.AttachedToParent;
			InternalTaskOptions internalOptionsForReplicas = InternalTaskOptions.ChildReplica | InternalTaskOptions.SelfReplicating | InternalTaskOptions.QueuedByRuntime;
			bool replicasAreQuitting = false;
			Action<object> taskReplicaDelegate = null;
			taskReplicaDelegate = delegate
			{
				Task internalCurrent = InternalCurrent;
				Task task = internalCurrent.HandedOverChildReplica;
				if (task == null)
				{
					if (!root.ShouldReplicate() || replicasAreQuitting)
					{
						return;
					}
					ExecutionContext capturedContext = root.m_capturedContext;
					task = root.CreateReplicaTask(taskReplicaDelegate, root.m_stateObject, root, root.ExecutingTaskScheduler, creationOptionsForReplicas, internalOptionsForReplicas);
					task.m_capturedContext = capturedContext?.CreateCopy();
					task.ScheduleAndStart(needsProtection: false);
				}
				try
				{
					root.InnerInvokeWithArg(internalCurrent);
				}
				catch (Exception ex)
				{
					root.HandleException(ex);
					if (ex is ThreadAbortException)
					{
						internalCurrent.FinishThreadAbortedTask(bTAEAddedToExceptionHolder: false, delegateRan: true);
					}
				}
				object savedStateForNextReplica = internalCurrent.SavedStateForNextReplica;
				if (savedStateForNextReplica != null)
				{
					Task task2 = root.CreateReplicaTask(taskReplicaDelegate, root.m_stateObject, root, root.ExecutingTaskScheduler, creationOptionsForReplicas, internalOptionsForReplicas);
					task2.m_capturedContext = root.m_capturedContext?.CreateCopy();
					task2.HandedOverChildReplica = task;
					task2.SavedStateFromPreviousReplica = savedStateForNextReplica;
					task2.ScheduleAndStart(needsProtection: false);
				}
				else
				{
					replicasAreQuitting = true;
					try
					{
						task.InternalCancel(bCancelNonExecutingOnly: true);
					}
					catch (Exception unhandledException)
					{
						root.HandleException(unhandledException);
					}
				}
			};
			taskReplicaDelegate(null);
		}

		/// <summary>
		/// IThreadPoolWorkItem override, which is the entry function for this task when the TP scheduler decides to run it.
		///
		/// </summary>
		[SecurityCritical]
		void IThreadPoolWorkItem.ExecuteWorkItem()
		{
			ExecuteEntry(bPreventDoubleExecution: false);
		}

		/// <summary>
		/// The ThreadPool calls this if a ThreadAbortException is thrown while trying to execute this workitem.  This may occur
		/// before Task would otherwise be able to observe it.  
		/// </summary>
		[SecurityCritical]
		void IThreadPoolWorkItem.MarkAborted(ThreadAbortException tae)
		{
			if (!IsCompleted)
			{
				HandleException(tae);
				FinishThreadAbortedTask(bTAEAddedToExceptionHolder: true, delegateRan: false);
			}
		}

		/// <summary>
		/// Outermost entry function to execute this task. Handles all aspects of executing a task on the caller thread.
		/// Currently this is called by IThreadPoolWorkItem.ExecuteWorkItem(), and TaskManager.TryExecuteInline. 
		///
		/// </summary>
		/// <param name="bPreventDoubleExecution"> Performs atomic updates to prevent double execution. Should only be set to true
		/// in codepaths servicing user provided TaskSchedulers. The ConcRT or ThreadPool schedulers don't need this. </param>
		[SecuritySafeCritical]
		internal bool ExecuteEntry(bool bPreventDoubleExecution)
		{
			if (bPreventDoubleExecution)
			{
				int oldFlags = 0;
				if (!AtomicStateUpdate(131072, 131072, ref oldFlags) && (oldFlags & 0x400000) == 0)
				{
					return false;
				}
			}
			else
			{
				m_stateFlags |= 131072;
			}
			if (!IsCancellationRequested && !IsCanceled)
			{
				ExecuteWithThreadLocal(ref ThreadLocals.s_currentTask);
			}
			else if (!IsCanceled)
			{
				int num = Interlocked.Exchange(ref m_stateFlags, m_stateFlags | 0x400000);
				if ((num & 0x400000) == 0)
				{
					CancellationCleanupLogic();
				}
			}
			return true;
		}

		[SecurityCritical]
		private void ExecuteWithThreadLocal(ref Task currentTaskSlot)
		{
			Task task = currentTaskSlot;
			try
			{
				currentTaskSlot = this;
				ExecutionContext capturedContext = m_capturedContext;
				if (capturedContext == null)
				{
					Execute();
				}
				else
				{
					if (IsSelfReplicatingRoot || IsChildReplica)
					{
						m_capturedContext = capturedContext.CreateCopy();
					}
					ExecutionContext.Run(capturedContext, s_ecCallback, this);
				}
				Finish(bUserDelegateExecuted: true);
			}
			finally
			{
				currentTaskSlot = task;
			}
		}

		[SecurityCritical]
		private static void ExecutionContextCallback(object obj)
		{
			Task task = obj as Task;
			task.Execute();
		}

		/// <summary>
		/// The actual code which invokes the body of the task. This can be overriden in derived types.
		/// </summary>
		internal void InnerInvoke()
		{
			Action action = m_action as Action;
			if (action != null)
			{
				action();
				return;
			}
			Action<object> action2 = m_action as Action<object>;
			action2(m_stateObject);
		}

		/// <summary>
		/// Alternate InnerInvoke prototype to be called from ExecuteSelfReplicating() so that
		/// the Parallel Debugger can discover the actual task being invoked. 
		/// Details: Here, InnerInvoke is actually being called on the rootTask object while we are actually executing the
		/// childTask. And the debugger needs to discover the childTask, so we pass that down as an argument.
		/// The NoOptimization and NoInlining flags ensure that the childTask pointer is retained, and that this
		/// function appears on the callstack.
		/// </summary>
		/// <param name="childTask"></param>
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
		internal void InnerInvokeWithArg(Task childTask)
		{
			InnerInvoke();
		}

		/// <summary>
		/// Performs whatever handling is necessary for an unhandled exception. Normally
		/// this just entails adding the exception to the holder object. 
		/// </summary>
		/// <param name="unhandledException">The exception that went unhandled.</param>
		private void HandleException(Exception unhandledException)
		{
			OperationCanceledException2 operationCanceledException = unhandledException as OperationCanceledException2;
			if (operationCanceledException != null && IsCancellationRequested && m_contingentProperties.m_cancellationToken == operationCanceledException.CancellationToken)
			{
				SetCancellationAcknowledged();
			}
			else
			{
				AddException(unhandledException);
			}
		}

		/// <summary>
		/// Waits for the <see cref="T:System.Threading.Tasks.Task" /> to complete execution.
		/// </summary>
		/// <exception cref="T:System.AggregateException">
		/// The <see cref="T:System.Threading.Tasks.Task" /> was canceled -or- an exception was thrown during
		/// the execution of the <see cref="T:System.Threading.Tasks.Task" />.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">
		/// The <see cref="T:System.Threading.Tasks.Task" /> has been disposed.
		/// </exception>
		public void Wait()
		{
			Wait(-1, CancellationToken.None);
		}

		/// <summary>
		/// Waits for the <see cref="T:System.Threading.Tasks.Task" /> to complete execution.
		/// </summary>
		/// <param name="timeout">
		/// A <see cref="T:System.TimeSpan" /> that represents the number of milliseconds to wait, or a <see cref="T:System.TimeSpan" /> that represents -1 milliseconds to wait indefinitely.
		/// </param>
		/// <returns>
		/// true if the <see cref="T:System.Threading.Tasks.Task" /> completed execution within the allotted time; otherwise, false.
		/// </returns>
		/// <exception cref="T:System.AggregateException">
		/// The <see cref="T:System.Threading.Tasks.Task" /> was canceled -or- an exception was thrown during the execution of the <see cref="T:System.Threading.Tasks.Task" />.
		/// </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// <paramref name="timeout" /> is a negative number other than -1 milliseconds, which represents an
		/// infinite time-out -or- timeout is greater than
		/// <see cref="F:System.Int32.MaxValue" />.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">
		/// The <see cref="T:System.Threading.Tasks.Task" /> has been disposed.
		/// </exception>
		public bool Wait(TimeSpan timeout)
		{
			long num = (long)timeout.TotalMilliseconds;
			if (num < -1 || num > int.MaxValue)
			{
				throw new ArgumentOutOfRangeException("timeout");
			}
			return Wait((int)num, CancellationToken.None);
		}

		/// <summary>
		/// Waits for the <see cref="T:System.Threading.Tasks.Task" /> to complete execution.
		/// </summary>
		/// <param name="cancellationToken">
		/// A <see cref="P:System.Threading.Tasks.Task.CancellationToken" /> to observe while waiting for the task to complete.
		/// </param>
		/// <exception cref="T:System.OperationCanceledException">
		/// The <paramref name="cancellationToken" /> was canceled.
		/// </exception>
		/// <exception cref="T:System.AggregateException">
		/// The <see cref="T:System.Threading.Tasks.Task" /> was canceled -or- an exception was thrown during the execution of the <see cref="T:System.Threading.Tasks.Task" />.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">
		/// The <see cref="T:System.Threading.Tasks.Task" />
		/// has been disposed.
		/// </exception>
		public void Wait(CancellationToken cancellationToken)
		{
			Wait(-1, cancellationToken);
		}

		/// <summary>
		/// Waits for the <see cref="T:System.Threading.Tasks.Task" /> to complete execution.
		/// </summary>
		/// <param name="millisecondsTimeout">
		/// The number of milliseconds to wait, or <see cref="F:System.Threading.Timeout.Infinite" /> (-1) to
		/// wait indefinitely.</param>
		/// <returns>true if the <see cref="T:System.Threading.Tasks.Task" /> completed execution within the allotted time; otherwise,
		/// false.
		/// </returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// <paramref name="millisecondsTimeout" /> is a negative number other than -1, which represents an
		/// infinite time-out.
		/// </exception>
		/// <exception cref="T:System.AggregateException">
		/// The <see cref="T:System.Threading.Tasks.Task" /> was canceled -or- an exception was thrown during the execution of the <see cref="T:System.Threading.Tasks.Task" />.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">
		/// The <see cref="T:System.Threading.Tasks.Task" />
		/// has been disposed.
		/// </exception>
		public bool Wait(int millisecondsTimeout)
		{
			return Wait(millisecondsTimeout, CancellationToken.None);
		}

		/// <summary>
		/// Waits for the <see cref="T:System.Threading.Tasks.Task" /> to complete execution.
		/// </summary>
		/// <param name="millisecondsTimeout">
		/// The number of milliseconds to wait, or <see cref="F:System.Threading.Timeout.Infinite" /> (-1) to
		/// wait indefinitely.
		/// </param>
		/// <param name="cancellationToken">
		/// A <see cref="P:System.Threading.Tasks.Task.CancellationToken" /> to observe while waiting for the task to complete.
		/// </param>
		/// <returns>
		/// true if the <see cref="T:System.Threading.Tasks.Task" /> completed execution within the allotted time; otherwise, false.
		/// </returns>
		/// <exception cref="T:System.AggregateException">
		/// The <see cref="T:System.Threading.Tasks.Task" /> was canceled -or- an exception was thrown during the execution of the <see cref="T:System.Threading.Tasks.Task" />.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">
		/// The <see cref="T:System.Threading.Tasks.Task" />
		/// has been disposed.
		/// </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// <paramref name="millisecondsTimeout" /> is a negative number other than -1, which represents an
		/// infinite time-out.
		/// </exception>
		/// <exception cref="T:System.OperationCanceledException">
		/// The <paramref name="cancellationToken" /> was canceled.
		/// </exception>
		public bool Wait(int millisecondsTimeout, CancellationToken cancellationToken)
		{
			ThrowIfDisposed();
			if (millisecondsTimeout < -1)
			{
				throw new ArgumentOutOfRangeException("millisecondsTimeout");
			}
			if (CompletedSuccessfully)
			{
				return true;
			}
			if (!InternalWait(millisecondsTimeout, cancellationToken))
			{
				return false;
			}
			ThrowIfExceptional(includeTaskCanceledExceptions: true);
			return true;
		}

		private bool WrappedTryRunInline()
		{
			if (m_taskScheduler == null)
			{
				return false;
			}
			try
			{
				return m_taskScheduler.TryRunInline(this, taskWasPreviouslyQueued: true);
			}
			catch (Exception ex)
			{
				if (!(ex is ThreadAbortException))
				{
					TaskSchedulerException ex2 = new TaskSchedulerException(ex);
					throw ex2;
				}
				throw;
			}
		}

		private bool WrappedTryRunInline(TaskScheduler currentScheduler, object currentSchedulerStatics)
		{
			if (m_taskScheduler == null)
			{
				return false;
			}
			try
			{
				if (currentScheduler == m_taskScheduler)
				{
					return currentScheduler.TryRunInline(this, taskWasPreviouslyQueued: true, currentSchedulerStatics);
				}
				return m_taskScheduler.TryRunInline(this, taskWasPreviouslyQueued: true);
			}
			catch (Exception ex)
			{
				if (!(ex is ThreadAbortException))
				{
					TaskSchedulerException ex2 = new TaskSchedulerException(ex);
					throw ex2;
				}
				throw;
			}
		}

		/// <summary>
		/// The core wait function, which is only accesible internally. It's meant to be used in places in TPL code where 
		/// the current context is known or cached.
		/// </summary>
		[MethodImpl(MethodImplOptions.NoOptimization)]
		internal bool InternalWait(int millisecondsTimeout, CancellationToken cancellationToken)
		{
			bool flag = IsCompleted;
			if (!flag)
			{
				flag = (millisecondsTimeout == -1 && !cancellationToken.CanBeCanceled && WrappedTryRunInline() && IsCompleted) || CompletedEvent.Wait(millisecondsTimeout, cancellationToken);
			}
			return flag;
		}

		/// <summary>
		/// Cancels the <see cref="T:System.Threading.Tasks.Task" />.
		/// </summary>
		/// <param name="bCancelNonExecutingOnly"> Indiactes whether we should only cancel non-invoked tasks.
		/// For the default scheduler this option will only be serviced through TryDequeue.
		/// For custom schedulers we also attempt an atomic state transition.</param>
		/// <returns>true if the task was successfully canceled; otherwise, false.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.Tasks.Task" />
		/// has been disposed.</exception>
		[SecuritySafeCritical]
		internal bool InternalCancel(bool bCancelNonExecutingOnly)
		{
			ThrowIfDisposed();
			bool flag = false;
			bool flag2 = false;
			TaskSchedulerException ex = null;
			if (((uint)m_stateFlags & 0x10000u) != 0)
			{
				TaskScheduler taskScheduler = m_taskScheduler;
				try
				{
					flag = taskScheduler?.TryDequeue(this) ?? false;
				}
				catch (Exception ex2)
				{
					if (!(ex2 is ThreadAbortException))
					{
						ex = new TaskSchedulerException(ex2);
					}
				}
				if (!flag && bCancelNonExecutingOnly && taskScheduler != null && taskScheduler.RequiresAtomicStartTransition)
				{
					flag2 = AtomicStateUpdate(4194304, 4325376);
				}
			}
			if (!bCancelNonExecutingOnly || flag || flag2)
			{
				RecordInternalCancellationRequest();
				if (flag)
				{
					flag2 = AtomicStateUpdate(4194304, 4325376);
				}
				else if (!flag2 && (m_stateFlags & 0x10000) == 0)
				{
					flag2 = AtomicStateUpdate(4194304, 23265280);
				}
				if (flag2)
				{
					CancellationCleanupLogic();
				}
			}
			if (ex != null)
			{
				throw ex;
			}
			return flag2;
		}

		internal void RecordInternalCancellationRequest()
		{
            _ = LazyInitializer.EnsureInitialized(ref m_contingentProperties, s_contingentPropertyCreator);
			m_contingentProperties.m_internalCancellationRequested = CANCELLATION_REQUESTED;
		}

		internal void CancellationCleanupLogic()
		{
			Interlocked.Exchange(ref m_stateFlags, m_stateFlags | 0x400000);
			SetCompleted();
			FinishStageThree();
		}

		/// <summary>
		/// Sets the task's cancellation acknowledged flag.
		/// </summary>    
		private void SetCancellationAcknowledged()
		{
			m_stateFlags |= 1048576;
		}

		/// <summary>
		/// Runs all of the continuations, as appropriate.
		/// </summary>
		private void FinishContinuations()
		{
			List<TaskContinuation> list = ((m_contingentProperties == null) ? null : m_contingentProperties.m_continuations);
			if (list == null)
			{
				return;
			}
			lock (m_contingentProperties)
			{
			}
			bool bCanInlineContinuationTask = (m_stateFlags & 0x8000000) == 0 && Thread.CurrentThread.ThreadState != ThreadState.AbortRequested;
			int num = -1;
			for (int num2 = list.Count - 1; num2 >= 0; num2--)
			{
				TaskContinuation taskContinuation = list[num2];
				if (taskContinuation.m_taskScheduler != null && (taskContinuation.m_options & TaskContinuationOptions.ExecuteSynchronously) == 0)
				{
					taskContinuation.Run(this, bCanInlineContinuationTask);
				}
				else
				{
					num = num2;
				}
			}
			if (num > -1)
			{
				for (int i = num; i < list.Count; i++)
				{
					TaskContinuation taskContinuation2 = list[i];
					if (taskContinuation2.m_taskScheduler == null || (taskContinuation2.m_options & TaskContinuationOptions.ExecuteSynchronously) != 0)
					{
						taskContinuation2.Run(this, bCanInlineContinuationTask);
					}
				}
			}
			m_contingentProperties.m_continuations = null;
		}

		/// <summary>
		/// Helper function to determine whether the current task is in the state desired by the
		/// continuation kind under evaluation. Three possibilities exist: the task failed with
		/// an unhandled exception (OnFailed), the task was canceled before running (OnAborted),
		/// or the task completed successfully (OnCompletedSuccessfully).  Note that the last
		/// one includes completing due to cancellation.
		/// </summary>
		/// <param name="options">The continuation options under evaluation.</param>
		/// <returns>True if the continuation should be run given the task's current state.</returns>
		internal bool ContinueWithIsRightKind(TaskContinuationOptions options)
		{
			if (IsFaulted)
			{
				return (options & TaskContinuationOptions.NotOnFaulted) == 0;
			}
			if (IsCanceled)
			{
				return (options & TaskContinuationOptions.NotOnCanceled) == 0;
			}
			return (options & TaskContinuationOptions.NotOnRanToCompletion) == 0;
		}

		/// <summary>
		/// Creates a continuation that executes when the target <see cref="T:System.Threading.Tasks.Task" /> completes.
		/// </summary>
		/// <param name="continuationAction">
		/// An action to run when the <see cref="T:System.Threading.Tasks.Task" /> completes. When run, the delegate will be
		/// passed the completed task as an argument.
		/// </param>
		/// <returns>A new continuation <see cref="T:System.Threading.Tasks.Task" />.</returns>
		/// <remarks>
		/// The returned <see cref="T:System.Threading.Tasks.Task" /> will not be scheduled for execution until the current task has
		/// completed, whether it completes due to running to completion successfully, faulting due to an
		/// unhandled exception, or exiting out early due to being canceled.
		/// </remarks>
		/// <exception cref="T:System.ArgumentNullException">
		/// The <paramref name="continuationAction" /> argument is null.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">
		/// The <see cref="T:System.Threading.Tasks.Task" /> has been disposed.
		/// </exception>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task ContinueWith(Action<Task> continuationAction)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return ContinueWith(continuationAction, TaskScheduler.Current, CancellationToken.None, TaskContinuationOptions.None, ref stackMark);
		}

		/// <summary>
		/// Creates a continuation that executes when the target <see cref="T:System.Threading.Tasks.Task" /> completes.
		/// </summary>
		/// <param name="continuationAction">
		/// An action to run when the <see cref="T:System.Threading.Tasks.Task" /> completes. When run, the delegate will be
		/// passed the completed task as an argument.
		/// </param>
		/// <param name="cancellationToken"> The <see cref="P:System.Threading.Tasks.Task.CancellationToken" /> that will be assigned to the new continuation task.</param>
		/// <returns>A new continuation <see cref="T:System.Threading.Tasks.Task" />.</returns>
		/// <remarks>
		/// The returned <see cref="T:System.Threading.Tasks.Task" /> will not be scheduled for execution until the current task has
		/// completed, whether it completes due to running to completion successfully, faulting due to an
		/// unhandled exception, or exiting out early due to being canceled.
		/// </remarks>
		/// <exception cref="T:System.ArgumentNullException">
		/// The <paramref name="continuationAction" /> argument is null.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">
		/// The <see cref="T:System.Threading.Tasks.Task" /> has been disposed.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken">CancellationToken</see>
		/// has already been disposed.
		/// </exception>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task ContinueWith(Action<Task> continuationAction, CancellationToken cancellationToken)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return ContinueWith(continuationAction, TaskScheduler.Current, cancellationToken, TaskContinuationOptions.None, ref stackMark);
		}

		/// <summary>
		/// Creates a continuation that executes when the target <see cref="T:System.Threading.Tasks.Task" /> completes.
		/// </summary>
		/// <param name="continuationAction">
		/// An action to run when the <see cref="T:System.Threading.Tasks.Task" /> completes.  When run, the delegate will be
		/// passed the completed task as an argument.
		/// </param>
		/// <param name="scheduler">
		/// The <see cref="T:System.Threading.Tasks.TaskScheduler" /> to associate with the continuation task and to use for its execution.
		/// </param>
		/// <returns>A new continuation <see cref="T:System.Threading.Tasks.Task" />.</returns>
		/// <remarks>
		/// The returned <see cref="T:System.Threading.Tasks.Task" /> will not be scheduled for execution until the current task has
		/// completed, whether it completes due to running to completion successfully, faulting due to an
		/// unhandled exception, or exiting out early due to being canceled.
		/// </remarks>
		/// <exception cref="T:System.ArgumentNullException">
		/// The <paramref name="continuationAction" /> argument is null.
		/// </exception>
		/// <exception cref="T:System.ArgumentNullException">
		/// The <paramref name="scheduler" /> argument is null.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">
		/// The <see cref="T:System.Threading.Tasks.Task" /> has been disposed.
		/// </exception>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task ContinueWith(Action<Task> continuationAction, TaskScheduler scheduler)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return ContinueWith(continuationAction, scheduler, CancellationToken.None, TaskContinuationOptions.None, ref stackMark);
		}

		/// <summary>
		/// Creates a continuation that executes when the target <see cref="T:System.Threading.Tasks.Task" /> completes.
		/// </summary>
		/// <param name="continuationAction">
		/// An action to run when the <see cref="T:System.Threading.Tasks.Task" /> completes. When run, the delegate will be
		/// passed the completed task as an argument.
		/// </param>
		/// <param name="continuationOptions">
		/// Options for when the continuation is scheduled and how it behaves. This includes criteria, such
		/// as <see cref="F:System.Threading.Tasks.TaskContinuationOptions.OnlyOnCanceled">OnlyOnCanceled</see>, as
		/// well as execution options, such as <see cref="F:System.Threading.Tasks.TaskContinuationOptions.ExecuteSynchronously">ExecuteSynchronously</see>.
		/// </param>
		/// <returns>A new continuation <see cref="T:System.Threading.Tasks.Task" />.</returns>
		/// <remarks>
		/// The returned <see cref="T:System.Threading.Tasks.Task" /> will not be scheduled for execution until the current task has
		/// completed. If the continuation criteria specified through the <paramref name="continuationOptions" /> parameter are not met, the continuation task will be canceled
		/// instead of scheduled.
		/// </remarks>
		/// <exception cref="T:System.ArgumentNullException">
		/// The <paramref name="continuationAction" /> argument is null.
		/// </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// The <paramref name="continuationOptions" /> argument specifies an invalid value for <see cref="T:System.Threading.Tasks.TaskContinuationOptions">TaskContinuationOptions</see>.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">
		/// The <see cref="T:System.Threading.Tasks.Task" /> has been disposed.
		/// </exception>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task ContinueWith(Action<Task> continuationAction, TaskContinuationOptions continuationOptions)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return ContinueWith(continuationAction, TaskScheduler.Current, CancellationToken.None, continuationOptions, ref stackMark);
		}

		/// <summary>
		/// Creates a continuation that executes when the target <see cref="T:System.Threading.Tasks.Task" /> completes.
		/// </summary>
		/// <param name="continuationAction">
		/// An action to run when the <see cref="T:System.Threading.Tasks.Task" /> completes. When run, the delegate will be
		/// passed the completed task as an argument.
		/// </param>
		/// <param name="continuationOptions">
		/// Options for when the continuation is scheduled and how it behaves. This includes criteria, such
		/// as <see cref="F:System.Threading.Tasks.TaskContinuationOptions.OnlyOnCanceled">OnlyOnCanceled</see>, as
		/// well as execution options, such as <see cref="F:System.Threading.Tasks.TaskContinuationOptions.ExecuteSynchronously">ExecuteSynchronously</see>.
		/// </param>
		/// <param name="cancellationToken">The <see cref="P:System.Threading.Tasks.Task.CancellationToken" /> that will be assigned to the new continuation task.</param>
		/// <param name="scheduler">
		/// The <see cref="T:System.Threading.Tasks.TaskScheduler" /> to associate with the continuation task and to use for its
		/// execution.
		/// </param>
		/// <returns>A new continuation <see cref="T:System.Threading.Tasks.Task" />.</returns>
		/// <remarks>
		/// The returned <see cref="T:System.Threading.Tasks.Task" /> will not be scheduled for execution until the current task has
		/// completed. If the criteria specified through the <paramref name="continuationOptions" /> parameter
		/// are not met, the continuation task will be canceled instead of scheduled.
		/// </remarks>
		/// <exception cref="T:System.ArgumentNullException">
		/// The <paramref name="continuationAction" /> argument is null.
		/// </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// The <paramref name="continuationOptions" /> argument specifies an invalid value for <see cref="T:System.Threading.Tasks.TaskContinuationOptions">TaskContinuationOptions</see>.
		/// </exception>
		/// <exception cref="T:System.ArgumentNullException">
		/// The <paramref name="scheduler" /> argument is null.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">
		/// The <see cref="T:System.Threading.Tasks.Task" /> has been disposed.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken">CancellationToken</see>
		/// has already been disposed.
		/// </exception>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task ContinueWith(Action<Task> continuationAction, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return ContinueWith(continuationAction, scheduler, cancellationToken, continuationOptions, ref stackMark);
		}

		private Task ContinueWith(Action<Task> continuationAction, TaskScheduler scheduler, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, ref StackCrawlMark2 stackMark)
		{
			ThrowIfDisposed();
			if (continuationAction == null)
			{
				throw new ArgumentNullException("continuationAction");
			}
			if (scheduler == null)
			{
				throw new ArgumentNullException("scheduler");
			}
			CreationOptionsFromContinuationOptions(continuationOptions, out var creationOptions, out var internalOptions);
			Task thisTask = this;
			Task task = new Task(delegate
			{
				continuationAction(thisTask);
			}, null, InternalCurrent, cancellationToken, creationOptions, internalOptions, null, ref stackMark);
			ContinueWithCore(task, scheduler, continuationOptions);
			return task;
		}

		/// <summary>
		/// Creates a continuation that executes when the target <see cref="T:System.Threading.Tasks.Task" /> completes.
		/// </summary>
		/// <typeparam name="TResult">
		/// The type of the result produced by the continuation.
		/// </typeparam>
		/// <param name="continuationFunction">
		/// A function to run when the <see cref="T:System.Threading.Tasks.Task" /> completes. When run, the delegate will be
		/// passed the completed task as an argument.
		/// </param>
		/// <returns>A new continuation <see cref="T:System.Threading.Tasks.Task`1" />.</returns>
		/// <remarks>
		/// The returned <see cref="T:System.Threading.Tasks.Task`1" /> will not be scheduled for execution until the current task has
		/// completed, whether it completes due to running to completion successfully, faulting due to an
		/// unhandled exception, or exiting out early due to being canceled.
		/// </remarks>
		/// <exception cref="T:System.ArgumentNullException">
		/// The <paramref name="continuationFunction" /> argument is null.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">
		/// The <see cref="T:System.Threading.Tasks.Task" /> has been disposed.
		/// </exception>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task<TResult> ContinueWith<TResult>(Func<Task, TResult> continuationFunction)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return ContinueWith(continuationFunction, TaskScheduler.Current, CancellationToken.None, TaskContinuationOptions.None, ref stackMark);
		}

		/// <summary>
		/// Creates a continuation that executes when the target <see cref="T:System.Threading.Tasks.Task" /> completes.
		/// </summary>
		/// <typeparam name="TResult">
		/// The type of the result produced by the continuation.
		/// </typeparam>
		/// <param name="continuationFunction">
		/// A function to run when the <see cref="T:System.Threading.Tasks.Task" /> completes. When run, the delegate will be
		/// passed the completed task as an argument.
		/// </param>
		/// <param name="cancellationToken">The <see cref="P:System.Threading.Tasks.Task.CancellationToken" /> that will be assigned to the new continuation task.</param>
		/// <returns>A new continuation <see cref="T:System.Threading.Tasks.Task`1" />.</returns>
		/// <remarks>
		/// The returned <see cref="T:System.Threading.Tasks.Task`1" /> will not be scheduled for execution until the current task has
		/// completed, whether it completes due to running to completion successfully, faulting due to an
		/// unhandled exception, or exiting out early due to being canceled.
		/// </remarks>
		/// <exception cref="T:System.ArgumentNullException">
		/// The <paramref name="continuationFunction" /> argument is null.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">
		/// The <see cref="T:System.Threading.Tasks.Task" /> has been disposed.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken">CancellationToken</see>
		/// has already been disposed.
		/// </exception>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task<TResult> ContinueWith<TResult>(Func<Task, TResult> continuationFunction, CancellationToken cancellationToken)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return ContinueWith(continuationFunction, TaskScheduler.Current, cancellationToken, TaskContinuationOptions.None, ref stackMark);
		}

		/// <summary>
		/// Creates a continuation that executes when the target <see cref="T:System.Threading.Tasks.Task" /> completes.
		/// </summary>
		/// <typeparam name="TResult">
		/// The type of the result produced by the continuation.
		/// </typeparam>
		/// <param name="continuationFunction">
		/// A function to run when the <see cref="T:System.Threading.Tasks.Task" /> completes.  When run, the delegate will be
		/// passed the completed task as an argument.
		/// </param>
		/// <param name="scheduler">
		/// The <see cref="T:System.Threading.Tasks.TaskScheduler" /> to associate with the continuation task and to use for its execution.
		/// </param>
		/// <returns>A new continuation <see cref="T:System.Threading.Tasks.Task`1" />.</returns>
		/// <remarks>
		/// The returned <see cref="T:System.Threading.Tasks.Task`1" /> will not be scheduled for execution until the current task has
		/// completed, whether it completes due to running to completion successfully, faulting due to an
		/// unhandled exception, or exiting out early due to being canceled.
		/// </remarks>
		/// <exception cref="T:System.ArgumentNullException">
		/// The <paramref name="continuationFunction" /> argument is null.
		/// </exception>
		/// <exception cref="T:System.ArgumentNullException">
		/// The <paramref name="scheduler" /> argument is null.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">
		/// The <see cref="T:System.Threading.Tasks.Task" /> has been disposed.
		/// </exception>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task<TResult> ContinueWith<TResult>(Func<Task, TResult> continuationFunction, TaskScheduler scheduler)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return ContinueWith(continuationFunction, scheduler, CancellationToken.None, TaskContinuationOptions.None, ref stackMark);
		}

		/// <summary>
		/// Creates a continuation that executes when the target <see cref="T:System.Threading.Tasks.Task" /> completes.
		/// </summary>
		/// <typeparam name="TResult">
		/// The type of the result produced by the continuation.
		/// </typeparam>
		/// <param name="continuationFunction">
		/// A function to run when the <see cref="T:System.Threading.Tasks.Task" /> completes. When run, the delegate will be
		/// passed the completed task as an argument.
		/// </param>
		/// <param name="continuationOptions">
		/// Options for when the continuation is scheduled and how it behaves. This includes criteria, such
		/// as <see cref="F:System.Threading.Tasks.TaskContinuationOptions.OnlyOnCanceled">OnlyOnCanceled</see>, as
		/// well as execution options, such as <see cref="F:System.Threading.Tasks.TaskContinuationOptions.ExecuteSynchronously">ExecuteSynchronously</see>.
		/// </param>
		/// <returns>A new continuation <see cref="T:System.Threading.Tasks.Task`1" />.</returns>
		/// <remarks>
		/// The returned <see cref="T:System.Threading.Tasks.Task`1" /> will not be scheduled for execution until the current task has
		/// completed. If the continuation criteria specified through the <paramref name="continuationOptions" /> parameter are not met, the continuation task will be canceled
		/// instead of scheduled.
		/// </remarks>
		/// <exception cref="T:System.ArgumentNullException">
		/// The <paramref name="continuationFunction" /> argument is null.
		/// </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// The <paramref name="continuationOptions" /> argument specifies an invalid value for <see cref="T:System.Threading.Tasks.TaskContinuationOptions">TaskContinuationOptions</see>.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">
		/// The <see cref="T:System.Threading.Tasks.Task" /> has been disposed.
		/// </exception>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task<TResult> ContinueWith<TResult>(Func<Task, TResult> continuationFunction, TaskContinuationOptions continuationOptions)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return ContinueWith(continuationFunction, TaskScheduler.Current, CancellationToken.None, continuationOptions, ref stackMark);
		}

		/// <summary>
		/// Creates a continuation that executes when the target <see cref="T:System.Threading.Tasks.Task" /> completes.
		/// </summary>
		/// <typeparam name="TResult">
		/// The type of the result produced by the continuation.
		/// </typeparam>
		/// <param name="continuationFunction">
		/// A function to run when the <see cref="T:System.Threading.Tasks.Task" /> completes. When run, the delegate will be
		/// passed the completed task as an argument.
		/// </param>
		/// <param name="cancellationToken">The <see cref="P:System.Threading.Tasks.Task.CancellationToken" /> that will be assigned to the new continuation task.</param>
		/// <param name="continuationOptions">
		/// Options for when the continuation is scheduled and how it behaves. This includes criteria, such
		/// as <see cref="F:System.Threading.Tasks.TaskContinuationOptions.OnlyOnCanceled">OnlyOnCanceled</see>, as
		/// well as execution options, such as <see cref="F:System.Threading.Tasks.TaskContinuationOptions.ExecuteSynchronously">ExecuteSynchronously</see>.
		/// </param>
		/// <param name="scheduler">
		/// The <see cref="T:System.Threading.Tasks.TaskScheduler" /> to associate with the continuation task and to use for its
		/// execution.
		/// </param>
		/// <returns>A new continuation <see cref="T:System.Threading.Tasks.Task`1" />.</returns>
		/// <remarks>
		/// The returned <see cref="T:System.Threading.Tasks.Task`1" /> will not be scheduled for execution until the current task has
		/// completed. If the criteria specified through the <paramref name="continuationOptions" /> parameter
		/// are not met, the continuation task will be canceled instead of scheduled.
		/// </remarks>
		/// <exception cref="T:System.ArgumentNullException">
		/// The <paramref name="continuationFunction" /> argument is null.
		/// </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// The <paramref name="continuationOptions" /> argument specifies an invalid value for <see cref="T:System.Threading.Tasks.TaskContinuationOptions">TaskContinuationOptions</see>.
		/// </exception>
		/// <exception cref="T:System.ArgumentNullException">
		/// The <paramref name="scheduler" /> argument is null.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">
		/// The <see cref="T:System.Threading.Tasks.Task" /> has been disposed.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken">CancellationToken</see>
		/// has already been disposed.
		/// </exception>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task<TResult> ContinueWith<TResult>(Func<Task, TResult> continuationFunction, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return ContinueWith(continuationFunction, scheduler, cancellationToken, continuationOptions, ref stackMark);
		}

		private Task<TResult> ContinueWith<TResult>(Func<Task, TResult> continuationFunction, TaskScheduler scheduler, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, ref StackCrawlMark2 stackMark)
		{
			ThrowIfDisposed();
			if (continuationFunction == null)
			{
				throw new ArgumentNullException("continuationFunction");
			}
			if (scheduler == null)
			{
				throw new ArgumentNullException("scheduler");
			}
			CreationOptionsFromContinuationOptions(continuationOptions, out var creationOptions, out var internalOptions);
			Task thisTask = this;
			Task<TResult> task = new Task<TResult>(() => continuationFunction(thisTask), InternalCurrent, cancellationToken, creationOptions, internalOptions, null, ref stackMark);
			ContinueWithCore(task, scheduler, continuationOptions);
			return task;
		}

		/// <summary>
		/// Converts TaskContinuationOptions to TaskCreationOptions, and also does
		/// some validity checking along the way.
		/// </summary>
		/// <param name="continuationOptions">Incoming TaskContinuationOptions</param>
		/// <param name="creationOptions">Outgoing TaskCreationOptions</param>
		/// <param name="internalOptions">Outgoing InternalTaskOptions</param>
		internal static void CreationOptionsFromContinuationOptions(TaskContinuationOptions continuationOptions, out TaskCreationOptions creationOptions, out InternalTaskOptions internalOptions)
		{
			TaskContinuationOptions taskContinuationOptions = TaskContinuationOptions.OnlyOnRanToCompletion | TaskContinuationOptions.NotOnRanToCompletion;
			TaskContinuationOptions taskContinuationOptions2 = TaskContinuationOptions.PreferFairness | TaskContinuationOptions.LongRunning | TaskContinuationOptions.AttachedToParent;
			TaskContinuationOptions taskContinuationOptions3 = TaskContinuationOptions.LongRunning | TaskContinuationOptions.ExecuteSynchronously;
			if ((continuationOptions & taskContinuationOptions3) == taskContinuationOptions3)
			{
				throw new ArgumentOutOfRangeException("continuationOptions", Environment2.GetResourceString("Task_ContinueWith_ESandLR"));
			}
			if ((continuationOptions & ~(taskContinuationOptions2 | taskContinuationOptions | TaskContinuationOptions.ExecuteSynchronously)) != 0)
			{
				throw new ArgumentOutOfRangeException("continuationOptions");
			}
			if ((continuationOptions & taskContinuationOptions) == taskContinuationOptions)
			{
				throw new ArgumentOutOfRangeException("continuationOptions", Environment2.GetResourceString("Task_ContinueWith_NotOnAnything"));
			}
			creationOptions = (TaskCreationOptions)(continuationOptions & taskContinuationOptions2);
			internalOptions = InternalTaskOptions.ContinuationTask;
		}

		/// <summary>
		/// Registers the continuation and possibly runs it (if the task is already finished).
		/// </summary>
		/// <param name="continuationTask">The continuation task itself.</param>
		/// <param name="scheduler">TaskScheduler with which to associate continuation task.</param>
		/// <param name="options">Restrictions on when the continuation becomes active.</param>
		internal void ContinueWithCore(Task continuationTask, TaskScheduler scheduler, TaskContinuationOptions options)
		{
			if (continuationTask.IsCompleted)
			{
				return;
			}
			TaskContinuation item = new TaskContinuation(continuationTask, scheduler, options);
			if (!IsCompleted)
			{
                _ = LazyInitializer.EnsureInitialized(ref m_contingentProperties, s_contingentPropertyCreator);
				if (m_contingentProperties.m_continuations == null)
				{
					Interlocked.CompareExchange(ref m_contingentProperties.m_continuations, new List<TaskContinuation>(), null);
				}
				lock (m_contingentProperties)
				{
					if (!IsCompleted)
					{
						m_contingentProperties.m_continuations.Add(item);
						return;
					}
				}
			}
			item.Run(this, bCanInlineContinuationTask: true);
		}

		internal void AddCompletionAction(Action<Task> action)
		{
			if (!IsCompleted)
			{
                _ = LazyInitializer.EnsureInitialized(ref m_contingentProperties, s_contingentPropertyCreator);
				TaskContinuation item = new TaskContinuation(action);
				if (m_contingentProperties.m_continuations == null)
				{
					Interlocked.CompareExchange(ref m_contingentProperties.m_continuations, new List<TaskContinuation>(), null);
				}
				lock (m_contingentProperties)
				{
					if (!IsCompleted)
					{
						m_contingentProperties.m_continuations.Add(item);
						return;
					}
				}
			}
			action(this);
		}

		/// <summary>
		/// Waits for all of the provided <see cref="T:System.Threading.Tasks.Task" /> objects to complete execution.
		/// </summary>
		/// <param name="tasks">
		/// An array of <see cref="T:System.Threading.Tasks.Task" /> instances on which to wait.
		/// </param>
		/// <exception cref="T:System.ArgumentNullException">
		/// The <paramref name="tasks" /> argument is null.
		/// </exception>
		/// <exception cref="T:System.ArgumentNullException">
		/// The <paramref name="tasks" /> argument contains a null element.
		/// </exception>
		/// <exception cref="T:System.AggregateException">
		/// At least one of the <see cref="T:System.Threading.Tasks.Task" /> instances was canceled -or- an exception was thrown during
		/// the execution of at least one of the <see cref="T:System.Threading.Tasks.Task" /> instances.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">
		/// The <see cref="T:System.Threading.Tasks.Task" /> has been disposed.
		/// </exception>
		[MethodImpl(MethodImplOptions.NoOptimization)]
		public static void WaitAll(params Task[] tasks)
		{
			WaitAll(tasks, -1);
		}

		/// <summary>
		/// Waits for all of the provided <see cref="T:System.Threading.Tasks.Task" /> objects to complete execution.
		/// </summary>
		/// <returns>
		/// true if all of the <see cref="T:System.Threading.Tasks.Task" /> instances completed execution within the allotted time;
		/// otherwise, false.
		/// </returns>
		/// <param name="tasks">
		/// An array of <see cref="T:System.Threading.Tasks.Task" /> instances on which to wait.
		/// </param>
		/// <param name="timeout">
		/// A <see cref="T:System.TimeSpan" /> that represents the number of milliseconds to wait, or a <see cref="T:System.TimeSpan" /> that represents -1 milliseconds to wait indefinitely.
		/// </param>
		/// <exception cref="T:System.ArgumentNullException">
		/// The <paramref name="tasks" /> argument is null.
		/// </exception>
		/// <exception cref="T:System.ArgumentException">
		/// The <paramref name="tasks" /> argument contains a null element.
		/// </exception>
		/// <exception cref="T:System.AggregateException">
		/// At least one of the <see cref="T:System.Threading.Tasks.Task" /> instances was canceled -or- an exception was thrown during
		/// the execution of at least one of the <see cref="T:System.Threading.Tasks.Task" /> instances.
		/// </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// <paramref name="timeout" /> is a negative number other than -1 milliseconds, which represents an
		/// infinite time-out -or- timeout is greater than
		/// <see cref="F:System.Int32.MaxValue" />.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">
		/// The <see cref="T:System.Threading.Tasks.Task" /> has been disposed.
		/// </exception>
		[MethodImpl(MethodImplOptions.NoOptimization)]
		public static bool WaitAll(Task[] tasks, TimeSpan timeout)
		{
			long num = (long)timeout.TotalMilliseconds;
			if (num < -1 || num > int.MaxValue)
			{
				throw new ArgumentOutOfRangeException("timeout");
			}
			return WaitAll(tasks, (int)num);
		}

		/// <summary>
		/// Waits for all of the provided <see cref="T:System.Threading.Tasks.Task" /> objects to complete execution.
		/// </summary>
		/// <returns>
		/// true if all of the <see cref="T:System.Threading.Tasks.Task" /> instances completed execution within the allotted time;
		/// otherwise, false.
		/// </returns>
		/// <param name="millisecondsTimeout">
		/// The number of milliseconds to wait, or <see cref="F:System.Threading.Timeout.Infinite" /> (-1) to
		/// wait indefinitely.</param>
		/// <param name="tasks">An array of <see cref="T:System.Threading.Tasks.Task" /> instances on which to wait.
		/// </param>
		/// <exception cref="T:System.ArgumentNullException">
		/// The <paramref name="tasks" /> argument is null.
		/// </exception>
		/// <exception cref="T:System.ArgumentException">
		/// The <paramref name="tasks" /> argument contains a null element.
		/// </exception>
		/// <exception cref="T:System.AggregateException">
		/// At least one of the <see cref="T:System.Threading.Tasks.Task" /> instances was canceled -or- an exception was thrown during
		/// the execution of at least one of the <see cref="T:System.Threading.Tasks.Task" /> instances.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">
		/// The <see cref="T:System.Threading.Tasks.Task" /> has been disposed.
		/// </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// <paramref name="millisecondsTimeout" /> is a negative number other than -1, which represents an
		/// infinite time-out.
		/// </exception>
		[MethodImpl(MethodImplOptions.NoOptimization)]
		public static bool WaitAll(Task[] tasks, int millisecondsTimeout)
		{
			return WaitAll(tasks, millisecondsTimeout, CancellationToken.None);
		}

		/// <summary>
		/// Waits for all of the provided <see cref="T:System.Threading.Tasks.Task" /> objects to complete execution.
		/// </summary>
		/// <returns>
		/// true if all of the <see cref="T:System.Threading.Tasks.Task" /> instances completed execution within the allotted time;
		/// otherwise, false.
		/// </returns>
		/// <param name="tasks">
		/// An array of <see cref="T:System.Threading.Tasks.Task" /> instances on which to wait.
		/// </param>
		/// <param name="cancellationToken">
		/// A <see cref="P:System.Threading.Tasks.Task.CancellationToken" /> to observe while waiting for the tasks to complete.
		/// </param>
		/// <exception cref="T:System.ArgumentNullException">
		/// The <paramref name="tasks" /> argument is null.
		/// </exception>
		/// <exception cref="T:System.ArgumentException">
		/// The <paramref name="tasks" /> argument contains a null element.
		/// </exception>
		/// <exception cref="T:System.AggregateException">
		/// At least one of the <see cref="T:System.Threading.Tasks.Task" /> instances was canceled -or- an exception was thrown during
		/// the execution of at least one of the <see cref="T:System.Threading.Tasks.Task" /> instances.
		/// </exception>
		/// <exception cref="T:System.OperationCanceledException">
		/// The <paramref name="cancellationToken" /> was canceled.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">
		/// The <see cref="T:System.Threading.Tasks.Task" /> has been disposed.
		/// </exception>
		[MethodImpl(MethodImplOptions.NoOptimization)]
		public static void WaitAll(Task[] tasks, CancellationToken cancellationToken)
		{
			WaitAll(tasks, -1, cancellationToken);
		}

		/// <summary>
		/// Waits for all of the provided <see cref="T:System.Threading.Tasks.Task" /> objects to complete execution.
		/// </summary>
		/// <returns>
		/// true if all of the <see cref="T:System.Threading.Tasks.Task" /> instances completed execution within the allotted time;
		/// otherwise, false.
		/// </returns>
		/// <param name="tasks">
		/// An array of <see cref="T:System.Threading.Tasks.Task" /> instances on which to wait.
		/// </param>
		/// <param name="millisecondsTimeout">
		/// The number of milliseconds to wait, or <see cref="F:System.Threading.Timeout.Infinite" /> (-1) to
		/// wait indefinitely.
		/// </param>
		/// <param name="cancellationToken">
		/// A <see cref="P:System.Threading.Tasks.Task.CancellationToken" /> to observe while waiting for the tasks to complete.
		/// </param>
		/// <exception cref="T:System.ArgumentNullException">
		/// The <paramref name="tasks" /> argument is null.
		/// </exception>
		/// <exception cref="T:System.ArgumentException">
		/// The <paramref name="tasks" /> argument contains a null element.
		/// </exception>
		/// <exception cref="T:System.AggregateException">
		/// At least one of the <see cref="T:System.Threading.Tasks.Task" /> instances was canceled -or- an exception was thrown during
		/// the execution of at least one of the <see cref="T:System.Threading.Tasks.Task" /> instances.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">
		/// The <see cref="T:System.Threading.Tasks.Task" /> has been disposed.
		/// </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// <paramref name="millisecondsTimeout" /> is a negative number other than -1, which represents an
		/// infinite time-out.
		/// </exception>
		/// <exception cref="T:System.OperationCanceledException">
		/// The <paramref name="cancellationToken" /> was canceled.
		/// </exception>
		[MethodImpl(MethodImplOptions.NoOptimization)]
		public static bool WaitAll(Task[] tasks, int millisecondsTimeout, CancellationToken cancellationToken)
		{
			if (tasks == null)
			{
				throw new ArgumentNullException("tasks");
			}
			if (millisecondsTimeout < -1)
			{
				throw new ArgumentOutOfRangeException("timeout");
			}
			cancellationToken.ThrowIfCancellationRequested();
			List<Exception> exceptions = null;
			List<Task> list = null;
			bool flag = true;
			Task internalCurrent = InternalCurrent;
			TaskScheduler taskScheduler = ((internalCurrent == null) ? TaskScheduler.Default : internalCurrent.ExecutingTaskScheduler);
			object threadStatics = taskScheduler.GetThreadStatics();
			for (int num = tasks.Length - 1; num >= 0; num--)
			{
				Task task = tasks[num];
				if (task == null)
				{
					throw new ArgumentException(Environment2.GetResourceString("Task_WaitMulti_NullTask"), "tasks");
				}
				task.ThrowIfDisposed();
				bool flag2 = task.IsCompleted;
				if (!flag2)
				{
					if (millisecondsTimeout != -1 || cancellationToken.CanBeCanceled)
					{
						if (list == null)
						{
							list = new List<Task>(tasks.Length);
						}
						list.Add(task);
					}
					else
					{
						flag2 = task.WrappedTryRunInline(taskScheduler, threadStatics) && task.IsCompleted;
						if (!flag2)
						{
							if (list == null)
							{
								list = new List<Task>(tasks.Length);
							}
							list.Add(task);
						}
					}
				}
				if (flag2)
				{
					AddExceptionsForCompletedTask(ref exceptions, task);
				}
			}
			if (list != null)
			{
				WaitHandle[] array = new WaitHandle[list.Count];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = list[i].CompletedEvent.WaitHandle;
				}
				flag = WaitAllSTAAnd64Aware(array, millisecondsTimeout, cancellationToken);
				if (flag)
				{
					for (int j = 0; j < list.Count; j++)
					{
						AddExceptionsForCompletedTask(ref exceptions, list[j]);
					}
				}
				GC.KeepAlive(tasks);
			}
			if (exceptions != null)
			{
				throw new AggregateException(exceptions);
			}
			return flag;
		}

		/// <summary>
		/// Waits for a set of handles in a STA-aware way.  In other words, it will wait for each
		/// of the events individually if we're on a STA thread, because MsgWaitForMultipleObjectsEx
		/// can't do a true wait-all due to its hidden message queue event. This is not atomic,
		/// of course, but we only wait on one-way (MRE) events anyway so this is OK.
		/// </summary>
		/// <param name="waitHandles">An array of wait handles to wait on.</param>
		/// <param name="millisecondsTimeout">The timeout to use during waits.</param>
		/// <param name="cancellationToken">The cancellationToken that enables a wait to be canceled.</param>
		/// <returns>True if all waits succeeded, false if a timeout occurred.</returns>
		private static bool WaitAllSTAAnd64Aware(WaitHandle[] waitHandles, int millisecondsTimeout, CancellationToken cancellationToken)
		{
			if (Thread.CurrentThread.GetApartmentState() == ApartmentState.STA || cancellationToken.CanBeCanceled)
			{
				WaitHandle[] array = null;
				if (cancellationToken.CanBeCanceled)
				{
					array = new WaitHandle[2] { null, cancellationToken.WaitHandle };
				}
				for (int i = 0; i < waitHandles.Length; i++)
				{
					long num = ((millisecondsTimeout == -1) ? 0 : DateTime.UtcNow.Ticks);
					if (cancellationToken.CanBeCanceled)
					{
						array[0] = waitHandles[i];
						int num2 = WaitHandle.WaitAny(array, millisecondsTimeout, exitContext: false);
						if (num2 == 258)
						{
							return false;
						}
						cancellationToken.ThrowIfCancellationRequested();
					}
					else if (!waitHandles[i].WaitOne(millisecondsTimeout, exitContext: false))
					{
						return false;
					}
					if (millisecondsTimeout != -1)
					{
						long num3 = (DateTime.UtcNow.Ticks - num) / 10000;
						if (num3 > int.MaxValue || num3 > millisecondsTimeout)
						{
							return false;
						}
						millisecondsTimeout -= (int)num3;
					}
				}
			}
			else if (waitHandles.Length <= 64)
			{
				if (!WaitHandle.WaitAll(waitHandles, millisecondsTimeout, exitContext: false))
				{
					return false;
				}
			}
			else
			{
				int num4 = (waitHandles.Length + 64 - 1) / 64;
				WaitHandle[] array2 = new WaitHandle[64];
				long num5 = ((millisecondsTimeout == -1) ? 0 : DateTime.UtcNow.Ticks);
				for (int j = 0; j < num4; j++)
				{
					if (j == num4 - 1 && waitHandles.Length % 64 != 0)
					{
						array2 = new WaitHandle[waitHandles.Length % 64];
					}
					Array.Copy(waitHandles, j * 64, array2, 0, array2.Length);
					if (!WaitHandle.WaitAll(array2, millisecondsTimeout, exitContext: false))
					{
						return false;
					}
					if (millisecondsTimeout != -1)
					{
						long num6 = (DateTime.UtcNow.Ticks - num5) / 10000;
						if (num6 > int.MaxValue || num6 > millisecondsTimeout)
						{
							return false;
						}
						millisecondsTimeout -= (int)num6;
					}
				}
			}
			return true;
		}

		/// <summary>
		/// Internal WaitAll implementation which is meant to be used with small number of tasks,
		/// optimized for Parallel.Invoke and other structured primitives.
		/// </summary>
		internal static void FastWaitAll(Task[] tasks)
		{
			List<Exception> exceptions = null;
			TaskScheduler current = TaskScheduler.Current;
			object threadStatics = current.GetThreadStatics();
			for (int num = tasks.Length - 1; num >= 0; num--)
			{
				if (!tasks[num].IsCompleted)
				{
					tasks[num].WrappedTryRunInline(current, threadStatics);
				}
			}
			for (int num2 = tasks.Length - 1; num2 >= 0; num2--)
			{
				tasks[num2].CompletedEvent.Wait();
				AddExceptionsForCompletedTask(ref exceptions, tasks[num2]);
			}
			if (exceptions != null)
			{
				throw new AggregateException(exceptions);
			}
		}

		/// <summary>
		/// This internal function is only meant to be called by WaitAll()
		/// If the completed task is canceled or it has other exceptions, here we will add those
		/// into the passed in exception list (which will be lazily initialized here).
		/// </summary>
		internal static void AddExceptionsForCompletedTask(ref List<Exception> exceptions, Task t)
		{
			AggregateException exceptions2 = t.GetExceptions(includeTaskCanceledExceptions: true);
			if (exceptions2 != null)
			{
				t.UpdateExceptionObservedStatus();
				if (exceptions == null)
				{
					exceptions = new List<Exception>(exceptions2.InnerExceptions.Count);
				}
				exceptions.AddRange(exceptions2.InnerExceptions);
			}
		}

		/// <summary>
		/// Waits for any of the provided <see cref="T:System.Threading.Tasks.Task" /> objects to complete execution.
		/// </summary>
		/// <param name="tasks">
		/// An array of <see cref="T:System.Threading.Tasks.Task" /> instances on which to wait.
		/// </param>
		/// <returns>The index of the completed task in the <paramref name="tasks" /> array argument.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		/// The <paramref name="tasks" /> argument is null.
		/// </exception>
		/// <exception cref="T:System.ArgumentException">
		/// The <paramref name="tasks" /> argument contains a null element.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">
		/// The <see cref="T:System.Threading.Tasks.Task" /> has been disposed.
		/// </exception>
		[MethodImpl(MethodImplOptions.NoOptimization)]
		public static int WaitAny(params Task[] tasks)
		{
			return WaitAny(tasks, -1);
		}

		/// <summary>
		/// Waits for any of the provided <see cref="T:System.Threading.Tasks.Task" /> objects to complete execution.
		/// </summary>
		/// <param name="tasks">
		/// An array of <see cref="T:System.Threading.Tasks.Task" /> instances on which to wait.
		/// </param>
		/// <param name="timeout">
		/// A <see cref="T:System.TimeSpan" /> that represents the number of milliseconds to wait, or a <see cref="T:System.TimeSpan" /> that represents -1 milliseconds to wait indefinitely.
		/// </param>
		/// <returns>
		/// The index of the completed task in the <paramref name="tasks" /> array argument, or -1 if the
		/// timeout occurred.
		/// </returns>
		/// <exception cref="T:System.ArgumentNullException">
		/// The <paramref name="tasks" /> argument is null.
		/// </exception>
		/// <exception cref="T:System.ArgumentException">
		/// The <paramref name="tasks" /> argument contains a null element.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">
		/// The <see cref="T:System.Threading.Tasks.Task" /> has been disposed.
		/// </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// <paramref name="timeout" /> is a negative number other than -1 milliseconds, which represents an
		/// infinite time-out -or- timeout is greater than
		/// <see cref="F:System.Int32.MaxValue" />.
		/// </exception>
		[MethodImpl(MethodImplOptions.NoOptimization)]
		public static int WaitAny(Task[] tasks, TimeSpan timeout)
		{
			long num = (long)timeout.TotalMilliseconds;
			if (num < -1 || num > int.MaxValue)
			{
				throw new ArgumentOutOfRangeException("timeout");
			}
			return WaitAny(tasks, (int)num);
		}

		/// <summary>
		/// Waits for any of the provided <see cref="T:System.Threading.Tasks.Task" /> objects to complete execution.
		/// </summary>
		/// <param name="tasks">
		/// An array of <see cref="T:System.Threading.Tasks.Task" /> instances on which to wait.
		/// </param>
		/// <param name="cancellationToken">
		/// A <see cref="P:System.Threading.Tasks.Task.CancellationToken" /> to observe while waiting for a task to complete.
		/// </param>
		/// <returns>
		/// The index of the completed task in the <paramref name="tasks" /> array argument.
		/// </returns>
		/// <exception cref="T:System.ArgumentNullException">
		/// The <paramref name="tasks" /> argument is null.
		/// </exception>
		/// <exception cref="T:System.ArgumentException">
		/// The <paramref name="tasks" /> argument contains a null element.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">
		/// The <see cref="T:System.Threading.Tasks.Task" /> has been disposed.
		/// </exception>
		/// <exception cref="T:System.OperationCanceledException">
		/// The <paramref name="cancellationToken" /> was canceled.
		/// </exception>
		[MethodImpl(MethodImplOptions.NoOptimization)]
		public static int WaitAny(Task[] tasks, CancellationToken cancellationToken)
		{
			return WaitAny(tasks, -1, cancellationToken);
		}

		/// <summary>
		/// Waits for any of the provided <see cref="T:System.Threading.Tasks.Task" /> objects to complete execution.
		/// </summary>
		/// <param name="tasks">
		/// An array of <see cref="T:System.Threading.Tasks.Task" /> instances on which to wait.
		/// </param>
		/// <param name="millisecondsTimeout">
		/// The number of milliseconds to wait, or <see cref="F:System.Threading.Timeout.Infinite" /> (-1) to
		/// wait indefinitely.
		/// </param>
		/// <returns>
		/// The index of the completed task in the <paramref name="tasks" /> array argument, or -1 if the
		/// timeout occurred.
		/// </returns>
		/// <exception cref="T:System.ArgumentNullException">
		/// The <paramref name="tasks" /> argument is null.
		/// </exception>
		/// <exception cref="T:System.ArgumentException">
		/// The <paramref name="tasks" /> argument contains a null element.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">
		/// The <see cref="T:System.Threading.Tasks.Task" /> has been disposed.
		/// </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// <paramref name="millisecondsTimeout" /> is a negative number other than -1, which represents an
		/// infinite time-out.
		/// </exception>
		[MethodImpl(MethodImplOptions.NoOptimization)]
		public static int WaitAny(Task[] tasks, int millisecondsTimeout)
		{
			return WaitAny(tasks, millisecondsTimeout, CancellationToken.None);
		}

		/// <summary>
		/// Waits for any of the provided <see cref="T:System.Threading.Tasks.Task" /> objects to complete execution.
		/// </summary>
		/// <param name="tasks">
		/// An array of <see cref="T:System.Threading.Tasks.Task" /> instances on which to wait.
		/// </param>
		/// <param name="millisecondsTimeout">
		/// The number of milliseconds to wait, or <see cref="F:System.Threading.Timeout.Infinite" /> (-1) to
		/// wait indefinitely.
		/// </param>
		/// <param name="cancellationToken">
		/// A <see cref="P:System.Threading.Tasks.Task.CancellationToken" /> to observe while waiting for a task to complete.
		/// </param>
		/// <returns>
		/// The index of the completed task in the <paramref name="tasks" /> array argument, or -1 if the
		/// timeout occurred.
		/// </returns>
		/// <exception cref="T:System.ArgumentNullException">
		/// The <paramref name="tasks" /> argument is null.
		/// </exception>
		/// <exception cref="T:System.ArgumentException">
		/// The <paramref name="tasks" /> argument contains a null element.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">
		/// The <see cref="T:System.Threading.Tasks.Task" /> has been disposed.
		/// </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// <paramref name="millisecondsTimeout" /> is a negative number other than -1, which represents an
		/// infinite time-out.
		/// </exception>
		/// <exception cref="T:System.OperationCanceledException">
		/// The <paramref name="cancellationToken" /> was canceled.
		/// </exception>
		[MethodImpl(MethodImplOptions.NoOptimization)]
		public static int WaitAny(Task[] tasks, int millisecondsTimeout, CancellationToken cancellationToken)
		{
			if (tasks == null)
			{
				throw new ArgumentNullException("tasks");
			}
			if (millisecondsTimeout < -1)
			{
				throw new ArgumentOutOfRangeException("millisecondsTimeout");
			}
			cancellationToken.ThrowIfCancellationRequested();
			int num = -1;
			int num2 = (cancellationToken.CanBeCanceled ? 1 : 0) + ((Thread.CurrentThread.GetApartmentState() == ApartmentState.STA) ? 1 : 0);
			int num3 = 64 - num2;
			int num4 = 0;
			int num5 = 0;
			if (tasks.Length > num3)
			{
				num4 = num3 - 1;
				num5 = tasks.Length - num4;
			}
			else
			{
				num4 = tasks.Length;
			}
			for (int i = 0; i < tasks.Length; i++)
			{
				Task task = tasks[i];
				if (task == null)
				{
					throw new ArgumentException(Environment2.GetResourceString("Task_WaitMulti_NullTask"), "tasks");
				}
				task.ThrowIfDisposed();
				if (task.IsCompleted && num == -1)
				{
					num = i;
				}
			}
			if (num != -1)
			{
				return num;
			}
			Task[] array = new Task[num4];
			Task[] tasksLocalCopy2 = ((num5 > 0) ? new Task[num5] : null);
			for (int j = 0; j < tasks.Length; j++)
			{
				Task task2 = tasks[j];
				if (task2 == null)
				{
					throw new ArgumentException(Environment2.GetResourceString("Task_WaitMulti_NullTask"), "tasks");
				}
				if (j < num4)
				{
					array[j] = task2;
				}
				else
				{
					tasksLocalCopy2[j - num4] = task2;
				}
				task2.ThrowIfDisposed();
				if (task2.IsCompleted && num == -1)
				{
					num = j;
				}
			}
			if (num == -1 && tasks.Length != 0)
			{
				int num6 = num4 + ((num5 > 0) ? 1 : 0) + (cancellationToken.CanBeCanceled ? 1 : 0);
				Task<int> task3 = null;
				WaitHandle[] array2 = new WaitHandle[num6];
				for (int k = 0; k < num4; k++)
				{
					array2[k] = array[k].CompletedEvent.WaitHandle;
				}
				if (num5 > 0)
				{
					task3 = Factory.ContinueWhenAny(tasksLocalCopy2, delegate(Task antecedent)
					{
						for (int l = 0; l < tasksLocalCopy2.Length; l++)
						{
							if (antecedent == tasksLocalCopy2[l])
							{
								return l;
							}
						}
						return tasksLocalCopy2.Length;
					});
					array2[num4] = task3.CompletedEvent.WaitHandle;
				}
				if (cancellationToken.CanBeCanceled)
				{
					array2[num6 - 1] = cancellationToken.WaitHandle;
				}
				int num7 = WaitHandle.WaitAny(array2, millisecondsTimeout, exitContext: false);
				cancellationToken.ThrowIfCancellationRequested();
				if (num7 != 258)
				{
					if (num5 > 0 && num7 == num4)
					{
						num7 = num4 + task3.Result;
					}
					num = num7;
				}
			}
			GC.KeepAlive(tasks);
			return num;
		}
	}
	/// <summary>
	/// Represents an asynchronous operation that produces a result at some time in the future.
	/// </summary>
	/// <typeparam name="TResult">
	/// The type of the result produced by this <see cref="T:System.Threading.Tasks.Task`1" />.
	/// </typeparam>
	/// <remarks>
	/// <para>
	/// <see cref="T:System.Threading.Tasks.Task`1" /> instances may be created in a variety of ways. The most common approach is by
	/// using the task's <see cref="P:System.Threading.Tasks.Task`1.Factory" /> property to retrieve a <see cref="T:System.Threading.Tasks.TaskFactory`1" /> instance that can be used to create tasks for several
	/// purposes. For example, to create a <see cref="T:System.Threading.Tasks.Task`1" /> that runs a function, the factory's StartNew
	/// method may be used:
	/// <code>
	/// // C# 
	/// var t = Task&lt;int&gt;.Factory.StartNew(() =&gt; GenerateResult());
	/// - or -
	/// var t = Task.Factory.StartNew(() =&gt; GenerateResult());
	///
	/// ' Visual Basic 
	/// Dim t = Task&lt;int&gt;.Factory.StartNew(Function() GenerateResult())
	/// - or -
	/// Dim t = Task.Factory.StartNew(Function() GenerateResult())
	/// </code>
	/// </para>
	/// <para>
	/// The <see cref="T:System.Threading.Tasks.Task`1" /> class also provides constructors that initialize the task but that do not
	/// schedule it for execution. For performance reasons, the StartNew method should be the
	/// preferred mechanism for creating and scheduling computational tasks, but for scenarios where creation
	/// and scheduling must be separated, the constructors may be used, and the task's 
	/// <see cref="M:System.Threading.Tasks.Task.Start">Start</see>
	/// method may then be used to schedule the task for execution at a later time.
	/// </para>
	/// <para>
	/// All members of <see cref="T:System.Threading.Tasks.Task`1" />, except for 
	/// <see cref="M:System.Threading.Tasks.Task.Dispose">Dispose</see>, are thread-safe
	/// and may be used from multiple threads concurrently.
	/// </para>
	/// </remarks>
	[DebuggerTypeProxy(typeof(SystemThreadingTasks_FutureDebugView<>))]
	[DebuggerDisplay("Id = {Id}, Status = {Status}, Method = {DebuggerDisplayMethodDescription}, Result = {DebuggerDisplayResultDescription}")]
	[HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
	public class Task<TResult> : Task
	{
		private object m_valueSelector;

		private TResult m_result;

		internal bool m_resultWasSet;

		private object m_futureState;

		private static TaskFactory<TResult> s_Factory = new TaskFactory<TResult>();

		private string DebuggerDisplayResultDescription
		{
			get
			{
				if (!m_resultWasSet)
				{
					return Environment2.GetResourceString("TaskT_DebuggerNoResult");
				}
				return string.Concat(m_result);
			}
		}

		private string DebuggerDisplayMethodDescription
		{
			get
			{
				Delegate @delegate = (Delegate)m_valueSelector;
				if ((object)@delegate == null)
				{
					return "{null}";
				}
				return @delegate.Method.ToString();
			}
		}

		/// <summary>
		/// Gets the result value of this <see cref="T:System.Threading.Tasks.Task`1" />.
		/// </summary>
		/// <remarks>
		/// The get accessor for this property ensures that the asynchronous operation is complete before
		/// returning. Once the result of the computation is available, it is stored and will be returned
		/// immediately on later calls to <see cref="P:System.Threading.Tasks.Task`1.Result" />.
		/// </remarks>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public TResult Result
		{
			get
			{
				if (!base.IsCompleted)
				{
					Wait();
				}
				ThrowIfExceptional(!m_resultWasSet);
				return m_result;
			}
			internal set
			{
				if (m_valueSelector != null)
				{
					throw new InvalidOperationException(Environment2.GetResourceString("TaskT_SetResult_HasAnInitializer"));
				}
				if (!TrySetResult(value))
				{
					throw new InvalidOperationException(Environment2.GetResourceString("TaskT_TransitionToFinal_AlreadyCompleted"));
				}
			}
		}

		/// <summary>
		/// Provides access to factory methods for creating <see cref="T:System.Threading.Tasks.Task`1" /> instances.
		/// </summary>
		/// <remarks>
		/// The factory returned from <see cref="P:System.Threading.Tasks.Task`1.Factory" /> is a default instance
		/// of <see cref="T:System.Threading.Tasks.TaskFactory`1" />, as would result from using
		/// the default constructor on the factory type.
		/// </remarks>
		public new static TaskFactory<TResult> Factory => s_Factory;

		internal override object InternalAsyncState => m_futureState;

		internal Task(object state, CancellationToken cancellationToken, TaskCreationOptions options, InternalTaskOptions internalOptions)
			: base(null, cancellationToken, options, internalOptions, promiseStyle: true)
		{
			m_valueSelector = null;
			m_futureState = state;
		}

		internal Task(bool canceled, TResult result, TaskCreationOptions creationOptions)
			: base(canceled, creationOptions)
		{
			if (!canceled)
			{
				m_result = result;
				m_resultWasSet = true;
			}
		}

		/// <summary>
		/// Initializes a new <see cref="T:System.Threading.Tasks.Task`1" /> with the specified function.
		/// </summary>
		/// <param name="function">
		/// The delegate that represents the code to execute in the task. When the function has completed,
		/// the task's <see cref="P:System.Threading.Tasks.Task`1.Result" /> property will be set to return the result value of the function.
		/// </param>
		/// <exception cref="T:System.ArgumentException">
		/// The <paramref name="function" /> argument is null.
		/// </exception>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task(Func<TResult> function)
			: this(function, Task.InternalCurrent, CancellationToken.None, TaskCreationOptions.None, InternalTaskOptions.None, (TaskScheduler)null)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			PossiblyCaptureContext(ref stackMark);
		}

		/// <summary>
		/// Initializes a new <see cref="T:System.Threading.Tasks.Task`1" /> with the specified function.
		/// </summary>
		/// <param name="function">
		/// The delegate that represents the code to execute in the task. When the function has completed,
		/// the task's <see cref="P:System.Threading.Tasks.Task`1.Result" /> property will be set to return the result value of the function.
		/// </param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> to be assigned to this task.</param>
		/// <exception cref="T:System.ArgumentException">
		/// The <paramref name="function" /> argument is null.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken">CancellationToken</see>
		/// has already been disposed.
		/// </exception>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task(Func<TResult> function, CancellationToken cancellationToken)
			: this(function, Task.InternalCurrent, cancellationToken, TaskCreationOptions.None, InternalTaskOptions.None, (TaskScheduler)null)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			PossiblyCaptureContext(ref stackMark);
		}

		/// <summary>
		/// Initializes a new <see cref="T:System.Threading.Tasks.Task`1" /> with the specified function and creation options.
		/// </summary>
		/// <param name="function">
		/// The delegate that represents the code to execute in the task. When the function has completed,
		/// the task's <see cref="P:System.Threading.Tasks.Task`1.Result" /> property will be set to return the result value of the function.
		/// </param>
		/// <param name="creationOptions">
		/// The <see cref="T:System.Threading.Tasks.TaskCreationOptions">TaskCreationOptions</see> used to
		/// customize the task's behavior.
		/// </param>
		/// <exception cref="T:System.ArgumentException">
		/// The <paramref name="function" /> argument is null.
		/// </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// The <paramref name="creationOptions" /> argument specifies an invalid value for <see cref="T:System.Threading.Tasks.TaskCreationOptions" />.
		/// </exception>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task(Func<TResult> function, TaskCreationOptions creationOptions)
			: this(function, Task.InternalCurrent, CancellationToken.None, creationOptions, InternalTaskOptions.None, (TaskScheduler)null)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			PossiblyCaptureContext(ref stackMark);
		}

		/// <summary>
		/// Initializes a new <see cref="T:System.Threading.Tasks.Task`1" /> with the specified function and creation options.
		/// </summary>
		/// <param name="function">
		/// The delegate that represents the code to execute in the task. When the function has completed,
		/// the task's <see cref="P:System.Threading.Tasks.Task`1.Result" /> property will be set to return the result value of the function.
		/// </param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> that will be assigned to the new task.</param>
		/// <param name="creationOptions">
		/// The <see cref="T:System.Threading.Tasks.TaskCreationOptions">TaskCreationOptions</see> used to
		/// customize the task's behavior.
		/// </param>
		/// <exception cref="T:System.ArgumentException">
		/// The <paramref name="function" /> argument is null.
		/// </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// The <paramref name="creationOptions" /> argument specifies an invalid value for <see cref="T:System.Threading.Tasks.TaskCreationOptions" />.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken">CancellationToken</see>
		/// has already been disposed.
		/// </exception>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task(Func<TResult> function, CancellationToken cancellationToken, TaskCreationOptions creationOptions)
			: this(function, Task.InternalCurrent, cancellationToken, creationOptions, InternalTaskOptions.None, (TaskScheduler)null)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			PossiblyCaptureContext(ref stackMark);
		}

		/// <summary>
		/// Initializes a new <see cref="T:System.Threading.Tasks.Task`1" /> with the specified function and state.
		/// </summary>
		/// <param name="function">
		/// The delegate that represents the code to execute in the task. When the function has completed,
		/// the task's <see cref="P:System.Threading.Tasks.Task`1.Result" /> property will be set to return the result value of the function.
		/// </param>
		/// <param name="state">An object representing data to be used by the action.</param>
		/// <exception cref="T:System.ArgumentException">
		/// The <paramref name="function" /> argument is null.
		/// </exception>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task(Func<object, TResult> function, object state)
			: this(function, state, Task.InternalCurrent, CancellationToken.None, TaskCreationOptions.None, InternalTaskOptions.None, (TaskScheduler)null)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			PossiblyCaptureContext(ref stackMark);
		}

		/// <summary>
		/// Initializes a new <see cref="T:System.Threading.Tasks.Task`1" /> with the specified action, state, and options.
		/// </summary>
		/// <param name="function">
		/// The delegate that represents the code to execute in the task. When the function has completed,
		/// the task's <see cref="P:System.Threading.Tasks.Task`1.Result" /> property will be set to return the result value of the function.
		/// </param>
		/// <param name="state">An object representing data to be used by the function.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> to be assigned to the new task.</param>
		/// <exception cref="T:System.ArgumentException">
		/// The <paramref name="function" /> argument is null.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken">CancellationToken</see>
		/// has already been disposed.
		/// </exception>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task(Func<object, TResult> function, object state, CancellationToken cancellationToken)
			: this(function, state, Task.InternalCurrent, cancellationToken, TaskCreationOptions.None, InternalTaskOptions.None, (TaskScheduler)null)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			PossiblyCaptureContext(ref stackMark);
		}

		/// <summary>
		/// Initializes a new <see cref="T:System.Threading.Tasks.Task`1" /> with the specified action, state, and options.
		/// </summary>
		/// <param name="function">
		/// The delegate that represents the code to execute in the task. When the function has completed,
		/// the task's <see cref="P:System.Threading.Tasks.Task`1.Result" /> property will be set to return the result value of the function.
		/// </param>
		/// <param name="state">An object representing data to be used by the function.</param>
		/// <param name="creationOptions">
		/// The <see cref="T:System.Threading.Tasks.TaskCreationOptions">TaskCreationOptions</see> used to
		/// customize the task's behavior.
		/// </param>
		/// <exception cref="T:System.ArgumentException">
		/// The <paramref name="function" /> argument is null.
		/// </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// The <paramref name="creationOptions" /> argument specifies an invalid value for <see cref="T:System.Threading.Tasks.TaskCreationOptions" />.
		/// </exception>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task(Func<object, TResult> function, object state, TaskCreationOptions creationOptions)
			: this(function, state, Task.InternalCurrent, CancellationToken.None, creationOptions, InternalTaskOptions.None, (TaskScheduler)null)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			PossiblyCaptureContext(ref stackMark);
		}

		/// <summary>
		/// Initializes a new <see cref="T:System.Threading.Tasks.Task`1" /> with the specified action, state, and options.
		/// </summary>
		/// <param name="function">
		/// The delegate that represents the code to execute in the task. When the function has completed,
		/// the task's <see cref="P:System.Threading.Tasks.Task`1.Result" /> property will be set to return the result value of the function.
		/// </param>
		/// <param name="state">An object representing data to be used by the function.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> to be assigned to the new task.</param>
		/// <param name="creationOptions">
		/// The <see cref="T:System.Threading.Tasks.TaskCreationOptions">TaskCreationOptions</see> used to
		/// customize the task's behavior.
		/// </param>
		/// <exception cref="T:System.ArgumentException">
		/// The <paramref name="function" /> argument is null.
		/// </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// The <paramref name="creationOptions" /> argument specifies an invalid value for <see cref="T:System.Threading.Tasks.TaskCreationOptions" />.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken">CancellationToken</see>
		/// has already been disposed.
		/// </exception>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task(Func<object, TResult> function, object state, CancellationToken cancellationToken, TaskCreationOptions creationOptions)
			: this(function, state, Task.InternalCurrent, cancellationToken, creationOptions, InternalTaskOptions.None, (TaskScheduler)null)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			PossiblyCaptureContext(ref stackMark);
		}

		internal Task(Func<TResult> valueSelector, Task parent, CancellationToken cancellationToken, TaskCreationOptions creationOptions, InternalTaskOptions internalOptions, TaskScheduler scheduler, ref StackCrawlMark2 stackMark)
			: this(valueSelector, parent, cancellationToken, creationOptions, internalOptions, scheduler)
		{
			PossiblyCaptureContext(ref stackMark);
		}

		/// <summary>
		/// Creates a new future object.
		/// </summary>
		/// <param name="parent">The parent task for this future.</param>
		/// <param name="valueSelector">A function that yields the future value.</param>
		/// <param name="scheduler">The task scheduler which will be used to execute the future.</param>
		/// <param name="cancellationToken">The CancellationToken for the task.</param>
		/// <param name="creationOptions">Options to control the future's behavior.</param>
		/// <param name="internalOptions">Internal options to control the future's behavior.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="creationOptions" /> argument specifies
		/// a SelfReplicating <see cref="T:System.Threading.Tasks.Task`1" />, which is illegal."/&gt;.</exception>
		internal Task(Func<TResult> valueSelector, Task parent, CancellationToken cancellationToken, TaskCreationOptions creationOptions, InternalTaskOptions internalOptions, TaskScheduler scheduler)
			: base((valueSelector != null) ? new Action<object>(InvokeFuture) : null, null, parent, cancellationToken, creationOptions, internalOptions, scheduler)
		{
			if ((internalOptions & InternalTaskOptions.SelfReplicating) != 0)
			{
				throw new ArgumentOutOfRangeException("creationOptions", Environment2.GetResourceString("TaskT_ctor_SelfReplicating"));
			}
			m_valueSelector = valueSelector;
			m_stateObject = this;
		}

		internal Task(Func<object, TResult> valueSelector, object state, Task parent, CancellationToken cancellationToken, TaskCreationOptions creationOptions, InternalTaskOptions internalOptions, TaskScheduler scheduler, ref StackCrawlMark2 stackMark)
			: this(valueSelector, state, parent, cancellationToken, creationOptions, internalOptions, scheduler)
		{
			PossiblyCaptureContext(ref stackMark);
		}

		/// <summary>
		/// Creates a new future object.
		/// </summary>
		/// <param name="parent">The parent task for this future.</param>
		/// <param name="state">An object containing data to be used by the action; may be null.</param>
		/// <param name="valueSelector">A function that yields the future value.</param>
		/// <param name="cancellationToken">The CancellationToken for the task.</param>
		/// <param name="scheduler">The task scheduler which will be used to execute the future.</param>
		/// <param name="creationOptions">Options to control the future's behavior.</param>
		/// <param name="internalOptions">Internal options to control the future's behavior.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="creationOptions" /> argument specifies
		/// a SelfReplicating <see cref="T:System.Threading.Tasks.Task`1" />, which is illegal."/&gt;.</exception>
		internal Task(Func<object, TResult> valueSelector, object state, Task parent, CancellationToken cancellationToken, TaskCreationOptions creationOptions, InternalTaskOptions internalOptions, TaskScheduler scheduler)
			: base((valueSelector != null) ? new Action<object>(InvokeFuture) : null, null, parent, cancellationToken, creationOptions, internalOptions, scheduler)
		{
			if ((internalOptions & InternalTaskOptions.SelfReplicating) != 0)
			{
				throw new ArgumentOutOfRangeException("creationOptions", Environment2.GetResourceString("TaskT_ctor_SelfReplicating"));
			}
			m_valueSelector = valueSelector;
			m_stateObject = this;
			m_futureState = state;
		}

		internal static Task<TResult> StartNew(Task parent, Func<TResult> function, CancellationToken cancellationToken, TaskCreationOptions creationOptions, InternalTaskOptions internalOptions, TaskScheduler scheduler, ref StackCrawlMark2 stackMark)
		{
			if (function == null)
			{
				throw new ArgumentNullException("function");
			}
			if (scheduler == null)
			{
				throw new ArgumentNullException("scheduler");
			}
			if ((internalOptions & InternalTaskOptions.SelfReplicating) != 0)
			{
				throw new ArgumentOutOfRangeException("creationOptions", Environment2.GetResourceString("TaskT_ctor_SelfReplicating"));
			}
			Task<TResult> task = new Task<TResult>(function, parent, cancellationToken, creationOptions, internalOptions | InternalTaskOptions.QueuedByRuntime, scheduler, ref stackMark);
			task.ScheduleAndStart(needsProtection: false);
			return task;
		}

		internal static Task<TResult> StartNew(Task parent, Func<object, TResult> function, object state, CancellationToken cancellationToken, TaskCreationOptions creationOptions, InternalTaskOptions internalOptions, TaskScheduler scheduler, ref StackCrawlMark2 stackMark)
		{
			if (function == null)
			{
				throw new ArgumentNullException("function");
			}
			if (scheduler == null)
			{
				throw new ArgumentNullException("scheduler");
			}
			if ((internalOptions & InternalTaskOptions.SelfReplicating) != 0)
			{
				throw new ArgumentOutOfRangeException("creationOptions", Environment2.GetResourceString("TaskT_ctor_SelfReplicating"));
			}
			Task<TResult> task = new Task<TResult>(function, state, parent, cancellationToken, creationOptions, internalOptions | InternalTaskOptions.QueuedByRuntime, scheduler, ref stackMark);
			task.ScheduleAndStart(needsProtection: false);
			return task;
		}

		internal bool TrySetResult(TResult result)
		{
			ThrowIfDisposed();
			if (AtomicStateUpdate(67108864, 90177536))
			{
				m_result = result;
				m_resultWasSet = true;
				Finish(bUserDelegateExecuted: false);
				return true;
			}
			return false;
		}

		internal bool TrySetException(object exceptionObject)
		{
			ThrowIfDisposed();
			bool result = false;
			LazyInitializer.EnsureInitialized(ref m_contingentProperties, Task.s_contingentPropertyCreator);
			if (AtomicStateUpdate(67108864, 90177536))
			{
				AddException(exceptionObject);
				Finish(bUserDelegateExecuted: false);
				result = true;
			}
			return result;
		}

		/// <summary>
		/// Evaluates the value selector of the Task which is passed in as an object and stores the result.
		/// </summary>        
		private static void InvokeFuture(object futureAsObj)
		{
			Task<TResult> task = (Task<TResult>)futureAsObj;
			Func<TResult> func = task.m_valueSelector as Func<TResult>;
			try
			{
				if (func != null)
				{
					task.m_result = func();
				}
				else
				{
					task.m_result = ((Func<object, TResult>)task.m_valueSelector)(task.m_futureState);
				}
				task.m_resultWasSet = true;
			}
			finally
			{
				task.m_valueSelector = null;
			}
		}

		/// <summary>
		/// Creates a continuation that executes when the target <see cref="T:System.Threading.Tasks.Task`1" /> completes.
		/// </summary>
		/// <param name="continuationAction">
		/// An action to run when the <see cref="T:System.Threading.Tasks.Task`1" /> completes. When run, the delegate will be
		/// passed the completed task as an argument.
		/// </param>
		/// <returns>A new continuation <see cref="T:System.Threading.Tasks.Task" />.</returns>
		/// <remarks>
		/// The returned <see cref="T:System.Threading.Tasks.Task" /> will not be scheduled for execution until the current task has
		/// completed, whether it completes due to running to completion successfully, faulting due to an
		/// unhandled exception, or exiting out early due to being canceled.
		/// </remarks>
		/// <exception cref="T:System.ArgumentNullException">
		/// The <paramref name="continuationAction" /> argument is null.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">
		/// The <see cref="T:System.Threading.Tasks.Task`1" /> has been disposed.
		/// </exception>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task ContinueWith(Action<Task<TResult>> continuationAction)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return ContinueWith(continuationAction, TaskScheduler.Current, CancellationToken.None, TaskContinuationOptions.None, ref stackMark);
		}

		/// <summary>
		/// Creates a continuation that executes when the target <see cref="T:System.Threading.Tasks.Task`1" /> completes.
		/// </summary>
		/// <param name="continuationAction">
		/// An action to run when the <see cref="T:System.Threading.Tasks.Task`1" /> completes. When run, the delegate will be
		/// passed the completed task as an argument.
		/// </param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> that will be assigned to the new continuation task.</param>
		/// <returns>A new continuation <see cref="T:System.Threading.Tasks.Task" />.</returns>
		/// <remarks>
		/// The returned <see cref="T:System.Threading.Tasks.Task" /> will not be scheduled for execution until the current task has
		/// completed, whether it completes due to running to completion successfully, faulting due to an
		/// unhandled exception, or exiting out early due to being canceled.
		/// </remarks>
		/// <exception cref="T:System.ArgumentNullException">
		/// The <paramref name="continuationAction" /> argument is null.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">
		/// The <see cref="T:System.Threading.Tasks.Task`1" /> has been disposed.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken">CancellationToken</see>
		/// has already been disposed.
		/// </exception>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task ContinueWith(Action<Task<TResult>> continuationAction, CancellationToken cancellationToken)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return ContinueWith(continuationAction, TaskScheduler.Current, cancellationToken, TaskContinuationOptions.None, ref stackMark);
		}

		/// <summary>
		/// Creates a continuation that executes when the target <see cref="T:System.Threading.Tasks.Task`1" /> completes.
		/// </summary>
		/// <param name="continuationAction">
		/// An action to run when the <see cref="T:System.Threading.Tasks.Task`1" /> completes. When run, the delegate will be
		/// passed the completed task as an argument.
		/// </param>
		/// <param name="scheduler">
		/// The <see cref="T:System.Threading.Tasks.TaskScheduler" /> to associate with the continuation task and to use for its execution.
		/// </param>
		/// <returns>A new continuation <see cref="T:System.Threading.Tasks.Task" />.</returns>
		/// <remarks>
		/// The returned <see cref="T:System.Threading.Tasks.Task" /> will not be scheduled for execution until the current task has
		/// completed, whether it completes due to running to completion successfully, faulting due to an
		/// unhandled exception, or exiting out early due to being canceled.
		/// </remarks>
		/// <exception cref="T:System.ArgumentNullException">
		/// The <paramref name="continuationAction" /> argument is null.
		/// </exception>
		/// <exception cref="T:System.ArgumentNullException">
		/// The <paramref name="scheduler" /> argument is null.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">
		/// The <see cref="T:System.Threading.Tasks.Task`1" /> has been disposed.
		/// </exception>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task ContinueWith(Action<Task<TResult>> continuationAction, TaskScheduler scheduler)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return ContinueWith(continuationAction, scheduler, CancellationToken.None, TaskContinuationOptions.None, ref stackMark);
		}

		/// <summary>
		/// Creates a continuation that executes when the target <see cref="T:System.Threading.Tasks.Task`1" /> completes.
		/// </summary>
		/// <param name="continuationAction">
		/// An action to run when the <see cref="T:System.Threading.Tasks.Task`1" /> completes. When run, the delegate will be
		/// passed the completed task as an argument.
		/// </param>
		/// <param name="continuationOptions">
		/// Options for when the continuation is scheduled and how it behaves. This includes criteria, such
		/// as <see cref="F:System.Threading.Tasks.TaskContinuationOptions.OnlyOnCanceled">OnlyOnCanceled</see>, as
		/// well as execution options, such as <see cref="F:System.Threading.Tasks.TaskContinuationOptions.ExecuteSynchronously">ExecuteSynchronously</see>.
		/// </param>
		/// <returns>A new continuation <see cref="T:System.Threading.Tasks.Task" />.</returns>
		/// <remarks>
		/// The returned <see cref="T:System.Threading.Tasks.Task" /> will not be scheduled for execution until the current task has
		/// completed. If the continuation criteria specified through the <paramref name="continuationOptions" /> parameter are not met, the continuation task will be canceled
		/// instead of scheduled.
		/// </remarks>
		/// <exception cref="T:System.ArgumentNullException">
		/// The <paramref name="continuationAction" /> argument is null.
		/// </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// The <paramref name="continuationOptions" /> argument specifies an invalid value for <see cref="T:System.Threading.Tasks.TaskContinuationOptions">TaskContinuationOptions</see>.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">
		/// The <see cref="T:System.Threading.Tasks.Task`1" /> has been disposed.
		/// </exception>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task ContinueWith(Action<Task<TResult>> continuationAction, TaskContinuationOptions continuationOptions)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return ContinueWith(continuationAction, TaskScheduler.Current, CancellationToken.None, continuationOptions, ref stackMark);
		}

		/// <summary>
		/// Creates a continuation that executes when the target <see cref="T:System.Threading.Tasks.Task`1" /> completes.
		/// </summary>
		/// <param name="continuationAction">
		/// An action to run when the <see cref="T:System.Threading.Tasks.Task`1" /> completes. When run, the delegate will be
		/// passed the completed task as an argument.
		/// </param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> that will be assigned to the new continuation task.</param>
		/// <param name="continuationOptions">
		/// Options for when the continuation is scheduled and how it behaves. This includes criteria, such
		/// as <see cref="F:System.Threading.Tasks.TaskContinuationOptions.OnlyOnCanceled">OnlyOnCanceled</see>, as
		/// well as execution options, such as <see cref="F:System.Threading.Tasks.TaskContinuationOptions.ExecuteSynchronously">ExecuteSynchronously</see>.
		/// </param>
		/// <param name="scheduler">
		/// The <see cref="T:System.Threading.Tasks.TaskScheduler" /> to associate with the continuation task and to use for its
		/// execution.
		/// </param>
		/// <returns>A new continuation <see cref="T:System.Threading.Tasks.Task" />.</returns>
		/// <remarks>
		/// The returned <see cref="T:System.Threading.Tasks.Task" /> will not be scheduled for execution until the current task has
		/// completed. If the criteria specified through the <paramref name="continuationOptions" /> parameter
		/// are not met, the continuation task will be canceled instead of scheduled.
		/// </remarks>
		/// <exception cref="T:System.ArgumentNullException">
		/// The <paramref name="continuationAction" /> argument is null.
		/// </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// The <paramref name="continuationOptions" /> argument specifies an invalid value for <see cref="T:System.Threading.Tasks.TaskContinuationOptions">TaskContinuationOptions</see>.
		/// </exception>
		/// <exception cref="T:System.ArgumentNullException">
		/// The <paramref name="scheduler" /> argument is null.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">
		/// The <see cref="T:System.Threading.Tasks.Task`1" /> has been disposed.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken">CancellationToken</see>
		/// has already been disposed.
		/// </exception>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task ContinueWith(Action<Task<TResult>> continuationAction, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return ContinueWith(continuationAction, scheduler, cancellationToken, continuationOptions, ref stackMark);
		}

		internal Task ContinueWith(Action<Task<TResult>> continuationAction, TaskScheduler scheduler, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, ref StackCrawlMark2 stackMark)
		{
			ThrowIfDisposed();
			if (continuationAction == null)
			{
				throw new ArgumentNullException("continuationAction");
			}
			if (scheduler == null)
			{
				throw new ArgumentNullException("scheduler");
			}
			Task.CreationOptionsFromContinuationOptions(continuationOptions, out var creationOptions, out var internalOptions);
			Task<TResult> thisFuture = this;
			Task task = new Task(delegate
			{
				continuationAction(thisFuture);
			}, null, Task.InternalCurrent, cancellationToken, creationOptions, internalOptions, null, ref stackMark);
			ContinueWithCore(task, scheduler, continuationOptions);
			return task;
		}

		/// <summary>
		/// Creates a continuation that executes when the target <see cref="T:System.Threading.Tasks.Task`1" /> completes.
		/// </summary>
		/// <typeparam name="TNewResult">
		/// The type of the result produced by the continuation.
		/// </typeparam>
		/// <param name="continuationFunction">
		/// A function to run when the <see cref="T:System.Threading.Tasks.Task`1" /> completes. When run, the delegate will be
		/// passed the completed task as an argument.
		/// </param>
		/// <returns>A new continuation <see cref="T:System.Threading.Tasks.Task`1" />.</returns>
		/// <remarks>
		/// The returned <see cref="T:System.Threading.Tasks.Task`1" /> will not be scheduled for execution until the current
		/// task has completed, whether it completes due to running to completion successfully, faulting due
		/// to an unhandled exception, or exiting out early due to being canceled.
		/// </remarks>
		/// <exception cref="T:System.ArgumentNullException">
		/// The <paramref name="continuationFunction" /> argument is null.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">
		/// The <see cref="T:System.Threading.Tasks.Task`1" /> has been disposed.
		/// </exception>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task<TNewResult> ContinueWith<TNewResult>(Func<Task<TResult>, TNewResult> continuationFunction)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return ContinueWith(continuationFunction, TaskScheduler.Current, CancellationToken.None, TaskContinuationOptions.None, ref stackMark);
		}

		/// <summary>
		/// Creates a continuation that executes when the target <see cref="T:System.Threading.Tasks.Task`1" /> completes.
		/// </summary>
		/// <typeparam name="TNewResult">
		/// The type of the result produced by the continuation.
		/// </typeparam>
		/// <param name="continuationFunction">
		/// A function to run when the <see cref="T:System.Threading.Tasks.Task`1" /> completes. When run, the delegate will be
		/// passed the completed task as an argument.
		/// </param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> that will be assigned to the new task.</param>
		/// <returns>A new continuation <see cref="T:System.Threading.Tasks.Task`1" />.</returns>
		/// <remarks>
		/// The returned <see cref="T:System.Threading.Tasks.Task`1" /> will not be scheduled for execution until the current
		/// task has completed, whether it completes due to running to completion successfully, faulting due
		/// to an unhandled exception, or exiting out early due to being canceled.
		/// </remarks>
		/// <exception cref="T:System.ArgumentNullException">
		/// The <paramref name="continuationFunction" /> argument is null.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">
		/// The <see cref="T:System.Threading.Tasks.Task`1" /> has been disposed.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken">CancellationToken</see>
		/// has already been disposed.
		/// </exception>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task<TNewResult> ContinueWith<TNewResult>(Func<Task<TResult>, TNewResult> continuationFunction, CancellationToken cancellationToken)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return ContinueWith(continuationFunction, TaskScheduler.Current, cancellationToken, TaskContinuationOptions.None, ref stackMark);
		}

		/// <summary>
		/// Creates a continuation that executes when the target <see cref="T:System.Threading.Tasks.Task`1" /> completes.
		/// </summary>
		/// <typeparam name="TNewResult">
		/// The type of the result produced by the continuation.
		/// </typeparam>
		/// <param name="continuationFunction">
		/// A function to run when the <see cref="T:System.Threading.Tasks.Task`1" /> completes.  When run, the delegate will be
		/// passed the completed task as an argument.
		/// </param>
		/// <param name="scheduler">
		/// The <see cref="T:System.Threading.Tasks.TaskScheduler" /> to associate with the continuation task and to use for its execution.
		/// </param>
		/// <returns>A new continuation <see cref="T:System.Threading.Tasks.Task`1" />.</returns>
		/// <remarks>
		/// The returned <see cref="T:System.Threading.Tasks.Task`1" /> will not be scheduled for execution until the current task has
		/// completed, whether it completes due to running to completion successfully, faulting due to an
		/// unhandled exception, or exiting out early due to being canceled.
		/// </remarks>
		/// <exception cref="T:System.ArgumentNullException">
		/// The <paramref name="continuationFunction" /> argument is null.
		/// </exception>
		/// <exception cref="T:System.ArgumentNullException">
		/// The <paramref name="scheduler" /> argument is null.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">
		/// The <see cref="T:System.Threading.Tasks.Task`1" /> has been disposed.
		/// </exception>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task<TNewResult> ContinueWith<TNewResult>(Func<Task<TResult>, TNewResult> continuationFunction, TaskScheduler scheduler)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return ContinueWith(continuationFunction, scheduler, CancellationToken.None, TaskContinuationOptions.None, ref stackMark);
		}

		/// <summary>
		/// Creates a continuation that executes when the target <see cref="T:System.Threading.Tasks.Task`1" /> completes.
		/// </summary>
		/// <typeparam name="TNewResult">
		/// The type of the result produced by the continuation.
		/// </typeparam>
		/// <param name="continuationFunction">
		/// A function to run when the <see cref="T:System.Threading.Tasks.Task`1" /> completes. When run, the delegate will be
		/// passed the completed task as an argument.
		/// </param>
		/// <param name="continuationOptions">
		/// Options for when the continuation is scheduled and how it behaves. This includes criteria, such
		/// as <see cref="F:System.Threading.Tasks.TaskContinuationOptions.OnlyOnCanceled">OnlyOnCanceled</see>, as
		/// well as execution options, such as <see cref="F:System.Threading.Tasks.TaskContinuationOptions.ExecuteSynchronously">ExecuteSynchronously</see>.
		/// </param>
		/// <returns>A new continuation <see cref="T:System.Threading.Tasks.Task`1" />.</returns>
		/// <remarks>
		/// <para>
		/// The returned <see cref="T:System.Threading.Tasks.Task`1" /> will not be scheduled for execution until the current
		/// task has completed, whether it completes due to running to completion successfully, faulting due
		/// to an unhandled exception, or exiting out early due to being canceled.
		/// </para>
		/// <para>
		/// The <paramref name="continuationFunction" />, when executed, should return a <see cref="T:System.Threading.Tasks.Task`1" />. This task's completion state will be transferred to the task returned
		/// from the ContinueWith call.
		/// </para>
		/// </remarks>
		/// <exception cref="T:System.ArgumentNullException">
		/// The <paramref name="continuationFunction" /> argument is null.
		/// </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// The <paramref name="continuationOptions" /> argument specifies an invalid value for <see cref="T:System.Threading.Tasks.TaskContinuationOptions">TaskContinuationOptions</see>.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">
		/// The <see cref="T:System.Threading.Tasks.Task`1" /> has been disposed.
		/// </exception>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task<TNewResult> ContinueWith<TNewResult>(Func<Task<TResult>, TNewResult> continuationFunction, TaskContinuationOptions continuationOptions)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return ContinueWith(continuationFunction, TaskScheduler.Current, CancellationToken.None, continuationOptions, ref stackMark);
		}

		/// <summary>
		/// Creates a continuation that executes when the target <see cref="T:System.Threading.Tasks.Task`1" /> completes.
		/// </summary>
		/// <typeparam name="TNewResult">
		/// The type of the result produced by the continuation.
		/// </typeparam>
		/// <param name="continuationFunction">
		/// A function to run when the <see cref="T:System.Threading.Tasks.Task`1" /> completes. When run, the delegate will be passed as
		/// an argument this completed task.
		/// </param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> that will be assigned to the new task.</param>
		/// <param name="continuationOptions">
		/// Options for when the continuation is scheduled and how it behaves. This includes criteria, such
		/// as <see cref="F:System.Threading.Tasks.TaskContinuationOptions.OnlyOnCanceled">OnlyOnCanceled</see>, as
		/// well as execution options, such as <see cref="F:System.Threading.Tasks.TaskContinuationOptions.ExecuteSynchronously">ExecuteSynchronously</see>.
		/// </param>
		/// <param name="scheduler">
		/// The <see cref="T:System.Threading.Tasks.TaskScheduler" /> to associate with the continuation task and to use for its
		/// execution.
		/// </param>
		/// <returns>A new continuation <see cref="T:System.Threading.Tasks.Task`1" />.</returns>
		/// <remarks>
		/// <para>
		/// The returned <see cref="T:System.Threading.Tasks.Task`1" /> will not be scheduled for execution until the current task has
		/// completed, whether it completes due to running to completion successfully, faulting due to an
		/// unhandled exception, or exiting out early due to being canceled.
		/// </para>
		/// <para>
		/// The <paramref name="continuationFunction" />, when executed, should return a <see cref="T:System.Threading.Tasks.Task`1" />.
		/// This task's completion state will be transferred to the task returned from the
		/// ContinueWith call.
		/// </para>
		/// </remarks>
		/// <exception cref="T:System.ArgumentNullException">
		/// The <paramref name="continuationFunction" /> argument is null.
		/// </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// The <paramref name="continuationOptions" /> argument specifies an invalid value for <see cref="T:System.Threading.Tasks.TaskContinuationOptions">TaskContinuationOptions</see>.
		/// </exception>
		/// <exception cref="T:System.ArgumentNullException">
		/// The <paramref name="scheduler" /> argument is null.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">
		/// The <see cref="T:System.Threading.Tasks.Task`1" /> has been disposed.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken">CancellationToken</see>
		/// has already been disposed.
		/// </exception>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task<TNewResult> ContinueWith<TNewResult>(Func<Task<TResult>, TNewResult> continuationFunction, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return ContinueWith(continuationFunction, scheduler, cancellationToken, continuationOptions, ref stackMark);
		}

		internal Task<TNewResult> ContinueWith<TNewResult>(Func<Task<TResult>, TNewResult> continuationFunction, TaskScheduler scheduler, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, ref StackCrawlMark2 stackMark)
		{
			ThrowIfDisposed();
			if (continuationFunction == null)
			{
				throw new ArgumentNullException("continuationFunction");
			}
			if (scheduler == null)
			{
				throw new ArgumentNullException("scheduler");
			}
			Task.CreationOptionsFromContinuationOptions(continuationOptions, out var creationOptions, out var internalOptions);
			Task<TResult> thisFuture = this;
			Task<TNewResult> task = new Task<TNewResult>(() => continuationFunction(thisFuture), Task.InternalCurrent, cancellationToken, creationOptions, internalOptions, null, ref stackMark);
			ContinueWithCore(task, scheduler, continuationOptions);
			return task;
		}
	}
}
