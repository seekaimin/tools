﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace TsSenders
{
    /// <summary>
    /// 发送帮助类
    /// </summary>
    public class SenderHelper : IDisposable
    {
        private SenderHelper()
        {

        }
        static SenderHelper _Ins;
        /// <summary>
        /// 单件实例
        /// </summary>
        public static SenderHelper Ins
        {
            get
            {
                if (_Ins == null) _Ins = new SenderHelper();
                return SenderHelper._Ins;
            }
        }

        /// <summary>
        /// 发送停止后处理
        /// </summary>
        public event Action SenderStpoedHandller;
        /// <summary>
        /// 同步字节
        /// </summary>
        public byte syns_byte = 0x47;
        /// <summary>
        /// 数据包长度
        /// </summary>
        public int package_size = 188;
        /// <summary>
        /// 已发送的包个数
        /// </summary>
        public decimal send_packages_count { get; private set; }
        /// <summary>
        /// 每秒发送的包个数
        /// </summary>
        public decimal packages_count_bysecond { get; private set; }
        /// <summary>
        /// 默认发送包数量
        /// </summary>
        decimal default_send_packages_count = 7;
        /// <summary>
        /// 单次最多发送包个数
        /// </summary>
        decimal max_send_packages_count = 7m;

        /// <summary>
        /// 当前发送包个数
        /// </summary>
        decimal current_send_packages_count { get; set; }

        /// <summary>
        /// 开始发送时间
        /// </summary>
        public DateTime StartTime { get; private set; }
        /// <summary>
        /// 已发送的时间
        /// </summary>
        protected decimal send_total_seconds
        {
            get { return (decimal)(DateTime.Now - StartTime).TotalSeconds; }
        }
        /// <summary>
        /// 0:停止;1:正在运行
        /// </summary>
        public SenderModels SenderModel { get; set; }
        /// <summary>
        /// 循环
        /// </summary>
        public bool loop { get; set; }
        /// <summary>
        /// 设置码率
        /// </summary>
        public decimal Rate { get; set; }

        /// <summary>
        /// 当前缓冲区大小
        /// </summary>
        public int BufferSize { get; set; }
        /// <summary>
        /// 最大缓冲区大小   默认40个包
        /// </summary>
        public int MaxBufferSize = 7520;
        /// <summary>
        /// 已读取的当前缓冲区位置
        /// </summary>
        int Buffer_Position { get; set; }
        /// <summary>
        /// 缓冲区
        /// </summary>
        byte[] Buffers = new byte[0];

        public void Init(bool _loop, SenderModels _sendermodel)
        {
            loop = _loop;
            SenderModel = _sendermodel;
        }
        void SetRate(decimal _rate)
        {
            if (_rate < 0)
                throw new Exception(string.Format("rate {0} error!", _rate));
            Rate = _rate;
            //总字节数/秒
            decimal bytes_bysecond = Rate * 1024m * 1024m;
            packages_count_bysecond = bytes_bysecond / package_size;

            //总字节数/毫秒
            decimal bytes_bymillisecond = bytes_bysecond / 1000m;
            //发送包数/每次
            current_send_packages_count = default_send_packages_count;
            if (SenderModel == SenderModels.Plyer_Box)
            {
                CheckPlayer(Rate);
            }
        }
        /// <summary>
        /// 加载数据到缓冲区
        /// </summary>
        /// <param name="stream">流</param>
        protected void LoadToBuffers(Stream stream)
        {
            #region 重置标记
            //重置计数器
            Buffer_Position = 0;
            //重置缓冲区大小
            BufferSize = MaxBufferSize;
            if (Buffers.Length < MaxBufferSize)
            {
                Buffers = new byte[MaxBufferSize];
            }
            else
            {
                //清空缓冲区
                Array.Clear(Buffers, 0, Buffers.Length);
            }
            #endregion
            #region 填充数据到缓冲区
            if (stream.Length - stream.Position < MaxBufferSize)
            {
                //数据不够填充满最大缓冲区
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
            Array.Copy(temp, 0, Buffers, 0, BufferSize);
            #endregion
        }
        /// <summary>
        /// 当前状态
        /// </summary>
        public RunningStates RunningState { get; private set; }

        /// <summary>
        /// 处理发送文件流
        /// </summary>
        /// <param name="stream">流</param>
        /// <param name="sendaction">发送代理</param>
        /// <param name="checkpackage">检核数据包代理</param>
        protected void DoStreamWithRate(Stream stream, SendHandle sendaction)
        {
            //线程休眠时间
            int sleep = 0;
            Reset(MaxBufferSize);
            RunningState = RunningStates.Run;
            List<byte[]> Packages = new List<byte[]>();
            LoadToBuffers(stream);
            while (RunningState == RunningStates.Run && Buffer_Position < BufferSize)
            {
                while (Packages.Count < current_send_packages_count)
                {
                    #region 不够读取一个包 继续获取数据
                    if (Buffer_Position + package_size + 1 > BufferSize)
                    {
                        //不够读取一个包 继续获取数据
                        stream.Position = stream.Position - (BufferSize - Buffer_Position);
                        LoadToBuffers(stream);
                        if (BufferSize == 0)
                        {
                            //没有数据可以获取
                            RunningState = RunningStates.Stop;
                            break;
                        }
                    }
                    #endregion
                    #region 检核同步字节
                    if (Buffers[Buffer_Position] != syns_byte)
                    {
                        //同步丢失
                        Buffer_Position++;
                        continue;
                    }
                    else if (Buffer_Position + package_size > BufferSize)
                    {
                        if (Buffers[Buffer_Position + package_size] != syns_byte)
                        {
                            //下一个包 同步丢失
                            Buffer_Position++;
                            continue;
                        }
                    }
                    #endregion
                    #region 添加需要发送的包
                    byte[] temp = new byte[package_size];
                    Array.Copy(Buffers, Buffer_Position, temp, 0, package_size);
                    Packages.Add(temp);
                    #endregion
                    Buffer_Position += package_size;
                }
                sendaction(Packages);
                send_packages_count += Packages.Count;
                Packages.Clear();
                sleep = CheckThreadSleepTimeSpanWithRate();
                if (sleep > 0)
                {
                    if (current_send_packages_count > default_send_packages_count)
                        current_send_packages_count = default_send_packages_count;
                    Thread.Sleep(sleep);
                }
                else
                {
                    current_send_packages_count = current_send_packages_count > max_send_packages_count ? max_send_packages_count : current_send_packages_count + 1;
                }
            }
            RunningState = RunningStates.Stop;
        }

        /// <summary>
        /// 处理发送文件流
        /// </summary>
        /// <param name="stream">流</param>
        /// <param name="sendaction">发送代理</param>
        /// <param name="checkpackage">检核数据包代理</param>
        protected void DoStreamWithPCR(Stream stream, SendHandle sendaction)
        {
            //线程休眠时间
            int sleep = 0;
            BufferSize = 0;
            Buffer_Position = 0;
            send_packages_count = 0;
            RunningState = RunningStates.Run;
            StartTime = DateTime.Now;
            List<byte[]> Packages = new List<byte[]>();
            LoadToBuffers(stream);
            while (RunningState == RunningStates.Run && Buffer_Position < BufferSize)
            {
                while (Packages.Count < current_send_packages_count)
                {
                    #region 不够读取一个包 继续获取数据
                    if (Buffer_Position + package_size + 1 > BufferSize)
                    {
                        //不够读取一个包 继续获取数据
                        stream.Position = stream.Position - (BufferSize - Buffer_Position);
                        LoadToBuffers(stream);
                        if (BufferSize == 0)
                        {
                            //没有数据可以获取
                            RunningState = RunningStates.Stop;
                            break;
                        }
                    }
                    #endregion
                    #region 检核同步字节
                    if (Buffers[Buffer_Position] != syns_byte)
                    {
                        //同步丢失
                        Buffer_Position++;
                        continue;
                    }
                    else if (Buffer_Position + package_size > BufferSize)
                    {
                        if (Buffers[Buffer_Position + package_size] != syns_byte)
                        {
                            //下一个包 同步丢失
                            Buffer_Position++;
                            continue;
                        }
                    }
                    #endregion
                    #region 添加需要发送的包
                    byte[] temp = new byte[package_size];
                    Array.Copy(Buffers, Buffer_Position, temp, 0, package_size);
                    int tempsleep = CheckPackage(temp);
                    if (tempsleep > sleep)
                    {
                        sleep = tempsleep;
                    }
                    Packages.Add(temp);
                    #endregion
                    Buffer_Position += package_size;
                }
                sendaction(Packages);
                send_packages_count += Packages.Count;
                Packages.Clear();
                if (sleep > 0)
                {
                    Thread.Sleep(sleep);
                }
            }
        }
        /// <summary>
        /// 检核线程休眠时间
        /// </summary>
        /// <returns></returns>
        public int CheckThreadSleepTimeSpanWithRate()
        {
            //理论发送包个数/s
            decimal calc_package_count_bysecond = send_total_seconds * packages_count_bysecond;
            if (calc_package_count_bysecond <= 0)
                return 0;
            //相差包的总个数
            decimal diff_package_count = send_packages_count - calc_package_count_bysecond;
            //相差的毫秒数
            decimal diff_milliseconds_timespan = (diff_package_count / packages_count_bysecond) * 1000m;

            Console.WriteLine("===========sleep(ms):{0:0}   send count:{1:0}  rate: {2:0.0000}===========",
                diff_milliseconds_timespan,
                current_send_packages_count,
                (send_packages_count * package_size / 1024m / 1024m) / send_total_seconds
                );
            return (int)diff_milliseconds_timespan;
        }



        int pid;
        DateTime first_os_time;
        DateTime last_os_time;
        long pcr;
        long first_pcr;
        long last_pcr;
        /// <summary>
        /// pcr抖动值 当前到开始
        /// </summary>
        long pcr_wobble
        {
            get
            {
                long pcr_diff = pcr - first_pcr;
                return (pcr_diff * 1000) / 27000000;
            }
        }
        /// <summary>
        /// pcr抖动值 最后一次抖动
        /// </summary>
        long pcr_last_wobble
        {
            get
            {
                long pcr_diff = pcr - last_pcr;
                return (pcr_diff * 1000) / 27000000;
            }
        }
        /// <summary>
        /// 允许PCR抖动范围 1000ms
        /// </summary>
        long max_pcr_wobble = 1000;

        /// <summary>
        /// 处理检查包的pcr
        /// </summary>
        /// <param name="package">包数据</param>
        /// <returns>下一次发送需要等待的时间</returns>
        int CheckPackage(byte[] package)
        {
            int theadsleepinterval = 0;
            //是否有调整字段
            if ((package[3] & 0x20) == 0)
                return theadsleepinterval;
            //是否有PCR
            if ((package[5] & 0x10) == 0)
                return theadsleepinterval;
            //调整字段的长度是否>=7字节
            if (package[4] < 7)
                return theadsleepinterval;
            long baseHeader = 0;
            long exten = 0;
            int nPID = ((package[1] & 0x1f) << 8) | package[2];
            if (nPID == 0x1FFF)
            {
                return theadsleepinterval;
            }
            if (pid == 0)
            {
                //保存开始的系统时间:ms
                first_os_time = DateTime.Now;
                //第一次出现含有PCR的PID
                pid = nPID;
                //计算PCR
                baseHeader = (long)(((package[6] << 24) | (package[7] << 16) |
                    (package[8] << 8) | package[9]) & 0xffffffff);
                baseHeader = baseHeader << 1;
                baseHeader |= (byte)(package[10] >> 7);
                exten = ((package[10] & 0x1) << 8) | package[11];
                pcr = baseHeader * 300 + exten;
                //保存开始的PCR
                first_pcr = pcr;
            }
            else if (pid == nPID)//是我们计算PCR用的PID
            {
                //计算PCR
                baseHeader = ((package[6] << 24) | (package[7] << 16) |
                    (package[8] << 8) | package[9]) & 0xffffffff;
                baseHeader = baseHeader << 1;
                baseHeader |= (byte)(package[10] >> 7);
                exten = ((package[10] & 0x1) << 8) | package[11];
                pcr = baseHeader * 300 + exten;

                DateTime dtnow = DateTime.Now;
                //从开始发数据到现在的系统持续时间ms
                long ostimewobble = (long)(dtnow - first_os_time).TotalMilliseconds;
                //如果出现过暂停时需要减去所有暂停累计的时间
                //不然PCR时间不变而播放时间增大，导致后面计算认为发送太慢
                //nSysTimeDiff -= fPauseStatisticalTimePCR;
                //差值决定是否发送过快
                theadsleepinterval = (int)(pcr_wobble - ostimewobble);
                long lostimelastwobble = (long)(dtnow - last_os_time).TotalMilliseconds;
                Console.WriteLine("fPID:[{0}]\tpcr interval:[{1}]\tos interval:[{2}]\t[{3}]", pid, pcr_last_wobble, lostimelastwobble, theadsleepinterval);
                if (pcr_last_wobble < max_pcr_wobble * -1)
                {
                    Console.WriteLine("reset wobble little: {0}", theadsleepinterval);
                    ReSet(dtnow, 0, pcr);
                }
                //qtss_printf("nPCRDiff:%"_U64BITARG_" - nSysTimeDiff:%"_U64BITARG_" = %"_U64BITARG_"\n",
                //	nPCRDiff,nSysTimeDiff,fNextSendPacketsTime);
                if (theadsleepinterval < 0)
                {
                    //发送过慢,连续发送
                    theadsleepinterval = 0;
                }
                last_pcr = pcr;
                last_os_time = dtnow;
            }
            return theadsleepinterval;
        }
        /// <summary>
        /// 重置
        /// </summary>
        void ReSet(DateTime _dtnow, int _pid, long _pcr)
        {
            //重置时间和PCR
            first_os_time = _dtnow;
            //保存开始的PCR
            first_pcr = _pcr;
            //需要保存的PID 
            pid = _pid;
        }

        /// <summary>
        /// 检核播放盒是否正常
        /// </summary>
        /// <param name="rate">码率</param>
        internal static void CheckPlayer(decimal rate)
        {
            if (TsPlayer.IsOpen == false)
            {
                TsPlayer.OpenDevice(true);
            }
            TsPlayer.SetRate((double)rate);
        }
        internal static void Send(decimal Rate, params byte[][] packages)
        {
            if (packages == null || packages.Length == 0) return;
            CheckPlayer(Rate);
            packages.ToList().ForEach((package) =>
            {
                TsPlayer.WriteData(package);
            });
        }
        internal static void Send(UdpClient client, IPEndPoint ipendpoint, params byte[][] packages)
        {
            if (packages == null || packages.Length == 0) return;
            List<byte> d = new List<byte>();
            packages.ToList().ForEach((package) =>
            {
                d.AddRange(package);
            });
            client.Send(d.ToArray(), d.Count, ipendpoint);
        }


        /// <summary>
        /// 码流播放器发送
        /// </summary>
        /// <param name="filepath">文件路径</param>
        /// <param name="rate">码率</param>
        public void Send(string filepath, decimal rate)
        {
            Reset();
            thread = new Thread(() =>
            {
                using (FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read))
                {
                    do
                    {
                        if (RunningState == RunningStates.Stop)
                        {
                            RunningState = RunningStates.Run;
                        }
                        SetRate(rate);
                        fs.Position = 0;
                        DoStreamWithRate(fs,
                            (packages) =>
                            {
                                SenderHelper.Send(rate, packages.ToArray());
                            });
                    } while (RunningState == RunningStates.Stop && loop);
                    if (SenderStpoedHandller != null)
                        SenderStpoedHandller();
                }
            });
            thread.Start();
        }
        public void Send(string filepath, UdpClient client, IPEndPoint ipendpoint)
        {
            Reset();
            thread = new Thread(() =>
            {
                using (FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read))
                {
                    do
                    {
                        if (RunningState == RunningStates.Stop)
                        {
                            RunningState = RunningStates.Run;
                        }
                        DoStreamWithRate(fs,
                            (packages) =>
                            {
                                SenderHelper.Send(client, ipendpoint, packages.ToArray());
                            });
                    } while (RunningState == RunningStates.Stop && loop);
                }
            });
            thread.Start();
        }
        Thread thread;
        public void Send(string filepath, decimal rate, UdpClient client, IPEndPoint ipendpoint)
        {
            Reset();
            thread = new Thread(() =>
            {
                using (FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read))
                {
                    do
                    {
                        if (RunningState == RunningStates.Stop)
                        {
                            RunningState = RunningStates.Run;
                        }
                        SetRate(rate);
                        fs.Position = 0;
                        DoStreamWithRate(fs,
                            (packages) =>
                            {
                                SenderHelper.Send(client, ipendpoint, packages.ToArray());
                            });
                    } while (RunningState == RunningStates.Stop && loop);
                    if (SenderStpoedHandller != null)
                        SenderStpoedHandller();
                }
            })
            {
                Priority = ThreadPriority.Highest
            };
            thread.Start();
        }
        void Reset(int buffsize = 0)
        {
            BufferSize = buffsize;
            Buffer_Position = 0;
            send_packages_count = 0;
            StartTime = DateTime.Now;
        }
        public void Dispose()
        {
            RunningState = RunningStates.Stop;
            Reset();
            try
            {
                thread.Abort();
            }
            catch { }
            if (SenderStpoedHandller != null)
                SenderStpoedHandller();
        }
    }
    /// <summary>
    /// 运行状态
    /// </summary>
    public enum RunningStates
    {
        Stop = 0,
        Run,
    }
    /// <summary>
    /// 发送模式
    /// </summary>
    public enum SenderModels
    {
        /// <summary>
        /// 播放盒
        /// </summary>
        Plyer_Box = 0,
        /// <summary>
        /// IP方案
        /// </summary>
        IP,
    }
    /// <summary>
    /// 处理获取数据包
    /// </summary>
    /// <param name="packages">获取的数据包</param>
    /// <returns></returns>
    public delegate void SendHandle(List<byte[]> packages);
    /// <summary>
    /// 检核数据包
    /// </summary>
    /// <param name="package">需要检核的数据包</param>
    /// <returns>sleep time(ms)</returns>
    public delegate int CheckPackageHandle(byte[] package);


}