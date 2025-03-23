using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;


namespace _i
{
    class Global
    {

        public static string HomePage = "http://45.77.129.211";
        public static IniParser IniParser { get; set; }
        public static bool IsBankFull { get; set; }
        public static bool IsBinhThanh { get; set; }
        public static bool IsDungDoat { get; set; }

        public static List<string> lstketban = new List<string>();
        public static List<string> lstsudo = new List<string>();
        public static bool PSShop { get; set; }

        public static bool IsThangCap { get; set; } = false;
        public static bool IsQuanSonHai { get; set; } = false;
        public static bool IsTamKy { get; set; } = false;

    
        public static bool IsChucPhucCungMay { get; set; }
        public static string Version
        {
            get
            {
                return "MicroAuto 40.5.0";

            }
        }
        public static bool IsHopDienTichVoHon { get; set; }
        public static int BuffPetPercent { get; set; }


        public static int MaxBong { get; set; } = 1;

        public static Dictionary<string, string> Captchas { get; set; } = new Dictionary<string, string>();



        public static bool IsChongKsBong { get; set; }

        public static int ChongX { get; set; }
        public static int ChongY { get; set; }

        public static bool IsOutOnline { get; set; }
        public static int OutOnlineMin { get; set; } = 179;

        public static bool IsOutIdle { get; set; }

        public static int OutIdleMin { get; set; } = 6;

        public static bool IsAutoDiemDanh { get; set; }

        public static bool IsSuDoo { get; set; }

        public static bool IsHuyBienThan { get; set; }

        public static bool IsKhongGomVang { get; set; }
        public static bool IsBigBall { get; set; }

        public static bool Is9Sao { get; set; }
        public static int ThoiBongX { get; set; } = 100;
        public static int ThoiBongY { get; set; } = 100;
        public static bool IsHetBossVeLacDuong { get; set; }
        public static bool IsBoBoss { get; set; }

        public static bool IsThoiBong { get; set; }
        public static bool IsQuanDoanBinhThanh
        {
            get
            {
                return IsBinhThanh;
            }
        }

        public static string linknew { get; set; }
        public static uint addmaytinh { get; set; }
        public static bool IsNhanBong { get; set; }
        public static bool IsMaTac { get; set; }
        public static bool IsBaoDoHiem { get; set; }

        public static bool IsBossMap { get; set; } = true;

        

        public static bool IsSmartBTD { get; set; } = false;


        public static int MinMaTac { get; set; } = 15;

        public static bool IsHoaHong { get; set; }

        public static bool IsKsThuongHoi { get; set; }


        public static bool IsLayVang { get; set; } = false;

        public static bool IsPhucDia { get; set; } = false;


       // public static bool IsLLTBNhanh { get; set; }

        public static bool IsS23 { get; set; }

        public static bool IsByPass { get; set; }

        public static bool IsSinhTieu { get; set; }


        public static bool IsNotDec { get; set; }

        public static bool IsNemBienThan { get; } = true;

        public static bool Is247 { get; } = true;


        public static bool IsBonKhacMay { get; set; }

        public static bool IsReportBad { get; set; } = true;

        public static bool IsChienBi { get; set; }

        public static bool IsForceFollow { get; } = true;

        public static bool IsTamThan { get; set; }

        public static bool IsChong { get; } = true;

        public static bool IsThuHoachHoa { get; set; }

        public static bool IsGom { get; set; }
        public static bool IsGomKNB { get; set; }
        public static bool IsUuTienBeri { get; set; } = true;

        public static bool IsSuaTrangBi { get; set; }

        //public static bool IsNhapCode { get; set; }


        public static Stopwatch CaptchaIdleTime { get; set; } = new Stopwatch();

        public static bool IsSmart { get;  } = true;

        public static bool IsBachBao { get; set; }

        public static void SaveCaptcha()
        {
            while (Game.CaptchaHash.Count > 500)
                Game.CaptchaHash.RemoveAt(0);
            StringBuilder sb = new StringBuilder();
            foreach (string s in Game.CaptchaHash)
            {                
                sb.AppendLine(s);
            }
            Game.IniParser.Write("Logs", "CaptchaHash", sb.ToString());
        }
        //lecaotri
        public static bool IsPhucLoi { get; set; }
        public static bool IsKNB { get; } = true;
        public static bool IsChanNguyen { get; } = true;
        public static bool IsCanQuet { get; set; }
        public static bool IsLaoHuu { get; } = true;

        public static int MaxSuMon { get; set; } = 20;
        public static int MaxTruAc { get; set; } = 5;


    
        public static bool IsYTO { get; set; }
        public static bool IsKS { get; set; }
        public static bool IsTKC { get; set; }
        public static bool IsTTT { get; set; }
        public static bool IsTC { get; set; }        
        public static bool IsNhanQuaThangCap { get; set; }
        public static bool IsThuTieuDatTai { get; set; }
        static bool isSatTinh;

