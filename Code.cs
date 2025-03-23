using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace _i
{
    public partial class Code : Form
    {
        Game game;
        public Code()
        {
            if (Main.CurGame != null)
                game = Main.CurGame;
            InitializeComponent();
        }

        private void btnCode_Click(object sender, EventArgs e)
        {
            string code = txtCode.Text;
            foreach(string c in code.Split('\n'))
            {
                string cod = c.Trim();
                if(cod.Length > 5)
                {
                    ListViewItem item = new ListViewItem(cod);
                    item.SubItems.Add("wait");
                    listViewCode.Items.Add(item);
                }
            }
        }

        private void Code_Load(object sender, EventArgs e)
        {
            Icon = Properties.Resources.icon;
            Text = game.TLBB.Name;
        }

 

        private void tmrCode_Tick(object sender, EventArgs e)
        {
            if (game == null)
                return;
            bool iscode = false;
            foreach(ListViewItem item in listViewCode.Items)
            {
                string code = item.Text;
                string status = item.SubItems[1].Text;
                if(status == "wait")
                {
                    item.SubItems[1].Text = "done";
                    game.DoStringEx("setmetatable(_G, {__index = NewbieCardActivation_Env}); NewUserCard_YDPrize_OpenClick('" + code + "')");
                    iscode = true;
                    break;
                }                
            }
            if(!iscode && (User.Email == "tieudattai@yahoo.com" || User.Email == "lecaotri@yahoo.com"))
            {
                string ran = RandomCode();
                game.DoStringEx("setmetatable(_G, {__index = NewbieCardActivation_Env}); NewUserCard_YDPrize_OpenClick('" + ran + "')");
                //Main.PushLogEx(ran);
            }
        }

        public static string RandomCode()
        {
            return "q0" + RandomString(16);
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "qwertyuiopasdfghjklzxcvbnm01234567890012345678900123456789001234567890012345678900123456789001234567890";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            tmrCode.Interval = 1000 * (int)numericUpDown2.Value;
        }
    }
}
