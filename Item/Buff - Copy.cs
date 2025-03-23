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
    partial class BuffBienThan : UserControl
    {
        public BuffBienThan()
        {
            InitializeComponent();
        }

        HashSet<string> DropName = new HashSet<string>();
        class ComboboxValue
        {
            public string Id { get; private set; }
            public string Name { get; private set; }

            public ComboboxValue(string id, string name)
            {
                Id = id;
                Name = name;
            }

            public override string ToString()
            {
                return Name;
            }
        }
        private void Buff_Load(object sender, EventArgs e)
        {
            txtLst.Text = SettingOld.IdBienThan;
            //cboPlayer.Items.Clear();
            //cboPlayer.Items.Add(new ComboboxValue("0000FFFF", "Thiết Lập - Toàn Bộ"));
            //Main.Instance.AllOnelineGame.ForEach(game => cboPlayer.Items.Add(new ComboboxValue(game.TLBB.Id, "Thiết Lập - " + game.TLBB.Name)));
            //cboPlayer.SelectedIndex = 0;
            foreach (var p in Game.AllPlayer)
            {
                listViewName.Items.Add(p.Name);
            }
        }

        private void txtLst_TextChanged(object sender, EventArgs e)
        {
            SettingOld.IdBienThan = txtLst.Text;
        }

        private void txtLst_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
                txtLst.SelectAll();
        }

   

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            listViewName.Search(txtSearch.Text);
        }

        private void listViewName_DoubleClick(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listViewName.SelectedItems)
            {
                if (item.Text.Trim() != "")
                {
                    bool isHave = false;
                    foreach (string str in txtLst.Text.Split('\n'))
                    {
                        if (str.Trim() == item.Text.Trim())
                        {
                            isHave = true;
                            break;
                        }
                    }
                    if (!isHave)
                        txtLst.Text = txtLst.Text.Trim() + "\r\n" + item.Text;
                }
            }
            txtLst.ScrollToEnd();
        }
    }
}
