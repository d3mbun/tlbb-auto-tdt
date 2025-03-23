using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;

namespace System.Collections.Concurrent
{
	/// <summary> 
	/// Provides blocking and bounding capabilities for thread-safe collections that 
	/// implement <see cref="T:System.Collections.Concurrent.IProducerConsumerCollection{T}" />. 
	/// </summary>
	/// <remarks>
	/// <see cref="T:System.Collections.Concurrent.IProducerConsumerCollection{T}" /> represents a collection
	/// that allows for thread-safe adding and removing of data. 
	/// <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> is used as a wrapper
	/// for an <see cref="T:System.Collections.Concurrent.IProducerConsumerCollection{T}" /> instance, allowing
	/// removal attempts from the collection to block until data is available to be removed.  Similarly,
	/// a <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> can be created to enforce
	/// an upper-bound on the number of data elements allowed in the 
	/// <see cref="T:System.Collections.Concurrent.IProducerConsumerCollection{T}" />; addition attempts to the
	/// collection may then block until space is available to store the added items.  In this manner,
	/// <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> is similar to a traditional
	/// blocking queue data structure, except that the underlying data storage mechanism is abstracted
	/// away as an <see cref="T:System.Collections.Concurrent.IProducerConsumerCollection{T}" />. 
	/// </remarks>
	/// <typeparam name="T">Specifies the type of elements in the collection.</typeparam>
	[ComVisible(false)]
	[DebuggerTypeProxy(typeof(SystemThreadingCollections_BlockingCollectionDebugView<>))]
	[DebuggerDisplay("Count = {Count}, Type = {m_collection}")]
	[HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
	public class BlockingCollection<T> : IEnumerable<T>, ICollection, IEnumerable, IDisposable
	{
		/// <summary>An enumerated data type used internal to the class to specify to a generic method
		/// the current mode of operation.</summary>
		private enum OperationMode
		{
			Add,
			Take
		}

		private const int NON_BOUNDED = -1;

		private const int COMPLETE_ADDING_ON_MASK = int.MinValue;

		private IProducerConsumerCollection<T> m_collection;

		private int m_boundedCapacity;

		private SemaphoreSlim m_freeNodes;

		private SemaphoreSlim m_occupiedNodes;

		private volatile bool m_isDisposed;

		private CancellationTokenSource m_ConsumersCancellationTokenSource;

		private CancellationTokenSource m_ProducersCancellationTokenSource;

		private volatile int m_currentAdders;

		/// <summary>Gets the bounded capacity of this <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> instance.</summary>
		/// <value>The bounded capacity of this collection, or int.MaxValue if no bound was supplied.</value>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> has been disposed.</exception>
		public int BoundedCapacity
		{
			get
			{
				CheckDisposed();
				return m_boundedCapacity;
			}
		}

		/// <summary>Gets whether this <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> has been marked as complete for adding.</summary>
		/// <value>Whether this collection has been marked as complete for adding.</value>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> has been disposed.</exception>
		public bool IsAddingCompleted
		{
			get
			{
				CheckDisposed();
				return m_currentAdders == int.MinValue;
			}
		}

		/// <summary>Gets whether this <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> has been marked as complete for adding and is empty.</summary>
		/// <value>Whether this collection has been marked as complete for adding and is empty.</value>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> has been disposed.</exception>
		public bool IsCompleted
		{
			get
			{
				CheckDisposed();
				if (IsAddingCompleted)
				{
					return m_occupiedNodes.CurrentCount == 0;
				}
				return false;
			}
		}

		/// <summary>Gets the number of items contained in the <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" />.</summary>
		/// <value>The number of items contained in the <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" />.</value>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> has been disposed.</exception>
		public int Count
		{
			get
			{
				CheckDisposed();
				return m_occupiedNodes.CurrentCount;
			}
		}

		/// <summary>Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection" /> is synchronized.</summary>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> has been disposed.</exception>
		bool ICollection.IsSynchronized
		{
			get
			{
				CheckDisposed();
				return false;
			}
		}

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

		/// <summary>Initializes a new instance of the 
		/// <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" />
		/// class without an upper-bound.
		/// </summary>
		/// <remarks>
		/// The default underlying collection is a <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1">ConcurrentQueue&lt;T&gt;</see>.
		/// </remarks>
		public BlockingCollection()
			: this((IProducerConsumerCollection<T>)new ConcurrentQueue<T>())
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" />
		/// class with the specified upper-bound.
		/// </summary>
		/// <param name="boundedCapacity">The bounded size of the collection.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="boundedCapacity" /> is
		/// not a positive value.</exception>
		/// <remarks>
		/// The default underlying collection is a <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1">ConcurrentQueue&lt;T&gt;</see>.
		/// </remarks>
		public BlockingCollection(int boundedCapacity)
			: this((IProducerConsumerCollection<T>)new ConcurrentQueue<T>(), boundedCapacity)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" />
		/// class with the specified upper-bound and using the provided 
		/// <see cref="T:System.Collections.Concurrent.IProducerConsumerCollection{T}" /> as its underlying data store.</summary>
		/// <param name="collection">The collection to use as the underlying data store.</param>
		/// <param name="boundedCapacity">The bounded size of the collection.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="collection" /> argument is
		/// null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="boundedCapacity" /> is not a positive value.</exception>
		/// <exception cref="T:System.ArgumentException">The supplied <paramref name="collection" /> contains more values 
		/// than is permitted by <paramref name="boundedCapacity" />.</exception>
		public BlockingCollection(IProducerConsumerCollection<T> collection, int boundedCapacity)
		{
			if (boundedCapacity < 1)
			{
				throw new ArgumentOutOfRangeException("boundedCapacity", boundedCapacity, "BlockingCollection_ctor_BoundedCapacityRange");
			}
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			int count = collection.Count;
			if (count > boundedCapacity)
			{
				throw new ArgumentException("BlockingCollection_ctor_CountMoreThanCapacity");
			}
			Initialize(collection, boundedCapacity, count);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" />
		/// class without an upper-bound and using the provided 
		/// <see cref="T:System.Collections.Concurrent.IProducerConsumerCollection{T}" /> as its underlying data store.</summary>
		/// <param name="collection">The collection to use as the underlying data store.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="collection" /> argument is
		/// null.</exception>
		public BlockingCollection(IProducerConsumerCollection<T> collection)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			Initialize(collection, -1, collection.Count);
		}

		/// <summary>Initializes the BlockingCollection instance.</summary>
		/// <param name="collection">The collection to use as the underlying data store.</param>
		/// <param name="boundedCapacity">The bounded size of the collection.</param>
		/// <param name="collectionCount">The number of items currently in the underlying collection.</param>
		private void Initialize(IProducerConsumerCollection<T> collection, int boundedCapacity, int collectionCount)
		{
			m_collection = collection;
			m_boundedCapacity = boundedCapacity;
			m_isDisposed = false;
			m_ConsumersCancellationTokenSource = new CancellationTokenSource();
			m_ProducersCancellationTokenSource = new CancellationTokenSource();
			if (boundedCapacity == -1)
			{
				m_freeNodes = null;
			}
			else
			{
				m_freeNodes = new SemaphoreSlim(boundedCapacity - collectionCount);
			}
			m_occupiedNodes = new SemaphoreSlim(collectionCount);
		}

		/// <summary>
		/// Adds the item to the <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" />.
		/// </summary>
		/// <param name="item">The item to be added to the collection. The value can be a null reference.</param>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> has been marked
		/// as complete with regards to additions.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> has been disposed.</exception>
		/// <exception cref="T:System.InvalidOperationException">The underlying collection didn't accept the item.</exception>
		/// <remarks>
		/// If a bounded capacity was specified when this instance of 
		/// <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> was initialized, 
		/// a call to Add may block until space is available to store the provided item.
		/// </remarks>
		public void Add(T item)
		{
			TryAddWithNoTimeValidation(item, -1, default(CancellationToken));
		}

		/// <summary>
		/// Adds the item to the <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" />.
		/// A <see cref="T:System.OperationCanceledException" /> is thrown if the <see cref="T:System.Threading.CancellationToken" /> is
		/// canceled.
		/// </summary>
		/// <param name="item">The item to be added to the collection. The value can be a null reference.</param>
		/// <param name="cancellationToken">A cancellation token to observe.</param>
		/// <exception cref="T:System.OperationCanceledException">If the <see cref="T:System.Threading.CancellationToken" /> is canceled.</exception>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> has been marked
		/// as complete with regards to additions.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> has been disposed.</exception>
		/// <exception cref="T:System.InvalidOperationException">The underlying collection didn't accept the item.</exception>
		/// <remarks>
		/// If a bounded capacity was specified when this instance of 
		/// <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> was initialized, 
		/// a call to <see cref="M:System.Collections.Concurrent.BlockingCollection`1.Add(`0,System.Threading.CancellationToken)" /> may block until space is available to store the provided item.
		/// </remarks>
		public void Add(T item, CancellationToken cancellationToken)
		{
			TryAddWithNoTimeValidation(item, -1, cancellationToken);
		}

		/// <summary>
		/// Attempts to add the specified item to the <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" />.
		/// </summary>
		/// <param name="item">The item to be added to the collection.</param>
		/// <returns>true if the <paramref name="item" /> could be added; otherwise, false.</returns>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> has been marked
		/// as complete with regards to additions.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> has been disposed.</exception>
		/// <exception cref="T:System.InvalidOperationException">The underlying collection didn't accept the item.</exception>
		public bool TryAdd(T item)
		{
			return TryAddWithNoTimeValidation(item, 0, default(CancellationToken));
		}

		/// <summary>
		/// Attempts to add the specified item to the <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" />.
		/// </summary>
		/// <param name="item">The item to be added to the collection.</param>
		/// <param name="timeout">A <see cref="T:System.TimeSpan" /> that represents the number of milliseconds
		/// to wait, or a <see cref="T:System.TimeSpan" /> that represents -1 milliseconds to wait indefinitely.
		/// </param>
		/// <returns>true if the <paramref name="item" /> could be added to the collection within 
		/// the alloted time; otherwise, false.</returns>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> has been marked
		/// as complete with regards to additions.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> has been disposed.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="timeout" /> is a negative number
		/// other than -1 milliseconds, which represents an infinite time-out -or- timeout is greater than
		/// <see cref="F:System.Int32.MaxValue" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">The underlying collection didn't accept the item.</exception>
		public bool TryAdd(T item, TimeSpan timeout)
		{
			ValidateTimeout(timeout);
			return TryAddWithNoTimeValidation(item, (int)timeout.TotalMilliseconds, default(CancellationToken));
		}

		/// <summary>
		/// Attempts to add the specified item to the <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" />.
		/// </summary>
		/// <param name="item">The item to be added to the collection.</param>
		/// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="F:System.Threading.Timeout.Infinite" /> (-1) to wait indefinitely.</param>
		/// <returns>true if the <paramref name="item" /> could be added to the collection within 
		/// the alloted time; otherwise, false.</returns>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> has been marked
		/// as complete with regards to additions.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> has been disposed.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="millisecondsTimeout" /> is a
		/// negative number other than -1, which represents an infinite time-out.</exception>
		/// <exception cref="T:System.InvalidOperationException">The underlying collection didn't accept the item.</exception>
		public bool TryAdd(T item, int millisecondsTimeout)
		{
			ValidateMillisecondsTimeout(millisecondsTimeout);
			return TryAddWithNoTimeValidation(item, millisecondsTimeout, default(CancellationToken));
		}

		/// <summary>
		/// Attempts to add the specified item to the <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" />.
		/// A <see cref="T:System.OperationCanceledException" /> is thrown if the <see cref="T:System.Threading.CancellationToken" /> is
		/// canceled.
		/// </summary>
		/// <param name="item">The item to be added to the collection.</param>
		/// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="F:System.Threading.Timeout.Infinite" /> (-1) to wait indefinitely.</param>
		/// <param name="cancellationToken">A cancellation token to observe.</param>
		/// <returns>true if the <paramref name="item" /> could be added to the collection within 
		/// the alloted time; otherwise, false.</returns>
		/// <exception cref="T:System.OperationCanceledException">If the <see cref="T:System.Threading.CancellationToken" /> is canceled.</exception>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> has been marked
		/// as complete with regards to additions.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> has been disposed.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="millisecondsTimeout" /> is a
		/// negative number other than -1, which represents an infinite time-out.</exception>
		/// <exception cref="T:System.InvalidOperationException">The underlying collection didn't accept the item.</exception>
		public bool TryAdd(T item, int millisecondsTimeout, CancellationToken cancellationToken)
		{
			ValidateMillisecondsTimeout(millisecondsTimeout);
			return TryAddWithNoTimeValidation(item, millisecondsTimeout, cancellationToken);
		}

		/// <summary>Adds an item into the underlying data store using its IProducerConsumerCollection&lt;T&gt;.Add 
		/// method. If a bounded capacity was specified and the collection was full, 
		/// this method will wait for, at most, the timeout period trying to add the item. 
		/// If the timeout period was exhaused before successfully adding the item this method will 
		/// return false.</summary>
		/// <param name="item">The item to be added to the collection.</param>
		/// <param name="millisecondsTimeout">The number of milliseconds to wait for the collection to accept the item,
		/// or Timeout.Infinite to wait indefinitely.</param>
		/// <param name="cancellationToken">A cancellation token to observe.</param>
		/// <returns>False if the collection remained full till the timeout period was exhausted.True otherwise.</returns>
		/// <exception cref="T:System.OperationCanceledException">If the <see cref="T:System.Threading.CancellationToken" /> is canceled.</exception>
		/// <exception cref="T:System.InvalidOperationException">the collection has already been marked
		/// as complete with regards to additions.</exception>
		/// <exception cref="T:System.ObjectDisposedException">If the collection has been disposed.</exception>
		/// <exception cref="T:System.InvalidOperationException">The underlying collection didn't accept the item.</exception>
		private bool TryAddWithNoTimeValidation(T item, int millisecondsTimeout, CancellationToken cancellationToken)
		{
			CheckDisposed();
			if (cancellationToken.IsCancellationRequested)
			{
				throw new OperationCanceledException2("Common_OperationCanceled", cancellationToken);
			}
			if (IsAddingCompleted)
			{
				throw new InvalidOperationException("BlockingCollection_Completed");
			}
			bool flag = true;
			if (m_freeNodes != null)
			{
				CancellationTokenSource cancellationTokenSource = null;
				try
				{
					flag = m_freeNodes.Wait(0);
					if (!flag && millisecondsTimeout != 0)
					{
						cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, m_ProducersCancellationTokenSource.Token);
						flag = m_freeNodes.Wait(millisecondsTimeout, cancellationTokenSource.Token);
					}
				}
				catch (OperationCanceledException)
				{
					if (cancellationToken.IsCancellationRequested)
					{
						throw new OperationCanceledException2("Common_OperationCanceled", cancellationToken);
					}
					throw new InvalidOperationException("BlockingCollection_Add_ConcurrentCompleteAdd");
				}
				finally
				{
					cancellationTokenSource?.Dispose();
				}
			}
			if (flag)
			{
				SpinWait spinWait = default(SpinWait);
				while (true)
				{
					int currentAdders = m_currentAdders;
					if (((uint)currentAdders & 0x80000000u) != 0)
					{
						spinWait.Reset();
						while (m_currentAdders != int.MinValue)
						{
							spinWait.SpinOnce();
						}
						throw new InvalidOperationException("BlockingCollection_Completed");
					}
					if (Interlocked.CompareExchange(ref m_currentAdders, currentAdders + 1, currentAdders) == currentAdders)
					{
						break;
					}
					spinWait.SpinOnce();
				}
				try
				{
					bool flag2 = false;
					try
					{
						cancellationToken.ThrowIfCancellationRequested();
						flag2 = m_collection.TryAdd(item);
					}
					catch
					{
						if (m_freeNodes != null)
						{
							m_freeNodes.Release();
						}
						throw;
					}
					if (flag2)
					{
						m_occupiedNodes.Release();
						return flag;
					}
					throw new InvalidOperationException("BlockingCollection_Add_Failed");
				}
				finally
				{
					Interlocked.Decrement(ref m_currentAdders);
				}
			}
			return flag;
		}

		/// <summary>Takes an item from the <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" />.</summary>
		/// <returns>The item removed from the collection.</returns>
		/// <exception cref="T:System.OperationCanceledException">The <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> is empty and has been marked
		/// as complete with regards to additions.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> has been disposed.</exception>
		/// <exception cref="T:System.InvalidOperationException">The underlying collection was modified
		/// outside of this <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> instance.</exception>
		/// <remarks>A call to <see cref="M:System.Collections.Concurrent.BlockingCollection`1.Take" /> may block until an item is available to be removed.</remarks>
		public T Take()
		{
			if (!TryTake(out var item, -1, CancellationToken.None))
			{
				throw new InvalidOperationException("BlockingCollection_CantTakeWhenDone");
			}
			return item;
		}

		/// <summary>Takes an item from the <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" />.</summary>
		/// <returns>The item removed from the collection.</returns>
		/// <exception cref="T:System.OperationCanceledException">If the <see cref="T:System.Threading.CancellationToken" /> is
		/// canceled or the <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> is empty and has been marked
		/// as complete with regards to additions.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> has been disposed.</exception>
		/// <exception cref="T:System.InvalidOperationException">The underlying collection was modified
		/// outside of this <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> instance.</exception>
		/// <remarks>A call to <see cref="M:System.Collections.Concurrent.BlockingCollection`1.Take(System.Threading.CancellationToken)" /> may block until an item is available to be removed.</remarks>
		public T Take(CancellationToken cancellationToken)
		{
			if (!TryTake(out var item, -1, cancellationToken))
			{
				throw new InvalidOperationException("BlockingCollection_CantTakeWhenDone");
			}
			return item;
		}

		/// <summary>
		/// Attempts to remove an item from the <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" />.
		/// </summary>
		/// <param name="item">The item removed from the collection.</param>
		/// <returns>true if an item could be removed; otherwise, false.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> has been disposed.</exception>
		/// <exception cref="T:System.InvalidOperationException">The underlying collection was modified
		/// outside of this <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> instance.</exception>
		public bool TryTake(out T item)
		{
			return TryTake(out item, 0, CancellationToken.None);
		}

		/// <summary>
		/// Attempts to remove an item from the <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" />.
		/// </summary>
		/// <param name="item">The item removed from the collection.</param>
		/// <param name="timeout">A <see cref="T:System.TimeSpan" /> that represents the number of milliseconds
		/// to wait, or a <see cref="T:System.TimeSpan" /> that represents -1 milliseconds to wait indefinitely.
		/// </param>
		/// <returns>true if an item could be removed from the collection within 
		/// the alloted time; otherwise, false.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> has been disposed.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="timeout" /> is a negative number
		/// other than -1 milliseconds, which represents an infinite time-out -or- timeout is greater than
		/// <see cref="F:System.Int32.MaxValue" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">The underlying collection was modified
		/// outside of this <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> instance.</exception>
		public bool TryTake(out T item, TimeSpan timeout)
		{
			ValidateTimeout(timeout);
			return TryTakeWithNoTimeValidation(out item, (int)timeout.TotalMilliseconds, CancellationToken.None, null);
		}

		/// <summary>
		/// Attempts to remove an item from the <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" />.
		/// </summary>
		/// <param name="item">The item removed from the collection.</param>
		/// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="F:System.Threading.Timeout.Infinite" /> (-1) to wait indefinitely.</param>
		/// <returns>true if an item could be removed from the collection within 
		/// the alloted time; otherwise, false.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> has been disposed.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="millisecondsTimeout" /> is a
		/// negative number other than -1, which represents an infinite time-out.</exception>
		/// <exception cref="T:System.InvalidOperationException">The underlying collection was modified
		/// outside of this <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> instance.</exception>
		public bool TryTake(out T item, int millisecondsTimeout)
		{
			ValidateMillisecondsTimeout(millisecondsTimeout);
			return TryTakeWithNoTimeValidation(out item, millisecondsTimeout, CancellationToken.None, null);
		}

		/// <summary>
		/// Attempts to remove an item from the <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" />.
		/// A <see cref="T:System.OperationCanceledException" /> is thrown if the <see cref="T:System.Threading.CancellationToken" /> is
		/// canceled.
		/// </summary>
		/// <param name="item">The item removed from the collection.</param>
		/// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="F:System.Threading.Timeout.Infinite" /> (-1) to wait indefinitely.</param>
		/// <param name="cancellationToken">A cancellation token to observe.</param>
		/// <returns>true if an item could be removed from the collection within 
		/// the alloted time; otherwise, false.</returns>
		/// <exception cref="T:System.OperationCanceledException">If the <see cref="T:System.Threading.CancellationToken" /> is canceled.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> has been disposed.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="millisecondsTimeout" /> is a
		/// negative number other than -1, which represents an infinite time-out.</exception>
		/// <exception cref="T:System.InvalidOperationException">The underlying collection was modified
		/// outside of this <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> instance.</exception>
		public bool TryTake(out T item, int millisecondsTimeout, CancellationToken cancellationToken)
		{
			ValidateMillisecondsTimeout(millisecondsTimeout);
			return TryTakeWithNoTimeValidation(out item, millisecondsTimeout, cancellationToken, null);
		}

		/// <summary>Takes an item from the underlying data store using its IProducerConsumerCollection&lt;T&gt;.Take 
		/// method. If the collection was empty, this method will wait for, at most, the timeout period (if AddingIsCompleted is false)
		/// trying to remove an item. If the timeout period was exhaused before successfully removing an item 
		/// this method will return false.
		/// A <see cref="T:System.OperationCanceledException" /> is thrown if the <see cref="T:System.Threading.CancellationToken" /> is
		/// canceled.
		/// </summary>
		/// <param name="item">The item removed from the collection.</param>
		/// <param name="millisecondsTimeout">The number of milliseconds to wait for the collection to have an item available 
		/// for removal, or Timeout.Infinite to wait indefinitely.</param>
		/// <param name="cancellationToken">A cancellation token to observe.</param>
		/// <param name="combinedTokenSource">A combined cancellation token if created, it is only created by GetConsumingEnumerable to avoid creating the linked token 
		/// multiple times.</param>
		/// <returns>False if the collection remained empty till the timeout period was exhausted. True otherwise.</returns>
		/// <exception cref="T:System.OperationCanceledException">If the <see cref="T:System.Threading.CancellationToken" /> is canceled.</exception>
		/// <exception cref="T:System.ObjectDisposedException">If the collection has been disposed.</exception>
		private bool TryTakeWithNoTimeValidation(out T item, int millisecondsTimeout, CancellationToken cancellationToken, CancellationTokenSource combinedTokenSource)
		{
			CheckDisposed();
			item = default(T);
			if (cancellationToken.IsCancellationRequested)
			{
				throw new OperationCanceledException2("Common_OperationCanceled", cancellationToken);
			}
			if (IsCompleted)
			{
				return false;
			}
			bool flag = false;
			CancellationTokenSource cancellationTokenSource = combinedTokenSource;
			try
			{
				flag = m_occupiedNodes.Wait(0);
				if (!flag && millisecondsTimeout != 0)
				{
					if (combinedTokenSource == null)
					{
						cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, m_ConsumersCancellationTokenSource.Token);
					}
					flag = m_occupiedNodes.Wait(millisecondsTimeout, cancellationTokenSource.Token);
				}
			}
			catch (OperationCanceledException)
			{
				if (cancellationToken.IsCancellationRequested)
				{
					throw new OperationCanceledException2("Common_OperationCanceled", cancellationToken);
				}
				return false;
			}
			finally
			{
				if (cancellationTokenSource != null && combinedTokenSource == null)
				{
					cancellationTokenSource.Dispose();
				}
			}
			if (flag)
			{
				bool flag2 = false;
				bool flag3 = true;
				try
				{
					cancellationToken.ThrowIfCancellationRequested();
					flag2 = m_collection.TryTake(out item);
					flag3 = false;
					if (!flag2)
					{
						throw new InvalidOperationException("BlockingCollection_Take_CollectionModified");
					}
					return flag;
				}
				finally
				{
					if (flag2)
					{
						if (m_freeNodes != null)
						{
							m_freeNodes.Release();
						}
					}
					else if (flag3)
					{
						m_occupiedNodes.Release();
					}
					if (IsCompleted)
					{
						CancelWaitingConsumers();
					}
				}
			}
			return flag;
		}

