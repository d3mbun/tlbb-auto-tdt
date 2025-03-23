using System;
using System.Collections.Generic;
using System.Linq;

namespace _i
{
    internal class PacketItems
    {
        public Game game;

        public uint SplitIndex
        {
            get
            {
                return game.Memory.Read(game.Address.PacketItemBase[0], 0xD831C);
            }

            set
            {
                game.Memory.Write(game.Memory.ReadAddress(game.Address.PacketItemBase[0], 0xD8318), 2);
                game.Memory.Write(game.Memory.ReadAddress(game.Address.PacketItemBase[0], 0xD831C), (int)value);
            }
        }

        public PacketItems(Game game)
        {
            this.game = game;
        }

        public IEnumerable<PacketItem> All
        {
            get
            {
                return DaoCu.Concat(NguyenLieu).Concat(NhiemVu);
            }
        }

        public PacketItem this[uint index]
        {
            get
            {
                uint address = game.Memory.Read(game.Address.PacketItemBase);
                if (game.Memory.Read(address + index * 0x4) != 0)
                    return new PacketItem(game, address + index * 0x4);
                return null;
            }
        }

        public IEnumerable<PacketItem> DaoCu
        {
            get
            {
                uint address = game.Memory.Read(game.Address.PacketItemBase);
                for (uint i = 0; i < 30; i++)
                {
                    if (game.Memory.Read(address + i * 0x4) != 0)
                    {
                        yield return new PacketItem(game, address + i * 0x4);
                    }
                }
            }
        }

        public IEnumerable<PacketItem> NguyenLieu
        {
            get
            {
                uint address = game.Memory.Read(game.Address.PacketItemBase);
                for (uint i = 30; i < 60; i++)
                {
                    if (game.Memory.Read(address + i * 0x4) != 0)
                    {
                        yield return new PacketItem(game, address + i * 0x4);
                    }
                }
            }
        }

        public IEnumerable<PacketItem> NhiemVu
        {
            get
            {
                uint address = game.Memory.Read(game.Address.PacketItemBase);
                for (uint i = 60; i < 80; i++)
                {
                    if (game.Memory.Read(address + i * 0x4) != 0)
                    {
                        yield return new PacketItem(game, address + i * 0x4);
                    }
                }
            }
        }

        public IEnumerable<PacketItem> ThienCo
        {
            get
            {
                uint address = game.Memory.Read(game.Address.ThiencobagBase);
                for (uint i = 0; i < 120; i++)
                {
                    if (game.Memory.Read(address + i * 0x4) != 0)
                    {
                        yield return new PacketItem(game, address + i * 0x4);
                    }
                }
            }
        }

        public IEnumerable<uint> ThienCoTrong
        {
            get
            {
                uint address = game.Memory.Read(game.Address.ThiencobagBase);
                for (uint i = 0; i < 120; i++)
                {
                    if (game.Memory.Read(address + i * 0x4) == 0)
                    {
                        yield return i;
                    }
                }
            }
        }

        public PacketItem VuKhi
        {
            get
            {
                uint address = game.Memory.Read(new uint[] { game.Address.HaveRide1[0], game.Address.HaveRide1[1] });
                if (game.Memory.Read(address + 0 * 0x4) != 0)
                {
                    return new PacketItem(game, address + 0 * 0x4);
                }
                return null;
            }
        }

        public IEnumerable<PacketItem> TrangBi
        {
            get
            {
                uint address = game.Memory.Read(new uint[] { game.Address.HaveRide1[0], game.Address.HaveRide1[1] });
                for (uint i = 0; i < 22; i++)
                {
                    if (game.Memory.Read(address + i * 0x4) != 0)
                    {
                        yield return new PacketItem(game, address + i * 0x4);
                    }
                }
            }
        }

        public IEnumerable<PacketItem> TuiChanNguyen
        {
            get
            {
                uint address = game.Memory.Read(new uint[] { game.Address.PetBase[0], 0x1C954 });
                for (uint i = 36; i < 60; i++)
                {
                    if (game.Memory.Read(address + i * 0x4) != 0)
                    {
                        yield return new PacketItem(game, address + i * 0x4);
                    }
                }
            }
        }

