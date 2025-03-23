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
    partial class BoQua : UserControl
    {
        public BoQua()
        {
            InitializeComponent();
        }

        private void listViewName_DoubleClick(object sender, EventArgs e)
        {
            AddName();
        }

        private void AddName()
        {
            foreach (ListViewItem item in listViewName.SelectedItems)
            {
                if (item.Text.Trim() != "")
                {
                    bool isHave = false;
                    foreach (string str in txtBoQua.Text.Split('\n'))
                    {
                        if (TDT.VietLien(str) == TDT.VietLien(item.Text))
                            isHave = true;
                    }
                    if (!isHave)
                        txtBoQua.Text = txtBoQua.Text.Trim() + "\r\n" + item.Text;
                }
            }
            txtBoQua.ScrollToEnd();
        }

        private void menuAddName_Click(object sender, EventArgs e)
        {
            AddName();
        }

        HashSet<string> DropName = new HashSet<string>();

        private void BoQua_Load(object sender, EventArgs e)
        {
            txtBoQua.Text = SettingOld.BoQua;
            foreach (KeyValuePair<int, Game> kvp in Main.DicGame)
            {
                var game = kvp.Value;
                foreach (GameObject _object in game.Objects.All)
                {
                    if (_object.Name.Trim() == "")
                        continue;
                    if (!DropName.Contains(_object.Name))
                    {
                        DropName.Add(_object.Name);
                        listViewName.Items.Add(_object.Name);
                    }
                }
            }
        }

        private void txtBoQua_TextChanged(object sender, EventArgs e)
        {
            SettingOld.BoQua = txtBoQua.Text;
            SettingOld.SetBoQua();
        }

        private void listViewName_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            listViewName.Search(txtSearch.Text);
        }

        private void listViewName_DockChanged(object sender, EventArgs e)
        {

        }
    }
}
