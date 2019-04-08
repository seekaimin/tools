using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Util.WinForm.Entity
{
    /// <summary>
    /// 控件状态
    /// </summary>
    public enum ControlState
    {
        /// <summary>
        /// 控件默认时
        /// </summary>
        Normal = 1,
        /// <summary>
        /// 鼠标移入
        /// </summary>
        MouseOver = 2,
        /// <summary>
        /// 鼠标按下
        /// </summary>
        MouseDown = 3,
        /// <summary>
        /// 控件不可用
        /// </summary>
        Disable = 4,
    }
}
