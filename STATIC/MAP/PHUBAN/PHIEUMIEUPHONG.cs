using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _i
{
    class PHIEUMIEUPHONG
    {
        public static int Id = MAP.PhieuMieuPhong;
        public static string Name => "Phiêu Miễu Phong";
        public static NPC CapDaiBa = new NPC()
        {
            Id = 12163,
            X = 124,
            Y = 86,
            Map = Id,
            Name = "Cáp Đại Bá"
        };

        public static NPC TangThoCong = new NPC()
        {
            Id = 12164,
            X = 41,
            Y = 105,
            Map = Id,
            Name = "Tang Thổ Công"
        };
    }
}
