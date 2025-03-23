using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _i
{
    class THAIHO
    {
        public static int Id = 4;
        public static string Name => "Thái Hồ";
        public static NPC HoDienKhanh = new NPC()
        {
            Id = 13,
            X = 67,
            Y = 77,
            Map = Id,
            Name = "Hô Diên Khánh "
        };

        public static NPC LyCuong = new NPC()
        {
            Id = 0,
            X = 70,
            Y = 119,
            Map = Id,
            Name = "Lý Cương"
        };
    }
}
