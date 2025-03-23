using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Web;
using System.Xml;
using System.Text.RegularExpressions;

namespace _i
{
    partial class LoginUser : UserControl
    {
        public LoginUser()
        {
            InitializeComponent();
            Load += LoginUser_Load1;
           
        }

        private void LoginUser_Load1(object sender, EventArgs e)
        {
        }

        private void btnRegisterLogin_Click(object sender, EventArgs e)
        {
            if (!TDT.IsValidEmail(textRegMail.Text) && !TDT.IsPhoneNumber(textRegPass.Text))
                MessageBox.Show(this, "Vui lòng nhập địa chỉ user hợp lệ", "MicroAuto", MessageBoxButtons.OK);
            else if (textRegPass.Text.Length < 6)
                MessageBox.Show(this, "Mật khẩu phải có ít nhất 6 kí tự", "MicroAuto", MessageBoxButtons.OK);
            else if (textRegPass.Text != textRetypePass.Text)
                MessageBox.Show(this, "Mật khẩu nhập lại không khớp", "MicroAuto", MessageBoxButtons.OK);
            else
            {
                this.Enabled = false;
                Register();
            }
            Dock = DockStyle.Fill;
        }

        public string Pass { get; set; }
        public string Email { get; set; }
        public static bool Remember { get; set; }

        private void Register()
        {
            Enabled = false;
            string data = "cmd=rg&email=" + HttpUtility.UrlEncode(User.Email)
                + "&loginpass=" + HttpUtility.UrlEncode(TDT.Hasher.MD5(textRegMail.Text))
                + "&loginemail=" + HttpUtility.UrlEncode(textRegPass.Text)
                + "&pass=" + User.Pass
                ;
            Poster posterRegister = new Poster();
            posterRegister.Url = "http://www.tieudattai.org/remlaw/user.php";
            posterRegister.Data = data;
            posterRegister.Control = this;
            posterRegister.Completed += new EventHandler(posterRegister_Completed);
            posterRegister.Post();
        }

        void posterRegister_Completed(object sender, EventArgs e)
        {
            Poster poster = (Poster)sender;
            if (poster.IsError)
            {
                btnRegisterLogin.Text = "Lỗi " + Poster.ErrorCount;
                Register();
                return;
            }
            Enabled = true;
            MessageBox.Show(this, poster.Response, "MicroAuto", MessageBoxButtons.OK);            
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            Login();
        }

        private void Login()
        {
            Enabled = false;

            if(Email == "Offline")
            {
                XML = MicroLogin.XML;
                Dispose();
                return;
            }
         
            try
            {
                Pass = textLoginPass.Text;
                Email = textLoginEmail.Text;
                string data = "cmd=rglogin&email=" + HttpUtility.UrlEncode(Email)
                    + "&pass=" + HttpUtility.UrlEncode(TDT.Hasher.MD5(Pass))
                    ;
                Poster posterLogin = new Poster();
                posterLogin.AutoReconnect = true;
                posterLogin.Url = "http://www.tieudattai.org/remlaw/user.php";
                posterLogin.Data = data;
                posterLogin.Control = this;
                posterLogin.Completed += PosterLogin_Completed;
                posterLogin.Post();
            }
            catch
            {
                Enabled = true;
            }
        }

