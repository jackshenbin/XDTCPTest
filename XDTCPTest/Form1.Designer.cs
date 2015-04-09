namespace XDTCPTest
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
            this.buttonConnect = new System.Windows.Forms.Button();
            this.textBoxIP = new System.Windows.Forms.TextBox();
            this.textBoxPort = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonSendLogin = new System.Windows.Forms.Button();
            this.buttonSendSubscribeDevStatus = new System.Windows.Forms.Button();
            this.buttonSendSubscribeDevChargeStatus = new System.Windows.Forms.Button();
            this.buttonSendGetDevChargeInfo = new System.Windows.Forms.Button();
            this.buttonSendGetDevVersion = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.buttonDisConnect = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(333, 12);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(75, 23);
            this.buttonConnect.TabIndex = 0;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // textBoxIP
            // 
            this.textBoxIP.Location = new System.Drawing.Point(70, 12);
            this.textBoxIP.Name = "textBoxIP";
            this.textBoxIP.Size = new System.Drawing.Size(100, 21);
            this.textBoxIP.TabIndex = 1;
            this.textBoxIP.Text = "127.0.0.1";
            // 
            // textBoxPort
            // 
            this.textBoxPort.Location = new System.Drawing.Point(227, 14);
            this.textBoxPort.Name = "textBoxPort";
            this.textBoxPort.Size = new System.Drawing.Size(100, 21);
            this.textBoxPort.TabIndex = 1;
            this.textBoxPort.Text = "9999";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(186, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "Port:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(41, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "IP:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "Receive:";
            // 
            // buttonSendLogin
            // 
            this.buttonSendLogin.Location = new System.Drawing.Point(70, 39);
            this.buttonSendLogin.Name = "buttonSendLogin";
            this.buttonSendLogin.Size = new System.Drawing.Size(75, 23);
            this.buttonSendLogin.TabIndex = 5;
            this.buttonSendLogin.Text = "用户登录";
            this.buttonSendLogin.UseVisualStyleBackColor = true;
            this.buttonSendLogin.Click += new System.EventHandler(this.buttonLogin_Click);
            // 
            // buttonSendSubscribeDevStatus
            // 
            this.buttonSendSubscribeDevStatus.Location = new System.Drawing.Point(151, 39);
            this.buttonSendSubscribeDevStatus.Name = "buttonSendSubscribeDevStatus";
            this.buttonSendSubscribeDevStatus.Size = new System.Drawing.Size(118, 23);
            this.buttonSendSubscribeDevStatus.TabIndex = 5;
            this.buttonSendSubscribeDevStatus.Text = "定制状态信息命令";
            this.buttonSendSubscribeDevStatus.UseVisualStyleBackColor = true;
            this.buttonSendSubscribeDevStatus.Click += new System.EventHandler(this.buttonSendSubscribeDevStatus_Click);
            // 
            // buttonSendSubscribeDevChargeStatus
            // 
            this.buttonSendSubscribeDevChargeStatus.Location = new System.Drawing.Point(275, 39);
            this.buttonSendSubscribeDevChargeStatus.Name = "buttonSendSubscribeDevChargeStatus";
            this.buttonSendSubscribeDevChargeStatus.Size = new System.Drawing.Size(150, 23);
            this.buttonSendSubscribeDevChargeStatus.TabIndex = 5;
            this.buttonSendSubscribeDevChargeStatus.Text = "定制充电实时监测数据";
            this.buttonSendSubscribeDevChargeStatus.UseVisualStyleBackColor = true;
            this.buttonSendSubscribeDevChargeStatus.Click += new System.EventHandler(this.buttonSendSubscribeDevChargeStatus_Click);
            // 
            // buttonSendGetDevChargeInfo
            // 
            this.buttonSendGetDevChargeInfo.Location = new System.Drawing.Point(432, 39);
            this.buttonSendGetDevChargeInfo.Name = "buttonSendGetDevChargeInfo";
            this.buttonSendGetDevChargeInfo.Size = new System.Drawing.Size(135, 23);
            this.buttonSendGetDevChargeInfo.TabIndex = 5;
            this.buttonSendGetDevChargeInfo.Text = "获取充电桩充电信息";
            this.buttonSendGetDevChargeInfo.UseVisualStyleBackColor = true;
            this.buttonSendGetDevChargeInfo.Click += new System.EventHandler(this.buttonSendGetDevChargeInfo_Click);
            // 
            // buttonSendGetDevVersion
            // 
            this.buttonSendGetDevVersion.Location = new System.Drawing.Point(573, 39);
            this.buttonSendGetDevVersion.Name = "buttonSendGetDevVersion";
            this.buttonSendGetDevVersion.Size = new System.Drawing.Size(129, 23);
            this.buttonSendGetDevVersion.TabIndex = 5;
            this.buttonSendGetDevVersion.Text = "获取充电桩软件版本";
            this.buttonSendGetDevVersion.UseVisualStyleBackColor = true;
            this.buttonSendGetDevVersion.Click += new System.EventHandler(this.buttonSendGetDevVersion_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(70, 68);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(695, 424);
            this.listBox1.TabIndex = 7;
            // 
            // buttonDisConnect
            // 
            this.buttonDisConnect.Location = new System.Drawing.Point(414, 12);
            this.buttonDisConnect.Name = "buttonDisConnect";
            this.buttonDisConnect.Size = new System.Drawing.Size(86, 23);
            this.buttonDisConnect.TabIndex = 0;
            this.buttonDisConnect.Text = "DisConnect";
            this.buttonDisConnect.UseVisualStyleBackColor = true;
            this.buttonDisConnect.Click += new System.EventHandler(this.buttonDisConnect_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(777, 509);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.buttonSendGetDevVersion);
            this.Controls.Add(this.buttonSendGetDevChargeInfo);
            this.Controls.Add(this.buttonSendSubscribeDevChargeStatus);
            this.Controls.Add(this.buttonSendSubscribeDevStatus);
            this.Controls.Add(this.buttonSendLogin);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxPort);
            this.Controls.Add(this.textBoxIP);
            this.Controls.Add(this.buttonDisConnect);
            this.Controls.Add(this.buttonConnect);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.TextBox textBoxIP;
        private System.Windows.Forms.TextBox textBoxPort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonSendLogin;
        private System.Windows.Forms.Button buttonSendSubscribeDevStatus;
        private System.Windows.Forms.Button buttonSendSubscribeDevChargeStatus;
        private System.Windows.Forms.Button buttonSendGetDevChargeInfo;
        private System.Windows.Forms.Button buttonSendGetDevVersion;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button buttonDisConnect;
    }
}
