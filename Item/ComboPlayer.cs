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
    public partial class ComboPlayer : UserControl
    {
        public ComboPlayer()
        {
            InitializeComponent();
            Resize();
        }

        new void Resize()
        {
            Height = cboPlayer.Height;
            if (lbSearch.Visible)
                Height = Height + lbSearch.Height;
        }

        public override string Text
        {
            get
            {
                return cboPlayer.Text;
            }
        }

        private void cboPlayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbSearch.Visible = false;
            Resize();
        }

        private void cboPlayer_TextChanged(object sender, EventArgs e)
        {
            var combobox = sender as ComboBox;
            // get the keyword to search
            string textToSearch = combobox.Text.VietLien();
            lbSearch.Visible = false; // hide the listbox, see below for why doing that
            if (String.IsNullOrEmpty(textToSearch))
                return; // return with listbox's Visible set to false if the keyword is empty
                        //search
            string[] result = (from i in Main.Instance.AllOnelineGame.Select(o => o.TLBB.Name)
                               where i.VietLien().Contains(textToSearch)
                               select i).ToArray();
            if (result.Length == 0)
                return; // return with listbox's Visible set to false if nothing found

            lbSearch.Items.Clear(); // remember to Clear before Add
            lbSearch.Items.AddRange(result);
            lbSearch.Visible = true; // show the listbox again
            Resize();
        }

        private void lbSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboPlayer.Text = lbSearch.SelectedItem.ToString();
            lbSearch.Visible = false;
            Resize();
        }

        private void cboPlayer_DropDown(object sender, EventArgs e)
        {
            cboPlayer.Items.Clear();
            cboPlayer.Items.Add("");
            cboPlayer.Items.AddRange(Main.Instance.AllOnelineGame.Select(o => o.TLBB.Name).ToArray());
        }
    }
}
