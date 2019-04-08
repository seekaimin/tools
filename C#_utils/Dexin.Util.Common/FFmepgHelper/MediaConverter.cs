using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dexin.Util.Common.FFmepgHelper
{
    /// <summary>
    /// 多媒体文件转换类
    /// </summary>
    public class MediaConverter : MediaBase
    {
        /// <summary>
        /// 组织转换参数
        /// </summary>
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
        /// <param name="outputFile">文件输出路径</param>
        /// <returns>转换参数</returns>
        protected static string ConvertPara(string inputFile, int audioSamplingRate, int audioBitRate, string audioFormat, ushort nitID, ushort tsID, ushort serviceID, ushort pmtStartPID, ushort mpegtsStartPID, string serviceProvider, string serviceName, string outputFile)
        {
            StringBuilder sbPara = new StringBuilder();
            sbPara.Append(string.Format("-i \"{0}\"", inputFile));

            if (audioSamplingRate > 0)
                sbPara.Append(string.Format(" -ar {0}", audioSamplingRate));
            if (audioBitRate > 0)
                sbPara.Append(string.Format(" -ab {0}", audioBitRate * 1000));
            if (!string.IsNullOrEmpty(audioFormat))
                sbPara.Append(string.Format(" -acodec {0}", audioFormat));
            if (!(audioSamplingRate > 0 || audioBitRate > 0 || !string.IsNullOrEmpty(audioFormat)))
            {
                sbPara.Append(" -c copy");
            }

            if (nitID > 0)
                sbPara.Append(string.Format(" -mpegts_original_network_id {0}", nitID));
            if (tsID > 0)
                sbPara.Append(string.Format(" -mpegts_transport_stream_id {0}", tsID));
            if (serviceID > 0)
                sbPara.Append(string.Format(" -mpegts_service_id {0}", serviceID));
            if (pmtStartPID > 0)
                sbPara.Append(string.Format(" -mpegts_pmt_start_pid {0}", pmtStartPID));
            if (mpegtsStartPID > 0)
                sbPara.Append(string.Format(" -mpegts_start_pid {0}", mpegtsStartPID));
            if (!string.IsNullOrEmpty(serviceProvider))
                sbPara.Append(string.Format(" -metadata service_provider=\"{0}\"", serviceProvider));
            if (!string.IsNullOrEmpty(serviceName))
                sbPara.Append(string.Format(" -metadata service_name=\"{0}\"", serviceName));

            if (!string.IsNullOrEmpty(outputFile))
                sbPara.Append(string.Format(" -y \"{0}\"", outputFile));

            return sbPara.ToString();
        }

        /// <summary>
        /// 用ffmpeg按照指定参数转换指定文件
        /// </summary>
        /// <param name="inputFile">输入文件</param>
        /// <param name="audioSamplingRate">采样率</param>
        /// <param name="audioBitRate">比特率</param>
        /// <param name="audioFormat">音频格式</param>
        /// <param name="nitID">mpegts_original_network_id</param>
        /// <param name="tsID">mpegts_transport_stream_id</param>
        /// <param name="serviceID">mpegts_service_id</param>
        /// <param name="pmtStartPID">mpegts_pmt_start_pid(0x10~0x1F00)</param>
        /// <param name="mpegtsStartPID">mpegts_start_pid(0x20~0xF00)</param>
        /// <param name="serviceProvider">service_provider</param>
        /// <param name="serviceName">service_name</param>
        /// <param name="outputFile">文件输出路径</param>
        /// <returns>转换信息</returns>
        public static string Convert(string inputFile, int audioSamplingRate, int audioBitRate, string audioFormat, ushort nitID, ushort tsID, ushort serviceID, ushort pmtStartPID, ushort mpegtsStartPID, string serviceProvider, string serviceName, string outputFile)
        {
            string paras = ConvertPara(inputFile, audioSamplingRate, audioBitRate, audioFormat, nitID, tsID, serviceID, pmtStartPID, mpegtsStartPID, serviceProvider, serviceName, outputFile);

            return RunProcess(paras);
        }

        /// <summary>
        /// 异步方式开始用ffmpeg按照指定参数转换指定文件
        /// </summary>
        /// <param name="inputFile">输入文件</param>
        /// <param name="audioSamplingRate">采样率</param>
        /// <param name="audioBitRate">比特率</param>
        /// <param name="audioFormat">音频格式</param>
        /// <param name="nitID">mpegts_original_network_id</param>
        /// <param name="tsID">mpegts_transport_stream_id</param>
        /// <param name="serviceID">mpegts_service_id</param>
        /// <param name="pmtStartPID">mpegts_pmt_start_pid(0x10~0x1F00)</param>
        /// <param name="mpegtsStartPID">mpegts_start_pid(0x20~0xF00)</param>
        /// <param name="serviceProvider">service_provider</param>
        /// <param name="serviceName">service_name</param>
        /// <param name="outputFile">文件输出路径</param>
        public static void BeginConvert(string inputFile, int audioSamplingRate, int audioBitRate, string audioFormat, ushort nitID, ushort tsID, ushort serviceID, ushort pmtStartPID, ushort mpegtsStartPID, string serviceProvider, string serviceName, string outputFile)
        {
            string paras = ConvertPara(inputFile, audioSamplingRate, audioBitRate, audioFormat, nitID, tsID, serviceID, pmtStartPID, mpegtsStartPID, serviceProvider, serviceName, outputFile);

            BeginRunProcess(paras);
        }
    }
}
