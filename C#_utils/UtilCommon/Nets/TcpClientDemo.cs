using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using Util.Common.ExtensionHelper;
using System.Net;

namespace Util.Common.Nets
{
    /// <summary>
    /// TCPClient
    /// </summary>
    public class TcpClientDemo : IDisposable
    {
        /// <summary>
        /// 默认包长度
        /// </summary>
        private int default_package_size = 1024;
        private string _Key = "";
        TcpClient _TcpClient = null;
        /// <summary>
        /// 是否释放
        /// </summary>
        public bool IsDisposed { get; private set; }
        /// <summary>
        /// 通讯套接字
        /// </summary>
        public TcpClient TcpClient
        {
            get { return _TcpClient; }
            set { _TcpClient = value; }
        }
        /// <summary>
        /// remote address
        /// </summary>
        public string Key
        {
            get { return _Key; }
            private set { _Key = value; }
        }
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="client">连接对象</param>
        public TcpClientDemo(TcpClient client)
        {
            this.TcpClient = client;
            Init();
        }
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="server">服务器名称或IP</param>
        /// <param name="port">通讯端口</param>
        public TcpClientDemo(string server, int port)
        {
            this.TcpClient = new TcpClient();
            if (server.IsIPAddress())
            {
                IPEndPoint remote = new IPEndPoint(server.ToIPAdress(), port);
                this.TcpClient.Connect(remote);
            }
            Init();
        }
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="localip">绑定本地IP</param>
        /// <param name="localport">绑定本地端口</param>
        /// <param name="server">服务器名称或IP</param>
        /// <param name="port">通讯端口</param>
        public TcpClientDemo(string localip, int localport, string server, int port)
        {
            IPEndPoint local = new IPEndPoint(localip.ToIPAdress(), localport);
            this.TcpClient = new TcpClient(local);
            if (server.IsIPAddress())
            {
                IPEndPoint remote = new IPEndPoint(server.ToIPAdress(), port);
                this.TcpClient.Connect(remote);
            }
            Init();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        void Init()
        {
            IsDisposed = false;
            this.Key = this.TcpClient == null ? "" : this.TcpClient.Client.RemoteEndPoint.ToStr();
        }


        /// <summary>
        /// 获取网络访问基础数据流
        /// </summary>
        /// <returns>基础数据流</returns>
        public NetworkStream GetNetworkStream()
        {
            return this.TcpClient.GetStream();
        }
        /// <summary>
        /// 设置超时时间
        /// </summary>
        /// <param name="sendtimeout">发送超时</param>
        /// <param name="receivetimeout">接收超时</param>
        public void SetTimeOut(int sendtimeout, int receivetimeout)
        {
            this.TcpClient.SendTimeout = sendtimeout;
            this.TcpClient.ReceiveTimeout = receivetimeout;
        }
        /// <summary>
        /// 设置缓冲区
        /// </summary>
        /// <param name="sendbuffersize">发送缓冲区</param>
        /// <param name="receivebuffersize">接收缓冲区</param>
        public void SetBufferSize(int sendbuffersize, int receivebuffersize)
        {
            this.TcpClient.SendBufferSize = sendbuffersize;
            this.TcpClient.ReceiveBufferSize = receivebuffersize;
        }
        /// <summary>
        /// 自定义处理
        /// </summary>
        /// <param name="action">自定义处理</param>
        public T Do<T>(Func<NetworkStream, T> action)
        {
            NetworkStream stream = this.GetNetworkStream();
            return action(stream);
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns>bool true:发送成功;false:发送失败;</returns>
        public bool Send(byte[] data)
        {
            return this.Do<bool>((stream) =>
            {
                if (stream.CanWrite)
                {
                    stream.Write(data, 0, data.Length);
                    stream.Flush();
                    return true;
                }
                else
                {
                    return false;
                }
            });
        }

        /// <summary>
        /// 数据接收
        /// </summary>
        /// <param name="buffer">接收数据缓存</param>
        /// <param name="startIndex">起始下标</param>
        /// <returns>接收数据长度</returns>
        public int Receive(byte[] buffer, int startIndex = 0)
        {
            NetworkStream stream = this.GetNetworkStream();
            int size = stream.Read(buffer, startIndex, buffer.Length);
            return size;
        }

        /// <summary>
        /// 接收数据
        /// </summary>
        /// <returns></returns>
        [Obsolete("this method is obsolete; use method Receive(byte[] buffer)  instead")]
        public List<byte[]> Receives()
        {
            List<byte[]> result = new List<byte[]>();
            NetworkStream stream = this.GetNetworkStream();
            if (stream.CanRead)
            {
                do
                {
                    byte[] temp = new byte[default_package_size];
                    int size = stream.Read(temp, 0, temp.Length);
                    if (size < 0)
                    {
                        break;
                    }
                    if (size < default_package_size)
                    {
                        byte[] data = temp.GetBytes(size);
                        result.Add(data);
                    }
                    else
                    {
                        result.Add(temp);
                    }
                } while (stream.DataAvailable);
            }
            return result;
        }
        /// <summary>
        /// 接收数据
        /// </summary>
        /// <returns></returns>
        [Obsolete("this method is obsolete; use method Receive(byte[] buffer)  instead")]
        public byte[] Receive()
        {
            List<byte> result = new List<byte>();
            this.Receives().ForEach((d) =>
            {
                result.AddRange(d);
            });
            return result.ToArray();
        }
        /// <summary>
        /// 大数据接收数据
        /// </summary>
        /// <param name="action">处理接收数据   返回值为:是否继续接收数据</param>
        [Obsolete("this method is obsolete; use method Receive(byte[] buffer)  instead")]
        public void Receive(Func<byte[], bool> action)

        {
            do
            {
                byte[] data = this.Receive();
                bool flag = action(data);
                if (flag == false)
                {
                    break;
                }
            } while (true);
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
            IsDisposed = true;
            if (this.TcpClient != null)
            {
                try
                {
                    this.TcpClient.Close();
                }
                catch (Exception ex)
                {
                }
            }
        }
        /// <summary>
        /// tostring
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Key;
        }
    }
}
