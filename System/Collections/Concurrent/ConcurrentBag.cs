using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Threading;

namespace System.Collections.Concurrent
{
	/// <summary>
	/// Represents an thread-safe, unordered collection of objects. 
	/// </summary>
	/// <typeparam name="T">Specifies the type of elements in the bag.</typeparam>
	/// <remarks>
	/// <para>
	/// Bags are useful for storing objects when ordering doesn't matter, and unlike sets, bags support
	/// duplicates. <see cref="T:System.Collections.Concurrent.ConcurrentBag`1" /> is a thread-safe bag implementation, optimized for
	/// scenarios where the same thread will be both producing and consuming data stored in the bag.
	/// </para>
	/// <para>
	/// <see cref="T:System.Collections.Concurrent.ConcurrentBag`1" /> accepts null reference (Nothing in Visual Basic) as a valid 
	/// value for reference types.
	/// </para>
	/// <para>
	/// All public and protected members of <see cref="T:System.Collections.Concurrent.ConcurrentBag`1" /> are thread-safe and may be used
	/// concurrently from multiple threads.
	/// </para>
	/// </remarks>
	[Serializable]
	[DebuggerTypeProxy(typeof(SystemThreadingCollection_IProducerConsumerCollectionDebugView<>))]
	[DebuggerDisplay("Count = {Count}")]
	[ComVisible(false)]
	[HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
	public class ConcurrentBag<T> : IProducerConsumerCollection<T>, IEnumerable<T>, ICollection, IEnumerable
	{
		/// <summary>
		/// A class that represents a node in the lock thread list
		/// </summary>
		[Serializable]
		internal class Node
		{
			public T m_value;

			public Node m_next;

			public Node m_prev;

			public Node(T value)
			{
				m_value = value;
			}
		}

		/// <summary>
		/// A class that represents the lock thread list
		/// </summary>
		internal class ThreadLocalList
		{
			internal Node m_head;

			private Node m_tail;

			internal volatile int m_currentOp;

			private int m_count;

			internal int m_stealCount;

			internal ThreadLocalList m_nextList;

			internal bool m_lockTaken;

			internal Thread m_ownerThread;

			internal volatile int m_version;

			/// <summary>
			/// Gets the total list count, it's not thread safe, may provide incorrect count if it is called concurrently
			/// </summary>
			internal int Count => m_count - m_stealCount;

			/// <summary>
			/// ThreadLocalList constructor
			/// </summary>
			/// <param name="ownerThread">The owner thread for this list</param>
			internal ThreadLocalList(Thread ownerThread)
			{
				m_ownerThread = ownerThread;
			}

			/// <summary>
			/// Add new item to head of the list
			/// </summary>
			/// <param name="item">The item to add.</param>
			/// <param name="updateCount">Whether to update the count.</param>
			internal void Add(T item, bool updateCount)
			{
				Node node;
				checked
				{
					m_count++;
					node = new Node(item);
				}
				if (m_head == null)
				{
					m_head = node;
					m_tail = node;
					m_version++;
				}
				else
				{
					node.m_next = m_head;
					m_head.m_prev = node;
					m_head = node;
				}
				if (updateCount)
				{
					m_count -= m_stealCount;
					m_stealCount = 0;
				}
			}

			/// <summary>
			/// Remove an item from the head of the list
			/// </summary>
			/// <param name="result">The removed item</param>
			internal void Remove(out T result)
			{
				Node head = m_head;
				m_head = m_head.m_next;
				if (m_head != null)
				{
					m_head.m_prev = null;
				}
				else
				{
					m_tail = null;
				}
				m_count--;
				result = head.m_value;
			}

			/// <summary>
			/// Peek an item from the head of the list
			/// </summary>
			/// <param name="result">the peeked item</param>
			/// <returns>True if succeeded, false otherwise</returns>
			internal bool Peek(out T result)
			{
				Node head = m_head;
				if (head != null)
				{
					result = head.m_value;
					return true;
				}
				result = default(T);
				return false;
			}

			/// <summary>
			/// Steal an item from the tail of the list
			/// </summary>
			/// <param name="result">the removed item</param>
			/// <param name="remove">remove or peek flag</param>
			internal void Steal(out T result, bool remove)
			{
				Node tail = m_tail;
				if (remove)
				{
					m_tail = m_tail.m_prev;
					if (m_tail != null)
					{
						m_tail.m_next = null;
					}
					else
					{
						m_head = null;
					}
					m_stealCount++;
				}
				result = tail.m_value;
			}
		}

		/// <summary>
		/// List operations
		/// </summary>
		internal enum ListOperation
		{
			None,
			Add,
			Take
		}

		[NonSerialized]
		private ThreadLocal<ThreadLocalList> m_locals;

		[NonSerialized]
		private volatile ThreadLocalList m_headList;

		[NonSerialized]
		private volatile ThreadLocalList m_tailList;

		[NonSerialized]
		private object m_globalListsLock;

		[NonSerialized]
		private bool m_needSync;

		private T[] m_serializationArray;

		/// <summary>
		/// Gets the number of elements contained in the <see cref="T:System.Collections.Concurrent.ConcurrentBag`1" />.
		/// </summary>
		/// <value>The number of elements contained in the <see cref="T:System.Collections.Concurrent.ConcurrentBag`1" />.</value>
		/// <remarks>
		/// The count returned represents a moment-in-time snapshot of the contents
		/// of the bag.  It does not reflect any updates to the collection after 
		/// <see cref="M:System.Collections.Concurrent.ConcurrentBag`1.GetEnumerator" /> was called.
		/// </remarks>
		public int Count
		{
			get
			{
				if (m_headList == null)
				{
					return 0;
				}
				bool lockTaken = false;
				try
				{
					FreezeBag(ref lockTaken);
					return GetCountInternal();
				}
				finally
				{
					UnfreezeBag(lockTaken);
				}
			}
		}

		/// <summary>
		/// Gets a value that indicates whether the <see cref="T:System.Collections.Concurrent.ConcurrentBag`1" /> is empty.
		/// </summary>
		/// <value>true if the <see cref="T:System.Collections.Concurrent.ConcurrentBag`1" /> is empty; otherwise, false.</value>
		public bool IsEmpty
		{
			get
			{
				if (m_headList == null)
				{
					return true;
				}
				bool lockTaken = false;
				try
				{
					FreezeBag(ref lockTaken);
					for (ThreadLocalList threadLocalList = m_headList; threadLocalList != null; threadLocalList = threadLocalList.m_nextList)
					{
						if (threadLocalList.m_head != null)
						{
							return false;
						}
					}
					return true;
				}
				finally
				{
					UnfreezeBag(lockTaken);
				}
			}
		}

		/// <summary>
		/// Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection" /> is
		/// synchronized with the SyncRoot.
		/// </summary>
		/// <value>true if access to the <see cref="T:System.Collections.ICollection" /> is synchronized
		/// with the SyncRoot; otherwise, false. For <see cref="T:System.Collections.Concurrent.ConcurrentBag`1" />, this property always
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
				throw new NotSupportedException("ConcurrentCollection_SyncRoot_NotSupported");
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.Collections.Concurrent.ConcurrentBag`1" />
		/// class.
		/// </summary>
		public ConcurrentBag()
		{
			Initialize(null);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.Collections.Concurrent.ConcurrentBag`1" />
		/// class that contains elements copied from the specified collection.
		/// </summary>
		/// <param name="collection">The collection whose elements are copied to the new <see cref="T:System.Collections.Concurrent.ConcurrentBag`1" />.</param>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="collection" /> is a null reference
		/// (Nothing in Visual Basic).</exception>
		public ConcurrentBag(IEnumerable<T> collection)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection", "ConcurrentBag_Ctor_ArgumentNullException");
			}
			Initialize(collection);
		}

