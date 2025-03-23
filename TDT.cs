using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Configuration;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Diagnostics;
using Microsoft.Win32;
using System.Globalization;
using System.Collections.Specialized;
using System.Net.NetworkInformation;
using System.Linq;
using System.Data;
using System.IO.Compression;
//using Finisar.SQLite;



namespace _i
{

    enum PlayerState : int
    {
        None,

    }

    class NPC
    {
        public NPC()
        {
            Id = 0;
        }
        public uint Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Map { get; set; }
        public string MapName { get; set; } = string.Empty;

        public string MD { get; set; } = "000";
        public string Name { get; set; } = string.Empty;

        

        public static NPC MID = new NPC()
        {
            Id = 191,
            X = 255,
            Y = 320,
            Map = LACDUONG.Id
        };

        public static NPC TranVinhNhan = new NPC()
        {
            Id = 18,
            X = 270,
            Y = 232,
            Map = LACDUONG.Id
        };

        public static NPC ThamTrinh = new NPC()
        {
            Id = 143,
            X = 249,
            Y = 172,
            Map = MAP.ToChau
        };

        public static NPC HongDaiQuy = new NPC()
        {
            Id = 157,
            X = 181,
            Y = 139,
            Map = MAP.DaiLy
        };

        public static NPC CungThaiVan = new NPC()
        {
            Id = 153,
            X = 172,
            Y = 122,
            Map = MAP.DaiLy
        };

        public static NPC VuongTichTan = new NPC()
        {
            Id = 142,
            X = 366,
            Y = 228,
            Map = LACDUONG.Id
        };

        public static NPC HoaHachCan = new NPC()
        {
            Id = 21,
            X = 71,
            Y = 28,
            Map = MAP.DaiLy
        };

        public static NPC TrieuThienSu = new NPC()
        {
            Id = 138,
            X = 160,
            Y = 158,
            Map = MAP.DaiLy
        };

        #region NPC Bái Sư
        public static NPC DuongXichPhong = new NPC()
        {
            Id = 0,
            X = 77,
            Y = 34,
            Map = MAP.DuongMon
        };

        public static NPC MoDungKiet = new NPC()
        {
            Id = 13,
            X = 48,
            Y = 144,
            Map = MAP.MoDung
        };

        public static NPC HanTheTrung = new NPC()
        {
            Id = 1,
            X = 95,
            Y = 75,
            Map = MAP.TinhTuc
        };

        public static NPC ToTinhHa = new NPC()
        {
            Id = 0,
            X = 125,
            Y = 144,
            Map = MAP.TieuDao
        };

        public static NPC HuyenTich = new NPC()
        {
            Id = 4,
            X = 89,
            Y = 72,
            Map = MAP.ThieuLam
        };

        public static NPC MaiKiem = new NPC()
        {
            Id = 0,
            X = 91,
            Y = 44,
            Map = MAP.ThienSon
        };

        public static NPC BanNhan = new NPC()
        {
            Id = 0,
            X = 96,
            Y = 66,
            Map = MAP.ThienLong
        };

        public static NPC LyThapNhiNuong = new NPC()
        {
            Id = 1,
            X = 96,
            Y = 51,
            Map = MAP.NgaMy
        };

        public static NPC TruongHuyenTo = new NPC()
        {
            Id = 0,
            X = 77,
            Y = 85,
            Map = MAP.VoDang
        };

        public static NPC LaSuTuong = new NPC()
        {
            Id = 11,
            X = 108,
            Y = 56,
            Map = MAP.MinhGiao
        };

        public static NPC TranCoNhan = new NPC()
        {
            Id = 23,
            X = 91,
            Y = 98,
            Map = MAP.CaiBang
        };
        #endregion

        public static NPC PhuManNghi = new NPC()
        {
            Id = 13176,
            X = 159,
            Y = 54,
            Map = MAP.PhieuMieuPhong,
        };
        public static NPC CapDaiBa = new NPC()
        {
            Id = 13157,
            X = 124,
            Y = 86,
            Map = MAP.PhieuMieuPhong,
        };
        public static NPC TangThoCong = new NPC()
        {
            Id = 13158,
            X = 41,
            Y = 105,
            Map = MAP.PhieuMieuPhong,
        };
        public static NPC OLaoDai = new NPC()
        {
            Id = 13159,
            X = 117,
            Y = 49,
            Map = MAP.PhieuMieuPhong,
        };
        public static NPC TrinhThanhSuong = new NPC()
        {
            Id = 56,
            X = 193,
            Y = 224,
            Map = MAP.LauLan,
        };
        public static NPC LinhThuocTienTu = new NPC()
        {
            Id = 160,
            X = 223,
            Y = 138,
            Map = MAP.TayHo,
        };
        public static NPC Aly = new NPC()
        {
            Id = 173,
            X = 185,
            Y = 65,
            Map = MAP.DaiLy,
        };
        public static NPC BaCaiLy = new NPC()
        {
            Id = 174,
            X = 182,
            Y = 66,
            Map = MAP.DaiLy,
        };
        public static NPC HuyenChung = new NPC()
        {
            Id = 24,
            X = 98,
            Y = 145,
            Map = MAP.ThieuLam,
        };
        public static NPC AuDuongQua = new NPC()
        {
            Id = 20,
            X = 93,
            Y = 152,
            Map = MAP.CaiBang,
        };
        public static NPC ThacCang = new NPC()
        {
            Id = 29,
            X = 95,
            Y = 161,
            Map = MAP.MinhGiao,
        };
        public static NPC TieuThienDat = new NPC()
        {
            Id = 21,
            X = 100,
            Y = 181,
            Map = MAP.VoDang,
        };
        public static NPC HoTuTruongLao = new NPC()
        {
            Id = 25,
            X = 99,
            Y = 142,
            Map = MAP.ThienLong,
        };
        public static NPC CongDaTuTruong = new NPC()
        {
            Id = 18,
            X = 44,
            Y = 125,
            Map = MAP.TieuDao,
        };
        public static NPC LieuTamMuoi = new NPC()
        {
            Id = 29,
            X = 94,
            Y = 147,
            Map = MAP.NgaMy,
        };
        public static NPC ThienToanTu = new NPC()
        {
            Id = 21,
            X = 100,
            Y = 143,
            Map = MAP.TinhTuc,
        };
        public static NPC DangBa = new NPC()
        {
            Id = 33,
            X = 96,
            Y = 148,
            Map = MAP.ThienSon,
        };
        public static NPC CongDaKhon = new NPC()
        {
            Id = 7,
            X = 159,
            Y = 163,
            Map = MAP.MoDung,
        };
        public static NPC DuongMoTuong = new NPC()
        {
            Id = 7,
            X = 152,
            Y = 154,
            Map = MAP.DuongMon,
        };

        #region lecaotri
        public static NPC ThanTinhYeu = new NPC()
        {
            Id = 203,
            X = 273,
            Y = 242,
            Map = LACDUONG.Id,
        };
        public static NPC BuiHoaHong = new NPC()
        {
            Id = 5120,
            X = 256,
            Y = 246,
            Map = LACDUONG.Id,
        };
        public static NPC MaiKhoiTienTu = new NPC()
        {
            Id = 12498,
            X = 256,
            Y = 246,
            Map = LACDUONG.Id,
        };

        public static NPC CauPhucThienQuan = new NPC()
        {
            Id = 178,
            X = 180,
            Y = 197,
            Map = MAP.DaiLy
        };

        //npc Bach Hoa Duyen
        public static NPC LoTamThat = new NPC()
        {
            Id = 12,
            X = 105,
            Y = 123,
            Map = MAP.DaiLy
        };
        public static NPC doanchinhthuan = new NPC()
        {
            Id = 16,
            X = 71,
            Y = 18,
            Map = MAP.DaiLy
        };
        public static NPC trithanhdaisu = new NPC()
        {
            Id = 34,
            X = 176,
            Y = 192,
            Map = LACDUONG.Id
        };
        public static NPC tothuc = new NPC()
        {
            Id = 2,
            X = 166,
            Y = 311,
            Map = MAP.ToChau
        };

        //npc Map bang hoi
        //quynhchau
        public static NPC QuynhChauChinhDong = new NPC()
        {
            Id = 120,
            X = 243,
            Y = 150,
            Map = 35
        };

        public static NPC QuynhChauDongBac = new NPC()
        {
            Id = 119,
            X = 273,
            Y = 52,
            Map = 35
        };

        public static NPC QuynhChauChinhTay = new NPC()
        {
            Id = 110,
            X = 80,
            Y = 139,
            Map = 35
        };
        //nam vuc
        public static NPC NamVucDongBac = new NPC()
        {
            Id = 116,
            X = 293,
            Y = 59,
            Map = 34
        };
        public static NPC NamVucTayNam = new NPC()
        {
            Id = 117,
            X = 107,
            Y = 228,
            Map = 34
        };
        public static NPC NamVucChinhBac = new NPC()
        {
            Id = 109,
            X = 134,
            Y = 40,
            Map = 34
        };
        //vodi
        public static NPC VoDiDongBac = new NPC()
        {
            Id = 151,
            X = 256,
            Y = 39,
            Map = 32
        };
        public static NPC VoDiChinhNam = new NPC()
        {
            Id = 150,
            X = 113,
            Y = 281,
            Map = 32
        };
        public static NPC VoDiChinhTay = new NPC()
        {
            Id = 142,
            X = 92,
            Y = 171,
            Map = 32
        };
        //mailinh
        public static NPC MaiLinhDongBac = new NPC()
        {
            Id = 159,
            X = 271,
            Y = 37,
            Map = 33
        };
        public static NPC MaiLinhTayBac = new NPC()
        {
            Id = 151,
            X = 31,
            Y = 90,
            Map = 33
        };
        public static NPC MaiLinhChinhDong = new NPC()
        {
            Id = 158,
            X = 283,
            Y = 236,
            Map = 33
        };
        //tayho
        public static NPC TayHoChinhDong = new NPC()
        {
            Id = 40,
            X = 262,
            Y = 232,
            Map = 30
        };
        public static NPC TayHoTayNam = new NPC()
        {
            Id = 39,
            X = 45,
            Y = 267,
            Map = 30
        };
        public static NPC TayHoChinhTay = new NPC()
        {
            Id = 23,
            X = 39,
            Y = 139,
            Map = 30
        };
        //longtuyen
        public static NPC LongTuyenDongNam = new NPC()
        {
            Id = 143,
            X = 218,
            Y = 282,
            Map = 31
        };
        public static NPC LongTuyenChinhBac = new NPC()
        {
            Id = 141,
            X = 63,
            Y = 33,
            Map = 31
        };
        public static NPC LongTuyenChinhTay = new NPC()
        {
            Id = 142,
            X = 36,
            Y = 121,
            Map = 31
        };
        //nhannam
        public static NPC NhanNamChinhDong = new NPC()
        {
            Id = 140,
            X = 283,
            Y = 113,
            Map = 18
        };
        public static NPC NhanNamChinhBac = new NPC()
        {
            Id = 139,
            X = 102,
            Y = 36,
            Map = 18
        };
        public static NPC NhanNamChinhNam = new NPC()
        {
            Id = 138,
            X = 72,
            Y = 284,
            Map = 18
        };
        //NhanBac
        public static NPC NhanBacDongBac = new NPC()
        {
            Id = 121,
            X = 235,
            Y = 24,
            Map = 19
        };
        public static NPC NhanBacTayBac = new NPC()
        {
            Id = 120,
            X = 116,
            Y = 30,
            Map = 19
        };
        public static NPC NhanBacChinhTay = new NPC()
        {
            Id = 115,
            X = 32,
            Y = 128,
            Map = 19
        };
        //ThaoNguyen
        public static NPC ThaoNguyenTayNam = new NPC()
        {
            Id = 173,
            X = 97,
            Y = 282,
            Map = 20
        };
        public static NPC ThaoNguyenChinhDong = new NPC()
        {
            Id = 174,
            X = 271,
            Y = 192,
            Map = 20
        };
        public static NPC ThaoNguyenChinhTay = new NPC()
        {
            Id = 166,
            X = 66,
            Y = 202,
            Map = 20
        };
        //lieutay
        public static NPC LieuTayDongNam = new NPC()
        {
            Id = 152,
            X = 278,
            Y = 258,
            Map = 21
        };
        public static NPC LieuTayTayBac = new NPC()
        {
            Id = 142,
            X = 75,
            Y = 35,
            Map = 21
        };
        public static NPC LieuTayChinhTay = new NPC()
        {
            Id = 151,
            X = 40,
            Y = 142,
            Map = 21
        };
        //TruongBachSon
        public static NPC TruongBachSonChinhNam = new NPC()
        {
            Id = 148,
            X = 217,
            Y = 282,
            Map = 22
        };
        public static NPC TruongBachSonTayBac = new NPC()
        {
            Id = 119,
            X = 39,
            Y = 63,
            Map = 22
        };
        public static NPC TruongBachSonChinhDong = new NPC()
        {
            Id = 147,
            X = 280,
            Y = 155,
            Map = 22
        };

        //HoangLongPhu
        public static NPC HoangLongPhuChinhDong = new NPC()
        {
            Id = 143,
            X = 290,
            Y = 116,
            Map = 23
        };
        public static NPC HoangLongPhuTayBac = new NPC()
        {
            Id = 114,
            X = 28,
            Y = 54,
            Map = 23
        };
        public static NPC HoangLongPhuDongNam = new NPC()
        {
            Id = 144,
            X = 254,
            Y = 286,
            Map = 23
        };


