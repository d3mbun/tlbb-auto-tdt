using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;
using System.Diagnostics;

namespace _i
{
    class Memory
    {

        public IntPtr Id { get; set; }

        public int ProcessID { get; set; }
        public int BaseGameAdd { get; set; }

        public Memory(int processId)
        {
            ProcessID = processId;
            Id = OpenProcess(0x1F0FFF, false, processId);
        }


        public static int Hex2Int(string hex)
        {
            if (hex == "??")
                return -1;
            int.TryParse(hex, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out int result);
            return result;
        }

        public static byte Hex2Byte(string hex)
        {
            byte.TryParse(hex, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out byte result);
            return result;
        }

        public static byte[] Hex2ByteArr(string hex)
        {
            byte[] arr = new byte[hex.Length / 2];
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = Hex2Byte(hex.Substring(i * 2, 2));
            }
            return arr;
        }

        public uint GetModuleAddress(string moduleName)
        {
            Process process = Process.GetProcessById(ProcessID);
            for (int i = 0; i < process.Modules.Count; i++)
            {
                if (process.Modules[i].ModuleName.ToLower() == moduleName.ToLower())
                {
                    return (uint)process.Modules[i].BaseAddress;
                }
            }
            return 0;
        }

        public string ReverseString(string input)
        {
            String temp = "";
            if (input.Length % 2 != 0)
            {
                input = "0" + input;
            }
            for (int i = input.Length / 2 - 1; i >= 0; i--)
            {
                temp += input.Substring(i * 2, 2);
            }
            return temp;
        }


        public static bool Compare(byte[] input, int[] pattern, int index)
        {
            for (int i = 0; i < pattern.Length; i++)
            {               
                if (pattern[i] == -1)
                    continue;
                if (pattern[i] == 257)
                {
                    if (input[index + i] == 0)
                        return false;
                    else
                        continue;
                }
                if (input[index + i] != pattern[i])
                {
                    return false;
                }
            }
            return true;
        }

        public int Scan(string hex, string moduleName)
        {
            Process process = Process.GetProcessById(ProcessID);
            for (int i = 0; i < process.Modules.Count; i++)
            {
                if (process.Modules[i].ModuleName.ToLower().Contains(moduleName.ToLower()))
                {
                    return Scan(hex, (int)process.Modules[i].BaseAddress, (int)process.Modules[i].BaseAddress + process.Modules[i].ModuleMemorySize, 0);
                }
            }
            return 0;
        }

        public int ScanStart(string hex, string moduleName, int startAddress)
        {
            Process process = Process.GetProcessById(ProcessID);
            for (int i = 0; i < process.Modules.Count; i++)
            {
                if (process.Modules[i].ModuleName.ToLower().Contains(moduleName.ToLower()))
                {
                    return Scan(hex, (int)process.Modules[i].BaseAddress + (startAddress - (int)process.Modules[i].BaseAddress), (int)process.Modules[i].BaseAddress + process.Modules[i].ModuleMemorySize, 0);
                }
            }
            return 0;
        }

        public int BytesCount;

        public int Scan(string hex, string moduleName, int index)
        {
            Process process = Process.GetProcessById(ProcessID);
            for (int i = 0; i < process.Modules.Count; i++)
            {
                if (process.Modules[i].ModuleName.ToLower() == moduleName.ToLower())
                {
                    return Scan(hex, (int)process.Modules[i].BaseAddress, (int)process.Modules[i].BaseAddress + process.Modules[i].ModuleMemorySize, index);
                }
            }
            return 0;
        }

        public int Scan(string hex)
        {
            return Scan(hex, -1, -1, 0);
        }

        public int Scan(string hex, int index)
        {
            return Scan(hex, -1, -1, index);
        }

        public int ScanString(string s, int startAdd, int endAdd, int index)
        {
            return Scan(ConverterEx.String2Hex(s), startAdd, endAdd, index);
        }

