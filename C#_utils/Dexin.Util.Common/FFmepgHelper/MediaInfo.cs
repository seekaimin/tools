using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Dexin.Util.Common.FFmepgHelper
{
    /// <summary>
    /// 多媒体信息
    /// </summary>
    public class MediaInfo
    {
        private string _Path;
        /// <summary>
        /// 文件路径
        /// </summary>
        public string Path
        {
            get
            {
                return _Path;
            }
            set
            {
                _Path = value;
            }
        }
        /// <summary>
        /// 时长
        /// </summary>
        public TimeSpan Duration { get; set; }
        /// <summary>
        /// 比特率
        /// </summary>
        public double BitRate { get; set; }
        /// <summary>
        /// 完整的音频信息
        /// </summary>
        public string RawAudioFormat { get; set; }
        /// <summary>
        /// 音频格式
        /// </summary>
        public string AudioFormat { get; set; }
        /// <summary>
        /// 采样率
        /// </summary>
        public int AudioSamplingRate { get; set; }
        /// <summary>
        /// 完整的视频信息
        /// </summary>
        public string RawVideoFormat { get; set; }
        /// <summary>
        /// 视频格式
        /// </summary>
        public string VideoFormat { get; set; }
        /// <summary>
        /// 视频高
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// 视频宽
        /// </summary>
        public int Width { get; set; }
       /// <summary>
       /// 完整的媒体信息
       /// </summary>
        public string RawInfo { get; set; }
        /// <summary>
        /// 指示是否用
        /// </summary>
        public bool infoGathered { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="path"></param>
        public MediaInfo(string path)
        {
            _Path = path;
            Initialize();
        }

        private void Initialize()
        {
            this.infoGathered = false;
            if (string.IsNullOrEmpty(_Path))
            {
                throw new Exception("Video file Path not set or empty.");
            }
            if (!File.Exists(_Path))
            {
                throw new Exception("The video file " + _Path + " does not exist.");
            }
        }
    }
}
