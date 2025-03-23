using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.Linq.Parallel
{
	/// <summary>
	/// The order preserving merge helper guarantees the output stream is in a specific order. This is done
	/// by comparing keys from a set of already-sorted input partitions, and coalescing output data using
	/// incremental key comparisons.
	/// </summary>
	/// <typeparam name="TInputOutput"></typeparam>
	/// <typeparam name="TKey"></typeparam>
	internal class OrderPreservingMergeHelper<TInputOutput, TKey> : IMergeHelper<TInputOutput>
	{
		private QueryTaskGroupState m_taskGroupState;

		private PartitionedStream<TInputOutput, TKey> m_partitions;

		private Shared<TInputOutput[]> m_results;

		private TaskScheduler m_taskScheduler;

		internal OrderPreservingMergeHelper(PartitionedStream<TInputOutput, TKey> partitions, TaskScheduler taskScheduler, CancellationState cancellationState, int queryId)
		{
			m_taskGroupState = new QueryTaskGroupState(cancellationState, queryId);
			m_partitions = partitions;
			m_results = new Shared<TInputOutput[]>(null);
			m_taskScheduler = taskScheduler;
		}

		void IMergeHelper<TInputOutput>.Execute()
		{
			OrderPreservingSpoolingTask<TInputOutput, TKey>.Spool(m_taskGroupState, m_partitions, m_results, m_taskScheduler);
		}

		IEnumerator<TInputOutput> IMergeHelper<TInputOutput>.GetEnumerator()
		{
			return ((IEnumerable<TInputOutput>)m_results.Value).GetEnumerator();
		}

		public TInputOutput[] GetResultsAsArray()
		{
			return m_results.Value;
		}
	}
}
