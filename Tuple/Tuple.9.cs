using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace System
{
	// Token: 0x0200000C RID: 12
	[Serializable]
	public class Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> : IStructuralEquatable, IStructuralComparable, IComparable, ITuple
	{
		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600007C RID: 124 RVA: 0x00003408 File Offset: 0x00001608
		public T1 Item1
		{
			get
			{
				return this.m_Item1;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600007D RID: 125 RVA: 0x00003410 File Offset: 0x00001610
		public T2 Item2
		{
			get
			{
				return this.m_Item2;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x0600007E RID: 126 RVA: 0x00003418 File Offset: 0x00001618
		public T3 Item3
		{
			get
			{
				return this.m_Item3;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x0600007F RID: 127 RVA: 0x00003420 File Offset: 0x00001620
		public T4 Item4
		{
			get
			{
				return this.m_Item4;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000080 RID: 128 RVA: 0x00003428 File Offset: 0x00001628
		public T5 Item5
		{
			get
			{
				return this.m_Item5;
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000081 RID: 129 RVA: 0x00003430 File Offset: 0x00001630
		public T6 Item6
		{
			get
			{
				return this.m_Item6;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000082 RID: 130 RVA: 0x00003438 File Offset: 0x00001638
		public T7 Item7
		{
			get
			{
				return this.m_Item7;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000083 RID: 131 RVA: 0x00003440 File Offset: 0x00001640
		public TRest Rest
		{
			get
			{
				return this.m_Rest;
			}
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00003448 File Offset: 0x00001648
		public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, TRest rest)
		{
			if (!(rest is ITuple))
			{
				throw new ArgumentException(string.Format("The last element of an eight element tuple must be a Tuple.", new object[0]));
			}
			this.m_Item1 = item1;
			this.m_Item2 = item2;
			this.m_Item3 = item3;
			this.m_Item4 = item4;
			this.m_Item5 = item5;
			this.m_Item6 = item6;
			this.m_Item7 = item7;
			this.m_Rest = rest;
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00002162 File Offset: 0x00000362
		public override bool Equals(object obj)
		{
			return ((IStructuralEquatable)this).Equals(obj, EqualityComparer<object>.Default);
		}

		// Token: 0x06000086 RID: 134 RVA: 0x000034BC File Offset: 0x000016BC
		bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
		{
			if (other == null)
			{
				return false;
			}
			Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> tuple = other as Tuple<T1, T2, T3, T4, T5, T6, T7, TRest>;
			return tuple != null && (comparer.Equals(this.m_Item1, tuple.m_Item1) && comparer.Equals(this.m_Item2, tuple.m_Item2) && comparer.Equals(this.m_Item3, tuple.m_Item3) && comparer.Equals(this.m_Item4, tuple.m_Item4) && comparer.Equals(this.m_Item5, tuple.m_Item5) && comparer.Equals(this.m_Item6, tuple.m_Item6) && comparer.Equals(this.m_Item7, tuple.m_Item7)) && comparer.Equals(this.m_Rest, tuple.m_Rest);
		}

		// Token: 0x06000087 RID: 135 RVA: 0x000021AA File Offset: 0x000003AA
		int IComparable.CompareTo(object obj)
		{
			return ((IStructuralComparable)this).CompareTo(obj, Comparer<object>.Default);
		}

		// Token: 0x06000088 RID: 136 RVA: 0x000035D4 File Offset: 0x000017D4
		int IStructuralComparable.CompareTo(object other, IComparer comparer)
		{
			if (other == null)
			{
				return 1;
			}
			Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> tuple = other as Tuple<T1, T2, T3, T4, T5, T6, T7, TRest>;
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
			num = comparer.Compare(this.m_Item7, tuple.m_Item7);
			if (num != 0)
			{
				return num;
			}
			return comparer.Compare(this.m_Rest, tuple.m_Rest);
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00002210 File Offset: 0x00000410
		public override int GetHashCode()
		{
			return ((IStructuralEquatable)this).GetHashCode(EqualityComparer<object>.Default);
		}

		// Token: 0x0600008A RID: 138 RVA: 0x0000371C File Offset: 0x0000191C
		int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
		{
			ITuple tuple = (ITuple)((object)this.m_Rest);
			if (tuple.Size >= 8)
			{
				return tuple.GetHashCode(comparer);
			}
			switch (8 - tuple.Size)
			{
			case 1:
				return Tuple.CombineHashCodes(comparer.GetHashCode(this.m_Item7), tuple.GetHashCode(comparer));
			case 2:
				return Tuple.CombineHashCodes(comparer.GetHashCode(this.m_Item6), comparer.GetHashCode(this.m_Item7), tuple.GetHashCode(comparer));
			case 3:
				return Tuple.CombineHashCodes(comparer.GetHashCode(this.m_Item5), comparer.GetHashCode(this.m_Item6), comparer.GetHashCode(this.m_Item7), tuple.GetHashCode(comparer));
			case 4:
				return Tuple.CombineHashCodes(comparer.GetHashCode(this.m_Item4), comparer.GetHashCode(this.m_Item5), comparer.GetHashCode(this.m_Item6), comparer.GetHashCode(this.m_Item7), tuple.GetHashCode(comparer));
			case 5:
				return Tuple.CombineHashCodes(comparer.GetHashCode(this.m_Item3), comparer.GetHashCode(this.m_Item4), comparer.GetHashCode(this.m_Item5), comparer.GetHashCode(this.m_Item6), comparer.GetHashCode(this.m_Item7), tuple.GetHashCode(comparer));
			case 6:
				return Tuple.CombineHashCodes(comparer.GetHashCode(this.m_Item2), comparer.GetHashCode(this.m_Item3), comparer.GetHashCode(this.m_Item4), comparer.GetHashCode(this.m_Item5), comparer.GetHashCode(this.m_Item6), comparer.GetHashCode(this.m_Item7), tuple.GetHashCode(comparer));
			case 7:
				return Tuple.CombineHashCodes(comparer.GetHashCode(this.m_Item1), comparer.GetHashCode(this.m_Item2), comparer.GetHashCode(this.m_Item3), comparer.GetHashCode(this.m_Item4), comparer.GetHashCode(this.m_Item5), comparer.GetHashCode(this.m_Item6), comparer.GetHashCode(this.m_Item7), tuple.GetHashCode(comparer));
			default:
				return -1;
			}
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00002230 File Offset: 0x00000430
		int ITuple.GetHashCode(IEqualityComparer comparer)
		{
			return ((IStructuralEquatable)this).GetHashCode(comparer);
		}

		// Token: 0x0600008C RID: 140 RVA: 0x000039B8 File Offset: 0x00001BB8
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("(");
			return ((ITuple)this).ToString(stringBuilder);
		}

		// Token: 0x0600008D RID: 141 RVA: 0x000039E0 File Offset: 0x00001BE0
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
			sb.Append(", ");
			return ((ITuple)((object)this.m_Rest)).ToString(sb);
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600008E RID: 142 RVA: 0x00003AD5 File Offset: 0x00001CD5
		int ITuple.Size
		{
			get
			{
				return 7 + ((ITuple)((object)this.m_Rest)).Size;
			}
		}

		// Token: 0x0400001F RID: 31
		private readonly T1 m_Item1;

		// Token: 0x04000020 RID: 32
		private readonly T2 m_Item2;

		// Token: 0x04000021 RID: 33
		private readonly T3 m_Item3;

		// Token: 0x04000022 RID: 34
		private readonly T4 m_Item4;

		// Token: 0x04000023 RID: 35
		private readonly T5 m_Item5;

		// Token: 0x04000024 RID: 36
		private readonly T6 m_Item6;

		// Token: 0x04000025 RID: 37
		private readonly T7 m_Item7;

		// Token: 0x04000026 RID: 38
		private readonly TRest m_Rest;
	}
}
