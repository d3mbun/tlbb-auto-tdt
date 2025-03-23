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
    public partial class AutoPK : Form
    {
        public static bool IsPK { get; set; }

        HashSet<string> DropName = new HashSet<string>();

        public AutoPK()
        {
            InitializeComponent();
            Disposed += AutoPK_Disposed;
        }

        private void AutoPK_Disposed(object sender, EventArgs e)
        {
            IsPK = false;
            NamePK = string.Empty;
        }

        private void AutoPK_Load(object sender, EventArgs e)
        {
            Icon = Properties.Resources.icon;
            IsPK = true;
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
                    }
                }
            }
            Win.Move(this, Win.WindowLocation.BottomRight);
        }

     
        public static string NamePK { get; set; } = string.Empty;

        private void button2_Click(object sender, EventArgs e)
        {
            listViewName.Items.Clear();
            DropName.Clear();
            foreach (KeyValuePair<int, Game> kvp in Main.DicGame)
            {
                var game = kvp.Value;
                foreach (GameObject _object in game.Objects.All)
                {
                    if (_object.Name.Trim() == "" || !_object.IsPlayer)
                        continue;
                    if (Game.ListNames.Contains(_object.Name))
                        continue; 
                    if (!DropName.Contains(_object.Name))
                    {
                        DropName.Add(_object.Name);
                        listViewName.Items.Add(_object.Name);
                    }
                }
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            listViewName.Search(txtSearch.Text);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                if (listViewName.SelectedItems.Count > 0)
                {
                    NamePK = listViewName.SelectedItems[0].Text;
                }
                else
                {
                    NamePK = string.Empty;
                }
            }
            else
            {
                NamePK = string.Empty;
            }
            label1.Text = NamePK;
        }

        private void listViewName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                if (listViewName.SelectedItems.Count > 0)
                {
                    NamePK = listViewName.SelectedItems[0].Text;
                }
                else
                {
                    NamePK = string.Empty;
                }
            }
            label1.Text = NamePK;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Dispose();
        }
    }
}
