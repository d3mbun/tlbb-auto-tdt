using System;

namespace System.Collections
{
	// Token: 0x0200000E RID: 14
	public interface IStructuralEquatable
	{
		// Token: 0x06000090 RID: 144
		bool Equals(object other, IEqualityComparer comparer);

		// Token: 0x06000091 RID: 145
		int GetHashCode(IEqualityComparer comparer);
	}
}
