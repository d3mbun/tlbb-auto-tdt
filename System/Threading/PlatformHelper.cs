namespace System.Threading
{
	/// <summary>
	/// A helper class to get the number of preocessors, it updates the numbers of processors every sampling interval
	/// </summary>
	internal static class PlatformHelper
	{
		private const int PROCESSOR_COUNT_REFRESH_INTERVAL_MS = 30000;

		private static int s_processorCount = -1;

		private static DateTime s_nextProcessorCountRefreshTime = DateTime.MinValue;

		/// <summary>
		/// Gets the number of available processors
		/// </summary>
		internal static int ProcessorCount
		{
			get
			{
				if (DateTime.UtcNow.CompareTo(s_nextProcessorCountRefreshTime) >= 0)
				{
					s_processorCount = Environment.ProcessorCount;
					s_nextProcessorCountRefreshTime = DateTime.UtcNow.AddMilliseconds(30000.0);
				}
				return s_processorCount;
			}
		}

		/// <summary>
		/// Gets whether the current machine has only a single processor.
		/// </summary>
		internal static bool IsSingleProcessor => ProcessorCount == 1;
	}
}
