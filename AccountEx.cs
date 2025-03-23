using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace _i
{
    class AccountEx
    {
        public bool Loged { get; set; }


        public string AcBa { get; set; }

        public XmlDocument XML = new XmlDocument();
        public Stopwatch swExitTime;
        public bool IsNhanHoaHong { get; set; }

        public Stopwatch CreateTime = Stopwatch.StartNew();
        public bool IsGomKNB { get; set; }
        public void SetNull()
        {
            AcBa = null;

            IsLoginDua = false;
            IsLoginTrungAc = false;
            IsLoginTiemNangTan = false;
            IsLoginNhiemVuThangCap = false;
            IsLoginNhiemVuExp = false;

            IsLoginAcTac = false;
            IsLoginThieuThatSon = false;
            IsLoginLauLanTamBao = false;

            IsLoginLeBao = false;
            IsNhanBienThan = false;
            IsLoginBoiThuong = false;
         

            IsNhanBong = false;
            IsThoiBong = false;
            IsSelectRole = false;
            IsEnterRole = false;
            IsForceOpen = false;
            Created = false;
            IsSave = false;
           
            IsSaveName = false;
            IsGetAn = false;
            Entered = false;
            IsGomDo = false;
            IsGomDoKNB = false;
            IsNhanQuaBuiHoaHong = false;
            IsKetNghiaNhanBong = false;
            IsSuDoNhanBong = false;
            IsOnline = false;
            IsBachHoaDuyen = false;
           
            IsLoginMuaNgua = false;
         
            IsTrongHoa = false;
            IsNhanTrongBon = false;
            IsBonHoa = false;
            IsDoiHoa = false;
            IsNhanMam = false;
            IsNhanHoaHong = false;            
                  
            IsNemBienThan = false;

        }
        public string Key { get; set; }
        public bool IsOnline { get; set; }
        public AccountEx(string user, string pass, string NPH, string server, string tail, string name, string id, XmlDocument XML, string key = null)
        {
            this.XML = XML;
            Key = key;      
            XmlElement node = XML.CreateElement("Account");
            if (Key == null)
            {
                node = XML.CreateElement("Account" + Main.ProfileName.Replace("#", ""));
            }
            XML.SelectSingleNode("/*").AppendChild(node);
            Node = node;
            User = user;
            Pass = pass;
            Tail = tail;
            Name = name;
            Ids = id;
            this.NPH = NPH;
            this.Server = server;
        }

        public AccountEx(XmlElement node, XmlDocument XML, string key = null)
        {
            this.XML = XML;
            Key = key;
            Node = node;
        }

        public AccountEx(XmlDocument XML, string key = null)
        {
            this.XML = XML;
            Key = key;
        }

        public string User
        {
            get
            {
                XmlAttribute atribute = Node.Attributes["User"];
                if (atribute == null)
                {
                    atribute = Node.OwnerDocument.CreateAttribute("User");
                    Node.Attributes.Append(atribute);
                }
                return atribute.Value;
            }
            set
            {
                XmlAttribute atribute = Node.Attributes["User"];
                if (atribute == null)
                {
                    atribute = Node.OwnerDocument.CreateAttribute("User");
                    Node.Attributes.Append(atribute);
                }
                atribute.Value = value;
                if (Item != null)
                {
                    Item.SubItems[1].Text = value;
                    fulltext = null;
                }
            }
        }

        string fulltext = null;

        public string FullText
        {
            get
            {
                try
                {
                    if (fulltext == null)
                    {
                        fulltext = string.Join("", new[] { User, Name, Server, LoginIndex, Status, Lvl, CountMam, CountShit, CountBut, Note }).VietLien();
                        //fulltext = TDT.VietLien(User + Name + Server + LoginIndex + Status + Lvl + CountMam + CountShit + CountBut + Note);
                        //fulltext = TDT.VietLien(new StringBuilder().Append(User).Append(Name).Append(Server).Append(LoginIndex).Append(Status).Append(Lvl).Append(CountMam).Append(CountShit).Append(CountBut).Append(Note).ToString());
                    }
                    return fulltext;
                }
                catch
                {
                    return "";
                }
            }
        }

        public ListViewItem Item;

        public string Pass
        {
            get
            {
                try
                {
                    XmlAttribute xa = Node.Attributes["Pass"];
                    if (xa == null)
                    {
                        xa = XML.CreateAttribute("Pass");
                        Node.Attributes.Append(xa);
                    }
                    if (Node.SelectSingleNode("Pass") != null)
                    {
                        xa.Value = Node.SelectSingleNode("Pass").InnerText;
                        Node.RemoveChild(Node.SelectSingleNode("Pass"));
                    }
                    if (Key == null)
                        return xa.Value;
                    return TDT.Hasher.Decrypt(xa.Value, Key);
                }
                catch
                {
                    return "";
                }
            }
            set
            {
                try
                {
                    XmlAttribute xa = Node.Attributes["Pass"];
                    if (xa == null)
                    {
                        xa = XML.CreateAttribute("Pass");
                        Node.Attributes.Append(xa);
                    }
                    if (Key != null)
                        xa.Value = TDT.Hasher.Encrypt(value, Key);
                    else
                        xa.Value = value;
                }
                catch { }
            }
        }
        public string NPH
        {
            get
            {
                try
                {
                    XmlAttribute xa = Node.Attributes["NPH"];
                    if (xa == null)
                    {
                        xa = XML.CreateAttribute("NPH");
                        Node.Attributes.Append(xa);
                    }
                    if (Node.SelectSingleNode("NPH") != null)
                    {
                        xa.Value = Node.SelectSingleNode("NPH").InnerText;
                        Node.RemoveChild(Node.SelectSingleNode("NPH"));
                    }
                    return xa.Value;
                }
                catch
                {
                    return "";
                }
            }
            set
            {
                try
                {
                    XmlAttribute xa = Node.Attributes["NPH"];
                    if (xa == null)
                    {
                        xa = XML.CreateAttribute("NPH");
                        Node.Attributes.Append(xa);
                    }
                    xa.Value = value;
                }
                catch { }
            }
        }
        public string Server
        {
            get
            {
                XmlAttribute xa = Node.Attributes["Server"];
                if (xa == null)
                {
                    xa = Node.OwnerDocument.CreateAttribute("Server");
                    Node.Attributes.Append(xa);
                }
                if (Node.SelectSingleNode("Server") != null)
                {
                    xa.Value = Node.SelectSingleNode("Server").InnerText;
                    Node.RemoveChild(Node.SelectSingleNode("Server"));
                }
                if (xa.Value.Contains("Giao Long 08") || xa.Value.Contains("Tuyết Nguyên"))
                {
                    xa.Value = "Bạch Hổ";
                }
                if (xa.Value.Contains("Thiên Long 02") || xa.Value.Contains("Thiên Long 03"))
                {
                    xa.Value = "Chu Tước";
                }
                if (xa.Value.Contains("Thiên Long 07") || xa.Value.Contains("Thiên Long 09") || xa.Value.Contains("Thiên Long 10"))
                {
                    xa.Value = "Huyền Vũ";
                }
                if (xa.Value.Contains("Hỏa Diệm") || xa.Value.Contains("Long Vũ Môn") || xa.Value.Contains("Hàn Băng Miên Chưởng"))
                {
                    xa.Value = "Thanh Long";
                }
                return xa.Value;
            }
            set
            {
                XmlAttribute atribute = Node.Attributes["Server"];
                if (atribute == null)
                {
                    atribute = Node.OwnerDocument.CreateAttribute("Server");
                    Node.Attributes.Append(atribute);
                }
                atribute.Value = value;
                if (Item != null)
                {
                    Item.SubItems[3].Text = value;
                    fulltext = null;
                }
            }
        }
        public string Tail
        {
            get
            {
                try
                {
                    XmlAttribute xa = Node.Attributes["Tail"];
                    if (xa == null)
                    {
                        xa = XML.CreateAttribute("Tail");
                        Node.Attributes.Append(xa);
                    }
                    if (Node.SelectSingleNode("Tail") != null)
                    {
                        xa.Value = Node.SelectSingleNode("Tail").InnerText;
                        Node.RemoveChild(Node.SelectSingleNode("Tail"));
                    }
                    return xa.Value;
                }
                catch
                {
                    return "";
                }
            }
            set
            {
                try
                {
                    XmlAttribute xa = Node.Attributes["Tail"];
                    if (xa == null)
                    {
                        xa = XML.CreateAttribute("Tail");
                        Node.Attributes.Append(xa);
                    }
                    xa.Value = value;
                }
                catch { }
            }
        }
        public string Name
        {
            get
            {
                XmlAttribute atribute = Node.Attributes["Name"];
                if (atribute == null)
                {
                    atribute = Node.OwnerDocument.CreateAttribute("Name");
                    Node.Attributes.Append(atribute);
                }
                if (string.IsNullOrEmpty(atribute.Value))
                    return ",,,";
                return atribute.Value;
            }
            set
            {
                XmlAttribute atribute = Node.Attributes["Name"];
                if (atribute == null)
                {
                    atribute = Node.OwnerDocument.CreateAttribute("Name");
                    Node.Attributes.Append(atribute);
                }
                atribute.Value = value;
                if (Item != null)
                {
                    Item.SubItems[2].Text = value;
                    fulltext = null;
                }
                if (ItemEx != null)
                {
                    ItemEx.SubItems[1].Text = value;
                }
            }
        }
        public string Ids
        {
            get
            {
                XmlAttribute atribute = Node.Attributes["Ids"];
                if (atribute == null)
                {
                    atribute = Node.OwnerDocument.CreateAttribute("Ids");
                    Node.Attributes.Append(atribute);
                }
                return atribute.Value;
            }
            set
            {
                XmlAttribute atribute = Node.Attributes["Ids"];
                if (atribute == null)
                {
                    atribute = Node.OwnerDocument.CreateAttribute("Ids");
                    Node.Attributes.Append(atribute);
                }
                atribute.Value = value;
            }
        }

        public string TeamInfo
        {
            get
            {
                XmlAttribute atribute = Node.Attributes["Team"];
                if (atribute == null)
                {
                    atribute = Node.OwnerDocument.CreateAttribute("Team");
                    Node.Attributes.Append(atribute);
                }
                return atribute.Value;
            }
            set
            {
                XmlAttribute atribute = Node.Attributes["Team"];
                if (atribute == null)
                {
                    atribute = Node.OwnerDocument.CreateAttribute("Team");
                    Node.Attributes.Append(atribute);
                }
                atribute.Value = value;
                if (Item != null)
                {
                    Item.SubItems[10].Text = Note;
                    fulltext = null;
                }
            }
        }

        public string Tag
        {
            get
            {
                XmlAttribute atribute = Node.Attributes["Tag"];
                if (atribute == null)
                {
                    atribute = Node.OwnerDocument.CreateAttribute("Tag");
                    Node.Attributes.Append(atribute);
                }
                return atribute.Value;
            }
            set
            {
                XmlAttribute atribute = Node.Attributes["Tag"];
                if (atribute == null)
                {
                    atribute = Node.OwnerDocument.CreateAttribute("Tag");
                    Node.Attributes.Append(atribute);
                }
                atribute.Value = value;
                if (Item != null)
                {
                    Item.SubItems[10].Text = Note;
                    fulltext = null;

                }
            }
        }

        public string Note
        {
            get
            {
                if (toadotronghoa != 1)
                {
                    return ("H" + toadotronghoa + "[" + TeamInfo + "]," + "[" + TLBB.GetMenpaiName(Menpai) + "]," + Tag).Trim(',');
                }
                return ("[" + TeamInfo + "]," + "[" + TLBB.GetMenpaiName(Menpai) + "]," + Tag).Trim(',');
            }
        }

        public string Menpai
        {
            get
            {
                XmlAttribute atribute = Node.Attributes["Menpai"];
                if (atribute == null)
                {
                    atribute = Node.OwnerDocument.CreateAttribute("Menpai");
                    Node.Attributes.Append(atribute);
                }
                return atribute.Value;
            }
            set
            {
                XmlAttribute atribute = Node.Attributes["Menpai"];
                if (atribute == null)
                {
                    atribute = Node.OwnerDocument.CreateAttribute("Menpai");
                    Node.Attributes.Append(atribute);
                }
                atribute.Value = value;
                if (Item != null)
                {
                    Item.SubItems[10].Text = Note;
                    fulltext = null;
                }
            }
        }

        public string Lvl
        {
            get
            {
                XmlAttribute atribute = Node.Attributes["Lvl"];
                if (atribute == null)
                {
                    atribute = Node.OwnerDocument.CreateAttribute("Lvl");
                    Node.Attributes.Append(atribute);
                }
                return atribute.Value;
            }
            set
            {
                XmlAttribute atribute = Node.Attributes["Lvl"];
                if (atribute == null)
                {
                    atribute = Node.OwnerDocument.CreateAttribute("Lvl");
                    Node.Attributes.Append(atribute);
                }
                atribute.Value = value;
                if (Item != null)
                {
                    Item.SubItems[6].Text = value;
                    fulltext = null;
                }
            }
        }

        public bool IsSelectRole = false;
        public bool IsEnterRole = false;
        public int IsSelect = 5;
        public Stopwatch LogonTime
        {
            get;
            set;
        }
        public bool IsForceOpen
        {
            get;
            set;
        }
        public Stopwatch SelectRoleTime = Stopwatch.StartNew();

        public int ServerIndex
        {
            get
            {
                try
                {
                    for (int i = 0; i < Publisher.XML.SelectSingleNode("//Publishers/Publisher[@Name=\"" + NPH + "\"]").SelectSingleNode("Servers").SelectNodes("Server").Count; i++)
                    {
                        if (Publisher.XML.SelectSingleNode("//Publishers/Publisher[@Name=\"" + NPH + "\"]").SelectSingleNode("Servers").SelectNodes("Server")[i].InnerText == Server)
                            return i;
                    }
                }
                catch { }
                return -1;
            }
        }
        public int TailIndex
        {
            get
            {
                try
                {
                    for (int i = 0; i < Publisher.XML.SelectSingleNode("//Publishers/Publisher[@Name=\"" + NPH + "\"]").SelectSingleNode("Tails").SelectNodes("Tail").Count; i++)
                    {
                        if (Publisher.XML.SelectSingleNode("//Publishers/Publisher[@Name=\"" + NPH + "\"]").SelectSingleNode("Tails").SelectNodes("Tail")[i].InnerText == Server)
                            return i;
                    }
                }
                catch { }
                return -1;
            }
        }

        public bool IsSave
        {
            get;
            set;
        }





        public string LoginIndex
        {
            get
            {
                XmlAttribute atribute = Node.Attributes["LoginIndex"];
                if (atribute == null)
                {
                    atribute = Node.OwnerDocument.CreateAttribute("LoginIndex");
                    Node.Attributes.Append(atribute);
                }
                return atribute.Value;
            }
            set
            {
                XmlAttribute atribute = Node.Attributes["LoginIndex"];
                if (atribute == null)
                {
                    atribute = Node.OwnerDocument.CreateAttribute("LoginIndex");
                    Node.Attributes.Append(atribute);
                }
                atribute.Value = value;
                if (Item != null)
                {
                    Item.SubItems[4].Text = value;
                    fulltext = null;
                }
            }
        }

        public bool IsSaveName
        {
            get;
            set;
        }

        public string CountMam
        {
            get
            {
                XmlAttribute atribute = Node.Attributes["TienHoa"];
                if (atribute == null)
                {
                    atribute = Node.OwnerDocument.CreateAttribute("TienHoa");
                    Node.Attributes.Append(atribute);
                }
                if (string.IsNullOrEmpty(atribute.Value))
                    return "0";
                return atribute.Value;
            }
            set
            {
                XmlAttribute atribute = Node.Attributes["TienHoa"];
                if (atribute == null)
                {
                    atribute = Node.OwnerDocument.CreateAttribute("TienHoa");
                    Node.Attributes.Append(atribute);
                }
                atribute.Value = value;
                if (Item != null)
                {
                    Item.SubItems[7].Text = value;
                    fulltext = null;
                }
            }
        }

        public string CountShit
        {
            get
            {
                XmlAttribute atribute = Node.Attributes["Shit"];
                if (atribute == null)
                {
                    atribute = Node.OwnerDocument.CreateAttribute("Shit");
                    Node.Attributes.Append(atribute);
                }
                if (string.IsNullOrEmpty(atribute.Value))
                    return "0";
                return atribute.Value;
            }
            set
            {
                XmlAttribute atribute = Node.Attributes["Shit"];
                if (atribute == null)
                {
                    atribute = Node.OwnerDocument.CreateAttribute("Shit");
                    Node.Attributes.Append(atribute);
                }
                atribute.Value = value;
                if (Item != null)
                {
                    Item.SubItems[8].Text = value;
                    fulltext = null;
                }
            }
        }


        public string CountBut
        {
            get
            {
                XmlAttribute atribute = Node.Attributes["But"];
                if (atribute == null)
                {
                    atribute = Node.OwnerDocument.CreateAttribute("But");
                    Node.Attributes.Append(atribute);
                }
                if (string.IsNullOrEmpty(atribute.Value))
                    return "0";
                return atribute.Value;
            }
            set
            {
                XmlAttribute atribute = Node.Attributes["But"];
                if (atribute == null)
                {
                    atribute = Node.OwnerDocument.CreateAttribute("But");
                    Node.Attributes.Append(atribute);
                }
                atribute.Value = value;
                if (Item != null)
                {
                    Item.SubItems[9].Text = value;
                    fulltext = null;
                }
            }
        }

        public string Path
        {
            get
            {
                try
                {
                    return Publisher.XML.SelectSingleNode("//Publishers/Publisher[@Name=\"" + NPH + "\"]").Attributes["Path"].Value;
                }
                catch { }
                return "";
            }
        }

        //public Stopwatch LogonTime
        //{
        //    get;
        //    set;
        //}

        //public EnterCaptchaLogin EnterCap;

        public bool IsCaptcha
        {
            get
            {
                if (game == null)
                    return false;
                if (!game.TLBB.IsSelectRole)
                    return false;
                if (game.TLBB.IsTextCaptcha)
                    return true;
                else
                    return false;
            }
        }

        public XmlNode Node;
        public Game game;
        public bool IsGetAn = false;
        public bool Entered;
        public int SelectCount = 0;
        public int LoginMessageTime;
        private string status;
        public string Status
        {

            get
            {
                if (status == null)
                {
                    XmlAttribute atribute = Node.Attributes["Status"];
                    if (atribute == null)
                    {
                        atribute = Node.OwnerDocument.CreateAttribute("Status");
                        Node.Attributes.Append(atribute);
                        atribute.Value = "...";
                    }
                    else
                    {
                        if (!atribute.Value.Contains("xong BHD") && !atribute.Value.Contains("Exit") && !atribute.Value.Contains("Captcha"))
                        {
                            atribute.Value = "";
                        }
                    }
                    status = atribute.Value;
                }
                if (string.IsNullOrEmpty(status))
                    return "...";
                return status;
            }
            set
            {
                status = value;
                if (oldvalue != value)
                {
                    oldvalue = value;
                    if (value.Contains("xong"))
                    {
                        MicroLogin.CountXongBHD++;
                        if (MicroLogin.swXongBHD == null)
                        {
                            MicroLogin.swXongBHD = Stopwatch.StartNew();
                        }
                    }
                }
                XmlAttribute atribute = Node.Attributes["Status"];
                if (atribute == null)
                {
                    atribute = Node.OwnerDocument.CreateAttribute("Status");
                    Node.Attributes.Append(atribute);
                }
                atribute.Value = value;
                if (Item != null)
                {
                    Item.SubItems[5].Text = value;
                    fulltext = null;
                }
                if (ItemEx != null)
                {
                    ItemEx.SubItems[2].Text = value;
                }
            }
        }
        public ListViewItem ItemEx { get; set; }
        public string oldvalue;

        public Stopwatch OpenGameTime
        {
            get;
            set;
        }
        public bool Online
        {
            get
            {
                if (game == null)
                    return false;
                else
                {
                    if (game.TLBB.IsSelectServer || game.TLBB.IsNexLogin || game.TLBB.IsLogon || game.TLBB.IsSelectRole)
                        return false;
                    return game.TLBB.Online;
                }
            }
        }


        public bool IsBachHoaDuyen { get; set; } 
        public bool IsTrongHoa = false;
        public bool IsNhanMam = false;
        public bool IsLoginTrungAc { get; set; }

        //lecaotri
        public bool IsLoginDua { get; set; } 

        public bool IsLoginTiemNangTan { get; set; }
        public bool IsLoginNhiemVuThangCap { get; set; }
        public bool IsLoginNhiemVuExp { get; set; }
        public bool IsLoginLeBao { get; set; }
        public bool IsNhanBienThan { get; set; }
        public bool IsLoginBoiThuong { get; set; }
        public bool IsLoginAcTac { get; set; }
        public bool IsLoginThieuThatSon{ get; set; }
        public bool IsLoginLauLanTamBao { get; set; }
        public bool IsLoginMuaNgua { get; set; }
        public bool IsGomDo { get; set; }
        public bool IsGomDoKNB { get; set; }
        public bool IsNhanQuaBuiHoaHong { get; set; }
        public bool IsNhanBong { get; set; }
        public bool IsThoiBong { get; set; }
        public bool IsSuDoNhanBong { get; set; }
        public bool IsKetNghiaNhanBong { get; set; }
        public bool IsNhanTrongBon { get; set; }
        public bool IsNemBienThan { get; set; }
        public bool IsBonHoa { get; set; }
        public bool IsDoiHoa { get; set; }
        public bool Created { get; set; }

        int toadotronghoa = 1;

        public int ToaDoTrongHoa
        {
            get
            {
                return toadotronghoa;
            }
            set
            {

                toadotronghoa = value;
                if (Item != null)
                {
                    Item.SubItems[10].Text = Note;
                    fulltext = null;

                }
            }
        }

        public void Answer(string txt)
        {
            if (game != null)
            {
                game.DoStringEx("DataPool:SendLoginCode(" + txt + ")");
            }
        }

        public int BaseImg { get; set; }
        public bool IsThreadRun = false;

        public void ReadBaseImgEx()
        {
            IsThreadRun = true;
            new Thread(new ThreadStart(ReadBaseImgThreadEx))
            {
                IsBackground = true
            }.Start();
        }

        public void ReadBaseImgThreadEx()
        {
            try
            {
                //BaseImg = game.Memory.GetModuleAddress("UI_CEGUI.dll");
                //BaseImg = game.Memory.Read(new int[] { BaseImg + game.Address.Captcha[0], game.Address.Captcha[1], game.Address.Captcha[2], game.Address.Captcha[3] });
                BaseImg = (int)game.AOB.SearchPrivateRegion(new byte[] { 0x01, 0x00, 0x00, 0x00, 0xFF, 0xA0, 0xFF, 0xA0, 0xFF, 0xA0, 0xFF, 0xA0, 0xFF, 0xA0, 0xFF, 0xA0, 0xFF, 0xA0, 0xFF, 0xA0, 0xFF, 0xA0, 0xFF, 0xA0, 0xFF, 0xA0, 0xFF, 0xA0 }, (uint)BaseImg, 0x7FFFFFFF) + 0x4;
            }
            catch { }
            IsThreadRun = false;
        }


        public void ReadBaseImg()
        {
            IsThreadRun = true;
            new Thread(new ThreadStart(ReadBaseImgThread))
            {
                IsBackground = true
            }.Start();
        }

        public void ReadBaseImgThread()
        {
            try
            {
                //BaseImg = game.Memory.GetModuleAddress("UI_CEGUI.dll");
                //BaseImg = game.Memory.Read(new int[] { BaseImg + game.Address.Captcha[0], game.Address.Captcha[1], game.Address.Captcha[2], game.Address.Captcha[3] });
                if (BaseImg == 0)
                {
                    BaseImg = (int)game.AOB.SearchPrivateRegion(new byte[] { 0x01, 0x00, 0x00, 0x00, 0xFF, 0xA0, 0xFF, 0xA0, 0xFF, 0xA0, 0xFF, 0xA0, 0xFF, 0xA0, 0xFF, 0xA0, 0xFF, 0xA0, 0xFF, 0xA0, 0xFF, 0xA0, 0xFF, 0xA0, 0xFF, 0xA0, 0xFF, 0xA0 }, 0, 0x7FFFFFFF) + 0x4;
                    //Main.AddLog(BaseImg.ToString("X8"));
                }
            }
            catch { }
            IsThreadRun = false;
        }

        public AntiCaptcha AntiCaptcha { get; set; }

      

        public static List<AccountEx> All
        {
            get
            {
                List<AccountEx> list = new List<AccountEx>();
                try
                {
                    int cnt = 1;
                    foreach (XmlElement node in MicroLogin.XML.SelectSingleNode("Accounts").SelectNodes("Account" + Main.ProfileName.Replace("#", "")))
                    {
                        try
                        {
                            var account = new AccountEx(node, MicroLogin.XML);
                            account.Item = new ListViewItem((cnt++).ToString());



                            account.Item.SubItems.Add(account.User);
                            account.Item.SubItems.Add(account.Name);
                            account.Item.SubItems.Add(account.Server);
                            account.Item.SubItems.Add(account.LoginIndex);
                            account.Item.SubItems.Add(account.Status);

                            account.Item.SubItems.Add(account.Lvl);
                            account.Item.SubItems.Add(account.CountMam);
                            account.Item.SubItems.Add(account.CountShit);
                            account.Item.SubItems.Add(account.CountBut);
                            account.Item.SubItems.Add(account.Note);
                            account.Item.Tag = account;

                          

                            list.Add(account);
                        }
                        catch { }
                    }
                }
                catch { }
                return list;
            }
        }

        public List<AccountEx> Enum()
        {
            List<AccountEx> list = new List<AccountEx>();
            int cnt = 1;
            foreach (XmlElement node in XML.SelectSingleNode("Accounts").SelectNodes("Account"))
            {
                try
                {
                    AccountEx account = new AccountEx(node, XML, Key);
                    account.Item = new ListViewItem((cnt++).ToString());

                
                    
                    account.Item.SubItems.Add(account.User);
                    account.Item.SubItems.Add(account.Name);
                    account.Item.SubItems.Add(account.Server);
                    account.Item.SubItems.Add(account.LoginIndex);
                    account.Item.SubItems.Add(account.Status);
                    account.Item.SubItems.Add(account.Lvl);
                    account.Item.SubItems.Add(account.CountMam);
                    account.Item.SubItems.Add(account.CountShit);
                    account.Item.SubItems.Add(account.CountBut);
                    account.Item.SubItems.Add(account.Note);
                    account.Item.Tag = account;

                    
                  

                    list.Add(account);
                }
                catch { }
            }
            return list;
        }
    }
}
