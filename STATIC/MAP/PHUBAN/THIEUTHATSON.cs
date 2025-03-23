using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _i
{
    class THIEUTHATSON
    {
        public static int Id = 566;
        public static string Name => "Thiếu Thất Sơn";
        public static NPC CuuMaTri = new NPC()
        {
            Id = 11719,
            X = 130,
            Y = 172,
            Map = Id,
            Name = "Cưu Ma Trí"
        };

        public static NPC DinhXuanThu = new NPC()
        {
            Id = 12005,
            X = 130,
            Y = 123,
            Map = Id,
            Name = "Đinh Xuân Thu"
        };

        public static NPC TrangTuHien = new NPC()
        {
            Id = 5402,
            X = 70,
            Y = 120,
            Map = Id,
            Name = "Trang Tụ Hiền"
        };

        public static NPC ThieuLamDeTu = new NPC()
        {
            Id = 1079,
            X = 130,
            Y = 123,
            Map = Id,
            Name = "Thiếu Lâm Đệ Tử"
        };

        public static NPC TaoDiaThanTang = new NPC()
        {
            Id = 5538,
            X = 137,
            Y = 36,
            Map = Id,
            Name = "Tảo Địa Thần Tăng"
        };

        public static NPC MoDungPhuc = new NPC()
        {
            Id = 11444,
            X = 195,
            Y = 86,
            Map = Id,
            Name = "Mộ Dung Phục"
        };

        public static NPC TieuPhong = new NPC()
        {
            Id = 524,
            X = 121,
            Y = 40,
            Map = Id,
            Name = "Tiêu Phong"
        };
    }
}
