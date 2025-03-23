using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _i
{
    class TRANLONGKYCUOC
    {
        public static int Id = 61;
        public static string Name => "Trân Long Kỳ Cuộc";
        public static NPC TeThanh = new NPC()
        {
            Id = 12349,
            X = 40,
            Y = 40,
            Map = Id,
            Name = "Tế Thánh"
        };
    }
}
