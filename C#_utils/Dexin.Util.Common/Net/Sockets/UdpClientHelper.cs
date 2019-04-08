using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace Dexin.Util.Common.Net.Sockets
{
    /// <summary>
    /// UdpClient扩展类
    /// </summary>
    public static class UdpClientHelper
    {
        /// <summary>
        /// 接收数据(带循环)
        /// </summary>
        /// <param name="client">接收数据套接字</param>
        /// <param name="ipendpoint">udp接收地址</param>
        /// <returns></returns>
        public static byte[] Receive(this UdpClient client, IPEndPoint ipendpoint)
        {
            List<byte> result = new List<byte>();
            do
            {
                byte[] temp = client.Receive(ref ipendpoint);
                result.AddRange(temp);
            } while (client.Available > 0);
            return result.ToArray();
        }
    }
}