		/// <summary>
		/// Adds the specified item to any one of the specified
		/// <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> instances.
		/// </summary>
		/// <param name="collections">The array of collections.</param>
		/// <param name="item">The item to be added to one of the collections.</param>
		/// <returns>The index of the collection in the <paramref name="collections" /> array to which the item was added.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="collections" /> argument is
		/// null.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="collections" /> argument is
		/// a 0-length array or contains a null element, or at least one of collections has been
		/// marked as complete for adding.</exception>
		/// <exception cref="T:System.ObjectDisposedException">At least one of the <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> instances has been disposed.</exception>
		/// <exception cref="T:System.InvalidOperationException">At least one underlying collection didn't accept the item.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The count of <paramref name="collections" /> is greater than the maximum size of
		/// 62 for STA and 63 for MTA.</exception>
		/// <remarks>
		/// If a bounded capacity was specified when all of the
		/// <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> instances were initialized, 
		/// a call to AddToAny may block until space is available in one of the collections
		/// to store the provided item.
		/// </remarks>
		public static int AddToAny(BlockingCollection<T>[] collections, T item)
		{
			return TryAddToAny(collections, item, -1, default(CancellationToken));
		}

		/// <summary>
		/// Adds the specified item to any one of the specified
		/// <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> instances.
		/// A <see cref="T:System.OperationCanceledException" /> is thrown if the <see cref="T:System.Threading.CancellationToken" /> is
		/// canceled. 
		/// </summary>
		/// <param name="collections">The array of collections.</param>
		/// <param name="item">The item to be added to one of the collections.</param>
		/// <param name="cancellationToken">A cancellation token to observe.</param>
		/// <returns>The index of the collection in the <paramref name="collections" /> array to which the item was added.</returns>
		/// <exception cref="T:System.OperationCanceledException">If the <see cref="T:System.Threading.CancellationToken" /> is canceled.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="collections" /> argument is
		/// null.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="collections" /> argument is
		/// a 0-length array or contains a null element, or at least one of collections has been
		/// marked as complete for adding.</exception>
		/// <exception cref="T:System.ObjectDisposedException">At least one of the <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> instances has been disposed.</exception>
		/// <exception cref="T:System.InvalidOperationException">At least one underlying collection didn't accept the item.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The count of <paramref name="collections" /> is greater than the maximum size of
		/// 62 for STA and 63 for MTA.</exception>
		/// <remarks>
		/// If a bounded capacity was specified when all of the
		/// <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> instances were initialized, 
		/// a call to AddToAny may block until space is available in one of the collections
		/// to store the provided item.
		/// </remarks>
		public static int AddToAny(BlockingCollection<T>[] collections, T item, CancellationToken cancellationToken)
		{
			return TryAddToAny(collections, item, -1, cancellationToken);
		}

