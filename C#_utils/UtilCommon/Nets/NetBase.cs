using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Util.Common.ExtensionHelper;

namespace Util.Common.Nets
{
    /// <summary>
    /// 网络基类
    /// </summary>
    public class NetBase
    {
        /// <summary>
        /// 本地网卡地址
        /// </summary>
        protected IPAddress local { get; set; }
        /// <summary>
        /// 本地端口
        /// </summary>
        protected int localport { get; set; }
        /// <summary>
        /// 服务器
        /// </summary>
        protected string server { get; set; }
        /// <summary>
        /// 通讯端口
        /// </summary>
        protected int port { get; set; }
        /// <summary>
        /// 构造
        /// </summary>
        public NetBase()
        {
            Init();
        }
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="_local">本地网卡地址</param>
        /// <param name="_localport">本地端口</param>
        /// <param name="_server">服务器</param>
        /// <param name="_port">通讯端口</param>
        public NetBase(string _local, int _localport, string _server, int _port)
        {
            this.local = _local.ToIPAdress();
            this.localport = _localport;
            this.server = _server;
            this.port = _port;
            Init();
        }
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="_server">服务器</param>
        /// <param name="_port">通讯端口</param>
        public NetBase(string _server, int _port)
        {
            this.local = IPAddress.Any;
            this.localport = 0;
            this.server = _server;
            this.port = _port;

            Init();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        protected virtual void Init() { }
    }
}
