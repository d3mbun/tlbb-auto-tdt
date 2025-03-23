using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace _i
{
    class ArrayOfByte
    {
        protected int ProcessID;

        public ArrayOfByte(int ProcessID)
        {
            this.ProcessID = ProcessID;
        }

        [DllImport("kernel32.dll")]
        protected static extern bool ReadProcessMemory(IntPtr hProcess, int lpBaseAddress, byte[] buffer, uint size, int lpNumberOfBytesRead);
        [DllImport("kernel32.dll")]
        protected static extern int VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress, out MEMORY_BASIC_INFORMATION lpBuffer, int dwLength);
    
        [StructLayout(LayoutKind.Sequential)]
        protected struct MEMORY_BASIC_INFORMATION
        {
            public int BaseAddress;
            public IntPtr AllocationBase;
            public uint AllocationProtect;
            public uint RegionSize;
            public uint State;
            public uint Protect;
            public uint Type;
        }

        protected List<MEMORY_BASIC_INFORMATION> MemoryRegion { get; set; }
        //protected List<byte[]> MemoryDump { get; set; }

        protected void MemInfo(IntPtr pHandle)
        {
            IntPtr Addy = new IntPtr();
            while (true)
            {
                MEMORY_BASIC_INFORMATION MemInfo = new MEMORY_BASIC_INFORMATION();
                int MemDump = VirtualQueryEx(pHandle, Addy, out MemInfo, Marshal.SizeOf(MemInfo));
                if (MemDump == 0) break;
                if ((MemInfo.State & 0x1000) != 0 && (MemInfo.Protect & 0x100) == 0)
                    MemoryRegion.Add(MemInfo);
                Addy = new IntPtr(MemInfo.BaseAddress + (int)MemInfo.RegionSize);
            }
        }

        protected int Scan(byte[] sIn, int[] sFor, int Pool = 0)
        {
            //int[] sBytes = new int[256];            
            int End = sFor.Length - 1;
            //for (int i = 0; i < 256; i++)
            //    sBytes[i] = sFor.Length;
            //for (int i = 0; i < End; i++)
            //    sBytes[sFor[i]] = End - i;
            while (Pool <= sIn.Length - sFor.Length)
            {
                for (int i = End; sFor[i] == -1 || (sFor[i] == 256 && sIn[Pool + i] > 1) || sIn[Pool + i] == sFor[i]; i--)
                    if (i == 0)
                        return Pool;
                //Pool += sBytes[sIn[Pool + End]];
                Pool++;
            }
            return 0;
        }

        public int LastMemIndex { get; set; }
        public int LastPool { get; set; }

        public byte[] LastBuff { get; set; }

        public void Flush()
        {
            //MemoryDump = null;
        }

        public uint Search(string hex)
        {
            return Search(Hex2Arr(hex));
        }

        public uint SearchNext(string hex)
        {
            return SearchNext(Hex2Arr(hex));
        }

        public uint Search(int[] Pattern)
        {
            //MemoryDump = new List<byte[]>();
            LastMemIndex = 0;
            LastPool = 0;
            Process Game = Process.GetProcessById(ProcessID);
            if (Game.Id == 0) return 0;
            MemoryRegion = new List<MEMORY_BASIC_INFORMATION>();
            MemInfo(Game.Handle);
            for (int i = 0; i < MemoryRegion.Count; i++)
            {
                if (IsSearchBaseImg)
                {
                    if(MemoryRegion[i].State == 0x1000 && MemoryRegion[i].Protect == 0x4 && MemoryRegion[i].Type == 0x20000 && MemoryRegion[i].AllocationProtect == 0x4 && MemoryRegion[i].RegionSize < 0xF00000)
                    {

                    }
                    else
                    {
                        continue;
                    }
                }
                byte[] buff = new byte[MemoryRegion[i].RegionSize];
                ReadProcessMemory(Game.Handle, MemoryRegion[i].BaseAddress, buff, MemoryRegion[i].RegionSize, 0);
                LastBuff = buff;
                //MemoryDump.Add(buff);
                LastMemIndex = i;
                int Result = Scan(buff, Pattern);
                if (Result != 0)
                {
                    LastPool = Result;
                    RegionAdr = MemoryRegion[i].BaseAddress; 
                    //IsSearchBaseImg = false;
                    return (uint)(MemoryRegion[i].BaseAddress + Result);
                }
            }
            //IsSearchBaseImg = false;
            return 0;
        }

        public bool IsSearchBaseImg { get; set; }

        public int RegionAdr { get; set; } = 0;

        public uint SearchNext(int[] Pattern)
        {
            if(MemoryRegion == null)
                return Search(Pattern);           
            Process Game = Process.GetProcessById((int)this.ProcessID);
            if (LastPool != 0 && LastMemIndex < MemoryRegion.Count)
            {
                int Result = Scan(LastBuff, Pattern, LastPool + 1);
                if (Result != 0)
                {
                    LastPool = Result;
                    return (uint)(MemoryRegion[LastMemIndex].BaseAddress + Result);
                }
            }
            for (int i = LastMemIndex + 1; i < MemoryRegion.Count; i++)
            {
                if (IsSearchBaseImg)
                {
                    if (MemoryRegion[i].State == 0x1000 && MemoryRegion[i].Protect == 0x4 && MemoryRegion[i].Type == 0x20000 && MemoryRegion[i].AllocationProtect == 0x4 && MemoryRegion[i].RegionSize < 0xF00000)
                    {

                    }
                    else
                    {
                        continue;
                    }
                }
                byte[] buff = new byte[MemoryRegion[i].RegionSize];
                ReadProcessMemory(Game.Handle, MemoryRegion[i].BaseAddress, buff, MemoryRegion[i].RegionSize, 0);
                //MemoryDump.Add(buff);
                LastBuff = buff;
                LastMemIndex = i;
                int Result = Scan(buff, Pattern);
                if (Result != 0)
                {
                    LastPool = Result;
                    RegionAdr = MemoryRegion[i].BaseAddress;
                    //IsSearchBaseImg = false;
                    return (uint)MemoryRegion[i].BaseAddress + (uint)Result;
                }
            }
            LastPool = 0;
            //IsSearchBaseImg = false;
            return 0;
        }

        public static int[] Hex2Arr(string hex)
        {
            hex = hex.Replace(" ", "");
            if (hex.Length % 2 != 0)
            {
                hex = hex + "0";
            }
            int[] buff = new int[hex.Length / 2];
            for (int i = 0; i < buff.Length; i++)
            {
                buff[i] = Hex2Int(hex.Substring(i * 2, 2));
            }
            return buff;
        }

        public static int Hex2Int(string hex)
        {
            if (hex.Contains("?"))
                return -1;
            if (hex.Contains("#"))
                return 256;
            int result = -1;
            int.TryParse(hex, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out result);
            return result;
        }
    }
}
