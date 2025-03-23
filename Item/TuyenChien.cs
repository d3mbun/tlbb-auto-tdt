using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace _i
{
    partial class TuyenChien : UserControl
    {
        public TuyenChien()
        {
            InitializeComponent();
        }

        HashSet<string> DropName = new HashSet<string>();

        private void AskPK_Load(object sender, EventArgs e)
        {
            txtName.Text = SettingOld.PkName;
            txtGuildName.Text = SettingOld.PkGuild;
            txtNameIgnore.Text = SettingOld.PkIgnore;
            foreach (KeyValuePair<int, Game> kvp in Main.DicGame)
            {
                var game = kvp.Value;
                foreach (GameObject _object in game.Objects.All)
                {
                    if (_object.Name.Trim() == "" || !_object.IsPlayer)
                        continue;
                    if (!DropName.Contains(_object.Name))
                    {
                        DropName.Add(_object.Name);
                        listViewName.Items.Add(_object.Name);
                        listViewIgnore.Items.Add(_object.Name);
                    }                    
                }               
            }
            DropName.Clear();
            foreach (KeyValuePair<int, Game> kvp in Main.DicGame)
            {
                var game = kvp.Value;
                foreach (GameObject _object in game.Objects.All)
                {
                    if (_object.GuildName.Trim() == "" || !_object.IsPlayer)
                        continue;
                    if (!DropName.Contains(_object.GuildName))
                    {
                        DropName.Add(_object.GuildName);
                        listViewGuildName.Items.Add(_object.GuildName);
                    }
                }
            }         
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
                    foreach (string str in txtName.Text.Split('\n'))
                    {
                        if (TDT.VietLien(str) == TDT.VietLien(item.Text))
                            isHave = true;
                    }
                    if (!isHave)
                    {
                        txtName.Text = txtName.Text.Trim() + "\r\n" + item.Text;
                    }
                }
            }
            txtName.ScrollToEnd();
        }

        private void AddGuild()
        {
            foreach (ListViewItem item in listViewGuildName.SelectedItems)
            {
                if (item.Text.Trim() != "")
                {
                    bool isHave = false;
                    foreach (string str in txtGuildName.Text.Split('\n'))
                    {
                        if (TDT.VietLien(str) == TDT.VietLien(item.Text))
                            isHave = true;
                    }
                    if (!isHave)
                    {
                        txtGuildName.Text = txtGuildName.Text.Trim() + "\r\n" + item.Text;
                    }
                }
            }
            txtGuildName.ScrollToEnd();
        }

        private void listViewGuildName_DoubleClick(object sender, EventArgs e)
        {
            AddGuild();
        }

        private void AddInogre()
        {
            foreach (ListViewItem item in listViewIgnore.SelectedItems)
            {
                if (item.Text.Trim() != "")
                {
                    bool isHave = false;
                    foreach (string str in txtNameIgnore.Text.Split('\n'))
                    {
                        if (TDT.VietLien(str) == TDT.VietLien(item.Text))
                            isHave = true;
                    }
                    if (!isHave)
                    {
                        txtNameIgnore.Text = txtNameIgnore.Text.Trim() + "\r\n" + item.Text;
                    }
                }
            }
            txtNameIgnore.ScrollToEnd();
        }

        private void listViewIgnore_DoubleClick(object sender, EventArgs e)
        {
            AddInogre();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            SettingOld.PkName = txtName.Text;
            SettingOld.PkGuild = txtGuildName.Text;
            SettingOld.PkIgnore = txtNameIgnore.Text;
            SettingOld.SetBoQua();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Dispose();
        }

     

        private void txtName_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