		/// <summary>
		/// Local helper function to initalize a new bag object
		/// </summary>
		/// <param name="collection">An enumeration containing items with which to initialize this bag.</param>
		private void Initialize(IEnumerable<T> collection)
		{
			m_locals = new ThreadLocal<ThreadLocalList>();
			m_globalListsLock = new object();
			if (collection == null)
			{
				return;
			}
			ThreadLocalList threadList = GetThreadList(forceCreate: true);
			foreach (T item in collection)
			{
				AddInternal(threadList, item);
			}
		}

		/// <summary>
		/// Adds an object to the <see cref="T:System.Collections.Concurrent.ConcurrentBag`1" />.
		/// </summary>
		/// <param name="item">The object to be added to the
		/// <see cref="T:System.Collections.Concurrent.ConcurrentBag`1" />. The value can be a null reference
		/// (Nothing in Visual Basic) for reference types.</param>
		public void Add(T item)
		{
			ThreadLocalList threadList = GetThreadList(forceCreate: true);
			AddInternal(threadList, item);
		}

		/// <summary>
		/// </summary>
		/// <param name="list"></param>
		/// <param name="item"></param>
		private void AddInternal(ThreadLocalList list, T item)
		{
			bool taken = false;
			try
			{
				Interlocked.Exchange(ref list.m_currentOp, 1);
				if (list.Count < 2 || m_needSync)
				{
					list.m_currentOp = 0;
					Monitor2.Enter(list, ref taken);
				}
				list.Add(item, taken);
			}
			finally
			{
				list.m_currentOp = 0;
				if (taken)
				{
					Monitor.Exit(list);
				}
			}
		}

