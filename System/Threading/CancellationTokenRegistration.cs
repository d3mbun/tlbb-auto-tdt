using System.Security.Permissions;

namespace System.Threading
{
	/// <summary>
	/// Represents a callback delegate that has been registered with a <see cref="T:System.Threading.CancellationToken">CancellationToken</see>.
	/// </summary>
	/// <remarks>
	/// To unregister a callback, dispose the corresponding Registration instance.
	/// </remarks>
	[HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
	public struct CancellationTokenRegistration : IEquatable<CancellationTokenRegistration>, IDisposable
	{
		private readonly CancellationTokenSource m_tokenSource;

		private readonly CancellationCallbackInfo m_callbackInfo;

		private readonly SparselyPopulatedArrayAddInfo<CancellationCallbackInfo> m_registrationInfo;

		internal CancellationTokenRegistration(CancellationTokenSource tokenSource, CancellationCallbackInfo callbackInfo, SparselyPopulatedArrayAddInfo<CancellationCallbackInfo> registrationInfo)
		{
			m_tokenSource = tokenSource;
			m_callbackInfo = callbackInfo;
			m_registrationInfo = registrationInfo;
		}

		/// <summary>
		/// Attempts to deregister the item. If it's already being run, this may fail.
		/// Entails a full memory fence.
		/// </summary>
		/// <returns>True if the callback was found and deregistered, false otherwise.</returns>
		internal bool TryDeregister()
		{
			if (m_registrationInfo.Source == null)
			{
				return false;
			}
			CancellationCallbackInfo cancellationCallbackInfo = m_registrationInfo.Source.SafeAtomicRemove(m_registrationInfo.Index, m_callbackInfo);
			if (cancellationCallbackInfo != m_callbackInfo)
			{
				return false;
			}
			return true;
		}

		/// <summary>
		/// Disposes of the registration and unregisters the target callback from the associated 
		/// <see cref="T:System.Threading.CancellationToken">CancellationToken</see>.
		/// If the target callback is currently executing this method will wait until it completes, except
		/// in the degenerate cases where a callback method deregisters itself.
		/// </summary>
		public void Dispose()
		{
			if (m_tokenSource != null)
			{
				m_tokenSource.ThrowIfDisposed();
			}
			bool flag = TryDeregister();
			if (m_tokenSource != null && m_tokenSource.IsCancellationRequested && !m_tokenSource.IsCancellationCompleted && !flag && m_tokenSource.ThreadIDExecutingCallbacks != Thread.CurrentThread.ManagedThreadId)
			{
				m_tokenSource.WaitForCallbackToComplete(m_callbackInfo);
			}
		}

		/// <summary>
		/// Determines whether two <see cref="T:System.Threading.CancellationTokenRegistration">CancellationTokenRegistration</see>
		/// instances are equal.
		/// </summary>
		/// <param name="left">The first instance.</param>
		/// <param name="right">The second instance.</param>
		/// <returns>True if the instances are equal; otherwise, false.</returns>
		public static bool operator ==(CancellationTokenRegistration left, CancellationTokenRegistration right)
		{
			return left.Equals(right);
		}

		/// <summary>
		/// Determines whether two <see cref="T:System.Threading.CancellationTokenRegistration">CancellationTokenRegistration</see> instances are not equal.
		/// </summary>
		/// <param name="left">The first instance.</param>
		/// <param name="right">The second instance.</param>
		/// <returns>True if the instances are not equal; otherwise, false.</returns>
		public static bool operator !=(CancellationTokenRegistration left, CancellationTokenRegistration right)
		{
			return !left.Equals(right);
		}

		/// <summary>
		/// Determines whether the current <see cref="T:System.Threading.CancellationTokenRegistration">CancellationTokenRegistration</see> instance is equal to the 
		/// specified <see cref="T:System.Object" />.
		/// </summary> 
		/// <param name="obj">The other object to which to compare this instance.</param>
		/// <returns>True, if both this and <paramref name="obj" /> are equal. False, otherwise.
		/// Two <see cref="T:System.Threading.CancellationTokenRegistration">CancellationTokenRegistration</see> instances are equal if
		/// they both refer to the output of a single call to the same Register method of a 
		/// <see cref="T:System.Threading.CancellationToken">CancellationToken</see>. 
		/// </returns>
		public override bool Equals(object obj)
		{
			if (obj is CancellationTokenRegistration)
			{
				return Equals((CancellationTokenRegistration)obj);
			}
			return false;
		}

		/// <summary>
		/// Determines whether the current <see cref="T:System.Threading.CancellationToken">CancellationToken</see> instance is equal to the 
		/// specified <see cref="T:System.Object" />.
		/// </summary> 
		/// <param name="other">The other <see cref="T:System.Threading.CancellationTokenRegistration">CancellationTokenRegistration</see> to which to compare this instance.</param>
		/// <returns>True, if both this and <paramref name="other" /> are equal. False, otherwise.
		/// Two <see cref="T:System.Threading.CancellationTokenRegistration">CancellationTokenRegistration</see> instances are equal if
		/// they both refer to the output of a single call to the same Register method of a 
		/// <see cref="T:System.Threading.CancellationToken">CancellationToken</see>. 
		/// </returns>
		public bool Equals(CancellationTokenRegistration other)
		{
			if (m_tokenSource == other.m_tokenSource && m_callbackInfo == other.m_callbackInfo && m_registrationInfo.Source == other.m_registrationInfo.Source)
			{
				return m_registrationInfo.Index == other.m_registrationInfo.Index;
			}
			return false;
		}

		/// <summary>
		/// Serves as a hash function for a <see cref="T:System.Threading.CancellationTokenRegistration">CancellationTokenRegistration.</see>.
		/// </summary>
		/// <returns>A hash code for the current <see cref="T:System.Threading.CancellationTokenRegistration">CancellationTokenRegistration</see> instance.</returns>
		public override int GetHashCode()
		{
			if (m_registrationInfo.Source != null)
			{
				return m_registrationInfo.Source.GetHashCode() ^ m_registrationInfo.Index.GetHashCode();
			}
			return m_registrationInfo.Index.GetHashCode();
		}
	}
}
