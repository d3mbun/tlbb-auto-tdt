using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Linq;

namespace _i
{
    class ListViewSortChecked : ListView
    {
        public ListViewSortChecked()
            : base()
        {
            View = View.Details;
            FullRowSelect = true;
            GridLines = true;
            HideSelection = false;
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SendMessage(this.Handle, WM_CHANGEUISTATE, MakeLong(UIS_SET, UISF_HIDEFOCUS), 0);
        }
        [DllImport("user32.dll", CharSet = CharSet.Auto)]

        static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        public void SetNull()
        {
            dicHides.Clear();
            hideItems.Clear();
        }


        public List<ListViewItem> Search(string input)
        {
            if (dicHides.Count != Items.Count + hideItems.Count)
            {
                foreach (ListViewItem item in Items)
                {
                    if (!dicHides.ContainsValue(item))
                    {
                        dicHides.Add(item.Index, item);
                    }
                }
                foreach (ListViewItem item in hideItems)
                {
                    if (!dicHides.ContainsValue(item))
                    {
                        dicHides.Add(item.Index, item);
                    }
                }
            }
            List<ListViewItem> li = new List<ListViewItem>();
            List<ListViewItem> liShow = new List<ListViewItem>();
            var niceSearch = minString(input);
            this.Items.Cast<ListViewItem>().ToList().ForEach(i =>
            {
                StringBuilder nice = new StringBuilder();
                foreach (var sub in i.SubItems)
                {
                    nice.Append(minString(sub.ToString()));

                }
                if (nice.ToString().Contains(niceSearch))
                {
                    li.Add(i);
                }
                else
                {
                    hideItems.Add(i);
                }
            });
            hideItems.ForEach(i =>
            {
                StringBuilder nice = new StringBuilder();
                foreach (var sub in i.SubItems)
                {
                    nice.Append(minString(sub.ToString()));
                }
                if (nice.ToString().Contains(niceSearch))
                {
                    li.Add(i);
                }
            });



            li.ForEach(i => { if (!Items.Contains(i)) { Items.Add(i); hideItems.Remove(i); } });

            //hideItems.ForEach(i => i.Remove());

            foreach (KeyValuePair<int, ListViewItem> kvp in dicHides)
            {
                if (!hideItems.Contains(kvp.Value))
                {
                    liShow.Add(kvp.Value);
                }
            }


            Items.Clear();
            Items.AddRange(liShow.ToArray());
            return li;
        }

        public static string NiceString(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;
            return RemoveSign(RemoveWhitespace(input));
        }

        public static string minString(string input)
        {
            return NiceString(input).ToLower();
        }

        private static readonly string[] VietnameseSigns = new string[]
     {
            "aAeEoOuUiIdDyY",
            "áàạảãâấầậẩẫăắằặẳẵ",
            "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
            "éèẹẻẽêếềệểễ",
            "ÉÈẸẺẼÊẾỀỆỂỄ",
            "óòọỏõôốồộổỗơớờợởỡ",
            "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
            "úùụủũưứừựửữ",
            "ÚÙỤỦŨƯỨỪỰỬỮ",
            "íìịỉĩ",
            "ÍÌỊỈĨ",
            "đ",
            "Đ",
            "ýỳỵỷỹ",
            "ÝỲỴỶỸ"
     };

        public static string RemoveSign(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;
            for (int i = 1; i < VietnameseSigns.Length; i++)
            {
                for (int j = 0; j < VietnameseSigns[i].Length; j++)
                    input = input.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);
            }
            return input;
        }

        public static string RemoveWhitespace(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;
            return new string(input.ToCharArray().Where(c => !System.Char.IsWhiteSpace(c)).ToArray());
        }


        Dictionary<int, ListViewItem> dicHides = new Dictionary<int, ListViewItem>();

        List<ListViewItem> hideItems { get; set; } = new List<ListViewItem>();
        private const int WM_CHANGEUISTATE = 0x127;
        private const int UIS_SET = 1;
        private const int UISF_HIDEFOCUS = 0x1;
        private int MakeLong(int wLow, int wHigh)
        {
            int low = (int)IntLoWord(wLow);
            short high = IntLoWord(wHigh);
            int product = 0x10000 * (int)high;
            int mkLong = (int)(low | product);
            return mkLong;
        }
        private short IntLoWord(int word)
        {
            return (short)(word & short.MaxValue);
        }
    }
}
