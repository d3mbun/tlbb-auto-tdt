using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;
using System.Linq;
using System.Collections.Specialized;

namespace _i
{
    class SettingOld
    {
        //public static List<NameValueCollection<>>
        //DataContractJsonSerializer json 

        public static IniParser MapIni = new IniParser(Properties.Resources.mapini);
        public static string Str = string.Empty;

        public static HashSet<string> hashBuff;
        public static HashSet<string> HashBuff
        {
            get
            {
                if (hashBuff == null)
                {
                    hashBuff = new HashSet<string>(Game.IniParser.Read("Buff", "All").Split('\n').ToList().Select(s => s.Trim()).Where(s => s.Length > 1));                  
                }
                return hashBuff;
            }
            set
            {
                hashBuff = value;
            }
        }

        public static string idBuff;

        public static string IdBienThan
        {
            get
            {
                return Game.IniParser.Read("Buff", "BienThan");
            }
            set
            {
                Game.IniParser.Write("Buff", "BienThan", value);
            }
        }

        public static string IdBuff
        {
            get
            {
                if(idBuff == null)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (string name in HashBuff)
                    {
                        sb.AppendLine(name +  "-");
                    }
                    idBuff = sb.ToString();
                }
                return idBuff;
            }
            set
            {
                HashBuff = new HashSet<string>();
                string buff = value;
                StringBuilder sb = new StringBuilder();
                foreach (string b in buff.Split('\n'))
                {
                    string name = b.Trim();
                    if (!name.Contains("-") && !name.Contains(" ") && !string.IsNullOrEmpty(name))
                    {
                        if (!HashBuff.Contains(name))
                        {
                            HashBuff.Add(name);
                            sb.AppendLine(name);
                        }
                    }
                }
                idBuff = sb.ToString();
                Game.IniParser.Write("Buff", "All", idBuff);
                //return sb.ToString();
                //Settings.Instance["IdsBuff"] = value;
            }
        }

        public static HashSet<string> HaveToBoQua = new HashSet<string>();
        public static HashSet<string> HaveToOnlyAttack = new HashSet<string>();
        public static HashSet<string> haveToBuy;

        public static string sellitemps;
        public static string SellItemPS
        {
            get
            {
                if(sellitemps == null)
                {
                    sellitemps = Game.IniParser.Read("Buff", "SellItemPS");
                }
                return sellitemps;
            }
            set
            {
                sellitemps = value;
                Game.IniParser.Write("Buff", "SellItemPS", value);
            }
        }


        public static string buyits;

        public static string BuyIts
        {
            get
            {
                if (buyits == null)
                {
                    buyits = Game.IniParser.Read("Buy", "All");
                }
                return buyits;
            }
            set
            {
                buyits = value;
                Game.IniParser.Write("Buy", "All", buyits);
                haveToBuy = null;
            }
        }

        public static void SetBoQua()
        {
            HaveToBoQua = new HashSet<string>(BoQua.Split('\n').ToList().Select(s => s.Trim()).Where(s => s.Length > 1));
            HaveToOnlyAttack = new HashSet<string>(OnlyAttack.Split('\n').ToList().Select(s => s.Trim()).Where(s => s.Length > 1));
            HaveToPKName = new HashSet<string>(PkName.Split('\n').ToList().Select(s => s.Trim()).Where(s => s.Length > 1));
            HaveToPKIgnore = new HashSet<string>(PkIgnore.Split('\n').ToList().Select(s => s.Trim()).Where(s => s.Length > 1));
            HaveToPKGuild = new HashSet<string>(PkGuild.Split('\n').ToList().Select(s => s.Trim()).Where(s => s.Length > 1));
        }

        static string boqua;

        public static string BoQua
        {
            get
            {
                //return Settings.Instance["BoQua"];
                if(boqua == null)
                {
                    boqua = Game.IniParser.Read("Item", "BoQua");
                }
                return boqua;
            }
            set
            {
                boqua = value;
                Game.IniParser.Write("Item", "BoQua", boqua);
                //Settings.Instance["BoQua"] = value;
            }
        }

