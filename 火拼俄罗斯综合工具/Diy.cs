using System;
using System.Text;

namespace QQGameTool
{
    internal static class Diy
    {
       
        static Diy()
        {
            for (int a = 0; a < 15; a++) 
            {
                BTV[a] = 16;
                BTU[a] = 0;
            }
        }
        internal static void Diy转换数据()
        {
            DelayHalf = BitConverter.GetBytes(RDelayHalf);
            DelayTen = BitConverter.GetBytes(RDelayTen);
            //
            TopLogo1 = Encoding.Default.GetBytes(RTitleLogo1);
            TopLogo2 = Encoding.Default.GetBytes(RTitleLogo2);
            BomLogo = Encoding.Unicode.GetBytes(RUserLogo);
            //
            if (License.LicSta > 0)
            {
                Bt1自动开始 = Data.StaV1U1;
                Bt2方块不落 = Data.StaV1U0;//V0无效
                Bt3半秒自杀 = Data.StaV1U0;//V0无效
                Bt4十秒自杀 = Data.StaV1U0;//V0无效
                Bt5显示速度 = Data.StaV1U1;
                Bt6数据统计 = Data.StaV1U0;
                Bt7键盘加速 = Data.StaV1U0;
                Bt8超快响应 = Data.StaV1U0;//V0无效
                Bt9键盘速度 = Data.StaV1U0;
                Bt10首次延时 = Data.StaV1U0;
            }
            if (License.LicSta > 10)
            {
                Bt1自动开始 = Data.StaV1U1;
                Bt2方块不落 = Data.StaV1U1;//V0无效
                Bt3半秒自杀 = Data.StaV1U0;//V0无效
                Bt4十秒自杀 = Data.StaV1U1;//V0无效
                Bt5显示速度 = Data.StaV1U1;
                Bt6数据统计 = Data.StaV1U0;
                Bt7键盘加速 = Data.StaV1U1;
                Bt8超快响应 = Data.StaV1U1;//V0无效
                Bt9键盘速度 = Data.StaV1U1;
                Bt10首次延时 = Data.StaV1U1;
            }
            if (License.LicSta > 15)
            {
                Bt1自动开始 = Data.StaV1U1;
                Bt2方块不落 = Data.StaV1U1;//V0无效
                Bt3半秒自杀 = Data.StaV1U1;//V0无效
                Bt4十秒自杀 = Data.StaV1U1;//V0无效
                Bt5显示速度 = Data.StaV1U1;
                Bt6数据统计 = Data.StaV1U0;
                Bt7键盘加速 = Data.StaV1U1;
                Bt8超快响应 = Data.StaV1U1;//V0无效
                Bt9键盘速度 = Data.StaV1U1;
                Bt10首次延时 = Data.StaV1U1;
            }
            if (License.LicSta > 20)
            {
                Bt1自动开始 = (byte)(Data.StaBase + BTV[1] + BTU[1]);
                Bt2方块不落 = (byte)(Data.StaBase + BTV[2] + BTU[2]);
                Bt3半秒自杀 = (byte)(Data.StaBase + BTV[3] + BTU[3]);
                Bt4十秒自杀 = (byte)(Data.StaBase + BTV[4] + BTU[4]);
                Bt5显示速度 = (byte)(Data.StaBase + BTV[5] + BTU[5]);
                Bt6数据统计 = (byte)(Data.StaBase + BTV[6] + BTU[6]);
                Bt7键盘加速 = (byte)(Data.StaBase + BTV[7] + BTU[7]);
                Bt8超快响应 = (byte)(Data.StaBase + BTV[8] + BTU[8]);
                Bt9键盘速度 = (byte)(Data.StaBase + 16 + BTU[9]);
                Bt10首次延时 = (byte)(Data.StaBase + 16 + BTU[10]);
            }
        }
        internal static void Diy写入数据()
        {
            Diy转换数据();
            int Liclevel = License.LicSta;

            FileGen.RData[Data.nadrn半秒自杀] = DelayHalf[0];
            FileGen.RData[Data.nadrn半秒自杀 + 1] = DelayHalf[1];
            //
            FileGen.RData[Data.nadrn十秒自杀] = DelayTen[0];
            FileGen.RData[Data.nadrn十秒自杀 + 1] = DelayTen[1];

            int a = 0;
            int addr = 0;
            string str = null;
            byte[] btstr = new byte[14];
            //
            for (a = 1; a < 3; a++)
            {
                switch (a)
                {
                    case 1:
                        addr = Data.tadr顶部文本1;
                        btstr = TopLogo1;
                        break;
                    case 2:
                        addr = Data.tadr顶部文本2;
                        btstr = TopLogo2;
                        break;
                    default:
                        break;
                }
                int 字节长度 = btstr.Length;   //字符长度
                int 当前写入位置 = 0;
                do
                {
                    if (当前写入位置 >= 字节长度) //填充前面
                    {
                        FileGen.RData[addr] = 0;

                    }
                    else
                    {
                        FileGen.RData[addr] = btstr[当前写入位置];
                    }
                    addr++;
                    当前写入位置++;
                } while (当前写入位置 < Data.text最大长度);
            }

            //LOGO文本写入
            for (a = 1; a < 2; a++)
            {
                switch (a)
                {
                    case 1:
                        addr = Data.tadr底部文本;
                        str = RUserLogo;
                        btstr = BomLogo;
                        break;
                    default:
                        break;
                }
                int 字符长度 = str.Length * 2;   //字符长度
                int 填充长度 = Data.text最大长度 - 字符长度;      //填充数量
                int 低位填充长度 = 填充长度 / 2;          //前面填充数
                int 高位补充位置 = 字符长度 + 低位填充长度;          //后面填编号
                int 当前写入位置 = 0;
                int 读取位置 = 0;
                do
                {
                    if (当前写入位置 < 低位填充长度 | 当前写入位置 >= 高位补充位置) //填充前面
                    {
                        FileGen.RData[addr] = 32;
                        当前写入位置++;
                        addr++;
                        FileGen.RData[addr] = 0;

                    }
                    else
                    {
                        FileGen.RData[addr] = btstr[读取位置];
                        当前写入位置++;
                        读取位置++;
                        addr++;
                        FileGen.RData[addr] = btstr[读取位置];
                        读取位置++;
                    }
                    addr++;
                    当前写入位置++;
                } while (当前写入位置 < Data.text最大长度);
            }


            FileGen.RData[Data.kadr自动开始] = Bt1自动开始;
            FileGen.RData[Data.kadr方块不落] = Bt2方块不落;
            FileGen.RData[Data.kadr半秒自杀] = Bt3半秒自杀;
            FileGen.RData[Data.kadr十秒自杀] = Bt4十秒自杀;
            FileGen.RData[Data.kadr显示速度] = Bt5显示速度;
            FileGen.RData[Data.kadr数据统计] = Bt6数据统计;
            FileGen.RData[Data.kadr键盘加速] = Bt7键盘加速;
            FileGen.RData[Data.kadr超快响应] = Bt8超快响应;
            FileGen.RData[Data.kadr键盘速度] = Bt9键盘速度;
            FileGen.RData[Data.kadr首次延时] = Bt10首次延时;

        }
        internal static void Diy获取属性(int 编号, bool 属性)
        {
            if (编号 <= 10 && 属性)
            {
                BTV[编号] = 16;
            }
            if (编号 <= 10 && !属性)
            {
                BTV[编号] = 0;
            }
            if (编号 > 10 && !属性)
            {
                BTU[编号 - 10] = 8;
            }
            if (编号 > 10 && 属性)
            {
                BTU[编号 - 10] = 0;
            }
            if (编号 == 2 && 属性) 
             {
                 FileGen.RData[14370] = 1;
             }
             if (编号 == 2 && !属性)
             {
                 FileGen.RData[14370] = 0;
             }
             if (编号 == 4 && 属性)
             {
                 FileGen.RData[14383] = 1;
             }
             if (编号 == 4 && !属性)
             {
                 FileGen.RData[14383] = 0;
             }
             if (编号 == 3 && 属性)
             {
                 FileGen.RData[14396] = 1;
             }
             if (编号 == 3 && !属性)
             {
                 FileGen.RData[14396] = 0;
             }

        }
        internal static byte[] HotKeyData = new byte[10];     //操作按键数据
        internal static byte[] DelayTen = new byte[4];        //十秒延时数据
        internal static byte[] DelayHalf = new byte[4];       //半秒延时数据
        internal static byte[] TopLogo1 = new byte[30];       //顶部LOGO1
        internal static byte[] TopLogo2 = new byte[30];       //顶部LOGO2
        internal static byte[] BomLogo = new byte[30];        //底部LOGO
        //
        internal static byte Bt1自动开始 = Data.StaV1U0;
        internal static byte Bt2方块不落 = Data.StaV1U0;//V0无效
        internal static byte Bt3半秒自杀 = Data.StaV1U0;//V0无效
        internal static byte Bt4十秒自杀 = Data.StaV1U0;//V0无效
        internal static byte Bt5显示速度 = Data.StaV1U0;
        internal static byte Bt6数据统计 = Data.StaV1U0;
        internal static byte Bt7键盘加速 = Data.StaV1U0;
        internal static byte Bt8超快响应 = Data.StaV1U0;//V0无效
        internal static byte Bt9键盘速度 = Data.StaV1U0;
        internal static byte Bt10首次延时 = Data.StaV1U0;
        //
        internal static byte[] BTV = new byte[20];
        internal static byte[] BTU = new byte[20];
        //
        internal static int RDelayTen = 0;
        internal static int RDelayHalf = 0;
        internal static string RTitleLogo1 = null;
        internal static string RTitleLogo2 = null;
        internal static string RUserLogo = null;
        //
    }
}