        public int ScanString(string s)
        {
            return Scan(ConverterEx.String2Hex(s));
        }

        public static int BufferSize = 20248;

        public int Scan(string hex, int startAddress, int endAddress, int index)
        {
            int address = 0;
            int count = 0;
            int result = 0;

            try
            {
                if (startAddress == -1)
                    startAddress = (int)Process.GetProcessById(ProcessID).MainModule.BaseAddress;
                if (endAddress == -1)
                    endAddress = startAddress + Process.GetProcessById(ProcessID).MainModule.ModuleMemorySize;
            }
            catch
            {
                startAddress = 0x40000;
                endAddress = 0x88FFFF;
            }

            if(startAddress == 0 && endAddress == 0)
            {
                startAddress = 0x00000000;
                endAddress = 0x7FFFFFFF;
            }

            int[] byteSearch = ConverterEx.Hex2IntArr(hex);
            byte[] bufferSearch = new byte[BufferSize + byteSearch.Length];

            int loopCount = (endAddress - startAddress) / BufferSize;
            int outOfBounds = (endAddress - startAddress) % BufferSize;

            for (int i = 0; i < loopCount; i++)
            {
                ReadProcessMemory(Id, startAddress + i * BufferSize, bufferSearch, bufferSearch.Length, out BytesCount);
                for (int j = 0; j < BufferSize; j++)
                {
                    if (Compare(bufferSearch, byteSearch, j))
                    {
                        address = address = j + i * BufferSize + startAddress;
                        if (count++ >= index)
                            return address;
                        result = address;
                    }
                }
                //if (byteSearch.Length > 1)
                //{
                //    byte[] bufferSearchMore = new byte[byteSearch.Length * 2];
                //    ReadProcessMemory(Id, startAddress + i * BufferSize - byteSearch.Length + 1, bufferSearchMore, bufferSearchMore.Length, 0);
                //    for (int j = 0; j < byteSearch.Length; j++)
                //    {
                //        if (Compare(bufferSearchMore, byteSearch, j))
                //        {
                //            address = address = j + i * BufferSize + startAddress;
                //            if (count++ >= index)
                //                return address;
                //            result = address;
                //        }
                //    }
                //}
            }
            if (outOfBounds > 0)
            {
                bufferSearch = new byte[outOfBounds + byteSearch.Length];
                ReadProcessMemory(Id, startAddress + loopCount * BufferSize, bufferSearch, bufferSearch.Length, 0);
                for (int j = 0; j < outOfBounds; j++)
                {
                    if (Compare(bufferSearch, byteSearch, j))
                    {
                        address = j + startAddress + loopCount * BufferSize;
                        if (count++ >= index)
                            return address;
                        result = address;
                    }
                }
            }
            return result;
        }
        public static int BaseGameAddreadnew(int ProcessID)
        {

            int baseAddress = 0;
            Process process = Process.GetProcessById((int)ProcessID);
            baseAddress = (int)process.Modules[0].BaseAddress;
            return baseAddress;

        }
        public static int Scan(string hex, int startAddress, int endAddress, int index, int procId)
        {
            int address = 0;
            int count = 0;
            int result = 0;

            try
            {
                if (startAddress == -1)
                    startAddress = (int)Process.GetProcessById(procId).MainModule.BaseAddress;
                if (endAddress == -1)
                    endAddress = startAddress + Process.GetProcessById(procId).MainModule.ModuleMemorySize;
            }
            catch
            {
                startAddress = 0x40000;
                endAddress = 0x88FFFF;
            }

            if (startAddress == 0 && endAddress == 0)
            {
                startAddress = 0x00000000;
                endAddress = 0x7FFFFFFF;
            }

            int[] byteSearch = ConverterEx.Hex2IntArr(hex);
            byte[] bufferSearch = new byte[BufferSize + byteSearch.Length];

            int loopCount = (endAddress - startAddress) / BufferSize;
            int outOfBounds = (endAddress - startAddress) % BufferSize;

            int BytesCount;
            IntPtr memId = OpenProcess(0x1F0FFF, false, procId);
            for (int i = 0; i < loopCount; i++)
            {
                ReadProcessMemory(memId, startAddress + i * BufferSize, bufferSearch, bufferSearch.Length, out BytesCount);
                for (int j = 0; j < BufferSize; j++)
                {
                    //if(j < byteSearch.Length)
                    //{
                    //    Main.PushLogEx(bufferSearch[j].ToString());
                    //}
                    if (Compare(bufferSearch, byteSearch, j))
                    {
                        address = address = j + i * BufferSize + startAddress;
                        if (count++ >= index)
                            return address;
                        result = address;
                    }
                }
                //if (byteSearch.Length > 1)
                //{
                //    byte[] bufferSearchMore = new byte[byteSearch.Length * 2];
                //    ReadProcessMemory(Id, startAddress + i * BufferSize - byteSearch.Length + 1, bufferSearchMore, bufferSearchMore.Length, 0);
                //    for (int j = 0; j < byteSearch.Length; j++)
                //    {
                //        if (Compare(bufferSearchMore, byteSearch, j))
                //        {
                //            address = address = j + i * BufferSize + startAddress;
                //            if (count++ >= index)
                //                return address;
                //            result = address;
                //        }
                //    }
                //}
            }
            if (outOfBounds > 0)
            {
                bufferSearch = new byte[outOfBounds + byteSearch.Length];
                ReadProcessMemory(memId, startAddress + loopCount * BufferSize, bufferSearch, bufferSearch.Length, 0);
                for (int j = 0; j < outOfBounds; j++)
                {
                    if (Compare(bufferSearch, byteSearch, j))
                    {
                        address = j + startAddress + loopCount * BufferSize;
                        if (count++ >= index)
                            return address;
                        result = address;
                    }
                }
            }
            return result;
        }