		/// <summary>
		/// Attempts to add the specified item to any one of the specified
		/// <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> instances.
		/// </summary>
		/// <param name="collections">The array of collections.</param>
		/// <param name="item">The item to be added to one of the collections.</param>
		/// <returns>The index of the collection in the <paramref name="collections" /> 
		/// array to which the item was added, or -1 if the item could not be added.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="collections" /> argument is
		/// null.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="collections" /> argument is
		/// a 0-length array or contains a null element, or at least one of collections has been
		/// marked as complete for adding.</exception>
		/// <exception cref="T:System.ObjectDisposedException">At least one of the <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> instances has been disposed.</exception>
		/// <exception cref="T:System.InvalidOperationException">At least one underlying collection didn't accept the item.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The count of <paramref name="collections" /> is greater than the maximum size of
		/// 62 for STA and 63 for MTA.</exception>
		public static int TryAddToAny(BlockingCollection<T>[] collections, T item)
		{
			return TryAddToAny(collections, item, 0, default(CancellationToken));
		}

		/// <summary>
		/// Attempts to add the specified item to any one of the specified
		/// <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> instances.
		/// </summary>
		/// <param name="collections">The array of collections.</param>
		/// <param name="item">The item to be added to one of the collections.</param>
		/// <param name="timeout">A <see cref="T:System.TimeSpan" /> that represents the number of milliseconds
		/// to wait, or a <see cref="T:System.TimeSpan" /> that represents -1 milliseconds to wait indefinitely.
		/// </param>
		/// <returns>The index of the collection in the <paramref name="collections" /> 
		/// array to which the item was added, or -1 if the item could not be added.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="collections" /> argument is
		/// null.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="collections" /> argument is
		/// a 0-length array or contains a null element, or at least one of collections has been
		/// marked as complete for adding.</exception>
		/// <exception cref="T:System.ObjectDisposedException">At least one of the <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> instances has been disposed.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="timeout" /> is a negative number
		/// other than -1 milliseconds, which represents an infinite time-out -or- timeout is greater than
		/// <see cref="F:System.Int32.MaxValue" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">At least one underlying collection didn't accept the item.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The count of <paramref name="collections" /> is greater than the maximum size of
		/// 62 for STA and 63 for MTA.</exception>
		public static int TryAddToAny(BlockingCollection<T>[] collections, T item, TimeSpan timeout)
		{
			ValidateTimeout(timeout);
			return TryAddTakeAny(collections, ref item, (int)timeout.TotalMilliseconds, OperationMode.Add, default(CancellationToken));
		}