        //NhiHai
        public static NPC NhiHaiChinhDong = new NPC()
        {
            Id = 46,
            X = 286,
            Y = 166,
            Map = 24
        };
        public static NPC NhiHaiChinhNam = new NPC()
        {
            Id = 45,
            X = 173,
            Y = 284,
            Map = 24
        };
        public static NPC NhiHaiChinhTay = new NPC()
        {
            Id = 43,
            X = 34,
            Y = 100,
            Map = 24
        };


        //ThuongSon
        public static NPC ThuongSonTranTay = new NPC()
        {
            Id = 70,
            X = 37,
            Y = 172,
            Map = 25
        };
        public static NPC ThuongSonChinhDong = new NPC()
        {
            Id = 161,
            X = 295,
            Y = 154,
            Map = 25
        };
        public static NPC ThuongSonChinhNam = new NPC()
        {
            Id = 160,
            X = 146,
            Y = 285,
            Map = 25
        };


        //ThachLam
        public static NPC ThachLamChinhBac = new NPC()
        {
            Id = 146,
            X = 227,
            Y = 37,
            Map = 26
        };
        public static NPC ThachLamChinhNam = new NPC()
        {
            Id = 138,
            X = 278,
            Y = 281,
            Map = 26
        };
        public static NPC ThachLamChinhTay = new NPC()
        {
            Id = 147,
            X = 45,
            Y = 178,
            Map = 26
        };

        //NgocKhe
        public static NPC NgocKheTayNam = new NPC()
        {
            Id = 136,
            X = 33,
            Y = 251,
            Map = 27
        };
        public static NPC NgocKheChinhBac = new NPC()
        {
            Id = 143,
            X = 178,
            Y = 38,
            Map = 27
        };
        public static NPC NgocKheChinhNam = new NPC()
        {
            Id = 142,
            X = 197,
            Y = 280,
            Map = 27
        };




        //NamChieu
        public static NPC NamChieuTayBac = new NPC()
        {
            Id = 127,
            X = 96,
            Y = 41,
            Map = 28
        };
        public static NPC NamChieuDongNam = new NPC()
        {
            Id = 128,
            X = 273,
            Y = 242,
            Map = 28
        };
        public static NPC NamChieuTayNam = new NPC()
        {
            Id = 120,
            X = 39,
            Y = 254,
            Map = 28
        };



        //MieuCuong
        public static NPC MieuCuongDongBac = new NPC()
        {
            Id = 157,
            X = 251,
            Y = 46,
            Map = 29
        };

        public static NPC MieuCuongTayNam = new NPC()
        {
            Id = 156,
            X = 38,
            Y = 251,
            Map = 29
        };

        public static NPC MieuCuongChinhDong = new NPC()
        {
            Id = 117,
            X = 281,
            Y = 160,
            Map = 29
        };





        #endregion



    }

   

    class POINT
    {
        public static int[,] CongDia = new int[5, 2] { { 13, 29 }, { 32, 34 }, { 48, 34 }, { 32, 20 }, { 40, 18 }, };
       

        public static int[,] BangHoiDua = new int[6, 2] { { 45, 82 }, {45, 75 }, {49, 68 }, { 39, 66 }, { 51, 64 }, { 46, 61 } };

        public static int[,] Get(int map)
        {            
            return new int[0, 0];
        }
    }

    enum VirtualKeyStates : int
    {
        VK_LBUTTON = 0x01,
        VK_RBUTTON = 0x02,
        VK_CANCEL = 0x03,
        VK_MBUTTON = 0x04,
        //
        VK_XBUTTON1 = 0x05,
        VK_XBUTTON2 = 0x06,
        //
        VK_BACK = 0x08,
        VK_TAB = 0x09,
        //
        VK_CLEAR = 0x0C,
        VK_RETURN = 0x0D,
        //
        VK_SHIFT = 0x10,
        VK_CONTROL = 0x11,
        VK_MENU = 0x12,
        VK_PAUSE = 0x13,
        VK_CAPITAL = 0x14,
        //
        VK_KANA = 0x15,
        VK_HANGEUL = 0x15,  /* old name - should be here for compatibility */
        VK_HANGUL = 0x15,
        VK_JUNJA = 0x17,
        VK_FINAL = 0x18,
        VK_HANJA = 0x19,
        VK_KANJI = 0x19,
        //
        VK_ESCAPE = 0x1B,
        //
        VK_CONVERT = 0x1C,
        VK_NONCONVERT = 0x1D,
        VK_ACCEPT = 0x1E,
        VK_MODECHANGE = 0x1F,
        //
        VK_SPACE = 0x20,
        VK_PRIOR = 0x21,
        VK_NEXT = 0x22,
        VK_END = 0x23,
        VK_HOME = 0x24,
        VK_LEFT = 0x25,
        VK_UP = 0x26,
        VK_RIGHT = 0x27,
        VK_DOWN = 0x28,
        VK_SELECT = 0x29,
        VK_PRINT = 0x2A,
        VK_EXECUTE = 0x2B,
        VK_SNAPSHOT = 0x2C,
        VK_INSERT = 0x2D,
        VK_DELETE = 0x2E,
        VK_HELP = 0x2F,
        //
        VK_LWIN = 0x5B,
        VK_RWIN = 0x5C,
        VK_APPS = 0x5D,
        //
        VK_SLEEP = 0x5F,
        //
        VK_NUMPAD0 = 0x60,
        VK_NUMPAD1 = 0x61,
        VK_NUMPAD2 = 0x62,
        VK_NUMPAD3 = 0x63,
        VK_NUMPAD4 = 0x64,
        VK_NUMPAD5 = 0x65,
        VK_NUMPAD6 = 0x66,
        VK_NUMPAD7 = 0x67,
        VK_NUMPAD8 = 0x68,
        VK_NUMPAD9 = 0x69,
        VK_MULTIPLY = 0x6A,
        VK_ADD = 0x6B,
        VK_SEPARATOR = 0x6C,
        VK_SUBTRACT = 0x6D,
        VK_DECIMAL = 0x6E,
        VK_DIVIDE = 0x6F,
        VK_F1 = 0x70,
        VK_F2 = 0x71,
        VK_F3 = 0x72,
        VK_F4 = 0x73,
        VK_F5 = 0x74,
        VK_F6 = 0x75,
        VK_F7 = 0x76,
        VK_F8 = 0x77,
        VK_F9 = 0x78,
        VK_F10 = 0x79,
        VK_F11 = 0x7A,
        VK_F12 = 0x7B,
        VK_F13 = 0x7C,
        VK_F14 = 0x7D,
        VK_F15 = 0x7E,
        VK_F16 = 0x7F,
        VK_F17 = 0x80,
        VK_F18 = 0x81,
        VK_F19 = 0x82,
        VK_F20 = 0x83,
        VK_F21 = 0x84,
        VK_F22 = 0x85,
        VK_F23 = 0x86,
        VK_F24 = 0x87,
        //
        VK_NUMLOCK = 0x90,
        VK_SCROLL = 0x91,
        //
        VK_OEM_NEC_EQUAL = 0x92,   // '=' key on numpad
        //
        VK_OEM_FJ_JISHO = 0x92,   // 'Dictionary' key
        VK_OEM_FJ_MASSHOU = 0x93,   // 'Unregister word' key
        VK_OEM_FJ_TOUROKU = 0x94,   // 'Register word' key
        VK_OEM_FJ_LOYA = 0x95,   // 'Left OYAYUBI' key
        VK_OEM_FJ_ROYA = 0x96,   // 'Right OYAYUBI' key
        //
        VK_LSHIFT = 0xA0,
        VK_RSHIFT = 0xA1,
        VK_LCONTROL = 0xA2,
        VK_RCONTROL = 0xA3,
        VK_LMENU = 0xA4,
        VK_RMENU = 0xA5,
        //
        VK_BROWSER_BACK = 0xA6,
        VK_BROWSER_FORWARD = 0xA7,
        VK_BROWSER_REFRESH = 0xA8,
        VK_BROWSER_STOP = 0xA9,
        VK_BROWSER_SEARCH = 0xAA,
        VK_BROWSER_FAVORITES = 0xAB,
        VK_BROWSER_HOME = 0xAC,
        //
        VK_VOLUME_MUTE = 0xAD,
        VK_VOLUME_DOWN = 0xAE,
        VK_VOLUME_UP = 0xAF,
        VK_MEDIA_NEXT_TRACK = 0xB0,
        VK_MEDIA_PREV_TRACK = 0xB1,
        VK_MEDIA_STOP = 0xB2,
        VK_MEDIA_PLAY_PAUSE = 0xB3,
        VK_LAUNCH_MAIL = 0xB4,
        VK_LAUNCH_MEDIA_SELECT = 0xB5,
        VK_LAUNCH_APP1 = 0xB6,
        VK_LAUNCH_APP2 = 0xB7,
        //
        VK_OEM_1 = 0xBA,   // ';:' for US
        VK_OEM_PLUS = 0xBB,   // '+' any country
        VK_OEM_COMMA = 0xBC,   // ',' any country
        VK_OEM_MINUS = 0xBD,   // '-' any country
        VK_OEM_PERIOD = 0xBE,   // '.' any country
        VK_OEM_2 = 0xBF,   // '/?' for US
        VK_OEM_3 = 0xC0,   // '`~' for US
        //
        VK_OEM_4 = 0xDB,  //  '[{' for US
        VK_OEM_5 = 0xDC,  //  '\|' for US
        VK_OEM_6 = 0xDD,  //  ']}' for US
        VK_OEM_7 = 0xDE,  //  ''"' for US
        VK_OEM_8 = 0xDF,
        //
        VK_OEM_AX = 0xE1,  //  'AX' key on Japanese AX kbd
        VK_OEM_102 = 0xE2,  //  "<>" or "\|" on RT 102-key kbd.
        VK_ICO_HELP = 0xE3,  //  Help key on ICO
        VK_ICO_00 = 0xE4,  //  00 key on ICO
        //
        VK_PROCESSKEY = 0xE5,
        //
        VK_ICO_CLEAR = 0xE6,
        //
        VK_PACKET = 0xE7,
        //
        VK_OEM_RESET = 0xE9,
        VK_OEM_JUMP = 0xEA,
        VK_OEM_PA1 = 0xEB,
        VK_OEM_PA2 = 0xEC,
        VK_OEM_PA3 = 0xED,
        VK_OEM_WSCTRL = 0xEE,
        VK_OEM_CUSEL = 0xEF,
        VK_OEM_ATTN = 0xF0,
        VK_OEM_FINISH = 0xF1,
        VK_OEM_COPY = 0xF2,
        VK_OEM_AUTO = 0xF3,
        VK_OEM_ENLW = 0xF4,
        VK_OEM_BACKTAB = 0xF5,
        //
        VK_ATTN = 0xF6,
        VK_CRSEL = 0xF7,
        VK_EXSEL = 0xF8,
        VK_EREOF = 0xF9,
        VK_PLAY = 0xFA,
        VK_ZOOM = 0xFB,
        VK_NONAME = 0xFC,
        VK_PA1 = 0xFD,
        VK_OEM_CLEAR = 0xFE
    }

    class TDT
    {
        public static bool PointInTriangleEx(Point p, Point p0, Point p1, Point p2)
        {
            var s = p0.Y * p2.X - p0.X * p2.Y + (p2.Y - p0.Y) * p.X + (p0.X - p2.X) * p.Y;
            var t = p0.X * p1.Y - p0.Y * p1.X + (p0.Y - p1.Y) * p.X + (p1.X - p0.X) * p.Y;

            if ((s < 0) != (t < 0))
                return false;

            var A = -p1.Y * p2.X + p0.Y * (p2.X - p1.X) + p0.X * (p1.Y - p2.Y) + p1.X * p2.Y;

            return A < 0 ?
                    (s <= 0 && s + t >= A) :
                    (s >= 0 && s + t <= A);
        }

        static bool ptInTriangle(Point p, Point p0, Point p1, Point p2)
        {
            var dX = p.X - p2.X;
            var dY = p.Y - p2.Y;
            var dX21 = p2.X - p1.X;
            var dY12 = p1.Y - p2.Y;
            var D = dY12 * (p0.X - p2.X) + dX21 * (p0.Y - p2.Y);
            var s = dY12 * dX + dX21 * dY;
            var t = (p2.Y - p0.Y) * dX + (p0.X - p2.X) * dY;
            if (D < 0) return s <= 0 && t <= 0 && s + t >= D;
            return s >= 0 && t >= 0 && s + t <= D;
        }


        public static bool is_point_inside_trigon(Point s, Point a, Point b, Point c)
        {
            //return PointInTriangleEx(s, a, b, c);
            int as_X = s.X - a.X;
            int as_Y = s.Y - a.Y;

            bool s_ab = (b.X - a.X) * as_Y - (b.Y - a.Y) * as_X > 0;

            if ((c.X - a.X) * as_Y - (c.Y - a.Y) * as_X > 0 == s_ab) return false;

            if ((c.X - b.X) * (s.Y - b.Y) - (c.Y - b.Y) * (s.X - b.X) > 0 != s_ab) return false;


            return true;
        }

        static float sign(Point p1, Point p2, Point p3)
        {
            return (p1.X - p3.X) * (p2.Y - p3.Y) - (p2.Y - p3.Y) * (p1.Y - p3.Y);
        }

   

        //quad
        public static bool is_point_inside_quad(Point s, Point a, Point b, Point c, Point d)
        {
            return (is_point_inside_trigon(s, a, b, c) || is_point_inside_trigon(s, a, c, d));
        }