        public uint[] ToArr(uint address, uint[] offset)
        {
            uint[] newoffset = new uint[offset.Length + 1];
            newoffset[0] = address;
            offset.CopyTo(newoffset, 1);
            return newoffset;
        }

   
        public uint Read(uint address)
        {
            byte[] buffer = new byte[4];
            ReadProcessMemory(Id, address, buffer, 4, 0);
            return BitConverter.ToUInt32(buffer, 0);
        }

  

        public uint Read1Byte(uint address)
        {
            byte[] buff = new byte[4];
            ReadProcessMemory(Id, address, buff, 1, 0);
            return BitConverter.ToUInt32(buff, 0);
        }

        public uint Read1Byte(uint address, uint offset)
        {
            address = ReadAddress(address, offset);
            byte[] buff = new byte[4];
            ReadProcessMemory(Id, address, buff, 1, 0);
            return BitConverter.ToUInt32(buff, 0);
        }


        public uint Read2Byte(uint address)
        {
            byte[] buff = new byte[4];
            ReadProcessMemory(Id, address, buff, 2, 0);
            return BitConverter.ToUInt32(buff, 0);
        }


        public UInt64 Read8Byte(uint address)
        {
            byte[] buff = new byte[8];
            ReadProcessMemory(Id, address, buff, 8, 0);
            return BitConverter.ToUInt64(buff, 0);
        }

        public UInt64 Read8Byte(uint[] offsets)
        {
            byte[] buff = new byte[8];
            uint address = ReadAddress(offsets);
            ReadProcessMemory(Id, address, buff, 8, 0);
            return BitConverter.ToUInt64(buff, 0);
        }

   
        public uint Read(uint address, uint offset)
        {
            address = Read(address);
            address = Read(address + offset);
            return address;
        }