        public IEnumerable<PacketItem> ThuongHoi
        {
            get
            {
                List<uint> listadd = new List<uint>();
                for (uint j = 0; j < 10; j++)
                {
                    for (uint i = 0; i < 20; i++)
                    {
                        uint adr = game.Memory.ReadAddress(new uint[] { game.AddressGameExe + 0x099AB00, 0x1e864, 0x1C * j + 0x4, 0x10 * i });
                        if (adr != 0 && !listadd.Contains(adr))
                        {
                            listadd.Add(adr);
                            PacketItem item = new PacketItem(game, adr);
                            if (item.Name != string.Empty)
                                yield return item;
                        }
                    }
                }
            }
        }

        public IEnumerable<PacketItem> Shop
        {
            get
            {
                uint address = game.Memory.Read(game.Address.BaseShopItem);
                for (uint i = 0; i < 80; i++)
                {
                    if (game.Memory.Read(address + i * 0x4) != 0)
                    {
                        yield return new PacketItem(game, address + i * 0x4);
                    }
                }
            }
        }

        public IEnumerable<PacketItem> Bank
        {
            get
            {
                uint address = game.Memory.Read(game.Address.BankBase);
                for (uint i = 0; i < 140; i++)
                {
                    if (game.Memory.Read(address + i * 0x4) != 0)
                    {
                        yield return new PacketItem(game, address + i * 0x4);
                    }
                }
            }
        }

        public IEnumerable<PacketItem> Ride
        {
            get
            {
                List<uint> list = new List<uint>();
                if (game.Memory.Read(game.Address.HaveRide1) != 0)
                    yield return new PacketItem(game, game.Memory.ReadAddress(game.Address.HaveRide1));
                if (game.Address.GameType == 1)
                {
                    uint offset = 0x0;
                    while (true)
                    {
                        game.Address.HaveRide2[2] = offset;
                        if (game.Memory.Read(game.Address.HaveRide2) != 0)
                            yield return new PacketItem(game, game.Memory.ReadAddress(game.Address.HaveRide2));
                        offset += 4;
                        if (offset > 0xC)
                            break;
                    }
                }
            }
        }

        public IEnumerable<PacketItem> LootPacket
        {
            get
            {
                uint lootItem = game.Memory.ReadAddress(game.Address.LootPacketItem);
                for (uint i = 0; i < 10; i++)
                {
                    if (game.Memory.Read(lootItem + i * 4) != 0)
                    {
                        yield return new PacketItem(game, lootItem + i * 4);
                    }
                }
            }
        }
    }

    internal class PacketItem
    {
        private Game game;

