using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Update
{
    class Settings
    {
        public static int MinSegmentSize = 0x32000;
        public static int MinSegmentLeftToStartNewSegment = 30;
        public static int RetryDelay = 5;
        public static int MaxRetries = 10;
        public static int MaxSegments = 64;
        public static string DownloadFolder = Application.StartupPath + "\\Downloads";
        public static string ProxyAddress = string.Empty;
        public static string ProxyUserName = string.Empty;
        public static string ProxyPassword = string.Empty;
        public static string ProxyDomain = string.Empty;
        public static bool UseProxy = false;
        public static bool ProxyByPassOnLocal = false;
        public static int ProxyPort = 80;
    }
}
