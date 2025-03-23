using System.Collections.Generic;

namespace System.Linq.Parallel
{
	/// <summary>
	/// A partitioned stream just partitions some data source using an extensible 
	/// partitioning algorithm and exposes a set of N enumerators that are consumed by
	/// their ordinal index [0..N). It is used to build up a set of streaming computations.
	/// At instantiation time, the actual data source to be partitioned is supplied; and
	/// then the caller will layer on top additional enumerators to represent phases in the
	/// computation. Eventually, a merge can then schedule enumeration of all of the
	/// individual partitions in parallel by obtaining references to the individual
	/// partition streams.
	///
	/// This type has a set of subclasses which implement different partitioning algorithms,
	/// allowing us to easily plug in different partitioning techniques as needed. The type
	/// supports wrapping IEnumerables and IEnumerators alike, with some preference for the
	/// former as many partitioning algorithms are more intelligent for certain data types.
	/// </summary>
	/// <typeparam name="TElement"></typeparam>
	/// <typeparam name="TKey"></typeparam>
	internal class PartitionedStream<TElement, TKey>
	{
		protected QueryOperatorEnumerator<TElement, TKey>[] m_partitions;

		private readonly IComparer<TKey> m_keyComparer;

		private readonly OrdinalIndexState m_indexState;

		internal QueryOperatorEnumerator<TElement, TKey> this[int index]
		{
			get
			{
				return m_partitions[index];
			}
			set
			{
				m_partitions[index] = value;
			}
		}

		public int PartitionCount => m_partitions.Length;

		internal IComparer<TKey> KeyComparer => m_keyComparer;

		internal OrdinalIndexState OrdinalIndexState => m_indexState;

		internal PartitionedStream(int partitionCount, IComparer<TKey> keyComparer, OrdinalIndexState indexState)
		{
			m_partitions = new QueryOperatorEnumerator<TElement, TKey>[partitionCount];
			m_keyComparer = keyComparer;
			m_indexState = indexState;
		}
	}
}
