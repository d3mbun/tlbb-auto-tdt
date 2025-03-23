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
    public partial class TextEdit : UserControl
    {
        public TextEdit()
        {
            InitializeComponent();
        }

        public override string Text
        {
            get
            {
                return text.Text;
            }
            set
            {
                text.Text = value;
            }
        }
        new void Resize()
        {
            Height = text.Height;
            if (lbSearch.Visible)
                Height = Height + lbSearch.Height;
        }


        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            text.Search(txtSearch.Text);

            // get the keyword to search
            string textToSearch = txtSearch.Text.VietLien();
            lbSearch.Visible = false; // hide the listbox, see below for why doing that
            if (String.IsNullOrEmpty(textToSearch))
                return; // return with listbox's Visible set to false if the keyword is empty
                        //search
            string[] result = (from i in text.Lines
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
            txtSearch.Text = lbSearch.Text;
            lbSearch.Visible = false;
            Resize();
        }
    }
}
