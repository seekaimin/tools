namespace UtilToolTipHelper
{
    partial class ToolTipHelperForm
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
            this.components = new System.ComponentModel.Container();
            this.timerFormKiller = new System.Windows.Forms.Timer(this.components);
            this.timerDeadFormKiller = new System.Windows.Forms.Timer(this.components);
            this.serviceNotify = new System.Windows.Forms.NotifyIcon(this.components);
            this.SuspendLayout();
            // 
            // timerFormKiller
            // 
            this.timerFormKiller.Interval = 500;
            this.timerFormKiller.Tick += new System.EventHandler(this.timerFormKiller_Tick);
            // 
            // timerDeadFormKiller
            // 
            this.timerDeadFormKiller.Interval = 500;
            this.timerDeadFormKiller.Tick += new System.EventHandler(this.timerDeadFormKiller_Tick);
            // 
            // serviceNotify
            // 
            this.serviceNotify.Text = "message";
            this.serviceNotify.Visible = true;
            // 
            // ToolTipHelperForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Name = "ToolTipHelperForm";
            this.ShowInTaskbar = false;
            this.Text = "ToolTipHelperForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.Load += new System.EventHandler(this.ToolTipHelperForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timerFormKiller;
        private System.Windows.Forms.Timer timerDeadFormKiller;
        private System.Windows.Forms.NotifyIcon serviceNotify;
    }
}