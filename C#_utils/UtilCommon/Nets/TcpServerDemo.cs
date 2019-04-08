using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using Util.Common.ExtensionHelper;
using System.Net;
using System.Threading;

namespace Util.Common.Nets
{
    /// <summary>
    /// 异步TCPServer
    /// </summary>
    public class TcpServerDemo : IDisposable
    {
        bool _IsDisposed = false;
        /// <summary>
        /// 是否处于运行状态
        /// </summary>
        public bool IsRunning { get; private set; }
        private TcpListener _Listener = null;
        private List<TcpClientDemo> _ClientPool = new List<TcpClientDemo>();

        /// <summary>
        /// 客户端池子
        /// </summary>
        protected List<TcpClientDemo> ClientPool
        {
            get { return _ClientPool; }
            set { _ClientPool = value; }
        }
        /// <summary>
        /// 监听服务端
        /// </summary>
        public TcpListener Listener
        {
            get { return _Listener; }
            set { _Listener = value; }
        }
        /// <summary>
        /// 是否已释放
        /// </summary>
        public bool IsDisposed
        {
            get { return _IsDisposed; }
            private set { _IsDisposed = value; }
        }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="host">本地节点</param>
        /// <param name="port">本地端口</param>
        public TcpServerDemo(string host, int port)
        {
            IPAddress local = host.IsIPAddress() ? host.ToIPAdress() : IPAddress.Any;
            this.Listener = new TcpListener(local, port);
        }


        /// <summary>
        /// 开始
        /// </summary>
        /// <param name="backlog">监听个数</param>
        public void Start(int backlog)
        {
            if (!IsRunning)
            {
                IsRunning = true;
                this.IsDisposed = false;
                this.Listener.Start();
                this.Listener.BeginAcceptTcpClient(new AsyncCallback(AcceptTcpClient), this);
            }
        }
        void AcceptTcpClient(IAsyncResult ar)
        {
            if (IsRunning)
            {
                TcpServerDemo listener = (TcpServerDemo)ar.AsyncState;
                TcpClient c = listener.Listener.EndAcceptTcpClient(ar);
                if (listener.IsRunning)
                {
                    listener.Listener.BeginAcceptTcpClient(new AsyncCallback(listener.AcceptTcpClient), listener);
                }
                TcpClientDemo client = new TcpClientDemo(c);

                if (listener.DoConnectedClient != null)
                {
                    listener.Add(client);
                    try
                    {
                        listener.DoConnectedClient(listener, client);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }





        /// <summary>
        /// 处理连接的客户端
        /// </summary>
        public event Action<TcpServerDemo, TcpClientDemo> DoConnectedClient;

        /// <summary>
        /// 添加客户端
        /// </summary>
        /// <param name="client">客户端</param>
        void Add(TcpClientDemo client)
        {
            lock (ClientPool)
            {
                var item = this.ClientPool.FirstOrDefault(p => p.Key.Equals(client.Key));
                if (item == null)
                {
                    this.ClientPool.Add(client);
                }
                else
                {
                    if (item.TcpClient.Connected == false)
                    {
                        this.Remove(item);
                    }
                    this.ClientPool.Add(client);
                }
            }
        }
        /// <summary>
        /// 移除客户端
        /// </summary>
        /// <param name="client">客户端</param>
        public void Remove(TcpClientDemo client)
        {
            lock (ClientPool)
            {
                var item = this.ClientPool.FirstOrDefault(p => p.Key.Equals(client.Key));
                if (item != null)
                {
                    item.Dispose();
                    this.ClientPool.Remove(item);
                }
            }
        }

        /// <summary>
        /// 释放所有客户端
        /// </summary>
        void DisposeClientPool()
        {
            lock (this.ClientPool)
            {
                while (ClientPool.Count > 0)
                {
                    try
                    {
                        ClientPool[0].Dispose();
                    }
                    catch (Exception e)
                    {
                    }
                    ClientPool.RemoveAt(0);
                }
            }
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }
            this.IsRunning = false;
            this.IsDisposed = true;
            DisposeClientPool();
            if (this.Listener != null)
            {
                try
                {
                    this.Listener.Stop();
                }
                catch (Exception ex)
                {
                }
            }
        }
    }


}
