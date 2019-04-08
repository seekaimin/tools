namespace SendFilesApp
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtfile1 = new System.Windows.Forms.TextBox();
            this.txtfile2 = new System.Windows.Forms.TextBox();
            this.btnselectfile1 = new System.Windows.Forms.Button();
            this.btnselectfile2 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtfile3 = new System.Windows.Forms.TextBox();
            this.btnselectfile3 = new System.Windows.Forms.Button();
            this.btnstop = new System.Windows.Forms.Button();
            this.btnstart = new System.Windows.Forms.Button();
            this.cboloop = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtip = new System.Windows.Forms.TextBox();
            this.txtport = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtdefaultrate = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtfile1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtfile2, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnselectfile1, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnselectfile2, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtfile3, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnselectfile3, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnstart, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.btnstop, 2, 7);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.cboloop, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.txtip, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.txtport, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.txtdefaultrate, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.label6, 0, 6);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 8;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(456, 232);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "file1";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "file2";
            // 
            // txtfile1
            // 
            this.txtfile1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtfile1.Location = new System.Drawing.Point(44, 4);
            this.txtfile1.Name = "txtfile1";
            this.txtfile1.ReadOnly = true;
            this.txtfile1.Size = new System.Drawing.Size(300, 21);
            this.txtfile1.TabIndex = 2;
            // 
            // txtfile2
            // 
            this.txtfile2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtfile2.Location = new System.Drawing.Point(44, 33);
            this.txtfile2.Name = "txtfile2";
            this.txtfile2.ReadOnly = true;
            this.txtfile2.Size = new System.Drawing.Size(300, 21);
            this.txtfile2.TabIndex = 2;
            // 
            // btnselectfile1
            // 
            this.btnselectfile1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnselectfile1.Location = new System.Drawing.Point(350, 3);
            this.btnselectfile1.Name = "btnselectfile1";
            this.btnselectfile1.Size = new System.Drawing.Size(75, 23);
            this.btnselectfile1.TabIndex = 3;
            this.btnselectfile1.Text = "...";
            this.btnselectfile1.UseVisualStyleBackColor = true;
            this.btnselectfile1.Click += new System.EventHandler(this.btnselectfile1_Click);
            // 
            // btnselectfile2
            // 
            this.btnselectfile2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnselectfile2.Location = new System.Drawing.Point(350, 32);
            this.btnselectfile2.Name = "btnselectfile2";
            this.btnselectfile2.Size = new System.Drawing.Size(75, 23);
            this.btnselectfile2.TabIndex = 3;
            this.btnselectfile2.Text = "...";
            this.btnselectfile2.UseVisualStyleBackColor = true;
            this.btnselectfile2.Click += new System.EventHandler(this.btnselectfile2_Click);
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "file3";
            // 
            // txtfile3
            // 
            this.txtfile3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtfile3.Location = new System.Drawing.Point(44, 62);
            this.txtfile3.Name = "txtfile3";
            this.txtfile3.ReadOnly = true;
            this.txtfile3.Size = new System.Drawing.Size(300, 21);
            this.txtfile3.TabIndex = 5;
            // 
            // btnselectfile3
            // 
            this.btnselectfile3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnselectfile3.Location = new System.Drawing.Point(350, 61);
            this.btnselectfile3.Name = "btnselectfile3";
            this.btnselectfile3.Size = new System.Drawing.Size(75, 23);
            this.btnselectfile3.TabIndex = 6;
            this.btnselectfile3.Text = "...";
            this.btnselectfile3.UseVisualStyleBackColor = true;
            this.btnselectfile3.Click += new System.EventHandler(this.btnselectfile3_Click);
            // 
            // btnstop
            // 
            this.btnstop.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnstop.Location = new System.Drawing.Point(350, 199);
            this.btnstop.Name = "btnstop";
            this.btnstop.Size = new System.Drawing.Size(75, 23);
            this.btnstop.TabIndex = 8;
            this.btnstop.Text = "stop";
            this.btnstop.UseVisualStyleBackColor = true;
            this.btnstop.Click += new System.EventHandler(this.btnstop_Click);
            // 
            // btnstart
            // 
            this.btnstart.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnstart.Location = new System.Drawing.Point(269, 199);
            this.btnstart.Name = "btnstart";
            this.btnstart.Size = new System.Drawing.Size(75, 23);
            this.btnstart.TabIndex = 7;
            this.btnstart.Text = "start";
            this.btnstart.UseVisualStyleBackColor = true;
            this.btnstart.Click += new System.EventHandler(this.btnstart_Click);
            // 
            // cboloop
            // 
            this.cboloop.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cboloop.AutoSize = true;
            this.cboloop.Location = new System.Drawing.Point(44, 144);
            this.cboloop.Name = "cboloop";
            this.cboloop.Size = new System.Drawing.Size(48, 16);
            this.cboloop.TabIndex = 9;
            this.cboloop.Text = "loop";
            this.cboloop.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(21, 94);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 12);
            this.label4.TabIndex = 10;
            this.label4.Text = "ip";
            // 
            // txtip
            // 
            this.txtip.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtip.Location = new System.Drawing.Point(44, 90);
            this.txtip.Name = "txtip";
            this.txtip.Size = new System.Drawing.Size(100, 21);
            this.txtip.TabIndex = 11;
            this.txtip.Text = "224.2.2.2";
            // 
            // txtport
            // 
            this.txtport.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtport.Location = new System.Drawing.Point(44, 117);
            this.txtport.Name = "txtport";
            this.txtport.Size = new System.Drawing.Size(100, 21);
            this.txtport.TabIndex = 11;
            this.txtport.Text = "5005";
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 121);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 10;
            this.label5.Text = "port";
            // 
            // txtdefaultrate
            // 
            this.txtdefaultrate.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtdefaultrate.Location = new System.Drawing.Point(44, 166);
            this.txtdefaultrate.Name = "txtdefaultrate";
            this.txtdefaultrate.Size = new System.Drawing.Size(100, 21);
            this.txtdefaultrate.TabIndex = 12;
            this.txtdefaultrate.Text = "0.5";
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 170);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 12);
            this.label6.TabIndex = 13;
            this.label6.Text = "rate";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(456, 232);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "send file";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtfile1;
        private System.Windows.Forms.TextBox txtfile2;
        private System.Windows.Forms.Button btnselectfile1;
        private System.Windows.Forms.Button btnselectfile2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtfile3;
        private System.Windows.Forms.Button btnselectfile3;
        private System.Windows.Forms.Button btnstart;
        private System.Windows.Forms.CheckBox cboloop;
        private System.Windows.Forms.Button btnstop;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtip;
        private System.Windows.Forms.TextBox txtport;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtdefaultrate;
        private System.Windows.Forms.Label label6;
    }
}

