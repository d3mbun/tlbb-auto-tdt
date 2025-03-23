using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace System
{
	// Token: 0x02000009 RID: 9
	[Serializable]
	public class Tuple<T1, T2, T3, T4, T5> : IStructuralEquatable, IStructuralComparable, IComparable, ITuple
	{
		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000049 RID: 73 RVA: 0x000028FE File Offset: 0x00000AFE
		public T1 Item1
		{
			get
			{
				return this.m_Item1;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600004A RID: 74 RVA: 0x00002906 File Offset: 0x00000B06
		public T2 Item2
		{
			get
			{
				return this.m_Item2;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600004B RID: 75 RVA: 0x0000290E File Offset: 0x00000B0E
		public T3 Item3
		{
			get
			{
				return this.m_Item3;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600004C RID: 76 RVA: 0x00002916 File Offset: 0x00000B16
		public T4 Item4
		{
			get
			{
				return this.m_Item4;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600004D RID: 77 RVA: 0x0000291E File Offset: 0x00000B1E
		public T5 Item5
		{
			get
			{
				return this.m_Item5;
			}
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00002926 File Offset: 0x00000B26
		public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5)
		{
			this.m_Item1 = item1;
			this.m_Item2 = item2;
			this.m_Item3 = item3;
			this.m_Item4 = item4;
			this.m_Item5 = item5;
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00002162 File Offset: 0x00000362
		public override bool Equals(object obj)
		{
			return ((IStructuralEquatable)this).Equals(obj, EqualityComparer<object>.Default);
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00002954 File Offset: 0x00000B54
		bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
		{
			if (other == null)
			{
				return false;
			}
			Tuple<T1, T2, T3, T4, T5> tuple = other as Tuple<T1, T2, T3, T4, T5>;
			return tuple != null && (comparer.Equals(this.m_Item1, tuple.m_Item1) && comparer.Equals(this.m_Item2, tuple.m_Item2) && comparer.Equals(this.m_Item3, tuple.m_Item3) && comparer.Equals(this.m_Item4, tuple.m_Item4)) && comparer.Equals(this.m_Item5, tuple.m_Item5);
		}

		// Token: 0x06000051 RID: 81 RVA: 0x000021AA File Offset: 0x000003AA
		int IComparable.CompareTo(object obj)
		{
			return ((IStructuralComparable)this).CompareTo(obj, Comparer<object>.Default);
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00002A08 File Offset: 0x00000C08
		int IStructuralComparable.CompareTo(object other, IComparer comparer)
		{
			if (other == null)
			{
				return 1;
			}
			Tuple<T1, T2, T3, T4, T5> tuple = other as Tuple<T1, T2, T3, T4, T5>;
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
			return comparer.Compare(this.m_Item5, tuple.m_Item5);
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00002210 File Offset: 0x00000410
		public override int GetHashCode()
		{
			return ((IStructuralEquatable)this).GetHashCode(EqualityComparer<object>.Default);
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00002AEC File Offset: 0x00000CEC
		int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
		{
			return Tuple.CombineHashCodes(comparer.GetHashCode(this.m_Item1), comparer.GetHashCode(this.m_Item2), comparer.GetHashCode(this.m_Item3), comparer.GetHashCode(this.m_Item4), comparer.GetHashCode(this.m_Item5));
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00002230 File Offset: 0x00000430
		int ITuple.GetHashCode(IEqualityComparer comparer)
		{
			return ((IStructuralEquatable)this).GetHashCode(comparer);
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00002B54 File Offset: 0x00000D54
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("(");
			return ((ITuple)this).ToString(stringBuilder);
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00002B7C File Offset: 0x00000D7C
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
			sb.Append(")");
			return sb.ToString();
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000058 RID: 88 RVA: 0x00002C25 File Offset: 0x00000E25
		int ITuple.Size
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x0400000D RID: 13
		private readonly T1 m_Item1;

		// Token: 0x0400000E RID: 14
		private readonly T2 m_Item2;

		// Token: 0x0400000F RID: 15
		private readonly T3 m_Item3;

		// Token: 0x04000010 RID: 16
		private readonly T4 m_Item4;

		// Token: 0x04000011 RID: 17
		private readonly T5 m_Item5;
	}
}
