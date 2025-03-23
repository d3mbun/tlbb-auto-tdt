using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;

namespace _i
{
    internal partial class Game
    {
        public void DatDoiBinhThanh()
        {
            if (QuanDoans.Count <= 0)
            {
                return;
            }
            QuanDoans.ForEach(g => g.PushMissions(MissionsType.DatDoiBinhThanh));
            if (Setting.Is("checkPhuBanDuNguoi"))
            {
                if (QuanDoans.Count < 12)
                    return;
            }

            if (GetRoundBinhThanh(RoundX, RoundY) == "Round4")
            {
                QuanDoans.ForEach(g => { if (g.StepBinhThanh < 10) g.StepBinhThanh = 10; });
            }

            QuanDoans.ForEach(g => { if (g.TLBB.MapId != MAP.BinhThanhKyTran) if (g.GoToEx(LAULAN.CaoDuong.X, LAULAN.CaoDuong.Y, LAULAN.CaoDuong.Map, true)) g.IsP = true; });
            QuanDoans.ForEach(g => g.StepBinhThanh = StepBinhThanh);
            foreach (Game game in QuanDoans)
            {
                foreach (Game g in QuanDoans)
                {
                    if (g == game)
                        continue;
                    if (game.quandoanindex == g.quandoanindex)
                    {
                        game.quandoanindex = -1;
                    }
                }
            }
            foreach (Game game in QuanDoans)
            {
                if (game.quandoanindex <= 0)
                {
                    for (int i = 0; i < QuanDoanParty1.Count; i++)
                    {
                        QuanDoanParty1[i].quandoanindex = i + 1;
                    }
                    for (int i = 0; i < QuanDoanParty2.Count; i++)
                    {
                        QuanDoanParty2[i].quandoanindex = 6 + i + 1;
                    }
                    break;
                }
            }
            if (quandoanindex != 1)
                return;
            if (IsHoiSinh)
                return;

            Stopwatch TrueQuanDoanClearMonterTime = QuanDoans.OrderBy(game => game.TrueClearMonterTime.Elapsed.TotalSeconds).FirstOrDefault().TrueClearMonterTime;            
            if (TLBB.MapId == MAP.BinhThanhKyTran) 
            {
                bool isfullgo = true;
                if (StepBinhThanh == 0)
                    StepBinhThanh = 1;
                if (StepBinhThanh == 1)
                {
                    foreach (Game game in QuanDoans)
                    {
                        if (!game.GoToEx(63, 85))
                        {
                            isfullgo = false;
                        }
                        else
                        {
                            if (game.Objects.Monters.Count > 0)
                                game.DownRide();
                        }
                    }
                    if (isfullgo && PartyMinSwBuffNM > 3 && TrueQuanDoanClearMonterTime.Elapsed.TotalSeconds > 5)
                        QuanDoans.ForEach(g => g.StepBinhThanh = 2);
                }
                if (StepBinhThanh == 2)
                {
                    List<string> points = new List<string>();
                    int x = 51;
                    int y = 54;
                    points.Add((x) + "," + (y + 2));
                    points.Add((x) + "," + (y + 6));
                    points.Add((x) + "," + (y - 2));
                    points.Add((x) + "," + (y - 6));
                    points.Add((x - 3) + "," + (y + 2));
                    points.Add((x - 3) + "," + (y + 6));
                    points.Add((x - 3) + "," + (y - 2));
                    points.Add((x - 3) + "," + (y - 6));
                    points.Add((x + 3) + "," + (y + 2));
                    points.Add((x + 3) + "," + (y + 6));
                    points.Add((x + 3) + "," + (y - 2));
                    points.Add((x + 3) + "," + (y - 6));
                    QuanDoans.ForEach(game => game.NeBayTime = Stopwatch.StartNew());
                    foreach (Game game in QuanDoans)
                    {
                        if (game.quandoanindex > 0)
                        {
                            if (!game.GoToEx(points[game.quandoanindex - 1]))
                            {
                                isfullgo = false;
                            }
                        }
                    }
                    if (isfullgo)
                    {
                        QuanDoans.ForEach(g => g.StepBinhThanh = 3);
                    }
                }
                if(StepBinhThanh == 3)
                {
                    foreach (Game game in QuanDoans)
                    {
                        if (game.Objects.Monters.Count > 0)
                            game.DownRide();
                    }
                    if (PartyMinSwBuffNM > 3 && TrueQuanDoanClearMonterTime.Elapsed.TotalSeconds > 15)
                        QuanDoans.ForEach(g => g.StepBinhThanh = 4);
                }
                if (StepBinhThanh == 4)
                {
                    foreach (Game game in QuanDoanParty1)
                    {
                        if (!game.GoToEx(134, 110))
                        {
                            isfullgo = false;
                        }
                        else
                        {
                            if (game.Objects.NearMonter(15).Count > 0)
                            {
                                game.DownRide();
                                isfullgo = false;
                            }
                        }
                    }
                    foreach (Game game in QuanDoanParty2)
                    {
                        if (!game.GoToEx(142, 64))
                        {
                            isfullgo = false;
                        }
                        else
                        {
                            if (game.Objects.NearMonter(15).Count > 0)
                            {
                                game.DownRide();
                                isfullgo = false;
                            }
                        }
                    }
                    if (isfullgo && PartyMinSwBuffNM > 3 && TrueQuanDoanClearMonterTime.Elapsed.TotalSeconds > 5)
                        QuanDoans.ForEach(g => g.StepBinhThanh = 5);
                }
                if (StepBinhThanh == 5)
                {
                    List<string> points = new List<string>();
                    points.Add("178,103");
                    points.Add("172,94");
                    points.Add("167,97");
                    points.Add("167,103");
                    points.Add("179,95");
                    points.Add("173,105");
                    points.Add("174,43");
                    points.Add("180,37");
                    points.Add("179,32");
                    points.Add("168,34");
                    points.Add("172,30");
                    points.Add("170,41");
                    QuanDoans.ForEach(game => game.NeBayTime = Stopwatch.StartNew());
                    foreach (Game game in QuanDoans)
                    {
                        if (game.quandoanindex > 0)
                        {
                            if (!game.GoToEx(points[game.quandoanindex - 1]))
                            {
                                isfullgo = false;
                            }
                        }
                    }
                    if (isfullgo)
                    {
                        QuanDoans.ForEach(g => g.StepBinhThanh = 6);
                    }
                }
                if (StepBinhThanh == 6)
                {
                    foreach (Game game in QuanDoans)
                    {
                        if (game.Objects.Monters.Count > 0)
                            game.DownRide();
                    }
                    if (PartyMinSwBuffNM > 3 && TrueQuanDoanClearMonterTime.Elapsed.TotalSeconds > 15)
                        QuanDoans.ForEach(g => g.StepBinhThanh = 7);
                }
                if (StepBinhThanh == 7)
                {
                    foreach (Game game in QuanDoans)
                    {
                        if (!game.GoToEx(200, 140))
                        {
                            isfullgo = false;
                        }
                        else
                        {
                            if (game.Objects.Monters.Count > 0)
                            {
                                game.DownRide();
                                isfullgo = false;
                            }
                        }
                    }
                    if (isfullgo && PartyMinSwBuffNM > 3 && TrueQuanDoanClearMonterTime.Elapsed.TotalSeconds > 5)
                        QuanDoans.ForEach(g => g.StepBinhThanh = 8);
                }
                if (StepBinhThanh == 8)
                {
                    QuanDoans.ForEach(game => game.NeBayTime = Stopwatch.StartNew());
                    foreach (Game game in QuanDoans)
                    {
                        if (!game.GoToEx(191, 195))
                        {
                            isfullgo = false;
                        }
                    }
                    if (isfullgo)
                    {
                        QuanDoans.ForEach(g => g.StepBinhThanh = 9);
                    }
                }
                if(StepBinhThanh == 9)
                {                   
                    foreach (Game game in QuanDoans)
                    {
                        if (game.Objects.Monters.Count > 0)
                            game.DownRide();
                    }                    
                    if (TrueClearMonterTime.Elapsed.TotalSeconds >= 3 && TrueClearMonterTime.Elapsed.TotalSeconds <= 20)
                    {
                        Point point = GetSafeDaLuatDiem();
                        if (point.X != 0)
                        {
                            QuanDoans.Where(g => g.PickTime.Elapsed.TotalSeconds > 1 && g.SWHoiSinh.Elapsed.TotalSeconds > 1).ForEach(game => game.Move(point.X, point.Y));         
                        }
                    }
                    if (PartyMinSwBuffNM > 3 && TrueQuanDoanClearMonterTime.Elapsed.TotalSeconds > 15)
                    {
                        QuanDoans.ForEach(g => g.StepBinhThanh = 10);
                    }
                }
                if (StepBinhThanh == 10)
                {
                    foreach (Game game in QuanDoans)
                    {
                        if (!game.GoToEx(95, 203))
                        {
                            isfullgo = false;
                        }
                    }
                    if (isfullgo && PartyMinSwBuffNM > 3)
                        QuanDoans.ForEach(g => g.StepBinhThanh = 11);
                }
                if (StepBinhThanh == 11)
                {
                    QuanDoans.ForEach(game => game.NeBayTime = Stopwatch.StartNew());
                    foreach (Game game in QuanDoans)
                    {
                        if (!game.GoToEx(52, 203))
                        {
                            isfullgo = false;
                        }
                    }
                    if (isfullgo)
                    {
                        QuanDoans.ForEach(g => g.StepBinhThanh = 12);
                    }
                }
                if (StepBinhThanh == 12)
                {
                    foreach (Game game in QuanDoans)
                    {
                        game.DownRide();
                    }
                }
            }
        }