        public static bool is_point_inside_penta(Point s, Point a, Point b, Point c, Point d, Point e)
        {
            return (is_point_inside_quad(s, a, b, c, d) || is_point_inside_quad(s, a, c, d, e));
        }

        public static int[] get_phuong_trinh_duong_thang(Point x, Point y)
        {
            int a = x.Y - y.Y;
            int b = y.X - x.X;
            int c = a * (-x.X) + b * (-x.Y);
            return new int[] { a, b, c };
        }

        public static int[] get_phuong_trinh_duong_thang_vuong_goc(Point m, Point x, Point y)
        {
            int a = x.Y - y.Y;
            int b = y.X - x.X;
            int c = -b * (-x.X) + a * (-x.Y);
            return new int[] { -b, a, c };
        }

        public static float tim_khoang_cach(Point m, Point x, Point y)
        {
            int[] duongthang = get_phuong_trinh_duong_thang(x, y);
            return (float)(Math.Abs((duongthang[0] * m.X + duongthang[1] * m.Y + duongthang[2])) / (Math.Sqrt(Math.Pow(duongthang[0], 2) + Math.Pow(duongthang[1], 2))));
        }


        ////////////////////////////////////////////////////////////////////////////////////
        //Le Cao Tri
        //public static void ReadTDTDB()
        //{
        //    try
        //    {
        //        if (File.Exists(Global.TDTDB))
        //        {
        //            SQLiteConnection sQLiteConnection = new SQLiteConnection("Data Source=" + Global.TDTDB);
        //            sQLiteConnection.Open();
        //            if (sQLiteConnection.State == ConnectionState.Open)
        //            {
        //                SQLiteCommand sQLiteCommand = new SQLiteCommand("SELECT * FROM AIInfo;", sQLiteConnection);
        //                SQLiteDataReader reader = sQLiteCommand.ExecuteReader();
        //                DataTable dataTable = new DataTable();
        //                dataTable.Load(reader);
        //                if (dataTable.Rows.Count > 0)
        //                {
        //                    foreach (DataRow dataRow in dataTable.Rows)
        //                    {
        //                        Tdtdb dbTDT = new Tdtdb();
        //                        dbTDT.id = int.Parse(dataRow["id"].ToString());
        //                        dbTDT.name = dataRow["name"].ToString();
        //                        dbTDT.val= dataRow["val"].ToString();
        //                        dbTDT.type= int.Parse(dataRow["type"].ToString());
        //                        Global.DbMain = dbTDT;
        //                    }
        //                }                       
        //                dataTable.Dispose();
        //                sQLiteConnection.Close();
        //            }
        //        }
        //    }
        //    catch 
        //    {

        //    }
        //}


