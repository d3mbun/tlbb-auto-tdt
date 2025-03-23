using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _i
{
    class DAILY
    {
        public static int Id = 2;
        public static string Name => "Đại Lý";
        public static NPC ChucPhucQuy = new NPC()
        {
            Id = 165,
            X = 149,
            Y = 121,
            Map = Id,
            Name = "Chúc Phúc Quý"
        };

        public static NPC ThamMinhLau = new NPC()
        {
            Id = 233,
            X = 206,
            Y = 53,
            Map = Id,
            Name = "Thẩm Minh Lâu"
        };

        public static NPC LyThanhLa = new NPC()
        {
            Id = 225,
            X = 293,
            Y = 92,
            Map = Id,
            Name = "Lý Thanh La"
        };

        public static NPC ChuThucBan = new NPC()
        {
            Id = 194,
            X = 185,
            Y = 104,
            Map = Id,
            Name = "Chu Thúc Bân"
        };


        public static NPC ThamHanChau = new NPC()
        {
            Id = 232,
            X = 211,
            Y = 51,
            Map = Id,
            Name = "Thẩm Hàn Châu"
        };

        public static NPC TruongTheBinh = new NPC()
        {
            Id = 128,
            X = 68,
            Y = 178,
            Map = Id,
            Name = "Trương Thế Bình"
        };

        public static NPC DuTroChi = new NPC()
        {
            Id = 145,
            X = 78,
            Y = 136,
            Map = Id,
            Name = "Dư Trợ Chi"
        };


        public static NPC TaoDiaThanTang = new NPC()
        {
            Id = 170,
            X = 242,
            Y = 31,
            Map = Id,
            Name = "Tảo Địa Thần Tăng"
        };

        public static NPC TonBatGia = new NPC()
        {
            Id = 118,
            X = 175,
            Y = 143,
            Map = Id,
            Name = "Tôn Bát Gia"
        };

        public static NPC VoDong = new NPC()
        {
            Id = 170,
            X = 139,
            Y = 197,
            Map = Id,
            Name = "Võ Đồng"
        };
        // co gai ban hoa
        public static NPC ThamHamHuong = new NPC()
        {
            Id = 97,
            X = 189,
            Y = 65,
            Map = Id,
            Name = "Thẩm Hàm Hương"
        };

        public static NPC LyCongBo = new NPC()
        {
            Id = 24,
            X = 160,
            Y = 123,
            Map = Id,
            Name = "Lý Công Bộ"
        };

        // nhan hoa hong
        public static NPC BaCaiLy = new NPC()
        {
            Id = 174,
            X = 182,
            Y = 69,
            Map = Id,
            Name = "Ba Cái Lý"
        };

        // bach hoa duyen
        public static NPC ALy = new NPC()
        {
            Id = 173,
            X = 185,
            Y = 65,
            Map = Id,
            Name = "A Lý"
        };

        public static NPC HoaHachCan = new NPC()
        {
            Id = 21,
            X = 71,
            Y = 28,
            Map = Id,
            Name = "Hoa Hách Cấn"
        };

        // thieu that son
        public static NPC ChuDanThan = new NPC()
        {
            Id = 182,
            X = 70,
            Y = 58,
            Map = Id,
            Name = "Chu Đan Thần"
        };

        // thuong kho
        public static NPC ThuongKho = new NPC()
        {
            Id = 2,
            X = 203,
            Y = 177,
            Map = Id,
            Name = "Hầu bàn Chu"
        };

        // thuong kho tinh kiem
        public static NPC ThuongKhoTinhKiem = new NPC()
        {
            Id = 2,
            X = 201,
            Y = 177,
            Map = Id,
            Name = "Hầu bàn Chu "
        };

        // npc tran thu
        public static NPC VanPhieuPhieu = new NPC()
        {
            Id = 35,
            X = 271,
            Y = 133,
            Map = Id,
            Name = "Vân Phiêu Phiêu"
        };

        // sattinh
        public static NPC KhoVinhDaiSu = new NPC()
        {
            Id = 166,
            X = 131,
            Y = 79,
            Map = Id,
            Name = "Khô Vinh Đại Sư"
        };

        public static NPC TrieuThienSu = new NPC()
        {
            Id = 138,
            X = 160,
            Y = 159,
            Map = Id,
            Name = "Triệu Thiên Sư"
        };

        // mpc chuc phuc
        public static NPC CauPhucThienQuan = new NPC()
        {
            Id = 177,
            X = 182,
            Y = 197,
            Map = Id,
            Name = "Cầu Phúc Thiên Quan"
        };

        // npc bang
        public static NPC PhamThuanLe = new NPC()
        {
            Id = 163,
            X = 179,
            Y = 121,
            Map = Id,
            Name = "Phạm Thuần Lễ"
        };

        // npc x2
        public static NPC HongDaiQuy = new NPC()
        {
            Id = 157,
            X = 181,
            Y = 139,
            Map = Id,
            Name = "Hồng Đại Quý"
        };

        // thien kiep lau
        public static NPC PhoKiepSinh = new NPC()
        {
            Id = 163,
            X = 94,
            Y = 201,
            Map = Id,
            Name = "Phó Kiếp Sinh"
        };

        public static NPC NhiepChinh = new NPC()
        {
            Id = 131,
            X = 139,
            Y = 132,
            Map = Id,
            Name = "Nhiếp Chính"
        };

        // lo tam that
        public static NPC LoTamThat = new NPC()
        {
            Id = 12,
            X = 105,
            Y = 123,
            Map = Id,
            Name = "Lô Tam Thất"
        };

        // doan chinh thuan
        public static NPC DoanChinhThuan = new NPC()
        {
            Id = 16,
            X = 71,
            Y = 18,
            Map = Id,
            Name = "Đoàn Chính Thuần"
        };

        public static NPC GetNPCSuMon(int menpai)
        {
            if (menpai == MENPAI.DuongMon)
                return DuongDuc;
            if (menpai == MENPAI.MinhGiao)
                return ThachBao;
            if (menpai == MENPAI.ThienSon)
                return TrinhThanhSuong;
            if (menpai == MENPAI.TinhTuc)
                return HaiPhongTu;
            if (menpai == MENPAI.ThienLong)
                return PhaTham;
            if (menpai == MENPAI.TieuDao)
                return DamDaiTuVu;
            if (menpai == MENPAI.MoDung)
                return MoDungTruyen;
            if (menpai == MENPAI.NgaMy)
                return LoTamNuong;
            if (menpai == MENPAI.CaiBang)
                return GianNinh;
            if (menpai == MENPAI.ThieuLam)
                return TueDich;
            if (menpai == MENPAI.VoDang)
                return TruongHoach;
            if (menpai == MENPAI.QuyCoc)
                return DuongTiem;
            if (menpai == MENPAI.DaoHoa)
                return HoangThoiVu;
            return null;
        }

        // phái đường môn
        public static NPC DuongDuc = new NPC()
        {
            Id = 195,
            X = 166,
            Y = 139,
            Map = Id,
            Name = "Đường Dục"
        };

        // phái minh giáo
        public static NPC ThachBao = new NPC()
        {
            Id = 29,
            X = 166,
            Y = 136,
            Map = Id,
            Name = "Thạch Bảo"
        };

        // phái thiên sơn
        public static NPC TrinhThanhSuong = new NPC()
        {
            Id = 62,
            X = 166,
            Y = 133,
            Map = Id,
            Name = "Trình Thanh Sương"
        };

        // phái tinh túc
        public static NPC HaiPhongTu = new NPC()
        {
            Id = 28,
            X = 166,
            Y = 130,
            Map = Id,
            Name = "Hải Phong Tử"
        };

        // phái thiên long
        public static NPC PhaTham = new NPC()
        {
            Id = 95,
            X = 166,
            Y = 127,
            Map = Id,
            Name = "Phá Tham"
        };
        // phái tiêu dao
        public static NPC DamDaiTuVu = new NPC()
        {
            Id = 26,
            X = 166,
            Y = 124,
            Map = Id,
            Name = "Đàm Đài Tử Vũ"
        };

        //phái đào hoa
        public static NPC HoangThoiVu = new NPC()
        {
            Id = 231,
            X = 166,
            Y = 142,
            Map = Id,
            Name = "Hoàng Thời Vũ"
        };
        // phái mộ dung
        public static NPC MoDungTruyen = new NPC()
        {
            Id = 177,
            X = 154,
            Y = 138,
            Map = Id,
            Name = "Mộ Dung Truyền"
        };

        // phái ngamy
        public static NPC LoTamNuong = new NPC()
        {
            Id = 64,
            X = 154,
            Y = 135,
            Map = Id,
            Name = "Lộ Tam Nương"
        };
        // phái cái bang
        public static NPC GianNinh = new NPC()
        {
            Id = 27,
            X = 154,
            Y = 131,
            Map = Id,
            Name = "Giản Ninh"
        };

        // phái thiếu lâm
        public static NPC TueDich = new NPC()
        {
            Id = 25,
            X = 154,
            Y = 128,
            Map = Id,
            Name = "Tuệ Dịch"
        };

        // quy coc
        public static NPC DuongTiem = new NPC()
        {
            Id = 226,
            X = 154,
            Y = 142,
            Map = Id,
            Name = "Dương Tiệm"
        };

    

        // phái võ đang
        public static NPC TruongHoach = new NPC()
        {
            Id = 30,
            X = 154,
            Y = 124,
            Map = Id,
            Name = "Trương Hoạch"
        };
    }
}