        public void DatDoiYenTuO()
        {
            if (IsHoiSinh)
                return;
            if (TLBB.MapId != MAP.YenTuO || IsObjectDead("Mộ Dung Phục"))
            {
                if (FullPartyGoToEx(THAIHO.LyCuong))
                {
                    Talk(THAIHO.LyCuong.Id);
                    Thread.Sleep(500);
                    QuestFrameOptionClicked(401040, -1);
                    Thread.Sleep(500);
                    if (QuestFrame.Text.Contains("#{FBJR_091019_13}"))
                    {
                        RemoveMission(MissionsType.DatDoiYenTuO);
                        if (Main.IsBanRac)
                            Party.ForEach(game => game.PushMissions(new[] { MissionsType.TriLieu, MissionsType.BanRac, MissionsType.CatVang }));
                        PushDebugMessage("Xong Yến Tử Ô");
                    }
                }
            }
            else
            {
                if (TLBB.IsQuestOpen)
                {
                    if (QuestFrame.Text.Contains("#{dazhan_yzw_003}"))
                    {
                        if (!DeadObjects.ContainsKey("Đoàn Diên Khánh"))
                            DeadObjects["Đoàn Diên Khánh"] = Stopwatch.StartNew();
                    }
                    if (QuestFrame.All(this).Contains("#{dazhan_yzw_006}"))
                    {
                        if (!DeadObjects.ContainsKey("Cưu Ma Trí"))
                            DeadObjects["Cưu Ma Trí"] = Stopwatch.StartNew();
                    }
                    QuestFrame.ClickAll();
                    QuestFrame.Close();
                    Thread.Sleep(3000);
                }
                if (IsObjectDead("Cưu Ma Trí") || (RoundX < 165 && RoundY < 125))
                {
                    if (FullPartyGoToEx(50, 75))
                    {
                        IsWaitDoiNgu = false;
                    }
                    if (TrueStandTime.Elapsed.TotalSeconds > 30)
                    {
                        if (GetDistance(50, 75) < 3)
                            IsWaitDoiNgu = false;
                    }
                }
                else if (IsObjectDead("Đoàn Diên Khánh"))
                {
                    FullPartyGoToEx(YENTUO.TienHoanhVu, 15);
                    if (TrueClearMonterTime.Elapsed.TotalSeconds > 15 && TrueStandTime.Elapsed.TotalSeconds >= 3)
                    {
                        Talk("tienhoanhvu");
                        Talk("captainchien");
                    }
                }
                else
                {
                    FullPartyGoToEx(YENTUO.HoDienBao, 15);
                    if (TrueClearMonterTime.Elapsed.TotalSeconds > 15 && TrueStandTime.Elapsed.TotalSeconds >= 3)
                    {
                        Talk("hodienbao");
                        Talk("leopardhu");
                    }
                }
            }
        }

        public void DatDoiTuTuyetTrang()
        {
            if (IsHoiSinh)
                return;
            if (TLBB.MapId != MAP.TuTuyetTrang)
            {
                if (FullPartyGoToEx(TOCHAU.PhanThanhThanh))
                {
                    Party.ForEach(p => p.IsP = true);
                    if (TLBB.IsQuestOpen)
                    {
                        if (QuestFrame.Text.Contains("#{SJZ_100129_08}"))
                        {
                            RemoveMission(MissionsType.DatDoiTuTuyetTrang);
                            return;
                        }
                    }
                }
            }
            else
            {
                if (!Objects.All.Any(o => o.CleanName == "tanvan" && o.HP < 1 && o.HP > 0.5))
                {
                    TrieuTapEx();
                }
            }
        }

        public void DatDoiQ123ToChau()
        {
            if (Objects.NearMonter(QDistance, Qx, Qy).Count > 0)
                QToChauClearMonterTime = Stopwatch.StartNew();
            if (QToChauClearMonterTime.Elapsed.TotalMinutes > 1)
            {
                QTOCHAUSTATE = 0;
                QToChauClearMonterTime = Stopwatch.StartNew();
            }

            if (TLBB.MapId != MAP.TamTaiHiepCoc)
            {
                if (FullPartyGoToEx(TOCHAU.TienHoanhVu))
                {
                    Party.ForEach(p => p.IsP = true);
                }
            }
            else
            {
                List<string> point = new List<string>()
                    {
                        "193,135=>15,10",
                        "170,135=>15,10",
                        "165,120=>15,10",
                        "185,115=>10,25",
                        "202,102=>20,30",
                        "195,48=>20,25",

                        // dã hùng
                        "148,67=>15,10",
                        "138,73=>15,10",
                        "132,51=>15,10",
                        "120,43=>15,10",
                        "102,49=>15,10",
                        "86,39=>15,10",
                        "62,40=>15,10",
                        "55,48=>15,10",
                        "61,57=>20,10",
                        "74,75=>20,10",

                        //cát vinh
                        "51,188=>10,10"
                    };
                if (QTOCHAUSTATE <= 0)
                    QTOCHAUSTATE = 0;
                if (QTOCHAUSTATE >= point.Count)
                    QTOCHAUSTATE = point.Count - 1;
                string curpoint = point[QTOCHAUSTATE];
                int x = TDT.ParseInt(curpoint);
                int y = TDT.ParseInt(curpoint.Replace(x + ",", ""));
                Qx = x;
                Qy = y;
                curpoint = curpoint.Split(new[] { "=>" }, StringSplitOptions.None)[1];
                int distance = TDT.ParseInt(curpoint);
                int duration = TDT.ParseInt(curpoint.Replace(distance + ",", ""));
                QDistance = distance;

                Party.Where(p => p.TLBB.PlayerState == 0).ToList().ForEach(p => p.GoToEx(x, y));

                if (GetDistance(x, y) <= 3)
                {
                    if (QToChauClearMonterTime.Elapsed.TotalSeconds >= duration && TrueStandTime.Elapsed.TotalSeconds >= 3)
                    {
                        QTOCHAUSTATE++;
                        QToChauClearMonterTime = Stopwatch.StartNew();
                        return;
                    }
                }
            }
        }

        public void DatDoiPhucDia()
        {
            if (!Global.IsPhucDia)
                return;
            if (!IsOneSec)
            {
                return;
            }
            if (TLBB.MapId == MAP.LanHoanPhucDia)
            {
                if (TDT.GetDistance(CharX, CharY, 213, 120) <= 20)
                {
                    if (!DeadObjects.ContainsKey("Lý Thu Thủy"))
                        DeadObjects["Lý Thu Thủy"] = Stopwatch.StartNew();
                }
                if (TDT.GetDistance(CharX, CharY, 183, 207) <= 20)
                {
                    if (!DeadObjects.ContainsKey("Lý Thu Thủy"))
                        DeadObjects["Lý Thu Thủy"] = Stopwatch.StartNew();
                    if (!DeadObjects.ContainsKey("Thiên Sơn Đồng Lão"))
                        DeadObjects["Thiên Sơn Đồng Lão"] = Stopwatch.StartNew();
                }
                if (TDT.GetDistance(CharX, CharY, 83, 181) <= 20)
                {
                    if (!DeadObjects.ContainsKey("Lý Thu Thủy"))
                        DeadObjects["Lý Thu Thủy"] = Stopwatch.StartNew();
                    if (!DeadObjects.ContainsKey("Thiên Sơn Đồng Lão"))
                        DeadObjects["Thiên Sơn Đồng Lão"] = Stopwatch.StartNew();
                    if (!DeadObjects.ContainsKey("Vô Nhai Tử"))
                        DeadObjects["Vô Nhai Tử"] = Stopwatch.StartNew();
                }
            }
            if (TLBB.MapId != MAP.LanHoanPhucDia)
            {
                if (FullPartyGoToEx(DAILY.LyThanhLa))
                {
                    TalkEx("Lý Thanh La");
                    Thread.Sleep(1000);
                    if (Missions.Contains(MissionsType.PhucDiaKho))
                    {
                        QuestFrame.Click("#{LHFD_160203_03}");
                    }
                    else
                    {
                        QuestFrame.Click("#{LHFD_160203_02}");
                    }
                    Thread.Sleep(1000);
                    if (QuestFrame.Text.Contains("LHFD_5"))
                    {
                        RemoveMission(MissionsType.DatDoiPhucDia);
                        if (Main.IsBanRac)
                            Party.ForEach(game => game.PushMissions(new[] { MissionsType.TriLieu, MissionsType.BanRac, MissionsType.CatVang }));
                        return;
                    }
                }
                return;
            }
            if (Objects.Monters.Count > 0)
            {
                Party.ForEach(p => { if (p.IsAuto) p.DownRide(); });
                swNotHave = null;
                return;
            }
            if (GetDistance(63, 56) < 10)
            {
            }
            else
            {
                if (TrueClearMonterTime.Elapsed.TotalSeconds < 20)
                    return;
            }
            if (!IsObjectDead("Lý Thu Thủy"))
            {
                Party.ForEach(p => p.RunTo("[Lang Hoàn Phúc Địa-Lý Thu Thủy][141,63-677]"));
                if (RunTo("[Lang Hoàn Phúc Địa-Lý Thu Thủy][141,63-677]"))
                {
                    if (!Objects.Have("Lý Thu Thủy") && TrueStandTime.Elapsed.TotalSeconds > 15)
                    {
                        if (!DeadObjects.ContainsKey("Lý Thu Thủy"))
                            DeadObjects["Lý Thu Thủy"] = Stopwatch.StartNew();
                    }
                    TalkEx("Lý Thu Thủy");
                    Thread.Sleep(1000);
                    QuestFrame.ClickAll();
                }
            }
            else
            {
                if (!IsObjectDead("Thiên Sơn Đồng Lão"))
                {
                    Party.ForEach(p => p.RunTo("[Lang Hoàn Phúc Địa-Thiên Sơn Đồng Lão][213,120-677]"));
                    if (RunTo("[Lang Hoàn Phúc Địa-Thiên Sơn Đồng Lão][213,120-677]"))
                    {
                        if (!Objects.Have("Thiên Sơn Đồng Lão") && TrueStandTime.Elapsed.TotalSeconds > 15)
                        {
                            if (!DeadObjects.ContainsKey("Thiên Sơn Đồng Lão"))
                                DeadObjects["Thiên Sơn Đồng Lão"] = Stopwatch.StartNew();
                        }
                        TalkEx("Thiên Sơn Đồng Lão");
                        Thread.Sleep(1000);
                        QuestFrame.ClickAll();
                    }
                }
                else
                {
                    if (!IsObjectDead("Vô Nhai Tử"))
                    {
                        Party.ForEach(p => p.RunTo("[Lang Hoàn Phúc Địa-Vô Nhai Tử][183,207-677]"));
                        if (RunTo("[Lang Hoàn Phúc Địa-Vô Nhai Tử][183,207-677]"))
                        {
                            if (!Objects.Have("Vô Nhai Tử") && TrueStandTime.Elapsed.TotalSeconds > 15)
                            {
                                if (!DeadObjects.ContainsKey("Vô Nhai Tử"))
                                    DeadObjects["Vô Nhai Tử"] = Stopwatch.StartNew();
                            }
                            TalkEx("Vô Nhai Tử");
                            Thread.Sleep(1000);
                            QuestFrame.ClickAll();
                        }
                    }
                    else
                    {
                        if (!IsObjectDead("Hư Trúc"))
                        {
                            Party.ForEach(p => p.RunTo("[Lang Hoàn Phúc Địa-Hư Trúc][83,181-677]"));
                            if (RunTo("[Lang Hoàn Phúc Địa-Hư Trúc][83,181-677]"))
                            {
                                TalkEx("Hư Trúc");
                                Thread.Sleep(1000);
                                QuestFrame.ClickAll();
                            }
                        }
                    }
                }
            }
        }