		/// <summary>
		/// Attempts to add the specified item to any one of the specified
		/// <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> instances.
		/// </summary>
		/// <param name="collections">The array of collections.</param>
		/// <param name="item">The item to be added to one of the collections.</param>
		/// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="F:System.Threading.Timeout.Infinite" /> (-1) to wait indefinitely.</param>        /// <returns>The index of the collection in the <paramref name="collections" /> 
		/// array to which the item was added, or -1 if the item could not be added.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="collections" /> argument is
		/// null.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="collections" /> argument is
		/// a 0-length array or contains a null element, or at least one of collections has been
		/// marked as complete for adding.</exception>
		/// <exception cref="T:System.ObjectDisposedException">At least one of the <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> instances has been disposed.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="millisecondsTimeout" /> is a
		/// negative number other than -1, which represents an infinite time-out.</exception>
		/// <exception cref="T:System.InvalidOperationException">At least one underlying collection didn't accept the item.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The count of <paramref name="collections" /> is greater than the maximum size of
		/// 62 for STA and 63 for MTA.</exception>
		public static int TryAddToAny(BlockingCollection<T>[] collections, T item, int millisecondsTimeout)
		{
			ValidateMillisecondsTimeout(millisecondsTimeout);
			return TryAddTakeAny(collections, ref item, millisecondsTimeout, OperationMode.Add, default(CancellationToken));
		}

