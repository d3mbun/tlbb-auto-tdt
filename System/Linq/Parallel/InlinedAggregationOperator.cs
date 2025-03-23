using System.Collections.Generic;
using System.Threading;

namespace System.Linq.Parallel
{
	/// <summary>
	/// This class is common to all of the "inlined" versions of various aggregations.  The
	/// inlined operators ensure that real MSIL instructions are used to perform elementary
	/// operations versus general purpose delegate-based binary operators.  For obvious reasons
	/// this is a quite bit more efficient, although it does lead to a fair bit of unfortunate
	/// code duplication. 
	/// </summary>
	/// <typeparam name="TSource"></typeparam>
	/// <typeparam name="TIntermediate"></typeparam>
	/// <typeparam name="TResult"></typeparam>
	internal abstract class InlinedAggregationOperator<TSource, TIntermediate, TResult> : UnaryQueryOperator<TSource, TIntermediate>
	{
		internal override bool LimitsParallelism => false;

		internal InlinedAggregationOperator(IEnumerable<TSource> child)
			: base(child)
		{
		}

		internal TResult Aggregate()
		{
			Exception singularExceptionToThrow = null;
			TResult result;
			try
			{
				result = InternalAggregate(ref singularExceptionToThrow);
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				if (!(ex2 is AggregateException))
				{
					OperationCanceledException2 operationCanceledException = ex2 as OperationCanceledException2;
					if (operationCanceledException != null && operationCanceledException.CancellationToken == base.SpecifiedQuerySettings.CancellationState.ExternalCancellationToken && base.SpecifiedQuerySettings.CancellationState.ExternalCancellationToken.IsCancellationRequested)
					{
						throw;
					}
					throw new AggregateException(ex2);
				}
				throw;
			}
			if (singularExceptionToThrow != null)
			{
				throw singularExceptionToThrow;
			}
			return result;
		}

		protected abstract TResult InternalAggregate(ref Exception singularExceptionToThrow);

		internal override QueryResults<TIntermediate> Open(QuerySettings settings, bool preferStriping)
		{
			QueryResults<TSource> childQueryResults = base.Child.Open(settings, preferStriping);
			return new UnaryQueryOperatorResults(childQueryResults, this, settings, preferStriping);
		}

		internal override void WrapPartitionedStream<TKey>(PartitionedStream<TSource, TKey> inputStream, IPartitionedStreamRecipient<TIntermediate> recipient, bool preferStriping, QuerySettings settings)
		{
			int partitionCount = inputStream.PartitionCount;
			PartitionedStream<TIntermediate, int> partitionedStream = new PartitionedStream<TIntermediate, int>(partitionCount, Util.GetDefaultComparer<int>(), OrdinalIndexState.Correct);
			for (int i = 0; i < partitionCount; i++)
			{
				partitionedStream[i] = CreateEnumerator(i, partitionCount, inputStream[i], null, settings.CancellationState.MergedCancellationToken);
			}
			recipient.Receive(partitionedStream);
		}

		protected abstract QueryOperatorEnumerator<TIntermediate, int> CreateEnumerator<TKey>(int index, int count, QueryOperatorEnumerator<TSource, TKey> source, object sharedData, CancellationToken cancellationToken);

		internal override IEnumerable<TIntermediate> AsSequentialQuery(CancellationToken token)
		{
			throw new NotSupportedException();
		}
	}
}