        public void DatDoiAcTac()
        {
            if (MapAcTac == 0)
            {
                RandomMapAcTac();
                return;
            }
            TrieuTap();
            if (TLBB.MapId == MAP.TacKhauDoanhDia)
            {
                if (Objects.NearMonter(18).Count > 0)
                {
                    ClearTime = Stopwatch.StartNew();
                    DownRidePartyEx(true);
                }
                if (!IsBossDie)
                {
                    foreach (GameObject _object in Objects.All)
                    {
                        if (!string.IsNullOrEmpty(_object.Title) && _object.HP == 0)
                        {
                            IsBossDie = true;
                            BossDieTime = Stopwatch.StartNew();
                        }
                    }
                }
                if (IsBossDie)
                {
                    if (ClearTime.Elapsed.TotalSeconds > 40)
                    {
                        IsBossDie = false;
                        ClearTime = Stopwatch.StartNew();
                    }
                    if (BossDieTime.Elapsed.TotalSeconds > 12)
                    {
                        if (!TLBB.IsRide && TLBB.HaveRide)
                        {
                            UpRide();
                            return;
                        }
                        AskTeamFollow();
                    }
                    return;
                }
                if (swStandTime.Elapsed.TotalSeconds >= 1)
                {
                    if (ClearTime.Elapsed.TotalSeconds > 2)
                    {
                        if (!TLBB.IsRide && TLBB.HaveRide)
                        {
                            UpRide();
                            return;
                        }
                        if (!IsMoveEx)
                        {
                            FixKetMap();
                            return;
                        }
                        AskTeamFollow();
                        MoveNext();
                    }
                    else
                    {
                        if (TLBB.IsRide && IsAuto)
                            DownRide();
                        StopFollow();
                        DownRideParty();
                    }
                }
            }
            else
            {
                if (TLBB.MapId != MapAcTac)
                {
                    GoToEx(100, 100, MapAcTac);
                    return;
                }
                else
                {
                    if (IsByLogin)
                        if (QuestFrame.Text.Contains("#{FZXX_170414_2"))
                        {
                            Party.ForEach(g => g.IsXongBHD = true);
                        }
                    if (!TalkNPCPhuBan())
                    {
                        if (!TLBB.IsRide && TLBB.HaveRide)
                        {
                            UpRide();
                            return;
                        }
                        AskTeamFollow();
                        MoveNextEx();
                    }
                    return;
                }
            }
        }

        public void DatDoiAcBa()
        {
            if (AcBa != TLBB.Menpai && AcBa != -1)
            {
                foreach (var game in Party)
                {
                    if (game.TLBB.Menpai == AcBa)
                    {
                        game.PushMissions(MissionsType.DatDoiAcBa);
                        game.AcBa = AcBa;
                        AppointLeader(game.TLBB.Name);
                        PushDebugMessage("Chuyển Key Ác Bá");
                        return;
                    }
                }
                PushDebugMessage("Không đúng Phái Ác Bá, Dừng Auto");
                RemoveMission(MissionsType.DatDoiAcBa);
                return;
            }
            if (PickItem())
                return;
            TrieuTap();
            if (IsMapAcBa)
            {
                if (Objects.NearMonter(18).Count > 0)
                {
                    ClearTime = Stopwatch.StartNew();
                    DownRidePartyEx(true);
                }
                foreach (GameObject _object in Objects.All)
                {
                    if ((_object.CleanName == "acba" || _object.CleanName == "tyrant") && _object.HP == 0 && !IsBossDie)
                    {
                        IsBossDie = true;
                        BossDieTime = Stopwatch.StartNew();
                    }
                }
                if (IsBossDie)
                {
                    if (ClearTime.Elapsed.TotalSeconds > 40)
                    {
                        IsBossDie = false;
                        ClearTime = Stopwatch.StartNew();
                    }
                }
                if (IsBossDie)
                {
                    if (BossDieTime.Elapsed.TotalSeconds > 15)
                    {
                        if (!TLBB.IsRide && TLBB.HaveRide)
                        {
                            UpRide();
                            return;
                        }
                        AskTeamFollow();
                    }
                    return;
                }
                if (swStandTime.Elapsed.TotalSeconds >= 1)
                {
                    if (ClearTime.Elapsed.TotalSeconds > 2)
                    {
                        if (!TLBB.IsRide && TLBB.HaveRide)
                        {
                            UpRide();
                            return;
                        }
                        if (!IsMoveEx)
                        {
                            FixKetMap();
                            return;
                        }
                        AskTeamFollow();
                        MoveNext();
                    }
                    else
                    {
                        if (TLBB.IsRide && IsAuto)
                            DownRide();
                        StopFollow();
                        DownRideParty();
                    }
                }
            }
            else
            {
                if (TLBB.MapId == TLBB.MapMonPhai)
                {
                    if (!TalkNPCPhuBan())
                    {
                        if (!TLBB.IsRide && TLBB.HaveRide)
                        {
                            UpRide();
                            return;
                        }
                        AskTeamFollow();
                        MoveNextEx();
                    }
                }
                else
                {
                    GoToEx(100, 100, TLBB.MapMonPhai);
                }
            }
        }

        public void DatDoiTangKinhCac()
        {
            if (MapTangKinhCac == 0)
            {
                RandomMapTangKinhCac();
                return;
            }

            if (TLBB.MapId != MAP.TangKinhCac)
            {
                if (!TLBB.IsRide && TLBB.HaveRide)
                {
                    UpRide();
                    return;
                }
            }
            TrieuTap();

            if (TLBB.MapId != MapTangKinhCac && TLBB.MapId != MAP.TangKinhCac)
            {
                GoToEx(100, 100, MapTangKinhCac);
            }
            else
            {
                if (TLBB.MapId == MapTangKinhCac)
                {
                    if (!TalkNPCPhuBan())
                    {
                        if (!TLBB.IsRide && TLBB.HaveRide)
                        {
                            UpRide();
                            return;
                        }
                        AskTeamFollow();
                        MoveNextEx();
                    }
                }
                if (TLBB.MapId == MAP.TangKinhCac)
                {
                    if (TLBB.PlayerState == 7 && TLBB.IsRide)
                        DownRide();
                    if (Objects.Monters.Count > 0)
                        Talked = true;
                    if (!Talked)
                    {
                        GoToEx(64, 104);
                        return;
                    }

                    if (Objects.NearMonter(20).Count == 0)
                    {
                        if (!TLBB.IsRide && TLBB.HaveRide)
                        {
                            UpRide();
                            return;
                        }
                        if (GetDistance(64, 28) < 8 && !TKCompleted)
                        {
                        }
                        else
                        {
                            AskTeamFollow();
                        }
                    }
                    else
                    {
                        if (TrueNotMoveTime.Elapsed.TotalSeconds > 1)
                        {
                            DownRidePartyEx();
                            StopFollow();
                        }
                        return;
                    }

                    if (TKCComplete && !CheckTKCComplete && TKCStateTime.Elapsed.TotalSeconds > 20)
                    {
                        if (GoToEx(64, 100))
                        {
                            if (TrueNotMoveTime.Elapsed.TotalSeconds > 5 && TrueClearMonterTime.Elapsed.TotalSeconds > 5)
                            {
                                if (!TLBB.IsRide && TLBB.HaveRide)
                                {
                                    UpRide();
                                    return;
                                }
                                AskTeamFollow();
                                CheckTKCComplete = true;
                            }
                        }
                        return;
                    }
                    if (CheckTKCComplete && !TKCompleted)
                    {
                        if (GoToEx(64, 28))
                        {
                            Party.ForEach(p => { if (p.ForcePickItem()) { p.StopFollow(); } });
                            if (TrueNotMoveTime.Elapsed.TotalSeconds > 5 && TrueClearMonterTime.Elapsed.TotalSeconds > 15)
                            {
                                TKCompleted = true;
                            }
                        }
                        return;
                    }
                    if (TKCompleted || IsObjectDead("Mông Diện Ác Tăng"))
                    {
                        if (!TLBB.IsRide && TLBB.HaveRide)
                        {
                            UpRide();
                            return;
                        }
                        AskTeamFollow();
                        GoToEx(70, 20);
                        return;
                    }

                    if (TKCStateTime.Elapsed.TotalSeconds < 20)
                    {
                        if (!TLBB.IsRide && TLBB.HaveRide)
                        {
                            UpRide();
                            return;
                        }
                        bool istrai = IsTraiTKC;
                        if (TKCInfo.Contains("CJG_090605_4"))
                        {
                            istrai = !IsTraiTKC;
                        }
                        if (!istrai)
                        {
                            GoToEx(96, 69);
                        }
                        else
                        {
                            GoToEx(29, 69);
                        }
                    }
                    else
                    {
                        GoToEx(64, 100);
                    }
                }
            }
        }

