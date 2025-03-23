using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace _i
{
    public partial class Module : Form
    {
        Process process;
        public Module(Process process)
        {
            InitializeComponent();
            this.process = process;
        }

        private void Module_Load(object sender, EventArgs e)
        {

        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                lvModule.Items.Clear();
                foreach (ProcessModule module in process.Modules)
                {
                    try
                    {
                        lvModule.Items.Add(new ListViewItem(module.FileName));
                    }
                    catch { }
                }
                Text = "Module => " + lvModule.Items.Count;
            }
            catch { }
        }
    }
}
