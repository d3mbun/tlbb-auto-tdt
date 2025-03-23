using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;

namespace _i
{    
    class Win
    {
        /// <summary>
        ///     Special window handles
        /// </summary>
        public enum SpecialWindowHandles
        {
            // ReSharper disable InconsistentNaming
            /// <summary>
            ///     Places the window at the top of the Z order.
            /// </summary>
            HWND_TOP = 0,
            /// <summary>
            ///     Places the window at the bottom of the Z order. If the hWnd parameter identifies a topmost window, the window loses its topmost status and is placed at the bottom of all other windows.
            /// </summary>
            HWND_BOTTOM = 1,
            /// <summary>
            ///     Places the window above all non-topmost windows. The window maintains its topmost position even when it is deactivated.
            /// </summary>
            HWND_TOPMOST = -1,
            /// <summary>
            ///     Places the window above all non-topmost windows (that is, behind all topmost windows). This flag has no effect if the window is already a non-topmost window.
            /// </summary>
            HWND_NOTOPMOST = -2
            // ReSharper restore InconsistentNaming
        }
        
        public class SetWindowPosFlags
        {
            // ReSharper disable InconsistentNaming

            /// <summary>
            ///     If the calling thread and the thread that owns the window are attached to different input queues, the system posts the request to the thread that owns the window. This prevents the calling thread from blocking its execution while other threads process the request.
            /// </summary>
            public static int SWP_ASYNCWINDOWPOS = 0x4000;

            /// <summary>
            ///     Prevents generation of the WM_SYNCPAINT message.
            /// </summary>
            public static int SWP_DEFERERASE = 0x2000;

            /// <summary>
            ///     Draws a frame (defined in the window's class description) around the window.
            /// </summary>
            public static int SWP_DRAWFRAME = 0x0020;

            /// <summary>
            ///     Applies new frame styles set using the SetWindowLong function. Sends a WM_NCCALCSIZE message to the window, even if the window's size is not being changed. If this flag is not specified, WM_NCCALCSIZE is sent only when the window's size is being changed.
            /// </summary>
            public static int SWP_FRAMECHANGED = 0x0020;

            /// <summary>
            ///     Hides the window.
            /// </summary>
            public static int SWP_HIDEWINDOW = 0x0080;

            /// <summary>
            ///     Does not activate the window. If this flag is not set, the window is activated and moved to the top of either the topmost or non-topmost group (depending on the setting of the hWndInsertAfter parameter).
            /// </summary>
            public static int SWP_NOACTIVATE = 0x0010;

            /// <summary>
            ///     Discards the entire contents of the client area. If this flag is not specified, the valid contents of the client area are saved and copied back into the client area after the window is sized or repositioned.
            /// </summary>
            public static int SWP_NOCOPYBITS = 0x0100;

            /// <summary>
            ///     Retains the current position (ignores X and Y parameters).
            /// </summary>
            public static int SWP_NOMOVE = 0x0002;

            /// <summary>
            ///     Does not change the owner window's position in the Z order.
            /// </summary>
            public static int SWP_NOOWNERZORDER = 0x0200;

            /// <summary>
            ///     Does not redraw changes. If this flag is set, no repainting of any kind occurs. This applies to the client area, the nonclient area (including the title bar and scroll bars), and any part of the parent window uncovered as a result of the window being moved. When this flag is set, the application must explicitly invalidate or redraw any parts of the window and parent window that need redrawing.
            /// </summary>
            public static int SWP_NOREDRAW = 0x0008;

            /// <summary>
            ///     Same as the SWP_NOOWNERZORDER flag.
            /// </summary>
            public static int SWP_NOREPOSITION = 0x0200;

            /// <summary>
            ///     Prevents the window from receiving the WM_WINDOWPOSCHANGING message.
            /// </summary>
            public static int SWP_NOSENDCHANGING = 0x0400;

            /// <summary>
            ///     Retains the current size (ignores the cx and cy parameters).
            /// </summary>
            public static int SWP_NOSIZE = 0x0001;

