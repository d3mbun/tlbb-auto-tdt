using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _i
{
    class QUYCOC
    {
        public static int Id = 678;
        public static string Name => "Quỷ Cốc";
        // npc kỵ
        public static NPC ToCam = new NPC()
        {
            Id = 4,
            X = 150,
            Y = 150,
            Map = Id,
            Name = "Tô Cầm"
        };

        // npc nhiệm vụ
        public static NPC VuongHuyenPhong = new NPC()
        {
            Id = 2,
            X = 96,
            Y = 98,
            Map = Id,
            Name = "Vương Huyền Phong"
        };

        public static NPC VuongThienNhat = new NPC()
        {
            Id = 0,
            X = 99,
            Y = 56,
            Map = Id,
            Name = "Vương Thiền Nhất"
        };

        // tâm pháp
        public static NPC LyKeLong = new NPC()
        {
            Id = 1,
            X = 92,
            Y = 55,
            Map = Id,
            Name = "Lý Kế Long"
        };
    }
}
