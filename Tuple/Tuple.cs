using System;

namespace System
{
	// Token: 0x02000004 RID: 4
	public static class Tuple
	{
		// Token: 0x06000004 RID: 4 RVA: 0x00002050 File Offset: 0x00000250
		public static Tuple<T1> Create<T1>(T1 item1)
		{
			return new Tuple<T1>(item1);
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002058 File Offset: 0x00000258
		public static Tuple<T1, T2> Create<T1, T2>(T1 item1, T2 item2)
		{
			return new Tuple<T1, T2>(item1, item2);
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002061 File Offset: 0x00000261
		public static Tuple<T1, T2, T3> Create<T1, T2, T3>(T1 item1, T2 item2, T3 item3)
		{
			return new Tuple<T1, T2, T3>(item1, item2, item3);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x0000206B File Offset: 0x0000026B
		public static Tuple<T1, T2, T3, T4> Create<T1, T2, T3, T4>(T1 item1, T2 item2, T3 item3, T4 item4)
		{
			return new Tuple<T1, T2, T3, T4>(item1, item2, item3, item4);
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002076 File Offset: 0x00000276
		public static Tuple<T1, T2, T3, T4, T5> Create<T1, T2, T3, T4, T5>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5)
		{
			return new Tuple<T1, T2, T3, T4, T5>(item1, item2, item3, item4, item5);
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002083 File Offset: 0x00000283
		public static Tuple<T1, T2, T3, T4, T5, T6> Create<T1, T2, T3, T4, T5, T6>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6)
		{
			return new Tuple<T1, T2, T3, T4, T5, T6>(item1, item2, item3, item4, item5, item6);
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002092 File Offset: 0x00000292
		public static Tuple<T1, T2, T3, T4, T5, T6, T7> Create<T1, T2, T3, T4, T5, T6, T7>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7)
		{
			return new Tuple<T1, T2, T3, T4, T5, T6, T7>(item1, item2, item3, item4, item5, item6, item7);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000020A3 File Offset: 0x000002A3
		public static Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8>> Create<T1, T2, T3, T4, T5, T6, T7, T8>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8)
		{
			return new Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8>>(item1, item2, item3, item4, item5, item6, item7, new Tuple<T8>(item8));
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000020BB File Offset: 0x000002BB
		internal static int CombineHashCodes(int h1, int h2)
		{
			return (h1 << 5) + h1 ^ h2;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000020C4 File Offset: 0x000002C4
		internal static int CombineHashCodes(int h1, int h2, int h3)
		{
			return Tuple.CombineHashCodes(Tuple.CombineHashCodes(h1, h2), h3);
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000020D3 File Offset: 0x000002D3
		internal static int CombineHashCodes(int h1, int h2, int h3, int h4)
		{
			return Tuple.CombineHashCodes(Tuple.CombineHashCodes(h1, h2), Tuple.CombineHashCodes(h3, h4));
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000020E8 File Offset: 0x000002E8
		internal static int CombineHashCodes(int h1, int h2, int h3, int h4, int h5)
		{
			return Tuple.CombineHashCodes(Tuple.CombineHashCodes(h1, h2, h3, h4), h5);
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000020FA File Offset: 0x000002FA
		internal static int CombineHashCodes(int h1, int h2, int h3, int h4, int h5, int h6)
		{
			return Tuple.CombineHashCodes(Tuple.CombineHashCodes(h1, h2, h3, h4), Tuple.CombineHashCodes(h5, h6));
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002113 File Offset: 0x00000313
		internal static int CombineHashCodes(int h1, int h2, int h3, int h4, int h5, int h6, int h7)
		{
			return Tuple.CombineHashCodes(Tuple.CombineHashCodes(h1, h2, h3, h4), Tuple.CombineHashCodes(h5, h6, h7));
		}

		// Token: 0x06000012 RID: 18 RVA: 0x0000212E File Offset: 0x0000032E
		internal static int CombineHashCodes(int h1, int h2, int h3, int h4, int h5, int h6, int h7, int h8)
		{
			return Tuple.CombineHashCodes(Tuple.CombineHashCodes(h1, h2, h3, h4), Tuple.CombineHashCodes(h5, h6, h7, h8));
		}
	}
}
