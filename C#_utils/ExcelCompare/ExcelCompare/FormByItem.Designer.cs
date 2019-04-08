namespace ExcelCompare
{
    partial class FormByItem
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormByItem));
            this.gbTop = new System.Windows.Forms.GroupBox();
            this.tableTop = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtold = new System.Windows.Forms.TextBox();
            this.txtnew = new System.Windows.Forms.TextBox();
            this.btnslect_old = new System.Windows.Forms.Button();
            this.btnslect_new = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.txtConsole = new System.Windows.Forms.RichTextBox();
            this.btnCompare = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.DGV = new System.Windows.Forms.DataGridView();
            this.colCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOldCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNewCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRemark = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.gbTop.Size = new System.Drawing.Size(1037, 100);
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
            this.tableTop.Controls.Add(this.label2, 0, 1);
            this.tableTop.Controls.Add(this.txtold, 1, 0);
            this.tableTop.Controls.Add(this.txtnew, 1, 1);
            this.tableTop.Controls.Add(this.btnslect_old, 2, 0);
            this.tableTop.Controls.Add(this.btnslect_new, 2, 1);
            this.tableTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableTop.Location = new System.Drawing.Point(3, 17);
            this.tableTop.Name = "tableTop";
            this.tableTop.RowCount = 4;
            this.tableTop.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableTop.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableTop.Size = new System.Drawing.Size(1031, 80);
            this.tableTop.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "更改前BOM";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "更改后BOM";
            // 
            // txtold
            // 
            this.txtold.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtold.Location = new System.Drawing.Point(68, 4);
            this.txtold.Name = "txtold";
            this.txtold.ReadOnly = true;
            this.txtold.Size = new System.Drawing.Size(904, 21);
            this.txtold.TabIndex = 1;
            this.txtold.Text = "D:\\WorkSpace\\C#\\MYUtilSolution\\ExcelCompare\\ExcelCompare\\doc\\2030030934.XLS";
            this.txtold.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtBox_DragDrop);
            this.txtold.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtBox_DragEnter);
            this.txtold.DoubleClick += new System.EventHandler(this.txtnew_DoubleClick);
            // 
            // txtnew
            // 
            this.txtnew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtnew.Location = new System.Drawing.Point(68, 33);
            this.txtnew.Name = "txtnew";
            this.txtnew.ReadOnly = true;
            this.txtnew.Size = new System.Drawing.Size(904, 21);
            this.txtnew.TabIndex = 2;
            this.txtnew.Text = "D:\\WorkSpace\\C#\\MYUtilSolution\\ExcelCompare\\ExcelCompare\\doc\\2030031405.XLS";
            this.txtnew.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtBox_DragDrop);
            this.txtnew.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtBox_DragEnter);
            this.txtnew.DoubleClick += new System.EventHandler(this.txtnew_DoubleClick);
            // 
            // btnslect_old
            // 
            this.btnslect_old.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnslect_old.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnslect_old.Location = new System.Drawing.Point(978, 3);
            this.btnslect_old.Name = "btnslect_old";
            this.btnslect_old.Size = new System.Drawing.Size(50, 23);
            this.btnslect_old.TabIndex = 3;
            this.btnslect_old.Text = "清空";
            this.btnslect_old.UseVisualStyleBackColor = true;
            this.btnslect_old.Click += new System.EventHandler(this.btnslect_old_Click);
            // 
            // btnslect_new
            // 
            this.btnslect_new.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnslect_new.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnslect_new.Location = new System.Drawing.Point(978, 32);
            this.btnslect_new.Name = "btnslect_new";
            this.btnslect_new.Size = new System.Drawing.Size(50, 23);
            this.btnslect_new.TabIndex = 3;
            this.btnslect_new.Text = "清空";
            this.btnslect_new.UseVisualStyleBackColor = true;
            this.btnslect_new.Click += new System.EventHandler(this.btnslect_new_Click);
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
            this.groupBox3.Location = new System.Drawing.Point(0, 100);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(1037, 382);
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
            this.colCode,
            this.colName,
            this.colMode,
            this.colOldCount,
            this.colNewCount,
            this.colRemark});
            this.DGV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DGV.Location = new System.Drawing.Point(3, 17);
            this.DGV.Name = "DGV";
            this.DGV.ReadOnly = true;
            this.DGV.RowTemplate.Height = 23;
            this.DGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.DGV.Size = new System.Drawing.Size(1031, 362);
            this.DGV.TabIndex = 0;
            // 
            // colCode
            // 
            this.colCode.HeaderText = "子件编码";
            this.colCode.Name = "colCode";
            this.colCode.ReadOnly = true;
            // 
            // colName
            // 
            this.colName.HeaderText = "子件名称";
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            // 
            // colMode
            // 
            this.colMode.HeaderText = "子件规格";
            this.colMode.Name = "colMode";
            this.colMode.ReadOnly = true;
            // 
            // colOldCount
            // 
            this.colOldCount.HeaderText = "(改前)数量";
            this.colOldCount.Name = "colOldCount";
            this.colOldCount.ReadOnly = true;
            this.colOldCount.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // colNewCount
            // 
            this.colNewCount.HeaderText = "(改后)数量";
            this.colNewCount.Name = "colNewCount";
            this.colNewCount.ReadOnly = true;
            // 
            // colRemark
            // 
            this.colRemark.HeaderText = "备注";
            this.colRemark.Name = "colRemark";
            this.colRemark.ReadOnly = true;
            // 
            // FormByItem
            // 
            this.AcceptButton = this.btnCompare;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1037, 639);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.gbTop);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormByItem";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "按子件分析";
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
        private System.Windows.Forms.Button btnslect_old;
        private System.Windows.Forms.Button btnslect_new;
        private System.Windows.Forms.RichTextBox txtConsole;
        private System.Windows.Forms.DataGridView DGV;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOldCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNewCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRemark;
    }
}

