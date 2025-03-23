using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _i
{
    class LOIDAISINHTU
    {
        public static int Id = 546;
        public static string Name => "Lôi Đài Sinh Tử";
        public static NPC KhoVinhDaiSu = new NPC()
        {
            Id = 12436,
            X = 12,
            Y = 34,
            Map = Id,
            Name = "Khô Vinh Đại Sư"
        };
    }
}
