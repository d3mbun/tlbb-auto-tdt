using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace System
{
	// Token: 0x0200000B RID: 11
	[Serializable]
	public class Tuple<T1, T2, T3, T4, T5, T6, T7> : IStructuralEquatable, IStructuralComparable, IComparable, ITuple
	{
		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600006A RID: 106 RVA: 0x00002FD6 File Offset: 0x000011D6
		public T1 Item1
		{
			get
			{
				return this.m_Item1;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600006B RID: 107 RVA: 0x00002FDE File Offset: 0x000011DE
		public T2 Item2
		{
			get
			{
				return this.m_Item2;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600006C RID: 108 RVA: 0x00002FE6 File Offset: 0x000011E6
		public T3 Item3
		{
			get
			{
				return this.m_Item3;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600006D RID: 109 RVA: 0x00002FEE File Offset: 0x000011EE
		public T4 Item4
		{
			get
			{
				return this.m_Item4;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600006E RID: 110 RVA: 0x00002FF6 File Offset: 0x000011F6
		public T5 Item5
		{
			get
			{
				return this.m_Item5;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600006F RID: 111 RVA: 0x00002FFE File Offset: 0x000011FE
		public T6 Item6
		{
			get
			{
				return this.m_Item6;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000070 RID: 112 RVA: 0x00003006 File Offset: 0x00001206
		public T7 Item7
		{
			get
			{
				return this.m_Item7;
			}
		}

		// Token: 0x06000071 RID: 113 RVA: 0x0000300E File Offset: 0x0000120E
		public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7)
		{
			this.m_Item1 = item1;
			this.m_Item2 = item2;
			this.m_Item3 = item3;
			this.m_Item4 = item4;
			this.m_Item5 = item5;
			this.m_Item6 = item6;
			this.m_Item7 = item7;
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00002162 File Offset: 0x00000362
		public override bool Equals(object obj)
		{
			return ((IStructuralEquatable)this).Equals(obj, EqualityComparer<object>.Default);
		}

		// Token: 0x06000073 RID: 115 RVA: 0x0000304C File Offset: 0x0000124C
		bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
		{
			if (other == null)
			{
				return false;
			}
			Tuple<T1, T2, T3, T4, T5, T6, T7> tuple = other as Tuple<T1, T2, T3, T4, T5, T6, T7>;
			return tuple != null && (comparer.Equals(this.m_Item1, tuple.m_Item1) && comparer.Equals(this.m_Item2, tuple.m_Item2) && comparer.Equals(this.m_Item3, tuple.m_Item3) && comparer.Equals(this.m_Item4, tuple.m_Item4) && comparer.Equals(this.m_Item5, tuple.m_Item5) && comparer.Equals(this.m_Item6, tuple.m_Item6)) && comparer.Equals(this.m_Item7, tuple.m_Item7);
		}

		// Token: 0x06000074 RID: 116 RVA: 0x000021AA File Offset: 0x000003AA
		int IComparable.CompareTo(object obj)
		{
			return ((IStructuralComparable)this).CompareTo(obj, Comparer<object>.Default);
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00003144 File Offset: 0x00001344
		int IStructuralComparable.CompareTo(object other, IComparer comparer)
		{
			if (other == null)
			{
				return 1;
			}
			Tuple<T1, T2, T3, T4, T5, T6, T7> tuple = other as Tuple<T1, T2, T3, T4, T5, T6, T7>;
			if (tuple == null)
			{
				throw new ArgumentException(string.Format("Argument must be of type {0}.", base.GetType().ToString()), "other");
			}
			int num = comparer.Compare(this.m_Item1, tuple.m_Item1);
			if (num != 0)
			{
				return num;
			}
			num = comparer.Compare(this.m_Item2, tuple.m_Item2);
			if (num != 0)
			{
				return num;
			}
			num = comparer.Compare(this.m_Item3, tuple.m_Item3);
			if (num != 0)
			{
				return num;
			}
			num = comparer.Compare(this.m_Item4, tuple.m_Item4);
			if (num != 0)
			{
				return num;
			}
			num = comparer.Compare(this.m_Item5, tuple.m_Item5);
			if (num != 0)
			{
				return num;
			}
			num = comparer.Compare(this.m_Item6, tuple.m_Item6);
			if (num != 0)
			{
				return num;
			}
			return comparer.Compare(this.m_Item7, tuple.m_Item7);
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00002210 File Offset: 0x00000410
		public override int GetHashCode()
		{
			return ((IStructuralEquatable)this).GetHashCode(EqualityComparer<object>.Default);
		}

		// Token: 0x06000077 RID: 119 RVA: 0x0000326C File Offset: 0x0000146C
		int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
		{
			return Tuple.CombineHashCodes(comparer.GetHashCode(this.m_Item1), comparer.GetHashCode(this.m_Item2), comparer.GetHashCode(this.m_Item3), comparer.GetHashCode(this.m_Item4), comparer.GetHashCode(this.m_Item5), comparer.GetHashCode(this.m_Item6), comparer.GetHashCode(this.m_Item7));
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00002230 File Offset: 0x00000430
		int ITuple.GetHashCode(IEqualityComparer comparer)
		{
			return ((IStructuralEquatable)this).GetHashCode(comparer);
		}

		// Token: 0x06000079 RID: 121 RVA: 0x000032F8 File Offset: 0x000014F8
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("(");
			return ((ITuple)this).ToString(stringBuilder);
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00003320 File Offset: 0x00001520
		string ITuple.ToString(StringBuilder sb)
		{
			sb.Append(this.m_Item1);
			sb.Append(", ");
			sb.Append(this.m_Item2);
			sb.Append(", ");
			sb.Append(this.m_Item3);
			sb.Append(", ");
			sb.Append(this.m_Item4);
			sb.Append(", ");
			sb.Append(this.m_Item5);
			sb.Append(", ");
			sb.Append(this.m_Item6);
			sb.Append(", ");
			sb.Append(this.m_Item7);
			sb.Append(")");
			return sb.ToString();
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600007B RID: 123 RVA: 0x00003405 File Offset: 0x00001605
		int ITuple.Size
		{
			get
			{
				return 7;
			}
		}

		// Token: 0x04000018 RID: 24
		private readonly T1 m_Item1;

		// Token: 0x04000019 RID: 25
		private readonly T2 m_Item2;

		// Token: 0x0400001A RID: 26
		private readonly T3 m_Item3;

		// Token: 0x0400001B RID: 27
		private readonly T4 m_Item4;

		// Token: 0x0400001C RID: 28
		private readonly T5 m_Item5;

		// Token: 0x0400001D RID: 29
		private readonly T6 m_Item6;

		// Token: 0x0400001E RID: 30
		private readonly T7 m_Item7;
	}
}
