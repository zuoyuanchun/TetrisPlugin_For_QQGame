using System;

namespace QQGameTool
{
   internal static class Conv
    {
       private static int ConvBaseValue = 440000;//基础参考值
       /// <summary>
       /// 通过键盘速度的Value属性值换算后续换算参数的参考值
       /// </summary>
       /// <param name="OptionValue">滑动条参数值</param>
       /// <param name="IntValue">换算后的整数值</param>
       /// <param name="StringValue">换算后的文本值</param>
       public static void SpeedDataConvert(int OptionValue,out int IntValue,out string StringValue)
       {
           IntValue = -1;
           StringValue = null;
           int opera = OptionValue + 32;
           if (opera <= 32 && opera > 102)
           { StringValue = "参数错误"; }
           if (opera == 102) 
           { 
               StringValue = "超级快";
               IntValue = 100;
           }
           if (opera >= 33 && opera <= 42) 
           {
               IntValue = opera * 2 - 63;
               StringValue = Convert.ToString(IntValue * 100);
           }
           if (opera > 42 && opera < 82)
           {
               IntValue = opera - 21;
               StringValue = Convert.ToString(IntValue * 100);
           }
           if (opera >= 82 && opera < 102)
           {
               IntValue = opera * 2 - 102;
               StringValue = Convert.ToString(IntValue * 100);
           }
       }
       /// <summary>
       /// 首次延时的结果值,返回字符串
       /// </summary>
       /// <param name="SpeedValue">键盘加速的参数值</param>
       /// <param name="DelayValue">首次延时的参数值</param>
       /// <returns>返回首次延时结果字符串</returns>
       public static string DelayOperaToView(int SpeedValue,int DelayValue)
       {
           int SpdInt = 0;
           string SpdStr = null;
           SpeedDataConvert(SpeedValue,out SpdInt,out SpdStr);
           
           int DelayMax = 0;
           int DelayViewValue = 0;
           //int DelayView = 0;
           DelayMax = ConvBaseValue / SpdInt;//最小首次延时值(Delay为100时)
           DelayViewValue = DelayMax / DelayValue;
           return Convert.ToString(DelayViewValue);
           //DelayView = ConvBaseValue / SpeedValue / DelayValue;//实时自动调整值

       }
       /// <summary>
       /// 计算键盘加速的缺省首次延时参数
       /// </summary>
       /// <param name="SpeedValue">键盘加速参数值滑动条Value值</param>
       /// <returns>返回缺省的首次延时参数值int</returns>
       public static int DelayNowValue(int SpeedValue)
       {
           /*
          int SpdInt = 0;
          string SpdStr = null;
          SpeedDataConvert(SpeedValue, out SpdInt, out SpdStr);
          int DelayMax = 0;
          int DelayViewValue = 0;
          DelayMax = ConvBaseValue / SpdInt / 100;//最小首次延时值(Delay为100时)
          DelayViewValue = DelayMax * 100 / DelayValue;
          */
           return Data.首次延时[SpeedValue];
       }
    }
}