        ////////////////////////////////////////////////////////////////////////////////////
        public static bool CheckForSQLInjection(string userInput)
        {
            bool result = false;
            string[] array = new string[]
            {
                "--",
                "=",
                "'1",
                "1'",
                ";--",
                ";",
                "/*",
                "*/",
                "@@",
                "union",
                "nchar",
                "varchar",
                "nvarchar",
                "alter",
                "delete",
                "drop",
                "insert",
                "select",
                "sysobjects",
                "syscolumns",
                "table"
            };
            string text = userInput.Replace("'", "''");
            for (int i = 0; i <= array.Length - 1; i++)
            {
                if (text.IndexOf(array[i], StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    result = true;
                }
            }
            return result;
        }
        public static bool IsFilePathValid(string a_path)
        {
            if (a_path.Trim() == string.Empty)
            {
                return false;
            }
            string pathname;
            string filename;
            try
            {
                pathname = Path.GetPathRoot(a_path);
                filename = Path.GetFileName(a_path);
            }
            catch (ArgumentException)
            {
                return false;
            }
            if (filename.Trim() == string.Empty)
            {
                return false;
            }
            if (pathname.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
            {
                return false;
            }
            if (filename.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
            {
                return false;
            }
            return true;
        }

        public static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string RandomEmptyString()
        {
            int length = random.Next(1, 10);
            const string chars = " ";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string StringBetween(string STR, string FirstString, string LastString)
        {
            string FinalString = null;
            int Pos1 = STR.IndexOf(FirstString);
            if(Pos1 != -1)
            {             
                FinalString = STR.Substring(Pos1 + FirstString.Length);
                if (string.IsNullOrEmpty(LastString))
                {
                    return FinalString;
                }
                int Pos2 = FinalString.IndexOf(LastString);
                if (Pos2 != -1)
                {                   
                    FinalString = FinalString.Substring(0, Pos2);
                }
                else
                {
                    FinalString = null;
                }
               
                //if (Pos1 >= 0 && Pos2 - Pos1 >= 1)
                //{
                //    FinalString = STR.Substring(Pos1, Pos2 - Pos1);
                //}
            }
            return FinalString;
        }

        public static Dictionary<string, string> TLBBDIC = new Dictionary<string, string>
        {
            { "#{XSLC_130831_01}", "Duyên khởi vô lượng" },
        };

        public static Dictionary<string, string> DicVatPhamNhiemVu = new Dictionary<string, string>();

        public class Menpai
        {
           
            public static int ThieuLam = 1;
            public static int MinhGiao = 2;
            public static int CaiBang = 3;
            public static int VoDang = 4;
            public static int NgaMy = 5;
            public static int TinhTuc = 6;
            public static int ThienLong = 7;
            public static int ThienSon = 8;
            public static int TieuDao = 9;
            public static int MoDung = 32;
            public static int DuongMon = 37;
            public static int QuyCoc = 53;
            public static int DaoHoa = 54;
            public static int KhongCo = 0;
        }

        public static string FormatMoney(int value)
        {
            if (value == 0)
                return "0";
            return string.Format("{0:#,###}", value);
        }

        public static string FormatMoney(double value)
        {
            if (value == 0)
                return "0";
            return string.Format("{0:#,###}", value);
        }

        public static void MoveToEnd(TextBox txt)
        {
            txt.Select(txt.Text.Length, 0);
            txt.Focus();
            txt.ScrollToCaret();
        }

        public static void MoveToBeg(TextBox txt)
        {
            txt.Select(0, 0);
            txt.Focus();
            txt.ScrollToCaret();
        }

        public static string GetMACAddress()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            String sMacAddress = string.Empty;
            foreach (NetworkInterface adapter in nics)
            {
                if (sMacAddress == String.Empty)// only return MAC Address from first card
                {
                    IPInterfaceProperties properties = adapter.GetIPProperties();
                    sMacAddress = adapter.GetPhysicalAddress().ToString();
                }
            } return sMacAddress;
        }

        /// <summary>
        /// Sending file via multipart\form-data
        /// </summary>
        /// <param name="url">URL for send</param>
        /// <param name="file">Local file path</param>
        /// <param name="paramName">Request file param</param>
        /// <param name="contentType">Content-Type file headr</param>
        /// <param name="nvc">Additional post params</param>
        public static string HttpUploadFile(string url, string file, string paramName, string contentType, NameValueCollection nvc)
        {
            //delimeter
            var boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");

            //creating request
            var wr = (HttpWebRequest)WebRequest.Create(url);
            wr.ContentType = "multipart/form-data; boundary=" + boundary;
            wr.Method = "POST";
            wr.KeepAlive = true;
            wr.ServicePoint.Expect100Continue = false;

            //sending request
            using (var requestStream = wr.GetRequestStream())
            {
                using (var requestWriter = new StreamWriter(requestStream, Encoding.UTF8))
                {
                    //params
                    const string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
                    foreach (string key in nvc.Keys)
                    {
                        requestWriter.Write("\r\n" + boundary + "\r\n");
                        requestWriter.Write(String.Format(formdataTemplate, key, nvc[key]));
                    }
                    requestWriter.Write("\r\n" + boundary + "\r\n");

                    //file header
                    const string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
                    requestWriter.Write(String.Format(headerTemplate, paramName, file, contentType));

                    //file content
                    using (var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read))
                    {
                        //fileStream.CopyTo(requestStream);
                        //requestWriter.Write(ToArr(fileStream));
                    }

                    requestWriter.Write("\r\n--" + boundary + "--\r\n");
                }
            }
            //reading response
            try
            {
                using (var wresp = (HttpWebResponse)wr.GetResponse())
                {
                    if (wresp.StatusCode == HttpStatusCode.OK)
                    {
                        using (var responseStream = wresp.GetResponseStream())
                        {
                            if (responseStream == null)
                                return null;
                            using (var responseReader = new StreamReader(responseStream))
                            {
                                return responseReader.ReadToEnd();
                            }
                        }
                    }

                    throw new ApplicationException("Error while upload files. Server status code: " + wresp.StatusCode.ToString());
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error while uploading file", ex);
            }
        }

        public static void CopyTo(Stream input, Stream output)
        {
            // This method exists only in .NET 4 and higher

            byte[] buffer = new byte[4 * 1024];
            int bytesRead;

            while ((bytesRead = input.Read(buffer, 0, buffer.Length)) != 0)
            {
                output.Write(buffer, 0, bytesRead);
            }
        }

        public static byte[] ToArr(Stream input)
        {
            Stream s = input;
            int streamEnd = Convert.ToInt32(s.Length);
            byte[] buffer = new byte[streamEnd];
            s.Read(buffer, 0, streamEnd);
            return buffer;
        }

        public static readonly char[] Unicodes = new char[] 
        {
            '\u0000', '\u0001', '\u1EB2', '\u0003', '\u0004', '\u1EB4', '\u1EAA', '\u0007', 
            '\u0008', '\u0009', '\u000A', '\u000B', '\u000C', '\u000D', '\u000E', '\u000F', 
            '\u0010', '\u0011', '\u0012', '\u0013', '\u1EF6', '\u0015', '\u0016', '\u0017', 
            '\u0018', '\u1EF8', '\u001A', '\u001B', '\u001C', '\u001D', '\u1EF4', '\u001F', 
            '\u0020', '\u0021', '\u0022', '\u0023', '\u0024', '\u0025', '\u0026', '\u0027', 
            '\u0028', '\u0029', '\u002A', '\u002B', '\u002C', '\u002D', '\u002E', '\u002F', 
            '\u0030', '\u0031', '\u0032', '\u0033', '\u0034', '\u0035', '\u0036', '\u0037', 
            '\u0038', '\u0039', '\u003A', '\u003B', '\u003C', '\u003D', '\u003E', '\u003F', 
            '\u0040', '\u0041', '\u0042', '\u0043', '\u0044', '\u0045', '\u0046', '\u0047', 
            '\u0048', '\u0049', '\u004A', '\u004B', '\u004C', '\u004D', '\u004E', '\u004F', 
            '\u0050', '\u0051', '\u0052', '\u0053', '\u0054', '\u0055', '\u0056', '\u0057', 
            '\u0058', '\u0059', '\u005A', '\u005B', '\u005C', '\u005D', '\u005E', '\u005F', 
            '\u0060', '\u0061', '\u0062', '\u0063', '\u0064', '\u0065', '\u0066', '\u0067', 
            '\u0068', '\u0069', '\u006A', '\u006B', '\u006C', '\u006D', '\u006E', '\u006F', 
            '\u0070', '\u0071', '\u0072', '\u0073', '\u0074', '\u0075', '\u0076', '\u0077',
            '\u0078', '\u0079', '\u007A', '\u007B', '\u007C', '\u007D', '\u007E', '\u007F', 
            '\u1EA0', '\u1EAE', '\u1EB0', '\u1EB6', '\u1EA4', '\u1EA6', '\u1EA8', '\u1EAC', 
            '\u1EBC', '\u1EB8', '\u1EBE', '\u1EC0', '\u1EC2', '\u1EC4', '\u1EC6', '\u1ED0', 
            '\u1ED2', '\u1ED4', '\u1ED6', '\u1ED8', '\u1EE2', '\u1EDA', '\u1EDC', '\u1EDE', 
            '\u1ECA', '\u1ECE', '\u1ECC', '\u1EC8', '\u1EE6', '\u0168', '\u1EE4', '\u1EF2', 
            '\u00D5', '\u1EAF', '\u1EB1', '\u1EB7', '\u1EA5', '\u1EA7', '\u1EA9', '\u1EAD', 
            '\u1EBD', '\u1EB9', '\u1EBF', '\u1EC1', '\u1EC3', '\u1EC5', '\u1EC7', '\u1ED1', 
            '\u1ED3', '\u1ED5', '\u1ED7', '\u1EE0', '\u01A0', '\u1ED9', '\u1EDD', '\u1EDF', 
            '\u1ECB', '\u1EF0', '\u1EE8', '\u1EEA', '\u1EEC', '\u01A1', '\u1EDB', '\u01AF', 
            '\u00C0', '\u00C1', '\u00C2', '\u00C3', '\u1EA2', '\u0102', '\u1EB3', '\u1EB5', 
            '\u00C8', '\u00C9', '\u00CA', '\u1EBA', '\u00CC', '\u00CD', '\u0128', '\u1EF3', 
            '\u0110', '\u1EE9', '\u00D2', '\u00D3', '\u00D4', '\u1EA1', '\u1EF7', '\u1EEB', 
            '\u1EED', '\u00D9', '\u00DA', '\u1EF9', '\u1EF5', '\u00DD', '\u1EE1', '\u01B0', 
            '\u00E0', '\u00E1', '\u00E2', '\u00E3', '\u1EA3', '\u0103', '\u1EEF', '\u1EAB', 
            '\u00E8', '\u00E9', '\u00EA', '\u1EBB', '\u00EC', '\u00ED', '\u0129', '\u1EC9', 
            '\u0111', '\u1EF1', '\u00F2', '\u00F3', '\u00F4', '\u00F5', '\u1ECF', '\u1ECD', 
            '\u1EE5', '\u00F9', '\u00FA', '\u0169', '\u1EE7', '\u00FD', '\u1EE3', '\u1EEE', 
        };

        public class Sign
        {
            public static string CharBase = "8B15 ???????? 8B42 ?? 8B80 ???????? 8B40";
            public static string ActionBase = "CHelperSystem";
            public static string MultiAcc = "E8 ???????? 85C0 0F84";
            public static string QuestInfo = "QUEST_INFO";
            public static string Logon = "LOGIN_MIBAO";
        }

        public static int Bool2Int(bool value)
        {
            if (value == true)
                return 1;
            return 0;
        }

        public static Keys String2Key(string key)
        {
            switch (key)
            {
                case "F1": return Keys.F1;
                case "F2": return Keys.F2;
                case "F3": return Keys.F3;
                case "F4": return Keys.F4;
                case "F5": return Keys.F5;
                case "F6": return Keys.F6;
                case "F7": return Keys.F7;
                case "F8": return Keys.F8;
                case "F9": return Keys.F9;
                case "F10": return Keys.F10;
                case "F11": return Keys.F11;
                case "F12": return Keys.F12;
                case "Alt 1": return Keys.D1;
                case "Alt 2": return Keys.D2;
                case "Alt 3": return Keys.D3;
                case "Alt 4": return Keys.D4;
                case "Alt 5": return Keys.D5;
                case "Alt 6": return Keys.D6;
                case "Alt 7": return Keys.D7;
                case "Alt 8": return Keys.D8;
                case "Alt 9": return Keys.D9;
                case "Alt 0": return Keys.D0;
            }
            return Keys.F13;
        }

        public static Keys Int2Key(int key)
        {
            switch (key)
            {
                case 0: return Keys.F1;
                case 1: return Keys.F2;
                case 2: return Keys.F3;
                case 3: return Keys.F4;
                case 4: return Keys.F5;
                case 5: return Keys.F6;
                case 6: return Keys.F7;
                case 7: return Keys.F8;
                case 8: return Keys.F9;
                case 9: return Keys.F10;
                case 10: return Keys.F11;
                case 11: return Keys.F12;
                case 12: return Keys.D1;
                case 13: return Keys.D2;
                case 14: return Keys.D3;
                case 15: return Keys.D4;
                case 16: return Keys.D5;
                case 17: return Keys.D6;
                case 18: return Keys.D7;
                case 19: return Keys.D8;
                case 20: return Keys.D9;
                case 21: return Keys.D0;
            }
            return Keys.F13;
        }

        public static int Key2Int(Keys key)
        {
            switch (key)
            {
                case Keys.F1: return 0;
                case Keys.F2: return 1;
                case Keys.F3: return 2;
                case Keys.F4: return 3;
                case Keys.F5: return 4;
                case Keys.F6: return 5;
                case Keys.F7: return 6;
                case Keys.F8: return 7;
                case Keys.F9: return 8;
                case Keys.F10: return 9;
                case Keys.F11: return 10;
                case Keys.F12: return 11;
                case Keys.D1: return 12;
                case Keys.D2: return 13;
                case Keys.D3: return 14;
                case Keys.D4: return 15;
                case Keys.D5: return 16;
                case Keys.D6: return 17;
                case Keys.D7: return 18;
                case Keys.D8: return 19;
                case Keys.D9: return 20;
                case Keys.D0: return 21;
            }
            return 22;
        }

        public static int GetTruyen(string ChuoiTruyen)
        {
            ChuoiTruyen = TDT.VietLien(ChuoiTruyen);
            if (ChuoiTruyen.Contains("namvuc")) return 4;
            else if (ChuoiTruyen.Contains("quynhchau")) return 4;
            else if (ChuoiTruyen.Contains("vodi")) return 4;
            else if (ChuoiTruyen.Contains("haitacdong")) return 4;
            else if (ChuoiTruyen.Contains("haitocdong")) return 4;
            else if (ChuoiTruyen.Contains("mieunhandong")) return 4;

            else if (ChuoiTruyen.Contains("namchieu")) return 5;
            else if (ChuoiTruyen.Contains("diemho")) return 5;
            else if (ChuoiTruyen.Contains("bachsadiemkhanh")) return 5;
            else if (ChuoiTruyen.Contains("bochsadiemkhanh")) return 5;
            else if (ChuoiTruyen.Contains("thachlam")) return 5;
            else if (ChuoiTruyen.Contains("thochlam")) return 5;
            else if (ChuoiTruyen.Contains("mieucuong")) return 5;
            else if (ChuoiTruyen.Contains("ngockhe")) return 5;

            else if (ChuoiTruyen.Contains("truongbachson")) return 6;
            else if (ChuoiTruyen.Contains("truongbochson")) return 6;
            else if (ChuoiTruyen.Contains("hoanglongphu")) return 6;
            else if (ChuoiTruyen.Contains("tuyetlangho")) return 6;
            else if (ChuoiTruyen.Contains("thaonguyen")) return 6;
            else if (ChuoiTruyen.Contains("thuykinhho")) return 6;
            else if (ChuoiTruyen.Contains("lieutay")) return 6;
            else if (ChuoiTruyen.Contains("tienvuongphan")) return 6;
            else if (ChuoiTruyen.Contains("nganngaituyetnguyen")) return 6;
            else return -1;
        }

        public static int SecDiff(DateTime from, DateTime to)
        {
            return (int)((to - from).TotalSeconds);
        }

        public static int IsKhoang(string ChuoiTruyen)
        {
            ChuoiTruyen = VietLien(ChuoiTruyen);
            if (ChuoiTruyen == "modong") return 1;
            else if (ChuoiTruyen == "mosat") return 2;
            else if (ChuoiTruyen == "mobac") return 3;
            else if (ChuoiTruyen == "mohanthiet") return 4;
            else if (ChuoiTruyen == "movang") return 5;
            else if (ChuoiTruyen == "mohuyenthiet") return 6;
            else if (ChuoiTruyen == "mophale") return 7;
            else if (ChuoiTruyen == "mophithuy") return 8;
            else if (ChuoiTruyen == "mochanvu") return 9;
            else if (ChuoiTruyen == "molonghuyet") return 10;
            else if (ChuoiTruyen == "mophunghuyet") return 11;
            else return -1;
        }

        public static int IsDuoc(string ChuoiTruyen)
        {
            ChuoiTruyen = VietLien(ChuoiTruyen);
            if (ChuoiTruyen == "bachanh") return 1;
            else if (ChuoiTruyen == "bochanh" ) return 1;
            else if (ChuoiTruyen == "bohoang") return 2;
            else if (ChuoiTruyen == "xuyenboi") return 3;
            else if (ChuoiTruyen == "nguyenho") return 4;
            else if (ChuoiTruyen == "tyba") return 5;
            else if (ChuoiTruyen == "camthao") return 6;
            else if (ChuoiTruyen == "kimnganhoa") return 7;
            else if (ChuoiTruyen == "hoangcam") return 8;
            else if (ChuoiTruyen == "cauky") return 9;
            else if (ChuoiTruyen == "tramhuong") return 10;
            else if (ChuoiTruyen == "dotrong") return 11;
            else if (ChuoiTruyen == "thuongthuat") return 12;
            else if (ChuoiTruyen == "phuclinh") return 13;
            else if (ChuoiTruyen == "phongphong") return 14;
            else if (ChuoiTruyen == "huongnhu") return 15;
            else if (ChuoiTruyen == "hoanglien") return 16;
            else if (ChuoiTruyen == "duongqui") return 17;
            else if (ChuoiTruyen == "quetam") return 18;
            else if (ChuoiTruyen == "huongphu") return 19;
            else if (ChuoiTruyen == "hoachuong") return 20;
            else if (ChuoiTruyen == "hoithanthao") return 21;
            else if (ChuoiTruyen == "thuo") return 22;
            else if (ChuoiTruyen == "dongtrunghathao") return 23;
            else if (ChuoiTruyen == "dongtrunghothao") return 23;
            else if (ChuoiTruyen == "longquitu") return 24;
            else if (ChuoiTruyen == "tuongboi") return 25;
            else if (ChuoiTruyen == "nhansam") return 26;
            else if (ChuoiTruyen == "linhchi") return 27;
            else if (ChuoiTruyen == "tuanthao") return 28;
            else if (ChuoiTruyen == "lientu") return 29;
            else if (ChuoiTruyen == "khomocxuan") return 30;
            else return -1;
        }

        public static string MapToString(uint mapid)
        {
            if (GAMEDIC.MapNameId.ContainsKey((int)mapid))
            {
                return TDT.VietLien(GAMEDIC.MapNameId[(int)mapid]);
            }
            if (mapid == MAP.KinhHo)
                return "kinhho";
            if (mapid == 0)
                return "lacduong";
            if (mapid == 1)
                return "tochau";
            if (mapid == 2)
                return "daily";
            if (mapid == 3)
                return "tungson";
            if (mapid == 4)
                return "thaiho";
            if (mapid == 7)
                return "kiemcac";
            if (mapid == 8)
                return "donhoang";
            if (mapid == 18)
                return "nhannam";
            if (mapid == 19)
                return "nhanbac";
            if (mapid == 20)
                return "thaonguyen";
            if (mapid == 21)
                return "lieutay";
            if (mapid == 22)
                return "truongbachson";
            if (mapid == 23)
                return "hoanglongphu";
            if (mapid == 24)
                return "nhihai";
            if (mapid == 25)
                return "thuongson";
            if (mapid == 26)
                return "thachlam";
            if (mapid == 27)
                return "ngockhue";
            if (mapid == 28)
                return "namchieu";
            if (mapid == 29)
                return "mieucuong";
            if (mapid == 30)
                return "tayho";
            if (mapid == 31)
                return "longtuyen";
            if (mapid == 32)
                return "vodi";
            if (mapid == 33)
                return "mailinh";
            if (mapid == 34)
                return "namhai";
            if (mapid == 35)
                return "quynhchau";
            return "khongbiet";
        }

        public static string GetBangXY(string input)
        {

            input = VietLien(input);
            if (input.Contains("thaonguyen")) //return 20;
            {
                if (input.Contains("chinhdong")) return "271,191,20," + NPC.ThaoNguyenChinhDong.Id;
                else if (input.Contains("chinhbac")) return "0,0,20," + NPC.ThaoNguyenChinhDong.Id;
                else if (input.Contains("chinhtay")) return "66,202,20," + NPC.ThaoNguyenChinhTay.Id;
                else if (input.Contains("taynam")) return "97,281,20," + NPC.ThaoNguyenTayNam.Id;
            }
            else if (input.Contains("nhannam")|| input.Contains("nhonnam")) //return 18;
            {
                if (input.Contains("chinhdong")) return "283,113,18," + NPC.NhanNamChinhDong.Id;
                else if (input.Contains("chinhbac")) return "102,36,18," + NPC.NhanNamChinhBac.Id;
                else if (input.Contains("chinhtay")) return "0,0,18," + NPC.NhanNamChinhBac.Id;
                else if (input.Contains("chinhnam")) return "72,284,18," + NPC.NhanNamChinhNam.Id;
            }
            else if (input.Contains("nhanbac") || input.Contains("nhonbac")) //return 19;
            {
                if (input.Contains("dongbac")) return "234,24,19," + NPC.NhanBacDongBac.Id;
                else if (input.Contains("taybac")) return "116,29,19," + NPC.NhanBacTayBac.Id;
                else if (input.Contains("chinhtay")) return "32,128,19," + NPC.NhanBacChinhTay.Id;
                else if (input.Contains("huongnam")) return "0,0,19," + NPC.NhanBacChinhTay.Id;
            }
            else if (input.Contains("lieutay")) //return 21;
            {
                if (input.Contains("dongnam")) return "277,258,21," + NPC.LieuTayDongNam.Id;
                else if (input.Contains("taybac")) return "75,35,21," + NPC.LieuTayTayBac.Id;
                else if (input.Contains("chinhtay")) return "40,142,21," + NPC.LieuTayChinhTay.Id;
                else if (input.Contains("huongnam")) return "0,0,21," + NPC.LieuTayChinhTay.Id;
            }

            else if (input.Contains("truongbachson") || input.Contains("truongbochson")) //return 22;
            {
                if (input.Contains("chinhnam")) return "216,282," + NPC.TruongBachSonChinhNam.Id;
                else if (input.Contains("taybac")) return "39,63," + NPC.TruongBachSonTayBac.Id;
                else if (input.Contains("chinhdong")) return "280,154," + NPC.TruongBachSonChinhDong.Id;
                else if (input.Contains("chinhnam")) return "0,0,22," + NPC.TruongBachSonChinhDong.Id;
            }
            else if (input.Contains("hoanglongphu")) //return 23;
            {
                if (input.Contains("dongnam")) return "253,285,23," + NPC.HoangLongPhuDongNam.Id;
                else if (input.Contains("taybac")) return "28,54,23," + NPC.HoangLongPhuTayBac.Id;
                else if (input.Contains("chinhdong")) return "290,115,23," + NPC.HoangLongPhuChinhDong.Id;
                else if (input.Contains("huongnam")) return "0,0,,23," + NPC.HoangLongPhuChinhDong.Id;
            }
            else if (input.Contains("nhihai")) //return 24;
            {
                if (input.Contains("chinhdong")) return "285,166,24," + NPC.NhiHaiChinhDong.Id;
                else if (input.Contains("chinhnam")) return "173,283,24," + NPC.NhiHaiChinhNam.Id;
                else if (input.Contains("chinhtay")) return "34,100,24," + NPC.NhiHaiChinhTay.Id;
                else if (input.Contains("huongnam")) return "0,0,24," + NPC.NhiHaiChinhTay.Id;
            }
            else if (input.Contains("thuongson")) //return 25;
            {
                if (input.Contains("tay")) return "37,172,25," + NPC.ThuongSonTranTay.Id;
                else if (input.Contains("chinhdong")) return "294,153,25," + NPC.ThuongSonChinhDong.Id;
                else if (input.Contains("chinhnam")) return "146,284,25," + NPC.ThuongSonChinhNam.Id;
                else if (input.Contains("huongnam")) return "0,0,25," + NPC.ThuongSonChinhNam.Id;
            }
            else if (input.Contains("thachlam") || input.Contains("thochlam")) //return 26;
            {
                if (input.Contains("chinhbac")) return "226,36,26," + NPC.ThachLamChinhBac.Id;
                else if (input.Contains("chinhnam")) return "278,281,26," + NPC.ThachLamChinhNam.Id;
                else if (input.Contains("chinhtay")) return "45,177,26," + NPC.ThachLamChinhTay.Id;
                else if (input.Contains("huongnam")) return "0,0,26," + NPC.ThachLamChinhTay.Id;
            }
            else if (input.Contains("mieucuong")) //return 29;
            {
                if (input.Contains("dongbac")) return "250,45,29," + NPC.MieuCuongDongBac.Id;
                else if (input.Contains("taynam")) return "37,251,29," + NPC.MieuCuongTayNam.Id;
                else if (input.Contains("chinhdong")) return "281,160,29," + NPC.MieuCuongChinhDong.Id;
                else if (input.Contains("huongnam")) return "0,0,29," + NPC.MieuCuongChinhDong.Id;
            }
            else if (input.Contains("tayho")) //return 30;
            {
                if (input.Contains("taynam")) return "45,267,30," + NPC.TayHoTayNam.Id;
                else if (input.Contains("chinhdong")) return "261,231,30," + NPC.TayHoChinhDong.Id;
                else if (input.Contains("chinhtay")) return "39,139,30," + NPC.TayHoChinhTay.Id;
                else if (input.Contains("huongnam")) return "0,0,30," + NPC.TayHoChinhTay.Id;
            }
            else if (input.Contains("longtuyen")) //return 31;
            {
                if (input.Contains("dongnam")) return "218,282,31," + NPC.LongTuyenDongNam.Id;
                else if (input.Contains("chinhbac")) return "63,33,31," + NPC.LongTuyenChinhBac.Id;
                else if (input.Contains("chinhtay")) return "35,121,31," + NPC.LongTuyenChinhTay.Id;
                else if (input.Contains("huongnam")) return "0,0,31," + NPC.LongTuyenChinhTay.Id;
            }
            else if (input.Contains("vodi")) //return 32;
            {
                if (input.Contains("dongbac")) return "254,38,32," + NPC.VoDiDongBac.Id;
                else if (input.Contains("chinhnam")) return "113,280,32," + NPC.VoDiChinhNam.Id;
                else if (input.Contains("chinhtay")) return "92,172,32," + NPC.VoDiChinhTay.Id;
                else if (input.Contains("huongnam")) return "0,0,32," + NPC.VoDiChinhTay.Id;
            }
            else if (input.Contains("mailinh")) //return 33;
            {
                if (input.Contains("dongbac")) return "271,37,33," + NPC.MaiLinhDongBac.Id;
                else if (input.Contains("taybac")) return "31,90,33," + NPC.MaiLinhTayBac.Id;
                else if (input.Contains("chinhdong")) return "282,236,33," + NPC.MaiLinhChinhDong.Id;
                else if (input.Contains("huongnam")) return "0,0,33," + NPC.MaiLinhChinhDong.Id;
            }
            else if (input.Contains("namvuc")) //return 34;
            {
                if (input.Contains("dongbac")) return "292,58,34," + NPC.NamVucDongBac.Id;
                else if (input.Contains("taynam")) return "108,227,34," + NPC.NamVucTayNam.Id;
                else if (input.Contains("chinhbac")) return "134,40,34," + NPC.NamVucChinhBac.Id;
                else if (input.Contains("huongnam")) return "0,0,34," + NPC.NamVucChinhBac.Id;
            }
            else if (input.Contains("quynhchau")) //return 35;
            {
                if (input.Contains("dongc")) return "243,150,35," + NPC.QuynhChauChinhDong.Id;
                else if (input.Contains("dongbac")) return "273,52,35," + NPC.QuynhChauDongBac.Id;
                else if (input.Contains("tay")) return "80,139,35," + NPC.QuynhChauChinhTay.Id;
                else if (input.Contains("nam")) return "0,0,35," + NPC.QuynhChauChinhTay.Id;
            }
            else return "";

            return "";

        }



        public static string GetDuaBangXY(string input)
        {
            //THAIHO(246,146),tayho(134,165)
            //nhihai(77,206),vodi(69,106),nhanbac(277,54)
            //thaonguyen(256,51),namvuc(110,64),thachlam(73,214)
            //quynhchau(136,236)mieucuong(195,49)
            input = VietLien(input);
            if (input.Contains("thaonguyen")|| input.Contains("hoanglongphu") || input.Contains("truongbachson") || input.Contains("truongbochson") || input.Contains("lieutay")) return "256,51,20"; 
            else if (input.Contains("nhanbac") || input.Contains("nhonbac") || input.Contains("nhannam") || input.Contains("nhonnam")) return "277,54,19";
            else if (input.Contains("nhihai")|| input.Contains("thuongson")) return "77,206,24";
            else if (input.Contains("thachlam") || input.Contains("thochlam") || input.Contains("namchieu") || input.Contains("ngockhe")) return "73,214,26";
            else if (input.Contains("mieucuong")) return "195,49,29";
            else if (input.Contains("tayho") || input.Contains("longtuyen")) return "134,165,30";
            else if (input.Contains("vodi")) return "69,106,32";
            else if (input.Contains("namvuc")) return "110,64,34";
            else if (input.Contains("quynhchau")) return "136,236,35";
            else if (input.Contains("thaiho")) return "246,146,4";
            return "-1";

        }
        public static int GetMapId(string input)
        {
            input = VietLien(input);
            if (input.Contains("lacduong")) return 0;
            else if (input.Contains("locduong")) return 0;
            else if (input.Contains("tochau")) return 1;
            else if (input.Contains("daily")) return 2;
            else if (input.Contains("doily")) return 2;
            else if (input.Contains("doilu")) return 2;
            else if (input.Contains("tungson")) return 3;
            else if (input.Contains("thaiho")) return 4;
            else if (input.Contains("kinhho")) return 5;
            else if (input.Contains("voluongson")) return 6;
            else if (input.Contains("kiemcac")) return 7;
            else if (input.Contains("donhoang")) return 8;
            else if (input.Contains("thieulamtu")) return 9;
            else if (input.Contains("caibangtongda")) return 10;
            else if (input.Contains("quangminhdien")) return 11;
            else if (input.Contains("vodangson")) return 12;
            else if (input.Contains("thienlongtu")) return 13;
            else if (input.Contains("langbadong")) return 14;
            else if (input.Contains("ngamison")) return 15;
            else if (input.Contains("tinhtuchai")) return 16;
            else if (input.Contains("thienson")) return 17;
            else if (input.Contains("nhannam")) return 18;
            else if (input.Contains("nhonnam")) return 18;
            else if (input.Contains("nhanbac")) return 19;
            else if (input.Contains("nhonbac")) return 19;
            else if (input.Contains("thaonguyen")) return 20;
            else if (input.Contains("lieutay")) return 21;
            else if (input.Contains("truongbachson")) return 22;
            else if (input.Contains("truongbochson")) return 22;
            else if (input.Contains("hoanglongphu")) return 23;
            else if (input.Contains("nhihai")) return 24;
            else if (input.Contains("thuongson")) return 25;
            else if (input.Contains("thachlam")) return 26;
            else if (input.Contains("thochlam")) return 26;
            else if (input.Contains("ngockhe")) return 27;
            else if (input.Contains("namchieu")) return 28;
            else if (input.Contains("mieucuong")) return 29;
            else if (input.Contains("tayho")) return 30;
            else if (input.Contains("longtuyen")) return 31;
            else if (input.Contains("vodi")) return 32;
            else if (input.Contains("mailinh")) return 33;
            else if (input.Contains("namvuc")) return 34;
            else if (input.Contains("quynhchau")) return 35;
            else if (input.Contains("huyenvudao")) return 112;
            else if (input.Contains("baotangdongtang1")) return 166;
            else if (input.Contains("baotangdongtang2")) return 169;
            else if (input.Contains("nganngaituyetnguyen")) return 229;
            else if (input.Contains("baotangdongtang3")) return 191;
            else if (input.Contains("baotangdongtang4")) return 192;
            else if (input.Contains("baotangdongtang5")) return 193;
            else if (input.Contains("thaolieutruong")) return 199;
            else if (input.Contains("mieu nhan dong")) return 200;
            else if (input.Contains("thanh thu son")) return 201;
            else if (input.Contains("yenvuongcomotang1")) return 202;
            else if (input.Contains("yenvuongcomotang2")) return 203;
            else if (input.Contains("yenvuongcomotang3")) return 204;
            else if (input.Contains("yenvuongcomotang4")) return 205;
            else if (input.Contains("yenvuongcomotang5")) return 206;
            else if (input.Contains("yenvuongcomotang6")) return 207;
            else if (input.Contains("yenvuongcomotang7")) return 208;
            else if (input.Contains("yenvuongcomotang8")) return 209;
            else if (input.Contains("yenvuongcomotang9")) return 210;
            else if (input.Contains("bentausondong")) return 211;
            else if (input.Contains("kiemgia")) return 212;
            else if (input.Contains("manhaidong")) return 213;
            else if (input.Contains("danhancau")) return 214;
            else if (input.Contains("ontuyendong")) return 215;
            else if (input.Contains("hoanglongdong")) return 216;
            else if (input.Contains("thuykinhho")) return 217;
            else if (input.Contains("tienvuongphan")) return 218;
            else if (input.Contains("thienkhanhthudong")) return 219;
            else if (input.Contains("daohoanguyen")) return 220;
            else if (input.Contains("haitacdong")) return 221;
            else if (input.Contains("tuyetlangho")) return 222;
            else if (input.Contains("diemho")) return 235;
            else if (input.Contains("bachsadiemkhanh")) return 237;
            else if (input.Contains("bochsadiemkhanh")) return 237;
            else if (input.Contains("hoadiemson")) return 244;
            else if (input.Contains("caoxuong")) return 245;
            else if (input.Contains("laulan")) return 246;
            else if (input.Contains("thaplymoc")) return 247;
            else if (input.Contains("thaplumoc")) return 247;
            else if (input.Contains("hoadiemcoc")) return 251;
            else if (input.Contains("caoxuongmecung")) return 252;
            else if (input.Contains("thapkhaclapmacan")) return 253;
            else if (input.Contains("daiuyen")) return 249;
            else if (input.Contains("hanhuyetlinh")) return 255;
            else if (input.Contains("honhuyetlinh")) return 255;
            else if (input.Contains("tanhoangdiacungtang1")) return 262;
            else if (input.Contains("tanhoangdiacungtang2")) return 263;
            else if (input.Contains("tanhoangdiacungtang3")) return 264;
            else if (input.Contains("tanhoangdiacungtang4")) return 292;
            else if (input.Contains("datayho")) return 164;
            else if (input.Contains("dotayho")) return 164;
            else if (input.Contains("conlonphucdia")) return 254;
            else if (input.Contains("conlonson")) return 248;
            else if (input.Contains("thanhnguyen")) return 282;
            else if (input.Contains("thanhnguyensondong")) return 283;
            else if (input.Contains("modungsontrang")) return 284;
            else if (input.Contains("tatmanhihan")) return 250;
            else if (input.Contains("thanhhoacung")) return 256;
            else if (input.Contains("lamhaikhecoc")) return 569;
            else if (input.Contains("macnamthanhnguyen")) return 573;
            else if (input.Contains("vongxuyenhoahai")) return 574;
            else if (input.Contains("thienkynamhoai")) return 575;
            else if (input.Contains("thongthienthapdiacung")) return 295;
            else if (input.Contains("thongthienthaptang1")) return 296;
            else if (input.Contains("thongthienthaptang2")) return 297;
            else if (input.Contains("thongthienthaptang3")) return 298;
            else if (input.Contains("dinhthongthienthap")) return 299;
            else if (input.Contains("phungminhtran")) return 580;
            else if (input.Contains("laulan")) return 246;
            else if (input.Contains("thuchacotran")) return 260;
            else if (input.Contains("denhatkhunghingoitailacduong")) return 238;
            else if (input.Contains("khunghingoitaidaily")) return 240;
            else if (input.Contains("khunghingoitaitochau")) return 241;
            else if (input.Contains("hanngoccoc")) return 243;
            else if (input.Contains("thuynguyetdongthien")) return 613;
            else if (input.Contains("huyenhai")) return 611;
            else if (input.Contains("daicondihai")) return 612;
            else if (input.Contains("phungminhtran")) return 580;
            else if (input.Contains("thuynguyetdongthien")) return 613;
            else if (input.Contains("lacduong")) return 242;
            else if (input.Contains("huyenvudao")) return 112;
            else if (input.Contains("laulan")) return 246;
            else if (input.Contains("thuchacotran")) return 260;
            else if (input.Contains("denhatkhunghingoitailacdduong")) return 238;
            else if (input.Contains("denhikhunghingoitailacduong")) return 239;
            else if (input.Contains("modungsontrang")) return 284;
            else if (input.Contains("tientrang")) return 224;
            else if (input.Contains("phungminhtran")) return 580;
            else if (input.Contains("huyenvudao")) return 112;
            else if (input.Contains("quangminhdong")) return 601;
            else if (input.Contains("daycoctieudao")) return 602;
            else if (input.Contains("linhtinhphong")) return 603;
            else if (input.Contains("caibangtuudieu")) return 604;
            else if (input.Contains("daohoatran")) return 605;
            else if (input.Contains("thaplam")) return 606;
            else if (input.Contains("nguthandong")) return 607;
            else if (input.Contains("chietmaiphong")) return 608;
            else if (input.Contains("chanthap")) return 609;
            else if (input.Contains("tangthuthuycac")) return 610;
            else if (input.Contains("hauhoavien")) return 123;
            else if (input.Contains("tieumocnhanhang")) return 122;
            else if (input.Contains("duonggiabao")) return 615;
            else if (input.Contains("laulan")) return 246;
            else if (input.Contains("thuchacotran")) return 260;
            else if (input.Contains("modungsontrang")) return 284;
            else if (input.Contains("phungminhtran")) return 580;
            else if (input.Contains("dienvotruong")) return 617;
            else if (input.Contains("quanthienthanh")) return 581;
            else if (input.Contains("trieukinhthanh")) return 583;
            else if (input.Contains("laphuthanh")) return 582;
            else return -1;
        }

        public static bool FileHostValid
        {
            get
            {
                string host = Environment.GetFolderPath(Environment.SpecialFolder.System) + @"\drivers\etc\hosts";
                host = TDT.ReadFile(host);
                if (host.ToLower().Contains("tieudattai"))
                {
                    return true;
                }
                return true;
            }
        }

        public static string GapNPC(string input)
        {
            input = VietLien(input);
            if (input.Contains("trithanhdaisu") || input.Contains("bhrwsc_110331_53")) return "0,176,192,trithanhdaisu,34";
            else if (input.Contains("trithanhdoisu") || input.Contains("bhrwsc_110331_53")) return "0,176,192,trithanhdoisu,34";
            else if (input.Contains("doanchinhthuan")) return "2,71,18,doanchinhthuan,16";
            else if (input.Contains("tothuc")) return "1,166,311,tothuc,2";
            else return "";
        }        

        public static string MuaDo(string input)
        {
            if (input.Contains("#{SDHDRW_091109_44}")) return "104,123";//134,158  117 167  120 207
            else return "";
        }

        public static string HaiDuoc(string input)
        {
            if (input.Contains("#{SDHDRW_091109_41}")) return "4,168,200,229,117,114,128,116,186,168,200";//134,158  117 167  120 207            
            else return "";
        }

        public static string PhuBanMP(string input)
        {
            input = VietLien(input);
            if (input.Contains("longtu")) return "186,98,142,hotutruonglao,13035,96,142";
            else if (input.Contains("modungsontrang")) return "289,152,154,congdakhon,9044,160,169";
            else if (input.Contains("duonggiabao")) return "616,152,154,duongmotuong,10051,173,170";
            else if (input.Contains("tinhtuchai")) return "189,100,145,thientoantu,16035,96,142";
            else if (input.Contains("badong")) return "187,45,126,congdatutruong,14035,44,129";
            else if (input.Contains("thieulamtu")) return "182,99,146,huyenchung,9035,96,158";
            else if (input.Contains("thienson")) return "190,94,147,dangba,17035,95,148";
            else if (input.Contains("ngamison")) return "188,95,146,lieutammuoi,15035,89,146";
            else if (input.Contains("vodangson")) return "185,100,181,tieuthiendat,12035,95,192";
            else if (input.Contains("quangminhdien")) return "184,95,162,thaccang,11035,98,159";
            else if (input.Contains("caibangtongda")) return "183,93,152,auduongqua,10035,91,159";
            else return "";
        }

        public static string PhuBanDanhQuai(string input)
        {
            input = VietLien(input);
            //thienlongtu
            if (input.Contains("dietyeukhoiloi")) return "127,113,dietyeukhoiloi";
            else if (input.Contains("trutienkhoiloi")) return "95,80,trutienkhoiloi";
            else if (input.Contains("thithankhoiloi")) return "51,72,thithankhoiloi";
            //modung
            else if (input.Contains("thambiphitac")) return "69,126,thambiphitac";
            else if (input.Contains("tamthuphitac")) return "54,61,tamthuphitac";
            else if (input.Contains("suubaophitac")) return "68,142,phitieuthiettac";
            //duong mon
            else if (input.Contains("tatlethiettac")) return "98,70,tatlethiettac";
            else if (input.Contains("docchamthiettac")) return "51,69,docchamthiettac";
            else if (input.Contains("phitieuthiettac")) return "100,181,phitieuthiettac";
            //tin tuc
            else if (input.Contains("mocvuongtrithu")) return "96,126,mocvuongtrithu";
            else if (input.Contains("thuyvuongtrithu")) return "118,112,thuyvuongtrithu";
            else if (input.Contains("hoavuongtrithu")) return "96,86,hoavuongtrithu";
            //tieudao
            if (input.Contains("huthekhoiloi")) return "52,72,huthekhoiloi";
            else if (input.Contains("thuctamkhoiloi")) return "120,140,thuctamkhoiloi";
            else if (input.Contains("hoaphachkhoiloi")) return "146,58,hoaphachkhoiloi";
            //thieu lam
            else if (input.Contains("mocnhanlaula")) return "96,110,mocnhanlaula";
            else if (input.Contains("mocnhantinhanh")) return "96,80,mocnhantinhanh";
            else if (input.Contains("mocnhanvosi")) return "40,98,mocnhanvosi";
            //thien son
            else if (input.Contains("thiensontieutuyetquai")) return "96,110,thiensontieutuyetquai";
            else if (input.Contains("thiensondaituyetquai")) return "96,86,thiensondaituyetquai";
            else if (input.Contains("thiensontuyetquaivuong")) return "96,50,thiensontuyetquaivuong";
            //ngamy
            else if (input.Contains("ngamibachmyacvien")) return "139,106,ngamibachmyacvien";
            else if (input.Contains("ngamiloitraoacvien")) return "96,63,ngamiloitraoacvien";
            else if (input.Contains("ngamihungnhu")) return "44,45,ngamihungnhu";
            //vodang
            else if (input.Contains("yeuditamma")) return "59,180,yeuditamma";
            else if (input.Contains("phasantamma")) return "78,132,phasantamma";
            else if (input.Contains("satductamma")) return "46,58,satductamma";
            //minhgiao
            else if (input.Contains("matthamtienphong")) return "97,117,matthamtienphong";
            else if (input.Contains("thanhkythamma")) return "155,102,thanhkythamma";
            else if (input.Contains("lamkythamma")) return "98,65,lamkythamma";
            //caibang
            else if (input.Contains("phuccuuachau")) return "69,145,phuccuuachau";
            else if (input.Contains("cuongtrangachau")) return "45,115,cuongtrangachau";
            else if (input.Contains("tinhtrangachau")) return "44,79,tinhtrangachau";

            else return "";
        }

        public static string GetInfo(string info)
        {
            info = info.Replace("Vương Đức Phú", "Vương Đức Phúc");
            info = info.Replace("Bách Hiểu Sinh", "Bạch Manh Sinh");
            info = info.Replace("Mộ Dung Chùy", "Mộ Dung Thùy");

            // QuyCoc
            if(info.Contains("Huyền Hồ Uyển"))
            {
                return "INFOAIM139,146,678,TuoiNuoc";
            }
            if (info.Contains("Quan Tinh Đài"))
            {
                return "INFOAIM60,42,678,TuoiNuoc";
            }
            if (info.Contains("Vu Sơn Cảnh"))
            {
                return "INFOAIM40,100,678,TuoiNuoc";
            }
            if (info.Contains("Âm Dương Thiên Đông Trắc"))
            {
                return "INFOAIM128,71,678,TuoiNuoc";
            }
            //Âm Dương Thiên Đông Trắc #{_INFOAIM125,70,678,}#
            //Vu Sơn Cảnh #{_INFOAIM40,100,678,}
            // Mo Dung
            if (info.Contains("Thính Hương Thủy Tạ"))
            {
                return "INFOAIM154,93,284,TuoiNuoc";
            }
            if (info.Contains("Cầm Âm Tiểu Trúc"))
            {
                return "INFOAIM78,142,284,TuoiNuoc";
            }
            if (info.Contains("Sâm Hợp Trang"))
            {
                return "INFOAIM28,28,284,TuoiNuoc";
            }
            if (info.Contains("Mạn Đà Viên"))
            {
                return "INFOAIM119,36,284,TuoiNuoc";
            }
            if (info.Contains("#{SMFB_120214_47}#r"))
            {
                return "INFOAIM67,110,284,MoDungThuy,PhuBan";
            }

            // ThienLong
            if (info.Contains("Túc Thái Âm Tì Kinh Đồng Nhân"))
            {
                return "INFOAIM121,90,13,TuoiNuoc";
            }
            if (info.Contains("Thủ Thái Âm Phế Kinh Đồng Nhân"))
            {
                return "INFOAIM62,90,13,TuoiNuoc";
            }
            if (info.Contains("Túc Dương Minh Vị Kinh Đồng Nhân"))
            {
                return "INFOAIM106,85,13,TuoiNuoc";
            }
            if (info.Contains("#{SMFB_120214_41}#{SMXL_090819_dali}#r#{SMRW_090206_01}"))
            {
                return "INFOAIM35,86,13,BanTuong,PhuBan";
            }
            if (info.Contains("Người đồng thủ dương minh đại trường kinh"))
            {
                return "INFOAIM84,84,13,TuoiNuoc";
            }

            // MinhGiao   
            if (info.Contains("Hoàng Thổ Kỳ"))
            {
                return "INFOAIM62,38,11,TuoiNuoc";
            }
            if (info.Contains("Bạch Kim Kỳ"))
            {
                return "INFOAIM65,139,11,TuoiNuoc";
            }
            if (info.Contains("Thanh Mộc Kỳ"))
            {
                return "INFOAIM131,139,11,TuoiNuoc";
            }
            if (info.Contains("Hắc Thủy Kỳ"))
            {
                return "INFOAIM129,55,11,TuoiNuoc";
            }
            if (info.Contains("#{SMFB_120214_51}#{SMXL_090819_mingjiao}#r#{SMRW_090206_01}"))
            {
                return "INFOAIM89,56,11,PhuongLap,PhuBan";
            }
          
            // TieuDao
            if (info.Contains("Đang Thanh Họa"))
            {
                return "INFOAIM142,59,14,TuoiNuoc";
            }
            if (info.Contains("Lạn Kha Kỳ"))
            {
                return "INFOAIM136,145,14,TuoiNuoc";
            }
            if (info.Contains("Phụng Hoàng Cầm"))
            {
                return "INFOAIM42,144,14,TuoiNuoc";
            }
            if (info.Contains("Thánh Hiền Thư"))
            {
                return "INFOAIM47,54,14,TuoiNuoc";
            }
            if (info.Contains("#{SMFB_120214_45}#{SMXL_090819_xiaoyao}#r#{SMRW_090206_01}"))
            {
                return "INFOAIM62,68,14,PhungATam,PhuBan";
            }

            // VoDang - Complete
            if (info.Contains("Kim Điện"))
            {
                return "INFOAIM82,58,12,TuoiNuoc";
            }
            if (info.Contains("Thiên Giới"))
            {
                return "INFOAIM45,87,12,TuoiNuoc";
            }
            if (info.Contains("Hồi Long Đài"))
            {
                return "INFOAIM76,133,12,TuoiNuoc";
            }
            if (info.Contains("Giải Kiếm Trì"))
            {
                return "INFOAIM49,180,12,TuoiNuoc";
            }
            if (info.Contains("#{SMFB_120214_39}#{SMXL_090819_wudang}#r#{SMRW_090206_01}"))
            {
                return "INFOAIM58,73,12,LamLinhTo,PhuBan";
            }

            // ThienSon
            if (info.Contains("Nham Băng Hộ"))
            {
                return "INFOAIM125,50,17,TuoiNuoc";
            }
            if (info.Contains("Huyền Băng Hộ"))
            {
                return "INFOAIM65,43,17,TuoiNuoc";
            }
            if (info.Contains("Hàn Băng Hộ"))
            {
                return "INFOAIM71,65,17,TuoiNuoc";
            }
            if (info.Contains("Toái Băng Hộ"))
            {
                return "INFOAIM123,89,17,TuoiNuoc";
            }
            if (info.Contains("#{SMFB_120214_43}#{SMXL_090819_tianshan}#r#{SMRW_090206_01}"))
            {
                return "INFOAIM101,44,17,CucKiem,PhuBan";
            }

            // ThieuLam
            if (info.Contains("Chung Lâu"))
            {
                return "INFOAIM81,69,9,TuoiNuoc";
            }
            if (info.Contains("Đại Hùng Bảo Điện"))
            {
                return "INFOAIM96,82,9,TuoiNuoc";
            }
            if (info.Contains("Tàng Kinh Các"))
            {
                return "INFOAIM134,132,9,TuoiNuoc";
            }
            if (info.Contains("GSơn môn"))
            {
                return "INFOAIM90,110,9,TuoiNuoc";
            }
            if (info.Contains("#{SMFB_120214_35}#{SMXL_090819_shaolin}#r#{SMRW_090206_01}"))
            {
                return "INFOAIM61,62,9,HuyenTrung,PhuBan";
            }

            // DuongMon
            if (info.Contains("Thiên Cơ Phường"))
            {
                return "INFOAIM56,136,615,TuoiNuoc";
            }
            if (info.Contains("Đường Gia Nội Bảo"))
            {
                return "INFOAIM80,46,615,TuoiNuoc";
            }
            if (info.Contains("Diễn Võ Trường"))
            {
                return "INFOAIM48,80,615,TuoiNuoc";
            }
            if (info.Contains("Phong Vũ Lâu"))
            {
                return "INFOAIM102,97,615,TuoiNuoc";
            }
            if (info.Contains("#{TMSM_130808_01}#") && info.Contains("#{SMRW_090206_01}"))
            {
                return "INFOAIM66,30,615,DuongNhacThien,Phuban";
            }

            // NgaMy            
            if (info.Contains("Phật Quang Phụng Hoàng"))
            {
                return "INFOAIM39,152,15,TuoiNuoc";
            }
            if (info.Contains("Kim Đỉnh Phụng Hoàng"))
            {
                return "INFOAIM45,42,15,TuoiNuoc";
            }
            if (info.Contains("Linh Tuyền Phụng Hoàng"))
            {
                return "INFOAIM146,46,15,TuoiNuoc";
            }
            if (info.Contains("Vạn Niên Phụng Hoàng"))
            {
                return "INFOAIM146,156,15,TuoiNuoc";
            }
            if (info == "#{SMFB_120214_37}#{SMXL_090819_emei}#r#{SMRW_090206_01}")
            {
                return "INFOAIM96,73,15,ManhThanhThanh,PhuBan";
            }

            // CaiBang
            if (info.Contains("Đỗ khang từ"))
            {
                return "INFOAIM131,112,10,TuoiNuoc";
            }
            if (info.Contains("Tiểu đào viên"))
            {
                return "INFOAIM39,147,10,TuoiNuoc";
            }
            if (info.Contains("Diễn binh đàn"))
            {
                return "INFOAIM46,36,10,TuoiNuoc";
            }
            if (info.Contains("Tây sương phòng"))
            {
                return "INFOAIM53,88,10,TuoiNuoc";
            }
            if (info == "#{SMFB_120214_49}#{SMXL_090819_gaibang}#r#{SMRW_090206_01}")
            {
                return "INFOAIM41,144,10,PhatAn,PhuBan";
            }

            // TinhTuc
            if (info.Contains("Rương Rết Độc"))
            {
                return "INFOAIM87,98,16,TuoiNuoc";
            }
            if (info.Contains("Rương Bọ Cạp Độc"))
            {
                return "INFOAIM127,73,16,TuoiNuoc";
            }
            if (info.Contains("Rương Nhện Độc"))
            {
                return "INFOAIM106,98,16,TuoiNuoc";
            }
            if (info.Contains("Rương Cóc Độc"))
            {
                return "INFOAIM95,56,16,TuoiNuoc";
            }
            if (info.Contains("#{SMFB_120214_33}#{SMXL_090819_xingxiu}#r#{SMRW_090206_01}"))
            {
                return "INFOAIM128,78,16,HongNgoc,PhuBan";
            }

            return info;
        }

        public static string ParseIntString(string input)
        {
            string b = string.Empty;
            for (int i = 0; i < input.Length; i++)
            {
                if (System.Char.IsDigit(input[i]))
                    b += input[i];
                else if (b.Length > 0)
                    break;
            }
            return b;
        }

        public static int ParseAllInt(string input)
        {
            string b = string.Empty;
            for (int i = 0; i < input.Length; i++)
            {
                if (System.Char.IsDigit(input[i]))
                    b += input[i];
            }
            if (string.IsNullOrEmpty(b))
                return 0;
            int re = 0;
            int.TryParse(b, out re);
            if (input.StartsWith("-"))
                return 0 - re;
            return re;
        }


        public static ulong ParseAllUlong(string input)
        {
            string b = string.Empty;
            for (int i = 0; i < input.Length; i++)
            {
                if (System.Char.IsDigit(input[i]))
                    b += input[i];
            }
            if (string.IsNullOrEmpty(b))
                return 0;
            ulong re = 0;
            ulong.TryParse(b, out re);
            return re;
        }

    

        public static int ParseInt(string input)
        {
            int val = 0;
            string b = string.Empty;
            for (int i = 0; i < input.Length; i++)
            {
                if (System.Char.IsDigit(input[i]))
                    b += input[i];
                else if (b.Length > 0)
                    break;
            }
            if (b.Length > 0)
                val = int.Parse(b);
            return val;
        }

        public static string ReplaceFirst(string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }

        //public static void Unlock(string fileName)
        //{
        //    foreach (Process process in Process.GetProcesses())
        //    {
        //        try
        //        {
        //            if (process.MainModule.FileName == fileName)
        //            {
        //                process.Kill();
        //            }
        //        }
        //        catch { }
        //    }
        //}

        public class GZip
        {
            public static byte[] Compress(string input)
            {
                byte[] raw;
                if (File.Exists(input))
                {
                    raw = File.ReadAllBytes(input);
                }
                else
                {
                    raw = Encoding.ASCII.GetBytes(input);
                }
                using (MemoryStream memory = new MemoryStream())
                {
                    using (System.IO.Compression.GZipStream gzip = new System.IO.Compression.GZipStream(memory,
                    System.IO.Compression.CompressionMode.Compress, true))
                    {
                        gzip.Write(raw, 0, raw.Length);
                    }
                    return memory.ToArray();
                }
            }

            public static void Compress(string input, string destinationPath)
            {
                byte[] raw;
                if (File.Exists(input))
                {
                    raw = File.ReadAllBytes(input);
                }
                else
                {
                    raw = Encoding.ASCII.GetBytes(input);
                }
                using (MemoryStream memory = new MemoryStream())
                {
                    using (System.IO.Compression.GZipStream gzip = new System.IO.Compression.GZipStream(memory,
                    System.IO.Compression.CompressionMode.Compress, true))
                    {
                        gzip.Write(raw, 0, raw.Length);
                    }
                    using (FileStream fs = new FileStream(destinationPath, FileMode.Create))
                    {
                        memory.WriteTo(fs);
                    }                    
                }
            }

            public static byte[] Uncompress(string input)
            {
                byte[] gzip;
                if (File.Exists(input))
                {
                    gzip = File.ReadAllBytes(input);
                }
                else
                {
                    gzip = Encoding.ASCII.GetBytes(input);
                }
                // Create a GZIP stream with decompression mode.
                // ... Then create a buffer and write into while reading from the GZIP stream.
                using (System.IO.Compression.GZipStream stream = new System.IO.Compression.GZipStream(new MemoryStream(gzip), System.IO.Compression.CompressionMode.Decompress))
                {
                    const int size = 4096;
                    byte[] buffer = new byte[size];
                    using (MemoryStream memory = new MemoryStream())
                    {
                        int count = 0;
                        do
                        {
                            count = stream.Read(buffer, 0, size);
                            if (count > 0)
                            {
                                memory.Write(buffer, 0, count);
                            }
                        }
                        while (count > 0);
                        return memory.ToArray();
                    }
                }
            }

            public static void Uncompress(string input, string destinationPath)
            {
                byte[] gzip;
                if (File.Exists(input))
                {
                    gzip = File.ReadAllBytes(input);
                }
                else
                {
                    gzip = Encoding.ASCII.GetBytes(input);
                }
                // Create a GZIP stream with decompression mode.
                // ... Then create a buffer and write into while reading from the GZIP stream.
                using (System.IO.Compression.GZipStream stream = new System.IO.Compression.GZipStream(new MemoryStream(gzip), System.IO.Compression.CompressionMode.Decompress))
                {
                    const int size = 4096;
                    byte[] buffer = new byte[size];
                    using (MemoryStream memory = new MemoryStream())
                    {
                        int count = 0;
                        do
                        {
                            count = stream.Read(buffer, 0, size);
                            if (count > 0)
                            {
                                memory.Write(buffer, 0, count);
                            }
                        }
                        while (count > 0);
                        using (FileStream fs = new FileStream(destinationPath, FileMode.Create))
                        {
                            memory.WriteTo(fs);
                        }
                    }
                }
            }
        }

        public static string LocalIPAddress()
        {
            IPHostEntry host;
            string localIP = "";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                    break;
                }
            }
            return localIP;
        }
        public static List<string> ListStringBetween(string STR, string FirstString, string LastString)
        {
            if (STR == null)
            {
                return null;
            }
            List<string> list = new List<string>();
            for (; ; )
            {
                int num = STR.IndexOf(FirstString);
                if (num == -1)
                {
                    break;
                }
                string text = STR.Substring(num + FirstString.Length);
                int num2 = text.IndexOf(LastString);
                if (num2 == -1)
                {
                    break;
                }
                text = text.Substring(0, num2);
                list.Add(text);
                if (STR.Length <= num + FirstString.Length + text.Length + LastString.Length)
                {
                    break;
                }
                STR = STR.Substring(num + FirstString.Length + text.Length + LastString.Length);
            }
            return list;
        }

        public static float GetDistance(string from, string to)
        {
            float fromX = 0;
            float fromY = 0;
            float toX = 0;
            float toY = 0;
            if (from.Split(',').Length >= 2) {
                fromX = TDT.ParseInt(from.Split(',')[0]);
                fromY = TDT.ParseInt(from.Split(',')[1]);
            }
            if (to.Split(',').Length >= 2)
            {
                toX = TDT.ParseInt(to.Split(',')[0]);
                toY = TDT.ParseInt(to.Split(',')[1]);
            }
            return (float)Math.Sqrt(Math.Pow(fromX - toX, 2) + Math.Pow(fromY - toY, 2));
        }
        public static float GetDistance(double fromX, double fromY, double toX, double toY)
        {
            return (float)Math.Sqrt(Math.Pow(fromX - toX, 2) + Math.Pow(fromY - toY, 2));
        }

        public static int NumDiff(int num1, int num2)
        {
            int diff = num1 - num2;
            if (diff < 0)
                diff = -diff;
            return diff;
        }

        public static void FileInstall(string defaltNamespace, string resourceName, string destinationPath)
        {
            Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(defaltNamespace + "." + resourceName);
            FileStream stream2 = new FileStream(destinationPath, FileMode.Create);
            for (int i = 0; i < manifestResourceStream.Length; i++)
            {
                stream2.WriteByte((byte)manifestResourceStream.ReadByte());
            }
            stream2.Close();
        }

        public static string AllowName = "aAeEoOuUiIdDyYáàạảãâấầậẩẫăắằặẳẵÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴéèẹẻẽêếềệểễÉÈẸẺẼÊẾỀỆỂỄóòọỏõôốồộổỗơớờợởỡÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠúùụủũưứừựửữÚÙỤỦŨƯỨỪỰỬỮíìịỉĩÍÌỊỈĨđĐýỳỵỷỹÝỲỴỶỸ0123456789qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM0123456789";

        public static string GetAllowName(string name)
        {
            string n = "";
            foreach (char s in name)
            {
                if (TDT.AllowName.Contains(s))
                {
                    n += s;
                }
            }
            return n;
        }

        public static string CompressString(string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);
            using (var msi = new MemoryStream(bytes))
            {
                using (var mso = new MemoryStream())
                {
                    using (var gs = new GZipStream(mso, CompressionMode.Compress))
                    {
                        CopyTo(msi, gs);
                    }
                    return Convert.ToBase64String(mso.ToArray());
                }
            }
        }