        static string boquaex;
        public static string BoQuaEx
        {
            get
            {
                //return Settings.Instance["BoQua"];
                if (boquaex == null)
                {
                    boquaex = Game.IniParser.Read("Item", "BoQuaEx");
                }
                return boquaex;
            }
            set
            {
                boquaex = value;
                Game.IniParser.Write("Item", "BoQuaEx", boquaex);
                //Settings.Instance["BoQua"] = value;
            }
        }

        static string onlybossmap;
        public static HashSet<string> HashOnlyBossMap { get; set; } = new HashSet<string>();
        public static string OnlyBossMap
        {
            get
            {
                //return Settings.Instance["BoQua"];
                if (onlybossmap == null)
                {
                    onlybossmap = Game.IniParser.Read("BossMAP", "Only");

                    foreach(string mon in onlybossmap.Split('\n'))
                    {
                        string m = mon.Trim();
                        if (!HashOnlyBossMap.Contains(m))
                        {
                            HashOnlyBossMap.Add(m);
                        }
                    }

                }
                return onlybossmap;
            }
            set
            {
                onlybossmap = value;
                HashOnlyBossMap.Clear();
                foreach (string mon in onlybossmap.Split('\n'))
                {
                    string m = mon.Trim();
                    if (!HashOnlyBossMap.Contains(m))
                    {
                        HashOnlyBossMap.Add(m);
                    }
                }

                Game.IniParser.Write("BossMAP", "Only", onlybossmap);

                //Settings.Instance["BoQua"] = value;
            }
        }




      


        public static string onlyAttack;
        public static string OnlyAttack
        {
            get
            {
                if (onlyAttack == null)
                {
                    onlyAttack = Game.IniParser.Read("Item", "OnlyAttack");
                }
                return onlyAttack;
            }
            set
            {
                onlyAttack = value;
                Game.IniParser.Write("Item", "OnlyAttack", onlyAttack);
            }
        }

        public static string dropName;
        public static string DropName
        {
            get
            {
                if (dropName == null)
                {
                    dropName = Game.IniParser.Read("Item", "DropName");
                }
                return dropName;                
            }
            set
            {
                dropName = value;
                Game.IniParser.Write("Item", "DropName", dropName);
            }
        }
        public static string dropType;
        public static string DropType
        {
            get
            {
                if (dropType == null)
                {
                    dropType = Game.IniParser.Read("Item", "DropType");
                }
                return dropType;
            }
            set
            {
                dropType = value;
                Game.IniParser.Write("Item", "DropType", dropType);
            }
        }

        public static string catThienCoName;
        public static string CatThienCoName
        {
            get
            {
                if (catThienCoName == null)
                {
                    catThienCoName = Game.IniParser.Read("Item", "CatThienCoName");
                }
                return catThienCoName;
            }
            set
            {
                catThienCoName = value;
                Game.IniParser.Write("Item", "CatThienCoName", catThienCoName);
            }
        }
        public static string catThienCoType;
        public static string CatThienCoType
        {
            get
            {
                if (catThienCoType == null)
                {
                    catThienCoType = Game.IniParser.Read("Item", "CatThienCoType");
                }
                return catThienCoType;
            }
            set
            {
                catThienCoType = value;
                Game.IniParser.Write("Item", "CatThienCoType", catThienCoType);
            }
        }



        public static string catName;
        public static string CatName
        {
            get
            {
                if (catName == null)
                {
                    catName = Game.IniParser.Read("Item", "BankName");
                }
                return catName;
            }
            set
            {
                catName = value;
                Game.IniParser.Write("Item", "BankName", catName);
            }
        }
        public static string catType;
        public static string CatType
        {
            get
            {
                if (catType == null)
                {
                    catType = Game.IniParser.Read("Item", "BankType");
                }
                return catType;
            }
            set
            {
                catType = value;
                Game.IniParser.Write("Item", "BankType", catType);
            }
        }


