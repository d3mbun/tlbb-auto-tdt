using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.Linq.Parallel
{
	/// <summary>
	/// The default merge helper uses a set of straightforward algorithms for output
	/// merging. Namely, for synchronous merges, the input data is yielded from the
	/// input data streams in "depth first" left-to-right order. For asynchronous merges,
	/// on the other hand, we use a biased choice algorithm to favor input channels in
	/// a "fair" way. No order preservation is carried out by this helper. 
	/// </summary>
	/// <typeparam name="TInputOutput"></typeparam>
	/// <typeparam name="TIgnoreKey"></typeparam>
	internal class DefaultMergeHelper<TInputOutput, TIgnoreKey> : IMergeHelper<TInputOutput>
	{
		private QueryTaskGroupState m_taskGroupState;

		private PartitionedStream<TInputOutput, TIgnoreKey> m_partitions;

		private AsynchronousChannel<TInputOutput>[] m_asyncChannels;

		private SynchronousChannel<TInputOutput>[] m_syncChannels;

		private IEnumerator<TInputOutput> m_channelEnumerator;

		private TaskScheduler m_taskScheduler;

		private bool m_ignoreOutput;

		internal DefaultMergeHelper(PartitionedStream<TInputOutput, TIgnoreKey> partitions, bool ignoreOutput, ParallelMergeOptions options, TaskScheduler taskScheduler, CancellationState cancellationState, int queryId)
		{
			m_taskGroupState = new QueryTaskGroupState(cancellationState, queryId);
			m_partitions = partitions;
			m_taskScheduler = taskScheduler;
			m_ignoreOutput = ignoreOutput;
			if (ignoreOutput)
			{
				return;
			}
			if (options != ParallelMergeOptions.FullyBuffered)
			{
				if (partitions.PartitionCount > 1)
				{
					m_asyncChannels = MergeExecutor<TInputOutput>.MakeAsynchronousChannels(partitions.PartitionCount, options, cancellationState.MergedCancellationToken);
					m_channelEnumerator = new AsynchronousChannelMergeEnumerator<TInputOutput>(m_taskGroupState, m_asyncChannels);
				}
				else
				{
					m_channelEnumerator = ExceptionAggregator.WrapQueryEnumerator(partitions[0], m_taskGroupState.CancellationState).GetEnumerator();
				}
			}
			else
			{
				m_syncChannels = MergeExecutor<TInputOutput>.MakeSynchronousChannels(partitions.PartitionCount);
				m_channelEnumerator = new SynchronousChannelMergeEnumerator<TInputOutput>(m_taskGroupState, m_syncChannels);
			}
		}

		void IMergeHelper<TInputOutput>.Execute()
		{
			if (m_asyncChannels != null)
			{
				SpoolingTask.SpoolPipeline(m_taskGroupState, m_partitions, m_asyncChannels, m_taskScheduler);
			}
			else if (m_syncChannels != null)
			{
				SpoolingTask.SpoolStopAndGo(m_taskGroupState, m_partitions, m_syncChannels, m_taskScheduler);
			}
			else if (m_ignoreOutput)
			{
				SpoolingTask.SpoolForAll(m_taskGroupState, m_partitions, m_taskScheduler);
			}
		}

		IEnumerator<TInputOutput> IMergeHelper<TInputOutput>.GetEnumerator()
		{
			return m_channelEnumerator;
		}

		public TInputOutput[] GetResultsAsArray()
		{
			if (m_syncChannels != null)
			{
				int num = 0;
				for (int i = 0; i < m_syncChannels.Length; i++)
				{
					num += m_syncChannels[i].Count;
				}
				TInputOutput[] array = new TInputOutput[num];
				int num2 = 0;
				for (int j = 0; j < m_syncChannels.Length; j++)
				{
					m_syncChannels[j].CopyTo(array, num2);
					num2 += m_syncChannels[j].Count;
				}
				return array;
			}
			List<TInputOutput> list = new List<TInputOutput>();
			foreach (TInputOutput item in (IMergeHelper<TInputOutput>)this)
			{
				list.Add(item);
			}
			return list.ToArray();
		}
	}
}