        public uint Read(uint address, uint[] offsets)
        {
            return Read(ToArr(address, offsets));
        }

   
        public uint Read(uint[] pointer, uint offset)
        {
            uint value = Read(pointer);
            return Read(value + offset);
        }

        

        public uint Read(uint[] offsets)
        {
            uint value = Read(offsets[0]);
            for (int i = 1; i < offsets.Length; i++)
                value = Read(value + offsets[i]);
            return value;
        }



   

        public bool IsRead(uint[] offsets)
        {
            byte[] buffer = new byte[4];   
            int cnt;
            ReadProcessMemory(Id, offsets[0], buffer, 4, out cnt);
            uint value = BitConverter.ToUInt32(buffer, 0);
            if (cnt == 0)
                return false;            
            for (int i = 1; i < offsets.Length - 1; i++)
            {
                cnt = 0;
                ReadProcessMemory(Id, value + offsets[i], buffer, 4, out cnt);
                if (cnt == 0)
                    return false;
                value = BitConverter.ToUInt32(buffer, 0);
            }
            if (offsets.Length > 1)
            {
                cnt = 0;
                ReadProcessMemory(Id, value + offsets[offsets.Length - 1], buffer, 4, out cnt);
                if (cnt == 0)
                    return false;
            }
            return true;
        }

        public float ReadFloat(uint address)
        {
            byte[] buffer = new byte[4];
            ReadProcessMemory(Id, address, buffer, 4, 0);
            return BitConverter.ToSingle(buffer, 0);            
        }

        public float ReadFloat(uint address, uint offset)
        {
            return ReadFloat(Read(address) + offset);
        }

        public float ReadFloat(uint[] offsets)
        {
            return ReadFloat(ReadAddress(offsets));
        }        

        public string ReadString(int address)
        {
            byte[] bufferStr = new byte[500];
            ReadProcessMemory(Id, address, bufferStr, bufferStr.Length, 0);              
            return VISCII2Unicode(bufferStr);
        }

        public string ReadString(uint address)
        {
            byte[] bufferStr = new byte[500];
            ReadProcessMemory(Id, address, bufferStr, 500, 0);
            return VISCII2Unicode(bufferStr);
        }

        public string ReadStringWithLength(int address, int length)
        {
            byte[] bufferStr = new byte[length];
            ReadProcessMemory(Id, address, bufferStr, length, 0);
            return VISCII2Unicode(bufferStr);
        }

        public string ReadShortString(uint address)
        {
            byte[] bufferStr = new byte[60];
            ReadProcessMemory(Id, address, bufferStr, 60, 0);

            return VISCII2Unicode(bufferStr);
        }

        public byte[] ReadVISCIIShortString(uint address)
        {
            byte[] bufferStr = new byte[60];
            ReadProcessMemory(Id, address, bufferStr, 60, 0);

            for (int i = 0; i < bufferStr.Length; i++)
            {
                if(bufferStr[i] == 0)
                {
                    Array.Resize(ref bufferStr, i);
                    break;
                }
            }


            return bufferStr;
        }

        public string ReadStringEx(int address)
        {
            byte[] bufferStr = new byte[20248];
            ReadProcessMemory(Id, address, bufferStr, bufferStr.Length, 0);
            return VISCII2Unicode(bufferStr);
        }

        public string ReadString(uint address, uint offset)
        {
            return ReadString(Read(address) + offset);
        }

        public string _ReadString(uint address, bool isShort = false)
        {
            if (Read(address + 0x14) == 15 || isShort)
                return ReadShortString(address);
            return ReadShortString(Read(address));
        }

        public string _ReadString(uint address, uint offset)
        {
            address = Read(address) + offset;
            return _ReadString(address);
        }

        public string _ReadString(uint[] offsets)
        {
            uint address = ReadAddress(offsets);
            return _ReadString(address);
        }

        public string ReadString(uint[] offsets)
        {
            return ReadString(ReadAddress(offsets));
        }

