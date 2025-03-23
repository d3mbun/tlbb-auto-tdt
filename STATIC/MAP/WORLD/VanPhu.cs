using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _i.STATIC.WORLD
{
    class VanPhu
    {

		static int Id = MAP.VanPhu;
		public static string Name => "Vân Phù";
		public static NPC LuongThienHoa = new NPC()
		{
			Id = 135,
			X = 228,
			Y = 63,
			Map = Id,
			Name = "Lương Thiên Hòa"
		};
	}
}
