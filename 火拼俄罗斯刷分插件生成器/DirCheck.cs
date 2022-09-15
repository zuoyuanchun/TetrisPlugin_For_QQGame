using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
namespace QQGameTool
{
    internal static class DirCheck
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="short_path">原始短文件名</param>
        /// <param name="long_path">转换后返回的长文件名</param>
        /// <param name="long_len">返回文件名的最大长度(缓冲区长度)</param>
        /// <returns>返回的长文件名字符实际长度</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern int GetLongPathName(string short_path, StringBuilder long_path, int long_len);

        internal static string Fp插件;
        internal static string Fp0游戏目录;
        internal static string Fp1主程序;
        internal static string Fp2库文件;
        internal static string Fp3按键配置;
        internal static string Fp4TetrisConfig;
        internal static string Fp5TetrisRes;
        internal static int ProcNum = 0;
        internal static DateTime LastStartTime = DateTime.MinValue;
        internal static string ExePath = string.Empty;
        internal static string ExeDir = string.Empty;
        internal static string ExeTitle = string.Empty;


        internal static bool Set选择目录检测(string basedir)
        {
            string tmp1 = Path.Combine(basedir, "TetrisConfig.dll");
            string tmp2 = Path.Combine(basedir, "TetrisRes.dll");
            string tmp3 = Path.Combine(basedir, "Tetris.exe");
            string tmp4 = Path.Combine(basedir, "tetris.ini");

            if (File.Exists(tmp1) && File.Exists(tmp2) && File.Exists(tmp3) && File.Exists(tmp4))
            {
                Fp插件 = Path.Combine(basedir, "TetrisPlugin.dll");
                Fp0游戏目录 = basedir;
                Fp1主程序 = Path.Combine(basedir, "Tetris.exe");
                Fp3按键配置 = Path.Combine(basedir, "tetris.ini");
                Fp2库文件 = Path.Combine(basedir, "lolita", "LoliCore32.dll");
                Fp4TetrisConfig = Path.Combine(basedir, "TetrisConfig.dll");
                Fp5TetrisRes = Path.Combine(basedir, "TetrisRes.dll");
                WriteRegGamePath(basedir);
                return true;
            }
            return false;
        }
        internal static bool Check检查同目录()
        {
            if (File.Exists(Fp1主程序) && File.Exists(Fp2库文件) && File.Exists(Fp3按键配置) && File.Exists(Fp4TetrisConfig) && File.Exists(Fp5TetrisRes)) 
            {
                return true;//优先自选目录检测
            }
            string basedir = System.IO.Directory.GetCurrentDirectory();
            if (Chk检测游戏进程())
            {
                basedir = ExeDir;
            }
            if(Set选择目录检测(basedir))
            {
                return true;
            }
            return false;
        }
        internal static void WriteRegGamePath(string gamepath)
        {
            RegU reg = new RegU();
            reg.SetValue(Data.reg游戏路径, gamepath);
            reg = null;
        }
        internal static string ReadRegInfo()
        {
            //判断当前操作系统版本,获取注册表主键位置
            string RegPluginPath;//不落块插件配置数据注册表主键位置
            string SysDir = Environment.GetFolderPath(Environment.SpecialFolder.SystemX86);
            if (SysDir.IndexOf("SysWOW64") > 0)
            {
                RegPluginPath = "WOW6432Node\\ZYCTool\\TetrisPlugin";//X64
            }
            else
            {
                RegPluginPath = "ZYCTool\\TetrisPlugin";//X86
            }
            return RegPluginPath;
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
                ExePath = ps[LastNum].MainModule.FileName;//获取最后一个进程主模块的完整程序路径（绝对路径）
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
                catch (Exception ex)
                {
                    return ex.Message;
                }
                string path = procpath.Substring(0, procpath.Length - 10);
                return path;
            }
            return string.Empty;
        }
    }
}
