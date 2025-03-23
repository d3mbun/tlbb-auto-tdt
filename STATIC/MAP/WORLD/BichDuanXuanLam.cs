using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _i
{
    class BichDuanXuanLam
    {
		public static int Id = 769;
		public static string Name => "Bích Duẩn Xuân Lâm";
		public static NPC GiaMinhDat = new NPC()
		{
			Id = 2,
			X = 100,
			Y = 143,
			Map = Id,
			Name = "Giả Minh Đạt"
		};

		public static NPC LoanAnhBac = new NPC()
		{
			Id = 1,
			X = 379,
			Y = 210,
			Map = Id,
			Name = "Loan Anh Bác"
		};
	}
}
