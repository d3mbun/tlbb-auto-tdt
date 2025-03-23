using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace _i
{
    class USER32
    {
        public static int VK_LSHIFT = 0xA0;
        public static int VK_RSHIFT = 0xA1;
        public static int VK_LCONTROL = 0xA2;
        public static int VK_RCONTROL = 0xA3;

        [DllImport("user32.dll")]
        public static extern int GetKeyState(int nVirtKey);
        public static bool IsPressed(int key)
        {
            int KEY_PRESSED = 0x8000;
            return Convert.ToBoolean(USER32.GetKeyState(key) & KEY_PRESSED);
        }

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
    }
}
