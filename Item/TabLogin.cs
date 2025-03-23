using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using System.Diagnostics;
using System.IO;
using System.Web;
using System.Threading;
using System.Linq;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace _i
{
    partial class TabLogin : UserControl
    {
        public static List<TabLogin> List { get; set; } = new List<TabLogin>();

        public TabLogin()
        {

            InitializeComponent();
            MicroLogin.Instance.splitContainer1.Visible = false;
            LoginUser loginUser = new LoginUser();
            this.Controls.Add(loginUser);
            loginUser.Disposed += LoginUser_Disposed;
            loginUser.Dock = DockStyle.Fill;
            loginUser.BringToFront();


        }


        public bool IsReset = false;
        public AccountEx AccountEx;


        private void Login(AccountEx account)
        {
            foreach (KeyValuePair<int, Game> kvp in Main.DicGame.Where(kvp => !kvp.Value.IsSelectLogin && !kvp.Value.Process.HasExited).ToList())
            {
                try
                {
                    if (Accounts.Where(a => a.game == kvp.Value).Count() > 0)
                        continue;
                    if (kvp.Value.TLBB.IsSelectServer || kvp.Value.TLBB.IsNexLogin)
                    {
                        account.Status = "Login";
                        account.game = kvp.Value;
                        account.IsOnline = false;
                        account.game.IsSelectLogin = true;
                        account.game.AccountEx = account;
                        account.game.BaseImg = 0;
                        account.Entered = false;
                        account.IsSelectRole = false;
                        account.game.BHDCount = 0;
                        account.game.IsXongBHD = false;
                        account.game.MissionState = "";
                        account.game.listCheckedOnline.Add(Handle);
                        account.game.swStandTime = Stopwatch.StartNew();
                        return;
                    }
                }
                catch { }
            }
            bool isOpen = true;
            if (account.swExitTime != null && account.swExitTime.Elapsed.TotalSeconds <= 20)
                return;
            int loginCount = Accounts.Where(acc => acc.Status.Contains("Đang chờ")).ToList().Count;
            if (Main.LoginGameCount >= loginCount)
                isOpen = false;
            if (Main.DicGame.Count >= Global.MaxLogin)
                isOpen = false;
            if (MicroLogin.swOpenGameTime == null)
                MicroLogin.swOpenGameTime = Stopwatch.StartNew();
            else
            {
                if (MicroLogin.swOpenGameTime.Elapsed.TotalSeconds < MicroLogin.OpenGameTime)
                    isOpen = false;
            }
            if (isOpen)
            {
                if ((!TDT.IsFilePathValid(account.Path) || !File.Exists(account.Path)) && !MicroLogin.IsOpenFile)
                {
                    MicroLogin.IsOpenFile = true;
                    OpenFileDialog openFile = new OpenFileDialog();
                    openFile.Filter = "Game.exe |Game.exe";
                    if (openFile.ShowDialog(this) == DialogResult.OK)
                    {
                        try
                        {
                            Publisher.XML.SelectSingleNode("//Publishers/Publisher[@Name=\"" + account.NPH + "\"]").Attributes["Path"].Value = openFile.FileName;
                            Publisher.SaveXML();
                        }
                        catch
                        {
                            XmlAttribute atb = Publisher.XML.CreateAttribute("Path");
                            atb.Value = openFile.FileName;
                            Publisher.XML.SelectSingleNode("//Publishers/Publisher[@Name=\"" + account.NPH + "\"]").Attributes.Append(atb);
                            Publisher.SaveXML();
                        }
                    }
                    MicroLogin.IsOpenFile = false;
                }
                MicroLogin.swOpenGameTime = Stopwatch.StartNew();
                account.OpenGameTime = Stopwatch.StartNew();
                account.Entered = false;
                account.IsSelectRole = false;
                int soluong = 0;
                if (Global.MaxLogin - Main.DicGame.Count <= 0)
                {
                    soluong = 0;
                }
                else
                {
                    soluong = CountDangCho;
                    if (soluong > Global.MaxLogin - Main.DicGame.Count)
                        soluong = Global.MaxLogin - Main.DicGame.Count;
                }
                for (int i = 0; i < soluong && i < MicroLogin.MaxOpenGame; i++)
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.FileName = account.Path;
                    startInfo.Arguments = "-fl";
                    if (account.NPH.Contains("Tình Kiếm"))
                        startInfo.Arguments += " " + TDT.RandomString(15);
                    startInfo.WorkingDirectory = Path.GetDirectoryName(account.Path);
                    Process.Start(startInfo);

                }
            }
        }


        public int CountDangCho
        {
            get
            {
                return Accounts.Where(account => account.Status.Contains("Đang")).ToList().Count;
            }
        }

        public static bool IsDangChoEx = false;



        Thread threadLogin;
        bool isstart = false;
        public static TabLogin Instance;
        public void StartThread()
        {
            if (!isstart)
                isstart = true;
            else
                return;

            threadLogin = new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                while (true)
                {
                    try
                    {
                        if (IsDisposed)
                            break;                        
                        timeLoginEx();
                    }
                    catch { }
                    Thread.Sleep(1200);
                }

            });
            threadLogin.Start();
        }
        public int ToMinute(string s)
        {
            int minute = 0;
            int hour = TDT.ParseInt(s);
            minute = hour * 60;
            int m = TDT.ParseInt(Regex.Replace(s, ".*:", ""));
            minute += m;
            return minute;
        }

        private void timeLoginEx()
        {

            if (IsStop && nudStop.Value > 0)
                return;
            if (MicroLogin.IsSleepTime)
            {
                string note = Game.IniParser.Read("Config", "SleepTime");
                foreach (string s in note.Split('\n'))
                {
                    string n = s.Trim();
                    if (n.Length > 2)
                    {
                        string time = n;
                        if (time.Split('-').Length > 1)
                        {
                            int fromMinute = ToMinute(time.Split('-')[0]);
                            int endMinute = ToMinute(time.Split('-')[1]);
                            int minuteNow = DateTime.Now.Hour * 60 + DateTime.Now.Minute;
                            if (minuteNow >= fromMinute && minuteNow < endMinute)
                            {
                                try
                                {
                                    Process.GetProcessesByName("Game").ToList().ForEach(p => p.Kill());
                                }
                                catch { }
                                return;
                            }
                        }
                    }
                }
            }

            if (MicroLogin.IsRunTrungAc)
            {
                bool isRun = false;
                if (Main.ServerTime.Now.Hour == 23 && Main.ServerTime.Now.Minute == 59 && Main.ServerTime.Now.Second >= 55)
                {
                    isRun = true;
                }
                if (Main.ServerTime.Now.Hour == 0 && Main.ServerTime.Now.Minute == 0 && Main.ServerTime.Now.Second <= 5)
                {
                    isRun = true;
                }
                if (isRun)
                {
                    Accounts.ToList().ForEach(a => { a.Status = "Đang chờ làm Trừng Ác..."; a.IsLoginTrungAc = true; });
                }
            }
            else if (MicroLogin.IsResetBHD && Main.ServerTime != null)
            {
                bool isRun = false;
                if (Main.ServerTime.Now.Hour == 23 && Main.ServerTime.Now.Minute == 59 && Main.ServerTime.Now.Second >= 55)
                {
                    isRun = true;
                }
                if (Main.ServerTime.Now.Hour == 0 && Main.ServerTime.Now.Minute == 0 && Main.ServerTime.Now.Second <= 5)
                {
                    isRun = true;
                }
                if (isRun)
                {
                    foreach (AccountEx account in Accounts)
                    {
                        account.IsBachHoaDuyen = true;
                        if (account.Status == "xong BHD")
                        {
                            account.Status = "Đang chờ làm BHD...";
                            if (account.game == null)
                            {

                            }
                            else
                            {
                                account.game.IsXongBHD = false;
                                account.game.PushMissions(MissionsType.BachHoaDuyen);
                                account.game.DaNhanHoaChung = false;
                                account.game.DaNhanHoaHong = false;
                            }
                        }
                    }
                }
            }

            foreach (KeyValuePair<int, Game> kvp in Main.DicGame.Where(kvp => !kvp.Value.listCheckedOnline.Contains(Handle)).ToList())
            {
                if (!kvp.Value.TLBB.Online)
                    continue;
                kvp.Value.listCheckedOnline.Add(Handle);
                foreach (AccountEx account in Accounts)
                {
                    if (account.Status.Contains("xong") || account.Status.Contains("đơ"))
                        continue;
                    if (string.IsNullOrEmpty(kvp.Value.TLBB.Id) || string.IsNullOrEmpty(account.Ids))
                        continue;
                    if (account.Ids.Contains(kvp.Value.TLBB.Id) && !string.IsNullOrEmpty(kvp.Value.TLBB.Id))
                    {
                        account.game = kvp.Value;
                        account.game.AccountEx = account;
                        account.IsSave = true;
                        break;
                    }
                }
            }


            int cnt = 0;

            int limit = TDT.ParseAllInt(txtLimit.Text);

            foreach (AccountEx account in Accounts)
            {
                if (account.game != null)
                    continue;
                if (limit > 0)
                {
                    if (Accounts.Where(acc => acc.game != null).Count() >= limit)
                        break;
                }
                if (account.Status.Contains("Đang chờ"))
                {
                    if (MicroLogin.IsLimitBonPhan)
                    {
                        if (account.IsBonHoa)
                        {
                            if (Accounts.Where(acc => acc.game != null && acc.IsBonHoa && acc.ToaDoTrongHoa == account.ToaDoTrongHoa).Count() >= MicroLogin.NumLimitBonPhan)
                            {
                                continue;
                            }
                        }
                    }
                    if (MicroLogin.IsLimitTrongHoa)
                    {
                        if (account.IsTrongHoa || account.IsNhanTrongBon)
                        {
                            if (Accounts.Where(acc => acc.game != null && (acc.IsTrongHoa || acc.IsNhanTrongBon) && acc.ToaDoTrongHoa == account.ToaDoTrongHoa).Count() >= MicroLogin.NumLimitTrongHoa)
                            {
                                continue;
                            }
                        }
                    }
                    if (MicroLogin.IsLimitBienThan)
                    {
                        if (account.IsNemBienThan)
                        {
                            if (Accounts.Where(acc => acc.game != null && acc.IsNemBienThan && acc.ToaDoTrongHoa == account.ToaDoTrongHoa).Count() >= MicroLogin.NumLimitBienThan)
                            {
                                continue;
                            }
                        }
                    }
                    if (cnt++ >= 100)
                        break;
                    Login(account);
                    if (account.Status == "Login")
                    {
                        if (cnt++ > 6)
                            break;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            foreach (AccountEx account in Accounts.Where(acc => acc.game != null))
            {
                var game = account.game;
                if (account.Item == null)
                    continue;
                if (account.Status.Contains("xong") || account.Status.Contains("đơ"))
                {
                    account.IsOnline = false;
                    continue;
                }
                if (game == null)
                {
                    continue;
                }

                game.TrongHoaIdx = account.ToaDoTrongHoa;
                if (game.TLBB.Online && game.IsSettingLoaded)
                {
                    if (account.IsLoginDua)
                    {
                        game.IsDuaHau = true;
                        game.IsByLogin = true;
                    }


                    if(account.AcBa != null)
                    {
                        game.PushMissions(MissionsType.DatDoiAcBa);
                        if (TLBB.GetMenpaiId(account.AcBa) > 0)
                            game.AcBa = TLBB.GetMenpaiId(account.AcBa);
                    }

                    if (account.IsLoginTrungAc)
                    {
                        game.PushMissions(MissionsType.TrungAc);
                        game.IsByLogin = true;
                    }

                    if (account.IsLoginTiemNangTan)
                    {
                        game.CheckTueHong();
                    }

                    if (account.IsLoginNhiemVuThangCap)
                    {
                        game.PushMissions(MissionsType.NhiemVuThangCap);
                    }

                    if (account.IsLoginNhiemVuExp)
                    {
                        game.PushMissions(MissionsType.NhiemVuExp);
                        game.IsByLogin = true;
                    }

                    if (account.IsLoginLeBao)
                    {
                        game.PushMissions(MissionsType.NhanLeBao);
                        game.IsByLogin = true;
                    }

                  

                    if (account.IsLoginBoiThuong)
                    {
                        game.PushMissions(MissionsType.NhanBoiThuong);
                        game.IsByLogin = true;
                    }

                    if (account.IsLoginAcTac)
                    {
                        if (game.MapAcTac == 0)
                            game.RandomMapAcTac();
                        game.PushMissions(MissionsType.DatDoiAcTac);
                        game.IsByLogin = true;
                    }

                    if (account.IsLoginThieuThatSon)
                    {
                        game.PushMissions(MissionsType.DatDoiThieuThatSon);
                        game.IsByLogin = true;
                    }

                    if (account.IsLoginLauLanTamBao)
                    {
                        game.PushMissions(MissionsType.DatDoiLauLanTamBao);
                        game.PushMissions(MissionsType.DatDoiKyCuoc);
                        game.IsByLogin = true;
                        account.IsLoginLauLanTamBao = false;
                    }

                    if (account.IsBachHoaDuyen)
                    {
                        game.PushMissions(MissionsType.BachHoaDuyen);
                        game.IsByLogin = true;
                    }

                    if (account.IsGomDo)
                    {
                        game.PushMissions(MissionsType.GomDo);
                        game.IsByLogin = true;
                    }


                    if (account.IsGomKNB)
                    {
                        game.PushMissions(MissionsType.GomDo);
                        game.PushMissions(MissionsType.GomKNB);
                        game.IsByLogin = true;
                    }

                    if (account.IsGomDoKNB)
                    {
                        game.PushMissions(MissionsType.GomDoKNB);
                        game.IsByLogin = true;
                    }

                    if (account.IsLoginMuaNgua)
                    {
                        game.PushMissions(MissionsType.DiMuaNga);
                    }

                    if (account.IsDoiHoa)
                    {
                        game.PushMissions(MissionsType.Doi999HoaHong);
                    }

                    if (account.IsNhanBienThan)
                    {
                        game.PushMissions(MissionsType.NhanBienThan);
                    }

                    if (account.IsNhanBong)
                    {
                        game.PushMissions(MissionsType.NhanBong);
                        game.IsByLogin = true;
                    }

                    if (account.IsBonHoa)
                    {
                        game.PushMissions(MissionsType.BonHoa);
                        game.IsByLogin = true;
                    }
                    if (account.IsNhanQuaBuiHoaHong)
                    {
                        game.PushMissions(MissionsType.NhanQuaBuiHoaHong);
                        game.IsByLogin = true;
                    }

                    if (account.IsKetNghiaNhanBong)
                    {
                        game.IsByLogin = game.IsKetNghia = game.IsByLogin = true;
                        game.PushMissions(MissionsType.NhanBong);
                    }
                    if (account.IsSuDoNhanBong)
                    {
                        game.IsByLogin = game.IsSuDoo = game.IsNhanBongSuDo = true;
                        game.PushMissions(MissionsType.NhanBong);
                    }

                    if (account.IsThoiBong)
                    {
                        game.IsThoiBong = true;
                    }

                    if (account.IsNhanTrongBon)
                    {
                        game.IsByLogin = game.IsNhanMam = true;
                        game.PushMissions(MissionsType.BachHoaDuyen);
                        game.PushMissions(MissionsType.BonHoa);
                        game.PushMissions(MissionsType.TrongHoa);
                    }

                    if (account.IsTrongHoa)
                    {
                        game.IsByLogin = true;
                        game.PushMissions(MissionsType.BonHoa);
                        game.PushMissions(MissionsType.TrongHoa);
                    }

                    if (account.IsNemBienThan)
                    {
                        game.IsByLogin = true;
                        game.PushMissions(MissionsType.NhanBienThan);
                    }

                    if (account.IsNhanHoaHong)
                    {
                        game.PushMissions(MissionsType.BachHoaDuyen);
                        game.IsNhanHoaHong = true;
                    }

                    if (account.IsNhanMam)
                    {
                        game.PushMissions(MissionsType.BachHoaDuyen);
                        game.IsNhanMam = true;
                    }

                }
                if (account.IsOnline)
                {
                    if (account.game.TLBB.IsSelectServer || account.game.TLBB.IsLogon)
                    {
                        account.game.IsSelectLogin = false;
                        if (CalendarEx.IsAutoLogin || !account.Loged)
                        {                           
                            account.Status = "Đang chờ...";
                        }
                        else
                        {
                            account.Status = "Exit...";
                        }
                        account.game = null;
                        account.IsOnline = false;
                        continue;
                    }
                }
                if (account.game.Process.HasExited)
                {
                    if (CalendarEx.IsAutoLogin || !account.Loged)
                    {
                        account.Status = "Đang chờ...";
                        account.swExitTime = Stopwatch.StartNew();
                        account.Created = false;
                    }
                    else
                    {
                        account.Status = "Exit...";
                    }
                    account.game = null;
                    account.IsOnline = false;
                    continue;
                }
                if (!MicroLogin.IsNotOut)
                {
                    if (account.game.IsXongBHD)
                    {
                        account.Status = "xong BHD";
                        account.game = null;
                        continue;
                    }
                }
                if (account.IsCaptcha)
                {
                    account.Status = "Captcha";
                }
                if (account.Online)
                {
                    account.swExitTime = Stopwatch.StartNew();
                    if (account.Status != "Online")
                        account.Status = "Online";
                    account.Created = false;
                    if (account.Lvl != game.TLBB.Lvl.ToString())
                        account.Lvl = game.TLBB.Lvl.ToString();
                    account.Loged = true;
                    if (MicroLogin.IsExit30 && account.IsSaveName)
                    {
                        if (account.game.TLBB.Lvl >= MicroLogin.LvExit)
                        {
                            account.game.IsXongBHD = true;
                            continue;
                        }
                    }

                    if (!account.IsSave)
                    {
                        account.IsSave = true;
                        if (!account.Ids.Contains(game.TLBB.Id))
                        {
                            account.Ids = game.TLBB.Id;
                        }
                    }
                    account.Entered = false;
                    account.IsSelectRole = false;



                    if (!account.IsSaveName && game.IsSettingLoaded)
                    {
                        if (game.TLBB.Lvl > 10 && game.TLBB.MenpaiName == "Không Có")
                        {

                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(account.game.TLBB.Name))
                            {
                                account.IsSaveName = true;
                            }
                            account.Name = account.game.TLBB.Name;
                            account.Lvl = account.game.TLBB.Lvl.ToString();
                            account.Menpai = account.game.TLBB.Menpai.ToString();
                        }
                    }
                    account.IsOnline = true;

                    continue;
                }
                else
                {
                    account.IsSaveName = false;
                    if (!account.game.TLBB.ON_SCENE_TRANSING)
                    {
                        account.Status = "Login";
                    }
                }
                if (!game.IsInit)
                    continue;
                if (!game.TLBB.IsSelectRole)
                    account.SelectRoleTime = Stopwatch.StartNew();
                if (!game.TLBB.IsLogon)
                    account.LogonTime = Stopwatch.StartNew();

                if (game.TLBB.IsNexLogin)
                {
                    game.LUA.HuoDongRiChengNextClick();
                }
                else if (game.TLBB.IsSelectServer)
                {
                    if (account.swExitTime == null || account.swExitTime.Elapsed.TotalSeconds > MicroLogin.SecLog)
                    {
                        account.IsEnterRole = false;
                        account.Entered = account.IsSelectRole = false;
                        if (account.ServerIndex != -1)
                        {
                            if (game.Address.GameType == 1)
                            {
                                if (account.Server == "Thiên Long 15")
                                {
                                    game.DoStringEx("setmetatable(_G, {__index = LoginSelectServer_Env}); SelectServerEvent_Area_SwitchPage(2);" + "setmetatable(_G, {__index = LoginSelectServer_Env}); SelectServerEvent_ServerBn_Clicked(" + 2 + ",0); SelectServerEvent_SelectOk();");
                                }
                                else
                                {
                                    if (account.ServerIndex > 0)
                                    {

                                        game.DoStringEx("setmetatable(_G, {__index = LoginSelectServer_Env}); SelectServerEvent_Area_SwitchPage(6);" + "setmetatable(_G, {__index = LoginSelectServer_Env}); SelectServerEvent_ServerBn_Clicked(" + (account.ServerIndex - 1) + ",0); SelectServerEvent_SelectOk();");
                                    }
                                    //game.LuaDoOneLineString("setmetatable(_G, {__index = LoginSelectServer_Env}); SelectServerEvent_ServerBn_Clicked(" + account.ServerIndex + ",0); SelectServerEvent_SelectOk();");
                                    //game.LuaDoOneLineString("setmetatable(_G, {__index = LoginSelectServer_Env}); SelectServer_Area_ClearAllStatus(); SelectServerEvent_AreaButtonUpdate(11, 0); SelectServerEvent_AreaUpdate(11); SelectServerEvent_ServerBn_Clicked(" + account.ServerIndex + ",0); SelectServerEvent_SelectOk();");
                                }
                            }
                            else
                            {
                                game.DoStringEx("setmetatable(_G, {__index = LoginSelectServer_Env}); local index = GameProduceLogin:GetServerAreaCount() - 1; setmetatable(_G, {__index = LoginSelectServer_Env}); SelectServer_SelectAreaServer(index); setmetatable(_G, {__index = LoginSelectServer_Env}); SelectServer_ConfirmSelectLine(" + account.ServerIndex + ");");
                            }
                        }
                    }
                }
                else
                {
                    if (game.TLBB.IsLogon)
                    {
                        //if (game.TLBB.IsTextCaptcha)
                        //{
                        //    account.Status = "Captcha";
                        //    account.game = null;
                        //    game.Exit();
                        //    continue;
                        //}
                        account.CreateTime = Stopwatch.StartNew();
                        if (!account.Entered)
                        {
                            if (account.TailIndex != -1)
                            {
                                game.LUA.LogOnSelectTail(account.TailIndex);
                            }
                            foreach (char c in account.User)
                            {
                                Win.PostMessage(game.Handle, 0x102, c, 0);
                            }
                            Win.PostMessage(game.Handle, 0x100, (int)Keys.Tab, 0);
                            Win.PostMessage(game.Handle, 0x101, (int)Keys.Tab, 0);
                            foreach (char c in account.Pass)
                            {
                                Win.PostMessage(game.Handle, 0x102, c, 0);
                            }
                            Win.PostMessage(game.Handle, 0x100, (int)Keys.Enter, 0);
                            Win.PostMessage(game.Handle, 0x101, (int)Keys.Enter, 0);
                            account.Entered = true;
                            account.IsSelectRole = false;
                            account.IsEnterRole = false;
                        }
                        else
                        {

                            if (!Main.IsKhongChonMayChu)
                            {
                                if (account.LogonTime.Elapsed.TotalSeconds < 22)
                                {
                                    account.game.PushDebugMessage("Auto tự chọn máy chủ sau " + (int)(22 - account.LogonTime.Elapsed.TotalSeconds) + " giây");
                                }
                                else
                                {
                                    account.IsSelectRole = true;
                                    account.Entered = false;
                                    account.LoginMessageTime = 0;
                                    game.LUA.LogOn_ExitToSelectServer();
                                }
                            }
                            // enter
                            if (!game.TLBB.IsSelectServerQuest)
                            {
                                account.LoginMessageTime++;
                                if (account.LoginMessageTime > 10)
                                {
                                    account.LoginMessageTime = 0;
                                    Win.PostMessage(game.Handle, 0x100, (int)Keys.Enter, 0);
                                    Win.PostMessage(game.Handle, 0x101, (int)Keys.Enter, 0);
                                }
                            }
                        }
                    }
                    else
                    {
                        // select character
                        if (game.TLBB.IsSelectRole)
                        {
                            account.Entered = false;




                            account.CreateTime = Stopwatch.StartNew();
                            if (!Main.IsKhongChonMayChu)
                            {
                                if (account.SelectRoleTime.Elapsed.TotalSeconds > 30)
                                {
                                    account.IsSelectRole = false;
                                    account.Entered = false;
                                    account.IsEnterRole = false;
                                    account.LoginMessageTime = 0;
                                    game.LUA.LoginOverTime();
                                }
                                else
                                {
                                    account.game.PushDebugMessage("Auto tự chọn máy chủ sau " + (int)(30 - account.SelectRoleTime.Elapsed.TotalSeconds) + " giây");
                                }
                            }


                            if (!account.IsSelectRole)
                            {
                                if (!account.IsEnterRole)
                                {
                                    if (account.LoginIndex == "")
                                    {
                                        game.DoStringEx("setmetatable(_G, {__index = LoginSelectRole_Env}); if this:IsVisible() then SelectRole_EnterGame(); end");
                                    }


                                    if (account.LoginIndex != "")
                                    {
                                        if (account.LoginIndex == "1")
                                        {
                                            game.DoStringEx("setmetatable(_G, {__index = LoginSelectRole_Env}); if this:IsVisible() then strName ,iMenPai ,iLevel ,iDelTime ,strFaceImgName ,iCYGSate = GameProduceLogin:GetRoleInfo(0); if strName == nil or strName == '' then  SelectRole_DoubleClicked(0); else SelectRole_Clicked(0); GameProduceLogin:MoveToCharacter(0);  SelectRole_EnterGame();  end end");
                                        }
                                        else if (account.LoginIndex == "2")
                                        {
                                            game.DoStringEx("setmetatable(_G, {__index = LoginSelectRole_Env}); if this:IsVisible() then strName ,iMenPai ,iLevel ,iDelTime ,strFaceImgName ,iCYGSate = GameProduceLogin:GetRoleInfo(1); if strName == nil or strName == '' then  SelectRole_DoubleClicked(1); else SelectRole_Clicked(1); GameProduceLogin:MoveToCharacter(0);  SelectRole_EnterGame();  end end");
                                        }
                                        else if (account.LoginIndex == "3")
                                        {
                                            game.DoStringEx("setmetatable(_G, {__index = LoginSelectRole_Env}); if this:IsVisible() then strName ,iMenPai ,iLevel ,iDelTime ,strFaceImgName ,iCYGSate = GameProduceLogin:GetRoleInfo(2); if strName == nil or strName == '' then  SelectRole_DoubleClicked(2); else SelectRole_Clicked(2); GameProduceLogin:MoveToCharacter(0);  SelectRole_EnterGame();  end end");
                                        }
                                    }

                                    account.IsEnterRole = true;
                                    account.IsSelectRole = false;

                                }
                                else
                                {
                                    account.IsSelectRole = false;
                                }
                            }
                        }
                        else
                        {

                            if (game.TLBB.IsSelectRoleCreate)
                            {


                                if (Global.IsAutoCreate && Setting.Is("chkAutoCreatePlayer"))
                                {
                                    if (account.CreateTime.Elapsed.TotalSeconds > 15 && !account.Created)
                                    {
                                        account.Created = true;
                                        string lua = "setmetatable(_G, {__index = LoginCreateRole_Env}); if this:IsVisible() then " + "GameProduceLogin:SetFaceId(0, 1); GameProduceLogin:CreateRole('" + TDT.NiceName(TDT.Random.Next(3, 5)) + TDT.NiceName(TDT.Random.Next(3, 5)) + "',0)" + " end";
                                        game.DoStringEx(lua);
                                        account.CreateTime = Stopwatch.StartNew();
                                    }
                                }

                                if (account.CreateTime.Elapsed.TotalSeconds > 100)
                                {
                                    account.Created = false;
                                    account.IsSelectRole = false;
                                    account.Entered = false;
                                    account.LoginMessageTime = 0;
                                    game.LUA.LoginOverTime();
                                }
                                else
                                {
                                    account.game.PushDebugMessage("Auto tự chọn máy chủ sau " + (int)(100 - account.CreateTime.Elapsed.TotalSeconds) + " giây");
                                }
                            }
                        }
                    }
                }

            }
        }

        public Dictionary<string, string> Answers = new Dictionary<string, string>();
        public int LoginCount
        {
            get;
            set;
        }



        public List<AccountEx> Accounts { get; set; } = new List<AccountEx>();

        public Dictionary<string, string> Captchas = new Dictionary<string, string>();

        bool IsPop = true;



        void Push(string data)
        {
            if (IsDisposed)
                return;
            if (!IsPop)
                return;
            Poster posterPush = new Poster();
            posterPush.Url = URL.HomePage + "microauto/captcha.php";
            posterPush.AutoReconnect = true;
            if (!data.Contains("="))
                posterPush.Data = "cmd=push" + "&image=" + data + "&email=" + HttpUtility.UrlEncode(User.Email) + "&pass=" + HttpUtility.UrlEncode(User.Pass);
            posterPush.Control = this;
            posterPush.Completed += new EventHandler(posterPush_Completed);
            posterPush.Post();
        }

        void posterPush_Completed(object sender, EventArgs e)
        {
            var poster = sender as Poster;
            if (poster.IsError)
            {
                Push(poster.Data);
            }
            else
            {
                if (poster.Response == "dis")
                {
                    IsPop = false;
                }
            }
        }

        public static bool IsSave = true;
        private void button1_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void Save()
        {

            Enabled = false;

            Task.Run(() =>
            {
                XmlNode root = XML.SelectSingleNode("/*");
                root.RemoveAll();
                foreach (AccountEx account in Accounts)
                {
                    root.AppendChild(account.Node);
                }


                foreach (XmlElement node in XML.SelectSingleNode("Accounts").SelectNodes("Account"))
                {
                    if (node.InnerXml.Trim() == string.Empty)
                    {
                        node.IsEmpty = true;
                    }
                }
                string msg = string.Empty;
                try
                {
                    xNet.HttpRequest request = new xNet.HttpRequest();
                    request.AddParam("cmd", "rgsave");
                    request.AddParam("email", Email);
                    request.AddParam("pass", TDT.Hasher.MD5(Pass));
                    request.AddParam("xml", XML.InnerXml);
                    request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/88.0.4324.190 Safari/537.36";
                    var r = request.Post("https://www.tieudattai.org/microauto/user.php");
                    msg = r.ToString();
                    //Main.PushLog(r.ToString());

                }
                catch (Exception ex)
                {
                    //Main.PushLog(ex.Message);
                    msg = ex.Message;
                }

                Enabled = true;
                this.Invoke(() =>
                {
                    new Loader("Lưu Danh Sách Online Thành Công").Show();
                });
                


            });




            //string data = "cmd=rgsave&xml=" + HttpUtility.UrlEncode(XML.InnerXml)
            //    + "&email=" + HttpUtility.UrlEncode(Email)
            //    + "&pass=" + HttpUtility.UrlEncode(TDT.Hasher.MD5(Pass))
            //    ;
            //Poster posterLogin = new Poster();
            //posterLogin.Url = "http://www.tieudattai.org/remlaw/user.php";
            //posterLogin.Data = data;
            //posterLogin.Control = this;
            //posterLogin.Completed += PosterLogin_Completed;
            //posterLogin.Post();
        }

        private void PosterLogin_Completed(object sender, EventArgs e)
        {
            Poster poster = (Poster)sender;
            if (poster.IsError)
            {
                Save();
                return;
            }
            Enabled = true;
            MessageBox.Show(this, poster.Response, "MicroAuto", MessageBoxButtons.OK);
            IsSave = true;
        }

        private void TabLogin_Load(object sender, EventArgs e)
        {
        
            lvLogin.Columns[2].DisplayIndex = 3;
            lvLogin.Columns[3].DisplayIndex = 2;
            txtLimit.Text = MicroLogin.Limit.ToString();
            if (MicroLogin.Limit.ToString() == "0")
                txtLimit.Text = "";
            string show = Game.IniParser.Read("Config", "Column");
            foreach(var i in show.Split(','))
            {
                if (i == "1")
                    userToolStripMenuItem.Checked = true;
                if (i == "3")
                    serverToolStripMenuItem.Checked = true;
                if (i == "7")
                    mầmToolStripMenuItem.Checked = true;
                if (i == "8")
                    shitToolStripMenuItem.Checked = true;
                if (i == "9")
                    bútToolStripMenuItem.Checked = true;
                if (i == "10")
                    infoToolStripMenuItem.Checked = true;
                listHideColumn.Remove(i.ToInt());
            }
            lvLogin.Columns.Cast<ColumnHeader>().ForEach(i => { if (listHideColumn.Contains(i.Index)) { i.Width = 0; } });
            HideShowCol();
          
        }

        public void LoginUser_Disposed(object sender, EventArgs e)
        {
            MicroLogin.Instance.splitContainer1.Visible = true;
            try
            {
                Parent.Tag = "loged";
                LoginUser loginUser = null;
                if (sender != null)
                {
                    loginUser = sender as LoginUser;
                    XML = loginUser.XML;
                    Email = loginUser.Email;
                }
                if (Email == "Offline")
                {
                    Instance = this;
                    Parent.Text = "Offline";
                    //label4.Visible = pictureBox1.Visible = false;
                    Accounts = MicroLogin.Accounts;
                }
                else
                {
                    Pass = loginUser.Pass;
                    AccountEx = new AccountEx(XML, Pass);
                    Accounts = AccountEx.Enum();
                }

                lvLogin.Items.Clear();
                StartThread();

                UpdateListS();

                txtSearch.WaterMark = "Search... [" + Email.Trim() + "]";
                List.Add(this);
                panel1.Visible = true;
            }
            catch { }
        }

        public static bool InogeFirst = true;

        string Email;
        string Pass;



        public void AddAcc()
        {
            if (MicroLogin.Instance.CboNPH.Text == "")
            {
                MessageBox.Show(this, "Vui lòng chọn nhà phát hành", "MicroAuto", MessageBoxButtons.OK);
                return;
            }
            if (MicroLogin.Instance.CboServer.Text == "")
            {
                MessageBox.Show(this, "Vui lòng chọn server", "MicroAuto", MessageBoxButtons.OK);
                return;
            }
            if (MicroLogin.Instance.TxtUser.Text == "")
            {
                MessageBox.Show(this, "Vui lòng điền user đăng nhập", "MicroAuto", MessageBoxButtons.OK);
                return;
            }
            if (MicroLogin.Instance.TxtPass.Text == "")
            {
                MessageBox.Show(this, "Vui lòng điền mật khẩu đăng nhập", "MicroAuto", MessageBoxButtons.OK);
                return;
            }
            string path = "";
            try
            {
                path = Publisher.XML.SelectSingleNode("//Publishers/Publisher[@Name=\"" + MicroLogin.Instance.CboNPH.Text + "\"]").Attributes["Path"].Value;
            }
            catch { }
            if (path == "")
            {
                MessageBox.Show(this, "Bạn cần phải chọn đường dẫn gameạn cần phải chọn đường dẫn game\r\nFile Game.exe nằm trong thư mục Bin của game", "MicroAuto", MessageBoxButtons.OK);
                OpenFileDialog openFile = new OpenFileDialog();
                openFile.Filter = "Game.exe |Game.exe";
                if (openFile.ShowDialog(this) == DialogResult.OK)
                {
                    try
                    {
                        Publisher.XML.SelectSingleNode("//Publishers/Publisher[@Name=\"" + MicroLogin.Instance.CboNPH.Text + "\"]").Attributes["Path"].Value = openFile.FileName;
                        Publisher.SaveXML();
                    }
                    catch
                    {
                        XmlAttribute at = Publisher.XML.CreateAttribute("Path");
                        at.Value = openFile.FileName;
                        Publisher.XML.SelectSingleNode("//Publishers/Publisher[@Name=\"" + MicroLogin.Instance.CboNPH.Text + "\"]").Attributes.Append(at);
                        Publisher.SaveXML();
                    }
                }
                else
                {
                    return;
                }
            }
            AccountEx ACC = new AccountEx(MicroLogin.Instance.TxtUser.Text, MicroLogin.Instance.TxtPass.Text, MicroLogin.Instance.CboNPH.Text, MicroLogin.Instance.CboServer.Text, MicroLogin.Instance.CboTail.Text, "", "", XML, Pass);


            ACC.Item = new ListViewItem((Accounts.Count + 1).ToString());
            ACC.Item.SubItems.Add(ACC.User);
            ACC.Item.SubItems.Add(ACC.Name);
            ACC.Item.SubItems.Add(ACC.Server);
            ACC.Item.SubItems.Add(ACC.LoginIndex);
            ACC.Item.SubItems.Add(ACC.Status);
            ACC.Item.SubItems.Add(ACC.Lvl);
            ACC.Item.SubItems.Add(ACC.CountMam);
            ACC.Item.SubItems.Add(ACC.CountShit);
            ACC.Item.SubItems.Add(ACC.CountBut);
            ACC.Item.SubItems.Add(ACC.Note);
            ACC.Item.Tag = ACC;


            Accounts.Add(ACC);
            UpdateListS();
            if (Email == "Offline")
            {
                MicroLogin.Instance.OnAccChange();
            }
        }

        public XmlDocument XML = new XmlDocument();

        private void listViewLogin_DoubleClick(object sender, EventArgs e)
        {
            if (FirstSelectedItem == null)
                return;
            AccountEx account = (AccountEx)FirstSelectedItem.Tag;
            if (account.Status == "xong BHD" || account.Status == "xong BTD")
            {
                try
                {
                    account.game.Active();
                }
                catch { }
                return;
            }
            if (account.game == null || account.Status == "Exit...")
            {
                if (account.Status == "" || account.Status == "Exit..." || account.Status == "...")
                {
                    account.Status = "Đang chờ...";
                    account.IsForceOpen = true;
                }
            }
            else
            {
                account.game.Active();
            }
        }



        private void listViewLogin_SelectedIndexChanged(object sender, EventArgs e)
        {
            timer2.Stop();
            timer2.Start();
        }

        private void listViewLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                delteAccounts(null, null);
            if (e.KeyCode == Keys.Enter)
            {
                foreach (ListViewItem item in SelectedItems)
                {
                    AccountEx acc = (AccountEx)item.Tag;
                    if (acc.game == null)
                    {
                        acc.Status = "Đang chờ...";
                    }
                }
            }
            if (e.KeyCode == Keys.A)
            {
                if (e.Control == true)
                {
                    lvLogin.SelectAllItems();
                }
            }
            if (e.Control == true)
            {
                if (e.KeyCode == Keys.Up)
                {
                    if (SelectedAccounts.Count() > 0)
                    {
                        int index = Accounts.FindIndex(a => a == SelectedAccounts.ToList()[0]);
                        List<AccountEx> list = SelectedAccounts.ToList();
                        if (Email == "Offline")
                        {
                            MicroLogin.Accounts = Accounts.Except(list).ToList();
                            list.Reverse();
                            foreach (AccountEx a in list)
                                MicroLogin.Accounts.Insert(index - 1, a);
                            Accounts = MicroLogin.Accounts;
                        }
                        else
                        {
                            Accounts = Accounts.Except(list).ToList();
                            list.Reverse();
                            foreach (AccountEx a in list)
                                Accounts.Insert(index - 1, a);
                        }
                        UpdateListS();
                        lvLogin.SelectedIndices.Clear();
                    }
                }
                if (e.KeyCode == Keys.Down)
                {
                    if (SelectedAccounts.Count() > 0)
                    {
                        int index = Accounts.FindIndex(a => a == SelectedAccounts.ToList()[0]);
                        List<AccountEx> list = SelectedAccounts.ToList();
                        if (Email == "Offline")
                        {
                            MicroLogin.Accounts = Accounts.Except(list).ToList();
                            list.Reverse();
                            foreach (AccountEx a in list)
                                MicroLogin.Accounts.Insert(index + 1, a);
                            Accounts = MicroLogin.Accounts;
                        }
                        else
                        {
                            Accounts = Accounts.Except(list).ToList();
                            list.Reverse();
                            foreach (AccountEx a in list)
                                Accounts.Insert(index + 1, a);
                        }
                        UpdateListS();
                        lvLogin.SelectedIndices.Clear();
                    }
                }
                if (e.KeyCode == Keys.X)
                {
                    cutToolStripMenuItem1_Click(null, null);
                }
                if (e.KeyCode == Keys.C)
                {
                    toolStripMenuItem25_Click(null, null);
                }
                if (e.KeyCode == Keys.V)
                {
                    toolStripMenuItem26_Click(null, null);
                }
            }
        }

   
        bool IsStop { get; set; }

        public int StopMinute = 30;
        public Stopwatch swStop = Stopwatch.StartNew();

        private void chkStop_CheckedChanged(object sender, EventArgs e)
        {
            StopMinute = (int)nudStop.Value;
            swStop = Stopwatch.StartNew();
            IsStop = chkStop.Checked;
        }




        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            UpdateListS();
        }


        private void nudStop_ValueChanged(object sender, EventArgs e)
        {
            StopMinute = (int)nudStop.Value;
            swStop = Stopwatch.StartNew();
        }

        List<int> hidecol = new List<int>();

        private void HideShowCol()
        {
            int cnt = 0;
            while (lvLogin.Columns.Cast<ColumnHeader>().Sum(i => i.Width) + 40 >= Width)
            {                
                foreach (ColumnHeader col in lvLogin.Columns.Cast<ColumnHeader>().Reverse().OrderByDescending(col => col.DisplayIndex))
                {
                    if (listHideColumn.Contains(col.Index))
                        continue;
                    if (col.Width > 0)
                    {
                        col.Tag = col.Width;
                        col.Width = 0;
                        hidecol.Add(col.Index);
                        break;
                    }
                }
                if (cnt++ > lvLogin.Columns.Count)
                    break;
            }

            cnt = 0;

            while (lvLogin.Columns.Cast<ColumnHeader>().Sum(i => i.Width) + 40 < Width)
            {
                foreach (ColumnHeader col in lvLogin.Columns.Cast<ColumnHeader>().OrderBy(col => col.DisplayIndex))
                {
                    if (listHideColumn.Contains(col.Index))
                        continue;
                    if (lvLogin.Columns.Cast<ColumnHeader>().Sum(i => i.Width) + 40 >= Width)
                        break;
                    if (col.Width == 0 && col.Tag != null)
                    {
                        if (lvLogin.Columns.Cast<ColumnHeader>().Sum(i => i.Width) + 40 + col.Tag.ToString().ToInt() < Width)
                        {
                            hidecol.Remove(col.Index);
                            col.Width = col.Tag.ToString().ToInt();
                            col.Tag = col.Width;
                            break;
                        }
                    }
                }
                if (cnt++ > lvLogin.Columns.Count)
                    break;
            }
        }

        private void listViewLogin_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            if (lvLogin.Columns[e.ColumnIndex].Width == 0)
            {
                if (hidecol.Contains(e.ColumnIndex))
                {
                    e.Cancel = true;
                    e.NewWidth = lvLogin.Columns[e.ColumnIndex].Width;
                }
            }
            if (e.ColumnIndex == 1 || listHideColumn.Contains(e.ColumnIndex))
            {
                e.Cancel = true;
                e.NewWidth = lvLogin.Columns[e.ColumnIndex].Width;
            }
        }

        private void lưuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Save();
        }

        Thread threadSearch;

        public List<AccountEx> listS = new List<AccountEx>();

        void UpdateListS()
        {
            try
            {
                if (threadSearch != null)
                {
                    if (threadSearch.IsAlive)
                    {
                        threadSearch.Abort();
                        threadSearch = null;
                    }
                    threadSearch = null;
                }
                if (threadSearch == null)
                {
                    threadSearch = new Thread(() =>
                    {
                        try
                        {
                            Thread.CurrentThread.IsBackground = true;
                            while (!IsHandleCreated)
                            {
                                Thread.Sleep(1000);
                            }


                            List<AccountEx> list = new List<AccountEx>();

                            if (txtSearch.Text.Length > 0)
                            {
                                string s = txtSearch.Text.VietLien();

                                list = Accounts.Where(a => a.FullText.Contains(s)).ToList();
                            }
                            else
                            {
                                list = Accounts;
                            }

                            Invoke(new Action(() =>
                            {
                                lvLogin.BeginUpdate();
                                listS = list;
                                lvLogin.VirtualListSize = listS.Count;
                                lvLogin.Columns[0].Text = "[" + listS.Count + "]";
                                lvLogin.EndUpdate();
                            }));
                        }
                        catch
                        {
                        }
                    });
                    threadSearch.Start();
                }
            }
            catch
            {
            }

        }



        private void lvLogin_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            if (listS.Count >= e.ItemIndex)
            {
                var account = listS[e.ItemIndex];
                if (account.Item == null)
                {
                    account.Item = new ListViewItem((e.ItemIndex + 1).ToString());
                    account.Item.SubItems.Add(account.User);
                    account.Item.SubItems.Add(account.Name);
                    account.Item.SubItems.Add(account.Server);
                    account.Item.SubItems.Add(account.LoginIndex);
                    account.Item.SubItems.Add(account.Status);
                    account.Item.SubItems.Add(account.Lvl);
                    account.Item.SubItems.Add(account.CountMam);
                    account.Item.SubItems.Add(account.CountShit);
                    account.Item.SubItems.Add(account.CountBut);
                    account.Item.SubItems.Add(account.Note);
                    account.Item.Tag = account;
                }
                e.Item = account.Item;
            }
        }

        public IEnumerable<ListViewItem> SelectedItems
        {
            get
            {
                foreach (int index in lvLogin.SelectedIndices)
                {
                    yield return lvLogin.Items[index];
                }
            }
        }

        public IEnumerable<AccountEx> SelectedAccounts
        {
            get
            {
                foreach (int index in lvLogin.SelectedIndices)
                {
                    yield return lvLogin.Items[index].Tag as AccountEx;
                }
            }
        }

        public ListViewItem FirstSelectedItem
        {
            get
            {
                foreach (int index in lvLogin.SelectedIndices)
                {
                    return lvLogin.Items[index];
                }
                return null;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lvLogin.Columns[0].Text = "[" + listS.Count + "]";
            lvLogin.Refresh();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (FirstSelectedItem == null)
                return;


            AccountEx account = (AccountEx)FirstSelectedItem.Tag;
            MicroLogin.Instance.TxtUser.Text = account.User;
            MicroLogin.Instance.TxtPass.Text = account.Pass;
            MicroLogin.Instance.CboNPH.SelectedIndex = MicroLogin.Instance.CboNPH.FindString(account.NPH);
            MicroLogin.Instance.CboServer.SelectedIndex = MicroLogin.Instance.CboServer.FindString(account.Server);
            MicroLogin.Instance.CboTail.SelectedIndex = MicroLogin.Instance.CboTail.FindString(account.Tail);

            timer2.Stop();
        }



        private void muListView_Opening(object sender, CancelEventArgs e)
        {
           
            try
            {
                muChuyenServer.DropDownItems.Clear();
                foreach (XmlNode node in Publisher.XML.SelectSingleNode("//Publishers/Publisher[@Name=\"" + MicroLogin.Instance.CboNPH.Text + "\"]/Servers").SelectNodes("Server"))
                {
                    ToolStripMenuItem item = new ToolStripMenuItem(node.InnerText);
                    muChuyenServer.DropDownItems.Add(item);
                    item.Click += (ss, ee) =>
                    {
                        foreach (AccountEx account in SelectedAccounts)
                        {
                            account.Server = item.Text;
                        }
                    };
                }
            }
            catch { }

            thổiBóngToolStripMenuItem.Enabled = Global.IsThoiBong;
            nhậnBóngToolStripMenuItem.Enabled = Global.IsNhanBong;
            if (User.IsEnglish)
            {
                chọnMáyChủToolStripMenuItem1.Text = "Select Server";
            }
            nhậnQuàBụiHoaHồngToolStripMenuItem.Visible = Global.IsHoaHong;
            toolStripMenuItem23.Text = "Refresh [" + lvLogin.SelectedIndices.Count + "]";
            kếtNghĩaNhậnBóngToolStripMenuItem.Visible = Global.IsBigBall;
            kếtNghĩaToolStripMenuItem.Visible = sưToolStripMenuItem.Visible = Global.IsBigBall;
            muNhanThoiBong.Visible = Global.IsAdminEx;
            tạoNhómToolStripMenuItem.Text = "Tạo Nhóm [" + SelectedAccounts.Where(acc => acc.game != null && acc.game.TLBB.Online).Count() + "]";
            try
            {
                var coordinates = lvLogin.PointToClient(Cursor.Position);
                if (coordinates.Y <= 22)
                {
                    foreach (ToolStripItem m in muListView.Items)
                    {
                        if (m.Tag == null)
                            m.Visible = false;
                        else if (m.Tag.ToString() == "cl")
                            m.Visible = true;
                        else
                            m.Visible = false;
                    }
                }
                else
                {
                    foreach (ToolStripItem m in muListView.Items)
                    {
                        if (m.Tag == null)
                            m.Visible = true;
                        else if (m.Tag.ToString() == "cl")
                            m.Visible = false;
                        else
                            m.Visible = true;
                    }
                }
            }
            catch { }
        }

        private void toolStripMenuItem13_Click(object sender, EventArgs e)
        {
            foreach (AccountEx account in SelectedAccounts)
            {
                if (account.game == null)
                {
                    account.Status = "Đang chờ làm Q Dưa...";
                    account.IsLoginDua = true;
                }
                else
                {
                    account.game.IsDuaHau = true;
                }
            }
        }

  
        private void toolStripMenuItem20_Click(object sender, EventArgs e)
        {
            SelectedAccounts.ToList().ForEach(a => { a.Status = "Đang chờ Mua Ngựa..."; a.IsLoginMuaNgua = true; });
        }

        private void toolStripMenuItem21_Click(object sender, EventArgs e)
        {
            foreach (AccountEx account in SelectedAccounts)
            {
                account.Status = "Đang chờ làm BHD...";
                account.IsGomDo = true;
            }
        }

        private void treoShopChoTaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (AccountEx account in SelectedAccounts)
            {
                account.Status = "Đang chờ làm BHD...";
                account.IsGomDoKNB = true;
            }
        }

        private void nhậnQuàBụiHoaHồngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (AccountEx account in SelectedAccounts)
            {
                account.Status = "Đang chờ làm BHD...";
                account.IsNhanQuaBuiHoaHong = true;
            }
        }

        private void nhậnBóngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedAccounts.ToList().ForEach(a => { a.Status = "Đang chờ Nhận Bóng..."; a.IsNhanBong = true; });
        }

        private void thổiBóngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedAccounts.ToList().ForEach(a => { a.Status = "Đang chờ Thổi Bóng..."; a.IsThoiBong = true; });
        }

        private void kếtNghĩaNhậnBóngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedAccounts.ToList().ForEach(a => { a.Status = "Đang chờ Kết Nghĩa Nhận Bóng..."; a.IsKetNghiaNhanBong = true; });
        }

        private void sưToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedAccounts.ToList().ForEach(a => { a.Status = "Đang chờ Sư Đồ Nhận Bóng..."; a.IsSuDoNhanBong = true; });
        }



        private void muNhanThoiBong_Click(object sender, EventArgs e)
        {
            SelectedAccounts.ToList().ForEach(a => { a.Status = "Đang chờ Nhận Thổi Bóng..."; a.IsNhanBong = a.IsThoiBong = true; });
        }

        private void kiểmTraTrùngLặpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<AccountEx> listDup = new List<AccountEx>();
            List<string> list = new List<string>();
            foreach (AccountEx account in Accounts)
            {
                string hash = account.User + "|" + account.Pass + "|" + account.NPH + "|" + account.Server + "|" + account.LoginIndex;
                if (!list.Contains(hash))
                {
                    list.Add(hash);
                }
                else
                {
                    listDup.Add(account);
                }
            }
            if (listDup.Count > 0)
            {
                if (MessageBox.Show(this, "Có " + listDup.Count + " tài khoản trùng" + "\r\n" + "Bạn có muốn di chuyển xuống dưới không", "MicroAuto", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (Email == "Offline")
                    {
                        MicroLogin.Accounts = Accounts.Except(listDup).ToList();
                        MicroLogin.Accounts.AddRange(listDup);
                        Accounts = MicroLogin.Accounts;
                    }
                    else
                    {
                        Accounts = Accounts.Except(listDup).ToList();
                        Accounts.AddRange(listDup);
                    }
                    if (Email == "Offline")
                    {
                        MicroLogin.Instance.OnAccChange();
                    }
                    MessageBox.Show(this, "Đã di chuyển " + listDup.Count + " tài khoản trùng xuống dưới", "MicroAuto", MessageBoxButtons.OK);

                }
            }
            else
            {
                MessageBox.Show(this, "Không có tải khoản trùng", "MicroAuto", MessageBoxButtons.OK);
            }
            UpdateListS();
        }

        private void xóaTàiKhoảnTrùngLặpToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            List<AccountEx> listDup = new List<AccountEx>();
            List<string> list = new List<string>();
            foreach (AccountEx account in Accounts)
            {
                string hash = account.User + "|" + account.Pass + "|" + account.NPH + "|" + account.Server + "|" + account.LoginIndex;
                if (!list.Contains(hash))
                {
                    list.Add(hash);
                }
                else
                {
                    listDup.Add(account);
                }
            }
            if (listDup.Count > 0)
            {
                if (MessageBox.Show(this, "Có " + listDup.Count + " tài khoản trùng" + "\r\n" + "Bạn có muốn xóa không", "MicroAuto", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (Email == "Offline")
                    {
                        MicroLogin.Accounts = Accounts.Except(listDup).ToList();
                        Accounts = MicroLogin.Accounts;
                    }
                    else
                    {
                        Accounts = Accounts.Except(listDup).ToList();
                    }
                    if (Email == "Offline")
                    {
                        MicroLogin.Instance.OnAccChange();
                    }
                    MessageBox.Show(this, "Đã xóa " + listDup.Count + " tài khoản trùng lặp", "MicroAuto", MessageBoxButtons.OK);
                }
            }
            else
            {
                MessageBox.Show(this, "Không có tải khoản trùng", "MicroAuto", MessageBoxButtons.OK);
            }
            UpdateListS();
        }

        private void diChuyểnXuốngDướiCùngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Email == "Offline")
            {
                MicroLogin.Accounts = Accounts.Except(SelectedAccounts).ToList();
                MicroLogin.Accounts.AddRange(SelectedAccounts);
                Accounts = MicroLogin.Accounts;
            }
            else
            {
                Accounts = Accounts.Except(SelectedAccounts).ToList();
                Accounts.AddRange(SelectedAccounts);
            }
            UpdateListS();
            if (Email == "Offline")
            {
                MicroLogin.Instance.OnAccChange();
            }
        }

        private void diChuyểnLênTrênCùngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (Email == "Offline")
                {
                    MicroLogin.Accounts = Accounts.Except(SelectedAccounts).ToList();
                    MicroLogin.Accounts.InsertRange(0, SelectedAccounts);
                    Accounts = MicroLogin.Accounts;
                }
                else
                {
                    Accounts = Accounts.Except(SelectedAccounts).ToList();
                    Accounts.InsertRange(0, SelectedAccounts);
                }
                UpdateListS();
                if (Email == "Offline")
                {
                    MicroLogin.Instance.OnAccChange();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message + ":" + ex.StackTrace);
            }
        }

        private void xóaToànBộTênToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lvLogin.BeginUpdate();
            SelectedAccounts.ToList().ForEach(acc => acc.Status = "");
            lvLogin.EndUpdate();
        }

        private void cleanNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lvLogin.BeginUpdate();
            SelectedAccounts.ToList().ForEach(acc => { acc.Name = ""; acc.Ids = ""; });
            lvLogin.EndUpdate();
        }

        private void nhânVật1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedAccounts.ToList().ForEach(acc => acc.LoginIndex = "1");
        }

        private void nhânVật2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedAccounts.ToList().ForEach(acc => acc.LoginIndex = "2");
        }

        private void nhânVật3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedAccounts.ToList().ForEach(acc => acc.LoginIndex = "3");
        }

        private void mặcĐịnhToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedAccounts.ToList().ForEach(acc => acc.LoginIndex = "");
        }

        private void chọnMáyChủToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SelectedAccounts.Where(o => o.game != null).ForEach(o => o.game.Quit());
        }

        private void setNhómToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach(var k in SelectedAccounts.ToArray().Split(6))
            {
                Setting.Teams[k.FirstOrDefault().Name] = k.Select(k => k.Name).ToList();
            }
        }

        private void kếtNghĩaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Main.Teams.Clear();
            if (SelectedItems.Count() > 1)
            {
                Main.Instance.tmrCreateTeam.Start();
                Main.CreateTeamSate = STATE.None;

                Global.lstketban.Clear();
                foreach (ListViewItem item in SelectedItems)
                {
                    Game game = ((AccountEx)item.Tag).game;
                    Main.Teams.Add(game);
                    game.IsKetBai = true;
                    game.TrangThaiNhiemVuKetBai = "";
                    Global.lstketban.Add(game.TLBB.Name);
                }
                return;
            }
        }

        private void loginAccLvl30ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (AccountEx account in SelectedAccounts)
            {
                if (account.game == null)
                {
                    if (TDT.ParseAllInt(account.Lvl) < 30)
                        account.Status = "Đang chờ...";
                }
            }
        }

        private void sắpXếpNgẫuNhiênToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Email == "Offline")
            {
                MicroLogin.Accounts = Accounts.OrderBy(_ => Guid.NewGuid()).ToList();
                Accounts = MicroLogin.Accounts;
            }
            else
            {
                Accounts = Accounts.OrderBy(_ => Guid.NewGuid()).ToList();
            }
            UpdateListS();
        }

        private void sắpXếpTheoMầmToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void đếmTàiNguyênToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int cntHave = 0;
            int cntMam = 0;
            int cntHaveShit = 0;
            int cntShit = 0;
            int cntHaveBut = 0;
            int cntBut = 0;
            foreach (AccountEx account in Accounts)
            {
                if (account.CountMam != "0")
                {
                    cntHave++;
                    cntMam += TDT.ParseInt(account.CountMam);
                }
                if (account.CountShit != "0")
                {
                    cntHaveShit++;
                    cntShit += TDT.ParseInt(account.CountShit);
                }
                if (account.CountBut != "0")
                {
                    cntHaveBut++;
                    cntBut += TDT.ParseInt(account.CountBut);
                }
            }
            MessageBox.Show(this, "Tổng cộng có " + TDT.FormatMoney(cntHave) + " Player => Có " + TDT.FormatMoney(cntMam) + " mầm hoa\r\n"
                + "Tổng cộng có " + TDT.FormatMoney(cntHaveShit) + " Player => Có " + TDT.FormatMoney(cntShit) + " phân bón\r\n"
                           + "Tổng cộng có " + TDT.FormatMoney(cntHaveBut) + " Player => Có " + TDT.FormatMoney(cntBut) + " mao bút\r\n"
                , "MicroAuto", MessageBoxButtons.OK);
        }

        private void chạyBáchHoaDuyênToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedAccounts.ToList().ForEach(a => { a.Status = "Đang chờ làm Bách Hoa Duyên..."; a.IsBachHoaDuyen = true; });
        }

        private void nhậnMầmHoaHồngBiếnThânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedAccounts.ToList().ForEach(a => { a.Status = "Đang chờ nhận Mầm, Hoa Hồng, Biến Thân..."; a.IsBachHoaDuyen = a.IsNhanMam = a.IsNhanBienThan = true; });
        }

        private void nhậnMầmHoaHồngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedAccounts.ToList().ForEach(a => { a.Status = "Đang chờ nhận Mầm, Hoa Hồng..."; a.IsBachHoaDuyen = a.IsNhanMam = true; });
        }

        private void trồngHoaBónHoaNhậnBiếnThânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedAccounts.ToList().ForEach(a => { a.Status = "Đang chờ Trồng, Bón, Nhận..."; a.IsNhanTrongBon = a.IsNhanBienThan = true; });
        }

        private void nhậnHoaHồngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedAccounts.ToList().ForEach(a => { a.Status = "Đang chờ nhận Hoa Hồng..."; a.IsBachHoaDuyen = a.IsNhanHoaHong = true; });
        }

        private void trồngHoaBónHoaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedAccounts.ToList().ForEach(a => { a.Status = "Đang chờ Trồng, Bón..."; a.IsTrongHoa = true; });
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            SelectedAccounts.ToList().ForEach(a => { a.Status = "Đang chờ Trồng, Bón, Nhận..."; a.IsNhanTrongBon = true; });
        }

        private void némBiếnThânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedAccounts.ToList().ForEach(a => { a.Status = "Đang chờ Ném Biến Thân..."; a.IsNemBienThan = true; });
        }

        private void bónHoaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedAccounts.ToList().ForEach(a => { a.Status = "Đang chờ Bón Hoa..."; a.IsBonHoa = true; });
        }

        private void đổi999HoaHồngToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SelectedAccounts.ToList().ForEach(a => { a.Status = "Đang chờ Đổi 999 Hoa Hồng..."; a.IsDoiHoa = true; });
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            SelectedAccounts.ToList().ForEach(acc => acc.ToaDoTrongHoa = 1);
        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            SelectedAccounts.ToList().ForEach(acc => acc.ToaDoTrongHoa = 2);
        }

        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {
            SelectedAccounts.ToList().ForEach(acc => acc.ToaDoTrongHoa = 3);
        }

        private void toolStripMenuItem10_Click(object sender, EventArgs e)
        {
            SelectedAccounts.ToList().ForEach(acc => acc.ToaDoTrongHoa = 4);
        }

        private void toolStripMenuItem11_Click(object sender, EventArgs e)
        {
            SelectedAccounts.ToList().ForEach(acc => acc.ToaDoTrongHoa = 5);
        }

        private void toolStripMenuItem22_Click(object sender, EventArgs e)
        {
            foreach (AccountEx account in SelectedAccounts)
            {
                if (account.game == null)
                {
                    account.Status = "Đang chờ...";
                }
            }
        }

        private void toolStripMenuItem23_Click(object sender, EventArgs e)
        {
            foreach (AccountEx account in SelectedAccounts)
            {
                account.Status = "...";
                account.Created = false;
                account.SetNull();
                if (account.game != null)
                {
                    account.game.IsSelectLogin = false;
                    account.game = null;
                    account.IsOnline = false;
                }
            }
            Main.DicGame.ToList().ForEach(kvp => kvp.Value.listCheckedOnline.Clear());
        }

        private void toolStripMenuItem24_Click(object sender, EventArgs e)
        {
            foreach (AccountEx acc in SelectedAccounts)
            {

                if (acc.game != null)
                    acc.game.Exit();
                acc.game = null; // exacly
                acc.Status = "...";
                acc.SetNull();
            }
        }

        private void toolStripMenuItem25_Click(object sender, EventArgs e)
        {
            MicroLogin.ListCopyEx.Clear();
            foreach (AccountEx account in SelectedAccounts)
            {
                MicroLogin.ListCopyEx.Add(account);
            }
        }

        private void toolStripMenuItem26_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (AccountEx account in MicroLogin.ListCopyEx)
                {
                    AccountEx ACC = new AccountEx(account.User, account.Pass, account.NPH, account.Server, account.Tail, account.Name, account.Ids, XML, Pass);
                    ACC.Lvl = account.Lvl;
                    ACC.LoginIndex = account.LoginIndex;
                    ACC.TeamInfo = account.TeamInfo;
                    ACC.Tag = account.Tag;
                    ACC.Menpai = account.Menpai;
                    ACC.CountMam = account.CountMam;
                    ACC.CountShit = account.CountShit;
                    ACC.CountBut = account.CountBut;

                    ACC.Item = new ListViewItem((Accounts.Count + 1).ToString());
                    ACC.Item.SubItems.Add(ACC.User);
                    ACC.Item.SubItems.Add(ACC.Name);
                    ACC.Item.SubItems.Add(ACC.Server);
                    ACC.Item.SubItems.Add(ACC.LoginIndex);
                    ACC.Item.SubItems.Add(ACC.Status);
                    ACC.Item.SubItems.Add(ACC.Lvl);
                    ACC.Item.SubItems.Add(ACC.CountMam);
                    ACC.Item.SubItems.Add(ACC.CountShit);
                    ACC.Item.SubItems.Add(ACC.CountBut);
                    ACC.Item.SubItems.Add(ACC.Note);
                    ACC.Item.Tag = ACC;


                    Accounts.Add(ACC);
                }
                foreach (string s in MicroLogin.Clone.Split('#'))
                {
                    if (s.Length >= 6)
                    {

                        string name = s;
                        AccountEx ACC = new AccountEx(name, name, "VinaGame", "Thánh Hỏa", "", "", "", XML, Pass);


                        ACC.Item = new ListViewItem((Accounts.Count + 1).ToString());
                        ACC.Item.SubItems.Add(ACC.User);
                        ACC.Item.SubItems.Add(ACC.Name);
                        ACC.Item.SubItems.Add(ACC.Server);
                        ACC.Item.SubItems.Add(ACC.LoginIndex);
                        ACC.Item.SubItems.Add(ACC.Status);
                        ACC.Item.SubItems.Add(ACC.Lvl);
                        ACC.Item.SubItems.Add(ACC.CountMam);
                        ACC.Item.SubItems.Add(ACC.CountShit);
                        ACC.Item.SubItems.Add(ACC.CountBut);
                        ACC.Item.SubItems.Add(ACC.Note);
                        ACC.Item.Tag = ACC;

                        Accounts.Add(ACC);

                    }
                }
                UpdateListS();
                if (Email == "Offline")
                {
                    MicroLogin.Instance.OnAccChange();
                }

            }
            catch
            {
            }
        }

        private void cutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MicroLogin.ListCopyEx.Clear();
            foreach (AccountEx account in SelectedAccounts)
            {
                MicroLogin.ListCopyEx.Add(account);
            }
            if (Email == "Offline")
            {
                MicroLogin.Accounts = Accounts.Except(MicroLogin.ListCopyEx).ToList();
                Accounts = MicroLogin.Accounts;
            }
            else
            {
                Accounts = Accounts.Except(MicroLogin.ListCopyEx).ToList();
            }
            UpdateListS();
        }

        private void delteAccounts(object sender, EventArgs e)
        {
            try
            {
                if (FirstSelectedItem == null)
                    return;
                if (MessageBox.Show(this, "Bạn có muốn xóa thông tin những acc đã chọn", "MicroAuto", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (Email == "Offline")
                    {
                        MicroLogin.Accounts = Accounts.Except(SelectedAccounts.ToList()).ToList();
                        Accounts = MicroLogin.Accounts;
                    }
                    else
                    {
                        Accounts = Accounts.Except(SelectedAccounts.ToList()).ToList();
                    }

                    UpdateListS();
                    if (Email == "Offline")
                    {
                        MicroLogin.Instance.OnAccChange();
                    }
                }
            }
            catch
            {
            }
        }



        private void getTeamInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (AccountEx account in SelectedAccounts)
            {
                try
                {
                    account.TeamInfo = string.Join(",", Setting.Teams.Where(k => k.Value.Contains(account.Name)).Select(k => k.Key).ToArray());
                }
                catch
                {
                }
            }
        }

        private void tagToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputBox input = new InputBox();

            input.Disposed += (ss, ee) =>
            {
                {
                    if (input.TextIn != null)
                    {
                        foreach (AccountEx item in SelectedAccounts)
                        {
                            try
                            {
                                item.Tag = input.TextIn;
                            }
                            catch { }
                        }
                    }
                }
            };

            input.Show(this);
        }

        private void lưuToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (Email == "guess@yahoo.com" || string.IsNullOrEmpty(Email))
            {
                MessageBox.Show(this, "Tài khoản nặc danh không thể lưu", "MicroAuto", MessageBoxButtons.OK);
                return;
            }

            Save();
        }


        private void chọnMáyChủToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedAccounts.ToList().ForEach(acc => { if (acc.game != null) acc.game.Quit(); });
        }

 

        private void txtGoTo_TextChanged(object sender, EventArgs e)
        {
            txtGoTo.Text = TDT.ParseAllInt(txtGoTo.Text).ToString();
            txtGoTo.Select(txtGoTo.Text.Length, 0);
            try
            {
                lvLogin.EnsureVisible(TDT.ParseAllInt(txtGoTo.Text) - 1);
                lvLogin.SelectedIndices.Add(TDT.ParseAllInt(txtGoTo.Text) - 1);
            }
            catch { }
        }

        private void tạoNhómToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                List<Game> list = SelectedAccounts.Where(a => a.game != null && a.game.TLBB.Online).Select(a => a.game).ToList();
                Main.CreateTeamFromList(list);
            });
        }

        private void label4_MouseClick(object sender, MouseEventArgs e)
        {
            if(Email == "Offline")
            {
                this.Invoke(() =>
                {
                    new Loader("Không Thể Thoát").Show();
                });
                return;
            }
            List.Remove(this);
            try
            {
                threadLogin.Abort();
            }
            catch { }
            Parent.Dispose();
            this.Dispose();
            Parent.Dispose();
        }

        private void xóaTênTrùngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<AccountEx> listDup = new List<AccountEx>();
            List<string> list = new List<string>();
            foreach (AccountEx account in Accounts)
            {
                string hash = account.Name;
                if (!string.IsNullOrEmpty(hash) && hash != ",,,")
                {
                    if (!list.Contains(hash))
                    {
                        list.Add(hash);
                    }
                    else
                    {
                        listDup.Add(account);
                    }
                }
            }
            if (listDup.Count > 0)
            {
                if (MessageBox.Show(this, "Có " + listDup.Count + " tài khoản trùng" + "\r\n" + "Bạn có muốn xóa tên không", "MicroAuto", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    foreach (AccountEx a in Accounts)
                    {
                        if (listDup.Select(a => a.Name).Contains(a.Name))
                        {
                            if (a.game != null) a.game.Exit();
                            a.Name = a.Ids = "";
                        }
                    }
                    //foreach (ListViewItem item in listDup)
                    //{
                    //    ListViewLogin.Items.Add(item);
                    //}
                    MessageBox.Show(this, "Đã xóa tên " + listDup.Count + " tài khoản trùng tên", "MicroAuto", MessageBoxButtons.OK);
                    //Account.Save();
                }
            }
            else
            {
                MessageBox.Show(this, "Không có tải khoản trùng", "MicroAuto", MessageBoxButtons.OK);
            }
            UpdateListS();
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (Email == "guess@yahoo.com" || string.IsNullOrEmpty(Email))
            {
                MessageBox.Show(this, "Tài khoản nặc danh không thể lưu", "MicroAuto", MessageBoxButtons.OK);
                return;
            }

            Save();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void dưaHấuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedAccounts.ToList().ForEach(a => { a.Status = "Đang chờ làm Q Dưa..."; a.IsLoginDua = true; });
        }

        private void trừngÁcToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedAccounts.ToList().ForEach(a => { a.Status = "Đang chờ làm Trừng Ác..."; a.IsLoginTrungAc = true; });
        }

        private void tiềmNăngTánToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedAccounts.ToList().ForEach(a => { a.Status = "Đang chờ làm Tiềm Năng Tán..."; a.IsLoginTiemNangTan = true; });
        }

        private void nhiệmVụThăngCấpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedAccounts.ToList().ForEach(a => { a.Status = "Đang chờ làm Nhiệm Vụ Thăng Cấp..."; a.IsLoginNhiemVuThangCap = true; });
        }

        private void nhiệmVụExp2838ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedAccounts.ToList().ForEach(a => { a.Status = "Đang chờ làm Nhiệm Vụ Exp..."; a.IsLoginNhiemVuExp = true; });
        }

        private void lễBaoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedAccounts.ToList().ForEach(a => { a.Status = "Đang chờ Nhận Lễ Bao..."; a.IsLoginLeBao = true; });
        }

        private void biếnThânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedAccounts.ToList().ForEach(a => { a.Status = "Đang chờ Nhận Biến Thân..."; a.IsNhanBienThan = true; });
        }

        private void bồiThườngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedAccounts.ToList().ForEach(a => { a.Status = "Đang chờ Nhận Bồi Thường..."; a.IsLoginBoiThuong = true; });
        }

        private void ácTặcToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedAccounts.ToList().ForEach(a => { a.Status = "Đang chờ Ác Tặc..."; a.IsLoginAcTac = true; });
        }

        private void thiếuThấtSơnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedAccounts.ToList().ForEach(a => { a.Status = "Đang chờ Thiếu Thất Sơn..."; a.IsLoginThieuThatSon = true; });
        }

        private void lâuLanTầmBảoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedAccounts.ToList().ForEach(a => { a.Status = "Đang chờ Lâu Lan Tầm Bảo..."; a.IsLoginLauLanTamBao = true; });
        }

        private void tínhLạiTốcĐộXongBHDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MicroLogin.CountXongBHD = 0;
            MicroLogin.swXongBHD = null;
            Game.TotalXongBHD = 0;
            Game.swTotalXongBHD = Stopwatch.StartNew();
        }
        bool isdes = false;
        private void lvLogin_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            
            if (Accounts.Where(acc => acc.Item == null).Count() > 0)
            {
                MessageBox.Show(this, "Ctrl + A để chọn toàn bộ acc trước rồi mới sắp xếp được nhé", "MicroAuto", MessageBoxButtons.OK);
                return;
            }
            else
            {
                if (MessageBox.Show(this, "Bạn có muốn sắp xếp danh sách Accounts", "MicroAuto", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }
            }
            isdes = !isdes;
            try
            {
                int col = Convert.ToInt32(e.Column.ToString());
                if (col == 0 || col == 4 || col >= 6)
                {
                    if (Email == "Offline")
                    {
                        if (isdes)
                        {
                            MicroLogin.Accounts = Accounts.OrderBy(acc => TDT.ParseAllInt(acc.Item.SubItems[col].Text)).ToList();
                            Accounts = MicroLogin.Accounts;
                        }
                        else
                        {
                            MicroLogin.Accounts = Accounts.OrderByDescending(acc => TDT.ParseAllInt(acc.Item.SubItems[col].Text)).ToList();
                            Accounts = MicroLogin.Accounts;
                        }
                    }
                    else
                    {
                        if (isdes)
                        {
                            Accounts = Accounts.OrderBy(acc => TDT.ParseAllInt(acc.Item.SubItems[col].Text)).ToList();
                        }
                        else
                        {
                            Accounts = Accounts.OrderByDescending(acc => TDT.ParseAllInt(acc.Item.SubItems[col].Text)).ToList();
                        }
                    }
                }
                else
                {
                    if (Email == "Offline")
                    {
                        if (isdes)
                        {
                            MicroLogin.Accounts = Accounts.OrderBy(acc => acc.Item.SubItems[col].Text).ToList();
                            Accounts = MicroLogin.Accounts;
                        }
                        else
                        {
                            MicroLogin.Accounts = Accounts.OrderByDescending(acc => acc.Item.SubItems[Convert.ToInt32(e.Column.ToString())].Text).ToList();
                            Accounts = MicroLogin.Accounts;
                        }
                    }
                    else
                    {
                        if (isdes)
                        {
                            Accounts = Accounts.OrderBy(acc => acc.Item.SubItems[col].Text).ToList();
                        }
                        else
                        {
                            Accounts = Accounts.OrderByDescending(acc => acc.Item.SubItems[Convert.ToInt32(e.Column.ToString())].Text).ToList();
                        }
                    }
                }
            }
            catch { }
            UpdateListS();
        }

        private void txtLimit_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void đưaKNBChoTaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (AccountEx account in SelectedAccounts)
            {
                account.Status = "Đang chờ làm BHD...";
                account.IsGomKNB = true;
            }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            try
            {
                nudStop.Value = nudStop.Value - 1;
            }
            catch { }
        }

        private void label1_MouseClick(object sender, MouseEventArgs e)
        {
            if(Email == "Offline")
            {
                Task.Run(() =>
                {
                    MicroLogin.SaveXML();
                    this.Invoke(() =>
                    {
                        new Loader("Lưu Danh Sách Login Thành Công").Show();
                    });                    
                });
                return;
            };
            
            if (Email == "guess@yahoo.com" || string.IsNullOrEmpty(Email))
            {
                this.Invoke(() =>
                {
                    new Loader("Không Thể Lưu").Show();
                });                
                return;
            }

            Save();
        }

        private void lvLogin_MouseClick(object sender, MouseEventArgs e)
        {
          
        }

        private void lvLogin_MouseMove(object sender, MouseEventArgs e)
        {
          
            //lvLogin.Columns[0].Text = coordinates.X + "," + coordinates.Y;
        }

        public List<int> listHideColumn = new List<int>()
        {
            1,3,7,8,9,10
        };

        public List<int> listshow = new List<int>()
        {
           
        };

        private void userToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var menu = sender as ToolStripMenuItem;
            menu.Checked = !menu.Checked;
            int idx = 1;
            listshow.Remove(idx);
            if (menu.Checked)
            {
                lvLogin.Columns[idx].Width = 80;
                listHideColumn.Remove(idx);
                listshow.Add(idx);
            }
            else
            {
                lvLogin.Columns[idx].Width = 0;
                listHideColumn.Add(idx);
            }
            SaveShow();
        }

        private void serverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var menu = sender as ToolStripMenuItem;
            menu.Checked = !menu.Checked;
            int idx = 3;
            listshow.Remove(idx);
            if (menu.Checked)
            {
                lvLogin.Columns[idx].Width = 70;
                listHideColumn.Remove(idx);
                listshow.Add(idx);
            }
            else
            {
                lvLogin.Columns[idx].Width = 0;
                listHideColumn.Add(idx);
            }
            SaveShow();
        }

        private void mầmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var menu = sender as ToolStripMenuItem;
            menu.Checked = !menu.Checked;
            int idx = 7;
            listshow.Remove(idx);
            if (menu.Checked)
            {
                lvLogin.Columns[idx].Width = 40;
                listHideColumn.Remove(idx);
                listshow.Add(idx);
            }
            else
            {
                lvLogin.Columns[idx].Width = 0;
                listHideColumn.Add(idx);
            }
            SaveShow();
        }

        private void shitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var menu = sender as ToolStripMenuItem;
            menu.Checked = !menu.Checked;
            int idx = 8;
            listshow.Remove(idx);
            if (menu.Checked)
            {
                lvLogin.Columns[idx].Width = 40;
                listHideColumn.Remove(idx);
                listshow.Add(idx);
            }
            else
            {
                lvLogin.Columns[idx].Width = 0;
                listHideColumn.Add(idx);
            }
            SaveShow();
        }

        private void bútToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var menu = sender as ToolStripMenuItem;
            menu.Checked = !menu.Checked;
            int idx = 9;
            listshow.Remove(idx);
            if (menu.Checked)
            {
                lvLogin.Columns[idx].Width = 40;
                listHideColumn.Remove(idx);
                listshow.Add(idx);
            }
            else
            {
                lvLogin.Columns[idx].Width = 0;
                listHideColumn.Add(idx);
            }
            SaveShow();
        }

        void SaveShow()
        {
            Game.IniParser.Write("Config", "Column", string.Join(",", listshow.Select(i => i.ToString()).ToArray()));
        }

        private void infoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var menu = sender as ToolStripMenuItem;
            menu.Checked = !menu.Checked;
            int idx = 10;
            listshow.Remove(idx);
            if (menu.Checked)
            {
                lvLogin.Columns[idx].Width = 120;
                listHideColumn.Remove(idx);
                listshow.Add(idx);
            }
            else
            {
                lvLogin.Columns[idx].Width = 0;
                listHideColumn.Add(idx);
            }
            SaveShow();
        }

        private void muAcBaClick(object sender, EventArgs e)
        {
            SelectedAccounts.ToList().ForEach(a => { a.Status = "Đang chờ Ác Bá..."; a.AcBa = (sender as ToolStripMenuItem).Text; });
        }

        private void TabLogin_Resize(object sender, EventArgs e)
        {
            HideShowCol();
        }
    }
}
