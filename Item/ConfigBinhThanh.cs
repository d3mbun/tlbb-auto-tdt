using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace _i.Item
{
    public partial class ConfigBinhThanh : UserControl
    {
        public ConfigBinhThanh()
        {
            InitializeComponent();
            Tag = "Cài Đặt Đánh Cọc Binh Thánh";
        }

        private void ConfigBinhThanh_Load(object sender, EventArgs e)
        {
            //Icon = Properties.Resources.icon;
            if(Game.IniParser.Read("Config","BinhThanh").Trim() == string.Empty || !Game.IniParser.Read("Config", "BinhThanh").Contains("["))
            {
                Game.IniParser.Write("Config", "BinhThanh", "[Băng][Hỏa][Nội]");
            }
            foreach(ListViewItem i in listViewEx1.Items)
            {
                if(Game.IniParser.Read("Config", "BinhThanh").Trim().Contains("[" + i.Text + "]"))
                {
                    i.Checked = true;
                }
            }

        }

        private void ConfigBinhThanh_FormClosing(object sender, FormClosingEventArgs e)
        {
            string s = "";
            foreach (ListViewItem i in listViewEx1.Items)
            {
                if (i.Checked)
                {
                    s += "[" + i.Text + "]";
                }
            }
            Game.IniParser.Write("Config", "BinhThanh", s);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string s = "";
            foreach (ListViewItem i in listViewEx1.Items)
            {
                if (i.Checked)
                {
                    s += "[" + i.Text + "]";
                }
            }
            Game.IniParser.Write("Config", "BinhThanh", s);
            Dispose();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Dispose();
        }
    }
}
