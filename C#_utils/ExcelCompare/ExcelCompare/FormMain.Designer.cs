namespace ExcelCompare
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.tsm_byposition = new System.Windows.Forms.ToolStripMenuItem();
            this.tsm_byitem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsm_about = new System.Windows.Forms.ToolStripMenuItem();
            this.tsm_modfy_excel_cell_length = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsm_byposition,
            this.tsm_byitem,
            this.tsm_modfy_excel_cell_length,
            this.tsm_about});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(885, 25);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // tsm_byposition
            // 
            this.tsm_byposition.Name = "tsm_byposition";
            this.tsm_byposition.Size = new System.Drawing.Size(80, 21);
            this.tsm_byposition.Text = "按区位分析";
            this.tsm_byposition.Click += new System.EventHandler(this.tsm_byposition_Click);
            // 
            // tsm_byitem
            // 
            this.tsm_byitem.Name = "tsm_byitem";
            this.tsm_byitem.Size = new System.Drawing.Size(80, 21);
            this.tsm_byitem.Text = "按子件分析";
            this.tsm_byitem.Click += new System.EventHandler(this.tsm_byitem_Click);
            // 
            // tsm_about
            // 
            this.tsm_about.Name = "tsm_about";
            this.tsm_about.Size = new System.Drawing.Size(44, 21);
            this.tsm_about.Text = "关于";
            this.tsm_about.Click += new System.EventHandler(this.tsm_about_Click);
            // 
            // tsm_modfy_excel_cell_length
            // 
            this.tsm_modfy_excel_cell_length.Name = "tsm_modfy_excel_cell_length";
            this.tsm_modfy_excel_cell_length.Size = new System.Drawing.Size(92, 21);
            this.tsm_modfy_excel_cell_length.Text = "修复读取问题";
            this.tsm_modfy_excel_cell_length.Click += new System.EventHandler(this.tsm_modfy_excel_cell_length_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(885, 569);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BOM分析工具";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tsm_byposition;
        private System.Windows.Forms.ToolStripMenuItem tsm_byitem;
        private System.Windows.Forms.ToolStripMenuItem tsm_about;
        private System.Windows.Forms.ToolStripMenuItem tsm_modfy_excel_cell_length;
    }
}