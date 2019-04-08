using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace Util.Common.Nets
{
    /// <summary>
    /// socket异步处理对象
    /// </summary>
    public class AsyncSocket : IDisposable
    {
        private event Func<int, byte[], Socket, bool> OnReceive;
        public event Action<Exception> OnException;
        public SocketError socketError = SocketError.Success;
        private SocketFlags _SocketFlag = SocketFlags.None;


        public AsyncSocket(int receiveBufferSize, Socket socket, Func<int, byte[], Socket, bool> receive)
        {
            this.ReceiveBufferSize = receiveBufferSize;
            this.Temp = new byte[this.ReceiveBufferSize];
            this.Packages = new List<byte>();
            this.Socket = socket;
            this.OnReceive = receive;
        }
        protected int ReceiveBufferSize { get; set; }
        public Socket Socket { get; protected set; }
        protected byte[] Temp { get; set; }
        public List<byte> Packages { get; set; }

        public void Dispose()
        {
            Packages.Clear();
            Socket.DoDispose();
        }
        public SocketFlags SocketFlag
        {
            get { return _SocketFlag; }
            set { _SocketFlag = value; }
        }
        public void AsyncReceive()
        {
            this.Socket.BeginReceive(this.Temp, 0, this.ReceiveBufferSize, this.SocketFlag, out this.socketError, AsyncReceiveCallback, this.Temp);
        }
        private void AsyncReceiveCallback(IAsyncResult ar)
        {
            try
            {
                int bytesRead = this.Socket.EndReceive(ar);
                if (bytesRead > 0)
                {
                    bool flag = this.OnReceive(bytesRead, this.Temp, this.Socket);
                    if (flag)
                    {
                        this.Socket.BeginReceive(this.Temp, 0, this.ReceiveBufferSize, this.SocketFlag, out this.socketError, AsyncReceiveCallback, this.Temp);
                    }
                    else
                    {
                        this.DoDispose();
                    }
                }
            }
            catch (Exception e)
            {
                if (this.OnException != null)
                {
                    this.OnException(e);
                }
            }
        }

    }
}
