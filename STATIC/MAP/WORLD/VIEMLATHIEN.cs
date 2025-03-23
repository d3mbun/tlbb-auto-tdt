using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _i
{
    class VIEMLATHIEN
    {
        public static int Id = 666;
        public static string Name => "Viêm La Thiên";
        public static NPC ThongBaoHauCan = new NPC()
        {
            Id = 22,
            X = 157,
            Y = 201,
            Map = Id,
            Name = "Thông Báo Hậu Cần"
        };

        public static NPC PhongBoQuy = new NPC()
        {
            Id = 6,
            X = 167,
            Y = 197,
            Map = Id,
            Name = "Phong Bộ Quy"
        };
    }
}
