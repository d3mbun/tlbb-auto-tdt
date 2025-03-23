namespace System.Diagnostics.Contracts
{
	/// <summary>
	/// A stub version of .NET 4.0 contracts.
	/// </summary>
	internal static class Contract
	{
		[Conditional("DEBUG")]
		internal static void Assert(bool condition)
		{
		}

		[Conditional("DEBUG")]
		internal static void Assert(bool condition, string message)
		{
		}

		[Conditional("DEBUG")]
		internal static void Ensures(bool condition)
		{
		}

		[Conditional("DEBUG")]
		internal static void EndContractBlock()
		{
		}
	}
}
