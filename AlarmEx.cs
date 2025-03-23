using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace _i
{
    partial class AlarmEx : Form
    {
        public AlarmEx()
        {
            InitializeComponent();
            if (Main.IsMute)
            {
                Opacity = 0;
            }
            else
            {
                Opacity = 75;
            }
        }

        public static AlarmEx alarmEx;
        public static AlarmEx Instance
        {
            get
            {
                if (alarmEx == null)
                    alarmEx = new AlarmEx();
                return alarmEx;
            }
        }

        public delegate void PushBack(Game game);

        public static void PushAlarm(object game)
        {
            //if (Instance.InvokeRequired)
            //{
            //    Instance.Invoke(new PushBack(PushAlarm), game);
            //}
            //else
            //{
           
            //}
            var g = game as Game;
            AlarmVaoPhai alarmVaoPhai = new AlarmVaoPhai(g);
            Instance.Controls.Add(alarmVaoPhai);
            Instance.Show();
        }

        private void AlarmEx_Load(object sender, EventArgs e)
        {
            Win.Move(this, Win.WindowLocation.BottomLeft);
            Location = new Point(Location.X, Location.Y - 25);
        }

        private void tmrMonitor_Tick(object sender, EventArgs e)
        {
            if (Main.IsMute)
            {
                Opacity = 0;
            }
            else
            {
                Opacity = 75;
            }
            if (Instance.Controls.Count == 0)
                this.Hide();
            else
                Show();
        }
    }
}
