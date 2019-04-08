using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using Util.Common.ExtensionHelper;
using System.Net;

namespace Util.Common.Nets
{
    public class UdpClientDemo : NetBase, IDisposable
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="_local">本地网卡地址</param>
        /// <param name="_localport">本地端口</param>
        /// <param name="_server">服务器</param>
        /// <param name="_port">通讯端口</param>
        public UdpClientDemo(string _local, int _localport, string _server, int _port)
            : base(_local, _localport, _server, _port)
        {
        }
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="_server">服务器</param>
        /// <param name="_port">通讯端口</param>
        public UdpClientDemo(string _server, int _port)
            : base(_server, _port)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        protected override void Init()
        {
            client = new UdpClient(0);
            if (this.server.IsIPAddress())
            {
                if (this.server.IsMulticaseAddress())
                {
                    IPAddress remote = this.server.ToIPAdress();
                    ReomteEndPoint = new IPEndPoint(remote, this.port);
                }
                else
                {
                    IPAddress remote = this.local;
                    ReomteEndPoint = new IPEndPoint(remote, this.port);
                }
            }
        }
        IPEndPoint ReomteEndPoint = null;
        UdpClient client = null;
        public int Send(byte[] data)
        {
            if (this.server.IsIPAddress())
            {
                return client.Send(data, data.Length, ReomteEndPoint);
            }
            else
            {
                return client.Send(data, data.Length, this.server, this.port);
            }
        }


        public void Dispose()
        {

        }
    }
}
