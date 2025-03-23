using System.Runtime.InteropServices;

namespace System.Linq.Parallel
{
	/// <summary>
	/// Used during hash partitioning, when the keys being memoized are not used for anything.
	/// </summary>
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	internal struct NoKeyMemoizationRequired
	{
	}
}
