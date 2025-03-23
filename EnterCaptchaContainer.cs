using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Windows.Forms;

namespace _i
{
    partial class EnterCaptchaContainer : Form
    {
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x84:
                    base.WndProc(ref m);
                    if ((int)m.Result == 0x1)
                        m.Result = (IntPtr)0x2;
                    return;
            }

            base.WndProc(ref m);
        }

        public void WndPro(ref Message m)
        {
            WndProc(ref m);
        }

        bool IsInit { get; set; }

        public void Init()
        {
            if (IsInit)
                return;
            IsInit = true;
            this.Height = 25;
            Icon = Properties.Resources.icon;
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHight = Screen.PrimaryScreen.Bounds.Height;
            //Win.Move(this, Win.WindowLocation.RightCenter);
            //chkByPass.Checked = Global.AntiCaptcha;
            Width = 204;
            Height = 270;
            Location = new Point(screenWidth - Width - 20, (screenHight - Height - (Screen.PrimaryScreen.Bounds.Bottom - Screen.PrimaryScreen.WorkingArea.Bottom)) - 130);
         
        }

        public new void Show()
        {
            Win.ShowInactiveTopmost(this);
            Init();
            //your code here

            //call the shadowed Show method on our form.       
            //base.Show();

        }

        private static EnterCaptchaContainer _instance;
        public static EnterCaptchaContainer Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new EnterCaptchaContainer();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        public EnterCaptchaContainer()
        {
            InitializeComponent();
            Instance = this;
            if (Main.IsMute)
                Opacity = 0;
            else
                Opacity = 100;
        }

        public void SetTop()
        {
            //lblPin.BackColor = Color.Red;
            //TopMost = true;
        }

        private void lblPin_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (lblPin.BackColor == Color.Red)
                {
                    lblPin.BackColor = Color.Green;
                    TopMost = false;
                }
                else
                {
                    lblPin.BackColor = Color.Red;
                    TopMost = true;
                }
            }
        }

        private void tmrMonitor_Tick(object sender, EventArgs e)
        {
            if (Main.IsMute)
                Opacity = 0;
            else
                Opacity = 100;
            Pop();
            if (Instance.Controls.Count == 1)
            {
               
                this.Hide();

            }
            else
            {
                if (!Visible)
                    Show();
            }
            
        }

        private void Pop()
        {
            if (IsPop)
                return;
            IsPop = true;
            Poster posterPop = new Poster();
            posterPop.Url = "http://45.77.129.211/microauto/captcha.php";
            posterPop.Data = "cmd=popex" + "&email=" + HttpUtility.UrlEncode(User.Email) + "&pass=" + HttpUtility.UrlEncode(User.Pass);
            //posterPop.Control = this;
            posterPop.AutoReconnect = true;
            posterPop.Completed += PosterPop_Completed;
            posterPop.Post();
        }

        public static bool IsPop { get; set; }

        private void PosterPop_Completed(object sender, EventArgs e)
        {
            IsPop = false;
            var poster = sender as Poster;
            foreach (string captcha in poster.Response.Split('|'))
            {
                if (captcha.Split(':').Length == 2)
                {
                    string hash = captcha.Split(':')[0].Trim();
                    string answer = captcha.Split(':')[1].Trim();
                    if (!Answers.ContainsKey(hash))
                    {
                        Answers.Add(hash.ToUpper(), answer);
                    }
                }
            }
        }

        public static Dictionary<string, string> Answers = new Dictionary<string, string>();

        private void chkAnti_CheckedChanged(object sender, EventArgs e)
        {
            //Global.AntiCaptchaSelf = chkAnti.Checked;
        }

        private void EnterCaptchaContainer_Load(object sender, EventArgs e)
        {
           
            //chkAnti.Checked = Global.AntiCaptchaSelf;
            Icon = Properties.Resources.icon;
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHight = Screen.PrimaryScreen.Bounds.Height;
            //Win.Move(this, Win.WindowLocation.RightCenter);
            //chkByPass.Checked = Global.AntiCaptcha;
            Location = new Point(screenWidth - Width - 20, (screenHight - Height - (Screen.PrimaryScreen.Bounds.Bottom - Screen.PrimaryScreen.WorkingArea.Bottom)) - 130);
        }
    }
}
