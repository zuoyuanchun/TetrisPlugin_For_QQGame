using System;
using System.IO;
using System.Windows.Forms;

namespace QQGameTool
{
    public partial class Main : Form
    {

        public Main()
        {
            InitializeComponent();
        }
        #region 窗口加载事件和全局变量实例化
        private int LicLevel;
        private byte[] tag自定义按键 = new byte[10];
        private bool MoveFlag;
        private int xPos;
        private int yPos;
        private int[] xDiyPos = new int[20];
        private int[] yDiyPos = new int[20];
        private void MainForm_Load(object sender, EventArgs e)
        {
            this.Text = "火拼俄罗斯方块刷分插件生成器 BY:工控闪剑";
            cmtext.Text = Data.软件说明;
            CheckKey();
            ReadKey();
            读取路径信息();
            检查授权状态();
            读取按键配置();
            DiyDefaultPos();
            DiySavePos();
            //注意顺序,颠倒可能报错
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
            if (DirCheck.Chk检测游戏进程())
            {
                DirCheck.Set选择目录检测(DirCheck.ExeDir);
                checkok = true;
            }
            if (checkok)
            {
                tb0俄罗斯路径.Text = DirCheck.Fp0游戏目录;
            }
            else
            {
                tb0俄罗斯路径.Text = "未获取到游戏路径!请进入游戏后点击[即时读取]按钮读取信息";
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
                Gen生成插件.Enabled = true;
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
                obr.Close();
                ofs.Close();
                obr = null;
                ofs = null;
            }
            else
            {
                Gen生成插件.Enabled = false;
                string msg1 = "未找到游戏的配置文件!插件生成功能将禁用!\r\n";
                string msg2 = "1.进入游戏后设置按键,并退出游戏窗口!\r\n";
                string msg3 = "2.重新进入游戏,点击[即时读取]按钮获取按键设置!\r\n";
                string msgt = "未找到配置文件";
                MessageBox.Show(msg1 + msg2 + msg3, msgt, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                tag自定义按键[1] = 38;
                tag自定义按键[2] = 96;
                tag自定义按键[3] = 37;
                tag自定义按键[4] = 39;
                tag自定义按键[5] = 40;
                tag自定义按键[6] = 32;
                tag自定义按键[7] = 114;
            }

        }
        private void 检查授权状态()
        {
            //LicLevel = 0;//开发时强制授权
            if (LicLevel > 0)
            {
                Group31Reg参数设置.Enabled = true;
                bt31读取参数.Enabled = true;
                bt32默认参数.Enabled = true;
                bt33写入参数.Enabled = true;
            }
            if (LicLevel > 15)
            {
                Panel11插件布局.Enabled = true;
                Do1恢复默认.Enabled = true;
                Do2保存更改.Enabled = true;
            }
            if (LicLevel > 20)
            {
                Panel22自杀延时.Enabled = true;
                TimeHalValue.Text = "258";
                TimeTenValue.Text = "11000";
            }
            if (LicLevel > 30)
            {
                Panel12LOGO文本.Enabled = true;
            }
        }
        private void Key验证授权()
        {
            RegU reg = new RegU();
            string KeyStr = reg.getValue(Data.reg授权码);
            if (KeyStr == "2950800")
            {
                LicLevel = 17;
                Key输入框.Text = "LV.3";
                Key输入框.Enabled = false;
            }
            if (KeyStr == "zuoyuanchun")
            {
                LicLevel = 27;
                Key输入框.Text = "LV.4";
                Key输入框.Enabled = false;
            }
            if (KeyStr == "5127" | KeyStr == "1177161017")
            {
                LicLevel = 37;
                Key输入框.Text = "LV.5";
                Key输入框.Enabled = false;
            }
            检查授权状态();
        }
        #endregion
        #region 第一页:界面定制-控件事件
        private void tptop顶部标题_Click(object sender, EventArgs e)
        {
            if(((Button)sender).Text==LogoTopText1.Text)
            {
                ((Button)sender).Text = LogoTopText2.Text;
            }
            else
            {
                ((Button)sender).Text = LogoTopText1.Text;
            }
        }

        private void DiyCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (MoveFlag)
            {
                ((CheckBox)sender).Left += Convert.ToInt16(e.X - xPos);//设置x坐标.
                ((CheckBox)sender).Top += Convert.ToInt16(e.Y - yPos);//设置y坐标.
            }
        }
        private void DiyButton_MouseMove(object sender, MouseEventArgs e)
        {
            if (MoveFlag)
            {
                ((Button)sender).Left += Convert.ToInt16(e.X - xPos);//设置x坐标.
                ((Button)sender).Top += Convert.ToInt16(e.Y - yPos);//设置y坐标.
            }
        }
        private void DiyLabel_MouseMove(object sender, MouseEventArgs e)
        {
            if (MoveFlag)
            {
                ((Label)sender).Left += Convert.ToInt16(e.X - xPos);//设置x坐标.
                ((Label)sender).Top += Convert.ToInt16(e.Y - yPos);//设置y坐标.
            }
        }
        private void DiyTrackBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (MoveFlag)
            {
                ((TrackBar)sender).Left += Convert.ToInt16(e.X - xPos);//设置x坐标.
                ((TrackBar)sender).Top += Convert.ToInt16(e.Y - yPos);//设置y坐标.
            }
        }
        private void Diy_MouseUp(object sender, MouseEventArgs e)
        {
            MoveFlag = false;
        }
        private void Diy_MouseDown(object sender, MouseEventArgs e)
        {
            MoveFlag = true;//已经按下.
            xPos = e.X;//当前x坐标.
            yPos = e.Y;//当前y坐标.
        }
        private void DoSaveDiy_Click(object sender, EventArgs e)
        {
            Panel11插件布局.Enabled = false;
            Panel12LOGO文本.Enabled = false;
            DiySavePos();
        }

