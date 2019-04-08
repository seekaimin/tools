using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;

namespace Util.Common.DataSend
{
    public class UdpClientSend
    {
        public bool Runing { get; set; }
        public int Rate { get; set; }
        public UdpClient sender { get; set; }
        public int SendLength { get; set; }
        protected byte[] SendBuffer { get; set; }
        protected int SendPosition { get; set; }
        protected double Total { get; set; }
        protected DateTime StartTime { get; set; }


        public byte[] GetEmptyPackage()
        {
            return new byte[188];
        }
        public byte[] GetEmptyPackages(int count)
        {
            byte[] data = this.GetEmptyPackage();
            int size = data.Length;
            byte[] result = new byte[count * size];
            for (int i = 0; i < count; i++)
            {
                Array.Copy(data, 0, result, i * size, size);
            }
            return result;
        }
        protected void Start()
        {
            while (Runing)
            {
                if ((DateTime.Now - StartTime).TotalSeconds > 3)
                {
                    Total = 0;
                    StartTime = DateTime.Now;
                }
                DateTime end = DateTime.Now;
                double sjtime = (end - StartTime).TotalMilliseconds;
                double lltime = Total / Rate;
                if (sjtime > lltime)
                {
                    Array.Clear(SendBuffer, 0, SendLength);
                    //慢了
                }
                else
                {
                    int sleep = (int)((lltime - sjtime));
                    Thread.Sleep(sleep);
                }
            }
        }

        protected void Send()
        {
            /*
            int pos = 0;
            do
            {
                byte[] d = DataBuffer.Ins.Get();
                if (d.Length <= 0)
                {
                    d = package();
                    Array.Copy(d, 0, sendbuffer, pos, d.Length);
                    pos += d.Length;
                }
                else
                {
                    d = package(7);
                    Array.Copy(d, 0, sendbuffer, pos, d.Length-pos);
                    pos += d.Length;
                }
            } while (pos < 1316);
            pos = 0;
            udp.Send(sendbuffer, 1316, remote);
            total = (total + sendbuffer.Length);
        }
             * */
        }

    }
}
