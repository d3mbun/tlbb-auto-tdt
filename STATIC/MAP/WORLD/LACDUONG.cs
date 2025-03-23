using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _i
{
    class LACDUONG
    {
        public static int Id = 0;
        public static string Name => "Lạc Dương";

        public static NPC ChuBaHanh = new NPC()
        {
            Id = 224,
            X = 296,
            Y = 225,
            Map = Id,
            Name = "Chu Bá Hành"
        };

        public static NPC TrieuTienTon = new NPC()
        {
            Id = 131,
            X = 183,
            Y = 231,
            Map = Id,
            Name = "Triệu Tiền Tôn"
        };

        public static NPC NgoDucXuong = new NPC()
        {
            Id = 86,
            X = 235,
            Y = 322,
            Map = Id,
            Name = "Ngô Đức Xương"
        };

        public static NPC KieuPhucThinh = new NPC()
        {
            Id = 131,
            X = 330,
            Y = 299,
            Map = Id,
            Name = "Kiều Phúc Thịnh"
        };

        public static NPC TranPhuChi = new NPC()
        {
            Id = 133,
            X = 239,
            Y = 204,
            Map = Id,
            Name = "Trần Phu Chi"
        };

        public static NPC BachManhSinh = new NPC()
        {
            Id = 159,
            X = 230,
            Y = 308,
            Map = Id,
            Name = "Bạch Manh Sinh"
        };

        public static NPC PhamThuanNhan = new NPC()
        {
            Id = 177,
            X = 237,
            Y = 236,
            Map = Id,
            Name = "Phạm Thuần Nhân"
        };

        // tạp hóa
        public static NPC NhuePhucTuong = new NPC()
        {
            Id = 58,
            X = 348,
            Y = 285,
            Map = Id,
            Name = "Nhuế Phúc Tường"
        };

        // huy vat pham quy
        public static NPC KhongTongUyen = new NPC()
        {
            Id = 156,
            X = 361,
            Y = 183,
            Map = Id,
            Name = "Khổng Tông Uyên"
        };

        // npc con
        public static NPC TruongDaiThuc = new NPC()
        {
            Id = 226,
            X = 153,
            Y = 184,
            Map = Id,
            Name = "Trương Đại Thúc"
        };

        // ban rac
        public static NPC VanDieuDieu = new NPC()
        {
            Id = 49,
            X = 275,
            Y = 295,
            Map = Id,
            Name = "Vân Diêu Diêu"
        };

        // x2
        public static NPC LuuKienMinh = new NPC()
        {
            Id = 17,
            X = 242,
            Y = 233,
            Map = Id,
            Name = "Lưu Kiện Minh"
        };

        // tiem quan ao
        public static NPC DaoDacBao = new NPC()
        {
            Id = 144,
            X = 274,
            Y = 322,
            Map = Id,
            Name = "Đào Đắc Bảo"
        };



        // long phu mau
        public static NPC MocTuTyTy = new NPC()
        {
            Id = 227,
            X = 151,
            Y = 187,
            Map = Id,
            Name = "Mộc Tử Tỷ Tỷ"
        };

        // thuong kho
        public static NPC ThuongKho = new NPC()
        {
            Id = 187,
            X = 347,
            Y = 248,
            Map = Id,
            Name = "Vệ tiên sinh"
        };

        // thuong kho tinh kiem
        public static NPC ThuongKhoTinhKiem = new NPC()
        {
            Id = 187,
            X = 347,
            Y = 249,
            Map = Id,
            Name = "Vệ tiên sinh "
        };

        // ky cuoc
        public static NPC VuongTichTan = new NPC()
        {
            Id = 142,
            X = 366,
            Y = 228,
            Map = Id,
            Name = "Vương Tích Tân"
        };        


        // tri thanh dai su
        public static NPC TriThanhDaiSu = new NPC()
        {
            Id = 34,
            X = 176,
            Y = 192,
            Map = Id,
            Name = "Trí Thanh Đại Sư"
        };

        public static NPC ToTriet = new NPC()
        {
            Id = 71,
            X = 258,
            Y = 163,
            Map = Id
        };

        public static NPC DinhDinh = new NPC()
        {
            Id = 190,
            X = 256,
            Y = 320,
            Map = Id
        };

        public static NPC DongDong = new NPC()
        {
            Id = 192,
            X = 253,
            Y = 320,
            Map = Id
        };
    }
}
