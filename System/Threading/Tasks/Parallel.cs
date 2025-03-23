using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Security.Permissions;

namespace System.Threading.Tasks
{
	/// <summary>
	/// Provides support for parallel loops and regions.
	/// </summary>
	/// <remarks>
	/// The <see cref="T:System.Threading.Tasks.Parallel" /> class provides library-based data parallel replacements
	/// for common operations such as for loops, for each loops, and execution of a set of statements.
	/// </remarks>
	[HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
	public static class Parallel
	{
		internal struct LoopTimer
		{
			private const int s_BaseNotifyPeriodMS = 100;

			private const int s_NotifyPeriodIncrementMS = 50;

			private int m_timeLimit;

			public LoopTimer(int nWorkerTaskIndex)
			{
				int num = 100 + nWorkerTaskIndex % Environment.ProcessorCount * 50;
				m_timeLimit = Environment.TickCount + num;
			}

			public bool LimitExceeded()
			{
				return Environment.TickCount > m_timeLimit;
			}
		}

		internal const int DEFAULT_LOOP_STRIDE = 16;

		internal static ParallelOptions s_defaultParallelOptions = new ParallelOptions();

		/// <summary>
		/// Executes each of the provided actions, possibly in parallel.
		/// </summary>
		/// <param name="actions">An array of <see cref="T:System.Action">Actions</see> to execute.</param>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the 
		/// <paramref name="actions" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the 
		/// <paramref name="actions" /> array contains a null element.</exception>
		/// <exception cref="T:System.AggregateException">The exception that is thrown when any
		/// action in the <paramref name="actions" /> array throws an exception.</exception>
		/// <remarks>
		/// This method can be used to execute a set of operations, potentially in parallel.   
		/// No guarantees are made about the order in which the operations execute or whether 
		/// they execute in parallel.  This method does not return until each of the 
		/// provided operations has completed, regardless of whether completion 
		/// occurs due to normal or exceptional termination.
		/// </remarks>
		public static void Invoke(params Action[] actions)
		{
			Invoke(s_defaultParallelOptions, actions);
		}

		/// <summary>
		/// Executes each of the provided actions, possibly in parallel.
		/// </summary>
		/// <param name="parallelOptions">A <see cref="T:System.Threading.Tasks.ParallelOptions">ParallelOptions</see> 
		/// instance that configures the behavior of this operation.</param>
		/// <param name="actions">An array of <see cref="T:System.Action">Actions</see> to execute.</param>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the 
		/// <paramref name="actions" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the 
		/// <paramref name="parallelOptions" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentException">The exception that is thrown when the 
		/// <paramref name="actions" /> array contains a null element.</exception>
		/// <exception cref="T:System.OperationCanceledException">The exception that is thrown when 
		/// the <see cref="T:System.Threading.CancellationToken">CancellationToken</see> in the 
		/// <paramref name="parallelOptions" /> is set.</exception>
		/// <exception cref="T:System.AggregateException">The exception that is thrown when any 
		/// action in the <paramref name="actions" /> array throws an exception.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The exception that is thrown when the 
		/// the <see cref="T:System.Threading.CancellationTokenSource">CancellationTokenSource</see> associated with the 
		/// the <see cref="T:System.Threading.CancellationToken">CancellationToken</see> in the 
		/// <paramref name="parallelOptions" /> has been disposed.</exception>
		/// <remarks>
		/// This method can be used to execute a set of operations, potentially in parallel.   
		/// No guarantees are made about the order in which the operations execute or whether 
		/// the they execute in parallel.  This method does not return until each of the 
		/// provided operations has completed, regardless of whether completion 
		/// occurs due to normal or exceptional termination.
		/// </remarks>
		public static void Invoke(ParallelOptions parallelOptions, params Action[] actions)
		{
			if (actions == null)
			{
				throw new ArgumentNullException("actions");
			}
			if (parallelOptions == null)
			{
				throw new ArgumentNullException("parallelOptions");
			}
			if (parallelOptions.CancellationToken.CanBeCanceled)
			{
				parallelOptions.CancellationToken.ThrowIfSourceDisposed();
			}
			if (parallelOptions.CancellationToken.IsCancellationRequested)
			{
				throw new OperationCanceledException2(parallelOptions.CancellationToken);
			}
			Action[] actionsCopy = new Action[actions.Length];
			for (int i = 0; i < actionsCopy.Length; i++)
			{
				actionsCopy[i] = actions[i];
				if (actionsCopy[i] == null)
				{
					throw new ArgumentException(Environment2.GetResourceString("Parallel_Invoke_ActionNull"));
				}
			}
			if (actionsCopy.Length < 1)
			{
				return;
			}
			if (actionsCopy.Length > 10 || (parallelOptions.MaxDegreeOfParallelism != -1 && parallelOptions.MaxDegreeOfParallelism < actionsCopy.Length))
			{
				ConcurrentQueue<Exception> exceptionQ = null;
				try
				{
					int actionIndex = 0;
					ParallelForReplicatingTask parallelForReplicatingTask = new ParallelForReplicatingTask(parallelOptions, delegate
					{
						for (int num = Interlocked.Increment(ref actionIndex); num <= actionsCopy.Length; num = Interlocked.Increment(ref actionIndex))
						{
							try
							{
								actionsCopy[num - 1]();
							}
							catch (Exception item)
							{
								LazyInitializer.EnsureInitialized(ref exceptionQ, () => new ConcurrentQueue<Exception>());
								exceptionQ.Enqueue(item);
							}
							if (parallelOptions.CancellationToken.IsCancellationRequested)
							{
								throw new OperationCanceledException2(parallelOptions.CancellationToken);
							}
						}
					}, TaskCreationOptions.None, InternalTaskOptions.SelfReplicating);
					parallelForReplicatingTask.RunSynchronously(parallelOptions.EffectiveTaskScheduler);
					parallelForReplicatingTask.Wait();
				}
				catch (Exception ex)
				{
					LazyInitializer.EnsureInitialized(ref exceptionQ, () => new ConcurrentQueue<Exception>());
					AggregateException ex2 = ex as AggregateException;
					if (ex2 != null)
					{
						foreach (Exception innerException in ex2.InnerExceptions)
						{
							exceptionQ.Enqueue(innerException);
						}
					}
					else
					{
						exceptionQ.Enqueue(ex);
					}
				}
				if (exceptionQ == null || exceptionQ.Count <= 0)
				{
					return;
				}
				ThrowIfReducableToSingleOCE(exceptionQ, parallelOptions.CancellationToken);
				throw new AggregateException(exceptionQ);
			}
			Task[] array = new Task[actionsCopy.Length];
			if (parallelOptions.CancellationToken.IsCancellationRequested)
			{
				throw new OperationCanceledException2(parallelOptions.CancellationToken);
			}
			for (int j = 0; j < array.Length; j++)
			{
				array[j] = Task.Factory.StartNew(actionsCopy[j], parallelOptions.CancellationToken, TaskCreationOptions.None, InternalTaskOptions.None, parallelOptions.EffectiveTaskScheduler);
			}
			try
			{
				if (array.Length <= 4)
				{
					Task.FastWaitAll(array);
				}
				else
				{
					Task.WaitAll(array);
				}
			}
			catch (AggregateException ex3)
			{
				ThrowIfReducableToSingleOCE(ex3.InnerExceptions, parallelOptions.CancellationToken);
				throw;
			}
			finally
			{
				for (int k = 0; k < array.Length; k++)
				{
					if (array[k].IsCompleted)
					{
						array[k].Dispose();
					}
				}
			}
		}

		/// <summary>
		/// Executes a for loop in which iterations may run in parallel.
		/// </summary>
		/// <param name="fromInclusive">The start index, inclusive.</param>
		/// <param name="toExclusive">The end index, exclusive.</param>
		/// <param name="body">The delegate that is invoked once per iteration.</param>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="body" /> 
		/// argument is null.</exception>
		/// <exception cref="T:System.AggregateException">The exception that is thrown to contain an exception
		/// thrown from one of the specified delegates.</exception>
		/// <returns>A <see cref="T:System.Threading.Tasks.ParallelLoopResult">ParallelLoopResult</see> structure
		/// that contains information on what portion of the loop completed.</returns>
		/// <remarks>
		/// The <paramref name="body" /> delegate is invoked once for each value in the iteration range: 
		/// [fromInclusive, toExclusive).  It is provided with the iteration count (an Int32) as a parameter.
		/// </remarks>
		public static ParallelLoopResult For(int fromInclusive, int toExclusive, Action<int> body)
		{
			if (body == null)
			{
				throw new ArgumentNullException("body");
			}
			return ForWorker<object>(fromInclusive, toExclusive, s_defaultParallelOptions, body, null, null, null, null);
		}

		/// <summary>
		/// Executes a for loop in which iterations may run in parallel.
		/// </summary>
		/// <param name="fromInclusive">The start index, inclusive.</param>
		/// <param name="toExclusive">The end index, exclusive.</param>
		/// <param name="body">The delegate that is invoked once per iteration.</param>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="body" /> 
		/// argument is null.</exception>
		/// <exception cref="T:System.AggregateException">The exception that is thrown to contain an exception
		/// thrown from one of the specified delegates.</exception>
		/// <returns>A <see cref="T:System.Threading.Tasks.ParallelLoopResult">ParallelLoopResult</see> structure
		/// that contains information on what portion of the loop completed.</returns>
		/// <remarks>
		/// The <paramref name="body" /> delegate is invoked once for each value in the iteration range: 
		/// [fromInclusive, toExclusive).  It is provided with the iteration count (an Int64) as a parameter.
		/// </remarks>
		public static ParallelLoopResult For(long fromInclusive, long toExclusive, Action<long> body)
		{
			if (body == null)
			{
				throw new ArgumentNullException("body");
			}
			return ForWorker64<object>(fromInclusive, toExclusive, s_defaultParallelOptions, body, null, null, null, null);
		}

		/// <summary>
		/// Executes a for loop in which iterations may run in parallel.
		/// </summary>
		/// <param name="fromInclusive">The start index, inclusive.</param>
		/// <param name="toExclusive">The end index, exclusive.</param>
		/// <param name="parallelOptions">A <see cref="T:System.Threading.Tasks.ParallelOptions">ParallelOptions</see> 
		/// instance that configures the behavior of this operation.</param>
		/// <param name="body">The delegate that is invoked once per iteration.</param>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="body" /> 
		/// argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the 
		/// <paramref name="parallelOptions" /> argument is null.</exception>
		/// <exception cref="T:System.OperationCanceledException">The exception that is thrown when the 
		/// <see cref="T:System.Threading.CancellationToken">CancellationToken</see> in the <paramref name="parallelOptions" /> 
		/// argument is set.</exception>
		/// <exception cref="T:System.AggregateException">The exception that is thrown to contain an exception
		/// thrown from one of the specified delegates.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The exception that is thrown when the 
		/// the <see cref="T:System.Threading.CancellationTokenSource">CancellationTokenSource</see> associated with the 
		/// the <see cref="T:System.Threading.CancellationToken">CancellationToken</see> in the 
		/// <paramref name="parallelOptions" /> has been disposed.</exception>
		/// <returns>A <see cref="T:System.Threading.Tasks.ParallelLoopResult">ParallelLoopResult</see> structure
		/// that contains information on what portion of the loop completed.</returns>
		/// <remarks>
		/// The <paramref name="body" /> delegate is invoked once for each value in the iteration range: 
		/// [fromInclusive, toExclusive).  It is provided with the iteration count (an Int32) as a parameter.
		/// </remarks>
		public static ParallelLoopResult For(int fromInclusive, int toExclusive, ParallelOptions parallelOptions, Action<int> body)
		{
			if (body == null)
			{
				throw new ArgumentNullException("body");
			}
			if (parallelOptions == null)
			{
				throw new ArgumentNullException("parallelOptions");
			}
			return ForWorker<object>(fromInclusive, toExclusive, parallelOptions, body, null, null, null, null);
		}

		/// <summary>
		/// Executes a for loop in which iterations may run in parallel.
		/// </summary>
		/// <param name="fromInclusive">The start index, inclusive.</param>
		/// <param name="toExclusive">The end index, exclusive.</param>
		/// <param name="parallelOptions">A <see cref="T:System.Threading.Tasks.ParallelOptions">ParallelOptions</see> 
		/// instance that configures the behavior of this operation.</param>
		/// <param name="body">The delegate that is invoked once per iteration.</param>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="body" /> 
		/// argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the 
		/// <paramref name="parallelOptions" /> argument is null.</exception>
		/// <exception cref="T:System.OperationCanceledException">The exception that is thrown when the 
		/// <see cref="T:System.Threading.CancellationToken">CancellationToken</see> in the <paramref name="parallelOptions" /> 
		/// argument is set.</exception>
		/// <exception cref="T:System.AggregateException">The exception that is thrown to contain an exception
		/// thrown from one of the specified delegates.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The exception that is thrown when the 
		/// the <see cref="T:System.Threading.CancellationTokenSource">CancellationTokenSource</see> associated with the 
		/// the <see cref="T:System.Threading.CancellationToken">CancellationToken</see> in the 
		/// <paramref name="parallelOptions" /> has been disposed.</exception>
		/// <returns>A <see cref="T:System.Threading.Tasks.ParallelLoopResult">ParallelLoopResult</see> structure
		/// that contains information on what portion of the loop completed.</returns>
		/// <remarks>
		/// The <paramref name="body" /> delegate is invoked once for each value in the iteration range: 
		/// [fromInclusive, toExclusive).  It is provided with the iteration count (an Int64) as a parameter.
		/// </remarks>
		public static ParallelLoopResult For(long fromInclusive, long toExclusive, ParallelOptions parallelOptions, Action<long> body)
		{
			if (body == null)
			{
				throw new ArgumentNullException("body");
			}
			if (parallelOptions == null)
			{
				throw new ArgumentNullException("parallelOptions");
			}
			return ForWorker64<object>(fromInclusive, toExclusive, parallelOptions, body, null, null, null, null);
		}

		/// <summary>
		/// Executes a for loop in which iterations may run in parallel.
		/// </summary>
		/// <param name="fromInclusive">The start index, inclusive.</param>
		/// <param name="toExclusive">The end index, exclusive.</param>
		/// <param name="body">The delegate that is invoked once per iteration.</param>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="body" /> 
		/// argument is null.</exception>
		/// <exception cref="T:System.AggregateException">The exception that is thrown to contain an exception
		/// thrown from one of the specified delegates.</exception>
		/// <returns>A <see cref="T:System.Threading.Tasks.ParallelLoopResult">ParallelLoopResult</see> structure
		/// that contains information on what portion of the loop completed.</returns>
		/// <remarks>
		/// <para>
		/// The <paramref name="body" /> delegate is invoked once for each value in the iteration range: 
		/// [fromInclusive, toExclusive).  It is provided with the following parameters: the iteration count (an Int32), 
		/// and a <see cref="T:System.Threading.Tasks.ParallelLoopState">ParallelLoopState</see> instance that may be 
		/// used to break out of the loop prematurely.
		/// </para>
		/// <para>
		/// Calling <see cref="M:System.Threading.Tasks.ParallelLoopState.Break">ParallelLoopState.Break()</see>
		/// informs the For operation that iterations after the current one need not 
		/// execute.  However, all iterations before the current one will still need to be executed if they have not already.
		/// Therefore, calling Break is similar to using a break operation within a 
		/// conventional for loop in a language like C#, but it is not a perfect substitute: for example, there is no guarantee that iterations 
		/// after the current one will definitely not execute.
		/// </para>
		/// <para>
		/// If executing all iterations before the current one is not necessary, 
		/// <see cref="M:System.Threading.Tasks.ParallelLoopState.Stop">ParallelLoopState.Stop()</see>
		/// should be preferred to using Break.  Calling Stop informs the For loop that it may abandon all remaining
		/// iterations, regardless of whether they're for interations above or below the current, 
		/// since all required work has already been completed.  As with Break, however, there are no guarantees regarding 
		/// which other iterations will not execute.
		/// </para>
		/// <para>
		/// When a loop is ended prematurely, the <see cref="T:ParallelLoopState" /> that's returned will contain
		/// relevant information about the loop's completion.
		/// </para>
		/// </remarks>
		public static ParallelLoopResult For(int fromInclusive, int toExclusive, Action<int, ParallelLoopState> body)
		{
			if (body == null)
			{
				throw new ArgumentNullException("body");
			}
			return ForWorker<object>(fromInclusive, toExclusive, s_defaultParallelOptions, null, body, null, null, null);
		}

		/// <summary>
		/// Executes a for loop in which iterations may run in parallel.
		/// </summary>
		/// <param name="fromInclusive">The start index, inclusive.</param>
		/// <param name="toExclusive">The end index, exclusive.</param>
		/// <param name="body">The delegate that is invoked once per iteration.</param>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="body" /> 
		/// argument is null.</exception>
		/// <exception cref="T:System.AggregateException">The exception that is thrown to contain an exception
		/// thrown from one of the specified delegates.</exception>
		/// <returns>A <see cref="T:System.Threading.Tasks.ParallelLoopResult">ParallelLoopResult</see> structure
		/// that contains information on what portion of the loop completed.</returns>
		/// <remarks>
		/// The <paramref name="body" /> delegate is invoked once for each value in the iteration range: 
		/// [fromInclusive, toExclusive).  It is provided with the following parameters: the iteration count (an Int64), 
		/// and a <see cref="T:System.Threading.Tasks.ParallelLoopState">ParallelLoopState</see> instance that may be 
		/// used to break out of the loop prematurely.
		/// </remarks>
		public static ParallelLoopResult For(long fromInclusive, long toExclusive, Action<long, ParallelLoopState> body)
		{
			if (body == null)
			{
				throw new ArgumentNullException("body");
			}
			return ForWorker64<object>(fromInclusive, toExclusive, s_defaultParallelOptions, null, body, null, null, null);
		}

		/// <summary>
		/// Executes a for loop in which iterations may run in parallel.
		/// </summary>
		/// <param name="fromInclusive">The start index, inclusive.</param>
		/// <param name="toExclusive">The end index, exclusive.</param>
		/// <param name="parallelOptions">A <see cref="T:System.Threading.Tasks.ParallelOptions">ParallelOptions</see> 
		/// instance that configures the behavior of this operation.</param>
		/// <param name="body">The delegate that is invoked once per iteration.</param>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="body" /> 
		/// argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the 
		/// <paramref name="parallelOptions" /> argument is null.</exception>
		/// <exception cref="T:System.OperationCanceledException">The exception that is thrown when the 
		/// <see cref="T:System.Threading.CancellationToken">CancellationToken</see> in the <paramref name="parallelOptions" /> 
		/// argument is set.</exception>
		/// <exception cref="T:System.AggregateException">The exception that is thrown to contain an exception
		/// thrown from one of the specified delegates.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The exception that is thrown when the 
		/// the <see cref="T:System.Threading.CancellationTokenSource">CancellationTokenSource</see> associated with the 
		/// the <see cref="T:System.Threading.CancellationToken">CancellationToken</see> in the 
		/// <paramref name="parallelOptions" /> has been disposed.</exception>
		/// <returns>A <see cref="T:System.Threading.Tasks.ParallelLoopResult">ParallelLoopResult</see> structure
		/// that contains information on what portion of the loop completed.</returns>
		/// <remarks>
		/// The <paramref name="body" /> delegate is invoked once for each value in the iteration range: 
		/// [fromInclusive, toExclusive).  It is provided with the following parameters: the iteration count (an Int32), 
		/// and a <see cref="T:System.Threading.Tasks.ParallelLoopState">ParallelLoopState</see> instance that may be 
		/// used to break out of the loop prematurely.
		/// </remarks>
		public static ParallelLoopResult For(int fromInclusive, int toExclusive, ParallelOptions parallelOptions, Action<int, ParallelLoopState> body)
		{
			if (body == null)
			{
				throw new ArgumentNullException("body");
			}
			if (parallelOptions == null)
			{
				throw new ArgumentNullException("parallelOptions");
			}
			return ForWorker<object>(fromInclusive, toExclusive, parallelOptions, null, body, null, null, null);
		}

		/// <summary>
		/// Executes a for loop in which iterations may run in parallel.
		/// </summary>
		/// <param name="fromInclusive">The start index, inclusive.</param>
		/// <param name="toExclusive">The end index, exclusive.</param>
		/// <param name="parallelOptions">A <see cref="T:System.Threading.Tasks.ParallelOptions">ParallelOptions</see> 
		/// instance that configures the behavior of this operation.</param>
		/// <param name="body">The delegate that is invoked once per iteration.</param>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="body" /> 
		/// argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the 
		/// <paramref name="parallelOptions" /> argument is null.</exception>
		/// <exception cref="T:System.OperationCanceledException">The exception that is thrown when the 
		/// <see cref="T:System.Threading.CancellationToken">CancellationToken</see> in the <paramref name="parallelOptions" /> 
		/// argument is set.</exception>
		/// <exception cref="T:System.AggregateException">The exception that is thrown to contain an exception
		/// thrown from one of the specified delegates.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The exception that is thrown when the 
		/// the <see cref="T:System.Threading.CancellationTokenSource">CancellationTokenSource</see> associated with the 
		/// the <see cref="T:System.Threading.CancellationToken">CancellationToken</see> in the 
		/// <paramref name="parallelOptions" /> has been disposed.</exception>
		/// <returns>A <see cref="T:System.Threading.Tasks.ParallelLoopResult">ParallelLoopResult</see> structure
		/// that contains information on what portion of the loop completed.</returns>
		/// <remarks>
		/// The <paramref name="body" /> delegate is invoked once for each value in the iteration range: 
		/// [fromInclusive, toExclusive).  It is provided with the following parameters: the iteration count (an Int64), 
		/// and a <see cref="T:System.Threading.Tasks.ParallelLoopState">ParallelLoopState</see> instance that may be 
		/// used to break out of the loop prematurely.
		/// </remarks>
		public static ParallelLoopResult For(long fromInclusive, long toExclusive, ParallelOptions parallelOptions, Action<long, ParallelLoopState> body)
		{
			if (body == null)
			{
				throw new ArgumentNullException("body");
			}
			if (parallelOptions == null)
			{
				throw new ArgumentNullException("parallelOptions");
			}
			return ForWorker64<object>(fromInclusive, toExclusive, parallelOptions, null, body, null, null, null);
		}

		/// <summary>
		/// Executes a for loop in which iterations may run in parallel.
		/// </summary>
		/// <typeparam name="TLocal">The type of the thread-local data.</typeparam>
		/// <param name="fromInclusive">The start index, inclusive.</param>
		/// <param name="toExclusive">The end index, exclusive.</param>
		/// <param name="localInit">The function delegate that returns the initial state of the local data 
		/// for each thread.</param>
		/// <param name="body">The delegate that is invoked once per iteration.</param>
		/// <param name="localFinally">The delegate that performs a final action on the local state of each
		/// thread.</param>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="body" /> 
		/// argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the 
		/// <paramref name="localInit" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the 
		/// <paramref name="localFinally" /> argument is null.</exception>
		/// <exception cref="T:System.AggregateException">The exception that is thrown to contain an exception
		/// thrown from one of the specified delegates.</exception>
		/// <returns>A <see cref="T:System.Threading.Tasks.ParallelLoopResult">ParallelLoopResult</see> structure
		/// that contains information on what portion of the loop completed.</returns>
		/// <remarks>
		/// <para>
		/// The <paramref name="body" /> delegate is invoked once for each value in the iteration range: 
		/// [fromInclusive, toExclusive).  It is provided with the following parameters: the iteration count (an Int32), 
		/// a <see cref="T:System.Threading.Tasks.ParallelLoopState">ParallelLoopState</see> instance that may be 
		/// used to break out of the loop prematurely, and some local state that may be shared amongst iterations 
		/// that execute on the same thread.
		/// </para>
		/// <para>
		/// The <paramref name="localInit" /> delegate is invoked once for each thread that participates in the loop's 
		/// execution and returns the initial local state for each of those threads.  These initial states are passed to the first
		/// <paramref name="body" /> invocations on each thread.  Then, every subsequent body invocation returns a possibly 
		/// modified state value that is passed to the next body invocation.  Finally, the last body invocation on each thread returns a state value 
		/// that is passed to the <paramref name="localFinally" /> delegate.  The localFinally delegate is invoked once per thread to perform a final 
		/// action on each thread's local state.
		/// </para>
		/// </remarks>
		public static ParallelLoopResult For<TLocal>(int fromInclusive, int toExclusive, Func<TLocal> localInit, Func<int, ParallelLoopState, TLocal, TLocal> body, Action<TLocal> localFinally)
		{
			if (body == null)
			{
				throw new ArgumentNullException("body");
			}
			if (localInit == null)
			{
				throw new ArgumentNullException("localInit");
			}
			if (localFinally == null)
			{
				throw new ArgumentNullException("localFinally");
			}
			return ForWorker(fromInclusive, toExclusive, s_defaultParallelOptions, null, null, body, localInit, localFinally);
		}

		/// <summary>
		/// Executes a for loop in which iterations may run in parallel.  Supports 64-bit indices.
		/// </summary>
		/// <typeparam name="TLocal">The type of the thread-local data.</typeparam>
		/// <param name="fromInclusive">The start index, inclusive.</param>
		/// <param name="toExclusive">The end index, exclusive.</param>
		/// <param name="localInit">The function delegate that returns the initial state of the local data 
		/// for each thread.</param>
		/// <param name="body">The delegate that is invoked once per iteration.</param>
		/// <param name="localFinally">The delegate that performs a final action on the local state of each
		/// thread.</param>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="body" /> 
		/// argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the 
		/// <paramref name="localInit" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the 
		/// <paramref name="localFinally" /> argument is null.</exception>
		/// <exception cref="T:System.AggregateException">The exception that is thrown to contain an exception
		/// thrown from one of the specified delegates.</exception>
		/// <returns>A <see cref="T:System.Threading.Tasks.ParallelLoopResult">ParallelLoopResult</see> structure
		/// that contains information on what portion of the loop completed.</returns>
		/// <remarks>
		/// <para>
		/// The <paramref name="body" /> delegate is invoked once for each value in the iteration range: 
		/// [fromInclusive, toExclusive).  It is provided with the following parameters: the iteration count (an Int64), 
		/// a <see cref="T:System.Threading.Tasks.ParallelLoopState">ParallelLoopState</see> instance that may be 
		/// used to break out of the loop prematurely, and some local state that may be shared amongst iterations 
		/// that execute on the same thread.
		/// </para>
		/// <para>
		/// The <paramref name="localInit" /> delegate is invoked once for each thread that participates in the loop's 
		/// execution and returns the initial local state for each of those threads.  These initial states are passed to the first
		/// <paramref name="body" /> invocations on each thread.  Then, every subsequent body invocation returns a possibly 
		/// modified state value that is passed to the next body invocation.  Finally, the last body invocation on each thread returns a state value 
		/// that is passed to the <paramref name="localFinally" /> delegate.  The localFinally delegate is invoked once per thread to perform a final 
		/// action on each thread's local state.
		/// </para>
		/// </remarks>
		public static ParallelLoopResult For<TLocal>(long fromInclusive, long toExclusive, Func<TLocal> localInit, Func<long, ParallelLoopState, TLocal, TLocal> body, Action<TLocal> localFinally)
		{
			if (body == null)
			{
				throw new ArgumentNullException("body");
			}
			if (localInit == null)
			{
				throw new ArgumentNullException("localInit");
			}
			if (localFinally == null)
			{
				throw new ArgumentNullException("localFinally");
			}
			return ForWorker64(fromInclusive, toExclusive, s_defaultParallelOptions, null, null, body, localInit, localFinally);
		}

		/// <summary>
		/// Executes a for loop in which iterations may run in parallel.
		/// </summary>
		/// <typeparam name="TLocal">The type of the thread-local data.</typeparam>
		/// <param name="fromInclusive">The start index, inclusive.</param>
		/// <param name="toExclusive">The end index, exclusive.</param>
		/// <param name="parallelOptions">A <see cref="T:System.Threading.Tasks.ParallelOptions">ParallelOptions</see> 
		/// instance that configures the behavior of this operation.</param>
		/// <param name="localInit">The function delegate that returns the initial state of the local data 
		/// for each thread.</param>
		/// <param name="body">The delegate that is invoked once per iteration.</param>
		/// <param name="localFinally">The delegate that performs a final action on the local state of each
		/// thread.</param>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="body" /> 
		/// argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the 
		/// <paramref name="localInit" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the 
		/// <paramref name="localFinally" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the 
		/// <paramref name="parallelOptions" /> argument is null.</exception>
		/// <exception cref="T:System.OperationCanceledException">The exception that is thrown when the 
		/// <see cref="T:System.Threading.CancellationToken">CancellationToken</see> in the <paramref name="parallelOptions" /> 
		/// argument is set.</exception>
		/// <exception cref="T:System.AggregateException">The exception that is thrown to contain an exception
		/// thrown from one of the specified delegates.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The exception that is thrown when the 
		/// the <see cref="T:System.Threading.CancellationTokenSource">CancellationTokenSource</see> associated with the 
		/// the <see cref="T:System.Threading.CancellationToken">CancellationToken</see> in the 
		/// <paramref name="parallelOptions" /> has been disposed.</exception>
		/// <returns>A <see cref="T:System.Threading.Tasks.ParallelLoopResult">ParallelLoopResult</see> structure
		/// that contains information on what portion of the loop completed.</returns>
		/// <remarks>
		/// <para>
		/// The <paramref name="body" /> delegate is invoked once for each value in the iteration range: 
		/// [fromInclusive, toExclusive).  It is provided with the following parameters: the iteration count (an Int32), 
		/// a <see cref="T:System.Threading.Tasks.ParallelLoopState">ParallelLoopState</see> instance that may be 
		/// used to break out of the loop prematurely, and some local state that may be shared amongst iterations 
		/// that execute on the same thread.
		/// </para>
		/// <para>
		/// The <paramref name="localInit" /> delegate is invoked once for each thread that participates in the loop's 
		/// execution and returns the initial local state for each of those threads.  These initial states are passed to the first
		/// <paramref name="body" /> invocations on each thread.  Then, every subsequent body invocation returns a possibly 
		/// modified state value that is passed to the next body invocation.  Finally, the last body invocation on each thread returns a state value 
		/// that is passed to the <paramref name="localFinally" /> delegate.  The localFinally delegate is invoked once per thread to perform a final 
		/// action on each thread's local state.
		/// </para>
		/// </remarks>
		public static ParallelLoopResult For<TLocal>(int fromInclusive, int toExclusive, ParallelOptions parallelOptions, Func<TLocal> localInit, Func<int, ParallelLoopState, TLocal, TLocal> body, Action<TLocal> localFinally)
		{
			if (body == null)
			{
				throw new ArgumentNullException("body");
			}
			if (localInit == null)
			{
				throw new ArgumentNullException("localInit");
			}
			if (localFinally == null)
			{
				throw new ArgumentNullException("localFinally");
			}
			if (parallelOptions == null)
			{
				throw new ArgumentNullException("parallelOptions");
			}
			return ForWorker(fromInclusive, toExclusive, parallelOptions, null, null, body, localInit, localFinally);
		}

		/// <summary>
		/// Executes a for loop in which iterations may run in parallel.
		/// </summary>
		/// <typeparam name="TLocal">The type of the thread-local data.</typeparam>
		/// <param name="fromInclusive">The start index, inclusive.</param>
		/// <param name="toExclusive">The end index, exclusive.</param>
		/// <param name="parallelOptions">A <see cref="T:System.Threading.Tasks.ParallelOptions">ParallelOptions</see> 
		/// instance that configures the behavior of this operation.</param>
		/// <param name="localInit">The function delegate that returns the initial state of the local data 
		/// for each thread.</param>
		/// <param name="body">The delegate that is invoked once per iteration.</param>
		/// <param name="localFinally">The delegate that performs a final action on the local state of each
		/// thread.</param>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="body" /> 
		/// argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the 
		/// <paramref name="localInit" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the 
		/// <paramref name="localFinally" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the 
		/// <paramref name="parallelOptions" /> argument is null.</exception>
		/// <exception cref="T:System.OperationCanceledException">The exception that is thrown when the 
		/// <see cref="T:System.Threading.CancellationToken">CancellationToken</see> in the <paramref name="parallelOptions" /> 
		/// argument is set.</exception>
		/// <exception cref="T:System.AggregateException">The exception that is thrown to contain an exception
		/// thrown from one of the specified delegates.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The exception that is thrown when the 
		/// the <see cref="T:System.Threading.CancellationTokenSource">CancellationTokenSource</see> associated with the 
		/// the <see cref="T:System.Threading.CancellationToken">CancellationToken</see> in the 
		/// <paramref name="parallelOptions" /> has been disposed.</exception>
		/// <returns>A <see cref="T:System.Threading.Tasks.ParallelLoopResult">ParallelLoopResult</see> structure
		/// that contains information on what portion of the loop completed.</returns>
		/// <remarks>
		/// <para>
		/// The <paramref name="body" /> delegate is invoked once for each value in the iteration range: 
		/// [fromInclusive, toExclusive).  It is provided with the following parameters: the iteration count (an Int64), 
		/// a <see cref="T:System.Threading.Tasks.ParallelLoopState">ParallelLoopState</see> instance that may be 
		/// used to break out of the loop prematurely, and some local state that may be shared amongst iterations 
		/// that execute on the same thread.
		/// </para>
		/// <para>
		/// The <paramref name="localInit" /> delegate is invoked once for each thread that participates in the loop's 
		/// execution and returns the initial local state for each of those threads.  These initial states are passed to the first
		/// <paramref name="body" /> invocations on each thread.  Then, every subsequent body invocation returns a possibly 
		/// modified state value that is passed to the next body invocation.  Finally, the last body invocation on each thread returns a state value 
		/// that is passed to the <paramref name="localFinally" /> delegate.  The localFinally delegate is invoked once per thread to perform a final 
		/// action on each thread's local state.
		/// </para>
		/// </remarks>
		public static ParallelLoopResult For<TLocal>(long fromInclusive, long toExclusive, ParallelOptions parallelOptions, Func<TLocal> localInit, Func<long, ParallelLoopState, TLocal, TLocal> body, Action<TLocal> localFinally)
		{
			if (body == null)
			{
				throw new ArgumentNullException("body");
			}
			if (localInit == null)
			{
				throw new ArgumentNullException("localInit");
			}
			if (localFinally == null)
			{
				throw new ArgumentNullException("localFinally");
			}
			if (parallelOptions == null)
			{
				throw new ArgumentNullException("parallelOptions");
			}
			return ForWorker64(fromInclusive, toExclusive, parallelOptions, null, null, body, localInit, localFinally);
		}

		/// <summary>
		/// Performs the major work of the parallel for loop. It assumes that argument validation has already
		/// been performed by the caller. This function's whole purpose in life is to enable as much reuse of
		/// common implementation details for the various For overloads we offer. Without it, we'd end up
		/// with lots of duplicate code. It handles: (1) simple for loops, (2) for loops that depend on
		/// ParallelState, and (3) for loops with thread local data.
		///
		/// @TODO: at some point in the future, we may want to manually inline the interesting bits into the
		/// specific overloads above. There is some overhead associated with the extra arguments passed to
		/// the function, and various if-checks in the code. It is also more difficult to follow what the
		/// code does as-is because it has to handle the three flavors.
		/// </summary>
		/// <typeparam name="TLocal">The type of the local data.</typeparam>
		/// <param name="fromInclusive">The loop's start index, inclusive.</param>
		/// <param name="toExclusive">The loop's end index, exclusive.</param>
		/// <param name="parallelOptions">A ParallelOptions instance.</param>
		/// <param name="body">The simple loop body.</param>
		/// <param name="bodyWithState">The loop body for ParallelState overloads.</param>
		/// <param name="bodyWithLocal">The loop body for thread local state overloads.</param>
		/// <param name="localInit">A selector function that returns new thread local state.</param>
		/// <param name="localFinally">A cleanup function to destroy thread local state.</param>
		/// <remarks>Only one of the body arguments may be supplied (i.e. they are exclusive).</remarks>
		/// <returns>A <see cref="T:System.Threading.Tasks.ParallelLoopResult" /> structure.</returns>
		private static ParallelLoopResult ForWorker<TLocal>(int fromInclusive, int toExclusive, ParallelOptions parallelOptions, Action<int> body, Action<int, ParallelLoopState> bodyWithState, Func<int, ParallelLoopState, TLocal, TLocal> bodyWithLocal, Func<TLocal> localInit, Action<TLocal> localFinally)
		{
			ParallelLoopResult result = default(ParallelLoopResult);
			if (toExclusive <= fromInclusive)
			{
				result.m_completed = true;
				return result;
			}
			ParallelLoopStateFlags32 sharedPStateFlags = new ParallelLoopStateFlags32();
			TaskCreationOptions creationOptions = TaskCreationOptions.None;
			InternalTaskOptions internalOptions = InternalTaskOptions.SelfReplicating;
			if (parallelOptions.CancellationToken.IsCancellationRequested)
			{
				throw new OperationCanceledException2(parallelOptions.CancellationToken);
			}
			int nNumExpectedWorkers = ((parallelOptions.EffectiveMaxConcurrencyLevel == -1) ? Environment.ProcessorCount : parallelOptions.EffectiveMaxConcurrencyLevel);
			RangeManager rangeManager = new RangeManager(fromInclusive, toExclusive, 1L, nNumExpectedWorkers);
			OperationCanceledException oce = null;
			CancellationTokenRegistration cancellationTokenRegistration = default(CancellationTokenRegistration);
			if (parallelOptions.CancellationToken.CanBeCanceled)
			{
				cancellationTokenRegistration = parallelOptions.CancellationToken.InternalRegisterWithoutEC(delegate
				{
					sharedPStateFlags.Cancel();
					oce = new OperationCanceledException2(parallelOptions.CancellationToken);
				}, null);
			}
			ParallelForReplicatingTask rootTask = null;
			try
			{
				rootTask = new ParallelForReplicatingTask(parallelOptions, delegate
				{
					Task internalCurrent = Task.InternalCurrent;
					bool flag = internalCurrent == rootTask;
					RangeWorker rangeWorker = default(RangeWorker);
					object savedStateFromPreviousReplica = internalCurrent.SavedStateFromPreviousReplica;
					rangeWorker = ((!(savedStateFromPreviousReplica is RangeWorker)) ? rangeManager.RegisterNewWorker() : ((RangeWorker)savedStateFromPreviousReplica));
					if (rangeWorker.FindNewWork32(out var nFromInclusiveLocal, out var nToExclusiveLocal) && !sharedPStateFlags.ShouldExitLoop(nFromInclusiveLocal))
					{
						TLocal val = default(TLocal);
						bool flag2 = false;
						try
						{
							ParallelLoopState32 parallelLoopState = null;
							if (bodyWithState != null)
							{
								parallelLoopState = new ParallelLoopState32(sharedPStateFlags);
							}
							else if (bodyWithLocal != null)
							{
								parallelLoopState = new ParallelLoopState32(sharedPStateFlags);
								if (localInit != null)
								{
									val = localInit();
									flag2 = true;
								}
							}
							LoopTimer loopTimer = new LoopTimer(rootTask.ActiveChildCount);
							do
							{
								if (body != null)
								{
									for (int i = nFromInclusiveLocal; i < nToExclusiveLocal; i++)
									{
										if (sharedPStateFlags.LoopStateFlags != ParallelLoopStateFlags.PLS_NONE && sharedPStateFlags.ShouldExitLoop())
										{
											break;
										}
										body(i);
									}
								}
								else if (bodyWithState != null)
								{
									for (int j = nFromInclusiveLocal; j < nToExclusiveLocal && (sharedPStateFlags.LoopStateFlags == ParallelLoopStateFlags.PLS_NONE || !sharedPStateFlags.ShouldExitLoop(j)); j++)
									{
										parallelLoopState.CurrentIteration = j;
										bodyWithState(j, parallelLoopState);
									}
								}
								else
								{
									for (int k = nFromInclusiveLocal; k < nToExclusiveLocal && (sharedPStateFlags.LoopStateFlags == ParallelLoopStateFlags.PLS_NONE || !sharedPStateFlags.ShouldExitLoop(k)); k++)
									{
										parallelLoopState.CurrentIteration = k;
										val = bodyWithLocal(k, parallelLoopState, val);
									}
								}
								if (!flag && loopTimer.LimitExceeded())
								{
									internalCurrent.SavedStateForNextReplica = rangeWorker;
									break;
								}
							}
							while (rangeWorker.FindNewWork32(out nFromInclusiveLocal, out nToExclusiveLocal) && (sharedPStateFlags.LoopStateFlags == ParallelLoopStateFlags.PLS_NONE || !sharedPStateFlags.ShouldExitLoop(nFromInclusiveLocal)));
						}
						catch
						{
							sharedPStateFlags.SetExceptional();
							throw;
						}
						finally
						{
							if (localFinally != null && flag2)
							{
								localFinally(val);
							}
						}
					}
				}, creationOptions, internalOptions);
				rootTask.RunSynchronously(parallelOptions.EffectiveTaskScheduler);
				rootTask.Wait();
				if (parallelOptions.CancellationToken.CanBeCanceled)
				{
					cancellationTokenRegistration.Dispose();
				}
				if (oce != null)
				{
					throw oce;
				}
			}
			catch (AggregateException ex)
			{
				if (parallelOptions.CancellationToken.CanBeCanceled)
				{
					cancellationTokenRegistration.Dispose();
				}
				ThrowIfReducableToSingleOCE(ex.InnerExceptions, parallelOptions.CancellationToken);
				throw;
			}
			catch (TaskSchedulerException)
			{
				if (parallelOptions.CancellationToken.CanBeCanceled)
				{
					cancellationTokenRegistration.Dispose();
				}
				throw;
			}
			finally
			{
				int loopStateFlags = sharedPStateFlags.LoopStateFlags;
				result.m_completed = loopStateFlags == ParallelLoopStateFlags.PLS_NONE;
				if ((loopStateFlags & ParallelLoopStateFlags.PLS_BROKEN) != 0)
				{
					result.m_lowestBreakIteration = sharedPStateFlags.LowestBreakIteration;
				}
				if (rootTask != null && rootTask.IsCompleted)
				{
					rootTask.Dispose();
				}
			}
			return result;
		}

		/// <summary>
		/// Performs the major work of the 64-bit parallel for loop. It assumes that argument validation has already
		/// been performed by the caller. This function's whole purpose in life is to enable as much reuse of
		/// common implementation details for the various For overloads we offer. Without it, we'd end up
		/// with lots of duplicate code. It handles: (1) simple for loops, (2) for loops that depend on
		/// ParallelState, and (3) for loops with thread local data.
		///
		/// @TODO: at some point in the future, we may want to manually inline the interesting bits into the
		/// specific overloads above. There is some overhead associated with the extra arguments passed to
		/// the function, and various if-checks in the code. It is also more difficult to follow what the
		/// code does as-is because it has to handle the three flavors.
		/// </summary>
		/// <typeparam name="TLocal">The type of the local data.</typeparam>
		/// <param name="fromInclusive">The loop's start index, inclusive.</param>
		/// <param name="toExclusive">The loop's end index, exclusive.</param>
		/// <param name="parallelOptions">A ParallelOptions instance.</param>
		/// <param name="body">The simple loop body.</param>
		/// <param name="bodyWithState">The loop body for ParallelState overloads.</param>
		/// <param name="bodyWithLocal">The loop body for thread local state overloads.</param>
		/// <param name="localInit">A selector function that returns new thread local state.</param>
		/// <param name="localFinally">A cleanup function to destroy thread local state.</param>
		/// <remarks>Only one of the body arguments may be supplied (i.e. they are exclusive).</remarks>
		/// <returns>A <see cref="T:System.Threading.Tasks.ParallelLoopResult" /> structure.</returns>
		private static ParallelLoopResult ForWorker64<TLocal>(long fromInclusive, long toExclusive, ParallelOptions parallelOptions, Action<long> body, Action<long, ParallelLoopState> bodyWithState, Func<long, ParallelLoopState, TLocal, TLocal> bodyWithLocal, Func<TLocal> localInit, Action<TLocal> localFinally)
		{
			ParallelLoopResult result = default(ParallelLoopResult);
			if (toExclusive <= fromInclusive)
			{
				result.m_completed = true;
				return result;
			}
			ParallelLoopStateFlags64 sharedPStateFlags = new ParallelLoopStateFlags64();
			TaskCreationOptions creationOptions = TaskCreationOptions.None;
			InternalTaskOptions internalOptions = InternalTaskOptions.SelfReplicating;
			if (parallelOptions.CancellationToken.IsCancellationRequested)
			{
				throw new OperationCanceledException2(parallelOptions.CancellationToken);
			}
			int nNumExpectedWorkers = ((parallelOptions.EffectiveMaxConcurrencyLevel == -1) ? Environment.ProcessorCount : parallelOptions.EffectiveMaxConcurrencyLevel);
			RangeManager rangeManager = new RangeManager(fromInclusive, toExclusive, 1L, nNumExpectedWorkers);
			OperationCanceledException oce = null;
			CancellationTokenRegistration cancellationTokenRegistration = default(CancellationTokenRegistration);
			if (parallelOptions.CancellationToken.CanBeCanceled)
			{
				cancellationTokenRegistration = parallelOptions.CancellationToken.InternalRegisterWithoutEC(delegate
				{
					sharedPStateFlags.Cancel();
					oce = new OperationCanceledException2(parallelOptions.CancellationToken);
				}, null);
			}
			ParallelForReplicatingTask rootTask = null;
			try
			{
				rootTask = new ParallelForReplicatingTask(parallelOptions, delegate
				{
					Task internalCurrent = Task.InternalCurrent;
					bool flag = internalCurrent == rootTask;
					RangeWorker rangeWorker = default(RangeWorker);
					object savedStateFromPreviousReplica = internalCurrent.SavedStateFromPreviousReplica;
					rangeWorker = ((!(savedStateFromPreviousReplica is RangeWorker)) ? rangeManager.RegisterNewWorker() : ((RangeWorker)savedStateFromPreviousReplica));
					if (rangeWorker.FindNewWork(out var nFromInclusiveLocal, out var nToExclusiveLocal) && !sharedPStateFlags.ShouldExitLoop(nFromInclusiveLocal))
					{
						TLocal val = default(TLocal);
						bool flag2 = false;
						try
						{
							ParallelLoopState64 parallelLoopState = null;
							if (bodyWithState != null)
							{
								parallelLoopState = new ParallelLoopState64(sharedPStateFlags);
							}
							else if (bodyWithLocal != null)
							{
								parallelLoopState = new ParallelLoopState64(sharedPStateFlags);
								if (localInit != null)
								{
									val = localInit();
									flag2 = true;
								}
							}
							LoopTimer loopTimer = new LoopTimer(rootTask.ActiveChildCount);
							do
							{
								if (body != null)
								{
									for (long num = nFromInclusiveLocal; num < nToExclusiveLocal; num++)
									{
										if (sharedPStateFlags.LoopStateFlags != ParallelLoopStateFlags.PLS_NONE && sharedPStateFlags.ShouldExitLoop())
										{
											break;
										}
										body(num);
									}
								}
								else if (bodyWithState != null)
								{
									for (long num2 = nFromInclusiveLocal; num2 < nToExclusiveLocal && (sharedPStateFlags.LoopStateFlags == ParallelLoopStateFlags.PLS_NONE || !sharedPStateFlags.ShouldExitLoop(num2)); num2++)
									{
										parallelLoopState.CurrentIteration = num2;
										bodyWithState(num2, parallelLoopState);
									}
								}
								else
								{
									for (long num3 = nFromInclusiveLocal; num3 < nToExclusiveLocal && (sharedPStateFlags.LoopStateFlags == ParallelLoopStateFlags.PLS_NONE || !sharedPStateFlags.ShouldExitLoop(num3)); num3++)
									{
										parallelLoopState.CurrentIteration = num3;
										val = bodyWithLocal(num3, parallelLoopState, val);
									}
								}
								if (!flag && loopTimer.LimitExceeded())
								{
									internalCurrent.SavedStateForNextReplica = rangeWorker;
									break;
								}
							}
							while (rangeWorker.FindNewWork(out nFromInclusiveLocal, out nToExclusiveLocal) && (sharedPStateFlags.LoopStateFlags == ParallelLoopStateFlags.PLS_NONE || !sharedPStateFlags.ShouldExitLoop(nFromInclusiveLocal)));
						}
						catch
						{
							sharedPStateFlags.SetExceptional();
							throw;
						}
						finally
						{
							if (localFinally != null && flag2)
							{
								localFinally(val);
							}
						}
					}
				}, creationOptions, internalOptions);
				rootTask.RunSynchronously(parallelOptions.EffectiveTaskScheduler);
				rootTask.Wait();
				if (parallelOptions.CancellationToken.CanBeCanceled)
				{
					cancellationTokenRegistration.Dispose();
				}
				if (oce != null)
				{
					throw oce;
				}
			}
			catch (AggregateException ex)
			{
				if (parallelOptions.CancellationToken.CanBeCanceled)
				{
					cancellationTokenRegistration.Dispose();
				}
				ThrowIfReducableToSingleOCE(ex.InnerExceptions, parallelOptions.CancellationToken);
				throw;
			}
			catch (TaskSchedulerException)
			{
				if (parallelOptions.CancellationToken.CanBeCanceled)
				{
					cancellationTokenRegistration.Dispose();
				}
				throw;
			}
			finally
			{
				int loopStateFlags = sharedPStateFlags.LoopStateFlags;
				result.m_completed = loopStateFlags == ParallelLoopStateFlags.PLS_NONE;
				if ((loopStateFlags & ParallelLoopStateFlags.PLS_BROKEN) != 0)
				{
					result.m_lowestBreakIteration = sharedPStateFlags.LowestBreakIteration;
				}
				if (rootTask != null && rootTask.IsCompleted)
				{
					rootTask.Dispose();
				}
			}
			return result;
		}

		/// <summary>
		/// Executes a for each operation on an <see cref="T:System.Collections.IEnumerable{TSource}" /> 
		/// in which iterations may run in parallel.
		/// </summary>
		/// <typeparam name="TSource">The type of the data in the source.</typeparam>
		/// <param name="source">An enumerable data source.</param>
		/// <param name="body">The delegate that is invoked once per iteration.</param>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="source" /> 
		/// argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="body" /> 
		/// argument is null.</exception>
		/// <exception cref="T:System.AggregateException">The exception that is thrown to contain an exception
		/// thrown from one of the specified delegates.</exception>
		/// <returns>A <see cref="T:System.Threading.Tasks.ParallelLoopResult">ParallelLoopResult</see> structure
		/// that contains information on what portion of the loop completed.</returns>
		/// <remarks>
		/// The <paramref name="body" /> delegate is invoked once for each element in the <paramref name="source" /> 
		/// enumerable.  It is provided with the current element as a parameter.
		/// </remarks>
		public static ParallelLoopResult ForEach<TSource>(IEnumerable<TSource> source, Action<TSource> body)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (body == null)
			{
				throw new ArgumentNullException("body");
			}
			return ForEachWorker<TSource, object>(source, s_defaultParallelOptions, body, null, null, null, null, null, null);
		}

		/// <summary>
		/// Executes a for each operation on an <see cref="T:System.Collections.IEnumerable{TSource}" /> 
		/// in which iterations may run in parallel.
		/// </summary>
		/// <typeparam name="TSource">The type of the data in the source.</typeparam>
		/// <param name="source">An enumerable data source.</param>
		/// <param name="parallelOptions">A <see cref="T:System.Threading.Tasks.ParallelOptions">ParallelOptions</see> 
		/// instance that configures the behavior of this operation.</param>
		/// <param name="body">The delegate that is invoked once per iteration.</param>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="source" /> 
		/// argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the 
		/// <paramref name="parallelOptions" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="body" /> 
		/// argument is null.</exception>
		/// <exception cref="T:System.OperationCanceledException">The exception that is thrown when the 
		/// <see cref="T:System.Threading.CancellationToken">CancellationToken</see> in the <paramref name="parallelOptions" /> 
		/// argument is set</exception>
		/// <exception cref="T:System.AggregateException">The exception that is thrown to contain an exception
		/// thrown from one of the specified delegates.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The exception that is thrown when the 
		/// the <see cref="T:System.Threading.CancellationTokenSource">CancellationTokenSource</see> associated with the 
		/// the <see cref="T:System.Threading.CancellationToken">CancellationToken</see> in the 
		/// <paramref name="parallelOptions" /> has been disposed.</exception>
		/// <returns>A <see cref="T:System.Threading.Tasks.ParallelLoopResult">ParallelLoopResult</see> structure
		/// that contains information on what portion of the loop completed.</returns>
		/// <remarks>
		/// The <paramref name="body" /> delegate is invoked once for each element in the <paramref name="source" /> 
		/// enumerable.  It is provided with the current element as a parameter.
		/// </remarks>
		public static ParallelLoopResult ForEach<TSource>(IEnumerable<TSource> source, ParallelOptions parallelOptions, Action<TSource> body)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (body == null)
			{
				throw new ArgumentNullException("body");
			}
			if (parallelOptions == null)
			{
				throw new ArgumentNullException("parallelOptions");
			}
			return ForEachWorker<TSource, object>(source, parallelOptions, body, null, null, null, null, null, null);
		}

