using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Web;
using System.IO;
using System.Drawing.Imaging;
using System.Diagnostics;

namespace _i
{
    partial class AntiCaptcha : UserControl
    {
        public Game game;

        public AntiCaptcha(Game game)
        {
            this.game = game;
            InitializeComponent();
            Dock = DockStyle.Fill;

            //ActiveControl = txtAnswer;
        }

        public NumericTextBox TxtAnswer
        {
            get
            {
                return txtAnswer;
            }
            set
            {
                txtAnswer = value;
            }
        }

        public static List<string> ListImg = new List<string>();

        public string img { get; set; }

        private void PostCaptcha(string Img)
        {
            if (string.IsNullOrEmpty(Img))
            {
                Img = img;
            }
            img = Img;
            //if (OptionEx.CaptchaKey.Length != 32 || !Global.AntiCaptcha2Captcha)
            //{
            //    return;
            //}
            if (ListImg.Contains(Img))
                return;
            else
                ListImg.Add(Img);
            string captcha = string.Empty;
            for (int i = 0; i < 1152; i = i + 2)
            {
                string str = Img.Substring(i, 2);
                int value = TDT.Hex2Int(str);
                string binary = Convert.ToString(value, 2);
                while (binary.Length < 8)
                    binary = "0" + binary;
                captcha += binary;
            }
            Bitmap pic = new Bitmap(128, 36);
            for (int i = 0; i < captcha.Length; i++)
            {
                if (captcha[i] == '0')
                {
                    pic.SetPixel(i % 128, i / 128, Color.Black);
                }
                else
                {
                    pic.SetPixel(i % 128, i / 128, Color.White);
                }
            }
            pic = new Bitmap(pic, new Size(160, 45));

            Poster posterPost = new Poster();
            posterPost.Url = "http://tieudattai.org/remlaw/2captcha/in.php";
            posterPost.Data = "numeric=1&method=base64&min_len=4&max_len=4&submib=" + HttpUtility.UrlEncode("download and get the ID") + "&body=";
            string SigBase64 = "";
            using (var ms = new MemoryStream())
            {
                pic.Save(ms, ImageFormat.Jpeg);
                SigBase64 = Convert.ToBase64String(ms.GetBuffer()); //Get Base64
            }
            posterPost.Data += HttpUtility.UrlEncode(SigBase64);
            posterPost.Data += "&email=" + HttpUtility.UrlEncode(User.Email) + "&pass=" + HttpUtility.UrlEncode(User.Pass);
            posterPost.AutoReconnect = true;
            posterPost.Completed += PosterPost_Completed;
            posterPost.Post();
            lastAS = null;
        }

        public string CaptchaId = string.Empty;
        public bool IsNeedGet { get; set; }

        private void PosterPost_Completed(object sender, EventArgs e)
        {
            var poster = sender as Poster;
            if (poster.Response.Split('|').Length == 2)
            {
                CaptchaId = poster.Response.Split('|')[1];
                IsNeedGet = true;
                GetCaptcha();
            }
            else
            {
                if (!poster.Response.Contains("notenough"))
                {
                    PostCaptcha(null);
                }
            }
        }


        private void GetCaptcha()
        {
            Poster posterGet = new Poster();
            posterGet.Url = "http://tieudattai.org/remlaw/2captcha/res.php?action=get&id=" + CaptchaId;
            posterGet.Completed += PosterGet_Completed;
            posterGet.Control = this;
            posterGet.Delay = 2000;
            posterGet.AutoReconnect = true;
            posterGet.Post();
        }

        private void PosterGet_Completed(object sender, EventArgs e)
        {
            if (IsDisposed)
                return;
            var poster = sender as Poster;
            if (poster.Response.Split('|').Length == 2)
            {
                bool isrep = false;
                TextAnswer = (poster.Response.Split('|')[1]).Trim();
                if (TextAnswer.Length < 4)
                {
                    isrep = true;
                    ReportBad();
                    TextAnswer += "0000".Substring(0, 4 - TextAnswer.Length);
                }
                if (TextAnswer.Length > 4)
                {
                    isrep = true;
                    ReportBad();
                    TextAnswer = TextAnswer.Substring(0, 4);
                }
                foreach (char c in TextAnswer)
                {
                    if (c < '0' || c > '9')
                    {
                        isrep = true;
                        ReportBad();
                        break;
                    }
                }
                if (!isrep)
                    lastAS = Stopwatch.StartNew();
                IsNeedGet = false;
                Ansewer();
            }
            else
            {
                if (poster.Response.Contains("UNSOLVABLE"))
                {
                    TextAnswer = "1234";
                    Ansewer();
                }
                else
                {
                    GetCaptcha();
                }
            }
        }

        public static List<string> IdReport = new List<string>();

        void ReportBad()
        {
            if (!IdReport.Contains(CaptchaId))
            {
                IdReport.Add(CaptchaId);
                Poster posterReport = new Poster();
                posterReport.AutoReconnect = true;
                posterReport.Url = "http://tieudattai.org/remlaw/2captcha/res.php?action=reportbad&id=" + CaptchaId;
                posterReport.Get();
            }
           
        }

        Stopwatch swCaptcha = Stopwatch.StartNew();

        Stopwatch OpenTime = Stopwatch.StartNew();