        public PacketItem(Game game, uint address)
        {
            this.game = game;
            Address = game.Memory.Read(address);
            Class = game.Memory.Read(Address);
            PacketId = game.Memory.Read(Address + 0x4);
            Count = 1;
            if (Class == game.AddressGameExe + 0x71AE24)
            {
                Name = game.Memory.ReadString(game.Memory.Read(Address + 0x28, 0x20));
                TypeName = "Chân Nguyên";
                Type = game.Memory.ReadString(game.Memory.Read(Address + 0x28, 0x5C));
            }
            else if (Class == game.Address.PacketType1 || Class == game.Address.PacketType5)
            {
                PacketTyped = 1;
                Name = game.Memory.ReadString(game.Memory.Read(Address + 0x28, 0x28));
                TypeName = game.Memory.ReadString(game.Memory.Read(Address + 0x28, 0x58));
                Type = game.Memory.ReadString(game.Memory.Read(Address + 0x28, 0x54));
                if (string.IsNullOrEmpty(Type))
                {
                    TypeName = game.Memory.ReadString(game.Memory.Read(Address + 0x28, 0x40));
                    Type = game.Memory.ReadString(game.Memory.Read(Address + 0x28, 0x3C));
                }
            }
            else if (Class == game.Address.PacketType2)
            {
                PacketTyped = 2;
                Name = game.Memory.ReadString(game.Memory.Read(Address + 0x28, 0x18));
                if (game.Address.GameType == 2)
                    Count = game.Memory.Read1Byte(Address + 0x14, 0x58);
                else
                    Count = game.Memory.Read1Byte(Address + 0x14, 0x40);
                TypeName = game.Memory.ReadString(game.Memory.Read(Address + 0x28, 0x4C));
                Type = game.Memory.ReadString(game.Memory.Read(Address + 0x28, 0x14));
            }
            else if (Class == game.Address.PacketType3)
            {
                PacketTyped = 3;
                Name = game.Memory.ReadString(game.Memory.Read(Address + 0x28, 0x1C));
                TypeName = game.Memory.ReadString(game.Memory.Read(Address + 0x28, 0x130));
                Type = game.Memory.ReadString(game.Memory.Read(Address + 0x28, 0x14));
            }
            else if (Class == game.Address.PacketType4)
            {
                PacketTyped = 4;
                Name = game.Memory.ReadString(game.Memory.Read(Address + 0x28, 0x28));
                TypeName = game.Memory.ReadString(game.Memory.Read(Address + 0x28, 0x4C));
                Type = game.Memory.ReadString(game.Memory.Read(Address + 0x28, 0x48));
            }
            else
            {
                Name = game.Memory.ReadString(game.Memory.Read(Address + 0x28, 0x58));
                Count = 1;
                TypeName = game.Memory.ReadString(game.Memory.Read(Address + 0x28, 0x50));
                Type = game.Memory.ReadString(game.Memory.Read(Address + 0x28, 0x14));
            }
            if (Class == game.Address.PacketType6 || string.IsNullOrEmpty(Name))
            {
                PacketTyped = 6;
                Name = game.Memory.ReadString(game.Memory.Read(Address + 0x28, 0x2C));
                TypeName = game.Memory.ReadString(game.Memory.Read(Address + 0x28, 0x68));
                Type = game.Memory.ReadString(game.Memory.Read(Address + 0x28, 0x28));
            }
            Name = Name.Trim();
            TypeName = TypeName.Trim();
            Type = Type.Trim();
            if (Type.Contains("Card_Icon"))
                TypeName = "Điển Tịch";
            Index = game.Memory.Read(Address + 0x10);
            if (game.Address.GameType == 1)
            {
                InfoBase = 0x46;
            }
            else
            {
                InfoBase = 0x5E;
            }
            uint info = game.Memory.Read(Address + 0x14);
            Line = game.Memory.Read1Byte(info + InfoBase);
            Star = game.Memory.Read1Byte(info + InfoBase + 0xC);
            if (Type == "Cloth2_9" || Type.Contains("Charm") || Type.Contains("CircularTaskTool64_5"))
            {
                if (game.Address.GameType == 1)
                {
                    MapId = game.Memory.Read2Byte(game.Memory.ReadAddress(new uint[] { Address + 0x14, 0x38 }));
                    X = game.Memory.Read2Byte(info + 0x3A);
                    Y = game.Memory.Read2Byte(info + 0x3C);
                }
                else
                {
                    MapId = game.Memory.Read2Byte(game.Memory.ReadAddress(new uint[] { Address + 0x14, 0x50 }));
                    X = game.Memory.Read2Byte(info + 0x52);
                    Y = game.Memory.Read2Byte(info + 0x54);
                }
            }
            Lvl = game.Memory.Read(Address + 0x28, 0x2C);
            IsCoDinh = game.Memory.Read1Byte(info + +0xD) == 1;

            if (Type == "CircularTaskTool33_6")
                CapTruongThanhLongVan = (int)game.Memory.Read1Byte(info + 0x13);
            if (TypeName == "Võ Hồn")
                CapHopThanhVoHon = (int)game.Memory.Read1Byte(info + 0x28);
        }

        public static IEnumerable<string> AllName => Main.Instance.AllOnelineGame.SelectMany(game => game.PacketItems.All.Concat(game.PacketItems.ThienCo).Select(i => i.Name)).Distinct();
        public static IEnumerable<string> AllType => Main.Instance.AllOnelineGame.SelectMany(game => game.PacketItems.All.Concat(game.PacketItems.ThienCo).Select(i => i.TypeName)).Distinct();

