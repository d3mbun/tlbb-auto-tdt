using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Threading;

namespace System.Collections.Concurrent
{
	/// <summary>
	/// Represents a thread-safe last-in, first-out collection of objects.
	/// </summary>
	/// <typeparam name="T">Specifies the type of elements in the stack.</typeparam>
	/// <remarks>
	/// All public and protected members of <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" /> are thread-safe and may be used
	/// concurrently from multiple threads.
	/// </remarks>
	[Serializable]
	[DebuggerDisplay("Count = {Count}")]
	[DebuggerTypeProxy(typeof(SystemCollectionsConcurrent_ProducerConsumerCollectionDebugView<>))]
	[HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
	public class ConcurrentStack<T> : IProducerConsumerCollection<T>, IEnumerable<T>, ICollection, IEnumerable
	{
		/// <summary>
		/// A simple (internal) node type used to store elements of concurrent stacks and queues.
		/// </summary>
		private class Node
		{
			internal T m_value;

			internal Node m_next;

			/// <summary>
			/// Constructs a new node with the specified value and no next node.
			/// </summary>
			/// <param name="value">The value of the node.</param>
			internal Node(T value)
			{
				m_value = value;
				m_next = null;
			}
		}

		private const int BACKOFF_MAX_YIELDS = 8;

		[NonSerialized]
		private volatile Node m_head;

		private T[] m_serializationArray;

		/// <summary>
		/// Gets a value that indicates whether the <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" /> is empty.
		/// </summary>
		/// <value>true if the <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" /> is empty; otherwise, false.</value>
		/// <remarks>
		/// For determining whether the collection contains any items, use of this property is recommended
		/// rather than retrieving the number of items from the <see cref="P:System.Collections.Concurrent.ConcurrentStack`1.Count" /> property and comparing it
		/// to 0.  However, as this collection is intended to be accessed concurrently, it may be the case
		/// that another thread will modify the collection after <see cref="P:System.Collections.Concurrent.ConcurrentStack`1.IsEmpty" /> returns, thus invalidating
		/// the result.
		/// </remarks>
		public bool IsEmpty => m_head == null;

		/// <summary>
		/// Gets the number of elements contained in the <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" />.
		/// </summary>
		/// <value>The number of elements contained in the <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" />.</value>
		/// <remarks>
		/// For determining whether the collection contains any items, use of the <see cref="P:System.Collections.Concurrent.ConcurrentStack`1.IsEmpty" />
		/// property is recommended rather than retrieving the number of items from the <see cref="P:System.Collections.Concurrent.ConcurrentStack`1.Count" />
		/// property and comparing it to 0.
		/// </remarks>
		public int Count
		{
			get
			{
				int num = 0;
				for (Node node = m_head; node != null; node = node.m_next)
				{
					num++;
				}
				return num;
			}
		}

		/// <summary>
		/// Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection" /> is
		/// synchronized with the SyncRoot.
		/// </summary>
		/// <value>true if access to the <see cref="T:System.Collections.ICollection" /> is synchronized
		/// with the SyncRoot; otherwise, false. For <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" />, this property always
		/// returns false.</value>
		bool ICollection.IsSynchronized => false;

		/// <summary>
		/// Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />. This property is not supported.
		/// </summary>
		/// <exception cref="T:System.NotSupportedException">The SyncRoot property is not supported</exception>
		object ICollection.SyncRoot
		{
			get
			{
				throw new NotSupportedException(Environment2.GetResourceString("ConcurrentCollection_SyncRoot_NotSupported"));
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" />
		/// class.
		/// </summary>
		public ConcurrentStack()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" />
		/// class that contains elements copied from the specified collection
		/// </summary>
		/// <param name="collection">The collection whose elements are copied to the new <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" />.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="collection" /> argument is
		/// null.</exception>
		public ConcurrentStack(IEnumerable<T> collection)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			InitializeFromCollection(collection);
		}

		/// <summary>
		/// Initializes the contents of the stack from an existing collection.
		/// </summary>
		/// <param name="collection">A collection from which to copy elements.</param>
		private void InitializeFromCollection(IEnumerable<T> collection)
		{
			Node node = null;
			foreach (T item in collection)
			{
				Node node2 = new Node(item);
				node2.m_next = node;
				node = node2;
			}
			m_head = node;
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
		/// Construct the stack from a previously seiralized one
		/// </summary>
		[OnDeserialized]
		private void OnDeserialized(StreamingContext context)
		{
			Node node = null;
			Node head = null;
			for (int i = 0; i < m_serializationArray.Length; i++)
			{
				Node node2 = new Node(m_serializationArray[i]);
				if (node == null)
				{
					head = node2;
				}
				else
				{
					node.m_next = node2;
				}
				node = node2;
			}
			m_head = head;
			m_serializationArray = null;
		}

		/// <summary>
		/// Removes all objects from the <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" />.
		/// </summary>
		public void Clear()
		{
			m_head = null;
		}

		/// <summary>
		/// Copies the elements of the <see cref="T:System.Collections.ICollection" /> to an <see cref="T:System.Array" />, starting at a particular
		/// <see cref="T:System.Array" /> index.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of
		/// the elements copied from the
		/// <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" />. The <see cref="T:System.Array" /> must
		/// have zero-based indexing.</param>
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
		/// Copies the <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" /> elements to an existing one-dimensional <see cref="T:System.Array" />, starting at the specified array index.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of
		/// the elements copied from the
		/// <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" />. The <see cref="T:System.Array" /> must have zero-based
		/// indexing.</param>
		/// <param name="index">The zero-based index in <paramref name="array" /> at which copying
		/// begins.</param>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="array" /> is a null reference (Nothing in
		/// Visual Basic).</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index" /> is less than
		/// zero.</exception>
		/// <exception cref="T:System.ArgumentException"><paramref name="index" /> is equal to or greater than the
		/// length of the <paramref name="array" />
		/// -or- The number of elements in the source <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" /> is greater than the
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
		/// Inserts an object at the top of the <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" />.
		/// </summary>
		/// <param name="item">The object to push onto the <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" />. The value can be
		/// a null reference (Nothing in Visual Basic) for reference types.
		/// </param>
		public void Push(T item)
		{
			Node node = new Node(item);
			node.m_next = m_head;
			if (Interlocked.CompareExchange(ref m_head, node, node.m_next) != node.m_next)
			{
				PushCore(node, node);
			}
		}

		/// <summary>
		/// Inserts multiple objects at the top of the <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" /> atomically.
		/// </summary>
		/// <param name="items">The objects to push onto the <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" />.</param>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="items" /> is a null reference
		/// (Nothing in Visual Basic).</exception>
		/// <remarks>
		/// When adding multiple items to the stack, using PushRange is a more efficient
		/// mechanism than using <see cref="M:System.Collections.Concurrent.ConcurrentStack`1.Push(`0)" /> one item at a time.  Additionally, PushRange
		/// guarantees that all of the elements will be added atomically, meaning that no other threads will
		/// be able to inject elements between the elements being pushed.  Items at lower indices in
		/// the <paramref name="items" /> array will be pushed before items at higher indices.
		/// </remarks>
		public void PushRange(T[] items)
		{
			if (items == null)
			{
				throw new ArgumentNullException("items");
			}
			PushRange(items, 0, items.Length);
		}

		/// <summary>
		/// Inserts multiple objects at the top of the <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" /> atomically.
		/// </summary>
		/// <param name="items">The objects to push onto the <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" />.</param>
		/// <param name="startIndex">The zero-based offset in <paramref name="items" /> at which to begin
		/// inserting elements onto the top of the <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" />.</param>
		/// <param name="count">The number of elements to be inserted onto the top of the <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" />.</param>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="items" /> is a null reference
		/// (Nothing in Visual Basic).</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="startIndex" /> or <paramref name="count" /> is negative. Or <paramref name="startIndex" /> is greater than or equal to the length 
		/// of <paramref name="items" />.</exception>
		/// <exception cref="T:System.ArgumentException"><paramref name="startIndex" /> + <paramref name="count" /> is
		/// greater than the length of <paramref name="items" />.</exception>
		/// <remarks>
		/// When adding multiple items to the stack, using PushRange is a more efficient
		/// mechanism than using <see cref="M:System.Collections.Concurrent.ConcurrentStack`1.Push(`0)" /> one item at a time. Additionally, PushRange
		/// guarantees that all of the elements will be added atomically, meaning that no other threads will
		/// be able to inject elements between the elements being pushed. Items at lower indices in the
		/// <paramref name="items" /> array will be pushed before items at higher indices.
		/// </remarks>
		public void PushRange(T[] items, int startIndex, int count)
		{
			ValidatePushPopRangeInput(items, startIndex, count);
			if (count != 0)
			{
				Node node;
				Node node2 = (node = new Node(items[startIndex]));
				for (int i = startIndex + 1; i < startIndex + count; i++)
				{
					Node node3 = new Node(items[i]);
					node3.m_next = node2;
					node2 = node3;
				}
				node.m_next = m_head;
				if (Interlocked.CompareExchange(ref m_head, node2, node.m_next) != node.m_next)
				{
					PushCore(node2, node);
				}
			}
		}

		/// <summary>
		/// Push one or many nodes into the stack, if head and tails are equal then push one node to the stack other wise push the list between head
		/// and tail to the stack
		/// </summary>
		/// <param name="head">The head pointer to the new list</param>
		/// <param name="tail">The tail pointer to the new list</param>
		private void PushCore(Node head, Node tail)
		{
			SpinWait spinWait = default(SpinWait);
			do
			{
				spinWait.SpinOnce();
				tail.m_next = m_head;
			}
			while (Interlocked.CompareExchange(ref m_head, head, tail.m_next) != tail.m_next);
		}

		/// <summary>
		/// Local helper function to validate the Pop Push range methods input
		/// </summary>
		private void ValidatePushPopRangeInput(T[] items, int startIndex, int count)
		{
			if (items == null)
			{
				throw new ArgumentNullException("items");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", Environment2.GetResourceString("ConcurrentStack_PushPopRange_CountOutOfRange"));
			}
			int num = items.Length;
			if (startIndex >= num || startIndex < 0)
			{
				throw new ArgumentOutOfRangeException("startIndex", Environment2.GetResourceString("ConcurrentStack_PushPopRange_StartOutOfRange"));
			}
			if (num - count < startIndex)
			{
				throw new ArgumentException(Environment2.GetResourceString("ConcurrentStack_PushPopRange_InvalidCount"));
			}
		}

		/// <summary>
		/// Attempts to add an object to the <see cref="T:System.Collections.Concurrent.IProducerConsumerCollection{T}" />.
		/// </summary>
		/// <param name="item">The object to add to the <see cref="T:System.Collections.Concurrent.IProducerConsumerCollection{T}" />. The value can be a null
		/// reference (Nothing in Visual Basic) for reference types.
		/// </param>
		/// <returns>true if the object was added successfully; otherwise, false.</returns>
		/// <remarks>For <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" />, this operation
		/// will always insert the object onto the top of the <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" />
		/// and return true.</remarks>
		bool IProducerConsumerCollection<T>.TryAdd(T item)
		{
			Push(item);
			return true;
		}

		/// <summary>
		/// Attempts to return an object from the top of the <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" />
		/// without removing it.
		/// </summary>
		/// <param name="result">When this method returns, <paramref name="result" /> contains an object from
		/// the top of the <see cref="T:System.Collections.Concurrent.ConccurrentStack{T}" /> or an
		/// unspecified value if the operation failed.</param>
		/// <returns>true if and object was returned successfully; otherwise, false.</returns>
		public bool TryPeek(out T result)
		{
			Node head = m_head;
			if (head == null)
			{
				result = default(T);
				return false;
			}
			result = head.m_value;
			return true;
		}

		/// <summary>
		/// Attempts to pop and return the object at the top of the <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" />.
		/// </summary>
		/// <param name="result">
		/// When this method returns, if the operation was successful, <paramref name="result" /> contains the
		/// object removed. If no object was available to be removed, the value is unspecified.
		/// </param>
		/// <returns>true if an element was removed and returned from the top of the <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" />
		/// succesfully; otherwise, false.</returns>
		public bool TryPop(out T result)
		{
			Node head = m_head;
			if (head == null)
			{
				result = default(T);
				return false;
			}
			if (Interlocked.CompareExchange(ref m_head, head.m_next, head) == head)
			{
				result = head.m_value;
				return true;
			}
			return TryPopCore(out result);
		}

		/// <summary>
		/// Attempts to pop and return multiple objects from the top of the <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" />
		/// atomically.
		/// </summary>
		/// <param name="items">
		/// The <see cref="T:System.Array" /> to which objects popped from the top of the <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" /> will be added.
		/// </param>
		/// <returns>The number of objects successfully popped from the top of the <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" /> and inserted in
		/// <paramref name="items" />.</returns>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="items" /> is a null argument (Nothing
		/// in Visual Basic).</exception>
		/// <remarks>
		/// When popping multiple items, if there is little contention on the stack, using
		/// TryPopRange can be more efficient than using <see cref="M:System.Collections.Concurrent.ConcurrentStack`1.TryPop(`0@)" />
		/// once per item to be removed.  Nodes fill the <paramref name="items" />
		/// with the first node to be popped at the startIndex, the second node to be popped
		/// at startIndex + 1, and so on.
		/// </remarks>
		public int TryPopRange(T[] items)
		{
			if (items == null)
			{
				throw new ArgumentNullException("items");
			}
			return TryPopRange(items, 0, items.Length);
		}

		/// <summary>
		/// Attempts to pop and return multiple objects from the top of the <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" />
		/// atomically.
		/// </summary>
		/// <param name="items">
		/// The <see cref="T:System.Array" /> to which objects popped from the top of the <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" /> will be added.
		/// </param>
		/// <param name="startIndex">The zero-based offset in <paramref name="items" /> at which to begin
		/// inserting elements from the top of the <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" />.</param>
		/// <param name="count">The number of elements to be popped from top of the <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" /> and inserted into <paramref name="items" />.</param>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="items" /> is a null reference
		/// (Nothing in Visual Basic).</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="startIndex" /> or <paramref name="count" /> is negative. Or <paramref name="startIndex" /> is greater than or equal to the length 
		/// of <paramref name="items" />.</exception>
		/// <exception cref="T:System.ArgumentException"><paramref name="startIndex" /> + <paramref name="count" /> is
		/// greater than the length of <paramref name="items" />.</exception>
		/// <remarks>
		/// When popping multiple items, if there is little contention on the stack, using
		/// TryPopRange can be more efficient than using <see cref="M:System.Collections.Concurrent.ConcurrentStack`1.TryPop(`0@)" />
		/// once per item to be removed.  Nodes fill the <paramref name="items" />
		/// with the first node to be popped at the startIndex, the second node to be popped
		/// at startIndex + 1, and so on.
		/// </remarks>
		public int TryPopRange(T[] items, int startIndex, int count)
		{
			ValidatePushPopRangeInput(items, startIndex, count);
			if (count == 0)
			{
				return 0;
			}
			Node poppedHead;
			int num = TryPopCore(count, out poppedHead);
			if (num > 0)
			{
				CopyRemovedItems(poppedHead, items, startIndex, num);
			}
			return num;
		}

		/// <summary>
		/// Local helper function to Pop an item from the stack, slow path
		/// </summary>
		/// <param name="result">The popped item</param>
		/// <returns>True if succeeded, false otherwise</returns>
		private bool TryPopCore(out T result)
		{
			if (TryPopCore(1, out var poppedHead) == 1)
			{
				result = poppedHead.m_value;
				return true;
			}
			result = default(T);
			return false;
		}

		/// <summary>
		/// Slow path helper for TryPop. This method assumes an initial attempt to pop an element
		/// has already occurred and failed, so it begins spinning right away.
		/// </summary>
		/// <param name="count">The number of items to pop.</param>
		/// <param name="poppedHead">
		/// When this method returns, if the pop succeeded, contains the removed object. If no object was
		/// available to be removed, the value is unspecified. This parameter is passed uninitialized.
		/// </param>
		/// <returns>True if an element was removed and returned; otherwise, false.</returns>
		private int TryPopCore(int count, out Node poppedHead)
		{
			SpinWait spinWait = default(SpinWait);
			int num = 1;
			Random random = new Random(Environment.TickCount & 0x7FFFFFFF);
			Node head;
			int i;
			while (true)
			{
				head = m_head;
				if (head == null)
				{
					poppedHead = null;
					return 0;
				}
				Node node = head;
				for (i = 1; i < count; i++)
				{
					if (node.m_next == null)
					{
						break;
					}
					node = node.m_next;
				}
				if (Interlocked.CompareExchange(ref m_head, node.m_next, head) == head)
				{
					break;
				}
				for (int j = 0; j < num; j++)
				{
					spinWait.SpinOnce();
				}
				num = (spinWait.NextSpinWillYield ? random.Next(1, 8) : (num * 2));
			}
			poppedHead = head;
			return i;
		}

		/// <summary>
		/// Local helper function to copy the poped elements into a given collection
		/// </summary>
		/// <param name="head">The head of the list to be copied</param>
		/// <param name="collection">The collection to place the popped items in</param>
		/// <param name="startIndex">the beginning of index of where to place the popped items</param>
		/// <param name="nodesCount">The number of nodes.</param>
		private void CopyRemovedItems(Node head, T[] collection, int startIndex, int nodesCount)
		{
			Node node = head;
			for (int i = startIndex; i < startIndex + nodesCount; i++)
			{
				collection[i] = node.m_value;
				node = node.m_next;
			}
		}

		/// <summary>
		/// Attempts to remove and return an object from the <see cref="T:System.Collections.Concurrent.IProducerConsumerCollection{T}" />.
		/// </summary>
		/// <param name="item">
		/// When this method returns, if the operation was successful, <paramref name="item" /> contains the
		/// object removed. If no object was available to be removed, the value is unspecified.
		/// </param>
		/// <returns>true if an element was removed and returned succesfully; otherwise, false.</returns>
		/// <remarks>For <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" />, this operation will attempt to pope the object at
		/// the top of the <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" />.
		/// </remarks>
		bool IProducerConsumerCollection<T>.TryTake(out T item)
		{
			return TryPop(out item);
		}

		/// <summary>
		/// Copies the items stored in the <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" /> to a new array.
		/// </summary>
		/// <returns>A new array containing a snapshot of elements copied from the <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" />.</returns>
		public T[] ToArray()
		{
			return ToList().ToArray();
		}

		/// <summary>
		/// Returns an array containing a snapshot of the list's contents, using
		/// the target list node as the head of a region in the list.
		/// </summary>
		/// <returns>An array of the list's contents.</returns>
		private List<T> ToList()
		{
			List<T> list = new List<T>();
			for (Node node = m_head; node != null; node = node.m_next)
			{
				list.Add(node.m_value);
			}
			return list;
		}

		/// <summary>
		/// Returns an enumerator that iterates through the <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" />.
		/// </summary>
		/// <returns>An enumerator for the <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" />.</returns>
		/// <remarks>
		/// The enumeration represents a moment-in-time snapshot of the contents
		/// of the stack.  It does not reflect any updates to the collection after 
		/// <see cref="M:System.Collections.Concurrent.ConcurrentStack`1.GetEnumerator" /> was called.  The enumerator is safe to use
		/// concurrently with reads from and writes to the stack.
		/// </remarks>
		public IEnumerator<T> GetEnumerator()
		{
			return GetEnumerator(m_head);
		}

		private IEnumerator<T> GetEnumerator(Node head)
		{
			for (Node current = head; current != null; current = current.m_next)
			{
				yield return current.m_value;
			}
		}

		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> that can be used to iterate through
		/// the collection.</returns>
		/// <remarks>
		/// The enumeration represents a moment-in-time snapshot of the contents of the stack. It does not
		/// reflect any updates to the collection after
		/// <see cref="M:System.Collections.Concurrent.ConcurrentStack`1.GetEnumerator" /> was called. The enumerator is safe to use concurrently with reads
		/// from and writes to the stack.
		/// </remarks>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<T>)this).GetEnumerator();
		}
	}
}