        public static string UnCompressString(string str)
        {
            byte[] bytes = Convert.FromBase64String(str);
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                {
                    CopyTo(gs, mso);
                }
                return Encoding.UTF8.GetString(mso.ToArray());
            }
        }

        public static string[] vietnameseSigns = new string[] 
            {     
                "aAeEoOuUiIdDyY",     
                "áàạảãâấầậẩẫăắằặẳẵ",     
                "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",     
                "éèẹẻẽêếềệểễ",     
                "ÉÈẸẺẼÊẾỀỆỂỄ",     
                "óòọỏõôốồộổỗơớờợởỡ",     
                "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",     
                "úùụủũưứừựửữ",     
                "ÚÙỤỦŨƯỨỪỰỬỮ",     
                "íìịỉĩ",     
                "ÍÌỊỈĨ",     
                "đ",     
                "Đ",     
                "ýỳỵỷỹ",     
                "ÝỲỴỶỸ"     
            };

        public static string ClearString(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach(char s in str)
            {            
                bool isHave = false;
                foreach(string sub in vietnameseSigns)
                {
                    foreach(char su in sub)
                    {
                        if (su == s)
                        {
                            isHave = true;
                        }
                        break;
                    }
                    if (isHave)
                        break;
                }
                if (isHave)
                {
                    sb.Append(s);
                }
            }
            return sb.ToString();
        }
        static Random r = new Random();


