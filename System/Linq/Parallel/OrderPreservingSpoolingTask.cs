using System.Threading.Tasks;

namespace System.Linq.Parallel
{
	/// <summary>
	/// A spooling task handles marshaling data from a producer to a consumer. It's given
	/// a single enumerator object that contains all of the production algorithms, a single
	/// destination channel from which consumers draw results, and (optionally) a
	/// synchronization primitive using which to notify asynchronous consumers. This
	/// particular task variant preserves sort order in the final data.
	/// </summary>
	/// <typeparam name="TInputOutput"></typeparam>
	/// <typeparam name="TKey"></typeparam>
	internal class OrderPreservingSpoolingTask<TInputOutput, TKey> : SpoolingTaskBase
	{
		private Shared<TInputOutput[]> m_results;

		private SortHelper<TInputOutput> m_sortHelper;

		private OrderPreservingSpoolingTask(int taskIndex, QueryTaskGroupState groupState, Shared<TInputOutput[]> results, SortHelper<TInputOutput> sortHelper)
			: base(taskIndex, groupState)
		{
			m_results = results;
			m_sortHelper = sortHelper;
		}

		internal static void Spool(QueryTaskGroupState groupState, PartitionedStream<TInputOutput, TKey> partitions, Shared<TInputOutput[]> results, TaskScheduler taskScheduler)
		{
			int maxToRunInParallel = partitions.PartitionCount - 1;
			SortHelper<TInputOutput, TKey>[] sortHelpers = SortHelper<TInputOutput, TKey>.GenerateSortHelpers(partitions, groupState);
			Task task = new Task(delegate
			{
				for (int j = 0; j < maxToRunInParallel; j++)
				{
					QueryTask queryTask = new OrderPreservingSpoolingTask<TInputOutput, TKey>(j, groupState, results, sortHelpers[j]);
					queryTask.RunAsynchronously(taskScheduler);
				}
				QueryTask queryTask2 = new OrderPreservingSpoolingTask<TInputOutput, TKey>(maxToRunInParallel, groupState, results, sortHelpers[maxToRunInParallel]);
				queryTask2.RunSynchronously(taskScheduler);
			});
			groupState.QueryBegin(task);
			task.RunSynchronously(taskScheduler);
			for (int i = 0; i < sortHelpers.Length; i++)
			{
				sortHelpers[i].Dispose();
			}
			groupState.QueryEnd(userInitiatedDispose: false);
		}

		protected override void SpoolingWork()
		{
			TInputOutput[] value = m_sortHelper.Sort();
			if (!m_groupState.CancellationState.MergedCancellationToken.IsCancellationRequested && m_taskIndex == 0)
			{
				m_results.Value = value;
			}
		}
	}
}
