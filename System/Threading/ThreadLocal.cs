using System.Collections.Concurrent;
using System.Diagnostics;
using System.Security;
using System.Security.Permissions;

namespace System.Threading
{
	/// <summary>
	/// Provides thread-local storage of data.
	/// </summary>
	/// <typeparam name="T">Specifies the type of data stored per-thread.</typeparam>
	/// <remarks>
	/// <para>
	/// With the exception of <see cref="M:System.Threading.ThreadLocal`1.Dispose" />, all public and protected members of 
	/// <see cref="T:System.Threading.ThreadLocal`1" /> are thread-safe and may be used
	/// concurrently from multiple threads.
	/// </para>
	/// </remarks>
	[DebuggerTypeProxy(typeof(SystemThreading_ThreadLocalDebugView<>))]
	[DebuggerDisplay("IsValueCreated={IsValueCreated}, Value={ValueForDebugDisplay}")]
	[HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
	public class ThreadLocal<T> : IDisposable
	{
		private class C0
		{
		}

		private class C1
		{
		}

		private class C2
		{
		}

		private class C3
		{
		}

		private class C4
		{
		}

		private class C5
		{
		}

		private class C6
		{
		}

		private class C7
		{
		}

		private class C8
		{
		}

		private class C9
		{
		}

		private class C10
		{
		}

		private class C11
		{
		}

		private class C12
		{
		}

		private class C13
		{
		}

		private class C14
		{
		}

		private class C15
		{
		}

		/// <summary>
		/// The base abstract class for the holder
		/// </summary>
		private abstract class HolderBase
		{
			internal abstract Boxed Boxed { get; set; }
		}

		/// <summary>
		/// The TLS holder representation
		/// </summary>
		private sealed class TLSHolder : HolderBase
		{
			private LocalDataStoreSlot m_slot = Thread.AllocateDataSlot();

			internal override Boxed Boxed
			{
				get
				{
					return (Boxed)Thread.GetData(m_slot);
				}
				set
				{
					Thread.SetData(m_slot, value);
				}
			}
		}

		/// <summary>
		/// The generic holder representation
		/// </summary>
		/// <typeparam name="U">Dummy param</typeparam>
		/// <typeparam name="V">Dummy param</typeparam>
		/// <typeparam name="W">Dummy param</typeparam>
		private sealed class GenericHolder<U, V, W> : HolderBase
		{
			[ThreadStatic]
			private static Boxed s_value;

			internal override Boxed Boxed
			{
				get
				{
					return s_value;
				}
				set
				{
					s_value = value;
				}
			}
		}

		/// <summary>
		/// wrapper to the actual value
		/// </summary>
		private class Boxed
		{
			internal T Value;

			internal HolderBase m_ownerHolder;
		}

		private static Type[] s_dummyTypes = new Type[16]
		{
			typeof(C0),
			typeof(C1),
			typeof(C2),
			typeof(C3),
			typeof(C4),
			typeof(C5),
			typeof(C6),
			typeof(C7),
			typeof(C8),
			typeof(C9),
			typeof(C10),
			typeof(C11),
			typeof(C12),
			typeof(C13),
			typeof(C14),
			typeof(C15)
		};

		private static int s_currentTypeId = -1;

		private static ConcurrentStack<int> s_availableIndices = new ConcurrentStack<int>();

		private static int TYPE_DIMENSIONS = typeof(GenericHolder<, , >).GetGenericArguments().Length;

		internal static int MAXIMUM_TYPES_LENGTH = (int)Math.Pow(s_dummyTypes.Length, TYPE_DIMENSIONS - 1);

		private HolderBase m_holder;

		private Func<T> m_valueFactory;

		private int m_currentInstanceIndex;

		/// <summary>
		/// Gets or sets the value of this instance for the current thread.
		/// </summary>
		/// <exception cref="T:System.InvalidOperationException">
		/// The initialization function referenced <see cref="P:System.Threading.ThreadLocal`1.Value" /> in an improper manner.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">
		/// The <see cref="T:System.Threading.ThreadLocal`1" /> instance has been disposed.
		/// </exception>
		/// <remarks>
		/// If this instance was not previously initialized for the current thread,
		/// accessing <see cref="P:System.Threading.ThreadLocal`1.Value" /> will attempt to initialize it. If an initialization function was 
		/// supplied during the construction, that initialization will happen by invoking the function 
		/// to retrieve the initial value for <see cref="P:System.Threading.ThreadLocal`1.Value" />.  Otherwise, the default value of 
		/// <typeparamref name="T" /> will be used.
		/// </remarks>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public T Value
		{
			get
			{
				if (m_holder == null)
				{
					throw new ObjectDisposedException(Environment2.GetResourceString("ThreadLocal_Disposed"));
				}
				Boxed boxed = m_holder.Boxed;
				if (boxed == null || boxed.m_ownerHolder != m_holder)
				{
					boxed = CreateValue();
				}
				return boxed.Value;
			}
			set
			{
				if (m_holder == null)
				{
					throw new ObjectDisposedException(Environment2.GetResourceString("ThreadLocal_Disposed"));
				}
				Boxed boxed = m_holder.Boxed;
				if (boxed != null && boxed.m_ownerHolder == m_holder)
				{
					boxed.Value = value;
					return;
				}
				m_holder.Boxed = new Boxed
				{
					Value = value,
					m_ownerHolder = m_holder
				};
			}
		}

		/// <summary>
		/// Gets whether <see cref="P:System.Threading.ThreadLocal`1.Value" /> is initialized on the current thread.
		/// </summary>
		/// <exception cref="T:System.ObjectDisposedException">
		/// The <see cref="T:System.Threading.ThreadLocal`1" /> instance has been disposed.
		/// </exception>
		public bool IsValueCreated
		{
			get
			{
				if (m_holder == null)
				{
					throw new ObjectDisposedException(Environment2.GetResourceString("ThreadLocal_Disposed"));
				}
				Boxed boxed = m_holder.Boxed;
				if (boxed != null)
				{
					return boxed.m_ownerHolder == m_holder;
				}
				return false;
			}
		}

		/// <summary>Gets the value of the ThreadLocal&lt;T&gt; for debugging display purposes. It takes care of getting
		/// the value for the current thread in the ThreadLocal mode.</summary>
		internal T ValueForDebugDisplay
		{
			get
			{
				if (m_holder == null || m_holder.Boxed == null || m_holder.Boxed.m_ownerHolder != m_holder)
				{
					return default(T);
				}
				return m_holder.Boxed.Value;
			}
		}

		/// <summary>
		/// Initializes the <see cref="T:System.Threading.ThreadLocal`1" /> instance.
		/// </summary>
		[System.Security.SecuritySafeCritical]
		public ThreadLocal()
		{
			if (FindNextTypeIndex())
			{
				Type[] typesFromIndex = GetTypesFromIndex();
				PermissionSet permissionSet = new PermissionSet(PermissionState.Unrestricted);
				permissionSet.Assert();
				try
				{
					m_holder = (HolderBase)Activator.CreateInstance(typeof(GenericHolder<, , >).MakeGenericType(typesFromIndex));
				}
				finally
				{
					PermissionSet.RevertAssert();
				}
			}
			else
			{
				m_holder = new TLSHolder();
			}
		}

		/// <summary>
		/// Initializes the <see cref="T:System.Threading.ThreadLocal`1" /> instance with the
		/// specified <paramref name="valueFactory" /> function.
		/// </summary>
		/// <param name="valueFactory">
		/// The <see cref="T:System.Func{T}" /> invoked to produce a lazily-initialized value when 
		/// an attempt is made to retrieve <see cref="P:System.Threading.ThreadLocal`1.Value" /> without it having been previously initialized.
		/// </param>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="valueFactory" /> is a null reference (Nothing in Visual Basic).
		/// </exception>
		public ThreadLocal(Func<T> valueFactory)
			: this()
		{
			if (valueFactory == null)
			{
				throw new ArgumentNullException("valueFactory");
			}
			m_valueFactory = valueFactory;
		}

		/// <summary>
		/// Releases the resources used by this <see cref="T:System.Threading.ThreadLocal{T}" /> instance.
		/// </summary>
		~ThreadLocal()
		{
			Dispose(disposing: false);
		}

		/// <summary>
		/// Releases the resources used by this <see cref="T:System.Threading.ThreadLocal{T}" /> instance.
		/// </summary>
		/// <remarks>
		/// Unlike most of the members of <see cref="T:System.Threading.ThreadLocal{T}" />, this method is not thread-safe.
		/// </remarks>
		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Releases the resources used by this <see cref="T:System.Threading.ThreadLocal{T}" /> instance.
		/// </summary>
		/// <param name="disposing">
		/// A Boolean value that indicates whether this method is being called due to a call to <see cref="M:System.Threading.ThreadLocal`1.Dispose" />.
		/// </param>
		/// <remarks>
		/// Unlike most of the members of <see cref="T:System.Threading.ThreadLocal{T}" />, this method is not thread-safe.
		/// </remarks>
		protected virtual void Dispose(bool disposing)
		{
			int currentInstanceIndex = m_currentInstanceIndex;
			if (currentInstanceIndex > -1 && Interlocked.CompareExchange(ref m_currentInstanceIndex, -1, currentInstanceIndex) == currentInstanceIndex)
			{
				s_availableIndices.Push(currentInstanceIndex);
			}
			m_holder = null;
		}

		/// <summary>
		/// Tries to get a unique index for the current instance of type T, it first tries to get it from the pool if it is not empty, otherwise it
		/// increments the global counter if it is still below the maximum, otherwise it fails and returns -1
		/// </summary>
		/// <returns>True if there is an index available, false otherwise</returns>
		private bool FindNextTypeIndex()
		{
			int result = -1;
			if (s_availableIndices.TryPop(out result))
			{
				m_currentInstanceIndex = result;
				return true;
			}
			if (s_currentTypeId < MAXIMUM_TYPES_LENGTH - 1 && ThreadLocalGlobalCounter.s_fastPathCount < ThreadLocalGlobalCounter.MAXIMUM_GLOBAL_COUNT && Interlocked.Increment(ref ThreadLocalGlobalCounter.s_fastPathCount) <= ThreadLocalGlobalCounter.MAXIMUM_GLOBAL_COUNT)
			{
				result = Interlocked.Increment(ref s_currentTypeId);
				if (result < MAXIMUM_TYPES_LENGTH)
				{
					m_currentInstanceIndex = result;
					return true;
				}
			}
			m_currentInstanceIndex = -1;
			return false;
		}

		/// <summary>
		/// Gets an array of types that will be used as generic parameters for the GenericHolder class
		/// </summary>
		/// <returns>The types array</returns>
		private Type[] GetTypesFromIndex()
		{
			Type[] array = new Type[TYPE_DIMENSIONS];
			array[0] = typeof(T);
			int num = m_currentInstanceIndex;
			for (int i = 1; i < TYPE_DIMENSIONS; i++)
			{
				array[i] = s_dummyTypes[num % s_dummyTypes.Length];
				num /= s_dummyTypes.Length;
			}
			return array;
		}

		/// <summary>Creates and returns a string representation of this instance for the current thread.</summary>
		/// <returns>The result of calling <see cref="M:System.Object.ToString" /> on the <see cref="P:System.Threading.ThreadLocal`1.Value" />.</returns>
		/// <exception cref="T:System.NullReferenceException">
		/// The <see cref="P:System.Threading.ThreadLocal`1.Value" /> for the current thread is a null reference (Nothing in Visual Basic).
		/// </exception>
		/// <exception cref="T:System.InvalidOperationException">
		/// The initialization function referenced <see cref="P:System.Threading.ThreadLocal`1.Value" /> in an improper manner.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">
		/// The <see cref="T:System.Threading.ThreadLocal`1" /> instance has been disposed.
		/// </exception>
		/// <remarks>
		/// Calling this method forces initialization for the current thread, as is the
		/// case with accessing <see cref="P:System.Threading.ThreadLocal`1.Value" /> directly.
		/// </remarks>
		public override string ToString()
		{
			return Value.ToString();
		}

		/// <summary>
		/// Private helper function to lazily create the value using the calueSelector if specified in the constructor or the default parameterless constructor
		/// </summary>
		/// <returns>Returns the boxed object</returns>
		private Boxed CreateValue()
		{
			Boxed boxed = new Boxed();
			boxed.m_ownerHolder = m_holder;
			boxed.Value = ((m_valueFactory == null) ? default(T) : m_valueFactory());
			if (m_holder.Boxed != null && m_holder.Boxed.m_ownerHolder == m_holder)
			{
				throw new InvalidOperationException(Environment2.GetResourceString("ThreadLocal_Value_RecursiveCallsToValue"));
			}
			m_holder.Boxed = boxed;
			return boxed;
		}
	}
}
