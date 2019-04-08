using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Util.WinForm.UC
{
    /// <summary>
    /// Winform
    /// </summary>
    public partial class PageControl : UserControl
    {
        private int _PageIndex = 0;
        private int _PageSize = 25;
        private int _TotleCount = 0;
        private int _PageCount = 0;

        /// <summary>
        /// 总记录数显示文本
        /// </summary>
        [
          Category("自定义属性"),
          Description("总记录数显示文本"),
          DefaultValue("总记录数")
        ]
        public string TotalCountText
        {
            get
            {
                return lblTotalCount.Text;
            }
            set
            {
                lblTotalCount.Text = value;
            }
        }
        #region 自定义属性
        /// <summary>
        /// 当前第几页
        /// 重0开始算
        /// </summary>
        [
          Category("自定义属性"),
          Description("当前页码下标")
        ]
        public int PageIndex
        {
            get
            {
                return _PageIndex;
            }
            set
            {
                _PageIndex = value;
                txtIndex.Text = (_PageIndex + 1).ToString();
                ChangeButtonEnable();
            }
        }
        /// <summary>
        /// 总页数
        /// </summary>
        [
          Category("自定义属性"),
          Description("总页数")
        ]
        public int PageCount
        {
            get
            {
                return _PageCount;
            }
            private set
            {
                _PageCount = value;
                lblPageCount.Text = _PageCount.ToString();
            }
        }
        /// <summary>
        /// 每页显示记录数
        /// </summary>
        [
          Category("自定义属性"),
          Description("每页记录数")
        ]
        public int PageSize
        {
            get { return _PageSize; }
            set { _PageSize = value; }
        }
        /// <summary>
        /// 总记录数
        /// </summary>
        [
          Category("自定义属性"),
          Description("总记录数")
        ]
        public int TotleCount
        {
            get { return _TotleCount; }
            set
            {
                _TotleCount = value;
                if (PageSize > 0)
                    PageCount = (value - 1) / PageSize + 1;
                else
                    PageCount = 0;
                this.lblPageCount.Text = PageCount.ToString();
                this.lblTotalCount.Text = value.ToString();
                //从新计算PageIndex
                if (PageIndex > PageCount - 1)
                {
                    PageIndex = PageCount - 1;
                }
                else
                {
                    PageIndex = PageIndex;
                }
            }
        }
        #endregion
        /// <summary>
        /// 分页控件
        /// </summary>
        public PageControl()
        {
            InitializeComponent();
        }
        private void txtIndex_TextChanged(object sender, EventArgs e)
        {
            int width = (int)(this.Font.Size + this.Font.Size);
            if (txtIndex.Text.Length > 0)
            {
                Label lbl = new Label();
                lbl.Text = txtIndex.Text;
                width = lbl.PreferredWidth + (int)this.Font.Size;
            }
            txtIndex.Width = width;
        }
        /// <summary>
        /// 当按钮按下时出发
        /// </summary>
        private void Button_Click(object sender, EventArgs e)
        {
            if (sender is Button)
            {
                Button btn = sender as Button;
                int index = 0;
                switch (btn.Name)
                {
                    case "btnFirst":
                        index = 0;
                        break;
                    case "btnPrevious":
                        index = this.PageIndex - 1;
                        break;
                    case "btnNext":
                        index = this.PageIndex + 1;
                        break;
                    case "btnLast":
                        index = this.PageCount - 1;
                        break;
                }
                DoPageChange(index);
            }
        }
        private void txtIndex_KeyDown(object sender, KeyEventArgs e)
        {
            //当按下回车查询
            if (e.KeyCode == Keys.Enter)
            {
                int index = 0;
                try
                {
                    index = Convert.ToInt32(txtIndex.Text) - 1;
                }
                catch
                {
                    index = 0;
                }
                if (index < 0)
                {
                    index = 0;
                }
                index = index > PageCount - 1 ? PageCount - 1 : index;

                DoPageChange(index);
            }
        }
        void DoPageChange(int index)
        {
            if (PageIndex != index)
            {
                if (PageIndexChanged != null)
                {
                    PageIndexChanged(new PageControlArgs(index, PageSize));
                }
            }
            PageIndex = index;
        }
        /// <summary>
        /// 分页函数代理
        /// </summary>
        [
            Description("分页函数代理"),
            Category("自定义函数")
        ]
        public event OnPageIndexChangedHandler PageIndexChanged;
        /// <summary>
        /// 控制按钮是否可用
        /// </summary>
        void ChangeButtonEnable()
        {
            this.btnFirst.Enabled = true;
            this.btnPrevious.Enabled = true;
            this.btnNext.Enabled = true;
            this.btnLast.Enabled = true;
            if (this.PageIndex == 0)
            {
                this.btnFirst.Enabled = false;
                this.btnPrevious.Enabled = false;
            }
            if (this.PageCount == 0 || this.PageIndex == this.PageCount - 1)
            {
                this.btnNext.Enabled = false;
                this.btnLast.Enabled = false;
            }
        }
    }
    /// <summary>
    /// 页码改变代理
    /// </summary>
    /// <param name="args">分页参数</param>
    public delegate void OnPageIndexChangedHandler(PageControlArgs args);
    /// <summary>
    /// 分页参数
    /// </summary>
    public class PageControlArgs : EventArgs
    {
        /// <summary>
        /// 当前页下标
        /// </summary>
        public int PageIndex { get; set; }
        private int _PageSize = 0;
        /// <summary>
        /// 每页显示记录数
        /// </summary>
        public int PageSize
        {
            get { return _PageSize; }
            set
            {
                _PageSize = value < 0 ? 25 : value;
            }
        }
        /// <summary>
        /// 分页参数
        /// </summary>
        /// <param name="index">当前页下标</param>
        /// <param name="size">每一页显示记录数</param>
        public PageControlArgs(int index, int size)
        {
            this.PageIndex = index;
            this.PageSize = size;
        }
    }
}
