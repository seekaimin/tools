using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Util.Common.Nets
{

    /// <summary>
    /// 简单的tcpserver
    /// </summary>
    public class TcpServerHelper
    {
        static TcpServerDemo _Ins = null;
        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="host">绑定本地网卡</param>
        /// <param name="port">绑定通讯端口</param>
        /// <param name="blockfog">监听数量</param>
        /// <param name="doconnectedclient">处理连接客户端</param>
        public static void Start(string host, int port, int blockfog, Action<TcpClientDemo> doconnectedclient)
        {
            if (_Ins == null || _Ins.IsDisposed)
            {
                _Ins = new TcpServerDemo(host, port);
                _Ins.DoConnectedClient += new Action<TcpServerDemo, TcpClientDemo>((server, client) =>
                {
                    try
                    {
                        doconnectedclient(client);
                    }
                    catch (Exception ex)
                    {
                        if (ex != null)
                            Console.WriteLine(ex.StackTrace);
                        server.Remove(client);
                    }
                });
            }
            if (_Ins.IsRunning == false)
                _Ins.Start(blockfog);
        }
        /// <summary>
        /// 停止服务
        /// </summary>
        public static void Stop()
        {
            if (_Ins != null)
                _Ins.Dispose();
        }
    }
}