        public void Write(int address, int value)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            WriteProcessMemory(Id, address, buffer, 4, 0);
        }

    

        public void Write(uint address, int value)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            WriteProcessMemory(Id, address, buffer, 4, 0);
        }

        public void WriteUint(int address, uint value)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            WriteProcessMemory(Id, address, buffer, 4, 0);
        }

        public void Write(int address, uint value, int length)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            WriteProcessMemory(Id, address, buffer, length, 0);
        }

        public void Write(uint address, uint value, int length)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            WriteProcessMemory(Id, address, buffer, length, 0);
        }

        public void Write(uint[] offsets, int value)
        {
            Write(ReadAddress(offsets), value);
        }

        public uint ReadAddress(uint address, uint[] offset)
        {
            offset[0] = Read(address) + offset[0];
            return ReadAddress(offset);
        }

   

        public uint ReadAddress(uint address, uint offset)
        {
            return ReadAddress(new uint[] { address, offset });
        }


        public uint ReadAddress(uint[] offsets)
        {
            uint value = Read(offsets[0]);
            for (int i = 1; i < offsets.Length - 1; i++)
                value = Read(value + offsets[i]);
            if (offsets.Length == 1) return value;
            return value + offsets[offsets.Length - 1];
        }

 

        public static string VISCII2Unicode(int input)
        {
            if (input < 256)
                return TDT.Unicodes[input].ToString();
            else
                return ((char)input).ToString();
        }

        //public

        public static string VISCII2Unicode(byte[] input)
        {
            StringBuilder builder = new StringBuilder();
            foreach (char c in input)
            {
                if (c == '\0')
                {
                    break;
                }
                builder.Append(TDT.Unicodes[c]);
            }
            return builder.ToString();
        }

        public static string VISCII2UnicodeEx(byte[] input)
        {
            
            StringBuilder builder = new StringBuilder();
            foreach (char c in input)
            {
                if (c == '\0')
                {
                    continue;
                }
                builder.Append(TDT.Unicodes[c]);
            }
            return builder.ToString();
        }


      
        public static byte[] Unicode2VISCII(byte[] input)
		{
            for (int i = 0; i < input.Length; i++)
            {
                input[i] = Unicode2VISCII(input[i]);
            }
            return input;
		}

        public static byte Unicode2VISCII(int input)
        {
            for (int i = 0; i < 256; i++)
            {
                if (TDT.Unicodes[i] == input)
                    return (byte)i;
            }
            return (63);
        }

        public static int Float2Int(float value)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            return BitConverter.ToInt32(buffer, 0);
        }

        public uint VirtualAllocEx(int length)
        {
            return VirtualAllocEx(Id, 0, length + 16, 0x1000, 0x40);
        }

        public int WriteString(string str)
        {            
            byte[] buffer = Encoding.Default.GetBytes(str);
            int address = (int)VirtualAllocEx(Id, 0, buffer.Length + 16, 0x1000, 0x40);            
            WriteProcessMemory(Id, address, buffer, buffer.Length, 0);
            return address;
        }

        public int WriteString(string str, int address)
        {
            byte[] buffer = Encoding.Default.GetBytes(str);
            WriteProcessMemory(Id, address, buffer, buffer.Length, 0);
            return address;
        }

        public static int WriteString(string str, int address, int processId)
        {
            IntPtr ID = OpenProcess(0x1F0FFF, false, processId);
            byte[] buffer = Encoding.Default.GetBytes(str);
            WriteProcessMemory(ID, address, buffer, buffer.Length, 0);
            return address;
        }
        public static bool WriteAdress(byte[] buffer, uint address, int processId)
        {
            IntPtr ID = OpenProcess(0x1F0FFF, false, processId);
            uint old;
            bool success = VirtualProtectEx(ID, address, buffer.Length, Protection.PAGE_READWRITE, out old);
            WriteProcessMemory(ID, address, buffer, buffer.Length, 0);
            success = VirtualProtectEx(ID, address, buffer.Length, (Protection)old, out _);  
            return success;
        }

        public enum Protection
        {
            PAGE_NOACCESS = 0x01,
            PAGE_READONLY = 0x02,
            PAGE_READWRITE = 0x04,
            PAGE_WRITECOPY = 0x08,
            PAGE_EXECUTE = 0x10,
            PAGE_EXECUTE_READ = 0x20,
            PAGE_EXECUTE_READWRITE = 0x40,
            PAGE_EXECUTE_WRITECOPY = 0x80,
            PAGE_GUARD = 0x100,
            PAGE_NOCACHE = 0x200,
            PAGE_WRITECOMBINE = 0x400
        }

        [DllImport("kernel32.dll")]
        static extern bool VirtualProtectEx(IntPtr hProcess, uint lpAddress,
   int dwSize, Protection flNewProtect, out uint lpflOldProtect);

        public static string ReadStringId(int address, int processId)
        {
            IntPtr ID = OpenProcess(0x1F0FFF, false, processId);
            byte[] bufferStr = new byte[500];
            ReadProcessMemory(ID, address, bufferStr, 500, 0);
            return VISCII2Unicode(bufferStr);
        }

        public static int ReadAddressId(int address, int processId)
        {
            IntPtr ID = OpenProcess(0x1F0FFF, false, processId);
            byte[] buff = new byte[4];
            ReadProcessMemory(ID, address, buff, 4, 0);
            return BitConverter.ToInt32(buff, 0);
        }

        public uint WriteUnicodeString(string str, uint address)
        {
            str = ConverterEx.Unicode2VISCII(str);
            byte[] buffer = Encoding.Default.GetBytes(str);        
            Array.Resize<byte>(ref buffer, buffer.Length + 32);
            WriteProcessMemory(Id, address, buffer, buffer.Length, 0);
            return address;
        }
   

        public void TrimMem()
        {
            SetProcessWorkingSetSize(Process.GetProcessById(ProcessID).Handle, -1, -1);
        }

        public static int Decommit = 0x4000;
        public static int Release = 0x8000;
        public static int MEM_RESERVE = 0x00002000;
        public static int MEM_COMMIT = 0x00001000;
        public static int PAGE_READWRITE = 0x4;



        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(long dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(IntPtr hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, int lpNumberOfBytesRead);
        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(IntPtr hProcess, uint lpBaseAddress, byte[] lpBuffer, int dwSize, int lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(IntPtr hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, out int lpNumberOfBytesRead);
        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(IntPtr hProcess, uint lpBaseAddress, byte[] lpBuffer, int dwSize, out int lpNumberOfBytesRead);
        [DllImport("kernel32.dll")]
        public static extern bool WriteProcessMemory(IntPtr hProcess, int lpBaseAddress, byte[] lpBuffer, int nSize, int lpNumberOfBytesWritten);

        [DllImport("kernel32.dll")]
        public static extern bool WriteProcessMemory(IntPtr hProcess, uint lpBaseAddress, byte[] lpBuffer, int nSize, int lpNumberOfBytesWritten);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        public static extern uint VirtualAllocEx(IntPtr hProcess, int lpAddress, int dwSize, uint flAllocationType, uint flProtect);

        [DllImport("kernel32.dll")]
        public static extern bool VirtualFreeEx(IntPtr hProcess, int lpAddress, int dwSize, int dwFreeType);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool VirtualProtect(int lpAddress, uint dwSize, uint flNewProtect, int lpflOldProtect);

        [DllImport("kernel32.dll")]
        static extern bool SetProcessWorkingSetSize(IntPtr hProcess, int dwMinimumWorkingSetSize, int dwMaximumWorkingSetSize);

        [DllImport("kernel32.dll")]
        public static extern bool FlushInstructionCache(int hProcess, int lpBaseAddress,
           int dwSize);
    }
}