		/// <summary>
		/// Attempts to add an object to the <see cref="T:System.Collections.Concurrent.ConcurrentBag`1" />.
		/// </summary>
		/// <param name="item">The object to be added to the 
		/// <see cref="T:System.Collections.Concurrent.ConcurrentBag`1" />. The value can be a null reference
		/// (Nothing in Visual Basic) for reference types.</param>
		/// <returns>Always returns true</returns>
		bool IProducerConsumerCollection<T>.TryAdd(T item)
		{
			Add(item);
			return true;
		}

		/// <summary>
		/// Attempts to remove and return an object from the <see cref="T:System.Collections.Concurrent.ConcurrentBag`1" />.
		/// </summary>
		/// <param name="result">When this method returns, <paramref name="result" /> contains the object
		/// removed from the <see cref="T:System.Collections.Concurrent.ConcurrentBag`1" /> or the default value
		/// of <typeparamref name="T" /> if the operation failed.</param>
		/// <returns>true if an object was removed successfully; otherwise, false.</returns>
		public bool TryTake(out T result)
		{
			return TryTakeOrPeek(out result, take: true);
		}

		/// <summary>
		/// Attempts to return an object from the <see cref="T:System.Collections.Concurrent.ConcurrentBag`1" />
		/// without removing it.
		/// </summary>
		/// <param name="result">When this method returns, <paramref name="result" /> contains an object from
		/// the <see cref="T:System.Collections.Concurrent.ConcurrentBag`1" /> or the default value of
		/// <typeparamref name="T" /> if the operation failed.</param>
		/// <returns>true if and object was returned successfully; otherwise, false.</returns>
		public bool TryPeek(out T result)
		{
			return TryTakeOrPeek(out result, take: false);
		}

		/// <summary>
		/// Local helper function to Take or Peek an item from the bag
		/// </summary>
		/// <param name="result">To receive the item retrieved from the bag</param>
		/// <param name="take">True means Take operation, false means Peek operation</param>
		/// <returns>True if succeeded, false otherwise</returns>
		private bool TryTakeOrPeek(out T result, bool take)
		{
			ThreadLocalList threadList = GetThreadList(forceCreate: false);
			if (threadList == null || threadList.Count == 0)
			{
				return Steal(out result, take);
			}
			bool taken = false;
			try
			{
				if (take)
				{
					Interlocked.Exchange(ref threadList.m_currentOp, 2);
					if (threadList.Count <= 2 || m_needSync)
					{
						threadList.m_currentOp = 0;
						Monitor2.Enter(threadList, ref taken);
						if (threadList.Count == 0)
						{
							if (taken)
							{
								try
								{
								}
								finally
								{
									taken = false;
									Monitor.Exit(threadList);
								}
							}
							return Steal(out result, take: true);
						}
					}
					threadList.Remove(out result);
				}
				else if (!threadList.Peek(out result))
				{
					return Steal(out result, take: false);
				}
			}
			finally
			{
				threadList.m_currentOp = 0;
				if (taken)
				{
					Monitor.Exit(threadList);
				}
			}
			return true;
		}