            /// <summary>
            ///     Retains the current Z order (ignores the hWndInsertAfter parameter).
            /// </summary>
            public static int SWP_NOZORDER = 0x0004;

            /// <summary>
            ///     Displays the window.
            /// </summary>
            public static int SWP_SHOWWINDOW = 0x0040;

            // ReSharper restore InconsistentNaming
        }

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, long uFlags);

        public class INSERTAFTER
        {
            public static IntPtr HWND_BOTTOM = (IntPtr)1;
            public static IntPtr HWND_NOTOPMOST = (IntPtr)(-2);
            public static IntPtr HWND_TOP = (IntPtr)0;
            public static IntPtr HWND_TOPMOST = (IntPtr)(-1);
        }


        public static class WindowStyles
        {
            public const uint WS_OVERLAPPED = 0x00000000;
            public const uint WS_POPUP = 0x80000000;
            public const uint WS_CHILD = 0x40000000;
            public const uint WS_MINIMIZE = 0x20000000;
            public const uint WS_VISIBLE = 0x10000000;
            public const uint WS_DISABLED = 0x08000000;
            public const uint WS_CLIPSIBLINGS = 0x04000000;
            public const uint WS_CLIPCHILDREN = 0x02000000;
            public const uint WS_MAXIMIZE = 0x01000000;
            public const uint WS_CAPTION = 0x00C00000;     /* WS_BORDER | WS_DLGFRAME  */
            public const int WS_BORDER = 0x00800000;
            public const uint WS_DLGFRAME = 0x00400000;
            public const uint WS_VSCROLL = 0x00200000;
            public const uint WS_HSCROLL = 0x00100000;
            public const uint WS_SYSMENU = 0x00080000;
            public const uint WS_THICKFRAME = 0x00040000;
            public const uint WS_GROUP = 0x00020000;
            public const uint WS_TABSTOP = 0x00010000;

            public const uint WS_MINIMIZEBOX = 0x00020000;
            public const uint WS_MAXIMIZEBOX = 0x00010000;

            public const uint WS_TILED = WS_OVERLAPPED;
            public const uint WS_ICONIC = WS_MINIMIZE;
            public const uint WS_SIZEBOX = WS_THICKFRAME;
            public const uint WS_TILEDWINDOW = WS_OVERLAPPEDWINDOW;

            // Common Window Styles

            public const uint WS_OVERLAPPEDWINDOW =
                (WS_OVERLAPPED |
                  WS_CAPTION |
                  WS_SYSMENU |
                  WS_THICKFRAME |
                  WS_MINIMIZEBOX |
                  WS_MAXIMIZEBOX);

            public const uint WS_POPUPWINDOW =
                (WS_POPUP |
                  WS_BORDER |
                  WS_SYSMENU);

            public const uint WS_CHILDWINDOW = WS_CHILD;

            //Extended Window Styles

            public const uint WS_EX_DLGMODALFRAME = 0x00000001;
            public const uint WS_EX_NOPARENTNOTIFY = 0x00000004;
            public const uint WS_EX_TOPMOST = 0x00000008;
            public const uint WS_EX_ACCEPTFILES = 0x00000010;
            public const uint WS_EX_TRANSPARENT = 0x00000020;

            //#if(WINVER >= 0x0400)
            public const uint WS_EX_MDICHILD = 0x00000040;
            public const uint WS_EX_TOOLWINDOW = 0x00000080;
            public const uint WS_EX_WINDOWEDGE = 0x00000100;
            public const uint WS_EX_CLIENTEDGE = 0x00000200;
            public const uint WS_EX_CONTEXTHELP = 0x00000400;

            public const uint WS_EX_RIGHT = 0x00001000;
            public const uint WS_EX_LEFT = 0x00000000;
            public const uint WS_EX_RTLREADING = 0x00002000;
            public const uint WS_EX_LTRREADING = 0x00000000;
            public const uint WS_EX_LEFTSCROLLBAR = 0x00004000;
            public const uint WS_EX_RIGHTSCROLLBAR = 0x00000000;

