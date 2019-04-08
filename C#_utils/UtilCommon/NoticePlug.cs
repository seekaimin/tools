using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Util.Common.ExtensionHelper;

namespace Util.Common
{
    /// <summary>
    /// 通知消息插件
    /// </summary>
    public class SendPlug : IDisposable
    {
        private SendPlug()
        {
        }
        private static SendPlug _Ins;
        /// <summary>
        /// 单件
        /// </summary>
        public static SendPlug Ins
        {
            get
            {
                if (_Ins == null)
                    _Ins = new SendPlug();
                return SendPlug._Ins;
            }
        }

        /// <summary>
        /// 开始
        /// </summary>
        public void Start()
        {
            IsDisposed = false;
            new Thread(new ThreadStart(Send)).Start();
            new Thread(new ThreadStart(ClearMessage)).Start();
        }
        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            this.Dispose();
        }
        private IPEndPoint _EndPoint = new IPEndPoint(IPAddress.Parse("224.2.2.2"), 5001);
        /// <summary>
        /// 默认地址  224.2.2.2:5001
        /// </summary>
        public IPEndPoint EndPoint
        {
            get { return _EndPoint; }
            set { _EndPoint = value; }
        }
        List<MyMessage> Messages = new List<MyMessage>();
        private bool _IsDisposed = false;
        /// <summary>
        /// 是否被释放
        /// </summary>
        public bool IsDisposed
        {
            get { return _IsDisposed; }
            private set { _IsDisposed = value; }
        }

        /// <summary>
        /// 清理消息队列
        /// </summary>
        void ClearMessage()
        {
            while (!IsDisposed)
            {
                lock (Messages)
                    Messages = Messages.Where(p => p.State).ToList();
                //每5秒清理一次消息队列
                Thread.Sleep(5000);
            }
        }
        MyMessage Get()
        {
            lock (Messages)
            {
                return Messages.OrderByDescending(p => p.Priority).FirstOrDefault(p => p.State);
            }
        }
        /// <summary>
        /// 发送报文
        /// </summary>
        void Send()
        {
            using (UdpClient udpSend = new UdpClient())
            {
                udpSend.Client.SendTimeout = 20000;
                while (!IsDisposed)
                {
                    MyMessage message = Get();
                    if (message == null)
                    {
                        //未发现需要发送的消息 停1秒继续
                        Thread.Sleep(1000);
                        continue;
                    }
                    byte[] buff = message.ToBuff();
                    udpSend.Send(buff, buff.Length, EndPoint);
                    message.State = false;
                    Thread.Sleep(100);
                }
            }
        }
        /// <summary>
        /// 添加消息
        /// </summary>
        /// <param name="buff">数据</param>
        /// <param name="priority">优先级 越大越高</param>
        public void AddMessage(byte[] buff, int priority)
        {
            lock (Messages)
                Messages.Add(new MyMessage(buff, priority));
        }
        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            IsDisposed = true;
            lock (Messages)
                Messages.Clear();
        }
    }
    /// <summary>
    /// 自定义消息
    /// </summary>
    public class MyMessage
    {
        private byte[] _data= new byte[0];
        private int _Priority = 0;
        bool _State = true;
        DateTime _BuildTime = DateTime.Now;
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="buff">数据</param>
        public MyMessage(byte[] buff)
        {
            int index = 0;
            BuildTime = buff.GetDateTime(ref index);
            Priority = buff.GetInt32(ref index);
            int length = buff.GetInt32(ref index);
            this.data = buff.GetBytes(length, ref index);
       }
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="buff">数据</param>
        /// <param name="priority">优先级 越大越高</param>
        public MyMessage(byte[] buff, int priority = 0)
        {
            this.data = buff;
            this.Priority = priority;
            this.BuildTime = DateTime.Now;
        }
        /// <summary>
        /// 消息流信息
        /// </summary>
        public byte[] data
        {
            get { return _data; }
            set { _data = value; }
        }
        /// <summary>
        /// 消息发送优先级
        /// </summary>
        public int Priority
        {
            get { return _Priority; }
            set { _Priority = value; }
        }
        /// <summary>
        /// 消息状态
        /// </summary>
        public bool State
        {
            get { return _State; }
            set { _State = value; }
        }
        /// <summary>
        /// 生成时间
        /// </summary>
        public DateTime BuildTime
        {
            get { return _BuildTime; }
            private set { _BuildTime = value; }
        }
        /// <summary>
        /// 转化为buff
        /// </summary>
        /// <returns></returns>
        public byte[] ToBuff()
        {
            byte[] result = new byte[15 + data.Length];
            int index = 0;
            result.CopyDateTime(BuildTime, ref index);
            result.Copy(Priority, ref index);
            result.Copy(data.Length, ref index);
            result.CopyBytes(data, ref index);
            return result;
        }

    }
}