        private void DoDefaultDiy_Click(object sender, EventArgs e)
        {
            if (LicLevel > 15)
            {
                Panel11插件布局.Enabled = true;
            }
            if (LicLevel > 30)
            {
                Panel12LOGO文本.Enabled = true;
            }

            tp1自动开始.Left = xDiyPos[1];
            tp1自动开始.Top = yDiyPos[1];
            tp2方块不落.Left = xDiyPos[2];
            tp2方块不落.Top = yDiyPos[2];
            tp3半秒自杀.Left = xDiyPos[3];
            tp3半秒自杀.Top = yDiyPos[3];
            tp4十秒自杀.Left = xDiyPos[4];
            tp4十秒自杀.Top = yDiyPos[4];
            tp5显示速度.Left = xDiyPos[5];
            tp5显示速度.Top = yDiyPos[5];
            tp6数据统计.Left = xDiyPos[6];
            tp6数据统计.Top = yDiyPos[6];
            tp7键盘加速.Left = xDiyPos[7];
            tp7键盘加速.Top = yDiyPos[7];
            tp8超快响应.Left = xDiyPos[8];
            tp8超快响应.Top = yDiyPos[8];
            tpa速度条.Left = xDiyPos[9];
            tpa速度条.Top = yDiyPos[9];
            tpa速度文本.Left = xDiyPos[10];
            tpa速度文本.Top = yDiyPos[10];
            tpa延时条.Left = xDiyPos[11];
            tpa延时条.Top = yDiyPos[11];
            tpa延时文本.Left = xDiyPos[12];
            tpa延时文本.Top = yDiyPos[12];
            tptop顶部标题.Left = xDiyPos[13];
            tptop顶部标题.Top = yDiyPos[13];
            tpbom底部标题.Left = xDiyPos[14];
            tpbom底部标题.Top = yDiyPos[14];


        }
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
            string tag = ((TextBox)sender).Tag.ToString();
            if (tag == "top1" | tag == "top2")
            {
                tptop顶部标题.Text = ((TextBox)sender).Text;
            }
            if (tag == "bom")
            {
                tpbom底部标题.Text = LogoBomText.Text;
            }
        }
        private void DiyDefaultPos()
        {
            tptop顶部标题.Text = LogoTopText1.Text;
            tpbom底部标题.Text = LogoBomText.Text;
            xDiyPos[1] = tp1自动开始.Left;
            yDiyPos[1] = tp1自动开始.Top;
            xDiyPos[2] = tp2方块不落.Left;
            yDiyPos[2] = tp2方块不落.Top;
            xDiyPos[3] = tp3半秒自杀.Left;
            yDiyPos[3] = tp3半秒自杀.Top;
            xDiyPos[4] = tp4十秒自杀.Left;
            yDiyPos[4] = tp4十秒自杀.Top;
            xDiyPos[5] = tp5显示速度.Left;
            yDiyPos[5] = tp5显示速度.Top;
            xDiyPos[6] = tp6数据统计.Left;
            yDiyPos[6] = tp6数据统计.Top;
            xDiyPos[7] = tp7键盘加速.Left;
            yDiyPos[7] = tp7键盘加速.Top;
            xDiyPos[8] = tp8超快响应.Left;
            yDiyPos[8] = tp8超快响应.Top;
            xDiyPos[9] = tpa速度条.Left;
            yDiyPos[9] = tpa速度条.Top;
            xDiyPos[10] = tpa速度文本.Left;
            yDiyPos[10] = tpa速度文本.Top;
            xDiyPos[11] = tpa延时条.Left;
            yDiyPos[11] = tpa延时条.Top;
            xDiyPos[12] = tpa延时文本.Left;
            yDiyPos[12] = tpa延时文本.Top;
            xDiyPos[13] = tptop顶部标题.Left;
            yDiyPos[13] = tptop顶部标题.Top;
            xDiyPos[14] = tpbom底部标题.Left;
            yDiyPos[14] = tpbom底部标题.Top;
        }
        private void DiySavePos()
        {
            Diy.Bt1x自动开始 = DConvPos(tp1自动开始.Left);
            Diy.Bt1y自动开始 = DConvPos(tp1自动开始.Top);
            Diy.Bt2x方块不落 = DConvPos(tp2方块不落.Left);
            Diy.Bt2y方块不落 = DConvPos(tp2方块不落.Top);
            Diy.Bt3x半秒自杀 = DConvPos(tp3半秒自杀.Left);
            Diy.Bt3y半秒自杀 = DConvPos(tp3半秒自杀.Top);
            Diy.Bt4x十秒自杀 = DConvPos(tp4十秒自杀.Left);
            Diy.Bt4y十秒自杀 = DConvPos(tp4十秒自杀.Top);
            Diy.Bt5x显示速度 = DConvPos(tp5显示速度.Left);
            Diy.Bt5y显示速度 = DConvPos(tp5显示速度.Top);
            Diy.Bt6x数据统计 = DConvPos(tp6数据统计.Left);
            Diy.Bt6y数据统计 = DConvPos(tp6数据统计.Top);
            Diy.Bt7x键盘加速 = DConvPos(tp7键盘加速.Left);
            Diy.Bt7y键盘加速 = DConvPos(tp7键盘加速.Top);
            Diy.Bt8x超快响应 = DConvPos(tp8超快响应.Left);
            Diy.Bt8y超快响应 = DConvPos(tp8超快响应.Top);
            Diy.Bt9x键盘速度 = DConvPos(tpa速度条.Left);
            Diy.Bt9y键盘速度 = DConvPos(tpa速度条.Top);
            Diy.Bt10x首次延时 = DConvPos(tpa延时条.Left);
            Diy.Bt10y首次延时 = DConvPos(tpa延时条.Top);
            Diy.Bt11x键速文本 = DConvPos(tpa速度文本.Left);
            Diy.Bt11y键速文本 = DConvPos(tpa速度文本.Top);
            Diy.Bt12x首延文本 = DConvPos(tpa延时文本.Left);
            Diy.Bt12y首延文本 = DConvPos(tpa延时文本.Top);
            Diy.Bt13x顶部logo = DConvPos(tptop顶部标题.Left);
            Diy.Bt13y顶部logo = DConvPos(tptop顶部标题.Top);
            Diy.Bt14x底部logo = DConvPos(tpbom底部标题.Left);
            Diy.Bt14y底部logo = DConvPos(tpbom底部标题.Top);
        }
        private byte DConvPos(int pos)
        {
            int cpos = pos / 2;
            if (cpos < 0 | cpos > 150) 
            {
                cpos = 180;
            }
            return Convert.ToByte(cpos);
        }
        #endregion
        #region 第二页:操作按键-控件事件
        private void ReadDir读取进程路径_Click(object sender, EventArgs e)
        {
            if (DirCheck.Chk检测游戏进程())
            {
                string GameDir = DirCheck.ExeDir;
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
        private void 自杀键盘_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = e.KeyChar < '0' || e.KeyChar > '9';
            if (e.Handled)
            {
                MessageBox.Show("只允许输入数字!", "警告");
                ((TextBox)sender).Text = "1";
                return;
            }
        }
        private void 自杀键盘_TextChanged(object sender, EventArgs e)
        {
            int tlen = ((TextBox)sender).Text.Length;
            int num = Convert.ToInt32(((TextBox)sender).Text);
            if (tlen < 1 | tlen > 5 | num < 0 | num > 50000) 
            {
                MessageBox.Show("自杀延迟上限不允许超过50秒(50000毫秒)!\r\n否则没有意义!", "警告");
                ((TextBox)sender).Text = "1";
                return;
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

            //写入操作按键设置
            Diy.OperaKey = tag自定义按键;

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
                MessageBox.Show("生成操作已完成\r\n可以正常进行游戏");
            }

        }
        #endregion
        #region 第三页:参数读写-读写内置键盘加速器参数页面控件事件
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
            if (LicLevel <= 10)
            {
                LicLevel += 5;
            }
            Key输入框.Enabled = true;
            检查授权状态();
            link教学视频.LinkVisited = true;
            System.Diagnostics.Process.Start("https://space.bilibili.com/1177161017");
        }
        private void link个人博客_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (LicLevel <= 10)
            {
                LicLevel += 6;
            }
            Key输入框.Enabled = true;
            检查授权状态();
            link个人博客.LinkVisited = true;
            System.Diagnostics.Process.Start("https://blog.csdn.net/zuoyuanchun");
        }
        private void linkUpdate_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://2950800.ysepan.com/");
        }
        private void Key输入框_MouseClick(object sender, MouseEventArgs e)
        {
            if (Key输入框.TextLength > 0)
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
                LicLevel = 17;
                Key输入框.Text = "LV.3";
                Key输入框.Enabled = false;
            }
            if (Key输入框.Text == "zuoyuanchun")
            {
                RegU reg = new RegU();
                reg.SetValue(Data.reg授权码, Key输入框.Text);
                reg = null;
                //
                LicLevel = 27;
                Key输入框.Text = "LV.4";
                Key输入框.Enabled = false;
            }
            if (Key输入框.Text == "5127" | Key输入框.Text == "1177161017")
            {
                RegU reg = new RegU();
                reg.SetValue(Data.reg授权码, Key输入框.Text);
                reg = null;
                //
                LicLevel = 37;
                Key输入框.Text = "LV.5";
                Key输入框.Enabled = false;
            }
            检查授权状态();
        }
        #endregion

        

        
    }
}
