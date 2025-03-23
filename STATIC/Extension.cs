using ChreneLib.Controls.TextBoxes;
using FastColoredTextBoxNS;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;

namespace _i
{

    public class fPoint
    {
        public fPoint(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double X { get; set; }
        public double Y { get; set; }
    }

    class ComboboxValue
    {
        public string Id { get; private set; }
        public string Name { get; private set; }

        public ComboboxValue(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public static class StringExtensions
    {
        //aaeeoouuiiddyy
        private static readonly string[] VietnameseLowerSigns = new string[]
{
  "aaeeoouuiiddyy",
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
        public static string VietLien(this string str)
        {
            return TDT.VietLien(str);
        }

        public static string RemoveSign4VietnameseString(this string str)
        {

            //Tiến hành thay thế , lọc bỏ dấu cho chuỗi
            for (int i = 1; i < VietnameseSigns.Length; i++)
            {
                for (int j = 0; j < VietnameseSigns[i].Length; j++)
                    str = str.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);
            }
            return str;
        }

        public static void AppendTextTop(this RichTextBox box, string text, Color color, bool nosu = false)
        {
            box.Text = box.Text.Insert(0, text);
            //if (text.Length > 2)
                RichTextBoxChangeWordColor(box, 0, text.Length, color, nosu);
        }

        public static RichTextBox RichTextBoxChangeWordColor(ref RichTextBox rtb, string startWord, string endWord, Color color)
        {
            rtb.SuspendLayout();
            Point scroll = rtb.AutoScrollOffset;
            int slct = rtb.SelectionIndent;
            int ss = rtb.SelectionStart;
            List<Point> ls = GetAllWordsIndecesBetween(rtb.Text, startWord, endWord, true);
            foreach (var item in ls)
            {
                rtb.SelectionStart = item.X;
                rtb.SelectionLength = item.Y - item.X;
                rtb.SelectionColor = color;
            }
            rtb.SelectionStart = ss;
            rtb.SelectionIndent = slct;
            rtb.AutoScrollOffset = scroll;
            rtb.ResumeLayout(true);
            return rtb;
        }

        public static List<Point> GetAllWordsIndecesBetween(string intoText, string fromThis, string toThis, bool withSigns = true)
        {
            List<Point> result = new List<Point>();
            Stack<int> stack = new Stack<int>();
            bool start = false;
            for (int i = 0; i < intoText.Length; i++)
            {
                string ssubstr = intoText.Substring(i);
                if (ssubstr.StartsWith(fromThis) && ((fromThis == toThis && !start) || !ssubstr.StartsWith(toThis)))
                {
                    if (!withSigns) i += fromThis.Length;
                    start = true;
                    stack.Push(i);
                }
                else if (ssubstr.StartsWith(toThis))
                {
                    if (withSigns) i += toThis.Length;
                    start = false;
                    if (stack.Count > 0)
                    {
                        int startindex = stack.Pop();
                        result.Add(new Point(startindex, i));
                    }
                }
            }
            return result;
        }


        public static RichTextBox RichTextBoxChangeWordColor(RichTextBox rtb, int start, int end, Color color, bool nosu = false)
        {
            rtb.SuspendLayout();
            rtb.SelectionStart = 0;
            rtb.SelectionLength = end;
            rtb.SelectionColor = color;
            rtb.ResumeLayout(true);

            return rtb;
        }

        public static void AppendText(this RichTextBox box, string text, Color color)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;

            box.SelectionColor = color;
            box.AppendText(text);
            box.SelectionColor = box.ForeColor;
        }

        public static void ScroolToEnd(this RichTextBox box)
        {
            box.SelectionStart = box.Text.Length;
            box.ScrollToCaret();
        }

        public static int ToNumber(this object o)
        {
            return TDT.ParseInt(o.ToString());
        }

        public static T DeepClone<T>(this T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }

        //NameValueCollection nvc = new NameValueCollection();
      //  nvc.Add("id", "TTR");
    //nvc.Add("btn-submit-photo", "Upload");
    //HttpUploadFile("http://your.server.com/upload",         @"C:\test\test.jpg", "file", "image/jpeg", nvc);
        //public static void HttpUploadFile(string url, string file, string paramName, string contentType, NameValueCollection nvc)
        //{
        //    Main.PushLog(string.Format("Uploading {0} to {1}", file, url));
        //    string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
        //    byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

        //    HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
        //    wr.ContentType = "multipart/form-data; boundary=" + boundary;
        //    wr.Method = "POST";
        //    wr.KeepAlive = true;
        //    wr.Credentials = System.Net.CredentialCache.DefaultCredentials;

        //    Stream rs = wr.GetRequestStream();

        //    string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
        //    foreach (string key in nvc.Keys)
        //    {
        //        rs.Write(boundarybytes, 0, boundarybytes.Length);
        //        string formitem = string.Format(formdataTemplate, key, nvc[key]);
        //        byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
        //        rs.Write(formitembytes, 0, formitembytes.Length);
        //    }
        //    rs.Write(boundarybytes, 0, boundarybytes.Length);

        //    string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
        //    string header = string.Format(headerTemplate, paramName, file, contentType);
        //    byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
        //    rs.Write(headerbytes, 0, headerbytes.Length);

        //    FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
        //    byte[] buffer = new byte[4096];
        //    int bytesRead = 0;
        //    while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
        //    {
        //        rs.Write(buffer, 0, bytesRead);
        //    }
        //    fileStream.Close();

        //    byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
        //    rs.Write(trailer, 0, trailer.Length);
        //    rs.Close();

        //    WebResponse wresp = null;
        //    try
        //    {
        //        wresp = wr.GetResponse();
        //        Stream stream2 = wresp.GetResponseStream();
        //        StreamReader reader2 = new StreamReader(stream2);
        //        Main.PushLog(string.Format("File uploaded, server response is: {0}", reader2.ReadToEnd()));
        //    }
        //    catch (Exception ex)
        //    {
        //        Main.PushLog("Error uploading file " + ex.Message);
        //        if (wresp != null)
        //        {
        //            wresp.Close();
        //            wresp = null;
        //        }
        //    }
        //    finally
        //    {
        //        wr = null;
        //    }
        //}

        public static void HttpUploadFile(string url, string file, string paramName, string contentType, NameValueCollection nvc)
        {
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
            wr.ContentType = "multipart/form-data; boundary=" + boundary;
            wr.Method = "POST";
            wr.KeepAlive = true;
            wr.Credentials = System.Net.CredentialCache.DefaultCredentials;

            Stream rs = wr.GetRequestStream();

            string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
            foreach (string key in nvc.Keys)
            {
                rs.Write(boundarybytes, 0, boundarybytes.Length);
                string formitem = string.Format(formdataTemplate, key, nvc[key]);
                byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                rs.Write(formitembytes, 0, formitembytes.Length);
            }
            rs.Write(boundarybytes, 0, boundarybytes.Length);

            string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
            string header = string.Format(headerTemplate, paramName, file, contentType);
            byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
            rs.Write(headerbytes, 0, headerbytes.Length);

            FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[4096];
            int bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                rs.Write(buffer, 0, bytesRead);
            }
            fileStream.Close();

            byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            rs.Write(trailer, 0, trailer.Length);
            rs.Close();

            WebResponse wresp = null;
            try
            {
                wresp = wr.GetResponse();
                Stream stream2 = wresp.GetResponseStream();
                StreamReader reader2 = new StreamReader(stream2);

            }
            catch
            {

                if (wresp != null)
                {
                    wresp.Close();
                    wresp = null;
                }
            }
            finally
            {
                wr = null;
            }
        }