		/// <summary>
		/// Attempts to add the specified item to any one of the specified
		/// <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> instances.
		/// A <see cref="T:System.OperationCanceledException" /> is thrown if the <see cref="T:System.Threading.CancellationToken" /> is
		/// canceled.
		/// </summary>
		/// <param name="collections">The array of collections.</param>
		/// <param name="item">The item to be added to one of the collections.</param>
		/// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="F:System.Threading.Timeout.Infinite" /> (-1) to wait indefinitely.</param>        /// <returns>The index of the collection in the <paramref name="collections" /> 
		/// array to which the item was added, or -1 if the item could not be added.</returns>
		/// <param name="cancellationToken">A cancellation token to observe.</param>
		/// <exception cref="T:System.OperationCanceledException">If the <see cref="T:System.Threading.CancellationToken" /> is canceled.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="collections" /> argument is
		/// null.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="collections" /> argument is
		/// a 0-length array or contains a null element, or at least one of collections has been
		/// marked as complete for adding.</exception>
		/// <exception cref="T:System.ObjectDisposedException">At least one of the <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> instances has been disposed.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="millisecondsTimeout" /> is a
		/// negative number other than -1, which represents an infinite time-out.</exception>
		/// <exception cref="T:System.InvalidOperationException">At least one underlying collection didn't accept the item.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The count of <paramref name="collections" /> is greater than the maximum size of
		/// 62 for STA and 63 for MTA.</exception>
		public static int TryAddToAny(BlockingCollection<T>[] collections, T item, int millisecondsTimeout, CancellationToken cancellationToken)
		{
			ValidateMillisecondsTimeout(millisecondsTimeout);
			return TryAddTakeAny(collections, ref item, millisecondsTimeout, OperationMode.Add, cancellationToken);
		}

