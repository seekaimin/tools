namespace TsSenders
{
    partial class Form2
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
            this.btn_active = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_supply = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_stop = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_total_rate = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btn_active
            // 
            this.btn_active.Location = new System.Drawing.Point(76, 121);
            this.btn_active.Name = "btn_active";
            this.btn_active.Size = new System.Drawing.Size(117, 23);
            this.btn_active.TabIndex = 0;
            this.btn_active.Text = "有效数据发送";
            this.btn_active.UseVisualStyleBackColor = true;
            this.btn_active.Click += new System.EventHandler(this.btn_active_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "label1";
            // 
            // btn_supply
            // 
            this.btn_supply.Location = new System.Drawing.Point(76, 150);
            this.btn_supply.Name = "btn_supply";
            this.btn_supply.Size = new System.Drawing.Size(117, 23);
            this.btn_supply.TabIndex = 2;
            this.btn_supply.Text = "补充";
            this.btn_supply.UseVisualStyleBackColor = true;
            this.btn_supply.Click += new System.EventHandler(this.btn_supply_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(31, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "label2";
            // 
            // btn_stop
            // 
            this.btn_stop.Location = new System.Drawing.Point(76, 374);
            this.btn_stop.Name = "btn_stop";
            this.btn_stop.Size = new System.Drawing.Size(117, 23);
            this.btn_stop.TabIndex = 4;
            this.btn_stop.Text = "停止";
            this.btn_stop.UseVisualStyleBackColor = true;
            this.btn_stop.Click += new System.EventHandler(this.btn_stop_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(31, 84);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "set rate(KB)";
            // 
            // txt_total_rate
            // 
            this.txt_total_rate.Location = new System.Drawing.Point(114, 81);
            this.txt_total_rate.MaxLength = 5;
            this.txt_total_rate.Name = "txt_total_rate";
            this.txt_total_rate.Size = new System.Drawing.Size(62, 21);
            this.txt_total_rate.TabIndex = 6;
            this.txt_total_rate.Text = "10";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(50, 215);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(169, 153);
            this.textBox1.TabIndex = 7;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(294, 409);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.txt_total_rate);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btn_stop);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btn_supply);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_active);
            this.Name = "Form2";
            this.Text = "Form2";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_active;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_supply;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_stop;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_total_rate;
        private System.Windows.Forms.TextBox textBox1;
    }
}