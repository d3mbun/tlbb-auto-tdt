namespace System.Linq.Parallel
{
	/// <summary>
	///
	/// An interface that allows developers to specify their own partitioning routines.
	///
	/// </summary>
	/// <typeparam name="T"></typeparam>
	internal interface IParallelPartitionable<T>
	{
		QueryOperatorEnumerator<T, int>[] GetPartitions(int partitionCount);
	}
}
