using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Threading
{
	/// <summary>
	/// Signals to a <see cref="T:System.Threading.CancellationToken" /> that it should be canceled.
	/// </summary>
	/// <remarks>
	/// <para>
	/// <see cref="T:System.Threading.CancellationTokenSource" /> is used to instantiate a <see cref="T:System.Threading.CancellationToken" />
	/// (via the source's <see cref="P:System.Threading.CancellationTokenSource.Token">Token</see> property)
	/// that can be handed to operations that wish to be notified of cancellation or that can be used to
	/// register asynchronous operations for cancellation. That token may have cancellation requested by
	/// calling to the source's <see cref="M:System.Threading.CancellationTokenSource.Cancel">Cancel</see>
	/// method.
	/// </para>
	/// <para>
	/// All members of this class, except <see cref="M:System.Threading.CancellationTokenSource.Dispose">Dispose</see>, are thread-safe and may be used
	/// concurrently from multiple threads.
	/// </para>
	/// </remarks>
	[ComVisible(false)]
	[HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
	public sealed class CancellationTokenSource : IDisposable
	{
		private const int CANNOT_BE_CANCELED = 0;

		private const int NOT_CANCELED = 1;

		private const int NOTIFYING = 2;

		private const int NOTIFYINGCOMPLETE = 3;

		private static readonly CancellationTokenSource _staticSource_Set = new CancellationTokenSource(set: true);

		private static readonly CancellationTokenSource _staticSource_NotCancelable = new CancellationTokenSource(set: false);

		private static readonly int s_nLists = ((PlatformHelper.ProcessorCount > 24) ? 24 : PlatformHelper.ProcessorCount);

		private volatile ManualResetEvent m_kernelEvent;

		private volatile SparselyPopulatedArray<CancellationCallbackInfo>[] m_registeredCallbacksLists;

		private volatile int m_state;

		/// The ID of the thread currently executing the main body of CTS.Cancel()
		/// this helps us to know if a call to ctr.Dispose() is running 'within' a cancellation callback.
		/// This is updated as we move between the main thread calling cts.Cancel() and any syncContexts that are used to 
		/// actually run the callbacks.
		private volatile int m_threadIDExecutingCallbacks = -1;

		private bool m_disposed;

		private List<CancellationTokenRegistration> m_linkingRegistrations;

		private static readonly Action<object> s_LinkedTokenCancelDelegate = LinkedTokenCancelDelegate;

		private volatile CancellationCallbackInfo m_executingCallback;

		/// <summary>
		/// Gets whether cancellation has been requested for this <see cref="T:System.Threading.CancellationTokenSource">CancellationTokenSource</see>.
		/// </summary>
		/// <value>Whether cancellation has been requested for this <see cref="T:System.Threading.CancellationTokenSource">CancellationTokenSource</see>.</value>
		/// <remarks>
		/// <para>
		/// This property indicates whether cancellation has been requested for this token source, such as
		/// due to a call to its
		/// <see cref="M:System.Threading.CancellationTokenSource.Cancel">Cancel</see> method.
		/// </para>
		/// <para>
		/// If this property returns true, it only guarantees that cancellation has been requested. It does not
		/// guarantee that every handler registered with the corresponding token has finished executing, nor
		/// that cancellation requests have finished propagating to all registered handlers. Additional
		/// synchronization may be required, particularly in situations where related objects are being
		/// canceled concurrently.
		/// </para>
		/// </remarks>
		public bool IsCancellationRequested => m_state >= 2;

		/// <summary>
		/// A simple helper to determine whether cancellation has finished.
		/// </summary>
		internal bool IsCancellationCompleted => m_state == 3;

		/// <summary>
		/// A simple helper to determine whether disposal has occured.
		/// </summary>
		internal bool IsDisposed => m_disposed;

		/// <summary>
		/// The ID of the thread that is running callbacks.
		/// </summary>
		internal int ThreadIDExecutingCallbacks
		{
			get
			{
				return m_threadIDExecutingCallbacks;
			}
			set
			{
				m_threadIDExecutingCallbacks = value;
			}
		}

		/// <summary>
		/// Gets the <see cref="T:System.Threading.CancellationToken">CancellationToken</see>
		/// associated with this <see cref="T:System.Threading.CancellationTokenSource" />.
		/// </summary>
		/// <value>The <see cref="T:System.Threading.CancellationToken">CancellationToken</see>
		/// associated with this <see cref="T:System.Threading.CancellationTokenSource" />.</value>
		/// <exception cref="T:System.ObjectDisposedException">The token source has been
		/// disposed.</exception>
		public CancellationToken Token
		{
			get
			{
				ThrowIfDisposed();
				return new CancellationToken(this);
			}
		}

		/// <summary>
		///
		/// </summary>
		internal bool CanBeCanceled => m_state != 0;

		/// <summary>
		///
		/// </summary>
		internal WaitHandle WaitHandle
		{
			get
			{
				ThrowIfDisposed();
				if (m_kernelEvent != null)
				{
					return m_kernelEvent;
				}
				ManualResetEvent manualResetEvent = new ManualResetEvent(initialState: false);
				if (Interlocked.CompareExchange(ref m_kernelEvent, manualResetEvent, null) != null)
				{
					((IDisposable)manualResetEvent).Dispose();
				}
				if (IsCancellationRequested)
				{
					m_kernelEvent.Set();
				}
				return m_kernelEvent;
			}
		}

		/// <summary>
		/// The currently executing callback
		/// </summary>
		internal CancellationCallbackInfo ExecutingCallback => m_executingCallback;

		private static void LinkedTokenCancelDelegate(object source)
		{
			CancellationTokenSource cancellationTokenSource = source as CancellationTokenSource;
			cancellationTokenSource.Cancel();
		}

		/// <summary>
		/// Initializes the <see cref="T:System.Threading.CancellationTokenSource" />.
		/// </summary>
		public CancellationTokenSource()
		{
			m_state = 1;
		}

		private CancellationTokenSource(bool set)
		{
			m_state = (set ? 3 : 0);
		}

		/// <summary>
		/// Communicates a request for cancellation.
		/// </summary>
		/// <remarks>
		/// <para>
		/// The associated <see cref="T:System.Threading.CancellationToken" /> will be
		/// notified of the cancellation and will transition to a state where 
		/// <see cref="P:System.Threading.CancellationToken.IsCancellationRequested">IsCancellationRequested</see> returns true. 
		/// Any callbacks or cancelable operations
		/// registered with the <see cref="T:System.Threading.CancellationToken" />  will be executed.
		/// </para>
		/// <para>
		/// Cancelable operations and callbacks registered with the token should not throw exceptions.
		/// However, this overload of Cancel will aggregate any exceptions thrown into a <see cref="T:System.AggregateException" />,
		/// such that one callback throwing an exception will not prevent other registered callbacks from being executed.
		/// </para>
		/// <para>
		/// The <see cref="T:System.Threading.ExecutionContext" /> that was captured when each callback was registered
		/// will be reestablished when the callback is invoked.
		/// </para>
		/// </remarks>
		/// <exception cref="T:System.AggregateException">An aggregate exception containing all the exceptions thrown
		/// by the registered callbacks on the associated <see cref="T:System.Threading.CancellationToken" />.</exception>
		/// <exception cref="T:System.ObjectDisposedException">This <see cref="T:System.Threading.CancellationTokenSource" /> has been disposed.</exception> 
		public void Cancel()
		{
			Cancel(throwOnFirstException: false);
		}

		/// <summary>
		/// Communicates a request for cancellation.
		/// </summary>
		/// <remarks>
		/// <para>
		/// The associated <see cref="T:System.Threading.CancellationToken" /> will be
		/// notified of the cancellation and will transition to a state where 
		/// <see cref="P:System.Threading.CancellationToken.IsCancellationRequested">IsCancellationRequested</see> returns true. 
		/// Any callbacks or cancelable operations
		/// registered with the <see cref="T:System.Threading.CancellationToken" />  will be executed.
		/// </para>
		/// <para>
		/// Cancelable operations and callbacks registered with the token should not throw exceptions. 
		/// If <paramref name="throwOnFirstException" /> is true, an exception will immediately propagate out of the
		/// call to Cancel, preventing the remaining callbacks and cancelable operations from being processed.
		/// If <paramref name="throwOnFirstException" /> is false, this overload will aggregate any 
		/// exceptions thrown into a <see cref="T:System.AggregateException" />,
		/// such that one callback throwing an exception will not prevent other registered callbacks from being executed.
		/// </para>
		/// <para>
		/// The <see cref="T:System.Threading.ExecutionContext" /> that was captured when each callback was registered
		/// will be reestablished when the callback is invoked.
		/// </para>
		/// </remarks>
		/// <param name="throwOnFirstException">Specifies whether exceptions should immediately propagate.</param>
		/// <exception cref="T:System.AggregateException">An aggregate exception containing all the exceptions thrown
		/// by the registered callbacks on the associated <see cref="T:System.Threading.CancellationToken" />.</exception>
		/// <exception cref="T:System.ObjectDisposedException">This <see cref="T:System.Threading.CancellationTokenSource" /> has been disposed.</exception> 
		public void Cancel(bool throwOnFirstException)
		{
			ThrowIfDisposed();
			NotifyCancellation(throwOnFirstException);
		}

		/// <summary>
		/// Releases the resources used by this <see cref="T:System.Threading.CancellationTokenSource" />.
		/// </summary>
		/// <remarks>
		/// This method is not thread-safe for any other concurrent calls.
		/// </remarks>
		public void Dispose()
		{
			if (m_disposed)
			{
				return;
			}
			if (m_linkingRegistrations != null)
			{
				foreach (CancellationTokenRegistration linkingRegistration in m_linkingRegistrations)
				{
					linkingRegistration.Dispose();
				}
				m_linkingRegistrations = null;
			}
			m_registeredCallbacksLists = null;
			if (m_kernelEvent != null)
			{
				m_kernelEvent.Close();
				m_kernelEvent = null;
			}
			m_disposed = true;
		}

		/// <summary>
		/// Throws an exception if the source has been disposed.
		/// </summary>
		internal void ThrowIfDisposed()
		{
			if (m_disposed)
			{
				throw new ObjectDisposedException(null, Environment2.GetResourceString("CancellationTokenSource_Disposed"));
			}
		}

		/// <summary>
		/// InternalGetStaticSource()
		/// </summary>
		/// <param name="set">Whether the source should be set.</param>
		/// <returns>A static source to be shared among multiple tokens.</returns>
		internal static CancellationTokenSource InternalGetStaticSource(bool set)
		{
			if (!set)
			{
				return _staticSource_NotCancelable;
			}
			return _staticSource_Set;
		}

		/// <summary>
		/// Registers a callback object. If cancellation has already occurred, the
		/// callback will have been run by the time this method returns.
		/// </summary>
		internal CancellationTokenRegistration InternalRegister(Action<object> callback, object stateForCallback, SynchronizationContext targetSyncContext, ExecutionContext executionContext)
		{
			ThrowIfDisposed();
			if (!IsCancellationRequested)
			{
				int num = Thread.CurrentThread.ManagedThreadId % s_nLists;
				CancellationCallbackInfo cancellationCallbackInfo = new CancellationCallbackInfo(callback, stateForCallback, targetSyncContext, executionContext, this);
				if (m_registeredCallbacksLists == null)
				{
					SparselyPopulatedArray<CancellationCallbackInfo>[] value = new SparselyPopulatedArray<CancellationCallbackInfo>[s_nLists];
					Interlocked.CompareExchange(ref m_registeredCallbacksLists, value, null);
				}
				if (m_registeredCallbacksLists[num] == null)
				{
					SparselyPopulatedArray<CancellationCallbackInfo> value2 = new SparselyPopulatedArray<CancellationCallbackInfo>(4);
					Interlocked.CompareExchange(ref m_registeredCallbacksLists[num], value2, null);
				}
				SparselyPopulatedArray<CancellationCallbackInfo> sparselyPopulatedArray = m_registeredCallbacksLists[num];
				SparselyPopulatedArrayAddInfo<CancellationCallbackInfo> registrationInfo = sparselyPopulatedArray.Add(cancellationCallbackInfo);
				CancellationTokenRegistration result = new CancellationTokenRegistration(this, cancellationCallbackInfo, registrationInfo);
				if (!IsCancellationRequested)
				{
					return result;
				}
				if (!result.TryDeregister())
				{
					WaitForCallbackToComplete(cancellationCallbackInfo);
					return default(CancellationTokenRegistration);
				}
			}
			callback(stateForCallback);
			return default(CancellationTokenRegistration);
		}

		/// <summary>
		///
		/// </summary>
		private void NotifyCancellation(bool throwOnFirstException)
		{
			if (!IsCancellationRequested && Interlocked.CompareExchange(ref m_state, 2, 1) == 1)
			{
				ThreadIDExecutingCallbacks = Thread.CurrentThread.ManagedThreadId;
				if (m_kernelEvent != null)
				{
					m_kernelEvent.Set();
				}
				ExecuteCallbackHandlers(throwOnFirstException);
			}
		}

		/// <summary>
		/// Invoke the Canceled event.
		/// </summary>
		/// <remarks>
		/// The handlers are invoked synchronously in LIFO order.
		/// </remarks>
		private void ExecuteCallbackHandlers(bool throwOnFirstException)
		{
			List<Exception> list = null;
			SparselyPopulatedArray<CancellationCallbackInfo>[] registeredCallbacksLists = m_registeredCallbacksLists;
			if (registeredCallbacksLists == null)
			{
				Interlocked.Exchange(ref m_state, 3);
				return;
			}
			try
			{
				foreach (SparselyPopulatedArray<CancellationCallbackInfo> sparselyPopulatedArray in registeredCallbacksLists)
				{
					if (sparselyPopulatedArray == null)
					{
						continue;
					}
					for (SparselyPopulatedArrayFragment<CancellationCallbackInfo> sparselyPopulatedArrayFragment = sparselyPopulatedArray.Tail; sparselyPopulatedArrayFragment != null; sparselyPopulatedArrayFragment = sparselyPopulatedArrayFragment.Prev)
					{
						for (int num = sparselyPopulatedArrayFragment.Length - 1; num >= 0; num--)
						{
							m_executingCallback = sparselyPopulatedArrayFragment[num];
							if (m_executingCallback != null)
							{
								CancellationCallbackCoreWorkArguments cancellationCallbackCoreWorkArguments = new CancellationCallbackCoreWorkArguments(sparselyPopulatedArrayFragment, num);
								try
								{
									if (m_executingCallback.TargetSyncContext != null)
									{
										m_executingCallback.TargetSyncContext.Send(CancellationCallbackCoreWork_OnSyncContext, cancellationCallbackCoreWorkArguments);
										ThreadIDExecutingCallbacks = Thread.CurrentThread.ManagedThreadId;
									}
									else
									{
										CancellationCallbackCoreWork_OnSyncContext(cancellationCallbackCoreWorkArguments);
									}
								}
								catch (Exception item)
								{
									if (throwOnFirstException)
									{
										throw;
									}
									if (list == null)
									{
										list = new List<Exception>();
									}
									list.Add(item);
								}
							}
						}
					}
				}
			}
			finally
			{
				m_state = 3;
				m_executingCallback = null;
				Thread.MemoryBarrier();
			}
			if (list == null)
			{
				return;
			}
			throw new AggregateException(list);
		}

		private void CancellationCallbackCoreWork_OnSyncContext(object obj)
		{
			CancellationCallbackCoreWorkArguments cancellationCallbackCoreWorkArguments = (CancellationCallbackCoreWorkArguments)obj;
			CancellationCallbackInfo cancellationCallbackInfo = cancellationCallbackCoreWorkArguments.m_currArrayFragment.SafeAtomicRemove(cancellationCallbackCoreWorkArguments.m_currArrayIndex, m_executingCallback);
			if (cancellationCallbackInfo == m_executingCallback)
			{
				if (cancellationCallbackInfo.TargetExecutionContext != null)
				{
					cancellationCallbackInfo.CancellationTokenSource.ThreadIDExecutingCallbacks = Thread.CurrentThread.ManagedThreadId;
				}
				cancellationCallbackInfo.ExecuteCallback();
			}
		}

		/// <summary>
		/// Creates a <see cref="T:System.Threading.CancellationTokenSource">CancellationTokenSource</see> that will be in the canceled state
		/// when any of the source tokens are in the canceled state.
		/// </summary>
		/// <param name="token1">The first <see cref="T:System.Threading.CancellationToken">CancellationToken</see> to observe.</param>
		/// <param name="token2">The second <see cref="T:System.Threading.CancellationToken">CancellationToken</see> to observe.</param>
		/// <returns>A <see cref="T:System.Threading.CancellationTokenSource">CancellationTokenSource</see> that is linked 
		/// to the source tokens.</returns>
		/// <exception cref="T:System.ObjectDisposedException">A <see cref="T:System.Threading.CancellationTokenSource">CancellationTokenSource</see> associated with
		/// one of the source tokens has been disposed.</exception>
		public static CancellationTokenSource CreateLinkedTokenSource(CancellationToken token1, CancellationToken token2)
		{
			CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
			if (token1.CanBeCanceled)
			{
				cancellationTokenSource.m_linkingRegistrations = new List<CancellationTokenRegistration>();
				cancellationTokenSource.m_linkingRegistrations.Add(token1.InternalRegisterWithoutEC(s_LinkedTokenCancelDelegate, cancellationTokenSource));
			}
			if (token2.CanBeCanceled)
			{
				if (cancellationTokenSource.m_linkingRegistrations == null)
				{
					cancellationTokenSource.m_linkingRegistrations = new List<CancellationTokenRegistration>();
				}
				cancellationTokenSource.m_linkingRegistrations.Add(token2.InternalRegisterWithoutEC(s_LinkedTokenCancelDelegate, cancellationTokenSource));
			}
			return cancellationTokenSource;
		}

		/// <summary>
		/// Creates a <see cref="T:System.Threading.CancellationTokenSource">CancellationTokenSource</see> that will be in the canceled state
		/// when any of the source tokens are in the canceled state.
		/// </summary>
		/// <param name="tokens">The <see cref="T:System.Threading.CancellationToken">CancellationToken</see> instances to observe.</param>
		/// <returns>A <see cref="T:System.Threading.CancellationTokenSource">CancellationTokenSource</see> that is linked 
		/// to the source tokens.</returns>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="tokens" /> is null.</exception>
		/// <exception cref="T:System.ObjectDisposedException">A <see cref="T:System.Threading.CancellationTokenSource">CancellationTokenSource</see> associated with
		/// one of the source tokens has been disposed.</exception>
		public static CancellationTokenSource CreateLinkedTokenSource(params CancellationToken[] tokens)
		{
			if (tokens == null)
			{
				throw new ArgumentNullException("tokens");
			}
			if (tokens.Length == 0)
			{
				throw new ArgumentException(Environment2.GetResourceString("CancellationToken_CreateLinkedToken_TokensIsEmpty"));
			}
			CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
			cancellationTokenSource.m_linkingRegistrations = new List<CancellationTokenRegistration>();
			for (int i = 0; i < tokens.Length; i++)
			{
				if (tokens[i].CanBeCanceled)
				{
					cancellationTokenSource.m_linkingRegistrations.Add(tokens[i].InternalRegisterWithoutEC(s_LinkedTokenCancelDelegate, cancellationTokenSource));
				}
			}
			return cancellationTokenSource;
		}

		internal void WaitForCallbackToComplete(CancellationCallbackInfo callbackInfo)
		{
			SpinWait spinWait = default(SpinWait);
			while (ExecutingCallback == callbackInfo)
			{
				spinWait.SpinOnce();
			}
		}
	}
}
