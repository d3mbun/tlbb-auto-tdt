using System.Diagnostics;

namespace System.Collections.Generic
{
	/// <summary>
	/// This internal class from mscorlib.dll is used by ConcurrentDictionary.
	/// </summary>
	internal sealed class Mscorlib_DictionaryDebugView<K, V>
	{
		private IDictionary<K, V> dict;

		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		internal KeyValuePair<K, V>[] Items
		{
			get
			{
				KeyValuePair<K, V>[] array = new KeyValuePair<K, V>[dict.Count];
				dict.CopyTo(array, 0);
				return array;
			}
		}

		internal Mscorlib_DictionaryDebugView(IDictionary<K, V> dictionary)
		{
			if (dictionary == null)
			{
				throw new ArgumentNullException("dictionary");
			}
			dict = dictionary;
		}
	}
}
