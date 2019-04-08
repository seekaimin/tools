using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using Util.Common;

namespace ExcelCompare
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            this.Text = string.Format("{0} v[{1}]",BOMAboutBox.AssemblyTitle, BOMAboutBox.AssemblyVersion);
        }
        #region  按区位分析
        FormByPosition formByPosition = null;
        private void tsm_byposition_Click(object sender, EventArgs e)
        {
            if (formByPosition == null || formByPosition.IsDisposed)
            {
                formByPosition = new FormByPosition();
                formByPosition.MdiParent = this;
            }
            formByPosition.WindowState = FormWindowState.Maximized;
            formByPosition.Show();
        }
        #endregion
        #region 按子件分析
        FormByItem formByItem = null;
        private void tsm_byitem_Click(object sender, EventArgs e)
        {
            if (formByItem == null || formByItem.IsDisposed)
            {
                formByItem = new FormByItem();
                formByItem.MdiParent = this;
            }
            formByItem.WindowState = FormWindowState.Maximized;
            formByItem.Show();
        }
        #endregion

        private void tsm_about_Click(object sender, EventArgs e)
        {
            new BOMAboutBox().ShowDialog();
        }

        private void tsm_modfy_excel_cell_length_Click(object sender, EventArgs e)
        {
            string cmd = "reg add \"HKEY_LOCAL_MACHINE\\SOFTWARE\\Wow6432Node\\Microsoft\\Jet\\3.5\\Engines\\Excel\" /v TypeGuessRows /d 0 /f \r\n"
+ " reg add \"HKEY_LOCAL_MACHINE\\SOFTWARE\\Wow6432Node\\Microsoft\\Jet\\4.0\\Engines\\Excel\" /v TypeGuessRows /d 0 /f \r\n"
+ " reg add \"HKEY_LOCAL_MACHINE\\SOFTWARE\\Wow6432Node\\Microsoft\\Office\\12.0\\Access Connectivity Engine\\Engines\\Excel\" /v TypeGuessRows /d 0 /f \r\n"
+ " @pause \r\n";
            WindowsCMD.Run(cmd, (s) =>
            {
                Tools.d(s);
                if (s == null)
                {
                    MessageBox.Show("修复完成  详情见日志");
                }
            });
        }
    }
}
