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
    partial class SellItemPS:UserControl
    {
        public SellItemPS()
        {
            InitializeComponent();
        }

        private void SellItemPS_Load(object sender, EventArgs e)
        {
            lvName.Items.AddRange(PacketItem.AllName.Select(i => new ListViewItem(i)).ToArray());
            foreach (string s in SettingOld.SellItemPS.Split('\n'))
            {
                if(s.Trim().Length > 3 && s.Trim().Split('|').Length == 2)
                {
                    ListViewItem i = new ListViewItem(s.Trim().Split('|')[0]);
                    i.SubItems.Add(s.Trim().Split('|')[1]);
                    lvPrice.Items.Add(i);
                }
            }
        }


        private void listViewName_DoubleClick(object sender, EventArgs e)
        {
            if (lvName.SelectedItems.Count > 0 && TDT.ParseAllInt(txtPrice.Text) > 0)
            {
                string itemname = lvName.SelectedItems[0].Text;
                foreach (ListViewItem i in lvPrice.Items)
                {
                    if(i.Text == itemname)
                    {
                        i.SubItems[1].Text = txtPrice.Text;
                        return;
                    }
                }
                ListViewItem it = new ListViewItem(itemname);
                it.SubItems.Add(txtPrice.Text);
                lvPrice.Items.Add(it);
            }
        }

     

    

        private void txtSearchName_TextChanged(object sender, EventArgs e)
        {
            lvName.Search(txtSearchName.Text);
        }

        private void SellItemPS_FormClosing(object sender, FormClosingEventArgs e)
        {
            string sellitemps = "";
            foreach(ListViewItem i in lvPrice.Items)
            {
                sellitemps += i.Text + "|" + i.SubItems[1].Text + "\n";
            }
            SettingOld.SellItemPS = sellitemps;
        }

        private void lvPrice_DoubleClick(object sender, EventArgs e)
        {
            while(lvPrice.SelectedItems.Count > 0)
            {
                lvPrice.SelectedItems[0].Remove();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listViewName_DoubleClick(null, null);
        }
    }
}
