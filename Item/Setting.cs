using ChreneLib.Controls.TextBoxes;
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
    public partial class Setting : UserControl
    {
        public static Dictionary<string, List<string>> Teams { get; set; } = new Dictionary<string, List<string>>();

        public static Dictionary<string, List<string>> TeamTyVo { get; set; } = new Dictionary<string, List<string>>();

        public static IEnumerable<string> TeamLeader => Teams.Values.Select(k => k.FirstOrDefault());
        public static string DropItem { get; set; } = string.Empty;
        public static string SellItem { get; set; } = string.Empty;
        public static string UseItem { get; set; } = string.Empty;
        public static string BuyItem { get; set; } = string.Empty;
        public static string Banktem { get; set; } = string.Empty;
        public static string ThienCotem { get; set; } = string.Empty;
        public static string GomItem { get; set; } = string.Empty;
        public static string GomItemKNB { get; set; } = string.Empty;
        public static string LayItem { get; set; } = string.Empty;
        public static string LayItemThienCo { get; set; } = string.Empty;


        public static Dictionary<string, bool> DicChecked { get; set; } = new Dictionary<string, bool>();

        public static Dictionary<string, int> DicNumber { get; set; } = new Dictionary<string, int>();

        public static bool Is(string name)
        {
            return DicChecked.ContainsKey(name) ? DicChecked[name] : false;
        }

        public static int Value(string name)
        {
            return DicNumber.ContainsKey(name) ? DicNumber[name] : -1;
        }

        public Setting()
        {
            InitializeComponent();
        }

        public new void Load()
        {
            foreach (var con in this.GetDescendants())
            {
                try
                {
                    string value = Game.IniParser.Read("SettingValue", con.Name.ToString());
                    if (!string.IsNullOrEmpty(value))
                    {
                        if (con.GetType() == typeof(NumericUpDown))
                        {
                            ((NumericUpDown)con).Value = value.ToInt();
                        }
                        if (con.GetType() == typeof(TextBoxEx))
                        {
                            ((TextBoxEx)con).Text = value;
                            DicNumber[con.Name] = ((TextBoxEx)con).Text.ToNumber();
                        }
                        if (con.GetType() == typeof(ComboBox))
                        {
                            ((ComboBox)con).SelectedIndex = value.ToInt();
                        }
                        if (con.GetType() == typeof(CheckBox))
                        {                            
                            ((CheckBox)con).Checked = value == "True";
                            DicChecked[con.Name] = ((CheckBox)con).Checked;
                            ((CheckBox)con).CheckedChanged += (ss, ee) =>
                            {
                                DicChecked[con.Name] = ((CheckBox)con).Checked;
                            };
                        }
                        if (con.GetType() == typeof(ColorSlider))
                        {
                            DicNumber[con.Name] = ((ColorSlider)con).Value.ToNumber();
                            ((ColorSlider)con).Value = value.ToInt();
                            ((ColorSlider)con).ValueChanged += (ss, ee) =>
                            {
                                DicNumber[con.Name] = ((ColorSlider)con).Value.ToNumber();
                            };
                        }
                    }
                }
                catch { }
            }
        }

        public void Save()
        {
            foreach (var con in this.GetDescendants())
            {
                try
                {
                    if (con.GetType() == typeof(NumericUpDown))
                        Game.IniParser.Write("SettingValue", con.Name, ((NumericUpDown)con).Value.ToString());
                    if (con.GetType() == typeof(TextBoxEx))
                        Game.IniParser.Write("SettingValue", con.Name, ((TextBoxEx)con).Text.ToString());
                    if (con.GetType() == typeof(ComboBox))
                        Game.IniParser.Write("SettingValue", con.Name, ((ComboBox)con).SelectedIndex.ToString());
                    if (con.GetType() == typeof(CheckBox))
                        Game.IniParser.Write("SettingValue", con.Name, ((CheckBox)con).Checked.ToString());
                    if (con.GetType() == typeof(ColorSlider))
                        Game.IniParser.Write("SettingValue", con.Name, ((ColorSlider)con).Value.ToString());
                }
                catch { }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            foreach(Control con in panGameItem.Controls)
            {
                if (con != sender)
                    con.Dispose();
            }
            panGameItem.SendToBack();
            panel1.Visible = panel2.Visible = true;
        }

        void CreateControl(UserControl use)
        {
            panel1.Visible = panel2.Visible = false;
            panGameItem.Visible = true;
            panGameItem.Dock = DockStyle.Fill;
            panGameItem.BringToFront();
            panGameItem.Controls.Add(use);
            use.Dock = DockStyle.Fill;
            use.BringToFront();
        }

        private void btnDropItemEx_Click(object sender, EventArgs e)
        {
            Main.Instance.CreateControl(new DropItem());
        }

        private void checkExitSleepTime_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