        public void DoAction() => game.DoAction(Type, PacketId);

        public void DoSubAction() => game.DoSubAction(Type, PacketId);

        public void Use() => game.DoStringEx("PlayerPackage:UseItem(" + Index + ");");

        public void Sell() => game.PostMessage(Address, 117);

        public uint Address { get; set; }
        public uint Class { get; set; }
        public uint PacketTyped { get; set; }
        public uint PacketId { get; set; }
        public uint DefID { get; set; }
        public string Name { get; set; }
        public uint Count { get; set; }
        public uint Index { get; set; }
        public string TypeName { get; set; }
        public uint Star { get; set; }
        public uint Line { get; set; }
        public string Type { get; set; }
        public uint MapId { get; set; } = 0xFFFFFFFF;
        public uint X { get; set; }
        public uint Y { get; set; }
        public uint InfoBase { get; set; }
        private string cleanname;

        public string ClearName
        {
            get
            {
                if (cleanname == null)
                    cleanname = Name.VietLien();
                return cleanname;
            }
        }

        public uint Lvl { get; set; }
        public int CapTruongThanhLongVan { get; set; }
        public int CapHopThanhVoHon { get; set; }
        public bool IsCoDinh { get; set; }

        public int Row => (int)(Math.Floor((decimal)Index / 5)) + 1;

        public uint Col => Index % 5 + 1;

        public int CountPoint
        {
            get
            {
                if (Star == 0 || Line == 0)
                {
                    return 0;
                }
                return (int)Lvl + ((int)Star + (int)Line) * 10;
            }
        }

        public int CardLevel
        {
            get
            {
                if (Type.Contains("Card_Icon_Jinzhuang1"))
                {
                    if (Type.EndsWith("_4") || Type.EndsWith("_6") || Type.EndsWith("_7") || Type.EndsWith("_9"))
                        return 1;
                    if (Type.EndsWith("_2") || Type.EndsWith("_3") || Type.EndsWith("_8") || Type.EndsWith("_13"))
                        return 2;
                    if (Type.EndsWith("_1") || Type.EndsWith("_14") || Type.EndsWith("_15") || Type.EndsWith("_16"))
                        return 3;
                    if (Type.EndsWith("_5") || Type.EndsWith("_10") || Type.EndsWith("_11") || Type.EndsWith("_12"))
                        return 4;
                }
                if (Type.Contains("Card_Icon_Mingju1"))
                {
                    if (Type.EndsWith("_8") || Type.EndsWith("_9") || Type.EndsWith("_11") || Type.EndsWith("_16"))
                        return 1;
                    if (Type.EndsWith("_3") || Type.EndsWith("_4") || Type.EndsWith("_10") || Type.EndsWith("_13"))
                        return 2;
                    if (Type.EndsWith("_2") || Type.EndsWith("_5") || Type.EndsWith("_7") || Type.EndsWith("_14"))
                        return 3;
                    if (Type.EndsWith("_1") || Type.EndsWith("_6") || Type.EndsWith("_12") || Type.EndsWith("_15"))
                        return 4;
                }
                if (Type.Contains("Card_Icon_Mingshi1"))
                {
                    if (Type.EndsWith("_1") || Type.EndsWith("_2") || Type.EndsWith("_4") || Type.EndsWith("_9"))
                        return 1;
                    if (Type.EndsWith("_5") || Type.EndsWith("_7") || Type.EndsWith("_8") || Type.EndsWith("_13"))
                        return 2;
                    if (Type.EndsWith("_10") || Type.EndsWith("_14") || Type.EndsWith("_15") || Type.EndsWith("_16"))
                        return 3;
                    if (Type.EndsWith("_3") || Type.EndsWith("_6") || Type.EndsWith("_11") || Type.EndsWith("_12"))
                        return 4;
                }
                if (Type.Contains("Card_Icon_Qinggong1"))
                {
                    if (Type.EndsWith("_3") || Type.EndsWith("_6") || Type.EndsWith("_10") || Type.EndsWith("_16"))
                        return 1;
                    if (Type.EndsWith("_1") || Type.EndsWith("_5") || Type.EndsWith("_12") || Type.EndsWith("_15"))
                        return 2;
                    if (Type.EndsWith("_2") || Type.EndsWith("_4") || Type.EndsWith("_8") || Type.EndsWith("_11"))
                        return 3;
                    if (Type.EndsWith("_7") || Type.EndsWith("_9") || Type.EndsWith("_13") || Type.EndsWith("_14"))
                        return 4;
                }
                if (Type.Contains("Card_Icon_Shenbing1"))
                {
                    if (Type.EndsWith("_2") || Type.EndsWith("_4") || Type.EndsWith("_6") || Type.EndsWith("_11"))
                        return 1;
                    if (Type.EndsWith("_3") || Type.EndsWith("_5") || Type.EndsWith("_10") || Type.EndsWith("_16"))
                        return 2;
                    if (Type.EndsWith("_8") || Type.EndsWith("_9") || Type.EndsWith("_13") || Type.EndsWith("_14"))
                        return 3;
                    if (Type.EndsWith("_1") || Type.EndsWith("_7") || Type.EndsWith("_12") || Type.EndsWith("_15"))
                        return 4;
                }
                if (Type.Contains("Card_Icon_Wujue1"))
                {
                    if (Type.EndsWith("_3") || Type.EndsWith("_10") || Type.EndsWith("_12") || Type.EndsWith("_14"))
                        return 1;
                    if (Type.EndsWith("_4") || Type.EndsWith("_8") || Type.EndsWith("_9") || Type.EndsWith("_13"))
                        return 2;
                    if (Type.EndsWith("_2") || Type.EndsWith("_11") || Type.EndsWith("_15") || Type.EndsWith("_16"))
                        return 3;
                    if (Type.EndsWith("_1") || Type.EndsWith("_5") || Type.EndsWith("_6") || Type.EndsWith("_7"))
                        return 4;
                }
                return -1;
            }
        }

