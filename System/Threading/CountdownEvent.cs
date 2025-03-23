using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Threading
{
	/// <summary>
	/// Represents a synchronization primitive that is signaled when its count reaches zero.
	/// </summary>
	/// <remarks>
	/// All public and protected members of <see cref="T:System.Threading.CountdownEvent" /> are thread-safe and may be used
	/// concurrently from multiple threads, with the exception of Dispose, which
	/// must only be used when all other operations on the <see cref="T:System.Threading.CountdownEvent" /> have
	/// completed, and Reset, which should only be used when no other threads are
	/// accessing the event.
	/// </remarks>
	[DebuggerDisplay("Initial Count={InitialCount}, Current Count={CurrentCount}")]
	[ComVisible(false)]
	[HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
	public class CountdownEvent : IDisposable
	{
		private int m_initialCount;

		private volatile int m_currentCount;

		private ManualResetEventSlim m_event;

		private volatile bool m_disposed;

		/// <summary>
		/// Gets the number of remaining signals required to set the event.
		/// </summary>
		/// <value>
		/// The number of remaining signals required to set the event.
		/// </value>
		public int CurrentCount => m_currentCount;

		/// <summary>
		/// Gets the numbers of signals initially required to set the event.
		/// </summary>
		/// <value>
		/// The number of signals initially required to set the event.
		/// </value>
		public int InitialCount => m_initialCount;

		/// <summary>
		/// Determines whether the event is set.
		/// </summary>
		/// <value>true if the event is set; otherwise, false.</value>
		public bool IsSet => m_currentCount == 0;

		/// <summary>
		/// Gets a <see cref="T:System.Threading.WaitHandle" /> that is used to wait for the event to be set. 
		/// </summary>
		/// <value>A <see cref="T:System.Threading.WaitHandle" /> that is used to wait for the event to be set.</value>
		/// <exception cref="T:System.ObjectDisposedException">The current instance has already been disposed.</exception>
		/// <remarks>
		/// <see cref="P:System.Threading.CountdownEvent.WaitHandle" /> should only be used if it's needed for integration with code bases
		/// that rely on having a WaitHandle.  If all that's needed is to wait for the <see cref="T:System.Threading.CountdownEvent" />
		/// to be set, the <see cref="M:System.Threading.CountdownEvent.Wait" /> method should be preferred.
		/// </remarks>
		public WaitHandle WaitHandle
		{
			get
			{
				ThrowIfDisposed();
				return m_event.WaitHandle;
			}
		}

		/// <summary>
		/// Initializes a new instance of <see cref="T:System.Threading.CountdownEvent" /> class with the
		/// specified count.
		/// </summary>
		/// <param name="initialCount">The number of signals required to set the <see cref="T:System.Threading.CountdownEvent" />.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="initialCount" /> is less
		/// than 0.</exception>
		public CountdownEvent(int initialCount)
		{
			if (initialCount < 0)
			{
				throw new ArgumentOutOfRangeException("initialCount");
			}
			m_initialCount = initialCount;
			m_currentCount = initialCount;
			m_event = new ManualResetEventSlim();
			if (initialCount == 0)
			{
				m_event.Set();
			}
		}

		/// <summary>
		/// Releases all resources used by the current instance of <see cref="T:System.Threading.CountdownEvent" />.
		/// </summary>
		/// <remarks>
		/// Unlike most of the members of <see cref="T:System.Threading.CountdownEvent" />, <see cref="M:System.Threading.CountdownEvent.Dispose" /> is not
		/// thread-safe and may not be used concurrently with other members of this instance.
		/// </remarks>
		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// When overridden in a derived class, releases the unmanaged resources used by the
		/// <see cref="T:System.Threading.CountdownEvent" />, and optionally releases the managed resources.
		/// </summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to release
		/// only unmanaged resources.</param>
		/// <remarks>
		/// Unlike most of the members of <see cref="T:System.Threading.CountdownEvent" />, <see cref="M:System.Threading.CountdownEvent.Dispose" /> is not
		/// thread-safe and may not be used concurrently with other members of this instance.
		/// </remarks>
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				m_event.Dispose();
				m_disposed = true;
			}
		}

		/// <summary>
		/// Registers a signal with the <see cref="T:System.Threading.CountdownEvent" />, decrementing its
		/// count.
		/// </summary>
		/// <returns>true if the signal caused the count to reach zero and the event was set; otherwise,
		/// false.</returns>
		/// <exception cref="T:System.InvalidOperationException">The current instance is already set.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">The current instance has already been
		/// disposed.</exception>
		public bool Signal()
		{
			return Signal(1);
		}

		/// <summary>
		/// Registers multiple signals with the <see cref="T:System.Threading.CountdownEvent" />,
		/// decrementing its count by the specified amount.
		/// </summary>
		/// <param name="signalCount">The number of signals to register.</param>
		/// <returns>true if the signals caused the count to reach zero and the event was set; otherwise,
		/// false.</returns>
		/// <exception cref="T:System.InvalidOperationException">
		/// The current instance is already set. -or- Or <paramref name="signalCount" /> is greater than <see cref="P:System.Threading.CountdownEvent.CurrentCount" />.
		/// </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="signalCount" /> is less
		/// than 1.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The current instance has already been
		/// disposed.</exception>
		public bool Signal(int signalCount)
		{
			if (signalCount <= 0)
			{
				throw new ArgumentOutOfRangeException("signalCount");
			}
			ThrowIfDisposed();
			SpinWait spinWait = default(SpinWait);
			int currentCount;
			while (true)
			{
				currentCount = m_currentCount;
				if (currentCount < signalCount)
				{
					throw new InvalidOperationException(Environment2.GetResourceString("CountdownEvent_Decrement_BelowZero"));
				}
				if (Interlocked.CompareExchange(ref m_currentCount, currentCount - signalCount, currentCount) == currentCount)
				{
					break;
				}
				spinWait.SpinOnce();
			}
			if (currentCount == signalCount)
			{
				m_event.Set();
				return true;
			}
			return false;
		}

		/// <summary>
		/// Increments the <see cref="T:System.Threading.CountdownEvent" />'s current count by one.
		/// </summary>
		/// <exception cref="T:System.InvalidOperationException">The current instance is already
		/// set.</exception>
		/// <exception cref="T:System.InvalidOperationException"><see cref="P:System.Threading.CountdownEvent.CurrentCount" /> is equal to <see cref="T:System.Int32.MaxValue" />.</exception>
		/// <exception cref="T:System.ObjectDisposedException">
		/// The current instance has already been disposed.
		/// </exception>
		public void AddCount()
		{
			AddCount(1);
		}

		/// <summary>
		/// Attempts to increment the <see cref="T:System.Threading.CountdownEvent" />'s current count by one.
		/// </summary>
		/// <returns>true if the increment succeeded; otherwise, false. If <see cref="P:System.Threading.CountdownEvent.CurrentCount" /> is
		/// already at zero. this will return false.</returns>
		/// <exception cref="T:System.InvalidOperationException"><see cref="P:System.Threading.CountdownEvent.CurrentCount" /> is equal to <see cref="T:System.Int32.MaxValue" />.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The current instance has already been
		/// disposed.</exception>
		public bool TryAddCount()
		{
			return TryAddCount(1);
		}

		/// <summary>
		/// Increments the <see cref="T:System.Threading.CountdownEvent" />'s current count by a specified
		/// value.
		/// </summary>
		/// <param name="signalCount">The value by which to increase <see cref="P:System.Threading.CountdownEvent.CurrentCount" />.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="signalCount" /> is less than
		/// 0.</exception>
		/// <exception cref="T:System.InvalidOperationException">The current instance is already
		/// set.</exception>
		/// <exception cref="T:System.InvalidOperationException"><see cref="P:System.Threading.CountdownEvent.CurrentCount" /> is equal to <see cref="T:System.Int32.MaxValue" />.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The current instance has already been
		/// disposed.</exception>
		public void AddCount(int signalCount)
		{
			if (!TryAddCount(signalCount))
			{
				throw new InvalidOperationException(Environment2.GetResourceString("CountdownEvent_Increment_AlreadyZero"));
			}
		}

		/// <summary>
		/// Attempts to increment the <see cref="T:System.Threading.CountdownEvent" />'s current count by a
		/// specified value.
		/// </summary>
		/// <param name="signalCount">The value by which to increase <see cref="P:System.Threading.CountdownEvent.CurrentCount" />.</param>
		/// <returns>true if the increment succeeded; otherwise, false. If <see cref="P:System.Threading.CountdownEvent.CurrentCount" /> is
		/// already at zero this will return false.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="signalCount" /> is less
		/// than 0.</exception>
		/// <exception cref="T:System.InvalidOperationException">The current instance is already
		/// set.</exception>
		/// <exception cref="T:System.InvalidOperationException"><see cref="P:System.Threading.CountdownEvent.CurrentCount" /> is equal to <see cref="T:System.Int32.MaxValue" />.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The current instance has already been
		/// disposed.</exception>
		public bool TryAddCount(int signalCount)
		{
			if (signalCount <= 0)
			{
				throw new ArgumentOutOfRangeException("signalCount");
			}
			ThrowIfDisposed();
			SpinWait spinWait = default(SpinWait);
			while (true)
			{
				int currentCount = m_currentCount;
				if (currentCount == 0)
				{
					return false;
				}
				if (currentCount > int.MaxValue - signalCount)
				{
					throw new InvalidOperationException(Environment2.GetResourceString("CountdownEvent_Increment_AlreadyMax"));
				}
				if (Interlocked.CompareExchange(ref m_currentCount, currentCount + signalCount, currentCount) == currentCount)
				{
					break;
				}
				spinWait.SpinOnce();
			}
			return true;
		}

		/// <summary>
		/// Resets the <see cref="P:System.Threading.CountdownEvent.CurrentCount" /> to the value of <see cref="P:System.Threading.CountdownEvent.InitialCount" />.
		/// </summary>
		/// <remarks>
		/// Unlike most of the members of <see cref="T:System.Threading.CountdownEvent" />, Reset is not
		/// thread-safe and may not be used concurrently with other members of this instance.
		/// </remarks>
		/// <exception cref="T:System.ObjectDisposedException">The current instance has already been
		/// disposed..</exception>
		public void Reset()
		{
			Reset(m_initialCount);
		}

		/// <summary>
		/// Resets the <see cref="P:System.Threading.CountdownEvent.CurrentCount" /> to a specified value.
		/// </summary>
		/// <param name="count">The number of signals required to set the <see cref="T:System.Threading.CountdownEvent" />.</param>
		/// <remarks>
		/// Unlike most of the members of <see cref="T:System.Threading.CountdownEvent" />, Reset is not
		/// thread-safe and may not be used concurrently with other members of this instance.
		/// </remarks>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="count" /> is
		/// less than 0.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The current instance has alread been disposed.</exception>
		public void Reset(int count)
		{
			ThrowIfDisposed();
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			m_currentCount = count;
			m_initialCount = count;
			if (count == 0)
			{
				m_event.Set();
			}
			else
			{
				m_event.Reset();
			}
		}

		/// <summary>
		/// Blocks the current thread until the <see cref="T:System.Threading.CountdownEvent" /> is set.
		/// </summary>
		/// <remarks>
		/// The caller of this method blocks indefinitely until the current instance is set. The caller will
		/// return immediately if the event is currently in a set state.
		/// </remarks>
		/// <exception cref="T:System.ObjectDisposedException">The current instance has already been
		/// disposed.</exception>
		public void Wait()
		{
			Wait(-1, default(CancellationToken));
		}

		/// <summary>
		/// Blocks the current thread until the <see cref="T:System.Threading.CountdownEvent" /> is set, while
		/// observing a <see cref="T:System.Threading.CancellationToken" />.
		/// </summary>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> to
		/// observe.</param>
		/// <remarks>
		/// The caller of this method blocks indefinitely until the current instance is set. The caller will
		/// return immediately if the event is currently in a set state.  If the 
		/// <see cref="T:System.Threading.CancellationToken">CancellationToken</see> being observed
		/// is canceled during the wait operation, an <see cref="T:System.OperationCanceledException" />
		/// will be thrown.
		/// </remarks>
		/// <exception cref="T:System.OperationCanceledException"><paramref name="cancellationToken" /> has been
		/// canceled.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The current instance has already been
		/// disposed.</exception>
		public void Wait(CancellationToken cancellationToken)
		{
			Wait(-1, cancellationToken);
		}

		/// <summary>
		/// Blocks the current thread until the <see cref="T:System.Threading.CountdownEvent" /> is set, using a
		/// <see cref="T:System.TimeSpan" /> to measure the time interval.
		/// </summary>
		/// <param name="timeout">A <see cref="T:System.TimeSpan" /> that represents the number of
		/// milliseconds to wait, or a <see cref="T:System.TimeSpan" /> that represents -1 milliseconds to
		/// wait indefinitely.</param>
		/// <returns>true if the <see cref="T:System.Threading.CountdownEvent" /> was set; otherwise,
		/// false.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="timeout" /> is a negative
		/// number other than -1 milliseconds, which represents an infinite time-out -or- timeout is greater
		/// than <see cref="F:System.Int32.MaxValue" />.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The current instance has already been
		/// disposed.</exception>
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
		/// Blocks the current thread until the <see cref="T:System.Threading.CountdownEvent" /> is set, using
		/// a <see cref="T:System.TimeSpan" /> to measure the time interval, while observing a
		/// <see cref="T:System.Threading.CancellationToken" />.
		/// </summary>
		/// <param name="timeout">A <see cref="T:System.TimeSpan" /> that represents the number of
		/// milliseconds to wait, or a <see cref="T:System.TimeSpan" /> that represents -1 milliseconds to
		/// wait indefinitely.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> to
		/// observe.</param>
		/// <returns>true if the <see cref="T:System.Threading.CountdownEvent" /> was set; otherwise,
		/// false.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="timeout" /> is a negative
		/// number other than -1 milliseconds, which represents an infinite time-out -or- timeout is greater
		/// than <see cref="F:System.Int32.MaxValue" />.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The current instance has already been
		/// disposed.</exception>
		/// <exception cref="T:System.OperationCanceledException"><paramref name="cancellationToken" /> has
		/// been canceled.</exception>
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
		/// Blocks the current thread until the <see cref="T:System.Threading.CountdownEvent" /> is set, using a
		/// 32-bit signed integer to measure the time interval.
		/// </summary>
		/// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="F:System.Threading.Timeout.Infinite" />(-1) to wait indefinitely.</param>
		/// <returns>true if the <see cref="T:System.Threading.CountdownEvent" /> was set; otherwise,
		/// false.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="millisecondsTimeout" /> is a
		/// negative number other than -1, which represents an infinite time-out.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The current instance has already been
		/// disposed.</exception>
		public bool Wait(int millisecondsTimeout)
		{
			return Wait(millisecondsTimeout, default(CancellationToken));
		}

		/// <summary>
		/// Blocks the current thread until the <see cref="T:System.Threading.CountdownEvent" /> is set, using a
		/// 32-bit signed integer to measure the time interval, while observing a
		/// <see cref="T:System.Threading.CancellationToken" />.
		/// </summary>
		/// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="F:System.Threading.Timeout.Infinite" />(-1) to wait indefinitely.</param>
		/// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> to
		/// observe.</param>
		/// <returns>true if the <see cref="T:System.Threading.CountdownEvent" /> was set; otherwise,
		/// false.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="millisecondsTimeout" /> is a
		/// negative number other than -1, which represents an infinite time-out.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The current instance has already been
		/// disposed.</exception>
		/// <exception cref="T:System.OperationCanceledException"><paramref name="cancellationToken" /> has
		/// been canceled.</exception>
		public bool Wait(int millisecondsTimeout, CancellationToken cancellationToken)
		{
			if (millisecondsTimeout < -1)
			{
				throw new ArgumentOutOfRangeException("millisecondsTimeout");
			}
			ThrowIfDisposed();
			cancellationToken.ThrowIfCancellationRequested();
			bool flag = IsSet;
			if (!flag)
			{
				flag = m_event.Wait(millisecondsTimeout, cancellationToken);
			}
			return flag;
		}

		/// <summary>
		/// Throws an exception if the latch has been disposed.
		/// </summary>
		private void ThrowIfDisposed()
		{
			if (m_disposed)
			{
				throw new ObjectDisposedException("CountdownEvent");
			}
		}
	}
}
