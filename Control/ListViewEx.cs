using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Linq;

namespace _i
{

    public class ListViewEx : ListView
    {
        public struct NMHDR
        {
            public IntPtr hwndFrom;
            public int idFrom;
            public int code;
        }

        public ListViewEx()
            : base()
        {
            allowReorder = true;
            lineColor = Color.Red;
            AllowDrop = true;
            View = View.Details;
            AllowColumnReorder = true;
            FullRowSelect = true;
            GridLines = true;
            HideSelection = false;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            USER32.SendMessage(this.Handle, WM_CHANGEUISTATE, MakeLong(UIS_SET, UISF_HIDEFOCUS), 0);

            if (m_changeDelayTimer == null)
            {
                m_changeDelayTimer = new Timer();
                m_changeDelayTimer.Tick += ChangeDelayTimerTick;
                m_changeDelayTimer.Interval = 200;
            }

            //KeyDown += (ss, ee) =>
            //{
                
            //    if (ee.KeyCode == Keys.A && ee.Control)
            //    {
            //        this.SelectAllItems();
            //    }
            //};

            //RetrieveVirtualItem += (ss, ee) =>
            //{
            //    if (VirtualItems.Count > ee.ItemIndex)
            //    {                   
            //        ee.Item = VirtualItems[ee.ItemIndex];
            //    }
            //};

            //DrawItem += (ss, ee) =>
            //{
            //    if (VirtualMode && CheckBoxes)
            //    {
            //        ee.DrawDefault = true;
            //        if (!ee.Item.Checked)
            //        {
            //            ee.Item.Checked = true;
            //            ee.Item.Checked = false;
            //        }
            //    }
            //};
        }

        //public ListViewItem SelectedVitualItem
        //{
        //    get
        //    {
        //        return SelectedVitualItems.FirstOrDefault();
        //    }
        //}

        //public IEnumerable<ListViewItem> SelectedVitualItems
        //{
        //    get
        //    {
        //        foreach(int i in SelectedIndices)
        //        {
        //            if (VirtualItems.Count > i)
        //            {
        //                yield return VirtualItems[i];
        //            }
        //        }
        //    }
        //}

        //private List<ListViewItem> virtualItems { get; set; } = new List<ListViewItem>();

        //public List<ListViewItem> VirtualItems
        //{
        //    get
        //    {
        //        return virtualItems;
        //    }
        //    set
        //    {
        //        virtualItems = value;
        //        if (VirtualMode == true)
        //            VirtualListSize = virtualItems.Count;
        //    }
        //}