        public void DatDoiDaTru()
        {
            if (IsBossDie && BossDieTime.Elapsed.TotalSeconds > 30)
                return;
            if (PickItem())
                return;
            TrieuTap();
            if (TLBB.MapId != MAP.ThanhThuSonPhuBan)
            {
                if (!TLBB.IsRide && TLBB.HaveRide)
                {
                    UpRide();
                    return;
                }
                if (TLBB.MapId == MAP.ThanhThuSon)
                {
                    bool isTalk = TalkNPCPhuBan();
                    if (!isTalk)
                    {
                        if (!IsMoveEx)
                        {
                            FixKetMap();
                            return;
                        }
                        AskTeamFollow();
                        MoveNext();
                    }
                    return;
                }
                else
                {
                    StopFollow();
                    GoToEx(220, 220, MAP.ThanhThuSon);
                }
            }
            else
            {
                if (TDT.GetDistance(90, 65, RoundX, RoundY) > 5)
                {
                    Move(90, 65);
                }
            }
        }

        public void DatDoiMongHeo()
        {
            if (TLBB.MapId != MAP.NongTruongDaTru)
            {
                if (ChangeKey.Elapsed.TotalSeconds > 15)
                {
                    if (!HaveMongHeo)
                    {
                        if (ChangeKey.Elapsed.TotalSeconds > 15)
                        {
                            foreach (Game game in Party)
                            {
                                if (game.HaveMongHeo)
                                {
                                    ChangeKey = Stopwatch.StartNew();
                                    game.PushMissions(MissionsType.DatDoiMongHeo);
                                    RemoveMission(MissionsType.DatDoiMongHeo);
                                    AppointLeader(game.TLBB.Name);
                                }
                            }
                        }
                        return;
                    }
                }
                if (FullPartyGoToEx(THANHTHUSON.VanPhieuPhieu))
                {
                    if (!TLBB.IsQuestOpen)
                    {
                        TalkEx("vanphieuphieu");
                    }
                    else
                    {
                        ChangeKey = Stopwatch.StartNew();
                        QuestFrame.Click(1);
                        QuestFrame.Close();
                    }
                }
            }
            else
            {
                TrieuTapEx();
                if (Objects.NearMonter(18).Count > 0)
                    ClearTime = Stopwatch.StartNew();
                if (!IsBossDie)
                {
                    foreach (GameObject _object in Objects.All)
                    {
                        if (_object.CleanName.Contains("datruvuong") && _object.HP == 0)
                        {
                            IsBossDie = true;
                            BossDieTime = Stopwatch.StartNew();
                        }
                    }
                }
                if (IsBossDie)
                {
                    if (BossDieTime.Elapsed.TotalSeconds > 12)
                    {
                        if (!TLBB.IsRide && TLBB.HaveRide)
                        {
                            UpRide();
                            return;
                        }
                        AskTeamFollow();
                    }
                    if (BossDieTime.Elapsed.TotalSeconds > 20)
                    {
                        GoToEx(THANHTHUSON.VanPhieuPhieu);
                    }
                    return;
                }
                if (ClearTime.Elapsed.TotalSeconds > 2)
                {
                    if (!TLBB.IsRide && TLBB.HaveRide)
                    {
                        UpRide();
                        return;
                    }
                    if (!IsMoveEx)
                    {
                        FixKetMap();
                        return;
                    }
                    AskTeamFollow();
                    MoveNext();
                }
                if (ClearTime.Elapsed.TotalSeconds < 1)
                {
                    if (swStandTime.Elapsed.TotalSeconds >= 1)
                    {
                        if (TLBB.IsRide && IsAuto)
                            DownRide();
                        StopFollow();
                    }
                    DownRideParty();                  
                }
            }
        }

        public void DatDoiLauLanTamBao()
        {
            if (IsXongTamBao)
            {
                RemoveMission(MissionsType.DatDoiLauLanTamBao);
                if (IsByLogin && !Missions.Contains(MissionsType.DatDoiKyCuoc))
                    Party.ForEach(g => g.IsXongBHD = true);
                return;
            }
            if (TLBB.MapId != MAP.LauLanBaoTang)
            {
                if (FullPartyGoToEx(LAULAN.KimCuuLinh))
                {
                    // 808039 1
                    // 808039 11
                    if (TLBB.IsQuestOpen)
                    {
                        if (QuestFrame.Click(808039, 1))
                            return;
                        if (Missions.Contains(MissionsType.LauLanTamBaoNhanh) || Setting.Is("checkDiCoNhanh"))
                        {
                            QuestFrame.Click(808039, 12);
                            Thread.Sleep(1000);
                            if (QuestFrame.Text.Contains("#{FBJR_091019_13}"))
                            {
                                RemoveMission(MissionsType.DatDoiLauLanTamBao);
                                if (IsByLogin && !Missions.Contains(MissionsType.DatDoiKyCuoc))
                                    Party.ForEach(g => g.IsXongBHD = true);
                            }
                        }
                        else
                        {
                            QuestFrame.Click(808039, 11);
                            Thread.Sleep(1000);
                            if (QuestFrame.Text.Contains("#{FBJR_091019_13}"))
                            {
                                RemoveMission(MissionsType.DatDoiLauLanTamBao);
                                if (IsByLogin && !Missions.Contains(MissionsType.DatDoiKyCuoc))
                                    Party.ForEach(g => g.IsXongBHD = true);
                            }
                        }
                        QuestFrame.Click(808039, 1);
                        QuestFrame.Close();
                    }
                    else
                    {
                        Talk(LAULAN.KimCuuLinh.Id);
                    }
                }
            }
            else
            {
                TrieuTap();
                StopFollow();
            }
        }

        public void DatDoiThienGiangKyThu()
        {
            if (PickItem())
                return;
            if (!TLBB.IsRide && TLBB.HaveRide && !Objects.Have("phithienmieu") && !Objects.Have("thanthu"))
            {
                UpRide();
                return;
            }
            TrieuTapEx();
            if (IsBossDie)
                return;
            if (TLBB.MapId != MAP.HuyenVuDaoPhuBan)
            {
                if (TLBB.MapId != MAP.LauLan)
                {
                    AskTeamFollow();
                    GoToEx(250, 45, LAULAN.Id);
                }
                else
                {
                    if (!TalkNPCPhuBan())
                    {                        
                        MoveNextEx();
                    }
                    return;
                }
            }
            else
            {
                GoToEx(119, 126);
                if (IdleTime > 2)
                {
                    foreach (GameObject _object in Objects.Monters)
                    {
                        if (_object.CleanName.Contains("phithienmieu") || _object.CleanName.Contains("thanthu"))
                        {
                            StopFollow();
                            if (IsAuto)
                                DownRide();
                        }
                    }
                }
            }
        }

        public void DatDoiPhungHoangLangMo()
        {
            if (!TLBB.IsRide && TLBB.HaveRide && TLBB.MapId != MAP.PhungHoangCoThanhPhuBan)
            {
                UpRide();
                return;
            }
            TrieuTap();
            if (!IsMapPhuBan())
            {
                if (TLBB.MapId != MAP.PhungHoangCoThanh)
                {
                    GoToEx(100, 100, MAP.PhungHoangCoThanh);
                    return;
                }
                else
                {
                    if (!TalkNPCPhuBan())
                    {
                        if (!IsMoveEx)
                        {
                            FixKetMap();
                            return;
                        }
                        MoveNext();
                    }
                    return;
                }
            }
            else
            {
                if (Objects.NearMonter(20).Count > 0)
                    ClearTime = Stopwatch.StartNew();
            }
            if (IsMapPhuBan())
            {
                if (ClearTime.Elapsed.TotalSeconds > 80)
                {
                    IsBossDie = false;
                    MapATIndex = -1;
                    MoveIndex = 0;
                    ClearTime = Stopwatch.StartNew();
                }
                if (!IsBossDie)
                {
                    if (TLBB.MapId == MAP.PhungHoangCoThanhPhuBan)
                    {
                        foreach (GameObject _object in Objects.All)
                        {
                            if (_object.CleanName.Contains("banson") && _object.HP == 0)
                            {
                                IsBossDie = true;
                                BossDieTime = Stopwatch.StartNew();
                            }
                        }
                    }
                }
                else
                {
                    MoveIndex = 0;
                    if (BossDieTime.Elapsed.TotalSeconds > 15)
                    {
                        if (!TLBB.IsRide && TLBB.HaveRide)
                        {
                            UpRide();
                            return;
                        }
                        AskTeamFollow();
                    }
                    return;
                }

                if (swStandTime.Elapsed.TotalSeconds >= 1)
                {
                    if (ClearTime.Elapsed.TotalSeconds > 2)
                    {
                        if (MoveIndex != (MapPoint.Split('-').Length - 1) || ClearTime.Elapsed.TotalSeconds > 15)
                        {
                            if (!TLBB.IsRide && TLBB.HaveRide)
                            {
                                UpRide();
                                return;
                            }
                            AskTeamFollow();
                            MoveNext();
                        }
                    }
                    else
                    {
                        if (TLBB.IsRide && IsAuto)
                            DownRide();
                        StopFollow();
                    }
                }
            }
            
        }

