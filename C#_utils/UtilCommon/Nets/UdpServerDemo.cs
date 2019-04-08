using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using Util.Common.ExtensionHelper;

namespace Util.Common.Nets
{
    /// <summary>
    /// udp服务端
    /// </summary>
    public class UdpServerDemo : NetBase, IDisposable
    {
        /// <summary>
        /// 接收数据超时
        /// </summary>
        public int ReceiveTimeOut = 10000;
        /// <summary>
        /// 发送数据超时
        /// </summary>
        public int SendTimeOut = 10000;
        UdpClient _Listener = null;
        IPEndPoint _ReomteEndPoint = null;
        /// <summary>
        /// 远程地址
        /// </summary>
        public IPEndPoint ReomteEndPoint
        {
            get { return _ReomteEndPoint; }
            set { _ReomteEndPoint = value; }
        }
        /// <summary>
        /// 监听server
        /// </summary>
        public UdpClient Listener
        {
            get { return _Listener; }
            set { _Listener = value; }
        }
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="_local">本地网卡地址</param>
        /// <param name="_localport">本地端口</param>
        /// <param name="_server">服务器</param>
        /// <param name="_port">通讯端口</param>
        public UdpServerDemo(string _local, int _localport, string _server, int _port)
            : base(_local, _localport, _server, _port)
        {
        }
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="_server">服务器</param>
        /// <param name="_port">通讯端口</param>
        public UdpServerDemo(string _server, int _port)
            : base(_server, _port)
        {
        }

        protected override void Init()
        {
            base.Init();
        }
        public bool IsRunning { get; set; }
        public bool IsDisposed { get; set; }
        /// <summary>
        /// 开始
        /// </summary>
        /// <param name="backlog">监听个数</param>
        public void BeginStart(int backlog)
        {
            if (!IsRunning)
            {
                this.IsRunning = true;
                this.IsDisposed = false;
                ReomteEndPoint = new IPEndPoint(this.server.ToIPAdress(), this.port);
                this.Listener = new UdpClient();
                if (this.server.IsMulticaseAddress())
                {
                    Listener.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);
                    Listener.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
                    Listener.Client.Bind(new IPEndPoint(this.local, port));
                    IPAddress remote = this.server.ToIPAdress();
                    this.Listener.JoinMulticastGroup(remote);
                }
                else //if (this.server.GetAddress() > IPAddressHelper.MAX_MULTICASE_ADDRESS)
                {
                    Listener.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);
                    Listener.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
                    Listener.Client.Bind(new IPEndPoint(IPAddress.Any, this.port));
                }
                this.Listener.Client.ReceiveTimeout = ReceiveTimeOut;
                this.Listener.Client.SendTimeout = SendTimeOut;
                //如果这里写while(true) 则会不停挂起异步接收操作，直到占满缓冲区间或队列。会报“由于系统缓冲区空间不足或队列已满，不能执行套接字上的操作”的错
                this.Listener.BeginReceive(new AsyncCallback(AsyncReceive), this);
            }
        }
        protected virtual void AsyncReceive(IAsyncResult ar)
        {
            try
            {
                UdpServerDemo listener = (UdpServerDemo)ar.AsyncState;
                if (listener.IsRunning)
                {
                    listener.Listener.BeginReceive(new AsyncCallback(listener.AsyncReceive), listener);
                }
                IPEndPoint remote = listener.ReomteEndPoint;
                byte[] data = listener.Listener.EndReceive(ar, ref remote);
                if (listener.DoConnectedClient != null)
                {
                    listener.DoConnectedClient(listener, data);
                }
            }
            catch (Exception e)
            {
                //Console.WriteLine(e);
            }
        }

        public void Start(int backlog)
        {
            if (!IsRunning)
            {
                this.IsRunning = true;
                this.IsDisposed = false;
                ReomteEndPoint = new IPEndPoint(IPAddress.Any, this.port);
                if (this.server.IsMulticaseAddress())
                {
                    #region UDP端口重用
                    this.Listener = new UdpClient();
                    Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                    socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);
                    socket.Bind(new IPEndPoint(IPAddress.Any, port));
                    Listener.Client = socket;
                    ////或
                    //Listener.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);
                    //Listener.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
                    //Listener.Client.Bind(new IPEndPoint(IPAddress.Any, port));
                    #endregion

                    IPAddress remote = this.server.ToIPAdress();
                    ReomteEndPoint = new IPEndPoint(remote, this.port);
                    this.Listener.JoinMulticastGroup(remote);
                }
                else
                {
                    this.Listener = new UdpClient(this.port);
                }

                IPEndPoint remoteEndPoint = ReomteEndPoint;
                do
                {
                    try
                    {
                        byte[] data = this.Listener.Receive(ref remoteEndPoint);
                        if (DoConnectedClient != null)
                        {
                            DoConnectedClient(this, data);
                        }
                    }
                    catch (Exception ex)
                    {
                        IsRunning = false;
                        if (!(ex is ObjectDisposedException || ex is SocketException))
                            throw ex;
                    }
                } while (IsRunning);
            }
        }

        public event Action<UdpServerDemo, byte[]> DoConnectedClient;

        public void Dispose()
        {
            if (this.IsDisposed) return;
            this.IsDisposed = true;
            this.IsRunning = false;
            if (this.Listener != null)
            {
                try
                {
                    Listener.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
