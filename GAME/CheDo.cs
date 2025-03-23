using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace _i
{
    partial class Game
    {
        public static bool IsGiamDinh
        {
            get;
            set;
        }

        public static bool IsMuaNguyenLieu
        {
            get;
            set;
        }

        public int MuaCount = 0;
        public string MuaName = string.Empty;
        public static int shopIndex = 6;

        public static bool Is69DO { get; set; }

        public void CheDoStart()
        {
            IsRunCraft = true;
            new Thread(new ThreadStart(CheDo))
            {
                IsBackground = true
            }.Start();
        }

        public bool IsRunCraft = false;

        public void CheDo()
        {
            while (true)
            {
                MuaCount = 0;
                if (!IsCheDo)
                {
                    IsRunCraft = false;
                    break;
                }
                try
                {
                    GiamDinh();
                    if (IsMuaNguyenLieu)
                    {
                        string name = HaveNguyenLieu();
                        if (name != "")
                        {
                            MuaCount = 20;
                            MuaName = name;
                        }
                        else if (MuaCount == 0)
                        {
                            name = HaveDTD();
                            if (name != "")
                            {
                                MuaCount = 20;
                                MuaName = name;
                            }
                        }
                        if (MuaCount == 0)
                        {
                            if (TLBB.IsToggleYuanbaoShop)
                            {
                                LUA.HideYuanbaoShop();
                            }
                        }
                        if (MuaCount > 0)
                        {
                            if (!TLBB.IsToggleYuanbaoShop)
                            {
                                LUA.ToggleYuanbaoShop();
                                Thread.Sleep(1000);
                            }
                            if (MuaName == "vaibong" || MuaName == "bingan" || MuaName == "tinhthiet" || MuaName == "darksilver" || MuaName == "cottoncloth" || MuaName == "refinediron")
                            {
                                int index = -1;
                                foreach (var item in PacketItems.Shop)
                                {
                                    if (TDT.VietLien(item.Name).Contains(MuaName) && (TDT.VietLien(item.Name).Contains("cap") || item.Name.Contains("Lv ")))
                                    {
                                        index = (int)item.Index;
                                    }
                                }
                                if (index != -1)
                                {
                                    while (MuaCount-- > 0)
                                    {
                                        Buy(index);
                                        Thread.Sleep(500);
                                    }
                                }
                                if (Is69DO)
                                {
                                    DoStringEx("setmetatable(_G, {__index = YuanbaoShop_Env}); YuanbaoShop_ChangeTabIndex(1);");
                                    Thread.Sleep(1000);
                                    DoStringEx("setmetatable(_G, {__index = YuanbaoShop_Env}); YuanbaoShop_UpdateList_Bind(4); YuanbaoShop_UpdateShop_Bind(2);");
                                }
                                else
                                {
                                    LUA.YuanbaoShop(4, 2);
                                }
                                Thread.Sleep(1000);
                            }
                            string muaName = TDT.VietLien(CheTen);
                            if (muaName.Contains("daophu") || muaName.Contains("thuongbong"))
                            {
                                foreach (var item in PacketItems.Shop)
                                {
                                    if (TDT.VietLien(item.Name).Contains(MuaName))
                                    {
                                        while (MuaCount-- > 0)
                                        {
                                            Buy(item.Index);
                                            Thread.Sleep(500);
                                        }
                                    }
                                }
                                if (Is69DO)
                                {
                                    DoStringEx("setmetatable(_G, {__index = YuanbaoShop_Env}); YuanbaoShop_ChangeTabIndex(1);");
                                    Thread.Sleep(1000);
                                    DoStringEx("setmetatable(_G, {__index = YuanbaoShop_Env}); YuanbaoShop_UpdateList_Bind(6); YuanbaoShop_UpdateShop_Bind(1);");
                                }
                                else
                                {
                                    LUA.YuanbaoShop(shopIndex, 1);
                                }
                                Thread.Sleep(1000);
                            }
                            if (muaName.Contains("dondoan") || muaName.Contains("songdoan"))
                            {
                                foreach (var item in PacketItems.Shop)
                                {
                                    if (TDT.VietLien(item.Name).Contains(MuaName))
                                    {
                                        while (MuaCount-- > 0)
                                        {
                                            Buy(item.Index);
                                            Thread.Sleep(500);
                                        }
                                    }
                                }
                                if (Is69DO)
                                {
                                    DoStringEx("setmetatable(_G, {__index = YuanbaoShop_Env}); YuanbaoShop_ChangeTabIndex(1);");
                                    Thread.Sleep(1000);
                                    DoStringEx("setmetatable(_G, {__index = YuanbaoShop_Env}); YuanbaoShop_UpdateList_Bind(6); YuanbaoShop_UpdateShop_Bind(2);");
                                }
                                else
                                {
                                    LUA.YuanbaoShop(shopIndex, 2);
                                }
                                Thread.Sleep(1000);
                            }
                            if (muaName.Contains("phien") || muaName.Contains("hoan"))
                            {
                                foreach (var item in PacketItems.Shop)
                                {
                                    if (TDT.VietLien(item.Name).Contains(MuaName))
                                    {
                                        while (MuaCount-- > 0)
                                        {
                                            Buy(item.Index);
                                            Thread.Sleep(500);
                                        }
                                    }
                                }
                                if (Is69DO)
                                {
                                    DoStringEx("setmetatable(_G, {__index = YuanbaoShop_Env}); YuanbaoShop_ChangeTabIndex(1);");
                                    Thread.Sleep(1000);
                                    DoStringEx("setmetatable(_G, {__index = YuanbaoShop_Env}); YuanbaoShop_UpdateList_Bind(6); YuanbaoShop_UpdateShop_Bind(3);");
                                }
                                else
                                {
                                    LUA.YuanbaoShop(shopIndex, 3);
                                }
                                Thread.Sleep(1000);
                            }
                            if (muaName.Contains("mao") || muaName.Contains("yphuc"))
                            {
                                foreach (var item in PacketItems.Shop)
                                {
                                    if (TDT.VietLien(item.Name).Contains(MuaName))
                                    {
                                        while (MuaCount-- > 0)
                                        {
                                            Buy(item.Index);
                                            Thread.Sleep(500);
                                        }
                                    }
                                }
                                if (Is69DO)
                                {
                                    DoStringEx("setmetatable(_G, {__index = YuanbaoShop_Env}); YuanbaoShop_ChangeTabIndex(1);");
                                    Thread.Sleep(1000);
                                    DoStringEx("setmetatable(_G, {__index = YuanbaoShop_Env}); YuanbaoShop_UpdateList_Bind(6); YuanbaoShop_UpdateShop_Bind(4);");
                                }
                                else
                                {
                                    LUA.YuanbaoShop(shopIndex, 4);
                                }
                                Thread.Sleep(1000);
                            }
                            if (muaName.Contains("hothu") || muaName.Contains("hai"))
                            {
                                foreach (var item in PacketItems.Shop)
                                {
                                    if (TDT.VietLien(item.Name).Contains(MuaName))
                                    {
                                        while (MuaCount-- > 0)
                                        {
                                            Buy(item.Index);
                                            Thread.Sleep(500);
                                        }
                                    }
                                }
                                if (Is69DO)
                                {
                                    DoStringEx("setmetatable(_G, {__index = YuanbaoShop_Env}); YuanbaoShop_ChangeTabIndex(1);");
                                    Thread.Sleep(1000);
                                    DoStringEx("setmetatable(_G, {__index = YuanbaoShop_Env}); YuanbaoShop_UpdateList_Bind(6); YuanbaoShop_UpdateShop_Bind(5);");
                                }
                                else
                                {
                                    LUA.YuanbaoShop(shopIndex, 5);
                                }
                                Thread.Sleep(1000);
                            }
                            if (muaName.Contains("houyen") || muaName.Contains("hokien"))
                            {
                                foreach (var item in PacketItems.Shop)
                                {
                                    if (TDT.VietLien(item.Name).Contains(MuaName))
                                    {
                                        while (MuaCount-- > 0)
                                        {
                                            Buy(item.Index);
                                            Thread.Sleep(500);
                                        }
                                    }
                                }
                                if (Is69DO)
                                {
                                    DoStringEx("setmetatable(_G, {__index = YuanbaoShop_Env}); YuanbaoShop_ChangeTabIndex(1);");
                                    Thread.Sleep(1000);
                                    DoStringEx("setmetatable(_G, {__index = YuanbaoShop_Env}); YuanbaoShop_UpdateList_Bind(6); YuanbaoShop_UpdateShop_Bind(6);");
                                }
                                else
                                {
                                    LUA.YuanbaoShop(shopIndex, 6);
                                }
                                Thread.Sleep(1000);
                            }
                            if (muaName.Contains("yeudai") || muaName.Contains("hanglien"))
                            {
                                foreach (var item in PacketItems.Shop)
                                {
                                    if (TDT.VietLien(item.Name).Contains(MuaName))
                                    {
                                        while (MuaCount-- > 0)
                                        {
                                            Buy(item.Index);
                                            Thread.Sleep(500);
                                        }
                                    }
                                }
                                if (Is69DO)
                                {
                                    DoStringEx("setmetatable(_G, {__index = YuanbaoShop_Env}); YuanbaoShop_ChangeTabIndex(1);");
                                    Thread.Sleep(1000);
                                    DoStringEx("setmetatable(_G, {__index = YuanbaoShop_Env}); YuanbaoShop_UpdateList_Bind(6); YuanbaoShop_UpdateShop_Bind(7);");
                                }
                                else
                                {
                                    LUA.YuanbaoShop(shopIndex, 7);
                                }
                                Thread.Sleep(1000);
                            }
                            if (muaName.Contains("gioichi") || muaName.Contains("hophu"))
                            {
                                foreach (var item in PacketItems.Shop)
                                {
                                    if (TDT.VietLien(item.Name).Contains(MuaName))
                                    {
                                        while (MuaCount-- > 0)
                                        {
                                            Buy(item.Index);
                                            Thread.Sleep(500);
                                        }
                                    }
                                }
                                if (Is69DO)
                                {
                                    DoStringEx("setmetatable(_G, {__index = YuanbaoShop_Env}); YuanbaoShop_ChangeTabIndex(1);");
                                    Thread.Sleep(1000);
                                    DoStringEx("setmetatable(_G, {__index = YuanbaoShop_Env}); YuanbaoShop_UpdateList_Bind(6); YuanbaoShop_UpdateShop_Bind(8);");
                                }
                                else
                                {
                                    LUA.YuanbaoShop(shopIndex, 8);
                                }
                                Thread.Sleep(100);
                            }
                        }
                    }
                    if (SecCount % 5 != 0)
                    {
                        Thread.Sleep(1000);
                    }
                    if (Global.AutoDropCraft)
                        IsDrop();
                    Che();
                }
                catch { }
            }
        }

        public void GiamDinh()
        {
            if (IsGiamDinh)
            {
                foreach (var item in PacketItems.All)
                    item.GiamDinh();
            }
        }

        public void Che()
        {
            string packet = "0C9C5F000000000000000000" + IdLoai().ToString("X2") + "000000" + IdCheCap().ToString("X2") + IdNoiNgoai().ToString("X2") + "0000FFFFFFFF" + IdNguyenLieu();
            SendPacket(packet);
            Thread.Sleep(4500);
        }

        public string DTDName()
        {
            string DTDName = string.Empty;
            if (Is69DO)
            {
                DTDName = "Lv " + (CheCap + 1) + " ";
                string name = TDT.VietLien(CheTen);
                if (name == "daophu")
                    DTDName += "Falchion Plans";
                else if (name == "thuongbong")
                    DTDName += "Spear Plans";
                else if (name == "dondoan")
                    DTDName += "One-Hand Plans";
                else if (name == "songdoan")
                    DTDName += "Two-Hand Plans";
                else if (name == "phien")
                    DTDName += "Fan Plans";
                else if (name == "hoan")
                    DTDName += "Circle Plans";
                else if (name == "mao")
                    DTDName += "Hat Pattern";
                else if (name == "yphuc")
                    DTDName += "Garment Plans";
                else if (name == "hothu")
                    DTDName += "Glove Pattern";
                else if (name == "hai")
                    DTDName += "Shoe Pattern";
                else if (name == "houyen")
                    DTDName += "Wristband Plans";
                else if (name == "hokien")
                    DTDName += "Shp. Plans";
                else if (name == "yeudai")
                    DTDName += "Belt Pattern";
                else if (name == "hanglien")
                    DTDName += "Necklace Plans";
                else if (name == "gioichi")
                    DTDName += "Ring Design";
                else if (name == "hophu")
                    DTDName += "Amulet Design";
            }
            return TDT.VietLien(DTDName);
        }

        public string HaveDTD()
        {
            string DTDName = string.Empty;
            if (TDT.VietLien(CheTen) == "daophu" || TDT.VietLien(CheTen) == "thuongbong")
            {
                DTDName = TDT.VietLien(CheTen + "dataodocap" + (CheCap + 1));
            }
            else if (TDT.VietLien(CheTen).Contains("mao"))
            {
                DTDName = TDT.VietLien(CheTen + "tudataodo" + (CheCap + 1));
            }
            else
            {
                DTDName = TDT.VietLien(CheTen + "dataodo" + (CheCap + 1));
            }
            if (Is69DO)
            {
                DTDName = "Lv " + (CheCap + 1) + " ";
                string name = TDT.VietLien(CheTen);
                if (name == "daophu")
                    DTDName += "Falchion Plans";
                else if (name == "thuongbong")
                    DTDName += "Spear Plans";
                else if (name == "dondoan")
                    DTDName += "One-Hand Plans";
                else if (name == "songdoan")
                    DTDName += "Two-Hand Plans";
                else if (name == "phien")
                    DTDName += "Fan Plans";
                else if (name == "hoan")
                    DTDName += "Circle Plans";
                else if (name == "mao")
                    DTDName += "Hat Pattern";
                else if (name == "yphuc")
                    DTDName += "Garment Plans";
                else if (name == "hothu")
                    DTDName += "Glove Pattern";
                else if (name == "hai")
                    DTDName += "Shoe Pattern";
                else if (name == "houyen")
                    DTDName += "Wristband Plans";
                else if (name == "hokien")
                    DTDName += "Shp. Plans";
                else if (name == "yeudai")
                    DTDName += "Belt Pattern";
                else if (name == "hanglien")
                    DTDName += "Necklace Plans";
                else if (name == "gioichi")
                    DTDName += "Ring Design";
                else if (name == "hophu")
                    DTDName += "Amulet Design";
            }
            foreach (var item in PacketItems.All)
            {
                if (TDT.VietLien(item.Name).Contains(TDT.VietLien(DTDName)))
                    return string.Empty;
            }
            return TDT.VietLien(DTDName);
        }

        public string HaveNguyenLieu()
        {
            switch (CheLoai)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                    if (Is69DO)
                    {
                        foreach (var item in PacketItems.All)
                        {
                            if (TDT.VietLien(item.Name).Contains("refinediron"))
                                return string.Empty;
                        }
                        return "refinediron";
                    }
                    else
                    {
                        foreach (var item in PacketItems.All)
                        {
                            if (TDT.VietLien(item.Name).Contains("tinhthiet"))
                                return string.Empty;
                        }
                        return "tinhthiet";
                    }
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                case 12:
                    if (Is69DO)
                    {
                        foreach (var item in PacketItems.All)
                        {
                            if (TDT.VietLien(item.Name).Contains("cottoncloth"))
                                return string.Empty;
                        }
                        return "cottoncloth";
                    }
                    else
                    {
                        foreach (var item in PacketItems.All)
                        {
                            if (TDT.VietLien(item.Name).Contains("vaibong"))
                                return string.Empty;
                        }
                        return "vaibong";
                    }
                case 13:
                case 14:
                case 15:
                    if (Is69DO)
                    {
                        foreach (var item in PacketItems.All)
                        {
                            if (TDT.VietLien(item.Name).Contains("darksilver"))
                                return string.Empty;
                        }
                        return "darksilver";
                    }
                    else
                    {
                        foreach (var item in PacketItems.All)
                        {
                            if (TDT.VietLien(item.Name).Contains("bingan"))
                                return string.Empty;
                        }
                        return "bingan";
                    }
                default:
                    return "";
            }
        }

        public string IdNguyenLieu()
        {
            switch (CheLoai)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                    foreach (var item in PacketItems.All)
                    {
                        if (item.Type.Contains("Ore_15"))
                            return item.Index.ToString("X2");
                    }
                    return "";

                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                case 12:
                    foreach (var item in PacketItems.All)
                    {
                        if (item.Type.Contains("Ore_14"))
                            return item.Index.ToString("X2");
                    }
                    return "";

                case 13:
                case 14:
                case 15:
                    foreach (var item in PacketItems.All)
                    {
                        if (item.Type.Contains("Ore_13"))
                            return item.Index.ToString("X2");
                    }
                    return "";

                default:
                    return "";
            }
        }

        public int IdLoai()
        {
            switch (CheLoai)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                    return 0x2e;

                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                case 12:
                    return 0x2f;

                case 13:
                case 14:
                case 15:
                    return 0x30;
            }
            return 0;
        }

        public int IdCheCap()
        {
            int num = 0;
            switch (CheLoai)
            {
                case 0:
                    return (0x84 + CheCap);

                case 1:
                    return (0x8e + CheCap);

                case 2:
                    return (0x98 + CheCap);

                case 3:
                    return (0xa2 + CheCap);

                case 4:
                    return (0xac + CheCap);

                case 5:
                    return (0xb6 + CheCap);

                case 6:
                    if (CheNoiNgoai != 1)
                    {
                        if (CheNoiNgoai == 2)
                        {
                            return (0x4c + CheCap);
                        }
                        return (0x24 + CheCap);
                    }
                    return (0xc0 + CheCap);

                case 7:
                    if (CheNoiNgoai != 1)
                    {
                        if (CheNoiNgoai == 2)
                        {
                            return (0x6a + CheCap);
                        }
                        return (0x42 + CheCap);
                    }
                    return (0xde + CheCap);

                case 8:
                    if (CheNoiNgoai != 1)
                    {
                        if (CheNoiNgoai == 2)
                        {
                            return (0x60 + CheCap);
                        }
                        return (0x38 + CheCap);
                    }
                    return (0xd4 + CheCap);

                case 9:
                    if (CheNoiNgoai != 1)
                    {
                        if (CheNoiNgoai == 2)
                        {
                            return (0x56 + CheCap);
                        }
                        return (0x2e + CheCap);
                    }
                    return (0xca + CheCap);

                case 10:
                    return (0xe8 + CheCap);

                case 11:
                    return (0xf2 + CheCap);

                case 12:
                    if (CheCap <= 4)
                    {
                        return (0xfc + CheCap);
                    }
                    return (0 + (CheCap - 4));

                case 13:
                    return (6 + CheCap);

                case 14:
                    if (CheNoiNgoai != 1)
                    {
                        if (CheNoiNgoai == 2)
                        {
                            if (CheCap > 6)
                            {
                                switch (CheCap)
                                {
                                    case 7:
                                        return 40;

                                    case 8:
                                        return 0x2a;

                                    case 9:
                                        return 0x2c;
                                }
                                return num;
                            }
                            return (0x10 + CheCap);
                        }
                        return (0x10 + CheCap);
                    }
                    if (CheCap <= 6)
                    {
                        return (0x10 + CheCap);
                    }
                    switch (CheCap)
                    {
                        case 7:
                            return 0x29;

                        case 8:
                            return 0x2b;

                        case 9:
                            return 0x2d;
                    }
                    return num;

                case 15:
                    if (CheNoiNgoai != 1)
                    {
                        if (CheNoiNgoai == 2)
                        {
                            if (CheCap > 6)
                            {
                                switch (CheCap)
                                {
                                    case 7:
                                        return 0x2f;

                                    case 8:
                                        return 0x30;

                                    case 9:
                                        return 50;
                                }
                                return num;
                            }
                            return (0x1a + CheCap);
                        }
                        return (0x1a + CheCap);
                    }
                    if (CheCap <= 6)
                    {
                        return (0x1a + CheCap);
                    }
                    switch (CheCap)
                    {
                        case 7:
                            return 0x2e;

                        case 8:
                            return 0x31;

                        case 9:
                            return 0x33;
                    }
                    return num;
            }
            return num;
        }

        public int IdNoiNgoai()
        {
            switch (CheLoai)
            {
                case 0:
                    return 2;

                case 1:
                    return 2;

                case 2:
                    return 2;

                case 3:
                    return 2;

                case 4:
                    return 2;

                case 5:
                    return 2;

                case 6:
                    if (CheNoiNgoai != 1)
                    {
                        if (CheNoiNgoai != 2)
                        {
                        }
                        return 3;
                    }
                    return 2;

                case 7:
                    if (CheNoiNgoai != 1)
                    {
                        if (CheNoiNgoai != 2)
                        {
                        }
                        return 3;
                    }
                    return 2;

                case 8:
                    if (CheNoiNgoai != 1)
                    {
                        if (CheNoiNgoai != 2)
                        {
                        }
                        return 3;
                    }
                    return 2;

                case 9:
                    if (CheNoiNgoai != 1)
                    {
                        if (CheNoiNgoai != 2)
                        {
                        }
                        return 3;
                    }
                    return 2;

                case 10:
                    return 2;

                case 11:
                    return 2;

                case 12:
                    if (CheCap <= 4)
                    {
                        return 2;
                    }
                    return 3;

                case 13:
                    return 3;

                case 14:
                    if (CheNoiNgoai != 1)
                    {
                        if (CheNoiNgoai == 2)
                        {
                            return 4;
                        }
                        return 3;
                    }
                    return 4;

                case 15:
                    if (CheNoiNgoai != 1)
                    {
                        if (CheNoiNgoai == 2)
                        {
                            return 4;
                        }
                        return 3;
                    }
                    return 4;
            }
            return 0;
        }

        public bool IsDrop()
        {
            bool isDrop = false;
            foreach (var item in PacketItems.All)
            {
                if (TDT.VietLien(item.TypeName) == TDT.VietLien(CheTen).Replace("hoan", "khuyen") || (!item.Name.Contains("Lv ") && Is69DO && TDT.VietLien(DTDName().Replace("lv", "")).Contains(TDT.VietLien(item.TypeName).Replace("shoulderpad", "shp.").TrimEnd('s').Replace("two-handed", "two-hand").Replace("one-handed", "one-hand"))))
                {
                    if ((item.Star < CheSao && item.Star > 0) || (item.Line < CheDong && item.Line > 0) || (CheDiem > 0 && item.TheLuc < CheDiem))
                    {
                        DropItem(item.Index);
                        isDrop = true;
                    }
                }
            }
            return isDrop;
        }


    }
}
