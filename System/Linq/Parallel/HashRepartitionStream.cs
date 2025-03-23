using System.Collections.Generic;

namespace System.Linq.Parallel
{
	/// <summary>
	/// A repartitioning stream must take input data that has already been partitioned and
	/// redistribute its contents based on a new partitioning algorithm. This is accomplished
	/// by making each partition p responsible for redistributing its input data to the
	/// correct destination partition. Some input elements may remain in p, but many will now
	/// belong to a different partition and will need to move. This requires a great deal of
	/// synchronization, but allows threads to repartition data incrementally and in parallel.
	/// Each partition will "pull" data on-demand instead of partitions "pushing" data, which
	/// allows us to reduce some amount of synchronization overhead.
	///
	/// We currently only offer one form of reparitioning via hashing.  This used to be an
	/// abstract base class, but we have eliminated that to get rid of some virtual calls on
	/// hot code paths.  Uses a key selection algorithm with mod'ding to determine destination.
	///
	/// @TODO: @BUG#519: consider adding a bound to the buffers. Unfortunately this can quite easily
	///     lead to deadlock when multiple repartitions are involved. Need a solution.
	/// @TODO: @BUG#504: consider amortizing synchronization overhead by enqueueing/dequeueing in chunks
	///     rather than single elements. Also need to be careful not to introduce deadlock.
	/// </summary>
	/// <typeparam name="TInputOutput"></typeparam>
	/// <typeparam name="THashKey"></typeparam>
	/// <typeparam name="TOrderKey"></typeparam>
	internal abstract class HashRepartitionStream<TInputOutput, THashKey, TOrderKey> : PartitionedStream<Pair<TInputOutput, THashKey>, TOrderKey>
	{
		private const int NULL_ELEMENT_HASH_CODE = 0;

		private readonly IEqualityComparer<THashKey> m_keyComparer;

		private readonly IEqualityComparer<TInputOutput> m_elementComparer;

		private readonly int m_distributionMod;

		internal HashRepartitionStream(int partitionsCount, IComparer<TOrderKey> orderKeyComparer, IEqualityComparer<THashKey> hashKeyComparer, IEqualityComparer<TInputOutput> elementComparer)
			: base(partitionsCount, orderKeyComparer, OrdinalIndexState.Shuffled)
		{
			m_keyComparer = hashKeyComparer;
			m_elementComparer = elementComparer;
			checked
			{
				for (m_distributionMod = 503; m_distributionMod < partitionsCount; m_distributionMod *= 2)
				{
				}
			}
		}

		internal int GetHashCode(TInputOutput element)
		{
			return (0x7FFFFFFF & ((m_elementComparer != null) ? m_elementComparer.GetHashCode(element) : (element?.GetHashCode() ?? 0))) % m_distributionMod;
		}

		internal int GetHashCode(THashKey key)
		{
			return (0x7FFFFFFF & ((m_keyComparer != null) ? m_keyComparer.GetHashCode(key) : (key?.GetHashCode() ?? 0))) % m_distributionMod;
		}
	}
}
