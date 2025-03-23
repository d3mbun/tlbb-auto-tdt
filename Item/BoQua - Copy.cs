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
    partial class BoQuaEx : Form
    {
        public BoQuaEx()
        {
            InitializeComponent();
        }


        private void BoQua_Load(object sender, EventArgs e)
        {
            Icon = Properties.Resources.icon;
            txtBoQua.Text = SettingOld.BoQuaEx;
           
        }

        private void txtBoQua_TextChanged(object sender, EventArgs e)
        {
            
          
        }

        private void listViewName_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void BoQuaEx_FormClosing(object sender, FormClosingEventArgs e)
        {

            SettingOld.BoQuaEx = txtBoQua.Text;

            string block = "";

            foreach (string s in SettingOld.BoQuaEx.Split('\n'))
            {
                if (s.Trim().Length >= 3)
                {
                    block += s.Trim() + "#";
                }
            }
            if (block.Length > 3)
            {
                Main.DicGame.ToList().ForEach(kvp => { var adr = kvp.Value.Memory.WriteString(block); kvp.Value.PostMessage(1, 988); kvp.Value.PostMessage(adr, 987); });
            }
            else
            {
                Main.DicGame.ToList().ForEach(kvp => { var adr = kvp.Value.Memory.WriteString(block); kvp.Value.PostMessage(0, 988); kvp.Value.PostMessage(adr, 987); });
            }

        }
    }
}