        public void DatDoiMaTac()
        {
            if (!Global.IsMaTac)
            {
                RemoveMission(MissionsType.DatDoiMaTac);
                return;
            }
            if (SwTimeOnMap.Elapsed.TotalMinutes >= Global.MinMaTac)
            {
                foreach (KeyValuePair<int, Stopwatch> kvp in dicmatac.Where(kvp => kvp.Key > 0).ToList().OrderBy(kvp => kvp.Value.Elapsed.TotalSeconds))
                {
                    CurMapMaTac = kvp.Key;
                    break;
                }
            }
            if (CurMapMaTac != -1)
            {
                if (TLBB.MapId != CurMapMaTac)
                {
                    GoToEx(100, 100, CurMapMaTac);
                    TrieuTap();
                    return;
                }
            }
            foreach (Game g in Party)
            {
                if (g.ForcePickItem())
                {
                    StopFollow();
                    return;
                }
            }
            TrieuTap();
            foreach (GameObject obj in Objects.All)
            {
                if (obj.Name == "Đoạt Bảo Mã Tặc" && obj.HP > 0)
                {
                    if (!obj.IsMonter)
                    {
                        if (TrueClearMonterTime.Elapsed.TotalSeconds > 2)
                            Talk(obj);
                    }
                    else
                    {
                        if (TrieuTapEx())
                        {
                            DownRideParty();
                            StopFollow();
                        }
                    }
                    return;
                }
            }
            if (!TLBB.IsRide)
            {
                StopFollow();
                UpRide();
                return;
            }
            if (TrueClearMonterTime.Elapsed.TotalSeconds >= 15 && TrueStandTime.Elapsed.TotalSeconds >= 2)
            {
                if (!IsMoveEx)
                {
                    FixKetMap();
                }
                else
                {
                    // AskTeamFollow();
                    if (SettingOld.LoadMAP(TLBB.MapId.ToString()).Split('-').Length <= 1)
                    {
                        int x = TDT.Random.Next(RoundX - 40, RoundX + 41);
                        int y = 0;
                        int ran = TDT.Random.Next(1, 3);
                        if (ran == 1)
                            y = (int)Math.Sqrt(Math.Pow(40, 2) - Math.Pow((x - RoundX), 2)) + RoundY;
                        if (ran == 2)
                            y = 0 - (int)Math.Sqrt(Math.Pow(40, 2) - Math.Pow((x - RoundX), 2)) + RoundY;
                        GoToEx(x, y);
                    }
                    else
                    {
                        MoveNext();
                    }
                }
            }
        }

        public void DatDoiBaoDoHiem()
        {
            if (!Global.IsBaoDoHiem)
            {
                RemoveMission(MissionsType.DatDoiBaoDoHiem);
                return;
            }
            if (Objects.Monters.Count > 0)
            {
                NPC npc = new NPC();
                npc.X = RoundX;
                npc.Y = RoundY;
                npc.Map = (int)TLBB.MapId;
                if (FullPartyGoToEx(npc))
                {
                    DownRideParty();
                    StopFollow();
                }
                return;
            }
            foreach (Game g in Party)
            {
                if (g.ForcePickItem())
                {
                    StopFollow();
                    return;
                }
            }
            if (ListPointBaoDoHiem.Count > 0)
            {
                if (TrueClearMonterTime.Elapsed.TotalSeconds >= 15)
                {
                    ListPointBaoDoHiem = ListPointBaoDoHiem.OrderBy(l => GetDistance(l)).ToList();
                    bool isrideall = true;
                    foreach (Game game in Party)
                    {
                        if (!game.TLBB.IsRide && game.TLBB.HaveRide)
                        {
                            game.StopFollow();
                            game.UpRide();
                            isrideall = false;
                        }
                    }
                    if (!isrideall)
                        return;
                    NPC npc = new NPC();
                    npc.X = TDT.ParseAllInt(ListPointBaoDoHiem[0].Split(',')[0]);
                    npc.Y = TDT.ParseAllInt(ListPointBaoDoHiem[0].Split(',')[1]);
                    npc.Map = (int)TLBB.MapId;
                    if (FullPartyGoToEx(npc))
                    {
                        if (TrueClearMonterTime.Elapsed.TotalSeconds > 3)
                        {
                            ListPointBaoDoHiem.Remove(npc.X + "," + npc.Y);
                            return;
                        }
                    }
                }
                return;
            }
            if (MissionState == "")
            {
                MissionX = MissionY = MissionMap = -1;
                if (!HaveItem("TaskTools16_2"))
                {
                    foreach (PacketItem item in PacketItems.ThienCo)
                    {
                        if (item.Type == "TaskTools16_2")
                        {
                            GetItemThienCo(item.Index);
                            LUA.PackUp();
                            return;
                        }
                    }
                }
                foreach (var item in PacketItems.All)
                {
                    if (item.Type == "TaskTools16_2")
                    {
                        StopFollow();
                        item.Use();
                        Thread.Sleep(1000);
                        if (!TLBB.IsQuestOpen)
                        {
                            MissionState = "";
                            return;
                        }
                        MissionInfo = QuestFrame.First(this);
                        if (MissionInfo.Split('*').Length != 6)
                        {
                            MissionState = "";
                            return;
                        }
                        MissionX = TDT.ParseAllInt(MissionInfo.Split('*')[3]);
                        MissionY = TDT.ParseAllInt(MissionInfo.Split('*')[4]);
                        MissionMap = TDT.ParseAllInt(MissionInfo.Split('*')[5]);
                        if (MissionX == 0 || MissionY == 0 || MissionMap == 0)
                        {
                            MissionState = "";
                        }
                        else
                        {
                            BTDIndex = item.Index;
                            MissionState = "Do";
                        }
                        QuestFrame.Close();
                        return;
                    }
                }
                RemoveMission(MissionsType.DatDoiBaoDoHiem);
                PushDebugMessage("Không có bảo đồ hiếm. Dừng Auto");
                foreach (Game game in Party)
                {
                    if (game.HaveItem("baodohiem"))
                    {
                        AppointLeader(game.TLBB.Name);
                        game.PushMissions(MissionsType.DatDoiBaoDoHiem);
                        return;
                    }
                    foreach (PacketItem p in game.PacketItems.ThienCo)
                    {
                        if (p.ClearName == "baodohiem")
                        {
                            AppointLeader(game.TLBB.Name);
                            game.PushMissions(MissionsType.DatDoiBaoDoHiem);
                            return;
                        }
                    }
                }
            }
            if (MissionState == "Do")
            {
                bool isrideall = true;
                foreach (Game game in Party)
                {
                    if (!game.TLBB.IsRide && game.TLBB.HaveRide)
                    {
                        game.StopFollow();
                        game.UpRide();
                        isrideall = false;
                    }
                }
                if (!isrideall)
                    return;
                NPC npc = new NPC();
                npc.X = MissionX;
                npc.Y = MissionY;
                npc.Map = MissionMap;
                if (FullPartyGoToEx(npc))
                {
                    foreach (var item in PacketItems.All)
                    {
                        if (item.Index == BTDIndex)
                        {
                            if (item.Type == "TaskTools16_2")
                            {
                                StopFollow();
                                item.Use();
                            }
                        }
                    }
                    MissionState = "";
                }
                return;
            }
            MissionState = "";
        }

