namespace System.Threading.Tasks
{
	/// <summary>
	/// Utility class for allocating structs as heap variables
	/// </summary>
	internal class Shared<T>
	{
		internal T Value;

		internal Shared(T value)
		{
			Value = value;
		}
	}
}
