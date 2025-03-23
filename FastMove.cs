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
    public partial class FastMove : Form
    {
        public FastMove()
        {
            InitializeComponent();
            Icon = Properties.Resources.icon;
        }

        private void FastMove_Load(object sender, EventArgs e)
        {
            foreach(KeyValuePair<int,string> kvp in GAMEDIC.MapNameId)
            {
                ListViewItem item = new ListViewItem(kvp.Value);
                item.Tag = kvp.Key;
                item.SubItems.Add(kvp.Key.ToString());
                listViewEx1.Items.Add(item);
            }
        }

        private void diChuyểnToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            if (listViewEx1.SelectedItems.Count > 0)
            {
                try
                {
                    myKey = (int)listViewEx1.SelectedItems[0].Tag;
                    Main.Instance.SelectedOnlinedGames.ForEach(g => g.GoToEx(100, 100, myKey));
                    Text = "=>" + listViewEx1.SelectedItems[0].Text;
                }
                catch { }
            }
        }
        int myKey = -1;
        private void textBoxEx1_KeyDown(object sender, KeyEventArgs e)
        {
            listViewEx1.Search(textBoxEx1.Text);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (myKey == -1)
                return;
            Main.Instance.SelectedOnlinedGames.ForEach(g => { if (g.TLBB.MapId != myKey) g.GoToEx(100, 100, myKey); });
        }
    }
}
