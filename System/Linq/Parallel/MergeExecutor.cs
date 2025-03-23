using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq.Parallel
{
	/// <summary>
	/// Drives execution of an actual merge operation, including creating channel data
	/// structures and scheduling parallel work as appropriate. The algorithms used
	/// internally are parameterized based on the type of data in the partitions; e.g.
	/// if an order preserved stream is found, the merge will automatically use an
	/// order preserving merge, and so forth. 
	/// </summary>
	/// <typeparam name="TInputOutput"></typeparam>
	internal class MergeExecutor<TInputOutput> : IEnumerable<TInputOutput>, IEnumerable
	{
		private IMergeHelper<TInputOutput> m_mergeHelper;

		private MergeExecutor()
		{
		}

		internal static MergeExecutor<TInputOutput> Execute<TKey>(PartitionedStream<TInputOutput, TKey> partitions, bool ignoreOutput, ParallelMergeOptions options, TaskScheduler taskScheduler, bool isOrdered, CancellationState cancellationState, int queryId)
		{
			MergeExecutor<TInputOutput> mergeExecutor = new MergeExecutor<TInputOutput>();
			if (isOrdered && !ignoreOutput)
			{
				if (options != ParallelMergeOptions.FullyBuffered && !partitions.OrdinalIndexState.IsWorseThan(OrdinalIndexState.Increasing))
				{
					bool autoBuffered = options == ParallelMergeOptions.AutoBuffered;
					if (partitions.PartitionCount > 1)
					{
						mergeExecutor.m_mergeHelper = new OrderPreservingPipeliningMergeHelper<TInputOutput>((PartitionedStream<TInputOutput, int>)(object)partitions, taskScheduler, cancellationState, autoBuffered, queryId);
					}
					else
					{
						mergeExecutor.m_mergeHelper = new DefaultMergeHelper<TInputOutput, TKey>(partitions, ignoreOutput: false, options, taskScheduler, cancellationState, queryId);
					}
				}
				else
				{
					mergeExecutor.m_mergeHelper = new OrderPreservingMergeHelper<TInputOutput, TKey>(partitions, taskScheduler, cancellationState, queryId);
				}
			}
			else
			{
				mergeExecutor.m_mergeHelper = new DefaultMergeHelper<TInputOutput, TKey>(partitions, ignoreOutput, options, taskScheduler, cancellationState, queryId);
			}
			mergeExecutor.Execute();
			return mergeExecutor;
		}

		private void Execute()
		{
			m_mergeHelper.Execute();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<TInputOutput>)this).GetEnumerator();
		}

		public IEnumerator<TInputOutput> GetEnumerator()
		{
			return m_mergeHelper.GetEnumerator();
		}

		internal TInputOutput[] GetResultsAsArray()
		{
			return m_mergeHelper.GetResultsAsArray();
		}

		internal static AsynchronousChannel<TInputOutput>[] MakeAsynchronousChannels(int partitionCount, ParallelMergeOptions options, CancellationToken cancellationToken)
		{
			AsynchronousChannel<TInputOutput>[] array = new AsynchronousChannel<TInputOutput>[partitionCount];
			int chunkSize = 0;
			if (options == ParallelMergeOptions.NotBuffered)
			{
				chunkSize = 1;
			}
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new AsynchronousChannel<TInputOutput>(chunkSize, cancellationToken);
			}
			return array;
		}

		internal static SynchronousChannel<TInputOutput>[] MakeSynchronousChannels(int partitionCount)
		{
			SynchronousChannel<TInputOutput>[] array = new SynchronousChannel<TInputOutput>[partitionCount];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new SynchronousChannel<TInputOutput>();
			}
			return array;
		}
	}
}
