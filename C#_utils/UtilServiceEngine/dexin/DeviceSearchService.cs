using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Util.Common.Nets;

namespace Util.ServiceEngine.dexin
{
    /// <summary>
    /// 设备搜索服务
    /// </summary>
    public class DeviceSearchService : IDisposable
    {
        /// <summary>
        /// 接收数据超时
        /// </summary>
        public int ReceiveTimeOut = 10000;
        /// <summary>
        /// 发送数据超时
        /// </summary>
        public int SendTimeOut = 10000;
        private string remote = "255.255.255.255";
        private int port = 10001;
        UdpServerDemo server;
        public bool IsRunning { get; set; }
        public bool IsDisposed { get; set; }
        /// <summary>
        /// 处理接收事件
        /// </summary>
        public event Action<byte[]> DoReceive;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="remote">监听地址</param>
        /// <param name="port">监听端口</param>
        public DeviceSearchService(string remote, int port)
        {
            this.remote = remote;
            this.port = port;
        }
        /// <summary>
        /// 开启服务
        /// </summary>
        public void Start()
        {
            if (IsRunning) { return; }
            server = new UdpServerDemo(remote, port);
            server.ReceiveTimeOut = this.ReceiveTimeOut;
            server.SendTimeOut = this.SendTimeOut;
            if (DoReceive != null)
            {
                server.DoConnectedClient += new Action<UdpServerDemo, byte[]>(server_DoConnectedClient);
            }
            IsDisposed = false;
            IsRunning = true;
            server.BeginStart(5);
        }

        void server_DoConnectedClient(UdpServerDemo arg1, byte[] arg2)
        {
            if (DoReceive != null)
            {
                DoReceive(arg2);
            }
        }
        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            this.Dispose();
        }
        /// <summary>
        /// 释放对象
        /// </summary>
        public void Dispose()
        {
            if (IsDisposed)
            {
                return;
            } try
            {
                if (server != null)
                {
                    server.Dispose();
                }
            }
            catch (Exception e)
            {
            }
            finally
            {
                IsDisposed = true;
                IsRunning = false;
            }
        }
    }
}
