using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Util.Common.Nets.Sync
{
    /// <summary>
    /// TCP服务简单控制类
    /// </summary>
    public class TCPServerDemo
    {
        private static TCPServerDemo _Ins = null;
        /// <summary>
        /// httpserver 单件
        /// </summary>
        public static TCPServerDemo Ins
        {
            get
            {
                if (_Ins == null)
                {
                    _Ins = new TCPServerDemo();
                }
                return TCPServerDemo._Ins;
            }
        }

        private TCPServer Server = null;
        /// <summary>
        /// 工作状态
        /// </summary>
        public bool Running
        {
            get
            {
                return this.Server == null ? false : this.Server.Running;
            }
        }

        /// <summary>
        /// 启动htp服务
        /// </summary>
        /// <param name="port">监听端口</param>
        /// <param name="handle">接收数据处理句柄</param>
        /// <param name="acceptcount">监听线程数</param>
        /// <param name="readcount">读取线程数</param>
        /// <param name="executecount">处理线程数</param>
        /// <param name="writecount">写线程数</param>
        public void Start(int port, TCPServerExecuteHandle handle, int acceptcount = 1, int readcount = 1, int executecount = 1, int writecount = 0)
        {
            if (this.Running)
            {
                return;
            }
            this.Stop();
            this.Server = new TCPServer(port);
            this.Server.AccpetThreadCount = acceptcount;
            this.Server.ReadThreadCount = readcount;
            this.Server.ExecuteThreadCount = executecount;
            this.Server.WriteThreadCount = writecount;
            this.Server.ExecuteHandle += handle;
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
