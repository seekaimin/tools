using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Util.ServiceEngine.Core
{
    /// <summary>
    /// 提示信息
    /// </summary>
    internal class ToolTopMessage
    {
        /// <summary>
        /// 提示信息
        /// </summary>
        internal ToolTopMessage()
        {

        }
        private String _Title;
        /// <summary>
        /// 消息标题
        /// </summary>
        internal String Title
        {
            get { return _Title; }
            set { _Title = value; }
        }

        private string _Content;
        /// <summary>
        /// 消息内容
        /// </summary>
        internal string Content
        {
            get { return _Content; }
            set { _Content = value; }
        }

        private Int32 _Timeout = 5000;
        /// <summary>
        /// 消息默认显示时间(毫秒)
        /// 默认5000毫秒
        /// </summary>
        internal Int32 Timeout
        {
            get { return _Timeout; }
            set { _Timeout = value; }
        }
        private ToolTipIcon _balloonType = ToolTipIcon.Info;
        /// <summary>
        /// 显示消息的方式
        /// </summary>
        internal ToolTipIcon BalloonType
        {
            get { return _balloonType; }
            set { _balloonType = value; }
        }
    }
}
