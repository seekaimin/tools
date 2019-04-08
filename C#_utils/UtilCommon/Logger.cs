using System;
using System.IO;
using System.Text;
using System.Threading;

namespace Util.Common
{
    /// <summary>
    /// 日志处理类
    /// </summary>
    public class Logger : IDisposable
    {
        private Encoding _Encoding = Encoding.UTF8;
        /// <summary>
        /// 日志文件编码格式
        /// </summary>
        public Encoding Encoding
        {
            get { return _Encoding; }
            set { _Encoding = value; }
        }
        private string _ExtendName = "log";
        /// <summary>
        /// 扩展名 默认值  log
        /// </summary>
        public string ExtendName
        {
            get { return _ExtendName; }
            set { _ExtendName = value; }
        }


        /// <summary>
        /// 日志对象
        /// </summary>
        public Logger()
        {
        }
        /// <summary>
        /// 日志对象
        /// </summary>
        /// <param name="basedirectory">日志信息根目录</param>
        public Logger(string basedirectory)
        {
            BaseDirectory = basedirectory;
        }

        private string _BaseDirectory;
        /// <summary>
        /// 日志记录的目录
        /// </summary>
        public string BaseDirectory
        {
            get
            {
                if (string.IsNullOrEmpty(_BaseDirectory))
                {
                    _BaseDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
                }
                return _BaseDirectory;
            }
            private set { _BaseDirectory = value; }
        }
        /// <summary>
        /// 日志全路径名称
        /// </summary>
        public string FullPath
        {
            get
            {
                //App/Year/Month/Day.log
                DateTime dt = DateTime.Now;
                string path = Path.Combine(BaseDirectory, dt.Year.ToString(), dt.Month.ToString());
                if (!Directory.Exists(path))
                {
                    //目录不存在  建立目录
                    Directory.CreateDirectory(path);
                }
                //day
                return Path.Combine(path, string.Format("{0}.{1}", dt.Day, ExtendName));
            }
        }
        private LogLevels _CurrentLevel = LogLevels.Info;
        /// <summary>
        /// 指定记录日志的级别
        /// ps:当记录日志的级别大于当前设定级别就不会记录
        /// </summary>
        public LogLevels CurrentLevel
        {
            get { return _CurrentLevel; }
            set { _CurrentLevel = value; }
        }
        private object lockObject = new object();


        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="level">日志记录级别</param>
        /// <param name="message">需要记录的信息</param>
        public void Write(LogLevels level, string message)
        {
            if (level == LogLevels.Console)
            {
                Console.WriteLine(message);
                return;
            }
            bool console = ((int)level & (int)LogLevels.Console) == 1;
            if (console)
            {
                Console.WriteLine(message);
            }
            if (level > CurrentLevel)
            {
                //当记录日志的级别>设置的日志级别就不记录日志
                return;
            }
            DoWrite(() =>
            {
                try
                {
                    string threadinfo = string.IsNullOrEmpty(Thread.CurrentThread.Name) ? Thread.CurrentThread.ManagedThreadId.ToString() : Thread.CurrentThread.Name;
                    string temp = string.Format("[{0}][{1}][{2}] {3}"
                        , DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")
                        , threadinfo
                        , level
                        , message);
                    sw.WriteLine(temp);
                    sw.Flush();
                    //using (StreamWriter sw = new StreamWriter(FullPath, true, Encoding.UTF8))
                    //{
                    //    string temp = string.Format("[{0}][{1}][{2}] {3} {4}"
                    //        , DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")
                    //        , threadinfo
                    //        , level
                    //        , message, Environment.NewLine);
                    //    byte[] buffer = Encode.GetBytes(temp);
                    //    fs.Write(buffer, 0, buffer.Length);
                    //    fs.Flush();
                    //}
                }
                catch (Exception) { }
                finally { }
            });
        }
        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="level">日志记录级别</param>
        /// <param name="formatMessage">需要格式化的消息</param>
        /// <param name="paramaters">参数</param>
        public void Write(LogLevels level, string formatMessage, params object[] paramaters)
        {
            Write(level, string.Format(formatMessage, paramaters));
        }

        /// <summary>
        /// 记录日志 
        /// </summary>
        /// <param name="message">需要记录的信息</param>
        public void Info(string message)
        {
            Write(Common.LogLevels.Info, message);
        }
        /// <summary>
        /// 记录日志 
        /// </summary>
        /// <param name="format">需要记录的信息</param>
        /// <param name="paramaters">参数</param>
        public void Info(string format, params object[] paramaters)
        {
            Write(Common.LogLevels.Info, format, paramaters);
        }
        /// <summary>
        /// 控制台输出日志 
        /// </summary>
        /// <param name="format">需要记录的信息</param>
        /// <param name="paramaters">参数</param>
        public void console(string format, params object[] paramaters)
        {
            Write(Common.LogLevels.Console, format, paramaters);
        }

        /// <summary>
        /// 控制台输出日志 
        /// </summary>
        /// <param name="message">需要记录的信息</param>
        public void console(string message)
        {
            Write(Common.LogLevels.Console, message);
        }
        /// <summary>
        /// 记录日志 
        /// </summary>
        /// <param name="message">需要记录的信息</param>
        public void Debug(string message)
        {
            Write(Common.LogLevels.Debug, message);
        }
        /// <summary>
        /// 记录日志 
        /// </summary>
        /// <param name="format">需要记录的信息</param>
        /// <param name="paramaters">参数</param>
        public void Debug(string format, params object[] paramaters)
        {
            Write(Common.LogLevels.Debug, format, paramaters);
        }

