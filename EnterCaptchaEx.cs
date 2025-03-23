using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Windows.Forms;

namespace _i
{
    partial class EnterCaptchaEx : UserControl
    {
        private Game game;
        public EnterCaptchaEx(Game game)
        {
            InitializeComponent();
            this.game = game;
        }

        string Img { get; set; }
        //string Hash { get; set; }
        string Id { get; set; }

        public EnterCaptchaEx(Bitmap captcha, string img, string id)
        {
            InitializeComponent();
            Id = id;
            picCaptcha.Image = captcha;
            Img = img;
            radAnswer1.Text = Img.Substring(Img.Length - 16, 4);
            radAnswer2.Text = Img.Substring(Img.Length - 12, 4);
            radAnswer3.Text = Img.Substring(Img.Length - 8, 4);
            radAnswer4.Text = Img.Substring(Img.Length - 4, 4);
            //game.TLBB. = TDT.Hasher.MD5(Img);
        }

        private void EnterCaptchaEx_Load(object sender, EventArgs e)
        {
            if (game != null)
            {
                if (game.TLBB.Captcha == null)
                {
                    picCaptcha.Image = Properties.Resources.loading;
                }
                else
                {
                    picCaptcha.Image = game.TLBB.Captcha;
                    string[] answers = game.TLBB.Answer;
                    radAnswer1.Text = answers[0];
                    radAnswer2.Text = answers[1];
                    radAnswer3.Text = answers[2];
                    radAnswer4.Text = answers[3];
                    //Img = game.TLBB.Img;
                    //Hash = TDT.Hasher.MD5(Img);
                }
            }
        }

