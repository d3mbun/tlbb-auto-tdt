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
    partial class AntiCaptcaManager : Form
    {
        public static Dictionary<string, string> Answers = new Dictionary<string, string>();
        public static Dictionary<string, string> Captchas = new Dictionary<string, string>();
        public static AntiCaptcaManager instance;
        public Panel PanelCaptcha
        {
            get
            {
                return panelCapcha;
            }
        }
        public static AntiCaptcaManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AntiCaptcaManager();
                }
                return instance;
            }
        }

        public AntiCaptcaManager()
        {
            InitializeComponent();
            Icon = Properties.Resources.icon;
            tmrHide_Tick(null, null);
            if (Main.IsMute)
                Opacity = 0;
            else
                Opacity = 100;
        }

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

        private void AntiCaptcaManager_Load(object sender, EventArgs e)
        {
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHight = Screen.PrimaryScreen.Bounds.Height;
            //Win.Move(this, Win.WindowLocation.RightCenter);
            //chkByPass.Checked = Global.AntiCaptcha;
            Location = new Point(screenWidth - Width - 20, (screenHight - Height - (Screen.PrimaryScreen.Bounds.Bottom - Screen.PrimaryScreen.WorkingArea.Bottom)) - 15);
        }

        bool IsInit = false;

        public void Init()
        {
            if (IsInit)
                return;
            IsInit = true;
            Icon = Properties.Resources.icon;
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHight = Screen.PrimaryScreen.Bounds.Height;
            //Win.Move(this, Win.WindowLocation.RightCenter);
            //chkByPass.Checked = Global.AntiCaptcha;
            Width = 260;
            Height = 45;
            Location = new Point(screenWidth - Width - 20, (screenHight - Height - (Screen.PrimaryScreen.Bounds.Bottom - Screen.PrimaryScreen.WorkingArea.Bottom)) - 15);

        }

        public new void Show()
        {
            Win.ShowInactiveTopmost(this);
            Init();
            //your code here

            //call the shadowed Show method on our form.       
            //base.Show();

        }


        private void vScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            panelCapcha.VerticalScroll.Value = ((ScrollBar)sender).Value;
        }

        public static bool IsVisible { get; set; }

        //public static AntiCaptcha CurAntiCaptcha;

        private void tmrHide_Tick(object sender, EventArgs e)
        {
            if (Main.IsMute)
                Opacity = 0;
            else
                Opacity = 100;
            if (panelCapcha.Controls.Count == 0)
            {
                IsVisible = false;
                this.Hide();                
            }
            else
            {
                //if (CurAntiCaptcha == null || CurAntiCaptcha.TextAnswer != "")
                //{
                //    for (int i = 0; i < panelCapcha.Controls.Count; i++)
                //    {
                //        AntiCaptcha antiCaptcha = (AntiCaptcha)panelCapcha.Controls[i];
                //        if (antiCaptcha.TextAnswer == "")
                //        {
                //            CurAntiCaptcha = antiCaptcha;
                //            break;
                //        }
                //    }
                //}
                //else if(CurAntiCaptcha != null)
                //{
                //    for (int i = panelCapcha.Controls.Count; i > 0; i--)
                //    {                        
                //        AntiCaptcha antiCaptcha = (AntiCaptcha)panelCapcha.Controls[i - 1];
                //        if (antiCaptcha == CurAntiCaptcha)
                //            continue;
                //        antiCaptcha.BackColor = Color.FromArgb(44, 135, 240);
                //    }                   
                //}
                //CurAntiCaptcha.BackColor = Color.Red;
                IsVisible = true;
                //if (Global.AntiCaptcha && (User.Beri + User.Beli > 5000))
                //    Pop();
                if (!Visible)
                    this.Show();

            }
        }

        public static void Pop()
        {          
            //if (Global.Is2Captcha && OptionEx.CaptchaKey.Length == 32 && Global.AntiCaptcha2Captcha)
            //{                
            //    return;
            //}
            if (IsPop || !Global.AntiCaptcha || Instance.panelCapcha.Controls.Count == 0)
                return;
            IsPop = true;
            Poster posterPop = new Poster();
            posterPop.Url = URL.HomePage + "microauto/captcha.php";
            posterPop.AutoReconnect = true;
            posterPop.Data = "cmd=pop" + "&email=" + HttpUtility.UrlEncode(User.Email) + "&pass=" + HttpUtility.UrlEncode(User.Pass);
            posterPop.Completed += PosterPop_Completed;
            posterPop.Post();           
        }

        public static void SetFocus()
        {
            try
            {
                Instance.Activate();                
                var antiCaptha = Instance.PanelCaptcha.Controls[0] as AntiCaptcha;
                antiCaptha.TxtAnswer.Select();
                antiCaptha.TxtAnswer.Focus();                
            }
            catch { }
        }

        public static void PosterPop_Completed(object sender, EventArgs e)
        {
            IsPop = false;
            var poster = sender as Poster;
            foreach (string captcha in poster.Response.Split('|'))
            {
                if (captcha.Split(':').Length == 2)
                {
                    string hash = captcha.Split(':')[0];
                    string answer = captcha.Split(':')[1];
                    if (!Answers.ContainsKey(hash))
                        Answers.Add(hash.ToUpper(), answer);
                }
            }
            Pop();
        }

        static bool IsPop = false;

    

    

        private void lblAnswer_Click(object sender, EventArgs e)
        {
            //if (textBoxEx1.Text.Length == 4)
            //{
            //    if (CurAntiCaptcha != null)
            //    {
            //        CurAntiCaptcha.TextAnswer = textBoxEx1.Text;
            //        CurAntiCaptcha.Ansewer();
            //        textBoxEx1.Text = "";
            //    }
            //}
        }

        private void AntiCaptcaManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.WindowsShutDown && e.CloseReason != CloseReason.ApplicationExitCall)
            {
                Hide();
                e.Cancel = true;
            }
        }

        private void txtAnswer_KeyDown(object sender, KeyEventArgs e)
        {
            if (txtAnswer.Text.Length == 4)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (panelCapcha.Controls.Count > 0)
                    {
                        ((AntiCaptcha)(panelCapcha.Controls[0])).TextAnswer = txtAnswer.Text;
                        //TextAnswer = txtAnswer.Text;
                        ((AntiCaptcha)(panelCapcha.Controls[0])).Ansewer();
                        txtAnswer.Text = "";
                    }
                }
            }
        }
    }
}