        public static void Search(this FastColoredTextBox txtDropName, string search)
        {
            Range range;
            search = search.VietLien();
            if(search.Length == 0)
            {
                range = txtDropName.Selection.Clone();
                range.Start = new Place(0, 0);
                range.End = new Place(0, 0);

                range.Normalize();

                txtDropName.Selection = range;
                txtDropName.DoSelectionVisible();
                txtDropName.Invalidate();
                return;
            }
            if (search.Length > 0)
            {
                for (int i = 0; i < txtDropName.Lines.Count; i++)
                {
                    if (TDT.VietLien(txtDropName.Lines[i]).Contains(search))
                    {
                        range = txtDropName.Selection.Clone();
                        range.Start = new Place(0, i);
                        range.End = new Place(txtDropName.Lines[i].Length, i);

                        range.Normalize();

                        txtDropName.Selection = range;
                        txtDropName.DoSelectionVisible();
                        txtDropName.Invalidate();
                        return;
                    }
                }
            }
            range = txtDropName.Selection.Clone();
            range.Start = new Place(0, 0);
            range.End = new Place(0, 0);

            range.Normalize();

            txtDropName.Selection = range;
            txtDropName.DoSelectionVisible();
            txtDropName.Invalidate();
        }


        internal const int LVM_FIRST = 0x1000;

        private const int LVM_SETITEMSTATE = LVM_FIRST + 43;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct LVITEM
        {
            public int mask;
            public int iItem;
            public int iSubItem;
            public int state;
            public int stateMask;
            [MarshalAs(UnmanagedType.LPTStr)] public string pszText;
            public int cchTextMax;
            public int iImage;
            public IntPtr lParam;
            public int iIndent;
            public int iGroupId;
            public int cColumns;
            public IntPtr puColumns;
        };

        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessageLVItem(HandleRef hWnd, int msg, int wParam, ref LVITEM lvi);

