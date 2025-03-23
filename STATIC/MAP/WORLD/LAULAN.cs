using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _i
{
    class LAULAN
    {
        public static int Id = 246;
        public static string Name => "Lâu Lan";
        public static NPC PhuSinh = new NPC()
        {
            Id = 88,
            X = 171,
            Y = 120,
            Map = Id,
            Name = "Phù Sinh"
        };

        public static NPC CaoDuong = new NPC()
        {
            Id = 14,
            X = 211,
            Y = 176,
            Map = Id,
            Name = "Cao Dương"
        };

        // bang
        public static NPC PhamThuanHuu = new NPC()
        {
            Id = 89,
            X = 191,
            Y = 130,
            Map = Id,
            Name = "Phạm Thuần Hựu"
        };

        // lau lan tam bao
        public static NPC KimCuuLinh = new NPC()
        {
            Id = 70,
            X = 162,
            Y = 75,
            Map = Id,
            Name = "Kim Cửu Linh"
        };

        // thuong kho
        public static NPC ThuongKho = new NPC()
        {
            Id = 7,
            X = 207,
            Y = 122,
            Map = Id,
            Name = "Hầu bàn Tống"
        };

        // q123
        public static NPC HaDuyet = new NPC()
        {
            Id = 71,
            X = 295,
            Y = 68,
            Map = Id,
            Name = "Hà Duyệt"
        };

        // pmp
        public static NPC TrinhThanhSuong = new NPC()
        {
            Id = 56,
            X = 193,
            Y = 224,
            Map = Id,
            Name = "Trình Thanh Sương"
        };
    }
}
