using System;
using System.IO;

namespace QQGameTool
{
    internal static class DirCheck
    {
        internal static string Fp插件;
        internal static string Fp0游戏目录;
        internal static string Fp1主程序;
        internal static string Fp2库文件;
        internal static string Fp3按键配置;
        internal static string Fp4TetrisConfig;
        internal static string Fp5TetrisRes;
      


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
            if (ProcCheck.Chk检测游戏进程())
            {
                basedir = ProcCheck.ExeDir;
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

    }
}
