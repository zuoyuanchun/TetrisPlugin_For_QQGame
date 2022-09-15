using System;
using System.Text;

namespace QQGameTool
{
    internal static class Diy
    {

        internal static void Diy转换数据()
        {
            DelayHalf = BitConverter.GetBytes(RDelayHalf);
            DelayTen = BitConverter.GetBytes(RDelayTen);
            //
            TopLogo1 = Encoding.Default.GetBytes(RTitleLogo1);
            TopLogo2 = Encoding.Default.GetBytes(RTitleLogo2);
            BomLogo = Encoding.Unicode.GetBytes(RUserLogo);
            //
           
        }
        internal static void Diy写入数据()
        {
            Diy转换数据();

            FileGen.RData[Data.nadrn半秒自杀] = DelayHalf[0];
            FileGen.RData[Data.nadrn半秒自杀 + 1] = DelayHalf[1];
            //
            FileGen.RData[Data.nadrn十秒自杀] = DelayTen[0];
            FileGen.RData[Data.nadrn十秒自杀 + 1] = DelayTen[1];

            int a = 0;
            int 基址 = 0;
            string 字符串文本 = null;
            byte[] 字节文本 = new byte[14];
            //
            for (a = 1; a < 3; a++)
            {
                switch (a)
                {
                    case 1:
                        基址 = Data.tadr顶部文本1;
                        字节文本 = TopLogo1;
                        break;
                    case 2:
                        基址 = Data.tadr顶部文本2;
                        字节文本 = TopLogo2;
                        break;
                    default:
                        break;
                }
                int 字节长度 = 字节文本.Length;   //字符长度
                int 当前写入位置 = 0;
                do
                {
                    if (当前写入位置 >= 字节长度) //填充前面
                    {
                        FileGen.RData[基址] = 0;

                    }
                    else
                    {
                        FileGen.RData[基址] = 字节文本[当前写入位置];
                    }
                    基址++;
                    当前写入位置++;
                } while (当前写入位置 < Data.text最大长度);
            }

            //LOGO文本写入
            for (a = 1; a < 2; a++)
            {
                switch (a)
                {
                    case 1:
                        基址 = Data.tadr底部文本;
                        字符串文本 = RUserLogo;
                        字节文本 = BomLogo;
                        break;
                    default:
                        break;
                }
                int 字符长度 = 字符串文本.Length * 2;   //字符长度
                int 填充长度 = Data.text最大长度 - 字符长度;      //填充数量
                int 低位填充长度 = 填充长度 / 2;          //前面填充数
                int 高位补充位置 = 字符长度 + 低位填充长度;          //后面填编号
                int 当前写入位置 = 0;
                int 读取位置 = 0;
                do
                {
                    if (当前写入位置 < 低位填充长度 | 当前写入位置 >= 高位补充位置) //填充前面
                    {
                        FileGen.RData[基址] = 32;
                        当前写入位置++;
                        基址++;
                        FileGen.RData[基址] = 0;

                    }
                    else
                    {
                        FileGen.RData[基址] = 字节文本[读取位置];
                        当前写入位置++;
                        读取位置++;
                        基址++;
                        FileGen.RData[基址] = 字节文本[读取位置];
                        读取位置++;
                    }
                    基址++;
                    当前写入位置++;
                } while (当前写入位置 < Data.text最大长度);
            }
            //游戏操作按键数据
            FileGen.RData[Data.jadr顺变] = OperaKey[1];
            FileGen.RData[Data.jadr逆变] = OperaKey[2];
            FileGen.RData[Data.jadr左移] = OperaKey[3];
            FileGen.RData[Data.jadr右移] = OperaKey[4];
            FileGen.RData[Data.jadr加落] = OperaKey[5];
            FileGen.RData[Data.jadr直落] = OperaKey[6];
            FileGen.RData[Data.jadr显示] = OperaKey[7];//隐藏显示热键
            FileGen.RData[Data.jadr统计] = 127;//屏蔽数据统计键
            //自定义UI数据
            FileGen.RData[Data.kadr自动开始 + 1] = Bt1x自动开始;
            FileGen.RData[Data.kadr自动开始 + 3] = Bt1y自动开始;
            FileGen.RData[Data.kadr方块不落 + 1] = Bt2x方块不落;
            FileGen.RData[Data.kadr方块不落 + 3] = Bt2y方块不落;
            FileGen.RData[Data.kadr半秒自杀 + 1] = Bt3x半秒自杀;
            FileGen.RData[Data.kadr半秒自杀 + 3] = Bt3y半秒自杀;
            FileGen.RData[Data.kadr十秒自杀 + 1] = Bt4x十秒自杀;
            FileGen.RData[Data.kadr十秒自杀 + 3] = Bt4y十秒自杀;
            FileGen.RData[Data.kadr显示速度 + 1] = Bt5x显示速度;
            FileGen.RData[Data.kadr显示速度 + 3] = Bt5y显示速度;
            FileGen.RData[Data.kadr数据统计 + 1] = Bt6x数据统计;
            FileGen.RData[Data.kadr数据统计 + 3] = Bt6y数据统计;
            FileGen.RData[Data.kadr键盘加速 + 1] = Bt7x键盘加速;
            FileGen.RData[Data.kadr键盘加速 + 3] = Bt7y键盘加速;
            FileGen.RData[Data.kadr超快响应 + 1] = Bt8x超快响应;
            FileGen.RData[Data.kadr超快响应 + 3] = Bt8y超快响应;

            FileGen.RData[Data.kadr键盘速度 + 1] = Bt9x键盘速度;
            FileGen.RData[Data.kadr键盘速度 + 3] = Bt9y键盘速度;
            FileGen.RData[Data.kadr首次延时 + 1] = Bt10x首次延时;
            FileGen.RData[Data.kadr首次延时 + 3] = Bt10y首次延时;
            FileGen.RData[Data.kadr键速文本 + 1] = Bt11x键速文本;
            FileGen.RData[Data.kadr键速文本 + 3] = Bt11y键速文本;
            FileGen.RData[Data.kadr首延文本 + 1] = Bt12x首延文本;
            FileGen.RData[Data.kadr首延文本 + 3] = Bt12y首延文本;
            FileGen.RData[Data.kadr顶部logo + 1] = Bt13x顶部logo;
            FileGen.RData[Data.kadr顶部logo + 3] = Bt13y顶部logo;
            FileGen.RData[Data.kadr底部logo + 1] = Bt14x底部logo;
            FileGen.RData[Data.kadr底部logo + 3] = Bt14y底部logo;

        }
        //核心数据初始化
        internal static byte[] HotKeyData = new byte[10];     //操作按键数据
        internal static byte[] DelayTen = new byte[4];        //十秒延时数据
        internal static byte[] DelayHalf = new byte[4];       //半秒延时数据
        internal static byte[] TopLogo1 = new byte[30];       //顶部LOGO1
        internal static byte[] TopLogo2 = new byte[30];       //顶部LOGO2
        internal static byte[] BomLogo = new byte[30];        //底部LOGO
        internal static byte[] OperaKey = new byte[10];       //操作按键
        //控件位置初始化
        internal static byte Bt1x自动开始 = 0;
        internal static byte Bt1y自动开始 = 0;
        internal static byte Bt2x方块不落 = 0;
        internal static byte Bt2y方块不落 = 0;
        internal static byte Bt3x半秒自杀 = 0;
        internal static byte Bt3y半秒自杀 = 0;
        internal static byte Bt4x十秒自杀 = 0;
        internal static byte Bt4y十秒自杀 = 0;
        internal static byte Bt5x显示速度 = 0;
        internal static byte Bt5y显示速度 = 0;
        internal static byte Bt6x数据统计 = 0;
        internal static byte Bt6y数据统计 = 0;
        internal static byte Bt7x键盘加速 = 0;
        internal static byte Bt7y键盘加速 = 0;
        internal static byte Bt8x超快响应 = 0;
        internal static byte Bt8y超快响应 = 0;
        internal static byte Bt9x键盘速度 = 0;
        internal static byte Bt9y键盘速度 = 0;
        internal static byte Bt10x首次延时 = 0;
        internal static byte Bt10y首次延时 = 0;
        internal static byte Bt11x键速文本 = 0;
        internal static byte Bt11y键速文本 = 0;
        internal static byte Bt12x首延文本 = 0;
        internal static byte Bt12y首延文本 = 0;
        internal static byte Bt13x顶部logo = 0;
        internal static byte Bt13y顶部logo = 0;
        internal static byte Bt14x底部logo = 0;
        internal static byte Bt14y底部logo = 0;

        //
        internal static int RDelayTen = 0;
        internal static int RDelayHalf = 0;
        internal static string RTitleLogo1 = null;
        internal static string RTitleLogo2 = null;
        internal static string RUserLogo = null;
        //
    }
}
