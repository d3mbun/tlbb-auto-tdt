using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Text.RegularExpressions;
using System.Web;

namespace _i
{
    partial class Loader : Form
    {
        public Loader(string msg)
        {
            InitializeComponent();
            lblMsg.Text = msg;            

            this.FormBorderStyle = FormBorderStyle.None;
            tmr.Start();
        }

        public void Init()
        {
            this.Height = 25;
            lblClose.Visible = false;
            Win.Move(this, Win.WindowLocation.BottomCenter);
        }

        public new void Show()
        {
            Win.ShowInactiveTopmost(this);
            Init();
            //your code here

            //call the shadowed Show method on our form.       
            //base.Show();

        }

        public Loader(int cmd, string msg)
        {
            InitializeComponent();
            lblMsg.Text = msg;

            this.FormBorderStyle = FormBorderStyle.None;
            tmrEx.Start();
        }
        //lecaotri
        private string SaveUrl = "http://www.tieudattai.info/microauto/user.php";

        public  void SaveSettings()
        {
            Poster posterSave = new Poster();
            posterSave.Control = this;
            posterSave.Url = SaveUrl;
            if (Global.OFFSET != "")
                posterSave.Data = "cmd=logout&settings=" + "Test" + "&email=" + HttpUtility.UrlEncode(User.Email);
            posterSave.Completed += new EventHandler(posterSave_Completed);
            posterSave.Post();
        }

        void posterSave_Completed(object sender, EventArgs e)
        {
            Poster posterSave = (Poster)sender;
            if (posterSave.IsError)
                SaveSettings();
            else
                this.Dispose();
        }

        private void tmr_Tick(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void lblClose_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                this.Dispose();
        }

        private void Loader_Load(object sender, EventArgs e)
        {
            this.Height = 25;
            lblClose.Visible = false;
            Win.Move(this, Win.WindowLocation.BottomCenter);
        }

        private void tmrEx_Tick(object sender, EventArgs e)
        {
            if (Main.IsSaved)
                Dispose();
        }
    }
}
