using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Threading;

namespace System
{
	/// <summary>
	/// Provides support for lazy initialization.
	/// </summary>
	/// <typeparam name="T">Specifies the type of element being laziliy initialized.</typeparam>
	/// <remarks>
	/// <para>
	/// By default, all public and protected members of <see cref="T:System.Lazy`1" /> are thread-safe and may be used
	/// concurrently from multiple threads.  These thread-safety guarantees may be removed optionally and per instance
	/// using parameters to the type's constructors.
	/// </para>
	/// </remarks>
	[Serializable]
	[DebuggerDisplay("ThreadSafetyMode={Mode}, IsValueCreated={IsValueCreated}, IsValueFaulted={IsValueFaulted}, Value={ValueForDebugDisplay}")]
	[ComVisible(false)]
	[DebuggerTypeProxy(typeof(System_LazyDebugView<>))]
	[HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
	public class Lazy<T>
	{
		/// <summary>
		/// wrapper class to box the initialized value, this is mainly created to avoid boxing/unboxing the value each time the value is called in case T is 
		/// a value type
		/// </summary>
		[Serializable]
		private class Boxed
		{
			internal T m_value;

			internal Boxed(T value)
			{
				m_value = value;
			}
		}

		/// <summary>
		/// Wrapper class to wrap the excpetion thrown by the value factory
		/// </summary>
		private class LazyInternalExceptionHolder
		{
			internal Exception m_exception;

			internal LazyInternalExceptionHolder(Exception ex)
			{
				m_exception = ex;
			}
		}

		private static Func<T> PUBLICATION_ONLY_OR_ALREADY_INITIALIZED = () => default(T);

		private volatile object m_boxed;

		[NonSerialized]
		private Func<T> m_valueFactory;

		[NonSerialized]
		private readonly object m_threadSafeObj;

		/// <summary>Gets the value of the Lazy&lt;T&gt; for debugging display purposes.</summary>
		internal T ValueForDebugDisplay
		{
			get
			{
				if (!IsValueCreated)
				{
					return default(T);
				}
				return ((Boxed)m_boxed).m_value;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this instance may be used concurrently from multiple threads.
		/// </summary>
		internal LazyThreadSafetyMode Mode
		{
			get
			{
				if (m_threadSafeObj == null)
				{
					return LazyThreadSafetyMode.None;
				}
                return (Func<T>)m_threadSafeObj != PUBLICATION_ONLY_OR_ALREADY_INITIALIZED
                    ? LazyThreadSafetyMode.ExecutionAndPublication
                    : LazyThreadSafetyMode.PublicationOnly;
            }
        }

		/// <summary>
		/// Gets whether the value creation is faulted or not
		/// </summary>
		internal bool IsValueFaulted => m_boxed is LazyInternalExceptionHolder;

		/// <summary>Gets a value indicating whether the <see cref="T:System.Lazy{T}" /> has been initialized.
		/// </summary>
		/// <value>true if the <see cref="T:System.Lazy{T}" /> instance has been initialized;
		/// otherwise, false.</value>
		/// <remarks>
		/// The initialization of a <see cref="T:System.Lazy{T}" /> instance may result in either
		/// a value being produced or an exception being thrown.  If an exception goes unhandled during initialization, 
		/// <see cref="P:System.Lazy`1.IsValueCreated" /> will return false.
		/// </remarks>
		public bool IsValueCreated
		{
			get
			{
				if (m_boxed != null)
				{
					return m_boxed is Boxed;
				}
				return false;
			}
		}

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public T Value
		{
			get
			{
				Boxed boxed = null;
				if (m_boxed != null)
				{
					boxed = m_boxed as Boxed;
					if (boxed != null)
					{
						return boxed.m_value;
					}
					LazyInternalExceptionHolder lazyInternalExceptionHolder = m_boxed as LazyInternalExceptionHolder;
					throw lazyInternalExceptionHolder.m_exception;
				}
				return LazyInitValue();
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.Threading.Lazy{T}" /> class that 
		/// uses <typeparamref name="T" />'s default constructor for lazy initialization.
		/// </summary>
		/// <remarks>
		/// An instance created with this constructor may be used concurrently from multiple threads.
		/// </remarks>
		public Lazy()
			: this(LazyThreadSafetyMode.ExecutionAndPublication)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.Threading.Lazy{T}" /> class that uses a
		/// specified initialization function.
		/// </summary>
		/// <param name="valueFactory">
		/// The <see cref="T:System.Func{T}" /> invoked to produce the lazily-initialized value when it is
		/// needed.
		/// </param>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="valueFactory" /> is a null
		/// reference (Nothing in Visual Basic).</exception>
		/// <remarks>
		/// An instance created with this constructor may be used concurrently from multiple threads.
		/// </remarks>
		public Lazy(Func<T> valueFactory)
			: this(valueFactory, LazyThreadSafetyMode.ExecutionAndPublication)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.Threading.Lazy{T}" />
		/// class that uses <typeparamref name="T" />'s default constructor and a specified thread-safety mode.
		/// </summary>
		/// <param name="isThreadSafe">true if this instance should be usable by multiple threads concurrently; false if the instance will only be used by one thread at a time.
		/// </param>
		public Lazy(bool isThreadSafe)
			: this(isThreadSafe ? LazyThreadSafetyMode.ExecutionAndPublication : LazyThreadSafetyMode.None)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.Threading.Lazy{T}" />
		/// class that uses <typeparamref name="T" />'s default constructor and a specified thread-safety mode.
		/// </summary>
		/// <param name="mode">The lazy thread-safety mode mode</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="mode" /> mode contains an invalid valuee</exception>
		public Lazy(LazyThreadSafetyMode mode)
		{
			m_threadSafeObj = GetObjectFromMode(mode);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.Threading.Lazy{T}" /> class
		/// that uses a specified initialization function and a specified thread-safety mode.
		/// </summary>
		/// <param name="valueFactory">
		/// The <see cref="T:System.Func{T}" /> invoked to produce the lazily-initialized value when it is needed.
		/// </param>
		/// <param name="isThreadSafe">true if this instance should be usable by multiple threads concurrently; false if the instance will only be used by one thread at a time.
		/// </param>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="valueFactory" /> is
		/// a null reference (Nothing in Visual Basic).</exception>
		public Lazy(Func<T> valueFactory, bool isThreadSafe)
			: this(valueFactory, isThreadSafe ? LazyThreadSafetyMode.ExecutionAndPublication : LazyThreadSafetyMode.None)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.Threading.Lazy{T}" /> class
		/// that uses a specified initialization function and a specified thread-safety mode.
		/// </summary>
		/// <param name="valueFactory">
		/// The <see cref="T:System.Func{T}" /> invoked to produce the lazily-initialized value when it is needed.
		/// </param>
		/// <param name="mode">The lazy thread-safety mode.</param>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="valueFactory" /> is
		/// a null reference (Nothing in Visual Basic).</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="mode" /> mode contains an invalid value.</exception>
		public Lazy(Func<T> valueFactory, LazyThreadSafetyMode mode)
		{
			if (valueFactory == null)
			{
				throw new ArgumentNullException("valueFactory");
			}
			m_threadSafeObj = GetObjectFromMode(mode);
			m_valueFactory = valueFactory;
		}

		/// <summary>
		/// Static helper function that returns an object based on the given mode. it also throws an exception if the mode is invalid
		/// </summary>
		private static object GetObjectFromMode(LazyThreadSafetyMode mode)
		{
			return mode switch
			{
				LazyThreadSafetyMode.ExecutionAndPublication => new object(), 
				LazyThreadSafetyMode.PublicationOnly => PUBLICATION_ONLY_OR_ALREADY_INITIALIZED, 
				LazyThreadSafetyMode.None => null, 
				_ => throw new ArgumentOutOfRangeException("mode", Environment2.GetResourceString("Lazy_ctor_ModeInvalid")), 
			};
		}

		/// <summary>Forces initialization during serialization.</summary>
		/// <param name="context">The StreamingContext for the serialization operation.</param>
		[OnSerializing]
		private void OnSerializing(StreamingContext context)
		{
			_ = Value;
		}

		/// <summary>Creates and returns a string representation of this instance.</summary>
		/// <returns>The result of calling <see cref="M:System.Object.ToString" /> on the <see cref="P:System.Lazy`1.Value" />.</returns>
		/// <exception cref="T:System.NullReferenceException">
		/// The <see cref="P:System.Lazy`1.Value" /> is null.
		/// </exception>
		public override string ToString()
		{
			if (!IsValueCreated)
			{
				return Environment2.GetResourceString("Lazy_ToString_ValueNotCreated");
			}
			return Value.ToString();
		}

		/// <summary>
		/// local helper method to initialize the value 
		/// </summary>
		/// <returns>The inititialized T value</returns>
		private T LazyInitValue()
		{
			Boxed boxed = null;
			switch (Mode)
			{
			case LazyThreadSafetyMode.None:
				boxed = (Boxed)(m_boxed = CreateValue());
				break;
			case LazyThreadSafetyMode.PublicationOnly:
				boxed = CreateValue();
				if (Interlocked.CompareExchange(ref m_boxed, boxed, null) != null)
				{
					boxed = (Boxed)m_boxed;
				}
				break;
			default:
				lock (m_threadSafeObj)
				{
					if (m_boxed == null)
					{
						boxed = (Boxed)(m_boxed = CreateValue());
						break;
					}
					boxed = m_boxed as Boxed;
					if (boxed == null)
					{
						LazyInternalExceptionHolder lazyInternalExceptionHolder = m_boxed as LazyInternalExceptionHolder;
						throw lazyInternalExceptionHolder.m_exception;
					}
				}
				break;
			}
			return boxed.m_value;
		}

		/// <summary>Creates an instance of T using m_valueFactory in case its not null or use reflection to create a new T()</summary>
		/// <returns>An instance of Boxed.</returns>
		private Boxed CreateValue()
		{
			LazyThreadSafetyMode mode = Mode;
			if (m_valueFactory != null)
			{
				try
				{
					if (mode != LazyThreadSafetyMode.PublicationOnly && m_valueFactory == PUBLICATION_ONLY_OR_ALREADY_INITIALIZED)
					{
						throw new InvalidOperationException(Environment2.GetResourceString("Lazy_Value_RecursiveCallsToValue"));
					}
					Func<T> valueFactory = m_valueFactory;
					if (mode != LazyThreadSafetyMode.PublicationOnly)
					{
						m_valueFactory = PUBLICATION_ONLY_OR_ALREADY_INITIALIZED;
					}
					return new Boxed(valueFactory());
				}
				catch (Exception ex)
				{
					if (mode != LazyThreadSafetyMode.PublicationOnly)
					{
						m_boxed = new LazyInternalExceptionHolder(ex);
					}
					throw;
				}
			}
			try
			{
				return new Boxed((T)Activator.CreateInstance(typeof(T)));
			}
			catch (MissingMethodException)
			{
				Exception ex2 = new MissingMemberException(Environment2.GetResourceString("Lazy_CreateValue_NoParameterlessCtorForT"));
				if (mode != LazyThreadSafetyMode.PublicationOnly)
				{
					m_boxed = new LazyInternalExceptionHolder(ex2);
				}
				throw ex2;
			}
		}
	}
}
