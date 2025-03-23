using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _i
{
    class BienKinh
    {
        public static int Id = 755;
        public static string Name => "Biện Kinh";
        public static NPC TieuLang = new NPC()
        {
            Id = 454,
            X = 410,
            Y = 380,
            Map = Id,
            Name = "Tiêu Lăng"
        };

        public static NPC BichLac = new NPC()
        {
            Id = 455,
            X = 621,
            Y = 380,
            Map = Id,
            Name = "Bích Lạc"
        };
    }
}
