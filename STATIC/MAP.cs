using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _i
{
    class MAP
    {
        public static int PhungMinhTran { get; set; } = 580;
        public static int LacDuong { get; set; } = 0;
        public static int ToChau { get; set; } = 1;
        public static int DaiLy { get; set; } = 2;
        public static int GiamNguc { get; set; } = 194;
        public static int DiaPhu { get; set; } = 77;        
        public static int TungSon { get; set; } = 3;
        public static int ThaiHo { get; set; } = 4;
        public static int KinhHo { get; set; } = 5;
        public static int VoLuongSon { get; set; } = 6;
        public static int KiemCac { get; set; } = 7;
        public static int DonHoang { get; set; } = 8;
        public static int ThieuLam { get; set; } = 9;
        public static int MinhGiao { get; set; } = 11;
        public static int CaiBang { get; set; } = 10;
        public static int VoDang { get; set; } = 12;
        public static int ThienLong { get; set; } = 13;        
        public static int TieuDao { get; set; } = 14;
        public static int NgaMy { get; set; } = 15;
        public static int TinhTuc { get; set; } = 16;
        public static int ThienSon { get; set; } = 17;
        public static int QuyCoc { get; set; } = 678;
        public static int DaoHoa { get; set; } = 731;

        public static int DaoHoaNV { get; set; } = 732;
        public static int NhanBac { get; set; } = 19;
        public static int ThaoNguyen { get; set; } = 20;
        public static int LieuTay { get; set; } = 21;
        public static int TruongBachSon { get; set; } = 22;
        public static int HoanLongPhu { get; set; } = 23;
        public static int NhiHai { get; set; } = 24;
        public static int ThuongSon { get; set; } = 25;
        public static int ThachLam { get; set; } = 26;
        public static int NgocKhuye { get; set; } = 27;
        public static int NamChieu { get; set; } = 28;
        public static int MieuCuong { get; set; } = 29;
        public static int TayHo { get; set; } = 30;
        public static int LongTuyen { get; set; } = 31;
        public static int VoDi { get; set; } = 32;
        public static int MaiLinh { get; set; } = 33;
        public static int NamHai { get; set; } = 34;
        public static int QuynhChau { get; set; } = 35;
        public static int BienGioiTongLieu { get; set; } = 79;
        public static int TrucLam { get; set; } = 81;
        public static int ThaoLieuTruong { get; set; } = 199;
        public static int HoangLongDong { get; set; } = 216;
        public static int TanHoangDiaCungTang1 { get; set; } = 262;
        public static int TanHoangDiaCungTang2 { get; set; } = 263;
        public static int TanHoangDiaCungTang3 { get; set; } = 264;
        public static int TanHoangDiaCungTang4 { get; set; } = 292;
        public static int BinhThanhKyTran { get; set; } = 294;
        public static int MaNhaiDong { get; set; } = 213;
        public static int MoDung { get; set; } = 284;
        public static int DuongMon { get; set; } = 615;
        public static int CongDia { get; set; } = 153;
        public static int ThieuLamPhuBan { get; set; } = 182;
        public static int CaiBangPhuBan { get; set; } = 183;
        public static int MinhGiaoPhuBan { get; set; } = 184;
        public static int VoDangPhuBan { get; set; } = 185;
        public static int ThienLongPhuBan { get; set; } = 186;
        public static int TieuDaoPhuBan { get; set; } = 187;
        public static int NgaMyPhuBan { get; set; } = 188;
        public static int TinhTucPhuBan { get; set; } = 189;
        public static int DaoHoaPhuBan { get; set; } = 741;
        public static int ThienSonPhuBan { get; set; } = 190;
        public static int MoDungPhuBan { get; set; } = 289;
        public static int DuongMonPhuBan { get; set; } = 616;
        public static int NganNgaiTuyetNguyen { get; set; } = 229;
        public static int NhanNam { get; set; } = 18;
        public static int ThieuLamAcBa { get; set; } = 173;
        public static int NgaMyAcBa { get; set; } = 179;
        public static int TieuDaoAcBa { get; set; } = 178;
        public static int DuongMonAcBa { get; set; } = 618;
        public static int QuyCocAcBa { get; set; } = 681;
        public static int MinhGiaoAcBa { get; set; } = 175;
        public static int VoDangAcBa { get; set; } = 176;
        public static int TinhTucAcBa { get; set; } = 180;
        public static int ThienSonAcBa { get; set; } = 181;
        public static int CaiBangAcBa { get; set; } = 174;
        public static int ThienLongAcBa { get; set; } = 177;
        public static int MoDungAcBa { get; set; } = 288;
        public static int DaoHoaAcBa { get; set; } = 742;
        public static int TacKhauDoanhDia { get; set; } = 170;
        public static int TangKinhCac { get; set; } = 272;
        public static int ThanhThuSon { get; set; } = 201;
        public static int ThanhHoaCung { get; set; } = 256;
        public static int ThanhThuSonPhuBan { get; set; } = 232;
        public static int LauLan { get; set; } = 246;
        public static int ThucHaCoTran { get; set; } = 260;
        public static int HuyenVuDaoPhuBan { get; set; } = 268;
        public static int PhungHoangCoThanh { get; set; } = 280;
        public static int PhungHoangCoThanhPhuBan { get; set; } = 281;
        public static int PhieuMieuPhong { get; set; } = 261;
        public static int VanKiemCoc { get; set; } = 119;
        public static int VanKiemCocDem { get; set; } = 118;
        public static int LauLanBaoTang { get; set; } = 269;    
        public static int TamTaiHiepCoc { get; set; } = 653;
        public static int TamTaiHiepCocDon { get; set; } = 652;
        public static int TranLongKyCuoc { get; set; } = 61;
        public static int TuTuyetTrang { get; set; } = 291;
        public static int LoiDaiSinhTu { get; set; } = 546;
        public static int ConGiapLoiDai { get; set; } = 547;
        public static int YenTuO { get; set; } = 236;        
        public static int HuyetMo { get; set; } = 110;
        public static int ThuyLao { get; set; } = 66;
        public static int ThieuThatSon { get; set; } = 566;
        public static int HuyenVuDao { get; set; } = 112;               
        public static int DiaPhuDaiTheGioi { get; set; } = 777;
        public static int PhungMinhVuongLang { get; set; } = 600;
        public static int HuyenHai { get; set; } = 611;
        public static int ViemMaSon { get; set; } = 651;
        public static int LanHoanPhucDia { get; set; } = 677;
        public static int TamThanHuyenCanh { get; set; } = 662;
        public static int ThienThuDien { get; set; } = 663;
        public static int BatQuyLam { get; set; } = 664;
        public static int VoNhaiHai { get; set; } = 665;
        public static int ViemLaThien { get; set; } = 666;
        public static int HoangNgocSon { get; set; } = 667;
        public static int NongTruongDaTru { get; set; } = 231;
        public static int YếnTửỔĐêm { get; set; } = 114;
        public static int YếnTửỔNgày { get; set; } = 37;
        public static int TuHuyenTrang { get; set; } = 113;
        public static int TuHuyenTrangNgay { get; set; } = 36;
        public static int ThuongMangSon { get; set; } = 121;
        public static int ThuongMangSonNgay { get; set; } = 120;
        public static int LoiCoSon { get; set; } = 116;
        public static int LoiCoSonNgay { get; set; } = 117;
        public static int NhatPhamDuong { get; set; } = 115;
        public static int NhatPhamDuongNgay { get; set; } = 38;
        public static int TrânLongKỳCuộc { get; set; } = 550;
        public static int TặcKhấuDoanhĐịa { get; set; } = 551;

        public static int VanPhu { get; set; } = 699;
        public static int ThuongNgoBiCanh { get; set; } = 698;
        public static int ThienLongHuyenCanh { get; set; } = 300;
        public static int NhaiDuDao { get; set; } = 697;
        public static int MauDonUyen { get; set; } = 233;
        public static int TienTrang { get; set; } = 224;
        public static int QuyThi { get; set; } = 738;

        public static int BienKinh { get; set; } = 755;
        public static int KimLang { get; set; } = 762;

        
        public static int TungSonPhongThienDai { get; set; } = 234;
        
        public static int PhongChuanBiCapUngDuong { get; set; } = 724;
        public static int PhongChuanBiCapHoKhieu { get; set; } = 725;
        public static int PhongChuanBiCapLongDang { get; set; } = 726;
        public static int ChienTruongThiDau { get; set; } = 727;
        public static int VuTrucCuong { get; set; } = 770;

        public static int CoMo1 { get; set; } = 202;

        public static int CoDongTuocDai { get; set; } = 752; //97,97
        public static int ThienNhanHoangSon { get; set; } = 749; //70,70
        public static int LangVanDaiPhat { get; set; } = 750; //59,59
                                                              // public static int LangVanDaiPhat { get; set; } //59,59
        public static int ThienPhamTrai { get; set; } = 756;
        public static int BichDuanXuanLam { get; set; } = 769;
    }
}
