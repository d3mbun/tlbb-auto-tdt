using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace System
{
	// Token: 0x02000008 RID: 8
	[Serializable]
	public class Tuple<T1, T2, T3, T4> : IStructuralEquatable, IStructuralComparable, IComparable, ITuple
	{
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600003A RID: 58 RVA: 0x00002650 File Offset: 0x00000850
		public T1 Item1
		{
			get
			{
				return this.m_Item1;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600003B RID: 59 RVA: 0x00002658 File Offset: 0x00000858
		public T2 Item2
		{
			get
			{
				return this.m_Item2;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600003C RID: 60 RVA: 0x00002660 File Offset: 0x00000860
		public T3 Item3
		{
			get
			{
				return this.m_Item3;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600003D RID: 61 RVA: 0x00002668 File Offset: 0x00000868
		public T4 Item4
		{
			get
			{
				return this.m_Item4;
			}
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002670 File Offset: 0x00000870
		public Tuple(T1 item1, T2 item2, T3 item3, T4 item4)
		{
			this.m_Item1 = item1;
			this.m_Item2 = item2;
			this.m_Item3 = item3;
			this.m_Item4 = item4;
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002162 File Offset: 0x00000362
		public override bool Equals(object obj)
		{
			return ((IStructuralEquatable)this).Equals(obj, EqualityComparer<object>.Default);
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002698 File Offset: 0x00000898
		bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
		{
			if (other == null)
			{
				return false;
			}
			Tuple<T1, T2, T3, T4> tuple = other as Tuple<T1, T2, T3, T4>;
			return tuple != null && (comparer.Equals(this.m_Item1, tuple.m_Item1) && comparer.Equals(this.m_Item2, tuple.m_Item2) && comparer.Equals(this.m_Item3, tuple.m_Item3)) && comparer.Equals(this.m_Item4, tuple.m_Item4);
		}

		// Token: 0x06000041 RID: 65 RVA: 0x000021AA File Offset: 0x000003AA
		int IComparable.CompareTo(object obj)
		{
			return ((IStructuralComparable)this).CompareTo(obj, Comparer<object>.Default);
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002730 File Offset: 0x00000930
		int IStructuralComparable.CompareTo(object other, IComparer comparer)
		{
			if (other == null)
			{
				return 1;
			}
			Tuple<T1, T2, T3, T4> tuple = other as Tuple<T1, T2, T3, T4>;
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
			return comparer.Compare(this.m_Item4, tuple.m_Item4);
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002210 File Offset: 0x00000410
		public override int GetHashCode()
		{
			return ((IStructuralEquatable)this).GetHashCode(EqualityComparer<object>.Default);
		}

		// Token: 0x06000044 RID: 68 RVA: 0x000027F0 File Offset: 0x000009F0
		int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
		{
			return Tuple.CombineHashCodes(comparer.GetHashCode(this.m_Item1), comparer.GetHashCode(this.m_Item2), comparer.GetHashCode(this.m_Item3), comparer.GetHashCode(this.m_Item4));
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00002230 File Offset: 0x00000430
		int ITuple.GetHashCode(IEqualityComparer comparer)
		{
			return ((IStructuralEquatable)this).GetHashCode(comparer);
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00002848 File Offset: 0x00000A48
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("(");
			return ((ITuple)this).ToString(stringBuilder);
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00002870 File Offset: 0x00000A70
		string ITuple.ToString(StringBuilder sb)
		{
			sb.Append(this.m_Item1);
			sb.Append(", ");
			sb.Append(this.m_Item2);
			sb.Append(", ");
			sb.Append(this.m_Item3);
			sb.Append(", ");
			sb.Append(this.m_Item4);
			sb.Append(")");
			return sb.ToString();
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000048 RID: 72 RVA: 0x000028FB File Offset: 0x00000AFB
		int ITuple.Size
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x04000009 RID: 9
		private readonly T1 m_Item1;

		// Token: 0x0400000A RID: 10
		private readonly T2 m_Item2;

		// Token: 0x0400000B RID: 11
		private readonly T3 m_Item3;

		// Token: 0x0400000C RID: 12
		private readonly T4 m_Item4;
	}
}
