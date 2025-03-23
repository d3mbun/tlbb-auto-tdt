using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.Linq;

namespace _i
{
    class GameObjects
    {
        Game game;

        public IEnumerable<GameObject> Players => All.Where(o => o.IsPlayer);

        public IEnumerable<GameObject> LootPackets => All.Where(_object => _object.Class == game.Address.PacketClass);
        public IEnumerable<GameObject> Traps => All.Where(o => o.IsTrap);
        public List<GameObject> All = new List<GameObject>();

    

        public IEnumerable<GameObject> PartyEx
        {
            get
            {
                foreach (var o in All)
                {
                    if (o.IsPlayer)
                    {
                        if (SettingOld.HashBuff.Contains(o.Name) || game.HashBuff.Contains(o.Name))
                        {
                            yield return o;
                        }
                        if (Global.BuffQuanDoan && Self.QDId != 65535 && game.Address.GameType == 1 && Self.QDId == o.QDId)
                        {
                            yield return o;
                        }
                        if (Self.PartyId != 0xFFFFFFFF && o.PartyId == Self.PartyId)
                        {
                            yield return o;
                        }
                        if (o == Self)
                            yield return o;
                    }
                    if (game.TLBB.MapId == MAP.YenTuO && (o.CleanName == "hodienbao" || o.CleanName == "tienhoanhvu"))
                    {
                        yield return o;
                    }
                    if (game.TLBB.MapId == MAP.ThanhThuSonPhuBan && o.Title.VietLien() == "linhthu")
                    {
                        yield return o;
                    }
                }
            }
        }
        public List<GameObject> Monters = new List<GameObject>();
      
        public List<GameObject> Pk = new List<GameObject>();
        public GameObject Self;
        public GameObject Key;
        public GameObject Target;
        public GameObject PartyMinHP;

        public IEnumerable<GameObject> AllNpc => All.Where(o => o.IsNPC);

  

        public GameObject GetObject(string name)
        {
            foreach (GameObject _object in All)
            {
                if (_object.Name == name)
                    return _object;
            }
            return null;
        }

      

        public bool Have(string name)
        {
            foreach(GameObject _object in All)
            {
                if (_object.CleanName.Contains(name.VietLien()))
                    return true;
            }
            return false;
        }

        

        public bool HaveMonter(string name)
        {
            name = name.VietLien();
            foreach (GameObject _object in Monters)
            {
                if (_object.CleanName.Contains(name))
                    return true;
            }
            return false;
        }

  


        public List<GameObject> NearMonter(float distance, float x = -1, float y = -1)
        {
            if(x == -1)
            {
                x = game.CharX;
            }
            if(y == -1)
            {
                y = game.CharY;
            }
            List<GameObject> list = new List<GameObject>();
            foreach (GameObject _object in Monters)
            {                
                if (_object.GetDistance(x, y) < distance)
                    list.Add(_object);
            }
            if(game.TLBB.MapId == MAP.ViemMaSon)
            {
                foreach (GameObject _object in All)
                {
                    if (_object.GetDistance(x, y) < distance && _object.Menpai == 12 && _object.IsNPC)
                    {
                        list.Add(_object);
                    }
                }
            }
            return list;
        }

   





        public List<GameObject> NearMonter12m
        {
            get
            {
                List<GameObject> list = new List<GameObject>();
                foreach (GameObject _object in game.Objects.Monters)
                {
                    if (TDT.GetDistance(game.CharX, game.CharY, _object.X, _object.Y) < 12)
                        list.Add(_object);
                }
                return list;
            }
        }



      

        public bool TargetIsMine
        {
            get
            {
                if (Target == null || !game.TLBB.MineIds.Contains(Target.Belong))
                    return false;
                return true;
            }
        }

        public GameObjects(Game game)
        {
            this.game = game;
            Read();
        }

        public List<GameObject> PT { get; set; } = new List<GameObject>();


   