        public void DatDoiQuanSonHai()
        {
            foreach (GameObject o in Objects.All.Where(o => o.HP > 0).OrderBy(o => o.Distance))
            {
                if (o.Title.Contains("#{PTFB"))
                {
                    if (o.State != 0)
                    {
                        quansonhaitime = Stopwatch.StartNew();
                        if (!isdakhieuchien)
                        {
                            isdakhieuchien = true;
                            countsovong++;
                        }
                        Party.ForEach(g => { if (g.IsAuto) g.DownRide(); });
                    }
                    else
                    {
                        if (o.CleanName == "conroily" || o.CleanName == "trieuphucvu" || o.Name == "Tô Diên Thanh")
                        {
                            GoToEx(DAILY.ThamHanChau);
                            return;
                        }
                        if (!isdakhieuchien)
                        {
                            if (quansonhaitime.Elapsed.TotalSeconds >= 3)
                            {
                                if (GoToEx(o))
                                {
                                    Talk(o.Id);
                                    Thread.Sleep(500);
                                    QuestFrame.Click(1);
                                    Thread.Sleep(500);
                                    DoStringEx("setmetatable(_G, {__index = MessageBox_Self2_Env}); MessageBox_Self2_Ok_Clicked(); ");
                                    return;
                                }
                            }
                        }
                        else
                        {
                            if (quansonhaitime.Elapsed.TotalSeconds >= 3)
                                FullPartyGoToEx(DAILY.ThamHanChau);
                        }
                    }
                    return;
                }
            }
            if (FullPartyGoToEx(DAILY.ThamHanChau))
            {
                if (countsovong >= 3)
                {
                    Party.ForEach(game => game.PushMissions(MissionsType.NhanThuongQuanSonHai));
                    RemoveMission(MissionsType.DatDoiQuanSonHai);
                    return;
                }
                Talk(DAILY.ThamHanChau);
                Thread.Sleep(500);
                QuestFrame.Click("#{PTFB_191228_02}");
                Thread.Sleep(500);
                DoStringEx("setmetatable(_G, {__index = WuLin_Peak_Env}); WuLin_Peak_EnterScene();");
                Thread.Sleep(500);
                DoStringEx(@"Clear_XSCRIPT();
        Set_XSCRIPT_Function_Name('LeaderChooseConfirmCheck')

        Set_XSCRIPT_ScriptID(893398)

        Set_XSCRIPT_Parameter(0, " + Main.TangQuanSonHai + @")

        Set_XSCRIPT_Parameter(1, 232)

        Set_XSCRIPT_ParamCount(2)

    Send_XSCRIPT()");
                Thread.Sleep(500);
                Party.ForEach(game => game.DoStringEx("setmetatable(_G, {__index = WuLin_Env}); WuLin_Member_OnChooseStageConfirm(); "));
                Thread.Sleep(2000);
            }
        }

        public void DatDoiBossMap()
        {
            if (CurBossMap != -1 && !IsXongBossMap)
            {
                if (TLBB.MapId != CurBossMap)
                {
                    GoToEx(100, 100, CurBossMap);
                    TrieuTap();
                    return;
                }
            }
            foreach (Game g in Party)
            {
                if (g.ForcePickItem())
                {
                    StopFollow();
                    return;
                }
            }
            TrieuTap();

            foreach (GameObject obj in Objects.All)
            {
                if (SettingOld.HashOnlyBossMap.Contains(obj.Name) && obj.HP > 0)
                {
                    DownRideParty();
                    StopFollow();
                    return;
                }
            }
            if (!TLBB.IsRide)
            {
                StopFollow();
                UpRide();
                return;
            }
            if (ClearMonterTimeEx.Elapsed.TotalSeconds <= 15)
                return;
            //AskTeamFollow();
            if (IsXongBossMap)
            {
                Party.ForEach(p => { p.RunTo("[Lạc Dương-Long Bá Thiên][185,336-0]"); });
                if (RunTo("[Lạc Dương-Long Bá Thiên][185,336-0]"))
                {
                    IsXongBossMap = false;
                    RemoveMission(MissionsType.DatDoiBossMap);
                    CurBossMap = -1;
                }
                return;
            }
            MoveNextBossMap();
        }

        public void DatDoiTyVo()
        {
            if (TLBB.MapId == MAP.PhongChuanBiCapHoKhieu || TLBB.MapId == MAP.PhongChuanBiCapLongDang || TLBB.MapId == MAP.PhongChuanBiCapUngDuong || TLBB.MapId == MAP.ChienTruongThiDau)
                Party.ForEach(g => g.RemoveMission(MissionsType.DatDoiTyVo));
            if (TLBB.Lvl >= 80 && TLBB.Lvl <= 89)
            {
                if (FullPartyGoToEx(DAILY.ChuThucBan))
                {
                    Talk(DAILY.ChuThucBan);
                    Thread.Sleep(1000);
                    foreach (QuestFrameItem i in QuestFrame.Items)
                    {
                        if (i.Name.Contains("#{BWDH_180804_11"))
                        {
                            QuestFrame.Click((int)i.StrOptionExtra1, (int)i.StrOptionExtra2);
                        }
                    }
                }
            }
            if (TLBB.Lvl >= 90 && TLBB.Lvl <= 109)
            {
                if (FullPartyGoToEx(TOCHAU.ChuTrongThanh))
                {
                    Talk(TOCHAU.ChuTrongThanh);
                    Thread.Sleep(1000);
                    foreach (QuestFrameItem i in QuestFrame.Items)
                    {
                        if (i.Name.Contains("#{BWDH_180804_11"))
                        {
                            QuestFrame.Click((int)i.StrOptionExtra1, (int)i.StrOptionExtra2);
                        }
                    }
                }
            }
            if (TLBB.Lvl >= 110)
            {
                if (FullPartyGoToEx(LACDUONG.ChuBaHanh))
                {
                    Talk(LACDUONG.ChuBaHanh);
                    Thread.Sleep(1000);
                    foreach (QuestFrameItem i in QuestFrame.Items)
                    {
                        if (i.Name.Contains("#{BWDH_180804_11"))
                        {
                            QuestFrame.Click((int)i.StrOptionExtra1, (int)i.StrOptionExtra2);
                        }
                    }
                }
            }
        }

        public void DatDoiTucCau()
        {
            if (TLBB.MapId != MAP.MauDonUyen)
            {
                if (FullPartyGoToEx(TOCHAU.VuThienThien))
                {
                    Talk(TOCHAU.VuThienThien);
                    Thread.Sleep(1000);
                    QuestFrame.Click("Hoành Tảo Mẫu Đơn Uyển");
                    Thread.Sleep(1000);
                    if (QuestFrame.Text.Contains("#{FBJR_091019_13}"))
                        RemoveMission(MissionsType.DatDoiTucCau);
                }
            }
            else
            {
                TrieuTap();
                DownRidePartyEx();
            }
        }

        public void DatDoiSatTinh()
        {
            if (IsHoiSinh)
                return;
            if (TLBB.MapId != MAP.LoiDaiSinhTu || ClearSatTinhCount >= 1)
            {
                Party.ForEach(p =>
                {
                    if (p.GoToEx(DAILY.KhoVinhDaiSu))
                        p.IsP = true;
                });
            }
            else
            {
                if (!Objects.Monters.Any(o => o.Name == "Lý Khôi"))
                    TrieuTapEx();
                if (TrueClearMonterTime.Elapsed.TotalSeconds > 10)
                {
                    GameObject bestBoss = Objects.All.Where(o => o.Menpai == 10 && o.X > 34).OrderBy(o => o.Y).FirstOrDefault();
                    if (bestBoss != null)
                    {
                        if (GoToEx(bestBoss.X, bestBoss.Y))
                        {
                            Talk(bestBoss.Id);
                            Thread.Sleep(1000);
                            QuestFrame.ClickAll();
                            Thread.Sleep(1000);
                            GoToEx(34, 33);
                            Thread.Sleep(3000);
                        }
                    }
                    else
                    {
                        if (GetDistance(34, 33) >= 2)
                        {
                            Move(34, 33);
                        }
                        else
                        {
                            if (TrueClearMonterTime.Elapsed.TotalSeconds > 15 && TrueStandTime.Elapsed.TotalSeconds > 15 && SwTimeOnMap.Elapsed.TotalSeconds > 30)
                            {
                                ClearSatTinhCount++;
                            }
                        }
                    }
                }
                else
                {
                    StopFollow();
                    DownRideParty();
                    if (TrueClearMonterTime.Elapsed.TotalSeconds > 5)
                    {
                        PushDebugMessage("Tự động khiêu chiến sau " + (10 - (int)swClearMontersTime.Elapsed.TotalSeconds) + "s");
                    }
                }
            }
        }

        public void DatDoiThieuThatSon()
        {
            if (IsHoiSinh)
                return;

            if (TLBB.MapId != MAP.ThieuThatSon || (IsObjectDead("Đinh Xuân Thu") && Global.IsBoBoss))
            {
                if (FullPartyGoToEx(DAILY.ChuDanThan))
                {
                    Talk(DAILY.ChuDanThan);
                    Thread.Sleep(1000);
                    QuestFrame.Click("#{CJG_101231_70}");
                    Thread.Sleep(1000);
                    if (QuestFrame.Text.Contains("#{CJG_101231_75"))
                    {
                        RemoveMission(MissionsType.DatDoiThieuThatSon);
                        if (IsByLogin)
                        {
                            Party.ForEach(g => g.IsXongBHD = true);
                        }
                        if (Main.IsBanRac)
                            Party.ForEach(game => game.PushMissions(new[] { MissionsType.TriLieu, MissionsType.BanRac, MissionsType.CatVang }));
                    }
                }
            }
            else
            {
                if (PartyMinSwBuffNM <= 3)
                    return;

                if (RoundX < 95)
                {
                    if (!DeadObjects.ContainsKey("Cưu Ma Trí"))
                        DeadObjects["Cưu Ma Trí"] = Stopwatch.StartNew();
                }
                if (RoundY < 95)
                {
                    if (!DeadObjects.ContainsKey("Trang Tụ Hiền"))
                        DeadObjects["Trang Tụ Hiền"] = Stopwatch.StartNew();
                }
                if (RoundY < 65)
                {
                    if (!DeadObjects.ContainsKey("Đinh Xuân Thu"))
                        DeadObjects["Đinh Xuân Thu"] = Stopwatch.StartNew();
                }
                if (Objects.Have("Đinh Xuân Thu"))
                {
                    if (!DeadObjects.ContainsKey("Mộ Dung Phục"))
                        DeadObjects["Mộ Dung Phục"] = Stopwatch.StartNew();
                }

                if (IsObjectDead("Đinh Xuân Thu"))
                {
                    GameObject ThieuLamDeTu = Objects.GetObject("Thiếu Lâm Đệ Tử");
                    if (ThieuLamDeTu != null)
                    {
                        FullPartyGoToEx(ThieuLamDeTu.X, ThieuLamDeTu.Y);
                        //TrieuTapEx();
                        Talk(ThieuLamDeTu.Id);
                        QuestFrame.Click("#{SSS_ZJ_120109_06}");
                    }
                    else
                    {
                        if (TrueClearMonterTime.Elapsed.TotalSeconds >= 25)
                        {
                            GameObject TieuPhong = Objects.GetObject("Tiêu Phong");
                            if (TieuPhong != null)
                            {
                                if (GoToEx(122, 35))
                                {
                                    GameObject TaoDiaThanTang = Objects.GetObject("Tảo Địa Thần Tăng");
                                    if (TaoDiaThanTang != null)
                                    {
                                        if (GoToEx(TaoDiaThanTang))
                                        {
                                            Talk(TaoDiaThanTang);
                                            QuestFrame.ClickAll();
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (FullPartyGoToEx(THIEUTHATSON.TaoDiaThanTang))
                                {
                                    Talk("Tảo Địa Thần Tăng");
                                    QuestFrame.ClickAll();
                                }
                            }
                        }
                    }
                }
                else if (IsObjectDead("Mộ Dung Phục") || Objects.Have("Đinh Xuân Thu"))
                {
                    if (FullPartyGoToEx(THIEUTHATSON.DinhXuanThu, 3))
                    {
                        if (!Objects.Have("Đinh Xuân Thu") && TrueStandTime.Elapsed.TotalSeconds > 15)
                        {
                            if (!DeadObjects.ContainsKey("Đinh Xuân Thu"))
                                DeadObjects["Đinh Xuân Thu"] = Stopwatch.StartNew();
                        }
                        if (TrueClearMonterTime.Elapsed.TotalSeconds > 5)
                        {
                            Talk("Đinh Xuân Thu");
                            QuestFrame.ClickAll();
                        }
                    }
                }
                else if (IsObjectDead("Trang Tụ Hiền") || Objects.Have("Mộ Dung Phục"))
                {
                    if (FullPartyGoToEx(THIEUTHATSON.MoDungPhuc, 3))
                    {
                        if (!Objects.Have("Mộ Dung Phục") && TrueStandTime.Elapsed.TotalSeconds > 15)
                        {
                            if (!DeadObjects.ContainsKey("Mộ Dung Phục"))
                                DeadObjects["Mộ Dung Phục"] = Stopwatch.StartNew();
                        }
                        Talk("Mộ Dung Phục");
                        if (TLBB.IsQuestOpen)
                        {
                            QuestFrame.ClickAll();
                        }
                    }
                }
                else if (IsObjectDead("Cưu Ma Trí") || Objects.Have("Trang Tụ Hiền"))
                {
                    if (FullPartyGoToEx(THIEUTHATSON.TrangTuHien, 12))
                    {
                        if (!Objects.Have("Trang Tụ Hiền") && TrueStandTime.Elapsed.TotalSeconds > 15)
                        {
                            if (!DeadObjects.ContainsKey("Trang Tụ Hiền"))
                                DeadObjects["Trang Tụ Hiền"] = Stopwatch.StartNew();
                        }
                        if (TrueClearMonterTime.Elapsed.TotalSeconds > 15)
                        {
                            Talk("Trang Tụ Hiền");
                            QuestFrame.ClickAll();
                        }
                    }
                }
                else
                {
                    if (!IsClearFistRound)
                    {
                        if (FullPartyGoToEx(130, 202))
                        {
                            if (TrueClearMonterTime.Elapsed.TotalSeconds > 5)
                                IsClearFistRound = true;
                        }
                    }
                    else
                    {
                        if (FullPartyGoToEx(THIEUTHATSON.CuuMaTri, 3))
                        {
                            if (!Objects.Have("Cưu Ma Trí") && TrueStandTime.Elapsed.TotalSeconds > 15)
                            {
                                if (!DeadObjects.ContainsKey("Cưu Ma Trí"))
                                    DeadObjects["Cưu Ma Trí"] = Stopwatch.StartNew();
                            }
                            Talk("Cưu Ma Trí");
                            QuestFrame.ClickAll();
                        }
                    }
                }
            }
        }

        public void DatDoiVuongLang()
        {
            if (IsHoiSinh)
                return;

            if (TLBB.MapId != MAP.PhungMinhVuongLang)
            {
                if (FullPartyGoToEx(KimLang.TieuLang))
                {
                    if (TLBB.IsQuestOpen)
                    {
                        QuestFrame.Click("#{FMZLW_111219_03}");
                        QuestFrame.Close();
                        if (QuestFrame.All(this).Contains("#{FMZLW_111219_17}"))
                        {
                            RemoveMission(MissionsType.DatDoiVuongLang);
                            if (Main.IsBanRac)
                                Party.ForEach(game => game.PushMissions(new[] { MissionsType.TriLieu, MissionsType.BanRac, MissionsType.CatVang }));
                        }
                    }
                    else
                    {
                        Talk(KimLang.TieuLang);
                    }
                }
            }
            else
            {
                StopFollow();
                DownRidePartyEx();

                if (NeBayTime.Elapsed.TotalSeconds > 1)
                {
                    if (!Objects.All.Any(o => o.CleanName == "codieu" || o.CleanName == "truonghuu"))
                    {
                        foreach (Game game in Party.Where(g => g.TLBB.IsNoiCong))
                        {
                            if (game.ForcePickItem())
                                continue;
                            if (game.GetDistance(GAMEDIC.PhungMinhVuongLang[game.PartyIndex]) > 2)
                            {
                                game.Move(GAMEDIC.PhungMinhVuongLang[game.PartyIndex]);
                            }
                        }
                    }
                    else
                    {
                        foreach (Game game in Party)
                        {
                            if (game.ForcePickItem())
                                return;
                        }
                        TrieuTap();
                    }
                }

                GameObject obj = Objects.GetObject("Trường Hữu");
                if (swClearMontersTime.Elapsed.TotalSeconds > 5)
                {
                    if (obj != null)
                    {
                        if (TLBB.IsQuestOpen)
                        {
                            QuestFrame.ClickAll();
                            QuestFrame.Close();
                            return;
                        }
                        Talk(obj);
                    }
                }
                obj = Objects.GetObject("Tất Phương");
                if (obj != null)
                {
                    if (TLBB.IsQuestOpen)
                    {
                        QuestFrame.ClickAll();
                        QuestFrame.Close();
                        return;
                    }
                    Talk(obj);
                }
                obj = Objects.GetObject("Cổ Điêu");
                if (obj != null)
                {
                    if (obj.Buff.Count == 0)
                    {
                        if (TLBB.IsQuestOpen)
                        {
                            QuestFrame.ClickAll();
                            QuestFrame.Close();
                            return;
                        }
                        Talk(obj);
                    }
                    else
                    {
                        if (GetDistance(obj.RoundX, obj.RoundY) >= 2)
                        {
                            Move(obj.RoundX, obj.RoundY);
                        }
                    }
                }
            }
        }

        public void DatDoiPhieuMieuPhong()
        {
            if (IsHoiSinh)
                return;
            if (TLBB.MapId != MAP.PhieuMieuPhong || IsObjectDead("Lý Thu Thủy"))
            {
                if (FullPartyGoToEx(NPC.TrinhThanhSuong))
                {
                    Talk(NPC.TrinhThanhSuong.Id);
                    Thread.Sleep(1000);
                    if (TLBB.IsQuestOpen)
                    {
                        if (Missions.Contains(MissionsType.DatDoiPhieuMieuPhong))
                        {
                            foreach (QuestFrame dialog in QuestFrame.Enum(this))
                            {
                                if (dialog.StrOptionExtra1 == 402276)
                                {
                                    QuestFrameOptionClicked(dialog);
                                    Thread.Sleep(1000);
                                }
                                if (QuestFrame.Text.Contains("#{FBJR_091019_11}"))
                                {
                                    RemoveMission(MissionsType.DatDoiPhieuMieuPhong);
                                    if (Main.IsBanRac)
                                        Party.ForEach(game => game.PushMissions(new[] { MissionsType.TriLieu, MissionsType.BanRac, MissionsType.CatVang }));
                                    PushDebugMessage("xong PMP");
                                    QuestFrame.Close();
                                    return;
                                }
                            }
                        }
                        else if (Missions.Contains(MissionsType.DatDoiKhieuChienPhieuMieuPhong))
                        {
                            foreach (QuestFrame dialog in QuestFrame.Enum(this))
                            {
                                if (dialog.StrOptionExtra1 == 402263)
                                {
                                    QuestFrameOptionClicked(dialog);
                                    Thread.Sleep(1000);
                                }
                                if (QuestFrame.Text.Contains("#{FBJR_091019_11}"))
                                {
                                    RemoveMission(MissionsType.DatDoiKhieuChienPhieuMieuPhong);
                                    if (Main.IsBanRac)
                                        Party.ForEach(game => game.PushMissions(new[] { MissionsType.TriLieu, MissionsType.BanRac, MissionsType.CatVang }));
                                    PushDebugMessage("xong Huyết Chiến PMP");
                                    QuestFrame.Close();
                                    return;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                TrieuTapEx();
                if (TrueClearMonterTime.Elapsed.TotalSeconds <= 15 || Objects.Have("Bích Lân Cương Thi"))
                {
                    StopFollow();
                    DownRidePartyEx();
                    return;
                }
                else
                {
                    if (!TLBB.IsRide && TLBB.HaveRide)
                    {
                        UpRide();
                        return;
                    }
                    AskTeamFollow();
                }
                if (PartyMinSwBuffNM < 3)
                    return;
                if (IsObjectDead("Nhậm Bình Sinh"))
                {
                    if (IsTalkOLaoDai)
                    {
                        if (GoToEx(123, 34))
                        {
                            if (!Objects.Have("Lý Thu Thủy") && TrueStandTime.Elapsed.TotalSeconds > 8)
                            {
                                if (!DeadObjects.ContainsKey("Lý Thu Thủy"))
                                    DeadObjects["Lý Thu Thủy"] = Stopwatch.StartNew();
                            }
                        }
                    }
                    else
                    {
                        if (GoToEx(95, 39))
                        {
                            for (int i = 0; i < 10; i++)
                            {
                                Talk("Ô Lão Đại");
                                Thread.Sleep(350);
                                if (TLBB.IsQuestOpen)
                                {
                                    QuestFrame.ClickAll();
                                    IsTalkOLaoDai = true;
                                }
                            }
                        }
                    }
                }
                else if (IsObjectDead("Ô Lão Đại"))
                {
                    if (IsTalkPhuManNghi)
                    {
                        if (GoToEx(123, 34))
                        {
                            if (!Objects.Have("Nhậm Bình Sinh") && TrueStandTime.Elapsed.TotalSeconds > 8)
                            {
                                if (!DeadObjects.ContainsKey("Nhậm Bình Sinh"))
                                    DeadObjects["Nhậm Bình Sinh"] = Stopwatch.StartNew();
                            }
                        }
                    }
                    else
                    {
                        if (GoToEx(NPC.PhuManNghi.X, NPC.PhuManNghi.Y))
                        {
                            for (int i = 0; i < 10; i++)
                            {
                                Talk("Phù Mẫn Nghi");
                                Thread.Sleep(350);
                                if (TLBB.IsQuestOpen)
                                    IsTalkPhuManNghi = true;
                                QuestFrame.ClickAll();
                            }
                        }
                    }
                }
                else if (IsObjectDead("Tang Thổ Công"))
                {
                    if (GoToEx(NPC.OLaoDai.X, NPC.OLaoDai.Y))
                    {
                        if (!Objects.Have("Ô Lão Đại") && TrueStandTime.Elapsed.TotalSeconds > 8)
                        {
                            if (!DeadObjects.ContainsKey("Ô Lão Đại"))
                                DeadObjects["Ô Lão Đại"] = Stopwatch.StartNew();
                        }
                        Talk("Ô Lão Đại");
                        QuestFrame.ClickAll();
                    }
                }
                else if (IsObjectDead("Cáp Đại Bá"))
                {
                    if (GoToEx(NPC.TangThoCong.X, NPC.TangThoCong.Y))
                    {
                        if (!Objects.Have("Tang Thổ Công") && TrueStandTime.Elapsed.TotalSeconds > 8)
                        {
                            if (!DeadObjects.ContainsKey("Tang Thổ Công"))
                                DeadObjects["Tang Thổ Công"] = Stopwatch.StartNew();
                        }
                        Talk("Tang Thổ Công");
                        QuestFrame.ClickAll();
                    }
                }
                else
                {
                    if (GoToEx(NPC.CapDaiBa.X, NPC.CapDaiBa.Y))
                    {
                        if (!Objects.Have("Cáp Đại Bá") && TrueStandTime.Elapsed.TotalSeconds > 8)
                        {
                            if (!DeadObjects.ContainsKey("Cáp Đại Bá"))
                                DeadObjects["Cáp Đại Bá"] = Stopwatch.StartNew();
                        }
                        Talk("Cáp Đại Bá");
                        QuestFrame.ClickAll();
                    }
                }
            }
        }

        public void DatDoiQ123LauLan()
        {
            if (Objects.NearMonter(QDistance, Qx, Qy).Count > 0)
                QLauLanClearMonterTime = Stopwatch.StartNew();
            if (QLauLanClearMonterTime.Elapsed.TotalMinutes > 4)
            {
                QLAULANSTATE = 0;
                QLauLanClearMonterTime = Stopwatch.StartNew();
            }

            List<string> pointBoss = new List<string>()
            {
                "57,81",
"78,68",
"83,38",
"52,31",
"33,64",
"55,56",
            };

            bool IsPointBoss()
            {
                foreach (string p in pointBoss)
                {
                    if (GetDistance(p) <= 5)
                        return true;
                }
                return false;
            }

            List<string> point = new List<string>()
            {
                "139,181=>15,10",
                "132,166=>15,10",
"123,179=>15,10",
"112,163=>15,10",
"97,158=>15,10",
"90,183=>15,10",

"57,81=>15,10",
"78,68=>15,10",
"83,38=>15,10",
"52,31=>15,10",
"33,64=>15,10",
"55,56=>15,10",

"212,40=>15,10",
            };

            if (TLBB.MapId != MAP.ViemMaSon)
            {
                if (FullPartyGoToEx(LAULAN.HaDuyet))
                {
                    Party.ForEach(p => p.IsP = true);
                }
            }
            else
            {
                if (QLAULANSTATE <= 0)
                    QLAULANSTATE = 0;
                if (QLAULANSTATE >= point.Count)
                    QLAULANSTATE = point.Count - 1;
                string curpoint = point[QLAULANSTATE];
                int x = TDT.ParseInt(curpoint);
                int y = TDT.ParseInt(curpoint.Replace(x + ",", ""));
                Qx = x;
                Qy = y;
                curpoint = curpoint.Split(new[] { "=>" }, StringSplitOptions.None)[1];
                int distance = TDT.ParseInt(curpoint);
                int duration = TDT.ParseInt(curpoint.Replace(distance + ",", ""));
                QDistance = distance;

                Party.Where(p => p.TLBB.PlayerState == 0).ToList().ForEach(p => p.GoToEx(x, y));

                if (GetDistance(x, y) <= 3)
                {
                    int maxdura = 3;

                    if (IsPointBoss())
                    {
                        foreach (GameObject o in Objects.All)
                        {
                            if (o.Menpai == 12 || o.IsMonter)
                            {
                                QLauLanClearMonterTime = Stopwatch.StartNew();
                                return;
                            }
                        }
                        maxdura = 10;
                    }
                    if (QLauLanClearMonterTime.Elapsed.TotalSeconds >= duration && TrueStandTime.Elapsed.TotalSeconds >= maxdura)
                    {
                        QLAULANSTATE++;
                        QLauLanClearMonterTime = Stopwatch.StartNew();
                        return;
                    }
                }
            }
        }

        public void DatDoiKyCuoc()
        {
            if (IsXongKyCuoc)
            {
                RemoveMission(MissionsType.DatDoiKyCuoc);
                if (IsByLogin)
                    Party.ForEach(g => g.IsXongBHD = true);
                return;
            }
            if (TLBB.MapId != MAP.TranLongKyCuoc)
            {
                if (FullPartyGoToEx(LACDUONG.VuongTichTan))
                {
                    // 401001 110
                    // 90 113
                    // 808039 1
                    // 808039 11
                    if (TLBB.IsQuestOpen)
                    {
                        if (QuestFrame.Click(401001, 110))
                            return;
                        if (Missions.Contains(MissionsType.KyCuocNhanh) || Setting.Is("checkDiCoNhanh"))
                        {
                            QuestFrame.Click(90, 112);
                            Thread.Sleep(1000);
                            if (QuestFrame.Text.Contains("#{FBJR_091019_13}"))
                            {
                                RemoveMission(MissionsType.DatDoiKyCuoc);
                                if (IsByLogin)
                                    Party.ForEach(g => g.IsXongBHD = true);
                            }
                        }
                        else
                        {
                            QuestFrame.Click(90, 113);
                            Thread.Sleep(1000);
                            if (QuestFrame.Text.Contains("#{FBJR_091019_13}"))
                            {
                                RemoveMission(MissionsType.DatDoiKyCuoc);
                                if (IsByLogin)
                                    Party.ForEach(g => g.IsXongBHD = true);
                            }
                        }
                        QuestFrame.Click(401001, -1);
                        //CloseQuest();
                        QuestFrame.Close();
                    }
                    else
                    {
                        Talk(LACDUONG.VuongTichTan.Id);
                    }
                }
            }
            else
            {
                TrieuTap();
                StopFollow();
                if (ClearMonterTimeEx.Elapsed.TotalSeconds > 35)
                {
                    Move(88, 88);
                }
            }
        }

        public void DatDoiTamThan()
        {
            if (IsHoiSinh)
                return;
            if (!Global.IsTamThan)
                return;
            if (!IsOneSec)
                return;
            if (TLBB.MapId != MAP.TamThanHuyenCanh)
            {
                if (FullPartyGoToEx(KimLang.BichLac))
                {
                    //#{SSHJ_150915_15}
                    if (TLBB.IsQuestOpen)
                    {
                        QuestFrame.Click("#{FMYZ_180529_2}");
                        Thread.Sleep(500);
                        if (QuestFrame.Text.Contains("#{MZFB_150810_36"))
                        {
                            RemoveMission(MissionsType.DatDoiTamThan);
                            if (Main.IsBanRac)
                                Party.ForEach(game => game.PushMissions(new[] { MissionsType.TriLieu, MissionsType.BanRac, MissionsType.CatVang }));
                        }
                        QuestFrame.Close();
                    }
                    else
                    {
                        Talk(KimLang.BichLac);
                    }
                }
            }
            else
            {
                if (Objects.Monters.Count > 0)
                {
                    DownRidePartyEx();
                    StopFollow();
                }
                if (PartyMinSwBuffNM <= 5)
                    return;
                TrieuTap();
                if (Objects.Monters.Count == 0)
                {
                    if (!DeadObjects.ContainsKey("Liệt Hải Ma Long"))
                    {
                        if (FullPartyGoToEx(62, 105))
                        {
                            foreach (GameObject obj in Objects.All)
                            {
                                if (obj.RoundX == 62 && obj.RoundY == 105 && obj.Menpai == 10)
                                {
                                    Talk(obj);
                                    Thread.Sleep(1000);
                                }
                            }
                            if (TLBB.IsQuestOpen)
                            {
                                QuestFrame.Click("#{MZFB_150810_45}");
                                QuestFrame.Close();
                            }
                        }
                    }
                    else if (!DeadObjects.ContainsKey("Diệt Thế Hỏa Phụng"))
                    {
                        if (IsObjectDead("Liệt Hải Ma Long"))
                        {
                            if (FullPartyGoToEx(130, 76))
                            {
                                foreach (GameObject obj in Objects.All)
                                {
                                    if (obj.RoundX == 130 && obj.RoundY == 76 && obj.Menpai == 10)
                                    {
                                        Talk(obj);
                                        Thread.Sleep(1000);
                                    }
                                }
                                if (TLBB.IsQuestOpen)
                                {
                                    QuestFrame.Click("#{MZFB_150810_45}");
                                    QuestFrame.Close();
                                }
                            }
                          
                        }
                    }
                    else if (!DeadObjects.ContainsKey("Phệ Hoa Yêu"))
                    {
                        if (IsObjectDead("Diệt Thế Hỏa Phụng"))
                        {
                            if (FullPartyGoToEx(131, 139))
                            {
                                foreach (GameObject obj in Objects.All)
                                {
                                    if (obj.RoundX == 131 && obj.RoundY == 139 && obj.Menpai == 10)
                                    {
                                        Talk(obj);
                                        Thread.Sleep(1000);
                                    }
                                }
                                if (TLBB.IsQuestOpen)
                                {
                                    QuestFrame.Click("#{MZFB_150810_45}");
                                    QuestFrame.Close();
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}