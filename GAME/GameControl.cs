using System;
using System.Collections.Generic;
using System.Text;

namespace _i
{
    class GameControl
    {
        public uint Address { get; set; }
        public uint Object { get; set; }
        public uint Class { get; set; }
        public uint Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public uint PacketId { get; set; }

        public bool IsSkill
        {
            get
            {
                if (Type.Contains("FightSkillXinShou_12"))
                    return false;
                if (Type.Contains("FightSkill") || Type.Contains("FabaoSkill") || Type.Contains("MiJiSkill") || Type.Contains("fuqiskill") || Type.Contains("Shoes2_5") || Type.Contains("RideHeader1_1") || Type.Contains("MenpaiLiveSkill2_7"))
                    return true;
                if (Type.Contains("WuhunSkill") && PacketId > 100)
                    return true;
                if (Type.Contains("TaskTools2_13"))
                    return true;
                if (Type.Contains("PetSkill2_4"))
                    return true;
                if (Type.Contains("CommonLiveSkill2_2"))
                    return true;
                if (Type.Contains("CircularTaskTool43_2"))
                    return true;
                if (Type.Contains("Shoes2_4"))
                    return true;
                if (Type.Contains("TaskTools4_1"))
                    return true;
                return false;
            }
        }



        public static IEnumerable<GameControl> Enum(Game game)
        {
            foreach (uint address in EnumGameControl(game.Address.ActionBase, game))
            {
                GameControl _control = new GameControl();
                _control.Address = address;
                if (game.Address.GameType != 1)
                {
                    _control.Object = game.Memory.Read(address + 0x10);
                    _control.Class = game.Memory.Read(_control.Object);
                    _control.Id = game.Memory.Read(_control.Object + 0x4);
                    _control.Name = game.Memory._ReadString(_control.Object + 0xC);
                    _control.Type = game.Memory._ReadString(_control.Object + 0x28).Trim();
                    _control.PacketId = game.Memory.Read(_control.Object + 0x5C);
                }
                else
                {
                    _control.Object = game.Memory.Read(address + 0x14);//lecaotri2020
                    _control.Class = game.Memory.Read(_control.Object);
                    _control.Id = game.Memory.Read(_control.Object + 0x4);
                    _control.Name = game.Memory._ReadString(_control.Object + 0x8);//C
                    _control.Type = game.Memory._ReadString(_control.Object + 0x20).Trim();
                    _control.PacketId = game.Memory.Read(_control.Object + 0x50);
                }
                yield return _control;
            }
        }

        private static HashSet<uint> EnumGameControl(uint address, Game game)
        {
            HashSet<uint> hash = new HashSet<uint>();
            NextGameControl(address, hash, game);
            hash.Remove(address);
            return hash;
        }

        public static HashSet<uint> EnumGameControl(uint[] addresses, Game game)
        {
            uint address = game.Memory.Read(addresses);
            if (address > 0)
                return EnumGameControl(address, game);
            else
                return new HashSet<uint>();
        }

        private static void NextGameControl(uint address, HashSet<uint> hash, Game game)
        {
            if (hash.Count > 10000)
                return;
            if (!hash.Contains(address) && address > 0)
            {
                hash.Add(address);
                NextGameControl(game.Memory.Read(address + 0x0), hash, game);
                NextGameControl(game.Memory.Read(address + 0x4), hash, game);
                NextGameControl(game.Memory.Read(address + 0x8), hash, game);
            }
        }
    }
}