        public void Push(string data)
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
                        //PostCaptcha(data);
                        //return;
                    }
                }
            }
            if (game != null && Global.AntiCaptchaSelf && !IsPush)
            {
                IsPush = true;
                if (IsDisposed)
                    return;
                //Poster posterPush = new Poster();
                //posterPush.Url = URL.HomePage + "microauto/captcha.php";
                hash = game.TLBB.ImgHash;
                //posterPush.Data = "cmd=push&hash=" + game.TLBB.ImgHash + "&image=" + Img + "&email=" + HttpUtility.UrlEncode(User.Email) + "&pass=" + HttpUtility.UrlEncode(User.Pass);
                //posterPush.Control = this;
                //posterPush.AutoReconnect = true;
                //posterPush.DataEx = data;
                //posterPush.Completed += PosterPush_Completed;
                //posterPush.Post();
                if (!Global.Captchas.ContainsKey(game.TLBB.ImgHash))
                    Global.Captchas.Add(game.TLBB.ImgHash, Img);
            }
        }

        string hash = "";


        //private void PostCaptcha(string Img)
        //{
        //    if (Img != null)
        //        img = Img;
        //    else
        //        Img = img;
        //    //if (OptionEx.CaptchaKey.Length != 32 || ! Global.AntiCaptchaSelf2Captcha)
        //    //{
        //    //    return;
        //    //}
        //    if (AntiCaptcha.ListImg.Contains(Img))
        //        return;
        //    else
        //        AntiCaptcha.ListImg.Add(Img);
        //    string captcha = string.Empty;
        //    for (int i = 0; i < 1152; i = i + 2)
        //    {
        //        string str = Img.Substring(i, 2);
        //        int value = TDT.Hex2Int(str);
        //        string binary = Convert.ToString(value, 2);
        //        while (binary.Length < 8)
        //            binary = "0" + binary;
        //        captcha += binary;
        //    }
        //    Bitmap pic = new Bitmap(128, 36);
        //    for (int i = 0; i < captcha.Length; i++)
        //    {
        //        if (captcha[i] == '0')
        //        {
        //            pic.SetPixel(i % 128, i / 128, Color.Black);
        //        }
        //        else
        //        {
        //            pic.SetPixel(i % 128, i / 128, Color.White);
        //        }
        //    }
        //    pic = new Bitmap(pic, new Size(160, 45));

        //    Poster posterPost = new Poster();
        //    posterPost.Url = "http://tieudattai.org/remlaw/2captcha/in.php";
        //    posterPost.Data = "textinstructions=" + HttpUtility.UrlEncode("Choice 1 or 2 or 3 or 4 is correct with image") +  "&numeric =1&method=base64&min_len=1&max_len=1&submib=" + HttpUtility.UrlEncode("download and get the ID") + "&body=";
        //    string SigBase64 = "";
        //    using (var ms = new MemoryStream())
        //    {
        //        using (var bmp = new Bitmap(Width, Height))
        //        {
        //            DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
        //            bmp.Save(ms, ImageFormat.Jpeg);
        //        }
        //        //pic.Save(ms, ImageFormat.Jpeg);
        //        SigBase64 = Convert.ToBase64String(ms.GetBuffer()); //Get Base64
        //    }
        //    posterPost.Data += HttpUtility.UrlEncode(SigBase64);
        //    posterPost.AutoReconnect = true;
        //    posterPost.Completed += PosterPost_Completed;
        //    posterPost.Post();
        //}


    

        //private void PosterPost_Completed(object sender, EventArgs e)
        //{
        //    var poster = sender as Poster;
        //    if (poster.Response.Split('|').Length == 2)
        //    {
        //        CaptchaId = poster.Response.Split('|')[1];
        //        GetCaptcha();
        //    }
        //    else
        //    {
        //        Dispose();
        //    }
        //}

        //private void PosterGet_Completed(object sender, EventArgs e)
        //{
        //    if (IsDisposed)
        //        return;
        //    var poster = sender as Poster;
        //    if (poster.Response.Split('|').Length == 2)
        //    {
        //        if ((poster.Response.Split('|')[1]).Trim().Length != 1)
        //        {
        //            ReportBad();
        //            PostCaptcha(null);
        //        }
        //        else
        //        {
        //            int choice = TDT.ParseInt((poster.Response.Split('|')[1]).Trim());
        //            if (choice >= 1 && choice <= 4)
        //            {
        //                if (choice == 1)
        //                {
        //                    game.LuaDoOneLineString("setmetatable(_G, {__index = AntiRobot_Env }); AntiRobot_SelAnswer(0); AntiRobot_OnCommit(1)");
        //                    Dispose();
        //                }
        //                if (choice == 2)
        //                {
        //                    game.LuaDoOneLineString("setmetatable(_G, {__index = AntiRobot_Env }); AntiRobot_SelAnswer(1); AntiRobot_OnCommit(1)");
        //                    Dispose();
        //                }
        //                if (choice == 3)
        //                {
        //                    game.LuaDoOneLineString("setmetatable(_G, {__index = AntiRobot_Env }); AntiRobot_SelAnswer(2); AntiRobot_OnCommit(1)");
        //                    Dispose();
        //                }
        //                if (choice == 4)
        //                {
        //                    game.LuaDoOneLineString("setmetatable(_G, {__index = AntiRobot_Env }); AntiRobot_SelAnswer(3); AntiRobot_OnCommit(1)");
        //                    Dispose();
        //                }
        //            }
        //            else
        //            {
        //                ReportBad();
        //                PostCaptcha(null);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        if (poster.Response.Contains("UNSOLVABLE"))
        //        {
        //            PostCaptcha(null);
        //        }
        //        else
        //        {
        //            GetCaptcha();
        //        }
        //    }
        //}

        //private void GetCaptcha()
        //{
        //    Poster posterGet = new Poster();
        //    posterGet.Url = "http://2captcha.com/res.php?action=get&id=" + CaptchaId;
        //    posterGet.Completed += PosterGet_Completed;
        //    posterGet.Control = this;
        //    posterGet.Post();
        //}

        //Stopwatch /*swCaptchaTime*/;

        private void timerRefresh_Tick(object sender, EventArgs e)
        {
      

            
             
            if (game != null)
            {

                if (EnterCaptchaContainer.Answers.ContainsKey(hash))
                {
                    if (radAnswer1.Text.ToUpper() == EnterCaptchaContainer.Answers[hash].ToUpper())
                    {
                        radAnswer1.Checked = true;
                        game.DoStringEx("setmetatable(_G, {__index = AntiRobot_Env }); AntiRobot_SelAnswer(0); AntiRobot_OnCommit(1)");
                    }
                    if (radAnswer2.Text.ToUpper() == EnterCaptchaContainer.Answers[hash].ToUpper())
                    {
                        radAnswer2.Checked = true;
                        game.DoStringEx("setmetatable(_G, {__index = AntiRobot_Env }); AntiRobot_SelAnswer(1); AntiRobot_OnCommit(1)");
                    }
                    if (radAnswer3.Text.ToUpper() == EnterCaptchaContainer.Answers[hash].ToUpper())
                    {
                        radAnswer3.Checked = true;
                        game.DoStringEx("setmetatable(_G, {__index = AntiRobot_Env }); AntiRobot_SelAnswer(2); AntiRobot_OnCommit(1)");
                    }
                    if (radAnswer4.Text.ToUpper() == EnterCaptchaContainer.Answers[hash].ToUpper())
                    {
                        radAnswer4.Checked = true;
                        game.DoStringEx("setmetatable(_G, {__index = AntiRobot_Env }); AntiRobot_SelAnswer(3); AntiRobot_OnCommit(1)");
                    }

                }
                else
                {
                }
                if (!game.TLBB.IsCaptcha)
                {
                    EnterCaptchaContainer.Answers.Remove(hash);
                    Dispose();                    
                }
                else
                {
                    if (game.TLBB.Captcha == null)
                    {
                        picCaptcha.Image = Properties.Resources.loading;
                    }
                    else
                    {

                        picCaptcha.Image = game.TLBB.Captcha;
                        //for (int i = 0; i < picCaptcha.Image.Width; i++)
                        //{
                        //    for (int j = 0; j < picCaptcha.Image.Height; j++)
                        //    {
                        //        if (((Bitmap)picCaptcha.Image).GetPixel(i, j) == Color.Transparent)
                        //        {
                        //            Main.PushLog("Nonono");c
                        //        }
                        //    }
                        //}
                     
                        string[] answers = game.TLBB.Answer;
                        radAnswer1.Text = answers[0];
                        radAnswer2.Text = answers[1];
                        radAnswer3.Text = answers[2];
                        radAnswer4.Text = answers[3];
                        Img = game.TLBB.Img;
                        //Hash = TDT.Hasher.MD5(Img);
                        if (!AntiCaptcaManager.Captchas.ContainsKey(game.TLBB.ImgHash))
                        {
                            try
                            {
                                if (Global.AntiCaptchaSelf)
                                {
                                    AntiCaptcaManager.Captchas.Add(game.TLBB.ImgHash, game.TLBB.Img);
                                    Push(game.TLBB.Img);
                                }
                            }
                            catch (Exception ex)
                            {
                                Main.PushLogEx(ex.Message + ex.StackTrace);
                            }
                        }
                    }
                }
                if (!Main.DicGame.ContainsValue(game))
                {
                    Dispose();
                }
            }
           
        
        }

        

        public bool IsPush { get; set; }

        private void PosterPush_Completed(object sender, EventArgs e)
        {
            var poster = sender as Poster;
            if (poster.Response == "ok")
            {
                Global.CaptchaIdleTime = new Stopwatch();
            }
            else
            {
                Global.CaptchaIdleTime = Stopwatch.StartNew();
                //PostCaptcha(poster.DataEx);
                return;
            }
        }

        private void lblAnswer_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if(game != null)
                {
                    if (radAnswer1.Checked)
                    {
                        game.DoStringEx("setmetatable(_G, {__index = AntiRobot_Env }); AntiRobot_SelAnswer(0); AntiRobot_OnCommit(1)");
                        Dispose();
                    }
                    if (radAnswer2.Checked)
                    {
                        game.DoStringEx("setmetatable(_G, {__index = AntiRobot_Env }); AntiRobot_SelAnswer(1); AntiRobot_OnCommit(1)");
                        Dispose();
                    }
                    if (radAnswer3.Checked)
                    {
                        game.DoStringEx("setmetatable(_G, {__index = AntiRobot_Env }); AntiRobot_SelAnswer(2); AntiRobot_OnCommit(1)");
                        Dispose();
                    }
                    if (radAnswer4.Checked)
                    {
                        game.DoStringEx("setmetatable(_G, {__index = AntiRobot_Env }); AntiRobot_SelAnswer(3); AntiRobot_OnCommit(1)");
                        Dispose();
                    }
                }
                else
                {
                    Answer();
                }
            }
        }

        void Answer()
        {
            if (IsDisposed)
                return;
            Poster posterAnswer = new Poster();
            posterAnswer.Url = URL.HomePage + "microauto/captcha.php";
            string answer = "";
            if (radAnswer1.Checked)
                answer = radAnswer1.Text;
            if (radAnswer2.Checked)
                answer = radAnswer2.Text;
            if (radAnswer3.Checked)
                answer = radAnswer3.Text;
            if (radAnswer4.Checked)
                answer = radAnswer4.Text;
            if (answer == "")
                return;
            lblAnswer.Enabled = false;
            posterAnswer.Data = "cmd=answer" + "&id=" + Id + "&answer=" + answer + "&email=" + HttpUtility.UrlEncode(User.Email) + "&pass=" + HttpUtility.UrlEncode(User.Pass);
            posterAnswer.Control = this;
            posterAnswer.AutoReconnect = true;
            posterAnswer.Completed += PosterAnswer_Completed;
            posterAnswer.Post();
            Dispose();
        }

        public bool IsForeDis { get; set; }

        private void PosterAnswer_Completed(object sender, EventArgs e)
        {
            //IsForeDis = true;
        }

        private void lblAnswer_Click(object sender, EventArgs e)
        {

        }

        private void picCaptcha_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (game != null)
                    game.Active();
            }
        }

        private void radAnswer1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void picCaptcha_Click(object sender, EventArgs e)
        {
           
        }
    }
}
