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
    public partial class GameInfo : UserControl
    {
        public GameInfo()
        {
            InitializeComponent();
        }

        public float HPPercent
        {
            set
            {
                picHP.Width = (int)(picHP.Parent.Width * value);
            }
        }

        public string MapName
        {
            set
            {
                label1.Text = value;
            }
        }

        public float MPPercent
        {
            set
            {
                picMP.Width = (int)(picMP.Parent.Width * value);
                //label3.Text = "";
            }
        }

        public float PetPercent
        {
            set
            {
                picPet.Width = (int)(picPet.Parent.Width * value);
                //label2.Text = "";
            }
        }

        public float ExpPercent
        {
            set
            {
                picExp.Width = (int)(picExp.Parent.Width * value);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }

    
}
