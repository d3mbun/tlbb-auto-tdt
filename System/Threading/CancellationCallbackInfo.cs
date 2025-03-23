namespace System.Threading
{
	/// <summary>
	/// A helper class for collating the various bits of information required to execute 
	/// cancellation callbacks.
	/// </summary>
	internal class CancellationCallbackInfo
	{
		internal readonly Action<object> Callback;

		internal readonly object StateForCallback;

		internal readonly SynchronizationContext TargetSyncContext;

		internal readonly ExecutionContext TargetExecutionContext;

		internal readonly CancellationTokenSource CancellationTokenSource;

		internal CancellationCallbackInfo(Action<object> callback, object stateForCallback, SynchronizationContext targetSyncContext, ExecutionContext targetExecutionContext, CancellationTokenSource cancellationTokenSource)
		{
			Callback = callback;
			StateForCallback = stateForCallback;
			TargetSyncContext = targetSyncContext;
			TargetExecutionContext = targetExecutionContext;
			CancellationTokenSource = cancellationTokenSource;
		}

		/// <summary>
		/// InternalExecuteCallbackSynchronously_GeneralPath
		/// This will be called on the target synchronization context, however, we still need to restore the required execution context
		/// </summary>
		[SecuritySafeCritical]
		internal void ExecuteCallback()
		{
			if (TargetExecutionContext != null)
			{
				ExecutionContext.Run(TargetExecutionContext, ExecutionContextCallback, this);
			}
			else
			{
				ExecutionContextCallback(this);
			}
		}

		private static void ExecutionContextCallback(object obj)
		{
			CancellationCallbackInfo cancellationCallbackInfo = obj as CancellationCallbackInfo;
			cancellationCallbackInfo.Callback(cancellationCallbackInfo.StateForCallback);
		}
	}
}
