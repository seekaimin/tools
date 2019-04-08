using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Util.ServiceEngine
{
    /// <summary>
    /// 服务运行状态
    /// </summary>
    public enum ServiceStates
    {
        /// <summary>
        /// 未启动
        /// </summary>
        UnStarted = 0,
        /// <summary>
        /// 正在启动
        /// </summary>
        Starting = 1,
        /// <summary>
        /// 已启动
        /// </summary>
        Started = 2,
        /// <summary>
        /// 挂起
        /// </summary>
        Suspend = 4,
        /// <summary>
        /// 正在停止
        /// </summary>
        Stoping = 8,
        /// <summary>
        /// 已经停止
        /// </summary>
        Stoped = 16,
    }
    /// <summary>
    /// 显示图标类型
    /// </summary>
    public enum IconType
    {
        /// <summary>
        /// 不是标准图标
        /// </summary>
        None = 0,
        /// <summary>
        /// 信息图标
        /// </summary>
        Info = 1,
        /// <summary>
        /// 警告图标
        /// </summary>
        Warning = 2,
        /// <summary>
        /// 错误图标
        /// </summary>
        Error = 3,
    }
    /// <summary>
    /// 服务控制模式
    /// </summary>
    public enum ServiceControlModel
    {
        /// <summary>
        /// 启动
        /// </summary>
        Start = 1,
        /// <summary>
        /// 停止
        /// </summary>
        Stop = 2,
        /// <summary>
        /// 重启
        /// </summary>
        ReStart = 3,
    }
}