        public static bool IsSatTinh
        {
            get
            {
                if (Global.IsVIP > 0)
                    return true;
                return isSatTinh;
            }
            set
            {
                isSatTinh = value;
            }
        }


   
        public static bool Is2CaptchaEx { get; set; }
        public static bool IsReg { get; set; }


        public static bool ThapDienMaiPhuc { get; set; }

        public static bool AntiLag { get; set; }

        public static bool IsChonMayChuThayChoThoatGame { get; set; } = true;

        public static bool AutoDropCraft = false;

        public static DateTime ExaclyTime = DateTime.MinValue;

        public static int YearExp { get; set; }

        public static bool IsVutRac { get { return false; } }



        public static string PublisherVersion
        {
            get
            {
                return "3.3";
            }
        }

        public static bool IsAdmin
        {
            get
            {
                return false;
            }
        }

        public static bool IsAdminEx
        {
            get
            {                
                if (User.Email == "0975716490" || User.Email == "0936543800" || User.Email == "lecaotri@yahoo.com" || User.Email == "tieudattai@yahoo.com")
                    return true;
                return false;
            }
        }

        public static bool IsAutoCreate { get; set; }

        public static bool IsKhongChiemManHinh => false;

        public static int OutLogin { get; set; } = 10;



   


      

        public static bool IsCode { get; set; }
        public static bool IsPackUpDanBuon { get; set; }


        public static bool IsAllowFree
        {
            get
            {
                return true;
            }
        }

        public static bool IsTrimRam = true;

  

  


        public static bool IsXuat { get; set; }

        public static int PetLvl = 199;

        public static int Wait = 2;
        public static int TimeLive
        {
            get;
            set;
        }
        public static bool LanLuot
        {
            get;
            set;
        }

        
        public static int IsFull;

        public static bool IsAnti;
        public static string OFFSET;
        //public static int TickCount;
        public static int HookMessage;
        public static bool BuffPet = true;
        public static bool ItemFillter = true;
        public static bool AtkFollowKey = false;
        public static int BuffHPPercent = 50;
        public static int BuffMPPercent = 50;
        public static int BuffNMPercent = 75;
        public static int ExitHPPercent = 10;
        public static int NoiRadius = 20;
        public static int NgoaiRadius = 15;
        //public static int PickRadius = 30;
        public static bool ExitPk = false;
        public static bool AlarmPk = true;
        //public static bool AlarmHP = true;
        //public static int AlarmHPPercent = 30;
        public static bool Mute = false;
        //public static bool AutoComeBack = false;
        public static bool IsPickItem = true;
        public static bool AutoUpLvl = false;
        public static bool ForceFollowKey = false;
        public static bool FollowKey = false;
        public static int AutoUpLvlBelow = 42;
        public static bool AutoShutDown = false;
        public static bool UseSkillPet = true;
        public static bool AutoResetTime = false;
        public static bool AutoResetTimeAll => false;
        public static bool AutoUseLocked = false;
        public static bool BuffQuanDoan = true;
        public static bool IsMaxPet { get; set; }
        public static bool PartyAcceptAll = true;
        public static bool Paused = false;
        public static bool IsXaPhu = true;
        public static Keys BaseSkill = Keys.F1;
        public static Keys NMSkill = Keys.F13;
        public static Keys HPKey = Keys.F13;
        public static bool IsHuyDanhQuai = false;
        public static bool IsHuyThaiHo = false;
        public static bool IsHuyHyHuu = false;
        public static int Speed = 3;
        //public static int FollowRadius = 3;
        public static int MaxBHD = 20;
        public static bool AntiCaptcha { get; set; }
        public static bool AntiCaptchaSelf => AntiCaptcha;
        public static bool AntiCaptcha2Captcha { get { return true; } }
        public static bool AntiCaptchaSelf2Captcha
        {
            get
            {
                return false;
            }
        }
        public static bool AutoPk = true;
        public static int MaxLogin = 30;//lecaotri2020

        public static bool IsTuVaoPhai = false;

        public static int XDua = 134;
        public static bool openbang = false;
        public static int ThanhSangIndex = 1;
        public static bool Mapbang = false;
        public static int YDua = 165;
        public static int MapDua = MAP.TayHo;

        public static List<string> BangOnPC = new List<string>();
        public static List<string> DoNgonDua = new List<string>();


        //lecaotri
        public static int MinNv { get; set; } = 0;

        public static int GlSetMenPai
        {
            get;
            set;
        }

        public static bool GlIsSetMenPai
        {
            get;
            set;
        }
        public static int IsVIP
        {
            get;
            set;
        }

        public static bool IsMoBTD { get; set; }

        public static bool RemoveAd
        {
            get;
            set;
        }

        public static string SelfMd5
        {
            get
            {
                return TDT.Hasher.MD5(Application.ExecutablePath);
            }
        }

    


     

  

      

        public static string APPPath
        {
            get
            {
                return Path.GetDirectoryName(Application.ExecutablePath);
            }
        }

        public static string Argument
        {
            get
            {
                return "\"" + Application.ExecutablePath + "\"";
            }
        }
    }
}
