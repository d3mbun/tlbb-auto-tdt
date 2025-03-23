namespace System.Threading
{
	/// <summary>A debugger view of the ThreadLocal&lt;T&gt; to surface additional debugging properties and 
	/// to ensure that the ThreadLocal&lt;T&gt; does not become initialized if it was not already.</summary>
	internal sealed class SystemThreading_ThreadLocalDebugView<T>
	{
		private readonly ThreadLocal<T> m_tlocal;

		/// <summary>Returns whether the ThreadLocal object is initialized or not.</summary>
		public bool IsValueCreated => m_tlocal.IsValueCreated;

		/// <summary>Returns the value of the ThreadLocal object.</summary>
		public T Value => m_tlocal.ValueForDebugDisplay;

		/// <summary>Constructs a new debugger view object for the provided ThreadLocal object.</summary>
		/// <param name="tlocal">A ThreadLocal object to browse in the debugger.</param>
		public SystemThreading_ThreadLocalDebugView(ThreadLocal<T> tlocal)
		{
			m_tlocal = tlocal;
		}
	}
}
