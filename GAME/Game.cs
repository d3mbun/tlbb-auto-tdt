using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _i
{
    internal partial class Game
    {
        public Dictionary<string, Stopwatch> DeadObjects { get; set; } = new Dictionary<string, Stopwatch>();


        private Stopwatch swThoLinhChau = Stopwatch.StartNew();

        public bool IsAuto { get; set; }
        public int ThuThapIdx { get; set; } = 1;
        public Win.RECT RECT;
        public int Width => RECT.Right - RECT.Left;
        public int Height => RECT.Bottom - RECT.Top;
        public int Left => RECT.Left;
        public int Right => RECT.Right;

        public static Dictionary<string, FastTask> FastTasks { get; set; } = new Dictionary<string, FastTask>();

        public bool IsSumonEx
        {
            get
            {
                if (Missions.Contains(MissionsType.NhiemVuSuMon))
                {
                    if (swHuyQ != null)
                    {
                        if (swHuyQ.Elapsed.TotalMinutes < 5)
                        {
                            PushDebugMessageEx("Sẽ tiếp tục làm nv sư môn sau " + (300 - (int)swHuyQ.Elapsed.TotalSeconds) + "s");
                            return false;
                        }
                    }
                }
                return Missions.Contains(MissionsType.NhiemVuSuMon);
            }
        }



        public bool IsNhatHop { get; set; }
        public bool IsNhatTuyet { get; set; }
        public bool IsThuHoach { get; set; }
        public int Q12TK { get; set; }
        public bool IsThuyLao { get; set; }



        public static int LastMapMaTac = -1;

        public static object SynQuyCoc = new object();
        public static Stopwatch QuyCocTime = Stopwatch.StartNew();

        private bool NgamyQuyCoc()
        {
            if (IsQuyCoc && TLBB.MapId > 2 && TLBB.Menpai == MENPAI.QuyCoc && !TLBB.IsRide)
            {
                lock (SynQuyCoc)
                {
                    bool haveCoc = Objects.All.Where(o => o.Title.Contains("Trường Sinh Tinh")).Count() > 0;
                    if (!haveCoc && QuyCocTime.Elapsed.TotalSeconds > 3 && !TLBB.BusyEx && !TLBB.IsBienThan && TLBB.PlayerState != 9 && TLBB.MPPercent > 5)
                    {
                        if (QuyCocSkill == null)
                        {
                            foreach (Skill skill in Skills)
                            {
                                if (skill.PacketId == 3438)
                                {
                                    QuyCocSkill = skill;
                                }
                            }
                        }
                        else
                        {
                            foreach (Game game in Party)
                            {
                                if (game.TLBB.HPPercent <= Main.QuyCocPercent)
                                {
                                    uint delay = 9999;
                                    if (QuyCocSkill.DelayOffset.ToString().Trim('0').Length < 8)
                                    {
                                        delay = Memory.Read(TLBB.DelayBase + QuyCocSkill.DelayOffset);
                                    }
                                    if (delay == 0 || delay == 0xFFFFFFFF || (delay == 9999))
                                    {
                                        UseSkill((int)QuyCocSkill.PacketId);
                                        QuyCocTime = Stopwatch.StartNew();
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            if(TLBB.MapId == MAP.LanHoanPhucDia)
            {
                if(Objects.All.Any(o => o.Name == "Kích Sát Phá Trận"))
                {
                    if (Objects.All.Any(o => o.Name == "Kích Sát Phá Trận" && o.Distance < 3))
                        return false;
                }
            }
            if (NeBayTime.Elapsed.TotalSeconds >= 2 && TLBB.Menpai == 5 && !TLBB.BusyEx && !TLBB.IsBienThan && TLBB.PlayerState != 9 && TLBB.MPPercent > 5)
            {
                if (TLBB.PlayerState != 2 && TLBB.PlayerState != 5 && !TLBB.IsRide && !TLBB.IsFollow)
                {
                    if (IsNM)
                    {
                        CareObject = null;
                        uint delay = 0xFFFFFFFF;
                        if (NgamySkill != null && NgamySkill.DelayOffset.ToString().Trim('0').Length < 8)
                        {
                            delay = Memory.Read(TLBB.DelayBase + NgamySkill.DelayOffset);
                        }
                        if (delay == 0 || delay == 0xFFFFFFFF)
                        {
                            if (Objects.PartyMinHP != null && DiaPhuNguYeu.Elapsed.TotalSeconds > 3)
                            {
                                if (Global.NMSkill == Keys.F13 && NgamySkill != null)
                                {
                                    UseSkill((int)NgamySkill.PacketId, Objects.PartyMinHP.Id);
                                    TargetId = (int)Objects.PartyMinHP.Id;
                                    Thread.Sleep(1000);
                                }
                                else
                                {

                                    SelectTarget(Objects.PartyMinHP.Id);
                                    SendKey(Global.NMSkill);
                                    Thread.Sleep(1000);
                                }                               
                                CareObject = Objects.PartyMinHP;
                                SwBuffNM = Stopwatch.StartNew();
                                BuffNMPercent = Objects.PartyMinHP.HP;
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        private float BuffNMPercent;

        private bool HoiSinh()
        {
            if (TLBB.Menpai != MENPAI.NgaMy || TLBB.PlayerState == 9)
                return false;
            if (TLBB.MapId == MAP.CoDongTuocDai || TLBB.MapId == MAP.ThienNhanHoangSon || TLBB.MapId == MAP.LangVanDaiPhat)
            {
                return false;
            }
            if (!Objects.Monters.Any(o => o.Distance < 18))
            {
                if (Setting.Is("checkNgamyHoiSinh") || IsMapPhuBan())
                {
                    GameObject deadObj = Objects.PartyEx.Where(o => o.State == 9).OrderByDescending(o => !AllPartyTargetId.Contains(o.Id)).ThenByDescending(o => o.Menpai == MENPAI.NgaMy).ThenBy(o => o.Distance).FirstOrDefault();
                    if (deadObj != null)
                    {
                        TargetId = (int)deadObj.Id;
                        if (GoToEx(deadObj.X, deadObj.Y))
                        {
                            DownRide();
                            UseSkill(408, (int)deadObj.Id);
                            Thread.Sleep(100);
                        }
                        SWHoiSinh = Stopwatch.StartNew();
                        return true;
                    }
                }
            }
            return false;
        }

        public void AutoCreateTeam()
        {
            if ((!Missions.Contains(MissionsType.NhanBong) || IsByLogin))
            {
                bool IsCreate = false;
                if (IsOneSec && TLBB.OnlineTimeSec > 3 && TLBB.Online && Objects.Self != null)
                {
                    if (Main.IsCreateamTyVo)
                    {
                        if (TLBB.PartyId == 0xFFFFFFFF)
                        {
                            foreach (KeyValuePair<string, string> kvp in CalendarEx.TyVos)
                            {
                                if (kvp.Value.StartsWith("[" + TLBB.Name + "]"))
                                {
                                    DoStringEx("Player:CreateTeamSelf();");
                                    IsCreate = true;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            if (TLBB.IsLeader)
                            {
                                foreach (KeyValuePair<string, string> kvp in CalendarEx.TyVos)
                                {
                                    if (kvp.Value.StartsWith("[" + TLBB.Name + "]"))
                                    {
                                        foreach (KeyValuePair<int, Game> kv in Main.DicGame)
                                        {
                                            if (kv.Value.TLBB.PartyId == 0xFFFFFFFF)
                                            {
                                                if (kvp.Value.Contains("[" + kv.Value.TLBB.Name + "]") && !kvp.Value.StartsWith("[" + kv.Value.TLBB.Name + "]"))
                                                {
                                                    kv.Value.AskTeam(TLBB.VISCIIName);
                                                }
                                            }
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    else if (!Missions.Contains(MissionsType.DatDoiBinhThanh) && CalendarEx.IsAutoCreaTeam && QuanDoanParty1.Count + QuanDoanParty2.Count == 0)
                    {
                        if (Objects.Self.PartyId == 0xFFFFFFFF)
                        {
                            foreach(var game in Main.Instance.AllOnelineGame.Where(game => game.TLBB.PartyId != 0xFFFFFFFF))
                            {
                                foreach(var k in Setting.Teams.Values)
                                {
                                    if(k.Contains(game.TLBB.Name) && k.Contains(TLBB.Name))
                                    {
                                        if (!IsKhacTheGioi(game.TLBB))
                                        {
                                            AskTeam(game.TLBB.VISCIIName);                                            
                                        }
                                        return;
                                    }
                                }
                            }

                            if (Setting.TeamLeader.Contains(TLBB.Name))
                            {
                                DoStringEx("Player:CreateTeamSelf();");
                                return;
                            }
                        }
                        else
                        {
                            if (TLBB.IsLeader)
                            {
                                if (!Missions.Contains(MissionsType.DatDoiBaoDoHiem) && !Missions.Contains(MissionsType.DatDoiAcBa) && !Missions.Contains(MissionsType.DatDoiMongHeo))
                                {
                                    if (!Setting.TeamLeader.Contains(TLBB.Name))
                                    {
                                        foreach (var game in Party)
                                        {
                                            if (Setting.TeamLeader.Contains(game.TLBB.Name))
                                                AppointLeader(game.TLBB.Name);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (Missions.Contains(MissionsType.DatDoiBinhThanh))
                    {
                        string mem = "";
                        byte[] mebuff = Encoding.UTF8.GetBytes("-");
                        List<string> list = new List<string>();
                        foreach (KeyValuePair<string, string> kvp in CalendarEx.QuanDoan)
                        {
                            if (kvp.Value.Contains("[" + TLBB.Name + "]"))
                            {
                                mem = kvp.Value;
                                list = TDT.ListStringBetween(kvp.Value, "[", "]");
                                break;
                            }
                        }
                        for (int i = 0; i < list.Count; i++)
                        {
                            list[i] = list[i].Trim();
                        }
                        if (mem != string.Empty)
                        {
                            foreach (var game in Main.Instance.AllOnelineGame)
                            {
                                if (mem.Contains("[" + game.TLBB.Name + "]"))
                                {
                                    game.PushMissions(MissionsType.DatDoiBinhThanh);
                                    if (Missions.Contains(MissionsType.BinhThanhKho))
                                    {
                                        game.PushMissions(MissionsType.BinhThanhKho);
                                    }
                                    mebuff = mebuff.Concat(game.TLBB.VISCIIName).Concat(Encoding.UTF8.GetBytes("-")).ToArray();
                                }
                            }
                            if (TLBB.MapId == MAP.BinhThanhKyTran)
                            {
                                if (QuanDoanParty1.Count + QuanDoanParty2.Count < 12 && QuanDoanParty1.Count + QuanDoanParty2.Count > 0)
                                {
                                    foreach (var game in Main.Instance.AllOnelineGame)
                                    {
                                        if (game.QuanDoanParty1.Count + game.QuanDoanParty2.Count == 0)
                                        {
                                            if (mem.Contains("[" + game.TLBB.Name + "]"))
                                            {
                                                game.AskRaid(TLBB.VISCIIName);
                                            }
                                        }
                                    }
                                    DoBuffer(Encoding.UTF8.GetBytes("teams = '").Concat(mebuff).Concat(Encoding.UTF8.GetBytes(@"'; for i = 0, Raid:GetApplicantCount() - 1  do szNick, iFamily, iLevel, iCapID, iHead, iArmourID, iCuffID, iFootID, iWeaponID, _, sZoneWorldID = Raid:GetApplyMemberInfoByIdx(i); if string.find(teams, szNick) then Player:SendAgreeRaidApplication(i); end end ")).ToArray());
                                }
                            }
                            else
                            {
                                if (TLBB.PartyId == 0xFFFFFFFF)
                                {
                                    if (QuanDoanParty1.Count == 0)
                                    {
                                        if (list[0] == TLBB.Name)
                                        {
                                            DoStringEx("Player:CreateTeamSelf();");
                                        }
                                    }
                                }
                                if (TLBB.IsLeader)
                                {
                                    if (QuanDoanParty1.Count == 6 && QuanDoanParty2.Count < 6)
                                    {
                                        if (QuanDoanParty1[0].TLBB.Name == TLBB.Name)
                                        {
                                            foreach (var game in Main.Instance.AllOnelineGame)
                                            {
                                                if (list.Skip(6).Contains(game.TLBB.Name))
                                                {
                                                    if (game.TLBB.PartyId != 0xFFFFFFFF)
                                                    {
                                                        game.DoStringEx("Player:LeaveTeam();");
                                                    }
                                                    else
                                                    {
                                                        if (game.QuanDoans.Count == 0)
                                                            game.AskRaid(TLBB.VISCIIName);
                                                    }
                                                    //game.LUA.DoString("Target:SendRaidApplication('" + foreGame.TLBB.Name + "')");
                                                }
                                            }
                                            string mems = "";
                                            mebuff = Encoding.UTF8.GetBytes("-");
                                            for (int i = 0; i < list.Count; i++)
                                            {
                                                mems += list[i] + "-";
                                                foreach (var gg in Main.Instance.AllOnelineGame)
                                                {
                                                    if (gg.TLBB.Name == list[i])
                                                    {
                                                        mebuff = mebuff.Concat(gg.TLBB.VISCIIName).Concat(Encoding.UTF8.GetBytes("-")).ToArray();
                                                    }
                                                }
                                            }
                                            DoBuffer(Encoding.UTF8.GetBytes("teams = '").Concat(mebuff).Concat(Encoding.UTF8.GetBytes(@"'; for i = 0, Raid:GetApplicantCount() - 1  do szNick, iFamily, iLevel, iCapID, iHead, iArmourID, iCuffID, iFootID, iWeaponID, _, sZoneWorldID = Raid:GetApplyMemberInfoByIdx(i); if string.find(teams, szNick) then Player:SendAgreeRaidApplication(i); end end ")).ToArray());
                                        }
                                    }
                                    if (list[0] != TLBB.Name)
                                    {
                                        AppointLeader(list[0]);
                                    }
                                    else
                                    {
                                        if (Party.Count() == 6)
                                        {
                                            bool isvalid = true;
                                            foreach (var game in Party)
                                            {
                                                bool ishave = false;
                                                for (int i = 0; i < list.Count && i < 6; i++)
                                                {
                                                    if (list[i] == game.TLBB.Name)
                                                    {
                                                        ishave = true;
                                                    }
                                                }
                                                if (!ishave)
                                                {
                                                    game.DoStringEx("Player:LeaveTeam();");
                                                    isvalid = false;
                                                }
                                            }
                                            if (isvalid)
                                            {
                                                DoStringEx("Player:CreateRaidSelf()");
                                            }
                                        }
                                        else
                                        {
                                            foreach (var game in Main.Instance.AllOnelineGame)
                                            {
                                                if (list.IndexOf(game.TLBB.Name) <= 5 && list.IndexOf(game.TLBB.Name) >= 0)
                                                {
                                                    if (game.Leader != null && game.Leader.TLBB.Name.Trim() != list[0].Trim())
                                                    {
                                                        game.DoStringEx("Player:LeaveTeam();");
                                                    }
                                                    else
                                                    {
                                                        if (game.Leader == null)
                                                        {
                                                            foreach (var gg in Main.Instance.AllOnelineGame)
                                                            {
                                                                if (gg.TLBB.Name == list[0])
                                                                {
                                                                    game.AskTeam(gg.TLBB.VISCIIName);
                                                                    game.AskRaid(gg.TLBB.VISCIIName);
                                                                    break;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            string mems = "";
                                            mebuff = Encoding.UTF8.GetBytes("-");
                                            for (int i = 0; i < list.Count && i < 6; i++)
                                            {
                                                mems += list[i] + "-";
                                                foreach (var gg in Main.Instance.AllOnelineGame)
                                                {
                                                    if (gg.TLBB.Name == list[i])
                                                    {
                                                        mebuff = mebuff.Concat(gg.TLBB.VISCIIName).Concat(Encoding.UTF8.GetBytes("-")).ToArray();
                                                    }
                                                }
                                            }
                                            DoBuffer(Encoding.UTF8.GetBytes("teams = '").Concat(mebuff).Concat(Encoding.UTF8.GetBytes(@"';
for i = 0, DataPool:GetApplyMemberCount() do
		if teams == nil then
			Player:SendAgreeJoinTeam_Apply(i);
		else
			szNick,iFamily,iLevel,iCapID,iHead,iArmourID,iCuffID,iFootID,iWeaponID,_,sZoneWorldID = DataPool:GetApplyMemberInfo(i);
			if string.find(teams, szNick) then
				Player:SendAgreeJoinTeam_Apply(i);
			end
		end
	end
	szNick,iFamily,iLevel,iCapID,iHead,iArmourID,iCuffID,iFootID,iWeaponID,_,sZoneWorldID = DataPool:GetInviteTeamMemberInfo(0,0);
	if teams ~= nil and DataPool:GetInviteTeamCount() > 0 and string.find(teams, szNick) then
		Player:AgreeJoinTeam(0);
	end
	if teams == nil  and DataPool:GetInviteTeamCount() > 0 then
		Player:AgreeJoinTeam(0);
	end
	DataPool:ClearAllApply();
	Player:RejectJoinTeam(0);")).ToArray());

                                            if (QuanDoanParty1.Count + QuanDoanParty2.Count > 0)
                                            {
                                                mebuff = Encoding.UTF8.GetBytes("-");
                                                for (int i = 0; i < list.Count; i++)
                                                {
                                                    foreach (var gg in Main.Instance.AllOnelineGame)
                                                    {
                                                        if (gg.TLBB.Name == list[i])
                                                        {
                                                            mebuff = mebuff.Concat(gg.TLBB.VISCIIName).Concat(Encoding.UTF8.GetBytes("-")).ToArray();
                                                        }
                                                    }
                                                }
                                                DoBuffer(Encoding.UTF8.GetBytes("teams = '").Concat(mebuff).Concat(Encoding.UTF8.GetBytes(@"'; for i = 0, Raid:GetApplicantCount() - 1  do szNick, iFamily, iLevel, iCapID, iHead, iArmourID, iCuffID, iFootID, iWeaponID, _, sZoneWorldID = Raid:GetApplyMemberInfoByIdx(i); if string.find(teams, szNick) then Player:SendAgreeRaidApplication(i); end end ")).ToArray());
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (!Main.IsCreateamTyVo)
                    {
                        if (!IsCreate)
                        {
                            if ((Setting.Is("checkToDoiGanNhau")) && !Missions.Contains(MissionsType.NhanBong))
                            {
                                SmartTeam();
                            }
                        }
                    }
                }
            }
        }



        public void TangCapTruongThanhLongVan()
        {
            if (GoToEx("368,421,762"))
            {
                foreach (PacketItem item in PacketItems.ThienCo.Where(i => i.IsCoDinh).Where(i => i.Name == "Long Văn Ngọc Linh" || i.Name == "Chú Văn Huyết Ngọc"))
                {
                    GetItemThienCo(item.Index);
                }
                Thread.Sleep(350);
                DoStringEx("setmetatable(_G, {__index = SelfEquip_Env});  if not this:IsVisible() then setmetatable(_G, {__index = MainMenuBar_Env}); MainMenuBar_SelfEquip_Clicked(); end");
                Thread.Sleep(1000);
                DoStringEx("setmetatable(_G, {__index = SelfEquip_Env}); SelfEquip_Equip_Click(25, 0);");
                Thread.Sleep(1000);
                while (true)
                {
                    PacketItem bestitem = PacketItems.All.Where(p => p.TypeName == "Long Văn").OrderByDescending(p => p.Star * p.CapTruongThanhLongVan).FirstOrDefault();
                    PacketItem longvanngoclinh = PacketItems.All.Where(p => p.Name == "Long Văn Ngọc Linh" && p.IsCoDinh).FirstOrDefault();
                    PacketItem chuvanhuyetngoc = PacketItems.All.Where(p => p.Name == "Chú Văn Huyết Ngọc" && p.IsCoDinh).FirstOrDefault();
                    if (bestitem != null && longvanngoclinh != null)
                    {
                        if (chuvanhuyetngoc == null)
                        {
                            DoStringEx("PlayerPackage:Lw_Op_Do(1, " + bestitem.Index + "," + longvanngoclinh.Index + ",-1)");
                        }
                        else
                        {
                            DoStringEx("PlayerPackage:Lw_Op_Do(1, " + bestitem.Index + "," + longvanngoclinh.Index + "," + chuvanhuyetngoc.Index + ")");
                        }
                        Thread.Sleep(300);
                    }
                    else
                    {
                        break;
                    }
                }
                for (int i = 0; i < 10; i++)
                {
                    PacketItem bestitem = PacketItems.All.Where(p => p.TypeName == "Long Văn").OrderByDescending(p => p.Star * p.CapTruongThanhLongVan).FirstOrDefault();
                    if (bestitem == null)
                        break;
                    if (PacketItems.TrangBi.Where(p => p.TypeName == "Long Văn").FirstOrDefault() != null)
                        break;
                    DoAction(bestitem.Type, bestitem.PacketId);
                    Thread.Sleep(350);
                }
                RemoveMission(MissionsType.TangCapTruongThanhLongVan);
                PushDebugMessage("Tăng Xong Rồi Nhé");
            }
        }

        public void SetNull()
        {
            Task.Run((Action)(() =>
            {
                NeedToMove = string.Empty;
                ApTieu = 0;
                CareObject = null;
                CareX = CareY = 0;
                CoBanFailTime = new Stopwatch();
                CurBossMap = -1;
                CurGomDo = null;
                CurMapMaTac = -1;
                DaNhanHoaChung = false;
                DaNhanHoaHong = false;
                DeadObjects.Clear();
                DoStringEx("COUNT = nil;");
                Global.lstketban.Clear();
                Global.lstsudo.Clear();
                IsCheDo = false;
                IsCheckNhiemmVu = false;
                IsCuongHoa7 = IsGiamDinhDo;
                IsDauCo = false;
                IsDead = false;
                IsDuaHau = false;
                IsHaoHuu = false;
                IsKetBai = false;
                IsKetNghia = false;
                IsMoBang = false;
                IsNhanBongSuDo = false;
                IsNhanHoaHong = false;
                IsNhanMam = false;
                IsNhatHop = false;
                IsNhatTuyet = false;
                IsOneTNT = false;
                IsP = false;
                IsSuDo = false;
                IsSuDoo = false;
                IsTalkNhiemVu = false;
                IsThoiBong = false;
                IsThuHoach = false;
                IsThuHoachHoaEx = false;
                IsThuyLao = false;
                IsXongBHD = false;
                IsXongBossMap = false;
                ListMoveEx.Clear();
                MissionState = string.Empty;
                Missions.Clear();
                MoveExTime = Stopwatch.StartNew();
                NeedToMove = string.Empty;
                Q12TK = 0;
                QTOCHAUSTATE = QLAULANSTATE = 0;
                QToChauClearMonterTime = QLauLanClearMonterTime = Stopwatch.StartNew();
                ResetHoaSpee();
                Skills.Clear();
                State = STATE.None;
                TamPhapNangToi = 0;
                Thread.CurrentThread.IsBackground = true;
                Win.SetWindowText(Handle, TDT.ClearSign(TLBB.Name));
                checknvcount = 0;
                sovongsumon = 0;
                swStandTime = Stopwatch.StartNew();
                IsByLogin = false;

                if (Main.GomDoName == TLBB.Name)
                    Main.GomDoName = string.Empty;
                NhiemVuChuaLam1 = new List<string>()
             {
                "Sinh Tài Chi Đạo",
                "Khoa Ngân Võ Lâm Ấn Túc Cầu Thịnh Hội",
                "Khoa Ngân Võ Lâm Ấn Tàng Kinh Các Nguy Cơ",
                "Khoa Ngân Võ Lâm Ấn Phồn Mang Tào Vận",
                "Quả Ngân Võ Lâm Ấn Bang HộiBuôn Bán",
             };
                NhiemVuChuaLam2 = new List<string>()
             {
                "Thanh Đồng Ấn-Tương Trợ Sư Môn",
                "Thanh Đồng Ấn-Trừ Ác",
                "Thanh Đồng Ấn-Trừng Hung",
             };
                if (threadcanquet != null)
                {
                    if (threadcanquet.IsAlive)
                    {
                        try
                        {
                            threadcanquet.Abort();
                        }
                        catch { }
                    }
                    threadcanquet = null;
                }
            }));

            //State = STATE.None;
        }

        private int sovongsumon = 0;
        public ArrayOfByte ArrayOfByte { get; set; }

        public bool IsSelectLogin { get; set; }



        public int ApTieu { get; set; }

        public Stopwatch swTieuVienSon = Stopwatch.StartNew();

        private string tieuxa = string.Empty;

        public void ApTieuPhungMinhTran()
        {
            if (!IsOneSec)
                return;
            bool isApTieu = false;            
            if (TLBB.IsQuestOpen)
            {
                if (QuestFrame.Click("#{BXJH_140815_54"))
                {
                    QuestFrame.Close();
                    return;
                }
                //#{BXJH_140815_07}
                if (QuestFrame.Click("#{BXJH_140815_07}"))
                {
                    QuestFrame.Close();
                    return;
                }
                foreach (QuestFrameItem q in QuestFrame.Items)
                {
                    if (q.Name.Contains("#{TWLB_200729_67"))
                    {
                        QuestFrame.Click(q.Name);
                        return;
                    }
                }
                if (QuestFrame.Click("#{TWLB_200729_55}"))
                    return;
                if (QuestFrame.Text.Contains("#{TWLB_200729_54}") && !QuestFrame.Text.Contains("#{TWLB_200729_55}"))
                {
                    tieuxa = string.Empty;
                }
            }
           
            float distance = 300;
            foreach (GameObject _object in Objects.All)
            {
                if (TDT.VietLien(_object.Title).Contains(TDT.VietLien(TLBB.Name)) && TDT.VietLien(_object.Title).Contains(TDT.VietLien("#{TWLB_200729_99}")))
                {
                    Move(_object.X, _object.Y);
                    tieuxa = _object.X + "," + _object.Y + "," + TLBB.MapId;
                    distance = _object.Distance;
                    isApTieu = true;
                }
            }
            if (isApTieu)
            {
                foreach (GameObject _object in Objects.All)
                {
                    if (_object.Title.Contains("Người Truyền Tống Hành Tiêu") && distance < 8)
                    {
                        Talk(_object.Id);
                    }
                    if (_object.Title.Contains("Người Tiếp Nhận Hành Tiêu") && distance < 8)
                    {
                        Talk(_object.Id);
                    }
                }
                return;
            }
            else
            {
                if (tieuxa.Split(',').Length == 3)
                {
                    Move(TDT.ParseAllInt(tieuxa.Split(',')[0]), TDT.ParseAllInt(tieuxa.Split(',')[1]), TDT.ParseAllInt(tieuxa.Split(',')[2]));
                }
                else
                {
                    if (GoToEx("392,345,762"))
                    {
                        DoStringEx(@" Clear_XSCRIPT()
                                    Set_XSCRIPT_Function_Name('ClientSelect') 
                                    Set_XSCRIPT_ScriptID(888160)
                                    Set_XSCRIPT_Parameter(0, 3)
                                    Set_XSCRIPT_Parameter(1, 1)
                                    Set_XSCRIPT_Parameter(2, " + ApTieu + @")
                                    Set_XSCRIPT_ParamCount(3)
                                    Send_XSCRIPT()");
                        Thread.Sleep(1000);
                        LUA.MessageBox_Self2_OK_Clicked();
                        Thread.Sleep(1000);
                    }
                }
            }
            //if (GoTo(PHUNGMINHTRAN.TieuPhong))
            //{
            //    if (TLBB.IsRide)
            //    {
            //        DownRide();
            //    }
            //    if (TLBB.IsQuestOpen)
            //    {
            //        QuestFrame.Click("#{BXJH_140815_123}");
            //        Thread.Sleep(300);
            //        if (ApTieu == 1)
            //            DoStringEx("setmetatable(_G, {__index = Escort_JieBiao_Env}); Escort_JieBiao_OnClick(1);");
            //        if (ApTieu == 2)
            //            DoStringEx("setmetatable(_G, {__index = Escort_JieBiao_Env}); Escort_JieBiao_OnClick(2);");
            //        if (ApTieu == 3)
            //            DoStringEx("setmetatable(_G, {__index = Escort_JieBiao_Env}); Escort_JieBiao_OnClick(3);");
            //        Thread.Sleep(300);
            //        MessageBoxSelf2OkClicked();
            //        QuestFrame.Close();
            //    }
            //    else
            //    {
            //        Talk(PHUNGMINHTRAN.TieuPhong);
            //    }
            //}
        }

        public string Img { get; set; }
        public string BinEx { get; set; }
        public uint BaseImg { get; set; }
        public bool IsThreadReadImgRun { get; set; }

        public string ImgHash
        {
            get
            {
                return TDT.Hasher.MD5(Img);
            }
        }

        public void ReadBaseImg()
        {
            if (!IsThreadReadImgRun)
            {
                new Thread(new ThreadStart(ReadBaseImgThread))
                {
                    IsBackground = true
                }.Start();
            }
        }


        /////////////////////////////////////////////////////////////////////
        //lecaotri
        public void phucloi()
        {
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                Thread.Sleep(1000);
                //50 08 8E 00 00 00 49 56 00 00 00 00 FF FF FF FF F3 9E 0D 00 0E 47 65 74 45 78 70 5F 42 79 4D 6F 6E 65 79 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 C4 46 7A C7 38 F3 15 01 21 95 88 00 01 00 00 00 54 F0 15 01 D0 3E 2F 59 90 E3 2D 01 80 F7 29 01 80 F7 29 01 EE D8 FF FF 48 96 70 05 6C F0 15 01 11 2E 30 59 80 F7 29 01 5C 80 00 01 80 F7 29 01 68 6E E4 2E B0 F0 15 01 E2 21 31 59 28 66 05 00 48 96 70 05 80 F7 29 01 80 F7 29 01 80 F7 29 01 30 0B EB 41 06 00 00 00 80 F7 29 01 00 00 00 00 01 00 00 00 0C 00 00 00 40 A1 23 35 60 FE 96 32 94 D4 AA 32 48 96 70 05 C0 F0 15 01 FF 29 30 59 80 F7 29 01 80 F7 29 01 D4 F0 15 01 03 BC 2F 59 80 F7 29 01 24 96 70 05 FF FF FF FF 40 F1 15 01 02 2F 30 59 80 F7 29 01 90 F1 15 01 00 00 00 00 F0 BB 2F 59 90 F1 15 01 80 F7 29 01 00 00 00 00
                SendPacket(HexToString(Address.NhanKn) + " 00 00 49 56 00 00 00 00 FF FF FF FF F3 9E 0D 00 0E 47 65 74 45 78 70 5F 42 79 4D 6F 6E 65 79 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 ");
                Thread.Sleep(1000);
            }).Start();
        }


        /////////////////////////////////////////////////////////////////////
        public void CheThanKhi()
        {
            //Liên Nhung Thần Tiết Cấp
            //Học Đúc kỹ năng
            //Tại hạ xác định muốn học

            if (GoToEx(TOCHAU.TietChuc))
            {
                if (TLBB.IsRide)
                {
                    DownRide();
                    Thread.Sleep(1000);
                }
                bool ishave = false;
                foreach (var item in PacketItems.All)
                {
                    if (item.Name.Contains("Liên Nhung Thần Tiết Cấp"))
                    {
                        ishave = true;
                        break;
                    }
                }
                if (!ishave)
                {
                    RemoveMission(MissionsType.CheThanKhi);
                    PushDebugMessage("Không có nguyên liệu, dừng auto");
                    return;
                }
                Thread.Sleep(1000);
                Talk(TOCHAU.TietChuc);
                for (int i = 0; i < 10; i++)
                {
                    Thread.Sleep(1000);
                    if (QuestFrame.Click("Học Đúc kỹ năng"))
                        break;
                }
                for (int i = 0; i < 10; i++)
                {
                    Thread.Sleep(1000);
                    if (QuestFrame.Click("Tại hạ xác định muốn học"))
                        break;
                }
                Thread.Sleep(1000);
                QuestFrame.Close();
                OpenPacketBase();
                for (int i = 0; i < 7; i++)
                {
                    foreach (var item in PacketItems.All)
                    {
                        if (GAMEDIC.ThanKhiDoTuong.Contains(item.Name.Trim()))
                        {
                            item.Use();
                            Thread.Sleep(1000);
                            while (TLBB.State != 0)
                            {
                                Thread.Sleep(1000);
                            }
                        }
                    }
                }
                int cheid = -1;
                if (TLBB.Menpai == MENPAI.VoDang || TLBB.Menpai == MENPAI.TinhTuc || TLBB.Menpai == MENPAI.NgaMy || TLBB.Menpai == MENPAI.MoDung || TLBB.Menpai == MENPAI.DaoHoa)
                    cheid = 420;
                if (TLBB.Menpai == MENPAI.CaiBang || TLBB.Menpai == MENPAI.MinhGiao || TLBB.Menpai == MENPAI.ThieuLam)
                    cheid = 419;
                if (TLBB.Menpai == MENPAI.ThienLong || TLBB.Menpai == MENPAI.ThienSon)
                    cheid = 421;
                if (TLBB.Menpai == MENPAI.TieuDao)
                    cheid = 422;
                if (TLBB.Menpai == MENPAI.QuyCoc)
                    cheid = 1291;
                if (TLBB.Menpai == MENPAI.DuongMon)
                    cheid = 1242;
                if (TLBB.Menpai == MENPAI.DaoHoa)
                    cheid = 1324;
                if (cheid != -1)
                {
                    PushDebugMessage("Bắt đầu chế thần khí 30s");
                    DoStringEx("setmetatable(_G, { __index = Synthesize_Env}); ComposeItem_Begin(" + cheid + ", 99, -1);");
                    Thread.Sleep(1000);
                    while (TLBB.State != 0)
                    {
                        Thread.Sleep(1000);
                    }
                }
                if (chetkcnt++ > 4)
                {
                    RemoveMission(MissionsType.CheThanKhi);
                    chetkcnt = 0;
                }
            }
        }

        private int chetkcnt = 0;

        public List<MissionsType> Missions { get; set; } = new List<MissionsType>();

        public void PushMissions(MissionsType miss, bool isPush = true)
        {
            if (!isPush)
                Missions.Remove(miss);
            else if (!Missions.Contains(miss))
                Missions.Add(miss);
        }

        public void PushMissions(MissionsType[] miss)
        {
            foreach (var m in miss)
            {
                if (!Missions.Contains(m))
                    Missions.Add(m);
            }
        }

        public void RemoveMission(MissionsType[] miss)
        {
            foreach (var m in miss)
            {
                Missions.Remove(m);
            }
        }

        public void RemoveMission(MissionsType miss)
        {
            Missions.Remove(miss);
        }

        public void ReadBaseImgThread()
        {
            IsThreadReadImgRun = true;

            //14967D0
            uint[] baseCaptcha = new uint[] { 0xB31020, 0x130, 0x0, 0x14, 0x1D0, 0x90 };
            uint baseAddress = Memory.Read(baseCaptcha);
            if (Memory.Read(baseAddress).ToString("X8").StartsWith("A0"))
            {
                BaseImg = baseAddress;
            }
            else
            {
                BaseImg = (uint)AOB.SearchPrivateRegion(new byte[] { 0x01, 0x00, 0x00, 0x00, 0xFF, 0xA0, 0xFF, 0xA0, 0xFF, 0xA0, 0xFF, 0xA0, 0xFF, 0xA0, 0xFF, 0xA0, 0xFF, 0xA0, 0xFF, 0xA0, 0xFF, 0xA0, 0xFF, 0xA0, 0xFF, 0xA0, 0xFF, 0xA0 }, 0, 0x7FFFFFFF) + 0x4;
            }
            IsThreadReadImgRun = false;
        }

        public Bitmap captcha;

        public Bitmap Captcha
        {
            get
            {
                if (captcha == null)
                {
                    captcha = new Bitmap(128, 36);
                    Img = string.Empty;
                    BinEx = string.Empty;

                    byte[] buffer = new byte[9218];
                    Memory.ReadProcessMemory(Memory.Id, BaseImg, buffer, buffer.Length, 0);
                    string bin = string.Empty;
                    for (int i = 0; i < 9216; i = i + 2)
                    {
                        if (BitConverter.ToInt32(new byte[4] { buffer[i], buffer[i + 1], 0, 0 }, 0) == 0xA000)
                        {
                            BinEx += "0";
                            bin += "0";
                        }
                        else
                        {
                            BinEx += "1";
                            bin += "1";
                        }
                        if (bin.Length == 8)
                        {
                            Img += Convert.ToInt32(bin, 2).ToString("X2");
                            bin = "";
                        }
                    }
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
        }

        public static List<string> CaptchaHash = new List<string>();

        public PRIVATEMOVE AutoMoveEx = new PRIVATEMOVE();


        public AOB AOB { get; set; }


        public ListViewItem Item { get; set; }

        public string MD5 { get; set; }

        private bool IsInstall { get; set; }
        public PacketItems PacketItems { get; set; }

        public Game(Process process, Address address)
        {
            Address = address;
            Process = process;
            ProcessId = process.Id;
            AOB = new AOB((uint)process.Id);
            PacketItems = new PacketItems(this);
            Handle = Win.GetHandle(ProcessId, "TianLongBaBu WndClass");
            if (Handle == IntPtr.Zero)
                Handle = Win.GetHandle(ProcessId, "kezgxubdpydrpeb");

            Memory = new Memory(ProcessId);

            ArrayOfByte = new ArrayOfByte(ProcessId);

            LUA = new LUA(this);
            QuestFrame = new QuestFrame(this);
            Team = new Team(this);

            Objects = new GameObjects(this);
            TLBB = new TLBB(this);

            ThreadAuto = new Thread(new ThreadStart(FuncAuto))
            {
                IsBackground = true,
            };
            ThreadAuto.Start();
        }

        private void Initx()
        {
            try
            {
                Init();
                IsInstall = true;
            }
            catch { }
        }

        public bool IsKhacTheGioi(TLBB tlbb)
        {
            if (TLBB.IsDaiTheGioi && tlbb.IsDaiTheGioi)
                return false;
            if (!TLBB.IsDaiTheGioi && !tlbb.IsDaiTheGioi)
                return false;
            return true;
        }

        private Stopwatch disPartyTime = new Stopwatch();



        private Stopwatch swexectime = Stopwatch.StartNew();

        public void FuncAuto()
        {
            while (true)
            {
                try
                {
                    int runtime = (int)(swexectime.Elapsed.TotalSeconds * 1000);
                    int delay = Setting.Value("numberDelay");
                    if (delay <= 0)
                        delay = 50;
                    if (delay - runtime > 0)
                    {
                        Thread.Sleep(delay - runtime);
                    }
                    swexectime = Stopwatch.StartNew();

                    while (Global.IsFull == 0 && !Global.IsAllowFree)
                        Thread.Sleep(1000);
                    if (!Main.DicGame.ContainsKey(ProcessId))
                    {
                        Exit();
                        break;
                    }
                    try
                    {
                        //if (Process.HasExited)
                        //{
                        //    Main.DicGame.Remove(Process.Id);
                        //    Main.Instance.Invoke(() => { Item.Remove(); });
                        //    break;
                        //}
                        if (!IsInstall)
                        {
                            Initx();
                            Thread.Sleep(1000);
                            continue;
                        }
                        Auto();

                        //addmaytinh = Memory.BaseGameAddreadnew(Process.Id);
                        //if(this.addmaytinh != addmaytinh)
                        //{
                        //    this.addmaytinh = addmaytinh;
                        //    Address = Address.GetInstance(MD5, Global.OFFSET, addmaytinh);
                        //}
                    }
                    catch { }
                }
                catch { }
            }
        }

        public void Sleep(int delay)
        {
            PostMessage(delay, 125);
        }

        public LUA LUA { get; set; }

        private string idLocDo;

        public string ChiNhat
        {
            get
            {
                idLocDo = IniParser.Read("LocDo", TLBB.AllowName);
                return idLocDo;
            }
            set
            {
                idLocDo = value;
                IniParser.Write("LocDo", TLBB.AllowName, idLocDo);
            }
        }

        public Process Process { get; set; }
        public int ProcessId { get; set; }
        public Address Address { get; set; }
        public IntPtr Handle { get; set; }

        public void PostMessage(int wParam, int lParam)
        {
            Win.PostMessage(Handle, Global.HookMessage, wParam, lParam);
        }

        public void PostMessage(uint wParam, int lParam) => PostMessage((int)wParam, (int)lParam);

        public void DropItem(uint idx)
        {
            PostMessage(idx, 108);
        }

        public Memory Memory { get; set; }


        public TLBB TLBB { get; set; }
        public bool IsRunAutoMap { get; set; }
        public bool IsRunAutoBossMap { get; set; }
        public GameObjects Objects { get; set; }
        public List<Skill> Skills { get; set; } = new List<Skill>();


        public event EventHandler SkillLoaded;

        public int IdleTime;
        public string LastName { get; set; }
        public int ExpStart { get; set; }
        public Stopwatch AutoTime { get; set; } = Stopwatch.StartNew();
        public int ExpGain { get; set; }
        public float ExpSpeed { get; set; }
        public float RadiusX { get; set; }
        public float RadiusY { get; set; }
        public int TargetId { get; set; }
        public Skill NgamySkill { get; set; }
        public Skill QuyCocSkill { get; set; }

        //public bool IsRide = false;
        public bool[] Alt { get; set; } = new bool[10];

        public bool[] F { get; set; } = new bool[12];
        public int[] KeyDelay { get; set; } = new int[22];
        public int BuffPetPercent = 50;
        public int DeadX;
        public int DeadY;
        public int DeadMap;
        public int DeadFakeMap;
        public bool IsDead;
        public bool IsAttack = true;

        public float CharX { get; set; }
        public float CharY { get; set; }

        public int RoundX
        {
            get
            {
                return (int)Math.Round(CharX, 0, MidpointRounding.AwayFromZero);
            }
        }

        public int RoundY
        {
            get
            {
                return (int)Math.Round(CharY, 0, MidpointRounding.AwayFromZero);
            }
        }

        public bool IsLureEx
        {
            get
            {
                if (IsPK)
                    return false;
                if (TLBB.MapId == MAP.BienGioiTongLieu)
                    return true;
                if (TLBB.MapId == MAP.LanHoanPhucDia)
                    return true;          
                if (TLBB.MapId == MAP.TuTuyetTrang || TLBB.MapId == MAP.TangKinhCac)
                    return true;
                if (TLBB.MapId == MAP.ThieuThatSon && TLBB.IsNoiCong)
                    return true;
                if (TLBB.MapId == MAP.PhungMinhVuongLang)
                    return true;
                if (TLBB.MapId == MAP.LoiDaiSinhTu)
                    return true;
                if (TLBB.MapId == MAP.TranLongKyCuoc || TLBB.MapId == MAP.LauLanBaoTang)
                    return false;
                if ((Global.AtkFollowKey && !TLBB.IsLeader))
                    return false;
                return IsLure;
            }
        }

        public bool IsLure { get; set; }

        public bool IsPet = true;
        public bool IsHP = true;
        public bool IsMP = true;
        public bool IsNM = false;
        public bool IsQuyCoc = false;
        public bool IsRao = false;
        public bool IsXongBHD = false;


        public bool DaNhanHoaHong = false;
        public bool DaNhanHoaChung = false;
        public bool IsPickItem = false;
        public bool IsX4 = false;
        public int TrongTrotIndex;
        public int ThuHoachIndex;
        public int MoveIndex = 0;
        public int MoveIndexEx = 0;

        public string Pass2
        {
            get
            {
                if (Global.IniParser != null)
                {
                    if (Global.IniParser.Read("Pass2", TLBB.AllowName).Length >= 4)
                    {
                        IniParser.Write("Pass2", TLBB.AllowName, Global.IniParser.Read("Pass2", TLBB.AllowName));
                        return Global.IniParser.Read("Pass2", TLBB.AllowName);
                    }
                }
                if (IniParser.Read("Pass2", TLBB.AllowName).Length >= 4)
                {
                    Global.IniParser.Write("Pass2", TLBB.AllowName, IniParser.Read("Pass2", TLBB.AllowName));
                }
                return IniParser.Read("Pass2", TLBB.AllowName);
            }
            set
            {
                Global.IniParser.Write("Pass2", TLBB.AllowName, value);
                IniParser.Write("Pass2", TLBB.AllowName, value);
            }
        }

        public void NhanQuaThangCap()
        {
            Task.Run(() =>
            {
                if (TLBB.Lvl >= 10)
                {
                    DoStringEx("setmetatable(_G, {__index = Fuli_QiankunBag_Env}); Fuli_QiankunBag_Clicked(1);");
                }
                if (TLBB.Lvl >= 20)
                {
                    DoStringEx("setmetatable(_G, {__index = Fuli_QiankunBag_Env}); Fuli_QiankunBag_Clicked(2);");
                }
                if (TLBB.Lvl >= 30)
                {
                    DoStringEx("setmetatable(_G, {__index = Fuli_QiankunBag_Env}); Fuli_QiankunBag_Clicked(3);");
                }
                if (TLBB.Lvl >= 35)
                {
                    DoStringEx("setmetatable(_G, {__index = Fuli_QiankunBag_Env}); Fuli_QiankunBag_Clicked(4);");
                }
                if (TLBB.Lvl >= 40)
                {
                    DoStringEx("setmetatable(_G, {__index = Fuli_QiankunBag_Env}); Fuli_QiankunBag_Clicked(5);");
                }
                if (TLBB.Lvl >= 45)
                {
                    DoStringEx("setmetatable(_G, {__index = Fuli_QiankunBag_Env}); Fuli_QiankunBag_Clicked(6);");
                }
                if (TLBB.Lvl >= 50)
                {
                    DoStringEx("setmetatable(_G, {__index = Fuli_QiankunBag_Env}); Fuli_QiankunBag_Clicked(7);");
                }
                if (TLBB.Lvl >= 55)
                {
                    DoStringEx("setmetatable(_G, {__index = Fuli_QiankunBag_Env}); Fuli_QiankunBag_Clicked(8);");
                }
            });
        }

        public string GameRaoTXT { get; set; }

        public string RaoTxt
        {
            get;
            set;
        }

        public bool AutoResetTime { get; set; }

        public int State
        {
            get;
            set;
        }


        public string MissionInfo { get; set; } = string.Empty;
        public int MissionX { get; set; }
        public int MissionY { get; set; }
        public int MissionMap { get; set; }
        public List<uint> lstNguoiRom = new List<uint>();

        public int MissionID { get; set; }

        public void ThoiBong()
        {
            if (!IsOneSec)
                return;
            if (TLBB.PlayerState != 0)
                return;
            if (GoToEx(Global.ThoiBongX, Global.ThoiBongY, DAILY.Id, false, 6, 20))
            {
                DownRide();

                bool iscobut = false;

                foreach (var item in PacketItems.All.Concat(PacketItems.ThienCo))
                {
                    if (item.ClearName.Contains("chucphucmaobut"))
                    {
                        iscobut = true;
                        break;
                    }
                }
                if (!iscobut)
                {
                    IsXongBHD = true;
                    IsThoiBong = false;
                    return;
                }

                GameObject bong = Objects.All.Where(o => o.GetDistance(Global.ThoiBongX, Global.ThoiBongY) <= 6 && o.Title.VietLien().Contains("cauphuckhicau")).OrderBy(_ => Guid.NewGuid()).FirstOrDefault();

                if (bong != null)
                {
                    if (bong.GetDistance(RoundX, RoundY) > 3)
                    {
                        GoToEx(bong.X, bong.Y);
                        Thread.Sleep(2000);
                    }

                    Talk(bong.Id);
                    Thread.Sleep(1000);

                    if (QuestFrame.Click("#{TGQF_XML_47}"))
                    {
                        Thread.Sleep(1000);
                        List<QuestFrameItem> list = QuestFrame.Items;
                        int count = list.Count;
                        int ranNum = new Random().Next(0, count);
                        QuestFrameOptionClicked((int)list[ranNum].StrOptionExtra1, (int)list[ranNum].StrOptionExtra2);
                    }
                    // Ta cũng cần chúc phúc cho họ
                    QuestFrame.Click("#{TGQF_XML_48}");
                }
            }
        }

        public void ChucPhucMaoBut()
        {
            if (!Missions.Contains(MissionsType.ChucPhucMaoBut) || TLBB.PlayerState != 0)
                return;
            if (!HaveItem("Chúc Phúc Mao Bút"))
            {
                foreach (var item in PacketItems.ThienCo)
                {
                    if (item.Name == "Chúc Phúc Mao Bút")
                    {
                        GetItemThienCo(item.Index);
                        return;
                    }
                }
                return;
            }
            foreach (GameObject _object in Objects.All)
            {
                if (_object.Title.VietLien().Contains("cauphuckhicau"))
                {
                    Talk(_object.Id);
                    Thread.Sleep(350);
                    // Hãy chọn cầu phúc
                    if (QuestFrame.Click("#{TGQF_XML_47}"))
                    {
                        Thread.Sleep(350);
                        List<QuestFrameItem> list = QuestFrame.Items;
                        int count = list.Count;
                        int ranNum = new Random().Next(0, count);
                        QuestFrameOptionClicked((int)list[ranNum].StrOptionExtra1, (int)list[ranNum].StrOptionExtra2);
                    }
                    // Ta cũng cần chúc phúc cho họ
                    QuestFrame.Click("#{TGQF_XML_48}");
                    return;
                }
            }
        }

        public void TrongTrot()
        {
            if (!IsOneSec)
                return;
            if (swStandTime.Elapsed.TotalSeconds < 2)
                return;
            if (TLBB.PlayerState != 0)
                return;
            if (Missions.Contains(MissionsType.TrongTrot))
            {
                if (!IsThuHoach)
                {
                    if (MissionState == string.Empty)
                    {
                        float minDistance = 100;
                        uint talkId = 0;
                        if (talkId == 0)
                        {
                            foreach (GameObject _object in Objects.All)
                            {
                                if (((TDT.VietLien(_object.Name).Contains("nguoirom")) || (TDT.VietLien(_object.Name).Contains("daothaonhan"))) && TDT.GetDistance(_object.X, _object.Y, CharX, CharY) < minDistance)
                                {
                                    if (lstNguoiRom.Contains(_object.Id))
                                        continue;
                                    minDistance = TDT.GetDistance(_object.X, _object.Y, CharX, CharY);
                                    talkId = _object.Id;
                                }
                            }
                        }
                        if (talkId != 0)
                        {
                            lstNguoiRom.Add(talkId);
                            Talk(talkId);
                            MissionState = "Talk";
                            return;
                        }
                        if (MissionState == string.Empty)
                            lstNguoiRom.Clear();
                    }
                    else if (MissionState == "Talk")
                    {
                        if (TrongTrotIndex == 0)
                        {
                            foreach (QuestFrame dialog in QuestFrame.Enum(this))
                            {
                                if (dialog.Name.EndsWith("sớm"))
                                {
                                    QuestFrameOptionClicked(dialog);
                                    MissionState = "Talk1";
                                    return;
                                }
                            }
                        }
                        if (TrongTrotIndex == 1)
                        {
                            foreach (QuestFrame dialog in QuestFrame.Enum(this))
                            {
                                if (dialog.Name.EndsWith("muộn"))
                                {
                                    QuestFrameOptionClicked(dialog);
                                    MissionState = "Talk1";
                                    return;
                                }
                            }
                        }
                    }
                    else if (MissionState == "Talk1")
                    {
                        List<QuestFrame> lstDialog = QuestFrame.Enum(this);
                        if (lstDialog.Count > ThuHoachIndex + 1)
                        {
                            QuestFrame dialog = lstDialog[ThuHoachIndex + 1];
                            MissionState = string.Empty;
                            QuestFrameOptionClicked(dialog);
                            return;
                        }
                    }
                    MissionState = "";
                }
                else
                {
                    float minDistance = 100;
                    int talkId = -1;
                    foreach (GameObject _object in Objects.All)
                    {
                        _object.DistanceEx = TDT.GetDistance(CharX, CharY, _object.X, _object.Y);
                        if (_object.IsTaiNguyen && _object.DistanceEx <= 4.5 && !_object.Name.Contains("("))
                        {
                            PickItem((int)_object.Id);
                            return;
                        }
                    }
                    if (MissionState == string.Empty)
                    {
                        if (talkId == -1)
                        {
                            foreach (GameObject _object in Objects.All)
                            {
                                if (((TDT.VietLien(_object.Name).Contains("nguoirom")) || (TDT.VietLien(_object.Name).Contains("daothaonhan"))) && TDT.GetDistance(_object.X, _object.Y, CharX, CharY) < minDistance)
                                {
                                    minDistance = TDT.GetDistance(_object.X, _object.Y, CharX, CharY);
                                    talkId = (int)_object.Id;
                                }
                            }
                        }
                        if (talkId != -1)
                        {
                            Talk(talkId);
                            MissionState = "Talk";
                            return;
                        }
                    }
                    else if (MissionState == "Talk")
                    {
                        if (TrongTrotIndex == 0)
                        {
                            foreach (QuestFrame dialog in QuestFrame.Enum(this))
                            {
                                if (dialog.Name.EndsWith("sớm"))
                                {
                                    QuestFrameOptionClicked(dialog);
                                    MissionState = "Talk1";
                                    return;
                                }
                            }
                        }
                        if (TrongTrotIndex == 1)
                        {
                            foreach (QuestFrame dialog in QuestFrame.Enum(this))
                            {
                                if (dialog.Name.EndsWith("muộn"))
                                {
                                    QuestFrameOptionClicked(dialog);
                                    MissionState = "Talk1";
                                    return;
                                }
                            }
                        }
                    }
                    else if (MissionState == "Talk1")
                    {
                        List<QuestFrame> lstDialog = QuestFrame.Enum(this);
                        if (lstDialog.Count > ThuHoachIndex + 1)
                        {
                            QuestFrame dialog = lstDialog[ThuHoachIndex + 1];
                            QuestFrameOptionClicked(dialog);
                            MissionState = string.Empty;
                            return;
                        }
                    }
                }
                MissionState = string.Empty;
            }

        }

        public void TalkTo(NPC npc)
        {
            if (npc == null)
                return;
            if (GoToEx(npc))
            {
                Talk(npc);
            }
        }

        public Stopwatch MoveExTime = new Stopwatch();
        public List<string> ListMoveEx = new List<string>();
        public Stopwatch NhatDoEX = Stopwatch.StartNew();


        public bool IsMoveEx
        {
            get
            {
                if (TLBB.MapId == MAP.TuTuyetTrang && GetDistance(100, 108) < 10)
                {
                    MoveExTime = Stopwatch.StartNew();
                    ListMoveEx.Clear();
                    return true;
                }
                if (SwTimeOnMap.Elapsed.TotalSeconds < 3)
                {
                    MoveExTime = Stopwatch.StartNew();
                    ListMoveEx.Clear();
                    return true;
                }
                if (Global.Paused)
                {
                    MoveExTime = Stopwatch.StartNew();
                    ListMoveEx.Clear();
                }
                while (ListMoveEx.Count > 120)
                {
                    ListMoveEx.RemoveAt(0);
                }
                string point = RoundX + "," + RoundY;
                if (!ListMoveEx.Contains(point))
                {
                    ListMoveEx.Add(point);
                    MoveExTime = Stopwatch.StartNew();
                    return true;
                }
                else
                {
                    if (MoveExTime.Elapsed.TotalSeconds > 5)
                        return false;
                }
                return true;
            }
        }





        public bool FullPartyGoToEx(NPC npc, int clearMonterTime = 0)
        {
            bool isgo = true;
            foreach (var game in Party)
            {
                if(game.TrueClearMonterTime.Elapsed.TotalSeconds < clearMonterTime || game.PickTime.Elapsed.TotalSeconds < clearMonterTime)
                {
                    isgo = false;
                    continue;
                }
                if (!game.GoToEx(npc))
                {
                    isgo = false;
                }
            }
            return isgo;
        }

        public bool FullPartyGoToEx(float x, float y, int mapId = -1)
        {
            bool isgo = true;
            Party.ForEach(p => { if (!p.GoToEx(x, y, mapId)) { isgo = false; } });
            return isgo;
        }



        public bool GoToEx(string point)
        {
            int x = point.Split(',')[0].ToNumber();
            int y = point.Split(',')[1].ToNumber();
            int map = -1;
            if (point.Split(',').Length > 2)
                map = point.Split(',')[2].ToNumber();
            return GoToEx(x, y, map);
        }

        public bool GoToEx(GameObject obj, bool forceMove = false, float distanceUpRide = 30) => GoToEx(obj.X, obj.Y, -1, false, 3, distanceUpRide);

        public bool GoToEx(NPC npc, bool forceMove = false) => GoToEx(npc.X, npc.Y, npc.Map, forceMove);

        public bool GoToEx(float x, float y, int mapId = -1, bool forceMove = false, float distance = 3, float distanceUpRide = 30)
        {
            if (distance <= 0)
                distance = 1;
            if (NeBayTime.Elapsed.TotalSeconds <= 3)
            {
                distanceUpRide = 999;
            }
            if (Objects.HaveMonter("daluatdiem"))
                distanceUpRide = 9999;
            if (mapId == 501)
            {
                if (!TLBB.IsMapBang(TLBB.MapId))
                {
                    VaoBang();
                    return false;
                }
                else
                {
                    if (GetDistance(x, y) <= distance)
                        return true;
                }
            }
            if (mapId == MAP.QuyThi && TLBB.MapId != MAP.QuyThi)
            {
                if (TLBB.MapId == MAP.ToChau)
                {
                    if (GetDistance(TOCHAU.LieuKimThiem.X, TOCHAU.LieuKimThiem.Y) < 3)
                    {
                        Talk(TOCHAU.LieuKimThiem);
                        Thread.Sleep(1000);
                        QuestFrame.Click("#{GSMK_190807_13}");
                        Thread.Sleep(1000);
                        QuestFrame.Click("#{GSMK_190807_29}");
                        return false;
                    }
                }
                else if (TLBB.MapId == MAP.DaiLy)
                {
                    if (GetDistance(DAILY.TruongTheBinh.X, DAILY.TruongTheBinh.Y) < 3)
                    {
                        Talk(DAILY.TruongTheBinh);
                        Thread.Sleep(1000);
                        QuestFrame.Click("#{GSMK_190807_13}");
                        Thread.Sleep(1000);
                        QuestFrame.Click("#{GSMK_190807_29}");
                        return false;
                    }
                }
                else if (TLBB.MapId == MAP.LacDuong)
                {
                    if (GetDistance(LACDUONG.TrieuTienTon.X, LACDUONG.TrieuTienTon.Y) < 3)
                    {
                        Talk(LACDUONG.TrieuTienTon);
                        Thread.Sleep(1000);
                        QuestFrame.Click("#{GSMK_190807_13}");
                        Thread.Sleep(1000);
                        QuestFrame.Click("#{GSMK_190807_29}");
                        return false;
                    }
                }
                //DoStringEx(@"Clear_XSCRIPT()
                //        Set_XSCRIPT_Function_Name('ClickGuiShiButton')
                //        Set_XSCRIPT_ScriptID(893194)
                //        Set_XSCRIPT_ParamCount(0)
                //        Send_XSCRIPT()");
                DoStringEx("setmetatable(_G, {__index = PlayerQuicklyEnter_Env}); PlayerQuicklyEnter_Clicked(55); setmetatable(_G, {__index = GuiShi_Basics_Env}); GuiShi_Basics_CloseHuoYueAwardGoToButton();");
                Thread.Sleep(3000);
                return false;
            }
            if (TLBB.MapId == MAP.BinhThanhKyTran && (mapId == -1 || mapId == MAP.BinhThanhKyTran))
            {
                if (GetRoundBinhThanh((int)x, (int)y) == "Round4" && GetRoundBinhThanh(RoundX, RoundY) != "Round4")
                {
                    if (GoToEx(142, 178))
                        IsP = true;
                    return false;
                }
            }
            if (TLBB.MapId == MAP.ThanhHoaCung)
            {
                string curname = GetMapRound(RoundX, RoundY);
                string nextname = GetMapRound((int)x, (int)y);
                if (mapId == -1 || mapId == MAP.ThanhHoaCung)
                {
                    if (TDT.ParseAllInt(curname) < TDT.ParseAllInt(nextname))
                    {
                        if (curname == "Jump1")
                        {
                            if (GoToEx(26, 95, -1, true, (float)1.5))
                            {
                                TLBB.Fly(25, 84);
                            }
                            return false;
                        }
                        if (curname == "Jump2")
                        {
                            if (GoToEx(26, 82, -1, true, (float)1.5))
                            {
                                TLBB.Fly(25, 73);
                            }
                            return false;
                        }
                    }
                }
                if ((TDT.ParseAllInt(curname) > TDT.ParseAllInt(nextname) && (mapId == -1 || mapId == MAP.ThanhHoaCung)) || (mapId != MAP.ThanhHoaCung && mapId != -1))
                {
                    if (curname == "Jump3")
                    {
                        if (GoToEx(25, 73, -1, true, (float)1.5))
                        {
                            TLBB.Fly(26, 82);
                        }
                        return false;
                    }
                    if (curname == "Jump2")
                    {
                        if (GoToEx(25, 84, -1, true, (float)1.5))
                        {
                            TLBB.Fly(26, 95);
                        }
                        return false;
                    }
                }
            }
            if (TLBB.MapId == 207 && mapId > 207 && mapId <= 210)
            {
                if (GoToEx(64, 62, -1, true, (float)1.5))
                {
                    TLBB.Fly(64, 55);
                }
                return false;
            }
            if (TLBB.MapId == 202 && (mapId > 202 && mapId <= 210))
            {
                if (GoToEx(27, 69, -1, true, (float)1.5))
                {
                    TLBB.Fly(18, 82);
                }
                return false;
            }
            if (TLBB.MapId == 203 && (mapId == 202 || !IsMapCoMo(mapId)) && mapId != -1)
            {
                if (GoToEx(91, 91, -1, true, (float)1.5))
                {
                    TLBB.Fly(90, 100);
                }
                return false;
            }
            if (TLBB.MapId == 209)
            {
                string curname = GetMapRound(RoundX, RoundY);
                string nextname = GetMapRound((int)x, (int)y);
                if (curname == nextname && mapId != 210)
                {
                }
                else
                {
                    if ((TDT.ParseAllInt(curname) < TDT.ParseAllInt(nextname) && (mapId == -1 || mapId == 209)) || mapId == 210)
                    {
                        if (curname == "Jump0")
                        {
                            if (GoToEx(82, 23, -1, true, (float)1.5))
                            {
                                TLBB.Fly(73, 25);
                            }
                            return false;
                        }
                        if (curname == "Jump1")
                        {
                            if (GoToEx(73, 25, -1, true, (float)1.5))
                            {
                                TLBB.Fly(66, 23);
                            }
                            return false;
                        }
                        if (curname == "Jump2")
                        {
                            if (GoToEx(66, 23, -1, true, (float)1.5))
                            {
                                TLBB.Fly(56, 24);
                            }
                            return false;
                        }
                    }
                    else if ((TDT.ParseAllInt(curname) > TDT.ParseAllInt(nextname) && (mapId == -1 || mapId == 209)) || (mapId != 210 && mapId != -1))
                    {
                        if (curname == "Jump3")
                        {
                            if (GoToEx(56, 24, -1, true, (float)1.5))
                            {
                                TLBB.Fly(66, 23);
                            }
                            return false;
                        }
                        if (curname == "Jump2")
                        {
                            if (GoToEx(66, 23, -1, true, (float)1.5))
                            {
                                TLBB.Fly(73, 25);
                            }
                            return false;
                        }
                        if (curname == "Jump1")
                        {
                            if (GoToEx(73, 25, -1, true, (float)1.5))
                            {
                                TLBB.Fly(82, 23);
                            }
                            return false;
                        }
                    }
                }
            }
            if (TLBB.MapId == MAP.MaNhaiDong)
            {
                string curname = GetNameMaNhaiDong(RoundX, RoundY);
                string nextname = GetNameMaNhaiDong((int)x, (int)y);
                if (TDT.ParseAllInt(curname) < TDT.ParseAllInt(nextname) && (mapId == -1 || mapId == MAP.MaNhaiDong))
                {
                    if (curname == "Jump0")
                    {
                        if (GoToEx(164, 24, -1, true, (float)1.5))
                        {
                            TLBB.Fly(176, 23);
                        }
                    }
                    if (curname == "Jump1")
                    {
                        if (GoToEx(176, 23, -1, true, (float)1.5))
                        {
                            TLBB.Fly(183, 22);
                        }
                    }
                    if (curname == "Jump2")
                    {
                        if (GoToEx(187, 23, -1, true, (float)1.5))
                        {
                            TLBB.Fly(188, 32);
                        }
                    }
                    if (curname == "Jump5")
                    {
                        if (GoToEx(188, 35, -1, true, (float)1.5))
                        {
                            TLBB.Fly(188, 40);
                        }
                    }
                    return false;
                }
                else if (TDT.ParseAllInt(curname) > TDT.ParseAllInt(nextname) || (mapId != MAP.MaNhaiDong && mapId != -1))
                {
                    if (curname == "Jump1")
                    {
                        if (GoToEx(176, 23, -1, true, (float)1.5))
                        {
                            TLBB.Fly(164, 24);
                        }
                    }
                    if (curname == "Jump2")
                    {
                        if (GoToEx(183, 22, -1, true, (float)1.5))
                        {
                            TLBB.Fly(176, 23);
                        }
                    }
                    if (curname == "Jump5")
                    {
                        if (GoToEx(188, 32, -1, true, (float)1.5))
                        {
                            TLBB.Fly(187, 23);
                        }
                    }
                    if (curname == "Jump6")
                    {
                        if (GoToEx(188, 40, -1, true, (float)1.5))
                        {
                            TLBB.Fly(188, 35);
                        }
                    }
                    if (curname != "Jump0")
                        return false;
                }
            }
            if (TLBB.MapId == MAP.TanHoangDiaCungTang2)
            {
                string curname = GetNameTanHoangDiaCung2(RoundX, RoundY);
                string nextname = GetNameTanHoangDiaCung2((int)x, (int)y);

                if ((TDT.ParseAllInt(curname) < TDT.ParseAllInt(nextname) && (mapId == -1 || mapId == MAP.TanHoangDiaCungTang2)) || mapId == MAP.TanHoangDiaCungTang3 || mapId == MAP.TanHoangDiaCungTang4)
                {
                    if (curname == "Jump1")
                    {
                        if (GoToEx(99, 41, -1, true, (float)1.5))
                        {
                            TLBB.Fly(110, 38);
                        }
                    }
                    if (curname == "Jump2")
                    {
                        if (GoToEx(115, 39, -1, true, (float)1.5))
                        {
                            TLBB.Fly(127, 40);
                        }
                    }
                    if (curname == "Jump3")
                    {
                        GoToEx(96, 164);
                    }
                    return false;
                }
                else if ((TDT.ParseAllInt(curname) > TDT.ParseAllInt(nextname) && (mapId == -1 || mapId == MAP.TanHoangDiaCungTang2)) || (mapId != MAP.TanHoangDiaCungTang3 && mapId != MAP.TanHoangDiaCungTang4 && mapId != -1))
                {
                    if (curname == "Jump3")
                    {
                        if (GoToEx(127, 40, -1, true, (float)1.5))
                        {
                            TLBB.Fly(115, 39);
                        }
                    }
                    if (curname == "Jump2")
                    {
                        if (GoToEx(110, 38, -1, true, (float)1.5))
                        {
                            TLBB.Fly(99, 41);
                        }
                    }
                    if (curname != "Jump1")
                        return false;
                }
            }
            if (TLBB.MapId == MAP.TanHoangDiaCungTang1)
            {
                string curname = GetNameTanHoangDiaCung(RoundX, RoundY);
                string nextname = GetNameTanHoangDiaCung((int)x, (int)y);
                if ((TDT.ParseAllInt(curname) < TDT.ParseAllInt(nextname) && (mapId == -1 || mapId == MAP.TanHoangDiaCungTang1)) || mapId == MAP.TanHoangDiaCungTang2 || mapId == MAP.TanHoangDiaCungTang3 || mapId == MAP.TanHoangDiaCungTang4)
                {
                    if (curname == "Jump1")
                    {
                        if (GoToEx(220, 115, -1, true, (float)1.5))
                        {
                            TLBB.Fly(212, 114);
                        }
                    }
                    if (curname == "Jump2")
                    {
                        if (GoToEx(207, 114, -1, true, (float)1.5))
                        {
                            TLBB.Fly(201, 114);
                        }
                    }
                    if (curname == "Jump3")
                    {
                        if (GoToEx(195, 114, -1, true, (float)1.5))
                        {
                            TLBB.Fly(188, 114);
                        }
                    }
                    if (curname == "Jump4")
                    {
                        if (GoToEx(152, 122, -1, true, (float)1.5))
                        {
                            TLBB.Fly(144, 122);
                        }
                    }
                    if (curname == "Jump5")
                    {
                        if (GoToEx(123, 123, -1, true, (float)1.5))
                        {
                            TLBB.Fly(117, 122);
                        }
                    }
                    if (curname == "Jump6")
                    {
                        if (GoToEx(26, 159, -1, true, (float)1.5))
                        {
                            TLBB.Fly(25, 153);
                        }
                    }
                    if (curname == "Jump7")
                    {
                        if (GoToEx(25, 146, -1, true, (float)1.5))
                        {
                            TLBB.Fly(27, 137);
                        }
                    }
                    if (curname == "Jump8")
                    {
                        if (GoToEx(27, 131, -1, true, (float)1.5))
                        {
                            TLBB.Fly(27, 124);
                        }
                    }
                    if (curname == "Jump9")
                        GoToEx(25, 40);
                    return false;
                }
                if ((TDT.ParseAllInt(curname) > TDT.ParseAllInt(nextname) && (mapId == -1 || mapId == MAP.TanHoangDiaCungTang1)) || (mapId != MAP.TanHoangDiaCungTang1 && mapId != MAP.TanHoangDiaCungTang2 && mapId != MAP.TanHoangDiaCungTang3 && mapId != MAP.TanHoangDiaCungTang4 && mapId != -1))
                {
                    if (curname == "Jump2")
                    {
                        if (GoToEx(212, 114, -1, true, (float)1.5))
                        {
                            TLBB.Fly(220, 114);
                        }
                        return false;
                    }
                    if (curname == "Jump3")
                    {
                        if (GoToEx(201, 114, -1, true, (float)1.5))
                        {
                            TLBB.Fly(207, 114);
                        }
                        return false;
                    }
                    if (curname == "Jump4")
                    {
                        if (GoToEx(188, 114, -1, true, (float)1.5))
                        {
                            TLBB.Fly(195, 114);
                        }
                        return false;
                    }
                    if (curname == "Jump5")
                    {
                        if (GoToEx(144, 122, -1, true, (float)1.5))
                        {
                            TLBB.Fly(152, 122);
                        }
                        return false;
                    }
                    if (curname == "Jump6")
                    {
                        if (GoToEx(117, 122, -1, true, (float)1.5))
                        {
                            TLBB.Fly(123, 123);
                        }
                        return false;
                    }
                    if (curname == "Jump7")
                    {
                        if (GoToEx(25, 153, -1, true, (float)1.5))
                        {
                            TLBB.Fly(26, 159);
                        }
                        return false;
                    }
                    if (curname == "Jump8")
                    {
                        if (GoToEx(27, 137, -1, true, (float)1.5))
                        {
                            TLBB.Fly(25, 146);
                        }
                        return false;
                    }
                    if (curname == "Jump9")
                    {
                        if (GoToEx(27, 124, -1, true, (float)1.5))
                        {
                            TLBB.Fly(27, 131);
                        }
                        return false;
                    }
                }
            }

            if (TDT.GetDistance(x, y, CharX, CharY) <= distance && (mapId == -1 || TLBB.MapId == mapId))
            {
                ListMoveEx.Clear();
                MoveExTime = new Stopwatch();
                return true;
            }

            if (!TLBB.IsLeader)
                StopFollow();

            if (TLBB.PlayerState == 7)
            {
                if (TLBB.IsRide)
                {
                    DownRide();
                    return false;
                }
            }

            if ((mapId != -1 && TLBB.MapId != mapId) || GetDistance(x, y) > distanceUpRide)
            {
                if (!TLBB.IsRide && !TLBB.IsBienThan && !TLBB.IsTrader)
                {
                    if (TLBB.HaveRide)
                    {
                        UpRide();
                        return false;
                    }
                }
            }

            if (!IsMoveEx)
            {
                if (SwTimeOnMap.Elapsed.TotalSeconds > 10)
                {
                    if (mapId != -1 && TLBB.MapId != mapId)
                    {
                        LUA.MessageBox_Self_OK_Clicked();
                        LUA.AcceptBox_OK_Clicked();
                    }
                    FixKetMap();
                    return false;
                }
            }

            if (TLBB.PlayerState != 0 && !forceMove)
                return false;

            if (TLBB.MapId == mapId || mapId == -1 || (TLBB.IsMapChienMinh((uint)mapId) && TLBB.IsMapChienMinh(TLBB.MapId)) || (TLBB.IsMapBang((uint)mapId) && TLBB.IsMapBang(TLBB.MapId)))
            {
                Move(x, y);
            }
            else
            {
                if (PhuIndex(mapId) != -1 && !TLBB.IsTrader && !TLBB.IsBienThan)
                {
                    if (TLBB.IsRide)
                    {
                        DownRide();
                        Thread.Sleep(1000);
                    }
                    AnDon();
                    PlayerPackageUseItem(PhuIndex(mapId));
                    return false;
                }
                if (Address.GameType != 1)
                {
                    if (TLBB.IsMapBang(TLBB.MapId))
                    {
                        RaBang(mapId);
                        return false;
                    }
                }
                Move(x, y, mapId);
            }

            return false;
        }


        public string GetRoundBinhThanh(int x, int y)
        {
            if (TDT.is_point_inside_quad(new Point(x, y), new Point(0, 150), new Point(125, 165), new Point(100, 255), new Point(0, 255)))
            {
                return "Round4";
            }
            return "";
        }

        public bool IsSafeDuaLuatDiem(float x, float y)
        {
            foreach (GameObject o in Objects.All)
            {
                if (o.Name == "Địa Hỏa Phần Thiên" && o.GetDistance(x, y) <= 6)
                    return false;
            }
            if (TDT.GetDistance(x, y, 192, 195) >= 10)
                return false;
            return true;
        }

        public Point GetSafeDaLuatDiem()
        {
            for (int x = 192 - 15; x < 192 + 15; x++)
            {
                for (int y = 195 - 15; y < 195 + 15; y++)
                {
                    if (IsSafeDuaLuatDiem(x, y))
                    {
                        return new Point(x, y);
                    }
                }
            }
            return new Point(0, 0);
        }

        public bool IsMapCoMo(int mapId)
        {
            if (mapId >= 202 && mapId <= 210)
                return true;
            return false;
        }

        public string GetMapRound(int x, int y, int mapId = -1)
        {
            if (mapId == -1)
                mapId = (int)TLBB.MapId;
            if (mapId == 209) // cổ mộ 8
            {
                if (TDT.GetDistance(x, y, 73, 25) <= 3)
                    return "Jump1";
                if (TDT.GetDistance(x, y, 66, 23) <= 3)
                    return "Jump2";
                if (TDT.GetDistance(x, y, 46, 21) <= 15)
                    return "Jump3";
                return "Jump0";
            }
            if (mapId == MAP.ThanhHoaCung)
            {
                if (TDT.is_point_inside_quad(new Point(x, y), new Point(24, 79), new Point(28, 79), new Point(28, 88), new Point(24, 88)))
                    return "Jump2";
                if (y > 88 && x < 36)
                    return "Jump1";
                return "Jump3";
            }
            return "";
        }




        private uint BTDIndex = 0;
        private bool ispack = false;

        public List<string> ListPointBaoDoHiem { get; set; } = new List<string>();



        public void MoBTD()
        {
            if (Global.Paused || TLBB.PlayerState == 8)
                return;
            if (TLBB.MapId == MAP.HuyetMo)
            {
                MissionState = "";
                MissionX = MissionY = MissionMap = 0;
                BTDIndex = 0;
                if (Global.IsSmartBTD)
                {
                    PushDebugMessageEx("Tự rời huyệt mộ nếu hết chìa khóa");
                    if (!ispack)
                    {
                        LUA.PackUp();
                        Thread.Sleep(350);
                        ispack = true;
                    }
                    if (ForcePickItem())
                        return;
                    if (Objects.Monters.Count > 0)
                    {
                        DownRide();
                        ForceAttack();
                    }
                    else
                    {
                        if (HaveItem("chiakhoabaoruong"))
                        {
                            GameObject picki = null;
                            if (CharX <= 37)
                                picki = Objects.All.Where(o => o.X <= 37 && o.Name == "Rương bảo vật" && !ListPickedIds.Contains((int)o.Id)).OrderBy(o => o.Distance).FirstOrDefault();
                            else
                                picki = Objects.All.Where(o => o.X > 37 && o.Name == "Rương bảo vật" && !ListPickedIds.Contains((int)o.Id)).OrderBy(o => o.Distance).FirstOrDefault();
                            if (picki != null)
                            {
                                if (picki.RoundX == 35 && picki.RoundY == 42)
                                {
                                    picki.RoundX = 34;
                                    picki.RoundY = 40;

                                }
                                if (GoToEx(picki.RoundX, picki.RoundY))
                                {
                                    if (TLBB.IsRide)
                                    {
                                        DownRide();
                                        Thread.Sleep(100);
                                    }
                                    PickItem((int)picki.Id);
                                    if (TrueStandTime.Elapsed.TotalSeconds > 4)
                                    {
                                        FixKetMap();
                                    }
                                    return;
                                }
                            }
                            if (TrueStandTime.Elapsed.TotalSeconds > 1.5)
                            {
                                if (TrueStandTime.Elapsed.TotalSeconds > 3)
                                {
                                    FixKetMap();
                                }
                                else
                                {
                                    MoveNext();
                                }
                            }
                        }
                        else
                        {
                            if (TrueStandTime.Elapsed.TotalSeconds > 2)
                                GoToEx(54, 48);
                        }
                    }
                }
                return;
            }
            ispack = false;
            if (MissionState == "")
            {
                if (!HaveItem("Merchandise4_16"))
                {
                    foreach (PacketItem packetItem in PacketItems.ThienCo)
                    {
                        if (packetItem.Type == "Merchandise4_16")
                        {
                            GetItemThienCo(packetItem.Index);
                            Thread.Sleep(350);
                            LUA.PackUp();
                            return;
                        }
                    }
                    PushDebugMessage("Không có bảo tàng đồ. Dừng auto");
                    RemoveMission(MissionsType.MoBaoTangDo);
                    return;
                }
                foreach (var item in PacketItems.All)
                {
                    if (item.Type == "Merchandise4_16")
                    {
                        item.Use();
                        Thread.Sleep(350);
                        BTDIndex = item.Index;
                        if (TLBB.IsQuestOpen)
                        {
                            MissionInfo = QuestFrame.All(this);
                            MissionMap = TDT.GetMapId(MissionInfo);
                            if (MissionMap > 0)
                            {
                                MissionInfo = Regex.Replace(MissionInfo, ".*_INFOAIM", "");
                                MissionX = TDT.ParseInt(MissionInfo);
                                if (MissionInfo.Split(',').Length > 1)
                                {
                                    MissionY = TDT.ParseInt(MissionInfo.Split(',')[1]);
                                }
                            }
                            if (MissionX != -1 && MissionY != -1 && MissionMap > 0)
                            {
                                if (MissionX == 110 && MissionY == 50 && MissionMap == MAP.VoDi)
                                {
                                    MissionX = 111;
                                    MissionY = 52;
                                }
                                MissionState = "Do";
                            }
                            else
                            {
                                MissionState = "";
                            }
                        }
                        return;
                    }
                }
            }
            if (MissionState == "Do")
            {
                if (GoToEx(MissionX, MissionY, MissionMap))
                {
                    foreach (var item in PacketItems.All)
                    {
                        if (item.Index == BTDIndex && item.Type == "Merchandise4_16")
                        {
                            item.Use();
                            break;
                        }
                    }
                    MissionState = "";
                }
                return;
            }
            MissionState = "";
        }





        public void SinhTieu()
        {
            if (!Global.IsSinhTieu)
                return;
            if (TLBB.MapId != MAP.ConGiapLoiDai)
            {
                PushDebugMessage("Vui Lòng Vào Lôi Đài Sinh Tiêu. Dừng auto");
                RemoveMission(MissionsType.SinhTieu);
            }
            if (!IsOneSec)
                return;
            if (ForcePickItem())
                return;
            ForceAttack();
            bool IsKhieuChien = false;
            foreach (GameObject obj in Objects.All)
            {
                if (obj.Menpai == 10)
                {
                    IsKhieuChien = true;
                    break;
                }
            }
            if (IsKhieuChien)
            {
                if (GoToEx(40, 41))
                {
                    Talk("cattuongmieu");
                    Thread.Sleep(300);
                    QuestFrame.Click("#{SXLT_100119_25}");
                    Thread.Sleep(300);
                    QuestFrame.Click("#{SXLT_100119_30}");
                }
            }
            if (swStandTime.Elapsed.TotalSeconds >= 2)
            {
                if (GoToEx(40, 41))
                {
                    Talk("cattuongmieu");
                    Thread.Sleep(300);
                    QuestFrame.Click("#{SXLT_100119_36}");
                    Thread.Sleep(300);
                    QuestFrame.Click("#{SXLT_100119_30}");
                }
            }
        }

        public void TruAc()
        {
            if (!IsOneSec)
                return;
            if (IsOneTNT)
            {
                if (SecCount % 10 == 0)
                {
                    UncheckTueHong();
                }
            }
            if (string.IsNullOrEmpty(TLBB.MapName.Trim()))
                return;
            if (!TDT.VietLien(TLBB.MapName).Contains("thienkieplau"))
            {
                if (GoToEx(DAILY.PhoKiepSinh))
                {
                    if (TLBB.IsQuestOpen)
                    {
                        QuestFrame.Click("#{TJL_xml_XX(02)}");
                    }
                    else
                    {
                        Talk(DAILY.PhoKiepSinh);
                    }
                }
            }
            else
            {
                MissionState = string.Empty;
                foreach (GameTask task in GameTask.Enum(this))
                {
                    if (task.ClearName == "truacthienkieplau")
                    {
                        if (task.Completed)
                        {
                            MissionState = "Done";
                        }
                        else
                        {
                            MissionState = "Do";
                        }
                    }
                }
                if (MissionState == "Do")
                {
                    if (TLBB.IsRide)
                        DownRide();
                    if (TLBB.IsTogleMission)
                    {
                        PostMessage(18, 105);
                    }
                    ForceAttack();
                }
                else if (MissionState == "Done")
                {
                    if (GoToEx(73, 79))
                    {
                        if (TLBB.IsQuestOpen)
                        {
                            QuestFrame.Click("#{TJL_xml_XX(01)}");
                            Sleep(1000);
                            LUA.QuestFrameMissionComplete();
                            MissionState = string.Empty;
                            QuestFrame.Close();
                        }
                        else
                        {
                            Talk("phokiepsinh");
                        }
                    }
                }
                else if (MissionState == string.Empty)
                {
                    if (GoToEx(73, 79))
                    {
                        if (TLBB.IsQuestOpen)
                        {
                            QuestFrame.Click(1);
                            Sleep(300);
                            QuestFrameAccept();
                            if (!TLBB.IsTogleMission)
                            {
                                PostMessage(18, 105);
                            }
                            QuestFrame.Close();
                            return;
                        }
                        else
                        {
                            Talk("phokiepsinh");
                        }
                    }
                }
            }
        }

        public void TrungAc()
        {
            if (ForcePickItem())
                return;
            if (TLBB.PlayerState != 0)
                return;

            if (TLBB.IsODaoCuFullTrungAc)
            {
                if (Global.IsSmartBTD)
                {
                    PushMissions(MissionsType.MoBaoTangDo);
                    return;
                }
            }

            if (TrueStandTime.Elapsed.TotalSeconds >= 15)
            {
                bool havetask = false;
                bool havelenhbai = false;
                foreach (var task in GameTask.Enum(this))
                {
                    if (task.Name == "#{CXDT_090304_01}")
                    {
                        if (!task.Completed)
                        {
                            havetask = true;
                        }
                    }
                }
                if (havetask)
                {
                    foreach (var item in PacketItems.All)
                    {
                        if (item.Name.Contains("Trừng Ác Lệnh"))
                        {
                            havelenhbai = true;
                        }
                    }
                }
                if (havetask && !havelenhbai)
                {
                    if (TrueStandTime.Elapsed.TotalSeconds >= 60)
                    {
                        LUA.DeleteMission("#{CXDT_090304_01}");
                    }
                    else
                    {
                        PushDebugMessageEx("Hình như hỏng nhiệm vụ trừng ác. Tự hủy sau " + (int)(60 - TrueClearMonterTime.Elapsed.TotalSeconds) + "s");
                    }
                }
            }

            if (State == STATE.Done || State == STATE.Null)
            {
                foreach (var item in PacketItems.All)
                {
                    if (item.Name.Contains("Trừng Ác Lệnh"))
                    {
                        State = STATE.None;
                        return;
                    }
                }
                if (GoToEx(TOCHAU.NgoGioi))
                {
                    Talk(TOCHAU.NgoGioi);
                    Thread.Sleep(1000);
                    QuestFrame.Click("#{CXDT_090304_01}");
                    Thread.Sleep(1000);
                    if (QuestFrame.All(this).Contains("#{CXDY_090423_01}") && QuestFrame.All(this).Contains("#{CXDY_090423_02}"))
                    {
                        if (IsByLogin)
                            IsXongBHD = true;
                        RemoveMission(MissionsType.TrungAc);
                        SaveSetting();
                        return;
                    }
                    LUA.QuestFrameMissionContinue();
                    Thread.Sleep(1000);
                    LUA.QuestFrameMissionComplete();
                    Thread.Sleep(350);
                    Talk(TOCHAU.NgoGioi);
                    Thread.Sleep(1000);
                    QuestFrame.Click("#{CXDT_090304_01}");
                    Thread.Sleep(1000);
                    if (QuestFrame.All(this).Contains("#{CXDY_090423_01}") && QuestFrame.All(this).Contains("#{CXDY_090423_02}"))
                    {
                        if (IsByLogin)
                            IsXongBHD = true;
                        RemoveMission(MissionsType.TrungAc);
                        SaveSetting();
                        return;
                    }
                    LUA.QuestFrameAcceptClicked();
                    State = STATE.None;
                }
                return;
            }
            if (State == STATE.None)
            {
                foreach (var item in PacketItems.All)
                {
                    if (item.Name.Contains("Trừng Ác Lệnh"))
                    {
                        item.Use();
                        Thread.Sleep(1000);
                        MissionInfo = QuestFrame.First(this);
                        MissionMap = TDT.GetMapId(MissionInfo);
                        if (MissionMap > 0)
                        {
                            if (MissionInfo.Split('*').Length > 4)
                            {
                                MissionX = TDT.ParseInt(MissionInfo.Split('*')[3]);
                                MissionY = TDT.ParseInt(MissionInfo.Split('*')[4]);
                                State = STATE.Do;
                                return;
                            }
                        }
                        return;
                    }
                }
                if (!TLBB.IsTogleMission)
                {
                    LUA.TOGLE_MISSION();
                    Thread.Sleep(1000);
                }
                LUA.CLOSE_MISSION();

                foreach (var task in GameTask.Enum(this))
                {
                    if (task.Name == "#{CXDT_090304_01}")
                    {
                        if (task.Completed)
                        {
                            State = STATE.Done;
                            return;
                        }
                    }
                }
                State = STATE.Null;
                return;
            }
            if (State == STATE.Do)
            {
                if ((!ForcePickItem() && !TLBB.IsCaptcha))
                {
                    foreach (var task in GameTask.Enum(this))
                    {
                        if (task.Name == "#{CXDT_090304_01}")
                        {
                            if (task.Completed)
                            {
                                State = STATE.Done;
                                return;
                            }
                        }
                    }
                }
                if (TrueStandTime.Elapsed.TotalSeconds >= 20)
                {
                    foreach (var task in GameTask.Enum(this))
                    {
                        if (task.Name == "#{CXDT_090304_01}")
                        {
                            if (task.Completed)
                            {
                                State = STATE.Done;
                                return;
                            }
                        }
                    }
                    State = STATE.None;
                }

                if (Global.IsXaPhu && TDT.GetTruyen(MissionInfo) != -1 && (TLBB.MapId == LACDUONG.Id || TLBB.MapId == 1 || TLBB.MapId == 2 || TLBB.MapId == 246))
                {
                    if (TLBB.MapId == LACDUONG.Id)//lacduong
                    {
                        if (TDT.GetDistance(CharX, CharY, 218, 337) > 1)
                        {
                            GoToEx(218, 337);
                            return;
                        }
                    }
                    else if (TLBB.MapId == 1)//tochau
                    {
                        if (TDT.GetDistance(CharX, CharY, 335, 250) > 1)
                        {
                            GoToEx(335, 250);
                            return;
                        }
                    }
                    else if (TLBB.MapId == 2)//daily
                    {
                        if (TDT.GetDistance(CharX, CharY, 258, 82) > 1)
                        {
                            GoToEx(258, 82);
                            return;
                        }
                    }
                    else if (TLBB.MapId == 246)//laulan
                    {
                        if (TDT.GetDistance(CharX, CharY, 298, 87) > 1)
                        {
                            GoToEx(298, 87);
                            return;
                        }
                    }
                    foreach (GameObject _object in Objects.All)
                    {
                        if (TDT.VietLien(_object.Name).Contains("xatruyen"))
                        {
                            Talk(_object.Id);
                            Thread.Sleep(1000);
                            int Extra1 = 0;
                            if (TLBB.MapId == LACDUONG.Id)//lacduong
                            {
                                Extra1 = 400956;
                            }
                            else if (TLBB.MapId == 1)//tochau
                            {
                                Extra1 = 400957;
                            }
                            else if (TLBB.MapId == 2)//daily
                            {
                                Extra1 = 400958;
                            }
                            else if (TLBB.MapId == 246)//laulan
                            {
                                Extra1 = 400959;
                            }
                            QuestFrame.Click(Extra1, TDT.GetTruyen(MissionInfo));
                            Thread.Sleep(1000);
                            QuestFrame.Click("dongy");
                            return;
                        }
                    }
                }
                if (MissionX == 110 && MissionY == 50 && MissionMap == MAP.VoDi)
                {
                    MissionX = 111;
                    MissionY = 52;
                }

                if (GoToEx(MissionX, MissionY, MissionMap, false, 3))
                {
                    foreach (var item in PacketItems.All)
                    {
                        if (item.Name.Contains("Trừng Ác Lệnh"))
                        {
                            item.Use();
                            return;
                        }
                    }
                    foreach (GameObject _object in Objects.Monters.Where(o => o.Distance <= 20))
                    {
                        if (_object.Title != "" && _object.Menpai == 28 && TDT.NumDiff((int)_object.Lvl, (int)TLBB.Lvl) <= 5)
                        {
                            if (TLBB.IsRide)
                            {
                                DownRide();
                                return;
                            }
                            SelectTarget((int)_object.Id);
                            SendKey(Global.BaseSkill);
                            return;
                        }
                    }
                }
                return;
            }
            State = STATE.Null;
        }





        public int BHDCount { get; set; }




        private bool isCountMam;


        public static int TotalXongBHD { get; set; }
        public static Stopwatch swTotalXongBHD { get; set; } = Stopwatch.StartNew();

        public void AskTeamFollow()
        {
            PostMessage(21, 105);
        }

        public void StopFollow()
        {
            if (!TLBB.IsFollow)
                return;
            PostMessage(20, 105);
        }



        public void ChucPhucHaoHuu()
        {
            if (Address.GameType != 1)
                return;
            try
            {
                TuChucPhuc();
                DoStringEx("local online = ''; for i = 1 , 8 do local j = i; if i == 8 then j = 13; end local relealFriendNumber = DataPool:GetFriendNumber( tonumber(j) ); local index=0; while index < relealFriendNumber  do local guid =  DataPool:GetFriend( tonumber(j), tonumber(index), 'ID' ); if( DataPool:GetFriend( tonumber(j), tonumber(index), 'ONLINE' ) ) then online = online .. guid .. '-'; end index = index + 1; end end return online;");
                string online = LuaToStringBanBe();
                Thread.Sleep(500);
                online = LuaToStringBanBe(true);
                foreach (string on in online.Split('-'))
                {
                    if (on.Trim().Length == 18)
                    {
                        ChucPhuc(on);
                    }
                }
                DoStringEx("setmetatable(_G, {__index = Sns_Env}); if this:IsVisible() then	this:Hide(); end");
            }
            catch { }
        }


        public static bool IsHoaBonMine(string title)
        {
            if (Global.IsBonKhacMay)
                return true;

            //if (Global.IsAdminEx || Global.IsKS)
            //    return true;
            if (string.IsNullOrEmpty(title.Trim()))
                return false;
            //#{SDJZH_32}
            if (title.IndexOf("#{SDJZH_32}") > 0)
            {
                string name = title.Replace("#{SDJZH_32}", "");
                return ListNames.Contains(name);
            }
            if (title.IndexOf("#{SDJZH") > 0)
            {
                string name = title.Substring(0, title.IndexOf("#{SDJZH"));
                return ListNames.Contains(name);
            }
            else
            {
                return false;
            }
            //foreach(string n in ListNames)
            //{
            //    if (title.Contains(n))
            //        return true;
            //}
            //return false;
        }

        public static bool IsHoaMine(string title)
        {
            //if (Global.IsAdminEx || Global.IsKS)
            //    return true;
            if (Global.IsAdminEx)
                return true;
            if (string.IsNullOrEmpty(title.Trim()))
                return false;
            //#{SDJZH_32}
            if (title.IndexOf("#{SDJZH_33}") > 0)
            {
                string name = title.Replace("#{SDJZH_33}", "");
                return ListNames.Contains(name);
            }
            if (title.IndexOf("#{SDJZH") > 0)
            {
                string name = title.Substring(0, title.IndexOf("#{SDJZH"));
                return ListNames.Contains(name);
            }
            else
            {
                return false;
            }
            //foreach(string n in ListNames)
            //{
            //    if (title.Contains(n))
            //        return true;
            //}
            //return false;
        }

        public string hashHoaDialog = string.Empty;

        public Stopwatch swthuhoa = Stopwatch.StartNew();
        public int hoabatdau = 0;


        public void ResetHoaSpee()
        {
            hoabatdau = 0;
            hoahientai = 0;
            swthuhoa = Stopwatch.StartNew();
            swcounthoa = Stopwatch.StartNew();
            kttbatdau = 0;
            ktthientai = 0;
            ktttime = Stopwatch.StartNew();
            swcountktt = Stopwatch.StartNew();
        }

        public int kttbatdau = 0;
        public int ktthientai = 0;
        public Stopwatch ktttime = Stopwatch.StartNew();
        public Stopwatch swcountktt = Stopwatch.StartNew();

        public int HoaSpeeI
        {
            get
            {
                return (int)((double)(hoahientai - hoabatdau) / swthuhoa.Elapsed.TotalSeconds * 3600);
            }
        }

        public int KTTI
        {
            get
            {
                return (int)((double)(ktthientai - kttbatdau) / ktttime.Elapsed.TotalSeconds * 3600);
            }
        }

        public string HoaKTTSpeed
        {
            get
            {
                return TDT.FormatMoney(KTTI) + " KTT | " + TDT.FormatMoney(HoaSpeeI) + " Hoa";
            }
        }

        public string HoaSpeed
        {
            get
            {
                int totalhoa = 0;
                int totalktt = 0;
                foreach (Game game in Main.Instance.AllOnelineGame)
                {
                    totalhoa += game.HoaSpeeI;
                    totalktt += game.KTTI;
                }
                //  Main.Instance.AllOnelineGame.Where(g => )
                return "Bắt đầu " + TDT.FormatMoney(hoabatdau) + " Hoa\r\n" +
                    "Hiện tại " + TDT.FormatMoney(hoahientai) + " Hoa\r\n" +
                    TDT.FormatMoney(HoaSpeeI) + " Hoa / h\r\n( " + TDT.FormatMoney(hoahientai - hoabatdau) + " / " + (int)swthuhoa.Elapsed.TotalMinutes + " min )\r\n\r\n"
                    + "Bắt đầu " + TDT.FormatMoney(kttbatdau) + " KTT\r\n" +
                         "Hiện tại " + TDT.FormatMoney(ktthientai) + " KTT\r\n" +
                      TDT.FormatMoney(KTTI) + " KTT / h\r\n( " + TDT.FormatMoney(ktthientai - kttbatdau) + " / " + (int)ktttime.Elapsed.TotalMinutes + " min )\r\n"
                      + "--------\r\n"
                      + TDT.FormatMoney(totalhoa) + " Hoa / h\r\n"
                      + TDT.FormatMoney(totalktt) + " KTT / h\r\n"
                      ;
            }
        }

        private Stopwatch swcounthoa = Stopwatch.StartNew();
        public int hoahientai = 0;

        public static List<GameObject> AllPlayer
        {
            get
            {
                List<GameObject> players = new List<GameObject>();
                foreach (var game in Main.Instance.AllOnelineGame)
                {
                    players = players.Concat(game.Objects.Players).ToList();
                }
                players = players.GroupBy(o => o.Id).Select(g => g.FirstOrDefault()).ToList();
                return players;
            }
        }


        public static List<GameObject> AllKSPlayer
        {
            get
            {
                List<GameObject> players = new List<GameObject>();
                foreach (var game in Main.Instance.AllOnelineGame.Where(g => g.HaveBienThan && g.TrueStandTime.Elapsed.TotalSeconds > 1))
                {
                    GameObject o = game.Objects.Players.Where(p => p.IsKs && players.Where(z => z.Id == p.Id).Count() == 0).FirstOrDefault();
                    if (o != null)
                        players.Add(o);
                }
                return players;
            }
        }


        public List<string> AllOtherCarePoint
        {
            get
            {
                return Main.Instance.AllOnelineGame.Where(g => g != this && g.CareX != 0).Select(g => g.CareX + "," + g.CareY).ToList();
            }
        }

        public List<uint> AllOtherCareKTT
        {
            get
            {
                return Main.Instance.AllOnelineGame.Where(g => g != this).Select(g => (uint)g.TakenKTT).ToList();
            }
        }

        public List<uint> AllOtherCareId
        {
            get
            {
                return Main.Instance.AllOnelineGame.Where(g => g != this && g.CareObject != null).Select(g => g.CareObject.Id).ToList();
            }
        }

        //public List<GameObject> AllPartyObjects
        //{
        //    get
        //    {
        //        return Main.Instance.AllOnelineGame.Select(g => g.Objects).GroupBy(o => o.id).ToList();
        //    }
        //}

        public IEnumerable<uint> AllPartyTargetId => Party.Where(p => p != this && p.TLBB.State != 9).Select(p => (uint)p.TargetId);

        public GameObject CareObject { get; set; }
        public int CareX { get; set; }
        public int CareY { get; set; }

        //public HashSet<string>









        public void DungThoLinhChau()
        {
            if (!Missions.Contains(MissionsType.DungThoLinhChau))
                return;
            RemoveMission(MissionsType.DungThoLinhChau);
            StopFollow();
            Thread.Sleep(100);
            DownRide();
            Thread.Sleep(350);
            foreach (var item in PacketItems.All)
            {
                if (item.MapId > 2 && item.MapId != 0xFFFFFFFF)
                {
                    item.Use();
                    Thread.Sleep(200);
                    break;
                }
            }
        }

        public void DungDinhViPhu()
        {
            if (!Missions.Contains(MissionsType.DungDinhViPhu))
                return;

            RemoveMission(MissionsType.DungDinhViPhu);
            StopFollow();
            Thread.Sleep(350);
            if (TLBB.IsRide)
            {
                DownRide();
            }
            Thread.Sleep(350);
            foreach (var item in PacketItems.All)
            {
                if (item.MapId <= 2 && item.X > 0)
                {
                    PlayerPackageUseItem((int)item.Index);
                    Thread.Sleep(200);
                    break;
                }
            }
        }

        public void Refresh()
        {
            SetNull();
            SaveSetting();
            PushDebugMessage("Refresh");
            SkillLoaded?.Invoke(this, null);
            if (AccountEx != null)
                AccountEx.SetNull();
        }

        private void AutoExitGame()
        {
            if (Global.IsOutIdle && TLBB.Online && TLBB.OnlineTimeSec >= 60 && TrueStandTime.Elapsed.TotalMinutes >= Global.OutIdleMin)
            {
                try
                {
                    if (AccountEx != null)
                    {
                        AccountEx.game = null;
                        AccountEx.Status = "...";
                    }
                    if (Global.IsChonMayChuThayChoThoatGame)
                    {
                        Quit();
                    }
                    else
                    {
                        Process.Kill();
                    }
                }
                catch { }
            }
            if (!TLBB.Online)
            {
                if (Setting.Is("checkExitLogin"))
                {
                    if (swLoginTime.Elapsed.TotalMinutes > Global.OutLogin)
                    {
                        Process.Kill();
                    }
                }
            }
            else
            {
                if (MicroLogin.IsExit30)
                {
                    if (TLBB.Lvl >= MicroLogin.LvExit)
                    {
                        IsXongBHD = true;
                    }
                }
                if (Global.IsOutOnline && !IsMapPhuBan() && SwTimeOnMap.Elapsed.TotalSeconds > 10)
                {
                    if (MicroLogin.IsNotOutKey && TLBB.IsLeader)
                    {
                    }
                    else
                    {
                        if ((float)TLBB.OnlineTimeSec / 60 > Global.OutOnlineMin)
                        {
                            try
                            {
                                if (!MicroLogin.IsLogLai)
                                {
                                    try
                                    {
                                        if (AccountEx != null)
                                        {
                                            AccountEx.game = null; // exacly
                                            AccountEx.Status = "...";
                                        }
                                    }
                                    catch { }
                                }
                                if (Global.IsChonMayChuThayChoThoatGame)
                                {
                                    Quit();
                                }
                                else
                                {
                                    Process.Kill();
                                }
                            }
                            catch { }
                        }
                    }
                }
            }
        }

        private uint lasmapid;

        private void DefineVar()
        {
            if (TLBB.PlayerState == 9)
                ListPickedIds.Clear();
            SetPartyIndex();
            if (Objects.Monters.Count > 0)
                TrueClearMonterTime = Stopwatch.StartNew();
            if (TLBB.PlayerState != 0)
                TrueStandTime = Stopwatch.StartNew();
            if (TLBB.PlayerState == 2)
                TrueNotMoveTime = Stopwatch.StartNew();
            if (TLBB.Online)
            {
                if (Global.IsTuVaoPhai)
                {
                    if (TLBB.Lvl < 30)
                    {
                        PushMissions(MissionsType.NhiemVuThangCap);
                    }
                    SetMenPai = Global.GlSetMenPai;
                    IsSetMenPai = true;
                }
                if (IsDead && TLBB.MapId == LACDUONG.Id && TLBB.HPPercent > 0 && TLBB.HPPercent < 40)
                {
                    PushMissions(MissionsType.TriLieu);
                }
            }
            ResetExpSpeed();

            foreach (var _object in Objects.All)
            {
                string name = _object.Name;
                if (_object.CleanName == "whiskered")
                    name = "Cưu Ma Trí";
                if (_object.CleanName == "furong")
                    name = "Mộ Dung Phục";
                if (_object.CleanName == "crueltuan")
                    name = "Đoàn Diên Khánh";
                if (_object.HP == 0 || (TLBB.MapId == MAP.TuTuyetTrang && _object.Menpai == 10))
                {
                    if (!DeadObjects.ContainsKey(_object.Name))
                        DeadObjects[_object.Name] = Stopwatch.StartNew();
                }
                else
                {
                    if (_object.IsMonter)
                        DeadObjects.Remove(_object.Name);
                }
            }
            if (TLBB.MapId == MAP.ToChau)
            {
                DeadObjects.Clear();
            }

            // here ok
            if (SecCount % 10 == 0)
            {
                if (!IsMapPhuBan())
                {
                    if (!Missions.Contains(MissionsType.TriLieu))
                    {
                        if (Setting.Is("checkTriLieuHP"))
                        {
                            if (TLBB.HPPercent <= Setting.Value("numberTriLieuHP") && TLBB.HP > 0)
                            {
                                NeedToMove = RoundX + "," + RoundY + "," + TLBB.MapId;
                                PushMissions(MissionsType.TriLieu);
                            }
                        }
                        if (Setting.Is("checkTriLieuMP"))
                        {
                            if (TLBB.MPPercent < 5)
                            {
                                NeedToMove = RoundX + "," + RoundY + "," + TLBB.MapId;
                                PushMissions(MissionsType.TriLieu);
                            }
                        }
                    }
                    if (!Missions.Contains(MissionsType.CatVang))
                    {
                        if (Setting.Is("checkCatDoDayTayNai"))
                        {
                            if (TLBB.IsODaoCuFull || TLBB.IsONguyenLieuFull)
                            {
                                NeedToMove = RoundX + "," + RoundY + "," + TLBB.MapId;
                                PushMissions(MissionsType.CatVang);
                            }
                        }
                    }
                    if (!Missions.Contains(MissionsType.BanRac))
                    {
                        if (Setting.Is("checkBanDoDayTayNai"))
                        {
                            if (TLBB.IsODaoCuFull || TLBB.IsONguyenLieuFull)
                            {
                                NeedToMove = RoundX + "," + RoundY + "," + TLBB.MapId;
                                PushMissions(MissionsType.BanRac);
                            }
                        }
                    }
                }
            }

            DemMam();

            if (TLBB.PlayerState == 9)
            {
                if(QuanDoans.Where(game => game.TLBB.IsDead).Count() == 12)
                {
                    QuanDoans.ForEach(game => game.isdauthai = true);
                }
                if(Party.Where(game => game.TLBB.IsDead).Count() == 6)
                {
                    Party.ForEach(game => game.isdauthai = true);
                }
            }

            if (TLBB.MapId == MAP.BinhThanhKyTran)
            {
                if (Party.Concat(QuanDoans).Where(g => g.TLBB.PlayerState != 9).Count() == 0)
                {
                    Party.ForEach(g => { g.StepBinhThanh = 0; g.isdauthai = true; });
                    QuanDoans.ForEach(g => { g.StepBinhThanh = 0; ; g.isdauthai = true; });
                }
            }
            if (Objects.NearMonter12m.Count > 0)
                ClearTime = Stopwatch.StartNew();
            if (SecCount % 100 == 0)
                Enemy = string.Empty;

            if (DeadObjects.Where(o => o.Key == "Lý Thu Thủy" && o.Value.Elapsed.TotalSeconds >= 15).Count() == 0)
            {
                if (IsOut)
                {
                    IsOut = false;
                    DoStringEx("IsOut = false;");
                }
            }
            else
            {
                if (!IsOut)
                {
                    IsOut = true;
                    DoStringEx("IsOut = true;");
                }
            }

            if (IsOneSec)
            {
                PostMessage(34, 105);
                string tmp = Enemy;
                Enemy = LuaToStringMemo("GetEnemy");
                List<string> replaces = TDT.ListStringBetween(Enemy, "@", "-");

                foreach (string s in replaces)
                {
                    Enemy = Enemy.Replace("@" + s + "-", "-");
                }

                //Enemy = Regex.Replace(Enemy, "@[^-]+-", "-");
                foreach (string name in tmp.Split('-'))
                {
                    if (name.Length >= 3)
                    {
                        if (!Enemy.Contains(name))
                            Enemy += "-" + name + "-";
                    }
                }
                foreach (string e in Enemy.Split('-'))
                {
                    if (e.Length >= 2)
                    {
                        if (!Enemys.ContainsKey(e))
                        {
                            Enemys.Add(e, DateTime.Now);
                        }
                    }
                }
                Enemys = Enemys.Where(kvp => (DateTime.Now - kvp.Value).TotalSeconds < 560).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            }

            if (SecCount % 10 == 0)
            {
                DoStringEx("return TXT;");
                GameRaoTXT = LuaToStringMemo("RaoTxt");
            }
            if (TLBB.MapId == MAP.TranLongKyCuoc || TLBB.MapId == MAP.LauLanBaoTang || TLBB.MapId == MAP.ThanhThuSonPhuBan)
            {
                if (TLBB.IsRide && Objects.Monters.Count > 0)
                    DownRide();
                if (!IsBossDie)
                {
                    foreach (GameObject _object in Objects.All.Where(o => o.HP == 0))
                    {
                        if (_object.CleanName == "viencokyhon")
                        {
                            IsBossDie = true;
                            BossDieTime = Stopwatch.StartNew();
                        }
                        if (_object.CleanName == "tranbaolongvuong")
                            IsXongTamBao = true;
                    }
                }
            }
            if (TLBB.MapId == MAP.HuyenVuDaoPhuBan)
            {
                if (!IsBossDie)
                {
                    foreach (GameObject _object in Objects.All)
                    {
                        if (_object.CleanName == "vodichphithienmieu" && _object.HP == 0)
                        {
                            IsBossDie = true;
                            BossDieTime = Stopwatch.StartNew();
                        }
                    }
                }
            }
            if (TLBB.MapId == MAP.ThanhThuSonPhuBan)
            {
                if (!IsBossDie)
                {
                    foreach (GameObject _object in Objects.All)
                    {
                        if (_object.CleanName == "datrudaumuc" && _object.HP == 0)
                        {
                            IsBossDie = true;
                            BossDieTime = Stopwatch.StartNew();
                        }
                    }
                }
            }
            if (SecCount % 15 == 0 && Address.GameType == 1)
            {
                DoStringEx("info = ''; setmetatable(_G, { __index = MainMenuBar_Env}); if MainMenuBar_EXP3:IsVisible() then info = info..'1,'; else info = info..'0,'; end wuqi, fangju, wuju, longwen, lingpai, haoxiayin, wuhun, zhenshou = LifeAbility:GetEquipRepairInfo(); total = wuqi + fangju + wuju + longwen + lingpai + haoxiayin + wuhun + zhenshou; info = info..total; return info;");
                string info = LuaToStringMemo("InfoDo");
                if (info.StartsWith("1,"))
                {
                    IsX2 = true;
                }
                else
                {
                    IsX2 = false;
                }
                if (info.Split(',').Length >= 2)
                {
                    TongDoHong = TDT.ParseAllInt(info.Split(',')[1]);
                }
            }

            if (SecCount % 15 == 0)
            {
                IntPtr h = Win.GetHandle(ProcessId, "TianLongBaBu WndClass");
                if (h == IntPtr.Zero)
                    h = Win.GetHandle(ProcessId, "kezgxubdpydrpeb");
                if (h != IntPtr.Zero)
                    Handle = h;
                IntPtr handle = Win.GetHandle(ProcessId, "#32770");
                if (handle != IntPtr.Zero)
                    Win.Close(handle);
                handle = Win.FindWindow(null, "CrashReport");
                if (handle != IntPtr.Zero)
                    Win.Close(handle);
            }
            ExpGain = (int)TLBB.Exp - (int)ExpStart;
            ExpSpeed = (float)(ExpGain / AutoTime.Elapsed.TotalHours);
            if (Objects.Monters.Count > 0)
                swClearMontersTime = Stopwatch.StartNew();
            if (TLBB.MapId == MAP.LauLanBaoTang)
            {
                if (IsOneSec)
                {
                    DoStringEx("return IsDone;");
                    string IsDone = LuaToStringMemo("IsDone");
                    if (TDT.GetDistance(65, 65, RoundX, RoundY) > 5)
                    {
                        if (IsDone == "Done")
                        {
                            if (TDT.GetDistance(65, 65, CharX, CharY) >= 5)
                                TLBB.Fly(65, 65);
                            PushDebugMessage("Bay Thôi");
                        }
                    }
                }
            }
            if (Objects.Monters.Count > 0)
            {
                ClearMonterTimeEx = Stopwatch.StartNew();
            }
            GETQDINFO();
            if (string.IsNullOrEmpty(TLBB.MapName) || TLBB.MapName.Contains(","))
            {
                if (TLBB.MapId == MAP.BienKinh)
                {
                    TLBB.MapName = "Biện Kinh";
                }
                else if (TLBB.MapId == MAP.CoDongTuocDai)
                {
                    TLBB.MapName = "Cổ Đồng Tước Đài";
                }
                else
                {
                    LUA.DoString("return GetCurrentSceneName();");
                    TLBB.MapName = LuaToStringMemo("MapName");
                }
            }
            if (TLBB.PlayerState == 0 || TLBB.PlayerState == 9)
            {
                if (IsOneSec)
                {
                    IdleTime++;
                }
            }
            else
            {
                swStandTime = Stopwatch.StartNew();
                IdleTime = 0;
            }
            if (TLBB.SafeTime > 0)
            {
                IsOpenPass2 = false;
                SafeTime = 0;
            }
            IsWriteKhongChiemMan = false;
            if (TLBB.Online)
                swLoginTime = Stopwatch.StartNew();
            if (TLBB.MapId == MAP.TangKinhCac && !Setting.Is("checkAlarmInbox"))
            {
                DoStringEx("return TKCINFO;");
                string info = LuaToStringMemo("TKCInfo");
                if ((info.Contains("#{CJG_090413_21}") || info.Contains("#{CJG_090605_4}")) && TLBB.IsLeader)
                {
                    Party.ForEach(g =>
                    {
                        {
                            if (!IsSameInfo(info, g.TKCInfo))
                            {
                                g.TKCInfo = info;
                                g.TKCStateTime = Stopwatch.StartNew();
                                if (g.TKCInfo.Contains("1/10") || g.TKCInfo.Contains("3/10") || g.TKCInfo.Contains("5/10") || g.TKCInfo.Contains("7/10") || g.TKCInfo.Contains("9/10"))
                                {
                                    g.IsTraiTKC = false;
                                }
                                if (g.TKCInfo.Contains("2/10") || g.TKCInfo.Contains("4/10") || g.TKCInfo.Contains("6/10") || g.TKCInfo.Contains("8/10") || g.TKCInfo.Contains("10/10"))
                                {
                                    g.IsTraiTKC = true;
                                }
                                if (g.TKCInfo.Contains("10/10"))
                                {
                                    g.TKCComplete = true;
                                }
                            }
                        }
                    });
                }
            }
            if (IsThuHoachHoaEx || IsNhatHop)
            {
                if (swcounthoa.Elapsed.TotalSeconds >= 5)
                {
                    swcounthoa = Stopwatch.StartNew();
                    int[] cntex = TLBB.CountHoaKTT;
                    if (hoabatdau == 0)
                    {
                        hoabatdau = cntex[0];
                        hoahientai = hoabatdau;
                        swthuhoa = Stopwatch.StartNew();
                    }
                    else
                    {
                        hoahientai = cntex[0];
                    }
                    if (kttbatdau == 0)
                    {
                        kttbatdau = cntex[1];
                        ktthientai = kttbatdau;
                        ktttime = Stopwatch.StartNew();
                    }
                    else
                    {
                        ktthientai = cntex[1];
                    }
                }
            }
        }

        public int CountThienCoTrong { get; set; }

        private void CatDoThienCo()
        {
            if (Global.Paused)
                return;
            if (TLBB.IsBankOpen)
                return;
            List<uint> list = PacketItems.ThienCoTrong.ToList();
            CountThienCoTrong = list.Count;
            if (Main.SettingForm.checkBankItem.Checked && Address.GameType == 1)
            {
                bool isCat = false;                
                foreach (var item in PacketItems.All)
                {
                    if (list.Count == 0)
                        break;
                    if (item.Name == "" || item.Type == "")
                        continue;
                    if (Missions.Contains(MissionsType.DatDoiBaoDoHiem) && item.Name == "Bảo Đồ Hiếm")
                        continue;
                    if (Missions.Contains(MissionsType.MoBaoTangDo) && item.Name == "Tàng Bảo Đồ")
                        continue;
                    if (Missions.Contains(MissionsType.ChucPhucMaoBut) && item.Name == "Chúc Phúc Mao Bút")
                        continue;
                    if (Missions.Contains(MissionsType.SuaThanKhi))
                    {
                        if (GAMEDIC.TanMangThanPhu.Contains(item.Type))
                            continue;
                    }

                    if (Main.GomDoName != TLBB.Name)
                    {
                        if (Missions.Contains(MissionsType.GomDo))
                        {
                            if (Missions.Contains(MissionsType.DuaTangBaoDo))
                            {
                                if (item.Name == "Tàng Bảo Đồ")
                                    continue;
                            }
                            if (Missions.Contains(MissionsType.DuaBaoDoHiem))
                            {
                                if (item.Name == "Bảo Đồ Hiếm")
                                    continue;
                            }
                            else
                            {
                                if (HaveGomName.Contains(item.Name))
                                    continue;
                                if (HaveGomName.Contains(item.TypeName))
                                    continue;
                            }
                        }

                        if (Missions.Contains(MissionsType.GomDoKNB))
                        {
                            if (HaveGomKNBName.Contains(item.Name))
                                continue;
                            if (HaveGomKNBName.Contains(item.TypeName))
                                continue;
                        }
                    }
                    if (TLBB.MapId == MAP.DaiLy)
                    {
                        if (GetDistance(DAILY.VoDong.X, DAILY.VoDong.Y) < 8)
                        {
                            if (item.TypeName == "Võ Hồn" || item.Name == "Võ Hồn Diên Thọ Đan")
                                continue;
                        }
                    }
                    if (HaveToCatThienCoName.Contains(item.Name.Trim()) || HaveToCatThienCoName.Contains(item.TypeName.Trim()))
                    {
                        if (list.Count > 0)
                        {
                            PushItemThienCo(item.Index, (int)list[0]);
                            isCat = true;
                            list.RemoveAt(0);
                        }
                    }
                }
                if (isCat)
                {
                    DoStringEx("setmetatable(_G, {__index = Packet_Temporary_Env}); Packet_Temporary_CleanButtonClk(); ");
                }
            }
        }

        private void RaKhoiDiaphu()
        {
            if (TLBB.MapId != MAP.DiaPhu && TLBB.MapId != MAP.DiaPhuDaiTheGioi)
                return;

            if (IsOneSec)
            {
                TLBB.Read();
                Objects.Read();
                State = STATE.None;
                MissionState = MissionState = MissionState = MissionState = MissionState = MissionState = "";
                if (TLBB.IsQuestOpen)
                {
                    if (TLBB.MapId == MAP.DiaPhu)
                    {
                        if (Address.GameType != 1)
                        {
                            QuestFrameOptionClicked(12009, 1);
                        }
                        else
                        {
                            QuestFrameOptionClicked(77001, 0);
                            QuestFrameOptionClicked(77001, 353);
                        }
                        CloseQuest();
                    }
                    else
                    {
                        QuestFrameOptionClicked(77001, 353);

                        CloseQuest();
                    }
                    return;
                }
                //Mạnh Bà
                foreach (GameObject _object in Objects.All)
                {
                    if (_object.Name == "Mạnh Bà")
                    {
                        Talk(_object.Id);
                        return;
                    }
                }
                foreach (GameObject _object in Objects.All)
                {
                    if (_object.CleanName.Contains("manhba") || _object.CleanName == "pomeng")
                    {
                        Talk(_object.Id);
                        return;
                    }
                }
            }
            return;
        }



        private void FakeMac()
        {
            if (Global.IsChucPhucCungMay && Address.GameType == 1)
            {
                if (!IsFakeMac)
                {
                    uint address = Memory.VirtualAllocEx(30);
                    byte[] buff = new byte[20];
                    for (int i = 0; i < buff.Length; i++)
                    {
                        buff[i] = (byte)TDT.random.Next(0, 255);
                    }
                    Memory.WriteAdress(buff, address, ProcessId);
                    byte[] buffer1 = Memory.Hex2ByteArr(address.ToString("X8")).Reverse().ToArray();
                    byte[] buffer2 = Memory.Hex2ByteArr((address + 1).ToString("X8")).Reverse().ToArray();
                    byte[] buffer3 = Memory.Hex2ByteArr((address + 2).ToString("X8")).Reverse().ToArray();
                    byte[] buffer4 = Memory.Hex2ByteArr((address + 3).ToString("X8")).Reverse().ToArray();
                    byte[] buffer5 = Memory.Hex2ByteArr((address + 4).ToString("X8")).Reverse().ToArray();
                    byte[] buffer6 = Memory.Hex2ByteArr((address + 5).ToString("X8")).Reverse().ToArray();
                    byte[] buffer7 = Memory.Hex2ByteArr((address + 6).ToString("X8")).Reverse().ToArray();

                    Memory.WriteAdress(buffer1, AddressGameExe + 0x425173 + 3, ProcessId);
                    Memory.WriteAdress(buffer2, AddressGameExe + 0x425173 + (0x8 * 1) + 3, ProcessId);
                    Memory.WriteAdress(buffer3, AddressGameExe + 0x425173 + (0x8 * 2) + 3, ProcessId);
                    Memory.WriteAdress(buffer4, AddressGameExe + 0x425173 + (0x8 * 3) + 3, ProcessId);
                    Memory.WriteAdress(buffer5, AddressGameExe + 0x425173 + (0x8 * 4) + 3, ProcessId);
                    Memory.WriteAdress(buffer6, AddressGameExe + 0x425173 + (0x8 * 5) + 3, ProcessId);
                    Memory.WriteAdress(buffer7, AddressGameExe + 0x42511B, ProcessId);
                    IsFakeMac = true;
                    Thread.Sleep(300);
                }
            }
        }

        public void MuaNgua()
        {
            if (TLBB.Lvl >= 60)
            {
                if (PacketItems.Ride.Where(i => GAMEDIC.ThuCuoi60.Contains(i.Name)).FirstOrDefault() != null)
                {
                    RemoveMission(MissionsType.DiMuaNga);
                    return;
                }
                if (PacketItems.DaoCu.Where(i => GAMEDIC.ThuCuoi60.Contains(i.Name)).FirstOrDefault() != null)
                {
                    PacketItems.DaoCu.Where(i => GAMEDIC.ThuCuoi60.Contains(i.Name)).FirstOrDefault().DoAction();
                    Thread.Sleep(350);
                    LUA.MessageBox_Self_OK_Clicked();
                    return;
                }
            }
            else if (TLBB.Lvl >= 40)
            {
                if (PacketItems.Ride.Where(i => GAMEDIC.ThuCuoi40.Contains(i.Name)).FirstOrDefault() != null)
                {
                    RemoveMission(MissionsType.DiMuaNga);
                    return;
                }
                if (PacketItems.DaoCu.Where(i => GAMEDIC.ThuCuoi40.Contains(i.Name)).FirstOrDefault() != null)
                {
                    PacketItems.DaoCu.Where(i => GAMEDIC.ThuCuoi40.Contains(i.Name)).FirstOrDefault().DoAction();
                    Thread.Sleep(350);
                    LUA.MessageBox_Self_OK_Clicked();
                    return;
                }
            }
            if (GoToEx(TLBB.NPCKy))
            {
                if (TLBB.IsShopOpen)
                {
                    foreach (var shop in PacketItems.Shop)
                    {
                        if (TLBB.Lvl >= 60)
                        {
                            if (GAMEDIC.ThuCuoi60.Contains(shop.Name))
                            {
                                Buy(shop.Index);
                            }
                        }
                        else if (TLBB.Lvl >= 40)
                        {
                            if (GAMEDIC.ThuCuoi40.Contains(shop.Name))
                            {
                                Buy(shop.Index);
                            }
                        }
                    }
                }
                else
                {
                    Talk(TLBB.NPCKy);
                    Thread.Sleep(350);
                    QuestFrame.ClickAll();
                }
            }
        }


        public void TrongHoa()
        {
            if (!TLBB.Online)
                return;
            if (Missions.Contains(MissionsType.NhanBienThan))
                return;
            if (Missions.Contains(MissionsType.BachHoaDuyen))
                return;

            if (IsByLogin)
            {
                if (!Missions.Contains(MissionsType.NhanBienThan))
                {
                    if (GoToEx(TrongHoaXX + (MaxHoaXX / 2), 148, 2))
                    {
                        if (!HaveBienThan)
                            IsXongBHD = true;
                    }
                    return;
                }
            }

            if (IsThuHoachHoaEx)
            {
                if (ThuHoachHoaEx())
                    return;
            }

            if (TLBB.PlayerState != 0)
                return;

            if (Missions.Contains(MissionsType.TrongHoa) && !Global.Paused)
            {
                if (TLBB.MapId != 2)
                {
                    GoToEx(100, 149, 2);
                    return;
                }
                uint delay = Memory.Read(TLBB.DelayBase + 0x5F4);

                if (delay == 0 || delay == 0xFFFFFFFF)
                {
                    int hoaIndex = -1;
                    foreach (var item in PacketItems.All)
                    {
                        if (item.ClearName.Contains("tienhoachungtu"))
                        {
                            hoaIndex = (int)item.Index;
                            break;
                        }
                    }
                    if (hoaIndex != -1)
                    {
                        GetToaDo();
                        if (BachHoaDuyenX != 0)
                        {
                            CareX = BachHoaDuyenX; CareY = BachHoaDuyenY;
                            if (TrueStandTime.Elapsed.TotalSeconds > 20)
                            {
                                if (!BlackListHoa.Contains(BachHoaDuyenX + "," + BachHoaDuyenY))
                                {
                                    BlackListHoa.Add(BachHoaDuyenX + "," + BachHoaDuyenY);
                                }
                            }
                            if (GoToEx(BachHoaDuyenX, BachHoaDuyenY, -1, false, (float)0.1))
                            {
                                DownRide();
                                PlayerPackageUseItem(hoaIndex);
                                return;
                            }
                        }
                        else
                        {
                            if (GetDistance(TrongHoaXX, 148) > 15)
                            {
                                GoToEx(TrongHoaXX, 148);
                            }
                            BlackListHoa.Clear();
                        }
                    }
                    else
                    {
                        if (IsByLogin && TLBB.OnlineTimeSec > 15)
                        {
                            IsXongBHD = true;
                        }
                    }
                }
            }
            if (Missions.Contains(MissionsType.BonHoa))
            {
                if (IsByLogin)
                {
                    if (TrongHoaXX != 0)
                    {
                        if (TLBB.MapId != MAP.DaiLy || TDT.GetDistance(TrongHoaXX + (MaxHoaXX / 2), 148, RoundX, RoundY) > 8)
                        {
                            GoToEx(TrongHoaXX + (MaxHoaXX / 2), 148, 2);
                        }
                    }
                }

                uint delay = Memory.Read(TLBB.DelayBase + 0x5E8);
                if (delay == 0 || delay == 0xFFFFFFFF)
                {
                    int phanHoaIndex = -1;
                    foreach (var item in PacketItems.All)
                    {
                        if (item.Type == "CircularTaskTool8_11")
                        {
                            phanHoaIndex = (int)item.Index;
                            break;
                        }
                    }
                    if (phanHoaIndex != -1)
                    {
                        GameObject o = Objects.All.Where(o => o.Name.Contains("Tiên Hoa Ấu Miêu") && IsHoaBonMine(o.Title)).OrderBy(_ => Guid.NewGuid()).FirstOrDefault();
                        if (o != null)
                        {
                            DownRide();
                            SelectTarget(o.Id);
                            DoAction("CircularTaskTool8_11");
                            return;
                        }
                    }
                    else
                    {
                        if (IsByLogin && TLBB.OnlineTimeSec > 15)
                        {
                            IsXongBHD = true;
                        }
                    }
                }
            }
        }

        public HashSet<string> BlackListHoa = new HashSet<string>();

        public string dialogInfo = "";
        public List<long[]> ListHoaTruongThanh = new List<long[]>();
        public Stopwatch ShowTime { get; set; } = Stopwatch.StartNew();

        public Stopwatch HideTime = Stopwatch.StartNew();

        public bool IsClearArea()
        {
            //if (TDT.GetDistance(CharX, CharX, BachHoaDuyenX, BachHoaDuyenY) > 15)
            //{
            //    Main.AddLog("!23");
            //    return false;
            //}

            if (GetDistance(BachHoaDuyenX, BachHoaDuyenY) > 14)
                return false;

            if (AllOtherCarePoint.Contains(BachHoaDuyenX + "," + BachHoaDuyenY))
            {
                return false;
            }

            if (BachHoaDuyenX - TrongHoaXX > MaxHoaXX)
                return false;
            if (BlackListHoa.Contains(BachHoaDuyenX + "," + BachHoaDuyenY))
                return false;
            string point = BachHoaDuyenX + "," + BachHoaDuyenY;
            foreach (GameObject _object in Objects.All)
            {
                if (_object.Name.Contains("Tiên Hoa Ấu Miêu") || _object.Name.Contains("Hoa Trưởng Thành") || _object.IsNPC)
                {
                    _object.DistanceEx = _object.GetDistance(BachHoaDuyenX, BachHoaDuyenY);
                    if (_object.DistanceEx < 2)
                    {
                        BlackListHoa.Add(BachHoaDuyenX + "," + BachHoaDuyenY);
                        return false;
                    }
                }
            }

            return true;
        }

        private HashSet<string> NotSafe = new HashSet<string>();

        public bool IsSafeArea(int x = -1, int y = -1)
        {
            if (x == -1)
                x = SafeX;
            if (y == -1)
                y = SafeY;

            if (TLBB.MapId == MAP.TamThanHuyenCanh)
            {
                foreach (GameObject _object in Objects.All)
                {
                    if (_object.IsTrap)
                    {
                        if (TDT.GetDistance(_object.RoundX, _object.RoundY, x, y) < 8)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }

            //if (TDT.GetDistance(x, y, RoundX, RoundY) > 20)
            //    return false;
            if (TLBB.MapId == MAP.PhungHoangCoThanhPhuBan)
            {
                foreach (GameObject _object in Objects.All)
                {
                    if (_object.IsTrap)
                    {
                        if (TDT.GetDistance(_object.RoundX, _object.RoundY, x, y) < 5)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }

            if (TLBB.MapId == MAP.ThieuThatSon)
            {
                //if(RoundY > 68)

                foreach (GameObject _object in Objects.All)
                {
                    if (_object.IsTrap)
                    {
                        if (TDT.GetDistance(_object.RoundX, _object.RoundY, x, y) < 6)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
            if (TLBB.MapId == MAP.LoiDaiSinhTu)
            {
                foreach (GameObject _object in Objects.All)
                {
                    if (_object.IsTrap)
                    {
                        if (TDT.GetDistance(_object.RoundX, _object.RoundY, x, y) < 4.5)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
            if (TLBB.MapId == MAP.PhungMinhVuongLang)
            {
                foreach (GameObject _object in Objects.All)
                {
                    if (_object.IsTrap)
                    {
                        if (TDT.GetDistance(_object.RoundX, _object.RoundY, x, y) < 6)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }

            if (TLBB.MapId == MAP.TuTuyetTrang)
            {
                if (GetRoundTuTuyet(RoundX, RoundY) == 1)
                {
                    if (GetRoundTuTuyet(SafeX, SafeY) != 1)
                        return false;
                }
                if (GetRoundTuTuyet(RoundX, RoundY) == 4)
                {
                    if (TDT.GetDistance(x, y, 29, 28) >= 12)
                        return false;
                }
            }
            if (GetRoundTuTuyet(RoundX, RoundY) == 4)
                NotSafe.Clear();
            if (NotSafe.Contains(x + "," + y))
                return false;
            foreach (GameObject _object in Objects.All)
            {
                if (_object.IsTrap)
                {
                    if (TDT.GetDistance(_object.RoundX, _object.RoundY, x, y) < 8)
                    {
                        NotSafe.Add(x + "," + y);
                        return false;
                    }
                }
                if (_object.CleanName == "hoiamphien")
                {
                    if (TDT.GetDistance(_object.RoundX, _object.RoundY, x, y) < 13)
                    {
                        NotSafe.Add(x + "," + y);
                        return false;
                    }
                }
            }
            return true;
        }



        private int SafeX;
        private int SafeY;
        private int CurPhungMinhIndex = -1;

        public void GetSafePoint()
        {
            //if()
            if (TLBB.MapId == MAP.PhungHoangCoThanhPhuBan)
            {
                for (int i = 58; i < 74; i++)
                {
                    for (int j = 64; j < 80; j++)
                    {
                        SafeX = i;
                        SafeY = j;
                        if (IsSafeArea())
                            break;
                    }
                    if (IsSafeArea())
                        break;
                }
                if (!IsSafeArea())
                {
                    SafeX = SafeY = 0;
                }
            }
            if (TLBB.MapId == MAP.TamThanHuyenCanh)
            {
                if (RoundX > 95 && RoundY < 90)
                {
                    for (int i = 0; i < GAMEDIC.TamThanRound2.Count; i++)
                    {
                        SafeX = TDT.ParseInt(GAMEDIC.TamThanRound2[i].Split(',')[0]);
                        SafeY = TDT.ParseInt(GAMEDIC.TamThanRound2[i].Split(',')[1]);
                        if (IsSafeArea())
                            return;
                    }
                    if (!IsSafeArea())
                    {
                        SafeX = SafeY = 0;
                    }
                }
                if (RoundX < 90 && RoundY > 85)
                {
                    //for (int x = 54 - 15; x < 54 + 15; x++)
                    //{
                    //    for (int y = 112 - 15; y < 112 + 15; y++)
                    //    {
                    //        SafeX = x;
                    //        SafeY = y;
                    //        PushDebugMessage(SafeX + "," + SafeY);
                    //        if (IsSafeArea())
                    //            break;
                    //    }
                    //    if (IsSafeArea())
                    //        break;
                    //}
                    for (int i = 0; i < GAMEDIC.TamThanRound1.Count; i++)
                    {
                        SafeX = TDT.ParseInt(GAMEDIC.TamThanRound1[i].Split(',')[0]);
                        SafeY = TDT.ParseInt(GAMEDIC.TamThanRound1[i].Split(',')[1]);
                        if (IsSafeArea())
                        {
                            return;
                        }
                    }

                    if (!IsSafeArea())
                    {
                        SafeX = SafeY = 0;
                    }
                }
                if (RoundX > 95 && RoundY > 115)
                {
                    for (int i = 0; i < GAMEDIC.TamThanRound3.Count; i++)
                    {
                        SafeX = TDT.ParseInt(GAMEDIC.TamThanRound3[i].Split(',')[0]);
                        SafeY = TDT.ParseInt(GAMEDIC.TamThanRound3[i].Split(',')[1]);
                        if (IsSafeArea())
                            return;
                    }
                    if (!IsSafeArea())
                    {
                        SafeX = SafeY = 0;
                    }
                }
            }
            if (TLBB.MapId == MAP.ThieuThatSon)
            {
                if (RoundY <= 68)
                {
                    if (CurPhungMinhIndex == -1)
                    {
                        CurPhungMinhIndex = 0;
                        SafeX = TDT.ParseInt(GAMEDIC.MoDungBac[CurPhungMinhIndex].Split(',')[0]);
                        SafeY = TDT.ParseInt(GAMEDIC.MoDungBac[CurPhungMinhIndex].Split(',')[1]);
                    }
                    while (!IsSafeArea())
                    {
                        CurPhungMinhIndex++;
                        if (CurPhungMinhIndex > 5)
                        {
                            SafeX = SafeY = 0;
                            CurPhungMinhIndex = -1;
                            break;
                        }
                        SafeX = TDT.ParseInt(GAMEDIC.MoDungBac[CurPhungMinhIndex].Split(',')[0]);
                        SafeY = TDT.ParseInt(GAMEDIC.MoDungBac[CurPhungMinhIndex].Split(',')[1]);
                    }
                    if (!IsSafeArea())
                    {
                        SafeX = SafeY = 0;
                    }
                }
                else
                {
                    if (CurPhungMinhIndex == -1)
                    {
                        CurPhungMinhIndex = 0;
                        SafeX = TDT.ParseInt(GAMEDIC.TrangTuHien[CurPhungMinhIndex].Split(',')[0]);
                        SafeY = TDT.ParseInt(GAMEDIC.TrangTuHien[CurPhungMinhIndex].Split(',')[1]);
                    }
                    while (!IsSafeArea())
                    {
                        CurPhungMinhIndex++;
                        if (CurPhungMinhIndex > 5)
                        {
                            SafeX = SafeY = 0;
                            CurPhungMinhIndex = -1;
                            break;
                        }
                        SafeX = TDT.ParseInt(GAMEDIC.TrangTuHien[CurPhungMinhIndex].Split(',')[0]);
                        SafeY = TDT.ParseInt(GAMEDIC.TrangTuHien[CurPhungMinhIndex].Split(',')[1]);
                    }
                    if (!IsSafeArea())
                    {
                        SafeX = SafeY = 0;
                    }
                }
                return;
            }
            if (TLBB.MapId == MAP.LoiDaiSinhTu)
            {
                if (CurPhungMinhIndex == -1)
                {
                    CurPhungMinhIndex = 0;
                    SafeX = TDT.ParseInt(GAMEDIC.LoiDaiSinhTu[CurPhungMinhIndex].Split(',')[0]);
                    SafeY = TDT.ParseInt(GAMEDIC.LoiDaiSinhTu[CurPhungMinhIndex].Split(',')[1]);
                }
                while (!IsSafeArea())
                {
                    CurPhungMinhIndex++;
                    if (CurPhungMinhIndex > 5)
                    {
                        SafeX = SafeY = 0;
                        CurPhungMinhIndex = -1;
                        break;
                    }
                    SafeX = TDT.ParseInt(GAMEDIC.LoiDaiSinhTu[CurPhungMinhIndex].Split(',')[0]);
                    SafeY = TDT.ParseInt(GAMEDIC.LoiDaiSinhTu[CurPhungMinhIndex].Split(',')[1]);
                }
                if (!IsSafeArea())
                {
                    SafeX = SafeY = 0;
                }
                return;
            }
            SafeX = SafeY = 0;
            if (TLBB.MapId == MAP.PhungMinhVuongLang)
            {
                SafeX = SafeY = 0;
                if (!Objects.Traps.Any(o => o.GetDistance(48,48) < 6))
                {
                    SafeX = 48;
                    SafeY = 48;
                    return;
                }
                for (int i = 28; i < 68; i++)
                {
                    for (int j = 28; j < 68; j++)
                    {
                        if (!Objects.Traps.Any(o => o.GetDistance(i, j) < 6))
                        {
                            if (GetDistance(i, j) < GetDistance(SafeX, SafeY))
                            {
                                SafeX = i;
                                SafeY = j;
                            }
                        }
                    }
                }
                return;
            }

            if (TLBB.MapId == MAP.TuTuyetTrang)
            {
                if (GetRoundTuTuyet(RoundX, RoundY) == 1)
                {
                    //List<string> listSafe = new List<string>()
                    //{
                    //    "92,94",
                    //    "97,86",
                    //    "107,86",
                    //    "97,102",
                    //    "107,102",
                    //    "112,94",
                    //};
                    //for(int i =0; i< listSafe.Count; i++)
                    //{
                    //    SafeX = TDT.ParseInt(listSafe[i].Split(',')[0]);
                    //    SafeY = TDT.ParseInt(listSafe[i].Split(',')[1]);
                    //    if (IsSafeArea())
                    //    {
                    //        return;
                    //    }
                    //}
                    if (SafeX == 0 || SafeY == 0)
                    {
                        SafeX = 88;
                        SafeY = 85;
                    }
                    while (!IsSafeArea())
                    {
                        SafeX = SafeX + 1;
                        if (SafeX > 112)
                        {
                            SafeX = 88;
                            SafeY = SafeY + 1;
                        }
                        if (SafeY > 104)
                        {
                            SafeX = 88;
                            SafeY = 85;
                            break;
                        }
                    }
                    if (!IsSafeArea())
                    {
                        SafeX = SafeY = 0;
                        NotSafe.Clear();
                    }
                    return;
                }

                if (GetRoundTuTuyet(RoundX, RoundY) == 2)
                {
                    if (SafeX == 0 || SafeY == 0)
                    {
                        SafeX = 15;
                        SafeY = 87;
                    }
                    while (!IsSafeArea())
                    {
                        SafeX = SafeX + 1;
                        if (SafeX > 40)
                        {
                            SafeX = 15;
                            SafeY = SafeY + 1;
                        }
                        if (SafeY > 112)
                        {
                            SafeX = 15;
                            SafeY = 87;
                            break;
                        }
                    }
                    if (!IsSafeArea())
                    {
                        SafeX = SafeY = 0;
                        NotSafe.Clear();
                    }
                    return;
                }
                if (GetRoundTuTuyet(RoundX, RoundY) == 4)
                {
                    if (SafeX == 0)
                    {
                        SafeX = 16;
                        SafeY = 18;
                    }
                    while (!IsSafeArea())
                    {
                        SafeX = SafeX + 1;
                        if (SafeX > 42)
                        {
                            SafeX = 18;
                            SafeY = SafeY + 1;
                        }
                        if (SafeY > 38)
                        {
                            SafeX = 16;
                            SafeY = 18;
                            break;
                        }
                    }
                    if (!IsSafeArea())
                        NotSafe.Clear();
                    return;
                }
            }
        }

        public void GetToaDo()
        {
            if (BachHoaDuyenX != 0)
            {
                if (IsClearArea())
                {
                    return;
                }
            }
            BachHoaDuyenX = TrongHoaXX;
            BachHoaDuyenY = 154;

            bool isc = false;

            while (true)
            {
                if (!isc)
                {
                    isc = true;
                }
                else
                {
                    if (BachHoaDuyenY == 144)
                    {
                        BachHoaDuyenY = 154;
                        BachHoaDuyenX = BachHoaDuyenX + 2;
                    }
                    else
                    {
                        BachHoaDuyenY = BachHoaDuyenY - 2;
                    }
                    if (BachHoaDuyenX - TrongHoaXX > MaxHoaXX)
                    {
                        BachHoaDuyenX = 0;
                        BachHoaDuyenY = 0;
                        break;
                    }
                }
                if (IsClearArea())
                {
                    return;
                }
            }
        }


        public bool IsMapMonPhai
        {
            get
            {
                if (TLBB.MapId == MAP.DuongMon || TLBB.MapId == MAP.MoDung || TLBB.MapId == MAP.TinhTuc || TLBB.MapId == MAP.TieuDao || TLBB.MapId == MAP.ThieuLam || TLBB.MapId == MAP.ThienSon || TLBB.MapId == MAP.ThienLong || TLBB.MapId == MAP.NgaMy || TLBB.MapId == MAP.VoDang || TLBB.MapId == MAP.MinhGiao || TLBB.MapId == MAP.CaiBang || TLBB.MapId == MAP.DaoHoa || TLBB.MapId == MAP.DuongMon)
                    return true;
                return false;
            }
        }

        public bool IsMapKeoDoi()
        {
            if (TLBB.MapId == MAP.VoLuongSon || TLBB.MapId == MAP.KiemCac || TLBB.MapId == MAP.DonHoang || TLBB.MapId == MAP.TungSon || TLBB.MapId == MAP.ThaiHo || TLBB.MapId == MAP.KinhHo)
                return true;
            if (TLBB.MapId == MAP.DuongMon || TLBB.MapId == MAP.MoDung || TLBB.MapId == MAP.TinhTuc || TLBB.MapId == MAP.TieuDao || TLBB.MapId == MAP.ThieuLam || TLBB.MapId == MAP.ThienSon || TLBB.MapId == MAP.ThienLong || TLBB.MapId == MAP.NgaMy || TLBB.MapId == MAP.VoDang || TLBB.MapId == MAP.MinhGiao || TLBB.MapId == MAP.CaiBang)
                return true;
            if (TLBB.MapId == MAP.TayHo || TLBB.MapId == MAP.NhiHai || TLBB.MapId == MAP.NhanNam)
                return true;
            if (TLBB.MapId == MAP.ThanhThuSon)
                return true;
            if (TLBB.MapId == MAP.LauLan)
                return true;
            if (TLBB.MapId == MAP.PhungHoangCoThanh)
                return true;
            return false;
        }

        public bool IsMapAcBa
        {
            get
            {
                if (TLBB.MapId == MAP.QuyCocAcBa || TLBB.MapId == MAP.ThieuLamAcBa || TLBB.MapId == MAP.NgaMyAcBa || TLBB.MapId == MAP.TieuDaoAcBa || TLBB.MapId == MAP.DuongMonAcBa || TLBB.MapId == MAP.MinhGiaoAcBa || TLBB.MapId == MAP.VoDangAcBa || TLBB.MapId == MAP.TinhTucAcBa || TLBB.MapId == MAP.ThienSonAcBa || TLBB.MapId == MAP.CaiBangAcBa || TLBB.MapId == MAP.ThienLongAcBa || TLBB.MapId == MAP.MoDungAcBa || TLBB.MapId == MAP.DaoHoaAcBa)
                    return true;
                return false;
            }
        }

        public int MapAcbaMen
        {
            get
            {
                if (TLBB.Menpai == MENPAI.ThieuLam)
                    return MAP.ThieuLamAcBa;
                if (TLBB.Menpai == MENPAI.NgaMy)
                    return MAP.NgaMyAcBa;
                if (TLBB.Menpai == MENPAI.TieuDao)
                    return MAP.TieuDaoAcBa;
                if (TLBB.Menpai == MENPAI.DuongMon)
                    return MAP.DuongMonAcBa;
                if (TLBB.Menpai == MENPAI.MinhGiao)
                    return MAP.MinhGiaoAcBa;
                if (TLBB.Menpai == MENPAI.VoDang)
                    return MAP.VoDangAcBa;
                if (TLBB.Menpai == MENPAI.TinhTuc)
                    return MAP.TinhTucAcBa;
                if (TLBB.Menpai == MENPAI.ThienSon)
                    return MAP.ThienSonAcBa;
                if (TLBB.Menpai == MENPAI.CaiBang)
                    return MAP.CaiBangAcBa;
                if (TLBB.Menpai == MENPAI.ThienLong)
                    return MAP.ThienLongAcBa;
                if (TLBB.Menpai == MENPAI.MoDung)
                    return MAP.MoDungAcBa;
                if (TLBB.Menpai == MENPAI.QuyCoc)
                    return MAP.QuyCocAcBa;
                return -1;
            }
        }


        private void Mua100KimSangDuoc()
        {
            if (GoToEx(LACDUONG.BachManhSinh))
            {
                Talk(LACDUONG.BachManhSinh);
                Thread.Sleep(700);
                if (TLBB.IsShopOpen)
                {
                    string buyname = "Kim Sáng Dược|100|True";
                    foreach (string nam in (buyname).Split('\n'))
                    {
                        string name = nam.Trim();
                        int num = 0;
                        if (name.Split('|').Length == 3)
                        {
                            string n = name.Split('|')[0];
                            int cnt = 0;
                            foreach (var item in PacketItems.All)
                            {
                                if (TDT.VietLien(item.Name) == TDT.VietLien(name.Split('|')[0]))
                                {
                                    cnt += (int)item.Count;
                                }
                            }
                            if (cnt < TDT.ParseInt(name.Split('|')[1]))
                            {
                                num = TDT.ParseInt(name.Split('|')[1]) - cnt;
                            }
                        }
                        if (num > 0)
                        {
                            foreach (var s in PacketItems.Shop)
                            {
                                if (TDT.VietLien(s.Name).Contains(TDT.VietLien(name.Split('|')[0])))
                                {
                                    while (num > 0)
                                    {
                                        int countkimsang = 0;
                                        foreach (var item in PacketItems.All)
                                        {
                                            if (item.ClearName == "kimsangduoc")
                                                countkimsang += (int)item.Count;
                                        }
                                        if (countkimsang >= 100)
                                            break;
                                        int bu = num;
                                        if (bu > 20)
                                            bu = 20;
                                        if (name.Split('|')[2] != "True")
                                            bu = 1;
                                        LUA.Buy((int)s.Index, bu);
                                        Thread.Sleep(300);
                                        num = num - bu;
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        public void DoiHuyenSac()
        {
            if (PacketItems.TrangBi.Where(p => p.Name == "Long Văn" && p.Star == 8).FirstOrDefault() == null)
            {
                RemoveMission(MissionsType.DoiHuyenSacCauThienThai);
                return;
            }
            if (GoToEx(KimLang.TieuUng))
            {
                Talk(KimLang.TieuUng);
                Thread.Sleep(1000);
                if (TLBB.IsQuestOpen)
                {
                    foreach (PacketItem item in PacketItems.ThienCo)
                    {
                        if (item.Name == "Cao Cấp Câu Thiên Thái")
                        {
                            GetItemThienCo(item.Index);
                            Thread.Sleep(350);
                        }
                    }
                    QuestFrame.Click("#{GTCHC_140213_01}");
                    Thread.Sleep(1000);
                    QuestFrame.Click("#{LW_KVK_130426_9}");
                    RemoveMission(MissionsType.DoiHuyenSacCauThienThai);
                }
            }
        }


        public void ThanhLyNhiemVu()
        {
            if (GoToEx(DAILY.DuTroChi))
            {
                Talk(DAILY.DuTroChi);
                Thread.Sleep(1000);
                QuestFrame.Click("Ta muốn xóa tất cả mọi nhiệm vụ");
                Thread.Sleep(1000);
                QuestFrame.Click("Duyệt");
                RemoveMission(MissionsType.ThanhLyNhiemVu);
            }
        }

        public bool IsMapPhuBan()
        {
            if (TLBB.MapId == MAP.CoDongTuocDai || TLBB.MapId == MAP.ThienNhanHoangSon || TLBB.MapId == MAP.LangVanDaiPhat)
                return true;
            if (TLBB.MapId == MAP.BienGioiTongLieu || TLBB.MapId == MAP.TrucLam)
                return true;
            if (TLBB.MapId == MAP.TacKhauDoanhDia)
                return true;
            if (IsMapAcBa)
                return true;
            if (TLBB.MapId == MAP.TangKinhCac)
                return true;
            if (TLBB.MapId == MAP.PhungHoangCoThanhPhuBan)
                return true;
            if (TLBB.MapId == MAP.ViemMaSon)
                return true;
            if (TLBB.MapId == MAP.TamTaiHiepCoc)
                return true;
            if (TLBB.MapId == MAP.ThanhThuSonPhuBan || TLBB.MapId == MAP.HuyenVuDaoPhuBan)
                return true;
            if (TLBB.MapId == MAP.TranLongKyCuoc)
                return true;
            if (TLBB.MapId == MAP.LauLanBaoTang)
                return true;
            if (TLBB.MapId == MAP.PhieuMieuPhong)
                return true;
            if (TLBB.MapId == MAP.YenTuO)
                return true;
            if (TLBB.MapId == MAP.ThieuThatSon)
                return true;
            if (TLBB.MapId == MAP.PhungMinhVuongLang)
                return true;
            if (TLBB.MapId == MAP.LoiDaiSinhTu)
                return true;
            if (TLBB.MapId == MAP.TuTuyetTrang)
                return true;
            if (TLBB.MapId == MAP.TamThanHuyenCanh)
                return true;
            if (TLBB.MapId == MAP.PhieuMieuPhong)
                return true;
            if (TLBB.MapId == MAP.BinhThanhKyTran)
                return true;
            if (TLBB.MapId == MAP.LanHoanPhucDia)
                return true;
            if (TLBB.MapId == MAP.NongTruongDaTru)
                return true;
            if (TLBB.MapId == MAP.ThienLongHuyenCanh)
                return true;
            if (TLBB.MapId == MAP.MauDonUyen)
                return true;
            if (TLBB.MapId == MAP.HuyetMo)
                return true;
            return false;
        }

        public bool Talked;
        public DateTime BossTime = DateTime.MinValue;
        public int TueHongState = 0;
        private bool NeedAlarmTueHong = false;

        public void TueHong()
        {
            if (!IsOneSec)
                return;
            if (GoToEx(DAILY.TonBatGia))
            {
                if (TLBB.IsQuestOpen)
                {
                    if (QuestFrame.All(this).Contains("#{QNG_090716_24}"))
                    {
                        IsXongBHD = true;
                        RemoveMission(MissionsType.ThienLongTueHong);
                        //NeedAlarmTueHong = true;
                    }
                }

                if (GoToEx(DAILY.TonBatGia))
                {
                    Talk(DAILY.TonBatGia);
                    Thread.Sleep(1000);
                    QuestFrame.Click("#{QNG_XML_1}");
                    Thread.Sleep(1000);
                    QuestFrame.Click("#{QNG_XML_6}");
                    Thread.Sleep(1000);
                    QuestFrameAccept();
                    Thread.Sleep(1000);
                }

                if (GoToEx(DAILY.TonBatGia))
                {
                    Talk(DAILY.TonBatGia);
                    Thread.Sleep(1000);
                    QuestFrame.Click("#{QNG_XML_1}");
                    Thread.Sleep(1000);
                    QuestFrame.Click("#{QNG_XML_7}");
                    Thread.Sleep(1000);
                    QuestFrameAccept();
                    Thread.Sleep(1000);
                }

                if (GoToEx(DAILY.TonBatGia))
                {
                    Talk(DAILY.TonBatGia);
                    Thread.Sleep(1000);
                    QuestFrame.Click("#{QNG_XML_1}");
                    Thread.Sleep(1000);
                    QuestFrame.Click("#{QNG_XML_8}");
                    Thread.Sleep(1000);
                    QuestFrameAccept();
                    Thread.Sleep(1000);
                }

                if (GoToEx(DAILY.TonBatGia))
                {
                    Talk(DAILY.TonBatGia);
                    Thread.Sleep(1000);
                    QuestFrame.Click("#{QNG_XML_1}");
                    Thread.Sleep(1000);
                    QuestFrame.Click("#{QNG_XML_6}");
                    Thread.Sleep(1000);
                    LUA.QuestFrameMissionComplete();
                    Thread.Sleep(1000);
                }

                if (GoToEx(DAILY.TonBatGia))
                {
                    Talk(DAILY.TonBatGia);
                    Thread.Sleep(1000);
                    QuestFrame.Click("#{QNG_XML_1}");
                    Thread.Sleep(1000);
                    QuestFrame.Click("#{QNG_XML_7}");
                    Thread.Sleep(1000);
                    LUA.QuestFrameMissionComplete();
                    Thread.Sleep(1000);
                }

                if (GoToEx(DAILY.TonBatGia))
                {
                    Talk(DAILY.TonBatGia);
                    Thread.Sleep(1000);
                    QuestFrame.Click("#{QNG_XML_1}");
                    Thread.Sleep(1000);
                    QuestFrame.Click("#{QNG_XML_8}");
                    Thread.Sleep(1000);
                    LUA.QuestFrameMissionComplete();
                    Thread.Sleep(1000);
                }

                if (GoToEx(DAILY.TonBatGia))
                {
                    Talk(DAILY.TonBatGia);
                    Thread.Sleep(1000);
                    QuestFrame.Click("#{QNG_XML_2}");
                    Thread.Sleep(1000);
                }

                if (GoToEx(DAILY.TonBatGia))
                {
                    Talk(DAILY.TonBatGia);
                    Thread.Sleep(1000);
                    QuestFrame.Click("#{QNG_XML_2}");
                    Thread.Sleep(1000);

                    if (QuestFrame.All(this).Contains("#{QNG_090716_24}"))
                    {
                        IsXongBHD = true;
                        RemoveMission(MissionsType.ThienLongTueHong);
                        //NeedAlarmTueHong = true;
                    }
                }
            }
        }

        public bool IsTalkedRose
        {
            get;
            set;
        }

        public string MissionState { get; set; } = string.Empty;

        public GameTask TaskCoBan
        {
            get;
            set;
        }

        public Script ScriptCoBan
        {
            get;
            set;
        }

        private int IsTalkToNpc
        {
            get;
            set;
        }

        public List<Script> ListNhiemVu = new List<Script>();

        public int GetRecvNPCId(Script script)
        {
            int id = -1;
            if (script.Name == "Tuyệt Thế Thần Binh")
            {
                foreach (GameObject _object in Objects.AllNpc)
                {
                    if (_object.Name == "Sách Mẫu Lạp")
                    {
                        id = (int)_object.Id;
                    }
                }
                return id;
            }

            foreach (GameObject _object in Objects.AllNpc)
            {
                if (script.Info.ToLower().Contains(_object.Name.ToLower()) && _object.RoundX == script.RecvNPC.X && _object.RoundY == script.RecvNPC.Y)
                {
                    id = (int)_object.Id;
                }
            }
            if (id == -1)
            {
                foreach (GameObject _object in Objects.AllNpc)
                {
                    if (script.Info.ToLower().Contains(_object.Name.ToLower()) && TDT.GetDistance(_object.X, _object.Y, CharX, CharY) < 3)
                    {
                        id = (int)_object.Id;
                    }
                }
            }
            if (id == -1)
            {
                foreach (GameObject _object in Objects.AllNpc)
                {
                    if (_object.Id == script.RecvNPC.Id)
                    {
                        id = (int)_object.Id;
                        break;
                    }
                }
            }
            if (id == -1)
            {
                foreach (GameObject _object in Objects.AllNpc)
                {
                    if (_object.RoundX == script.RecvNPC.X && _object.RoundY == script.RecvNPC.Y)
                    {
                        id = (int)_object.Id;
                        break;
                    }
                }
            }
            if (id == -1)
            {
                foreach (GameObject _object in Objects.All)
                {
                    if (_object.IsSelf)
                        continue;
                    if (_object.RoundX == script.RecvNPC.X && _object.RoundY == script.RecvNPC.Y)
                    {
                        id = (int)_object.Id;
                        break;
                    }
                }
            }
            return id;
        }

        public int GetSendNPCId(Script script)
        {
            int id = -1;
            if (script.SendNPC.Id > 1)
            {
                foreach (GameObject _object in Objects.AllNpc)
                {
                    if (_object.Id == script.SendNPC.Id)
                    {
                        id = (int)_object.Id;
                        break;
                    }
                }
            }
            if (id == -1)
            {
                foreach (GameObject _object in Objects.AllNpc)
                {
                    if (script.Info.ToLower().Contains(_object.Name.ToLower()) && TDT.GetDistance(_object.X, _object.Y, CharX, CharY) < 3)
                    {
                        id = (int)_object.Id;
                        break;
                    }
                }
            }
            if (id == -1)
            {
                foreach (GameObject _object in Objects.AllNpc)
                {
                    if (_object.Id == script.SendNPC.Id)
                    {
                        id = (int)_object.Id;
                        break;
                    }
                }
            }
            if (id == -1)
            {
                foreach (GameObject _object in Objects.AllNpc)
                {
                    if (_object.RoundX == script.SendNPC.X && _object.RoundY == script.SendNPC.Y)
                    {
                        id = (int)_object.Id;
                        break;
                    }
                }
            }
            if (id == -1)
            {
                foreach (GameObject _object in Objects.All)
                {
                    if (_object.IsSelf)
                        continue;
                    if (_object.RoundX == script.SendNPC.X && _object.RoundY == script.SendNPC.Y)
                    {
                        id = (int)_object.Id;
                        break;
                    }
                }
            }
            return id;
        }

        public int GetNPCId(NPC npc)
        {
            return GetNPCId(npc.X, npc.Y);
        }

        public int GetNPCId(float x, float y)
        {
            float minDistance = 999;
            int id = -1;
            foreach (GameObject _object in Objects.AllNpc)
            {
                float distance = TDT.GetDistance(x, y, _object.X, _object.Y);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    id = (int)_object.Id;
                }
            }
            return id;
        }

        public void CloseMission()
        {
            PostMessage(18, 105);
        }

        private bool IsX2Coban { get; set; }
        public Stopwatch swTaskCoban = Stopwatch.StartNew();
        public string nameCobBan = string.Empty;



        public List<string> NhiemVuChuaLam1 = new List<string>()
        {
            "Sinh Tài Chi Đạo",
            "Khoa Ngân Võ Lâm Ấn Túc Cầu Thịnh Hội",
            "Khoa Ngân Võ Lâm Ấn Tàng Kinh Các Nguy Cơ",
            "Khoa Ngân Võ Lâm Ấn Phồn Mang Tào Vận",
            "Quả Ngân Võ Lâm Ấn Bang HộiBuôn Bán",
        };

        private bool IsCheckNhiemmVu = false;
        private int checknvcount = 0;

        public List<string> NhiemVuChuaLam2 = new List<string>()
        {
            "Thanh Đồng Ấn-Tương Trợ Sư Môn",
            "Thanh Đồng Ấn-Trừ Ác",
            "Thanh Đồng Ấn-Trừng Hung",
        };



        private bool IsTalkNhiemVu { get; set; }

        private string CuTaskCoBanName { get; set; } = string.Empty;

        private Stopwatch CoBanFailTime = new Stopwatch();

        public bool NexStep
        {
            get;
            set;
        }

        private int failcnt { get; set; } = 0;

        public void DoiKimTamTy()
        {
            if (!IsOneSec)
                return;
            if (TDT.GetDistance(CharX, CharY, 256, 246) > 2 || TLBB.MapId != LACDUONG.Id)
            {
                GoToEx(256, 246, MAP.LacDuong);
                return;
            }
            if (!IsTalkedRose)
            {
                IsTalkedRose = true;
                TalkEx("buihoahong");
            }
            else
            {
                foreach (QuestFrame dialog in QuestFrame.Enum(this))
                {
                    if (dialog.Name.Contains("#{MGZL_130117_01}"))
                    {
                        QuestFrameOptionClicked(dialog);
                        return;
                    }
                }
                foreach (QuestFrame dialog in QuestFrame.Enum(this))
                {
                    if (dialog.Name.Contains("#{MGZL_130117_03}"))
                    {
                        QuestFrameOptionClicked(dialog);
                        if (!NexStep)
                        {
                            NexStep = true;
                            return;
                        }
                    }
                }
                LUA.QuestFrameMissionComplete(0);
                IsTalkedRose = false;
                NexStep = false;
            }
        }

        public bool IsThoiBong { get; set; }

        public void AcTac()
        {
            if (!Talked && TLBB.MapId == 272 && GetDistance(64, 104) < 5)
            {
                if (TLBB.IsLeader && !Talked && TrueNotMoveTime.Elapsed.TotalSeconds > 3)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Thread.Sleep(2000);
                        foreach (GameObject _object in Objects.All)
                        {
                            if (_object.Name.Contains("n Du V") && _object.Name.Length == 24)
                            {
                                Talk(_object.Id);
                                Talked = true;
                                Thread.Sleep(1000);
                                break;
                                //return;
                            }
                        }
                        foreach (QuestFrame dialog in QuestFrame.Enum(this))
                        {
                            if (dialog.StrOptionExtra1 == 402108)
                            {
                                QuestFrameOptionClicked(dialog);
                                Talked = true;
                                return;
                            }
                        }
                    }
                }
                return;
            }
        }

        private bool TalkNPCPhuBan()
        {
            float minDistance = 100;
            int id = -1;
            foreach (GameObject _object in Objects.All)
            {
                if (TLBB.MapId == MAP.LauLan && _object.Title.Contains("Thiên Niên Kỳ Thú"))
                {
                    minDistance = TDT.GetDistance(CharX, CharY, _object.X, _object.Y);
                    id = (int)_object.Id;
                }
                if (((_object.Name.Contains("c T") && _object.Name.Contains("o Ph") && _object.Name.Length == 15) || _object.CleanName.Contains("rebel") || _object.CleanName.Contains("thief")) && TDT.GetDistance(CharX, CharY, _object.X, _object.Y) < minDistance)
                {
                    minDistance = TDT.GetDistance(CharX, CharY, _object.X, _object.Y);
                    id = (int)_object.Id;
                    if (User.IsEnglish && minDistance <= 5 && Address.GameType != 1)
                    {
                        QuestFrame.ClickAll();
                    }
                }
                //Giang H° Tiêu Ti¬u
                //Giang h° tà ðÕo
                if (_object.Name.StartsWith("Giang h") && _object.Name.Contains(" t") && _object.Name.Length == 15 && TDT.GetDistance(CharX, CharY, _object.X, _object.Y) < minDistance)
                {
                    minDistance = TDT.GetDistance(CharX, CharY, _object.X, _object.Y);
                    id = (int)_object.Id;
                    if (TLBB.IsQuestOpen && minDistance <= 5 && Address.GameType != 1)
                        QuestFrame.ClickAll();
                }
                if (_object.Name.Contains("n Du V") && _object.Name.Length == 24 && TDT.GetDistance(CharX, CharY, _object.X, _object.Y) < minDistance)
                {
                    minDistance = TDT.GetDistance(CharX, CharY, _object.X, _object.Y);
                    id = (int)_object.Id;
                }
                //Mô Kim Hiệu Úy
                if (_object.Name.StartsWith("M") && _object.Name.Contains("Kim H") && _object.Name.Contains("u ") && TDT.GetDistance(CharX, CharY, _object.X, _object.Y) < minDistance)
                {
                    minDistance = TDT.GetDistance(CharX, CharY, _object.X, _object.Y);
                    id = (int)_object.Id;
                }
                if (TLBB.MapId == MAP.ThanhThuSon && _object.Title == ("Linh Thú"))
                {
                    minDistance = TDT.GetDistance(CharX, CharY, _object.X, _object.Y);
                    id = (int)_object.Id;
                }
            }
            if (id != -1)
            {
                if (!TLBB.IsQuestOpen)
                {
                    Talk(id);
                    return true;
                }
                if (TLBB.MapId == MAP.ThanhThuSon)
                {
                    foreach (QuestFrame dialog in QuestFrame.Enum(this))
                    {
                        if (dialog.Name.Contains("Giải cứu Linh Thú"))
                        {
                            QuestFrameOptionClicked(dialog);
                            CloseQuest();
                            Thread.Sleep(5000);
                            return true;
                        }
                    }
                }
                if (TLBB.MapId == MAP.LauLan)
                {
                    foreach (QuestFrame dialog in QuestFrame.Enum(this))
                    {
                        if (dialog.Name.Contains("Thiên Giáng Kỳ Thú"))
                        {
                            QuestFrameOptionClicked(dialog);
                            CloseQuest();
                            Thread.Sleep(5000);
                            return true;
                        }
                    }
                }
                foreach (QuestFrame dialog in QuestFrame.Enum(this))
                {
                    if (dialog.StrOptionExtra1 == 50013 && dialog.StrOptionExtra2 == 0xFFFFFFFF)
                    {
                        QuestFrameOptionClicked(dialog);
                        CloseQuest();
                        Thread.Sleep(5000);
                        return true;
                    }
                }
                QuestFrame.ClickAll();

                CloseQuest();
                Thread.Sleep(5000);
                return true;
            }
            return false;
        }

        public QuestFrame QuestFrame { get; set; }
        public Team Team { get; set; }

        public int Radius
        {
            get
            {
                if (RadiusX == 0)
                    return 0;
                return (int)TDT.GetDistance(RadiusX, RadiusY, CharX, CharY);
            }
        }

        public string CurGomName
        {
            get
            {
                if (CurGomDo != null)
                    return CurGomDo.TLBB.Name;
                if (CurGomDoThuong != null)
                    return CurGomDoThuong.TLBB.Name;
                return "";
            }
        }

        public string ExpSpeedPerH
        {
            get
            {
                return string.Format("{0:0.00}M", (float)ExpSpeed / 1000000) + " | " + string.Format("{0:0.00%}", (float)ExpGain / TLBB.MaxExp);
            }
        }

        public string ExpTime
        {
            get
            {
                string timeRemain = "Tận Thế";
                int expRemain = (int)TLBB.MaxExp - (int)TLBB.Exp;
                if (expRemain <= 0) timeRemain = "0S";
                else if (ExpSpeed != 0)
                {
                    double hoursRemain = (double)expRemain / ExpSpeed;
                    TimeSpan span = TimeSpan.FromHours(hoursRemain);
                    timeRemain = TimeSpanToStringEx(span);
                }
                return timeRemain;
            }
        }

        public static string TimeSpanToStringEx(TimeSpan timeSpan)
        {
            if (timeSpan.Days > 0)
                return timeSpan.Days + "D " + timeSpan.Hours + "H";
            if (timeSpan.Hours > 0)
                return timeSpan.Hours + "H " + timeSpan.Minutes + "M";
            if (timeSpan.Minutes > 0)
                return timeSpan.Minutes + "M " + timeSpan.Seconds + "S";
            return timeSpan.Seconds + "S";
        }


        public string ExpInfo
        {
            get
            {
                try
                {
                    if (TLBB.Lvl < 1 || TLBB.Lvl > 149)
                        return TimeSpanToString(AutoTime.Elapsed) + "\r\n\r\n0.00M\r\n0.00M\r\n0.00M\r\n0.00M\r\n0.00M\r\n0.00M\r\n0.00M (0.00%)\r\n\r\n00:00:00:00";
                    string timeRemain = "vô tận";
                    int expRemain = (int)TLBB.MaxExp - (int)TLBB.Exp;
                    if (expRemain <= 0) timeRemain = "0 giây";
                    else if (ExpSpeed != 0)
                    {
                        double hoursRemain = (double)expRemain / ExpSpeed;
                        TimeSpan span = TimeSpan.FromHours(hoursRemain);
                        timeRemain = TimeSpanToString(span);
                    }
                    return TimeSpanToString(AutoTime.Elapsed) + "\r\n\r\n\r\n"
                        + string.Format("{0:0.00}M\r\n", (float)ExpStart / 1000000)
                        + string.Format("{0:0.00}M\r\n", (float)TLBB.Exp / 1000000)
                        + string.Format("{0:0.00}M ({1:0.00%})\r\n", (float)ExpGain / 1000000, (float)ExpGain / TLBB.MaxExp)
                        + string.Format("{0:0.00}M\r\n", (float)expRemain / 1000000)
                        + string.Format("{0:0.00}M\r\n", (float)TLBB.MaxExp / 1000000)
                        + string.Format("{0:0.00}M ({1:0.00%})", ExpSpeed / 1000000, ExpSpeed / TLBB.MaxExp) + (IsX2 ? " ( x2 )" : "")
                        + "\r\n\r\n"
                        + timeRemain + "\r\n" + CurGomName
                        ;
                }
                catch
                {
                    string timeRemain = "vô tận";
                    int CharExpRemain = 0;
                    return TimeSpanToString(AutoTime.Elapsed) + "\r\n\r\n\r\n"
                        + string.Format("{0:0.00}M\r\n", (float)ExpStart / 1000000)
                        + string.Format("{0:0.00}M\r\n", (float)TLBB.Exp / 1000000)
                        + string.Format("{0:0.00}M ({1:0.00%})\r\n", (float)ExpGain / 1000000, (float)ExpGain / TLBB.MaxExp)
                        + string.Format("{0:0.00}M\r\n", (float)CharExpRemain / 1000000)
                        + string.Format("{0:0.00}M\r\n", (float)TLBB.MaxExp / 1000000)
                        + string.Format("{0:0.00}M ({1:0.00%})\r\n\r\n", ExpSpeed / 1000000, ExpSpeed / TLBB.MaxExp)
                        + timeRemain
                        ;
                }
            }
        }

        public static string TimeSpanToString(TimeSpan timeSpan)
        {
            if (timeSpan.Days > 0)
                return timeSpan.Days + " ngày " + timeSpan.Hours + " giờ";
            if (timeSpan.Hours > 0)
                return timeSpan.Hours + " giờ " + timeSpan.Minutes + " phút";
            if (timeSpan.Minutes > 0)
                return timeSpan.Minutes + " phút " + timeSpan.Seconds + " giây";
            return timeSpan.Seconds + " giây";
        }

        public bool IsChangeMap
        {
            get
            {
                return !Memory.IsRead(Address.ParaUseSkill);
            }
        }


        public bool IsFakeMac { get; set; }

        public void Init()
        {
            if (FuncLuaToString == 0)
            {
                if (Address.GameType == 1)
                {
                    FuncLuaToString = Memory.Scan("55 8b ec 56 57 ff 75 0c 8b 7d 08 57 e8  0f ef", "LuaPlus.dll");//558BEC 8B 450C 85 C0 7E188B 55 08 8D
                }
                else
                    FuncLuaToString = Memory.Scan("8B4424 08 85C0 56 57 8B7C24 0C 7E 11", "LuaPlus.dll");

                return;
            }
            Handle = Win.GetHandle(ProcessId, "TianLongBaBu WndClass");
            if (Handle == IntPtr.Zero)
                Handle = Win.GetHandle(ProcessId, "kezgxubdpydrpeb");

            if (Handle != IntPtr.Zero)
            {
                AutoSearch();
                SetHook(Handle);
                SetDll();
                DisableActiveGame();
                IsInit = true;
            }
            IntPtr handle = Win.GetHandle(Process.Id, "#32770");
            if (handle != IntPtr.Zero)
            {
                if (Global.IsFull > 0)
                {
                    Memory.Write(Address.MultiAcc, 0x90909090, 4);
                    Memory.Write(Address.MultiAcc + 0x4, 0x90909090, 4);
                    if (Main.DicGame.Count >= 2 && Address.GameType == 1)
                    {
                        if (Global.IsFull > 0)
                        {
                            Memory.Write(Address.MultiAcc, 0x90909090, 4);
                            Memory.Write(Address.MultiAcc + 0x4, 0x90909090, 4);
                            try
                            {
                                string sign = TDT.Hasher.Decrypt("dH9FmDVtEjIEo0lG7YUHSf36v8VKV1bBB702tEolYvs=", Global.OFFSET.Substring(0, 8));
                                string hex = TDT.Hasher.Decrypt("bOCzMlP604c=", Global.OFFSET.Substring(0, 8));
                                Main.PushLog(sign + "=>" + hex);
                                string newhex = TDT.Hasher.Decrypt("BYDcJexn12Q=", Global.OFFSET.Substring(0, 8));
                                int h = ConverterEx.Hex2Int(hex);
                                int newH = ConverterEx.Hex2Int(newhex);
                                int address = Memory.Scan(sign) + 0xE;
                                if (address != 0)
                                {
                                    Memory.Write(address, (uint)newH, 2);
                                }
                                Win.Close(handle);
                            }
                            catch { }
                        }
                    }
                    else
                    {
                        Memory.Write(Address.MultiAcc, 0x90909090, 4);
                        Memory.Write(Address.MultiAcc + 0x4, 0x90909090, 4);
                        Win.Close(handle);
                    }
                }
            }
        }






        public bool IsInit { get; set; }

        public List<int> listPhuTang = new List<int>();
        private int curPhuTang;
        public string VuKhoString = "";


        public string Status
        {
            get
            {
                if (CanQuetIdx != -1 || (threadcanquet != null && threadcanquet.IsAlive))
                    return "CànQuét";

                if (TLBB.IsMapBang(TLBB.MapId))
                {
                    if (QuestFrame.All(this).Contains("#{LJYH_141105_06}") || QuestFrame.All(this).Contains("#{DJBHGZ_110511_08}"))
                    {
                        return "XongLuyệnKim";
                    }
                }
                else if (IsSumonEx)
                {
                    if (Global.IsAdminEx)
                    {
                        return sovongsumon + " vòng Sư Môn => " + MissionState;
                    }
                    return sovongsumon + " vòng Sư Môn";
                }
                if (Missions.Contains(MissionsType.NhiemVuSuMon))
                {
                    if (Global.IsAdminEx)
                    {
                        return sovongsumon + " vòng Sư Môn => " + MissionState;
                    }
                    return sovongsumon + " vòng Sư Môn";
                }

                if (TLBB.Online)
                {
                    if (TLBB.IsODaoCuFull || TLBB.IsONguyenLieuFull || (Address.GameType == 1 && CountThienCoTrong <= 10))
                        return "Full...";
                }
                if (Missions.Contains(MissionsType.NhiemVuThangCap))
                {
                    if (CoBanFailTime.IsRunning)
                    {
                        return "Lỗi nhiệm vụ";
                    }
                }

                //if (TrangThaiNVCoBan != "")
                //    return TrangThaiNVCoBan;

                if (!TLBB.HaveRide)
                    return "Chưa có ngựa";
                if (PacketItems.VuKhi == null)
                    return "ChưaCóVũKhí";
                return "";
                //int time = TLBB.FreshmanWatchTime;
                //if (TLBB.FreshmanWatchTime > 0)i
                //    return time.ToString();
                //return "";
            }
        }

        public bool IsOnlyPick { get; set; }

        public void TriLieu()
        {
            if (!IsOneSec)
                return;
            if (TLBB.IsFollow)
                StopFollow();
            if (TLBB.HPPercent + TLBB.MPPercent > 195)
            {
                PushDebugMessage("sinh lực đã đầy. không cần trị liệu");
                RemoveMission(MissionsType.TriLieu);
                return;
            }
            if (Address.GameType == 1)
            {
                //185,336-0
                if (GoToEx(185, 336, 0))
                {
                    foreach (GameObject _object in Objects.AllNpc)//lecaotri2020
                    {
                        if (_object.CleanName == "longbathien" || _object.CleanName == "skylong")
                        {
                            Talk(_object.Id);
                            Thread.Sleep(2000);
                        }
                    }
                    for (int i = 0; i < 7; i++)
                    {
                        if (QuestFrame.Click(129, 0))
                            break;
                        Thread.Sleep(500);
                    }
                    for (int i = 0; i < 7; i++)
                    {
                        if (QuestFrame.Click(129, 1001))
                            break;
                        Thread.Sleep(500);
                    }
                    return;
                }
            }
            else
            {
                if (GoToEx(190, 335, 0))
                {
                    foreach (GameObject _object in Objects.AllNpc)
                    {
                        if (_object.CleanName == "longbathien" || _object.CleanName == "skylong")
                        {
                            Talk(_object.Id);
                            Thread.Sleep(2000);
                        }
                    }
                    QuestFrame.Click(129, 0);
                    QuestFrame.Click(129, 1001);
                    QuestFrame.Close();
                    return;
                }
            }
        }

        public void SaveGold()
        {
            if (!IsOneSec)
                return;
            if (TLBB.IsFollow)
                StopFollow();
            if (GoToEx(TLBB.NPCThuongKho))
            {
                if (!TLBB.IsBankOpen)
                {
                    if (!TLBB.IsQuestOpen)
                    {
                        Talk(TLBB.NPCThuongKho.Id);
                        return;
                    }
                    foreach (QuestFrame quest in QuestFrame.Enum(this))
                    {
                        if (quest.StrOptionExtra1 == 7 && quest.StrOptionExtra2 == 0xFFFFFFFF)
                        {
                            QuestFrameOptionClicked(quest);
                            return;
                        }
                    }
                }
                else
                {
                    DoStringEx("Bank:SaveMoneyToBank(" + TLBB.Gold + ");");
                    RemoveMission(MissionsType.CatVang);
                    CloseQuest();
                }
            }
        }


        public bool IsXuatPet { get; set; }

        private void AutoSearch()
        {
            //FuncLuaToString = Memory.Scan("8B4424 08 85C0 56 57 8B7C24 0C 7E 11", "LuaPlus.dll");
            //FuncLuaToString = Memory.Scan("55 8b ec 56 57 ff 75 0c 8b 7d 08 57 e8  0f ef", "LuaPlus.dll");//lecaotri

            uint tmp;
            if (Address.Is3D)
            {
                Address.QuestInfo[0] = AddressGameExe + 0xADD558;//0ADE528
                Address.QuestInfo[1] = 0x8;
                Address.QuestInfo[2] = 0x4;
                Address.QuestInfo[3] = 0x14C;
            }
            else if (Address.Is2D)
            {
                Address.QuestInfo[0] = AddressGameExe + 0x852FC0;//0ADE528
                Address.QuestInfo[1] = 0x8;
                Address.QuestInfo[2] = 0x4;
                Address.QuestInfo[3] = 0x144;
            }
            else
            {
                if (Address.QuestInfo[0] == 0)
                {
                    //tmp = Memory.ScanString(TDT.Sign.QuestInfo);
                    //tmp = Memory.Scan(Memory.ReverseString(tmp.ToString("X8")));
                    //if (Address.GameType == 1)
                    //    tmp = Memory.Read(tmp + 0xF);
                    //else
                    //    tmp = Memory.Read(tmp + 0x10);
                    Address.QuestInfo[0] = 0x63FB40;
                    Address.QuestInfo[1] = 0;
                    Address.QuestInfo[2] = 0xC;
                    Address.QuestInfo[3] = 0x64;
                }
            }
            if (Address.Is3D)
            {
                Address.IsBankOpen[0] = AddressGameExe + 0xADD870;
                Address.IsBankOpen[1] = 0x8;
                Address.IsBankOpen[2] = 0x4;
                Address.IsBankOpen[3] = 0x14C;
            }
            else if (Address.Is2D)
            {
                Address.IsBankOpen[0] = AddressGameExe + 0x853320;
                Address.IsBankOpen[1] = 0x8;
                Address.IsBankOpen[2] = 0x4;
                Address.IsBankOpen[3] = 0x144;
            }
            else
            {
                if (Address.IsBankOpen[0] == 0)
                {
                    tmp = (uint)Memory.ScanString("UPDATE_BANK");
                    //MessageBox.Show(tmp.ToString("X8"));
                    tmp = (uint)Memory.Scan(Memory.ReverseString(tmp.ToString("X8")));
                    //MessageBox.Show(tmp.ToString("X8"));
                    if (Address.GameType == 1)
                        tmp = Memory.Read(tmp + 0xA);
                    else
                        tmp = Memory.Read(tmp + 0x10);
                    //MessageBox.Show(tmp.ToString("X8"));
                    Address.IsBankOpen[0] = tmp;
                }
            }
            if (Address.Is3D)
            {
                Address.IsBigBankOpen[0] = AddressGameExe + 0xADD8D0;
                Address.IsBigBankOpen[1] = 0xC;
                Address.IsBigBankOpen[2] = 0x4;
                Address.IsBigBankOpen[3] = 0x14C;
            }
            else if (Address.Is2D)
            {
                Address.IsBigBankOpen[0] = AddressGameExe + 0x853398;
                Address.IsBigBankOpen[1] = 0xC;
                Address.IsBigBankOpen[2] = 0x4;
                Address.IsBigBankOpen[3] = 0x144;
            }
            else
            {
                if (Address.GameType == 1)
                {
                    if (Address.IsBigBankOpen[0] == 0)
                    {
                        tmp = (uint)Memory.ScanString("TOGLE_BIGBANK");
                        //MessageBox.Show(tmp.ToString("X8"));
                        tmp = (uint)Memory.Scan(Memory.ReverseString(tmp.ToString("X8")));
                        //MessageBox.Show(tmp.ToString("X8"));
                        tmp = Memory.Read(tmp + 0x10);
                        //MessageBox.Show(tmp.ToString("X8"));
                        Address.IsBigBankOpen[0] = tmp;
                    }
                }
            }
            if (Address.Is3D)
            {
                Address.ON_SCENE_TRANSING[0] = AddressGameExe + 0xADC99C;
                Address.ON_SCENE_TRANSING[1] = 0x8;
                Address.ON_SCENE_TRANSING[2] = 0x4;
                Address.ON_SCENE_TRANSING[3] = 0x14C;
            }
            else if (Address.Is2D)
            {
                Address.ON_SCENE_TRANSING[0] = AddressGameExe + 0x852450;
                Address.ON_SCENE_TRANSING[1] = 0x8;
                Address.ON_SCENE_TRANSING[2] = 0x4;
                Address.ON_SCENE_TRANSING[3] = 0x144;
            }
            else
            {
                if (Address.ON_SCENE_TRANSING[0] == 0)
                {
                    tmp = (uint)Memory.ScanString("ON_SCENE_TRANSING");
                    tmp = (uint)Memory.Scan(Memory.ReverseString(tmp.ToString("X8")));
                    if (Address.GameType == 1)
                        tmp = Memory.Read(tmp + 0xA);
                    else
                        tmp = Memory.Read(tmp + 0x10);
                    Address.ON_SCENE_TRANSING[0] = tmp;
                    //Main.AddLog(Address.ON_SCENE_TRANSING.ToString("X8"));
                }
            }
            //AE14B8
            if (Address.Is3D)
            {
                Address.CountDown10Sec[0] = AddressGameExe + 0xAE14B8;
                Address.CountDown10Sec[1] = 0x8;
                Address.CountDown10Sec[2] = 0x4;
                Address.CountDown10Sec[3] = 0x14C;
            }
            else if (Address.Is2D)
            {
                Address.CountDown10Sec[0] = AddressGameExe + 0x931CA0;
                Address.CountDown10Sec[1] = 0x8;
                Address.CountDown10Sec[2] = 0x4;
                Address.CountDown10Sec[3] = 0x144;
            }
            else
            {
                if (Address.CountDown10Sec[0] == 0)
                {
                    tmp = (uint)Memory.ScanString("COUNTDOWN_10SEC");
                    tmp = (uint)Memory.Scan(Memory.ReverseString(tmp.ToString("X8")));
                    tmp = Memory.Read(tmp + 0x10);
                    Address.CountDown10Sec[0] = tmp;
                }
            }
            //ADD738
            if (Address.Is3D)
            {
                Address.IsShopOpen[0] = AddressGameExe + 0xADD738;
                Address.IsShopOpen[1] = 0x8;
                Address.IsShopOpen[2] = 0x4;
                Address.IsShopOpen[3] = 0x14C;
            }
            else if (Address.Is2D)
            {
                Address.IsShopOpen[0] = AddressGameExe + 0x8530C8;
                Address.IsShopOpen[1] = 0x8;
                Address.IsShopOpen[2] = 0x4;
                Address.IsShopOpen[3] = 0x144;
            }
            else
            {
                if (Address.IsShopOpen[0] == 0)
                {
                    tmp = (uint)Memory.ScanString("UPDATE_BOOTH");
                    tmp = (uint)Memory.Scan(Memory.ReverseString(tmp.ToString("X8")));
                    if (Address.GameType == 1)
                        tmp = Memory.Read(tmp + 0x13);
                    else
                        tmp = Memory.Read(tmp + 0x10);
                    Address.IsShopOpen[0] = tmp;
                }
            }
            if (Address.Is3D)
            {
                Address.IsCreatePlayer[0] = AddressGameExe + 0xADE608;
                Address.IsCreatePlayer[1] = 0x8;
                Address.IsCreatePlayer[2] = 0x4;
                Address.IsCreatePlayer[3] = 0x14C;
            }
            else if (Address.Is2D)
            {
                Address.IsCreatePlayer[0] = AddressGameExe + 0x92F018;
                Address.IsCreatePlayer[1] = 0x8;
                Address.IsCreatePlayer[2] = 0x4;
                Address.IsCreatePlayer[3] = 0x144;
            }
            else
            {
                if (Address.IsCreatePlayer[0] == 0)
                {
                    tmp = (uint)Memory.ScanString("GAMELOGIN_CREATE_CLEAR_NAME");
                    tmp = (uint)Memory.Scan(Memory.ReverseString(tmp.ToString("X8")));
                    tmp = Memory.Read(tmp + 0x10);
                    Address.IsCreatePlayer[0] = tmp;
                }
            }
            //FuncLuaToString = Memory.Scan("8B4424 08 85C0 56 57 8B7C24 0C 7E 11", "LuaPlus.dll");
        }

        private int FuncLuaToString { get; set; }

  


        private int[] adrSendPacket = new int[100];
        private int cntSendPacket = 0;

        public void SendPacket(string hex)
        {
            hex = hex.Replace(" ", "");
            //int address = Memory.WriteHex(hex);

            byte[] arr = Memory.Hex2ByteArr(hex.Replace(" ", ""));
            Array.Resize(ref arr, arr.Length + 64);
            if (adrSendPacket[cntSendPacket] == 0)
                adrSendPacket[cntSendPacket] = (int)Memory.VirtualAllocEx(Memory.Id, 0, arr.Length, 0x1000, 0x40);
            Memory.WriteProcessMemory(Memory.Id, adrSendPacket[cntSendPacket], arr, arr.Length, 0);

            PostMessage(adrSendPacket[cntSendPacket], 114);
            cntSendPacket++;
            if (cntSendPacket > 99)
                cntSendPacket = 0;
        }


        public string HexToString(string input)
        {
            int[] Pattern = ConverterEx.Hex2IntArr(input);

            string kiemtra = Pattern[3].ToString("x2") + Pattern[2].ToString("x2") + Pattern[1].ToString("x2") + Pattern[0].ToString("x2");
            return kiemtra;
        }

        public string HexToString(uint input)
        {
            int[] Pattern = ConverterEx.Hex2IntArr(input.ToString("X8"));

            string kiemtra = Pattern[3].ToString("x2") + Pattern[2].ToString("x2") + Pattern[1].ToString("x2") + Pattern[0].ToString("x2");
            return kiemtra;
        }

        public bool IsOpenPass2 { get; set; }
        public int TimeOnMap = 0;
        public int SafeTime = 0;
        public bool IsMoBang = false;

        public int TakenKTT { get; set; }

        public bool NhatHop()
        {
            if (TLBB.MapId == MAP.LacDuong || TLBB.MapId == MAP.HuyetMo)
                return false;
            if (!IsNhatHop)
                return false;
            if (TLBB.PlayerState != 0 || !IsOneSec || TLBB.Busy)
            {
                return false;
            }
            List<uint> oIds = Objects.All.Select(o => o.Id).ToList();
            foreach (var i in listKTT.ToList())
            {
                if (!oIds.Contains(i))
                    listKTT.Remove(i);
            }
            TakenKTT = -1;
            CareObject = null;

            if (!IsOnlyPick)
            {
                CareObject = Objects.All.OrderBy(o => o.Distance).Where(o => o.IsTaiNguyenEx && listKTT.Contains(o.Id) && !AllOtherCareKTT.Contains(o.Id)).FirstOrDefault();
                if (CareObject != null)
                {
                    TakenKTT = (int)CareObject.Id;
                }
                if (CareObject == null)
                {
                    CareObject = Objects.All.OrderBy(o => o.Distance).Where(o => o.IsTaiNguyenEx && !AllOtherCareId.Contains(o.Id)).FirstOrDefault();
                }
                if (CareObject == null)
                {
                    CareObject = Objects.All.OrderBy(o => o.Distance).Where(o => o.IsTaiNguyenEx).FirstOrDefault();
                }
            }
            else
            {
                CareObject = Objects.All.OrderBy(o => o.Distance).Where(o => o.IsTaiNguyenEx && !AllOtherCareId.Contains(o.Id) && !ListPickedIds.Contains((int)o.Id)).FirstOrDefault();
                if (CareObject == null)
                {
                    CareObject = Objects.All.OrderBy(o => o.Distance).Where(o => o.IsTaiNguyenEx && !AllOtherCareId.Contains(o.Id)).FirstOrDefault();
                }
                if (CareObject == null)
                {
                    CareObject = Objects.All.OrderBy(o => o.Distance).Where(o => o.IsTaiNguyenEx).FirstOrDefault();
                }
            }

            if (CareObject != null)
            {
                DownRide();
                if (GoToEx(CareObject.X, CareObject.Y, -1, true, 2))
                {
                    if (TrueStandTime.Elapsed.TotalSeconds >= 4)
                    {
                        FixKetMap();
                    }
                    else
                    {
                        ListMoveEx.Clear();
                        MoveExTime = Stopwatch.StartNew();
                    }
                    PickItem((int)CareObject.Id);
                }
                return true;
            }
            return false;
        }



        public float GetDistance(string point, GameObject obj)
        {
            int x = TDT.ParseInt(point);
            int y = TDT.ParseInt(point.Replace(x.ToString() + ",", ""));
            return TDT.GetDistance(x, y, obj.X, obj.Y);
        }

        public float GetDistance(string point)
        {
            int x = TDT.ParseInt(point);
            int y = TDT.ParseInt(point.Replace(x.ToString() + ",", ""));
            return GetDistance(x, y);
        }

        public float GetDistance(float x, float y)
        {
            return TDT.GetDistance(RoundX, RoundY, x, y);
        }

        public string Enemy = "";


        private Stopwatch ClearTime = Stopwatch.StartNew();

        private bool IsOut
        {
            get;
            set;
        }

        public Stopwatch MeanBossDie = new Stopwatch();

        private bool IsObjectDead(string name)
        {
            return DeadObjects.Where(o => o.Key.VietLien() == name.VietLien() && o.Value.Elapsed.TotalSeconds >= 15).Count() > 0;
        }

        private bool IsTalkPhuManNghi
        {
            get;
            set;
        }

        private bool IsTalkOLaoDai
        {
            get;
            set;
        }

        private Stopwatch swNotHave { get; set; }

        private int MapATIndex = -1;
        private int CurMapATIndex = -1;

        public List<Game> QuanDoans
        {
            get
            {
                return QuanDoanParty1.Concat(QuanDoanParty2).ToList().Where(g => g.TLBB.HP > 0).ToList();
            }
        }

        private void AutoXuongNgua()
        {
            if (Setting.Is("checkXuongNgua"))
            {
                if (TrueStandTime.Elapsed.TotalSeconds > 6 && !TLBB.IsFollow)
                {
                    if (TLBB.IsRide)
                        DownRide();
                }
            }
        }

        private void CongDiemNhanVat()
        {
            if (Setting.Is("checkCongTheLuc"))
            {
                if (TLBB.Lvl <= 89)
                    DoStringEx("POINT = Player:GetData('POINT_REMAIN'); Player:SendAskManualAttr(0, 0, POINT, 0, 0);");
            }
        }

        private void AutoDiemDanh()
        {
            if (Address.GameType == 1)
            {
                if (!IsDiemDanhEx)
                {
                    IsDiemDanhEx = true;
                    DoStringEx(" setmetatable(_G, { __index = Fuli_MonthlySign_Env}); Fuli_MonthlySign_OnEvent('OPEN_MONTHLYSIGN'); Fuli_MonthlySign_DateSignClicked();");
                    Thread.Sleep(500);
                    DoStringEx("setmetatable(_G, { __index = Fuli_MonthlySign_Env}); Fuli_MonthlySign_OnClosed(); ");
                }
                if (!IsOpenPass2)
                    return;
                if (!IsDiemDanh)
                {
                    DiemDanh();
                    IsDiemDanh = true;
                }
            }
        }

        private void AutoUnlockPass2()
        {
            if (TLBB.SafeTime > 0)
            {
                IsOpenPass2 = false;
                SafeTime = 0;
            }
            else
            {
                SafeTime++;
                if (SafeTime >= 3 && Setting.Is("checkDatAnToan") && !setSafeTime && Address.GameType == 1)
                {
                    setSafeTime = true;
                    SetSafeTime();
                }
                if (!IsOpenPass2)
                {
                    if (Pass2 == string.Empty)
                    {
                        IsOpenPass2 = true;
                    }
                    else if (SafeTime >= 2)
                    {
                        UnlockPass2();
                        IsOpenPass2 = true;
                    }
                }
            }
        }

        private void AutoChucPhucBanBe()
        {
            if (!Setting.Is("checkChucPhuc"))
                return;
            if (swChucPhuc == null || swChucPhuc.Elapsed.TotalMinutes >= 60)
            {
                swChucPhuc = Stopwatch.StartNew();
                ChucPhucHaoHuu();
            }
        }

        private void GetLostLeader()
        {
            if (IsLostLeader)
            {
                if (TLBB.OnlineTimeSec < 20 && TLBB.OnlineTimeSec > 3)
                {
                    if (!TLBB.IsLeader)
                    {
                        foreach (var game in Party)
                        {
                            if (game.TLBB.IsLeader)
                            {
                                game.AppointLeader(TLBB.Name);
                                IsLostLeader = false;
                                break;
                            }
                        }
                    }
                }
            }
        }


        public bool HaveMongHeo
        {
            get
            {
                return PacketItems.NhiemVu.Any(i => i.Type == "CircularTaskTool4_7");
            }
        }

        private Stopwatch ChangeKey = Stopwatch.StartNew();
        public int StepBinhThanh { get; set; }

      

        public int quandoanindex = -1;

        public void PhanGiaiChanNguyen(uint index)
        {
            SendPacket(HexToString(AddressGameExe + 0x62FE50) + "00 00 00 00 00 00 00 00 FF FF FF FF " + index.ToString("X2") + " FF 03");
            //50 FE F0 00 00 00 00 00 00 00 00 00 FF FF FF FF 24 FF 03
        }

        public string TKCInfo { get; set; } = string.Empty;


        public Stopwatch TKCStateTime
        {
            get;
            set;
        } = Stopwatch.StartNew();

        public bool IsTraiTKC
        {
            get;
            set;
        }

        public bool TKCComplete
        {
            get;
            set;
        }

        public bool CheckTKCComplete
        {
            get;
            set;
        }

        public bool TKCompleted
        {
            get;
            set;
        }


        public bool IsSameInfo(string old, string newin)
        {
            for (int i = 0; i < 10; i++)
            {
                string p = (i + 1) + "/10";
                if (old.Contains(p) && newin.Contains(p))
                    return true;
            }
            if (old.Contains("#{CJG_090605_4}") && newin.Contains("#{CJG_090605_4}"))
                return true;
            return false;
        }

        private bool IsBossDie { get; set; }

        private bool IsNguyTongQuanDie = false;
        private int CauQ1 = 0;
        private Stopwatch swCauQ1 = Stopwatch.StartNew();
        public bool IsDuDocDie { get; set; }
        public bool IsHongHungVuongDie = false;

        public void DatDoiQ12TinhKiem()
        {
            if (!IsOneSec)
                return;
            if (!TLBB.IsLeader)
                return;
            if (Address.GameType == 1)
                return;
            if (Q12TK == 0 || Global.IsVIP == 0)
                return;
            if (TLBB.MapId == MAP.TrucLam)
            {
                if (IsHongHungVuongDie)
                {
                    if (PickItem())
                        return;
                    if (TLBB.IsFollow)
                        StopFollow();
                    GoToEx(TOCHAU.HoaKiemAnh);
                    return;
                }
                if (PickItem())
                    return;
                TrieuTap();
                if (TLBB.PlayerState != 0)
                    return;
                if (Objects.Monters.Count > 0)
                {
                    StopFollow();
                    if (TLBB.IsRide)
                        DownRide();
                    if (IsXong50NguyTongBinh)
                    {
                        foreach (GameObject _object in Objects.Monters)
                        {
                            if (_object.CleanName == "honghungvuong" && _object.HP == 0)
                            {
                                IsHongHungVuongDie = true;
                            }
                            if (_object.CleanName == "honghungvuong" && _object.Distance > 5)
                            {
                                Move(_object.X, _object.Y);
                                TrieuTap((int)_object.X, (int)_object.Y, MAP.TrucLam);
                            }
                        }
                    }
                    return;
                }
                if (IsXong50NguyTongBinh)
                {
                    Move(70, 88);
                }
                AskTeamFollow();
                string txt = SettingOld.LoadMAP(TLBB.MapId.ToString());
                if (txt.Split('-').Length < 3)
                    txt = "64,113-49,116-28,103-45,94-64,109-77,90-95,89-109,76-94,62-73,67-49,74-24,72-41,52-67,48-89,45-103,26-81,22-56,31-36,32";
                int cnt = 0;
                foreach (string s in txt.Split('-'))
                {
                    int x = 0;
                    int y = 0;
                    try
                    {
                        x = TDT.ParseInt(s.Split(',')[0]);
                        y = TDT.ParseInt(s.Split(',')[1]);
                    }
                    catch { }
                    if (x != 0 && y != 0)
                    {
                        cnt++;
                    }
                }
                if (cnt > 1)
                {
                    int[,] Point = new int[cnt, 2];
                    int c = 0;
                    foreach (string s in txt.Split('-'))
                    {
                        int x = 0;
                        int y = 0;
                        try
                        {
                            x = TDT.ParseInt(s.Split(',')[0]);
                            y = TDT.ParseInt(s.Split(',')[1]);
                        }
                        catch { }
                        if (x != 0 && y != 0)
                        {
                            Point[c, 0] = x;
                            Point[c, 1] = y;
                            c++;
                        }
                    }
                    MoveNext(Point);
                    return;
                }
                return;
            }
            if (TLBB.MapId == MAP.BienGioiTongLieu)
            {
                if (IsDuDocDie)
                {
                    if (PickItem())
                        return;
                    if (TLBB.IsFollow)
                        StopFollow();
                    GoToEx(TOCHAU.TienHongVu);
                    return;
                }
                if (IsNguyTongQuanDie)
                {
                    if (swCauQ1.Elapsed.TotalMinutes >= 2)
                    {
                        Move(56, 73);
                    }
                    else
                    {
                        TrieuTap();
                        return;
                    }
                }
                if (PickItem())
                    return;
                TrieuTap();
                if (TLBB.PlayerState != 0)
                    return;
                if (Objects.Monters.Count > 0)
                {
                    if (TLBB.IsRide)
                        DownRide();
                    StopFollow();
                    return;
                }
                foreach (GameObject _object in Objects.All)
                {
                    if (_object.CleanName == "nguytongquandothong")
                    {
                        if (_object.HP == 0)
                        {
                            swCauQ1 = Stopwatch.StartNew();
                            IsNguyTongQuanDie = true;
                        }
                    }
                    if (_object.CleanName == "dudoc")
                    {
                        if (_object.HP == 0)
                        {
                            IsDuDocDie = true;
                        }
                    }
                }
                AskTeamFollow();
                string txt = SettingOld.LoadMAP(TLBB.MapId.ToString());
                if (txt.Split('-').Length < 3)
                    txt = "55,49-33,52-27,68-32,83-47,84-67,80-82,83-98,81-101,68-101,53-80,54-63,70-56,73";
                int cnt = 0;
                foreach (string s in txt.Split('-'))
                {
                    int x = 0;
                    int y = 0;
                    try
                    {
                        x = TDT.ParseInt(s.Split(',')[0]);
                        y = TDT.ParseInt(s.Split(',')[1]);
                    }
                    catch { }
                    if (x != 0 && y != 0)
                    {
                        cnt++;
                    }
                }
                if (cnt > 1)
                {
                    int[,] Point = new int[cnt, 2];
                    int c = 0;
                    foreach (string s in txt.Split('-'))
                    {
                        int x = 0;
                        int y = 0;
                        try
                        {
                            x = TDT.ParseInt(s.Split(',')[0]);
                            y = TDT.ParseInt(s.Split(',')[1]);
                        }
                        catch { }
                        if (x != 0 && y != 0)
                        {
                            Point[c, 0] = x;
                            Point[c, 1] = y;
                            c++;
                        }
                    }
                    MoveNext(Point);
                    return;
                }
                return;
            }
            if (Q12TK == 1 || Q12TK == 3)
            {
                if (TLBB.MapId == MAP.ToChau)
                {
                    TrieuTap(TOCHAU.TienHongVu.X, TOCHAU.TienHongVu.Y, MAP.ToChau);
                    if (GoToEx(TOCHAU.TienHongVu))
                    {
                        IsP = true;
                        if (Q12TK == 3)
                        {
                            if (swStandTime.Elapsed.TotalSeconds > 60)
                            {
                                Q12TK = 2;
                            }
                        }
                    }
                }
                else
                {
                    PushDebugMessage("Vui lòng di chuyển đến tô châu");
                }
            }
            if (Q12TK == 2)
            {
                if (TLBB.MapId == MAP.ToChau)
                {
                    TrieuTap(TOCHAU.HoaKiemAnh.X, TOCHAU.HoaKiemAnh.Y, MAP.ToChau);
                    if (GoToEx(TOCHAU.HoaKiemAnh))
                    {
                        IsP = true;
                    }
                }
                else
                {
                    PushDebugMessage("Vui lòng di chuyển đến tô châu");
                }
            }
        }

        private Stopwatch QLauLanClearMonterTime = Stopwatch.StartNew();
        private int QLAULANSTATE { get; set; } = 0;
        private int QDistance { get; set; }

        private Stopwatch QToChauClearMonterTime = Stopwatch.StartNew();
        private int QTOCHAUSTATE { get; set; } = 0;

        private int Qx = 0;
        private int Qy = 0;



        public bool IsGiamDinhDo { get; set; }

        public void OpenPacketBase()
        {
            DoStringEx("PushEvent('TOGLE_CONTAINER')");
            Thread.Sleep(1000);
        }

        public void OpenEQUIP()
        {
            DoStringEx("PushEvent('OPEN_EQUIP')");
            Thread.Sleep(1000);
        }



        public void NhanBienThan()
        {
            if (!IsOneSec)
                return;
            if (GoToEx(DAILY.NhiepChinh))
            {
                if (TLBB.IsQuestOpen)
                {
                    if (QuestFrame.Click("Sư Đồ Tâm Liên Tâm"))
                    {
                        //if (!IsTrongHoa && !IsBonHoa)
                        //    IsXongBHD = true;
                        RemoveMission(MissionsType.NhanBienThan);
                    }
                    QuestFrame.Close();
                }
                else
                {
                    Talk(DAILY.NhiepChinh);
                }
            }
        }

        public void TrangBiThanKhi()
        {
            ShowPacket();

            Thread.Sleep(1000);
            PacketItem bestitem = PacketItems.All.Where(i => i.TypeName == "Ám Khí" && i.Lvl <= TLBB.Lvl).OrderByDescending(i => i.CountPoint).FirstOrDefault();
            if (bestitem != null)
            {
                DoStringEx("PushEvent('TOGLE_CONTAINER')");
                Thread.Sleep(1000);
                DoAction(bestitem.Type, bestitem.PacketId);
                Thread.Sleep(1000);
            }

            Thread.Sleep(1000);
            bestitem = PacketItems.All.Where(i => Weapon.Contains(i.TypeName) && i.Lvl <= TLBB.Lvl).OrderByDescending(i => i.CountPoint).FirstOrDefault();
            if (bestitem != null)
            {
                DoStringEx("PushEvent('TOGLE_CONTAINER')");
                Thread.Sleep(1000);
                DoAction(bestitem.Type, bestitem.PacketId);
            }
        }

        public bool IsCuongHoa7 { get; set; }

        public void Mua1CuongHoaLo()
        {
            int cnt = 0;
            foreach (var item in PacketItems.All)
            {
                if (item.ClearName.Contains("cuonghoalo"))
                {
                    cnt++;
                }
            }
            if (cnt >= 3)
                return;
            int buycnt = 3 - cnt;
            if (!TLBB.IsToggleYuanbaoShop)
            {
                LUA.ToggleYuanbaoShop();
                Thread.Sleep(100);
            }
            DoStringEx("setmetatable(_G, {__index = YuanbaoShop_Env}); YuanbaoShop_ChangeTabIndex(1);");
            Thread.Sleep(500);
            DoStringEx("setmetatable(_G, {__index = YuanbaoShop_Env}); YuanbaoShop_UpdateList_Bind(4);");
            Thread.Sleep(500);
            DoStringEx("setmetatable(_G, {__index = YuanbaoShop_Env}); YuanbaoShop_UpdateShop_Bind(2);");
            Thread.Sleep(500);
            int index = -1;
            foreach (var item in PacketItems.Shop)
            {
                if (TDT.VietLien(item.Name).Contains("cuonghoalo"))
                {
                    index = (int)item.Index;
                }
            }
            if (index != -1)
            {
                while (buycnt-- > 0)
                {
                    Buy(index);
                    Thread.Sleep(500);
                }
            }
            DoStringEx("setmetatable(_G, {__index = YuanbaoShop_Env}); this:Hide();");
        }

        public List<int> lstindex = new List<int>();

        public bool Is2Mon { get; set; }

        public void ShowPacket()
        {
            OpenPacketBase();
            //LuaDoOneLineString("setmetatable(_G, {__index = Packet_Env}); if not this:IsVisible() then this:Show(); end");
        }

        public void ShowEQUIP()
        {
            OpenEQUIP();
            //LuaDoOneLineString("setmetatable(_G, {__index = Packet_Env}); if not this:IsVisible() then this:Show(); end");
        }

        public void CuongHoa7()
        {
            if (!Global.IsLaoHuu)
                IsCuongHoa7 = false;
            if (IsCuongHoa7)
            {
                if (GoToEx(TOCHAU.AuDaTu))
                {
                    //Cường hóa trang bị
                    //#{ZBQHJ_130508_2}
                    ShowPacket();
                    PacketItem bestitem = null;

                    // Thần Khí 11
                    lstindex.Clear();
                    foreach (var item in PacketItems.All)
                        lstindex.Add((int)item.Index);

                    ShowEQUIP();
                    DoStringEx("setmetatable(_G, {__index = SelfEquip_Env}); SelfEquip_Equip_Click(11, 0);");
                    Thread.Sleep(1000);
                    bestitem = null;
                    foreach (var item in PacketItems.All)
                    {
                        if (Weapon.Contains(item.TypeName.Trim()))
                        {
                            if (lstindex.Contains((int)item.Index))
                                continue;
                            else
                                bestitem = item;
                        }
                    }
                    if (bestitem != null)
                    {
                        Mua1CuongHoaLo();
                        Talk(TOCHAU.AuDaTu);
                        Thread.Sleep(1000);
                        QuestFrame.Click("Cường hóa trang bị");
                        Thread.Sleep(1000);
                        QuestFrame.Click("#{ZBQHJ_130508_2}");
                        Thread.Sleep(1000);
                        DoStringEx("setmetatable(_G, {__index = Packet_Env}); Packet_ItemBtnClicked(" + bestitem.Row + "," + bestitem.Col + ");");
                        Thread.Sleep(1000);
                        DoStringEx("setmetatable(_G, {__index = EquipStrengthen_Env}); if this:IsVisible() then EquipStrengthen_Grade:SetText(7); EquipStrengthen_Quick_Up_Click(); end");
                        Thread.Sleep(1000);
                        LUA.MessageBox_Self2_OK_Clicked();
                        Thread.Sleep(10000);
                        DoStringEx("setmetatable(_G, {__index = EquipStrengthen_Env}); if this:IsVisible() then this:Hide(); end");
                        Thread.Sleep(1000);
                        //LuaDoOneLineString("setmetatable(_G, {__index = Packet_Env}); Packet_ItemBtnClicked(" + bestitem.Row + "," + bestitem.Col + ");");
                        bestitem.DoAction();
                        Thread.Sleep(1000);
                    }

                    // Ám Khí 14
                    lstindex.Clear();
                    foreach (var item in PacketItems.All)
                        lstindex.Add((int)item.Index);
                    ShowEQUIP();
                    DoStringEx("setmetatable(_G, {__index = SelfEquip_Env}); SelfEquip_Equip_Click(14, 0);");
                    Thread.Sleep(1000);
                    bestitem = null;
                    foreach (var item in PacketItems.All)
                    {
                        if (item.TypeName == "Ám Khí")
                        {
                            if (lstindex.Contains((int)item.Index))
                                continue;
                            else
                                bestitem = item;
                        }
                    }
                    if (bestitem != null)
                    {
                        Mua1CuongHoaLo();
                        Talk(TOCHAU.AuDaTu);
                        Thread.Sleep(1000);
                        QuestFrame.Click("Cường hóa trang bị");
                        Thread.Sleep(1000);
                        QuestFrame.Click("#{ZBQHJ_130508_2}");
                        Thread.Sleep(1000);
                        DoStringEx("setmetatable(_G, {__index = Packet_Env}); Packet_ItemBtnClicked(" + bestitem.Row + "," + bestitem.Col + ");");
                        Thread.Sleep(1000);
                        DoStringEx("setmetatable(_G, {__index = EquipStrengthen_Env}); if this:IsVisible() then EquipStrengthen_Grade:SetText(7); EquipStrengthen_Quick_Up_Click(); end");
                        Thread.Sleep(1000);
                        LUA.MessageBox_Self2_OK_Clicked();
                        Thread.Sleep(10000);
                        DoStringEx("setmetatable(_G, {__index = EquipStrengthen_Env}); if this:IsVisible() then this:Hide(); end");
                        Thread.Sleep(1000);
                        // LuaDoOneLineString("setmetatable(_G, {__index = Packet_Env}); Packet_ItemBtnClicked(" + bestitem.Row + "," + bestitem.Col + ");");
                        bestitem.DoAction();
                        Thread.Sleep(1000);
                    }
                    if (!Is2Mon)
                    {
                        // Y Phục 12
                        lstindex.Clear();
                        foreach (var item in PacketItems.All)
                            lstindex.Add((int)item.Index);
                        ShowEQUIP();
                        DoStringEx("setmetatable(_G, {__index = SelfEquip_Env}); SelfEquip_Equip_Click(12, 0);");
                        Thread.Sleep(1000);
                        bestitem = null;
                        foreach (var item in PacketItems.All)
                        {
                            if (item.TypeName == "Y Phục")
                            {
                                if (lstindex.Contains((int)item.Index))
                                    continue;
                                else
                                    bestitem = item;
                            }
                        }
                        if (bestitem != null)
                        {
                            Mua1CuongHoaLo();
                            Talk(TOCHAU.AuDaTu);
                            Thread.Sleep(1000);
                            QuestFrame.Click("Cường hóa trang bị");
                            Thread.Sleep(1000);
                            QuestFrame.Click("#{ZBQHJ_130508_2}");
                            Thread.Sleep(1000);
                            DoStringEx("setmetatable(_G, {__index = Packet_Env}); Packet_ItemBtnClicked(" + bestitem.Row + "," + bestitem.Col + ");");
                            Thread.Sleep(1000);
                            DoStringEx("setmetatable(_G, {__index = EquipStrengthen_Env}); if this:IsVisible() then EquipStrengthen_Grade:SetText(7); EquipStrengthen_Quick_Up_Click(); end");
                            Thread.Sleep(1000);
                            LUA.MessageBox_Self2_OK_Clicked();
                            Thread.Sleep(10000);
                            DoStringEx("setmetatable(_G, {__index = EquipStrengthen_Env}); if this:IsVisible() then this:Hide(); end");
                            Thread.Sleep(1000);
                            //LuaDoOneLineString("setmetatable(_G, {__index = Packet_Env}); Packet_ItemBtnClicked(" + bestitem.Row + "," + bestitem.Col + ");");
                            bestitem.DoAction();
                            Thread.Sleep(1000);
                        }

                        // hộ phu 10
                        lstindex.Clear();
                        foreach (var item in PacketItems.All)
                            lstindex.Add((int)item.Index);
                        ShowEQUIP();
                        DoStringEx("setmetatable(_G, {__index = SelfEquip_Env}); SelfEquip_Equip_Click(10, 0);");
                        Thread.Sleep(1000);

                        foreach (var item in PacketItems.All)
                        {
                            if (item.TypeName == "Hộ Phù")
                            {
                                if (lstindex.Contains((int)item.Index))
                                    continue;
                                else
                                    bestitem = item;
                            }
                        }
                        if (bestitem != null)
                        {
                            Mua1CuongHoaLo();
                            Talk(TOCHAU.AuDaTu);
                            Thread.Sleep(1000);
                            QuestFrame.Click("Cường hóa trang bị");
                            Thread.Sleep(1000);
                            QuestFrame.Click("#{ZBQHJ_130508_2}");
                            Thread.Sleep(1000);
                            DoStringEx("setmetatable(_G, {__index = Packet_Env}); Packet_ItemBtnClicked(" + bestitem.Row + "," + bestitem.Col + ");");
                            Thread.Sleep(1000);
                            DoStringEx("setmetatable(_G, {__index = EquipStrengthen_Env}); if this:IsVisible() then EquipStrengthen_Grade:SetText(7); EquipStrengthen_Quick_Up_Click(); end");
                            Thread.Sleep(1000);
                            LUA.MessageBox_Self2_OK_Clicked();
                            Thread.Sleep(10000);
                            DoStringEx("setmetatable(_G, {__index = EquipStrengthen_Env}); if this:IsVisible() then this:Hide(); end");
                            Thread.Sleep(1000);
                            // LuaDoOneLineString("setmetatable(_G, {__index = Packet_Env}); Packet_ItemBtnClicked(" + bestitem.Row + "," + bestitem.Col + ");");
                            bestitem.DoAction();
                            Thread.Sleep(1000);
                        }
                        // hộ phù 9
                        lstindex.Clear();
                        foreach (var item in PacketItems.All)
                            lstindex.Add((int)item.Index);
                        ShowEQUIP();
                        DoStringEx("setmetatable(_G, {__index = SelfEquip_Env}); SelfEquip_Equip_Click(9, 0);");
                        Thread.Sleep(1000);
                        bestitem = null;
                        foreach (var item in PacketItems.All)
                        {
                            if (item.TypeName == "Hộ Phù")
                            {
                                if (lstindex.Contains((int)item.Index))
                                    continue;
                                else
                                    bestitem = item;
                            }
                        }
                        if (bestitem != null)
                        {
                            Mua1CuongHoaLo();
                            Talk(TOCHAU.AuDaTu);
                            Thread.Sleep(1000);
                            QuestFrame.Click("Cường hóa trang bị");
                            Thread.Sleep(1000);
                            QuestFrame.Click("#{ZBQHJ_130508_2}");
                            Thread.Sleep(1000);
                            DoStringEx("setmetatable(_G, {__index = Packet_Env}); Packet_ItemBtnClicked(" + bestitem.Row + "," + bestitem.Col + ");");
                            Thread.Sleep(1000);
                            DoStringEx("setmetatable(_G, {__index = EquipStrengthen_Env}); if this:IsVisible() then EquipStrengthen_Grade:SetText(7); EquipStrengthen_Quick_Up_Click(); end");
                            Thread.Sleep(1000);
                            LUA.MessageBox_Self2_OK_Clicked();
                            Thread.Sleep(10000);
                            DoStringEx("setmetatable(_G, {__index = EquipStrengthen_Env}); if this:IsVisible() then this:Hide(); end");
                            Thread.Sleep(1000);
                            //LuaDoOneLineString("setmetatable(_G, {__index = Packet_Env}); Packet_ItemBtnClicked(" + bestitem.Row + "," + bestitem.Col + ");");
                            bestitem.DoAction();
                            Thread.Sleep(1000);
                        }
                        // nhẫn 8
                        lstindex.Clear();
                        foreach (var item in PacketItems.All)
                            lstindex.Add((int)item.Index);
                        ShowEQUIP();
                        DoStringEx("setmetatable(_G, {__index = SelfEquip_Env}); SelfEquip_Equip_Click(8, 0);");
                        Thread.Sleep(1000);
                        bestitem = null;
                        foreach (var item in PacketItems.All)
                        {
                            if (item.TypeName == "Giới Chỉ")
                            {
                                if (lstindex.Contains((int)item.Index))
                                    continue;
                                else
                                    bestitem = item;
                            }
                        }
                        if (bestitem != null)
                        {
                            Mua1CuongHoaLo();
                            Talk(TOCHAU.AuDaTu);
                            Thread.Sleep(1000);
                            QuestFrame.Click("Cường hóa trang bị");
                            Thread.Sleep(1000);
                            QuestFrame.Click("#{ZBQHJ_130508_2}");
                            Thread.Sleep(1000);
                            DoStringEx("setmetatable(_G, {__index = Packet_Env}); Packet_ItemBtnClicked(" + bestitem.Row + "," + bestitem.Col + ");");
                            Thread.Sleep(1000);
                            DoStringEx("setmetatable(_G, {__index = EquipStrengthen_Env}); if this:IsVisible() then EquipStrengthen_Grade:SetText(7); EquipStrengthen_Quick_Up_Click(); end");
                            Thread.Sleep(1000);
                            LUA.MessageBox_Self2_OK_Clicked();
                            Thread.Sleep(10000);
                            DoStringEx("setmetatable(_G, {__index = EquipStrengthen_Env}); if this:IsVisible() then this:Hide(); end");
                            Thread.Sleep(1000);
                            //LuaDoOneLineString("setmetatable(_G, {__index = Packet_Env}); Packet_ItemBtnClicked(" + bestitem.Row + "," + bestitem.Col + ");");
                            //
                            bestitem.DoAction();
                            Thread.Sleep(1000);
                        }
                        // nhẫn 7
                        lstindex.Clear();
                        foreach (var item in PacketItems.All)
                            lstindex.Add((int)item.Index);
                        ShowEQUIP();
                        DoStringEx("setmetatable(_G, {__index = SelfEquip_Env}); SelfEquip_Equip_Click(7, 0);");
                        Thread.Sleep(1000);
                        bestitem = null;
                        foreach (var item in PacketItems.All)
                        {
                            if (item.TypeName == "Giới Chỉ")
                            {
                                if (lstindex.Contains((int)item.Index))
                                    continue;
                                else
                                    bestitem = item;
                            }
                        }
                        if (bestitem != null)
                        {
                            Mua1CuongHoaLo();
                            Talk(TOCHAU.AuDaTu);
                            Thread.Sleep(1000);
                            QuestFrame.Click("Cường hóa trang bị");
                            Thread.Sleep(1000);
                            QuestFrame.Click("#{ZBQHJ_130508_2}");
                            Thread.Sleep(1000);
                            DoStringEx("setmetatable(_G, {__index = Packet_Env}); Packet_ItemBtnClicked(" + bestitem.Row + "," + bestitem.Col + ");");
                            Thread.Sleep(1000);
                            DoStringEx("setmetatable(_G, {__index = EquipStrengthen_Env}); if this:IsVisible() then EquipStrengthen_Grade:SetText(7); EquipStrengthen_Quick_Up_Click(); end");
                            Thread.Sleep(1000);
                            LUA.MessageBox_Self2_OK_Clicked();
                            Thread.Sleep(10000);
                            DoStringEx("setmetatable(_G, {__index = EquipStrengthen_Env}); if this:IsVisible() then this:Hide(); end");
                            Thread.Sleep(1000);
                            // LuaDoOneLineString("setmetatable(_G, {__index = Packet_Env}); Packet_ItemBtnClicked(" + bestitem.Row + "," + bestitem.Col + ");");
                            bestitem.DoAction();
                            Thread.Sleep(1000);
                        }
                    }
                    Talk(TOCHAU.AuDaTu);
                    Thread.Sleep(1000);
                    QuestFrame.Click("#{QHLFT_0614_07}");
                    Thread.Sleep(1000);
                    QuestFrame.Click("#{QHLFT_0614_11}");
                    Thread.Sleep(1000);
                    QuestFrame.Close();
                    Thread.Sleep(1000);
                    Talk(TOCHAU.AuDaTu);
                    Thread.Sleep(1000);
                    QuestFrame.Click("#{QHLFT_0614_07}");
                    Thread.Sleep(1000);
                    QuestFrame.Click("#{QHLFT_0614_11}");
                    Thread.Sleep(1000);
                    QuestFrame.Close();
                    Thread.Sleep(1000);
                    Talk(TOCHAU.AuDaTu);
                    Thread.Sleep(1000);
                    QuestFrame.Click("#{QHLFT_0614_07}");
                    Thread.Sleep(1000);
                    QuestFrame.Click("#{QHLFT_0614_11}");
                    Thread.Sleep(1000);
                    QuestFrame.Close();
                    Thread.Sleep(1000);
                    //#{QHLFT_0614_07}
                    //#{QHLFT_0614_11}
                    IsCuongHoa7 = false;
                }
            }
        }
        public int MuaX2 { get; set; }

        public void Muax2Shop()
        {
            if (MuaX2 > 0)
            {
                if (!TLBB.IsToggleYuanbaoShop)
                {
                    LUA.ToggleYuanbaoShop();
                    Thread.Sleep(100);
                }
                DoStringEx("setmetatable(_G, {__index = YuanbaoShop_Env}); NpcShop:SetBuyDirectly(1);");
                Thread.Sleep(500);
                DoStringEx("setmetatable(_G, {__index = YuanbaoShop_Env}); YuanbaoShop_ChangeTabIndex(1);");
                Thread.Sleep(500);
                DoStringEx("setmetatable(_G, {__index = YuanbaoShop_Env}); YuanbaoShop_UpdateList_Bind(1);");
                Thread.Sleep(1000);
                int index = -1;
                foreach (var item in PacketItems.Shop)
                {
                    if (TDT.VietLien(item.Name).Contains("thienlinhdan"))
                    {
                        index = (int)item.Index;
                    }
                }
                if (index != -1)
                {
                    while (MuaX2-- > 0)
                    {
                        Buy(index);
                        Thread.Sleep(500);
                    }
                }
                MuaX2 = 0;
                DoStringEx("setmetatable(_G, {__index = YuanbaoShop_Env}); this:Hide();");
            }
        }

        //Giám Định Phù 8
        //Giám Định Phù 9
        //Giám Định Phù Thường Cấp 10
        public void GiamDinhDo()
        {
            if (!Global.IsLaoHuu)
                IsGiamDinhDo = false;
            if (!IsGiamDinhDo)
                return;
            IsGiamDinhDo = false;
            foreach (var item in PacketItems.All)
            {
                if (GAMEDIC.TrangBiLaoHuu.Contains(item.Name))
                {
                    if (item.Star == 0)
                    {
                        int lv = (int)item.Lvl;
                        foreach (var i in PacketItems.All)
                        {
                            if (lv <= 80)
                            {
                                if ((TDT.VietLien(i.Name).Contains("giamdinhphu") || TDT.VietLien(i.Name).Contains("giamdinhthu")) && (i.Name.Contains("8") || i.Name.Contains("9") || i.Name.Contains("10")))
                                {
                                    DoStringEx("PushEvent('TOGLE_CONTAINER')");
                                    Thread.Sleep(300);
                                    DoStringEx("setmetatable(_G, {__index = Packet_Env}); Packet_ItemBtnClicked(" + i.Row + "," + i.Col + ");");
                                    Thread.Sleep(1000);
                                    DoStringEx("setmetatable(_G, {__index = Packet_Env}); Packet_ItemBtnSubClicked(" + item.Row + "," + item.Col + ");");
                                    Thread.Sleep(1000);
                                    break;
                                }
                            }
                            else if (lv <= 90)
                            {
                                if ((TDT.VietLien(i.Name).Contains("giamdinhphu") || TDT.VietLien(i.Name).Contains("giamdinhthu")) && (i.Name.Contains("9") || i.Name.Contains("10")))
                                {
                                    DoStringEx("PushEvent('TOGLE_CONTAINER')");
                                    Thread.Sleep(300);
                                    DoStringEx("setmetatable(_G, {__index = Packet_Env}); Packet_ItemBtnClicked(" + i.Row + "," + i.Col + ");");
                                    Thread.Sleep(1000);
                                    DoStringEx("setmetatable(_G, {__index = Packet_Env}); Packet_ItemBtnSubClicked(" + item.Row + "," + item.Col + ");");
                                    Thread.Sleep(1000);
                                    break;
                                }
                            }
                            else if (lv <= 100)
                            {
                                if ((TDT.VietLien(i.Name).Contains("giamdinhphu") || TDT.VietLien(i.Name).Contains("giamdinhthu")) && (i.Name.Contains("9") || i.Name.Contains("10")))
                                {
                                    DoStringEx("PushEvent('TOGLE_CONTAINER')");
                                    Thread.Sleep(300);
                                    DoStringEx("setmetatable(_G, {__index = Packet_Env}); Packet_ItemBtnClicked(" + i.Row + "," + i.Col + ");");
                                    Thread.Sleep(10000);
                                    DoStringEx("setmetatable(_G, {__index = Packet_Env}); Packet_ItemBtnSubClicked(" + item.Row + "," + item.Col + ");");
                                    Thread.Sleep(10000);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        private bool isQuangCao;

        private bool IsQuangCao
        {
            get
            {
                if (Global.RemoveAd)
                    return true;
                return isQuangCao;
            }
            set
            {
                isQuangCao = value;
            }
        }

        public Stopwatch BossDieTime = Stopwatch.StartNew();

        public int MoveAcBaIndex { get; set; } = 0;

        private bool DaNhanThuyLao { get; set; }

        private string TrangThaiThuyLao = "";

        private void DiThuyLao()
        {
            if (TLBB.MapId != MAP.ThaiHo && TLBB.MapId != MAP.ToChau && TLBB.MapId != MAP.ThuyLao)
            {
                PushDebugMessage("vui lòng di chuyển đến thái hồ hoặc tô châu");
                return;
            }
            if (GoToEx(THAIHO.HoDienKhanh))
            {
                if (TLBB.IsQuestOpen)
                {
                    if (!IsClick)
                    {
                        QuestFrameOptionClicked(232002, -1);
                        IsClick = true;
                    }
                    else
                    {
                        QuestFrameAccept();
                        IsClick = false;
                        CloseQuest();
                    }
                }
                else
                {
                    Talk(THAIHO.HoDienKhanh);
                }
            }
        }

        private void NhanThuyLao()
        {
            if (DaNhanThuyLao)
                return;
            if (TrangThaiThuyLao == "")
            {
                if (!TLBB.IsTogleMission)
                {
                    if (!TLBB.IsTogleMission)
                    {
                        PostMessage(18, 105);
                    }
                }
                TrangThaiThuyLao = "OpenMission";
                return;
            }
            if (TrangThaiThuyLao == "OpenMission")
            {
                PostMessage(18, 105);
                TrangThaiThuyLao = "CloseMission";
                return;
            }
            if (TrangThaiThuyLao == "CloseMission")
            {
                foreach (GameTask task in GameTask.Enum(this))
                {
                    if (task.ClearName.Contains("binhdinhthuylao"))
                    {
                        DaNhanThuyLao = true;
                        TrangThaiThuyLao = "";
                        return;
                    }
                }
                TrangThaiThuyLao = "NhanThuyLao";
                return;
            }
            if (TrangThaiThuyLao == "NhanThuyLao")
            {
                if (TLBB.MapId != MAP.ThaiHo && TLBB.MapId != MAP.ToChau && TLBB.MapId != MAP.ThuyLao)
                {
                    PushDebugMessage("vui lòng di chuyển đến thái hồ hoặc tô châu");
                    return;
                }
                if (GoToEx(TOCHAU.HoDienBao))
                {
                    if (TLBB.IsQuestOpen)
                    {
                        if (!IsClick)
                        {
                            QuestFrameOptionClicked(232000, -1);
                            IsClick = true;
                        }
                        else
                        {
                            QuestFrameAccept();
                            IsClick = false;
                            CloseQuest();
                            TrangThaiThuyLao = "";
                        }
                    }
                    else
                    {
                        Talk(TOCHAU.HoDienBao);
                        Talk(TOCHAU.HoDienBaoHoaiNiem);
                    }
                }
                return;
            }
        }

        public void DatDoiThuyLao()
        {
            if (!IsThuyLao)
                return;
            if (!IsOneSec)
                return;
            if (PickItem())
                return;
            TrieuTap();
            if (TLBB.MapId == MAP.ThuyLao)
            {
                if (IsXongThuyLao)
                {
                    //if (!IsQuangCao)
                    //{
                    //    LuaDoUnicodeString("TXT = '#cFF0000 TDT#cffcccc với #GMicroAut0#cffcccc giúp ta hoàn thành phụ bản#cFF0000 Thủy Lao#cffcccc từ a->z chỉ với 1 click, quả là aut0 hoàn hảo';");
                    //    PostMessage(3, 105);
                    //    IsQuangCao = true;
                    //}
                    GoToEx(TOCHAU.HoDienBao);
                    return;
                }
                if (Objects.NearMonter(20).Count > 0)
                {
                    if (TLBB.PlayerState == 0)
                    {
                        if (TLBB.IsFollow)
                            StopFollow();
                        if (TLBB.IsRide)
                            DownRide();
                    }
                    ClearTime = Stopwatch.StartNew();
                }
                if (ClearTime.Elapsed.TotalSeconds > 2)
                {
                    if (!TLBB.IsRide && TLBB.HaveRide)
                    {
                        UpRide();
                        return;
                    }
                    MoveNext();
                    return;
                }
            }
            else
            {
                if (!DaNhanThuyLao)
                {
                    NhanThuyLao();
                }
                else
                {
                    DiThuyLao();
                }
            }
        }

        public bool IsClearFistRound { get; set; }

        public int CanQuetIdx { get; set; } = -1;

        public void CanQuet()
        {
            if (!Global.IsCanQuet)
                CanQuetIdx = -1;
            if (CanQuetIdx != -1)
            {
                int tmp = CanQuetIdx;
                CanQuetIdx = -1;
                Task.Run(() =>
                {
                    DoStringEx("setmetatable(_G, {__index = PlayerQuicklyEnter_Env}); PlayerQuicklyEnter_Clicked(10);");
                    DoStringEx(@"Clear_XSCRIPT();
		Set_XSCRIPT_Function_Name('OnFuBenSecKill');
		Set_XSCRIPT_ScriptID(891062);
		Set_XSCRIPT_Parameter(0," + tmp + @");
		Set_XSCRIPT_Parameter(1,1);
		Set_XSCRIPT_Parameter(2,0);
		Set_XSCRIPT_ParamCount(3);
	Send_XSCRIPT();");

                    Thread.Sleep(1000);

                    NhanDoCanQuet();
                });
            }
        }

        private Thread threadcanquet;

        public void CanQuetAll()
        {
            if (!Global.IsCanQuet)
                CanQuetIdx = -1;
            if (threadcanquet != null)
            {
                if (threadcanquet.IsAlive)
                {
                    try
                    {
                        threadcanquet.Abort();
                    }
                    catch { }
                }
                threadcanquet = null;
            }
            threadcanquet = new Thread(() =>
            {
                DoStringEx("setmetatable(_G, {__index = PlayerQuicklyEnter_Env}); PlayerQuicklyEnter_Clicked(10);");
                Thread.CurrentThread.IsBackground = true;
                for (int i = 0; i < 12; i++)
                {
                    DoStringEx(@"Clear_XSCRIPT();
		Set_XSCRIPT_Function_Name('OnFuBenSecKill');
		Set_XSCRIPT_ScriptID(891062);
		Set_XSCRIPT_Parameter(0," + i + @");
		Set_XSCRIPT_Parameter(1,1);
		Set_XSCRIPT_Parameter(2,0);
		Set_XSCRIPT_ParamCount(3);
	Send_XSCRIPT();");

                    Thread.Sleep(1000);

                    NhanDoCanQuet();
                }
            });
            threadcanquet.Start();
        }

        public void NhanDoCanQuet()
        {
            for (int i = 0; i < 30; i++)
            {
                DoStringEx("if SecKillItemListEmpty() ~=1 then SecKillRemoveItem(); return 'CanQuet'; end isInSecKill,FubenID,BossIndex,DoubleExp = GetSecKillData(); if isInSecKill == 1 then Clear_XSCRIPT(); Set_XSCRIPT_Function_Name('OnFuBenSecKill'); Set_XSCRIPT_ScriptID(891062); Set_XSCRIPT_Parameter(0, FubenID); Set_XSCRIPT_Parameter(1, BossIndex + 1); Set_XSCRIPT_Parameter(2, 0); Set_XSCRIPT_ParamCount(3); Send_XSCRIPT(); return 'CanQuet'; end return 'Xong';");
                LuaToStringThread();
                Thread.Sleep(700);
                string xong = LuaToStringThread();
                if (xong == "Xong")
                    break;
            }
        }

        public int ClearSatTinhCount { get; set; }

        public void MuaDoShop()
        {
            if (!Global.IsBachBao || !IsOneSec || !Main.SettingForm.checkBuyItem.Checked)
                return;
            if (TLBB.IsShopHungBaOpen)
            {
                string note = ("*" + Game.IniParser.Read("BachBao", "All") + "*" + Game.IniParser.Read("BachBao", TLBB.AllowName) + "*").Replace("**", "*").Replace("*", "1").Trim();

                if (PacketItems.TrangBi.Concat(PacketItems.All).Where(i => i.TypeName == "Hào Hiệp Ấn" && i.Name.Contains("Bát Cực")).FirstOrDefault() != null)
                    note = note.Replace("Hào Hiệp Huy Hiệu", "");

                DoStringEx(@"BACH = '" + note + "'; " +
                    @"for i = 0, 21 do

	                        theAction,isSoldOut = City:EnumHonourShop(i);

	                        if ( theAction:GetID() ~= 0 ) then
		                        nRemainNum = City:GetHonourShopInfo( i, 'remainnum' );
                                if string.find(BACH, theAction:GetName() .. '1') then
                                     if nRemainNum >= 0 then
                                        City:DoHonourShopBuy( i , nRemainNum);
                                    end
		                        end
	                        end

                        end"
                    );
            }
            if (TLBB.IsShopQuyThiOpen)
            {
                string note = (Game.IniParser.Read("QuyThi", "All") + "*" + Game.IniParser.Read("QuyThi", TLBB.AllowName)).Replace("**", "*").Trim();

                DoStringEx(@"BACH = '" + note + "'; " +
                    @"for i = 0, 11 do
	                        local result, name, isLimited, limitedNum, buyNum, needType1, needNum1, needStep1, needType2, needNum2, needStep2= GuiShiUI:LuaFnGetNPCShopGoodsInfo(i)
	                        local theAction = EnumAction(i, 'gs_npcshop_goodslist')
                            if result == 0 then
                                        continue
                                    end
                            if theAction:GetID() == 0 then
                                continue
                            end
                            if limitedNum > 0 and string.find(BACH, name) then
                                Clear_XSCRIPT()
                                Set_XSCRIPT_Function_Name('onBuy')
                                Set_XSCRIPT_ScriptID(893190)
                                Set_XSCRIPT_Parameter(0, 0)
                                Set_XSCRIPT_Parameter(1, i + 104)
                                Set_XSCRIPT_Parameter(2, limitedNum)
                                Set_XSCRIPT_Parameter(3, 4)
                                Set_XSCRIPT_Parameter(4, 0)
                                Set_XSCRIPT_ParamCount(5)
                                Send_XSCRIPT()
                            end
                        end");
            }
            if (TLBB.IsShopTrungDoOpen)
            {
                string note = ("*" + Game.IniParser.Read("BachBao", "All") + "*" + Game.IniParser.Read("BachBao", TLBB.AllowName) + "*").Replace("**", "*").Replace("*", "1").Trim();
                if (PacketItems.TrangBi.Where(p => Weapon.Contains(p.TypeName) && (TDT.ParseAllInt(p.Star.ToString().Substring(0, 1)) >= 8 || TDT.ParseAllInt(p.Star.ToString()) >= 80)).FirstOrDefault() != null)
                    note = note.Replace("Huyền Binh Thạch", "");
                DoStringEx(@"BACH = '" + note + "'; " +
                    @"for i = 0, 30 do
                            local item_shop_index = DataPool:LuaFnGetT3ShopItemInfo(i, 'SHOPINDEX')
                         theAction = EnumAction(item_shop_index, 't3shop')

                         if ( theAction:GetID() ~= 0 ) then
                          nRemainNum = DataPool:LuaFnGetT3ShopItemInfo(i, 'PLAYERREMAIN')
                          if nRemainNum > 0 and (string.find(BACH, theAction:GetName() .. '1') or (string.find(BACH, 't.Tốt') and string.find(theAction:GetName(), 't.Tốt')) or (string.find(BACH, 't.Tinh') and string.find(theAction:GetName(), 't.Tinh'))  or (string.find(BACH, 'c.Tốt') and string.find(theAction:GetName(), 'c.Tốt')) ) then
                            NpcShop:LuaFnT3ShopBuyItem(i, nRemainNum)
                          end
                         end

                        end"
                    );
            }
            if (TLBB.IsShopBachBaoOpen)
            {
                string note = (Game.IniParser.Read("BachBao", "All") + "*" + Game.IniParser.Read("BachBao", TLBB.AllowName)).Replace("**", "*").Trim();

                if (PacketItems.TrangBi.Concat(PacketItems.All).Where(i => i.TypeName == "Bang Hội Lệnh Bài" && i.Star >= 8).FirstOrDefault() != null)
                    note = note.Replace("Hoán Linh Dịch", "");

                DoStringEx(@"BACH = '" + note + "'; " +
                    @"for i = 1, 16 do

	                        theAction,isSoldOut = City:EnumGuildShop(i);

	                        if ( theAction:GetID() ~= 0 ) then
		                        nRemainNum = City:GetGuildShopInfo(i, 'MaxNumber');
		                        if string.find(BACH, theAction:GetName()) then
                                     if nRemainNum >= 0 then
			                            City:DoGuildShopBuy( i , nRemainNum );
                                     end
		                        end
	                        end

                        end"
                    );
            }
        }

        public void HopVoHon(int first, int second)
        {
            //6D8CB8
            SendPacket(HexToString(AddressGameExe + 0x6D8CB8) + "00 00 00 00 00 00 00 00 FF FF FF FF 03 00 00 00" + first.ToString("X2") + second.ToString("X2"));
        }


        public void DungDoatBaoRuong()
        {
            if (!Global.IsDungDoat)
            {
                RemoveMission(MissionsType.DungDoatBaoRuong);
            }
            List<string> list = new List<string>()
            {
                "Sào Ỷ Phong",            "Sào Linh Lâm",    "Hoa Triệu Lâm",  "Tiêu Ngô Tịch"
            };

            foreach (GameObject obj in Objects.All)
            {
                if (list.Contains(obj.Name))
                {
                    Talk(obj.Id);
                    Thread.Sleep(1000);
                    QuestFrame.Click("#{VNYDBX_151028_15}");
                    Thread.Sleep(1000);
                    QuestFrame.Click("#{VNYDBX_151028_06}");
                }
            }
        }

        public bool TrieuTap(int x, int y, int mapId)
        {
            bool isTrieuTap = false;
            foreach (Game game in Party)
            {
                if (game.SWHoiSinh.Elapsed.TotalSeconds < 12 || game.tranTime.Elapsed.TotalSeconds < 2 || game.NeBayTime.Elapsed.TotalSeconds < 5)
                    continue;
                if (game.PickItem())
                    continue;
                if (IsThuyLao)
                {
                    if (game.TLBB.MapId != MAP.ThuyLao || TLBB.MapId != MAP.ThuyLao)
                    {
                        if (!game.DaNhanThuyLao)
                        {
                            game.NhanThuyLao();
                        }
                        else
                        {
                            game.DiThuyLao();
                        }
                        continue;
                    }
                    else
                    {
                        if (game.PickItem())
                            continue;
                        if (game.Objects.NearMonter(20).Count > 0)
                        {
                            if (game.TLBB.PlayerState == 0)
                            {
                                if (game.TLBB.IsFollow)
                                    game.StopFollow();
                                if (game.TLBB.IsRide)
                                    game.DownRide();
                            }
                        }
                        else
                        {
                            if (!game.TLBB.IsRide && game.TLBB.HaveRide)
                            {
                                game.UpRide();
                            }
                        }
                        if (TDT.GetDistance(game.RoundX, game.RoundY, x, y) > 4)
                        {
                            game.GoToEx(x, y);
                        }
                        continue;
                    }
                }
                //if (game.TLBB.KeyId != TLBB.Id || game.TLBB.IsLeader || (game.TLBB.PlayerState == 2 && game.TLBB.MapId != MAP.TuTuyetTrang))
                //{
                //    game.IsTrieuTap = false;
                //    continue;
                //}

                if ((game.TLBB.MapId == MAP.ViemMaSon || game.TLBB.MapId == MAP.TamTaiHiepCoc) && game.TLBB.IsFollow)
                    continue;
                if (game.IsMapPhuBan())
                {
                    if ((game.Objects.NearMonter12m.Count > 0 || ((game.IsMapAcBa || game.TLBB.MapId == MAP.TangKinhCac) && game.Objects.NearMonter(20).Count > 0)) && (game.TLBB.PlayerState == 0 || game.TLBB.MapId == MAP.TuTuyetTrang))
                    {
                        if (game.TLBB.MapId != MAP.ViemMaSon && game.TLBB.MapId != MAP.TamTaiHiepCoc)
                            game.StopFollow();
                        if (game.TLBB.IsRide)
                        {
                            game.DownRide();
                        }
                    }
                    //else if(game.TLBB.MapId == MAP.TangKinhCac && game.TLBB.PlayerState == 0 && game.Objects.Near20m.Count > 0)
                    //{
                    //    if (game.TLBB.IsFollow)
                    //        game.StopFollow();
                    //    if (game.IsRide)
                    //        game.DownRide();
                    //}
                }
                if (((Q12TK != 0 && TLBB.MapId == MAP.ToChau)))
                {
                    if (IsP)
                    {
                        if (!game.IsP)
                        {
                            game.IsP = true;
                            game.TraQ = game.NhanQ = game.IsClick = game.IsContinute = false;
                        }
                    }
                    else
                    {
                        game.IsP = false;
                    }
                    if (game.GetDistance(TOCHAU.TienHongVu.X, TOCHAU.TienHongVu.Y) > 10 && (Q12TK == 1 || Q12TK == 3))
                    {
                        game.IsP = false;
                    }
                    if (game.GetDistance(TOCHAU.HoaKiemAnh.X, TOCHAU.HoaKiemAnh.Y) > 10 && Q12TK == 2)
                    {
                        game.IsP = false;
                    }
                }
                if ((game.TLBB.MapId == MAP.ViemMaSon || game.TLBB.MapId == MAP.TacKhauDoanhDia || game.TLBB.MapId == game.TLBB.MapAcBa || game.TLBB.MapId == MAP.TangKinhCac || game.TLBB.MapId == MAP.TamTaiHiepCoc) && (game.TLBB.PlayerState != 0 && game.TLBB.MapId != MAP.TuTuyetTrang))
                {
                    //game.IsTrieuTap = false;
                    continue;
                }
                if (game.TLBB.MapId != mapId || TDT.GetDistance(game.RoundX, game.RoundY, x, y) >= 8 || (TDT.GetDistance(game.RoundX, game.RoundY, x, y) >= 3 && x == TOCHAU.TienHongVu.X && y == TOCHAU.TienHongVu.Y) || (TDT.GetDistance(game.RoundX, game.RoundY, CharX, CharY) >= 3 && (game.TLBB.MapId == MAP.TuTuyetTrang || game.TLBB.MapId == MAP.TangKinhCac || game.IsMapAcBa || game.TLBB.MapId == MAP.PhungHoangCoThanhPhuBan || game.TLBB.MapId == MAP.HuyenVuDaoPhuBan || game.TLBB.MapId == MAP.TuTuyetTrang || game.TLBB.MapId == MAP.ThanhThuSonPhuBan || game.TLBB.MapId == MAP.TacKhauDoanhDia || game.TLBB.MapId == MAP.ViemMaSon || game.TLBB.MapId == MAP.TamTaiHiepCoc)))
                {
                    if ((game.TLBB.MapId == MAP.TangKinhCac || game.TLBB.MapId == MAP.TacKhauDoanhDia) && game.TLBB.IsFollow)
                    {
                        //game.IsTrieuTap = false;
                    }
                    else
                    {
                        //isTrieuTap = game.IsTrieuTap = true;
                        if (game.TLBB.IsFollow && (TDT.GetDistance(game.CharX, game.CharY, CharX, CharY) >= 10 || game.TLBB.MapId != TLBB.MapId))
                            StopFollow();
                        if (game.TLBB.IsFollow)
                            game.GoToEx(x, y, (int)TLBB.MapId);
                        else
                            game.GoToEx(x, y, (int)TLBB.MapId, true);
                    }
                }
                else
                {
                    //game.IsTrieuTap = false;
                }
            }
            if (isTrieuTap)
            {
            }
            return isTrieuTap;
        }

        public bool TrieuTapEx(float x = -1, float y = -1, int mapId = -1)
        {
            if (x == -1)
                x = CharX;
            if (y == -1)
                y = CharY;
            if (mapId == -1)
                mapId = (int)TLBB.MapId;
            bool isTrieuTap = true;
            foreach (var game in Party.Where(o => o.NeBayTime.Elapsed.TotalSeconds > 3 && o.PickTime.Elapsed.TotalSeconds > 3))
            {
                if (game.TLBB.MapId == mapId && game.GetDistance(x, y) <= 3)
                {
                    continue;
                }
                game.GoToEx(x, y, mapId);
                isTrieuTap = false;
            }
            return isTrieuTap;
        }

        public bool TrieuTap()
        {
            if (TLBB.MapId == MAP.LoiDaiSinhTu)
            {
                if (Objects.HaveMonter("Lý Khôi"))
                    return true;
            }
            bool isdatrieutap = true;

            //if (!TLBB.IsLeader && TLBB.MapId == MAP.TacKhauDoanhDia)
            //{
            //    if (Leader != null)
            //    {
            //        if (Leader.TLBB.MapId != MAP.TacKhauDoanhDia)
            //        {
            //            if (TLBB.IsFollow)
            //                StopFollow();
            //        }
            //    }
            //}

            if (IsByLogin || IsSuDoo)
            {
                foreach (Game game in Party)
                {
                    if (game.TLBB.MapId != TLBB.MapId)
                    {
                        isdatrieutap = false;
                        game.GoToEx(CharX, CharY, (int)TLBB.MapId);
                    }
                    else if (TDT.GetDistance(game.CharX, game.CharY, CharX, CharY) > 3)
                    {
                        isdatrieutap = false;
                        game.GoToEx(CharX, CharY, (int)TLBB.MapId);
                    }
                    else
                    {
                    }
                }
                if (isdatrieutap)
                {
                    return isdatrieutap;
                }
                return false;
            }

            ///if(TLBB.IsLeader && TLBB.MapId !=MAP.TacKhauDoanhDia && isact)
            if (MapAcTac != 0)
            {
                if (TLBB.MapId == MapAcTac)
                {
                    foreach (var game in Party)
                    {
                        if (game.TLBB.MapId == MAP.TacKhauDoanhDia)
                        {
                            game.StopFollow();
                        }
                    }
                }
            }

            if (IsPhanDame)
                return false;
            bool isTrieuTap = false;
            foreach (Game game in Party)
            {
                if (TLBB.MapId == MAP.TuTuyetTrang && game.TLBB.MapId != MAP.TuTuyetTrang)
                {
                    game.GoToEx(RoundX, RoundY, MAP.TuTuyetTrang);
                    continue;
                }
                if (game.Objects.All.Where(o => o.IsTrap).Count() > 0)
                    continue;

                if (game.NeBayTime.Elapsed.TotalSeconds < 2)
                    continue;
                if (game.tranTime.Elapsed.TotalSeconds < 2)
                    continue;
                if (game.IsP)
                    continue;
                if (game.SWHoiSinh.Elapsed.TotalSeconds < 12 || game.tranTime.Elapsed.TotalSeconds < 2)
                    continue;
                if (!game.TLBB.IsLeader && game.Leader != null && game.Leader.IsThuyLao && game.TLBB.MapId == MAP.ThuyLao)
                {
                    if (game.Leader.TLBB.MapId == MAP.ToChau)
                    {
                        if (game.PhuIndex(MAP.ToChau) != -1)
                        {
                            if (game.TLBB.IsRide)
                                game.DownRide();
                            game.PlayerPackageUseItem(game.PhuIndex(MAP.ToChau));
                            continue;
                        }
                    }
                }
                if (IsNguyTongQuanDie && TLBB.MapId == MAP.BienGioiTongLieu)
                {
                    if (swCauQ1.Elapsed.TotalMinutes < 2)
                    {
                        if (game.TLBB.IsFollow)
                            game.StopFollow();
                        if (game.CauQ1 == 1)
                        {
                            if (!game.Move(79, 97))
                            {
                                isTrieuTap = true;
                            }
                            else
                            {
                                isTrieuTap = false;
                            }
                            continue;
                        }
                        else if (game.CauQ1 == 2)
                        {
                            if (!game.Move(42, 97))
                                isTrieuTap = true;
                            else
                            {
                                isTrieuTap = false;
                            }
                            continue;
                        }
                        continue;
                    }
                }
                if (game.TLBB.IsLeader)
                    continue;
                if (!IsP)
                {
                    game.IsP = false;
                }

                if (game.PickItem())
                    continue;
                if (game.NeBayTime.Elapsed.TotalSeconds < 15 && (game.TLBB.MapId == MAP.LoiDaiSinhTu || game.TLBB.MapId == MAP.PhungMinhVuongLang || game.TLBB.MapId == MAP.ThieuThatSon))
                {
                    continue;
                }
                if (game.TLBB.MapId == MAP.ThieuThatSon && game.swTieuVienSon.Elapsed.TotalSeconds < 15)
                {
                    continue;
                }
                if (game.TLBB.MapId == MAP.ThieuThatSon && game.IsPhanDame)
                {
                    continue;
                }
                if (TLBB.MapId != game.TLBB.MapId && game.TLBB.MapId == MAP.YenTuO)
                {
                    game.Move(64, 21);
                    continue;
                }
                if (game.TLBB.MapId == MAP.BienGioiTongLieu)
                {
                    if (IsDuDocDie || IsHongHungVuongDie)
                    {
                        if (game.PickItem())
                            continue;
                    }
                    if (game.CauQ1 == 0)
                    {
                        game.DoStringEx("count = DataPool:GetTeamMemberCount(); for i = 0,count - 1 do szNick ,iFamily ,iLevel ,bDead ,bDeadlink ,bSex ,sZoneWorldID = DataPool:GetTeamMemInfoByIndex(i); if (szNick == Player:GetName()) then return i .. ',' .. count; end end return '-1';");
                        game.LuaToString();
                        string party = game.LuaString();
                        if (party.Split(',').Length > 1)
                        {
                            int index = TDT.ParseInt(party);
                            int count = TDT.ParseInt(party.Replace(index + ",", ""));
                            if (count / 2 > index)
                            {
                                game.CauQ1 = 1;
                                game.PushDebugMessage("Nhân vật này sẽ chạy cầu " + game.CauQ1);
                            }
                            else
                            {
                                game.CauQ1 = 2;
                                game.PushDebugMessage("Nhân vật này sẽ chạy cầu " + game.CauQ1);
                            }
                        }
                    }
                }
                if (game.TLBB.IsFollow && game.TLBB.PlayerState == 2)
                    continue;

                if (IsThuyLao)
                {
                    if (game.TLBB.MapId != MAP.ThuyLao || TLBB.MapId != MAP.ThuyLao)
                    {
                        if (!game.DaNhanThuyLao)
                        {
                            game.NhanThuyLao();
                        }
                        else
                        {
                            game.DiThuyLao();
                        }
                        continue;
                    }
                    else
                    {
                        if (game.PickItem())
                            continue;
                        if (game.Objects.NearMonter(20).Count > 0)
                        {
                            if (game.TLBB.PlayerState == 0)
                            {
                                if (game.TLBB.IsFollow)
                                    game.StopFollow();
                                if (game.TLBB.IsRide)
                                    game.DownRide();
                            }
                        }
                        else
                        {
                            if (!game.TLBB.IsRide && game.TLBB.HaveRide)
                            {
                                game.UpRide();
                            }
                        }
                        if (TDT.GetDistance(game.RoundX, game.RoundY, RoundX, RoundY) > 4)
                        {
                            game.GoToEx(RoundX, RoundY);
                        }
                        continue;
                    }
                }
                //if (game.TLBB.KeyId != TLBB.Id)
                //{
                //    game.IsTrieuTap = false;
                //    continue;
                //}
                if ((game.TLBB.MapId == MAP.ViemMaSon || game.TLBB.MapId == MAP.TamTaiHiepCoc) && game.TLBB.IsFollow)
                    continue;
                if (game.IsMapPhuBan())
                {
                    if (IsXong50NguyTongBinh)
                        game.IsXong50NguyTongBinh = true;
                    else
                        game.IsXong50NguyTongBinh = false;
                    if ((game.Objects.NearMonter12m.Count > 0 || (((game.TLBB.MapId == MAP.BienGioiTongLieu && game.Objects.Monters.Count > 0) || game.IsMapAcBa || game.TLBB.MapId == MAP.TangKinhCac) && game.Objects.NearMonter(20).Count > 0)) && (game.TLBB.PlayerState == 0 || game.TLBB.MapId == MAP.TuTuyetTrang))
                    {
                        //if (game.TLBB.MapId != MAP.ViemMaSon && game.TLBB.MapId != MAP.TamTaiHiepCoc && game.TLBB.PlayerState == 0)
                        //    game.StopFollow();
                        if (game.TLBB.IsRide && game.IsAuto)
                        {
                            game.DownRide();
                        }
                    }
                    //else if(game.TLBB.MapId == MAP.TangKinhCac && game.TLBB.PlayerState == 0 && game.Objects.Near20m.Count > 0)
                    //{
                    //    if (game.TLBB.IsFollow)
                    //        game.StopFollow();
                    //    if (game.IsRide)
                    //        game.DownRide();
                    //}
                }
                if ((game.TLBB.MapId == MAP.ViemMaSon || game.TLBB.MapId == MAP.TacKhauDoanhDia || game.TLBB.MapId == game.TLBB.MapAcBa || game.TLBB.MapId == MAP.TangKinhCac || game.TLBB.MapId == MAP.TamTaiHiepCoc) && (game.TLBB.PlayerState != 0 && game.TLBB.MapId != MAP.TuTuyetTrang))
                {
                    continue;
                }
                if (game.TLBB.MapId != TLBB.MapId || TDT.GetDistance(game.CharX, game.CharY, CharX, CharY) >= 8 || (TDT.GetDistance(game.CharX, game.CharY, CharX, CharY) >= 3 && (game.TLBB.MapId == MAP.LoiDaiSinhTu || game.TLBB.MapId == MAP.TuTuyetTrang || game.TLBB.MapId == MAP.TangKinhCac || game.IsMapAcBa || game.TLBB.MapId == MAP.PhungHoangCoThanhPhuBan || game.TLBB.MapId == MAP.HuyenVuDaoPhuBan || game.TLBB.MapId == MAP.TuTuyetTrang || game.TLBB.MapId == MAP.ThanhThuSonPhuBan || game.TLBB.MapId == MAP.TacKhauDoanhDia || game.TLBB.MapId == MAP.ViemMaSon || game.TLBB.MapId == MAP.TamTaiHiepCoc)))
                {
                    if (game.IsMapPhuBan() && game.TLBB.IsFollow)
                    {
                    }
                    else
                    {
                        isTrieuTap = true;
                        //if (game.TLBB.IsFollow && (TDT.GetDistance(game.CharX, game.CharY, CharX, CharY) >= 10 || game.TLBB.MapId != TLBB.MapId))
                        //    StopFollow();
                        if (game.TLBB.IsFollow)
                            game.GoToEx(RoundX, RoundY, (int)TLBB.MapId);
                        else
                            game.GoToEx(RoundX, RoundY, (int)TLBB.MapId, true);
                    }
                }
            }
            if (isTrieuTap)
            {
            }
            return isTrieuTap;
        }

        public int acba = -1;

        public int AcBa
        {
            get
            {
                if (acba == -1)
                {
                    acba = LastAcBa;
                }
                return acba;
            }
            set
            {
                acba = value;
            }
        }

        public static int LastAcBa { get; set; } = -1;

        public void SetAcBa()
        {
            foreach (Game game in Party)
            {
                game.AcBa = AcBa;
            }
        }

        public static bool IsHoldPK
        {
            get;
            set;
        }




        public void UnHookRecv()
        {
            if (IsHooked)
            {
                if (RecvAddress != 0)
                {
                    Memory.WriteProcessMemory(Memory.Id, RecvAddress, bufferRecv, 10, 0);
                }
                IsHooked = false;
            }
        }



        public bool IsTKC
        {
            get
            {
                foreach (Game game in Party)
                {
                    if (game.Missions.Contains(MissionsType.DatDoiTangKinhCac))
                        return true;
                }
                return false;
            }
        }

        public void SetSafeTime()
        {
            DoStringEx("Lua_SetProtectTime(0,1);");
            Thread.Sleep(1000);
            LUA.MessageBox_Self_OK_Clicked();
        }

        public bool IsP { get; set; }

        public bool TraQ = false;
        public bool NhanQ = false;
        public bool IsContinute = false;
        public bool IsClick = false;

        public void Phuong()
        {
            if (!IsP)
                return;
            if (!IsOneSec)
                return;
            if (TLBB.IsMapBang(TLBB.MapId) && GetDistance(107, 61) <= 6)
            {
                TalkEx("vanlinhtranh");
                Thread.Sleep(1000);
                QuestFrame.Click("#{LDDQ_20200819_75}");
                IsP = false; return;
            }
            if (TLBB.MapId == MAP.MauDonUyen && GetDistance(37, 31) < 5)
            {
                Talk("dongthe");
                Thread.Sleep(1000);
                QuestFrame.Click("Đưa ta về Tô Châu");
                IsP = false;
                return;
            }
            if (TLBB.MapId == MAP.TuTuyetTrang && Objects.Monters.Count > 0)
            {
                IsP = false;
                return;
            }
            if (IsP)
            {
                if (TLBB.IsFollow)
                    StopFollow();

                if (TLBB.MapId == MAP.LauLan && GetDistance(LAULAN.PhuSinh.X, LAULAN.PhuSinh.Y) < 5)
                {
                    Talk(LAULAN.PhuSinh);
                    Thread.Sleep(1000);
                    QuestFrame.Click("#{TLHJ_120110_62}");
                    Thread.Sleep(1000);
                    if (TLBB.IsQuestOpen)
                    {
                        DoStringEx(@"setmetatable(_G, {__index = Quest_Env});
                                    if  this:IsVisible() and  Quest_Button_Continue:GetText() == 'Xong' then
	                                    QuestFrameMissionComplete(1);
                                    end");
                    }
                    QuestFrame.Accept();
                    IsP = false;
                    return;
                }

                if (TLBB.MapId == MAP.BinhThanhKyTran && TDT.GetDistance(CharX, CharY, 142, 178) < 8)
                {
                    if (TLBB.IsRide)
                        DownRide();
                    DoAction("PetSkill2_2");
                    foreach (GameObject _object in Objects.All)
                    {
                        if (TDT.VietLien(_object.Name) == "trandungnghia")
                        {
                            Talk(_object.Id);
                            break;
                        }
                    }
                    Thread.Sleep(1000);
                    QuestFrame.Click("#{BSQZ_101223_49}");
                    IsP = false;
                    return;
                }
                if (TDT.VietLien(TLBB.MapName).Contains("thienkieplau"))
                {
                    if (TLBB.IsQuestOpen)
                    {
                        if (TLBB.IsQuestOpen)
                        {
                            if (!IsClick)
                            {
                                foreach (QuestFrame quest in QuestFrame.Enum(this))
                                {
                                    if (quest.Name.Contains("#{TJL_xml_XX(01)}"))
                                    {
                                        QuestFrameOptionClicked(quest);
                                        IsClick = true;
                                        return;
                                    }
                                }
                                CloseQuest();
                                return;
                            }
                            if (!TraQ && IsClick)
                            {
                                LUA.QuestFrameMissionComplete();
                                CloseQuest();
                                TraQ = true;
                                IsClick = false;
                                return;
                            }
                            if (!NhanQ && IsClick)
                            {
                                QuestFrameAccept();
                                CloseQuest();
                                NhanQ = true;
                                IsP = false;
                                IsClick = false;
                                return;
                            }
                        }
                    }
                    foreach (GameObject _object in Objects.All)
                    {
                        if (TDT.VietLien(_object.Name) == "phokiepsinh")
                        {
                            Talk(_object.Id);
                            break;
                        }
                    }
                    return;
                }
                // sattinh
                if (TLBB.MapId == DAILY.Id && TDT.GetDistance(CharX, CharY, 131, 79) < 8)
                {
                    IsP = false;
                    if (TrueStandTime.Elapsed.TotalSeconds >= 1)
                    {
                        Talk(DAILY.KhoVinhDaiSu);
                        Thread.Sleep(1000);
                        QuestFrame.Click("#{SXRW_090119_002}");
                        Thread.Sleep(1000);
                        if (QuestFrame.Click("#{SXRW_090119_015}"))
                        {
                            Thread.Sleep(3000);
                            if (QuestFrame.Text.Contains("#{SXRW_090119_016}"))
                            {
                                LUA.DeleteMission("#{SXRW_090119_002}");
                            }
                            return;
                        }
                        if (QuestFrame.Text.Contains("#{SXRW_090119_175}"))
                        {
                            Party.ForEach(g => g.RemoveMission(MissionsType.DatDoiSatTinh));
                            if (Main.IsBanRac)
                                Party.ForEach(game => game.PushMissions(new[] { MissionsType.TriLieu, MissionsType.BanRac, MissionsType.CatVang, MissionsType.PhanGiaiTrangBiPet }));
                        }
                        if (TLBB.IsQuestOpen)
                        {
                            DoStringEx(@"setmetatable(_G, {__index = Quest_Env});
                                    if  this:IsVisible() and  Quest_Button_Continue:GetText() == 'Xong' then
	                                    QuestFrameMissionComplete(1);
                                    end");
                        }
                        QuestFrame.Accept();
                    }

                    return;
                }
                if (TLBB.MapId == 61 && TDT.GetDistance(CharX, CharY, 40, 40) < 8) // ky cuoc
                {
                    if (TLBB.IsQuestOpen)
                    {
                        QuestFrameOptionClicked(44000, 0);
                        IsP = false;
                    }
                    foreach (GameObject _object in Objects.All)
                    {
                        if (TDT.VietLien(_object.Name) == "tethanh")
                        {
                            Talk(_object.Id);
                            break;
                        }
                    }
                    return;
                }
                if (TLBB.MapId == MAP.LauLan && TDT.GetDistance(CharX, CharY, 211, 176) < 8)
                {
                    IsP = false;
                    if (TrueStandTime.Elapsed.TotalSeconds >= 1)
                    {
                        foreach (GameObject _object in Objects.All)
                        {
                            if (TDT.VietLien(_object.Name) == "caoduong")
                            {
                                Talk(_object.Id);
                                Thread.Sleep(1000);
                                break;
                            }
                        }
                        QuestFrame.Click("#{BSQZ_101223_02}");
                        Thread.Sleep(1000);
                        LUA.QuestFrameMissionContinue();
                        Thread.Sleep(1000);
                        LUA.QuestFrameMissionComplete();
                        Thread.Sleep(1000);
                        foreach (GameObject _object in Objects.All)
                        {
                            if (TDT.VietLien(_object.Name) == "caoduong")
                            {
                                Talk(_object.Id);
                                Thread.Sleep(1000);
                                break;
                            }
                        }
                        QuestFrame.Click("#{BSQZ_101223_02}");
                        Thread.Sleep(1000);
                        QuestFrameAccept();
                        Thread.Sleep(1000);
                        if (QuestFrame.Text.Contains("#{BSQZ_101223_12}"))
                        {
                            QuanDoans.ForEach(g => g.RemoveMission(MissionsType.DatDoiBinhThanh));
                            return;
                        }

                        Thread.Sleep(1000);
                        if (Missions.Contains(MissionsType.DatDoiBinhThanh))
                        {
                            foreach (GameObject _object in Objects.All)
                            {
                                if (TDT.VietLien(_object.Name) == "caoduong")
                                {
                                    Talk(_object.Id);
                                    Thread.Sleep(1000);
                                    break;
                                }
                            }
                            QuestFrame.Click("#{BSQZ_101223_03}");
                            Thread.Sleep(1000);
                            if (QuanDoans.Exists(g => g.Missions.Contains(MissionsType.BinhThanhKho)))
                                QuestFrame.Click("#{BSQZ_101223_07}");
                            else
                            {
                                QuestFrame.Click("#{BSQZ_101223_06}");
                            }
                        }
                    }
                    return;
                }
                // qlaulan
                if (TLBB.MapId == MAP.LauLan && TDT.GetDistance(CharX, CharY, 295, 68) < 8)
                {
                    IsP = false;
                    if (TrueStandTime.Elapsed.TotalSeconds >= 1)
                    {
                        Talk(LAULAN.HaDuyet);
                        Thread.Sleep(1000);
                        QuestFrame.Click("#{XSHYH_150211_2}");
                        Thread.Sleep(1000);
                        if (QuestFrame.Text.Contains("#{XSHYH_150211_13}") || QuestFrame.Text.Contains("#{LSCS_180827_3}"))
                        {
                            Party.ForEach(game => game.RemoveMission(MissionsType.DatDoiQ123LauLan));
                            if (Main.IsBanRac)
                                Party.ForEach(game => game.PushMissions(new[] { MissionsType.TriLieu, MissionsType.BanRac, MissionsType.CatVang }));
                        }

                        if (QuestFrame.Click("#{XSHYH_150211_3}"))
                        {
                            Thread.Sleep(1000);
                            if (QuestFrame.Text.Contains("#{XSHYH_150211_92}") || QuestFrame.Text.Contains("#{XSHYH_150211_97}"))
                            {
                                PushDebugMessage("Hủy Q");
                                LUA.DeleteMission("#{XSHYH_150211_2}");
                                return;
                            }
                            Thread.Sleep(2000);
                            return;
                        }
                        if (TLBB.IsQuestOpen)
                        {
                            DoStringEx(@"setmetatable(_G, {__index = Quest_Env});
                                    if this:IsVisible() and Quest_Button_Continue:GetText() == 'Xong' then
	                                    QuestFrameMissionComplete(1);
                                    end");
                        }
                        LUA.QuestFrameAcceptClicked();
                        Thread.Sleep(1000);
                        return;
                    }
                }
                // qtochau
                if (TLBB.MapId == TOCHAU.Id && TDT.GetDistance(CharX, CharY, 134, 260) < 8)
                {
                    IsP = false;
                    if (TrueStandTime.Elapsed.TotalSeconds >= 1)
                    {
                        Talk(TOCHAU.TienHoanhVu);
                        Thread.Sleep(1000);
                        QuestFrame.Click("Trừ Phỉ Phá Tam Quan");
                        Thread.Sleep(1000);
                        Talk(TOCHAU.TienHoanhVu);
                        Thread.Sleep(1000);
                        if (QuestFrame.Click("#{LSHYH_150210_78}"))
                        {
                            Thread.Sleep(3000);
                            return;
                        }
                        QuestFrame.Click("Trừ Phỉ Phá Tam Quan");
                        Thread.Sleep(1000);
                        LUA.QuestFrameAcceptClicked();
                        Thread.Sleep(1000);
                        if (QuestFrame.Text.Contains("#{XSCS_180827_1}"))
                        {
                            RemoveMission(MissionsType.DatDoiQ123ToChau);
                            if (Main.IsBanRac)
                                Party.ForEach(game => game.PushMissions(new[] { MissionsType.TriLieu, MissionsType.BanRac, MissionsType.CatVang }));
                        }
                        return;
                    }
                }
                if (TLBB.MapId == TOCHAU.Id && TDT.GetDistance(CharX, CharY, TOCHAU.HoaKiemAnh.X, TOCHAU.HoaKiemAnh.Y) < 8)
                {
                    if (TLBB.IsQuestOpen)
                    {
                        if (QuestFrame.All(this).Contains("#{LSHYH_150210_6}"))
                        {
                            RemoveMission(MissionsType.DatDoiQ123ToChau);
                        }
                        if (TLBB.IsLeader)
                            QuestFrame.Click("thangtienrungtruc");
                        if (TLBB.IsLeader && Q12TK == 2 && NhanQ)
                        {
                            foreach (QuestFrame quest in QuestFrame.Enum(this))
                            {
                                if (quest.StrOptionExtra1 == 50101 && quest.StrOptionExtra2 == 2)
                                {
                                    QuestFrameOptionClicked(quest);
                                    return;
                                }
                            }
                            IsClick = TraQ = NhanQ = IsContinute = false;
                        }
                        if (!IsClick)
                        {
                            foreach (QuestFrame quest in QuestFrame.Enum(this))
                            {
                                if (quest.StrOptionExtra1 == 50101 && quest.StrOptionExtra2 == 1)
                                {
                                    QuestFrameOptionClicked(quest);
                                    IsClick = true;
                                    return;
                                }
                            }
                            CloseQuest();
                            return;
                        }
                        if (!TraQ && IsClick)
                        {
                            LUA.QuestFrameMissionComplete();
                            CloseQuest();
                            TraQ = true;
                            IsClick = false;
                            return;
                        }
                        if (!NhanQ && IsClick)
                        {
                            QuestFrameAccept();
                            CloseQuest();
                            NhanQ = true;
                            IsP = false;
                            IsClick = false;
                            if (TLBB.IsLeader && (Missions.Contains(MissionsType.DatDoiQ123ToChau) || Q12TK > 0))
                            {
                                IsP = true;
                            }
                            return;
                        }
                        CloseQuest();
                    }
                    foreach (GameObject _object in Objects.All)
                    {
                        if (TDT.VietLien(_object.Name) == "hoakiemanh")
                        {
                            Talk(_object.Id);
                            break;
                        }
                    }
                    return;
                }
                // tutuyettrang
                if (TLBB.MapId == TOCHAU.Id && TDT.GetDistance(CharX, CharY, 195, 214) < 8)
                {
                    IsP = false;
                    if (TrueStandTime.Elapsed.TotalSeconds >= 1)
                    {
                        Talk(TOCHAU.PhanThanhThanh);
                        Thread.Sleep(1000);

                        QuestFrame.Click("#{SJZ_100129_03}");
                        Thread.Sleep(1000);
                        if (TLBB.IsQuestOpen)
                        {
                            DoStringEx(@"setmetatable(_G, {__index = Quest_Env});
                                    if  this:IsVisible() and  Quest_Button_Continue:GetText() == 'Xong' then
	                                    QuestFrameMissionComplete(1);
                                    end");
                        }
                        LUA.QuestFrameAcceptClicked();
                        Thread.Sleep(1000);
                        if (QuestFrame.Text.Contains("#{SJZ_100129_08}"))
                        {
                            Party.ForEach(g => g.RemoveMission(MissionsType.DatDoiTuTuyetTrang));
                            if (Main.IsBanRac)
                                Party.ForEach(game => game.PushMissions(new[] { MissionsType.TriLieu, MissionsType.BanRac, MissionsType.CatVang }));
                            return;
                        }

                        if (QuestFrame.Click("#{SJZ_100129_11}"))
                        {
                            Thread.Sleep(3000);
                            if (QuestFrame.Text.Contains("#{SJZ_100129_121}"))
                            {
                                LUA.DeleteMission("#{SJZ_100129_03}");
                            }
                        }
                    }
                    return;
                }
                if (TLBB.MapId == MAP.TuTuyetTrang && (TDT.GetDistance(CharX, CharY, 96, 79) < 3.2 || TDT.GetDistance(CharX, CharY, 35, 87) < 3.2 || TDT.GetDistance(CharX, CharY, 84, 23) < 3.2 || TDT.GetDistance(CharX, CharY, 23, 17) < 3.2))
                {
                    IsP = false;
                    foreach (GameObject _object in Objects.All.OrderBy(o => o.Distance))
                    {
                        if (TDT.VietLien(_object.Name) == "phanthanhthanh")
                        {
                            Talk(_object.Id);
                            Thread.Sleep(1000);
                            break;
                        }
                    }
                    if (TLBB.IsQuestOpen)
                    {
                        if (QuestFrame.Click("#{SJZYH_150824_5}"))
                        {
                            Thread.Sleep(1000);
                            return;
                        }
                        if (QuestFrame.Click("#{SJZYH_150824_6}"))
                        {
                            Thread.Sleep(1000);
                            return;
                        }
                        if (TDT.GetDistance(CharX, CharY, 23, 17) < 8)
                        {
                            QuestFrameOptionClicked(402051, 25);
                        }
                        else if (TDT.GetDistance(CharX, CharY, 84, 23) < 8)
                        {
                            QuestFrameOptionClicked(402051, 24);
                        }
                        else
                        {
                            QuestFrameOptionClicked(402051, 22);
                            Thread.Sleep(1000);
                            QuestFrameOptionClicked(402051, 23);
                        }
                        Thread.Sleep(2000);
                    }

                    return;
                }
            }
            foreach (GameObject g in Objects.All)
            {
                if (g.Title.Contains("#{PTFB"))
                {
                    Talk(g.Id);
                    Thread.Sleep(1000);
                    QuestFrame.Click("#{PTFB_191228_218}");
                    QuestFrame.Click("#{PTFB_191228_73}");

                    Thread.Sleep(1000);
                    QuestFrame.Click("#{PTFB_191228_477}");
                }
            }
            IsP = false;
        }

    

        public Stopwatch tranTime = Stopwatch.StartNew();

        public Stopwatch TrimTime = Stopwatch.StartNew();

        public Thread ThreadAuto;

        public Stopwatch SwBuffNM { get; set; } = Stopwatch.StartNew();

        public int PartyMinSwBuffNM
        {
            get
            {
                int min = 999;
                foreach (Game game in Party)
                {
                    if (game.SwBuffNM.Elapsed.TotalSeconds < min)
                    {
                        min = (int)game.SwBuffNM.Elapsed.TotalSeconds;
                    }
                }
                return min;
            }
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

        public void RandomMapAcTac()
        {
            int ran = TDT.Random.Next(1, 6);
            //if (ran == 0)
            //    MapAcTac = MAP.KinhHo;
            if (ran == 1)
                MapAcTac = MAP.ThaiHo;
            else if (ran == 2)
                MapAcTac = MAP.TungSon;
            else if (ran == 3)
                MapAcTac = MAP.KiemCac;
            else if (ran == 4)
                MapAcTac = MAP.DonHoang;
            else if (ran == 5)
                MapAcTac = MAP.VoLuongSon;
            else
                MapAcTac = MAP.KiemCac;
            PushMissions(MissionsType.DatDoiAcTac);
        }

        public void DeleteFail()
        {
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                if (!TLBB.IsTogleMission)
                {
                    PostMessage(18, 105);
                    Thread.Sleep(1000);
                }
                PostMessage(18, 105);
                Thread.Sleep(1000);
                foreach (GameTask task in GameTask.Enum(this))
                {
                    if (task.IsFail)
                    {
                        LUA.DeleteMission(task.Name);
                    }
                }
            }).Start();
        }

        public void RandomMapTangKinhCac()
        {
            int ran = TDT.Random.Next(0, 3);
            if (ran == 0)
                MapTangKinhCac = MAP.TayHo;
            else if (ran == 1)
                MapTangKinhCac = MAP.NhiHai;
            else if (ran == 2)
                MapTangKinhCac = MAP.NhanNam;
            else
                MapTangKinhCac = MAP.NhanNam;
            PushMissions(MissionsType.DatDoiTangKinhCac);
        }

        private bool IsXongTamBao { get; set; }

        private bool IsXongKyCuoc { get; set; }

        private static Stopwatch resettimeall { get; set; } = Stopwatch.StartNew();



        private Stopwatch NeBayTime = Stopwatch.StartNew();

        public bool IsX2 { get; set; }
        public Stopwatch swStandTime = Stopwatch.StartNew();

        private void NhanCon()
        {
            if (!IsOneSec)
            {
                return;
            }
            if (QuestFrame.Text.Contains("#{SYZN_130820_448}") || QuestFrame.Text.Contains("#{SYZN_130820_513}") || QuestFrame.Text.Contains("#{SYZN_130820_383}"))
            {
                RemoveMission(MissionsType.NhanCon);
                PushDebugMessage("Xong rồi mai làm tiếp");
                return;
            }

            if (TLBB.PlayerState != 0)
                return;
            if (MissionState == "")
            {
                if (!TLBB.IsTogleMission)
                {
                    TogleMission();
                    MissionState = "OpenMission";
                }
                else
                {
                    MissionState = "OpenMission";
                }
                return;
            }
            if (MissionState == "OpenMission")
            {
                CloseMission();
                MissionState = "CloseMission";
                return;
            }
            if (MissionState == "CloseMission")
            {
                foreach (GameTask task in GameTask.Enum(this))
                {
                    if (task.Name.Contains("#{SYZN_130820_519}"))
                    {
                        if (task.Completed)
                        {
                            MissionState = "Completed";
                            return;
                        }
                        else
                        {
                            if (task.Count1 != 1)
                            {
                                MissionState = "[Lạc Dương-Vương Tích Tân][366,228-0]";
                                return;
                            }
                            else if (task.Count2 != 1)
                            {
                                MissionState = "[Lạc Dương-Đậu Ngô Đồng][353,274-0]";
                                return;
                            }
                            else if (task.Count3 != 1)
                            {
                                MissionState = "[Tô Châu-Liễu Nguyệt Hồng][230,350-1]";
                                return;
                            }
                            else if (task.Count4 != 1)
                            {
                                MissionState = "[Tô Châu-Âu Dã Vu][355,240-1]";
                                return;
                            }
                            else if (task.Count5 != 1)
                            {
                                MissionState = "[Đại Lý-Tảo Địa Thần Tăng][242,31-2]";
                                return;
                            }
                        }
                    }
                    if (task.Name.Contains("#{SYZN_130820_456}"))
                    {
                        if (task.Completed)
                        {
                            MissionState = "Completed";
                            return;
                        }
                        else
                        {
                            if (task.Count1 != 1)
                            {
                                MissionState = "2GoLacDuong";
                                return;
                            }
                            else if (task.Count2 != 1)
                            {
                                MissionState = "2GoDaiLy";
                                return;
                            }
                            else if (task.Count3 != 1)
                            {
                                MissionState = "2GoToChau";
                                return;
                            }
                            else if (task.Count4 != 1)
                            {
                                MissionState = "2GoThaiHo";
                                return;
                            }
                            else if (task.Count5 != 1)
                            {
                                MissionState = "2GoTayHo";
                                return;
                            }
                        }
                    }
                    if (task.Name.Contains("#{SYZN_130820_403}"))
                    {
                        if (task.Completed)
                        {
                            MissionState = "Completed";
                            return;
                        }
                        else
                        {
                            if (task.Count1 != 1)
                            {
                                MissionState = "GoLacDuong";
                                return;
                            }
                            else if (task.Count2 != 1)
                            {
                                MissionState = "GoToChau";
                                return;
                            }
                            else if (task.Count3 != 1)
                            {
                                MissionState = "GoTayHo";
                                return;
                            }
                            else if (task.Count4 != 1)
                            {
                                MissionState = "GoDaiLy";
                                return;
                            }
                            else if (task.Count5 != 1)
                            {
                                MissionState = "GoThaiHo";
                                return;
                            }
                        }
                    }
                }
                MissionState = "RecvMission";
                return;
            }
            if (MissionState.StartsWith("["))
            {
                if (RunTo(MissionState))
                {
                    string npcname = GetNPCName(MissionState);
                    TalkEx(npcname);
                    Thread.Sleep(1000);
                    QuestFrame.Click("#{SYZN_130820_533}");
                    MissionState = "";
                }
                return;
            }
            if (MissionState.StartsWith("2"))
            {
                string point = "";
                if (MissionState.Contains("LacDuong"))
                    point = "267,421," + MAP.LacDuong;
                if (MissionState.Contains("DaiLy"))
                    point = "176,76," + MAP.DaiLy;
                if (MissionState.Contains("ToChau"))
                    point = "202,328," + MAP.ToChau;
                if (MissionState.Contains("ThaiHo"))
                    point = "165,179," + MAP.ThaiHo;
                if (MissionState.Contains("TayHo"))
                    point = "68,113," + MAP.TayHo;
                if (GoToEx(point))
                {
                    DownRide();
                    foreach (GameObject obj in Objects.All)
                    {
                        if (obj.CleanName == "hoamai" || obj.CleanName == "hoahong" || obj.CleanName == "hoasung" || obj.CleanName == "hoanhai" || obj.CleanName == "hoalan")
                        {
                            SelectTarget((int)obj.Id);
                            break;
                        }
                    }
                    foreach (var item in PacketItems.All)
                    {
                        if (item.ClearName == "logom")
                        {
                            item.DoSubAction();
                            MissionState = "";
                            break;
                        }
                    }
                }
                return;
            }
            if (MissionState == "Completed")
            {
                if (GoToEx(LACDUONG.TruongDaiThuc))
                {
                    Talk(LACDUONG.TruongDaiThuc.Id);
                    Thread.Sleep(1000);
                    QuestFrameOptionClicked(890202, 2);
                    QuestFrame.Click("#{SYZN_130820_403}");
                    QuestFrame.Click("#{SYZN_130820_456}");
                    QuestFrame.Click("#{SYZN_130820_519}");

                    MissionState = "CompletedMission";
                }
                return;
            }
            if (MissionState == "CompletedMission")
            {
                LUA.QuestFrameMissionComplete();
                MissionState = "";
                return;
            }
            if (MissionState == "RecvMission")
            {
                if (GoToEx(LACDUONG.TruongDaiThuc))
                {
                    if (Objects.Self != null)
                    {
                        if (Objects.Self.Buff.Contains(2720))
                        {
                            Talk(LACDUONG.TruongDaiThuc);
                            Thread.Sleep(1000);
                            QuestFrame.Click("#{SYZN_130820_25}");
                            Thread.Sleep(1000);
                            QuestFrame.Click("#{SYZN_130820_566}");
                            MissionState = "";
                            return;
                        }
                    }
                    Talk(LACDUONG.TruongDaiThuc);
                    Thread.Sleep(1000);
                    if (QuestFrame.Click("#{SYZN_130820_456}"))
                    {
                        Thread.Sleep(1000);
                        QuestFrameAccept();
                        Thread.Sleep(1000);
                        MissionState = "";
                        return;
                    }
                    if (QuestFrame.Click("#{SYZN_130820_519}"))
                    {
                        Thread.Sleep(1000);
                        QuestFrameAccept();
                        Thread.Sleep(1000);
                        MissionState = "";
                        return;
                    }
                    Talk(LACDUONG.TruongDaiThuc);
                    Thread.Sleep(1000);
                    QuestFrame.Click("#{SYZN_130820_21}");
                    Thread.Sleep(1000);
                    QuestFrame.Click("#{SYZN_130820_13}");
                    Thread.Sleep(1000);
                    if (QuestFrame.Text.Contains("#{SYZN_130820_383}"))
                    {
                        RemoveMission(MissionsType.NhanCon);
                        PushDebugMessage("Đã có Con. Dừng Auto");
                        return;
                    }
                    if (QuestFrame.Text.Contains("#{SYZN_130820_387}"))
                    {
                        Talk(LACDUONG.TruongDaiThuc);
                        Thread.Sleep(1000);
                        QuestFrame.Click("#{SYZN_130820_26}");
                        Thread.Sleep(1000);
                        QuestFrame.Click(1);
                        Thread.Sleep(1000);
                    }
                    Talk(LACDUONG.TruongDaiThuc);
                    Thread.Sleep(1000);
                    QuestFrame.Click(890202, 2);

                    MissionState = "AcceptMission";
                }
                return;
            }
            if (MissionState == "AcceptMission")
            {
                QuestFrameAccept();
                QuestFrame.Close();
                MissionState = "";
                return;
            }

            if (MissionState == "GoLacDuong")
            {
                if (GoToEx(254, 118, LACDUONG.Id))
                {
                    if (TLBB.IsRide)
                    {
                        DownRide();
                    }
                    else
                    {
                        foreach (var item in PacketItems.All)
                        {
                            if (item.Type.Contains("CircularTaskTool50_5"))
                            {
                                item.DoSubAction();
                            }
                        }
                        MissionState = "";
                    }
                }
                return;
            }
            if (MissionState == "GoToChau")
            {
                if (GoToEx(244, 433, MAP.ToChau))
                {
                    if (TLBB.IsRide)
                    {
                        DownRide();
                    }
                    else
                    {
                        foreach (var item in PacketItems.All)
                        {
                            if (item.Type.Contains("CircularTaskTool50_5"))
                            {
                                item.DoSubAction();
                            }
                        }
                    }
                }
            }
            if (MissionState == "GoTayHo")
            {
                if (GoToEx(246, 193, MAP.TayHo))
                {
                    if (TLBB.IsRide)
                    {
                        DownRide();
                    }
                    else
                    {
                        foreach (var item in PacketItems.All)
                        {
                            if (item.Type.Contains("CircularTaskTool50_5"))
                            {
                                item.DoSubAction();
                            }
                        }
                        MissionState = "";
                    }
                }
                return;
            }
            if (MissionState == "GoDaiLy")
            {
                if (GoToEx(33, 108, MAP.DaiLy))
                {
                    if (TLBB.IsRide)
                    {
                        DownRide();
                    }
                    else
                    {
                        foreach (var item in PacketItems.All)
                        {
                            if (item.Type.Contains("CircularTaskTool50_5"))
                            {
                                item.DoSubAction();
                            }
                        }
                        MissionState = "";
                    }
                }
                return;
            }
            if (MissionState == "GoThaiHo")
            {
                if (GoToEx(154, 191, MAP.ThaiHo))
                {
                    if (TLBB.IsRide)
                    {
                        DownRide();
                    }
                    else
                    {
                        foreach (var item in PacketItems.All)
                        {
                            if (item.Type.Contains("CircularTaskTool50_5"))
                            {
                                item.DoSubAction();
                            }
                        }
                        MissionState = "";
                    }
                }
                return;
            }
            MissionState = "";
        }


        public void GiaoNguHanhThiep()
        {
            if (GoToEx(TOCHAU.DocCoHoi))
            {
                //Talk(TOCHAU.DocCoHoi);
                PacketItems.ThienCo.Where(o => o.Name == "Ngũ Hành Pháp Thiệp").ToList().ForEach(o => GetItemThienCo(o.Index));
                Talk(TOCHAU.DocCoHoi);
                Thread.Sleep(1000);
                if (QuestFrame.Click("#{XKSC_200325_01}"))
                {
                    Thread.Sleep(1000);
                    for (int i = 0; i < 50; i++)
                    {
                        QuestFrame.Click("#{XKSC_200325_03}");
                    }
                    RemoveMission(MissionsType.GiaoNguHanhPhapThiep);
                }

                //Thread.Sleep(1000);
                //QuestFrame.Click("#{XKSC_200325_01}");
                //Thread.Sleep(1000);
                //QuestFrame.Click("#{XKSC_200325_07}");
                //Thread.Sleep(1000);

                //Talk(TOCHAU.DocCoHoi);
                //Thread.Sleep(1000);
                //QuestFrame.Click("#{XKSC_200325_01}");
                //Thread.Sleep(1000);
                //QuestFrame.Click("#{XKSC_200325_06}");
                //Thread.Sleep(1000);

                //Talk(TOCHAU.DocCoHoi);
                //Thread.Sleep(1000);
                //QuestFrame.Click("#{XKSC_200325_01}");
                //Thread.Sleep(1000);
                //QuestFrame.Click("#{XKSC_200325_06}");
                //Thread.Sleep(1000);

                //Talk(TOCHAU.DocCoHoi);
                //Thread.Sleep(1000);
                //QuestFrame.Click("#{XKSC_200325_01}");
                //Thread.Sleep(1000);
                //QuestFrame.Click("#{XKSC_200325_05}");

            }
        }

        private void NhanPhiThuy()
        {
            if (!TLBB.IsMapBang(TLBB.MapId))
            {
                VaoBang();
            }
            else
            {
                if (GoToEx(BANG.TienViNhat))
                {
                    if (TLBB.IsQuestOpen)
                    {
                        if (QuestFrame.Click("#{CJBL_140805_03}"))
                            return;
                        if (QuestFrame.Click("#{DSKQ_141110_65}"))
                            RemoveMission(MissionsType.NhanPhiThuy);
                        QuestFrame.Close();
                    }
                    else
                    {
                        Talk(BANG.TienViNhat.Id);
                    }
                }
            }
            return;
        }

        public void GoSell()
        {
            if (!IsOneSec)
                return;
            if (TLBB.IsFollow)
                StopFollow();
            if (TLBB.IsShopOpen)
            {
                RemoveMission(MissionsType.BanRac);
                return;
            }
            if (GoToEx(LACDUONG.VanDieuDieu))
            {
                Talk(LACDUONG.VanDieuDieu);
                Thread.Sleep(1000);
                QuestFrame.Click(101, 0);
                QuestFrame.Close();
            }
        }

        public AntiCaptcha AntiCaptcha { get; set; }


        public bool IsHoiSinh
        {
            get
            {
                foreach (Game game in Party)
                {
                    if (game.SWHoiSinh.Elapsed.TotalSeconds < 12)
                        return true;
                }
                return false;
            }
        }



        private Stopwatch swClearMontersTime = Stopwatch.StartNew();

        public string PointVuCo = "";

        public uint LastShareAddress { get; set; }
        public uint ShareAddress { get; set; }
        public bool IsMoHop { get; set; }

        private bool isSafeVuCo(int x, int y)
        {
            foreach (var o in Objects.All.Where(o => o.Name == "Huyết chú vu cổ"))
            {
                if (o.GetDistance(x, y) <= 6)
                    return false;
            }
            return true;
        }

        //130,127
        public List<string> NeBayXuanThu = new List<string>()
        {
            "123,127",
            "137,127",
            "126,133",
            "134,133",
            "126,121",
            "134,121",
        };

        private int isafe = 0;

        private List<string> safevuco = new List<string>()
            {
                "126,112",
                "134,112",
                "142,112",
                "150,112",

                "150,120",
                "150,128",
                "150,136",
                "150,144",

                "142,144",
                "134,144",
                "126,144",
                "118,144",
                "110,144",
                "102,144",

                "102,136",
                "102,128",
                "102,120",

                "110,120",
                "118,120",
                "126,120",
                "134,120",
                "142,120",

                "142,128",
                "142,136",

                "136,136",
                "128,136",
                "120,136",
                "112,136",

                "112,128",
            };

        public string poitvuco = string.Empty;

        public string GetPointVuCo()
        {
            if (TLBB.IsLeader || Leader == null)
            {
                for (; isafe < safevuco.Count; isafe++)
                {
                    if (isSafeVuCo(TDT.ParseAllInt(safevuco[isafe].Split(',')[0]), TDT.ParseAllInt(safevuco[isafe].Split(',')[1])))
                    {
                        poitvuco = safevuco[isafe];
                        return safevuco[isafe];
                    }
                }
                isafe = 0;
                poitvuco = string.Empty;
                return string.Empty;
            }
            else
            {
                if (Leader != null)
                {
                    return Leader.poitvuco;
                }
                return string.Empty;
            }
        }

        public bool IsPhanDame { get; set; }

        public Stopwatch swLoginTime = Stopwatch.StartNew();

        public Stopwatch SwTimeOnMap = Stopwatch.StartNew();

        public bool ischeckttt { get; set; }
        public bool ischeckttt2 { get; set; }

        public bool IsPartyBusy
        {
            get
            {
                if (Missions.Contains(MissionsType.TriLieu) || Missions.Contains(MissionsType.BanRac) || Missions.Contains(MissionsType.CatVang) || Missions.Contains(MissionsType.CatDo))
                    return true;
                return false;
            }
        }

        public bool IsBusyAll
        {
            get
            {
                foreach (Game game in Party)
                {
                    if (game.IsPartyBusy)
                        return true;
                }
                return false;
            }
        }



        public bool RunTo(string point)
        {
            if (point.StartsWith("[") && point.Contains("]["))
            {
                point = TDT.StringBetween(point, "][", "]").Replace("-", ",");
                return GoToEx(point);
            }
            return true;
        }

        public string GetNPCName(string info)
        {
            info = info.Substring(info.IndexOf("-") + 1);
            info = info.Substring(0, info.IndexOf("]"));
            return info;
        }

        private string lastchinhtuyenname = string.Empty;
        private Stopwatch chinhtuyentime = Stopwatch.StartNew();

        public void ChinhTuyen()
        {
            if (!IsOneSec)
            {
                return;
            }
            ChinhTuyenTask = null;
            foreach (GameTask task in GameTask.Enum(this))
            {
                if (task.Name.Length > 2)
                {
                    if (task.Name == ChinhTuyenName)
                    {
                        ChinhTuyenTask = task;
                        break;
                    }
                }
            }
            if (ChinhTuyenTask != null)
            {
                if (ChinhTuyenTask.Name == "Đến Thiếu Lâm trừ gian kết nghĩa")
                {
                    RemoveMission(MissionsType.NhiemVuChinhTuyen);
                    alarmXongChinhTuyen = new Alam(this, "xong Chính Tuyến", 0);
                    _i.Alan.Instance.Controls.Add(alarmXongChinhTuyen);
                    PostMessage(3, 105);
                    return;
                }
                if (lastchinhtuyenname != ChinhTuyenName)
                {
                    lastchinhtuyenname = ChinhTuyenName;
                    chinhtuyentime = Stopwatch.StartNew();
                }
                else
                {
                    if (lastchinhtuyenname.Length > 2)
                    {
                        int maxtime = 1000;
                        if (ChinhTuyenName == "Ẩn danh do thám Cái Bang")
                        {
                            maxtime = 300;
                        }
                        if (SecCount % 3 == 0)
                        {
                            if (maxtime - chinhtuyentime.Elapsed.TotalSeconds > 0)
                            {
                                PushDebugMessage("Tự động hủy nhiệm vụ " + lastchinhtuyenname + " sau " + (int)(maxtime - chinhtuyentime.Elapsed.TotalSeconds) + "s");
                            }
                            else
                            {
                                LUA.DeleteMission(lastchinhtuyenname);
                                chinhtuyentime = Stopwatch.StartNew();
                                MissionState = "";
                            }
                        }
                    }
                }
            }
            else
            {
                chinhtuyentime = Stopwatch.StartNew();
            }
            if (MissionState == string.Empty)
            {
                if (!TLBB.IsTogleMission)
                {
                    PostMessage(18, 105);
                    Thread.Sleep(1000);
                }
                PostMessage(18, 105);
                Thread.Sleep(1000);
                foreach (GameTask task in GameTask.Enum(this))
                {
                    if (task.Name.Length > 2)
                    {
                        if (Properties.Resources.ChinhTuyen.Contains("[" + task.Name + "]"))
                        {
                            ChinhTuyenTask = task;
                            ChinhTuyenName = task.Name;
                            ChinhTuyenMission = TDT.StringBetween(Properties.Resources.ChinhTuyen, "[" + task.Name + "]->", "\r\n");
                            MissionState = "Do";
                            break;
                        }
                    }
                }
                if (MissionState == string.Empty)
                {
                    MissionState = "GoGet";
                }
                return;
            }
            if (MissionState == "GoGet")
            {
                if (RunTo("[Đại Lý-Triệu Thiên Sư][160,159-2]"))
                {
                    TalkEx("Triệu Thiên Sư");
                    Thread.Sleep(500);
                    foreach (QuestFrame frame in QuestFrame.Enum(this))
                    {
                        if (Properties.Resources.ChinhTuyen.Contains("[" + frame + "]"))
                        {
                            QuestFrame.Click((int)frame.StrOptionExtra1, (int)frame.StrOptionExtra2);
                            Thread.Sleep(500);
                            QuestFrameAccept();
                            Thread.Sleep(500);
                            MissionState = string.Empty;
                            return;
                        }
                    }
                    QuestFrame.Click("NV cốt truyện");
                    Thread.Sleep(500);
                    QuestFrame.Click("Hiện giờ ta đã tiến hành đến đâu rồi?");
                    Thread.Sleep(1000);
                    string txt = QuestFrame.Items[0].Name.Replace("#{", "").Replace("}", "");

                    if (GAMEDIC.DicCotTruyen.ContainsKey(txt))
                    {
                        ChinhTuyenMission = GAMEDIC.DicCotTruyen[txt];
                        ChinhTuyenName = ChinhTuyenMission;
                        if (Properties.Resources.ChinhTuyen.Contains("[" + ChinhTuyenMission + "]"))
                        {
                            ChinhTuyenMission = TDT.StringBetween(Properties.Resources.ChinhTuyen, "[" + ChinhTuyenMission + "]->", "\r\n");
                            MissionState = "GoRe";
                            return;
                        }
                        else
                        {
                        }
                    }
                    else
                    {
                        //Main.PushLog("Không hiểu");
                    }
                    chinhtuyentime = Stopwatch.StartNew();
                    MissionState = string.Empty;
                }
                return;
            }
            if (MissionState == "GoKimDang")
            {
                if (RunTo("[Thương Mang Sơn-Tiêu Phong][75,31-120]"))
                {
                    TalkEx("Tiêu Phong");
                    Thread.Sleep(1000);
                    if (QuestFrame.Click("Kim Qua Đãng Khấu Ngao Binh"))
                    {
                        MissionState = "ClearKimDang";
                        return;
                    }
                    MissionState = "";
                }
                return;
            }
            if (MissionState == "ClearKimDang")
            {
                if (GoToEx(50, 106, 120, false, 6))
                {
                    DownRide();
                    GameObject best = null;
                    foreach (GameObject obj in Objects.All)
                    {
                        if (obj.Name == "Gia Luật Niết Lỗ Cổ")
                        {
                            if (obj.HP > 0)
                            {
                                best = obj;
                            }
                            else
                            {
                                MissionState = "GoReEx";
                                ChinhTuyenName = "Lục Quân Tị Dịch";
                                ChinhTuyenMission = TDT.StringBetween(Properties.Resources.ChinhTuyen, "[" + ChinhTuyenName + "]->", "\r\n");
                                return;
                            }
                        }
                    }
                    if (best != null)
                    {
                        SelectTarget((int)best.Id);
                        SendKey(Global.BaseSkill);
                        chinhtuyentime = Stopwatch.StartNew();
                    }
                }
                return;
            }
            if (MissionState == "GoReEx")
            {
                if (IdleTime > 60)
                {
                    MissionState = "";
                    return;
                }
                if (chinhtuyentime.Elapsed.TotalMinutes > 2)
                {
                    MissionState = "";
                    return;
                }
                if (ChinhTuyenMission.Contains("->"))
                {
                    string re = ChinhTuyenMission.Substring(0, ChinhTuyenMission.IndexOf("->"));
                    if (RunTo(re))
                    {
                        Talk(TDT.StringBetween(re, "[", "]").Substring(re.IndexOf("-") + 1));
                        Thread.Sleep(1000);
                        if (QuestFrame.Click(ChinhTuyenName))
                        {
                            MissionState = "";
                        }
                        Thread.Sleep(1000);
                        QuestFrameAccept();
                        return;
                    }
                    else
                    {
                        return;
                    }
                }
                return;
            }
            if (MissionState == "GoRe")
            {
                if (ChinhTuyenMission.Contains("->"))
                {
                    string re = ChinhTuyenMission.Substring(0, ChinhTuyenMission.IndexOf("->"));
                    if (RunTo(re))
                    {
                        if (ChinhTuyenName == "Ẩn danh do thám Cái Bang")
                        {
                            DownRide();
                        }
                        Talk(TDT.StringBetween(re, "[", "]").Substring(re.IndexOf("-") + 1));
                        Thread.Sleep(1000);
                        if (ChinhTuyenName == "Lục Quân Tị Dịch")
                        {
                            if (!QuestFrame.Click(ChinhTuyenName))
                            {
                                MissionState = "GoKimDang";
                                return;
                            }
                        }
                        else
                        {
                            QuestFrame.Click(ChinhTuyenName);
                        }

                        if (ChinhTuyenName == "Bi Tô Thanh Phong")
                        {
                            if (QuestFrame.Click("Chỉ Điểm Quần Hào"))
                            {
                                MissionState = "ClearNhatPham";
                                return;
                            }
                        }
                        if (ChinhTuyenName == "Tình yêu không phải là mơ")
                        {
                            if (QuestFrame.Click("Bày tiệc rượu hỏi Quân Tam Ngữ"))
                            {
                                MissionState = "ClearDuoc";
                                swClearMontersTime = Stopwatch.StartNew();
                                return;
                            }
                        }

                        Thread.Sleep(2000);
                        QuestFrameAccept();
                        chinhtuyentime = Stopwatch.StartNew();
                        MissionState = "";
                        return;
                    }
                    else
                    {
                        return;
                    }
                }
                return;
            }
            if (MissionState == "ClearDuoc")
            {
                if (GoToEx(31, 80, 38, false, 12))
                {
                    DownRide();
                    float minHp = 999;
                    GameObject best = null;
                    foreach (GameObject obj in Objects.Monters)
                    {
                        if (obj.Name == "Nhất phẩm đốt đuốc")
                        {
                            if (obj.HP < minHp)
                            {
                                minHp = obj.HP;
                                best = obj;
                            }
                        }
                    }
                    if (best != null)
                    {
                        SelectTarget(best.Id);
                        SendKey(Global.BaseSkill);
                        return;
                    }
                    else
                    {
                        if (swClearMontersTime.Elapsed.TotalSeconds >= 10)
                        {
                            GoToEx("[Nhất phẩm đường-Hiểu Lôi][32,73-38]");
                        }
                        if (swClearMontersTime.Elapsed.TotalMinutes > 4.1)
                        {
                            MissionState = "GoRe";
                            ChinhTuyenMission = ChinhTuyenName = "Tình yêu không phải là mơ";
                            if (Properties.Resources.ChinhTuyen.Contains("[" + ChinhTuyenMission + "]"))
                            {
                                ChinhTuyenMission = TDT.StringBetween(Properties.Resources.ChinhTuyen, "[" + ChinhTuyenMission + "]->", "\r\n");
                                MissionState = "GoRe";
                                return;
                            }
                        }
                    }
                }
                else
                {
                    swClearMontersTime = Stopwatch.StartNew();
                }

                return;
            }
            if (MissionState == "ClearNhatPham")
            {
                if (GoToEx(62, 80, 37, false, 8))
                {
                    DownRide();
                    float minHp = 999;
                    GameObject best = null;
                    foreach (GameObject obj in Objects.Monters)
                    {
                        if (obj.Title == "Cao Thủ Của Nhất Phẩm Đường")
                        {
                            if (obj.HP < minHp)
                            {
                                minHp = obj.HP;
                                best = obj;
                            }
                        }
                    }
                    if (best != null)
                    {
                        SelectTarget((int)best.Id);
                        SendKey(Global.BaseSkill);
                        return;
                    }
                    else
                    {
                        if (swClearMontersTime.Elapsed.TotalSeconds >= 10)
                        {
                            MissionState = "GoRe";
                            ChinhTuyenMission = ChinhTuyenName = "Bi Tô Thanh Phong";
                            if (Properties.Resources.ChinhTuyen.Contains("[" + ChinhTuyenMission + "]"))
                            {
                                ChinhTuyenMission = TDT.StringBetween(Properties.Resources.ChinhTuyen, "[" + ChinhTuyenMission + "]->", "\r\n");
                                MissionState = "GoRe";
                                return;
                            }
                        }
                    }
                }
                else
                {
                    swClearMontersTime = Stopwatch.StartNew();
                }

                return;
            }
            if (MissionState == "Do")
            {
                if (ChinhTuyenTask.Name == "Cùng lên Thiếu Lâm" && !ChinhTuyenTask.Completed)
                {
                    if (RunTo("[Đại Lý-Đoàn Chính Thuần][71,18-2]"))
                    {
                        TalkEx("Đoàn Chính Thuần");
                        Thread.Sleep(1000);
                        QuestFrame.Click("#{CJG_101231_373}");
                    }
                    return;
                }
                if (ChinhTuyenTask.Name == "Ẩn danh do thám Cái Bang" && !ChinhTuyenTask.Completed)
                {
                    GoToEx(155, 135, 10);
                    return;
                }
                if (ChinhTuyenTask.Name == "Bi Tô Thanh Phong" && !ChinhTuyenTask.Completed)
                {
                    if (RunTo("[Yến Tử Ổ-Lý Diên Tông][62,76-37]"))
                    {
                        TalkEx("Lý Diên Tông");
                        Thread.Sleep(1000);
                        QuestFrame.ClickAll();
                    }
                    return;
                }
                if (ChinhTuyenTask.Name == "Trừ độc vật giải nguy Cái Bang" && !ChinhTuyenTask.Completed)
                {
                    if (GoToEx(55, 72, 27, false, 6))
                    {
                        ForcePickItem();
                        DownRide();
                        GameObject best = null;
                        float minHp = 999;
                        foreach (GameObject obj in Objects.Monters)
                        {
                            if (obj.Name == "Kịch Độc Lang Thù")
                            {
                                if (obj.HP < minHp)
                                {
                                    minHp = obj.HP;
                                    best = obj;
                                }
                            }
                        }
                        if (best != null)
                        {
                            SelectTarget((int)best.Id);
                            SendKey(Global.BaseSkill);
                        }
                    }
                    return;
                }
                if (ChinhTuyenTask.Name == "Huyết Chiến Tụ Hiền Trang" && !ChinhTuyenTask.Completed)
                {
                    if (GoToEx(59, 28, 36, false, 4))
                    {
                        DownRide();
                        float minHp = 999;
                        GameObject best = null;
                        foreach (GameObject obj in Objects.Monters)
                        {
                            if (obj.HP < minHp)
                            {
                                minHp = obj.HP;
                                best = obj;
                            }
                        }
                        if (best != null)
                        {
                            SelectTarget(best.Id);
                            SendKey(Global.BaseSkill);
                        }
                    }
                    return;
                }
                if (ChinhTuyenTask.Name == "Kim Qua Đãng Khấu Ngao Binh" && !ChinhTuyenTask.Completed)
                {
                    if (GoToEx(50, 106, 120, false, 6))
                    {
                        DownRide();
                        GameObject best = null;
                        foreach (GameObject obj in Objects.Monters)
                        {
                            if (obj.Name == "Gia Luật Niết Lỗ Cổ")
                            {
                                best = obj;
                            }
                        }
                        if (best != null)
                        {
                            SelectTarget((int)best.Id);
                            SendKey(Global.BaseSkill);
                        }
                    }
                    return;
                }
                if (ChinhTuyenTask.Name == "Đóng cửa bắt trộm" && !ChinhTuyenTask.Completed)
                {
                    if (RunTo("[Lôi Cổ Sơn-Đinh Xuân Thu][67,37-116]"))
                    {
                        DownRide();
                        GameObject best = null;
                        foreach (GameObject obj in Objects.Monters)
                        {
                            if (obj.Name == "Đinh Xuân Thu")
                            {
                                best = obj;
                            }
                        }
                        if (best != null)
                        {
                            SelectTarget(best.Id);
                            SendKey(Global.BaseSkill);
                            return;
                        }
                        TalkEx("Đinh Xuân Thu");
                        Thread.Sleep(1000);
                        QuestFrame.ClickAll();
                    }
                    return;
                }
                if (ChinhTuyenTask.Name == "Thua thắng thành bại ai biết được" && !ChinhTuyenTask.Completed)
                {
                    if (GoToEx(75, 31, 117, false, 4))
                    {
                        DownRide();
                        GameObject best = null;
                        float minHp = 999;
                        foreach (GameObject obj in Objects.Monters)
                        {
                            if (obj.HP < minHp && obj.Name == "Cương Thi")
                            {
                                minHp = obj.HP;
                                best = obj;
                            }
                        }
                        if (best != null)
                        {
                            SelectTarget((int)best.Id);
                            SendKey(Global.BaseSkill);
                        }
                    }
                    return;
                }
                if (ChinhTuyenTask.Name == "Chỉ Điểm Quần Hào" && !ChinhTuyenTask.Completed)
                {
                    if (GoToEx(62, 80, 37, false, 8))
                    {
                        DownRide();
                        float minHp = 999;
                        GameObject best = null;
                        foreach (GameObject obj in Objects.Monters)
                        {
                            if (obj.Title == "Cao Thủ Của Nhất Phẩm Đường")
                            {
                                if (obj.HP < minHp)
                                {
                                    minHp = obj.HP;
                                    best = obj;
                                }
                            }
                        }
                        if (best != null)
                        {
                            SelectTarget((int)best.Id);
                            SendKey(Global.BaseSkill);
                        }
                    }
                    return;
                }
                if (ChinhTuyenTask.Name == "Hổ khiếu long ngâm" && !ChinhTuyenTask.Completed)
                {
                    ForcePickItem();
                    foreach (var item in PacketItems.All)
                    {
                        if (item.Name == "Chìa Khóa Thạch Ốc")
                        {
                            if (RunTo("[Vạn Kiếp Cốc-Vạn Kiếp Cốc Thạch Cảm Đương][48,95-118]"))
                            {
                                Talk("Vạn Kiếp Cốc Thạch Cảm Đương");
                                Thread.Sleep(1000);
                                QuestFrame.ClickAll();
                            }
                            return;
                        }
                    }
                    if (RunTo("[Vạn Kiếp Cốc-Đoàn Diên Khánh][51,97-118]"))
                    {
                        DownRide();
                        SelectTarget((int)Objects.GetObject("Đoàn Diên Khánh").Id);
                        SendKey(Global.BaseSkill);
                    }
                    return;
                }
                if (ChinhTuyenTask.Name == "Độc chiến song hùng" && !ChinhTuyenTask.Completed)
                {
                    if (ChinhTuyenTask != null)
                    {
                        bool isloicongoanh = false;
                        bool isnguhodoanmondao = false;
                        ForcePickItem();
                        foreach (var item in PacketItems.All)
                        {
                            if (item.Name == "Lôi Công Oanh")
                            {
                                isloicongoanh = true;
                            }
                            if (item.Name == "Ngũ Hổ Đoạn Môn Đao")
                            {
                                isnguhodoanmondao = true;
                            }
                        }
                        foreach (GameObject obj in Objects.All)
                        {
                            if (obj.Menpai == 12)
                            {
                                if (obj.Name == "Diêu Bá Đương" || obj.Name == "Tư Mã Lâm")
                                {
                                    LUA.DeleteMission("Độc chiến song hùng");
                                    MissionState = "";
                                    return;
                                }
                            }
                        }
                        if (!isloicongoanh)
                        {
                            if (RunTo("[Yến Tử Ổ-Tư Mã Lâm][59,15-114]"))
                            {
                                DownRide();
                                SelectTarget((int)Objects.GetObject("Tư Mã Lâm").Id);
                                SendKey(Global.BaseSkill);
                                return;
                            }
                        }
                        else
                        {
                            if (!isnguhodoanmondao)
                            {
                                if (RunTo("[Yến Tử Ổ-Diêu Bá Đương][94,47-114]"))
                                {
                                    DownRide();
                                    SelectTarget(Objects.GetObject("Diêu Bá Đương").Id);
                                    SendKey(Global.BaseSkill);
                                    return;
                                }
                            }
                        }
                    }
                    else
                    {
                        MissionState = "";
                        return;
                    }
                    return;
                }
                if (ChinhTuyenTask.Name == "Đỉnh thiên lập địa" && !ChinhTuyenTask.Completed)
                {
                    if (ChinhTuyenTask != null)
                    {
                        if (ChinhTuyenTask.Count1 == 0)
                        {
                            if (RunTo("[Tụ Hiền Trang-Bào Thiên Linh][72,64-113]"))
                            {
                                DownRide();
                                SelectTarget(Objects.GetObject("Bào Thiên Linh").Id);
                                SendKey(Global.BaseSkill);
                                return;
                            }
                        }
                        else
                        {
                            if (ChinhTuyenTask.Count2 == 0)
                            {
                                if (RunTo("[Tụ Hiền Trang-Kỳ Lục][50,63-113]"))
                                {
                                    DownRide();
                                    SelectTarget(Objects.GetObject("Kỳ Lục").Id);
                                    SendKey(Global.BaseSkill);
                                    return;
                                }
                            }
                            else
                            {
                                if (RunTo("[Tụ Hiền Trang-Hướng Vọng Hải][60,56-113]"))
                                {
                                    DownRide();
                                    SelectTarget(Objects.GetObject("Hướng Vọng Hải").Id);
                                    SendKey(Global.BaseSkill);
                                    return;
                                }
                            }
                        }
                    }
                    else
                    {
                        MissionState = "";
                        return;
                    }
                    return;
                }
                if (ChinhTuyenTask.Name == "Thiên thời không bằng địa lợi" && !ChinhTuyenTask.Completed)
                {
                    if (ChinhTuyenTask != null)
                    {
                        if (ChinhTuyenTask.Count1 == 0)
                        {
                            GoToEx("47,106,121");
                            return;
                        }
                        else
                        {
                            if (ChinhTuyenTask.Count2 == 0)
                            {
                                GoToEx("65,87,121");
                                return;
                            }
                            else
                            {
                                if (ChinhTuyenTask.Count3 == 0)
                                {
                                    GoToEx("23,83,121");
                                    return;
                                }
                                else
                                {
                                    GoToEx("83,105,121");
                                    return;
                                }
                            }
                        }
                    }
                    else
                    {
                        MissionState = "";
                        return;
                    }
                }
                if (ChinhTuyenTask.Name == "Đại chiến Vạn Kiếp Cốc" && !ChinhTuyenTask.Completed)
                {
                    if (ChinhTuyenTask != null)
                    {
                        foreach (GameObject obj in Objects.All)
                        {
                            if (obj.Menpai == 16)
                            {
                                if (obj.Name == "Vân Trung Hạc" || obj.Name == "Nhạc Lão Tam" || obj.Name == "Diệp Nhị Nương")
                                {
                                    LUA.DeleteMission("Đại chiến Vạn Kiếp Cốc");
                                    MissionState = "";
                                    return;
                                }
                            }
                        }
                        if (ChinhTuyenTask.Count2 == 0)
                        {
                            if (RunTo("[Vạn Kiếp Cốc-Vân Trung Hạc][106,27-118]"))
                            {
                                DownRide();
                                SelectTarget(Objects.GetObject("Vân Trung Hạc").Id);
                                SendKey(Global.BaseSkill);
                                return;
                            }
                        }
                        else
                        {
                            if (ChinhTuyenTask.Count1 == 0)
                            {
                                if (RunTo("[Vạn Kiếp Cốc-Nhạc Lão Tam][78,20-118]"))
                                {
                                    DownRide();
                                    SelectTarget(Objects.GetObject("Nhạc Lão Tam").Id);
                                    SendKey(Global.BaseSkill);
                                    return;
                                }
                            }
                            else
                            {
                                if (RunTo("[Vạn Kiếp Cốc-Diệp Nhị Nương][52,33-118]"))
                                {
                                    DownRide();
                                    SelectTarget(Objects.GetObject("Diệp Nhị Nương").Id);
                                    SendKey(Global.BaseSkill);
                                    return;
                                }
                            }
                        }
                    }
                    else
                    {
                        MissionState = "";
                        return;
                    }
                    return;
                }
                string re = ChinhTuyenMission.Substring(ChinhTuyenMission.IndexOf("->") + 2);
                if (RunTo(re))
                {
                    Talk(TDT.StringBetween(re, "[", "]").Substring(re.IndexOf("-") + 1));
                    Thread.Sleep(1000);
                    QuestFrame.Click(ChinhTuyenName);
                    if (ChinhTuyenName == "Trừ độc vật giải nguy Cái Bang")
                        DownRide();
                    Thread.Sleep(1000);
                    LUA.QuestFrameMissionContinue();
                    Thread.Sleep(1000);
                    LUA.QuestFrameMissionComplete(0);
                    Thread.Sleep(1000);
                    QuestFrameAccept();
                    Thread.Sleep(1000);
                    Talk(TDT.StringBetween(re, "[", "]").Substring(re.IndexOf("-") + 1));
                    Thread.Sleep(1000);
                    foreach (QuestFrame frame in QuestFrame.Enum(this))
                    {
                        if (GAMEDIC.DicCotTruyen.ContainsValue(frame.Name))
                        {
                            QuestFrame.Click((int)frame.StrOptionExtra1, (int)frame.StrOptionExtra2);
                            Thread.Sleep(1000);
                            break;
                        }
                    }
                    QuestFrameAccept();
                    MissionState = "";
                    return;
                }
                else
                {
                    return;
                }
            }
            MissionState = string.Empty;
        }

        public void MuaThoLinhChau()
        {
            Task.Run(() =>
            {
                if (HaveItem("tholinhchau"))
                    return;
                if (!TLBB.IsToggleYuanbaoShop)
                {
                    LUA.ToggleYuanbaoShop();
                    Thread.Sleep(100);
                }
                DoStringEx("setmetatable(_G, {__index = YuanbaoShop_Env}); NpcShop:SetBuyDirectly(1);");
                Thread.Sleep(500);
                DoStringEx("setmetatable(_G, {__index = YuanbaoShop_Env}); YuanbaoShop_ChangeTabIndex(1);");
                Thread.Sleep(500);
                DoStringEx("setmetatable(_G, {__index = YuanbaoShop_Env}); YuanbaoShop_UpdateList_Bind(4);");
                Thread.Sleep(500);
                DoStringEx("setmetatable(_G, {__index = YuanbaoShop_Env}); YuanbaoShop_UpdateShop_Bind(2);");
                Thread.Sleep(1000);
                foreach (var item in PacketItems.Shop)
                {
                    if (TDT.VietLien(item.Name).Contains("tholinhchau"))
                    {
                        Buy((int)item.Index);
                        Thread.Sleep(500);
                        break;
                    }
                }
                DoStringEx("setmetatable(_G, {__index = YuanbaoShop_Env}); this:Hide();");

            });
        }

        public void MuaTuBoDanChuaCo()
        {
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                if (PacketItems.All.Where(i => i.ClearName.Contains("tubodan") || i.ClearName.Contains("hoixuandan")).FirstOrDefault() != null)
                    return;
                if (!TLBB.IsToggleYuanbaoShop)
                {
                    LUA.ToggleYuanbaoShop();
                    Thread.Sleep(100);
                }
                DoStringEx("setmetatable(_G, {__index = YuanbaoShop_Env}); NpcShop:SetBuyDirectly(1);");
                Thread.Sleep(500);
                DoStringEx("setmetatable(_G, {__index = YuanbaoShop_Env}); YuanbaoShop_ChangeTabIndex(1);");
                Thread.Sleep(500);
                DoStringEx("setmetatable(_G, {__index = YuanbaoShop_Env}); YuanbaoShop_UpdateList_Bind(3);");
                Thread.Sleep(1000);
                int index = -1;
                foreach (var item in PacketItems.Shop)
                {
                    if (TDT.VietLien(item.Name).Contains("tranthutubodan"))
                    {
                        index = (int)item.Index;
                        Buy(index);
                        break;
                    }
                }
                DoStringEx("setmetatable(_G, {__index = YuanbaoShop_Env}); this:Hide();");
            }).Start();
        }

        private Stopwatch ThreeSec = Stopwatch.StartNew();

        public Stopwatch swIsOneSec = Stopwatch.StartNew();
        public bool IsOneSec { get; set; }

        public int SecCount { get; set; }

        public bool IsHaoHuu { get; set; }

        public bool IsNhanBongSuDo { get; set; }

        public void SuDoo()
        {
            if (!Global.IsSuDoo)
                return;
            if (!TLBB.IsLeader)
                return;
            TrieuTap();

            if (TLBB.Lvl < 30)
            {
                foreach (Game game in Party)
                {
                    if (game.TLBB.Lvl >= 30)
                    {
                        AppointLeader(game.TLBB.Name);
                        return;
                    }
                }
            }

            if (GoToEx(DAILY.NhiepChinh))
            {
                if (Party.Count() < 6)
                {
                    return;
                }
                if (!TrieuTap())
                {
                    return;
                }
                if (!IsHaoHuu)
                {
                    Party.ForEach(g =>
                    {
                        foreach (var game in g.Party)
                        {
                            if (game.TLBB.Name == g.TLBB.Name)
                                continue;
                            g.DoStringEx("DataPool: AddFriendAndGrouping('" + game.TLBB.Name + "');");
                            Thread.Sleep(1000);
                            g.DoStringEx("setmetatable(_G, { __index = Friend_IMGrouping_Env}); Friend_IMGrouping_OK_Clicked();");
                            Thread.Sleep(1000);
                        }
                        //for (int i = 0; i < g.Party.Count; i++)
                        //{
                        //    if (g.Party[i].TLBB.Name == g.TLBB.Name)
                        //        continue;
                        //    g.DoStringEx("DataPool: AddFriendAndGrouping('" + g.Party[i].TLBB.Name + "');");
                        //    Thread.Sleep(1000);
                        //    g.DoStringEx("setmetatable(_G, { __index = Friend_IMGrouping_Env}); Friend_IMGrouping_OK_Clicked();");
                        //    Thread.Sleep(1000);
                        //}
                        g.IsHaoHuu = true;
                    });
                    return;
                }
                Talk(DAILY.NhiepChinh);
                Thread.Sleep(1000);
                QuestFrame.Click("#{BSYH_150519_02}");
                Thread.Sleep(1000);
                QuestFrame.Click("#{BSYH_150519_266}");
                Thread.Sleep(1000);
                if (QuestFrame.All(this).Contains("#{BSYH_150519_288}"))
                {
                    Party.ForEach(g => g.DoStringEx("Player:LeaveTeam();"));
                    Party.ForEach(g => g.IsHaoHuu = false);
                    cntKetNghia = 0;
                    return;
                }
                if (QuestFrame.All(this).Contains("#{BSYH_150519_277}"))
                {
                    Party.ForEach(g => g.DoStringEx("Player:LeaveTeam();"));
                    Party.ForEach(g => g.IsHaoHuu = false);
                    cntKetNghia = 0;
                    return;
                }
                else
                {
                    if (cntKetNghia++ > 3)
                    {
                        IsSuDoo = false;
                        cntKetNghia = 0;
                    }
                }
            }
        }

        public void KetNghia()
        {
            if (!TLBB.IsLeader)
                return;
            if (Party.Count() < 6)
            {
                return;
            }
            if (FullPartyGoToEx(LACDUONG.TranPhuChi))
            {
                if (IsByLogin)
                {
                    DoStringEx("return DataPool:GetBrotherCount();");
                    LuaToString();
                    Thread.Sleep(500);
                    if (LuaToString() == "6")
                    {
                        IsKetNghia = false;
                        return;
                    }
                }
                if (!IsHaoHuu)
                {
                    Party.ForEach(g =>
                    {
                        Task.Run(() =>
                        {
                            g.KetNghiaByTeam();
                        });
                        g.IsHaoHuu = true;
                    });
                    return;
                }
                Talk(LACDUONG.TranPhuChi);
                Thread.Sleep(1000);
                QuestFrame.Click("#G#{JBLC_150528_2}");
                Thread.Sleep(1000);
                QuestFrame.Click("#{JBLC_150528_341}");
                Thread.Sleep(1000);
                Party.ForEach(g => { if (g != this) { g.DoStringEx("setmetatable(_G, {__index = WuhunQuest_Env}); WuhunQuest_Bn1Click();"); } });
                if (QuestFrame.All(this).Contains("#{JBLC_150528_14}"))
                {
                    Party.ForEach(g => g.DoStringEx("Player:LeaveTeam();"));
                    Party.ForEach(g => g.IsHaoHuu = false);
                    cntKetNghia = 0;
                    return;
                }
                if (QuestFrame.All(this).Contains("#{JBLC_150528_287}"))
                {
                    Party.ForEach(g => g.DoStringEx("Player:LeaveTeam();"));
                    Party.ForEach(g => g.IsHaoHuu = false);
                    cntKetNghia = 0;
                    return;
                }
                else
                {
                    //if (IsKetNghiaByLogin)
                    //{
                    //    if (cntKetNghia++ > 3)
                    //    {
                    //        IsKetNghia = false;
                    //        cntKetNghia = 0;
                    //    }

                    //}
                }
            }
        }


        public Stopwatch TrueClearMonterTime { get; set; } = Stopwatch.StartNew();
        public bool IsSettingLoaded { get; set; }

        public bool IsDiemDanh { get; set; } = true;
        public bool IsDiemDanhEx { get; set; } = true;

        public static object syncObj = new object();

        public Stopwatch TrueStandTime { get; set; } = Stopwatch.StartNew();




        public void ThaThiemDienDieu()
        {
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                DoStringEx("for	i=1, 10 do if Pet:IsPresent(i-1) then szPetName = Pet :GetPetList_Appoint(i-1); if string.find(szPetName,'Thi¬m ði®n ðiêu') then PushDebugMessage(szPetName); Pet:Free_Confirm(i-1); break; end	end end");
                Thread.Sleep(1000);
                LUA.MessageBox_Self_OK_Clicked();
            }).Start();
        }

        private void GETQDINFO()
        {
            if (Address.GameType == 1)
            {
                try
                {
                    DoStringEx("local memCount = Raid:GetMemCount(); local info = ''; for i = 1, 1 do for j = 1, 6 do  local name, menpai, level, dead, offline, scene = Raid:GetMemberInfoByIdx(i - 1, j - 1);  if level > 0 then  info = info .. name .. '-';  memCount = memCount - 1;  end  if memCount == 0 then  break;  end end if memCount == 0 then  break; end end info = info .. ';'; for i = 2, 2 do for j = 1, 6 do  local name, menpai, level, dead, offline, scene = Raid:GetMemberInfoByIdx(i - 1, j - 1);  if level > 0 then  info = info .. name .. '-';  memCount = memCount - 1;  end  if memCount == 0 then  break;  end end if memCount == 0 then  break; end end return info;");
                    string quandoan = LuaToStringMemo("QuanDoan");
                    string pt1 = quandoan.Split(';')[0].Trim('-');
                    QuanDoanParty1.Clear();
                    QuanDoanParty2.Clear();
                    foreach (string n in pt1.Split('-'))
                    {
                        if (n.Length >= 0)
                        {
                            string allowname = TDT.GetAllowName(n);
                            if (allowname.Length > 0)
                            {
                                foreach (KeyValuePair<int, Game> kvp in Main.DicGame)
                                {
                                    if (kvp.Value.TLBB.AllowName == allowname)
                                    {
                                        QuanDoanParty1.Add(kvp.Value);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    if (quandoan.Split(';')[1].Length >= 2)
                    {
                        string pt2 = quandoan.Split(';')[1].Trim('-');
                        foreach (string n in pt2.Split('-'))
                        {
                            if (n.Length >= 0)
                            {
                                string allowname = TDT.GetAllowName(n);
                                if (allowname.Length > 0)
                                {
                                    foreach (KeyValuePair<int, Game> kvp in Main.DicGame)
                                    {
                                        if (kvp.Value.TLBB.AllowName == allowname)
                                        {
                                            QuanDoanParty2.Add(kvp.Value);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch { }
            }
        }

        private void FreshVal()
        {
            ListMoveEx.Clear();
            MoveExTime = new Stopwatch();
            lasmapid = TLBB.MapId;
            ListPickedIds.Clear();
            IsWaitDoiNgu = true;
            DeadObjects.Clear();
            TLBB.MapName = string.Empty;
            if (IsMapPhuBan())
                MoveIndex = 0;

            ListPointBaoDoHiem.Clear();
            isafe = 0;
            isdakhieuchien = false;
            IdleTime = 0;
            if (TLBB.MapId != TLBB.MapMonPhai && !IsTKC)
            {
                if (TLBB.MapId != MAP.TacKhauDoanhDia && TLBB.MapId != MapAcTac && !IsTKC)
                    MoveIndex = -1;
                else if (TLBB.MapId == MAP.PhungHoangCoThanh || IsMapPhuBan() || TLBB.MapId == MAP.ThuyLao)
                    MoveIndex = 0;
                else
                    MoveIndex = 0;
            }
            quandoanindex = -1;
            CareX = CareY = 0;
            CareObject = null;
            QTOCHAUSTATE = QLAULANSTATE = 0;
            QToChauClearMonterTime = QLauLanClearMonterTime = Stopwatch.StartNew();
            ClearMonterTimeEx = Stopwatch.StartNew();
            swonly = null;
            ThuThapIdx = 1;
            RadiusExMap = -1;
            MoveNextExIdx = 0;
            IsSetRadiusEx = false;
            ischeckttt = false;
            ischeckttt2 = false;
            IsXongThuyLao = false;
            swAutoMove = Stopwatch.StartNew();
            IdleTime = 0;
            Talked = false;
            IsMoHop = false;
            BossTime = DateTime.MinValue;
            MapATIndex = -1;
            IsBossDie = false;
            IsTalkPhuManNghi = false;
            IsTalkOLaoDai = false;
            TKCInfo = "";
            IsTraiTKC = false;
            IsDuDocDie = false;
            IsHongHungVuongDie = false;
            IsNguyTongQuanDie = false;
            TKCComplete = CheckTKCComplete = TKCompleted = false;
            tranTime = Stopwatch.StartNew();
            IsClick = TraQ = NhanQ = IsContinute = false;
            IsP = false;
            CauQ1 = 0;
            CurMapATIndex = -1;
            BossDieTime = Stopwatch.StartNew();
            IsQuangCao = false;
            isAlarmHuyetMo = false;
            DaNhanThuyLao = false;
            IsXong50NguyTongBinh = false;
            NotSafe.Clear();
            ListDangLumHop.Clear();
            //AcBa = -1;
            BlackList.Clear();
            CareTime = Stopwatch.StartNew();
            randomTXT = null;
            swStandTime = Stopwatch.StartNew();
            IsXongKyCuoc = IsXongTamBao = false;
            SatTinhMonter = "";

            ClearTime = Stopwatch.StartNew();
            swClearMontersTime = Stopwatch.StartNew();
            IsClearFistRound = false;
            MoveExTime = Stopwatch.StartNew();
            ListMoveEx.Clear();
            ClearSatTinhCount = 0;
            IsAlarmLoiNhiemVu = false;
            SwTimeOnMap = Stopwatch.StartNew();
        }

        public int MapMaTac { get; set; } = -1;

        public int CurMapMaTac { get; set; } = -1;

        private Dictionary<int, Stopwatch> dicmatac = new Dictionary<int, Stopwatch>();

        public int CurBossMap { get; set; } = -1;
        public uint AddressGameExe { get; set; } = 0;

        public void SuaTrangBi()
        {
            if (!Global.IsSuaTrangBi)
            {
                RemoveMission(MissionsType.SuaTrangBi);
                return;
            }
            var doben = Setting.Value("numberSuaTrangBi");
            if (doben <= 0)
                doben = 25;
            if (TLBB.MapId == MAP.KimLang)
            {
                if (GoToEx(KimLang.LyLapThanh))
                {
                    RemoveMission(MissionsType.SuaTrangBi);
                    DoStringEx("local g_ValidEquipPos={ 1,15,14,3,5,4,6,11,12,13, 2,7,21,19,20 }; local maxrun = 0; for k,v in pairs(g_ValidEquipPos) do ActionEquip=EnumAction(v, 'equip'); if ActionEquip:GetID() ~=0 then local nTimes = ActionEquip:GetRemainRepairCount(); if nTimes > 0 then local curDur,MaxDur= ActionEquip:GetEquipDurValue(); if curDur <= " + doben + " then Clear_XSCRIPT(); Set_XSCRIPT_Function_Name('OnEquipRepairofEquip'); Set_XSCRIPT_ScriptID(805027); Set_XSCRIPT_Parameter(0,0);Set_XSCRIPT_Parameter(1,139); Set_XSCRIPT_Parameter(2,v); Set_XSCRIPT_ParamCount(3); Send_XSCRIPT(); Send_XSCRIPT(); end end end end ");
                    Thread.Sleep(1000);
                    DoStringEx("local g_ValidEquipPos={ 1,15,14,3,5,4,6,11,12,13, 2,7,21,19,20 }; local maxrun = 0; for k,v in pairs(g_ValidEquipPos) do ActionEquip=EnumAction(v, 'equip'); if ActionEquip:GetID() ~=0 then local nTimes = ActionEquip:GetRemainRepairCount(); if nTimes > 0 then local curDur,MaxDur= ActionEquip:GetEquipDurValue(); if curDur <= " + doben + " then Clear_XSCRIPT(); Set_XSCRIPT_Function_Name('OnEquipRepairofEquip'); Set_XSCRIPT_ScriptID(805027); Set_XSCRIPT_Parameter(0,0);Set_XSCRIPT_Parameter(1,139); Set_XSCRIPT_Parameter(2,v); Set_XSCRIPT_ParamCount(3); Send_XSCRIPT(); Send_XSCRIPT(); end end end end ");
                }
            }
            else
            {
                if (GoToEx(TOCHAU.TietPhi))
                {
                    RemoveMission(MissionsType.SuaTrangBi);
                    DoStringEx("local g_ValidEquipPos={ 1,15,14,3,5,4,6,11,12,13, 2,7,21,19,20 }; local maxrun = 0; for k,v in pairs(g_ValidEquipPos) do ActionEquip=EnumAction(v, 'equip'); if ActionEquip:GetID() ~=0 then local nTimes = ActionEquip:GetRemainRepairCount(); if nTimes > 0 then local curDur,MaxDur= ActionEquip:GetEquipDurValue(); if curDur <= " + doben + "  then Clear_XSCRIPT(); Set_XSCRIPT_Function_Name('OnEquipRepairofEquip'); Set_XSCRIPT_ScriptID(805027); Set_XSCRIPT_Parameter(0,0);Set_XSCRIPT_Parameter(1,139); Set_XSCRIPT_Parameter(2,v); Set_XSCRIPT_ParamCount(3); Send_XSCRIPT(); Send_XSCRIPT(); end end end end ");
                    Thread.Sleep(1000);
                    DoStringEx("local g_ValidEquipPos={ 1,15,14,3,5,4,6,11,12,13, 2,7,21,19,20 }; local maxrun = 0; for k,v in pairs(g_ValidEquipPos) do ActionEquip=EnumAction(v, 'equip'); if ActionEquip:GetID() ~=0 then local nTimes = ActionEquip:GetRemainRepairCount(); if nTimes > 0 then local curDur,MaxDur= ActionEquip:GetEquipDurValue(); if curDur <= " + doben + "  then Clear_XSCRIPT(); Set_XSCRIPT_Function_Name('OnEquipRepairofEquip'); Set_XSCRIPT_ScriptID(805027); Set_XSCRIPT_Parameter(0,0);Set_XSCRIPT_Parameter(1,139); Set_XSCRIPT_Parameter(2,v); Set_XSCRIPT_ParamCount(3); Send_XSCRIPT(); Send_XSCRIPT(); end end end end ");
                }
            }
        }

        private void TieuKNBKhoa()
        {


        }

        private static Stopwatch swhook = Stopwatch.StartNew();

        private void HookRecv()
        {
            if (IsHooked || Main.IsExit)
                return;
            if (!Setting.Is("checkAlarmInbox"))
                return;
            lock (syncObj)
            {
         
                if (ShareAddress == 0)
                {
                    ShareAddress = Memory.VirtualAllocEx(1001000);
                }
                if (LastShareAddress == 0)
                    LastShareAddress = ShareAddress;
                if (ShareAddress != 0 && !IsHooked && (Missions.Contains(MissionsType.LuyenKim) || (Missions.Contains(MissionsType.LuyenKimNhanh) || Missions.Contains(MissionsType.DatDoiThieuThatSon))))
                {
                    RecvAddress = (int)GetRemoteProcAddress(Process.GetProcessById(ProcessId), "ws2_32.dll", "recv");
                    if (RecvAddress != 0)
                    {
                        IsHooked = true;
                        Memory.ReadProcessMemory(Memory.Id, RecvAddress, bufferRecv, 10, 0);
                        PostMessage((int)ShareAddress, -8);
                        PostMessage(ProcessId, -10);
                    }
                }
            }
        }

        public Stopwatch SWHoiSinh = Stopwatch.StartNew();

        private bool setSafeTime;

        public string GetNameMaNhaiDong(int x, int y)
        {
            if (TDT.is_point_inside_quad(new Point(x, y), new Point(169, 21), new Point(177, 21), new Point(177, 24), new Point(169, 24)))
            {
                return "Jump1";
            }
            if (TDT.is_point_inside_quad(new Point(x, y), new Point(181, 20), new Point(188, 20), new Point(188, 24), new Point(181, 24)))
            {
                return "Jump2";
            }
            if (TDT.is_point_inside_quad(new Point(x, y), new Point(194, 20), new Point(200, 20), new Point(200, 24), new Point(194, 24)))
            {
                return "Jump3";
            }
            if (TDT.is_point_inside_quad(new Point(x, y), new Point(204, 20), new Point(212, 20), new Point(212, 24), new Point(204, 20)))
            {
                return "Jump4";
            }
            if (TDT.is_point_inside_quad(new Point(x, y), new Point(186, 30), new Point(190, 30), new Point(190, 36), new Point(186, 36)))
            {
                return "Jump5";
            }
            if (TDT.is_point_inside_penta(new Point(x, y), new Point(152, 32), new Point(228, 0), new Point(255, 0), new Point(255, 76), new Point(176, 65)))
            {
                return "Jump6";
            }
            if (TDT.is_point_inside_quad(new Point(x, y), new Point(176, 65), new Point(255, 65), new Point(255, 255), new Point(176, 255)))
            {
                return "Jump6";
            }
            return "Jump0";
        }

        public string GetNameTanHoangDiaCung2(int x, int y)
        {
            if (x >= 108 && x <= 117 && y >= 33 && y <= 44)
            {
                return "Jump2";
            }
            if (TDT.is_point_inside_penta(new Point(x, y), new Point(91, 193), new Point(91, 79), new Point(143, 0), new Point(255, 0), new Point(255, 193)))
            {
                return "Jump3";
            }
            return "Jump1";
        }

        public string GetNameTanHoangDiaCung(int x, int y)
        {
            if (x >= 205 && x <= 215 && y >= 112 && y <= 116)
            {
                return "Jump2";
            }
            if (x >= 194 && x <= 202 && y >= 112 && y <= 116)
            {
                return "Jump3";
            }
            if (x >= 122 && x <= 147 && y >= 120 && y <= 124)
            {
                return "Jump5";
            }
            if (x >= 23 && x <= 28 && y >= 145 && y <= 154)
            {
                return "Jump7";
            }
            if (x >= 25 && x <= 28 && y >= 130 && y <= 139)
            {
                return "Jump8";
            }
            if (x >= 106 && x <= 131 && y >= 209 && y <= 217)
            {
                return "Jump10";
            }
            if (TDT.is_point_inside_trigon(new Point(x, y), new Point(205, 112), new Point(255, 51), new Point(255, 112)))
            {
                return "Jump1";
            }
            if (TDT.is_point_inside_quad(new Point(x, y), new Point(208, 112), new Point(255, 112), new Point(255, 255), new Point(208, 255)))
            {
                return "Jump1";
            }
            if (TDT.is_point_inside_quad(new Point(x, y), new Point(136, 90), new Point(205, 90), new Point(215, 116), new Point(147, 124)))
            {
                return "Jump4";
            }
            if (TDT.is_point_inside_penta(new Point(x, y), new Point(147, 124), new Point(208, 116), new Point(208, 255), new Point(119, 255), new Point(119, 211)))
            {
                return "Jump4";
            }
            if (TDT.is_point_inside_penta(new Point(x, y), new Point(53, 115), new Point(136, 90), new Point(147, 120), new Point(119, 211), new Point(28, 154)))
            {
                return "Jump6";
            }
            if (TDT.is_point_inside_penta(new Point(x, y), new Point(0, 154), new Point(28, 154), new Point(119, 211), new Point(119, 255), new Point(0, 255)))
            {
                return "Jump6";
            }

            return "Jump9";
        }


        public string TaskNote
        {
            get
            {
                string note = Game.IniParser.Read("Calender", TLBB.AllowName); ;
                if (note == null)
                    note = "";
                note += "\r\n" + Game.IniParser.Read("Calender", "All"); ;
                return note;
            }
        }

        private void SetCalendar()
        {
            if (Main.SettingForm.checkUseCalender.Checked && SecCount % 5 == 0 && !IsMapPhuBan())
            {
                string note = TaskNote;
                foreach (string s in note.Split('\n'))
                {
                    string n = s.Trim();
                    if (n.Split('|').Length > 1)
                    {
                        string time = n.Split('|')[0];
                        if (time.Split('-').Length > 1)
                        {
                            int fromMinute = ToMinute(time.Split('-')[0]);
                            int endMinute = ToMinute(time.Split('-')[1]);
                            int minuteNow = DateTime.Now.Hour * 60 + DateTime.Now.Minute;
                            if (minuteNow >= fromMinute && minuteNow < endMinute)
                            {

                                string nhiemvu = "";
                                nhiemvu = TDT.VietLien(n);
                                //ClearMission();
                                Missions = Missions.Where(type =>
                                type == MissionsType.TriLieu ||
                                type == MissionsType.BanRac ||
                                type == MissionsType.CatVang ||
                                type == MissionsType.CatDo ||
                                type == MissionsType.ThuTaiVanMay ||
                                type == MissionsType.LoLyHoa ||
                                type == MissionsType.NguyenVongThienLinh
                                ).ToList();
                                NeedToMove = string.Empty;
                                if (IsMapPhuBan())
                                {
                                }
                                else if (nhiemvu.Contains("thoatgame"))
                                {
                                    Exit();
                                }
                                if (nhiemvu.Contains("tubaobon"))
                                {
                                    PushMissions(MissionsType.TuBaoBon);
                                }
                                else if (nhiemvu.Contains("binhthanhkho"))
                                {
                                    PushMissions(MissionsType.DatDoiBinhThanh);
                                    PushMissions(MissionsType.BinhThanhKho);
                                }
                                else if (nhiemvu.Contains("binhthanh"))
                                {
                                    PushMissions(MissionsType.DatDoiBinhThanh);
                                }                               
                                else if (nhiemvu.Contains("denkimlang"))
                                {
                                    NeedToMove = "383,531,762";
                                }
                                else if (nhiemvu.Contains("phangiai"))
                                {
                                    PushMissions(MissionsType.PhanGiaiTrangBiPet);
                                }
                                else if (nhiemvu.Contains("trangsuc"))
                                {
                                    PushMissions(MissionsType.TrangSucCuuLe);
                                }
                                else if (nhiemvu.Contains("trungac"))
                                {
                                    PushMissions(MissionsType.TrungAc);
                                }
                                else if (nhiemvu.Contains("thankhi"))
                                {
                                    if(PacketItems.TrangBi.Where(p => Game.Weapon.Contains(p.TypeName) && (TDT.ParseAllInt(p.Star.ToString().Substring(0, 1)) >= 8 || TDT.ParseAllInt(p.Star.ToString()) >= 80)).FirstOrDefault() != null)
                                    {
                                        PushMissions(MissionsType.ThanKhi9Sao);
                                    }
                                }
                                else if (nhiemvu.Contains("cauthienthai"))
                                {
                                    PushMissions(MissionsType.DoiHuyenSacCauThienThai);
                                }
                                else if (nhiemvu.Contains("resettime"))
                                {
                                    LUA.DataPoolReConnect();
                                }
                                else if (nhiemvu.Contains("resetgio"))
                                {
                                    if (resettimeall.Elapsed.TotalSeconds >= 5)
                                    {
                                        resettimeall = Stopwatch.StartNew();
                                        foreach (var game in Main.Instance.AllOnelineGame)
                                        {
                                            if (game.TLBB.IsLeader)
                                            {
                                                game.IsLostLeader = true;
                                            }
                                            if (game.TLBB.PlayerState == 10)
                                                game.IsOpenShop = true;
                                            game.IsOpenPass2 = false;
                                            game.ReConnect();
                                        }
                                    }
                                }
                                else if (nhiemvu.Contains("train"))
                                {
                                    NeedToMove = ToaDoTrain;
                                }
                                else if (nhiemvu.Contains("tholinhchau"))
                                {
                                    if (swThoLinhChau.Elapsed.TotalSeconds > 65)
                                    {
                                        PushMissions(MissionsType.DungThoLinhChau);
                                        swThoLinhChau = Stopwatch.StartNew();
                                    }
                                }
                                else if (nhiemvu.Contains("phuvethanh"))
                                {
                                    foreach (Game game in Party)
                                    {
                                        if (game.TLBB.MapId > 2)
                                        {
                                            StopFollow();
                                            if (TLBB.IsRide)
                                                DownRide();
                                            if (game.PhuIndex(0) != -1)
                                            {
                                                if (game.TLBB.IsRide)
                                                {
                                                    game.DownRide();
                                                }
                                                game.PlayerPackageUseItem(PhuIndex(0));
                                            }
                                            else if (game.PhuIndex(1) != -1)
                                            {
                                                if (game.TLBB.IsRide)
                                                {
                                                    game.DownRide();
                                                }
                                                game.PlayerPackageUseItem(PhuIndex(1));
                                            }
                                            else if (game.PhuIndex(2) != -1)
                                            {
                                                if (game.TLBB.IsRide)
                                                {
                                                    game.DownRide();
                                                }
                                                game.PlayerPackageUseItem(PhuIndex(2));
                                            }
                                            else
                                            {
                                                foreach (Skill skill in Skills)
                                                {
                                                    if (skill.PacketId == 22)
                                                    {
                                                        uint delay = Memory.Read(TLBB.DelayBase + skill.DelayOffset);
                                                        if (delay == 0xFFFFFFFF || delay == 0)
                                                            UseSkill((int)skill.PacketId);
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else if (nhiemvu.Contains("hangngay"))
                                {
                                    if (swHangNgay == null || swHangNgay.Elapsed.TotalSeconds > 600)
                                    {
                                        swHangNgay = Stopwatch.StartNew();
                                        PushMissions(MissionsType.ThuTaiVanMay);
                                        PushMissions(MissionsType.LoLyHoa);
                                        foreach (var item in PacketItems.All)
                                        {
                                            if (item.ClearName == "nguyenlinhtuyen" && item.Count >= 5)
                                            {
                                                PushMissions(MissionsType.NguyenVongThienLinh);
                                                break;
                                            }
                                        }
                                    }
                                }
                                else if (nhiemvu.Contains("sumon"))
                                {
                                    PushMissions(MissionsType.NhiemVuSuMon);
                                }
                                else if (nhiemvu.Contains("luyenkimnhanh"))
                                {
                                    PushMissions(MissionsType.LuyenKimNhanh);
                                }
                                else if (nhiemvu.Contains("phithuy"))
                                {
                                    PushMissions(MissionsType.NhanPhiThuy);
                                }
                                else if (nhiemvu.Contains("luyenkim"))
                                {
                                    PushMissions(MissionsType.LuyenKim);
                                }
                                else if (nhiemvu.Contains("nhanbong"))
                                {
                                    PushMissions(MissionsType.NhanBong);
                                }
                                else if (nhiemvu.Contains("bando"))
                                {
                                    if (swBanDo == null || swBanDo.Elapsed.TotalMinutes > 10)
                                    {
                                        PushMissions(new[] { MissionsType.TriLieu, MissionsType.BanRac, MissionsType.CatVang, MissionsType.CatDo });
                                        swBanDo = Stopwatch.StartNew();
                                    }
                                }
                                else if (TLBB.IsLeader)
                                {
                                    if (Setting.Is("checkPhuBanDuNguoi") && Party.Count() < 6)
                                    {
                                        continue;
                                    }
                                    if (nhiemvu.Contains("actac"))
                                    {

                                        PushMissions(MissionsType.DatDoiAcTac);
                                        if (nhiemvu.Contains("thaiho"))
                                        {
                                            MapAcTac = MAP.ThaiHo;
                                        }
                                        else if (nhiemvu.Contains("tunson"))
                                        {
                                            MapAcTac = MAP.TungSon;
                                        }
                                        else if (nhiemvu.Contains("kiemcac"))
                                        {
                                            MapAcTac = MAP.KiemCac;
                                        }
                                        else if (nhiemvu.Contains("donhoang"))
                                        {
                                            MapAcTac = MAP.DonHoang;
                                        }
                                        else if (nhiemvu.Contains("kinhho"))
                                        {
                                            MapAcTac = MAP.KinhHo;
                                        }
                                        else if (nhiemvu.Contains("voluongson"))
                                        {
                                            MapAcTac = MAP.VoLuongSon;
                                        }
                                        else
                                        {
                                            if (MapAcTac == 0)
                                                RandomMapAcTac();
                                        }
                                    }
                                    else if (nhiemvu.Contains("acba"))
                                    {
                                        PushMissions(MissionsType.DatDoiAcBa);
                                    }
                                    else if (nhiemvu.Contains("tangkinhcac"))
                                    {
                                        PushMissions(MissionsType.DatDoiTangKinhCac);
                                        if (nhiemvu.Contains("tayho"))
                                        {
                                            MapTangKinhCac = MAP.TayHo;
                                        }
                                        else if (nhiemvu.Contains("nhihai"))
                                        {
                                            MapTangKinhCac = MAP.NhiHai;
                                        }
                                        else if (nhiemvu.Contains("nhannam"))
                                        {
                                            MapTangKinhCac = MAP.NhanNam;
                                        }
                                        else
                                        {
                                            if (MapTangKinhCac == 0)
                                                RandomMapTangKinhCac();
                                        }
                                    }
                                    else if (nhiemvu.Contains("datru"))
                                    {
                                        PushMissions(MissionsType.DatDoiDaTru);
                                    }
                                    else if (nhiemvu.Contains("mongheo"))
                                    {
                                        PushMissions(MissionsType.DatDoiMongHeo);
                                    }
                                    else if (nhiemvu.Contains("langmo"))
                                    {
                                        PushMissions(MissionsType.DatDoiPhungHoangLangMo);
                                    }
                                    else if (nhiemvu.Contains("kycuoc"))
                                    {
                                        PushMissions(MissionsType.DatDoiKyCuoc);
                                    }
                                    else if (nhiemvu.Contains("matac"))
                                    {
                                        PushMissions(MissionsType.DatDoiMaTac);
                                    }
                                    else if (nhiemvu.Contains("tambao"))
                                    {
                                        PushMissions(MissionsType.DatDoiLauLanTamBao);
                                    }
                                    else if (nhiemvu.Contains("huyetchien"))
                                    {
                                        PushMissions(MissionsType.DatDoiKhieuChienPhieuMieuPhong);
                                    }
                                    else if (nhiemvu.Contains("phieumieuphong"))
                                    {
                                        PushMissions(MissionsType.DatDoiPhieuMieuPhong);
                                    }
                                    else if (nhiemvu.Contains("phucdiakho"))
                                    {
                                        PushMissions(MissionsType.DatDoiPhucDia);
                                        PushMissions(MissionsType.PhucDiaKho);
                                    }
                                    else if (nhiemvu.Contains("phucdia"))
                                    {
                                        PushMissions(MissionsType.DatDoiPhucDia);
                                        RemoveMission(MissionsType.PhucDiaKho);
                                    }
                                    else if (nhiemvu.Contains("tochau"))
                                    {
                                        PushMissions(MissionsType.DatDoiQ123ToChau);
                                    }
                                    else if (nhiemvu.Contains("laulan"))
                                    {
                                        PushMissions(MissionsType.DatDoiQ123LauLan);
                                    }
                                    else if (nhiemvu.Contains("yentuo"))
                                    {
                                        PushMissions(MissionsType.DatDoiYenTuO);
                                    }
                                    else if (nhiemvu.Contains("kythu"))
                                    {
                                        PushMissions(MissionsType.DatDoiThienGiangKyThu);
                                    }
                                    else if (nhiemvu.Contains("tutuyettrang"))
                                    {
                                        PushMissions(MissionsType.DatDoiTuTuyetTrang);
                                    }
                                    else if (nhiemvu.Contains("sattinh"))
                                    {
                                        PushMissions(MissionsType.DatDoiSatTinh);
                                    }
                                    else if (nhiemvu.Contains("thieuthatson"))
                                    {
                                        PushMissions(MissionsType.DatDoiThieuThatSon);
                                    }
                                    else if (nhiemvu.Contains("tamthan"))
                                    {
                                        PushMissions(MissionsType.DatDoiTamThan);
                                    }
                                    else if (nhiemvu.Contains("vuonglang"))
                                    {
                                        PushMissions(MissionsType.DatDoiVuongLang);
                                    }
                                }
                                break;
                            }
                        }
                        //ListViewItem item = new ListViewItem();
                        //item.Text = n.Split('|')[0].Trim();
                        //item.SubItems.Add(n.Split('|')[1].Trim());
                    }
                }
            }
        }

        private Stopwatch swBanDo = null;

        private Stopwatch swHangNgay;

        public void FightPet(int idx, bool food = true)
        {
            if (TLBB.PlayerState == 7)
            {
                if (TLBB.Menpai == MENPAI.NgaMy)
                {
                    FixKetMap(1);
                }
                else
                    return;
            }
            if (DiaPhuNguYeu.Elapsed.TotalSeconds <= 3)
                return;
            swXuaPet = Stopwatch.StartNew();
            if (food)
            {
                DoStringEx("if Pet:IsPresent(" + idx + ") then Pet:Feed(" + idx + "); Pet:Dome(" + idx + ");  Pet:Go_Fight(" + idx + "); end");
            }
            else
            {
                DoStringEx("if Pet:IsPresent(" + idx + ") then Pet:Go_Fight(" + idx + ");  end");
            }
        }


        public void MoShopTrungDo()
        {
            if (!TLBB.IsMapBang(TLBB.MapId))
            {
                VaoBang();
            }
            else
            {
                if (GoToEx(BANG.VanLinhThuong))
                {
                    if (TLBB.IsQuestOpen)
                    {
                        if (QuestFrame.Click("#{LDSD_200805_05}"))
                            RemoveMission(MissionsType.MoShopTrungDo);
                        QuestFrame.Close();
                    }
                    else
                    {
                        Talk(BANG.VanLinhThuong.Id);
                    }
                }
            }
        }

        public void MoShopHungBa()
        {
            if (!TLBB.IsMapBang(TLBB.MapId))
            {
                VaoBang();
            }
            else
            {
                if (GoToEx(BANG.TrieuTuHuan))
                {
                    if (TLBB.IsQuestOpen)
                    {
                        if (QuestFrame.Click("#{JWSD_140821_3}"))
                            RemoveMission(MissionsType.MoShopHungBa);
                        QuestFrame.Close();
                    }
                    else
                    {
                        Talk(BANG.TrieuTuHuan.Id);
                    }
                }
            }
        }

        public void MoBachBaoCac()
        {
            if (!TLBB.IsMapBang(TLBB.MapId))
            {
                VaoBang();
            }
            else
            {
                if (GoToEx(BANG.VanTangBao))
                {
                    if (TLBB.IsQuestOpen)
                    {
                        if (QuestFrame.Click("#{BHSC_140509_04}"))
                            RemoveMission(MissionsType.MoShopBachBaoCac);
                        QuestFrame.Close();
                    }
                    else
                    {
                        Talk(BANG.VanTangBao.Id);
                    }
                }
            }
        }


        private void MoTiemThuoc()
        {
            if (GoToEx(LACDUONG.BachManhSinh))
            {
                Talk(LACDUONG.BachManhSinh);
                RemoveMission(MissionsType.MoTiemThuoc);
            }
        }

        private void MoShopQuyThi()
        {
            if (GoToEx("196,161,738"))
            {
                Talk(4);
                Thread.Sleep(500);
                QuestFrame.Click("#{GSHB_190801_47}");
                RemoveMission(MissionsType.MoShopQuyThi);
            }
        }

        private void AutoHideGame()
        {
            if (SecCount % 60 == 0)
            {
                Handle = Win.GetHandle(ProcessId, "TianLongBaBu WndClass");
            }
            if (IsOneSec)
            {
                if (Handle != IntPtr.Zero)
                {
                    Win.RECT rect;
                    if (!Win.IsWindowMini(Handle))
                    {
                        Win.GetWindowRect(Handle, out rect);
                        if (rect.Top > 0)
                            RECT = rect;
                    }
                }
                if (Win.IsShow(Handle))
                {
                    if (IsActiveByUser || CurGomDo == this)
                    {
                    }
                    else
                    {
                        if (Setting.Is("checkLuonAnGame"))
                        {
                            if (CurGomDo == this || CurGomDoThuong == this)
                            {
                            }
                            else
                            {
                                if (ShowTime.Elapsed.TotalSeconds >= 3)
                                {
                                    if (!Global.Paused)
                                        Hide();
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (ActiveTime.Elapsed.TotalSeconds > 10)
                        IsActiveByUser = false;
                }

                if (TLBB.MapId == MAP.TangKinhCac && TLBB.IsLeader)
                {
                }
                else
                {
                    if (IsHideAgain && ShowTime.Elapsed.TotalSeconds >= 3)
                    {
                        Hide();
                        IsHideAgain = false;
                    }
                }
            }
        }


        private void XuatPet()
        {
            if (swXuattieudattai.Elapsed.TotalSeconds < 5)
                return;
            if (TLBB.MapId == MAP.TienTrang)
                return;
            if (NeBayTime.Elapsed.TotalSeconds < 15)
                return;
            if (SecCount % 2 == 0 && TLBB.PlayerState != 5 && !TLBB.IsRide && TLBB.PetHP == 0 && TLBB.Online)
            {
                if (IsXuatPet && !PetId.Contains("00000000") && PetId != "")
                {
                    uint PetBase = Memory.Read(Address.PetBase);
                    int cnt = -1;
                    for (uint i = 0; i < 10; i++)
                    {
                        cnt++;
                        uint id = Memory.Read(PetBase + Address.PetDataSize * i + Address.PetId);
                        uint dome = Memory.Read(PetBase + Address.PetDataSize * i + Address.PetEnjoy);
                        uint lvl = Memory.Read(PetBase + Address.PetDataSize * i + Address.PetLvl);
                        if (lvl < Global.PetLvl)
                        {
                            if (id.ToString("X8") == PetId)
                            {
                                if (dome < 60)
                                {
                                    if (TLBB.HaveFood)
                                    {
                                        FightPet(cnt);
                                        break;
                                    }
                                }
                                else
                                {
                                    FightPet(cnt, false);
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (Global.IsMaxPet || Missions.Contains(MissionsType.NhiemVuThangCap) && !TLBB.IsBienThan)
                    {
                        if (TLBB.MapId == MAP.YenTuO && CharX < 100 && CharY < 100)
                        {
                        }
                        if (TLBB.MapId == MAP.ThieuThatSon && CharX > 170 && CharY > 65 && CharX < 215 && CharY < 110)
                        {
                        }
                        else if (TLBB.MapId == MAP.LoiDaiSinhTu && Objects.HaveMonter("Lư Quân Dật"))
                        {
                        }
                        else
                        {
                            if (TLBB.PetCount > 0)
                            {
                                if (TLBB.HaveFood)
                                {
                                    if (TLBB.MaxPetDome != null)
                                    {
                                        if (TLBB.MaxPetDome.PetDome < 60)
                                            FightPet((int)TLBB.MaxPetDome.idx);
                                        else
                                            FightPet((int)TLBB.MaxPetDome.idx, false);
                                    }
                                }
                                else
                                {
                                    if (TLBB.MaxPet != null)
                                    {
                                        FightPet((int)TLBB.MaxPet.idx, false);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void PetCongDiemCuongLuc()
        {
            DoStringEx("setmetatable(_G, {__index = Pet_Env }); PETNUM = -1; PETPOINT = -1; for i=1, 10 do if Pet:IsPresent(i-1) and Pet:GetIsFighting(i-1) then PETNUM = i -1; strName = Pet:GetPotential(i -1); PETPOINT = tonumber(strName); end end if PETPOINT ~= -1 then Pet:Add_Attribute(PETNUM ,PETPOINT ,0, 0, 0, 0); end");
        }

        public void PetCongDiemNoiLuc()
        {
            DoStringEx("setmetatable(_G, {__index = Pet_Env }); PETNUM = -1; PETPOINT = -1; for i=1, 10 do if Pet:IsPresent(i-1) and Pet:GetIsFighting(i-1) then PETNUM = i -1; strName = Pet:GetPotential(i -1); PETPOINT = tonumber(strName); end end if PETPOINT ~= -1 then Pet:Add_Attribute(PETNUM ,0 , PETPOINT, 0, 0, 0); end");
        }

        public void PetCongDiemTheLuc()
        {
            DoStringEx("setmetatable(_G, {__index = Pet_Env }); PETNUM = -1; PETPOINT = -1; for i=1, 10 do if Pet:IsPresent(i-1) and Pet:GetIsFighting(i-1) then PETNUM = i -1; strName = Pet:GetPotential(i -1); PETPOINT = tonumber(strName); end end if PETPOINT ~= -1 then Pet:Add_Attribute(PETNUM ,0 , 0, 0, PETPOINT, 0); end");
        }

        public void PetCongDiemThanPhap()
        {
            DoStringEx("setmetatable(_G, {__index = Pet_Env }); PETNUM = -1; PETPOINT = -1; for i=1, 10 do if Pet:IsPresent(i-1) and Pet:GetIsFighting(i-1) then PETNUM = i -1; strName = Pet:GetPotential(i -1); PETPOINT = tonumber(strName); end end if PETPOINT ~= -1 then Pet:Add_Attribute(PETNUM ,0 , 0, PETPOINT, 0, 0); end");
        }

        public void CongDiemPet()
        {
            if (!Setting.Is("checkCongDiemPet"))
                return;
            Pet petfight = TLBB.Pets.Where(pet => pet.IsFight).FirstOrDefault();
            if (petfight != null)
            {
                if (petfight.IsNoiCong)
                    PetCongDiemNoiLuc();
                else if (petfight.IsNgoaiCong)
                    PetCongDiemCuongLuc();
                else if (petfight.IsTheLuc)
                    PetCongDiemTheLuc();
                else if (petfight.IsThanPhap)
                    PetCongDiemThanPhap();
            }
        }

        private Stopwatch swXuaPet = Stopwatch.StartNew();

        private bool AutoComBack()
        {
            if (IsDead && IsAutoComeBack)
            {
                if (TLBB.PlayerState == 0)
                {
                    if (DeadX == 0)
                    {
                        IsDead = false;
                    }
                    else
                    {
                        if (TLBB.MapId != DeadMap)
                        {
                            if (PhuIndex(DeadFakeMap) != -1)
                            {
                                PlayerPackageUseItem(PhuIndex(DeadFakeMap));
                                PostMessage(22, 105);
                            }
                            else
                            {
                                GoToEx(DeadX, DeadY, DeadMap);
                            }
                        }
                        else
                        {
                            if (IsHoldPK)
                            {
                                if (TLBB.IsRide)
                                {
                                    DownRide();
                                }
                                else
                                {
                                    IsDead = false;
                                }
                            }
                            else
                            {
                                if (GoToEx(DeadX, DeadY, DeadMap))
                                {
                                    if (TLBB.IsRide)
                                    {
                                        DownRide();
                                    }
                                    else
                                    {
                                        IsDead = false;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return IsDead;
        }

        public void TrimProc()
        {
            if (TrimTime.Elapsed.TotalMinutes > 3 && Main.TrimRam)
            {
                TrimTime = Stopwatch.StartNew();
                Memory.TrimMem();
            }
        }

        public int CurTab { get; set; }
        public int NPCID = 191;

        public bool IsChangeTab { get; set; }

        public bool IsHideAgain { get; set; }

        public void BuyKNB()
        {
            if (!IsOneSec)
                return;
            if (CurTab < 3)
            {
                if (!IsChangeTab)
                {
                    DoStringEx("setmetatable(_G, {__index = CommisionStall_Env}); CommisionStall_ChangeTabIndex(" + (CurTab + 1) + ");");
                    IsChangeTab = true;
                }
                else
                {
                    DoStringEx("CurTab = " + CurTab + ";" + "NPCID = " + NPCID + ";" + "for i = 1,20 do local theAction = CommisionShop:EnumAction(i-1); if theAction and theAction:GetID() ~= 0 then local price = CommisionShop:EnumItem(0, i - 1,'price'); if CurTab == 1 then price = (price / 4) end if CurTab == 2 then price = (price / 10) end if price <= PRICE then local Sailer = CommisionShop:EnumItem(0, i - 1, 'serial'); Clear_XSCRIPT(); Set_XSCRIPT_Function_Name('Buy'); Set_XSCRIPT_ScriptID(800116); Set_XSCRIPT_Parameter(0,tonumber(NPCID)); Set_XSCRIPT_Parameter(1,tonumber(CurTab)); Set_XSCRIPT_Parameter(2,tonumber(Sailer)); Set_XSCRIPT_ParamCount(3); Send_XSCRIPT(); end end end");
                    IsChangeTab = false;
                    CurTab++;
                }
            }
            else
            {
                if (NPCID == 190)
                {
                    if (GoToEx(NPC.MID))
                    {
                        Talk(192);
                        if (TLBB.IsQuestOpen && QuestFrame.ID == 192)
                        {
                            QuestFrameOptionClicked(800116, 2);
                            NPCID = 192;
                            CurTab = 0;
                        }
                    }
                }
                else
                {
                    if (GoToEx(NPC.MID))
                    {
                        Talk(190);
                        if (TLBB.IsQuestOpen && QuestFrame.ID == 190)
                        {
                            QuestFrameOptionClicked(800116, 2);
                            NPCID = 190;
                            CurTab = 0;
                        }
                    }
                }
                IsChangeTab = true;
            }
            return;
        }


        public void Nhanx2()
        {
            if (GoToEx(LACDUONG.LuuKienMinh))
            {
                Talk(LACDUONG.LuuKienMinh);
                Thread.Sleep(500);
                if (TLBB.IsQuestOpen)
                {
                    QuestFrame.Click("#{SBSJ_100726_02}");
                    Thread.Sleep(500);
                    for (int i = 0; i < 4; i++)
                    {
                        Talk(LACDUONG.LuuKienMinh);
                        Thread.Sleep(500);
                        QuestFrame.Click("Một canh giờ gấp đôi kinh nghiệm");
                        Thread.Sleep(500);
                        QuestFrame.Click("Đúng vậy, ta muốn lĩnh giờ gấp đôi kinh nghiệm");
                        Thread.Sleep(500);
                    }
                    RemoveMission(MissionsType.NhanX2);
                }
            }
        }

        public void Dongx2()
        {
            if (GoToEx(LACDUONG.LuuKienMinh))
            {
                Talk(LACDUONG.LuuKienMinh);
                Thread.Sleep(1000);
                if (QuestFrame.Click("#{SBSJ_100726_01}"))
                    RemoveMission(MissionsType.DongX2);
            }
        }






        public List<IntPtr> listCheckedOnline = new List<IntPtr>();



        public void NhiemVuHangNgay()
        {
            if (TLBB.PlayerState != 0)
                return;
            StopFollow();
            if (TLBB.Lvl >= 20)
            {
                if (Missions.Contains(MissionsType.ThuTaiVanMay))
                {
                    if (GoToEx(TOCHAU.LieuNguyetHong))
                    {                      
                        Talk(TOCHAU.LieuNguyetHong);
                        Thread.Sleep(350);
                        if (QuestFrame.Click(808071, 111))
                        {
                            Thread.Sleep(1000);
                            if (QuestFrame.Text.Contains("Hôm nay các hạ đã tham gia sự kiện rút thăm Thử Tài Vận May rồi"))
                            {
                                PushDebugMessage("hoàn thành thử tài vận may");
                                RemoveMission(MissionsType.ThuTaiVanMay);
                                return;
                            }
                            else
                            {
                                for (int i = 0; i < 10; i++)
                                    DoStringEx("Clear_XSCRIPT(); Set_XSCRIPT_Function_Name('OnAugury'); Set_XSCRIPT_ScriptID(808071); Set_XSCRIPT_Parameter(0,0); Set_XSCRIPT_ParamCount(1); Send_XSCRIPT();");
                                Talk(TOCHAU.LieuNguyetHong);
                                Thread.Sleep(350);
                                QuestFrame.Click("Nhận thưởng Thử Tài Vận May");
                            }
                        }
                    }
                }
            }
            else
            {
                RemoveMission(MissionsType.ThuTaiVanMay);
            }
            if (Missions.Contains(MissionsType.ThuTaiVanMay))
                return;
            if (!IsOneSec)
                return;

            if (TLBB.Lvl >= 80)
            {
                if (Missions.Contains(MissionsType.LoLyHoa))
                {
                    if (GoToEx(TOCHAU.MoBach))
                    {
                        if (TLBB.SafeTime > 0)
                            return;

                        for(int i= 0;i < 5; i++)
                        {
                            DoStringEx(@"Clear_XSCRIPT();
		                                Set_XSCRIPT_Function_Name('YinHuo');
                                        Set_XSCRIPT_ScriptID(890174);
                                        Set_XSCRIPT_Parameter(0, tonumber(174));
                                        Set_XSCRIPT_Parameter(1, tonumber(1));
                                        Set_XSCRIPT_ParamCount(2);
                                        Send_XSCRIPT();

                                        Clear_XSCRIPT();
                                        Set_XSCRIPT_Function_Name('LianShi');
                                        Set_XSCRIPT_ScriptID(890174);
                                        Set_XSCRIPT_Parameter(0, 174);
                                        Set_XSCRIPT_ParamCount(1);
                                        Send_XSCRIPT(); ");
                        }

                        DoStringEx(@"Clear_XSCRIPT();
		                            Set_XSCRIPT_Function_Name('DuiHuanOK');
		                            Set_XSCRIPT_ScriptID(890174);
		                            Set_XSCRIPT_ParamCount(0);
	                                Send_XSCRIPT();");

                        //QuestFrame.SendClick()

                        //Talk(TOCHAU.MoBach);
                        //Thread.Sleep(1000);
                        //QuestFrame.Click("#{LHLL_130624_01}");
                        //Thread.Sleep(1000);
                        //LUA.TheFireStove_FireButton_OnClick();
                        //Thread.Sleep(1000);
                        //LUA.TheFireStove_StoneButton_OnClick();
                        //Thread.Sleep(1000);
                        //LUA.TheFireStove_MessageBox_OK_Clicked();
                        //Thread.Sleep(1000);
                        //LUA.TheFireStove_StoneButton_OnClick();

                        //for (int i = 0; i < 4; i++)
                        //{
                        //    Thread.Sleep(1000);
                        //    LUA.TheFireStove_FireButton_OnClick();
                        //    Thread.Sleep(1000);
                        //    LUA.TheFireStove_StoneButton_OnClick();
                        //}

                        //Thread.Sleep(1000);
                        //Talk(TOCHAU.MoBach);
                        //Thread.Sleep(1000);
                        //QuestFrame.Click("#{LHLL_130624_02}");
                        //Thread.Sleep(1000);
                        //DoStringEx("setmetatable(_G, {__index = TheFireStove_MessageBox2_Env}); if this:IsVisible() then  TheFireStove_MessageBox2_OK_Clicked(); end");

                        RemoveMission(MissionsType.LoLyHoa);
                    }
                }
            }
            else
            {
                RemoveMission(MissionsType.LoLyHoa);
            }
            if (Missions.Contains(MissionsType.LoLyHoa))
                return;

            if (Missions.Contains(MissionsType.NguyenVongThienLinh))
            {
                foreach (GameTask task in GameTask.Enum(this))
                {
                    if (task.Name == "#{SQXY_09061_4}")
                    {
                        if (task.Completed)
                        {
                            State = STATE.Done;
                        }
                    }
                }
                if (TLBB.PlayerState != 0)
                    return;
                if (!IsOneSec)
                    return;
                if (TLBB.IsQuestOpen)
                {
                    if (QuestFrame.All(this).Contains("#{SQXY_09061_7}"))
                    {
                        RemoveMission(MissionsType.NguyenVongThienLinh);
                        return;
                    }
                }

                if (State == STATE.None)
                {                   
                    if (!TLBB.IsTogleMission)
                    {
                        TogleMission();
                        Thread.Sleep(1000);
                    }
                    LUA.CLOSE_MISSION();
                    Thread.Sleep(1000);
                    DoStringEx("local cnt = 0; while true do local name, content = DataPool:GetPlayerMission_Memo(cnt); if string.find(name, '#{SQXY_09061_4}') then if DataPool:GetPlayerMission_Variable(cnt, 0) == 1 then return 'Xong'; else return content; end end cnt = cnt + 1; if cnt == 20 then return 'Chua'; end end");
                    LuaToString();
                    Thread.Sleep(1000);
                    string info = LuaToString();
                    if (info == "Xong")
                    {
                        State = STATE.Done;
                    }
                    else if (info == "Chua")
                    {
                        State = STATE.Null;
                    }
                    else
                    {
                        State = STATE.Do;
                    }
                    return;
                }
                if (State == STATE.Do)
                {
                    if (GoToEx(157, 185, 4))
                    {
                        if (TLBB.IsRide)
                        {
                            DownRide();
                            return;
                        }
                        foreach (var item in PacketItems.All)
                        {
                            if (item.Type == "TaskTools11_16")
                            {
                                PlayerPackageUseItem((int)item.Index);
                            }
                        }
                        if (IdleTime > 3)
                        {
                            State = STATE.None;
                        }
                    }
                    return;
                }
                if (State == STATE.Done || State == STATE.Null)
                {
                    if (GoToEx(TOCHAU.LuongDaoSi))
                    {
                        Talk(TOCHAU.LuongDaoSi);
                        Thread.Sleep(1000);
                        QuestFrame.Click("#{SQXY_09061_4}");
                        Thread.Sleep(1000);
                        if (State == STATE.Null)
                        {
                            LUA.QuestFrameAcceptClicked();
                        }
                        else
                        {
                            LUA.QuestFrameMissionContinue();
                            Thread.Sleep(1000);
                            LUA.QuestFrameMissionComplete();
                        }
                        State = STATE.None;
                    }
                    return;
                }
                State = STATE.None;
            }

            State = STATE.None;
        }

        public bool IsDungIm { get; set; }

        public void SellItem(int item)
        {
            PostMessage(item, 117);
        }

        public void NhatTuyet()
        {
            if (!IsNhatTuyet)
                return;
            if ((TLBB.IsONguyenLieuFull || TLBB.IsODaoCuFull))
                return;
            if (IsDungIm && Missions.Contains(MissionsType.BanRac) && TDT.GetDistance(CharX, CharY, SaveX, SaveY) > 3)
            {
                GoToEx(SaveX, SaveY, LACDUONG.Id);
            }
            if (TDT.GetDistance(CharX, CharY, SaveX, SaveY) <= 3)
            {
                RemoveMission(MissionsType.BanRac);
                SaveX = 0;
                SaveY = 0;
            }
            if (!IsDungIm && !IsMoveEx)
            {
                FixKetMap();
                return;
            }
            float minDistance = 1000;
            int id = -1;
            float x = 0;
            float y = 0;

            //if (TDT.VietLien(TLBB.MapName) != TDT.VietLien(TenThanhKT) && TLBB.MapId > 500)
            //{
            //    if (!(Setting.LoadSetting("ab")[0] > 0))
            //        return;
            //}

            foreach (GameObject _object in Objects.All)
            {
                if (_object.IsTaiNguyen)
                {
                    if (TDT.GetDistance(CharX, CharY, _object.X, _object.Y) < minDistance)
                    {
                        minDistance = TDT.GetDistance(CharX, CharY, _object.X, _object.Y);
                        id = (int)_object.Id;
                        x = _object.X;
                        y = _object.Y;
                    }
                }
            }
            if (IsDungIm && minDistance > 5)
                return;
            if (id != -1)
            {
                if (TDT.GetDistance(CharX, CharY, x, y) > 2 || TLBB.PlayerState == 5)
                {
                    LUA.Move(x, y);
                    return;
                }
                if (TLBB.IsRide)
                {
                    DownRide();
                    return;
                }
                PickItem(id);
                return;
            }
            if (!TLBB.IsRide && !IsDungIm && TLBB.HaveRide)
            {
                UpRide();
                return;
            }
            if (!IsDungIm)
                MoveNext();
        }

        public void KhaiKhoang()
        {
            if (!IsOneSec)
                return;
            if (TLBB.PlayerState == 8)
            {
                ListMoveEx.Clear();
                MoveExTime = new Stopwatch();
                return;
            }

            if (IsMapNghe())
            {
                if (!IsMoveEx)
                {
                    FixKetMap();
                    return;
                }
            }
            float minDistance = 1000;
            int id = -1;
            float x = 0;
            float y = 0;
            GameObject objHoa = null;
            foreach (GameObject _object in Objects.All)
            {
                if (BlackList.Contains(_object.Id))
                    continue;
                //if (_object.CleanName == "dongtrunghathao" || _object.CleanName == "nhansamthiennien")
                //    continue;
                if ((_object.IsKhoang && Missions.Contains(MissionsType.KhaiKhoang)) || (_object.IsDuoc && Missions.Contains(MissionsType.HaiDuoc)))
                {
                    //lecaotri
                    if (TDT.GetDistance(CharX, CharY, _object.X, _object.Y) < minDistance)
                    {
                        minDistance = TDT.GetDistance(CharX, CharY, _object.X, _object.Y);
                        id = (int)_object.Id;
                        x = _object.X;
                        y = _object.Y;
                        objHoa = _object;
                    }
                }
            }
            if (objHoa != null)
            {
                if (minDistance > 3)
                {
                    if (objHoa.Id != PickedId)
                    {
                        PickedId = objHoa.Id;
                        CareTime = Stopwatch.StartNew();
                    }
                    else
                    {
                        if (CareTime.Elapsed.TotalSeconds > 40)
                            BlackList.Add(PickedId);
                    }
                    Move(objHoa.X, objHoa.Y);
                }
                else
                {
                    if (TLBB.IsRide)
                        DownRide();
                    PickItem((int)objHoa.Id);
                }
                if (IdleTime > 4)
                    FixKetMap();
                return;
            }
            if (!TLBB.IsRide && TLBB.HaveRide)
            {
                UpRide();
                return;
            }
            if (IsMapNghe())
                MoveNext();
        }

        public bool IsMapNghe()
        {
            if (TLBB.MapId == MAP.KiemCac || TLBB.MapId == MAP.VoLuongSon || TLBB.MapId == MAP.DonHoang || TLBB.MapId == MAP.TungSon || TLBB.MapId == MAP.ThaiHo)
                return true;
            if (TLBB.MapId == MAP.TayHo || TLBB.MapId == MAP.NhiHai || TLBB.MapId == MAP.NhanNam || TLBB.MapId == MAP.LongTuyen || TLBB.MapId == MAP.ThuongSon || TLBB.MapId == MAP.NhanBac || TLBB.MapId == MAP.VoDi || TLBB.MapId == MAP.ThachLam || TLBB.MapId == MAP.NganNgaiTuyetNguyen || TLBB.MapId == MAP.ThaoNguyen)
                return true;
            return false;
        }

        public void OkPhu()
        {
            if (Address.GameType == 2)
            {
                if (Memory.Read(new uint[] { 0x63EFE8, 0x0, 0x0, 0xC, 0x64 }) == 1)
                {
                    DoStringEx("setmetatable(_G, {__index = Item_TuDunZhu_Env}); Item_TuDunZhu_OK_Clicked();");
                }
            }
            else
            {
                DoStringEx("setmetatable(_G, {__index = Item_TuDunZhu_Env}); if this:IsVisible() then Item_TuDunZhu_OK_Clicked(); end");
            }
        }

        public void DisableActiveGame()
        {
            if (Address.GameType == 1 || Address.GameType == 2)
            {
                byte[] buffer = new byte[14];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer[i] = 0x90;
                }

                Memory.WriteAdress(buffer, Address.DisableActiveGame, Process.Id);
            }
            else
            {
                byte[] buffer = new byte[15];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer[i] = 0x90;
                }
                Memory.WriteAdress(buffer, Address.DisableActiveGame, Process.Id);
            }
        }

        private Stopwatch RaoTime { get; set; }

        public void Rao()
        {
            if (!IsRao)
                return;

            //Cam rao lecaotri2020
            if ((Address.ControlEvent & 1) == 1)
            {
                return;
            }

            RaoTxt = RaoTxt.Replace("\"", "");
            if (RaoTime == null || RaoTime.Elapsed.TotalSeconds > TalkChannel.Time + 3)
            {
                RaoTime = Stopwatch.StartNew();
                if (Address.GameType == 1)
                {
                    string txt = ConverterEx.Unicode2VISCII(RaoTxt);
                    string msg = "";
                    foreach (char c in txt)
                    {
                        msg += ((int)c).ToString("X2");
                    }
                    //lecaotri2020
                    //            string near = "FC 59 94 00 00 00 19 00 00 00 00 00 FF FF FF FF 00 00 00 00 03 25 00 22" + (msg.Length / 2).ToString("X2") + "00 00 00" + msg;
                    //            string word = "FC 59 94 00 00 00 19 00 00 00 00 00 FF FF FF FF 02 00 00 00 03 25 00 22" + (msg.Length / 2).ToString("X2") + "00 00 00" + msg;
                    //00 C1 95 00 00 00 C5 7F 00 00 00 00 FF FF FF FF 00 00 00 00 AF 09 14 00 00 00 00 00 00 00 00 00
                    string near = "FC 59 94 00 00 00 19 00 00 00 00 00 FF FF FF FF 00 00 00 00 03 25 00 22" + (msg.Length / 2).ToString("X2") + "00 00 00" + msg;
                    string wolrd = "FC 59 94 00 00 00 19 00 00 00 00 00 FF FF FF FF 02 00 00 00 03 25 00 22" + (msg.Length / 2).ToString("X2") + "00 00 00" + msg;
                    if (TalkChannel.IsNear)
                    {
                        LUA.Talk("near", RaoTxt);
                    }
                    if (TalkChannel.IsScene)
                    {
                        if (TalkChannel.IsR)
                            SendPacket(wolrd);
                        else
                            LUA.Talk("scene", RaoTxt);
                    }
                    if (TalkChannel.IsIPRegion)
                    {
                        LUA.Talk("ipregion", RaoTxt);
                    }
                    if (TalkChannel.IsGuildLeague)
                    {
                        LUA.Talk("guild_league", RaoTxt);
                    }
                    if (TalkChannel.IsGuild)
                    {
                        LUA.Talk("guild", RaoTxt);
                    }
                    if (TalkChannel.IsMenpai)
                    {
                        LUA.Talk("menpai", RaoTxt);
                    }
                    if (TalkChannel.IsTeam)
                    {
                        LUA.Talk("team", RaoTxt);
                    }
                    if (TalkChannel.IsBigWorld)
                    {
                        LUA.Talk("bigworld", RaoTxt);
                    }
                }
                else
                {
                    if (RaoTxt.Trim() != "")
                    {
                        if (TalkChannel.IsNear)
                        {
                            LUA.Talk("near", RaoTxt);
                        }
                        if (TalkChannel.IsScene)
                        {
                            LUA.Talk("scene", RaoTxt);
                        }
                        if (TalkChannel.IsIPRegion)
                        {
                            LUA.Talk("ipregion", RaoTxt);
                        }
                        if (TalkChannel.IsGuildLeague)
                        {
                            LUA.Talk("guild_league", RaoTxt);
                        }
                        if (TalkChannel.IsGuild)
                        {
                            LUA.Talk("guild", RaoTxt);
                        }
                        if (TalkChannel.IsMenpai)
                        {
                            LUA.Talk("menpai", RaoTxt);
                        }
                        if (TalkChannel.IsTeam)
                        {
                            LUA.Talk("team", RaoTxt);
                        }
                        if (TalkChannel.IsBigWorld)
                        {
                            LUA.Talk("bigworld", RaoTxt);
                        }
                    }
                    PostMessage(3, 105);
                }
            }
        }

        public void Dungx2()
        {
            if (!Setting.Is("checkDungx2"))
                return;
            if (SecCount % 30 == 0)
            {
                DoStringEx("setmetatable(_G, {__index = MainMenuBar_Env}); if MainMenuBar_EXP3:IsVisible() then return '1'; else return '0'; end");
                Thread.Sleep(350);
                string x2 = LuaToString();
                if (x2 == "0")
                {
                    foreach (var item in PacketItems.All)
                    {
                        if (item.Type == "Cloth3_1")
                        {
                            item.Use();
                            break;
                        }
                    }
                }
            }
        }

        public void Muax2()
        {
            if (!Setting.Is("checkMuax2"))
                return;
            if (SecCount % 30 == 0)
            {
                bool ishave = false;
                foreach (var item in PacketItems.All)
                {
                    if (item.Type == "Cloth3_1")
                    {
                        ishave = true;
                        break;
                    }
                }
                if (!ishave)
                {
                    ShowPacket();
                }
            }
        }



        public static List<uint> listKTT = new List<uint>();

        public bool IsCollect()
        {
            if (IsMapPhuBan())
                return true;
            if (Missions.Contains(MissionsType.LuyenKim))
                return true;
            if (Missions.Contains(MissionsType.TuBaoBon))
                return true;
            if (Missions.Contains(MissionsType.TuDuongCon))
                return true;
            if (Missions.Contains(MissionsType.LongPhuMau))
                return true;
            if (Missions.Contains(MissionsType.TrungAc))
                return true;
            if (Missions.Contains(MissionsType.NhiemVuChinhTuyen))
                return true;
            if (Missions.Contains(MissionsType.BachHoaDuyen))
                return true;
            if (Missions.Contains(MissionsType.KhaiKhoang))
                return true;
            if (Missions.Contains(MissionsType.HaiDuoc))
                return true;
            if (Missions.Contains(MissionsType.NhiemVuSuMon))
                return true;
            if (IsDuaHau || IsThuHoach || IsNhatTuyet)
                return true;
            if (IsNhatHop)
                return true;
            if (Missions.Contains(MissionsType.NhiemVuThangCap))
                return true;
            if (IsPickItem || IsPickEx || Global.IsPickItem)
                return true;
            return false;
        }

        public void CollectItem()
        {
            //if (TLBB.MapId == MAP.YenTuO || TLBB.MapId == MAP.ThieuThatSon || TLBB.MapId == MAP.PhieuMieuPhong || TLBB.MapId == MAP.TamThanHuyenCanh || TLBB.MapId == MAP.BinhThanhKyTran || TLBB.MapId == MAP.PhungMinhVuongLang || TLBB.MapId == MAP.TuTuyetTrang || TLBB.MapId == MAP.LoiDaiSinhTu || TLBB.MapId == MAP.LanHoanPhucDia)
            //{
            //    if (!TLBB.IsODaoCuFull && !TLBB.IsONguyenLieuFull)
            //        ListPickedIds.Clear();
            //}
            if (ListPickedIds.Count > 10)
                ListPickedIds.RemoveAt(0);
            uint lootpacketId = Memory.Read(Address.LootPacketId);
            if (!ListPickedIds.Contains((int)lootpacketId))
            {
                ListPickedIds.Add((int)lootpacketId);
            }
            else
            {
                return;
            }
            if (IsNhatHop)
            {
                foreach (var item in PacketItems.LootPacket)
                {
                    if (item.Type == "Tattoo_1")
                    {
                        if (!listKTT.Contains(lootpacketId))
                            listKTT.Add(lootpacketId);
                    }
                }
            }

            if (!IsCollect())
                return;
            uint lootItem = Memory.ReadAddress(Address.LootPacketItem);
            for (uint i = 0; i < 10; i++)
            {
                uint lootId = i + 1;

                uint _item = Memory.Read(lootItem + i * 4);
                uint _itemDefineId = Memory.Read(_item + 0x8);

                if (_itemDefineId != 0 && lootId >= 5)
                {
                    ListPickedIds.Remove((int)lootpacketId);
                    PickAll();
                    return;
                }

                if (IsFilter)
                {
                    if (IsVipItemEx(_item))
                    {
                        DoStringEx("setmetatable(_G, {__index = LootPacket_Env}); LootPacket_Clicked(" + lootId + ")  ");
                    }
                }
                else
                {
                    if (Missions.Contains(MissionsType.NhiemVuChinhTuyen) || TLBB.MapId == MAP.TangKinhCac || TLBB.MapId == MAP.YenTuO || TLBB.MapId == MAP.ThieuThatSon || TLBB.MapId == MAP.PhieuMieuPhong || TLBB.MapId == MAP.TamThanHuyenCanh || TLBB.MapId == MAP.BinhThanhKyTran || TLBB.MapId == MAP.PhungMinhVuongLang || _item != 0 && (IsDuaHau || Missions.Contains(MissionsType.BachHoaDuyen) || Missions.Contains(MissionsType.LongPhuMau) || IsNhatTuyet || IsNhatHop || Missions.Contains(MissionsType.KhaiKhoang) || Missions.Contains(MissionsType.HaiDuoc) || IsThuHoach || (!Global.ItemFillter && !IsFilter)))
                    {
                        //if (!IsOptLocDo) {
                        //    PickAll();
                        //    break;
                        //}
                        PickAll();
                        break;
                    }
                    //_packet packetitem = new _packet(_item);
                    //if(HaveToDropName.Contains(packetitem.Name) || HaveToDropType.Contains(_item.t)
                    if (IsVIPItem((int)_itemDefineId))
                    {
                        if (_item != 0)
                        {
                            //PostMessageNew(_item, 107);
                            DoStringEx("setmetatable(_G, {__index = LootPacket_Env}); LootPacket_Clicked(" + lootId + ") ");
                        }
                    }
                }
            }
        }

        private bool IsVipItemEx(uint _item)
        {
            bool IsVip = false;
            uint Class = 0;
            string Name = "", TypeName = "", Type = "";
            Class = Memory.Read(_item);
            if (Class == Address.PacketType1 || Class == Address.PacketType5)
            {
                Name = Memory.ReadString(Memory.Read(_item + 0x28, 0x28));
                TypeName = Memory.ReadString(Memory.Read(_item + 0x28, 0x58));
                Type = Memory.ReadString(Memory.Read(_item + 0x28, 0x54));
            }
            else if (Class == Address.PacketType2)
            {
                Name = Memory.ReadString(Memory.Read(_item + 0x28, 0x18));
                TypeName = Memory.ReadString(Memory.Read(_item + 0x28, 0x50));
                Type = Memory.ReadString(Memory.Read(_item + 0x28, 0x14));
            }
            else if (Class == Address.PacketType3)
            {
                Name = Memory.ReadString(Memory.Read(_item + 0x28, 0x1C));
                TypeName = Memory.ReadString(Memory.Read(_item + 0x28, 0x130));
                Type = Memory.ReadString(Memory.Read(_item + 0x28, 0x14));
            }
            else if (Class == Address.PacketType4)
            {
                Name = Memory.ReadString(Memory.Read(_item + 0x28, 0x28));
                TypeName = Memory.ReadString(Memory.Read(_item + 0x28, 0x4C));//48 lecaotriloi
                Type = Memory.ReadString(Memory.Read(_item + 0x28, 0x14));
            }
            else
            {
                Name = Memory.ReadString(Memory.Read(_item + 0x28, 0x58));
                TypeName = Memory.ReadString(Memory.Read(_item + 0x28, 0x50));
                Type = Memory.ReadString(Memory.Read(_item + 0x28, 0x14));
            }
            if (Class == Address.PacketType6)
            {
                Name = Memory.ReadString(Memory.Read(_item + 0x28, 0x2C));
                TypeName = Memory.ReadString(Memory.Read(_item + 0x28, 0x68));
                Type = Memory.ReadString(Memory.Read(_item + 0x28, 0x28));
            }

            foreach (string name in ChiNhat.Split('\n'))
            {
                if (TDT.VietLien(name).Contains(TDT.VietLien(Name)))
                {
                    if (_item != 0)
                        IsVip = true;
                }
            }

            return IsVip;
        }

        public static bool IsVIPItem(int item)
        {
            if (item == 0)
                return false;
            if (item == 40004430) // duy tu moc tai
                return false;
            //if (item == 40005066) // hoa diem hoa
            //    return true;
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
            if (GAMEDIC.ThuCuoi80.ContainsKey(item))
                return true;
            if (item == 10157001 || item == 10157002) //Long van
                return true;
            if (item == 10156001 || item == 10156002 || item == 10156003 || item == 10156004) // võ hồn
                return true;
            if (item == 10141030 || item == 10141040)
                return true;
            if (item.ToString().StartsWith("30120")) // điêu văn đồ tường
                return true;
            if (item.ToString().StartsWith("10124")) // thời trang
                return true;
            //if (item == 20502009) // hoang chi
            //    return false;
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

        private List<int> ListPickedIds = new List<int>();


        public bool PickItem()
        {
            if (TLBB.MapId == MAP.TangKinhCac && TDT.GetDistance(CharX, CharY, 64, 28) > 15)
                return false;
            if (TLBB.MapId == MAP.YenTuO || TLBB.MapId == MAP.ThieuThatSon || TLBB.MapId == MAP.PhieuMieuPhong || TLBB.MapId == MAP.TamThanHuyenCanh || TLBB.MapId == MAP.BinhThanhKyTran || TLBB.MapId == MAP.PhungMinhVuongLang || TLBB.MapId == MAP.TuTuyetTrang || TLBB.MapId == MAP.LoiDaiSinhTu || TLBB.MapId == MAP.LanHoanPhucDia)
            {
                if (!TLBB.IsODaoCuFull && !TLBB.IsONguyenLieuFull)
                    ListPickedIds.Clear();
            }
            if (TLBB.MapId == MAP.TuTuyetTrang)
            {
                if (NeBayTime.Elapsed.TotalSeconds < 3)
                    return false;
            }
            if (Objects.LootPackets.Count() == 0 && Objects.All.Where(o => o.IsTaiNguyenEx).Count() == 0)
            {
                if (TLBB.MapId != MAP.HuyetMo)
                    ListPickedIds.Clear();
            }
            GameObject pickObject = Objects.LootPackets.Where(_object => !ListPickedIds.Contains((int)_object.Id)).OrderBy(o => o.Distance).FirstOrDefault();
            if (pickObject == null)
                return false;

            if (Missions.Contains(MissionsType.NhiemVuSuMon) || Missions.Contains(MissionsType.BachHoaDuyen))
                return false;
       
            if (TLBB.Busy && TLBB.PlayerState != 7)
                return false;
            if (Global.IsPickItem || IsPickItem || IsPickEx || IsDuaHau)//lecaotri
            {
                if (pickObject != null)
                {
                    if (IsMapPhuBan() && TLBB.MapId != MAP.PhungHoangCoThanhPhuBan)
                        StopFollow();
                    if (PickId != pickObject.Id)
                    {
                        PickId = (int)pickObject.Id;
                        CareTime = Stopwatch.StartNew();
                    }
                    CareObject = pickObject;
                    PickItem((int)pickObject.Id);
                    Thread.Sleep(100);
                    if (GoToEx(pickObject, false, 999))
                    {
                        if (TrueStandTime.Elapsed.TotalSeconds >= 4)
                        {
                            FixKetMap();
                        }
                    }

                    return true;
                }
                else
                {
                    PickId = -1;
                    CareTime = Stopwatch.StartNew();
                }
            }
            return false;
        }

        public bool ForcePickItem()
        {
            if (TLBB.PlayerState == 5)
                return false;
            GameObject pickObject = Objects.LootPackets.OrderBy(o => o.Distance).Where(_object => !ListPickedIds.Contains((int)_object.Id)).FirstOrDefault();
            if (pickObject != null)
            {
                if (IsMapPhuBan() && TLBB.MapId != MAP.PhungHoangCoThanhPhuBan)
                    StopFollow();
                if (PickId != pickObject.Id)
                {
                    PickId = (int)pickObject.Id;
                    CareTime = Stopwatch.StartNew();
                }
                else
                {
                    //if (CareTime.Elapsed.TotalSeconds >= 999)
                    //{
                    //    ListPickedIds.Add(PickId);
                    //}
                }
                CareObject = pickObject;
                PickItem((int)pickObject.Id);
                Thread.Sleep(100);
                if (GoToEx(pickObject, false, 999))
                {
                    if (TrueStandTime.Elapsed.TotalSeconds >= 4)
                    {
                        FixKetMap();
                    }
                }

                return true;
            }
            else
            {
                PickId = -1;
                CareTime = Stopwatch.StartNew();
            }
            return false;
        }

        private Stopwatch CareTime = Stopwatch.StartNew();
        private uint PickedId = 0xFFFFFFFF;
        private List<uint> BlackList = new List<uint>();
        public Stopwatch PickTime { get; set; } = Stopwatch.StartNew();

        public void PickItem(int id)
        {
            PickTime = Stopwatch.StartNew();
            PostMessage(id, 106);
        }

        public int PickId
        {
            get;
            set;
        }

        public void AnDon()
        {
            foreach (Skill skill in Skills)
            {
                if (skill.PacketId == 248)
                {
                    uint delay = Memory.Read(TLBB.DelayBase + skill.DelayOffset);
                    if (delay == 0 || delay == 0xFFFFFFFF)
                        UseSkill((int)skill.PacketId);
                    break;
                }
            }
        }

        private void PickAll()
        {
            DoStringEx("setmetatable(_G, {__index = LootPacket_Env}); LootPacket_Collect_Clicked();  ");
        }

        public void Hide()
        {
            Task.Run(() =>
            {
                Handle = Win.GetHandle(ProcessId, "TianLongBaBu WndClass");
                if (Handle == IntPtr.Zero)
                    Handle = Win.GetHandle(ProcessId, "kezgxubdpydrpeb");
                Win.HidePos(Handle);
            });
        }


        public bool IsActiveByUser { get; set; }
        Stopwatch ActiveTime = Stopwatch.StartNew();
        public void Active()
        {
            ActiveTime = Stopwatch.StartNew();
            IsActiveByUser = true;
            Task.Run(() =>
            {
                Handle = Win.GetHandle(ProcessId, "TianLongBaBu WndClass");
                if (Handle == IntPtr.Zero)
                    Handle = Win.GetHandle(ProcessId, "kezgxubdpydrpeb");
                Win.Active(this.Handle);                
            });
        }

        public void Exit(bool push = true)
        {
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                try
                {
                    Process.GetProcessById(ProcessId).Kill();
                }
                catch { }
            }).Start();

            try
            {
                if (alarmDead != null && alarmDead.Visible)
                    alarmDead.Dispose();
                if (alarmHP != null && alarmHP.Visible)
                    alarmHP.Dispose();
                if (alarmPk != null && alarmPk.Visible)
                    alarmPk.Dispose();
                if (alarmDisconnected != null && alarmDisconnected.Visible)
                    alarmDisconnected.Dispose();
                if (alarmCobanFail != null && alarmCobanFail.Visible)
                    alarmCobanFail.Dispose();

                if (LastName != "ĐăngNhập")
                {
                    if (IsXongBHD)
                    {
                        //Main.AddLog(DateTime.Now.ToString("HH:mm dd-MM") + " " + LastName + " xong BHD");
                    }
                    else
                    {
                        if (push)
                        {
                            if (LastName != null)
                            {
                                Main.PushLog(DateTime.Now.ToString("HH:mm dd-MM ") + "Thoát " + LastName);
                                LastName = null;
                            }
                        }
                    }
                }
            }
            catch { }
        }

        public void DemMam()
        {
            if (isCountMam)
                return;
            if (TLBB.Online)
            {
                isCountMam = true;
                var tainguyen = TLBB.CountTaiNguyen();

                Main.Instance.Invoke(() =>
                {
                    try
                    {
                        if (AccountEx != null)
                        {
                            AccountEx.CountMam = tainguyen[0].ToString();
                            AccountEx.CountShit = tainguyen[1].ToString();
                            AccountEx.CountBut = tainguyen[2].ToString();
                        }
                    }
                    catch { }
                });
            }
        }

        public void Quit()
        {
            IsXongBHD = false;
            Task.Run(() =>
            {
                try
                {
                    try
                    {
                        DemMam();
                    }
                    catch { }
                    if (TLBB.Online)
                    {
                        LUA.AskRet2SelServer();
                    }
                    else if (TLBB.IsLogon)
                        LUA.LogOn_ExitToSelectServer();
                    else if (TLBB.IsSelectRole)
                    {
                        LUA.LoginOverTime();
                        Thread.Sleep(1000);
                        LUA.LogOn_ExitToSelectServer();
                    }
                    setSafeTime = false;

                    IsWriteKhongChiemMan = false;


                    IsInit = false;
                    swStandTime = Stopwatch.StartNew();

                    IsSelectLogin = false;

                    LUA.DoString("COUNT = nil;");
                    SaveSetting();
                }
                catch { }
            });
        }

        public GameObject BestTarget { get; set; }





        public bool SimpleAttack((int, int, int) point)
        {
            if (!IsAuto)
                return false;
            if (TLBB.PlayerState != 0)
                return false;
            GameObject bestTarget = Objects.Monters.Where(o => point.Item1 == 0 || o.GetDistance(point.Item1, point.Item2) <= point.Item3).OrderBy(o => o.Distance).ThenBy(o => o.HP).FirstOrDefault();

            if (bestTarget != null)
            {
                SelectTarget(bestTarget.Id);
                SendKey(Global.BaseSkill);
                DownRide();
                return true;
            }
            return false;
        }

        public void ForceAttack(bool withIsAuto = false)
        {
            if (Leader != null)
            {
                if (Leader.Missions.Contains(MissionsType.DatDoiQ123ToChau) || Leader.Missions.Contains(MissionsType.DatDoiQ123LauLan))
                {
                    if (TLBB.MapId == MAP.ViemMaSon || TLBB.MapId == MAP.TamTaiHiepCoc)
                    {
                        return;
                    }
                }
            }
            if (withIsAuto)
            {
                if (!IsAuto)
                    return;
            }
            if (swXuaPet.Elapsed.TotalSeconds < 5)
                return;
            if (TLBB.IsNoiCong && TLBB.PlayerState == 2 && !Missions.Contains(MissionsType.ThanKhi9Sao))
                return;
            if (TLBB.IsFollow || TLBB.IsRide)
                return;
            if (Global.AtkFollowKey && Objects.Key != null && !TLBB.IsLeader)
            {
                if (IsOneSec)
                {
                    if (Objects.Key.State == 7)
                    {
                        if (Objects.Self.AtkToId != Objects.Key.AtkToId || Objects.Self.State != 7)
                        {
                            SelectTarget(Objects.Key.AtkToId);

                            SendKey(Global.BaseSkill);
                        }
                    }
                }
            }
            else if (!Global.Paused)
            {
                if (Objects.Target == null || Objects.Target.HP <= 0 || !Objects.TargetIsMine || swStandTime.Elapsed.TotalSeconds > 0.4 || IsLureEx)
                {
                    GetBestTarget();
                    if (BestTarget != null)
                    {
                        if (TLBB.PlayerState == 7 && BestTarget.Id == TargetId)
                        {

                        }
                        else
                        {
                            SelectTarget(BestTarget.Id);
                            SendKey(Global.BaseSkill);
                        }
                    }
                }
            }
        }

        public bool IsOnlyAttack { get; set; }


        public uint SelfId { get; set; } = 0xFFFFFFFF;

        public void PhanGiaiTrangBiPet()
        {
            if (GoToEx(TOCHAU.VanSanSan))
            {
                if (Setting.Is("checkKhiPhanGiai"))
                {
                    PacketItems.ThienCo.Where(i => GAMEDIC.TrangBiTranThu75.Contains(i.Name.Trim())).ForEach(i => GetItemThienCo(i.Index));
                    Thread.Sleep(350);
                }
                foreach (var item in PacketItems.All)
                {
                    if (GAMEDIC.TrangBiTranThu75.Contains(item.Name.Trim()))
                    {
                        DoStringEx(@"PetEquipSuitDepart:SetPetEquipDepartFunc('OnPetEquipSuitDepart', 800122, " + item.Index + @", 158, 2)
                            PetEquipSuitDepart: ConfirmDepart()");
                    }
                }
                RemoveMission(MissionsType.PhanGiaiTrangBiPet);
            }
        }


        public List<GameObject> npchacthuy = new List<GameObject>();

    

        public bool IsSafeLanHoang(int x, int y)
        {
          
            foreach (GameObject obj in Objects.All)
            {
                if (obj.Name.Contains("NPC Hắc Thủy"))
                {
                    if (!npchacthuy.Any(n => n.X == obj.X && n.Y == obj.Y))
                        npchacthuy.Add(obj);
                }
                if (obj.Name.Contains("Sát thương Vi Đà Chưởng") || obj.Name.Contains("Sát thương La Hán Quyền"))
                {
                    if (obj.GetDistance(x, y) < 6)
                        return false;
                }
                if (obj.Name.Contains("Sát thương Lăng Ba Vi Bộ") || obj.Name.Contains("Bẫy Cờ"))
                {
                    if (obj.GetDistance(x, y) < 6)
                        return false;
                }
            }
            foreach (GameObject obj in npchacthuy)
            {
                if (obj.GetDistance(x, y) <= 16)
                    return false;
            }
            return true;
        }

        public bool IsAskPk { get; set; }
        public int safex = 0;
        public int safey = 0;

        public void MoveVongTron()
        {
            int tamx = 0;
            int tamy = 0;
            int x = 0;
            int y = 0;
            if (TLBB.MapId == MAP.LangVanDaiPhat)
                tamx = tamy = 59;
            if (TLBB.MapId == MAP.CoDongTuocDai)
                tamx = tamy = 96;
            if (TLBB.MapId == MAP.ThienNhanHoangSon)
                tamx = tamy = 70;
            if (PartyIndex == 0)
                x = -6;
            if (PartyIndex == 5)
                x = 6;
            if (PartyIndex == 1)
            {
                x = -3; y = -5;
            }
            if (PartyIndex == 2)
            {
                x = 3; y = -5;
            }
            if (PartyIndex == 3)
            {
                x = -3; y = 5;
            }
            if (PartyIndex == 4)
            {
                x = 3; y = 5;
            }
            if (tamx != 0 && GetDistance(tamx + x, tamy + y) > 2)
                LUA.Move(tamx + x, tamy + y);
        }

        //59,59 => langvan
        //70,70 => thiennhanhoangson
        //96,96 => codongtuocdai
        //setmetatable(_G, {__index = WuLin_YinYang_Env}); 

        //PushDebugMessage(WuLin_YinYang_Point:GetProperty("UnifiedPosition"));
        //codongtuocdai => conroi 102,97; 91,97 .. langvan => conroi 65,61;54,58 .. thiennhan => conroi 77,70;66,70
        public void Attack()
        {

            bool isHotro = Setting.Is("checkHoTro");


            if (TLBB.IsRide || IsPhanDame || NeBayTime.Elapsed.TotalSeconds < 2)
                return;
            if (TLBB.IsFollow || !IsAuto || !IsAttack)
                return;
            if (Leader != null)
            {
                if (Leader.Missions.Contains(MissionsType.DatDoiQ123ToChau) || Leader.Missions.Contains(MissionsType.DatDoiQ123LauLan))
                {
                    if (TLBB.MapId == MAP.ViemMaSon || TLBB.MapId == MAP.TamTaiHiepCoc)
                    {
                        SimpleAttack((Qx, Qy, QDistance));
                        return;
                    }
                }
                if (TLBB.MapId == MAP.YenTuO)
                {
                    if (Leader.Missions.Contains(MissionsType.DatDoiYenTuO))
                    {
                        if (GetDistance(50, 75) < 30)
                        {
                            if (Leader.IsWaitDoiNgu)
                                return;
                        }
                    }
                }
                if(Leader.Missions.Contains(MissionsType.DatDoiAcBa) || Leader.Missions.Contains(MissionsType.DatDoiAcTac) || Leader.Missions.Contains(MissionsType.DatDoiTangKinhCac))
                {
                    if (!IsMapPhuBan())
                        return;
                }
            }

            if (isHotro)
            {
                if(TLBB.MapId == MAP.CoDongTuocDai || TLBB.MapId == MAP.ThienNhanHoangSon || TLBB.MapId == MAP.LangVanDaiPhat)
                {
                    List<string> ListBossMoveVongQuanh = new List<string>()
                    {
                        "Nguyên Vi Chi",//tieudao
                        "Sương Kiến",//thienson
                        "Mộ Dung Tín",//modung
                        "Yến Trì Dã",//minhgiao
                        "Đường Giản",//duongmon
                        "Dạ Lai",//tinhtuc
                        "Đoạn Tư An",//thienlong
                        "Thôi Anh",//ngamy
                    };
                    if (TLBB.IsNoiCong)
                    {
                        if (Objects.Monters.Any(o => ListBossMoveVongQuanh.Contains(o.Name)))
                        {
                            MoveVongTron();
                        }
                    }
                    if(Objects.All.Any(o => o.Name == "Phó Chí Viễn"))
                    {
                        DoStringEx("setmetatable(_G, {__index = WuLin_YinYang_Env}); if this:IsVisible() then return WuLin_YinYang_Point:GetProperty('UnifiedPosition'); end");
                        string amduong = LuaToStringMemo("AmDuong");
                        if (amduong.Split(',').Length >= 2)
                        {
                            int am = TDT.ParseInt(amduong.Split(',')[1]);
                            if(am <= 30)
                            {
                                GameObject npc = Objects.All.Where(o => o.Name == "NPC Bạch Võ Đang").OrderBy(o => o.Distance).FirstOrDefault();
                                if(npc!= null)
                                {
                                    Move(npc.X, npc.Y);
                                }
                            }
                            else if(am >= 90)
                            {
                                GameObject npc = Objects.All.Where(o => o.Name == "NPC Hắc Võ Đang").OrderBy(o => o.Distance).FirstOrDefault();
                                if (npc != null)
                                {
                                    Move(npc.X, npc.Y);
                                }
                            }
                        }
                    }

                    BestTarget = Objects.All.Where(o => o.Name.Contains("Phương Tướng")).OrderBy(o => o.Distance).FirstOrDefault();
                    if (BestTarget != null)
                    {
                        if (GoToEx(BestTarget.X, BestTarget.Y))
                            UseSkill(0xF78, BestTarget.Id);
                        return;
                    }

                    if (Objects.All.Where(o => (o.Name == "Phương Lương" || o.Name == "NPC Cảnh Báo Bắt Giữ" || o.Name == "NPC Bẫy" || o.Name == "NPC Cảnh Báo Làm Mới Quỷ Diện") && o.Distance < 6).Count() > 0)
                    {
                        int tamx = 0;
                        int tamy = 0;
                        if (TLBB.MapId == MAP.LangVanDaiPhat)
                            tamx = tamy = 59;
                        if (TLBB.MapId == MAP.CoDongTuocDai)
                            tamx = tamy = 96;
                        if (TLBB.MapId == MAP.ThienNhanHoangSon)
                            tamx = tamy = 70;
                        for (int i = tamx - 8; i < tamx + 9; i++)
                        {
                            for (int j = tamy - 8; j < tamy + 9; j++)
                            {
                                if (Objects.All.Where(o => (o.Name == "Phương Lương" || o.Name == "NPC Cảnh Báo Bắt Giữ" || o.Name == "NPC Bẫy" || o.Name == "NPC Cảnh Báo Làm Mới Quỷ Diện") && o.GetDistance(i, j) < 6).Count() == 0)
                                {
                                    LUA.Move(i, j);
                                    return;
                                }
                            }
                        }
                    }
                    
                    BestTarget = Objects.Monters.Where(o => o.Name.Contains("Tri Thù") || o.Name.Contains("Thiết Tật Lê")).OrderBy(o => o.Distance).FirstOrDefault();
                    if (BestTarget != null)
                    {
                        SelectTarget(BestTarget.Id);
                        SendKey(Global.BaseSkill);
                        return;
                    }
                    BestTarget = Objects.Monters.Where(o => !o.Name.Contains(" ")).OrderBy(o => o.Distance).FirstOrDefault();
                    if (BestTarget != null)
                    {
                        if (Objects.Target.Id != BestTarget.Id)
                        {
                            SelectTarget(BestTarget.Id);
                            SendKey(Global.BaseSkill);
                            return;
                        }
                    }
                }
            }

          

            if (TLBB.MapId == MAP.LanHoanPhucDia && Global.IsPhucDia)
            {
                GameObject toanphong = Objects.Monters.Where(o => o.Name == "Kích Sát Phá Trận" && (new Point(RoundX, RoundY).IsClockwise(new Point(o.RoundX, o.RoundY), new Point(182, 206)) || o.Distance <= 10)).OrderBy(o => o.Distance).FirstOrDefault();
                if (toanphong != null)
                {
                    if (GetDistance(toanphong.X, toanphong.Y) > 4)
                    {
                        LUA.Move(toanphong.X, toanphong.Y);
                    }
                    else
                    {
                        SelectTarget(toanphong.Id);
                        SendKey(Global.BaseSkill);
                    }
                    return;
                }
            }

            if (TLBB.Menpai == MENPAI.NgaMy)
            {
                if (swXuattieudattai.Elapsed.TotalSeconds < 15 && TLBB.PetHPPercent == 0)
                    return;
            }


            if (TLBB.MapId == MAP.BinhThanhKyTran && (isHotro || QuanDoans.Any(o => o.Missions.Contains(MissionsType.DatDoiBinhThanh))))
            {
                List<string> CocTruc = new List<string>()
                        {
                            "Trúc Đồng Tâm-Tím",
                            "Trúc Đồng Tâm-Lam",
                            "Trúc Đồng Tâm-Đỏ",
                        };
                string s = Game.IniParser.Read("Config", "BinhThanh").Trim();
                if (s.Contains("["))
                {
                    CocTruc.Clear();
                    if (s.Contains("Băng"))
                        CocTruc.Add("Trúc Đồng Tâm-Lam");
                    if (s.Contains("Hỏa"))
                        CocTruc.Add("Trúc Đồng Tâm-Đỏ");
                    if (s.Contains("Độc"))
                        CocTruc.Add("Trúc Đồng Tâm-Lục");
                    if (s.Contains("Huyền"))
                        CocTruc.Add("Trúc Đồng Tâm-Vàng");
                    if (s.Contains("Nội"))
                        CocTruc.Add("Trúc Đồng Tâm-Tím");
                    if (s.Contains("Ngoại"))
                        CocTruc.Add("Trúc Đồng Tâm-Trắng");
                }
                if (TDT.GetDistance(CharX, CharY, 172, 35) <= 20)
                {
                    GameObject bestarget = Objects.All.Where(o => o.Name.Contains("Trúc Đồng Tâm")).Where(o => o.HP > 0 && CocTruc.Contains(o.Name)).OrderBy(o => o.Distance).FirstOrDefault();
                    if (bestarget != null)
                    {
                        SelectTarget(bestarget.Id);
                        SendKey(Global.BaseSkill);
                        return;
                    }
                }
                if (TDT.GetDistance(CharX, CharY, 172, 100) <= 20)
                {
                    GameObject bestarget = Objects.All.Where(o => o.Name.Contains("Trúc Đồng Tâm")).Where(o => o.HP > 0 && !CocTruc.Contains(o.Name)).OrderBy(o => o.Distance).FirstOrDefault();
                    if (bestarget != null)
                    {
                        SelectTarget(bestarget.Id);
                        SendKey(Global.BaseSkill);
                        return;
                    }
                }

                GameObject bestObj = Objects.Monters.Where(o => o.Name == "Địa Phủ Ngưu Yêu" || o.Name == "Toàn Phong").OrderBy(o => o.HP).FirstOrDefault();
                if (bestObj != null && TLBB.IsNoiCong)
                {
                    SelectTarget(bestObj.Id);
                    SendKey(Global.BaseSkill);
                    return;
                }

                ////if (DiaPhuNguYeu.Elapsed.TotalSeconds <= 3)
                ////{
                ////    GameObject daluatdiem = Objects.Monters.Where(o => o.CleanName == "daluatdiem" && o.Distance < 6).FirstOrDefault();
                ////    if (daluatdiem != null)
                ////    {
                ////        float mindistance = 9999;
                ////        for (int i = 181; i < 201; i++)
                ////        {
                ////            for (int j = 185; j < 205; j++)
                ////            {
                ////                if (TDT.GetDistance(191, 195, i, j) < 20)
                ////                {
                ////                    if (TDT.GetDistance(i, j, daluatdiem.X, daluatdiem.Y) >= 8 && TDT.GetDistance(191, 195, i, j) < mindistance)
                ////                    {
                ////                        mindistance = TDT.GetDistance(191, 195, i, j);
                ////                        safex = i;
                ////                        safey = j;
                ////                    }
                ////                }
                ////            }
                ////        }
                ////        if (safex != 0)
                ////        {
                ////            Move(safex, safey);
                ////            return;
                ////        }
                ////    }
                ////}

               
            }

            if (NeedToMove != string.Empty)
                return;
            if (Missions.Contains(MissionsType.NhiemVuThangCap))
                return;
            if (Missions.Contains(MissionsType.NhiemVuSuMon) || Missions.Contains(MissionsType.TrungAc) || Missions.Contains(MissionsType.BachHoaDuyen) || (Missions.Contains(MissionsType.KhaiKhoang) && IsMapNghe()) || (Missions.Contains(MissionsType.HaiDuoc) && IsMapNghe()))
                return;

            BestTarget = Objects.Monters.Where(o => o.Name == "Tất Phương Huyễn Tượng" || o.Name == "Thị Ma Giả" || o.Name == "Vô Tướng Tung Ảnh").OrderBy(o => o.HP).FirstOrDefault();
            if (BestTarget != null)
            {
                if (TLBB.PlayerState == 7 && BestTarget.Id == TargetId)
                {

                }
                else
                {
                    SelectTarget(BestTarget.Id);
                    SendKey(Global.BaseSkill);
                }
                return;
            }

            if (TLBB.MapId == MAP.PhungMinhVuongLang)
            {
                GameObject codieu = Objects.All.Where(o => o.Name == "Cổ Điêu").FirstOrDefault();
                if (codieu != null)
                {
                    BestTarget = Objects.All.Where(o => o.Name == "Trấn Ma Thiên Lôi" && o.GetDistance(codieu.X, codieu.Y) < 4).FirstOrDefault();
                    if(BestTarget != null)
                    {
                        SelectTarget(BestTarget.Id);
                        SendKey(Global.BaseSkill);
                    }
                    else
                    {
                        GoToEx(codieu.X, codieu.Y);
                    }
                    return;
                }
                BestTarget = Objects.Monters.Where(o => o.Name == "Thức Ăn Thơm Ngon").OrderBy(o => o.GetDistance(48,48)).FirstOrDefault();
                if (BestTarget != null)
                {
                    SelectTarget(BestTarget.Id);
                    SendKey(Global.BaseSkill);
                    return;
                }            
            }
            if (TLBB.PlayerState == 6)
                return;
            if ((TLBB.IsNoiCong || IsMapPhuBan()) && TLBB.PlayerState == 2)
                return;
            if (Global.AtkFollowKey && Objects.Key != null && !TLBB.IsLeader)
            {
                if (Objects.Key.State == 7)
                {
                    if (Objects.Self.AtkToId != Objects.Key.AtkToId || Objects.Self.State != 7)
                    {
                        GameObject objKeyAtk = Objects.All.Where(o => o.Id == Objects.Key.AtkToId).FirstOrDefault();
                        if (objKeyAtk != null)
                        {
                            SelectTarget(objKeyAtk.Id);
                            SendKey(Global.BaseSkill);
                            return;
                        }
                    }
                }
                else
                {
                    if (Address.GameType == 1)
                    {
                        SelectTarget(Objects.Key.Id);
                        Thread.Sleep(500);
                        SelectTargetOfTarget();
                        Thread.Sleep(500);
                        SendKey(Global.BaseSkill);
                        return;
                    }
                }
            }
            else if (!Global.Paused)
            {
                if (AutoPK.IsPK && AutoPK.NamePK != string.Empty)
                {
                    foreach (GameObject _object in Objects.Players.Where(o => o.Id != SelfId))
                    {
                        if (AutoPK.NamePK == _object.Name)
                        {
                            if (TLBB.IsRide)
                                DownRide();
                            PostMessage(_object.Id, 128);
                            break;
                        }
                    }
                }
                if (IsAskPk && !TLBB.IsThanhThi && Objects.Pk.Count == 0)
                {
                    foreach (GameObject _object in Objects.Players.Where(o => o.Id != Objects.Self.Id && !SettingOld.HaveToPKIgnore.Contains(o.Name)))
                    {
                        if (SettingOld.HaveToPKName.Contains(_object.Name) || SettingOld.HaveToPKGuild.Contains(_object.GuildName))
                        {
                            PostMessage(_object.Id, 128);
                            return;
                        }
                    }
                }

                if (!IsMapPhuBan() && RadiusX != 0 && TLBB.MapId == RadiusMap && TrueStandTime.Elapsed.TotalSeconds > 0.5 && GetDistance(RadiusX, RadiusY) > 5)
                {
                    Move(RadiusX, RadiusY);
                }

                if (IsLureEx || Objects.Target == null || Objects.Target.HP <= 0 || !Objects.TargetIsMine || TrueStandTime.Elapsed.TotalSeconds > 0.4)
                {
                    GetBestTarget();

                    if (BestTarget != null)
                    {
                        if ((TLBB.MapId == MAP.LauLanBaoTang || TLBB.MapId == MAP.TranLongKyCuoc || TLBB.MapId == MAP.MauDonUyen) && TLBB.IsLeader)
                        {
                            if (GetDistance(BestTarget.X, BestTarget.Y) > 5)
                            {
                                Move(BestTarget.X, BestTarget.Y);
                                return;
                            }
                        }
                        CareObject = BestTarget;
                        if (TLBB.PlayerState == 7 && BestTarget.Id == TargetId)
                        {

                        }
                        else
                        {
                            SelectTarget(BestTarget.Id);
                            SendKey(Global.BaseSkill);
                        }
                    }
                }
            }
        }


        public void SelectTargetOfTarget()
        {
            PostMessage(0, 105);
        }

        private int RoundIdx = 1;

        private int GetRound(float x, float y)
        {
            if (TLBB.MapId == MAP.TanHoangDiaCungTang1)
            {
                if (x > 196 && x < 213 && y > 112 && y < 116)
                {
                    RoundIdx = 1;
                    return 1;
                }
                if (x > 219 && x < 232 && y > 97 && y < 240)
                {
                    RoundIdx = 1;
                    return 1;
                }
                // round 2
                if (x > 121 && x < 147 && y > 120 && y < 124)
                {
                    RoundIdx = 2;
                    return 2;
                }
                if ((x < 200 && x > 140 && y < 110 && y > 100) || (x < 190 && x > 151 && y >= 110 && y < 130) || (x <= 210 && x >= 130 && y >= 130))
                {
                    RoundIdx = 2;
                    return 2;
                }
                //if(x < 190 && x > 151 && y >= 110 && y < 130)
                //{
                //    TanHoangRoundIdx = 2;
                //    return 2;
                //}
                //if (x <= 210 && x >= 130 && y >= 130)
                //{
                //    TanHoangRoundIdx = 2;
                //    return 2;
                //}

                if (x > 20 && x < 40 && y > 102 && y < 127)
                {
                    RoundIdx = 4;
                    return 4;
                }
                if (x > 20 && x < 240 && y < 95)
                {
                    RoundIdx = 4;
                    return 4;
                }

                // round 3
                if (x > 23 && x < 29 && y > 129 && y < 155)
                {
                    RoundIdx = 3;
                    return 3;
                }
                if ((x <= 119 && x >= 45 && y >= 115 && y <= 140) || (x <= 125 && x >= 28 && y >= 115 && y > 140) || (x >= 80 && y <= 90))
                {
                    RoundIdx = 3;
                    return 3;
                }
                //if ((x <= 40 && y <= 127) || (x <= 80 && y <= 105) || (x <= 28 && y >= 115 && y >= 158))
                //{
                //    TanHoangRoundIdx = 3;
                //    return 3;
                //}
                //if (y > 170)
                //{
                //    if (x < 210 && x > 137)
                //    {
                //        TanHoangRoundIdx = 2;
                //        return 2;
                //    }
                //    if (y < 183 && x < 188 && x > 151)
                //    {
                //        TanHoangRoundIdx = 2;
                //        return 2;
                //    }
                //}
                //if (x > 45 && y > 108 && x < 119 && y < 225)
                //{
                //    TanHoangRoundIdx = 3;
                //    return 3;
                //}
                //if (x > 18 && x < 75 && y > 160 && y < 225)
                //{
                //    TanHoangRoundIdx = 3;
                //    return 3;
                //}
            }
            if (TLBB.MapId == MAP.TanHoangDiaCungTang2)
            {
                if (x < 95 && y > 195)
                {
                    RoundIdx = 1;
                    return 1;
                }
                if (x > 95 && y > 205)
                {
                    RoundIdx = 1;
                    return 1;
                }
                if (x < 80 && y <= 195)
                {
                    RoundIdx = 1;
                    return 1;
                }
                if (x < 100 && y <= 80)
                {
                    RoundIdx = 1;
                    return 1;
                }

                if (x > 108 && x < 117 && y > 33 && y < 44)
                {
                    RoundIdx = 1;
                    return 1;
                }

                if (x > 125 && y < 45)
                {
                    RoundIdx = 2;
                    return 2;
                }
                if (x > 125 && x < 240 && y >= 45 && y <= 195)
                {
                    RoundIdx = 2;
                    return 2;
                }
                if (x >= 95 && x <= 135 && y >= 45 && y <= 190)
                {
                    RoundIdx = 2;
                    return 2;
                }
            }
            if (TLBB.MapId == MAP.HoangLongDong)
            {
                if (x >= 120 && x <= 250 && y >= 190)
                {
                    RoundIdx = 1;
                    return 1;
                }
                if (x >= 90 && x <= 120 && y >= 200)
                {
                    RoundIdx = 1;
                    return 1;
                }
                RoundIdx = 2;
                return 2;
            }
            return RoundIdx;
        }



        public void Move(string point)
        {
            int x = TDT.ParseInt(point);
            int y = TDT.ParseInt(point.Replace(x + ",", ""));
            Move(x, y);
        }


        public void SuaVoHon()
        {
            if (GoToEx(DAILY.VoDong))
            {
                if (PacketItems.All.Concat(PacketItems.ThienCo).Where(i => i.Name == "Võ Hồn Diên Thọ Đan").Count() == 0)
                {
                    RemoveMission(MissionsType.SuaVoHon);
                    PushDebugMessage("Không có Võ Hồn Diên Thọ Đan. Dừng Auto");
                    return;
                }               
                PacketItem bestitem = null;
             
                //F0 D1 CB 00 00 00 00 00 00 00 00 00 FF FF FF FF 12 FF => 62D1F0
          
                foreach (var item in PacketItems.ThienCo)
                {
                    if (item.Name == "Võ Hồn Diên Thọ Đan")
                    {
                        GetItemThienCo(item.Index);
                        Thread.Sleep(350);
                    }
                }

                lstindex.Clear();
                foreach (var item in PacketItems.All)
                    lstindex.Add((int)item.Index);

                SendPacket(HexToString(AddressGameExe + 0x62D1F0) + "00 00 00 00 00 00 00 00 FF FF FF FF 12 FF");
                Thread.Sleep(1000);
                bestitem = null;

                foreach (var item in PacketItems.All)
                {
                    if (item.TypeName.Trim() == "Võ Hồn")
                    {
                        if (lstindex.Contains((int)item.Index))
                            continue;
                        else
                            bestitem = item;
                    }
                }
                if (bestitem == null)
                    bestitem = PacketItems.DaoCu.Where(i => i.TypeName == "Võ Hồn").OrderByDescending(o => o.CapHopThanhVoHon).FirstOrDefault();
                if (bestitem != null)
                {
                    foreach (var item in PacketItems.All)
                    {
                        if (item.Name == "Võ Hồn Diên Thọ Đan")
                        {
                            SendPacket(HexToString(AddressGameExe + 0x6D8CB8) + "00 00 00 00 00 00 00 00 FF FF FF FF 04 00 00 00 " + bestitem.Index.ToString("X2") + item.Index.ToString("X2"));            
                            break;
                        }
                    }
                    int cnt = 0;
                    while (true)
                    {
                        if (cnt++ > 4)
                            break;
                        if (TLBB.IsRide)
                        {
                            DownRide();
                            Thread.Sleep(350);
                        }
                        bestitem.DoAction();
                        if (PacketItems.TrangBi.Where(i => i.TypeName == "Võ Hồn").Count() > 0)
                            break;
                    }
                }
                RemoveMission(MissionsType.SuaVoHon);
            }
        }

        public void SuaThanKhi()
        {
            if (GoToEx(TOCHAU.AuDaTu))
            {
                PacketItem tanmangthanphu = null;
                foreach (var item in PacketItems.All)
                {
                    if (GAMEDIC.TanMangThanPhu.Contains(item.Type))
                    {
                        tanmangthanphu = item;
                    }
                }

                if (tanmangthanphu == null)
                {
                    bool isget = false;
                    foreach (var item in PacketItems.ThienCo)
                    {
                        if (GAMEDIC.TanMangThanPhu.Contains(item.Type))
                        {
                            GetItemThienCo(item.Index);                            
                            isget = true;
                        }
                    }
                    if (!isget)
                    {
                        RemoveMission(MissionsType.SuaThanKhi);
                        PushDebugMessage("Không có tần mãng thần phù");
                    }
                    else
                    {
                        Thread.Sleep(350);
                    }
                    return;
                }

                if (tanmangthanphu == null)
                {
                    foreach (var item in PacketItems.All)
                    {
                        if (GAMEDIC.TanMangThanPhu.Contains(item.Type))
                        {
                            tanmangthanphu = item;
                        }
                    }
                }

                lstindex.Clear();
                foreach (var item in PacketItems.All)
                    lstindex.Add((int)item.Index);

                SendPacket(HexToString(AddressGameExe + 0x62D1F0) + "00 00 00 00 00 00 00 00 FF FF FF FF 00 FF");
                Thread.Sleep(1000);
                PacketItem bestitem = null;

                foreach (var item in PacketItems.All)
                {
                    if (Weapon.Contains(item.TypeName.Trim()))
                    {
                        if (lstindex.Contains((int)item.Index))
                            continue;
                        else
                            bestitem = item;
                    }
                }
                if (bestitem != null && tanmangthanphu != null)
                {
                    DoStringEx("Clear_XSCRIPT(); Set_XSCRIPT_Function_Name('OnEquipRepair'); Set_XSCRIPT_ScriptID(805027); Set_XSCRIPT_Parameter(0,0); Set_XSCRIPT_Parameter(1,108); Set_XSCRIPT_Parameter(2," + bestitem.Index + @"); Set_XSCRIPT_Parameter(3," + tanmangthanphu.Index + @"); Set_XSCRIPT_Parameter(4,1); Set_XSCRIPT_ParamCount(5); Send_XSCRIPT();");
                    DoStringEx("Clear_XSCRIPT(); Set_XSCRIPT_Function_Name('OnEquipRepair'); Set_XSCRIPT_ScriptID(805027); Set_XSCRIPT_Parameter(0,0); Set_XSCRIPT_Parameter(1,108); Set_XSCRIPT_Parameter(2," + bestitem.Index + @"); Set_XSCRIPT_Parameter(3," + tanmangthanphu.Index + @"); Set_XSCRIPT_Parameter(4,1); Set_XSCRIPT_ParamCount(5); Send_XSCRIPT();");
                    DoStringEx("Clear_XSCRIPT(); Set_XSCRIPT_Function_Name('OnEquipRepair'); Set_XSCRIPT_ScriptID(805027); Set_XSCRIPT_Parameter(0,0); Set_XSCRIPT_Parameter(1,108); Set_XSCRIPT_Parameter(2," + bestitem.Index + @"); Set_XSCRIPT_Parameter(3," + tanmangthanphu.Index + @"); Set_XSCRIPT_Parameter(4,1); Set_XSCRIPT_ParamCount(5); Send_XSCRIPT();");

                    bestitem.DoAction();
                    PushDebugMessage("Trang bị " + bestitem.Name);
                }
                RemoveMission(MissionsType.SuaThanKhi);
            }
        }

        //move
        public void Move(float x, float y, int mapId)
        {
            if (TLBB.ON_SCENE_TRANSING || IsChangeMap)
                return;
            if (swAutoMove.Elapsed.TotalSeconds < 4)
            {
                return;
            }
            if (tranTime.Elapsed.TotalSeconds < 4)
                return;

            if (Leader != null && Leader.Missions.Contains(MissionsType.DatDoiQuanSonHai))
            {
                if (mapId == MAP.DaiLy && TLBB.MapId != MAP.DaiLy)
                {
                    foreach (GameObject g in Objects.All.Where(o => o.HP > 0).OrderBy(o => o.Distance))
                    {
                        if (g.Title.Contains("#{PTFB"))
                        {
                            if (GoToEx(g))
                            {
                                Talk(g.Id);
                                Thread.Sleep(500);
                                QuestFrame.Click("#{PTFB_191228_218}");
                                QuestFrame.Click("#{PTFB_191228_73}");

                                Thread.Sleep(500);
                                QuestFrame.Click("#{PTFB_191228_477}");
                            }
                            return;
                        }
                    }
                }
            }

            if (TLBB.IsMapChienMinh((uint)mapId) && !TLBB.IsMapChienMinh(TLBB.MapId))
            {
                if (GoToEx(PHUNGMINHTRAN.SaoVanKhue))
                {
                    if (TLBB.IsQuestOpen)
                    {
                        QuestFrame.Click("#{MZBUILD_150817_262}");
                        CloseQuest();
                    }
                    else
                    {
                        Talk(PHUNGMINHTRAN.SaoVanKhue);
                    }
                }
                return;
            }
            if (TLBB.MapId == MAP.HoangLongDong)
            {
                int curRound = GetRound(RoundX, RoundY);
                int toRound = GetRound(x, y);
                if (mapId == MAP.HoangLongDong || mapId == -1)
                {
                    if (curRound == toRound)
                    {
                        Move(x, y);
                        return;
                    }
                    else
                    {
                        if (curRound < toRound)
                        {
                            Move(221, 205);
                            return;
                        }
                        else
                        {
                            Move(215, 165);
                            return;
                        }
                    }
                }
                else
                {
                    if (curRound == 2)
                    {
                        Move(215, 165);
                        return;
                    }
                }
            }
            if (TLBB.MapId == mapId)
            {
                if (TDT.GetDistance(CharX, CharY, x, y) > 1.5)
                {
                    Move(x, y);

                    return;
                }
                return;
            }
            if (mapId == MAP.TranLongKyCuoc)
            {
                return;
            }
            if (mapId == MAP.TungSonPhongThienDai)
            {
                if (GoToEx(146, 156))
                {
                    Talk(6);
                    Thread.Sleep(1000);
                    QuestFrame.Click("Đưa ta đi Lạc Dương");
                }
                return;
            }
            if (mapId == MAP.TuTuyetTrang)
            {
                if (GoToEx(TOCHAU.PhanThanhThanh))
                {
                    IsP = true;
                }
                return;
            }
            if (mapId == MAP.LoiDaiSinhTu)
            {
                if (GoToEx(DAILY.KhoVinhDaiSu))
                {
                    IsP = true;
                }
                return;
            }

            if (mapId == MAP.YenTuO)
            {
                GoToEx(THAIHO.LyCuong);
            }
            if (IsMapMonPhai && mapId == MAP.DonHoang)
            {
                if (TLBB.Lvl < 20)
                {
                    Move(LACDUONG.KieuPhucThinh.X, LACDUONG.KieuPhucThinh.Y, LACDUONG.KieuPhucThinh.Map);
                    return;
                }
            }

            if (TLBB.MapId == MAP.MauDonUyen)
            {
                GoToEx(37, 31);
                IsP = true;
                return;
            }

            if(TLBB.MapId == MAP.TuTuyetTrang)
            {
                if (GoToEx(23, 17))
                    IsP = true;
                return;
            }

            if (TLBB.MapId == MAP.ThieuThatSon)
            {
                if (Global.IsBoBoss)
                {
                    foreach (GameObject _obj in Objects.All)
                    {
                        if (_obj.CleanName == "thieulamdetu")
                        {
                            Talk(_obj.Id);
                            Thread.Sleep(1000);
                            QuestFrame.Click("#{CJG_101231_79}");
                            Thread.Sleep(1000);
                            return;
                        }
                    }
                    GoToEx(THIEUTHATSON.DinhXuanThu);
                    return;
                }
            }

            // phu ban dao hoa
            if (TLBB.MapId == 732)
            {
                Move(60, 87);
                return;
            }
            // phu ban nhiem vu quy coc
            if (TLBB.MapId == 679)
            {
                Move(31, 53);
                return;
            }
            if (GAMEDIC.CuaRaPhuBan.ContainsKey((int)TLBB.MapId))
            {
                Move(GAMEDIC.CuaRaPhuBan[(int)TLBB.MapId]);
                return;
            }
            if (TLBB.MapId == 0 && mapId == DUONGMON.Id)
            {
                if (TDT.GetDistance(RoundX, RoundY, LACDUONG.NgoDucXuong.X, LACDUONG.NgoDucXuong.Y) > 3)
                {
                    GoToEx(LACDUONG.NgoDucXuong.X, LACDUONG.NgoDucXuong.Y);
                    return;
                }
            }

            if (Address.GameType != 1)
            {
                AutoMoveEx.ArrMap.Clear();
                AutoMoveEx.ChuaTimThay = true;
                AutoMoveEx.MapCanDen = PRIVATEMOVE.MapToString(mapId);
                if (!string.IsNullOrEmpty(AutoMoveEx.MapCanDen))
                {
                    AutoMoveEx.MapHienTai = PRIVATEMOVE.MapToString((int)TLBB.MapId);
                    if (!string.IsNullOrEmpty(AutoMoveEx.MapCanDen))
                    {
                        AutoMoveEx.TimDuongDi(AutoMoveEx.MapHienTai);
                        if (AutoMoveEx.ArrMap.Count > 1)
                        {
                            string point = AutoMoveEx.TimIndex(AutoMoveEx.ArrMap[0], AutoMoveEx.ArrMap[1]);
                            if (point.Split(',').Length > 1)
                            {
                                x = TDT.ParseInt(point.Split(',')[0]);
                                y = TDT.ParseInt(point.Split(',')[1]);
                                Move(x, y);
                                return;
                            }
                        }
                    }
                }
                if (TLBB.MapId == MAP.ToChau)
                {
                    Move(279, 43);
                }
                if (TLBB.MapId == MAP.ThaiHo)
                {
                    Move(216, 282);
                }
                if (TLBB.MapId == MAP.ThuyLao)
                {
                    Move(95, 95);
                }
                return;
            }
            string clearmapname = TDT.VietLien(TLBB.MapName);
            if (clearmapname.Contains("thienkieplau"))
            {
                Move(81, 81);
                return;
            }

            if (TLBB.MapId == MAP.ThieuThatSon)
            {
                Move(122, 35);
                GameObject TaoDiaThanTang = Objects.GetObject("Tảo Địa Thần Tăng");
                if (TaoDiaThanTang != null)
                {
                    if (TLBB.IsQuestOpen)
                    {
                        QuestFrame.ClickAll();
                        QuestFrame.Close();
                    }
                    else
                    {
                        Talk(TaoDiaThanTang);
                    }
                }
            }
            if (TLBB.MapId == MAP.LoiDaiSinhTu)
            {
                if (GoToEx(LOIDAISINHTU.KhoVinhDaiSu))
                {
                    foreach (GameObject _object in Objects.All)
                    {
                        if (TDT.VietLien(_object.Name) == "khovinhdaisu" && _object.IsNPC)
                        {
                            Talk(_object.Id);
                            Thread.Sleep(350);
                            break;
                        }
                    }
                    if (QuestFrame.Click(402049, 1))
                    {
                        IsP = false;
                    }
                    if (QuestFrame.Click(402049, 3))
                    {
                        IsP = false;
                    }
                }
                return;
            }
            if (mapId == MAP.LoiDaiSinhTu)
            {
                if (GoToEx(DAILY.KhoVinhDaiSu))
                {
                    if (!IsP)
                        IsP = true;
                }
                return;
            }
            if (TLBB.MapId == MAP.PhieuMieuPhong)
            {
                if (Address.GameType == 1)
                {
                    if (GoToEx(96, 40))
                    {
                        if (TLBB.IsQuestOpen)
                        {
                            if (QuestFrame.Text.Contains("Quyết chiến Lý Thu Thủy?"))
                            {
                                DeadObjects.Remove("Lý Thu Thủy");
                                IsTalkOLaoDai = false;
                            }
                            QuestFrame.Click(402275, 3);
                            QuestFrame.Click(402275, 4);
                            QuestFrameOptionClicked(402276, 3);
                            QuestFrameOptionClicked(402288, 4);
                        }
                        else
                        {
                            Talk("olaodai");
                        }
                    }
                    return;
                }
                else
                {
                    if (TLBB.MapId == MAP.PhieuMieuPhong)
                    {
                        if (TDT.GetDistance(CharX, CharY, 124, 171) <= 10)
                            DoStringEx(" setmetatable(_G, { __index = MessageBox_Self_Env});MessageBox_Self_OK_Clicked();setmetatable(_G, {__index = AcceptBox_Env}); AcceptBox_OK_Clicked(); ");
                        Move(124, 171);
                        return;
                    }
                }
                return;
            }

            if (TLBB.MapId == 604)
            {
                Move(45, 51);
                return;
            }
            if (TLBB.MapId == MAP.TranLongKyCuoc)
            {
                if (GoToEx(40, 40))
                {
                    if (TLBB.IsQuestOpen)
                    {
                        QuestFrame.Click(44000, 0);
                        QuestFrame.Close();
                    }
                    foreach (GameObject _object in Objects.AllNpc)
                    {
                        if (_object.RoundX == 40 && _object.RoundY == 40)
                        {
                            Talk(_object.Id);
                            return;
                        }
                    }
                }
                return;
            }
            if (TLBB.MapId == 550)
            {
                foreach (GameObject _object in Objects.AllNpc)
                {
                    int npcx = (int)_object.X;
                    int npcy = (int)_object.Y;
                    if (x == 40 && y == 40)
                    {
                        Talk(_object.Id);
                        if (TLBB.IsQuestOpen)
                        {
                            QuestFrame.ClickOut(this);
                        }
                        return;
                    }
                }
                if (TDT.GetDistance(CharX, CharY, 104, 79) > 3)
                {
                    Move(40, 40);
                }
                return;
            }
            if (TLBB.MapId == MAP.VanKiemCoc || TLBB.MapId == MAP.VanKiemCocDem)
            {
                if (TLBB.IsQuestOpen)
                {
                    QuestFrame.ClickOut(this);
                    CloseQuest();
                }
                foreach (GameObject _object in Objects.All)
                {
                    if (_object.CleanName == "hoahachcan" || _object.CleanName == "vankiepcocmocnhanthuve")
                    {
                        Talk(_object.Id);
                        return;
                    }
                }
                if (TDT.GetDistance(CharX, CharY, 105, 79) > 3)
                {
                    Move(105, 79);
                }
                return;
            }
            if (mapId == MAP.VanKiemCoc || mapId == MAP.VanKiemCocDem)
            {
                if (TLBB.IsQuestOpen)
                {
                    QuestFrame.ClickPhuBanMonPhai(this);
                    QuestFrame.Close();
                    return;
                }
                else
                {
                    if (GoToEx(DAILY.HoaHachCan))
                    {
                        Talk(DAILY.HoaHachCan);
                    }
                }
                return;
            }

            if (TLBB.MapId == MAP.ThuongMangSon || TLBB.MapId == MAP.ThuongMangSonNgay)
            {
                if (RunTo("[Thương Mang Sơn-Gia Luật Mạc Ca][104,20-" + TLBB.MapId + "]"))
                {
                    TalkEx("Gia Luật Mạc Ca");
                    Thread.Sleep(1000);
                    QuestFrame.ClickAll();
                }
                return;
            }
            if (mapId == MAP.ThuongMangSon || mapId == MAP.ThuongMangSonNgay)
            {
                if (RunTo("[Nhạn Nam-Gia Luật Mạc Ca][129,50-18]"))
                {
                    TalkEx("Gia Luật Mạc Ca");
                    Thread.Sleep(1000);
                    QuestFrame.ClickAll();
                }
                return;
            }
            if (TLBB.MapId == MAP.LoiCoSon || TLBB.MapId == MAP.LoiCoSonNgay)
            {
                if (RunTo("[Lôi Cổ Sơn-Phạm Bách Linh][57,107-" + TLBB.MapId + "]"))
                {
                    TalkEx("Phạm Bách Linh");
                    Thread.Sleep(1000);
                    QuestFrame.ClickAll();
                }
                return;
            }
            if (mapId == MAP.LoiCoSon || mapId == MAP.LoiCoSonNgay)
            {
                if (RunTo("[Lăng Ba Động-Phạm Bách Linh][152,153-14]"))
                {
                    TalkEx("Phạm Bách Linh");
                    Thread.Sleep(1000);
                    QuestFrame.ClickAll();
                }
                return;
            }
            if (TLBB.MapId == MAP.TuHuyenTrang || TLBB.MapId == MAP.TuHuyenTrangNgay)
            {
                if (RunTo("[Tụ Hiền Trang-Từ Kinh Lôi][60,112-" + TLBB.MapId + "]"))
                {
                    TalkEx("Từ Kinh Lôi");
                    Thread.Sleep(1000);
                    QuestFrame.ClickAll();
                }
                return;
            }
            if (mapId == MAP.TuHuyenTrang || mapId == MAP.TuHuyenTrangNgay)
            {
                if (RunTo("[Lạc Dương-Từ Kinh Lôi][307,343-0]"))
                {
                    TalkEx("Từ Kinh Lôi");
                    Thread.Sleep(1000);
                    QuestFrame.ClickAll();
                }
                return;
            }
            if (TLBB.MapId == MAP.NhatPhamDuong || TLBB.MapId == MAP.NhatPhamDuongNgay)
            {
                if (RunTo("[Nhất phẩm đường-Nỗ Nhi Hải][32,115-" + TLBB.MapId + "]"))
                {
                    TalkEx("Nỗ Nhi Hải");
                    Thread.Sleep(1000);
                    QuestFrame.ClickAll();
                }
                return;
            }
            if (mapId == MAP.NhatPhamDuong || mapId == MAP.NhatPhamDuongNgay)
            {
                if (RunTo("[Lạc Dương-Nỗ Nhi Hải][208,207-0]"))
                {
                    TalkEx("Nỗ Nhi Hải");
                    Thread.Sleep(1000);
                    QuestFrame.ClickAll();
                }
                return;
            }
            if (TLBB.MapId == MAP.YếnTửỔĐêm || TLBB.MapId == MAP.YếnTửỔNgày)
            {
                if (RunTo("[Yến Tử Ổ-Lão Cố][110,104-" + TLBB.MapId + "]"))
                {
                    TalkEx("Lão Cố");
                    Thread.Sleep(1000);
                    QuestFrame.ClickAll();
                }
                return;
            }
            if (mapId == MAP.YếnTửỔĐêm || mapId == MAP.YếnTửỔNgày)
            {
                if (RunTo("[Tô Châu-Lão Cố][180,395-1]"))
                {
                    TalkEx("Lão Cố");
                    Thread.Sleep(1000);
                    QuestFrame.ClickAll();
                }
                return;
            }
            if (TLBB.MapId >= 663 && TLBB.MapId <= 667)
            {
                if (GoToEx(212, 184))
                {
                    if (TLBB.IsQuestOpen)
                    {
                        QuestFrame.Click(2);
                    }
                    else
                    {
                        foreach (GameObject _object in Objects.AllNpc)
                        {
                            if (_object.X == 212 && _object.Y == 184)
                            {
                                Talk(_object.Id);
                                return;
                            }
                        }
                    }
                }
                return;
            }
            if (mapId == MAP.ThieuLamPhuBan)
            {
                if (TLBB.MapId != MAP.ThieuLam)
                {
                    GoToEx(NPC.HuyenChung.X, NPC.HuyenChung.Y, NPC.HuyenChung.Map);
                }
                else
                {
                    if (TDT.GetDistance(CharX, CharY, NPC.HuyenChung.X, NPC.HuyenChung.Y) > 3)
                    {
                        Move(NPC.HuyenChung.X, NPC.HuyenChung.Y);
                    }
                    else
                    {
                        if (TLBB.IsQuestOpen)
                        {
                            QuestFrame.ClickPhuBanMonPhai(this);
                        }
                        else
                        {
                            Talk(NPC.HuyenChung.Id);
                        }
                    }
                }
                return;
            }
            if (mapId == MAP.CaiBangPhuBan)
            {
                if (TLBB.MapId != MAP.CaiBang)
                {
                    GoToEx(NPC.AuDuongQua.X, NPC.AuDuongQua.Y, NPC.AuDuongQua.Map);
                }
                else
                {
                    if (TDT.GetDistance(CharX, CharY, NPC.AuDuongQua.X, NPC.AuDuongQua.Y) > 3)
                    {
                        Move(NPC.AuDuongQua.X, NPC.AuDuongQua.Y);
                    }
                    else
                    {
                        if (TLBB.IsQuestOpen)
                        {
                            QuestFrame.ClickPhuBanMonPhai(this);
                        }
                        else
                        {
                            Talk(NPC.AuDuongQua.Id);
                        }
                    }
                }
                return;
            }
            if (mapId == MAP.MinhGiaoPhuBan)
            {
                if (TLBB.MapId != MAP.MinhGiao)
                {
                    GoToEx(NPC.ThacCang.X, NPC.ThacCang.Y, NPC.ThacCang.Map);
                }
                else
                {
                    if (TDT.GetDistance(CharX, CharY, NPC.ThacCang.X, NPC.ThacCang.Y) > 3)
                    {
                        Move(NPC.ThacCang.X, NPC.ThacCang.Y);
                    }
                    else
                    {
                        if (TLBB.IsQuestOpen)
                        {
                            QuestFrame.ClickPhuBanMonPhai(this);
                        }
                        else
                        {
                            Talk(NPC.ThacCang.Id);
                        }
                    }
                }
                return;
            }
            if (mapId == MAP.VoDangPhuBan)
            {
                if (TLBB.MapId != MAP.VoDang)
                {
                    GoToEx(NPC.TieuThienDat.X, NPC.TieuThienDat.Y, NPC.TieuThienDat.Map);
                }
                else
                {
                    if (TDT.GetDistance(CharX, CharY, NPC.TieuThienDat.X, NPC.TieuThienDat.Y) > 3)
                    {
                        Move(NPC.TieuThienDat.X, NPC.TieuThienDat.Y);
                    }
                    else
                    {
                        if (TLBB.IsQuestOpen)
                        {
                            QuestFrame.ClickPhuBanMonPhai(this);
                        }
                        else
                        {
                            Talk(NPC.TieuThienDat.Id);
                        }
                    }
                }
                return;
            }
            if (mapId == MAP.ThienLongPhuBan)
            {
                if (TLBB.MapId != MAP.ThienLong)
                {
                    GoToEx(NPC.HoTuTruongLao.X, NPC.HoTuTruongLao.Y, NPC.HoTuTruongLao.Map);
                }
                else
                {
                    if (TDT.GetDistance(CharX, CharY, NPC.HoTuTruongLao.X, NPC.HoTuTruongLao.Y) > 3)
                    {
                        Move(NPC.HoTuTruongLao.X, NPC.HoTuTruongLao.Y);
                    }
                    else
                    {
                        if (TLBB.IsQuestOpen)
                        {
                            QuestFrame.ClickPhuBanMonPhai(this);
                        }
                        else
                        {
                            Talk(NPC.HoTuTruongLao.Id);
                        }
                    }
                }
                return;
            }
            if (mapId == MAP.TieuDaoPhuBan)
            {
                if (TLBB.MapId != MAP.TieuDao)
                {
                    GoToEx(NPC.CongDaTuTruong.X, NPC.CongDaTuTruong.Y, NPC.CongDaTuTruong.Map);
                }
                else
                {
                    if (TDT.GetDistance(CharX, CharY, NPC.CongDaTuTruong.X, NPC.CongDaTuTruong.Y) > 3)
                    {
                        Move(NPC.CongDaTuTruong.X, NPC.CongDaTuTruong.Y);
                    }
                    else
                    {
                        if (TLBB.IsQuestOpen)
                        {
                            QuestFrame.ClickPhuBanMonPhai(this);
                        }
                        else
                        {
                            Talk(NPC.CongDaTuTruong.Id);
                        }
                    }
                }
                return;
            }
            if (mapId == MAP.NgaMyPhuBan)
            {
                if (TLBB.MapId != MAP.NgaMy)
                {
                    GoToEx(NPC.LieuTamMuoi.X, NPC.LieuTamMuoi.Y, NPC.LieuTamMuoi.Map);
                }
                else
                {
                    if (TDT.GetDistance(CharX, CharY, NPC.LieuTamMuoi.X, NPC.LieuTamMuoi.Y) > 3)
                    {
                        Move(NPC.LieuTamMuoi.X, NPC.LieuTamMuoi.Y);
                    }
                    else
                    {
                        if (TLBB.IsQuestOpen)
                        {
                            QuestFrame.ClickPhuBanMonPhai(this);
                        }
                        else
                        {
                            Talk(NPC.LieuTamMuoi.Id);
                        }
                    }
                }
                return;
            }
            if (mapId == MAP.TinhTucPhuBan)
            {
                if (TLBB.MapId != MAP.TinhTuc)
                {
                    GoToEx(NPC.ThienToanTu.X, NPC.ThienToanTu.Y, NPC.ThienToanTu.Map);
                }
                else
                {
                    if (TDT.GetDistance(CharX, CharY, NPC.ThienToanTu.X, NPC.ThienToanTu.Y) > 3)
                    {
                        Move(NPC.ThienToanTu.X, NPC.ThienToanTu.Y);
                    }
                    else
                    {
                        if (TLBB.IsQuestOpen)
                        {
                            QuestFrame.ClickPhuBanMonPhai(this);
                        }
                        else
                        {
                            Talk(NPC.ThienToanTu.Id);
                        }
                    }
                }
                return;
            }
            if (mapId == MAP.ThienSonPhuBan)
            {
                if (TLBB.MapId != MAP.ThienSon)
                {
                    GoToEx(NPC.DangBa.X, NPC.DangBa.Y, NPC.DangBa.Map);
                }
                else
                {
                    if (TDT.GetDistance(CharX, CharY, NPC.DangBa.X, NPC.DangBa.Y) > 3)
                    {
                        Move(NPC.DangBa.X, NPC.DangBa.Y);
                    }
                    else
                    {
                        if (TLBB.IsQuestOpen)
                        {
                            QuestFrame.ClickPhuBanMonPhai(this);
                        }
                        else
                        {
                            Talk(NPC.DangBa.Id);
                        }
                    }
                }
                return;
            }
            if (mapId == MAP.MoDungPhuBan)
            {
                if (TLBB.MapId != MAP.MoDung)
                {
                    GoToEx(NPC.CongDaKhon.X, NPC.CongDaKhon.Y, NPC.CongDaKhon.Map);
                }
                else
                {
                    if (TDT.GetDistance(CharX, CharY, NPC.CongDaKhon.X, NPC.CongDaKhon.Y) > 3)
                    {
                        Move(NPC.CongDaKhon.X, NPC.CongDaKhon.Y);
                    }
                    else
                    {
                        if (TLBB.IsQuestOpen)
                        {
                            QuestFrame.ClickPhuBanMonPhai(this);
                        }
                        else
                        {
                            Talk(NPC.CongDaKhon.Id);
                        }
                    }
                }
                return;
            }
            if (mapId == MAP.DuongMonPhuBan)
            {
                if (TLBB.MapId != MAP.DuongMon)
                {
                    GoToEx(NPC.DuongMoTuong.X, NPC.DuongMoTuong.Y, NPC.DuongMoTuong.Map);
                }
                else
                {
                    if (TDT.GetDistance(CharX, CharY, NPC.DuongMoTuong.X, NPC.DuongMoTuong.Y) > 3)
                    {
                        Move(NPC.DuongMoTuong.X, NPC.DuongMoTuong.Y);
                    }
                    else
                    {
                        if (TLBB.IsQuestOpen)
                        {
                            QuestFrame.ClickPhuBanMonPhai(this);
                        }
                        else
                        {
                            Talk(NPC.DuongMoTuong.Id);
                        }
                    }
                }
                return;
            }
            if (TLBB.MapId == MAP.PhungHoangCoThanh)
            {
                if (GoToEx(PHUNGHOANGCOTHANH.HoangLongThien))
                {
                    if (TLBB.IsQuestOpen)
                    {
                        QuestFrameOptionClicked(403007, 1);
                        CloseQuest();
                    }
                    else
                    {
                        Talk("hoanglongthien");
                    }
                }
                return;
            }
            if (TLBB.PlayerState == 2)
            {
                return;
            }
            if (mapId == MAP.ViemLaThien)
            {
                if (GoToEx(PHUNGMINHTRAN.SaoVanKhue))
                {
                    if (TLBB.IsQuestOpen)
                    {
                        QuestFrame.Click("#{MZBUILD_150817_262}");
                        CloseQuest();
                    }
                    else
                    {
                        Talk(PHUNGMINHTRAN.SaoVanKhue);
                    }
                }
                return;
            }
            if (mapId == MAP.PhungHoangCoThanh)
            {
                if (GoToEx(THUCHACOTRAN.LyDa))
                {
                    if (TLBB.IsQuestOpen)
                    {
                        QuestFrameOptionClicked(403001, 1);
                        CloseQuest();
                    }
                    else
                    {
                        Talk(THUCHACOTRAN.LyDa.Id);
                    }
                }
                return;
            }
            swAutoMove = Stopwatch.StartNew();

            PostMessage((int)x, 50);
            PostMessage((int)y, 51);
            PostMessage(mapId, 52);
            PostMessage(10, 105);
        }

        public bool IsWaitDoiNgu { get; set; } = true;

        public string NeedToMove { get; set; } = string.Empty;

        private Stopwatch swAutoMove = Stopwatch.StartNew();

        public bool Move(float x, float y)
        {
            if (TLBB.ON_SCENE_TRANSING || IsChangeMap)
            {
                return false;
            }
            if (swAutoMove.Elapsed.TotalSeconds < 3)
            {
                return false;
            }
            int curRound = GetRound(RoundX, RoundY);
            curRound = RoundIdx;
            int nextRound = GetRound(x, y);
            nextRound = RoundIdx;
            if (TLBB.MapId == MAP.HoangLongDong)
            {
                int toRound = GetRound(x, y);
                if (curRound == toRound)
                {
                }
                else
                {
                    if (curRound < toRound)
                    {
                        x = 221;
                        y = 205;
                    }
                    else
                    {
                        x = 215;
                        y = 165;
                    }
                }
            }
            if (TLBB.MapId == MAP.VanKiemCoc || TLBB.MapId == MAP.VanKiemCocDem)
            {
                if (TDT.GetDistance(CharX, CharY, 110, 110) < 20)
                {
                    foreach (GameObject _object in Objects.All)
                    {
                        if ((int)_object.X == 107 && ((int)_object.Y == 110 || (int)_object.Y == 111))
                        {
                            Talk(_object.Id);
                        }
                    }
                    if (TLBB.IsQuestOpen)
                    {
                        QuestFrame.ClickPhuBanMonPhai(this);
                    }
                }
            }
            if (TDT.GetDistance(CharX, CharY, x, y) < 1.5)
                return true;

            if (TLBB.MapId == MAP.ThieuThatSon)
            {
                if (TDT.GetDistance(x, y, THIEUTHATSON.MoDungPhuc.X, THIEUTHATSON.MoDungPhuc.Y) < 30)
                {
                    if (RoundX >= 97 && RoundX <= 159 && RoundY >= 95)
                    {
                        x = THIEUTHATSON.TrangTuHien.X;
                        y = THIEUTHATSON.TrangTuHien.Y;
                    }
                    if (RoundX < 97 && RoundY > 95)
                    {
                        x = 54;
                        y = 88;
                    }
                }
                if (TDT.GetDistance(x, y, THIEUTHATSON.DinhXuanThu.X, THIEUTHATSON.DinhXuanThu.Y) < 30)
                {
                    if (RoundX >= 172 && RoundY <= 107)
                    {
                        x = 212;
                        y = 122;
                    }
                    if (RoundX >= 212)
                    {
                        x = 210;
                        y = 175;
                    }
                }
            }
            if (TLBB.MapId == MAP.YenTuO)
            {
                if (TLBB.PlayerState == 0)
                {
                    if (Address.GameType != 1)
                    {
                        if (TDT.GetDistance(x, y, YENTUO.HoDienBao.X, YENTUO.HoDienBao.Y) < 10 && GetDistance(YENTUO.HoDienBao.X, YENTUO.HoDienBao.Y) > 10)
                        {
                            List<string> list = new List<string>()
                        {
                            "171,232",
                            "178,212",
                            "178,203,fly",
                            "171,200",
                            "171,187,fly",
                            "155,178",
                            "143,180,fly",
                            "127,175",
                            "115,178,fly",
                            "97,182",
                            "93,194,fly",
                        };
                            float minDistance = 999;
                            int curPoint = -1;
                            if (curPoint == -1)
                            {
                                for (int i = 0; i < list.Count; i++)
                                {
                                    float distance = GetDistance(list[i]);
                                    if (distance < minDistance)
                                    {
                                        minDistance = distance;
                                        curPoint = i;
                                    }
                                }
                            }
                            if (IdleTime > 6)
                                FixKetMap();
                            if (curPoint != list.Count - 1)
                            {
                                if (list[curPoint + 1].Split(',').Length > 2 || swStandTime.Elapsed.TotalSeconds > 5)
                                    TLBB.Fly(list[curPoint + 1]);
                                else
                                    Move(list[curPoint + 1]);
                                return false;
                            }
                        }
                        // 195,155
                        if ((TDT.GetDistance(x, y, YENTUO.TienHoanhVu.X, YENTUO.TienHoanhVu.Y) < 10 && GetDistance(YENTUO.TienHoanhVu.X, YENTUO.TienHoanhVu.Y) > 10) || (TDT.GetDistance(x, y, YENTUO.MoDungPhuc.X, YENTUO.MoDungPhuc.Y) < 10) && GetDistance(YENTUO.MoDungPhuc.X, YENTUO.MoDungPhuc.Y) > 10)
                        {
                            List<string> list = new List<string>()
                        {
                            "79,202",
                            "92,194",
                            "97,181,fly",
                            "124,181",
                            "134,176,fly",
                            "145,172,fly",
                            "162,167",
                            "172,158,fly",
                            "191,158,fly",
                        };
                            if (IdleTime > 3 && IdleTime % 3 == 0)
                            {
                                list = new List<string>()
                            {
                                "171,232",
                                "177,212",
                                "178,203,fly",
                                "169,201",
                                "170,186,fly",
                                "156,176",
                                "162,167,fly",
                                "172,158,fly",
                                "191,158,fly",
                            };
                            }
                            float minDistance = 999;
                            int curPoint = -1;
                            if (curPoint == -1)
                            {
                                for (int i = 0; i < list.Count; i++)
                                {
                                    float distance = GetDistance(list[i]);
                                    if (distance < minDistance)
                                    {
                                        minDistance = distance;
                                        curPoint = i;
                                    }
                                }
                            }
                            if (IdleTime > 6)
                                FixKetMap();
                            if (curPoint != list.Count - 1)
                            {
                                if (list[curPoint + 1].Split(',').Length > 2)
                                    TLBB.Fly(list[curPoint + 1]);
                                else
                                    Move(list[curPoint + 1]);
                                return false;
                            }
                        }
                    }
                }
                if (x < 165 && y < 125)
                {
                    if (RoundX > 170 || RoundY > 125)
                    {
                        x = YENTUO.HoaHachCan.X;
                        y = YENTUO.HoaHachCan.Y;
                    }
                }
                if (x == YENTUO.HoaHachCan.X && y == YENTUO.HoaHachCan.Y)
                {
                    if (GetDistance(YENTUO.HoaHachCan.X, YENTUO.HoaHachCan.Y) <= 6)
                    {
                        if (TLBB.IsQuestOpen)
                        {
                            QuestFrame.ClickAll();
                        }
                        else
                        {
                            foreach (GameObject _object in Objects.All)
                            {
                                if (TDT.VietLien(_object.Name) == "hoahachcan" || TDT.VietLien(_object.Name) == "officerhua")
                                {
                                    Talk(_object.Id);
                                    break;
                                }
                            }
                        }
                        return false;
                    }
                }
            }

            if (TLBB.MapId == MAP.TuTuyetTrang)
            {
                int curround = GetRoundTuTuyet(RoundX, RoundY);
                int toround = GetRoundTuTuyet((int)x, (int)y);
                if (curround < toround)
                {
                    NPC phan = PhanThanhThanh[curround - 1];
                    if (GoToEx(phan))
                    {
                        IsP = true;
                    }
                    return false;
                }
            }

            //if ((TLBB.MapId >500 && IsNhatHopQDua && !IsQDua ) || (TLBB.MapId > 500 && IsNhatHopQDua && TrangThaiQD=="NhatQua"))
            //{
            //    if (x > 49 && y>66)
            //    {
            //        x = 49;
            //    }
            //    if (y > 82)
            //    {
            //        y = 82;
            //    }
            //    if (x > 50 && y > 65 && y<=66)
            //    {
            //        x = 51;
            //        y = 64;
            //    }

            //}
            LUA.Move(x, y);
            //ve(x, y);
            return false;
        }

        public NPC[] PhanThanhThanh = new NPC[]
        {
            new NPC(){X = 96, Y = 79, Map = -1},
            new NPC(){X = 35, Y = 87, Map = -1},
            new NPC(){X = 84, Y = 23, Map = -1},
            new NPC(){X = 23, Y = 17, Map = -1},
        };

        public int GetRoundTuTuyet(int x, int y)
        {
            Point s = new Point(x, y);

            if (TDT.is_point_inside_quad(s, new Point(82, 76), new Point(127, 76), new Point(127, 127), new Point(82, 127)))
            {
                return 1;
            }

            if (TDT.is_point_inside_quad(s, new Point(0, 84), new Point(82, 84), new Point(82, 127), new Point(0, 127)))
                return 2;

            if (TDT.is_point_inside_penta(s, new Point(29, 79), new Point(29, 84), new Point(44, 84), new Point(90, 60), new Point(80, 50)))
                return 3;
            if (TDT.is_point_inside_quad(s, new Point(80, 0), new Point(127, 0), new Point(127, 60), new Point(80, 60)))
                return 3;

            if (TDT.is_point_inside_penta(s, new Point(0, 0), new Point(80, 0), new Point(80, 50), new Point(29, 79), new Point(0, 79)))
                return 4;

            return 0;
        }




        public bool IsThieuThatSonEx
        {
            get
            {
                if (Leader == null)
                    return false;
                if (Leader.Missions.Contains(MissionsType.DatDoiThieuThatSon))
                    return true;
                return false;
            }
        }

        public void SelectTarget(int targetId)
        {
            TargetId = targetId;
            PostMessage(targetId, 100);
        }

        public void SelectTarget(uint targetId) => SelectTarget((int)targetId);

        private bool IsPK { get; set; }

        public void GetBestTarget()
        {
            BestTarget = null;

            if (Global.AtkFollowKey && Objects.Key != null && !TLBB.IsLeader)
            {
                if (Objects.Key.State == 7)
                {                    
                    foreach (GameObject obj in Objects.All)
                    {
                        if (obj.Id == Objects.Key.AtkToId)
                        {
                            BestTarget = obj;
                            return;
                        }
                    }
                }
            }

            if (TLBB.MapId == MAP.TangKinhCac)
            {
                BestTarget = Objects.Monters.OrderByDescending(o => o.Name == "Đạo Thư Ác Tăng").ThenByDescending(o => !AllPartyTargetId.Contains(o.Id)).ThenByDescending(o => o.HP == 1).ThenBy(o => o.GetDistance(62, 100)).FirstOrDefault();
                if (BestTarget != null)
                    return;
            }
            if (TLBB.MapId == MAP.LoiDaiSinhTu)
            {
                BestTarget = Objects.Monters.Where(o => o.Name == "Công Tôn Thánh" || o.Name == SatTinhMonter).OrderByDescending(o => o.HP).FirstOrDefault();
                if (BestTarget != null)
                {
                    return;
                }
            }
         
            if (Objects.Pk.Count > 0)
            {
                IsPK = true;
                BestTarget = Objects.Pk.OrderByDescending(o => Main.IsPKNM && o.Menpai == 5).ThenBy(o => o.HP).FirstOrDefault();
                return;
            }
            else
            {
                IsPK = false;
            }

        

            if (TLBB.MapId == MAP.YenTuO)
            {
                foreach (GameObject obj in Objects.Monters)
                {
                    if (obj.CleanName == "doandienkhanh" || obj.CleanName == "nhaclaotam" || obj.CleanName == "diepnhinuong" || obj.CleanName == "crueltuan" || obj.CleanName == "laosanyue" || obj.CleanName == "crazymother")
                    {
                        BestTarget = obj;
                        return;
                    }
                }
            }

            BestTarget = Objects.Monters.OrderByDescending(o => GAMEDIC.UuTien.Contains(o.Name)).ThenByDescending(o => IsLureEx && o.Belong.Contains("FFFFFFFF") && !AllPartyTargetId.Contains(o.Id)).ThenByDescending(o => TLBB.MineIds.Contains(o.Belong)).ThenBy(o => o.Distance).ThenBy(o => o.HP).FirstOrDefault();
        }

        private void SendKey()
        {
            if (swXuaPet.Elapsed.TotalSeconds < 5)
                return;
            if (TLBB.MapId == MAP.YenTuO)
            {
                if (Objects.Have("modungphuc") || Objects.Have("furong"))
                {
                    return;
                }
            }
            if (TLBB.IsFollow || !IsAuto || TLBB.IsRide || TLBB.IsBienThan || TLBB.BusyEx)
                return;
            if (IsAuto && !Global.Paused && !TLBB.IsFollow && !TLBB.IsRide)
            {
                for (int i = 0; i < 12; i++)
                {
                    if (F[i] && SecCount % KeyDelay[i] == 0)
                    {
                        _DoSkill(i);
                    }
                }
                for (int i = 0; i < 10; i++)
                {
                    if (Alt[i] && SecCount % KeyDelay[i + 12] == 0)
                    {
                        _DoSkill(i + 20);
                    }
                }
            }
        }


        public static Game CurGomDo { get; set; }

        public static Game CurGomDoThuong { get; set; }

        public bool HaveItemKNBGom
        {
            get
            {
                if (Process.HasExited)
                {
                    return false;
                }
                DoStringEx("setmetatable(_G, { __index = Packet_Env}); if not this:IsVisible() then PushEvent('TOGLE_CONTAINER'); end");
                if (TLBB.OnlineTimeSec <= 5)
                    return true;
                foreach (var item in PacketItems.All.Concat(PacketItems.ThienCo).Where(i => !i.IsCoDinh))
                {
                    if (HaveGomKNBName.Contains(item.Name.Trim()) || HaveGomKNBName.Contains(item.TypeName.Trim()))
                        return true;
                }
                return false;
            }
        }

        public bool HaveItemGom
        {
            get
            {
                if (Process.HasExited)
                {
                    return false;
                }
                if (TLBB.Gold > 0 && Missions.Contains(MissionsType.GomVang))
                    return true;
                if (Missions.Contains(MissionsType.GomKNB))
                {
                    if (PacketItems.DaoCu.Any(i => i.Name == "Phiếu Kim Nguyên Bảo"))
                        return true;
                    if (TLBB.KNB > 0)
                        return true;
                    else
                        return false;
                }

                DoStringEx("setmetatable(_G, { __index = Packet_Env}); if not this:IsVisible() then PushEvent('TOGLE_CONTAINER'); end");
                if (TLBB.OnlineTimeSec <= 5)
                    return true;

                if (Setting.Is("checkKhiGomDo"))
                {
                    foreach (var item in PacketItems.All.Concat(PacketItems.ThienCo).Where(i => !i.IsCoDinh))
                    {
                        if (Missions.Contains(MissionsType.DuaTangBaoDo))
                        {
                            if (item.Name == "Tàng Bảo Đồ")
                                return true;
                        }
                        if (Missions.Contains(MissionsType.DuaBaoDoHiem))
                        {
                            if (item.Name == "Bảo Đồ Hiếm")
                                return true;
                        }
                        else
                        {
                            if (HaveGomName.Contains(item.Name))
                                return true;
                            if (HaveGomName.Contains(item.TypeName))
                                return true;
                        }
                    }
                }
                else
                {
                    foreach (var item in PacketItems.All.Where(i => !i.IsCoDinh))
                    {
                        if (item.Name == "Quà Tân Thiên Long")
                            return true;
                        if (Missions.Contains(MissionsType.DuaTangBaoDo))
                        {
                            if (item.Name == "Tàng Bảo Đồ")
                                return true;
                        }
                        if (Missions.Contains(MissionsType.DuaBaoDoHiem))
                        {
                            if (item.Name == "Bảo Đồ Hiếm")
                                return true;
                        }
                        else
                        {
                            if (HaveGomName.Contains(item.Name))
                                return true;
                            if (HaveGomName.Contains(item.TypeName))
                                return true;
                        }
                    }
                }
                return false;
            }
        }


        private void NhanTiemNangTan()
        {
            if (Global.Paused)
                return;
            if (!Global.IsGom)
                return;
            if (!IsOneSec)
                return;
            if (Main.GomDoName != TLBB.Name)
                return;
            if (CurGomDoThuong != null)
            {
                if (!Main.Instance.AllOnelineGame.Contains(CurGomDo) || !CurGomDo.TLBB.Online)
                {
                    CurGomDoThuong = null;
                }
                else
                {
                    if (CurGomDoThuong == this || !CurGomDoThuong.Missions.Contains(MissionsType.GomDo))
                    {
                        CurGomDoThuong = null;
                    }
                    else
                    {
                        if (!CurGomDoThuong.HaveItemGom)
                        {
                            CurGomDoThuong.RemoveMission(MissionsType.GomDo);
                            CurGomDoThuong.RemoveMission(MissionsType.DuaBaoDoHiem);
                            CurGomDoThuong.RemoveMission(MissionsType.DuaTangBaoDo);
                            CurGomDoThuong.RemoveMission(MissionsType.GomKNB);
                            Win.Hide(CurGomDo.Handle);
                            if (CurGomDoThuong.IsByLogin)
                                CurGomDoThuong.IsXongBHD = true;
                            CurGomDoThuong = null;
                        }
                    }
                }
            }
            if (CurGomDoThuong == null)
            {
                foreach (var game in Main.Instance.AllOnelineGame.Where(g => g.Missions.Contains(MissionsType.GomDo) && g.SafeTime > 0))
                {
                    if (game == this)
                        continue;
                    if (game.HaveItemGom)
                    {
                        CurGomDoThuong = game;
                        break;
                    }
                    else
                    {
                        game.RemoveMission(MissionsType.DuaBaoDoHiem);
                        game.RemoveMission(MissionsType.DuaTangBaoDo);
                        game.RemoveMission(MissionsType.GomDo);
                        game.RemoveMission(MissionsType.GomKNB);
                        if (game.IsByLogin)
                            game.IsXongBHD = true;
                    }
                }
            }
            if (CurGomDoThuong == null)
                return;

            if (CurGomDoThuong.GoToEx(CharX, CharY, (int)TLBB.MapId))
            {
                Stopwatch tradetime = Stopwatch.StartNew();
                DoStringEx("Exchange:ExchangeCancel(); Exchange:CloseExchangeInfo()");
                CurGomDoThuong.DoStringEx("Exchange:ExchangeCancel(); Exchange:CloseExchangeInfo()");
                if (Win.IsHideOrMini(CurGomDoThuong.Handle))
                {
                    Win.ShowInactive(CurGomDoThuong);
                    Thread.Sleep(1000);
                }
                Thread.Sleep(1000);
                int laycnt = 0;
                if (Setting.Is("checkKhiGomDo"))
                {
                    foreach (PacketItem item in CurGomDoThuong.PacketItems.ThienCo.Where(i => !i.IsCoDinh))
                    {
                        if (CurGomDoThuong.Missions.Contains(MissionsType.DuaTangBaoDo))
                        {
                            if (item.Name == "Tàng Bảo Đồ")
                            {
                                CurGomDoThuong.GetItemThienCo(item.Index);
                                if (laycnt++ >= 5)
                                    break;
                            }
                        }
                        else if (CurGomDoThuong.Missions.Contains(MissionsType.DuaBaoDoHiem))
                        {
                            if (item.Name == "Bảo Đồ Hiếm")
                            {
                                CurGomDoThuong.GetItemThienCo(item.Index);
                                if (laycnt++ >= 5)
                                    break;
                            }
                        }
                        else if(!CurGomDoThuong.Missions.Contains(MissionsType.GomKNB))
                        {
                            if (HaveGomName.Contains(item.Name) || HaveGomName.Contains(item.TypeName))
                            {
                                CurGomDoThuong.GetItemThienCo(item.Index);
                                if (laycnt++ >= 5)
                                    break;
                            }
                        }
                    }
                }
                foreach (GameObject _object in Objects.All)
                {
                    if (_object.Name == CurGomDoThuong.TLBB.Name)
                    {
                        SendExchange((int)_object.Id);
                        Thread.Sleep(2000);
                        CurGomDoThuong.DoStringEx("setmetatable(_G, {__index = MainMenuBar_Env}); MainMenuBar_Exchange_Clicked();");
                        Thread.Sleep(1000);
                        break;
                    }
                }
                if (CurGomDoThuong.Missions.Contains(MissionsType.GomKNB))
                {
                    CurGomDoThuong.DoStringEx("Player:YuanBaoToTicket(" + CurGomDoThuong.TLBB.KNB + ")");
                    Thread.Sleep(350);
                }
                int cnt = 0;
                if (CurGomDoThuong.TLBB.IsExchangeOpen)
                {
                    foreach (var item in CurGomDoThuong.PacketItems.All.Where(i => !i.IsCoDinh))
                    {
                        if (CurGomDoThuong.Missions.Contains(MissionsType.GomKNB))
                        {
                            if (item.Name == "Phiếu Kim Nguyên Bảo")
                            {
                                if (cnt++ >= 5)
                                    break;
                                item.DoSubAction();
                                Thread.Sleep(500);
                            }
                            else
                            {
                                continue;
                            }
                        }
                        if (CurGomDoThuong.Missions.Contains(MissionsType.DuaTangBaoDo))
                        {
                            if (item.Name == "Tàng Bảo Đồ")
                            {
                                if (cnt++ >= 5)
                                    break;
                                item.DoSubAction();
                                Thread.Sleep(500);
                            }
                        }
                        else if (CurGomDoThuong.Missions.Contains(MissionsType.DuaBaoDoHiem))
                        {
                            if (item.Name == "Bảo Đồ Hiếm")
                            {
                                if (cnt++ >= 5)
                                    break;
                                item.DoSubAction();
                                Thread.Sleep(500);
                            }
                        }
                        else
                        {
                            if (HaveGomName.Contains(item.Name) || HaveGomName.Contains(item.TypeName))
                            {
                                if (cnt++ >= 5)
                                    break;
                                item.DoSubAction();
                                Thread.Sleep(500);
                            }
                        }
                    }
                    if (CurGomDoThuong.TLBB.Gold > 0 && CurGomDoThuong.Missions.Contains(MissionsType.GomVang))
                    {
                        CurGomDoThuong.DoStringEx("Exchange:GetMoneyFromInput(" + CurGomDoThuong.TLBB.Gold + ");");
                    }
                    Thread.Sleep(1000);
                    CurGomDoThuong.DoStringEx("setmetatable(_G, {__index = Exchange_Env}); if this:IsVisible() then Exchange_Lock_Button_Clicked(); end");
                    Thread.Sleep(1000);
                    DoStringEx("setmetatable(_G, {__index = Exchange_Env}); if this:IsVisible() then Exchange_Lock_Button_Clicked(); end");
                    Thread.Sleep(2500);
                    DoStringEx("setmetatable(_G, {__index = Exchange_Env});  if this:IsVisible() then Trade_Accept_Button_Clicked(); end");
                    Thread.Sleep(2500);
                    CurGomDoThuong.DoStringEx("GOM = \"" + Main.GomDoName + "\"; " + "setmetatable(_G, {__index = Exchange_Env});  if this:IsVisible() then if string.find(Exchange:GetOthersName(), GOM)  then Trade_Accept_Button_Clicked(); else	setmetatable(_G, { __index = Exchange_Env}); Exchange_Cancel(); end end");
                    Thread.Sleep(1000);
                }
            }
        }

        private void NhanTiemNangTanKNB()
        {
            if (Global.Paused)
                return;
            if (!Global.IsGomKNB)
                return;
            if (!IsOneSec)
                return;
            if (Main.GomDoName != TLBB.Name)
                return;

            if (CurGomDo != null)
            {
                if (!CurGomDo.TLBB.Online || CurGomDo == this || !CurGomDo.Missions.Contains(MissionsType.GomDoKNB))
                {
                    CurGomDo = null;
                    return;
                }
                if (!CurGomDo.HaveItemKNBGom)
                {
                    if (CurGomDo.IsByLogin)
                        CurGomDo.IsXongBHD = true;
                    else
                        CurGomDo.RemoveMission(MissionsType.GomDoKNB);
                    Win.Hide(CurGomDo.Handle);
                    CurGomDo = null;
                    return;
                }
            }

            if (CurGomDo == null)
            {
                foreach (var game in Main.Instance.AllOnelineGame.Where(g => g.TLBB.OnlineTimeSec > 10 && g.SafeTime > 0 && g != this && g.Missions.Contains(MissionsType.GomDoKNB)).OrderByDescending(o => o.GetDistance(CharX, CharY)))
                {
                    if (game.HaveItemKNBGom)
                    {
                        game.GoToEx(CharX, CharY, (int)TLBB.MapId);
                        CurGomDo = game;
                    }
                    else
                    {
                        if (game.TLBB.PlayerState == 10)
                        {
                            game.RemoveStall();
                        }
                        else
                        {
                            if (game.IsByLogin)
                                game.IsXongBHD = true;
                            else
                                game.RemoveMission(MissionsType.GomDoKNB);
                        }
                    }
                }
            }

            if (CurGomDo == null)
                return;

            List<Game> listStall = Main.Instance.AllOnelineGame.Where(g => g != CurGomDo).Where(g => g.GetDistance(CharX, CharY) < 10).Where(g => g.TLBB.State == 10).ToList();

            if(listStall.Count > 0)
            {
                listStall.ForEach(g => Task.Run(() => { g.RemoveStall(); }));
                Thread.Sleep(2000);
            }

            if (CurGomDo.GoToEx(CharX, CharY, (int)TLBB.MapId))
            {
                CurGomDo.StopFollow();
                if (Win.IsHideOrMini(CurGomDo.Handle))
                {
                    Win.ShowInactive(CurGomDo);
                }
                if (CurGomDo.TLBB.IsRide)
                {
                    CurGomDo.DownRide();
                    Thread.Sleep(350);
                }
                if (CurGomDo.TLBB.PetHPPercent > 0)
                {
                    CurGomDo.ThuPet();
                    Thread.Sleep(350);
                }
                if (CurGomDo.TLBB.State != 10)
                {
                    CurGomDo.DoStringEx("setmetatable(_G, {__index = StallSale_Env}); if not this:IsVisible() then PlayerPackage:OpenStallSaleFrame(); end");
                    Thread.Sleep(500);
                    CurGomDo.LUA.MessageBox_Self_OK_Clicked();
                    Thread.Sleep(500);                    
                }

                CurGomDo.PacketItems.ThienCo.Where(i => !i.IsCoDinh).Where(i => HaveGomKNBName.Contains(i.Name) || HaveGomKNBName.Contains(i.TypeName)).Take(5).ForEach(i => CurGomDo.GetItemThienCo(i.Index));          

                if (CurGomDo.TLBB.State == 10)
                {
                    foreach (GameObject obj in Objects.All)
                    {
                        if (obj.Name == CurGomDo.TLBB.Name)
                        {
                            PostMessage((int)obj.Id, 130);
                            break;
                        }
                    }

                    foreach (var item in CurGomDo.PacketItems.All.Where(i => !i.IsCoDinh))
                    {
                        if (HaveGomKNBName.Contains(item.Name) || HaveGomKNBName.Contains(item.Type.Trim()))
                        {
                            item.DoSubAction();
                            Thread.Sleep(700);
                            DoStringEx(@"setmetatable(_G, {__index = MessageBox_Self_Env});
                                        STR = MessageBox_Self_Text:GetText();
                                        if string.find(STR, '?,?kh') then
                                            setmetatable(_G, { __index = MessageBox_Self_Env});
                                            MessageBox_Self_Cancel_Clicked(1);
                                        end
                                        return STR");
                            CurGomDo.DoStringEx("setmetatable(_G, {__index = InputYuanbao_Env}); if this:IsVisible() then InputYuanbao_EditBox:SetText('1');  end");
                            Thread.Sleep(700);
                            CurGomDo.DoStringEx("setmetatable(_G, { __index = InputYuanbao_Env}); if this:IsVisible() then InputYuanbao_OnOK(); end");
                            Thread.Sleep(700);
                            break;
                        }
                    }
                    DoStringEx("PushEvent('STALL_BUY_SELECT',0)");
                    Thread.Sleep(500);
                    DoStringEx("setmetatable(_G, {__index = StallBuy_Env}); if this:IsVisible() and StallBuy:GetPrice('item', 0) == 1 then StallBuy_Buy_Clicked(); end");
                    Thread.Sleep(500);
                    DoStringEx("setmetatable(_G, {__index = MessageBoxCommon_Env}); if this:IsVisible() then MessageBoxCommon_Ok_Clicked(); end");
                }             
            }
            else
            {
                if (CurGomDo.TLBB.State == 10)
                {
                    CurGomDo.RemoveStall();
                }
            }
        }

        public void RemoveStall()
        {
            DoStringEx("StallSale:ConfirmRemoveStall();");
            Thread.Sleep(500);
            LUA.MessageBox_Self_OK_Clicked();
            Thread.Sleep(500);
            LUA.MessageBox_Self_OK_Clicked();
            Thread.Sleep(500);
        }

        private Stopwatch AskFollowTime = Stopwatch.StartNew();

        private void HoTroPhuBan()
        {
            bool isHotro = Setting.Is("checkHoTro");
            if (TLBB.MapId == MAP.BinhThanhKyTran)
            {
                GameObject objThuyTich = Objects.All.Where(obj => obj.Name.Contains("Thủy Tích")).FirstOrDefault();
                if (objThuyTich != null && (objThuyTich.Distance > 2 || TLBB.IsNgoaiCong))
                {
                    Move(objThuyTich.X, objThuyTich.Y);
                    NeBayTime = Stopwatch.StartNew();
                }
            }

            if (TLBB.MapId == MAP.ThieuThatSon || TLBB.MapId == MAP.YenTuO || TLBB.MapId == MAP.TuTuyetTrang || TLBB.MapId == MAP.TamTaiHiepCoc || TLBB.MapId == MAP.ViemMaSon)
            {
                if (Objects.Monters.Count > 0)
                {
                    if (TLBB.IsRide && TrueStandTime.Elapsed.TotalSeconds > 2 && IsAuto)
                        DownRide();
                }
            }

            if (TLBB.MapId == MAP.ThieuThatSon)
            {
                if (IsThieuThatSonEx)
                {
                    GameObject tieuviensonmodungbac = Objects.All.Where(o => o.AtkToId == SelfId).Where(o => o.Name == "Tiêu Viễn Sơn" || o.Name == "Mộ Dung Bác").OrderByDescending(o => o.HP).OrderByDescending(o => o.AtkToId == SelfId).FirstOrDefault();
                    if (IsOnlyMoDungBac)
                        tieuviensonmodungbac = null;
                    if (TLBB.Menpai == MENPAI.NgaMy)
                    {
                        if (SwBuffNM.Elapsed.TotalSeconds < 3 && BuffNMPercent < 0.20)
                        {
                            tieuviensonmodungbac = null;
                        }
                    }
                    if (tieuviensonmodungbac != null)
                    {
                        GameObject objChanhTam = null;
                        if (tieuviensonmodungbac.Name == "Tiêu Viễn Sơn")
                            objChanhTam = Objects.All.Where(o => o.Name.Contains("Chánh Tâm") && o.Title.Contains("Tiêu Viễn Sơ")).OrderBy(o => o.Distance).FirstOrDefault();
                        else if (tieuviensonmodungbac.Name == "Mộ Dung Bác")
                            objChanhTam = Objects.All.Where(o => o.Name.Contains("Chánh Tâm") && o.Title.Contains("Mộ Dung Bác")).OrderBy(o => o.Distance).FirstOrDefault();
                        if (objChanhTam == null)
                        {
                            objChanhTam = Objects.All.Where(o => o.Name.Contains("Chánh Tâm")).OrderBy(o => o.Distance).FirstOrDefault();
                            if (objChanhTam != null)
                            {
                                if (objChanhTam.Title.Contains("Mộ Dung Bác"))
                                {
                                    tieuviensonmodungbac = Objects.All.Where(o => o.Name == "Mộ Dung Bác").OrderByDescending(o => o.HP).OrderByDescending(o => o.AtkToId == SelfId).FirstOrDefault();
                                }
                                else
                                {
                                    tieuviensonmodungbac = Objects.All.Where(o => o.Name == "Tiêu Viễn Sơn").OrderByDescending(o => o.HP).OrderByDescending(o => o.AtkToId == SelfId).FirstOrDefault();
                                }
                            }
                        }
                        if (objChanhTam != null && tieuviensonmodungbac != null)
                        {
                            float moveX = objChanhTam.X;
                            float moveY = objChanhTam.Y;
                            if (tieuviensonmodungbac.X < objChanhTam.X)
                            {
                                moveX = objChanhTam.X + 2;
                            }
                            else if (tieuviensonmodungbac.X > objChanhTam.X)
                            {
                                moveX = objChanhTam.X - 2;
                            }
                            if (tieuviensonmodungbac.Y < objChanhTam.Y)
                            {
                                moveY = objChanhTam.Y + 2;
                            }
                            else if (tieuviensonmodungbac.Y > objChanhTam.Y)
                            {
                                moveY = objChanhTam.Y - 2;
                            }
                            if (GetDistance(moveX, moveY) >= 1)
                            {
                                Move(moveX, moveY);
                                IsPhanDame = true;
                            }
                        }
                    }
                }
            }

            if (TLBB.MapId == MAP.YenTuO)
            {
                if (TLBB.HPPercent <= 50)
                {
                    IsPhanDame = true;
                }
                if (Objects.Monters.Where(_object => _object.Buff.Contains(1012)).FirstOrDefault() != null)
                {
                    IsPhanDame = true;
                    if (TLBB.IsLeader)
                    {
                        if (AskFollowTime.Elapsed.TotalSeconds >= 8)
                        {
                            AskFollowTime = Stopwatch.StartNew();
                            AskTeamFollow();
                            Thread.Sleep(2000);
                            StopFollow();
                        }
                    }
                    else
                    {
                        if (TLBB.PlayerState == 7)
                        {
                            if (TLBB.Menpai == MENPAI.NgaMy && Objects.Self.AtkToId != Objects.All.Where(_object => _object.Buff.Contains(1012)).FirstOrDefault().Id)
                            {
                            }
                            else
                            {
                                FixKetMap();
                            }
                        }
                    }
                }
            }
            if (TLBB.MapId == MAP.LanHoanPhucDia)
            {
                if (IsObjectDead("Hư Trúc"))
                {
                    if (TrueClearMonterTime.Elapsed.TotalSeconds >= 30 && SWHoiSinh.Elapsed.TotalSeconds >= 20)
                    {
                        if (GoToEx(83, 181))
                        {
                            TalkEx("Lý Thanh La");
                            Thread.Sleep(1000);
                            QuestFrame.Click("#{LHFD_160203_26}");
                        }
                    }
                }
            }
            if (TLBB.MapId == MAP.TranLongKyCuoc || TLBB.MapId == MAP.LauLanBaoTang || TLBB.MapId == MAP.ThanhThuSonPhuBan)
            {
                if (TLBB.IsRide && Objects.Monters.Count > 0)
                    DownRide();
                if (TLBB.MapId == MAP.TranLongKyCuoc)
                {
                    if (IsBossDie)
                    {
                        if (BossDieTime.Elapsed.TotalSeconds > 20)
                        {
                            GoToEx(TRANLONGKYCUOC.TeThanh);
                            IsP = true;
                            IsXongKyCuoc = true;
                        }
                        else
                        {
                            PushDebugMessageEx("Di chuyển sau " + (20 - BossDieTime.Elapsed.Seconds) + "s");
                        }
                    }
                }
            }
            if (TLBB.MapId == MAP.ThanhThuSonPhuBan)
            {
                if (Objects.Monters.Count > 0 && TLBB.IsFollow)
                    StopFollow();
                if (TLBB.IsRide && Objects.Monters.Count > 0)
                    DownRide();
                if (IsBossDie)
                {
                    if (BossDieTime.Elapsed.TotalSeconds > 30)
                    {
                        if (!TLBB.IsRide && TLBB.HaveRide)
                        {
                            UpRide();
                            return;
                        }
                        StopFollow();
                        GoToEx(220, 220, MAP.ThanhThuSon);
                    }
                }
            }
            if (TLBB.MapId == MAP.HuyenVuDaoPhuBan && (Leader != null && Leader.Missions.Contains(MissionsType.DatDoiThienGiangKyThu)))
            {
                foreach (GameObject _object in Objects.All)
                {
                    if ((_object.CleanName == "vodichphithienmieu" || _object.CleanName.Contains("thanthu")) && _object.HP > 0)
                    {
                        if (TLBB.IsRide)
                            DownRide();
                        if (TLBB.IsFollow && !TLBB.IsLeader)
                            StopFollow();
                    }
                }

                if (IsBossDie)
                {
                    if (BossDieTime.Elapsed.TotalSeconds > 20)
                    {
                        if (TLBB.IsFollow && !TLBB.IsLeader)
                            StopFollow();
                        GoToEx(70, 67);
                    }
                    else
                    {
                        PushDebugMessageEx("Di chuyển sau " + (20 - BossDieTime.Elapsed.Seconds) + "s");
                    }
                }
            }

            if (TLBB.MapId == MAP.TuTuyetTrang && (Leader != null && Leader.Missions.Contains(MissionsType.DatDoiTuTuyetTrang) || isHotro))
            {
                if (GetRoundTuTuyet(RoundX, RoundY) == 4)
                {
                    if (Objects.All.Where(o => o.IsTrap && o.Distance < 8).Count() > 0)
                    {
                        SafeX = SafeY = 0;
                        GetSafePoint();

                        if (SafeX != 0 && SafeY != 0)
                        {
                            NeBayTime = Stopwatch.StartNew();
                            Move(SafeX, SafeY);
                        }
                    }
                }
                if (GetRoundTuTuyet(RoundX, RoundY) != 4)
                {
                    float distance = 8;
                    if (GetRoundTuTuyet(RoundX, RoundY) == 2)
                        distance = 13;
                    foreach (GameObject _object in Objects.All.Where(o => o.IsTrap && o.Distance < distance))
                    {
                        SafeX = SafeY = 0;

                        GetSafePoint();
                        if (SafeX != 0 && SafeY != 0)
                        {
                            NeBayTime = Stopwatch.StartNew();
                            Move(SafeX, SafeY);
                        }
                    }
                }

                if (TLBB.IsQuestOpen)
                {
                    string QuestFrameText = QuestFrame.Text;
                    if (QuestFrameText.Contains("#{SJZYH_150824_4}") || GetRoundTuTuyet(RoundX, RoundY) == 4)
                    {
                        if (!DeadObjects.ContainsKey("Đào Thanh"))
                            DeadObjects["Đào Thanh"] = Stopwatch.StartNew();
                    }
                    if (QuestFrameText.Contains("#{SJZ_100129_62}") || QuestFrameText.Contains("#{SJZYH_150824_3}") || GetRoundTuTuyet(RoundX, RoundY) == 3)
                    {
                        if (!DeadObjects.ContainsKey("Tần Vận"))
                            DeadObjects["Tần Vận"] = Stopwatch.StartNew();
                    }
                    if (QuestFrameText.Contains("#{SJZ_100129_48}") || QuestFrameText.Contains("#{SJZYH_150824_2}") || GetRoundTuTuyet(RoundX, RoundY) == 2)
                    {
                        if (!DeadObjects.ContainsKey("Mẫn Mặc"))
                            DeadObjects["Mẫn Mặc"] = Stopwatch.StartNew();
                    }
                    foreach (QuestFrame quest in QuestFrame.Enum(this))
                    {
                        if (quest.Name.Contains("#{SJZYH_150824_1}") || quest.Name.Contains("#{SJZYH_150824_6}") || quest.Name.Contains("#{SJZYH_150824_5}"))
                        {
                            QuestFrameOptionClicked(quest);
                        }
                        if (quest.Name.Contains("#{SJZ_100129_72}"))
                        {
                            if (!DeadObjects.ContainsKey("Bàng Xí"))
                                DeadObjects["Bàng Xí"] = Stopwatch.StartNew();
                        }
                    }
                }
                if (!IsHoiSinh)
                {
                    if (GetRoundTuTuyet(RoundX, RoundY) == 4 && PartyMinSwBuffNM >= 3)
                    {
                        if (TrueClearMonterTime.Elapsed.TotalSeconds >= 3)
                        {
                            if (IsObjectDead("Bàng Xí"))
                            {
                                if (GoToEx(23, 17))
                                    IsP = true;
                            }
                            else
                            {
                                if (!DeadObjects.ContainsKey("Bàng Xí"))
                                {
                                    if (GoToEx(23, 17))
                                        Talk("Phan Thanh THanh");
                                }
                            }
                        }
                        else
                        {
                            if (NeBayTime.Elapsed.TotalSeconds > 3)
                                GoToEx(30, 29);
                        }
                    }
                    if (GetRoundTuTuyet(RoundX, RoundY) == 3 && PartyMinSwBuffNM >= 3)
                    {
                        if (IsObjectDead("Đào Thanh"))
                        {
                            GoToEx(27, 27);
                        }
                        else
                        {
                            if (TrueClearMonterTime.Elapsed.TotalSeconds > 15)
                            {
                                if (GetDistance(85, 23) < 4)
                                {
                                    if (TrueStandTime.Elapsed.TotalSeconds > 4)
                                    {
                                        GoToEx(95, 40);
                                        Thread.Sleep(3000);
                                    }
                                    else
                                    {
                                        Talk("Phan Thanh Thanh");
                                    }
                                }
                                else
                                {
                                    if (TrueStandTime.Elapsed.TotalSeconds > 4)
                                    {
                                        GoToEx(85, 23);
                                    }
                                }
                            }
                            else
                            {
                                GoToEx(95, 40);
                            }
                        }
                    }
                    if (GetRoundTuTuyet(RoundX, RoundY) == 2)
                    {
                        if (Objects.Monters.Where(o => o.Name == "Tần Vận" && o.HP > 0.5).FirstOrDefault() != null)
                        {
                            string point = GAMEDIC.TuTuyetTrang[PartyIndex];
                            if (GetDistance(point) >= 2)
                            {
                                Move(point);
                            }
                        }
                        else
                        {
                            if (TrueClearMonterTime.Elapsed.TotalSeconds > 15 && PartyMinSwBuffNM >= 3)
                            {
                                if (IsObjectDead("Tần Vận"))
                                {
                                    if (GoToEx(35, 87))
                                        IsP = true;
                                }
                                else
                                {
                                    if (GoToEx(35, 87))
                                    {
                                        Talk("Phan Thanh Thanh");
                                        if (TrueStandTime.Elapsed.TotalSeconds > 3)
                                        {
                                            string point = GAMEDIC.TuTuyetTrang[PartyIndex];
                                            if (GetDistance(point) >= 2)
                                            {
                                                Move(point);
                                                Thread.Sleep(3000);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (GetRoundTuTuyet(RoundX, RoundY) == 1 && PartyMinSwBuffNM >= 3)
                    {
                        if (TrueClearMonterTime.Elapsed.TotalSeconds > 3)
                        {
                            if (IsObjectDead("Mẫn Mặc"))
                            {
                                if (GoToEx(96, 79))
                                    IsP = true;
                            }
                            else
                            {
                                if (TLBB.IsLeader)
                                {
                                    if (!ischeckttt)
                                    {
                                        if (GoToEx(112, 92))
                                        {
                                            ischeckttt = true;
                                        }
                                    }
                                    else if (!ischeckttt2)
                                    {
                                        if (GoToEx(90, 92))
                                        {
                                            ischeckttt2 = true;
                                        }
                                    }
                                    else
                                    {
                                        if (TrueClearMonterTime.Elapsed.TotalSeconds > 15)
                                            if (GoToEx(96, 79))
                                                Talk("Phan Thanh Thanh");
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (TLBB.MapId == MAP.LanHoanPhucDia && Global.IsPhucDia)
            {
                foreach (GameObject obj in Objects.All)
                {
                    if (obj.Name == "Nhắc nhở khu an toàn bát quái")
                    {
                        if (GetDistance(obj.X, obj.Y) > 2)
                        {
                            Move(obj.X, obj.Y);                           
                        }
                        IsPhanDame = true;
                    }
                }
                if (Objects.Monters.Count > 0 && TLBB.IsNoiCong)
                {
                    if (NeBayTime.Elapsed.TotalSeconds > 3)
                    {
                        if (TDT.GetDistance(RoundX, RoundY, 141, 63) < 30)
                        {
                            if (GetDistance(GAMEDIC.PhucDiaLyThuThuy[PartyIndex]) >= 2)
                            {
                                Move(GAMEDIC.PhucDiaLyThuThuy[PartyIndex]);
                            }
                        }
                        if (TDT.GetDistance(RoundX, RoundY, 213, 120) < 30)
                        {
                            if (GetDistance(GAMEDIC.PhucDiaLyDongLao[PartyIndex]) >= 2)
                            {
                                Move(GAMEDIC.PhucDiaLyDongLao[PartyIndex]);
                            }
                        }
                        if (Objects.Monters.Any(o => o.CleanName == "hutruc" && o.HP > 0 && o.HP <= 0.5))
                        {
                            if (GetDistance(GAMEDIC.PhucDiaHuTruc[PartyIndex]) >= 2)
                            {
                                Move(GAMEDIC.PhucDiaHuTruc[PartyIndex]);
                            }
                        }
                    }
                }
               
                int safex = 0; int safey = 0;
                npchacthuy = npchacthuy.Where(o => o.SwXuatHien.Elapsed.TotalSeconds < 30).ToList();

                if (GetDistance(182, 206) < 30)
                {
                    if (!IsSafeLanHoang(RoundX, RoundY))
                    {
                        Point me = new Point(RoundX, RoundY);
                        for (int i = 164; i <= 198; i++)
                        {
                            for (int j = 188; j <= 224; j++)
                            {
                                if (!me.IsClockwise(new Point(i, j), new Point(182, 206)))
                                    continue;
                                if (TDT.GetDistance(181, 206, i, j) < 15)
                                    continue;
                                if (TDT.GetDistance(i, j, 182, 206) > 18)
                                    continue;
                                if (IsSafeLanHoang(i, j))
                                {
                                    safex = i; safey = j;
                                }
                            }
                        }
                        if (safex == 0)
                        {
                            for (int i = 164; i <= 198; i++)
                            {
                                for (int j = 188; j <= 224; j++)
                                {
                                    if (TDT.GetDistance(182, 206, i, j) < 15)
                                        continue;
                                    if (TDT.GetDistance(i, j, 182, 206) > 18)
                                        continue;
                                    if (IsSafeLanHoang(i, j))
                                    {
                                        safex = i; safey = j;
                                    }
                                }
                            }
                        }
                    }
                }
                if (GetDistance(83, 181) < 30)
                {
                    if (!IsSafeLanHoang(RoundX, RoundY))
                    {
                        for (int i = 63; i <= 103; i++)
                        {
                            for (int j = 161; j <= 201; j++)
                            {
                                if (TDT.GetDistance(i, j, 83, 181) > 15)
                                    continue;
                                if (IsSafeLanHoang(i, j))
                                {
                                    safex = i; safey = j;
                                    break;
                                }
                            }
                        }
                    }
                }
                if (safex != 0)
                {                    
                    if (GetDistance(safex, safey) > 2)
                    {
                        Move(safex, safey);
                        PushDebugMessageEx("Né Bẫy");
                        NeBayTime = Stopwatch.StartNew();
                    }
                }
            }
            if (TLBB.MapId == MAP.PhungMinhVuongLang && isHotro && !TLBB.IsFollow)
            {
                if (Objects.Traps.Any(o => o.Distance < 6))
                {
                    SafeX = SafeY = 0;
                    if (!Objects.Traps.Any(o => o.GetDistance(48, 48) < 6))
                    {
                        SafeX = 48;
                        SafeY = 48;
                    }
                    else
                    {
                        for (int i = 28; i < 68; i++)
                        {
                            for (int j = 28; j < 68; j++)
                            {
                                if (!Objects.Traps.Any(o => o.GetDistance(i, j) < 6))
                                {
                                    if (GetDistance(i, j) < GetDistance(SafeX, SafeY))
                                    {
                                        SafeX = i;
                                        SafeY = j;
                                    }
                                }
                            }
                        }
                    }
                    if (SafeX != 0 && GetDistance(SafeX, SafeY) > 2)
                    {
                        PushDebugMessageEx("Né bẫy");
                        NeBayTime = Stopwatch.StartNew();
                        if (Objects.Monters.Any(o => o.Name == "Lục Ngô"))
                            TLBB.Fly(SafeX, SafeY);
                        else
                            Move(SafeX, SafeY);
                    }
                }
            }
            if (TLBB.MapId == MAP.ThieuThatSon && (IsThieuThatSonEx || isHotro))
            {
                GameObject tieuvienson = Party.Where(g => g.Objects.Monters.Any(o => o.Name == "Tiêu Viễn Sơn")).OrderBy(g => g.HideTime.Elapsed.TotalSeconds).Select(g => g.Objects.Monters.Where(o => o.Name == "Tiêu Viễn Sơn").FirstOrDefault()).FirstOrDefault();             
                if (tieuvienson != null)
                {
                    if (swTieuVienSon.Elapsed.TotalSeconds < 15 || Objects.All.Any(o => o.Name == "Phần Thiên Diệt Địa"))
                    {
                        if (tieuvienson.GetDistance(136, 50) > tieuvienson.GetDistance(111, 50))
                        {
                            Move(136, 50);
                        }
                        else
                        {
                            Move(111, 50);
                        }
                        NeBayTime = Stopwatch.StartNew();
                        PushDebugMessageEx("Né Bẫy");
                    }
                }
                if (Objects.Monters.Any(o => o.Name == "Trang Tụ Hiền") && TLBB.IsNoiCong)
                {
                    if (GetDistance(GAMEDIC.TrangTuHien[PartyIndex]) >= 2)
                    {
                        Move(GAMEDIC.TrangTuHien[PartyIndex]);
                    }
                }
                if (Leader.IsOnlyMoDungBac)
                {
                    if (GetDistance(GAMEDIC.MoDungBacPoint[PartyIndex]) >= 2)
                    {
                        Move(GAMEDIC.MoDungBacPoint[PartyIndex]);
                        NeBayTime = Stopwatch.StartNew();
                    }
                }
                if (Objects.Self != null && Objects.HaveMonter("Đinh Xuân Thu"))
                {
                    if (Objects.Self.Buff.Contains(1657))
                    {
                        IsPhanDame = true;
                        if (Objects.Have("Huyết chú vu cổ"))
                        {
                            List<string> listvuco = Party.Where(_ => _.Objects.Self.Buff.Contains(1657)).Select(_ => _.PointVuCo).ToList();
                            if (TDT.GetDistance(NeBayXuanThu[PartyIndex], safevuco[isafe]) < 6)
                            {
                                foreach (var game in Party)
                                {
                                    if (!game.Objects.Self.Buff.Contains(1657))
                                    {
                                        if (TDT.GetDistance(NeBayXuanThu[game.PartyIndex], safevuco[isafe]) >= 6 && !listvuco.Contains(NeBayXuanThu[game.PartyIndex]))
                                        {
                                            Move(NeBayXuanThu[game.PartyIndex]);
                                            PointVuCo = NeBayXuanThu[game.PartyIndex];
                                        }
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                Move(NeBayXuanThu[PartyIndex]);
                                PointVuCo = NeBayXuanThu[PartyIndex];
                            }
                        }
                        else
                        {
                            Move(NeBayXuanThu[PartyIndex]);
                            PointVuCo = NeBayXuanThu[PartyIndex];
                        }
                    }
                    else
                    {
                        if (Objects.Have("Huyết chú vu cổ") && Setting.Is("checkNeBinh"))
                        {
                            string point = GetPointVuCo();
                            if (point != string.Empty)
                            {
                                Move(point);
                            }
                        }
                        else
                        {
                            Move(130, 127);
                        }
                    }
                }
                foreach (GameObject _object in Objects.Monters)
                {
                    if (_object.CleanName == "modungphuc" || _object.CleanName == "furong")
                    {
                        if (TLBB.HPPercent <= 50)
                        {
                            IsPhanDame = true;
                        }
                        if (_object.Buff.Contains(1652) || Party.Where(p => p.Objects.Self.Buff.Contains(1650)).FirstOrDefault() != null)
                        {
                            IsPhanDame = true;
                            if (TLBB.IsLeader)
                            {
                                if (AskFollowTime.Elapsed.TotalSeconds >= 8)
                                {
                                    AskFollowTime = Stopwatch.StartNew();
                                    AskTeamFollow();
                                    Thread.Sleep(2000);
                                    StopFollow();
                                }
                            }
                            if (TLBB.PlayerState == 7)
                            {
                                if (TLBB.Menpai == MENPAI.NgaMy && Objects.Self != null && Objects.Self.AtkToId != _object.Id)
                                {
                                }
                                else
                                {
                                    if (GetDistance("192,87") > 2)
                                    {
                                        Move("192,87");
                                    }
                                    else
                                    {
                                        Move("198,87");
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (TLBB.MapId == MAP.LoiDaiSinhTu)
            {
                if ((Leader != null && Leader.Missions.Contains(MissionsType.DatDoiSatTinh)) || isHotro)
                {
                    GameObject tongkhuong = Objects.Monters.Where(o => o.Name == "Tống Khương").FirstOrDefault();
                    if (tongkhuong != null)
                    {                     
                        if (tongkhuong.Buff.Contains(565))
                        {
                            string point = GAMEDIC.LoiDaiSinhTu.OrderByDescending(p => tongkhuong.GetDistance(p)).FirstOrDefault();
                            if (GetDistance(point) > 2)
                                Move(point);
                            IsPhanDame = true;
                        }
                        else
                        {
                            if (Party.Any(p => p.TLBB.HPPercent > 0 && p.TLBB.HPPercent < 70))
                            {
                                IsPhanDame = true;
                                if (TLBB.PlayerState == 7 && Objects.Self.AtkToId == tongkhuong.Id)
                                {
                                    FixKetMap(1);                                    
                                }
                            }
                        }
                    }
                    if (TLBB.IsLeader && Objects.Monters.Any(o => o.Name == "Quan Thịnh") && Objects.Self.Buff.Contains(587))
                    {
                        AskTeamFollow();
                        Thread.Sleep(3000);
                        StopFollow();
                    }
                    if (Objects.Monters.Any(o => o.Name == "Lý Khôi"))
                    {
                        if (TLBB.PetHP > 0)
                        {
                            ThuPet();
                        }
                        if (GetDistance(GAMEDIC.LoiDaiSinhTu[PartyIndex]) >= 2)
                        {
                            Move(GAMEDIC.LoiDaiSinhTu[PartyIndex]);
                            IsPhanDame = true;
                        }
                    }
                    GameObject luquandat = Objects.Monters.Where(o => o.Name == "Lư Quân Dật").FirstOrDefault();
                    if (luquandat != null)
                    {
                        if (TLBB.HPPercent <= 50)
                        {
                            IsPhanDame = true;
                        }
                        if (luquandat.Buff.Contains(533))
                        {
                            if (TLBB.PlayerState == 7 && Objects.Self.AtkToId == luquandat.Id)
                            {
                                FixKetMap(1);
                            }
                            IsPhanDame = true;
                        }
                    }
                    if (Objects.Traps.Any(o => o.Distance < 5))
                    {
                        PushDebugMessageEx("Né bẫy");
                        NeBayTime = Stopwatch.StartNew();
                        SafeX = SafeY = 0;
                        CurPhungMinhIndex = -1;
                        GetSafePoint();
                        if (SafeX != 0)
                            Move(SafeX, SafeY);
                    }
                }
            }

            if ((isHotro || (Leader != null && Leader.Missions.Contains(MissionsType.DatDoiTamThan))) && Global.IsTamThan)
            {
                if (TLBB.MapId == MAP.TamThanHuyenCanh)
                {
                    if (RoundX > 95)
                    {
                        if (!DeadObjects.ContainsKey("Liệt Hải Ma Long"))
                            DeadObjects["Liệt Hải Ma Long"] = Stopwatch.StartNew();
                    }
                    if (RoundX > 95 && RoundY > 115)
                    {
                        if (!DeadObjects.ContainsKey("Liệt Hải Ma Long"))
                            DeadObjects["Liệt Hải Ma Long"] = Stopwatch.StartNew();
                        if (!DeadObjects.ContainsKey("Diệt Thế Hỏa Phụng"))
                            DeadObjects["Diệt Thế Hỏa Phụng"] = Stopwatch.StartNew();
                    }
                 
                    if (Objects.Traps.Any(o => o.Distance < 8))
                    {
                        SafeX = SafeY = 0;
                        GetSafePoint();
                        if (SafeX != 0 && GetDistance(SafeX, SafeY) > 2)
                        {
                            PushDebugMessageEx("Né bẫy");
                            Move(SafeX, SafeY);
                            NeBayTime = Stopwatch.StartNew();
                        }
                    }

                    if (DeadObjects.ContainsKey("Phệ Hoa Yêu") && !IsMoHop && SWHoiSinh.Elapsed.TotalSeconds > 3)
                    {
                        if (!ForcePickItem())
                        {
                            string nameHopTamThan = "Rương Tam Thần-Nhân";
                            foreach (var item in PacketItems.All)
                            {
                                if (item.Name.Contains("Chìa Tiên Côn Ngô"))
                                {
                                    nameHopTamThan = "Rương Tam Thần-Thiên";
                                    break;
                                }
                                if (item.Name.Contains("Chìa Mật Côn Ngô"))
                                {
                                    nameHopTamThan = "Rương Tam Thần-Địa";
                                    break;
                                }
                            }
                            GameObject objHopTamThan = Objects.All.Where(o => o.Name.Contains(nameHopTamThan)).FirstOrDefault();
                            if (objHopTamThan != null)
                            {
                                if (GoToEx(objHopTamThan.X, objHopTamThan.Y))
                                {
                                    Talk(objHopTamThan);
                                    Thread.Sleep(1000);
                                    if (TLBB.IsQuestOpen)
                                    {
                                        if (QuestFrame.Click("#{SSHJ_150915_92}"))
                                            Thread.Sleep(1000);
                                        if (QuestFrame.Click("#{SSHJ_150915_65}"))
                                        {
                                            Thread.Sleep(1000);
                                            QuestFrame.Close();
                                            IsMoHop = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (TLBB.MapId == MAP.ThieuThatSon)
            {
                if (Global.IsBoBoss && IdleTime > 10)
                {
                    foreach (GameObject _obj in Objects.All)
                    {
                        if (_obj.CleanName == "thieulamdetu")
                        {
                            Talk(_obj.Id);
                            Thread.Sleep(1000);
                            QuestFrame.Click("#{CJG_101231_79}");
                            Thread.Sleep(1000);
                            return;
                        }
                    }
                }
            }
        }

        public void NhanThuongTyVo()
        {
            if (GoToEx(LACDUONG.ChuBaHanh))
            {
                Talk(LACDUONG.ChuBaHanh);
                Thread.Sleep(1000);
                QuestFrame.Click("#{BWDH_180804_15}");
                Thread.Sleep(1000);
                QuestFrame.Click("#{BWDH_180804_218}");
                RemoveMission(MissionsType.NhanThuongTyVo);
            }
        }

        public void NhanBuaBaoRuong()
        {
            if (TLBB.IsMapBang(TLBB.MapId))
            {
                if (GoToEx(BANG.VoDaiUy))
                {
                    Talk(BANG.VoDaiUy);
                    Thread.Sleep(1000);
                    for (int i = 0; i < 4; i++)
                    {
                        QuestFrame.Click("#{VNYDBX_151028_16}");
                        Thread.Sleep(1000);
                    }
                    RemoveMission(MissionsType.NhanBuaBaoRuong);
                }
            }
            else
            {
                VaoBang();
            }
        }

        public void NopChienHonNgoc()
        {
            if (!TLBB.IsMapBang(TLBB.MapId))
            {
                VaoBang();
                return;
            }
            if (Missions.Contains(MissionsType.NhanChienCong))
            {
                if (GoToEx(94, 55))
                {
                    TalkEx("Triệu Tử Huân");
                    Thread.Sleep(1000);
                    if (TLBB.IsQuestOpen)
                    {
                        QuestFrame.Click("#{ZGMW_201009_01}");
                        Thread.Sleep(1000);
                        DoStringEx("setmetatable(_G, {__index = T3ZhanGongGet_Env}); if this:IsVisible() then T3ZhanGongGet_OnOKClicked(); end");
                        RemoveMission(MissionsType.NhanChienCong);
                    }
                }
                return;
            }
            if (Missions.Contains(MissionsType.NhanKiemChi))
            {
                if (GoToEx(BANG.VanLinhTranh))
                {
                    TalkEx("Vạn Linh Tranh");
                    Thread.Sleep(1000);
                    if (TLBB.IsQuestOpen)
                    {
                        QuestFrame.Click("#{JLDQZ_200814_128}");
                        Thread.Sleep(1000);
                        QuestFrame.Click("#{LDDQ_20200819_94}");
                        Missions.Remove(MissionsType.NhanKiemChi);
                    }
                }
            }
        }

        public void NhanQuanSonHai()
        {
            if (GoToEx(DAILY.ThamHanChau))
            {
                Talk(DAILY.ThamHanChau);
                Thread.Sleep(1000);
                QuestFrame.Click("#{PTFB_191228_259}");
                Thread.Sleep(1000);
                QuestFrame.Click("#{PTFB_191228_467}");
                Thread.Sleep(1000);
                QuestFrame.Click("#{PTFB_191228_469}");
                Thread.Sleep(1000);
                QuestFrame.Click("#{PTFB_191228_471}");
                Thread.Sleep(1000);
                Talk(DAILY.ThamHanChau);
                Thread.Sleep(1000);
                QuestFrame.Click("#{PTFB_191228_04}");
                Thread.Sleep(1000);
                QuestFrame.Click("#{PTFB_191228_241}");
                RemoveMission(MissionsType.NhanThuongQuanSonHai);
            }
        }

        public void NhanKeoHallowen()
        {
            if (GoToEx(TOCHAU.VanNganBac))
            {
                Talk(TOCHAU.VanNganBac);
                Thread.Sleep(1000);
                if (TLBB.IsQuestOpen)
                {
                    QuestFrame.Click("#{WSTGY_111012_01}");
                    Thread.Sleep(1000);
                    QuestFrame.Click("#{WSTGY_111012_08}");
                    Thread.Sleep(1000);
                    QuestFrame.Accept();
                    RemoveMission(MissionsType.NhanKeoHallowen);
                }
            }
        }

        public void SendExchange(int id)
        {
            //4C 19 58 01 00 00 B5 01 00 00 00 00 FF FF FF FF AA 47 00 00 FF FF FF FF FF FF FF FF
            byte[] buff = BitConverter.GetBytes(id);
            SendPacket(HexToString(Address.Exchange) + " 00 00 B5 01 00 00 00 00 FF FF FF FF " + buff[0].ToString("X2") + buff[1].ToString("X2") + " 00 00 FF FF FF FF FF FF FF FF");
        }

        public void DropItemThienCo(uint itemIndex)
        {
            if (Address.GameType == 1)
            {
                PostMessage(itemIndex, 131);
            }
        }

        public void GetItemThienCo(uint itemIndex)
        {
            if (Address.GameType != 1)
                return;
            SendPacket(HexToString(Address.thiencobase) + " 00 00 F1 0E 00 00 00 00 FF FF FF FF 02 00  " + itemIndex.ToString("X2") + "FF 70 2A E5 3C 98 5F DF 00 F8 EA 9E 00 0E 00 00 00");
        }

        public void PushItemThienCo(uint packetIdx, int thiencoIdx)
        {
            if (Address.GameType != 1)
                return;
            SendPacket(HexToString(Address.thiencobase) + " 00 00 4E 55 00 00 00 00 FF FF FF FF 00 01 " + packetIdx.ToString("X2") + thiencoIdx.ToString("X2") + "1F 00 00 00");
        }

        public void SkillDo()
        {
            if (swXuaPet.Elapsed.TotalSeconds < 5)
                return;
            if (!IsOneSec)
                return;
            if (TLBB.IsFollow || !IsAuto || TLBB.IsRide || Global.Paused || TLBB.IsBienThan || TLBB.BusyEx)
                return;
            foreach (Skill skill in Skills)
            {
                if (skill.PacketId == 448) // ngoc toai
                {
                    if (IsMapPhuBan())
                    {
                        if (Objects.NearMonter(5).Count >= 3)
                        {
                            uint delay = Memory.Read(TLBB.DelayBase + skill.DelayOffset);
                            if (delay == 0 || delay == 0xFFFFFFFF)
                            {
                                DoSkill((int)skill.PacketId);
                            }
                            continue;
                        }
                    }
                }
                if (skill.Use || (TLBB.MPPercent > 5 && (TLBB.MapId == MAP.PhieuMieuPhong || TLBB.MapId == MAP.LoiDaiSinhTu) && skill.PacketId == 535) || (skill.UsePK && IsPK))
                {
                    //listskill lecaotri2020

                    if (Skill.IsBase((int)skill.PacketId))
                        continue;
                    uint delay = Memory.Read(TLBB.DelayBase + skill.DelayOffset);
                    if (delay == 0 || delay == 0xFFFFFFFF)
                    {
                        DoSkill((int)skill.PacketId);
                        Thread.Sleep(200);
                        //lecaotri2020
                    }
                }
            }
        }

        public bool IsAutoComeBack { get; set; }

        public void _DoSkill(int key)
        {
            SendKey(key);
        }

        public void DoSkill(int id)
        {
            GetBestTarget();
            if (Skill.IsLan(id))
            {
                if (BestTarget != null)
                {
                    if (TDT.GetDistance(CharX, CharY, BestTarget.X, BestTarget.Y) <= 19)
                        UseSkill(id, (int)BestTarget.Id, BestTarget.X, BestTarget.Y);
                }
                return;
            }

            if (Skill.BuffSelf(id) != 0)
            {
                if (Objects.Self != null)
                {
                    if (!Objects.Self.Buff.Contains(Skill.BuffSelf(id)))
                    {
                        UseSkill(id, (int)Objects.Self.Id);
                        return;
                    }
                }
                return;
            }
            if (Skill.Buff(id) != 0)
            {
                foreach (GameObject _object in Objects.PartyEx.Where(p => p.State != 9))
                {
                    if (!_object.Buff.Contains((uint)Skill.Buff(id)))
                    {
                        UseSkill(id, (int)_object.Id);
                        return;
                    }
                }
                return;
            }
            if (Skill.ChieuQuan(id) != 0)
            {
                if (BestTarget != null && !BestTarget.Buff.Contains((uint)Skill.ChieuQuan(id)))
                {
                    if (TDT.GetDistance(CharX, CharY, BestTarget.X, BestTarget.Y) <= 19)
                        UseSkill(id, (int)BestTarget.Id, BestTarget.X, BestTarget.Y);
                }
                return;
            }
            if (id == 447) // kinh mach nghich hanh
            {
                if (TLBB.MPPercent <= 50)
                    UseSkill(id);
                return;
            }
            if (id == 3443) // kinh mach nghich hanh
            {
                if (TLBB.HPPercent <= 80)
                    UseSkill(id);
                return;
            }
            if (Skill.ChiDiem(id) != 0)
            {
                foreach (GameObject _object in Objects.Monters)
                {
                    if (!_object.Buff.Contains((uint)Skill.ChiDiem(id)))
                    {
                        UseSkill(id, (int)_object.Id);
                        return;
                    }
                }
                return;
            }
            if (Skill.IsHalfRage(id))
            {
                if (TLBB.Rage >= 500 && BestTarget != null)
                {
                    UseSkill(id, (int)BestTarget.Id);
                }
                return;
            }
            if (Skill.IsHalfRageBuff(id))
            {
                if (TLBB.Rage >= 500)
                {
                    UseSkill(id);
                }
                return;
            }
            if (Skill.IsFullRage(id))
            {
                if (TLBB.Rage == 1000)
                {
                    if (BestTarget != null)
                    {
                        UseSkill(id, BestTarget.Id);
                    }
                    else
                    {
                        if (TLBB.Rage == 1000)
                        {
                            UseSkill(id);
                        }
                    }
                }
                return;
            }
            if (id == 424 || id == 407)
            {
                if (Objects.PartyMinHP != null && Objects.PartyMinHP.HP <= 0.8)
                    UseSkill(id, Objects.PartyMinHP.Id);
                return;
            }
            if (!Skill.IsRage(id))
            {
                if (BestTarget != null)
                    UseSkill(id, (int)BestTarget.Id, BestTarget.X, BestTarget.Y);
            }
        }

        public uint DelayOffset(int id)
        {
            foreach (Skill skill in Skills)
            {
                if (skill.PacketId == id)
                    return skill.DelayOffset;
            }
            return 0;
        }

        //lecaotri
        public uint SkillId(uint key)
        {
            if (key < 20)
                key += 0x70;
            else if (key < 0x30)
                key += 0x1C;
            uint keySkillIdBase = Memory.Read(Address.KeySkillIdBase);
            if (key >= 0x70)
            {
                key = Memory.Read(keySkillIdBase + (key - 0x70) * 0x18 + 4);
            }
            else
            {
                uint alt = key - 0x1C - 20 - 1;
                if (alt < 0)
                    alt = 9;
                key = Memory.Read(keySkillIdBase + (alt) * 0x18 + 0x2D0 + 4);
            }
            return key;
        }

        public void SendKey(int key)
        {
            if (key < 20)
                key += 0x70;
            else if (key < 0x30)
                key += 0x1C;
            PostMessage(key, 101);
            //if (Global.IsFull > 0)
            //{
            //    //PostMessage(key, 101);
            //}
            //else
            //{
            //    //Win.PostMessage(Handle, WM_KEYDOWN, key, 0);
            //    //Win.PostMessage(Handle, WM_KEYUP, key, 0);
            //}
        }

        private bool isdauthai = false;
        public int TongDoHong { get; set; } = 0;

        public Stopwatch TrueNotMoveTime { get; set; } = Stopwatch.StartNew();

        private Stopwatch swChucPhuc = null;

        public bool IsWriteKhongChiemMan { get; set; }

        public void BuffPet()
        {
            if (!IsOneSec)
                return;
            if (!TLBB.Online)
                return;
            if (!IsPet || TLBB.PetHPPercent == 0 || TLBB.IsFollow || TLBB.IsRide)
                return;
            if ((Global.BuffPetPercent == 0 && (TLBB.PetHPPercent <= 50 || (TLBB.PetHPPercent <= 85 && (TLBB.PetMaxHP - TLBB.PetHP >= 10000)))) || (TLBB.PetHPPercent <= Global.BuffPetPercent && Global.BuffPetPercent > 0))
                PostMessage(1, 105);
            if (SecCount % 3 == 0 && TLBB.PetEnjoy <= 81 && TLBB.PetEnjoy > 0 && HaveItem("PetBauble_4"))
                PostMessage(2, 105);
        }

        //public void PlayerPackageUseItem(int index)
        //{
        //    PostMessage(index, 111);
        //}


        public void TuLuyenTheLuc()
        {
            if (GoToEx(DAILY.TaoDiaThanTang))
            {
                for (int i = 0; i < 6; i++)
                {
                    DoStringEx(@"
                    Clear_XSCRIPT();
                    Set_XSCRIPT_Function_Name('AskXiuLianAdvanceJingJie');
                    Set_XSCRIPT_ScriptID(002099);
                    Set_XSCRIPT_Parameter(0, 171);
                    Set_XSCRIPT_Parameter(1, " + i + @");
                    Set_XSCRIPT_ParamCount(2);
                    Send_XSCRIPT();
                    XiulianStudy_SetSelectState();"
                    );
                }
                for (int i = 0; i < 6; i++)
                {
                    DoStringEx("Clear_XSCRIPT(); Set_XSCRIPT_Function_Name('AskXiuLianLevelUp'); Set_XSCRIPT_ScriptID(002099); Set_XSCRIPT_Parameter(0,171); Set_XSCRIPT_Parameter(1,2); Set_XSCRIPT_ParamCount(2); Send_XSCRIPT();");
                }
                RemoveMission(MissionsType.TuLuyenTheLuc);
            }
        }

        public void TuLuyenNoiCong()
        {
            if (GoToEx(DAILY.TaoDiaThanTang))
            {
                for (int i = 0; i < 6; i++)
                {
                    DoStringEx(@"
                    Clear_XSCRIPT();
                    Set_XSCRIPT_Function_Name('AskXiuLianAdvanceJingJie');
                    Set_XSCRIPT_ScriptID(002099);
                    Set_XSCRIPT_Parameter(0, 171);
                    Set_XSCRIPT_Parameter(1, " + i + @");
                    Set_XSCRIPT_ParamCount(2);
                    Send_XSCRIPT();
                    XiulianStudy_SetSelectState();"
                    );
                }
                for (int i = 0; i < 6; i++)
                {
                    DoStringEx("Clear_XSCRIPT(); Set_XSCRIPT_Function_Name('AskXiuLianLevelUp'); Set_XSCRIPT_ScriptID(002099); Set_XSCRIPT_Parameter(0,171); Set_XSCRIPT_Parameter(1,1); Set_XSCRIPT_ParamCount(2); Send_XSCRIPT();");
                }
                RemoveMission(MissionsType.TuLuyenNoiLuc);
            }
        }


        public void TuLuyenThanPhap()
        {
            if (GoToEx(DAILY.TaoDiaThanTang))
            {
                for (int i = 0; i < 6; i++)
                {
                    DoStringEx(@"
                    Clear_XSCRIPT();
                    Set_XSCRIPT_Function_Name('AskXiuLianAdvanceJingJie');
                    Set_XSCRIPT_ScriptID(002099);
                    Set_XSCRIPT_Parameter(0, 171);
                    Set_XSCRIPT_Parameter(1, " + i + @");
                    Set_XSCRIPT_ParamCount(2);
                    Send_XSCRIPT();
                    XiulianStudy_SetSelectState();"
                    );
                }
                for (int i = 0; i < 6; i++)
                {
                    DoStringEx("Clear_XSCRIPT(); Set_XSCRIPT_Function_Name('AskXiuLianLevelUp'); Set_XSCRIPT_ScriptID(002099); Set_XSCRIPT_Parameter(0,171); Set_XSCRIPT_Parameter(1,4); Set_XSCRIPT_ParamCount(2); Send_XSCRIPT();");
                }
                RemoveMission(MissionsType.TuLuyenThanPhap);
            }
        }


        public void TuLuyenNgoaiCong()
        {
            if (GoToEx(DAILY.TaoDiaThanTang))
            {
                for (int i = 0; i < 6; i++)
                {
                    DoStringEx(@"
                    Clear_XSCRIPT();
                    Set_XSCRIPT_Function_Name('AskXiuLianAdvanceJingJie');
                    Set_XSCRIPT_ScriptID(002099);
                    Set_XSCRIPT_Parameter(0, 171);
                    Set_XSCRIPT_Parameter(1, " + i + @");
                    Set_XSCRIPT_ParamCount(2);
                    Send_XSCRIPT();
                    XiulianStudy_SetSelectState();");
                }
                for (int i = 0; i < 6; i++)
                {
                    DoStringEx(@"
                    Clear_XSCRIPT();
                    Set_XSCRIPT_Function_Name('AskXiuLianAdvanceJingJie');
                    Set_XSCRIPT_ScriptID(002099);
                    Set_XSCRIPT_Parameter(0, 171);
                    Set_XSCRIPT_Parameter(1, " + i + @");
                    Set_XSCRIPT_ParamCount(2);
                    Send_XSCRIPT();
                    XiulianStudy_SetSelectState();"
                    );
                }
                for (int i = 0; i < 6; i++)
                {
                    DoStringEx("Clear_XSCRIPT(); Set_XSCRIPT_Function_Name('AskXiuLianLevelUp'); Set_XSCRIPT_ScriptID(002099); Set_XSCRIPT_Parameter(0,171); Set_XSCRIPT_Parameter(1,0); Set_XSCRIPT_ParamCount(2); Send_XSCRIPT();");
                }
                RemoveMission(MissionsType.TuLuyenCuongLuc);
            }
        }

        public void Buff()
        {
            if (SecCount % 3 != 0)
                return;
            if (TLBB.Name == "ĐăngNhập" || TLBB.IsFollow || TLBB.IsRide || TLBB.HPPercent == 0)
                return;
            if (IsHP && TLBB.HPPercent <= Global.BuffHPPercent)
            {
                foreach (var item in PacketItems.All)
                {
                    if (item.Type == "Icons03_1" || item.Type == "Medicine1_3" || item.Type == "Medicine1_13" || item.Type == "Cloth2_4" || item.Type == "Cloth2_5")
                    {
                        item.Use();
                    }
                }
                //lecaotri2020
                if (TLBB.SkillPetType1.Contains("PetSkill1_11") || TLBB.SkillPetType2.Contains("PetSkill1_11"))
                {
                    if (Address.GameType == 1)
                    {
                        DoStringEx("setmetatable(_G, {__index = MainMenuBar_3_Env}); MainMenuBar_3_Clicked(1); ");
                    }
                    else
                        UseSkillPet(0x2AE);
                }
                if (TLBB.SkillPetType1.Contains("PetSkill1_12") || TLBB.SkillPetType2.Contains("PetSkill1_12"))
                {
                    if (Address.GameType == 1)
                    {
                        DoStringEx("setmetatable(_G, {__index = MainMenuBar_3_Env}); MainMenuBar_3_Clicked(1);");
                    }
                    else
                        UseSkillPet(0x2AF);
                }
            }
            if (IsMP && NeBayTime.Elapsed.TotalSeconds > 2)
            {
                if (TLBB.MPPercent <= Global.BuffMPPercent)
                {
                    if (TLBB.Menpai == MENPAI.TinhTuc)
                    {
                        UseSkill(447);
                    }
                    foreach (var item in PacketItems.All)
                    {
                        if (item.Type == "Medicine1_1" || item.Type == "Medicine1_12" || item.Type == "Cloth2_6" || item.Type == "Cloth2_7")
                        {
                            item.Use();
                        }
                    }
                    if (Setting.Value("numberDungCHT") > TLBB.PetHPPercent && Setting.Value("numberDungCHT") > 0)
                    {
                    }
                    else
                    {
                        if (TLBB.SkillPetType1.Contains("PetSkill1_9") || TLBB.SkillPetType2.Contains("PetSkill1_9"))
                        {
                            if (Objects.Self != null && Objects.Self.Buff.Contains(353))
                            {
                            }
                            else
                            {
                                if (Address.GameType == 1)
                                {
                                    if (TLBB.SkillPetType1.Contains("PetSkill1_9"))
                                        DoStringEx("setmetatable(_G, {__index = MainMenuBar_3_Env}); MainMenuBar_3_Clicked(1);");
                                    else
                                        DoStringEx("setmetatable(_G, {__index = MainMenuBar_3_Env}); MainMenuBar_3_Clicked(2);");
                                }
                                else
                                    UseSkillPet(0x2B8);
                            }
                        }
                        else if (TLBB.SkillPetType1.Contains("PetSkill1_10") || TLBB.SkillPetType2.Contains("PetSkill1_10"))
                        {
                            if (Objects.Self != null && Objects.Self.Buff.Contains(353))
                            {
                            }
                            else
                            {
                                if (Address.GameType == 1)
                                {
                                    if (TLBB.SkillPetType1.Contains("PetSkill1_10"))
                                        DoStringEx("setmetatable(_G, {__index = MainMenuBar_3_Env}); MainMenuBar_3_Clicked(1);");
                                    else
                                        DoStringEx("setmetatable(_G, {__index = MainMenuBar_3_Env}); MainMenuBar_3_Clicked(2);");
                                }
                                else
                                    UseSkillPet(0x2B9);
                            }
                        }
                        //lecaotri2020
                    }
                    if (TLBB.PetName != "tieudattai" && swXuattieudattai.Elapsed.TotalSeconds >= 3 && DiaPhuNguYeu.Elapsed.TotalSeconds > 3)
                    {
                        uint PetBase = Memory.Read(Address.PetBase);
                        int cnt = -1;
                        for (uint i = 0; i < 20; i++)
                        {
                            cnt++;
                            uint id = Memory.Read(PetBase + Address.PetDataSize * i + Address.PetId);
                            uint dome = Memory.Read(PetBase + Address.PetDataSize * i + Address.PetEnjoy);
                            uint lvl = Memory.Read(PetBase + Address.PetDataSize * i + Address.PetLvl);
                            string PetName = "";
                            if (Address.GameType == 1)
                                PetName = Memory._ReadString(PetBase + Address.PetDataSize * i + 0x20).Trim();
                            else
                                PetName = Memory._ReadString(PetBase + Address.PetDataSize * i + 0x1C).Trim();
                            if (PetName == "tieudattai")
                            {
                                if (dome < 60)
                                {
                                    if (TLBB.HaveFood)
                                    {
                                        FightPet(cnt);
                                        swXuattieudattai = Stopwatch.StartNew();
                                    }
                                }
                                else
                                {
                                    FightPet(cnt, false);
                                    swXuattieudattai = Stopwatch.StartNew();
                                }
                                break;
                            }
                        }
                    }
                }
                else
                {
                    if (Global.IsThuTieuDatTai && !Global.IsMaxPet)
                    {
                        if (TLBB.PetName == "tieudattai")
                        {
                            DoAction("PetSkill2_2");
                        }
                    }
                }
            }
        }

        public Stopwatch swXuattieudattai = Stopwatch.StartNew();
        private Stopwatch swUpRide = Stopwatch.StartNew();

        public void UpRide()
        {
            if (SwTimeOnMap.Elapsed.TotalSeconds < 5)
                return;
            if (TLBB.IsFollow)
                StopFollow();
            if (TLBB.IsRide)
                return;
            if (swUpRide.Elapsed.TotalSeconds <= 4)
                return;
            swUpRide = Stopwatch.StartNew();
            UseSkill(21);
        }

        public void HuyBienThan()
        {
            DoStringEx("local buff_num = Player:GetBuffNumber(); local i = 0; while i < buff_num do szToolTips = Player:GetBuffIconNameByIndex(i); if (string.find(szToolTips, 'Buff') or string.find(szToolTips, 'Charm')) and not string.find(szToolTips, 'Buff6_12') then Player:DispelBuffByIndex(i); end i = i+1 end");
        }

        public void DownRide()
        {
            if (TLBB.PlayerState == 2 && (TLBB.MapId == MAP.ThieuThatSon))
                return;
            if (TLBB.IsRide)
            {
                DoStringEx("local buff_num = Player:GetBuffNumber(); local i = 0; while i < buff_num do szToolTips = Player:GetBuffIconNameByIndex(i); if string.find(szToolTips, 'Ride') or string.find(szToolTips, 'ThaiIcons') then Player:DispelBuffByIndex(i);  end i = i+1 end");
            }
        }

        public void DownRidePartyEx(bool without = false)
        {
            foreach (Game game in Party)
            {
                if (game.IsAuto && game.TrueStandTime.Elapsed.TotalSeconds > 2)
                {
                    if (without)
                    {
                        if (game == this)
                            continue;
                    }
                    game.DownRide();
                }
            }
        }

        public void DownRideParty()
        {
            foreach (Game game in Party)
            {
                if (!game.TLBB.IsRide)
                    continue;
                if (game.IsAuto && game.swStandTime.Elapsed.TotalSeconds > 2)
                {
                    if (game.TLBB.MapId == MAP.PhungMinhVuongLang || game.TLBB.MapId == MAP.LoiDaiSinhTu || game.TLBB.MapId == MAP.ThieuThatSon || game.TLBB.MapId == MAP.NongTruongDaTru)
                    {
                        game.DownRide();
                        continue;
                    }
                    if (game.IsMapAcBa || game.TLBB.MapId == MAP.TangKinhCac)
                    {
                        game.DownRide();
                        continue;
                    }
                    if (game.TLBB.MapId == MAP.TacKhauDoanhDia)
                    {
                        game.DownRide();
                        continue;
                    }
                    if (game.Leader != null)
                    {
                        if (game.Leader.Missions.Contains(MissionsType.DatDoiBaoDoHiem))
                        {
                            game.DownRide();
                            continue;
                        }
                        if (game.Leader.Missions.Contains(MissionsType.DatDoiTamThan) || game.Leader.Missions.Contains(MissionsType.DatDoiMaTac) || game.Leader.Missions.Contains(MissionsType.DatDoiBossMap))
                        {
                            game.DownRide();
                            continue;
                        }
                    }
                }
            }
        }

        public void InviteTeam(byte[] name)
        {
            byte[] lua = Encoding.UTF8.GetBytes("Friend:InviteTeam('").Concat(name).Concat(Encoding.UTF8.GetBytes("')")).ToArray();
            Array.Resize(ref lua, lua.Length + 16);
            DoBuffer(lua);
        }

        public void AskTeam(byte[] name)
        {
            byte[] lua = Encoding.UTF8.GetBytes("Friend:AskTeam('").Concat(name).Concat(Encoding.UTF8.GetBytes("')")).ToArray();
            Array.Resize(ref lua, lua.Length + 16);
            DoBuffer(lua);
        }

        public void AskRaid(byte[] name)
        {
            byte[] lua = Encoding.UTF8.GetBytes("Target:SendRaidApplication('").Concat(name).Concat(Encoding.UTF8.GetBytes("')")).ToArray();
            Array.Resize(ref lua, lua.Length + 16);
            DoBuffer(lua);
        }

        public void KetNghia(List<Game> list)
        {
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                foreach (Game g in list)
                {
                    if (g == this)
                        continue;
                    byte[] lua = Encoding.UTF8.GetBytes("DataPool:AddFriendAndGrouping('").Concat(g.TLBB.VISCIIName).Concat(Encoding.UTF8.GetBytes("')")).ToArray();
                    Array.Resize(ref lua, lua.Length + 16);
                    DoBuffer(lua);
                    Thread.Sleep(1000);
                    DoStringEx("setmetatable(_G, {__index = Friend_IMGrouping_Env}); if this:IsVisible() then Friend_IMGrouping_OK_Clicked() end");
                    Thread.Sleep(1000);
                }
            }).Start();
        }

        //lecaotri
        public bool IsKetBai { get; set; }

        public bool IsSuDo { get; set; }
        public string TrangThaiNhiemVuKetBai = "";
        public string TrangThaiNhiemVuSuDo = "";

        private List<string> lstdennoi = new List<string>();

        public void KetBaisudo()
        {
            if (!IsSuDo)
                return;

            if (Global.lstsudo.Count > 1 && Global.lstsudo.Contains(TLBB.Name) && TrangThaiNhiemVuSuDo == "")
            {
                for (int i = 0; i < Global.lstsudo.Count; i++)
                {
                    if (Global.lstsudo[i] == TLBB.Name)
                        continue;
                    DoStringEx("DataPool: AddFriendAndGrouping('" + Global.lstsudo[i] + "');");
                    Thread.Sleep(500);
                    DoStringEx("setmetatable(_G, { __index = Friend_IMGrouping_Env}); Friend_IMGrouping_OK_Clicked();");
                    Thread.Sleep(500);
                }
                Global.lstsudo.Clear();
                TrangThaiNhiemVuSuDo = "TalkKetBai";
                lstdennoi.Clear();
                return;
            }

            if (TrangThaiNhiemVuSuDo == "TalkKetBai")
            {
                if (GoToEx(DAILY.NhiepChinh))
                {
                    if (TLBB.IsLeader)
                    {
                        foreach (var game in Party)
                        {
                            if (TDT.GetDistance(CharX, CharY, game.CharX, game.CharY) <= 1 && !lstdennoi.Contains(game.TLBB.Name))
                            {
                                lstdennoi.Add(game.TLBB.Name);
                            }
                        }

                        //for (int i = 0; i < Party.Count; i++)
                        //{
                        //    if (TDT.GetDistance(CharX, CharY, Party[i].CharX, Party[i].CharY) <= 1 && !lstdennoi.Contains(Party[i].Name))
                        //    {
                        //        lstdennoi.Add(Party[i].Name);
                        //    }
                        //}

                        if (lstdennoi.Count == Party.Count() && lstdennoi.Count != 0)
                        {
                            Talk(DAILY.NhiepChinh);
                            if (TLBB.IsQuestOpen)
                            {
                                Thread.Sleep(1000);
                                QuestFrame.Click("#{BSYH_150519_02}");
                                Thread.Sleep(1000);
                                QuestFrame.Click("#{BSYH_150519_266}");
                                Thread.Sleep(1000);
                                QuestFrame.Close();

                                Thread.Sleep(1000);
                                TrangThaiNhiemVuSuDo = "";
                                //lstdennoi.Clear();
                            }
                        }
                    }
                }
            }
        }

        public void KetBai()
        {
            if (!IsKetBai)
                return;

            if (Global.lstketban.Count > 1 && Global.lstketban.Contains(TLBB.Name) && TrangThaiNhiemVuKetBai == "")
            {
                for (int i = 0; i < Global.lstketban.Count; i++)
                {
                    if (Global.lstketban[i] == TLBB.Name)
                        continue;
                    DoStringEx("DataPool: AddFriendAndGrouping('" + Global.lstketban[i] + "');");
                    Thread.Sleep(500);
                    DoStringEx("setmetatable(_G, { __index = Friend_IMGrouping_Env}); Friend_IMGrouping_OK_Clicked();");
                    Thread.Sleep(500);
                }
                Global.lstketban.Clear();
                TrangThaiNhiemVuKetBai = "TalkKetBai";
                return;
            }

            if (TrangThaiNhiemVuKetBai == "TalkKetBai")
            {
                if (GoToEx(LACDUONG.TranPhuChi))
                {
                    if (!TLBB.IsLeader)
                    {
                        if (TLBB.IsKetBaiOpen)
                        {
                            DoStringEx("setmetatable(_G, { __index = WuhunQuest_Env}); WuhunQuest_Bn1Click();");
                            Thread.Sleep(100);
                            TrangThaiNhiemVuKetBai = "";
                            IsKetBai = false;
                        }
                        else
                        {
                            return;
                        }
                    }
                    Talk(LACDUONG.TranPhuChi);
                    if (TLBB.IsQuestOpen)
                    {
                        Thread.Sleep(1000);
                        QuestFrame.Click("#{JBLC_150528_2}");
                        Thread.Sleep(1000);
                        QuestFrame.Click("#{JBLC_150528_341}");
                        Thread.Sleep(1000);
                        QuestFrame.Close();
                        //TrangThaiNhiemVuKetBai = "MissionContinute";
                        if (QuestFrame.Text.Contains("#{JBLC_150528_27}"))
                            TrangThaiNhiemVuKetBai = "ChoDongy";
                        Thread.Sleep(1000);
                    }
                }
            }

            if (TrangThaiNhiemVuKetBai == "ChoDongy")
            {
                IsKetBai = false;
            }
        }

        public void PlayerPackageUseItem(int index) => DoStringEx("PlayerPackage:UseItem(" + index + ");");



        public uint[] AddressOneLineEx = new uint[100];
        private int curLine = 0;

        private object LockLua = new object();

        public void DoStringEx(string lua, bool iswait = false)
        {
            lock (LockLua)
            {
                if (AddressOneLineEx[curLine] == 0)
                {
                    AddressOneLineEx[curLine] = Memory.VirtualAllocEx(0x10000);
                }
                if (AddressOneLineEx[curLine] != 0)
                {
                    Memory.WriteUnicodeString(lua + "--", AddressOneLineEx[curLine]);

                    PostMessage(AddressOneLineEx[curLine], 104);
                }
                if (curLine < 99)
                    curLine++;
                else
                    curLine = 0;
            }
        }

        public void DoBuffer(byte[] lua, bool iswait = false)
        {
            Array.Resize(ref lua, lua.Length + 16);
            if (AddressOneLineEx[curLine] == 0)
            {
                AddressOneLineEx[curLine] = Memory.VirtualAllocEx(10240);
            }
            Memory.WriteProcessMemory(Memory.Id, AddressOneLineEx[curLine], lua, lua.Length, 0);
            PostMessage(AddressOneLineEx[curLine], 104);
            if (curLine < 99)
                curLine++;
            else
                curLine = 0;
        }

        public void UseSkillPet(int id, float x, float y)
        {
            //if (Address.GameType == 1)
            //{
            //    LuaDoOneLineString("setmetatable(_G, {__index = MainMenuBar_3_Env}); MainMenuBar_3_Clicked(1); MainMenuBar_3_Clicked(2);");
            //    return;
            //}
            PostMessage(Memory.Float2Int(x), 50);
            PostMessage(Memory.Float2Int(y), 51);
            PostMessage(-1, 53);
            PostMessage(id, 103);
        }

        public void UseSkillPet(int id)
        {
            //if (Address.GameType == 1)
            //{
            //    LuaDoOneLineString("setmetatable(_G, {__index = MainMenuBar_3_Env}); MainMenuBar_3_Clicked(1); MainMenuBar_3_Clicked(2);");
            //    return;
            //}
            PostMessage(0xBF800000, 50);
            PostMessage(0xBF800000, 51);
            PostMessage(-1, 53);
            PostMessage(id, 103);
        }

        public void UseSkill(int skillId, int targetId, float x, float y) => UseSkill((uint)skillId, (uint)targetId, x, y);
        public void UseSkill(uint skillId, uint targetId, float x, float y)
        {
            if (IsChangeMap)
                return;
            PostMessage(Memory.Float2Int(x), 50);
            PostMessage(Memory.Float2Int(y), 51);
            PostMessage(targetId, 53);
            PostMessage(skillId, 102);
        }

        public void UseSkill(int skillId, int targetId)
        {
            UseSkill(skillId, targetId, -1, -1);
        }

        public void UseSkill(int skillId, uint targetId)
        {
            UseSkill(skillId, (int)targetId, -1, -1);
        }

        public void UseSkill(int skillId)
        {
            UseSkill(skillId, -1, -1, -1);
        }

        public void SendKey(Keys key)
        {
            SendKey((int)key);
        }

        //IntPtr lastHandle = IntPtr.Zero;

        public void SetMicroHandle()
        {
            PostMessage((int)Main.Instance.Handle, -5);
            //if (lastHandle != Main.Instance.Handle) {
            //    lastHandle = Main.Instance.Handle;
            //}
        }

        public void SetDll()
        {

            PostMessage(Address.ParaLuaToString, 19);
            //PostMessage(Address.FuncLuaToString, 20);
            PostMessage(FuncLuaToString, 20);
            return;

            PostMessage(Address.CharState[0], 29);
            PostMessage(Address.BasePK, 30);
            PostMessage(Address.GameType, 0);
            PostMessage(Address.ParaSelectTarget, 1);
            PostMessage(Address.ParaSendKey[0], 2);
            PostMessage(Address.ParaSendKey[1], 3);
            PostMessage(Address.FuncSendKey, 4);
            PostMessage(Address.ParaUseSkill[0], 5);
            PostMessage(Address.ParaUseSkill[1], 6);
            PostMessage(Address.ParaUseSkill[2], 7);
            PostMessage(Address.FuncUseSkill, 8);
            PostMessage(Address.ParaUseSkillPet, 9);
            PostMessage(Address.FuncUseSkillPet, 10);
            PostMessage(Address.ParaLuaDoString, 11);
            PostMessage(Address.FuncLuaDoString, 12);
            PostMessage(Address.ParaPickItem, 13);
            PostMessage(Address.ParaCollectItem, 14);
            PostMessage(Address.DropBase[0], 15);
            PostMessage(Address.DropBase[1], 16);
            PostMessage(Address.DropBase[2], 17);
            PostMessage(Address.DropBase[3], 18);
      
            PostMessage(Address.ParaTalk, 21);
            PostMessage(Address.FuncUpLvl, 22);
            PostMessage(Address.FuncSelectTargetOfTarget, 23);
            PostMessage(Address.ParaSendPacket, 24);
            PostMessage(Address.FuncSendPacket, 25);
            AddressToString = Memory.VirtualAllocEx(20248);
            AddressTenBang = Memory.VirtualAllocEx(20248);
            PostMessage(Address.CharState[0], 26);
            PostMessage(Address.Exchange, 129);

            try
            {
                if (Address.GameType != 1)
                {
                    Process process = Process.GetProcessById(ProcessId);
                    int missaAddress = -1;
                    int misaSize = -1;
                    for (int i = 0; i < process.Modules.Count; i++)
                    {
                        if (process.Modules[i].ModuleName.ToLower() == "misahelp.dll")
                        {
                            missaAddress = (int)process.Modules[i].BaseAddress;
                            misaSize = process.Modules[i].ModuleMemorySize;
                        }
                    }
                    if (missaAddress != -1)
                    {
                        for (int i = 0; i < process.Threads.Count; i++)
                        {
                            int startAddress = (int)GetThreadStartAddress(process.Threads[i].Id);
                            if (startAddress > missaAddress && startAddress < missaAddress + misaSize)
                            {
                                SuspendThread((int)OpenThread(ThreadAccess.SuspendResume, false, (uint)process.Threads[i].Id));
                            }
                        }
                    }
                    int celisttl = -1;
                    int celisttlSize = -1;
                    for (int i = 0; i < process.Modules.Count; i++)
                    {
                        if (process.Modules[i].ModuleName.ToLower() == "celisttl.dll")
                        {
                            celisttl = (int)process.Modules[i].BaseAddress;
                            celisttlSize = process.Modules[i].ModuleMemorySize;
                        }
                    }
                    if (celisttl != -1)
                    {
                        for (int i = 0; i < process.Threads.Count; i++)
                        {
                            int startAddress = (int)GetThreadStartAddress(process.Threads[i].Id);
                            if (startAddress > celisttl && startAddress < celisttl + celisttlSize)
                            {
                                SuspendThread((int)OpenThread(ThreadAccess.SuspendResume, false, (uint)process.Threads[i].Id));
                            }
                        }
                    }
                }
            }
            catch { }
        }

        [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        private static extern uint GetProcAddress(IntPtr hModule, string procName);

        private uint GetRemoteProcAddress(Process targetProcess, string moduleName, string functionName)
        {
            uint func,
                 offset,
                 result;

            offset = 0;
            result = 0;

            foreach (ProcessModule mod in Process.GetCurrentProcess().Modules)
            {
                if (mod.ModuleName.ToLower() == moduleName)
                {
                    func = GetProcAddress(mod.BaseAddress, functionName);

                    if (func != 0)
                        offset = func - (uint)mod.BaseAddress;

                    break;
                }
            }

            if (offset != 0)
            {
                foreach (ProcessModule mod in targetProcess.Modules)
                {
                    if (moduleName.ToLower().Contains(mod.ModuleName.ToLower()))
                    {
                        result = (uint)mod.BaseAddress + offset;

                        break;
                    }
                }
            }

            return result;
        }

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [Out] byte[] lpBuffer, int dwSize, out IntPtr lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out int lpNumberOfBytesWritten);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr OpenThread(ThreadAccess dwDesiredAccess, bool bInheritHandle, uint dwThreadId);

        private static IntPtr GetThreadStartAddress(int threadId)
        {
            var hThread = OpenThread(ThreadAccess.QueryInformation, false, threadId);
            if (hThread == IntPtr.Zero)
                return IntPtr.Zero;
            var buf = Marshal.AllocHGlobal(IntPtr.Size);
            try
            {
                var result = NtQueryInformationThread(hThread,
                                 ThreadInfoClass.ThreadQuerySetWin32StartAddress,
                                 buf, IntPtr.Size, IntPtr.Zero);
                if (result != 0)
                    return IntPtr.Zero;
                return Marshal.ReadIntPtr(buf);
            }
            finally
            {
                CloseHandle(hThread);
                Marshal.FreeHGlobal(buf);
            }
        }

        [DllImport("ntdll.dll", SetLastError = true)]
        private static extern int NtQueryInformationThread(
            IntPtr threadHandle,
            ThreadInfoClass threadInformationClass,
            IntPtr threadInformation,
            int threadInformationLength,
            IntPtr returnLengthPtr);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr OpenThread(ThreadAccess dwDesiredAccess, bool bInheritHandle, int dwThreadId);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool CloseHandle(IntPtr hObject);

        [Flags]
        public enum ThreadAccess : int
        {
            Terminate = 0x0001,
            SuspendResume = 0x0002,
            GetContext = 0x0008,
            SetContext = 0x0010,
            SetInformation = 0x0020,
            QueryInformation = 0x0040,
            SetThreadToken = 0x0080,
            Impersonate = 0x0100,
            DirectImpersonation = 0x0200
        }

        public enum ThreadInfoClass : int
        {
            ThreadQuerySetWin32StartAddress = 9
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern int SuspendThread(int hThread);

        public byte[] bufferRecv = new byte[10];

        public bool IsHooked { get; set; }

        public bool IsLostLeader
        {
            get;
            set;
        }

        public bool IsOpenShop { get; set; }

        private static Stopwatch swResetTime = Stopwatch.StartNew();
        public bool IsWaitFast { get; set; } = false;

        public void ResetTime()
        {
            if (Address.GameType != 1)
                return;
            if (TLBB.PlayerState == 10 && TLBB.OnlineTime < 250)
                return;
            if (Global.AutoResetTimeAll)
            {
                if (resettimeall.Elapsed.TotalSeconds >= 3 && TLBB.OnlineTime > 175)
                {
                    resettimeall = Stopwatch.StartNew();
                    foreach (KeyValuePair<int, Game> kvp in Main.DicGame)
                    {
                        var game = kvp.Value;
                        if (game.TLBB.IsLeader)
                        {
                            game.IsLostLeader = true;
                        }
                        if (game.TLBB.PlayerState == 10)
                            game.IsOpenShop = true;
                        game.IsOpenPass2 = false;
                        game.ReConnect();
                    }
                }
            }
            if (!Global.AutoResetTime && !AutoResetTime)
                return;
            if (TLBB.OnlineTime > 175 && swResetTime.Elapsed.TotalSeconds > 30)
            {
                swResetTime = Stopwatch.StartNew();
                if (TLBB.PlayerState == 10)
                    IsOpenShop = true;
                IsOpenPass2 = false;
                SafeTime = -10;
                if (TLBB.IsLeader)
                {
                    IsLostLeader = true;
                }
                LUA.DataPoolReConnect();
                tranTime = Stopwatch.StartNew();
            }
        }

        public void ReConnect()
        {
            if (Address.GameType != 1)
                return;
            IsOpenPass2 = false;
            LUA.DataPoolReConnect();
        }

        private void UpLvl()
        {
            if (TLBB.Lvl == 0)
                return;

            if (Missions.Contains(MissionsType.NhiemVuThangCap) && TLBB.Exp > TLBB.MaxExp && TLBB.Lvl < 30 && TLBB.Lvl > 9 && TLBB.Menpai != 0)
            {
                PostMessage(7, 105);
                CoBanFailTime = new Stopwatch();
            }
            if (Global.AutoUpLvl && TLBB.Exp > TLBB.MaxExp && TLBB.Lvl < Global.AutoUpLvlBelow && TLBB.Menpai != 0)
            {
                PostMessage(7, 105);
                CoBanFailTime = new Stopwatch();
            }
        }



        public void VaoBang()
        {
            NPC npcBang = TOCHAU.PhamThuanTuy;
            if (TLBB.MapId == DAILY.Id)
                npcBang = DAILY.PhamThuanLe;
            if (TLBB.MapId == LACDUONG.Id)
                npcBang = LACDUONG.PhamThuanNhan;
            if (TLBB.MapId == LAULAN.Id)
                npcBang = LAULAN.PhamThuanHuu;
            //lecaotri
            if (Global.openbang && IsDuaHau && MissionState == "XongQD" && TLBB.MapId != LACDUONG.Id && TLBB.MapId != DAILY.Id && TLBB.MapId != TOCHAU.Id)
            {
                //THAIHO(246,146),tayho(134,165)
                //nhihai(77,206),vodi(69,106),nhanbac(277,54)
                //thaonguyen(256,51),namvuc(110,64),thachlam(73,214)
                //quynhchau(136,236)mieucuong(195,49)
                if (TenMapThanh() == "-1")
                {
                    BangOpen();
                    IsMoBang = true;
                }

                string thongtinbang = TDT.GetBangXY(TenMapThanh());
                string[] infomap = thongtinbang.Split(',');

                if (!GoToEx(int.Parse(infomap[0]), int.Parse(infomap[1]), int.Parse(infomap[2])))
                    return;

                if (!TLBB.IsQuestOpen)
                {
                    Talk(int.Parse(infomap[3]));
                    return;
                }

                foreach (QuestFrame dialog in QuestFrame.Enum(this))
                {
                    if (TDT.VietLien(dialog.Name.Substring(10)).Contains(TDT.VietLien(TenThanh())))
                    {
                        QuestFrameOptionClicked(dialog);
                        break;
                    }
                }
                CloseQuest();
                return;
            }

            if (GoToEx(npcBang))
            {
                if (!TLBB.IsQuestOpen)
                {
                    Talk(npcBang.Id);
                }
                else
                {
                    foreach (QuestFrame dialog in QuestFrame.Enum(this))
                    {
                        //#{BHCS_090219_07}
                        if (dialog.Name.Contains("#{BHCS_090219_07}") || dialog.Name.Contains("Vào thành thị của bổn bang"))
                        {
                            QuestFrameOptionClicked(dialog);
                            break;
                        }
                    }
                    CloseQuest();
                }
            }
        }

        public void PackUp()
        {
            if (Global.IsPackUpDanBuon)
            {
                if (SecCount % 10 == 0)
                {
                    LUA.PackUp();
                    Thread.Sleep(1000);
                }
            }
        }

        public void RaBang(int map)
        {
            if (TLBB.MapId < 500)
                return;

            if (Global.Mapbang && IsDuaHau)
            {
                if (!GoToEx(100, 158))
                    return;
            }

            if (!GoToEx(BANG.TrinhVoDanh))
                return;
            TalkEx("trinhvodanh");
            Thread.Sleep(500);
            foreach (QuestFrameItem item in QuestFrame.Items)
            {
                if (item.Name.Contains("_090226_10"))
                {
                    QuestFrameOptionClicked(item);
                    Thread.Sleep(500);
                    break;
                }
            }
            foreach (QuestFrameItem item in QuestFrame.Items)
            {
                if (map == 1)
                {
                    if (item.Name.Contains("#{BHCS_090219_03}"))
                    {
                        QuestFrameOptionClicked(item);
                        break;
                    }
                }
                else if (map == 2)
                {
                    if (item.Name.Contains("#{BHCS_090219_02}"))
                    {
                        QuestFrameOptionClicked(item);
                        break;
                    }
                }
                else
                {
                    if (item.Name.Contains("Quay về Lạc Dương"))
                    {
                        QuestFrameOptionClicked(item);
                        break;
                    }
                }
            }
            CloseQuest();
        }



        public int CapHopDienTich { get; set; }
        public int CapHopVoHon { get; set; }

        public void HopVoHon()
        {
            if (!Global.IsHopDienTichVoHon)
                return;
            if (!Missions.Contains(MissionsType.HopVoHon))
                return;

            int cnt = 0;
            while (true)
            {
                if (cnt++ > 30)
                    break;
                if (!Missions.Contains(MissionsType.HopVoHon))
                    return;
                bool isCat = false;
                List<uint> list = PacketItems.ThienCoTrong.ToList();
                foreach (var item in PacketItems.All)
                {
                    if (item.TypeName.Equals("Võ Hồn"))
                    {
                        if (list.Count > 0)
                        {
                            PushItemThienCo(item.Index, (int)list[0]);
                            isCat = true;
                            list.RemoveAt(0);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                if (isCat)
                {
                    DoStringEx("setmetatable(_G, {__index = Packet_Temporary_Env}); Packet_Temporary_CleanButtonClk(); ");
                }
                Thread.Sleep(350);
                Dictionary<int, int> diccodinh = new Dictionary<int, int>();
                Dictionary<int, int> dickocodinh = new Dictionary<int, int>();
                foreach (PacketItem packetItem in PacketItems.ThienCo)
                {
                    if (packetItem.TypeName.Equals("Võ Hồn"))
                    {
                        if (CapHopVoHon < packetItem.CapHopThanhVoHon)
                            continue;
                        if (packetItem.IsCoDinh)
                        {
                            if (!diccodinh.ContainsKey((int)packetItem.Index))
                            {
                                diccodinh.Add((int)packetItem.Index, packetItem.CapHopThanhVoHon);
                            }
                        }
                        else
                        {
                            if (!dickocodinh.ContainsKey((int)packetItem.Index))
                            {
                                dickocodinh.Add((int)packetItem.Index, packetItem.CapHopThanhVoHon);
                            }
                        }
                    }
                }
                bool ishop = false;
                for (int i = 0; i <= CapHopVoHon; i++)
                {
                    if (diccodinh.Where(kvp => kvp.Value == i).Count() >= 2)
                    {
                        ishop = true;
                        GetItemThienCo((uint)diccodinh.Where(kvp => kvp.Value == i).ToList()[0].Key);
                        GetItemThienCo((uint)diccodinh.Where(kvp => kvp.Value == i).ToList()[1].Key);
                        Thread.Sleep(350);
                        List<int> listdientich = new List<int>();
                        foreach (var item in PacketItems.All)
                        {
                            if (item.TypeName.Equals("Võ Hồn") && item.CapHopThanhVoHon == i)
                            {
                                listdientich.Add((int)item.Index);
                                if (listdientich.Count == 2)
                                    break;
                            }
                        }
                        HopVoHon(listdientich[0], listdientich[1]);
                        break;
                    }
                    if (dickocodinh.Where(kvp => kvp.Value == i).Count() >= 2)
                    {
                        ishop = true;
                        GetItemThienCo((uint)dickocodinh.Where(kvp => kvp.Value == i).ToList()[0].Key);
                        GetItemThienCo((uint)dickocodinh.Where(kvp => kvp.Value == i).ToList()[1].Key);
                        Thread.Sleep(350);
                        List<int> listdientich = new List<int>();
                        foreach (var item in PacketItems.All)
                        {
                            if (item.TypeName.Equals("Võ Hồn") && item.CapHopThanhVoHon == i)
                            {
                                listdientich.Add((int)item.Index);
                                if (listdientich.Count == 2)
                                    break;
                            }
                        }
                        HopVoHon(listdientich[0], listdientich[1]);
                        break;
                    }
                }
                if (!ishop && !isCat)
                    break;
            }
            RemoveMission(MissionsType.HopVoHon);
            PushDebugMessage("Hợp Xong Rồi Nhé");
        }

        public void HopDienTich()
        {
            if (!Missions.Contains(MissionsType.HopDienTich) || !Global.IsHopDienTichVoHon)
                return;
            int cnt = 0;
            while (true)
            {
                if (cnt++ > 30)
                    break;
                if (!Missions.Contains(MissionsType.HopDienTich))
                    break;
                bool isCat = false;
                List<uint> list = PacketItems.ThienCoTrong.ToList();
                foreach (var item in PacketItems.All)
                {
                    if (item.Name == "" || item.Type == "")
                        continue;
                    if (item.Type.Contains("Card_Icon"))
                    {
                        if (list.Count > 0)
                        {
                            PushItemThienCo(item.Index, (int)list[0]);
                            isCat = true;
                            list.RemoveAt(0);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                if (isCat)
                {
                    DoStringEx("setmetatable(_G, {__index = Packet_Temporary_Env}); Packet_Temporary_CleanButtonClk(); ");
                }
                Thread.Sleep(350);
                Dictionary<int, int> diccodinh = new Dictionary<int, int>();
                Dictionary<int, int> dickocodinh = new Dictionary<int, int>();
                foreach (PacketItem packetItem in PacketItems.ThienCo)
                {
                    if (packetItem.Type.Contains("Card_Icon"))
                    {
                        if (CapHopDienTich < packetItem.CardLevel)
                            continue;
                        if (packetItem.IsCoDinh)
                        {
                            if (!diccodinh.ContainsKey((int)packetItem.Index))
                            {
                                diccodinh.Add((int)packetItem.Index, packetItem.CardLevel);
                            }
                        }
                        else
                        {
                            if (!dickocodinh.ContainsKey((int)packetItem.Index))
                            {
                                dickocodinh.Add((int)packetItem.Index, packetItem.CardLevel);
                            }
                        }
                    }
                }
                bool ishop = false;
                for (int i = 1; i <= CapHopDienTich; i++)
                {
                    if (diccodinh.Where(kvp => kvp.Value == i).Count() >= 2)
                    {
                        ishop = true;
                        GetItemThienCo((uint)diccodinh.Where(kvp => kvp.Value == i).ToList()[0].Key);
                        GetItemThienCo((uint)diccodinh.Where(kvp => kvp.Value == i).ToList()[1].Key);
                        Thread.Sleep(350);
                        List<int> listdientich = new List<int>();
                        foreach (var item in PacketItems.All)
                        {
                            if (item.Type.Contains("Card_Icon") && item.CardLevel == i)
                            {
                                listdientich.Add((int)item.Index);
                                if (listdientich.Count == 2)
                                    break;
                            }
                        }
                        DoStringEx("g_SrcBagIndex = " + listdientich[0] + ";"
                                  + "g_DestBagIndex = " + listdientich[1] + ";"
                                  + @"srcBindStatus = PlayerPackage:GetItemBindStatusByIndex(g_SrcBagIndex);

                                    destBindStatus = PlayerPackage:GetItemBindStatusByIndex(g_DestBagIndex);

                                    if srcBindStatus ~= destBindStatus then
                                        PushDebugMessage('Không Hợp 2 Điển Tịch Cố Định Và Không Cố Định');
                                        return;
                                    end

                                    Clear_XSCRIPT();
                                    Set_XSCRIPT_Function_Name('CompositeBooks');
                                    Set_XSCRIPT_ScriptID(890630);
                                    Set_XSCRIPT_Parameter(0, g_SrcBagIndex);
                                    Set_XSCRIPT_Parameter(1, g_DestBagIndex);
                                    Set_XSCRIPT_ParamCount(2);
                                    Send_XSCRIPT(); ");
                        break;
                    }
                    if (dickocodinh.Where(kvp => kvp.Value == i).Count() >= 2)
                    {
                        ishop = true;
                        GetItemThienCo((uint)dickocodinh.Where(kvp => kvp.Value == i).ToList()[0].Key);
                        GetItemThienCo((uint)dickocodinh.Where(kvp => kvp.Value == i).ToList()[1].Key);
                        Thread.Sleep(350);
                        List<int> listdientich = new List<int>();
                        foreach (var item in PacketItems.All)
                        {
                            if (item.Type.Contains("Card_Icon") && item.CardLevel == i)
                            {
                                listdientich.Add((int)item.Index);
                                if (listdientich.Count == 2)
                                    break;
                            }
                        }
                        DoStringEx("g_SrcBagIndex = " + listdientich[0] + ";"
                                  + "g_DestBagIndex = " + listdientich[1] + ";"
                                  + @"srcBindStatus = PlayerPackage:GetItemBindStatusByIndex(g_SrcBagIndex);

                                    destBindStatus = PlayerPackage:GetItemBindStatusByIndex(g_DestBagIndex);

                                    if srcBindStatus ~= destBindStatus then
                                        PushDebugMessage('Không Hợp 2 Điển Tịch Cố Định Và Không Cố Định');
                                        return;
                                    end

                                    Clear_XSCRIPT();
                                    Set_XSCRIPT_Function_Name('CompositeBooks');
                                    Set_XSCRIPT_ScriptID(890630);
                                    Set_XSCRIPT_Parameter(0, g_SrcBagIndex);
                                    Set_XSCRIPT_Parameter(1, g_DestBagIndex);
                                    Set_XSCRIPT_ParamCount(2);
                                    Send_XSCRIPT(); ");
                        break;
                    }
                }
                if (!ishop && !isCat)
                    break;
            }
            RemoveMission(MissionsType.HopDienTich);
            PushDebugMessage("Hợp Xong Rồi Nhé");
        }


        public void VoTuPho()
        {
            if (GoToEx(DAILY.ThamMinhLau))
            {
                Talk(DAILY.ThamMinhLau);
                Thread.Sleep(500);
                if (TLBB.IsQuestOpen)
                {
                    QuestFrame.Click("#{XZYD_200401_42}");
                    Thread.Sleep(500);
                    QuestFrame.Accept();
                    Talk(DAILY.ThamMinhLau);
                    Thread.Sleep(500);
                    QuestFrame.Click("#{XZYD_200401_42}");
                    Thread.Sleep(500);
                    QuestFrame.Click(1);
                    Thread.Sleep(500);
                    QuestFrame.Click(1);
                    Thread.Sleep(500);
                    QuestFrame.Click(1);
                    Thread.Sleep(500);
                    QuestFrame.Click(1);
                    Thread.Sleep(500);
                    QuestFrame.Complete();
                    RemoveMission(MissionsType.VoTuPho);
                }
            }
        }

        public bool HaveItem(string type)
        {
            foreach (var item in PacketItems.All)
            {
                if (item.Type.Trim() == type.Trim() || item.ClearName == type.VietLien())
                    return true;
            }
            return false;
        }

        public bool HaveNhuYHa { get; set; }
        public bool HaveCatTuongKhanh { get; set; }
        public bool HaveKhoaiHanhLac { get; set; }

        public void CheckTueHong()
        {
            HaveNhuYHa = HaveCatTuongKhanh = HaveKhoaiHanhLac = false;
            foreach (var item in PacketItems.All)
            {
                string clearname = item.ClearName;
                if (clearname.Contains("nhuyha"))
                    HaveNhuYHa = true;
                else if (clearname.Contains("cattuongkhanh"))
                    HaveCatTuongKhanh = true;
                else if (clearname.Contains("khoailachanh"))
                    HaveKhoaiHanhLac = true;
            }
            if (!HaveNhuYHa)
                PushMissions(MissionsType.TruAc);
            if (!HaveKhoaiHanhLac)
                PushMissions(MissionsType.ThuTaiVanMay);
            if (!HaveCatTuongKhanh)
                PushMissions(MissionsType.NhiemVuSuMon);
            PushMissions(MissionsType.ThienLongTueHong);
            IsOneTNT = true;
        }

        private Stopwatch uncheck = Stopwatch.StartNew();

        public void UncheckTueHong()
        {
            if (uncheck.Elapsed.TotalSeconds < 3)
                return;
            uncheck = Stopwatch.StartNew();
            //if (TLBB.OnlineTimeSec <= 60)
            //    return;
            HaveNhuYHa = HaveCatTuongKhanh = HaveKhoaiHanhLac = false;
            foreach (var item in PacketItems.All)
            {
                string clearname = item.ClearName;
                if (clearname.Contains("nhuyha"))
                    HaveNhuYHa = true;
                else if (clearname.Contains("cattuongkhanh"))
                    HaveCatTuongKhanh = true;
                else if (clearname.Contains("khoailachanh"))
                    HaveKhoaiHanhLac = true;
            }
            if (HaveNhuYHa)
                RemoveMission(MissionsType.TruAc);
            if (HaveCatTuongKhanh)
                RemoveMission(MissionsType.NhiemVuSuMon);
            if (Missions.Contains(MissionsType.NhiemVuSuMon) || Missions.Contains(MissionsType.TruAc))
            {
                if (IsOneTNT)
                {
                    if (!HaveKhoaiHanhLac && !Missions.Contains(MissionsType.ThuTaiVanMay) && SafeTime >= 3)
                    {
                        RemoveMission(MissionsType.NhiemVuSuMon);
                        RemoveMission(MissionsType.TruAc);
                        IsXongBHD = true;
                    }
                }
            }
        }

        public bool IsOneTNT { get; set; }

        public void DoAction(GameControl control)
        {
            PostMessage(control.Object, 112);
        }

        public void DoSubAction(GameControl control)
        {
            PostMessage(control.Object, 124);
        }

        public void RaKhoiGiamNguc()
        {
            if (TLBB.MapId != MAP.GiamNguc)
                return;
            if (TrueStandTime.Elapsed.TotalSeconds <= 5)
                return;
            if (GoToEx(GIAMNGUC.TruongChinhQuy))
            {
                if (TLBB.IsQuestOpen)
                {
                    foreach (QuestFrame dialog in QuestFrame.Enum(this))
                    {
                        if (dialog.StrOptionExtra1 == 77011 && (dialog.StrOptionExtra2 == 1 || dialog.StrOptionExtra2 == 11))
                        {
                            QuestFrameOptionClicked(dialog);
                            return;
                        }
                    }
                    QuestFrame.Close();
                    Thread.Sleep(5000);
                }
                else
                {
                    foreach (GameObject _object in Objects.All)
                    {
                        if (TDT.VietLien(_object.Name).Contains("truongchinhquy"))
                        {
                            Talk(_object.Id);
                        }
                    }
                }
            }
        }

        public void GiaiDoc()
        {
            if (TLBB.MapId != MAP.YenTuO || !Objects.Self.Buff.Contains(22))
            {
                return;
            }
            if (Address.GameType == 1)
            {
                foreach (PacketItem item in PacketItems.ThienCo)
                {
                    if (item.ClearName.Contains("thuocgiai") && item.ClearName.Contains("bitothanhphong"))
                    {
                        GetItemThienCo(item.Index);
                        Thread.Sleep(350);
                        break;
                    }
                }
            }
            foreach (var item in PacketItems.DaoCu)
            {
                if ((item.ClearName.Contains("thuocgiai") && item.ClearName.Contains("bitothanhphong")) || TDT.VietLien(item.Name) == "antidoteofbitothanhphong")
                {
                    item.DoAction();
                    break;
                }
            }
        }


        public void DinhViThoLinhChau(bool isAll = false)
        {
            foreach (var item in PacketItems.All)
            {
                if (item.Type == "Cloth2_9" || item.Type == "CircularTaskTool64_5")
                {
                    item.DoAction();
                    Thread.Sleep(150);
                    DoStringEx("setmetatable(_G, {__index = Item_TuDunZhu_Env}); if this:IsVisible() then Item_TuDunZhu_OK_Clicked(); end");
                    Thread.Sleep(150);
                    LUA.MessageBox_Self_OK_Clicked();
                    Thread.Sleep(150);
                    if (!isAll)
                        break;
                }
            }
        }

        public bool DoSubAction(string type, uint packetId)
        {
            foreach (GameControl control in GameControl.Enum(this))
            {
                if (control.Type == type && control.PacketId == packetId)
                {
                    DoSubAction(control);
                    return true;
                }
            }
            return false;
        }

        public bool DoAction(string type, uint packetId = 0xFFFFFFFF)
        {
            foreach (GameControl control in GameControl.Enum(this))
            {
                if (control.Type == type)
                {
                    if (control.PacketId == packetId || packetId == 0xFFFFFFFF)
                    {
                        DoAction(control);
                        return true;
                    }
                }
            }
            return false;
        }

        public void ThuPet()
        {
            DoAction("PetSkill2_2");
        }

        public int TuoiNuoc()
        {
            foreach (var item in PacketItems.All)
            {
                if (item.Type == "CircularTaskTool103_2" || item.Type == "TaskTools7_1" || TDT.VietLien(item.Name).Contains("thuyho") || item.Type == "TaskTools5_9" || item.Type == "TaskTools6_3" || item.Type == "TaskTools5_4" || item.Type == "CircularTaskTool49_15" || item.Type == "TaskTools5_7" || item.Type == "TaskTools5_6" || item.Type.Contains("TaskTools5_8") || item.Type == "TaskTools6_7" || item.Type == "TaskTools6_1" || item.Type == "TaskTools6_10" || item.Type == "TaskTools5_5" || item.Type == "CircularTaskTool38_15")
                {
                    item.Use();
                    MissionState = "";
                }
            }
            return -1;
        }

        public int UseItemNhiemVu()
        {
            foreach (var item in PacketItems.All)
            {
                if (item.Type.Contains("TaskTools6_1") || item.Type.Contains("CircularTaskTool53_2") || item.Type.Contains("CircularTaskTool52_10"))
                {
                    item.Use();
                    Thread.Sleep(100);
                }
                if (item.Type == "CircularTaskTool52_15" || item.Type == "CircularTaskTool52_6" || item.Type == "CircularTaskTool51_7" || item.Type == "CircularTaskTool38_15" || item.Type == "Medicine4_12" || item.Type == "CircularTaskTool53_7")
                {
                    item.Use();
                    Thread.Sleep(100);
                }
                if ("CircularTaskTool51_11".Contains(item.Type.Trim()))
                {
                    item.Use();
                    Thread.Sleep(100);
                }
            }
            return -1;
        }

        public int PhuIndex(int map)
        {
            foreach (var item in PacketItems.All)
            {
                if ((item.ClearName.Contains("dinhviphu") || item.ClearName.Contains("dialinhchau") || item.ClearName.Contains("tholinhchau") || item.ClearName.Contains("lightdustcapsule")) && item.MapId == map)
                {
                    if (item.X > 0 && item.MapId == map)
                        return (int)item.Index;
                }
            }
            return -1;
        }

        public int PhuIndex()
        {
            foreach (var item in PacketItems.All)
            {
                if ((TDT.VietLien(item.Name).Contains("dinhviphu") || TDT.VietLien(item.Name).Contains("tholinhchau") || TDT.VietLien(item.Name).Contains("dialinhchau")))
                {
                    if (item.X > 0)
                    {
                        if (item.MapId == 0 || item.MapId == 1 || item.MapId == 2)
                            return (int)item.Index;
                    }
                }
            }
            return -1;
        }

        public void CloseQuest()
        {
            PostMessage(0, 122);
        }

        public void DoiLoanPhiMatHam()
        {
            if (TLBB.PlayerState != 0)
                return;
            if (!IsOneSec)
                return;
            if (GoToEx(LACDUONG.ToTriet))
            {
                if (!TLBB.IsQuestOpen)
                {
                    Talk(LACDUONG.ToTriet);
                }
                else
                {
                    if (QuestFrame.Click("#{YWBOSS_140117_07}"))
                    {
                        Thread.Sleep(300);
                        if (QuestFrame.Click("#{YWBOSS_140117_10}"))
                        {
                            Thread.Sleep(300);
                            LUA.QuestFrameMissionComplete();
                        }
                    }
                    else
                    {
                        QuestFrame.Close();
                    }
                }
            }
        }

        public void DoiChanNguyenLinhPhach()
        {
            if (TLBB.PlayerState != 0)
                return;
            if (!IsOneSec)
                return;
            if (GoToEx(LACDUONG.ToTriet))
            {
                if (!TLBB.IsQuestOpen)
                {
                    Talk(LACDUONG.ToTriet);
                }
                else
                {
                    if (QuestFrame.Click("#{YWBOSS_140117_07}"))
                    {
                        Thread.Sleep(300);
                        if (QuestFrame.Click("#{YWBOSS_140117_24}"))
                        {
                            Thread.Sleep(300);
                            LUA.QuestFrameMissionComplete();
                        }
                    }
                    else
                    {
                        QuestFrame.Close();
                    }
                }
            }
        }


        public void DoiTiemNangTan()
        {
            if (TLBB.PlayerState != 0)
                return;
            if (!IsOneSec)
                return;
            if (GoToEx(DAILY.TonBatGia))
            {
                if (!TLBB.IsQuestOpen)
                {
                    Talk(DAILY.TonBatGia);
                }
                else
                {
                    QuestFrame.Click("#{QNG_XML_11}");
                }
            }
        }

        public void TrangSucCuuLe()
        {
            if (TLBB.MapId != MAP.VuTrucCuong)
            {
                GoToEx(100, 100, MAP.VuTrucCuong);
            }
            else
            {
                if (TrueClearMonterTime.Elapsed.TotalSeconds > 3)
                {
                    if (TLBB.HaveRide && !TLBB.IsRide)
                    {
                        UpRide();
                        return;
                    }
                    MoveNext();
                }
                else
                {
                    DownRide();
                    ForceAttack();
                }
            }
        }

        public void ThanKhi9Sao()
        {
            if (!Global.Is9Sao || TLBB.Lvl < 102)
            {
                RemoveMission(MissionsType.ThanKhi9Sao);
                return;
            }

            if (MissionState == "")
            {
                if (!TLBB.IsTogleMission)
                {
                    PostMessage(18, 105);
                    Thread.Sleep(500);
                }
                PostMessage(18, 105);
                Thread.Sleep(500);
                DoStringEx("local cnt = 0; while true do local name, content = DataPool:GetPlayerMission_Memo(cnt); if string.find(name, 'JXMR_171027_06') then if DataPool:GetPlayerMission_Variable(cnt, 0) == 1 then return 'Xong'; else return content; end end cnt = cnt + 1; if cnt == 20 then return 'Chua'; end end");
                LuaToString();
                Thread.Sleep(500);
                string info = LuaString();
                Thread.Sleep(500);
                if (info.Contains("#{JXMR_171027_04}"))
                {
                    MissionState = "Do";
                }
                if (info.Contains("Chua"))
                {
                    MissionState = "Chua";
                }
                if (info.Contains("Xong"))
                {
                    MissionState = "Xong";
                }
                return;
            }
            if (MissionState == "Do")
            {
                if (TLBB.MapId != MAP.ThuongNgoBiCanh)
                {
                    GoToEx(100, 100, MAP.ThuongNgoBiCanh);
                }
                else
                {
                    if (ClearMonterTimeEx.Elapsed.TotalSeconds > 3 || SecCount % 15 == 0)
                    {
                        MissionState = "";
                        MoveNext();
                    }
                    else
                    {
                        DownRide();
                        ForceAttack();
                    }
                }
                return;
            }
            if (MissionState == "Chua" || MissionState == "Xong")
            {
                if (GoToEx(BichDuanXuanLam.LoanAnhBac))
                {
                    Talk(BichDuanXuanLam.LoanAnhBac);
                    Thread.Sleep(1000);
                    if (QuestFrame.Click("#{JXMR_171027_06}"))
                    {
                        Thread.Sleep(1000);
                        if (QuestFrame.Text.Contains("#{JXMR_171027_10}"))
                        {
                            MissionState = "";
                            RemoveMission(MissionsType.ThanKhi9Sao);
                            NeedToMove = "329,296,0";
                            return;
                        }
                        if (MissionState == "Xong")
                        {
                            LUA.QuestFrameMissionContinue();
                            Thread.Sleep(1000);
                            LUA.QuestFrameMissionComplete();
                            MissionState = "";
                            RemoveMission(MissionsType.ThanKhi9Sao);
                            NeedToMove = "329,296,0";
                        }
                        if (MissionState == "Chua")
                        {
                            QuestFrameAccept();
                            MissionState = "";
                        }
                    }
                }
                return;
            }
            MissionState = "";
        }

        private Stopwatch swrefreshluyenkim = Stopwatch.StartNew();

        public void LuyenKim()
        {

            if (TLBB.PlayerState == 7)
            {
            }
            else
            {
                if (TLBB.PlayerState != 0)
                    return;
            }

            StopFollow();
            if (!TLBB.IsMapBang(TLBB.MapId))
            {
                VaoBang();
                return;
            }
            if (ForcePickItem())
                return;

            if (swrefreshluyenkim.Elapsed.TotalMinutes >= 1)
            {
                swrefreshluyenkim = Stopwatch.StartNew();
                MissionState = "";
            }

            if (string.IsNullOrEmpty(MissionState))
            {
                MissionX = MissionY = 0;
                if (!TLBB.IsTogleMission)
                {
                    LUA.TOGLE_MISSION();
                    Thread.Sleep(1000);
                }
                LUA.CLOSE_MISSION();
                Thread.Sleep(1000);
                DoStringEx("local cnt = 0; while true do local name, content = DataPool:GetPlayerMission_Memo(cnt); if string.find(name, 'Nhi®m vø Luy®n Kim') then if DataPool:GetPlayerMission_Variable(cnt, 0) == 1 then return 'Xong'; else return content; end end cnt = cnt + 1; if cnt == 20 then return 'Chua'; end end");
                LuaToString();
                Thread.Sleep(1000);
                string info = LuaToString();
                if (info == "Chua")
                {
                    MissionState = "Chua";
                    return;
                }
                if (info.Contains("Đới Tam Kim") && !info.Contains("#{BHRWSC_110331_167}") && !info.Contains("#{BHRWSC_110331_168}") && !info.Contains("#{BHRWSC_110331_163}"))
                {
                    string point = TDT.StringBetween(info, "(", ")");

                    MissionX = TDT.ParseInt(point);
                    MissionY = TDT.ParseInt(point.Replace(MissionX + ",", ""));
                    MissionState = "ChuyenKhoang";
                    return;
                }
                if (info == "Xong")
                {
                    MissionState = "Xong";
                }
                else
                {
                    MissionState = "DaNhanLuyenKim";
                }
            }
            if (MissionState == "Chua")
            {
                if (GoToEx(BANG.DoiTamKim, true))
                {
                    foreach (GameObject _object in Objects.All)
                    {
                        if (TDT.VietLien(_object.Name).Contains("itamkim"))
                        {
                            Talk(_object.Id);
                            Thread.Sleep(1000);
                        }
                    }
                    foreach (QuestFrame dialog in QuestFrame.Enum(this))
                    {
                        if ((dialog.Name.Contains("_187") && Missions.Contains(MissionsType.LuyenKim)) || (dialog.Name.Contains("#{LJYH_141105_02}") && Missions.Contains(MissionsType.LuyenKimNhanh)))
                        {
                            QuestFrameOptionClicked(dialog);
                            Thread.Sleep(1000);
                            LUA.QuestFrameAcceptClicked();
                            Thread.Sleep(1000);
                            if (QuestFrame.All(this).Contains("#{LJYH_141105_06}") || QuestFrame.All(this).Contains("#{DJBHGZ_110511_08}"))
                            {
                                RemoveMission(MissionsType.LuyenKim);
                                RemoveMission(MissionsType.LuyenKimNhanh);
                                PushDebugMessage("Hết điểm năng động, dừng luyện kim");
                            }
                            MissionState = "";
                            break;
                        }
                    }
                }
                return;
            }
            if (MissionState == "DaNhanLuyenKim")
            {
                if (!TLBB.IsBienThan)
                {
                    if (GoToEx(BANG.DoiTamKim, true))
                    {
                        foreach (GameObject _object in Objects.All)
                        {
                            if (TDT.VietLien(_object.Name).Contains("itamkim"))
                            {
                                Talk(_object.Id);
                                Thread.Sleep(1000);
                                break;
                            }
                        }
                        if (TLBB.IsRide)
                        {
                            DownRide();
                        }
                        foreach (QuestFrame dialog in QuestFrame.Enum(this))
                        {
                            if (dialog.Name.Contains("_178"))
                            {
                                QuestFrameOptionClicked(dialog);
                                break;
                            }
                        }
                        MissionState = "";
                    }
                }
                else
                {
                    if (MissionX != 0)
                    {
                        if (TLBB.PlayerState == 7)
                        {
                            HuyBienThan();
                            MissionState = "";
                            return;
                        }
                        if (GoToEx(MissionX, MissionY))
                        {
                            SendKey(Keys.F1);
                            Thread.Sleep(1000);
                            MissionState = "";
                        }
                        return;
                    }
                    foreach (GameTask task in GameTask.Enum(this))
                    {
                        if (task.ClearName.Contains("nhiemvuluyenkim") && task.Completed)
                        {
                            MissionState = "";
                            return;
                        }
                    }
                    SendKey(Keys.F2);
                    Thread.Sleep(1000);
                    DoStringEx("return DEBUG;");
                    LuaToString();
                    Thread.Sleep(1000);
                    MissionX = 0;
                    MissionY = 0;
                    string info = LuaString();
                    string par = Regex.Replace(info, @".*_[\d]+", "");
                    if (info.Contains("#{BHRWSC_110331_22"))
                    {
                        par = Regex.Replace(info, @".*_[\d]+", "");
                        MissionX = TDT.ParseInt(par);
                        if (MissionX > 0)
                        {
                            par = ConverterEx.ReplaceFirst(par, MissionX.ToString(), "");
                            MissionY = TDT.ParseInt(par);
                        }
                        else
                        {
                            MissionX = MissionY = 0;
                        }
                    }
                    if (MissionX == 0)
                    {
                        info = VuKhoString;
                        par = Regex.Replace(info, @".*_[\d]+", "");
                        MissionX = TDT.ParseInt(par);
                        if (MissionX > 0)
                        {
                            par = ConverterEx.ReplaceFirst(par, MissionX.ToString(), "");
                            MissionY = TDT.ParseInt(par);
                        }
                    }
                    if (MissionX == 0)
                    {
                        MissionState = "";
                    }
                }
                return;
            }
            if (MissionState == "Xong")
            {
                if (TLBB.IsBienThan)
                {
                    HuyBienThan();
                    return;
                }
                if (GoToEx(BANG.DoiTamKim, true))
                {
                    foreach (GameObject _object in Objects.All)
                    {
                        if (TDT.VietLien(_object.Name).Contains("itamkim"))
                        {
                            Talk(_object.Id);
                            MissionState = "TraNhiemVu";
                        }
                    }
                }
                return;
            }
            if (MissionState == "TraNhiemVu")
            {
                foreach (QuestFrame dialog in QuestFrame.Enum(this))
                {
                    if ((dialog.Name.Contains("_187") && Missions.Contains(MissionsType.LuyenKim)) || (dialog.Name.Contains("#{LJYH_141105_02}") && Missions.Contains(MissionsType.LuyenKimNhanh)))
                    {
                        QuestFrameOptionClicked(dialog);
                        Thread.Sleep(1000);
                        LUA.QuestFrameMissionContinue();
                        Thread.Sleep(1000);
                        LUA.QuestFrameMissionComplete();
                        MissionState = "";
                        return;
                    }
                }
            }
            if (MissionState == "ChuyenKhoang")
            {
                if (TLBB.IsBienThan)
                {
                    if (GoToEx(BANG.DoiTamKim, true))
                    {
                        foreach (GameObject _object in Objects.All)
                        {
                            if (TDT.VietLien(_object.Name).Contains("itamkim"))
                            {
                                Talk(_object.Id);
                                MissionState = "TraNhiemVu";
                            }
                        }
                    }
                    return;
                }
                if (GoToEx(MissionX, MissionY))
                {
                    foreach (GameObject _object in Objects.All)
                    {
                        if (TDT.GetDistance(_object.X, _object.Y, MissionX, MissionY) < 2 && _object.IsNPC)
                        {
                            Talk(_object.Id);
                            Thread.Sleep(1000);
                            QuestFrame.Click("{BHRWSC_110331_69}");
                        }
                    }
                }
                return;
            }
            MissionState = "";
        }

        public void HuyVatPhamQuy()
        {
            if (!IsOneSec)
                return;
            if (GoToEx(LACDUONG.KhongTongUyen))
            {
                if (TLBB.IsQuestOpen)
                {
                    if (QuestFrame.Click("#{SCGZ_091119_03}"))
                    {
                        Thread.Sleep(350);
                        DoStringEx("setmetatable(_G, {__index = ProtectGoodsDel_Env}); ProtectGoodsDel_OnApply();");
                        RemoveMission(MissionsType.HuyVatPhamQuy);
                    }
                }
                else
                {
                    Talk(LACDUONG.KhongTongUyen);
                }
            }
        }

        private void DiemDanh()
        {
            if (Address.GameType == 1)
            {
                DoStringEx("setmetatable(_G, {__index = Huoyuehaoli_Env}); Huoyuehaoli_PlayHead(1); Huoyuehaoli_PlayHead(2); Huoyuehaoli_PlayHead(3); Huoyuehaoli_PlayHead(4);");
                DoStringEx("setmetatable(_G, {__index = MiniMap_Env}); this:Show();");
                TuChucPhuc();
                DoStringEx("CloseWindow('Sns'); CloseWindow('SnsGame'); ");
                Thread.Sleep(500);
             
                DoStringEx("Clear_XSCRIPT(); Set_XSCRIPT_Function_Name('GetPrize'); Set_XSCRIPT_ScriptID(891106); Set_XSCRIPT_Parameter(0, 1); Set_XSCRIPT_ParamCount(1); Send_XSCRIPT()");
                DoStringEx("Clear_XSCRIPT(); Set_XSCRIPT_Function_Name('GetPrize'); Set_XSCRIPT_ScriptID(891106); Set_XSCRIPT_Parameter(0, 2); Set_XSCRIPT_ParamCount(1); Send_XSCRIPT()");
                DoStringEx("Clear_XSCRIPT(); Set_XSCRIPT_Function_Name('GetPrize'); Set_XSCRIPT_ScriptID(891106); Set_XSCRIPT_Parameter(0, 3); Set_XSCRIPT_ParamCount(1); Send_XSCRIPT()");
                DoStringEx("Clear_XSCRIPT(); Set_XSCRIPT_Function_Name('GetPrize'); Set_XSCRIPT_ScriptID(891106); Set_XSCRIPT_Parameter(0, 4); Set_XSCRIPT_ParamCount(1); Send_XSCRIPT()");

                DoStringEx("Clear_XSCRIPT(); Set_XSCRIPT_Function_Name('GetPrize'); Set_XSCRIPT_ScriptID(891106); Set_XSCRIPT_Parameter(0, 5); Set_XSCRIPT_ParamCount(1); Send_XSCRIPT()");

                for (int i = 0; i < 11; i++)
                {
                    DoStringEx("setmetatable(_G, {__index = Fuli_ZhanLing_Env}); Fuli_ZhanLing_GetPrize(" + (i + 1) + ", 0)");
                }
                DoStringEx("setmetatable(_G, {__index = WuLin_Env}); WuLin_Member_OnChooseStageConfirm(); ");
                IsOpenPass2 = false;

                if (!TLBB.IsRide)
                    UseSkill(276);
            }
        }

        public void QuestFrameAccept()
        {
            PostMessage(13, 105);
        }


        public void TuDuong()
        {
            if (!IsOneSec)
                return;
            if (TLBB.PlayerState != 0)
                return;
            if (IdleTime > 20)
            {
                MissionState = "";
                IdleTime = 0;
            }
            if (MissionState == "")
            {
                if (!TLBB.IsTogleMission)
                {
                    TogleMission();
                    Thread.Sleep(1000);
                }
                TogleMission();
                Thread.Sleep(1000);
                DoStringEx(@"local cnt = 0
local xong = 'Xong,'
while true do
    local name, content = DataPool:GetPlayerMission_Memo(cnt)
    if string.find(name, '#{SXPY_130826_32}') then
        if DataPool:GetPlayerMission_Variable(cnt, 0) ~= 1 then
            PushDebugMessage('XØ lý nhi®m vø ð¯i th½')
            return 'DoiTho'
        end
        xong = xong .. 'DoiTho,'
    end
    if string.find(name, '#{SXPY_130826_78}') then
        if DataPool:GetPlayerMission_Variable(cnt, 0) ~= 1 then
            local pos = {
                [1] = {
                    [1] = {SceneID = 0, PosX = 153, PosZ = 158, SceneName = 'LÕc Dß½ng'},
                    [2] = {SceneID = 0, PosX = 295, PosZ = 260, SceneName = 'LÕc Dß½ng'},
                    [3] = {SceneID = 0, PosX = 226, PosZ = 419, SceneName = 'LÕc Dß½ng'},
                    [4] = {SceneID = 1, PosX = 367, PosZ = 171, SceneName = 'Tô Châu'},
                    [5] = {SceneID = 1, PosX = 278, PosZ = 326, SceneName = 'Tô Châu'},
                    [6] = {SceneID = 1, PosX = 181, PosZ = 355, SceneName = 'Tô Châu'}
                },
                [2] = {
                    [1] = {SceneID = 2, PosX = 70, PosZ = 49, SceneName = 'ÐÕi Lý'},
                    [2] = {SceneID = 2, PosX = 293, PosZ = 46, SceneName = 'ÐÕi Lý'},
                    [3] = {SceneID = 2, PosX = 69, PosZ = 235, SceneName = 'ÐÕi Lý'},
                    [4] = {SceneID = 18, PosX = 156, PosZ = 277, SceneName = 'NhÕn Nam'},
                    [5] = {SceneID = 18, PosX = 250, PosZ = 82, SceneName = 'NhÕn Nam'},
                    [6] = {SceneID = 18, PosX = 138, PosZ = 242, SceneName = 'NhÕn Nam'}
                },
                [3] = {
                    [1] = {SceneID = 4, PosX = 168, PosZ = 189, SceneName = 'Thái H°'},
                    [2] = {SceneID = 4, PosX = 62, PosZ = 74, SceneName = 'Thái H°'},
                    [3] = {SceneID = 4, PosX = 100, PosZ = 175, SceneName = 'Thái H°'},
                    [4] = {SceneID = 30, PosX = 124, PosZ = 50, SceneName = 'Tây H°'},
                    [5] = {SceneID = 30, PosX = 246, PosZ = 193, SceneName = 'Tây H°'},
                    [6] = {SceneID = 30, PosX = 154, PosZ = 132, SceneName = 'Tây H°'}
                }
            }
            local index = DataPool:GetPlayerMission_Variable(cnt, 2)
            local sub = DataPool:GetPlayerMission_Variable(cnt, 3)
            local city = pos[index][sub].SceneName
            local x = pos[index][sub].PosX
            local y = pos[index][sub].PosZ
            local map = pos[index][sub].SceneID
            PushDebugMessage('XØ lý nhi®m vø h÷c v¨')
            return 'Ve,' .. x .. ',' .. y .. ',' .. map
        end
        xong = xong .. 'Ve,'
    end
    if string.find(name, '#{SXPY_130826_108}') then
        if DataPool:GetPlayerMission_Variable(cnt, 0) ~= 1 then
            PushDebugMessage('XØ lý nhi®m vø h÷c l­ nghi')
            return 'LeNghi,' .. DataPool:GetPlayerMission_Variable(cnt, 2)
        end
        xong = xong .. 'LeNghi,'
    end
    if string.find(name, '#{SXPY_130826_144}') then
        if DataPool:GetPlayerMission_Variable(cnt, 0) ~= 1 then
            PushDebugMessage('XØ lý nhi®m vø tÖ thí âm lu§t')
            return 'AmLuat'
        end
        xong = xong .. 'AmLuat,'
    end
    if string.find(name, '#{SXPY_130826_188}') then
        if DataPool:GetPlayerMission_Variable(cnt, 0) ~= 1 then
            PushDebugMessage('XØ lý nhi®m vø h÷c b¡n cung')
            return 'BanCung'
        end
        xong = xong .. 'BanCung,'
    end
    cnt = cnt + 1
    if cnt == 20 then
        return xong
    end
end
");
                MissionState = "GetTuDuongInfo";
                LuaToString();
                Thread.Sleep(1000);
            }
            if (MissionState == "GetTuDuongInfo")
            {
                MissionInfo = LuaToString();
                if (MissionInfo == "DoiTho")
                {
                    MissionState = "DoiTho";
                }
                if (MissionInfo == "BanCung")
                {
                    MissionState = "BanCung";
                }
                if (MissionInfo.StartsWith("Ve"))
                {
                    MissionState = "Ve";
                    try
                    {
                        string[] info = MissionInfo.Split(',');
                        MissionX = int.Parse(info[1]);
                        MissionY = int.Parse(info[2]);
                        MissionMap = int.Parse(info[3]);
                    }
                    catch { }
                }
                if (MissionInfo.StartsWith("LeNghi"))
                {
                    MissionState = "LeNghi";
                    try
                    {
                        string[] info = MissionInfo.Split(',');
                        int npcIndex = int.Parse(info[1]);
                        MissionMap = 0;
                        if (npcIndex == 1)
                        {
                            MissionX = 118;
                            MissionY = 138;
                        }
                        else if (npcIndex == 2)
                        {
                            MissionX = 118;
                            MissionY = 132;
                        }
                        else
                        {
                            MissionX = 127;
                            MissionY = 132;
                        }
                    }
                    catch { }
                }
                if (MissionInfo == "AmLuat")
                {
                    MissionState = "AmLuat";
                }
                if (MissionInfo.Contains("Xong,"))
                {
                    if (MissionInfo.Length > 5)
                        MissionState = "TraNhiemVu";
                    else
                        MissionState = "NhanNhiemVu";
                }
            }
            if (MissionState == "AmLuat")
            {
                if (GoToEx(162, 266, LACDUONG.Id))
                {
                    foreach (GameObject _object in Objects.All)
                    {
                        if (TDT.VietLien(_object.Name).Contains("truongbatri"))
                        {
                            Talk(_object.Id);
                            MissionState = "TalkToAmLuat";
                            return;
                        }
                    }
                }
            }
            if (MissionState == "TalkToAmLuat")
            {
                foreach (QuestFrame dialog in QuestFrame.Enum(this))
                {
                    if (!dialog.Name.Contains("#c7777777"))
                    {
                        QuestFrameOptionClicked(dialog);
                    }
                    if (dialog.Name.Contains("SXPY_130826_172}"))
                    {
                        MissionState = "";
                        return;
                    }
                    if (dialog.Name.Contains("{SXPY_130826_149}"))
                    {
                        MissionState = "";
                        return;
                    }
                }
            }
            if (MissionState == "LeNghi")
            {
                if (GoToEx(MissionX, MissionY, MissionMap))
                {
                    if (TLBB.IsRide)
                    {
                        DownRide();
                        return;
                    }
                    foreach (GameObject _object in Objects.All)
                    {
                        if (TDT.GetDistance(_object.X, _object.Y, MissionX, MissionY) < 2 && _object.IsNPC)
                        {
                            Talk(_object.Id);
                            MissionState = "TalkToLeNghi";
                            return;
                        }
                    }
                }
            }
            if (MissionState == "TalkToLeNghi")
            {
                foreach (QuestFrame dialog in QuestFrame.Enum(this))
                {
                    if (dialog.Name.Contains("SXPY_130826_12"))
                    {
                        QuestFrameOptionClicked(dialog);
                        MissionState = "TienHanhLeNghi";
                        return;
                    }
                }
            }
            if (MissionState == "TienHanhLeNghi")
            {
                if (TLBB.IsBienThan)
                {
                    SendKey(Keys.F1);
                    SendKey(Keys.F2);
                    SendKey(Keys.F3);
                }
                else
                {
                    MissionState = "";
                }
            }
            if (MissionState == "Ve")
            {
                if (GoToEx(MissionX, MissionY, MissionMap))
                {
                    if (TLBB.IsRide)
                    {
                        DownRide();
                        return;
                    }
                    DoAction("CircularTaskTool50_10");
                    MissionState = "";
                }
            }
            if (MissionState == "DoiTho")
            {
                if (GoToEx(221, 236, LACDUONG.Id))
                {
                    foreach (GameObject _object in Objects.All)
                    {
                        if (TDT.Contain(_object.Name, "thaibinhthan"))
                        {
                            Talk(_object.Id);
                            MissionState = "TalkToDoiTho";
                            return;
                        }
                    }
                }
            }
            else if (MissionState == "TalkToDoiTho")
            {
                foreach (QuestFrame dialog in QuestFrame.Enum(this))
                {
                    if (!dialog.Name.Contains("#c7777777"))
                    {
                        QuestFrameOptionClicked(dialog);
                    }
                    if (dialog.Name.Contains("SXPY_130826_60"))
                    {
                        MissionState = "";
                        return;
                    }
                    if (dialog.Name.Contains("{SXPY_130826_37}"))
                    {
                        MissionState = "";
                        return;
                    }
                }
            }
            if (MissionState == "BanCung")
            {
                if (GoToEx(176, 150, LACDUONG.Id))
                {
                    if (TLBB.IsRide)
                    {
                        DownRide();
                        return;
                    }
                    foreach (GameObject _object in Objects.All)
                    {
                        if (TDT.VietLien(_object.Name).Contains("biatapban") && _object.Buff.Contains(2748))
                        {
                            SelectTarget(_object.Id);
                            if (IdleTime > 2)
                            {
                                DoAction("CircularTaskTool50_12");
                            }
                            if (IdleTime > 5)
                            {
                                MissionState = string.Empty;
                            }
                        }
                    }
                }
            }
            if (MissionState == "TraNhiemVu")
            {
                if (GoToEx(151, 187, LACDUONG.Id))
                {
                    foreach (GameObject _object in Objects.All)
                    {
                        if (TDT.VietLien(_object.Name).Contains("moctutyty"))
                        {
                            Talk(_object.Id);
                            MissionState = "TalkToTraNhiemVu";
                            return;
                        }
                    }
                }
            }
            if (MissionState == "TalkToTraNhiemVu")
            {
                foreach (QuestFrame dialog in QuestFrame.Enum(this))
                {
                    if (dialog.Name.Contains("{SXPY_130826_1}"))
                    {
                        QuestFrameOptionClicked(dialog);
                        return;
                    }
                }
                foreach (QuestFrame dialog in QuestFrame.Enum(this))
                {
                    if (dialog.Name.Contains("{SXPY_130826_3}") && MissionInfo.Contains("DoiTho"))
                    {
                        MissionInfo = MissionInfo.Replace("DoiTho,", "");
                        QuestFrameOptionClicked(dialog);
                        MissionState = "TraNV1";
                        return;
                    }
                    if (dialog.Name.Contains("{SXPY_130826_4}") && MissionInfo.Contains("Ve"))
                    {
                        MissionInfo = MissionInfo.Replace("Ve,", "");
                        QuestFrameOptionClicked(dialog);
                        MissionState = "TraNV1";
                        return;
                    }
                    if (dialog.Name.Contains("{SXPY_130826_5}") && MissionInfo.Contains("LeNghi"))
                    {
                        MissionInfo = MissionInfo.Replace("LeNghi,", "");
                        QuestFrameOptionClicked(dialog);
                        MissionState = "TraNV1";
                        return;
                    }
                    if (dialog.Name.Contains("{SXPY_130826_6}") && MissionInfo.Contains("AmLuat"))
                    {
                        MissionInfo = MissionInfo.Replace("AmLuat,", "");
                        QuestFrameOptionClicked(dialog);
                        MissionState = "TraNV1";
                        return;
                    }
                    if (dialog.Name.Contains("{SXPY_130826_7}") && MissionInfo.Contains("BanCung"))
                    {
                        MissionInfo = MissionInfo.Replace("BanCung,", "");
                        QuestFrameOptionClicked(dialog);
                        MissionState = "TraNV1";
                        return;
                    }
                }
                MissionState = "";
            }
            if (MissionState == "TraNV1")
            {
                PostMessage(15, 105);
                if (MissionInfo.Length > 5)
                {
                    MissionState = "TraNhiemVu";
                }
                else
                {
                    MissionState = "NhanNhiemVu";
                    MissionInfo = "";
                }
                return;
            }
            if (MissionState == "NhanNhiemVu")
            {
                if (GoToEx(151, 187, LACDUONG.Id))
                {
                    foreach (GameObject _object in Objects.All)
                    {
                        if (TDT.VietLien(_object.Name).Contains("moctutyty"))
                        {
                            Talk(_object.Id);
                            MissionState = "TalkToNhanNhiemVu";
                            return;
                        }
                    }
                }
            }
            if (MissionState == "TalkToNhanNhiemVu")
            {
                if (MissionInfo.Contains("DoiTho") && MissionInfo.Contains("Ve") && MissionInfo.Contains("LeNghi") && MissionInfo.Contains("AmLuat") && MissionInfo.Contains("BanCung"))
                {
                    MissionInfo = MissionState = "";
                }
                foreach (QuestFrame dialog in QuestFrame.Enum(this))
                {
                    if (dialog.Name.Contains("{SXPY_130826_1}"))
                    {
                        QuestFrameOptionClicked(dialog);
                        return;
                    }
                }
                foreach (QuestFrame dialog in QuestFrame.Enum(this))
                {
                    if (dialog.Name.Contains("{SXPY_130826_3}") && !MissionInfo.Contains("DoiTho"))
                    {
                        MissionInfo += "DoiTho,";
                        QuestFrameOptionClicked(dialog);
                        MissionState = "NhanNhiemVu";
                        return;
                    }
                    if (dialog.Name.Contains("{SXPY_130826_4}") && !MissionInfo.Contains("Ve"))
                    {
                        MissionInfo += "Ve,";
                        QuestFrameOptionClicked(dialog);
                        MissionState = "NhanNhiemVu";
                        return;
                    }
                    if (dialog.Name.Contains("{SXPY_130826_5}") && !MissionInfo.Contains("LeNghi"))
                    {
                        MissionInfo += "LeNghi,";
                        QuestFrameOptionClicked(dialog);
                        MissionState = "NhanNhiemVu";
                        return;
                    }
                    if (dialog.Name.Contains("{SXPY_130826_6}") && !MissionInfo.Contains("AmLuat"))
                    {
                        MissionInfo += "AmLuat,";
                        QuestFrameOptionClicked(dialog);
                        MissionState = "NhanNhiemVu";
                        return;
                    }
                    if (dialog.Name.Contains("{SXPY_130826_7}") && !MissionInfo.Contains("BanCung"))
                    {
                        MissionInfo += "BanCung,";
                        QuestFrameOptionClicked(dialog);
                        MissionState = "NhanNhiemVu";
                        return;
                    }
                }
                MissionState = "";
            }
        }

        private void TogleMission()
        {
            PostMessage(18, 105);
        }



        public int[,] AcBaPoint
        {
            get
            {
                string txt = SettingOld.LoadMAP(TLBB.MapAcBa.ToString());
                int cnt = 0;
                foreach (string s in txt.Split('-'))
                {
                    int x = 0;
                    int y = 0;
                    try
                    {
                        x = TDT.ParseInt(s.Split(',')[0]);
                        y = TDT.ParseInt(s.Split(',')[1]);
                    }
                    catch { }
                    if (x != 0 && y != 0)
                    {
                        cnt++;
                    }
                }
                if (cnt > 1)
                {
                    int[,] Point = new int[cnt, 2];
                    int c = 0;
                    foreach (string s in txt.Split('-'))
                    {
                        int x = 0;
                        int y = 0;
                        try
                        {
                            x = TDT.ParseInt(s.Split(',')[0]);
                            y = TDT.ParseInt(s.Split(',')[1]);
                        }
                        catch { }
                        if (x != 0 && y != 0)
                        {
                            Point[c, 0] = x;
                            Point[c, 1] = y;
                            c++;
                        }
                    }
                    return Point;
                }
                return null;
            }
        }

        public int[,] TamTaiHiepCocPoint
        {
            get
            {
                string txt = "195,140-175,140-170,125-190,120-190,110-194,54-165,60-145,65-135,75-130,50-115,45-95,45-70,45-60,55-65,40-50,43-60,65-52,188";
                int cnt = 0;
                foreach (string s in txt.Split('-'))
                {
                    int x = 0;
                    int y = 0;
                    try
                    {
                        x = TDT.ParseInt(s.Split(',')[0]);
                        y = TDT.ParseInt(s.Split(',')[1]);
                    }
                    catch { }
                    if (x != 0 && y != 0)
                    {
                        cnt++;
                    }
                }
                if (cnt > 1)
                {
                    int[,] Point = new int[cnt, 2];
                    int c = 0;
                    foreach (string s in txt.Split('-'))
                    {
                        int x = 0;
                        int y = 0;
                        try
                        {
                            x = TDT.ParseInt(s.Split(',')[0]);
                            y = TDT.ParseInt(s.Split(',')[1]);
                        }
                        catch { }
                        if (x != 0 && y != 0)
                        {
                            Point[c, 0] = x;
                            Point[c, 1] = y;
                            c++;
                        }
                    }
                    return Point;
                }
                return null;
            }
        }

        private string randomTXT = null;

        public HashSet<string> hashBuff;

        public HashSet<string> HashBuff
        {
            get
            {
                if (hashBuff == null)
                {
                    hashBuff = new HashSet<string>(Game.IniParser.Read("Buff", TLBB.AllowName).Split('\n').ToList().Select(s => s.Trim()).Where(s => s.Length > 1));
                }
                return hashBuff;
            }
            set
            {
                hashBuff = value;
            }
        }

        public string idBuff;

        public string IdBuff
        {
            get
            {
                if (idBuff == null)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (string name in HashBuff)
                    {
                        sb.AppendLine(name);
                    }
                    idBuff = sb.ToString();
                }
                return idBuff;
            }
            set
            {
                HashBuff = new HashSet<string>();
                string buff = value;
                StringBuilder sb = new StringBuilder();
                foreach (string b in buff.Split('\n'))
                {
                    string name = b.Trim();
                    if (!name.Contains("-") && !name.Contains(" ") && !string.IsNullOrEmpty(name))
                    {
                        if (!HashBuff.Contains(name))
                        {
                            HashBuff.Add(name);
                            sb.AppendLine(name);
                        }
                    }
                }
                idBuff = sb.ToString();
                Game.IniParser.Write("Buff", TLBB.AllowName, idBuff);
            }
        }



        private string buyits;

        public string BuyIts
        {
            get
            {
                if (buyits == null)
                {
                    buyits = Game.IniParser.Read("Buy", TLBB.AllowName);
                }
                return buyits;
            }
            set
            {
                buyits = value;
                Game.IniParser.Write("Buy", TLBB.AllowName, buyits);
            }
        }

        public string MapPoint
        {
            get
            {
                string txt = SettingOld.LoadMAP(TLBB.MapId.ToString());
                return txt;
            }
        }

        public int RadiusExMap { get; set; } = -1;

        public bool IsSetRadiusEx { get; set; } = false;

        public int MoveNextExIdx { get; set; } = 0;
        public Stopwatch ClearMonterTimeEx { get; set; } = Stopwatch.StartNew();

        public void MoveNextBossMap()
        {
            if (TLBB.PlayerState != 0)
                return;

            string txt = SettingOld.LoadBossMAP(TLBB.MapId.ToString());
            int cnt = 0;
            foreach (string s in txt.Split('-'))
            {
                int x = 0;
                int y = 0;
                try
                {
                    x = TDT.ParseInt(s.Split(',')[0]);
                    y = TDT.ParseInt(s.Split(',')[1]);
                }
                catch { }
                if (x != 0 && y != 0)
                {
                    cnt++;
                }
            }
            if (cnt > 1)
            {
                int[,] Point = new int[cnt, 2];
                int c = 0;
                foreach (string s in txt.Split('-'))
                {
                    int x = 0;
                    int y = 0;
                    try
                    {
                        x = TDT.ParseInt(s.Split(',')[0]);
                        y = TDT.ParseInt(s.Split(',')[1]);
                    }
                    catch { }
                    if (x != 0 && y != 0)
                    {
                        Point[c, 0] = x;
                        Point[c, 1] = y;
                        c++;
                    }
                }
                MoveNext(Point);
                return;
            }
        }

        public void MoveNextEx()
        {
            if (TLBB.PlayerState != 0)
                return;
            string txt = SettingOld.LoadMAP(TLBB.MapId.ToString());

            List<string> listPoint = txt.Split('-').ToList().Where(s => s.Split(',').Length >= 2).OrderBy(_ => Guid.NewGuid()).ToList();

            GoToEx(listPoint[0]);
        }

        public void MoveNext()
        {
            if (TLBB.PlayerState != 0)
                return;
            if (IsRunAutoMap && TrueStandTime.Elapsed.TotalSeconds <= 2)
                return;
            string txt = SettingOld.LoadMAP(TLBB.MapId.ToString());
            if (Missions.Contains(MissionsType.NhiemVuThangCap) && TLBB.MapId > 551)
            {
                txt = SettingOld.LoadMAP(MapAcbaMen.ToString());
            }
            else
            {
            }
            if (string.IsNullOrEmpty(txt.Trim()))
            {
                if (Missions.Contains(MissionsType.NhiemVuThangCap) && TLBB.MapId > 551)
                {
                    TLBB.MapId = (uint)MapAcbaMen;
                }
            }

            if (Missions.Contains(MissionsType.BachHoaDuyen))
            {
                if (randomTXT == null)
                {
                    randomTXT = string.Empty;
                    List<string> list = new List<string>();
                    foreach (string s in txt.Split('-'))
                    {
                        list.Add(s);
                    }
                    while (list.Count > 0)
                    {
                        int index = new Random().Next(0, list.Count);
                        randomTXT += list[index] + "-";
                        list.RemoveAt(index);
                    }
                    randomTXT = randomTXT.Trim('-');
                }
            }
            if (randomTXT != null)
                txt = randomTXT;
            int cnt = 0;
            foreach (string s in txt.Split('-'))
            {
                int x = 0;
                int y = 0;
                try
                {
                    x = TDT.ParseInt(s.Split(',')[0]);
                    y = TDT.ParseInt(s.Split(',')[1]);
                }
                catch { }
                if (x != 0 && y != 0)
                {
                    cnt++;
                }
            }
            if (cnt > 1)
            {
                int[,] Point = new int[cnt, 2];
                int c = 0;
                foreach (string s in txt.Split('-'))
                {
                    int x = 0;
                    int y = 0;
                    try
                    {
                        x = TDT.ParseInt(s.Split(',')[0]);
                        y = TDT.ParseInt(s.Split(',')[1]);
                    }
                    catch { }
                    if (x != 0 && y != 0)
                    {
                        Point[c, 0] = x;
                        Point[c, 1] = y;
                        c++;
                    }
                }
                MoveNext(Point);
                return;
            }
        }

        public bool IsXongBossMap { get; set; }

        public void MoveNext(int[,] point)
        {
            if (Missions.Contains(MissionsType.DatDoiBossMap) && Global.IsHetBossVeLacDuong)
            {
                if (MoveIndex > point.GetLength(0) - 1)
                {
                    IsXongBossMap = true;
                }
            }
            if (TLBB.MapId == TLBB.MapMonPhai || TLBB.MapId == MapAcTac || TLBB.MapId == MAP.TangKinhCac || TLBB.MapId == MAP.ThanhThuSon || TLBB.MapId == MAP.LauLan || TLBB.MapId == MAP.PhungHoangCoThanh)
            {
                if (TDT.GetDistance(CharX, CharY, point[MoveAcBaIndex, 0], point[MoveAcBaIndex, 1]) <= 5)
                    MoveAcBaIndex++;
                if (MoveAcBaIndex == -1 || MoveAcBaIndex > point.GetLength(0) - 1)
                    MoveAcBaIndex = 0;
                Move(point[MoveAcBaIndex, 0], point[MoveAcBaIndex, 1]);
            }
            else
            {
                if (MoveIndex == -1)
                {
                    MoveIndex = 0;
                    if (TLBB.MapId == MAP.PhungHoangCoThanh || IsMapPhuBan() || TLBB.MapId == MAP.ThuyLao)
                    {
                        MoveIndex = 0;
                    }
                    else
                    {
                        float minDistance = 9999;
                        for (int i = 0; i < point.GetLength(0); i++)
                        {
                            float distance = TDT.GetDistance(CharX, CharY, point[i, 0], point[i, 0]);
                            if (distance < minDistance)
                            {
                                minDistance = distance;
                                MoveIndex = i;
                            }
                        }
                    }
                }
                if (MoveIndex > point.GetLength(0) - 1)
                {
                    if (Missions.Contains(MissionsType.DatDoiBossMap) && Global.IsHetBossVeLacDuong)
                    {
                        IsXongBossMap = true;
                    }
                    IsXong50NguyTongBinh = true;
                    if (IsThuyLao)
                    {
                        IsXongThuyLao = true;
                    }
                    if (TLBB.MapId != MAP.BienGioiTongLieu && TLBB.MapId != MAP.TrucLam)
                        MoveIndex = 0;
                }
                if (TDT.GetDistance(CharX, CharY, point[MoveIndex, 0], point[MoveIndex, 1]) <= 5)
                    MoveIndex++;
                if (MoveIndex > point.GetLength(0) - 1)
                {
                    if (Missions.Contains(MissionsType.DatDoiBossMap) && Global.IsHetBossVeLacDuong)
                    {
                        if (MoveIndex > point.GetLength(0) - 1)
                        {
                            IsXongBossMap = true;
                        }
                    }
                    IsXong50NguyTongBinh = true;
                    if (IsThuyLao)
                    {
                        IsXongThuyLao = true;
                    }
                    if (TLBB.MapId != MAP.BienGioiTongLieu && TLBB.MapId != MAP.TrucLam)
                        MoveIndex = 0;
                }
                Move(point[MoveIndex, 0], point[MoveIndex, 1]);
            }
        }

        public bool IsXong50NguyTongBinh = false;

        private bool IsXongThuyLao { get; set; }

    


        public void Accept(bool isAll = false)
        {
            if (Main.SettingForm.checkAcceptAll.Checked || isAll)
            {
                DoStringEx(@"for i = 0, DataPool:GetApplyMemberCount() do 
                              Player:SendAgreeJoinTeam_Apply(i); 
                            end
                            if DataPool:GetInviteTeamCount() > 0 then
	                            szNick,iFamily,iLevel,iCapID,iHead,iArmourID,iCuffID,iFootID,iWeaponID,_,sZoneWorldID = DataPool:GetInviteTeamMemberInfo(0,0);	  
	                            Player:AgreeJoinTeam(0) 
                            end
                            DataPool:ClearAllApply();
                            if DataPool:GetInviteTeamCount() > 0 or DataPool:GetApplyMemberCount() then 
                                Player:RejectJoinTeam(0);
                            end;"
                            );
            }
            else
            {
                if (Main.SettingForm.checkAccept.Checked)
                    DoStringEx("TEAM = '" + (SettingOld.IdBuff + IdBuff).MergeLine() + "';\r\n" + @"
                                for i = 0, DataPool:GetApplyMemberCount() do 
                                    szNick,iFamily,iLevel,iCapID,iHead,iArmourID,iCuffID,iFootID,iWeaponID,_,sZoneWorldID = DataPool:GetApplyMemberInfo(i); 
	                                if string.find(TEAM, szNick) then 
		                                Player:SendAgreeJoinTeam_Apply(i); 				
	                                end 
                                end 
	                       
                                if DataPool:GetInviteTeamCount() > 0 then 
	                                szNick,iFamily,iLevel,iCapID,iHead,iArmourID,iCuffID,iFootID,iWeaponID,_,sZoneWorldID = DataPool:GetInviteTeamMemberInfo(0,0);
	                                if string.find(TEAM, szNick) then
		                                Player:AgreeJoinTeam(0) 
	                                end
                                end 	
                                DataPool:ClearAllApply();
                                if DataPool:GetInviteTeamCount() > 0 or DataPool:GetApplyMemberCount() then 
                                    Player:RejectJoinTeam(0);
                                end "
                                );
            }
        }

        public void AppointLeader(string name)
        {
            DoStringEx("AppointLeader(\"" + name + "\")");
        }

        public bool IsAcceptAll = false;

      

        public void FixKetMap(int radius = 4)
        {        
            LUA.Move(RoundX + new Random().Next(0 - radius, 1 + radius), RoundY + new Random().Next(0 - radius, 1 + radius));
        }



        public void TuBaoBon()
        {
            if (TLBB.PlayerState != 0)
                return;
            StopFollow();
            if (!TLBB.IsMapBang(TLBB.MapId))
            {
                VaoBang();
                return;
            }
            //if (TrangThaiTuBaoBon != "Xong")
            //{
            //    if (!IsMove)
            //    {
            //        FixKetMap();
            //        return;
            //    }
            //}
            if (TLBB.IsBienThan)
            {
                PostMessage(5, 105);
                LuaToString();
                Thread.Sleep(500);
                string info = LuaString();
                if (info == "DaDao")
                {
                    if (TDT.GetDistance(CharX, CharY, 100, 81) > 1)
                    {
                        Move(100, 81);
                    }
                    else if (TDT.GetDistance(CharX, CharY, 100, 81) < 1)
                    {
                        SendKey(Keys.F2);
                        MissionState = "";
                        Thread.Sleep(2000);
                    }
                }
                else
                {
                    if (TDT.GetDistance(CharX, CharY, 100, 145) > 1)
                    {
                        Move(100, 145);
                    }
                    else if (TDT.GetDistance(CharX, CharY, 100, 145) < 1)
                    {
                        SendKey(Keys.F1);
                        MissionState = "";
                    }
                }
                return;
            }
            else
            {
                if (TDT.GetDistance(CharX, CharY, 100, 81) > 1)
                {
                    Move(100, 81);
                    return;
                }
                if (TLBB.IsRide)
                {
                    DownRide();
                    return;
                }
                if (MissionState == "TalkToNhanNV")
                {
                    foreach (QuestFrame dialog in QuestFrame.Enum(this))
                    {
                        //#{BHCJ_140523_32} ta muon bien than tho mo
                        //#{BHCJ_140523_33} tiep tuc bien than tho mo
                        //#{BHCJ_140523_34} nhan phi thuy
                        if (dialog.Name.Contains("{BHCJ_140523_47}"))
                        {
                            QuestFrameOptionClicked(dialog);
                            MissionState = "";
                            return;
                        }
                        if (dialog.Name.Contains("{BHCJ_140523_32}"))
                        {
                            QuestFrameOptionClicked(dialog);
                            return;
                        }
                        if (dialog.Name.Contains("{BHCJ_140523_33}"))
                        {
                            QuestFrameOptionClicked(dialog);
                            MissionState = "";
                            return;
                        }
                        if (dialog.Name.Contains("{BHCJ_140523_34}"))
                        {
                            QuestFrameOptionClicked(dialog);
                            return;
                        }
                        if (dialog.Name.Contains("{BHCJ_140523_180}"))
                        {
                            QuestFrameOptionClicked(dialog);
                            MissionState = "Xong";
                            return;
                        }
                    }
                }
                else if (MissionState != "Xong")
                {
                    foreach (GameObject _object in Objects.All)
                    {
                        if (TDT.VietLien(_object.Name).Contains("tubaobon"))
                        {
                            Talk(_object.Id);
                            MissionState = "TalkToNhanNV";
                            return;
                        }
                    }
                }
            }
        }

        private bool IsDauCo = false;
        public string DoSuMon = "";

        private Stopwatch swHuyQ;

        public void SuMon()
        {
            if (!IsOneSec)
                return;
            if (IsOneTNT)
            {
                if (SecCount % 10 == 0)
                {
                    UncheckTueHong();
                }
            }
            StopFollow();
            if (IdleTime > 0 && IdleTime % 10 == 0)
            {
                MissionState = "";
            }
            if (TLBB.Busy)
            {
                if (TLBB.PlayerState != 7)
                {
                    MissionState = "";
                    return;
                }
            }
            if (MissionState == "TimDo")
            {
                if (!OSuMon())
                {
                    return;
                }
                GameObject objHoa = Objects.All.Where(o => !BlackList.Contains(o.Id) && TDT.VietLien(MissionInfo) == TDT.VietLien(o.Name)).OrderBy(o => o.Distance).FirstOrDefault();

                if (objHoa != null)
                {
                    if (objHoa.Id != PickedId)
                    {
                        PickedId = objHoa.Id;
                        CareTime = Stopwatch.StartNew();
                    }
                    else
                    {
                        if (CareTime.Elapsed.TotalSeconds > 15)
                            BlackList.Add(PickedId);
                    }
                    if (GoToEx(objHoa.X, objHoa.Y, -1, true))
                    {
                        if (TLBB.IsRide)
                            DownRide();
                        PickItem((int)objHoa.Id);
                    }
                    return;
                }
                if (!TLBB.IsRide && TLBB.HaveRide)
                {
                    UpRide();
                    return;
                }
                MoveNextEx();
                return;
            }

            if (TLBB.PlayerState != 0)
                return;
            if (MissionState == "")
            {
                if (!IsOneTNT)
                {
                    if (sovongsumon >= Global.MaxSuMon)
                        RemoveMission(MissionsType.NhiemVuSuMon);
                }
                if (!TLBB.IsTogleMission)
                {
                    PostMessage(18, 105);
                    Thread.Sleep(1000);
                }
                PostMessage(18, 105);
                Thread.Sleep(1000);
                DoStringEx("local cnt = 0; while true do local name, content = DataPool:GetPlayerMission_Memo(cnt); if string.find(name, 'm v') and string.find(name, 'Nhi') == 1  and string.find(name, 'S') == 10 and string.len(name) == 15 then if DataPool:GetPlayerMission_Variable(cnt, 0) == 1 then if string.find(content, 'Huy«n Vû Ðäo') then return 'XongPet' .. content; end return 'Xong' .. content; else return '" + TDT.RandomEmptyString() + "' .. content; end end cnt = cnt + 1; if cnt == 20 then return 'Chua'; end end");
                LuaToString();
                Thread.Sleep(1000);
                MissionInfo = LuaString();
                if (MissionInfo == "Chua")
                {
                    if (DenSuMon())
                    {
                        foreach (GameObject _object in Objects.All)
                        {
                            if (IsNPCSuMon(_object))
                            {
                                Talk(_object.Id);
                                MissionState = "TalkToNhanNV";
                                return;
                            }
                        }
                    }
                }
                if (MissionInfo.StartsWith("Xong"))
                {
                    MissionState = "Xong";
                    return;
                }
                MissionInfo = TDT.GetInfo(MissionInfo.Trim()).Trim();
                //if (SuMonInfo.Contains("tìm được điểm nhắc nhở màu vàng") || SuMonInfo.Contains("tìm điểm chỉ thị màu vàng"))'
                //Main.PushLogEx(SuMonInfo);
                // tìm và thu thập
                if (TDT.VietLien(MissionInfo).Contains("timvathuthap") || TDT.VietLien(MissionInfo).Contains("diemnhacnhomauvang") || TDT.VietLien(MissionInfo).Contains("timdiemchidanmauvang") || TDT.VietLien(MissionInfo).Contains("timduocdiemnhacnhomauvang") || TDT.VietLien(MissionInfo).Contains("timdiemchithimauvang"))
                {
                    MissionState = "TimDo";
                    DoSuMon = MissionInfo.Substring(MissionInfo.IndexOf("#G") + 2);
                    DoSuMon = DoSuMon.Replace("Châu Báu Bị Mất", "Tiểu bạch hầu");
                    DoSuMon = DoSuMon.Replace("Trang Sức Bị Mất", "Tiểu hoàng hầu");
                    DoSuMon = DoSuMon.Replace("Tế Phẩm Bị Mất", "Tiểu hắc hầu");

                    DoSuMon = DoSuMon.Replace("Huyễn Hải Trầm Hương Mộc", "Ảo Hải Trầm Hương");
                    DoSuMon = DoSuMon.Replace("Quả Vong Ưu", "Hoa Vong Ưu");
                    DoSuMon = DoSuMon.Replace("Ly Hồn", "Dẫn Phách Chung");

                    DoSuMon = Regex.Replace(DoSuMon, "#.*", "").ToLower();
                    DoSuMon = DoSuMon.Replace("bị mất", "");
                    DoSuMon = DoSuMon.Replace("châu báu", "châu bảo").Trim();
                    //"    Nhờ các hạ đến Đào Hoa Đảo tìm và thu thập 5 #GHuyễn Hải Trầm Hương Mộc#W giao cho ta!#r  #GNhắc nhở: #G#r  Các hạ có thể tìm chấm vàng trên bản đồ nhỏ góc phải trên màn hình.#G#r    Sau khi hoàn thành nhiệm vụ, hãy tìm Người Công Bố Nhiệm Vụ Sư Môn #GTiêu Thanh Yết #G#{_INFOAIM160,72,731,Tiêu Thanh Yết} trả nhiệm vụ.#r#{SMRW_090206_01}"

                    try
                    {
                        MissionInfo = MissionInfo.Substring(0, MissionInfo.ToLower().IndexOf("#y" + DoSuMon) - ("#y" + DoSuMon).Length);
                        MissionInfo = Regex.Replace(MissionInfo, ".*#R", "");
                        MissionInfo = Regex.Replace(MissionInfo, "#.*", "");
                    }
                    catch
                    {
                        MissionInfo = DoSuMon;
                    }
                    return;
                }
                if (TDT.VietLien(MissionInfo).Contains("cothedungkynang"))
                {
                    MissionState = "NhiemVuKho";
                    return;
                }
                MissionX = MissionY = MissionMap = 0;
                try
                {
                    string info = MissionInfo.Substring(MissionInfo.IndexOf("INFOAIM"));
                    MissionX = TDT.ParseInt(info.Split(',')[0]);
                    MissionY = TDT.ParseInt(info.Split(',')[1]);
                    MissionMap = TDT.ParseInt(info.Split(',')[2]);
                }
                catch { }
                if (MissionX > 0 && MissionY > 0)
                    MissionState = "DaNhanSuMon";
                else
                    MissionState = "";
                if (MissionInfo.Contains("#{GGSM_160624_10}"))
                {
                    MissionX = 96;
                    MissionY = 52;
                }

                if (MissionInfo.Contains("#{THSM_190727_1}"))
                {
                    MissionX = 160;
                    MissionY = 51;
                }
                if (MissionMap == MAP.LacDuong)
                {
                    if (TDT.Contain(MissionInfo, "tongcaban"))
                    {
                        MissionX = 208;
                        MissionY = 193;
                    }
                }
                if (TDT.VietLien(MissionInfo).Contains("cothemuatai"))
                {
                    if (TLBB.Gold < 10000)
                    {
                        MissionState = "HuyQ";
                        DoStringEx("local cnt = 0; while true do local name, content = DataPool:GetPlayerMission_Memo(cnt); if string.find(name, 'm v') and string.find(name, 'Nhi') and string.find(name, 'S') == 10 and string.len(name) == 15 then if DataPool:GetPlayerMission_Variable(cnt, 0) ~= 1 then DataPool:Mission_Abnegate_Popup(cnt,DataPool:GetPlayerMission_Memo(cnt)); end end cnt = cnt + 1; if cnt == 20 then return end end");
                        Thread.Sleep(1000);
                        return;
                    }
                }
                if (Setting.Is("chkHuyBatPet"))
                {
                    if (MissionMap == MAP.HuyenVuDao)
                    {
                        MissionState = "HuyQ";
                        DoStringEx("local cnt = 0; while true do local name, content = DataPool:GetPlayerMission_Memo(cnt); if string.find(name, 'm v') and string.find(name, 'Nhi') and string.find(name, 'S') == 10 and string.len(name) == 15 then if DataPool:GetPlayerMission_Variable(cnt, 0) ~= 1 then DataPool:Mission_Abnegate_Popup(cnt,DataPool:GetPlayerMission_Memo(cnt)); end end cnt = cnt + 1; if cnt == 20 then return end end");
                        Thread.Sleep(1000);
                        return;
                    }
                }

                return;
            }
            if (MissionState == "NhiemVuKho")
            {
                if (DenSuMon())
                {
                    foreach (GameObject _object in Objects.All)
                    {
                        if (IsNPCSuMon(_object))
                        {
                            Talk(_object.Id);
                            MissionState = "";
                            Thread.Sleep(1000);
                            if (!IsDauCo)
                            {
                                foreach (QuestFrame dialog in QuestFrame.Enum(this))
                                {
                                    if (dialog.Name.Contains("#{TJQQ_90428_1}"))
                                    {
                                        QuestFrameOptionClicked(dialog);
                                        Thread.Sleep(1000);
                                        break;
                                    }
                                }
                                foreach (QuestFrame dialog in QuestFrame.Enum(this))
                                {
                                    if (dialog.Name.Contains("#{INTERFACE_XML_557}"))
                                    {
                                        QuestFrameOptionClicked(dialog);
                                        IsDauCo = true;
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                if (TLBB.MenpaiPoint < 150 || Setting.Is("checkKhongDungDongMon"))
                                {
                                    DoStringEx("local cnt = 0; while true do local name, content = DataPool:GetPlayerMission_Memo(cnt); if string.find(name, 'Nhiệm vụ Sư Môn') then if DataPool:GetPlayerMission_Variable(cnt, 0) ~= 1 then DataPool:Mission_Abnegate_Popup(cnt,DataPool:GetPlayerMission_Memo(cnt)); end end cnt = cnt + 1; if cnt == 20 then return end end");
                                    MissionState = "HuyQ";
                                    Thread.Sleep(1000);
                                    return;
                                }
                                foreach (QuestFrame dialog in QuestFrame.Enum(this))
                                {
                                    if (TDT.VietLien(dialog.Name).Contains("dongmontuongtro")) // Đồng môn tương trợ"))
                                    {
                                        QuestFrameOptionClicked(dialog);
                                        Thread.Sleep(1000);
                                        break;
                                    }
                                }
                                foreach (QuestFrame dialog in QuestFrame.Enum(this))
                                {
                                    if (TDT.VietLien(dialog.Name).Contains("dongy"))//Đồng ý"))
                                    {
                                        QuestFrameOptionClicked(dialog);
                                        IsDauCo = true;
                                        return;
                                    }
                                }
                            }
                            return;
                        }
                    }
                }
                return;
            }
            if (MissionState == "HuyQ")
            {
                LUA.MessageBox_Self_OK_Clicked();
                Thread.Sleep(100);
                //MessageboxSelfOkClicked();
                MissionState = "";
                swHuyQ = Stopwatch.StartNew();
                return;
            }
            if (MissionState == "TalkToNhanNV")
            {
                foreach (QuestFrame dialog in QuestFrame.Enum(this))
                {
                    if (TDT.VietLien(dialog.Name).StartsWith("nhiemvusumon"))//nhiệm vụ sư môn"))
                    {
                        QuestFrameOptionClicked(dialog);
                        MissionState = "";
                        return;
                    }
                }
            }
            if (MissionState == "Xong")
            {
                if (DenSuMon())
                {
                    foreach (GameObject _object in Objects.All)
                    {
                        if (IsNPCSuMon(_object))
                        {
                            Talk(_object.Id);
                            Thread.Sleep(1000);
                            sovongsumon++;
                            foreach (QuestFrame dialog in QuestFrame.Enum(this))
                            {
                                if (TDT.VietLien(dialog.Name).StartsWith("nhiemvusumon"))// nhiệm vụ sư môn"))
                                {
                                    QuestFrameOptionClicked(dialog);
                                    Thread.Sleep(1000);
                                    if (MissionInfo.StartsWith("XongPet") || TDT.VietLien(MissionInfo).Contains("huyenvudao"))
                                    {
                                        Thread.Sleep(1000);
                                        string petName = Regex.Replace(MissionInfo, ".*#G#R", "");
                                        petName = Regex.Replace(petName, "#W.*", "");
                                        string lua = "PETNAME = \"" + petName + "\";";
                                        DoStringEx(lua);
                                        DoStringEx("local index = -1; for i = 1, 10 do local szPetName,szOn = Pet:GetPetList_Appoint(i-1); if szPetName ~= '' then local name = Pet:GetName(i - 1); if string.find(name, ' Tr') and string.find(name, 'ng Th') then index = i; end if string.find(name, PETNAME) then Exchange:AddPet(i - 1); i = -1; break; end end end if index ~= -1 then Exchange:AddPet(index - 1); end");
                                        Thread.Sleep(1000);
                                        DoStringEx("setmetatable(_G, {__index = MissionReply_Env}); MissionReply_Accept_Clicked();");
                                        MissionState = "";
                                    }
                                    else
                                    {
                                        Thread.Sleep(1000);
                                        PostMessage(14, 105);
                                        Thread.Sleep(1000);
                                        PostMessage(15, 105);
                                        MissionState = "";
                                    }
                                    break;
                                }
                            }
                            break;
                        }
                    }
                }
                return;
            }
            if (MissionState == "DaNhanSuMon")
            {
                if (MissionX == 0)
                {
                    MissionState = "";
                    return;
                }
                if (MissionMap == 0 && MissionX == 183 && MissionY == 231)
                {
                    DoStringEx("local cnt = 0; while true do local name, content = DataPool:GetPlayerMission_Memo(cnt); if string.find(name, 'Nhiệm vụ Sư Môn') then if DataPool:GetPlayerMission_Variable(cnt, 0) ~= 1 then DataPool:Mission_Abnegate_Popup(cnt,DataPool:GetPlayerMission_Memo(cnt)); end end cnt = cnt + 1; if cnt == 20 then return end end");
                    MissionState = "HuyQ";
                    Thread.Sleep(1000);
                    return;
                }
                float distance = 3;
                bool havepet = false;
                if (MissionMap == MAP.HuyenVuDao || MissionMap == MAP.ThanhThuSon)
                {
                    distance = 10;
                    if (MissionInfo.Trim() != string.Empty)
                    {
                        foreach (GameObject _object in Objects.All)
                        {
                            if (TDT.Contain(MissionInfo, _object.Name) && _object.Menpai == 0 && !string.IsNullOrEmpty(_object.Name.Trim()))
                            {
                                havepet = true;
                                break;
                            }
                        }
                    }
                }

                if (havepet || GoToEx(MissionX, MissionY, MissionMap, false, distance))
                {
                    if (TDT.Contain(MissionInfo, "tuoinuoc") || TDT.Contain(MissionInfo, "congcuvevatthuc"))
                    {
                        if (IdleTime > 10)
                            FixKetMap();
                        if (TLBB.IsRide)
                        {
                            DownRide();
                            return;
                        }
                        TuoiNuoc();
                        return;
                    }
                    if (TLBB.MapId == 112 || TLBB.MapId == MAP.ThanhThuSon)
                    {
                        if (TLBB.IsRide)
                        {
                            DownRide();
                            return;
                        }
                        foreach (GameObject _object in Objects.All)
                        {
                            if (TDT.Contain(MissionInfo, _object.Name) && _object.Menpai == 0)
                            {
                                if (TDT.GetDistance(CharX, CharY, _object.X, _object.Y) <= 3)
                                {
                                    UseSkill(1, _object.Id);
                                    Thread.Sleep(3000);
                                    MissionState = "";
                                }
                                else
                                {
                                    Move(_object.X, _object.Y);
                                    Thread.Sleep(3000);
                                }

                                return;
                            }
                        }
                        if (Objects.Have("hodaothanthu"))
                        {
                            if (TLBB.MP > 50)
                                AnDon();
                        }
                        ForceAttack();
                        return;
                    }
                    if (TDT.VietLien(MissionInfo).Contains("cothemuatai") || TDT.VietLien(MissionInfo).Contains("cothemuatoi"))
                    {
                        foreach (GameObject _object in Objects.All)
                        {
                            if (TDT.Contain(MissionInfo, _object.Name))
                            {
                                Talk(_object.Id);
                                MissionInfo = MissionInfo.Replace("#G#Y", "");
                                MissionInfo = Regex.Replace(MissionInfo, "#.*", "");
                                MissionState = "TalkToBuy";
                                return;
                            }
                        }
                    }
                    if (TDT.Contain(MissionInfo, "PhuBan"))
                    {
                        foreach (GameObject _object in Objects.All)
                        {
                            if (TDT.Contain(MissionInfo, _object.Name))
                            {
                                Talk(_object.Id);
                                Thread.Sleep(1000);
                                foreach (QuestFrame dialog in QuestFrame.Enum(this))
                                {
                                    if (dialog.StrOptionExtra1.ToString().Contains("8898"))
                                    {
                                        QuestFrameOptionClicked(dialog);
                                        Thread.Sleep(1000);
                                        LUA.QuestFrameAcceptClicked();
                                    }
                                }
                                MissionState = "";
                                return;
                            }
                        }
                    }
                    if (TDT.Contain(MissionInfo, "dalaukhonggap") || TDT.Contain(MissionInfo, "lauroikhonggap"))
                    {
                        foreach (GameObject _object in Objects.All)
                        {
                            if (TDT.Contain(MissionInfo, _object.Name))
                            {
                                Talk(_object.Id);
                                MissionState = "TalkToNhanNV";
                                return;
                            }
                        }
                    }
                    foreach (GameObject _object in Objects.All)
                    {
                        if ((TLBB.MapId == MAP.DaoHoa && _object.CleanName == "chammongquy") || (TDT.Contain(MissionInfo, _object.Name) || TDT.VietLien(_object.Name) == "huyentrung" || (TLBB.MapId == MAP.QuyCoc && _object.CleanName == "vuonghuchi")) && _object.IsNPC)
                        {
                            Talk(_object.Id);
                            Thread.Sleep(1000);
                            foreach (QuestFrame dialog in QuestFrame.Enum(this))
                            {
                                if (dialog.StrOptionExtra1.ToString().Contains("8898"))
                                {
                                    QuestFrameOptionClicked(dialog);
                                    Thread.Sleep(1000);
                                    LUA.QuestFrameAcceptClicked();
                                }
                            }
                            MissionState = "";
                            return;
                        }
                    }
                }
            }
            if (MissionState == "TalkToBuy")
            {
                if (TLBB.IsShopOpen)
                {
                    foreach (var item in PacketItems.Shop)
                    {
                        if (item.Name.Trim() == MissionInfo.Trim())
                        {
                            // PostMessageNew(item.Address, 113);
                            LUA.Buy((int)item.Index, 1);
                            MissionState = "";
                            return;
                        }
                    }
                }
                else
                {
                    foreach (QuestFrame dialog in QuestFrame.Enum(this))
                    {
                        if (TDT.VietLien(dialog.Name).StartsWith("muadodungchotranthu") || TDT.VietLien(dialog.Name).StartsWith("muadataodo") || TDT.VietLien(dialog.Name).StartsWith("muadatoodo") || TDT.VietLien(dialog.Name).StartsWith("buonbantaphoa"))
                        {
                            QuestFrameOptionClicked(dialog);
                            return;
                        }
                    }
                    foreach (QuestFrame dialog in QuestFrame.Enum(this))
                    {
                        if (TDT.VietLien(dialog.Name).StartsWith("mua"))
                        {
                            QuestFrameOptionClicked(dialog);
                            return;
                        }
                    }
                }
            }
        }

        public void NhanLeBao()
        {
            if (!IsOneSec)
                return;
            if (GoToEx(NPC.CungThaiVan))
            {
                if (TLBB.IsQuestOpen)
                {
                    foreach (QuestFrame dialog in QuestFrame.Enum(this))
                    {
                        if (dialog.Name.Contains("#{TSJH_090224_3}"))
                        {
                            QuestFrameOptionClicked(dialog);
                            break;
                        }
                        if (dialog.Name.Contains("#{TSJH_090224_5}"))
                        {
                            QuestFrameOptionClicked(dialog);
                            RemoveMission(MissionsType.NhanLeBao);
                            if (IsByLogin)
                            {
                                IsXongBHD = true;
                            }
                            break;
                        }
                    }
                }
                else
                {
                    Talk(NPC.CungThaiVan.Id);
                }
            }
        }

        public void NhanBoiThuong()
        {
            if (!IsOneSec)
                return;
            if (GoToEx(NPC.CungThaiVan))
            {
                if (TLBB.IsQuestOpen)
                {
                    foreach (QuestFrame dialog in QuestFrame.Enum(this))
                    {
                        if (dialog.Name.Contains("#{VNWM_20150513_01}"))
                        {
                            QuestFrameOptionClicked(dialog);
                            if (IsByLogin)
                                IsXongBHD = true;
                            Missions.Remove(MissionsType.NhanBoiThuong);
                            break;
                        }
                    }
                }
                else
                {
                    Talk(NPC.CungThaiVan.Id);
                }
            }
        }

        public void Buy(int index)
        {
            uint address = Memory.Read(Address.BaseShopItem);
            address = Memory.Read(address + (uint)index * 0x4);
            if (address == 0)
                return;
            PostMessage(address, 113);
        }

        public void Buy(uint index) => Buy((int)index);

        public bool IsNPCSuMon(GameObject _object)
        {
            if (TDT.VietLien(_object.Title).Contains("nguoigiaonhiemvu")) // Người Giao Nhiệm Vụ"))
                return true;
            if (TDT.VietLien(_object.Title).Contains("congbonhiemvu"))//"Công Bố Nhiệm Vụ"))
                return true;
            if (TDT.VietLien(_object.Title).Contains("nhiemvutuyendatsu") || TDT.VietLien(_object.Title).Contains("nhiemvutuyendotsu"))//Nhiệm Vụ Tuyên Đạt Sứ"))
                return true;
            return false;
        }

        public int SetMenPai
        {
            get; set;
        }

        public bool IsSetMenPai
        {
            get;
            set;
        }

        private bool isAlarmVaoPhai
        {
            get;
            set;
        }

        public void VaoPhai()
        {
            if (Setting.Is("checkDatSkillF1") && TLBB.Menpai != 0 && !Skill.IsBase((int)SkillId(0)) && Address.GameType == 1)
            {
                DoStringEx("MainmenuBar_JoinMenpai()");
            }

            if (TLBB.Lvl > 10)
                return;

            if (TLBB.Menpai != 0)
                return;
            else
            {
                if (!isAlarmVaoPhai && !Global.IsTuVaoPhai)
                {
                    //lecaotri
                    isAlarmVaoPhai = true;
                    //alarmVaoPhai = new AlarmVaoPhai(this);
                    //AlarmEx.Instance.Controls.Add(alarmVaoPhai);
                    AlarmEx.Instance.Invoke(new AlarmEx.PushBack(AlarmEx.PushAlarm), this);
                }
            }
            if (!IsSetMenPai)
                return;
            if (TLBB.Lvl != 10)
                return;

            NPC npc = new NPC();
            if (SetMenPai == TDT.Menpai.DuongMon)
                npc = DAILY.DuongDuc;
            if (SetMenPai == TDT.Menpai.MoDung)
                npc = DAILY.MoDungTruyen;
            if (SetMenPai == TDT.Menpai.TinhTuc)
                npc = DAILY.HaiPhongTu;
            if (SetMenPai == TDT.Menpai.TieuDao)
                npc = DAILY.DamDaiTuVu;
            if (SetMenPai == TDT.Menpai.ThieuLam)
                npc = DAILY.TueDich;
            if (SetMenPai == TDT.Menpai.ThienSon)
                npc = DAILY.TrinhThanhSuong;
            if (SetMenPai == TDT.Menpai.ThienLong)
                npc = DAILY.PhaTham;
            if (SetMenPai == TDT.Menpai.NgaMy)
                npc = DAILY.LoTamNuong;
            if (SetMenPai == TDT.Menpai.VoDang)
                npc = DAILY.TruongHoach;
            if (SetMenPai == TDT.Menpai.MinhGiao)
                npc = DAILY.ThachBao;
            if (SetMenPai == TDT.Menpai.CaiBang)
                npc = DAILY.GianNinh;
            if (SetMenPai == TDT.Menpai.QuyCoc)
                npc = DAILY.DuongTiem;
            if (SetMenPai == TDT.Menpai.DaoHoa)
                npc = DAILY.HoangThoiVu;
            if (npc.Map == 0)
                return;

            if (GoToEx(npc))
            {
                Talk(npc);
                Thread.Sleep(1000);
                foreach (QuestFrame dialog in QuestFrame.Enum(this))
                {
                    if (dialog.Name == "#GVào môn phái" || dialog.Name == "Đồng ý vào Quỷ Cốc")
                    {
                        QuestFrameOptionClicked(dialog);
                        Thread.Sleep(1000);
                        break;
                    }
                }
                QuestFrameOptionClicked(QuestFrame.Enum(this)[1]);
                Thread.Sleep(1000);
                foreach (QuestFrame dialog in QuestFrame.Enum(this))
                {
                    if (dialog.Name == "#GVào môn phái" || dialog.Name == "Đồng ý vào Quỷ Cốc")
                    {
                        QuestFrameOptionClicked(dialog);
                        Thread.Sleep(1000);
                        break;
                    }
                }
                QuestFrameOptionClicked(QuestFrame.Enum(this)[1]);
                MissionState = "";
            }
        }

        public bool DenSuMon()
        {
            //if (!IsRide && TLBB.HaveRide)
            //{
            //    UpRide();
            //    return false;
            //}
            if (TLBB.Menpai == 1)
            {
                if (TLBB.MapId == 9)
                {
                    if (TDT.GetDistance(CharX, CharY, 96, 82) > 3)
                    {
                        GoToEx(96, 82);
                        return false;
                    }
                }
                else
                {
                    GoToEx(96, 82, 9);
                    return false;
                }
            }
            if (TLBB.Menpai == 2)
            {
                if (TLBB.MapId == 11)
                {
                    if (TDT.GetDistance(CharX, CharY, 98, 105) > 3)
                    {
                        GoToEx(98, 105);
                        return false;
                    }
                }
                else
                {
                    GoToEx(98, 105, 11);
                    return false;
                }
            }
            if (TLBB.Menpai == 3)
            {
                if (TLBB.MapId == 10)
                {
                    if (TDT.GetDistance(CharX, CharY, 92, 77) > 3)
                    {
                        GoToEx(92, 77);
                        return false;
                    }
                }
                else
                {
                    GoToEx(92, 77, 10);
                    return false;
                }
            }
            if (TLBB.Menpai == 4)
            {
                if (TLBB.MapId == 12)
                {
                    if (TDT.GetDistance(CharX, CharY, 78, 95) > 3)
                    {
                        GoToEx(78, 95);
                        return false;
                    }
                }
                else
                {
                    GoToEx(78, 95, 12);
                    return false;
                }
            }
            if (TLBB.Menpai == 5)
            {
                if (TLBB.MapId == 15)
                {
                    if (TDT.GetDistance(CharX, CharY, 95, 86) > 3)
                    {
                        GoToEx(95, 86);
                        return false;
                    }
                }
                else
                {
                    GoToEx(95, 86, 15);
                    return false;
                }
            }
            if (TLBB.Menpai == 6)
            {
                if (TLBB.MapId == 16)
                {
                    if (TDT.GetDistance(CharX, CharY, 96, 92) > 3)
                    {
                        GoToEx(96, 92);
                        return false;
                    }
                }
                else
                {
                    GoToEx(96, 92, 16);
                    return false;
                }
            }
            if (TLBB.Menpai == 8)
            {
                if (TLBB.MapId == 17)
                {
                    if (TDT.GetDistance(CharX, CharY, 95, 60) > 3)
                    {
                        GoToEx(95, 60);
                        return false;
                    }
                }
                else
                {
                    GoToEx(95, 60, 17);
                    return false;
                }
            }
            if (TLBB.Menpai == 9)
            {
                if (TLBB.MapId == 14)
                {
                    if (TDT.GetDistance(CharX, CharY, 119, 152) > 3)
                    {
                        GoToEx(119, 152);
                        return false;
                    }
                }
                else
                {
                    GoToEx(119, 152, 14);
                    return false;
                }
            }
            if (TLBB.Menpai == 32)
            {
                if (TLBB.MapId == 284)
                {
                    if (TDT.GetDistance(CharX, CharY, 69, 125) > 3)
                    {
                        GoToEx(69, 125);
                        return false;
                    }
                }
                else
                {
                    GoToEx(69, 125, 284);
                    return false;
                }
            }
            if (TLBB.Menpai == 37) // DuongMon
            {
                if (TLBB.MapId == 615)
                {
                    if (TDT.GetDistance(CharX, CharY, 100, 64) > 3)
                    {
                        GoToEx(100, 64);
                        return false;
                    }
                }
                else
                {
                    GoToEx(100, 64, 615);
                    return false;
                }
            }
            if (TLBB.Menpai == MENPAI.ThienLong)
            {
                if (TLBB.MapId == MAP.ThienLong)
                {
                    if (TDT.GetDistance(CharX, CharY, 95, 88) > 3)
                    {
                        GoToEx(95, 88);
                        return false;
                    }
                }
                else
                {
                    GoToEx(95, 88, MAP.ThienLong);
                    return false;
                }
            }
            if (TLBB.Menpai == MENPAI.QuyCoc)
            {
                if (TLBB.MapId == MAP.QuyCoc)
                {
                    if (TDT.GetDistance(CharX, CharY, 96, 98) > 3)
                    {
                        GoToEx(96, 98);
                        return false;
                    }
                }
                else
                {
                    GoToEx(96, 98, MAP.QuyCoc);
                    return false;
                }
            }

            if (TLBB.Menpai == MENPAI.DaoHoa)
            {
                if (TLBB.MapId == MAP.DaoHoa)
                {
                    if (TDT.GetDistance(CharX, CharY, 160, 72) > 3)
                    {
                        GoToEx(160, 72);
                        return false;
                    }
                }
                else if (TLBB.MapId == MAP.DaoHoaNV)
                {
                    if (TDT.GetDistance(CharX, CharY, 60, 87) > 3)
                    {
                        GoToEx(60, 87);
                        return false;
                    }
                }
                else
                {
                    GoToEx(160, 72, MAP.DaoHoa);
                    return false;
                }
            }
            return true;
        }



        public bool IsPickEx
        {
            get
            {
                if (Missions.Contains(MissionsType.TrangSucCuuLe))
                    return true;
                if (Leader != null && Leader.Missions.Contains(MissionsType.DatDoiMaTac))
                    return true;
                if (Leader != null && Leader.Missions.Contains(MissionsType.DatDoiBaoDoHiem))
                    return true;
                if (Leader != null && Leader.Missions.Contains(MissionsType.DatDoiBossMap))
                    return true;
                if (Missions.Contains(MissionsType.LuyenKim) || Missions.Contains(MissionsType.LuyenKimNhanh))
                    return true;
                if (IsMapPhuBan())
                    return true;
                if (TLBB.MapId == MAP.LanHoanPhucDia)
                    return true;
                if (TLBB.MapId == MAP.LoiDaiSinhTu)
                    return true;
                if (TLBB.MapId == MAP.TangKinhCac)
                    return true;
                if (TLBB.MapId == MAP.BinhThanhKyTran)
                    return true;
                if (TLBB.MapId == MAP.TranLongKyCuoc)
                    return true;
                if (TLBB.MapId == MAP.TuTuyetTrang)
                    return true;
                if (TLBB.MapId == MAP.ThieuThatSon)
                    return true;
                if (TLBB.MapId == MAP.PhungMinhVuongLang)
                    return true;
                if (TLBB.MapId == MAP.PhieuMieuPhong)
                    return true;
                if (TLBB.MapId == MAP.YenTuO)
                    return true;
                if (TLBB.MapId == MAP.TangKinhCac)
                    return true;
                if (TLBB.MapId == MAP.NgaMyAcBa)
                    return true;
                if (TLBB.MapId == MAP.DuongMonAcBa)
                    return true;
                if (TLBB.MapId == MAP.ThienLongAcBa)
                    return true;
                if (TLBB.MapId == MAP.VoDangAcBa)
                    return true;
                if (TLBB.MapId == MAP.TinhTucAcBa)
                    return true;
                if (TLBB.MapId == MAP.TieuDaoAcBa)
                    return true;
                if (TLBB.MapId == MAP.ThieuLamAcBa)
                    return true;
                if (TLBB.MapId == MAP.ThienSonAcBa)
                    return true;
                if (TLBB.MapId == MAP.MoDungAcBa)
                    return true;
                if (TLBB.MapId == MAP.CaiBangAcBa)
                    return true;
                if (TLBB.MapId == MAP.MinhGiaoAcBa)
                    return true;
                if (TLBB.MapId == MAP.TranLongKyCuoc)
                    return true;
                if (TLBB.MapId == MAP.TacKhauDoanhDia)
                    return true;
                if (TLBB.MapId == MAP.TamTaiHiepCoc)
                    return true;
                if (TLBB.MapId == MAP.ViemMaSon)
                    return true;
                if (TLBB.MapId == MAP.TrânLongKỳCuộc || TLBB.MapId == MAP.TặcKhấuDoanhĐịa)
                    return true;
                if (TLBB.MapId >= 552 && TLBB.MapId <= 561 || TLBB.MapId == 683)
                    return true;
                return false;
            }
        }

        public bool IsFilter
        {
            get
            {
                if (ChiNhat.Trim() == string.Empty) return false;
                return IsOnlyPick;
            }
        }

        //lecaotri

        public bool OSuMon()
        {
            if (TLBB.Menpai == MENPAI.ThieuLam)
            {
                if (TLBB.MapId == MAP.ThieuLam)
                    return true;
            }
            if (TLBB.Menpai == MENPAI.MinhGiao)
            {
                if (TLBB.MapId == MAP.MinhGiao)
                    return true;
            }
            if (TLBB.Menpai == MENPAI.CaiBang)
            {
                if (TLBB.MapId == MAP.CaiBang)
                    return true;
            }
            if (TLBB.Menpai == MENPAI.VoDang)
            {
                if (TLBB.MapId == MAP.VoDang)
                    return true;
            }
            if (TLBB.Menpai == MENPAI.NgaMy)
            {
                if (TLBB.MapId == MAP.NgaMy)
                    return true;
            }
            if (TLBB.Menpai == MENPAI.TinhTuc)
            {
                if (TLBB.MapId == MAP.TinhTuc)
                    return true;
            }
            if (TLBB.Menpai == MENPAI.ThienLong)
            {
                if (TLBB.MapId == MAP.ThienLong)
                    return true;
            }
            if (TLBB.Menpai == MENPAI.ThienSon)
            {
                if (TLBB.MapId == MAP.ThienSon)
                    return true;
            }
            if (TLBB.Menpai == MENPAI.TieuDao)
            {
                if (TLBB.MapId == MAP.TieuDao)
                    return true;
            }
            if (TLBB.Menpai == MENPAI.MoDung)
            {
                if (TLBB.MapId == MAP.MoDung)
                    return true;
            }
            if (TLBB.Menpai == MENPAI.DuongMon)
            {
                if (TLBB.MapId == MAP.DuongMon)
                    return true;
            }
            if (TLBB.Menpai == MENPAI.QuyCoc)
            {
                if (TLBB.MapId == MAP.QuyCoc)
                    return true;
            }
            if (TLBB.Menpai == MENPAI.DaoHoa)
            {
                if (TLBB.MapId == MAP.DaoHoa)
                    return true;
            }
            DenSuMon();
            return false;
        }


        public void AOE()
        {
            if (Leader != null)
            {
                if (Leader.Missions.Contains(MissionsType.DatDoiAcTac))
                {
                    if (TLBB.MapId != MAP.TacKhauDoanhDia)
                        return;
                }
            }
            if (SecCount % 10 != 0 || !Global.UseSkillPet || TLBB.MapId == MAP.BienGioiTongLieu || TLBB.MapId == MAP.TrucLam)
                return;
            if (Objects.NearMonter(20).Count < 0)
                return;
            if ((SkillPetId(TLBB.SkillPetType1 + TLBB.SkillPetType2) != -1))
            {
                if (Objects.Monters.Count > 0)
                    UseSkillPet(SkillPetId(TLBB.SkillPetType1 + TLBB.SkillPetType2), Objects.Monters[0].X, Objects.Monters[0].Y);
                return;
            }
        }

        public int SkillPetId(string name)
        {
            if (name.Contains("MenpaiLiveSkill2_14"))
                return 742;
            if (name.Contains("MenpaiLiveSkill2_13"))
                return 743;
            if (name.Contains("MenpaiLiveSkill2_16"))
                return 744;
            if (name.Contains("MenpaiLiveSkill2_15"))
                return 745;
            if (name.Contains("PetSkill4_13"))
                return 676;
            if (name.Contains("PetSkill4_14"))
                return 677;
            if (name.Contains("PetSkill7_7"))
                return 672;
            if (name.Contains("PetSkill1_8"))
                return 673;
            if (name.Contains("PetSkill7_8"))
                return 674;
            if (name.Contains("PetSkill3_4"))
                return 675;
            if (name.Contains("PetSkill4_15"))
                return 747;
            if (name.Contains("PetSkill7_1"))
                return 694;
            if (name.Contains("PetSkill7_2"))
                return 695;
            return -1;
        }

        public void Talk(GameObject _object)
        {
            if (GetDistance(_object.RoundX, _object.RoundY) >= 3)
                Move(_object.RoundX, _object.RoundY);
            else
                Talk(_object.Id);
        }

        public void Talk(NPC npc)
        {
            foreach (GameObject _object in Objects.All)
            {
                if (npc.Name.Trim() == _object.Name.Trim())
                {
                    PostMessage(_object.Id, 110);
                    return;
                }
            }
            if (npc.Id == 0xFFFFFFFF)
            {
                foreach (GameObject _object in Objects.All)
                {
                    if (npc.Name.Trim() == _object.Name.Trim())
                    {
                        PostMessage(_object.Id, 110);
                    }
                    if (_object.RoundX == npc.X && _object.RoundY == npc.Y)
                    {
                        PostMessage(_object.Id, 110);
                    }
                }
            }
            else
            {
                foreach (GameObject _object in Objects.All)
                {
                    if (_object.Id == npc.Id)
                    {
                        PostMessage(npc.Id, 110);
                        return;
                    }
                }
                foreach (GameObject _object in Objects.All)
                {
                    if (npc.Name.Trim() == _object.Name.Trim())
                    {
                        PostMessage(_object.Id, 110);
                    }
                    if (_object.RoundX == npc.X && _object.RoundY == npc.Y)
                    {
                        PostMessage(_object.Id, 110);
                    }
                }
            }
        }

        public void Talk(string name)
        {
            foreach (GameObject _object in Objects.All.Where(o => o.HP > 0))
            {
                if (_object.CleanName.Contains(TDT.VietLien(name)))
                {
                    Talk(_object.Id);
                    return;
                }
            }
        }

        public void TalkEx(string name)
        {
            foreach (GameObject _object in Objects.All)
            {
                if (TDT.VietLien(_object.Name) == TDT.VietLien(name))
                {
                    Talk(_object.Id);
                    return;
                }
            }
        }

        public void Talk(int id)
        {
            PostMessage(id, 110);
        }

        public void Talk(uint id)
        {
            PostMessage(id, 110);
        }

        public void QuestFrameOptionClicked(QuestFrame dialog)
        {
            QuestFrameOptionClicked((int)dialog.StrOptionExtra1, (int)dialog.StrOptionExtra2);
        }

        public void QuestFrameOptionClicked(QuestFrameItem item)
        {
            QuestFrameOptionClicked((int)item.StrOptionExtra1, (int)item.StrOptionExtra2);
        }

        public void QuestFrameOptionClicked(int StrOptionExtra1, int StrOptionExtra2)
        {
            PostMessage(StrOptionExtra1, 54);
            PostMessage(StrOptionExtra2, 55);
            PostMessage(12, 105);
        }



        public void SmartTeam()
        {
            if (TLBB.Lvl >= 90)
                return;
            if (!IsOneSec)
                return;
            if (TLBB.PartyId != 0xFFFFFFFF)
            {
                if (Party.Where(p => p.TLBB.MapId == TLBB.MapId && p.GetDistance(CharX, CharY) < 20).Count() <= 1)
                {
                    if (CreateTeamTime.Elapsed.TotalSeconds > 15)
                    {
                        LUA.DestroyTeam();
                    }
                }
            }
            else
            {
                foreach (var game in Main.Instance.AllOnelineGame)
                {
                    if (game == this)
                        continue;
                    if (game.TLBB.MapId == TLBB.MapId && TDT.GetDistance(game.RoundX, game.RoundY, RoundX, RoundY) < 20)
                    {
                        if (game.TLBB.PartyId != 0xFFFFFFFF && game.Party.Count() < 6)
                        {
                            AskTeam(game.TLBB.VISCIIName);
                            return;
                        }
                    }
                }
                foreach (var game in Main.Instance.AllOnelineGame)
                {
                    if (game == this)
                        continue;
                    if (game.TLBB.MapId == TLBB.MapId && TDT.GetDistance(game.RoundX, game.RoundY, RoundX, RoundY) < 20)
                    {
                        if (game.TLBB.PartyId == 0xFFFFFFFF)
                        {
                            InviteTeam(game.TLBB.VISCIIName);
                            return;
                        }
                    }
                }
            }
        }

        public Stopwatch CreateTeamTime { get; set; } = Stopwatch.StartNew();

        public static Stopwatch NewTeamCreate { get; set; } = Stopwatch.StartNew();

        public void NhanBong()
        {
            if (IsNhanBongSuDo)
            {
                if (TLBB.Lvl < 30)
                {
                    foreach (Game game in Party)
                    {
                        if (game.TLBB.Lvl >= 30)
                        {
                            AppointLeader(game.TLBB.Name);
                            return;
                        }
                    }
                }
            }
            if (IsByLogin)
            {
                bool istrieutap = true;
                foreach (Game game in Party)
                {
                    if (!game.GoToEx(DAILY.CauPhucThienQuan))
                    {
                        istrieutap = false;
                    }
                }
                if (istrieutap)
                {
                    if (Party.Count() < 6)
                    {
                        return;
                    }
                    Talk(DAILY.CauPhucThienQuan);
                    Thread.Sleep(1000);
                    QuestFrame.Click("#{TGQF_100111_03}");
                    Thread.Sleep(1000);
                    Talk(DAILY.CauPhucThienQuan);
                    Thread.Sleep(1000);
                    if (TLBB.IsQuestOpen)
                    {
                        if (QuestFrame.Click("#{TGQF_100111_04}"))
                        {
                            if (!IsThoiBong)
                                Party.ToList().ForEach(game => game.IsXongBHD = true);
                            else
                                Party.ToList().ForEach(game => game.RemoveMission(MissionsType.NhanBong));
                        }
                    }
                    else
                    {
                        Talk(DAILY.CauPhucThienQuan);
                    }
                }
                return;
            }
            if (IsXongBong)
            {
                if (TLBB.PartyId != 0xFFFFFFFF)
                {
                    DoStringEx("if Player:IsInTeam() == 1 then Player:LeaveTeam() elseif Player:IsInRaid() == 1 then Player:LeaveRiad() end");
                }
                else
                {
                    IsXongBong = false;
                    RemoveMission(MissionsType.NhanBong);
                }
                return;
            }
            if (!IsOneSec)
                return;
            string party = "";
            foreach (Game game in Party)
            {
                party += game.TLBB.Id + "-" + game.TLBB.Name + ";";
            }
            party.Trim(';');
            bool valid = true;
            bool isHave = false;
            Dictionary<string, string> leaders = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> kvp in CalendarEx.Balls)
            {
                if (kvp.Value.Contains(TLBB.Id))
                {
                    string members = kvp.Value;
                    if (!leaders.ContainsKey(members.Substring(0, members.IndexOf("-"))))
                        leaders.Add(members.Substring(0, members.IndexOf("-")), members);
                    isHave = true;
                }
            }
            if (!isHave)
            {
                RemoveMission(MissionsType.NhanBong);
                return;
            }
            if (TLBB.IsLeader)
            {
                if (!leaders.ContainsKey(TLBB.Id))
                {
                    DoStringEx("if Player:IsInTeam() == 1 then Player:LeaveTeam() elseif Player:IsInRaid() == 1 then Player:LeaveRiad() end");
                    valid = false;
                }
            }
            else
            {
                if (TLBB.PartyId == 0xFFFFFFFF)
                {
                    foreach (KeyValuePair<int, Game> kvp in Main.DicGame)
                    {
                        if (leaders.ContainsKey(kvp.Value.TLBB.Id))
                        {
                            if (kvp.Value.TLBB.PartyId == 0xFFFFFFFF)
                            {
                                DoStringEx("Player:CreateTeamSelf();");
                            }
                            else
                            {
                                AskTeam(kvp.Value.TLBB.VISCIIName);
                            }
                        }
                    }
                    valid = false;
                }
                else if (Party.Count() > 2)
                {
                    DoStringEx("if Player:IsInTeam() == 1 then Player:LeaveTeam() elseif Player:IsInRaid() == 1 then Player:LeaveRiad() end");
                    valid = false;
                }
            }
            if (valid && Party.Count() == 2)
            {
                if (TLBB.IsLeader)
                {
                    TrieuTap();
                    if (GoToEx(DAILY.CauPhucThienQuan))
                    {
                        foreach (Game game in Party)
                        {
                            if (game.TLBB.MapId != MAP.DaiLy)
                                return;
                            if (game.GetDistance(DAILY.CauPhucThienQuan.X, DAILY.CauPhucThienQuan.Y) >= 8)
                            {
                                return;
                            }
                        }
                        if (TLBB.IsQuestOpen)
                        {
                            if (QuestFrame.Click("#{TGQF_100111_04}"))
                            {
                                Party.ToList().ForEach(game => game.IsXongBong = true);
                            }
                        }
                        else
                        {
                            Talk(DAILY.CauPhucThienQuan);
                        }
                    }
                }
            }
        }

        public void CreateTeam()
        {
            DoStringEx("Player:CreateTeamSelf();");
        }

        public bool IsXongBong { get; set; }

        public static HashSet<string> JunkItem10x = new HashSet<string>
        {
            "Ô Mã Phủ", // Đao Búa
            "Khảm Kim Minh Đầu Chuý", // Đao Búa
            "Ngao Hán", // Thương Bổng
            "Tháp Cáp", // Đơn Đoản
            "Nộ Cáp", // Song Đoản
            "Sa Khâu", // Phiến
            "Hạ Phất Hoàn", // Khuyên
            "Khai Nguyên Mảo", // Mão
            "Vô Đạo Quán", // Mão
            "Mạc Sầu Mạo", // Mão
            "Khai Nguyên Cừu", // Y Phục
            "Sư Vương Cẩm Bào", // Y Phục
            "Cao Xương Bào", // Y Phục
            "Mạc Sầu Y", // Y Phục
            "Lan Thấm Hộ Oản", // Hộ Uyển
            "Tiêu Dao Ngoa", // Hài
            "Khai Nguyên Ngoa", // Hài
            "Mạc Sầu Hài", // Hài
            "Mạc Sầu Hộ Thủ", // Hộ Thủ
            "Thiền Quyên Hộ Thủ", // Hộ Thủ
            "Khai Nguyên Hộ Thủ", // Hộ Thủ
            "Kiền Tâm Hộ Thủ", // Hộ Thủ
            "An Tức Chỉ Hoàn", // Giới Chỉ
            "Lưu Tinh", // Hộ Phù
            "Nạp Lâm Hộ Kiên", // Hộ Kiên
            "Huyết Ngọc Đai", // Yêu Đai
        };

        public static HashSet<string> Weapon = new HashSet<string>()
        {
            "Phiến",
            "Đao Búa",
            "Song Đoản",
            "Thương Bổng",
            "Khuyên",
            "Nỏ",
            "Đơn Đoản",
            "Loại Trường Trượng",
            "Loại Tiêu Kiếm"
        };

        public static HashSet<string> JunkItemName = new HashSet<string>
        {
            "Tang Cúc Chúc", // ăn 204 khí
            "Tùng Tử Chúc", // ăn 672 khí
            "Hắc Mễ Chúc", // ăn 909 khí
            "Hồ Đào Chúc", // ăn 1140 khí
            "Hải Tiên Chúc", // ăn 1380 khí
            "Xuyên Bối Bách Hợp Chúc", // ăn 1860 khí

            "Tô hợp hương đậu chúc", // ăn 2340 khí
            "Thiết Đả Dược Cao", // ăn 360 máu
            "Bạch Phong Cao", // ăn 810 máu
            "Thiên Ma Cao", // ăn 1180 máu
            "Đỗ Trọng Cao", // ăn 1595 máu
            "Bát Trân Cao", // ăn 2005 máu
            "Hồng Nhan Cao", // ăn 2415 máu
            "Diên Thọ Cao", // ăn 3650 máu

            "Hạnh Đào Lạc", // ăn 2840 máu
            "Kim Tiền Ma Hoa", // ăn 3852 máu
            "Thất khiếu hoàn", // ăn 4060 máu
            "Xuân Bính", // ăn 4860 máu
            "Đậu Muộn Phạn", // ăn 5890 máu
            "Băng Hoa Cầu", // ăn 7950 máu
            "Ba tư dương thối", // ăn 10000 máu
            "Thủy Tinh Hoàn Tử", // ăn 20500 máu
            "Bánh Tiêm", // ăn 22310 máu

            "Hành Khí Tán", // hồi 184 khí
            "Thái Hương Hoàn", // hồi 760 khí
            "Hành Huyết Tán", // hồi 450 máu
            "Lộc Nhung Đan", // hồi 1992 máu
            "Hoạt Huyết Tán", // hồi khí 964 điểm
            "Phục Linh Cao", // hồi sinh lực 770 điểm
            "Ngưu Hoàn Phấn", // hồi sinh lực 1820 điểm
            "Kiện Bộ Cao", // hồi sinh lực 3240 điểm
            "Bất Lão Cao", // hồi sinh lực 4060 điểm
            "Hoàn Linh Đan", // hồi khí 376 điểm
            "Đậu khấu đan", // hồi khí 760 điểm
            "Sơn Dược Chúc", // hồi khí 434 điểm
            "Phấn Bì", // ăn khí 7600 điểm
            "Thủy Tinh Hoàn Tử", // hồi máu 20500

            "Tiểu Hành Nang", // ô đạo cụ + 1
            "Trung Hành Nang", // ô đạo cụ + 2
            "Đại Hành Nang", // ô đạo cụ + 3
            "Tiểu Cách Rương", // ô nguyên liệu + 1
            "Trung Cách Rương", // ô nguyên liệu + 2
            "Đại Cách Rương", // ô nguyên liệu + 3

            "Linh Thú Giáp",
            "Linh Thú Diện",
            "Linh Thú Sức",
            "Linh Thú Trảo",
            "Linh Thú Hoàn",
        };

        public static HashSet<string> JunkItemType = new HashSet<string>
        {
            "Nguyên liệu đúc",
            "Vật liệu may mặc",
            "N.liệu công nghệ",
            "Thịt Sơ Cấp",
            "Thịt Trung Cấp",
            "Thịt Cao Cấp",
            "Da Sơ Cấp",
            "Da Trung Cấp",
            "Da Cao Cấp",
            "Vật liệu chế dược",
        };

        public static HashSet<string> TrangBi = new HashSet<string>
        {
            "Phiến",
            "Đao Búa",
            "Song Đoản",
            "Thương Bổng",
            "Khuyên",
            "Nỏ",
            "Đơn Đoản",
            "Hộ Phù",
            "Hộ Kiên",
            "Hộ Uyển",
            "Y Phục",
            "Hài",
            "Hộ Thủ",
            "Mão",
            "Hạng Liên",
            "Yêu Đai",
            "Giới Chỉ"
        };

        //Le cao tri
        /////////////////////////////////////////////////////////////

        public string SatTinhMonter = "";

        public List<string> HaveToCatName { get; set; } = new List<string>();
        public List<string> HaveToCatThienCoName { get; set; } = new List<string>();
        public List<string> HaveToLayName { get; set; } = new List<string>();
        public List<string> HaveToLayType { get; set; } = new List<string>();
        public List<string> HaveToLayThienCoName { get; set; } = new List<string>();

        public List<string> HaveToDropName { get; set; } = new List<string>();
        //public List<string> HaveToDropType { get; set; } = new List<string>();
        public List<string> HaveToDropNameLocked { get; set; } = new List<string>();
        //public List<string> HaveToDropTypeLocked { get; set; } = new List<string>();

        public List<string> HaveToUseName { get; set; } = new List<string>();
        public List<string> HaveToUseNameLocked { get; set; } = new List<string>();
        public List<string> HaveToUseEquipName { get; set; } = new List<string>();
        public List<string> HaveToUseEquipType { get; set; } = new List<string>();

        public static List<string> HaveGomName { get; set; } = new List<string>();
        public static List<string> HaveGomKNBName { get; set; } = new List<string>();

        public void LayDo()
        {
            if (Global.Paused)
            {
                RemoveMission(new[] { MissionsType.LayDo, MissionsType.LayVang });
                return;
            }
            StopFollow();
            if (GoToEx(TLBB.NPCThuongKho))
            {
                if (!TLBB.IsBankOpen)
                {
                    Talk(TLBB.NPCThuongKho.Id);
                    Thread.Sleep(1000);
                    foreach (QuestFrame quest in QuestFrame.Enum(this))
                    {
                        if (quest.StrOptionExtra1 == 7 && quest.StrOptionExtra2 == 0xFFFFFFFF)
                        {
                            QuestFrameOptionClicked(quest);
                            Thread.Sleep(1000);
                        }
                    }
                }
                if (TLBB.IsBankOpen)
                {
                    if (Missions.Contains(MissionsType.LayVang))
                    {
                        DoStringEx("Bank:GetMoneyFromBank(" + TLBB.BankGold + ");");
                        Thread.Sleep(350);
                        RemoveMission(MissionsType.LayVang);
                    }
                    if (Missions.Contains(MissionsType.LayDo))
                    {
                        foreach (PacketItem item in PacketItems.Bank)
                        {
                            if (Setting.Is("checkKhongLayCoDinh") && item.IsCoDinh)
                                continue;
                            if (item.Name == "" || item.Type == "")
                                continue;
                            if (HaveToLayName.Contains(item.Name) || HaveToLayType.Contains(item.TypeName))
                            {
                                item.DoSubAction();
                                Thread.Sleep(350);
                            }
                        }
                        RemoveMission(MissionsType.LayDo);
                    }
                    LUA.CloseBank();
                }
            }
        }


        public void NopTuViHuyTinh()
        {
            if (GoToEx(KimLang.AnHienKy))
            {
                foreach (PacketItem item in PacketItems.ThienCo)
                {
                    if (item.Name == "Tử Vi Huy Tinh")
                    {
                        GetItemThienCo(item.Index);
                        Thread.Sleep(350);
                    }
                }
                Talk(KimLang.AnHienKy);
                Thread.Sleep(1000);
                if (TLBB.IsQuestOpen)
                {
                    QuestFrame.Click("#{ZWLP_200812_01}");
                    Thread.Sleep(350);
                    for (int i = 0; i < 8; i++)
                    {
                        QuestFrame.Click("#{ZWLP_200812_11}");
                        Thread.Sleep(350);
                        if (QuestFrame.Text.Contains("#{ZWLP_200812_08}15}"))
                        {
                            break;
                        }
                    }
                    Talk(KimLang.AnHienKy);
                    Thread.Sleep(1000);
                    QuestFrame.Click("#{ZWLP_200812_02}");
                    Thread.Sleep(1000);
                    if (QuestFrame.Click("#{ZWLP_200812_14}"))
                        RemoveMission(MissionsType.NopTuViHuyTinh);
                }
            }
        }

        public void LayDoThienCo()
        {
            foreach (PacketItem packetItem in PacketItems.ThienCo)
            {
                if (packetItem.Name == "" || packetItem.Type == "")
                    continue;
                if (HaveToLayThienCoName.Contains(packetItem.Name) || HaveToLayThienCoName.Contains(packetItem.TypeName))
                {
                    GetItemThienCo(packetItem.Index);
                }
            }
        }

        public void LoadDrop()
        {
            if (!TLBB.Online)
                return;

            HaveToCatName = SettingOld.CatName.Split('\n').ToList().Select(s => s.Trim()).Where(s => s.Length > 1).Distinct().ToList();            
            HaveToCatThienCoName = SettingOld.CatThienCoName.Split('\n').ToList().Select(s => s.Trim()).Where(s => s.Length > 1).Distinct().ToList();           
            HaveToDropNameLocked.Clear();
            HaveToDropName.Clear();
            //HaveToDropTypeLocked.Clear();
            //HaveToDropType.Clear();
            HaveToUseEquipName.Clear();
            HaveToUseNameLocked.Clear();
            HaveToUseName.Clear();
            HaveToUseEquipType.Clear();
            HaveToLayName = SettingOld.LayName.Split('\n').ToList().Select(s => s.Trim()).Where(s => s.Length > 1).Distinct().ToList();
            HaveToLayType = SettingOld.LayType.Split('\n').ToList().Select(s => s.Trim()).Where(s => s.Length > 1).Distinct().ToList();
            HaveToLayThienCoName = SettingOld.LayThienCoName.Split('\n').ToList().Select(s => s.Trim()).Where(s => s.Length > 1).Distinct().ToList();
            
            HaveGomName = SettingOld.GomName.Split('\n').ToList().Select(s => s.Trim()).Where(s => s.Length > 1).Distinct().ToList();           
            HaveGomKNBName = SettingOld.GomKNBName.Split('\n').ToList().Select(s => s.Trim()).Where(s => s.Length > 1).Distinct().ToList();
            

            foreach (string str in SettingOld.DropName.Split('\n').ToList().Select(s => s.Trim()).Where(s => s.Length > 1))
            {
                string strs = str;
                if (str.Contains("{") && str.Contains("}"))
                {
                    if (str.Contains("{" + TLBB.Name + "}"))
                    {
                        strs = str.Replace("{" + TLBB.Name + "}", "");
                    }
                    else
                    {
                        continue;
                    }
                }
                if (strs.Contains("[Locked]"))
                {
                    HaveToDropNameLocked.Add(strs.Replace("[Locked]", "").Trim());
                }
                else if (strs.Trim().Length > 1)
                {
                    HaveToDropName.Add(strs.Trim());
                }
            }

            //foreach (string str in Setting.DropType.Split('\n').ToList().Select(s => s.Trim()).Where(s => s.Length > 1))
            //{
            //    string strs = str;
            //    if (str.Contains("{") && str.Contains("}") && str.Contains("{" + TLBB.Name + "}"))
            //    {
            //        if (str.Contains("{" + TLBB.Name + "}"))
            //        {
            //            strs = str.Replace("{" + TLBB.Name + "}", "");
            //        }
            //        else
            //        {
            //            continue;
            //        }
            //    }
            //    if (strs.Contains("[Locked]"))
            //    {
            //        if (strs.Replace("[Locked]", "").Trim().Length > 1)
            //            HaveToDropTypeLocked.Add(strs.Replace("[Locked]", "").Trim());
            //    }
            //    else if (strs.Trim().Length > 1)
            //    {
            //        HaveToDropType.Add(strs.Trim());
            //    }
            //}

            foreach (string str in SettingOld.UseName.Split('\n').ToList().Select(s => s.Trim()).Where(s => s.Length > 1))
            {
                string strs = str;
                if (str.Contains("{") && str.Contains("}"))
                {
                    if (str.Contains("{" + TLBB.Name + "}"))
                    {
                        strs = str.Replace("{" + TLBB.Name + "}", "");
                    }
                    else
                    {
                        continue;
                    }
                }
                if (str.Contains("Equip"))
                {
                    if (str.Replace("[Equip]", "").Trim().Length > 1)
                        HaveToUseEquipName.Add(str.Replace("[Equip]", "").Trim());
                }
                else
                {
                    if (strs.Contains("[Locked]"))
                    {
                        if (strs.Replace("[Locked]", "").Trim().Length > 1)
                            HaveToUseNameLocked.Add(strs.Replace("[Locked]", "").Trim());
                    }
                    else
                    {
                        if (strs.Trim().Length > 1)
                            HaveToUseName.Add(strs.Trim());
                    }
                }
            }
        }

        public bool SaveItem()
        {
            if (!IsOpenPass2)
                return Missions.Contains(MissionsType.CatDo);
            if (TLBB.SafeTime > 0)
                return Missions.Contains(MissionsType.CatDo);
            if (!IsOneSec)
                return Missions.Contains(MissionsType.CatDo);
            bool isCat = false;
            if (Missions.Contains(MissionsType.CatDo))
            {
                if (TLBB.IsFollow)
                    StopFollow();
                if (GoToEx(TLBB.NPCThuongKho))
                {
                    if (!TLBB.IsBankOpen)
                    {
                        if (!TLBB.IsQuestOpen)
                        {
                            Talk(TLBB.NPCThuongKho.Id);
                            return Missions.Contains(MissionsType.CatDo);
                        }
                        foreach (QuestFrame quest in QuestFrame.Enum(this))
                        {
                            if (quest.StrOptionExtra1 == 7 && quest.StrOptionExtra2 == 0xFFFFFFFF)
                            {
                                QuestFrameOptionClicked(quest);
                                return Missions.Contains(MissionsType.CatDo);
                            }
                        }
                    }
                    else
                    {
                        CloseQuest();
                    }
                }
            }
            if (Main.SettingForm.checkBankItem.Checked || Missions.Contains(MissionsType.CatDo))
            {
                if (Address.GameType == 1 && !Global.Paused)
                {
                    if (TLBB.IsBankOpen)
                    {
                        if (Setting.Is("checkKhiCatDo"))
                        {
                            foreach (var item in PacketItems.ThienCo)
                            {
                                if (Setting.Is("checkChiCatCoDinh") && !item.IsCoDinh)
                                    continue;
                                if (item.Name == "" || item.Type == "")
                                    continue;
                                if (HaveToCatName.Contains(item.Name) || HaveToCatName.Contains(item.TypeName))
                                {
                                    GetItemThienCo(item.Index);
                                    break;
                                }
                            }
                        }
                        foreach (var item in PacketItems.All)
                        {
                            if (Setting.Is("checkChiCatCoDinh") && !item.IsCoDinh)
                                continue;
                            if (item.Name == "" || item.Type == "")
                                continue;
                            if (HaveToCatName.Contains(item.Name.Trim()) || HaveToCatName.Contains(item.TypeName.Trim()))
                            {
                                item.DoSubAction();
                                PushDebugMessage("auto vừa cất " + item.Name + " vào rương [Paused để ngừng]");
                                isCat = true;
                                break;
                            }
                            if (isCat)
                                break;
                        }
                        if (!isCat)
                        {
                            RemoveMission(MissionsType.CatDo);
                        }
                    }
                }
            }
            return Missions.Contains(MissionsType.CatDo);
        }


        public void DoiNgocThoiTrang()
        {
            if (GoToEx(DAILY.TonBatGia))
            {
                if (!HaveItem("DongSonThach"))
                {
                    RemoveMission(MissionsType.DaiLeHungVuong);
                }
                Talk(DAILY.TonBatGia);
                Thread.Sleep(1000);
                QuestFrame.Click("#{VNXWDH_220321_09}");
                Thread.Sleep(1000);
                QuestFrame.Click("#{VNXWDH_220321_01}");
            }
        }

        public void UseItem()
        {
            if (!IsOpenPass2)
                return;
            if (IsX4)
            {
                if (Objects.Self != null && !Objects.Self.Buff.Contains(1685))
                {
                    foreach (var item in PacketItems.DaoCu)
                    {
                        if (item.Name == "Huyền Linh Đan")
                        {
                            item.Use();
                            Thread.Sleep(350);
                            LUA.MessageBox_Self_OK_Clicked();
                            break;
                        }
                    }
                }
            }
            if (Isx2VoY)
            {
                if (Objects.Self != null && !Objects.Self.Buff.Contains(4884))
                {
                    foreach (var item in PacketItems.All)
                    {
                        if (item.Name == "Linh Tức Đan")
                        {
                            item.Use();
                            Thread.Sleep(350);
                            LUA.MessageBox_Self2_OK_Clicked();
                            break;
                        }
                    }
                }
            }
            if (Main.SettingForm.checkUseItem.Checked)
            {
                if (SecCount % 3 == 0)
                {
                    foreach (var item in PacketItems.TuiChanNguyen)
                    {
                        if (IsUse(item))
                        {
                            PhanGiaiChanNguyen(item.Index);
                        }
                    }
                }
                if (Setting.Is("checkKhiDungDo"))
                {
                    if (SecCount % 10 == 0)
                    {
                        PacketItems.ThienCo.Where(i => IsUse(i)).ForEach(i => GetItemThienCo(i.Index));
                    }
                }
                foreach (var item in PacketItems.DaoCu)
                {
                    if (IsUse(item))
                    {
                        if (item.Type == "CircularTaskTool24_7" || item.Name == "Tiểu Huyền Linh Đan")
                        {
                            if (!Objects.Self.Buff.Contains(1696))
                            {
                                DoStringEx("PlayerPackage:UseItem(" + item.Index + ");");
                                //lecaotri
                                Thread.Sleep(500);
                                LUA.MessageBox_Self_OK_Clicked();
                                Thread.Sleep(500);
                            }
                        }
                        else
                        {
                            float maxdis = 6;
                            if (Global.MaxBong <= 1)
                                maxdis = 3;

                            if (TDT.VietLien(item.Name).Contains("cauphuc"))
                            {
                                if (TDT.GetDistance(Global.ThoiBongX, Global.ThoiBongY, RoundX, RoundY) >= maxdis)
                                {
                                    continue;
                                }
                            }
                            if (Global.MaxBong > 1)
                            {
                                if (TDT.VietLien(item.Name).Contains("cauphuc"))
                                {
                                    Tuple<int, int> tupe = GetToaDoThoiBong();
                                    if (tupe.Item1 != -1)
                                    {
                                        Thread.Sleep(1000);
                                        GoToEx(tupe.Item1, tupe.Item2, -1, true);
                                        Thread.Sleep(3000);
                                        if (TLBB.IsRide)
                                            DownRide();
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                            }
                            item.Use();
                        }
                    }
                    if (IsEquip(item))
                    {
                        item.DoAction();
                    }
                }
            }
        }

        public bool IsQuanSonHai { get; set; }

        public int countsovong = 0;
        private bool isdakhieuchien = false;

        public bool Isx2VoY { get; set; }

        private Stopwatch quansonhaitime = Stopwatch.StartNew();



        public void VoY()
        {
            //#{WYYD_20170816_41}
            if (GoToEx(KimLang.LacHaPhong))
            {
                Talk(KimLang.LacHaPhong);
                Thread.Sleep(500);
                if (TLBB.IsQuestOpen)
                {
                    QuestFrame.Click("#{WYYD_20170816_41}");
                    Thread.Sleep(500);
                    QuestFrame.Accept();
                    Thread.Sleep(500);
                    Talk(KimLang.LacHaPhong);
                    Thread.Sleep(500);
                    QuestFrame.Click("#{WYYD_20170816_41}");
                    Thread.Sleep(500);
                    LUA.QuestFrameMissionComplete();
                    RemoveMission(MissionsType.NhiemVuVoY);
                }
            }
        }

        public Tuple<int, int> GetToaDoThoiBong()
        {
            int x = -1;
            int y = -1;
            if (Objects.All.Where(o => o.CleanName.Contains("thiencungcauphuc") && TDT.VietLien(o.Title).Contains(TLBB.Name)).Count() > 0)
            {
                return Tuple.Create(x, y);
            }
            if (Objects.All.Where(o => o.CleanName.Contains("thiencungcauphuc") && TDT.GetDistance(o.X, o.Y, Global.ThoiBongX, Global.ThoiBongY) <= 6).Count() >= Global.MaxBong)
            {
                return Tuple.Create(x, y);
            }
            for (int i = -4; i <= 4; i++)
            {
                for (int j = -4; j <= 4; j++)
                {
                    x = Global.ThoiBongX + i;
                    y = Global.ThoiBongY + j;
                    if (Objects.All.Where(o => o.CleanName.Contains("thiencungcauphuc") && TDT.GetDistance(x, y, o.X, o.Y) < 4).Count() > 0)
                    {
                        x = -1;
                        y = -1;
                    }
                    else
                    {
                        x = Global.ThoiBongX + i;
                        y = Global.ThoiBongY + j;
                    }
                    if (x != -1)
                        break;
                }
                if (x != -1)
                    break;
            }

            return Tuple.Create(x, y);
        }

        public void DropItem()
        {
            if (!IsOpenPass2)
                return;
            if (TLBB.SafeTime > 0)
                return;
            if (SecCount % 3 == 0)
            {
                if (Setting.Is("checkDropItem"))
                {
                    foreach (PacketItem packetItem in PacketItems.All.Concat(PacketItems.ThienCo))
                    {
                        if (IsDrop(packetItem))
                        {
                            DropItemThienCo(packetItem.Index);
                        }
                    }
                }            
            }
        }

        public bool IsSale { get; set; }

        public void AutoBuyItem()
        {
            if (!Main.SettingForm.checkBuyItem.Checked)
                return;
            if (!TLBB.IsShopOpen)
                return;
            if (!IsOneSec)
                return;
            foreach (string name in string.Concat(BuyIts, ";", SettingOld.BuyIts).Split(';'))
            {
                int num = 0;
                if (name.Split('|').Length >= 2)
                {
                    string n = name.Split('|')[0];
                    int cnt = 0;
                    foreach (var item in PacketItems.All)
                    {
                        if (item.ClearName == TDT.VietLien(name.Split('|')[0]))
                        {
                            cnt += (int)item.Count;
                        }
                    }
                    if (cnt < TDT.ParseInt(name.Split('|')[1]))
                    {
                        num = TDT.ParseInt(name.Split('|')[1]) - cnt;
                    }
                }
                if (num > 0)
                {
                    foreach (var s in PacketItems.Shop)
                    {
                        if (TDT.VietLien(s.Name) == TDT.VietLien(name.Split('|')[0]))
                        {
                            while (num > 0)
                            {
                                int bu = num;
                                if (bu > 20)
                                    bu = 20;
                                LUA.Buy((int)s.Index, bu);
                                Thread.Sleep(300);
                                num = num - bu;
                            }
                        }
                    }
                }
            }
        }

        public void TuChucPhuc()
        {
            string hex = "88 98 16 01 00 00 82 01 00 00 00 00 FF FF FF FF 02";
            hex = HexToString(0x689888 + AddressGameExe) + "00 00 00 00 00 00 00 00 FF FF FF FF 01";
            SendPacket(hex);
            hex = HexToString(0x689888 + AddressGameExe) + "00 00 00 00 00 00 00 00 FF FF FF FF 02";
            SendPacket(hex);
        }

        public void ChucPhuc(string id)
        {
            id = id.Replace("0X", "");
            string he = HexToString(0x68DB74 + AddressGameExe) + "00 00 00 00 00 00 00 00 FF FF FF FF 01 00 00 00 " + HexToString(id.Substring(0, 8)) + HexToString(id.Substring(8, 8));
            SendPacket(he);
            string hex = HexToString(0x6898AC + AddressGameExe) + "00 00 00 00 00 00 00 00 FF FF FF FF FF FF 00 00 FF FF FF FF FF FF FF FF 01 00 00 00" + HexToString(id.Substring(0, 8)) + HexToString(id.Substring(8, 8));
            SendPacket(hex);
        }

        public bool SellItem()
        {
            if (!Main.SettingForm.checkSellItem.Checked)
                return false;
            if (!IsOpenPass2)
                return false;
            if (!TLBB.IsShopOpen)
                return false;
            IsSale = false;
            if (Setting.Is("checkKhiBanRac"))
            {
                foreach (var item in PacketItems.ThienCo)
                {
                    if (item.Name == "Tiền Vàng Con Giáp Cao Cấp")
                        continue;
                    if (IsSell(item))
                    {
                        GetItemThienCo(item.Index);
                        IsSale = true;
                    }
                }
            }
            for (uint i = 0; i < 60; i++)
            {
                PacketItem item = PacketItems[i];
                if (item != null && IsSell(item))
                {
                    if (!TLBB.IsShopOpen)
                        break;
                    if (Address.GameType == 1)
                    {
                        SendSell(item.Index);
                        Thread.Sleep(50);
                    }
                    else
                    {
                        item.Sell();
                        Thread.Sleep(350);
                    }
                    
                    IsSale = true;
                    
                }
            }
            return IsSale;
        }

        void SendSell(uint index)
        {
            SendPacket(HexToString(AddressGameExe + 0x6A0C98) + " 00 00 00 00 00 00 00 00 FF FF FF FF " + index.ToString("X2") + " 00 80 3F " + TLBB.FakeMapId.ToString("X4").Substring(2,2) + TLBB.FakeMapId.ToString("X4").Substring(0, 2) + " 00 00 " + Memory.Read(AddressGameExe + 0x99AB00, 0x1CB90).ToString("X2"));
        }

        public float SaveX { get; set; }
        public float SaveY { get; set; }

        public bool IsUse(PacketItem item)
        {
            if (item.Name.Trim() == "" || item.TypeName.Trim() == "")
                return false;
            if(Missions.Contains(MissionsType.GomKNB))
            {
                if (item.Name == "Phiếu Kim Nguyên Bảo")
                    return false;
            }
            if (HaveToUseName.Contains(item.Name.Trim()))
                return true;
            if (HaveToUseName.Contains(item.TypeName.Trim()))
                return true;
            if (item.IsCoDinh)
            {
                if (HaveToUseNameLocked.Contains(item.Name.Trim()))
                    return true;
                if (HaveToUseNameLocked.Contains(item.TypeName.Trim()))
                    return true;
            }
            return false;
        }

        public bool IsEquip(PacketItem packetItem)
        {
            if (packetItem.Name.Trim() == "" || packetItem.TypeName.Trim() == "")
                return false;
            if (HaveToUseEquipName.Contains(packetItem.Name))
                return true;
            if (HaveToUseEquipType.Contains(packetItem.TypeName))
                return true;
            return false;
        }

        public bool IsDrop(PacketItem item)
        {
            if (item.Name.Trim() == "" || item.TypeName.Trim() == "")
                return false;
            if (HaveToDropName.Contains(item.TypeName))
                return true;
            if (HaveToDropName.Contains(item.Name.Trim()))
                return true;
            if (HaveToDropName.Contains("Trang Bị 10x,11x"))
            {
                if (TrangBi.Contains(item.TypeName) || Weapon.Contains(item.TypeName))
                {
                    if (item.Lvl >= 110)
                    {
                        return true;
                    }
                    else if (item.Lvl >= 100)
                    {
                        if (item.Star == 0)
                        {
                            return true;
                        }
                    }
                }
            }
            if (HaveToDropName.Contains("Trang Bị 1,2,3,4 Sao"))
            {
                if (TrangBi.Contains(item.TypeName) || Weapon.Contains(item.TypeName))
                {
                    if (item.TypeName == "Đơn Đoản" && item.Lvl == 1 && item.Star == 1 && item.Line == 0)
                        return false;
                    if (item.Star == 0)
                    {
                        return false;
                    }
                    if (item.Star >= 1 && item.Star <= 4)
                    {
                        return true;
                    }
                }
            }
            if (HaveToDropName.Contains("Trang Bị 5 Sao"))
            {
                if (TrangBi.Contains(item.TypeName) || Weapon.Contains(item.TypeName))
                {
                    if (item.TypeName == "Đơn Đoản" && item.Lvl == 1 && item.Star == 1 && item.Line == 0)
                        return false;
                    if (item.Star == 0)
                    {
                        return false;
                    }
                    if (item.Star == 5)
                    {
                        return true;
                    }
                }
            }
            if (item.IsCoDinh)
            {
                if (HaveToDropNameLocked.Contains("Trang Bị 10x,11x"))
                {
                    if (TrangBi.Contains(item.TypeName) || Weapon.Contains(item.TypeName))
                    {
                        if (item.Lvl >= 110)
                        {
                            return true;
                        }
                        else if (item.Lvl >= 100)
                        {
                            if (item.Star == 0)
                            {
                                return true;
                            }
                        }
                    }
                }
                if (HaveToDropNameLocked.Contains("Trang Bị 1,2,3,4 Sao"))
                {
                    if (TrangBi.Contains(item.TypeName) || Weapon.Contains(item.TypeName))
                    {
                        if (item.TypeName == "Đơn Đoản" && item.Lvl == 1 && item.Star == 1 && item.Line == 0)
                            return false;
                        if (item.Star == 0)
                        {
                            return false;
                        }
                        if (item.Star >= 1 && item.Star <= 4)
                        {
                            return true;
                        }
                    }
                }
                if (HaveToDropNameLocked.Contains("Trang Bị 5 Sao"))
                {
                    if (TrangBi.Contains(item.TypeName) || Weapon.Contains(item.TypeName))
                    {
                        if (item.TypeName == "Đơn Đoản" && item.Lvl == 1 && item.Star == 1 && item.Line == 0)
                            return false;
                        if (item.Star == 0)
                        {
                            return false;
                        }
                        if (item.Star == 5)
                        {
                            return true;
                        }
                    }
                }
            }
            if (item.IsCoDinh)
            {
                if (HaveToDropNameLocked.Contains(item.Name.Trim()))
                    return true;
                if (HaveToDropNameLocked.Contains(item.TypeName))
                    return true;
            }
            return false;
        }

        public bool IsSell(PacketItem packetItem)
        {
            if (packetItem.Name.Trim() == "" || packetItem.TypeName == "")
                return false;
            if (SettingOld.HaveToSellName.Contains(packetItem.Name))
                return true;
            if (SettingOld.HaveToSellName.Contains(packetItem.TypeName))
                return true;
            if (SettingOld.SellDinhSan.Contains("Ngọc Thời Trang") && GAMEDIC.NgocThoiTrangCap1.Contains(packetItem.Name) && packetItem.IsCoDinh)
                return true;
            if (SettingOld.SellDinhSan.Contains("Vũ Khí Đả Tạo Đồ") && packetItem.TypeName == "Đả Tạo Đồ")
            {
                foreach (string name in GAMEDIC.VuKhiDaTaoDo)
                {
                    if (packetItem.Name.Contains(name))
                    {
                        return true;
                    }
                }
            }
            if (SettingOld.SellDinhSan.Contains("Đả Tạo Đồ 1,2,3,4") && packetItem.TypeName == "Đả Tạo Đồ")
            {
                if (TDT.ParseAllInt(packetItem.Name) <= 4)
                    return true;
            }
            if (SettingOld.SellDinhSan.Contains("Đục Lỗ") && packetItem.TypeName.Contains("Đục Lỗ"))
            {
                if (TDT.ParseAllInt(packetItem.TypeName) <= 7)
                    return true;
            }
            if (SettingOld.SellDinhSan.Contains("Thức Ăn"))
            {
                if (JunkItemName.Contains(packetItem.Name))
                    return true;
            }
            if (SettingOld.SellDinhSan.Contains("Trang Bị 1,2,3,4 Sao"))
            {
                if (TrangBi.Contains(packetItem.TypeName) || Weapon.Contains(packetItem.TypeName))
                {
                    if (packetItem.Star <= 4 && packetItem.Star >= 1)
                    {
                        return true;
                    }
                }
            }
            if (SettingOld.SellDinhSan.Contains("Trang Bị 5 Sao"))
            {
                if (TrangBi.Contains(packetItem.TypeName) || Weapon.Contains(packetItem.TypeName))
                {
                    if (packetItem.Star == 5)
                    {
                        return true;
                    }
                }
            }
            if (SettingOld.SellDinhSan.Contains("Trang Bị Chưa Giám Định"))
            {
                if (TrangBi.Contains(packetItem.TypeName) || Weapon.Contains(packetItem.TypeName))
                {
                    if (packetItem.Star == 0)
                    {
                        return true;
                    }
                }
            }
            if (SettingOld.SellDinhSan.Contains("Nguyên Liệu Đúc"))
            {
                if (JunkItemType.Contains(packetItem.TypeName))
                    return true;
            }
            List<PacketItem> listtrangbi = PacketItems.TrangBi.ToList();
            if (SettingOld.SellDinhSan.Contains("Long Văn, Chú Văn"))
            {
                if (packetItem.IsCoDinh && (packetItem.Name == "Long Văn Ngọc Linh" || packetItem.Name == "Chú Văn Huyết Ngọc"))
                {
                    if (listtrangbi.Where(p => p.TypeName == "Long Văn" && p.CapTruongThanhLongVan == 100).FirstOrDefault() != null)
                        return true;
                }
            }
            if (SettingOld.SellDinhSan.Contains("Huyền Binh Thạch") && packetItem.Name.Contains("Huyền Binh Thạch"))
            {
                if (listtrangbi.Where(p => Weapon.Contains(p.TypeName) && (TDT.ParseAllInt(p.Star.ToString().Substring(0, 1)) >= 8 || TDT.ParseAllInt(p.Star.ToString()) >= 80)).FirstOrDefault() != null)
                    return true;
            }
            if (SettingOld.SellDinhSan.Contains("Võ Hồn") && packetItem.TypeName == "Võ Hồn" && packetItem.IsCoDinh && packetItem.CapHopThanhVoHon <= 1)
            {
                if (listtrangbi.Where(p => p.TypeName == "Võ Hồn" && p.CapHopThanhVoHon == 7).FirstOrDefault() != null)
                    return true;
            }
            return false;
        }

        public string ToaDoTrain
        {
            get
            {
                return IniParser.Read("Train", TLBB.AllowName);
            }
            set
            {
                IniParser.Write("Train", TLBB.AllowName, value);
            }
        }

        public string ToaDoTrainEx
        {
            get
            {
                string train = ToaDoTrain;
                if (train.Split(',').Length == 3)
                {
                    string mapname = "Không Biết";
                    if (GAMEDIC.MapNameId.ContainsKey(TDT.ParseAllInt(train.Split(',')[2])))
                        mapname = GAMEDIC.MapNameId[(TDT.ParseAllInt(train.Split(',')[2]))];
                    return mapname + " [" + train.Split(',')[0] + "," + train.Split(',')[1] + "]";
                }
                else
                {
                    return "Chưa Có";
                }
            }
        }

        public void LoadSkill()
        {
            if (SwTimeOnMap.Elapsed.TotalSeconds < 5)
                return;
            if (!TLBB.Online)
                return;
            if (Skills.Count == 0 || (TLBB.Menpai == MENPAI.NgaMy && NgamySkill == null) || (TLBB.Lvl > 10 && Skills.Count < 10))
            {
                Skills = Skill.Enum(this);

                if (Skills.Count > 4)
                {
                    string setting = IniParser.Read(TLBB.Name,"Skill");
                    string settingex = IniParser.Read(TLBB.Name, "SkillPK");
                    NgamySkill = null;
                    foreach (Skill skill in Skills)
                    {
                        if (skill.PacketId == 424)
                        {
                            NgamySkill = skill;
                        }
                        if (Address.GameType != 1)
                        {
                            foreach (GameControl control in GameControl.Enum(this))
                            {
                                if (control.IsSkill && control.PacketId == skill.PacketId)
                                {
                                    skill.Name = control.Name;
                                    skill.Type = control.Type;
                                    break;
                                }
                            }
                        }
                        if (setting.Contains("[" + skill.PacketId + "]"))
                            skill.Use = true;
                        if (settingex.Contains("[" + skill.PacketId + "]"))
                            skill.UsePK = true;
                    }
                    if (NgamySkill == null)
                    {
                        foreach (Skill skill in Skills)
                        {
                            if (skill.PacketId == 407)
                            {
                                NgamySkill = skill;
                            }
                        }
                    }
                    SkillLoaded?.Invoke(this, null);
                }
                else
                {
                    Skills.Clear();
                }
            }
        }

        public string MemInfo { get; set; } = string.Empty;

        private void FollowKey()
        {
            if (TLBB.IsFollow)
                return;

            if (Global.FollowKey || Global.ForceFollowKey)
            {
                bool force = Global.ForceFollowKey;
                if (Objects.Key != null)
                {
                    if (Objects.Key.Id == SelfId)
                        return;
                    GoToEx(Objects.Key.X, Objects.Key.Y, -1, force, 3, 9999);
                }
                else if (Leader != null)
                {
                    if (Leader == this)
                        return;
                    if (TLBB.MapId != Leader.TLBB.MapId)
                    {
                        GoToEx(Leader.CharX, Leader.CharY, (int)Leader.TLBB.MapId, force, 3, 9999);
                    }
                    else
                    {
                        GoToEx(Leader.CharX, Leader.CharY, -1, force, 3, 9999);
                    }
                }
                else
                {
                    if (TLBB.PartyId != 0xFFFFFFFF)
                    {
                        if (!MemInfo.Contains("NULL"))
                        {
                            DoStringEx("meminfo = ''; iMemCount = DataPool:GetTeamMemberCount(); if((iMemCount < 1)or(iMemCount > 6)) then return; end for index = 0, iMemCount - 1 do MemberName , strIconIndex , HPValue , HPMax , MPValue , MPMax , Fammily , Level , Anger , DeadLink , Dead , sex , ScenceName = DataPool:GetTeamMemberInfo(index); meminfo = meminfo .. index .. ',' .. MemberName .. ',' .. ScenceName .. ';'; end return meminfo .. 'NULL';");
                            MemInfo = LuaToStringMemInfo();
                            //Main.TDTLog("mem");
                        }
                        else
                        {
                            lstmem.Clear();
                            Dictionary<string, MemIn> dicMap = new Dictionary<string, MemIn>();
                            foreach (string s in MemInfo.Split(';'))
                            {
                                if (s.Split(',').Length == 3)
                                {
                                    MemIn mem = new MemIn();
                                    mem.Index = TDT.ParseInt(s.Split(',')[0]);
                                    mem.Name = s.Split(',')[1];
                                    mem.MapName = s.Split(',')[2];
                                    if (!dicMap.ContainsKey(mem.Name))
                                        dicMap.Add(mem.Name, mem);
                                    lstmem.Add(mem);
                                }
                            }
                            if (lstmem.Count >= 2)
                            {
                                Team.Count = lstmem.Count;
                                Team.Read();
                                foreach (TeamMem mem in Team.All)
                                {
                                    if (dicMap.ContainsKey(mem.Name))
                                    {
                                        mem.MapName = dicMap[mem.Name].MapName;
                                        mem.Index = (uint)dicMap[mem.Name].Index;
                                        if (mem.Index == 1)
                                            mem.IsLeader = true;
                                    }
                                }
                                if (SettingOld.Leader.Trim().Length > 0)
                                {
                                    foreach (string str in SettingOld.Leader.Split('\n'))
                                    {
                                        bool isNewLead = false; ;
                                        if (str.Trim() == "")
                                            continue;
                                        foreach (TeamMem mem in Team.All)
                                        {
                                            if (TDT.VietLien(str) == TDT.VietLien(mem.Name))
                                            {
                                                foreach (TeamMem m in Team.All)
                                                {
                                                    m.IsLeader = false;
                                                }
                                                mem.IsLeader = true;
                                                isNewLead = true;
                                                break;
                                            }
                                        }
                                        if (isNewLead)
                                            break;
                                    }
                                }
                                foreach (TeamMem mem in Team.All)
                                {
                                    if (mem.IsLeader)
                                    {
                                        if (mem.Name == TLBB.Name)
                                            return;
                                        if (mem.MapName == TLBB.MapName)
                                        {
                                            if (TDT.GetDistance(CharX, CharY, mem.X, mem.Y) >= RemConfig.FollowRadius)
                                                GoToEx(mem.X, mem.Y, -1, force, 3, 9999);
                                        }
                                        else
                                        {
                                            int mapId = -1;
                                            if (GAMEDIC.MapNameId.ContainsValue(mem.MapName))
                                            {
                                                mapId = GAMEDIC.MapNameId.FirstOrDefault(x => x.Value == mem.MapName).Key;
                                            }
                                            if (mapId != -1)
                                            {
                                                GoToEx(100, 100, mapId, force, 3, 9999);
                                            }
                                        }
                                    }
                                }
                                MemInfo = string.Empty;
                            }
                        }
                    }
                }
            }
        }

        public List<MemIn> lstmem { get; set; } = new List<MemIn>();

        public class MemIn
        {
            public string Name { get; set; }
            public string MapName { get; set; }
            public int Index { get; set; }
        }

        public void UnlockPass2()
        {
            if (Pass2.Length >= 4)
                DoStringEx("UnLockMinorPassword(\"" + Pass2 + "\");");
        }

        public void SetWay(string way)
        {
            DoStringEx("WAY = \"" + way + "\"; SetWay()");
        }

        public uint RecvData
        {
            get;
            set;
        }

        public int RecvAddress
        {
            get;
            set;
        }

        public void LinhLuong()
        {
            if (GoToEx(DAILY.ChucPhucQuy))
            {
                Talk(DAILY.ChucPhucQuy);
                Thread.Sleep(1000);
                QuestFrame.Click("#{GZRW_XML_2}");
                Thread.Sleep(1000);
                if (QuestFrame.Click("#{GZGZ_120514_42}"))
                    Missions.Remove(MissionsType.LinhLuong);
            }
        }

        public static HashSet<string> ListNames { get; set; } = new HashSet<string>();
        public string LastAllowName { get; set; }

        public void LoadSetting()
        {
            NgamySkill = null;
            Skills.Clear();
            LastAllowName = TLBB.Name;



            if (Setting.Is("checkLuuTrangThai"))
            {
                string mission = IniParser.Read("Mission", TLBB.AllowName);
                foreach (string miss in mission.Split(','))
                {
                    if (miss.Length > 0)
                    {
                        int m = TDT.ParseInt(miss);
                        var enumValue = (MissionsType?)m;
                        if (!enumValue.HasValue)
                        {

                        }
                        else
                        {
                            PushMissions((MissionsType)m);
                        }
                    }
                }
            }

            string block = "";
            foreach (string s in SettingOld.BoQuaEx.Split('\n'))
            {
                if (s.Trim().Length >= 3)
                {
                    block += s.Trim() + "#";
                }
            }
            if (block.Length > 3 && Main.IsAutoBlock)
            {
                var adr = Memory.WriteString(block);
                PostMessage(1, 988);
                PostMessage(adr, 987);
            }
            else
            {
                PostMessage(0, 988);
            }

            try
            {
                try
                {


                    swHuyQ = null;
                    Skills.Clear();
                    //SkillLoaded?.Invoke(this, null);
                    MissionState = string.Empty;
                    MoveExTime = Stopwatch.StartNew();
                    //Main.pu
                    RaoTxt = IniParser.Read("Talk", TLBB.AllowName);
                    LastName = TLBB.Name;
                    sovongsumon = 0;
                    IsXongBHD = false;
                    LoadDrop();
                    IsCheckNhiemmVu = false;
                    checknvcount = 0;
                    IsX2Coban = false;
                    NhiemVuChuaLam1 = new List<string>()
            {
                "Sinh Tài Chi Đạo",
                "Khoa Ngân Võ Lâm Ấn Túc Cầu Thịnh Hội",
                "Khoa Ngân Võ Lâm Ấn Tàng Kinh Các Nguy Cơ",
                "Khoa Ngân Võ Lâm Ấn Phồn Mang Tào Vận",
                "Quả Ngân Võ Lâm Ấn Bang HộiBuôn Bán",
            };
                    NhiemVuChuaLam2 = new List<string>()
            {
                "Thanh Đồng Ấn-Tương Trợ Sư Môn",
                "Thanh Đồng Ấn-Trừ Ác",
                "Thanh Đồng Ấn-Trừng Hung",
            };
                    if (Setting.Is("checkDiemDanh"))
                    {
                        IsDiemDanh = false;
                        IsDiemDanhEx = false;
                    }
                    if (Address.GameType == 3)
                    {
                        string way = SettingOld.LoadWAY(TLBB.GuildId.ToString());
                        if (way.Contains("@way"))
                        {
                            SetWay(way);
                        }
                    }

                    DoStringEx("function AntiRobot_TimeReach()  AskRet2SelServer(); end");
                    if (Address.GameType == 1)
                    {
                        DoStringEx(Properties.Resources.Fix3D);
                        DoStringEx(Properties.Resources.Lua);
                    }
                    else if (Address.GameType == 2)
                    {
                        DoStringEx(Properties.Resources.Fix2D);
                        DoStringEx(Properties.Resources.Lua);
                    }

                    if (!ListNames.Contains(TLBB.Name))
                    {
                        ListNames.Add(TLBB.Name);
                    }
                }
                catch { }
                try
                {
                    string radius = IniParser.Read(LastAllowName, "Radius");
                    if(radius.Split(',').Length == 3)
                    {
                        RadiusX = radius.Split(',')[0].ToInt();
                        RadiusY = radius.Split(',')[1].ToInt();
                        RadiusMap = (uint)radius.Split(',')[2].ToInt();
                    }
                    buyits = null;
                    isCountMam = false;
                    Win.SetWindowText(Handle, TDT.ClearSign(TLBB.Name));
                    string set = IniParser.Read(LastAllowName, "Config");
                    int[] setting = SettingOld.String2Arr(set);
                    if (setting == null || setting.Length < 53)
                    {
                        for (int i = 0; i < 22; i++)
                            KeyDelay[i] = 1;
                        IsAttack = IsPet = IsHP = IsMP = IsAuto = IsNM = true;
                    }
                    else
                    {
                        try
                        {
                            IsAttack = setting[1] == 1;
                            IsLure = setting[2] == 1;
                            for (int i = 0; i < 12; i++)
                            {
                                F[i] = setting[i + 3] == 1;
                            }
                            IsPet = setting[15] == 1;
                            IsHP = setting[16] == 1;
                            IsMP = setting[17] == 1;
                            IsNM = setting[19] == 1;
                            for (int i = 0; i < 10; i++)
                                Alt[i] = setting[20 + i] == 1;
                            for (int i = 0; i < 22; i++)
                                KeyDelay[i] = setting[30 + i];
                            BuffPetPercent = setting[52];
                            if (setting.Length > 53)
                                IsPickItem = setting[53] == 1;
                            if (setting.Length > 59)
                            {
                                CheLoai = setting[54];
                                CheCap = setting[55];
                                CheNoiNgoai = setting[56];
                                CheSao = setting[57];
                                CheDong = setting[58];
                                CheDiem = setting[59];
                            }
                            if (setting.Length > 60)
                                IsXuatPet = setting[60] == 1;
                            if (setting.Length > 61)
                                IsAutoComeBack = setting[61] == 1;
                            if (setting.Length > 62)
                                IsQuyCoc = setting[62] == 1;
                            IsNhatHop = setting[65] == 1;
                            IsOnlyPick = setting[66] == 1;
                            IsThuHoachHoaEx = setting[67] == 1;
                            IsRao = setting[68] == 1;
                            if (setting[69] == 1)
                                PushMissions(MissionsType.NhiemVuThangCap);
                            if (setting[70] == 1)
                                PushMissions(MissionsType.TrungAc);
                            IsX4 = setting[71] == 1;
                            RadiusX = setting[72];
                            RadiusY = setting[73];
                            RadiusMap = (uint)setting[74];
                        }
                        catch { }
                    }
                }
                catch
                {
                }
                for (int i = 0; i < KeyDelay.Length; i++)
                {
                    if (KeyDelay[i] == 0)
                        KeyDelay[i] = 1;
                }
                swStandTime = Stopwatch.StartNew();
            }
            catch
            {
                //Main.TDTLog(ex.Message);
            }
            IsAuto = true;
            SkillLoaded?.Invoke(this, null);
        }



        public void DoiHoaHong()
        {
            if (PacketItems.All.Where(i => i.ClearName.Contains("hoahongbatu")).Select(i => (int)i.Count).Sum() < 70)
            {
                RemoveMission(MissionsType.Doi999HoaHong);
                return;
            }
            if (GoToEx(DAILY.BaCaiLy))
            {
                if (TLBB.IsQuestOpen)
                {
                    foreach (QuestFrame quest in QuestFrame.Enum(this))
                    {
                        if (quest.Name.Contains("#{SDJZH_xml_XX(02)}"))
                        {
                            QuestFrameOptionClicked(quest);
                            //QuestFrame.Click(quest);
                            return;
                        }
                        if (quest.Name.Contains("#{SDJZH_20100823_2}"))
                        {
                            QuestFrameOptionClicked(quest);
                            //QuestFrame.Click(quest);
                            Thread.Sleep(300);
                            LUA.QuestFrameMissionComplete(0);
                            return;
                        }
                        QuestFrame.Close();
                    }
                }
                else
                {
                    Talk(DAILY.BaCaiLy);
                }
            }
        }


        public int TamPhapNangToi { get; set; }


        public void NangTamPhap()
        {
            if (GoToEx(TLBB.NPCTamPhap))
            {
                Talk(TLBB.NPCTamPhap);
                Thread.Sleep(1000);
                if (TLBB.IsQuestOpen)
                {
                    // Thread.Sleep(1000);Học kỹ năng
                    //#{XMPTM_130123_126}
                    QuestFrame.Click("Học kỹ năng");
                    //#{XMPTM_130123_126}
                    QuestFrame.Click("{XMPTM_130123_126}");
                }
                Thread.Sleep(1000);
                if (Missions.Contains(MissionsType.Quyen3Len30))
                {
                    //DoStringEx("xinfaidx = -1; setmetatable(_G, { __index = ActionSkillsStudy_Env}); if this:IsVisible() then xinfo = " + 30 + ";" + " for i = 2, 2 do  local theAction = EnumAction(i, 'xinfa'); local nXinfaId = LifeAbility:GetLifeAbility_Number(theAction:GetID()); local nLevel = Player:GetXinfaInfo(nXinfaId, 'level'); if nLevel < xinfo then  xinfaidx = i; end end end");
                    //Thread.Sleep(350);
                    //DoStringEx("if xinfaidx ~= - 1 then setmetatable(_G, { __index = ActionSkillsStudy_Env}); if this:IsVisible() then setmetatable(_G, { __index = ActionSkillsStudy_Env}); ActionSkillsStudy_Xinfa_Clicked(xinfaidx + 1); ActionSkillsStudy_UpLevel_Clicked(); end end");
                    //Thread.Sleep(350);
                    for (int i = 0; i < 200; i++)
                    {
                        string CodeLearn = @"i = 2
                                            g_XinfaDefineID = {}
                                            nActionIndex = 0;
                                            while i<=6  do
                                                theAction = EnumAction(nActionIndex, 'xinfa')
                                                if (theAction:GetID() == 0) then
                                                    g_XinfaDefineID[i] = -1
                                                else
                                                    if i == 4 then
                                                        g_XinfaDefineID[i] = theAction:GetDefineID()
                                                        nXinfaId = LifeAbility:GetLifeAbility_Number(theAction:GetID())
                                                        nLevel = Player:GetXinfaInfo(nXinfaId, 'level')
                                                        if nLevel < " + 30 + @" then
	                                                        SkillsStudyFrame_study( g_XinfaDefineID[i] )
                                                            PushDebugMessage('Đã Học Quyển ' .. i .. ', Refresh để dừng')
	                                                        return 'learn'
                                                        end
                                                    end
                                                end
                                                i = i+1
                                                nActionIndex = nActionIndex + 1

                                            end
                                            return 'done'";
                        DoStringEx(CodeLearn);
                        LuaToString();
                        Thread.Sleep(500);
                        if (LuaToString() == "done")
                        {
                            RemoveMission(MissionsType.NangTamPhap);
                            PushDebugMessage("Học xong rồi nhé");
                            return;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < 1000; i++)
                    {
                        string CodeLearn = @"i = 1
                                            g_XinfaDefineID = {}
                                            nActionIndex = 0;
                                            while i<= 6 do
                                                theAction = EnumAction(nActionIndex, 'xinfa')
                                                if (theAction:GetID() == 0) then
                                                    g_XinfaDefineID[i] = -1
                                                else
                                                    g_XinfaDefineID[i] = theAction:GetDefineID()
                                                    nXinfaId = LifeAbility:GetLifeAbility_Number(theAction:GetID())
                                                    nLevel = Player:GetXinfaInfo(nXinfaId, 'level')
                                                    if nLevel < " + TamPhapNangToi + @" then
	                                                    SkillsStudyFrame_study( g_XinfaDefineID[i] )
                                                        PushDebugMessage('Đã Học Quyển ' .. i .. ', Refresh để dừng')
	                                                    return 'learn'
                                                    end
                                                end
                                                i = i+1
                                                nActionIndex = nActionIndex + 1

                                            end
                                            return 'done'";
                        DoStringEx(CodeLearn);
                        LuaToString();
                        Thread.Sleep(500);
                        if (LuaToString() == "done")
                        {
                            RemoveMission(MissionsType.NangTamPhap);
                            PushDebugMessage("Học xong rồi nhé");
                            return;
                        }
                    }
                    //LuaDoOneLineString("xinfaidx = -1; setmetatable(_G, { __index = ActionSkillsStudy_Env}); if this:IsVisible() then xinfo = " + TamPhapNangToi + ";" + " for i = 0, 5 do  local theAction = EnumAction(i, 'xinfa'); local nXinfaId = LifeAbility:GetLifeAbility_Number(theAction:GetID()); local nLevel = Player:GetXinfaInfo(nXinfaId, 'level'); if nLevel < xinfo then  xinfaidx = i; end end end");
                    //Thread.Sleep(350);
                    //LuaDoOneLineString("if xinfaidx ~= - 1 then setmetatable(_G, { __index = ActionSkillsStudy_Env}); if this:IsVisible() then setmetatable(_G, { __index = ActionSkillsStudy_Env}); ActionSkillsStudy_Xinfa_Clicked(xinfaidx + 1); ActionSkillsStudy_UpLevel_Clicked(); end end");
                    //Thread.Sleep(350);
                }
                RemoveMission(MissionsType.NangTamPhap);
            }
        }

        public static IniParser IniParser { get; set; } = IniParser.Load("nier.ini");

        public void SaveSetting()
        {

            string allow;
            if (LastAllowName != null)
                allow = LastAllowName;
            else
                return;

            string mission = "";
            foreach (var miss in Missions.ToList())
            {
                mission += (int)miss + ",";
            }
            IniParser.Write("Mission", allow, mission);

            IniParser.Write("Talk", allow, RaoTxt);
            //TDT.WriteFile(Global.RaoPath + "\\" + TLBB.Id + ".txt", RaoTxt);
            //Setting.SaveSettingOffline(TLBB.Id + "RAO", );
            string setting = TDT.Bool2Int(IsAuto) + "," + TDT.Bool2Int(IsAttack) + "," + TDT.Bool2Int(IsLure) + ",";
            for (int i = 0; i < 12; i++)
                setting += TDT.Bool2Int(F[i]) + ",";
            setting += TDT.Bool2Int(IsPet) + "," + TDT.Bool2Int(IsHP) + "," + TDT.Bool2Int(IsMP) + "," + TDT.Bool2Int(false) + "," + TDT.Bool2Int(IsNM) + ",";
            for (int i = 0; i < 10; i++)
                setting += TDT.Bool2Int(Alt[i]) + ",";
            for (int i = 0; i < 22; i++)
                setting += KeyDelay[i] + ",";
            setting += BuffPetPercent + "," + TDT.Bool2Int(IsPickItem) + ",";
            setting += CheLoai + "," + CheCap + "," + CheNoiNgoai + "," + CheSao + "," + CheDong + "," + CheDiem + ",";
            setting += TDT.Bool2Int(IsXuatPet) + ",";
            setting += TDT.Bool2Int(IsAutoComeBack) + ",";
            setting += TDT.Bool2Int(IsQuyCoc) + "," + TDT.Bool2Int(IsAuto) + "," + TDT.Bool2Int(false);
            setting += "," + TDT.Bool2Int(IsNhatHop)
                + "," + TDT.Bool2Int(IsOnlyPick) + "," + TDT.Bool2Int(IsThuHoachHoaEx) + "," + TDT.Bool2Int(IsRao) + "," + Missions.Contains(MissionsType.NhiemVuThangCap).ToInt() + "," + Missions.Contains(MissionsType.TrungAc).ToInt() + "," + IsX4.ToInt()
                + "," + RadiusX
                + "," + RadiusY
                + "," + RadiusMap


                ;

            IniParser.Write(allow, "Config", setting);
            IniParser.Write(allow, "Radius", RadiusX + "," + RadiusY + "," + RadiusMap);

            string sk = "";

            foreach(var skill in Skills.Where(i => i.Use))
            {
                sk += "[" + skill.PacketId + "]";
            }

            IniParser.Write(allow, "Skill", sk);

            sk = "";

            foreach (var skill in Skills.Where(i => i.UsePK))
            {
                sk += "[" + skill.PacketId + "]";
            }

            IniParser.Write(allow, "SkillPK", sk);

        }

        public string PetId
        {
            get
            {
                return IniParser.Read("Pet", TLBB.AllowName);
            }
            set
            {
                IniParser.Write("Pet", TLBB.AllowName, value);
            }
        }

        public string RecvDat
        {
            get;
            set;
        }

        public string ReadRecvData()
        {
            int address = (int)Memory.Read(RecvData);
            if (address == 0)
                return "";
            string hex = "";
            bool Khac0 = false;
            byte[] buff = new byte[100];
            Memory.ReadProcessMemory(Memory.Id, address, buff, 100, 0);
            for (int i = 0; i < 100; i++)
            {
                //int h = Memory.Read(address + i);
                //if (h != 0)
                //    Khac0 = true;
                //hex += Memory.ReverseString(h.ToString("X8"));
                string h = buff[i].ToString("X2");
                if (h != "00")
                    Khac0 = true;
                hex += h;
            }
            if (Khac0)
                RecvDat = hex;
            return hex;
        }

        public string GetTrieuTap(string hex)
        {
            string point = "";
            if (!hex.Contains("746965756461747461692067") || !hex.StartsWith("DA03"))
            {
                return "";
            }
            else
            {
                string toado = Regex.Replace(hex, ".*D569205B", "");
                toado = Regex.Replace(toado, "5D.*", "");
                for (int i = 0; i < toado.Length / 2; i++)
                {
                    try
                    {
                        point += (char)Int16.Parse(toado.Substring(i * 2, 2), NumberStyles.AllowHexSpecifier);
                    }
                    catch
                    {
                        point += ".";
                    }
                }
            }
            return point;
        }

        //public bool ReadRecv()
        //{
        //RecvData = Memory.VirtualAllocEx(1024);
        //PostMessage(RecvData, -3);
        ////if (RecvSize == 0)
        ////{
        ////    RecvData = Memory.VirtualAllocEx(1024);
        ////    PostMessage(RecvData, -4);
        ////}
        //int address = Memory.Scan("53 56 57 58", "tieudattai.dll");
        //if (address != 0)
        //{
        //    MyRecvAddress = address;
        //    Memory.Write(MyRecvAddress, 0x90909090, 3);
        //}
        //else
        //{
        //    address = Memory.Scan("90 90 90 58 61 9D", "tieudattai.dll");
        //    if (address != 0)
        //    {
        //        MyRecvAddress = address;
        //        Memory.Write(MyRecvAddress, 0x90909090, 3);
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
        //RecvAddress = Memory.Scan("8B 44 24 10 8B 4C 24 0C 8B 54 24 08", 1);
        //if (RecvAddress != 0)
        //{
        //    Memory.VirtualProtect(RecvAddress, 24, 0x40, 0);
        //    PostMessage(RecvAddress + 0xC, -2);
        //    Memory.Write(RecvAddress, 0x006A609C, 4);
        //    Memory.Write(RecvAddress + 4, 0x68, 1);
        //    Memory.Write(RecvAddress + 5, (uint)MyRecvAddress, 4);
        //    Memory.Write(RecvAddress + 9, 0x9090C3, 3);
        //}
        //Memory.FlushInstructionCache(ProcessId, 0, 0);
        //return true;
        //}

        public int MapAcTac { get; set; }

        public int MapTangKinhCac { get; set; }

 

 
        public void ResetExpSpeed(bool force = false)
        {
            if (TLBB.Lvl < 1 || TLBB.Lvl > 149)
                return;
            if (ExpStart > TLBB.Exp || ExpStart <= 0 || force)
            {
                AutoTime = Stopwatch.StartNew();
                ExpStart = (int)TLBB.Exp;
            }
        }

        public uint RadiusMap { get; set; }

    
        private Alam alarmCobanFail;
        private Alam alarmHP;
        private Alam alarmPk;
        private Alam alarmDead;
        private Alam alarmDisconnected;
        private Alam alarmXongChinhTuyen;

        private bool isAlarmHP = false;
        private bool isAlarmPK = false;
        private bool isAlarmDead = false;
        private bool isAlarmCaptcha = false;
        private int disconnectedTime = 0;
        private bool isAlarmBachHoaDuyenCompleted = false;
        public bool isAlarmKet = false;
        public bool isAlarmHong = false;

        private uint AddressToString;
        private uint AddressTenBang;

        public string LuaToString(bool islong = false)
        {
            //lecaotri2020
            // return "Kiem tra";

            if (AddressToString == 0)
                AddressToString = Memory.VirtualAllocEx(20248);
            PostMessage(AddressToString, 109);//luatostring
            if (islong)
                return Memory.ReadStringEx((int)Memory.Read(AddressToString));
            return Memory.ReadString(Memory.Read(AddressToString));
        }


        private uint AddressToStringBanBe;

        public string LuaToStringBanBe(bool islong = false)
        {
            //lecaotri2020
            // return "Kiem tra";

            if (AddressToStringBanBe == 0)
                AddressToStringBanBe = Memory.VirtualAllocEx(20248);
            PostMessage(AddressToStringBanBe, 109);//luatostring
            if (islong)
                return Memory.ReadStringEx((int)Memory.Read(AddressToStringBanBe));
            return Memory.ReadString(Memory.Read(AddressToStringBanBe));
        }

        private uint AddressToStringThread;

        public string LuaToStringThread(bool islong = false)
        {
            //lecaotri2020
            // return "Kiem tra";

            if (AddressToStringThread == 0)
                AddressToStringThread = Memory.VirtualAllocEx(20248);
            PostMessage(AddressToStringThread, 109);//luatostring
            if (islong)
                return Memory.ReadStringEx((int)Memory.Read(AddressToStringThread));
            return Memory.ReadString(Memory.Read(AddressToStringThread));
        }

        public List<Game> QuanDoanParty1 { get; set; } = new List<Game>();
        public List<Game> QuanDoanParty2 { get; set; } = new List<Game>();

   

        Dictionary<string, uint> dicAdressMemo = new Dictionary<string, uint>();

        public string LuaToStringMemo(string memo = "")
        {
            lock (LockLua)
            {
                if (!dicAdressMemo.ContainsKey(memo))
                {
                    dicAdressMemo.Add(memo, Memory.VirtualAllocEx(0x1000));
                }
                PostMessage(dicAdressMemo[memo], 109);
                return Memory.ReadString((int)Memory.Read(dicAdressMemo[memo]));
            }
        }

        private uint AddressFriend;

        public string LuaToStringFriend()
        {
            if (AddressFriend == 0)
            {
                AddressFriend = Memory.VirtualAllocEx(0x1000);
            }
            PostMessage(AddressFriend, 109);//luatostring
            return Memory.ReadStringWithLength((int)Memory.Read(AddressFriend), 0x1000);
        }

    

        private uint AdrMemInfo;

        public string LuaToStringMemInfo()
        {
            if (AdrMemInfo == 0)
            {
                AdrMemInfo = Memory.VirtualAllocEx(0x1000);
                return "";
            }
            else
            {
                PostMessage(AdrMemInfo, 109);//luatostring
                return Memory.ReadString(Memory.Read(AdrMemInfo));
            }
        }

        public string LuaToStringBang()
        {
            PostMessage(AddressTenBang, 109);//luatostring
            return Memory.ReadString(Memory.Read(AddressTenBang));
        }


      



        public string LuaString()
        {
            if (AddressToString == 0)
                AddressToString = Memory.VirtualAllocEx(20248);
            return Memory.ReadString(Memory.Read(AddressToString));
        }

        public string LuaStringBang()
        {
            return Memory.ReadString(Memory.Read(AddressTenBang));
        }

        public void EnterReconnect()
        {
            PostMessage(24, 105);
        }




        private bool IsShowCap { get; set; }
        public Stopwatch swCaptchaTime { get; set; }

        public void PushAlarm(string msg)
        {
            _i.Alan.Instance.Controls.Add(new Alam(this, msg, 0));
        }

        public bool IsAlarmLoiNhiemVu { get; set; }

        private bool isAlarmHuyetMo { get; set; }
        private bool isAlarmDayTayNai { get; set; }

        private bool IsDeadEx
        {
            get
            {
                if (Missions.Contains(MissionsType.TrungAc))
                    return true;
                if (Missions.Contains(MissionsType.BachHoaDuyen))
                    return true;
                if (Missions.Contains(MissionsType.NhiemVuSuMon))
                    return true;
                if (TLBB.MapId == MAP.ThieuThatSon)
                    return false;
                if (TLBB.MapId == MAP.LoiDaiSinhTu)
                    return false;
                if (TLBB.MapId == MAP.BinhThanhKyTran)
                    return false;
                if (TLBB.MapId == MAP.TuTuyetTrang)
                    return false;
                return Setting.Is("checkDauThai");
            }
        }



        public void LongPhuMau()
        {
            if (!IsOneSec)
                return;
            if (QuestFrame.All(this).Contains("#{CZRW_130822_144}"))
            {
                PushDebugMessage("Xong Q lòng phụ mẫu");
                RemoveMission(MissionsType.LongPhuMau);
                return;
            }
            if (TLBB.PlayerState != 0)
                return;
            if (IdleTime > 12)
            {
                MissionState = "";
                IdleTime = 0;
                return;
            }
            if (MissionState == "")
            {
                if (!TLBB.IsTogleMission)
                {
                    TogleMission();
                    MissionState = "OpenMission";
                    return;
                }
                else
                {
                    MissionState = "OpenMission";
                    return;
                }
            }
            if (MissionState == "OpenMission")
            {
                TogleMission();
                MissionState = "CloseMission";
                return;
            }
            if (MissionState == "CloseMission")
            {
                DoStringEx("local cnt = 0; while true do local name, content = DataPool:GetPlayerMission_Memo(cnt); if string.find(name, 'Lòng phø mçu') then if DataPool:GetPlayerMission_Variable(cnt, 0) == 1 then if string.find(QuestLog_GetInfantTarget(cnt), 'Trß·ng Thành') then return 'XongPet' .. QuestLog_GetInfantTarget(cnt) end return 'Xong'; else return QuestLog_GetInfantTarget(cnt); end end cnt = cnt + 1; if cnt == 20 then return 'Chua'; end end");
                MissionState = "GetPhuMauInfo";
                LuaToString();
                return;
            }
            if (MissionState == "GetPhuMauInfo")
            {
                MissionX = MissionY = 0;
                MissionInfo = LuaString();
                if (MissionInfo == "Chua")
                {
                    MissionState = "Chua";
                    return;
                }
                if (MissionInfo == "Xong")
                {
                    MissionState = "Xong";
                    return;
                }
                if (MissionInfo.StartsWith("XongPet"))
                {
                    MissionState = MissionInfo;
                    return;
                }
                MissionMap = TDT.GetMapId(TDT.VietLien(MissionInfo));
                if (TDT.VietLien(MissionInfo).Contains("truongthanh"))
                    MissionMap = MAP.HuyenVuDao;
                foreach (string info in MissionInfo.Split('*'))
                {
                    if (MissionX != 0)
                    {
                        MissionState = "Do";
                        MissionY = TDT.ParseInt(info);
                        if (MissionY <= 500)
                            return;
                    }
                    MissionX = TDT.ParseInt(info);
                    if (MissionX > 500)
                        MissionX = 0;
                }
                if (MissionX == 0 && MissionY == 0)
                {
                    MissionState = "Mua";
                }
                return;
            }
            if (MissionState == "Chua")
            {
                if (GoToEx(LACDUONG.MocTuTyTy))
                {
                    if (TLBB.IsQuestOpen)
                    {
                        QuestFrame.Click(890500, 10);
                        QuestFrame.Close();
                        MissionState = "";
                    }
                    else
                    {
                        Talk(LACDUONG.MocTuTyTy);
                        return;
                    }
                }
                return;
            }
            if (MissionState.StartsWith("XongPet"))
            {
                if (GoToEx(LACDUONG.MocTuTyTy))
                {
                    if (TLBB.IsQuestOpen)
                    {
                        QuestFrame.Click(890500, 10);
                        MissionState = "TraPet";
                    }
                    else
                    {
                        Talk(LACDUONG.MocTuTyTy);
                        return;
                    }
                }
                return;
            }
            if (MissionState == "TraPet")
            {
                int index = 0;
                int cnt = -1;
                foreach (char s in MissionInfo)
                {
                    cnt++;
                    if ((int)s < 32)
                        index = cnt;
                }
                if (index != 0)
                {
                    string petname = MissionInfo.Substring(index + 1).Trim('}');

                    DoStringEx("PETNAME = '" + petname + "'");
                    Thread.Sleep(450);
                    DoStringEx("local index = -1; for i = 1, 10 do local szPetName,szOn = Pet:GetPetList_Appoint(i-1); if szPetName ~= '' then local name = Pet:GetName(i - 1); if string.find(name, ' Tr') and string.find(name, 'ng Th') then index = i; end if string.find(PETNAME, name) then Exchange:AddPet(i - 1); i = -1; break; end end end if index ~= -1 then Exchange:AddPet(index - 1); end");
                    Thread.Sleep(450);
                    DoStringEx("setmetatable(_G, {__index = MissionReply_Env}); MissionReply_Accept_Clicked();");
                    MissionState = "";
                }
                QuestFrame.Close();
                MissionState = "";
                return;
            }
            if (MissionState == "Xong")
            {
                if (GoToEx(LACDUONG.MocTuTyTy))
                {
                    if (TLBB.IsQuestOpen)
                    {
                        QuestFrame.Click(890500, 10);
                        Thread.Sleep(300);
                        LUA.QuestFrameMissionContinue();
                        Thread.Sleep(300);
                        LUA.QuestFrameMissionComplete();
                        QuestFrame.Close();
                        MissionState = "";
                    }
                    else
                    {
                        Talk(LACDUONG.MocTuTyTy);
                        return;
                    }
                }
                return;
            }
            if (MissionState == "Mua")
            {
                NPC npc = LACDUONG.BachManhSinh;
                if (TDT.VietLien(MissionInfo).Contains("mienbo") || TDT.VietLien(MissionInfo).Contains("yeudai") || TDT.VietLien(MissionInfo).Contains("bomao"))
                {
                    npc = LACDUONG.DaoDacBao;
                }
                if (GoToEx(npc))
                {
                    if (TLBB.IsShopOpen)
                    {
                        foreach (var item in PacketItems.Shop)
                        {
                            if (TDT.VietLien(MissionInfo).Contains(TDT.VietLien(item.Name)) && !(TDT.VietLien(MissionInfo).Contains("mienbo") && !item.ClearName.Contains("mienbo")))
                            {
                                Buy(item.Index);
                                MissionState = "";
                                return;
                            }
                        }
                    }
                    else
                    {
                        Talk(npc);
                    }
                }
                return;
            }
            if (MissionState == "Do")
            {
                if (MissionMap == MAP.HuyenVuDao)
                {
                    if (TLBB.MapId == MAP.HuyenVuDao)
                    {
                        if (TDT.GetDistance(RoundX, RoundY, MissionX, MissionY) > 10)
                        {
                            GoToEx(MissionX, MissionY);
                        }
                        else
                        {
                            if (TLBB.IsRide)
                            {
                                DownRide();
                                return;
                            }
                            foreach (GameObject _object in Objects.All)
                            {
                                if (TDT.Contain(MissionInfo, _object.Name) && _object.Menpai == 0)
                                {
                                    UseSkill(1, _object.Id);
                                    MissionState = "";
                                    return;
                                }
                            }
                            ForceAttack();
                        }
                    }
                    else
                    {
                        GoToEx(MissionX, MissionY, MissionMap);
                    }
                    return;
                }
                if (GoToEx(MissionX, MissionY, MissionMap))
                {
                    if (MissionMap > 2)
                    {
                        if (TLBB.IsRide)
                        {
                            DownRide();
                            return;
                        }
                        foreach (GameObject _object in Objects.All)
                        {
                            if (TDT.VietLien(MissionInfo).Contains(TDT.VietLien(_object.Name)))
                            {
                                PickItem((int)_object.Id);
                                MissionState = "";
                            }
                        }
                    }
                    else
                    {
                        if (TLBB.IsQuestOpen)
                        {
                            QuestFrame.Click("longphumau");
                            QuestFrame.Close();
                            MissionState = "";
                        }
                        foreach (GameObject _object in Objects.All)
                        {
                            if (TDT.VietLien(MissionInfo).Contains(TDT.VietLien(_object.Name)))
                            {
                                Talk(_object.Id);
                                return;
                            }
                        }
                        foreach (var item in PacketItems.All)
                        {
                            if (item.ClearName == "phihoaluutinh" || item.ClearName == "dieugiay" || item.ClearName == "kienkhangthu" || item.ClearName == "xengnho")
                            {
                                if (TLBB.IsRide)
                                {
                                    DownRide();
                                }
                                else
                                {
                                    item.DoAction();
                                    MissionState = "";
                                }
                            }
                        }
                    }
                }
                return;
            }
            MissionState = "";
        }


        public void Alarm()
        {
            if (!IsOneSec)
                return;
            try
            {
                if (IsXongBHD)
                {
                    if (!TLBB.IsCaptcha)
                    {
                        if (!isAlarmBachHoaDuyenCompleted)
                        {
                            Missions.Clear();
                            isAlarmBachHoaDuyenCompleted = true;
                            if (AccountEx != null)
                            {
                                AccountEx.Status = "xong BHD";
                                AccountEx.game = null;
                            }
                            if (!MicroLogin.IsNotOut)
                            {
                                Main.PushLog(DateTime.Now.ToString("HH:mm dd-MM ") + "Xong " + LastName);
                            }
                            if (!MicroLogin.IsNotOut)
                                Quit();
                        }
                    }
                }
                else
                {
                    isAlarmBachHoaDuyenCompleted = false;
                }
            }
            catch
            {
            }

            if (TLBB.MapId == MAP.HuyetMo && !isAlarmHuyetMo)
            {
                if (!IsQuangCao)
                {
                    DoStringEx("TXT = '#cFF0000 tieudattai#cffcccc với #GMicroAuto#cffcccc giúp ta hoàn vào#cFF0000 Huyệt Mộ#cffcccc khi mở BTD, quả là auto hoàn hảo';");
                    PostMessage(3, 105);
                }
                isAlarmHuyetMo = true;
                Main.Instance.Invoke(() =>
                {
                    PushAlarm("đã vào huyệt mộ");
                });
            }

            if (TLBB.IsPk && TLBB.MapId != 92)
            {
                if (Global.ExitPk)
                {
                    Exit();
                    return;
                }
                if (!isAlarmPK && Global.AlarmPk)
                {
                    isAlarmPK = true;
                    Main.Instance.Invoke(new Action(() => { alarmPk = new Alam(this, "bị " + NamePk + " PK", 0); }));
                    Alan.Push(alarmPk);
                }
            }
            else
            {
                if (isAlarmPK)
                {
                    isAlarmPK = false;
                    if (alarmPk != null && !alarmPk.IsDisposed)
                        Main.Instance.Invoke(new Action(() => { alarmPk.Dispose(); }));
                }
            }

            if (Missions.Contains(MissionsType.BachHoaDuyen))
            {
                if (TLBB.IsODaoCuFull)
                {
                    if (!isAlarmDayTayNai)
                    {
                        isAlarmDayTayNai = true;
                        PushAlarm("đầy tay nải");
                    }
                }
                else
                {
                    isAlarmDayTayNai = false;
                }
            }

            if (CoBanFailTime.IsRunning)
            {
                if (TLBB.Lvl > 30)
                {
                    if (CoBanFailTime.Elapsed.TotalSeconds > 60)
                    {
                        if (!IsAlarmLoiNhiemVu)
                        {
                            Main.Instance.Invoke(new Action(() => { alarmCobanFail = new Alam(this, "lỗi nhiệm vụ", 10); }));
                            Alan.Push(alarmCobanFail);
                            IsAlarmLoiNhiemVu = true;
                        }
                    }
                }
            }
            else
            {
                if (alarmCobanFail != null)
                {
                    Main.Instance.Invoke(new Action(() => { alarmCobanFail.Dispose(); }));
                    alarmCobanFail = null;
                }
            }

            if (TLBB.Disconnected)
            {
                if (disconnectedTime++ == 3)
                {
                    Main.Instance.Invoke(new Action(() => { alarmDisconnected = new Alam(this, "bị mất kết nối", 60); }));
                    Alan.Push(alarmDisconnected);
                }
                if (disconnectedTime == 5)
                    EnterReconnect();
            }
            else
            {
                if (disconnectedTime != 0)
                {
                    disconnectedTime = 0;
                    if (alarmDisconnected != null && !alarmDisconnected.IsDisposed)
                        Main.Instance.Invoke(new Action(() => { alarmDisconnected.Dispose(); }));
                }
            }

            if (TLBB.IsCaptcha)
            {
                if (Address.GameType == 1)
                {
                    if (!IsShowCap)
                    {
                        IsShowCap = true;
                        try
                        {
                            Main.Instance.Invoke(new Action(() =>
                            {
                                EnterCaptchaContainer.Instance.Controls.Add(new EnterCaptchaEx(this) { Location = new Point(11, 0) });
                                EnterCaptchaContainer.Instance.Show();
                                EnterCaptchaContainer.Instance.SetTop();
                            }));
                        }
                        catch { }
                    }
                }
                if (!isAlarmCaptcha)
                {
                    DoStringEx("setmetatable(_G, {__index = AntiRobot_Env }); AntiRobot_SelAnswer(1);");
                    isAlarmCaptcha = true;
                }
            }
            else
            {
                swCaptchaTime = null;
                IsShowCap = false;
                TLBB.Captcha = null;
                TLBB.BinEx = null;
                TLBB.BaseImg = 0;
                if (isAlarmCaptcha)
                {
                    isAlarmCaptcha = false;
                }
            }

            if (TLBB.PlayerState == 9)
            {
                if (Setting.Is("checkHoiSinh"))
                {
                    LUA.Relive();
                }
                if (IsDeadEx || isdauthai)
                {
                    if (!isAlarmDead)
                    {
                        if (IsAutoComeBack)
                        {
                            DeadX = (int)CharX;
                            DeadY = (int)CharY;
                            DeadMap = (int)TLBB.MapId;
                            DeadFakeMap = (int)TLBB.FakeMapId;
                            IsDead = true;
                        }
                        else
                        {
                            DeadX = DeadY = 0;
                        }
                        isAlarmDead = true;
                    }
                    isdauthai = false;
                    LUA.OutGhost();
                }
                else
                {
                    if (!isAlarmDead)
                    {
                        if (IsAutoComeBack)
                        {
                            DeadX = (int)CharX;
                            DeadY = (int)CharY;
                            DeadMap = (int)TLBB.MapId;
                            DeadFakeMap = (int)TLBB.FakeMapId;
                            IsDead = true;
                        }
                        else
                        {
                            DeadX = DeadY = 0;
                        }
                        PostMessage(0, 90);
                        isAlarmDead = true;
                        if (!IsAutoComeBack && !IsMapPhuBan() && Main.IsExitDead)
                            Main.Instance.Invoke(new Action(() => { alarmDead = new Alam(this, "đã tử vong", 270); }));
                        else
                            Main.Instance.Invoke(new Action(() => { alarmDead = new Alam(this, "đã tử vong", 0); }));
                        Alan.Push(alarmDead);
                        if (alarmHP != null && !alarmHP.IsDisposed)
                            Main.Instance.Invoke(new Action(() => { alarmHP.Dispose(); }));
                    }
                }
            }
            else
            {
                isdauthai = false;
                if (isAlarmDead)
                {
                    isAlarmDead = false;
                    if (alarmDead != null && !alarmDead.IsDisposed)
                        Main.Instance.Invoke(new Action(() => { alarmDead.Dispose(); }));
                }
            }
            if (NeedAlarmTueHong == true)
            {
                NeedAlarmTueHong = false;
                Main.Instance.Invoke(new Action(() => { Alan.Push(new Alam(this, "xong tuế hồng", 0)); }));
            }

            if (RemConfig.IsAlarmHP && TLBB.HPPercent < RemConfig.PercentAlarmHP && TLBB.Online && TLBB.HP > 0 && TLBB.MaxHP > 0 && TLBB.HP < TLBB.MaxHP)
            {
                if (!isAlarmHP)
                {
                    isAlarmHP = true;
                    Main.Instance.Invoke(new Action(() => { alarmHP = new Alam(this, "sắp hết máu", 0); }));
                    Alan.Push(alarmHP);
                }
            }
            else
            {
                if (isAlarmHP)
                {
                    isAlarmHP = false;
                    if (alarmHP != null && !alarmHP.IsDisposed)
                        Main.Instance.Invoke(new Action(() => { alarmHP.Dispose(); }));
                }
            }
        }

        public void TamKy()
        {
            if (!Global.IsTamKy)
            {
                RemoveMission(MissionsType.TamKy);
                return;
            }
            if (TLBB.PlayerState == 5)
                return;
            if(TLBB.MapId != MAP.QuyThi)
            {
                GoToEx(0, 0, MAP.QuyThi);
                return;
            }
            if (string.IsNullOrEmpty(MissionState))
            {
    //            Clear_XSCRIPT()

    //    Set_XSCRIPT_Function_Name("SaleXiansuo")

    //    Set_XSCRIPT_ScriptID(893197)

    //    Set_XSCRIPT_Parameter(0, clueIndex)

    //    Set_XSCRIPT_Parameter(1, 1)

    //    Set_XSCRIPT_ParamCount(2)

    //Send_XSCRIPT()

                DoStringEx(@"maxNum = GuiShiUI:LuaFnGetClueCount()
                            for index = 1, maxNum do
                                clueID = GuiShiUI:LuaFnGetClueID(index - 1)
	                            if nil ~= clueID and clueID > 0 then
		                            clueID, clueName, clueDesc, clueQual, clueFunc, vilaidType, viladTime = GuiShiUI:LuaFnGetXianSuoDataFromTable(clueID)		                            
    	                            if clueName == 'Tầm Kỳ' then
	                                    Clear_XSCRIPT()
	                                    Set_XSCRIPT_Function_Name('ClickXianSuoGoToButton')
	                                    Set_XSCRIPT_ScriptID( 893194 )
	                                    Set_XSCRIPT_Parameter(0,index - 1)
	                                    Set_XSCRIPT_Parameter(1, clueID)
	                                    Set_XSCRIPT_ParamCount(2)
	                                    Send_XSCRIPT()
	                                    return
    	                            end
	                            end
                            end
                            ");
                MissionState = "TamKy";
            }
            if(MissionState == "TamKy")
            {
                if (MissionX == 0)
                {
                    MissionState = string.Empty;
                    return;
                }
                if (GoToEx(MissionX, MissionY))
                {
                    DoStringEx("setmetatable(_G, {__index = GuiShi_ChanXiao_LuoPan_Env }); GuiShi_ChanXiao_LuoPan_OnChuTu()");
                    MissionState = string.Empty;
                }
                return;
            }
            MissionState = string.Empty;
        }

        public void OpenShop()
        {
            if (IsOpenShop && SafeTime >= 2 && IsOneSec && IsOpenPass2)
            {
                IsOpenShop = false;
                DoStringEx("setmetatable(_G, {__index = Packet_Env }); Packet_Sale_Clicked(); IsMessageBox = 1;");
            }
        }

        public bool IsCheDo = false;
        public int CheLoai = 0;
        public string CheTen = string.Empty;
        public int CheCap = 0;
        public int CheNoiNgoai = 1;
        public int CheSao = 6;
        public int CheDong = 5;
        public int CheDiem = 0;

        public int PartyIndex { get; set; }

        public List<string> SavedParty = new List<string>();

        private bool isrunketnghia = false;

        public void KetNghiaByTeam()
        {
            if (!isrunketnghia)
                isrunketnghia = true;
            else
                return;
            foreach (List<string> list in CalendarEx.Tea)
            {
                if (list.Contains(TLBB.Id))
                {
                    List<string> l = Main.DicGame.Where(kvp => kvp.Value != this && list.Contains(kvp.Value.TLBB.Id)).Select(kvp => kvp.Value.TLBB.Name).ToList();
                    foreach (string name in l)
                    {
                        DoStringEx("DataPool: AddFriendAndGrouping('" + name + "');");
                        Thread.Sleep(1000);
                        DoStringEx("setmetatable(_G, { __index = Friend_IMGrouping_Env}); Friend_IMGrouping_OK_Clicked();");
                        Thread.Sleep(1000);
                    }
                    break;
                }
            }
            isrunketnghia = false;
        }

        public IEnumerable<Game> PartyEx
        {
            get
            {
                if (TLBB.KeyId.Contains("00000000") || TLBB.KeyId.Contains("FFFFFFFF"))
                {
                    yield return this;
                }
                else
                {
                    foreach (var game in Main.Instance.AllOnelineGame.Where(o => o.TLBB.KeyId == TLBB.KeyId))
                    {
                        yield return game;
                    }
                }
            }
        }

        public List<Game> Party { get; set; } = new List<Game>();

 

        public void SetPartyIndex()
        {
            if (!TLBB.IsLeader)
                return;
            int cnt = 0;
            foreach (var game in Party)
            {
                game.PartyIndex = cnt++;
            }
        }

        public Game Leader { get; set; }

        public void PushDebugMessage(string msg)
        {
            DoStringEx("PushDebugMessage(\"" + msg + "\");");
        }


        public void PushDebugMessageEx(string msg)
        {
            if (!IsOneSec)
                return;
            DoStringEx("PushDebugMessage(\"" + msg + "\");");
        }

        private Stopwatch swonly;
        //public bool IsOptLocDo = false;

        public bool IsOnlyMoDungBac
        {
            get
            {
                if (swonly != null && swonly.Elapsed.TotalSeconds <= 10)
                    return true;
                foreach (Game game in Party)
                {
                    if (game.Objects.HaveMonter("tieuvienson"))
                        return false;
                }
                foreach (Game game in Party)
                {
                    if (game.Objects.HaveMonter("modungbac"))
                    {
                        swonly = Stopwatch.StartNew();
                        return true;
                    }
                }
                return false;
            }
        }

        public bool IsNhanQuaHoaHong = false;

     


        public bool IsByLogin { get; set; }

     
        internal AccountEx AccountEx { get; set; }


        //public List<int> ListReceives = new List<int>();
        //public Dictionary<int, int> DicReceives { get; set; } = new Dictionary<int, int>();
        public List<int> ListReceives { get; set; } = new List<int>();

        private List<byte[]> Receives = new List<byte[]>();
        public int LastReceivesIndex { get; set; }

        public List<byte[]> ListRecv { get; set; } = new List<byte[]>();
        public int IdPk { get; set; }
        private string NamePk { get; set; }
        public Dictionary<string, DateTime> Enemys { get; set; } = new Dictionary<string, DateTime>();

        //public Dictionary<int, int> tradeted { get; set; } = new Dictionary<int, int>();

        public Stopwatch DiaPhuNguYeu = Stopwatch.StartNew();

        private void ExecRecv()
        {
            HookRecv();
            try
            {
                Receives.Clear();
                List<int> list = ListReceives.ToList();
                ListReceives.Clear();
                while (list.Count > 0)
                {
                    int index = list[0];
                    if (LastReceivesIndex > index)
                    {
                        LastReceivesIndex = 0;
                    }
                    int length = index - LastReceivesIndex;
                    byte[] buffs = new byte[length];

                    Memory.ReadProcessMemory(Memory.Id, (int)ShareAddress + LastReceivesIndex, buffs, length, 0);

                    Receives.Add(buffs);
                    if (Global.IsAdminEx || User.Email == "lecaotri@yahoo.com")
                    {
                        ListRecv.Add(buffs);
                        while (ListRecv.Count > 200)
                            ListRecv.RemoveAt(0);
                    }
                    LastReceivesIndex = index;
                    list.RemoveAt(0);
                }
                foreach (byte[] buff in Receives)
                {
                    if (Address.GameType == 1)
                    {
                        if (buff.Length > 0x20)
                        {
                            //if (buff[0] == 0x8B && buff[1] == 0x2 && buff[2] == 0xF)
                            if (buff[0] == 0x7C && buff[1] == 0x1 && buff[2] == 0x11)
                            {
                                int toId = IdPk = BitConverter.ToInt32(new byte[] { buff[0x12], buff[0x13], 0, 0 }, 0);

                                if (toId == SelfId)
                                {
                                    if (Skill.IsBase(BitConverter.ToInt32(new byte[] { buff[0xE], buff[0xF], 0, 0 }, 0)))
                                    {
                                        IdPk = BitConverter.ToInt32(new byte[] { buff[6], buff[7], 0, 0 }, 0);

                                        foreach (GameObject _object in Objects.All)
                                        {
                                            if (_object.Id == IdPk)
                                            {
                                                if (_object.Name == TLBB.Name)
                                                {
                                                    break;
                                                }
                                                NamePk = _object.Name;
                                                if (Enemys.ContainsKey(NamePk))
                                                {
                                                    Enemys[NamePk] = DateTime.Now;
                                                }
                                                else
                                                {
                                                    Enemys.Add(NamePk, DateTime.Now);
                                                }
                                                break;
                                            }
                                        }
                                        //Main.TDTLog(IdPk.ToString());
                                    }
                                }
                            }
                        }
                    }
                    int isMsg = -1;
                    int isAcBa = -1;
                    //if(buff.Length > 10)
                    //{
                    //    if (buff[0] == 0xDA && buff[1] == 0x03 && buff[6] == 0x03)
                    //    {
                    //        isMsg =0;
                    //    }
                    //    if (buff[0] == 0x1E && buff[1] == 0x02 && buff[6] == 0x03)
                    //    {
                    //        isMsg = 0;
                    //    }
                    //    if (buff[0] == 0xB0 && buff[1] == 0x04 && buff[6] == 0x03)
                    //    {
                    //        IsVuKho = true;
                    //    }
                    //    if (buff[0] == 0xDA && buff[1] == 0x03 && buff[6] == 0x04)
                    //    {
                    //        isAcBa = 0;
                    //    }
                    //}

                    int isTheGioi = -1;
                    int isLatBa = -1;

                    for (int i = 0; i < buff.Length - 10; i++)
                    {
                        if (Address.GameType == 1)
                        {
                            if (buff[i] == 0x8C && buff[i + 1] == 0x02 && buff[i + 6] == 0x03)
                            {
                                isMsg = i;
                                break;
                            }
                            if (!ChatLogs.IsKhongTheGioi)
                            {
                                if (buff[i] == 0xDA && buff[i + 1] == 0x03 && buff[i + 6] == 0x02)
                                {
                                    isTheGioi = i;
                                    break;
                                }
                                if (buff[i] == 0xDA && buff[i + 1] == 0x03 && buff[i + 6] == 0x0A)
                                {
                                    isLatBa = i;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            if (buff[i] == 0x1E && buff[i + 1] == 0x02 && buff[i + 6] == 0x03)
                            {
                                isMsg = i;
                                break;
                            }
                        }
                        if (buff[i] == 0x8C && buff[i + 1] == 0x02 && buff[i + 6] == 0x04)
                        {
                            isAcBa = i;
                            break;
                        }
                    }
                    string msg = ConverterEx.VISCII2UnicodeEx(buff);

                    if (msg.Contains("#{GSCX_190727_291}"))
                    {
                        for(int i = 0; i < buff.Length + 0x40; i++)
                        {
                            if(buff[i] == 0x7B && buff[i+1] == 0x47 && buff[i+2] == 0x53 && buff[i + 3] == 0x43)
                            {
                                try
                                {
                                    MissionX = int.Parse(buff.Skip(i + 0x29 - 4).Take(1).ToArray()[0].ToString("X2"), System.Globalization.NumberStyles.HexNumber);
                                    MissionY = int.Parse(buff.Skip(i + 0x2D - 4).Take(1).ToArray()[0].ToString("X2"), System.Globalization.NumberStyles.HexNumber);
                                    PushDebugMessage("Tầm Kỳ: " + MissionX + "," + MissionY);
                                }
                                catch { }
                                break;
                            }
                        }
                    }

                    if (Missions.Contains(MissionsType.DatDoiBaoDoHiem) && TLBB.IsLeader)
                    {
                        //23 7B 44 54 59 48 5F 31 32 31 30 30 39 5F 30 38 7D
                        byte[] combuff = new byte[] { 0x23, 0x7B, 0x44, 0x54, 0x59, 0x48, 0x5F, 0x31, 0x32, 0x31, 0x30, 0x30, 0x39, 0x5F, 0x30, 0x38, 0x7D };
                        byte[] toado = null;
                        if (msg.Contains("#{DTYH_121009_08}"))
                        {
                            for (int i = 0; i < buff.Length - (17 + 6 * 4 + 6); i++)
                            {
                                bool isequal = true;
                                for (int j = 0; j < combuff.Length; j++)
                                {
                                    if (combuff[j] != buff[i + j])
                                    {
                                        isequal = false;
                                        break;
                                    }
                                }
                                if (isequal)
                                {
                                    toado = buff.Skip(i + 6 + 17).Take(24).ToArray();
                                    break;
                                }
                            }
                            if (toado != null)
                            {
                                float x1 = BitConverter.ToSingle(toado.Skip(0).Take(4).ToArray(), 0);
                                float y1 = BitConverter.ToSingle(toado.Skip(4).Take(4).ToArray(), 0);
                                float x2 = BitConverter.ToSingle(toado.Skip(8).Take(4).ToArray(), 0);
                                float y2 = BitConverter.ToSingle(toado.Skip(12).Take(4).ToArray(), 0);
                                float x3 = BitConverter.ToSingle(toado.Skip(16).Take(4).ToArray(), 0);
                                float y3 = BitConverter.ToSingle(toado.Skip(20).Take(4).ToArray(), 0);
                                if (x1 > 0 && x1 < 800 && y1 > 0 && y1 < 800)
                                {
                                    ListPointBaoDoHiem.Add(Math.Floor(x1) + "," + Math.Floor(y1));
                                }
                                if (x2 > 0 && x2 < 800 && y2 > 0 && y2 < 800)
                                {
                                    ListPointBaoDoHiem.Add(Math.Floor(x2) + "," + Math.Floor(y2));
                                }
                                if (x3 > 0 && x3 < 800 && y3 > 0 && y3 < 800)
                                {
                                    ListPointBaoDoHiem.Add(Math.Floor(x3) + "," + Math.Floor(y3));
                                }
                            }
                        }
                    }

                    if (TLBB.MapId == MAP.TangKinhCac)
                    {
                        string info = msg;
                        if ((info.Contains("#{CJG_090413_21}") || info.Contains("#{CJG_090605_4}")) && TLBB.IsLeader)
                        {
                            Party.ToList().ForEach(g =>
                            {
                                {
                                    if (!IsSameInfo(info, g.TKCInfo))
                                    {
                                        g.TKCInfo = info;
                                        g.TKCStateTime = Stopwatch.StartNew();
                                        if (g.TKCInfo.Contains("1/10") || g.TKCInfo.Contains("3/10") || g.TKCInfo.Contains("5/10") || g.TKCInfo.Contains("7/10") || g.TKCInfo.Contains("9/10"))
                                        {
                                            g.IsTraiTKC = false;
                                        }
                                        if (g.TKCInfo.Contains("2/10") || g.TKCInfo.Contains("4/10") || g.TKCInfo.Contains("6/10") || g.TKCInfo.Contains("8/10") || g.TKCInfo.Contains("10/10"))
                                        {
                                            g.IsTraiTKC = true;
                                        }
                                        if (g.TKCInfo.Contains("10/10"))
                                        {
                                            g.TKCComplete = true;
                                        }
                                    }
                                }
                            });
                        }
                    }
                    if (TLBB.MapId == MAP.BinhThanhKyTran && (Missions.Contains(MissionsType.DatDoiBinhThanh)))
                    {
                        if (msg.Contains("BSQZ_101223_122"))
                        {
                            Party.ForEach(p => { p.DiaPhuNguYeu = Stopwatch.StartNew(); p.safex = p.safey = 0; });
                            QuanDoans.ForEach(p => { p.DiaPhuNguYeu = Stopwatch.StartNew(); p.safex = p.safey = 0; });
                        }
                    }
                    if (Global.IsMaTac)
                    {
                        if (msg.Contains("#{_BOSS48}"))
                        {
                            string map = TDT.StringBetween(msg, "#G", "#P");
                            MapMaTac = TDT.GetMapId(map);
                            if (LastMapMaTac != MapMaTac)
                            {
                                LastMapMaTac = MapMaTac;
                                Main.PushLog(DateTime.Now.ToString("HH:mm dd-MM ") + "Mã Tặc " + " -> " + map + " [" + LastName + "]");
                            }
                            try
                            {
                                if (!dicmatac.ContainsKey(MapMaTac))
                                {
                                    dicmatac.Add(MapMaTac, Stopwatch.StartNew());
                                }
                                else
                                {
                                    dicmatac[MapMaTac] = Stopwatch.StartNew();
                                }
                            }
                            catch
                            {
                            }
                        }
                    }
                    if (msg.Contains("BHRWSC_110331_22"))
                    {
                        VuKhoString = msg;
                    }
                    if (msg.Contains("#{DJTS_110509_46}") || msg.Contains("#{DJTS_110509_47}"))
                    {
                        Thread.Sleep(500);
                        OkPhu();
                        Thread.Sleep(500);
                        OkPhu();
                        Thread.Sleep(500);
                        OkPhu();
                        Thread.Sleep(500);
                        OkPhu();
                        Thread.Sleep(500);
                        OkPhu();
                        Thread.Sleep(500);
                        OkPhu();
                    }
                    if (TLBB.MapId == MAP.LoiDaiSinhTu)
                    {
                        if (msg.Contains("SXRW_090630_111"))
                        {
                            Party.ForEach(g => g.SatTinhMonter = "Tử Sắc Yêu Tinh");
                        }
                        else if (msg.Contains("SXRW_090630_112"))
                        {
                            Party.ForEach(g => g.SatTinhMonter = "Hoàng Sắc Yêu Tinh");
                        }
                        else if (msg.Contains("SXRW_090630_113"))
                        {
                            Party.ForEach(g => g.SatTinhMonter = "Hồng Sắc Yêu Tinh");
                        }
                    }
                    if (TLBB.MapId == MAP.ThieuThatSon)
                    {
                        if (msg.Contains("SSS_ZJ_120109_13"))
                        {
                            Party.ForEach(g => g.swTieuVienSon = Stopwatch.StartNew());
                        }
                    }
                    if (isAcBa != -1)
                    {
                        string vietlien = TDT.VietLien(msg);
                        if (vietlien.Contains("gianghotieutieu") || vietlien.Contains("#{qyxt_15}"))
                        {
                            if (vietlien.Contains("duongmon"))
                            {
                                AcBa = 37;
                            }
                            else if (vietlien.Contains("daohoadao"))
                            {
                                AcBa = MENPAI.DaoHoa;
                            }
                            else if (vietlien.Contains("modung"))
                            {
                                AcBa = 32;
                            }
                            else if (vietlien.Contains("tinhtuc"))
                            {
                                AcBa = 6;
                            }
                            else if (vietlien.Contains("tieudao"))
                            {
                                AcBa = 9;
                            }
                            else if (vietlien.Contains("thieulam"))
                            {
                                AcBa = 1;
                            }
                            else if (vietlien.Contains("thienson"))
                            {
                                AcBa = 8;
                            }
                            else if (vietlien.Contains("thienlong"))
                            {
                                AcBa = 7;
                            }
                            else if (vietlien.Contains("ngamy"))
                            {
                                AcBa = 5;
                            }
                            else if (vietlien.Contains("vodang"))
                            {
                                AcBa = 4;
                            }
                            else if (vietlien.Contains("minhgiao"))
                            {
                                AcBa = 2;
                            }
                            else if (vietlien.Contains("caibang"))
                            {
                                AcBa = 3;
                            }
                            else if (vietlien.Contains("quycoc"))
                            {
                                AcBa = MENPAI.QuyCoc;
                            }
                            if(LastAcBa != AcBa)
                            {
                                LastAcBa = AcBa;                                
                                Main.PushLog(DateTime.Now.ToString("HH:mm dd-MM ") + "Ác Bá " + " -> " + TLBB.GetMenpaiName(AcBa.ToString()) + " [" + LastName + "]");
                            }
                            SetAcBa();
                        }
                    }

                    if (isMsg != -1)
                    {
                        Byte[] buffer;
                        if (Address.GameType == 1)
                        {
                            try
                            {
                                buffer = buff.Skip(11 + isMsg).ToArray();
                                int spiIdx = -1;
                                int endIdx = -1;

                                for (int i = 0; i < buffer.Length; i++)
                                {
                                    //if (buffer[i] <= 0xF && buffer[i] != 0 && spiIdx == -1 && buffer[i] > 0x5)
                                    //{
                                    //    //buffer[i] = 0x20;
                                    //    spiIdx = i;
                                    //}
                                    if (buffer[i] == 0)
                                    {
                                        endIdx = i;
                                        break;
                                    }
                                }

                                for (int i = endIdx - 12; i < endIdx; i++)
                                {
                                    if (buffer[i] < 0xF)
                                    {
                                        spiIdx = i;
                                        break;
                                    }
                                }

                                byte[] buffMsg = buffer.Take(spiIdx).ToArray();
                                byte[] buffFrom = buffer.Skip(spiIdx + 1).Take(endIdx - spiIdx).ToArray();

                                string from = ConverterEx.VISCII2Unicode(buffFrom);
                                string mg = ConverterEx.VISCII2Unicode(buffMsg).Replace("\n", "#r").Replace("\\0", "");
                                mg = Regex.Replace(mg, "INFOMSG([0-9]*)[^}]*}", "INFOMSG$1}");

                                if (!string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(mg))
                                {
                                    string ct = DateTime.Now.ToString("HH:mm dd-MM") + "#" + "[" + from + "] -> [" + TLBB.Name + "]:" + mg;
                                    IniParser.Write("Item", "Chat", ct + "\n" + string.Join("\n", IniParser.Read("Item", "Chat").Split('\n').Take(300).ToArray()));
                                    string content = Game.IniParser.Read("Item", "Chat");

                                    Main.Instance.Invoke(new Action(() =>
                                    {
                                        try
                                        {
                                            ChatLogs.Instance.ReadChat();
                                            // string last = Main.Instance.ricInbox.Text;
                                            // Main.Instance.ricInbox.Text = string.Empty;
                                            // Main.Instance.ricInbox.Text = last.Trim() + Environment.NewLine
                                            //+ "->[" + DateTime.Now.ToString("HH:mm dd-MM") + "]" + "[" + from + " pm " + TLBB.Name + "]<-" + Environment.NewLine + mg.Replace("-", "");
                                            // Main.Instance.ricInbox.Text = string.Join("\n", Main.Instance.ricInbox.Text.Split('\n').Skip(Math.Max(0, Main.Instance.ricInbox.Text.Split('\n').Length - 200)).ToArray());
                                            // Main.Instance.ricInbox.ScroolToEnd();

                                            // StringExtensions.RichTextBoxChangeWordColor(ref Main.Instance.ricInbox, "->", "<-", Color.BlueViolet);
                                        }
                                        catch { }

                                        if (!Main.IsMuteInbox)
                                            Music.PlayPM();
                                    }));
                                }

                                continue;
                            }
                            catch { }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Main.PushLogEx(ex.Message + ex.StackTrace);
            }
        }

        [DllImport("tieudattai.dll")]
        private static extern IntPtr SetHook(IntPtr handle);

        [DllImport("tieudattai.dll")]
        public static extern IntPtr UnHook(IntPtr handle);
    }
}