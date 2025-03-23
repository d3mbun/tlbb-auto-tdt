using System.Collections.Generic;
using System.Security.Permissions;

namespace System.Threading.Tasks
{
	/// <summary>
	/// Represents the producer side of a <see cref="T:System.Threading.Tasks.Task{TResult}" /> unbound to a
	/// delegate, providing access to the consumer side through the <see cref="P:System.Threading.Tasks.TaskCompletionSource`1.Task" /> property.
	/// </summary>
	/// <remarks>
	/// <para>
	/// It is often the case that a <see cref="T:System.Threading.Tasks.Task{TResult}" /> is desired to
	/// represent another asynchronous operation.
	/// <see cref="T:System.Threading.Tasks.TaskCompletionSource`1">TaskCompletionSource</see> is provided for this purpose. It enables
	/// the creation of a task that can be handed out to consumers, and those consumers can use the members
	/// of the task as they would any other. However, unlike most tasks, the state of a task created by a
	/// TaskCompletionSource is controlled explicitly by the methods on TaskCompletionSource. This enables the
	/// completion of the external asynchronous operation to be propagated to the underlying Task. The
	/// separation also ensures that consumers are not able to transition the state without access to the
	/// corresponding TaskCompletionSource.
	/// </para>
	/// <para>
	/// All members of <see cref="T:System.Threading.Tasks.TaskCompletionSource`1" /> are thread-safe
	/// and may be used from multiple threads concurrently.
	/// </para>
	/// </remarks>
	/// <typeparam name="TResult">The type of the result value assocatied with this <see cref="T:System.Threading.Tasks.TaskCompletionSource`1" />.</typeparam>
	[HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
	public class TaskCompletionSource<TResult>
	{
		private Task<TResult> m_task;

		/// <summary>
		/// Gets the <see cref="T:System.Threading.Tasks.Task{TResult}" /> created
		/// by this <see cref="T:System.Threading.Tasks.TaskCompletionSource`1" />.
		/// </summary>
		/// <remarks>
		/// This property enables a consumer access to the <see cref="T:System.Threading.Tasks.Task{TResult}" /> that is controlled by this instance.
		/// The <see cref="M:System.Threading.Tasks.TaskCompletionSource`1.SetResult(`0)" />, <see cref="M:System.Threading.Tasks.TaskCompletionSource`1.SetException(System.Exception)" />,
		/// <see cref="M:System.Threading.Tasks.TaskCompletionSource`1.SetException(System.Collections.Generic.IEnumerable{System.Exception})" />, and <see cref="M:System.Threading.Tasks.TaskCompletionSource`1.SetCanceled" />
		/// methods (and their "Try" variants) on this instance all result in the relevant state
		/// transitions on this underlying Task.
		/// </remarks>
		public Task<TResult> Task => m_task;

		/// <summary>
		/// Creates a <see cref="T:System.Threading.Tasks.TaskCompletionSource`1" />.
		/// </summary>
		public TaskCompletionSource()
			: this((object)null, TaskCreationOptions.None)
		{
		}

		/// <summary>
		/// Creates a <see cref="T:System.Threading.Tasks.TaskCompletionSource`1" />
		/// with the specified options.
		/// </summary>
		/// <remarks>
		/// The <see cref="T:System.Threading.Tasks.Task{TResult}" /> created
		/// by this instance and accessible through its <see cref="P:System.Threading.Tasks.TaskCompletionSource`1.Task" /> property
		/// will be instantiated using the specified <paramref name="creationOptions" />.
		/// </remarks>
		/// <param name="creationOptions">The options to use when creating the underlying
		/// <see cref="T:System.Threading.Tasks.Task{TResult}" />.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// The <paramref name="creationOptions" /> represent options invalid for use
		/// with a <see cref="T:System.Threading.Tasks.TaskCompletionSource`1" />.
		/// </exception>
		public TaskCompletionSource(TaskCreationOptions creationOptions)
			: this((object)null, creationOptions)
		{
		}

		/// <summary>
		/// Creates a <see cref="T:System.Threading.Tasks.TaskCompletionSource`1" />
		/// with the specified state.
		/// </summary>
		/// <param name="state">The state to use as the underlying 
		/// <see cref="T:System.Threading.Tasks.Task{TResult}" />'s AsyncState.</param>
		public TaskCompletionSource(object state)
			: this(state, TaskCreationOptions.None)
		{
		}

		/// <summary>
		/// Creates a <see cref="T:System.Threading.Tasks.TaskCompletionSource`1" /> with
		/// the specified state and options.
		/// </summary>
		/// <param name="creationOptions">The options to use when creating the underlying
		/// <see cref="T:System.Threading.Tasks.Task{TResult}" />.</param>
		/// <param name="state">The state to use as the underlying 
		/// <see cref="T:System.Threading.Tasks.Task{TResult}" />'s AsyncState.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// The <paramref name="creationOptions" /> represent options invalid for use
		/// with a <see cref="T:System.Threading.Tasks.TaskCompletionSource`1" />.
		/// </exception>
		public TaskCompletionSource(object state, TaskCreationOptions creationOptions)
		{
			m_task = new Task<TResult>(state, CancellationToken.None, creationOptions, InternalTaskOptions.PromiseTask);
		}

		/// <summary>
		/// Attempts to transition the underlying
		/// <see cref="T:System.Threading.Tasks.Task{TResult}" /> into the 
		/// <see cref="F:System.Threading.Tasks.TaskStatus.Faulted">Faulted</see>
		/// state.
		/// </summary>
		/// <param name="exception">The exception to bind to this <see cref="T:System.Threading.Tasks.Task{TResult}" />.</param>
		/// <returns>True if the operation was successful; otherwise, false.</returns>
		/// <remarks>This operation will return false if the 
		/// <see cref="T:System.Threading.Tasks.Task{TResult}" /> is already in one
		/// of the three final states:
		/// <see cref="F:System.Threading.Tasks.TaskStatus.RanToCompletion">RanToCompletion</see>, 
		/// <see cref="F:System.Threading.Tasks.TaskStatus.Faulted">Faulted</see>, or
		/// <see cref="F:System.Threading.Tasks.TaskStatus.Canceled">Canceled</see>.
		/// </remarks>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="exception" /> argument is null.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="P:System.Threading.Tasks.TaskCompletionSource`1.Task" /> was disposed.</exception>
		public bool TrySetException(Exception exception)
		{
			if (exception == null)
			{
				throw new ArgumentNullException("exception");
			}
			bool flag = m_task.TrySetException(exception);
			if (!flag && !m_task.IsCompleted)
			{
				SpinWait spinWait = default(SpinWait);
				while (!m_task.IsCompleted)
				{
					spinWait.SpinOnce();
				}
			}
			return flag;
		}

		/// <summary>
		/// Attempts to transition the underlying
		/// <see cref="T:System.Threading.Tasks.Task{TResult}" /> into the 
		/// <see cref="F:System.Threading.Tasks.TaskStatus.Faulted">Faulted</see>
		/// state.
		/// </summary>
		/// <param name="exceptions">The collection of exceptions to bind to this <see cref="T:System.Threading.Tasks.Task{TResult}" />.</param>
		/// <returns>True if the operation was successful; otherwise, false.</returns>
		/// <remarks>This operation will return false if the 
		/// <see cref="T:System.Threading.Tasks.Task{TResult}" /> is already in one
		/// of the three final states:
		/// <see cref="F:System.Threading.Tasks.TaskStatus.RanToCompletion">RanToCompletion</see>, 
		/// <see cref="F:System.Threading.Tasks.TaskStatus.Faulted">Faulted</see>, or
		/// <see cref="F:System.Threading.Tasks.TaskStatus.Canceled">Canceled</see>.
		/// </remarks>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="exceptions" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentException">There are one or more null elements in <paramref name="exceptions" />.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="exceptions" /> collection is empty.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="P:System.Threading.Tasks.TaskCompletionSource`1.Task" /> was disposed.</exception>
		public bool TrySetException(IEnumerable<Exception> exceptions)
		{
			if (exceptions == null)
			{
				throw new ArgumentNullException("exceptions");
			}
			List<Exception> list = new List<Exception>();
			foreach (Exception exception in exceptions)
			{
				if (exception == null)
				{
					throw new ArgumentException(Environment2.GetResourceString("TaskCompletionSourceT_TrySetException_NullException"), "exceptions");
				}
				list.Add(exception);
			}
			if (list.Count == 0)
			{
				throw new ArgumentException(Environment2.GetResourceString("TaskCompletionSourceT_TrySetException_NoExceptions"), "exceptions");
			}
			bool flag = m_task.TrySetException(list);
			if (!flag && !m_task.IsCompleted)
			{
				SpinWait spinWait = default(SpinWait);
				while (!m_task.IsCompleted)
				{
					spinWait.SpinOnce();
				}
			}
			return flag;
		}

		/// <summary>
		/// Transitions the underlying
		/// <see cref="T:System.Threading.Tasks.Task{TResult}" /> into the 
		/// <see cref="F:System.Threading.Tasks.TaskStatus.Faulted">Faulted</see>
		/// state.
		/// </summary>
		/// <param name="exception">The exception to bind to this <see cref="T:System.Threading.Tasks.Task{TResult}" />.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="exception" /> argument is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		/// The underlying <see cref="T:System.Threading.Tasks.Task{TResult}" /> is already in one
		/// of the three final states:
		/// <see cref="F:System.Threading.Tasks.TaskStatus.RanToCompletion">RanToCompletion</see>, 
		/// <see cref="F:System.Threading.Tasks.TaskStatus.Faulted">Faulted</see>, or
		/// <see cref="F:System.Threading.Tasks.TaskStatus.Canceled">Canceled</see>.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="P:System.Threading.Tasks.TaskCompletionSource`1.Task" /> was disposed.</exception>
		public void SetException(Exception exception)
		{
			if (exception == null)
			{
				throw new ArgumentNullException("exception");
			}
			if (!TrySetException(exception))
			{
				throw new InvalidOperationException(Environment2.GetResourceString("TaskT_TransitionToFinal_AlreadyCompleted"));
			}
		}

		/// <summary>
		/// Transitions the underlying
		/// <see cref="T:System.Threading.Tasks.Task{TResult}" /> into the 
		/// <see cref="F:System.Threading.Tasks.TaskStatus.Faulted">Faulted</see>
		/// state.
		/// </summary>
		/// <param name="exceptions">The collection of exceptions to bind to this <see cref="T:System.Threading.Tasks.Task{TResult}" />.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="exceptions" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentException">There are one or more null elements in <paramref name="exceptions" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		/// The underlying <see cref="T:System.Threading.Tasks.Task{TResult}" /> is already in one
		/// of the three final states:
		/// <see cref="F:System.Threading.Tasks.TaskStatus.RanToCompletion">RanToCompletion</see>, 
		/// <see cref="F:System.Threading.Tasks.TaskStatus.Faulted">Faulted</see>, or
		/// <see cref="F:System.Threading.Tasks.TaskStatus.Canceled">Canceled</see>.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="P:System.Threading.Tasks.TaskCompletionSource`1.Task" /> was disposed.</exception>
		public void SetException(IEnumerable<Exception> exceptions)
		{
			if (!TrySetException(exceptions))
			{
				throw new InvalidOperationException(Environment2.GetResourceString("TaskT_TransitionToFinal_AlreadyCompleted"));
			}
		}

		/// <summary>
		/// Attempts to transition the underlying
		/// <see cref="T:System.Threading.Tasks.Task{TResult}" /> into the 
		/// <see cref="F:System.Threading.Tasks.TaskStatus.RanToCompletion">RanToCompletion</see>
		/// state.
		/// </summary>
		/// <param name="result">The result value to bind to this <see cref="T:System.Threading.Tasks.Task{TResult}" />.</param>
		/// <returns>True if the operation was successful; otherwise, false.</returns>
		/// <remarks>This operation will return false if the 
		/// <see cref="T:System.Threading.Tasks.Task{TResult}" /> is already in one
		/// of the three final states:
		/// <see cref="F:System.Threading.Tasks.TaskStatus.RanToCompletion">RanToCompletion</see>, 
		/// <see cref="F:System.Threading.Tasks.TaskStatus.Faulted">Faulted</see>, or
		/// <see cref="F:System.Threading.Tasks.TaskStatus.Canceled">Canceled</see>.
		/// </remarks>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="P:System.Threading.Tasks.TaskCompletionSource`1.Task" /> was disposed.</exception>
		public bool TrySetResult(TResult result)
		{
			if (m_task.IsCompleted)
			{
				return false;
			}
			bool flag = m_task.TrySetResult(result);
			if (!flag && !m_task.IsCompleted)
			{
				SpinWait spinWait = default(SpinWait);
				while (!m_task.IsCompleted)
				{
					spinWait.SpinOnce();
				}
			}
			return flag;
		}

		/// <summary>
		/// Transitions the underlying
		/// <see cref="T:System.Threading.Tasks.Task{TResult}" /> into the 
		/// <see cref="F:System.Threading.Tasks.TaskStatus.RanToCompletion">RanToCompletion</see>
		/// state.
		/// </summary>
		/// <param name="result">The result value to bind to this <see cref="T:System.Threading.Tasks.Task{TResult}" />.</param>
		/// <exception cref="T:System.InvalidOperationException">
		/// The underlying <see cref="T:System.Threading.Tasks.Task{TResult}" /> is already in one
		/// of the three final states:
		/// <see cref="F:System.Threading.Tasks.TaskStatus.RanToCompletion">RanToCompletion</see>, 
		/// <see cref="F:System.Threading.Tasks.TaskStatus.Faulted">Faulted</see>, or
		/// <see cref="F:System.Threading.Tasks.TaskStatus.Canceled">Canceled</see>.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="P:System.Threading.Tasks.TaskCompletionSource`1.Task" /> was disposed.</exception>
		public void SetResult(TResult result)
		{
			m_task.Result = result;
		}

		/// <summary>
		/// Transitions the underlying
		/// <see cref="T:System.Threading.Tasks.Task{TResult}" /> into the 
		/// <see cref="F:System.Threading.Tasks.TaskStatus.Canceled">Canceled</see>
		/// state.
		/// </summary>
		/// <exception cref="T:System.InvalidOperationException">
		/// The underlying <see cref="T:System.Threading.Tasks.Task{TResult}" /> is already in one
		/// of the three final states:
		/// <see cref="F:System.Threading.Tasks.TaskStatus.RanToCompletion">RanToCompletion</see>, 
		/// <see cref="F:System.Threading.Tasks.TaskStatus.Faulted">Faulted</see>, or
		/// <see cref="F:System.Threading.Tasks.TaskStatus.Canceled">Canceled</see>.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="P:System.Threading.Tasks.TaskCompletionSource`1.Task" /> was disposed.</exception>
		public void SetCanceled()
		{
			if (!TrySetCanceled())
			{
				throw new InvalidOperationException(Environment2.GetResourceString("TaskT_TransitionToFinal_AlreadyCompleted"));
			}
		}

		/// <summary>
		/// Attempts to transition the underlying
		/// <see cref="T:System.Threading.Tasks.Task{TResult}" /> into the 
		/// <see cref="F:System.Threading.Tasks.TaskStatus.Canceled">Canceled</see>
		/// state.
		/// </summary>
		/// <returns>True if the operation was successful; otherwise, false.</returns>
		/// <remarks>This operation will return false if the 
		/// <see cref="T:System.Threading.Tasks.Task{TResult}" /> is already in one
		/// of the three final states:
		/// <see cref="F:System.Threading.Tasks.TaskStatus.RanToCompletion">RanToCompletion</see>, 
		/// <see cref="F:System.Threading.Tasks.TaskStatus.Faulted">Faulted</see>, or
		/// <see cref="F:System.Threading.Tasks.TaskStatus.Canceled">Canceled</see>.
		/// </remarks>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="P:System.Threading.Tasks.TaskCompletionSource`1.Task" /> was disposed.</exception>
		public bool TrySetCanceled()
		{
			bool result = false;
			if (m_task.AtomicStateUpdate(67108864, 90177536))
			{
				m_task.RecordInternalCancellationRequest();
				m_task.CancellationCleanupLogic();
				result = true;
			}
			else if (!m_task.IsCompleted)
			{
				SpinWait spinWait = default(SpinWait);
				while (!m_task.IsCompleted)
				{
					spinWait.SpinOnce();
				}
			}
			return result;
		}
	}
}
