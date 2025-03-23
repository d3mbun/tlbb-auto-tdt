using System.Diagnostics;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Threading
{
	/// <summary>
	/// Provides a mutual exclusion lock primitive where a thread trying to acquire the lock waits in a loop
	/// repeatedly checking until the lock becomes available.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Spin locks can be used for leaf-level locks where the object allocation implied by using a <see cref="T:System.Threading.Monitor" />, in size or due to garbage collection pressure, is overly
	/// expensive. Avoiding blocking is another reason that a spin lock can be useful, however if you expect
	/// any significant amount of blocking, you are probably best not using spin locks due to excessive
	/// spinning. Spinning can be beneficial when locks are fine grained and large in number (for example, a
	/// lock per node in a linked list) as well as when lock hold times are always extremely short. In
	/// general, while holding a spin lock, one should avoid blocking, calling anything that itself may
	/// block, holding more than one spin lock at once, making dynamically dispatched calls (interface and
	/// virtuals), making statically dispatched calls into any code one doesn't own, or allocating memory.
	/// </para>
	/// <para>
	/// <see cref="T:System.Threading.SpinLock" /> should only be used when it's been determined that doing so will improve an
	/// application's performance. It's also important to note that <see cref="T:System.Threading.SpinLock" /> is a value type,
	/// for performance reasons. As such, one must be very careful not to accidentally copy a SpinLock
	/// instance, as the two instances (the original and the copy) would then be completely independent of
	/// one another, which would likely lead to erroneous behavior of the application. If a SpinLock instance
	/// must be passed around, it should be passed by reference rather than by value.
	/// </para>
	/// <para>
	/// Do not store <see cref="T:System.Threading.SpinLock" /> instances in readonly fields.
	/// </para>
	/// <para>
	/// All members of <see cref="T:System.Threading.SpinLock" /> are thread-safe and may be used from multiple threads
	/// concurrently.
	/// </para>
	/// </remarks>
	[DebuggerDisplay("IsHeld = {IsHeld}")]
	[ComVisible(false)]
	[DebuggerTypeProxy(typeof(SystemThreading_SpinLockDebugView))]
	[HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
	public struct SpinLock
	{
		/// <summary>
		/// Internal class used by debug type proxy attribute to display the owner thread ID 
		/// </summary>
		internal class SystemThreading_SpinLockDebugView
		{
			private SpinLock m_spinLock;

			/// <summary>
			/// Checks if the lock is held by the current thread or not
			/// </summary>
			public bool? IsHeldByCurrentThread
			{
				get
				{
					try
					{
						return m_spinLock.IsHeldByCurrentThread;
					}
					catch (InvalidOperationException)
					{
						return null;
					}
				}
			}

			/// <summary>
			/// Gets the current owner thread, zero if it is released
			/// </summary>
			public int? OwnerThreadID
			{
				get
				{
					if (m_spinLock.IsThreadOwnerTrackingEnabled)
					{
						return m_spinLock.m_owner;
					}
					return null;
				}
			}

			/// <summary>
			///  Gets whether the lock is currently held by any thread or not.
			/// </summary>
			public bool IsHeld => m_spinLock.IsHeld;

			/// <summary>
			/// SystemThreading_SpinLockDebugView constructor
			/// </summary>
			/// <param name="spinLock">The SpinLock to be proxied.</param>
			public SystemThreading_SpinLockDebugView(SpinLock spinLock)
			{
				m_spinLock = spinLock;
			}
		}

		private const int SPINNING_FACTOR = 100;

		private const int SLEEP_ONE_FREQUENCY = 40;

		private const int SLEEP_ZERO_FREQUENCY = 10;

		private const int TIMEOUT_CHECK_FREQUENCY = 10;

		private const int LOCK_ID_DISABLE_MASK = int.MinValue;

		private const int LOCK_ANONYMOUS_OWNED = 1;

		private const int WAITERS_MASK = 2147483646;

		private const int LOCK_UNOWNED = 0;

		private volatile int m_owner;

		private static int MAXIMUM_WAITERS = 2147483646;

		/// <summary>
		/// Gets whether the lock is currently held by any thread.
		/// </summary>
		public bool IsHeld
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			get
			{
				if (IsThreadOwnerTrackingEnabled)
				{
					return m_owner != 0;
				}
				return (m_owner & 1) != 0;
			}
		}

		/// <summary>
		/// Gets whether the lock is currently held by any thread.
		/// </summary>
		/// <summary>
		/// Gets whether the lock is held by the current thread.
		/// </summary>
		/// <remarks>
		/// If the lock was initialized to track owner threads, this will return whether the lock is acquired
		/// by the current thread. It is invalid to use this property when the lock was initialized to not
		/// track thread ownership.
		/// </remarks>
		/// <exception cref="T:System.InvalidOperationException">
		/// Thread ownership tracking is disabled.
		/// </exception>
		public bool IsHeldByCurrentThread
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			get
			{
				if (!IsThreadOwnerTrackingEnabled)
				{
					throw new InvalidOperationException(Environment2.GetResourceString("SpinLock_IsHeldByCurrentThread"));
				}
				return (m_owner & 0x7FFFFFFF) == Thread.CurrentThread.ManagedThreadId;
			}
		}

		/// <summary>Gets whether thread ownership tracking is enabled for this instance.</summary>
		public bool IsThreadOwnerTrackingEnabled
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			get
			{
				return (m_owner & int.MinValue) == 0;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.Threading.SpinLock" />
		/// structure with the option to track thread IDs to improve debugging.
		/// </summary>
		/// <remarks>
		/// The default constructor for <see cref="T:System.Threading.SpinLock" /> tracks thread ownership.
		/// </remarks>
		/// <param name="enableThreadOwnerTracking">Whether to capture and use thread IDs for debugging
		/// purposes.</param>
		public SpinLock(bool enableThreadOwnerTracking)
		{
			m_owner = 0;
			if (!enableThreadOwnerTracking)
			{
				m_owner |= int.MinValue;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.Threading.SpinLock" />
		/// structure with the option to track thread IDs to improve debugging.
		/// </summary>
		/// <remarks>
		/// The default constructor for <see cref="T:System.Threading.SpinLock" /> tracks thread ownership.
		/// </remarks>
		/// <summary>
		/// Acquires the lock in a reliable manner, such that even if an exception occurs within the method
		/// call, <paramref name="lockTaken" /> can be examined reliably to determine whether the lock was
		/// acquired.
		/// </summary>
		/// <remarks>
		/// <see cref="T:System.Threading.SpinLock" /> is a non-reentrant lock, meaning that if a thread holds the lock, it is
		/// not allowed to enter the lock again. If thread ownership tracking is enabled (whether it's
		/// enabled is available through <see cref="P:System.Threading.SpinLock.IsThreadOwnerTrackingEnabled" />), an exception will be
		/// thrown when a thread tries to re-enter a lock it already holds. However, if thread ownership
		/// tracking is disabled, attempting to enter a lock already held will result in deadlock.
		/// </remarks>
		/// <param name="lockTaken">True if the lock is acquired; otherwise, false. <paramref name="lockTaken" /> must be initialized to false prior to calling this method.</param>
		/// <exception cref="T:System.Threading.LockRecursionException">
		/// Thread ownership tracking is enabled, and the current thread has already acquired this lock.
		/// </exception>
		/// <exception cref="T:System.ArgumentException">
		/// The <paramref name="lockTaken" /> argument must be initialized to false prior to calling Enter.
		/// </exception>
		public void Enter(ref bool lockTaken)
		{
			if (lockTaken)
			{
				lockTaken = false;
				throw new ArgumentException(Environment2.GetResourceString("SpinLock_TryReliableEnter_ArgumentException"));
			}
			int owner = m_owner;
			int num = 0;
			if ((m_owner & int.MinValue) == 0)
			{
				if (owner == 0)
				{
					num = Thread.CurrentThread.ManagedThreadId;
				}
			}
			else if ((owner & 1) == 0)
			{
				num = owner | 1;
			}
			if (num != 0)
			{
				Thread.BeginCriticalRegion();
				if (Interlocked.CompareExchange(ref m_owner, num, owner) == owner)
				{
					lockTaken = true;
					return;
				}
				Thread.EndCriticalRegion();
			}
			ContinueTryEnter(-1, ref lockTaken);
		}

		/// <summary>
		/// Attempts to acquire the lock in a reliable manner, such that even if an exception occurs within
		/// the method call, <paramref name="lockTaken" /> can be examined reliably to determine whether the
		/// lock was acquired.
		/// </summary>
		/// <remarks>
		/// Unlike <see cref="M:System.Threading.SpinLock.Enter(System.Boolean@)" />, TryEnter will not block waiting for the lock to be available. If the
		/// lock is not available when TryEnter is called, it will return immediately without any further
		/// spinning.
		/// </remarks>
		/// <param name="lockTaken">True if the lock is acquired; otherwise, false. <paramref name="lockTaken" /> must be initialized to false prior to calling this method.</param>
		/// <exception cref="T:System.Threading.LockRecursionException">
		/// Thread ownership tracking is enabled, and the current thread has already acquired this lock.
		/// </exception>
		/// <exception cref="T:System.ArgumentException">
		/// The <paramref name="lockTaken" /> argument must be initialized to false prior to calling TryEnter.
		/// </exception>
		public void TryEnter(ref bool lockTaken)
		{
			TryEnter(0, ref lockTaken);
		}

		/// <summary>
		/// Attempts to acquire the lock in a reliable manner, such that even if an exception occurs within
		/// the method call, <paramref name="lockTaken" /> can be examined reliably to determine whether the
		/// lock was acquired.
		/// </summary>
		/// <remarks>
		/// Unlike <see cref="M:System.Threading.SpinLock.Enter(System.Boolean@)" />, TryEnter will not block indefinitely waiting for the lock to be
		/// available. It will block until either the lock is available or until the <paramref name="timeout" />
		/// has expired.
		/// </remarks>
		/// <param name="timeout">A <see cref="T:System.TimeSpan" /> that represents the number of milliseconds
		/// to wait, or a <see cref="T:System.TimeSpan" /> that represents -1 milliseconds to wait indefinitely.
		/// </param>
		/// <param name="lockTaken">True if the lock is acquired; otherwise, false. <paramref name="lockTaken" /> must be initialized to false prior to calling this method.</param>
		/// <exception cref="T:System.Threading.LockRecursionException">
		/// Thread ownership tracking is enabled, and the current thread has already acquired this lock.
		/// </exception>
		/// <exception cref="T:System.ArgumentException">
		/// The <paramref name="lockTaken" /> argument must be initialized to false prior to calling TryEnter.
		/// </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="timeout" /> is a negative
		/// number other than -1 milliseconds, which represents an infinite time-out -or- timeout is greater
		/// than <see cref="F:System.Int32.MaxValue" /> milliseconds.
		/// </exception>
		public void TryEnter(TimeSpan timeout, ref bool lockTaken)
		{
			long num = (long)timeout.TotalMilliseconds;
			if (num < -1 || num > int.MaxValue)
			{
				throw new ArgumentOutOfRangeException("timeout", timeout, Environment2.GetResourceString("SpinLock_TryEnter_ArgumentOutOfRange"));
			}
			TryEnter((int)timeout.TotalMilliseconds, ref lockTaken);
		}

		/// <summary>
		/// Attempts to acquire the lock in a reliable manner, such that even if an exception occurs within
		/// the method call, <paramref name="lockTaken" /> can be examined reliably to determine whether the
		/// lock was acquired.
		/// </summary>
		/// <remarks>
		/// Unlike <see cref="M:System.Threading.SpinLock.Enter(System.Boolean@)" />, TryEnter will not block indefinitely waiting for the lock to be
		/// available. It will block until either the lock is available or until the <paramref name="millisecondsTimeout" /> has expired.
		/// </remarks>
		/// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="F:System.Threading.Timeout.Infinite" /> (-1) to wait indefinitely.</param>
		/// <param name="lockTaken">True if the lock is acquired; otherwise, false. <paramref name="lockTaken" /> must be initialized to false prior to calling this method.</param>
		/// <exception cref="T:System.Threading.LockRecursionException">
		/// Thread ownership tracking is enabled, and the current thread has already acquired this lock.
		/// </exception>
		/// <exception cref="T:System.ArgumentException">
		/// The <paramref name="lockTaken" /> argument must be initialized to false prior to calling TryEnter.
		/// </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="millisecondsTimeout" /> is
		/// a negative number other than -1, which represents an infinite time-out.</exception>
		public void TryEnter(int millisecondsTimeout, ref bool lockTaken)
		{
			if (lockTaken)
			{
				lockTaken = false;
				throw new ArgumentException(Environment2.GetResourceString("SpinLock_TryReliableEnter_ArgumentException"));
			}
			if (millisecondsTimeout < -1)
			{
				throw new ArgumentOutOfRangeException("millisecondsTimeout", millisecondsTimeout, Environment2.GetResourceString("SpinLock_TryEnter_ArgumentOutOfRange"));
			}
			int owner = m_owner;
			int num = 0;
			if (IsThreadOwnerTrackingEnabled)
			{
				if (owner == 0)
				{
					num = Thread.CurrentThread.ManagedThreadId;
				}
			}
			else if ((owner & 1) == 0)
			{
				num = owner | 1;
			}
			if (num != 0)
			{
				Thread.BeginCriticalRegion();
				if (Interlocked.CompareExchange(ref m_owner, num, owner) == owner)
				{
					lockTaken = true;
					return;
				}
				Thread.EndCriticalRegion();
			}
			ContinueTryEnter(millisecondsTimeout, ref lockTaken);
		}

		/// <summary>
		/// Try acquire the lock with long path, this is usually called after the first path in Enter and
		/// TryEnter failed The reason for short path is to make it inline in the run time which improves the
		/// performance. This method assumed that the parameter are validated in Enter ir TryENter method
		/// </summary>
		/// <param name="millisecondsTimeout">The timeout milliseconds</param>
		/// <param name="lockTaken">The lockTaken param</param>
		private void ContinueTryEnter(int millisecondsTimeout, ref bool lockTaken)
		{
			long startTicks = 0L;
			if (millisecondsTimeout != -1 && millisecondsTimeout != 0)
			{
				startTicks = DateTime.UtcNow.Ticks;
			}
			if (IsThreadOwnerTrackingEnabled)
			{
				ContinueTryEnterWithThreadTracking(millisecondsTimeout, startTicks, ref lockTaken);
				return;
			}
			SpinWait spinWait = default(SpinWait);
			int owner;
			while (true)
			{
				owner = m_owner;
				if ((owner & 1) == 0)
				{
					Thread.BeginCriticalRegion();
					if (Interlocked.CompareExchange(ref m_owner, owner | 1, owner) == owner)
					{
						lockTaken = true;
						return;
					}
					Thread.EndCriticalRegion();
				}
				else if ((owner & 0x7FFFFFFE) == MAXIMUM_WAITERS || Interlocked.CompareExchange(ref m_owner, owner + 2, owner) == owner)
				{
					break;
				}
				spinWait.SpinOnce();
			}
			if (millisecondsTimeout == 0 || (millisecondsTimeout != -1 && TimeoutExpired(startTicks, millisecondsTimeout)))
			{
				DecrementWaiters();
				return;
			}
			int num = ((owner + 2) & 0x7FFFFFFE) / 2;
			int processorCount = PlatformHelper.ProcessorCount;
			if (num < processorCount)
			{
				int num2 = 1;
				for (int i = 1; i <= num * 100; i++)
				{
					Thread.SpinWait((num + i) * 100 * num2);
					if (num2 < processorCount)
					{
						num2++;
					}
					owner = m_owner;
					if ((owner & 1) == 0)
					{
						Thread.BeginCriticalRegion();
						int value = (((owner & 0x7FFFFFFE) == 0) ? (owner | 1) : ((owner - 2) | 1));
						if (Interlocked.CompareExchange(ref m_owner, value, owner) == owner)
						{
							lockTaken = true;
							return;
						}
						Thread.EndCriticalRegion();
					}
				}
			}
			if (millisecondsTimeout != -1 && TimeoutExpired(startTicks, millisecondsTimeout))
			{
				DecrementWaiters();
				return;
			}
			int num3 = 0;
			while (true)
			{
				owner = m_owner;
				if ((owner & 1) == 0)
				{
					Thread.BeginCriticalRegion();
					int value2 = (((owner & 0x7FFFFFFE) == 0) ? (owner | 1) : ((owner - 2) | 1));
					if (Interlocked.CompareExchange(ref m_owner, value2, owner) == owner)
					{
						lockTaken = true;
						return;
					}
					Thread.EndCriticalRegion();
				}
				if (num3 % 40 == 0)
				{
					Thread.Sleep(1);
				}
				else if (num3 % 10 == 0)
				{
					Thread.Sleep(0);
				}
				else
				{
					Platform.Yield();
				}
				if (num3 % 10 == 0 && millisecondsTimeout != -1 && TimeoutExpired(startTicks, millisecondsTimeout))
				{
					break;
				}
				num3++;
			}
			DecrementWaiters();
		}

		/// <summary>
		/// decrements the waiters, in case of the timeout is expired
		/// </summary>
		private void DecrementWaiters()
		{
			SpinWait spinWait = default(SpinWait);
			while (true)
			{
				int owner = m_owner;
				if ((owner & 0x7FFFFFFE) == 0 || Interlocked.CompareExchange(ref m_owner, owner - 2, owner) == owner)
				{
					break;
				}
				spinWait.SpinOnce();
			}
		}

		/// <summary>
		/// ContinueTryEnter for the thread tracking mode enabled
		/// </summary>
		private void ContinueTryEnterWithThreadTracking(int millisecondsTimeout, long startTicks, ref bool lockTaken)
		{
			int num = 0;
			int managedThreadId = Thread.CurrentThread.ManagedThreadId;
			if (m_owner == managedThreadId)
			{
				throw new LockRecursionException(Environment2.GetResourceString("SpinLock_TryEnter_LockRecursionException"));
			}
			SpinWait spinWait = default(SpinWait);
			while (true)
			{
				spinWait.SpinOnce();
				if (m_owner == num)
				{
					Thread.BeginCriticalRegion();
					if (Interlocked.CompareExchange(ref m_owner, managedThreadId, num) == num)
					{
						lockTaken = true;
						break;
					}
					Thread.EndCriticalRegion();
				}
				switch (millisecondsTimeout)
				{
				case -1:
					continue;
				case 0:
					return;
				}
				if (spinWait.NextSpinWillYield && TimeoutExpired(startTicks, millisecondsTimeout))
				{
					return;
				}
			}
		}

		/// <summary>
		/// Helper function to validate the timeout
		/// </summary>
		/// <param name="startTicks"> The start time in ticks</param>
		/// <param name="originalWaitTime">The orginal wait time</param>
		/// <returns>True if expired, false otherwise</returns>
		private static bool TimeoutExpired(long startTicks, int originalWaitTime)
		{
			long num = DateTime.UtcNow.Ticks - startTicks;
			return num >= (long)originalWaitTime * 10000L;
		}

		/// <summary>
		/// Releases the lock.
		/// </summary>
		/// <remarks>
		/// The default overload of <see cref="M:System.Threading.SpinLock.Exit" /> provides the same behavior as if calling <see cref="M:System.Threading.SpinLock.Exit(System.Boolean)" /> using true as the argument.
		/// </remarks>
		/// <exception cref="T:System.Threading.SynchronizationLockException">
		/// Thread ownership tracking is enabled, and the current thread is not the owner of this lock.
		/// </exception>
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public void Exit()
		{
			Exit(useMemoryBarrier: true);
		}

		/// <summary>
		/// Releases the lock.
		/// </summary>
		/// <param name="useMemoryBarrier">
		/// A Boolean value that indicates whether a memory fence should be issued in order to immediately
		/// publish the exit operation to other threads.
		/// </param>
		/// <remarks>
		/// Calling <see cref="M:System.Threading.SpinLock.Exit(System.Boolean)" /> with the <paramref name="useMemoryBarrier" /> argument set to
		/// true will improve the fairness of the lock at the expense of some performance. The default <see cref="M:System.Threading.SpinLock.Enter(System.Boolean@)" />
		/// overload behaves as if specifying true for <paramref name="useMemoryBarrier" />.
		/// </remarks>
		/// <exception cref="T:System.Threading.SynchronizationLockException">
		/// Thread ownership tracking is enabled, and the current thread is not the owner of this lock.
		/// </exception>
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public void Exit(bool useMemoryBarrier)
		{
			if (IsThreadOwnerTrackingEnabled && !IsHeldByCurrentThread)
			{
				throw new SynchronizationLockException(Environment2.GetResourceString("SpinLock_Exit_SynchronizationLockException"));
			}
			if (useMemoryBarrier)
			{
				if (IsThreadOwnerTrackingEnabled)
				{
					Interlocked.Exchange(ref m_owner, 0);
				}
				else
				{
					Interlocked.Decrement(ref m_owner);
				}
			}
			else if (IsThreadOwnerTrackingEnabled)
			{
				m_owner = 0;
			}
			else
			{
				m_owner--;
			}
			Thread.EndCriticalRegion();
		}
	}
}
