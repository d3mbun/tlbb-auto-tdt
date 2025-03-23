using System.Collections.Generic;
using System.Threading;

namespace System.Linq.Parallel
{
	/// <summary>
	/// SelectMany is effectively a nested loops join. It is given two data sources, an
	/// outer and an inner -- actually, the inner is sometimes calculated by invoking a
	/// function for each outer element -- and we walk the outer, walking the entire
	/// inner enumerator for each outer element. There is an optional result selector
	/// function which can transform the output before yielding it as a result element.
	///
	/// Notes:
	///     Although select many takes two enumerable objects as input, it appears to the
	///     query analysis infrastructure as a unary operator. That's because it works a
	///     little differently than the other binary operators: it has to re-open the right
	///     child every time an outer element is walked. The right child is NOT partitioned. 
	/// </summary>
	/// <typeparam name="TLeftInput"></typeparam>
	/// <typeparam name="TRightInput"></typeparam>
	/// <typeparam name="TOutput"></typeparam>
	internal sealed class SelectManyQueryOperator<TLeftInput, TRightInput, TOutput> : UnaryQueryOperator<TLeftInput, TOutput>
	{
		private class IndexedSelectManyQueryOperatorEnumerator : QueryOperatorEnumerator<TOutput, Pair<int, int>>
		{
			private class Mutables
			{
				internal int m_currentRightSourceIndex = -1;

				internal TLeftInput m_currentLeftElement;

				internal int m_currentLeftSourceIndex;

				internal int m_lhsCount;
			}

			private readonly QueryOperatorEnumerator<TLeftInput, int> m_leftSource;

			private readonly SelectManyQueryOperator<TLeftInput, TRightInput, TOutput> m_selectManyOperator;

			private IEnumerator<TRightInput> m_currentRightSource;

			private IEnumerator<TOutput> m_currentRightSourceAsOutput;

			private Mutables m_mutables;

			private readonly CancellationToken m_cancellationToken;

			internal IndexedSelectManyQueryOperatorEnumerator(QueryOperatorEnumerator<TLeftInput, int> leftSource, SelectManyQueryOperator<TLeftInput, TRightInput, TOutput> selectManyOperator, CancellationToken cancellationToken)
			{
				m_leftSource = leftSource;
				m_selectManyOperator = selectManyOperator;
				m_cancellationToken = cancellationToken;
			}

			internal override bool MoveNext(ref TOutput currentElement, ref Pair<int, int> currentKey)
			{
				while (true)
				{
					if (m_currentRightSource == null)
					{
						m_mutables = new Mutables();
						if ((m_mutables.m_lhsCount++ & 0x3F) == 0)
						{
							CancellationState.ThrowIfCanceled(m_cancellationToken);
						}
						if (!m_leftSource.MoveNext(ref m_mutables.m_currentLeftElement, ref m_mutables.m_currentLeftSourceIndex))
						{
							return false;
						}
						IEnumerable<TRightInput> enumerable = m_selectManyOperator.m_indexedRightChildSelector(m_mutables.m_currentLeftElement, m_mutables.m_currentLeftSourceIndex);
						m_currentRightSource = enumerable.GetEnumerator();
						if (m_selectManyOperator.m_resultSelector == null)
						{
							m_currentRightSourceAsOutput = (IEnumerator<TOutput>)m_currentRightSource;
						}
					}
					if (m_currentRightSource.MoveNext())
					{
						break;
					}
					m_currentRightSource.Dispose();
					m_currentRightSource = null;
					m_currentRightSourceAsOutput = null;
				}
				m_mutables.m_currentRightSourceIndex++;
				if (m_selectManyOperator.m_resultSelector != null)
				{
					currentElement = m_selectManyOperator.m_resultSelector(m_mutables.m_currentLeftElement, m_currentRightSource.Current);
				}
				else
				{
					currentElement = m_currentRightSourceAsOutput.Current;
				}
				currentKey = new Pair<int, int>(m_mutables.m_currentLeftSourceIndex, m_mutables.m_currentRightSourceIndex);
				return true;
			}

			protected override void Dispose(bool disposing)
			{
				m_leftSource.Dispose();
				if (m_currentRightSource != null)
				{
					m_currentRightSource.Dispose();
				}
			}
		}

		private class SelectManyQueryOperatorEnumerator<TLeftKey> : QueryOperatorEnumerator<TOutput, Pair<TLeftKey, int>>
		{
			private class Mutables
			{
				internal int m_currentRightSourceIndex = -1;

				internal TLeftInput m_currentLeftElement;

				internal TLeftKey m_currentLeftKey;

				internal int m_lhsCount;
			}

