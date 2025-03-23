namespace System.Linq.Parallel
{
	/// <summary>
	/// A spooling task handles marshaling data from a producer to a consumer. It simply
	/// takes data from a producer and hands it off to a consumer. This class is the base
	/// class from which other concrete spooling tasks derive, encapsulating some common
	/// logic (such as capturing exceptions).
	/// </summary>
	internal abstract class SpoolingTaskBase : QueryTask
	{
		protected SpoolingTaskBase(int taskIndex, QueryTaskGroupState groupState)
			: base(taskIndex, groupState)
		{
		}

		protected override void Work()
		{
			try
			{
				SpoolingWork();
			}
			catch (Exception ex)
			{
				OperationCanceledException2 operationCanceledException = ex as OperationCanceledException2;
				if (operationCanceledException == null || !(operationCanceledException.CancellationToken == m_groupState.CancellationState.MergedCancellationToken) || !m_groupState.CancellationState.MergedCancellationToken.IsCancellationRequested)
				{
					m_groupState.CancellationState.InternalCancellationTokenSource.Cancel();
					throw;
				}
			}
			finally
			{
				SpoolingFinally();
			}
		}

		protected abstract void SpoolingWork();

		protected virtual void SpoolingFinally()
		{
		}
	}
}
