using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.IO;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TetrisMonitor
{
    public partial class Main : Form
    {
        #region 全局
        public Main()
        {
            InitializeComponent();
        }
        private AutoResetEvent ExitEvent = new AutoResetEvent(false);
        private Thread MonProc;
        private Thread ChgForm;
        private int OldProcNum;
        public ArrayList ProcID = new ArrayList();//进程ID
        public ArrayList StartTime = new ArrayList();//开始时间
        public ArrayList StopTime = new ArrayList();//结束时间
        public ArrayList RunTime = new ArrayList();//运行时间
        public ArrayList QQid = new ArrayList();//QQ号码
        public ArrayList ProcState = new ArrayList();//进程状态
        private string strok = "正常";
        private string strdrop = "掉线";
        private string strexit = "退出";
        private bool Playing;
        private bool playstate;
        private bool Lock1, Lock2;
        private void Main_Load(object sender, EventArgs e)
        {
            base.Text = "QQ游戏_火拼俄罗斯掉线监控报警助手 V1.3   BY:工控闪剑";
            Playing = false;
            playstate = false;
            OldProcNum = 0;
            Lock1 = false;
            Lock2 = false;
            ListInit();
            MusicInit();
            MonProc = new Thread(new ThreadStart(MonitorProc));
            MonProc.Start();
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            MonProc.Abort();
            MonProc = null;
        }
        #endregion
        #region 子线程程序
        private void MonitorProc()
        {
            this.Invoke(new Action(() =>
            {
                string logstr = "监控已开启[" + DateTime.Now.ToString("F") + "]";
                loglist.ATC(logstr, Color.Black);
            }));
            int DropPid = 0;
            int GameProcNum = 0;
            #region 大循环
            while (true)
            {
                //获取游戏进程数据
                #region 获取进程数据
                Thread.Sleep(100);
                int gcn = ProcApi.Chk检测游戏进程();
                if (gcn == 0)
                {
                    this.Invoke(new Action(() =>
                    {
                        qqlist.Items.Clear();
                    }));
                }
                if (gcn > 0 && gcn != OldProcNum)
                {
                    //新加入一个进程数量不变不进行轮询扫描数据,降低CPU使用率
                    Thread.Sleep(3000);
                    GameProcNum = ProcApi.Chk检测游戏进程();
                    OldProcNum = GameProcNum;
                    this.Invoke(new Action(() =>
                    {
                        qqlist.Items.Clear();
                        for (int q = 0; q < GameProcNum; q++)
                        {
                            qqlist.Items.Add(ProcApi.QQid[q].ToString());
                        }
                    }));

                    DateTime[] dttmp = ProcApi.StartTime;
                    //历遍排序后的时间
                    for (int a1 = 0; a1 < dttmp.Length; a1++)
                    {
                        //未排序指定下标数据与排序后数据相等
                        if (ProcApi.ProcID[a1] > 0 && ProcApi.StartTime[a1] > DateTime.MinValue)
                        {
                            //有新游戏进程加入
                            if (!StartTime.Contains(dttmp[a1]))
                            {
                                ProcID.Add(ProcApi.ProcID[a1]);//进程ID
                                StartTime.Add(ProcApi.StartTime[a1]);//加入时间
                                StopTime.Add(DateTime.MinValue);//退出时间
                                RunTime.Add(TimeSpan.MinValue);//用时
                                QQid.Add(ProcApi.QQid[a1]);//游戏账号
                                ProcState.Add(strok);//状态
                                this.Invoke(new Action(() =>
                                {
                                    string logstr = "新游戏窗口[PID:" + ProcID[a1] + "◆" + ProcApi.StartTime[a1].ToString("T") + "◆" + ProcApi.QQid[a1].ToString() + "]";
                                    loglist.ATC(logstr, Color.Green);
                                }));
                            }
                        }
                    }
                }
                #endregion
                //检查是否有掉线进程
                #region 掉线进程检测
                do
                {
                    DropPid = ProcApi.ccp检查关闭进程();
                    if (DropPid > 0)
                    {
                        this.Invoke(new Action(() =>
                        {
                            string logstr = "检测到弹窗并关闭![操作时间:" + DateTime.Now.ToString("T") + "]";
                            loglist.ATC(logstr, Color.Brown);
                            this.Show();
                            this.WindowState = FormWindowState.Normal;
                        }));
                        if (ProcID.Contains(DropPid))
                        {
                            int pnum = ProcID.IndexOf(DropPid);
                            StopTime[pnum] = DateTime.Now;
                            RunTime[pnum] = (DateTime)StopTime[pnum] - (DateTime)StartTime[pnum];
                            ProcState[pnum] = strdrop;
                            this.Invoke(new Action(() =>
                            {
                                TimeSpan run = (TimeSpan)RunTime[pnum];
                                string logstr = "游戏掉线【PID:" + ProcID[pnum] + "★" + run.ToString("c") + "★" + QQid[pnum].ToString() + "】";
                                loglist.ATC(logstr, Color.Red);
                            }));
                        }
                        if (!Playing)
                        {
                            Playing = true;
                        }
                        Thread.Sleep(200);
                    }
                } while (DropPid > 0);
                #endregion
                //检查进程是否退出
                #region 检测退出进程
                for (int c1 = 1; c1 < StartTime.Count; c1++)
                {
                    bool plost = true;
                    foreach (DateTime pt in ProcApi.StartTime)
                    {
                        if ((DateTime)StartTime[c1] == pt)
                        {
                            plost = false;
                        }
                    }
                    if (plost && (string)ProcState[c1] == strok)
                    {
                        StopTime[c1] = DateTime.Now;
                        RunTime[c1] = (DateTime)StopTime[c1] - (DateTime)StartTime[c1];
                        ProcState[c1] = strexit;
                        this.Invoke(new Action(() =>
                        {
                            TimeSpan run = (TimeSpan)RunTime[c1];
                            string logstr = "游戏窗口被关闭【PID:" + ProcID[c1] + "★" + run.ToString("c") + "★" + QQid[c1].ToString() + "】";
                            loglist.ATC(logstr, Color.DarkBlue);
                        }));

                    }
                }
                #endregion
                //播放报警音乐
                #region 播放报警音乐
                if (Playing && !playstate)
                {
                    this.Invoke(new Action(() =>
                    {
                        WMP.settings.setMode("loop", MusicCycle.Checked);
                        WMP.Ctlcontrols.play();
                        PauseMusic.ForeColor = Color.Red;
                        string logstr = "播放报警音乐![操作时间:" + DateTime.Now.ToString("T") + "]";
                        loglist.ATC(logstr, Color.BlueViolet);
                    }));
                    playstate = true;
                }
                #endregion
                //当前时间显示
                #region 显示当前系统时间
                DateTime t = DateTime.Now;
                this.Invoke(new Action(() =>
                {
                    shijian.Text = t.ToString("yyyy/MM/dd") + "\r\n" + t.ToString("T");
                }));
                #endregion
            }
            #endregion
        }
        #endregion
        #region 初始化方法
        private void ListInit()
        {
            ProcID.Clear();
            StartTime.Clear();
            StopTime.Clear();
            RunTime.Clear();
            QQid.Clear();
            ProcState.Clear();
        }
        private void MusicInit()
        {
            WMP.settings.autoStart = false;
            string SndPath = Path.Combine(Directory.GetCurrentDirectory(), "music.mp3");
            if (!File.Exists(SndPath))
            {
                byte[] mp3 = global::TetrisMonitor.Properties.Resources.xz;
                BinaryWriter wr = null;
                wr = new BinaryWriter(File.Open(SndPath, FileMode.Create));
                wr.Write(mp3, 0, mp3.Length);
                wr.Flush();
                wr.Close();
                wr = null;
            }
            WMP.URL = SndPath;
        }
        #endregion
        #region 按钮事件处理
        private void PauseMusic_Click(object sender, EventArgs e)
        {
            int playsta = (int)WMP.playState;
            if (playsta == 3)
            {
                string logstr = "停止播放报警音乐[操作时间:" + DateTime.Now.ToString("T") + "]";
                loglist.ATC(logstr, Color.Teal);
            }
            WMP.Ctlcontrols.stop();
            PauseMusic.ForeColor = Color.Black;
            playstate = false;
            Playing = false;

        }
        private void WindowTop_CheckedChanged(object sender, EventArgs e)
        {
            base.TopMost = WindowTop.Checked;
        }
        private void link教学视频_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://space.bilibili.com/1177161017");
        }
        private void link检查更新_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://2950800.ysepan.com/");
        }
       
        private void 托盘_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Show();
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.Hide();
                this.WindowState = FormWindowState.Minimized;
            }
        }
        #endregion
    }
}
