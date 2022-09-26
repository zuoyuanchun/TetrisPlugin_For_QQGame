namespace TetrisMonitor
{
    partial class Main
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.groupbutton = new System.Windows.Forms.GroupBox();
            this.shijian = new System.Windows.Forms.Label();
            this.qqlist = new System.Windows.Forms.ListBox();
            this.link检查更新 = new System.Windows.Forms.LinkLabel();
            this.link教学视频 = new System.Windows.Forms.LinkLabel();
            this.WindowTop = new System.Windows.Forms.CheckBox();
            this.MusicCycle = new System.Windows.Forms.CheckBox();
            this.PauseMusic = new System.Windows.Forms.Button();
            this.WMP = new AxWMPLib.AxWindowsMediaPlayer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.loglist = new System.Windows.Forms.RichTextBox();
            this.托盘 = new System.Windows.Forms.NotifyIcon(this.components);
            this.groupbutton.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.WMP)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupbutton
            // 
            this.groupbutton.Controls.Add(this.shijian);
            this.groupbutton.Controls.Add(this.qqlist);
            this.groupbutton.Controls.Add(this.link检查更新);
            this.groupbutton.Controls.Add(this.link教学视频);
            this.groupbutton.Controls.Add(this.WindowTop);
            this.groupbutton.Controls.Add(this.MusicCycle);
            this.groupbutton.Controls.Add(this.PauseMusic);
            this.groupbutton.Controls.Add(this.WMP);
            this.groupbutton.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupbutton.Location = new System.Drawing.Point(620, 1);
            this.groupbutton.Margin = new System.Windows.Forms.Padding(4);
            this.groupbutton.Name = "groupbutton";
            this.groupbutton.Padding = new System.Windows.Forms.Padding(4);
            this.groupbutton.Size = new System.Drawing.Size(149, 541);
            this.groupbutton.TabIndex = 0;
            this.groupbutton.TabStop = false;
            this.groupbutton.Text = "状态信息";
            // 
            // shijian
            // 
            this.shijian.Location = new System.Drawing.Point(7, 488);
            this.shijian.Name = "shijian";
            this.shijian.Size = new System.Drawing.Size(135, 49);
            this.shijian.TabIndex = 19;
            this.shijian.Text = "日期\r\n时间";
            this.shijian.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // qqlist
            // 
            this.qqlist.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.qqlist.FormattingEnabled = true;
            this.qqlist.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.qqlist.ItemHeight = 20;
            this.qqlist.Location = new System.Drawing.Point(-1, 30);
            this.qqlist.Name = "qqlist";
            this.qqlist.Size = new System.Drawing.Size(148, 284);
            this.qqlist.TabIndex = 18;
            this.qqlist.TabStop = false;
            // 
            // link检查更新
            // 
            this.link检查更新.AutoSize = true;
            this.link检查更新.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.link检查更新.LinkColor = System.Drawing.Color.Purple;
            this.link检查更新.Location = new System.Drawing.Point(81, 431);
            this.link检查更新.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.link检查更新.Name = "link检查更新";
            this.link检查更新.Size = new System.Drawing.Size(49, 20);
            this.link检查更新.TabIndex = 15;
            this.link检查更新.TabStop = true;
            this.link检查更新.Text = "更新";
            this.link检查更新.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.link检查更新.VisitedLinkColor = System.Drawing.Color.Black;
            this.link检查更新.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.link检查更新_LinkClicked);
            // 
            // link教学视频
            // 
            this.link教学视频.AutoSize = true;
            this.link教学视频.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.link教学视频.Location = new System.Drawing.Point(15, 431);
            this.link教学视频.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.link教学视频.Name = "link教学视频";
            this.link教学视频.Size = new System.Drawing.Size(49, 20);
            this.link教学视频.TabIndex = 14;
            this.link教学视频.TabStop = true;
            this.link教学视频.Text = "教学";
            this.link教学视频.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.link教学视频.VisitedLinkColor = System.Drawing.Color.Black;
            this.link教学视频.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.link教学视频_LinkClicked);
            // 
            // WindowTop
            // 
            this.WindowTop.AutoSize = true;
            this.WindowTop.ForeColor = System.Drawing.SystemColors.ControlText;
            this.WindowTop.Location = new System.Drawing.Point(19, 403);
            this.WindowTop.Margin = new System.Windows.Forms.Padding(4);
            this.WindowTop.Name = "WindowTop";
            this.WindowTop.Size = new System.Drawing.Size(111, 24);
            this.WindowTop.TabIndex = 11;
            this.WindowTop.Text = "窗口置顶";
            this.WindowTop.UseVisualStyleBackColor = true;
            this.WindowTop.CheckedChanged += new System.EventHandler(this.WindowTop_CheckedChanged);
            // 
            // MusicCycle
            // 
            this.MusicCycle.AutoSize = true;
            this.MusicCycle.Checked = true;
            this.MusicCycle.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MusicCycle.ForeColor = System.Drawing.SystemColors.ControlText;
            this.MusicCycle.Location = new System.Drawing.Point(19, 371);
            this.MusicCycle.Margin = new System.Windows.Forms.Padding(4);
            this.MusicCycle.Name = "MusicCycle";
            this.MusicCycle.Size = new System.Drawing.Size(111, 24);
            this.MusicCycle.TabIndex = 10;
            this.MusicCycle.Text = "循环播放";
            this.MusicCycle.UseVisualStyleBackColor = true;
            // 
            // PauseMusic
            // 
            this.PauseMusic.BackColor = System.Drawing.SystemColors.Control;
            this.PauseMusic.ForeColor = System.Drawing.Color.Black;
            this.PauseMusic.Location = new System.Drawing.Point(19, 321);
            this.PauseMusic.Margin = new System.Windows.Forms.Padding(4);
            this.PauseMusic.Name = "PauseMusic";
            this.PauseMusic.Size = new System.Drawing.Size(111, 42);
            this.PauseMusic.TabIndex = 12;
            this.PauseMusic.Text = "停止报警";
            this.PauseMusic.UseVisualStyleBackColor = true;
            this.PauseMusic.Click += new System.EventHandler(this.PauseMusic_Click);
            // 
            // WMP
            // 
            this.WMP.Enabled = true;
            this.WMP.Location = new System.Drawing.Point(63, 428);
            this.WMP.Margin = new System.Windows.Forms.Padding(4);
            this.WMP.Name = "WMP";
            this.WMP.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("WMP.OcxState")));
            this.WMP.Size = new System.Drawing.Size(20, 23);
            this.WMP.TabIndex = 17;
            this.WMP.TabStop = false;
            this.WMP.Visible = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.loglist);
            this.groupBox1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(3, 1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(610, 542);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "日志明细";
            // 
            // loglist
            // 
            this.loglist.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.loglist.Dock = System.Windows.Forms.DockStyle.Fill;
            this.loglist.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.loglist.Location = new System.Drawing.Point(3, 26);
            this.loglist.Name = "loglist";
            this.loglist.ReadOnly = true;
            this.loglist.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.loglist.Size = new System.Drawing.Size(604, 513);
            this.loglist.TabIndex = 0;
            this.loglist.TabStop = false;
            this.loglist.Text = "";
            // 
            // 托盘
            // 
            this.托盘.Icon = ((System.Drawing.Icon)(resources.GetObject("托盘.Icon")));
            this.托盘.Text = "火拼俄罗斯掉线监控";
            this.托盘.Visible = true;
            this.托盘.MouseClick += new System.Windows.Forms.MouseEventHandler(this.托盘_MouseClick);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(782, 555);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupbutton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "Main";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MainForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.Load += new System.EventHandler(this.Main_Load);
            this.SizeChanged += new System.EventHandler(this.Main_SizeChanged);
            this.groupbutton.ResumeLayout(false);
            this.groupbutton.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.WMP)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupbutton;
        private AxWMPLib.AxWindowsMediaPlayer WMP;
        private System.Windows.Forms.LinkLabel link检查更新;
        private System.Windows.Forms.LinkLabel link教学视频;
        private System.Windows.Forms.CheckBox WindowTop;
        private System.Windows.Forms.CheckBox MusicCycle;
        private System.Windows.Forms.Button PauseMusic;
        private System.Windows.Forms.ListBox qqlist;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RichTextBox loglist;
        private System.Windows.Forms.Label shijian;
        private System.Windows.Forms.NotifyIcon 托盘;

    }
}

