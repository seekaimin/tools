using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using Util.Common.ExtensionHelper;

namespace Util.Common.Nets.Async
{
    /// <summary>
    /// 异步TCP服务端
    /// </summary>
    public class ATCPServer
    {
        /// <summary>
        /// 服务停止触发代理
        /// </summary>
        public event Action ServiceStop;
        /// <summary>
        /// 出现异常触发代理
        /// </summary>
        public event Action<Exception> OnException;
        /// <summary>
        /// 接收到数据后触发代理
        /// </summary>
        public event Func<int, byte[], Socket, bool> Receive;

        /// <summary>
        /// 运行状态
        /// </summary>
        public bool Running { get; protected set; }
        Socket server = null;
        /// <summary>
        /// 启动
        /// </summary>
        /// <param name="port"></param>
        /// <param name="block"></param>
        public void Start(int port, int block = 50)
        {
            if (this.Running)
            {
                return;
            }
            if (server == null)
            {
                //定义IP地址
                IPEndPoint iep = new IPEndPoint(IPAddress.Any, port);
                //创建服务器的socket对象
                server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                server.Bind(iep);
                server.Listen(block);
            }
            this.Running = true;
            server.BeginAccept(AcceptCallback, server);
        }
        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            if (this.Running == false)
            {
                return;
            }
            this.Running = true;
            server.DoDispose();
        }
        private void AcceptCallback(IAsyncResult iar)
        {
            //还原传入的原始套接字
            Socket tempServer = (Socket)iar.AsyncState;
            //在原始套接字上调用EndAccept方法，返回新的套接字
            try
            {
                Socket client = tempServer.EndAccept(iar);
                if (this.Running)
                {
                    tempServer.BeginAccept(AcceptCallback, tempServer);
                }
                receive(client);
            }
            catch (Exception ex)
            {
                if (ServiceStop != null)
                {
                    ServiceStop();
                }
            }
        }
        private void receive(Socket client)
        {
            int size = 10;
            AsyncSocket temp = new AsyncSocket(size, client, this.Receive);
            if (this.OnException != null)
            {
                temp.OnException += this.OnException;
            }
            temp.AsyncReceive();
        }
    }

}
