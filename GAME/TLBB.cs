using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.Linq;
using System.Text.RegularExpressions;

namespace _i
{
    class TlBBChild : TLBB
    {
        public TlBBChild(Game game) : base(game)
        {
            //base();
        }
    }

    class TLBB
    {

        public bool IsDead
        {
            get
            {
                return PlayerState == 9;
            }
        }

        public string AllowName
        {
            get
            {
                
                string n = "";
                foreach(char s in Name)
                {
                    if (TDT.AllowName.Contains(s))
                    {
                        n += s;
                    }
                }
                return n;
            }
        }

        

        public bool IsDaiTheGioi
        {
            get
            {
                return IsMapChienMinh(MapId) || MapId == MAP.PhungMinhTran || MapId == MAP.VanPhu || MapId == MAP.NhaiDuDao || MapId == MAP.KimLang;
            }
        }

        public NPC NPCKy
        {
            get
            {
                if (Menpai == MENPAI.CaiBang)
                    return CAIBANG.LyNhatViet;
                if (Menpai == MENPAI.DuongMon)
                    return DUONGMON.DuongChuCo;
                if (Menpai == MENPAI.MinhGiao)
                    return MINHGIAO.DangNguyenGiac;
                if (Menpai == MENPAI.MoDung)
                    return MODUNG.PhongThienLy;
                if (Menpai == MENPAI.NgaMy)
                    return NGAMY.TieuTuongNgoc;
                if (Menpai == MENPAI.QuyCoc)
                    return QUYCOC.ToCam;
                if (Menpai == MENPAI.ThienLong)
                    return THIENLONG.DuongBachNguu;
                if (Menpai == MENPAI.ThienSon)
                    return THIENSON.NhamPhiHong;
                if (Menpai == MENPAI.ThieuLam)
                    return THIEULAM.HuyenSinh;
                if (Menpai == MENPAI.TieuDao)
                    return TIEUDAO.CauDoc;
                if (Menpai == MENPAI.TinhTuc)
                    return TINHTUC.ThienUngTu;
                if (Menpai == MENPAI.VoDang)
                    return VODANG.TruongQuanMo;
                if (Menpai == MENPAI.DaoHoa)
                    return DAOHOA.HoangChiTri;
                return null;
            }
        }


        public Game Game { get; set; }
        public Memory Memory;
        public Address Address;

        public uint Base { get; set; }
        public string Id { get; set; } = string.Empty;
        public string Name
        {
            get
            {
                Base = Memory.Read(Address.CharBase);
                string name = Memory.ReadShortString(Base + Address.CharName);
                if (string.IsNullOrEmpty(name.Trim()))
                    name = "ĐăngNhập";
                return name;
            }
        }

        public byte[] VISCIIName
        {
            get
            {
                Base = Memory.Read(Address.CharBase);
                return Memory.ReadVISCIIShortString(Base + Address.CharName);
            }
        }

        public uint Menpai { get; set; }
        public uint Lvl { get; set; }
        public uint Rage { get; set; }
        public bool IsFollow { get { return Memory.Read(Base + Address.CharIsFollow) == 1; } }
        public uint PetId { get; set; }
        public uint HP { get; set; }
        public uint MP { get; set; }
        public uint Exp { get; set; }
        public uint MaxHP { get; set; }
        public uint MaxMP { get; set; }
        public uint PetBase { get; set; }
        public uint PetHP { get; set; }
        public uint PetMaxHP { get; set; }
        public uint PetEnjoy { get; set; }
        public uint PetLvl { get; set; }
        public uint NangDong { get; set; }

        public void Jump()
        {
            Game.PostMessage(0, 120);
        }

        public void Talk(string msg)
        {
            Game.DoStringEx("TXT = '" + msg + "';");
            Game.PostMessage(3, 105);
        }
        public bool IsPlayAni
        {
            get
            {
                return Memory.Read(Address.TEXTVALIDATE_SAVELOGINSELECT) == 1;
            }
        }
        public bool IsRelive
        {
            get
            {
                return Memory.Read1Byte(Memory.ReadAddress(Address.IsRelive)) == 1;
            }
        }
        public uint Gold
        {
            get
            {
                if (Address.GameType == 1)
                    Address.PlayerGold = 0x4468; 
                return Memory.Read(Base + Address.PlayerGold);
            }
        }

        public uint BankGold
        {
            get
            {
                return Memory.Read(Address.PetBase[0], 0x1CBA4);
            }
        }



        public uint GiaoTu
        {
            get
            {
                if (Address.GameType == 1)
                    Address.PlayerLockedGold = 0x2B9C;
                return Memory.Read(Base + Address.PlayerLockedGold);
            }
        }
public uint KNBKhoa
        {
            get
            {
                if (Address.GameType == 1)
                    Address.PlayerLockedKNB = 0x2E58;//294c
                return Memory.Read(Base + Address.PlayerLockedKNB);
            }
        }
        public uint KNB
        {
            get
            {
                if (Address.GameType == 1)
                    Address.PlayerKNB = 0x45AC;
                return Memory.Read(Base + Address.PlayerKNB);
            }
        }

        public uint KNBThongBao
        {
            get
            {
                if (Address.GameType == 1)
                    Address.PlayerKNBThongBao = 0x2E64;
                return Memory.Read(Base + Address.PlayerKNBThongBao);
            }
        }

        public uint DiemTang
        {
            get
            {
                if (Address.GameType == 1)
                    Address.PlayerDiemTang = 0x2E60;
                return Memory.Read(Base + Address.PlayerDiemTang);
            }
        }
        public uint MaxODaoCu
        {
            get
            {
                uint maxnum = Memory.Read(Address.ODaoCu);
                if (maxnum == 10141355)
                    maxnum = 30;
                else if (maxnum >= 10141020)
                    maxnum = maxnum - 10141020 + 20;                
                if (maxnum == 0)
                    maxnum = 20;            
                return maxnum;
            }
        }

        public uint MaxONguyenLieu
        {
            //$+8      >009ABD6A  Game.009ABD6A +4
            //$+8      >009ABD6A  Game.009ABD6A


            get
            {
                uint maxnum = Memory.Read(Address.ONguyenLieu);
                if (maxnum == 10141356)
                    maxnum = 30;
                else if (maxnum >= 10141030)
                    maxnum = maxnum - 10141030 + 20;
                if (maxnum == 0)
                    maxnum = 20;
                return maxnum;
            }
        }

