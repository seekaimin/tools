using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using Util.Common.ExtensionHelper;

namespace Util.Common.Nets.Sync
{
    /// <summary>
    /// TCP服务端
    /// </summary>
    public sealed class TCPServer : NetServerBase
    {
        /// <summary>
        /// 接收到数据执行句柄
        /// </summary>
        public event TCPServerExecuteHandle ExecuteHandle;
        private Socket ServerSocket { get; set; }
        /// <summary>
        /// 监听端口
        /// </summary>
        public int Port { get; private set; }
        private int _AccpetThreadCount = 1;
        private int _ReadThreadCount = 1;
        private int _ExecuteThreadCount = 1;
        private int _WriteThreadCount = 1;
        private int Count = 0;

        private int _Backlog = 50;
        /// <summary>
        /// 数据接收缓存
        /// </summary>
        public int ReceiveBufferSize { get; set; }


        /// <summary>
        /// 挂起连接队列的最大长度
        /// </summary>
        public int Backlog
        {
            get { return _Backlog; }
            set
            {
                if (value < 1)
                {
                    _Backlog = 1;
                }
                else
                {
                    _Backlog = value;
                }
            }
        }




        private DisposeQueuePool<ClientSocket> ReadPool = new DisposeQueuePool<ClientSocket>();
        private DisposeQueuePool<ClientSocket> ExecutePool = new DisposeQueuePool<ClientSocket>();
        private DisposeQueuePool<ClientSocket> WritePool = new DisposeQueuePool<ClientSocket>();


        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="port">服务监听端口</param>
        public TCPServer(int port)
        {
            Port = port;
            this.Running = false;
            this.ReceiveTimeout = 10;
            this.SendTimeout = 1000;
            this.ReceiveBufferSize = 1024;
        }

        /// <summary>
        /// 读取数据超时时间
        /// </summary>
        public int ReceiveTimeout { get; set; }
        /// <summary>
        /// 发送数据超时时间
        /// </summary>
        public int SendTimeout { get; set; }
        /// <summary>
        /// 监听线程数
        /// </summary>
        public int AccpetThreadCount
        {
            get { return _AccpetThreadCount; }
            set
            {
                if (value < 1)
                {
                    _AccpetThreadCount = 1;
                }
                else
                {
                    _AccpetThreadCount = value;
                }
            }
        }
        /// <summary>
        /// 读取线程
        /// </summary>
        public int ReadThreadCount
        {
            get { return _ReadThreadCount; }
            set
            {
                if (value < 1)
                {
                    _ReadThreadCount = 1;
                }
                else
                {
                    _ReadThreadCount = value;
                }
            }
        }
        /// <summary>
        /// 处理线程线程
        /// </summary>
        public int ExecuteThreadCount
        {
            get { return _ExecuteThreadCount; }
            set
            {
                if (value < 1)
                {
                    _ExecuteThreadCount = 1;
                }
                else
                {
                    _ExecuteThreadCount = value;
                }
            }
        }
        /// <summary>
        /// 写线程数量
        /// </summary>
        public int WriteThreadCount
        {
            get { return _WriteThreadCount; }
            set { _WriteThreadCount = value; }
        }

