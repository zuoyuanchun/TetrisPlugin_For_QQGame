using System;
using System.IO;
using System.Windows.Forms;

namespace QQGameTool
{
    internal static class FileGen
    {
        internal static byte[] RData;
        //
        static FileGen()
        {
            RData = global::QQGameTool.Properties.Resources.TData;//读取内置资源文件
        }
        internal static bool Gen生成插件()
        {
            //DirCheck.Check检查同目录();
            if(DirCheck.Chk检测游戏进程())
            {
                DirCheck.Set选择目录检测(DirCheck.ExeDir);
                MessageBox.Show(Data.生成提示,"警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            //
            if (!File.Exists(DirCheck.Fp1主程序) | !File.Exists(DirCheck.Fp2库文件) | !File.Exists(DirCheck.Fp4TetrisConfig) | !File.Exists(DirCheck.Fp5TetrisRes)) 
            {
                MessageBox.Show("未检测到游戏目录,无法安装插件!", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            Diy.Diy写入数据();//生成数据
            Wrt写入文件(DirCheck.Fp插件, RData);

             //关键游戏程序数据版本效验
            byte[] Daty原始主程序 = Conv文件转数组(DirCheck.Fp1主程序);
            byte[] Daty原始库文件 = Conv文件转数组(DirCheck.Fp2库文件);

            byte[] Datz资源主程序 = global::QQGameTool.Properties.Resources.Tetris;
            byte[] Datz资源库文件 = global::QQGameTool.Properties.Resources.LoliCore32;
            //
            if (!Chk效验文件(Daty原始主程序, Datz资源主程序))
            {
                //MessageBox.Show("检测到游戏版本不兼容,正在写入兼容数据!\r\nTetris.exe");
                Wrt写入文件(DirCheck.Fp1主程序, Datz资源主程序);
            }
            else
            {
               // MessageBox.Show("效验正确!\r\nTetris.exe");
            }

            if(!Chk效验文件(Daty原始库文件,Datz资源库文件))
            {
                //MessageBox.Show("检测到游戏版本不兼容,正在写入兼容数据!\r\nLoliCore32.dll");
                Wrt写入文件(DirCheck.Fp2库文件, Datz资源库文件);
            }
            else
            {
               // MessageBox.Show("效验正确!\r\nLoliCore32.dll");
            }
            return true;
        }
        internal static byte[] Conv文件转数组(string Old原始文件路径)
        {

            FileStream ofs = new FileStream(Old原始文件路径, FileMode.Open, FileAccess.Read);
            int Lenth = Convert.ToInt32(ofs.Length);
            byte[] oldbytes = new byte[Lenth];
            BinaryReader obr = new BinaryReader(ofs);
            obr.Read(oldbytes, 0, Lenth);
            obr.Close();
            ofs.Close();
            obr = null;
            ofs = null;
            return oldbytes;
        }
        internal static bool Chk效验文件(byte[] Old原始文件, byte[] New资源文件)
        {
            int leno = Old原始文件.Length;
            int lenn = New资源文件.Length;
            if (leno == lenn)
            {
                for (int a = 0; a < leno; a++)
                {
                    if (Old原始文件[a] == New资源文件[a])
                    {
                        continue;
                    }
                    return false;
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        internal static void Wrt写入文件(string File文件路径,byte[] Resb写入数据)
       {
           try
           {
              // MessageBox.Show(File文件路径);
               BinaryWriter wr = null;
               wr = new BinaryWriter(File.Open(File文件路径, FileMode.Create));
                wr.Write(Resb写入数据, 0,Resb写入数据.Length);
               wr.Flush();
               wr.Close();
               wr = null;
           }
           catch (Exception exr)
           {
               MessageBox.Show(exr.Message, "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
           }
       }
    }
}
