using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace _i
{
    class TaskInfo
    {
        public uint Address;
        public uint Class;
        public uint Id;
        private Game game;

        public TaskInfo(Game game)
        {
            this.game = game;
        }

        public uint TrangThai
        {
            get;
            set;
        }

        public void SetTrangThai(int value)
        {
            game.Memory.Write(Address + 0x8, value);
        }


        public static List<TaskInfo> Enum(Game game)
        {
            List<TaskInfo> list = new List<TaskInfo>();
            uint address = game.Memory.Read(game.Address.TaskInfoBase);
            for (uint i = 0; i < 80; i++)
            {
                if (game.Memory.Read(address + 5 + i * 0x29) != 0)
                {
                    TaskInfo taskInfo = new TaskInfo(game);
                    taskInfo.Address = address + 5 + i * 0x29;
                    taskInfo.Class = game.Memory.Read(taskInfo.Address);
                    taskInfo.Id = game.Memory.Read(taskInfo.Address + 0x4);
                    taskInfo.TrangThai = game.Memory.Read(taskInfo.Address + 0x8);                    
                    if (taskInfo.Id > 0)
                    {
                        list.Add(taskInfo);
                    }
                }
            }
            return list;    
        }
    }
}
