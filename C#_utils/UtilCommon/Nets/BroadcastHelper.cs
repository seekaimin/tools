using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Util.Common.Nets
{
    public class BroadcastHelper
    {
        private IPEndPoint iep1;
        private byte[] data;
        static bool flag = false;
        public void Start(int port,byte[] buffer)
        {
            flag = true;
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            //255.255.255.255
            iep1 = new IPEndPoint(IPAddress.Broadcast, port);
            string hostname = Dns.GetHostName();
            data = Encoding.ASCII.GetBytes(hostname);
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);
            Thread t = new Thread(()=> {
                while (flag)
                {
                    socket.SendTo(data, iep1);
                    Thread.Sleep(2000);
                }
            });
            t.Start();
        }
        public static void Stop()
        {
            flag = false;
        }
    }
}
