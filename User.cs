using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Web;
using MicroAuto.Properties;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml;
using System.Threading.Tasks;
using System.Threading;

namespace _i
{
    partial class User : UserControl
    {
        public event EventHandler Updated;

        public static string Email = string.Empty;
        public static string Pass = string.Empty;
        public static int Beri
        {
            get;
            set;
        }
        public static int Beli
        {
            get;
            set;
        }

        public static bool Dec_Beli(int beli)
        {
            //if (Global.IsNotDec)
            //    return true;
            //if (Beri + Beli < beli)
            //    return false;
            //Beri = Beri - beli;
            //if(Beri < 0)
            //{
            //    Beli += Beri;
            //    Beri = 0;                
            //}
            //Poster posterDecBeli = new Poster();
            //posterDecBeli.Url = "http://tieudattai.org/remlaw/user.php";
            //posterDecBeli.Data = "cmd=decbeli&email=" + HttpUtility.UrlEncode(Email) + "&pass=" + HttpUtility.UrlEncode(Pass) + "&beli=" + beli;
            //posterDecBeli.AutoReconnect = true;            
            //posterDecBeli.Post();
            return true;
        }

        public User()
        {
            InitializeComponent();
           
        }       

        private void txtLoginEmail_TextChanged(object sender, EventArgs e)
        {
            txtLoginEmail.Text = txtLoginEmail.Text.Trim();
            SettingOld.Write("User", "Email", txtLoginEmail.Text);
            Email = txtLoginEmail.Text;
        }

        private void txtLoginPass_TextChanged(object sender, EventArgs e)
        {
            if (txtLoginPass.Text.Length == 32)
            {
                Pass = txtLoginPass.Text;
            }
            else
            {
                RawPass = txtLoginPass.Text;
                Pass = TDT.Hasher.MD5(txtLoginPass.Text);
            }
            SettingOld.Write("User", "Pass", RawPass);
        }

        public static string RawPass { get; set; } = string.Empty;

        //public static Downloader dl;

        private void btnLogin_Click(object sender, EventArgs e)
        {
            ////////////////////////////////////////////////////////////////////////
            //lecaotri
            
            bool flag = false;
            if (TDT.CheckForSQLInjection(this.txtLoginEmail.Text.Trim()))
            {
                flag = true;
            }
            if (flag)
            {
                MessageBox.Show(this, "Vui lòng nhập địa chỉ user hợp lệ", "MicroAuto", MessageBoxButtons.OK);
                return;
            }

            ////////////////////////////////////////////////////////////////////////
            txtLoginEmail.Text = txtLoginEmail.Text.Trim();
            if (!TDT.IsValidEmail(txtLoginEmail.Text) && !TDT.IsPhoneNumber(txtLoginEmail.Text))
                MessageBox.Show(this, "Vui lòng nhập địa chỉ user hợp lệ", "MicroAuto", MessageBoxButtons.OK);
            else if (txtLoginPass.Text.Length < 6 && sender != null)
                ShowMsg("Vui lòng nhập mật khẩu hợp lệ");
            else
            {
                Enabled = false;
                Login();
            }
        }
        private void ShowMsg(string msg)
        {
            MessageBox.Show(this, msg, "MicroAuto", MessageBoxButtons.OK);
        }
        private void Login()
        {         
            Poster posterLogin = new Poster();
            posterLogin.Url = "http://micro.tieudattai.org/user.php";
            posterLogin.Control = this;
            posterLogin.Data = "cmd=login&vip=" + Global.IsVIP + "&email=" + HttpUtility.UrlEncode(txtLoginEmail.Text) + "&pass=" + HttpUtility.UrlEncode(Pass) + "&multed=" + TDT.Bool2Int(chkMulti.Checked) + "&ispri=" + TDT.Bool2Int(checkBox1.Checked);
            Poster.IsPri  = "&ispri=" + TDT.Bool2Int(checkBox1.Checked);
            Poster.IsMulted = "&multed=" + TDT.Bool2Int(chkMulti.Checked);
            posterLogin.Completed += new EventHandler(posterLogin_Completed);
            posterLogin.Post(); 
        }

   
        public bool IsLogged { get; set; }

