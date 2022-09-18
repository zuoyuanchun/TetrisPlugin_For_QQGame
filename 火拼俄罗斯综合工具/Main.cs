using System;
using System.IO;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Generic;

namespace QQGameTool
{
    public partial class Main : Form
    {
        
        public Main()
        {
            InitializeComponent();
        }
        #region 窗口加载事件和全局变量实例化
        private AutoResetEvent ExitEvent = new AutoResetEvent(false);
        private Thread Tthread;
        private byte[] tag自定义按键 = new byte[10];
        private DateTime[] sj记录时间 = new DateTime[20];
        private TimeSpan[] sj统计时间 = new TimeSpan[20];
        private string[] sj文本时间 = new string[20];
        private int dtnum = 1;//报警时间数组当前记录编号
        

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.Text = "火拼俄罗斯方块刷分辅助工具&内置键盘加速器 BY:工控闪剑";
            cmtext.Text = Data.软件说明;
            MusicInit();//初始化音乐播放器
            CheckKey();
            ReadKey();
            读取路径信息();
            检查授权状态();
            读取按键配置();
            Time报警数据初始化();
           // Tthread = new Thread(new ThreadStart(Tthread子线程));
            //Tthread.Start();
            //注意顺序,颠倒可能报错
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
           // ExitEvent.Set();
            //Tthread.Join();
        }
        #endregion
        private void Tthread子线程()
        {
            bool chk重新检测QQ号码 = false;
            bool proclock无进程锁定 = false;
            string QQhaoma1 = string.Empty;//第一行QQ号码
            string QQhaoma2 = string.Empty;//第二行QQ号码
            string qqview = string.Empty;
            List<long> QQlist = new List<long>();//所有游戏进程QQ号码列表
            while (true)
            {
                bool Chk掉线Checked = CK0掉线检测功能.Checked;
                bool Chk掉线Enabled = CK0掉线检测功能.Enabled;
                if(Chk掉线Checked)
                {
                    proclock无进程锁定 = false;
                }
                //■■■■■■■■■掉线检测功能代码
                if (dtnum >= 16)
                {
                    Time报警数据初始化();
                }
                if (dtnum % 4 == 0)
                {
                    dtnum++;//确认复位后
                }
                if (ProcCheck.Chk检测游戏进程())
                {
                    Chk掉线Enabled = true;
                    if (QQlist.Count != ProcCheck.ProcNum)
                    {
                        chk重新检测QQ号码 = true;
                    }
                }
                else 
                {
                    if (!proclock无进程锁定 && Chk掉线Checked) 
                    {
                        MessageBox.Show("未发现游戏进程,无法使用本功能", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    Chk掉线Checked = false;
                    Chk掉线Enabled = false;
                    QQlist.Clear();
                    QQhaoma1 = string.Empty;
                    QQhaoma2 = string.Empty;
                    proclock无进程锁定 = true;
                    goto 输出信息;
                }
                if (Chk掉线Checked)
                {
                    int Result = 0;

                    if (!ProcCheck.Chk检测游戏进程() | ProcCheck.ProcNum == 0) 
                    {
                        goto 输出信息;
                    }
                    DateTime LastStart = ProcCheck.LastStartTime;
                    if (dtnum % 4 == 1)
                    {
                        sj记录时间[dtnum] = LastStart;
                        sj文本时间[dtnum] = string.Format("{0:T}", sj记录时间[dtnum]);
                    }
                    else
                    {
                        goto 显示账号;
                    }
                    ProcCheck.CheckCloseProcess(false, out Result);
                    //有窗口掉线
                    if (Result > 0)
                    {
                        MusicPlayer.settings.setMode("loop", CK0报警音乐循环.Checked);
                        if (dtnum % 4 == 1)
                        {
                            dtnum++;
                            sj记录时间[dtnum] = DateTime.Now;
                            sj文本时间[dtnum] = string.Format("{0:T}", sj记录时间[dtnum]);
                        }
                        MusicPlayer.Ctlcontrols.play();
                        Chk掉线Checked = false;
                        Chk掉线Enabled = false;
                        if (CK0关闭掉线窗口.Checked)
                        {
                            do
                            {
                                ProcCheck.CheckCloseProcess(true, out Result);
                                Thread.Sleep(100);
                            } while (Result > 0);
                            chk重新检测QQ号码 = true;
                        }

                    }
                }
            //■■■■■■■■■获取QQ号码
            显示账号:
                if (chk重新检测QQ号码) 
                {
                    if (!ProcCheck.Chk检测游戏进程())
                    {
                        goto 输出信息;
                    }
                    if (QQlist.Count != ProcCheck.ProcNum)
                    {
                        Thread.Sleep(3100);//预加载窗口延迟扫描账号信息
                    }
                    if (ProcCheck.Get获取游戏QQ号())
                    {
                        QQhaoma1 = string.Empty;
                        QQhaoma2 = string.Empty;
                        QQlist = ProcCheck.QQhaoma;
                        QQlist.Sort((x, y) => x.CompareTo(y));//排序
                        for (int n = 0; n < QQlist.Count; n++)
                        {
                            if (QQhaoma1.Length < 73)
                            {
                                QQhaoma1 += string.Concat("<", QQlist[n].ToString(), ">");//组合字符串
                            }
                            else
                            {
                                QQhaoma2 += string.Concat("<", QQlist[n].ToString(), ">");//组合字符串
                            }
                        }
                        if (ProcCheck.ProcNum == QQlist.Count)
                        {
                            chk重新检测QQ号码 = false;
                        }
                        Thread.Sleep(100);
                    }
                    //qqstate = "未检测到正在游戏中的窗口";
                }
            输出信息:

                if (QQhaoma1.Length == 0)
                {
                    qqview = string.Concat("★未检测到游戏窗口!部分功能不可用!★\r\n当前时间", "[", DateTime.Now.ToLongTimeString(), "]");
                }
                if (QQhaoma1.Length > 0 && QQhaoma2.Length > 0)
                {
                    qqview = string.Concat(QQhaoma1, "\r\n", QQhaoma2, "更新", "[", DateTime.Now.ToLongTimeString(), "]");//双排显示
                }
                if (QQhaoma1.Length > 0 && QQhaoma2.Length == 0)
                {
                    qqview = string.Concat(QQhaoma1, "\r\n当前数据更新时间", "[", DateTime.Now.ToLongTimeString(), "]");//单排显示
                }

                if (dtnum % 4 == 2)
                {
                    dtnum++;
                    sj统计时间[dtnum] = sj记录时间[dtnum - 1] - sj记录时间[dtnum - 2];
                    sj文本时间[dtnum] = sj统计时间[dtnum].ToString(@"hh\:mm\:ss");
                }

                //更新窗口控件数据(需要添加队列更新,否则可能造成主窗口假死■■■■■■■■■■
                this.Invoke(new Action(() =>
                    {
                        lb0数据信息.Text = string.Concat("窗口数量[", ProcCheck.ProcNum.ToString(), "]■[", QQlist.Count.ToString(), "]账号数量");
                        dtt1.Text = sj文本时间[1];
                        dtt2.Text = sj文本时间[2];
                        dtt3.Text = sj文本时间[3];
                        dtt4.Text = sj文本时间[4];
                        dtt5.Text = sj文本时间[5];
                        dtt6.Text = sj文本时间[6];
                        dtt7.Text = sj文本时间[7];
                        dtt8.Text = sj文本时间[8];
                        dtt9.Text = sj文本时间[9];
                        dtt10.Text = sj文本时间[10];
                        dtt11.Text = sj文本时间[11];
                        dtt12.Text = sj文本时间[12];
                        dtt13.Text = sj文本时间[13];
                        dtt14.Text = sj文本时间[14];
                        dtt15.Text = sj文本时间[15];
                        dtt16.Text = sj文本时间[16];

                        CK0掉线检测功能.Enabled = Chk掉线Enabled;
                        CK0掉线检测功能.Checked = Chk掉线Checked;
                        qqlist附加功能列表.Text = qqview;
                        qqlist生成界面列表.Text = qqview;
                        base.TopMost = chk0top窗口置顶.Checked;
                        if (chk0内存占用优化.Checked)
                        {
                            ProcCheck.ClearMemory释放内存();
                            chk0内存占用优化.Checked = false;
                        }
                    }));
                if (ExitEvent.WaitOne(500))
                {
                    break;
                }
            }
        }
    
        #region 公用方法模块
        private void Time报警数据初始化()
        {
            dtnum = 1;
            for (int n = 0; n < 20; n++) 
            {
                sj记录时间[n] = DateTime.MinValue;
                sj统计时间[n] = TimeSpan.MinValue;
                sj文本时间[n] = string.Empty;
            }
        }
        private void MusicInit()
        {
            MusicPlayer.settings.autoStart = false;
            string SndPath = Path.Combine(Directory.GetCurrentDirectory(), "music.mp3");
            if (File.Exists(SndPath))
            {
                MusicPlayer.URL = SndPath;
                CK0掉线检测功能.Enabled = true;
            }
            else
            {
                MessageBox.Show("未检测到报警音频文件,请将MP3音乐文件放置到本软件同目录,并改名为\r\nmusic.mp3\r\n否则无法使用掉线报警功能", "友情提示");
                CK0掉线检测功能.Enabled = false;
            }

        }
        private void 读取路径信息()
        {
            bool checkok = false;
            RegU reg = new RegU();
            string gamedir = reg.getValue(Data.reg游戏路径);
            reg = null;
            if (gamedir.Contains("Tetris") && DirCheck.Set选择目录检测(gamedir))
            {
                checkok = true;
            }
            else
            {
                MessageBox.Show("原始配置参数目录非游戏目录!\r\n" + gamedir);
            }
            if (DirCheck.Check检查同目录())
            {
                checkok = true;
            }
            if (ProcCheck.Chk检测游戏进程())
            {
                DirCheck.Set选择目录检测(ProcCheck.ExeDir);
                checkok = true;
            }
            if (checkok)
            {
                tb0俄罗斯路径.Text = DirCheck.Fp0游戏目录;
            }
            else
            {
                tb0俄罗斯路径.Text = "未获取到游戏路径!请进入游戏后点击[更新]按钮读取信息";
            }
        }
        #region 注册表操作
        private void CheckKey()
        {
            RegU reg = new RegU();
            if (!reg.IsRegistryValueNameExist(Data.reg显示速度))
            {
                reg.SetValue(Data.reg显示速度, "1");
            }
            if (!reg.IsRegistryValueNameExist(Data.reg键盘加速))
            {
                reg.SetValue(Data.reg键盘加速, "1");
            }
            if (!reg.IsRegistryValueNameExist(Data.reg超快响应))
            {
                reg.SetValue(Data.reg超快响应, "0");
            }
            if (!reg.IsRegistryValueNameExist(Data.reg键盘速度))
            {
                reg.SetValue(Data.reg键盘速度, "39");
            }
            if (!reg.IsRegistryValueNameExist(Data.reg首次延时))
            {
                reg.SetValue(Data.reg首次延时, "98");
            }
            if (!reg.IsRegistryValueNameExist(Data.reg授权码))
            {
                reg.SetValue(Data.reg授权码, "1177161017");
            }
            if (!reg.IsRegistryValueNameExist(Data.reg游戏路径))
            {
                reg.SetValue(Data.reg游戏路径, "0");
            }
            reg = null;
        }
        private void ReadKey()
        {
            RegU reg = new RegU();
            string rk显示速度 = reg.getValue(Data.reg显示速度);
            string rk键盘加速 = reg.getValue(Data.reg键盘加速);
            string rk超快响应 = reg.getValue(Data.reg超快响应);
            //
            string rk键盘速度 = reg.getValue(Data.reg键盘速度);
            string rk首次延时 = reg.getValue(Data.reg首次延时);
            string rk授权码 = reg.getValue(Data.reg授权码);

            //
            if (rk显示速度 == "1")
            {
                this.SpeedShow.Checked = true;
            }
            if (rk显示速度 == "0")
            {
                this.SpeedShow.Checked = false;
            }
            if (rk键盘加速 == "1")
            {
                this.Accelerator.Checked = true;
            }
            if (rk键盘加速 == "0")
            {
                this.Accelerator.Checked = false;
            }
            if (rk超快响应 == "1")
            {
                this.SuperFast.Checked = true;
            }
            if (rk超快响应 == "0")
            {
                this.SuperFast.Checked = false;
            }
            reg = null;
            Key验证授权();

            //
            speed参数值.Text = rk键盘速度;
            speed滑动条.Value = Convert.ToUInt16(rk键盘速度);
            speed实际值.Text = Data.键盘速度[Convert.ToUInt16(rk键盘速度)];
            //
            delay参数值.Text = rk首次延时;
            delay滑动条.Value = Convert.ToUInt16(rk首次延时);
        }
        private void WriteKey()
        {
            RegU reg = new RegU();
            //
            if (SpeedShow.Checked == true)
            {
                reg.SetValue(Data.reg显示速度, "1");
            }
            else
            {
                reg.SetValue(Data.reg显示速度, "0");
            }

            //
            if (Accelerator.Checked == true)
            {
                reg.SetValue(Data.reg键盘加速, "1");
            }
            else
            {
                reg.SetValue(Data.reg键盘加速, "0");
            }

            //
            if (SuperFast.Checked == true)
            {
                reg.SetValue(Data.reg超快响应, "1");
            }
            else
            {
                reg.SetValue(Data.reg超快响应, "0");
            }
            reg.SetValue(Data.reg键盘速度, speed参数值.Text);
            reg.SetValue(Data.reg首次延时, delay参数值.Text);
            reg = null;
        }
        #endregion
        private void 读取按键配置()
        {
            if (File.Exists(DirCheck.Fp3按键配置))
            {
                FileStream ofs = new FileStream(DirCheck.Fp3按键配置, FileMode.Open, FileAccess.Read);
                int Lenth = Convert.ToInt32(ofs.Length);
                byte[] inibytes = new byte[Lenth];
                BinaryReader obr = new BinaryReader(ofs);
                obr.Read(inibytes, 0, Lenth);
                tag自定义按键[1] = inibytes[80];
                tag自定义按键[2] = inibytes[84];
                tag自定义按键[3] = inibytes[88];
                tag自定义按键[4] = inibytes[92];
                tag自定义按键[5] = inibytes[96];
                tag自定义按键[6] = inibytes[100];
                tag自定义按键[7] = 114;
                tag自定义按键[8] = 115;
                obr.Close();
                ofs.Close();
                obr = null;
                ofs = null;
            }
            else
            {
                string msg1 = "未找到游戏的配置文件!插件生成功能将禁用!\r\n";
                string msg2 = "1.进入游戏后设置按键,并退出游戏窗口!\r\n";
                string msg3 = "2.重新进入游戏,最小化游戏窗口,然后重新打开本软件\r\n";
                string msgt = "未找到配置文件";
                MessageBox.Show(msg1 + msg2 + msg3, msgt, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                tag自定义按键[1] = 38;
                tag自定义按键[2] = 96;
                tag自定义按键[3] = 37;
                tag自定义按键[4] = 39;
                tag自定义按键[5] = 40;
                tag自定义按键[6] = 32;
                tag自定义按键[7] = 114;
                tag自定义按键[8] = 115;
                TimeHalValue.Text = "258";
                TimeTenValue.Text = "11111";
            }
            转换参数();
        }
        private void 转换参数()
        {
            keycw.Text = Data.键码表[(int)tag自定义按键[1]];//顺时针按键
            keyccw.Text = Data.键码表[(int)tag自定义按键[2]];//逆时针按键
            keyleft.Text = Data.键码表[(int)tag自定义按键[3]];//左移按键
            keyright.Text = Data.键码表[(int)tag自定义按键[4]];//右移按键
            keydw.Text = Data.键码表[(int)tag自定义按键[5]];//加速下落按键
            keybot.Text = Data.键码表[(int)tag自定义按键[6]];//直接下落按键
            keyshow.Text = Data.键码表[(int)tag自定义按键[7]];//显示隐藏插件窗口
            if (p1aj8.Checked == false)
            {
                keystats.Text = Data.键码表[(int)tag自定义按键[8]];//显示隐藏数据统计
            }
        }
        private void 检查授权状态()
        {
            //License.LicSta = 0;//开发时强制授权
            if (License.LicCheck() > 0)
            {
                group11操作按键.Visible = true;
                group12附加参数.Visible = true;
                shengcheng.Visible = true;
                Group31Reg参数设置.Visible = true;
                Group31Reg参数设置.Enabled = true;
            }
            if (License.LicCheck() > 10)
            {
                
                Panel自杀延时.Visible = true;
                shengcheng.Enabled = true;
            }
            if (License.LicCheck()>15)
            {
                group11操作按键.Enabled = true;
                group12附加参数.Enabled = true;
                Group21效果预览.Visible = true;
                Group22显示效果.Visible = true;
                Group31Reg参数设置.Visible = true;
                Panel上下LOGO.Visible = true;
                panel功能有效性.Visible = true;
                Panel位置参数.Visible = true;
            }
            if (License.LicCheck() > 20)
            {
                group11操作按键.Enabled = true;
                group12附加参数.Enabled = true;
                Group21效果预览.Enabled = true;
                Group22显示效果.Enabled = true;
                Panel自杀延时.Enabled = true;
                Panel上下LOGO.Enabled = true;
                
                
            }
            if (License.LicCheck() > 30) 
            {
                panel功能有效性.Enabled = true;
                Panel位置参数.Enabled = true;
            }
        }
        private void Key验证授权()
        {
            RegU reg = new RegU();
            string KeyStr = reg.getValue(Data.reg授权码);
            if (KeyStr == "2950800")
            {
                License.LicSta = 17;
                Key输入框.Text = "LV.3";
                Key输入框.Enabled = false;
            }
            if (KeyStr == "zuoyuanchun")
            {
                License.LicSta = 27;
                Key输入框.Text = "LV.4";
                Key输入框.Enabled = false;
            }
            if (KeyStr == "5127" | KeyStr == "1177161017")
            {
                License.LicSta = 37;
                Key输入框.Text = "LV.5";
                Key输入框.Enabled = false;
            }
            检查授权状态();
        }
        #endregion
        #region 第一页:附加功能-控件事件

        private void 调试按钮_Click(object sender, EventArgs e)
        {
            int Result = 0;
            ProcCheck.CheckCloseProcess(CK0关闭掉线窗口.Checked, out Result);
            //ProcCheck.Chk检测游戏进程();
        }
        private void ReadDir读取进程路径_Click(object sender, EventArgs e)
        {
            if (ProcCheck.Chk检测游戏进程())
            {
                string GameDir = ProcCheck.ExeDir;
                DirCheck.Set选择目录检测(GameDir);
                tb0俄罗斯路径.Text = GameDir;
                读取按键配置();
            }
            else
            {
                MessageBox.Show("请先正常进入游戏(无需准备)\r\n然后再次点击此按钮", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
       
        private void CoDir指定文件夹_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog FolDir = new FolderBrowserDialog();
            FolDir.Description = "请选择俄罗斯方块所在的文件夹";
            FolDir.ShowNewFolderButton = false;
            FolDir.RootFolder = Environment.SpecialFolder.MyComputer;
            if (FolDir.ShowDialog() == DialogResult.OK)
            {
                string basedir = FolDir.SelectedPath;
                if (DirCheck.Set选择目录检测(basedir))
                {
                    tb0俄罗斯路径.Text = basedir;
                }
                else
                {
                    MessageBox.Show("选择的目录非游戏文件夹!请重新选择", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void bt0报警复位_Click(object sender, EventArgs e)
        {
            if (dtnum % 4 == 3) 
            {
                dtnum++;
                sj记录时间[dtnum] = DateTime.Now;
                sj文本时间[dtnum] = string.Format("{0:T}", sj记录时间[dtnum]);
            }
            MusicPlayer.Ctlcontrols.stop();//停止播放音乐
            bt0报警复位.Text = "报警复位";
            CK0掉线检测功能.Enabled = true;
            CK0掉线检测功能.Checked = false;
        }
        #endregion
        #region 第二页:操作按键-控件事件

        private void 键盘_KeyDown(object sender, KeyEventArgs e)
        {
            byte tnum = Convert.ToByte(((TextBox)sender).Tag);//获取调用此方法的控件Tag编号
            this.tag自定义按键[(int)tnum] = (byte)e.KeyValue;//获取吊用此方法控的件键盘码编号
            ((TextBox)sender).Text = Data.键码表[(int)this.tag自定义按键[(int)tnum]];//返回键盘码编号对应的键码表文本内容
            // this.shengcheng.Focus();//切换焦点防止二次输入
        }
        private void 键盘_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;//标记已经执行过键盘按键事件
        }
        private void 自杀键盘_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = e.KeyChar < '0' || e.KeyChar > '9';
            if(e.Handled)
            {
                MessageBox.Show("只允许输入数字!", "警告");
                ((TextBox)sender).Text = null;
            }

        }
 
        private void sc生成按钮_Click(object sender, EventArgs e)
        {
            //操作按键数据处理
            int maxtag = 7;//需定义按键数量
            for (int a = 1; a < maxtag + 1; a++)
            {
                if (tag自定义按键[a] == 0)
                {
                    MessageBox.Show("有未设置的按键!\r\n请检查按键设置!", "写入失败", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                for (int b = 1; b < maxtag + 1; b++)
                {
                    if (tag自定义按键[a] == tag自定义按键[b] && a != b)
                    {
                        MessageBox.Show("设置的按键有重复!", "写入失败", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }
            }
            FileGen.RData[Data.jadr顺变] = tag自定义按键[1];
            FileGen.RData[Data.jadr逆变] = tag自定义按键[2];
            FileGen.RData[Data.jadr左移] = tag自定义按键[3];
            FileGen.RData[Data.jadr右移] = tag自定义按键[4];
            FileGen.RData[Data.jadr加落] = tag自定义按键[5];
            FileGen.RData[Data.jadr直落] = tag自定义按键[6];
            FileGen.RData[Data.jadr显示] = tag自定义按键[7];//隐藏显示热键
            if (p1aj8.Checked == true)
            {
                FileGen.RData[Data.jadr统计] = 127;//屏蔽数据统计键
            }
            else
            {
                FileGen.RData[Data.jadr统计] = tag自定义按键[8];
            }
            //默认延时数据设置
            Diy.RDelayHalf = Convert.ToInt16(TimeHalValue.Text);
            if (Diy.RDelayHalf % 16 == 0) 
            {
                Diy.RDelayHalf++;
            }
            Diy.RDelayTen = Convert.ToInt16(TimeTenValue.Text);
            if (Diy.RDelayTen % 16 == 0)
            {
                Diy.RDelayTen++;
            }
            //默认标题数据设置
            Diy.RTitleLogo1 = LogoTopText1.Text;
            Diy.RTitleLogo2 = LogoTopText2.Text;
            Diy.RUserLogo = LogoBomText.Text;
            //调用生成程序
            bool gensta = FileGen.Gen生成插件();
            if (gensta)
            {
                MessageBox.Show("生成操作已完成");
            }

        }
        private void checkf4_CheckedChanged(object sender, EventArgs e)
        {
            if(p1aj8.Checked==true)
            {
                keystats.Enabled = false;
                keystats.Text = "已禁用";
                return;
            }
            else
            {
                keystats.Enabled = true;
                keystats.Text = Data.键码表[(int)tag自定义按键[8]];
            }
        }
        #endregion
        #region 第三页:界面定制-控件事件
        private void Logo_TextChanged(object sender, EventArgs e)
        {
            string str = ((TextBox)sender).Text;
            if (str.Length > 14) 
            {
                ((TextBox)sender).Text = "内容太多!请精简内容!";
                MessageBox.Show("不能超过14个字符\r\n请重新输入!");
                ((TextBox)sender).Text = null;
                return; 
            }
            

        }
        private void 功能显示_CheckedChanged(object sender, EventArgs e)
        {
            int tnum = Convert.ToInt32(((CheckBox)sender).Tag);//获取调用此方法的控件Tag编号
            bool tchk = ((CheckBox)sender).Checked;//获取调用此方法的控件Checked状态
            Diy.Diy获取属性(tnum, tchk);
        }
        #endregion
        #region 第四页:草书读写-读写内置键盘加速器参数页面控件事件
        private void 滑动条_ValueChanged(object sender, EventArgs e)
        {
            //键盘速度换算
            int spdint = 0;
            string spdstr = null;
            Conv.SpeedDataConvert(speed滑动条.Value, out spdint, out spdstr);
            speed参数值.Text = Convert.ToString(speed滑动条.Value);
            speed实际值.Text = spdstr;

            //首次延时换算
            delay参数值.Text = Convert.ToString(delay滑动条.Value);
            int depid = Conv.DelayNowValue(speed滑动条.Value);
            delay实际值.Text = Conv.DelayOperaToView(speed滑动条.Value, depid);
            delay滑动条.Value = depid;
            //
            if (speed滑动条.Value >= 70)
            {
                SuperFast.Enabled = true;
            }
            else
            {
                SuperFast.Enabled = false;
                SuperFast.Checked = false;
            }
        }

        private void delay滑动条_ValueChanged(object sender, EventArgs e)
        {
            //首次延时换算
            delay参数值.Text = Convert.ToString(delay滑动条.Value);
            delay实际值.Text = Conv.DelayOperaToView(speed滑动条.Value, delay滑动条.Value);
        }
        private void cs读取参数按钮_Click(object sender, EventArgs e)
        {
            ReadKey();
            MessageBox.Show("配置数据读取完成");
        }

        private void cs默认参数按钮_Click(object sender, EventArgs e)
        {
            SpeedShow.Checked = true;
            Accelerator.Checked = true;
            SuperFast.Checked = false;
            speed滑动条.Value = 39;
            delay滑动条.Value = 98;
        }

        private void cs写入参数按钮_Click(object sender, EventArgs e)
        {
            WriteKey();
            MessageBox.Show("配置参数已写入");
        }
        #endregion
        #region 第五页:关于软件-页面控件事件
        private void link教学视频_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (License.LicSta <= 10)
            {
                License.LicSta += 5;
            }
            Key输入框.Enabled = true; 
            检查授权状态();
            link教学视频.LinkVisited = true;
            System.Diagnostics.Process.Start("https://space.bilibili.com/1177161017");
        }
        private void link个人博客_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (License.LicSta <= 10)
            {
                License.LicSta += 6;
            }
            Key输入框.Enabled = true; 
            检查授权状态();
            link个人博客.LinkVisited = true;
            System.Diagnostics.Process.Start("https://blog.csdn.net/zuoyuanchun");
        }

        private void Key输入框_MouseClick(object sender, MouseEventArgs e)
        {
            if (Key输入框.TextLength>0)
            {
                Key输入框.Text = "";
            }
            
        }

        private void Key输入框_TextChanged(object sender, EventArgs e)
        {
            if (Key输入框.Text == "2950800")
            {
                RegU reg = new RegU();
                reg.SetValue(Data.reg授权码, Key输入框.Text);
                reg = null;
                //
                License.LicSta = 17;
                Key输入框.Text = "LV.3";
                Key输入框.Enabled = false;
            }
            if (Key输入框.Text == "zuoyuanchun")
            {
                RegU reg = new RegU();
                reg.SetValue(Data.reg授权码, Key输入框.Text);
                reg = null;
                //
                License.LicSta = 27;
                Key输入框.Text = "LV.4";
                Key输入框.Enabled = false;
            }
            if (Key输入框.Text == "5127" | Key输入框.Text == "1177161017")
            {
                RegU reg = new RegU();
                reg.SetValue(Data.reg授权码, Key输入框.Text);
                reg = null;
                //
                License.LicSta = 37;
                Key输入框.Text = "LV.5";
                Key输入框.Enabled = false;
            }
            检查授权状态();
        }
        #endregion

     
    }
}
