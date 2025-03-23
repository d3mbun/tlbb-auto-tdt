using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _i
{
    class KimLang
    {
		public static int Id = 762;
		public static string Name => "Kim Lăng";
		public static NPC LacHaPhong = new NPC()
		{
			Id = 38,
			X = 362,
			Y = 536,
			Map = Id,
			Name = "Lạc Hà Phong"
		};

		public static NPC LyLapThanh = new NPC()
		{
			Id = 24,
			X = 365,
			Y = 350,
			Map = Id,
			Name = "Lý Lập Thanh"
		};

		public static NPC TieuLang = new NPC()
		{
			Id = 30,
			X = 359,
			Y = 421,
			Map = Id,
			Name = "Tiêu Lăng"
		};

		public static NPC BichLac = new NPC()
		{
			Id = 31,
			X = 361,
			Y = 446,
			Map = Id,
			Name = "Bích Lạc"
		};

		public static NPC TieuUng = new NPC()
		{
			Id = 8,
			X = 368,
			Y = 421,
			Map = Id,
			Name = "Tiêu Ưng"
		};

		public static NPC AnHienKy = new NPC()
		{
			Id = 0,
			X = 277,
			Y = 220,
			Map = Id,
			Name = "Ân Hiên Kỳ"
		};
	}
}
