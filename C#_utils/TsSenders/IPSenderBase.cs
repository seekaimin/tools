using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace TsSenders
{

    /*
    /// <summary>
    /// IP发送器基类
    /// </summary>
    public abstract class IPSenderBase : FileSenderBase
    {
        private List<IPEndPoint> _Remotes = new List<IPEndPoint>();
        /// <summary>
        /// 远程地址
        /// </summary>
        public List<IPEndPoint> Remotes
        {
            get { return _Remotes; }
            set { _Remotes = value; }
        }
        private List<UdpClient> _UdpClients = new List<UdpClient>();
        /// <summary>
        /// 保存的UDP客户端s
        /// </summary>
        public List<UdpClient> UdpClients
        {
            get { return _UdpClients; }
            set { _UdpClients = value; }
        }
        /// <summary>
        /// 以IP的方式发送数据
        /// </summary>
        /// <param name="packages"></param>
        public override void Send(List<byte[]> packages)
        {
            if (UdpClients == null || UdpClients.Count == 0) return;
            if (packages == null || packages.Count == 0) return;
            packages.ForEach((package) =>
            {
                UdpClients.ForEach((client) =>
                {
                    Remotes.ForEach((remote) =>
                    {
                        client.Send(package, package.Length, remote);
                    });
                });
            });
        }
        /// <summary>
        /// 以IP的方式发送数据
        /// </summary>
        /// <param name="packages">需要发送的包</param>
        /// <param name="client">socket</param>
        /// <param name="remote">远程地址</param>
        public override void Send(List<byte[]> packages, UdpClient client, IPEndPoint remote)
        {
            if (UdpClients == null || UdpClients.Count == 0) return;
            if (packages == null || packages.Count == 0) return;
            packages.ForEach((package) =>
            {
                client.Send(package, package.Length, remote);
            });
        }

    }
    /// <summary>
    /// 指定码率发送
    /// </summary>
    public class IPWithRateSender : IPSenderBase
    {
        public void Send(byte[] buffs, decimal defaultrate, string ip, int port)
        {
            //总字节数/秒
            bytes_bysecond = (defaultrate * 1024 * 1024);
            //包总个数/秒
            package_total_count_bysecond = bytes_bysecond / package_size;

            //总字节数/毫秒
            bytes_bymillisecond = bytes_bysecond / 1000m;
            //包总个数/毫秒
            package_total_count_bymillisecond = bytes_bymillisecond / package_size;

            Console.WriteLine("1毫秒    总字节数:{0}/每毫秒包个数:{1}", bytes_bymillisecond, package_total_count_bymillisecond);
            Console.ReadLine();
            List<byte[]> packages = new List<byte[]>();
            #region =========================读取所有包=========================
            for (int index = 0; index < buffs.Length; )
            {
                if (index + package_size >= buffs.Length)
                {
                    //读取数据完毕
                    break;
                }
                #region =========================检测同步字节=========================
                if (buffs[index] != syns_byte)
                {
                    //同步字节丢失
                    index++;
                    continue;
                }
                else if (buffs[index + package_size] != syns_byte)
                {
                    //同步字节丢失
                    index++;
                    continue;
                }
                #endregion
                byte[] temp = new byte[package_size];
                Array.Copy(buffs, index, temp, 0, package_size);
                packages.Add(temp);
                index += package_size;
            }
            #endregion
            using (UdpClient client = new UdpClient())
            {
                int s_count = (int)((package_total_count_bymillisecond % 100) * 5);
                int sendcount = s_count;// (int)package_total_count_bymillisecond;
                client.Client.SendBufferSize = sendcount * package_size;
                DateTime starttime = DateTime.Now;
                List<byte> send = new List<byte>();
                IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse(ip), port);
                for (int i = 0; i < packages.Count; i++)
                {
                    if (RunningState == RunningStates.Stop)
                        break;
                    send.AddRange(packages[i]);
                    if (send.Count / package_size < sendcount)
                        continue;
                    byte[] ttt = send.ToArray();
                    int p_size = 10 * 188;
                    for (int j = 0; j < send.Count; j += 10 * 188)
                    {
                        int size = j + p_size > send.Count - 1 ? send.Count - j - 1 : p_size;
                        byte[] te = new byte[size];
                        Array.Copy(ttt, j, te, 0, size);
                        client.Send(te, te.Length, endpoint);
                    }
                    //实际发送的包总个数
                    decimal send_total_package_count = (i + 1);
                    //发送的总时间(ms)
                    decimal send_total_milliseconds = (decimal)(DateTime.Now - starttime).TotalMilliseconds;
                    if (send_total_milliseconds <= 0)
                        continue;
                    //理论发送包个数
                    decimal calc_package_count_bymillisecond = send_total_milliseconds * package_total_count_bymillisecond;

                    decimal diff_package_count = (send_total_package_count - calc_package_count_bymillisecond);
                    //实际相差毫秒数         
                    decimal sleepmilliseconds = diff_package_count;
                    Console.WriteLine("{0:0}/{1:0}--------相差(ms):{2:0}--------count:{3}------rate:{4:0.0000}K/s",
                        send_total_package_count,
                        calc_package_count_bymillisecond,
                        diff_package_count,
                        sendcount,
                        (send_total_package_count * package_size * 1000) / (send_total_milliseconds * 1024m));
                    Console.WriteLine("================{0:0}================", sleepmilliseconds);

                    //sleepmilliseconds -= (int)(sleepmilliseconds * 0.5m);
                    if (sleepmilliseconds > 0)
                    {
                        Thread.Sleep((int)(sleepmilliseconds));
                        sendcount = s_count;// (int)package_total_count_bymillisecond;
                    }
                    else
                    {
                        sendcount++;
                    }
                    send.Clear();
                }
            }
        }
        public override void MyDispose()
        {

        }
        public override int CheckSendRate(byte[] package)
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
    /// <summary>
    /// 按PCR播放
    /// </summary>
    public class IPWithPcrSender : IPSenderBase
    {
        /// <summary>
        /// 允许PCR抖动范围 1000ms
        /// </summary>
        protected int max_pcr_wobble = 1000;
        /// <summary>
        /// 默认缓冲区大小 5K
        /// </summary>
        protected const int default_buff_size = 5 * 1024;
        private int _MaxBufferSize = 0;
        private int _MinCountOfOneTimes = 4;

        /// <summary>
        /// 需要处理的PID
        /// </summary>
        protected int pid;
        /// <summary>
        /// 第一个PCR
        /// </summary>
        protected long first_pcr;
        /// <summary>
        /// 获取第一个PCR的系统时间
        /// </summary>
        protected DateTime first_os_time;
        /// <summary>
        /// 当前获取的PCR
        /// </summary>
        protected long pcr;
        /// <summary>
        /// 保存的最后一次PCR
        /// </summary>
        protected long last_pcr;
        /// <summary>
        /// 保存的最后一次获取PCR的系统时间
        /// </summary>
        protected DateTime last_os_time = DateTime.Now;
        /// <summary>
        /// pcr抖动值 当前到开始
        /// </summary>
        protected long pcr_wobble
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
        protected long pcr_last_wobble
        {
            get
            {
                long pcr_diff = pcr - last_pcr;
                return (pcr_diff * 1000) / 27000000;
            }
        }

        /// <summary>
        /// 一次发送包最少个数
        /// </summary>
        public int MinCountOfOneTimes
        {
            get
            {
                if (_MinCountOfOneTimes < 1)
                {
                    _MinCountOfOneTimes = 1;
                }
                return _MinCountOfOneTimes;
            }
            set { _MinCountOfOneTimes = value; }
        }

        /// <summary>
        /// 缓存包个数
        /// </summary>
        protected List<byte[]> packages = new List<byte[]>();
        protected int index = 0;
        protected int sleep = 0;
        byte[] _packetBuffer;
        protected byte[] packetBuffer
        {
            get
            {
                if (_packetBuffer == null)
                { _packetBuffer = new byte[MaxBufferSize]; }
                return _packetBuffer;
            }
        }
        /// <summary>
        /// 通过网络发送数据
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <param name="ip">目的地址</param>
        /// <param name="port">目的端口</param>
        /// <param name="local">指定本地地址发送</param>
        public virtual void Send(string filename, string ip, int port, params string[] local)
        {
            List<UdpClient> udpclients = new List<UdpClient>();
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse(ip), port);
            if (local == null || local.Length == 0)
            {
                UdpClient client = new UdpClient();
                client.Client.SendBufferSize = this.MinCountOfOneTimes * package_size;
                udpclients.Add(client);
            }
            else
            {
                foreach (var i in local)
                {
                    UdpClient client = new UdpClient(new IPEndPoint(IPAddress.Parse(i.ToString()), 0));
                    client.Client.SendBufferSize = this.MinCountOfOneTimes * package_size;
                    udpclients.Add(client);
                }
            }
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
                        DoTs(fs, new SendHandle(delegate(List<byte[]> packages)
                        {
                            foreach (UdpClient client in udpclients)
                            {
                                for (int i = 0; i < packages.Count; i++)
                                {
                                    client.Send(packages[i], packages[i].Length, endpoint);
                                }
                            }
                        }));
                    } while (RunningState == RunningStates.Stop && Loop);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0}\r\n{1}", ex.Message, ex.StackTrace);
                throw ex;
            }
            finally
            {
                foreach (UdpClient client in udpclients)
                {
                    client.Close();
                }
            }
        }
        /// <summary>
        /// 通过播放盒发送
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <param name="sendRate">发送速度</param>
        /// <param name="rate">码率</param>
        public virtual void Send(string filename, decimal sendRate, double rate = 2.0)
        {
            try
            {
                using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
                {
                    do
                    {
                        if (TsPlayer.IsOpen == false)
                        {
                            TsPlayer.OpenDevice(true);
                            TsPlayer.SetRate(rate);
                        }
                        fs.Position = 0;
                        if (RunningState == RunningStates.Stop)
                        {
                            RunningState = RunningStates.Run;
                        }
                        DoTs(fs, new SendHandle(delegate(List<byte[]> packages)
                        {
                            for (int i = 0; i < packages.Count; i++)
                            {
                                TsPlayer.WriteData(packages[i]);
                            }
                        }));
                    } while (RunningState == RunningStates.Stop && Loop);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
            finally
            {
                TsPlayer.CloseDevice();
            }
        }

        /// <summary>
        /// 处理发送数据包
        /// </summary>
        /// <param name="stream">流</param>
        /// <param name="sendRate">发送速度</param>
        /// <param name="doSend">发送代理</param>
        protected virtual void DoTs(Stream stream, SendHandle doSend)
        {
            index = 0;
            BufferSize = MaxBufferSize;
            LoadBuffer(stream);
            while (RunningState == RunningStates.Run && index < BufferSize)
            {
                sleep = 0;
                while (packages.Count < MinCountOfOneTimes)
                {
                    if (index + package_size + 1 > BufferSize)
                    {
                        //不够读取一个包 继续获取数据
                        stream.Position = stream.Position - (BufferSize - index);
                        LoadBuffer(stream);
                        if (BufferSize == 0)
                        {
                            //没有数据可以获取
                            RunningState = RunningStates.Stop;
                            break;
                        }
                    }
                    if (packetBuffer[index] != syns_byte)
                    {
                        //同步丢失
                        index++;
                        continue;
                    }
                    else if (BufferSize > package_size)
                    {
                        if (packetBuffer[index + package_size] != syns_byte)
                        {
                            //下一个包 同步丢失
                            index++;
                            continue;
                        }
                    }
                    byte[] temp = new byte[package_size];
                    Array.Copy(packetBuffer, index, temp, 0, package_size);
                    int tempsleep = CheckPackage(temp);
                    if (tempsleep > sleep)
                    {
                        sleep = tempsleep;
                    }
                    packages.Add(temp);
                    index += package_size;
                }
                if (sleep > 0)
                {
                    Thread.Sleep(sleep);
                }
                doSend(packages);
                packages.Clear();
            }
        }

        /// <summary>
        /// 处理检查包的pcr
        /// </summary>
        /// <param name="package">包数据</param>
        /// <returns>下一次发送需要等待的时间</returns>
        protected int CheckPackage(byte[] package)
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
        /// 加载数据到缓冲区
        /// </summary>
        /// <param name="stream">流</param>
        protected void LoadBuffer(Stream stream)
        {
            //重置计数器
            index = 0;
            Array.Clear(packetBuffer, 0, packetBuffer.Length);
            BufferSize = MaxBufferSize;
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
            Array.Copy(temp, 0, packetBuffer, 0, BufferSize);
        }
        /// <summary>
        /// 重置
        /// </summary>
        public void ReSet(DateTime _dtnow, int _pid, long _pcr)
        {
            //重置时间和PCR
            first_os_time = _dtnow;
            //保存开始的PCR
            first_pcr = _pcr;
            //需要保存的PID 
            pid = _pid;
        }

        public override int CheckSendRate(byte[] package)
        {
            throw new NotImplementedException();
        }
        public override void MyDispose()
        {
            throw new NotImplementedException();
        }
    }
    **/
}