		/// <summary>
		/// Local helper function to retrieve a thread local list by a thread object
		/// </summary>
		/// <param name="forceCreate">Create a new list if the thread does ot exist</param>
		/// <returns>The local list object</returns>
		private ThreadLocalList GetThreadList(bool forceCreate)
		{
			ThreadLocalList value = m_locals.Value;
			if (value != null)
			{
				return value;
			}
			if (forceCreate)
			{
				lock (m_globalListsLock)
				{
					if (m_headList == null)
					{
						value = (m_tailList = (m_headList = new ThreadLocalList(Thread.CurrentThread)));
					}
					else
					{
						value = GetUnownedList();
						if (value == null)
						{
							value = new ThreadLocalList(Thread.CurrentThread);
							m_tailList.m_nextList = value;
							m_tailList = value;
						}
					}
					m_locals.Value = value;
					return value;
				}
			}
			return null;
		}

		/// <summary>
		/// Try to reuse an unowned list if exist
		/// unowned lists are the lists that their owner threads are aborted or terminated
		/// this is workaround to avoid memory leaks.
		/// </summary>
		/// <returns>The list object, null if all lists are owned</returns>
		private ThreadLocalList GetUnownedList()
		{
			for (ThreadLocalList threadLocalList = m_headList; threadLocalList != null; threadLocalList = threadLocalList.m_nextList)
			{
				if (threadLocalList.m_ownerThread.ThreadState == System.Threading.ThreadState.Stopped)
				{
					threadLocalList.m_ownerThread = Thread.CurrentThread;
					return threadLocalList;
				}
			}
			return null;
		}

		/// <summary>
		/// Local helper method to steal an item from any other non empty thread
		/// It enumerate all other threads in two passes first pass acquire the lock with TryEnter if succeeded
		/// it steals the item, otherwise it enumerate them again in 2nd pass and acquire the lock using Enter
		/// </summary>
		/// <param name="result">To receive the item retrieved from the bag</param>
		/// <param name="take">Whether to remove or peek.</param>
		/// <returns>True if succeeded, false otherwise.</returns>
		private bool Steal(out T result, bool take)
		{
			bool flag;
			do
			{
				flag = false;
				List<int> list = new List<int>();
				ThreadLocalList threadLocalList;
				for (threadLocalList = m_headList; threadLocalList != null; threadLocalList = threadLocalList.m_nextList)
				{
					list.Add(threadLocalList.m_version);
					if (threadLocalList.m_head != null && TrySteal(threadLocalList, out result, take))
					{
						return true;
					}
				}
				threadLocalList = m_headList;
				foreach (int item in list)
				{
					if (item != threadLocalList.m_version)
					{
						flag = true;
						if (threadLocalList.m_head != null && TrySteal(threadLocalList, out result, take))
						{
							return true;
						}
					}
					threadLocalList = threadLocalList.m_nextList;
				}
			}
			while (flag);
			result = default(T);
			return false;
		}

		/// <summary>
		/// local helper function tries to steal an item from given local list
		/// </summary>
		private bool TrySteal(ThreadLocalList list, out T result, bool take)
		{
			lock (list)
			{
				if (CanSteal(list))
				{
					list.Steal(out result, take);
					return true;
				}
				result = default(T);
				return false;
			}
		}

		/// <summary>
		/// Local helper function to check the list if it became empty after acquiring the lock
		/// and wait if there is unsynchronized Add/Take operation in the list to be done
		/// </summary>
		/// <param name="list">The list to steal</param>
		/// <returns>True if can steal, false otherwise</returns>
		private bool CanSteal(ThreadLocalList list)
		{
			if (list.Count <= 2 && list.m_currentOp != 0)
			{
				SpinWait spinWait = default(SpinWait);
				while (list.m_currentOp != 0)
				{
					spinWait.SpinOnce();
				}
			}
			if (list.Count > 0)
			{
				return true;
			}
			return false;
		}