        public static string layName;
        public static string LayName
        {
            get
            {
                if (layName == null)
                {
                    layName = Game.IniParser.Read("Item", "LayName");
                }
                return layName;
            }
            set
            {
                layName = value;
                Game.IniParser.Write("Item", "LayName", layName);
            }
        }
        public static string layType;
        public static string LayType
        {
            get
            {
                if (layType == null)
                {
                    layType = Game.IniParser.Read("Item", "LayType");
                }
                return layType;
            }
            set
            {
                layType = value;
                Game.IniParser.Write("Item", "LayType", layType);
            }
        }

        public static string useName;
        public static string UseName
        {
            get
            {
                if (useName == null)
                {
                    useName = Game.IniParser.Read("Item", "UseName");
                }
                return useName;
            }
            set
            {
                useName = value;
                Game.IniParser.Write("Item", "UseName", useName);
            }
        }
        public static string useType;
        public static string UseType
        {
            get
            {
                if (useType == null)
                {
                    useType = Game.IniParser.Read("Item", "UseType");
                }
                return useType;
            }
            set
            {
                useType = value;
                Game.IniParser.Write("Item", "UseType", useType);
            }
        }

        public static string layThienCoName;
        public static string LayThienCoName
        {
            get
            {
                if (layThienCoName == null)
                {
                    layThienCoName = Game.IniParser.Read("Item", "LayThienCoName");
                }
                return layThienCoName;
            }
            set
            {
                layThienCoName = value;
                Game.IniParser.Write("Item", "LayThienCoName", layThienCoName);
            }
        }

        public static string layThienCoType;
        public static string LayThienCoType
        {
            get
            {
                if (layThienCoType == null)
                {
                    layThienCoType = Game.IniParser.Read("Item", "LayThienCoType");
                }
                return layThienCoType;
            }
            set
            {
                layThienCoType = value;
                Game.IniParser.Write("Item", "LayThienCoType", layThienCoType);
            }
        }

        public static string gomKNBName;
        public static string GomKNBName
        {
            get
            {
                if (gomKNBName == null)
                {
                    gomKNBName = Game.IniParser.Read("Item", "GomKNBName");
                }
                return gomKNBName;
            }
            set
            {
                gomKNBName = value;
                Game.IniParser.Write("Item", "GomKNBName", gomKNBName);
            }
        }

        public static string gomKNBType;
        public static string GomKNBType
        {
            get
            {
                if (gomKNBType == null)
                {
                    gomKNBType = Game.IniParser.Read("Item", "GomKNBType");
                }
                return gomKNBType;
            }
            set
            {
                gomKNBType = value;
                Game.IniParser.Write("Item", "GomKNBType", gomKNBType);
            }
        }

        public static string gomName;
        public static string GomName
        {
            get
            {
                if (gomName == null)
                {
                    gomName = Game.IniParser.Read("Item", "GomName");
                }
                return gomName;
            }
            set
            {
                gomName = value;
                Game.IniParser.Write("Item", "GomName", gomName);
            }
        }
        public static string gomTye;
        public static string GomType
        {
            get
            {
                if (gomTye == null)
                {
                    gomTye = Game.IniParser.Read("Item", "GomType");
                }
                return gomTye;
            }
            set
            {
                gomTye = value;
                Game.IniParser.Write("Item", "GomType", gomTye);
            }
        }