            public const uint WS_EX_CONTROLPARENT = 0x00010000;
            public const uint WS_EX_STATICEDGE = 0x00020000;
            public const uint WS_EX_APPWINDOW = 0x00040000;

            public const uint WS_EX_OVERLAPPEDWINDOW = (WS_EX_WINDOWEDGE | WS_EX_CLIENTEDGE);
            public const uint WS_EX_PALETTEWINDOW = (WS_EX_WINDOWEDGE | WS_EX_TOOLWINDOW | WS_EX_TOPMOST);
            //#endif /* WINVER >= 0x0400 */

            //#if(_WIN32_WINNT >= 0x0500)
            public const uint WS_EX_LAYERED = 0x00080000;
            //#endif /* _WIN32_WINNT >= 0x0500 */

            //#if(WINVER >= 0x0500)
            public const uint WS_EX_NOINHERITLAYOUT = 0x00100000; // Disable inheritence of mirroring by children
            public const uint WS_EX_LAYOUTRTL = 0x00400000; // Right to left mirroring
                                                            //#endif /* WINVER >= 0x0500 */

            //#if(_WIN32_WINNT >= 0x0500)
            public const uint WS_EX_COMPOSITED = 0x02000000;
            public const uint WS_EX_NOACTIVATE = 0x08000000;
            //#endif /* _WIN32_WINNT >= 0x0500 */
        }



        public static void HidePos(IntPtr handle)
        {
            if (IsWindowVisible(handle))
            {
                ShowWindow(handle, WindowShowStyle.ShowMinNoActivate);
                ShowWindow(handle, WindowShowStyle.Hide);
            }      
        }

        public static void ShowPos(IntPtr handle)
        {
            int GWL_STYLE = -16;
            ////SetWindowPos(handle, INSERTAFTER.HWND_TOP, savedX, saveY, saveW, saveH, 0x0002 | 0x0001);            
            ////Get current style
            ////int lCurStyle = GetWindowLong(handle, (int)WindowStyles.WS_CAPTION);

            //////remove titlebar elements
            ////Get current style
            //int lCurStyle = GetWindowLong(handle, (int)WindowStyles.WS_CAPTION);

            ////remove titlebar elements
            //lCurStyle |= 0xc00000;
            //SetWindowLong(handle, GWL_STYLE, (int)lCurStyle);
            //SetWindowPos(handle, IntPtr.Zero, 0, 0, 0, 0, 0x27);

            SetWindowLong(handle, GWL_STYLE, 0 | WindowStyles.WS_BORDER);
            //Active(handle);
            //SetWindowPos(handle, INSERTAFTER.HWND_NOTOPMOST, 0, 0, 0, 0, 0x0002 | 0x0001);
            //SetWindowPos(handle, IntPtr.Zero, 0, 0, 600, 800, SetWindowPosFlags.SWP_NOSIZE | SetWindowPosFlags.SWP_NOMOVE | SetWindowPosFlags.SWP_SHOWWINDOW | SetWindowPosFlags.SWP_NOACTIVATE);
            //ToggleTitleBar(handle, true);
            Hide(handle);
        }

        public static void ToggleTitleBar(IntPtr hwnd, bool showTitle)
        {
            int style = GetWindowLong(hwnd, -16);
            //if (showTitle)
            //    style |= (int)WindowStyles.WS_CAPTION;
            //else
            //    style &= -12582913;
            if (!showTitle)
                SetWindowLong(hwnd, -16, (int)(style & ~WindowStyles.WS_CAPTION & ~WindowStyles.WS_MAXIMIZEBOX & ~WindowStyles.WS_MINIMIZEBOX & ~WindowStyles.WS_SYSMENU & ~WindowStyles.WS_EX_APPWINDOW));
            else
                SetWindowLong(hwnd, -16, (int)(style | WindowStyles.WS_CAPTION | WindowStyles.WS_MAXIMIZEBOX | WindowStyles.WS_MINIMIZEBOX | WindowStyles.WS_SYSMENU | WindowStyles.WS_EX_APPWINDOW));
            // 0x27
            SetWindowPos(hwnd, INSERTAFTER.HWND_NOTOPMOST, 0, 0, 0, 0, SetWindowPosFlags.SWP_NOSIZE | SetWindowPosFlags.SWP_NOMOVE);
        }

