namespace System.Collections.Concurrent
{
	/// <summary>
	/// A simple class for the debugger view window
	/// </summary>
	internal sealed class SystemThreadingCollection_IProducerConsumerCollectionDebugView<T>
	{
		private IProducerConsumerCollection<T> m_collection;

		public T[] Items => m_collection.ToArray();

		public SystemThreadingCollection_IProducerConsumerCollectionDebugView(IProducerConsumerCollection<T> collection)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			m_collection = collection;
		}
	}
}
