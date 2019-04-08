using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Util.WinForm.DGVHelpers
{
    /// <summary>
    /// 选择
    /// </summary>
    public partial class FormSelectColumns : Form
    {
        /// <summary>
        /// 选择的结果
        /// </summary>
        public Dictionary<string, string> SelectedColumns = new Dictionary<string, string>();
        /// <summary>
        /// 构造选择DGV的列
        /// </summary>
        /// <param name="kvs">需要选择的集合</param>
        public FormSelectColumns(List<KeyValuePair<string, string>> kvs)
        {
            InitializeComponent();

            this.SuspendLayout();
            AddControls(kvs);
            this.ResumeLayout(false);
        }
        int width = 150;
        int height = 20;
        int width_count = 3;
        void AddControls(List<KeyValuePair<string, string>> kvs)
        {
            for (int i = 0; i < kvs.Count; i++)
            {
                CheckBox cbo = new CheckBox()
                {
                    Name = kvs[i].Key,
                    Text = kvs[i].Value,
                };
                int x = (i % width_count) * width;
                int y = (i / width_count) * height;
                cbo.Location = new Point(x, y);
                cbo.Click += (s, e) =>
                {
                    int checked_count = 0;
                    foreach (CheckBox c in PanelPool.Controls)
                    {
                        if (c.Checked == false)
                        {
                            checked_count++;
                        }
                    }
                    if (checked_count == 0)
                    {
                        cboSelectedAll.CheckState = CheckState.Checked;
                    }
                    else if (checked_count == PanelPool.Controls.Count)
                    {
                        cboSelectedAll.CheckState = CheckState.Unchecked;
                    }
                    else
                    {
                        cboSelectedAll.CheckState = CheckState.Indeterminate;
                    }
                };
                PanelPool.Controls.Add(cbo);
            }
        }
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            SelectedColumns.Clear();
            foreach (CheckBox cbo in PanelPool.Controls)
            {
                if (cbo.Checked)
                {
                    SelectedColumns.Add(cbo.Name, cbo.Text);
                }
            }
            if (SelectedColumns.Count > 0)
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            else
            {
                MessageBox.Show("please select item(s)!");
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.No;
        }
        private void cboSelectedAll_Click(object sender, EventArgs e)
        {
            if (cboSelectedAll.CheckState == CheckState.Checked)
            {
                cboSelectedAll.CheckState = CheckState.Checked;
            }
            else
            {
                cboSelectedAll.CheckState = CheckState.Unchecked;
            }
            foreach (CheckBox cbo in PanelPool.Controls)
            {
                cbo.Checked = cboSelectedAll.Checked;
            }
        }
    }
}
