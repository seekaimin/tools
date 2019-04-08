namespace UtilToolTipHelper
{
    partial class FormProgressBar
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
            this.valBar = new System.Windows.Forms.ProgressBar();
            this.serviceController = new System.ServiceProcess.ServiceController();
            this.killTimer = new System.Windows.Forms.Timer(this.components);
            this.bkWork = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // valBar
            // 
            this.valBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.valBar.Location = new System.Drawing.Point(0, 0);
            this.valBar.Name = "valBar";
            this.valBar.Size = new System.Drawing.Size(544, 29);
            this.valBar.TabIndex = 1;
            // 
            // killTimer
            // 
            this.killTimer.Interval = 1000;
            this.killTimer.Tick += new System.EventHandler(this.killTimer_Tick);
            // 
            // bkWork
            // 
            this.bkWork.WorkerReportsProgress = true;
            this.bkWork.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bkWork_DoWork);
            this.bkWork.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bkWork_ProgressChanged);
            this.bkWork.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bkWork_RunWorkerCompleted);
            // 
            // FormProgressBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(544, 29);
            this.Controls.Add(this.valBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormProgressBar";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ProgressBar";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar valBar;
        private System.ServiceProcess.ServiceController serviceController;
        private System.Windows.Forms.Timer killTimer;
        private System.ComponentModel.BackgroundWorker bkWork;
    }
}