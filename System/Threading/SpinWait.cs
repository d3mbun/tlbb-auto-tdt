using System.Security.Permissions;

namespace System.Threading
{
	/// <summary>
	/// Provides support for spin-based waiting.
	/// </summary>
	/// <remarks>
	/// <para>
	/// <see cref="T:System.Threading.SpinWait" /> encapsulates common spinning logic. On single-processor machines, yields are
	/// always used instead of busy waits, and on computers with Intel™ processors employing Hyper-Threading™
	/// technology, it helps to prevent hardware thread starvation. SpinWait encapsulates a good mixture of
	/// spinning and true yielding.
	/// </para>
	/// <para>
	/// <see cref="T:System.Threading.SpinWait" /> is a value type, which means that low-level code can utilize SpinWait without
	/// fear of unnecessary allocation overheads. SpinWait is not generally useful for ordinary applications.
	/// In most cases, you should use the synchronization classes provided by the .NET Framework, such as
	/// <see cref="T:System.Threading.Monitor" />. For most purposes where spin waiting is required, however,
	/// the <see cref="T:System.Threading.SpinWait" /> type should be preferred over the <see cref="M:System.Threading.Thread.SpinWait(System.Int32)" /> method.
	/// </para>
	/// <para>
	/// While SpinWait is designed to be used in concurrent applications, it is not designed to be
	/// used from multiple threads concurrently.  SpinWait's members are not thread-safe.  If multiple
	/// threads must spin, each should use its own instance of SpinWait.
	/// </para>
	/// </remarks>
	[HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
	public struct SpinWait
	{
		internal const int YIELD_THRESHOLD = 10;

		internal const int SLEEP_0_EVERY_HOW_MANY_TIMES = 5;

		internal const int SLEEP_1_EVERY_HOW_MANY_TIMES = 20;

		private int m_count;

		/// <summary>
		/// Gets the number of times <see cref="M:System.Threading.SpinWait.SpinOnce" /> has been called on this instance.
		/// </summary>
		public int Count => m_count;

		/// <summary>
		/// Gets whether the next call to <see cref="M:System.Threading.SpinWait.SpinOnce" /> will yield the processor, triggering a
		/// forced context switch.
		/// </summary>
		/// <value>Whether the next call to <see cref="M:System.Threading.SpinWait.SpinOnce" /> will yield the processor, triggering a
		/// forced context switch.</value>
		/// <remarks>
		/// On a single-CPU machine, <see cref="M:System.Threading.SpinWait.SpinOnce" /> always yields the processor. On machines with
		/// multiple CPUs, <see cref="M:System.Threading.SpinWait.SpinOnce" /> may yield after an unspecified number of calls.
		/// </remarks>
		public bool NextSpinWillYield
		{
			get
			{
				if (m_count <= 10)
				{
					return PlatformHelper.IsSingleProcessor;
				}
				return true;
			}
		}

		/// <summary>
		/// Performs a single spin.
		/// </summary>
		/// <remarks>
		/// This is typically called in a loop, and may change in behavior based on the number of times a
		/// <see cref="M:System.Threading.SpinWait.SpinOnce" /> has been called thus far on this instance.
		/// </remarks>
		public void SpinOnce()
		{
			if (NextSpinWillYield)
			{
				int num = ((m_count >= 10) ? (m_count - 10) : m_count);
				if (num % 20 == 19)
				{
					Thread.Sleep(1);
				}
				else if (num % 5 == 4)
				{
					Thread.Sleep(0);
				}
				else
				{
					Platform.Yield();
				}
			}
			else
			{
				Thread.SpinWait(4 << m_count);
			}
			m_count = ((m_count == int.MaxValue) ? 10 : (m_count + 1));
		}

		/// <summary>
		/// Resets the spin counter.
		/// </summary>
		/// <remarks>
		/// This makes <see cref="M:System.Threading.SpinWait.SpinOnce" /> and <see cref="P:System.Threading.SpinWait.NextSpinWillYield" /> behave as though no calls
		/// to <see cref="M:System.Threading.SpinWait.SpinOnce" /> had been issued on this instance. If a <see cref="T:System.Threading.SpinWait" /> instance
		/// is reused many times, it may be useful to reset it to avoid yielding too soon.
		/// </remarks>
		public void Reset()
		{
			m_count = 0;
		}

		/// <summary>
		/// Spins until the specified condition is satisfied.
		/// </summary>
		/// <param name="condition">A delegate to be executed over and over until it returns true.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="condition" /> argument is null.</exception>
		public static void SpinUntil(Func<bool> condition)
		{
			SpinUntil(condition, -1);
		}

		/// <summary>
		/// Spins until the specified condition is satisfied or until the specified timeout is expired.
		/// </summary>
		/// <param name="condition">A delegate to be executed over and over until it returns true.</param>
		/// <param name="timeout">
		/// A <see cref="T:System.TimeSpan" /> that represents the number of milliseconds to wait, 
		/// or a TimeSpan that represents -1 milliseconds to wait indefinitely.</param>
		/// <returns>True if the condition is satisfied within the timeout; otherwise, false</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="condition" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="timeout" /> is a negative number
		/// other than -1 milliseconds, which represents an infinite time-out -or- timeout is greater than
		/// <see cref="F:System.Int32.MaxValue" />.</exception>
		public static bool SpinUntil(Func<bool> condition, TimeSpan timeout)
		{
			long num = (long)timeout.TotalMilliseconds;
			if (num < -1 || num > int.MaxValue)
			{
				throw new ArgumentOutOfRangeException("timeout", timeout, Environment2.GetResourceString("SpinWait_SpinUntil_TimeoutWrong"));
			}
			return SpinUntil(condition, (int)timeout.TotalMilliseconds);
		}

		/// <summary>
		/// Spins until the specified condition is satisfied or until the specified timeout is expired.
		/// </summary>
		/// <param name="condition">A delegate to be executed over and over until it returns true.</param>
		/// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="F:System.Threading.Timeout.Infinite" /> (-1) to wait indefinitely.</param>
		/// <returns>True if the condition is satisfied within the timeout; otherwise, false</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="condition" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="millisecondsTimeout" /> is a
		/// negative number other than -1, which represents an infinite time-out.</exception>
		public static bool SpinUntil(Func<bool> condition, int millisecondsTimeout)
		{
			if (millisecondsTimeout < -1)
			{
				throw new ArgumentOutOfRangeException("millisecondsTimeout", millisecondsTimeout, Environment2.GetResourceString("SpinWait_SpinUntil_TimeoutWrong"));
			}
			if (condition == null)
			{
				throw new ArgumentNullException("condition", Environment2.GetResourceString("SpinWait_SpinUntil_ArgumentNull"));
			}
			long num = 0L;
			if (millisecondsTimeout != 0 && millisecondsTimeout != -1)
			{
				num = DateTime.UtcNow.Ticks;
			}
			SpinWait spinWait = default(SpinWait);
			while (!condition())
			{
				if (millisecondsTimeout == 0)
				{
					return false;
				}
				spinWait.SpinOnce();
				if (millisecondsTimeout != -1 && spinWait.NextSpinWillYield && millisecondsTimeout <= (DateTime.UtcNow.Ticks - num) / 10000)
				{
					return false;
				}
			}
			return true;
		}
	}
}
