namespace Util.WinForm.UC
{
    partial class UCDateTimePicker
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.numYear = new System.Windows.Forms.NumericUpDown();
            this.numMonth = new System.Windows.Forms.NumericUpDown();
            this.numDay = new System.Windows.Forms.NumericUpDown();
            this.numHour = new System.Windows.Forms.NumericUpDown();
            this.numMinute = new System.Windows.Forms.NumericUpDown();
            this.numSecond = new System.Windows.Forms.NumericUpDown();
            this.lblYear = new System.Windows.Forms.Label();
            this.lblMonth = new System.Windows.Forms.Label();
            this.lblDay = new System.Windows.Forms.Label();
            this.lblHour = new System.Windows.Forms.Label();
            this.lblMinute = new System.Windows.Forms.Label();
            this.lblSecond = new System.Windows.Forms.Label();
            this.table_date_time = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.numYear)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMonth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHour)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMinute)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSecond)).BeginInit();
            this.table_date_time.SuspendLayout();
            this.SuspendLayout();
            // 
            // numYear
            // 
            this.numYear.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.numYear.Location = new System.Drawing.Point(26, 9);
            this.numYear.Maximum = new decimal(new int[] {
            9998,
            0,
            0,
            0});
            this.numYear.Minimum = new decimal(new int[] {
            2015,
            0,
            0,
            0});
            this.numYear.Name = "numYear";
            this.numYear.Size = new System.Drawing.Size(46, 21);
            this.numYear.TabIndex = 0;
            this.numYear.Value = new decimal(new int[] {
            9998,
            0,
            0,
            0});
            // 
            // numMonth
            // 
            this.numMonth.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.numMonth.Location = new System.Drawing.Point(101, 9);
            this.numMonth.Maximum = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.numMonth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMonth.Name = "numMonth";
            this.numMonth.Size = new System.Drawing.Size(35, 21);
            this.numMonth.TabIndex = 0;
            this.numMonth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numDay
            // 
            this.numDay.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.numDay.Location = new System.Drawing.Point(165, 9);
            this.numDay.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.numDay.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numDay.Name = "numDay";
            this.numDay.Size = new System.Drawing.Size(35, 21);
            this.numDay.TabIndex = 0;
            this.numDay.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numHour
            // 
            this.numHour.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.numHour.Location = new System.Drawing.Point(229, 9);
            this.numHour.Maximum = new decimal(new int[] {
            23,
            0,
            0,
            0});
            this.numHour.Name = "numHour";
            this.numHour.Size = new System.Drawing.Size(35, 21);
            this.numHour.TabIndex = 0;
            // 
            // numMinute
            // 
            this.numMinute.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.numMinute.Location = new System.Drawing.Point(293, 9);
            this.numMinute.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
            this.numMinute.Name = "numMinute";
            this.numMinute.Size = new System.Drawing.Size(35, 21);
            this.numMinute.TabIndex = 0;
            // 
            // numSecond
            // 
            this.numSecond.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.numSecond.Location = new System.Drawing.Point(357, 9);
            this.numSecond.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
            this.numSecond.Name = "numSecond";
            this.numSecond.Size = new System.Drawing.Size(35, 21);
            this.numSecond.TabIndex = 0;
            // 
            // lblYear
            // 
            this.lblYear.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblYear.AutoSize = true;
            this.lblYear.Location = new System.Drawing.Point(3, 13);
            this.lblYear.Name = "lblYear";
            this.lblYear.Size = new System.Drawing.Size(17, 12);
            this.lblYear.TabIndex = 1;
            this.lblYear.Text = "年";
            // 
            // lblMonth
            // 
            this.lblMonth.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblMonth.AutoSize = true;
            this.lblMonth.Location = new System.Drawing.Point(78, 13);
            this.lblMonth.Name = "lblMonth";
            this.lblMonth.Size = new System.Drawing.Size(17, 12);
            this.lblMonth.TabIndex = 1;
            this.lblMonth.Text = "月";
            // 
            // lblDay
            // 
            this.lblDay.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblDay.AutoSize = true;
            this.lblDay.Location = new System.Drawing.Point(142, 13);
            this.lblDay.Name = "lblDay";
            this.lblDay.Size = new System.Drawing.Size(17, 12);
            this.lblDay.TabIndex = 1;
            this.lblDay.Text = "日";
            // 
            // lblHour
            // 
            this.lblHour.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblHour.AutoSize = true;
            this.lblHour.Location = new System.Drawing.Point(206, 13);
            this.lblHour.Name = "lblHour";
            this.lblHour.Size = new System.Drawing.Size(17, 12);
            this.lblHour.TabIndex = 1;
            this.lblHour.Text = "时";
            // 
            // lblMinute
            // 
            this.lblMinute.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblMinute.AutoSize = true;
            this.lblMinute.Location = new System.Drawing.Point(270, 13);
            this.lblMinute.Name = "lblMinute";
            this.lblMinute.Size = new System.Drawing.Size(17, 12);
            this.lblMinute.TabIndex = 1;
            this.lblMinute.Text = "分";
            // 
            // lblSecond
            // 
            this.lblSecond.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblSecond.AutoSize = true;
            this.lblSecond.Location = new System.Drawing.Point(334, 13);
            this.lblSecond.Name = "lblSecond";
            this.lblSecond.Size = new System.Drawing.Size(17, 12);
            this.lblSecond.TabIndex = 1;
            this.lblSecond.Text = "秒";
            // 
            // table_date_time
            // 
            this.table_date_time.ColumnCount = 12;
            this.table_date_time.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.table_date_time.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.table_date_time.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.table_date_time.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.table_date_time.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.table_date_time.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.table_date_time.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.table_date_time.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.table_date_time.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.table_date_time.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.table_date_time.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.table_date_time.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.table_date_time.Controls.Add(this.numSecond, 11, 0);
            this.table_date_time.Controls.Add(this.numMinute, 9, 0);
            this.table_date_time.Controls.Add(this.lblSecond, 10, 0);
            this.table_date_time.Controls.Add(this.numHour, 7, 0);
            this.table_date_time.Controls.Add(this.numDay, 5, 0);
            this.table_date_time.Controls.Add(this.lblMinute, 8, 0);
            this.table_date_time.Controls.Add(this.numMonth, 3, 0);
            this.table_date_time.Controls.Add(this.numYear, 1, 0);
            this.table_date_time.Controls.Add(this.lblHour, 6, 0);
            this.table_date_time.Controls.Add(this.lblYear, 0, 0);
            this.table_date_time.Controls.Add(this.lblMonth, 2, 0);
            this.table_date_time.Controls.Add(this.lblDay, 4, 0);
            this.table_date_time.Dock = System.Windows.Forms.DockStyle.Fill;
            this.table_date_time.Location = new System.Drawing.Point(0, 0);
            this.table_date_time.Name = "table_date_time";
            this.table_date_time.RowCount = 1;
            this.table_date_time.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.table_date_time.Size = new System.Drawing.Size(577, 39);
            this.table_date_time.TabIndex = 0;
            // 
            // UCDateTimePicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.table_date_time);
            this.Name = "UCDateTimePicker";
            this.Size = new System.Drawing.Size(577, 39);
            ((System.ComponentModel.ISupportInitialize)(this.numYear)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMonth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHour)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMinute)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSecond)).EndInit();
            this.table_date_time.ResumeLayout(false);
            this.table_date_time.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NumericUpDown numHour;
        private System.Windows.Forms.NumericUpDown numMinute;
        private System.Windows.Forms.NumericUpDown numSecond;
        private System.Windows.Forms.Label lblHour;
        private System.Windows.Forms.Label lblMinute;
        private System.Windows.Forms.Label lblSecond;
        private System.Windows.Forms.NumericUpDown numYear;
        private System.Windows.Forms.NumericUpDown numMonth;
        private System.Windows.Forms.NumericUpDown numDay;
        private System.Windows.Forms.Label lblYear;
        private System.Windows.Forms.Label lblMonth;
        private System.Windows.Forms.Label lblDay;
        private System.Windows.Forms.TableLayoutPanel table_date_time;
    }
}
