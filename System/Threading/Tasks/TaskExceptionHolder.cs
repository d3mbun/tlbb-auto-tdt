using System.Collections.Generic;

namespace System.Threading.Tasks
{
	/// <summary>
	/// An exception holder manages a list of exceptions for one particular task.
	/// It offers the ability to aggregate, but more importantly, also offers intrinsic
	/// support for propagating unhandled exceptions that are never observed. It does
	/// this by aggregating and throwing if the holder is ever GC'd without the holder's
	/// contents ever having been requested (e.g. by a Task.Wait, Task.get_Exception, etc).
	/// </summary>
	internal class TaskExceptionHolder
	{
		private List<Exception> m_exceptions;

		private bool m_isHandled;

		private Task m_task;

		/// <summary>
		/// Creates a new holder; it will be registered for finalization.
		/// </summary>
		/// <param name="task">The task this holder belongs to.</param>
		internal TaskExceptionHolder(Task task)
		{
			m_exceptions = new List<Exception>(1);
			m_task = task;
		}

		/// <summary>
		/// A finalizer that repropagates unhandled exceptions.
		/// </summary>
		~TaskExceptionHolder()
		{
			if (m_isHandled || (Environment.HasShutdownStarted || AppDomain.CurrentDomain.IsFinalizingForUnload()))
			{
				return;
			}
			foreach (Exception exception in m_exceptions)
			{
				AggregateException ex = exception as AggregateException;
				if (ex != null)
				{
					AggregateException ex2 = ex.Flatten();
					foreach (Exception innerException in ex2.InnerExceptions)
					{
						if (innerException is ThreadAbortException)
						{
							return;
						}
					}
				}
				else
				{
					if (exception is ThreadAbortException)
					{
						return;
					}
				}
			}
			AggregateException ex3 = new AggregateException(Environment2.GetResourceString("TaskExceptionHolder_UnhandledException"), m_exceptions);
			UnobservedTaskExceptionEventArgs unobservedTaskExceptionEventArgs = new UnobservedTaskExceptionEventArgs(ex3);
			TaskScheduler.PublishUnobservedTaskException(m_task, unobservedTaskExceptionEventArgs);
			if (!unobservedTaskExceptionEventArgs.m_observed)
			{
				throw ex3;
			}
		}

		/// <summary>
		/// Add an exception to the internal list.  This will ensure the holder is
		/// in the proper state (handled/unhandled) depending on the list's contents.
		/// </summary>
		/// <param name="exceptionObject">An exception object (either an Exception or an 
		/// IEnumerable{Exception}) to add to the list.</param>
		internal void Add(object exceptionObject)
		{
			Exception ex = exceptionObject as Exception;
			if (ex != null)
			{
				m_exceptions.Add(ex);
			}
			else
			{
				IEnumerable<Exception> enumerable = exceptionObject as IEnumerable<Exception>;
				if (enumerable == null)
				{
					throw new ArgumentException(Environment2.GetResourceString("TaskExceptionHolder_UnknownExceptionType"), "exceptionObject");
				}
				m_exceptions.AddRange(enumerable);
			}
			for (int i = 0; i < m_exceptions.Count; i++)
			{
				if (m_exceptions[i].GetType() != typeof(ThreadAbortException) && m_exceptions[i].GetType() != typeof(AppDomainUnloadedException))
				{
					MarkAsUnhandled();
					break;
				}
				if (i == m_exceptions.Count - 1)
				{
					MarkAsHandled(calledFromFinalizer: false);
				}
			}
		}

		/// <summary>
		/// A private helper method that ensures the holder is considered
		/// unhandled, i.e. it is registered for finalization.
		/// </summary>
		private void MarkAsUnhandled()
		{
			if (m_isHandled)
			{
				GC.ReRegisterForFinalize(this);
				m_isHandled = false;
			}
		}

		/// <summary>
		/// A private helper method that ensures the holder is considered
		/// handled, i.e. it is not registered for finalization.
		/// </summary>
		/// <param name="calledFromFinalizer">Whether this is called from the finalizer thread.</param> 
		internal void MarkAsHandled(bool calledFromFinalizer)
		{
			if (!m_isHandled)
			{
				if (!calledFromFinalizer)
				{
					GC.SuppressFinalize(this);
				}
				m_isHandled = true;
			}
		}

		/// <summary>
		/// Allocates a new aggregate exception and adds the contents of the list to
		/// it. By calling this method, the holder assumes exceptions to have been
		/// "observed", such that the finalization check will be subsequently skipped.
		/// </summary>
		/// <param name="calledFromFinalizer">Whether this is being called from a finalizer.</param>
		/// <param name="includeThisException">An extra exception to be included (optionally).</param>
		/// <returns>The aggregate exception to throw.</returns>
		internal AggregateException CreateExceptionObject(bool calledFromFinalizer, Exception includeThisException)
		{
			MarkAsHandled(calledFromFinalizer);
			List<Exception> list = m_exceptions;
			if (includeThisException != null)
			{
				list = new List<Exception>(list);
				list.Add(includeThisException);
			}
			return new AggregateException(list);
		}
	}
}
