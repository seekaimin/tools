using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace Util.WPF.UC
{
    /// <summary>
    /// PageControl.xaml 的交互逻辑
    /// </summary>
    public partial class PageControl : UserControl
    {
        /// <summary>
        /// 构造
        /// </summary>
        public PageControl()
        {
            InitializeComponent();
            DataContext = this;
        }
        #region 自定义属性
        /// <summary>
        /// 改变页数
        /// </summary>
        [
            Description("改变页数"),
            Category("自定义")
        ]
        public event OnPageIndexChangedHandler PageIndexChanged;
        private int _PageIndex = 0;
        private int _PageSize = 25;
        private static readonly DependencyProperty PageCountProperty = DependencyProperty.Register("PageCount", typeof(int), typeof(PageControl), new PropertyMetadata(1));
        private static readonly DependencyProperty TotalCountProperty = DependencyProperty.Register("TotalCount", typeof(int), typeof(PageControl));

        /// <summary>
        /// 当前第几页
        /// </summary>
        [
            Description("当前第几页"),
            Category("自定义")
        ]
        public int PageIndex
        {
            get { return _PageIndex; }
            set
            {
                _PageIndex = value;
                txtIndex.Text = (value + 1).ToString();
                ChangeButtonEnable();
            }
        }
        /// <summary>
        /// 每页显示记录数
        /// </summary>
        [
            Description("每页显示记录数"),
            Category("自定义")
        ]
        public int PageSize
        {
            get { return _PageSize; }
            set { _PageSize = value; }
        }
        /// <summary>
        /// 总页数
        /// </summary>
        [
            Description("总页数"),
            Category("自定义")
        ]
        public int PageCount
        {
            get { return (int)GetValue(PageCountProperty); }
            private set { SetValue(PageCountProperty, value); }
        }
        /// <summary>
        /// 总页数
        /// </summary>
        [
            Description("总记录数"),
            Category("自定义")
        ]
        public int TotalCount
        {
            get { return (int)GetValue(TotalCountProperty); }
            set
            {
                SetValue(TotalCountProperty, value);
                if (PageSize > 0)
                    PageCount = (value - 1) / PageSize + 1;
                else
                    PageCount = 0;
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
        private void Button_Click(object sender, RoutedEventArgs e)
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
            if (e.Key == Key.Enter)
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
        /// <summary>
        /// 处理页码变更事件
        /// </summary>
        /// <param name="index">处理下标</param>
        void DoPageChange(int index)
        {
            if (PageIndex != index)
                if (PageIndexChanged != null)
                {
                    PageIndexChanged(new PageControlArgs(index, PageSize));
                }
            PageIndex = index;
        }
        /// <summary>
        /// 控制按钮是否可用
        /// </summary>
        void ChangeButtonEnable()
        {
            this.btnFirst.IsEnabled = true;
            this.btnPrevious.IsEnabled = true;
            this.btnNext.IsEnabled = true;
            this.btnLast.IsEnabled = true;
            if (this.PageIndex == 0)
            {
                this.btnFirst.IsEnabled = false;
                this.btnPrevious.IsEnabled = false;
            }
            if (this.PageCount == 0 || this.PageIndex == this.PageCount - 1)
            {
                this.btnNext.IsEnabled = false;
                this.btnLast.IsEnabled = false;
            }
        }
    }
    /// <summary>
    /// 定义翻页委托
    /// </summary>
    /// <param name="args">分页控件参数</param>
    public delegate void OnPageIndexChangedHandler(PageControlArgs args);
    /// <summary>
    /// 分页控件参数
    /// </summary>
    public class PageControlArgs : EventArgs
    {
        /// <summary>
        /// 分页控件参数
        /// </summary>
        /// <param name="index">当前页下标</param>
        /// <param name="size">每页显示记录数</param>
        public PageControlArgs(int index, int size)
            : base()
        {
            this.PageIndex = index;
            this.PageSize = size;
        }
        /// <summary>
        /// 当前第几页
        /// </summary>
        public int PageIndex
        {
            get;
            set;
        }
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
        /// ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("PageIndex[{0}]PageSize[{1}]", PageIndex, PageSize);
        }
    }
}
