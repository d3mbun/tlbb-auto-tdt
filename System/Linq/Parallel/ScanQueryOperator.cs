using System.Collections.Generic;
using System.Threading;

namespace System.Linq.Parallel
{
	/// <summary>
	/// A scan is just a simple operator that is positioned directly on top of some
	/// real data source. It's really just a place holder used during execution and
	/// analysis -- it should never actually get opened.
	/// </summary>
	/// <typeparam name="TElement"></typeparam>
	internal sealed class ScanQueryOperator<TElement> : QueryOperator<TElement>
	{
		private class ScanEnumerableQueryOperatorResults : QueryResults<TElement>
		{
			private IEnumerable<TElement> m_data;

			private QuerySettings m_settings;

			internal ScanEnumerableQueryOperatorResults(IEnumerable<TElement> data, QuerySettings settings)
			{
				m_data = data;
				m_settings = settings;
			}

			internal override void GivePartitionedStream(IPartitionedStreamRecipient<TElement> recipient)
			{
				PartitionedStream<TElement, int> partitionedStream = ExchangeUtilities.PartitionDataSource(m_data, m_settings.DegreeOfParallelism.Value, useStriping: false);
				recipient.Receive(partitionedStream);
			}
		}

		private readonly IEnumerable<TElement> m_data;

		public IEnumerable<TElement> Data => m_data;

		internal override OrdinalIndexState OrdinalIndexState
		{
			get
			{
				if (!(m_data is IList<TElement>))
				{
					return OrdinalIndexState.Correct;
				}
				return OrdinalIndexState.Indexible;
			}
		}

		internal override bool LimitsParallelism => false;

		internal ScanQueryOperator(IEnumerable<TElement> data)
			: base(isOrdered: false, QuerySettings.Empty)
		{
			ParallelEnumerableWrapper<TElement> parallelEnumerableWrapper = data as ParallelEnumerableWrapper<TElement>;
			if (parallelEnumerableWrapper != null)
			{
				data = parallelEnumerableWrapper.WrappedEnumerable;
			}
			m_data = data;
		}

		internal override QueryResults<TElement> Open(QuerySettings settings, bool preferStriping)
		{
			IList<TElement> list = m_data as IList<TElement>;
			if (list != null)
			{
				return new ListQueryResults<TElement>(list, settings.DegreeOfParallelism.GetValueOrDefault(), preferStriping);
			}
			return new ScanEnumerableQueryOperatorResults(m_data, settings);
		}

		internal override IEnumerator<TElement> GetEnumerator(ParallelMergeOptions? mergeOptions, bool suppressOrderPreservation)
		{
			return m_data.GetEnumerator();
		}

		internal override IEnumerable<TElement> AsSequentialQuery(CancellationToken token)
		{
			return m_data;
		}
	}
}
