using System.Collections.Generic;

namespace System.Linq.Parallel
{
	/// <summary>
	/// The simplest channel is one that has no synchronization.  This is used for stop-
	/// and-go productions where we are guaranteed the consumer is not running
	/// concurrently. It just wraps a FIFO queue internally.
	///
	/// Assumptions:
	///     Producers and consumers never try to enqueue/dequeue concurrently.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	internal sealed class SynchronousChannel<T>
	{
		private Queue<T> m_queue;

		internal int Count => m_queue.Count;

		internal SynchronousChannel()
		{
		}

		internal void Init()
		{
			m_queue = new Queue<T>();
		}

		internal void Enqueue(T item)
		{
			m_queue.Enqueue(item);
		}

		internal T Dequeue()
		{
			return m_queue.Dequeue();
		}

		internal void SetDone()
		{
		}

		internal void CopyTo(T[] array, int arrayIndex)
		{
			m_queue.CopyTo(array, arrayIndex);
		}
	}
}
