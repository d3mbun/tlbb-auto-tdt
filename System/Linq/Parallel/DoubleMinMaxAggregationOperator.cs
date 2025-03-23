using System.Collections.Generic;
using System.Threading;

namespace System.Linq.Parallel
{
	/// <summary>
	/// An inlined min/max aggregation and its enumerator, for doubles.
	///
	/// Notes:
	///     Note that normally double.NaN &lt; anything is false, as is anything &lt; NaN.  This would
	///     lead to some strangeness in Min and Max, e.g. Min({ NaN, 5.0 } == NaN, yet
	///     Min({ 5.0, NaN }) == 5.0!  We impose a total ordering so that NaN is smaller than
	///     everything, including -infinity, which is consistent with Comparer_T. 
	/// </summary>
	internal sealed class DoubleMinMaxAggregationOperator : InlinedAggregationOperator<double, double, double>
	{
		private class DoubleMinMaxAggregationOperatorEnumerator<TKey> : InlinedAggregationOperatorEnumerator<double>
		{
			private QueryOperatorEnumerator<double, TKey> m_source;

			private int m_sign;

			internal DoubleMinMaxAggregationOperatorEnumerator(QueryOperatorEnumerator<double, TKey> source, int partitionIndex, int sign, CancellationToken cancellationToken)
				: base(partitionIndex, cancellationToken)
			{
				m_source = source;
				m_sign = sign;
			}

			protected override bool MoveNextCore(ref double currentElement)
			{
				QueryOperatorEnumerator<double, TKey> source = m_source;
				TKey currentKey = default(TKey);
				if (source.MoveNext(ref currentElement, ref currentKey))
				{
					int num = 0;
					if (m_sign == -1)
					{
						double currentElement2 = 0.0;
						while (source.MoveNext(ref currentElement2, ref currentKey))
						{
							if ((num++ & 0x3F) == 0)
							{
								CancellationState.ThrowIfCanceled(m_cancellationToken);
							}
							if (currentElement2 < currentElement || double.IsNaN(currentElement2))
							{
								currentElement = currentElement2;
							}
						}
					}
					else
					{
						double currentElement3 = 0.0;
						while (source.MoveNext(ref currentElement3, ref currentKey))
						{
							if ((num++ & 0x3F) == 0)
							{
								CancellationState.ThrowIfCanceled(m_cancellationToken);
							}
							if (currentElement3 > currentElement || double.IsNaN(currentElement))
							{
								currentElement = currentElement3;
							}
						}
					}
					return true;
				}
				return false;
			}

			protected override void Dispose(bool disposing)
			{
				m_source.Dispose();
			}
		}

		private readonly int m_sign;

		internal DoubleMinMaxAggregationOperator(IEnumerable<double> child, int sign)
			: base(child)
		{
			m_sign = sign;
		}

		protected override double InternalAggregate(ref Exception singularExceptionToThrow)
		{
			using (IEnumerator<double> enumerator = GetEnumerator(ParallelMergeOptions.FullyBuffered, suppressOrderPreservation: true))
			{
				if (!enumerator.MoveNext())
				{
					singularExceptionToThrow = new InvalidOperationException("NoElements");
					return 0.0;
				}
				double num = enumerator.Current;
				if (m_sign == -1)
				{
					while (enumerator.MoveNext())
					{
						double current = enumerator.Current;
						if (current < num || double.IsNaN(current))
						{
							num = current;
						}
					}
				}
				else
				{
					while (enumerator.MoveNext())
					{
						double current2 = enumerator.Current;
						if (current2 > num || double.IsNaN(num))
						{
							num = current2;
						}
					}
				}
				return num;
			}
		}

		protected override QueryOperatorEnumerator<double, int> CreateEnumerator<TKey>(int index, int count, QueryOperatorEnumerator<double, TKey> source, object sharedData, CancellationToken cancellationToken)
		{
			return new DoubleMinMaxAggregationOperatorEnumerator<TKey>(source, index, m_sign, cancellationToken);
		}
	}
}