        void posterLogin_Completed(object sender, EventArgs e)
        {
            Poster poster = (Poster)sender;
            if (poster.IsError || poster.Response.Contains("Kết nối thất bại"))
            {
                btnLogin.Text = "Lỗi " + Poster.ErrorCount;
                Login();
                return;
            }
            try
            {
                SettingOld.Str = poster.Response.Substring(poster.Response.IndexOf('@'), poster.Response.Length - poster.Response.IndexOf('@'));
                if (SettingOld.LoadSetting("fupdate")[0] != 0)
                {
                   
               
                    btnLogin.Text = "Update";
                    try
                    {
                        Process.Start(Global.APPPath + "//Update.exe", Global.Argument);
                        Main.Instance.Dispose();
                    }
                    catch
                    {
                        Enabled = true;
                        MessageBox.Show(this, "Có lỗi xảy ra\r\nUpdate thất bại, vui lòng thử lại", "MicroAuto", MessageBoxButtons.OK);
                    }
                    Updated(this, null);
                }
                else
                {
                    SettingOld.Str = poster.Response.Substring(poster.Response.IndexOf('@'), poster.Response.Length - poster.Response.IndexOf('@'));
                    Global.OFFSET = Regex.Replace(poster.Response, "@.*", "");
                    Global.IsFull = SettingOld.LoadSetting("f")[0];
            
                    Global.IsVIP = SettingOld.LoadSetting("vip")[0];
                    Global.IsAnti = SettingOld.LoadSetting("anti") != null;
                    if (TDT.FileHostValid)
                    {
                        IsLogged = true;
                        LoginSettings();
                    }
                }
           
               
              
            }
            catch
            {
                Enabled = true;
                if (!poster.Response.Contains("@"))
                {
                    //lecaotri2020 		"Vui lòng tải bản mới nhất tại tieudattai.org hoặc chạy file Update\r\nRun update or download lastet version from tieudattai.org"	string
                    if (poster.Response.Contains("Run update") || poster.Response.Contains("update Auto"))
                    {
                        try
                        {
                            Process.Start(Global.APPPath + "//Update.exe", Global.Argument);
                        }
                        catch
                        {
                            Enabled = true;
                            MessageBox.Show(this, "Có lỗi xảy ra\r\nUpdate thất bại, vui lòng thử lại", "MicroAuto", MessageBoxButtons.OK);
                        }
                        Updated(this, null);
                    }
                    MessageBox.Show(this, poster.Response, "MicroAuto", MessageBoxButtons.OK);

                }
            }
        }

        private void LoginSettings()
        {
            Task.Run(() =>
            {
                Main.RunWS();

       
                Scripts.Load();
                if (Global.IniParser == null)
                {
                    Global.IniParser = new IniParser(Poster.CurlGet("https://tieudattai.org/microauto/user.php?cmd=ini&do=load&email=" + HttpUtility.UrlEncode(User.Email) + "&pass=" + HttpUtility.UrlEncode(User.Pass), null, true));
                    foreach (var l in Global.IniParser.EnumSection("MicroLogin"))
                    {
                        if ((TDT.IsValidEmail(l) || TDT.IsPhoneNumber(l)))
                        {
                            Game.IniParser.Write("MicroLogin", l, Global.IniParser.Read("MicroLogin", l));
                        }
                    }
                }
                Invoke(new Action(() =>
                {
                    Dispose();
                }));
            });
          
            
            //string data = "cmd=stload&email=" + HttpUtility.UrlEncode(User.Email)
            //    + "&pass=" + HttpUtility.UrlEncode(User.Pass)
            //    ;
            //Poster posterLogin = new Poster();
            //posterLogin.Url = "http://www.tieudattai.org/remlaw/user.php";
            //posterLogin.Data = data;
            //posterLogin.Control = this;
            //posterLogin.AutoReconnect = true;
            //posterLogin.Completed += PosterLogin_Completed;
            //posterLogin.Post();
        }

