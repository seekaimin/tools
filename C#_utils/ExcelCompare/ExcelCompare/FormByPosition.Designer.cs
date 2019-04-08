namespace ExcelCompare
{
    partial class FormByPosition
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormByPosition));
            this.gbTop = new System.Windows.Forms.GroupBox();
            this.tableTop = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtold = new System.Windows.Forms.TextBox();
            this.txtnew = new System.Windows.Forms.TextBox();
            this.btnslect_old_clear = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtoldfj = new System.Windows.Forms.TextBox();
            this.btnslect_new_clear = new System.Windows.Forms.Button();
            this.btnslect_old_fj_clear = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtnewfj = new System.Windows.Forms.TextBox();
            this.btnslect_new_fj_clear = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.txtConsole = new System.Windows.Forms.RichTextBox();
            this.btnCompare = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.DGV = new System.Windows.Forms.DataGridView();
            this.colDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPosition = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOldCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOldName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOldMode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOldRemark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNewCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNewName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNewMode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNewRemark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gbTop.SuspendLayout();
            this.tableTop.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV)).BeginInit();
            this.SuspendLayout();
            // 
            // gbTop
            // 
            this.gbTop.Controls.Add(this.tableTop);
            this.gbTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbTop.Location = new System.Drawing.Point(0, 0);
            this.gbTop.Name = "gbTop";
            this.gbTop.Size = new System.Drawing.Size(1037, 177);
            this.gbTop.TabIndex = 0;
            this.gbTop.TabStop = false;
            this.gbTop.Text = "Excel文件";
            // 
            // tableTop
            // 
            this.tableTop.ColumnCount = 3;
            this.tableTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableTop.Controls.Add(this.label1, 0, 0);
            this.tableTop.Controls.Add(this.label2, 0, 2);
            this.tableTop.Controls.Add(this.txtold, 1, 0);
            this.tableTop.Controls.Add(this.txtnew, 1, 2);
            this.tableTop.Controls.Add(this.btnslect_old_clear, 2, 0);
            this.tableTop.Controls.Add(this.label3, 0, 1);
            this.tableTop.Controls.Add(this.txtoldfj, 1, 1);
            this.tableTop.Controls.Add(this.btnslect_new_clear, 2, 2);
            this.tableTop.Controls.Add(this.btnslect_old_fj_clear, 2, 1);
            this.tableTop.Controls.Add(this.label4, 0, 3);
            this.tableTop.Controls.Add(this.txtnewfj, 1, 3);
            this.tableTop.Controls.Add(this.btnslect_new_fj_clear, 2, 3);
            this.tableTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableTop.Location = new System.Drawing.Point(3, 17);
            this.tableTop.Name = "tableTop";
            this.tableTop.RowCount = 6;
            this.tableTop.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableTop.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableTop.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableTop.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableTop.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableTop.Size = new System.Drawing.Size(1031, 157);
            this.tableTop.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "更改前BOM";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "更改后BOM";
            // 
            // txtold
            // 
            this.txtold.AllowDrop = true;
            this.txtold.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtold.Location = new System.Drawing.Point(92, 4);
            this.txtold.Name = "txtold";
            this.txtold.ReadOnly = true;
            this.txtold.Size = new System.Drawing.Size(880, 21);
            this.txtold.TabIndex = 1;
            this.txtold.Text = "D:\\WorkSpace\\C#\\MYUtilSolution\\ExcelCompare\\ExcelCompare\\doc\\2030030934.XLS";
            this.txtold.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtBox_DragDrop);
            this.txtold.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtBox_DragEnter);
            this.txtold.DoubleClick += new System.EventHandler(this.txtSelect_DoubleClick);
            // 
            // txtnew
            // 
            this.txtnew.AllowDrop = true;
            this.txtnew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtnew.Location = new System.Drawing.Point(92, 62);
            this.txtnew.Name = "txtnew";
            this.txtnew.ReadOnly = true;
            this.txtnew.Size = new System.Drawing.Size(880, 21);
            this.txtnew.TabIndex = 2;
            this.txtnew.Text = "D:\\WorkSpace\\C#\\MYUtilSolution\\ExcelCompare\\ExcelCompare\\doc\\2030031405.XLS";
            this.txtnew.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtBox_DragDrop);
            this.txtnew.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtBox_DragEnter);
            this.txtnew.DoubleClick += new System.EventHandler(this.txtSelect_DoubleClick);
            // 
            // btnslect_old_clear
            // 
            this.btnslect_old_clear.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnslect_old_clear.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnslect_old_clear.Location = new System.Drawing.Point(978, 3);
            this.btnslect_old_clear.Name = "btnslect_old_clear";
            this.btnslect_old_clear.Size = new System.Drawing.Size(50, 23);
            this.btnslect_old_clear.TabIndex = 3;
            this.btnslect_old_clear.Text = "清空";
            this.btnslect_old_clear.UseVisualStyleBackColor = true;
            this.btnslect_old_clear.Click += new System.EventHandler(this.btnslect_old_clear_Click);
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 37);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "更改前BOM附件";
            // 
            // txtoldfj
            // 
            this.txtoldfj.AllowDrop = true;
            this.txtoldfj.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtoldfj.Location = new System.Drawing.Point(92, 33);
            this.txtoldfj.Name = "txtoldfj";
            this.txtoldfj.ReadOnly = true;
            this.txtoldfj.Size = new System.Drawing.Size(880, 21);
            this.txtoldfj.TabIndex = 1;
            this.txtoldfj.Text = "D:\\WorkSpace\\C#\\MYUtilSolution\\ExcelCompare\\ExcelCompare\\doc\\2030030934.XLS";
            this.txtoldfj.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtBox_DragDrop);
            this.txtoldfj.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtBox_DragEnter);
            this.txtoldfj.DoubleClick += new System.EventHandler(this.txtSelect_DoubleClick);
            // 
            // btnslect_new_clear
            // 
            this.btnslect_new_clear.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnslect_new_clear.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnslect_new_clear.Location = new System.Drawing.Point(978, 61);
            this.btnslect_new_clear.Name = "btnslect_new_clear";
            this.btnslect_new_clear.Size = new System.Drawing.Size(50, 23);
            this.btnslect_new_clear.TabIndex = 3;
            this.btnslect_new_clear.Text = "清空";
            this.btnslect_new_clear.UseVisualStyleBackColor = true;
            this.btnslect_new_clear.Click += new System.EventHandler(this.btnslect_new_clear_Click);
            // 
            // btnslect_old_fj_clear
            // 
            this.btnslect_old_fj_clear.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnslect_old_fj_clear.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnslect_old_fj_clear.Location = new System.Drawing.Point(978, 32);
            this.btnslect_old_fj_clear.Name = "btnslect_old_fj_clear";
            this.btnslect_old_fj_clear.Size = new System.Drawing.Size(50, 23);
            this.btnslect_old_fj_clear.TabIndex = 3;
            this.btnslect_old_fj_clear.Text = "清空";
            this.btnslect_old_fj_clear.UseVisualStyleBackColor = true;
            this.btnslect_old_fj_clear.Click += new System.EventHandler(this.btnslect_old_fj_clear_Click);
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 95);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "更改后BOM附件";
            // 
            // txtnewfj
            // 
            this.txtnewfj.AllowDrop = true;
            this.txtnewfj.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtnewfj.Location = new System.Drawing.Point(92, 91);
            this.txtnewfj.Name = "txtnewfj";
            this.txtnewfj.ReadOnly = true;
            this.txtnewfj.Size = new System.Drawing.Size(880, 21);
            this.txtnewfj.TabIndex = 2;
            this.txtnewfj.Text = "D:\\WorkSpace\\C#\\MYUtilSolution\\ExcelCompare\\ExcelCompare\\doc\\2030031405.XLS";
            this.txtnewfj.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtBox_DragDrop);
            this.txtnewfj.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtBox_DragEnter);
            this.txtnewfj.DoubleClick += new System.EventHandler(this.txtSelect_DoubleClick);
            // 
            // btnslect_new_fj_clear
            // 
            this.btnslect_new_fj_clear.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnslect_new_fj_clear.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnslect_new_fj_clear.Location = new System.Drawing.Point(978, 90);
            this.btnslect_new_fj_clear.Name = "btnslect_new_fj_clear";
            this.btnslect_new_fj_clear.Size = new System.Drawing.Size(50, 23);
            this.btnslect_new_fj_clear.TabIndex = 3;
            this.btnslect_new_fj_clear.Text = "清空";
            this.btnslect_new_fj_clear.UseVisualStyleBackColor = true;
            this.btnslect_new_fj_clear.Click += new System.EventHandler(this.btnslect_new_fj_clear_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tableLayoutPanel1);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox2.Location = new System.Drawing.Point(0, 482);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1037, 157);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "操作";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel1.Controls.Add(this.txtConsole, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnCompare, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnExport, 2, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 17);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1031, 137);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // txtConsole
            // 
            this.txtConsole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtConsole.Location = new System.Drawing.Point(3, 3);
            this.txtConsole.Name = "txtConsole";
            this.txtConsole.Size = new System.Drawing.Size(865, 131);
            this.txtConsole.TabIndex = 1;
            this.txtConsole.Text = "";
            // 
            // btnCompare
            // 
            this.btnCompare.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCompare.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCompare.Location = new System.Drawing.Point(874, 43);
            this.btnCompare.Name = "btnCompare";
            this.btnCompare.Size = new System.Drawing.Size(74, 50);
            this.btnCompare.TabIndex = 0;
            this.btnCompare.Text = "开始分析";
            this.btnCompare.UseVisualStyleBackColor = true;
            this.btnCompare.Click += new System.EventHandler(this.btnCompare_Click);
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnExport.Location = new System.Drawing.Point(954, 43);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(74, 50);
            this.btnExport.TabIndex = 2;
            this.btnExport.Text = "导出";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.DGV);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(0, 177);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(1037, 305);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "分析结果";
            // 
            // DGV
            // 
            this.DGV.AllowUserToAddRows = false;
            this.DGV.AllowUserToDeleteRows = false;
            this.DGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colDescription,
            this.colPosition,
            this.colOldCode,
            this.colOldName,
            this.colOldMode,
            this.colOldRemark,
            this.colNewCode,
            this.colNewName,
            this.colNewMode,
            this.colNewRemark});
            this.DGV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DGV.Location = new System.Drawing.Point(3, 17);
            this.DGV.Name = "DGV";
            this.DGV.ReadOnly = true;
            this.DGV.RowTemplate.Height = 23;
            this.DGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.DGV.Size = new System.Drawing.Size(1031, 285);
            this.DGV.TabIndex = 0;
            // 
            // colDescription
            // 
            this.colDescription.HeaderText = "是否焊";
            this.colDescription.Name = "colDescription";
            this.colDescription.ReadOnly = true;
            this.colDescription.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // colPosition
            // 
            this.colPosition.HeaderText = "位号";
            this.colPosition.Name = "colPosition";
            this.colPosition.ReadOnly = true;
            // 
            // colOldCode
            // 
            this.colOldCode.HeaderText = "(改前)子件编码";
            this.colOldCode.Name = "colOldCode";
            this.colOldCode.ReadOnly = true;
            // 
            // colOldName
            // 
            this.colOldName.HeaderText = "(改前)子件名称";
            this.colOldName.Name = "colOldName";
            this.colOldName.ReadOnly = true;
            // 
            // colOldMode
            // 
            this.colOldMode.HeaderText = "(改前)子件规格";
            this.colOldMode.Name = "colOldMode";
            this.colOldMode.ReadOnly = true;
            // 
            // colOldRemark
            // 
            this.colOldRemark.HeaderText = "(改前)备注";
            this.colOldRemark.Name = "colOldRemark";
            this.colOldRemark.ReadOnly = true;
            // 
            // colNewCode
            // 
            this.colNewCode.HeaderText = "(改后)子件编码";
            this.colNewCode.Name = "colNewCode";
            this.colNewCode.ReadOnly = true;
            // 
            // colNewName
            // 
            this.colNewName.HeaderText = "(改后)子件名称";
            this.colNewName.Name = "colNewName";
            this.colNewName.ReadOnly = true;
            // 
            // colNewMode
            // 
            this.colNewMode.HeaderText = "(改后)子件规格";
            this.colNewMode.Name = "colNewMode";
            this.colNewMode.ReadOnly = true;
            // 
            // colNewRemark
            // 
            this.colNewRemark.HeaderText = "(改后)备注";
            this.colNewRemark.Name = "colNewRemark";
            this.colNewRemark.ReadOnly = true;
            // 
            // FormByPosition
            // 
            this.AcceptButton = this.btnCompare;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1037, 639);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.gbTop);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormByPosition";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "按区位分析";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.gbTop.ResumeLayout(false);
            this.tableTop.ResumeLayout(false);
            this.tableTop.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DGV)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbTop;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnCompare;
        private System.Windows.Forms.TableLayoutPanel tableTop;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtold;
        private System.Windows.Forms.TextBox txtnew;
        private System.Windows.Forms.Button btnslect_old_clear;
        private System.Windows.Forms.RichTextBox txtConsole;
        private System.Windows.Forms.DataGridView DGV;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPosition;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOldCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOldName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOldMode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOldRemark;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNewCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNewName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNewMode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNewRemark;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtoldfj;
        private System.Windows.Forms.Button btnslect_new_clear;
        private System.Windows.Forms.Button btnslect_old_fj_clear;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtnewfj;
        private System.Windows.Forms.Button btnslect_new_fj_clear;
    }
}

