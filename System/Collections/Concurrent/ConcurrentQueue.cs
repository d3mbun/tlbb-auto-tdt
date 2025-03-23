using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Threading;

namespace System.Collections.Concurrent
{
	/// <summary>
	/// Represents a thread-safe first-in, first-out collection of objects.
	/// </summary>
	/// <typeparam name="T">Specifies the type of elements in the queue.</typeparam>
	/// <remarks>
	/// All public  and protected members of <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1" /> are thread-safe and may be used
	/// concurrently from multiple threads.
	/// </remarks>
	[Serializable]
	[DebuggerDisplay("Count = {Count}")]
	[ComVisible(false)]
	[DebuggerTypeProxy(typeof(SystemCollectionsConcurrent_ProducerConsumerCollectionDebugView<>))]
	[HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
	public class ConcurrentQueue<T> : IProducerConsumerCollection<T>, IEnumerable<T>, ICollection, IEnumerable
	{
		/// <summary>
		/// private class for ConcurrentQueue. 
		/// a queue is a linked list of small arrays, each node is called a segment.
		/// A segment contains an array, a pointer to the next segment, and m_low, m_high indices recording
		/// the first and last valid elements of the array.
		/// </summary>
		private class Segment
		{
			internal volatile T[] m_array;

			private volatile int[] m_state;

			private volatile Segment m_next;

			internal readonly long m_index;

			private volatile int m_low;

			private volatile int m_high;

			/// <summary>
			/// return the next segment
			/// </summary>
			internal Segment Next => m_next;

			/// <summary>
			/// return true if the current segment is empty (doesn't have any element available to dequeue, 
			/// false otherwise
			/// </summary>
			internal bool IsEmpty => Low > High;

			/// <summary>
			/// return the position of the head of the current segment
			/// </summary>
			internal int Low => Math.Min(m_low, 32);

			/// <summary>
			/// return the logical position of the tail of the current segment      
			/// </summary>
			internal int High => Math.Min(m_high, 31);

			/// <summary>
			/// Create and initialize a segment with the specified index.
			/// </summary>
			internal Segment(long index)
			{
				m_array = new T[32];
				m_state = new int[32];
				m_high = -1;
				m_index = index;
			}

			/// <summary>
			/// Add an element to the tail of the current segment
			/// exclusively called by ConcurrentQueue.InitializedFromCollection
			/// InitializeFromCollection is responsible to guaratee that there is no index overflow,
			/// and there is no contention
			/// </summary>
			/// <param name="value"></param>
			internal void UnsafeAdd(T value)
			{
				m_high++;
				m_array[m_high] = value;
				m_state[m_high] = 1;
			}

			/// <summary>
			/// Create a new segment and append to the current one
			/// Does not update the m_tail pointer
			/// exclusively called by ConcurrentQueue.InitializedFromCollection
			/// InitializeFromCollection is responsible to guaratee that there is no index overflow,
			/// and there is no contention
			/// </summary>
			/// <returns>the reference to the new Segment</returns>
			internal Segment UnsafeGrow()
			{
				return m_next = new Segment(m_index + 1);
			}

			/// <summary>
			/// Create a new segment and append to the current one
			/// Update the m_tail pointer
			/// This method is called when there is no contention
			/// </summary>
			internal void Grow(ref Segment tail)
			{
				Segment segment = (m_next = new Segment(m_index + 1));
				tail = m_next;
			}

			/// <summary>
			/// Try to append an element at the end of this segment.
			/// </summary>
			/// <param name="value">the element to append</param>
			/// <param name="tail">The tail.</param>
			/// <returns>true if the element is appended, false if the current segment is full</returns>
			/// <remarks>if appending the specified element succeeds, and after which the segment is full, 
			/// then grow the segment</remarks>
			internal bool TryAppend(T value, ref Segment tail)
			{
				if (m_high >= 31)
				{
					return false;
				}
				int num = 32;
				try
				{
				}
				finally
				{
					num = Interlocked.Increment(ref m_high);
					if (num <= 31)
					{
						m_array[num] = value;
						m_state[num] = 1;
					}
					if (num == 31)
					{
						Grow(ref tail);
					}
				}
				return num <= 31;
			}

			/// <summary>
			/// try to remove an element from the head of current segment
			/// </summary>
			/// <param name="result">The result.</param>
			/// <param name="head">The head.</param>
			/// <returns>return false only if the current segment is empty</returns>
			internal bool TryRemove(out T result, ref Segment head)
			{
				SpinWait spinWait = default(SpinWait);
				int low = Low;
				int high = High;
				while (low <= high)
				{
					if (Interlocked.CompareExchange(ref m_low, low + 1, low) == low)
					{
						SpinWait spinWait2 = default(SpinWait);
						while (m_state[low] == 0)
						{
							spinWait2.SpinOnce();
						}
						result = m_array[low];
						if (low + 1 >= 32)
						{
							spinWait2 = default(SpinWait);
							while (m_next == null)
							{
								spinWait2.SpinOnce();
							}
							head = m_next;
						}
						return true;
					}
					spinWait.SpinOnce();
					low = Low;
					high = High;
				}
				result = default(T);
				return false;
			}

			/// <summary>
			/// try to peek the current segment
			/// </summary>
			/// <param name="result">holds the return value of the element at the head position, 
			/// value set to default(T) if there is no such an element</param>
			/// <returns>true if there are elements in the current segment, false otherwise</returns>
			internal bool TryPeek(out T result)
			{
				result = default(T);
				int low = Low;
				if (low > High)
				{
					return false;
				}
				SpinWait spinWait = default(SpinWait);
				while (m_state[low] == 0)
				{
					spinWait.SpinOnce();
				}
				result = m_array[low];
				return true;
			}

			/// <summary>
			/// Convert part or all of the current segment into a List
			/// </summary>
			/// <param name="start">the start position</param>
			/// <param name="end">the end position</param>
			/// <returns>the result list </returns>
			internal List<T> ToList(int start, int end)
			{
				List<T> list = new List<T>();
				for (int i = start; i <= end; i++)
				{
					SpinWait spinWait = default(SpinWait);
					while (m_state[i] == 0)
					{
						spinWait.SpinOnce();
					}
					list.Add(m_array[i]);
				}
				return list;
			}
		}

		private const int SEGMENT_SIZE = 32;

		[NonSerialized]
		private Segment m_head;

		[NonSerialized]
		private Segment m_tail;

		private T[] m_serializationArray;

		/// <summary>
		/// Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection" /> is
		/// synchronized with the SyncRoot.
		/// </summary>
		/// <value>true if access to the <see cref="T:System.Collections.ICollection" /> is synchronized
		/// with the SyncRoot; otherwise, false. For <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1" />, this property always
		/// returns false.</value>
		bool ICollection.IsSynchronized => false;

		/// <summary>
		/// Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />. This property is not supported.
		/// </summary>
		/// <exception cref="T:System.NotSupportedException">The SyncRoot property is not supported.</exception>
		object ICollection.SyncRoot
		{
			get
			{
				throw new NotSupportedException(Environment2.GetResourceString("ConcurrentCollection_SyncRoot_NotSupported"));
			}
		}

		/// <summary>
		/// Gets a value that indicates whether the <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1" /> is empty.
		/// </summary>
		/// <value>true if the <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1" /> is empty; otherwise, false.</value>
		/// <remarks>
		/// For determining whether the collection contains any items, use of this property is recommended
		/// rather than retrieving the number of items from the <see cref="P:System.Collections.Concurrent.ConcurrentQueue`1.Count" /> property and comparing it
		/// to 0.  However, as this collection is intended to be accessed concurrently, it may be the case
		/// that another thread will modify the collection after <see cref="P:System.Collections.Concurrent.ConcurrentQueue`1.IsEmpty" /> returns, thus invalidating
		/// the result.
		/// </remarks>
		public bool IsEmpty
		{
			get
			{
				Segment head = m_head;
				if (!head.IsEmpty)
				{
					return false;
				}
				if (head.Next == null)
				{
					return true;
				}
				SpinWait spinWait = default(SpinWait);
				while (head.IsEmpty)
				{
					if (head.Next == null)
					{
						return true;
					}
					spinWait.SpinOnce();
					head = m_head;
				}
				return false;
			}
		}

		/// <summary>
		/// Gets the number of elements contained in the <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1" />.
		/// </summary>
		/// <value>The number of elements contained in the <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1" />.</value>
		/// <remarks>
		/// For determining whether the collection contains any items, use of the <see cref="P:System.Collections.Concurrent.ConcurrentQueue`1.IsEmpty" />
		/// property is recommended rather than retrieving the number of items from the <see cref="P:System.Collections.Concurrent.ConcurrentQueue`1.Count" />
		/// property and comparing it to 0.
		/// </remarks>
		public int Count
		{
			get
			{
				GetHeadTailPositions(out var head, out var tail, out var headLow, out var tailHigh);
				if (head == tail)
				{
					return tailHigh - headLow + 1;
				}
				int num = 32 - headLow;
				num += 32 * (int)(tail.m_index - head.m_index - 1);
				return num + (tailHigh + 1);
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1" /> class.
		/// </summary>
		public ConcurrentQueue()
		{
			m_head = (m_tail = new Segment(0L));
		}

		/// <summary>
		/// Initializes the contents of the queue from an existing collection.
		/// </summary>
		/// <param name="collection">A collection from which to copy elements.</param>
		private void InitializeFromCollection(IEnumerable<T> collection)
		{
			m_head = (m_tail = new Segment(0L));
			int num = 0;
			foreach (T item in collection)
			{
				m_tail.UnsafeAdd(item);
				num++;
				if (num >= 32)
				{
					m_tail = m_tail.UnsafeGrow();
					num = 0;
				}
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1" />
		/// class that contains elements copied from the specified collection
		/// </summary>
		/// <param name="collection">The collection whose elements are copied to the new <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1" />.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="collection" /> argument is
		/// null.</exception>
		public ConcurrentQueue(IEnumerable<T> collection)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			InitializeFromCollection(collection);
		}

		/// <summary>
		/// Get the data array to be serialized
		/// </summary>
		[OnSerializing]
		private void OnSerializing(StreamingContext context)
		{
			m_serializationArray = ToArray();
		}

		/// <summary>
		/// Construct the queue from a previously seiralized one
		/// </summary>
		[OnDeserialized]
		private void OnDeserialized(StreamingContext context)
		{
			InitializeFromCollection(m_serializationArray);
			m_serializationArray = null;
		}

		/// <summary>
		/// Copies the elements of the <see cref="T:System.Collections.ICollection" /> to an <see cref="T:System.Array" />, starting at a particular
		/// <see cref="T:System.Array" /> index.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array">Array</see> that is the
		/// destination of the elements copied from the
		/// <see cref="T:System.Collections.Concurrent.ConcurrentBag" />. The <see cref="T:System.Array">Array</see> must have zero-based indexing.</param>
		/// <param name="index">The zero-based index in <paramref name="array" /> at which copying
		/// begins.</param>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="array" /> is a null reference (Nothing in
		/// Visual Basic).</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index" /> is less than
		/// zero.</exception>
		/// <exception cref="T:System.ArgumentException">
		/// <paramref name="array" /> is multidimensional. -or-
		/// <paramref name="array" /> does not have zero-based indexing. -or-
		/// <paramref name="index" /> is equal to or greater than the length of the <paramref name="array" />
		/// -or- The number of elements in the source <see cref="T:System.Collections.ICollection" /> is
		/// greater than the available space from <paramref name="index" /> to the end of the destination
		/// <paramref name="array" />. -or- The type of the source <see cref="T:System.Collections.ICollection" /> cannot be cast automatically to the type of the
		/// destination <paramref name="array" />.
		/// </exception>
		void ICollection.CopyTo(Array array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			((ICollection)ToList()).CopyTo(array, index);
		}

		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> that can be used to iterate through the collection.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<T>)this).GetEnumerator();
		}

		/// <summary>
		/// Attempts to add an object to the <see cref="T:System.Collections.Concurrent.IProducerConsumerCollection{T}" />.
		/// </summary>
		/// <param name="item">The object to add to the <see cref="T:System.Collections.Concurrent.IProducerConsumerCollection{T}" />. The value can be a null
		/// reference (Nothing in Visual Basic) for reference types.
		/// </param>
		/// <returns>true if the object was added successfully; otherwise, false.</returns>
		/// <remarks>For <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1" />, this operation will always add the object to the
		/// end of the <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1" />
		/// and return true.</remarks>
		bool IProducerConsumerCollection<T>.TryAdd(T item)
		{
			Enqueue(item);
			return true;
		}

		/// <summary>
		/// Attempts to remove and return an object from the <see cref="T:System.Collections.Concurrent.IProducerConsumerCollection{T}" />.
		/// </summary>
		/// <param name="item">
		/// When this method returns, if the operation was successful, <paramref name="item" /> contains the
		/// object removed. If no object was available to be removed, the value is unspecified.
		/// </param>
		/// <returns>true if an element was removed and returned succesfully; otherwise, false.</returns>
		/// <remarks>For <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1" />, this operation will attempt to remove the object
		/// from the beginning of the <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1" />.
		/// </remarks>
		bool IProducerConsumerCollection<T>.TryTake(out T item)
		{
			return TryDequeue(out item);
		}

		/// <summary>
		/// Copies the elements stored in the <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1" /> to a new array.
		/// </summary>
		/// <returns>A new array containing a snapshot of elements copied from the <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1" />.</returns>
		public T[] ToArray()
		{
			return ToList().ToArray();
		}

		/// <summary>
		/// Copies the <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1" /> elements to a new <see cref="T:System.Collections.Generic.List{T}" />.
		/// </summary>
		/// <returns>A new <see cref="T:System.Collections.Generic.List{T}" /> containing a snapshot of
		/// elements copied from the <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1" />.</returns>
		private List<T> ToList()
		{
			GetHeadTailPositions(out var head, out var tail, out var headLow, out var tailHigh);
			if (head == tail)
			{
				return head.ToList(headLow, tailHigh);
			}
			List<T> list = new List<T>(head.ToList(headLow, 31));
			for (Segment next = head.Next; next != tail; next = next.Next)
			{
				list.AddRange(next.ToList(0, 31));
			}
			list.AddRange(tail.ToList(0, tailHigh));
			return list;
		}

		/// <summary>
		/// Store the position of the current head and tail positions.
		/// </summary>
		/// <param name="head">return the head segment</param>
		/// <param name="tail">return the tail segment</param>
		/// <param name="headLow">return the head offset</param>
		/// <param name="tailHigh">return the tail offset</param>
		private void GetHeadTailPositions(out Segment head, out Segment tail, out int headLow, out int tailHigh)
		{
			head = m_head;
			tail = m_tail;
			headLow = head.Low;
			tailHigh = tail.High;
			SpinWait spinWait = default(SpinWait);
			while (head != m_head || tail != m_tail || headLow != head.Low || tailHigh != tail.High || head.m_index > tail.m_index)
			{
				spinWait.SpinOnce();
				head = m_head;
				tail = m_tail;
				headLow = head.Low;
				tailHigh = tail.High;
			}
		}

		/// <summary>
		/// Copies the <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1" /> elements to an existing one-dimensional <see cref="T:System.Array">Array</see>, starting at the specified array index.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array">Array</see> that is the
		/// destination of the elements copied from the
		/// <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1" />. The <see cref="T:System.Array">Array</see> must have zero-based
		/// indexing.</param>
		/// <param name="index">The zero-based index in <paramref name="array" /> at which copying
		/// begins.</param>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="array" /> is a null reference (Nothing in
		/// Visual Basic).</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index" /> is less than
		/// zero.</exception>
		/// <exception cref="T:System.ArgumentException"><paramref name="index" /> is equal to or greater than the
		/// length of the <paramref name="array" />
		/// -or- The number of elements in the source <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1" /> is greater than the
		/// available space from <paramref name="index" /> to the end of the destination <paramref name="array" />.
		/// </exception>
		public void CopyTo(T[] array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			ToList().CopyTo(array, index);
		}

		/// <summary>
		/// Returns an enumerator that iterates through the <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1" />.
		/// </summary>
		/// <returns>An enumerator for the contents of the <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1" />.</returns>
		/// <remarks>
		/// The enumeration represents a moment-in-time snapshot of the contents
		/// of the queue.  It does not reflect any updates to the collection after 
		/// <see cref="M:System.Collections.Concurrent.ConcurrentQueue`1.GetEnumerator" /> was called.  The enumerator is safe to use
		/// concurrently with reads from and writes to the queue.
		/// </remarks>
		public IEnumerator<T> GetEnumerator()
		{
			return ToList().GetEnumerator();
		}

		/// <summary>
		/// Adds an object to the end of the <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1" />.
		/// </summary>
		/// <param name="item">The object to add to the end of the <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1" />. The value can be a null reference
		/// (Nothing in Visual Basic) for reference types.
		/// </param>
		public void Enqueue(T item)
		{
			SpinWait spinWait = default(SpinWait);
			while (true)
			{
				Segment tail = m_tail;
				if (tail.TryAppend(item, ref m_tail))
				{
					break;
				}
				spinWait.SpinOnce();
			}
		}

		/// <summary>
		/// Attempts to remove and return the object at the beginning of the <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1" />.
		/// </summary>
		/// <param name="result">
		/// When this method returns, if the operation was successful, <paramref name="result" /> contains the
		/// object removed. If no object was available to be removed, the value is unspecified.
		/// </param>
		/// <returns>true if an element was removed and returned from the beggining of the <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1" />
		/// succesfully; otherwise, false.</returns>
		public bool TryDequeue(out T result)
		{
			while (!IsEmpty)
			{
				Segment head = m_head;
				if (head.TryRemove(out result, ref m_head))
				{
					return true;
				}
			}
			result = default(T);
			return false;
		}

		/// <summary>
		/// Attempts to return an object from the beginning of the <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1" />
		/// without removing it.
		/// </summary>
		/// <param name="result">When this method returns, <paramref name="result" /> contains an object from
		/// the beginning of the <see cref="T:System.Collections.Concurrent.ConccurrentQueue{T}" /> or an
		/// unspecified value if the operation failed.</param>
		/// <returns>true if and object was returned successfully; otherwise, false.</returns>
		public bool TryPeek(out T result)
		{
			while (!IsEmpty)
			{
				Segment head = m_head;
				if (head.TryPeek(out result))
				{
					return true;
				}
			}
			result = default(T);
			return false;
		}
	}
}
