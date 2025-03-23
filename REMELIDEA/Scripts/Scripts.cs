using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Xml;

namespace _i
{
    class Scripts
    {
        public static XmlDocument XML = new XmlDocument();

        public static string Path
        {
            get
            {
                return Global.APPPath + "\\Config" + "\\Scripts.xml";
            }
        }

        public static List<Script> All
        {
            get;
            set;
        }

        public static List<Script> Load()
        {
            try
            {
                XML.Load("https://tieudattai.org/microauto/ScriptExs.php?email=" + HttpUtility.UrlEncode(User.Email) + "&pass=" + User.Pass);
                //XML.LoadXml(Properties.Resources.Scripts);
            }
            catch
            {
                if (XML.SelectSingleNode("/*") == null)
                {
                    XML.InsertBefore(XML.CreateXmlDeclaration("1.0", "UTF-8", null), XML.DocumentElement);
                    XML.AppendChild(XML.CreateNode(XmlNodeType.Element, "Scripts", ""));
                    //XML.Save(Path);
                }
            }
            List<Script> scripts = new List<Script>();
            foreach (XmlNode node in Scripts.XML.SelectSingleNode("Scripts").SelectNodes("Script"))
            {
                Script script = new Script();
                script.Node = node;
                scripts.Add(script);
            }
            All = scripts;
            return scripts;
        }      

        public static Script Get(string id)
        {
            foreach(Script script in All)
            {
                if (script.ID == id || script.MD.Contains(id))
                    return script;
            }
            return null;
        }

        public static Script GetByName(string id)
        {
            foreach (Script script in All)
            {
                if (TDT.VietLien(script.Name) == TDT.VietLien(id))
                {
                    //if(script.Level > )
                    return script;
                }
                //if (TDT.VietLien(id) == "chantuong")
                //{
                //    if(TDT.VietLien(script.Name) == TDT.VietLien(id))
                //    {
                //        //if(script.Level > )
                //        return script;
                //    }
                //}
                //else
                //{
                //    if (script.Name.Contains(id) || TDT.VietLien(script.Name) == TDT.VietLien(id))
                //        return script;
                //}
            }
            return null;
        }

        public static string Add(Script script)
        {
            foreach (XmlNode node in Scripts.XML.SelectSingleNode("Scripts").SelectNodes("Script"))
            {
                Script scr = new Script();
                scr.Node = node;
                if (scr.MD.Contains(script.MD) || (scr.ID == script.ID && script.ID != ""))
                    return "Script Đã Tồn Tại";
            }
            XmlNode root = Scripts.XML.SelectSingleNode("/*");
            //XmlElement script = Scripts.XML.CreateElement("Scripts");
            root.AppendChild(script.Node);
            Save();
            return "";            
        }  

        public static void Save()
        {
            return;
           // XML.Save(Path);
        }

        public static void Remove(Script script)
        {
            XmlNode root = Scripts.XML.SelectSingleNode("/*");
            root.RemoveChild(script.Node);
            //XML.Save(Path);
        }
    }

    class Script
    {
        public XmlNode Node
        {
            get;
            set;
        }

        public Script()
        {
            Node = Scripts.XML.CreateElement("Script");
        }

        public Script(XmlNode node)
        {
            Node = node;
        }

        public string ID
        {
            get
            {
                try
                {
                    return Node.Attributes["ID"].Value;
                }
                catch { }
                return "";
            }
            set
            {
                try
                {
                    Node.Attributes["ID"].Value = value;
                }
                catch
                {
                    XmlAttribute atb = Scripts.XML.CreateAttribute("ID");
                    atb.Value = value;
                    Node.Attributes.Append(atb);
                }
            }
        }

        public string Level
        {
            get
            {
                if(Name.Contains("Thiên sư kỳ đãi"))
                {
                    try
                    {
                        return (TDT.ParseInt(Node.Attributes["Level"].Value) + 5).ToString();
                    }
                    catch { }
                    return "";
                }
                try
                {
                    return Node.Attributes["Level"].Value;
                }
                catch { }
                return "";
            }
            set
            {
                try
                {
                    Node.Attributes["Level"].Value = value;
                }
                catch
                {
                    XmlAttribute atb = Scripts.XML.CreateAttribute("Level");
                    atb.Value = value;
                    Node.Attributes.Append(atb);
                }
            }
        }

