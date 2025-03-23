using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace _i
{
    public struct MEMORY_BASIC_INFORMATION
    {
        public IntPtr BaseAddress;
        public IntPtr AllocationBase;
        public uint AllocationProtect;
        public uint RegionSize;
        public uint State;
        public uint Protect;
        public uint Type;
    }

    public class AOB
    {
        public bool IsSearchBaseImg { get; set; }

        protected uint ProcessID;
        public AOB(uint ProcessID)
        {
            this.ProcessID = ProcessID;
        }

        [DllImport("kernel32.dll")]
        protected static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] buffer, uint size, int lpNumberOfBytesRead);
        [DllImport("kernel32.dll")]
        protected static extern int VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress, out MEMORY_BASIC_INFORMATION lpBuffer, int dwLength);


        public static List<MEMORY_BASIC_INFORMATION> GetMemInfo(int processId)
        {
            IntPtr pHandle = Process.GetProcessById(processId).Handle;
            List<MEMORY_BASIC_INFORMATION> list = new List<MEMORY_BASIC_INFORMATION>();
            IntPtr Addy = new IntPtr();
            while (true)
            {
                MEMORY_BASIC_INFORMATION MemInfo = new MEMORY_BASIC_INFORMATION();
                int MemDump = VirtualQueryEx(pHandle, Addy, out MemInfo, Marshal.SizeOf(MemInfo));
                if (MemDump == 0) break;
                if ((MemInfo.State & 0x1000) != 0 && (MemInfo.Protect & 0x100) == 0)
                    list.Add(MemInfo);
                Addy = new IntPtr(MemInfo.BaseAddress.ToInt32() + (int)MemInfo.RegionSize);
            }
            return list;
        }

        protected List<MEMORY_BASIC_INFORMATION> MemoryRegion { get; set; }

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
                Addy = new IntPtr(MemInfo.BaseAddress.ToInt32() + (int)MemInfo.RegionSize);
            }
        }

        public static bool Compare(byte[] input, byte[] pattern, int index)
        {
            for (int i = 0; i < pattern.Length; i++)
            {
                if (pattern[i] == 0xFF)
                    continue;
                if (pattern[i] == 0x01)
                {
                    if (input[index + i] == 0)
                        return false;
                    else
                        continue;
                }
                if (input[index + i] != pattern[i])
                    return false;
            }
            return true;
        }

        protected IntPtr Scan(byte[] sIn, byte[] sFor)
        {
            for (int i = 0; i < sIn.Length - sFor.Length; i++)
            {
                if (Compare(sIn, sFor, i))
                    return (IntPtr)i;
            }
            return IntPtr.Zero;
            //int[] sBytes = new int[256]; int Pool = 0;
            //int End = sFor.Length - 1;
            //for (int i = 0; i < 256; i++)
            //    sBytes[i] = sFor.Length;
            //for (int i = 0; i < End; i++)
            //    sBytes[sFor[i]] = End - i;
            //while (Pool <= sIn.Length - sFor.Length)
            //{
            //    for (int i = End; sIn[Pool + i] == sFor[i]; i--)
            //    {
            //        if (i == 0) return new IntPtr(Pool);
            //    }
            //    Pool += sBytes[sIn[Pool + End]];
            //}
            //return IntPtr.Zero;
        }

        protected List<int> ScanEx(byte[] sIn, byte[] sFor)
        {
            List<int> result = new List<int>();
            for (int i = 0; i < sIn.Length - sFor.Length; i++)
            {
                if (Compare(sIn, sFor, i))
                    result.Add(i);
            }
            return result;
            //int[] sBytes = new int[256]; int Pool = 0;
            //int End = sFor.Length - 1;
            //for (int i = 0; i < 256; i++)
            //    sBytes[i] = sFor.Length;
            //for (int i = 0; i < End; i++)
            //    sBytes[sFor[i]] = End - i;
            //while (Pool <= sIn.Length - sFor.Length)
            //{
            //    for (int i = End; sIn[Pool + i] == sFor[i]; i--)
            //    {
            //        if (i == 0) return new IntPtr(Pool);
            //    }
            //    Pool += sBytes[sIn[Pool + End]];
            //}
            //return IntPtr.Zero;
        }

        public IntPtr Search(byte[] Pattern, uint startAddress, uint endAddress)
        {
            Process Game = Process.GetProcessById((int)this.ProcessID);
            if (Game.Id == 0) return IntPtr.Zero;
            MemoryRegion = new List<MEMORY_BASIC_INFORMATION>();
            MemInfo(Game.Handle);
            for (int i = 0; i < MemoryRegion.Count; i++)
            {
                if ((uint)MemoryRegion[i].BaseAddress < startAddress || (uint)MemoryRegion[i].BaseAddress > endAddress)
                    continue;
                //Main.AddLog(MemoryRegion[i].BaseAddress.ToString("X8"));
                byte[] buff = new byte[MemoryRegion[i].RegionSize];
                ReadProcessMemory(Game.Handle, MemoryRegion[i].BaseAddress, buff, MemoryRegion[i].RegionSize, 0);
                IntPtr Result = Scan(buff, Pattern);
                if (Result != IntPtr.Zero)
                {
                    //Main.PushLogEx(MemoryRegion[i].BaseAddress.ToString("X8"));
                    //Main.AddLog(MemoryRegion[i].Type.ToString("X8"));
                    return new IntPtr(MemoryRegion[i].BaseAddress.ToInt32() + Result.ToInt32());
                }
            }
            return IntPtr.Zero;
        }

        public IntPtr SearchPrivateRegion(byte[] Pattern, uint startAddress, uint endAddress)
        {
            Process Game = Process.GetProcessById((int)this.ProcessID);
            if (Game.Id == 0) return IntPtr.Zero;
            MemoryRegion = new List<MEMORY_BASIC_INFORMATION>();
            MemInfo(Game.Handle);
            for (int i = 0; i < MemoryRegion.Count; i++)
            {
                if ((uint)MemoryRegion[i].BaseAddress < startAddress || (uint)MemoryRegion[i].BaseAddress > endAddress || MemoryRegion[i].Type != 0x20000)
                    continue;
                byte[] buff = new byte[MemoryRegion[i].RegionSize];
                ReadProcessMemory(Game.Handle, MemoryRegion[i].BaseAddress, buff, MemoryRegion[i].RegionSize, 0);
                IntPtr Result = Scan(buff, Pattern);
                if (Result != IntPtr.Zero)
                {
                    //Main.PushLogEx(MemoryRegion[i].BaseAddress.ToString("X8"));
                    //Main.PushLogEx(MemoryRegion[i].BaseAddress.ToString("X8"));
                    //Main.AddLog(MemoryRegion[i].Type.ToString("X8"));
                    return new IntPtr(MemoryRegion[i].BaseAddress.ToInt32() + Result.ToInt32());
                }
            }
            return IntPtr.Zero;
        }
        public IntPtr SearchPrivateRegionWithoutAddress(byte[] Pattern, uint startAddress, uint endAddress, int wtAddress, int wtAddress2 = 0)
        {
            Process Game = Process.GetProcessById((int)this.ProcessID);
            if (Game.Id == 0) return IntPtr.Zero;
            MemoryRegion = new List<MEMORY_BASIC_INFORMATION>();
            MemInfo(Game.Handle);
            for (int i = 0; i < MemoryRegion.Count; i++)
            {
                if ((uint)MemoryRegion[i].BaseAddress < startAddress || (uint)MemoryRegion[i].BaseAddress > endAddress || MemoryRegion[i].Type != 0x20000)
                    continue;
                if ((wtAddress > (int)MemoryRegion[i].BaseAddress && wtAddress < ((int)MemoryRegion[i].BaseAddress + (int)MemoryRegion[i].RegionSize)))
                    continue;
                if (wtAddress2 != 0)
                {
                    if ((wtAddress2 > (int)MemoryRegion[i].BaseAddress && wtAddress2 < ((int)MemoryRegion[i].BaseAddress + (int)MemoryRegion[i].RegionSize)))
                        continue;
                }
                byte[] buff = new byte[MemoryRegion[i].RegionSize];
                ReadProcessMemory(Game.Handle, MemoryRegion[i].BaseAddress, buff, MemoryRegion[i].RegionSize, 0);
                IntPtr Result = Scan(buff, Pattern);
                if (Result != IntPtr.Zero)
                {
                    //Main.PushLogEx(MemoryRegion[i].BaseAddress.ToString("X8"));
                    //Main.PushLogEx(MemoryRegion[i].BaseAddress.ToString("X8"));
                    //Main.AddLog(MemoryRegion[i].Type.ToString("X8"));
                    return new IntPtr(MemoryRegion[i].BaseAddress.ToInt32() + Result.ToInt32());
                }
            }
            return IntPtr.Zero;
        }

        public IntPtr SearchPrivateRegionEx(byte[] Pattern, uint skippAddress)
        {
            Process Game = Process.GetProcessById((int)this.ProcessID);
            if (Game.Id == 0) return IntPtr.Zero;
            MemoryRegion = new List<MEMORY_BASIC_INFORMATION>();
            MemInfo(Game.Handle);
            for (int i = 0; i < MemoryRegion.Count; i++)
            {
                if (((uint)MemoryRegion[i].BaseAddress > skippAddress && (uint)MemoryRegion[i].BaseAddress < skippAddress) || MemoryRegion[i].Type != 0x20000)
                    continue;
                byte[] buff = new byte[MemoryRegion[i].RegionSize];
                ReadProcessMemory(Game.Handle, MemoryRegion[i].BaseAddress, buff, MemoryRegion[i].RegionSize, 0);
                IntPtr Result = Scan(buff, Pattern);
                if (Result != IntPtr.Zero)
                {
                    //Main.PushLogEx(MemoryRegion[i].BaseAddress.ToString("X8"));
                    //Main.PushLogEx(MemoryRegion[i].BaseAddress.ToString("X8"));
                    //Main.AddLog(MemoryRegion[i].Type.ToString("X8"));
                    return new IntPtr(MemoryRegion[i].BaseAddress.ToInt32() + Result.ToInt32());
                }
            }
            return IntPtr.Zero;
        }

        public List<int> SearchAnswer(byte[] Pattern)
        {
            List<int> Result = new List<int>();
            List<int> ResultEx = new List<int>();
            Process Game = Process.GetProcessById((int)this.ProcessID);
            if (Game.Id == 0) return Result;
            MemoryRegion = new List<MEMORY_BASIC_INFORMATION>();
            MemInfo(Game.Handle);
            //MemoryRegion = MemoryRegion.OrderBy(x => (int)x.BaseAddress).ToList();
            for (int i = 0; i < MemoryRegion.Count; i++)
            {
                if (MemoryRegion[i].Type != 0x20000)
                    continue;
                byte[] buff = new byte[MemoryRegion[i].RegionSize];
                ReadProcessMemory(Game.Handle, MemoryRegion[i].BaseAddress, buff, MemoryRegion[i].RegionSize, 0);
                Result = ScanEx(buff, Pattern);
                if (Result.Count > 0)
                {
                    for (int j = 0; j < Result.Count; j++)
                    {
                        Result[j] = (int)MemoryRegion[i].BaseAddress + Result[j];
                    }
                    for(int k = 0; k < Result.Count; k++)
                        ResultEx.Add(Result[k]);
                    if (ResultEx.Count >= 4)
                        return ResultEx;
                }               
                //if (Result != IntPtr.Zero)
                //{
                //    Main.PushLogEx(MemoryRegion[i].BaseAddress.ToString("X8"));
                //    //Main.PushLogEx(MemoryRegion[i].BaseAddress.ToString("X8"));
                //    //Main.AddLog(MemoryRegion[i].Type.ToString("X8"));
                //    return new IntPtr(MemoryRegion[i].BaseAddress.ToInt32() + Result.ToInt32());
                //}
            }
            return Result;
        }

        public List<int> SearchPrivateRegionEx(byte[] Pattern, uint startAddress, uint endAddress)
        {
            List<int> Result = new List<int>();
            Process Game = Process.GetProcessById((int)this.ProcessID);
            if (Game.Id == 0) return Result;
            MemoryRegion = new List<MEMORY_BASIC_INFORMATION>();
            MemInfo(Game.Handle);
            for (int i = 0; i < MemoryRegion.Count; i++)
            {
                if ((uint)MemoryRegion[i].BaseAddress < startAddress || (uint)MemoryRegion[i].BaseAddress > endAddress || MemoryRegion[i].Type != 0x20000)
                    continue;
                byte[] buff = new byte[MemoryRegion[i].RegionSize];
                ReadProcessMemory(Game.Handle, MemoryRegion[i].BaseAddress, buff, MemoryRegion[i].RegionSize, 0);
                Result = ScanEx(buff, Pattern);
                if (Result.Count > 0)
                {
                    for (int j = 0; j < Result.Count; j++)
                    {
                        Result[j] = (int)MemoryRegion[i].BaseAddress + Result[j];
                    }
                    return Result;
                }
                //if (Result != IntPtr.Zero)
                //{
                //    Main.PushLogEx(MemoryRegion[i].BaseAddress.ToString("X8"));
                //    //Main.PushLogEx(MemoryRegion[i].BaseAddress.ToString("X8"));
                //    //Main.AddLog(MemoryRegion[i].Type.ToString("X8"));
                //    return new IntPtr(MemoryRegion[i].BaseAddress.ToInt32() + Result.ToInt32());
                //}
            }
            return Result;
        }

        public IntPtr AobScan(byte[] Pattern, int startAddress)
        {
            Process Game = Process.GetProcessById((int)this.ProcessID);
            if (Game.Id == 0) return IntPtr.Zero;
            MemoryRegion = new List<MEMORY_BASIC_INFORMATION>();
            MemInfo(Game.Handle);
            for (int i = 0; i < MemoryRegion.Count; i++)
            {
                if ((int)MemoryRegion[i].BaseAddress < startAddress)
                    continue;
                byte[] buff = new byte[MemoryRegion[i].RegionSize];
                ReadProcessMemory(Game.Handle, MemoryRegion[i].BaseAddress, buff, MemoryRegion[i].RegionSize, 0);
                IntPtr Result = Scan(buff, Pattern);
                if (Result != IntPtr.Zero)
                    return new IntPtr(MemoryRegion[i].BaseAddress.ToInt32() + Result.ToInt32());
            }
            return IntPtr.Zero;
        }

        public IntPtr AobScan(byte[] Pattern)
        {
            Process Game = Process.GetProcessById((int)this.ProcessID);
            if (Game.Id == 0) return IntPtr.Zero;
            MemoryRegion = new List<MEMORY_BASIC_INFORMATION>();
            MemInfo(Game.Handle);
            for (int i = 0; i < MemoryRegion.Count; i++)
            {
                byte[] buff = new byte[MemoryRegion[i].RegionSize];
                ReadProcessMemory(Game.Handle, MemoryRegion[i].BaseAddress, buff, MemoryRegion[i].RegionSize, 0);
                IntPtr Result = Scan(buff, Pattern);
                if (Result != IntPtr.Zero)
                    return new IntPtr(MemoryRegion[i].BaseAddress.ToInt32() + Result.ToInt32());
            }
            return IntPtr.Zero;
        }

        public static string StateToString(uint state)
        {
            if (state == 0x100)
                return "MEM_COMMIT";
            else if (state == 0x10000)
                return "MEM_FREE";
            else if (state == 0x2000)
                return "MEM_RESERVE";
            else
                return "UNKNOWN";
        }

        public static string TypeToString(uint state)
        {
            if (state == 0x1000000)
                return "MEM_IMAGE";
            else if (state == 0x40000)
                return "MEM_MAPPED";
            else if (state == 0x20000)
                return "MEM_PRIVATE";
            else
                return "UNKNOWN";
        }

    }
}