        public static void ShowPos(IntPtr handle, RECT rec)
        {
            //SetWindowPos(handle, IntPtr.Zero, 0, 0, 0, 0, SetWindowPosFlags.SWP_NOSIZE | SetWindowPosFlags.SWP_NOMOVE | SetWindowPosFlags.SWP_SHOWWINDOW | SetWindowPosFlags.SWP_NOACTIVATE);
            //MoveWindow(handle, 0, 0, 600, 800, true);
            //int savedX = rec.Left;
            //int saveY = rec.Top;
            //int saveW = rec.Left - rec.Right;
            //int saveH = rec.Top - rec.Bottom;

            ////SetWindowPos(handle, INSERTAFTER.HWND_NOTOPMOST, 0, 0, 0, 0, 0x0002 | 0x0001);
            ////Get current style
            ////int lCurStyle = GetWindowLong(handle, (int)WindowStyles.WS_CAPTION);

            ////remove titlebar elements
            ////lCurStyle = (int)(lCurStyle | WindowStyles.WS_BORDER | WindowStyles.WS_MAXIMIZEBOX | WindowStyles.WS_MINIMIZEBOX | WindowStyles.WS_SYSMENU);
            ////int GWL_STYLE = -16;
            ////SetWindowLong(handle, GWL_STYLE, (int)lCurStyle);
            ////SetWindowPos(handle, INSERTAFTER.HWND_TOP, 0, 0, 600, 800, 0x0002 | 0x0001);

            //int GWL_STYLE = -16;
            //SetWindowPos(handle, INSERTAFTER.HWND_TOP, savedX, saveY, saveW, saveH, 0x0002 | 0x0001);            
            //////Get current style
            //////int lCurStyle = GetWindowLong(handle, (int)WindowStyles.WS_CAPTION);

            ////////remove titlebar elements
            //////Get current style
            ////int lCurStyle = GetWindowLong(handle, (int)WindowStyles.WS_CAPTION);

            //////remove titlebar elements
            ////lCurStyle |= 0xc00000;
            ////SetWindowLong(handle, GWL_STYLE, (int)lCurStyle);
            ////SetWindowPos(handle, IntPtr.Zero, 0, 0, 0, 0, 0x27);
            ////Active(handle);
          
            //ToggleTitleBar(handle, true);
            //ToggleTitleBar(handle, true);
            Active(handle);
            //MoveWindow(handle, 0, 0, 800, 600, true);
        }


        public enum WindowLocation : byte
        {
            TopLeft = 0,
            TopRight = 1,
            BottomRight = 2,
            BottomLeft = 3,
            Center = 4,
            None = 5,
            TopCenter = 6,
            RightCenter = 7,
            BottomCenter = 8,
            LeftCenter = 9,
        }

