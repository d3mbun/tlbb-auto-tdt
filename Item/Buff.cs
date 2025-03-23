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
    partial class Buff : UserControl
    {
        public Buff()
        {
            InitializeComponent();
            //Icon = Properties.Resources.icon;
        }
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


        HashSet<string> DropName = new HashSet<string>();
        private void Buff_Load(object sender, EventArgs e)
        {
            cboPlayer.Items.Clear();
            cboPlayer.Items.Add(new ComboboxValue("0000FFFF", "Thiết Lập - Toàn Bộ"));
            Main.Instance.AllOnelineGame.ToList().ToList().ForEach(game => cboPlayer.Items.Add(new ComboboxValue(game.TLBB.Id, "Thiết Lập - " + game.TLBB.Name)));
            cboPlayer.SelectedIndex = 0;

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

        private void txtLst_TextChanged(object sender, EventArgs e)
        {
            if (Tag.ToString() == "Buff - All")
            {
                SettingOld.IdBuff = txtLst.Text;
            }
            else
            {
                Game.IniParser.Write("Buff", TDT.GetAllowName(Tag.ToString().Replace("Buff - ", "")), txtLst.Text);
                Main.DicGame.ToList().Select(kvp => kvp.Value).ToList().ForEach
                    (g => {
                        if (g.TLBB.AllowName == TDT.GetAllowName(Tag.ToString().Replace("Buff - ", "")))
                            g.IdBuff = txtLst.Text; });
            }
        }

        private void txtLst_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
                txtLst.SelectAll();
        }

 

        private void cboPlayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            var item = cboPlayer.SelectedItem as ComboboxValue;
            var id = item.Id;



            if (id == "0000FFFF")
            {
                Tag = "Buff - All";
                txtLst.Text = Game.IniParser.Read("Buff", "All");                
            }
            else
            {
                foreach (KeyValuePair<int, Game> kvp in Main.DicGame)
                {
                    if (kvp.Value.TLBB.Id == id)
                    {
                        var game = kvp.Value;
                        Tag = "Buff - " + game.TLBB.Name;
                        txtLst.Text = Game.IniParser.Read("Buff", game.TLBB.AllowName);
                    }
                }
            }
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
                        if (TDT.VietLien(str) == TDT.VietLien(item.Text))
                            isHave = true;
                    }
                    if (!isHave)
                        txtLst.Text = txtLst.Text.Trim() + "\r\n" + item.Text;
                }
            }
            txtLst.ScrollToEnd();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            foreach (KeyValuePair<int, Game> kvp in Main.DicGame)
            {
                //32-37
                if (!txtLst.Text.Contains(kvp.Value.TLBB.Name))
                {
                    txtLst.Text = txtLst.Text + "\r\n" + kvp.Value.TLBB.Name;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void cboPlayer_DropDown(object sender, EventArgs e)
        {
            cboPlayer.Items.Clear();
            cboPlayer.Items.Add(new ComboboxValue("0000FFFF", "Thiết Lập - Toàn Bộ"));
            Main.Instance.AllOnelineGame.ToList().ToList().ForEach(game => cboPlayer.Items.Add(new ComboboxValue(game.TLBB.Id, "Thiết Lập - " + game.TLBB.Name)));

        }
    }
}
