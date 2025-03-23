using System.Diagnostics;

namespace System.Collections.Concurrent
{
	/// <summary>
	/// A debugger view of the IProducerConsumerCollection that makes it simple to browse the
	/// collection's contents at a point in time.
	/// </summary>
	/// <typeparam name="T">The type of elements stored within.</typeparam>
	internal sealed class SystemCollectionsConcurrent_ProducerConsumerCollectionDebugView<T>
	{
		private IProducerConsumerCollection<T> m_collection;

		/// <summary>
		/// Returns a snapshot of the underlying collection's elements.
		/// </summary>
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public T[] Items => m_collection.ToArray();

		/// <summary>
		/// Constructs a new debugger view object for the provided collection object.
		/// </summary>
		/// <param name="collection">A collection to browse in the debugger.</param>
		public SystemCollectionsConcurrent_ProducerConsumerCollectionDebugView(IProducerConsumerCollection<T> collection)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			m_collection = collection;
		}
	}
}
