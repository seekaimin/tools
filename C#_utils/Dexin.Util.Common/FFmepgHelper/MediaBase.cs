using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace Dexin.Util.Common.FFmepgHelper
{
    /// <summary>
    /// ffmpeg工具类基类
    /// </summary>
    public class MediaBase
    {
        private static string _FFmpegPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ffmpeg.exe");
        /// <summary>
        /// ffmpeg文件地址
        /// </summary>
        protected static string FFmpegPath
        {
            get { return MediaBase._FFmpegPath; }
            set { MediaBase._FFmpegPath = value; }
        }

        #region 处理进程的输出信息
        static void proc_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
                Console.WriteLine(e.Data);
        }
        static void proc_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!String.IsNullOrEmpty(e.Data))
                Console.WriteLine(e.Data);
        }
        #endregion

        #region 获取多媒体信息
        /// <summary>
        /// 获取指定多媒体文件的信息
        /// </summary>
        /// <param name="mediaFile">多媒体文件位置</param>
        /// <returns>多媒体信息</returns>
        public static MediaInfo GetVideoInfo(string mediaFile)
        {
            string Params = string.Format("-i \"{0}\"", mediaFile);
            string output = RunProcess(Params);
            MediaInfo input = new MediaInfo(mediaFile);
            input.RawInfo = output;
            input.Duration = ExtractDuration(input.RawInfo);
            input.BitRate = ExtractBitrate(input.RawInfo);
            input.RawAudioFormat = ExtractRawAudioFormat(input.RawInfo);
            input.AudioFormat = ExtractAudioFormat(input.RawAudioFormat);
            input.AudioSamplingRate = ExtractAudioSamplingRate(input.RawAudioFormat);
            input.RawVideoFormat = ExtractRawVideoFormat(input.RawInfo);
            input.VideoFormat = ExtractVideoFormat(input.RawVideoFormat);
            input.Width = ExtractVideoWidth(input.RawInfo);
            input.Height = ExtractVideoHeight(input.RawInfo);
            input.infoGathered = true;
            return input;
        }

        private static TimeSpan ExtractDuration(string rawInfo)
        {
            TimeSpan t = new TimeSpan(0);
            Regex re = new Regex("[D|d]uration:.((\\d|:|\\.)*)", RegexOptions.Compiled);
            Match m = re.Match(rawInfo);

            if (m.Success)
            {
                string duration = m.Groups[1].Value;
                string[] timepieces = duration.Split(new char[] { ':', '.' });
                if (timepieces.Length == 4)
                {
                    t = new TimeSpan(0, Convert.ToInt16(timepieces[0]), Convert.ToInt16(timepieces[1]), Convert.ToInt16(timepieces[2]), Convert.ToInt16(timepieces[3]));
                }
            }

            return t;
        }
        private static double ExtractBitrate(string rawInfo)
        {
            Regex re = new Regex("[B|b]itrate:.((\\d|:)*)", RegexOptions.Compiled);
            Match m = re.Match(rawInfo);
            double kb = 0.0;
            if (m.Success)
            {
                Double.TryParse(m.Groups[1].Value, out kb);
            }
            return kb;
        }
        private static string ExtractRawAudioFormat(string rawInfo)
        {
            string a = string.Empty;
            Regex re = new Regex("[A|a]udio:.*", RegexOptions.Compiled);
            Match m = re.Match(rawInfo);
            if (m.Success)
            {
                a = m.Value;
            }
            return a.Replace("Audio: ", "");
        }
        private static string ExtractAudioFormat(string rawAudioFormat)
        {
            string[] parts = rawAudioFormat.Split(new string[] { ", " }, StringSplitOptions.None);
            return parts[0].Replace("Audio: ", "");
        }
        private static int ExtractAudioSamplingRate(string rawAudioFormat)
        {
            string[] parts = rawAudioFormat.Split(new string[] { ", " }, StringSplitOptions.None);
            if (parts.Length > 1)
                return int.Parse(parts[1].Replace(" Hz", ""));
            else
                return 0;
        }
        private static string ExtractRawVideoFormat(string rawInfo)
        {
            string v = string.Empty;
            Regex re = new Regex("[V|v]ideo:.*", RegexOptions.Compiled);
            Match m = re.Match(rawInfo);
            if (m.Success)
            {
                v = m.Value;
            }
            return v.Replace("Video: ", "");
            ;
        }
        private static string ExtractVideoFormat(string rawVideoFormat)
        {
            string[] parts = rawVideoFormat.Split(new string[] { ", " }, StringSplitOptions.None);
            return parts[0].Replace("Video: ", "");
        }
        private static int ExtractVideoWidth(string rawInfo)
        {
            int width = 0;
            Regex re = new Regex("(\\d{2,4})x(\\d{2,4})", RegexOptions.Compiled);
            Match m = re.Match(rawInfo);
            if (m.Success)
            {
                int.TryParse(m.Groups[1].Value, out width);
            }
            return width;
        }
        private static int ExtractVideoHeight(string rawInfo)
        {
            int height = 0;
            Regex re = new Regex("(\\d{2,4})x(\\d{2,4})", RegexOptions.Compiled);
            Match m = re.Match(rawInfo);
            if (m.Success)
            {
                int.TryParse(m.Groups[2].Value, out height);
            }
            return height;
        }
        #endregion

        /// <summary>
        /// 用ffmpeg执行指定参数的命令，并等待进程结束返回执行结果
        /// </summary>
        /// <param name="Parameters">执行参数</param>
        /// <returns></returns>
        public static string RunProcess(string Parameters)
        {
            if (!File.Exists(FFmpegPath))
                throw new FileNotFoundException("ffmpeg file not found");

            //create a process info
            ProcessStartInfo oInfo = new ProcessStartInfo(FFmpegPath, Parameters);
            oInfo.UseShellExecute = false;
            oInfo.CreateNoWindow = true;
            oInfo.RedirectStandardOutput = true;
            oInfo.RedirectStandardError = true;

            StringBuilder sbRes = new StringBuilder();
            //run the process
            using (Process proc = System.Diagnostics.Process.Start(oInfo))
            {
                //now put it in a string
                do
                {
                    string sout = proc.StandardError.ReadToEnd();
                    //Console.WriteLine(sout);
                    sbRes.Append(sout);
                }
                while (!proc.HasExited);
                proc.WaitForExit();
            }
            return sbRes.ToString();
        }

        /// <summary>
        /// 用ffmpeg执行指定参数的命令
        /// </summary>
        /// <param name="Parameters">执行参数</param>
        public static Process BeginRunProcess(string Parameters)
        {
            //create a process info
            ProcessStartInfo oInfo = new ProcessStartInfo(FFmpegPath, Parameters);
            oInfo.UseShellExecute = false;
            oInfo.CreateNoWindow = true;
            oInfo.RedirectStandardOutput = true;
            oInfo.RedirectStandardError = true;

            //run the process
            Process proc = System.Diagnostics.Process.Start(oInfo);
            proc.ErrorDataReceived += new DataReceivedEventHandler(proc_ErrorDataReceived);
            proc.OutputDataReceived += new DataReceivedEventHandler(proc_OutputDataReceived);

            proc.BeginOutputReadLine();
            proc.BeginErrorReadLine();

            return proc;
        }
    }
}
