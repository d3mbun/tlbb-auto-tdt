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
    partial class BuyItem : UserControl
    {
        public BuyItem()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            int num = TDT.ParseInt(txtNum.Text);
            if(num <= 0)
            {
                MessageBox.Show(this, "Vui lòng điền số lượng cần mua", "MicroAuto");
                return;
            }
            if(txtName.Text.Trim().Length == 0)
            {
                MessageBox.Show(this, "Vui lòng điền tên vật phẩm cần mua", "MicroAuto");
                return;
            }
            bool isHave = false;
            foreach (ListViewItem i in lvBuyName.Items)
            {
                if (i.Text == txtName.Text)
                {
                    i.SubItems[1].Text = TDT.ParseInt(txtNum.Text).ToString();
                    isHave = true;
                }
            }
            if (!isHave)
            {
                ListViewItem item = new ListViewItem(txtName.Text);
                item.SubItems.Add(txtNum.Text);
                lvBuyName.Items.Add(item);
            }
            Save();
        }

        private void BuyItem_Load(object sender, EventArgs e)
        {
            lvName.Items.AddRange(PacketItem.AllName.Select(i => new ListViewItem(i)).ToArray());

            cboPlayer.Items.Clear();
            cboPlayer.Items.Add(new ComboboxValue("0000FFFF", "Thiết Lập - Toàn Bộ"));
            Main.Instance.AllOnelineGame.ToList().ForEach(game => cboPlayer.Items.Add(new ComboboxValue(game.TLBB.Id, "Thiết Lập - " + game.TLBB.Name)));
            cboPlayer.SelectedIndex = 0;
            tabPage2.Controls.Add(new BachBaoCac() { Dock = DockStyle.Fill });
        }

        private void listViewType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvName.SelectedItems.Count > 0)
                txtName.Text = lvName.SelectedItems[0].Text;
        }

    
        private void BuyItem_FormClosing(object sender, FormClosingEventArgs e)
        {
      
          
        }

        private void listViewName_DoubleClick(object sender, EventArgs e)
        {
            while(lvBuyName.SelectedItems.Count > 0)
            {
                lvBuyName.SelectedItems[0].Remove();
            }
            Save();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            lvName.Search(txtSearch.Text);
        }

        private void cboPlayer_DropDown(object sender, EventArgs e)
        {
            cboPlayer.Items.Clear();
            cboPlayer.Items.Add(new ComboboxValue("0000FFFF", "Thiết Lập - Toàn Bộ"));
            Main.Instance.AllOnelineGame.ToList().ForEach(game => cboPlayer.Items.Add(new ComboboxValue(game.TLBB.Id, "Thiết Lập - " + game.TLBB.Name)));
        }

        void Save()
        {
            string note = "";
            foreach (ListViewItem it in lvBuyName.Items)
            {
                note += it.Text + "|" + it.SubItems[1].Text + ";";
            }
            note = note.Trim(';');

            var item = cboPlayer.SelectedItem as ComboboxValue;
            var id = item.Id;
            if (id == "0000FFFF")
            {
                Text = "Mua Đồ - BuyItem - All";
                SettingOld.BuyIts = note;
            }
            else
            {
                foreach (KeyValuePair<int, Game> kvp in Main.DicGame)
                {
                    if (kvp.Value.TLBB.Id == id)
                    {
                        var game = kvp.Value;
                        Text = "Mua Đồ - BuyItem - " + game.TLBB.Name;
                        game.BuyIts = note;
                        break;
                    }
                }
            }
        }

        private void cboPlayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            lvBuyName.Items.Clear();
            var item = cboPlayer.SelectedItem as ComboboxValue;
            var id = item.Id;
            string note = "";
            if (id == "0000FFFF")
            {
                Text = "Mua Đồ - BuyItem - All";
                note = SettingOld.BuyIts;
            }
            else
            {
                foreach (KeyValuePair<int, Game> kvp in Main.DicGame)
                {
                    if (kvp.Value.TLBB.Id == id)
                    {
                        var game = kvp.Value;
                        Text = "Mua Đồ - BuyItem - " + game.TLBB.Name;
                        note = game.BuyIts;
                        break;
                    }
                }
            }

            foreach (string name in note.Split(';'))
            {
                if (name.Split('|').Length >= 2)
                {
                    lvBuyName.Items.Add(new ListViewItem(new string[] { name.Split('|')[0], name.Split('|')[1] }));
                }
            }
        }

        private void lvBuyName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvBuyName.SelectedItems.Count > 0)
                txtName.Text = lvBuyName.SelectedItems[0].Text;
        }

        private void lvName_DoubleClick(object sender, EventArgs e)
        {
            if (lvName.SelectedItems.Count == 0)
                return;
            txtName.Text = lvName.SelectedItems[0].Text;
            int num = TDT.ParseInt(txtNum.Text);
            if (num <= 0)
            {
                return;
            }
            if (txtName.Text.Trim().Length == 0)
            {                
                return;
            }
            bool isHave = false;
            foreach (ListViewItem i in lvBuyName.Items)
            {
                if (i.Text == txtName.Text)
                {
                    i.SubItems[1].Text = TDT.ParseInt(txtNum.Text).ToString();
                    isHave = true;
                }
            }
            if (!isHave)
            {
                ListViewItem item = new ListViewItem(txtName.Text);
                item.SubItems.Add(txtNum.Text);
                lvBuyName.Items.Add(item);
            }
            Save();
        }
    }
}
