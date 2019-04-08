using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Util.Common.Nets.Sync
{
    /// <summary>
    /// 建议的http服务端
    /// </summary>
    public class HTTPServerDemo
    {
        private static HTTPServerDemo _Ins = null;
        /// <summary>
        /// httpserver 单件
        /// </summary>
        public static HTTPServerDemo Ins
        {
            get
            {
                if (_Ins == null)
                {
                    _Ins = new HTTPServerDemo();
                }
                return HTTPServerDemo._Ins;
            }
        }

        private HTTPServer Server = null;

        /// <summary>
        /// 启动htp服务
        /// </summary>
        /// <param name="port">监听端口</param>
        /// <param name="handle">接收数据处理句柄</param>
        public void Start(int port, HTTPServerExecuteHandle handle)
        {
            if (this.Server == null)
            {
                this.Server = new HTTPServer(port);
                this.Server.Handle += handle;
            }
            else if (this.Server.Running)
            {
                return;
            }
            else
            {
                this.Server.DoDispose();
                this.Server = new HTTPServer(port);
                this.Server.Handle += handle;
            }
            if (this.Server.Running)
            {
                return;
            }
            this.Server.Start();
        }
        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            this.Server.DoDispose();
        }


    }
}
