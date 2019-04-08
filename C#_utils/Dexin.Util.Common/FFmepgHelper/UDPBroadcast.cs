using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Dexin.Util.Common.FFmepgHelper
{
    /// <summary>
    /// 使用ffmpeg推送UDP类
    /// </summary>
    public class UDPBroadcast : MediaConverter
    {
        /// <summary>
        /// 组织发送参数
        /// </summary>
        /// <param name="loops">推流的循环次数，-1：无限次循环</param>
        /// <param name="inputFile">输入文件</param>
        /// <param name="audioSamplingRate">采样率(Hz)</param>
        /// <param name="audioBitRate">比特率(kb/s)</param>
        /// <param name="audioFormat">音频格式</param>
        /// <param name="nitID">mpegts_original_network_id</param>
        /// <param name="tsID">mpegts_transport_stream_id</param>
        /// <param name="serviceID">mpegts_service_id</param>
        /// <param name="pmtStartPID">mpegts_pmt_start_pid(0x10~0x1F00)</param>
        /// <param name="mpegtsStartPID">mpegts_start_pid(0x20~0xF00)</param>
        /// <param name="serviceProvider">service_provider</param>
        /// <param name="serviceName">service_name</param>
        /// <param name="sendRate">发送码率(kb/s)</param>
        /// <param name="udpIPAddress">输出的组播地址</param>
        /// <param name="udpPort">输出组播地址端口</param>
        /// <param name="packetSize">每次发送数据大小</param>
        /// <param name="localIPAddress">使用本机的IP地址</param>
        /// <param name="localPort">使用本机的端口</param>
        /// <returns>发送参数</returns>
        protected static string SendPara(int loops, string inputFile, int audioSamplingRate, int audioBitRate, string audioFormat, ushort nitID, ushort tsID, ushort serviceID, ushort pmtStartPID, ushort mpegtsStartPID, string serviceProvider, string serviceName, int sendRate, string udpIPAddress, int udpPort, int packetSize, string localIPAddress, int localPort)
        {
            StringBuilder sbPara = new StringBuilder();
            sbPara.Append(string.Format("-re -stream_loop {0} ", loops));
            sbPara.Append(ConvertPara(inputFile, audioSamplingRate, audioBitRate, audioFormat, nitID, tsID, serviceID, pmtStartPID, mpegtsStartPID, serviceProvider, serviceName, null));
            sbPara.Append(" -f mpegts");
            sbPara.Append(string.Format(" -pkt_size {0}", packetSize));
            if (sendRate > 0)
                sbPara.Append(string.Format(" -muxrate {0}", sendRate * 1000));
            sbPara.Append(string.Format(" udp://{0}:{1}", udpIPAddress, udpPort));
            if (!string.IsNullOrEmpty(localIPAddress))
            {
                sbPara.Append(string.Format("?localaddr={0}", localIPAddress));
                if (localPort > 0)
                {
                    sbPara.Append(string.Format("&localport={0}", localPort));
                }
            }

            return sbPara.ToString();
        }

        /// <summary>
        /// 按照指定条件推送数据
        /// </summary>
        /// <param name="loops">推流的循环次数，-1：无限次循环</param>
        /// <param name="inputFile">输入文件</param>
        /// <param name="nitID">mpegts_original_network_id</param>
        /// <param name="tsID">mpegts_transport_stream_id</param>
        /// <param name="serviceID">mpegts_service_id</param>
        /// <param name="pmtStartPID">mpegts_pmt_start_pid(0x10~0x1F00)</param>
        /// <param name="mpegtsStartPID">mpegts_start_pid(0x20~0xF00)</param>
        /// <param name="serviceProvider">service_provider</param>
        /// <param name="serviceName">service_name</param>
        /// <param name="udpAddress">输出的组播地址</param>
        /// <param name="udpPort">输出组播地址端口</param>
        /// <param name="packetSize">每次发送数据大小</param>
        /// <param name="sendRate">发送码率(kb/s)</param>
        /// <param name="audioSamplingRate">采样率(Hz)</param>
        /// <param name="audioBitRate">比特率(kb/s)</param>
        /// <param name="audioFormat">音频格式</param>
        /// <param name="localIP">使用本机的IP地址</param>
        /// <param name="localPort">使用本机的端口</param>
        /// <returns>推送数据的进程</returns>
        public static Process BeginSend(int loops, string inputFile, ushort nitID, ushort tsID, ushort serviceID, ushort pmtStartPID, ushort mpegtsStartPID, string serviceProvider, string serviceName, string udpAddress, int udpPort, int packetSize, int sendRate = 0, int audioSamplingRate = 0, int audioBitRate = 0, string audioFormat = null, string localIP = null, int localPort = 0)
        {
            string param = SendPara(loops, inputFile, audioSamplingRate, audioBitRate, audioFormat, nitID, tsID, serviceID, pmtStartPID, mpegtsStartPID, serviceProvider, serviceName, sendRate, udpAddress, udpPort, packetSize, localIP, localPort);

            return BeginRunProcess(param);
        }
    }
}
