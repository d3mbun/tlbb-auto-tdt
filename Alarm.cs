using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace _i
{
    partial class Alan : Form
    {
        //private Game game;
        //private int countDown;

        public Alan()
        {            
            InitializeComponent();
            if (Main.IsMute)
                Opacity = 0;
            else
                Opacity = 75;
        }
        

        public static void Push(Alam alan)
        {
            Instance.Invoke(new Action(() =>
            {
                Instance.Controls.Add(alan);
            }));
            
        }

        private static Alan alarm;
        public static Alan Instance
        {
            get
            {
                if (alarm == null)
                    alarm = new Alan();
                return alarm;
            }
        }

        //public Alarm(Game game, string alarm, int countDown)
        //{          
        //    InitializeComponent();
        //    this.Disposed += new EventHandler(Alarm_Disposed);
        //    this.game = game;            
        //    if (game.TLBB.Name != "ĐăngNhập")
        //        lblCharName.Text = game.TLBB.Name;
        //    else
        //        lblCharName.Text = game.LastName;
        //    lblAlarm.Text = alarm;
        //    if (countDown > 0)
        //    {
        //        this.countDown = countDown;
        //        lblExit.Text = string.Format("{0:0}:{1:00}", countDown / 60, countDown % 60);
        //        tmrCountDown.Start();
        //    }
        //    else
        //    {
        //        lblExit.Text = "";
        //    }
        //    lblMute.Visible = Global.Mute;
        //}

        private void lblClose_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //if (game.BachHoaDuyenCompleted)
                //{
                //    this.Hide();
                //}
                //else
                //{
                //    this.Dispose();
                //}
                this.Dispose();
            }
        }

        public void Init()
        {
            //lblMute.Visible = Global.Mute;
            Height = 25;
            Win.Move(this, Win.WindowLocation.BottomLeft);
        }

        private void tmrCountDown_Tick(object sender, EventArgs e)
        {
            if (Instance.Controls.Count == 0)
            {
                Music.Stop();
                this.Hide();
            }
            //lblMute.Visible = Global.Mute;
            //Height = 25;
            //Win.Move(this, Win.WindowLocation.BottomLeft);
            //if (countDown-- < 10 && MicroLogin.IsDangCho)
            //{
            //    if (lblAlarm.Text.Contains("BHD"))
            //    {
            //        game.LuaDoString("AskRet2SelServer()");                    
            //    }
            //}
            //if (countDown-- < 2)
            //{
            //if (lblAlarm.Text.Contains("BHD") && MicroLogin.IsDangCho)
            //{
            //    game.LuaDoString("AskRet2SelServer()");
            //    this.Dispose();
            //}
            //else
            //{
            //        try
            //        {                        
            //            Process.GetProcessById(game.ProcessId).Kill();
            //            //game = null;
            //        }
            //        catch { }
            //    //}
            //}
            //lblExit.Text = string.Format("{0:0}:{1:00}", countDown / 60, countDown % 60);            
        }

        private void Alarm_Load(object sender, EventArgs e)
        {
            if (!Global.Mute)
                Music.Play();
            //cnt++;
            //lblMute.Visible = Global.Mute;
            Height = 25;
            Win.Move(this, Win.WindowLocation.BottomCenter);
        }

        //public static int /*cnt*/ = 0;

        private void lblCharName_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //game.Active();
            }
        }
        private void tmrMute_Tick(object sender, EventArgs e)
        {
            //lblMute.Visible = Global.Mute;
            if (Main.IsMute)
                Opacity = 0;
            else
                Opacity = 75;
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            Global.Mute = !Global.Mute;
            //lblMute.Visible = Global.Mute;
            if (Global.Mute)
            {
                Music.Stop();
            }
            else
            {
                Music.Play();
            }
        }

      
    }
}