        public static HashSet<string> haveToSellName;
        public static HashSet<string> haveToSellType;
        public static HashSet<string> HaveToSellName
        {
            get
            {
                if(haveToSellName == null)
                {
                    haveToSellName = new HashSet<string>();
                    foreach(string s in SellName.Split('\n'))
                    {
                        string name = s.Trim();
                        if (string.IsNullOrEmpty(name.Trim()))
                            continue;
                        if (!haveToSellName.Contains(name))
                        {
                            haveToSellName.Add(name);
                        }
                    }
                }
                return haveToSellName;
            }
        }
        public static HashSet<string> HaveToSellType
        {
            get
            {
                if (haveToSellType == null)
                {
                    haveToSellType = new HashSet<string>();
                    foreach (string s in SellType.Split('\n'))
                    {
                        string name = s.Trim();
                        if (string.IsNullOrEmpty(name.Trim()))
                            continue;
                        if (!haveToSellType.Contains(name))
                        {
                            haveToSellType.Add(name);
                        }
                    }
                }
                return haveToSellType;
            }
        }

        public static string sellName;
        public static string SellName
        {
            get
            {
                if (sellName == null)
                {
                    sellName = Game.IniParser.Read("Item", "SellName");
                }
                return sellName;
            }
            set
            {
                sellName = value;
                Game.IniParser.Write("Item", "SellName", sellName);
                haveToSellName = null;
            }
        }
        public static string sellType;
        public static string SellType
        {
            get
            {
                if (sellType == null)
                {                   
                    sellType = Game.IniParser.Read("Item", "SellType");
                }
                return sellType;
            }
            set
            {
                sellType = value;
                Game.IniParser.Write("Item", "SellType", sellType);
                haveToSellType = null;
            }
        }
        static string selldinhsan;
        public static string SellDinhSan
        {
            get
            {
                if (selldinhsan == null)
                    selldinhsan = Game.IniParser.Read("Item", "SellDinhSan");
                return selldinhsan;
            }
            set
            {
                selldinhsan = value;
                Game.IniParser.Write("Item", "SellDinhSan", value);
            }
        }
        static string dropdinhsan;
        public static string DropDinhSan
        {
            get
            {
                if (dropdinhsan == null)
                    dropdinhsan = Game.IniParser.Read("Item", "DropDinhSan");
                return dropdinhsan;
            }
            set
            {
                dropdinhsan = value;
                Game.IniParser.Write("Item", "DropDinhSan", value);
            }
        }

        public static string pkName;
        public static string PkName
        {
            get
            {
                if (pkName == null)
                {
                    pkName = Game.IniParser.Read("Item", "PkName");
                }
                return pkName;
            }
            set
            {
                pkName = value;
                Game.IniParser.Write("Item", "PkName", pkName);
            }
        }

        public static HashSet<string> HaveToPKName = new HashSet<string>();

        //public static List<string> ListPkName
        //{
        //    get
        //    {
        //        List<string> pk = new List<string>();
        //        foreach (string s in PkName.Split('\n'))
        //        {
        //            pk.Add(TDT.VietLien(s.Trim()));
        //        }
        //        return pk;
        //    }
        //}
        public static string pkGuild;
        public static string PkGuild
        {
            get
            {
                if (pkGuild == null)
                {
                    pkGuild = Game.IniParser.Read("Item", "PkGuild");
                }
                return pkGuild;
            }
            set
            {
                pkGuild = value;
                Game.IniParser.Write("Item", "PkGuild", pkGuild);
            }
        }

        public static HashSet<string> HaveToPKGuild = new HashSet<string>();

        //public static List<string> ListPkGuild
        //{
        //    get
        //    {
        //        List<string> pk = new List<string>();
        //        foreach (string s in PkGuild.Split('\n'))
        //        {
        //            pk.Add(TDT.VietLien(s.Trim()));
        //        }
        //        return pk;
        //    }
        //}
        public static string pkIgnore;
        public static string PkIgnore
        {
            get
            {
                if (pkIgnore == null)
                {
                    pkIgnore = Game.IniParser.Read("Item", "PkIgnore");
                }
                return pkIgnore;
            }
            set
            {
                pkIgnore = value;
                Game.IniParser.Write("Item", "PkIgnore", pkIgnore);
            }
        }

