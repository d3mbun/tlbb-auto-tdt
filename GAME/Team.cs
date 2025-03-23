using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _i
{
    class TeamMem
    {
        public uint Address { get; set; }
        public string TrueId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string MapName { get; set; } = string.Empty;
        public uint Index { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public bool IsLeader { get; set; }
    }

    class Team
    {
        public Game Game { get; set; }

        public List<TeamMem> All { get; set; } = new List<TeamMem>();

        public Team(Game game)
        {
            Game = game;
        }

        public int Count { get; set; }

        public void Read()
        {
            All.Clear();
            uint baseAdr = Game.Memory.Read(new uint[] { Game.Address.KeyId[0], 0xDC });
            for(uint i = 0;i< Count; i++)
            {
                TeamMem mem = new TeamMem();
                mem.Address = Game.Memory.Read(baseAdr + i * 4);
                //Main.TDTLog(mem.Address.ToString("X8"));
                mem.TrueId = Game.Memory.Read8Byte(mem.Address).ToString("X8");
                mem.Name = Game.Memory.ReadString(mem.Address + 0x40);
                mem.X = Game.Memory.ReadFloat(mem.Address + 0x70);
                mem.Y = Game.Memory.ReadFloat(mem.Address + 0x74);
                All.Add(mem);
            }
        }
    }
}
