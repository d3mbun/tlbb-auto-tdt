using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _i.STATIC
{
    class AutoMove
    {
        public static int TimIndex(string maphientai, string maptieptheo)
        {
            if (maphientai == "daily")
            {
                if (maptieptheo == "nhihai") return 3;
                if (maptieptheo == "kiemcac") return 4;
            }
            if (maphientai == "nhihai")
            {
                if (maptieptheo == "daily") return 2;
                if (maptieptheo == "thachlam") return 3;
                if (maptieptheo == "thuongson") return 1;
            }
            if (maphientai == "thuongson")
            {
                if (maptieptheo == "nhihai") return 2;
            }
            if (maphientai == "thachlam")
            {
                if (maptieptheo == "nhihai") return 1;
                if (maptieptheo == "ngockhue") return 5;
                if (maptieptheo == "namchieu") return 2;
            }
            if (maphientai == "ngockhue")
            {
                if (maptieptheo == "thachlam") return 1;
            }
            if (maphientai == "namchieu")
            {
                if (maptieptheo == "thachlam") return 2;
                if (maptieptheo == "mieucuong") return 1;
            }
            if (maphientai == "mieucuong")
            {
                if (maptieptheo == "namchieu") return 1;
            }
            if (maphientai == "kiemcac")
            {
                if (maptieptheo == "daily") return 2;
                if (maptieptheo == "donhoang") return 3;
            }
            if (maphientai == "donhoang")
            {
                if (maptieptheo == "kiemcac") return 2;
                if (maptieptheo == "lacduong") return 3;
            }
            if (maphientai == "lacduong")
            {
                if (maptieptheo == "donhoang") return 4;
                if (maptieptheo == "nhannam") return 6;
                if (maptieptheo == "tungson") return 5;
            }
            if (maphientai == "nhannam")
            {
                if (maptieptheo == "lacduong") return 2;
                if (maptieptheo == "nhanbac") return 4;
                if (maptieptheo == "thaonguyen") return 1;
            }
            if (maphientai == "nhanbac")
            {
                if (maptieptheo == "nhannam") return 3;
            }
            if (maphientai == "thaonguyen")
            {
                if (maptieptheo == "lieutay") return 2;
                if (maptieptheo == "nhannam") return 4;
                if (maptieptheo == "truongbachson") return 1;
            }
            if (maphientai == "lieutay")
            {
                if (maptieptheo == "thaonguyen") return 1;
            }
            if (maphientai == "truongbachson")
            {
                if (maptieptheo == "thaonguyen") return 1;
                if (maptieptheo == "hoanglongphu") return 2;
            }
            if (maphientai == "hoanglongphu")
            {
                if (maptieptheo == "truongbachson") return 1;
            }
            if (maphientai == "tungson")
            {
                if (maptieptheo == "lacduong") return 1;
                if (maptieptheo == "thaiho") return 3;
            }
            if (maphientai == "thaiho")
            {
                if (maptieptheo == "tungson") return 3;
                if (maptieptheo == "tochau") return 4;
            }
            if (maphientai == "tochau")
            {
                if (maptieptheo == "tayho") return 6;
                if (maptieptheo == "thaiho") return 5;
            }
            if (maphientai == "tayho")
            {
                if (maptieptheo == "tochau") return 3;
                if (maptieptheo == "longtuyen") return 1;
                if (maptieptheo == "vodi") return 4;
            }
            if (maphientai == "longtuyen")
            {
                if (maptieptheo == "tayho") return 2;
            }
            if (maphientai == "vodi")
            {
                if (maptieptheo == "tayho") return 4;
                if (maptieptheo == "mailinh") return 2;
                if (maptieptheo == "namhai") return 3;
            }
            if (maphientai == "mailinh")
            {
                if (maptieptheo == "vodi") return 2;
            }
            if (maphientai == "namhai")
            {
                if (maptieptheo == "vodi") return 3;
                if (maptieptheo == "quynhchau") return 2;
            }
            if (maphientai == "quynhchau")
            {
                if (maptieptheo == "namhai") return 1;
            }
            return -1;
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
            if (mapid == 34) return "namhai";
            if (mapid == 35) return "quynhchau";
            return "khongbiet";
        }

        public static int MapToNumber(string mapname) {
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
            return -1;
        }
    }
}
