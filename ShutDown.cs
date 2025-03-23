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
    partial class ShutDown : Form
    {
        public ShutDown()
        {
            InitializeComponent();

            FormBorderStyle = FormBorderStyle.None;

            this.Disposed += new EventHandler(ShutDown_Disposed);

            this.Show();
        }

        void ShutDown_Disposed(object sender, EventArgs e)
        {
            Music.Stop();
        }

        private void lblClose_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                this.Dispose();
        }

        private int exitTime = 60;

        private void tmrCountDown_Tick(object sender, EventArgs e)
        {
            //if (Main.ListView.Items.Count > 0)
            //    this.Dispose();
            exitTime--;

            Music.Play();

            TimeSpan span = TimeSpan.FromSeconds(exitTime);

            lblExitTime.Text = "Tắt máy sau " + string.Format("{0:0}:{1:00}", span.Minutes, span.Seconds);

            if(exitTime == 0)
                Process.Start("shutdown", "-s -t 0");
        }

        private void ShutDown_Load(object sender, EventArgs e)
        {
            Win.Move(this, Win.WindowLocation.TopLeft);
        }
    }
}
