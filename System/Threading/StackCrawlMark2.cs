using System.Runtime.InteropServices;

namespace System.Threading
{
	/// <summary>
	/// A dummy replacement for the .NET internal class StackCrawlMark.
	/// </summary>
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	internal struct StackCrawlMark2
	{
		internal static StackCrawlMark2 LookForMyCaller => default(StackCrawlMark2);
	}
}
