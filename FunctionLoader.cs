using System;
using System.Runtime.InteropServices;

class FunctionLoader
{
    [DllImport("Kernel32.dll")]
    private static extern IntPtr LoadLibrary(string path);

    [DllImport("Kernel32.dll")]
    private static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

    public static Delegate LoadFunction<T>(string dllPath, string functionName)
    {
        var hModule = LoadLibrary(dllPath);
        var functionAddress = GetProcAddress(hModule, functionName);
        return Marshal.GetDelegateForFunctionPointer(functionAddress, typeof(T));
    }
}