        /// <summary>
        /// 启动服务
        /// </summary>
        public override void Start()
        {
            if (this.Running)
            {
                return;
            }
            ReadPool.Reset();
            ExecutePool.Reset();
            ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            ServerSocket.Bind(new IPEndPoint(IPAddress.Any, this.Port));
            ServerSocket.Listen(this.Backlog);
            this.Running = true;
            new Thread(() =>
            {
                while (this.Running)
                {
                    Console.WriteLine("读取:{0}--执行:{1}--写:{2}---终端数量:{3}", ReadPool.Count(), ExecutePool.Count(), WritePool.Count(), this.Count);
                    Thread.Sleep(1000);
                }
            }).Start();
            #region 监听线程
            for (int i = 0; i < this.AccpetThreadCount; i++)
            {
                new Thread(() =>
                {
                    while (this.Running)
                    {
                        try
                        {
                            Socket client = ServerSocket.Accept();
                            client.ReceiveTimeout = this.ReceiveTimeout;
                            client.SendTimeout = this.SendTimeout;
                            ClientSocket temp = new ClientSocket(this.ReceiveBufferSize, client);
                            ReadPool.Set(temp);
                            this.AddClient();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("{0}-{1}", Thread.CurrentThread.Name, ex.Message);
                        }
                    }
                    Console.WriteLine("自定义线程退出 {0}", Thread.CurrentThread.Name);
                })
                {
                    Name = "监听线程-{0}".Fmt(i),
                }.Start();
            }
            #endregion
            #region 读取线程
            for (int i = 0; i < this.ReadThreadCount; i++)
            {
                new Thread(() =>
                {
                    while (this.Running)
                    {
                        ClientSocket client = null;
                        try
                        {
                            client = this.ReadPool.Get();
                            if (client != null)
                            {
                                bool flag = client.Read();
                                if (flag)
                                {
                                    this.ExecutePool.Set(client);
                                }
                                else
                                {
                                    this.ReadPool.Set(client);
                                }
                            }
                        }
                        catch (SocketException socex)
                        {
                            if (client != null)
                            {
                                if (this.Running && socex.ErrorCode == 10060)
                                {
                                    ReadPool.Set(client);
                                }
                                else
                                {
                                    this.RemoveClient(client);
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            client.DoDispose();
                            Console.WriteLine("{0}-{1}", Thread.CurrentThread.Name, ex.Message);
                        }
                    }
                    Console.WriteLine("自定义线程退出 {0}", Thread.CurrentThread.Name);
                })
                {
                    Name = "读取线程-{0}".Fmt(i)
                }
                .Start();
            }
            #endregion
            if (this.ExecuteHandle != null)
            {
                #region 处理线程
                for (int i = 0; i < this.ExecuteThreadCount; i++)
                {
                    new Thread(() =>
                    {
                        while (this.Running)
                        {
                            ClientSocket client = null;
                            try
                            {
                                client = this.ExecutePool.Get();
                                if (client != null)
                                {
                                    ExecuteHandle(client);
                                    if (this.WriteThreadCount > 0)
                                    {
                                        this.WritePool.Set(client);
                                    }
                                    else if (client.ClientSocketMode == ClientSocketModes.LONG)
                                    {
                                        this.ReadPool.Set(client);
                                    }
                                    else {
                                        this.RemoveClient(client);
                                    }
                                }
                            }
                            catch (SocketException socex)
                            {
                                if (client != null)
                                {
                                    if (this.Running && socex.ErrorCode == 10060)
                                    {
                                        this.ExecutePool.Set(client);
                                    }
                                    else
                                    {
                                        this.RemoveClient(client);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                this.RemoveClient(client);
                                Console.WriteLine("{0}-{1}", Thread.CurrentThread.Name, ex.Message);
                            }
                        }
                        lock (ExecutePool)
                        {
                            Monitor.PulseAll(ExecutePool);
                        }
                        lock (ReadPool)
                        {
                            Monitor.PulseAll(ReadPool);
                        }
                        Console.WriteLine("自定义线程退出 {0}", Thread.CurrentThread.Name);
                    })
                    {
                        Name = "处理线程-{0}".Fmt(i)
                    }
                    .Start();
                }
                #endregion

                #region 发送线程
                for (int i = 0; i < this.WriteThreadCount; i++)
                {
                    new Thread(() =>
                    {
                        while (this.Running)
                        {
                            ClientSocket client = null;
                            try
                            {
                                client = this.WritePool.Get();
                                if (client != null)
                                {
                                    if (client.ResponseData != null)
                                    {
                                        client.Socket.Send(client.ResponseData, client.SocketFlag);
                                    }
                                    if (client.ClientSocketMode == ClientSocketModes.LONG)
                                    {
                                        this.ReadPool.Set(client);
                                    }
                                    else
                                    {
                                        this.RemoveClient(client);
                                    }
                                }
                            }
                            catch (SocketException socex)
                            {
                                if (client != null)
                                {
                                    if (this.Running && socex.ErrorCode == 10060)
                                    {
                                        this.WritePool.Set(client);
                                    }
                                    else
                                    {
                                        this.RemoveClient(client);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                this.RemoveClient(client);
                                Console.WriteLine("{0}-{1}", Thread.CurrentThread.Name, ex.Message);
                            }
                        }
                        lock (ReadPool)
                        {
                            Monitor.PulseAll(ReadPool);
                        }
                        lock (WritePool)
                        {
                            Monitor.PulseAll(WritePool);
                        }
                        Console.WriteLine("自定义线程退出 {0}", Thread.CurrentThread.Name);
                    })
                    {
                        Name = "发送线程-{0}".Fmt(i)
                    }
                    .Start();
                }
                #endregion

            }
        }
        /// <summary>
        /// 停止服务
        /// </summary>
        public override void Stop()
        {
            this.DoDispose();
        }
        /// <summary>
        /// 资源释放
        /// </summary>
        public override void Dispose()
        {
            if (this.Running == false)
            {
                return;
            }
            this.Running = false;
            ServerSocket.DoDispose();
            ReadPool.Dispose();
            ExecutePool.Dispose();
            WritePool.Dispose();

            Count = 0;
        }
        static object countloc = new object();
        public void AddClient()
        {
            lock (countloc)
            {
                this.Count++;
            }
        }

        /// <summary>
        /// 移除客户端
        /// </summary>
        /// <param name="socket">套接字信息</param>
        public void RemoveClient(ClientSocket socket)
        {
            socket.DoDispose();
            lock (countloc)
            {
                this.Count--;
            }
        }
    }
    /// <summary>
    /// 定义TCP客户端操作句柄
    /// </summary>
    /// <param name="client">客户端</param>
    public delegate void TCPServerExecuteHandle(ClientSocket client);

}
