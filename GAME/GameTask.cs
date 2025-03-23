using System.Collections.Generic;

namespace _i
{
    class GameTask
    {
        public GameTask(Game game)
        {
            this.game = game;
        }
        public int Address;
        public int Id;
        public string Name { get; set; }
        public string MucTieu;
        public int Lvl;
        public int X;
        public int Y;
        public string NPCNhanTask;
        public bool Completed { get; set; }
        public int Complete
        {
            get;
            set;
        }

        public bool IsFail
        {
            get
            {
                return Complete == 512 || Complete == 513;
            }
        }

        public string ClearName
        {
            get
            {
                return TDT.VietLien(Name);
            }
        }


        public void SetComplete()
        {
            List<TaskInfo> taskInfos = TaskInfo.Enum(game);
            foreach (TaskInfo info in taskInfos)
            {
                if (info.Id == Id)
                {
                    info.SetTrangThai(256);
                    break;
                }
            }
        }
        
        private Game game;
        public uint TaskInfoAddress
        {
            get;
            set;
        }

        public uint CountEx1
        {
            get
            {
                return game.Memory.Read2Byte(TaskInfoAddress + 0x36);
            }
        }

        public uint CountEx2
        {
            get
            {
                return game.Memory.Read2Byte(TaskInfoAddress + 0x3A);
            }
        }

        public uint CountEx3
        {
            get
            {
                return game.Memory.Read2Byte(TaskInfoAddress + 0x3E);
            }
        }

        public int Count1
        {
            get
            {
                return (int)game.Memory.Read(TaskInfoAddress + 0xD);
            }
            set
            {
                game.Memory.Write(TaskInfoAddress + 0xD, 1);
            }
        }

        public int CountAdr
        {
            get
            {
                return (int)TaskInfoAddress + 0xD;
            }
            set
            {
                game.Memory.Write(TaskInfoAddress + 0xD, 1);
            }
        }

        public int Count2
        {
            get
            {
                return (int)game.Memory.Read(TaskInfoAddress + 0xD + 0x4);
            }
            set
            {
                game.Memory.Write(TaskInfoAddress + 0xD + 0x4, 1);
            }
        }

        public int Count3
        {
            get
            {
                return (int)game.Memory.Read(TaskInfoAddress + 0xD + 0x8);
            }
            set
            {
                game.Memory.Write(TaskInfoAddress + 0xD + 0x8, 1);
            }
        }

        public int Count4
        {
            get
            {
                return (int)game.Memory.Read(TaskInfoAddress + 0xD + 0xC);
            }
            set
            {
                game.Memory.Write(TaskInfoAddress + 0xD + 0x8, 1);
            }
        }

        public int Count5
        {
            get
            {
                return (int)game.Memory.Read(TaskInfoAddress + 0xD + 0x10);
            }
            set
            {
                game.Memory.Write(TaskInfoAddress + 0xD + 0x8, 1);
            }
        }



        public static bool Have(Game game, string name)
        {
            game.LUA.OpenWindowMissionTrack();
            List<uint> listAddress = EnumTask(game.Address.TaskBase, game);
            foreach (uint address in listAddress)
            {
                GameTask task = new GameTask(game);
                task.Name = game.Memory._ReadString(address + 0xE0);
                if (task.Name.Contains(name))
                    return true;
            }
            return false;
        }

        public static List<GameTask> Enum(Game game)
        {
            //game.LUA.OpenWindowMissionTrack();
            List<uint> listAddress = EnumTask(game.Address.TaskBase, game);
            List<GameTask> list = new List<GameTask>();
            List<TaskInfo> taskInfos = TaskInfo.Enum(game);
            foreach (uint address in listAddress)
            {
                GameTask task = new GameTask(game);
                task.Address = (int)address;
                task.Name = game.Memory._ReadString(address + 0xC0).Trim();
                if (task.Name == "Quả Ngân Võ Lâm Ấn Kiếm Tiền")
                    task.Name = "Quả Ngân Võ Lâm Ấn Bang HộiBuôn Bán";
                if (task.Name == "#{CJG_101231_121}")
                    task.Name = "Cái Bang Nghi Vấn Ban Đầu";
                if (GAMEDIC.Replace.ContainsKey(task.Name))
                {
                    task.Name = GAMEDIC.Replace[task.Name];
                }
                // if (task.Name.Contains("Đèn nhà ai nấy sáng"))
                // continue;

                if (task.Name=="")
                    continue;

                task.Lvl = (int)game.Memory.Read(address + 0x18);
                task.Id = (int)game.Memory.Read(address + 0x10);
                task.X = (int)game.Memory.Read(address + 0x30);
                task.Y = (int)game.Memory.Read(address + 0x34);                 
                task.NPCNhanTask = game.Memory._ReadString(address + 0x3C);
                task.MucTieu = game.Memory._ReadString(address + 0xC8);
                if (task.Id > 0)
                {
                    list.Add(task);
                    foreach (TaskInfo info in taskInfos)
                    {
                        if (info.Id == task.Id)
                        {
                            if (info.TrangThai >= 256)
                                task.Completed = true;
                            task.Complete = (int)info.TrangThai;
                            task.TaskInfoAddress = info.Address;
                        }
                    }
                }
            }            
            return list;
        }

        public static List<uint> EnumTask(uint address, Game game)
        {
            List<uint> listAddress = new List<uint>();
            NextTask(address, listAddress, game);
            return listAddress;
        }

        public static List<uint> EnumTask(uint[] addresses, Game game)
        {
            uint address = game.Memory.Read(addresses);
            if (address > 0)
                return EnumTask(address, game);
            else
                return new List<uint>();
        }

        private static void NextTask(uint address, List<uint> listAddress, Game game)
        {
            if (listAddress.Count > 1000)
                return;
            if (!listAddress.Contains(address))
            {
                listAddress.Add(address);
                uint address1 = game.Memory.Read(address + 0x0);
                if (address1 > 0)
                    NextTask(address1, listAddress, game);
                uint address2 = game.Memory.Read(address + 0x4);
                if (address2 > 0)
                    NextTask(address2, listAddress, game);
                uint address3 = game.Memory.Read(address + 0x8);
                if (address3 > 0)
                    NextTask(address3, listAddress, game);
            }
        }
    }
}