        public string InfoEx
        {
            get
            {
                try
                {
                    return Node.Attributes["InfoEx"].Value;
                }
                catch { }
                return "";
            }
            set
            {
                try
                {
                    Node.Attributes["InfoEx"].Value = value;
                }
                catch
                {
                    XmlAttribute atb = Scripts.XML.CreateAttribute("InfoEx");
                    atb.Value = value;
                    Node.Attributes.Append(atb);
                }
            }
        }

        public string MD
        {
            get
            {
                try
                {
                    return Node.Attributes["MD"].Value;
                }
                catch { }
                return "";
            }
            set
            {
                try
                {
                    Node.Attributes["MD"].Value = value;
                }
                catch
                {
                    XmlAttribute atb = Scripts.XML.CreateAttribute("MD");
                    atb.Value = value;
                    Node.Attributes.Append(atb);
                }
            }
        }

        public string Monter
        {
            get
            {
                if (Completed)
                    return "[Completed]";
                if (IsBienThan)
                    return "[BienThan]";
                if (IsComplete)
                    return "[IsComplete]";
                if (IsCollect != "0")
                    return "[IsCollect]";
                if (IsUseItem)
                    return "[UseItem]";
                try
                {
                    return Node.Attributes["Monter"].Value;
                }
                catch { }
                return "";
            }
            set
            {

                try
                {
                    Node.Attributes["Monter"].Value = value;
                }
                catch
                {
                    XmlAttribute atb = Scripts.XML.CreateAttribute("Monter");
                    atb.Value = value;
                    Node.Attributes.Append(atb);
                }
            }
        }

        public string NameEx
        {
            get
            {
                try
                {
                    string name = Info.Split(':')[0];
                    name = TDT.ClearSign(name).Replace(" ", "");
                    return name;
                }
                catch
                {
                    return "";
                }
            }
        }

        public string Name
        {
            get
            {
                try
                {
                    return Node.Attributes["Name"].Value;
                }
                catch { }
                XmlAttribute atb = Scripts.XML.CreateAttribute("Name");
                atb.Value = "";
                Node.Attributes.Append(atb);
                return "";
            }
            set
            {
                try
                {
                    Node.Attributes["Name"].Value = value;
                }
                catch
                {
                    XmlAttribute atb = Scripts.XML.CreateAttribute("Name");
                    atb.Value = value;
                    Node.Attributes.Append(atb);
                }
            }
        }

        public string Recv
        {
            get
            {
                try
                {
                    return Node.Attributes["Recv"].Value;
                }
                catch { }
                return "";
            }
            set
            {
                try
                {
                    Node.Attributes["Recv"].Value = value;
                }
                catch
                {
                    XmlAttribute atb = Scripts.XML.CreateAttribute("Recv");
                    atb.Value = value;
                    Node.Attributes.Append(atb);
                }
            }
        }

        public NPC RecvNPC
        {
            get
            {
                NPC npc = new NPC();
                if (Recv.Length == 12)
                {
                    npc.Id = (uint)Memory.Hex2Int(Recv.Substring(0, 3));
                    npc.X = Memory.Hex2Int(Recv.Substring(3, 3));
                    npc.Y = Memory.Hex2Int(Recv.Substring(6, 3));
                    npc.Map = Memory.Hex2Int(Recv.Substring(9, 3));
                    if (npc.Map == 0)
                        npc.Map = LACDUONG.Id;
                }
                return npc;
            }
        }

        public NPC AtkNPC1
        {
            get
            {
                NPC npc = new NPC();
                if (Do.Length >= 18)
                {
                    npc.MD = Do.Substring(6, 3);
                    npc.X = Memory.Hex2Int(Do.Substring(9, 3));
                    npc.Y = Memory.Hex2Int(Do.Substring(12, 3));
                    npc.Map = Memory.Hex2Int(Do.Substring(15, 3));
                    if (npc.Map == 0)
                        npc.Map = LACDUONG.Id;
                }
                return npc;
            }
        }

