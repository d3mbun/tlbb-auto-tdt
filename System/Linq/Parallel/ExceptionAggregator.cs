using System.Collections.Generic;
using System.Threading;

namespace System.Linq.Parallel
{
	internal static class ExceptionAggregator
	{
		/// <summary>
		/// WrapEnumerable.ExceptionAggregator wraps the enumerable with another enumerator that will
		/// catch exceptions, and wrap each with an AggregateException.
		///
		/// If PLINQ decides to execute a query sequentially, we will reuse LINQ-to-objects
		/// implementations for the different operators. However, we still need to throw
		/// AggregateException in the cases when parallel execution would have thrown an
		/// AggregateException. Thus, we introduce a wrapper enumerator that catches exceptions
		/// and wraps them with an AggregateException.
		/// </summary>
		internal static IEnumerable<TElement> WrapEnumerable<TElement>(IEnumerable<TElement> source, CancellationState cancellationState)
		{
			using (IEnumerator<TElement> enumerator = source.GetEnumerator())
			{
				while (true)
				{
					TElement elem = default(TElement);
					try
					{
						if (!enumerator.MoveNext())
						{
							yield break;
						}
						elem = enumerator.Current;
					}
					catch (ThreadAbortException)
					{
						throw;
					}
					catch (Exception ex2)
					{
						ThrowOCEorAggregateException(ex2, cancellationState);
					}
					yield return elem;
				}
			}
		}

		/// <summary>
		/// A variant of WrapEnumerable that accepts a QueryOperatorEnumerator{,} instead of an IEnumerable{}.
		/// The code duplication is necessary to avoid extra virtual method calls that would otherwise be needed to
		/// convert the QueryOperatorEnumerator{,} to an IEnumerator{}.
		/// </summary>
		internal static IEnumerable<TElement> WrapQueryEnumerator<TElement, TIgnoreKey>(QueryOperatorEnumerator<TElement, TIgnoreKey> source, CancellationState cancellationState)
		{
			TElement elem = default(TElement);
			TIgnoreKey ignoreKey = default(TIgnoreKey);
			try
			{
				while (true)
				{
					try
					{
						if (!source.MoveNext(ref elem, ref ignoreKey))
						{
							yield break;
						}
					}
					catch (ThreadAbortException)
					{
						throw;
					}
					catch (Exception ex2)
					{
						ThrowOCEorAggregateException(ex2, cancellationState);
					}
					yield return elem;
				}
			}
			finally
			{
				source.Dispose();
			}
		}

		/// <summary>
		/// Accepts an exception, wraps it as if it was crossing the parallel-&gt;sequential boundary, and throws the
		/// wrapped exception. In sequential fallback cases, we use this method to throw exceptions that are consistent
		/// with exceptions thrown by PLINQ when the query is executed by worker tasks.
		///
		/// The exception will be wrapped into an AggregateException, except for the case when the query is being
		/// legitimately cancelled, in which case we will propagate the CancellationException with the appropriate
		/// token.
		/// </summary>
		internal static void ThrowOCEorAggregateException(Exception ex, CancellationState cancellationState)
		{
			if (ThrowAnOCE(ex, cancellationState))
			{
				CancellationState.ThrowWithStandardMessageIfCanceled(cancellationState.ExternalCancellationToken);
				return;
			}
			throw new AggregateException(ex);
		}

		/// <summary>
		/// Wraps a function with a try/catch that morphs all exceptions into AggregateException.
		/// </summary>
		/// <typeparam name="T">The input argument type.</typeparam>
		/// <typeparam name="U">The return value type.</typeparam>
		/// <param name="f">A function to use internally.</param>
		/// <param name="cancellationState">The cancellation state to use.</param>
		/// <returns>A new function containing exception wrapping logic.</returns>
		internal static Func<T, U> WrapFunc<T, U>(Func<T, U> f, CancellationState cancellationState)
		{
			return delegate(T t)
			{
				U result = default(U);
				try
				{
					result = f(t);
					return result;
				}
				catch (ThreadAbortException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					ThrowOCEorAggregateException(ex2, cancellationState);
					return result;
				}
			};
		}

		private static bool ThrowAnOCE(Exception ex, CancellationState cancellationState)
		{
			OperationCanceledException2 operationCanceledException = ex as OperationCanceledException2;
			if (operationCanceledException != null && operationCanceledException.CancellationToken == cancellationState.ExternalCancellationToken && cancellationState.ExternalCancellationToken.IsCancellationRequested)
			{
				return true;
			}
			if (operationCanceledException != null && operationCanceledException.CancellationToken == cancellationState.MergedCancellationToken && cancellationState.MergedCancellationToken.IsCancellationRequested && cancellationState.ExternalCancellationToken.IsCancellationRequested)
			{
				return true;
			}
			return false;
		}
	}
}