        /// <summary>
        /// Make sure to properly dispose of the timer
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && m_changeDelayTimer != null)
            {
                m_changeDelayTimer.Tick -= ChangeDelayTimerTick;
                m_changeDelayTimer.Dispose();
            }
            base.Dispose(disposing);
        }

        private Timer m_changeDelayTimer = null;

        /// <summary>
        /// Hack to avoid lots of unnecessary change events by marshaling with a timer:
        /// http://stackoverflow.com/questions/86793/how-to-avoid-thousands-of-needless-listview-selectedindexchanged-events
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSelectedIndexChanged(EventArgs e)
        {
        
            // When a new SelectedIndexChanged event arrives, disable, then enable the
            // timer, effectively resetting it, so that after the last one in a batch
            // arrives, there is at least 40 ms before we react, plenty of time 
            // to wait any other selection events in the same batch.
            m_changeDelayTimer.Enabled = false;
            m_changeDelayTimer.Enabled = true;
        }

        private void ChangeDelayTimerTick(object sender, EventArgs e)
        {
            m_changeDelayTimer.Enabled = false;
            base.OnSelectedIndexChanged(new EventArgs());
        }

        public void PerformDoubleClick(EventArgs e = null)
        {
            base.OnDoubleClick(e);
        }

        public class SubItemEventArgs : EventArgs
        {
            public SubItemEventArgs(ListViewItem item, int subItem)
            {
                _subItemIndex = subItem;
                _item = item;
            }
            private int _subItemIndex = -1;
            private ListViewItem _item = null;
            public int SubItem
            {
                get { return _subItemIndex; }
            }
            public ListViewItem Item
            {
                get { return _item; }
            }
        }

        public class SubItemEndEditingEventArgs : SubItemEventArgs
        {
            private string _text = string.Empty;
            private bool _cancel = true;

            public SubItemEndEditingEventArgs(ListViewItem item, int subItem, string display, bool cancel) :
                base(item, subItem)
            {
                _text = display;
                _cancel = cancel;
            }
            public string DisplayText
            {
                get { return _text; }
                set { _text = value; }
            }
            public bool Cancel
            {
                get { return _cancel; }
                set { _cancel = value; }
            }
        }

        private Control _editingControl;
        public event SubItemEndEditingEventHandler SubItemEndEditing;
        public event SubItemEventHandler SubItemBeginEditing;
        public delegate void SubItemEndEditingEventHandler(object sender, SubItemEndEditingEventArgs e);
        public delegate void SubItemEventHandler(object sender, SubItemEventArgs e);
        protected void OnSubItemBeginEditing(SubItemEventArgs e)
        {
            if (SubItemBeginEditing != null)
                SubItemBeginEditing(this, e);
        }


        // ListView messages
        private const int LVM_FIRST = 0x1000;
        private const int LVM_GETCOLUMNORDERARRAY = (LVM_FIRST + 59);

        // Windows Messages that will abort editing
        private const int WM_HSCROLL = 0x114;
        private const int WM_VSCROLL = 0x115;
        private const int WM_SIZE = 0x05;
        private const int WM_NOTIFY = 0x4E;

        private const int HDN_FIRST = -300;
        private const int HDN_BEGINDRAG = (HDN_FIRST - 10);
        private const int HDN_ITEMCHANGINGA = (HDN_FIRST - 0);
        private const int HDN_ITEMCHANGINGW = (HDN_FIRST - 20);

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wPar, IntPtr lPar);
        [DllImport("user32.dll", CharSet = CharSet.Ansi)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int len, ref int[] order);

        public int[] GetColumnOrder()
        {
            IntPtr lPar = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(int)) * Columns.Count);

            IntPtr res = SendMessage(Handle, LVM_GETCOLUMNORDERARRAY, new IntPtr(Columns.Count), lPar);
            if (res.ToInt32() == 0) // Something went wrong
            {
                Marshal.FreeHGlobal(lPar);
                return null;
            }

            int[] order = new int[Columns.Count];
            Marshal.Copy(lPar, order, 0, Columns.Count);

            Marshal.FreeHGlobal(lPar);

            return order;
        }

        public Rectangle GetSubItemBounds(ListViewItem Item, int SubItem)
        {
            int[] order = GetColumnOrder();

            Rectangle subItemRect = Rectangle.Empty;
            if (SubItem >= order.Length)
                throw new IndexOutOfRangeException("SubItem " + SubItem + " out of range");

            if (Item == null)
                throw new ArgumentNullException("Item");

            Rectangle lviBounds = Item.GetBounds(ItemBoundsPortion.Entire);
            int subItemX = lviBounds.Left;

            ColumnHeader col;
            int i;
            for (i = 0; i < order.Length; i++)
            {
                col = this.Columns[order[i]];
                if (col.Index == SubItem)
                    break;
                subItemX += col.Width;
            }
            subItemRect = new Rectangle(subItemX, lviBounds.Top, this.Columns[order[i]].Width, lviBounds.Height);
            return subItemRect;
        }

        public event SubItemEventHandler SubItemClicked;

        protected void OnSubItemClicked(SubItemEventArgs e)
        {
            if (SubItemClicked != null)
                SubItemClicked(this, e);
        }

        private void EditSubitemAt(Point p)
        {
            ListViewItem item;
            int idx = GetSubItemAt(p.X, p.Y, out item);
            if (idx >= 0)
            {
                OnSubItemClicked(new SubItemEventArgs(item, idx));
            }
        }

        public int GetSubItemAt(int x, int y, out ListViewItem item)
        {
            item = this.GetItemAt(x, y);

            if (item != null)
            {
                int[] order = GetColumnOrder();
                Rectangle lviBounds;
                int subItemX;

                lviBounds = item.GetBounds(ItemBoundsPortion.Entire);
                subItemX = lviBounds.Left;
                for (int i = 0; i < order.Length; i++)
                {
                    ColumnHeader h = this.Columns[order[i]];
                    if (x < subItemX + h.Width)
                    {
                        return h.Index;
                    }
                    subItemX += h.Width;
                }
            }

            return -1;
        }

        public void StartEditing(Control c, ListViewItem Item, int SubItem)
        {
            OnSubItemBeginEditing(new SubItemEventArgs(Item, SubItem));

            Rectangle rcSubItem = GetSubItemBounds(Item, SubItem);

            if (rcSubItem.X < 0)
            {
                // Left edge of SubItem not visible - adjust rectangle position and width
                rcSubItem.Width += rcSubItem.X;
                rcSubItem.X = 0;
            }
            if (rcSubItem.X + rcSubItem.Width > this.Width)
            {
                // Right edge of SubItem not visible - adjust rectangle width
                rcSubItem.Width = this.Width - rcSubItem.Left;
            }

            // Subitem bounds are relative to the location of the ListView!
            rcSubItem.Offset(Left, Top);

            // In case the editing control and the listview are on different parents,
            // account for different origins
            Point origin = new Point(0, 0);
            Point lvOrigin = this.Parent.PointToScreen(origin);
            Point ctlOrigin = c.Parent.PointToScreen(origin);

            rcSubItem.Offset(lvOrigin.X - ctlOrigin.X, lvOrigin.Y - ctlOrigin.Y);

            // Position and show editor
            c.Bounds = rcSubItem;
            c.Text = Item.SubItems[SubItem].Text;
            c.Visible = true;
            c.BringToFront();
            c.Focus();

            _editingControl = c;
            _editingControl.Leave += new EventHandler(_editControl_Leave);
            _editingControl.KeyPress += new KeyPressEventHandler(_editControl_KeyPress);

            _editItem = Item;
            _editSubItem = SubItem;
        }
        private ListViewItem _editItem;
        // The SubItem being edited
        private int _editSubItem;

        private void _editControl_Leave(object sender, EventArgs e)
        {
            // cell editor losing focus
            EndEditing(true);
        }

        private void _editControl_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case (char)(int)Keys.Escape:
                    {
                        EndEditing(false);
                        break;
                    }

                case (char)(int)Keys.Enter:
                    {
                        EndEditing(true);
                        break;
                    }
            }
        }

        public void EndEditing(bool AcceptChanges)
        {
            if (_editingControl == null)
                return;

            SubItemEndEditingEventArgs e = new SubItemEndEditingEventArgs(
                _editItem,      // The item being edited
                _editSubItem,   // The subitem index being edited
                AcceptChanges ?
                    _editingControl.Text :  // Use editControl text if changes are accepted
                    _editItem.SubItems[_editSubItem].Text,  // or the original subitem's text, if changes are discarded
                !AcceptChanges  // Cancel?
            );



            _editItem.SubItems[_editSubItem].Text = e.DisplayText;

            _editingControl.Leave -= new EventHandler(_editControl_Leave);
            _editingControl.KeyPress -= new KeyPressEventHandler(_editControl_KeyPress);

            _editingControl.Visible = false;

            _editingControl = null;
            _editItem = null;
            _editSubItem = -1;

            OnSubItemEndEditing(e);
        }

        protected void OnSubItemEndEditing(SubItemEndEditingEventArgs e)
        {
            if (SubItemEndEditing != null)
                SubItemEndEditing(this, e);
        }

        public List<ListViewItem> Search(string input)
        {
            try
            {
                if (dicHides.Count != Items.Count + hideItems.Count)
                {
                    foreach (ListViewItem item in Items)
                    {
                        if (!dicHides.Contains(item))
                        {
                            dicHides.Add(item);
                        }
                    }
                    foreach (ListViewItem item in hideItems)
                    {
                        if (!dicHides.Contains(item))
                        {
                            dicHides.Add(item);
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

                foreach (var item in dicHides)
                {
                    if (!hideItems.Contains(item))
                    {
                        liShow.Add(item);
                    }
                }


                Items.Clear();
                Items.AddRange(liShow.ToArray());
                return li;
            }
            catch
            {
                return new List<ListViewItem>();
            }
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


        public List<ListViewItem> dicHides = new List<ListViewItem>();

        public List<ListViewItem> hideItems { get; set; } = new List<ListViewItem>();

        //public event ScrollEventHandler Scroll;
        //protected virtual void OnScroll(ScrollEventArgs e)
        //{
        //    this.Scroll?.Invoke(this, e);
        //}

        //protected override void WndProc(ref Message m)
        //{
        //    base.WndProc(ref m);
        //    if (m.Msg == 0x115)
        //    { // Trap WM_VSCROLL
        //        OnScroll(new ScrollEventArgs((ScrollEventType)(m.WParam.ToInt32() & 0xffff), 0));
        //    }
        //}

   

        public event ScrollEventHandler Scroll;
        protected virtual void OnScroll(ScrollEventArgs e)
        {
            ScrollEventHandler handler = this.Scroll;
            if (handler != null) handler(this, e);
        }
        private const int WM_MOUSEWHEEL = 0x020A;

        protected override void WndProc(ref Message msg)
        {
            base.WndProc(ref msg);
            switch (msg.Msg)
            {

                // Look	for	WM_VSCROLL,WM_HSCROLL or WM_SIZE messages.
                case WM_MOUSEWHEEL: OnScroll(new ScrollEventArgs((ScrollEventType)(msg.WParam.ToInt32() & 0xffff), 0)); break;
                case WM_VSCROLL: OnScroll(new ScrollEventArgs((ScrollEventType)(msg.WParam.ToInt32() & 0xffff), 0)); break;
                case WM_HSCROLL: OnScroll(new ScrollEventArgs((ScrollEventType)(msg.WParam.ToInt32() & 0xffff), 0)); break;
                case WM_SIZE:
                    EndEditing(false);
                    break;

                case WM_NOTIFY:
                    // Look for WM_NOTIFY of events that might also change the
                    // editor's position/size: Column reordering or resizing
                    NMHDR h = (NMHDR)Marshal.PtrToStructure(msg.LParam, typeof(NMHDR));
                    if (h.code == HDN_BEGINDRAG ||
                        h.code == HDN_ITEMCHANGINGA ||
                        h.code == HDN_ITEMCHANGINGW)
                        EndEditing(false);
                    break;
            }


        }


        private ListViewItem previousItem;
        private bool allowReorder;
        private Color lineColor;
        private bool allowSort;


        public void ScrollToEnd()
        {
            EnsureVisible(Items.Count - 1);
        }

        public List<ListViewItem> VisibleItems
        {
            get
            {
                List<ListViewItem> list = new List<ListViewItem>();
                if (TopItem == null)
                    return list;
                ListViewItem lastVisible = TopItem;
                list.Add(TopItem);
                for (int i = TopItem.Index + 1; i < Items.Count; i++)
                {
                    if (ClientRectangle.Top <= Items[i].Bounds.Top && ClientRectangle.Bottom >= Items[i].Bounds.Bottom)
                    {
                        lastVisible = Items[i];
                        list.Add(Items[i]);
                    }
                    //if (ClientRectangle.Contains(Items[i].Bounds))
                    //{
                    //    lastVisible = Items[i];
                    //    list.Add(Items[i]);
                    //}
                    else
                    {
                        break;
                    }
                }
                return list;
            }
        }

        public ListViewItem LastVisibleItem
        {
            get
            {
                ListViewItem lastVisible = TopItem;
                for (int i = TopItem.Index + 1; i < Items.Count; i++)
                {
                    if (ClientRectangle.Contains(Items[i].Bounds))
                    {
                        lastVisible = Items[i];
                    }
                    else
                    {
                        break;
                    }
                }
                return lastVisible;
            }
        }

        public bool AllowReorder
        {
            get
            {
                return allowReorder;
            }
            set
            {
                allowReorder = value;
            }
        }

        public bool AllowSort
        {
            get { return allowSort; }
            set
            {
                allowSort = value;
                if (allowSort)
                    base.ColumnClick += new ColumnClickEventHandler(ListViewEx_ColumnClick);
                else
                    ColumnClick -= new ColumnClickEventHandler(ListViewEx_ColumnClick);
            }
        }

        private int sortOrder = 0;
        private int lastColumnIndex = 0;

        private void ChangeOrder()
        {
            if (sortOrder == 0)
                sortOrder = 1;
            else if (sortOrder == 1)
                sortOrder = 2;
            else
                sortOrder = 0;
        }

        private void ListViewEx_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            //Main.PushLog(e.)
            if (e.Column == lastColumnIndex)
            {
                ChangeOrder();
            }
            else
            {
                sortOrder = 1;
                lastColumnIndex = e.Column;
            }
            ListViewExtensions.SetSortIcon(this, e.Column, (SortOrder)sortOrder);
            if (sortOrder == 0)
            {
                ListViewItemSorter = null;
            }
            else
            {
                ListViewItemComparer sorter = new ListViewItemComparer(e.Column);
                sorter.SortOrder = sortOrder;
                ListViewItemSorter = sorter;
                Sort();
                ListViewItemSorter = null;
            }
        }

        public Color LineColor
        {
            get { return lineColor; }
            set { lineColor = value; }
        }


        private bool checkFromDoubleClick = false;

        protected override void OnItemCheck(ItemCheckEventArgs ice)
        {
            if (this.checkFromDoubleClick || USER32.IsPressed(USER32.VK_LSHIFT) || USER32.IsPressed(USER32.VK_LCONTROL) || USER32.IsPressed(USER32.VK_RSHIFT) || USER32.IsPressed(USER32.VK_RCONTROL))
            {
                ice.NewValue = ice.CurrentValue;
                this.checkFromDoubleClick = false;
            }
            else
            {
                base.OnItemCheck(ice);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            // Is this a double-click?
            if ((e.Button == MouseButtons.Left) && (e.Clicks > 1))
            {
                this.checkFromDoubleClick = true;
            }
            base.OnMouseDown(e);
        }



        protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (DoubleClickActivation)
            {
                return;
            }

            EditSubitemAt(new Point(e.X, e.Y));
        }

        protected override void OnDoubleClick(EventArgs e)
        {
            base.OnDoubleClick(e);

            if (!DoubleClickActivation)
            {
                return;
            }

            Point pt = this.PointToClient(Cursor.Position);

            EditSubitemAt(pt);
        }

        private bool _doubleClickActivation = false;
        /// <summary>
        /// Is a double click required to start editing a cell?
        /// </summary>
        public bool DoubleClickActivation
        {
            get { return _doubleClickActivation; }
            set { _doubleClickActivation = value; }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            this.checkFromDoubleClick = false;
            base.OnKeyDown(e);
        }




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

        protected override void OnDragDrop(DragEventArgs drgevent)
        {
            if (!allowReorder)
            {
                base.OnDragDrop(drgevent);
                return;
            }

            // get the currently hovered row that the items will be dragged to
            Point clientPoint = base.PointToClient(new Point(drgevent.X, drgevent.Y));
            ListViewItem hoverItem = base.GetItemAt(clientPoint.X, clientPoint.Y);

            if (!drgevent.Data.GetDataPresent(typeof(DragItemData).ToString()) || ((DragItemData)drgevent.Data.GetData(typeof(DragItemData).ToString())).ListView == null || ((DragItemData)drgevent.Data.GetData(typeof(DragItemData).ToString())).DragItems.Count == 0)
                return;

            // retrieve the drag item data
            DragItemData data = (DragItemData)drgevent.Data.GetData(typeof(DragItemData).ToString());

            //if (hoverItem != null)
            //{
            // the user wishes to re-order the items

            // get the index of the hover item

            int hoverIndex = -1;
            if (hoverItem != null)
                hoverIndex = hoverItem.Index;

            // determine if the items to be dropped are from
            // this list view. If they are, perform a hack
            // to increment the hover index so that the items
            // get moved properly.
            if (this == data.ListView)
            {
                if (hoverIndex == -1)
                {
                    hoverIndex = base.Items.Count;
                }
                if (hoverIndex > base.SelectedItems[0].Index)
                {
                    hoverIndex = hoverIndex - data.ListView.SelectedItems.Count;
                }
            }

            // remove all the selected items from the previous list view
            // if the list view was found
            List<ListViewItem> selectedItems = new List<ListViewItem>();
            if (data.ListView != null)
            {
                foreach (ListViewItem itemToRemove in data.ListView.SelectedItems)
                {
                    data.ListView.Items.Remove(itemToRemove);
                    selectedItems.Add(itemToRemove);
                }
            }

            for (int i = selectedItems.Count; i > 0; i--)
            {
                base.Items.Insert(hoverIndex, selectedItems[i - 1]);
            }

            // set the back color of the previous item, then nullify it
            if (previousItem != null)
            {
                previousItem = null;
            }

            this.Invalidate();

            // call the base on drag drop to raise the event
            base.OnDragDrop(drgevent);
        }

        protected override void OnDragOver(DragEventArgs drgevent)
        {
            if (!allowReorder)
            {
                base.OnDragOver(drgevent);
                return;
            }

            if (!drgevent.Data.GetDataPresent(typeof(DragItemData).ToString()))
            {
                // the item(s) being dragged do not have any data associated
                drgevent.Effect = DragDropEffects.None;
                return;
            }

            if (base.Items.Count > 0)
            {
                // get the currently hovered row that the items will be dragged to
                Point clientPoint = base.PointToClient(new Point(drgevent.X, drgevent.Y));
                ListViewItem hoverItem = base.GetItemAt(clientPoint.X, clientPoint.Y);

                Graphics g = this.CreateGraphics();

                if (hoverItem == null)
                {
                    // no item was found, so no drop should take place
                    drgevent.Effect = DragDropEffects.Move;

                    if (previousItem != null)
                    {
                        previousItem = null;
                        Invalidate();
                    }

                    hoverItem = base.Items[base.Items.Count - 1];

                    if (this.View == View.Details || this.View == View.List)
                    {
                        g.DrawLine(new Pen(lineColor, 2), new Point(hoverItem.Bounds.X, hoverItem.Bounds.Y + hoverItem.Bounds.Height), new Point(hoverItem.Bounds.X + this.Bounds.Width, hoverItem.Bounds.Y + hoverItem.Bounds.Height));
                        g.FillPolygon(new SolidBrush(lineColor), new Point[] { new Point(hoverItem.Bounds.X, hoverItem.Bounds.Y + hoverItem.Bounds.Height - 5), new Point(hoverItem.Bounds.X + 5, hoverItem.Bounds.Y + hoverItem.Bounds.Height), new Point(hoverItem.Bounds.X, hoverItem.Bounds.Y + hoverItem.Bounds.Height + 5) });
                        g.FillPolygon(new SolidBrush(lineColor), new Point[] { new Point(this.Bounds.Width - 4, hoverItem.Bounds.Y + hoverItem.Bounds.Height - 5), new Point(this.Bounds.Width - 9, hoverItem.Bounds.Y + hoverItem.Bounds.Height), new Point(this.Bounds.Width - 4, hoverItem.Bounds.Y + hoverItem.Bounds.Height + 5) });
                    }
                    else
                    {
                        g.DrawLine(new Pen(lineColor, 2), new Point(hoverItem.Bounds.X + hoverItem.Bounds.Width, hoverItem.Bounds.Y), new Point(hoverItem.Bounds.X + hoverItem.Bounds.Width, hoverItem.Bounds.Y + hoverItem.Bounds.Height));
                        g.FillPolygon(new SolidBrush(lineColor), new Point[] { new Point(hoverItem.Bounds.X + hoverItem.Bounds.Width - 5, hoverItem.Bounds.Y), new Point(hoverItem.Bounds.X + hoverItem.Bounds.Width + 5, hoverItem.Bounds.Y), new Point(hoverItem.Bounds.X + hoverItem.Bounds.Width, hoverItem.Bounds.Y + 5) });
                        g.FillPolygon(new SolidBrush(lineColor), new Point[] { new Point(hoverItem.Bounds.X + hoverItem.Bounds.Width - 5, hoverItem.Bounds.Y + hoverItem.Bounds.Height), new Point(hoverItem.Bounds.X + hoverItem.Bounds.Width + 5, hoverItem.Bounds.Y + hoverItem.Bounds.Height), new Point(hoverItem.Bounds.X + hoverItem.Bounds.Width, hoverItem.Bounds.Y + hoverItem.Bounds.Height - 5) });
                    }

                    // call the base OnDragOver event
                    base.OnDragOver(drgevent);

                    return;
                }

                // determine if the user is currently hovering over a new
                // item. If so, set the previous item's back color back
                // to the default color.
                if ((previousItem != null && previousItem != hoverItem) || previousItem == null)
                {
                    this.Invalidate();
                }

                // set the background color of the item being hovered
                // and assign the previous item to the item being hovered
                //hoverItem.BackColor = Color.Beige;
                previousItem = hoverItem;

                if (this.View == View.Details || this.View == View.List)
                {
                    g.DrawLine(new Pen(lineColor, 2), new Point(hoverItem.Bounds.X, hoverItem.Bounds.Y), new Point(hoverItem.Bounds.X + this.Bounds.Width, hoverItem.Bounds.Y));
                    g.FillPolygon(new SolidBrush(lineColor), new Point[] { new Point(hoverItem.Bounds.X, hoverItem.Bounds.Y - 5), new Point(hoverItem.Bounds.X + 5, hoverItem.Bounds.Y), new Point(hoverItem.Bounds.X, hoverItem.Bounds.Y + 5) });
                    g.FillPolygon(new SolidBrush(lineColor), new Point[] { new Point(this.Bounds.Width - 4, hoverItem.Bounds.Y - 5), new Point(this.Bounds.Width - 9, hoverItem.Bounds.Y), new Point(this.Bounds.Width - 4, hoverItem.Bounds.Y + 5) });
                }
                else
                {
                    g.DrawLine(new Pen(lineColor, 2), new Point(hoverItem.Bounds.X, hoverItem.Bounds.Y), new Point(hoverItem.Bounds.X, hoverItem.Bounds.Y + hoverItem.Bounds.Height));
                    g.FillPolygon(new SolidBrush(lineColor), new Point[] { new Point(hoverItem.Bounds.X - 5, hoverItem.Bounds.Y), new Point(hoverItem.Bounds.X + 5, hoverItem.Bounds.Y), new Point(hoverItem.Bounds.X, hoverItem.Bounds.Y + 5) });
                    g.FillPolygon(new SolidBrush(lineColor), new Point[] { new Point(hoverItem.Bounds.X - 5, hoverItem.Bounds.Y + hoverItem.Bounds.Height), new Point(hoverItem.Bounds.X + 5, hoverItem.Bounds.Y + hoverItem.Bounds.Height), new Point(hoverItem.Bounds.X, hoverItem.Bounds.Y + hoverItem.Bounds.Height - 5) });
                }

                // go through each of the selected items, and if any of the
                // selected items have the same index as the item being
                // hovered, disable dropping.
                foreach (ListViewItem itemToMove in base.SelectedItems)
                {
                    if (itemToMove.Index == hoverItem.Index)
                    {
                        drgevent.Effect = DragDropEffects.None;
                        hoverItem.EnsureVisible();
                        return;
                    }
                }

                // ensure that the hover item is visible
                hoverItem.EnsureVisible();
            }

            // everything is fine, allow the user to move the items
            drgevent.Effect = DragDropEffects.Move;

            // call the base OnDragOver event
            base.OnDragOver(drgevent);
        }

        protected override void OnDragEnter(DragEventArgs drgevent)
        {
            if (!allowReorder)
            {
                base.OnDragEnter(drgevent);
                return;
            }

            if (!drgevent.Data.GetDataPresent(typeof(DragItemData).ToString()))
            {
                // the item(s) being dragged do not have any data associated
                drgevent.Effect = DragDropEffects.None;
                return;
            }

            // everything is fine, allow the user to move the items
            drgevent.Effect = DragDropEffects.Move;

            // call the base OnDragEnter event
            base.OnDragEnter(drgevent);
        }

        protected override void OnItemDrag(ItemDragEventArgs e)
        {
            if (!allowReorder)
            {
                base.OnItemDrag(e);
                return;
            }

            // call the DoDragDrop method
            base.DoDragDrop(GetDataForDragDrop(), DragDropEffects.Move);

            // call the base OnItemDrag event
            base.OnItemDrag(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            // reset the selected items background and remove the previous item
            ResetOutOfRange();

            Invalidate();

            // call the OnLostFocus event
            base.OnLostFocus(e);
        }

        protected override void OnDragLeave(EventArgs e)
        {
            // reset the selected items background and remove the previous item
            ResetOutOfRange();

            Invalidate();

            // call the base OnDragLeave event
            base.OnDragLeave(e);
        }


        private DragItemData GetDataForDragDrop()
        {
            // create a drag item data object that will be used to pass along with the drag and drop
            DragItemData data = new DragItemData(this);

            // go through each of the selected items and
            // add them to the drag items collection
            // by creating a clone of the list item
            foreach (ListViewItem item in this.SelectedItems)
            {
                ListViewItem newItem = (ListViewItem)item.Clone();
                newItem.Name = item.Name;
                data.DragItems.Add(newItem);
            }

            return data;
        }

        private void ResetOutOfRange()
        {
            // determine if the previous item exists,
            // if it does, reset the background and release
            // the previous item
            if (previousItem != null)
            {
                previousItem = null;
            }

        }


        class DragItemData
        {

            private ListViewEx m_listView;
            private ArrayList m_dragItems;



            public ListViewEx ListView
            {
                get { return m_listView; }
            }

            public ArrayList DragItems
            {
                get { return m_dragItems; }
            }



            public DragItemData(ListViewEx listView)
            {
                m_listView = listView;
                m_dragItems = new ArrayList();
            }
        }

        class ListViewItemComparer : IComparer
        {
            private int sortOrder = 0;
            private int col;

            public int SortOrder
            {
                get
                {
                    return sortOrder;
                }
                set
                {
                    sortOrder = value;
                }
            }

            public ListViewItemComparer()
            {
                col = 0;
            }
            public ListViewItemComparer(int column)
            {
                col = column;
            }
            public int Compare(object x, object y)
            {
                string str1 = "";
                string str2 = "";
                if (col < ((ListViewItem)x).SubItems.Count)
                    str1 = ((ListViewItem)x).SubItems[col].Text;
                if (col < ((ListViewItem)y).SubItems.Count)
                    str2 = ((ListViewItem)y).SubItems[col].Text;
                int value;
                if (int.TryParse(str1, out value) && int.TryParse(str2, out value))
                {
                    if (str2.Length > str1.Length)
                        str1 = new string('0', str2.Length - str1.Length) + str1;
                    else
                        str2 = new string('0', str1.Length - str2.Length) + str2;
                }
                else
                {
                    if (str2.Length > str1.Length)
                        str1 = str1 + new string('0', str2.Length - str1.Length);
                    else
                        str2 = str2 + new string('0', str1.Length - str2.Length);
                }
                if (sortOrder == 1)
                    return String.Compare(str1, str2);
                else if (sortOrder == 2)
                    return String.Compare(str2, str1);
                return 0;
            }
        }
    }
    class ListViewExtensions
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct LVCOLUMN
        {
            public Int32 mask;
            public Int32 cx;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pszText;
            public IntPtr hbm;
            public Int32 cchTextMax;
            public Int32 fmt;
            public Int32 iSubItem;
            public Int32 iImage;
            public Int32 iOrder;
        }

        const Int32 HDI_WIDTH = 0x0001;
        const Int32 HDI_HEIGHT = HDI_WIDTH;
        const Int32 HDI_TEXT = 0x0002;
        const Int32 HDI_FORMAT = 0x0004;
        const Int32 HDI_LPARAM = 0x0008;
        const Int32 HDI_BITMAP = 0x0010;
        const Int32 HDI_IMAGE = 0x0020;
        const Int32 HDI_DI_SETITEM = 0x0040;
        const Int32 HDI_ORDER = 0x0080;
        const Int32 HDI_FILTER = 0x0100;

        const Int32 HDF_LEFT = 0x0000;
        const Int32 HDF_RIGHT = 0x0001;
        const Int32 HDF_CENTER = 0x0002;
        const Int32 HDF_JUSTIFYMASK = 0x0003;
        const Int32 HDF_RTLREADING = 0x0004;
        const Int32 HDF_OWNERDRAW = 0x8000;
        const Int32 HDF_STRING = 0x4000;
        const Int32 HDF_BITMAP = 0x2000;
        const Int32 HDF_BITMAP_ON_RIGHT = 0x1000;
        const Int32 HDF_IMAGE = 0x0800;
        const Int32 HDF_SORTUP = 0x0400;
        const Int32 HDF_SORTDOWN = 0x0200;

        const Int32 LVM_FIRST = 0x1000;         // List messages
        const Int32 LVM_GETHEADER = LVM_FIRST + 31;
        const Int32 HDM_FIRST = 0x1200;         // Header messages
        const Int32 HDM_SETIMAGELIST = HDM_FIRST + 8;
        const Int32 HDM_GETIMAGELIST = HDM_FIRST + 9;
        const Int32 HDM_GETITEM = HDM_FIRST + 11;
        const Int32 HDM_SETITEM = HDM_FIRST + 12;

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        private static extern IntPtr SendMessageLVCOLUMN(IntPtr hWnd, Int32 Msg, IntPtr wParam, ref LVCOLUMN lPLVCOLUMN);


        //This method used to set arrow icon
        public static void SetSortIcon(ListView listView, int columnIndex, SortOrder order)
        {
            IntPtr columnHeader = SendMessage(listView.Handle, LVM_GETHEADER, IntPtr.Zero, IntPtr.Zero);

            for (int columnNumber = 0; columnNumber <= listView.Columns.Count - 1; columnNumber++)
            {
                IntPtr columnPtr = new IntPtr(columnNumber);
                LVCOLUMN lvColumn = new LVCOLUMN();
                lvColumn.mask = HDI_FORMAT;

                SendMessageLVCOLUMN(columnHeader, HDM_GETITEM, columnPtr, ref lvColumn);

                if (!(order == SortOrder.None) && columnNumber == columnIndex)
                {
                    switch (order)
                    {
                        case SortOrder.Ascending:
                            lvColumn.fmt &= ~HDF_SORTDOWN;
                            lvColumn.fmt |= HDF_SORTUP;
                            break;
                        case SortOrder.Descending:
                            lvColumn.fmt &= ~HDF_SORTUP;
                            lvColumn.fmt |= HDF_SORTDOWN;
                            break;
                    }
                    lvColumn.fmt |= (HDF_LEFT | HDF_BITMAP_ON_RIGHT);
                }
                else
                {
                    lvColumn.fmt &= ~HDF_SORTDOWN & ~HDF_SORTUP & ~HDF_BITMAP_ON_RIGHT;
                }

                SendMessageLVCOLUMN(columnHeader, HDM_SETITEM, columnPtr, ref lvColumn);
            }
        }
    }
}