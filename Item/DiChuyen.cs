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
    public partial class DiChuyen : UserControl
    {
        Game game;
        public DiChuyen()
        {

            InitializeComponent();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            while(lstv.SelectedItems.Count > 0)
            {
                lstv.SelectedItems[0].Remove();
            }
            string dichuyen = string.Empty;
            foreach (ListViewItem item in lstv.Items)
            {
                dichuyen += item.Tag + "|" + item.Text + "|" + item.SubItems[1].Text + "\r\n";
            }
            Game.IniParser.Write("Item", "DiChuyen", dichuyen);
        }

        private void DiChuyen_Load(object sender, EventArgs e)
        {
            game = Main.CurGame;
            if (game != null)
            {
                Tag = "Di Chuyển (" + game.TLBB.Name + ")";
                textBoxEx1.Text = label3.Text = game.TLBB.MapName + " [" + game.RoundX + "," + game.RoundY + "]";
                tag = game.RoundX + "," + game.RoundY + "," + game.TLBB.MapId;
                string map = Game.IniParser.Read("Item", "DiChuyen");
                foreach (string line in map.Split('\n'))
                {
                    string l = line.Trim();
                    if (l.Split('|').Length == 3)
                    {
                        string xy = l.Split('|')[0];
                        string name = l.Split('|')[1];
                        string nicename = l.Split('|')[2];
                        ListViewItem item = new ListViewItem(name);
                        item.SubItems.Add(nicename);
                        item.Tag = xy;
                        lstv.Items.Add(item);
                    }
                }
            }
        }

        string tag;

        private void button1_Click(object sender, EventArgs e)
        {
            if (game == null)
                return;
            textBoxEx1.Text = textBoxEx1.Text.Replace("|", "");
            ListViewItem item = new ListViewItem(label3.Text);
            if (textBoxEx1.Text.Length > 0)
                item.SubItems.Add(textBoxEx1.Text);
            else
                item.SubItems.Add(label3.Text);
            item.Tag = tag;
            lstv.Items.Add(item);
            string dichuyen = string.Empty;
            foreach (ListViewItem i in lstv.Items)
            {
                dichuyen += i.Tag + "|" + i.Text + "|" + i.SubItems[1].Text + "\r\n";
            }
            Game.IniParser.Write("Item", "DiChuyen", dichuyen);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBoxEx1.Text = label3.Text = game.TLBB.MapName + " [" + game.RoundX + "," + game.RoundY + "]";
            tag = game.RoundX + "," + game.RoundY + "," + game.TLBB.MapId;
        }
    }
}
