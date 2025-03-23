using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;

namespace System.Collections.Concurrent
{
	/// <summary>
	/// Represents a particular manner of splitting a data source into multiple partitions.
	/// </summary>
	/// <typeparam name="TSource">Type of the elements in the collection.</typeparam>
	/// <remarks>
	/// <para>
	/// Inheritors of <see cref="T:System.Collections.Concurrent.Partitioner`1" /> must adhere to the following rules:
	/// <ol>
	/// <li><see cref="M:System.Collections.Concurrent.Partitioner`1.GetPartitions(System.Int32)" /> should throw a
	/// <see cref="T:System.ArgumentOutOfRangeException" /> if the requested partition count is less than or
	/// equal to zero.</li>
	/// <li><see cref="M:System.Collections.Concurrent.Partitioner`1.GetPartitions(System.Int32)" /> should always return a number of enumerables equal to the requested
	/// partition count. If the partitioner runs out of data and cannot create as many partitions as 
	/// requested, an empty enumerator should be returned for each of the remaining partitions. If this rule
	/// is not followed, consumers of the implementation may throw a <see cref="T:System.InvalidOperationException" />.</li>
	/// <li><see cref="M:System.Collections.Concurrent.Partitioner`1.GetPartitions(System.Int32)" /> and <see cref="M:System.Collections.Concurrent.Partitioner`1.GetDynamicPartitions" />
	/// should never return null. If null is returned, a consumer of the implementation may throw a
	/// <see cref="T:System.InvalidOperationException" />.</li>
	/// <li><see cref="M:System.Collections.Concurrent.Partitioner`1.GetPartitions(System.Int32)" /> and <see cref="M:System.Collections.Concurrent.Partitioner`1.GetDynamicPartitions" /> should always return
	/// partitions that can fully and uniquely enumerate the input data source. All of the data and only the
	/// data contained in the input source should be enumerated, with no duplication that was not already in
	/// the input, unless specifically required by the particular partitioner's design. If this is not
	/// followed, the output ordering may be scrambled.</li>
	/// </ol>
	/// </para>
	/// </remarks>
	[HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
	public abstract class Partitioner<TSource>
	{
		/// <summary>
		/// Gets whether additional partitions can be created dynamically.
		/// </summary>
		/// <returns>
		/// true if the <see cref="T:System.Collections.Concurrent.Partitioner`1" /> can create partitions dynamically as they are
		/// requested; false if the <see cref="T:System.Collections.Concurrent.Partitioner`1" /> can only allocate
		/// partitions statically.
		/// </returns>
		/// <remarks>
		/// <para>
		/// If a derived class does not override and implement <see cref="M:System.Collections.Concurrent.Partitioner`1.GetDynamicPartitions" />,
		/// <see cref="P:System.Collections.Concurrent.Partitioner`1.SupportsDynamicPartitions" /> should return false. The value of <see cref="P:System.Collections.Concurrent.Partitioner`1.SupportsDynamicPartitions" /> should not vary over the lifetime of this instance.
		/// </para>
		/// </remarks>
		public virtual bool SupportsDynamicPartitions => false;

		/// <summary>
		/// Partitions the underlying collection into the given number of partitions.
		/// </summary>
		/// <param name="partitionCount">The number of partitions to create.</param>
		/// <returns>A list containing <paramref name="partitionCount" /> enumerators.</returns>
		public abstract IList<IEnumerator<TSource>> GetPartitions(int partitionCount);

		/// <summary>
		/// Creates an object that can partition the underlying collection into a variable number of
		/// partitions.
		/// </summary>
		/// <remarks>
		/// <para>
		/// The returned object implements the <see cref="T:System.Collections.Generic.IEnumerable{TSource}" /> interface. Calling <see cref="M:System.Collections.Generic.IEnumerable`1.GetEnumerator">GetEnumerator</see> on the
		/// object creates another partition over the sequence.
		/// </para>
		/// <para>
		/// The <see cref="M:System.Collections.Concurrent.Partitioner`1.GetDynamicPartitions" /> method is only supported if the <see cref="P:System.Collections.Concurrent.Partitioner`1.SupportsDynamicPartitions" />
		/// property returns true.
		/// </para>
		/// </remarks>
		/// <returns>An object that can create partitions over the underlying data source.</returns>
		/// <exception cref="T:System.NotSupportedException">Dynamic partitioning is not supported by this
		/// partitioner.</exception>
		public virtual IEnumerable<TSource> GetDynamicPartitions()
		{
			throw new NotSupportedException(Environment2.GetResourceString("Partitioner_DynamicPartitionsNotSupported"));
		}
	}
	/// <summary>
	/// Provides common partitioning strategies for arrays, lists, and enumerables.
	/// </summary>
	/// <remarks>
	/// <para>
	/// The static methods on <see cref="T:System.Collections.Concurrent.Partitioner" /> are all thread-safe and may be used concurrently
	/// from multiple threads. However, while a created partitioner is in use, the underlying data source
	/// should not be modified, whether from the same thread that's using a partitioner or from a separate
	/// thread.
	/// </para>
	/// </remarks>
	[HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
	public static class Partitioner
	{
		/// <summary>
		/// DynamicPartitionEnumerator_Abstract defines the enumerator for each partition for the dynamic load-balance
		/// partitioning algorithm. 
		/// - Partition is an enumerator of KeyValuePairs, each corresponding to an item in the data source: 
		///   the key is the index in the source collection; the value is the item itself.
		/// - a set of such partitions share a reader over data source. The type of the reader is specified by
		///   TSourceReader. 
		/// - each partition requests a contiguous chunk of elements at a time from the source data. The chunk 
		///   size is initially 1, and doubles every time until it reaches the maximum chunk size. 
		///   The implementation for GrabNextChunk() method has two versions: one for data source of IndexRange 
		///   types (IList and the array), one for data source of IEnumerable.
		/// - The method "Reset" is not supported for any partitioning algorithm.
		/// - The implementation for MoveNext() method is same for all dynanmic partitioners, so we provide it
		///   in this abstract class.
		/// </summary>
		/// <typeparam name="TSource">Type of the elements in the data source</typeparam>
		/// <typeparam name="TSourceReader">Type of the reader on the data source</typeparam>
		private abstract class DynamicPartitionEnumerator_Abstract<TSource, TSourceReader> : IEnumerator<KeyValuePair<long, TSource>>, IDisposable, IEnumerator
		{
			private const int CHUNK_DOUBLING_RATE = 3;

			protected readonly TSourceReader m_sharedReader;

			protected static int s_defaultMaxChunkSize = GetDefaultChunkSize<TSource>();

			protected Shared<int> m_currentChunkSize;

			protected Shared<int> m_localOffset;

			private int m_doublingCountdown;

			protected readonly int m_maxChunkSize;

			protected readonly Shared<long> m_sharedIndex;

			/// <summary>
			/// Abstract property, returns whether or not the shared reader has already read the last 
			/// element of the source data 
			/// </summary>
			protected abstract bool HasNoElementsLeft { get; set; }

			/// <summary>
			/// Get the current element in the current partition. Property required by IEnumerator interface
			/// This property is abstract because the implementation is different depending on the type
			/// of the source data: IList, Array or IEnumerable
			/// </summary>
			public abstract KeyValuePair<long, TSource> Current { get; }

			/// <summary>
			/// Get the current element in the current partition. Property required by IEnumerator interface
			/// </summary>
			object IEnumerator.Current => Current;

			protected DynamicPartitionEnumerator_Abstract(TSourceReader sharedReader, Shared<long> sharedIndex)
				: this(sharedReader, sharedIndex, -1)
			{
			}

			protected DynamicPartitionEnumerator_Abstract(TSourceReader sharedReader, Shared<long> sharedIndex, int maxChunkSize)
			{
				m_sharedReader = sharedReader;
				m_sharedIndex = sharedIndex;
				if (maxChunkSize == -1)
				{
					m_maxChunkSize = s_defaultMaxChunkSize;
				}
				else
				{
					m_maxChunkSize = maxChunkSize;
				}
			}

			/// <summary>
			/// Abstract method to request a contiguous chunk of elements from the source collection
			/// </summary>
			/// <param name="requestedChunkSize">specified number of elements requested</param>
			/// <returns>
			/// true if we successfully reserved at least one element (up to #=requestedChunkSize) 
			/// false if all elements in the source collection have been reserved.
			/// </returns>
			protected abstract bool GrabNextChunk(int requestedChunkSize);

			/// <summary>
			/// Dispose is abstract, and depends on the type of the source data:
			/// - For source data type IList and Array, the type of the shared reader is just the dataitself.
			///   We don't do anything in Dispose method for IList and Array. 
			/// - For source data type IEnumerable, the type of the shared reader is an enumerator we created.
			///   Thus we need to dispose this shared reader enumerator, when there is no more active partitions
			///   left.
			/// </summary>
			public abstract void Dispose();

			/// <summary>
			/// Reset on partitions is not supported
			/// </summary>
			public void Reset()
			{
				throw new NotSupportedException();
			}

			/// <summary>
			/// Moves to the next element if any.
			/// Try current chunk first, if the current chunk do not have any elements left, then we 
			/// attempt to grab a chunk from the source collection.
			/// </summary>
			/// <returns>
			/// true if successfully moving to the next position;
			/// false otherwise, if and only if there is no more elements left in the current chunk 
			/// AND the source collection is exhausted. 
			/// </returns>
			public bool MoveNext()
			{
				if (m_localOffset == null)
				{
					m_localOffset = new Shared<int>(-1);
					m_currentChunkSize = new Shared<int>(0);
					m_doublingCountdown = 3;
				}
				if (m_localOffset.Value < m_currentChunkSize.Value - 1)
				{
					m_localOffset.Value++;
					return true;
				}
				int requestedChunkSize;
				if (m_currentChunkSize.Value == 0)
				{
					requestedChunkSize = 1;
				}
				else if (m_doublingCountdown > 0)
				{
					requestedChunkSize = m_currentChunkSize.Value;
				}
				else
				{
					requestedChunkSize = Math.Min(m_currentChunkSize.Value * 2, m_maxChunkSize);
					m_doublingCountdown = 3;
				}
				m_doublingCountdown--;
				if (GrabNextChunk(requestedChunkSize))
				{
					m_localOffset.Value = 0;
					return true;
				}
				return false;
			}
		}

		/// <summary>
		/// Inherits from DynamicPartitioners
		/// Provides customized implementation of GetOrderableDynamicPartitions_Factory method, to return an instance
		/// of EnumerableOfPartitionsForIEnumerator defined internally
		/// </summary>
		/// <typeparam name="TSource">Type of elements in the source data</typeparam>
		private class DynamicPartitionerForIEnumerable<TSource> : OrderablePartitioner<TSource>
		{
			/// <summary>
			/// Provides customized implementation for source data of IEnumerable
			/// Different from the counterpart for IList/Array, this enumerable maintains several additional fields
			/// shared by the partitions it owns, including a boolean "m_hasNoElementsLef", a shared lock, and a 
			/// shared count "m_activePartitionCount"
			/// </summary>
			private class InternalPartitionEnumerable : IEnumerable<KeyValuePair<long, TSource>>, IEnumerable, IDisposable
			{
				private readonly IEnumerator<TSource> m_sharedReader;

				private Shared<long> m_sharedIndex;

				private Shared<bool> m_hasNoElementsLeft;

				private object m_sharedLock;

				private bool m_disposed;

				private Shared<int> m_activePartitionCount;

				private readonly int m_maxChunkSize;

				internal InternalPartitionEnumerable(IEnumerator<TSource> sharedReader, int maxChunkSize)
				{
					m_sharedReader = sharedReader;
					m_sharedIndex = new Shared<long>(-1L);
					m_hasNoElementsLeft = new Shared<bool>(value: false);
					m_sharedLock = new object();
					m_activePartitionCount = new Shared<int>(0);
					m_maxChunkSize = maxChunkSize;
				}

				public IEnumerator<KeyValuePair<long, TSource>> GetEnumerator()
				{
					if (m_disposed)
					{
						throw new ObjectDisposedException(Environment2.GetResourceString("PartitionerStatic_CanNotCallGetEnumeratorAfterSourceHasBeenDisposed"));
					}
					return new InternalPartitionEnumerator(m_sharedReader, m_sharedIndex, m_hasNoElementsLeft, m_sharedLock, m_activePartitionCount, this, m_maxChunkSize);
				}

				IEnumerator IEnumerable.GetEnumerator()
				{
					return GetEnumerator();
				}

				public void Dispose()
				{
					if (!m_disposed)
					{
						m_disposed = true;
						m_sharedReader.Dispose();
					}
				}
			}

			/// <summary>
			/// Inherits from DynamicPartitionEnumerator_Abstract directly
			/// Provides customized implementation for: GrabNextChunk, HasNoElementsLeft, Current, Dispose
			/// </summary>
			private class InternalPartitionEnumerator : DynamicPartitionEnumerator_Abstract<TSource, IEnumerator<TSource>>
			{
				private KeyValuePair<long, TSource>[] m_localList;

				private readonly Shared<bool> m_hasNoElementsLeft;

				private readonly object m_sharedLock;

				private readonly Shared<int> m_activePartitionCount;

				private InternalPartitionEnumerable m_enumerable;

				/// <summary>
				/// Returns whether or not the shared reader has already read the last 
				/// element of the source data 
				/// </summary>
				/// <remarks>
				/// We cannot call m_sharedReader.MoveNext(), to see if it hits the last element
				/// or not, because we can't undo MoveNext(). Thus we need to maintain a shared 
				/// boolean value m_hasNoElementsLeft across all partitions
				/// </remarks>
				protected override bool HasNoElementsLeft
				{
					get
					{
						return m_hasNoElementsLeft.Value;
					}
					set
					{
						m_hasNoElementsLeft.Value = true;
					}
				}

				public override KeyValuePair<long, TSource> Current
				{
					get
					{
						if (m_currentChunkSize == null)
						{
							throw new InvalidOperationException(Environment2.GetResourceString("PartitionerStatic_CurrentCalledBeforeMoveNext"));
						}
						return m_localList[m_localOffset.Value];
					}
				}

				internal InternalPartitionEnumerator(IEnumerator<TSource> sharedReader, Shared<long> sharedIndex, Shared<bool> hasNoElementsLeft, object sharedLock, Shared<int> activePartitionCount, InternalPartitionEnumerable enumerable, int maxChunkSize)
					: base(sharedReader, sharedIndex, maxChunkSize)
				{
					m_hasNoElementsLeft = hasNoElementsLeft;
					m_sharedLock = sharedLock;
					m_enumerable = enumerable;
					m_activePartitionCount = activePartitionCount;
					Interlocked.Increment(ref m_activePartitionCount.Value);
				}

				/// <summary>
				/// Reserves a contiguous range of elements from source data
				/// </summary>
				/// <param name="requestedChunkSize">specified number of elements requested</param>
				/// <returns>
				/// true if we successfully reserved at least one element (up to #=requestedChunkSize) 
				/// false if all elements in the source collection have been reserved.
				/// </returns>
				protected override bool GrabNextChunk(int requestedChunkSize)
				{
					if (HasNoElementsLeft)
					{
						return false;
					}
					lock (m_sharedLock)
					{
						if (HasNoElementsLeft)
						{
							return false;
						}
						try
						{
							int i;
							for (i = 0; i < requestedChunkSize; i++)
							{
								if (m_sharedReader.MoveNext())
								{
									if (m_localList == null)
									{
										m_localList = new KeyValuePair<long, TSource>[m_maxChunkSize];
									}
									m_sharedIndex.Value = checked(m_sharedIndex.Value + 1);
									ref KeyValuePair<long, TSource> reference = ref m_localList[i];
									reference = new KeyValuePair<long, TSource>(m_sharedIndex.Value, m_sharedReader.Current);
									continue;
								}
								HasNoElementsLeft = true;
								break;
							}
							if (i > 0)
							{
								m_currentChunkSize.Value = i;
								return true;
							}
							return false;
						}
						catch
						{
							HasNoElementsLeft = true;
							throw;
						}
					}
				}

				/// <summary>
				/// If the current partition is to be disposed, we decrement the number of active partitions
				/// for the shared reader. 
				/// If the number of active partitions becomes 0, we need to dispose the shared reader we created
				/// </summary>
				public override void Dispose()
				{
					if (Interlocked.Decrement(ref m_activePartitionCount.Value) == 0)
					{
						m_enumerable.Dispose();
					}
				}
			}

			private IEnumerable<TSource> m_source;

			private int m_maxChunkSize;

			/// <summary>
			/// Whether additional partitions can be created dynamically.
			/// </summary>
			public override bool SupportsDynamicPartitions => true;

			internal DynamicPartitionerForIEnumerable(IEnumerable<TSource> source, int maxChunkSize)
				: base(keysOrderedInEachPartition: true, keysOrderedAcrossPartitions: false, keysNormalized: true)
			{
				m_source = source;
				m_maxChunkSize = maxChunkSize;
			}

			/// <summary>
			/// Overrides OrderablePartitioner.GetOrderablePartitions.
			/// Partitions the underlying collection into the given number of orderable partitions.
			/// </summary>
			/// <param name="partitionCount">number of partitions requested</param>
			/// <returns>A list containing <paramref name="partitionCount" /> enumerators.</returns>
			public override IList<IEnumerator<KeyValuePair<long, TSource>>> GetOrderablePartitions(int partitionCount)
			{
				if (partitionCount <= 0)
				{
					throw new ArgumentOutOfRangeException("partitionCount");
				}
				IEnumerator<KeyValuePair<long, TSource>>[] array = new IEnumerator<KeyValuePair<long, TSource>>[partitionCount];
				IEnumerable<KeyValuePair<long, TSource>> enumerable = new InternalPartitionEnumerable(m_source.GetEnumerator(), m_maxChunkSize);
				for (int i = 0; i < partitionCount; i++)
				{
					array[i] = enumerable.GetEnumerator();
				}
				return array;
			}

			/// <summary>
			/// Overrides OrderablePartitioner.GetOrderableDyanmicPartitions
			/// </summary>
			/// <returns>a enumerable collection of orderable partitions</returns>
			public override IEnumerable<KeyValuePair<long, TSource>> GetOrderableDynamicPartitions()
			{
				return new InternalPartitionEnumerable(m_source.GetEnumerator(), m_maxChunkSize);
			}
		}

		/// <summary>
		/// Dynamic load-balance partitioner. This class is abstract and to be derived from by 
		/// the customized partitioner classes for IList, Array, and IEnumerable
		/// </summary>
		/// <typeparam name="TSource">Type of the elements in the source data</typeparam>
		/// <typeparam name="TCollection"> Type of the source data collection</typeparam>
		private abstract class DynamicPartitionerForIndexRange_Abstract<TSource, TCollection> : OrderablePartitioner<TSource>
		{
			private TCollection m_data;

			/// <summary>
			/// Whether additional partitions can be created dynamically.
			/// </summary>
			public override bool SupportsDynamicPartitions => true;

			/// <summary>
			/// Constructs a new orderable partitioner 
			/// </summary>
			/// <param name="data">source data collection</param>
			protected DynamicPartitionerForIndexRange_Abstract(TCollection data)
				: base(keysOrderedInEachPartition: true, keysOrderedAcrossPartitions: false, keysNormalized: true)
			{
				m_data = data;
			}

			/// <summary>
			/// Partition the source data and create an enumerable over the resulting partitions. 
			/// </summary>
			/// <param name="data">the source data collection</param>
			/// <returns>an enumerable of partitions of </returns>
			protected abstract IEnumerable<KeyValuePair<long, TSource>> GetOrderableDynamicPartitions_Factory(TCollection data);

			/// <summary>
			/// Overrides OrderablePartitioner.GetOrderablePartitions.
			/// Partitions the underlying collection into the given number of orderable partitions.
			/// </summary>
			/// <param name="partitionCount">number of partitions requested</param>
			/// <returns>A list containing <paramref name="partitionCount" /> enumerators.</returns>
			public override IList<IEnumerator<KeyValuePair<long, TSource>>> GetOrderablePartitions(int partitionCount)
			{
				if (partitionCount <= 0)
				{
					throw new ArgumentOutOfRangeException("partitionCount");
				}
				IEnumerator<KeyValuePair<long, TSource>>[] array = new IEnumerator<KeyValuePair<long, TSource>>[partitionCount];
				IEnumerable<KeyValuePair<long, TSource>> orderableDynamicPartitions_Factory = GetOrderableDynamicPartitions_Factory(m_data);
				for (int i = 0; i < partitionCount; i++)
				{
					array[i] = orderableDynamicPartitions_Factory.GetEnumerator();
				}
				return array;
			}

			/// <summary>
			/// Overrides OrderablePartitioner.GetOrderableDyanmicPartitions
			/// </summary>
			/// <returns>a enumerable collection of orderable partitions</returns>
			public override IEnumerable<KeyValuePair<long, TSource>> GetOrderableDynamicPartitions()
			{
				return GetOrderableDynamicPartitions_Factory(m_data);
			}
		}

		/// <summary>
		/// Defines dynamic partition for source data of IList and Array. 
		/// This class inherits DynamicPartitionEnumerator_Abstract
		///   - implements GrabNextChunk, HasNoElementsLeft, and Dispose methods for IList and Array
		///   - Current property still remains abstract, implementation is different for IList and Array
		///   - introduces another abstract method SourceCount, which returns the number of elements in
		///     the source data. Implementation differs for IList and Array
		/// </summary>
		/// <typeparam name="TSource">Type of the elements in the data source</typeparam>
		/// <typeparam name="TSourceReader">Type of the reader on the source data</typeparam>
		private abstract class DynamicPartitionEnumeratorForIndexRange_Abstract<TSource, TSourceReader> : DynamicPartitionEnumerator_Abstract<TSource, TSourceReader>
		{
			protected int m_startIndex;

			/// <summary>
			/// Get the number of elements from the source reader.
			/// It calls IList.Count or Array.Length
			/// </summary>
			protected abstract int SourceCount { get; }

			/// <summary>
			/// Returns whether or not the shared reader has already read the last 
			/// element of the source data 
			/// </summary>
			protected override bool HasNoElementsLeft
			{
				get
				{
					return m_sharedIndex.Value >= SourceCount - 1;
				}
				set
				{
				}
			}

			protected DynamicPartitionEnumeratorForIndexRange_Abstract(TSourceReader sharedReader, Shared<long> sharedIndex)
				: base(sharedReader, sharedIndex)
			{
			}

			/// <summary>
			/// Reserves a contiguous range of elements from source data
			/// </summary>
			/// <param name="requestedChunkSize">specified number of elements requested</param>
			/// <returns>
			/// true if we successfully reserved at least one element (up to #=requestedChunkSize) 
			/// false if all elements in the source collection have been reserved.
			/// </returns>
			protected override bool GrabNextChunk(int requestedChunkSize)
			{
				while (!HasNoElementsLeft)
				{
					long value = m_sharedIndex.Value;
					if (HasNoElementsLeft)
					{
						return false;
					}
					long num = Math.Min(SourceCount - 1, value + requestedChunkSize);
					if (Interlocked.CompareExchange(ref m_sharedIndex.Value, num, value) == value)
					{
						m_currentChunkSize.Value = (int)(num - value);
						m_localOffset.Value = -1;
						m_startIndex = (int)(value + 1);
						return true;
					}
				}
				return false;
			}

			/// <summary>
			/// For source data type IList and Array, the type of the shared reader is just the data itself.
			/// We don't do anything in Dispose method for IList and Array. 
			/// </summary>
			public override void Dispose()
			{
			}
		}

		/// <summary>
		/// Inherits from DynamicPartitioners
		/// Provides customized implementation of GetOrderableDynamicPartitions_Factory method, to return an instance
		/// of EnumerableOfPartitionsForIList defined internally
		/// </summary>
		/// <typeparam name="TSource">Type of elements in the source data</typeparam>
		private class DynamicPartitionerForIList<TSource> : DynamicPartitionerForIndexRange_Abstract<TSource, IList<TSource>>
		{
			/// <summary>
			/// Inherits from PartitionList_Abstract 
			/// Provides customized implementation for source data of IList
			/// </summary>
			private class InternalPartitionEnumerable : IEnumerable<KeyValuePair<long, TSource>>, IEnumerable
			{
				private readonly IList<TSource> m_sharedReader;

				private Shared<long> m_sharedIndex;

				internal InternalPartitionEnumerable(IList<TSource> sharedReader)
				{
					m_sharedReader = sharedReader;
					m_sharedIndex = new Shared<long>(-1L);
				}

				public IEnumerator<KeyValuePair<long, TSource>> GetEnumerator()
				{
					return new InternalPartitionEnumerator(m_sharedReader, m_sharedIndex);
				}

				IEnumerator IEnumerable.GetEnumerator()
				{
					return GetEnumerator();
				}
			}

			/// <summary>
			/// Inherits from DynamicPartitionEnumeratorForIndexRange_Abstract
			/// Provides customized implementation of SourceCount property and Current property for IList
			/// </summary>
			private class InternalPartitionEnumerator : DynamicPartitionEnumeratorForIndexRange_Abstract<TSource, IList<TSource>>
			{
				protected override int SourceCount => m_sharedReader.Count;

				/// <summary>
				/// return a KeyValuePair of the current element and its key 
				/// </summary>
				public override KeyValuePair<long, TSource> Current
				{
					get
					{
						if (m_currentChunkSize == null)
						{
							throw new InvalidOperationException(Environment2.GetResourceString("PartitionerStatic_CurrentCalledBeforeMoveNext"));
						}
						return new KeyValuePair<long, TSource>(m_startIndex + m_localOffset.Value, m_sharedReader[m_startIndex + m_localOffset.Value]);
					}
				}

				internal InternalPartitionEnumerator(IList<TSource> sharedReader, Shared<long> sharedIndex)
					: base(sharedReader, sharedIndex)
				{
				}
			}

			internal DynamicPartitionerForIList(IList<TSource> source)
				: base(source)
			{
			}

			protected override IEnumerable<KeyValuePair<long, TSource>> GetOrderableDynamicPartitions_Factory(IList<TSource> m_data)
			{
				return new InternalPartitionEnumerable(m_data);
			}
		}

		/// <summary>
		/// Inherits from DynamicPartitioners
		/// Provides customized implementation of GetOrderableDynamicPartitions_Factory method, to return an instance
		/// of EnumerableOfPartitionsForArray defined internally
		/// </summary>
		/// <typeparam name="TSource">Type of elements in the source data</typeparam>
		private class DynamicPartitionerForArray<TSource> : DynamicPartitionerForIndexRange_Abstract<TSource, TSource[]>
		{
			/// <summary>
			/// Inherits from PartitionList_Abstract 
			/// Provides customized implementation for source data of Array
			/// </summary>
			private class InternalPartitionEnumerable : IEnumerable<KeyValuePair<long, TSource>>, IEnumerable
			{
				private readonly TSource[] m_sharedReader;

				private Shared<long> m_sharedIndex;

				internal InternalPartitionEnumerable(TSource[] sharedReader)
				{
					m_sharedReader = sharedReader;
					m_sharedIndex = new Shared<long>(-1L);
				}

				IEnumerator IEnumerable.GetEnumerator()
				{
					return GetEnumerator();
				}

				public IEnumerator<KeyValuePair<long, TSource>> GetEnumerator()
				{
					return new InternalPartitionEnumerator(m_sharedReader, m_sharedIndex);
				}
			}

			/// <summary>
			/// Inherits from DynamicPartitionEnumeratorForIndexRange_Abstract
			/// Provides customized implementation of SourceCount property and Current property for Array
			/// </summary>
			private class InternalPartitionEnumerator : DynamicPartitionEnumeratorForIndexRange_Abstract<TSource, TSource[]>
			{
				protected override int SourceCount => m_sharedReader.Length;

				public override KeyValuePair<long, TSource> Current
				{
					get
					{
						if (m_currentChunkSize == null)
						{
							throw new InvalidOperationException(Environment2.GetResourceString("PartitionerStatic_CurrentCalledBeforeMoveNext"));
						}
						return new KeyValuePair<long, TSource>(m_startIndex + m_localOffset.Value, m_sharedReader[m_startIndex + m_localOffset.Value]);
					}
				}

				internal InternalPartitionEnumerator(TSource[] sharedReader, Shared<long> sharedIndex)
					: base(sharedReader, sharedIndex)
				{
				}
			}

			internal DynamicPartitionerForArray(TSource[] source)
				: base(source)
			{
			}

			protected override IEnumerable<KeyValuePair<long, TSource>> GetOrderableDynamicPartitions_Factory(TSource[] m_data)
			{
				return new InternalPartitionEnumerable(m_data);
			}
		}

		/// <summary>
		/// Static partitioning over IList. 
		/// - dynamic and load-balance
		/// - Keys are ordered within each partition
		/// - Keys are ordered across partitions
		/// - Keys are normalized
		/// - Number of partitions is fixed once specified, and the elements of the source data are 
		/// distributed to each partition as evenly as possible. 
		/// </summary>
		/// <typeparam name="TSource">type of the elements</typeparam>        
		/// <typeparam name="TCollection">Type of the source data collection</typeparam>
		private abstract class StaticIndexRangePartitioner<TSource, TCollection> : OrderablePartitioner<TSource>
		{
			/// <summary>
			/// Abstract method to return the number of elements in the source data
			/// </summary>
			protected abstract int SourceCount { get; }

			protected StaticIndexRangePartitioner()
				: base(keysOrderedInEachPartition: true, keysOrderedAcrossPartitions: true, keysNormalized: true)
			{
			}

			/// <summary>
			/// Abstract method to create a partition that covers a range over source data, 
			/// starting from "startIndex", ending at "endIndex"
			/// </summary>
			/// <param name="startIndex">start index of the current partition on the source data</param>
			/// <param name="endIndex">end index of the current partition on the source data</param>
			/// <returns>a partition enumerator over the specified range</returns>
			protected abstract IEnumerator<KeyValuePair<long, TSource>> CreatePartition(int startIndex, int endIndex);

			/// <summary>
			/// Overrides OrderablePartitioner.GetOrderablePartitions
			/// Return a list of partitions, each of which enumerate a fixed part of the source data
			/// The elements of the source data are distributed to each partition as evenly as possible. 
			/// Specifically, if the total number of elements is N, and number of partitions is x, and N = a*x +b, 
			/// where a is the quotient, and b is the remainder. Then the first b partitions each has a + 1 elements,
			/// and the last x-b partitions each has a elements.
			/// For example, if N=10, x =3, then 
			///    partition 0 ranges [0,3],
			///    partition 1 ranges [4,6],
			///    partition 2 ranges [7,9].
			/// This also takes care of the situation of (x&gt;N), the last x-N partitions are empty enumerators. 
			/// An empty enumerator is indicated by 
			///      (m_startIndex == list.Count &amp;&amp; m_endIndex == list.Count -1)
			/// </summary>
			/// <param name="partitionCount">specified number of partitions</param>
			/// <returns>a list of partitions</returns>
			public override IList<IEnumerator<KeyValuePair<long, TSource>>> GetOrderablePartitions(int partitionCount)
			{
				if (partitionCount <= 0)
				{
					throw new ArgumentOutOfRangeException("partitionCount");
				}
				int result;
				int num = Math.DivRem(SourceCount, partitionCount, out result);
				IEnumerator<KeyValuePair<long, TSource>>[] array = new IEnumerator<KeyValuePair<long, TSource>>[partitionCount];
				int num2 = -1;
				for (int i = 0; i < partitionCount; i++)
				{
					int num3 = num2 + 1;
					num2 = ((i >= result) ? (num3 + num - 1) : (num3 + num));
					array[i] = CreatePartition(num3, num2);
				}
				return array;
			}
		}

		/// <summary>
		/// Static Partition for IList/Array.
		/// This class implements all methods required by IEnumerator interface, except for the Current property.
		/// Current Property is different for IList and Array. Arrays calls 'ldelem' instructions for faster element 
		/// retrieval.
		/// </summary>
		private abstract class StaticIndexRangePartition<TSource> : IEnumerator<KeyValuePair<long, TSource>>, IDisposable, IEnumerator
		{
			protected readonly int m_startIndex;

			protected readonly int m_endIndex;

			protected volatile int m_offset;

			/// <summary>
			/// Current Property is different for IList and Array. Arrays calls 'ldelem' instructions for faster 
			/// element retrieval.
			/// </summary>
			public abstract KeyValuePair<long, TSource> Current { get; }

			object IEnumerator.Current => Current;

			/// <summary>
			/// Constructs an instance of StaticIndexRangePartition
			/// </summary>
			/// <param name="startIndex">the start index in the source collection for the current partition </param>
			/// <param name="endIndex">the end index in the source collection for the current partition</param>
			protected StaticIndexRangePartition(int startIndex, int endIndex)
			{
				m_startIndex = startIndex;
				m_endIndex = endIndex;
				m_offset = startIndex - 1;
			}

			/// <summary>
			/// We don't dispose the source for IList and array
			/// </summary>
			public void Dispose()
			{
			}

			public void Reset()
			{
				throw new NotSupportedException();
			}

			/// <summary>
			/// Moves to the next item
			/// Before the first MoveNext is called: m_offset == m_startIndex-1;
			/// </summary>
			/// <returns>true if successful, false if there is no item left</returns>
			public bool MoveNext()
			{
				if (m_offset < m_endIndex)
				{
					m_offset++;
					return true;
				}
				m_offset = m_endIndex + 1;
				return false;
			}
		}

		/// <summary>
		/// Inherits from StaticIndexRangePartitioner
		/// Provides customized implementation of SourceCount and CreatePartition
		/// </summary>
		/// <typeparam name="TSource"></typeparam>
		private class StaticIndexRangePartitionerForIList<TSource> : StaticIndexRangePartitioner<TSource, IList<TSource>>
		{
			private IList<TSource> m_list;

			protected override int SourceCount => m_list.Count;

			internal StaticIndexRangePartitionerForIList(IList<TSource> list)
			{
				m_list = list;
			}

			protected override IEnumerator<KeyValuePair<long, TSource>> CreatePartition(int startIndex, int endIndex)
			{
				return new StaticIndexRangePartitionForIList<TSource>(m_list, startIndex, endIndex);
			}
		}

		/// <summary>
		/// Inherits from StaticIndexRangePartition
		/// Provides customized implementation of Current property
		/// </summary>
		/// <typeparam name="TSource"></typeparam>
		private class StaticIndexRangePartitionForIList<TSource> : StaticIndexRangePartition<TSource>
		{
			private volatile IList<TSource> m_list;

			public override KeyValuePair<long, TSource> Current
			{
				get
				{
					if (m_offset < m_startIndex)
					{
						throw new InvalidOperationException(Environment2.GetResourceString("PartitionerStatic_CurrentCalledBeforeMoveNext"));
					}
					return new KeyValuePair<long, TSource>(m_offset, m_list[m_offset]);
				}
			}

			internal StaticIndexRangePartitionForIList(IList<TSource> list, int startIndex, int endIndex)
				: base(startIndex, endIndex)
			{
				m_list = list;
			}
		}

		/// <summary>
		/// Inherits from StaticIndexRangePartitioner
		/// Provides customized implementation of SourceCount and CreatePartition for Array
		/// </summary>
		private class StaticIndexRangePartitionerForArray<TSource> : StaticIndexRangePartitioner<TSource, TSource[]>
		{
			private TSource[] m_array;

			protected override int SourceCount => m_array.Length;

			internal StaticIndexRangePartitionerForArray(TSource[] array)
			{
				m_array = array;
			}

			protected override IEnumerator<KeyValuePair<long, TSource>> CreatePartition(int startIndex, int endIndex)
			{
				return new StaticIndexRangePartitionForArray<TSource>(m_array, startIndex, endIndex);
			}
		}

		/// <summary>
		/// Inherits from StaticIndexRangePartitioner
		/// Provides customized implementation of SourceCount and CreatePartition
		/// </summary>
		private class StaticIndexRangePartitionForArray<TSource> : StaticIndexRangePartition<TSource>
		{
			private volatile TSource[] m_array;

			public override KeyValuePair<long, TSource> Current
			{
				get
				{
					if (m_offset < m_startIndex)
					{
						throw new InvalidOperationException(Environment2.GetResourceString("PartitionerStatic_CurrentCalledBeforeMoveNext"));
					}
					return new KeyValuePair<long, TSource>(m_offset, m_array[m_offset]);
				}
			}

			internal StaticIndexRangePartitionForArray(TSource[] array, int startIndex, int endIndex)
				: base(startIndex, endIndex)
			{
				m_array = array;
			}
		}

		/// <summary>
		/// A very simple primitive that allows us to share a value across multiple threads.
		/// </summary>
		/// <typeparam name="TSource"></typeparam>
		private class Shared<TSource>
		{
			internal TSource Value;

			internal Shared(TSource value)
			{
				Value = value;
			}
		}

		private const int DEFAULT_BYTES_PER_CHUNK = 512;

		/// <summary>
		/// Creates an orderable partitioner from an <see cref="T:System.Collections.Generic.IList`1" />
		/// instance.
		/// </summary>
		/// <typeparam name="TSource">Type of the elements in source list.</typeparam>
		/// <param name="list">The list to be partitioned.</param>
		/// <param name="loadBalance">
		/// A Boolean value that indicates whether the created partitioner should dynamically
		/// load balance between partitions rather than statically partition.
		/// </param>
		/// <returns>
		/// An orderable partitioner based on the input list.
		/// </returns>
		public static OrderablePartitioner<TSource> Create<TSource>(IList<TSource> list, bool loadBalance)
		{
			if (list == null)
			{
				throw new ArgumentNullException("list");
			}
			if (loadBalance)
			{
				return new DynamicPartitionerForIList<TSource>(list);
			}
			return new StaticIndexRangePartitionerForIList<TSource>(list);
		}

		/// <summary>
		/// Creates an orderable partitioner from a <see cref="T:System.Array" /> instance.
		/// </summary>
		/// <typeparam name="TSource">Type of the elements in source array.</typeparam>
		/// <param name="array">The array to be partitioned.</param>
		/// <param name="loadBalance">
		/// A Boolean value that indicates whether the created partitioner should dynamically load balance
		/// between partitions rather than statically partition.
		/// </param>
		/// <returns>
		/// An orderable partitioner based on the input array.
		/// </returns>
		public static OrderablePartitioner<TSource> Create<TSource>(TSource[] array, bool loadBalance)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (loadBalance)
			{
				return new DynamicPartitionerForArray<TSource>(array);
			}
			return new StaticIndexRangePartitionerForArray<TSource>(array);
		}

		/// <summary>
		/// Creates an orderable partitioner from a <see cref="T:System.Collections.Generic.IEnumerable`1" /> instance.
		/// </summary>
		/// <typeparam name="TSource">Type of the elements in source enumerable.</typeparam>
		/// <param name="source">The enumerable to be partitioned.</param>
		/// <returns>
		/// An orderable partitioner based on the input array.
		/// </returns>
		/// <remarks>
		/// The ordering used in the created partitioner is determined by the natural order of the elements 
		/// as retrieved from the source enumerable.
		/// </remarks>
		public static OrderablePartitioner<TSource> Create<TSource>(IEnumerable<TSource> source)
		{
			return Create(source, -1);
		}

		internal static OrderablePartitioner<TSource> Create<TSource>(IEnumerable<TSource> source, int maxChunkSize)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			return new DynamicPartitionerForIEnumerable<TSource>(source, maxChunkSize);
		}

		private static int GetDefaultChunkSize<TSource>()
		{
			if (typeof(TSource).IsValueType)
			{
				if (typeof(TSource).StructLayoutAttribute.Value == LayoutKind.Explicit)
				{
					return Math.Max(1, 512 / Marshal.SizeOf(typeof(TSource)));
				}
				return 128;
			}
			return 512 / IntPtr.Size;
		}
	}
}
