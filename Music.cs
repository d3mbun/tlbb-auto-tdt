using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Media;

namespace _i
{
    class Music
    {
     

        public static void PlayPM()
        {
            if (Main.IsMute)
                return;
            SoundPlayer player = new System.Media.SoundPlayer();
            player.Stream = Properties.Resources.pm;
            player.Play();
        }

        public static void PlayChat()
        {
            SoundPlayer player = new System.Media.SoundPlayer();
            player.Stream = Properties.Resources.chatex;
            player.Play();
        }



        private static bool isOpen = false;
        public static string path = Global.APPPath + "\\alarm.mp3";
        public static string pathTele = Global.APPPath + "\\temp.mp3";
        public static string Path
        {
            get
            {
                return path;
            }
            set
            {
                path = value;
            }
        }

        private static void Open()
        {
            mciSendString("open \"" + Path + "\" type mpegvideo alias MediaFile", null, 0, IntPtr.Zero);
            isOpen = true;
        }

        private static void Pause()
        {
            try { mciSendString("stop MediaFile", null, 0, IntPtr.Zero); }
            catch { }
        }

        public static void Play()
        {
            if (Global.Mute)
                return;
            if (Main.IsMute)
                return;
            if (!isOpen)
            {
                Open();
                mciSendString("play MediaFile REPEAT", null, 0, IntPtr.Zero);
            }
        }

     

        public static void ForcePlay()
        {
            if (!isOpen)
            {
                Open();
                mciSendString("play MediaFile REPEAT", null, 0, IntPtr.Zero);
            }
        }

        public static void ForcePlayTemp()
        {
            mciSendString("close MediaFile", null, 0, IntPtr.Zero);
            mciSendString("open \"" + pathTele + "\" type mpegvideo alias MediaFile", null, 0, IntPtr.Zero);
            mciSendString("play MediaFile", null, 0, IntPtr.Zero);
        }

        public static void Stop()
        {
            mciSendString("close MediaFile", null, 0, IntPtr.Zero);
            isOpen = false;
        }

        [DllImport("winmm.dll")]
        private static extern long mciSendString(string stay, StringBuilder strbuilder, int width, IntPtr sign);
    }
}