        //public gamo
        //public  Dictionary<uint, GameObject> DicObject = new Dictionary<uint, GameObject>();
        /// <summary>
        /// 
        /// </summary>
        public void Read()
        {
            try
            {
                Self = Key = Target = null;
                All.Clear();
                Pk.Clear();
                Monters.Clear();

                HashSet<uint> hash = EnumGameObject(new uint[] { (uint)game.Address.FirstObject[0], (uint)game.Address.FirstObject[1], (uint)game.Address.FirstObject[2] });
    
                foreach (uint address in hash)
                {
                    GameObject _object = new GameObject(game, address);
                    _object = _object.ReadObj();
                    if (_object.Name != null)
                    {
                        if (_object.Distance <= Setting.Value("numberTamXa") && Setting.Value("numberTamXa") > 0)
                            All.Add(_object);
                        
                        if (_object.IsPlayer)
                        {
                            foreach (string str in SettingOld.Leader.Split('\n'))
                            {
                                if (str.Trim() == "")
                                    continue;
                                if (TDT.VietLien(str) == TDT.VietLien(_object.Name) && _object.IsPlayer)
                                {
                                    if (Self != null && _object == Self)
                                        continue;
                                    Key = _object;
                                    break;
                                }
                            }
                        }
                     
                        if (_object.Id == game.TargetId)
                            Target = _object;
             
                        if (_object.TrueId == game.TLBB.Id && game.TLBB.Online)
                        {
                            Self = _object;
                            game.TLBB.PartyId = Self.PartyId;
                            game.SelfId = Self.Id;
                            game.CharX = Self.X;
                            game.CharY = Self.Y;
                        }
                    }
                }

                game.TLBB.MineIds = "0000000000000000FFFFFFFFFFFFFFFF";
                if (Self != null)
                {
                    game.TLBB.MineIds += Self.TrueId;
                    bool isAutoPk = false;
                    foreach (GameObject _object in All)
                    {
                        if (_object == Self)
                            continue;
                        if (AutoPK.NamePK != string.Empty)
                        {
                            if (AutoPK.IsPK)
                            {
                                if (AutoPK.NamePK == _object.Name)
                                {
                                    Pk.Clear();
                                    Pk.Add(_object);
                                    isAutoPk = true;
                                }
                            }
                        }
                       
                        if (!string.IsNullOrEmpty(_object.Name) && game.Enemys.ContainsKey(_object.Name) && _object.HP > 0)
                        {
                            if (!isAutoPk)
                            {                              
                                if (Global.AutoPk)
                                {
                                    if (Main.SettingForm.checkSkipMonter.Checked && SettingOld.HaveToBoQua.Contains(_object.Name))
                                    {

                                    }
                                    else
                                    {
                                        Pk.Add(_object);
                                    }
                                }
                            }
                        }
                     
                        if (Self.HP > 0.3 && Self.QDId != 65535 && game.Address.GameType == 1 && Global.BuffQuanDoan)
                        {
                            if (Self.QDId == _object.QDId && _object.IsPlayer)
                            {
                                if (_object.HP > 0)
                                {
                                    game.TLBB.MineIds += _object.TrueId;
                                }
                            }
                        }
                        else if (Self.PartyId != 0xFFFFFFFF && _object.PartyId == Self.PartyId)
                        {
                            if (_object.HP > 0)
                            {
                                game.TLBB.MineIds += _object.TrueId;
                                if (_object.TrueId == game.TLBB.KeyId && Key == null)
                                    Key = _object;
                            }
                        }
                        if (_object.IsMonter)
                        {
                            Monters.Add(_object);
                        }                        
                    }
                }
                PartyMinHP = null;
                if (Self != null && Self.HP < 0.3)
                {
                    PartyMinHP = Self;
                }
                else
                {
                    PartyMinHP = PartyEx.Where(o => o.HP > 0 && !o.Buff.Contains(4638) && o.HP * 100 <= Global.BuffNMPercent && o.Distance < 15).OrderBy(o => o.HP).ThenByDescending(o => !game.AllPartyTargetId.Contains(o.Id)).FirstOrDefault();
                }
                if (Global.BuffPet)
                {
                    if(PartyMinHP == null)
                    {
                        PartyMinHP = All.Where(o => o.HP > 0 && o.IsPet && o.HP * 100 <= Global.BuffNMPercent && o.Distance < 15).OrderBy(o => o.HP).FirstOrDefault();
                    }
                }
            }
            catch { }
        }


        public HashSet<uint> EnumGameObject(uint address)
        {
            HashSet<uint> hash = new HashSet<uint>();
            NextGameObject(address, hash);
            return hash;
        }

        public HashSet<uint> EnumGameObject(uint[] addresses)
        {
            uint address = game.Memory.Read(addresses);
            if (address > 0)
                return EnumGameObject(address);
            else
                return new HashSet<uint>();
        }

        private void NextGameObject(uint address, HashSet<uint> hash)
        {
            if (address <= 0)
                return;
            //kiemtraloi
            //if (game.Memory.Read1Byte(address + 0x15) == 1)
            //    return;
            if (hash.Count > 5000)
            {
                return;
            }
            if (!hash.Contains(address))
            {
                hash.Add(address);                
                //int address1 = game.Memory.Read(address + 0x0);
                //if (address1 > 0)
                    NextGameObject(game.Memory.Read(address + 0x0), hash);
                NextGameObject(game.Memory.Read(address + 0x4), hash);
                //int address2 = game.Memory.Read(address + 0x8);
                //if (address2 > 0)
                NextGameObject(game.Memory.Read(address + 0x8), hash);
                //int address3 = game.Memory.Read(address + 0x4);
                //if (address3 > 0)
                //    NextGameObject(address3, hash);
            }
        }
    }
}
