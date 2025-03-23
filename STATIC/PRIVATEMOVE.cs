using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _i
{
    class PRIVATEMOVE
    {
        public string MapCanDen = string.Empty;
        public string MapHienTai = string.Empty;

        public bool ChuaDiQua(string map)
        {
            if (ArrMap.Contains(map))
            {
                return false;
            }
            return true;
        }

        public List<string> ArrMap = new List<string>();
        public bool ChuaTimThay = true;

      

        public void TimDuongDi(string maphientai)
        {
            ArrMap.Add(maphientai);

            if (MapCanDen == maphientai)
            {
                ChuaTimThay = false;
                return;
            }
            if (maphientai == "daily")
            {
                if (ChuaDiQua("nhihai") && ChuaTimThay) { TimDuongDi("nhihai"); }
                if (ChuaDiQua("kiemcac") && ChuaTimThay) { TimDuongDi("kiemcac"); }
                if (ChuaDiQua("voluongson") && ChuaTimThay) { TimDuongDi("voluongson"); }
                if (ChuaTimThay) { ArrMap.Remove(maphientai); }
            }
            else if (maphientai == "nhihai")
            {
                if (ChuaDiQua("daily") && ChuaTimThay) { TimDuongDi("daily"); }
                if (ChuaDiQua("thuongson") && ChuaTimThay) { TimDuongDi("thuongson"); }
                if (ChuaDiQua("thachlam") && ChuaTimThay) { TimDuongDi("thachlam"); }
                if (ChuaTimThay) { ArrMap.Remove(maphientai); }
            }
            else if (maphientai == "thuongson")
            {
                if (ChuaDiQua("nhihai") && ChuaTimThay) { TimDuongDi("nhihai"); }
                if (ChuaTimThay) { ArrMap.Remove(maphientai); }
            }
            else if (maphientai == "thachlam")
            {
                if (ChuaDiQua("nhihai") && ChuaTimThay) { TimDuongDi("nhihai"); }
                if (ChuaDiQua("ngockhue") && ChuaTimThay) { TimDuongDi("ngockhue"); }
                if (ChuaDiQua("namchieu") && ChuaTimThay) { TimDuongDi("namchieu"); }
                if (ChuaTimThay) { ArrMap.Remove(maphientai); }
            }
            else if (maphientai == "ngockhue")
            {
                if (ChuaDiQua("thachlam") && ChuaTimThay) { TimDuongDi("thachlam"); }
                if (ChuaTimThay) { ArrMap.Remove(maphientai); }
            }
            else if (maphientai == "namchieu")
            {
                if (ChuaDiQua("thachlam") && ChuaTimThay) { TimDuongDi("thachlam"); }
                if (ChuaDiQua("mieucuong") && ChuaTimThay) { TimDuongDi("mieucuong"); }
                if (ChuaTimThay) { ArrMap.Remove(maphientai); }
            }
            else if (maphientai == "mieucuong")
            {
                if (ChuaDiQua("namchieu") && ChuaTimThay) { TimDuongDi("namchieu"); }
                if (ChuaTimThay) { ArrMap.Remove(maphientai); }
            }
            else if (maphientai == "kiemcac")
            {
                if (ChuaDiQua("daily") && ChuaTimThay) { TimDuongDi("daily"); }
                if (ChuaDiQua("donhoang") && ChuaTimThay) { TimDuongDi("donhoang"); }
                if (ChuaTimThay) { ArrMap.Remove(maphientai); }
            }
            else if (maphientai == "donhoang")
            {
                if (ChuaDiQua("kiemcac") && ChuaTimThay) { TimDuongDi("kiemcac"); }
                if (ChuaDiQua("lacduong") && ChuaTimThay) { TimDuongDi("lacduong"); }
                if (ChuaTimThay) { ArrMap.Remove(maphientai); }
            }
            else if (maphientai == "lacduong")
            {
                if (ChuaDiQua("donhoang") && ChuaTimThay) { TimDuongDi("donhoang"); }
                if (ChuaDiQua("nhannam") && ChuaTimThay) { TimDuongDi("nhannam"); }
                if (ChuaDiQua("tungson") && ChuaTimThay) { TimDuongDi("tungson"); }
                if (ChuaTimThay) { ArrMap.Remove(maphientai); }
            }
            else if (maphientai == "nhannam")
            {
                if (ChuaDiQua("lacduong") && ChuaTimThay) { TimDuongDi("lacduong"); }
                if (ChuaDiQua("nhanbac") && ChuaTimThay) { TimDuongDi("nhanbac"); }
                if (ChuaDiQua("thaonguyen") && ChuaTimThay) { TimDuongDi("thaonguyen"); }
                if (ChuaTimThay) { ArrMap.Remove(maphientai); }
            }
            else if (maphientai == "nhanbac")
            {
                if (ChuaDiQua("nhannam") && ChuaTimThay) { TimDuongDi("nhannam"); }
                if (ChuaTimThay) { ArrMap.Remove(maphientai); }
            }
            else if (maphientai == "thaonguyen")
            {
                if (ChuaDiQua("lieutay") && ChuaTimThay) { TimDuongDi("lieutay"); }
                if (ChuaDiQua("nhannam") && ChuaTimThay) { TimDuongDi("nhannam"); }
                if (ChuaDiQua("truongbachson") && ChuaTimThay) { TimDuongDi("truongbachson"); }
                if (ChuaTimThay) { ArrMap.Remove(maphientai); }
            }
            else if (maphientai == "lieutay")
            {
                if (ChuaDiQua("thaonguyen") && ChuaTimThay) { TimDuongDi("thaonguyen"); }
                if (ChuaTimThay) { ArrMap.Remove(maphientai); }
            }
            else if (maphientai == "truongbachson")
            {
                if (ChuaDiQua("thaonguyen") && ChuaTimThay) { TimDuongDi("thaonguyen"); }
                if (ChuaDiQua("hoanglongphu") && ChuaTimThay) { TimDuongDi("hoanglongphu"); }
                if (ChuaTimThay) { ArrMap.Remove(maphientai); }
            }
            else if (maphientai == "hoanglongphu")
            {
                if (ChuaDiQua("truongbachson") && ChuaTimThay) { TimDuongDi("truongbachson"); }
                if (ChuaTimThay) { ArrMap.Remove(maphientai); }
            }
            else if (maphientai == "tungson")
            {
                if (ChuaDiQua("lacduong") && ChuaTimThay) { TimDuongDi("lacduong"); }
                if (ChuaDiQua("thaiho") && ChuaTimThay) { TimDuongDi("thaiho"); }
                if (ChuaTimThay) { ArrMap.Remove(maphientai); }
            }
            else if (maphientai == "thaiho")
            {
                if (ChuaDiQua("tungson") && ChuaTimThay) { TimDuongDi("tungson"); }
                if (ChuaDiQua("tochau") && ChuaTimThay) { TimDuongDi("tochau"); }
                if (ChuaTimThay) { ArrMap.Remove(maphientai); }
            }
            else if (maphientai == "tochau")
            {
                if (ChuaDiQua("thaiho") && ChuaTimThay) { TimDuongDi("thaiho"); }
                if (ChuaDiQua("tayho") && ChuaTimThay) { TimDuongDi("tayho"); }
                if (ChuaDiQua("kinhho") && ChuaTimThay) { TimDuongDi("kinhho"); }
                if (ChuaTimThay) { ArrMap.Remove(maphientai); }
            }
            else if (maphientai == "kinhho")
            {
                if (ChuaDiQua("tochau") && ChuaTimThay) { TimDuongDi("tochau"); }
                if (ChuaDiQua("voluongson") && ChuaTimThay) { TimDuongDi("voluongson"); }
                if (ChuaTimThay) { ArrMap.Remove(maphientai); }
            }
            else if (maphientai == "voluongson")
            {
                if (ChuaDiQua("daily") && ChuaTimThay) { TimDuongDi("daily"); }
                if (ChuaDiQua("kinhho") && ChuaTimThay) { TimDuongDi("kinhho"); }
                if (ChuaTimThay) { ArrMap.Remove(maphientai); }
            }
            else if (maphientai == "tayho")
            {
                if (ChuaDiQua("tochau") && ChuaTimThay) { TimDuongDi("tochau"); }
                if (ChuaDiQua("longtuyen") && ChuaTimThay) { TimDuongDi("longtuyen"); }
                if (ChuaDiQua("vodi") && ChuaTimThay) { TimDuongDi("vodi"); }
                if (ChuaTimThay) { ArrMap.Remove(maphientai); }
            }
            else if (maphientai == "longtuyen")
            {
                if (ChuaDiQua("tayho") && ChuaTimThay) { TimDuongDi("tayho"); }
                if (ChuaTimThay) { ArrMap.Remove(maphientai); }
            }
            else if (maphientai == "vodi")
            {
                if (ChuaDiQua("tayho") && ChuaTimThay) { TimDuongDi("tayho"); }
                if (ChuaDiQua("mailinh") && ChuaTimThay) { TimDuongDi("mailinh"); }
                if (ChuaDiQua("namhai") && ChuaTimThay) { TimDuongDi("namhai"); }
                if (ChuaTimThay) { ArrMap.Remove(maphientai); }
            }
            else if (maphientai == "mailinh")
            {
                if (ChuaDiQua("vodi") && ChuaTimThay) { TimDuongDi("vodi"); }
                if (ChuaTimThay) { ArrMap.Remove(maphientai); }
            }
            else if (maphientai == "namhai")
            {
                if (ChuaDiQua("quynhchau") && ChuaTimThay) { TimDuongDi("quynhchau"); }
                if (ChuaDiQua("vodi") && ChuaTimThay) { TimDuongDi("vodi"); }
                if (ChuaTimThay) { ArrMap.Remove(maphientai); }
            }
            else if (maphientai == "quynhchau")
            {
                if (ChuaDiQua("namhai") && ChuaTimThay) { TimDuongDi("namhai"); }
                if (ChuaTimThay) { ArrMap.Remove(maphientai); }
            }
        }

        public string TimIndex(string maphientai, string maptieptheo)
        {
            if (maphientai == "kinhho")
            {
                if (maptieptheo == "tochau") { return "285,44"; }
                if (maptieptheo == "voluongson") { return "38,284"; }
            }
            if (maphientai == "voluongson")
            {
                if (maptieptheo == "daily") { return "32,176"; }
                if (maptieptheo == "kinhho") { return "287,77"; }
            }
            if (maphientai == "daily")
            {
                if (maptieptheo == "nhihai") { return "159,294"; }
                if (maptieptheo == "kiemcac") { return "31,148"; }
                if (maptieptheo == "voluongson") { return "304,149"; }
            }
            if (maphientai == "nhihai")
            {
                if (maptieptheo == "daily") { return "287,32"; }
                if (maptieptheo == "thachlam") { return "34,279"; }
                if (maptieptheo == "thuongson") { return "34,157"; }
            }
            if (maphientai == "thuongson")
            {
                if (maptieptheo == "nhihai") { return "286,56"; }
            }
            if (maphientai == "thachlam")
            {
                if (maptieptheo == "nhihai") { return "268,128"; }
                if (maptieptheo == "ngockhue") { return "112,34"; }
                if (maptieptheo == "namchieu") { return "33,250"; }
            }
            if (maphientai == "ngockhue")
            {
                if (maptieptheo == "thachlam") { return "36,46"; }
            }
            if (maphientai == "namchieu")
            {
                if (maptieptheo == "thachlam") { return "280,45"; }
                if (maptieptheo == "mieucuong") { return "106,285"; }
            }
            if (maphientai == "mieucuong")
            {
                if (maptieptheo == "namchieu") { return "67,32"; }
            }
            if (maphientai == "kiemcac")
            {
                if (maptieptheo == "daily") { return "36,286"; }
                if (maptieptheo == "donhoang") { return "105,40"; }
            }
            if (maphientai == "donhoang")
            {
                if (maptieptheo == "kiemcac") { return "231,286"; }
                if (maptieptheo == "lacduong") { return "284,146"; }
            }
            if (maphientai == "lacduong")
            {
                if (maptieptheo == "donhoang") { return "45,248"; }
                if (maptieptheo == "nhannam") { return "448,259"; }
                if (maptieptheo == "tungson") { return "255,462"; }
            }
            if (maphientai == "nhannam")
            {
                if (maptieptheo == "lacduong") { return "264,286"; }
                if (maptieptheo == "nhanbac") { return "248,37"; }
                if (maptieptheo == "thaonguyen") { return "33,58"; }
            }
            if (maphientai == "nhanbac")
            {
                if (maptieptheo == "nhannam") { return "228,280"; }
                if (maptieptheo == "thaolieutruong") { return "59,60"; }
            }
            if (maphientai == "thaolieutruong")
            {
                if (maptieptheo == "nhanbac") { return "23,228"; }
            }
            if (maphientai == "thaonguyen")
            {
                if (maptieptheo == "lieutay") { return "119,32"; }
                if (maptieptheo == "nhannam") { return "287,58"; }
                if (maptieptheo == "truongbachson") { return "271,33"; }
            }
            if (maphientai == "lieutay")
            {
                if (maptieptheo == "thaonguyen") { return "61,297"; }
            }
            if (maphientai == "truongbachson")
            {
                if (maptieptheo == "thaonguyen") { return "33,269"; }
                if (maptieptheo == "hoanglongphu") { return "286,73"; }
            }
            if (maphientai == "hoanglongphu")
            {
                if (maptieptheo == "truongbachson") { return "48,287"; }
            }
            if (maphientai == "tungson")
            {
                if (maptieptheo == "lacduong") { return "34,55"; }
                if (maptieptheo == "thaiho") { return "286,247"; }
            }
            if (maphientai == "thaiho")
            {
                if (maptieptheo == "tungson") { return "91,23"; }
                if (maptieptheo == "tochau") { return "215,281"; }
            }
            if (maphientai == "tochau")
            {
                if (maptieptheo == "tayho") { return "278,412"; }
                if (maptieptheo == "thaiho") { return "279,44"; }
                if (maptieptheo == "kinhho") { return "65,270"; }
            }
            if (maphientai == "tayho")
            {
                if (maptieptheo == "tochau") { return "34,50"; }
                if (maptieptheo == "longtuyen") { return "287,54"; }
                if (maptieptheo == "vodi") { return "265,286"; }
            }
            if (maphientai == "longtuyen")
            {
                if (maptieptheo == "tayho") { return "34,274"; }
            }
            if (maphientai == "vodi")
            {
                if (maptieptheo == "tayho") { return "36,33"; }
                if (maptieptheo == "mailinh") { return "283,180"; }
                if (maptieptheo == "namhai") { return "248,286"; }
            }
            if (maphientai == "mailinh")
            {
                if (maptieptheo == "vodi") { return "33,275"; }
            }
            if (maphientai == "namhai")
            {
                if (maptieptheo == "vodi") { return "34,36"; }
                if (maptieptheo == "quynhchau") { return "286,174"; }
            }
            if (maphientai == "quynhchau")
            {
                if (maptieptheo == "namhai") { return "279,286"; }
            }
            return "0,0";
        }

        public static string MapToString(int mapid)
        {
            if (mapid == 0) return "lacduong";
            if (mapid == 1) return "tochau";
            if (mapid == 2) return "daily";
            if (mapid == 3) return "tungson";
            if (mapid == 4) return "thaiho";
            if (mapid == 7) return "kiemcac";
            if (mapid == 8) return "donhoang";
            if (mapid == 18) return "nhannam";
            if (mapid == 19) return "nhanbac";
            if (mapid == 20) return "thaonguyen";
            if (mapid == 21) return "lieutay";
            if (mapid == 22) return "truongbachson";
            if (mapid == 23) return "hoanglongphu";
            if (mapid == 24) return "nhihai";
            if (mapid == 25) return "thuongson";
            if (mapid == 26) return "thachlam";
            if (mapid == 27) return "ngockhue";
            if (mapid == 28) return "namchieu";
            if (mapid == 29) return "mieucuong";
            if (mapid == 30) return "tayho";
            if (mapid == 31) return "longtuyen";
            if (mapid == 32) return "vodi";
            if (mapid == 33) return "mailinh";
            if (mapid == MAP.NamHai) return "namhai";
            if (mapid == MAP.QuynhChau) return "quynhchau";
            if (mapid == MAP.ThaoLieuTruong) return "thaolieutruong";
            return "";
        }

        public static int MapToNumber(string mapname)
        {
            if (mapname == "lacduong") return 0;
            if (mapname == "tochau") return 1;
            if (mapname == "daily") return 2;
            if (mapname == "tungson") return 3;
            if (mapname == "thaiho") return 4;
            if (mapname == "kiemcac") return 7;
            if (mapname == "donhoang") return 8;
            if (mapname == "nhannam") return 18;
            if (mapname == "nhanbac") return 19;
            if (mapname == "thaonguyen") return 20;
            if (mapname == "lieutay") return 21;
            if (mapname == "truongbachson") return 22;
            if (mapname == "hoanglongphu") return 23;
            if (mapname == "nhihai") return 24;
            if (mapname == "thuongson") return 25;
            if (mapname == "thachlam") return 26;
            if (mapname == "ngockhue") return 27;
            if (mapname == "namchieu") return 28;
            if (mapname == "mieucuong") return 29;
            if (mapname == "tayho") return 30;
            if (mapname == "longtuyen") return 31;
            if (mapname == "vodi") return 32;
            if (mapname == "mailinh") return 33;
            if (mapname == "namhai") return 34;
            if (mapname == "quynhchau") return 35;
            if (mapname == "thaolieutruong") return MAP.ThaoLieuTruong;
            return -1;
        }
    }
}
