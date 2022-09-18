using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace QQGameTool
{
    internal class Memory
    {

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(int hProcess, int lpBaseAddress, int lpBuffer, int nSize, int lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        private static extern void CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll")]
        public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, int[] lpBuffer, int nSize, IntPtr lpNumberOfBytesWritten);




        public static long ReadMemoryValue(int baseAddress, int pid)
        {
            long result;
            try
            {
                byte[] arr = new byte[4];
                int num = (int)Marshal.UnsafeAddrOfPinnedArrayElement(arr, 0);
                int hProcess = (int)OpenProcess(2035711, false, pid);
                ReadProcessMemory(hProcess, baseAddress, num, 4, 0);
                result = Marshal.ReadInt64((IntPtr)num);
            }
            catch
            {
                result = 0L;
            }
            return result;
        }
    }
}
