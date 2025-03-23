using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _i
{
    class DAOHOA
    {
        public static int Id = 731;
        public static string Name => "Đào Hoa Đảo";
        // npc kỵ
        public static NPC HoangChiTri = new NPC()
        {
            Id = 4,
            X = 103,
            Y = 138,
            Map = Id,
            Name = "Hoàng Chí Tri"
        };

        // npc bái sư
        public static NPC QuanHoanChau = new NPC()
        {
            Id = 0,
            X = 207,
            Y = 72,
            Map = Id,
            Name = "Quân Hoàn Châu"
        };

        // npc nhiệm vụ
        public static NPC TieuThanhYet = new NPC()
        {
            Id = 2,
            X = 160,
            Y = 72,
            Map = Id,
            Name = "Tiêu Thanh Yết"
        };

        // npc tâm pháp
        public static NPC YenPhan = new NPC()
        {
            Id = 1,
            X = 207,
            Y = 66,
            Map = Id,
            Name = "Yến Phản"
        };
    }
}
