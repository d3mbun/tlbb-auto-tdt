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
    public partial class BachBaoCac : UserControl
    {
        public BachBaoCac()
        {
            InitializeComponent();
        }

        private void BachBaoCac_Load(object sender, EventArgs e)
        {
            cboPlayer.Items.Clear();
            cboPlayer.Items.Add(new ComboboxValue("0000FFFF", "Thiết Lập - Toàn Bộ"));
            Main.Instance.AllOnelineGame.ToList().ForEach(game => cboPlayer.Items.Add(new ComboboxValue(game.TLBB.Id, "Thiết Lập - " + game.TLBB.Name)));
            cboPlayer.SelectedIndex = 0;
        }

        private void cboPlayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            IsHold = true;
            lvItems.Items.Cast<ListViewItem>().ToList().ForEach(i => i.Checked = false);
            var item = cboPlayer.SelectedItem as ComboboxValue;
            var id = item.Id;


            string note = "";

            if (id == "0000FFFF")
            {
                Text = "Mua Bách Bảo - All";
                note = Game.IniParser.Read("BachBao", "All") + "*" + Game.IniParser.Read("QuyThi", "All");
            }
            else
            {
                foreach (KeyValuePair<int, Game> kvp in Main.DicGame)
                {
                    if (kvp.Value.TLBB.Id == id)
                    {
                        var game = kvp.Value;
                        Text = "Mua Bách Bảo - " + game.TLBB.Name;
                        note = Game.IniParser.Read("BachBao", game.TLBB.AllowName) + "*" + Game.IniParser.Read("QuyThi", game.TLBB.AllowName);
                    }
                }
            }
            foreach (string nn in note.Split('*'))
            {
                string n = nn.Trim();
                if (n.Length > 5)
                {
                    lvItems.Items.Cast<ListViewItem>().ToList().ForEach(i => { if (i.Text == n) i.Checked = true; });
                }
            }
            IsHold = false;
        }

        bool IsHold { get; set; }

        private void lvItems_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (IsHold)
                return;
            string note = "";
            foreach (ListViewItem i in lvItems.Items.Cast<ListViewItem>().ToList().Concat(lvItems.hideItems))
            {
                if (i.Text.Contains("Quỷ Thị"))
                    continue;
                if (i.Checked)
                {
                    note += "*" + i.Text;
                }
            }

            if (Text.Contains(" - All"))
            {
                Game.IniParser.Write("BachBao", "All", note);
            }
            else
            {
                Game.IniParser.Write("BachBao", TDT.GetAllowName(Text.Replace("Mua Bách Bảo - ", "")), note);
            }
            note = "";
            foreach (ListViewItem i in lvItems.Items.Cast<ListViewItem>().ToList().Concat(lvItems.hideItems))
            {
                if (!i.Text.Contains("Quỷ Thị"))
                    continue;
                if (i.Checked)
                {
                    note += "*" + i.Text;
                }
            }

            if (Text.Contains(" - All"))
            {
                Game.IniParser.Write("QuyThi", "All", note);
            }
            else
            {
                Game.IniParser.Write("QuyThi", TDT.GetAllowName(Text.Replace("Mua Bách Bảo - ", "")), note);
            }
        }

        private void textBoxEx1_TextChanged(object sender, EventArgs e)
        {
            lvItems.Search(textBoxEx1.Text);
        }
    }
}