        private void PosterLogin_Completed(object sender, EventArgs e)
        {
            //Poster poster = (Poster)sender;
            //Settings.xml = poster.Response;           
            //Dispose();
        }

        private void linkTieuDatTai_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void User_Load(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                string dllPath = Global.APPPath + "//tieudattai.dll";
                string updatePath = Global.APPPath + "//update.exe";
                string mp3Path = Global.APPPath + "//alarm.mp3";
                if (!File.Exists(mp3Path))
                {
                    try
                    {
                        TDT.FileInstall("MicroAuto", "nier.mp3", mp3Path);
                    }
                    catch { }
                }
                try
                {
                    TDT.FileInstall("MicroAuto", "tieudattai.dll", dllPath);
                }
                catch { }
                try
                {
                    TDT.FileInstall("MicroAuto", "Update.exe", updatePath);
                }
                catch { }
                txtLoginEmail.Text = SettingOld.Read("User", "Email");
                txtLoginPass.Text = SettingOld.Read("User", "Pass");
                chkMulti.Checked = SettingOld.Read("User", "Multi") == "True";
                chkEnglish.Checked = SettingOld.Read("User", "English") == "True";
                checkBox1.Checked = SettingOld.Read("User", "Private") == "True";
                if (txtLoginPass.Text.Length == 32)
                    txtLoginPass.Text = string.Empty;
                txtLoginEmail.Text = txtLoginEmail.Text.Trim();
                Enabled = true;
            });
            //Poster posterAds = new Poster();
            //posterAds.Url = "http://tieudattai.org/getads.php";
            //posterAds.Completed += PosterAds_Completed;
            //posterAds.AutoReconnect = true;
            //posterAds.Control = this;
            //posterAds.Get();
            //Enabled = false;
      
