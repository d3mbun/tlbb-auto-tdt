using System.Collections.Generic;

namespace System.Linq.Parallel
{
	/// <summary>
	/// We occassionally need a no-op enumerator to stand-in when we don't have data left
	/// within a partition's data stream. These are simple enumerable and enumerator
	/// implementations that always and consistently yield no elements.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	internal class EmptyEnumerable<T> : ParallelQuery<T>
	{
		private static EmptyEnumerable<T> s_instance;

		private static EmptyEnumerator<T> s_enumeratorInstance;

		internal static EmptyEnumerable<T> Instance
		{
			get
			{
				if (s_instance == null)
				{
					s_instance = new EmptyEnumerable<T>();
				}
				return s_instance;
			}
		}

		private EmptyEnumerable()
			: base(QuerySettings.Empty)
		{
		}

		public override IEnumerator<T> GetEnumerator()
		{
			if (s_enumeratorInstance == null)
			{
				s_enumeratorInstance = new EmptyEnumerator<T>();
			}
			return s_enumeratorInstance;
		}
	}
}
