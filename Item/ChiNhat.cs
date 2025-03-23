using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;

namespace _i
{
    partial class FrmLocdoOpt : UserControl
    {
        Game game;
        public FrmLocdoOpt(Game game)
        {
            this.game = game;
            InitializeComponent();
            //Icon = Properties.Resources.icon;
        }

        private void txtLst_TextChanged(object sender, EventArgs e)
        {
            game.ChiNhat = txtLst.Text;
        }

        private void FrmLocdoOpt_Load(object sender, EventArgs e)
        {
            List<string> DropName = new List<string>();
            txtLst.Text = game.ChiNhat;
            lvName.Items.AddRange(PacketItem.AllName.Select(i => new ListViewItem(i)).ToArray());
            Tag = "Chỉ Nhặt - OnlyPick - " + game.TLBB.Name;
        }

 

        private void listViewName_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void AddName()
        {
            foreach (ListViewItem item in lvName.SelectedItems)
            {
                if (item.Text.Trim() != "")
                {
                    bool isHave = false;
                    foreach (string str in txtLst.Text.Split('\n'))
                    {
                        if (TDT.VietLien(str) == TDT.VietLien(item.Text))
                            isHave = true;
                    }
                    if (!isHave)
                        txtLst.Text = txtLst.Text.Trim() + "\r\n" + item.Text;
                }
            }

        }

        private void listViewName_DoubleClick(object sender, EventArgs e)
        {
            AddName();
        }

        private void menuName_Opening(object sender, CancelEventArgs e)
        {

        }

        private void menuAddName_Click(object sender, EventArgs e)
        {
            AddName();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            lvName.Search(txtSearch.Text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Dispose();
        }
    }
}
