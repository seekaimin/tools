using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Dexin.Util.Common.LogManager
{
    /// <summary>
    /// 不依赖于任何三方dll的方式提供记录日志的方法
    /// </summary>
    public class DirectLog
    {
        /// <summary>
        /// 线程同步锁
        /// </summary>
        private static Object locObject = new Object();


        private static string _LogDirectory;
        /// <summary>
        /// 日志主目录
        /// </summary>
        public static string LogDirectory
        {
            get
            {
                if (string.IsNullOrEmpty(_LogDirectory))
                {
                    _LogDirectory = AppDomain.CurrentDomain.BaseDirectory;
                }
                return _LogDirectory;
            }
            set
            {
                _LogDirectory = value;
            }
        }

        private static LogLevels _LogLevel = LogLevels.Error;
        /// <summary>
        /// 日志记录级别
        /// </summary>
        public static LogLevels LogLevel
        {
            get { return DirectLog._LogLevel; }
            set { DirectLog._LogLevel = value; }
        }

        /// <summary>
        /// 获取日志文件路径
        /// </summary>
        protected static string LogFilePath
        {
            get
            {
                lock (locObject)
                {
                    DateTime nowTime = DateTime.Now;

                    StringBuilder sbPath = new StringBuilder();
                    sbPath.Append(LogDirectory);
                    if (!Directory.Exists(sbPath.ToString()))
                    {
                        Directory.CreateDirectory(sbPath.ToString());
                    }
                    sbPath.Append("\\");
                    sbPath.Append(nowTime.ToString("yyyyMM"));
                    if (!Directory.Exists(sbPath.ToString()))
                    {
                        Directory.CreateDirectory(sbPath.ToString());
                    }

                    return string.Format("{0}\\{1}.txt"
                        , sbPath.ToString()
                        , nowTime.ToString("yyyy-MM-dd")
                    );
                }
            }
        }

        /// <summary>
        /// 向本地磁盘写入文本日志；
        /// 如果给定的写入级别比当前日志级别高级，将会忽略日志写入
        /// </summary>
        /// <param name="logMessage">日志内容</param>
        /// <param name="logLevel">日志级别</param>
        public static void WriteTXTLog(string logMessage, LogLevels logLevel = LogLevels.Error)
        {
            lock (locObject)
            {
                if (LogLevel < logLevel)
                    return;

                DateTime dt = DateTime.Now;
                try
                {
                    using (StreamWriter sw = new StreamWriter(LogFilePath, true, Encoding.UTF8))
                    {
                        sw.WriteLine(string.Format("[{0}][{1}] {2}"
                            , logLevel
                            , dt.ToString("yyyy-MM-dd HH:mm:ss.fff")
                            , logMessage));  //追加文本
                        sw.AutoFlush = true;
                    }
                }
                catch { }
            }
        }

        /// <summary>
        /// 向本地日志写入异常消息日志；
        /// 如果综合日志级别高于Warn：日志记录堆栈信息，否则只记录摘要信息
        /// </summary>
        /// <param name="ex">写入日志的异常消息</param>
        /// <param name="logLevl">日志级别</param>
        public static void WriteTXTLog(Exception ex, LogLevels logLevl = LogLevels.Debug)
        {
            string logMessage = ex.Message;
            if (logLevl > LogLevel && LogLevel < LogLevels.Warn)
            {
                WriteTXTLog(logMessage, LogLevel);
            }
            else
            {
                logMessage += ex.StackTrace;
                WriteTXTLog(logMessage, logLevl > LogLevel ? LogLevel : logLevl);
            }
        }
    }
}