        /// <summary>Enumeration of the different ways of showing a window using
        /// ShowWindow</summary>
        public enum WindowShowStyle : uint
        {
            /// <summary>Hides the window and activates another window.</summary>
            /// <remarks>See SW_HIDE</remarks>
            Hide = 0,
            /// <summary>Activates and displays a window. If the window is minimized
            /// or maximized, the system restores it to its original size and
            /// position. An application should specify this flag when displaying
            /// the window for the first time.</summary>
            /// <remarks>See SW_SHOWNORMAL</remarks>
            ShowNormal = 1,
            /// <summary>Activates the window and displays it as a minimized window.</summary>
            /// <remarks>See SW_SHOWMINIMIZED</remarks>
            ShowMinimized = 2,
            /// <summary>Activates the window and displays it as a maximized window.</summary>
            /// <remarks>See SW_SHOWMAXIMIZED</remarks>
            ShowMaximized = 3,
            /// <summary>Maximizes the specified window.</summary>
            /// <remarks>See SW_MAXIMIZE</remarks>
            Maximize = 3,
            /// <summary>Displays a window in its most recent size and position.
            /// This value is similar to "ShowNormal", except the window is not
            /// actived.</summary>
            /// <remarks>See SW_SHOWNOACTIVATE</remarks>
            ShowNormalNoActivate = 4,
            /// <summary>Activates the window and displays it in its current size
            /// and position.</summary>
            /// <remarks>See SW_SHOW</remarks>
            Show = 5,
            /// <summary>Minimizes the specified window and activates the next
            /// top-level window in the Z order.</summary>
            /// <remarks>See SW_MINIMIZE</remarks>
            Minimize = 6,
            /// <summary>Displays the window as a minimized window. This value is
            /// similar to "ShowMinimized", except the window is not activated.</summary>
            /// <remarks>See SW_SHOWMINNOACTIVE</remarks>
            ShowMinNoActivate = 7,
            /// <summary>Displays the window in its current size and position. This
            /// value is similar to "Show", except the window is not activated.</summary>
            /// <remarks>See SW_SHOWNA</remarks>
            ShowNoActivate = 8,
            /// <summary>Activates and displays the window. If the window is
            /// minimized or maximized, the system restores it to its original size
            /// and position. An application should specify this flag when restoring
            /// a minimized window.</summary>
            /// <remarks>See SW_RESTORE</remarks>
            Restore = 9,
            /// <summary>Sets the show state based on the SW_ value specified in the
            /// STARTUPINFO structure passed to the CreateProcess function by the
            /// program that started the application.</summary>
            /// <remarks>See SW_SHOWDEFAULT</remarks>
            ShowDefault = 10,
            /// <summary>Windows 2000/XP: Minimizes a window, even if the thread
            /// that owns the window is hung. This flag should only be used when
            /// minimizing windows from a different thread.</summary>
            /// <remarks>See SW_FORCEMINIMIZE</remarks>
            ForceMinimized = 11
        }

        public struct RECT
        {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner
        }

        enum GetWindow_Cmd : uint
        {
            GW_HWNDFIRST = 0,
            GW_HWNDLAST = 1,
            GW_HWNDNEXT = 2,
            GW_HWNDPREV = 3,
            GW_OWNER = 4,
            GW_CHILD = 5,
            GW_ENABLEDPOPUP = 6
        }

        public static IEnumerable<IntPtr> AllWindows
        {
            get
            {
                IntPtr window = GetWindow(GetForegroundWindow(), GetWindow_Cmd.GW_HWNDFIRST);                
                while (window != IntPtr.Zero)
                {
                    yield return window;
                    window = GetWindow(window, GetWindow_Cmd.GW_HWNDNEXT);
                }
            }
        }

        public static List<IntPtr> AllWindow { get; set; } = new List<IntPtr>();

        public static IntPtr GetHandle(int processId, string className)
        {
            StringBuilder builder = new StringBuilder(100);
            foreach (var window in AllWindows)
            {                
                GetClassName(window, builder, 100);
                if (builder.ToString().IndexOf(className) != -1 || (builder.ToString().Length == 15 && className.Length == 15))
                {
                    GetWindowThreadProcessId(window, out int num);
                    if (num == processId)
                        return window;
                }
            }
            return IntPtr.Zero;
        }

        public static IntPtr GetHandle(string className, string windowText)
        {
            IntPtr window = GetWindow(GetForegroundWindow(), GetWindow_Cmd.GW_HWNDFIRST);
            StringBuilder builder = new StringBuilder(100);
            StringBuilder builderText = new StringBuilder(100);
            while (window != IntPtr.Zero)
            {
                GetClassName(window, builder, 100);
                GetWindowText(window, builderText, 100);
                if (builder.ToString().IndexOf(className) != -1 && builderText.ToString().IndexOf(windowText) != -1)
                {
                    return window;
                }
                window = GetWindow(window, GetWindow_Cmd.GW_HWNDNEXT);
            }
            return IntPtr.Zero;
        }

