using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace _i
{
    internal partial class Game
    {
        public void Auto()
        {
            if (!IsInit)
            {
                Init();
                return;
            }

            if (Main.IsHold)
                return;

            if (swIsOneSec.Elapsed.TotalSeconds >= 1)
            {
                swIsOneSec = Stopwatch.StartNew();
                IsOneSec = true;
                SecCount++;
            }
            else
            {
                IsOneSec = false;
            }

          

            lock (syncObj)
            {
                TLBB.Read();
                Objects.Read();
            }

            return;

            AutoHideGame();

            AutoExitGame();

            if (TLBB.IsSelectServer || TLBB.IsLogon || TLBB.IsNexLogin || TLBB.IsSelectRole)
            {
                IsSettingLoaded = false;
                isAlarmBachHoaDuyenCompleted = false;
                IsXongBHD = false;
                if (Setting.Is("checkKhongChiemManHinh") && !IsWriteKhongChiemMan)
                {
                    if (Handle != IntPtr.Zero)
                    {
                        IsWriteKhongChiemMan = true;
                        PostMessage(0, -12);
                        PostMessage(0, -11);
                    }
                }
                FakeMac();
                return;
            }
            if (TLBB.ON_SCENE_TRANSING || IsChangeMap || lasmapid != TLBB.MapId)
            {
                FreshVal();
                DoStringEx("IsDone = ''; TKCINFO = '';");
                return;
            }

            if (tranTime.Elapsed.TotalSeconds < 3)
            {
                return;
            }

            ExecRecv();

            Party = PartyEx.ToList();
            Leader = Party.Where(o => o.TLBB.IsLeader && o.TLBB.Id == TLBB.KeyId).FirstOrDefault();

            IsPhanDame = false;

            if (!IsSettingLoaded)
            {
                if (TLBB.Online)
                {
                    IsSettingLoaded = true;
                    LoadSetting();
                }
                return;
            }

            DefineVar();

            if (!TLBB.Online)
                return;

            if (Global.IsFull > 0)
            {
                Alarm();

                if (TLBB.MapId == MAP.DiaPhu || TLBB.MapId == MAP.DiaPhuDaiTheGioi || TLBB.MapId == MAP.GiamNguc)
                {
                    RaKhoiGiamNguc();
                    RaKhoiDiaphu();
                    return;
                }

                CollectItem();

                BuffPet();
                UseItem();

                if (IsOneSec)
                {
                    LoadSkill();

                    SetMicroHandle();
                    XuatPet();
                    Buff();

                    ResetTime();
                    GiaiDoc();
                    DungThoLinhChau();
                    DungDinhViPhu();

                    if (SecCount % 3 == 0)
                    {
                        DropItem();
                        CatDoThienCo();
                    }

                    Dungx2();

                    MuaDoShop();

                    UpLvl();

                    HopDienTich();
                    HopVoHon();

                    AOE();
                    AutoBuyItem();
                    Accept();
                    CongDiemNhanVat();
                    CongDiemPet();
                    AcTac();
                    Rao();

                    AutoXuongNgua();

                    TieuKNBKhoa();
                    Muax2();
                    SetCalendar();
                    AutoCreateTeam();
                    OpenShop();
                    CanQuet();
                    AutoUnlockPass2();
                    AutoDiemDanh();
                    AutoChucPhucBanBe();
                    GetLostLeader();
                    TrimProc();
                    ChucPhucMaoBut();
                }

                if (SellItem())
                    return;
                HoTroPhuBan();

                if (PickItem())
                    return;

                if (HoiSinh())
                    return;
                if (NhatHop())
                    return;

                if (IsOneSec && Address.GameType != 1)
                {
                    PostMessage(57, 105);
                }

                if (TLBB.PetHP > 0)
                {
                    if (TLBB.PetLvl >= Global.PetLvl && SecCount % 2 == 0)
                    {
                        DoAction("PetSkill2_2");
                    }
                }

                if (Address.GameType == 1)
                {
                    if (SecCount % 60 == 0 && TLBB.OnlineTimeSec > 5)
                    {
                        DoStringEx("setmetatable(_G, {__index = FreshmanWatch_Env}); if this:IsVisible() then FreshmanWatch_Bn2Click(); end");
                    }
                }

                DatDoiThuyLao();

                if (!Global.Paused && IsOneSec && TLBB.IsLeader && !IsWaitFast && !(Setting.Is("checkPhuBanDuNguoi") && Party.Count() < 6) && !IsBusyAll)
                {

                    if (Setting.Is("checkTuKeoDoi"))
                    {
                        if (TLBB.MapId == MAP.BinhThanhKyTran)
                            DatDoiBinhThanh();
                        else if (TLBB.MapId == MAP.PhieuMieuPhong)
                            DatDoiPhieuMieuPhong();
                        else if (TLBB.MapId == MAP.YenTuO)
                            DatDoiYenTuO();
                        else if (TLBB.MapId == MAP.TuTuyetTrang)
                            DatDoiTuTuyetTrang();
                        else if (TLBB.MapId == MAP.ThieuThatSon)
                            DatDoiThieuThatSon();
                        else if (TLBB.MapId == MAP.LoiDaiSinhTu)
                            DatDoiSatTinh();
                        else if (TLBB.MapId == MAP.ViemMaSon)
                            DatDoiQ123LauLan();
                        else if (TLBB.MapId == MAP.TamTaiHiepCoc)
                            DatDoiQ123ToChau();
                        else if (TLBB.MapId == MAP.LanHoanPhucDia)
                            DatDoiPhucDia();
                        else if (TLBB.MapId == MAP.PhungMinhVuongLang)
                            DatDoiVuongLang();
                        else if (TLBB.MapId == MAP.TamThanHuyenCanh)
                            DatDoiTamThan();
                        else if (TLBB.MapId == MAP.TacKhauDoanhDia)
                            DatDoiAcTac();
                        else if (IsMapAcBa)
                            DatDoiAcBa();
                        else if (TLBB.MapId == MAP.TangKinhCac)
                            DatDoiTangKinhCac();
                        else if (TLBB.MapId == MAP.LauLanBaoTang)
                            DatDoiLauLanTamBao();
                        else if (TLBB.MapId == MAP.TranLongKyCuoc)
                            DatDoiKyCuoc();
                        else if (TLBB.MapId == MAP.MauDonUyen)
                            DatDoiTucCau();
                        else if (TLBB.MapId == MAP.ThanhThuSonPhuBan)
                            DatDoiDaTru();
                        else if (TLBB.MapId == MAP.NongTruongDaTru)
                            DatDoiMongHeo();
                        else if (TLBB.MapId == MAP.HuyenVuDaoPhuBan)
                            DatDoiThienGiangKyThu();
                        else if (TLBB.MapId == MAP.PhungHoangCoThanhPhuBan)
                            DatDoiPhungHoangLangMo();
                        else if ((TLBB.MapId == MAP.TrucLam || TLBB.MapId == MAP.BienGioiTongLieu))
                            DatDoiQ12TinhKiem();
                    }
                    else
                    {
                        if (Missions.Contains(MissionsType.DatDoiBinhThanh))
                            DatDoiBinhThanh();
                        else if (Missions.Contains(MissionsType.DatDoiPhieuMieuPhong) || Missions.Contains(MissionsType.DatDoiKhieuChienPhieuMieuPhong))
                            DatDoiPhieuMieuPhong();
                        else if (Missions.Contains(MissionsType.DatDoiYenTuO))
                            DatDoiYenTuO();
                        else if (Missions.Contains(MissionsType.DatDoiTuTuyetTrang))
                            DatDoiTuTuyetTrang();
                        else if (Missions.Contains(MissionsType.DatDoiThieuThatSon))
                            DatDoiThieuThatSon();
                        else if (Missions.Contains(MissionsType.DatDoiSatTinh))
                            DatDoiSatTinh();
                        else if (Missions.Contains(MissionsType.DatDoiQ123LauLan))
                            DatDoiQ123LauLan();
                        else if (Missions.Contains(MissionsType.DatDoiQ123ToChau))
                            DatDoiQ123ToChau();
                        else if (Missions.Contains(MissionsType.DatDoiPhucDia))
                            DatDoiPhucDia();
                        else if (Missions.Contains(MissionsType.DatDoiVuongLang))
                            DatDoiVuongLang();
                        else if (Missions.Contains(MissionsType.DatDoiTamThan))
                            DatDoiTamThan();
                        else if (Missions.Contains(MissionsType.DatDoiAcTac))
                            DatDoiAcTac();
                        else if (Missions.Contains(MissionsType.DatDoiAcBa))
                            DatDoiAcBa();
                        else if (Missions.Contains(MissionsType.DatDoiTangKinhCac))
                            DatDoiTangKinhCac();
                        else if (Missions.Contains(MissionsType.DatDoiLauLanTamBao))
                            DatDoiLauLanTamBao();
                        else if (Missions.Contains(MissionsType.DatDoiKyCuoc))
                            DatDoiKyCuoc();
                        else if (Missions.Contains(MissionsType.DatDoiTucCau))
                            DatDoiTucCau();
                        else if (Missions.Contains(MissionsType.DatDoiDaTru))
                            DatDoiDaTru();
                        else if (Missions.Contains(MissionsType.DatDoiMongHeo))
                            DatDoiMongHeo();
                        else if (Missions.Contains(MissionsType.DatDoiThienGiangKyThu))
                            DatDoiThienGiangKyThu();
                        else if (Missions.Contains(MissionsType.DatDoiPhungHoangLangMo))
                            DatDoiPhungHoangLangMo();
                        else if (Missions.Contains(MissionsType.DatDoiMaTac))
                            DatDoiMaTac();
                        else if (Missions.Contains(MissionsType.DatDoiBaoDoHiem))
                            DatDoiBaoDoHiem();
                        else if (Missions.Contains(MissionsType.DatDoiQuanSonHai))
                            DatDoiQuanSonHai();
                        else if (Missions.Contains(MissionsType.DatDoiBossMap))
                            DatDoiBossMap();
                        else if (Missions.Contains(MissionsType.DatDoiTyVo))
                            DatDoiTyVo();
                        else if (Q12TK != 0)
                            DatDoiQ12TinhKiem();
                    }
                }

                KetBai();
                KetBaisudo();
                Muax2Shop();

                if (Missions.Contains(MissionsType.TriLieu))
                    TriLieu();
                else if (Missions.Contains(MissionsType.BanRac))
                    GoSell();
                else if (Missions.Contains(MissionsType.CatVang))
                    SaveGold();
                else if (SaveItem())
                {
                }
                else if (Missions.Contains(MissionsType.LayDo) || Missions.Contains(MissionsType.LayVang))
                    LayDo();
                else if (Missions.Contains(MissionsType.PhanGiaiTrangBiPet))
                    PhanGiaiTrangBiPet();
                else if (Missions.Contains(MissionsType.NhanKiemChi) || Missions.Contains(MissionsType.NhanChienCong))
                    NopChienHonNgoc();
                else if (Missions.Contains(MissionsType.NhanThuongQuanSonHai))
                    NhanQuanSonHai();
                else if (Missions.Contains(MissionsType.NhanThuongTyVo))
                    NhanThuongTyVo();
                else if (Missions.Contains(MissionsType.NhanBuaBaoRuong))
                    NhanBuaBaoRuong();
                else if (Missions.Contains(MissionsType.NhanLeBao))
                    NhanLeBao();
                else if (Missions.Contains(MissionsType.NhanBong))
                    NhanBong();
                else if (Missions.Contains(MissionsType.NhanKeoHallowen))
                    NhanKeoHallowen();
                else if (Missions.Contains(MissionsType.NhanBoiThuong))
                    NhanBoiThuong();
                else if (Missions.Contains(MissionsType.LinhLuong))
                    LinhLuong();
                else if (IsNhatTuyet)
                    NhatTuyet();
                else if (Missions.Contains(MissionsType.NhanX2))
                    Nhanx2();
                else if (Missions.Contains(MissionsType.DongX2))
                    Dongx2();
                else if (AutoComBack())
                {
                }
                else if (IsRunAutoMap)
                    MoveNext();
                else if (Missions.Contains(MissionsType.KhaiKhoang) || Missions.Contains(MissionsType.HaiDuoc))
                    KhaiKhoang();
                else if (IsRunAutoBossMap)
                    MoveNextBossMap();
                else if (Missions.Contains(MissionsType.DungDoatBaoRuong))
                    DungDoatBaoRuong();
                else if (Missions.Contains(MissionsType.Doi999HoaHong))
                    DoiHoaHong();
                else if (Missions.Contains(MissionsType.TangCapTruongThanhLongVan))
                    TangCapTruongThanhLongVan();
                else if (Missions.Contains(MissionsType.VoTuPho))
                    VoTuPho();
                else if (Missions.Contains(MissionsType.AutoBuyKNB))
                    BuyKNB();
                else if (Missions.Contains(MissionsType.SuaTrangBi))
                    SuaTrangBi();
                else if (Missions.Contains(MissionsType.MoBaoTangDo) || TLBB.MapId == MAP.HuyetMo)
                    MoBTD();
                else if (Missions.Contains(MissionsType.SuaThanKhi))
                    SuaThanKhi();
                else if (Missions.Contains(MissionsType.SuaVoHon))
                    SuaVoHon();
                else if (Missions.Contains(MissionsType.CheThanKhi))
                    CheThanKhi();
                else if (IsGiamDinhDo)
                    GiamDinhDo();
                else if (IsCuongHoa7)
                    CuongHoa7();
                else if (Missions.Contains(MissionsType.NangTamPhap))
                    NangTamPhap();
                else if (Missions.Contains(MissionsType.GiaoNguHanhPhapThiep))
                    GiaoNguHanhThiep();
                else if (Missions.Contains(MissionsType.NopTuViHuyTinh))
                    NopTuViHuyTinh();
                else if (Missions.Contains(MissionsType.SinhTieu))
                    SinhTieu();
                else if (Missions.Contains(MissionsType.DiMuaNga))
                    MuaNgua();
                else if (Missions.Contains(MissionsType.NhanBienThan))
                    NhanBienThan();
                else if (Missions.Contains(MissionsType.BachHoaDuyen))
                    BachHoaDuyen();
                else if (Missions.Contains(MissionsType.LuyenKim) || Missions.Contains(MissionsType.LuyenKimNhanh))
                    LuyenKim();
                else if (Missions.Contains(MissionsType.ThanKhi9Sao))
                    ThanKhi9Sao();
                else if (Missions.Contains(MissionsType.NhiemVuChinhTuyen))
                    ChinhTuyen();
                else if (Missions.Contains(MissionsType.TuBaoBon))
                    TuBaoBon();
                else if (Missions.Contains(MissionsType.MoTiemThuoc))
                    MoTiemThuoc();
                else if (Missions.Contains(MissionsType.MoShopQuyThi))
                    MoShopQuyThi();
                else if (Missions.Contains(MissionsType.MoShopBachBaoCac))
                    MoBachBaoCac();
                else if (Missions.Contains(MissionsType.MoShopHungBa))
                    MoShopHungBa();
                else if (Missions.Contains(MissionsType.MoShopTrungDo))
                    MoShopTrungDo();
                else if (Missions.Contains(MissionsType.TuDuongCon))
                    TuDuong();
                else if (Missions.Contains(MissionsType.LongPhuMau))
                    LongPhuMau();
                else if (Missions.Contains(MissionsType.ThuTaiVanMay) || Missions.Contains(MissionsType.LoLyHoa) || Missions.Contains(MissionsType.NguyenVongThienLinh))
                    NhiemVuHangNgay();
                else if (IsSumonEx)
                    SuMon();
                else if (Missions.Contains(MissionsType.TruAc))
                    TruAc();
                else if (Missions.Contains(MissionsType.ThienLongTueHong) && !Missions.Contains(MissionsType.NhiemVuSuMon))
                    TueHong();
                else if (Missions.Contains(MissionsType.TrungAc) && TLBB.Lvl >= 30)
                    TrungAc();
                else if (ApTieu != 0)
                    ApTieuPhungMinhTran();
                else if (Missions.Contains(MissionsType.TuLuyenTheLuc))
                    TuLuyenTheLuc();
                else if (Missions.Contains(MissionsType.TuLuyenCuongLuc))
                    TuLuyenNgoaiCong();
                else if (Missions.Contains(MissionsType.TuLuyenNoiLuc))
                    TuLuyenNoiCong();
                else if (Missions.Contains(MissionsType.TuLuyenThanPhap))
                    TuLuyenThanPhap();
                else if (Missions.Contains(MissionsType.NhanCon))
                    NhanCon();
                else if (Missions.Contains(MissionsType.DoiLoanPhiMatHam))
                    DoiLoanPhiMatHam();
                else if (Missions.Contains(MissionsType.DoiChanNguyenLinhPhach))
                    DoiChanNguyenLinhPhach();
                else if (Missions.Contains(MissionsType.DoiTiemNangTan))
                    DoiTiemNangTan();
                else if (Missions.Contains(MissionsType.DoiKimTamTy))
                    DoiKimTamTy();
                else if (Missions.Contains(MissionsType.DoiHuyenSacCauThienThai))
                    DoiHuyenSac();
                else if (Missions.Contains(MissionsType.ThanhLyNhiemVu))
                    ThanhLyNhiemVu();
                else if (Missions.Contains(MissionsType.NhanPhiThuy))
                    NhanPhiThuy();
                else if (Missions.Contains(MissionsType.HuyVatPhamQuy))
                    HuyVatPhamQuy();
                else if (Missions.Contains(MissionsType.Mua100KimSangDuoc))
                    Mua100KimSangDuoc();
                else if (IsSuDoo)
                    SuDoo();
                else if (IsKetNghia)
                    KetNghia();
                else if (IsThoiBong)
                    ThoiBong();
                else if (IsTuoiHoa)
                    TuoiHoaHong();
                else if (Missions.Contains(MissionsType.NhiemVuVoY))
                    VoY();
                else if (Missions.Contains(MissionsType.NhanHoaHongLo))
                    NhanHoaHongLo();
                else if (Missions.Contains(MissionsType.NhanQuaBuiHoaHong))
                    NhanQuaBuiHoaHong();
                else if (Missions.Contains(MissionsType.DaiLeHungVuong))
                    DoiNgocThoiTrang();
                else if (Missions.Contains(MissionsType.TrangSucCuuLe))
                    TrangSucCuuLe();
                else if (Missions.Contains(MissionsType.TamKy))
                    TamKy();
                else if (Missions.Contains(MissionsType.NhiemVuThangCap) || Missions.Contains(MissionsType.NhiemVuKNBKhoa) || Missions.Contains(MissionsType.NgoChanNguyen) || Missions.Contains(MissionsType.NhiemVuExp))
                    _NhiemVuCoBan();
                else
                {
                    if (NeedToMove != string.Empty)
                    {
                        if (GoToEx(NeedToMove))
                        {
                            if (NeedToMove == "356,436,762")
                            {
                                for (int i = 0; i < 10; i++)
                                {
                                    DoStringEx(@"Clear_XSCRIPT()
                                                Set_XSCRIPT_Function_Name('FiveElementsCohesion')
                                                Set_XSCRIPT_ScriptID(880039)
                                                Set_XSCRIPT_Parameter(0, -1)
                                                Set_XSCRIPT_Parameter(1, 0)
                                                Set_XSCRIPT_ParamCount(2)
                                                Send_XSCRIPT()");
                                }
                                if (TLBB.IsRide)
                                    DownRide();
                                else
                                    NeedToMove = string.Empty;
                            }
                            else
                            {
                                if (TLBB.IsRide)
                                    DownRide();
                                else
                                    NeedToMove = string.Empty;
                            }
                            if (TLBB.MapId == MAP.TienTrang)
                            {
                                DoStringEx("Auction:OpenOnSaleWindow()");
                                Thread.Sleep(1000);

                                for (int i = 0; i < 10; i++)
                                {
                                    DoStringEx("Auction:ReUpExpired(2 ," + i + ",0)");
                                    Thread.Sleep(500);
                                }
                            }
                        }
                    }
                    else
                    {
                        FollowKey();
                    }
                }

                TrongTrot();

                NhanTiemNangTan();
                NhanTiemNangTanKNB();

                Phuong();
                if (NgamyQuyCoc())
                    return;

                if (!Main.PausedSkill && NeBayTime.Elapsed.TotalSeconds > 3)
                {
                    if (!Objects.Monters.Any(o => o.CleanName == "modungphuc" || o.CleanName == "furong"))
                    {
                        if (!IsPhanDame && !TLBB.IsBienThan)
                        {
                            SkillDo();

                            SendKey();
                        }
                    }
                }
                PackUp();
            }
            Attack();
        }

    }
}