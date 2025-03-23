using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _i
{
    class THUCHACOTRAN
    {
        public static int Id = 260;
        public static string Name => "Thúc Hà Cổ Trấn";
        public static NPC LyDa = new NPC()
        {
            Id = 55,
            X = 150,
            Y = 152,
            Map = Id,
            Name = "Lý Dã"
        };

        // thuong kho
        public static NPC ThuongKho = new NPC()
        {
            Id = 10,
            X = 200,
            Y = 253,
            Map = Id,
            Name = "Hầu bàn Lưu"
        };

        // thuong kho tinh kiem
        public static NPC ThuongKhoTinhKiem = new NPC()
        {
            Id = 10,
            X = 200,
            Y = 253,
            Map = Id,
            Name = "Lưu tiểu nhị"
        };
    }
}