        private void PosterLogin_Completed(object sender, EventArgs e)
        {
            Enabled = true;
            Poster poster = (Poster)sender;
         
            if (poster.Response.Contains("sai email"))
            {
                MessageBox.Show(this, poster.Response, "MicroAuto", MessageBoxButtons.OK);
                return;
            }    
            try
            {
                XML.LoadXml(poster.Response);
            }
            catch
            {
                if (XML.SelectSingleNode("/*") == null)
                {
                    XML.InsertBefore(XML.CreateXmlDeclaration("1.0", "UTF-8", null), XML.DocumentElement);
                    XML.AppendChild(XML.CreateNode(XmlNodeType.Element, "Accounts", ""));
                }
            }
            // Game.IniParser.Write("Login", Email, Pass);
            Global.IniParser.Write("MicroLogin", Email, Pass);
            Game.IniParser.Write("MicroLogin", Email, Pass);

            try
            {

                if (PATH.Game == "" || !System.IO.File.Exists(PATH.Game))
                {

                }
                else
                {
                    System.IO.File.Move(PATH.Game + "\\WebClient\\webclient.exe", PATH.Game + "\\WebClient\\webclientt.exe");
                }
            }
            catch
            {

            }

            Dispose();
        }
        public XmlDocument XML = new XmlDocument();

     


        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string msg = "mật khẩu là chìa khóa quan trọng để giải mã dữ liệu được lưu\r\nchính admin cũng không biết mật khẩu của bạn\r\nbạn vui lòng nhớ kỹ thông tin đăng nhập \r\ntrong trường hợp bạn quên mật khẩu\r\nadmin chỉ có thể phục hồi lại được danh sách tên tài khoản\r\ncòn mật khẩu của tài khoản lưu trong dữ liệu bị mã hóa\r\nkhông thể giải mã nếu không có mật khẩu gốc";
            MessageBox.Show(this, msg, "MicroAuto", MessageBoxButtons.OK);
        }

        public static bool IsLoadOffline { get; set; }

        private void LoginUser_Load(object sender, EventArgs e)
        {
            if (!IsLoadOffline)
            {
                IsLoadOffline = true;
                Email = "Offline";
                Login();
            }
        }

        public static List<string> login = new List<string>();

       

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textLoginPass.PasswordChar = '\0';
            }
            else
            {
                textLoginPass.PasswordChar = '*';
            }
        }

        private void comboBox1_DropDown(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();

            foreach (var l in Game.IniParser.EnumSection("MicroLogin"))
            {
                if ((TDT.IsValidEmail(l) || TDT.IsPhoneNumber(l)))
                {
                    comboBox1.Items.Add(l);
                }
            }

            SettingOld.LoginInfo = SettingOld.LoginInfo.Replace("FFFFFFFF", "FFFF");
            string logins = SettingOld.LoginInfo;
            string tmp = logins.Replace("FFFF", "\n");
            foreach (string lo in tmp.Split('\n'))
            {
                string l = lo.Trim();
                if ((TDT.IsValidEmail(l) || TDT.IsPhoneNumber(l)))
                {
                    comboBox1.Items.Add(l);
                    Global.IniParser.Write("MicroLogin", l, Game.IniParser.Read("Login", l));
                    SettingOld.LoginInfo = SettingOld.LoginInfo.Replace(l, "");
                }
                else
                {
                    if (!string.IsNullOrEmpty(l))
                        SettingOld.LoginInfo = SettingOld.LoginInfo.Replace(l, "");
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textLoginEmail.Text = comboBox1.Text;
            textLoginPass.Text = Global.IniParser.Read("MicroLogin", textLoginEmail.Text);
        }

        private void label4_MouseClick(object sender, MouseEventArgs e)
        {
            this.Parent.Parent.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Email = "guess@yahoo.com";
            string data = "cmd=rglogin&email=" + HttpUtility.UrlEncode("guess@yahoo.com")
            + "&pass=" + HttpUtility.UrlEncode(TDT.Hasher.MD5("000000"))
            ;
            Poster posterLogin = new Poster();
            posterLogin.AutoReconnect = true;
            posterLogin.Url = "http://www.tieudattai.org/remlaw/user.php";
            posterLogin.Data = data;
            posterLogin.Control = this;
            posterLogin.Completed += PosterLogin_Completed;
            posterLogin.Post();
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void txtLoginEmail_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void LoginUser_Resize(object sender, EventArgs e)
        {
            tab.Location = new Point(this.Width / 2 - tab.Width / 2, this.Height / 2 - tab.Height / 2);
        }

        private void label4_Click(object sender, EventArgs e)
        {
            this.Parent.Parent.Dispose();
        }
    }
}
