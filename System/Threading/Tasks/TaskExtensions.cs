namespace System.Threading.Tasks
{
	/// <summary>
	/// Provides a set of static (Shared in Visual Basic) methods for working with specific kinds of 
	/// <see cref="T:System.Threading.Tasks.Task" /> instances.
	/// </summary>
	public static class TaskExtensions
	{
		/// <summary>
		/// Creates a proxy <see cref="T:System.Threading.Tasks.Task">Task</see> that represents the 
		/// asynchronous operation of a Task{Task}.
		/// </summary>
		/// <remarks>
		/// It is often useful to be able to return a Task from a <see cref="T:System.Threading.Tasks.Task`1">
		/// Task{TResult}</see>, where the inner Task represents work done as part of the outer Task{TResult}.  However, 
		/// doing so results in a Task{Task}, which, if not dealt with carefully, could produce unexpected behavior.  Unwrap 
		/// solves this problem by creating a proxy Task that represents the entire asynchronous operation of such a Task{Task}.
		/// </remarks>
		/// <param name="task">The Task{Task} to unwrap.</param>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown if the 
		/// <paramref name="task" /> argument is null.</exception>
		/// <returns>A Task that represents the asynchronous operation of the provided Task{Task}.</returns>
		public static Task Unwrap(this Task<Task> task)
		{
			if (task == null)
			{
				throw new ArgumentNullException("task");
			}
			TaskCompletionSource<Task> tcs = new TaskCompletionSource<Task>(task.CreationOptions & TaskCreationOptions.AttachedToParent);
			bool result;
			task.ContinueWith(delegate
			{
				switch (task.Status)
				{
				case TaskStatus.Canceled:
				case TaskStatus.Faulted:
					result = tcs.TrySetFromTask(task);
					break;
				case TaskStatus.RanToCompletion:
					if (task.Result == null)
					{
						tcs.TrySetCanceled();
					}
					else
					{
						task.Result.ContinueWith(delegate
						{
							result = tcs.TrySetFromTask(task.Result);
						}, TaskContinuationOptions.ExecuteSynchronously).ContinueWith(delegate(Task antecedent)
						{
							tcs.TrySetException(antecedent.Exception);
						}, TaskContinuationOptions.OnlyOnFaulted);
					}
					break;
				}
			}, TaskContinuationOptions.ExecuteSynchronously).ContinueWith(delegate(Task antecedent)
			{
				tcs.TrySetException(antecedent.Exception);
			}, TaskContinuationOptions.OnlyOnFaulted);
			return tcs.Task;
		}

		/// <summary>
		/// Creates a proxy <see cref="T:System.Threading.Tasks.Task`1">Task{TResult}</see> that represents the 
		/// asynchronous operation of a Task{Task{TResult}}.
		/// </summary>
		/// <remarks>
		/// It is often useful to be able to return a Task{TResult} from a Task{TResult}, where the inner Task{TResult} 
		/// represents work done as part of the outer Task{TResult}.  However, doing so results in a Task{Task{TResult}}, 
		/// which, if not dealt with carefully, could produce unexpected behavior.  Unwrap solves this problem by 
		/// creating a proxy Task{TResult} that represents the entire asynchronous operation of such a Task{Task{TResult}}.
		/// </remarks>
		/// <param name="task">The Task{Task{TResult}} to unwrap.</param>
		/// <exception cref="T:System.ArgumentNullException">The exception that is thrown if the 
		/// <paramref name="task" /> argument is null.</exception>
		/// <returns>A Task{TResult} that represents the asynchronous operation of the provided Task{Task{TResult}}.</returns>        /// <summary>Unwraps a Task that returns another Task.</summary>
		public static Task<TResult> Unwrap<TResult>(this Task<Task<TResult>> task)
		{
			if (task == null)
			{
				throw new ArgumentNullException("task");
			}
			TaskCompletionSource<TResult> tcs = new TaskCompletionSource<TResult>(task.CreationOptions & TaskCreationOptions.AttachedToParent);
			bool result;
			task.ContinueWith(delegate
			{
				switch (task.Status)
				{
				case TaskStatus.Canceled:
				case TaskStatus.Faulted:
					result = tcs.TrySetFromTask(task);
					break;
				case TaskStatus.RanToCompletion:
					if (task.Result == null)
					{
						tcs.TrySetCanceled();
					}
					else
					{
						task.Result.ContinueWith(delegate
						{
							result = tcs.TrySetFromTask(task.Result);
						}, TaskContinuationOptions.ExecuteSynchronously).ContinueWith(delegate(Task antecedent)
						{
							tcs.TrySetException(antecedent.Exception);
						}, TaskContinuationOptions.OnlyOnFaulted);
					}
					break;
				}
			}, TaskContinuationOptions.ExecuteSynchronously).ContinueWith(delegate(Task antecedent)
			{
				tcs.TrySetException(antecedent.Exception);
			}, TaskContinuationOptions.OnlyOnFaulted);
			return tcs.Task;
		}

		private static bool TrySetFromTask<TResult>(this TaskCompletionSource<TResult> me, Task source)
		{
			bool result = false;
			switch (source.Status)
			{
			case TaskStatus.Canceled:
				result = me.TrySetCanceled();
				break;
			case TaskStatus.Faulted:
				result = me.TrySetException(source.Exception.InnerExceptions);
				break;
			case TaskStatus.RanToCompletion:
				result = ((!(source is Task<TResult>)) ? me.TrySetResult(default(TResult)) : me.TrySetResult(((Task<TResult>)source).Result));
				break;
			}
			return result;
		}
	}
}