        public static HashSet<string> HaveToPKIgnore = new HashSet<string>();

        //public static List<string> ListPkIgnore
        //{
        //    get
        //    {
        //        List<string> pk = new List<string>();
        //        foreach (string s in PkIgnore.Split('\n'))
        //        {
        //            pk.Add(TDT.VietLien(s.Trim()));
        //        }
        //        return pk;
        //    }
        //}

        static string leader;

        public static string Leader
        {
            get
            {
                if (leader == null)
                {                 
                    leader = Game.IniParser.Read("Item", "Leader");
                }
                return leader;
            }
            set
            {
                leader = value;
                Game.IniParser.Write("Item", "Leader", leader);
            }
        }




        static string logininfo;

        public static string LoginInfo
        {
            get
            {
                if (logininfo == null)
                {
                    logininfo = Game.IniParser.Read("Item", "LoginInfo");
                }
                return logininfo;
            }
            set
            {
                logininfo = value;
                Game.IniParser.Write("Item", "LoginInfo", logininfo);
            }
        }

        public static void Write(string section, string key, string value)
        {
            Game.IniParser.Write(section, key, value);
        }

        public static string Read(string section, string key)
        {
            return Game.IniParser.Read(section, key);
        }

        public static int[] LoadSetting(string name)
        {
            try
            {
                name = name.ToUpper();
                if (!Str.Contains("@" + name + ":"))
                {
                    return new int[1];
                }
                name = Str.Substring(Str.IndexOf("@" + name + ":") + ("@" + name + ":").Length);
                name = name.Substring(0, name.IndexOf("#"));
                //name = Regex.Replace(Str, ".*@" + name + ":", "");
                //name = Regex.Replace(name, "#.*", "");

                return String2Arr(name);
            }
            catch
            {
                return new int[] { 0 };
            }
        }



        public static void SaveMAP(string name, string value)
        {
            //MapIni.Write("MAP", name, value);
            Game.IniParser.Write("MAP", name, value);
        }        

        public static string LoadMAP(string name)
        {
            string val = Game.IniParser.Read("MAP", name);
            if (val == string.Empty)
                val = MapIni.Read("MAP", name);

            return val;
        }
        public static void SaveBossMAP(string name, string value)
        {
            //MapIni.Write("MAP", name, value);
            Game.IniParser.Write("BossMap", name, value);
        }
        public static string LoadBossMAP(string name)
        {
            string val = Game.IniParser.Read("BossMAP", name);
            if (val == string.Empty)
                val = MapIni.Read("BossMap", name);

            return val;
        }

        public static void SaveWAY(string name, string value)
        {
            Game.IniParser.Write("WAY", name, value);
        }

        public static string LoadWAY(string name)
        {
            name = Game.IniParser.Read("WAY", name);
            return name;
        }

        public static int[] String2Arr(string str)
        {
            string[] sets = str.Split(',');
            int[] arr = new int[sets.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = String2Int(sets[i]);
            }
            return arr;
        }

        public static int String2Int(string input)
        {
            int value = 0;
            int.TryParse(input, out value);
            return value;
        }


        public static void SetNull()
        {
            catThienCoName = catThienCoType = null;
            hashBuff = null;
            idBuff = null;
            haveToBuy = null;
            buyits = null;
            boqua = null;
            boquaex = null;
            onlybossmap = null;            
            onlyAttack = null;
            dropName = null;
            dropType = null;
            catName = null;
            catType = null;
            layName = null;
            layType = null;
            useName = null;
            useType = null;
            layThienCoName = null;
            layThienCoType = null;
            gomKNBName = null;
            gomKNBType = null;
            gomName = null;
            gomTye = null;
            haveToSellName = null;
            haveToSellType = null;
            sellName = null;
            sellType = null;
            pkName = null;
            pkGuild = null;
            pkIgnore = null;
            leader = null;
            logininfo = null;
        }
    }
}
