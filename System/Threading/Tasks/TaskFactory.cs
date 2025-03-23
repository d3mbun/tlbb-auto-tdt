using System.Runtime.CompilerServices;
using System.Security.Permissions;

namespace System.Threading.Tasks
{
	/// <summary>
	/// Provides support for creating and scheduling
	/// <see cref="T:System.Threading.Tasks.Task{TResult}">Task{TResult}</see> objects.
	/// </summary>
	/// <typeparam name="TResult">The type of the results that are available though 
	/// the <see cref="T:System.Threading.Tasks.Task{TResult}">Task{TResult}</see> objects that are associated with 
	/// the methods in this class.</typeparam>
	/// <remarks>
	/// <para>
	/// There are many common patterns for which tasks are relevant. The <see cref="T:System.Threading.Tasks.TaskFactory`1" />
	/// class encodes some of these patterns into methods that pick up default settings, which are
	/// configurable through its constructors.
	/// </para>
	/// <para>
	/// A default instance of <see cref="T:System.Threading.Tasks.TaskFactory`1" /> is available through the
	/// <see cref="P:System.Threading.Tasks.Task`1.Factory">Task{TResult}.Factory</see> property.
	/// </para>
	/// </remarks>
	[HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
	public class TaskFactory<TResult>
	{
		private CancellationToken m_defaultCancellationToken;

		private TaskScheduler m_defaultScheduler;

		private TaskCreationOptions m_defaultCreationOptions;

		private TaskContinuationOptions m_defaultContinuationOptions;

		private TaskScheduler DefaultScheduler
		{
			get
			{
				if (m_defaultScheduler == null)
				{
					return TaskScheduler.Current;
				}
				return m_defaultScheduler;
			}
		}

		/// <summary>
		/// Gets the default <see cref="T:System.Threading.CancellationToken">CancellationToken</see> of this
		/// TaskFactory.
		/// </summary>
		/// <remarks>
		/// This property returns the default <see cref="P:System.Threading.Tasks.TaskFactory`1.CancellationToken" /> that will be assigned to all 
		/// tasks created by this factory unless another CancellationToken value is explicitly specified 
		/// during the call to the factory methods.
		/// </remarks>
		public CancellationToken CancellationToken => m_defaultCancellationToken;

		/// <summary>
		/// Gets the <see cref="T:System.Threading.Tasks.TaskScheduler">TaskScheduler</see> of this
		/// TaskFactory{TResult}.
		/// </summary>
		/// <remarks>
		/// This property returns the default scheduler for this factory.  It will be used to schedule all 
		/// tasks unless another scheduler is explicitly specified during calls to this factory's methods.  
		/// If null, <see cref="P:System.Threading.Tasks.TaskScheduler.Current">TaskScheduler.Current</see> 
		/// will be used.
		/// </remarks>
		public TaskScheduler Scheduler => m_defaultScheduler;

		/// <summary>
		/// Gets the <see cref="T:System.Threading.Tasks.TaskCreationOptions">TaskCreationOptions
		/// </see> value of this TaskFactory{TResult}.
		/// </summary>
		/// <remarks>
		/// This property returns the default creation options for this factory.  They will be used to create all 
		/// tasks unless other options are explicitly specified during calls to this factory's methods.
		/// </remarks>
		public TaskCreationOptions CreationOptions => m_defaultCreationOptions;

		/// <summary>
		/// Gets the <see cref="T:System.Threading.Tasks.TaskCreationOptions">TaskContinuationOptions
		/// </see> value of this TaskFactory{TResult}.
		/// </summary>
		/// <remarks>
		/// This property returns the default continuation options for this factory.  They will be used to create 
		/// all continuation tasks unless other options are explicitly specified during calls to this factory's methods.
		/// </remarks>
		public TaskContinuationOptions ContinuationOptions => m_defaultContinuationOptions;

		private TaskScheduler GetDefaultScheduler(Task currTask)
		{
			if (m_defaultScheduler != null)
			{
				return m_defaultScheduler;
			}
			if (currTask != null)
			{
				return currTask.ExecutingTaskScheduler;
			}
			return TaskScheduler.Default;
		}

		/// <summary>
		/// Initializes a <see cref="T:System.Threading.Tasks.TaskFactory`1" /> instance with the default configuration.
		/// </summary>
		/// <remarks>
		/// This constructor creates a <see cref="T:System.Threading.Tasks.TaskFactory`1" /> instance with a default configuration. The
		/// <see cref="T:System.Threading.Tasks.TaskCreationOptions" /> property is initialized to
		/// <see cref="F:System.Threading.Tasks.TaskCreationOptions.None">TaskCreationOptions.None</see>, the
		/// <see cref="T:System.Threading.Tasks.TaskContinuationOptions" /> property is initialized to <see cref="F:System.Threading.Tasks.TaskContinuationOptions.None">TaskContinuationOptions.None</see>,
		/// and the <see cref="T:System.Threading.Tasks.TaskScheduler">TaskScheduler</see> property is
		/// initialized to the current scheduler (see <see cref="P:System.Threading.Tasks.TaskScheduler.Current">TaskScheduler.Current</see>).
		/// </remarks>
		public TaskFactory()
			: this(CancellationToken.None, TaskCreationOptions.None, TaskContinuationOptions.None, (TaskScheduler)null)
		{
		}

		/// <summary>
		/// Initializes a <see cref="T:System.Threading.Tasks.TaskFactory`1" /> instance with the default configuration.
		/// </summary>
		/// <param name="cancellationToken">The default <see cref="P:System.Threading.Tasks.TaskFactory`1.CancellationToken" /> that will be assigned 
		/// to tasks created by this <see cref="T:System.Threading.Tasks.TaskFactory" /> unless another CancellationToken is explicitly specified 
		/// while calling the factory methods.</param>
		/// <remarks>
		/// This constructor creates a <see cref="T:System.Threading.Tasks.TaskFactory`1" /> instance with a default configuration. The
		/// <see cref="T:System.Threading.Tasks.TaskCreationOptions" /> property is initialized to
		/// <see cref="F:System.Threading.Tasks.TaskCreationOptions.None">TaskCreationOptions.None</see>, the
		/// <see cref="T:System.Threading.Tasks.TaskContinuationOptions" /> property is initialized to <see cref="F:System.Threading.Tasks.TaskContinuationOptions.None">TaskContinuationOptions.None</see>,
		/// and the <see cref="T:System.Threading.Tasks.TaskScheduler">TaskScheduler</see> property is
		/// initialized to the current scheduler (see <see cref="P:System.Threading.Tasks.TaskScheduler.Current">TaskScheduler.Current</see>).
		/// </remarks>
		public TaskFactory(CancellationToken cancellationToken)
			: this(cancellationToken, TaskCreationOptions.None, TaskContinuationOptions.None, (TaskScheduler)null)
		{
		}

		/// <summary>
		/// Initializes a <see cref="T:System.Threading.Tasks.TaskFactory`1" /> instance with the specified configuration.
		/// </summary>
		/// <param name="scheduler">
		/// The <see cref="T:System.Threading.Tasks.TaskScheduler">
		/// TaskScheduler</see> to use to schedule any tasks created with this TaskFactory{TResult}. A null value
		/// indicates that the current TaskScheduler should be used.
		/// </param>
		/// <remarks>
		/// With this constructor, the
		/// <see cref="T:System.Threading.Tasks.TaskCreationOptions" /> property is initialized to
		/// <see cref="F:System.Threading.Tasks.TaskCreationOptions.None">TaskCreationOptions.None</see>, the
		/// <see cref="T:System.Threading.Tasks.TaskContinuationOptions" /> property is initialized to <see cref="F:System.Threading.Tasks.TaskContinuationOptions.None">TaskContinuationOptions.None</see>,
		/// and the <see cref="T:System.Threading.Tasks.TaskScheduler">TaskScheduler</see> property is
		/// initialized to <paramref name="scheduler" />, unless it's null, in which case the property is
		/// initialized to the current scheduler (see <see cref="P:System.Threading.Tasks.TaskScheduler.Current">TaskScheduler.Current</see>).
		/// </remarks>
		public TaskFactory(TaskScheduler scheduler)
			: this(CancellationToken.None, TaskCreationOptions.None, TaskContinuationOptions.None, scheduler)
		{
		}

		/// <summary>
		/// Initializes a <see cref="T:System.Threading.Tasks.TaskFactory`1" /> instance with the specified configuration.
		/// </summary>
		/// <param name="creationOptions">
		/// The default <see cref="T:System.Threading.Tasks.TaskCreationOptions">
		/// TaskCreationOptions</see> to use when creating tasks with this TaskFactory{TResult}.
		/// </param>
		/// <param name="continuationOptions">
		/// The default <see cref="T:System.Threading.Tasks.TaskContinuationOptions">
		/// TaskContinuationOptions</see> to use when creating continuation tasks with this TaskFactory{TResult}.
		/// </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// The exception that is thrown when the
		/// <paramref name="creationOptions" /> argument or the <paramref name="continuationOptions" />
		/// argument specifies an invalid value.
		/// </exception>
		/// <remarks>
		/// With this constructor, the
		/// <see cref="T:System.Threading.Tasks.TaskCreationOptions" /> property is initialized to <paramref name="creationOptions" />,
		/// the
		/// <see cref="T:System.Threading.Tasks.TaskContinuationOptions" /> property is initialized to <paramref name="continuationOptions" />, and the <see cref="T:System.Threading.Tasks.TaskScheduler">TaskScheduler</see> property is initialized to the
		/// current scheduler (see <see cref="P:System.Threading.Tasks.TaskScheduler.Current">TaskScheduler.Current</see>).
		/// </remarks>
		public TaskFactory(TaskCreationOptions creationOptions, TaskContinuationOptions continuationOptions)
			: this(CancellationToken.None, creationOptions, continuationOptions, (TaskScheduler)null)
		{
		}

		/// <summary>
		/// Initializes a <see cref="T:System.Threading.Tasks.TaskFactory`1" /> instance with the specified configuration.
		/// </summary>
		/// <param name="cancellationToken">The default <see cref="P:System.Threading.Tasks.TaskFactory`1.CancellationToken" /> that will be assigned 
		/// to tasks created by this <see cref="T:System.Threading.Tasks.TaskFactory" /> unless another CancellationToken is explicitly specified 
		/// while calling the factory methods.</param>
		/// <param name="creationOptions">
		/// The default <see cref="T:System.Threading.Tasks.TaskCreationOptions">
		/// TaskCreationOptions</see> to use when creating tasks with this TaskFactory{TResult}.
		/// </param>
		/// <param name="continuationOptions">
		/// The default <see cref="T:System.Threading.Tasks.TaskContinuationOptions">
		/// TaskContinuationOptions</see> to use when creating continuation tasks with this TaskFactory{TResult}.
		/// </param>
		/// <param name="scheduler">
		/// The default <see cref="T:System.Threading.Tasks.TaskScheduler">
		/// TaskScheduler</see> to use to schedule any Tasks created with this TaskFactory{TResult}. A null value
		/// indicates that TaskScheduler.Current should be used.
		/// </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// The exception that is thrown when the
		/// <paramref name="creationOptions" /> argument or the <paramref name="continuationOptions" />
		/// argumentspecifies an invalid value.
		/// </exception>
		/// <remarks>
		/// With this constructor, the
		/// <see cref="T:System.Threading.Tasks.TaskCreationOptions" /> property is initialized to <paramref name="creationOptions" />,
		/// the
		/// <see cref="T:System.Threading.Tasks.TaskContinuationOptions" /> property is initialized to <paramref name="continuationOptions" />, and the <see cref="T:System.Threading.Tasks.TaskScheduler">TaskScheduler</see> property is initialized to
		/// <paramref name="scheduler" />, unless it's null, in which case the property is initialized to the
		/// current scheduler (see <see cref="P:System.Threading.Tasks.TaskScheduler.Current">TaskScheduler.Current</see>).
		/// </remarks>
		public TaskFactory(CancellationToken cancellationToken, TaskCreationOptions creationOptions, TaskContinuationOptions continuationOptions, TaskScheduler scheduler)
		{
			m_defaultCancellationToken = cancellationToken;
			m_defaultScheduler = scheduler;
			m_defaultCreationOptions = creationOptions;
			m_defaultContinuationOptions = continuationOptions;
			TaskFactory.CheckCreationOptions(m_defaultCreationOptions);
			TaskFactory.CheckMultiTaskContinuationOptions(m_defaultContinuationOptions);
		}

		/// <summary>
		/// Creates and starts a <see cref="T:System.Threading.Tasks.Task{TResult}" />.
		/// </summary>
		/// <param name="function">A function delegate that returns the future result to be available through
		/// the <see cref="T:System.Threading.Tasks.Task{TResult}" />.</param>
		/// <returns>The started <see cref="T:System.Threading.Tasks.Task{TResult}" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="function" />
		/// argument is null.</exception>
		/// <remarks>
		/// Calling StartNew is functionally equivalent to creating a <see cref="T:System.Threading.Tasks.Task`1" /> using one
		/// of its constructors and then calling
		/// <see cref="M:System.Threading.Tasks.Task.Start">Start</see> to schedule it for execution.
		/// However, unless creation and scheduling must be separated, StartNew is the recommended approach
		/// for both simplicity and performance.
		/// </remarks>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task<TResult> StartNew(Func<TResult> function)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			Task internalCurrent = Task.InternalCurrent;
			return Task<TResult>.StartNew(internalCurrent, function, m_defaultCancellationToken, m_defaultCreationOptions, InternalTaskOptions.None, GetDefaultScheduler(internalCurrent), ref stackMark);
		}

		/// <summary>
		/// Creates and starts a <see cref="T:System.Threading.Tasks.Task{TResult}" />.
		/// </summary>
		/// <param name="function">A function delegate that returns the future result to be available through
		/// the <see cref="T:System.Threading.Tasks.Task{TResult}" />.</param>
		/// <param name="cancellationToken">The <see cref="P:System.Threading.Tasks.TaskFactory`1.CancellationToken" /> that will be assigned to the new task.</param>
		/// <returns>The started <see cref="T:System.Threading.Tasks.Task{TResult}" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="function" />
		/// argument is null.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken">CancellationToken</see>
		/// has already been disposed.
		/// </exception>
		/// <remarks>
		/// Calling StartNew is functionally equivalent to creating a <see cref="T:System.Threading.Tasks.Task`1" /> using one
		/// of its constructors and then calling
		/// <see cref="M:System.Threading.Tasks.Task.Start">Start</see> to schedule it for execution.
		/// However, unless creation and scheduling must be separated, StartNew is the recommended approach
		/// for both simplicity and performance.
		/// </remarks>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task<TResult> StartNew(Func<TResult> function, CancellationToken cancellationToken)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			Task internalCurrent = Task.InternalCurrent;
			return Task<TResult>.StartNew(internalCurrent, function, cancellationToken, m_defaultCreationOptions, InternalTaskOptions.None, GetDefaultScheduler(internalCurrent), ref stackMark);
		}

