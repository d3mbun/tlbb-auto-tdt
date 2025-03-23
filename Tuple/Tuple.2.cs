using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace System
{
	// Token: 0x02000005 RID: 5
	[Serializable]
	public class Tuple<T1> : IStructuralEquatable, IStructuralComparable, IComparable, ITuple
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000013 RID: 19 RVA: 0x0000214B File Offset: 0x0000034B
		public T1 Item1
		{
			get
			{
				return this.m_Item1;
			}
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002153 File Offset: 0x00000353
		public Tuple(T1 item1)
		{
			this.m_Item1 = item1;
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002162 File Offset: 0x00000362
		public override bool Equals(object obj)
		{
			return ((IStructuralEquatable)this).Equals(obj, EqualityComparer<object>.Default);
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002170 File Offset: 0x00000370
		bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
		{
			if (other == null)
			{
				return false;
			}
			Tuple<T1> tuple = other as Tuple<T1>;
			return tuple != null && comparer.Equals(this.m_Item1, tuple.m_Item1);
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000021AA File Offset: 0x000003AA
		int IComparable.CompareTo(object obj)
		{
			return ((IStructuralComparable)this).CompareTo(obj, Comparer<object>.Default);
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000021B8 File Offset: 0x000003B8
		int IStructuralComparable.CompareTo(object other, IComparer comparer)
		{
			if (other == null)
			{
				return 1;
			}
			Tuple<T1> tuple = other as Tuple<T1>;
			if (tuple == null)
			{
				throw new ArgumentException(string.Format("Argument must be of type {0}.", base.GetType().ToString()), "other");
			}
			return comparer.Compare(this.m_Item1, tuple.m_Item1);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002210 File Offset: 0x00000410
		public override int GetHashCode()
		{
			return ((IStructuralEquatable)this).GetHashCode(EqualityComparer<object>.Default);
		}

		// Token: 0x0600001A RID: 26 RVA: 0x0000221D File Offset: 0x0000041D
		int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
		{
			return comparer.GetHashCode(this.m_Item1);
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002230 File Offset: 0x00000430
		int ITuple.GetHashCode(IEqualityComparer comparer)
		{
			return ((IStructuralEquatable)this).GetHashCode(comparer);
		}

		// Token: 0x0600001C RID: 28 RVA: 0x0000223C File Offset: 0x0000043C
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("(");
			return ((ITuple)this).ToString(stringBuilder);
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002262 File Offset: 0x00000462
		string ITuple.ToString(StringBuilder sb)
		{
			sb.Append(this.m_Item1);
			sb.Append(")");
			return sb.ToString();
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600001E RID: 30 RVA: 0x00002288 File Offset: 0x00000488
		int ITuple.Size
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x04000003 RID: 3
		private readonly T1 m_Item1;
	}
}
