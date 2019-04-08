using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Util.Common.Nets
{
    /// <summary>
    /// 网络服务基类
    /// </summary>
    public abstract class NetServerBase : IDisposable
    {
        /// <summary>
        /// 运行状态
        /// </summary>
        public bool Running { get; protected set; }
        /// <summary>
        /// 服务启动
        /// </summary>
        public abstract void Start();
        /// <summary>
        /// 服务停止
        /// </summary>
        public abstract void Stop();
        /// <summary>
        /// 释放资源
        /// </summary>
        public abstract void Dispose();
    }
}
