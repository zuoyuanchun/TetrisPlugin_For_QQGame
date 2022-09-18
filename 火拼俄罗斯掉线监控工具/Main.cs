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
        private bool 暂停处理数据;
        private bool Lock1, Lock2;
        private void Main_Load(object sender, EventArgs e)
        {
            base.Text = "QQ游戏_火拼俄罗斯方块掉线监控报警工具BY:工控闪剑";
            Playing = false;
            playstate = false;
            暂停处理数据 = false;
            Lock1 = false;
            Lock2 = false;
            ListInit();
            MusicInit();
            MonProc = new Thread(new ThreadStart(MonitorProc));
            MonProc.Start();
            ChgForm = new Thread(new ThreadStart(ChangeForm));
            ChgForm.Start();
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            MonProc.Abort();
            MonProc = null;
            ChgForm.Abort();
            ChgForm = null;
        }
        #endregion
        #region 子线程程序
        private void MonitorProc()
        {
            int DropPid = 0;
            int GameProcNum = 0;
            while (true)
            {
                while (暂停处理数据) { Lock1 = true; }
                //获取游戏进程数据
                GameProcNum = ProcApi.Chk检测游戏进程();
                if (GameProcNum == 0 && ProcApi.StartTime != null) 
                {
                    ProcApi.StartTime[0] = DateTime.MinValue;
                }
                if (GameProcNum > 0)
                {

                    DateTime[] dttmp = ProcApi.StartTime;
                    Array.Sort(dttmp);
                    //历遍排序后的时间
                    for (int a1 = 0; a1 < dttmp.Length; a1++)
                    {
                        //历遍未排序的时间
                        for (int a2 = 0; a2 < dttmp.Length; a2++)
                        {
                            //未排序指定下标数据与排序后数据相等
                            if (ProcApi.StartTime[a2] == dttmp[a1] && ProcApi.ProcID[a2] > 0) 
                            {
                                //有新游戏进程加入
                                if (!StartTime.Contains(dttmp[a1]))
                                {
                                    ProcID.Add(ProcApi.ProcID[a2]);
                                    StartTime.Add(ProcApi.StartTime[a2]);
                                    StopTime.Add(DateTime.MinValue);
                                    RunTime.Add(TimeSpan.MinValue);
                                    QQid.Add(ProcApi.QQid[a2]);
                                    ProcState.Add(strok);
                                }
                            }
                        }
                    }
                }
                //检查是否有掉线进程
                DropPid = ProcApi.ccp检查关闭进程();
                if (DropPid > 0)
                {
                    if (ProcID.Contains(DropPid))
                    {
                        int pnum = ProcID.IndexOf(DropPid);
                        StopTime[pnum] = DateTime.Now;
                        RunTime[pnum] = (DateTime)StopTime[pnum] - (DateTime)StartTime[pnum];
                        ProcState[pnum] = strdrop;
                    }
                    if (!Playing)
                    {
                        Playing = true;
                    }
                }
                //检查进程是否退出
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
                    }
                }
            }

        }
        private void ChangeForm()
        {
            int prnum = 0;
            string st = string.Empty;
            while (true)
            {
                while (暂停处理数据) { Lock2 = true; }
                this.Invoke(new Action(() =>
                {
                    base.TopMost = WindowTop.Checked;
                }));
                if (Playing && !playstate)
                {
                    this.Invoke(new Action(() =>
                    {
                        WMP.settings.setMode("loop", MusicCycle.Checked);
                        WMP.Ctlcontrols.play();
                        PauseMusic.BackColor = Color.Red;
                        PauseMusic.UseVisualStyleBackColor = true;
                    }));
                    playstate = true;
                }
                if (StartTime == null)
                {
                    prnum = 0;
                }
                else
                {
                    prnum = StartTime.Count;
                }
                //第1行数据
                if (prnum > 1 | join1.Text.Length > 2)
                {
                    this.Invoke(new Action(() =>
                    {
                        pid1.Text = ProcID[1].ToString();
                        join1.Text = string.Format("{0:T}", StartTime[1]);
                        qqnum1.Text = QQid[1].ToString();
                        state1.Text = ProcState[1].ToString();
                    }));
                    st = state1.Text;
                    if (st == strexit | st == strdrop)
                    {
                        this.Invoke(new Action(() =>
                        {
                            state1.ForeColor = Color.Red;
                        }));
                    }
                    else
                    {
                        this.Invoke(new Action(() =>
                        {
                            state1.ForeColor = Color.Black;
                        }));
                    }
                    if ((DateTime)StopTime[1] != DateTime.MinValue && (TimeSpan)RunTime[1] != TimeSpan.MinValue)
                    {
                        this.Invoke(new Action(() =>
                        {
                            exit1.Text = string.Format("{0:T}", StopTime[1]);
                            TimeSpan work = (TimeSpan)RunTime[1];
                            work1.Text = work.ToString(@"hh\:mm\:ss");
                        }));
                    }
                }
                //第2行数据
                if (prnum > 2 | join2.Text.Length > 2)
                {
                    this.Invoke(new Action(() =>
                    {
                        pid2.Text = ProcID[2].ToString();
                        join2.Text = string.Format("{0:T}", StartTime[2]);
                        qqnum2.Text = QQid[2].ToString();
                        state2.Text = ProcState[2].ToString();
                    }));
                    st = state2.Text;
                    if (st == strexit | st == strdrop)
                    {
                        this.Invoke(new Action(() =>
                        {
                            state2.ForeColor = Color.Red;
                        }));
                    }
                    else
                    {
                        this.Invoke(new Action(() =>
                        {
                            state2.ForeColor = Color.Black;
                        }));
                    }
                    if ((DateTime)StopTime[2] != DateTime.MinValue && (TimeSpan)RunTime[2] != TimeSpan.MinValue)
                    {
                        this.Invoke(new Action(() =>
                        {
                            exit2.Text = string.Format("{0:T}", StopTime[2]);
                            TimeSpan work = (TimeSpan)RunTime[2];
                            work2.Text = work.ToString(@"hh\:mm\:ss");
                        }));
                    }
                }
                //第3行数据
                if (prnum > 3 | join3.Text.Length > 2)
                {
                    this.Invoke(new Action(() =>
                    {
                        pid3.Text = ProcID[3].ToString();
                        join3.Text = string.Format("{0:T}", StartTime[3]);
                        qqnum3.Text = QQid[3].ToString();
                        state3.Text = ProcState[3].ToString();
                    }));
                    st = state3.Text;
                    if (st == strexit | st == strdrop)
                    {
                        this.Invoke(new Action(() =>
                        {
                            state3.ForeColor = Color.Red;
                        }));
                    }
                    else
                    {
                        this.Invoke(new Action(() =>
                        {
                            state3.ForeColor = Color.Black;
                        }));
                    }
                    if ((DateTime)StopTime[3] != DateTime.MinValue && (TimeSpan)RunTime[3] != TimeSpan.MinValue)
                    {
                        this.Invoke(new Action(() =>
                        {
                            exit3.Text = string.Format("{0:T}", StopTime[3]);
                            TimeSpan work = (TimeSpan)RunTime[3];
                            work3.Text = work.ToString(@"hh\:mm\:ss");
                        }));
                    }
                }
                //第4行数据
                if (prnum > 4 | join4.Text.Length > 2)
                {
                    this.Invoke(new Action(() =>
                    {
                        pid4.Text = ProcID[4].ToString();
                        join4.Text = string.Format("{0:T}", StartTime[4]);
                        qqnum4.Text = QQid[4].ToString();
                        state4.Text = ProcState[4].ToString();
                    }));
                    st = state4.Text;
                    if (st == strexit | st == strdrop)
                    {
                        this.Invoke(new Action(() =>
                        {
                            state4.ForeColor = Color.Red;
                        }));
                    }
                    else
                    {
                        this.Invoke(new Action(() =>
                        {
                            state4.ForeColor = Color.Black;
                        }));
                    }
                    if ((DateTime)StopTime[4] != DateTime.MinValue && (TimeSpan)RunTime[4] != TimeSpan.MinValue)
                    {
                        this.Invoke(new Action(() =>
                        {
                            exit4.Text = string.Format("{0:T}", StopTime[4]);
                            TimeSpan work = (TimeSpan)RunTime[4];
                            work4.Text = work.ToString(@"hh\:mm\:ss");
                        }));
                    }
                }
                //第5行数据
                if (prnum > 5 | join5.Text.Length > 2)
                {
                    this.Invoke(new Action(() =>
                    {
                        pid5.Text = ProcID[5].ToString();
                        join5.Text = string.Format("{0:T}", StartTime[5]);
                        qqnum5.Text = QQid[5].ToString();
                        state5.Text = ProcState[5].ToString();
                    }));
                    st = state5.Text;
                    if (st == strexit | st == strdrop)
                    {
                        this.Invoke(new Action(() =>
                        {
                            state5.ForeColor = Color.Red;
                        }));
                    }
                    else
                    {
                        this.Invoke(new Action(() =>
                        {
                            state5.ForeColor = Color.Black;
                        }));
                    }
                    if ((DateTime)StopTime[5] != DateTime.MinValue && (TimeSpan)RunTime[5] != TimeSpan.MinValue)
                    {
                        this.Invoke(new Action(() =>
                        {
                            exit5.Text = string.Format("{0:T}", StopTime[5]);
                            TimeSpan work = (TimeSpan)RunTime[5];
                            work5.Text = work.ToString(@"hh\:mm\:ss");
                        }));
                    }
                }
                //第6行数据
                if (prnum > 6 | join6.Text.Length > 2)
                {
                    this.Invoke(new Action(() =>
                    {
                        pid6.Text = ProcID[6].ToString();
                        join6.Text = string.Format("{0:T}", StartTime[6]);
                        qqnum6.Text = QQid[6].ToString();
                        state6.Text = ProcState[6].ToString();
                    }));
                    st = state6.Text;
                    if (st == strexit | st == strdrop)
                    {
                        this.Invoke(new Action(() =>
                        {
                            state6.ForeColor = Color.Red;
                        }));
                    }
                    else
                    {
                        this.Invoke(new Action(() =>
                        {
                            state6.ForeColor = Color.Black;
                        }));
                    }
                    if ((DateTime)StopTime[6] != DateTime.MinValue && (TimeSpan)RunTime[6] != TimeSpan.MinValue)
                    {
                        this.Invoke(new Action(() =>
                        {
                            exit6.Text = string.Format("{0:T}", StopTime[6]);
                            TimeSpan work = (TimeSpan)RunTime[6];
                            work6.Text = work.ToString(@"hh\:mm\:ss");
                        }));
                    }
                }
                //第7行数据
                if (prnum > 7 | join7.Text.Length > 2)
                {
                    this.Invoke(new Action(() =>
                    {
                        pid7.Text = ProcID[7].ToString();
                        join7.Text = string.Format("{0:T}", StartTime[7]);
                        qqnum7.Text = QQid[7].ToString();
                        state7.Text = ProcState[7].ToString();
                    }));
                    st = state7.Text;
                    if (st == strexit | st == strdrop)
                    {
                        this.Invoke(new Action(() =>
                        {
                            state7.ForeColor = Color.Red;
                        }));
                    }
                    else
                    {
                        this.Invoke(new Action(() =>
                        {
                            state7.ForeColor = Color.Black;
                        }));
                    }
                    if ((DateTime)StopTime[7] != DateTime.MinValue && (TimeSpan)RunTime[7] != TimeSpan.MinValue)
                    {
                        this.Invoke(new Action(() =>
                        {
                            exit7.Text = string.Format("{0:T}", StopTime[7]);
                            TimeSpan work = (TimeSpan)RunTime[7];
                            work7.Text = work.ToString(@"hh\:mm\:ss");
                        }));
                    }
                }
                //第8行数据
                if (prnum > 8)
                {
                    this.Invoke(new Action(() =>
                    {
                        pid8.Text = ProcID[8].ToString();
                        join8.Text = string.Format("{0:T}", StartTime[8]);
                        qqnum8.Text = QQid[8].ToString();
                        state8.Text = ProcState[8].ToString();
                    }));
                    st = state8.Text;
                    if (st == strexit | st == strdrop)
                    {
                        this.Invoke(new Action(() =>
                        {
                            state8.ForeColor = Color.Red;
                        }));
                    }
                    else
                    {
                        this.Invoke(new Action(() =>
                        {
                            state8.ForeColor = Color.Black;
                        }));
                    }
                    if ((DateTime)StopTime[8] != DateTime.MinValue && (TimeSpan)RunTime[8] != TimeSpan.MinValue)
                    {
                        this.Invoke(new Action(() =>
                        {
                            exit8.Text = string.Format("{0:T}", StopTime[8]);
                            TimeSpan work = (TimeSpan)RunTime[8];
                            work8.Text = work.ToString(@"hh\:mm\:ss");
                        }));
                    }
                }
                //第9行数据
                if (prnum > 9)
                {
                    this.Invoke(new Action(() =>
                    {
                        pid9.Text = ProcID[9].ToString();
                        join9.Text = string.Format("{0:T}", StartTime[9]);
                        qqnum9.Text = QQid[9].ToString();
                        state9.Text = ProcState[9].ToString();
                    }));
                    st = state9.Text;
                    if (st == strexit | st == strdrop)
                    {
                        this.Invoke(new Action(() =>
                        {
                            state9.ForeColor = Color.Red;
                        }));
                    }
                    else
                    {
                        this.Invoke(new Action(() =>
                        {
                            state9.ForeColor = Color.Black;
                        }));
                    }
                    if ((DateTime)StopTime[9] != DateTime.MinValue && (TimeSpan)RunTime[9] != TimeSpan.MinValue)
                    {
                        this.Invoke(new Action(() =>
                        {
                            exit9.Text = string.Format("{0:T}", StopTime[9]);
                            TimeSpan work = (TimeSpan)RunTime[9];
                            work9.Text = work.ToString(@"hh\:mm\:ss");
                        }));
                    }
                }
                //第10行数据
                if (prnum > 10)
                {
                    this.Invoke(new Action(() =>
                    {
                        pid10.Text = ProcID[10].ToString();
                        join10.Text = string.Format("{0:T}", StartTime[10]);
                        qqnum10.Text = QQid[10].ToString();
                        state10.Text = ProcState[10].ToString();
                    }));
                    st = state10.Text;
                    if (st == strexit | st == strdrop)
                    {
                        this.Invoke(new Action(() =>
                        {
                            state10.ForeColor = Color.Red;
                        }));
                    }
                    else
                    {
                        this.Invoke(new Action(() =>
                        {
                            state10.ForeColor = Color.Black;
                        }));
                    }
                    if ((DateTime)StopTime[10] != DateTime.MinValue && (TimeSpan)RunTime[10] != TimeSpan.MinValue)
                    {
                        this.Invoke(new Action(() =>
                        {
                            exit10.Text = string.Format("{0:T}", StopTime[10]);
                            TimeSpan work = (TimeSpan)RunTime[10];
                            work10.Text = work.ToString(@"hh\:mm\:ss");
                        }));
                    }
                }
                //第11行数据
                if (prnum > 11)
                {
                    this.Invoke(new Action(() =>
                    {
                        pid11.Text = ProcID[11].ToString();
                        join11.Text = string.Format("{0:T}", StartTime[11]);
                        qqnum11.Text = QQid[11].ToString();
                        state11.Text = ProcState[11].ToString();
                    }));
                    st = state11.Text;
                    if (st == strexit | st == strdrop)
                    {
                        this.Invoke(new Action(() =>
                        {
                            state11.ForeColor = Color.Red;
                        }));
                    }
                    else
                    {
                        this.Invoke(new Action(() =>
                        {
                            state11.ForeColor = Color.Black;
                        }));
                    }
                    if ((DateTime)StopTime[11] != DateTime.MinValue && (TimeSpan)RunTime[11] != TimeSpan.MinValue)
                    {
                        this.Invoke(new Action(() =>
                        {
                            exit11.Text = string.Format("{0:T}", StopTime[11]);
                            TimeSpan work = (TimeSpan)RunTime[11];
                            work11.Text = work.ToString(@"hh\:mm\:ss");
                        }));
                    }
                }
                //第12行数据
                if (prnum > 12)
                {
                    this.Invoke(new Action(() =>
                    {
                        pid12.Text = ProcID[12].ToString();
                        join12.Text = string.Format("{0:T}", StartTime[12]);
                        qqnum12.Text = QQid[12].ToString();
                        state12.Text = ProcState[12].ToString();
                    }));
                    st = state12.Text;
                    if (st == strexit | st == strdrop)
                    {
                        this.Invoke(new Action(() =>
                        {
                            state12.ForeColor = Color.Red;
                        }));
                    }
                    else
                    {
                        this.Invoke(new Action(() =>
                        {
                            state12.ForeColor = Color.Black;
                        }));
                    }
                    if ((DateTime)StopTime[12] != DateTime.MinValue && (TimeSpan)RunTime[12] != TimeSpan.MinValue)
                    {
                        this.Invoke(new Action(() =>
                        {
                            exit12.Text = string.Format("{0:T}", StopTime[12]);
                            TimeSpan work = (TimeSpan)RunTime[12];
                            work12.Text = work.ToString(@"hh\:mm\:ss");
                        }));
                    }
                }
                //第13行数据
                if (prnum > 13)
                {
                    this.Invoke(new Action(() =>
                    {
                        pid13.Text = ProcID[13].ToString();
                        join13.Text = string.Format("{0:T}", StartTime[13]);
                        qqnum13.Text = QQid[13].ToString();
                        state13.Text = ProcState[13].ToString();
                    }));
                    st = state13.Text;
                    if (st == strexit | st == strdrop)
                    {
                        this.Invoke(new Action(() =>
                        {
                            state13.ForeColor = Color.Red;
                        }));
                    }
                    else
                    {
                        this.Invoke(new Action(() =>
                        {
                            state13.ForeColor = Color.Black;
                        }));
                    }
                    if ((DateTime)StopTime[13] != DateTime.MinValue && (TimeSpan)RunTime[13] != TimeSpan.MinValue)
                    {
                        this.Invoke(new Action(() =>
                        {
                            exit13.Text = string.Format("{0:T}", StopTime[13]);
                            TimeSpan work = (TimeSpan)RunTime[13];
                            work13.Text = work.ToString(@"hh\:mm\:ss");
                        }));
                    }
                }
                //第14行数据
                if (prnum > 14)
                {
                    this.Invoke(new Action(() =>
                    {
                        pid14.Text = ProcID[14].ToString();
                        join14.Text = string.Format("{0:T}", StartTime[14]);
                        qqnum14.Text = QQid[14].ToString();
                        state14.Text = ProcState[14].ToString();
                    }));
                    st = state14.Text;
                    if (st == strexit | st == strdrop)
                    {
                        this.Invoke(new Action(() =>
                        {
                            state14.ForeColor = Color.Red;
                        }));
                    }
                    else
                    {
                        this.Invoke(new Action(() =>
                        {
                            state14.ForeColor = Color.Black;
                        }));
                    }
                    if ((DateTime)StopTime[14] != DateTime.MinValue && (TimeSpan)RunTime[14] != TimeSpan.MinValue)
                    {
                        this.Invoke(new Action(() =>
                        {
                            exit14.Text = string.Format("{0:T}", StopTime[14]);
                            TimeSpan work = (TimeSpan)RunTime[14];
                            work14.Text = work.ToString(@"hh\:mm\:ss");
                        }));
                    }
                }
                //第15行数据
                if (prnum > 15)
                {
                    this.Invoke(new Action(() =>
                    {
                        pid15.Text = ProcID[15].ToString();
                        join15.Text = string.Format("{0:T}", StartTime[15]);
                        qqnum15.Text = QQid[15].ToString();
                        state15.Text = ProcState[15].ToString();
                    }));
                    st = state15.Text;
                    if (st == strexit | st == strdrop)
                    {
                        this.Invoke(new Action(() =>
                        {
                            state15.ForeColor = Color.Red;
                        }));
                    }
                    else
                    {
                        this.Invoke(new Action(() =>
                        {
                            state15.ForeColor = Color.Black;
                        }));
                    }
                    if ((DateTime)StopTime[15] != DateTime.MinValue && (TimeSpan)RunTime[15] != TimeSpan.MinValue)
                    {
                        this.Invoke(new Action(() =>
                        {
                            exit15.Text = string.Format("{0:T}", StopTime[15]);
                            TimeSpan work = (TimeSpan)RunTime[15];
                            work15.Text = work.ToString(@"hh\:mm\:ss");
                        }));
                    }
                }
                //第16行数据
                if (prnum > 16)
                {
                    this.Invoke(new Action(() =>
                    {
                        pid16.Text = ProcID[16].ToString();
                        join16.Text = string.Format("{0:T}", StartTime[16]);
                        qqnum16.Text = QQid[16].ToString();
                        state16.Text = ProcState[16].ToString();
                    }));
                    st = state16.Text;
                    if (st == strexit | st == strdrop)
                    {
                        this.Invoke(new Action(() =>
                        {
                            state16.ForeColor = Color.Red;
                        }));
                    }
                    else
                    {
                        this.Invoke(new Action(() =>
                        {
                            state16.ForeColor = Color.Black;
                        }));
                    }
                    if ((DateTime)StopTime[16] != DateTime.MinValue && (TimeSpan)RunTime[16] != TimeSpan.MinValue)
                    {
                        this.Invoke(new Action(() =>
                        {
                            exit16.Text = string.Format("{0:T}", StopTime[16]);
                            TimeSpan work = (TimeSpan)RunTime[16];
                            work16.Text = work.ToString(@"hh\:mm\:ss");
                        }));
                    }
                }
                //第17行数据
                if (prnum > 17)
                {
                    this.Invoke(new Action(() =>
                    {
                        pid17.Text = ProcID[17].ToString();
                        join17.Text = string.Format("{0:T}", StartTime[17]);
                        qqnum17.Text = QQid[17].ToString();
                        state17.Text = ProcState[17].ToString();
                    }));
                    st = state17.Text;
                    if (st == strexit | st == strdrop)
                    {
                        this.Invoke(new Action(() =>
                        {
                            state17.ForeColor = Color.Red;
                        }));
                    }
                    else
                    {
                        this.Invoke(new Action(() =>
                        {
                            state17.ForeColor = Color.Black;
                        }));
                    }
                    if ((DateTime)StopTime[17] != DateTime.MinValue && (TimeSpan)RunTime[17] != TimeSpan.MinValue)
                    {
                        this.Invoke(new Action(() =>
                        {
                            exit17.Text = string.Format("{0:T}", StopTime[17]);
                            TimeSpan work = (TimeSpan)RunTime[17];
                            work17.Text = work.ToString(@"hh\:mm\:ss");
                        }));
                    }
                }
                //第18行数据
                if (prnum > 18)
                {
                    this.Invoke(new Action(() =>
                    {
                        pid18.Text = ProcID[18].ToString();
                        join18.Text = string.Format("{0:T}", StartTime[18]);
                        qqnum18.Text = QQid[18].ToString();
                        state18.Text = ProcState[18].ToString();
                    }));
                    st = state18.Text;
                    if (st == strexit | st == strdrop)
                    {
                        this.Invoke(new Action(() =>
                        {
                            state18.ForeColor = Color.Red;
                        }));
                    }
                    else
                    {
                        this.Invoke(new Action(() =>
                        {
                            state18.ForeColor = Color.Black;
                        }));
                    }
                    if ((DateTime)StopTime[18] != DateTime.MinValue && (TimeSpan)RunTime[18] != TimeSpan.MinValue)
                    {
                        this.Invoke(new Action(() =>
                        {
                            exit18.Text = string.Format("{0:T}", StopTime[18]);
                            TimeSpan work = (TimeSpan)RunTime[18];
                            work18.Text = work.ToString(@"hh\:mm\:ss");
                        }));
                    }
                }
                //第19行数据
                if (prnum > 19)
                {
                    this.Invoke(new Action(() =>
                    {
                        pid19.Text = ProcID[19].ToString();
                        join19.Text = string.Format("{0:T}", StartTime[19]);
                        qqnum19.Text = QQid[19].ToString();
                        state19.Text = ProcState[19].ToString();
                    }));
                    st = state19.Text;
                    if (st == strexit | st == strdrop)
                    {
                        this.Invoke(new Action(() =>
                        {
                            state19.ForeColor = Color.Red;
                        }));
                    }
                    else
                    {
                        this.Invoke(new Action(() =>
                        {
                            state19.ForeColor = Color.Black;
                        }));
                    }
                    if ((DateTime)StopTime[19] != DateTime.MinValue && (TimeSpan)RunTime[19] != TimeSpan.MinValue)
                    {
                        this.Invoke(new Action(() =>
                        {
                            exit19.Text = string.Format("{0:T}", StopTime[19]);
                            TimeSpan work = (TimeSpan)RunTime[19];
                            work19.Text = work.ToString(@"hh\:mm\:ss");
                        }));
                    }
                }
                //第20行数据
                if (prnum > 20)
                {
                    this.Invoke(new Action(() =>
                    {
                        pid20.Text = ProcID[20].ToString();
                        join20.Text = string.Format("{0:T}", StartTime[20]);
                        qqnum20.Text = QQid[20].ToString();
                        state20.Text = ProcState[20].ToString();
                    }));
                    st = state20.Text;
                    if (st == strexit | st == strdrop)
                    {
                        this.Invoke(new Action(() =>
                        {
                            state20.ForeColor = Color.Red;
                        }));
                    }
                    else
                    {
                        this.Invoke(new Action(() =>
                        {
                            state20.ForeColor = Color.Black;
                        }));
                    }
                    if ((DateTime)StopTime[20] != DateTime.MinValue && (TimeSpan)RunTime[20] != TimeSpan.MinValue)
                    {
                        this.Invoke(new Action(() =>
                        {
                            exit20.Text = string.Format("{0:T}", StopTime[20]);
                            TimeSpan work = (TimeSpan)RunTime[20];
                            work20.Text = work.ToString(@"hh\:mm\:ss");
                        }));
                    }
                }
                //第21行数据
                if (prnum > 21)
                {
                    this.Invoke(new Action(() =>
                    {
                        pid21.Text = ProcID[21].ToString();
                        join21.Text = string.Format("{0:T}", StartTime[21]);
                        qqnum21.Text = QQid[21].ToString();
                        state21.Text = ProcState[21].ToString();
                    }));
                    st = state21.Text;
                    if (st == strexit | st == strdrop)
                    {
                        this.Invoke(new Action(() =>
                        {
                            state21.ForeColor = Color.Red;
                        }));
                    }
                    else
                    {
                        this.Invoke(new Action(() =>
                        {
                            state21.ForeColor = Color.Black;
                        }));
                    }
                    if ((DateTime)StopTime[21] != DateTime.MinValue && (TimeSpan)RunTime[21] != TimeSpan.MinValue)
                    {
                        this.Invoke(new Action(() =>
                        {
                            exit21.Text = string.Format("{0:T}", StopTime[21]);
                            TimeSpan work = (TimeSpan)RunTime[21];
                            work21.Text = work.ToString(@"hh\:mm\:ss");
                        }));
                    }
                }
                //第22行数据
                if (prnum > 22)
                {
                    this.Invoke(new Action(() =>
                    {
                        pid22.Text = ProcID[22].ToString();
                        join22.Text = string.Format("{0:T}", StartTime[22]);
                        qqnum22.Text = QQid[22].ToString();
                        state22.Text = ProcState[22].ToString();
                    }));
                    st = state22.Text;
                    if (st == strexit | st == strdrop)
                    {
                        this.Invoke(new Action(() =>
                        {
                            state22.ForeColor = Color.Red;
                        }));
                    }
                    else
                    {
                        this.Invoke(new Action(() =>
                        {
                            state22.ForeColor = Color.Black;
                        }));
                    }
                    if ((DateTime)StopTime[22] != DateTime.MinValue && (TimeSpan)RunTime[22] != TimeSpan.MinValue)
                    {
                        this.Invoke(new Action(() =>
                        {
                            exit22.Text = string.Format("{0:T}", StopTime[22]);
                            TimeSpan work = (TimeSpan)RunTime[22];
                            work22.Text = work.ToString(@"hh\:mm\:ss");
                        }));
                    }
                }
                //第23行数据
                if (prnum > 23)
                {
                    this.Invoke(new Action(() =>
                    {
                        pid23.Text = ProcID[23].ToString();
                        join23.Text = string.Format("{0:T}", StartTime[23]);
                        qqnum23.Text = QQid[23].ToString();
                        state23.Text = ProcState[23].ToString();
                    }));
                    st = state23.Text;
                    if (st == strexit | st == strdrop)
                    {
                        this.Invoke(new Action(() =>
                        {
                            state23.ForeColor = Color.Red;
                        }));
                    }
                    else
                    {
                        this.Invoke(new Action(() =>
                        {
                            state23.ForeColor = Color.Black;
                        }));
                    }
                    if ((DateTime)StopTime[23] != DateTime.MinValue && (TimeSpan)RunTime[23] != TimeSpan.MinValue)
                    {
                        this.Invoke(new Action(() =>
                        {
                            exit23.Text = string.Format("{0:T}", StopTime[23]);
                            TimeSpan work = (TimeSpan)RunTime[23];
                            work23.Text = work.ToString(@"hh\:mm\:ss");
                        }));
                    }
                }
                //第24行数据
                if (prnum > 24)
                {
                    this.Invoke(new Action(() =>
                    {
                        pid24.Text = ProcID[24].ToString();
                        join24.Text = string.Format("{0:T}", StartTime[24]);
                        qqnum24.Text = QQid[24].ToString();
                        state24.Text = ProcState[24].ToString();
                    }));
                    st = state24.Text;
                    if (st == strexit | st == strdrop)
                    {
                        this.Invoke(new Action(() =>
                        {
                            state24.ForeColor = Color.Red;
                        }));
                    }
                    else
                    {
                        this.Invoke(new Action(() =>
                        {
                            state24.ForeColor = Color.Black;
                        }));
                    }
                    if ((DateTime)StopTime[24] != DateTime.MinValue && (TimeSpan)RunTime[24] != TimeSpan.MinValue)
                    {
                        this.Invoke(new Action(() =>
                        {
                            exit24.Text = string.Format("{0:T}", StopTime[24]);
                            TimeSpan work = (TimeSpan)RunTime[24];
                            work24.Text = work.ToString(@"hh\:mm\:ss");
                        }));
                    }
                }
                //第25行数据
                if (prnum > 25)
                {
                    this.Invoke(new Action(() =>
                    {
                        pid25.Text = ProcID[25].ToString();
                        join25.Text = string.Format("{0:T}", StartTime[25]);
                        qqnum25.Text = QQid[25].ToString();
                        state25.Text = ProcState[25].ToString();
                    }));
                    st = state25.Text;
                    if (st == strexit | st == strdrop)
                    {
                        this.Invoke(new Action(() =>
                        {
                            state25.ForeColor = Color.Red;
                        }));
                    }
                    else
                    {
                        this.Invoke(new Action(() =>
                        {
                            state25.ForeColor = Color.Black;
                        }));
                    }
                    if ((DateTime)StopTime[25] != DateTime.MinValue && (TimeSpan)RunTime[25] != TimeSpan.MinValue)
                    {
                        this.Invoke(new Action(() =>
                        {
                            exit25.Text = string.Format("{0:T}", StopTime[25]);
                            TimeSpan work = (TimeSpan)RunTime[25];
                            work25.Text = work.ToString(@"hh\:mm\:ss");
                        }));
                    }
                }
                //第26行数据
                if (prnum > 26)
                {
                    this.Invoke(new Action(() =>
                    {
                        pid26.Text = ProcID[26].ToString();
                        join26.Text = string.Format("{0:T}", StartTime[26]);
                        qqnum26.Text = QQid[26].ToString();
                        state26.Text = ProcState[26].ToString();
                    }));
                    st = state26.Text;
                    if (st == strexit | st == strdrop)
                    {
                        this.Invoke(new Action(() =>
                        {
                            state26.ForeColor = Color.Red;
                        }));
                    }
                    else
                    {
                        this.Invoke(new Action(() =>
                        {
                            state26.ForeColor = Color.Black;
                        }));
                    }
                    if ((DateTime)StopTime[26] != DateTime.MinValue && (TimeSpan)RunTime[26] != TimeSpan.MinValue)
                    {
                        this.Invoke(new Action(() =>
                        {
                            exit26.Text = string.Format("{0:T}", StopTime[26]);
                            TimeSpan work = (TimeSpan)RunTime[26];
                            work26.Text = work.ToString(@"hh\:mm\:ss");
                        }));
                    }
                }
                //第27行数据
                if (prnum > 27)
                {
                    this.Invoke(new Action(() =>
                    {
                        pid27.Text = ProcID[27].ToString();
                        join27.Text = string.Format("{0:T}", StartTime[27]);
                        qqnum27.Text = QQid[27].ToString();
                        state27.Text = ProcState[27].ToString();
                    }));
                    st = state27.Text;
                    if (st == strexit | st == strdrop)
                    {
                        this.Invoke(new Action(() =>
                        {
                            state27.ForeColor = Color.Red;
                        }));
                    }
                    else
                    {
                        this.Invoke(new Action(() =>
                        {
                            state27.ForeColor = Color.Black;
                        }));
                    }
                    if ((DateTime)StopTime[27] != DateTime.MinValue && (TimeSpan)RunTime[27] != TimeSpan.MinValue)
                    {
                        this.Invoke(new Action(() =>
                        {
                            exit27.Text = string.Format("{0:T}", StopTime[27]);
                            TimeSpan work = (TimeSpan)RunTime[27];
                            work27.Text = work.ToString(@"hh\:mm\:ss");
                        }));
                    }
                }
                //第28行数据
                if (prnum > 28)
                {
                    this.Invoke(new Action(() =>
                    {
                        pid28.Text = ProcID[28].ToString();
                        join28.Text = string.Format("{0:T}", StartTime[28]);
                        qqnum28.Text = QQid[28].ToString();
                        state28.Text = ProcState[28].ToString();
                    }));
                    st = state28.Text;
                    if (st == strexit | st == strdrop)
                    {
                        this.Invoke(new Action(() =>
                        {
                            state28.ForeColor = Color.Red;
                        }));
                    }
                    else
                    {
                        this.Invoke(new Action(() =>
                        {
                            state28.ForeColor = Color.Black;
                        }));
                    }
                    if ((DateTime)StopTime[28] != DateTime.MinValue && (TimeSpan)RunTime[28] != TimeSpan.MinValue)
                    {
                        this.Invoke(new Action(() =>
                        {
                            exit28.Text = string.Format("{0:T}", StopTime[28]);
                            TimeSpan work = (TimeSpan)RunTime[28];
                            work28.Text = work.ToString(@"hh\:mm\:ss");
                        }));
                    }
                }
                //第29行数据
                if (prnum > 29)
                {
                    this.Invoke(new Action(() =>
                    {
                        pid29.Text = ProcID[29].ToString();
                        join29.Text = string.Format("{0:T}", StartTime[29]);
                        qqnum29.Text = QQid[29].ToString();
                        state29.Text = ProcState[29].ToString();
                    }));
                    st = state29.Text;
                    if (st == strexit | st == strdrop)
                    {
                        this.Invoke(new Action(() =>
                        {
                            state29.ForeColor = Color.Red;
                        }));
                    }
                    else
                    {
                        this.Invoke(new Action(() =>
                        {
                            state29.ForeColor = Color.Black;
                        }));
                    }
                    if ((DateTime)StopTime[29] != DateTime.MinValue && (TimeSpan)RunTime[29] != TimeSpan.MinValue)
                    {
                        this.Invoke(new Action(() =>
                        {
                            exit29.Text = string.Format("{0:T}", StopTime[29]);
                            TimeSpan work = (TimeSpan)RunTime[29];
                            work29.Text = work.ToString(@"hh\:mm\:ss");
                        }));
                    }
                }
            }
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
            ProcID.Add("进程ID");
            StartTime.Add("加入时间");
            StopTime.Add("掉线时间");
            RunTime.Add("运行时长");
            QQid.Add("游戏账号");
            ProcState.Add("状态");
            pid0.Text = ProcID[0].ToString();
            join0.Text = StartTime[0].ToString();
            exit0.Text = StopTime[0].ToString();
            work0.Text = RunTime[0].ToString();
            qqnum0.Text = QQid[0].ToString();
            state0.Text = ProcState[0].ToString();
        }
        private void ViewClear()
        {
            string d = "-";
            pid1.Text = d;
            join1.Text = d;
            exit1.Text = d;
            work1.Text = d;
            qqnum1.Text = d;
            state1.Text = d;
            pid2.Text = d;
            join2.Text = d;
            exit2.Text = d;
            work2.Text = d;
            qqnum2.Text = d;
            state2.Text = d;
            pid3.Text = d;
            join3.Text = d;
            exit3.Text = d;
            work3.Text = d;
            qqnum3.Text = d;
            state3.Text = d;
            pid4.Text = d;
            join4.Text = d;
            exit4.Text = d;
            work4.Text = d;
            qqnum4.Text = d;
            state4.Text = d;
            pid5.Text = d;
            join5.Text = d;
            exit5.Text = d;
            work5.Text = d;
            qqnum5.Text = d;
            state5.Text = d;
            pid6.Text = d;
            join6.Text = d;
            exit6.Text = d;
            work6.Text = d;
            qqnum6.Text = d;
            state6.Text = d;
            pid7.Text = d;
            join7.Text = d;
            exit7.Text = d;
            work7.Text = d;
            qqnum7.Text = d;
            state7.Text = d;
            pid8.Text = d;
            join8.Text = d;
            exit8.Text = d;
            work8.Text = d;
            qqnum8.Text = d;
            state8.Text = d;
            pid9.Text = d;
            join9.Text = d;
            exit9.Text = d;
            work9.Text = d;
            qqnum9.Text = d;
            state9.Text = d;
            pid10.Text = d;
            join10.Text = d;
            exit10.Text = d;
            work10.Text = d;
            qqnum10.Text = d;
            state10.Text = d;
            pid11.Text = d;
            join11.Text = d;
            exit11.Text = d;
            work11.Text = d;
            qqnum11.Text = d;
            state11.Text = d;
            pid12.Text = d;
            join12.Text = d;
            exit12.Text = d;
            work12.Text = d;
            qqnum12.Text = d;
            state12.Text = d;
            pid13.Text = d;
            join13.Text = d;
            exit13.Text = d;
            work13.Text = d;
            qqnum13.Text = d;
            state13.Text = d;
            pid14.Text = d;
            join14.Text = d;
            exit14.Text = d;
            work14.Text = d;
            qqnum14.Text = d;
            state14.Text = d;
            pid15.Text = d;
            join15.Text = d;
            exit15.Text = d;
            work15.Text = d;
            qqnum15.Text = d;
            state15.Text = d;
            pid16.Text = d;
            join16.Text = d;
            exit16.Text = d;
            work16.Text = d;
            qqnum16.Text = d;
            state16.Text = d;
            pid17.Text = d;
            join17.Text = d;
            exit17.Text = d;
            work17.Text = d;
            qqnum17.Text = d;
            state17.Text = d;
            pid18.Text = d;
            join18.Text = d;
            exit18.Text = d;
            work18.Text = d;
            qqnum18.Text = d;
            state18.Text = d;
            pid19.Text = d;
            join19.Text = d;
            exit19.Text = d;
            work19.Text = d;
            qqnum19.Text = d;
            state19.Text = d;
            pid20.Text = d;
            join20.Text = d;
            exit20.Text = d;
            work20.Text = d;
            qqnum20.Text = d;
            state20.Text = d;
            pid21.Text = d;
            join21.Text = d;
            exit21.Text = d;
            work21.Text = d;
            qqnum21.Text = d;
            state21.Text = d;
            pid22.Text = d;
            join22.Text = d;
            exit22.Text = d;
            work22.Text = d;
            qqnum22.Text = d;
            state22.Text = d;
            pid23.Text = d;
            join23.Text = d;
            exit23.Text = d;
            work23.Text = d;
            qqnum23.Text = d;
            state23.Text = d;
            pid24.Text = d;
            join24.Text = d;
            exit24.Text = d;
            work24.Text = d;
            qqnum24.Text = d;
            state24.Text = d;
            pid25.Text = d;
            join25.Text = d;
            exit25.Text = d;
            work25.Text = d;
            qqnum25.Text = d;
            state25.Text = d;
            pid26.Text = d;
            join26.Text = d;
            exit26.Text = d;
            work26.Text = d;
            qqnum26.Text = d;
            state26.Text = d;
            pid27.Text = d;
            join27.Text = d;
            exit27.Text = d;
            work27.Text = d;
            qqnum27.Text = d;
            state27.Text = d;
            pid28.Text = d;
            join28.Text = d;
            exit28.Text = d;
            work28.Text = d;
            qqnum28.Text = d;
            state28.Text = d;
            pid29.Text = d;
            join29.Text = d;
            exit29.Text = d;
            work29.Text = d;
            qqnum29.Text = d;
            state29.Text = d;
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
            WMP.Ctlcontrols.stop();
            PauseMusic.BackColor = SystemColors.Control;
            PauseMusic.UseVisualStyleBackColor = true;
            playstate = false;
            Playing = false;
        }
        private void ListClear_Click(object sender, EventArgs e)
        {
            ListClear.Enabled = false;
            暂停处理数据 = true;
            bool lockwait = true;
            do
            {
                if (Lock1 && Lock2)
                {
                    lockwait = false;
                }
            } while (lockwait);
            ListInit();
            ViewClear();
            暂停处理数据 = false;
            ListClear.Enabled = true;
        }
        #endregion

        private void link教学视频_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://space.bilibili.com/1177161017");
        }

        private void link作者博客_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://blog.csdn.net/zuoyuanchun");
        }

        private void link个人博客_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://2950800.ysepan.com/");
        }
    }
}