		/// <summary>
		/// Copies the <see cref="T:System.Collections.Concurrent.ConcurrentBag`1" /> elements to an existing
		/// one-dimensional <see cref="T:System.Array">Array</see>, starting at the specified array
		/// index.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array">Array</see> that is the
		/// destination of the elements copied from the
		/// <see cref="T:System.Collections.Concurrent.ConcurrentBag`1" />. The <see cref="T:System.Array">Array</see> must have zero-based indexing.</param>
		/// <param name="index">The zero-based index in <paramref name="array" /> at which copying
		/// begins.</param>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="array" /> is a null reference (Nothing in
		/// Visual Basic).</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index" /> is less than
		/// zero.</exception>
		/// <exception cref="T:System.ArgumentException"><paramref name="index" /> is equal to or greater than the
		/// length of the <paramref name="array" />
		/// -or- the number of elements in the source <see cref="T:System.Collections.Concurrent.ConcurrentBag`1" /> is greater than the available space from
		/// <paramref name="index" /> to the end of the destination <paramref name="array" />.</exception>
		public void CopyTo(T[] array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array", "ConcurrentBag_CopyTo_ArgumentNullException");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", "ConcurrentBag_CopyTo_ArgumentOutOfRangeException");
			}
			if (m_headList != null)
			{
				bool lockTaken = false;
				try
				{
					FreezeBag(ref lockTaken);
					ToList().CopyTo(array, index);
				}
				finally
				{
					UnfreezeBag(lockTaken);
				}
			}
		}

		/// <summary>
		/// Copies the elements of the <see cref="T:System.Collections.ICollection" /> to an <see cref="T:System.Array" />, starting at a particular
		/// <see cref="T:System.Array" /> index.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array">Array</see> that is the
		/// destination of the elements copied from the
		/// <see cref="T:System.Collections.Concurrent.ConcurrentBag`1" />. The <see cref="T:System.Array">Array</see> must have zero-based indexing.</param>
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
				throw new ArgumentNullException("array", "ConcurrentBag_CopyTo_ArgumentNullException");
			}
			bool lockTaken = false;
			try
			{
				FreezeBag(ref lockTaken);
				((ICollection)ToList()).CopyTo(array, index);
			}
			finally
			{
				UnfreezeBag(lockTaken);
			}
		}

		/// <summary>
		/// Copies the <see cref="T:System.Collections.Concurrent.ConcurrentBag`1" /> elements to a new array.
		/// </summary>
		/// <returns>A new array containing a snapshot of elements copied from the <see cref="T:System.Collections.Concurrent.ConcurrentBag`1" />.</returns>
		public T[] ToArray()
		{
			if (m_headList == null)
			{
				return new T[0];
			}
			bool lockTaken = false;
			try
			{
				FreezeBag(ref lockTaken);
				return ToList().ToArray();
			}
			finally
			{
				UnfreezeBag(lockTaken);
			}
		}

		/// <summary>
		/// Returns an enumerator that iterates through the <see cref="T:System.Collections.Concurrent.ConcurrentBag`1" />.
		/// </summary>
		/// <returns>An enumerator for the contents of the <see cref="T:System.Collections.Concurrent.ConcurrentBag`1" />.</returns>
		/// <remarks>
		/// The enumeration represents a moment-in-time snapshot of the contents
		/// of the bag.  It does not reflect any updates to the collection after 
		/// <see cref="M:System.Collections.Concurrent.ConcurrentBag`1.GetEnumerator" /> was called.  The enumerator is safe to use
		/// concurrently with reads from and writes to the bag.
		/// </remarks>
		public IEnumerator<T> GetEnumerator()
		{
			T[] array = ToArray();
			return ((IEnumerable<T>)array).GetEnumerator();
		}

		/// <summary>
		/// Returns an enumerator that iterates through the <see cref="T:System.Collections.Concurrent.ConcurrentBag`1" />.
		/// </summary>
		/// <returns>An enumerator for the contents of the <see cref="T:System.Collections.Concurrent.ConcurrentBag`1" />.</returns>
		/// <remarks>
		/// The items enumerated represent a moment-in-time snapshot of the contents
		/// of the bag.  It does not reflect any update to the collection after 
		/// <see cref="M:System.Collections.Concurrent.ConcurrentBag`1.GetEnumerator" /> was called.
		/// </remarks>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
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
			m_locals = new ThreadLocal<ThreadLocalList>();
			m_globalListsLock = new object();
			ThreadLocalList threadList = GetThreadList(forceCreate: true);
			T[] serializationArray = m_serializationArray;
			foreach (T item in serializationArray)
			{
				AddInternal(threadList, item);
			}
			m_headList = threadList;
			m_tailList = threadList;
			m_serializationArray = null;
		}

		/// <summary>
		/// Local helper method to freeze all bag operations, it
		/// 1- Acquire the global lock to prevent any other thread to freeze the bag, and also new new thread can be added
		/// to the dictionary
		/// 2- Then Acquire all local lists locks to prevent steal and synchronized operations
		/// 3- Wait for all un-synchronized operations to be done
		/// </summary>
		/// <param name="lockTaken">Retrieve the lock taken result for the global lock, to be passed to Unfreeze method</param>
		private void FreezeBag(ref bool lockTaken)
		{
			Monitor2.Enter(m_globalListsLock, ref lockTaken);
			m_needSync = true;
			AcquireAllLocks();
			WaitAllOperations();
		}

		/// <summary>
		/// Local helper method to unfreeze the bag from a frozen state
		/// </summary>
		/// <param name="lockTaken">The lock taken result from the Freeze method</param>
		private void UnfreezeBag(bool lockTaken)
		{
			ReleaseAllLocks();
			m_needSync = false;
			if (lockTaken)
			{
				Monitor.Exit(m_globalListsLock);
			}
		}

		/// <summary>
		/// local helper method to acquire all local lists locks
		/// </summary>
		private void AcquireAllLocks()
		{
			bool taken = false;
			for (ThreadLocalList threadLocalList = m_headList; threadLocalList != null; threadLocalList = threadLocalList.m_nextList)
			{
				try
				{
					Monitor2.Enter(threadLocalList, ref taken);
				}
				finally
				{
					if (taken)
					{
						threadLocalList.m_lockTaken = true;
						taken = false;
					}
				}
			}
		}

		/// <summary>
		/// Local helper method to release all local lists locks
		/// </summary>
		private void ReleaseAllLocks()
		{
			for (ThreadLocalList threadLocalList = m_headList; threadLocalList != null; threadLocalList = threadLocalList.m_nextList)
			{
				if (threadLocalList.m_lockTaken)
				{
					threadLocalList.m_lockTaken = false;
					Monitor.Exit(threadLocalList);
				}
			}
		}

		/// <summary>
		/// Local helper function to wait all unsynchronized operations
		/// </summary>
		private void WaitAllOperations()
		{
			for (ThreadLocalList threadLocalList = m_headList; threadLocalList != null; threadLocalList = threadLocalList.m_nextList)
			{
				if (threadLocalList.m_currentOp != 0)
				{
					SpinWait spinWait = default(SpinWait);
					while (threadLocalList.m_currentOp != 0)
					{
						spinWait.SpinOnce();
					}
				}
			}
		}

		/// <summary>
		/// Local helper function to get the bag count, the caller should call it from Freeze/Unfreeze block
		/// </summary>
		/// <returns>The current bag count</returns>
		private int GetCountInternal()
		{
			int num = 0;
			for (ThreadLocalList threadLocalList = m_headList; threadLocalList != null; threadLocalList = threadLocalList.m_nextList)
			{
				num = checked(num + threadLocalList.Count);
			}
			return num;
		}

		/// <summary>
		/// Local helper function to return the bag item in a list, this is mainly used by CopyTo and ToArray
		/// This is not thread safe, should be called in Freeze/UnFreeze bag block
		/// </summary>
		/// <returns>List the contains the bag items</returns>
		private List<T> ToList()
		{
			List<T> list = new List<T>();
			for (ThreadLocalList threadLocalList = m_headList; threadLocalList != null; threadLocalList = threadLocalList.m_nextList)
			{
				for (Node node = threadLocalList.m_head; node != null; node = node.m_next)
				{
					list.Add(node.m_value);
				}
			}
			return list;
		}
	}
}
