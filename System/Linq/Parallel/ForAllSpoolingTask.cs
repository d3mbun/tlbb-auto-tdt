namespace System.Linq.Parallel
{
	/// <summary>
	/// A spooling task handles marshaling data from a producer to a consumer. It's given
	/// a single enumerator object that contains all of the production algorithms, a single
	/// destination channel from which consumers draw results, and (optionally) a
	/// synchronization primitive using which to notify asynchronous consumers.
	/// </summary>
	/// <typeparam name="TInputOutput"></typeparam>
	/// <typeparam name="TIgnoreKey"></typeparam>
	internal class ForAllSpoolingTask<TInputOutput, TIgnoreKey> : SpoolingTaskBase
	{
		private QueryOperatorEnumerator<TInputOutput, TIgnoreKey> m_source;

		internal ForAllSpoolingTask(int taskIndex, QueryTaskGroupState groupState, QueryOperatorEnumerator<TInputOutput, TIgnoreKey> source)
			: base(taskIndex, groupState)
		{
			m_source = source;
		}

		protected override void SpoolingWork()
		{
			TInputOutput currentElement = default(TInputOutput);
			TIgnoreKey currentKey = default(TIgnoreKey);
			while (m_source.MoveNext(ref currentElement, ref currentKey))
			{
			}
		}

		protected override void SpoolingFinally()
		{
			base.SpoolingFinally();
			m_source.Dispose();
		}
	}
}