        public bool IsODaoCuFull { get; set; }
        public bool IsONguyenLieuFull { get; set; }
        public bool isONguyenLieuFull
        {
            get
            {
                uint maxnum = MaxONguyenLieu;
                uint address = Memory.Read(Address.PacketItemBase);
                for (uint i = 30; i < 30 + maxnum; i++)
                {
                    if (Memory.Read(address + i * 0x4) == 0)
                    {
                        return false;
                    }
                }
                return true;
            }
        }
        public bool isODaoCuFull
        {
            get
            {
                uint maxnum = MaxODaoCu;
                uint address = Memory.Read(Address.PacketItemBase);
                for (uint i = 0; i < maxnum; i++)
                {
                    if (Memory.Read(address + i * 0x4) == 0)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public bool IsCanQuet
        {
            get
            {
                for(uint i = 0; i < 12; i++)
                {
                    if (Memory.Read(new uint[] { Address.KeyId[0], 0x194C, i * 4 }) > 0)
                    {
                       
                        return true;

                    }
                }
                return false;
            }
        }

        public bool IsODaoCuFullBHD
        {
            get
            {
                uint maxnum = MaxODaoCu;
                uint address = Memory.Read(Address.PacketItemBase);
                uint oTrong = 0;
                for (uint i = 0; i < maxnum; i++)
                {
                    if (Memory.Read(address + i * 0x4) == 0)
                    {
                        oTrong++;
                    }
                    if (oTrong >= 2)
                        break;
                }
                if (oTrong >= 2)
                    return false;
                return true;
            }
        }

        public bool IsODaoCuFullTrungAc
        {
            get
            {
                uint maxnum = MaxODaoCu;
                uint address = Memory.Read(Address.PacketItemBase);
                uint oTrong = 0;
                for (uint i = 0; i < maxnum; i++)
                {
                    if (Memory.Read(address + i * 0x4) == 0)
                    {
                        oTrong++;
                    }
                    if (oTrong >= 3)
                        break;
                }
                if (oTrong >= 3)
                    return false;
                return true;
            }
        }


       

        public string Img { get; set; }
        public uint BaseImg { get; set; }

        int BaseAnswer { get; set; }

        public string[] answer;

        public string[] Answer
        {
            get
            {
                return answer;
            }
        }

        private Bitmap captcha;

        public string ImgHash { get; set; }
        public string bin { get; set; }

        public string BinEx { get; set; }
        

        public static bool ValidAnswer(int answer)
        {
            if (answer >= 31 && answer <= 0x5A && answer != 0x40)
                return true;
            return false;
        }

        class ANSWER
        {
            public int Height;
            public string AS;
        }

        int[] BaseAnswerEx = new int[4];
        List<uint> ResultCaptcha = new List<uint>();

        public static string GetHash(int baseAddress)
        {
            return "";
        }

        public string NiceName
        {
            get
            {
                return Regex.Replace(Name, "[^0-9a-zA-Z]", "");
            }
        }

        public void ReadCaptcha()
        {
            try
            {
                //if (IsReadCaptcha && (ResultCaptcha.Count == 0 || BaseImg == 0))
                //    return;
                bin = "";
                BinEx = "";
                int select1;
                int select2;
                int select3;
                int select4;
                Game.ArrayOfByte.Flush();
                Game.ArrayOfByte.IsSearchBaseImg = true;
                Stopwatch swRead = Stopwatch.StartNew();
                ResultCaptcha.Clear();
                if (ResultCaptcha.Count == 0)
                {
                    answer = new string[4];

                    ResultCaptcha.Add(Game.Memory.Read(Game.AddressGameExe + 0x99AB00) + 0x4881A4);
                    ResultCaptcha.Add(Game.Memory.Read(Game.AddressGameExe + 0x99AB00) + 0x4881A4 + 0x10);
                    ResultCaptcha.Add(Game.Memory.Read(Game.AddressGameExe + 0x99AB00) + 0x4881A4 + 0x20);
                    ResultCaptcha.Add(Game.Memory.Read(Game.AddressGameExe + 0x99AB00) + 0x4881A4 + 0x30);

                }


                int cnt = -1;
                List<ANSWER> asw = new List<ANSWER>();
                //Dictionary<int, string> = new 
                for (int i = 0; i < ResultCaptcha.Count; i++)
                {
                    if (cnt > 3)
                        break;
                    //Main.PushLogEx(Result[i].ToString("X8"));
                    //select1 = Game.Memory.Read(ResultCaptcha[i] - 0x184);
                    //select2 = Game.Memory.Read(ResultCaptcha[i] - 0x184 + 0x4);
                    //select3 = Game.Memory.Read(ResultCaptcha[i] - 0x184 + 0x8);
                    //select4 = Game.Memory.Read(ResultCaptcha[i] - 0x184 + 0xC);   
                    select1 = (int)Game.Memory.Read2Byte(ResultCaptcha[i]);
                    select2 = (int)Game.Memory.Read2Byte(ResultCaptcha[i] + 0x2);
                    select3 = (int)Game.Memory.Read2Byte(ResultCaptcha[i] + 0x4);
                    select4 = (int)Game.Memory.Read2Byte(ResultCaptcha[i] + 0x6);
                    if (ValidAnswer(select1) && ValidAnswer(select2) && ValidAnswer(select3) && ValidAnswer(select4))
                    {
                        cnt = cnt + 1;
                        answer[cnt] = ConverterEx.Unicodes[select1].ToString() + ConverterEx.Unicodes[select2].ToString() + ConverterEx.Unicodes[select3].ToString() + ConverterEx.Unicodes[select4].ToString();
                        ANSWER aw = new ANSWER();
                        aw.Height = 4-cnt;
                        aw.AS = answer[cnt];
                        asw.Add(aw);
                    }
                }
                asw = asw.OrderBy(x => x.Height).Reverse().ToList();
                for (int i = 0; i < asw.Count; i++)
                {
                    answer[i] = asw[i].AS;
                }

                uint hexColor;
                string img;
                try
                {
                    Img = string.Empty;
                    ImgHash = string.Empty;                
                    while (true)
                    {                       
                        if (BaseImg == 0)
                        {
                            BaseImg = Game.Memory.Read(new uint[] { Game.AddressGameExe + 0x9A977C, 0x1C, 0x10, 0x24, 0x44, 0x10, 0xD4, 0x4, 0x68, 0x8, 0xF0 });                            
                            if (Memory.Read(BaseImg).ToString("X8").StartsWith("A0"))
                            {
                                Img = "";
                                hexColor = Memory.Read2Byte(BaseImg);
                                if (hexColor == 0xA000 || hexColor == 0xA0FF)
                                {
                                    //Main.PushLog("base " + BaseImg.ToString("X8"));
                                    byte[] buffer = new byte[9216];
                                    Memory.ReadProcessMemory(Memory.Id, BaseImg, buffer, buffer.Length, 0);
                                    for (int j = 0; j < 9216; j = j + 2)
                                    {
                                        hexColor = BitConverter.ToUInt32(new byte[4] { buffer[j], buffer[j + 1], 0, 0 }, 0);
                                        if (hexColor == 0xA000)
                                        {
                                            //captcha.SetPixel((j / 2) % 128, (j / 2) / 128, Color.Black);
                                            bin += "0";
                                            BinEx += "0";
                                        }
                                        else
                                        {
                                            //captcha.SetPixel((j / 2) % 128, (j / 2) / 128, Color.White);
                                            bin += "1";
                                            BinEx += "1";
                                        }
                                        if (bin.Length == 8)
                                        {
                                            Img += Convert.ToInt32(bin, 2).ToString("X2");
                                            bin = "";
                                        }
                                    }
                                    //captcha = new Bitmap(captcha, new Size(160, 45));
                                }
                                else
                                {
                                    //continue;
                                }
                                img = Img;
                                
                                Img += answer[0] + answer[1] + answer[2] + answer[3];
                                ImgHash = TDT.Hasher.MD5(Img);
                                //Main.PushLog("here" + BaseImg.ToString("X8"));
                                // Main.PushLog(ImgHash);
                                if (!Game.CaptchaHash.Contains(ImgHash))
                                {
                                    Game.CaptchaHash.Add(ImgHash);
                                    Global.SaveCaptcha();
                                    // Main.PushLog("break");
                                   
                                }
                                Game.ArrayOfByte.IsSearchBaseImg = false;
                                IsReadCaptcha = true;
                                break;
                            }
                            else
                            {
                                //Main.PushLog("clgt" + BaseImg.ToString("X8"));
                                //Stopwatch sw = Stopwatch.StartNew();
                                //BaseImg = Memory.Scan("## 00 00 00 ?? A0 ?? A0 ?? A0 ?? A0 ?? A0 ?? A0 ?? A0 ?? A0 ?? A0 ?? A0 ?? A0 ?? A0 ?? A0 ?? A0", 0, 0x7FFFFFFF, 0) + 0x4;
                                //BaseImg = (int)Game.AOB.SearchPrivateRegion(new byte[] { 0x01, 0x00, 0x00, 0x00, 0xFF, 0xA0, 0xFF, 0xA0, 0xFF, 0xA0, 0xFF, 0xA0, 0xFF, 0xA0, 0xFF, 0xA0, 0xFF, 0xA0, 0xFF, 0xA0, 0xFF, 0xA0, 0xFF, 0xA0, 0xFF, 0xA0, 0xFF, 0xA0 }, 0, 0x7FFFFFFF) + 0x4;
                                uint adr = Game.ArrayOfByte.Search("## 00 00 00 ?? A0 ?? A0 ?? A0 ?? A0 ?? A0 ?? A0 ?? A0 ?? A0 ?? A0 ?? A0 ?? A0 ?? A0 ??");
                                if (adr != 0)
                                {
                                    BaseImg = adr + 0x4;
                                }
                                else
                                {
                                    //Main.PushLogEx(adr.ToString("X8"));
                                    break;
                                }
                            }
                        }
                        else
                        {
                            // Main.PushLog("vkl");
                            uint adr = Game.ArrayOfByte.SearchNext("## 00 00 00 ?? A0 ?? A0 ?? A0 ?? A0 ?? A0 ?? A0 ?? A0 ?? A0 ?? A0 ?? A0 ?? A0 ?? A0 ??");
                            if (adr != 0)
                            {
                                //Main.PushLog("vaidai" + adr.ToString("X8"));
                                BaseImg = adr + 0x4;
                            }
                            else
                            {
                               // Main.PushLog("dcm" + adr.ToString("X8"));
                                break;
                            }
                        }
                        hexColor = Memory.Read2Byte(BaseImg);
                        if (hexColor == 0xA000 || hexColor == 0xA0FF)
                        {
                            Img = "";
                            byte[] buffer = new byte[9216];
                            Memory.ReadProcessMemory(Memory.Id, BaseImg, buffer, buffer.Length, 0);
                            for (int j = 0; j < 9216; j = j + 2)
                            {
                                hexColor = BitConverter.ToUInt32(new byte[4] { buffer[j], buffer[j + 1], 0, 0 }, 0);
                                if (hexColor == 0xA000)
                                {
                                    //captcha.SetPixel((j / 2) % 128, (j / 2) / 128, Color.Black);
                                    bin += "0";
                                    BinEx += "0";
                                }
                                else
                                {
                                    //captcha.SetPixel((j / 2) % 128, (j / 2) / 128, Color.White);
                                    bin += "1";
                                    BinEx += "1";
                                }
                                if (bin.Length == 8)
                                {
                                    Img += Convert.ToInt32(bin, 2).ToString("X2");
                                    bin = "";
                                }
                            }
                            //captcha = new Bitmap(captcha, new Size(160, 45));
                        }
                        else
                        {
                            //continue;
                        }
                        img = Img;
                        
                        Img += answer[0] + answer[1] + answer[2] + answer[3];
                        ImgHash = TDT.Hasher.MD5(Img);
                        //Main.PushLog("here" + Img.ToString("X8"));
                        // Main.PushLog(ImgHash);
                       // Main.PushLog(BaseImg.ToString("X8"));
                        if (!Game.CaptchaHash.Contains(ImgHash))
                        {
                            Game.CaptchaHash.Add(ImgHash);
                            Global.SaveCaptcha();
                           // Main.PushLog("break");
                            break;
                        }
                      
                     
                    }
                          
                }
                catch { }
            }
            catch { }
            Game.ArrayOfByte.IsSearchBaseImg = false;
            IsReadCaptcha = true;
        }


        public uint PartyId { get; set; }
        public Bitmap Captcha
        {
            get
            {          
                if (captcha == null && !string.IsNullOrEmpty(BinEx) && BinEx.Length == 4608)
                {
                    captcha = new Bitmap(128, 36);
                    int j = 0;
                    foreach (char b in BinEx)
                    {
                        if (b == '0')
                        {
                            captcha.SetPixel((j / 2) % 128, (j / 2) / 128, Color.Black);
                        }
                        else
                        {
                            captcha.SetPixel((j / 2) % 128, (j / 2) / 128, Color.White);
                        }                        
                        j = j + 2;
                    }
                    captcha = new Bitmap(captcha, new Size(160, 45));
                }
                return captcha;
            }
            set
            {
                captcha = value;
            }
        }

        public string RaoTxt
        {
            get
            {
                uint baseAddress = Memory.GetModuleAddress("UI_CEGUI.dll");
                if(Address.GameType == 1)
                {
                    baseAddress = baseAddress + 0x61C08;
                }
                else
                {
                    baseAddress = baseAddress + 0x37304;
                }
                return Memory.ReadString(Memory.Read(baseAddress));
            }
        }
        
        public bool IsFresh
        {
            get
            {
                return Memory.Read(Address.CountDown10Sec) == 1;
            }
        }

        public bool IsShopOpen
        {
            get
            {
                return Memory.Read(Address.IsShopOpen) == 1;
            }
        }

     

        //10141356

        public uint State
        {
            get
            {
                return Memory.Read(Address.CharState);
            }
        }
        
        public uint PlayerState // 5 - lên ngựa, thu phục | 6, đào khoáng bang | 7 - đánh
        {
            get; set;
        }
        public bool IsCaptcha
        {
            get
            {
                //lecaotri2020cansua
               // return false;
                return Memory.Read(Address.IsCaptcha) == 1;
            }
        }



        public bool IsChuyenDoiMess
        {
            get
            {
                return Memory.Read(Address.IsCHuyenToDoiMess) == 1;
            }
        }

        public bool IsPk;
        public bool Disconnected
        {
            get
            {
                return Memory.Read(Address.Disconnected) == 1;
            }
        }
        public string KeyId { get; set; }
        public uint MapId { get; set; }

        public bool IsMapChienMinh(uint mapId)
        {
            if (mapId >= 663 && mapId <= 667)
                return true;
            return false;
        }

        public bool IsMapBang(uint mapId)
        {
            if (mapId >= 501 && mapId <= 545)
                return true;
            return false;
        }

        public string MapName;

        public string MapNameClean
        {
            get
            {
                return TDT.ClearSign(MapName);
            }
        }

        public string SkillPetType { get; set; }

        public string SkillPetType1 { get; set; }
        public string SkillPetType2 { get; set; }
        public uint PetHaveSkill1 { get; set; }
        public uint PetHaveSkill2 { get; set; }

        public int PetHaveSkill { get; set; }

        public uint OnlineTime;
        public uint OnlineTimeSec
        {
            get;
            set;
        }
        public uint DelayBase;
        public string GuildName { get; set; }
        public uint GuildId { get; set; }
        public uint MenpaiPoint
        {
            get
            {
                return Memory.Read(Base + Address.CharMenpaiPoint);
            }
        }

        public uint SafeTime
        {
            get
            {
                return Memory.Read(Address.SafeTime);
            }
        }

        public bool IsNexLogin
        {
            get
            {
                return Memory.Read(Address.IsNexLogin) == 1;
            }
        }
       
        public bool IsSelectServer
        {
            get
            {
                return Memory.Read(Address.IsSelectServer) == 1;
            }
        }

        public bool IsCreatePlayer
        {
            get
            {
                return Memory.Read(Address.IsCreatePlayer) == 1;
            }
        }

        public bool IsTextCaptcha
        {
            get
            {
                return Memory.Read(Address.IsTextCaptcha) == 1;
            }
        }

        public bool IsTrader
        {
            get
            {
                
                if (Game.Objects.Self == null)
                    return false;
                else if (Game.Objects.Self.Buff.Contains(271))
                    return true;
                return false;
            }
        }

        public bool IsLogon
        {
            get
            {
                return Memory.Read(Address.IsLogon) == 1;
            }
        }

        

        public bool IsSelectServerQuest
        {
            get
            {
                return Memory.Read(Address.IsLoginMessage) == 1;
            }
        }
        public bool IsSelectRole
        {
            get
            {
                return Memory.Read(Address.IsSelectCharacter) == 1;
            }
        }

        public bool IsSelectRoleCreate
        {
            get
            {
                return Memory.Read(Address.IsSelectRoleCreateA) == 1;
            }
        }

        //dd [[[0ADF4B8]+8]+4]+14c
        public uint FakeMapId
        {
            get
            {
                return Memory.Read(Address.FakeMapId);
            }
        }
        public uint HPPercent
        {
            get
            {                
                return Percent(HP, MaxHP);
            }
        }

        public uint HPPercentEx
        {
            get
            {
                return Percent(HP, MaxHP,false);
            }
        }

        public uint MPPercent
        {
            get
            {
                return Percent(MP, MaxMP);
            }
        }

        public uint MPPercentEx
        {
            get
            {
                return Percent(MP, MaxMP,false);
            }
        }

        public uint PetHPPercentEx
        {
            get
            {
                return Percent(PetHP, PetMaxHP, false);
            }
        }
        public uint PetHPPercent
        {
            get
            {
                return Percent(PetHP, PetMaxHP);
            }
        }

        public bool Busy
        {
            get
            {
                if (PlayerState == 5)
                    return false;
                if (PlayerState > 2 && PlayerState < 10)
                {
                    return true;
                }
                return false;
            }
        }


        public bool BusyEx
        {
            get
            {
                if (PlayerState <= 0)
                    return false;
                if (PlayerState == 2 || PlayerState == 7)
                    return false;
                return true;                
            }
        }

        private static uint Percent(uint min, uint max, bool is99 = true)
        {
            if (max == 0) return 0;
            uint percent = (uint)(min * 100 / max);
            if (percent == 0 && min > 0) return 1;
            if (is99)
                if (percent >= 100) return 99;
            return percent;
        }
        public int MaxExp
        {
            get
            {
                if (Lvl > 0 && Lvl < 150)
                {
                    return MAXEXP.Lvl[Lvl];
                }
                return MAXEXP.Lvl[1];
            }
        }
        public float ExpPercent
        {
            get
            {
                return (float)Exp * 100 / MaxExp;
            }
        }
        public bool IsNoiCong
        {
            get
            {
                if(Menpai == MENPAI.QuyCoc || Menpai == MENPAI.VoDang || Menpai == MENPAI.ThienLong || Menpai == MENPAI.NgaMy || Menpai == MENPAI.DuongMon || Menpai == MENPAI.TinhTuc || Menpai == MENPAI.TieuDao || Menpai == MENPAI.DaoHoa)
                    return true;
                return false;
            }
        }

        public bool IsNgoaiCong => !IsNoiCong;

        public bool IsRide { get; set; }
        public bool IsKetBaiOpen
        {
            get
            {
                return Memory.Read(Address.KetBaiInfo) == 1;
            }
        }
        public bool IsLeader
        {
            get
            {
                if (!Online)
                    return false;
                if (Id.Contains(KeyId))
                    return true;
                return false;
            }
        }

        public bool IsOnline
        {
            get;
            set;
        }

       

        public bool Online
        {
            get
            {
                //if (Game.tranTime.Elapsed.TotalSeconds < 2)
                //    return false;
                if (string.IsNullOrEmpty(Id) || string.IsNullOrEmpty(Name))
                    return false;
                if (Id.Contains("00000000") || Id.Contains("FFFFFFFF") || Name == "ĐăngNhập" || Lvl < 1 || Lvl == 0xFFFFFFFF)
                    return false;
                IsOnline = true;
                return true;
            }
        }

        public uint PetCount
        {
            get
            {
                uint cnt = 0;
                uint petBase = Memory.Read(Address.PetBase);
                for (uint i = 0; i < 20; i++)
                {
                    if (Memory.Read(petBase + Address.PetDataSize * i + Address.PetId) != 0)
                    {
                        uint maxHP = Memory.Read(petBase + Address.PetDataSize * i + Address.PetMaxHP);
                        if (maxHP > 0)
                            cnt++;
                        else
                            break;
                    }
                    else
                    {
                        //break;
                    }
                }
                return cnt;
            }
        }

        public bool IsBienThan { get; set; }

        public bool IsTogleMission
        {
            get
            {
                return Memory.Read(Address.IsTogleMission) == 1;
            }
        }

        public bool IsToggleYuanbaoShop
        {
            get
            {
                if(Address.GameType == 1)
                {
                    if (Game.Address.Is2D)
                    {
                        //ACEB8C
                        return Memory.Read(new uint[] { 0xAE48E8, 0x8, 0x4, 0x14C }) == 1;
                    }
                    else
                    {
                        return Memory.Read(new uint[] { 0xAD1B8C, 0x0, 0xC, 0x64 }) == 1;
                    }
                }
                return Memory.Read(new uint[] { 0x642678, 0x0, 0xC, 0x64 }) == 1;
            }
        }

        public bool IsExchangeOpen
        {
            get
            {
                return Memory.Read(new uint[] { Game.AddressGameExe + 0x855CA8, 0x8, 0x4, 0x144 }) == 1;
            }
        }

        public bool IsQuestOpen
        {
            get
            {
                return Memory.Read(Address.QuestInfo) == 1;
            }
        }

        public bool IsThongBaoShow
        {
            get
            {
                return Memory.Read(Address.ThongBaoInfo) == 1;
            }
        }

        
        public List<uint> Rides
        {
            get
            {
                List<uint> list = new List<uint>();
                if (Memory.Read(Address.HaveRide1) != 0)
                    list.Add(Memory.Read(Address.HaveRide1));
                if (Address.GameType == 1)
                {
                    uint offset = 0x0;
                    while (true)
                    {
                        Address.HaveRide2[2] = offset;
                        if (Memory.Read(Address.HaveRide2) != 0)
                            list.Add(Memory.Read(Address.HaveRide2));
                        offset += 4;
                        if (offset > 0xC)
                            break;
                    }
                }
                return list;
            }
        }


        public bool HaveRide
        {
            get
            {
                if (Main.IsNotUseRide)
                    return false;
                if (User.IsEnglish && Lvl >= 20)
                    return true;
                bool isHave = Memory.Read(Address.HaveRide1) != 0;
                if (Address.GameType == 1)
                {
                    uint offset = 0x0;
                    while (!isHave)
                    {
                        Address.HaveRide2[2] = offset;
                        isHave = Memory.Read(Address.HaveRide2) != 0;
                        offset += 4;
                        if (offset > 0xC)
                            break;
                    }
                }
                return isHave;
            }
        }

        public string MenpaiName
        {
            get
            {
                switch (Menpai)
                {
                    case 32: return "Mộ Dung";
                    case 1: return "Thiếu Lâm";
                    case 2: return "Minh Giáo";
                    case 3: return "Cái Bang";
                    case 4: return "Võ Đang";
                    case 5: return "Nga My";
                    case 6: return "Tinh Túc";
                    case 7: return "Thiên Long";
                    case 8: return "Thiên Sơn";
                    case 9: return "Tiêu Dao";
                    case 37: return "Đường Môn";
                    case 53: return "Quỷ Cốc";
                    case 54: return "Đào Hoa";
                    default: return "Không Có";
                }
            }
        }

        public static string GetMenpaiName(string m)
        {
            int men = TDT.ParseAllInt(m);
            switch (men)
            {
                case 32: return "Mộ Dung";
                case 1: return "Thiếu Lâm";
                case 2: return "Minh Giáo";
                case 3: return "Cái Bang";
                case 4: return "Võ Đang";
                case 5: return "Nga My";
                case 6: return "Tinh Túc";
                case 7: return "Thiên Long";
                case 8: return "Thiên Sơn";
                case 9: return "Tiêu Dao";
                case 37: return "Đường Môn";
                case 53: return "Quỷ Cốc";
                case 54: return "Đào Hoa";
                default: return "Không Có";
            }
        }

        public static int GetMenpaiId(string m)
        {
            switch (m)
            {
                case "Mộ Dung": return 32;
                case "Thiếu Lâm": return 1;
                case "Minh Giáo": return 2;
                case "Cái Bang": return 3;
                case "Võ Đang": return 4;
                case "Nga My": return 5;
                case "Tinh Túc": return 6;
                case "Thiên Long": return 7;
                case "Thiên Sơn": return 8;
                case "Tiêu Dao": return 9;
                case "Đường Môn": return 37;
                case "Quỷ Cốc": return 53;
                case "Đào Hoa": return 54;
                default: return -1;
            }
        }


        public static string GetMenpaiName(int men)
        {
            switch (men)
            {
                case 32: return "Mộ Dung";
                case 1: return "Thiếu Lâm";
                case 2: return "Minh Giáo";
                case 3: return "Cái Bang";
                case 4: return "Võ Đang";
                case 5: return "Nga My";
                case 6: return "Tinh Túc";
                case 7: return "Thiên Long";
                case 8: return "Thiên Sơn";
                case 9: return "Tiêu Dao";
                case 37: return "Đường Môn";
                case 53: return "Quỷ Cốc";
                case 54: return "Đào Hoa";
                default: return "Không Có";
            }
        }

        public NPC NPCBaiSu
        {
            get
            {
                if (Menpai == MENPAI.ThieuLam)
                    return THIEULAM.HuyenTich;
                if (Menpai == MENPAI.MinhGiao)
                    return MINHGIAO.LaSuTuong;
                if (Menpai == MENPAI.CaiBang)
                    return CAIBANG.TranCoNhan;
                if (Menpai == MENPAI.VoDang)
                    return VODANG.TruongHuyenTo;
                if (Menpai == MENPAI.NgaMy)
                    return NGAMY.LyThapNhiNuong;
                if (Menpai == MENPAI.TinhTuc)
                    return TINHTUC.HanTheTrung;
                if (Menpai == MENPAI.ThienLong)
                    return THIENLONG.BanNhan;
                if (Menpai == MENPAI.ThienSon)
                    return THIENSON.MaiKiem;
                if (Menpai == MENPAI.TieuDao)
                    return TIEUDAO.ToTinhHa;
                if (Menpai == MENPAI.MoDung)
                    return MODUNG.MoDungKiet;
                if (Menpai == MENPAI.DuongMon)
                    return DUONGMON.DuongXichPhong;
                if (Menpai == MENPAI.QuyCoc)
                    return QUYCOC.VuongThienNhat;
                if (Menpai == MENPAI.DaoHoa)
                    return DAOHOA.QuanHoanChau;
                return null;
            }
        }

        public NPC NPCThuongKho
        {
            get
            {
                if(Address.GameType == 1)
                {
                    if (MapId == DAILY.Id)
                        return DAILY.ThuongKho;
                    if (MapId == TOCHAU.Id)
                        return TOCHAU.LuongHoaKe;
                    if (MapId == LACDUONG.Id)
                        return LACDUONG.ThuongKho;
                    if (MapId == LAULAN.Id)
                        return LAULAN.ThuongKho;
                    if (MapId == THUCHACOTRAN.Id)
                        return THUCHACOTRAN.ThuongKho;
                    if (MapId == PHUNGMINHTRAN.Id)
                        return PHUNGMINHTRAN.ThuongKho;
                    return LACDUONG.ThuongKho;
                }
                else
                {
                    if (MapId == DAILY.Id)
                        return DAILY.ThuongKhoTinhKiem;
                    if (MapId == TOCHAU.Id)
                        return TOCHAU.ThuongKhoTinhKiem;
                    if (MapId == LACDUONG.Id)
                        return LACDUONG.ThuongKhoTinhKiem;
                    if (MapId == LAULAN.Id)
                        return LAULAN.ThuongKho;
                    if (MapId == THUCHACOTRAN.Id)
                        return THUCHACOTRAN.ThuongKhoTinhKiem;
                    if (MapId == PHUNGMINHTRAN.Id)
                        return PHUNGMINHTRAN.ThuongKho;
                    return LACDUONG.ThuongKhoTinhKiem;
                }
            }
        }

        public bool IsBankOpen
        {
            get
            {
                if(Address.GameType == 1)
                {
                    if (Memory.Read(Address.IsBigBankOpen) == 1)
                        return true;
                }
                return Memory.Read(Address.IsBankOpen) == 1;
            }
        }

        public NPC NPCBaiSuDaiLy
        {
            get
            {
                if (Menpai == MENPAI.DuongMon)
                    return DAILY.DuongDuc;
                if (Menpai == MENPAI.MinhGiao)
                    return DAILY.ThachBao;
                if (Menpai == MENPAI.ThienSon)
                    return DAILY.TrinhThanhSuong;
                if (Menpai == MENPAI.TinhTuc)
                    return DAILY.HaiPhongTu;
                if (Menpai == MENPAI.ThienLong)
                    return DAILY.PhaTham;
                if (Menpai == MENPAI.TieuDao)
                    return DAILY.DamDaiTuVu;
                if (Menpai == MENPAI.MoDung)
                    return DAILY.MoDungTruyen;
                if (Menpai == MENPAI.NgaMy)
                    return DAILY.LoTamNuong;
                if (Menpai == MENPAI.CaiBang)
                    return DAILY.GianNinh;
                if (Menpai == MENPAI.ThieuLam)
                    return DAILY.TueDich;
                if (Menpai == MENPAI.VoDang)
                    return DAILY.TruongHoach;
                if (Menpai == MENPAI.QuyCoc)
                    return DAILY.DuongTiem;
                if (Menpai == MENPAI.DaoHoa)
                    return DAILY.HoangThoiVu;
                return null;
            }
        }

        public NPC NPCTamPhap
        {
            get
            {
                if (Menpai == MENPAI.ThieuLam)
                    return THIEULAM.HuyenNan;
                if (Menpai == MENPAI.MinhGiao)
                    return MINHGIAO.BangVanXuan;
                if (Menpai == MENPAI.CaiBang)
                    return CAIBANG.HeTamKi;
                if (Menpai == MENPAI.VoDang)
                    return VODANG.DuVienSon;
                if (Menpai == MENPAI.NgaMy)
                    return NGAMY.ThoiLucHoa;
                if (Menpai == MENPAI.TinhTuc)
                    return TINHTUC.ThiToan;
                if (Menpai == MENPAI.ThienLong)
                    return THIENLONG.BanQuan;
                if (Menpai == MENPAI.ThienSon)
                    return THIENSON.LanKiem;
                if (Menpai == MENPAI.TieuDao)
                    return TIEUDAO.KhangQuangLang;
                if (Menpai == MENPAI.MoDung)
                    return MODUNG.MoDungThanhSon;
                if (Menpai == MENPAI.DuongMon)
                    return DUONGMON.DuongNhacXung;
                if (Menpai == MENPAI.QuyCoc)
                    return QUYCOC.LyKeLong;
                if (Menpai == MENPAI.DaoHoa)
                    return DAOHOA.YenPhan;
                return null;
            }
        }

        public int MapAcBa
        {
            get
            {
                if (Menpai == MENPAI.ThieuLam)
                    return MAP.ThieuLamAcBa;
                if (Menpai == MENPAI.MinhGiao)
                    return MAP.MinhGiaoAcBa;
                if (Menpai == MENPAI.CaiBang)
                    return MAP.CaiBangAcBa;
                if (Menpai == MENPAI.VoDang)
                    return MAP.VoDangAcBa;
                if (Menpai == MENPAI.NgaMy)
                    return MAP.NgaMyAcBa;
                if (Menpai == MENPAI.TinhTuc)
                    return MAP.TinhTucAcBa;
                if (Menpai == MENPAI.ThienLong)
                    return MAP.ThienLongAcBa;
                if (Menpai == MENPAI.ThienSon)
                    return MAP.ThienSonAcBa;
                if (Menpai == MENPAI.TieuDao)
                    return MAP.TieuDaoAcBa;
                if (Menpai == MENPAI.MoDung)
                    return MAP.MoDungAcBa;
                if (Menpai == MENPAI.DuongMon)
                    return MAP.DuongMonAcBa;
                if (Menpai == MENPAI.QuyCoc)
                    return MAP.QuyCocAcBa;
                if (Menpai == MENPAI.DaoHoa)
                    return MAP.DaoHoa;
                return -1;
            }
        }

        public int MapMonPhai
        {
            get
            {
                if (Menpai == MENPAI.ThieuLam)
                    return THIEULAM.Id;
                if (Menpai == MENPAI.MinhGiao)
                    return MINHGIAO.Id;
                if (Menpai == MENPAI.CaiBang)
                    return CAIBANG.Id;
                if (Menpai == MENPAI.VoDang)
                    return VODANG.Id;
                if (Menpai == MENPAI.NgaMy)
                    return NGAMY.Id;
                if (Menpai == MENPAI.TinhTuc)
                    return TINHTUC.Id;
                if (Menpai == MENPAI.ThienLong)
                    return THIENLONG.Id;
                if (Menpai == MENPAI.ThienSon)
                    return THIENSON.Id;
                if (Menpai == MENPAI.TieuDao)
                    return TIEUDAO.Id;
                if (Menpai == MENPAI.MoDung)
                    return MODUNG.Id;
                if (Menpai == MENPAI.DuongMon)
                    return DUONGMON.Id;
                if (Menpai == MENPAI.QuyCoc)
                    return QUYCOC.Id;
                if (Menpai == MENPAI.DaoHoa)
                    return DAOHOA.Id;
                return -1;
            }
        }

        public static bool IsVIPItem(int item)
        {
            if (item == 0)
                return false;
            if (item == 30008034) // kim cuong sa
                return true;
            if (item == 30000000) // bao tang do
                return true;
            if (item >= 20109001 && item <= 20109015) // đục lỗ
                return true;
            if (item == 20109101) // đục lỗ
                return true;
            if (item == 20109102) // đục lỗ
                return true;
            if (item == 30008053) // chưởng cự yếu quyết
                return true;
            if (item == 30103042) // thuốc giải bi tô thanh phong
                return true;
            if (item == 10141153) // thu cuoi 80 
                return true;
            if (item == 10157001 || item == 10157002) //Long van
                return true;
            if (item == 10156001 || item == 10156002 || item == 10156003 || item == 10156004) // võ hồn
                return true;
            if (item.ToString().StartsWith("101"))
                return false;
            if (item.ToString().StartsWith("102"))
                return false;
            if (item.ToString().StartsWith("103"))
                return false;
            if (item.ToString().StartsWith("104"))
                return false;
            if (item.ToString().StartsWith("201"))
                return false;
            if (item.ToString().StartsWith("300"))
                return false;
            if (item.ToString().StartsWith("301"))
                return false;
            return true;
        }

        public TLBB(Game game)
        {
            Game = game;
            Address = game.Address;
            Memory = game.Memory;
            Read();
        }


        public Dictionary<uint, string> DicPet
        {
            get
            {
                Dictionary<uint, string> dic = new Dictionary<uint, string>();
                PetBase = Memory.Read(Address.PetBase);
                for (uint i = 0; i < 10; i++)
                {
                    uint id = Memory.Read(PetBase + Address.PetDataSize * i + Address.PetId);
                    string name = "";                    
                    if(Address.GameType != 1)
                        name = Memory._ReadString(PetBase + Address.PetDataSize * i + 0x1C);
                    else
                        name = Memory._ReadString(PetBase + Address.PetDataSize * i + 0x20);
                    if (id != 0)
                    {
                        dic.Add(id, name);
                    }                  
                }
                return dic;
            }
        }

      



        public uint[] CountTaiNguyen()
        {
            uint cntMam = 0;
            uint cntShit = 0;
            uint cntBut = 0;

            foreach (var item in Game.PacketItems.All.Concat(Game.PacketItems.ThienCo))
            {
                if (item.ClearName.Contains("tienhoachungtu"))
                {
                    cntMam += item.Count;
                }
                if (item.ClearName.Contains("hoaphi"))
                {
                    cntShit += item.Count;
                }
                if (item.ClearName.Contains("chucphucmaobut"))
                {
                    cntBut += item.Count;
                }
            }
            return new uint[] { cntMam, cntShit, cntBut };
        }

      



 

        public int[] CountHoaKTT
        {
            get
            {
                int cnt = 0;
                int cntex = 0;
                foreach (var item in Game.PacketItems.All)
                {
                    if (item.ClearName.Contains("hoahongbatu"))
                    {
                        cnt += (int)item.Count;
                    }
                    if (item.ClearName.Contains("kimtamti"))
                    {
                        cntex += (int)item.Count;
                    }
                }
                return new [] { cnt, cntex };
            }
        }


        public IEnumerable<Pet> Pets
        {
            get
            {
                uint PetBase = Memory.Read(Address.PetBase);
                uint cnt = 0;
                for (uint i = 0; i < 10; i++)
                {

                    uint id = Memory.Read(PetBase + Address.PetDataSize * i + Address.PetId);
                    if (id <= 0)
                        continue;



                    string name = string.Empty;
                    if (Address.GameType == 1)
                        name = Memory._ReadString(PetBase + Address.PetDataSize * i + 0x20).Trim();
                    else
                        name = Memory._ReadString(PetBase + Address.PetDataSize * i + 0x1C).Trim();
                    if (string.IsNullOrEmpty(name))
                        continue;

                    Pet pet = new Pet();
                    pet.idx = (cnt);
                    if (id == Memory.Read(Base + Address.CharCurPetId))
                    {
                        pet.IsFight = true;
                    }
                    cnt++;
                    pet.PetAddress = PetBase + Address.PetDataSize * i;
                    pet.PetId = id.ToString("X8");
                    pet.PetDome = Memory.Read(PetBase + Address.PetDataSize * i + Address.PetEnjoy);
                    pet.Lvl = Memory.Read(PetBase + Address.PetDataSize * i + Address.PetLvl);

                    pet.TuChatNgoaiCong = Memory.Read(PetBase + Address.PetDataSize * i + 0xBC);
                    pet.TuChatNoiCong = Memory.Read(PetBase + Address.PetDataSize * i + 0xC8);
                    pet.TuChatTheLuc = Memory.Read(PetBase + Address.PetDataSize * i + 0xC0);
                    pet.TuChatTriLuc = Memory.Read(PetBase + Address.PetDataSize * i + 0xCC);
                    pet.TuChatThanPhap = Memory.Read(PetBase + Address.PetDataSize * i + 0xC4);
                    pet.NoiCong = Memory.Read(PetBase + Address.PetDataSize * i + 0x6C);
                    pet.NgoaiCong = Memory.Read(PetBase + Address.PetDataSize * i + 0x68);


                    pet.PetName = name;
                    if (TDT.VietLien(pet.PetName) == "thiemdiendieu" || TDT.VietLien(pet.PetName).Contains("nier"))
                    {
                        continue;
                    }
                    yield return pet;
                }
            }
        }

        public Pet MaxPet
        {
            get
            {
                uint PetBase = Memory.Read(Address.PetBase);
                uint cnt = 0;
                Pet maxPet = null;
                uint maxTuChat = 0;
                for (uint i = 0; i < 10; i++)
                {

                    uint id = Memory.Read(PetBase + Address.PetDataSize * i + Address.PetId);
                    if (id <= 0)
                        continue;
                    if (!DicPet.ContainsKey(id))
                        continue;
                    string name = string.Empty;
                    if (Address.GameType == 1)
                        name = Memory._ReadString(PetBase + Address.PetDataSize * i + 0x20).Trim();
                    else
                        name = Memory._ReadString(PetBase + Address.PetDataSize * i + 0x1C).Trim();
                    if (string.IsNullOrEmpty(name))
                        continue;

                    uint dome = Memory.Read(PetBase + Address.PetDataSize * i + Address.PetEnjoy);
                    uint lvl = Memory.Read(PetBase + Address.PetDataSize * i + Address.PetLvl);
                    Pet pet = new Pet();
                    pet.idx = (cnt);
                    cnt++;
                    pet.PetAddress = PetBase + Address.PetDataSize * i;
                    pet.PetId = id.ToString("X8");
                    pet.PetDome = dome;
                    pet.Lvl = lvl;
                    pet.TuChatNgoaiCong = Memory.Read(PetBase + Address.PetDataSize * i + 0xC0);
                    pet.TuChatNoiCong = Memory.Read(PetBase + Address.PetDataSize * i + 0xCC);
                    pet.TuChatTheLuc = Memory.Read(PetBase + Address.PetDataSize * i + 0xC4);
                    pet.TuChatTriLuc = Memory.Read(PetBase + Address.PetDataSize * i + 0xD0);
                    pet.TuChatThanPhap = Memory.Read(PetBase + Address.PetDataSize * i + 0xC8);
                    pet.NgoaiCong = Memory.Read(PetBase + Address.PetDataSize * i + 0x68);
                    pet.NoiCong = Memory.Read(PetBase + Address.PetDataSize * i + 0x6C);


                    pet.PetName = name;
                    if(TDT.VietLien(pet.PetName) == "thiemdiendieu" || TDT.VietLien(pet.PetName).Contains("nier"))
                    {
                        continue;
                    }
                    if(pet.PetDome >= 60)
                    {
                        if(pet.NgoaiCong > maxTuChat)
                        {
                            maxTuChat = pet.NgoaiCong;
                            maxPet = pet;
                        }
                        if(pet.NoiCong > maxTuChat)
                        {
                            maxTuChat = pet.NoiCong;
                            maxPet = pet;
                        }
                    }
                }
                return maxPet;
            }
        }

        public Pet MaxPetDome
        {
            get
            {
                uint PetBase = Memory.Read(Address.PetBase);
                uint cnt = 0;
                Pet maxPet = null;
                uint maxTuChat = 0;
                for (uint i = 0; i < 10; i++)
                {

                    uint id = Memory.Read(PetBase + Address.PetDataSize * i + Address.PetId);
                    if (id == 0 || id == 0xFFFFFFFF)
                        continue;
                    string name = string.Empty;

                    if (Address.GameType == 1)
                        name = Memory._ReadString(PetBase + Address.PetDataSize * i + 0x20).Trim();
                    else
                        name = Memory._ReadString(PetBase + Address.PetDataSize * i + 0x1C).Trim();
              
                    uint dome = Memory.Read(PetBase + Address.PetDataSize * i + Address.PetEnjoy);
                    uint lvl = Memory.Read(PetBase + Address.PetDataSize * i + Address.PetLvl);
                    Pet pet = new Pet();
                    pet.idx = (cnt);
                    cnt++;
                    pet.PetAddress = PetBase + Address.PetDataSize * i;
                    pet.PetId = id.ToString("X8");
                    pet.PetDome = dome;
                    pet.Lvl = lvl;
                    pet.TuChatNgoaiCong = Memory.Read(PetBase + Address.PetDataSize * i + 0xC0);
                    pet.TuChatNoiCong = Memory.Read(PetBase + Address.PetDataSize * i + 0xCC);
                    pet.TuChatTheLuc = Memory.Read(PetBase + Address.PetDataSize * i + 0xC4);
                    pet.TuChatTriLuc = Memory.Read(PetBase + Address.PetDataSize * i + 0xD0);
                    pet.TuChatThanPhap = Memory.Read(PetBase + Address.PetDataSize * i + 0xC8);
                    pet.NgoaiCong = Memory.Read(PetBase + Address.PetDataSize * i + 0x68);
                    pet.NoiCong = Memory.Read(PetBase + Address.PetDataSize * i + 0x6C);
                    pet.PetName = name;
                    if (TDT.VietLien(pet.PetName) == "thiemdiendieu" || TDT.VietLien(pet.PetName).Contains("nier"))
                    {
                        continue;
                    }
                    if (pet.NgoaiCong > maxTuChat)
                    {
                        maxTuChat = pet.NgoaiCong;
                        maxPet = pet;
                    }
                    if (pet.NoiCong > maxTuChat)
                    {
                        maxTuChat = pet.NoiCong;
                        maxPet = pet;
                    }
                }
                return maxPet;
            }
        }

        public uint ServerIndex { get; set; }

        public bool IsShopBachBaoOpen
        {
            get
            {
                return Memory.Read(new uint[] { Game.AddressGameExe + 0x85A730, 0x8, 0x4, 0x144 }) == 1;
            }
        }
        //8595A8
        public bool IsShopTrungDoOpen
        {
            get
            {
                return Memory.Read(new uint[] { Game.AddressGameExe + 0x860EE0, 0x8, 0x4, 0x144 }) == 1;
            }
        }

        public bool IsShopHungBaOpen
        {
            get
            {
                return Memory.Read(new uint[] { Game.AddressGameExe + 0x8595A8, 0x8, 0x4, 0x144 }) == 1;
            }
        }

        public bool IsShopQuyThiOpen
        {
            get
            {
                return Memory.Read(new uint[] { Game.AddressGameExe + 0x860208, 0x8, 0x4, 0x144 }) == 1;
            }
        }


        public void Read()
        {
            ServerIndex = Memory.Read(Game.AddressGameExe + 0xAB7204);
            IsODaoCuFull = isODaoCuFull;
            IsONguyenLieuFull = isONguyenLieuFull;
            Base = Memory.Read(Address.CharBase);
            Menpai = Memory.Read(Base + Address.CharMenpai);
            Lvl = Memory.Read(Base + Address.CharLvl);


            if (Address.GameType == 1)
                Id = Memory.Read8Byte(Base + Address.CharId).ToString("X8");
            else
                Id = Memory.Read(Base + Address.CharId).ToString("X8");
            PlayerState = Memory.Read(Address.CharState);
            MapId = Memory.Read(Address.MapId);
            if (MapId == 242)
                MapId = 0;
            //lecaotri2020    
            MapName = Memory._ReadString(Address.MapName);

            Rage = Memory.Read(Base + Address.CharRage);

            PetId = Memory.Read(Base + Address.CharCurPetId);
            HP = Memory.Read(Base + Address.CharCurHP);
            MaxHP = Memory.Read(Base + Address.CharMaxHP);
            MP = Memory.Read(Base + Address.CharCurMP);
            Exp = Memory.Read(Base + Address.CharExp);
            MaxMP = Memory.Read(Base + Address.CharMaxMP);
            NangDong = Memory.Read(Base + 0x46F8);
            GuildName = Memory.ReadString(Base + Address.CharGuildName);
            GuildId = Memory.Read(Base + Address.CharGuildID);
            PetName = "";
            if (PetId != 0)
            {
                PetBase = Memory.Read(Address.PetBase);
                for (uint i = 0; i < 10; i++)
                {
                    if (Memory.Read(PetBase + Address.PetDataSize * i + Address.PetId) == PetId)
                    {
                        PetHP = Memory.Read(PetBase + Address.PetDataSize * i + Address.PetCurHP);
                        PetMaxHP = Memory.Read(PetBase + Address.PetDataSize * i + Address.PetMaxHP);
                        PetEnjoy = Memory.Read(PetBase + Address.PetDataSize * i + Address.PetEnjoy);
                        PetLvl = Memory.Read(PetBase + Address.PetDataSize * i + Address.PetLvl);
                        if (Address.GameType == 1)
                            PetName = Memory._ReadString(PetBase + Address.PetDataSize * i + 0x20).Trim();
                        else
                            PetName = Memory._ReadString(PetBase + Address.PetDataSize * i + 0x1C).Trim();
                        break;
                    }
                }
            }
            else
            {
                PetHP = PetMaxHP = PetEnjoy = 0;
            }
            bool prewCap = IsCaptcha;
            if (IsCaptcha && string.IsNullOrEmpty(BinEx))
            {
                if (Game.swCaptchaTime == null)
                    Game.swCaptchaTime = Stopwatch.StartNew();
                if (Game.swCaptchaTime.Elapsed.TotalSeconds >= 6 && !IsRead)
                {
                    IsRead = true;
                    new Thread(new ThreadStart(() =>
                    {
                        Thread.CurrentThread.IsBackground = true;
                        ReadCaptcha();

                    })).Start();
                    //ReadCaptcha();
                }
            }
            if (!IsCaptcha)
            {
                IsRead = false;
                BinEx = null;
            }
            IsPk = Memory.Read(Address.IsPK) == 1;
            //LECAOTRI2020SUA
            //IsPk = false;





            if (Address.GameType == 1)
                KeyId = Memory.Read8Byte(Address.KeyId).ToString("X8");
            else
                KeyId = Memory.Read(Address.KeyId).ToString("X8");
            if (Game.QuanDoanParty1.Contains(Game))
            {
                if (Game.QuanDoanParty1.Count > 0)
                {
                    KeyId = Game.QuanDoanParty1[0].TLBB.Id;
                }
            }
            if (Game.QuanDoanParty2.Contains(Game))
            {
                if (Game.QuanDoanParty2.Count > 0)
                {
                    KeyId = Game.QuanDoanParty2[0].TLBB.Id;
                }
            }
            if (Address.GameType == 1)
                Address.SkillPetBase[1] = skillpetbase;

            PetHaveSkill1 = Memory.Read(Address.SkillPetBase[0], skillpetbase);
            PetHaveSkill2 = Memory.Read(Address.SkillPetBase[0], skillpetbase + 4);

            if ((PetHaveSkill2 - PetHaveSkill1) == 4)
            {
                PetHaveSkill = 1;
                SkillPetType1 = Memory._ReadString(Memory.Read(Address.SkillPetBase) + 0x0, 0x20);
                SkillPetType2 = "";
            }
            else if ((PetHaveSkill2 - PetHaveSkill1) == 8)
            {
                PetHaveSkill = 2;
                SkillPetType2 = Memory._ReadString(Memory.Read(Address.SkillPetBase) + 0x4, 0x20);
                SkillPetType1 = Memory._ReadString(Memory.Read(Address.SkillPetBase) + 0x0, 0x20);
            }
            else
            {
                PetHaveSkill = 0;
                SkillPetType1 = "";
                SkillPetType2 = "";
            }


            SkillPetType = Memory._ReadString(Memory.Read(Address.SkillPetBase) + 0x0, 0x20) + Memory._ReadString(Memory.Read(Address.SkillPetBase) + 0x4, 0x20);


            if (Address.GameType != 1)
            {
                SkillPetType1 = Memory._ReadString(Memory.Read(Address.SkillPetBase) + 0x0, 0x28);
                SkillPetType2 = Memory._ReadString(Memory.Read(Address.SkillPetBase) + 0x4, 0x28);
            }
            SkillPetType = SkillPetType1 + SkillPetType2;

            OnlineTime = Memory.Read(Address.OnlineTime) / 60000;
            OnlineTimeSec = Memory.Read(Address.OnlineTime) / 1000;
            DelayBase = Memory.Read(Address.SkillDelayBase);///kt
            IsBienThan = false;
            if (Game.Objects.Self != null)
            {
                if (Game.Objects.Self.Buff.Contains(1665) || Game.Objects.Self.Buff.Contains(2746) || Game.Objects.Self.Buff.Contains(2209))
                    IsBienThan = true;
            }
            if (!IsBienThan)
            {
                if (Address.GameType == 1)
                {
                    IsBienThan = Memory.Read(Base + 0xA0) != 0xFFFFFFFF;
                }
                else
                {
                    IsBienThan = Memory.Read(Address.LuyenKimBase, Address.BienThan) == 1;
                }
            }
            IsRide =  Memory.Read(Base + Address.ObjectRide) != 0xFFFFFFFF;
        }

        public static uint skillpetbase { get; set; } = 0x44;

        public bool IsRead { get; set; }
        public string PetName
        {
            get; set;
        }
        public bool IsReadCaptcha = false;

        public bool HaveFood
        {
            get
            {
                foreach(var item in Game.PacketItems.All)
                {
                    if (GAMEDIC.YuanBaoFood.ContainsKey(item.Type))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

   
     

        public bool IsFly { get; set; }

        public void Fly(string point)
        {
            int x = TDT.ParseInt(point);
            int y = TDT.ParseInt(point.Replace(x + ",", ""));
            Fly(x, y);
        }

        public void Fly(float x, float y)
        {
            //Game.GoToPoint.Item6 = Stopwatch.StartNew();
            if (IsRide)
            {
                Game.DownRide();                
            }
            foreach(Skill skill in Game.Skills)
            {
                if (string.IsNullOrEmpty(skill.Type))
                    continue;
                if (skill.Type.Contains("Shoes2"))
                {
                    Game.PostMessage(ConverterEx.Float2Int(x), 59);
                    Game.PostMessage(ConverterEx.Float2Int(y), 60);
                    Game.PostMessage(skill.PacketId, 61);
                    Game.PostMessage(0, 126); 
                   
                    break;
                }
            }
        }


        public void MoveEx(float x, float y)
        {
            Game.PostMessage(Memory.Float2Int(x), 50);
            Game.PostMessage(Memory.Float2Int(y), 51);
            Game.PostMessage(0, 119);
        }



        public string MineIds = "";

        public bool IsDanhBinhThanh
        {
            get
            {
                foreach(GameObject obj in Game.Objects.Monters)
                {
                    if(obj.Name =="Tiêu Như Úy" || obj.Name == "Tiêu Như Quân")
                    {
                        if (Game.TargetId == obj.Id)
                            return true;
                    }
                }
                return false;
            }
        }

      

       

        //public List<GameObject> ToanPhong
        //{
        //    get
        //    {
        //        return Monters.Cast<GameObject>().Where(_object => _object.CleanName == "toanphong").OrderBy(_object => _object.Id).ToList<GameObject>();
        //    }
        //}

        //public List<GameObject> PhongLoiDan
        //{
        //    get
        //    {
        //        return Monters.Cast<GameObject>().Where(_object => _object.CleanName == "phongloidan").OrderBy(_object => _object.Id).ToList<GameObject>();
        //    }
        //}

  


        public bool ON_SCENE_TRANSING
        {
            get
            {
                return Memory.Read(Address.ON_SCENE_TRANSING) == 1;
            }
        }

        public uint ON_Skill_NV
        {
            get
            {
                return Memory.Read(Address.Skill_Nv, 0xC4);
            }
        }
        public bool IsThanhThi
        {
            get
            {
                if (MapId == MAP.LacDuong)
                    return true;
                if (MapId == MAP.ToChau)
                    return true;
                if (MapId == MAP.DaiLy)
                    return true;
                if (MapId == MAP.LauLan)
                    return true;
                if (MapId == PHUNGMINHTRAN.Id)
                    return true;
                return false;
            }
        }
    }
}
