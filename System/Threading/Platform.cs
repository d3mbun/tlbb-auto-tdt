using System.Runtime.InteropServices;

namespace System.Threading
{
	/// <summary>
	/// A convenience class for common platform-related logic.
	/// </summary>
	internal static class Platform
	{
		/// <summary>
		/// Gets the number of available processors available to this process on the current machine.
		/// </summary>
		internal static int ProcessorCount => Environment.ProcessorCount;

		internal static bool IsSingleProcessor => ProcessorCount == 1;

		[DllImport("kernel32.dll")]
		private static extern int SwitchToThread();

		internal static void Yield()
		{
			SwitchToThread();
		}
	}
}
