using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace TetrisMonitor
{
    class ProcApi
    {
        #region 导入函数部分

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(int hProcess, int lpBaseAddress, int lpBuffer, int nSize, int lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        private static extern void CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll")]
        public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, int[] lpBuffer, int nSize, IntPtr lpNumberOfBytesWritten);

        /// <summary>
        /// 查找所有窗口(只要是在进程里面的)
        /// 如果不限制类名或者标题使用null代替
        /// </summary>
        /// <param name="lpClassName">窗口类名,不限制使用null</param>
        /// <param name="lpWindowName">窗口标题,不限制使用null</param>
        /// <returns>找到的窗口句柄</returns>
        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        // 遍历窗口的所有子窗口，通过CallBack回调
        [DllImport("user32.dll")]
        public static extern int EnumChildWindows(IntPtr hWndParent, CallBack lpfn, int lParam);
        public delegate bool CallBack(IntPtr hwnd, int lParam);

        // 获取窗口的类名
        /// <summary>
        /// 获取目标句柄的类名
        /// </summary>
        /// <param name="hWnd">目标窗口句柄</param>
        /// <param name="lpClassName">返回的Class名称</param>
        /// <param name="nMaxCount">允许返回的Class名称的字符数量上限</param>
        /// <returns>获取到的实际Class字符长度</returns>
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        // 判断窗口是否可见
        /// <summary>
        /// 判断目标窗口是否可见
        /// </summary>
        /// <param name="hWnd">窗口句柄</param>
        /// <returns>可见true,不可见false</returns>
        [DllImport("user32.dll")]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        // 获取窗口文本长度
        /// <summary>
        /// 获取目标窗口标题的文本长度
        /// </summary>
        /// <param name="hWnd">目标窗口句柄</param>
        /// <returns>标题文本长度</returns>
        [DllImport("user32.dll")]
        public static extern int GetWindowTextLength(IntPtr hWnd);

        /// <summary>
        /// 获取窗口文本，文本会塞入StringBuilder中，需要指明字符串最大长度nMaxCount
        /// </summary>
        /// <param name="hwnd">窗口句柄</param>
        /// <param name="lpString">返回目标窗口的内容</param>
        /// <param name="nMaxCount">允许返回的字符数量上限</param>
        /// <returns>实际获取到的文本长度</returns>
        [DllImport("User32.dll", EntryPoint = "GetWindowText")]
        private static extern int GetWindowText(IntPtr hwnd, StringBuilder lpString, int nMaxCount);

        // 给窗口发送消息
        /// <summary>
        /// 给目标句柄发送消息
        /// </summary>
        /// <param name="hwnd">目标句柄</param>
        /// <param name="wMsg">消息内容</param>
        /// <param name="wParam">附加自定义参数1</param>
        /// <param name="lParam">附加自定义参数2</param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "SendMessageA")]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        /// <summary>
        /// 给窗口发送消息，事件返回的数据通过Byte[]数组获得
        /// </summary>
        /// <param name="hwnd">目标句柄</param>
        /// <param name="wMsg">消息内容</param>
        /// <param name="wParam">附加自定义参数1</param>
        /// <param name="lParam">附加自定义参数2</param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "SendMessageA")]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, Byte[] lParam);

        /// <summary>
        /// 获取当前活动窗口(当前焦点窗口,键盘可操作的那种)
        /// </summary>
        /// <returns>返回窗口句柄</returns>
        [DllImport("user32.dll")]
        public static extern IntPtr GetActiveWindow();

        /// <summary>
        /// 获取当前置顶窗口(窗口总在最前,类似于QQ那种)
        /// </summary>
        /// <returns>返回窗口句柄</returns>
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();
        /// <summary>
        /// 通过窗口句柄获取线程ID(TID)和进程ID(PID)
        /// </summary>
        /// <param name="hwnd">窗口句柄</param>
        /// <param name="PID">返回进程ID</param>
        /// <returns>返回线程ID</returns>
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowThreadProcessId(IntPtr hwnd, out int PID);   //获取线程ID

        /// <summary>
        /// 获取指定窗口（或控件）在屏幕中的位置信息 （左边界，上边界，右边界，下边界）
        /// </summary>
        /// <param name="hWnd">窗口句柄</param>
        /// <param name="lpRect">LPRECT矩形结构的长指针,数据存储使用struct类型</param>
        /// <returns>获取成功返回非0值,失败返回0</returns>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hWnd, ref RECT_INFO lpRect);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="short_path">原始短文件名</param>
        /// <param name="long_path">转换后返回的长文件名</param>
        /// <param name="long_len">返回文件名的最大长度(缓冲区长度)</param>
        /// <returns>返回的长文件名字符实际长度</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern int GetLongPathName(string short_path, StringBuilder long_path, int long_len);

        /// <summary>
        /// 设置程序工作内存
        /// </summary>
        /// <param name="process">进程ID</param>
        /// <param name="minSize">最小值</param>
        /// <param name="maxSize">最大值</param>
        /// <returns></returns>
        [DllImport("kernel32.dll", EntryPoint = "SetProcessWorkingSetSize")]
        public static extern int SetProcessWorkingSetSize(IntPtr process, int minSize, int maxSize);

        /// <summary>
        ///  GetWindowRect导入函数配属参数结构体
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT_INFO
        {
            /// <summary>
            /// 当前矩形范围的最左边界
            /// </summary>
            public int Left;
            /// <summary>
            /// 当前矩形的最上边界
            /// </summary>
            public int Top;
            /// <summary>
            /// 当前矩形的最右边界
            /// </summary>
            public int Right;
            /// <summary>
            /// 当前矩形的最下边界
            /// </summary>
            public int Bottom;
        }
        const int WM_GETTEXT = 0x000D;
        const int WM_GETTEXTLENGTH = 0x000E;
        const int WM_CLOSE = 0x10;
        const int SW_HIDE = 0;
        const int SW_NORMAL = 1;
        const int SW_MAXIMIZE = 3;
        const int SW_SHOWNOACTIVATE = 4;
        const int SW_SHOW = 5;
        const int SW_MINIMIZE = 6;
        const int SW_RESTORE = 9;
        const int SW_SHOWDEFAULT = 10;
        #endregion
        #region 声明变量类
        internal static int[] ProcID;
        internal static DateTime[] StartTime;
        internal static long[] QQid;
        #endregion
        #region 调用方法类
        /// <summary>
        /// 检测掉线弹窗/关闭掉线进程
        /// </summary>
        /// <returns>返回进程ID(PID)</returns>
        internal static int ccp检查关闭进程()
        {
            //条件值(API函数接口)
            string findTitle = "提示信息";//窗口标题
            string findClass1 = "#32770";//窗口类名
            string findClass2 = "GRootViewClass";//窗口类名
            int findClassMaxLen = 2048;//返回类名字符上限

            //返回值(API函数接口)
            IntPtr hWnd = IntPtr.Zero;//窗口句柄
            StringBuilder sbr = new StringBuilder(500);//返回类名字符串数据流
            string scr = string.Empty;//返回的类名字符串
            int ClassLen = 0;//实际返回类名字符长度
            int hwTID = 0;//线程ID
            int hwPID = 0;//进程ID
            int hwMSR = 0;//关闭窗口返回值

            //C#部分变量初始化
            string pchkTetris = "Tetris";//游戏窗口进程名
            string pchkQQGame1 = "QQGame";//新版游戏大厅进程名
            string pchkQQGame2 = "QQGameHall";//旧版游戏大厅进程名
            string pchkTitle = "火拼俄罗斯";
            string ProcTitleResult = string.Empty;//标题名
            Process ProcInfo = null;//实例化进程数据类
            string ProcNameResult = string.Empty;//进程名
            // 查找标题为Error的窗口
            hWnd = FindWindow(null, findTitle);//通过标题找句柄
            //判断窗口是否存在
            if (hWnd != IntPtr.Zero)
            {
                //找到了标题窗口的句柄
                ClassLen = GetClassName(hWnd, sbr, findClassMaxLen);//获取类名
                if (ClassLen > 0)
                {
                    //有类名
                    scr = sbr.ToString();//将返回的类名字符串数据流转换为字符串
                    if (scr == findClass1 | scr == findClass2)
                    {
                        //判断类名是否符合条件
                        if (IsWindowVisible(hWnd))
                        {
                            //窗口可见
                            hwTID = GetWindowThreadProcessId(hWnd, out hwPID);//获取线程ID和进程ID
                            ProcInfo = Process.GetProcessById(hwPID);//通过进程ID获取进程所有信息
                            ProcNameResult = ProcInfo.ProcessName;//获取进程名
                            ProcTitleResult = ProcInfo.MainWindowTitle;//主进程标题
                            if (ProcNameResult == pchkTetris | ProcNameResult == pchkQQGame1 | ProcNameResult == pchkQQGame2)
                            {
                                //是QQ游戏相关进程
                                if (ProcTitleResult.Contains(pchkTitle))
                                {
                                    //程序标题验证通过
                                    hwMSR = SendMessage(hWnd, WM_CLOSE, 0, 0);
                                    if (hwMSR == 0)
                                    {
                                        //窗口成功关闭
                                        if (ProcNameResult == pchkTetris)
                                        {
                                            //是Tetris关闭进程
                                            ProcInfo.CloseMainWindow();//结束进程
                                        }
                                        return hwPID;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return 0;
        }
        /// <summary>
        /// 检测游戏进程数量并获取必要数据
        /// </summary>
        /// <param name="pid">进程ID数组</param>
        /// <param name="StartTime">启动时间数组</param>
        /// <returns>返回进程数量</returns>
        internal static int Chk检测游戏进程()
        {
            Process[] ps = Process.GetProcessesByName("Tetris");//获取所有进程名为Tetris的进程,并将相关数据以数组方式存储
            if (ps.Length > 0)
            {
                ProcID = new int[ps.Length];
                StartTime = new DateTime[ps.Length];
                QQid = new long[ps.Length];
                for (int n = 0; n < ps.Length; n++)
                {
                    if (ps[n].MainWindowTitle.Contains("火拼俄罗斯方块"))
                    {
                        ProcID[n] = ps[n].Id;
                        StartTime[n] = ps[n].StartTime;
                        QQid[n] = GetQQ获取游戏账号(ProcID[n]);
                    }
                }
            }
            return ps.Length;
        }
        /// <summary>
        /// 获取游戏账号
        /// </summary>
        /// <param name="pid">进程ID</param>
        /// <returns>返回账号/QQ号(long)</returns>
        internal static long GetQQ获取游戏账号(int pid)
        {
            IntPtr MBA模块基址 = IntPtr.Zero;
            IntPtr TBA特征基址 = IntPtr.Zero;
            IntPtr QBA账号基址 = IntPtr.Zero;
            long QQID账号 = 0;
            Process ps = Process.GetProcessById(pid);
            for (int mo = 0; mo < ps.Modules.Count; mo++)
            {
                if (ps.Modules[mo].ModuleName == "CUQGEx.ocx")
                {
                    MBA模块基址 = ps.Modules[mo].BaseAddress;//模块基址
                    TBA特征基址 = MBA模块基址 + 167464;//HEX:28E28(HEX值:5151BAC5→GB2312:QQ号)
                    QBA账号基址 = TBA特征基址 - 52;//HEX:34
                    //轮询60次(6秒钟),解决do while循环体卡死的问题
                    for (int q = 0; q < 60; q++)
                    {
                        QQID账号 = ReadMemoryValue((int)QBA账号基址, pid);
                        if (QQID账号 > 10000) 
                        {
                            return QQID账号;
                        }
                        else
                        {
                            Thread.Sleep(100);
                        }
                    }
                }
            }
            return 0;
        }
        internal static void ClearMemory释放内存()
        {
            int MemoryMaxM = 100;//设置进程内存占用上限(MB)
            long MemoryMaxByte = 1024 * 1024 * (long)MemoryMaxM;
            Process Pproc = Process.GetCurrentProcess();
            long UserMemory = Pproc.PrivateMemorySize64;
            if (UserMemory > MemoryMaxByte)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
                }
            }
        }
        #endregion
        #region 内存操作方法类
        public static int GetPid(string windowTitle)
        {
            int result = 0;
            Process[] processes = Process.GetProcesses();
            foreach (Process process in processes)
            {
                if (process.MainWindowTitle.IndexOf(windowTitle) != -1)
                {
                    result = process.Id;
                    return result;
                }
            }
            return result;
        }

        public static int GetPidByProcessName(string processName)
        {
            Process[] processesByName = Process.GetProcessesByName(processName);
            Process[] array = processesByName;
            int num = 0;
            if (0 < array.Length)
            {
                Process process = array[num];
                return process.Id;
            }
            return 0;
        }

        public static IntPtr FindWindow(string title)
        {
            Process[] processes = Process.GetProcesses();
            foreach (Process process in processes)
            {
                if (process.MainWindowTitle.IndexOf(title) != -1)
                {
                    return process.MainWindowHandle;
                }
            }
            return IntPtr.Zero;
        }

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

        public static byte ReadMemoryByte(int baseAddress, int pid)
        {
            byte result;
            try
            {
                byte[] arr = new byte[1];
                int num = (int)Marshal.UnsafeAddrOfPinnedArrayElement(arr, 0);
                int hProcess = (int)OpenProcess(2035711, false, pid);
                ReadProcessMemory(hProcess, baseAddress, num, 1, 0);
                result = Marshal.ReadByte((IntPtr)num);
            }
            catch
            {
                result = 0;
            }
            return result;
        }

        public static int SearchMemoryValue(int baseAddress, int endAddress, int pid)
        {
            int result;
            try
            {
                byte[] array = new byte[4];
                int lpBuffer = (int)Marshal.UnsafeAddrOfPinnedArrayElement(array, 0);
                int hProcess = (int)OpenProcess(2035711, false, pid);
                bool flag = true;
                while (flag)
                {
                    ReadProcessMemory(hProcess, baseAddress, lpBuffer, 4, 0);
                    int num = BitConverter.ToInt32(array, 0);
                    if (num == 2191939)
                    {
                        baseAddress -= 256;
                        return baseAddress;
                    }
                    baseAddress += 8;
                    lpBuffer = (int)Marshal.UnsafeAddrOfPinnedArrayElement(array, 0);
                    if (baseAddress > endAddress)
                    {
                        return 0;
                    }
                }
                result = 0;
            }
            catch
            {
                result = 0;
            }
            return result;
        }

        public static int SearchMemoryValue2(int baseAddress, int endAddress, int pid)
        {
            int result;
            try
            {
                byte[] array = new byte[32768];
                int lpBuffer = (int)Marshal.UnsafeAddrOfPinnedArrayElement(array, 0);
                int hProcess = (int)OpenProcess(2035711, false, pid);
                for (int i = 0; i < 1472; i++)
                {
                    ReadProcessMemory(hProcess, baseAddress, lpBuffer, 32768, 0);
                    int j = 0;
                    while (j < 8192)
                    {
                        int num = j * 4;
                        int num2 = BitConverter.ToInt32(array, num);
                        int num3 = num2;
                        if (num3 <= -65805222)
                        {
                            if (num3 == -2129797706)
                            {
                                goto IL_FB;
                            }
                            if (num3 == -65805222)
                            {
                                int num4 = BitConverter.ToInt32(array, num + 4);
                                if (num4 == 201683700)
                                {
                                    baseAddress += j * 4 - 256;
                                    return baseAddress;
                                }
                            }
                        }
                        else
                        {
                            if (num3 == 2163842)
                            {
                                goto IL_FB;
                            }
                            if (num3 == 2191939)
                            {
                                goto IL_FB;
                            }
                        }
                        j++;
                        continue;
                    IL_FB:
                        baseAddress += num - 256;
                        return baseAddress;
                    }
                    baseAddress += 32768;
                    array = new byte[32768];
                    lpBuffer = (int)Marshal.UnsafeAddrOfPinnedArrayElement(array, 0);
                }
                result = 0;
            }
            catch
            {
                result = 0;
            }
            return result;
        }

        public static byte[] MemoryGame(int baseAddress, int pid)
        {
            byte[] result;
            try
            {
                byte[] array = new byte[252];
                int lpBuffer = (int)Marshal.UnsafeAddrOfPinnedArrayElement(array, 0);
                int hProcess = (int)OpenProcess(2035711, false, pid);
                ReadProcessMemory(hProcess, baseAddress, lpBuffer, 252, 0);
                result = array;
            }
            catch
            {
                byte[] array2 = new byte[1];
                result = array2;
            }
            return result;
        }

        public static void WriteMemoryValue(int baseAddress, string processName, int value)
        {
            IntPtr intPtr = OpenProcess(2035711, false, GetPidByProcessName(processName));
            WriteProcessMemory(intPtr, (IntPtr)baseAddress, new int[] { value }, 4, IntPtr.Zero);
            CloseHandle(intPtr);
        }
        #endregion
    }
}