        public bool HaveTheLuc
        {
            get
            {
                if (game.Address.GameType == 1)
                    return DiemType[50] == '1';
                else
                    return DiemType[51] == '1';
            }
        }

        public string DiemType
        {
            get
            {
                string output = "";
                for (uint i = 0; i < 2; i++)
                {
                    uint number = 0;
                    if (game.Address.GameType == 1)
                    {
                        number = game.Memory.Read(game.Memory.Read(Address + 0x14) + 0x54 + i * 4);
                    }
                    else
                    {
                        number = game.Memory.Read(game.Memory.Read(Address + 0x14) + 0x70 + i * 4);
                    }
                    string str = Convert.ToString(number, 2);
                    while (str.Length < 32)
                    {
                        str = "0" + str;
                    }
                    output += str;
                }
                return output;
            }
        }

        public uint TheLuc
        {
            get
            {
                if (!HaveTheLuc)
                    return 0;
                uint count = 0;
                for (int i = 0; i < 32; i++)
                {
                    if (DiemType[i] == '1')
                        count++;
                }
                for (int i = 52; i < 64; i++)
                {
                    if (DiemType[i] == '1')
                        count++;
                }
                if (game.Address.GameType == 1)
                    return game.Memory.Read2Byte(game.Memory.Read(Address + 0x14) + (0x6E + count * 2));
                return game.Memory.Read2Byte(game.Memory.Read(Address + 0x14) + (0x8A + count * 2));
            }
        }

        public void GiamDinh()
        {
            if (game.Memory.Read(game.Memory.Read(Address + 0x14) + 0xD).ToString("X8").EndsWith("000010") || game.Memory.Read(game.Memory.Read(Address + 0x14) + 0xD).ToString("X8").EndsWith("000011")) // 0x53000010 0x54000010 0x46000010
            {
                game.Memory.Write(game.Memory.Read(Address + 0x14) + 0xD, 50);
            }
        }
    }
}