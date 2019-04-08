using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Net.Sockets;
using System.Net;

namespace TsSenders
{
    /// <summary>
    /// 文件数据发送基类
    /// </summary>
    public class FileSenderBase : SenderBase
    {
        /// <summary>
        /// 设置码率  单位 M
        /// </summary>
        public decimal Rate { get; set; }
        /// <summary>
        /// 需要发送的包集合
        /// </summary>
        protected List<byte[]> Packages = new List<byte[]>();
        /// <summary>
        /// 最少发送的包个数
        /// </summary>
        protected int min_send_package_count { get; set; }
        public int calc_min_send_package_count { get; set; }
        /// <summary>
        /// 当前读取缓冲区的位置
        /// </summary>
        protected int Buffer_Index { get; set; }
        /// <summary>
        /// 缓冲区
        /// </summary>
        private byte[] Buffers = new byte[0];
        /// <summary>
        /// 最大缓存区大小  默认8K
        /// </summary>
        public const int MaxBufferSize = 8192;

        /// <summary>
        /// 总字节数/秒
        /// </summary>
        protected decimal bytes_bysecond { get; private set; }
        /// <summary>
        /// 包总个数/秒
        /// </summary>
        protected decimal package_total_count_bysecond { get; private set; }
        /// <summary>
        /// 总字节数/毫秒
        /// </summary>
        protected decimal bytes_bymillisecond { get; private set; }
        /// <summary>
        /// 包总个数/毫秒
        /// </summary>
        protected decimal package_total_count_bymillisecond { get; private set; }
        /// <summary>
        /// 设置码率
        /// </summary>
        /// <param name="rate">码率</param>
        public void SetRate(decimal rate)
        {
            if (rate < 0)
                throw new Exception(string.Format("rate {0} error!", rate));
            Rate = rate;

            //总字节数/秒
            bytes_bysecond = (rate * 1024 * 1024);
            //包总个数/秒
            package_total_count_bysecond = bytes_bysecond / package_size;
            if (package_total_count_bysecond < 1m) package_total_count_bysecond = 1m;
            //总字节数/毫秒
            bytes_bymillisecond = bytes_bysecond / 1000m;
            //包总个数/毫秒
            package_total_count_bymillisecond = bytes_bymillisecond / package_size;
            if (package_total_count_bymillisecond < 1m) package_total_count_bymillisecond = 1m;
            min_send_package_count = (int)((package_total_count_bymillisecond % 100) * 2);//(int)package_total_count_bymillisecond;
            if (min_send_package_count < 1) min_send_package_count = 1;
            //calc_min_send_package_count = min_send_package_count;
            total_package_count_bymillisecond = bytes_bymillisecond / package_size;
        }

        /// <summary>
        /// 缓冲区大小 默认为最大缓冲区的值
        /// </summary>
        public int BufferSize { get; set; }
        /// <summary>
        /// 加载数据到缓冲区
        /// </summary>
        /// <param name="stream">流</param>
        protected void LoadBuffers(Stream stream)
        {
            //重置计数器
            Buffer_Index = 0;
            //重置缓冲区大小
            BufferSize = MaxBufferSize;
            if (Buffers.Length == 0)
            {
                Buffers = new byte[BufferSize];
            }
            Array.Clear(Buffers, 0, Buffers.Length);
            if (stream.Length - stream.Position < MaxBufferSize)
            {
                BufferSize = (int)(stream.Length - stream.Position);
                if (BufferSize <= 0)
                {
                    BufferSize = 0;
                    return;
                }
            }
            byte[] temp = new byte[BufferSize];
            int flg = stream.Read(temp, 0, BufferSize);
            if (flg == 0)
            {
                BufferSize = 0;
            }
            Array.Copy(temp, Buffers, temp.Length);
        }
        /// <summary>
        /// 处理发送文件流
        /// </summary>
        /// <param name="stream">流</param>
        /// <param name="sendaction">发送代理</param>
        /// <param name="checkpackage">检核数据包代理</param>
        protected void DoStream(Stream stream, SendHandle sendaction, CheckPackageHandle checkpackage)
        {
            //线程休眠时间
            int sleep = 0;
            Buffer_Index = 0;
            StartTime = DateTime.Now;
            send_total_package_count = 0;
            LoadBuffers(stream);
            while (RunningState == RunningStates.Run && Buffer_Index < BufferSize)
            {
                while (Packages.Count < min_send_package_count)
                {
                    if (Buffer_Index + package_size + 1 > BufferSize)
                    {
                        //不够读取一个包 继续获取数据
                        stream.Position = stream.Position - (BufferSize - Buffer_Index);
                        LoadBuffers(stream);
                        if (BufferSize == 0)
                        {
                            //没有数据可以获取
                            RunningState = RunningStates.Stop;
                            break;
                        }
                    }
                    if (Buffers[Buffer_Index] != syns_byte)
                    {
                        //同步丢失
                        Buffer_Index++;
                        continue;
                    }
                    else if (BufferSize > package_size)
                    {
                        if (Buffers[Buffer_Index + package_size] != syns_byte)
                        {
                            //下一个包 同步丢失
                            Buffer_Index++;
                            continue;
                        }
                    }
                    byte[] temp = new byte[package_size];
                    Array.Copy(Buffers, Buffer_Index, temp, 0, package_size);
                    //int tempsleep = 0;
                    //if (checkpackage != null)
                    //{
                    //    tempsleep = checkpackage(temp);
                    //    if (tempsleep > sleep)
                    //    {
                    //        sleep = tempsleep;
                    //    }
                    //}
                    Packages.Add(temp);
                    Buffer_Index += package_size;
                }
                sendaction(Packages);
                send_total_package_count += Packages.Count;
                Packages.Clear();
                sleep = checkpackage(null);
                if (sleep > 0)
                {

                    min_send_package_count = calc_min_send_package_count;
                    Console.WriteLine("================{0:0}================", sleep);
                    Thread.Sleep(sleep);
                }
                else
                {
                    min_send_package_count++;
                }
            }
        }