        /// <summary>
        /// 记录日志 
        /// </summary>
        /// <param name="message">需要记录的信息</param>
        public void Trace(string message)
        {
            Write(Common.LogLevels.Trace, message);
        }
        /// <summary>
        /// 记录日志 
        /// </summary>
        /// <param name="format">需要记录的信息</param>
        /// <param name="paramaters">参数</param>
        public void Trace(string format, params object[] paramaters)
        {
            Write(Common.LogLevels.Trace, format, paramaters);
        }

        /// <summary>
        /// 记录日志 
        /// </summary>
        /// <param name="message">需要记录的信息</param>
        public void Warn(string message)
        {
            Write(Common.LogLevels.Warn, message);
        }
        /// <summary>
        /// 记录日志 
        /// </summary>
        /// <param name="format">需要记录的信息</param>
        /// <param name="paramaters">参数</param>
        public void Warn(string format, params object[] paramaters)
        {
            Write(Common.LogLevels.Warn, format, paramaters);
        }


        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="level">日志记录级别</param>
        /// <param name="ex">异常</param>
        public void Write(LogLevels level, Exception ex)
        {
            if (ex == null || level < LogLevels.Warn)
            {
                //当出现异常时  如果小于Warn  不记录
                return;
            }
            try
            {
                StringBuilder msg = new StringBuilder();
                if (ex != null)
                {
                    if (level < LogLevels.Trace)
                    {
                        msg.Append(ex.Message);
                    }
                    else
                    {
                        msg.Append(string.Format("{0}" + Environment.NewLine + "{1}", ex.Message, ex.StackTrace));
                    }
                }
                Write(level, msg.ToString());
            }
            catch (Exception) { }
        }
        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="level">日志记录级别</param>
        /// <param name="ex">异常</param>
        /// <param name="message">需要记录的信息</param>
        public void Write(LogLevels level, Exception ex, string message)
        {
            if (level < LogLevels.Warn)
            {
                //当出现异常时  如果小于Warn  不记录
                return;
            }
            try
            {
                StringBuilder msg = new StringBuilder();
                msg.Append(message);
                msg.Append(Environment.NewLine);
                if (ex != null)
                {
                    if (level < LogLevels.Trace)
                    {
                        msg.Append(ex.Message);
                    }
                    else
                    {
                        msg.Append(string.Format("{0}" + Environment.NewLine + "{1}", ex.Message, ex.StackTrace));
                    }
                }
                Write(level, msg.ToString());
            }
            catch (Exception) { }
        }
        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="level">日志记录级别</param>
        /// <param name="ex">异常</param>
        /// <param name="formatString">需要格式化的信息</param>
        /// <param name="paramaters">参数</param>
        public void Write(LogLevels level, Exception ex, string formatString, params object[] paramaters)
        {
            if (level < LogLevels.Warn)
            {
                //当出现异常时  如果小于Warn  不记录
                return;
            }
            try
            {
                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format(formatString, paramaters));
                msg.Append(Environment.NewLine);
                if (ex != null)
                {
                    if (level < LogLevels.Trace)
                    {
                        msg.Append(ex.Message);
                    }
                    else
                    {
                        msg.Append(string.Format("{0}" + Environment.NewLine + "{1}", ex.Message, ex.StackTrace));
                    }
                }
                Write(level, msg.ToString());
            }
            catch (Exception) { }
        }


        /// <summary>
        /// 处理写日志代理
        /// </summary>
        /// <param name="func">处理方法</param>
        void DoWrite(Action func)
        {
            lock (lockObject)
            {
                this.check();
                func();
            }
        }
        FileStream fs = null;
        StreamWriter sw = null;
        private void check()
        {
            //App/Year/Month/Day.log
            DateTime dt = DateTime.Now;
            string folder = Path.Combine(BaseDirectory, dt.Year.ToString(), dt.Month.ToString());
            if (!Directory.Exists(folder))
            {
                this.closeFileStream();
                //目录不存在  建立目录
                Directory.CreateDirectory(folder);
            }
            //day
            string path = Path.Combine(folder, string.Format("{0}.{1}", dt.Day, ExtendName));
            bool rebuilder = false;
            if (fs == null || sw == null)
            {
                rebuilder = true;
            }
            else if (false == path.Equals(fs.Name))
            {
                sw.DoDispose();
                fs.DoDispose();
                rebuilder = true;
            }
            if (rebuilder)
            {
                fs = new FileStream(path, FileMode.Append, FileAccess.Write);
                //或者用streamWriter指定
                sw = new StreamWriter(fs, this.Encoding);
            }
        }

        /// <summary>
        /// 关闭文件流
        /// </summary>
        private void closeFileStream()
        {
            if (sw != null)
            {
                try
                {
                    sw.Flush();
                    sw.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("关闭日志:{0}", ex.Message);
                }
                sw.DoDispose();
                sw = null;
            }
            if (fs != null)
            {
                fs.DoDispose();
                fs = null;
            }
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            closeFileStream();

        }
    }
    /// <summary>
    /// 日志级别
    /// </summary>
    public enum LogLevels
    {
        /// <summary>
        /// 无任何提示
        /// </summary>
        None = 1,
        /// <summary>
        /// 无任何提示
        /// </summary>
        Console = 2,
        /// <summary>
        /// 常规提示
        /// </summary>
        Info = 4,
        /// <summary>
        /// 警告
        /// </summary>
        Warn = 8,
        /// <summary>
        /// Trace
        /// </summary>
        Trace = 16,
        /// <summary>
        /// Debug
        /// </summary>
        Debug = 32,
        /// <summary>
        /// 全部显示
        /// </summary>
        ALL = 64,
    }
}
