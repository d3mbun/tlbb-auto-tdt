using System.Collections.Generic;

namespace System.Linq.Parallel
{
	/// <summary>
	/// Used as a stand-in for replaceable merge algorithms. Alternative implementations
	/// are chosen based on the style of merge required. 
	/// </summary>
	/// <typeparam name="TInputOutput"></typeparam>
	internal interface IMergeHelper<TInputOutput>
	{
		void Execute();

		IEnumerator<TInputOutput> GetEnumerator();

		TInputOutput[] GetResultsAsArray();
	}
}
