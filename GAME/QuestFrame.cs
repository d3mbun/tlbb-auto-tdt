using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace _i
{
    class QuestFrameItem
    {
        public uint Address { get; set; }
        public uint Id { get; set; }
        public uint StrOptionExtra1 { get; set; }
        public uint StrOptionExtra2 { get; set; }
        public string Name { get; set; }
    }

    class QuestFrame
    {
        public List<QuestFrameItem> Items
        {
            get
            {
                List<QuestFrameItem> list = new List<QuestFrameItem>();
                uint firstItem = game.Memory.Read(game.Address.QuestFrameBase) + 0x8 + 0x118 * 0;
                QuestFrameItem item = new QuestFrameItem();
                item.Address = firstItem;
                item.StrOptionExtra1 = game.Memory.Read(item.Address + 0x110);
                item.StrOptionExtra2 = game.Memory.Read(item.Address + 0x8);
                item.Name = game.Memory.ReadString(item.Address + 0xE).Trim();
                list.Add(item);
                for (uint i = 1; i < 17; i++)
                {
                    uint address = firstItem + 0x118 * i;
                    QuestFrameItem _item = new QuestFrameItem();
                    _item.Address = address;
                    _item.Id = i;
                    _item.StrOptionExtra1 = game.Memory.Read(_item.Address + 0x110);
                    _item.StrOptionExtra2 = game.Memory.Read(_item.Address + 0x8);
                    _item.Name = game.Memory.ReadString(address + 0xE).Trim();
                    if (_item.Name == "#{CJG_101231_121}")
                    {
                        _item.Name = "Cái Bang Nghi Vấn Ban Đầu";
                    }
                    if (GAMEDIC.Replace.ContainsKey(_item.Name))
                    {
                        _item.Name = GAMEDIC.Replace[_item.Name];
                    }
                    if (_item.Name.Trim() != string.Empty)
                        list.Add(_item);
                }
                return list;
            }
        }

        public QuestFrame(Game game)
        {
            this.game = game;
        }

        public void ClickAll()
        {
            foreach(QuestFrameItem item in Items)
            {
                game.QuestFrameOptionClicked(item);
            }
        }

        public bool Click(int option1, int option2)
        {
            foreach (QuestFrameItem item in Items)
            {
                if (item.StrOptionExtra1 == option1 && item.StrOptionExtra2 == option2)
                {
                    game.QuestFrameOptionClicked(item);
                    return true;
                }
            }
            return false;
        }

   

        public bool Click(int index)
        {
            if(index <= Items.Count - 1)
            {
                game.QuestFrameOptionClicked(Items[index]);
                return true;
            }
            return false;
        }

        public void Accept()
        {
            game.DoStringEx("QuestFrameAcceptClicked();");
        }
        public void Complete()
        {
            game.DoStringEx("QuestFrameMissionComplete(1);");
        }

        public bool Click(string name)
        {
            foreach (QuestFrameItem item in Items)
            {
                if (TDT.VietLien(item.Name).Contains(TDT.VietLien(name)))
                {
                    game.QuestFrameOptionClicked(item);
                    return true;
                }
            }
            if (game.Missions.Contains(MissionsType.NhiemVuChinhTuyen))
            {
                Thread.Sleep(1000);
                foreach (QuestFrameItem item in Items)
                {
                    if (TDT.VietLien(item.Name).Contains(TDT.VietLien(name)))
                    {
                        game.QuestFrameOptionClicked(item);
                        return true;
                    }
                }
                Thread.Sleep(1000);
                foreach (QuestFrameItem item in Items)
                {
                    if (TDT.VietLien(item.Name).Contains(TDT.VietLien(name)))
                    {
                        game.QuestFrameOptionClicked(item);
                        return true;
                    }
                }
                Thread.Sleep(1000);
                foreach (QuestFrameItem item in Items)
                {
                    if (TDT.VietLien(item.Name).Contains(TDT.VietLien(name)))
                    {
                        game.QuestFrameOptionClicked(item);
                        return true;
                    }
                }
            }
            return false;
        }

        public void Close()
        {
            game.PostMessage(0, 122);
        }

        public uint Address;
        public uint Id { get; set; }
        public uint StrOptionExtra1 { get; set; }
        public uint StrOptionExtra2 { get; set; }
        Game game;
        public string Name;

        public uint ID
        {
            get
            {
                return game.Memory.Read(game.Address.QuestFrameBase[0], 0x3C);
            }
        }

        public string MD
        {
            get
            {
                return TDT.Hasher.MD5(StrOptionExtra1.ToString() + StrOptionExtra2.ToString()).Substring(0, 3);
            }
        }

        public static void ClickPhuBanMonPhai(Game game)
        {
            foreach (QuestFrame dialog in QuestFrame.Enum(game))
            {
                if (dialog.IsClickPhuBanMonPhai)
                {
                    game.QuestFrameOptionClicked(dialog);
                    break;
                }
            }
        }

        public static void ClickOut(Game game)
        {
            foreach (QuestFrame dialog in QuestFrame.Enum(game))
            {
                if (((dialog.StrOptionExtra1 == 119001 || dialog.StrOptionExtra1 == 118001) && dialog.StrOptionExtra2 == 1) || dialog.StrOptionExtra1 == 119005 || dialog.StrOptionExtra1 == 118012 || dialog.StrOptionExtra1 == 2108)
                {
                    game.QuestFrameOptionClicked(dialog);
                    break;
                }
            }
        }

        public bool IsClickPhuBanMonPhai
        {
            get
            {
                if (StrOptionExtra1 == 200005)
                    return true;
                if (StrOptionExtra1 == 118001)
                    return true;
                if (StrOptionExtra1 == 119005)
                    return true;
                if (StrOptionExtra1 == 119001)
                    return true;
                if (StrOptionExtra1 == 200001)
                    return true;
                if (StrOptionExtra1 == 13035) // thien long
                    return true;
                if (StrOptionExtra1 == 9044) // mo dung
                    return true;
                if (StrOptionExtra1 == 10051) // duong mon
                    return true;
                if (StrOptionExtra1 == 16035) // tinh tuc
                    return true;
                if (StrOptionExtra1 == 14035) // tieu dao
                    return true;
                if (StrOptionExtra1 == 9035) // thieu lam
                    return true;
                if (StrOptionExtra1 == 17035) // thien son
                    return true;
                if (StrOptionExtra1 == 15035) // nga my
                    return true;
                if (StrOptionExtra1 == 12035) // vo dang
                    return true;
                if (StrOptionExtra1 == 11035) // minh giao
                    return true;
                if (StrOptionExtra1 == 10035) // cai bang
                    return true;
                return false;
            }
        }

        public static List<QuestFrame> Enum(Game game)
        {
            List<QuestFrame> list = new List<QuestFrame>();
            uint firstDialogItem = game.Memory.Read(game.Address.QuestFrameBase) + 0x8 + 0x118 * 0;
            QuestFrame dialog = new QuestFrame(game);
            dialog.Address = firstDialogItem;
            dialog.StrOptionExtra1 = game.Memory.Read(dialog.Address + 0x110);
            dialog.StrOptionExtra2 = game.Memory.Read(dialog.Address + 0x8);
            dialog.Name = game.Memory.ReadString(dialog.Address + 0xE);
            list.Add(dialog);
            for (uint i = 1; i < 20; i++)
            {
                uint address = firstDialogItem + 0x118 * i;
                dialog = new QuestFrame(game);
                dialog.Address = address;
                dialog.Id = i;
                dialog.StrOptionExtra1 = game.Memory.Read(dialog.Address + 0x110);
                dialog.StrOptionExtra2 = game.Memory.Read(dialog.Address + 0x8);
                dialog.Name = game.Memory.ReadString(address + 0xE).Trim();
                if(dialog.Name == "#{CJG_101231_121}")
                {
                    dialog.Name = "Cái Bang Nghi Vấn Ban Đầu";
                }
                if (GAMEDIC.Replace.ContainsKey(dialog.Name))
                {
                    dialog.Name = GAMEDIC.Replace[dialog.Name];
                }
                if (dialog.Name.Trim() != "")
                    list.Add(dialog);
            }
            return list;
        }


        public string Text
        {
            get
            {
                string s = "";
                foreach (QuestFrameItem dialog in Items)
                {
                    s += dialog.Name;
                }
                return s;
            }
        }

        public static string All(Game game)
        {
            string s = "";
            foreach (QuestFrame dialog in Enum(game))
            {
                s += dialog.Name;
            }
            return s;
        }

        public static string First(Game game)
        {
            string s = "";
            foreach (QuestFrame dialog in Enum(game))
            {
                s += dialog.Name;
                break;
            }
            return s;
        }
    }
}
