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
    /// 日期和时间控件
    /// </summary>
    public partial class UCDateAndTime : UserControl
    {
        private DateTimeDisplayModel _DisPlayModel = DateTimeDisplayModel.All;


        /// <summary>
        /// 获取或设置控件的日期和时间
        /// </summary>
        [Category("自定义属性")]
        [Description("获取或设置控件的日期和时间")]
        public DateTime Value
        {
            get { return new DateTime(txtDate.Value.Year, txtDate.Value.Month, txtDate.Value.Day, txtTime.Value.Hour, txtTime.Value.Minute, txtTime.Value.Second); }
            set { txtDate.Value = value; txtTime.Value = value; }
        }
        /// <summary>
        /// 获取或设置控件的日期和时间
        /// </summary>
        [Category("自定义属性")]
        [Description("获取或设置控件的显示模式")]
        public DateTimeDisplayModel DisPlayModel
        {
            get { return _DisPlayModel; }
            set
            {
                _DisPlayModel = value;
                if (DisPlayModel == DateTimeDisplayModel.All || DisPlayModel == DateTimeDisplayModel.Date)
                {
                    txtDate.Visible = true;
                }
                else
                {
                    txtDate.Visible = false;
                }
                if (DisPlayModel == DateTimeDisplayModel.All || DisPlayModel == DateTimeDisplayModel.Date)
                {
                    txtTime.Visible = true;
                }
                else
                {
                    txtTime.Visible = false;
                }
            }
        }

        /// <summary>
        /// 日期和时间控件
        /// </summary>
        public UCDateAndTime()
        {
            InitializeComponent();
        }

        private void txtDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtTime.Focus();
            }
        }
    }

    /// <summary>
    /// 显示模式
    /// </summary>
    public enum DateTimeDisplayModel
    {
        //显示日期和时间
        All,
        /// <summary>
        /// 仅显示时间
        /// </summary>
        Date,
        /// <summary>
        /// 仅显示时间
        /// </summary>
        Time
    }
}
