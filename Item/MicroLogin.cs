using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Forms;
using System.Xml;
using System.Linq;
namespace _i
{
    partial class MicroLogin : UserControl
    {

        public MicroLogin()
        {
            InitializeComponent();
            Instance = this;
            //Pan = panelLogin;
            //Icon = Properties.Resources.icon;

     
        }

        public static int Limit { get; set; }
     
        private void MicroLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (Main.IsExit)
                {
                    this.Dispose();
                    return;
                }
                if (e.CloseReason != CloseReason.WindowsShutDown && e.CloseReason != CloseReason.ApplicationExitCall)
                {
                    Hide();
                    e.Cancel = true;
                }
                else if (e.CloseReason == CloseReason.WindowsShutDown)
                {
                    this.Dispose();
                }
            }
            catch
            {

            }
        }

        private void btnPublisher_Click(object sender, EventArgs e)
        {
            var control = new Publisher() { Dock = DockStyle.Fill };
            Controls.Add(control);
            control.BringToFront();
            splitContainer1.Visible = false;
            control.Disposed += (ss, ee) =>
            {
                splitContainer1.Visible = true;
            };
        }


        private void btnAddAcc_Click(object sender, EventArgs e)
        {
            try
            {
                if (tab.SelectedTab.Tag != null && tab.SelectedTab.Tag.ToString() == "loged")
                {
                    var tabLogin = tab.SelectedTab.Controls[0] as TabLogin;
                    tabLogin.AddAcc();
                    return;
                }

            }
            catch { }
        }








        public void OnAccChange()
        {
            AccChanged?.Invoke(MicroLogin.Instance, null);
        }




        public static List<AccountEx> Accounts { get; set; }

        public event System.EventHandler AccChanged;



        private void MicroLogin_Load(object sender, EventArgs e)
        {
            tab.TabPages[0].Controls.Add(new TabLogin() { Dock = DockStyle.Fill });
         


            Instance = this;

            CboNPH.Items.Clear();
            foreach (XmlNode node in Publisher.XML.SelectSingleNode("Publishers").SelectNodes("Publisher"))
            {
                CboNPH.Items.Add(node.Attributes["Name"].Value);
            }
           
            //else
            //{
            //    this.ShowInTaskbar = true;
            //}
            //759, 522
        }






        public static bool IsOpenFile = false;



        private void LoadInfo()
        {
            CboNPH.Items.Clear();
            foreach (XmlNode node in Publisher.XML.SelectSingleNode("Publishers").SelectNodes("Publisher"))
            {
                CboNPH.Items.Add(node.Attributes["Name"].Value);
            }
        }

        private void cboNPH_SelectedIndexChanged(object sender, EventArgs e)
        {
            CboServer.Items.Clear();
            try
            {
                foreach (XmlNode node in Publisher.XML.SelectSingleNode("//Publishers/Publisher[@Name=\"" + CboNPH.Text + "\"]/Servers").SelectNodes("Server"))
                {
                    CboServer.Items.Add(node.InnerText);
                }
            }
            catch { }
            if (CboTail.Items.Count > 0)
                CboTail.Items.Clear();
            try
            {
                foreach (XmlNode node in Publisher.XML.SelectSingleNode("//Publishers/Publisher[@Name=\"" + CboNPH.Text + "\"]/Tails").SelectNodes("Tail"))
                {
                    CboTail.Items.Add(node.InnerText);
                }
            }
            catch { }
        }




        public static int MaxOpenGame { get; set; } = 6;
        public static int OpenGameTime { get; set; } = 5;



        public static Stopwatch swOpenGameTime;

        string PathGame
        {
            get;
            set;
        }


        bool IsPop = true;



        void Push(string data)
        {
            if (IsDisposed)
                return;
            if (!IsPop)
                return;
            Poster posterPush = new Poster();
            posterPush.Url = URL.HomePage + "microauto/captcha.php";
            posterPush.AutoReconnect = true;
            if (!data.Contains("="))
                posterPush.Data = "cmd=push" + "&image=" + data + "&email=" + HttpUtility.UrlEncode(User.Email) + "&pass=" + HttpUtility.UrlEncode(User.Pass);
            posterPush.Control = this;
            posterPush.Completed += new EventHandler(posterPush_Completed);
            posterPush.Post();
        }

        void posterPush_Completed(object sender, EventArgs e)
        {
            var poster = sender as Poster;
            if (poster.IsError)
            {
                Push(poster.Data);
            }
            else
            {
                if (poster.Response == "dis")
                {
                    IsPop = false;
                }
            }
        }



        public static Dictionary<string, string> Answers = new Dictionary<string, string>();

        public static AccountEx AccCaptchaEx = null;
        public static bool IsRefresh = false;

        private void chkShowPass_CheckedChanged(object sender, EventArgs e)
        {
            if (chkShowPass.Checked)
            {
                TxtPass.PasswordChar = '\0';
            }
            else
            {
                TxtPass.PasswordChar = '*';
            }
        }




        private void txtUser_TextChanged(object sender, EventArgs e)
        {
            TxtUser.Text = TxtUser.Text.Trim();
        }




        private void cboServer_SelectedIndexChanged(object sender, EventArgs e)
        {

        }




        private void MicroLogin_Resize(object sender, EventArgs e)
        {

            //if (WindowState == FormWindowState.Minimized)
            //{
            //    isRefresh = true;
            //}
            //else if (WindowState == FormWindowState.Normal && isRefresh)
            //{
            //    isRefresh = false;
            //    Refresh();
            //}
        }




        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            //IsTopMost = TopMost = chkTopMost.Checked;
        }



        public static bool IsDungHop
        {
            get;
            set;
        }

        public static MicroLogin Instance
        {
            get;
            set;
        }



        public static List<AccountEx> ListCopyEx = new List<AccountEx>();



        public static int StopMinute = 30;
        public static Stopwatch swStop = Stopwatch.StartNew();
        public static Stopwatch swReset = Stopwatch.StartNew();
        public static bool IsStop { get; set; }















        public static bool IsResetBHD { get; set; }



        public static bool IsAllBHD { get; set; }






        public static bool IsExit30 { get; set; }
        public static int LvExit { get; set; } = 30;














        private void txtUser_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void txtUser_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAddAcc_Click(null, null);
            }
        }

        private void txtPass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAddAcc_Click(null, null);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (tab.SelectedTab.Tag != null && tab.SelectedTab.Tag.ToString() == "loged")
                {
                    var tabLogin = tab.SelectedTab.Controls[0] as TabLogin;
                    if (tabLogin.FirstSelectedItem == null)
                        return;
                    var account = tabLogin.FirstSelectedItem.Tag as AccountEx;
                    account.User = TxtUser.Text;
                    account.Pass = TxtPass.Text;
                    account.NPH = CboNPH.Text;
                    account.Server = CboServer.Text;
                    account.Tail = CboTail.Text;
                    AccChanged?.Invoke(this, null);
                    return;
                }

            }
            catch { }
        }






        public static bool IsTopMost { get; set; }

    


        public static Stopwatch swXongBHD;
        public static int CountXongBHD;

        public static string Clone { get; set; } = string.Empty;




        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this, "Bạn cần phải chọn đường dẫn game\r\nFile Game.exe nằm trong thư mục Bin của game", "MicroAuto", MessageBoxButtons.OK);
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Game.exe |Game.exe";
            if (openFile.ShowDialog(this) == DialogResult.OK)
            {
                new Thread(() =>
                {
                    try
                    {
                        //progressBar1.Value = 50;


                        while (Process.GetProcessesByName("Game").Length > 0)
                        {
                            try
                            {
                                Process.GetProcessesByName("Game")[0].Kill();
                            }
                            catch { }
                            Thread.Sleep(100);
                        }
                        string gameex = Poster.CurlGet("http://tieudattai.org/gamehex.php");

                        byte[] file = File.ReadAllBytes(openFile.FileName);

                        foreach (string line in gameex.Split('\n'))
                        {
                            try
                            {
                                string l = line.Trim();
                                int offset = TDT.ParseInt(l.Split(':')[0]);
                                int olval = ConverterEx.Hex2Int(l.Split(':')[1].Substring(0, l.Split(':')[1].IndexOf("=>")));
                                int val = ConverterEx.Hex2Int(l.Substring(l.IndexOf("=>") + 2));
                                if (file[offset] == olval)
                                    file[offset] = (byte)val;
                            }
                            catch { }
                        }
                        File.WriteAllBytes(openFile.FileName, file);
                        //progressBar1.Value = 100;
                        MessageBox.Show(this, "Vá xong rồi nhé", "MicroAuto", MessageBoxButtons.OK);

                    }
                    catch
                    {
                        MessageBox.Show(this, "Có lỗi xảy ra vui lòng thử lại hoặc reset máy", "MicroAuto", MessageBoxButtons.OK);
                    }

                }).Start();

            }
        }

        public static bool IsLogLai => CalendarEx.IsAutoLogin;

  





        public static bool IsLimitBonPhan { get; set; }

        public static int NumLimitBonPhan { get; set; } = 6;

        public static bool IsLimitTrongHoa { get; set; }

        public static int NumLimitTrongHoa { get; set; } = 6;

        public static bool IsLimitBienThan { get; set; }

        public static int NumLimitBienThan { get; set; } = 6;



       



    
        public static bool IsUuTienTrongHoa => true;


   

        public static bool IsNotOut { get; set; }




        private void tab_Selecting(object sender, TabControlCancelEventArgs e)
        {
            var tabcontrol = sender as TabControlEx;
            if (e.TabPageIndex == tabcontrol.TabCount - 1)
                e.Cancel = true;
        }

        private void tab_MouseDown(object sender, MouseEventArgs e)
        {


        }

        private void tab_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var tabcontrol = sender as TabControlEx;
                var lastIndex = tabcontrol.TabCount - 1;
                if (tabcontrol.GetTabRect(lastIndex).Contains(e.Location))
                {
                    TabPage page = new TabPage("Online");
                  
                    tabcontrol.TabPages.Insert(lastIndex, page);
                    tabcontrol.SelectedIndex = lastIndex;
                    page.Controls.Add(new TabLogin() { Dock = DockStyle.Fill });
                }
            }
        }





        public static XmlDocument XML = new XmlDocument();
        public static void LoadXML()
        {
            try
            {
                string path = "nier.xml";
                if (File.Exists(path))
                {
                    XML.Load(path);
                }
                else if (File.Exists(path + ".old"))
                {
                    XML.Load(path + ".old");
                }
                else
                {
                    XML = new XmlDocument();
                    XML.InsertBefore(XML.CreateXmlDeclaration("1.0", "UTF-8", null), XML.DocumentElement);
                    XML.AppendChild(XML.CreateNode(XmlNodeType.Element, "Accounts", ""));
                }
            }
            catch
            {
                try
                {
                    XML = new XmlDocument();
                    if (XML.SelectSingleNode("/*") == null)
                    {
                        XML.InsertBefore(XML.CreateXmlDeclaration("1.0", "UTF-8", null), XML.DocumentElement);
                        XML.AppendChild(XML.CreateNode(XmlNodeType.Element, "Accounts", ""));
                    }
                }
                catch { }
            }
        }


        public static void SaveXML()
        {
            try
            {
                XmlNode root = XML.SelectSingleNode("/*");

                List<XmlNode> list = new List<XmlNode>();

                foreach (XmlElement node in XML.SelectSingleNode("Accounts").SelectNodes("*"))
                {
                    if (node.Name == "Account" + Main.ProfileName.Replace("#", ""))
                        continue;
                    else
                        list.Add(node);
                }
                root.RemoveAll();

                foreach (var account in Accounts)
                {
                    root.AppendChild(account.Node);
                }
                list.ForEach(n => root.AppendChild(n));

                string path = "nier.xml";

                XML.Save(path + ".new");
                try
                {
                    File.Delete(path + ".old");
                    File.Move(path, path + ".old");
                }
                catch { }
                try
                {
                    File.Delete(path);
                    File.Move(path + ".new", path);
                }
                catch { }
                try
                {
                    File.Delete(path + ".old");
                }
                catch { }

            }
            catch
            {

            }
        }





        public static int SecLog = 60;
    

        public static bool IsNotOutKey { get; set; }

    

        public static bool IsSleepTime { get; set; } = true;

      
        private void btnSleepTime_Click(object sender, EventArgs e)
        {
            var control = new Item.SleepTime() { Dock = DockStyle.Fill };
            Controls.Add(control);
            control.BringToFront();
            //new Item.SleepTime().Show(this);        
        }

        public static bool IsRunTrungAc { get; set; }



        private void tab_TabIndexChanged(object sender, EventArgs e)
        {
        }

        private void tab_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tab.TabPages[tab.SelectedIndex].Tag == null)
                splitContainer1.Visible = false;
            else
                splitContainer1.Visible = true;
        }

    }
}
