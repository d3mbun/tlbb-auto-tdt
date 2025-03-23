using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _i
{
    class PHUNGMINHTRAN
    {
        public static int Id = 580;
        public static string Name => "Phụng Minh Trấn";
        public static NPC TieuUng = new NPC()
        {
            Id = 4,
            X = 151,
            Y = 70,
            Map = Id,
            Name = "Tiêu Ưng"
        };

        public static NPC BichLac = new NPC()
        {
            Id = 115,
            X = 289,
            Y = 81,
            Map = Id,
            Name = "Bích Lạc"
        };

        // vuong lang
        public static NPC TieuLang = new NPC()
        {
            Id = 29,
            X = 289,
            Y = 67,
            Map = Id,
            Name = "Tiêu Lăng"
        };

        public static NPC SaoVanKhue = new NPC()
        {
            Id = 33,
            X = 158,
            Y = 65,
            Map = Id,
            Name = "Sào Vân Khuê"
        };

        public static NPC ThuongKho = new NPC()
        {
            Id = 14,
            X = 267,
            Y = 118,
            Map = Id,
            Name = "Tiền Tiên Sinh"
        };

        public static NPC TieuPhong = new NPC()
        {
            Id = 98,
            X = 101,
            Y = 127,
            Map = Id,
            Name = "Tiêu Phong"
        };
    }
}
