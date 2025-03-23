using System.Collections.Generic;

namespace System.Linq.Parallel
{
	/// <summary>
	/// A special merge helper for indexible queries. Given an indexible query, we know how many elements
	/// we'll have in the result set, so we can allocate the array ahead of time. Then, as each result element
	/// is produced, we can directly insert it into the appropriate position in the output array, paying
	/// no extra cost for ordering.
	/// </summary>
	/// <typeparam name="TInputOutput"></typeparam>
	internal class ArrayMergeHelper<TInputOutput> : IMergeHelper<TInputOutput>
	{
		private QueryResults<TInputOutput> m_queryResults;

		private TInputOutput[] m_outputArray;

		private QuerySettings m_settings;

		/// <summary>
		/// Instantiates the array merge helper.
		/// </summary>
		/// <param name="settings">The query settings</param>
		/// <param name="queryResults">The query results</param>
		public ArrayMergeHelper(QuerySettings settings, QueryResults<TInputOutput> queryResults)
		{
			m_settings = settings;
			m_queryResults = queryResults;
			int count = m_queryResults.Count;
			m_outputArray = new TInputOutput[count];
		}

		/// <summary>
		/// A method used as a delegate passed into the ForAll operator
		/// </summary>
		private void ToArrayElement(int index)
		{
			m_outputArray[index] = m_queryResults[index];
		}

		/// <summary>
		/// Schedules execution of the merge itself.
		/// </summary>
		public void Execute()
		{
			ParallelQuery<int> source = ParallelEnumerable.Range(0, m_queryResults.Count);
			source = new QueryExecutionOption<int>(QueryOperator<int>.AsQueryOperator(source), m_settings);
			source.ForAll(ToArrayElement);
		}

		/// <summary>
		/// Gets the enumerator over the results.
		///
		/// We never expect this method to be called. ArrayMergeHelper is intended to be used when we want
		/// to consume the results using GetResultsAsArray().
		/// </summary>
		public IEnumerator<TInputOutput> GetEnumerator()
		{
			return ((IEnumerable<TInputOutput>)GetResultsAsArray()).GetEnumerator();
		}

		/// <summary>
		/// Returns the merged results as an array.
		/// </summary>
		/// <returns></returns>
		public TInputOutput[] GetResultsAsArray()
		{
			return m_outputArray;
		}
	}
}
