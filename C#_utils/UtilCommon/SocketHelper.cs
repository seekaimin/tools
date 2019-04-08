using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using Util.Common.ExtensionHelper;
using System.Net;
using System.Threading;
using System.IO;
using Util.Common.Nets.Async;
using Util.Common.Nets;

namespace Util.Common
{
    /// <summary>
    /// socket扩展类
    /// </summary>
    public static class SocketHelper
    {
        /// <summary>
        /// 重试次数
        /// </summary>
        public static int Retry = 2;
        /// <summary>
        /// 当未收到数据等待时长 ms
        /// </summary>
        public static int timespan = 100;
        /// <summary>
        /// 接收数据(带循环)
        /// </summary>
        /// <param name="socket">接收数据套接字</param>
        /// <param name="receivebuffsize">缓存大小</param>
        /// <param name="socketflag">指定套接字的发送和接收行为</param>
        /// <returns></returns>
        public static byte[] Receive(this Socket socket, int receivebuffsize, SocketFlags socketflag = SocketFlags.None)
        {
            long size;
            int index = 0;
            List<byte[]> datas = socket.ReceiveLongData(receivebuffsize, socketflag, out size);
            byte[] result = new byte[size];
            foreach (byte[] data in datas)
            {
                result.CopyBytes(data, ref index);
            }
            return result;
        }
        /// <summary>
        /// 接收较长的数据(带循环)
        /// </summary>
        /// <param name="socket">接收数据套接字</param>
        /// <param name="receivebuffsize">缓存大小</param>
        /// <param name="socketflag">指定套接字的发送和接收行为</param>
        /// <param name="data_length">接收数据总长度</param>
        /// <returns></returns>
        public static List<byte[]> ReceiveLongData(this Socket socket, int receivebuffsize, SocketFlags socketflag, out long data_length)
        {
            data_length = 0;
            List<byte[]> result = new List<byte[]>();
            do
            {
                byte[] temp = new byte[receivebuffsize];
                int size = socket.Receive(temp, socketflag);
                if (size <= 0)
                {
                    break;
                }

                if (size == receivebuffsize)
                {
                    result.Add(temp);
                }
                else
                {
                    byte[] data = temp.GetBytes(size);
                    result.Add(data);
                }
                data_length += size;
            } while (socket.Available > 0);
            return result;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="socket">需要释放的对象</param>
        public static void DoDispose(this Socket socket)
        {
            if (socket == null)
            {
                return;
            }
            try
            {
                using (socket)
                {
                    socket.Close();
                    socket.Dispose();
                }
                socket = null;
            }
            catch// (Exception e)
            {

            }
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="dispose">需要释放的对象</param>
        public static void DoDispose(this IDisposable dispose)
        {
            if (dispose == null)
            {
                return;
            }
            try
            {
                dispose.Dispose();
            }
            catch (Exception e) { }
            finally
            {
                dispose = null;
            }
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="receive_data_length">接收数据总长度</param>
        /// <param name="receivebuffsize">缓存大小</param>
        /// <returns></returns>
        public static List<byte[]> GetResponseData(this HttpWebRequest request, int receivebuffsize, out long receive_data_length)
        {
            List<byte[]> result = new List<byte[]>();
            receive_data_length = 0;
            using (WebResponse response = request.GetResponse())
            {
                using (Stream data = response.GetResponseStream())
                {
                    do
                    {
                        byte[] buff = new byte[receivebuffsize];
                        int size = data.Read(buff, 0, receivebuffsize);
                        if (size <= 0)
                        {
                            break;
                        }
                        receive_data_length += size;
                        if (size < receivebuffsize)
                        {
                            result.Add(buff.GetBytes(size));
                        }
                        else
                        {
                            result.Add(buff);
                        }
                    } while (data.CanRead);
                }
            }
            return result;
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="receivebuffsize">缓存大小</param>
        /// <returns></returns>
        public static byte[] GetResponseData(this HttpWebRequest request, int receivebuffsize = 1024)
        {
            long length;
            int index = 0;
            List<byte[]> datas = GetResponseData(request, receivebuffsize, out length);
            byte[] result = new byte[length];
            foreach (byte[] data in datas)
            {
                result.CopyBytes(data, ref index);
            }
            return result;
        }


        public static void Do()
        {
            //1 创建套节字
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            s.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

            //2 绑定到 4567 端口
            IPEndPoint ipe = new IPEndPoint(IPAddress.Any, 5001);
            EndPoint ep = (EndPoint)ipe;
            s.Bind(ep);

            //3 加入多播组 234.5.6.7
            MulticastOption optionValue = new MulticastOption(IPAddress.Parse("224.2.2.2"));
            s.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, optionValue);

            //4 接收多播组数据
            Console.WriteLine("开始接收多播组 234.5.6.7 上的数据...");
            byte[] buffer = new byte[1024];
            while (true)
            {
                int nRet = s.ReceiveFrom(buffer, ref ep);
                if (nRet > 0)
                {
                    byte[] d = buffer.GetBytes(nRet);
                    string data = d.BytesToString(" ");
                    Console.WriteLine(data);
                }
            }
        }
    }
}
