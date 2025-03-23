using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Windows.Forms;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace _i
{
    class Address
    {
        private string offstr;
        private uint[] offsets;
        public static Dictionary<string, Address> Dic = new Dictionary<string, Address>();

        public Address(string md5, string offsetStr)
        {
            try
            {
                
                GetOffset(md5, offsetStr);
            }
            catch
            {
            }
        }

        uint addmaytinh = 0;

        public Address(string md5, string offsetStr, uint addmaytinh)
        {
            try
            {
                this.addmaytinh = addmaytinh;
                GetOffset(md5, offsetStr);
            }
            catch
            {
            }
        }


        public bool Is3D
        {
            get
            {
                if (!Is2D && GameType == 1)
                {
                    return true;
                }
                return false;
            }
        }

        public static Address GetInstance(string md5, string offsetStr)
        {
            if (!Dic.ContainsKey(md5))
            {
                Address address = new Address(md5, offsetStr);
                Dic.Add(md5, address);
                return address;
            }
            else
            {
                return Dic[md5];
            }
        }

        public static Address GetInstance(string md5, string offsetStr, uint addmaytinh)
        {
            if (!Dic.ContainsKey(md5))
            {
                Address address = new Address(md5, offsetStr, addmaytinh);
                Dic.Add(md5, address);

                return address;
            }
            else
            {
                return Dic[md5];
            }
        }

        public static Address GetInstanceByPlayerState(string playerState, string offsetStr)
        {
            if (!Dic.ContainsKey(playerState))
            {
                Address address = new Address(playerState, offsetStr);
                Dic.Add(playerState, address);
                return address;
            }
            else
            {
                return Dic[playerState];
            }
        }

        public bool Is2D { get; set; }

        private void GetOffset(string md5, string offsetStr)
        {
            if (md5.Length == 32)
            {
                if (offsetStr.Contains(md5))
                {
                    offstr = offsetStr.Substring(offsetStr.IndexOf(md5));
                }
                else
                {
                    if (offsetStr.Contains("FFFF00000000000000000000000000000000"))
                    {
                        offstr = offsetStr.Substring(offsetStr.IndexOf("FFFF00000000000000000000000000000000"));
                    }
                    offstr = offsetStr;
                }
            }
            else
            {
                if (md5.Length == 7)
                {
                    if (offsetStr.Contains(md5))
                    {
                        int index = -1;
                        index = offsetStr.IndexOf(md5);
                        string tmp = offsetStr.Substring(0, index);
                        tmp = Regex.Replace(tmp, ".*FFFF", "");
                        index = offsetStr.IndexOf(tmp);
                        offstr = offsetStr.Substring(index);
                    }
                    else
                    {
                        if (offsetStr.Contains("FFFF00000000000000000000000000000000"))
                        {
                            offstr = offsetStr.Substring(offsetStr.IndexOf("FFFF00000000000000000000000000000000"));
                        }
                        offstr = offsetStr;
                    }
                }
            }
            offstr = Regex.Replace(this.offstr, "@.*", "");

            NextOffset();
            GameType = offsets[0];
            if (GameType == 9)
            {
                GameType = 1;
                Is2D = true;
            }

            NextOffset();
            CharBase = offsets;
            CharBase[0] = addmaytinh + CharBase[0];//lecaotri2020

            NextOffset();
            CharId = offsets[0];
            CharMenpaiPoint = offsets[1];
            CharName = offsets[2];
            CharMenpai = offsets[3];
            CharLvl = offsets[4];
            CharRage = offsets[5];
            CharGuildID = offsets[6];
            CharIsFollow = offsets[7];
            CharGuildName = offsets[8];
            CharCurPetId = offsets[9];
            CharCurHP = offsets[10];
            CharCurMP = offsets[11];
            CharExp = offsets[12];
            CharMaxHP = offsets[13];
            CharMaxMP = offsets[14];
            PetDataSize = offsets[15];
            PetId = offsets[16];
            PetCurHP = offsets[17];
            PetMaxHP = offsets[18];
            PetEnjoy = offsets[19];

            NextOffset();
            PetBase = offsets;
            ThiencobagBase[0] = PetBase[0] = addmaytinh + PetBase[0];//lecaotri2020


            NextOffset();
            CharState = offsets;
            Skill_Nv[0] = CharState[0] = addmaytinh + CharState[0];//lecaotri2020

            NextOffset();
            IsCaptcha = offsets;
            IsCaptcha[0] = addmaytinh + IsCaptcha[0];//lecaotri2020

            NextOffset();
            IsPK = offsets;
            IsPK[0] = addmaytinh + IsPK[0];//lecaotri2020

            NextOffset();
            Disconnected = offsets;
            IsCHuyenToDoiMess[0] = Disconnected[0] = addmaytinh + Disconnected[0];//lecaotri2020


            NextOffset();
            KeyId = offsets;
            KeyId[0] = addmaytinh + KeyId[0];//lecaotri2020

            NextOffset();
            Follow = offsets;
            Follow[0] = addmaytinh + Follow[0];//lecaotri2020

            NextOffset();
            IdFollow = offsets;
            IdFollow[0] = addmaytinh + IdFollow[0];//lecaotri2020

            NextOffset();
            MapId = offsets;
            MapId[0] = addmaytinh + MapId[0];//lecaotri2020
            X2[0] = MapId[0];
            IsRelive[0] = MapId[0] + 0x4;

            NextOffset();
            FakeMapId = offsets;
            FakeMapId[0] = addmaytinh + FakeMapId[0];//lecaotri2020

            NextOffset();
            MapName = offsets;
            MapName[0] = addmaytinh + MapName[0];//lecaotri2020

            NextOffset();
            FirstObject = offsets;
            FirstObject[0] = addmaytinh + FirstObject[0];//lecaotri2020

            NextOffset();
            ObjectId = offsets[0];
            ObjectObject = offsets[1];
            ObjectX = offsets[2];
            ObjectY = offsets[3];
            ObjectBuff = offsets[4];
            ObjectAtkToId = offsets[5];
            ObjectAtkById = offsets[6];
            ObjectTaiNguyenName = offsets[7];

            NextOffset();
            ObjectInfo = offsets;


            NextOffset();
            ObjectHP = offsets[0];
            ObjectMP = offsets[1];
            ObjectTrueId = offsets[2];
            ObjectBelong = offsets[3];
            ObjectName = offsets[4];
            ObjectMenpai = offsets[5];
            ObjectType = offsets[6];
            ObjectLvl = offsets[7];
            ObjectPartyId = offsets[8];
            ObjectTitle = offsets[9];
            ObjectRide = offsets[10];

            NextOffset();
            ActionBase = offsets;
            ActionBase[0] = addmaytinh + ActionBase[0];//lecaotri2020

            NextOffset();
            TaiNguyenClass = addmaytinh + offsets[0];//lecaotri2020


            NextOffset();
            PacketClass = addmaytinh + offsets[0];//lecaotri2020


            NextOffset();
            LootPacketItem = offsets;
            LootPacketItem[0] = addmaytinh + LootPacketItem[0];//lecaotri2020

            NextOffset();
            LootPacketId = offsets;
            LootPacketId[0] = addmaytinh + LootPacketId[0];//lecaotri2020

            NextOffset();
            SkillDelayBase = offsets;
            SkillDelayBase[0] = addmaytinh + SkillDelayBase[0];//lecaotri2020

            NextOffset();
            PacketItemBase = offsets;
            PacketItemBase[0] = addmaytinh + PacketItemBase[0];//lecaotri2020

            NextOffset();
            PacketType1 = addmaytinh + offsets[0];

            NextOffset();
            PacketType2 = addmaytinh + offsets[0];

            NextOffset();
            PacketType3 = addmaytinh + offsets[0];

            NextOffset();
            PacketType4 = addmaytinh + offsets[0];

            NextOffset();
            PacketType5 = addmaytinh + offsets[0];

            NextOffset();
            PacketType6 = addmaytinh + offsets[0];

            NextOffset();
            OnlineTime = offsets;
            OnlineTime[0] = addmaytinh + OnlineTime[0];//lecaotri2020

            NextOffset();
            QuestFrameBase = offsets;
            QuestFrameBase[0] = addmaytinh + QuestFrameBase[0];//lecaotri2020

            NextOffset();
            KeySkillIdBase = offsets;
            KeySkillIdBase[0] = addmaytinh + KeySkillIdBase[0];//lecaotri2020

            NextOffset();
            try
            {
                LuyenKimBase = addmaytinh + offsets[0];//lecaotri2020
                BienThan = offsets[1];
                LuyenKimOffset = offsets[2];
                LuyenKimX = offsets[3];
                if (Global.TimeLive == 0)
                    Global.TimeLive = (int)LuyenKimX;
                State = offsets[4];
            }
            catch { }

            NextOffset();
            TaskBase = offsets;
            TaskBase[0] = addmaytinh + TaskBase[0];//lecaotri2020

            NextOffset();
            TaskInfoBase = offsets;
            TaskInfoBase[0] = addmaytinh + TaskInfoBase[0];//lecaotri2020

            NextOffset();
            IsTogleMission = offsets;
            IsTogleMission[0] = addmaytinh + IsTogleMission[0];//lecaotri2020

            NextOffset();
            IsNexLogin = offsets;
            IsNexLogin[0] = addmaytinh + IsNexLogin[0];//lecaotri2020

            NextOffset();
            IsSelectServer = offsets;
            IsSelectServer[0] = addmaytinh + IsSelectServer[0];//lecaotri2020

            NextOffset();
            IsLoginMessage = offsets;
            IsLoginMessage[0] = addmaytinh + IsLoginMessage[0];//lecaotri2020

            NextOffset();
            IsLogon = offsets;
            IsLogon[0] = addmaytinh + IsLogon[0];//lecaotri2020

            NextOffset();
            IsTextCaptcha = offsets;
            IsTextCaptcha[0] = addmaytinh + IsTextCaptcha[0];//lecaotri2020

            NextOffset();
            IsSelectCharacter = offsets;
            IsSelectCharacter[0] = addmaytinh + IsSelectCharacter[0];//lecaotri2020

            NextOffset();
            Captcha = offsets;
            Captcha[0] = addmaytinh + Captcha[0];//lecaotri2020

            NextOffset();
            SafeTime = offsets;
            SafeTime[0] = addmaytinh + SafeTime[0];//lecaotri2020

            NextOffset();
            MultiAcc = addmaytinh + offsets[0];//lecaotri2020

            NextOffset();
            DisableActiveGame = addmaytinh + offsets[0];//lecaotri2020

            NextOffset();
            DisconnectAddress = addmaytinh + offsets[0];//lecaotri2020

            NextOffset();
            FuncSendKey = addmaytinh + offsets[0];//lecaotri2020

            NextOffset();
            ParaSendKey = offsets;
            ParaSendKey[0] = addmaytinh + ParaSendKey[0];//lecaotri2020

            NextOffset();
            FuncUseSkill = addmaytinh + offsets[0];//lecaotri2020


            NextOffset();
            ParaUseSkill = offsets;
            ParaUseSkill[0] = addmaytinh + ParaUseSkill[0];//lecaotri2020

            NextOffset();
            FuncUseSkillPet = addmaytinh + offsets[0];//lecaotri2020


            SkillPetBase[0] = ActionBase[0];
            ParaUseSkillPet = PetBase[0];

            NextOffset();
            FuncLuaDoString = addmaytinh + offsets[0];

            NextOffset();
            ParaLuaDoString = addmaytinh + offsets[0];

            NextOffset();
            ParaSelectTarget = ParaTalk = addmaytinh + offsets[0];

            NextOffset();
            ParaPickItem = addmaytinh + offsets[0];

            NextOffset();
            PickAll = offsets;
            PickAll[0] = addmaytinh + PickAll[0];//lecaotri2020

            NextOffset();
            FuncUpLvl = addmaytinh + offsets[0];

            NextOffset();
            FuncSelectTargetOfTarget = addmaytinh + offsets[0];

            NextOffset();
            DropBase = offsets;
            DropBase[0] = addmaytinh + DropBase[0];//lecaotri2020
            DropBase[3] = addmaytinh + DropBase[3];//lecaotri2020

            NextOffset();
            BaseShopItem = offsets;
            BaseShopItem[0] = addmaytinh + BaseShopItem[0];//lecaotri2020

            NextOffset();
            ParaCollectItem = addmaytinh + offsets[0];

            NextOffset();
            ParaLuaToString = addmaytinh + offsets[0];

            NextOffset();
            FuncSendPacket = addmaytinh + offsets[0];

            NextOffset();
            ParaSendPacket = addmaytinh + offsets[0];

            HaveRide1[0] = HaveRide2[0] = PacketItemBase[0];
            if (GameType != 1)
            {
                HaveRide1[1] = ONguyenLieu[1] = ODaoCu[1] = 0x348;
            }
            ODaoCu[0] = ONguyenLieu[0] = PacketItemBase[0];

            if (GameType == 1)
            {
                PetName = 0x24;
                PetLvl = 0x38;
            }
            else
            {
                PetName = 0x1C;
                PetLvl = 0x34;
            }
            BankBase[0] = PacketItemBase[0];


            //lecaotri
            NextOffset();
            bakePacket = offsets[0].ToString("x4") + offsets[1].ToString("x4") + offsets[2].ToString("x4") + offsets[3].ToString("x4");
            NextOffset();
            IsSelectRoleCreateA = offsets;
            IsSelectRoleCreateA[0] = addmaytinh + IsSelectRoleCreateA[0];

            NextOffset();
            BasePK = addmaytinh + offsets[0];
            NextOffsetNew();
            //lecaotri
            Exchange = addmaytinh + offsets[0];
            DeleteItemPara1 = addmaytinh + offsets[1];
            NhanKn = addmaytinh + offsets[2];
            thiencobase = addmaytinh + offsets[3];
            TaiNguyenNVClass = addmaytinh + offsets[4];
            PickitemNVBase = addmaytinh + offsets[5];
            ControlEvent = offsets[6];
            DeleteItemthienco = addmaytinh + offsets[7];
        }

        public uint BasePK;
        private void NextOffset()
        {
            offstr = offstr.Remove(0, offstr.IndexOf("00FF") + 4);

            string sub = offstr.Substring(0, offstr.IndexOf("00FF"));

            int length = sub.Length;
            bool isBase = length % 2 == 1;

            if (isBase)
            {
                offsets = new uint[length / 6];
                ParseHex(sub.Substring(0, 7), out offsets[0]);
                for (int i = 1; i < offsets.Length; i++)
                    ParseHex(sub.Substring(i * 6 + 1, 6), out offsets[i]);
            }
            else
            {
                offsets = new uint[length / 4];
                ParseHex(sub.Substring(0, 4), out offsets[0]);
                for (int i = 1; i < offsets.Length; i++)
                    ParseHex(sub.Substring(i * 4, 4), out offsets[i]);
            }
        }
        private void NextOffsetNew()
        {
            offstr = offstr.Remove(0, offstr.IndexOf("00FF") + 4);

            string sub = offstr.Substring(0, offstr.IndexOf("00FF"));

            int length = sub.Length;

            offsets = new uint[length / 7];
            ParseHex(sub.Substring(0, 7), out offsets[0]);
            for (int i = 1; i < offsets.Length; i++)
                ParseHex(sub.Substring(i *7, 7), out offsets[i]);

        }


        private void ParseHex(string hex, out int result)
        {
            int.TryParse(hex, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out result);
        }

        private void ParseHex(string hex, out uint result)
        {
            uint.TryParse(hex, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out result);
        }

        public uint[] IsTogleMission { get; set; }
        public uint[] TaskInfoBase { get; set; }
        public uint[] CharBase { get; set; } = new uint[] { 0x0, 0x0, 0x0, 0x4 };
        public uint CharId { get; set; }
        public uint CharMenpaiPoint { get; set; }
        public uint CharName { get; set; }
        public uint CharMenpai { get; set; }
        public uint CharLvl { get; set; }
        public uint CharRage { get; set; }
        public uint CharGuildID { get; set; }
        public uint CharIsFollow { get; set; }
        public uint CharCurPetId { get; set; }
        public uint CharCurHP { get; set; }
        public uint CharCurMP { get; set; }
        public uint CharExp { get; set; }
        public uint CharMaxHP { get; set; }
        public uint CharMaxMP { get; set; }
        public uint CharGuildName { get; set; }
        public uint PlayerGold { get; set; }
        public uint PlayerLockedGold { get; set; }
        public uint PlayerLockedKNB { get; set; }
        public uint PlayerKNB { get; set; }
        public uint PlayerKNBThongBao { get; set; }
        public uint PlayerDiemTang { get; set; }



        public uint PetDataSize { get; set; }
        public uint PetId { get; set; }
        public uint PetCurHP { get; set; }
        public uint PetMaxHP { get; set; }
        public uint PetEnjoy { get; set; }
        public uint DisconnectAddress { get; set; }
        public uint ObjectBuff { get; set; }
        public uint ObjectAtkToId { get; set; }
        public uint ObjectAtkById { get; set; }
        public uint ObjectTaiNguyenName { get; set; }
        public uint ParaUseSkillPet { get; set; }
        public uint FuncUseSkillPet { get; set; }
        public uint FuncUseSkill { get; set; }
        public uint[] ParaUseSkill { get; set; } = new uint[] { 0x0, 0xC, 0x0 };
        public uint ParaMove { get; set; }
        public uint[] SkillDelayBase = new uint[] { 0x0, 0x0 };
        public uint[] SkillPetDelayBase { get; set; } = new uint[] { 0x88EB00, 0x4774 };
        public uint DisableActiveGame { get; set; } = 0x06B62F4;//tri
        public uint[] SkillArr { get; set; } = new uint[] { 0x96F360, 0x70, 0x1E0, 0x4, 0x2590, 0x4 };
        public uint ParaTalk { get; set; }
        public uint SkillClass { get; set; } = 0x783218;//
        public uint TaiNguyenClass { get; set; }
        public uint TaiNguyenNVClass { get; set; }
        public uint[] OnlineTime { get; set; } = new uint[] { 0x0, 0x0 };

        public uint[] ActionBase { get; set; } = new uint[] { 0x0, 0x34 };
        public uint ActionAddress { get; set; }
        public uint ActionID { get; set; }
        public uint ActionName { get; set; }
        public uint ActionType { get; set; }
        public uint ActionPacketID { get; set; }

        public uint[] SkillPetBase { get; set; } = new uint[] { 0x88EAF0, 0x64 };/// action base
        public uint[] PacketItemBase { get; set; }
        public uint[] BankBase { get; set; } = new uint[] { 0x0, 0x1CB94 };
        public uint[] BaseShopItem { get; set; }
        public uint[] delaySkillPetBase { get; set; } = new uint[] { 0x12cd614, 0x492c, 0xc };/// action basedd [[12cd614]+492c]+0bf4+c
        public uint[] PetBase { get; set; } = new uint[] { 0x0, 0x0 };
        public uint[] CharState { get; set; }
        public uint[] IsCaptcha { get; set; } = new uint[] { 0x0, 0x0, 0xC, 0x64 };
        public uint[] IsPK { get; set; } = new uint[] { 0x0, 0x0 };
        public uint[] Disconnected { get; set; } = new uint[] { 0x0, 0x0 };
        public uint[] KeyId { get; set; } = new uint[] { 0x0, 0x0 };
        public uint[] Follow { get; set; }
        public uint[] IdFollow { get; set; }
        public uint[] MapId { get; set; } = new uint[] { 0x0, 0x0 };
        public uint[] FakeMapId { get; set; }
        public uint[] MapName { get; set; } = new uint[] { 0x0, 0x0, 0x0 };
        public uint[] FirstObject { get; set; } = new uint[] { 0x0, 0x0, 0x0 };
        public uint ObjectId { get; set; }
        public uint ObjectObject { get; set; }
        public uint ObjectX { get; set; }
        public uint ObjectY { get; set; }
        public uint[] ObjectInfo { get; set; }
        public uint ObjectHP { get; set; }
        public uint ObjectMP { get; set; }
        public uint ObjectTrueId { get; set; }
        public uint ObjectBelong { get; set; }
        public uint ObjectName { get; set; }
        public uint ObjectTitle { get; set; }
        public uint ObjectMenpai { get; set; }
        public uint ObjectType { get; set; }
        public uint ObjectLvl { get; set; }
        public uint ObjectPartyId { get; set; }
        public uint ObjectRide { get; set; }
        public uint FuncSendKey { get; set; }
        public uint[] ParaSendKey = new uint[] { 0x0, 0x40 };
        public uint ParaSelectTarget { get; set; }

        public uint Exchange { get; set; }
        public uint DeleteItemPara1 { get; set; } = 0;
        public uint DeleteItemthienco { get; set; } = 0;
        public uint PickitemNVBase { get; set; } = 0;

        public uint ControlEvent { get; set; } = 0;
        public uint thiencobase { get; set; } = 0;
        public uint NhanKn { get; set; }
        public uint ParaPickItem { get; set; }
        public uint FuncSelectTargetOfTarget { get; set; }
        public uint[] LootPacketId { get; set; }
        public uint[] PickAll { get; set; }
        public uint PacketClass { get; set; }
        public uint FuncUpLvl { get; set; }
        public uint[] KeySkillIdBase { get; set; }
        public uint MultiAcc { get; set; }
        public uint ParaLuaDoString { get; set; }
        public uint FuncLuaDoString { get; set; }
        public uint[] LootPacketItem { get; set; }
        public uint GameType { get; set; }
        public uint[] TaskBase { get; set; }
        public uint LuyenKimBase { get; set; }
        public uint BienThan { get; set; }
        public uint LuyenKimOffset { get; set; }
        public uint LuyenKimX { get; set; }
        public uint State { get; set; }
        public uint PacketType1 { get; set; }
        public uint PacketType2 { get; set; }
        public uint PacketType3 { get; set; }
        public uint PacketType4 { get; set; }
        public uint PacketType5 { get; set; }
        public uint PacketType6 { get; set; }
        public uint[] QuestFrameBase { get; set; }
        public uint[] DropBase { get; set; } = new uint[] { 0x0, 0x0, 0x0, 0x0 };
        public uint ParaCollectItem { get; set; }
        public uint[] IsSelectServer { get; set; } = new uint[] { 0x947180, 0x8, 0x4, 0x144 };

        public uint[] KetBaiInfo { get; set; } = new uint[] { Global.addmaytinh + 0x93AEB0, 0x8, 0x4, 0x144 };

        public uint[] IsLoginMessage { get; set; } = new uint[] { 0x9470D8, 0x8, 0x4, 0x144 };
        public uint[] IsCreatePlayer { get; set; } = new uint[] { 0x0, 0x8, 0x4, 0x144 };
        public uint[] IsLogon { get; set; } = new uint[] { 0x947F60, 0x8, 0x4, 0x144 };
        public uint[] IsTextCaptcha { get; set; } = new uint[] { 0x94A3A8, 0x8, 0x4, 0x144 };
        public uint[] IsSelectCharacter { get; set; } = new uint[] { 0x947048, 0x8, 0x4, 0x144 };
        public uint[] FreshmanWatchTime { get; set; } = new uint[] { 0xCA4B0, 0x7E0, 0x770, 0x0, 0xE4, 0x35C };
        public uint[] IsSelectRoleCreateA { get; set; } = new uint[] { 0xADF4B8, 0x8, 0x4, 0x144 };
        //public uint FuncLuaToString;
        public uint ParaLuaToString { get; set; }
        public uint[] SafeTime { get; set; } = new uint[] { 0x12ED924, 0xC50C0 };
        public uint[] Captcha { get; set; }
        public uint[] IsNexLogin { get; set; }
        public uint[] QuestInfo { get; set; } = new uint[] { 0x0, 0x8, 0x4, 0x144 };
        public uint[] ThongBaoInfo { get; set; } = new uint[] { 0xAE0B68, 0x28, 0x24, 0x38, 0x6E0 };
        public uint[] CountDown10Sec { get; set; } = new uint[] { 0x0, 0x8, 0x4, 0x144 };
        public uint[] IsShopOpen { get; set; } = new uint[] { 0x0, 0x8, 0x4, 0x14C };
        public uint FuncSendPacket { get; set; }
        public uint ParaSendPacket { get; set; }
        public uint[] ON_SCENE_TRANSING { get; set; } = new uint[] { 0x0, 0x8, 0x4, 0x144 };
        public uint[] HaveRide1 { get; set; } = new uint[] { 0x0, 0x1C860, 0x20 };//{ 0x0, 0xE650, 0x20 }
        public uint[] HaveRide2 { get; set; } = new uint[] { 0x0, 0x1CBE0, 0x0 };// { 0x0, 0xEA4C, 0x0 }    public uint[] HaveRide2 { get; set; } = new uint[] { 0x0, 0xDA4EC};

        public uint[] ODaoCu { get; set; } = new uint[] { 0x0, 0x1C860, 0x24, 0x8 };//{ 0x0, 0xE650, 0x24, 0x8 }//lecaotri2020
        public uint[] ONguyenLieu { get; set; } = new uint[] { 0x0, 0x1C860, 0x28, 0x8 };//1C860 v{ 0x0, 0xE650, 0x28, 0x8 }
        public uint[] TuiChanNguyenBase { get; set; } = new uint[] { Global.addmaytinh + 0xAA5EB8, 0x1C954 };

        public uint PetName { get; set; }
        public uint PetLvl { get; set; }
        public uint[] IsBankOpen { get; set; } = new uint[] { 0x0, 0x8, 0x4, 0x14C };
        public uint[] IsBigBankOpen { get; set; } = new uint[] { 0x0, 0x8, 0x4, 0x14C };

        public uint[] X2 { get; set; } = new uint[] { 0xB31394, 0x4, 0xAC, 0x258, 0x8, 0x35C };
        //public uint[] X2 { get; set; } = new uint[] { 0x0, 0x68, 0x6C8, 0x2CC, 0xE8, 0x35C };

        public uint[] IsRelive { get; set; } = new uint[] { 0x0, 0xAC, 0x6F4, 0x124, 0x710, 0x7B8 };
        public uint[] TEXTVALIDATE_SAVELOGINSELECT { get; set; } = new uint[] { 0x0, 0x0, 0x75C, 0x2E0, 0xFC, 0x64 };

        public string bakePacket { get; set; }
        public uint PlayerPouint { get; set; }

        public uint OffsetEquip { get; set; } = 0xE650;

        public uint[] ThiencobagBase { get; set; } = new uint[] { 0x0, 0x1CC30 };

        //$address .= '092FAC8' . '000040' . '000004' . '0004D4' .
        public uint[] IsCHuyenToDoiMess { get; set; } = new uint[] { 0x0, 0x40, 0x4, 0x4D4 };
        public uint[] Skill_Nv { get; set; } = new uint[] { 0x0, 0x54, 0x1B4 };

    }
}
