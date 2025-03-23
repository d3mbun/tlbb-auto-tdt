using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace _i
{
    partial class Alam : UserControl
    {
        private Game game;
    
        private int countDown;

        public Alam()
        {
            InitializeComponent();
            this.Disposed += Alam_Disposed;
        }

        public Alam(Game game, string alarm, int countDown)
        {
            if(countDown > 0 && MicroLogin.IsNotOut && !alarm.Contains("kết nối"))
            {
                countDown = 0;
            }
            InitializeComponent();
            this.Disposed += new EventHandler(Alam_Disposed);
            this.game = game;
            if (game.TLBB.Name != "ĐăngNhập")
                lblCharName.Text = game.TLBB.Name;
            else
                lblCharName.Text = game.LastName;
            lblAlarm.Text = alarm;
            if (countDown > 0)
            {
                this.countDown = countDown;
                lblExit.Text = string.Format("{0:0}:{1:00}", countDown / 60, countDown % 60);
                tmrCountDown.Start();
            }
            else
            {
                lblExit.Text = "";
            }
            lblMute.Visible = Global.Mute;
        }


        private void Alam_Disposed(object sender, EventArgs e)
        {
          
            //try
            //{
            //    Parent.Controls.Remove(this);
            //}
            //catch { }
            //if (Alarm.Instance.Controls.Count == 0 && Alarm.Instance.Visible)
            //{
            //    Alarm.Instance.Hide();
            //}
        }

        private void tmrMute_Tick(object sender, EventArgs e)
        {
            lblMute.Visible = Global.Mute;
            if (lblAlarm.Text.Contains("huyệt mộ"))
            {
                if (game.TLBB.MapId != MAP.HuyetMo)
                    Dispose();
            }
        }

        private void tmrCountDown_Tick(object sender, EventArgs e)
        {
            if (game.TLBB.IsSelectServer)
            {
                Dispose();
                return;
            }
      
            if (lblAlarm.Text.Contains("đầy tay nải"))
            {
                if (game.TLBB.IsODaoCuFull && game.TLBB.IsONguyenLieuFull)
                {

                }
                else
                {
                    Dispose();
                }
            }
            if (countDown-- < 3)
            {
                if (countDown <= 0)
                    Dispose();
                if (Global.IsChonMayChuThayChoThoatGame && !lblAlarm.Text.Contains("đơ BHD") && !lblAlarm.Text.Contains("mất kết nối"))
                {
                    if (game.TLBB.Online)
                    {
                        try
                        {
                            //game.DemMam();
                            game.DoStringEx("COUNT = nil;");
                            game.Quit();
                        }
                        catch { }
                    }
                }
                else
                {
                    try
                    {
                        //game.DemMam();
                        Process.GetProcessById(game.ProcessId).Kill();
                    }
                    catch { }
                }
            }        
            //}
            lblExit.Text = string.Format("{0:0}:{1:00}", countDown / 60, countDown % 60);
   
        }

        private void lblAlarm_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                game.Active();
            }
        }

        private void lblCharName_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                game.Active();
            }
        }

        private void lblExit_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (MessageBox.Show(this, "Bạn có muốn thoát " + game.TLBB.Name, "MicroAuto", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    game.Exit();
                }
            }
        }

        private void lblClose_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Dispose();
            }
        }

        private void lblMute_MouseClick(object sender, MouseEventArgs e)
        {
            Global.Mute = !Global.Mute;
            lblMute.Visible = Global.Mute;
            if (Global.Mute)
            {
                Music.Stop();
            }
            else
            {
                Music.Play();
            }
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            Global.Mute = !Global.Mute;
            lblMute.Visible = Global.Mute;
            if (Global.Mute)
            {
                Music.Stop();
            }
            else
            {
                Music.Play();
            }
        }

        private void Alam_Load(object sender, EventArgs e)
        {
            if (!Global.Mute)
                Music.Play();
            lblMute.Visible = Global.Mute;
            Height = 25;
        }
    }
}