            //if (TDT.IsValidEmail(txtLoginEmail.Text) && txtLoginPass.Text.Length == 32)
            //{
            //    btnLogin_Click(null, null);
            //}
        }

        private void PosterAds_Completed(object sender, EventArgs e)
        {
            var poster = sender as Poster;
            string response = poster.Response;
            if (response.Split('\n').Length == 2)
            {
                adsurl = response.Split('\n')[1].Trim();
                Poster posterImg = new Poster();
                posterImg.Url = response.Split('\n')[0].Trim();
                //Poster.IsImage = true;
                posterImg.Completed += PosterImg_Completed;
                posterImg.AutoReconnect = true;
                posterImg.Get();
                posterImg.Control = this;
            }
            else
            {
                Enabled = true;
            }
        }

        private void PosterImg_Completed(object sender, EventArgs e)
        {
            //Poster.IsImage = false;
            //var poster = sender as Poster;
            //pictureBox1.Image = Poster.bitmap;
            //Enabled = true;
        }

        private void txtLoginEmail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && txtLoginEmail.Text.Trim() != "")
                txtLoginPass.Select();
        }

        private void txtLoginPass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && txtLoginPass.Text.Length >= 6)
                btnLogin_Click(null, null);
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            txtEmail.Text = txtEmail.Text.Trim();
            //txtName.Text = txtName.Text.Trim();
            //if (txtName.Text.Length < 1)
            //    MessageBox.Show(this, "Vui lòng điền tên của bạn", "MicroAuto", MessageBoxButtons.OK);
            if (!TDT.IsValidEmail(txtEmail.Text) && !TDT.IsPhoneNumber(txtEmail.Text))
                MessageBox.Show(this, "Vui lòng nhập địa chỉ user hợp lệ", "MicroAuto", MessageBoxButtons.OK);
            else if (txtPass.Text.Length < 6)
                MessageBox.Show(this, "Mật khẩu phải có ít nhất 6 kí tự", "MicroAuto", MessageBoxButtons.OK);
            else if (txtRetypePass.Text != txtPass.Text)
                 MessageBox.Show(this, "Mật khẩu nhập lại không khớp", "MicroAuto", MessageBoxButtons.OK);
            else
            {
                this.Enabled = false;
                Register();
            }
        }

        private void Register()
        {
            string data = "email=" + HttpUtility.UrlEncode(txtEmail.Text)
                + "&pass=" + HttpUtility.UrlEncode(TDT.Hasher.MD5(txtPass.Text))
                ;
            Poster posterRegister = new Poster();
            posterRegister.Url = URL.REGISTER;
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
                btnRegister.Text = "Lỗi " + Poster.ErrorCount;
                Register();
                return;
            }
            Enabled = true;
            MessageBox.Show(this, poster.Response, "MicroAuto", MessageBoxButtons.OK);
        }

        private void btnGet_Click(object sender, EventArgs e)
        {
            txtLostPass.Text = txtLostPass.Text.Trim();

            if (!TDT.IsValidEmail(txtLostPass.Text) && !TDT.IsPhoneNumber(txtLostPass.Text))
                 MessageBox.Show(this, "Vui lòng nhập địa chỉ email hợp lệ", "MicroAuto", MessageBoxButtons.OK);
            else
            {
                this.Enabled = false;
                LostPassword();
            }
        }

        private void LostPassword()
        {            
            string data = "email=" + HttpUtility.UrlEncode(txtLostPass.Text);
            Poster posterLostPass = new Poster();
            posterLostPass.Url = URL.HomePage + "microauto/login.php?action=lostpassword";
            posterLostPass.Data = data;
            posterLostPass.Control = this;
            posterLostPass.Completed += new EventHandler(posterLostPass_Completed);
            posterLostPass.Post();
        }

        void posterLostPass_Completed(object sender, EventArgs e)
        {
            Poster poster = (Poster)sender;
            if (poster.IsError)
            {
                btnGet.Text = "Lỗi " + Poster.ErrorCount;
                LostPassword();
                return;
            }
            Enabled = true;
            MessageBox.Show(this, poster.Response, "MicroAuto", MessageBoxButtons.OK);
        }

        private void linktieudattai_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(URL.HomePage + "");
        }

        private void btnFree_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this, "Soạn tin nhắn \"free\" gửi đến số 0936543800 để nhận 3 ngày sử dụng miễn phí\r\nChỉ free cho số điện thoại chưa đăng ký với auto\r\nĐăng nhập bằng số điện thoại và pass là 123456 sau khi nhận được tin nhắn phản hồi từ Tiêu Dật Tài\r\nSau đó vào auto để đổi pass", "MicroAuto", MessageBoxButtons.OK);
        }

        private void lblVIP_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ////lblVIP.Enabled = false;
            Poster posterVIP = new Poster();
            posterVIP.Control = this;
            posterVIP.Url = "http://tieudattai.org/remlaw/vip.php";
            posterVIP.Completed += new EventHandler(posterVIP_Completed);
            posterVIP.AutoReconnect = true;
            posterVIP.Post();
        }

        void posterVIP_Completed(object sender, EventArgs e)
        {
            //lblVIP.Enabled = true;
            var poster = sender as Poster;
            MessageBox.Show(this, poster.Response, "MicroAuto", MessageBoxButtons.OK);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("perfect");
        }

        public static string adsurl { get; set; } = "http://tieudattai.org";

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Process.Start(adsurl);
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void chkEnglish_CheckedChanged(object sender, EventArgs e)
        {
            IsEnglish = chkEnglish.Checked;
            SettingOld.Write("User", "English", chkEnglish.Checked.ToString());
        }

        public static bool IsEnglish { get; set; }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            SettingOld.Write("User", "Private", checkBox1.Checked.ToString());
        }

        private void chkMulti_CheckedChanged(object sender, EventArgs e)
        {
            SettingOld.Write("User", "Multi", chkMulti.Checked.ToString());
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                txtLoginPass.PasswordChar = '\0';
            }
            else
            {
                txtLoginPass.PasswordChar = '*';
            }
        }
    }
}
