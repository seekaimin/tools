namespace Util.WinForm.UC
{
    partial class UCDateAndTime
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
            this.Pnltable = new System.Windows.Forms.TableLayoutPanel();
            this.txtDate = new System.Windows.Forms.DateTimePicker();
            this.txtTime = new System.Windows.Forms.DateTimePicker();
            this.Pnltable.SuspendLayout();
            this.SuspendLayout();
            // 
            // Pnltable
            // 
            this.Pnltable.ColumnCount = 2;
            this.Pnltable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.Pnltable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.Pnltable.Controls.Add(this.txtDate, 0, 0);
            this.Pnltable.Controls.Add(this.txtTime, 1, 0);
            this.Pnltable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Pnltable.Location = new System.Drawing.Point(0, 0);
            this.Pnltable.Margin = new System.Windows.Forms.Padding(0);
            this.Pnltable.Name = "Pnltable";
            this.Pnltable.RowCount = 1;
            this.Pnltable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.Pnltable.Size = new System.Drawing.Size(229, 20);
            this.Pnltable.TabIndex = 0;
            // 
            // txtDate
            // 
            this.txtDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.txtDate.Location = new System.Drawing.Point(0, 0);
            this.txtDate.Margin = new System.Windows.Forms.Padding(0);
            this.txtDate.Name = "txtDate";
            this.txtDate.Size = new System.Drawing.Size(114, 21);
            this.txtDate.TabIndex = 0;
            this.txtDate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtDate_KeyDown);
            // 
            // txtTime
            // 
            this.txtTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTime.Cursor = System.Windows.Forms.Cursors.Default;
            this.txtTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.txtTime.Location = new System.Drawing.Point(114, 0);
            this.txtTime.Margin = new System.Windows.Forms.Padding(0);
            this.txtTime.Name = "txtTime";
            this.txtTime.ShowUpDown = true;
            this.txtTime.Size = new System.Drawing.Size(115, 21);
            this.txtTime.TabIndex = 1;
            this.txtTime.Visible = false;
            // 
            // UCDateAndTime
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Pnltable);
            this.Name = "UCDateAndTime";
            this.Size = new System.Drawing.Size(229, 20);
            this.Pnltable.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel Pnltable;
        private System.Windows.Forms.DateTimePicker txtDate;
        private System.Windows.Forms.DateTimePicker txtTime;
    }
}
