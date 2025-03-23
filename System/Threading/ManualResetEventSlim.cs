using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Threading
{
	/// <summary>
	/// Provides a slimmed down version of <see cref="T:System.Threading.ManualResetEvent" />.
	/// </summary>
	/// <remarks>
	/// All public and protected members of <see cref="T:System.Threading.ManualResetEventSlim" /> are thread-safe and may be used
	/// concurrently from multiple threads, with the exception of Dispose, which
	/// must only be used when all other operations on the <see cref="T:System.Threading.ManualResetEventSlim" /> have
	/// completed, and Reset, which should only be used when no other threads are
	/// accessing the event.
	/// </remarks>
	[ComVisible(false)]
	[DebuggerDisplay("Set = {IsSet}")]
	[HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
	public class ManualResetEventSlim : IDisposable
	{
		private const int DEFAULT_SPIN_SP = 1;

		private const int DEFAULT_SPIN_MP = 10;

		private const int SignalledState_BitMask = int.MinValue;

		private const int SignalledState_ShiftCount = 31;

		private const int Dispose_BitMask = 1073741824;

		private const int SpinCountState_BitMask = 1073217536;

		private const int SpinCountState_ShiftCount = 19;

		private const int SpinCountState_MaxValue = 2047;

		private const int NumWaitersState_BitMask = 524287;

		private const int NumWaitersState_ShiftCount = 0;

		private const int NumWaitersState_MaxValue = 524287;

		private object m_lock;

		private ManualResetEvent m_eventObj;

		private volatile int m_combinedState;

		/// <summary>
		/// Private helper method to wake up waiters when a cancellationToken gets canceled.
		/// </summary>
		private static Action<object> s_cancellationTokenCallback = CancellationTokenCallback;

		/// <summary>
		/// Gets the underlying <see cref="T:System.Threading.WaitHandle" /> object for this <see cref="T:System.Threading.ManualResetEventSlim" />.
		/// </summary>
		/// <value>The underlying <see cref="T:System.Threading.WaitHandle" /> event object fore this <see cref="T:System.Threading.ManualResetEventSlim" />.</value>
		/// <remarks>
		/// Accessing this property forces initialization of an underlying event object if one hasn't
		/// already been created.  To simply wait on this <see cref="T:System.Threading.ManualResetEventSlim" />, 
		/// the public Wait methods should be preferred.
		/// </remarks>
		public WaitHandle WaitHandle
		{
			get
			{
				ThrowIfDisposed();
				if (m_eventObj == null)
				{
					LazyInitializeEvent();
				}
				return m_eventObj;
			}
		}

		/// <summary>
		/// Gets whether the event is set.
		/// </summary>
		/// <value>true if the event has is set; otherwise, false.</value>
		public bool IsSet
		{
			get
			{
				return 0 != ExtractStatePortion(m_combinedState, int.MinValue);
			}
			private set
			{
				UpdateStateAtomically((value ? 1 : 0) << 31, int.MinValue);
			}
		}

		/// <summary>
		/// Gets the number of spin waits that will be occur before falling back to a true wait.
		/// </summary>
		public int SpinCount
		{
			get
			{
				return ExtractStatePortionAndShiftRight(m_combinedState, 1073217536, 19);
			}
			private set
			{
				m_combinedState = (m_combinedState & -1073217537) | (value << 19);
			}
		}

		/// <summary>
		/// How many threads are waiting.
		/// </summary>
		private int Waiters
		{
			get
			{
				return ExtractStatePortionAndShiftRight(m_combinedState, 524287, 0);
			}
			set
			{
				if (value >= 524287)
				{
					throw new InvalidOperationException(string.Format(Environment2.GetResourceString("ManualResetEventSlim_ctor_TooManyWaiters"), 524287));
				}
				UpdateStateAtomically(value, 524287);
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.Threading.ManualResetEventSlim" />
		/// class with an initial state of nonsignaled.
		/// </summary>
		public ManualResetEventSlim()
			: this(initialState: false)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.Threading.ManualResetEventSlim" />
		/// class with a Boolen value indicating whether to set the intial state to signaled.
		/// </summary>
		/// <param name="initialState">true to set the initial state signaled; false to set the initial state
		/// to nonsignaled.</param>
		public ManualResetEventSlim(bool initialState)
		{
			Initialize(initialState, 10);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.Threading.ManualResetEventSlim" />
		/// class with a Boolen value indicating whether to set the intial state to signaled and a specified
		/// spin count.
		/// </summary>
		/// <param name="initialState">true to set the initial state to signaled; false to set the initial state
		/// to nonsignaled.</param>
		/// <param name="spinCount">The number of spin waits that will occur before falling back to a true
		/// wait.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="spinCount" /> is less than
		/// 0 or greater than the maximum allowed value.</exception>
		public ManualResetEventSlim(bool initialState, int spinCount)
		{
			if (spinCount < 0)
			{
				throw new ArgumentOutOfRangeException("spinCount");
			}
			if (spinCount > 2047)
			{
				throw new ArgumentOutOfRangeException("spinCount", string.Format(Environment2.GetResourceString("ManualResetEventSlim_ctor_SpinCountOutOfRange"), 2047));
			}
			Initialize(initialState, spinCount);
		}

		/// <summary>
		/// Initializes the internal state of the event.
		/// </summary>
		/// <param name="initialState">Whether the event is set initially or not.</param>
		/// <param name="spinCount">The spin count that decides when the event will block.</param>
		private void Initialize(bool initialState, int spinCount)
		{
			IsSet = initialState;
			SpinCount = (PlatformHelper.IsSingleProcessor ? 1 : spinCount);
		}

		/// <summary>
		/// Helper to ensure the lock object is created before first use.
		/// </summary>
		private void EnsureLockObjectCreated()
		{
			if (m_lock == null)
			{
				object value = new object();
				Interlocked.CompareExchange(ref m_lock, value, null);
			}
		}

		/// <summary>
		/// This method lazily initializes the event object. It uses CAS to guarantee that
		/// many threads racing to call this at once don't result in more than one event
		/// being stored and used. The event will be signaled or unsignaled depending on
		/// the state of the thin-event itself, with synchronization taken into account.
		/// </summary>
		/// <returns>True if a new event was created and stored, false otherwise.</returns>
		private bool LazyInitializeEvent()
		{
			bool isSet = IsSet;
			ManualResetEvent manualResetEvent = new ManualResetEvent(isSet);
			if (Interlocked.CompareExchange(ref m_eventObj, manualResetEvent, null) != null)
			{
				manualResetEvent.Close();
				return false;
			}
			bool isSet2 = IsSet;
			if (isSet2 != isSet)
			{
				lock (manualResetEvent)
				{
					if (m_eventObj == manualResetEvent)
					{
						manualResetEvent.Set();
					}
				}
			}
			return true;
		}

		/// <summary>
		/// Sets the state of the event to signaled, which allows one or more threads waiting on the event to
		/// proceed.
		/// </summary>
		public void Set()
		{
			Set(duringCancellation: false);
		}

		/// <summary>
		/// Private helper to actually perform the Set.
		/// </summary>
		/// <param name="duringCancellation">Indicates whether we are calling Set() during cancellation.</param>
		/// <exception cref="T:System.OperationCanceledException">The object has been canceled.</exception>
		private void Set(bool duringCancellation)
		{
			IsSet = true;
			if (Waiters > 0)
			{
				lock (m_lock)
				{
					Monitor.PulseAll(m_lock);
				}
			}
			ManualResetEvent eventObj = m_eventObj;
			if (eventObj == null || duringCancellation)
			{
				return;
			}
			lock (eventObj)
			{
				if (m_eventObj != null)
				{
					m_eventObj.Set();
				}
			}
		}

		/// <summary>
		/// Sets the state of the event to nonsignaled, which causes threads to block.
		/// </summary>
		/// <remarks>
		/// Unlike most of the members of <see cref="T:System.Threading.ManualResetEventSlim" />, <see cref="M:System.Threading.ManualResetEventSlim.Reset" /> is not
		/// thread-safe and may not be used concurrently with other members of this instance.
		/// </remarks>
		public void Reset()
		{
			ThrowIfDisposed();
			if (m_eventObj != null)
			{
				m_eventObj.Reset();
			}
			IsSet = false;
		}

		/// <summary>
		/// Blocks the current thread until the current <see cref="T:System.Threading.ManualResetEventSlim" /> is set.
		/// </summary>
		/// <exception cref="T:System.InvalidOperationException">
		/// The maximum number of waiters has been exceeded.
		/// </exception>
		/// <remarks>
		/// The caller of this method blocks indefinitely until the current instance is set. The caller will
		/// return immediately if the event is currently in a set state.
		/// </remarks>
		public void Wait()
		{
			Wait(-1, default(CancellationToken));
		}

		/// <summary>
		/// Blocks the current thread until the current <see cref="T:System.Threading.ManualResetEventSlim" /> receives a signal,
		/// while observing a <see cref="T:System.Threading.CancellationToken" />.
		/// </summary>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> to
		/// observe.</param>
		/// <exception cref="T:System.InvalidOperationException">
		/// The maximum number of waiters has been exceeded.
		/// </exception>
		/// <exception cref="T:System.OperationCanceledExcepton"><paramref name="cancellationToken" /> was
		/// canceled.</exception>
		/// <remarks>
		/// The caller of this method blocks indefinitely until the current instance is set. The caller will
		/// return immediately if the event is currently in a set state.
		/// </remarks>
		public void Wait(CancellationToken cancellationToken)
		{
			Wait(-1, cancellationToken);
		}

		/// <summary>
		/// Blocks the current thread until the current <see cref="T:System.Threading.ManualResetEventSlim" /> is set, using a
		/// <see cref="T:System.TimeSpan" /> to measure the time interval.
		/// </summary>
		/// <param name="timeout">A <see cref="T:System.TimeSpan" /> that represents the number of milliseconds
		/// to wait, or a <see cref="T:System.TimeSpan" /> that represents -1 milliseconds to wait indefinitely.
		/// </param>
		/// <returns>true if the <see cref="T:System.Threading.ManualResetEventSlim" /> was set; otherwise,
		/// false.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="timeout" /> is a negative
		/// number other than -1 milliseconds, which represents an infinite time-out -or- timeout is greater
		/// than <see cref="F:System.Int32.MaxValue" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		/// The maximum number of waiters has been exceeded.
		/// </exception>
		public bool Wait(TimeSpan timeout)
		{
			long num = (long)timeout.TotalMilliseconds;
			if (num < -1 || num > int.MaxValue)
			{
				throw new ArgumentOutOfRangeException("timeout");
			}
			return Wait((int)num, default(CancellationToken));
		}

		/// <summary>
		/// Blocks the current thread until the current <see cref="T:System.Threading.ManualResetEventSlim" /> is set, using a
		/// <see cref="T:System.TimeSpan" /> to measure the time interval, while observing a <see cref="T:System.Threading.CancellationToken" />.
		/// </summary>
		/// <param name="timeout">A <see cref="T:System.TimeSpan" /> that represents the number of milliseconds
		/// to wait, or a <see cref="T:System.TimeSpan" /> that represents -1 milliseconds to wait indefinitely.
		/// </param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> to
		/// observe.</param>
		/// <returns>true if the <see cref="T:System.Threading.ManualResetEventSlim" /> was set; otherwise,
		/// false.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="timeout" /> is a negative
		/// number other than -1 milliseconds, which represents an infinite time-out -or- timeout is greater
		/// than <see cref="F:System.Int32.MaxValue" />.</exception>
		/// <exception cref="T:System.Threading.OperationCanceledException"><paramref name="cancellationToken" /> was canceled.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		/// The maximum number of waiters has been exceeded.
		/// </exception>
		public bool Wait(TimeSpan timeout, CancellationToken cancellationToken)
		{
			long num = (long)timeout.TotalMilliseconds;
			if (num < -1 || num > int.MaxValue)
			{
				throw new ArgumentOutOfRangeException("timeout");
			}
			return Wait((int)num, cancellationToken);
		}

		/// <summary>
		/// Blocks the current thread until the current <see cref="T:System.Threading.ManualResetEventSlim" /> is set, using a
		/// 32-bit signed integer to measure the time interval.
		/// </summary>
		/// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="F:System.Threading.Timeout.Infinite" />(-1) to wait indefinitely.</param>
		/// <returns>true if the <see cref="T:System.Threading.ManualResetEventSlim" /> was set; otherwise,
		/// false.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="millisecondsTimeout" /> is a
		/// negative number other than -1, which represents an infinite time-out.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		/// The maximum number of waiters has been exceeded.
		/// </exception>
		public bool Wait(int millisecondsTimeout)
		{
			return Wait(millisecondsTimeout, default(CancellationToken));
		}

		/// <summary>
		/// Blocks the current thread until the current <see cref="T:System.Threading.ManualResetEventSlim" /> is set, using a
		/// 32-bit signed integer to measure the time interval, while observing a <see cref="T:System.Threading.CancellationToken" />.
		/// </summary>
		/// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="F:System.Threading.Timeout.Infinite" />(-1) to wait indefinitely.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> to
		/// observe.</param>
		/// <returns>true if the <see cref="T:System.Threading.ManualResetEventSlim" /> was set; otherwise,
		/// false.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="millisecondsTimeout" /> is a
		/// negative number other than -1, which represents an infinite time-out.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		/// The maximum number of waiters has been exceeded.
		/// </exception>
		/// <exception cref="T:System.Threading.OperationCanceledException"><paramref name="cancellationToken" /> was canceled.</exception>
		public bool Wait(int millisecondsTimeout, CancellationToken cancellationToken)
		{
			ThrowIfDisposed();
			cancellationToken.ThrowIfCancellationRequested();
			if (millisecondsTimeout < -1)
			{
				throw new ArgumentOutOfRangeException("millisecondsTimeout");
			}
			if (!IsSet)
			{
				if (millisecondsTimeout == 0)
				{
					return false;
				}
				long startTimeTicks = 0L;
				bool flag = false;
				int num = millisecondsTimeout;
				if (millisecondsTimeout != -1)
				{
					startTimeTicks = DateTime.UtcNow.Ticks;
					flag = true;
				}
				int num2 = 10;
				int num3 = 5;
				int num4 = 20;
				for (int i = 0; i < SpinCount; i++)
				{
					if (IsSet)
					{
						return true;
					}
					if (i < num2)
					{
						if (i == num2 / 2)
						{
							Platform.Yield();
						}
						else
						{
							Thread.SpinWait(Environment.ProcessorCount * (4 << i));
						}
					}
					else if (i % num4 == 0)
					{
						Thread.Sleep(1);
					}
					else if (i % num3 == 0)
					{
						Thread.Sleep(0);
					}
					else
					{
						Platform.Yield();
					}
					if (i >= 100 && i % 10 == 0)
					{
						cancellationToken.ThrowIfCancellationRequested();
					}
				}
				EnsureLockObjectCreated();
				using (cancellationToken.Register(s_cancellationTokenCallback, this))
				{
					lock (m_lock)
					{
						while (!IsSet)
						{
							cancellationToken.ThrowIfCancellationRequested();
							if (flag)
							{
								num = UpdateTimeOut(startTimeTicks, millisecondsTimeout);
								if (num <= 0)
								{
									return false;
								}
							}
							Waiters++;
							if (IsSet)
							{
								Waiters--;
								return true;
							}
							try
							{
								if (!Monitor.Wait(m_lock, num))
								{
									return false;
								}
							}
							finally
							{
								Waiters--;
							}
						}
					}
				}
			}
			return true;
		}

		/// <summary>
		/// Releases all resources used by the current instance of <see cref="T:System.Threading.ManualResetEventSlim" />.
		/// </summary>
		/// <remarks>
		/// Unlike most of the members of <see cref="T:System.Threading.ManualResetEventSlim" />, <see cref="M:System.Threading.ManualResetEventSlim.Dispose" /> is not
		/// thread-safe and may not be used concurrently with other members of this instance.
		/// </remarks>
		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// When overridden in a derived class, releases the unmanaged resources used by the 
		/// <see cref="T:System.Threading.ManualResetEventSlim" />, and optionally releases the managed resources.
		/// </summary>
		/// <param name="disposing">true to release both managed and unmanaged resources;
		/// false to release only unmanaged resources.</param>
		/// <remarks>
		/// Unlike most of the members of <see cref="T:System.Threading.ManualResetEventSlim" />, <see cref="M:System.Threading.ManualResetEventSlim.Dispose(System.Boolean)" /> is not
		/// thread-safe and may not be used concurrently with other members of this instance.
		/// </remarks>
		protected virtual void Dispose(bool disposing)
		{
			if (((uint)m_combinedState & 0x40000000u) != 0)
			{
				return;
			}
			m_combinedState |= 1073741824;
			if (!disposing)
			{
				return;
			}
			ManualResetEvent eventObj = m_eventObj;
			if (eventObj != null)
			{
				lock (eventObj)
				{
					eventObj.Close();
					m_eventObj = null;
				}
			}
		}

		/// <summary>
		/// Throw ObjectDisposedException if the MRES is disposed
		/// </summary>
		private void ThrowIfDisposed()
		{
			if (((uint)m_combinedState & 0x40000000u) != 0)
			{
				throw new ObjectDisposedException(Environment2.GetResourceString("ManualResetEventSlim_Disposed"));
			}
		}

		private static void CancellationTokenCallback(object obj)
		{
			ManualResetEventSlim manualResetEventSlim = obj as ManualResetEventSlim;
			lock (manualResetEventSlim.m_lock)
			{
				Monitor.PulseAll(manualResetEventSlim.m_lock);
			}
		}

		/// <summary>
		/// Private helper method for updating parts of a bit-string state value.
		/// Mainly called from the IsSet and Waiters properties setters
		/// </summary>
		/// <remarks>
		/// Note: the parameter types must be int as CompareExchange cannot take a Uint
		/// </remarks>
		/// <param name="newBits">The new value</param>
		/// <param name="updateBitsMask">The mask used to set the bits</param>
		private void UpdateStateAtomically(int newBits, int updateBitsMask)
		{
			SpinWait spinWait = default(SpinWait);
			while (true)
			{
				int combinedState = m_combinedState;
				int value = (combinedState & ~updateBitsMask) | newBits;
				if (Interlocked.CompareExchange(ref m_combinedState, value, combinedState) == combinedState)
				{
					break;
				}
				spinWait.SpinOnce();
			}
		}

		/// <summary>
		/// Private helper method - performs Mask and shift, particular helpful to extract a field from a packed word.
		/// eg ExtractStatePortionAndShiftRight(0x12345678, 0xFF000000, 24) =&gt; 0x12, ie extracting the top 8-bits as a simple integer 
		///
		/// ?? is there a common place to put this rather than being private to MRES?
		/// </summary>
		/// <param name="state"></param>
		/// <param name="mask"></param>
		/// <param name="rightBitShiftCount"></param>
		/// <returns></returns>
		private static int ExtractStatePortionAndShiftRight(int state, int mask, int rightBitShiftCount)
		{
			return (int)((uint)(state & mask) >> rightBitShiftCount);
		}

		/// <summary>
		/// Performs a Mask operation, but does not perform the shift.
		/// This is acceptable for boolean values for which the shift is unnecessary
		/// eg (val &amp; Mask) != 0 is an appropriate way to extract a boolean rather than using
		/// ((val &amp; Mask) &gt;&gt; shiftAmount) == 1
		///
		/// ?? is there a common place to put this rather than being private to MRES?
		/// </summary>
		/// <param name="state"></param>
		/// <param name="mask"></param>
		private static int ExtractStatePortion(int state, int mask)
		{
			return state & mask;
		}

		/// <summary>
		/// Helper function to measure and update the wait time
		/// </summary>
		/// <param name="startTimeTicks"> The first time (in Ticks) observed when the wait started.</param>
		/// <param name="originalWaitMillisecondsTimeout">The orginal wait timeoutout in milliseconds.</param>
		/// <returns>The new wait time in milliseconds, -1 if the time expired, -2 if overflow in counters
		/// has occurred.</returns>
		private static int UpdateTimeOut(long startTimeTicks, int originalWaitMillisecondsTimeout)
		{
			long num = (DateTime.UtcNow.Ticks - startTimeTicks) / 10000;
			if (num > int.MaxValue)
			{
				return -2;
			}
			int num2 = originalWaitMillisecondsTimeout - (int)num;
			if (num2 < 0)
			{
				return -1;
			}
			return num2;
		}
	}
}
