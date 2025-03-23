namespace System.Linq.Parallel
{
	/// <summary>
	/// An enum to specify whether an aggregate operator is associative, commutative,
	/// neither, or both. This influences query analysis and execution: associative
	/// aggregations can run in parallel, whereas non-associative cannot; non-commutative
	/// aggregations must be run over data in input-order. 
	/// </summary>
	[Flags]
	internal enum QueryAggregationOptions
	{
		None = 0x0,
		Associative = 0x1,
		Commutative = 0x2,
		AssociativeCommutative = 0x3
	}
}