        public static string NiceNameNum(int len = 5)
        {
            string[] consonants = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "l", "n", "p", "q", "r", "s", "sh", "zh", "t", "v", "w", "x" };
            string[] vowels = { "a", "e", "i", "o", "u", "ae", "y" };
            string Name = "";
            Name += consonants[r.Next(consonants.Length)].ToUpper();
            Name += vowels[r.Next(vowels.Length)];
            int b = 2; //b tells how many times a new letter has been added. It's 2 right now because the first two letters are already in the name.
            while (b < len)
            {
                Name += consonants[r.Next(consonants.Length)];
                b++;
                Name += vowels[r.Next(vowels.Length)];
                b++;
            }

            Name += random.Next(1000, 9999);
            Name += consonants[r.Next(consonants.Length)].ToUpper();
            Name += vowels[r.Next(vowels.Length)];
            b = 2; //b tells how many times a new letter has been added. It's 2 right now because the first two letters are already in the name.
            while (b < len)
            {
                Name += consonants[r.Next(consonants.Length)];
                b++;
                Name += vowels[r.Next(vowels.Length)];
                b++;
            }
            Name += random.Next(1, 100);
            return Name;
        }

        public static string NiceName(int len)
        {          
            string[] consonants = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "l", "n", "p", "q", "r", "s", "sh", "zh", "t", "v", "w", "x" };
            string[] vowels = { "a", "e", "i", "o", "u", "ae", "y" };
            string Name = "";
            Name += consonants[r.Next(consonants.Length)].ToUpper();
            Name += vowels[r.Next(vowels.Length)];
            int b = 2; //b tells how many times a new letter has been added. It's 2 right now because the first two letters are already in the name.
            while (b < len)
            {
                Name += consonants[r.Next(consonants.Length)];
                b++;
                Name += vowels[r.Next(vowels.Length)];
                b++;
            }

            return Name;
        }

        public static string VietLien(string str)
        {
            if (string.IsNullOrEmpty(str))
                return "";
            return ClearSign(str).Replace(" ", "").Replace("\r", "").ToLower();
        }


        public static string VietLienEx(string str)
        {
            if (string.IsNullOrEmpty(str))
                return "";
            return ClearSign(str);
        }


        public static string CleanName(string str)
        {
            str = VietLien(str);
            StringBuilder sb = new StringBuilder();
            foreach(char c in str)
            {
                if (Regex.IsMatch(c.ToString(), "[A-Za-z0-9]"))
                    sb.Append(c);
            }
            return sb.ToString();
        }

        public static string ConvertToUnsign3(string str)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = str.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty)
                        .Replace('\u0111', 'd').Replace('\u0110', 'D');
        }

        public static string RemoveVietnameseTone(string text)
        {
            string result = text.ToLower();
            result = Regex.Replace(result, "à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ|/g", "a");
            result = Regex.Replace(result, "è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ|/g", "e");
            result = Regex.Replace(result, "ì|í|ị|ỉ|ĩ|/g", "i");
            result = Regex.Replace(result, "ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ|/g", "o");
            result = Regex.Replace(result, "ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ|/g", "u");
            result = Regex.Replace(result, "ỳ|ý|ỵ|ỷ|ỹ|/g", "y");
            result = Regex.Replace(result, "đ", "d");
            return result;
        }

        public static bool Contain(string input, string pattern)
        {
            if (pattern == null || pattern == "")
                return false;
            if (TDT.VietLien(input).Contains(TDT.VietLien(pattern)))
                return true;
            return false;
        }

        public static bool Unlock(string fileName)
        {
            bool isLock = false;
            foreach (Process process in Process.GetProcesses())
            {
                try
                {
                    if (process.MainModule.FileName == fileName)
                    {
                        process.Kill();
                        isLock = true;
                    }
                }
                catch { }
            }
            return isLock;
        }

        public static string ClearSign(string str)
        {
            //str = str.Replace(Convert.ToChar(208), 'D');
            for (int i = 1; i < vietnameseSigns.Length; i++)
            {
                for (int j = 0; j < vietnameseSigns[i].Length; j++)
                    str = str.Replace(vietnameseSigns[i][j], vietnameseSigns[0][i - 1]);
            }            
            return str;            
        }

        public static string ReadFile(string name)
        {
            if (!File.Exists(name))
                return "";
            try
            {
                StreamReader sr = new StreamReader(name);
                string s = sr.ReadToEnd();
                sr.Close();
                return s;
            }
            catch { }
            return "";
        }

        public static int SeconNow
        {
            get
            {
                return (DateTime.Now.Month * 30 * 24 * 3600) + (DateTime.Now.Day * 24 * 3600) + (DateTime.Now.Hour * 3600) + (DateTime.Now.Minute * 60) + DateTime.Now.Second;
            }
        }

        public static Random Random = new Random();

        public static void AppendFile(string name, string content)
        {
            try
            {
                content = content + "\r\n" + ReadFile(name);
                if(content.Split('\n').Length > 1000)
                {
                    string newContent = "";
                    for (int i = 0; i < 1000; i++)
                    {
                        newContent += content.Split('\n')[i].Trim() + "\r\n";
                    }
                    content = newContent;
                }
                FileStream fs = new FileStream(name, FileMode.Create);
                fs.Close();
                StreamWriter sw = new StreamWriter(name);
                sw.Write(content);
                sw.Close();
            }
            catch { }
        }

        public static void MoveFile(string source, string des)
        {
            try
            {
                File.Move(source, des);
            }
            catch { }
        }

        public static void DeleteFile(string fileName)
        {
            try
            {
                File.Delete(fileName);
            }
            catch { }
        }

        public static void WriteFile(string name, string content)
        {
            try
            {
                FileStream fs = new FileStream(name, FileMode.Create);
                fs.Close();
                StreamWriter sw = new StreamWriter(name);
                sw.Write(content);
                sw.Close();
            }
            catch { }
        }

        public static void CreateFile(string name, string content)
        {
            try
            {
                var utf8WithoutBom = new UTF8Encoding(false);
                FileStream fs = new FileStream(name, FileMode.Create);
                fs.Close();
                StreamWriter sw = new StreamWriter(name, false, utf8WithoutBom);
                sw.Write(content);
                sw.Close();
            }
            catch { }
        }

        public static bool IsPhoneNumber(string number)
        {
            if (Regex.Match(number, @"^[0]([0-9]{9})$").Success == false)
            {
                return Regex.Match(number, @"^[0]([0-9]{10})$").Success;
            }
            else
            {
                return Regex.Match(number, @"^[0]([0-9]{9})$").Success;
            }
        }

        public static bool IsValidEmail(string email)
        {
            string mathEmailPattern = @"^(([^<>()[\]\\.,;:\s@\""]+"
                      + @"(\.[^<>()[\]\\.,;:\s@\""]+)*)|(\"".+\""))@"
                      + @"((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}"
                      + @"\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+"
                      + @"[a-zA-Z]{2,}))$";
            if (email != null && email != "") return Regex.IsMatch(email, mathEmailPattern);
            else return false;
        }

        public static bool IsPressedCtrl
        {
            get
            {
                return (TDT.IsPressed(VirtualKeyStates.VK_CONTROL) || TDT.IsPressed(VirtualKeyStates.VK_RCONTROL));
            }
        }

        public static bool IsPressedShift
        {
            get
            {
                return (TDT.IsPressed(VirtualKeyStates.VK_LSHIFT) || TDT.IsPressed(VirtualKeyStates.VK_RSHIFT));
            }
        }

        public static bool IsPressedAlt
        {
            get
            {
                return (TDT.IsPressed(VirtualKeyStates.VK_LMENU) || TDT.IsPressed(VirtualKeyStates.VK_RMENU));
            }
        }

        public static bool IsPressed(VirtualKeyStates key)
        {
            int KEY_PRESSED = 0x8000;
            return Convert.ToBoolean(GetKeyState(key) & KEY_PRESSED);
        }
        
        

        [DllImport("user32.dll")]
        static extern short GetKeyState(VirtualKeyStates nVirtKey);        

        [DllImport("user32.dll")]
        public static extern bool CloseWindow(IntPtr hWnd);

        private void UnZip(string file, string unZipTo)
        {
            try
            {
                // Specifying Console.Out here causes diagnostic msgs to be sent to the Console
                // In a WinForms or WPF or Web app, you could specify nothing, or an alternate
                // TextWriter to capture diagnostic messages. 

                //using (ZipFile zip = ZipFile.Read(file))
                //{
                    // This call to ExtractAll() assumes:
                    //   - none of the entries are password-protected.
                    //   - want to extract all entries to current working directory
                    //   - none of the files in the zip already exist in the directory;
                    //     if they do, the method will throw.
                //    zip.ExtractAll(unZipTo);
                //}
            }
            catch (System.Exception)
            {
            }
        }

        public static string HKLM_GetString(string path, string key)
        {
            try
            {
                RegistryKey rk = Registry.LocalMachine.OpenSubKey(path);
                if (rk == null) return "";
                return (string)rk.GetValue(key);
            }
            catch { return ""; }
        }

        public static int Hex2Int(string hex)
        {
            if (hex == "??")
                return -1;
            int result = -1;
            int.TryParse(hex, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out result);
            return result;
        }

        public static string FriendlyName()
        {
            string ProductName = HKLM_GetString(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ProductName");
            string CSDVersion = HKLM_GetString(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", "CSDVersion");
            if (ProductName != "")
            {
                return (ProductName.StartsWith("Microsoft") ? "" : "Microsoft ") + ProductName +
                            (CSDVersion != "" ? " " + CSDVersion : "");
            }
            return "";
        }

        public class Hasher
        {
            private Hasher() { }

            private static byte[] ConvertStringToByteArray(string data)
            {
                return (new UnicodeEncoding()).GetBytes(data);
            }

            private static FileStream GetFileStream(string pathName)
            {
                return (new FileStream(pathName, System.IO.FileMode.Open,
                          FileAccess.Read, System.IO.FileShare.ReadWrite));
            }

            public static string SHA1(string pathName)
            {
                string strResult = "";
                string strHashData = "";

                byte[] arrbytHashValue;
                FileStream oFileStream = null;

                SHA1CryptoServiceProvider oSHA1Hasher =
                           new SHA1CryptoServiceProvider();

                try
                {
                    oFileStream = GetFileStream(pathName);
                    arrbytHashValue = oSHA1Hasher.ComputeHash(oFileStream);
                    oFileStream.Close();

                    strHashData = System.BitConverter.ToString(arrbytHashValue);
                    strHashData = strHashData.Replace("-", "");
                    strResult = strHashData;
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error!",
                             MessageBoxButtons.OK,
                             System.Windows.Forms.MessageBoxIcon.Error,
                             MessageBoxDefaultButton.Button1);
                }

                return (strResult);
            }

            public static string MD5(string input)
            {
                string strResult = "";
                string strHashData = "";

                byte[] arrbytHashValue;
                System.IO.FileStream oFileStream = null;

                System.Security.Cryptography.MD5CryptoServiceProvider oMD5Hasher =
                           new System.Security.Cryptography.MD5CryptoServiceProvider();

                try
                {
                    if (File.Exists(input))
                    {
                        oFileStream = GetFileStream(input);
                        arrbytHashValue = oMD5Hasher.ComputeHash(oFileStream);
                        oFileStream.Close();

                        strHashData = System.BitConverter.ToString(arrbytHashValue);
                        strHashData = strHashData.Replace("-", "");
                        strResult = strHashData;
                    }
                    else
                    {
                        MD5 md5 = System.Security.Cryptography.MD5.Create();
                        byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                        byte[] hash = md5.ComputeHash(inputBytes);

                        // step 2, convert byte array to hex string
                        StringBuilder sb = new StringBuilder();
                        for (int i = 0; i < hash.Length; i++)
                        {
                            sb.Append(hash[i].ToString("X2"));
                        }
                        strResult = sb.ToString();
                    }
                }
                catch
                {
                    // step 1, calculate MD5 hash from input
                    MD5 md5 = System.Security.Cryptography.MD5.Create();
                    byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                    byte[] hash = md5.ComputeHash(inputBytes);

                    // step 2, convert byte array to hex string
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < hash.Length; i++)
                    {
                        sb.Append(hash[i].ToString("X2"));
                    }
                    strResult = sb.ToString();
                }

                return (strResult.ToUpper());
            }

            /// <summary>
            /// Encrypt a string using dual encryption method. Return a encrypted cipher Text
            /// </summary>
            /// <param name="toEncrypt">string to be encrypted</param>
            /// <param name="useHashing">use hashing? send to for extra secirity</param>
            /// <returns></returns>
            public static string Encrypt(string toEncrypt, string key)
            {
                byte[] keyArray;
                byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

                System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();
                // Get the key from config file
                //string key = (string)settingsReader.GetValue("SecurityKey", typeof(String));
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                hashmd5.Clear();

                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                tdes.Key = keyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = tdes.CreateEncryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                tdes.Clear();
                return Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }
            /// <summary>
            /// DeCrypt a string using dual encryption method. Return a DeCrypted clear string
            /// </summary>
            /// <param name="cipherString">encrypted string</param>
            /// <param name="useHashing">Did you use hashing to encrypt this data? pass true is yes</param>
            /// <returns></returns>
            public static string Decrypt(string cipherString, string key)
            {
                byte[] keyArray;
                byte[] toEncryptArray = Convert.FromBase64String(cipherString);

                System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();
                //Get your key from config file to open the lock!
                //string key = (string)settingsReader.GetValue("SecurityKey", typeof(String));

                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                hashmd5.Clear();

                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                tdes.Key = keyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = tdes.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

                tdes.Clear();
                return UTF8Encoding.UTF8.GetString(resultArray);
            }
        }
    }
}
