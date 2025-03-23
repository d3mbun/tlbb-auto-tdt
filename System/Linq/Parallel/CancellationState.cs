using System.Threading;

namespace System.Linq.Parallel
{
	internal class CancellationState
	{
		/// <summary>
		/// Poll frequency (number of loops per cancellation check) for situations where per-1-loop testing is too high an overhead. 
		/// </summary>
		internal const int POLL_INTERVAL = 63;

		internal CancellationTokenSource InternalCancellationTokenSource;

		internal CancellationToken ExternalCancellationToken;

		internal CancellationTokenSource MergedCancellationTokenSource;

		internal Shared<bool> TopLevelDisposedFlag;

		internal CancellationToken MergedCancellationToken
		{
			get
			{
				if (MergedCancellationTokenSource != null)
				{
					return MergedCancellationTokenSource.Token;
				}
				return new CancellationToken(canceled: false);
			}
		}

		internal CancellationState(CancellationToken externalCancellationToken)
		{
			ExternalCancellationToken = externalCancellationToken;
			TopLevelDisposedFlag = new Shared<bool>(value: false);
		}

		/// <summary>
		/// Throws an OCE if the merged token has been canceled.
		/// </summary>
		/// <param name="token">A token to check for cancelation.</param>
		internal static void ThrowIfCanceled(CancellationToken token)
		{
			if (token.IsCancellationRequested)
			{
				throw new OperationCanceledException2(token);
			}
		}

		internal static void ThrowWithStandardMessageIfCanceled(CancellationToken externalCancellationToken)
		{
			if (externalCancellationToken.IsCancellationRequested)
			{
				string message = "PLINQ_ExternalCancellationRequested";
				throw new OperationCanceledException2(message, externalCancellationToken);
			}
		}
	}
}
