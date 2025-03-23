using System;
using System.Collections;
using System.Text;

namespace System
{
	// Token: 0x02000003 RID: 3
	internal interface ITuple
	{
		// Token: 0x06000001 RID: 1
		string ToString(StringBuilder sb);

		// Token: 0x06000002 RID: 2
		int GetHashCode(IEqualityComparer comparer);

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000003 RID: 3
		int Size { get; }
	}
}