        private void tmrRefresh_Tick(object sender, EventArgs e)
        {
            try
            {
                if (!game.TLBB.IsTextCaptcha || !Main.DicGame.ContainsValue(game))
                {
                    //if (AntiCaptcaManager.CurAntiCaptcha == this)
                    //    AntiCaptcaManager.CurAntiCaptcha = null;
                    this.Dispose();
                    return;
                }
                if (game.BaseImg == 0)
                {
                    game.ReadBaseImg();
                    picCaptcha.Image = Properties.Resources.loading;
                }
                else
                {
                    picCaptcha.Image = game.Captcha;
                    if (AntiCaptcaManager.Answers.ContainsKey(game.ImgHash))
                    {
                        game.DoStringEx("DataPool:SendLoginCode('" + AntiCaptcaManager.Answers[game.ImgHash] + "')");
                    }
              




                    if (!AntiCaptcaManager.Captchas.ContainsKey(game.ImgHash))
                    {
                        if (lastAS != null && Global.IsReportBad)
                        {
                            lastAS = null;
                            ReportBad();
                        }
                        else
                        {
                            TextAnswer = "";
                            if (Global.AntiCaptcha)
                            {
                                AntiCaptcaManager.Captchas.Add(game.ImgHash, game.Img);
                                Push(game.Img);
                            }
                        }

                    }
                    if (!Game.CaptchaHash.Contains(game.ImgHash))
                    {

                        Game.CaptchaHash.Add(game.ImgHash);
                        Global.SaveCaptcha();
                    }
                }

                if (Global.AntiCaptcha)
                {
                    if (swCaptcha.Elapsed.TotalSeconds > 120)
                    {
                        game.LUA.LoginOverTime();
                    }
                }
            }
            catch
            {
                //Main.PushLog(ex.Message + ex.StackTrace);
            }
        }

        public void Push(string data)
        {
            //if (User.Email == "tieudattai@yahoo.com")
            //{
            //    PostCaptcha(data);
            //    return;
            //}
            if (Global.IsUuTienBeri)
            {
                if (Global.IsUuTienBeri)
                {
                    if (Global.CaptchaIdleTime.IsRunning)
                    {
                        if (Global.CaptchaIdleTime.Elapsed.TotalMinutes > 5)
                        {
                            Global.CaptchaIdleTime = new Stopwatch();
                        }
                        else
                        {
                            PostCaptcha(data);
                            return;
                        }
                    }
                }
            }        
            if (IsDisposed)
                return;
            Poster posterPush = new Poster();
            posterPush.Url = URL.HomePage + "microauto/captcha.php";
            posterPush.AutoReconnect = true;
            if (!data.Contains("="))
                posterPush.Data = "cmd=push" + "&image=" + data + "&email=" + HttpUtility.UrlEncode(User.Email) + "&pass=" + HttpUtility.UrlEncode(User.Pass) + "&type=" + game.Address.GameType;
            posterPush.Control = this;
            posterPush.DataEx = data;
            posterPush.Completed += PosterPush_Completed;
            posterPush.Post();
        }

        private void PosterPush_Completed(object sender, EventArgs e)
        {
            var poster = sender as Poster;
            if(poster.Response == "ok")
            {
                Global.CaptchaIdleTime = new Stopwatch();
            }
            else
            {
                Global.CaptchaIdleTime = Stopwatch.StartNew();
                PostCaptcha(poster.DataEx);
                return;
            }
        }

        private void picCaptcha_MouseClick(object sender, MouseEventArgs e)
        {
            //AntiCaptcaManager.CurAntiCaptcha = this;
            if (e.Button == MouseButtons.Left)
                game.Active();
        }

        public string TextAnswer = "";
        

        private void txtAnswer_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                if (TextAnswer.Length == 4)
                {                    
                    game.DoStringEx("DataPool:SendLoginCode('" + TextAnswer + "')");
                }
            }
        }



        public void Ansewer()
        {
            if (TextAnswer.Length == 4)
            {
               
                if (Global.IsS23)
                {
                    Timer tmr = new Timer()
                    {
                        Enabled = true,
                        Interval = 10
                    };
                    tmr.Tick += Tmr_Tick;
                    txt = TextAnswer;
                }
                else
                {
                    game.DoStringEx("DataPool:SendLoginCode('" + TextAnswer + "')");
                }
                try
                {
                    this.SendToBack();
                }
                catch { }
                txtAnswer.Text = "";
                TxtAnswer.Text = "";
            }
        }

        public string txt = string.Empty;

        private void Tmr_Tick(object sender, EventArgs e)
        {
            if (Global.IsByPass)
            {
                game.DoStringEx("DataPool:SendLoginCode('" + txt + "')");
                Timer tim = sender as Timer;
                tim.Stop();
            }
        }

        Stopwatch lastAS;

        private void lblAnswer_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                if (TextAnswer.Length == 4)
                {                    
                    game.DoStringEx("DataPool:SendLoginCode('" + TextAnswer + "')");
                }
            }
        }

        private void label1_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                game.Exit();
                this.Dispose();
            }
        }

        private void txtAnswer_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (txtAnswer.Text.Length == 4)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    TextAnswer = txtAnswer.Text;
                    Ansewer();
                }
            }
        }

        private void exitGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            game.Exit();
            this.Dispose();
        }

        private void AntiCaptcha_Load(object sender, EventArgs e)
        {
            
        }

    }
}