			private readonly QueryOperatorEnumerator<TLeftInput, TLeftKey> m_leftSource;

			private readonly SelectManyQueryOperator<TLeftInput, TRightInput, TOutput> m_selectManyOperator;

			private IEnumerator<TRightInput> m_currentRightSource;

			private IEnumerator<TOutput> m_currentRightSourceAsOutput;

			private Mutables m_mutables;

			private readonly CancellationToken m_cancellationToken;

			internal SelectManyQueryOperatorEnumerator(QueryOperatorEnumerator<TLeftInput, TLeftKey> leftSource, SelectManyQueryOperator<TLeftInput, TRightInput, TOutput> selectManyOperator, CancellationToken cancellationToken)
			{
				m_leftSource = leftSource;
				m_selectManyOperator = selectManyOperator;
				m_cancellationToken = cancellationToken;
			}

			internal override bool MoveNext(ref TOutput currentElement, ref Pair<TLeftKey, int> currentKey)
			{
				while (true)
				{
					if (m_currentRightSource == null)
					{
						m_mutables = new Mutables();
						if ((m_mutables.m_lhsCount++ & 0x3F) == 0)
						{
							CancellationState.ThrowIfCanceled(m_cancellationToken);
						}
						if (!m_leftSource.MoveNext(ref m_mutables.m_currentLeftElement, ref m_mutables.m_currentLeftKey))
						{
							return false;
						}
						IEnumerable<TRightInput> enumerable = m_selectManyOperator.m_rightChildSelector(m_mutables.m_currentLeftElement);
						m_currentRightSource = enumerable.GetEnumerator();
						if (m_selectManyOperator.m_resultSelector == null)
						{
							m_currentRightSourceAsOutput = (IEnumerator<TOutput>)m_currentRightSource;
						}
					}
					if (m_currentRightSource.MoveNext())
					{
						break;
					}
					m_currentRightSource.Dispose();
					m_currentRightSource = null;
					m_currentRightSourceAsOutput = null;
				}
				m_mutables.m_currentRightSourceIndex++;
				if (m_selectManyOperator.m_resultSelector != null)
				{
					currentElement = m_selectManyOperator.m_resultSelector(m_mutables.m_currentLeftElement, m_currentRightSource.Current);
				}
				else
				{
					currentElement = m_currentRightSourceAsOutput.Current;
				}
				currentKey = new Pair<TLeftKey, int>(m_mutables.m_currentLeftKey, m_mutables.m_currentRightSourceIndex);
				return true;
			}

			protected override void Dispose(bool disposing)
			{
				m_leftSource.Dispose();
				if (m_currentRightSource != null)
				{
					m_currentRightSource.Dispose();
				}
			}
		}

		private readonly Func<TLeftInput, IEnumerable<TRightInput>> m_rightChildSelector;

		private readonly Func<TLeftInput, int, IEnumerable<TRightInput>> m_indexedRightChildSelector;

		private readonly Func<TLeftInput, TRightInput, TOutput> m_resultSelector;

		private bool m_prematureMerge;

		internal override bool LimitsParallelism => m_prematureMerge;

		internal SelectManyQueryOperator(IEnumerable<TLeftInput> leftChild, Func<TLeftInput, IEnumerable<TRightInput>> rightChildSelector, Func<TLeftInput, int, IEnumerable<TRightInput>> indexedRightChildSelector, Func<TLeftInput, TRightInput, TOutput> resultSelector)
			: base(leftChild)
		{
			m_rightChildSelector = rightChildSelector;
			m_indexedRightChildSelector = indexedRightChildSelector;
			m_resultSelector = resultSelector;
			m_outputOrdered = base.Child.OutputOrdered || indexedRightChildSelector != null;
			InitOrderIndex();
		}

		private void InitOrderIndex()
		{
			if (m_indexedRightChildSelector != null)
			{
				m_prematureMerge = base.Child.OrdinalIndexState.IsWorseThan(OrdinalIndexState.Correct);
			}
			else if (base.OutputOrdered)
			{
				m_prematureMerge = base.Child.OrdinalIndexState.IsWorseThan(OrdinalIndexState.Increasing);
			}
			SetOrdinalIndexState(OrdinalIndexState.Shuffled);
		}

