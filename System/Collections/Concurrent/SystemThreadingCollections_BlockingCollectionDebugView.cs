using System.Diagnostics;

namespace System.Collections.Concurrent
{
	/// <summary>A debugger view of the blocking collection that makes it simple to browse the
	/// collection's contents at a point in time.</summary>
	/// <typeparam name="T">The type of element that the BlockingCollection will hold.</typeparam>
	internal sealed class SystemThreadingCollections_BlockingCollectionDebugView<T>
	{
		private BlockingCollection<T> m_blockingCollection;

		/// <summary>Returns a snapshot of the underlying collection's elements.</summary>
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public T[] Items => m_blockingCollection.ToArray();

		/// <summary>Constructs a new debugger view object for the provided blocking collection object.</summary>
		/// <param name="collection">A blocking collection to browse in the debugger.</param>
		public SystemThreadingCollections_BlockingCollectionDebugView(BlockingCollection<T> collection)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			m_blockingCollection = collection;
		}
	}
}
