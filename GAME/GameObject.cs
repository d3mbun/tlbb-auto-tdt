using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.Linq;

namespace _i
{
    class GameObject
    {
        public Game Game { get; set; }
        Memory Memory { get; set; }
        Address ADDRESS { get; set; }
        public uint Address { get; set; }
        public uint Id { get; set; }
        public uint Object { get; set; }
        public uint Class { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public List<uint> buff;


        public List<uint> Buff
        {
            get
            {
                if (buff == null)
                {
                    buff = new List<uint>();
                    foreach (uint address in BuffAddress)
                        buff.Add(Game.Memory.Read(address + 0x10));
                }
                return buff;
            }
        }


        public uint State { get; set; }

   

        public uint AtkToId { get; set; }
        public uint AtkById { get; set; }
        public uint InfoAddress { get; set; }
        public float HP { get; set; }
        public float MP { get; set; }
        public string TrueId { get; set; }
        public string Belong { get; set; }
        public string Name { get; set; } 
        public byte[] VISCIIName { get; set; }

        string cleanname = null;

        public string CleanName
        {
            get
            {
                if (cleanname == null)
                    cleanname = TDT.VietLien(Name);
                return cleanname;
            }
        }
        public uint Menpai { get; set; }
        public string Type { get; set; }
        public uint Lvl { get; set; }
        public uint PartyId { get; set; }
        public string Title { get; set; }
        public uint Ride { get; set; }
        public uint QDId { get; set; }


        public float Distance => TDT.GetDistance(Game.CharX, Game.CharY, X, Y);
        public float DistanceEx { get; set; }

        public int RoundX
        {
            get
            {
                if (roundx != -1)
                    return roundx;
                return (int)Math.Round(X, 0, MidpointRounding.AwayFromZero);
            }
            set
            {
                roundx = value;
            }
        }

        int roundy = -1;
        int roundx = -1;

        public int RoundY
        {
            get
            {
                if (roundy != -1)
                    return roundy;
                return (int)Math.Round(Y, 0, MidpointRounding.AwayFromZero);
            }
            set
            {
                roundy = value;
            }
        }

        public string Point => RoundX + "," + RoundY;

        public bool IsTaiNguyenEx
        {
            get
            {
                if (Main.SettingForm.checkSkipMonter.Checked && SettingOld.HaveToBoQua.Contains(Name))
                {
                    return false;
                }
                if (!Global.IsAdminEx)
                {
                    if (Game.TLBB.MapId == MAP.DaiLy)
                    {
                        if (!Global.IsChongKsBong)
                            return false;
                        if (TDT.GetDistance(RoundX, RoundY, Global.ChongX, Global.ChongY) > 20)
                            return false;
                    }
                }
                if (Class == Game.Address.TaiNguyenClass)
                    return true;


                //if (Class == Game.Address.TaiNguyenNVClass)
                //    return true;

                return false;
            }
        }

        public bool IsTaiNguyen
        {
            get
            {
                if (Main.SettingForm.checkSkipMonter.Checked && SettingOld.HaveToBoQua.Contains(Name))
                {
                    return false;
                }
                if (Class == Game.Address.TaiNguyenClass)
                    return true;


                //if (Class == Game.Address.TaiNguyenNVClass)
                //    return true;

                return false;
            }
        }

        public bool IsKhoang
        {
            get
            {
                if (SettingOld.HaveToBoQua.Contains(Name))
                {
                    return false;
                }
                if (!IsTaiNguyen)
                    return false;
                if (TDT.IsKhoang(Name) == -1 && !Game.Is69DO)
                    return false;
                return true;
            }
        }

        public string MD
        {
            get
            {
                return TDT.Hasher.MD5(Name).Substring(0, 3);
            }
        }

        public bool IsDuoc
        {
            get
            {
                if (SettingOld.HaveToBoQua.Contains(Name))
                {
                    return false;
                }
                if (!IsTaiNguyen)
                    return false;
                if (TDT.IsDuoc(Name) == -1 && !Game.Is69DO)
                    return false;
                return true;
            }
        }

        public bool IsSelf
        {
            get
            {
                if (Id == Game.SelfId)
                    return true;
                return false;
            }
        }

        public bool IsLootPacket => Class == Game.Address.PacketClass;


        public bool GetIsMonter()
        {
            if (HP <= 0)
                return false;
            if (GAMEDIC.BoQua.Contains(Name))
                return false;

            if(Game.RadiusX > 0 && Game.TLBB.MapId == Game.RadiusMap)
            {
                int radius = Global.NgoaiRadius;
                if (Game.TLBB.IsNoiCong)
                    radius = Global.NoiRadius;
                if (GetDistance(Game.RadiusX, Game.RadiusY) > radius)
                    return false;
            }

            if (Game.TLBB.MapId == MAP.BinhThanhKyTran || Game.TLBB.MapId == MAP.TamTaiHiepCoc)
            {
                if (Distance > 20)
                    return false;
            }

            if (Game.Missions.Contains(MissionsType.TrangSucCuuLe))
            {
                if (Name != "Cửu Lê Chiến Sĩ")
                    return false;
            }

            if (Game.TLBB.MapId == MAP.ThieuThatSon)
            {
                if (Game.RoundY > 210)
                    return false;
                if (Setting.Is("checkNeBinh"))
                {
                    if (Name == "Huyết chú vu cổ")
                    {
                        return false;
                    }
                }
                if (Name == "Tinh Túc Đệ Tử" || Name == "Tinh túc môn đồ")
                {
                    if (Distance > 12)
                    {
                        return false;
                    }
                }
            }
            if (Game.TLBB.MapId == MAP.TuTuyetTrang)
            {
                if (Name == "Tinh La Võ Sĩ")
                    return true;
            }

            if (Game.ApTieu != 0)
                return false;
            if (Main.SettingForm.checkSkipMonter.Checked && SettingOld.HaveToBoQua.Contains(Name))
            {
                return false;
            }

            if (Title.Contains("#{PTFB"))
            {
                if (State == 2 || State == 7)
                    return true;
                if (State == 0)
                    return false;
            }

            if (Game.Missions.Contains(MissionsType.DatDoiBinhThanh))
            {
                List<string> list = new List<string>()
                        {
                            "Trúc Đồng Tâm-Trắng",
                            "Trúc Đồng Tâm-Lục",
                            "Trúc Đồng Tâm-Tím",
                               "Trúc Đồng Tâm-Vàng",
                            "Trúc Đồng Tâm-Đỏ",
                            "Trúc Đồng Tâm-Lam",
                            "Phong Lôi Đàn"
                        };
                if (list.Contains(Name))
                {
                    return false;
                }
            }




            if (Game.TLBB.Lvl < 10 && Title.Trim() != "" && Game.Address.GameType == 1)
            {
                return false;
            }
      
            var leader = Game.Leader;
            if (leader != null && leader.Missions.Contains(MissionsType.DatDoiBaoDoHiem))
            {
                if (Name != "Đoạt Bảo Mã Tặc Đầu Lĩnh")
                    return false;
            }

            if (Menpai == 0xFFFFFFFF)
                return false;

            if (leader != null && leader.Missions.Contains(MissionsType.DatDoiMaTac))
            {
                if (Name != "Đoạt Bảo Mã Tặc")
                    return false;
            }
            if (leader != null && leader.Missions.Contains(MissionsType.DatDoiBossMap))
            {
                if (!SettingOld.HashOnlyBossMap.Contains(Name))
                    return false;
            }



            if (Name == "Rương bảo vật")
                return false;
            if (Name == "Tình Báo Thám Tử" || Name == "Mật Tín Thám Tử")
                return false;
            if (Name == "Tuần La Sĩ Binh" || Name.Contains(" Môn "))
                return true;
            if (Game.TLBB.MapId == MAP.CoDongTuocDai)
            {
                if (Name.Contains(" "))
                    return true;
            }
            if (Game.TLBB.MapId <= 2)
                return false;
            if (Game.TLBB.MapId == MAP.BienGioiTongLieu && !Game.IsXong50NguyTongBinh && TDT.VietLien(Name) == "nguytongquandothong")
                return false;
            if (Game.TLBB.MapId == MAP.TrucLam && !Game.IsXong50NguyTongBinh && TDT.VietLien(Name) == "honghungvuong")
                return false;
            if (Game.Missions.Contains(MissionsType.NhiemVuSuMon))
            {
                if (CleanName == "hodaothanthu")
                    return false;
            }
            if (Game.TLBB.MapId == MAP.LanHoanPhucDia)
            {
                if (Menpai == 16)
                    return false;
                if (Menpai == 12)
                    return false;
                if (Menpai == 53)
                    return false;
                if (Menpai == 54)
                    return false;
                if (Menpai > 37)
                    return true;
            }
            //if (Type == "40600000" && TrueId.Contains("FFFFFFFFFFFFFFFF") && Menpai != -1)
            //    return true;
            if (HP <= 0 || (Menpai >= 0 && Menpai < 16) || Menpai == 32 || Menpai == 21 || Menpai == 22 || Menpai == 19 || Menpai == 37 || Menpai > 40)
                return false;

            if (Game.TLBB.MapId == MAP.YenTuO)
            {
                if (Name.Contains("Tháp"))
                    return false;
                if (Name.Contains("Tower"))
                    return false;
            }
            if (TDT.VietLien(Name).Contains("tieulang"))
                return false;
            if (Name.Contains("Niên Thú") && Game.TLBB.MapId != 547)
                return false;
            if (Main.IsOnlyPlayer)
                return false;
            if (Game.IsOnlyAttack)
            {
                if (SettingOld.HaveToOnlyAttack.Contains(Name))
                    return true;
                return false;
            }
            return true;
        }

        public bool IsMonter { get; set; }

        public bool IsPlayer
        {
            get
            {
                if (Menpai >= 1 && Menpai <= 9)
                    return true;
                if (Menpai == 32 || Menpai == 37 || Menpai == 53 || Menpai == 54)
                    return true;
                return false;
            }
        }

        public bool IsPet
        {
            get
            {
                if (Type == "40600000" && TrueId.Contains("FFFFFFFF") && Menpai == 0xFFFFFFFF)
                    return true;
                return false;
            }
        }



        public bool IsNPC
        {
            get
            {
                if (Type == "3FE66666" || Type == "3F4CCCCD")
                    return true;
                if (Name == "Trương Sĩ Tâm" || Name.Trim() == "Khô Vinh Đại Sư")
                    return true;
                return false;
            }
        }

        public int GuildId { get; set; }
        public string GuildName { get; set; }
        public HashSet<uint> BuffAddress
        {
            get
            {
                return EnumGameBuff(Memory.Read(Object + Game.Address.ObjectBuff));
            }
        }

        public bool IsTrap { get; set; }

        public Stopwatch SwXuatHien { get; set; } = Stopwatch.StartNew();

        uint ADD { get; set; }

        public GameObject(Game game, uint address)
        {
            Game = game;
            Memory = game.Memory;
            this.ADDRESS = game.Address;
            ADD = address;
            Id = Memory.Read(address + ADDRESS.ObjectId);
        }



        public bool IsKs
        {
            get
            {
                if (string.IsNullOrEmpty(Name))
                    return false;
                if (State == 5 && SettingOld.IdBienThan.Contains(Name))
                    return true;
                return false;
            }
        }

        public GameObject ReadObj()
        {
            uint address = ADD;
            Game game = Game;

       
            Address = address;
            InfoAddress = (uint)game.Memory.Read(address + ADDRESS.ObjectObject, ADDRESS.ObjectInfo);

            Object = Memory.Read(address + ADDRESS.ObjectObject);
            Class = Memory.Read(Object);
            if (ADDRESS.GameType == 1)
            {
                if (IsTaiNguyen || (Class == 0x935540 || Class == 0x935570 || Class == 0x913550 || Class == ADDRESS.PacketClass || Class == (Game.AddressGameExe + 0x829E9C) || Class == (Game.AddressGameExe + 0x7552A4)) || Class == (Game.AddressGameExe + 0x7552B4))//tui do-0x913BC0 --0x913D68‬ do trang tri --911F88 bay ADDRESS.PacketClass//7552B4
                {
                    X = Memory.ReadFloat(Object + 0x2C);
                    Y = Memory.ReadFloat(Object + 0x34);
                }
                else
                {
                    X = Memory.ReadFloat(Object + ADDRESS.ObjectX);
                    Y = Memory.ReadFloat(Object + ADDRESS.ObjectY);
                }
            }
            else
            {
                X = Memory.ReadFloat(Object + ADDRESS.ObjectX);
                Y = Memory.ReadFloat(Object + ADDRESS.ObjectY);
            }
            if (Id >= 0 && (int)X > 0 && (int)Y > 0)
            //if(Id >= 0)
            {
                if (ADDRESS.GameType == 1)
                    State = Memory.Read(Object + 0x1A8);
                else
                    State = Memory.Read(Object + 0x158);




                AtkToId = Memory.Read(Object + ADDRESS.ObjectAtkToId);
                AtkById = Memory.Read(Object + ADDRESS.ObjectAtkById);
                byte[] buff = new byte[ADDRESS.ObjectPartyId + 0x4];
                Memory.ReadProcessMemory(game.Memory.Id, InfoAddress, buff, buff.Length, 0);
                HP = BitConverter.ToSingle(buff, (int)ADDRESS.ObjectHP);
                MP = BitConverter.ToSingle(buff, (int)ADDRESS.ObjectMP);
                if (ADDRESS.GameType == 1)
                    TrueId = BitConverter.ToInt64(buff, (int)ADDRESS.ObjectTrueId).ToString("X8");
                else
                    TrueId = BitConverter.ToInt32(buff, (int)ADDRESS.ObjectTrueId).ToString("X8");
                if (ADDRESS.GameType == 1)
                    Belong = BitConverter.ToInt64(buff, (int)ADDRESS.ObjectBelong).ToString("X8");
                else
                    Belong = BitConverter.ToInt32(buff, (int)ADDRESS.ObjectBelong).ToString("X8");
                Menpai = (uint)BitConverter.ToInt32(buff, (int)ADDRESS.ObjectMenpai);
                Type = BitConverter.ToInt32(buff, (int)ADDRESS.ObjectType).ToString("X8");
                Lvl = (uint)BitConverter.ToInt32(buff, (int)ADDRESS.ObjectLvl);
                PartyId = (uint)BitConverter.ToInt32(buff, (int)ADDRESS.ObjectPartyId);
                Title = Memory._ReadString(InfoAddress + ADDRESS.ObjectTitle).Trim();
                Ride = (uint)BitConverter.ToInt32(buff, (int)ADDRESS.ObjectRide);
                uint offTaiNguyen = ADDRESS.ObjectTaiNguyenName;
                if (Class == 0x935540 || Class == 0x935570 || Class == 0x913550 || game.Address.GameType == 1 || Class == ADDRESS.PacketClass || Class == 0x913D68)
                {
                    offTaiNguyen = 0xFC + 4;
                }


                if (Name == null)
                {
                    if (IsTaiNguyen || (Class == 0x935540 || Class == 0x935570 || Class == 0x913550 || Class == ADDRESS.PacketClass || Class == (Game.AddressGameExe + 0x829E9C) || Class == (Game.AddressGameExe + 0x7552A4) || Class == (Game.AddressGameExe + 0x7552B4)))//tui do-0x913BC0 --0x913D68‬ do trang tri --911F88 bay ADDRESS.PacketClass
                    {
                        if (Class == (Game.AddressGameExe + 0x7552A4) || Class == (Game.AddressGameExe + 0x7552B4))
                        {
                            offTaiNguyen = 0x104;
                            IsTrap = true;
                        }
                        Name = game.Memory.ReadString(new uint[] { Object + offTaiNguyen, 4, 0 }).Trim();
                    }
                    else
                    {
                        Name = Memory._ReadString(InfoAddress + ADDRESS.ObjectName).Trim();
                        if (string.IsNullOrEmpty(Name.Trim()))
                        {
                            Name = Memory._ReadString(InfoAddress + ADDRESS.ObjectName, true).Trim();
                        }
                    }
                }                
                QDId = Memory.Read2Byte(InfoAddress + 0x2BC0);
                GuildId = (int)Memory.Read(InfoAddress + ADDRESS.CharGuildID);
                GuildName = Memory.ReadString(InfoAddress + ADDRESS.CharGuildName);

                if (Name.Contains("Công Tôn Thắng") || Name == "Định Hải Thần Châm phòng thủ" || Name == "Tru Tiên Trận Phòng" || Name == "Đào Hoa Trận Phòng" || Name == "Bát Trận Đồ Phòng" || Name == "Bộ Bộ Sinh Hoa")
                {
                    IsTrap = false;
                }
                if (GAMEDIC.Trap.Contains(Name) || Name.Contains("Thập Bộ Nhất Sát"))
                    IsTrap = true;
                IsMonter = GetIsMonter();
            }
         

            return this;
        }


        public float GetDistance(string from)
        {
            float fromX = 0;
            float fromY = 0;
            if (from.Split(',').Length >= 2)
            {
                fromX = TDT.ParseInt(from.Split(',')[0]);
                fromY = TDT.ParseInt(from.Split(',')[1]);
            }
            return (float)Math.Sqrt(Math.Pow(fromX - X, 2) + Math.Pow(fromY - Y, 2));
        }

        public float GetDistance(float x, float y) => (float)Math.Sqrt(Math.Pow(x - X, 2) + Math.Pow(y - Y, 2));

        public HashSet<uint> EnumGameBuff(uint address)
        {
            HashSet<uint> hash = new HashSet<uint>();
            NextGameBuff(address, hash);
            hash.Remove(address);
            return hash;
        }


        public static Dictionary<string, int> HoaTime = new Dictionary<string, int>();
        public static HashSet<int> CheckedHoa = new HashSet<int>();

        public bool IsChecked
        {
            get
            {
                if (CheckedHoa.Contains((int)Id))
                    return true;
                return false;
            }
        }




        public double TimeLeft
        {
            get
            {
                string point = RoundX + "," + RoundY;
                if (!HoaTime.ContainsKey(point))
                {
                    HoaTime.Add(point, TDT.SeconNow);
                }
                return HoaTime[point];
            }
        }

        public int TimeXuatHien
        {
            get
            {
                string point = RoundX + "," + RoundY;
                if (!HoaTime.ContainsKey(point))
                {
                    return 0;
                }
                return TDT.SeconNow - HoaTime[point];
            }

        }

        //public DateTime Time { get; set; }

        private void NextGameBuff(uint address, HashSet<uint> hash)
        {
            if (address <= 0)
                return;
            if (hash.Count > 200)
                return;
            if (!hash.Contains(address))
            {
                hash.Add(address);
                //int address1 = Game.Memory.Read(address + 0x0);
                //if (address1 > 0)
                NextGameBuff(Game.Memory.Read(address + 0x0), hash);
                //int address2 = Game.Memory.Read(address + 0x4);
                //if (address2 > 0)
                NextGameBuff(Game.Memory.Read(address + 0x4), hash);
                //int address3 = Game.Memory.Read(address + 0x8);
                //if (address3 > 0)
                NextGameBuff(Game.Memory.Read(address + 0x8), hash);
            }
        }
    }
}