        public static void Active(Form form)
        {
            form.Show();
            Active(form.Handle);
            form.Activate();
            form.Refresh();
        }

        public static void Active(IntPtr handle)
        {
            RECT rec;
            GetWindowRect(handle, out rec);
            bool isMini = rec.Left == -32000;
            if (!IsWindowVisible(handle))
                ShowWindow(handle, WindowShowStyle.Show);
            if (isMini)
                ShowWindow(handle,  WindowShowStyle.Restore);
            if (GetForegroundWindow() != handle)
                SetForegroundWindow(handle);
        }

        public static bool IsHideOrMini(IntPtr handle)
        {
            return IsWindowVisible(handle) || IsWindowMini(handle);
        }

        public static bool IsShow(IntPtr handle)
        {
            if (IsWindowMini(handle))
                  return false;
            return IsWindowVisible(handle);
        }

        public static bool IsWindowMini(IntPtr handle)
        {
            RECT rec;
            GetWindowRect(handle, out rec);
            bool isMini = rec.Left == -32000;
            return isMini;
        }

        public static void Hide(IntPtr handle)
        {
            ShowWindow(handle, WindowShowStyle.Hide);
        }

        public static int ScreenWidth
        {
            get
            {
                return Screen.PrimaryScreen.Bounds.Width;
            }
        }

