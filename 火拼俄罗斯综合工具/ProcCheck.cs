using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Collections.Generic;

namespace QQGameTool
{

    internal class ProcCheck
    {
        #region 导入函数部分
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

        #endregion
        internal static void CheckCloseProcess(bool KillPorcess,out int ProcessResult)
        {
            ProcessResult = 0;
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
                                ProcessResult = 5;
                                if (ProcTitleResult.Contains(pchkTitle))
                                {
                                    //程序标题验证通过
                                    ProcessResult = 6;
                                    if (KillPorcess)
                                    {
                                        ProcessResult = 7;
                                        hwMSR = SendMessage(hWnd, WM_CLOSE, 0, 0);
                                        if (hwMSR == 0)
                                        {
                                            //窗口成功关闭
                                            ProcessResult = 8;
                                            if (ProcNameResult == pchkTetris)
                                            {
                                                //是Tetris关闭进程
                                                ProcessResult = 9;
                                                ProcInfo.CloseMainWindow();//结束进程
                                                return;
                                            }
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        ProcessResult = 1;
                                        return;

                                    }
                                }
                            }
                        }
                    }
                }
            }
            ProcessResult = 0;
        }

        internal static bool Chk检测游戏进程()
        {
            Process[] ps = Process.GetProcessesByName("Tetris");//获取所有进程名为Tetris的进程,并将相关数据以数组方式存储
            if (ps.Length > 0)
            {
                ProcNum = ps.Length;//获取进程名的进程数量
                int LastNum = ps.Length - 1;
                int LastProcID = ps[LastNum].Id;//获取最有一个进程的ID
                LastStartTime = ps[LastNum].StartTime;//获取最后一个进程的启动时间
                ExePath = ps[LastNum].MainModule.FileName   ;//获取最后一个进程主模块的完整程序路径（绝对路径）
                ExeTitle = ps[LastNum].MainWindowTitle;//主模块标题
                ExeDir = GetDir获取游戏目录(ExePath);
                return true;
            }
            else
            {
                ProcNum = 0;
                return false;
            }
        }
        internal static string GetDir获取游戏目录(string procpath)
        {
            if (procpath.Length > 0)
            {
                try
                {
                    if (procpath.Contains("~"))
                    {
                        StringBuilder sbrlong = new StringBuilder(256);
                        GetLongPathName(procpath, sbrlong, 256);
                        procpath = sbrlong.ToString();
                    }
                }
                catch(Exception ex)
                {
                    return ex.Message;
                }
                string path = procpath.Substring(0, procpath.Length - 10);
                return path;
            }
            return string.Empty;
        }
        internal static bool Get获取游戏QQ号()
        {
            Process[] process = Process.GetProcessesByName("Tetris");//获取所有进程名为Tetris的进程,并将相关数据以数组方式存储
            if (process.Length > 0)
            {
                QQhaoma.Clear();
                string chkTitle = "火拼俄罗斯";//完整名"火拼俄罗斯方块"
                for (int n = 0; n < process.Length; n++)
                {
                    if (process[n].MainWindowTitle.Contains(chkTitle)) 
                    {
                        bool qqok = false;
                        Process ps = process[n];
                        IntPtr MBA模块基址 = IntPtr.Zero;
                        IntPtr TBA特征基址 = IntPtr.Zero;
                        IntPtr QBA账号基址 = IntPtr.Zero;
                        long QQID账号 = 0;
                        for (int mo = 0; mo < ps.Modules.Count; mo++)
                        {
                            if (ps.Modules[mo].ModuleName == "CUQGEx.ocx")
                            {
                                MBA模块基址 = ps.Modules[mo].BaseAddress;
                                TBA特征基址 = MBA模块基址 + 167464;//HEX:28E28
                                QBA账号基址 = TBA特征基址 - 52;//HEX:34
                                for (int q = 0; q < 50; q++)
                                {
                                    QQID账号 = Memory.ReadMemoryValue((int)QBA账号基址, (int)ps.Id);
                                    if (QQID账号 > 10000)
                                    {
                                        if (!qqok)
                                        {
                                            QQhaoma.Add(QQID账号);
                                            qqok = true;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        Thread.Sleep(100);
                                    }
                                }
                            }
                            if (qqok)
                            {
                                break;
                            }
                        }
                        /*
                        bool Tz1 = true;//查找特征
                        bool Tz2 = true;//查找区域
                        int BaseAddr = 16813608;//内存基址
                        long QQtezheng = 0;
                        long QQnumber = 0;
                        while (Tz1)
                        {
                            QQtezheng = Memory.ReadMemoryValue(BaseAddr, (int)process[n].Id);
                            if (QQtezheng == 3317322065L)
                            {
                                Tz1 = false;
                                BaseAddr -= 52;
                                while (Tz2)
                                {
                                    QQnumber = Memory.ReadMemoryValue(BaseAddr, (int)process[n].Id);
                                    if (QQnumber == 0L)
                                    {
                                        Thread.Sleep(100);
                                    }
                                    else
                                    {
                                        Tz2 = false;
                                        QQhaoma.Add(QQnumber);
                                    }
                                }
                            }
                            else
                            {
                                BaseAddr += 65536;
                            }
                        }
                         * */
                    }
                    
                }
                return true;
            }
            return false;
        }
        internal static void ClearMemory释放内存()
        {
            int MemoryMaxM = 100;//设置进程内存占用上限(MB)
            long MemoryMaxByte = 1024 * 1024 * (long)MemoryMaxM;
            Process Pproc = Process.GetCurrentProcess();
            long UserMemory = Pproc.PrivateMemorySize64;
            if(UserMemory>MemoryMaxByte)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                if(Environment.OSVersion.Platform==PlatformID.Win32NT)
                {
                    SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
                }
            }
        }
   

        internal static int ProcNum = 0;
        internal static DateTime LastStartTime = DateTime.MinValue;
        internal static List<long> QQhaoma = new List<long>();
        internal static string ExePath = string.Empty;
        internal static string ExeDir = string.Empty;
        internal static string ExeTitle = string.Empty;
    }
}
