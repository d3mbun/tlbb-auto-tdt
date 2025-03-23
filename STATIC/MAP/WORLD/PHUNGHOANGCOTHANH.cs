using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _i
{
    class PHUNGHOANGCOTHANH
    {
        public static int Id = 280;
        public static string Name => "Phụng Hoàng Cổ Thành";
        public static NPC HoangLongThien = new NPC()
        {
            Id = 643,
            X = 163,
            Y = 164,
            Map = Id,
            Name = "Hoàng Long Thiên"
        };
    }
}