        public NPC AtkNPC2
        {
            get
            {
                NPC npc = new NPC();
                if (Do.Length >= 30)
                {
                    npc.MD = Do.Substring(18, 3);
                    npc.X = Memory.Hex2Int(Do.Substring(21, 3));
                    npc.Y = Memory.Hex2Int(Do.Substring(24, 3));
                    npc.Map = Memory.Hex2Int(Do.Substring(27, 3));
                    if (npc.Map == 0)
                        npc.Map = LACDUONG.Id;
                }
                return npc;
            }
        }

        public NPC AtkNPC3
        {
            get
            {
                NPC npc = new NPC();
                if (Do.Length >= 42)
                {
                    npc.MD = Do.Substring(30, 3);
                    npc.X = Memory.Hex2Int(Do.Substring(33, 3));
                    npc.Y = Memory.Hex2Int(Do.Substring(36, 3));
                    npc.Map = Memory.Hex2Int(Do.Substring(39, 3));
                    if (npc.Map == 0)
                        npc.Map = LACDUONG.Id;
                }
                return npc;
            }
        }

        public bool AtkAny
        {
            get
            {
                if (Do.Length >= 43)
                {
                    return Do[42] == '1';
                }
                return false;
            }
        }

        public string IsCollect
        {
            get
            {
                if (Do.Length >= 44)
                {
                    return Do[43].ToString();
                }
                return "0";
            }
            set
            {
                if (Do.Length >= 44)
                {
                    StringBuilder builder = new StringBuilder(Do);
                    builder[43] = value.ToString()[0];
                    Do = builder.ToString();
                }
            }
        }
        public bool IsComplete
        {
            get
            {
                if (Do.Length >= 45)
                {
                    return Do[44] == '1';
                }
                return false;
            }
            set
            {
                if (Do.Length >= 45)
                {
                    StringBuilder builder = new StringBuilder(Do);
                    if (value == true)
                        builder[44] = '1';
                    else
                        builder[44] = '0';
                    Do = builder.ToString();
                }
            }
        }        

        public bool Completed
        {
            get
            {
                if (Do.Length >= 46)
                {
                    return Do[45] == '1';
                }
                return false;
            }
            set
            {
                if (Do.Length >= 46)
                {
                    StringBuilder builder = new StringBuilder(Do);
                    if (value == true)
                        builder[45] = '1';
                    else
                        builder[45] = '0';
                    Do = builder.ToString();
                }
            }
        }

        public bool IsUseItem
        {
            get
            {
                if (Do.Length >= 47)
                {
                    return Do[46] == '1';
                }
                return false;
            }
            set
            {
                if (Do.Length >= 47)
                {
                    StringBuilder builder = new StringBuilder(Do);
                    if (value == true)
                        builder[46] = '1';
                    else
                        builder[46] = '0';
                    Do = builder.ToString();
                }
            }
        }

        public int CompleteNoi
        {
            get
            {
                if (Do.Length >= 48)
                {
                    return TDT.ParseInt(Do[47].ToString()) - 1;
                }
                return -1;
            }
            set
            {
                if(Do.Length >= 48)
                {
                    StringBuilder builder = new StringBuilder(Do);
                    builder[47] = value.ToString()[0];
                    Do = builder.ToString();
                }
            }
        }

        public int CompleteNgoai
        {
            get
            {
                if (Do.Length >= 49)
                {
                    return TDT.ParseInt( Do[48].ToString()) - 1;
                }
                return -1;
            }
            set
            {
                if (Do.Length >= 49)
                {
                    StringBuilder builder = new StringBuilder(Do);
                    builder[48] = value.ToString()[0];
                    Do = builder.ToString();
                }
            }
        }

        public bool IsPick
        {
            get
            {
                if (Do.Length >= 50)
                {
                    return Do[49] == '1';
                }
                return false;
            }
            set
            {
                if (Do.Length >= 50)
                {
                    StringBuilder builder = new StringBuilder(Do);
                    if (value == true)
                        builder[49] = '1';
                    else
                        builder[49] = '0';
                    Do = builder.ToString();
                }
            }
        }

        public bool IsThuThap
        {
            get
            {
                if (Do.Length >= 51)
                {
                    return Do[50] == '1';
                }
                return false;
            }
            set
            {
                if (Do.Length >= 51)
                {
                    StringBuilder builder = new StringBuilder(Do);
                    if (value == true)
                        builder[50] = '1';
                    else
                        builder[50] = '0';
                    Do = builder.ToString();
                }
            }
        }

