using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using Dexin.Util.Common.ExtensionHelper;

namespace Dexin.Util.Common.Net.Sockets
{
    /// <summary>
    /// socket扩展类
    /// </summary>
    public static class SocketHelper
    {
        private static int _AvailableTimespan = 50;
        /// <summary>
        /// 获取或设置接收等待超时时间片,默认50ms
        /// </summary>
        public static int AvailableTimespan
        {
            get { return _AvailableTimespan; }
            set { _AvailableTimespan = value; }
        }

        private static int _AvailableRetray = 3;
        /// <summary>
        /// 获取或设置接收等待超时重试次数,默认为3次
        /// </summary>
        public static int AvailableRetray
        {
            get { return _AvailableRetray; }
            set { _AvailableRetray = value; }
        }


        /// <summary>
        /// 接收数据(带循环)
        /// 等待超时时间片,默认50ms
        /// 接收等待超时重试次数,默认为3次
        /// 可通过SocketHelper的相应属性配置等待时间和超时次数
        /// </summary>
        /// <param name="socket">接收数据套接字</param>
        /// <param name="receivebuffsize">缓存大小</param>
        /// <param name="socketflag">指定套接字的发送和接收行为</param>
        /// <returns></returns>
        public static byte[] Receive(this Socket socket, int receivebuffsize, SocketFlags socketflag = SocketFlags.None)
        {
            List<byte> result = new List<byte>();
            int retray = 0;
            do
            {
                byte[] temp = new byte[receivebuffsize];
                int size = socket.Receive(temp, socketflag);
                if (size > 0)
                {
                    if (size != temp.Length)
                        temp = temp.GetBytes(size);

                    result.AddRange(temp);
                }
                else
                {
                    while (true)
                    {
                        if (socket.Available == 0)
                        {
                            //超过重试次数后结束接受数据
                            if (retray > AvailableRetray)
                                break;

                            //重试次数内休眠指定时间后重试
                            Thread.Sleep(AvailableTimespan);
                            retray++;
                            continue;
                        }
                        else
                        {
                            retray = 0;//接收到数据，重置重试次数
                            break;
                        }
                    }
                }
            } while (socket.Available > 0);

            return result.ToArray();
        }
    }
}