        /// <summary>
        /// 加载数据到缓冲区
        /// </summary>
        /// <param name="stream">流</param>
        protected void LoadToBuffers(Stream stream)
        {
            stream.Position -= (BufferSize - Buffer_Index);
            //重置计数器
            Buffer_Index = 0;
            //重置缓冲区大小
            BufferSize = MaxBufferSize;
            if (Buffers.Length == 0)
            {
                Buffers = new byte[BufferSize];
            }
            Array.Clear(Buffers, 0, Buffers.Length);
            if (stream.Length - stream.Position < BufferSize)
            {
                BufferSize = (int)(stream.Length - stream.Position);
                if (BufferSize <= 0)
                {
                    BufferSize = 0;
                    return;
                }
            }
            byte[] temp = new byte[BufferSize];
            int flg = stream.Read(temp, 0, BufferSize);
            if (flg == 0)
            {
                BufferSize = 0;
            }
            Array.Copy(temp, Buffers, temp.Length);
        }
        /// <summary>
        /// 处理发送文件流
        /// </summary>
        /// <param name="stream">流</param>
        /// <param name="sendaction">发送代理</param>
        /// <param name="checkpackage">检核数据包代理</param>
        protected void DoStream(Stream stream, SendHandle sendaction)
        {
            //线程休眠时间
            int sleep = 0;
            Buffer_Index = 0;
            StartTime = DateTime.Now;
            send_total_package_count = 0;
            LoadBuffers(stream);
            while (RunningState == RunningStates.Run && Buffer_Index < BufferSize)
            {
                while (Packages.Count < min_send_package_count)
                {
                    if (Buffer_Index + package_size + 1 > BufferSize)
                    {
                        //不够读取一个包 继续获取数据
                        stream.Position = stream.Position - (BufferSize - Buffer_Index);
                        LoadBuffers(stream);
                        if (BufferSize == 0)
                        {
                            //没有数据可以获取
                            RunningState = RunningStates.Stop;
                            break;
                        }
                    }
                    if (Buffers[Buffer_Index] != syns_byte)
                    {
                        //同步丢失
                        Buffer_Index++;
                        continue;
                    }
                    else if (BufferSize > package_size)
                    {
                        if (Buffers[Buffer_Index + package_size] != syns_byte)
                        {
                            //下一个包 同步丢失
                            Buffer_Index++;
                            continue;
                        }
                    }
                    byte[] temp = new byte[package_size];
                    Array.Copy(Buffers, Buffer_Index, temp, 0, package_size);
                    if (Rate <= 0)
                    {
                        int tempsleep = 0;
                        tempsleep = CheckSendRate(temp);
                        if (tempsleep > sleep)
                        {
                            sleep = tempsleep;
                        }
                    }
                    Packages.Add(temp);
                    Buffer_Index += package_size;
                }
                sendaction(Packages);
                send_total_package_count += Packages.Count;
                Packages.Clear();
                if (Rate > 0)
                {
                    sleep = CheckThreadSleepTimeSpanWithRate();
                    if (sleep > 0)
                    {
                        min_send_package_count = calc_min_send_package_count;
                        Console.WriteLine("================{0:0}***********Rate:{1:0}================", sleep, Rate);
                        Thread.Sleep(sleep);
                    }
                    else
                    {
                        min_send_package_count++;
                    }
                }
                else if (sleep > 0)
                {
                    Console.WriteLine("================{0:0}================", sleep);
                    Thread.Sleep(sleep);
                }
            }
        }

        private int CheckSendRate(byte[] temp)
        {
            return 0;// throw new NotImplementedException();
        }

        /// <summary>
        /// 通过播放盒发送
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <param name="sendRate">发送速度</param>
        /// <param name="rate">码率</param>
        public virtual void Send(string filename, UdpClient client, IPEndPoint ipendpoint)
        {
            try
            {
                using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
                {
                    do
                    {
                        fs.Position = 0;
                        if (RunningState == RunningStates.Stop)
                        {
                            RunningState = RunningStates.Run;
                        }
                        DoStream(
                            fs,
                            (List<byte[]> packages) =>
                            {
                                SenderHelper.Send(client, ipendpoint, packages.ToArray());
                            });
                    } while (RunningState == RunningStates.Stop && Loop);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //throw ex;
            }
            finally
            {
                Dispose();
            }
        }
        /// <summary>
        /// 检核线程休眠时间
        /// </summary>
        /// <returns></returns>
        public int CheckThreadSleepTimeSpanWithRate()
        {
            //理论发送包个数
            decimal calc_package_count_bymillisecond = send_total_milliseconds * package_total_count_bymillisecond;
            if (calc_package_count_bymillisecond <= 0)
                return 0;
            decimal diff_package_count = (send_total_package_count - calc_package_count_bymillisecond);
            //实际相差毫秒数                                 
            Console.WriteLine("{0:0}/{1:0}--------相差(ms):{2:0}--------count:{3}------rate:{4:0.0000}K/s",
                            send_total_package_count,
                            calc_package_count_bymillisecond,
                            diff_package_count,
                            min_send_package_count,
                            (send_total_package_count * package_size * 1000) / (send_total_milliseconds * 1024m));
            //Console.WriteLine("================{0:0}================", diff_package_count);
            return (int)diff_package_count;
        }
    }
}