        /// <summary>
        /// Select all rows on the given listview
        /// </summary>
        /// <param name="listView">The listview whose items are to be selected</param>
        public static void SelectAllItems(this ListView listView)
        {
            SetItemState(listView, -1, 2, 2);
        }


        public static void ForEach<T>(this IEnumerable<T> @this, Action<T> action)
        {
            foreach (T item in @this)
            {
                action(item);
            }
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
     (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

      

        public static void Invoke(this Control control, Action action)
        {
            control.Invoke((Delegate)action);
        }

        /// <summary>
        /// Set the item state on the given item
        /// </summary>
        /// <param name="list">The listview whose item's state is to be changed</param>
        /// <param name="itemIndex">The index of the item to be changed</param>
        /// <param name="mask">Which bits of the value are to be set?</param>
        /// <param name="value">The value to be set</param>
        public static void SetItemState(ListView listView, int itemIndex, int mask, int value)
        {
            LVITEM lvItem = new LVITEM();
            lvItem.stateMask = mask;
            lvItem.state = value;
            SendMessageLVItem(new HandleRef(listView, listView.Handle), LVM_SETITEMSTATE, itemIndex, ref lvItem);
        }

        public static int ToInt(this bool val)
        {
            return TDT.Bool2Int(val);
        }

        public static int ToInt(this string val)
        {
            return TDT.ParseAllInt(val);
        }

        public static int ToIntEx(this string val)
        {
            return TDT.ParseInt(val);
        }

        public static void Invoke(this Form form, Action a)
        {
            form.Invoke(new Action(a));
        }


        public static double DistanceToLine(this Point point, Point x, Point y)
        {
            int a = x.Y - y.Y;
            int b = y.X - x.X;
            int c = a * (0 - x.X) + b * (0 - x.Y);
            return Math.Abs(a * point.X + b * point.Y + c) / Math.Sqrt(a * a + b * b);
        }

        public static bool IsInLine(this Point point, Point x, Point y)
        {
            int a = x.Y - y.Y;
            int b = y.X - x.X;
            int c = a * (0 - x.X) + b * (0 - x.Y);
            return a * point.X + b * point.Y + c == 0;
        }

        public static bool IsInLine(this fPoint point, Point x, Point y)
        {
            int a = x.Y - y.Y;
            int b = y.X - x.X;
            int c = a * (0 - x.X) + b * (0 - x.Y);
            return a * point.X + b * point.Y + c == 0;
        }

        public static bool IsClockwise(this Point from, Point to, Point tam)
        {
            from.X = from.X - tam.X;
            to.X = to.X - tam.X;
            from.Y = tam.Y - from.Y;
            to.Y = tam.Y - to.Y;
            return ((to.X - from.X) * (to.Y + from.Y)) > 0;
        }


        public static int TimDelta(Point x, Point y, Point m, float R)
        {
            int a = x.Y - y.Y;
            int b = y.X - x.X;
            int c = a * (0 - x.X) + b * (0 - x.Y);

            a = -a / b;
            b = -c / b;
            int u = m.X;
            int v = m.Y;


            double delta = 4 * (a * (b - v) - u) * (a * (b - v) - u) - 4 * (a * a + 1) * (u * u + (b - v) * (b - v) - R * R);


            return (int)delta;
        }

        public static List<fPoint> TimGiaoDiem(Point x, Point y, Point m, float R)
        {
            double delta = 0;
            List<fPoint> list = new List<fPoint>();
            double a = x.Y - y.Y;
            double b = y.X - x.X;
            double c = a * (0 - x.X) + b * (0 - x.Y);

            if (b == 0)
            {
                double X = x.X;
                delta = 4 * m.Y * m.Y - 4 * (m.Y * m.Y + (X - m.X) * (X - m.X) - R * R);
                if (delta == 0)
                {
                    double Y1 = (2 * m.Y) / 2;
                    list.Add(new fPoint(X, Y1));
                    return list;
                }
                else
                {
                    if (delta > 0)
                    {
                        double Y1 = (2 * m.Y + Math.Sqrt(delta)) / 2;
                        double Y2 = (2 * m.Y - Math.Sqrt(delta)) / 2;
                        list.Add(new fPoint(X, Y1));
                        list.Add(new fPoint(X, Y2));
                        return list;
                    }
                }
            }
            else if (a == 0)
            {
                double Y = x.Y;
                delta = 4 * m.X * m.X - 4 * (m.X * m.X + (Y - m.Y) * (Y - m.Y) - R * R);
                if (delta == 0)
                {
                    double X1 = (2 * m.X) / 2;
                    list.Add(new fPoint(X1, Y));
                    return list;
                }
                else
                {
                    if (delta > 0)
                    {
                        double X1 = (2 * m.X + Math.Sqrt(delta)) / 2;
                        double X2 = (2 * m.X - Math.Sqrt(delta)) / 2;
                        list.Add(new fPoint(X1, Y));
                        list.Add(new fPoint(X2, Y));
                        return list;
                    }
                }
            }
            else
            {
                a = -(double)a / b;
                b = -(double)c / b;


                delta = 4 * (a * (b - m.Y) - m.X) * (a * (b - m.Y) - m.X) - 4 * (a * a + 1) * (m.X + (b - m.Y) * (b - m.Y)  - R * R);

                if (delta == 0)
                {
                    double X1 = (-2 * (a * (b - m.Y) - m.X)) / (2 * (a * a + 1));
                    list.Add(new fPoint(X1, a * X1 + b));
                    return list;
                }
                else
                {
                    if (delta > 0)
                    {
                        double X1 = (-2 * (a * (b - m.Y) - m.X) + Math.Sqrt(delta)) / (2 * (a * a + 1));
                        double X2 = (-2 * (a * (b - m.Y) - m.X) - Math.Sqrt(delta)) / (2 * (a * a + 1));
                        list.Add(new fPoint(X1, a * X1 + b));
                        list.Add(new fPoint(X2, a * X2 + b));
                        return list;
                    }
                }

            }
            return list;
        }

        public static IEnumerable<IEnumerable<T>> Split<T>(this T[] array, int size)
        {
            for (var i = 0; i < (float)array.Length / size; i++)
            {
                yield return array.Skip(i * size).Take(size);
            }
        }

        public static void SafeSave(this string content, string path)
        {
            TDT.WriteFile(path + ".new", content);

            TDT.DeleteFile(path + ".old");
            TDT.MoveFile(path, path + ".old");

            TDT.DeleteFile(path);
            TDT.MoveFile(path + ".new", path);


            TDT.DeleteFile(path + ".old");
        }
        public static IEnumerable<T> AllControls<T>(this Control startingPoint) where T : Control
        {
            bool hit = startingPoint is T;
            if (hit)
            {
                yield return startingPoint as T;
            }
            foreach (var child in startingPoint.Controls.Cast<Control>())
            {
                foreach (var item in AllControls<T>(child))
                {
                    yield return item;
                }
            }
        }

        public static IEnumerable<Control> GetControlHierarchy(this Control root)
        {
            var queue = new Queue<Control>();

            queue.Enqueue(root);

            do
            {
                var control = queue.Dequeue();

                yield return control;

                foreach (var child in control.Controls.OfType<Control>())
                    queue.Enqueue(child);

            } while (queue.Count > 0);

        }

        public static IEnumerable<Control> GetAllChildren(this Control root)
        {
            var q = new Queue<Control>(root.Controls.Cast<Control>());

            while (q.Any())
            {
                var next = q.Dequeue();
                foreach (Control c in next.Controls)
                    q.Enqueue(c);

                yield return next;
            }
        }



        /// <summary>
        /// Recursive function to get all descendant controls.
        /// </summary>
        public static IEnumerable<Control> GetDescendants(this Control control)
        {
            var children = control.Controls.Cast<Control>();
            return children.Concat(children.SelectMany(c => GetDescendants(c)));
        }

        public static IEnumerable<ToolStripMenuItem> GetDescendants(this ToolStripMenuItem control)
        {
            var children = control.DropDownItems.Cast<ToolStripItem>().Where(i => i.GetType() == typeof(ToolStripMenuItem)).Cast<ToolStripMenuItem>();
            return children.Concat(children.SelectMany(c => GetDescendants(c)));
        }


        public static string MergeLine(this string input, string delimiter = "-")
        {
            StringBuilder sb = new StringBuilder();

            if (input == null)
            {
                return string.Empty;
            }

            using (StringReader reader = new StringReader(input))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    sb.Append(line).Append(delimiter);
                }
            }

            return sb.ToString();
        }

        public static string EmptyIfNull(this object value)
        {
            if (value == null)
                return "";
            return value.ToString();
        }

        public static IEnumerable<string> SplitToLines(this string input)
        {
            if (input == null)
            {
                yield break;
            }

            using (System.IO.StringReader reader = new System.IO.StringReader(input))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }

        public static int ToNumber(this string o)
        {
            if (string.IsNullOrEmpty(o))
                return -1;
            int val = 0;
            string b = string.Empty;
            for (int i = 0; i < o.Length; i++)
            {
                if (System.Char.IsDigit(o[i]))
                    b += o[i];
                else if (b.Length > 0)
                    break;
            }
            if (b.Length > 0)
                val = int.Parse(b);
            if (o.StartsWith("-"))
                return -val;
            return val;
        }

        //public static void Is()
    }

   
}