		/// <summary>
		/// Executes a for each operation on an <see cref="T:System.Collections.IEnumerable{TSource}" /> 
		/// in which iterations may run in parallel.
		/// </summary>
		/// <typeparam name="TSource">The type of the data in the source.</typeparam>
		/// <param name="source">An enumerable data source.</param>
		/// <param name="body">The delegate that is invoked once per iteration.</param>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="source" /> 
		/// argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="body" /> 
		/// argument is null.</exception>
		/// <exception cref="T:System.AggregateException">The exception that is thrown to contain an exception
		/// thrown from one of the specified delegates.</exception>
		/// <returns>A <see cref="T:System.Threading.Tasks.ParallelLoopResult">ParallelLoopResult</see> structure
		/// that contains information on what portion of the loop completed.</returns>
		/// <remarks>
		/// The <paramref name="body" /> delegate is invoked once for each element in the <paramref name="source" /> 
		/// enumerable.  It is provided with the following parameters: the current element, 
		/// and a <see cref="T:System.Threading.Tasks.ParallelLoopState">ParallelLoopState</see> instance that may be 
		/// used to break out of the loop prematurely.
		/// </remarks>
		public static ParallelLoopResult ForEach<TSource>(IEnumerable<TSource> source, Action<TSource, ParallelLoopState> body)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (body == null)
			{
				throw new ArgumentNullException("body");
			}
			return ForEachWorker<TSource, object>(source, s_defaultParallelOptions, null, body, null, null, null, null, null);
		}

		/// <summary>
		/// Executes a for each operation on an <see cref="T:System.Collections.IEnumerable{TSource}" /> 
		/// in which iterations may run in parallel.
		/// </summary>
		/// <typeparam name="TSource">The type of the data in the source.</typeparam>
		/// <param name="source">An enumerable data source.</param>
		/// <param name="parallelOptions">A <see cref="T:System.Threading.Tasks.ParallelOptions">ParallelOptions</see> 
		/// instance that configures the behavior of this operation.</param>
		/// <param name="body">The delegate that is invoked once per iteration.</param>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="source" /> 
		/// argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the 
		/// <paramref name="parallelOptions" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="body" /> 
		/// argument is null.</exception>
		/// <exception cref="T:System.OperationCanceledException">The exception that is thrown when the 
		/// <see cref="T:System.Threading.CancellationToken">CancellationToken</see> in the <paramref name="parallelOptions" /> 
		/// argument is set</exception>
		/// <exception cref="T:System.AggregateException">The exception that is thrown to contain an exception
		/// thrown from one of the specified delegates.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The exception that is thrown when the 
		/// the <see cref="T:System.Threading.CancellationTokenSource">CancellationTokenSource</see> associated with the 
		/// the <see cref="T:System.Threading.CancellationToken">CancellationToken</see> in the 
		/// <paramref name="parallelOptions" /> has been disposed.</exception>
		/// <returns>A <see cref="T:System.Threading.Tasks.ParallelLoopResult">ParallelLoopResult</see> structure
		/// that contains information on what portion of the loop completed.</returns>
		/// <remarks>
		/// The <paramref name="body" /> delegate is invoked once for each element in the <paramref name="source" /> 
		/// enumerable.  It is provided with the following parameters: the current element, 
		/// and a <see cref="T:System.Threading.Tasks.ParallelLoopState">ParallelLoopState</see> instance that may be 
		/// used to break out of the loop prematurely.
		/// </remarks>
		public static ParallelLoopResult ForEach<TSource>(IEnumerable<TSource> source, ParallelOptions parallelOptions, Action<TSource, ParallelLoopState> body)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (body == null)
			{
				throw new ArgumentNullException("body");
			}
			if (parallelOptions == null)
			{
				throw new ArgumentNullException("parallelOptions");
			}
			return ForEachWorker<TSource, object>(source, parallelOptions, null, body, null, null, null, null, null);
		}

		/// <summary>
		/// Executes a for each operation on an <see cref="T:System.Collections.IEnumerable{TSource}" /> 
		/// in which iterations may run in parallel.
		/// </summary>
		/// <typeparam name="TSource">The type of the data in the source.</typeparam>
		/// <param name="source">An enumerable data source.</param>
		/// <param name="body">The delegate that is invoked once per iteration.</param>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="source" /> 
		/// argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="body" /> 
		/// argument is null.</exception>
		/// <exception cref="T:System.AggregateException">The exception that is thrown to contain an exception
		/// thrown from one of the specified delegates.</exception>
		/// <returns>A <see cref="T:System.Threading.Tasks.ParallelLoopResult">ParallelLoopResult</see> structure
		/// that contains information on what portion of the loop completed.</returns>
		/// <remarks>
		/// The <paramref name="body" /> delegate is invoked once for each element in the <paramref name="source" /> 
		/// enumerable.  It is provided with the following parameters: the current element, 
		/// a <see cref="T:System.Threading.Tasks.ParallelLoopState">ParallelLoopState</see> instance that may be 
		/// used to break out of the loop prematurely, and the current element's index (an Int64).
		/// </remarks>
		public static ParallelLoopResult ForEach<TSource>(IEnumerable<TSource> source, Action<TSource, ParallelLoopState, long> body)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (body == null)
			{
				throw new ArgumentNullException("body");
			}
			return ForEachWorker<TSource, object>(source, s_defaultParallelOptions, null, null, body, null, null, null, null);
		}

		/// <summary>
		/// Executes a for each operation on an <see cref="T:System.Collections.IEnumerable{TSource}" /> 
		/// in which iterations may run in parallel.
		/// </summary>
		/// <typeparam name="TSource">The type of the data in the source.</typeparam>
		/// <param name="source">An enumerable data source.</param>
		/// <param name="parallelOptions">A <see cref="T:System.Threading.Tasks.ParallelOptions">ParallelOptions</see> 
		/// instance that configures the behavior of this operation.</param>
		/// <param name="body">The delegate that is invoked once per iteration.</param>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="source" /> 
		/// argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the 
		/// <paramref name="parallelOptions" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="body" /> 
		/// argument is null.</exception>
		/// <exception cref="T:System.OperationCanceledException">The exception that is thrown when the 
		/// <see cref="T:System.Threading.CancellationToken">CancellationToken</see> in the <paramref name="parallelOptions" /> 
		/// argument is set</exception>
		/// <exception cref="T:System.AggregateException">The exception that is thrown to contain an exception
		/// thrown from one of the specified delegates.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The exception that is thrown when the 
		/// the <see cref="T:System.Threading.CancellationTokenSource">CancellationTokenSource</see> associated with the 
		/// the <see cref="T:System.Threading.CancellationToken">CancellationToken</see> in the 
		/// <paramref name="parallelOptions" /> has been disposed.</exception>
		/// <returns>A <see cref="T:System.Threading.Tasks.ParallelLoopResult">ParallelLoopResult</see> structure
		/// that contains information on what portion of the loop completed.</returns>
		/// <remarks>
		/// The <paramref name="body" /> delegate is invoked once for each element in the <paramref name="source" /> 
		/// enumerable.  It is provided with the following parameters: the current element, 
		/// a <see cref="T:System.Threading.Tasks.ParallelLoopState">ParallelLoopState</see> instance that may be 
		/// used to break out of the loop prematurely, and the current element's index (an Int64).
		/// </remarks>
		public static ParallelLoopResult ForEach<TSource>(IEnumerable<TSource> source, ParallelOptions parallelOptions, Action<TSource, ParallelLoopState, long> body)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (body == null)
			{
				throw new ArgumentNullException("body");
			}
			if (parallelOptions == null)
			{
				throw new ArgumentNullException("parallelOptions");
			}
			return ForEachWorker<TSource, object>(source, parallelOptions, null, null, body, null, null, null, null);
		}

		/// <summary>
		/// Executes a for each operation on an <see cref="T:System.Collections.IEnumerable{TSource}" /> 
		/// in which iterations may run in parallel.
		/// </summary>
		/// <typeparam name="TSource">The type of the data in the source.</typeparam>
		/// <typeparam name="TLocal">The type of the thread-local data.</typeparam>
		/// <param name="source">An enumerable data source.</param>
		/// <param name="localInit">The function delegate that returns the initial state of the local data 
		/// for each thread.</param>
		/// <param name="body">The delegate that is invoked once per iteration.</param>
		/// <param name="localFinally">The delegate that performs a final action on the local state of each
		/// thread.</param>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="source" /> 
		/// argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="body" /> 
		/// argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the 
		/// <paramref name="localInit" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the 
		/// <paramref name="localFinally" /> argument is null.</exception>
		/// <exception cref="T:System.AggregateException">The exception that is thrown to contain an exception
		/// thrown from one of the specified delegates.</exception>
		/// <returns>A <see cref="T:System.Threading.Tasks.ParallelLoopResult">ParallelLoopResult</see> structure
		/// that contains information on what portion of the loop completed.</returns>
		/// <remarks>
		/// <para>
		/// The <paramref name="body" /> delegate is invoked once for each element in the <paramref name="source" /> 
		/// enumerable.  It is provided with the following parameters: the current element, 
		/// a <see cref="T:System.Threading.Tasks.ParallelLoopState">ParallelLoopState</see> instance that may be 
		/// used to break out of the loop prematurely, and some local state that may be shared amongst iterations 
		/// that execute on the same thread.
		/// </para>
		/// <para>
		/// The <paramref name="localInit" /> delegate is invoked once for each thread that participates in the loop's 
		/// execution and returns the initial local state for each of those threads.  These initial states are passed to the first
		/// <paramref name="body" /> invocations on each thread.  Then, every subsequent body invocation returns a possibly 
		/// modified state value that is passed to the next body invocation.  Finally, the last body invocation on each thread returns a state value 
		/// that is passed to the <paramref name="localFinally" /> delegate.  The localFinally delegate is invoked once per thread to perform a final 
		/// action on each thread's local state.
		/// </para>
		/// </remarks>
		public static ParallelLoopResult ForEach<TSource, TLocal>(IEnumerable<TSource> source, Func<TLocal> localInit, Func<TSource, ParallelLoopState, TLocal, TLocal> body, Action<TLocal> localFinally)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (body == null)
			{
				throw new ArgumentNullException("body");
			}
			if (localInit == null)
			{
				throw new ArgumentNullException("localInit");
			}
			if (localFinally == null)
			{
				throw new ArgumentNullException("localFinally");
			}
			return ForEachWorker(source, s_defaultParallelOptions, null, null, null, body, null, localInit, localFinally);
		}

		/// <summary>
		/// Executes a for each operation on an <see cref="T:System.Collections.IEnumerable{TSource}" /> 
		/// in which iterations may run in parallel.
		/// </summary>
		/// <typeparam name="TSource">The type of the data in the source.</typeparam>
		/// <typeparam name="TLocal">The type of the thread-local data.</typeparam>
		/// <param name="source">An enumerable data source.</param>
		/// <param name="parallelOptions">A <see cref="T:System.Threading.Tasks.ParallelOptions">ParallelOptions</see> 
		/// instance that configures the behavior of this operation.</param>
		/// <param name="localInit">The function delegate that returns the initial state of the local data 
		/// for each thread.</param>
		/// <param name="body">The delegate that is invoked once per iteration.</param>
		/// <param name="localFinally">The delegate that performs a final action on the local state of each
		/// thread.</param>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="source" /> 
		/// argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the 
		/// <paramref name="parallelOptions" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="body" /> 
		/// argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the 
		/// <paramref name="localInit" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the 
		/// <paramref name="localFinally" /> argument is null.</exception>
		/// <exception cref="T:System.OperationCanceledException">The exception that is thrown when the 
		/// <see cref="T:System.Threading.CancellationToken">CancellationToken</see> in the <paramref name="parallelOptions" /> 
		/// argument is set</exception>
		/// <exception cref="T:System.AggregateException">The exception that is thrown to contain an exception
		/// thrown from one of the specified delegates.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The exception that is thrown when the 
		/// the <see cref="T:System.Threading.CancellationTokenSource">CancellationTokenSource</see> associated with the 
		/// the <see cref="T:System.Threading.CancellationToken">CancellationToken</see> in the 
		/// <paramref name="parallelOptions" /> has been disposed.</exception>
		/// <returns>A <see cref="T:System.Threading.Tasks.ParallelLoopResult">ParallelLoopResult</see> structure
		/// that contains information on what portion of the loop completed.</returns>
		/// <remarks>
		/// <para>
		/// The <paramref name="body" /> delegate is invoked once for each element in the <paramref name="source" /> 
		/// enumerable.  It is provided with the following parameters: the current element, 
		/// a <see cref="T:System.Threading.Tasks.ParallelLoopState">ParallelLoopState</see> instance that may be 
		/// used to break out of the loop prematurely, and some local state that may be shared amongst iterations 
		/// that execute on the same thread.
		/// </para>
		/// <para>
		/// The <paramref name="localInit" /> delegate is invoked once for each thread that participates in the loop's 
		/// execution and returns the initial local state for each of those threads.  These initial states are passed to the first
		/// <paramref name="body" /> invocations on each thread.  Then, every subsequent body invocation returns a possibly 
		/// modified state value that is passed to the next body invocation.  Finally, the last body invocation on each thread returns a state value 
		/// that is passed to the <paramref name="localFinally" /> delegate.  The localFinally delegate is invoked once per thread to perform a final 
		/// action on each thread's local state.
		/// </para>
		/// </remarks>
		public static ParallelLoopResult ForEach<TSource, TLocal>(IEnumerable<TSource> source, ParallelOptions parallelOptions, Func<TLocal> localInit, Func<TSource, ParallelLoopState, TLocal, TLocal> body, Action<TLocal> localFinally)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (body == null)
			{
				throw new ArgumentNullException("body");
			}
			if (localInit == null)
			{
				throw new ArgumentNullException("localInit");
			}
			if (localFinally == null)
			{
				throw new ArgumentNullException("localFinally");
			}
			if (parallelOptions == null)
			{
				throw new ArgumentNullException("parallelOptions");
			}
			return ForEachWorker(source, parallelOptions, null, null, null, body, null, localInit, localFinally);
		}

		/// <summary>
		/// Executes a for each operation on an <see cref="T:System.Collections.IEnumerable{TSource}" /> 
		/// in which iterations may run in parallel.
		/// </summary>
		/// <typeparam name="TSource">The type of the data in the source.</typeparam>
		/// <typeparam name="TLocal">The type of the thread-local data.</typeparam>
		/// <param name="source">An enumerable data source.</param>
		/// <param name="localInit">The function delegate that returns the initial state of the local data 
		/// for each thread.</param>
		/// <param name="body">The delegate that is invoked once per iteration.</param>
		/// <param name="localFinally">The delegate that performs a final action on the local state of each
		/// thread.</param>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="source" /> 
		/// argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="body" /> 
		/// argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the 
		/// <paramref name="localInit" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the 
		/// <paramref name="localFinally" /> argument is null.</exception>
		/// <exception cref="T:System.AggregateException">The exception that is thrown to contain an exception
		/// thrown from one of the specified delegates.</exception>
		/// <returns>A <see cref="T:System.Threading.Tasks.ParallelLoopResult">ParallelLoopResult</see> structure
		/// that contains information on what portion of the loop completed.</returns>
		/// <remarks>
		/// <para>
		/// The <paramref name="body" /> delegate is invoked once for each element in the <paramref name="source" /> 
		/// enumerable.  It is provided with the following parameters: the current element, 
		/// a <see cref="T:System.Threading.Tasks.ParallelLoopState">ParallelLoopState</see> instance that may be 
		/// used to break out of the loop prematurely, the current element's index (an Int64), and some local 
		/// state that may be shared amongst iterations that execute on the same thread.
		/// </para>
		/// <para>
		/// The <paramref name="localInit" /> delegate is invoked once for each thread that participates in the loop's 
		/// execution and returns the initial local state for each of those threads.  These initial states are passed to the first
		/// <paramref name="body" /> invocations on each thread.  Then, every subsequent body invocation returns a possibly 
		/// modified state value that is passed to the next body invocation.  Finally, the last body invocation on each thread returns a state value 
		/// that is passed to the <paramref name="localFinally" /> delegate.  The localFinally delegate is invoked once per thread to perform a final 
		/// action on each thread's local state.
		/// </para>
		/// </remarks>
		public static ParallelLoopResult ForEach<TSource, TLocal>(IEnumerable<TSource> source, Func<TLocal> localInit, Func<TSource, ParallelLoopState, long, TLocal, TLocal> body, Action<TLocal> localFinally)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (body == null)
			{
				throw new ArgumentNullException("body");
			}
			if (localInit == null)
			{
				throw new ArgumentNullException("localInit");
			}
			if (localFinally == null)
			{
				throw new ArgumentNullException("localFinally");
			}
			return ForEachWorker(source, s_defaultParallelOptions, null, null, null, null, body, localInit, localFinally);
		}

		/// <summary>
		/// Executes a for each operation on an <see cref="T:System.Collections.IEnumerable{TSource}" /> 
		/// in which iterations may run in parallel.
		/// </summary>
		/// <typeparam name="TSource">The type of the data in the source.</typeparam>
		/// <typeparam name="TLocal">The type of the thread-local data.</typeparam>
		/// <param name="source">An enumerable data source.</param>
		/// <param name="parallelOptions">A <see cref="T:System.Threading.Tasks.ParallelOptions">ParallelOptions</see> 
		/// instance that configures the behavior of this operation.</param>
		/// <param name="localInit">The function delegate that returns the initial state of the local data 
		/// for each thread.</param>
		/// <param name="body">The delegate that is invoked once per iteration.</param>
		/// <param name="localFinally">The delegate that performs a final action on the local state of each
		/// thread.</param>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="source" /> 
		/// argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the 
		/// <paramref name="parallelOptions" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="body" /> 
		/// argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the 
		/// <paramref name="localInit" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the 
		/// <paramref name="localFinally" /> argument is null.</exception>
		/// <exception cref="T:System.OperationCanceledException">The exception that is thrown when the 
		/// <see cref="T:System.Threading.CancellationToken">CancellationToken</see> in the <paramref name="parallelOptions" /> 
		/// argument is set</exception>
		/// <exception cref="T:System.AggregateException">The exception that is thrown to contain an exception
		/// thrown from one of the specified delegates.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The exception that is thrown when the 
		/// the <see cref="T:System.Threading.CancellationTokenSource">CancellationTokenSource</see> associated with the 
		/// the <see cref="T:System.Threading.CancellationToken">CancellationToken</see> in the 
		/// <paramref name="parallelOptions" /> has been disposed.</exception>
		/// <returns>A <see cref="T:System.Threading.Tasks.ParallelLoopResult">ParallelLoopResult</see> structure
		/// that contains information on what portion of the loop completed.</returns>
		/// <remarks>
		/// <para>
		/// The <paramref name="body" /> delegate is invoked once for each element in the <paramref name="source" /> 
		/// enumerable.  It is provided with the following parameters: the current element, 
		/// a <see cref="T:System.Threading.Tasks.ParallelLoopState">ParallelLoopState</see> instance that may be 
		/// used to break out of the loop prematurely, the current element's index (an Int64), and some local 
		/// state that may be shared amongst iterations that execute on the same thread.
		/// </para>
		/// <para>
		/// The <paramref name="localInit" /> delegate is invoked once for each thread that participates in the loop's 
		/// execution and returns the initial local state for each of those threads.  These initial states are passed to the first
		/// <paramref name="body" /> invocations on each thread.  Then, every subsequent body invocation returns a possibly 
		/// modified state value that is passed to the next body invocation.  Finally, the last body invocation on each thread returns a state value 
		/// that is passed to the <paramref name="localFinally" /> delegate.  The localFinally delegate is invoked once per thread to perform a final 
		/// action on each thread's local state.
		/// </para>
		/// </remarks>
		public static ParallelLoopResult ForEach<TSource, TLocal>(IEnumerable<TSource> source, ParallelOptions parallelOptions, Func<TLocal> localInit, Func<TSource, ParallelLoopState, long, TLocal, TLocal> body, Action<TLocal> localFinally)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (body == null)
			{
				throw new ArgumentNullException("body");
			}
			if (localInit == null)
			{
				throw new ArgumentNullException("localInit");
			}
			if (localFinally == null)
			{
				throw new ArgumentNullException("localFinally");
			}
			if (parallelOptions == null)
			{
				throw new ArgumentNullException("parallelOptions");
			}
			return ForEachWorker(source, parallelOptions, null, null, null, null, body, localInit, localFinally);
		}

		/// <summary>
		/// Performs the major work of the parallel foreach loop. It assumes that argument validation has
		/// already been performed by the caller. This function's whole purpose in life is to enable as much
		/// reuse of common implementation details for the various For overloads we offer. Without it, we'd
		/// end up with lots of duplicate code. It handles: (1) simple foreach loops, (2) foreach loops that
		/// depend on ParallelState, and (3) foreach loops that access indices, (4) foreach loops with thread
		/// local data, and any necessary permutations thereof.
		///
		/// @TODO: at some point in the future, we may want to manually inline the interesting bits into the
		/// specific overloads above. There is some overhead associated with the extra arguments passed to
		/// the function, and various if-checks in the code. It is also more difficult to follow what the
		/// code does as-is because it has to handle the all flavors.
		/// </summary>
		/// <typeparam name="TSource">The type of the source data.</typeparam>
		/// <typeparam name="TLocal">The type of the local data.</typeparam>
		/// <param name="source">An enumerable data source.</param>
		/// <param name="parallelOptions">ParallelOptions instance to use with this ForEach-loop</param>
		/// <param name="body">The simple loop body.</param>
		/// <param name="bodyWithState">The loop body for ParallelState overloads.</param>
		/// <param name="bodyWithStateAndIndex">The loop body for ParallelState/indexed overloads.</param>
		/// <param name="bodyWithStateAndLocal">The loop body for ParallelState/thread local state overloads.</param>
		/// <param name="bodyWithEverything">The loop body for ParallelState/indexed/thread local state overloads.</param>
		/// <param name="localInit">A selector function that returns new thread local state.</param>
		/// <param name="localFinally">A cleanup function to destroy thread local state.</param>
		/// <remarks>Only one of the bodyXX arguments may be supplied (i.e. they are exclusive).</remarks>
		/// <returns>A <see cref="T:System.Threading.Tasks.ParallelLoopResult" /> structure.</returns>
		private static ParallelLoopResult ForEachWorker<TSource, TLocal>(IEnumerable<TSource> source, ParallelOptions parallelOptions, Action<TSource> body, Action<TSource, ParallelLoopState> bodyWithState, Action<TSource, ParallelLoopState, long> bodyWithStateAndIndex, Func<TSource, ParallelLoopState, TLocal, TLocal> bodyWithStateAndLocal, Func<TSource, ParallelLoopState, long, TLocal, TLocal> bodyWithEverything, Func<TLocal> localInit, Action<TLocal> localFinally)
		{
			if (parallelOptions.CancellationToken.IsCancellationRequested)
			{
				throw new OperationCanceledException2(parallelOptions.CancellationToken);
			}
			TSource[] array = source as TSource[];
			if (array != null)
			{
				return ForEachWorker(array, parallelOptions, body, bodyWithState, bodyWithStateAndIndex, bodyWithStateAndLocal, bodyWithEverything, localInit, localFinally);
			}
			IList<TSource> list = source as IList<TSource>;
			if (list != null)
			{
				return ForEachWorker(list, parallelOptions, body, bodyWithState, bodyWithStateAndIndex, bodyWithStateAndLocal, bodyWithEverything, localInit, localFinally);
			}
			return PartitionerForEachWorker(Partitioner.Create(source), parallelOptions, body, bodyWithState, bodyWithStateAndIndex, bodyWithStateAndLocal, bodyWithEverything, localInit, localFinally);
		}

		/// <summary>
		/// A fast path for the more general ForEachWorker method above. This uses ldelem instructions to
		/// access the individual elements of the array, which will be faster.
		/// </summary>
		/// <typeparam name="TSource">The type of the source data.</typeparam>
		/// <typeparam name="TLocal">The type of the local data.</typeparam>
		/// <param name="array">An array data source.</param>
		/// <param name="parallelOptions">The options to use for execution.</param>
		/// <param name="body">The simple loop body.</param>
		/// <param name="bodyWithState">The loop body for ParallelState overloads.</param>
		/// <param name="bodyWithStateAndIndex">The loop body for indexed/ParallelLoopState overloads.</param>
		/// <param name="bodyWithStateAndLocal">The loop body for local/ParallelLoopState overloads.</param>
		/// <param name="bodyWithEverything">The loop body for the most generic overload.</param>
		/// <param name="localInit">A selector function that returns new thread local state.</param>
		/// <param name="localFinally">A cleanup function to destroy thread local state.</param>
		/// <returns>A <see cref="T:System.Threading.Tasks.ParallelLoopResult" /> structure.</returns>
		private static ParallelLoopResult ForEachWorker<TSource, TLocal>(TSource[] array, ParallelOptions parallelOptions, Action<TSource> body, Action<TSource, ParallelLoopState> bodyWithState, Action<TSource, ParallelLoopState, long> bodyWithStateAndIndex, Func<TSource, ParallelLoopState, TLocal, TLocal> bodyWithStateAndLocal, Func<TSource, ParallelLoopState, long, TLocal, TLocal> bodyWithEverything, Func<TLocal> localInit, Action<TLocal> localFinally)
		{
			int lowerBound = array.GetLowerBound(0);
			int toExclusive = array.GetUpperBound(0) + 1;
			if (body != null)
			{
				return ForWorker<object>(lowerBound, toExclusive, parallelOptions, delegate(int i)
				{
					body(array[i]);
				}, null, null, null, null);
			}
			if (bodyWithState != null)
			{
				return ForWorker<object>(lowerBound, toExclusive, parallelOptions, null, delegate(int i, ParallelLoopState state)
				{
					bodyWithState(array[i], state);
				}, null, null, null);
			}
			if (bodyWithStateAndIndex != null)
			{
				return ForWorker<object>(lowerBound, toExclusive, parallelOptions, null, delegate(int i, ParallelLoopState state)
				{
					bodyWithStateAndIndex(array[i], state, i);
				}, null, null, null);
			}
			if (bodyWithStateAndLocal != null)
			{
				return ForWorker(lowerBound, toExclusive, parallelOptions, null, null, (int i, ParallelLoopState state, TLocal local) => bodyWithStateAndLocal(array[i], state, local), localInit, localFinally);
			}
			return ForWorker(lowerBound, toExclusive, parallelOptions, null, null, (int i, ParallelLoopState state, TLocal local) => bodyWithEverything(array[i], state, i, local), localInit, localFinally);
		}

		/// <summary>
		/// A fast path for the more general ForEachWorker method above. This uses IList&lt;T&gt;'s indexer
		/// capabilities to access the individual elements of the list rather than an enumerator.
		/// </summary>
		/// <typeparam name="TSource">The type of the source data.</typeparam>
		/// <typeparam name="TLocal">The type of the local data.</typeparam>
		/// <param name="list">A list data source.</param>
		/// <param name="parallelOptions">The options to use for execution.</param>
		/// <param name="body">The simple loop body.</param>
		/// <param name="bodyWithState">The loop body for ParallelState overloads.</param>
		/// <param name="bodyWithStateAndIndex">The loop body for indexed/ParallelLoopState overloads.</param>
		/// <param name="bodyWithStateAndLocal">The loop body for local/ParallelLoopState overloads.</param>
		/// <param name="bodyWithEverything">The loop body for the most generic overload.</param>
		/// <param name="localInit">A selector function that returns new thread local state.</param>
		/// <param name="localFinally">A cleanup function to destroy thread local state.</param>
		/// <returns>A <see cref="T:System.Threading.Tasks.ParallelLoopResult" /> structure.</returns>
		private static ParallelLoopResult ForEachWorker<TSource, TLocal>(IList<TSource> list, ParallelOptions parallelOptions, Action<TSource> body, Action<TSource, ParallelLoopState> bodyWithState, Action<TSource, ParallelLoopState, long> bodyWithStateAndIndex, Func<TSource, ParallelLoopState, TLocal, TLocal> bodyWithStateAndLocal, Func<TSource, ParallelLoopState, long, TLocal, TLocal> bodyWithEverything, Func<TLocal> localInit, Action<TLocal> localFinally)
		{
			if (body != null)
			{
				return ForWorker<object>(0, list.Count, parallelOptions, delegate(int i)
				{
					body(list[i]);
				}, null, null, null, null);
			}
			if (bodyWithState != null)
			{
				return ForWorker<object>(0, list.Count, parallelOptions, null, delegate(int i, ParallelLoopState state)
				{
					bodyWithState(list[i], state);
				}, null, null, null);
			}
			if (bodyWithStateAndIndex != null)
			{
				return ForWorker<object>(0, list.Count, parallelOptions, null, delegate(int i, ParallelLoopState state)
				{
					bodyWithStateAndIndex(list[i], state, i);
				}, null, null, null);
			}
			if (bodyWithStateAndLocal != null)
			{
				return ForWorker(0, list.Count, parallelOptions, null, null, (int i, ParallelLoopState state, TLocal local) => bodyWithStateAndLocal(list[i], state, local), localInit, localFinally);
			}
			return ForWorker(0, list.Count, parallelOptions, null, null, (int i, ParallelLoopState state, TLocal local) => bodyWithEverything(list[i], state, i, local), localInit, localFinally);
		}

		/// <summary>
		/// Executes a for each operation on a <see cref="T:System.Collections.Concurrent.Partitioner{TSource}">
		/// Partitioner</see> in which iterations may run in parallel.
		/// </summary>
		/// <typeparam name="TSource">The type of the elements in <paramref name="source" />.</typeparam>
		/// <param name="source">The Partitioner that contains the original data source.</param>
		/// <param name="body">The delegate that is invoked once per iteration.</param>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="source" /> 
		/// argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="body" /> 
		/// argument is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The exception that is thrown when the 
		/// SupportsDynamicPartitions property in the <paramref name="source" /> Partitioner returns 
		/// false.</exception>
		/// <exception cref="T:System.InvalidOperationException">The exception that is thrown when any 
		/// methods in the <paramref name="source" /> Partitioner return null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The exception that is thrown when the 
		/// GetPartitions() method in the <paramref name="source" /> Partitioner does not return 
		/// the correct number of partitions.</exception>
		/// <exception cref="T:System.InvalidOperationException">The exception that is thrown when the 
		/// GetPartitions() method in the <paramref name="source" /> Partitioner returns an IList 
		/// with at least one null value.</exception>
		/// <exception cref="T:System.InvalidOperationException">The exception that is thrown when the 
		/// GetDynamicPartitions() method in the <paramref name="source" /> Partitioner returns an 
		/// IEnumerable whose GetEnumerator() method returns null.</exception>
		/// <exception cref="T:System.AggregateException">The exception that is thrown to contain an exception
		/// thrown from one of the specified delegates.</exception>
		/// <returns>A <see cref="T:System.Threading.Tasks.ParallelLoopResult">ParallelLoopResult</see> structure
		/// that contains information on what portion of the loop completed.</returns>
		/// <remarks>
		/// <para>
		/// The <see cref="T:System.Collections.Concurrent.Partitioner{TSource}">Partitioner</see> is used to retrieve 
		/// the elements to be processed, in place of the original data source.  If the current element's 
		/// index is desired, the source must be an <see cref="T:System.Collections.Concurrent.OrderablePartitioner">
		/// OrderablePartitioner</see>.
		/// </para>
		/// <para>
		/// The <paramref name="body" /> delegate is invoked once for each element in the <paramref name="source" /> 
		/// Partitioner.  It is provided with the current element as a parameter.
		/// </para>
		/// </remarks>
		public static ParallelLoopResult ForEach<TSource>(Partitioner<TSource> source, Action<TSource> body)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (body == null)
			{
				throw new ArgumentNullException("body");
			}
			return PartitionerForEachWorker<TSource, object>(source, s_defaultParallelOptions, body, null, null, null, null, null, null);
		}

		/// <summary>
		/// Executes a for each operation on a <see cref="T:System.Collections.Concurrent.Partitioner{TSource}">
		/// Partitioner</see> in which iterations may run in parallel.
		/// </summary>
		/// <typeparam name="TSource">The type of the elements in <paramref name="source" />.</typeparam>
		/// <param name="source">The Partitioner that contains the original data source.</param>
		/// <param name="body">The delegate that is invoked once per iteration.</param>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="source" /> 
		/// argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="body" /> 
		/// argument is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The exception that is thrown when the 
		/// SupportsDynamicPartitions property in the <paramref name="source" /> Partitioner returns 
		/// false.</exception>
		/// <exception cref="T:System.InvalidOperationException">The exception that is thrown when any 
		/// methods in the <paramref name="source" /> Partitioner return null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The exception that is thrown when the 
		/// GetPartitions() method in the <paramref name="source" /> Partitioner does not return 
		/// the correct number of partitions.</exception>
		/// <exception cref="T:System.InvalidOperationException">The exception that is thrown when the 
		/// GetPartitions() method in the <paramref name="source" /> Partitioner returns an IList 
		/// with at least one null value.</exception>
		/// <exception cref="T:System.InvalidOperationException">The exception that is thrown when the 
		/// GetDynamicPartitions() method in the <paramref name="source" /> Partitioner returns an 
		/// IEnumerable whose GetEnumerator() method returns null.</exception>
		/// <exception cref="T:System.AggregateException">The exception that is thrown to contain an exception
		/// thrown from one of the specified delegates.</exception>
		/// <returns>A <see cref="T:System.Threading.Tasks.ParallelLoopResult">ParallelLoopResult</see> structure
		/// that contains information on what portion of the loop completed.</returns>
		/// <remarks>
		/// <para>
		/// The <see cref="T:System.Collections.Concurrent.Partitioner{TSource}">Partitioner</see> is used to retrieve 
		/// the elements to be processed, in place of the original data source.  If the current element's 
		/// index is desired, the source must be an <see cref="T:System.Collections.Concurrent.OrderablePartitioner">
		/// OrderablePartitioner</see>.
		/// </para>
		/// <para>
		/// The <paramref name="body" /> delegate is invoked once for each element in the <paramref name="source" /> 
		/// Partitioner.  It is provided with the following parameters: the current element, 
		/// and a <see cref="T:System.Threading.Tasks.ParallelLoopState">ParallelLoopState</see> instance that may be 
		/// used to break out of the loop prematurely.
		/// </para>
		/// </remarks>
		public static ParallelLoopResult ForEach<TSource>(Partitioner<TSource> source, Action<TSource, ParallelLoopState> body)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (body == null)
			{
				throw new ArgumentNullException("body");
			}
			return PartitionerForEachWorker<TSource, object>(source, s_defaultParallelOptions, null, body, null, null, null, null, null);
		}

		/// <summary>
		/// Executes a for each operation on a <see cref="T:System.Collections.Concurrent.OrderablePartitioner{TSource}">
		/// OrderablePartitioner</see> in which iterations may run in parallel.
		/// </summary>
		/// <typeparam name="TSource">The type of the elements in <paramref name="source" />.</typeparam>
		/// <param name="source">The OrderablePartitioner that contains the original data source.</param>
		/// <param name="body">The delegate that is invoked once per iteration.</param>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="source" /> 
		/// argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="body" /> 
		/// argument is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The exception that is thrown when the 
		/// SupportsDynamicPartitions property in the <paramref name="source" /> OrderablePartitioner returns 
		/// false.</exception>
		/// <exception cref="T:System.InvalidOperationException">The exception that is thrown when the 
		/// KeysNormalized property in the <paramref name="source" /> OrderablePartitioner returns 
		/// false.</exception>
		/// <exception cref="T:System.InvalidOperationException">The exception that is thrown when any 
		/// methods in the <paramref name="source" /> OrderablePartitioner return null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The exception that is thrown when the 
		/// GetPartitions() or GetOrderablePartitions() methods in the <paramref name="source" /> 
		/// OrderablePartitioner do not return the correct number of partitions.</exception>
		/// <exception cref="T:System.InvalidOperationException">The exception that is thrown when the 
		/// GetPartitions() or GetOrderablePartitions() methods in the <paramref name="source" /> 
		/// OrderablePartitioner return an IList with at least one null value.</exception>
		/// <exception cref="T:System.InvalidOperationException">The exception that is thrown when the 
		/// GetDynamicPartitions() or GetDynamicOrderablePartitions() methods in the <paramref name="source" /> 
		/// OrderablePartitioner return an IEnumerable whose GetEnumerator() method returns null.</exception>
		/// <exception cref="T:System.AggregateException">The exception that is thrown to contain an exception
		/// thrown from one of the specified delegates.</exception>
		/// <returns>A <see cref="T:System.Threading.Tasks.ParallelLoopResult">ParallelLoopResult</see> structure
		/// that contains information on what portion of the loop completed.</returns>
		/// <remarks>
		/// <para>
		/// The <see cref="T:System.Collections.Concurrent.Partitioner{TSource}">Partitioner</see> is used to retrieve 
		/// the elements to be processed, in place of the original data source.  If the current element's 
		/// index is desired, the source must be an <see cref="T:System.Collections.Concurrent.OrderablePartitioner">
		/// OrderablePartitioner</see>.
		/// </para>
		/// <para>
		/// The <paramref name="body" /> delegate is invoked once for each element in the <paramref name="source" /> 
		/// Partitioner.  It is provided with the following parameters: the current element, 
		/// a <see cref="T:System.Threading.Tasks.ParallelLoopState">ParallelLoopState</see> instance that may be 
		/// used to break out of the loop prematurely, and the current element's index (an Int64).
		/// </para>
		/// </remarks>
		public static ParallelLoopResult ForEach<TSource>(OrderablePartitioner<TSource> source, Action<TSource, ParallelLoopState, long> body)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (body == null)
			{
				throw new ArgumentNullException("body");
			}
			if (!source.KeysNormalized)
			{
				throw new InvalidOperationException(Environment2.GetResourceString("Parallel_ForEach_OrderedPartitionerKeysNotNormalized"));
			}
			return PartitionerForEachWorker<TSource, object>(source, s_defaultParallelOptions, null, null, body, null, null, null, null);
		}

		/// <summary>
		/// Executes a for each operation on a <see cref="T:System.Collections.Concurrent.Partitioner{TSource}">
		/// Partitioner</see> in which iterations may run in parallel.
		/// </summary>
		/// <typeparam name="TSource">The type of the elements in <paramref name="source" />.</typeparam>
		/// <typeparam name="TLocal">The type of the thread-local data.</typeparam>
		/// <param name="source">The Partitioner that contains the original data source.</param>
		/// <param name="localInit">The function delegate that returns the initial state of the local data 
		/// for each thread.</param>
		/// <param name="body">The delegate that is invoked once per iteration.</param>
		/// <param name="localFinally">The delegate that performs a final action on the local state of each
		/// thread.</param>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="source" /> 
		/// argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="body" /> 
		/// argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the 
		/// <paramref name="localInit" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the 
		/// <paramref name="localFinally" /> argument is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The exception that is thrown when the 
		/// SupportsDynamicPartitions property in the <paramref name="source" /> Partitioner returns 
		/// false.</exception>
		/// <exception cref="T:System.InvalidOperationException">The exception that is thrown when any 
		/// methods in the <paramref name="source" /> Partitioner return null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The exception that is thrown when the 
		/// GetPartitions() method in the <paramref name="source" /> Partitioner does not return 
		/// the correct number of partitions.</exception>
		/// <exception cref="T:System.InvalidOperationException">The exception that is thrown when the 
		/// GetPartitions() method in the <paramref name="source" /> Partitioner returns an IList 
		/// with at least one null value.</exception>
		/// <exception cref="T:System.InvalidOperationException">The exception that is thrown when the 
		/// GetDynamicPartitions() method in the <paramref name="source" /> Partitioner returns an 
		/// IEnumerable whose GetEnumerator() method returns null.</exception>
		/// <exception cref="T:System.AggregateException">The exception that is thrown to contain an exception
		/// thrown from one of the specified delegates.</exception>
		/// <returns>A <see cref="T:System.Threading.Tasks.ParallelLoopResult">ParallelLoopResult</see> structure
		/// that contains information on what portion of the loop completed.</returns>
		/// <remarks>
		/// <para>
		/// The <see cref="T:System.Collections.Concurrent.Partitioner{TSource}">Partitioner</see> is used to retrieve 
		/// the elements to be processed, in place of the original data source.  If the current element's 
		/// index is desired, the source must be an <see cref="T:System.Collections.Concurrent.OrderablePartitioner">
		/// OrderablePartitioner</see>.
		/// </para>
		/// <para>
		/// The <paramref name="body" /> delegate is invoked once for each element in the <paramref name="source" /> 
		/// Partitioner.  It is provided with the following parameters: the current element, 
		/// a <see cref="T:System.Threading.Tasks.ParallelLoopState">ParallelLoopState</see> instance that may be 
		/// used to break out of the loop prematurely, and some local state that may be shared amongst iterations 
		/// that execute on the same thread.
		/// </para>
		/// <para>
		/// The <paramref name="localInit" /> delegate is invoked once for each thread that participates in the loop's 
		/// execution and returns the initial local state for each of those threads.  These initial states are passed to the first
		/// <paramref name="body" /> invocations on each thread.  Then, every subsequent body invocation returns a possibly 
		/// modified state value that is passed to the next body invocation.  Finally, the last body invocation on each thread returns a state value 
		/// that is passed to the <paramref name="localFinally" /> delegate.  The localFinally delegate is invoked once per thread to perform a final 
		/// action on each thread's local state.
		/// </para>
		/// </remarks>
		public static ParallelLoopResult ForEach<TSource, TLocal>(Partitioner<TSource> source, Func<TLocal> localInit, Func<TSource, ParallelLoopState, TLocal, TLocal> body, Action<TLocal> localFinally)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (body == null)
			{
				throw new ArgumentNullException("body");
			}
			if (localInit == null)
			{
				throw new ArgumentNullException("localInit");
			}
			if (localFinally == null)
			{
				throw new ArgumentNullException("localFinally");
			}
			return PartitionerForEachWorker(source, s_defaultParallelOptions, null, null, null, body, null, localInit, localFinally);
		}

		/// <summary>
		/// Executes a for each operation on a <see cref="T:System.Collections.Concurrent.OrderablePartitioner{TSource}">
		/// OrderablePartitioner</see> in which iterations may run in parallel.
		/// </summary>
		/// <typeparam name="TSource">The type of the elements in <paramref name="source" />.</typeparam>
		/// <typeparam name="TLocal">The type of the thread-local data.</typeparam>
		/// <param name="source">The OrderablePartitioner that contains the original data source.</param>
		/// <param name="localInit">The function delegate that returns the initial state of the local data 
		/// for each thread.</param>
		/// <param name="body">The delegate that is invoked once per iteration.</param>
		/// <param name="localFinally">The delegate that performs a final action on the local state of each
		/// thread.</param>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="source" /> 
		/// argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="body" /> 
		/// argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the 
		/// <paramref name="localInit" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the 
		/// <paramref name="localFinally" /> argument is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The exception that is thrown when the 
		/// SupportsDynamicPartitions property in the <paramref name="source" /> OrderablePartitioner returns 
		/// false.</exception>
		/// <exception cref="T:System.InvalidOperationException">The exception that is thrown when the 
		/// KeysNormalized property in the <paramref name="source" /> OrderablePartitioner returns 
		/// false.</exception>
		/// <exception cref="T:System.InvalidOperationException">The exception that is thrown when any 
		/// methods in the <paramref name="source" /> OrderablePartitioner return null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The exception that is thrown when the 
		/// GetPartitions() or GetOrderablePartitions() methods in the <paramref name="source" /> 
		/// OrderablePartitioner do not return the correct number of partitions.</exception>
		/// <exception cref="T:System.InvalidOperationException">The exception that is thrown when the 
		/// GetPartitions() or GetOrderablePartitions() methods in the <paramref name="source" /> 
		/// OrderablePartitioner return an IList with at least one null value.</exception>
		/// <exception cref="T:System.InvalidOperationException">The exception that is thrown when the 
		/// GetDynamicPartitions() or GetDynamicOrderablePartitions() methods in the <paramref name="source" /> 
		/// OrderablePartitioner return an IEnumerable whose GetEnumerator() method returns null.</exception>
		/// <exception cref="T:System.AggregateException">The exception that is thrown to contain an exception
		/// thrown from one of the specified delegates.</exception>
		/// <returns>A <see cref="T:System.Threading.Tasks.ParallelLoopResult">ParallelLoopResult</see> structure
		/// that contains information on what portion of the loop completed.</returns>
		/// <remarks>
		/// <para>
		/// The <see cref="T:System.Collections.Concurrent.Partitioner{TSource}">Partitioner</see> is used to retrieve 
		/// the elements to be processed, in place of the original data source.  If the current element's 
		/// index is desired, the source must be an <see cref="T:System.Collections.Concurrent.OrderablePartitioner">
		/// OrderablePartitioner</see>.
		/// </para>
		/// <para>
		/// The <paramref name="body" /> delegate is invoked once for each element in the <paramref name="source" /> 
		/// Partitioner.  It is provided with the following parameters: the current element, 
		/// a <see cref="T:System.Threading.Tasks.ParallelLoopState">ParallelLoopState</see> instance that may be 
		/// used to break out of the loop prematurely, the current element's index (an Int64), and some local 
		/// state that may be shared amongst iterations that execute on the same thread.
		/// </para>
		/// <para>
		/// The <paramref name="localInit" /> delegate is invoked once for each thread that participates in the loop's 
		/// execution and returns the initial local state for each of those threads.  These initial states are passed to the first
		/// <paramref name="body" /> invocations on each thread.  Then, every subsequent body invocation returns a possibly 
		/// modified state value that is passed to the next body invocation.  Finally, the last body invocation on each thread returns a state value 
		/// that is passed to the <paramref name="localFinally" /> delegate.  The localFinally delegate is invoked once per thread to perform a final 
		/// action on each thread's local state.
		/// </para>
		/// </remarks>
		public static ParallelLoopResult ForEach<TSource, TLocal>(OrderablePartitioner<TSource> source, Func<TLocal> localInit, Func<TSource, ParallelLoopState, long, TLocal, TLocal> body, Action<TLocal> localFinally)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (body == null)
			{
				throw new ArgumentNullException("body");
			}
			if (localInit == null)
			{
				throw new ArgumentNullException("localInit");
			}
			if (localFinally == null)
			{
				throw new ArgumentNullException("localFinally");
			}
			if (!source.KeysNormalized)
			{
				throw new InvalidOperationException(Environment2.GetResourceString("Parallel_ForEach_OrderedPartitionerKeysNotNormalized"));
			}
			return PartitionerForEachWorker(source, s_defaultParallelOptions, null, null, null, null, body, localInit, localFinally);
		}

		/// <summary>
		/// Executes a for each operation on a <see cref="T:System.Collections.Concurrent.Partitioner{TSource}">
		/// Partitioner</see> in which iterations may run in parallel.
		/// </summary>
		/// <typeparam name="TSource">The type of the elements in <paramref name="source" />.</typeparam>
		/// <param name="source">The Partitioner that contains the original data source.</param>
		/// <param name="parallelOptions">A <see cref="T:System.Threading.Tasks.ParallelOptions">ParallelOptions</see> 
		/// instance that configures the behavior of this operation.</param>
		/// <param name="body">The delegate that is invoked once per iteration.</param>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="source" /> 
		/// argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the 
		/// <paramref name="parallelOptions" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="body" /> 
		/// argument is null.</exception>
		/// <exception cref="T:System.OperationCanceledException">The exception that is thrown when the 
		/// <see cref="T:System.Threading.CancellationToken">CancellationToken</see> in the <paramref name="parallelOptions" /> 
		/// argument is set</exception>
		/// <exception cref="T:System.InvalidOperationException">The exception that is thrown when the 
		/// SupportsDynamicPartitions property in the <paramref name="source" /> Partitioner returns 
		/// false.</exception>
		/// <exception cref="T:System.InvalidOperationException">The exception that is thrown when any 
		/// methods in the <paramref name="source" /> Partitioner return null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The exception that is thrown when the 
		/// GetPartitions() method in the <paramref name="source" /> Partitioner does not return 
		/// the correct number of partitions.</exception>
		/// <exception cref="T:System.InvalidOperationException">The exception that is thrown when the 
		/// GetPartitions() method in the <paramref name="source" /> Partitioner returns an IList 
		/// with at least one null value.</exception>
		/// <exception cref="T:System.InvalidOperationException">The exception that is thrown when the 
		/// GetDynamicPartitions() method in the <paramref name="source" /> Partitioner returns an 
		/// IEnumerable whose GetEnumerator() method returns null.</exception>
		/// <exception cref="T:System.AggregateException">The exception that is thrown to contain an exception
		/// thrown from one of the specified delegates.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The exception that is thrown when the 
		/// the <see cref="T:System.Threading.CancellationTokenSource">CancellationTokenSource</see> associated with the 
		/// the <see cref="T:System.Threading.CancellationToken">CancellationToken</see> in the 
		/// <paramref name="parallelOptions" /> has been disposed.</exception>
		/// <returns>A <see cref="T:System.Threading.Tasks.ParallelLoopResult">ParallelLoopResult</see> structure
		/// that contains information on what portion of the loop completed.</returns>
		/// <remarks>
		/// <para>
		/// The <see cref="T:System.Collections.Concurrent.Partitioner{TSource}">Partitioner</see> is used to retrieve 
		/// the elements to be processed, in place of the original data source.  If the current element's 
		/// index is desired, the source must be an <see cref="T:System.Collections.Concurrent.OrderablePartitioner">
		/// OrderablePartitioner</see>.
		/// </para>
		/// <para>
		/// The <paramref name="body" /> delegate is invoked once for each element in the <paramref name="source" /> 
		/// Partitioner.  It is provided with the current element as a parameter.
		/// </para>    
		/// </remarks>
		public static ParallelLoopResult ForEach<TSource>(Partitioner<TSource> source, ParallelOptions parallelOptions, Action<TSource> body)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (body == null)
			{
				throw new ArgumentNullException("body");
			}
			if (parallelOptions == null)
			{
				throw new ArgumentNullException("parallelOptions");
			}
			return PartitionerForEachWorker<TSource, object>(source, parallelOptions, body, null, null, null, null, null, null);
		}

		/// <summary>
		/// Executes a for each operation on a <see cref="T:System.Collections.Concurrent.Partitioner{TSource}">
		/// Partitioner</see> in which iterations may run in parallel.
		/// </summary>
		/// <typeparam name="TSource">The type of the elements in <paramref name="source" />.</typeparam>
		/// <param name="source">The Partitioner that contains the original data source.</param>
		/// <param name="parallelOptions">A <see cref="T:System.Threading.Tasks.ParallelOptions">ParallelOptions</see> 
		/// instance that configures the behavior of this operation.</param>
		/// <param name="body">The delegate that is invoked once per iteration.</param>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="source" /> 
		/// argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the 
		/// <paramref name="parallelOptions" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="body" /> 
		/// argument is null.</exception>
		/// <exception cref="T:System.OperationCanceledException">The exception that is thrown when the 
		/// <see cref="T:System.Threading.CancellationToken">CancellationToken</see> in the <paramref name="parallelOptions" /> 
		/// argument is set</exception>
		/// <exception cref="T:System.InvalidOperationException">The exception that is thrown when the 
		/// SupportsDynamicPartitions property in the <paramref name="source" /> Partitioner returns 
		/// false.</exception>
		/// <exception cref="T:System.InvalidOperationException">The exception that is thrown when any 
		/// methods in the <paramref name="source" /> Partitioner return null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The exception that is thrown when the 
		/// GetPartitions() method in the <paramref name="source" /> Partitioner does not return 
		/// the correct number of partitions.</exception>
		/// <exception cref="T:System.InvalidOperationException">The exception that is thrown when the 
		/// GetPartitions() method in the <paramref name="source" /> Partitioner returns an IList 
		/// with at least one null value.</exception>
		/// <exception cref="T:System.InvalidOperationException">The exception that is thrown when the 
		/// GetDynamicPartitions() method in the <paramref name="source" /> Partitioner returns an 
		/// IEnumerable whose GetEnumerator() method returns null.</exception>
		/// <exception cref="T:System.AggregateException">The exception that is thrown to contain an exception
		/// thrown from one of the specified delegates.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The exception that is thrown when the 
		/// the <see cref="T:System.Threading.CancellationTokenSource">CancellationTokenSource</see> associated with the 
		/// the <see cref="T:System.Threading.CancellationToken">CancellationToken</see> in the 
		/// <paramref name="parallelOptions" /> has been disposed.</exception>
		/// <returns>A <see cref="T:System.Threading.Tasks.ParallelLoopResult">ParallelLoopResult</see> structure
		/// that contains information on what portion of the loop completed.</returns>
		/// <remarks>
		/// <para>
		/// The <see cref="T:System.Collections.Concurrent.Partitioner{TSource}">Partitioner</see> is used to retrieve 
		/// the elements to be processed, in place of the original data source.  If the current element's 
		/// index is desired, the source must be an <see cref="T:System.Collections.Concurrent.OrderablePartitioner">
		/// OrderablePartitioner</see>.
		/// </para>
		/// <para>
		/// The <paramref name="body" /> delegate is invoked once for each element in the <paramref name="source" /> 
		/// Partitioner.  It is provided with the following parameters: the current element, 
		/// and a <see cref="T:System.Threading.Tasks.ParallelLoopState">ParallelLoopState</see> instance that may be 
		/// used to break out of the loop prematurely.
		/// </para>  
		/// </remarks>
		public static ParallelLoopResult ForEach<TSource>(Partitioner<TSource> source, ParallelOptions parallelOptions, Action<TSource, ParallelLoopState> body)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (body == null)
			{
				throw new ArgumentNullException("body");
			}
			if (parallelOptions == null)
			{
				throw new ArgumentNullException("parallelOptions");
			}
			return PartitionerForEachWorker<TSource, object>(source, parallelOptions, null, body, null, null, null, null, null);
		}

		/// <summary>
		/// Executes a for each operation on a <see cref="T:System.Collections.Concurrent.OrderablePartitioner{TSource}">
		/// OrderablePartitioner</see> in which iterations may run in parallel.
		/// </summary>
		/// <typeparam name="TSource">The type of the elements in <paramref name="source" />.</typeparam>
		/// <param name="source">The OrderablePartitioner that contains the original data source.</param>
		/// <param name="parallelOptions">A <see cref="T:System.Threading.Tasks.ParallelOptions">ParallelOptions</see> 
		/// instance that configures the behavior of this operation.</param>
		/// <param name="body">The delegate that is invoked once per iteration.</param>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="source" /> 
		/// argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the 
		/// <paramref name="parallelOptions" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="body" /> 
		/// argument is null.</exception>
		/// <exception cref="T:System.OperationCanceledException">The exception that is thrown when the 
		/// <see cref="T:System.Threading.CancellationToken">CancellationToken</see> in the <paramref name="parallelOptions" /> 
		/// argument is set</exception>
		/// <exception cref="T:System.InvalidOperationException">The exception that is thrown when the 
		/// SupportsDynamicPartitions property in the <paramref name="source" /> OrderablePartitioner returns 
		/// false.</exception>
		/// <exception cref="T:System.InvalidOperationException">The exception that is thrown when the 
		/// KeysNormalized property in the <paramref name="source" /> OrderablePartitioner returns 
		/// false.</exception>
		/// <exception cref="T:System.InvalidOperationException">The exception that is thrown when any 
		/// methods in the <paramref name="source" /> OrderablePartitioner return null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The exception that is thrown when the 
		/// GetPartitions() or GetOrderablePartitions() methods in the <paramref name="source" /> 
		/// OrderablePartitioner do not return the correct number of partitions.</exception>
		/// <exception cref="T:System.InvalidOperationException">The exception that is thrown when the 
		/// GetPartitions() or GetOrderablePartitions() methods in the <paramref name="source" /> 
		/// OrderablePartitioner return an IList with at least one null value.</exception>
		/// <exception cref="T:System.InvalidOperationException">The exception that is thrown when the 
		/// GetDynamicPartitions() or GetDynamicOrderablePartitions() methods in the <paramref name="source" /> 
		/// OrderablePartitioner return an IEnumerable whose GetEnumerator() method returns null.</exception>
		/// <exception cref="T:System.AggregateException">The exception that is thrown to contain an exception
		/// thrown from one of the specified delegates.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The exception that is thrown when the 
		/// the <see cref="T:System.Threading.CancellationTokenSource">CancellationTokenSource</see> associated with the 
		/// the <see cref="T:System.Threading.CancellationToken">CancellationToken</see> in the 
		/// <paramref name="parallelOptions" /> has been disposed.</exception>
		/// <returns>A <see cref="T:System.Threading.Tasks.ParallelLoopResult">ParallelLoopResult</see> structure
		/// that contains information on what portion of the loop completed.</returns>
		/// <remarks>
		/// <para>
		/// The <see cref="T:System.Collections.Concurrent.Partitioner{TSource}">Partitioner</see> is used to retrieve 
		/// the elements to be processed, in place of the original data source.  If the current element's 
		/// index is desired, the source must be an <see cref="T:System.Collections.Concurrent.OrderablePartitioner">
		/// OrderablePartitioner</see>.
		/// </para>
		/// <para>
		/// The <paramref name="body" /> delegate is invoked once for each element in the <paramref name="source" /> 
		/// Partitioner.  It is provided with the following parameters: the current element, 
		/// a <see cref="T:System.Threading.Tasks.ParallelLoopState">ParallelLoopState</see> instance that may be 
		/// used to break out of the loop prematurely, and the current element's index (an Int64).
		/// </para>
		/// </remarks>
		public static ParallelLoopResult ForEach<TSource>(OrderablePartitioner<TSource> source, ParallelOptions parallelOptions, Action<TSource, ParallelLoopState, long> body)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (body == null)
			{
				throw new ArgumentNullException("body");
			}
			if (parallelOptions == null)
			{
				throw new ArgumentNullException("parallelOptions");
			}
			if (!source.KeysNormalized)
			{
				throw new InvalidOperationException(Environment2.GetResourceString("Parallel_ForEach_OrderedPartitionerKeysNotNormalized"));
			}
			return PartitionerForEachWorker<TSource, object>(source, parallelOptions, null, null, body, null, null, null, null);
		}

		/// <summary>
		/// Executes a for each operation on a <see cref="T:System.Collections.Concurrent.Partitioner{TSource}">
		/// Partitioner</see> in which iterations may run in parallel.
		/// </summary>
		/// <typeparam name="TSource">The type of the elements in <paramref name="source" />.</typeparam>
		/// <typeparam name="TLocal">The type of the thread-local data.</typeparam>
		/// <param name="source">The Partitioner that contains the original data source.</param>
		/// <param name="parallelOptions">A <see cref="T:System.Threading.Tasks.ParallelOptions">ParallelOptions</see> 
		/// instance that configures the behavior of this operation.</param>
		/// <param name="localInit">The function delegate that returns the initial state of the local data 
		/// for each thread.</param>
		/// <param name="body">The delegate that is invoked once per iteration.</param>
		/// <param name="localFinally">The delegate that performs a final action on the local state of each
		/// thread.</param>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="source" /> 
		/// argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the 
		/// <paramref name="parallelOptions" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="body" /> 
		/// argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the 
		/// <paramref name="localInit" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the 
		/// <paramref name="localFinally" /> argument is null.</exception>
		/// <exception cref="T:System.OperationCanceledException">The exception that is thrown when the 
		/// <see cref="T:System.Threading.CancellationToken">CancellationToken</see> in the <paramref name="parallelOptions" /> 
		/// argument is set</exception>
		/// <exception cref="T:System.InvalidOperationException">The exception that is thrown when the 
		/// SupportsDynamicPartitions property in the <paramref name="source" /> Partitioner returns 
		/// false.</exception>
		/// <exception cref="T:System.InvalidOperationException">The exception that is thrown when any 
		/// methods in the <paramref name="source" /> Partitioner return null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The exception that is thrown when the 
		/// GetPartitions() method in the <paramref name="source" /> Partitioner does not return 
		/// the correct number of partitions.</exception>
		/// <exception cref="T:System.InvalidOperationException">The exception that is thrown when the 
		/// GetPartitions() method in the <paramref name="source" /> Partitioner returns an IList 
		/// with at least one null value.</exception>
		/// <exception cref="T:System.InvalidOperationException">The exception that is thrown when the 
		/// GetDynamicPartitions() method in the <paramref name="source" /> Partitioner returns an 
		/// IEnumerable whose GetEnumerator() method returns null.</exception>
		/// <exception cref="T:System.AggregateException">The exception that is thrown to contain an exception
		/// thrown from one of the specified delegates.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The exception that is thrown when the 
		/// the <see cref="T:System.Threading.CancellationTokenSource">CancellationTokenSource</see> associated with the 
		/// the <see cref="T:System.Threading.CancellationToken">CancellationToken</see> in the 
		/// <paramref name="parallelOptions" /> has been disposed.</exception>
		/// <returns>A <see cref="T:System.Threading.Tasks.ParallelLoopResult">ParallelLoopResult</see> structure
		/// that contains information on what portion of the loop completed.</returns>
		/// <remarks>
		/// <para>
		/// The <see cref="T:System.Collections.Concurrent.Partitioner{TSource}">Partitioner</see> is used to retrieve 
		/// the elements to be processed, in place of the original data source.  If the current element's 
		/// index is desired, the source must be an <see cref="T:System.Collections.Concurrent.OrderablePartitioner">
		/// OrderablePartitioner</see>.
		/// </para>
		/// <para>
		/// The <paramref name="body" /> delegate is invoked once for each element in the <paramref name="source" /> 
		/// Partitioner.  It is provided with the following parameters: the current element, 
		/// a <see cref="T:System.Threading.Tasks.ParallelLoopState">ParallelLoopState</see> instance that may be 
		/// used to break out of the loop prematurely, and some local state that may be shared amongst iterations 
		/// that execute on the same thread.
		/// </para>
		/// <para>
		/// The <paramref name="localInit" /> delegate is invoked once for each thread that participates in the loop's 
		/// execution and returns the initial local state for each of those threads.  These initial states are passed to the first
		/// <paramref name="body" /> invocations on each thread.  Then, every subsequent body invocation returns a possibly 
		/// modified state value that is passed to the next body invocation.  Finally, the last body invocation on each thread returns a state value 
		/// that is passed to the <paramref name="localFinally" /> delegate.  The localFinally delegate is invoked once per thread to perform a final 
		/// action on each thread's local state.
		/// </para>
		/// </remarks>
		public static ParallelLoopResult ForEach<TSource, TLocal>(Partitioner<TSource> source, ParallelOptions parallelOptions, Func<TLocal> localInit, Func<TSource, ParallelLoopState, TLocal, TLocal> body, Action<TLocal> localFinally)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (body == null)
			{
				throw new ArgumentNullException("body");
			}
			if (localInit == null)
			{
				throw new ArgumentNullException("localInit");
			}
			if (localFinally == null)
			{
				throw new ArgumentNullException("localFinally");
			}
			if (parallelOptions == null)
			{
				throw new ArgumentNullException("parallelOptions");
			}
			return PartitionerForEachWorker(source, parallelOptions, null, null, null, body, null, localInit, localFinally);
		}

		/// <summary>
		/// Executes a for each operation on a <see cref="T:System.Collections.Concurrent.OrderablePartitioner{TSource}">
		/// OrderablePartitioner</see> in which iterations may run in parallel.
		/// </summary>
		/// <typeparam name="TSource">The type of the elements in <paramref name="source" />.</typeparam>
		/// <typeparam name="TLocal">The type of the thread-local data.</typeparam>
		/// <param name="source">The OrderablePartitioner that contains the original data source.</param>
		/// <param name="parallelOptions">A <see cref="T:System.Threading.Tasks.ParallelOptions">ParallelOptions</see> 
		/// instance that configures the behavior of this operation.</param>
		/// <param name="localInit">The function delegate that returns the initial state of the local data 
		/// for each thread.</param>
		/// <param name="body">The delegate that is invoked once per iteration.</param>
		/// <param name="localFinally">The delegate that performs a final action on the local state of each
		/// thread.</param>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="source" /> 
		/// argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the 
		/// <paramref name="parallelOptions" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the <paramref name="body" /> 
		/// argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the 
		/// <paramref name="localInit" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown when the 
		/// <paramref name="localFinally" /> argument is null.</exception>
		/// <exception cref="T:System.OperationCanceledException">The exception that is thrown when the 
		/// <see cref="T:System.Threading.CancellationToken">CancellationToken</see> in the <paramref name="parallelOptions" /> 
		/// argument is set</exception>
		/// <exception cref="T:System.InvalidOperationException">The exception that is thrown when the 
		/// SupportsDynamicPartitions property in the <paramref name="source" /> OrderablePartitioner returns 
		/// false.</exception>
		/// <exception cref="T:System.InvalidOperationException">The exception that is thrown when the 
		/// KeysNormalized property in the <paramref name="source" /> OrderablePartitioner returns 
		/// false.</exception>
		/// <exception cref="T:System.InvalidOperationException">The exception that is thrown when any 
		/// methods in the <paramref name="source" /> OrderablePartitioner return null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The exception that is thrown when the 
		/// GetPartitions() or GetOrderablePartitions() methods in the <paramref name="source" /> 
		/// OrderablePartitioner do not return the correct number of partitions.</exception>
		/// <exception cref="T:System.InvalidOperationException">The exception that is thrown when the 
		/// GetPartitions() or GetOrderablePartitions() methods in the <paramref name="source" /> 
		/// OrderablePartitioner return an IList with at least one null value.</exception>
		/// <exception cref="T:System.InvalidOperationException">The exception that is thrown when the 
		/// GetDynamicPartitions() or GetDynamicOrderablePartitions() methods in the <paramref name="source" /> 
		/// OrderablePartitioner return an IEnumerable whose GetEnumerator() method returns null.</exception>
		/// <exception cref="T:System.AggregateException">The exception that is thrown to contain an exception
		/// thrown from one of the specified delegates.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The exception that is thrown when the 
		/// the <see cref="T:System.Threading.CancellationTokenSource">CancellationTokenSource</see> associated with the 
		/// the <see cref="T:System.Threading.CancellationToken">CancellationToken</see> in the 
		/// <paramref name="parallelOptions" /> has been disposed.</exception>
		/// <returns>A <see cref="T:System.Threading.Tasks.ParallelLoopResult">ParallelLoopResult</see> structure
		/// that contains information on what portion of the loop completed.</returns>
		/// <remarks>
		/// <para>
		/// The <see cref="T:System.Collections.Concurrent.Partitioner{TSource}">Partitioner</see> is used to retrieve 
		/// the elements to be processed, in place of the original data source.  If the current element's 
		/// index is desired, the source must be an <see cref="T:System.Collections.Concurrent.OrderablePartitioner">
		/// OrderablePartitioner</see>.
		/// </para>
		/// <para>
		/// The <paramref name="body" /> delegate is invoked once for each element in the <paramref name="source" /> 
		/// Partitioner.  It is provided with the following parameters: the current element, 
		/// a <see cref="T:System.Threading.Tasks.ParallelLoopState">ParallelLoopState</see> instance that may be 
		/// used to break out of the loop prematurely, the current element's index (an Int64), and some local 
		/// state that may be shared amongst iterations that execute on the same thread.
		/// </para>
		/// <para>
		/// The <paramref name="localInit" /> delegate is invoked once for each thread that participates in the loop's 
		/// execution and returns the initial local state for each of those threads.  These initial states are passed to the first
		/// <paramref name="body" /> invocations on each thread.  Then, every subsequent body invocation returns a possibly 
		/// modified state value that is passed to the next body invocation.  Finally, the last body invocation on each thread returns a state value 
		/// that is passed to the <paramref name="localFinally" /> delegate.  The localFinally delegate is invoked once per thread to perform a final 
		/// action on each thread's local state.
		/// </para>
		/// </remarks>
		public static ParallelLoopResult ForEach<TSource, TLocal>(OrderablePartitioner<TSource> source, ParallelOptions parallelOptions, Func<TLocal> localInit, Func<TSource, ParallelLoopState, long, TLocal, TLocal> body, Action<TLocal> localFinally)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (body == null)
			{
				throw new ArgumentNullException("body");
			}
			if (localInit == null)
			{
				throw new ArgumentNullException("localInit");
			}
			if (localFinally == null)
			{
				throw new ArgumentNullException("localFinally");
			}
			if (parallelOptions == null)
			{
				throw new ArgumentNullException("parallelOptions");
			}
			if (!source.KeysNormalized)
			{
				throw new InvalidOperationException(Environment2.GetResourceString("Parallel_ForEach_OrderedPartitionerKeysNotNormalized"));
			}
			return PartitionerForEachWorker(source, parallelOptions, null, null, null, null, body, localInit, localFinally);
		}

		private static ParallelLoopResult PartitionerForEachWorker<TSource, TLocal>(Partitioner<TSource> source, ParallelOptions parallelOptions, Action<TSource> simpleBody, Action<TSource, ParallelLoopState> bodyWithState, Action<TSource, ParallelLoopState, long> bodyWithStateAndIndex, Func<TSource, ParallelLoopState, TLocal, TLocal> bodyWithStateAndLocal, Func<TSource, ParallelLoopState, long, TLocal, TLocal> bodyWithEverything, Func<TLocal> localInit, Action<TLocal> localFinally)
		{
			OrderablePartitioner<TSource> orderedSource = source as OrderablePartitioner<TSource>;
			if (!source.SupportsDynamicPartitions)
			{
				throw new InvalidOperationException(Environment2.GetResourceString("Parallel_ForEach_PartitionerNotDynamic"));
			}
			if (parallelOptions.CancellationToken.IsCancellationRequested)
			{
				throw new OperationCanceledException2(parallelOptions.CancellationToken);
			}
			ParallelLoopStateFlags64 sharedPStateFlags = new ParallelLoopStateFlags64();
			ParallelLoopResult result = default(ParallelLoopResult);
			OperationCanceledException oce = null;
			CancellationTokenRegistration cancellationTokenRegistration = default(CancellationTokenRegistration);
			if (parallelOptions.CancellationToken.CanBeCanceled)
			{
				cancellationTokenRegistration = parallelOptions.CancellationToken.InternalRegisterWithoutEC(delegate
				{
					sharedPStateFlags.Cancel();
					oce = new OperationCanceledException2(parallelOptions.CancellationToken);
				}, null);
			}
			IEnumerable<TSource> partitionerSource = null;
			IEnumerable<KeyValuePair<long, TSource>> orderablePartitionerSource = null;
			if (orderedSource != null)
			{
				orderablePartitionerSource = orderedSource.GetOrderableDynamicPartitions();
				if (orderablePartitionerSource == null)
				{
					throw new InvalidOperationException(Environment2.GetResourceString("Parallel_ForEach_PartitionerReturnedNull"));
				}
			}
			else
			{
				partitionerSource = source.GetDynamicPartitions();
				if (partitionerSource == null)
				{
					throw new InvalidOperationException(Environment2.GetResourceString("Parallel_ForEach_PartitionerReturnedNull"));
				}
			}
			ParallelForReplicatingTask rootTask = null;
			Action action = delegate
			{
				Task internalCurrent = Task.InternalCurrent;
				TLocal val = default(TLocal);
				bool flag = false;
				try
				{
					ParallelLoopState64 parallelLoopState = null;
					if (bodyWithState != null || bodyWithStateAndIndex != null)
					{
						parallelLoopState = new ParallelLoopState64(sharedPStateFlags);
					}
					else if (bodyWithStateAndLocal != null || bodyWithEverything != null)
					{
						parallelLoopState = new ParallelLoopState64(sharedPStateFlags);
						if (localInit != null)
						{
							val = localInit();
							flag = true;
						}
					}
					bool flag2 = rootTask == internalCurrent;
					LoopTimer loopTimer = new LoopTimer(rootTask.ActiveChildCount);
					if (orderedSource != null)
					{
						IEnumerator<KeyValuePair<long, TSource>> enumerator = internalCurrent.SavedStateFromPreviousReplica as IEnumerator<KeyValuePair<long, TSource>>;
						if (enumerator == null)
						{
							enumerator = orderablePartitionerSource.GetEnumerator();
							if (enumerator == null)
							{
								throw new InvalidOperationException(Environment2.GetResourceString("Parallel_ForEach_NullEnumerator"));
							}
						}
						while (enumerator.MoveNext())
						{
							KeyValuePair<long, TSource> current = enumerator.Current;
							long key = current.Key;
							TSource value = current.Value;
							if (parallelLoopState != null)
							{
								parallelLoopState.CurrentIteration = key;
							}
							if (simpleBody != null)
							{
								simpleBody(value);
							}
							else if (bodyWithState != null)
							{
								bodyWithState(value, parallelLoopState);
							}
							else if (bodyWithStateAndIndex == null)
							{
								val = ((bodyWithStateAndLocal == null) ? bodyWithEverything(value, parallelLoopState, key, val) : bodyWithStateAndLocal(value, parallelLoopState, val));
							}
							else
							{
								bodyWithStateAndIndex(value, parallelLoopState, key);
							}
							if (sharedPStateFlags.ShouldExitLoop(key))
							{
								break;
							}
							if (!flag2 && loopTimer.LimitExceeded())
							{
								internalCurrent.SavedStateForNextReplica = enumerator;
								break;
							}
						}
					}
					else
					{
						IEnumerator<TSource> enumerator2 = internalCurrent.SavedStateFromPreviousReplica as IEnumerator<TSource>;
						if (enumerator2 == null)
						{
							enumerator2 = partitionerSource.GetEnumerator();
							if (enumerator2 == null)
							{
								throw new InvalidOperationException(Environment2.GetResourceString("Parallel_ForEach_NullEnumerator"));
							}
						}
						if (parallelLoopState != null)
						{
							parallelLoopState.CurrentIteration = 0L;
						}
						while (enumerator2.MoveNext())
						{
							TSource current2 = enumerator2.Current;
							if (simpleBody != null)
							{
								simpleBody(current2);
							}
							else if (bodyWithState != null)
							{
								bodyWithState(current2, parallelLoopState);
							}
							else if (bodyWithStateAndLocal != null)
							{
								val = bodyWithStateAndLocal(current2, parallelLoopState, val);
							}
							if (sharedPStateFlags.LoopStateFlags != ParallelLoopStateFlags.PLS_NONE)
							{
								break;
							}
							if (!flag2 && loopTimer.LimitExceeded())
							{
								internalCurrent.SavedStateForNextReplica = enumerator2;
								break;
							}
						}
					}
				}
				catch
				{
					sharedPStateFlags.SetExceptional();
					throw;
				}
				finally
				{
					if (localFinally != null && flag)
					{
						localFinally(val);
					}
				}
			};
			try
			{
				rootTask = new ParallelForReplicatingTask(parallelOptions, action, TaskCreationOptions.None, InternalTaskOptions.SelfReplicating);
				rootTask.RunSynchronously(parallelOptions.EffectiveTaskScheduler);
				rootTask.Wait();
				if (parallelOptions.CancellationToken.CanBeCanceled)
				{
					cancellationTokenRegistration.Dispose();
				}
				if (oce != null)
				{
					throw oce;
				}
			}
			catch (AggregateException ex)
			{
				if (parallelOptions.CancellationToken.CanBeCanceled)
				{
					cancellationTokenRegistration.Dispose();
				}
				ThrowIfReducableToSingleOCE(ex.InnerExceptions, parallelOptions.CancellationToken);
				throw;
			}
			catch (TaskSchedulerException)
			{
				if (parallelOptions.CancellationToken.CanBeCanceled)
				{
					cancellationTokenRegistration.Dispose();
				}
				throw;
			}
			finally
			{
				int loopStateFlags = sharedPStateFlags.LoopStateFlags;
				result.m_completed = loopStateFlags == ParallelLoopStateFlags.PLS_NONE;
				if ((loopStateFlags & ParallelLoopStateFlags.PLS_BROKEN) != 0)
				{
					result.m_lowestBreakIteration = sharedPStateFlags.LowestBreakIteration;
				}
				if (rootTask != null && rootTask.IsCompleted)
				{
					rootTask.Dispose();
				}
				((orderablePartitionerSource == null) ? (partitionerSource as IDisposable) : (orderablePartitionerSource as IDisposable))?.Dispose();
			}
			return result;
		}

		/// <summary>
		/// Internal utility function that implements the OCE filtering behavior for all Parallel.* APIs.
		/// Throws a single OperationCancelledException object with the token if the Exception collection only contains 
		/// OperationCancelledExceptions with the given CancellationToken.
		///
		/// </summary>
		/// <param name="excCollection"> The exception collection to filter</param>
		/// <param name="ct"> The CancellationToken expected on all inner exceptions</param>
		/// <returns></returns>
		internal static void ThrowIfReducableToSingleOCE(IEnumerable<Exception> excCollection, CancellationToken ct)
		{
			bool flag = false;
			if (!ct.IsCancellationRequested)
			{
				return;
			}
			foreach (Exception item in excCollection)
			{
				flag = true;
				OperationCanceledException2 operationCanceledException = item as OperationCanceledException2;
				if (operationCanceledException == null || operationCanceledException.CancellationToken != ct)
				{
					return;
				}
			}
			if (flag)
			{
				throw new OperationCanceledException2(ct);
			}
		}
	}
}