        public bool IsNeedBenThan
        {
            get
            {
                try
                {
                    return Node.Attributes["BienThan"].Value == "True";
                }
                catch { }
                return false;
            }
            set
            {
                try
                {
                    Node.Attributes["BienThan"].Value = value.ToString();
                }
                catch
                {
                    XmlAttribute atb = Scripts.XML.CreateAttribute("BienThan");
                    atb.Value = value.ToString();
                    Node.Attributes.Append(atb);
                }
            }
        }

        public bool IsBienThan
        {
            get
            {
                if (Do.Length >= 52)
                {
                    return Do[51] == '1';
                }
                return false;
            }
            set
            {
                if (Do.Length >= 52)
                {
                    StringBuilder builder = new StringBuilder(Do);
                    if (value == true)
                        builder[51] = '1';
                    else
                        builder[51] = '0';
                    Do = builder.ToString();
                }
            }
        }

        public string Send
        {
            get
            {
                try
                {
                    return Node.Attributes["Send"].Value;
                }
                catch { }
                return "";
            }
            set
            {
                try
                {
                    Node.Attributes["Send"].Value = value;
                }
                catch
                {
                    XmlAttribute atb = Scripts.XML.CreateAttribute("Send");
                    atb.Value = value;
                    Node.Attributes.Append(atb);
                }
            }
        }

        public NPC SendNPC
        {
            get
            {
                NPC npc = new NPC();
                if (Send.Length == 12)
                {
                    npc.Id = (uint)Memory.Hex2Int(Send.Substring(0, 3));
                    npc.X = Memory.Hex2Int(Send.Substring(3, 3));
                    npc.Y = Memory.Hex2Int(Send.Substring(6, 3));
                    npc.Map = Memory.Hex2Int(Send.Substring(9, 3));
                    if (npc.Map == 0)
                        npc.Map = LACDUONG.Id;
                }
                return npc;
            }
        }

        public string SendClickMD
        {
            get
            {
                if(Do.Length >= 6)
                {
                    return Do.Substring(3, 3);
                }
                else
                {
                    return "000";
                }
            }
        }

        //public bool IsClickExacly(string md)
        //{
        //    if (SendClickMD == md || RecvClickMD == md)
        //        return true;
        //    return false;
        //}

        public bool IsClickExacly(QuestFrame dialog)
        {
            if (dialog.Name.Trim() == string.Empty)
                return false;
            if (SendClickMD == dialog.MD || RecvClickMD == dialog.MD)
                return true;
            if (Name.Contains(dialog.Name) || TDT.VietLien(Name) == TDT.VietLien(dialog.Name))
                return true;
            if (Name == "Quả Ngân Võ Lâm Ấn Bang HộiBuôn Bán" && dialog.Name == "Quả Ngân Võ Lâm Ấn Kiếm Tiền")
                return true;
            if (Name == "#{YD_100806_58}")
                return true;
            return false;
            //Quả Ngân Võ Lâm Ấn Kiếm Tiền
        }

        public string RecvClickMD
        {
            get
            {
                if (Do.Length >= 3)
                {
                    return Do.Substring(0, 3);
                }
                else
                {
                    return "000";
                }
            }
        }

        public string Do
        {
            get
            {
                try
                {
                    return Node.Attributes["Do"].Value;
                }
                catch { }
                return "";
            }
            set
            {
                try
                {
                    Node.Attributes["Do"].Value = value;
                }
                catch
                {
                    XmlAttribute atb = Scripts.XML.CreateAttribute("Do");
                    atb.Value = value;
                    Node.Attributes.Append(atb);
                }
            }
        }

        public string Info
        {
            get
            {
                try
                {
                    return Node.Attributes["Info"].Value;
                }
                catch { }
                return "";
            }
            set
            {
                try
                {
                    Node.Attributes["Info"].Value = value;
                }
                catch
                {
                    XmlAttribute atb = Scripts.XML.CreateAttribute("Info");
                    atb.Value = value;
                    Node.Attributes.Append(atb);
                }
            }
        }
    }
}
