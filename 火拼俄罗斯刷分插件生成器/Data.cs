
namespace QQGameTool
{
    internal class Data
    {
        #region 初始值赋值
        static Data()
        {
            软件说明 = string.Concat(new string[]
           {
               "\r\n***************************************************************************",
               "\r\n*                        2022版火拼俄罗斯不落块插件                       *",
               "\r\n*            插件内核基于原作者[oneaddone]的2011年内部测试版开发          *",
               "\r\n*                                                                         *",
               "\r\n*  1.自动开始：无需每次按F2开始游戏，勾选后自动开始游戏                   *",
               "\r\n*  2.方块不落：勾选后方块不下落，相当于暂停功能                           *",
               "\r\n*  3.键盘加速：勾选后内置键盘加速功能,不勾选可使用其他键盘加速器          *",
               "\r\n*  4.半秒自杀：游戏开始后半秒自杀，刷负分专用功能,可自定义时间            *",
               "\r\n*  5.十秒自杀：对手上分专用功能,可自定义时间                              *",
               "\r\n*  6.速度测试：每次方块落地更新当前块速和当前开局用时总时长               *",
               "\r\n*  a.本软件可自定义[自杀时长,界面布局,Logo文本]]                          *",
               "\r\n*  b.键盘[F3键]显示/隐藏本插件窗口                                        *",
               "\r\n*  c.本软件可脱离游戏窗口直接修改插件内置配置参数                         *",
               "\r\n*  d.游戏掉线监控报警&自动关闭掉线窗口等功能作为独立软件另行发布!         *",
               "\r\n*                                                                         *",
               "\r\n*                                  BY:工控闪剑    Email 2590800@qq.com    *",
               "\r\n*                                                        2022年9月12日    *",
               "\r\n***************************************************************************",
               "\r\n",
               "\r\nDATE: 2022年11月01日",
               "\r\n1.整合所有已测试通过功能,去除授权相关代码",
               "\r\n2.请大家多多支持本人B站和CSDN博客,更多精彩内容欢迎您的点击评论",
               "\r\n3.本软件纯属个人爱好开发,如发现BUG请发送E-mail到本人邮箱(截图+文字说明)",
               "\r\n"
           });
            生成提示 = string.Concat(new string[]
            {
                "检测到游戏窗口未关闭!",
                "\r\n本软件将根据您打开的游戏窗口所在目录进行生成操作",
                "\r\n鉴于安全考虑本软件不会关闭你的游戏窗口!",
                "\r\n请关闭所有游戏窗口后重新点击生成按钮生成插件!"
            });
            键码表 = new string[]
           {"", "鼠标左键", "鼠标右键", "Cancel", "鼠标中键", "XButton1", "XButton2", "", "退格键", "Tab键",
				"", "", "Clear", "回车键", "", "", "Shift", "Ctrl", "Alt", "Pause",
				"CapsLock", "Kana", "Hangul", "Junja", "Final", "Hanja", "Kanji", "Esc键", "Convert", "Nonconvert",
				"Accept", "Modechange", "空格键", "PageUp", "PageDown", "End", "Home", "←", "↑", "→",
				"↓", "Select", "Print", "Execute", "Snapshot", "Insert", "DDelete", "Help", "0", "1",
				"2", "3", "4", "5", "6", "7", "8", "9", "", "",
				"", "", "", "", "", "A", "B", "C", "D", "E",
				"F", "G", "H", "I", "J", "K", "L", "M", "N", "O",
				"P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y",
				"Z", "LWin", "RWin", "Apps", "", "Sleep", "NUM 0", "NUM 1", "NUM 2", "NUM 3",
				"NUM 4", "NUM 5", "NUM 6", "NUM 7", "NUM 8", "NUM 9", "NUM *", "NUM +", "NUM Enter", "NUM -",
				"NUM .", "NUM /", "F1键", "F2键", "F3键", "F4键", "F5键", "F6键", "F7键", "F8键",
				"F9键", "F10键", "F11键", "F12键", "F13键", "F14键", "F15键", "F16键", "F17键", "F18键",
				"F19键", "F20键", "F21键", "F22键", "F23键", "F24键", "", "", "", "",
				"", "", "", "", "NumLock", "Scroll", "", "", "", "",
				"", "", "", "", "", "", "", "", "", "",
				"LShift", "RShift", "LCtrl", "RCtrl", "LAlt", "RAlt", "wwwBack", "wwwForward", "wwwRefresh", "wwwStop",
				"wwwSearch", "wwwFavorites", "wwwHome", "VolumeMute", "VolumeDown", "VolumeUp", "MediaNext", "MediaPrev", "MediaStop", "MediaPlay",
				"LaunchMal", "LaunchMediaSelect", "LaunchApp1", "LaunchApp2", "", "", ";:", "=+", ",<", "-_",
				".>", "/?", "`~", "", "", "", "", "", "", "",
				"", "", "", "", "", "", "", "", "", "",
				"", "", "", "", "", "", "", "", "", "[{",
				"＼｜", "]}", "‘“", "Oem 8", "", "", "Oem 102", "", "", "",
				"", "Packet", "", "", "", "", "", "", "", "ProcessKey",
				"", "", "", "", "", "", "Attn", "Crsel", "Exsel", "Ereof",
				"Play", "Zoom", "Noname", "Pa1", "OemClear" 
           };
            键盘速度 = new string[]
           {
               "无效值","300","500","700","900","1100","1300","1500","1700","1900",
               "2100","2200","2300","2400","2500","2600","2700","2800","2900","3000",
               "3100","3200","3300","3400","3500","3600","3700","3800","3900","4000",
               "4100","4200","4300","4400","4500","4600","4700","4800","4900","5000",
               "5100","5200","5300","5400","5500","5600","5700","5800","5900","6000",
               "6200","6400","6600","6800","7000","7200","7400","7600","7800","8000",
               "8200","8400","8600","8800","9000","9200","9400","9600","9800","10000",
               "超级快"
           };
            首次延时 = new int[]
            {
                0,100,100,100,100,100,100,100,100,100,
                100,100,100,100,100,100,100,100,100,100,
                100,98,95,93,90,89,87,84,82,81,
                79,78,76,75,73,72,70,70,69,68,
                66,65,64,64,63,62,61,60,59,60,
                58,56,56,55,53,53,52,51,51,50,
                49,49,48,48,48,47,46,47,46,46,
                46
            };
            reg显示速度 = "SpeedShow";
            reg键盘加速 = "Accelerator";
            reg超快响应 = "SuperFast";
            reg键盘速度 = "Speed";
            reg首次延时 = "Delay";
            reg游戏路径 = "GamePath";
        }
        #endregion
        //软件全局变量
        //地址变量-按键KeyCode
        internal const int jadr顺变 = 5504;
        internal const int jadr逆变 = 5514;
        internal const int jadr左移 = 5524;
        internal const int jadr右移 = 5534;
        internal const int jadr加落 = 5544;
        internal const int jadr直落 = 5554;
        internal const int jadr显示 = 11992;
        internal const int jadr统计 = 12069;
        //
        internal const int nadrn半秒自杀 = 12483;
        internal const int nadrn十秒自杀 = 12423;
        //地址变量-控件属性
        internal const int kadr底部logo = 175491;//按钮
        internal const int kadr键盘速度 = 175551;//滑动条
        internal const int kadr首次延时 = 175615;//滑动条
        internal const int kadr键速文本 = 175679;//文本标签
        internal const int kadr键盘加速 = 175711;//复选框
        internal const int kadr首延文本 = 175751;//文本标签
        internal const int kadr显示速度 = 175783;//复选框
        internal const int kadr自动开始 = 175823;//复选框
        internal const int kadr十秒自杀 = 175863;//复选框
        internal const int kadr超快响应 = 175903;//复选框
        internal const int kadr方块不落 = 175943;//复选框
        internal const int kadr数据统计 = 175983;//复选框
        internal const int kadr顶部logo = 176023;//按钮
        internal const int kadr半秒自杀 = 176083;//复选框
        //
        internal const int tadr顶部文本 = 176040;//暂不考虑修改
        internal const int tadr顶部文本1 = 158784;
        internal const int tadr顶部文本2 = 158720;
        internal const int tadr底部文本 = 175508;
        internal const int text最大长度 = 28;
        
        //状态变量-控件属性
        internal const byte StaBase = 64;
        internal const byte StaV1U1 = 80;
        internal const byte StaV1U0 = 88;
        internal const byte StaV0U1 = 64;
        internal const byte StaV0U0 = 72;
        //数组变量
        internal static string 软件说明;
        internal static string 生成提示;
        internal static string[] 键码表;
        internal static string[] 键盘速度;
        internal static int[] 首次延时;
        //注册表子键
        internal static string reg显示速度;
        internal static string reg键盘加速;
        internal static string reg超快响应;
        internal static string reg键盘速度;
        internal static string reg首次延时;
        internal static string reg游戏路径;

    }
}
