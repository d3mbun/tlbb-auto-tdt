using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace _i
{
    partial class Game
    {
        public void _NhiemVuCoBan()
        {
            if (Missions.Contains(MissionsType.NhiemVuThangCap))
            {
                if (!Global.IsThangCap)
                {
                    RemoveMission(MissionsType.NhiemVuThangCap);
                    return;
                }
            }
            if (!IsOneSec)
                return;
            if (IsThoiBong)
                return;
   
            //SetMenPai = Global.GlSetMenPai;
            if (Address.GameType == 1)
                VaoPhai();
            if (TLBB.Lvl == 10 && (TLBB.Menpai == 0xFFFFFFFF || TLBB.Menpai == 0))
            {
                return;
            }

            if (IdleTime % 10 == 0)
            {
                MissionState = "";
                return;
            }

            if (MissionState == "DoTask")
            {
                try
                {
                    if (ScriptCoBan.IsNeedBenThan)
                    {
                        if (!TLBB.IsBienThan)
                        {
                            PushDebugMessage("Nhân vật mất biến thân. Hủy nhiệm vụ");
                            MissionState = "";
                            swTaskCoban = Stopwatch.StartNew();
                            LUA.DeleteMission(TaskCoBan.Name);
                            CuTaskCoBanName = "";
                            return;
                        }
                    }
                    if (CuTaskCoBanName != TaskCoBan.Name)
                    {
                        CuTaskCoBanName = TaskCoBan.Name;
                        swTaskCoban = Stopwatch.StartNew();
                    }
                    else
                    {
                        if (TDT.ParseAllInt(ScriptCoBan.Level) <= 30)
                        {
                            if (swTaskCoban.Elapsed.TotalSeconds <= 360)
                            {
                                PushDebugMessageEx("Tự động hủy nhiệm vụ " + TaskCoBan.Name + " sau " + (int)(360 - swTaskCoban.Elapsed.TotalSeconds) + "s");
                            }
                            else
                            {
                                MissionState = "";
                                swTaskCoban = Stopwatch.StartNew();
                                LUA.DeleteMission(TaskCoBan.Name);
                                CuTaskCoBanName = "";
                            }
                        }
                    }
                }
                catch { }
            }
            if (CoBanFailTime.IsRunning)
            {
                if (CoBanFailTime.Elapsed.TotalSeconds >= 90)
                {
                    CoBanFailTime = new Stopwatch();
                }
                else
                {
                    if (IdleTime >= 3)
                    {
                        if (TLBB.Lvl >= 10)
                        {
                            if (GoToEx(243, 170, MAP.DonHoang))
                            {
                                if (TLBB.IsRide)
                                    DownRide();
                                ForceAttack();
                            }
                        }
                        else
                        {
                            if (GoToEx(133, 206, MAP.VoLuongSon))
                            {
                                if (TLBB.IsRide)
                                    DownRide();
                                ForceAttack();
                            }
                        }
                    }
                }
                return;
            }
            if (TLBB.Lvl > 40)
            {
                if (TLBB.HPPercent < 20)
                {
                    PushMissions(MissionsType.TriLieu);
                }
            }
            if (Setting.Is("checkNhanx2LuyenCap") && !Missions.Contains(MissionsType.NgoChanNguyen) && !Missions.Contains(MissionsType.NhiemVuKNBKhoa))
            {
                if (TLBB.Lvl >= 10 && !IsX2Coban && TLBB.Menpai != 0)
                {
                    IsX2Coban = true;
                    PushMissions(MissionsType.NhanX2);
                }
            }


            if (SecCount % 10 == 0)
            {
                if (TLBB.Lvl <= 30 && Missions.Contains(MissionsType.NhiemVuThangCap) && Address.GameType == 1)
                {
                    DoStringEx("setmetatable(_G, {__index = FreshManMission_JLYH_Env}); if this:IsVisible() then FreshManMission_JLYH_OK_Clicked(); end");
                    Thread.Sleep(100);
                    DoStringEx("setmetatable(_G, {__index = MessageBox_Self_Env}); if this:IsVisible() and string.find(MessageBox_Self_Text:GetText(),'Sau khi s') and string.len(MessageBox_Self_Text:GetText()) == 49 then MessageBox_Self_OK_Clicked(); end");
                }
            }

            if (Global.Paused || IdleTime > 60)
            {
                MissionState = "";
                IdleTime = 0;
                IsTalkToNpc = 0;
                return;
            }
            if (MissionState == "MissionContinute")
            {
                int index = -1;
                if (ScriptCoBan != null && (ScriptCoBan.Name == "#{DHRW_130904_235}" || ScriptCoBan.Name == "#{DHRW_130904_236}" || ScriptCoBan.Name == "#{DHRW_130904_237}" || ScriptCoBan.Name == "Giải thích rõ" || ScriptCoBan.Name == "Vết Đỏ Thứ 2" || ScriptCoBan.Name == "Nhân vật kịch tình" || ScriptCoBan.Name == "Tuyệt Tình Kiếm" || ScriptCoBan.Name == "Liêu Quân Nguyên Soái") || ScriptCoBan.Name == "#{DHRW_130904_238}")
                {

                }
                else
                {
                    index = -1;
                    if (TLBB.IsNoiCong)
                        index = ScriptCoBan.CompleteNoi;
                    else
                        index = ScriptCoBan.CompleteNgoai;
                    if (index != -1)
                    {
                        LUA.QuestFrameMissionComplete(index);
                        return;
                    }
                    else
                    {

                    }
                    LUA.QuestFrameMissionContinue();
                }
                Thread.Sleep(500);
                index = -1;
                if (TLBB.IsNoiCong)
                    index = ScriptCoBan.CompleteNoi;
                else
                    index = ScriptCoBan.CompleteNgoai;
                if (index != -1)
                {
                    LUA.QuestFrameMissionComplete(index);
                }
                else
                {
                    LUA.QuestFrameMissionComplete();
                }
                Thread.Sleep(500);
                QuestFrameAccept();
                MissionState = "";
                return;
            }



            if (TLBB.PlayerState != 0)
            {
                return;
            }
            if (TLBB.MapId == 551 && !GameTask.Have(this, "#{YD_100806_56}") && MissionState == "")
            {
                if (Objects.Monters.Count > 0)
                {
                    DownRide();
                    ForceAttack();
                    swClearMontersTime = Stopwatch.StartNew();
                }
                if (swStandTime.Elapsed.TotalSeconds > 2)
                {
                    if (swClearMontersTime.Elapsed.TotalSeconds > 2)
                    {
                        MoveNext();
                    }
                }
                return;
            }


            if (TLBB.MapId == MAP.TrânLongKỳCuộc && !IsBossDie && MissionState == "" && !GameTask.Have(this, "#{YD_100806_52}"))
            {
                foreach (GameObject _object in Objects.All)
                {
                    if (TDT.VietLien(_object.Name) == "tranlungkyhon" && _object.HP == 0)
                    {
                        IsBossDie = true;
                    }
                }
                if (TLBB.IsRide && Objects.NearMonter(20).Count > 0)
                {
                    DownRide();
                    return;
                }
                GameObject nearestMonter = Objects.Monters.OrderBy(o => o.Distance).FirstOrDefault();
                if (nearestMonter != null && nearestMonter.GetDistance(CharX, CharY) < 3)
                {
                    GoToEx(nearestMonter.X, nearestMonter.Y);
                }
                if (Objects.NearMonter(20).Count > 0)
                {
                    ForceAttack();
                    return;
                }
                if (ClearTime.Elapsed.TotalSeconds > 5 && TDT.GetDistance(CharX, CharY, 40, 40) > 8 && TDT.GetDistance(CharX, CharY, 62, 62) < 3)
                {
                    GoToEx(62, 62);
                }
                return;
            }

            //#{LSHDR_150203_15}
            if (TLBB.MapId == MAP.TamTaiHiepCocDon && !GameTask.Have(this, "#{LSHDR_150203_15}") && MissionState == "")
            {
                if (tranTime.Elapsed.TotalSeconds < 10)
                {
                    if (TLBB.IsQuestOpen)
                    {
                        QuestFrame.Click("#{LSHDR_150203_15}");
                        Thread.Sleep(1000);
                        LUA.DoString("QuestFrameMissionComplete(0);");
                        Thread.Sleep(1000);
                        QuestFrameAccept();
                        //QuestFrame.Close();
                        return;
                    }
                    else
                    {
                        bool ista = false;
                        foreach (GameObject _object in Objects.AllNpc)
                        {
                            ista = true;
                            Talk(_object.Id);
                        }
                        if (!ista)
                            Talk(BANG.ChiDanTamQuan);
                    }
                    return;
                }
                if (TrueClearMonterTime.Elapsed.TotalSeconds < 1)
                {
                    DownRide();
                    ForceAttack();
                }
                if (swStandTime.Elapsed.TotalSeconds >= 1)
                {
                    int[,] point = TamTaiHiepCocPoint;
                    if (CurMapATIndex != -1 && CurMapATIndex <= point.GetLength(0) - 1)
                    {
                        if (Objects.NearMonter(25, point[CurMapATIndex, 0], point[CurMapATIndex, 1]).Count > 0)
                            ClearTime = Stopwatch.StartNew();
                    }
                    if (MapATIndex == -1)
                    {
                        MapATIndex++;
                    }
                    if ((MapATIndex <= point.GetLength(0) - 1) && TDT.GetDistance(CharX, CharY, point[MapATIndex, 0], point[MapATIndex, 1]) <= 2)
                    {
                        MapATIndex++;
                    }
                    if (ClearTime.Elapsed.TotalSeconds > 4 || MapATIndex == 0)
                    {
                        if (MapATIndex == 5 || MapATIndex == 6)
                        {
                            if (ClearTime.Elapsed.TotalSeconds < 30)
                                return;
                        }
                        if (MapATIndex == 4 || MapATIndex == 5)
                        {
                            if (TLBB.PlayerState == 2)
                                ClearTime = Stopwatch.StartNew();
                        }
                        if (MapATIndex <= point.GetLength(0) - 1)
                        {
                            if (!TLBB.IsRide && TLBB.HaveRide)
                            {
                                UpRide();
                                return;
                            }
                            GoToEx(point[MapATIndex, 0], point[MapATIndex, 1]);
                            CurMapATIndex = MapATIndex;
                            ClearTime = Stopwatch.StartNew();
                            return;
                        }
                    }
                }
                return;
            }

            // #{YD_100806_64} gian hồ tà đạo
            if (TLBB.MapId > 551 && TLBB.MapId != MAP.DaoHoa && TLBB.MapId != 652 && TLBB.MapId != QUYCOC.Id && TLBB.MapId != DUONGMON.Id && !GameTask.Have(this, "#{YD_100806_64}") && MissionState == "")
            {
                if (!IsTalkNhiemVu)
                {
                    foreach (GameObject obj in Objects.All)
                    {
                        if (obj.CleanName == "nguoichidanmonphai")
                        {
                            Talk(obj.Id);
                            Thread.Sleep(1000);
                            if (QuestFrame.Click("#{YD_100806_64}"))
                            {
                                IsTalkNhiemVu = true;
                                Thread.Sleep(1000);
                                LUA.QuestFrameMissionComplete();
                            }
                            break;
                        }
                    }
                }

                if (Objects.Monters.Count > 0)
                {
                    DownRide();
                    ForceAttack();
                    swClearMontersTime = Stopwatch.StartNew();
                }
                if (swStandTime.Elapsed.TotalSeconds > 2)
                {
                    if (swClearMontersTime.Elapsed.TotalSeconds > 2)
                    {
                        MoveNext();
                    }
                }
                return;
            }

            if (MissionState == "")
            {
                if (!TLBB.IsTogleMission)
                {
                    TogleMission();
                    Thread.Sleep(1000);
                }
                CloseMission();
                MissionState = "CloseMission";
                Thread.Sleep(1000);
            }
            if (MissionState == "CloseMission")
            {
                foreach (GameTask task in GameTask.Enum(this))
                {
                    if (GAMEDIC.CotTruyen.Contains(task.Name.Trim()))
                    {
                        continue;
                    }
                    if (!Missions.Contains(MissionsType.NhiemVuExp))
                    {
                        if (GAMEDIC.NhiemVuEXP.Contains(task.Name.Trim()))
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if (!GAMEDIC.NhiemVuEXP.Contains(task.Name.Trim()))
                        {
                            continue;
                        }
                    }
                    if (!Missions.Contains(MissionsType.NgoChanNguyen))
                    {
                        if (GAMEDIC.NgoChanNguyen.Contains(task.Name.Trim()))
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if (!GAMEDIC.NgoChanNguyen.Contains(task.Name.Trim()))
                        {
                            continue;
                        }
                    }
                    if (!Missions.Contains(MissionsType.NhiemVuKNBKhoa))
                    {
                        if (GAMEDIC.NhiemVuKNBKhoa.Contains(task.Name.Trim()))
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if (!GAMEDIC.NhiemVuKNBKhoa.Contains(task.Name.Trim()))
                        {
                            continue;
                        }
                    }
                    if (task.Name.Contains("Thiên sư kỳ đãi"))
                    {
                        bool islow = false;
                        foreach (GameTask t in GameTask.Enum(this))
                        {
                            if (t == task)
                                continue;
                            else
                            {
                                if (t.Name.Contains("Thiên sư kỳ đãi"))
                                {
                                    if (TDT.ParseAllInt(t.Name) < TDT.ParseAllInt(task.Name))
                                    {
                                        islow = true;
                                        break;
                                    }
                                }
                            }
                        }
                        if (islow)
                            continue;
                    }
                    Script cs = Scripts.Get(task.Id.ToString("X8"));
                    if (cs != null)
                    {
                        if (cs.MD == "0")
                            continue;
                        if (TDT.ParseInt(Scripts.GetByName(task.Name).Level) > TLBB.Lvl)
                        {
                            continue;
                        }
                        else
                        {
                            ScriptCoBan = cs;
                            MissionState = "DoTask";
                            TaskCoBan = task;
                        }
                        return;
                    }
                    if (Scripts.GetByName(task.Name) != null)
                    {
                        if ((!Missions.Contains(MissionsType.NgoChanNguyen) && !Missions.Contains(MissionsType.NhiemVuKNBKhoa) && !Missions.Contains(MissionsType.NhiemVuExp)) && (TDT.ParseInt(Scripts.GetByName(task.Name).Level) > TLBB.Lvl || TDT.ParseInt(Scripts.GetByName(task.Name).Level) < Global.MinNv))
                        {
                            continue;
                        }
                        else
                        {
                            MissionState = "DoTask";
                            TaskCoBan = task;
                            ScriptCoBan = Scripts.GetByName(task.Name);
                        }
                        return;
                    }

                }
                MissionState = "RecvTask";
            }
            if (MissionState == "DoTask")
            {
                //if (TLBB.IsTogleMission)
                //{
                //    LUA.TogleMissionOutline();
                //}
                //    "Khoa Ngân Võ Lâm Ấn Túc Cầu Thịnh Hội",
                //  "Khoa Ngân Võ Lâm Ấn Tàng Kinh Các Nguy Cơ",
                //"Khoa Ngân Võ Lâm Ấn Phồn Mang Tào Vận",
                //"Quả Ngân Võ Lâm Ấn Bang HộiBuôn Bán",
                if (TaskCoBan.Name == "Quả Ngân Võ Lâm Ấn Bang HộiBuôn Bán")
                {
                    NhiemVuChuaLam1.Remove("Quả Ngân Võ Lâm Ấn Bang HộiBuôn Bán");
                    NhiemVuChuaLam1.Remove("Khoa Ngân Võ Lâm Ấn Phồn Mang Tào Vận");
                    NhiemVuChuaLam1.Remove("Khoa Ngân Võ Lâm Ấn Tàng Kinh Các Nguy Cơ");
                    NhiemVuChuaLam1.Remove("Khoa Ngân Võ Lâm Ấn Túc Cầu Thịnh Hội");
                    NhiemVuChuaLam1.Remove("Sinh Tài Chi Đạo");
                }
                if (TaskCoBan.Name == "Khoa Ngân Võ Lâm Ấn Phồn Mang Tào Vận")
                {
                    NhiemVuChuaLam1.Remove("Khoa Ngân Võ Lâm Ấn Phồn Mang Tào Vận");
                    NhiemVuChuaLam1.Remove("Khoa Ngân Võ Lâm Ấn Tàng Kinh Các Nguy Cơ");
                    NhiemVuChuaLam1.Remove("Khoa Ngân Võ Lâm Ấn Túc Cầu Thịnh Hội");
                    NhiemVuChuaLam1.Remove("Sinh Tài Chi Đạo");
                }
                if (TaskCoBan.Name == "Khoa Ngân Võ Lâm Ấn Tàng Kinh Các Nguy Cơ")
                {
                    NhiemVuChuaLam1.Remove("Khoa Ngân Võ Lâm Ấn Tàng Kinh Các Nguy Cơ");
                    NhiemVuChuaLam1.Remove("Khoa Ngân Võ Lâm Ấn Túc Cầu Thịnh Hội");
                    NhiemVuChuaLam1.Remove("Sinh Tài Chi Đạo");
                }
                if (TaskCoBan.Name == "Khoa Ngân Võ Lâm Ấn Túc Cầu Thịnh Hội")
                {
                    NhiemVuChuaLam1.Remove("Khoa Ngân Võ Lâm Ấn Túc Cầu Thịnh Hội");
                    NhiemVuChuaLam1.Remove("Sinh Tài Chi Đạo");
                }
                if (TaskCoBan.Name == "Sinh Tài Chi Đạo")
                {
                    NhiemVuChuaLam1.Remove("Sinh Tài Chi Đạo");
                }
                //      "Thanh Đồng Ấn-Tương Trợ Sư Môn",
                //  "Thanh Đồng Ấn-Trừ Ác",
                //"Thanh Đồng Ấn-Trừng Hung",
                if (TaskCoBan.Name == "Thanh Đồng Ấn-Trừng Hung")
                {
                    NhiemVuChuaLam2.Remove("Thanh Đồng Ấn-Trừng Hung");
                    NhiemVuChuaLam2.Remove("Thanh Đồng Ấn-Trừ Ác");
                    NhiemVuChuaLam2.Remove("Thanh Đồng Ấn-Tương Trợ Sư Môn");
                }
                if (TaskCoBan.Name == "Thanh Đồng Ấn-Trừ Ác")
                {
                    NhiemVuChuaLam2.Remove("Thanh Đồng Ấn-Trừ Ác");
                    NhiemVuChuaLam2.Remove("Thanh Đồng Ấn-Tương Trợ Sư Môn");
                }
                if (TaskCoBan.Name == "Thanh Đồng Ấn-Tương Trợ Sư Môn")
                {
                    NhiemVuChuaLam2.Remove("Thanh Đồng Ấn-Tương Trợ Sư Môn");
                }
                if (nameCobBan != TaskCoBan.Name)
                {
                    nameCobBan = TaskCoBan.Name;
                    swTaskCoban = Stopwatch.StartNew();
                }
                else
                {
                    if (swTaskCoban.Elapsed.TotalMinutes > 50)
                    {
                        LUA.DeleteMission(nameCobBan);
                        MissionState = "";
                        nameCobBan = "";
                        return;
                    }
                }
                if (TLBB.IsTogleMission)
                {
                    TogleMission();
                    return;
                }
                bool isHave = false;
                foreach (GameTask task in GameTask.Enum(this))
                {
                    if (task.Id == TaskCoBan.Id)
                    {
                        TaskCoBan = task;
                        isHave = true;
                        break;
                    }
                }
                if (!isHave)
                {
                    MissionState = "";
                    return;
                }
                if (ScriptCoBan.Name.Contains("Hổ khiếu long ngâm"))
                {
                    if (HaveItem("TaskTools3_15") && !TaskCoBan.Completed)
                    {
                        if (TLBB.MapId == MAP.VanKiemCocDem)
                        {
                            GoToEx(47, 94);
                        }
                        foreach (GameObject npc in Objects.AllNpc)
                        {
                            int npcx = (int)npc.X;
                            int npcy = (int)npc.Y;
                            if (npcx == 47 && npcy == 94)
                            {
                                Talk(npc.Id);
                            }
                            if (TLBB.IsQuestOpen)
                            {
                                foreach (QuestFrame dialog in QuestFrame.Enum(this))
                                {
                                    QuestFrameOptionClicked(dialog);
                                }
                            }
                        }
                        return;
                    }
                }

                if (TaskCoBan.Name == "#{YD_100806_56}")// ác tặc tạo phản
                {
                    if (TLBB.MapId != 551)
                    {
                        if (GoToEx(108, 110, 3))
                        {
                            if (TLBB.IsQuestOpen)
                            {
                                QuestFrame.Click("#{YD_100806_54}");
                                QuestFrame.Close();
                            }
                            else
                            {
                                Talk(62);
                            }
                        }
                    }
                    else
                    {
                        if (TLBB.IsQuestOpen)
                        {
                            QuestFrame.Click("#{YD_100806_56}");
                            MissionContinute();
                            //QuestFrame.Close();
                            return;
                        }
                        else
                        {
                            foreach (GameObject _object in Objects.AllNpc)
                            {
                                Talk(_object.Id);
                            }
                        }
                    }
                    return;
                }
                if (TaskCoBan.Name == "#{YD_100806_52}") // Ván cờ sinh tử
                {
                    if (TLBB.MapId != 550)
                    {
                        if (GoToEx(NPC.VuongTichTan))
                        {
                            if (TLBB.IsQuestOpen)
                            {
                                QuestFrameOptionClicked(402060, -1);
                                QuestFrame.Close();
                            }
                            else
                            {
                                Talk(NPC.VuongTichTan.Id);
                            }
                        }
                    }
                    else
                    {
                        if (TLBB.IsQuestOpen)
                        {
                            QuestFrameOptionClicked(500601, 746);
                            foreach (var item in PacketItems.All)
                            {
                                if (item.Type == "Icons03_1")
                                {
                                    item.Use();
                                }
                            }
                            MissionContinute();
                            //QuestFrame.Close();
                            return;
                        }
                        else
                        {
                            foreach (GameObject _object in Objects.AllNpc)
                            {
                                Talk(_object.Id);
                            }
                        }
                    }
                    return;
                }
                if (TaskCoBan.Completed || ScriptCoBan.Completed || TaskCoBan.Name == "#{SSCJ_130828_304}")
                {
                    if (TLBB.IsQuestOpen)
                    {
                        if (!TaskCoBan.Name.Contains("Thiên sư kỳ đãi"))
                        {
                            foreach (QuestFrame dialog in QuestFrame.Enum(this))
                            {
                                if (dialog.Name == "#{XSLC_130831_352}")
                                    MissionState = "";
                                if (dialog.Name == "#{XSLC_130831_346}" || dialog.Name == "#{XSLC_130831_345}" || dialog.Name == "#{XSLC_130831_350}" || dialog.Name == "#{XSLC_130831_352}")
                                {
                                    QuestFrameOptionClicked(dialog);
                                    return;
                                }
                                if (ScriptCoBan.IsClickExacly(dialog))
                                {
                                    QuestFrameOptionClicked(dialog);
                                    MissionContinute();
                                    return;
                                }
                            }
                            QuestFrame.Close();
                        }
                        else
                        {
                            foreach (QuestFrame dialog in QuestFrame.Enum(this))
                            {
                                if (TDT.VietLien(dialog.Name) == TDT.VietLien(TaskCoBan.Name))
                                {
                                    QuestFrameOptionClicked(dialog);
                                    MissionContinute();

                                    return;
                                }
                            }

                            QuestFrame.Close();
                        }
                    }
                    //#{LSHDR_150203_15} vào tam tài trận
                    if (ScriptCoBan.Name == "#{LSHDR_150203_15}")
                    {
                        if (TLBB.MapId != 652)
                        {
                            if (GoToEx(TOCHAU.TienHoanhVu))
                            {
                                if (!TLBB.IsQuestOpen)
                                {
                                    Talk(TOCHAU.TienHoanhVu);
                                }
                                else
                                {
                                    QuestFrame.Click("#{LSHDR_150203_10}");
                                    QuestFrame.Close();
                                }
                            }
                        }
                        else
                        {
                            if (TLBB.IsQuestOpen)
                            {
                                QuestFrame.Click("#{LSHDR_150203_15}");
                                Thread.Sleep(1000);
                                LUA.DoString("QuestFrameMissionComplete(0);");
                                Thread.Sleep(1000);
                                QuestFrameAccept();
                                //QuestFrame.Close();
                                return;
                            }
                            else
                            {
                                bool ista = false;
                                foreach (GameObject _object in Objects.AllNpc)
                                {
                                    ista = true;
                                    Talk(_object.Id);
                                }
                                if (!ista)
                                    Talk(BANG.ChiDanTamQuan);
                            }
                        }
                        return;
                    }
                    //#{YD_100806_64}
                    if (ScriptCoBan.Name == "#{YD_100806_64}") // giang hồ tà đạo
                    {
                        if (TLBB.MapId <= 551 || TLBB.MapId == TLBB.MapMonPhai)
                        {
                            //NPC npc = TLBB.NPCBaiSu//
                            if (DenSuMon())
                            {
                                if (!TLBB.IsQuestOpen)
                                {
                                    foreach (GameObject _object in Objects.All)
                                    {
                                        if (IsNPCSuMon(_object))
                                        {
                                            Talk(_object.Id);
                                            return;
                                        }
                                    }
                                }
                                else
                                {
                                    QuestFrame.Click("#{YD_100806_58}");
                                    MissionContinute();
                                    return;
                                }
                            }
                        }
                        else
                        {
                            if (TLBB.IsQuestOpen)
                            {
                                QuestFrame.Click("#{YD_100806_64}");
                                MissionContinute();
                                return;
                            }
                            else
                            {
                                foreach (GameObject _object in Objects.AllNpc)
                                {
                                    Talk(_object.Id);
                                }
                            }
                        }
                        return;
                    }
                    if (ScriptCoBan.Name == "Thanh Đồng Ấn-Tương Trợ Sư Môn")
                    {
                        if (DenSuMon())
                        {
                            if (!TLBB.IsQuestOpen)
                            {
                                foreach (GameObject _object in Objects.All)
                                {
                                    if (IsNPCSuMon(_object))
                                    {
                                        Talk(_object.Id);
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                foreach (QuestFrame dialog in QuestFrame.Enum(this))
                                {
                                    if (ScriptCoBan.IsClickExacly(dialog))
                                    {
                                        QuestFrameOptionClicked(dialog);
                                        MissionContinute();
                                        return;
                                    }
                                }
                            }
                        }
                    }
                    if (ScriptCoBan.Name == "#{XSLC_130831_177}") // vào phái
                    {
                        NPC npcSuMonDaiLy = DAILY.GetNPCSuMon((int)TLBB.Menpai);
                        if (npcSuMonDaiLy == null)
                        {
                            return;
                        }
                        if (GoToEx(npcSuMonDaiLy))
                        {
                            Talk((int)npcSuMonDaiLy.Id);
                            Thread.Sleep(1000);
                            foreach (QuestFrame dialog in QuestFrame.Enum(this))
                            {
                                if (ScriptCoBan.IsClickExacly(dialog))
                                {
                                    QuestFrameOptionClicked(dialog);
                                    MissionContinute();
                                    return;
                                }
                            }
                        }
                        return;
                    }
                    if (ScriptCoBan.Name == "#{XSLC_130831_190}") // bái kiến tiền bối
                    {
                        TalkTo(TLBB.NPCBaiSu);
                        return;
                    }
                    if (ScriptCoBan.Name == "#{XSLC_130831_232}") // tôn sư thượng võ
                    {
                        TalkTo(TLBB.NPCTamPhap);
                        return;
                    }
                    if (ScriptCoBan.Name == "#{XSLC_130831_305}") // võ học ảo diệu
                    {
                        TalkTo(TLBB.NPCTamPhap);
                        return;
                    }
                    if (ScriptCoBan.Name == "#{SSCJ_130828_296}")
                    {
                        if (!GoToEx(ScriptCoBan.SendNPC.X, ScriptCoBan.SendNPC.Y, ScriptCoBan.SendNPC.Map))
                            return;
                        if (TLBB.IsRide)
                        {
                            DownRide();
                            return;
                        }
                        if (TaskCoBan.Count1 == 0)
                        {
                            if (HaveItem("CircularTaskTool51_7"))
                            {
                                UseItemNhiemVu();
                            }
                        }
                        else
                        {
                            if (!GoToEx(ScriptCoBan.SendNPC))
                                return;
                            if (!TLBB.IsQuestOpen)
                            {
                                int id = GetSendNPCId(ScriptCoBan);
                                if (id != -1)
                                    Talk(id);
                            }
                            else
                            {
                                foreach (QuestFrame dialog in QuestFrame.Enum(this))
                                {
                                    if (ScriptCoBan.IsClickExacly(dialog))
                                    {
                                        QuestFrameOptionClicked(dialog);
                                        MissionContinute();
                                        return;
                                    }
                                }
                            }
                        }
                        return;
                    }
                    if (!TLBB.IsBienThan && TLBB.HaveRide && ScriptCoBan.Name != "#{SSCJ_130828_300}" && ScriptCoBan.Name != "#{SSCJ_130828_302}" && TaskCoBan.Name != "#{XSLC_130831_81}" && TaskCoBan.Name != "#{DHRW_130904_233}" && TaskCoBan.Name != "#{DHRW_130904_232}" && TaskCoBan.Name != "#{XHCJ_130822_274}")
                    {
                        if (!TLBB.IsRide && TLBB.HaveRide)
                        {
                            UpRide();
                            return;
                        }
                    }
                    if (TaskCoBan.Name.Contains("Giang hồ cứu viện"))
                    {
                        if (TLBB.MapId != NPC.TrieuThienSu.Map)
                        {
                            GoToEx(NPC.TrieuThienSu.X, NPC.TrieuThienSu.Y, NPC.TrieuThienSu.Map);
                            IsTalkToNpc = 0;
                        }
                        else
                        {
                            int distance = 2;
                            if (ScriptCoBan.Name == "#{SSCJ_130828_300}" || ScriptCoBan.Name == "#{SSCJ_130828_301}" || ScriptCoBan.Name == "#{SSCJ_130828_302}" || ScriptCoBan.Name == "#{SSCJ_130828_303}")
                            {
                                distance = 5;
                            }
                            if (TDT.GetDistance(CharX, CharY, NPC.TrieuThienSu.X, NPC.TrieuThienSu.Y) > distance)
                            {
                                GoToEx(NPC.TrieuThienSu.X, NPC.TrieuThienSu.Y);
                                IsTalkToNpc = 0;
                            }
                            else
                            {
                                if (IsTalkToNpc == 0)
                                {
                                    foreach (GameObject _object in Objects.AllNpc)
                                    {
                                        if ((int)_object.X == NPC.TrieuThienSu.X && (int)_object.Y == NPC.TrieuThienSu.Y)
                                        {
                                            Talk(_object.Id);
                                            IsTalkToNpc = 1;
                                        }
                                    }
                                }
                                else
                                {
                                    if (IsTalkToNpc++ > 2)
                                        IsTalkToNpc = 0;
                                    foreach (QuestFrame dialog in QuestFrame.Enum(this))
                                    {
                                        if (ScriptCoBan.IsClickExacly(dialog))
                                        {
                                            QuestFrameOptionClicked(dialog);
                                            MissionContinute();
                                            return;
                                        }
                                    }
                                }
                            }
                        }
                        return;
                    }

                    if (TLBB.MapId != ScriptCoBan.SendNPC.Map)
                    {
                        GoToEx(ScriptCoBan.SendNPC.X, ScriptCoBan.SendNPC.Y, ScriptCoBan.SendNPC.Map);
                        IsTalkToNpc = 0;
                    }
                    else
                    {
                        int distance = 2;
                        if (ScriptCoBan.Name == "#{SSCJ_130828_300}" || ScriptCoBan.Name == "#{SSCJ_130828_301}" || ScriptCoBan.Name == "#{SSCJ_130828_302}" || ScriptCoBan.Name == "#{SSCJ_130828_303}" || ScriptCoBan.Name == "#{SSCJ_130828_299}")
                        {
                            distance = 5;
                        }
                        distance = 5;
                        if (TDT.GetDistance(CharX, CharY, ScriptCoBan.SendNPC.X, ScriptCoBan.SendNPC.Y) > distance)
                        {
                            foreach (GameObject _object in Objects.AllNpc)
                            {
                                if (((int)_object.X == ScriptCoBan.SendNPC.X && (int)_object.Y == ScriptCoBan.SendNPC.Y) || (_object.RoundX == ScriptCoBan.SendNPC.X && _object.RoundY == ScriptCoBan.SendNPC.Y))
                                {
                                    Talk(_object.Id);
                                    IsTalkToNpc = 1;
                                }
                            }
                            GoToEx(ScriptCoBan.SendNPC.X, ScriptCoBan.SendNPC.Y);
                            IsTalkToNpc = 0;
                        }
                        else
                        {
                            if (GoToEx(ScriptCoBan.SendNPC))
                            {
                                if (!TLBB.IsQuestOpen)
                                {
                                    int talkedid = -1;
                                    int id = GetSendNPCId(ScriptCoBan);
                                    if (id != -1)
                                    {
                                        Talk(id);
                                        talkedid = id;
                                    }
                                    else
                                    {
                                        foreach (GameObject _object in Objects.AllNpc)
                                        {
                                            if (_object.Id == SelfId)
                                                continue;
                                            if (((int)_object.X == ScriptCoBan.SendNPC.X && (int)_object.Y == ScriptCoBan.SendNPC.Y) || (_object.RoundX == ScriptCoBan.SendNPC.X && _object.RoundY == ScriptCoBan.SendNPC.Y))
                                            {
                                                Talk(_object.Id);
                                                IsTalkToNpc = 1;
                                                talkedid = (int)_object.Id;
                                            }
                                        }
                                    }
                                    if (TaskCoBan.ClearName.StartsWith("anhhungcuu"))
                                    {
                                        bool havet = false;
                                        if (talkedid != -1)
                                        {
                                            foreach (GameObject _object in Objects.All)
                                            {
                                                if (talkedid == _object.Id)
                                                {
                                                    havet = true;
                                                    break;
                                                }
                                            }
                                        }
                                        if (!havet)
                                        {
                                            LUA.DeleteMission(nameCobBan);
                                            MissionState = "";
                                            nameCobBan = "";
                                            return;
                                        }
                                    }
                                }
                                else
                                {
                                    foreach (QuestFrame dialog in QuestFrame.Enum(this))
                                    {
                                        if (ScriptCoBan.IsClickExacly(dialog))
                                        {
                                            QuestFrameOptionClicked(dialog);
                                            MissionContinute();
                                            return;
                                        }
                                    }

                                }
                            }
                        }
                    }
                    return;
                }
                else
                {
                    if (TaskCoBan.Name == "Đại chiến Vạn Kiếp Cốc")
                    {
                        if (TaskCoBan.Count1 + TaskCoBan.Count2 + TaskCoBan.Count3 == 0)
                        {
                            if (GoToEx(106, 26, MAP.VanKiemCocDem))
                            {
                                foreach (GameObject _object in Objects.Monters)
                                {
                                    if (_object.CleanName == "vantrunghac")
                                    {
                                        if (_object.Menpai == 16 && _object.HP > 0)
                                        {
                                            LUA.DeleteMission(nameCobBan);
                                            MissionState = "";
                                            nameCobBan = "";
                                            return;
                                        }
                                        DownRide();
                                        SelectTarget((int)_object.Id);
                                        SendKey(Global.BaseSkill);
                                        return;
                                    }
                                }
                            }
                            return;
                        }
                        if (TaskCoBan.Count1 + TaskCoBan.Count2 + TaskCoBan.Count3 == 1)
                        {
                            if (GoToEx(79, 19, MAP.VanKiemCocDem))
                            {
                                foreach (GameObject _object in Objects.Monters)
                                {
                                    if (_object.CleanName == "nhaclaotam")
                                    {
                                        if (_object.Menpai == 16 && _object.HP > 0)
                                        {
                                            LUA.DeleteMission(nameCobBan);
                                            MissionState = "";
                                            nameCobBan = "";
                                            return;
                                        }
                                        DownRide();
                                        SelectTarget((int)_object.Id);
                                        SendKey(Global.BaseSkill);
                                        return;
                                    }
                                }
                            }
                            return;
                        }
                        if (TaskCoBan.Count1 + TaskCoBan.Count2 + TaskCoBan.Count3 == 2)
                        {
                            if (GoToEx(52, 32, MAP.VanKiemCocDem))
                            {
                                foreach (GameObject _object in Objects.Monters)
                                {
                                    if (_object.CleanName == "diepnhinuong")
                                    {
                                        if (_object.Menpai == 16 && _object.HP > 0)
                                        {
                                            LUA.DeleteMission(nameCobBan);
                                            MissionState = "";
                                            nameCobBan = "";
                                            return;
                                        }
                                        DownRide();
                                        SelectTarget((int)_object.Id);
                                        SendKey(Global.BaseSkill);
                                        return;
                                    }
                                }
                            }
                            return;
                        }
                    }

                    if (TaskCoBan.Name == "Quét sạch Ma Nhai Động")
                    {
                        if (TaskCoBan.Count1 < 10)
                        {
                            if (GoToEx(ScriptCoBan.AtkNPC1))
                            {
                                if (TLBB.IsRide)
                                    DownRide();
                                ForceAttack();
                                return;
                            }
                        }
                        else if (TaskCoBan.Count2 < 10)
                        {
                            if (GoToEx(ScriptCoBan.AtkNPC2))
                            {
                                if (TLBB.IsRide)
                                    DownRide();
                                ForceAttack();
                                return;
                            }
                        }
                        else if (TaskCoBan.Count3 < 10)
                        {
                            if (GoToEx(ScriptCoBan.AtkNPC3))
                            {
                                if (TLBB.IsRide)
                                    DownRide();
                                ForceAttack();
                                return;
                            }
                        }
                        return;
                    }
                    if (TaskCoBan.Name == "Mãng Cái Tam Kiệt")
                    {
                        if (TaskCoBan.Count1 == 0)
                        {
                            if (GoToEx(ScriptCoBan.AtkNPC1))
                            {
                                foreach (GameObject _object in Objects.Monters)
                                {
                                    if (_object.CleanName == "longthang")
                                    {
                                        DownRide();
                                        SelectTarget((int)_object.Id);
                                        SendKey(Global.BaseSkill);
                                        return;
                                    }
                                }
                            }
                            return;
                        }
                        if (TaskCoBan.Count2 == 0)
                        {
                            if (GoToEx(ScriptCoBan.AtkNPC2))
                            {
                                foreach (GameObject _object in Objects.Monters)
                                {
                                    if (_object.CleanName == "tieudoan")
                                    {
                                        DownRide();
                                        SelectTarget((int)_object.Id);
                                        SendKey(Global.BaseSkill);
                                        return;
                                    }
                                }
                            }
                            return;
                        }
                        if (TaskCoBan.Count3 == 0)
                        {
                            if (GoToEx(ScriptCoBan.AtkNPC3))
                            {
                                foreach (GameObject _object in Objects.Monters)
                                {
                                    if (_object.CleanName == "lythong")
                                    {
                                        DownRide();
                                        SelectTarget((int)_object.Id);
                                        SendKey(Global.BaseSkill);
                                        return;
                                    }
                                }
                            }
                            return;
                        }
                        //if (IsBossVanKiemCocDie == 3)
                        //    IsBossVanKiemCocDie = 0;
                        return;
                    }
                    if (TaskCoBan.Name == "#{ZYRW_120522_67}")
                    {
                        if (GoToEx(TOCHAU.TruongSiTam))
                        {
                            if (TLBB.IsQuestOpen)
                            {
                                QuestFrame.Click("#{ZYXT_120528_03}");
                                Thread.Sleep(1000);
                                DoStringEx("setmetatable(_G, {__index = ZhenYuanNingLian_Env}); if this:IsVisible() then ZYNL_NingLian_Click(); end");
                            }
                            else
                            {
                                Talk(TOCHAU.TruongSiTam);
                                Thread.Sleep(1000);
                                QuestFrame.Click("#{ZYXT_120528_03}");
                                Thread.Sleep(1000);
                                DoStringEx("setmetatable(_G, {__index = ZhenYuanNingLian_Env}); if this:IsVisible() then ZYNL_NingLian_Click(); end");
                            }
                            MissionState = "";
                        }
                        return;
                    }
                    if (TaskCoBan.Name == "#{ZYRW_120522_68}")
                    {
                        if (GoToEx(TOCHAU.TruongSiTam))
                        {
                            if (TLBB.IsQuestOpen)
                            {
                                QuestFrame.Click("#{ZYXT_120528_02}");
                                Thread.Sleep(1000);
                                DoStringEx("Clear_XSCRIPT(); Set_XSCRIPT_Function_Name('OnActivePneumaSlot'); Set_XSCRIPT_ScriptID(889903); Set_XSCRIPT_Parameter(0, 7); Set_XSCRIPT_ParamCount(1); Send_XSCRIPT();");
                            }
                            else
                            {
                                Talk(TOCHAU.TruongSiTam);
                                Thread.Sleep(1000);
                                QuestFrame.Click("#{ZYXT_120528_02}");
                                Thread.Sleep(1000);
                                DoStringEx("Clear_XSCRIPT(); Set_XSCRIPT_Function_Name('OnActivePneumaSlot'); Set_XSCRIPT_ScriptID(889903); Set_XSCRIPT_Parameter(0, 7); Set_XSCRIPT_ParamCount(1); Send_XSCRIPT();");
                            }
                            MissionState = "";
                        }

                        return;
                    }
                    if (TaskCoBan.Name == "#{ZYRW_120522_69}")
                    {
                        if (GoToEx(TOCHAU.TruongSiTam))
                        {
                            if (TLBB.IsQuestOpen)
                            {
                                QuestFrame.Click("#{ZYXT_120528_03}");
                                Thread.Sleep(1000);
                                DoStringEx("setmetatable(_G, {__index = ZhenYuanNingLian_Env}); if this:IsVisible() then for i = 0, 24 do local theAction, bLocked = Pneuma:EnumPneumaItem('pneuma', 72 + i); if theAction:GetID() ~= 0 then PushEvent('PNEUMA_CUIJIE_CONFIRM', 72 + i) break; end end end");
                                Thread.Sleep(1000);
                                DoStringEx("setmetatable(_G, { __index = LoginSelectServerQuest_Env}); if this:IsVisible() then SelectServerQuest_Bn1Click(); end");
                            }
                            else
                            {
                                Talk(TOCHAU.TruongSiTam);
                                Thread.Sleep(1000);
                                QuestFrame.Click("#{ZYXT_120528_03}");
                                Thread.Sleep(1000);
                                DoStringEx("setmetatable(_G, {__index = ZhenYuanNingLian_Env}); if this:IsVisible() then for i = 0, 24 do local theAction, bLocked = Pneuma:EnumPneumaItem('pneuma', 72 + i); if theAction:GetID() ~= 0 then PushEvent('PNEUMA_CUIJIE_CONFIRM', 72 + i) break; end end end");
                                Thread.Sleep(1000);
                                DoStringEx("setmetatable(_G, { __index = LoginSelectServerQuest_Env}); if this:IsVisible() then SelectServerQuest_Bn1Click(); end");
                            }
                            MissionState = "";
                        }
                        return;
                    }
                    if (TaskCoBan.Name == "#{ZYRW_120522_70}")
                    {

                        // trang bi chan nguyen 
                        // 70 10 93 01 00 00 5E 00 00 00 00 00 FF FF FF FF 24 06
                        if (GoToEx(TOCHAU.TruongSiTam))
                        {
                            // Talk(TOCHAU.TruongSiTam);
                            //Thread.Sleep(1000);
                            //QuestFrame.Click("#{ZYXT_120528_03}");
                            //Thread.Sleep(1000);
                            DoStringEx("Clear_XSCRIPT(); Set_XSCRIPT_Function_Name('ApplyNiangLian'); Set_XSCRIPT_ScriptID(889902); Set_XSCRIPT_Parameter(0, 169); Set_XSCRIPT_ParamCount(1); Send_XSCRIPT(); ");
                            ////////////44 9F 9B 01 00 00 39 3A 00 00 00 00 FF FF FF FF 00 80 E4 43 02 00 00 00 01 00 00 00 2C FB 25 00 CD 19 8F 01 00 00 00 00 F0 F8 25 00 27 4C 67 01 00 00 00 00 09 78 03 75 D2 05 04 75 40 52 5A 28 E3 8E 21 00 F8 1C 6B 00 E9 CA A3 70 E3 8E 21 00 00 00 00 00 FF FF FF FF 00 00 00 00 40 52 5A 28 2C 41 1F 00 38 FB 25 00 82 16 73 01 1B 6F 75 01 E4 42 1F 00 09 78 03 75 D2 05 04 75 80 FB 25 00 70 D8 6B 00 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 50 F9 25 00 3E 15 54 6F 00 00 00 00 70 D8 6B 00 00 00 00 00 00 00 00 00 00 00 5E 00 20 8B 60 4D 00 00 00 00 74 F9 25 00 00 00 FF FF A7 1B 08 00 00 00 00 00 70 F9 25 00 C5 CF 6F 73 C8 84 60 4D F8 1C 6B 00 18 8B 60 4D 88 F9 25 00 C3 E0 5D 77 20 8B 60 4D 00 00 00 00 00 00 00 00 18 8B 60 4D 9C F9 25 00 BD 14 69 75 00 00 5E 00 00 00 00 00 20 8B 60 4D B0 F9 25 00 C2 DC AC 6F 00 00 5E 00 00 00 00 00 20 8B 60 4D 28 FA 25 00 04 1A 79 01 20 8B 60 4D F4 43 1F 00 00 00 00 00 80 18 79 01 42 1A 79 01 4D 69 6E 69 8D 01 5D 77 75 17 45 75 02 00 00 00 20 FA 25 00 01 00 00 00 00 00 00 00 E0 17 45 75 A5 B7 C1 0C 02 00 00 00 98 FA 25 00 94 FA 25 00 24 00 00 00 01 00 00 00 00 00 00 00 00 00 00
                            // SendPacket("E4 52 92 00 00 00 6E 64 00 00 00 00 FF FF FF FF 2E 94 0D 00 0E 41 70 70 6C 79 4E 69 61 6E 67 4C 69 61 6E 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 4F 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 B1 08 E1 D2");
                            Thread.Sleep(1000);
                            DoStringEx("Pneuma:PickPneumaUpToBag(0)");
                            //SendPacket("C4 3D 92 00 00 00 C4 7F 00 00 00 00 FF FF FF FF 48 FF 05 00 00 00 00 00 FF FF FF FF");
                            //30 16 A4 01 00 00 23 6B 00 00 00 00 FF FF FF FF
                            //Thread.Sleep(1000);
                            //30 16 A4 01 00 00 5E 00 00 00 00 00 FF FF FF FF 00 00 00 00 37 7A 19 00 00 00 00 00 00 00 00 00 60 4E 6C 00 00 00 00 00 00 00 00 00 00 00 00 00 07 00 00 00 00 00 00 00 2D 75 85 1D E8 F6 25 00 49 76 23 6B 00 00 00 00 00 00 00 00 00 00 00 00 1F 00 00 00 09 00 00 00 FF FF FF FF
                            //LuaDoOneLineString("PushEvent('TOGGLE_PNEUMA_UI')");
                            //Thread.Sleep(1000);
                            //SendPacket("C4 3D 92 00 00 00 3E 7C 00 00 00 00 FF FF FF FF 2F 06 00 7C 18 F8 19 45 4D 02 00 00 4E 02 00 00 4F 02 00 00 50 02 00 00 51 02 00 00 52 02 00 00 13 00 00 00 54 02 00 00 55 02 00 00 56 02 00 00 57 02 00 00 58 02 00 00 59 02 00 00 5A 02 00 00 5C 02 00 00 5D 02 00 00 5E 02 00 00 5F 02 00 00 60 02 00 00 61 02 00 00 62 02 00 00 63 02 00 00 64 02 00 00 65 02 00 00 36 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 34 37 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 B9 03 E1 D2");
                            //SendPacket("C4 3D 92 00 00 00 3E 7C 00 00 00 00 FF FF FF FF 2F 06 00 7C 18 F8 19 45 4D 02 00 00 4E 02 00 00 4F 02 00 00 50 02 00 00 51 02 00 00 52 02 00 00 13 00 00 00 54 02 00 00 55 02 00 00 56 02 00 00 57 02 00 00 58 02 00 00 59 02 00 00 5A 02 00 00 5C 02 00 00 5D 02 00 00 5E 02 00 00 5F 02 00 00 60 02 00 00 61 02 00 00 62 02 00 00 63 02 00 00 64 02 00 00 65 02 00 00 36 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 34 37 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 B9 03 E1 D2");
                            //SendPacket("C4 3D 92 00 00 00 3E 7C 00 00 00 00 FF FF FF FF 24 06 00 7C 08 F8 19 45 4D 02 00 00 4E 02 00 00 4F 02 00 00 50 02 00 00 51 02 00 00 52 02 00 00 13 00 00 00 54 02 00 00 55 02 00 00 56 02 00 00 57 02 00 00 58 02 00 00 59 02 00 00 5A 02 00 00 5C 02 00 00 5D 02 00 00 5E 02 00 00 5F 02 00 00 60 02 00 00 61 02 00 00 62 02 00 00 63 02 00 00 64 02 00 00 65 02 00 00 36 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 33 36 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 A2 18 78 62");
                            //////////////70 10 93 01 00 00 5E 00 00 00 00 00 FF FF FF FF 24 06 00 00 06 00 00 00 00 00 00 45 00 00 5E 00 60 69 88 4F 20 00 00 00 CC CC E0 42 1A 03 87 11 40 1B 01 00 08 F7 25 00 F8 F6 25 00 7C 6A 0A 6B A8 DC 87 4F 01 00 00 00 58 69 88 4F 4D 02 00 00 4E 02 00 00 4F 02 00 00 50 02 00 00 51 02 00 00 52 02 00 00 53 02 00 00 54 02 00 00 55 02 00 00 56 02 00 00 57 02 00 00 58 02 00 00 59 02 00 00 5A 02 00 00 5C 02 00 00 5D 02 00 00 5E 02 00 00 5F 02 00 00 60 02 00 00 61 02 00 00 62 02 00 00 63 02 00 00 64 02 00 00 65 02 00 00 33 36 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 36
                            //SendPacket("C4 3D 92 00 00 00 3E 7C 00 00 00 00 FF FF FF FF 24 06 00 7C 08 F8 19 45 4D 02 00 00 4E 02 00 00 4F 02 00 00 50 02 00 00 51 02 00 00 52 02 00 00 13 00 00 00 54 02 00 00 55 02 00 00 56 02 00 00 57 02 00 00 58 02 00 00 59 02 00 00 5A 02 00 00 5C 02 00 00 5D 02 00 00 5E 02 00 00 5F 02 00 00 60 02 00 00 61 02 00 00 62 02 00 00 63 02 00 00 64 02 00 00 65 02");
                            SendPacket(HexToString(AddressGameExe + 0x62FE50) + " 00 00 00 00 00 00 00 00 FF FF FF FF 24 06");
                            Thread.Sleep(1000);
                            //SendPacket("E4 52 92 00 00 00 DE BD 00 00 00 00 FF FF FF FF 2C 94 0D 00 10 4F 6E 55 70 67 72 61 64 65 43 6C 69 63 6B 65 64 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 06 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 B1 08 E1 D2");
                            DoStringEx("Clear_XSCRIPT(); Set_XSCRIPT_Function_Name('OnUpgradeClicked'); Set_XSCRIPT_ScriptID(889900); Set_XSCRIPT_Parameter(0, 6); Set_XSCRIPT_ParamCount(1); Send_XSCRIPT(); ");
                            Thread.Sleep(1000);
                            MissionState = "";
                        }
                        return;
                    }
                    //LuaDoOneLineString("setmetatable(_G, {__index = ZhenYuanNingLian_Env}); if this:IsVisible() then for i = 0, 24 do local theAction, bLocked = Pneuma:EnumPneumaItem('pneuma', 72 + i); if theAction:GetID() ~= 0 then Pneuma:PickPneumaUpToBag(i); end end end");
                    //Mua thuốc      
                    if (TaskCoBan.Name.Contains("Thanh Đồng Ấn-Trừ Ác"))
                    {
                        if (GoToEx(DAILY.LoTamThat))
                        {
                            if (TLBB.IsShopOpen)
                            {
                                int index = -1;
                                foreach (var item in PacketItems.Shop)
                                {
                                    if (TDT.VietLien(item.Name).Contains("kimsangduoc"))
                                    {
                                        index = (int)item.Index;
                                    }
                                }
                                if (index != -1)
                                {
                                    Buy(index);
                                    Thread.Sleep(500);
                                }
                            }
                            else
                            {
                                if (TLBB.IsQuestOpen)
                                {
                                    QuestFrame.Click("Mua thuốc");
                                }
                                else
                                {
                                    Talk(DAILY.LoTamThat);
                                }
                            }
                        }
                        return;
                    }
                    if (TaskCoBan.Name.Contains("Khoa Ngân Võ Lâm Ấn Túc Cầu Thịnh Hội"))
                    {
                        if (GoToEx(LACDUONG.NhuePhucTuong))
                        {
                            if (TLBB.IsShopOpen)
                            {
                                int index = -1;
                                foreach (var item in PacketItems.Shop)
                                {
                                    if (TDT.VietLien(item.Name).Contains("bachngocbinh"))
                                    {
                                        index = (int)item.Index;
                                    }
                                }
                                if (index != -1)
                                {
                                    Buy(index);
                                    Thread.Sleep(500);
                                }
                            }
                            else
                            {
                                if (TLBB.IsQuestOpen)
                                {
                                    QuestFrame.Click("Buôn bán tạp hóa");
                                }
                                else
                                {
                                    Talk(LACDUONG.NhuePhucTuong);
                                }
                            }
                        }
                        return;
                    }
                    if (ScriptCoBan.IsUseItem)
                    {
                        if (GoToEx(ScriptCoBan.AtkNPC1))
                        {
                            if (ScriptCoBan.IsPick)
                            {
                                if (ScriptCoBan.AtkNPC1.MD != "000")
                                {
                                    foreach (GameObject _object in Objects.NearMonter(20))
                                    {
                                        if (_object.MD == ScriptCoBan.AtkNPC1.MD)
                                        {
                                            SelectTarget(_object.Id);
                                            SendKey(Global.BaseSkill);
                                            return;
                                        }
                                    }
                                }
                                else
                                {
                                    ForceAttack();
                                }
                                ForcePickItem();
                            }
                            if (TLBB.IsRide)
                            {
                                DownRide();
                                return;
                            }
                            UseItemNhiemVu();
                        }
                        return;
                    }
                    if (ScriptCoBan.IsComplete)
                    {
                        if (ScriptCoBan.AtkNPC1.X != 0 && ScriptCoBan.AtkNPC2.X != 0 && ScriptCoBan.AtkNPC3.X != 0)
                        {
                            if (TaskCoBan.Count1 == 0)
                            {
                                if (GoToEx(ScriptCoBan.AtkNPC1))
                                {
                                    if (IsTalkToNpc == 0)
                                    {
                                        foreach (GameObject _object in Objects.AllNpc)
                                        {
                                            if (((int)_object.X == ScriptCoBan.AtkNPC1.X && (int)_object.Y == ScriptCoBan.AtkNPC1.Y) || (_object.RoundX == ScriptCoBan.AtkNPC1.X && _object.RoundY == ScriptCoBan.AtkNPC1.Y))
                                            {
                                                Talk(_object.Id);
                                                IsTalkToNpc = 1;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (IsTalkToNpc++ > 2)
                                            IsTalkToNpc = 0;
                                        foreach (QuestFrame dialog in QuestFrame.Enum(this))
                                        {
                                            QuestFrameOptionClicked(dialog);
                                        }
                                    }
                                }
                            }
                            else if (TaskCoBan.Count2 == 0)
                            {
                                if (GoToEx(ScriptCoBan.AtkNPC2))
                                {
                                    if (IsTalkToNpc == 0)
                                    {
                                        foreach (GameObject _object in Objects.AllNpc)
                                        {
                                            if ((int)_object.X == ScriptCoBan.AtkNPC2.X && (int)_object.Y == ScriptCoBan.AtkNPC2.Y)
                                            {
                                                Talk(_object.Id);
                                                IsTalkToNpc = 1;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (IsTalkToNpc++ > 2)
                                            IsTalkToNpc = 0;
                                        foreach (QuestFrame dialog in QuestFrame.Enum(this))
                                        {
                                            QuestFrameOptionClicked(dialog);
                                        }
                                    }
                                }
                            }
                            else if (TaskCoBan.Count3 == 0)
                            {
                                if (GoToEx(ScriptCoBan.AtkNPC3))
                                {
                                    if (IsTalkToNpc == 0)
                                    {
                                        foreach (GameObject _object in Objects.AllNpc)
                                        {
                                            if ((int)_object.X == ScriptCoBan.AtkNPC3.X && (int)_object.Y == ScriptCoBan.AtkNPC3.Y)
                                            {
                                                Talk(_object.Id);
                                                IsTalkToNpc = 1;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (IsTalkToNpc++ > 2)
                                            IsTalkToNpc = 0;
                                        foreach (QuestFrame dialog in QuestFrame.Enum(this))
                                        {
                                            QuestFrameOptionClicked(dialog);
                                        }
                                    }
                                }
                            }
                        }
                        else if (GoToEx(ScriptCoBan.AtkNPC1))
                        {
                            if (IsTalkToNpc == 0)
                            {
                                //Talk(Memory.Hex2Int(ScriptCoBan.AtkNPC1.MD));
                                foreach (GameObject _object in Objects.AllNpc)
                                {
                                    if (((int)_object.X == ScriptCoBan.AtkNPC1.X && (int)_object.Y == ScriptCoBan.AtkNPC1.Y) || (_object.RoundX == ScriptCoBan.AtkNPC1.X && _object.RoundY == ScriptCoBan.AtkNPC1.Y))
                                    {
                                        Talk(_object.Id);
                                        IsTalkToNpc = 1;
                                    }
                                }
                            }
                            else
                            {
                                if (ScriptCoBan.Name == "#{SSCJ_130828_296}")
                                {
                                    if (QuestFrame.All(this).Contains("#{SSCJ_130828_85}"))
                                    {
                                        QuestFrameOptionClicked(QuestFrame.Enum(this)[1]);
                                        return;
                                    }
                                    if (QuestFrame.All(this).Contains("#{SSCJ_130828_87}"))
                                    {
                                        QuestFrameOptionClicked(QuestFrame.Enum(this)[2]);
                                        return;
                                    }
                                    if (QuestFrame.All(this).Contains("#{SSCJ_130828_90}"))
                                    {
                                        QuestFrameOptionClicked(QuestFrame.Enum(this)[1]);
                                        return;
                                    }
                                    if (QuestFrame.All(this).Contains("#{SSCJ_130828_93}"))
                                    {
                                        QuestFrameOptionClicked(QuestFrame.Enum(this)[2]);
                                        return;
                                    }
                                    if (QuestFrame.All(this).Contains("#{SSCJ_130828_96}"))
                                    {
                                        QuestFrameOptionClicked(QuestFrame.Enum(this)[1]);
                                        return;
                                    }
                                    if (QuestFrame.All(this).Contains("#{SSCJ_130828_83}"))
                                    {
                                        TaskCoBan.SetComplete();
                                        return;
                                    }
                                }
                                if (ScriptCoBan.Name != "#{XHCJ_130822_272}" && IsTalkToNpc++ > 2)
                                    IsTalkToNpc = 0;
                                if (ScriptCoBan.Name == "#{XHCJ_130822_272}" && IsTalkToNpc++ > 5)
                                    IsTalkToNpc = 0;
                                foreach (QuestFrame dialog in QuestFrame.Enum(this))
                                {
                                    QuestFrameOptionClicked(dialog);
                                }
                                if (IsTalkToNpc == 2)
                                    if (ScriptCoBan.Name == "#{SSCJ_130828_301}")
                                        Buy(0);
                            }
                        }
                        return;
                    }
                    if (ScriptCoBan.IsCollect != "0")
                    {
                        if (ScriptCoBan.IsCollect == "3" || ScriptCoBan.IsCollect == "6")
                        {
                            if (TaskCoBan.Count1 == 0)
                            {
                                if (GoToEx(ScriptCoBan.AtkNPC1))
                                {
                                    foreach (GameObject _object in Objects.AllNpc)
                                    {
                                        if ((int)_object.X == ScriptCoBan.AtkNPC1.X && (int)_object.Y == ScriptCoBan.AtkNPC1.Y)
                                        {
                                            DownRide();
                                            UseSkill(3, _object.Id);
                                            return;
                                        }
                                    }
                                }
                            }
                            else if (TaskCoBan.Count2 == 0)
                            {
                                if (GoToEx(ScriptCoBan.AtkNPC2))
                                {
                                    foreach (GameObject _object in Objects.AllNpc)
                                    {
                                        if ((int)_object.X == ScriptCoBan.AtkNPC2.X && (int)_object.Y == ScriptCoBan.AtkNPC2.Y)
                                        {
                                            DownRide();
                                            UseSkill(3, _object.Id);
                                            return;
                                        }
                                    }
                                }
                            }
                            else if (TaskCoBan.Count3 == 0)
                            {
                                if (GoToEx(ScriptCoBan.AtkNPC3))
                                {
                                    foreach (GameObject _object in Objects.AllNpc)
                                    {
                                        if ((int)_object.X == ScriptCoBan.AtkNPC3.X && (int)_object.Y == ScriptCoBan.AtkNPC3.Y)
                                        {
                                            DownRide();
                                            UseSkill(3, _object.Id);
                                            return;
                                        }
                                    }
                                }
                            }
                        }
                        else if (GoToEx(ScriptCoBan.AtkNPC1))
                        {
                            if (TLBB.IsRide)
                            {
                                DownRide();
                                return;
                            }
                            foreach (GameObject _object in Objects.AllNpc)
                            {
                                if (ScriptCoBan.AtkNPC1.MD != "000")
                                {
                                    if (_object.MD != ScriptCoBan.AtkNPC1.MD)
                                        continue;
                                }
                                if ((int)_object.X == ScriptCoBan.AtkNPC1.X && (int)_object.Y == ScriptCoBan.AtkNPC1.Y)
                                {
                                    UseSkill(3, _object.Id);
                                }
                            }
                        }
                        return;
                    }

                    if (ScriptCoBan.AtkNPC1.X != 0)
                    {
                        if (ScriptCoBan.IsPick)
                        {
                            if (ForcePickItem())
                                return;
                        }
                        if (TLBB.MapId != ScriptCoBan.AtkNPC1.Map)
                        {
                            GoToEx(ScriptCoBan.AtkNPC1.X, ScriptCoBan.AtkNPC1.Y, ScriptCoBan.AtkNPC1.Map);
                        }
                        else
                        {
                            float distance = 5;
                            if (ScriptCoBan.IsThuThap)
                                distance = 15;
                            if (ScriptCoBan.IsThuThap)
                            {
                                NPC npcThu = ScriptCoBan.AtkNPC1;
                                if (ThuThapIdx == 2)
                                {
                                    if (ScriptCoBan.AtkNPC2.X != 0 && ScriptCoBan.AtkNPC2.Y != 0)
                                        npcThu = ScriptCoBan.AtkNPC2;
                                }
                                if (IdleTime > 5)
                                {
                                    FixKetMap();
                                }
                                if (GoToEx(npcThu.X, npcThu.Y, npcThu.Map, false, 15))
                                {
                                    float minDistance = 9999;
                                    GameObject beTarget = null;
                                    foreach (GameObject _object in Objects.All.Where(o => o.Distance <= 20))
                                    {
                                        //lecaotri2020nhiemvucoban
                                        //if (!_object.IsTaiNguyen || TDT.ClearSign(_object.Name).EndsWith("Ngu"))
                                        //    continue;
                                        if (!string.IsNullOrEmpty(ScriptCoBan.Monter))
                                        {
                                            if (!ScriptCoBan.Monter.Contains("[" + _object.Name + "]"))
                                            {
                                                continue;
                                            }
                                        }
                                        //if (_object.MD == ScriptCoBan.AtkNPC1.MD || ScriptCoBan.AtkAny)
                                        //{
                                        _object.DistanceEx = TDT.GetDistance(CharX, CharY, _object.X, _object.Y);
                                        if (_object.DistanceEx < minDistance)
                                        {
                                            minDistance = _object.DistanceEx;
                                            beTarget = _object;
                                        }
                                        //}
                                    }
                                    if (beTarget != null)
                                    {
                                        if (TLBB.IsRide)
                                        {
                                            DownRide();
                                            return;
                                        }
                                        //lecaotri2020
                                        if (TDT.GetDistance(CharX, CharY, beTarget.X, beTarget.Y) <= 1)
                                        {
                                            //UseSkill(0xE61, beTarget.Id);
                                            if (TLBB.MapId == MAP.TayHo)
                                            {
                                                UseSkill(0xE61, beTarget.Id, 0xBF800000, 0xBF800000);
                                            }
                                            else if (TLBB.MapId == MAP.TungSon)
                                            {
                                                UseSkill(0xE62, beTarget.Id, 0xBF800000, 0xBF800000);
                                            }
                                            else if (TLBB.MapId == MAP.NhiHai)
                                            {
                                                UseSkill(0xE63, beTarget.Id, 0xBF800000, 0xBF800000);
                                            }
                                            else
                                            {
                                                //UseSkill(3, beTarget.Id, 0xBF800000, 0xBF800000);
                                                PickItem((int)beTarget.Id);
                                            }
                                        }
                                        else
                                        {
                                            GoToEx(beTarget.X, beTarget.Y);
                                        }
                                        //PickItem(beTarget.Id);
                                    }
                                    else
                                    {
                                        if (ThuThapIdx == 1)
                                            ThuThapIdx = 2;
                                        else if (ThuThapIdx == 2)
                                            ThuThapIdx = 1;
                                        if (TaskCoBan.Name == "Đầu sỏ tội ác" || TaskCoBan.Name == "Thử thách lòng thành")
                                        {
                                            if (swTaskCoban.Elapsed.TotalMinutes > 1)
                                            {
                                                LUA.DeleteMission(nameCobBan);
                                                MissionState = "";
                                                nameCobBan = "";
                                                return;
                                            }
                                        }
                                    }
                                }
                                else
                                {

                                }

                                return;
                            }
                            if (TDT.GetDistance(CharX, CharY, ScriptCoBan.AtkNPC1.X, ScriptCoBan.AtkNPC1.Y) > distance)
                            {
                                GoToEx(ScriptCoBan.AtkNPC1.X, ScriptCoBan.AtkNPC1.Y);
                            }
                            else
                            {                                
                                GameObject beTarget = Objects.Monters.Where(o => o.Distance <= 20).OrderBy(o => o.Distance).FirstOrDefault();                           
                                if (beTarget != null)
                                {
                                    if (TLBB.IsRide)
                                    {
                                        DownRide();
                                        return;
                                    }
                                    SelectTarget((int)beTarget.Id);
                                    SendKey(Global.BaseSkill);
                                }
                            }
                        }
                    }
                    return;
                }
            }
            if (MissionState == "RecvTask")
            {
                if (!TLBB.IsTogleMission)
                {
                    LUA.TogleMissionOutline();
                    Thread.Sleep(1000);
                }
                LUA.CLOSE_MISSION();
                Thread.Sleep(1000);
                MissionState = "CloseMissionOutline";
            }
            if (MissionState == "CloseMissionOutline")
            {
                //LuaDoString(Properties.Resources.LuaEx);

                DoStringEx("MISSION = ''; for iMissionType = 1, 200 do local DeployNum = GetMissionOutlineNum(iMissionType); local nMyLevel = Player:GetData('LEVEL'); for i=1, DeployNum do local MissionLevel, MinLevel, MaxLevel, strNpcName, strNpcPos, strScene, strMissionName, PosX, PosY, SceneID = GetMissionOutlineInfo(iMissionType, i); if(MissionLevel <= nMyLevel and MissionLevel ~= nil and strMissionName ~= nil) then local strInfo = ''; strNpcPos = '#{_INFOAIM'..(PosX)..','..(PosY)..','..(SceneID)..','..(strNpcName)..'}'; if strScene and strScene ~= '' then strInfo = strInfo..' #G'..strScene..' #R'..strNpcName..strNpcPos; else strInfo = strInfo..' #R'..strNpcName..strNpcPos; end if strInfo ~= nil then MISSION = MISSION .. '[' .. MissionLevel .. ']' .. ' ' .. strMissionName .. '=' .. strInfo .. ';'; end end end end return '" + TDT.RandomEmptyString() + "' .. MISSION;");
                LuaToString();
                ScriptCoBan = null;
                Thread.Sleep(500);
                string info = LuaToString(true);
                info = info.Replace("=", ":");
                info = info.Replace("   ", "");
                info = info.Replace("  ", "");
                info.Trim(';');
                info = info.Trim();
                int index = -1;
                bool issinhtai = false;
                foreach (string s in info.Split(';'))
                {
                    if (s.Trim() == "")
                        continue;
                    string lvl = s.Substring(0, s.IndexOf(' '));
                    int level = TDT.ParseInt(lvl);
                    string str = s.Substring(s.IndexOf(' ') + 1);
                    string name = "";
                    string nameex = "";
                    try
                    {
                        name = str.Split(':')[0];
                        nameex = name;
                        name = TDT.ClearSign(name).Replace(" ", "");
                    }
                    catch { }



                    if (level < Global.MinNv && (!Missions.Contains(MissionsType.NgoChanNguyen) && !Missions.Contains(MissionsType.NhiemVuKNBKhoa) && !Missions.Contains(MissionsType.NhiemVuExp)))
                        continue;


                    if (GAMEDIC.CotTruyen.Contains(nameex.Trim()))
                    {
                        continue;
                    }
                    if (!Missions.Contains(MissionsType.NhiemVuExp))
                    {
                        if (GAMEDIC.NhiemVuEXP.Contains(nameex.Trim()))
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if (!GAMEDIC.NhiemVuEXP.Contains(nameex.Trim()))
                        {
                            continue;
                        }
                    }
                    if (!Missions.Contains(MissionsType.NgoChanNguyen))
                    {
                        if (GAMEDIC.NgoChanNguyen.Contains(nameex.Trim()))
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if (!GAMEDIC.NgoChanNguyen.Contains(nameex.Trim()))
                        {
                            continue;
                        }
                    }
                    if (!Missions.Contains(MissionsType.NhiemVuKNBKhoa))
                    {
                        if (GAMEDIC.NhiemVuKNBKhoa.Contains(nameex.Trim()))
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if (!GAMEDIC.NhiemVuKNBKhoa.Contains(nameex.Trim()))
                        {
                            continue;
                        }
                    }
                    string md5 = TDT.Hasher.MD5(str);
                    int cnt = 0;

                    foreach (Script scr in Scripts.All)
                    {
                        if (string.IsNullOrEmpty(scr.Name))
                        {
                            continue;
                        }
                        //if (scr.Name.Contains("Đèn nhà ai nấy sáng"))
                        //    continue;
                        if (scr.ID + scr.Name == "")
                            continue;
                        if (scr.Name == "#{XSLC_130831_177}" && TLBB.Menpai == 0)
                            continue;
                        cnt++;
                        //if (scr.Name == "#{XSLC_130831_190}" && TLBB.Menpai == MENPAI.DaoHoa)
                        //    continue;
                        //if (scr.Name == "#{XSLC_130831_177}" && TLBB.Menpai == MENPAI.DaoHoa)
                        //    continue;
                        //if (scr.Name == "#{XSLC_130831_232}" && TLBB.Menpai == MENPAI.DaoHoa)
                        //    continue;
                        //#{XSLC_130831_232}
                        if (scr.MD.Contains(md5) || TDT.VietLien(scr.NameEx) == TDT.VietLien(name) && (TDT.ParseInt(scr.Level) == level || (scr.Name == "#{XHCJ_130822_275}" && level == 24)))
                        {
                            if (ScriptCoBan != null)
                            {
                                if (scr.Name == "Khách thuê phòng che mặt")
                                    continue;
                            }
                            if (!issinhtai)
                            {
                                //if (!ListNhiemVu.Contains(scr))
                                //    ListNhiemVu.Add(scr);
                                if (ScriptCoBan == null || TDT.ParseInt(scr.Level) < TDT.ParseInt(ScriptCoBan.Level) || scr.Name == "Sinh Tài Chi Đạo")
                                {
                                    index = cnt;
                                    MissionState = "RecvTaskDo";
                                    ScriptCoBan = scr;
                                    if (scr.Level == "")
                                    {
                                        scr.Level = level.ToString();
                                        Scripts.Save();
                                    }
                                    if (scr.Name == "Sinh Tài Chi Đạo")
                                    {
                                        issinhtai = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                if (index != -1)
                {
                    failcnt = 0;
                    PushDebugMessage("Tiến hành làm nhiệm vụ " + ScriptCoBan.Name);
                    CoBanFailTime = new Stopwatch();
                    //if(alarmCobanFail != null)
                    //{
                    //    alarmCobanFail.Dispose();
                    //    alarmCobanFail = null;
                    //}
                    return;
                }
                else
                {
                    if (Missions.Contains(MissionsType.NhiemVuExp))
                    {
                        if (failcnt++ > 5)
                        {
                            PushDebugMessage("Đã kiểm tra hết và làm xong rồi nhé");
                            if (IsByLogin)
                                IsXongBHD = true;
                            RemoveMission(MissionsType.NhiemVuExp);

                        }
                        MissionState = "";
                        return;
                    }
                    if (Missions.Contains(MissionsType.NgoChanNguyen))
                    {
                        if (failcnt++ > 4)
                        {
                            //lecaotri
                            if (!IsQuangCao)
                            {
                                DoStringEx("TXT = '#cFF0000 tieudattai#cffcccc với #GMicroAuto#cffcccc giúp ta hoàn thành chuỗi nhiệm vụ#cFF0000 Ngộ Chân Ng/uyên#cffcccc chỉ cần 1 click, quả là auto hoàn hảo';");
                                PostMessage(3, 105);
                                IsQuangCao = true;
                            }

                            PushDebugMessage("Đã kiểm tra hết và làm xong rồi nhé");
                            RemoveMission(MissionsType.NgoChanNguyen);

                        }
                        MissionState = "";
                        return;
                    }
                    if (Missions.Contains(MissionsType.NhiemVuKNBKhoa))
                    {
                        if (NhiemVuChuaLam1.Count > 0 && TLBB.Lvl >= 45)
                        {
                            foreach (Script scr in Scripts.All)
                            {
                                if (string.IsNullOrEmpty(scr.Name))
                                {
                                    continue;
                                }
                                if (scr.Name == NhiemVuChuaLam1[0])
                                {
                                    MissionState = "RecvTaskDo";
                                    ScriptCoBan = scr;
                                    IsCheckNhiemmVu = true;
                                    PushDebugMessage("Ta thử đi kiểm tra nhiệm vụ chưa làm xem");
                                    PushDebugMessage("Để kiểm tra lại lần nữa hãy Refresh nhân vật nhé");
                                    return;
                                }
                            }
                        }
                        else
                        {
                            if (NhiemVuChuaLam2.Count > 0 && TLBB.Lvl >= 30)
                            {
                                foreach (Script scr in Scripts.All)
                                {
                                    if (string.IsNullOrEmpty(scr.Name))
                                    {
                                        continue;
                                    }
                                    if (scr.Name == NhiemVuChuaLam2[0])
                                    {
                                        MissionState = "RecvTaskDo";
                                        ScriptCoBan = scr;
                                        IsCheckNhiemmVu = true;
                                        PushDebugMessage("Ta thử đi kiểm tra nhiệm vụ chưa làm xem");
                                        PushDebugMessage("Để kiểm tra lại lần nữa hãy Refresh nhân vật nhé");
                                        return;
                                    }
                                }
                            }
                        }
                        PushDebugMessage("Đã kiểm tra hết và làm xong rồi nhé");
                        if (!IsQuangCao)
                        {
                            DoStringEx("TXT = '#cFF0000 tieudattai#cffcccc với #GMicroAuto#cffcccc giúp ta hoàn thành chuỗi nhiệm vụ#cFF0000 nhận được 1020 KNB khóa#cffcccc chỉ cần 1 click, quả là auto hoàn hảo';");
                            PostMessage(3, 105);
                            IsQuangCao = true;
                        }
                        RemoveMission(MissionsType.NhiemVuKNBKhoa);
                        return;
                    }
                }
                if (CoBanFailTime.IsRunning)
                {

                }
                else
                {
                    if (!Missions.Contains(MissionsType.NhiemVuKNBKhoa) && !Missions.Contains(MissionsType.NgoChanNguyen) && !Missions.Contains(MissionsType.NhiemVuExp))
                    {
                        if (failcnt++ > 5)
                            CoBanFailTime.Start();
                    }
                }
                //if(CoBanFailTime.Elapsed.TotalSeconds > 60)
                //{

                //}
                MissionState = "";
                return;
            }
            //if (TrangThaiNhiemVuCoBan == "ReadMissionOutline")
            //{
            //    LuaDoString(Properties.Resources.LuaEx);

            //}
            if (MissionState == "RecvTaskDo")
            {

                if (TLBB.IsQuestOpen)
                {
                    foreach (QuestFrame dialog in QuestFrame.Enum(this))
                    {
                        if (ScriptCoBan.IsClickExacly(dialog))
                        {
                            QuestFrameOptionClicked(dialog);
                            Thread.Sleep(500);
                            QuestFrameAccept();
                            MissionState = "";
                            return;
                        }
                    }
                    if (ScriptCoBan.Name == "#{SSCJ_130828_304}")
                    {
                        if (QuestFrame.Click("Giang hồ cứu viện"))
                        {
                            Thread.Sleep(500);
                            QuestFrameAccept();
                            MissionState = "";
                            return;
                        }
                    }
                    if (IsCheckNhiemmVu)
                    {
                        if (checknvcount++ > 3)
                        {
                            checknvcount = 0;
                            NhiemVuChuaLam1.Remove(ScriptCoBan.Name);
                            NhiemVuChuaLam2.Remove(ScriptCoBan.Name);
                            IsCheckNhiemmVu = false;
                            PushDebugMessage("Ồ hình như " + ScriptCoBan.Name + " đã làm rồi");
                            MissionState = "";
                            return;
                        }
                    }
                }
                else
                {
                    if (ScriptCoBan.Name == "#{XSLC_130831_190}") // bái kiến tiền bối
                    {
                        TalkTo(TLBB.NPCBaiSuDaiLy);
                        return;
                    }
                    if (ScriptCoBan.Name == "#{XSLC_130831_232}") // tôn sư thượng võ
                    {
                        TalkTo(TLBB.NPCBaiSu);
                        return;
                    }
                    if (ScriptCoBan.Name == "#{XSLC_130831_305}") // võ học ảo diệu
                    {
                        TalkTo(TLBB.NPCTamPhap);
                        return;
                    }
                    //Mua thuốc      
                    if (ScriptCoBan.Name == "Thanh Đồng Ấn-Trừ Ác")
                    {
                        //NPC npc = TLBB.NPCBaiSu//
                        if (DenSuMon())
                        {
                            foreach (GameObject _object in Objects.All)
                            {
                                if (IsNPCSuMon(_object))
                                {
                                    Talk(_object.Id);
                                    return;
                                }
                            }
                        }
                        return;
                    }
                    if (ScriptCoBan.Name == "#{XHCJ_130822_274}")
                        DownRide();

                }
                if (!GoToEx(ScriptCoBan.RecvNPC))
                {
                    //Main.TDTLog(ScriptCoBan.RecvNPC.X.ToString());
                    return;
                }
                int id = GetRecvNPCId(ScriptCoBan);
                if (id != -1)
                {
                    Talk(id);
                    return;
                }

            }

            MissionState = "";
        }

        public void MissionContinute()
        {
            Thread.Sleep(500);
            int index = -1;
            if (ScriptCoBan != null && (ScriptCoBan.Name == "#{DHRW_130904_235}" || ScriptCoBan.Name == "#{DHRW_130904_236}" || ScriptCoBan.Name == "#{DHRW_130904_237}" || ScriptCoBan.Name == "Giải thích rõ" || ScriptCoBan.Name == "Vết Đỏ Thứ 2" || ScriptCoBan.Name == "Nhân vật kịch tình" || ScriptCoBan.Name == "Tuyệt Tình Kiếm" || ScriptCoBan.Name == "Liêu Quân Nguyên Soái") || ScriptCoBan.Name == "#{DHRW_130904_238}")
            {

            }
            else
            {
                index = -1;
                if (TLBB.IsNoiCong)
                    index = ScriptCoBan.CompleteNoi;
                else
                    index = ScriptCoBan.CompleteNgoai;
                if (index != -1)
                {
                    LUA.QuestFrameMissionComplete(index);
                    return;
                }
                else
                {

                }
                LUA.QuestFrameMissionContinue();
            }
            Thread.Sleep(500);
            index = -1;
            if (TLBB.IsNoiCong)
                index = ScriptCoBan.CompleteNoi;
            else
                index = ScriptCoBan.CompleteNgoai;
            if (index != -1)
            {
                LUA.QuestFrameMissionComplete(index);
            }
            else
            {
                LUA.QuestFrameMissionComplete();
            }
            Thread.Sleep(1000);
            QuestFrameAccept();
            Thread.Sleep(1000);
            QuestFrame.ClickAll();
            MissionState = "";
        }

    }
}
