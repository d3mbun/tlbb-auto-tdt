namespace System.Linq.Parallel
{
	/// <summary>
	/// Describes the state of order preservation index associated with an enumerator. 
	/// </summary>
	internal enum OrdinalIndexState : byte
	{
		Indexible,
		Correct,
		Increasing,
		Shuffled
	}
}