		internal override void WrapPartitionedStream<TLeftKey>(PartitionedStream<TLeftInput, TLeftKey> inputStream, IPartitionedStreamRecipient<TOutput> recipient, bool preferStriping, QuerySettings settings)
		{
			int partitionCount = inputStream.PartitionCount;
			if (m_indexedRightChildSelector != null)
			{
				PartitionedStream<TLeftInput, int> inputStream2;
				if (m_prematureMerge)
				{
					ListQueryResults<TLeftInput> listQueryResults = QueryOperator<TLeftInput>.ExecuteAndCollectResults(inputStream, partitionCount, base.OutputOrdered, preferStriping, settings);
					inputStream2 = listQueryResults.GetPartitionedStream();
				}
				else
				{
					inputStream2 = (PartitionedStream<TLeftInput, int>)(object)inputStream;
				}
				WrapPartitionedStreamIndexed(inputStream2, recipient, settings);
			}
			else if (m_prematureMerge)
			{
				PartitionedStream<TLeftInput, int> partitionedStream = QueryOperator<TLeftInput>.ExecuteAndCollectResults(inputStream, partitionCount, base.OutputOrdered, preferStriping, settings).GetPartitionedStream();
				WrapPartitionedStreamNotIndexed(partitionedStream, recipient, settings);
			}
			else
			{
				WrapPartitionedStreamNotIndexed(inputStream, recipient, settings);
			}
		}

		/// <summary>
		/// A helper method for WrapPartitionedStream. We use the helper to reuse a block of code twice, but with
		/// a different order key type. (If premature merge occured, the order key type will be "int". Otherwise, 
		/// it will be the same type as "TLeftKey" in WrapPartitionedStream.)
		/// </summary>
		private void WrapPartitionedStreamNotIndexed<TLeftKey>(PartitionedStream<TLeftInput, TLeftKey> inputStream, IPartitionedStreamRecipient<TOutput> recipient, QuerySettings settings)
		{
			int partitionCount = inputStream.PartitionCount;
			PairComparer<TLeftKey, int> keyComparer = new PairComparer<TLeftKey, int>(inputStream.KeyComparer, Util.GetDefaultComparer<int>());
			PartitionedStream<TOutput, Pair<TLeftKey, int>> partitionedStream = new PartitionedStream<TOutput, Pair<TLeftKey, int>>(partitionCount, keyComparer, OrdinalIndexState);
			for (int i = 0; i < partitionCount; i++)
			{
				partitionedStream[i] = new SelectManyQueryOperatorEnumerator<TLeftKey>(inputStream[i], this, settings.CancellationState.MergedCancellationToken);
			}
			recipient.Receive(partitionedStream);
		}

		/// <summary>
		/// Similar helper method to WrapPartitionedStreamNotIndexed, except that this one is for the indexed variant
		/// of SelectMany (i.e., the SelectMany that passes indices into the user sequence-generating delegate)
		/// </summary>
		private void WrapPartitionedStreamIndexed(PartitionedStream<TLeftInput, int> inputStream, IPartitionedStreamRecipient<TOutput> recipient, QuerySettings settings)
		{
			PairComparer<int, int> keyComparer = new PairComparer<int, int>(inputStream.KeyComparer, Util.GetDefaultComparer<int>());
			PartitionedStream<TOutput, Pair<int, int>> partitionedStream = new PartitionedStream<TOutput, Pair<int, int>>(inputStream.PartitionCount, keyComparer, OrdinalIndexState);
			for (int i = 0; i < inputStream.PartitionCount; i++)
			{
				partitionedStream[i] = new IndexedSelectManyQueryOperatorEnumerator(inputStream[i], this, settings.CancellationState.MergedCancellationToken);
			}
			recipient.Receive(partitionedStream);
		}

		internal override QueryResults<TOutput> Open(QuerySettings settings, bool preferStriping)
		{
			QueryResults<TLeftInput> childQueryResults = base.Child.Open(settings, preferStriping);
			return new UnaryQueryOperatorResults(childQueryResults, this, settings, preferStriping);
		}

		internal override IEnumerable<TOutput> AsSequentialQuery(CancellationToken token)
		{
			if (m_rightChildSelector != null)
			{
				if (m_resultSelector != null)
				{
					return CancellableEnumerable.Wrap(base.Child.AsSequentialQuery(token), token).SelectMany(m_rightChildSelector, m_resultSelector);
				}
				return (IEnumerable<TOutput>)CancellableEnumerable.Wrap(base.Child.AsSequentialQuery(token), token).SelectMany(m_rightChildSelector);
			}
			if (m_resultSelector != null)
			{
				return CancellableEnumerable.Wrap(base.Child.AsSequentialQuery(token), token).SelectMany(m_indexedRightChildSelector, m_resultSelector);
			}
			return (IEnumerable<TOutput>)CancellableEnumerable.Wrap(base.Child.AsSequentialQuery(token), token).SelectMany(m_indexedRightChildSelector);
		}
	}
}
