using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace _i
{
    class Skill
    {
        public uint Address { get; set; }
        public uint Id { get; set; }
        public uint Icheckd { get; set; }
        public uint Icheckd10 { get; set; }
        public uint PacketId { get; set; }
        public uint DelayOffset { get; set; }
        
        public string Name { get; set; }
        public string Type { get; set; }

        bool use;

        public bool UsePK { get; set; }
        
        public bool Use
        {
            get
            {                
                return use;
            }
            set
            {
                use = value;
            }
        }
        public Game game;

        public Skill(Game game)
        {
            this.game = game;
        }

        public static bool IsBase(int id)
        {
            if ("-311-341-371-281-401-431-461-491-521-760-2900-3430-;".Contains("-" + id + "-"))
                return true;
            return false;
        }

        public static bool IsRage(int id)
        {
            if ("-328-358-388-298-418-448-478-508-538-778-2918-".Contains("-" + id + "-"))
                return true;
            if ("-329-359-389-299-419-449-479-509-539-779-2919-".Contains("-" + id + "-"))
                return true;
            if ("-330-360-390-300-420-450-480-510-540-780-2920-".Contains("-" + id + "-"))
                return true;
            return false;
        }

        public static bool IsHalfRage(int id)
        {
            if ("-328-358-388-298-418-448-478-508-538-778-2918-3448-".Contains("-" + id + "-"))
                return true;
            return false;
        }

        public static bool IsHalfRageBuff(int id)
        {
            if ("-329-359-389-299-419-449-479-509-539-779-2919-3449-".Contains("-" + id + "-"))
                return true;
            return false;
        }

        public static bool IsFullRage(int id)
        {
            if ("-330-360-390-300-420-450-480-510-540-780-2920-3450-".Contains("-" + id + "-"))
                return true;
            return false;
        }

        public static int ChieuQuan(int id)
        {
            if (id == 402)
                return 199;
            return 0;
        }

        public static int ChiDiem(int id)
        {
            if (id == 467)
                return 36;
            return 0;
        }

        public static uint BuffSelf(int id)
        {
            switch (id)
            {
                case 468: return 168; // thien canh chinh khi
                case 767: return 1328; // that ha anh nhat
                case 317: return 115; // no phat xung quan
                case 318: return 0xFFFFFFFF; // ly dao dai cuong
                case 439: return 0xFFFFFFFF; // ly dao dai cuong
                case 289: return 0xFFFFFFFF; // ly dao dai cuong
                case 438: return 158; // dam tieu tu nhuoc
                case 248: return 1284;
                case 2907: return 2611;
            }
            return 0;
        }

        public static int Buff(int id)
        {
            switch (id)
            {
                case 302: return 283; // di lac ho than
                case 332: return 284; // quy hoa than luc
                case 362: return 285; // nhu anh tuy hinh
                case 392: return 286; // te nguu vong nguyet   
                case 422: return 287; // toa hai bat loan
                case 452: return 288; // bach doc bat xam
                case 482: return 289; // chinh khi hao thien
                case 512: return 290; // bang co ngoc cot
                case 542: return 291; // mac thu thanh quy                                
                case 762: return 1305; // thien tang tue nguyet
                case 2902: return 2607; // tay kinh phat mach
                case 286: return 102; // la han tran
                case 316: return 114; // quy hoa huong duong
                case 346: return 124; // niem y thap bat diet
                case 436: return 154; // ngo cong tran
                case 466: return 166; // di dat dai lao
                case 526: return 189; // van the phong than     
                case 766: return 1308; // kich co linh
                case 2906: return 2610; // dong thanh thiet bich
                case 3432: return 3646;
                case 3436: return 3649;
                case 3802: return 4124;
            }
            return 0;
        }

        public static bool IsLan(int id)
        {
            if (id == 395) // that tinh tu thu
                return true;
            if (id == 374) // tam hoang sao nguyet
                return true;
            if (id == 3431)
                return true; // thái thượng vong tình
            if (id == 3447)
                return true; // thái thượng vong tình
            if (id == 3439)
                return true; // thái thượng vong tình
            return false;
        }

        public override string ToString()
        {
            string info = "Address: " + Address.ToString("X8");
            info += "\r\n";
            info += "DelayOffset: " + DelayOffset.ToString("X8");
            info += "\r\n";
            info += "Name:" + Name + "\r\n";
            info += "\r\n";
            return info;
        }

        public static uint BaseSkill = 0x4584;//lecaotri2020

        public static List<Skill> Enum(Game game)
        {
            List<Skill> skills = new List<Skill>();
            List<int> Ids = new List<int>();
            uint[] Base = game.Address.CharBase;
            Array.Resize(ref Base, Base.Length + 2);
            if (game.Address.GameType == 1)
                Base[Base.Length - 2] = BaseSkill;
            else if(game.Address.GameType == 2)
                Base[Base.Length - 2] = 0x7BC;
            else
                Base[Base.Length - 2] = 0xAD0;
            Base[Base.Length - 1] = 0x4;
            uint skillBase = game.Memory.Read(Base);
            List<uint> listAddress = new List<uint>();
            try
            {
                NextSkill(skillBase, listAddress, game);
            }
            catch { }
            foreach (uint address in listAddress)
            {
                if (game.Address.GameType == 1)
                {
                    Skill skill = new Skill(game);
                    skill.Address = address;

                    skill.Icheckd = game.Memory.Read1Byte(address + 0xd);
                    skill.Icheckd10 = game.Memory.Read1Byte(address + 0x10);
                    skill.PacketId = game.Memory.Read(address + 0x10);
                    skill.Name = game.Memory.ReadString(game.Memory.Read(address + 0x18, 8));
                    skill.Type = game.Memory.ReadString(game.Memory.Read(address + 0x18, 0xC));
                    skill.DelayOffset = game.Memory.Read(address + 0x18, 0x34) * 0xC;

                    if (skill.PacketId > 0 && skill.DelayOffset > 0 && skill.DelayOffset < 0x10000)
                    {
                        skills.Add(skill);
                    }
                }
                else
                {
                    Skill skill = new Skill(game);
                    skill.Address = address;
                    skill.PacketId = game.Memory.Read(address + 0xC);
                    skill.DelayOffset = game.Memory.Read(address + 0x10 + 0x4, 0x40) * 0xC;
                    if (skill.PacketId < 0x1000)
                    {
                        skills.Add(skill);
                    }
                }
            }
            if (skills.Count > 100)
            {
                skills.Clear();
            }
            return skills;
        }


        
        public static void NextSkill(uint address, List<uint> listAddress, Game game)
        {
            if (listAddress.Contains(address))
                return;
            if (listAddress.Count > 110)
                return;
            listAddress.Add(address);
            uint nextAddress1 = game.Memory.Read(address + 0x0);
            NextSkill(nextAddress1, listAddress, game);
            uint nextAddress2 = game.Memory.Read(address + 0x4);
            NextSkill(nextAddress2, listAddress, game);
            uint nextAddress3 = game.Memory.Read(address + 0x8);
            NextSkill(nextAddress3, listAddress, game);
        }        
    }
}
