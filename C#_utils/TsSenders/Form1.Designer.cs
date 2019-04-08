namespace TsSenders
{
    partial class s
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
            this.btnSelete = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtRate = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.cboIsCalcRate = new System.Windows.Forms.CheckBox();
            this.btnSendWithRate = new System.Windows.Forms.Button();
            this.txtSendCount = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnSelete
            // 
            this.btnSelete.Location = new System.Drawing.Point(357, 68);
            this.btnSelete.Name = "btnSelete";
            this.btnSelete.Size = new System.Drawing.Size(31, 23);
            this.btnSelete.TabIndex = 0;
            this.btnSelete.Text = "...";
            this.btnSelete.UseVisualStyleBackColor = true;
            this.btnSelete.Click += new System.EventHandler(this.btnSelete_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(42, 73);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "Path";
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(89, 70);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(262, 21);
            this.txtPath.TabIndex = 2;
            this.txtPath.Text = "F:\\MyGathering\\视频\\mv\\ice_age.ts";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(42, 110);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "Rate";
            // 
            // txtRate
            // 
            this.txtRate.Location = new System.Drawing.Point(89, 107);
            this.txtRate.Name = "txtRate";
            this.txtRate.Size = new System.Drawing.Size(100, 21);
            this.txtRate.TabIndex = 4;
            this.txtRate.Text = "1";
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(195, 105);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 5;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(276, 105);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 6;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(54, 41);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "IP";
            // 
            // txtIP
            // 
            this.txtIP.Location = new System.Drawing.Point(89, 38);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(100, 21);
            this.txtIP.TabIndex = 8;
            this.txtIP.Text = "224.2.2.2";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(195, 41);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 9;
            this.label4.Text = "Port";
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(230, 38);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(121, 21);
            this.txtPort.TabIndex = 10;
            this.txtPort.Text = "5006";
            // 
            // cboIsCalcRate
            // 
            this.cboIsCalcRate.AutoSize = true;
            this.cboIsCalcRate.Location = new System.Drawing.Point(89, 184);
            this.cboIsCalcRate.Name = "cboIsCalcRate";
            this.cboIsCalcRate.Size = new System.Drawing.Size(72, 16);
            this.cboIsCalcRate.TabIndex = 11;
            this.cboIsCalcRate.Text = "显示码率";
            this.cboIsCalcRate.UseVisualStyleBackColor = true;
            // 
            // btnSendWithRate
            // 
            this.btnSendWithRate.Location = new System.Drawing.Point(195, 142);
            this.btnSendWithRate.Name = "btnSendWithRate";
            this.btnSendWithRate.Size = new System.Drawing.Size(94, 23);
            this.btnSendWithRate.TabIndex = 12;
            this.btnSendWithRate.Text = "SendWithRate";
            this.btnSendWithRate.UseVisualStyleBackColor = true;
            this.btnSendWithRate.Click += new System.EventHandler(this.btnSendWithRate_Click);
            // 
            // txtSendCount
            // 
            this.txtSendCount.Location = new System.Drawing.Point(89, 144);
            this.txtSendCount.Name = "txtSendCount";
            this.txtSendCount.Size = new System.Drawing.Size(100, 21);
            this.txtSendCount.TabIndex = 13;
            // 
            // s
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(449, 212);
            this.Controls.Add(this.txtSendCount);
            this.Controls.Add(this.btnSendWithRate);
            this.Controls.Add(this.cboIsCalcRate);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtIP);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.txtRate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSelete);
            this.Name = "s";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSelete;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtRate;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.CheckBox cboIsCalcRate;
        private System.Windows.Forms.Button btnSendWithRate;
        private System.Windows.Forms.TextBox txtSendCount;
    }
}

