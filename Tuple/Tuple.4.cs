using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace System
{
	// Token: 0x02000007 RID: 7
	[Serializable]
	public class Tuple<T1, T2, T3> : IStructuralEquatable, IStructuralComparable, IComparable, ITuple
	{
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600002C RID: 44 RVA: 0x00002432 File Offset: 0x00000632
		public T1 Item1
		{
			get
			{
				return this.m_Item1;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600002D RID: 45 RVA: 0x0000243A File Offset: 0x0000063A
		public T2 Item2
		{
			get
			{
				return this.m_Item2;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600002E RID: 46 RVA: 0x00002442 File Offset: 0x00000642
		public T3 Item3
		{
			get
			{
				return this.m_Item3;
			}
		}

		// Token: 0x0600002F RID: 47 RVA: 0x0000244A File Offset: 0x0000064A
		public Tuple(T1 item1, T2 item2, T3 item3)
		{
			this.m_Item1 = item1;
			this.m_Item2 = item2;
			this.m_Item3 = item3;
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002162 File Offset: 0x00000362
		public override bool Equals(object obj)
		{
			return ((IStructuralEquatable)this).Equals(obj, EqualityComparer<object>.Default);
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002468 File Offset: 0x00000668
		bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
		{
			if (other == null)
			{
				return false;
			}
			Tuple<T1, T2, T3> tuple = other as Tuple<T1, T2, T3>;
			return tuple != null && (comparer.Equals(this.m_Item1, tuple.m_Item1) && comparer.Equals(this.m_Item2, tuple.m_Item2)) && comparer.Equals(this.m_Item3, tuple.m_Item3);
		}

		// Token: 0x06000032 RID: 50 RVA: 0x000021AA File Offset: 0x000003AA
		int IComparable.CompareTo(object obj)
		{
			return ((IStructuralComparable)this).CompareTo(obj, Comparer<object>.Default);
		}

		// Token: 0x06000033 RID: 51 RVA: 0x000024E0 File Offset: 0x000006E0
		int IStructuralComparable.CompareTo(object other, IComparer comparer)
		{
			if (other == null)
			{
				return 1;
			}
			Tuple<T1, T2, T3> tuple = other as Tuple<T1, T2, T3>;
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
			return comparer.Compare(this.m_Item3, tuple.m_Item3);
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002210 File Offset: 0x00000410
		public override int GetHashCode()
		{
			return ((IStructuralEquatable)this).GetHashCode(EqualityComparer<object>.Default);
		}

		// Token: 0x06000035 RID: 53 RVA: 0x0000257E File Offset: 0x0000077E
		int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
		{
			return Tuple.CombineHashCodes(comparer.GetHashCode(this.m_Item1), comparer.GetHashCode(this.m_Item2), comparer.GetHashCode(this.m_Item3));
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002230 File Offset: 0x00000430
		int ITuple.GetHashCode(IEqualityComparer comparer)
		{
			return ((IStructuralEquatable)this).GetHashCode(comparer);
		}

		// Token: 0x06000037 RID: 55 RVA: 0x000025B8 File Offset: 0x000007B8
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("(");
			return ((ITuple)this).ToString(stringBuilder);
		}

		// Token: 0x06000038 RID: 56 RVA: 0x000025E0 File Offset: 0x000007E0
		string ITuple.ToString(StringBuilder sb)
		{
			sb.Append(this.m_Item1);
			sb.Append(", ");
			sb.Append(this.m_Item2);
			sb.Append(", ");
			sb.Append(this.m_Item3);
			sb.Append(")");
			return sb.ToString();
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000039 RID: 57 RVA: 0x0000264D File Offset: 0x0000084D
		int ITuple.Size
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x04000006 RID: 6
		private readonly T1 m_Item1;

		// Token: 0x04000007 RID: 7
		private readonly T2 m_Item2;

		// Token: 0x04000008 RID: 8
		private readonly T3 m_Item3;
	}
}