		/// <summary>Adds/Takes an item to/from anyone of the specified collections.
		/// A <see cref="T:System.OperationCanceledException" /> is thrown if the <see cref="T:System.Threading.CancellationToken" /> is
		/// canceled. 
		/// </summary>
		/// <param name="collections">The collections into which the item can be added.</param>
		/// <param name="item">The item to be added or the item removed and returned to the caller.</param>
		/// <param name="millisecondsTimeout">The number of milliseconds to wait for a collection to accept the 
		/// operation, or -1 to wait indefinitely.</param>
		/// <param name="operationMode">Indicates whether this method is called to Add or Take.</param>
		/// <param name="externalCancellationToken">A cancellation token to observe.</param>
		/// <returns>The index into collections for the collection which accepted the 
		/// adding/removal of the item; -1 if the item could not be added/removed.</returns>
		/// <exception cref="T:System.OperationCanceledException">If the <see cref="T:System.Threading.CancellationToken" /> is canceled.</exception>
		/// <exception cref="T:System.ArgumentNullException">If the collections argument is null.</exception>
		/// <exception cref="T:System.ArgumentException">If the collections argument is a 0-length array or contains a 
		/// null element. Also, if atleast one of the collections has been marked complete for adds.</exception>
		/// <exception cref="T:System.ObjectDisposedException">If atleast one of the collections has been disposed.</exception>
		private static int TryAddTakeAny(BlockingCollection<T>[] collections, ref T item, int millisecondsTimeout, OperationMode operationMode, CancellationToken externalCancellationToken)
		{
			BlockingCollection<T>[] array = ValidateCollectionsArray(collections, operationMode);
			int num = millisecondsTimeout;
			long startTimeTicks = 0L;
			if (millisecondsTimeout != -1)
			{
				startTimeTicks = DateTime.UtcNow.Ticks;
			}
			if (operationMode == OperationMode.Add)
			{
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i].m_freeNodes == null)
					{
						array[i].TryAdd(item);
						return i;
					}
				}
			}
			CancellationToken[] cancellationTokens;
			List<WaitHandle> handles = GetHandles(array, operationMode, externalCancellationToken, excludeCompleted: false, out cancellationTokens);
			while (millisecondsTimeout == -1 || num >= 0)
			{
				int num2 = -1;
				CancellationTokenSource cancellationTokenSource = null;
				try
				{
					num2 = WaitHandle_WaitAny(handles, 0, externalCancellationToken, externalCancellationToken);
					if (num2 == 258)
					{
						cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationTokens);
						num2 = WaitHandle_WaitAny(handles, num, cancellationTokenSource.Token, externalCancellationToken);
					}
				}
				catch (OperationCanceledException)
				{
					if (externalCancellationToken.IsCancellationRequested)
					{
						throw;
					}
					if (operationMode == OperationMode.Take && millisecondsTimeout != -1)
					{
						handles = GetHandles(array, operationMode, externalCancellationToken, excludeCompleted: true, out cancellationTokens);
						num = UpdateTimeOut(startTimeTicks, millisecondsTimeout);
						if (handles.Count == 0 || num == 0)
						{
							return -1;
						}
						continue;
					}
					throw new ArgumentException("BlockingCollection_CantTakeAnyWhenDone", "collections");
				}
				finally
				{
					cancellationTokenSource?.Dispose();
				}
				if (num2 != 258)
				{
					switch (operationMode)
					{
					case OperationMode.Add:
						if (array[num2].TryAdd(item))
						{
							return num2;
						}
						break;
					case OperationMode.Take:
						if (array[num2].TryTake(out item))
						{
							return num2;
						}
						break;
					}
					if (millisecondsTimeout > 0)
					{
						num = UpdateTimeOut(startTimeTicks, millisecondsTimeout);
						if (num <= 0)
						{
							return -1;
						}
					}
					continue;
				}
				return -1;
			}
			return -1;
		}

		/// <summary>
		/// Local static method, used by TryAddTakeAny to get the wait handles for the collection, with exclude option to exclude the Compeleted collections
		/// </summary>
		/// <param name="collections">The blocking collections</param>
		/// <param name="operationMode">Add or Take operation</param>
		/// <param name="externalCancellationToken">The original CancellationToken</param>
		/// <param name="excludeCompleted">True to exclude the compeleted collections</param>
		/// <param name="cancellationTokens">Complete list of cancellationTokens to observe</param>
		/// <returns>The collections wait handles</returns>
		private static List<WaitHandle> GetHandles(BlockingCollection<T>[] collections, OperationMode operationMode, CancellationToken externalCancellationToken, bool excludeCompleted, out CancellationToken[] cancellationTokens)
		{
			List<WaitHandle> list = new List<WaitHandle>(collections.Length);
			List<CancellationToken> list2 = new List<CancellationToken>(collections.Length + 1);
			list2.Add(externalCancellationToken);
			if (operationMode == OperationMode.Add)
			{
				for (int i = 0; i < collections.Length; i++)
				{
					if (collections[i].m_freeNodes != null)
					{
						list.Add(collections[i].m_freeNodes.AvailableWaitHandle);
						list2.Add(collections[i].m_ProducersCancellationTokenSource.Token);
					}
				}
			}
			else
			{
				for (int j = 0; j < collections.Length; j++)
				{
					if (!excludeCompleted || !collections[j].IsCompleted)
					{
						list.Add(collections[j].m_occupiedNodes.AvailableWaitHandle);
						list2.Add(collections[j].m_ConsumersCancellationTokenSource.Token);
					}
				}
			}
			cancellationTokens = list2.ToArray();
			return list;
		}

		/// <summary>
		/// Helper to perform WaitHandle.WaitAny(.., CancellationToken)
		/// this should eventually appear on the WaitHandle class.
		/// </summary>
		/// <param name="handles"></param>
		/// <param name="millisecondsTimeout"></param>
		/// <param name="combinedToken"></param>
		/// <param name="externalToken"></param>
		/// <returns></returns>
		private static int WaitHandle_WaitAny(List<WaitHandle> handles, int millisecondsTimeout, CancellationToken combinedToken, CancellationToken externalToken)
		{
			WaitHandle[] array = new WaitHandle[handles.Count + 1];
			for (int i = 0; i < handles.Count; i++)
			{
				array[i] = handles[i];
			}
			array[handles.Count] = combinedToken.WaitHandle;
			int result = WaitHandle.WaitAny(array, millisecondsTimeout, exitContext: false);
			if (combinedToken.IsCancellationRequested)
			{
				if (externalToken.IsCancellationRequested)
				{
					throw new OperationCanceledException2("Common_OperationCanceled", externalToken);
				}
				throw new OperationCanceledException2("Common_OperationCanceled");
			}
			return result;
		}

		/// <summary>
		/// Helper function to measure and update the wait time
		/// </summary>
		/// <param name="startTimeTicks"> The first time (in Ticks) observed when the wait started</param>
		/// <param name="originalWaitMillisecondsTimeout">The orginal wait timeoutout in milliseconds</param>
		/// <returns>The new wait time in milliseconds, -1 if the time expired</returns>
		private static int UpdateTimeOut(long startTimeTicks, int originalWaitMillisecondsTimeout)
		{
			if (originalWaitMillisecondsTimeout == 0)
			{
				return 0;
			}
			long num = (DateTime.UtcNow.Ticks - startTimeTicks) / 10000;
			if (num > int.MaxValue)
			{
				return 0;
			}
			int num2 = originalWaitMillisecondsTimeout - (int)num;
			if (num2 <= 0)
			{
				return 0;
			}
			return num2;
		}

		/// <summary>
		/// Takes an item from any one of the specified
		/// <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> instances.
		/// </summary>
		/// <param name="collections">The array of collections.</param>
		/// <param name="item">The item removed from one of the collections.</param>
		/// <returns>The index of the collection in the <paramref name="collections" /> array from which 
		/// the item was removed, or -1 if an item could not be removed.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="collections" /> argument is
		/// null.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="collections" /> argument is
		/// a 0-length array or contains a null element.</exception>
		/// <exception cref="T:System.ObjectDisposedException">At least one of the <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> instances has been disposed.</exception>
		/// <exception cref="T:System.InvalidOperationException">At least one of the underlying collections was modified
		/// outside of its <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> instance.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The count of <paramref name="collections" /> is greater than the maximum size of
		/// 62 for STA and 63 for MTA.</exception>
		/// <remarks>A call to TakeFromAny may block until an item is available to be removed.</remarks>
		public static int TakeFromAny(BlockingCollection<T>[] collections, out T item)
		{
			return TryTakeFromAny(collections, out item, -1);
		}

		/// <summary>
		/// Takes an item from any one of the specified
		/// <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> instances.
		/// A <see cref="T:System.OperationCanceledException" /> is thrown if the <see cref="T:System.Threading.CancellationToken" /> is
		/// canceled.
		/// </summary>
		/// <param name="collections">The array of collections.</param>
		/// <param name="item">The item removed from one of the collections.</param>
		/// <param name="cancellationToken">A cancellation token to observe.</param>
		/// <returns>The index of the collection in the <paramref name="collections" /> array from which 
		/// the item was removed, or -1 if an item could not be removed.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="collections" /> argument is
		/// null.</exception>
		/// <exception cref="T:System.OperationCanceledException">If the <see cref="T:System.Threading.CancellationToken" /> is canceled.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="collections" /> argument is
		/// a 0-length array or contains a null element.</exception>
		/// <exception cref="T:System.ObjectDisposedException">At least one of the <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> instances has been disposed.</exception>
		/// <exception cref="T:System.InvalidOperationException">At least one of the underlying collections was modified
		/// outside of its <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> instance.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The count of <paramref name="collections" /> is greater than the maximum size of 
		/// 62 for STA and 63 for MTA.</exception>
		/// <remarks>A call to TakeFromAny may block until an item is available to be removed.</remarks>
		public static int TakeFromAny(BlockingCollection<T>[] collections, out T item, CancellationToken cancellationToken)
		{
			return TryTakeFromAny(collections, out item, -1, cancellationToken);
		}

		/// <summary>
		/// Attempts to remove an item from any one of the specified
		/// <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> instances.
		/// </summary>
		/// <param name="collections">The array of collections.</param>
		/// <param name="item">The item removed from one of the collections.</param>
		/// <returns>The index of the collection in the <paramref name="collections" /> array from which 
		/// the item was removed, or -1 if an item could not be removed.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="collections" /> argument is
		/// null.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="collections" /> argument is
		/// a 0-length array or contains a null element.</exception>
		/// <exception cref="T:System.ObjectDisposedException">At least one of the <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> instances has been disposed.</exception>
		/// <exception cref="T:System.InvalidOperationException">At least one of the underlying collections was modified
		/// outside of its <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> instance.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The count of <paramref name="collections" /> is greater than the maximum size of
		/// 62 for STA and 63 for MTA.</exception>
		/// <remarks>A call to TryTakeFromAny may block until an item is available to be removed.</remarks>
		public static int TryTakeFromAny(BlockingCollection<T>[] collections, out T item)
		{
			return TryTakeFromAny(collections, out item, 0);
		}

		/// <summary>
		/// Attempts to remove an item from any one of the specified
		/// <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> instances.
		/// </summary>
		/// <param name="collections">The array of collections.</param>
		/// <param name="item">The item removed from one of the collections.</param>
		/// <param name="timeout">A <see cref="T:System.TimeSpan" /> that represents the number of milliseconds
		/// to wait, or a <see cref="T:System.TimeSpan" /> that represents -1 milliseconds to wait indefinitely.
		/// </param>
		/// <returns>The index of the collection in the <paramref name="collections" /> array from which 
		/// the item was removed, or -1 if an item could not be removed.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="collections" /> argument is
		/// null.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="collections" /> argument is
		/// a 0-length array or contains a null element.</exception>
		/// <exception cref="T:System.ObjectDisposedException">At least one of the <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> instances has been disposed.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="timeout" /> is a negative number
		/// other than -1 milliseconds, which represents an infinite time-out -or- timeout is greater than
		/// <see cref="F:System.Int32.MaxValue" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">At least one of the underlying collections was modified
		/// outside of its <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> instance.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The count of <paramref name="collections" /> is greater than the maximum size of
		/// 62 for STA and 63 for MTA.</exception>
		/// <remarks>A call to TryTakeFromAny may block until an item is available to be removed.</remarks>
		public static int TryTakeFromAny(BlockingCollection<T>[] collections, out T item, TimeSpan timeout)
		{
			ValidateTimeout(timeout);
			T item2 = default(T);
			int result = TryAddTakeAny(collections, ref item2, (int)timeout.TotalMilliseconds, OperationMode.Take, default(CancellationToken));
			item = item2;
			return result;
		}

		/// <summary>
		/// Attempts to remove an item from any one of the specified
		/// <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> instances.
		/// </summary>
		/// <param name="collections">The array of collections.</param>
		/// <param name="item">The item removed from one of the collections.</param>
		/// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="F:System.Threading.Timeout.Infinite" /> (-1) to wait indefinitely.</param>
		/// <returns>The index of the collection in the <paramref name="collections" /> array from which 
		/// the item was removed, or -1 if an item could not be removed.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="collections" /> argument is
		/// null.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="collections" /> argument is
		/// a 0-length array or contains a null element.</exception>
		/// <exception cref="T:System.ObjectDisposedException">At least one of the <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> instances has been disposed.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="millisecondsTimeout" /> is a
		/// negative number other than -1, which represents an infinite time-out.</exception>
		/// <exception cref="T:System.InvalidOperationException">At least one of the underlying collections was modified
		/// outside of its <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> instance.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The count of <paramref name="collections" /> is greater than the maximum size of
		/// 62 for STA and 63 for MTA.</exception>
		/// <remarks>A call to TryTakeFromAny may block until an item is available to be removed.</remarks>
		public static int TryTakeFromAny(BlockingCollection<T>[] collections, out T item, int millisecondsTimeout)
		{
			ValidateMillisecondsTimeout(millisecondsTimeout);
			T item2 = default(T);
			int result = TryAddTakeAny(collections, ref item2, millisecondsTimeout, OperationMode.Take, default(CancellationToken));
			item = item2;
			return result;
		}

		/// <summary>
		/// Attempts to remove an item from any one of the specified
		/// <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> instances.
		/// A <see cref="T:System.OperationCanceledException" /> is thrown if the <see cref="T:System.Threading.CancellationToken" /> is
		/// canceled. 
		/// </summary>
		/// <param name="collections">The array of collections.</param>
		/// <param name="item">The item removed from one of the collections.</param>
		/// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="F:System.Threading.Timeout.Infinite" /> (-1) to wait indefinitely.</param>
		/// <param name="cancellationToken">A cancellation token to observe.</param>
		/// <returns>The index of the collection in the <paramref name="collections" /> array from which 
		/// the item was removed, or -1 if an item could not be removed.</returns>
		/// <exception cref="T:System.OperationCanceledException">If the <see cref="T:System.Threading.CancellationToken" /> is canceled.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="collections" /> argument is
		/// null.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="collections" /> argument is
		/// a 0-length array or contains a null element.</exception>
		/// <exception cref="T:System.ObjectDisposedException">At least one of the <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> instances has been disposed.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="millisecondsTimeout" /> is a
		/// negative number other than -1, which represents an infinite time-out.</exception>
		/// <exception cref="T:System.InvalidOperationException">At least one of the underlying collections was modified
		/// outside of its <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> instance.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The count of <paramref name="collections" /> is greater than the maximum size of
		/// 62 for STA and 63 for MTA.</exception>
		/// <remarks>A call to TryTakeFromAny may block until an item is available to be removed.</remarks>
		public static int TryTakeFromAny(BlockingCollection<T>[] collections, out T item, int millisecondsTimeout, CancellationToken cancellationToken)
		{
			ValidateMillisecondsTimeout(millisecondsTimeout);
			T item2 = default(T);
			int result = TryAddTakeAny(collections, ref item2, millisecondsTimeout, OperationMode.Take, cancellationToken);
			item = item2;
			return result;
		}

		/// <summary>
		/// Marks the <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> instances
		/// as not accepting any more additions.  
		/// </summary>
		/// <remarks>
		/// After a collection has been marked as complete for adding, adding to the collection is not permitted 
		/// and attempts to remove from the collection will not wait when the collection is empty.
		/// </remarks>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> has been disposed.</exception>
		public void CompleteAdding()
		{
			CheckDisposed();
			if (IsAddingCompleted)
			{
				return;
			}
			SpinWait spinWait = default(SpinWait);
			while (true)
			{
				int currentAdders = m_currentAdders;
				if (((uint)currentAdders & 0x80000000u) != 0)
				{
					spinWait.Reset();
					while (m_currentAdders != int.MinValue)
					{
						spinWait.SpinOnce();
					}
					return;
				}
				if (Interlocked.CompareExchange(ref m_currentAdders, currentAdders | int.MinValue, currentAdders) == currentAdders)
				{
					break;
				}
				spinWait.SpinOnce();
			}
			spinWait.Reset();
			while (m_currentAdders != int.MinValue)
			{
				spinWait.SpinOnce();
			}
			if (Count == 0)
			{
				CancelWaitingConsumers();
			}
			CancelWaitingProducers();
		}

		/// <summary>Cancels the semaphores.</summary>
		private void CancelWaitingConsumers()
		{
			m_ConsumersCancellationTokenSource.Cancel();
		}

		private void CancelWaitingProducers()
		{
			m_ProducersCancellationTokenSource.Cancel();
		}

		/// <summary>
		/// Releases resources used by the <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> instance.
		/// </summary>
		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Releases resources used by the <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> instance.
		/// </summary>
		/// <param name="disposing">Whether being disposed explicitly (true) or due to a finalizer (false).</param>
		protected virtual void Dispose(bool disposing)
		{
			if (!m_isDisposed)
			{
				if (m_freeNodes != null)
				{
					m_freeNodes.Dispose();
				}
				m_occupiedNodes.Dispose();
				m_isDisposed = true;
			}
		}

		/// <summary>Copies the items from the <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> instance into a new array.</summary>
		/// <returns>An array containing copies of the elements of the collection.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> has been disposed.</exception>
		/// <remarks>
		/// The copied elements are not removed from the collection.
		/// </remarks>
		public T[] ToArray()
		{
			CheckDisposed();
			return m_collection.ToArray();
		}

		/// <summary>Copies all of the items in the <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> instance 
		/// to a compatible one-dimensional array, starting at the specified index of the target array.
		/// </summary>
		/// <param name="array">The one-dimensional array that is the destination of the elements copied from 
		/// the <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> instance. The array must have zero-based indexing.</param>
		/// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="array" /> argument is
		/// null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="index" /> argument is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="index" /> argument is equal to or greater 
		/// than the length of the <paramref name="array" />.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> has been disposed.</exception>
		public void CopyTo(T[] array, int index)
		{
			((ICollection)this).CopyTo((Array)array, index);
		}

		/// <summary>Copies all of the items in the <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> instance 
		/// to a compatible one-dimensional array, starting at the specified index of the target array.
		/// </summary>
		/// <param name="array">The one-dimensional array that is the destination of the elements copied from 
		/// the <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> instance. The array must have zero-based indexing.</param>
		/// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="array" /> argument is
		/// null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="index" /> argument is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="index" /> argument is equal to or greater 
		/// than the length of the <paramref name="array" />, the array is multidimensional, or the type parameter for the collection 
		/// cannot be cast automatically to the type of the destination array.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> has been disposed.</exception>
		void ICollection.CopyTo(Array array, int index)
		{
			CheckDisposed();
			T[] array2 = m_collection.ToArray();
			try
			{
				Array.Copy(array2, 0, array, index, array2.Length);
			}
			catch (ArgumentNullException)
			{
				throw new ArgumentNullException("array");
			}
			catch (ArgumentOutOfRangeException)
			{
				throw new ArgumentOutOfRangeException("index", index, "BlockingCollection_CopyTo_NonNegative");
			}
			catch (ArgumentException)
			{
				throw new ArgumentException("BlockingCollection_CopyTo_TooManyElems", "index");
			}
			catch (RankException)
			{
				throw new ArgumentException("BlockingCollection_CopyTo_MultiDim", "array");
			}
			catch (InvalidCastException)
			{
				throw new ArgumentException("BlockingCollection_CopyTo_IncorrectType", "array");
			}
			catch (ArrayTypeMismatchException)
			{
				throw new ArgumentException("BlockingCollection_CopyTo_IncorrectType", "array");
			}
		}

		/// <summary>Provides a consuming <see cref="T:System.Collections.Generics.IEnumerable{T}" /> for items in the collection.</summary>
		/// <returns>An <see cref="T:System.Collections.Generics.IEnumerable{T}" /> that removes and returns items from the collection.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> has been disposed.</exception>
		public IEnumerable<T> GetConsumingEnumerable()
		{
			return GetConsumingEnumerable(CancellationToken.None);
		}

		/// <summary>Provides a consuming <see cref="T:System.Collections.Generics.IEnumerable{T}" /> for items in the collection.
		/// Calling MoveNext on the returned enumerable will block if there is no data available, or will
		/// throw an <see cref="T:System.OperationCanceledException" /> if the <see cref="T:System.Threading.CancellationToken" /> is canceled.
		/// </summary>
		/// <param name="cancellationToken">A cancellation token to observe.</param>
		/// <returns>An <see cref="T:System.Collections.Generics.IEnumerable{T}" /> that removes and returns items from the collection.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> has been disposed.</exception>
		/// <exception cref="T:System.OperationCanceledException">If the <see cref="T:System.Threading.CancellationToken" /> is canceled.</exception>
		public IEnumerable<T> GetConsumingEnumerable(CancellationToken cancellationToken)
		{
			CancellationTokenSource linkedTokenSource = null;
			try
			{
				linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, m_ConsumersCancellationTokenSource.Token);
				while (!IsCompleted)
				{
					if (TryTakeWithNoTimeValidation(out var item, -1, cancellationToken, linkedTokenSource))
					{
						yield return item;
					}
				}
			}
			finally
			{
				linkedTokenSource?.Dispose();
			}
		}

		/// <summary>Provides an <see cref="T:System.Collections.Generics.IEnumerator{T}" /> for items in the collection.</summary>
		/// <returns>An <see cref="T:System.Collections.Generics.IEnumerator{T}" /> for the items in the collection.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> has been disposed.</exception>
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			CheckDisposed();
			return m_collection.GetEnumerator();
		}

		/// <summary>Provides an <see cref="T:System.Collections.IEnumerator" /> for items in the collection.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> for the items in the collection.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Collections.Concurrent.BlockingCollection{T}" /> has been disposed.</exception>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<T>)this).GetEnumerator();
		}

		/// <summary>Centralizes the logic for validating the BlockingCollections array passed to TryAddToAny()
		/// and TryTakeFromAny().</summary>
		/// <param name="collections">The collections to/from which an item should be added/removed.</param>
		/// <param name="operationMode">Indicates whether this method is called to Add or Take.</param>
		/// <returns>A copy of the collections array that acts as a defense to prevent an “outsider” from changing 
		/// elements of the array after we have done the validation on them.</returns>
		/// <exception cref="T:System.ArgumentNullException">If the collections argument is null.</exception>
		/// <exception cref="T:System.ArgumentException">If the collections argument is a 0-length array or contains a 
		/// null element. Also, if atleast one of the collections has been marked complete for adds.</exception>
		/// <exception cref="T:System.ObjectDisposedException">If atleast one of the collections has been disposed.</exception>
		private static BlockingCollection<T>[] ValidateCollectionsArray(BlockingCollection<T>[] collections, OperationMode operationMode)
		{
			if (collections == null)
			{
				throw new ArgumentNullException("collections");
			}
			if (collections.Length < 1)
			{
				throw new ArgumentException("BlockingCollection_ValidateCollectionsArray_ZeroSize", "collections");
			}
			if ((Thread.CurrentThread.GetApartmentState() == ApartmentState.MTA && collections.Length > 63) || (Thread.CurrentThread.GetApartmentState() == ApartmentState.STA && collections.Length > 62))
			{
				throw new ArgumentOutOfRangeException("collections", "BlockingCollection_ValidateCollectionsArray_LargeSize");
			}
			BlockingCollection<T>[] array = new BlockingCollection<T>[collections.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = collections[i];
				if (array[i] == null)
				{
					throw new ArgumentException("BlockingCollection_ValidateCollectionsArray_NullElems", "collections");
				}
				if (array[i].m_isDisposed)
				{
					throw new ObjectDisposedException("collections", "BlockingCollection_ValidateCollectionsArray_DispElems");
				}
				if (operationMode == OperationMode.Add && array[i].IsAddingCompleted)
				{
					throw new ArgumentException("BlockingCollection_CantTakeAnyWhenDone", "collections");
				}
			}
			return array;
		}

		/// <summary>Centeralizes the logic of validating the timeout input argument.</summary>
		/// <param name="timeout">The TimeSpan to wait for to successfully complete an operation on the collection.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">If the number of millseconds represented by the timeout 
		/// TimeSpan is less than 0 or is larger than Int32.MaxValue and not Timeout.Infinite</exception>
		private static void ValidateTimeout(TimeSpan timeout)
		{
			long num = (long)timeout.TotalMilliseconds;
			if ((num < 0 || num > int.MaxValue) && num != -1)
			{
				throw new ArgumentOutOfRangeException("timeout", timeout, string.Format(CultureInfo.InvariantCulture, "BlockingCollection_TimeoutInvalid", int.MaxValue));
			}
		}

		/// <summary>Centralizes the logic of validating the millisecondsTimeout input argument.</summary>
		/// <param name="millisecondsTimeout">The number of milliseconds to wait for to successfully complete an 
		/// operation on the collection.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">If the number of millseconds is less than 0 and not 
		/// equal to Timeout.Infinite.</exception>
		private static void ValidateMillisecondsTimeout(int millisecondsTimeout)
		{
			if (millisecondsTimeout < 0 && millisecondsTimeout != -1)
			{
				throw new ArgumentOutOfRangeException("millisecondsTimeout", millisecondsTimeout, string.Format(CultureInfo.InvariantCulture, "BlockingCollection_TimeoutInvalid", int.MaxValue));
			}
		}

		/// <summary>Throws a System.ObjectDisposedException if the collection was disposed</summary>
		/// <exception cref="T:System.ObjectDisposedException">If the collection has been disposed.</exception>
		private void CheckDisposed()
		{
			if (m_isDisposed)
			{
				throw new ObjectDisposedException("BlockingCollection", "BlockingCollection_Disposed");
			}
		}
	}
}
