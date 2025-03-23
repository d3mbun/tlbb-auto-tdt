using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace System
{
	// Token: 0x0200000A RID: 10
	[Serializable]
	public class Tuple<T1, T2, T3, T4, T5, T6> : IStructuralEquatable, IStructuralComparable, IComparable, ITuple
	{
		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000059 RID: 89 RVA: 0x00002C28 File Offset: 0x00000E28
		public T1 Item1
		{
			get
			{
				return this.m_Item1;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600005A RID: 90 RVA: 0x00002C30 File Offset: 0x00000E30
		public T2 Item2
		{
			get
			{
				return this.m_Item2;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600005B RID: 91 RVA: 0x00002C38 File Offset: 0x00000E38
		public T3 Item3
		{
			get
			{
				return this.m_Item3;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600005C RID: 92 RVA: 0x00002C40 File Offset: 0x00000E40
		public T4 Item4
		{
			get
			{
				return this.m_Item4;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600005D RID: 93 RVA: 0x00002C48 File Offset: 0x00000E48
		public T5 Item5
		{
			get
			{
				return this.m_Item5;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600005E RID: 94 RVA: 0x00002C50 File Offset: 0x00000E50
		public T6 Item6
		{
			get
			{
				return this.m_Item6;
			}
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00002C58 File Offset: 0x00000E58
		public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6)
		{
			this.m_Item1 = item1;
			this.m_Item2 = item2;
			this.m_Item3 = item3;
			this.m_Item4 = item4;
			this.m_Item5 = item5;
			this.m_Item6 = item6;
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00002162 File Offset: 0x00000362
		public override bool Equals(object obj)
		{
			return ((IStructuralEquatable)this).Equals(obj, EqualityComparer<object>.Default);
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00002C90 File Offset: 0x00000E90
		bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
		{
			if (other == null)
			{
				return false;
			}
			Tuple<T1, T2, T3, T4, T5, T6> tuple = other as Tuple<T1, T2, T3, T4, T5, T6>;
			return tuple != null && (comparer.Equals(this.m_Item1, tuple.m_Item1) && comparer.Equals(this.m_Item2, tuple.m_Item2) && comparer.Equals(this.m_Item3, tuple.m_Item3) && comparer.Equals(this.m_Item4, tuple.m_Item4) && comparer.Equals(this.m_Item5, tuple.m_Item5)) && comparer.Equals(this.m_Item6, tuple.m_Item6);
		}

		// Token: 0x06000062 RID: 98 RVA: 0x000021AA File Offset: 0x000003AA
		int IComparable.CompareTo(object obj)
		{
			return ((IStructuralComparable)this).CompareTo(obj, Comparer<object>.Default);
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00002D68 File Offset: 0x00000F68
		int IStructuralComparable.CompareTo(object other, IComparer comparer)
		{
			if (other == null)
			{
				return 1;
			}
			Tuple<T1, T2, T3, T4, T5, T6> tuple = other as Tuple<T1, T2, T3, T4, T5, T6>;
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
			return comparer.Compare(this.m_Item6, tuple.m_Item6);
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00002210 File Offset: 0x00000410
		public override int GetHashCode()
		{
			return ((IStructuralEquatable)this).GetHashCode(EqualityComparer<object>.Default);
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00002E6C File Offset: 0x0000106C
		int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
		{
			return Tuple.CombineHashCodes(comparer.GetHashCode(this.m_Item1), comparer.GetHashCode(this.m_Item2), comparer.GetHashCode(this.m_Item3), comparer.GetHashCode(this.m_Item4), comparer.GetHashCode(this.m_Item5), comparer.GetHashCode(this.m_Item6));
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00002230 File Offset: 0x00000430
		int ITuple.GetHashCode(IEqualityComparer comparer)
		{
			return ((IStructuralEquatable)this).GetHashCode(comparer);
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00002EE4 File Offset: 0x000010E4
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("(");
			return ((ITuple)this).ToString(stringBuilder);
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00002F0C File Offset: 0x0000110C
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
			sb.Append(")");
			return sb.ToString();
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000069 RID: 105 RVA: 0x00002FD3 File Offset: 0x000011D3
		int ITuple.Size
		{
			get
			{
				return 6;
			}
		}

		// Token: 0x04000012 RID: 18
		private readonly T1 m_Item1;

		// Token: 0x04000013 RID: 19
		private readonly T2 m_Item2;

		// Token: 0x04000014 RID: 20
		private readonly T3 m_Item3;

		// Token: 0x04000015 RID: 21
		private readonly T4 m_Item4;

		// Token: 0x04000016 RID: 22
		private readonly T5 m_Item5;

		// Token: 0x04000017 RID: 23
		private readonly T6 m_Item6;
	}
}
