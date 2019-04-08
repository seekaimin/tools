using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Speech.Synthesis;
using System.Reflection;
using System.Speech.AudioFormat;
using System.IO;
using System.Threading;
using Util.Common.ExtensionHelper;

namespace Util.Common
{
    /// <summary>
    /// TTS事件参数
    /// </summary>
    public class TTSEventArgs
    {
        private bool _Cancelled = false;
        private Exception _Error = null;
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="canceled">是否主动取消</param>
        /// <param name="ex">异常信息</param>
        public TTSEventArgs(bool canceled, Exception ex)
        {
            this.Cancelled = canceled;
            this.Error = ex;
        }
        /// <summary>
        /// 是否被取消
        /// </summary>
        public bool Cancelled
        {
            get { return _Cancelled; }
            private set { _Cancelled = value; }
        }
        /// <summary>
        /// 存储的异常
        /// </summary>
        public Exception Error
        {
            get { return _Error; }
            private set { _Error = value; }
        }
    }
    /// <summary>
    /// TTS信息
    /// </summary>
    public class TTS
    {
        /// <summary>
        /// 完成事件
        /// </summary>
        public event Action<object, TTSEventArgs> OnCompleted;
        private void Completed(TTSEventArgs args)
        {
            this.Running = false;
            if (this.OnCompleted != null)
            {
                this.OnCompleted(this, args);
            }
        }
        private SpeechSynthesizer speech = null;
        /// <summary>
        /// 工作状态
        /// </summary>
        public bool Running { get; private set; }
        /// <summary>
        /// 语音播放
        /// </summary>
        /// <param name="message">文本信息</param>
        /// <param name="volume">音量 0-100</param>
        /// <param name="rate">语速  (-10,10)</param>
        /// <param name="speaker">声音选择</param>
        /// <param name="samplesPreSecond">采样率</param>
        public void Speak(string message, int volume = 100, int rate = 0, string speaker = null, int samplesPreSecond = 48000)
        {
            this.init(volume, rate, speaker, samplesPreSecond);
            speech.SpeakCompleted += (o, e) =>
            {
                TTSEventArgs temp = new TTSEventArgs(e.Cancelled, e.Error);
                this.Completed(temp);
            };
            speech.SpeakAsync(message);//异步阅读
        }

        /// <summary>
        /// 取消操作
        /// </summary>
        public void Stop()
        {
            if (this.Running)
            {
                this.speech.SpeakAsyncCancelAll();
            }
        }
        /// <summary>
        /// 生成文件
        /// </summary>
        /// <param name="path">保存路径</param>
        /// <param name="message">文本信息</param>
        /// <param name="volume">音量 0-100</param>
        /// <param name="rate">语速</param>
        /// <param name="speaker">声音选择</param>
        /// <param name="samplesPreSecond">采样率</param>
        public void File(string path, string message, int volume = 100, int rate = 1, string speaker = null, int samplesPreSecond = 48000)
        {
            DateTime start = DateTime.Now;
            this.init(volume, rate, speaker, samplesPreSecond);
            Stream ret = new MemoryStream();
            var mi = speech.GetType().GetMethod("SetOutputStream", BindingFlags.Instance | BindingFlags.NonPublic);
            var fmt = new SpeechAudioFormatInfo(samplesPreSecond, AudioBitsPerSample.Sixteen, AudioChannel.Stereo);
            mi.Invoke(speech, new object[] { ret, fmt, true, true });
            speech.SpeakCompleted += (o, e) =>
            {
                TTSEventArgs temp = new TTSEventArgs(e.Cancelled, e.Error);
                if (e.Cancelled == false && e.Error == null)
                {
                    try
                    {
                        using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
                        {
                            ret.Position = 0;
                            byte[] buffer = new byte[1024 * 1024];
                            for (; ; )
                            {
                                int len = ret.Read(buffer, 0, buffer.Length);
                                if (len <= 0) { break; }
                                fs.Write(buffer, 0, len);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        temp = new TTSEventArgs(e.Cancelled, ex);
                    }
                }
                ret.DoDispose();
                this.Completed(temp);
                DateTime end = DateTime.Now;
                Console.WriteLine("耗时:{0}", (end - start).TotalSeconds);
            };
            speech.SpeakAsync(message);

        }

        /// <summary>
        /// 生成文件
        /// </summary>
        /// <param name="volume">音量 0-100</param>
        /// <param name="rate">语速</param>
        /// <param name="speaker">声音选择</param>
        /// <param name="samplesPreSecond">采样率</param>
        private void init(int volume, int rate, string speaker, int samplesPreSecond)
        {
            if (this.Running)
            {
                throw new Exception("正在运行请停止后重新开始!");
            }
            this.Running = true;
            this.speech = new SpeechSynthesizer();
            if (speaker != null)
            {
                speech.SelectVoice(speaker);
            }
            this.speech.Volume = volume;
            this.speech.Rate = rate;
        }

        /// <summary>
        /// 获取已安装的发生人名称
        /// </summary>
        /// <returns></returns>
        public static List<string> GetInstalledVoices()
        {
            SpeechSynthesizer speech = new SpeechSynthesizer();
            return speech.GetInstalledVoices().Select(p => p.VoiceInfo.Name).ToList();
        }
    }
}
