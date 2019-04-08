using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace Util.Common.Nets
{
    /// <summary>
    /// 自定义客户端网络套接字
    /// </summary>
    public class ClientSocket : IDisposable
    {
        private int ReceiveBufferSize { get; set; }
        private SocketFlags _SocketFlag = SocketFlags.None;
        /// <summary>
        /// 是否需要关闭
        /// </summary>
        private ClientSocketModes _ClientSocketMode = ClientSocketModes.LONG;

        public ClientSocketModes ClientSocketMode
        {
            get { return _ClientSocketMode; }
            set { _ClientSocketMode = value; }
        }
        public byte[] TempBuffer = null;
        private byte[] _ResponseData = null;

        /// <summary>
        /// 资源释放标记
        /// </summary>
        public bool IsDisposed { get; private set; }
        /// <summary>
        /// 套接字
        /// </summary>
        public Socket Socket { get; private set; }


        /// <summary>
        /// 自定义数据缓存
        /// </summary>
        public List<byte> Buffers { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="receiveBufferSize"></param>
        /// <param name="socket"></param>
        public ClientSocket(int receiveBufferSize, Socket socket)
        {
            this.Socket = socket;
            this.ReceiveBufferSize = receiveBufferSize;
            this.TempBuffer = new byte[this.ReceiveBufferSize];
            this.Buffers = new List<byte>();
        }


        /// <summary>
        /// 临时保存接收数据长度
        /// </summary>
        public int TempSize { get; private set; }
        /// <summary>
        /// 临时存放接收到的数据
        /// </summary>

        /// <summary>
        /// 标记
        /// </summary>
        public SocketFlags SocketFlag
        {
            get { return _SocketFlag; }
            set { _SocketFlag = value; }
        }
        /// <summary>
        /// 数据读取
        /// </summary>
        /// <returns>是否成功读取数据</returns>
        public bool Read()
        {
            if (this.IsDisposed)
            {
                throw new Exception("套接字已经被释放");
            }
            this.TempSize = this.Socket.Receive(TempBuffer, this.SocketFlag);
            return this.TempSize > 0;
        }


        /// <summary>
        /// 响应数据
        /// </summary>
        public byte[] ResponseData
        {
            get { return _ResponseData; }
            set { _ResponseData = value; }
        }

        public int Write(byte[] data)
        {
            int size = this.Socket.Send(data, this.SocketFlag);
            return size;
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
            this.Buffers.Clear();
            this.Buffers = null;
            this.TempBuffer = null;
            this.ResponseData = null;
            this.DoDispose();
        }
    }

    /// <summary>
    /// 连接模式
    /// </summary>
    public enum ClientSocketModes
    {
        /// <summary>
        /// 
        /// </summary>
        SHORT = 1,
        /// <summary>
        /// 
        /// </summary>
        LONG,
    }
}
