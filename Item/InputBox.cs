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
    public partial class InputBox : Form
    {
        public InputBox()
        {
            InitializeComponent();
        }

        public string TextIn { get; set; }

        private void button1_Click(object sender, EventArgs e)
        {
            TextIn = textBoxEx1.Text;
            this.Dispose();
        }

        private void InputBox_Load(object sender, EventArgs e)
        {
            Icon = Properties.Resources.icon;
        }
    }
}