		/// <summary>
		/// Creates and starts a <see cref="T:System.Threading.Tasks.Task{TResult}" />.
		/// </summary>
		/// <param name="function">A function delegate that returns the future result to be available through
		/// the <see cref="T:System.Threading.Tasks.Task{TResult}" />.</param>
		/// <param name="creationOptions">A TaskCreationOptions value that controls the behavior of the
		/// created
		/// <see cref="T:System.Threading.Tasks.Task{TResult}" />.</param>
		/// <returns>The started <see cref="T:System.Threading.Tasks.Task{TResult}" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="function" />
		/// argument is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The exception that is thrown when the
		/// <paramref name="creationOptions" /> argument specifies an invalid TaskCreationOptions
		/// value.</exception>
		/// <remarks>
		/// Calling StartNew is functionally equivalent to creating a <see cref="T:System.Threading.Tasks.Task`1" /> using one
		/// of its constructors and then calling
		/// <see cref="M:System.Threading.Tasks.Task.Start">Start</see> to schedule it for execution.
		/// However, unless creation and scheduling must be separated, StartNew is the recommended approach
		/// for both simplicity and performance.
		/// </remarks>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task<TResult> StartNew(Func<TResult> function, TaskCreationOptions creationOptions)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			Task internalCurrent = Task.InternalCurrent;
			return Task<TResult>.StartNew(internalCurrent, function, m_defaultCancellationToken, creationOptions, InternalTaskOptions.None, GetDefaultScheduler(internalCurrent), ref stackMark);
		}

		/// <summary>
		/// Creates and starts a <see cref="T:System.Threading.Tasks.Task{TResult}" />.
		/// </summary>
		/// <param name="function">A function delegate that returns the future result to be available through
		/// the <see cref="T:System.Threading.Tasks.Task{TResult}" />.</param>
		/// <param name="creationOptions">A TaskCreationOptions value that controls the behavior of the
		/// created
		/// <see cref="T:System.Threading.Tasks.Task{TResult}" />.</param>
		/// <param name="cancellationToken">The <see cref="P:System.Threading.Tasks.TaskFactory`1.CancellationToken" /> that will be assigned to the new task.</param>
		/// <param name="scheduler">The <see cref="T:System.Threading.Tasks.TaskScheduler">TaskScheduler</see>
		/// that is used to schedule the created <see cref="T:System.Threading.Tasks.Task{TResult}">
		/// Task{TResult}</see>.</param>
		/// <returns>The started <see cref="T:System.Threading.Tasks.Task{TResult}" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="function" />
		/// argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="scheduler" />
		/// argument is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The exception that is thrown when the
		/// <paramref name="creationOptions" /> argument specifies an invalid TaskCreationOptions
		/// value.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken">CancellationToken</see>
		/// has already been disposed.
		/// </exception>
		/// <remarks>
		/// Calling StartNew is functionally equivalent to creating a <see cref="T:System.Threading.Tasks.Task`1" /> using one
		/// of its constructors and then calling
		/// <see cref="M:System.Threading.Tasks.Task.Start">Start</see> to schedule it for execution.
		/// However, unless creation and scheduling must be separated, StartNew is the recommended approach
		/// for both simplicity and performance.
		/// </remarks>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task<TResult> StartNew(Func<TResult> function, CancellationToken cancellationToken, TaskCreationOptions creationOptions, TaskScheduler scheduler)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return Task<TResult>.StartNew(Task.InternalCurrent, function, cancellationToken, creationOptions, InternalTaskOptions.None, scheduler, ref stackMark);
		}

		/// <summary>
		/// Creates and starts a <see cref="T:System.Threading.Tasks.Task{TResult}" />.
		/// </summary>
		/// <param name="function">A function delegate that returns the future result to be available through
		/// the <see cref="T:System.Threading.Tasks.Task{TResult}" />.</param>
		/// <param name="state">An object containing data to be used by the <paramref name="function" />
		/// delegate.</param>
		/// <returns>The started <see cref="T:System.Threading.Tasks.Task{TResult}" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="function" />
		/// argument is null.</exception>
		/// <remarks>
		/// Calling StartNew is functionally equivalent to creating a <see cref="T:System.Threading.Tasks.Task`1" /> using one
		/// of its constructors and then calling
		/// <see cref="M:System.Threading.Tasks.Task.Start">Start</see> to schedule it for execution.
		/// However, unless creation and scheduling must be separated, StartNew is the recommended approach
		/// for both simplicity and performance.
		/// </remarks>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task<TResult> StartNew(Func<object, TResult> function, object state)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			Task internalCurrent = Task.InternalCurrent;
			return Task<TResult>.StartNew(internalCurrent, function, state, m_defaultCancellationToken, m_defaultCreationOptions, InternalTaskOptions.None, GetDefaultScheduler(internalCurrent), ref stackMark);
		}

		/// <summary>
		/// Creates and starts a <see cref="T:System.Threading.Tasks.Task{TResult}" />.
		/// </summary>
		/// <param name="function">A function delegate that returns the future result to be available through
		/// the <see cref="T:System.Threading.Tasks.Task{TResult}" />.</param>
		/// <param name="state">An object containing data to be used by the <paramref name="function" />
		/// delegate.</param>
		/// <param name="cancellationToken">The <see cref="P:System.Threading.Tasks.TaskFactory`1.CancellationToken" /> that will be assigned to the new task.</param>
		/// <returns>The started <see cref="T:System.Threading.Tasks.Task{TResult}" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="function" />
		/// argument is null.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken">CancellationToken</see>
		/// has already been disposed.
		/// </exception>
		/// <remarks>
		/// Calling StartNew is functionally equivalent to creating a <see cref="T:System.Threading.Tasks.Task`1" /> using one
		/// of its constructors and then calling
		/// <see cref="M:System.Threading.Tasks.Task.Start">Start</see> to schedule it for execution.
		/// However, unless creation and scheduling must be separated, StartNew is the recommended approach
		/// for both simplicity and performance.
		/// </remarks>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task<TResult> StartNew(Func<object, TResult> function, object state, CancellationToken cancellationToken)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			Task internalCurrent = Task.InternalCurrent;
			return Task<TResult>.StartNew(internalCurrent, function, state, cancellationToken, m_defaultCreationOptions, InternalTaskOptions.None, GetDefaultScheduler(internalCurrent), ref stackMark);
		}

		/// <summary>
		/// Creates and starts a <see cref="T:System.Threading.Tasks.Task{TResult}" />.
		/// </summary>
		/// <param name="function">A function delegate that returns the future result to be available through
		/// the <see cref="T:System.Threading.Tasks.Task{TResult}" />.</param>
		/// <param name="state">An object containing data to be used by the <paramref name="function" />
		/// delegate.</param>
		/// <param name="creationOptions">A TaskCreationOptions value that controls the behavior of the
		/// created
		/// <see cref="T:System.Threading.Tasks.Task{TResult}" />.</param>
		/// <returns>The started <see cref="T:System.Threading.Tasks.Task{TResult}" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="function" />
		/// argument is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The exception that is thrown when the
		/// <paramref name="creationOptions" /> argument specifies an invalid TaskCreationOptions
		/// value.</exception>
		/// <remarks>
		/// Calling StartNew is functionally equivalent to creating a <see cref="T:System.Threading.Tasks.Task`1" /> using one
		/// of its constructors and then calling
		/// <see cref="M:System.Threading.Tasks.Task.Start">Start</see> to schedule it for execution.
		/// However, unless creation and scheduling must be separated, StartNew is the recommended approach
		/// for both simplicity and performance.
		/// </remarks>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task<TResult> StartNew(Func<object, TResult> function, object state, TaskCreationOptions creationOptions)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			Task internalCurrent = Task.InternalCurrent;
			return Task<TResult>.StartNew(internalCurrent, function, state, m_defaultCancellationToken, creationOptions, InternalTaskOptions.None, GetDefaultScheduler(internalCurrent), ref stackMark);
		}

		/// <summary>
		/// Creates and starts a <see cref="T:System.Threading.Tasks.Task{TResult}" />.
		/// </summary>
		/// <param name="function">A function delegate that returns the future result to be available through
		/// the <see cref="T:System.Threading.Tasks.Task{TResult}" />.</param>
		/// <param name="state">An object containing data to be used by the <paramref name="function" />
		/// delegate.</param>
		/// <param name="cancellationToken">The <see cref="P:System.Threading.Tasks.TaskFactory`1.CancellationToken" /> that will be assigned to the new task.</param>
		/// <param name="creationOptions">A TaskCreationOptions value that controls the behavior of the
		/// created
		/// <see cref="T:System.Threading.Tasks.Task{TResult}" />.</param>
		/// <param name="scheduler">The <see cref="T:System.Threading.Tasks.TaskScheduler">TaskScheduler</see>
		/// that is used to schedule the created <see cref="T:System.Threading.Tasks.Task{TResult}">
		/// Task{TResult}</see>.</param>
		/// <returns>The started <see cref="T:System.Threading.Tasks.Task{TResult}" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="function" />
		/// argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="scheduler" />
		/// argument is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The exception that is thrown when the
		/// <paramref name="creationOptions" /> argument specifies an invalid TaskCreationOptions
		/// value.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken">CancellationToken</see>
		/// has already been disposed.
		/// </exception>
		/// <remarks>
		/// Calling StartNew is functionally equivalent to creating a <see cref="T:System.Threading.Tasks.Task`1" /> using one
		/// of its constructors and then calling
		/// <see cref="M:System.Threading.Tasks.Task.Start">Start</see> to schedule it for execution.
		/// However, unless creation and scheduling must be separated, StartNew is the recommended approach
		/// for both simplicity and performance.
		/// </remarks>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task<TResult> StartNew(Func<object, TResult> function, object state, CancellationToken cancellationToken, TaskCreationOptions creationOptions, TaskScheduler scheduler)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return Task<TResult>.StartNew(Task.InternalCurrent, function, state, cancellationToken, creationOptions, InternalTaskOptions.None, scheduler, ref stackMark);
		}

		private static void FromAsyncCoreLogic(IAsyncResult iar, Func<IAsyncResult, TResult> endMethod, TaskCompletionSource<TResult> tcs)
		{
			Exception ex = null;
			OperationCanceledException ex2 = null;
			TResult result = default(TResult);
			try
			{
				result = endMethod(iar);
			}
			catch (OperationCanceledException ex3)
			{
				ex2 = ex3;
			}
			catch (Exception ex4)
			{
				ex = ex4;
			}
			finally
			{
				if (ex2 != null)
				{
					tcs.TrySetCanceled();
				}
				else if (ex != null)
				{
					if (tcs.TrySetException(ex) && ex is ThreadAbortException)
					{
						tcs.Task.m_contingentProperties.m_exceptionsHolder.MarkAsHandled(calledFromFinalizer: false);
					}
				}
				else
				{
					tcs.TrySetResult(result);
				}
			}
		}

		/// <summary>
		/// Creates a <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see> that executes an end
		/// method function when a specified <see cref="T:System.IAsyncResult">IAsyncResult</see> completes.
		/// </summary>
		/// <param name="asyncResult">The IAsyncResult whose completion should trigger the processing of the
		/// <paramref name="endMethod" />.</param>
		/// <param name="endMethod">The function delegate that processes the completed <paramref name="asyncResult" />.</param>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="asyncResult" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="endMethod" /> argument is null.</exception>
		/// <returns>A <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see> that represents the
		/// asynchronous operation.</returns>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task<TResult> FromAsync(IAsyncResult asyncResult, Func<IAsyncResult, TResult> endMethod)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return FromAsyncImpl(asyncResult, endMethod, m_defaultCreationOptions, DefaultScheduler, ref stackMark);
		}

		/// <summary>
		/// Creates a <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see> that executes an end
		/// method function when a specified <see cref="T:System.IAsyncResult">IAsyncResult</see> completes.
		/// </summary>
		/// <param name="asyncResult">The IAsyncResult whose completion should trigger the processing of the
		/// <paramref name="endMethod" />.</param>
		/// <param name="endMethod">The function delegate that processes the completed <paramref name="asyncResult" />.</param>
		/// <param name="creationOptions">The TaskCreationOptions value that controls the behavior of the
		/// created <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>.</param>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="asyncResult" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="endMethod" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The exception that is thrown when the
		/// <paramref name="creationOptions" /> argument specifies an invalid TaskCreationOptions
		/// value.</exception>
		/// <returns>A <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see> that represents the
		/// asynchronous operation.</returns>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task<TResult> FromAsync(IAsyncResult asyncResult, Func<IAsyncResult, TResult> endMethod, TaskCreationOptions creationOptions)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return FromAsyncImpl(asyncResult, endMethod, creationOptions, DefaultScheduler, ref stackMark);
		}

		/// <summary>
		/// Creates a <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see> that executes an end
		/// method function when a specified <see cref="T:System.IAsyncResult">IAsyncResult</see> completes.
		/// </summary>
		/// <param name="asyncResult">The IAsyncResult whose completion should trigger the processing of the
		/// <paramref name="endMethod" />.</param>
		/// <param name="endMethod">The function delegate that processes the completed <paramref name="asyncResult" />.</param>
		/// <param name="scheduler">The <see cref="T:System.Threading.Tasks.TaskScheduler">TaskScheduler</see>
		/// that is used to schedule the task that executes the end method.</param>
		/// <param name="creationOptions">The TaskCreationOptions value that controls the behavior of the
		/// created <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>.</param>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="asyncResult" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="endMethod" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="scheduler" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The exception that is thrown when the
		/// <paramref name="creationOptions" /> argument specifies an invalid TaskCreationOptions
		/// value.</exception>
		/// <returns>A <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see> that represents the
		/// asynchronous operation.</returns>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task<TResult> FromAsync(IAsyncResult asyncResult, Func<IAsyncResult, TResult> endMethod, TaskCreationOptions creationOptions, TaskScheduler scheduler)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return FromAsyncImpl(asyncResult, endMethod, creationOptions, scheduler, ref stackMark);
		}

		internal static Task<TResult> FromAsyncImpl(IAsyncResult asyncResult, Func<IAsyncResult, TResult> endMethod, TaskCreationOptions creationOptions, TaskScheduler scheduler, ref StackCrawlMark2 stackMark)
		{
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			if (endMethod == null)
			{
				throw new ArgumentNullException("endMethod");
			}
			if (scheduler == null)
			{
				throw new ArgumentNullException("scheduler");
			}
			TaskFactory.CheckFromAsyncOptions(creationOptions, hasBeginMethod: false);
			TaskCompletionSource<TResult> tcs = new TaskCompletionSource<TResult>(creationOptions);
			Task t = new Task(delegate
			{
				FromAsyncCoreLogic(asyncResult, endMethod, tcs);
			}, null, Task.InternalCurrent, CancellationToken.None, TaskCreationOptions.None, InternalTaskOptions.None, null, ref stackMark);
			if (asyncResult.IsCompleted)
			{
				try
				{
					t.RunSynchronously(scheduler);
				}
				catch (Exception exception)
				{
					tcs.TrySetException(exception);
				}
			}
			else
			{
				ThreadPool.RegisterWaitForSingleObject(asyncResult.AsyncWaitHandle, delegate
				{
					try
					{
						t.RunSynchronously(scheduler);
					}
					catch (Exception exception2)
					{
						tcs.TrySetException(exception2);
					}
				}, null, -1, executeOnlyOnce: true);
			}
			return tcs.Task;
		}

		/// <summary>
		/// Creates a <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see> that represents a pair of
		/// begin and end methods that conform to the Asynchronous Programming Model pattern.
		/// </summary>
		/// <param name="beginMethod">The delegate that begins the asynchronous operation.</param>
		/// <param name="endMethod">The delegate that ends the asynchronous operation.</param>
		/// <param name="state">An object containing data to be used by the <paramref name="beginMethod" />
		/// delegate.</param>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="beginMethod" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="endMethod" /> argument is null.</exception>
		/// <returns>The created <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see> that
		/// represents the asynchronous operation.</returns>
		/// <remarks>
		/// This method throws any exceptions thrown by the <paramref name="beginMethod" />.
		/// </remarks>
		public Task<TResult> FromAsync(Func<AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, object state)
		{
			return FromAsyncImpl(beginMethod, endMethod, state, m_defaultCreationOptions);
		}

		/// <summary>
		/// Creates a <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see> that represents a pair of
		/// begin and end methods that conform to the Asynchronous Programming Model pattern.
		/// </summary>
		/// <param name="beginMethod">The delegate that begins the asynchronous operation.</param>
		/// <param name="endMethod">The delegate that ends the asynchronous operation.</param>
		/// <param name="creationOptions">The TaskCreationOptions value that controls the behavior of the
		/// created <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>.</param>
		/// <param name="state">An object containing data to be used by the <paramref name="beginMethod" />
		/// delegate.</param>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="beginMethod" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="endMethod" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The exception that is thrown when the
		/// <paramref name="creationOptions" /> argument specifies an invalid TaskCreationOptions
		/// value.</exception>
		/// <returns>The created <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see> that
		/// represents the asynchronous operation.</returns>
		/// <remarks>
		/// This method throws any exceptions thrown by the <paramref name="beginMethod" />.
		/// </remarks>
		public Task<TResult> FromAsync(Func<AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, object state, TaskCreationOptions creationOptions)
		{
			return FromAsyncImpl(beginMethod, endMethod, state, creationOptions);
		}

		internal static Task<TResult> FromAsyncImpl(Func<AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, object state, TaskCreationOptions creationOptions)
		{
			if (beginMethod == null)
			{
				throw new ArgumentNullException("beginMethod");
			}
			if (endMethod == null)
			{
				throw new ArgumentNullException("endMethod");
			}
			TaskFactory.CheckFromAsyncOptions(creationOptions, hasBeginMethod: true);
			TaskCompletionSource<TResult> tcs = new TaskCompletionSource<TResult>(state, creationOptions);
			try
			{
				beginMethod(delegate(IAsyncResult iar)
				{
					FromAsyncCoreLogic(iar, endMethod, tcs);
				}, state);
			}
			catch
			{
				tcs.TrySetResult(default(TResult));
				throw;
			}
			return tcs.Task;
		}

		/// <summary>
		/// Creates a <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see> that represents a pair of
		/// begin and end methods that conform to the Asynchronous Programming Model pattern.
		/// </summary>
		/// <typeparam name="TArg1">The type of the first argument passed to the <paramref name="beginMethod" /> delegate.</typeparam>
		/// <param name="beginMethod">The delegate that begins the asynchronous operation.</param>
		/// <param name="endMethod">The delegate that ends the asynchronous operation.</param>
		/// <param name="arg1">The first argument passed to the <paramref name="beginMethod" />
		/// delegate.</param>
		/// <param name="state">An object containing data to be used by the <paramref name="beginMethod" />
		/// delegate.</param>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="beginMethod" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="endMethod" /> argument is null.</exception>
		/// <returns>The created <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see> that
		/// represents the asynchronous operation.</returns>
		/// <remarks>
		/// This method throws any exceptions thrown by the <paramref name="beginMethod" />.
		/// </remarks>
		public Task<TResult> FromAsync<TArg1>(Func<TArg1, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, object state)
		{
			return FromAsyncImpl(beginMethod, endMethod, arg1, state, m_defaultCreationOptions);
		}

		/// <summary>
		/// Creates a <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see> that represents a pair of
		/// begin and end methods that conform to the Asynchronous Programming Model pattern.
		/// </summary>
		/// <typeparam name="TArg1">The type of the first argument passed to the <paramref name="beginMethod" /> delegate.</typeparam>
		/// <param name="beginMethod">The delegate that begins the asynchronous operation.</param>
		/// <param name="endMethod">The delegate that ends the asynchronous operation.</param>
		/// <param name="arg1">The first argument passed to the <paramref name="beginMethod" />
		/// delegate.</param>
		/// <param name="creationOptions">The TaskCreationOptions value that controls the behavior of the
		/// created <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>.</param>
		/// <param name="state">An object containing data to be used by the <paramref name="beginMethod" />
		/// delegate.</param>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="beginMethod" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="endMethod" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The exception that is thrown when the
		/// <paramref name="creationOptions" /> argument specifies an invalid TaskCreationOptions
		/// value.</exception>
		/// <returns>The created <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see> that
		/// represents the asynchronous operation.</returns>
		/// <remarks>
		/// This method throws any exceptions thrown by the <paramref name="beginMethod" />.
		/// </remarks>
		public Task<TResult> FromAsync<TArg1>(Func<TArg1, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, object state, TaskCreationOptions creationOptions)
		{
			return FromAsyncImpl(beginMethod, endMethod, arg1, state, creationOptions);
		}

		internal static Task<TResult> FromAsyncImpl<TArg1>(Func<TArg1, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, object state, TaskCreationOptions creationOptions)
		{
			if (beginMethod == null)
			{
				throw new ArgumentNullException("beginMethod");
			}
			if (endMethod == null)
			{
				throw new ArgumentNullException("endMethod");
			}
			TaskFactory.CheckFromAsyncOptions(creationOptions, hasBeginMethod: true);
			TaskCompletionSource<TResult> tcs = new TaskCompletionSource<TResult>(state, creationOptions);
			try
			{
				beginMethod(arg1, delegate(IAsyncResult iar)
				{
					FromAsyncCoreLogic(iar, endMethod, tcs);
				}, state);
			}
			catch
			{
				tcs.TrySetResult(default(TResult));
				throw;
			}
			return tcs.Task;
		}

		/// <summary>
		/// Creates a <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see> that represents a pair of
		/// begin and end methods that conform to the Asynchronous Programming Model pattern.
		/// </summary>
		/// <typeparam name="TArg1">The type of the first argument passed to the <paramref name="beginMethod" /> delegate.</typeparam>
		/// <typeparam name="TArg2">The type of the second argument passed to <paramref name="beginMethod" />
		/// delegate.</typeparam>
		/// <param name="beginMethod">The delegate that begins the asynchronous operation.</param>
		/// <param name="endMethod">The delegate that ends the asynchronous operation.</param>
		/// <param name="arg1">The first argument passed to the <paramref name="beginMethod" />
		/// delegate.</param>
		/// <param name="arg2">The second argument passed to the <paramref name="beginMethod" />
		/// delegate.</param>
		/// <param name="state">An object containing data to be used by the <paramref name="beginMethod" />
		/// delegate.</param>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="beginMethod" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="endMethod" /> argument is null.</exception>
		/// <returns>The created <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see> that
		/// represents the asynchronous operation.</returns>
		/// <remarks>
		/// This method throws any exceptions thrown by the <paramref name="beginMethod" />.
		/// </remarks>
		public Task<TResult> FromAsync<TArg1, TArg2>(Func<TArg1, TArg2, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, TArg2 arg2, object state)
		{
			return FromAsyncImpl(beginMethod, endMethod, arg1, arg2, state, m_defaultCreationOptions);
		}

		/// <summary>
		/// Creates a <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see> that represents a pair of
		/// begin and end methods that conform to the Asynchronous Programming Model pattern.
		/// </summary>
		/// <typeparam name="TArg1">The type of the first argument passed to the <paramref name="beginMethod" /> delegate.</typeparam>
		/// <typeparam name="TArg2">The type of the second argument passed to <paramref name="beginMethod" />
		/// delegate.</typeparam>
		/// <param name="beginMethod">The delegate that begins the asynchronous operation.</param>
		/// <param name="endMethod">The delegate that ends the asynchronous operation.</param>
		/// <param name="arg1">The first argument passed to the <paramref name="beginMethod" />
		/// delegate.</param>
		/// <param name="arg2">The second argument passed to the <paramref name="beginMethod" />
		/// delegate.</param>
		/// <param name="creationOptions">The TaskCreationOptions value that controls the behavior of the
		/// created <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>.</param>
		/// <param name="state">An object containing data to be used by the <paramref name="beginMethod" />
		/// delegate.</param>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="beginMethod" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="endMethod" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The exception that is thrown when the
		/// <paramref name="creationOptions" /> argument specifies an invalid TaskCreationOptions
		/// value.</exception>
		/// <returns>The created <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see> that
		/// represents the asynchronous operation.</returns>
		/// <remarks>
		/// This method throws any exceptions thrown by the <paramref name="beginMethod" />.
		/// </remarks>
		public Task<TResult> FromAsync<TArg1, TArg2>(Func<TArg1, TArg2, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, TArg2 arg2, object state, TaskCreationOptions creationOptions)
		{
			return FromAsyncImpl(beginMethod, endMethod, arg1, arg2, state, creationOptions);
		}

		internal static Task<TResult> FromAsyncImpl<TArg1, TArg2>(Func<TArg1, TArg2, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, TArg2 arg2, object state, TaskCreationOptions creationOptions)
		{
			if (beginMethod == null)
			{
				throw new ArgumentNullException("beginMethod");
			}
			if (endMethod == null)
			{
				throw new ArgumentNullException("endMethod");
			}
			TaskFactory.CheckFromAsyncOptions(creationOptions, hasBeginMethod: true);
			TaskCompletionSource<TResult> tcs = new TaskCompletionSource<TResult>(state, creationOptions);
			try
			{
				beginMethod(arg1, arg2, delegate(IAsyncResult iar)
				{
					FromAsyncCoreLogic(iar, endMethod, tcs);
				}, state);
			}
			catch
			{
				tcs.TrySetResult(default(TResult));
				throw;
			}
			return tcs.Task;
		}

		/// <summary>
		/// Creates a <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see> that represents a pair of
		/// begin and end methods that conform to the Asynchronous Programming Model pattern.
		/// </summary>
		/// <typeparam name="TArg1">The type of the first argument passed to the <paramref name="beginMethod" /> delegate.</typeparam>
		/// <typeparam name="TArg2">The type of the second argument passed to <paramref name="beginMethod" />
		/// delegate.</typeparam>
		/// <typeparam name="TArg3">The type of the third argument passed to <paramref name="beginMethod" />
		/// delegate.</typeparam>
		/// <param name="beginMethod">The delegate that begins the asynchronous operation.</param>
		/// <param name="endMethod">The delegate that ends the asynchronous operation.</param>
		/// <param name="arg1">The first argument passed to the <paramref name="beginMethod" />
		/// delegate.</param>
		/// <param name="arg2">The second argument passed to the <paramref name="beginMethod" />
		/// delegate.</param>
		/// <param name="arg3">The third argument passed to the <paramref name="beginMethod" />
		/// delegate.</param>
		/// <param name="state">An object containing data to be used by the <paramref name="beginMethod" />
		/// delegate.</param>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="beginMethod" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="endMethod" /> argument is null.</exception>
		/// <returns>The created <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see> that
		/// represents the asynchronous operation.</returns>
		/// <remarks>
		/// This method throws any exceptions thrown by the <paramref name="beginMethod" />.
		/// </remarks>
		public Task<TResult> FromAsync<TArg1, TArg2, TArg3>(Func_<TArg1, TArg2, TArg3, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, TArg2 arg2, TArg3 arg3, object state)
		{
			return FromAsyncImpl(beginMethod, endMethod, arg1, arg2, arg3, state, m_defaultCreationOptions);
		}

		/// <summary>
		/// Creates a <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see> that represents a pair of
		/// begin and end methods that conform to the Asynchronous Programming Model pattern.
		/// </summary>
		/// <typeparam name="TArg1">The type of the first argument passed to the <paramref name="beginMethod" /> delegate.</typeparam>
		/// <typeparam name="TArg2">The type of the second argument passed to <paramref name="beginMethod" />
		/// delegate.</typeparam>
		/// <typeparam name="TArg3">The type of the third argument passed to <paramref name="beginMethod" />
		/// delegate.</typeparam>
		/// <param name="beginMethod">The delegate that begins the asynchronous operation.</param>
		/// <param name="endMethod">The delegate that ends the asynchronous operation.</param>
		/// <param name="arg1">The first argument passed to the <paramref name="beginMethod" />
		/// delegate.</param>
		/// <param name="arg2">The second argument passed to the <paramref name="beginMethod" />
		/// delegate.</param>
		/// <param name="arg3">The third argument passed to the <paramref name="beginMethod" />
		/// delegate.</param>
		/// <param name="creationOptions">The TaskCreationOptions value that controls the behavior of the
		/// created <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>.</param>
		/// <param name="state">An object containing data to be used by the <paramref name="beginMethod" />
		/// delegate.</param>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="beginMethod" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="endMethod" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The exception that is thrown when the
		/// <paramref name="creationOptions" /> argument specifies an invalid TaskCreationOptions
		/// value.</exception>
		/// <returns>The created <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see> that
		/// represents the asynchronous operation.</returns>
		/// <remarks>
		/// This method throws any exceptions thrown by the <paramref name="beginMethod" />.
		/// </remarks>
		public Task<TResult> FromAsync<TArg1, TArg2, TArg3>(Func_<TArg1, TArg2, TArg3, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, TArg2 arg2, TArg3 arg3, object state, TaskCreationOptions creationOptions)
		{
			return FromAsyncImpl(beginMethod, endMethod, arg1, arg2, arg3, state, creationOptions);
		}

		internal static Task<TResult> FromAsyncImpl<TArg1, TArg2, TArg3>(Func_<TArg1, TArg2, TArg3, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, TArg2 arg2, TArg3 arg3, object state, TaskCreationOptions creationOptions)
		{
			if (beginMethod == null)
			{
				throw new ArgumentNullException("beginMethod");
			}
			if (endMethod == null)
			{
				throw new ArgumentNullException("endMethod");
			}
			TaskFactory.CheckFromAsyncOptions(creationOptions, hasBeginMethod: true);
			TaskCompletionSource<TResult> tcs = new TaskCompletionSource<TResult>(state, creationOptions);
			try
			{
				beginMethod(arg1, arg2, arg3, delegate(IAsyncResult iar)
				{
					FromAsyncCoreLogic(iar, endMethod, tcs);
				}, state);
			}
			catch
			{
				tcs.TrySetResult(default(TResult));
				throw;
			}
			return tcs.Task;
		}

		private static Task<TResult> CreateCanceledTask(TaskContinuationOptions continuationOptions)
		{
			Task.CreationOptionsFromContinuationOptions(continuationOptions, out var creationOptions, out var _);
			return new Task<TResult>(canceled: true, default(TResult), creationOptions);
		}

		/// <summary>
		/// Creates a continuation <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see> 
		/// that will be started upon the completion of a set of provided Tasks.
		/// </summary>
		/// <param name="tasks">The array of tasks from which to continue.</param>
		/// <param name="continuationFunction">The function delegate to execute when all tasks in 
		/// the <paramref name="tasks" /> array have completed.</param>
		/// <returns>The new continuation <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>.</returns>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the 
		/// <paramref name="tasks" /> array is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the 
		/// <paramref name="continuationFunction" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the 
		/// <paramref name="tasks" /> array contains a null value.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the 
		/// <paramref name="tasks" /> array is empty.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The exception that is thrown when one 
		/// of the elements in the <paramref name="tasks" /> array has been disposed.</exception>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task<TResult> ContinueWhenAll(Task[] tasks, Func<Task[], TResult> continuationFunction)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return ContinueWhenAll(tasks, continuationFunction, m_defaultContinuationOptions, m_defaultCancellationToken, DefaultScheduler, ref stackMark);
		}

		/// <summary>
		/// Creates a continuation <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see> 
		/// that will be started upon the completion of a set of provided Tasks.
		/// </summary>
		/// <param name="tasks">The array of tasks from which to continue.</param>
		/// <param name="continuationFunction">The function delegate to execute when all tasks in 
		/// the <paramref name="tasks" /> array have completed.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken">CancellationToken</see> 
		/// that will be assigned to the new continuation task.</param>
		/// <returns>The new continuation <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>.</returns>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the 
		/// <paramref name="tasks" /> array is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the 
		/// <paramref name="continuationFunction" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the 
		/// <paramref name="tasks" /> array contains a null value.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the 
		/// <paramref name="tasks" /> array is empty.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The exception that is thrown when one 
		/// of the elements in the <paramref name="tasks" /> array has been disposed.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken">CancellationToken</see>
		/// has already been disposed.
		/// </exception>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task<TResult> ContinueWhenAll(Task[] tasks, Func<Task[], TResult> continuationFunction, CancellationToken cancellationToken)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return ContinueWhenAll(tasks, continuationFunction, m_defaultContinuationOptions, cancellationToken, DefaultScheduler, ref stackMark);
		}

		/// <summary>
		/// Creates a continuation <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>
		/// that will be started upon the completion of a set of provided Tasks.
		/// </summary>
		/// <param name="tasks">The array of tasks from which to continue.</param>
		/// <param name="continuationFunction">The function delegate to execute when all tasks in the <paramref name="tasks" /> array have completed.</param>
		/// <param name="continuationOptions">The <see cref="T:System.Threading.Tasks.TaskContinuationOptions">
		/// TaskContinuationOptions</see> value that controls the behavior of
		/// the created continuation <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>.</param>
		/// <returns>The new continuation <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>.</returns>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="continuationFunction" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array contains a null value.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is empty.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The exception that is thrown when the
		/// <paramref name="continuationOptions" /> argument specifies an invalid TaskContinuationOptions
		/// value.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The exception that is thrown when one 
		/// of the elements in the <paramref name="tasks" /> array has been disposed.</exception>
		/// <remarks>
		/// The NotOn* and OnlyOn* <see cref="T:System.Threading.Tasks.TaskContinuationOptions">TaskContinuationOptions</see>, 
		/// which constrain for which <see cref="T:System.Threading.Tasks.TaskStatus">TaskStatus</see> states a continuation 
		/// will be executed, are illegal with ContinueWhenAll.
		/// </remarks>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task<TResult> ContinueWhenAll(Task[] tasks, Func<Task[], TResult> continuationFunction, TaskContinuationOptions continuationOptions)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return ContinueWhenAll(tasks, continuationFunction, continuationOptions, m_defaultCancellationToken, DefaultScheduler, ref stackMark);
		}

		/// <summary>
		/// Creates a continuation <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>
		/// that will be started upon the completion of a set of provided Tasks.
		/// </summary>
		/// <param name="tasks">The array of tasks from which to continue.</param>
		/// <param name="continuationFunction">The function delegate to execute when all tasks in the <paramref name="tasks" /> array have completed.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken">CancellationToken</see> 
		/// that will be assigned to the new continuation task.</param>
		/// <param name="continuationOptions">The <see cref="T:System.Threading.Tasks.TaskContinuationOptions">
		/// TaskContinuationOptions</see> value that controls the behavior of
		/// the created continuation <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>.</param>
		/// <param name="scheduler">The <see cref="T:System.Threading.Tasks.TaskScheduler">TaskScheduler</see>
		/// that is used to schedule the created continuation <see cref="T:System.Threading.Tasks.Task">Task</see>.</param>
		/// <returns>The new continuation <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>.</returns>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="continuationFunction" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="scheduler" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array contains a null value.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is empty.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The exception that is thrown when the
		/// <paramref name="continuationOptions" /> argument specifies an invalid TaskContinuationOptions
		/// value.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The exception that is thrown when one 
		/// of the elements in the <paramref name="tasks" /> array has been disposed.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken">CancellationToken</see>
		/// has already been disposed.
		/// </exception>
		/// <remarks>
		/// The NotOn* and OnlyOn* <see cref="T:System.Threading.Tasks.TaskContinuationOptions">TaskContinuationOptions</see>, 
		/// which constrain for which <see cref="T:System.Threading.Tasks.TaskStatus">TaskStatus</see> states a continuation 
		/// will be executed, are illegal with ContinueWhenAll.
		/// </remarks>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task<TResult> ContinueWhenAll(Task[] tasks, Func<Task[], TResult> continuationFunction, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return ContinueWhenAll(tasks, continuationFunction, continuationOptions, cancellationToken, scheduler, ref stackMark);
		}

		internal static Task<TResult> ContinueWhenAll(Task[] tasks, Func<Task[], TResult> continuationFunction, TaskContinuationOptions continuationOptions, CancellationToken cancellationToken, TaskScheduler scheduler, ref StackCrawlMark2 stackMark)
		{
			TaskFactory.CheckMultiTaskContinuationOptions(continuationOptions);
			if (tasks == null)
			{
				throw new ArgumentNullException("tasks");
			}
			if (continuationFunction == null)
			{
				throw new ArgumentNullException("continuationFunction");
			}
			if (scheduler == null)
			{
				throw new ArgumentNullException("scheduler");
			}
			Task[] tasksCopy = TaskFactory.CheckMultiContinuationTasksAndCopy(tasks);
			if (cancellationToken.IsCancellationRequested)
			{
				return CreateCanceledTask(continuationOptions);
			}
			Task<bool> task = TaskFactory.CommonCWAllLogic(tasksCopy);
			return task.ContinueWith((Task<bool> finishedTask) => continuationFunction(tasksCopy), scheduler, cancellationToken, continuationOptions, ref stackMark);
		}

		/// <summary>
		/// Creates a continuation <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>
		/// that will be started upon the completion of a set of provided Tasks.
		/// </summary>
		/// <typeparam name="TAntecedentResult">The type of the result of the antecedent <paramref name="tasks" />.</typeparam>
		/// <param name="tasks">The array of tasks from which to continue.</param>
		/// <param name="continuationFunction">The function delegate to execute when all tasks in the
		/// <paramref name="tasks" /> array have completed.</param>
		/// <returns>The new continuation <see cref="T:System.Threading.Tasks.Task{TResult}" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="continuationFunction" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array contains a null value.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is empty.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The exception that is thrown when one 
		/// of the elements in the <paramref name="tasks" /> array has been disposed.</exception>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task<TResult> ContinueWhenAll<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>[], TResult> continuationFunction)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return ContinueWhenAll(tasks, continuationFunction, m_defaultContinuationOptions, m_defaultCancellationToken, DefaultScheduler, ref stackMark);
		}

		/// <summary>
		/// Creates a continuation <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>
		/// that will be started upon the completion of a set of provided Tasks.
		/// </summary>
		/// <typeparam name="TAntecedentResult">The type of the result of the antecedent <paramref name="tasks" />.</typeparam>
		/// <param name="tasks">The array of tasks from which to continue.</param>
		/// <param name="continuationFunction">The function delegate to execute when all tasks in the
		/// <paramref name="tasks" /> array have completed.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken">CancellationToken</see> 
		/// that will be assigned to the new continuation task.</param>
		/// <returns>The new continuation <see cref="T:System.Threading.Tasks.Task{TResult}" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="continuationFunction" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array contains a null value.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is empty.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The exception that is thrown when one 
		/// of the elements in the <paramref name="tasks" /> array has been disposed.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken">CancellationToken</see>
		/// has already been disposed.
		/// </exception>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task<TResult> ContinueWhenAll<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>[], TResult> continuationFunction, CancellationToken cancellationToken)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return ContinueWhenAll(tasks, continuationFunction, m_defaultContinuationOptions, cancellationToken, DefaultScheduler, ref stackMark);
		}

		/// <summary>
		/// Creates a continuation <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>
		/// that will be started upon the completion of a set of provided Tasks.
		/// </summary>
		/// <typeparam name="TAntecedentResult">The type of the result of the antecedent <paramref name="tasks" />.</typeparam>
		/// <param name="tasks">The array of tasks from which to continue.</param>
		/// <param name="continuationFunction">The function delegate to execute when all tasks in the
		/// <paramref name="tasks" /> array have completed.</param>
		/// <param name="continuationOptions">The <see cref="T:System.Threading.Tasks.TaskContinuationOptions">
		/// TaskContinuationOptions</see> value that controls the behavior of
		/// the created continuation <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>.</param>
		/// <returns>The new continuation <see cref="T:System.Threading.Tasks.Task{TResult}" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="continuationFunction" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array contains a null value.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is empty.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The exception that is thrown when the
		/// <paramref name="continuationOptions" /> argument specifies an invalid TaskContinuationOptions
		/// value.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The exception that is thrown when one 
		/// of the elements in the <paramref name="tasks" /> array has been disposed.</exception>
		/// <remarks>
		/// The NotOn* and OnlyOn* <see cref="T:System.Threading.Tasks.TaskContinuationOptions">TaskContinuationOptions</see>, 
		/// which constrain for which <see cref="T:System.Threading.Tasks.TaskStatus">TaskStatus</see> states a continuation 
		/// will be executed, are illegal with ContinueWhenAll.
		/// </remarks>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task<TResult> ContinueWhenAll<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>[], TResult> continuationFunction, TaskContinuationOptions continuationOptions)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return ContinueWhenAll(tasks, continuationFunction, continuationOptions, m_defaultCancellationToken, DefaultScheduler, ref stackMark);
		}

		/// <summary>
		/// Creates a continuation <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>
		/// that will be started upon the completion of a set of provided Tasks.
		/// </summary>
		/// <typeparam name="TAntecedentResult">The type of the result of the antecedent <paramref name="tasks" />.</typeparam>
		/// <param name="tasks">The array of tasks from which to continue.</param>
		/// <param name="continuationFunction">The function delegate to execute when all tasks in the
		/// <paramref name="tasks" /> array have completed.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken">CancellationToken</see> 
		/// that will be assigned to the new continuation task.</param>
		/// <param name="continuationOptions">The <see cref="T:System.Threading.Tasks.TaskContinuationOptions">
		/// TaskContinuationOptions</see> value that controls the behavior of
		/// the created continuation <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>.</param>
		/// <param name="scheduler">The <see cref="T:System.Threading.Tasks.TaskScheduler">TaskScheduler</see>
		/// that is used to schedule the created continuation <see cref="T:System.Threading.Tasks.Task{TResult}" />.</param>
		/// <returns>The new continuation <see cref="T:System.Threading.Tasks.Task{TResult}" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="continuationFunction" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="scheduler" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array contains a null value.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is empty.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The exception that is thrown when the
		/// <paramref name="continuationOptions" /> argument specifies an invalid TaskContinuationOptions
		/// value.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The exception that is thrown when one 
		/// of the elements in the <paramref name="tasks" /> array has been disposed.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken">CancellationToken</see>
		/// has already been disposed.
		/// </exception>
		/// <remarks>
		/// The NotOn* and OnlyOn* <see cref="T:System.Threading.Tasks.TaskContinuationOptions">TaskContinuationOptions</see>, 
		/// which constrain for which <see cref="T:System.Threading.Tasks.TaskStatus">TaskStatus</see> states a continuation 
		/// will be executed, are illegal with ContinueWhenAll.
		/// </remarks>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task<TResult> ContinueWhenAll<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>[], TResult> continuationFunction, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return ContinueWhenAll(tasks, continuationFunction, continuationOptions, cancellationToken, scheduler, ref stackMark);
		}

		internal static Task<TResult> ContinueWhenAll<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>[], TResult> continuationFunction, TaskContinuationOptions continuationOptions, CancellationToken cancellationToken, TaskScheduler scheduler, ref StackCrawlMark2 stackMark)
		{
			TaskFactory.CheckMultiTaskContinuationOptions(continuationOptions);
			if (tasks == null)
			{
				throw new ArgumentNullException("tasks");
			}
			if (continuationFunction == null)
			{
				throw new ArgumentNullException("continuationFunction");
			}
			if (scheduler == null)
			{
				throw new ArgumentNullException("scheduler");
			}
			Task<TAntecedentResult>[] tasksCopy = TaskFactory.CheckMultiContinuationTasksAndCopy(tasks);
			if (cancellationToken.IsCancellationRequested)
			{
				return CreateCanceledTask(continuationOptions);
			}
			Task<bool> task = TaskFactory.CommonCWAllLogic(tasksCopy);
			return task.ContinueWith((Task<bool> finishedTask) => continuationFunction(tasksCopy), scheduler, cancellationToken, continuationOptions, ref stackMark);
		}

		/// <summary>
		/// Creates a continuation <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>
		/// that will be started upon the completion of any Task in the provided set.
		/// </summary>
		/// <param name="tasks">The array of tasks from which to continue when one task completes.</param>
		/// <param name="continuationFunction">The function delegate to execute when one task in the <paramref name="tasks" /> array completes.</param>
		/// <returns>The new continuation <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>.</returns>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="continuationFunction" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array contains a null value.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is empty.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The exception that is thrown when one 
		/// of the elements in the <paramref name="tasks" /> array has been disposed.</exception>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task<TResult> ContinueWhenAny(Task[] tasks, Func<Task, TResult> continuationFunction)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return ContinueWhenAny(tasks, continuationFunction, m_defaultContinuationOptions, m_defaultCancellationToken, DefaultScheduler, ref stackMark);
		}

		/// <summary>
		/// Creates a continuation <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>
		/// that will be started upon the completion of any Task in the provided set.
		/// </summary>
		/// <param name="tasks">The array of tasks from which to continue when one task completes.</param>
		/// <param name="continuationFunction">The function delegate to execute when one task in the <paramref name="tasks" /> array completes.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken">CancellationToken</see> 
		/// that will be assigned to the new continuation task.</param>
		/// <returns>The new continuation <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>.</returns>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="continuationFunction" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array contains a null value.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is empty.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The exception that is thrown when one 
		/// of the elements in the <paramref name="tasks" /> array has been disposed.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken">CancellationToken</see>
		/// has already been disposed.
		/// </exception>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task<TResult> ContinueWhenAny(Task[] tasks, Func<Task, TResult> continuationFunction, CancellationToken cancellationToken)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return ContinueWhenAny(tasks, continuationFunction, m_defaultContinuationOptions, cancellationToken, DefaultScheduler, ref stackMark);
		}

		/// <summary>
		/// Creates a continuation <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>
		/// that will be started upon the completion of any Task in the provided set.
		/// </summary>
		/// <param name="tasks">The array of tasks from which to continue when one task completes.</param>
		/// <param name="continuationFunction">The function delegate to execute when one task in the <paramref name="tasks" /> array completes.</param>
		/// <param name="continuationOptions">The <see cref="T:System.Threading.Tasks.TaskContinuationOptions">
		/// TaskContinuationOptions</see> value that controls the behavior of
		/// the created continuation <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>.</param>
		/// <returns>The new continuation <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>.</returns>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="continuationFunction" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array contains a null value.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is empty.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The exception that is thrown when the
		/// <paramref name="continuationOptions" /> argument specifies an invalid TaskContinuationOptions
		/// value.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The exception that is thrown when one 
		/// of the elements in the <paramref name="tasks" /> array has been disposed.</exception>
		/// <remarks>
		/// The NotOn* and OnlyOn* <see cref="T:System.Threading.Tasks.TaskContinuationOptions">TaskContinuationOptions</see>, 
		/// which constrain for which <see cref="T:System.Threading.Tasks.TaskStatus">TaskStatus</see> states a continuation 
		/// will be executed, are illegal with ContinueWhenAny.
		/// </remarks>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task<TResult> ContinueWhenAny(Task[] tasks, Func<Task, TResult> continuationFunction, TaskContinuationOptions continuationOptions)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return ContinueWhenAny(tasks, continuationFunction, continuationOptions, m_defaultCancellationToken, DefaultScheduler, ref stackMark);
		}

		/// <summary>
		/// Creates a continuation <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>
		/// that will be started upon the completion of any Task in the provided set.
		/// </summary>
		/// <param name="tasks">The array of tasks from which to continue when one task completes.</param>
		/// <param name="continuationFunction">The function delegate to execute when one task in the <paramref name="tasks" /> array completes.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken">CancellationToken</see> 
		/// that will be assigned to the new continuation task.</param>
		/// <param name="continuationOptions">The <see cref="T:System.Threading.Tasks.TaskContinuationOptions">
		/// TaskContinuationOptions</see> value that controls the behavior of
		/// the created continuation <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>.</param>
		/// <param name="scheduler">The <see cref="T:System.Threading.Tasks.TaskScheduler">TaskScheduler</see>
		/// that is used to schedule the created continuation <see cref="T:System.Threading.Tasks.Task">Task</see>.</param>
		/// <returns>The new continuation <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>.</returns>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="continuationFunction" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="scheduler" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array contains a null value.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is empty.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The exception that is thrown when the
		/// <paramref name="continuationOptions" /> argument specifies an invalid TaskContinuationOptions
		/// value.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The exception that is thrown when one 
		/// of the elements in the <paramref name="tasks" /> array has been disposed.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken">CancellationToken</see>
		/// has already been disposed.
		/// </exception>
		/// <remarks>
		/// The NotOn* and OnlyOn* <see cref="T:System.Threading.Tasks.TaskContinuationOptions">TaskContinuationOptions</see>, 
		/// which constrain for which <see cref="T:System.Threading.Tasks.TaskStatus">TaskStatus</see> states a continuation 
		/// will be executed, are illegal with ContinueWhenAny.
		/// </remarks>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task<TResult> ContinueWhenAny(Task[] tasks, Func<Task, TResult> continuationFunction, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return ContinueWhenAny(tasks, continuationFunction, continuationOptions, cancellationToken, scheduler, ref stackMark);
		}

		internal static Task<TResult> ContinueWhenAny(Task[] tasks, Func<Task, TResult> continuationFunction, TaskContinuationOptions continuationOptions, CancellationToken cancellationToken, TaskScheduler scheduler, ref StackCrawlMark2 stackMark)
		{
			TaskFactory.CheckMultiTaskContinuationOptions(continuationOptions);
			if (tasks == null)
			{
				throw new ArgumentNullException("tasks");
			}
			if (continuationFunction == null)
			{
				throw new ArgumentNullException("continuationFunction");
			}
			if (scheduler == null)
			{
				throw new ArgumentNullException("scheduler");
			}
			Task[] tasksCopy = TaskFactory.CheckMultiContinuationTasksAndCopy(tasks);
			if (cancellationToken.IsCancellationRequested)
			{
				return CreateCanceledTask(continuationOptions);
			}
			Task<Task> task = TaskFactory.CommonCWAnyLogic(tasksCopy);
			return task.ContinueWith((Task<Task> completedTask) => continuationFunction(completedTask.Result), scheduler, cancellationToken, continuationOptions, ref stackMark);
		}

		/// <summary>
		/// Creates a continuation <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>
		/// that will be started upon the completion of any Task in the provided set.
		/// </summary>
		/// <typeparam name="TAntecedentResult">The type of the result of the antecedent <paramref name="tasks" />.</typeparam>
		/// <param name="tasks">The array of tasks from which to continue when one task completes.</param>
		/// <param name="continuationFunction">The function delegate to execute when one task in the
		/// <paramref name="tasks" /> array completes.</param>
		/// <returns>The new continuation <see cref="T:System.Threading.Tasks.Task{TResult}" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="continuationFunction" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array contains a null value.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is empty.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The exception that is thrown when one 
		/// of the elements in the <paramref name="tasks" /> array has been disposed.</exception>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task<TResult> ContinueWhenAny<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>, TResult> continuationFunction)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return ContinueWhenAny(tasks, continuationFunction, m_defaultContinuationOptions, m_defaultCancellationToken, DefaultScheduler, ref stackMark);
		}

		/// <summary>
		/// Creates a continuation <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>
		/// that will be started upon the completion of any Task in the provided set.
		/// </summary>
		/// <typeparam name="TAntecedentResult">The type of the result of the antecedent <paramref name="tasks" />.</typeparam>
		/// <param name="tasks">The array of tasks from which to continue when one task completes.</param>
		/// <param name="continuationFunction">The function delegate to execute when one task in the
		/// <paramref name="tasks" /> array completes.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken">CancellationToken</see> 
		/// that will be assigned to the new continuation task.</param>
		/// <returns>The new continuation <see cref="T:System.Threading.Tasks.Task{TResult}" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="continuationFunction" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array contains a null value.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is empty.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The exception that is thrown when one 
		/// of the elements in the <paramref name="tasks" /> array has been disposed.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken">CancellationToken</see>
		/// has already been disposed.
		/// </exception>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task<TResult> ContinueWhenAny<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>, TResult> continuationFunction, CancellationToken cancellationToken)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return ContinueWhenAny(tasks, continuationFunction, m_defaultContinuationOptions, cancellationToken, DefaultScheduler, ref stackMark);
		}

		/// <summary>
		/// Creates a continuation <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>
		/// that will be started upon the completion of any Task in the provided set.
		/// </summary>
		/// <typeparam name="TAntecedentResult">The type of the result of the antecedent <paramref name="tasks" />.</typeparam>
		/// <param name="tasks">The array of tasks from which to continue when one task completes.</param>
		/// <param name="continuationFunction">The function delegate to execute when one task in the
		/// <paramref name="tasks" /> array completes.</param>
		/// <param name="continuationOptions">The <see cref="T:System.Threading.Tasks.TaskContinuationOptions">
		/// TaskContinuationOptions</see> value that controls the behavior of
		/// the created continuation <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>.</param>
		/// <returns>The new continuation <see cref="T:System.Threading.Tasks.Task{TResult}" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="continuationFunction" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array contains a null value.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is empty.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The exception that is thrown when the
		/// <paramref name="continuationOptions" /> argument specifies an invalid TaskContinuationOptions
		/// value.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The exception that is thrown when one 
		/// of the elements in the <paramref name="tasks" /> array has been disposed.</exception>
		/// <remarks>
		/// The NotOn* and OnlyOn* <see cref="T:System.Threading.Tasks.TaskContinuationOptions">TaskContinuationOptions</see>, 
		/// which constrain for which <see cref="T:System.Threading.Tasks.TaskStatus">TaskStatus</see> states a continuation 
		/// will be executed, are illegal with ContinueWhenAny.
		/// </remarks>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task<TResult> ContinueWhenAny<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>, TResult> continuationFunction, TaskContinuationOptions continuationOptions)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return ContinueWhenAny(tasks, continuationFunction, continuationOptions, m_defaultCancellationToken, DefaultScheduler, ref stackMark);
		}

		/// <summary>
		/// Creates a continuation <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>
		/// that will be started upon the completion of any Task in the provided set.
		/// </summary>
		/// <typeparam name="TAntecedentResult">The type of the result of the antecedent <paramref name="tasks" />.</typeparam>
		/// <param name="tasks">The array of tasks from which to continue when one task completes.</param>
		/// <param name="continuationFunction">The function delegate to execute when one task in the
		/// <paramref name="tasks" /> array completes.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken">CancellationToken</see> 
		/// that will be assigned to the new continuation task.</param>
		/// <param name="continuationOptions">The <see cref="T:System.Threading.Tasks.TaskContinuationOptions">
		/// TaskContinuationOptions</see> value that controls the behavior of
		/// the created continuation <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>.</param>
		/// <param name="scheduler">The <see cref="T:System.Threading.Tasks.TaskScheduler">TaskScheduler</see>
		/// that is used to schedule the created continuation <see cref="T:System.Threading.Tasks.Task{TResult}" />.</param>
		/// <returns>The new continuation <see cref="T:System.Threading.Tasks.Task{TResult}" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="continuationFunction" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="scheduler" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array contains a null value.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is empty.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The exception that is thrown when the
		/// <paramref name="continuationOptions" /> argument specifies an invalid TaskContinuationOptions
		/// value.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The exception that is thrown when one 
		/// of the elements in the <paramref name="tasks" /> array has been disposed.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken">CancellationToken</see>
		/// has already been disposed.
		/// </exception>
		/// <remarks>
		/// The NotOn* and OnlyOn* <see cref="T:System.Threading.Tasks.TaskContinuationOptions">TaskContinuationOptions</see>, 
		/// which constrain for which <see cref="T:System.Threading.Tasks.TaskStatus">TaskStatus</see> states a continuation 
		/// will be executed, are illegal with ContinueWhenAny.
		/// </remarks>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task<TResult> ContinueWhenAny<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>, TResult> continuationFunction, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return ContinueWhenAny(tasks, continuationFunction, continuationOptions, cancellationToken, scheduler, ref stackMark);
		}

		internal static Task<TResult> ContinueWhenAny<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>, TResult> continuationFunction, TaskContinuationOptions continuationOptions, CancellationToken cancellationToken, TaskScheduler scheduler, ref StackCrawlMark2 stackMark)
		{
			TaskFactory.CheckMultiTaskContinuationOptions(continuationOptions);
			if (tasks == null)
			{
				throw new ArgumentNullException("tasks");
			}
			if (continuationFunction == null)
			{
				throw new ArgumentNullException("continuationFunction");
			}
			if (scheduler == null)
			{
				throw new ArgumentNullException("scheduler");
			}
			Task<TAntecedentResult>[] array = TaskFactory.CheckMultiContinuationTasksAndCopy(tasks);
			if (cancellationToken.IsCancellationRequested)
			{
				return CreateCanceledTask(continuationOptions);
			}
			Task<Task> task = TaskFactory.CommonCWAnyLogic(array);
			return task.ContinueWith(delegate(Task<Task> completedTask)
			{
				Task<TAntecedentResult> arg = completedTask.Result as Task<TAntecedentResult>;
				return continuationFunction(arg);
			}, scheduler, cancellationToken, continuationOptions, ref stackMark);
		}
	}
	/// <summary>
	/// Provides support for creating and scheduling
	/// <see cref="T:System.Threading.Tasks.Task">Tasks</see>.
	/// </summary>
	/// <remarks>
	/// <para>
	/// There are many common patterns for which tasks are relevant. The <see cref="T:System.Threading.Tasks.TaskFactory" />
	/// class encodes some of these patterns into methods that pick up default settings, which are
	/// configurable through its constructors.
	/// </para>
	/// <para>
	/// A default instance of <see cref="T:System.Threading.Tasks.TaskFactory" /> is available through the
	/// <see cref="P:System.Threading.Tasks.Task.Factory">Task.Factory</see> property.
	/// </para>
	/// </remarks>
	[HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
	public class TaskFactory
	{
		private CancellationToken m_defaultCancellationToken;

		private TaskScheduler m_defaultScheduler;

		private TaskCreationOptions m_defaultCreationOptions;

		private TaskContinuationOptions m_defaultContinuationOptions;

		private TaskScheduler DefaultScheduler
		{
			get
			{
				if (m_defaultScheduler == null)
				{
					return TaskScheduler.Current;
				}
				return m_defaultScheduler;
			}
		}

		/// <summary>
		/// Gets the default <see cref="T:System.Threading.CancellationToken">CancellationToken</see> of this
		/// TaskFactory.
		/// </summary>
		/// <remarks>
		/// This property returns the default <see cref="P:System.Threading.Tasks.TaskFactory.CancellationToken" /> that will be assigned to all 
		/// tasks created by this factory unless another CancellationToken value is explicitly specified 
		/// during the call to the factory methods.
		/// </remarks>
		public CancellationToken CancellationToken => m_defaultCancellationToken;

		/// <summary>
		/// Gets the <see cref="T:System.Threading.Tasks.TaskScheduler">TaskScheduler</see> of this
		/// TaskFactory.
		/// </summary>
		/// <remarks>
		/// This property returns the default scheduler for this factory.  It will be used to schedule all 
		/// tasks unless another scheduler is explicitly specified during calls to this factory's methods.  
		/// If null, <see cref="P:System.Threading.Tasks.TaskScheduler.Current">TaskScheduler.Current</see> 
		/// will be used.
		/// </remarks>
		public TaskScheduler Scheduler => m_defaultScheduler;

		/// <summary>
		/// Gets the <see cref="T:System.Threading.Tasks.TaskCreationOptions">TaskCreationOptions
		/// </see> value of this TaskFactory.
		/// </summary>
		/// <remarks>
		/// This property returns the default creation options for this factory.  They will be used to create all 
		/// tasks unless other options are explicitly specified during calls to this factory's methods.
		/// </remarks>
		public TaskCreationOptions CreationOptions => m_defaultCreationOptions;

		/// <summary>
		/// Gets the <see cref="T:System.Threading.Tasks.TaskCreationOptions">TaskContinuationOptions
		/// </see> value of this TaskFactory.
		/// </summary>
		/// <remarks>
		/// This property returns the default continuation options for this factory.  They will be used to create 
		/// all continuation tasks unless other options are explicitly specified during calls to this factory's methods.
		/// </remarks>
		public TaskContinuationOptions ContinuationOptions => m_defaultContinuationOptions;

		private TaskScheduler GetDefaultScheduler(Task currTask)
		{
			if (m_defaultScheduler != null)
			{
				return m_defaultScheduler;
			}
			if (currTask != null)
			{
				return currTask.ExecutingTaskScheduler;
			}
			return TaskScheduler.Default;
		}

		/// <summary>
		/// Initializes a <see cref="T:System.Threading.Tasks.TaskFactory" /> instance with the default configuration.
		/// </summary>
		/// <remarks>
		/// This constructor creates a <see cref="T:System.Threading.Tasks.TaskFactory" /> instance with a default configuration. The
		/// <see cref="T:System.Threading.Tasks.TaskCreationOptions" /> property is initialized to
		/// <see cref="F:System.Threading.Tasks.TaskCreationOptions.None">TaskCreationOptions.None</see>, the
		/// <see cref="T:System.Threading.Tasks.TaskContinuationOptions" /> property is initialized to <see cref="F:System.Threading.Tasks.TaskContinuationOptions.None">TaskContinuationOptions.None</see>,
		/// and the <see cref="T:System.Threading.Tasks.TaskScheduler">TaskScheduler</see> property is
		/// initialized to the current scheduler (see <see cref="P:System.Threading.Tasks.TaskScheduler.Current">TaskScheduler.Current</see>).
		/// </remarks>
		public TaskFactory()
			: this(CancellationToken.None, TaskCreationOptions.None, TaskContinuationOptions.None, null)
		{
		}

		/// <summary>
		/// Initializes a <see cref="T:System.Threading.Tasks.TaskFactory" /> instance with the specified configuration.
		/// </summary>
		/// <param name="cancellationToken">The default <see cref="P:System.Threading.Tasks.TaskFactory.CancellationToken" /> that will be assigned 
		/// to tasks created by this <see cref="T:System.Threading.Tasks.TaskFactory" /> unless another CancellationToken is explicitly specified 
		/// while calling the factory methods.</param>
		/// <remarks>
		/// This constructor creates a <see cref="T:System.Threading.Tasks.TaskFactory" /> instance with a default configuration. The
		/// <see cref="T:System.Threading.Tasks.TaskCreationOptions" /> property is initialized to
		/// <see cref="F:System.Threading.Tasks.TaskCreationOptions.None">TaskCreationOptions.None</see>, the
		/// <see cref="T:System.Threading.Tasks.TaskContinuationOptions" /> property is initialized to <see cref="F:System.Threading.Tasks.TaskContinuationOptions.None">TaskContinuationOptions.None</see>,
		/// and the <see cref="T:System.Threading.Tasks.TaskScheduler">TaskScheduler</see> property is
		/// initialized to the current scheduler (see <see cref="P:System.Threading.Tasks.TaskScheduler.Current">TaskScheduler.Current</see>).
		/// </remarks>
		public TaskFactory(CancellationToken cancellationToken)
			: this(cancellationToken, TaskCreationOptions.None, TaskContinuationOptions.None, null)
		{
		}

		/// <summary>
		/// Initializes a <see cref="T:System.Threading.Tasks.TaskFactory" /> instance with the specified configuration.
		/// </summary>
		/// <param name="scheduler">
		/// The <see cref="T:System.Threading.Tasks.TaskScheduler">
		/// TaskScheduler</see> to use to schedule any tasks created with this TaskFactory. A null value
		/// indicates that the current TaskScheduler should be used.
		/// </param>
		/// <remarks>
		/// With this constructor, the
		/// <see cref="T:System.Threading.Tasks.TaskCreationOptions" /> property is initialized to
		/// <see cref="F:System.Threading.Tasks.TaskCreationOptions.None">TaskCreationOptions.None</see>, the
		/// <see cref="T:System.Threading.Tasks.TaskContinuationOptions" /> property is initialized to <see cref="F:System.Threading.Tasks.TaskContinuationOptions.None">TaskContinuationOptions.None</see>,
		/// and the <see cref="T:System.Threading.Tasks.TaskScheduler">TaskScheduler</see> property is
		/// initialized to <paramref name="scheduler" />, unless it's null, in which case the property is
		/// initialized to the current scheduler (see <see cref="P:System.Threading.Tasks.TaskScheduler.Current">TaskScheduler.Current</see>).
		/// </remarks>
		public TaskFactory(TaskScheduler scheduler)
			: this(CancellationToken.None, TaskCreationOptions.None, TaskContinuationOptions.None, scheduler)
		{
		}

		/// <summary>
		/// Initializes a <see cref="T:System.Threading.Tasks.TaskFactory" /> instance with the specified configuration.
		/// </summary>
		/// <param name="creationOptions">
		/// The default <see cref="T:System.Threading.Tasks.TaskCreationOptions">
		/// TaskCreationOptions</see> to use when creating tasks with this TaskFactory.
		/// </param>
		/// <param name="continuationOptions">
		/// The default <see cref="T:System.Threading.Tasks.TaskContinuationOptions">
		/// TaskContinuationOptions</see> to use when creating continuation tasks with this TaskFactory.
		/// </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// The exception that is thrown when the
		/// <paramref name="creationOptions" /> argument or the <paramref name="continuationOptions" />
		/// argument specifies an invalid value.
		/// </exception>
		/// <remarks>
		/// With this constructor, the
		/// <see cref="T:System.Threading.Tasks.TaskCreationOptions" /> property is initialized to <paramref name="creationOptions" />,
		/// the
		/// <see cref="T:System.Threading.Tasks.TaskContinuationOptions" /> property is initialized to <paramref name="continuationOptions" />, and the <see cref="T:System.Threading.Tasks.TaskScheduler">TaskScheduler</see> property is initialized to the
		/// current scheduler (see <see cref="P:System.Threading.Tasks.TaskScheduler.Current">TaskScheduler.Current</see>).
		/// </remarks>
		public TaskFactory(TaskCreationOptions creationOptions, TaskContinuationOptions continuationOptions)
			: this(CancellationToken.None, creationOptions, continuationOptions, null)
		{
		}

		/// <summary>
		/// Initializes a <see cref="T:System.Threading.Tasks.TaskFactory" /> instance with the specified configuration.
		/// </summary>
		/// <param name="cancellationToken">The default <see cref="P:System.Threading.Tasks.TaskFactory.CancellationToken" /> that will be assigned 
		/// to tasks created by this <see cref="T:System.Threading.Tasks.TaskFactory" /> unless another CancellationToken is explicitly specified 
		/// while calling the factory methods.</param>
		/// <param name="creationOptions">
		/// The default <see cref="T:System.Threading.Tasks.TaskCreationOptions">
		/// TaskCreationOptions</see> to use when creating tasks with this TaskFactory.
		/// </param>
		/// <param name="continuationOptions">
		/// The default <see cref="T:System.Threading.Tasks.TaskContinuationOptions">
		/// TaskContinuationOptions</see> to use when creating continuation tasks with this TaskFactory.
		/// </param>
		/// <param name="scheduler">
		/// The default <see cref="T:System.Threading.Tasks.TaskScheduler">
		/// TaskScheduler</see> to use to schedule any Tasks created with this TaskFactory. A null value
		/// indicates that TaskScheduler.Current should be used.
		/// </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// The exception that is thrown when the
		/// <paramref name="creationOptions" /> argument or the <paramref name="continuationOptions" />
		/// argumentspecifies an invalid value.
		/// </exception>
		/// <remarks>
		/// With this constructor, the
		/// <see cref="T:System.Threading.Tasks.TaskCreationOptions" /> property is initialized to <paramref name="creationOptions" />,
		/// the
		/// <see cref="T:System.Threading.Tasks.TaskContinuationOptions" /> property is initialized to <paramref name="continuationOptions" />, and the <see cref="T:System.Threading.Tasks.TaskScheduler">TaskScheduler</see> property is initialized to
		/// <paramref name="scheduler" />, unless it's null, in which case the property is initialized to the
		/// current scheduler (see <see cref="P:System.Threading.Tasks.TaskScheduler.Current">TaskScheduler.Current</see>).
		/// </remarks>
		public TaskFactory(CancellationToken cancellationToken, TaskCreationOptions creationOptions, TaskContinuationOptions continuationOptions, TaskScheduler scheduler)
		{
			m_defaultCancellationToken = cancellationToken;
			m_defaultScheduler = scheduler;
			m_defaultCreationOptions = creationOptions;
			m_defaultContinuationOptions = continuationOptions;
			CheckCreationOptions(m_defaultCreationOptions);
			CheckMultiTaskContinuationOptions(m_defaultContinuationOptions);
		}

		internal static void CheckCreationOptions(TaskCreationOptions creationOptions)
		{
			if (((uint)creationOptions & 0xFFFFFFF8u) != 0)
			{
				throw new ArgumentOutOfRangeException("creationOptions");
			}
		}

		/// <summary>
		/// Creates and starts a <see cref="T:System.Threading.Tasks.Task">Task</see>.
		/// </summary>
		/// <param name="action">The action delegate to execute asynchronously.</param>
		/// <returns>The started <see cref="T:System.Threading.Tasks.Task">Task</see>.</returns>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="action" /> 
		/// argument is null.</exception>
		/// <remarks>
		/// Calling StartNew is functionally equivalent to creating a Task using one of its constructors 
		/// and then calling 
		/// <see cref="M:System.Threading.Tasks.Task.Start">Start</see> to schedule it for execution.  However,
		/// unless creation and scheduling must be separated, StartNew is the recommended
		/// approach for both simplicity and performance.
		/// </remarks>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task StartNew(Action action)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			Task internalCurrent = Task.InternalCurrent;
			return Task.InternalStartNew(internalCurrent, action, null, m_defaultCancellationToken, GetDefaultScheduler(internalCurrent), m_defaultCreationOptions, InternalTaskOptions.None, ref stackMark);
		}

		/// <summary>
		/// Creates and starts a <see cref="T:System.Threading.Tasks.Task">Task</see>.
		/// </summary>
		/// <param name="action">The action delegate to execute asynchronously.</param>
		/// <param name="cancellationToken">The <see cref="P:System.Threading.Tasks.TaskFactory.CancellationToken" /> that will be assigned to the new task.</param>
		/// <returns>The started <see cref="T:System.Threading.Tasks.Task">Task</see>.</returns>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="action" /> 
		/// argument is null.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken">CancellationToken</see>
		/// has already been disposed.
		/// </exception>
		/// <remarks>
		/// Calling StartNew is functionally equivalent to creating a Task using one of its constructors 
		/// and then calling 
		/// <see cref="M:System.Threading.Tasks.Task.Start">Start</see> to schedule it for execution.  However,
		/// unless creation and scheduling must be separated, StartNew is the recommended
		/// approach for both simplicity and performance.
		/// </remarks>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task StartNew(Action action, CancellationToken cancellationToken)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			Task internalCurrent = Task.InternalCurrent;
			return Task.InternalStartNew(internalCurrent, action, null, cancellationToken, GetDefaultScheduler(internalCurrent), m_defaultCreationOptions, InternalTaskOptions.None, ref stackMark);
		}

		/// <summary>
		/// Creates and starts a <see cref="T:System.Threading.Tasks.Task">Task</see>.
		/// </summary>
		/// <param name="action">The action delegate to execute asynchronously.</param>
		/// <param name="creationOptions">A TaskCreationOptions value that controls the behavior of the
		/// created
		/// <see cref="T:System.Threading.Tasks.Task">Task.</see></param>
		/// <returns>The started <see cref="T:System.Threading.Tasks.Task">Task</see>.</returns>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="action" />
		/// argument is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The exception that is thrown when the
		/// <paramref name="creationOptions" /> argument specifies an invalid TaskCreationOptions
		/// value.</exception>
		/// <remarks>
		/// Calling StartNew is functionally equivalent to creating a Task using one of its constructors and
		/// then calling
		/// <see cref="M:System.Threading.Tasks.Task.Start">Start</see> to schedule it for execution.
		/// However, unless creation and scheduling must be separated, StartNew is the recommended approach
		/// for both simplicity and performance.
		/// </remarks>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task StartNew(Action action, TaskCreationOptions creationOptions)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			Task internalCurrent = Task.InternalCurrent;
			return Task.InternalStartNew(internalCurrent, action, null, m_defaultCancellationToken, GetDefaultScheduler(internalCurrent), creationOptions, InternalTaskOptions.None, ref stackMark);
		}

		/// <summary>
		/// Creates and starts a <see cref="T:System.Threading.Tasks.Task">Task</see>.
		/// </summary>
		/// <param name="action">The action delegate to execute asynchronously.</param>
		/// <param name="cancellationToken">The <see cref="P:System.Threading.Tasks.TaskFactory.CancellationToken" /> that will be assigned to the new <see cref="T:System.Threading.Tasks.Task" /></param>
		/// <param name="creationOptions">A TaskCreationOptions value that controls the behavior of the
		/// created
		/// <see cref="T:System.Threading.Tasks.Task">Task.</see></param>
		/// <param name="scheduler">The <see cref="T:System.Threading.Tasks.TaskScheduler">TaskScheduler</see>
		/// that is used to schedule the created <see cref="T:System.Threading.Tasks.Task">Task</see>.</param>
		/// <returns>The started <see cref="T:System.Threading.Tasks.Task">Task</see>.</returns>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="action" />
		/// argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="scheduler" />
		/// argument is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The exception that is thrown when the
		/// <paramref name="creationOptions" /> argument specifies an invalid TaskCreationOptions
		/// value.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken">CancellationToken</see>
		/// has already been disposed.
		/// </exception>
		/// <remarks>
		/// Calling StartNew is functionally equivalent to creating a Task using one of its constructors and
		/// then calling
		/// <see cref="M:System.Threading.Tasks.Task.Start">Start</see> to schedule it for execution.
		/// However, unless creation and scheduling must be separated, StartNew is the recommended approach
		/// for both simplicity and performance.
		/// </remarks>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task StartNew(Action action, CancellationToken cancellationToken, TaskCreationOptions creationOptions, TaskScheduler scheduler)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return Task.InternalStartNew(Task.InternalCurrent, action, null, cancellationToken, scheduler, creationOptions, InternalTaskOptions.None, ref stackMark);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal Task StartNew(Action action, CancellationToken cancellationToken, TaskCreationOptions creationOptions, InternalTaskOptions internalOptions, TaskScheduler scheduler)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return Task.InternalStartNew(Task.InternalCurrent, action, null, cancellationToken, scheduler, creationOptions, internalOptions, ref stackMark);
		}

		/// <summary>
		/// Creates and starts a <see cref="T:System.Threading.Tasks.Task">Task</see>.
		/// </summary>
		/// <param name="action">The action delegate to execute asynchronously.</param>
		/// <param name="state">An object containing data to be used by the <paramref name="action" />
		/// delegate.</param>
		/// <returns>The started <see cref="T:System.Threading.Tasks.Task">Task</see>.</returns>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="action" />
		/// argument is null.</exception>
		/// <remarks>
		/// Calling StartNew is functionally equivalent to creating a Task using one of its constructors and
		/// then calling
		/// <see cref="M:System.Threading.Tasks.Task.Start">Start</see> to schedule it for execution.
		/// However, unless creation and scheduling must be separated, StartNew is the recommended approach
		/// for both simplicity and performance.
		/// </remarks>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task StartNew(Action<object> action, object state)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			Task internalCurrent = Task.InternalCurrent;
			return Task.InternalStartNew(internalCurrent, action, state, m_defaultCancellationToken, GetDefaultScheduler(internalCurrent), m_defaultCreationOptions, InternalTaskOptions.None, ref stackMark);
		}

		/// <summary>
		/// Creates and starts a <see cref="T:System.Threading.Tasks.Task">Task</see>.
		/// </summary>
		/// <param name="action">The action delegate to execute asynchronously.</param>
		/// <param name="state">An object containing data to be used by the <paramref name="action" />
		/// delegate.</param>
		/// <param name="cancellationToken">The <see cref="P:System.Threading.Tasks.TaskFactory.CancellationToken" /> that will be assigned to the new <see cref="T:System.Threading.Tasks.Task" /></param>
		/// <returns>The started <see cref="T:System.Threading.Tasks.Task">Task</see>.</returns>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="action" />
		/// argument is null.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken">CancellationToken</see>
		/// has already been disposed.
		/// </exception>
		/// <remarks>
		/// Calling StartNew is functionally equivalent to creating a Task using one of its constructors and
		/// then calling
		/// <see cref="M:System.Threading.Tasks.Task.Start">Start</see> to schedule it for execution.
		/// However, unless creation and scheduling must be separated, StartNew is the recommended approach
		/// for both simplicity and performance.
		/// </remarks>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task StartNew(Action<object> action, object state, CancellationToken cancellationToken)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			Task internalCurrent = Task.InternalCurrent;
			return Task.InternalStartNew(internalCurrent, action, state, cancellationToken, GetDefaultScheduler(internalCurrent), m_defaultCreationOptions, InternalTaskOptions.None, ref stackMark);
		}

		/// <summary>
		/// Creates and starts a <see cref="T:System.Threading.Tasks.Task">Task</see>.
		/// </summary>
		/// <param name="action">The action delegate to execute asynchronously.</param>
		/// <param name="state">An object containing data to be used by the <paramref name="action" />
		/// delegate.</param>
		/// <param name="creationOptions">A TaskCreationOptions value that controls the behavior of the
		/// created
		/// <see cref="T:System.Threading.Tasks.Task">Task.</see></param>
		/// <returns>The started <see cref="T:System.Threading.Tasks.Task">Task</see>.</returns>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="action" />
		/// argument is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The exception that is thrown when the
		/// <paramref name="creationOptions" /> argument specifies an invalid TaskCreationOptions
		/// value.</exception>
		/// <remarks>
		/// Calling StartNew is functionally equivalent to creating a Task using one of its constructors and
		/// then calling
		/// <see cref="M:System.Threading.Tasks.Task.Start">Start</see> to schedule it for execution.
		/// However, unless creation and scheduling must be separated, StartNew is the recommended approach
		/// for both simplicity and performance.
		/// </remarks>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task StartNew(Action<object> action, object state, TaskCreationOptions creationOptions)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			Task internalCurrent = Task.InternalCurrent;
			return Task.InternalStartNew(internalCurrent, action, state, m_defaultCancellationToken, GetDefaultScheduler(internalCurrent), creationOptions, InternalTaskOptions.None, ref stackMark);
		}

		/// <summary>
		/// Creates and starts a <see cref="T:System.Threading.Tasks.Task">Task</see>.
		/// </summary>
		/// <param name="action">The action delegate to execute asynchronously.</param>
		/// <param name="state">An object containing data to be used by the <paramref name="action" />
		/// delegate.</param>
		/// <param name="cancellationToken">The <see cref="P:System.Threading.Tasks.TaskFactory.CancellationToken" /> that will be assigned to the new task.</param>
		/// <param name="creationOptions">A TaskCreationOptions value that controls the behavior of the
		/// created
		/// <see cref="T:System.Threading.Tasks.Task">Task.</see></param>
		/// <param name="scheduler">The <see cref="T:System.Threading.Tasks.TaskScheduler">TaskScheduler</see>
		/// that is used to schedule the created <see cref="T:System.Threading.Tasks.Task">Task</see>.</param>
		/// <returns>The started <see cref="T:System.Threading.Tasks.Task">Task</see>.</returns>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="action" />
		/// argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="scheduler" />
		/// argument is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The exception that is thrown when the
		/// <paramref name="creationOptions" /> argument specifies an invalid TaskCreationOptions
		/// value.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken">CancellationToken</see>
		/// has already been disposed.
		/// </exception>
		/// <remarks>
		/// Calling StartNew is functionally equivalent to creating a Task using one of its constructors and
		/// then calling
		/// <see cref="M:System.Threading.Tasks.Task.Start">Start</see> to schedule it for execution.
		/// However, unless creation and scheduling must be separated, StartNew is the recommended approach
		/// for both simplicity and performance.
		/// </remarks>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task StartNew(Action<object> action, object state, CancellationToken cancellationToken, TaskCreationOptions creationOptions, TaskScheduler scheduler)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return Task.InternalStartNew(Task.InternalCurrent, action, state, cancellationToken, scheduler, creationOptions, InternalTaskOptions.None, ref stackMark);
		}

		/// <summary>
		/// Creates and starts a <see cref="T:System.Threading.Tasks.Task{TResult}" />.
		/// </summary>
		/// <typeparam name="TResult">The type of the result available through the
		/// <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>.
		/// </typeparam>
		/// <param name="function">A function delegate that returns the future result to be available through
		/// the <see cref="T:System.Threading.Tasks.Task{TResult}" />.</param>
		/// <returns>The started <see cref="T:System.Threading.Tasks.Task{TResult}" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="function" />
		/// argument is null.</exception>
		/// <remarks>
		/// Calling StartNew is functionally equivalent to creating a <see cref="T:System.Threading.Tasks.Task`1" /> using one
		/// of its constructors and then calling
		/// <see cref="M:System.Threading.Tasks.Task.Start">Start</see> to schedule it for execution.
		/// However, unless creation and scheduling must be separated, StartNew is the recommended approach
		/// for both simplicity and performance.
		/// </remarks>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task<TResult> StartNew<TResult>(Func<TResult> function)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			Task internalCurrent = Task.InternalCurrent;
			return Task<TResult>.StartNew(internalCurrent, function, m_defaultCancellationToken, m_defaultCreationOptions, InternalTaskOptions.None, GetDefaultScheduler(internalCurrent), ref stackMark);
		}

		/// <summary>
		/// Creates and starts a <see cref="T:System.Threading.Tasks.Task{TResult}" />.
		/// </summary>
		/// <typeparam name="TResult">The type of the result available through the
		/// <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>.
		/// </typeparam>
		/// <param name="function">A function delegate that returns the future result to be available through
		/// the <see cref="T:System.Threading.Tasks.Task{TResult}" />.</param>
		/// <param name="cancellationToken">The <see cref="P:System.Threading.Tasks.TaskFactory.CancellationToken" /> that will be assigned to the new <see cref="T:System.Threading.Tasks.Task" /></param>
		/// <returns>The started <see cref="T:System.Threading.Tasks.Task{TResult}" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="function" />
		/// argument is null.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken">CancellationToken</see>
		/// has already been disposed.
		/// </exception>
		/// <remarks>
		/// Calling StartNew is functionally equivalent to creating a <see cref="T:System.Threading.Tasks.Task`1" /> using one
		/// of its constructors and then calling
		/// <see cref="M:System.Threading.Tasks.Task.Start">Start</see> to schedule it for execution.
		/// However, unless creation and scheduling must be separated, StartNew is the recommended approach
		/// for both simplicity and performance.
		/// </remarks>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task<TResult> StartNew<TResult>(Func<TResult> function, CancellationToken cancellationToken)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			Task internalCurrent = Task.InternalCurrent;
			return Task<TResult>.StartNew(internalCurrent, function, cancellationToken, m_defaultCreationOptions, InternalTaskOptions.None, GetDefaultScheduler(internalCurrent), ref stackMark);
		}

		/// <summary>
		/// Creates and starts a <see cref="T:System.Threading.Tasks.Task{TResult}" />.
		/// </summary>
		/// <typeparam name="TResult">The type of the result available through the
		/// <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>.
		/// </typeparam>
		/// <param name="function">A function delegate that returns the future result to be available through
		/// the <see cref="T:System.Threading.Tasks.Task{TResult}" />.</param>
		/// <param name="creationOptions">A TaskCreationOptions value that controls the behavior of the
		/// created
		/// <see cref="T:System.Threading.Tasks.Task{TResult}" />.</param>
		/// <returns>The started <see cref="T:System.Threading.Tasks.Task{TResult}" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="function" />
		/// argument is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The exception that is thrown when the
		/// <paramref name="creationOptions" /> argument specifies an invalid TaskCreationOptions
		/// value.</exception>
		/// <remarks>
		/// Calling StartNew is functionally equivalent to creating a <see cref="T:System.Threading.Tasks.Task`1" /> using one
		/// of its constructors and then calling
		/// <see cref="M:System.Threading.Tasks.Task.Start">Start</see> to schedule it for execution.
		/// However, unless creation and scheduling must be separated, StartNew is the recommended approach
		/// for both simplicity and performance.
		/// </remarks>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task<TResult> StartNew<TResult>(Func<TResult> function, TaskCreationOptions creationOptions)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			Task internalCurrent = Task.InternalCurrent;
			return Task<TResult>.StartNew(internalCurrent, function, m_defaultCancellationToken, creationOptions, InternalTaskOptions.None, GetDefaultScheduler(internalCurrent), ref stackMark);
		}

		/// <summary>
		/// Creates and starts a <see cref="T:System.Threading.Tasks.Task{TResult}" />.
		/// </summary>
		/// <typeparam name="TResult">The type of the result available through the
		/// <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>.
		/// </typeparam>
		/// <param name="function">A function delegate that returns the future result to be available through
		/// the <see cref="T:System.Threading.Tasks.Task{TResult}" />.</param>
		/// <param name="cancellationToken">The <see cref="P:System.Threading.Tasks.TaskFactory.CancellationToken" /> that will be assigned to the new task.</param>
		/// <param name="creationOptions">A TaskCreationOptions value that controls the behavior of the
		/// created
		/// <see cref="T:System.Threading.Tasks.Task{TResult}" />.</param>
		/// <param name="scheduler">The <see cref="T:System.Threading.Tasks.TaskScheduler">TaskScheduler</see>
		/// that is used to schedule the created <see cref="T:System.Threading.Tasks.Task{TResult}">
		/// Task{TResult}</see>.</param>
		/// <returns>The started <see cref="T:System.Threading.Tasks.Task{TResult}" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="function" />
		/// argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="scheduler" />
		/// argument is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The exception that is thrown when the
		/// <paramref name="creationOptions" /> argument specifies an invalid TaskCreationOptions
		/// value.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken">CancellationToken</see>
		/// has already been disposed.
		/// </exception>
		/// <remarks>
		/// Calling StartNew is functionally equivalent to creating a <see cref="T:System.Threading.Tasks.Task`1" /> using one
		/// of its constructors and then calling
		/// <see cref="M:System.Threading.Tasks.Task.Start">Start</see> to schedule it for execution.
		/// However, unless creation and scheduling must be separated, StartNew is the recommended approach
		/// for both simplicity and performance.
		/// </remarks>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task<TResult> StartNew<TResult>(Func<TResult> function, CancellationToken cancellationToken, TaskCreationOptions creationOptions, TaskScheduler scheduler)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return Task<TResult>.StartNew(Task.InternalCurrent, function, cancellationToken, creationOptions, InternalTaskOptions.None, scheduler, ref stackMark);
		}

		/// <summary>
		/// Creates and starts a <see cref="T:System.Threading.Tasks.Task{TResult}" />.
		/// </summary>
		/// <typeparam name="TResult">The type of the result available through the
		/// <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>.
		/// </typeparam>
		/// <param name="function">A function delegate that returns the future result to be available through
		/// the <see cref="T:System.Threading.Tasks.Task{TResult}" />.</param>
		/// <param name="state">An object containing data to be used by the <paramref name="function" />
		/// delegate.</param>
		/// <returns>The started <see cref="T:System.Threading.Tasks.Task{TResult}" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="function" />
		/// argument is null.</exception>
		/// <remarks>
		/// Calling StartNew is functionally equivalent to creating a <see cref="T:System.Threading.Tasks.Task`1" /> using one
		/// of its constructors and then calling
		/// <see cref="M:System.Threading.Tasks.Task.Start">Start</see> to schedule it for execution.
		/// However, unless creation and scheduling must be separated, StartNew is the recommended approach
		/// for both simplicity and performance.
		/// </remarks>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task<TResult> StartNew<TResult>(Func<object, TResult> function, object state)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			Task internalCurrent = Task.InternalCurrent;
			return Task<TResult>.StartNew(internalCurrent, function, state, m_defaultCancellationToken, m_defaultCreationOptions, InternalTaskOptions.None, GetDefaultScheduler(internalCurrent), ref stackMark);
		}

		/// <summary>
		/// Creates and starts a <see cref="T:System.Threading.Tasks.Task{TResult}" />.
		/// </summary>
		/// <typeparam name="TResult">The type of the result available through the
		/// <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>.
		/// </typeparam>
		/// <param name="function">A function delegate that returns the future result to be available through
		/// the <see cref="T:System.Threading.Tasks.Task{TResult}" />.</param>
		/// <param name="state">An object containing data to be used by the <paramref name="function" />
		/// delegate.</param>
		/// <param name="cancellationToken">The <see cref="P:System.Threading.Tasks.TaskFactory.CancellationToken" /> that will be assigned to the new <see cref="T:System.Threading.Tasks.Task" /></param>
		/// <returns>The started <see cref="T:System.Threading.Tasks.Task{TResult}" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="function" />
		/// argument is null.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken">CancellationToken</see>
		/// has already been disposed.
		/// </exception>
		/// <remarks>
		/// Calling StartNew is functionally equivalent to creating a <see cref="T:System.Threading.Tasks.Task`1" /> using one
		/// of its constructors and then calling
		/// <see cref="M:System.Threading.Tasks.Task.Start">Start</see> to schedule it for execution.
		/// However, unless creation and scheduling must be separated, StartNew is the recommended approach
		/// for both simplicity and performance.
		/// </remarks>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task<TResult> StartNew<TResult>(Func<object, TResult> function, object state, CancellationToken cancellationToken)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			Task internalCurrent = Task.InternalCurrent;
			return Task<TResult>.StartNew(internalCurrent, function, state, cancellationToken, m_defaultCreationOptions, InternalTaskOptions.None, GetDefaultScheduler(internalCurrent), ref stackMark);
		}

		/// <summary>
		/// Creates and starts a <see cref="T:System.Threading.Tasks.Task{TResult}" />.
		/// </summary>
		/// <typeparam name="TResult">The type of the result available through the
		/// <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>.
		/// </typeparam>
		/// <param name="function">A function delegate that returns the future result to be available through
		/// the <see cref="T:System.Threading.Tasks.Task{TResult}" />.</param>
		/// <param name="state">An object containing data to be used by the <paramref name="function" />
		/// delegate.</param>
		/// <param name="creationOptions">A TaskCreationOptions value that controls the behavior of the
		/// created
		/// <see cref="T:System.Threading.Tasks.Task{TResult}" />.</param>
		/// <returns>The started <see cref="T:System.Threading.Tasks.Task{TResult}" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="function" />
		/// argument is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The exception that is thrown when the
		/// <paramref name="creationOptions" /> argument specifies an invalid TaskCreationOptions
		/// value.</exception>
		/// <remarks>
		/// Calling StartNew is functionally equivalent to creating a <see cref="T:System.Threading.Tasks.Task`1" /> using one
		/// of its constructors and then calling
		/// <see cref="M:System.Threading.Tasks.Task.Start">Start</see> to schedule it for execution.
		/// However, unless creation and scheduling must be separated, StartNew is the recommended approach
		/// for both simplicity and performance.
		/// </remarks>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task<TResult> StartNew<TResult>(Func<object, TResult> function, object state, TaskCreationOptions creationOptions)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			Task internalCurrent = Task.InternalCurrent;
			return Task<TResult>.StartNew(internalCurrent, function, state, m_defaultCancellationToken, creationOptions, InternalTaskOptions.None, GetDefaultScheduler(internalCurrent), ref stackMark);
		}

		/// <summary>
		/// Creates and starts a <see cref="T:System.Threading.Tasks.Task{TResult}" />.
		/// </summary>
		/// <typeparam name="TResult">The type of the result available through the
		/// <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>.
		/// </typeparam>
		/// <param name="function">A function delegate that returns the future result to be available through
		/// the <see cref="T:System.Threading.Tasks.Task{TResult}" />.</param>
		/// <param name="state">An object containing data to be used by the <paramref name="function" />
		/// delegate.</param>
		/// <param name="cancellationToken">The <see cref="P:System.Threading.Tasks.TaskFactory.CancellationToken" /> that will be assigned to the new task.</param>
		/// <param name="creationOptions">A TaskCreationOptions value that controls the behavior of the
		/// created
		/// <see cref="T:System.Threading.Tasks.Task{TResult}" />.</param>
		/// <param name="scheduler">The <see cref="T:System.Threading.Tasks.TaskScheduler">TaskScheduler</see>
		/// that is used to schedule the created <see cref="T:System.Threading.Tasks.Task{TResult}">
		/// Task{TResult}</see>.</param>
		/// <returns>The started <see cref="T:System.Threading.Tasks.Task{TResult}" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="function" />
		/// argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="scheduler" />
		/// argument is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The exception that is thrown when the
		/// <paramref name="creationOptions" /> argument specifies an invalid TaskCreationOptions
		/// value.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken">CancellationToken</see>
		/// has already been disposed.
		/// </exception>
		/// <remarks>
		/// Calling StartNew is functionally equivalent to creating a <see cref="T:System.Threading.Tasks.Task`1" /> using one
		/// of its constructors and then calling
		/// <see cref="M:System.Threading.Tasks.Task.Start">Start</see> to schedule it for execution.
		/// However, unless creation and scheduling must be separated, StartNew is the recommended approach
		/// for both simplicity and performance.
		/// </remarks>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task<TResult> StartNew<TResult>(Func<object, TResult> function, object state, CancellationToken cancellationToken, TaskCreationOptions creationOptions, TaskScheduler scheduler)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return Task<TResult>.StartNew(Task.InternalCurrent, function, state, cancellationToken, creationOptions, InternalTaskOptions.None, scheduler, ref stackMark);
		}

		private static void FromAsyncCoreLogic(IAsyncResult iar, Action<IAsyncResult> endMethod, TaskCompletionSource<object> tcs)
		{
			Exception ex = null;
			OperationCanceledException ex2 = null;
			try
			{
				endMethod(iar);
			}
			catch (OperationCanceledException ex3)
			{
				ex2 = ex3;
			}
			catch (Exception ex4)
			{
				ex = ex4;
			}
			finally
			{
				if (ex2 != null)
				{
					tcs.TrySetCanceled();
				}
				else if (ex != null)
				{
					if (tcs.TrySetException(ex) && ex is ThreadAbortException)
					{
						tcs.Task.m_contingentProperties.m_exceptionsHolder.MarkAsHandled(calledFromFinalizer: false);
					}
				}
				else
				{
					tcs.TrySetResult(null);
				}
			}
		}

		/// <summary>
		/// Creates a <see cref="T:System.Threading.Tasks.Task">Task</see> that executes an end method action
		/// when a specified <see cref="T:System.IAsyncResult">IAsyncResult</see> completes.
		/// </summary>
		/// <param name="asyncResult">The IAsyncResult whose completion should trigger the processing of the
		/// <paramref name="endMethod" />.</param>
		/// <param name="endMethod">The action delegate that processes the completed <paramref name="asyncResult" />.</param>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="asyncResult" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="endMethod" /> argument is null.</exception>
		/// <returns>A <see cref="T:System.Threading.Tasks.Task">Task</see> that represents the asynchronous
		/// operation.</returns>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task FromAsync(IAsyncResult asyncResult, Action<IAsyncResult> endMethod)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return FromAsync(asyncResult, endMethod, m_defaultCreationOptions, DefaultScheduler, ref stackMark);
		}

		/// <summary>
		/// Creates a <see cref="T:System.Threading.Tasks.Task">Task</see> that executes an end method action
		/// when a specified <see cref="T:System.IAsyncResult">IAsyncResult</see> completes.
		/// </summary>
		/// <param name="asyncResult">The IAsyncResult whose completion should trigger the processing of the
		/// <paramref name="endMethod" />.</param>
		/// <param name="endMethod">The action delegate that processes the completed <paramref name="asyncResult" />.</param>
		/// <param name="creationOptions">The TaskCreationOptions value that controls the behavior of the
		/// created <see cref="T:System.Threading.Tasks.Task">Task</see>.</param>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="asyncResult" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="endMethod" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The exception that is thrown when the
		/// <paramref name="creationOptions" /> argument specifies an invalid TaskCreationOptions
		/// value.</exception>
		/// <returns>A <see cref="T:System.Threading.Tasks.Task">Task</see> that represents the asynchronous
		/// operation.</returns>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task FromAsync(IAsyncResult asyncResult, Action<IAsyncResult> endMethod, TaskCreationOptions creationOptions)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return FromAsync(asyncResult, endMethod, creationOptions, DefaultScheduler, ref stackMark);
		}

		/// <summary>
		/// Creates a <see cref="T:System.Threading.Tasks.Task">Task</see> that executes an end method action
		/// when a specified <see cref="T:System.IAsyncResult">IAsyncResult</see> completes.
		/// </summary>
		/// <param name="asyncResult">The IAsyncResult whose completion should trigger the processing of the
		/// <paramref name="endMethod" />.</param>
		/// <param name="endMethod">The action delegate that processes the completed <paramref name="asyncResult" />.</param>
		/// <param name="scheduler">The <see cref="T:System.Threading.Tasks.TaskScheduler">TaskScheduler</see>
		/// that is used to schedule the task that executes the end method.</param>
		/// <param name="creationOptions">The TaskCreationOptions value that controls the behavior of the
		/// created <see cref="T:System.Threading.Tasks.Task">Task</see>.</param>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="asyncResult" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="endMethod" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="scheduler" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The exception that is thrown when the
		/// <paramref name="creationOptions" /> argument specifies an invalid TaskCreationOptions
		/// value.</exception>
		/// <returns>A <see cref="T:System.Threading.Tasks.Task">Task</see> that represents the asynchronous
		/// operation.</returns>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task FromAsync(IAsyncResult asyncResult, Action<IAsyncResult> endMethod, TaskCreationOptions creationOptions, TaskScheduler scheduler)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return FromAsync(asyncResult, endMethod, creationOptions, scheduler, ref stackMark);
		}

		private Task FromAsync(IAsyncResult asyncResult, Action<IAsyncResult> endMethod, TaskCreationOptions creationOptions, TaskScheduler scheduler, ref StackCrawlMark2 stackMark)
		{
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			if (endMethod == null)
			{
				throw new ArgumentNullException("endMethod");
			}
			if (scheduler == null)
			{
				throw new ArgumentNullException("scheduler");
			}
			CheckFromAsyncOptions(creationOptions, hasBeginMethod: false);
			TaskCompletionSource<object> tcs = new TaskCompletionSource<object>(null, creationOptions);
			Task t = new Task(delegate
			{
				FromAsyncCoreLogic(asyncResult, endMethod, tcs);
			}, null, Task.InternalCurrent, CancellationToken.None, TaskCreationOptions.None, InternalTaskOptions.None, null, ref stackMark);
			if (asyncResult.IsCompleted)
			{
				try
				{
					t.RunSynchronously(scheduler);
				}
				catch (Exception exception)
				{
					tcs.TrySetException(exception);
				}
			}
			else
			{
				ThreadPool.RegisterWaitForSingleObject(asyncResult.AsyncWaitHandle, delegate
				{
					try
					{
						t.RunSynchronously(scheduler);
					}
					catch (Exception exception2)
					{
						tcs.TrySetException(exception2);
					}
				}, null, -1, executeOnlyOnce: true);
			}
			return tcs.Task;
		}

		/// <summary>
		/// Creates a <see cref="T:System.Threading.Tasks.Task">Task</see> that represents a pair of begin
		/// and end methods that conform to the Asynchronous Programming Model pattern.
		/// </summary>
		/// <param name="beginMethod">The delegate that begins the asynchronous operation.</param>
		/// <param name="endMethod">The delegate that ends the asynchronous operation.</param>
		/// <param name="state">An object containing data to be used by the <paramref name="beginMethod" />
		/// delegate.</param>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="beginMethod" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="endMethod" /> argument is null.</exception>
		/// <returns>The created <see cref="T:System.Threading.Tasks.Task">Task</see> that represents the
		/// asynchronous operation.</returns>
		/// <remarks>
		/// This method throws any exceptions thrown by the <paramref name="beginMethod" />.
		/// </remarks>
		public Task FromAsync(Func<AsyncCallback, object, IAsyncResult> beginMethod, Action<IAsyncResult> endMethod, object state)
		{
			return FromAsync(beginMethod, endMethod, state, m_defaultCreationOptions);
		}

		/// <summary>
		/// Creates a <see cref="T:System.Threading.Tasks.Task">Task</see> that represents a pair of begin
		/// and end methods that conform to the Asynchronous Programming Model pattern.
		/// </summary>
		/// <param name="beginMethod">The delegate that begins the asynchronous operation.</param>
		/// <param name="endMethod">The delegate that ends the asynchronous operation.</param>
		/// <param name="creationOptions">The TaskCreationOptions value that controls the behavior of the
		/// created <see cref="T:System.Threading.Tasks.Task">Task</see>.</param>
		/// <param name="state">An object containing data to be used by the <paramref name="beginMethod" />
		/// delegate.</param>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="beginMethod" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="endMethod" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The exception that is thrown when the
		/// <paramref name="creationOptions" /> argument specifies an invalid TaskCreationOptions
		/// value.</exception>
		/// <returns>The created <see cref="T:System.Threading.Tasks.Task">Task</see> that represents the
		/// asynchronous operation.</returns>
		/// <remarks>
		/// This method throws any exceptions thrown by the <paramref name="beginMethod" />.
		/// </remarks>
		public Task FromAsync(Func<AsyncCallback, object, IAsyncResult> beginMethod, Action<IAsyncResult> endMethod, object state, TaskCreationOptions creationOptions)
		{
			if (beginMethod == null)
			{
				throw new ArgumentNullException("beginMethod");
			}
			if (endMethod == null)
			{
				throw new ArgumentNullException("endMethod");
			}
			CheckFromAsyncOptions(creationOptions, hasBeginMethod: true);
			TaskCompletionSource<object> tcs = new TaskCompletionSource<object>(state, creationOptions);
			try
			{
				beginMethod(delegate(IAsyncResult iar)
				{
					FromAsyncCoreLogic(iar, endMethod, tcs);
				}, state);
			}
			catch
			{
				tcs.TrySetResult(null);
				throw;
			}
			return tcs.Task;
		}

		/// <summary>
		/// Creates a <see cref="T:System.Threading.Tasks.Task">Task</see> that represents a pair of begin
		/// and end methods that conform to the Asynchronous Programming Model pattern.
		/// </summary>
		/// <typeparam name="TArg1">The type of the first argument passed to the <paramref name="beginMethod" />
		/// delegate.</typeparam>
		/// <param name="beginMethod">The delegate that begins the asynchronous operation.</param>
		/// <param name="endMethod">The delegate that ends the asynchronous operation.</param>
		/// <param name="arg1">The first argument passed to the <paramref name="beginMethod" />
		/// delegate.</param>
		/// <param name="state">An object containing data to be used by the <paramref name="beginMethod" />
		/// delegate.</param>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="beginMethod" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="endMethod" /> argument is null.</exception>
		/// <returns>The created <see cref="T:System.Threading.Tasks.Task">Task</see> that represents the
		/// asynchronous operation.</returns>
		/// <remarks>
		/// This method throws any exceptions thrown by the <paramref name="beginMethod" />.
		/// </remarks>
		public Task FromAsync<TArg1>(Func<TArg1, AsyncCallback, object, IAsyncResult> beginMethod, Action<IAsyncResult> endMethod, TArg1 arg1, object state)
		{
			return FromAsync(beginMethod, endMethod, arg1, state, m_defaultCreationOptions);
		}

		/// <summary>
		/// Creates a <see cref="T:System.Threading.Tasks.Task">Task</see> that represents a pair of begin
		/// and end methods that conform to the Asynchronous Programming Model pattern.
		/// </summary>
		/// <typeparam name="TArg1">The type of the first argument passed to the <paramref name="beginMethod" />
		/// delegate.</typeparam>
		/// <param name="beginMethod">The delegate that begins the asynchronous operation.</param>
		/// <param name="endMethod">The delegate that ends the asynchronous operation.</param>
		/// <param name="arg1">The first argument passed to the <paramref name="beginMethod" />
		/// delegate.</param>
		/// <param name="creationOptions">The TaskCreationOptions value that controls the behavior of the
		/// created <see cref="T:System.Threading.Tasks.Task">Task</see>.</param>
		/// <param name="state">An object containing data to be used by the <paramref name="beginMethod" />
		/// delegate.</param>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="beginMethod" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="endMethod" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The exception that is thrown when the
		/// <paramref name="creationOptions" /> argument specifies an invalid TaskCreationOptions
		/// value.</exception>
		/// <returns>The created <see cref="T:System.Threading.Tasks.Task">Task</see> that represents the
		/// asynchronous operation.</returns>
		/// <remarks>
		/// This method throws any exceptions thrown by the <paramref name="beginMethod" />.
		/// </remarks>
		public Task FromAsync<TArg1>(Func<TArg1, AsyncCallback, object, IAsyncResult> beginMethod, Action<IAsyncResult> endMethod, TArg1 arg1, object state, TaskCreationOptions creationOptions)
		{
			if (beginMethod == null)
			{
				throw new ArgumentNullException("beginMethod");
			}
			if (endMethod == null)
			{
				throw new ArgumentNullException("endMethod");
			}
			CheckFromAsyncOptions(creationOptions, hasBeginMethod: true);
			TaskCompletionSource<object> tcs = new TaskCompletionSource<object>(state, creationOptions);
			try
			{
				beginMethod(arg1, delegate(IAsyncResult iar)
				{
					FromAsyncCoreLogic(iar, endMethod, tcs);
				}, state);
			}
			catch
			{
				tcs.TrySetResult(null);
				throw;
			}
			return tcs.Task;
		}

		/// <summary>
		/// Creates a <see cref="T:System.Threading.Tasks.Task">Task</see> that represents a pair of begin
		/// and end methods that conform to the Asynchronous Programming Model pattern.
		/// </summary>
		/// <typeparam name="TArg1">The type of the first argument passed to the <paramref name="beginMethod" />
		/// delegate.</typeparam>
		/// <typeparam name="TArg2">The type of the second argument passed to <paramref name="beginMethod" />
		/// delegate.</typeparam>
		/// <param name="beginMethod">The delegate that begins the asynchronous operation.</param>
		/// <param name="endMethod">The delegate that ends the asynchronous operation.</param>
		/// <param name="arg1">The first argument passed to the <paramref name="beginMethod" />
		/// delegate.</param>
		/// <param name="arg2">The second argument passed to the <paramref name="beginMethod" />
		/// delegate.</param>
		/// <param name="state">An object containing data to be used by the <paramref name="beginMethod" />
		/// delegate.</param>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="beginMethod" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="endMethod" /> argument is null.</exception>
		/// <returns>The created <see cref="T:System.Threading.Tasks.Task">Task</see> that represents the
		/// asynchronous operation.</returns>
		/// <remarks>
		/// This method throws any exceptions thrown by the <paramref name="beginMethod" />.
		/// </remarks>
		public Task FromAsync<TArg1, TArg2>(Func<TArg1, TArg2, AsyncCallback, object, IAsyncResult> beginMethod, Action<IAsyncResult> endMethod, TArg1 arg1, TArg2 arg2, object state)
		{
			return FromAsync(beginMethod, endMethod, arg1, arg2, state, m_defaultCreationOptions);
		}

		/// <summary>
		/// Creates a <see cref="T:System.Threading.Tasks.Task">Task</see> that represents a pair of begin
		/// and end methods that conform to the Asynchronous Programming Model pattern.
		/// </summary>
		/// <typeparam name="TArg1">The type of the first argument passed to the <paramref name="beginMethod" />
		/// delegate.</typeparam>
		/// <typeparam name="TArg2">The type of the second argument passed to <paramref name="beginMethod" />
		/// delegate.</typeparam>
		/// <param name="beginMethod">The delegate that begins the asynchronous operation.</param>
		/// <param name="endMethod">The delegate that ends the asynchronous operation.</param>
		/// <param name="arg1">The first argument passed to the <paramref name="beginMethod" />
		/// delegate.</param>
		/// <param name="arg2">The second argument passed to the <paramref name="beginMethod" />
		/// delegate.</param>
		/// <param name="creationOptions">The TaskCreationOptions value that controls the behavior of the
		/// created <see cref="T:System.Threading.Tasks.Task">Task</see>.</param>
		/// <param name="state">An object containing data to be used by the <paramref name="beginMethod" />
		/// delegate.</param>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="beginMethod" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="endMethod" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The exception that is thrown when the
		/// <paramref name="creationOptions" /> argument specifies an invalid TaskCreationOptions
		/// value.</exception>
		/// <returns>The created <see cref="T:System.Threading.Tasks.Task">Task</see> that represents the
		/// asynchronous operation.</returns>
		/// <remarks>
		/// This method throws any exceptions thrown by the <paramref name="beginMethod" />.
		/// </remarks>
		public Task FromAsync<TArg1, TArg2>(Func<TArg1, TArg2, AsyncCallback, object, IAsyncResult> beginMethod, Action<IAsyncResult> endMethod, TArg1 arg1, TArg2 arg2, object state, TaskCreationOptions creationOptions)
		{
			if (beginMethod == null)
			{
				throw new ArgumentNullException("beginMethod");
			}
			if (endMethod == null)
			{
				throw new ArgumentNullException("endMethod");
			}
			CheckFromAsyncOptions(creationOptions, hasBeginMethod: true);
			TaskCompletionSource<object> tcs = new TaskCompletionSource<object>(state, creationOptions);
			try
			{
				beginMethod(arg1, arg2, delegate(IAsyncResult iar)
				{
					FromAsyncCoreLogic(iar, endMethod, tcs);
				}, state);
			}
			catch
			{
				tcs.TrySetResult(null);
				throw;
			}
			return tcs.Task;
		}

		/// <summary>
		/// Creates a <see cref="T:System.Threading.Tasks.Task">Task</see> that represents a pair of begin
		/// and end methods that conform to the Asynchronous Programming Model pattern.
		/// </summary>
		/// <typeparam name="TArg1">The type of the first argument passed to the <paramref name="beginMethod" />
		/// delegate.</typeparam>
		/// <typeparam name="TArg2">The type of the second argument passed to <paramref name="beginMethod" />
		/// delegate.</typeparam>
		/// <typeparam name="TArg3">The type of the third argument passed to <paramref name="beginMethod" />
		/// delegate.</typeparam>
		/// <param name="beginMethod">The delegate that begins the asynchronous operation.</param>
		/// <param name="endMethod">The delegate that ends the asynchronous operation.</param>
		/// <param name="arg1">The first argument passed to the <paramref name="beginMethod" />
		/// delegate.</param>
		/// <param name="arg2">The second argument passed to the <paramref name="beginMethod" />
		/// delegate.</param>
		/// <param name="arg3">The third argument passed to the <paramref name="beginMethod" />
		/// delegate.</param>
		/// <param name="state">An object containing data to be used by the <paramref name="beginMethod" />
		/// delegate.</param>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="beginMethod" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="endMethod" /> argument is null.</exception>
		/// <returns>The created <see cref="T:System.Threading.Tasks.Task">Task</see> that represents the
		/// asynchronous operation.</returns>
		/// <remarks>
		/// This method throws any exceptions thrown by the <paramref name="beginMethod" />.
		/// </remarks>
		public Task FromAsync<TArg1, TArg2, TArg3>(Func_<TArg1, TArg2, TArg3, AsyncCallback, object, IAsyncResult> beginMethod, Action<IAsyncResult> endMethod, TArg1 arg1, TArg2 arg2, TArg3 arg3, object state)
		{
			return FromAsync(beginMethod, endMethod, arg1, arg2, arg3, state, m_defaultCreationOptions);
		}

		/// <summary>
		/// Creates a <see cref="T:System.Threading.Tasks.Task">Task</see> that represents a pair of begin
		/// and end methods that conform to the Asynchronous Programming Model pattern.
		/// </summary>
		/// <typeparam name="TArg1">The type of the first argument passed to the <paramref name="beginMethod" />
		/// delegate.</typeparam>
		/// <typeparam name="TArg2">The type of the second argument passed to <paramref name="beginMethod" />
		/// delegate.</typeparam>
		/// <typeparam name="TArg3">The type of the third argument passed to <paramref name="beginMethod" />
		/// delegate.</typeparam>
		/// <param name="beginMethod">The delegate that begins the asynchronous operation.</param>
		/// <param name="endMethod">The delegate that ends the asynchronous operation.</param>
		/// <param name="arg1">The first argument passed to the <paramref name="beginMethod" />
		/// delegate.</param>
		/// <param name="arg2">The second argument passed to the <paramref name="beginMethod" />
		/// delegate.</param>
		/// <param name="arg3">The third argument passed to the <paramref name="beginMethod" />
		/// delegate.</param>
		/// <param name="creationOptions">The TaskCreationOptions value that controls the behavior of the
		/// created <see cref="T:System.Threading.Tasks.Task">Task</see>.</param>
		/// <param name="state">An object containing data to be used by the <paramref name="beginMethod" />
		/// delegate.</param>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="beginMethod" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="endMethod" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The exception that is thrown when the
		/// <paramref name="creationOptions" /> argument specifies an invalid TaskCreationOptions
		/// value.</exception>
		/// <returns>The created <see cref="T:System.Threading.Tasks.Task">Task</see> that represents the
		/// asynchronous operation.</returns>
		/// <remarks>
		/// This method throws any exceptions thrown by the <paramref name="beginMethod" />.
		/// </remarks>
		public Task FromAsync<TArg1, TArg2, TArg3>(Func_<TArg1, TArg2, TArg3, AsyncCallback, object, IAsyncResult> beginMethod, Action<IAsyncResult> endMethod, TArg1 arg1, TArg2 arg2, TArg3 arg3, object state, TaskCreationOptions creationOptions)
		{
			if (beginMethod == null)
			{
				throw new ArgumentNullException("beginMethod");
			}
			if (endMethod == null)
			{
				throw new ArgumentNullException("endMethod");
			}
			CheckFromAsyncOptions(creationOptions, hasBeginMethod: true);
			TaskCompletionSource<object> tcs = new TaskCompletionSource<object>(state, creationOptions);
			try
			{
				beginMethod(arg1, arg2, arg3, delegate(IAsyncResult iar)
				{
					FromAsyncCoreLogic(iar, endMethod, tcs);
				}, state);
			}
			catch
			{
				tcs.TrySetResult(null);
				throw;
			}
			return tcs.Task;
		}

		/// <summary>
		/// Creates a <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see> that executes an end
		/// method function when a specified <see cref="T:System.IAsyncResult">IAsyncResult</see> completes.
		/// </summary>
		/// <typeparam name="TResult">The type of the result available through the
		/// <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>.
		/// </typeparam>
		/// <param name="asyncResult">The IAsyncResult whose completion should trigger the processing of the
		/// <paramref name="endMethod" />.</param>
		/// <param name="endMethod">The function delegate that processes the completed <paramref name="asyncResult" />.</param>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="asyncResult" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="endMethod" /> argument is null.</exception>
		/// <returns>A <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see> that represents the
		/// asynchronous operation.</returns>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task<TResult> FromAsync<TResult>(IAsyncResult asyncResult, Func<IAsyncResult, TResult> endMethod)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return TaskFactory<TResult>.FromAsyncImpl(asyncResult, endMethod, m_defaultCreationOptions, DefaultScheduler, ref stackMark);
		}

		/// <summary>
		/// Creates a <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see> that executes an end
		/// method function when a specified <see cref="T:System.IAsyncResult">IAsyncResult</see> completes.
		/// </summary>
		/// <typeparam name="TResult">The type of the result available through the
		/// <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>.
		/// </typeparam>
		/// <param name="asyncResult">The IAsyncResult whose completion should trigger the processing of the
		/// <paramref name="endMethod" />.</param>
		/// <param name="endMethod">The function delegate that processes the completed <paramref name="asyncResult" />.</param>
		/// <param name="creationOptions">The TaskCreationOptions value that controls the behavior of the
		/// created <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>.</param>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="asyncResult" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="endMethod" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The exception that is thrown when the
		/// <paramref name="creationOptions" /> argument specifies an invalid TaskCreationOptions
		/// value.</exception>
		/// <returns>A <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see> that represents the
		/// asynchronous operation.</returns>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task<TResult> FromAsync<TResult>(IAsyncResult asyncResult, Func<IAsyncResult, TResult> endMethod, TaskCreationOptions creationOptions)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return TaskFactory<TResult>.FromAsyncImpl(asyncResult, endMethod, creationOptions, DefaultScheduler, ref stackMark);
		}

		/// <summary>
		/// Creates a <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see> that executes an end
		/// method function when a specified <see cref="T:System.IAsyncResult">IAsyncResult</see> completes.
		/// </summary>
		/// <typeparam name="TResult">The type of the result available through the
		/// <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>.
		/// </typeparam>
		/// <param name="asyncResult">The IAsyncResult whose completion should trigger the processing of the
		/// <paramref name="endMethod" />.</param>
		/// <param name="endMethod">The function delegate that processes the completed <paramref name="asyncResult" />.</param>
		/// <param name="scheduler">The <see cref="T:System.Threading.Tasks.TaskScheduler">TaskScheduler</see>
		/// that is used to schedule the task that executes the end method.</param>
		/// <param name="creationOptions">The TaskCreationOptions value that controls the behavior of the
		/// created <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>.</param>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="asyncResult" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="endMethod" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="scheduler" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The exception that is thrown when the
		/// <paramref name="creationOptions" /> argument specifies an invalid TaskCreationOptions
		/// value.</exception>
		/// <returns>A <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see> that represents the
		/// asynchronous operation.</returns>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task<TResult> FromAsync<TResult>(IAsyncResult asyncResult, Func<IAsyncResult, TResult> endMethod, TaskCreationOptions creationOptions, TaskScheduler scheduler)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return TaskFactory<TResult>.FromAsyncImpl(asyncResult, endMethod, creationOptions, scheduler, ref stackMark);
		}

		/// <summary>
		/// Creates a <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see> that represents a pair of
		/// begin and end methods that conform to the Asynchronous Programming Model pattern.
		/// </summary>
		/// <typeparam name="TResult">The type of the result available through the
		/// <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>.
		/// </typeparam>
		/// <param name="beginMethod">The delegate that begins the asynchronous operation.</param>
		/// <param name="endMethod">The delegate that ends the asynchronous operation.</param>
		/// <param name="state">An object containing data to be used by the <paramref name="beginMethod" />
		/// delegate.</param>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="beginMethod" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="endMethod" /> argument is null.</exception>
		/// <returns>The created <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see> that
		/// represents the asynchronous operation.</returns>
		/// <remarks>
		/// This method throws any exceptions thrown by the <paramref name="beginMethod" />.
		/// </remarks>
		public Task<TResult> FromAsync<TResult>(Func<AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, object state)
		{
			return TaskFactory<TResult>.FromAsyncImpl(beginMethod, endMethod, state, m_defaultCreationOptions);
		}

		/// <summary>
		/// Creates a <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see> that represents a pair of
		/// begin and end methods that conform to the Asynchronous Programming Model pattern.
		/// </summary>
		/// <typeparam name="TResult">The type of the result available through the
		/// <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>.
		/// </typeparam>
		/// <param name="beginMethod">The delegate that begins the asynchronous operation.</param>
		/// <param name="endMethod">The delegate that ends the asynchronous operation.</param>
		/// <param name="creationOptions">The TaskCreationOptions value that controls the behavior of the
		/// created <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>.</param>
		/// <param name="state">An object containing data to be used by the <paramref name="beginMethod" />
		/// delegate.</param>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="beginMethod" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="endMethod" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The exception that is thrown when the
		/// <paramref name="creationOptions" /> argument specifies an invalid TaskCreationOptions
		/// value.</exception>
		/// <returns>The created <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see> that
		/// represents the asynchronous operation.</returns>
		/// <remarks>
		/// This method throws any exceptions thrown by the <paramref name="beginMethod" />.
		/// </remarks>
		public Task<TResult> FromAsync<TResult>(Func<AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, object state, TaskCreationOptions creationOptions)
		{
			return TaskFactory<TResult>.FromAsyncImpl(beginMethod, endMethod, state, creationOptions);
		}

		/// <summary>
		/// Creates a <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see> that represents a pair of
		/// begin and end methods that conform to the Asynchronous Programming Model pattern.
		/// </summary>
		/// <typeparam name="TArg1">The type of the first argument passed to the <paramref name="beginMethod" /> delegate.</typeparam>
		/// <typeparam name="TResult">The type of the result available through the
		/// <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>.
		/// </typeparam>
		/// <param name="beginMethod">The delegate that begins the asynchronous operation.</param>
		/// <param name="endMethod">The delegate that ends the asynchronous operation.</param>
		/// <param name="arg1">The first argument passed to the <paramref name="beginMethod" />
		/// delegate.</param>
		/// <param name="state">An object containing data to be used by the <paramref name="beginMethod" />
		/// delegate.</param>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="beginMethod" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="endMethod" /> argument is null.</exception>
		/// <returns>The created <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see> that
		/// represents the asynchronous operation.</returns>
		/// <remarks>
		/// This method throws any exceptions thrown by the <paramref name="beginMethod" />.
		/// </remarks>
		public Task<TResult> FromAsync<TArg1, TResult>(Func<TArg1, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, object state)
		{
			return TaskFactory<TResult>.FromAsyncImpl(beginMethod, endMethod, arg1, state, m_defaultCreationOptions);
		}

		/// <summary>
		/// Creates a <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see> that represents a pair of
		/// begin and end methods that conform to the Asynchronous Programming Model pattern.
		/// </summary>
		/// <typeparam name="TArg1">The type of the first argument passed to the <paramref name="beginMethod" /> delegate.</typeparam>
		/// <typeparam name="TResult">The type of the result available through the
		/// <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>.
		/// </typeparam>
		/// <param name="beginMethod">The delegate that begins the asynchronous operation.</param>
		/// <param name="endMethod">The delegate that ends the asynchronous operation.</param>
		/// <param name="arg1">The first argument passed to the <paramref name="beginMethod" />
		/// delegate.</param>
		/// <param name="creationOptions">The TaskCreationOptions value that controls the behavior of the
		/// created <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>.</param>
		/// <param name="state">An object containing data to be used by the <paramref name="beginMethod" />
		/// delegate.</param>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="beginMethod" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="endMethod" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The exception that is thrown when the
		/// <paramref name="creationOptions" /> argument specifies an invalid TaskCreationOptions
		/// value.</exception>
		/// <returns>The created <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see> that
		/// represents the asynchronous operation.</returns>
		/// <remarks>
		/// This method throws any exceptions thrown by the <paramref name="beginMethod" />.
		/// </remarks>
		public Task<TResult> FromAsync<TArg1, TResult>(Func<TArg1, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, object state, TaskCreationOptions creationOptions)
		{
			return TaskFactory<TResult>.FromAsyncImpl(beginMethod, endMethod, arg1, state, creationOptions);
		}

		/// <summary>
		/// Creates a <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see> that represents a pair of
		/// begin and end methods that conform to the Asynchronous Programming Model pattern.
		/// </summary>
		/// <typeparam name="TArg1">The type of the first argument passed to the <paramref name="beginMethod" /> delegate.</typeparam>
		/// <typeparam name="TArg2">The type of the second argument passed to <paramref name="beginMethod" />
		/// delegate.</typeparam>
		/// <typeparam name="TResult">The type of the result available through the
		/// <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>.
		/// </typeparam>
		/// <param name="beginMethod">The delegate that begins the asynchronous operation.</param>
		/// <param name="endMethod">The delegate that ends the asynchronous operation.</param>
		/// <param name="arg1">The first argument passed to the <paramref name="beginMethod" />
		/// delegate.</param>
		/// <param name="arg2">The second argument passed to the <paramref name="beginMethod" />
		/// delegate.</param>
		/// <param name="state">An object containing data to be used by the <paramref name="beginMethod" />
		/// delegate.</param>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="beginMethod" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="endMethod" /> argument is null.</exception>
		/// <returns>The created <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see> that
		/// represents the asynchronous operation.</returns>
		/// <remarks>
		/// This method throws any exceptions thrown by the <paramref name="beginMethod" />.
		/// </remarks>
		public Task<TResult> FromAsync<TArg1, TArg2, TResult>(Func<TArg1, TArg2, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, TArg2 arg2, object state)
		{
			return TaskFactory<TResult>.FromAsyncImpl(beginMethod, endMethod, arg1, arg2, state, m_defaultCreationOptions);
		}

		/// <summary>
		/// Creates a <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see> that represents a pair of
		/// begin and end methods that conform to the Asynchronous Programming Model pattern.
		/// </summary>
		/// <typeparam name="TArg1">The type of the first argument passed to the <paramref name="beginMethod" /> delegate.</typeparam>
		/// <typeparam name="TArg2">The type of the second argument passed to <paramref name="beginMethod" />
		/// delegate.</typeparam>
		/// <typeparam name="TResult">The type of the result available through the
		/// <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>.
		/// </typeparam>
		/// <param name="beginMethod">The delegate that begins the asynchronous operation.</param>
		/// <param name="endMethod">The delegate that ends the asynchronous operation.</param>
		/// <param name="arg1">The first argument passed to the <paramref name="beginMethod" />
		/// delegate.</param>
		/// <param name="arg2">The second argument passed to the <paramref name="beginMethod" />
		/// delegate.</param>
		/// <param name="creationOptions">The TaskCreationOptions value that controls the behavior of the
		/// created <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>.</param>
		/// <param name="state">An object containing data to be used by the <paramref name="beginMethod" />
		/// delegate.</param>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="beginMethod" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="endMethod" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The exception that is thrown when the
		/// <paramref name="creationOptions" /> argument specifies an invalid TaskCreationOptions
		/// value.</exception>
		/// <returns>The created <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see> that
		/// represents the asynchronous operation.</returns>
		/// <remarks>
		/// This method throws any exceptions thrown by the <paramref name="beginMethod" />.
		/// </remarks>
		public Task<TResult> FromAsync<TArg1, TArg2, TResult>(Func<TArg1, TArg2, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, TArg2 arg2, object state, TaskCreationOptions creationOptions)
		{
			return TaskFactory<TResult>.FromAsyncImpl(beginMethod, endMethod, arg1, arg2, state, creationOptions);
		}

		/// <summary>
		/// Creates a <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see> that represents a pair of
		/// begin and end methods that conform to the Asynchronous Programming Model pattern.
		/// </summary>
		/// <typeparam name="TArg1">The type of the first argument passed to the <paramref name="beginMethod" /> delegate.</typeparam>
		/// <typeparam name="TArg2">The type of the second argument passed to <paramref name="beginMethod" />
		/// delegate.</typeparam>
		/// <typeparam name="TArg3">The type of the third argument passed to <paramref name="beginMethod" />
		/// delegate.</typeparam>
		/// <typeparam name="TResult">The type of the result available through the
		/// <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>.
		/// </typeparam>
		/// <param name="beginMethod">The delegate that begins the asynchronous operation.</param>
		/// <param name="endMethod">The delegate that ends the asynchronous operation.</param>
		/// <param name="arg1">The first argument passed to the <paramref name="beginMethod" />
		/// delegate.</param>
		/// <param name="arg2">The second argument passed to the <paramref name="beginMethod" />
		/// delegate.</param>
		/// <param name="arg3">The third argument passed to the <paramref name="beginMethod" />
		/// delegate.</param>
		/// <param name="state">An object containing data to be used by the <paramref name="beginMethod" />
		/// delegate.</param>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="beginMethod" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="endMethod" /> argument is null.</exception>
		/// <returns>The created <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see> that
		/// represents the asynchronous operation.</returns>
		/// <remarks>
		/// This method throws any exceptions thrown by the <paramref name="beginMethod" />.
		/// </remarks>
		public Task<TResult> FromAsync<TArg1, TArg2, TArg3, TResult>(Func_<TArg1, TArg2, TArg3, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, TArg2 arg2, TArg3 arg3, object state)
		{
			return TaskFactory<TResult>.FromAsyncImpl(beginMethod, endMethod, arg1, arg2, arg3, state, m_defaultCreationOptions);
		}

		/// <summary>
		/// Creates a <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see> that represents a pair of
		/// begin and end methods that conform to the Asynchronous Programming Model pattern.
		/// </summary>
		/// <typeparam name="TArg1">The type of the first argument passed to the <paramref name="beginMethod" /> delegate.</typeparam>
		/// <typeparam name="TArg2">The type of the second argument passed to <paramref name="beginMethod" />
		/// delegate.</typeparam>
		/// <typeparam name="TArg3">The type of the third argument passed to <paramref name="beginMethod" />
		/// delegate.</typeparam>
		/// <typeparam name="TResult">The type of the result available through the
		/// <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>.
		/// </typeparam>
		/// <param name="beginMethod">The delegate that begins the asynchronous operation.</param>
		/// <param name="endMethod">The delegate that ends the asynchronous operation.</param>
		/// <param name="arg1">The first argument passed to the <paramref name="beginMethod" />
		/// delegate.</param>
		/// <param name="arg2">The second argument passed to the <paramref name="beginMethod" />
		/// delegate.</param>
		/// <param name="arg3">The third argument passed to the <paramref name="beginMethod" />
		/// delegate.</param>
		/// <param name="creationOptions">The TaskCreationOptions value that controls the behavior of the
		/// created <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>.</param>
		/// <param name="state">An object containing data to be used by the <paramref name="beginMethod" />
		/// delegate.</param>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="beginMethod" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="endMethod" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The exception that is thrown when the
		/// <paramref name="creationOptions" /> argument specifies an invalid TaskCreationOptions
		/// value.</exception>
		/// <returns>The created <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see> that
		/// represents the asynchronous operation.</returns>
		/// <remarks>
		/// This method throws any exceptions thrown by the <paramref name="beginMethod" />.
		/// </remarks>
		public Task<TResult> FromAsync<TArg1, TArg2, TArg3, TResult>(Func_<TArg1, TArg2, TArg3, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, TArg2 arg2, TArg3 arg3, object state, TaskCreationOptions creationOptions)
		{
			return TaskFactory<TResult>.FromAsyncImpl(beginMethod, endMethod, arg1, arg2, arg3, state, creationOptions);
		}

		/// <summary>
		/// Check validity of options passed to FromAsync method
		/// </summary>
		/// <param name="creationOptions">The options to be validated.</param>
		/// <param name="hasBeginMethod">determines type of FromAsync method that called this method</param>
		internal static void CheckFromAsyncOptions(TaskCreationOptions creationOptions, bool hasBeginMethod)
		{
			if (hasBeginMethod)
			{
				if ((creationOptions & TaskCreationOptions.LongRunning) != 0)
				{
					throw new ArgumentOutOfRangeException("creationOptions", Environment2.GetResourceString("Task_FromAsync_LongRunning"));
				}
				if ((creationOptions & TaskCreationOptions.PreferFairness) != 0)
				{
					throw new ArgumentOutOfRangeException("creationOptions", Environment2.GetResourceString("Task_FromAsync_PreferFairness"));
				}
			}
			if (((uint)creationOptions & 0xFFFFFFF8u) != 0)
			{
				throw new ArgumentOutOfRangeException("creationOptions");
			}
		}

		internal static Task<bool> CommonCWAllLogic(Task[] tasksCopy)
		{
			TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
			int tasksLeft = tasksCopy.Length;
			Action<Task> action = delegate
			{
				if (Interlocked.Decrement(ref tasksLeft) == 0)
				{
					tcs.TrySetResult(result: true);
				}
			};
			for (int i = 0; i < tasksCopy.Length; i++)
			{
				if (tasksCopy[i].IsCompleted)
				{
					action(tasksCopy[i]);
				}
				else
				{
					tasksCopy[i].AddCompletionAction(action);
				}
			}
			return tcs.Task;
		}

		/// <summary>
		/// Creates a continuation <see cref="T:System.Threading.Tasks.Task">Task</see> 
		/// that will be started upon the completion of a set of provided Tasks.
		/// </summary>
		/// <param name="tasks">The array of tasks from which to continue.</param>
		/// <param name="continuationAction">The action delegate to execute when all tasks in 
		/// the <paramref name="tasks" /> array have completed.</param>
		/// <returns>The new continuation <see cref="T:System.Threading.Tasks.Task">Task</see>.</returns>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the 
		/// <paramref name="tasks" /> array is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the 
		/// <paramref name="continuationAction" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the 
		/// <paramref name="tasks" /> array contains a null value.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the 
		/// <paramref name="tasks" /> array is empty.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The exception that is thrown when one 
		/// of the elements in the <paramref name="tasks" /> array has been disposed.</exception>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task ContinueWhenAll(Task[] tasks, Action<Task[]> continuationAction)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return ContinueWhenAll(tasks, continuationAction, m_defaultContinuationOptions, m_defaultCancellationToken, DefaultScheduler, ref stackMark);
		}

		/// <summary>
		/// Creates a continuation <see cref="T:System.Threading.Tasks.Task">Task</see> 
		/// that will be started upon the completion of a set of provided Tasks.
		/// </summary>
		/// <param name="tasks">The array of tasks from which to continue.</param>
		/// <param name="continuationAction">The action delegate to execute when all tasks in 
		/// the <paramref name="tasks" /> array have completed.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken">CancellationToken</see> 
		/// that will be assigned to the new continuation task.</param>
		/// <returns>The new continuation <see cref="T:System.Threading.Tasks.Task">Task</see>.</returns>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the 
		/// <paramref name="tasks" /> array is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the 
		/// <paramref name="continuationAction" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the 
		/// <paramref name="tasks" /> array contains a null value.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the 
		/// <paramref name="tasks" /> array is empty.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The exception that is thrown when one 
		/// of the elements in the <paramref name="tasks" /> array has been disposed.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken">CancellationToken</see>
		/// has already been disposed.
		/// </exception>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task ContinueWhenAll(Task[] tasks, Action<Task[]> continuationAction, CancellationToken cancellationToken)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return ContinueWhenAll(tasks, continuationAction, m_defaultContinuationOptions, cancellationToken, DefaultScheduler, ref stackMark);
		}

		/// <summary>
		/// Creates a continuation <see cref="T:System.Threading.Tasks.Task">Task</see>
		/// that will be started upon the completion of a set of provided Tasks.
		/// </summary>
		/// <param name="tasks">The array of tasks from which to continue.</param>
		/// <param name="continuationAction">The action delegate to execute when all tasks in the <paramref name="tasks" /> array have completed.</param>
		/// <param name="continuationOptions">The <see cref="T:System.Threading.Tasks.TaskContinuationOptions">
		/// TaskContinuationOptions</see> value that controls the behavior of
		/// the created continuation <see cref="T:System.Threading.Tasks.Task">Task</see>.</param>
		/// <returns>The new continuation <see cref="T:System.Threading.Tasks.Task">Task</see>.</returns>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="continuationAction" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array contains a null value.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is empty.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The exception that is thrown when the
		/// <paramref name="continuationOptions" /> argument specifies an invalid TaskContinuationOptions
		/// value.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The exception that is thrown when one 
		/// of the elements in the <paramref name="tasks" /> array has been disposed.</exception>
		/// <remarks>
		/// The NotOn* and OnlyOn* <see cref="T:System.Threading.Tasks.TaskContinuationOptions">TaskContinuationOptions</see>, 
		/// which constrain for which <see cref="T:System.Threading.Tasks.TaskStatus">TaskStatus</see> states a continuation 
		/// will be executed, are illegal with ContinueWhenAll.
		/// </remarks>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task ContinueWhenAll(Task[] tasks, Action<Task[]> continuationAction, TaskContinuationOptions continuationOptions)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return ContinueWhenAll(tasks, continuationAction, continuationOptions, m_defaultCancellationToken, DefaultScheduler, ref stackMark);
		}

		/// <summary>
		/// Creates a continuation <see cref="T:System.Threading.Tasks.Task">Task</see>
		/// that will be started upon the completion of a set of provided Tasks.
		/// </summary>
		/// <param name="tasks">The array of tasks from which to continue.</param>
		/// <param name="continuationAction">The action delegate to execute when all tasks in the <paramref name="tasks" /> array have completed.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken">CancellationToken</see> 
		/// that will be assigned to the new continuation task.</param>
		/// <param name="continuationOptions">The <see cref="T:System.Threading.Tasks.TaskContinuationOptions">
		/// TaskContinuationOptions</see> value that controls the behavior of
		/// the created continuation <see cref="T:System.Threading.Tasks.Task">Task</see>.</param>
		/// <param name="scheduler">The <see cref="T:System.Threading.Tasks.TaskScheduler">TaskScheduler</see>
		/// that is used to schedule the created continuation <see cref="T:System.Threading.Tasks.Task">Task</see>.</param>
		/// <returns>The new continuation <see cref="T:System.Threading.Tasks.Task">Task</see>.</returns>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="continuationAction" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="scheduler" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array contains a null value.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is empty.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The exception that is thrown when the
		/// <paramref name="continuationOptions" /> argument specifies an invalid TaskContinuationOptions
		/// value.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The exception that is thrown when one 
		/// of the elements in the <paramref name="tasks" /> array has been disposed.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken">CancellationToken</see>
		/// has already been disposed.
		/// </exception>
		/// <remarks>
		/// The NotOn* and OnlyOn* <see cref="T:System.Threading.Tasks.TaskContinuationOptions">TaskContinuationOptions</see>, 
		/// which constrain for which <see cref="T:System.Threading.Tasks.TaskStatus">TaskStatus</see> states a continuation 
		/// will be executed, are illegal with ContinueWhenAll.
		/// </remarks>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task ContinueWhenAll(Task[] tasks, Action<Task[]> continuationAction, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return ContinueWhenAll(tasks, continuationAction, continuationOptions, cancellationToken, scheduler, ref stackMark);
		}

		private static Task ContinueWhenAll(Task[] tasks, Action<Task[]> continuationAction, TaskContinuationOptions continuationOptions, CancellationToken cancellationToken, TaskScheduler scheduler, ref StackCrawlMark2 stackMark)
		{
			CheckMultiTaskContinuationOptions(continuationOptions);
			if (tasks == null)
			{
				throw new ArgumentNullException("tasks");
			}
			if (continuationAction == null)
			{
				throw new ArgumentNullException("continuationAction");
			}
			if (scheduler == null)
			{
				throw new ArgumentNullException("scheduler");
			}
			Task[] tasksCopy = CheckMultiContinuationTasksAndCopy(tasks);
			if (cancellationToken.IsCancellationRequested)
			{
				return CreateCanceledTask(continuationOptions);
			}
			Task<bool> task = CommonCWAllLogic(tasksCopy);
			return task.ContinueWith(delegate
			{
				continuationAction(tasksCopy);
			}, scheduler, cancellationToken, continuationOptions, ref stackMark);
		}

		/// <summary>
		/// Creates a continuation <see cref="T:System.Threading.Tasks.Task">Task</see> 
		/// that will be started upon the completion of a set of provided Tasks.
		/// </summary>
		/// <typeparam name="TAntecedentResult">The type of the result of the antecedent <paramref name="tasks" />.</typeparam>
		/// <param name="tasks">The array of tasks from which to continue.</param>
		/// <param name="continuationAction">The action delegate to execute when all tasks in 
		/// the <paramref name="tasks" /> array have completed.</param>
		/// <returns>The new continuation <see cref="T:System.Threading.Tasks.Task">Task</see>.</returns>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the 
		/// <paramref name="tasks" /> array is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the 
		/// <paramref name="continuationAction" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the 
		/// <paramref name="tasks" /> array contains a null value.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the 
		/// <paramref name="tasks" /> array is empty.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The exception that is thrown when one 
		/// of the elements in the <paramref name="tasks" /> array has been disposed.</exception>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task ContinueWhenAll<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Action<Task<TAntecedentResult>[]> continuationAction)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return ContinueWhenAll(tasks, continuationAction, m_defaultContinuationOptions, m_defaultCancellationToken, DefaultScheduler, ref stackMark);
		}

		/// <summary>
		/// Creates a continuation <see cref="T:System.Threading.Tasks.Task">Task</see> 
		/// that will be started upon the completion of a set of provided Tasks.
		/// </summary>
		/// <typeparam name="TAntecedentResult">The type of the result of the antecedent <paramref name="tasks" />.</typeparam>
		/// <param name="tasks">The array of tasks from which to continue.</param>
		/// <param name="continuationAction">The action delegate to execute when all tasks in 
		/// the <paramref name="tasks" /> array have completed.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken">CancellationToken</see> 
		/// that will be assigned to the new continuation task.</param>
		/// <returns>The new continuation <see cref="T:System.Threading.Tasks.Task">Task</see>.</returns>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the 
		/// <paramref name="tasks" /> array is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the 
		/// <paramref name="continuationAction" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the 
		/// <paramref name="tasks" /> array contains a null value.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the 
		/// <paramref name="tasks" /> array is empty.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The exception that is thrown when one 
		/// of the elements in the <paramref name="tasks" /> array has been disposed.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken">CancellationToken</see>
		/// has already been disposed.
		/// </exception>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task ContinueWhenAll<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Action<Task<TAntecedentResult>[]> continuationAction, CancellationToken cancellationToken)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return ContinueWhenAll(tasks, continuationAction, m_defaultContinuationOptions, cancellationToken, DefaultScheduler, ref stackMark);
		}

		/// <summary>
		/// Creates a continuation <see cref="T:System.Threading.Tasks.Task">Task</see>
		/// that will be started upon the completion of a set of provided Tasks.
		/// </summary>
		/// <typeparam name="TAntecedentResult">The type of the result of the antecedent <paramref name="tasks" />.</typeparam>
		/// <param name="tasks">The array of tasks from which to continue.</param>
		/// <param name="continuationAction">The action delegate to execute when all tasks in the <paramref name="tasks" /> array have completed.</param>
		/// <param name="continuationOptions">The <see cref="T:System.Threading.Tasks.TaskContinuationOptions">
		/// TaskContinuationOptions</see> value that controls the behavior of
		/// the created continuation <see cref="T:System.Threading.Tasks.Task">Task</see>.</param>
		/// <returns>The new continuation <see cref="T:System.Threading.Tasks.Task">Task</see>.</returns>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="continuationAction" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array contains a null value.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is empty.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The exception that is thrown when the
		/// <paramref name="continuationOptions" /> argument specifies an invalid TaskContinuationOptions
		/// value.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The exception that is thrown when one 
		/// of the elements in the <paramref name="tasks" /> array has been disposed.</exception>
		/// <remarks>
		/// The NotOn* and OnlyOn* <see cref="T:System.Threading.Tasks.TaskContinuationOptions">TaskContinuationOptions</see>, 
		/// which constrain for which <see cref="T:System.Threading.Tasks.TaskStatus">TaskStatus</see> states a continuation 
		/// will be executed, are illegal with ContinueWhenAll.
		/// </remarks>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task ContinueWhenAll<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Action<Task<TAntecedentResult>[]> continuationAction, TaskContinuationOptions continuationOptions)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return ContinueWhenAll(tasks, continuationAction, continuationOptions, m_defaultCancellationToken, DefaultScheduler, ref stackMark);
		}

		/// <summary>
		/// Creates a continuation <see cref="T:System.Threading.Tasks.Task">Task</see>
		/// that will be started upon the completion of a set of provided Tasks.
		/// </summary>
		/// <typeparam name="TAntecedentResult">The type of the result of the antecedent <paramref name="tasks" />.</typeparam>
		/// <param name="tasks">The array of tasks from which to continue.</param>
		/// <param name="continuationAction">The action delegate to execute when all tasks in the <paramref name="tasks" /> array have completed.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken">CancellationToken</see> 
		/// that will be assigned to the new continuation task.</param>
		/// <param name="continuationOptions">The <see cref="T:System.Threading.Tasks.TaskContinuationOptions">
		/// TaskContinuationOptions</see> value that controls the behavior of
		/// the created continuation <see cref="T:System.Threading.Tasks.Task">Task</see>.</param>
		/// <param name="scheduler">The <see cref="T:System.Threading.Tasks.TaskScheduler">TaskScheduler</see>
		/// that is used to schedule the created continuation <see cref="T:System.Threading.Tasks.Task">Task</see>.</param>
		/// <returns>The new continuation <see cref="T:System.Threading.Tasks.Task">Task</see>.</returns>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="continuationAction" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="scheduler" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array contains a null value.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is empty.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The exception that is thrown when the
		/// <paramref name="continuationOptions" /> argument specifies an invalid TaskContinuationOptions
		/// value.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The exception that is thrown when one 
		/// of the elements in the <paramref name="tasks" /> array has been disposed.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken">CancellationToken</see>
		/// has already been disposed.
		/// </exception>
		/// <remarks>
		/// The NotOn* and OnlyOn* <see cref="T:System.Threading.Tasks.TaskContinuationOptions">TaskContinuationOptions</see>, 
		/// which constrain for which <see cref="T:System.Threading.Tasks.TaskStatus">TaskStatus</see> states a continuation 
		/// will be executed, are illegal with ContinueWhenAll.
		/// </remarks>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task ContinueWhenAll<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Action<Task<TAntecedentResult>[]> continuationAction, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return ContinueWhenAll(tasks, continuationAction, continuationOptions, cancellationToken, scheduler, ref stackMark);
		}

		private static Task ContinueWhenAll<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Action<Task<TAntecedentResult>[]> continuationAction, TaskContinuationOptions continuationOptions, CancellationToken cancellationToken, TaskScheduler scheduler, ref StackCrawlMark2 stackMark)
		{
			CheckMultiTaskContinuationOptions(continuationOptions);
			if (tasks == null)
			{
				throw new ArgumentNullException("tasks");
			}
			if (continuationAction == null)
			{
				throw new ArgumentNullException("continuationAction");
			}
			if (scheduler == null)
			{
				throw new ArgumentNullException("scheduler");
			}
			Task<TAntecedentResult>[] tasksCopy = CheckMultiContinuationTasksAndCopy(tasks);
			if (cancellationToken.IsCancellationRequested)
			{
				return CreateCanceledTask(continuationOptions);
			}
			Task<bool> task = CommonCWAllLogic(tasksCopy);
			return task.ContinueWith(delegate
			{
				continuationAction(tasksCopy);
			}, scheduler, cancellationToken, continuationOptions, ref stackMark);
		}

		/// <summary>
		/// Creates a continuation <see cref="T:System.Threading.Tasks.Task">Task</see>
		/// that will be started upon the completion of a set of provided Tasks.
		/// </summary>
		/// <typeparam name="TResult">The type of the result that is returned by the <paramref name="continuationFunction" />
		/// delegate and associated with the created <see cref="T:System.Threading.Tasks.Task{TResult}" />.</typeparam>
		/// <param name="tasks">The array of tasks from which to continue.</param>
		/// <param name="continuationFunction">The function delegate to execute when all tasks in the
		/// <paramref name="tasks" /> array have completed.</param>
		/// <returns>The new continuation <see cref="T:System.Threading.Tasks.Task{TResult}" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="continuationFunction" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array contains a null value.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is empty.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The exception that is thrown when one 
		/// of the elements in the <paramref name="tasks" /> array has been disposed.</exception>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task<TResult> ContinueWhenAll<TResult>(Task[] tasks, Func<Task[], TResult> continuationFunction)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return ContinueWhenAll(tasks, continuationFunction, m_defaultContinuationOptions, m_defaultCancellationToken, DefaultScheduler, ref stackMark);
		}

		/// <summary>
		/// Creates a continuation <see cref="T:System.Threading.Tasks.Task">Task</see>
		/// that will be started upon the completion of a set of provided Tasks.
		/// </summary>
		/// <typeparam name="TResult">The type of the result that is returned by the <paramref name="continuationFunction" />
		/// delegate and associated with the created <see cref="T:System.Threading.Tasks.Task{TResult}" />.</typeparam>
		/// <param name="tasks">The array of tasks from which to continue.</param>
		/// <param name="continuationFunction">The function delegate to execute when all tasks in the
		/// <paramref name="tasks" /> array have completed.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken">CancellationToken</see> 
		/// that will be assigned to the new continuation task.</param>
		/// <returns>The new continuation <see cref="T:System.Threading.Tasks.Task{TResult}" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="continuationFunction" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array contains a null value.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is empty.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The exception that is thrown when one 
		/// of the elements in the <paramref name="tasks" /> array has been disposed.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken">CancellationToken</see>
		/// has already been disposed.
		/// </exception>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task<TResult> ContinueWhenAll<TResult>(Task[] tasks, Func<Task[], TResult> continuationFunction, CancellationToken cancellationToken)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return ContinueWhenAll(tasks, continuationFunction, m_defaultContinuationOptions, cancellationToken, DefaultScheduler, ref stackMark);
		}

		/// <summary>
		/// Creates a continuation <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>
		/// that will be started upon the completion of a set of provided Tasks.
		/// </summary>
		/// <typeparam name="TResult">The type of the result that is returned by the <paramref name="continuationFunction" />
		/// delegate and associated with the created <see cref="T:System.Threading.Tasks.Task{TResult}" />.</typeparam>
		/// <param name="tasks">The array of tasks from which to continue.</param>
		/// <param name="continuationFunction">The function delegate to execute when all tasks in the
		/// <paramref name="tasks" /> array have completed.</param>
		/// <param name="continuationOptions">The <see cref="T:System.Threading.Tasks.TaskContinuationOptions">
		/// TaskContinuationOptions</see> value that controls the behavior of
		/// the created continuation <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>.</param>
		/// <returns>The new continuation <see cref="T:System.Threading.Tasks.Task{TResult}" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="continuationFunction" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array contains a null value.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is empty.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The exception that is thrown when the
		/// <paramref name="continuationOptions" /> argument specifies an invalid TaskContinuationOptions
		/// value.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The exception that is thrown when one 
		/// of the elements in the <paramref name="tasks" /> array has been disposed.</exception>
		/// <remarks>
		/// The NotOn* and OnlyOn* <see cref="T:System.Threading.Tasks.TaskContinuationOptions">TaskContinuationOptions</see>, 
		/// which constrain for which <see cref="T:System.Threading.Tasks.TaskStatus">TaskStatus</see> states a continuation 
		/// will be executed, are illegal with ContinueWhenAll.
		/// </remarks>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task<TResult> ContinueWhenAll<TResult>(Task[] tasks, Func<Task[], TResult> continuationFunction, TaskContinuationOptions continuationOptions)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return ContinueWhenAll(tasks, continuationFunction, continuationOptions, m_defaultCancellationToken, DefaultScheduler, ref stackMark);
		}

		/// <summary>
		/// Creates a continuation <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>
		/// that will be started upon the completion of a set of provided Tasks.
		/// </summary>
		/// <typeparam name="TResult">The type of the result that is returned by the <paramref name="continuationFunction" />
		/// delegate and associated with the created <see cref="T:System.Threading.Tasks.Task{TResult}" />.</typeparam>
		/// <param name="tasks">The array of tasks from which to continue.</param>
		/// <param name="continuationFunction">The function delegate to execute when all tasks in the
		/// <paramref name="tasks" /> array have completed.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken">CancellationToken</see> 
		/// that will be assigned to the new continuation task.</param>
		/// <param name="continuationOptions">The <see cref="T:System.Threading.Tasks.TaskContinuationOptions">
		/// TaskContinuationOptions</see> value that controls the behavior of
		/// the created continuation <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>.</param>
		/// <param name="scheduler">The <see cref="T:System.Threading.Tasks.TaskScheduler">TaskScheduler</see>
		/// that is used to schedule the created continuation <see cref="T:System.Threading.Tasks.Task{TResult}" />.</param>
		/// <returns>The new continuation <see cref="T:System.Threading.Tasks.Task{TResult}" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="continuationFunction" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="scheduler" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array contains a null value.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is empty.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The exception that is thrown when the
		/// <paramref name="continuationOptions" /> argument specifies an invalid TaskContinuationOptions
		/// value.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The exception that is thrown when one 
		/// of the elements in the <paramref name="tasks" /> array has been disposed.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken">CancellationToken</see>
		/// has already been disposed.
		/// </exception>
		/// <remarks>
		/// The NotOn* and OnlyOn* <see cref="T:System.Threading.Tasks.TaskContinuationOptions">TaskContinuationOptions</see>, 
		/// which constrain for which <see cref="T:System.Threading.Tasks.TaskStatus">TaskStatus</see> states a continuation 
		/// will be executed, are illegal with ContinueWhenAll.
		/// </remarks>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task<TResult> ContinueWhenAll<TResult>(Task[] tasks, Func<Task[], TResult> continuationFunction, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return ContinueWhenAll(tasks, continuationFunction, continuationOptions, cancellationToken, scheduler, ref stackMark);
		}

		private Task<TResult> ContinueWhenAll<TResult>(Task[] tasks, Func<Task[], TResult> continuationFunction, TaskContinuationOptions continuationOptions, CancellationToken cancellationToken, TaskScheduler scheduler, ref StackCrawlMark2 stackMark)
		{
			return TaskFactory<TResult>.ContinueWhenAll(tasks, continuationFunction, continuationOptions, cancellationToken, scheduler, ref stackMark);
		}

		/// <summary>
		/// Creates a continuation <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>
		/// that will be started upon the completion of a set of provided Tasks.
		/// </summary>
		/// <typeparam name="TResult">The type of the result that is returned by the <paramref name="continuationFunction" />
		/// delegate and associated with the created <see cref="T:System.Threading.Tasks.Task{TResult}" />.</typeparam>
		/// <typeparam name="TAntecedentResult">The type of the result of the antecedent <paramref name="tasks" />.</typeparam>
		/// <param name="tasks">The array of tasks from which to continue.</param>
		/// <param name="continuationFunction">The function delegate to execute when all tasks in the
		/// <paramref name="tasks" /> array have completed.</param>
		/// <returns>The new continuation <see cref="T:System.Threading.Tasks.Task{TResult}" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="continuationFunction" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array contains a null value.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is empty.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The exception that is thrown when one 
		/// of the elements in the <paramref name="tasks" /> array has been disposed.</exception>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task<TResult> ContinueWhenAll<TAntecedentResult, TResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>[], TResult> continuationFunction)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return ContinueWhenAll(tasks, continuationFunction, m_defaultContinuationOptions, m_defaultCancellationToken, DefaultScheduler, ref stackMark);
		}

		/// <summary>
		/// Creates a continuation <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>
		/// that will be started upon the completion of a set of provided Tasks.
		/// </summary>
		/// <typeparam name="TResult">The type of the result that is returned by the <paramref name="continuationFunction" />
		/// delegate and associated with the created <see cref="T:System.Threading.Tasks.Task{TResult}" />.</typeparam>
		/// <typeparam name="TAntecedentResult">The type of the result of the antecedent <paramref name="tasks" />.</typeparam>
		/// <param name="tasks">The array of tasks from which to continue.</param>
		/// <param name="continuationFunction">The function delegate to execute when all tasks in the
		/// <paramref name="tasks" /> array have completed.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken">CancellationToken</see> 
		/// that will be assigned to the new continuation task.</param>
		/// <returns>The new continuation <see cref="T:System.Threading.Tasks.Task{TResult}" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="continuationFunction" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array contains a null value.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is empty.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The exception that is thrown when one 
		/// of the elements in the <paramref name="tasks" /> array has been disposed.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken">CancellationToken</see>
		/// has already been disposed.
		/// </exception>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task<TResult> ContinueWhenAll<TAntecedentResult, TResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>[], TResult> continuationFunction, CancellationToken cancellationToken)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return ContinueWhenAll(tasks, continuationFunction, m_defaultContinuationOptions, cancellationToken, DefaultScheduler, ref stackMark);
		}

		/// <summary>
		/// Creates a continuation <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>
		/// that will be started upon the completion of a set of provided Tasks.
		/// </summary>
		/// <typeparam name="TResult">The type of the result that is returned by the <paramref name="continuationFunction" />
		/// delegate and associated with the created <see cref="T:System.Threading.Tasks.Task{TResult}" />.</typeparam>
		/// <typeparam name="TAntecedentResult">The type of the result of the antecedent <paramref name="tasks" />.</typeparam>
		/// <param name="tasks">The array of tasks from which to continue.</param>
		/// <param name="continuationFunction">The function delegate to execute when all tasks in the
		/// <paramref name="tasks" /> array have completed.</param>
		/// <param name="continuationOptions">The <see cref="T:System.Threading.Tasks.TaskContinuationOptions">
		/// TaskContinuationOptions</see> value that controls the behavior of
		/// the created continuation <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>.</param>
		/// <returns>The new continuation <see cref="T:System.Threading.Tasks.Task{TResult}" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="continuationFunction" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array contains a null value.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is empty.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The exception that is thrown when the
		/// <paramref name="continuationOptions" /> argument specifies an invalid TaskContinuationOptions
		/// value.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The exception that is thrown when one 
		/// of the elements in the <paramref name="tasks" /> array has been disposed.</exception>
		/// <remarks>
		/// The NotOn* and OnlyOn* <see cref="T:System.Threading.Tasks.TaskContinuationOptions">TaskContinuationOptions</see>, 
		/// which constrain for which <see cref="T:System.Threading.Tasks.TaskStatus">TaskStatus</see> states a continuation 
		/// will be executed, are illegal with ContinueWhenAll.
		/// </remarks>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task<TResult> ContinueWhenAll<TAntecedentResult, TResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>[], TResult> continuationFunction, TaskContinuationOptions continuationOptions)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return ContinueWhenAll(tasks, continuationFunction, continuationOptions, m_defaultCancellationToken, DefaultScheduler, ref stackMark);
		}

		/// <summary>
		/// Creates a continuation <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>
		/// that will be started upon the completion of a set of provided Tasks.
		/// </summary>
		/// <typeparam name="TResult">The type of the result that is returned by the <paramref name="continuationFunction" />
		/// delegate and associated with the created <see cref="T:System.Threading.Tasks.Task{TResult}" />.</typeparam>
		/// <typeparam name="TAntecedentResult">The type of the result of the antecedent <paramref name="tasks" />.</typeparam>
		/// <param name="tasks">The array of tasks from which to continue.</param>
		/// <param name="continuationFunction">The function delegate to execute when all tasks in the
		/// <paramref name="tasks" /> array have completed.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken">CancellationToken</see> 
		/// that will be assigned to the new continuation task.</param>
		/// <param name="continuationOptions">The <see cref="T:System.Threading.Tasks.TaskContinuationOptions">
		/// TaskContinuationOptions</see> value that controls the behavior of
		/// the created continuation <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>.</param>
		/// <param name="scheduler">The <see cref="T:System.Threading.Tasks.TaskScheduler">TaskScheduler</see>
		/// that is used to schedule the created continuation <see cref="T:System.Threading.Tasks.Task{TResult}" />.</param>
		/// <returns>The new continuation <see cref="T:System.Threading.Tasks.Task{TResult}" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="continuationFunction" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="scheduler" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array contains a null value.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is empty.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The exception that is thrown when the
		/// <paramref name="continuationOptions" /> argument specifies an invalid TaskContinuationOptions
		/// value.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The exception that is thrown when one 
		/// of the elements in the <paramref name="tasks" /> array has been disposed.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken">CancellationToken</see>
		/// has already been disposed.
		/// </exception>
		/// <remarks>
		/// The NotOn* and OnlyOn* <see cref="T:System.Threading.Tasks.TaskContinuationOptions">TaskContinuationOptions</see>, 
		/// which constrain for which <see cref="T:System.Threading.Tasks.TaskStatus">TaskStatus</see> states a continuation 
		/// will be executed, are illegal with ContinueWhenAll.
		/// </remarks>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task<TResult> ContinueWhenAll<TAntecedentResult, TResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>[], TResult> continuationFunction, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return ContinueWhenAll(tasks, continuationFunction, continuationOptions, cancellationToken, scheduler, ref stackMark);
		}

		private Task<TResult> ContinueWhenAll<TAntecedentResult, TResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>[], TResult> continuationFunction, TaskContinuationOptions continuationOptions, CancellationToken cancellationToken, TaskScheduler scheduler, ref StackCrawlMark2 stackMark)
		{
			return TaskFactory<TResult>.ContinueWhenAll(tasks, continuationFunction, continuationOptions, cancellationToken, scheduler, ref stackMark);
		}

		private static Task CreateCanceledTask(TaskContinuationOptions continuationOptions)
		{
			Task.CreationOptionsFromContinuationOptions(continuationOptions, out var creationOptions, out var _);
			return new Task(canceled: true, creationOptions);
		}

		internal static Task<Task> CommonCWAnyLogic(Task[] tasksCopy)
		{
			TaskCompletionSource<Task> tcs = new TaskCompletionSource<Task>();
			Action<Task> action = delegate(Task t)
			{
				tcs.TrySetResult(t);
			};
			for (int i = 0; i < tasksCopy.Length; i++)
			{
				if (tcs.Task.IsCompleted)
				{
					break;
				}
				if (tasksCopy[i].IsCompleted)
				{
					action(tasksCopy[i]);
					break;
				}
				tasksCopy[i].AddCompletionAction(action);
			}
			return tcs.Task;
		}

		/// <summary>
		/// Creates a continuation <see cref="T:System.Threading.Tasks.Task">Task</see>
		/// that will be started upon the completion of any Task in the provided set.
		/// </summary>
		/// <param name="tasks">The array of tasks from which to continue when one task completes.</param>
		/// <param name="continuationAction">The action delegate to execute when one task in the <paramref name="tasks" /> array completes.</param>
		/// <returns>The new continuation <see cref="T:System.Threading.Tasks.Task">Task</see>.</returns>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="continuationAction" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array contains a null value.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is empty.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The exception that is thrown when one 
		/// of the elements in the <paramref name="tasks" /> array has been disposed.</exception>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task ContinueWhenAny(Task[] tasks, Action<Task> continuationAction)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return ContinueWhenAny(tasks, continuationAction, m_defaultContinuationOptions, m_defaultCancellationToken, DefaultScheduler, ref stackMark);
		}

		/// <summary>
		/// Creates a continuation <see cref="T:System.Threading.Tasks.Task">Task</see>
		/// that will be started upon the completion of any Task in the provided set.
		/// </summary>
		/// <param name="tasks">The array of tasks from which to continue when one task completes.</param>
		/// <param name="continuationAction">The action delegate to execute when one task in the <paramref name="tasks" /> array completes.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken">CancellationToken</see> 
		/// that will be assigned to the new continuation task.</param>
		/// <returns>The new continuation <see cref="T:System.Threading.Tasks.Task">Task</see>.</returns>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="continuationAction" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array contains a null value.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is empty.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The exception that is thrown when one 
		/// of the elements in the <paramref name="tasks" /> array has been disposed.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken">CancellationToken</see>
		/// has already been disposed.
		/// </exception>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task ContinueWhenAny(Task[] tasks, Action<Task> continuationAction, CancellationToken cancellationToken)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return ContinueWhenAny(tasks, continuationAction, m_defaultContinuationOptions, cancellationToken, DefaultScheduler, ref stackMark);
		}

		/// <summary>
		/// Creates a continuation <see cref="T:System.Threading.Tasks.Task">Task</see>
		/// that will be started upon the completion of any Task in the provided set.
		/// </summary>
		/// <param name="tasks">The array of tasks from which to continue when one task completes.</param>
		/// <param name="continuationAction">The action delegate to execute when one task in the <paramref name="tasks" /> array completes.</param>
		/// <param name="continuationOptions">The <see cref="T:System.Threading.Tasks.TaskContinuationOptions">
		/// TaskContinuationOptions</see> value that controls the behavior of
		/// the created continuation <see cref="T:System.Threading.Tasks.Task">Task</see>.</param>
		/// <returns>The new continuation <see cref="T:System.Threading.Tasks.Task">Task</see>.</returns>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="continuationAction" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array contains a null value.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is empty.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The exception that is thrown when the
		/// <paramref name="continuationOptions" /> argument specifies an invalid TaskContinuationOptions
		/// value.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The exception that is thrown when one 
		/// of the elements in the <paramref name="tasks" /> array has been disposed.</exception>
		/// <remarks>
		/// The NotOn* and OnlyOn* <see cref="T:System.Threading.Tasks.TaskContinuationOptions">TaskContinuationOptions</see>, 
		/// which constrain for which <see cref="T:System.Threading.Tasks.TaskStatus">TaskStatus</see> states a continuation 
		/// will be executed, are illegal with ContinueWhenAny.
		/// </remarks>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task ContinueWhenAny(Task[] tasks, Action<Task> continuationAction, TaskContinuationOptions continuationOptions)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return ContinueWhenAny(tasks, continuationAction, continuationOptions, m_defaultCancellationToken, DefaultScheduler, ref stackMark);
		}

		/// <summary>
		/// Creates a continuation <see cref="T:System.Threading.Tasks.Task">Task</see>
		/// that will be started upon the completion of any Task in the provided set.
		/// </summary>
		/// <param name="tasks">The array of tasks from which to continue when one task completes.</param>
		/// <param name="continuationAction">The action delegate to execute when one task in the <paramref name="tasks" /> array completes.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken">CancellationToken</see> 
		/// that will be assigned to the new continuation task.</param>
		/// <param name="continuationOptions">The <see cref="T:System.Threading.Tasks.TaskContinuationOptions">
		/// TaskContinuationOptions</see> value that controls the behavior of
		/// the created continuation <see cref="T:System.Threading.Tasks.Task">Task</see>.</param>
		/// <param name="scheduler">The <see cref="T:System.Threading.Tasks.TaskScheduler">TaskScheduler</see>
		/// that is used to schedule the created continuation <see cref="T:System.Threading.Tasks.Task">Task</see>.</param>
		/// <returns>The new continuation <see cref="T:System.Threading.Tasks.Task">Task</see>.</returns>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="continuationAction" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="scheduler" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array contains a null value.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is empty.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The exception that is thrown when the
		/// <paramref name="continuationOptions" /> argument specifies an invalid TaskContinuationOptions
		/// value.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The exception that is thrown when one 
		/// of the elements in the <paramref name="tasks" /> array has been disposed.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken">CancellationToken</see>
		/// has already been disposed.
		/// </exception>
		/// <remarks>
		/// The NotOn* and OnlyOn* <see cref="T:System.Threading.Tasks.TaskContinuationOptions">TaskContinuationOptions</see>, 
		/// which constrain for which <see cref="T:System.Threading.Tasks.TaskStatus">TaskStatus</see> states a continuation 
		/// will be executed, are illegal with ContinueWhenAny.
		/// </remarks>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task ContinueWhenAny(Task[] tasks, Action<Task> continuationAction, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return ContinueWhenAny(tasks, continuationAction, continuationOptions, cancellationToken, scheduler, ref stackMark);
		}

		private Task ContinueWhenAny(Task[] tasks, Action<Task> continuationAction, TaskContinuationOptions continuationOptions, CancellationToken cancellationToken, TaskScheduler scheduler, ref StackCrawlMark2 stackMark)
		{
			CheckMultiTaskContinuationOptions(continuationOptions);
			if (tasks == null)
			{
				throw new ArgumentNullException("tasks");
			}
			if (continuationAction == null)
			{
				throw new ArgumentNullException("continuationAction");
			}
			if (scheduler == null)
			{
				throw new ArgumentNullException("scheduler");
			}
			Task[] tasksCopy = CheckMultiContinuationTasksAndCopy(tasks);
			if (cancellationToken.IsCancellationRequested)
			{
				return CreateCanceledTask(continuationOptions);
			}
			Task<Task> task = CommonCWAnyLogic(tasksCopy);
			return task.ContinueWith(delegate(Task<Task> completedTask)
			{
				continuationAction(completedTask.Result);
			}, scheduler, cancellationToken, continuationOptions, ref stackMark);
		}

		/// <summary>
		/// Creates a continuation <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>
		/// that will be started upon the completion of any Task in the provided set.
		/// </summary>
		/// <typeparam name="TResult">The type of the result that is returned by the <paramref name="continuationFunction" />
		/// delegate and associated with the created <see cref="T:System.Threading.Tasks.Task{TResult}" />.</typeparam>
		/// <param name="tasks">The array of tasks from which to continue when one task completes.</param>
		/// <param name="continuationFunction">The function delegate to execute when one task in the
		/// <paramref name="tasks" /> array completes.</param>
		/// <returns>The new continuation <see cref="T:System.Threading.Tasks.Task{TResult}" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="continuationFunction" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array contains a null value.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is empty.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The exception that is thrown when one 
		/// of the elements in the <paramref name="tasks" /> array has been disposed.</exception>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task<TResult> ContinueWhenAny<TResult>(Task[] tasks, Func<Task, TResult> continuationFunction)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return ContinueWhenAny(tasks, continuationFunction, m_defaultContinuationOptions, m_defaultCancellationToken, DefaultScheduler, ref stackMark);
		}

		/// <summary>
		/// Creates a continuation <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>
		/// that will be started upon the completion of any Task in the provided set.
		/// </summary>
		/// <typeparam name="TResult">The type of the result that is returned by the <paramref name="continuationFunction" />
		/// delegate and associated with the created <see cref="T:System.Threading.Tasks.Task{TResult}" />.</typeparam>
		/// <param name="tasks">The array of tasks from which to continue when one task completes.</param>
		/// <param name="continuationFunction">The function delegate to execute when one task in the
		/// <paramref name="tasks" /> array completes.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken">CancellationToken</see> 
		/// that will be assigned to the new continuation task.</param>
		/// <returns>The new continuation <see cref="T:System.Threading.Tasks.Task{TResult}" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="continuationFunction" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array contains a null value.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is empty.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The exception that is thrown when one 
		/// of the elements in the <paramref name="tasks" /> array has been disposed.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken">CancellationToken</see>
		/// has already been disposed.
		/// </exception>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task<TResult> ContinueWhenAny<TResult>(Task[] tasks, Func<Task, TResult> continuationFunction, CancellationToken cancellationToken)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return ContinueWhenAny(tasks, continuationFunction, m_defaultContinuationOptions, cancellationToken, DefaultScheduler, ref stackMark);
		}

		/// <summary>
		/// Creates a continuation <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>
		/// that will be started upon the completion of any Task in the provided set.
		/// </summary>
		/// <typeparam name="TResult">The type of the result that is returned by the <paramref name="continuationFunction" />
		/// delegate and associated with the created <see cref="T:System.Threading.Tasks.Task{TResult}" />.</typeparam>
		/// <param name="tasks">The array of tasks from which to continue when one task completes.</param>
		/// <param name="continuationFunction">The function delegate to execute when one task in the
		/// <paramref name="tasks" /> array completes.</param>
		/// <param name="continuationOptions">The <see cref="T:System.Threading.Tasks.TaskContinuationOptions">
		/// TaskContinuationOptions</see> value that controls the behavior of
		/// the created continuation <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>.</param>
		/// <returns>The new continuation <see cref="T:System.Threading.Tasks.Task{TResult}" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="continuationFunction" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array contains a null value.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is empty.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The exception that is thrown when the
		/// <paramref name="continuationOptions" /> argument specifies an invalid TaskContinuationOptions
		/// value.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The exception that is thrown when one 
		/// of the elements in the <paramref name="tasks" /> array has been disposed.</exception>
		/// <remarks>
		/// The NotOn* and OnlyOn* <see cref="T:System.Threading.Tasks.TaskContinuationOptions">TaskContinuationOptions</see>, 
		/// which constrain for which <see cref="T:System.Threading.Tasks.TaskStatus">TaskStatus</see> states a continuation 
		/// will be executed, are illegal with ContinueWhenAny.
		/// </remarks>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task<TResult> ContinueWhenAny<TResult>(Task[] tasks, Func<Task, TResult> continuationFunction, TaskContinuationOptions continuationOptions)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return ContinueWhenAny(tasks, continuationFunction, continuationOptions, m_defaultCancellationToken, DefaultScheduler, ref stackMark);
		}

		/// <summary>
		/// Creates a continuation <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>
		/// that will be started upon the completion of any Task in the provided set.
		/// </summary>
		/// <typeparam name="TResult">The type of the result that is returned by the <paramref name="continuationFunction" />
		/// delegate and associated with the created <see cref="T:System.Threading.Tasks.Task{TResult}" />.</typeparam>
		/// <param name="tasks">The array of tasks from which to continue when one task completes.</param>
		/// <param name="continuationFunction">The function delegate to execute when one task in the
		/// <paramref name="tasks" /> array completes.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken">CancellationToken</see> 
		/// that will be assigned to the new continuation task.</param>
		/// <param name="continuationOptions">The <see cref="T:System.Threading.Tasks.TaskContinuationOptions">
		/// TaskContinuationOptions</see> value that controls the behavior of
		/// the created continuation <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>.</param>
		/// <param name="scheduler">The <see cref="T:System.Threading.Tasks.TaskScheduler">TaskScheduler</see>
		/// that is used to schedule the created continuation <see cref="T:System.Threading.Tasks.Task{TResult}" />.</param>
		/// <returns>The new continuation <see cref="T:System.Threading.Tasks.Task{TResult}" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="continuationFunction" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="scheduler" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array contains a null value.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is empty.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The exception that is thrown when the
		/// <paramref name="continuationOptions" /> argument specifies an invalid TaskContinuationOptions
		/// value.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The exception that is thrown when one 
		/// of the elements in the <paramref name="tasks" /> array has been disposed.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken">CancellationToken</see>
		/// has already been disposed.
		/// </exception>
		/// <remarks>
		/// The NotOn* and OnlyOn* <see cref="T:System.Threading.Tasks.TaskContinuationOptions">TaskContinuationOptions</see>, 
		/// which constrain for which <see cref="T:System.Threading.Tasks.TaskStatus">TaskStatus</see> states a continuation 
		/// will be executed, are illegal with ContinueWhenAny.
		/// </remarks>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task<TResult> ContinueWhenAny<TResult>(Task[] tasks, Func<Task, TResult> continuationFunction, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return ContinueWhenAny(tasks, continuationFunction, continuationOptions, cancellationToken, scheduler, ref stackMark);
		}

		private Task<TResult> ContinueWhenAny<TResult>(Task[] tasks, Func<Task, TResult> continuationFunction, TaskContinuationOptions continuationOptions, CancellationToken cancellationToken, TaskScheduler scheduler, ref StackCrawlMark2 stackMark)
		{
			return TaskFactory<TResult>.ContinueWhenAny(tasks, continuationFunction, continuationOptions, cancellationToken, scheduler, ref stackMark);
		}

		/// <summary>
		/// Creates a continuation <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>
		/// that will be started upon the completion of any Task in the provided set.
		/// </summary>
		/// <typeparam name="TResult">The type of the result that is returned by the <paramref name="continuationFunction" />
		/// delegate and associated with the created <see cref="T:System.Threading.Tasks.Task{TResult}" />.</typeparam>
		/// <typeparam name="TAntecedentResult">The type of the result of the antecedent <paramref name="tasks" />.</typeparam>
		/// <param name="tasks">The array of tasks from which to continue when one task completes.</param>
		/// <param name="continuationFunction">The function delegate to execute when one task in the
		/// <paramref name="tasks" /> array completes.</param>
		/// <returns>The new continuation <see cref="T:System.Threading.Tasks.Task{TResult}" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="continuationFunction" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array contains a null value.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is empty.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The exception that is thrown when one 
		/// of the elements in the <paramref name="tasks" /> array has been disposed.</exception>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task<TResult> ContinueWhenAny<TAntecedentResult, TResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>, TResult> continuationFunction)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return ContinueWhenAny(tasks, continuationFunction, m_defaultContinuationOptions, m_defaultCancellationToken, DefaultScheduler, ref stackMark);
		}

		/// <summary>
		/// Creates a continuation <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>
		/// that will be started upon the completion of any Task in the provided set.
		/// </summary>
		/// <typeparam name="TResult">The type of the result that is returned by the <paramref name="continuationFunction" />
		/// delegate and associated with the created <see cref="T:System.Threading.Tasks.Task{TResult}" />.</typeparam>
		/// <typeparam name="TAntecedentResult">The type of the result of the antecedent <paramref name="tasks" />.</typeparam>
		/// <param name="tasks">The array of tasks from which to continue when one task completes.</param>
		/// <param name="continuationFunction">The function delegate to execute when one task in the
		/// <paramref name="tasks" /> array completes.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken">CancellationToken</see> 
		/// that will be assigned to the new continuation task.</param>
		/// <returns>The new continuation <see cref="T:System.Threading.Tasks.Task{TResult}" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="continuationFunction" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array contains a null value.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is empty.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The exception that is thrown when one 
		/// of the elements in the <paramref name="tasks" /> array has been disposed.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken">CancellationToken</see>
		/// has already been disposed.
		/// </exception>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task<TResult> ContinueWhenAny<TAntecedentResult, TResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>, TResult> continuationFunction, CancellationToken cancellationToken)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return ContinueWhenAny(tasks, continuationFunction, m_defaultContinuationOptions, cancellationToken, DefaultScheduler, ref stackMark);
		}

		/// <summary>
		/// Creates a continuation <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>
		/// that will be started upon the completion of any Task in the provided set.
		/// </summary>
		/// <typeparam name="TResult">The type of the result that is returned by the <paramref name="continuationFunction" />
		/// delegate and associated with the created <see cref="T:System.Threading.Tasks.Task{TResult}" />.</typeparam>
		/// <typeparam name="TAntecedentResult">The type of the result of the antecedent <paramref name="tasks" />.</typeparam>
		/// <param name="tasks">The array of tasks from which to continue when one task completes.</param>
		/// <param name="continuationFunction">The function delegate to execute when one task in the
		/// <paramref name="tasks" /> array completes.</param>
		/// <param name="continuationOptions">The <see cref="T:System.Threading.Tasks.TaskContinuationOptions">
		/// TaskContinuationOptions</see> value that controls the behavior of
		/// the created continuation <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>.</param>
		/// <returns>The new continuation <see cref="T:System.Threading.Tasks.Task{TResult}" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="continuationFunction" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array contains a null value.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is empty.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The exception that is thrown when the
		/// <paramref name="continuationOptions" /> argument specifies an invalid TaskContinuationOptions
		/// value.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The exception that is thrown when one 
		/// of the elements in the <paramref name="tasks" /> array has been disposed.</exception>
		/// <remarks>
		/// The NotOn* and OnlyOn* <see cref="T:System.Threading.Tasks.TaskContinuationOptions">TaskContinuationOptions</see>, 
		/// which constrain for which <see cref="T:System.Threading.Tasks.TaskStatus">TaskStatus</see> states a continuation 
		/// will be executed, are illegal with ContinueWhenAny.
		/// </remarks>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task<TResult> ContinueWhenAny<TAntecedentResult, TResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>, TResult> continuationFunction, TaskContinuationOptions continuationOptions)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return ContinueWhenAny(tasks, continuationFunction, continuationOptions, m_defaultCancellationToken, DefaultScheduler, ref stackMark);
		}

		/// <summary>
		/// Creates a continuation <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>
		/// that will be started upon the completion of any Task in the provided set.
		/// </summary>
		/// <typeparam name="TResult">The type of the result that is returned by the <paramref name="continuationFunction" />
		/// delegate and associated with the created <see cref="T:System.Threading.Tasks.Task{TResult}" />.</typeparam>
		/// <typeparam name="TAntecedentResult">The type of the result of the antecedent <paramref name="tasks" />.</typeparam>
		/// <param name="tasks">The array of tasks from which to continue when one task completes.</param>
		/// <param name="continuationFunction">The function delegate to execute when one task in the
		/// <paramref name="tasks" /> array completes.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken">CancellationToken</see> 
		/// that will be assigned to the new continuation task.</param>
		/// <param name="continuationOptions">The <see cref="T:System.Threading.Tasks.TaskContinuationOptions">
		/// TaskContinuationOptions</see> value that controls the behavior of
		/// the created continuation <see cref="T:System.Threading.Tasks.Task{TResult}">Task</see>.</param>
		/// <param name="scheduler">The <see cref="T:System.Threading.Tasks.TaskScheduler">TaskScheduler</see>
		/// that is used to schedule the created continuation <see cref="T:System.Threading.Tasks.Task{TResult}" />.</param>
		/// <returns>The new continuation <see cref="T:System.Threading.Tasks.Task{TResult}" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="continuationFunction" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="scheduler" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array contains a null value.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is empty.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The exception that is thrown when the
		/// <paramref name="continuationOptions" /> argument specifies an invalid TaskContinuationOptions
		/// value.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The exception that is thrown when one 
		/// of the elements in the <paramref name="tasks" /> array has been disposed.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken">CancellationToken</see>
		/// has already been disposed.
		/// </exception>
		/// <remarks>
		/// The NotOn* and OnlyOn* <see cref="T:System.Threading.Tasks.TaskContinuationOptions">TaskContinuationOptions</see>, 
		/// which constrain for which <see cref="T:System.Threading.Tasks.TaskStatus">TaskStatus</see> states a continuation 
		/// will be executed, are illegal with ContinueWhenAny.
		/// </remarks>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task<TResult> ContinueWhenAny<TAntecedentResult, TResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>, TResult> continuationFunction, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return ContinueWhenAny(tasks, continuationFunction, continuationOptions, cancellationToken, scheduler, ref stackMark);
		}

		private Task<TResult> ContinueWhenAny<TAntecedentResult, TResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>, TResult> continuationFunction, TaskContinuationOptions continuationOptions, CancellationToken cancellationToken, TaskScheduler scheduler, ref StackCrawlMark2 stackMark)
		{
			return TaskFactory<TResult>.ContinueWhenAny(tasks, continuationFunction, continuationOptions, cancellationToken, scheduler, ref stackMark);
		}

		/// <summary>
		/// Creates a continuation <see cref="T:System.Threading.Tasks.Task">Task</see>
		/// that will be started upon the completion of any Task in the provided set.
		/// </summary>
		/// <typeparam name="TAntecedentResult">The type of the result of the antecedent <paramref name="tasks" />.</typeparam>
		/// <param name="tasks">The array of tasks from which to continue when one task completes.</param>
		/// <param name="continuationAction">The action delegate to execute when one task in the
		/// <paramref name="tasks" /> array completes.</param>
		/// <returns>The new continuation <see cref="T:System.Threading.Tasks.Task" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="continuationAction" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array contains a null value.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is empty.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The exception that is thrown when one 
		/// of the elements in the <paramref name="tasks" /> array has been disposed.</exception>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task ContinueWhenAny<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Action<Task<TAntecedentResult>> continuationAction)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return ContinueWhenAny(tasks, continuationAction, m_defaultContinuationOptions, m_defaultCancellationToken, DefaultScheduler, ref stackMark);
		}

		/// <summary>
		/// Creates a continuation <see cref="T:System.Threading.Tasks.Task">Task</see>
		/// that will be started upon the completion of any Task in the provided set.
		/// </summary>
		/// <typeparam name="TAntecedentResult">The type of the result of the antecedent <paramref name="tasks" />.</typeparam>
		/// <param name="tasks">The array of tasks from which to continue when one task completes.</param>
		/// <param name="continuationAction">The action delegate to execute when one task in the
		/// <paramref name="tasks" /> array completes.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken">CancellationToken</see> 
		/// that will be assigned to the new continuation task.</param>
		/// <returns>The new continuation <see cref="T:System.Threading.Tasks.Task" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="continuationAction" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array contains a null value.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is empty.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The exception that is thrown when one 
		/// of the elements in the <paramref name="tasks" /> array has been disposed.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken">CancellationToken</see>
		/// has already been disposed.
		/// </exception>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task ContinueWhenAny<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Action<Task<TAntecedentResult>> continuationAction, CancellationToken cancellationToken)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return ContinueWhenAny(tasks, continuationAction, m_defaultContinuationOptions, cancellationToken, DefaultScheduler, ref stackMark);
		}

		/// <summary>
		/// Creates a continuation <see cref="T:System.Threading.Tasks.Task">Task</see>
		/// that will be started upon the completion of any Task in the provided set.
		/// </summary>
		/// <typeparam name="TAntecedentResult">The type of the result of the antecedent <paramref name="tasks" />.</typeparam>
		/// <param name="tasks">The array of tasks from which to continue when one task completes.</param>
		/// <param name="continuationAction">The action delegate to execute when one task in the
		/// <paramref name="tasks" /> array completes.</param>
		/// <param name="continuationOptions">The <see cref="T:System.Threading.Tasks.TaskContinuationOptions">
		/// TaskContinuationOptions</see> value that controls the behavior of
		/// the created continuation <see cref="T:System.Threading.Tasks.Task">Task</see>.</param>
		/// <returns>The new continuation <see cref="T:System.Threading.Tasks.Task" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="continuationAction" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array contains a null value.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is empty.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The exception that is thrown when the
		/// <paramref name="continuationOptions" /> argument specifies an invalid TaskContinuationOptions
		/// value.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The exception that is thrown when one 
		/// of the elements in the <paramref name="tasks" /> array has been disposed.</exception>
		/// <remarks>
		/// The NotOn* and OnlyOn* <see cref="T:System.Threading.Tasks.TaskContinuationOptions">TaskContinuationOptions</see>, 
		/// which constrain for which <see cref="T:System.Threading.Tasks.TaskStatus">TaskStatus</see> states a continuation 
		/// will be executed, are illegal with ContinueWhenAny.
		/// </remarks>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task ContinueWhenAny<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Action<Task<TAntecedentResult>> continuationAction, TaskContinuationOptions continuationOptions)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return ContinueWhenAny(tasks, continuationAction, continuationOptions, m_defaultCancellationToken, DefaultScheduler, ref stackMark);
		}

		/// <summary>
		/// Creates a continuation <see cref="T:System.Threading.Tasks.Task">Task</see>
		/// that will be started upon the completion of any Task in the provided set.
		/// </summary>
		/// <typeparam name="TAntecedentResult">The type of the result of the antecedent <paramref name="tasks" />.</typeparam>
		/// <param name="tasks">The array of tasks from which to continue when one task completes.</param>
		/// <param name="continuationAction">The action delegate to execute when one task in the
		/// <paramref name="tasks" /> array completes.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken">CancellationToken</see> 
		/// that will be assigned to the new continuation task.</param>
		/// <param name="continuationOptions">The <see cref="T:System.Threading.Tasks.TaskContinuationOptions">
		/// TaskContinuationOptions</see> value that controls the behavior of
		/// the created continuation <see cref="T:System.Threading.Tasks.Task">Task</see>.</param>
		/// <param name="scheduler">The <see cref="T:System.Threading.Tasks.TaskScheduler">TaskScheduler</see>
		/// that is used to schedule the created continuation <see cref="T:System.Threading.Tasks.Task{TResult}" />.</param>
		/// <returns>The new continuation <see cref="T:System.Threading.Tasks.Task" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="continuationAction" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the
		/// <paramref name="scheduler" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array contains a null value.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the
		/// <paramref name="tasks" /> array is empty.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The exception that is thrown when the
		/// <paramref name="continuationOptions" /> argument specifies an invalid TaskContinuationOptions
		/// value.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The exception that is thrown when one 
		/// of the elements in the <paramref name="tasks" /> array has been disposed.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken">CancellationToken</see>
		/// has already been disposed.
		/// </exception>
		/// <remarks>
		/// The NotOn* and OnlyOn* <see cref="T:System.Threading.Tasks.TaskContinuationOptions">TaskContinuationOptions</see>, 
		/// which constrain for which <see cref="T:System.Threading.Tasks.TaskStatus">TaskStatus</see> states a continuation 
		/// will be executed, are illegal with ContinueWhenAny.
		/// </remarks>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Task ContinueWhenAny<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Action<Task<TAntecedentResult>> continuationAction, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler)
		{
			StackCrawlMark2 stackMark = StackCrawlMark2.LookForMyCaller;
			return ContinueWhenAny(tasks, continuationAction, continuationOptions, cancellationToken, scheduler, ref stackMark);
		}

		private Task ContinueWhenAny<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Action<Task<TAntecedentResult>> continuationAction, TaskContinuationOptions continuationOptions, CancellationToken cancellationToken, TaskScheduler scheduler, ref StackCrawlMark2 stackMark)
		{
			CheckMultiTaskContinuationOptions(continuationOptions);
			if (tasks == null)
			{
				throw new ArgumentNullException("tasks");
			}
			if (continuationAction == null)
			{
				throw new ArgumentNullException("continuationAction");
			}
			if (scheduler == null)
			{
				throw new ArgumentNullException("scheduler");
			}
			Task<TAntecedentResult>[] array = CheckMultiContinuationTasksAndCopy(tasks);
			if (cancellationToken.IsCancellationRequested)
			{
				return CreateCanceledTask(continuationOptions);
			}
			Task<Task> task = CommonCWAnyLogic(array);
			return task.ContinueWith(delegate(Task<Task> completedTask)
			{
				Task<TAntecedentResult> obj = completedTask.Result as Task<TAntecedentResult>;
				continuationAction(obj);
			}, scheduler, cancellationToken, continuationOptions, ref stackMark);
		}

		internal static Task[] CheckMultiContinuationTasksAndCopy(Task[] tasks)
		{
			if (tasks == null)
			{
				throw new ArgumentNullException("tasks");
			}
			if (tasks.Length == 0)
			{
				throw new ArgumentException(Environment2.GetResourceString("Task_MultiTaskContinuation_EmptyTaskList"), "tasks");
			}
			Task[] array = new Task[tasks.Length];
			for (int i = 0; i < tasks.Length; i++)
			{
				array[i] = tasks[i];
				if (array[i] == null)
				{
					throw new ArgumentException(Environment2.GetResourceString("Task_MultiTaskContinuation_NullTask"), "tasks");
				}
				array[i].ThrowIfDisposed();
			}
			return array;
		}

		internal static Task<TResult>[] CheckMultiContinuationTasksAndCopy<TResult>(Task<TResult>[] tasks)
		{
			if (tasks == null)
			{
				throw new ArgumentNullException("tasks");
			}
			if (tasks.Length == 0)
			{
				throw new ArgumentException(Environment2.GetResourceString("Task_MultiTaskContinuation_EmptyTaskList"), "tasks");
			}
			Task<TResult>[] array = new Task<TResult>[tasks.Length];
			for (int i = 0; i < tasks.Length; i++)
			{
				array[i] = tasks[i];
				if (array[i] == null)
				{
					throw new ArgumentException(Environment2.GetResourceString("Task_MultiTaskContinuation_NullTask"), "tasks");
				}
				array[i].ThrowIfDisposed();
			}
			return array;
		}

		internal static void CheckMultiTaskContinuationOptions(TaskContinuationOptions continuationOptions)
		{
			TaskContinuationOptions taskContinuationOptions = TaskContinuationOptions.OnlyOnRanToCompletion | TaskContinuationOptions.NotOnRanToCompletion;
			TaskContinuationOptions taskContinuationOptions2 = TaskContinuationOptions.LongRunning | TaskContinuationOptions.ExecuteSynchronously;
			if ((continuationOptions & taskContinuationOptions2) == taskContinuationOptions2)
			{
				throw new ArgumentOutOfRangeException("continuationOptions", Environment2.GetResourceString("Task_ContinueWith_ESandLR"));
			}
			if ((continuationOptions & ~(TaskContinuationOptions.PreferFairness | TaskContinuationOptions.LongRunning | TaskContinuationOptions.AttachedToParent | taskContinuationOptions | TaskContinuationOptions.ExecuteSynchronously)) != 0)
			{
				throw new ArgumentOutOfRangeException("continuationOptions");
			}
			if ((continuationOptions & taskContinuationOptions) != 0)
			{
				throw new ArgumentOutOfRangeException("continuationOptions", Environment2.GetResourceString("Task_MultiTaskContinuation_FireOptions"));
			}
		}
	}
}
