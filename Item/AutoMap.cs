using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace _i
{
    partial class AutoMap : UserControl
    {
        private Game game;
        public AutoMap(Game game)
        {
            this.game = game;    
            InitializeComponent();
            lv.Columns[0].Text = game.TLBB.MapName + " [" + game.TLBB.Name + "]";
            checkBox1.Checked = game.IsRunAutoMap;
            Disposed += (ss, ee) =>
            {
                game.IsRunAutoMap = false;
            };
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string txt = ((int)game.CharX).ToString() + "," + ((int)game.CharY).ToString();
            if (lv.Items.Count > 0 && lv.Items[lv.Items.Count - 1].Text == txt)
                return;
            ListViewItem newItem = new ListViewItem(txt);
            lv.Items.Add(newItem);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string txt = string.Empty;
            foreach (ListViewItem item in lv.Items)
            {
                txt += item.Text + "-";
            }
            txt = txt.Trim('-');
            SettingOld.SaveMAP(game.TLBB.MapId.ToString(), txt);
        }

        private void AutoMap_Load(object sender, EventArgs e)
        {
            string txt = SettingOld.LoadMAP(game.TLBB.MapId.ToString());
            if (Global.IsAdminEx)
            {
                string point = game.RoundX + "," + game.RoundY + "\r\n";
                point += (game.RoundX - 5) + "," + (game.RoundY + 6) + "\r\n";
                point += (game.RoundX +  5) + "," + (game.RoundY + 6) + "\r\n";
                point += (game.RoundX + 8) + "," + (game.RoundY + 0) + "\r\n";
                point += (game.RoundX + 5) + "," + (game.RoundY - 6) + "\r\n";
                point += (game.RoundX - 5) + "," + (game.RoundY - 6) + "\r\n";
                point += (game.RoundX - 8) + "," + (game.RoundY + 0) + "\r\n";
                Clipboard.SetText(point);
            }
            foreach (string s in txt.Split('-'))
            {
                int x = 0;
                int y = 0;
                try
                {
                    x = TDT.ParseInt(s.Split(',')[0]);
                    y = TDT.ParseInt(s.Split(',')[1]);
                }
                catch { }
                if (x != 0 && y != 0)
                {
                    lv.Items.Add(new ListViewItem(x + "," + y));
                }
            }
        }

        private void menuDelete_Click(object sender, EventArgs e)
        {
            while (lv.SelectedItems.Count > 0)
            {
                lv.Items.Remove(lv.SelectedItems[0]);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
           game.IsRunAutoMap = checkBox1.Checked;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Dispose();
        }
    }
}