        public static int ScreenHight
        {
            get
            {
                return Screen.PrimaryScreen.Bounds.Height - (Screen.PrimaryScreen.Bounds.Bottom - Screen.PrimaryScreen.WorkingArea.Bottom);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle">Handle of window wana to close</param>
        public static void Close(IntPtr handle)
        {
            PostMessage(handle, 0x10, 0, 0);
        }

        public static void MoveEx(Form form, WindowLocation location, int width, int height)
        {
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHight = Screen.PrimaryScreen.Bounds.Height;
            switch (location)
            {
                case WindowLocation.TopLeft:
                    form.Location = new Point(0, 0);
                    break;
                case WindowLocation.TopRight:
                    form.Location = new Point(screenWidth - width, 0);
                    break;
                case WindowLocation.BottomRight:
                    form.Location = new Point(screenWidth - width, screenHight - height - (Screen.PrimaryScreen.Bounds.Bottom - Screen.PrimaryScreen.WorkingArea.Bottom));
                    break;
                case WindowLocation.BottomLeft:
                    form.Location = new Point(0, screenHight - height - (Screen.PrimaryScreen.Bounds.Bottom - Screen.PrimaryScreen.WorkingArea.Bottom));
                    break;
                case WindowLocation.Center:
                    form.Location = new Point((screenWidth - width) / 2, (screenHight - height - (Screen.PrimaryScreen.Bounds.Bottom - Screen.PrimaryScreen.WorkingArea.Bottom)) / 2);
                    break;
                case WindowLocation.None:
                    form.Location = new Point(-1000, -1000);
                    break;
                case WindowLocation.TopCenter:
                    form.Location = new Point((screenWidth - width) / 2, 0);
                    break;
                case WindowLocation.RightCenter:
                    form.Location = new Point(screenWidth - width, (screenHight - height - (Screen.PrimaryScreen.Bounds.Bottom - Screen.PrimaryScreen.WorkingArea.Bottom)) / 2);
                    break;
                case WindowLocation.BottomCenter:
                    form.Location = new Point((screenWidth - width) / 2, screenHight - height - (Screen.PrimaryScreen.Bounds.Bottom - Screen.PrimaryScreen.WorkingArea.Bottom));
                    break;
                case WindowLocation.LeftCenter:
                    form.Location = new Point(0, (screenHight - height - (Screen.PrimaryScreen.Bounds.Bottom - Screen.PrimaryScreen.WorkingArea.Bottom)) / 2);
                    break;
            }
        }

        public static void Move(Form form, WindowLocation location)
        {
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHight = Screen.PrimaryScreen.Bounds.Height;
            switch (location)
            {                
                case WindowLocation.TopLeft: 
                    form.Location = new Point(0, 0); 
                    break;
                case WindowLocation.TopRight: 
                    form.Location = new Point(screenWidth - form.Width, 0); 
                    break;
                case WindowLocation.BottomRight: 
                    form.Location = new Point(screenWidth - form.Width, screenHight - form.Height - (Screen.PrimaryScreen.Bounds.Bottom - Screen.PrimaryScreen.WorkingArea.Bottom)); 
                    break;
                case WindowLocation.BottomLeft: 
                    form.Location = new Point(0, screenHight - form.Height - (Screen.PrimaryScreen.Bounds.Bottom - Screen.PrimaryScreen.WorkingArea.Bottom)); 
                    break;
                case WindowLocation.Center: 
                    form.Location = new Point((screenWidth - form.Width) / 2, (screenHight - form.Height - (Screen.PrimaryScreen.Bounds.Bottom - Screen.PrimaryScreen.WorkingArea.Bottom)) / 2); 
                    break;
                case WindowLocation.None: 
                    form.Location = new Point(-1000, -1000); 
                    break;
                case WindowLocation.TopCenter: 
                    form.Location = new Point((screenWidth - form.Width) / 2, 0); 
                    break;
                case WindowLocation.RightCenter: 
                    form.Location = new Point(screenWidth - form.Width, (screenHight - form.Height - (Screen.PrimaryScreen.Bounds.Bottom - Screen.PrimaryScreen.WorkingArea.Bottom)) / 2); 
                    break;
                case WindowLocation.BottomCenter: 
                    form.Location = new Point((screenWidth - form.Width) / 2, screenHight - form.Height - (Screen.PrimaryScreen.Bounds.Bottom - Screen.PrimaryScreen.WorkingArea.Bottom)); 
                    break;
                case WindowLocation.LeftCenter: 
                    form.Location = new Point(0, (screenHight - form.Height - (Screen.PrimaryScreen.Bounds.Bottom - Screen.PrimaryScreen.WorkingArea.Bottom)) / 2); 
                    break;
            }
        }

        private const int SW_SHOWNOACTIVATE = 4;
        private const int HWND_TOPMOST = -1;
        private const uint SWP_NOACTIVATE = 0x0010;


        public static void ShowInactiveTopmost(Form frm)
        {
            if (frm.WindowState == FormWindowState.Minimized)
            {
                SetWindowPos(frm.Handle.ToInt32(), HWND_TOPMOST,
                frm.Left, frm.Top, frm.Width, frm.Height,
                SWP_NOACTIVATE);
            }
            ShowWindow(frm.Handle, SW_SHOWNOACTIVATE);
        }

        public static void ShowInactiveTopmost(Game game)
        {
            //ShowWindow(game.Handle, SW_SHOWNOACTIVATE);
            //if (game.HX == 0)
            //{
            //    SetWindowPos(game.Handle.ToInt32(), HWND_TOPMOST,
            //     20, 20, 800, 600,
            //     SWP_NOACTIVATE);
            //}
            //else
            //{
            //    SetWindowPos(game.Handle.ToInt32(), HWND_TOPMOST,
            //    game.HX, game.HY, game.HWidth, game.HHeight,
            //    SWP_NOACTIVATE);
            //}
            //SetWindowPos(game.Handle, INSERTAFTER.HWND_NOTOPMOST, 0, 0, 0, 0, SetWindowPosFlags.SWP_NOSIZE | SetWindowPosFlags.SWP_NOMOVE | SetWindowPosFlags.SWP_SHOWWINDOW | SetWindowPosFlags.SWP_NOOWNERZORDER);
            //SetWindowPos(game.Handle, INSERTAFTER.HWND_NOTOPMOST, 0, 0, 0, 0, SetWindowPosFlags.SWP_NOMOVE | SetWindowPosFlags.SWP_NOSIZE | SetWindowPosFlags.SWP_SHOWWINDOW);
            if (game.Left == 0)
            {
                SetWindowPos(game.Handle, IntPtr.Zero, 20, 20, 1030, 797, SetWindowPosFlags.SWP_NOZORDER | (int)SWP_NOACTIVATE);
            }
            else
            {
                SetWindowPos(game.Handle, IntPtr.Zero, game.Left, game.Right, game.Width, game.Height, SetWindowPosFlags.SWP_NOZORDER | (int)SWP_NOACTIVATE);
            }
            //SetWindowPos(game.Handle, IntPtr.Zero, 20, 20, 800, 600, SetWindowPosFlags.SWP_NOZORDER | (int)SWP_NOACTIVATE);
            ShowWindow(game.Handle, SW_SHOWNOACTIVATE);
        }

        public static void ShowInactive(Game game)
        {
            //ShowWindow(game.Handle, SW_SHOWNOACTIVATE);
            //if (game.HX == 0)
            //{
            //    SetWindowPos(game.Handle.ToInt32(), HWND_TOPMOST,
            //     20, 20, 800, 600,
            //     SWP_NOACTIVATE);
            //}
            //else
            //{
            //    SetWindowPos(game.Handle.ToInt32(), HWND_TOPMOST,
            //    game.HX, game.HY, game.HWidth, game.HHeight,
            //    SWP_NOACTIVATE);
            //}
            //SetWindowPos(game.Handle, INSERTAFTER.HWND_NOTOPMOST, 0, 0, 0, 0, SetWindowPosFlags.SWP_NOSIZE | SetWindowPosFlags.SWP_NOMOVE | SetWindowPosFlags.SWP_SHOWWINDOW | SetWindowPosFlags.SWP_NOOWNERZORDER);
            //SetWindowPos(game.Handle, INSERTAFTER.HWND_NOTOPMOST, 0, 0, 0, 0, SetWindowPosFlags.SWP_NOMOVE | SetWindowPosFlags.SWP_NOSIZE | SetWindowPosFlags.SWP_SHOWWINDOW);
            if (IsWindowMini(game.Handle))
            {
                if (game.Left == 0)
                {
                    SetWindowPos(game.Handle, IntPtr.Zero, 20, 20, 1030, 797, SetWindowPosFlags.SWP_NOZORDER | (int)SWP_NOACTIVATE);
                }
                else
                {
                    SetWindowPos(game.Handle, IntPtr.Zero, game.Left, game.Right, game.Width, game.Height, SetWindowPosFlags.SWP_NOZORDER | (int)SWP_NOACTIVATE);
                }
            }
            //MoveWindow(game.Handle, -1200, -1200, 1200, 1200, true);
            //SetWindowPos(game.Handle, IntPtr.Zero, 20, 20, 800, 600, SetWindowPosFlags.SWP_NOZORDER | (int)SWP_NOACTIVATE);
            ShowWindow(game.Handle, SW_SHOWNOACTIVATE);
        }

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        static extern bool SetWindowPos(
         int hWnd,             // Window handle
         int hWndInsertAfter,  // Placement-order handle
         int X,                // Horizontal position
         int Y,                // Vertical position
         int cx,               // Width
         int cy,               // Height
         uint uFlags);         // Window positioning flags

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, WindowShowStyle nCmdShow);        
        [DllImport("user32.dll")]
        public static extern bool PostMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        static extern bool BringWindowToTop(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern bool IsWindowVisible(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr GetWindow(IntPtr hWnd, GetWindow_Cmd uCmd);
        [DllImport("user32.dll")]
        public static extern int SetWindowText(IntPtr hWnd, string text);
        [DllImport("user32.dll")]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);
        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern bool EnableWindow(IntPtr hwnd, bool enabled);
        [DllImport("user32.dll")]
        public static extern bool ShowWindowAsync(IntPtr hWnd, WindowShowStyle nCmdShow);
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, ExactSpelling = true, SetLastError = true)]
        public static extern void MoveWindow(IntPtr hwnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);
        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
       public static extern int RegisterWindowMessage(string lpString);
    }
}
