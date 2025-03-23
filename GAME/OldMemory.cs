using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace _i
{
    partial class Game
    {
        public void NhanQuaSuDo()
        {
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                Thread.Sleep(200);
                DoStringEx("Clear_XSCRIPT() Set_XSCRIPT_Function_Name('GetGrowthPlanPrize') Set_XSCRIPT_ScriptID(891112) Set_XSCRIPT_Parameter(0, 2); Set_XSCRIPT_Parameter(1, 1); Set_XSCRIPT_ParamCount(2) Send_XSCRIPT()");
                Thread.Sleep(200);
                DoStringEx("Clear_XSCRIPT() Set_XSCRIPT_Function_Name('GetGrowthPlanPrize') Set_XSCRIPT_ScriptID(891112) Set_XSCRIPT_Parameter(0, 3); Set_XSCRIPT_Parameter(1, 1); Set_XSCRIPT_ParamCount(2) Send_XSCRIPT()");
                Thread.Sleep(200);
                DoStringEx("Clear_XSCRIPT() Set_XSCRIPT_Function_Name('GetGrowthPlanPrize') Set_XSCRIPT_ScriptID(891112) Set_XSCRIPT_Parameter(0, 4); Set_XSCRIPT_Parameter(1, 1); Set_XSCRIPT_ParamCount(2) Send_XSCRIPT()");
                Thread.Sleep(200);
                DoStringEx("Clear_XSCRIPT() Set_XSCRIPT_Function_Name('GetGrowthPlanPrize') Set_XSCRIPT_ScriptID(891112) Set_XSCRIPT_Parameter(0, 5); Set_XSCRIPT_Parameter(1, 1); Set_XSCRIPT_ParamCount(2) Send_XSCRIPT()");
                Thread.Sleep(200);
                DoStringEx("Clear_XSCRIPT() Set_XSCRIPT_Function_Name('GetGrowthPlanPrize') Set_XSCRIPT_ScriptID(891112) Set_XSCRIPT_Parameter(0, 6); Set_XSCRIPT_Parameter(1, 1); Set_XSCRIPT_ParamCount(2) Send_XSCRIPT()");
                Thread.Sleep(200);
                DoStringEx("Clear_XSCRIPT() Set_XSCRIPT_Function_Name('GetGrowthPlanPrize') Set_XSCRIPT_ScriptID(891112) Set_XSCRIPT_Parameter(0, 7); Set_XSCRIPT_Parameter(1, 1); Set_XSCRIPT_ParamCount(2) Send_XSCRIPT()");
                Thread.Sleep(200);
                DoStringEx("Clear_XSCRIPT() Set_XSCRIPT_Function_Name('GetGrowthPlanPrize') Set_XSCRIPT_ScriptID(891112) Set_XSCRIPT_Parameter(0, 8); Set_XSCRIPT_Parameter(1, 1); Set_XSCRIPT_ParamCount(2) Send_XSCRIPT()");
                Thread.Sleep(200);
                DoStringEx("Clear_XSCRIPT() Set_XSCRIPT_Function_Name('GetGrowthPlanPrize') Set_XSCRIPT_ScriptID(891112) Set_XSCRIPT_Parameter(0, 9); Set_XSCRIPT_Parameter(1, 1); Set_XSCRIPT_ParamCount(2) Send_XSCRIPT()");
                Thread.Sleep(200);
                DoStringEx("Clear_XSCRIPT() Set_XSCRIPT_Function_Name('GetGrowthPlanPrize') Set_XSCRIPT_ScriptID(891112) Set_XSCRIPT_Parameter(0, 10); Set_XSCRIPT_Parameter(1, 1); Set_XSCRIPT_ParamCount(2) Send_XSCRIPT()");
                Thread.Sleep(500);
            }).Start();
        }

        private int BachHoaDuyenX;
        private int BachHoaDuyenY;

        public bool LamDayTayNai()
        {
            if (TLBB.IsODaoCuFull && TLBB.IsONguyenLieuFull)
                return true;
            if (!TLBB.IsODaoCuFull)
            {
                foreach (var item in PacketItems.DaoCu)
                {
                    if (item.Count > 1)
                    {
                        PacketItems.SplitIndex = item.Index;
                        DoStringEx("setmetatable(_G, {__index = SplitItem_Env}); PlayerPackage:SplitItem(1);");
                        return false;
                    }
                }
                PushDebugMessage("không thể làm đầy tay nải");
                RemoveMission(MissionsType.MoBaoTangDo);
                return false;
            }
            if (!TLBB.IsONguyenLieuFull)
            {
                foreach (var item in PacketItems.All)
                {
                    if (item.Count > 1)
                    {
                        PacketItems.SplitIndex = item.Index;
                        DoStringEx("PlayerPackage:SplitItem(1);");
                        return false;
                    }
                }
                PushDebugMessage("không thể làm đầy tay nải");
                RemoveMission(MissionsType.MoBaoTangDo);
                return false;
            }
            return false;
        }

        public bool IsNhanMam { get; set; }
        public bool IsNhanHoaHong { get; set; }
        public bool IsKetNghia { get; set; }

        public bool IsSuDoo { get; set; }

        private int cntKetNghia { get; set; } = 0;

        public string TenThanh()
        {
            DoStringEx("local szMsgtenthanh = Guild:GetMyGuildDetailInfo('CityName'); return szMsgtenthanh;");
            LuaToStringBang();
            return LuaStringBang();
        }

        public string TenMapThanh()
        {
            DoStringEx("local szMsgtenmap = Guild:GetMyGuildDetailInfo('LocalScene'); return szMsgtenmap;");
            LuaToString();
            return LuaString();
        }

        public void BangOpen()
        {
            DoStringEx("setmetatable(_G, {__index = NewBangHui_Hygl_Env});Guild:AskGuildDetailInfo();");
        }

        public void BangClose()
        {
            PostMessage(63, 105);
        }
        public void NhanQuaBuiHoaHong()
        {
            if (!IsOneSec)
                return;
            if (GoToEx(NPC.MaiKhoiTienTu))
            {
                if (TLBB.IsQuestOpen)
                {
                    foreach (QuestFrame dialog in QuestFrame.Enum(this))
                    {
                        if (dialog.Name.Contains("#{MGNQ_20130724_19}"))
                        {
                            QuestFrameOptionClicked(dialog);
                            RemoveMission(MissionsType.NhanQuaBuiHoaHong);
                            if (IsByLogin)
                            {
                                IsByLogin = false;
                                IsXongBHD = true;
                            }
                            break;
                        }
                    }
                }
                else
                {
                    Talk("maikhoitientu");
                }
            }
        }


        public void NhanHoaHongLo()
        {
            if (!IsOneSec)
                return;
            if (GoToEx(NPC.ThanTinhYeu))
            {
                if (TLBB.IsQuestOpen)
                {
                    foreach (QuestFrame dialog in QuestFrame.Enum(this))
                    {
                        if (dialog.Name.Contains("#{MGZL_101130_01}"))
                        {
                            QuestFrameOptionClicked(dialog);
                            RemoveMission(MissionsType.NhanHoaHongLo);
                            break;
                        }
                    }
                }
                else
                {
                    Talk("thantinhyeu");
                }
            }
        }
        public bool IsTuoiHoa { get; set; }

        public void TuoiHoaHong()
        {
            if (!IsTuoiHoa)
                return;
            if (!IsOneSec)
                return;
            if (GoToEx(256, 246, 0))
            {
                if (TLBB.IsRide)
                    DownRide();
                GameObject _hoagannhat = null;

                try
                {
                    _hoagannhat = Objects.All.Where(_object => _object.CleanName == "maikhoitientu").OrderBy(_object => _object.Distance).First();
                }
                catch { }
                if (_hoagannhat != null)
                {
                    SelectTarget(_hoagannhat.Id);
                    DoAction("CircularTaskTool20_3");
                }

                //CircularTaskTool20_3
                //if (TLBB.IsQuestOpen)
                //{
                //    foreach (QuestFrame dialog in QuestFrame.Enum(this))
                //    {
                //        if (dialog.Name.Contains("#{MGZL_101130_01}"))
                //        {
                //            QuestFrameOptionClicked(dialog);
                //            IsNhanHoaHongLo = false;
                //            break;
                //        }

                //    }
                //}
                //else
                //{
                //    Talk("thantinhyeu");
                //}
            }
        }

        public static int TrongHoaX1 = 55;
        public static int TrongHoaX2 = 55;
        public static int TrongHoaX3 = 55;
        public static int TrongHoaX4 = 55;
        public static int TrongHoaX5 = 55;

        public static int MaxHoa1 = 14;
        public static int MaxHoa2 = 14;
        public static int MaxHoa3 = 14;
        public static int MaxHoa4 = 14;
        public static int MaxHoa5 = 14;
        public int TrongHoaIdx { get; set; } = 1;

        public int TrongHoaXX
        {
            get
            {
                if (TrongHoaIdx == 1)
                    return TrongHoaX1;
                if (TrongHoaIdx == 2)
                    return TrongHoaX2;
                if (TrongHoaIdx == 3)
                    return TrongHoaX3;
                if (TrongHoaIdx == 4)
                    return TrongHoaX4;
                if (TrongHoaIdx == 5)
                    return TrongHoaX5;
                return TrongHoaX1;
            }
        }

        public int MaxHoaXX
        {
            get
            {
                if (TrongHoaIdx == 1)
                    return MaxHoa1;
                if (TrongHoaIdx == 2)
                    return MaxHoa2;
                if (TrongHoaIdx == 3)
                    return MaxHoa3;
                if (TrongHoaIdx == 4)
                    return MaxHoa4;
                if (TrongHoaIdx == 5)
                    return MaxHoa5;
                return MaxHoa1;
            }
        }

        public int ThuHoaX { get; set; } = 0;
        public int RaDiusThuHoa { get; set; } = 999;
        public bool HaveBienThan
        {
            get
            {
                if (TLBB.MapId != MAP.DaiLy)
                    return false;
                foreach (var item in PacketItems.All)
                {
                    if (item.Type.Contains("Charm4_16") || item.ClearName.Contains("sudodai"))
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        public bool ThuHoachHoaEx()
        {
            if (!Global.IsThuHoachHoa)
                return false;

            if (!IsThuHoachHoaEx)
                return false;

            if (TLBB.PlayerState != 0)
                return true;
            foreach (GameObject _object in Objects.All)
            {
                if (_object.Name == "Hoa Trưởng Thành")
                {
                    try
                    {
                        GameObject.HoaTime.Add(_object.Point, TDT.SeconNow);
                    }
                    catch { }
                }
                if (_object.Name.Contains("Tiên Hoa Ấu Miêu"))
                {
                    GameObject.HoaTime.Remove(_object.Point);
                }
            }

            if (TLBB.IsQuestOpen)
            {
                try
                {
                    GameObject _object = Objects.All.Where(obj => obj.Name.Contains("Hoa Trưởng Thành") && obj.Id == QuestFrame.ID).FirstOrDefault();
                    if (_object != null)
                    {
                        dialogInfo = QuestFrame.All(this).Replace("#{SDJZH_091106_08}", "");
                        dialogInfo = dialogInfo.Replace("#{SDJZH_091106_09}", "");
                        if (hashHoaDialog != dialogInfo)
                        {
                            hashHoaDialog = dialogInfo;
                            int time = TDT.ParseInt(dialogInfo);
                            if (time < 0)
                            {
                                time = 0;
                            }
                            if (GameObject.HoaTime.ContainsKey(_object.Point))
                            {
                                GameObject.HoaTime[_object.Point] = TDT.SeconNow - (182 - time);
                            }
                            else
                            {
                                try
                                {
                                    GameObject.HoaTime.Add(_object.Point, TDT.SeconNow - (182 - time));
                                }
                                catch { }
                            }
                            if (time <= 3)
                            {
                                Stopwatch runtime = Stopwatch.StartNew();
                                while (true)
                                {
                                    if (runtime.Elapsed.TotalSeconds >= 10)
                                        break;

                                    UseSkill(3, QuestFrame.ID);
                                    Thread.Sleep(500);

                                    if (TLBB.IsQuestOpen)
                                    {
                                        if (QuestFrame.Text.Contains("#{SDJZH_091106_08}"))
                                        {
                                            dialogInfo = QuestFrame.All(this).Replace("#{SDJZH_091106_08}", "");
                                            dialogInfo = dialogInfo.Replace("#{SDJZH_091106_09}", "");
                                            time = TDT.ParseInt(dialogInfo);
                                            if (GameObject.HoaTime.ContainsKey(_object.Point))
                                            {
                                                GameObject.HoaTime[_object.Point] = TDT.SeconNow - (182 - time);
                                            }
                                            else
                                            {
                                                try
                                                {
                                                    GameObject.HoaTime.Add(_object.Point, TDT.SeconNow - (182 - time));
                                                }
                                                catch { }
                                            }
                                            if (time <= 3)
                                            {
                                                continue;
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                catch
                {
                }
            }

            GameObject BestHoa = null;

            BestHoa = Objects.All.Where(
                o => o.X >= ThuHoaX && TDT.NumDiff(o.RoundX, ThuHoaX) <= RaDiusThuHoa
                && o.Name.Contains("Hoa Trưởng Thành") && IsHoaMine(o.Title)
                && !AllOtherCareId.Contains(o.Id)).OrderBy(o => o.Distance).Where(o => o.TimeXuatHien >= 180).FirstOrDefault();
            if (BestHoa == null)
            {
                BestHoa = Objects.All.Where(
                    o => o.X >= ThuHoaX && TDT.NumDiff(o.RoundX, ThuHoaX) <= RaDiusThuHoa
                    && o.Name.Contains("Hoa Trưởng Thành") && IsHoaMine(o.Title)
                    && !AllOtherCareId.Contains(o.Id)).OrderByDescending(o => o.TimeXuatHien).FirstOrDefault();
            }

            if (BestHoa != null)
            {
                CareObject = BestHoa;
                if (GoToEx(BestHoa.X, BestHoa.Y))
                {
                    if (TLBB.IsRide)
                        DownRide();
                    if (TLBB.IsBienThan)
                        HuyBienThan();
                    UseSkill(3, BestHoa.Id);
                    return true;
                }
            }
            else
            {
                CareObject = null;
            }
            return false;
        }

        public bool IsThuHoachHoaEx { get; set; }
        public bool IsDuaHau { get; set; }
        public void BachHoaDuyen()
        {
            if (Global.Paused)
                return;
            if (!IsOneSec)
                return;
            if (TLBB.Lvl < 30)
            {
                IsXongBHD = true;
                return;
            }
            if (IsNhanMam && IsXongBHD)
                return;

            if (!TLBB.Online)
                return;

            if ((TLBB.OnlineTimeSec / 60) > Setting.Value("numerBachHoaDuyenTime") && Setting.Value("numerBachHoaDuyenTime") > 0)
            {
                IsXongBHD = true;
                return;
            }
            if (TLBB.Lvl > 0 && TLBB.Lvl < 30)
            {
                IsXongBHD = true;
                return;
            }
            if (SecCount % 30 == 0)
            {
                State = STATE.None;
            }
            if (TLBB.MapId > 500 && TLBB.MapId < 600)
            {
                RaBang(MAP.DaiLy);
                return;
            }
            if (State == STATE.LayBinhHoa)
            {
                if (TLBB.MapId != MissionMap)
                {
                    GoToEx(0, 0, MissionMap, true);
                    return;
                }
                if (PacketItems.All.Where(i => i.Type == "TaskTools12_1" && i.Count >= 5).FirstOrDefault() != null)
                {
                    State = STATE.Done;
                    return;
                }
                GameObject objHoa = null;
                foreach (GameObject _object in Objects.All.Where(o => o.CleanName.Contains("uhoab")).OrderBy(o => o.Distance))
                {
                    if (BlackList.Contains(_object.Id))
                        continue;
                    if (TLBB.MapId == 189 && TDT.GetDistance(_object.X, _object.Y, 104, 145) <= 3) // fix tinh tuc
                    {
                        continue;
                    }
                    if (TLBB.PlayerState != 8)
                    {
                        foreach (GameObject obj in Objects.All)
                        {
                            if (TDT.GetDistance(_object.X, _object.Y, obj.X, obj.Y) < 3 && obj.State == 8 && Objects.Self != null && Objects.Self.Id != obj.Id)
                            {
                                continue;
                            }
                        }
                    }
                    objHoa = _object;
                    break;
                }
                if (objHoa != null)
                {
                    bool isDangHai = false;
                    if (TLBB.PlayerState != 8)
                    {
                        foreach (GameObject obj in Objects.All)
                        {
                            if (TDT.GetDistance(objHoa.X, objHoa.Y, obj.X, obj.Y) < 3 && obj.State == 8 && Objects.Self != null && Objects.Self.Id != obj.Id)
                            {
                                isDangHai = true;
                                break;
                            }
                        }
                    }
                    if (!isDangHai)
                    {
                        if (objHoa.Id != PickedId)
                        {
                            PickedId = objHoa.Id;
                            CareTime = Stopwatch.StartNew();
                        }
                        else
                        {
                            if (CareTime.Elapsed.TotalSeconds > 20)
                                BlackList.Add(PickedId);
                        }
                        if (GoToEx(objHoa, true))
                        {
                            if (TLBB.IsRide)
                                DownRide();
                            PickItem((int)objHoa.Id);
                        }
                        return;
                    }
                }
                MoveNextEx();
                return;
            }
            if (State == STATE.DiHaiDuoc)
            {
                if (TLBB.MapId != 4)
                {
                    GoToEx(0, 0, 4, true);
                    return;
                }
                if (PacketItems.All.Where(i => i.ClearName == "bachhophoa" && i.Count >= 3).FirstOrDefault() != null)
                {
                    State = STATE.Done;
                    return;
                }
                float minDistance = 1000;
                GameObject objHoa = null;
                foreach (GameObject _object in Objects.All)
                {
                    if (BlackList.Contains(_object.Id))
                        continue;
                    if (TDT.VietLien(_object.Name) == "bachhophoa")
                    {
                        if (TLBB.PlayerState != 8)
                        {
                            foreach (GameObject obj in Objects.All)
                            {
                                if (TDT.GetDistance(_object.X, _object.Y, obj.X, obj.Y) < 3 && obj.State == 8 && Objects.Self != null && Objects.Self.Id != obj.Id)
                                {
                                    continue;
                                }
                            }
                        }
                        if (TDT.GetDistance(CharX, CharY, _object.X, _object.Y) < minDistance)
                        {
                            minDistance = TDT.GetDistance(CharX, CharY, _object.X, _object.Y);
                            objHoa = _object;
                        }
                    }
                }
                if (objHoa != null)
                {
                    bool isDangHai = false;
                    if (TLBB.PlayerState != 8)
                    {
                        foreach (GameObject obj in Objects.All)
                        {
                            if (TDT.GetDistance(objHoa.X, objHoa.Y, obj.X, obj.Y) < 3 && obj.State == 8 && Objects.Self != null && Objects.Self.Id != obj.Id)
                            {
                                isDangHai = true;
                                break;
                            }
                        }
                    }
                    if (!isDangHai)
                    {
                        if (GoToEx(objHoa, true))
                        {
                            if (TLBB.IsRide)
                                DownRide();
                            PickItem((int)objHoa.Id);
                        }
                        return;
                    }
                }
                MoveNextEx();
                return;
            }
            if (State == STATE.Done || State == STATE.Null)
            {
                if (!DaNhanHoaHong)
                {
                    if (GoToEx(DAILY.BaCaiLy))
                    {
                        if (TLBB.Lvl < 40)
                        {
                            DaNhanHoaHong = true;
                        }
                        else if (GoToEx(DAILY.BaCaiLy))
                        {
                            for (int i = 0; i < 10; i++)
                            {
                                Talk(DAILY.BaCaiLy.Id);
                                Thread.Sleep(500);
                                if (TLBB.IsQuestOpen)
                                {
                                    break;
                                }
                            }
                            if (QuestFrame.Click("#{SDJZH_xml_XX(01)}"))
                            {
                                DaNhanHoaHong = true;
                                State = STATE.None;
                                if (IsNhanHoaHong)
                                {
                                    if (!Missions.Contains(MissionsType.NhanBienThan))
                                        IsXongBHD = true;
                                    else
                                        RemoveMission(MissionsType.BachHoaDuyen);
                                    return;
                                }
                            }
                            QuestFrame.Close();
                        }
                    }
                    return;
                }
                if (GoToEx(DAILY.ALy))
                {
                    for (int i = 0; i < 10; i++)
                    {
                        Talk(DAILY.ALy.Id);
                        Thread.Sleep(500);
                        if (TLBB.IsQuestOpen)
                        {
                            break;
                        }
                    }
                    if (!DaNhanHoaChung)
                    {
                        if (SafeTime > 3)
                        {
                            if (QuestFrame.Click("#{SDJZH_xml_XX(13)}"))
                            {
                                State = STATE.None;
                                if (IsNhanMam)
                                {
                                    if (!Missions.Contains(MissionsType.TrongHoa))
                                    {
                                        if (!Missions.Contains(MissionsType.NhanBienThan))
                                            IsXongBHD = true;
                                    }
                                    else
                                        RemoveMission(MissionsType.BachHoaDuyen);
                                    return;
                                }
                                DaNhanHoaChung = true;
                                Thread.Sleep(5000);
                            }
                            QuestFrame.Close();
                        }
                        return;
                    }
                    for (int i = 0; i < 10; i++)
                    {
                        if (QuestFrame.Click("#{SDHDRW_xml_XX(03)}"))
                            break;
                        Thread.Sleep(1500);
                    }
                    for (int i = 0; i < 10; i++)
                    {
                        if (QuestFrame.Click("bachhoaduyen"))
                            break;
                        Thread.Sleep(1500);
                    }
                    if (State == STATE.Done)
                    {
                        Thread.Sleep(1000);
                        LUA.QuestFrameMissionComplete();
                        Thread.Sleep(1000);
                        DemMam();
                        LUA.DoString("return COUNT;");
                        LuaToString();
                        Thread.Sleep(1000);
                        BHDCount++;
                        string info = LuaToString();
                        if (info.Contains("#{SDHDRW_091109_29}"))
                        {
                            info = info.Replace("#{SDHDRW_091109_29}", "");
                            int count = TDT.ParseInt(info);
                            if (count != 0)
                            {
                                int cnt = TDT.ParseInt(info);
                                if (BHDCount != cnt)
                                {
                                    BHDCount = cnt;
                                    if (TotalXongBHD == 0)
                                    {
                                        swTotalXongBHD = Stopwatch.StartNew();
                                    }
                                    TotalXongBHD++;
                                }
                            }
                        }

                        if (BHDCount >= Global.MaxBHD)
                        {
                            RemoveMission(MissionsType.BachHoaDuyen);
                            if (IsByLogin)
                                IsXongBHD = true;
                            DoStringEx("COUNT = nil;");
                        }
                    }
                    State = STATE.None;
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
                DoStringEx("local cnt = 0; while true do local name, content = DataPool:GetPlayerMission_Memo(cnt); if string.find(name, 'Bách Hoa Duyên') then if DataPool:GetPlayerMission_Variable(cnt, 0) == 1 and not string.find(content, '#{SDHDRW_091109_44}') then return '" + TDT.RandomEmptyString() + "' ..'Xong'; else return '" + TDT.RandomEmptyString() + "' .. content; end end cnt = cnt + 1; if cnt == 20 then return '" + TDT.RandomEmptyString() + "' ..'Chua'; end end");
                LuaToString();
                Thread.Sleep(1000);
                MissionInfo = LuaToString().Trim();
                if (MissionInfo == "")
                {
                    State = STATE.None;
                    return;
                }
                if (MissionInfo == "Xong")
                {
                    State = STATE.Done;
                    return;
                }
                else if (MissionInfo == "Chua")
                {
                    State = STATE.Null;
                    return;
                }
                else
                {
                    if (TDT.MuaDo(MissionInfo) != "")
                    {
                        MissionX = 104;
                        MissionY = 123;
                        State = STATE.DiMuaDo;
                        foreach (var item in PacketItems.All)
                        {
                            if (TDT.VietLien(item.Name) == "bake")
                            {
                                State = STATE.Done;
                                return;
                            }
                        }
                        return;
                    }
                    if ((Global.IsHuyDanhQuai) && TDT.PhuBanDanhQuai(MissionInfo) != "")
                    {
                        DoStringEx("local cnt = 0; while true do local name, content = DataPool:GetPlayerMission_Memo(cnt); if string.find(name, 'ch Hoa') then if DataPool:GetPlayerMission_Variable(cnt, 0) ~= 1 then DataPool:Mission_Abnegate_Popup(cnt,DataPool:GetPlayerMission_Memo(cnt)); end end cnt = cnt + 1; if cnt == 20 then return end end");
                        State = STATE.HuyQ;
                        return;
                    }
                    if (TDT.PhuBanMP(MissionInfo) != "")
                    {
                        MissionInfo = TDT.PhuBanMP(MissionInfo);
                        MissionMap = MissionInfo.Split(',')[0].ToNumber();
                        if (MissionMap == 0)
                            MissionMap = LACDUONG.Id;
                        State = STATE.LayBinhHoa;
                        if (TDT.PhuBanDanhQuai(MissionInfo) != string.Empty)
                        {
                            MissionInfo = TDT.PhuBanDanhQuai(MissionInfo);
                            MissionX = int.Parse(MissionInfo.Split(',')[0]);
                            MissionY = int.Parse(MissionInfo.Split(',')[1]);
                            State = STATE.DanhQuai;

                            return;
                        }
                        if (Global.IsHuyHyHuu)
                        {
                            DoStringEx("local cnt = 0; while true do local name, content = DataPool:GetPlayerMission_Memo(cnt); if string.find(name, 'ch Hoa') then if DataPool:GetPlayerMission_Variable(cnt, 0) ~= 1 then DataPool:Mission_Abnegate_Popup(cnt,DataPool:GetPlayerMission_Memo(cnt)); end end cnt = cnt + 1; if cnt == 20 then return end end");
                            State = STATE.HuyQ;
                        }
                        return;
                    }
                    if (TDT.HaiDuoc(MissionInfo) != "")
                    {
                        if (Global.IsHuyThaiHo)
                        {
                            DoStringEx("local cnt = 0; while true do local name, content = DataPool:GetPlayerMission_Memo(cnt); if string.find(name, 'ch Hoa') then if DataPool:GetPlayerMission_Variable(cnt, 0) ~= 1 then DataPool:Mission_Abnegate_Popup(cnt,DataPool:GetPlayerMission_Memo(cnt)); end end cnt = cnt + 1; if cnt == 20 then return end end");
                            State = STATE.HuyQ;
                            return;
                        }
                        MissionMap = 4;
                        MissionX = 218;
                        MissionY = 271;
                        State = STATE.DiHaiDuoc;
                        return;
                    }
                    if (TDT.GapNPC(MissionInfo) != "")
                    {
                        string temp = TDT.GapNPC(MissionInfo);
                        MissionMap = int.Parse(temp.Split(',')[0]);
                        if (MissionMap == 0)
                            MissionMap = LACDUONG.Id;
                        MissionX = int.Parse(temp.Split(',')[1]);
                        MissionY = int.Parse(temp.Split(',')[2]);
                        MissionID = int.Parse(temp.Split(',')[4]);
                        State = STATE.GapNPCGiaoDo;
                        return;
                    }
                }
                State = STATE.None;
                return;
            }
            if (State == STATE.HuyQ)
            {
                swStandTime = Stopwatch.StartNew();
                LUA.MessageBox_Self_OK_Clicked();
                State = STATE.None;
                return;
            }
            if (State == STATE.DiMuaDo)
            {
                if (PacketItems.All.Where(i => i.ClearName == "bake").FirstOrDefault() != null)
                {
                    State = STATE.Done;
                    return;
                }
                if (GoToEx(DAILY.LoTamThat))
                {
                    if (TLBB.IsShopOpen)
                    {
                        foreach (var item in PacketItems.Shop)
                        {
                            if (item.ClearName == "bake")
                            {
                                LUA.Buy((int)item.Index, 20);
                                Thread.Sleep(2000);
                                return;
                            }
                        }
                    }
                    if (TLBB.IsQuestOpen)
                    {
                        foreach (QuestFrame dialog in QuestFrame.Enum(this))
                        {
                            if (TDT.VietLien(dialog.Name).Contains("muathuoc"))
                            {
                                QuestFrameOptionClicked(dialog);
                                return;
                            }
                        }
                        QuestFrame.Close();
                        return;
                    }
                    Talk(NPC.LoTamThat.Id);
                    return;
                }
            }
            if (State == STATE.GapNPCGiaoDo)
            {
                if (TLBB.IsQuestOpen)
                {
                    QuestFrame.Click("bachhoaduyen");
                    State = STATE.None;
                    QuestFrame.Close();
                    return;
                }
                if (MissionID == 16)
                {
                    if (GoToEx(DAILY.DoanChinhThuan))
                    {
                        Talk(DAILY.DoanChinhThuan.Id);
                    }
                }
                if (MissionID == 34)
                {
                    if (GoToEx(LACDUONG.TriThanhDaiSu))
                    {
                        Talk(LACDUONG.TriThanhDaiSu.Id);
                    }
                }
                if (MissionID == 2)
                {
                    if (GoToEx(TOCHAU.ToThuc))
                    {
                        Talk(TOCHAU.ToThuc.Id);
                    }
                }
                return;
            }
            if (State == STATE.DanhQuai)
            {
                if (GoToEx(MissionX, MissionY, MissionMap, false, 15))
                {
                    foreach (var item in PacketItems.All)
                    {
                        if (TDT.VietLien(item.Name).StartsWith("ti") && TDT.VietLien(item.Name).EndsWith("c") && TDT.VietLien(item.Name).Length == 16)
                        {
                            if (item.Count == 10)
                            {
                                State = STATE.Done;
                                return;
                            }
                        }
                    }
                    if (TLBB.IsRide)
                        DownRide();
                    if (!ForcePickItem())
                        ForceAttack();
                }
                return;
            }
            State = STATE.None;
        }

        public string QDInfo = "";
        public int QDuaX = 134;
        public int QDuaY = 165;
        public int QDuaMap = MAP.TayHo;
        //lecaotriduahau
        public string thongtinbang = "";
        public int SoHopQua = 0;
        public static List<int> ListDangLumHop = new List<int>();
        public string TenThanhKT = "-1";
        public string TenMapThanhKT = "-1";

        public static List<string> ListThanh { get; set; } = new List<string>();

        public void DuaHau()
        {
            if (!IsDuaHau || Global.Paused)
                return;

            if (TLBB.IsSelectServer || TLBB.IsLogon || TLBB.IsNexLogin || TLBB.IsSelectRole)
            {
                MissionState = "";
                TenThanhKT = "";
                return;
            }
            if (!TLBB.Online)
            {
                MissionState = "";
                return;
            }

            IsAuto = !IsDuaHau;

            if (!IsOneSec)
                return;

            if (TLBB.GuildId == 0xFFFFFFFF)
            {
                if (GoToEx(LACDUONG.PhamThuanNhan))
                {
                    if (!TLBB.IsQuestOpen)
                        TalkEx("phanthuannhan");
                }
                return;
            }

            if (!(TLBB.MapId > 500))
            {
                if (!IsMoveEx)
                {
                    FixKetMap();
                    return;
                }
            }

            if (TenThanhKT == "-1" || TenThanhKT == "")
            {
                BangOpen();
                TenThanhKT = TenThanh();
                TenMapThanhKT = TenMapThanh();
                string tenthanh = TDT.VietLien(TenThanhKT);
                if (!ListThanh.Contains(tenthanh))
                {
                    ListThanh.Add(tenthanh);
                }
                IsMoBang = true;
                return;
            }

            if (IsMoBang)
            {
                BangClose();
            }
            //xu ly tam thoi
            if (!Global.BangOnPC.Contains(TDT.VietLien(TenThanhKT)) && TLBB.IsBienThan)
            {
                Global.BangOnPC.Add(TDT.VietLien(TenThanhKT));
            }

            if ((TLBB.MapId > 500) && IdleTime % 20 == 0 && IdleTime > 0)
            {
                MissionState = "";
                return;
            }

            if (MissionState == "Xong3VongNV")
            {
                if (TLBB.IsBienThan)
                {
                    MissionState = "DenChuNha";
                    return;
                }
                if (GoToEx(113, 100))
                {
                    if (IsByLogin)
                        IsXongBHD = true;
                    //TrangThaiQD = "";
                    return;
                }

                //Global.BangOnPC.Remove(TenThanhKT);

                return;
            }

            if (MissionState == "")
            {
                if (!TLBB.IsTogleMission)
                {
                    PostMessage(18, 105);
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
                PostMessage(18, 105);
                MissionState = "CloseMission";
                return;
            }
            if (MissionState == "CloseMission")
            {
                if (TLBB.IsBienThan)
                {
                    foreach (GameTask task in GameTask.Enum(this))
                    {
                        if (task.Name.Contains("#{QLYX_100517_28}") && task.Completed)
                        {
                            MissionState = "XongQD";
                            return;
                        }
                    }

                    MissionState = "DenChuNha";
                    return;
                }
                MissionState = "GetQDInfo";
                return;
            }
            if (MissionState == "GetQDInfo")
            {
                //if (!Task.Have(this, "#{QLYX_100517_28}"))
                if (!TLBB.IsBienThan)
                {
                    if (!TLBB.IsMapBang(TLBB.MapId))
                    {
                        VaoBang();
                        return;
                    }
                    if (GoToEx(BANG.ChuTheHuu))
                    {
                        if (TLBB.IsRide)
                        {
                            DownRide();
                            return;
                        }
                        if (TLBB.IsQuestOpen)
                        {
                            MissionState = "TalkToNhanNV";
                        }
                        else
                        {
                            TalkEx("chuthehuu");
                        }
                        return;
                    }
                }
                else
                {
                    MissionState = "CloseMission";
                    return;
                }

                return;
            }

            if (MissionState == "TalkToNhanNV")
            {
                if (GoToEx(BANG.ChuTheHuu))
                {
                    if (TLBB.IsQuestOpen)
                    {
                        if (QuestFrame.Click("{QLYX_100517_28}"))
                            MissionState = "NhanNV";
                    }
                    else
                    {
                        TalkEx("chuthehuu");
                        //Talk(BANG.ChuTheHuu);
                        //foreach (GameObject _object in Objects.AllNpc)
                        //{
                        //    if (_object.CleanName == "chuthehuu")
                        //    {
                        //        if (_object.Id != BANG.ChuTheHuu.Id)
                        //            Talk(_object.Id);
                        //        break;
                        //    }
                        //}
                        return;
                    }
                }
            }

            if (MissionState == "NhanNV")
            {
                if (GoToEx(BANG.ChuTheHuu))
                {
                    if (TLBB.IsQuestOpen)
                    {
                        foreach (QuestFrame dialog in QuestFrame.Enum(this))
                        {
                            if (dialog.Name.Contains("{QLYX_100517_32}"))
                            {
                                QuestFrameOptionClicked(dialog);
                                MissionState = "NhanNVMoi";
                                return;
                            }
                        }
                    }
                    else
                    {
                        TalkEx("chuthehuu");
                        //Talk(BANG.ChuTheHuu.Id);
                        //foreach (GameObject _object in Objects.AllNpc)
                        //{
                        //    if (_object.CleanName == "chuthehuu")
                        //    {
                        //        if (_object.Id != BANG.ChuTheHuu.Id)
                        //            Talk(_object.Id);
                        //        break;
                        //    }
                        //}
                        MissionState = "TalkToNhanNV";
                        return;
                    }
                }
            }
            if (MissionState == "NhanNVMoi")
            {
                PostMessage(13, 105);
                MissionState = "NhanNVMoi2";
                return;
            }
            if (MissionState == "NhanNVMoi2")
            {
                foreach (GameTask task in GameTask.Enum(this))
                {
                    if (task.Name.Contains("#{QLYX_100517_28}") && !task.Completed)
                    {
                        MissionState = "DenChuNha";
                        return;
                    }
                }
                MissionState = "Xong3VongNV";
                return;
            }
            if (MissionState == "DenChuNha")
            {
                if (TLBB.MapId > 500)
                {
                    if (Global.MapDua == MAP.NhiHai)
                    {
                        RaBang(2);
                        return;
                    }

                    if (Global.MapDua == MAP.TayHo)
                    {
                        RaBang(1);
                        return;
                    }

                    return;
                }

                QDuaX = Global.XDua;
                QDuaY = Global.YDua;
                QDuaMap = Global.MapDua;

                if (Global.Mapbang)
                {
                    if (TenMapThanhKT == "-1")
                    {
                        BangOpen();
                        TenMapThanhKT = TenMapThanh();
                    }
                    thongtinbang = TDT.GetDuaBangXY(TenMapThanhKT);
                    string[] infomap = thongtinbang.Split(',');

                    QDuaX = int.Parse(infomap[0]);
                    QDuaY = int.Parse(infomap[1]);
                    QDuaMap = int.Parse(infomap[2]);
                }
                if (TLBB.MapId != QDuaMap)
                {
                    Move(QDuaX, QDuaY, QDuaMap);
                    return;
                }
                else if (TDT.GetDistance(CharX, CharY, QDuaX, QDuaY) > 4)
                {
                    Move(QDuaX, QDuaY);
                }
                else if (TDT.GetDistance(CharX, CharY, QDuaX, QDuaY) <= 4)
                {
                    MissionState = "DaChayDenNoi";
                    return;
                }
            }

            if (MissionState == "DaChayDenNoi")
            {
                if (TLBB.MapId != QDuaMap || TDT.GetDistance(CharX, CharY, QDuaX, QDuaY) > 10)
                {
                    Move(QDuaX, QDuaY, QDuaMap);
                    return;
                }
                TalkEx("nongdan");
                MissionState = "TalkToNPCDUA";
                //foreach (GameObject _object in Objects.All)
                //{
                //    if (TDT.VietLien(_object.Name).Contains("nongdan"))
                //    {
                //        Talk(_object.Id);
                //        QDuaState = "TalkToNPCDUA";
                //        return;
                //    }
                //}
            }
            if (MissionState == "TalkToNPCDUA")
            {
                if (TLBB.IsQuestOpen)
                {
                    foreach (QuestFrame dialog in QuestFrame.Enum(this))
                    {
                        if (dialog.Name.Contains("{QLYX_100517_37}"))
                        {
                            QuestFrameOptionClicked(dialog);
                            MissionState = "XongQD";
                            return;
                        }
                    }
                }
                else
                {
                    TalkEx("nongdan");
                    //foreach (GameObject _object in Objects.All)
                    //{
                    //    if (TDT.VietLien(_object.Name).Contains("nongdan"))
                    //    {
                    //        Talk(_object.Id);
                    //        return;
                    //    }
                    //}
                }
            }
            if (MissionState == "XongQD")
            {
                if (!TLBB.IsMapBang(TLBB.MapId))
                {
                    VaoBang();
                    return;
                }
                if (GoToEx(BANG.DoTuBan.X, BANG.DoTuBan.Y))
                {
                    TalkEx("dotuban");
                    //Talk(BANG.DoTuBan.Id);
                    //foreach (GameObject _object in Objects.AllNpc)
                    //{
                    //    if (_object.CleanName == "dotuban")
                    //    {
                    //        if (_object.Id != BANG.DoTuBan.Id)
                    //            Talk(_object.Id);
                    //        break;
                    //    }
                    //}
                    MissionState = "TalkToTraNV";
                    return;
                }
            }

            if (MissionState == "TalkToTraNV")
            {
                if (GoToEx(BANG.DoTuBan.X, BANG.DoTuBan.Y))
                {
                    if (TLBB.IsQuestOpen)
                    {
                        foreach (QuestFrame dialog in QuestFrame.Enum(this))
                        {
                            if (TDT.Contain(dialog.Name, "{QLYX_100517_28}"))
                            {
                                QuestFrameOptionClicked(dialog);
                                MissionState = "XongClick";
                                return;
                            }
                        }
                    }
                    else
                    {
                        TalkEx("dotuban");
                        //Talk(BANG.DoTuBan.Id);
                        //foreach (GameObject _object in Objects.AllNpc)
                        //{
                        //    if (_object.CleanName == "dotuban")
                        //    {
                        //        if (_object.Id != BANG.DoTuBan.Id)
                        //            Talk(_object.Id);
                        //        break;
                        //    }
                        //}
                    }
                }
            }
            if (MissionState == "XongClick")
            {
                LUA.QuestFrameMissionComplete();
                MissionState = "";
                ListDangLumHop.Clear();
                return;
            }

            if (MissionState == "NhatQua")
            {
                if (TLBB.MapId < 500 || (!(TDT.VietLien(TLBB.MapName) == TDT.VietLien(TenThanhKT)) && !Global.BangOnPC.Contains(TenThanhKT)))
                {
                    //if (!(Setting.LoadSetting("ab")[0]>0) || !(User.Beri + User.Berli >= 1000000))
                    if (!(SettingOld.LoadSetting("ab")[0] > 0))
                        return;
                }
                float minDistance = 1000;
                int id = -1;
                float x = 0;
                float y = 0;
                SoHopQua = 0;
                foreach (GameObject _object in Objects.All)
                {
                    if (TDT.VietLien(_object.Name).StartsWith("hopduahau") && _object.IsTaiNguyen)
                    {
                        if (TDT.GetDistance(CharX, CharY, _object.X, _object.Y) < minDistance)
                        {
                            minDistance = TDT.GetDistance(CharX, CharY, _object.X, _object.Y);
                            id = (int)_object.Id;
                            x = _object.X;
                            y = _object.Y;
                        }

                        if (id != -1 && !ListDangLumHop.Contains(id))
                        {
                            if (TDT.GetDistance(CharX, CharY, x, y) > 3)
                            {
                                if (!TLBB.Busy && NhatDoEX.Elapsed.TotalSeconds > 10)
                                {
                                    GoToEx(x, y);
                                    return;
                                }
                            }
                            if (IdleTime > 3)
                            {
                                FixKetMap();
                            }

                            PickItem(id);
                            NhatDoEX = Stopwatch.StartNew();
                            ListDangLumHop.Add(id);
                            return;
                        }
                        SoHopQua++;
                    }
                }

                //if (id != -1)
                //{
                //    if (TDT.GetDistance(CharX, CharY, x, y) > 3)
                //    {
                //        GoTo(x, y, true);
                //        return;
                //    }
                //    if (IdleTime > 3)
                //    {
                //        FixKetMap();
                //    }
                //    PickItem(id);
                //    return;
                //}
                if (!IsMoveEx)
                {
                    FixKetMap();
                    return;
                }
                MoveNext();

                if (SoHopQua == 0)
                {
                    MissionState = "";
                    return;
                }
            }
        }


        public string XayDungInfo = "";
        public int XayDungDanhQuaiTime = 0;
        public void XayDung()
        {
            if (!IsOneSec)
                return;
            if (!(TLBB.MapId > 500 && TDT.GetDistance(CharX, CharY, 100, 55) <= 3))
            {
                if (!IsMoveEx)
                {
                    FixKetMap();
                    return;
                }
            }
            if (TLBB.MapId == 153)
            {
                MissionState = "";
                if (TLBB.IsRide)
                {
                    DownRide();
                    return;
                }
                if (TLBB.PlayerState == 0)
                {
                    if (Objects.Monters.Count == 0 || IdleTime > 2)
                    {
                        MoveNext(POINT.CongDia);
                        return;
                    }
                    else
                    {
                        ForceAttack();
                        return;
                    }
                }
                return;
            }
            if (MissionState == "PickItem")
            {
                MissionState = "";
                return;
            }
            if (MissionState == "")
            {
                if (!TLBB.IsTogleMission)
                {
                    PostMessage(18, 105);
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
                PostMessage(18, 105);
                MissionState = "CloseMission";
                return;
            }
            if (MissionState == "CloseMission")
            {
                PostMessage(6, 105);
                MissionState = "GetXayDungInfo";
                LuaToString();
                return;
            }
            if (MissionState == "GetXayDungInfo")
            {
                XayDungInfo = LuaString();
                if (XayDungInfo == "Chua" || TDT.Contain(XayDungInfo, "batdienthaitue"))
                {
                    if (!TLBB.IsMapBang(TLBB.MapId))
                    {
                        VaoBang();
                        return;
                    }
                    if (TDT.GetDistance(CharX, CharY, 100, 55) > 1)
                    {
                        Move(100, 55);
                        return;
                    }
                    else if (TDT.GetDistance(CharX, CharY, 100, 55) <= 1)
                    {
                        foreach (GameObject _object in Objects.All)
                        {
                            if (TDT.VietLien(_object.Name).Contains("trinhvodanh"))
                            {
                                Talk(_object.Id);
                                MissionState = "TalkToNhanNV";
                                return;
                            }
                        }
                    }
                }
                if (XayDungInfo == "Xong")
                {
                    MissionState = "XongXayDung";
                    return;
                }
                string info = Regex.Replace(XayDungInfo, ".*INFOAIM", "");
                XayDungX = TDT.ParseInt(info.Split(',')[0]);
                XayDungY = TDT.ParseInt(info.Split(',')[1]);
                if (TDT.VietLien(XayDungInfo).Contains("tatma"))
                {
                    XayDungX = 155;
                    XayDungY = 205;
                }
                XayDungMap = TDT.ParseInt(info.Split(',')[2]);
                if (XayDungMap == 0)
                    XayDungMap = LACDUONG.Id;
                if (XayDungX > 0 && XayDungY > 0)
                    MissionState = "DaNhanXayDung";
                else
                    MissionState = "";
                return;
            }
            if (MissionState == "TalkToNhanNV")
            {
                foreach (QuestFrame dialog in QuestFrame.Enum(this))
                {
                    if (TDT.Contain(dialog.Name, "nhiemvuxaydung"))
                    {
                        QuestFrameOptionClicked(dialog);
                        MissionState = "NhanNV";
                        return;
                    }
                }
            }
            if (MissionState == "NhanNV")
            {
                XayDungX = XayDungY = 0;
                foreach (QuestFrame dialog in QuestFrame.Enum(this))
                {
                    if (dialog.Name.Contains("Vào công trường"))
                    {
                        QuestFrameOptionClicked(dialog);
                        MissionState = "DanhQuai";
                        return;
                    }
                    if (dialog.Name.Contains("{DJBHGZ_110622_144}"))
                    {
                        QuestFrameOptionClicked(dialog);
                        MissionState = "DanhQuai";
                        return;
                    }
                }
                MissionState = "";
            }
            if (MissionState == "DaNhanXayDung")
            {
                if (TLBB.MapId > 500)
                {
                    if (XayDungMap == 1)
                        RaBang(1);
                    else if (XayDungMap == 2)
                        RaBang(2);
                    else
                        RaBang(0);
                    return;
                }
                if (TLBB.MapId != XayDungMap)
                {
                    if (PhuIndex(XayDungMap) != -1)
                    {
                        if (TLBB.IsRide)
                        {
                            DownRide();
                            return;
                        }
                        PlayerPackageUseItem(PhuIndex(XayDungMap));
                        return;
                    }
                    if (XayDungMap < 3 && TLBB.MapId > 2)
                    {
                        if (PhuIndex() != -1)
                        {
                            if (TLBB.IsRide)
                            {
                                DownRide();
                                return;
                            }
                            PlayerPackageUseItem(PhuIndex());
                            return;
                        }
                    }
                }
                if (TLBB.MapId != XayDungMap)
                {
                    if (!TLBB.IsRide && TLBB.HaveRide)
                    {
                        UpRide();
                        return;
                    }
                    GoToEx(XayDungX, XayDungY, XayDungMap);
                    return;
                }
                else if (TDT.GetDistance(CharX, CharY, XayDungX, XayDungY) > 4)
                {
                    if (TDT.GetDistance(CharX, CharY, XayDungX, XayDungY) > 30)
                    {
                        if (!TLBB.IsRide && TLBB.HaveRide)
                        {
                            UpRide();
                            return;
                        }
                    }
                    Move(XayDungX, XayDungY);
                }
                else if (TDT.GetDistance(CharX, CharY, XayDungX, XayDungY) <= 4)
                {
                    MissionState = "DaChayDenNoi";
                    return;
                }
            }
            if (MissionState == "DaChayDenNoi")
            {
                if (TLBB.MapId != XayDungMap || TDT.GetDistance(CharX, CharY, XayDungX, XayDungY) > 10)
                {
                    Move(XayDungX, XayDungY, XayDungMap);
                    return;
                }
                foreach (GameObject _object in Objects.All)
                {
                    if (TDT.Contain(XayDungInfo, _object.Name))
                    {
                        if (_object.IsNPC)
                        {
                            Talk(_object.Id);
                            MissionState = "";
                            return;
                        }
                        if (_object.IsMonter)
                        {
                            MissionState = "DanhQuai";
                            XayDungDanhQuaiTime = 0;
                            return;
                        }
                    }
                }
            }
            if (MissionState == "DanhQuai")
            {
                if (TLBB.MapId > 500)
                    return;
                else
                    if (XayDungX == 0)
                {
                    if (IdleTime < 3)
                        return;
                    XayDungX = (int)CharX;
                    XayDungY = (int)CharY;
                }
                if (TDT.GetDistance(CharX, CharY, XayDungX, XayDungY) > 10)
                {
                    Move(XayDungX, XayDungY);
                }
                if (TLBB.IsRide)
                {
                    if (TLBB.PlayerState == 0)
                        DownRide();
                    return;
                }
                if (XayDungDanhQuaiTime++ > 25)
                {
                    XayDungDanhQuaiTime = 0;
                    foreach (var item in PacketItems.All)
                    {
                        if (TDT.Contain(XayDungInfo, item.Name))
                        {
                            MissionState = "";
                            return;
                        }
                        if (item.Type == "TaskTools7_10")
                        {
                            MissionState = "";
                            return;
                        }
                    }
                }
                if (!ForcePickItem())
                    ForceAttack();
                return;
            }
            if (MissionState == "XongXayDung")
            {
                if (!TLBB.IsMapBang(TLBB.MapId))
                {
                    VaoBang();
                    return;
                }
                if (TDT.GetDistance(CharX, CharY, 100, 55) > 1)
                {
                    Move(100, 55);
                }
                else if (TDT.GetDistance(CharX, CharY, 100, 55) <= 1)
                {
                    foreach (GameObject _object in Objects.All)
                    {
                        if (TDT.VietLien(_object.Name).Contains("trinhvodanh"))
                        {
                            Talk(_object.Id);
                            MissionState = "TalkToTraNV";
                            return;
                        }
                    }
                }
            }
            if (MissionState == "TalkToTraNV")
            {
                foreach (QuestFrame dialog in QuestFrame.Enum(this))
                {
                    if (dialog.Name.Contains("Nhiệm vụ xây dựng"))
                    {
                        QuestFrameOptionClicked(dialog);
                        MissionState = "TraNV1";
                        return;
                    }
                }
            }
            if (MissionState == "TraNV1")
            {
                PostMessage(14, 105);
                MissionState = "TraNV2";
                return;
            }
            if (MissionState == "TraNV2")
            {
                PostMessage(15, 105);
                MissionState = "";
                return;
            }
        }

        public int XayDungX;
        public int XayDungY;
        public int XayDungMap;
        public string ChinhTuyenMission { get; set; } = string.Empty;
        public string ChinhTuyenName { get; set; } = string.Empty;
        public GameTask ChinhTuyenTask { get; set; } = null;
    
        public void KsThuongHoi()
        {
            if (GoToEx(LACDUONG.KieuPhucThinh))
            {
                DoStringEx("PlayerShop: CreateShop('" + TDT.RandomString(5) + "', 1)");
                Thread.Sleep(350);
                DoStringEx("PlayerShop: CreateShop('" + TDT.RandomString(5) + "', 2)");
                Thread.Sleep(350);
            }
        }
        public bool AtkBack()
        {
            foreach (GameObject obj in Objects.All)
            {
                if (obj.AtkToId == SelfId && !obj.IsPlayer && obj.IsMonter)
                {
                    if (obj.HP > 0 && obj.GetDistance(RoundX, RoundY) <= 5)
                    {
                        SelectTarget((int)obj.Id);
                        SendKey(Global.BaseSkill);
                        return true;
                    }
                }
            }
            return false;
        }
        public string ChienBiName { get; set; } = string.Empty;
        public string ChienBiPoint { get; set; } = string.Empty;
        public void ChienBi()
        {
            if (!IsOneSec)
                return;
            if (ForcePickItem())
                return;
            if (TLBB.PlayerState == 5)
                return;

            if (SecCount % 20 == 0 && IdleTime > 10)
            {
                MissionState = "";
            }

            if (TLBB.MapId < 663 || TLBB.MapId > 667)
            {
                GoToEx(VIEMLATHIEN.PhongBoQuy);
                return;
            }
            else
            {
                if (RoundX >= 96 && RoundX <= 276 && RoundY >= 96 && RoundY <= 276)
                {
                    if (GoToEx(VIEMLATHIEN.PhongBoQuy))
                    {
                        foreach (GameObject obj in Objects.All)
                        {
                            if (TDT.VietLien(obj.Title).Contains("chienbiquan"))
                            {
                                Talk(obj.Id);
                                break;
                            }
                        }
                        Talk("phongboquy");
                        Thread.Sleep(1000);
                        QuestFrame.Click("#{MZZBQ_150811_466}");
                        Thread.Sleep(1000);
                        Talk("phongboquy");
                        Thread.Sleep(1000);
                        QuestFrame.Click("#{MZZBQ_150811_467}");
                        Thread.Sleep(1000);
                    }
                    return;
                }
            }

            foreach (GameTask task in GameTask.Enum(this))
            {
                if (GAMEDIC.NhiemVuChienBi.Contains(task.Name))
                {
                    if (task.Completed)
                    {
                        MissionState = "Done";
                    }
                    break;
                }
            }

            if (MissionState == "")
            {
                if (!TLBB.IsTogleMission)
                {
                    PostMessage(18, 105);
                }
                MissionState = "OpenMission";
                return;
            }
            if (MissionState == "Do")
            {
                if (ChienBiPoint.Split(',').Length != 3 || TDT.ParseInt(ChienBiPoint.Split(',')[0]) == 0 || TDT.ParseInt(ChienBiPoint.Split(',')[1]) == 0)
                {
                    MissionState = "";
                }
                else
                {
                    ChienBiPoint = ChienBiPoint.Replace("291,134", "295,137");
                    ChienBiPoint = ChienBiPoint.Replace("342,150", "345,150");
                    if (GoToEx(ChienBiPoint))
                    {
                        // #{MZZBQ_150811_333}
                        if (ChienBiName == "#{MZZBQ_150811_331}" || ChienBiName == "#{MZZBQ_150811_333}") // gửi thư
                        {
                            if (!TLBB.IsQuestOpen)
                            {
                                foreach (GameObject _object in Objects.All)
                                {
                                    if (_object.Menpai == 47 && _object.GetDistance(RoundX, RoundY) < 5)
                                    {
                                        Talk(_object.Id);
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                QuestFrame.ClickAll();
                                QuestFrame.Close();
                            }
                        }
                        else
                        {
                            if (TLBB.IsRide)
                            {
                                DownRide();
                                return;
                            }
                        }
                        //#{MZZBQ_150811_332}
                        // #{MZZBQ_150811_330}
                        // #{MZZBQ_150811_335} Gửi Thư Khuyên Hàng CircularTaskTool70_15 => 299,157
                        if (ChienBiName == "#{MZZBQ_150811_334}") // đốt kho thóc
                        {
                            if (!AtkBack())
                            {
                                foreach (GameObject _object in Objects.All)
                                {
                                    if (_object.CleanName.Contains("khothoc") && _object.Menpai > 40)
                                    {
                                        SelectTarget((int)_object.Id);
                                        break;
                                    }
                                }
                                Thread.Sleep(1000);
                                foreach (var item in PacketItems.All)
                                {
                                    if (item.Type == "CircularTaskTool70_16")
                                    {
                                        item.DoSubAction();
                                        Thread.Sleep(1000);
                                        break;
                                    }
                                }
                            }
                        }
                        if (ChienBiName == "#{MZZBQ_150811_332}") // xây vương kỳ
                        {
                            if (!AtkBack())
                            {
                                foreach (GameObject _object in Objects.All)
                                {
                                    if (_object.CleanName.Contains("vuongky") && _object.Menpai > 40)
                                    {
                                        SelectTarget((int)_object.Id);
                                        break;
                                    }
                                }
                                Thread.Sleep(1000);
                                foreach (var item in PacketItems.All)
                                {
                                    if (item.Type == "Merchandise1_13")
                                    {
                                        item.DoSubAction();
                                        Thread.Sleep(1000);
                                        break;
                                    }
                                }
                            }
                        }
                        if (ChienBiName == "#{MZZBQ_150811_330}") // cắm cờ
                        {
                            if (!AtkBack())
                            {
                                foreach (var item in PacketItems.All)
                                {
                                    if (item.Type == "CircularTaskTool70_12")
                                    {
                                        item.DoSubAction();
                                        Thread.Sleep(1000);
                                        break;
                                    }
                                }
                            }
                        }
                        if (ChienBiName == "#{MZZBQ_150811_335}") // gửi thư
                        {
                            if (!AtkBack())
                            {
                                foreach (var item in PacketItems.All)
                                {
                                    if (item.Type == "CircularTaskTool70_15")
                                    {
                                        item.DoSubAction();
                                        Thread.Sleep(1000);
                                        break;
                                    }
                                }
                            }
                        }
                        if (ChienBiName == "Diệt Thủ Vệ") // gửi thư
                        {
                            ForceAttack();
                        }

                        //#{MZZBQ_150811_331}
                    }
                }
                return;
            }
            if (MissionState == "OpenMission")
            {
                PostMessage(18, 105);
                MissionState = "CloseMission";
                return;
            }
            if (MissionState == "CloseMission")
            {
                foreach (GameTask task in GameTask.Enum(this))
                {
                    if (GAMEDIC.NhiemVuChienBi.Contains(task.Name))
                    {
                        if (task.Completed)
                        {
                            MissionState = "Done";
                            return;
                        }
                        else
                        {
                            MissionState = "Do";
                            ChienBiName = task.Name;
                            ChienBiPoint = "";
                            //string lua = "nSelIndex = -1; cnt = 0; while true do name, content = DataPool:GetPlayerMission_Memo(cnt);   if string.find(name, 'Diệt Thủ Vệ') then if DataPool:GetPlayerMission_Variable(cnt, 0) == 1 then return 'Xong';     else nSelIndex = cnt;       PushDebugMessage(nSelIndex); 			DietThuVe(nSelIndex);       break;   end end cnt = cnt + 1;   if cnt == 20 then return 'Chua';   end end if nSelIndex == -1 then return '-1'; end PosX = 0; PosZ = 0; toScene = 0; str = ''; MissType = DataPool:GetPlayerMission_Variable( nSelIndex, 6 ); nXZZMId = DataPool:GetPlayerMission_Variable( nSelIndex, 5 ); nTargetIndex = DataPool:GetPlayerMission_Variable( nSelIndex, 2 ); nSelfZMId_temp = DataPool:GetPlayerMission_Variable( nSelIndex, 4 ); nSelfZMId = math.floor(nSelfZMId_temp/10000); nAcceptNpcIndex = math.mod(nSelfZMId_temp, 10000); AllianceNpcNameList = { [0] = { [1] = {xpos=368, zpos=10, name='Tiền Phong Doanh Địa Chiến Bị Quan-Đông Bắc'}, [2] = {xpos=373, zpos=370, name='Tiền Phong Doanh Địa Chiến Bị Quan-Đông Nam'}, [3] = {xpos=15, zpos=371, name='Tiền Phong Doanh Địa Chiến Bị Quan-Tây Nam'}, [4] = {xpos=9, zpos=11, name='Tiền Phong Doanh Địa Chiến Bị Quan-Tây Bắc'}, }, [1] = { [1] = {xpos=368, zpos=10, name='Tiền Phong Doanh Địa Chiến Bị Quan-Đông Bắc'}, [2] = {xpos=373, zpos=370, name='Tiền Phong Doanh Địa Chiến Bị Quan-Đông Nam'}, [3] = {xpos=15, zpos=371, name='Tiền Phong Doanh Địa Chiến Bị Quan-Tây Nam'}, [4] = {xpos=9, zpos=11, name='Tiền Phong Doanh Địa Chiến Bị Quan-Tây Bắc'}, }, [2] = { [1] = {xpos=368, zpos=10, name='Tiền Phong Doanh Địa Chiến Bị Quan-Đông Bắc'}, [2] = {xpos=373, zpos=370, name='Tiền Phong Doanh Địa Chiến Bị Quan-Đông Nam'}, [3] = {xpos=15, zpos=371, name='Tiền Phong Doanh Địa Chiến Bị Quan-Tây Nam'}, [4] = {xpos=9, zpos=11, name='Tiền Phong Doanh Địa Chiến Bị Quan-Tây Bắc'}, }, [3] = { [1] = {xpos=368, zpos=10, name='Tiền Phong Doanh Địa Chiến Bị Quan-Đông Bắc'}, [2] = {xpos=373, zpos=370, name='Tiền Phong Doanh Địa Chiến Bị Quan-Đông Nam'}, [3] = {xpos=15, zpos=371, name='Tiền Phong Doanh Địa Chiến Bị Quan-Tây Nam'}, [4] = {xpos=9, zpos=11, name='Tiền Phong Doanh Địa Chiến Bị Quan-Tây Bắc'}, }, [4] = { [1] = {xpos=368, zpos=10, name='Tiền Phong Doanh Địa Chiến Bị Quan-Đông Bắc'}, [2] = {xpos=373, zpos=370, name='Tiền Phong Doanh Địa Chiến Bị Quan-Đông Nam'}, [3] = {xpos=15, zpos=371, name='Tiền Phong Doanh Địa Chiến Bị Quan-Tây Nam'}, [4] = {xpos=9, zpos=11, name='Tiền Phong Doanh Địa Chiến Bị Quan-Tây Bắc'}, }, } AllianceSceneList = { [ 0 ] = 663, [ 1 ] = 664, [ 2 ] = 665, [ 3 ] = 666, [ 4 ] = 667, } AllianceName_list = { [0] = 'Thiên Thu Điện', [1] = 'Bất Quy Lâm', [2] = 'Vô Nhai Hải', [3] = 'Viêm La Thiên', [4] = 'Ngọc Hoàng Sơn', } targetZMName = ''; toScene = 0; selfScene = 0; missNpcName = ''; acceptxPos = 0; acceptzPos = 0;  if nXZZMId >= 0 and nXZZMId <= 4 then targetZMName = AllianceName_list[nXZZMId];   toScene = AllianceSceneList[nXZZMId];   selfScene = AllianceSceneList[nXZZMId]; end if nSelfZMId >= 0 and nSelfZMId <= 4 and nAcceptNpcIndex > 0 and nAcceptNpcIndex <= 4 then AcceptMisInfo = AllianceNpcNameList[nSelfZMId];   missNpcName = AcceptMisInfo[nAcceptNpcIndex].name;   acceptxPos = AcceptMisInfo[nAcceptNpcIndex].xpos;   acceptzPos = AcceptMisInfo[nAcceptNpcIndex].zpos; end if MissType == 1 then DestPosList = { [1] = {xlefttoppos = 129, zlefttoppos = 80, xrightbottompos = 150, zrightbottompos = 91, tipposx = 139, tipposz = 85}, [2] = {xlefttoppos = 222, zlefttoppos = 83, xrightbottompos = 236, zrightbottompos = 92, tipposx = 229, tipposz = 87}, [3] = {xlefttoppos = 286, zlefttoppos = 124, xrightbottompos = 296, zrightbottompos = 144, tipposx = 291, tipposz = 134}, [4] = {xlefttoppos = 284, zlefttoppos = 223, xrightbottompos = 294, zrightbottompos = 243, tipposx = 289, tipposz = 233}, [5] = {xlefttoppos = 220, zlefttoppos = 285, xrightbottompos = 240, zrightbottompos = 295, tipposx = 230, tipposz = 290}, [6] = {xlefttoppos = 144, zlefttoppos = 286, xrightbottompos = 164, zrightbottompos = 296, tipposx = 154, tipposz = 291}, [7] = {xlefttoppos = 84, zlefttoppos = 217, xrightbottompos = 94, zrightbottompos = 237, tipposx = 89, tipposz = 227}, [8] = {xlefttoppos = 84, zlefttoppos = 151, xrightbottompos = 94, zrightbottompos = 171, tipposx = 89, tipposz = 161}, } if nTargetIndex >= 1 and nTargetIndex <= 8 then PosX = DestPosList[nTargetIndex].tipposx;   PosZ = DestPosList[nTargetIndex].tipposz; end str = ScriptGlobal_Format('#{MZZBQ_150811_336}', targetZMName,PosX,PosZ,toScene); elseif MissType == 2 then ZhanBeiNpc_Info = { [1] = {npcid = 43906, poslist = {[1] = {xpos=147, zpos=35}, [2] = {xpos=230, zpos=39}, [3] = {xpos=342, zpos=150}, [4] = {xpos=349, zpos=224}, [5] = {xpos=232, zpos=346}, [6] = {xpos=162, zpos=349}, [7] = {xpos=33, zpos=232}, [8] = {xpos=30, zpos=160}}, baseai=3, aiScript=-1, scriptid=891451, dir=0}, [2] = {npcid = 43907, poslist = {[1] = {xpos=159, zpos=66}, [2] = {xpos=231, zpos=60}, [3] = {xpos=314, zpos=150}, [4] = {xpos=311, zpos=232}, [5] = {xpos=222, zpos=312}, [6] = {xpos=150, zpos=317}, [7] = {xpos=63, zpos=230}, [8] = {xpos=62, zpos=149}}, baseai=3, aiScript=-1, scriptid=891452, dir=0}, [3] = {npcid = 43908, poslist = {[1] = {xpos=312, zpos=62}, [2] = {xpos=315, zpos=311}, [3] = {xpos=67, zpos=314},[4] = {xpos=64, zpos=68}}, baseai=3, aiScript=-1, scriptid=891453, dir=0}, [4] = {npcid = 43909, poslist = {[1] = {xpos=82, zpos=134}, [2] = {xpos=84, zpos=246}, [3] = {xpos=143, zpos=298}, [4] = {xpos=241, zpos=305}, [5] = {xpos=287, zpos=244}, [6] = {xpos=297, zpos=133}, [7] = {xpos=240, zpos=78}, [8] = {xpos=133, zpos=82}}, baseai=3, aiScript=-1, scriptid=891454, dir=0}, } if nTargetIndex >= 1 and nTargetIndex <= 8 then TempData = ZhanBeiNpc_Info[1].poslist;   PosX = TempData[nTargetIndex].xpos;   PosZ = TempData[nTargetIndex].zpos; end str = ScriptGlobal_Format('#{MZZBQ_150811_337}', targetZMName,PosX,PosZ,toScene); elseif MissType == 3 then ZhanBeiNpc_Info = { [1] = {npcid = 43906, poslist = {[1] = {xpos=147, zpos=35}, [2] = {xpos=230, zpos=39}, [3] = {xpos=342, zpos=150}, [4] = {xpos=349, zpos=224}, [5] = {xpos=232, zpos=346}, [6] = {xpos=162, zpos=349}, [7] = {xpos=33, zpos=232}, [8] = {xpos=30, zpos=160}}, baseai=3, aiScript=-1, scriptid=891451, dir=0}, [2] = {npcid = 43907, poslist = {[1] = {xpos=159, zpos=66}, [2] = {xpos=231, zpos=60}, [3] = {xpos=314, zpos=150}, [4] = {xpos=311, zpos=232}, [5] = {xpos=222, zpos=312}, [6] = {xpos=150, zpos=317}, [7] = {xpos=63, zpos=230}, [8] = {xpos=62, zpos=149}}, baseai=3, aiScript=-1, scriptid=891452, dir=0}, [3] = {npcid = 43908, poslist = {[1] = {xpos=312, zpos=62}, [2] = {xpos=315, zpos=311}, [3] = {xpos=67, zpos=314},[4] = {xpos=64, zpos=68}}, baseai=3, aiScript=-1, scriptid=891453, dir=0}, [4] = {npcid = 43909, poslist = {[1] = {xpos=82, zpos=134}, [2] = {xpos=84, zpos=246}, [3] = {xpos=143, zpos=298}, [4] = {xpos=241, zpos=305}, [5] = {xpos=287, zpos=244}, [6] = {xpos=297, zpos=133}, [7] = {xpos=240, zpos=78}, [8] = {xpos=133, zpos=82}}, baseai=3, aiScript=-1, scriptid=891454, dir=0}, } if nTargetIndex >= 1 and nTargetIndex <= 4 then TempData = ZhanBeiNpc_Info[3].poslist;   PosX = TempData[nTargetIndex].xpos;   PosZ = TempData[nTargetIndex].zpos; end str = ScriptGlobal_Format('#{MZZBQ_150811_338}', targetZMName,PosX,PosZ,toScene); elseif MissType == 4 then ZhanBeiNpc_Info = { [1] = {npcid = 43906, poslist = {[1] = {xpos=147, zpos=35}, [2] = {xpos=230, zpos=39}, [3] = {xpos=342, zpos=150}, [4] = {xpos=349, zpos=224}, [5] = {xpos=232, zpos=346}, [6] = {xpos=162, zpos=349}, [7] = {xpos=33, zpos=232}, [8] = {xpos=30, zpos=160}}, baseai=3, aiScript=-1, scriptid=891451, dir=0}, [2] = {npcid = 43907, poslist = {[1] = {xpos=159, zpos=66}, [2] = {xpos=231, zpos=60}, [3] = {xpos=314, zpos=150}, [4] = {xpos=311, zpos=232}, [5] = {xpos=222, zpos=312}, [6] = {xpos=150, zpos=317}, [7] = {xpos=63, zpos=230}, [8] = {xpos=62, zpos=149}}, baseai=3, aiScript=-1, scriptid=891452, dir=0}, [3] = {npcid = 43908, poslist = {[1] = {xpos=312, zpos=62}, [2] = {xpos=315, zpos=311}, [3] = {xpos=67, zpos=314},[4] = {xpos=64, zpos=68}}, baseai=3, aiScript=-1, scriptid=891453, dir=0}, [4] = {npcid = 43909, poslist = {[1] = {xpos=82, zpos=134}, [2] = {xpos=84, zpos=246}, [3] = {xpos=143, zpos=298}, [4] = {xpos=241, zpos=305}, [5] = {xpos=287, zpos=244}, [6] = {xpos=297, zpos=133}, [7] = {xpos=240, zpos=78}, [8] = {xpos=133, zpos=82}}, baseai=3, aiScript=-1, scriptid=891454, dir=0}, } if nTargetIndex >= 1 and nTargetIndex <= 8 then TempData = ZhanBeiNpc_Info[2].poslist;   PosX = TempData[nTargetIndex].xpos;   PosZ = TempData[nTargetIndex].zpos; end str = ScriptGlobal_Format('#{MZZBQ_150811_339}', targetZMName,PosX,PosZ,toScene); elseif MissType == 5 then ZhanBeiNpc_Info = { [1] = {npcid = 43906, poslist = {[1] = {xpos=147, zpos=35}, [2] = {xpos=230, zpos=39}, [3] = {xpos=342, zpos=150}, [4] = {xpos=349, zpos=224}, [5] = {xpos=232, zpos=346}, [6] = {xpos=162, zpos=349}, [7] = {xpos=33, zpos=232}, [8] = {xpos=30, zpos=160}}, baseai=3, aiScript=-1, scriptid=891451, dir=0}, [2] = {npcid = 43907, poslist = {[1] = {xpos=159, zpos=66}, [2] = {xpos=231, zpos=60}, [3] = {xpos=314, zpos=150}, [4] = {xpos=311, zpos=232}, [5] = {xpos=222, zpos=312}, [6] = {xpos=150, zpos=317}, [7] = {xpos=63, zpos=230}, [8] = {xpos=62, zpos=149}}, baseai=3, aiScript=-1, scriptid=891452, dir=0}, [3] = {npcid = 43908, poslist = {[1] = {xpos=312, zpos=62}, [2] = {xpos=315, zpos=311}, [3] = {xpos=67, zpos=314},[4] = {xpos=64, zpos=68}}, baseai=3, aiScript=-1, scriptid=891453, dir=0}, [4] = {npcid = 43909, poslist = {[1] = {xpos=82, zpos=134}, [2] = {xpos=84, zpos=246}, [3] = {xpos=143, zpos=298}, [4] = {xpos=241, zpos=305}, [5] = {xpos=287, zpos=244}, [6] = {xpos=297, zpos=133}, [7] = {xpos=240, zpos=78}, [8] = {xpos=133, zpos=82}}, baseai=3, aiScript=-1, scriptid=891454, dir=0}, } if nTargetIndex >= 1 and nTargetIndex <= 8 then TempData = ZhanBeiNpc_Info[4].poslist;   PosX = TempData[nTargetIndex].xpos;   PosZ = TempData[nTargetIndex].zpos; end str = ScriptGlobal_Format('#{MZZBQ_150811_340}', targetZMName,PosX,PosZ,toScene); elseif MissType == 6 then TargetPosInfo = { [1] = {xpos = 78, zpos = 162}, [2] = {xpos = 80, zpos = 241}, [3] = {xpos = 157, zpos = 299}, [4] = {xpos = 223, zpos = 297}, [5] = {xpos = 300, zpos = 220}, [6] = {xpos = 299, zpos = 157}, [7] = {xpos = 230, zpos = 80}, [8] = {xpos = 146, zpos = 83}, } if nTargetIndex >= 1 and nTargetIndex <= 8 then PosX = TargetPosInfo[nTargetIndex].xpos;   PosZ = TargetPosInfo[nTargetIndex].zpos; end str = ScriptGlobal_Format('#{MZZBQ_150811_405}', targetZMName,PosX,PosZ,toScene); end return PosX .. ',' .. PosZ .. ',' .. toScene; function DietThuVe(nSceneId) local nDirection = DataPool:GetPlayerMission_Variable( nSelIndex, 2 );	 	local nCombination = DataPool:GetPlayerMission_Variable( nSelIndex, 4 );	 	local nXZZMId = DataPool:GetPlayerMission_Variable( nSelIndex, 5 );			 	local nSubMission = DataPool:GetPlayerMission_Variable( nSelIndex, 6 );		 	if ( nDirection < 1 or nDirection > 4 ) then 		return ''; 	end 	local nSelfAllianceId = math.floor( nCombination / 10000 ); 	if ( nSelfAllianceId < 0 or nSelfAllianceId > 4 ) then 		return ''; 	end 	local npcIndex = math.mod( nCombination, 10000 ); 	if ( npcIndex < 1 or npcIndex > 4 ) then 		return ''; 	end 	if ( nXZZMId < 0 or nXZZMId > 4 ) then 		return ''; 	end 	if ( nSubMission ~= 7 ) then 		return ''; 	end local aryAllianceScene = { 		[ 0 ] = 663, 		[ 1 ] = 664, 		[ 2 ] = 665, 		[ 3 ] = 666, 		[ 4 ] = 667, 	}; 	local aryAllianceName = { 		[0] = 'Thiên Thu Ði®n', 		[1] = 'B¤t Quy Lâm', 		[2] = 'Vô Nhai Häi', 		[3] = 'Viêm La Thiên', 		[4] = 'Ng÷c Hoàng S½n', 	}; 	local aryDirection = { 'Ðông', 'Tây', 'Nam', 'B¡c' }; local aryCaptains = { 		[ 0 ] = { 			{ NpcId = 44603, Pos = { 282, 189 }, MissionPos = { 299, 189 } },	 			{ NpcId = 44604, Pos = { 99, 189 }, MissionPos = { 77, 180 } },		 			{ NpcId = 44605, Pos = { 189, 282 }, MissionPos = { 190, 300 } },	 			{ NpcId = 44606, Pos = { 189, 97 }, MissionPos = { 189, 78 } },		 		}, 		[ 1 ] = { 			{ NpcId = 44613, Pos = { 282, 189 }, MissionPos = { 299, 189 } },	 			{ NpcId = 44614, Pos = { 99, 189 }, MissionPos = { 77, 180 } },	 			{ NpcId = 44615, Pos = { 189, 282 }, MissionPos = { 190, 300 } },	 			{ NpcId = 44616, Pos = { 189, 97 }, MissionPos = { 189, 78 } },		 		}, 		[ 2 ] = { 			{ NpcId = 44623, Pos = { 282, 189 }, MissionPos = { 299, 189 } },	 			{ NpcId = 44624, Pos = { 99, 189 }, MissionPos = { 77, 180 } },		 			{ NpcId = 44625, Pos = { 189, 282 }, MissionPos = { 190, 300 } },	 			{ NpcId = 44626, Pos = { 189, 97 }, MissionPos = { 189, 78 } },		 		}, 		[ 3 ] = { 			{ NpcId = 44633, Pos = { 282, 189 }, MissionPos = { 299, 189 } },	 			{ NpcId = 44634, Pos = { 99, 189 }, MissionPos = { 77, 180 } },		 			{ NpcId = 44635, Pos = { 189, 282 }, MissionPos = { 190, 300 } }, 			{ NpcId = 44636, Pos = { 189, 97 }, MissionPos = { 189, 78 } },	 		}, 		[ 4 ] = { 			{ NpcId = 44643, Pos = { 282, 189 }, MissionPos = { 299, 189 } },	 			{ NpcId = 44644, Pos = { 99, 189 }, MissionPos = { 77, 180 } },	 			{ NpcId = 44645, Pos = { 189, 282 }, MissionPos = { 190, 300 } }, 			{ NpcId = 44646, Pos = { 189, 97 }, MissionPos = { 189, 78 } },	 		}, 	}; 	 	local aryAllianceNpcName = { 			[0] = 'Løc Thi¬n Thu', 			[1] = 'Løc BÕc Quy', 			[2] = 'Løc Ngû Nhai', 			[3] = 'Løc NgÕn La', 			[4] = 'Løc HuÏnh S½n', 		};	 local aryZhanBeiNpc_Ex = { 		[0] = { 				[1] = {npcid = 44599, xpos=368, zpos=10, name='Ti«n Phong Doanh Ð¸a Chiªn B¸ Quan-Ðông B¡c' }, 				[2] = {npcid = 44600, xpos=373, zpos=370, name='Ti«n Phong Doanh Ð¸a Chiªn B¸ Quan-Ðông Nam' }, 				[3] = {npcid = 44601, xpos=15, zpos=371, name='Ti«n Phong Doanh Ð¸a Chiªn B¸ Quan-Tây Nam' }, 	 				[4] = {npcid = 44602, xpos=9, zpos=11, name='Ti«n Phong Doanh Ð¸a Chiªn B¸ Quan-Tây B¡c' }, 	 			 }, 		[1] = { 				[1] = {npcid = 44609, xpos=368, zpos=10, name='Ti«n Phong Doanh Ð¸a Chiªn B¸ Quan-Ðông B¡c' }, 				[2] = {npcid = 44610, xpos=373, zpos=370, name='Ti«n Phong Doanh Ð¸a Chiªn B¸ Quan-Ðông Nam' }, 				[3] = {npcid = 44611, xpos=15, zpos=371, name='Ti«n Phong Doanh Ð¸a Chiªn B¸ Quan-Tây Nam' }, 	 				[4] = {npcid = 44612, xpos=9, zpos=11, name='Ti«n Phong Doanh Ð¸a Chiªn B¸ Quan-Tây B¡c' }, 	 			 }, 		[2] = { 				[1] = {npcid = 44619, xpos=368, zpos=10, name='Ti«n Phong Doanh Ð¸a Chiªn B¸ Quan-Ðông B¡c' }, 				[2] = {npcid = 44620, xpos=373, zpos=370, name='Ti«n Phong Doanh Ð¸a Chiªn B¸ Quan-Ðông Nam' }, 				[3] = {npcid = 44621, xpos=15, zpos=371, name='Ti«n Phong Doanh Ð¸a Chiªn B¸ Quan-Tây Nam' }, 	 				[4] = {npcid = 44622, xpos=9, zpos=11, name='Ti«n Phong Doanh Ð¸a Chiªn B¸ Quan-Tây B¡c' }, 	 			 }, 		[3] = { 				[1] = {npcid = 44629, xpos=368, zpos=10, name='Ti«n Phong Doanh Ð¸a Chiªn B¸ Quan-Ðông B¡c' }, 	 				[2] = {npcid = 44630, xpos=373, zpos=370, name='Ti«n Phong Doanh Ð¸a Chiªn B¸ Quan-Ðông Nam' }, 				[3] = {npcid = 44631, xpos=15, zpos=371, name='Ti«n Phong Doanh Ð¸a Chiªn B¸ Quan-Tây Nam' }, 	 				[4] = {npcid = 44632, xpos=9, zpos=11, name='Ti«n Phong Doanh Ð¸a Chiªn B¸ Quan-Tây B¡c' }, 	 			 }, 		[4] = { 				[1] = {npcid = 44639, xpos=368, zpos=10, name='Ti«n Phong Doanh Ð¸a Chiªn B¸ Quan-Ðông B¡c' }, 	 				[2] = {npcid = 44640, xpos=373, zpos=370, name='Ti«n Phong Doanh Ð¸a Chiªn B¸ Quan-Ðông Nam' }, 				[3] = {npcid = 44641, xpos=15, zpos=371, name='Ti«n Phong Doanh Ð¸a Chiªn B¸ Quan-Tây Nam' }, 	 				[4] = {npcid = 44642, xpos=9, zpos=11, name='Ti«n Phong Doanh Ð¸a Chiªn B¸ Quan-Tây B¡c' }, 	 			 }, 	};	 	 	local szAllianceName = aryAllianceName[ nXZZMId ]; 	local szDirectionName = aryDirection[ nDirection ]; 	local aryPos = aryCaptains[ nXZZMId ][ nDirection ].MissionPos; 	local nSceneId = aryAllianceScene[ nXZZMId ]; 	local strText = ScriptGlobal_Format( '#{MZZBQ_150811_531}', szAllianceName, szDirectionName, aryPos[ 1 ], aryPos[ 2 ], nSceneId, szDirectionName ); 	return aryPos[ 1 ] .. ',' .. aryPos[ 2 ] .. ',' .. nSceneId; end  ";
                            //if (task.Name == "----")
                            //{
                            //    LuaDoOneLineString(lua);
                            //}
                            //else
                            //{
                            //    LuaDoOneLineString("nSelIndex = -1; cnt = 0; while true do name, content = DataPool:GetPlayerMission_Memo(cnt); if string.find(name, '" + task.Name + "') then if DataPool:GetPlayerMission_Variable(cnt, 0) == 1 then return 'Xong'; else nSelIndex = cnt; PushDebugMessage(nSelIndex); break; end end cnt = cnt + 1; if cnt == 20 then return 'Chua'; end end if nSelIndex == -1 then return '-1'; end PosX = 0; PosZ = 0; toScene = 0; str = ''; MissType = DataPool:GetPlayerMission_Variable( nSelIndex, 6 ); nXZZMId = DataPool:GetPlayerMission_Variable( nSelIndex, 5 ); nTargetIndex = DataPool:GetPlayerMission_Variable( nSelIndex, 2 ); nSelfZMId_temp = DataPool:GetPlayerMission_Variable( nSelIndex, 4 ); nSelfZMId = math.floor(nSelfZMId_temp/10000); nAcceptNpcIndex = math.mod(nSelfZMId_temp, 10000); AllianceNpcNameList = { [0] = { [1] = {xpos=368, zpos=10, name='Tiền Phong Doanh Địa Chiến Bị Quan-Đông Bắc'}, [2] = {xpos=373, zpos=370, name='Tiền Phong Doanh Địa Chiến Bị Quan-Đông Nam'}, [3] = {xpos=15, zpos=371, name='Tiền Phong Doanh Địa Chiến Bị Quan-Tây Nam'}, [4] = {xpos=9, zpos=11, name='Tiền Phong Doanh Địa Chiến Bị Quan-Tây Bắc'}, }, [1] = { [1] = {xpos=368, zpos=10, name='Tiền Phong Doanh Địa Chiến Bị Quan-Đông Bắc'}, [2] = {xpos=373, zpos=370, name='Tiền Phong Doanh Địa Chiến Bị Quan-Đông Nam'}, [3] = {xpos=15, zpos=371, name='Tiền Phong Doanh Địa Chiến Bị Quan-Tây Nam'}, [4] = {xpos=9, zpos=11, name='Tiền Phong Doanh Địa Chiến Bị Quan-Tây Bắc'}, }, [2] = { [1] = {xpos=368, zpos=10, name='Tiền Phong Doanh Địa Chiến Bị Quan-Đông Bắc'}, [2] = {xpos=373, zpos=370, name='Tiền Phong Doanh Địa Chiến Bị Quan-Đông Nam'}, [3] = {xpos=15, zpos=371, name='Tiền Phong Doanh Địa Chiến Bị Quan-Tây Nam'}, [4] = {xpos=9, zpos=11, name='Tiền Phong Doanh Địa Chiến Bị Quan-Tây Bắc'}, }, [3] = { [1] = {xpos=368, zpos=10, name='Tiền Phong Doanh Địa Chiến Bị Quan-Đông Bắc'}, [2] = {xpos=373, zpos=370, name='Tiền Phong Doanh Địa Chiến Bị Quan-Đông Nam'}, [3] = {xpos=15, zpos=371, name='Tiền Phong Doanh Địa Chiến Bị Quan-Tây Nam'}, [4] = {xpos=9, zpos=11, name='Tiền Phong Doanh Địa Chiến Bị Quan-Tây Bắc'}, }, [4] = { [1] = {xpos=368, zpos=10, name='Tiền Phong Doanh Địa Chiến Bị Quan-Đông Bắc'}, [2] = {xpos=373, zpos=370, name='Tiền Phong Doanh Địa Chiến Bị Quan-Đông Nam'}, [3] = {xpos=15, zpos=371, name='Tiền Phong Doanh Địa Chiến Bị Quan-Tây Nam'}, [4] = {xpos=9, zpos=11, name='Tiền Phong Doanh Địa Chiến Bị Quan-Tây Bắc'}, }, } AllianceSceneList = { [ 0 ] = 663, [ 1 ] = 664, [ 2 ] = 665, [ 3 ] = 666, [ 4 ] = 667, } AllianceName_list = { [0] = 'Thiên Thu Điện', [1] = 'Bất Quy Lâm', [2] = 'Vô Nhai Hải', [3] = 'Viêm La Thiên', [4] = 'Ngọc Hoàng Sơn', } targetZMName = ''; toScene = 0; selfScene = 0; missNpcName = ''; acceptxPos = 0; acceptzPos = 0; if nXZZMId >= 0 and nXZZMId <= 4 then targetZMName = AllianceName_list[nXZZMId]; toScene = AllianceSceneList[nXZZMId]; selfScene = AllianceSceneList[nXZZMId]; end if nSelfZMId >= 0 and nSelfZMId <= 4 and nAcceptNpcIndex > 0 and nAcceptNpcIndex <= 4 then AcceptMisInfo = AllianceNpcNameList[nSelfZMId]; missNpcName = AcceptMisInfo[nAcceptNpcIndex].name; acceptxPos = AcceptMisInfo[nAcceptNpcIndex].xpos; acceptzPos = AcceptMisInfo[nAcceptNpcIndex].zpos; end if MissType == 1 then DestPosList = { [1] = {xlefttoppos = 129, zlefttoppos = 80, xrightbottompos = 150, zrightbottompos = 91, tipposx = 139, tipposz = 85}, [2] = {xlefttoppos = 222, zlefttoppos = 83, xrightbottompos = 236, zrightbottompos = 92, tipposx = 229, tipposz = 87}, [3] = {xlefttoppos = 286, zlefttoppos = 124, xrightbottompos = 296, zrightbottompos = 144, tipposx = 291, tipposz = 134}, [4] = {xlefttoppos = 284, zlefttoppos = 223, xrightbottompos = 294, zrightbottompos = 243, tipposx = 289, tipposz = 233}, [5] = {xlefttoppos = 220, zlefttoppos = 285, xrightbottompos = 240, zrightbottompos = 295, tipposx = 230, tipposz = 290}, [6] = {xlefttoppos = 144, zlefttoppos = 286, xrightbottompos = 164, zrightbottompos = 296, tipposx = 154, tipposz = 291}, [7] = {xlefttoppos = 84, zlefttoppos = 217, xrightbottompos = 94, zrightbottompos = 237, tipposx = 89, tipposz = 227}, [8] = {xlefttoppos = 84, zlefttoppos = 151, xrightbottompos = 94, zrightbottompos = 171, tipposx = 89, tipposz = 161}, } if nTargetIndex >= 1 and nTargetIndex <= 8 then PosX = DestPosList[nTargetIndex].tipposx; PosZ = DestPosList[nTargetIndex].tipposz; end str = ScriptGlobal_Format('#{MZZBQ_150811_336}', targetZMName,PosX,PosZ,toScene); elseif MissType == 2 then ZhanBeiNpc_Info = { [1] = {npcid = 43906, poslist = {[1] = {xpos=147, zpos=35}, [2] = {xpos=230, zpos=39}, [3] = {xpos=342, zpos=150}, [4] = {xpos=349, zpos=224}, [5] = {xpos=232, zpos=346}, [6] = {xpos=162, zpos=349}, [7] = {xpos=33, zpos=232}, [8] = {xpos=30, zpos=160}}, baseai=3, aiScript=-1, scriptid=891451, dir=0}, [2] = {npcid = 43907, poslist = {[1] = {xpos=159, zpos=66}, [2] = {xpos=231, zpos=60}, [3] = {xpos=314, zpos=150}, [4] = {xpos=311, zpos=232}, [5] = {xpos=222, zpos=312}, [6] = {xpos=150, zpos=317}, [7] = {xpos=63, zpos=230}, [8] = {xpos=62, zpos=149}}, baseai=3, aiScript=-1, scriptid=891452, dir=0}, [3] = {npcid = 43908, poslist = {[1] = {xpos=312, zpos=62}, [2] = {xpos=315, zpos=311}, [3] = {xpos=67, zpos=314},[4] = {xpos=64, zpos=68}}, baseai=3, aiScript=-1, scriptid=891453, dir=0}, [4] = {npcid = 43909, poslist = {[1] = {xpos=82, zpos=134}, [2] = {xpos=84, zpos=246}, [3] = {xpos=143, zpos=298}, [4] = {xpos=241, zpos=305}, [5] = {xpos=287, zpos=244}, [6] = {xpos=297, zpos=133}, [7] = {xpos=240, zpos=78}, [8] = {xpos=133, zpos=82}}, baseai=3, aiScript=-1, scriptid=891454, dir=0}, } if nTargetIndex >= 1 and nTargetIndex <= 8 then TempData = ZhanBeiNpc_Info[1].poslist; PosX = TempData[nTargetIndex].xpos; PosZ = TempData[nTargetIndex].zpos; end str = ScriptGlobal_Format('#{MZZBQ_150811_337}', targetZMName,PosX,PosZ,toScene); elseif MissType == 3 then ZhanBeiNpc_Info = { [1] = {npcid = 43906, poslist = {[1] = {xpos=147, zpos=35}, [2] = {xpos=230, zpos=39}, [3] = {xpos=342, zpos=150}, [4] = {xpos=349, zpos=224}, [5] = {xpos=232, zpos=346}, [6] = {xpos=162, zpos=349}, [7] = {xpos=33, zpos=232}, [8] = {xpos=30, zpos=160}}, baseai=3, aiScript=-1, scriptid=891451, dir=0}, [2] = {npcid = 43907, poslist = {[1] = {xpos=159, zpos=66}, [2] = {xpos=231, zpos=60}, [3] = {xpos=314, zpos=150}, [4] = {xpos=311, zpos=232}, [5] = {xpos=222, zpos=312}, [6] = {xpos=150, zpos=317}, [7] = {xpos=63, zpos=230}, [8] = {xpos=62, zpos=149}}, baseai=3, aiScript=-1, scriptid=891452, dir=0}, [3] = {npcid = 43908, poslist = {[1] = {xpos=312, zpos=62}, [2] = {xpos=315, zpos=311}, [3] = {xpos=67, zpos=314},[4] = {xpos=64, zpos=68}}, baseai=3, aiScript=-1, scriptid=891453, dir=0}, [4] = {npcid = 43909, poslist = {[1] = {xpos=82, zpos=134}, [2] = {xpos=84, zpos=246}, [3] = {xpos=143, zpos=298}, [4] = {xpos=241, zpos=305}, [5] = {xpos=287, zpos=244}, [6] = {xpos=297, zpos=133}, [7] = {xpos=240, zpos=78}, [8] = {xpos=133, zpos=82}}, baseai=3, aiScript=-1, scriptid=891454, dir=0}, } if nTargetIndex >= 1 and nTargetIndex <= 4 then TempData = ZhanBeiNpc_Info[3].poslist; PosX = TempData[nTargetIndex].xpos; PosZ = TempData[nTargetIndex].zpos; end str = ScriptGlobal_Format('#{MZZBQ_150811_338}', targetZMName,PosX,PosZ,toScene); elseif MissType == 4 then ZhanBeiNpc_Info = { [1] = {npcid = 43906, poslist = {[1] = {xpos=147, zpos=35}, [2] = {xpos=230, zpos=39}, [3] = {xpos=342, zpos=150}, [4] = {xpos=349, zpos=224}, [5] = {xpos=232, zpos=346}, [6] = {xpos=162, zpos=349}, [7] = {xpos=33, zpos=232}, [8] = {xpos=30, zpos=160}}, baseai=3, aiScript=-1, scriptid=891451, dir=0}, [2] = {npcid = 43907, poslist = {[1] = {xpos=159, zpos=66}, [2] = {xpos=231, zpos=60}, [3] = {xpos=314, zpos=150}, [4] = {xpos=311, zpos=232}, [5] = {xpos=222, zpos=312}, [6] = {xpos=150, zpos=317}, [7] = {xpos=63, zpos=230}, [8] = {xpos=62, zpos=149}}, baseai=3, aiScript=-1, scriptid=891452, dir=0}, [3] = {npcid = 43908, poslist = {[1] = {xpos=312, zpos=62}, [2] = {xpos=315, zpos=311}, [3] = {xpos=67, zpos=314},[4] = {xpos=64, zpos=68}}, baseai=3, aiScript=-1, scriptid=891453, dir=0}, [4] = {npcid = 43909, poslist = {[1] = {xpos=82, zpos=134}, [2] = {xpos=84, zpos=246}, [3] = {xpos=143, zpos=298}, [4] = {xpos=241, zpos=305}, [5] = {xpos=287, zpos=244}, [6] = {xpos=297, zpos=133}, [7] = {xpos=240, zpos=78}, [8] = {xpos=133, zpos=82}}, baseai=3, aiScript=-1, scriptid=891454, dir=0}, } if nTargetIndex >= 1 and nTargetIndex <= 8 then TempData = ZhanBeiNpc_Info[2].poslist; PosX = TempData[nTargetIndex].xpos; PosZ = TempData[nTargetIndex].zpos; end str = ScriptGlobal_Format('#{MZZBQ_150811_339}', targetZMName,PosX,PosZ,toScene); elseif MissType == 5 then ZhanBeiNpc_Info = { [1] = {npcid = 43906, poslist = {[1] = {xpos=147, zpos=35}, [2] = {xpos=230, zpos=39}, [3] = {xpos=342, zpos=150}, [4] = {xpos=349, zpos=224}, [5] = {xpos=232, zpos=346}, [6] = {xpos=162, zpos=349}, [7] = {xpos=33, zpos=232}, [8] = {xpos=30, zpos=160}}, baseai=3, aiScript=-1, scriptid=891451, dir=0}, [2] = {npcid = 43907, poslist = {[1] = {xpos=159, zpos=66}, [2] = {xpos=231, zpos=60}, [3] = {xpos=314, zpos=150}, [4] = {xpos=311, zpos=232}, [5] = {xpos=222, zpos=312}, [6] = {xpos=150, zpos=317}, [7] = {xpos=63, zpos=230}, [8] = {xpos=62, zpos=149}}, baseai=3, aiScript=-1, scriptid=891452, dir=0}, [3] = {npcid = 43908, poslist = {[1] = {xpos=312, zpos=62}, [2] = {xpos=315, zpos=311}, [3] = {xpos=67, zpos=314},[4] = {xpos=64, zpos=68}}, baseai=3, aiScript=-1, scriptid=891453, dir=0}, [4] = {npcid = 43909, poslist = {[1] = {xpos=82, zpos=134}, [2] = {xpos=84, zpos=246}, [3] = {xpos=143, zpos=298}, [4] = {xpos=241, zpos=305}, [5] = {xpos=287, zpos=244}, [6] = {xpos=297, zpos=133}, [7] = {xpos=240, zpos=78}, [8] = {xpos=133, zpos=82}}, baseai=3, aiScript=-1, scriptid=891454, dir=0}, } if nTargetIndex >= 1 and nTargetIndex <= 8 then TempData = ZhanBeiNpc_Info[4].poslist; PosX = TempData[nTargetIndex].xpos; PosZ = TempData[nTargetIndex].zpos; end str = ScriptGlobal_Format('#{MZZBQ_150811_340}', targetZMName,PosX,PosZ,toScene); elseif MissType == 6 then TargetPosInfo = { [1] = {xpos = 78, zpos = 162}, [2] = {xpos = 80, zpos = 241}, [3] = {xpos = 157, zpos = 299}, [4] = {xpos = 223, zpos = 297}, [5] = {xpos = 300, zpos = 220}, [6] = {xpos = 299, zpos = 157}, [7] = {xpos = 230, zpos = 80}, [8] = {xpos = 146, zpos = 83}, } if nTargetIndex >= 1 and nTargetIndex <= 8 then PosX = TargetPosInfo[nTargetIndex].xpos; PosZ = TargetPosInfo[nTargetIndex].zpos; end str = ScriptGlobal_Format('#{MZZBQ_150811_405}', targetZMName,PosX,PosZ,toScene); end return PosX .. ',' .. PosZ .. ',' .. toScene;");
                            //}
                            DoStringEx("nSelIndex = -1; cnt = 0; while true do name, content = DataPool:GetPlayerMission_Memo(cnt); if string.find(name, '" + task.Name + "') then if DataPool:GetPlayerMission_Variable(cnt, 0) == 1 then return 'Xong'; else nSelIndex = cnt; break; end end cnt = cnt + 1; if cnt == 20 then return 'Chua'; end end if nSelIndex == -1 then return '-1'; end PosX = 0; PosZ = 0; toScene = 0; str = ''; MissType = DataPool:GetPlayerMission_Variable( nSelIndex, 6 ); nXZZMId = DataPool:GetPlayerMission_Variable( nSelIndex, 5 ); nTargetIndex = DataPool:GetPlayerMission_Variable( nSelIndex, 2 ); nSelfZMId_temp = DataPool:GetPlayerMission_Variable( nSelIndex, 4 ); nSelfZMId = math.floor(nSelfZMId_temp/10000); nAcceptNpcIndex = math.mod(nSelfZMId_temp, 10000); AllianceNpcNameList = { [0] = { [1] = {xpos=368, zpos=10, name='Tiền Phong Doanh Địa Chiến Bị Quan-Đông Bắc'}, [2] = {xpos=373, zpos=370, name='Tiền Phong Doanh Địa Chiến Bị Quan-Đông Nam'}, [3] = {xpos=15, zpos=371, name='Tiền Phong Doanh Địa Chiến Bị Quan-Tây Nam'}, [4] = {xpos=9, zpos=11, name='Tiền Phong Doanh Địa Chiến Bị Quan-Tây Bắc'}, }, [1] = { [1] = {xpos=368, zpos=10, name='Tiền Phong Doanh Địa Chiến Bị Quan-Đông Bắc'}, [2] = {xpos=373, zpos=370, name='Tiền Phong Doanh Địa Chiến Bị Quan-Đông Nam'}, [3] = {xpos=15, zpos=371, name='Tiền Phong Doanh Địa Chiến Bị Quan-Tây Nam'}, [4] = {xpos=9, zpos=11, name='Tiền Phong Doanh Địa Chiến Bị Quan-Tây Bắc'}, }, [2] = { [1] = {xpos=368, zpos=10, name='Tiền Phong Doanh Địa Chiến Bị Quan-Đông Bắc'}, [2] = {xpos=373, zpos=370, name='Tiền Phong Doanh Địa Chiến Bị Quan-Đông Nam'}, [3] = {xpos=15, zpos=371, name='Tiền Phong Doanh Địa Chiến Bị Quan-Tây Nam'}, [4] = {xpos=9, zpos=11, name='Tiền Phong Doanh Địa Chiến Bị Quan-Tây Bắc'}, }, [3] = { [1] = {xpos=368, zpos=10, name='Tiền Phong Doanh Địa Chiến Bị Quan-Đông Bắc'}, [2] = {xpos=373, zpos=370, name='Tiền Phong Doanh Địa Chiến Bị Quan-Đông Nam'}, [3] = {xpos=15, zpos=371, name='Tiền Phong Doanh Địa Chiến Bị Quan-Tây Nam'}, [4] = {xpos=9, zpos=11, name='Tiền Phong Doanh Địa Chiến Bị Quan-Tây Bắc'}, }, [4] = { [1] = {xpos=368, zpos=10, name='Tiền Phong Doanh Địa Chiến Bị Quan-Đông Bắc'}, [2] = {xpos=373, zpos=370, name='Tiền Phong Doanh Địa Chiến Bị Quan-Đông Nam'}, [3] = {xpos=15, zpos=371, name='Tiền Phong Doanh Địa Chiến Bị Quan-Tây Nam'}, [4] = {xpos=9, zpos=11, name='Tiền Phong Doanh Địa Chiến Bị Quan-Tây Bắc'}, }, } AllianceSceneList = { [ 0 ] = 663, [ 1 ] = 664, [ 2 ] = 665, [ 3 ] = 666, [ 4 ] = 667, } AllianceName_list = { [0] = 'Thiên Thu Điện', [1] = 'Bất Quy Lâm', [2] = 'Vô Nhai Hải', [3] = 'Viêm La Thiên', [4] = 'Ngọc Hoàng Sơn', } targetZMName = ''; toScene = 0; selfScene = 0; missNpcName = ''; acceptxPos = 0; acceptzPos = 0; if nXZZMId >= 0 and nXZZMId <= 4 then targetZMName = AllianceName_list[nXZZMId]; toScene = AllianceSceneList[nXZZMId]; selfScene = AllianceSceneList[nXZZMId]; end if nSelfZMId >= 0 and nSelfZMId <= 4 and nAcceptNpcIndex > 0 and nAcceptNpcIndex <= 4 then AcceptMisInfo = AllianceNpcNameList[nSelfZMId]; missNpcName = AcceptMisInfo[nAcceptNpcIndex].name; acceptxPos = AcceptMisInfo[nAcceptNpcIndex].xpos; acceptzPos = AcceptMisInfo[nAcceptNpcIndex].zpos; end if MissType == 1 then DestPosList = { [1] = {xlefttoppos = 129, zlefttoppos = 80, xrightbottompos = 150, zrightbottompos = 91, tipposx = 139, tipposz = 85}, [2] = {xlefttoppos = 222, zlefttoppos = 83, xrightbottompos = 236, zrightbottompos = 92, tipposx = 229, tipposz = 87}, [3] = {xlefttoppos = 286, zlefttoppos = 124, xrightbottompos = 296, zrightbottompos = 144, tipposx = 291, tipposz = 134}, [4] = {xlefttoppos = 284, zlefttoppos = 223, xrightbottompos = 294, zrightbottompos = 243, tipposx = 289, tipposz = 233}, [5] = {xlefttoppos = 220, zlefttoppos = 285, xrightbottompos = 240, zrightbottompos = 295, tipposx = 230, tipposz = 290}, [6] = {xlefttoppos = 144, zlefttoppos = 286, xrightbottompos = 164, zrightbottompos = 296, tipposx = 154, tipposz = 291}, [7] = {xlefttoppos = 84, zlefttoppos = 217, xrightbottompos = 94, zrightbottompos = 237, tipposx = 89, tipposz = 227}, [8] = {xlefttoppos = 84, zlefttoppos = 151, xrightbottompos = 94, zrightbottompos = 171, tipposx = 89, tipposz = 161}, } if nTargetIndex >= 1 and nTargetIndex <= 8 then PosX = DestPosList[nTargetIndex].tipposx; PosZ = DestPosList[nTargetIndex].tipposz; end end if MissType == 2 then ZhanBeiNpc_Info = { [1] = {npcid = 43906, poslist = {[1] = {xpos=147, zpos=35}, [2] = {xpos=230, zpos=39}, [3] = {xpos=342, zpos=150}, [4] = {xpos=349, zpos=224}, [5] = {xpos=232, zpos=346}, [6] = {xpos=162, zpos=349}, [7] = {xpos=33, zpos=232}, [8] = {xpos=30, zpos=160}}, baseai=3, aiScript=-1, scriptid=891451, dir=0}, [2] = {npcid = 43907, poslist = {[1] = {xpos=159, zpos=66}, [2] = {xpos=231, zpos=60}, [3] = {xpos=314, zpos=150}, [4] = {xpos=311, zpos=232}, [5] = {xpos=222, zpos=312}, [6] = {xpos=150, zpos=317}, [7] = {xpos=63, zpos=230}, [8] = {xpos=62, zpos=149}}, baseai=3, aiScript=-1, scriptid=891452, dir=0}, [3] = {npcid = 43908, poslist = {[1] = {xpos=312, zpos=62}, [2] = {xpos=315, zpos=311}, [3] = {xpos=67, zpos=314},[4] = {xpos=64, zpos=68}}, baseai=3, aiScript=-1, scriptid=891453, dir=0}, [4] = {npcid = 43909, poslist = {[1] = {xpos=82, zpos=134}, [2] = {xpos=84, zpos=246}, [3] = {xpos=143, zpos=298}, [4] = {xpos=241, zpos=305}, [5] = {xpos=287, zpos=244}, [6] = {xpos=297, zpos=133}, [7] = {xpos=240, zpos=78}, [8] = {xpos=133, zpos=82}}, baseai=3, aiScript=-1, scriptid=891454, dir=0}, } if nTargetIndex >= 1 and nTargetIndex <= 8 then TempData = ZhanBeiNpc_Info[1].poslist; PosX = TempData[nTargetIndex].xpos; PosZ = TempData[nTargetIndex].zpos; end end if MissType == 3 then ZhanBeiNpc_Info = { [1] = {npcid = 43906, poslist = {[1] = {xpos=147, zpos=35}, [2] = {xpos=230, zpos=39}, [3] = {xpos=342, zpos=150}, [4] = {xpos=349, zpos=224}, [5] = {xpos=232, zpos=346}, [6] = {xpos=162, zpos=349}, [7] = {xpos=33, zpos=232}, [8] = {xpos=30, zpos=160}}, baseai=3, aiScript=-1, scriptid=891451, dir=0}, [2] = {npcid = 43907, poslist = {[1] = {xpos=159, zpos=66}, [2] = {xpos=231, zpos=60}, [3] = {xpos=314, zpos=150}, [4] = {xpos=311, zpos=232}, [5] = {xpos=222, zpos=312}, [6] = {xpos=150, zpos=317}, [7] = {xpos=63, zpos=230}, [8] = {xpos=62, zpos=149}}, baseai=3, aiScript=-1, scriptid=891452, dir=0}, [3] = {npcid = 43908, poslist = {[1] = {xpos=312, zpos=62}, [2] = {xpos=315, zpos=311}, [3] = {xpos=67, zpos=314},[4] = {xpos=64, zpos=68}}, baseai=3, aiScript=-1, scriptid=891453, dir=0}, [4] = {npcid = 43909, poslist = {[1] = {xpos=82, zpos=134}, [2] = {xpos=84, zpos=246}, [3] = {xpos=143, zpos=298}, [4] = {xpos=241, zpos=305}, [5] = {xpos=287, zpos=244}, [6] = {xpos=297, zpos=133}, [7] = {xpos=240, zpos=78}, [8] = {xpos=133, zpos=82}}, baseai=3, aiScript=-1, scriptid=891454, dir=0}, } if nTargetIndex >= 1 and nTargetIndex <= 4 then TempData = ZhanBeiNpc_Info[3].poslist; PosX = TempData[nTargetIndex].xpos; PosZ = TempData[nTargetIndex].zpos; end end if MissType == 4 then ZhanBeiNpc_Info = { [1] = {npcid = 43906, poslist = {[1] = {xpos=147, zpos=35}, [2] = {xpos=230, zpos=39}, [3] = {xpos=342, zpos=150}, [4] = {xpos=349, zpos=224}, [5] = {xpos=232, zpos=346}, [6] = {xpos=162, zpos=349}, [7] = {xpos=33, zpos=232}, [8] = {xpos=30, zpos=160}}, baseai=3, aiScript=-1, scriptid=891451, dir=0}, [2] = {npcid = 43907, poslist = {[1] = {xpos=159, zpos=66}, [2] = {xpos=231, zpos=60}, [3] = {xpos=314, zpos=150}, [4] = {xpos=311, zpos=232}, [5] = {xpos=222, zpos=312}, [6] = {xpos=150, zpos=317}, [7] = {xpos=63, zpos=230}, [8] = {xpos=62, zpos=149}}, baseai=3, aiScript=-1, scriptid=891452, dir=0}, [3] = {npcid = 43908, poslist = {[1] = {xpos=312, zpos=62}, [2] = {xpos=315, zpos=311}, [3] = {xpos=67, zpos=314},[4] = {xpos=64, zpos=68}}, baseai=3, aiScript=-1, scriptid=891453, dir=0}, [4] = {npcid = 43909, poslist = {[1] = {xpos=82, zpos=134}, [2] = {xpos=84, zpos=246}, [3] = {xpos=143, zpos=298}, [4] = {xpos=241, zpos=305}, [5] = {xpos=287, zpos=244}, [6] = {xpos=297, zpos=133}, [7] = {xpos=240, zpos=78}, [8] = {xpos=133, zpos=82}}, baseai=3, aiScript=-1, scriptid=891454, dir=0}, } if nTargetIndex >= 1 and nTargetIndex <= 8 then TempData = ZhanBeiNpc_Info[2].poslist; PosX = TempData[nTargetIndex].xpos; PosZ = TempData[nTargetIndex].zpos; end end if MissType == 5 then ZhanBeiNpc_Info = { [1] = {npcid = 43906, poslist = {[1] = {xpos=147, zpos=35}, [2] = {xpos=230, zpos=39}, [3] = {xpos=342, zpos=150}, [4] = {xpos=349, zpos=224}, [5] = {xpos=232, zpos=346}, [6] = {xpos=162, zpos=349}, [7] = {xpos=33, zpos=232}, [8] = {xpos=30, zpos=160}}, baseai=3, aiScript=-1, scriptid=891451, dir=0}, [2] = {npcid = 43907, poslist = {[1] = {xpos=159, zpos=66}, [2] = {xpos=231, zpos=60}, [3] = {xpos=314, zpos=150}, [4] = {xpos=311, zpos=232}, [5] = {xpos=222, zpos=312}, [6] = {xpos=150, zpos=317}, [7] = {xpos=63, zpos=230}, [8] = {xpos=62, zpos=149}}, baseai=3, aiScript=-1, scriptid=891452, dir=0}, [3] = {npcid = 43908, poslist = {[1] = {xpos=312, zpos=62}, [2] = {xpos=315, zpos=311}, [3] = {xpos=67, zpos=314},[4] = {xpos=64, zpos=68}}, baseai=3, aiScript=-1, scriptid=891453, dir=0}, [4] = {npcid = 43909, poslist = {[1] = {xpos=82, zpos=134}, [2] = {xpos=84, zpos=246}, [3] = {xpos=143, zpos=298}, [4] = {xpos=241, zpos=305}, [5] = {xpos=287, zpos=244}, [6] = {xpos=297, zpos=133}, [7] = {xpos=240, zpos=78}, [8] = {xpos=133, zpos=82}}, baseai=3, aiScript=-1, scriptid=891454, dir=0}, } if nTargetIndex >= 1 and nTargetIndex <= 8 then TempData = ZhanBeiNpc_Info[4].poslist; PosX = TempData[nTargetIndex].xpos; PosZ = TempData[nTargetIndex].zpos; end end if MissType == 6 then TargetPosInfo = { [1] = {xpos = 78, zpos = 162}, [2] = {xpos = 80, zpos = 241}, [3] = {xpos = 157, zpos = 299}, [4] = {xpos = 223, zpos = 297}, [5] = {xpos = 300, zpos = 220}, [6] = {xpos = 299, zpos = 157}, [7] = {xpos = 230, zpos = 80}, [8] = {xpos = 146, zpos = 83}, } if nTargetIndex >= 1 and nTargetIndex <= 8 then PosX = TargetPosInfo[nTargetIndex].xpos; PosZ = TargetPosInfo[nTargetIndex].zpos; end end if MissType == 7 then aryCaptains = { [ 0 ] = { { NpcId = 44603, Pos = { 282, 189 }, MissionPos = { 299, 189 } }, { NpcId = 44604, Pos = { 99, 189 }, MissionPos = { 77, 180 } }, { NpcId = 44605, Pos = { 189, 282 }, MissionPos = { 190, 300 } }, { NpcId = 44606, Pos = { 189, 97 }, MissionPos = { 189, 78 } }, }, [ 1 ] = { { NpcId = 44613, Pos = { 282, 189 }, MissionPos = { 299, 189 } }, { NpcId = 44614, Pos = { 99, 189 }, MissionPos = { 77, 180 } }, { NpcId = 44615, Pos = { 189, 282 }, MissionPos = { 190, 300 } }, { NpcId = 44616, Pos = { 189, 97 }, MissionPos = { 189, 78 } }, }, [ 2 ] = { { NpcId = 44623, Pos = { 282, 189 }, MissionPos = { 299, 189 } }, { NpcId = 44624, Pos = { 99, 189 }, MissionPos = { 77, 180 } }, { NpcId = 44625, Pos = { 189, 282 }, MissionPos = { 190, 300 } }, { NpcId = 44626, Pos = { 189, 97 }, MissionPos = { 189, 78 } }, }, [ 3 ] = { { NpcId = 44633, Pos = { 282, 189 }, MissionPos = { 299, 189 } }, { NpcId = 44634, Pos = { 99, 189 }, MissionPos = { 77, 180 } }, { NpcId = 44635, Pos = { 189, 282 }, MissionPos = { 190, 300 } }, { NpcId = 44636, Pos = { 189, 97 }, MissionPos = { 189, 78 } }, }, [ 4 ] = { { NpcId = 44643, Pos = { 282, 189 }, MissionPos = { 299, 189 } }, { NpcId = 44644, Pos = { 99, 189 }, MissionPos = { 77, 180 } }, { NpcId = 44645, Pos = { 189, 282 }, MissionPos = { 190, 300 } }, { NpcId = 44646, Pos = { 189, 97 }, MissionPos = { 189, 78 } }, }, }; nDirection = DataPool:GetPlayerMission_Variable(nSelIndex, 2); aryPos = aryCaptains[ nXZZMId ][ nDirection ].MissionPos; PosX = aryPos[1]; PosZ = aryPos[2]; end return '" + TDT.RandomEmptyString() + "' .. PosX .. ',' .. PosZ .. ',' .. toScene; ");
                            Thread.Sleep(1000);
                            LuaToString();
                            Thread.Sleep(1000);
                            ChienBiPoint = LuaToString().Trim();
                            //Main.TDTLog(ChienBiPoint);
                            return;
                        }
                    }
                }
                // Tìm Mật Thám Viêm La Kỳ Công
                MissionState = "None";
                return;
            }
            if (MissionState == "Done" || MissionState == "None")
            {
                if (GoToEx(PointNPCChienBi()))
                {
                    Talk("tienphongdoanhdiachienbi");
                    Thread.Sleep(1000);
                    QuestFrame.Click("#{MZZBQ_150811_13}");
                    Thread.Sleep(1000);
                    if (MissionState == "Done")
                    {
                        LUA.QuestFrameMissionComplete();
                        MissionState = "";
                        Thread.Sleep(1000);
                    }
                    if (MissionState == "None")
                    {
                        QuestFrameAccept();
                        MissionState = "";
                        Thread.Sleep(1000);
                    }
                }
                return;
            }

            // npc nhan nv Tiền Phong Doanh Địa Chiến Bị 373 370
            //373,370 => dongnam 15,371 => taynam 368,310 => dongbac 9,11 =>taybac
            // Diệt Thủ Vệ => 190,300
            // #{MZZBQ_150811_334} Đốt Kho Thóc CircularTaskTool70_16 => Kho Thóc 241,305 menpai=45
            // #{MZZBQ_150811_332} Xây Vương Kỳ Merchandise1_13 => Vương Kỳ 312,62 menpai=44
            // #{MZZBQ_150811_335} Gửi Thư Khuyên Hàng CircularTaskTool70_15 => 299,157
            // #{MZZBQ_150811_330} Cắm Cờ Tiền Tuyến 154,291 CircularTaskTool70_12
            // playerstate = 5
            //Tiền Phong Lệnh Tiễn-Tây Nam
            // #{MZZBQ_150811_13} nhiemvuchienbi
            // #{MZZBQ_150811_467} vanchuyenden
            // #{MZZBQ_150811_466} nhanlenhtien
        }

        public string PointNPCChienBi()
        {
            foreach (var item in PacketItems.All)
            {
                if (item.ClearName.StartsWith("tienphonglenhtien"))
                {
                    if (item.ClearName.Contains("dongbac"))
                        return "368,10";
                    if (item.ClearName.Contains("dongnam"))
                        return "373,370";
                    if (item.ClearName.Contains("taynam"))
                        return "15,371";
                    if (item.ClearName.Contains("taybac"))
                        return "9,11";
                }
            }
            return "";
            // 96 96 276 276
        }

        public void ThuBi()
        {
            if (!IsOneSec)
                return;
            if (ForcePickItem())
                return;
            StopFollow();
            if (TLBB.IsQuestOpen)
            {
                string text = QuestFrame.All(this);
                if (text.Contains("#{MZPVE_150812_42}") || text.Contains("#{MZPVE_150812_812}") || text.Contains("#{MZPVE_150812_1558}") || text.Contains("#{MZPVE_150812_1184}") || text.Contains("#{MZPVE_150812_1931}"))
                {
                    //IsThuBi = false;
                    return;
                }
            }
            if (TLBB.PlayerState != 0)
                return;
            if (SecCount % 20 == 0 && IdleTime > 10)
            {
                MissionState = "";
            }
            if (TLBB.MapId < 660)
            {
                GoToEx(VIEMLATHIEN.ThongBaoHauCan);
                return;
            }
            // Höa di®m chi ð¸a t¥m kÏ hoa
            // #{MZPVE_150812_1757} // thiên hoang dược phổ, hỏa diệm hoa

            // #{MZPVE_150812_360} -- Vô sở bất tri thám kỳ văn
            // #{MZPVE_150812_971} -- Chúc tửu cộng tế kiến mộc đài
            // #{MZPVE_150812_1346} -- Phá Băng Ngư Đường Tập Ngư Thuật
            // #{MZPVE_150812_1720} -- Dung Thiết Luyện Lô Tu Chú Tạo
            // #{MZPVE_150812_2094} -- Bách Luyện Đoàn Đài Chú Kiếm Thành

            if (MissionState != "Done")
            {
                foreach (GameTask task in GameTask.Enum(this))
                {
                    if (GAMEDIC.ChienMinhQuestUseItem.ContainsKey(task.Name))
                    {
                        if (task.Completed)
                        {
                            MissionState = "Done";
                            return;
                        }
                    }
                    if (GAMEDIC.ChienMinhQuestThuThap.ContainsKey(task.Name))
                    {
                        if (task.Completed)
                        {
                            MissionState = "Done";
                            return;
                        }
                    }
                    if (GAMEDIC.ChienMinhQuestDuocPho.ContainsKey(task.Name))
                    {
                        if (task.Completed)
                        {
                            MissionState = "Done";
                            return;
                        }
                    }
                    if (GAMEDIC.ChienMinhQuestHoTong.ContainsKey(task.Name))
                    {
                        if (task.Completed)
                        {
                            MissionState = "Done";
                            return;
                        }
                    }
                    if (task.ClearName == "hoadiemchidiatamkyhoa" || task.ClearName.Contains("timmattham"))
                    {
                        if (task.Completed)
                        {
                            MissionState = "Done";
                            return;
                        }
                    }
                    if (task.ClearName.Contains("phutangxan") || task.ClearName.Contains("dieubienchangia"))
                    {
                        if (task.Completed)
                        {
                            MissionState = "Done";
                            return;
                        }
                    }
                    if (task.ClearName.Contains("vokhomanhoa") || task.ClearName.Contains("tayluongkhoangxa"))
                    {
                        if (task.Completed)
                        {
                            MissionState = "Done";
                            return;
                        }
                    }
                }
            }
            if (MissionState == "")
            {
                if (!TLBB.IsTogleMission)
                {
                    PostMessage(18, 105);
                    Thread.Sleep(1000);
                }
                LUA.CLOSE_MISSION();
                MissionState = "CloseMission";

                return;
            }
            if (MissionState == "None")
            {
                if (GoToEx(157, 201))
                {
                    if (!Talked)
                    {
                        Talk("thongbaohaucan");
                        Talked = true;
                        return;
                    }
                    else
                    {
                        LUA.DoString("setmetatable(_G, {__index = CityWarDailyQuest_Env}); CityWarDailyQuest_Client1_Enter_Clicked();");
                        Talked = false;
                    }
                }
            }
            if (MissionState == "Done")
            {
                if (GoToEx(VIEMLATHIEN.ThongBaoHauCan.X, VIEMLATHIEN.ThongBaoHauCan.Y))
                {
                    if (!Talked)
                    {
                        Talk(VIEMLATHIEN.ThongBaoHauCan);
                        Talked = true;
                        return;
                    }
                    else
                    {
                        LUA.DoString("setmetatable(_G, {__index = CityWarDailyQuest_Env}); CityWarDailyQuest_Client1_Finish_Clicked();");
                        Talked = false;
                    }
                }
            }

            if (MissionState == "CloseMission")
            {
                foreach (GameTask task in GameTask.Enum(this))
                {
                    if (GAMEDIC.ChienMinhQuestUseItem.ContainsKey(task.Name))
                    {
                        if (task.Completed)
                        {
                            MissionState = "Done";
                            return;
                        }
                        else
                        {
                            if (task.Count1 == 0)
                                MissionState = "UseItem";
                            else
                                MissionState = "UseItemGo";
                            return;
                        }
                    }
                    if (GAMEDIC.ChienMinhQuestHoTong.ContainsKey(task.Name))
                    {
                        if (task.Completed)
                        {
                            MissionState = "Done";
                            return;
                        }
                        else
                        {
                            if (task.Count1 == 0)
                                MissionState = "VoKhoTalk";
                            else
                                MissionState = "VoKhoGo";
                            return;
                        }
                    }
                    if (GAMEDIC.ChienMinhQuestDuocPho.ContainsKey(task.Name))
                    {
                        if (task.Completed)
                        {
                            MissionState = "Done";
                            return;
                        }
                        else
                        {
                            MissionState = "DoHoaDiem";
                            return;
                        }
                    }
                    if (task.ClearName.Contains("timmattham"))
                    {
                        if (task.Completed)
                        {
                            MissionState = "Done";
                            return;
                        }
                        else
                        {
                            MissionState = "KyCong";
                            MissionX = 128;
                            MissionY = 258;
                            return;
                        }
                    }
                    if (GAMEDIC.ChienMinhQuestThuThap.ContainsKey(task.Name))
                    {
                        if (task.Completed)
                        {
                            MissionState = "Done";
                            return;
                        }
                        else
                        {
                            MissionState = "DoThuThap";
                            return;
                        }
                    }
                }
                // Tìm Mật Thám Viêm La Kỳ Công
                MissionState = "None";
                return;
            }
            if (MissionState == "UseItem")
            {
                if (GoToEx(259, 254))
                {
                    foreach (GameTask task in GameTask.Enum(this))
                    {
                        if (GAMEDIC.ChienMinhQuestUseItem.ContainsKey(task.Name))
                        {
                            if (task.Count1 == 1)
                            {
                                MissionState = "UseItemGo";
                                return;
                            }
                        }
                    }
                    if (TLBB.IsRide)
                    {
                        DownRide();
                        return;
                    }
                    foreach (GameObject _object in Objects.All)
                    {
                        if (_object.GetDistance(262, 255) < 3 && _object.Menpai > 40 && _object.Menpai != MENPAI.QuyCoc && _object.Menpai != MENPAI.DaoHoa && _object.Menpai != 0xFFFFFFFF)
                        {
                            SelectTarget((int)_object.Id);
                            break;
                        }
                    }
                    foreach (var item in PacketItems.All)
                    {
                        if (GAMEDIC.ChienMinhItemUse.ContainsKey(item.Name))
                        {
                            item.DoSubAction();
                            Thread.Sleep(100);
                        }
                    }
                    return;
                }
                else
                {
                    return;
                }
            }
            if (MissionState == "UseItemGo")
            {
                if (GoToEx(243, 254))
                {
                    if (TLBB.IsQuestOpen)
                    {
                        if (QuestFrame.Click(1))
                        {
                            MissionState = "";
                        }
                        QuestFrame.Close();
                    }
                    foreach (GameObject _object in Objects.All)
                    {
                        if (_object.GetDistance(243, 254) < 3 && (_object.Menpai == 48 || _object.Menpai == 47 || _object.Menpai == 49 || _object.Menpai == 50 || _object.Menpai == 51))
                        {
                            Talk(_object.Id);
                            break;
                        }
                    }
                    return;
                }
                else
                {
                    return;
                }
            }
            if (MissionState == "VoKhoTalk" || MissionState == "VoKhoTalkForce")
            {
                if (GoToEx(195, 218))
                {
                    if (TLBB.IsRide)
                    {
                        DownRide();
                        return;
                    }
                    if (TLBB.IsQuestOpen)
                    {
                        QuestFrame.Click(1);
                        MissionState = "";
                        //if(QuestFrame.Click("#{MZPVE_150812_2539}") || QuestFrame.Click("#{MZPVE_150812_1571}"))
                        //{
                        //    MissionState = "";
                        //}
                        //if (QuestFrame.Click("#{MZPVE_150812_2539}") || QuestFrame.Click("#{MZPVE_150812_72}"))
                        //{
                        //    MissionState = "";
                        //}
                        QuestFrame.Close();
                    }
                    foreach (GameObject _object in Objects.All)
                    {
                        if (_object.RoundX == 195 && _object.RoundY == 218 && _object.Menpai > 40 && _object.Menpai != MENPAI.DaoHoa && _object.Menpai != MENPAI.QuyCoc && _object.Menpai != 0xFFFFFFFF)
                        {
                            Talk(_object.Id);
                            break;
                        }
                    }
                    return;
                }
                else
                {
                    return;
                }
            }
            // ỶC^#{MZPVE_150812_1577}#{MZPVE_150812_1584**Khương Cảnh*189*215}#{MZPVE_150812_1585*Ẫ*0}ỹỌ
            if (MissionState == "VoKhoGo")
            {
                if (TLBB.IsQuestOpen)
                {
                    QuestFrame.Click(1);
                    QuestFrame.Close();
                    return;
                }
                string point = VuKhoString;
                if (point.Contains("Khương"))
                    point = point.Substring(point.IndexOf("Khương") + 10);
                else if (point.Contains("Người"))
                    point = point.Substring(point.IndexOf("Người") + 2);
                else if (point.Contains("Thu"))
                    point = point.Substring(point.IndexOf("Thu") + 2);
                else if (point.Contains("Tây"))
                    point = point.Substring(point.IndexOf("Tây") + 2);
                else if (point.Contains("Lam"))
                    point = point.Substring(point.IndexOf("Lam") + 2);
                else if (point.Contains("Dung"))
                    point = point.Substring(point.IndexOf("Dung") + 2);
                else if (point.Contains("Nhu"))
                    point = point.Substring(point.IndexOf("Nhu") + 2);
                MissionX = TDT.ParseInt(point);
                MissionY = TDT.ParseInt(ConverterEx.ReplaceFirst(point, MissionX.ToString(), ""));
                if (MissionX == 0 && MissionY == 0)
                {
                    string miss = Memory.ReadString(AddressGameExe + 0xAA5EB8, 0x560);
                    MissionX = TDT.ParseInt(miss);
                    MissionY = TDT.ParseInt(ConverterEx.ReplaceFirst(miss, MissionX.ToString(), ""));
                    PushDebugMessage(MissionX + "," + MissionY);
                }
                if (MissionX == 0 && MissionY == 0)
                {
                    string miss = Memory.ReadString(AddressGameExe + 0xAA5EB8, 0x560);
                    MissionState = "VoKhoTalkForce";
                    return;
                }
                if (GoToEx(MissionX, MissionY))
                {
                    foreach (GameObject _object in Objects.All)
                    {
                        if (TDT.GetDistance(RoundX, RoundY, _object.RoundX, _object.RoundY) < 9 && _object.Title.Contains("Chặn Đường"))
                        {
                            if (TLBB.IsRide)
                                DownRide();
                            if (_object.Menpai > 40)
                                Talk(_object.Id);
                            else
                            {
                                SelectTarget((int)_object.Id);
                                SendKey(Global.BaseSkill);
                            }
                            return;
                        }
                    }
                    //if (TLBB.IsRide)
                    //{
                    //    DownRide();
                    //    return;
                    //}
                    //if (TLBB.IsQuestOpen)
                    //{
                    //    if (QuestFrame.Click("#{MZPVE_150812_2539}"))
                    //    {
                    //        MissionState = "";
                    //    }
                    //    QuestFrame.Close();
                    //}
                    return;
                }
                else
                {
                    return;
                }
            }
            // #{MZPVE_150812_2422}
            if (MissionState == "KyCong")
            {
                if (GoToEx(MissionX, MissionY))
                {
                    if (TLBB.IsQuestOpen)
                    {
                        // Uống Mê Hồn Dược
                        //if (QuestFrame.Click("#{MZPVE_150812_2427}"))
                        //    MissionState = "";
                        //if(QuestFrame.Click("#{MZPVE_150812_2483}"))
                        //    MissionState = "";
                        //if (QuestFrame.Click("#{MZPVE_150812_2402}"))
                        //    MissionState = "";
                        if (QuestFrame.Click(1))
                            MissionState = "";
                        QuestFrame.Close();
                        return;
                    }
                    foreach (GameObject _object in Objects.All)
                    {
                        if (_object.IsMonter && _object.CleanName.Contains("mattham"))
                        {
                            if (TLBB.IsRide)
                                DownRide();
                            ForceAttack();
                            return;
                        }
                    }
                    foreach (GameObject _object in Objects.All)
                    {
                        if (_object.RoundX == MissionX && _object.RoundY == MissionY && _object.Menpai > 40 && _object.Menpai != MENPAI.QuyCoc && _object.Menpai != MENPAI.DaoHoa && _object.Menpai != 0xFFFFFFFF)
                        {
                            Talk(_object.Id);
                            return;
                        }
                    }
                }
                return;
            }
            // #{MZPVE_150812_1664*Ẳ*132 *220}
            if (MissionState == "DoThuThap")
            {
                if (TLBB.IsRide)
                {
                    DownRide();
                    return;
                }
                foreach (var item in PacketItems.All)
                {
                    if (item.ClearName == "nuocduongngon" || GAMEDIC.ChienMinhItemThuThap.ContainsKey(item.Name))
                    {
                        item.Use();
                        MissionState = "UseThuThap";
                        return;
                    }
                }
            }
            if (MissionState == "DoHoaDiem")
            {
                if (TLBB.IsRide)
                {
                    DownRide();
                    return;
                }
                foreach (var item in PacketItems.All)
                {
                    if (item.ClearName == "thienhoangduocpho")
                    {
                        item.Use();
                        MissionState = "UseThienHoaDuocPho";
                        return;
                    }
                }
            }
            if (MissionState == "UseThienHoaDuocPho" || MissionState == "UseThuThap")
            {
                if (MissionState == "UseThuThap")
                {
                    DoStringEx("return DEBUG");
                    LuaToString();
                    MissionState = "GetPointPhuTang";
                }
                else
                {
                    DoStringEx("return DEBUG");
                    LuaToString();
                    MissionState = "GetPointHoa";
                }
                return;
            }
            if (MissionState == "GetPointHoa" || MissionState == "GetPointPhuTang")
            {
                //#{MZPVE_150812_2271*Ẳ*251 *218}
                // #{MZPVE_150812_1787*Ẳ*202 *113}
                //#{MZPVE_150812_1664*Ẳ*132 *220}
                //#{MZPVE_150812_1785* *Hỏa Diệm Hoa Tiên*250*218}
                //#{MZPVE_150812_1785**Hỏa Diệm Hoa Yêu*191*125}
                //#{MZPVE_150812_2271*Ẳ*257 *218}
                //#{MZPVE_150812_1785**Hỏa Diệm Hoa*117*251}
                //#{MZPVE_150812_1793* *Hỏa Diệm Hoa Tiên*256*217}
                //#{MZPVE_150812_1785* *Hỏa Diệm Hoa Tiên*202*112}
                //#{MZPVE_150812_463*Ẳ                *205 *111}
                //#{MZPVE_150812_462*!*Tây Lương Hà Thủ Ô*206*110}
                //#{MZPVE_150812_467*Ẳ                *137 *188}
                //#{MZPVE_150812_462* *Thủ Sơn Linh Tiên*136*188}
                //#{MZPVE_150812_462**Thủ Sơn Yêu Tiên*132*222}
                //#{MZPVE_150812_258*Ẳ                *220 *116}
                string point = LuaString();
                if (MissionState == "GetPointHoa")
                {
                    string par = Regex.Replace(point, @".*_[\d]+", "");
                    MissionX = TDT.ParseInt(par);
                    if (MissionX > 0)
                    {
                        par = ConverterEx.ReplaceFirst(par, MissionX.ToString(), "");
                        MissionY = TDT.ParseInt(par);
                    }
                    else
                    {
                        MissionY = 0;
                    }
                    PushDebugMessage(MissionX + "," + MissionY);
                    MissionState = "GoHoa";
                    return;
                }
                else if (MissionState == "GetPointPhuTang")
                {
                    string par = Regex.Replace(point, @".*_[\d]+", "");
                    MissionX = TDT.ParseInt(par);
                    if (MissionX > 0)
                    {
                        par = ConverterEx.ReplaceFirst(par, MissionX.ToString(), "");
                        MissionY = TDT.ParseInt(par);
                    }
                    else
                    {
                        MissionY = 0;
                    }
                    MissionState = "GoPhuTang";
                    return;
                }
                if (point.Contains("{MZPVE_150812_1787") || point.Contains("{MZPVE_150812_2271") || point.Contains("Hỏa Diệm Hoa") || point.Contains("#{MZPVE_150812_463") || point.Contains("Tây Lương Hà Thủ Ô") || point.Contains("#{MZPVE_150812_467") || point.Contains("Thủ Sơn"))
                {
                    point = point.Replace("{MZPVE_150812_1787", "");
                    point = point.Replace("{MZPVE_150812_2271", "");
                    point = point.Replace("#{MZPVE_150812_463", "");
                    point = point.Replace("#{MZPVE_150812_467", "");
                    MissionX = TDT.ParseInt(point);
                    MissionY = TDT.ParseInt(point.Substring(6 + MissionX.ToString().Length));
                    if (point.Contains("Thủ Sơn Yêu Tiên"))
                    {
                        point = point.Substring(point.IndexOf("Thủ Sơn Yêu Tiên") + 17);
                        MissionX = TDT.ParseInt(point);
                        MissionY = TDT.ParseInt(point.Substring(2 + MissionX.ToString().Length));
                    }
                    else if (point.Contains("Thủ Sơn Linh Tiên"))
                    {
                        point = point.Substring(point.IndexOf("Thủ Sơn Linh Tiên") + 18);
                        MissionX = TDT.ParseInt(point);
                        MissionY = TDT.ParseInt(point.Substring(2 + MissionX.ToString().Length));
                    }
                    else if (point.Contains("Tây Lương Hà Thủ Ô"))
                    {
                        point = point.Substring(point.IndexOf("Tây Lương Hà Thủ Ô") + 19);
                        //point = point.Substring(41);
                        MissionX = TDT.ParseInt(point);
                        //point = Regex.Replace(point, "[\\S\\s]*Hỏa Diệm Hoa Tiên", "");
                        MissionY = TDT.ParseInt(point.Substring(2 + MissionX.ToString().Length));
                    }
                    else if (point.Contains("Hỏa Diệm Hoa Tiên"))
                    {
                        point = point.Substring(point.IndexOf("Hỏa Diệm Hoa Tiên") + 18);
                        //point = point.Substring(41);
                        MissionX = TDT.ParseInt(point);
                        //point = Regex.Replace(point, "[\\S\\s]*Hỏa Diệm Hoa Tiên", "");
                        MissionY = TDT.ParseInt(point.Substring(2 + MissionX.ToString().Length));
                    }
                    else if (point.Contains("Hỏa Diệm Hoa Yêu"))
                    {
                        point = point.Substring(point.IndexOf("Hỏa Diệm Hoa Yêu") + 17);
                        MissionX = TDT.ParseInt(point);
                        //point = Regex.Replace(point, "[\\S\\s]*Hỏa Diệm Hoa Yêu", "");
                        MissionY = TDT.ParseInt(point.Substring(2 + MissionX.ToString().Length));
                    }
                    else if (point.Contains("Hỏa Diệm Hoa"))
                    {
                        point = point.Substring(point.IndexOf("Hỏa Diệm Hoa") + 13);
                        MissionX = TDT.ParseInt(point);
                        //point = Regex.Replace(point, "[\\S\\s]*Hỏa Diệm Hoa", "");
                        MissionY = TDT.ParseInt(point.Substring(2 + MissionX.ToString().Length));
                    }
                    MissionState = "GoHoa";
                    return;
                }
                //#{MZPVE_150812_1664*Ẳ                *132 *220}
                if (point.Contains("#{MZPVE_150812_1664") || point.Contains("#{MZPVE_150812_258"))
                {
                    point = point.Replace("#{MZPVE_150812_1664", "");
                    point = point.Replace("#{MZPVE_150812_258", "");
                    MissionX = TDT.ParseInt(point);
                    MissionY = TDT.ParseInt(point.Substring(5 + MissionX.ToString().Length));
                    MissionState = "GoPhuTang";
                    return;
                }
                if (point.Contains("#{MZPVE_150812_1665}") || point.Contains("#{MZPVE_150812_259}"))
                {
                    MissionX = 0;
                    MissionY = 0;
                    MissionState = "GoPhuTang";
                    return;
                }
            }
            if (MissionState == "GoPhuTang")
            {
                listPhuTang.Clear();
                float distance = (float)1.5;
                foreach (GameObject _object in Objects.All)
                {
                    if (TDT.VietLien(_object.Title).Contains(TDT.VietLien(TLBB.Name)) && TDT.VietLien(_object.Title).Contains("trieuhoi"))
                    {
                        listPhuTang.Add((int)_object.Id);
                        //else
                        //{
                        //    SelectTarget(_object.Id);
                        //    SendKey(Global.BaseSkill);
                        //    return;
                        //}
                    }
                }
                listPhuTang.Sort();
                if (listPhuTang.Count == 1)
                {
                    distance = 8;
                }
                if (MissionX == 0 || GoToEx(MissionX, MissionY, -1, false, distance))
                {
                    if (TLBB.IsRide)
                    {
                        DownRide();
                        return;
                    }
                    if (TLBB.IsQuestOpen)
                    {
                        if (QuestFrame.Click(1))
                            curPhuTang++;
                        //if (QuestFrame.Click("#{MZPVE_150812_1650}") || QuestFrame.Click("#{MZPVE_150812_242}") || QuestFrame.Click("#{MZPVE_150812_1276}"))
                        //{
                        //    curPhuTang++;
                        //}
                        QuestFrame.Close();
                    }
                    if (TLBB.IsRide)
                    {
                        DownRide();
                        return;
                    }
                    listPhuTang.Clear();
                    CareObject = null;
                    foreach (GameObject _object in Objects.All)
                    {
                        if (TDT.VietLien(_object.Title).Contains(TDT.VietLien(TLBB.Name)) && TDT.VietLien(_object.Title).Contains("trieuhoi"))
                        {
                            CareObject = _object;
                            listPhuTang.Add((int)_object.Id);
                            //else
                            //{
                            //    SelectTarget(_object.Id);
                            //    SendKey(Global.BaseSkill);
                            //    return;
                            //}
                        }
                    }
                    listPhuTang.Sort();
                    if (listPhuTang.Count == 1)
                    {
                        if (CareObject != null)
                        {
                            if (GoToEx(CareObject))
                            {
                                if (TrueStandTime.Elapsed.TotalSeconds >= 4)
                                    FixKetMap();
                                UseSkill(3, listPhuTang[0]);
                            }
                        }
                        return;
                    }
                    else if (listPhuTang.Count > 0)
                    {
                        if (CareObject != null)
                        {
                            if (!TLBB.IsBienThan)
                            {
                                if (curPhuTang > listPhuTang.Count - 1)
                                    curPhuTang = 0;
                                if (GoToEx(CareObject))
                                {
                                    if (TrueStandTime.Elapsed.TotalSeconds >= 4)
                                        FixKetMap();
                                    Talk(listPhuTang[curPhuTang]);
                                }
                            }
                        }
                        return;
                    }
                    foreach (var item in PacketItems.All)
                    {
                        if (GAMEDIC.ChienMinhItemThuThap.ContainsKey(item.Name))
                        {
                            PlayerPackageUseItem((int)item.Index);
                            MissionState = "UseThuThap";
                            return;
                        }
                    }
                }
                else
                {
                    return;
                }
            }
            if (MissionState == "GoHoa")
            {
                if (MissionX == 0 || GoToEx(MissionX, MissionY))
                {
                    if (ForcePickItem())
                        return;
                    if (TLBB.IsRide)
                    {
                        DownRide();
                        return;
                    }
                    if (TLBB.IsQuestOpen)
                    {
                        QuestFrame.Click(1);
                        QuestFrame.ClickAll();
                        QuestFrame.Close();
                        // Cô nương hãy nói
                        //if (QuestFrame.Click("#{MZPVE_150812_1767}") || QuestFrame.Click("#{MZPVE_150812_436}") || QuestFrame.Click("#{MZPVE_150812_1393}"))
                        //    return;
                        //// Trả lời
                        //if (QuestFrame.Click("#{MZPVE_150812_1840}") || QuestFrame.Click("#{MZPVE_150812_570}") || QuestFrame.Click("#{MZPVE_150812_1466}"))
                        //    return;
                        //// Nhận dược liệu
                        //if (QuestFrame.Click("#{MZPVE_150812_1843}") || QuestFrame.Click("#{MZPVE_150812_573}") || QuestFrame.Click("#{MZPVE_150812_1469}"))
                        //{
                        //    QuestFrame.Close();
                        //    return;
                        //}
                        ////if (QuestFrame.Click("#{MZPVE_150812_1764}") || QuestFrame.Click("#{MZPVE_150812_430}"))
                        ////{
                        ////    QuestFrame.Close();
                        ////}
                        //if (QuestFrame.Click(1))
                        //    QuestFrame.Close();
                    }
                    // #{MZPVE_150812_1767} -- Cô nương hãy nói
                    // #{MZPVE_150812_1840} -- Thạch lựu
                    // #{MZPVE_150812_1843} -- Nhận dược liệu
                    foreach (GameObject _object in Objects.All)
                    {
                        if (_object.Name == TLBB.Name)
                            continue;
                        if (TDT.VietLien(_object.Title).Contains(TDT.VietLien(TLBB.Name)) && _object.Menpai == 29)
                        {
                            SelectTarget((int)_object.Id);
                            SendKey(Global.BaseSkill);
                            return;
                        }
                        if ((TDT.VietLien(_object.Title).Contains(TDT.VietLien(TLBB.Name)) && TDT.VietLien(_object.Title).Contains("linhduoc")) || TDT.VietLien(_object.Name).Contains("tayluonghathuo") || TDT.VietLien(_object.Name).Contains("hoadiemhoa") || TDT.VietLien(_object.Name).Contains("thuongmoctienthao") || TDT.VietLien(_object.Name).Contains("quynhhoa") || TDT.VietLien(_object.Name).Contains("ngochoangdaothao"))
                        {
                            if (!TDT.VietLien(_object.Name).Contains("hoadiemhoatien"))
                            {
                                if (GoToEx(_object))
                                {
                                    if (TrueStandTime.Elapsed.TotalSeconds >= 4)
                                        FixKetMap();
                                    UseSkill(3, _object.Id);
                                }

                                return;
                            }
                        }
                        if (TDT.VietLien(_object.Title).Contains(TDT.VietLien(TLBB.Name)) && (TDT.VietLien(_object.Title).Contains("banghuu") || (TDT.VietLien(_object.Title).Contains("ame?")) && !_object.CleanName.Contains("nhuocthuybanglien")))
                        {
                            Talk(_object.Id);
                            return;
                        }
                        if (TDT.VietLien(_object.Title).Contains(TDT.VietLien(TLBB.Name)) && TDT.VietLien(_object.Title).Contains("ke"))
                        {
                            if (_object.IsNPC)
                            {
                                Talk(_object.Id);
                                return;
                            }
                            else
                            {
                                SelectTarget(_object.Id);
                                SendKey(Global.BaseSkill);
                                return;
                            }
                        }

                        if (TDT.VietLien(_object.Title).Contains(TDT.VietLien(TLBB.Name)))
                        {
                            if (_object.IsNPC)
                            {
                                Talk(_object.Id);
                                return;
                            }
                            else
                            {
                                UseSkill(3, _object.Id);
                                //SelectTarget(_object.Id);
                                //SendKey(Global.BaseSkill);
                                return;
                            }
                        }
                    }
                    foreach (var item in PacketItems.All)
                    {
                        if (item.ClearName == "cuocnguhanh")
                        {
                            PlayerPackageUseItem((int)item.Index);
                            MissionState = "";
                            return;
                        }
                    }
                }
                else
                {
                    return;
                }
            }
            MissionState = "";
        }

    }
}
