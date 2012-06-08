namespace BroadcastDesktop
{
    partial class Form1
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
            this.btnListen = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.cmbIP = new System.Windows.Forms.ComboBox();
            this.nupPort = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.txturl = new System.Windows.Forms.TextBox();
            this.nupFPS = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nupPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupFPS)).BeginInit();
            this.SuspendLayout();
            // 
            // btnListen
            // 
            this.btnListen.Location = new System.Drawing.Point(56, 214);
            this.btnListen.Name = "btnListen";
            this.btnListen.Size = new System.Drawing.Size(75, 23);
            this.btnListen.TabIndex = 0;
            this.btnListen.Text = "开始";
            this.btnListen.UseVisualStyleBackColor = true;
            this.btnListen.Click += new System.EventHandler(this.btnListen_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(170, 214);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 1;
            this.btnStop.Text = "停止";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // cmbIP
            // 
            this.cmbIP.FormattingEnabled = true;
            this.cmbIP.Location = new System.Drawing.Point(12, 12);
            this.cmbIP.Name = "cmbIP";
            this.cmbIP.Size = new System.Drawing.Size(167, 20);
            this.cmbIP.TabIndex = 2;
            // 
            // nupPort
            // 
            this.nupPort.Location = new System.Drawing.Point(185, 11);
            this.nupPort.Maximum = new decimal(new int[] {
            -1530494977,
            232830,
            0,
            0});
            this.nupPort.Name = "nupPort";
            this.nupPort.Size = new System.Drawing.Size(89, 21);
            this.nupPort.TabIndex = 3;
            this.nupPort.Value = new decimal(new int[] {
            8080,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 85);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "您的地址为：";
            // 
            // txturl
            // 
            this.txturl.Location = new System.Drawing.Point(14, 100);
            this.txturl.Name = "txturl";
            this.txturl.ReadOnly = true;
            this.txturl.Size = new System.Drawing.Size(260, 21);
            this.txturl.TabIndex = 6;
            // 
            // nupFPS
            // 
            this.nupFPS.Location = new System.Drawing.Point(14, 38);
            this.nupFPS.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nupFPS.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nupFPS.Name = "nupFPS";
            this.nupFPS.Size = new System.Drawing.Size(89, 21);
            this.nupFPS.TabIndex = 7;
            this.nupFPS.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(106, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "帧/秒";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.nupFPS);
            this.Controls.Add(this.txturl);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nupPort);
            this.Controls.Add(this.cmbIP);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnListen);
            this.Name = "Form1";
            this.Text = "桌面共享";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.nupPort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupFPS)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnListen;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.ComboBox cmbIP;
        private System.Windows.Forms.NumericUpDown nupPort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txturl;
        private System.Windows.Forms.NumericUpDown nupFPS;
        private System.Windows.Forms.Label label2;
    }
}

