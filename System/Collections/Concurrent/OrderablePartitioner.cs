using System.Collections.Generic;
using System.Security.Permissions;

namespace System.Collections.Concurrent
{
	/// <summary>
	/// Represents a particular manner of splitting an orderable data source into multiple partitions.
	/// </summary>
	/// <typeparam name="TSource">Type of the elements in the collection.</typeparam>
	/// <remarks>
	/// <para>
	/// Each element in each partition has an integer index associated with it, which determines the relative
	/// order of that element against elements in other partitions.
	/// </para>
	/// <para>
	/// Inheritors of <see cref="T:System.Collections.Concurrent.OrderablePartitioner`1" /> must adhere to the following rules:
	/// <ol>
	/// <li>All indices must be unique, such that there may not be duplicate indices. If all indices are not
	/// unique, the output ordering may be scrambled.</li>
	/// <li>All indices must be non-negative. If any indices are negative, consumers of the implementation
	/// may throw exceptions.</li>
	/// <li><see cref="M:System.Collections.Concurrent.OrderablePartitioner`1.GetPartitions(System.Int32)" /> and <see cref="M:System.Collections.Concurrent.OrderablePartitioner`1.GetOrderablePartitions(System.Int32)" /> should throw a
	/// <see cref="T:System.ArgumentOutOfRangeException" /> if the requested partition count is less than or
	/// equal to zero.</li>
	/// <li><see cref="M:System.Collections.Concurrent.OrderablePartitioner`1.GetPartitions(System.Int32)" /> and <see cref="M:System.Collections.Concurrent.OrderablePartitioner`1.GetOrderablePartitions(System.Int32)" /> should always return a number
	/// of enumerables equal to the requested partition count. If the partitioner runs out of data and cannot
	/// create as many partitions as requested, an empty enumerator should be returned for each of the
	/// remaining partitions. If this rule is not followed, consumers of the implementation may throw a <see cref="T:System.InvalidOperationException" />.</li>
	/// <li><see cref="M:System.Collections.Concurrent.OrderablePartitioner`1.GetPartitions(System.Int32)" />, <see cref="M:System.Collections.Concurrent.OrderablePartitioner`1.GetOrderablePartitions(System.Int32)" />,
	/// <see cref="M:System.Collections.Concurrent.OrderablePartitioner`1.GetDynamicPartitions" />, and <see cref="M:System.Collections.Concurrent.OrderablePartitioner`1.GetOrderableDynamicPartitions" />
	/// should never return null. If null is returned, a consumer of the implementation may throw a
	/// <see cref="T:System.InvalidOperationException" />.</li>
	/// <li><see cref="M:System.Collections.Concurrent.OrderablePartitioner`1.GetPartitions(System.Int32)" />, <see cref="M:System.Collections.Concurrent.OrderablePartitioner`1.GetOrderablePartitions(System.Int32)" />,
	/// <see cref="M:System.Collections.Concurrent.OrderablePartitioner`1.GetDynamicPartitions" />, and <see cref="M:System.Collections.Concurrent.OrderablePartitioner`1.GetOrderableDynamicPartitions" />
	/// should always return partitions that can fully and uniquely enumerate the input data source. All of
	/// the data and only the data contained in the input source should be enumerated, with no duplication
	/// that was not already in the input, unless specifically required by the particular partitioner's
	/// design. If this is not followed, the output ordering may be scrambled.</li>
	/// <li>If <see cref="P:System.Collections.Concurrent.OrderablePartitioner`1.KeysOrderedInEachPartition" /> returns true, each partition must return elements
	/// with increasing key indices.</li>
	/// <li>If <see cref="P:System.Collections.Concurrent.OrderablePartitioner`1.KeysOrderedAcrossPartitions" /> returns true, all the keys in partition numbered N
	/// must be larger than all the keys in partition numbered N-1.</li>
	/// <li>If <see cref="P:System.Collections.Concurrent.OrderablePartitioner`1.KeysNormalized" /> returns true, all indices must be monotonically increasing from
	/// 0, though not necessarily within a single partition.</li>
	/// </ol>
	/// </para>
	/// </remarks>
	[HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
	public abstract class OrderablePartitioner<TSource> : Partitioner<TSource>
	{
		/// <summary>
		/// Converts an enumerable over key-value pairs to an enumerable over values.
		/// </summary>
		private class EnumerableDropIndices : IEnumerable<TSource>, IEnumerable, IDisposable
		{
			private readonly IEnumerable<KeyValuePair<long, TSource>> m_source;

			public EnumerableDropIndices(IEnumerable<KeyValuePair<long, TSource>> source)
			{
				m_source = source;
			}

			public IEnumerator<TSource> GetEnumerator()
			{
				return new EnumeratorDropIndices(m_source.GetEnumerator());
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}

			public void Dispose()
			{
				(m_source as IDisposable)?.Dispose();
			}
		}

		private class EnumeratorDropIndices : IEnumerator<TSource>, IDisposable, IEnumerator
		{
			private readonly IEnumerator<KeyValuePair<long, TSource>> m_source;

			public TSource Current => m_source.Current.Value;

			object IEnumerator.Current => Current;

			public EnumeratorDropIndices(IEnumerator<KeyValuePair<long, TSource>> source)
			{
				m_source = source;
			}

			public bool MoveNext()
			{
				return m_source.MoveNext();
			}

			public void Dispose()
			{
				m_source.Dispose();
			}

			public void Reset()
			{
				m_source.Reset();
			}
		}

		/// <summary>
		/// Gets whether elements in each partition are yielded in the order of increasing keys.
		/// </summary>
		public bool KeysOrderedInEachPartition { get; private set; }

		/// <summary>
		/// Gets whether elements in an earlier partition always come before elements in a later partition.
		/// </summary>
		/// <remarks>
		/// If <see cref="P:System.Collections.Concurrent.OrderablePartitioner`1.KeysOrderedAcrossPartitions" /> returns true, each element in partition 0 has a
		/// smaller order key than any element in partition 1, each element in partition 1 has a smaller
		/// order key than any element in partition 2, and so on.
		/// </remarks>
		public bool KeysOrderedAcrossPartitions { get; private set; }

		/// <summary>
		/// Gets whether order keys are normalized.
		/// </summary>
		/// <remarks>
		/// If <see cref="P:System.Collections.Concurrent.OrderablePartitioner`1.KeysNormalized" /> returns true, all order keys are distinct integers in the range
		/// [0 .. numberOfElements-1]. If the property returns false, order keys must still be dictinct, but
		/// only their relative order is considered, not their absolute values.
		/// </remarks>
		public bool KeysNormalized { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.Collections.Concurrent.OrderablePartitioner`1" /> class with the
		/// specified constraints on the index keys.
		/// </summary>
		/// <param name="keysOrderedInEachPartition">
		/// Indicates whether the elements in each partition are yielded in the order of
		/// increasing keys.
		/// </param>
		/// <param name="keysOrderedAcrossPartitions">
		/// Indicates whether elements in an earlier partition always come before
		/// elements in a later partition. If true, each element in partition 0 has a smaller order key than
		/// any element in partition 1, each element in partition 1 has a smaller order key than any element
		/// in partition 2, and so on.
		/// </param>
		/// <param name="keysNormalized">
		/// Indicates whether keys are normalized. If true, all order keys are distinct
		/// integers in the range [0 .. numberOfElements-1]. If false, order keys must still be dictinct, but
		/// only their relative order is considered, not their absolute values.
		/// </param>
		protected OrderablePartitioner(bool keysOrderedInEachPartition, bool keysOrderedAcrossPartitions, bool keysNormalized)
		{
			KeysOrderedInEachPartition = keysOrderedInEachPartition;
			KeysOrderedAcrossPartitions = keysOrderedAcrossPartitions;
			KeysNormalized = keysNormalized;
		}

		/// <summary>
		/// Partitions the underlying collection into the specified number of orderable partitions.
		/// </summary>
		/// <remarks>
		/// Each partition is represented as an enumerator over key-value pairs.
		/// The value of the pair is the element itself, and the key is an integer which determines
		/// the relative ordering of this element against other elements in the data source.
		/// </remarks>
		/// <param name="partitionCount">The number of partitions to create.</param>
		/// <returns>A list containing <paramref name="partitionCount" /> enumerators.</returns>
		public abstract IList<IEnumerator<KeyValuePair<long, TSource>>> GetOrderablePartitions(int partitionCount);

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
		/// Each partition is represented as an enumerator over key-value pairs. The value in the pair is the element
		/// itself, and the key is an integer which determines the relative ordering of this element against
		/// other elements.
		/// </para>
		/// <para>
		/// The <see cref="M:System.Collections.Concurrent.OrderablePartitioner`1.GetOrderableDynamicPartitions" /> method is only supported if the <see cref="P:System.Collections.Concurrent.Partitioner`1.SupportsDynamicPartitions">SupportsDynamicPartitions</see>
		/// property returns true.
		/// </para>
		/// </remarks>
		/// <returns>An object that can create partitions over the underlying data source.</returns>
		/// <exception cref="T:System.NotSupportedException">Dynamic partitioning is not supported by this
		/// partitioner.</exception>
		public virtual IEnumerable<KeyValuePair<long, TSource>> GetOrderableDynamicPartitions()
		{
			throw new NotSupportedException(Environment2.GetResourceString("Partitioner_DynamicPartitionsNotSupported"));
		}

		/// <summary>
		/// Partitions the underlying collection into the given number of ordered partitions.
		/// </summary>
		/// <remarks>
		/// The default implementation provides the same behavior as <see cref="M:System.Collections.Concurrent.OrderablePartitioner`1.GetOrderablePartitions(System.Int32)" /> except
		/// that the returned set of partitions does not provide the keys for the elements.
		/// </remarks>
		/// <param name="partitionCount">The number of partitions to create.</param>
		/// <returns>A list containing <paramref name="partitionCount" /> enumerators.</returns>
		public override IList<IEnumerator<TSource>> GetPartitions(int partitionCount)
		{
			IList<IEnumerator<KeyValuePair<long, TSource>>> orderablePartitions = GetOrderablePartitions(partitionCount);
			if (orderablePartitions.Count != partitionCount)
			{
				throw new InvalidOperationException("OrderablePartitioner_GetPartitions_WrongNumberOfPartitions");
			}
			IEnumerator<TSource>[] array = new IEnumerator<TSource>[partitionCount];
			for (int i = 0; i < partitionCount; i++)
			{
				array[i] = new EnumeratorDropIndices(orderablePartitions[i]);
			}
			return array;
		}

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
		/// The default implementation provides the same behavior as <see cref="M:System.Collections.Concurrent.OrderablePartitioner`1.GetOrderableDynamicPartitions" /> except
		/// that the returned set of partitions does not provide the keys for the elements.
		/// </para>
		/// <para>
		/// The <see cref="M:System.Collections.Concurrent.OrderablePartitioner`1.GetDynamicPartitions" /> method is only supported if the <see cref="P:System.Collections.Concurrent.Partitioner`1.SupportsDynamicPartitions" />
		/// property returns true.
		/// </para>
		/// </remarks>
		/// <returns>An object that can create partitions over the underlying data source.</returns>
		/// <exception cref="T:System.NotSupportedException">Dynamic partitioning is not supported by this
		/// partitioner.</exception>
		public override IEnumerable<TSource> GetDynamicPartitions()
		{
			IEnumerable<KeyValuePair<long, TSource>> orderableDynamicPartitions = GetOrderableDynamicPartitions();
			return new EnumerableDropIndices(orderableDynamicPartitions);
		}
	}
}
