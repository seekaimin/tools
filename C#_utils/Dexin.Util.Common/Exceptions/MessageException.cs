using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dexin.Util.Common.Exceptions
{
    /// <summary>
    /// 自定义消息类异常类
    /// </summary>
    public class MessageException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public MessageException() : base() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msgFormat"></param>
        /// <param name="args"></param>
        public MessageException(string msgFormat, params object[] args) : base(string.Format(msgFormat, args)) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="innerException"></param>
        public MessageException(Exception innerException) : base("", innerException) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="innerException"></param>
        /// <param name="msgFormat"></param>
        /// <param name="args"></param>
        public MessageException(Exception innerException, string msgFormat, params object[] args) : base(string.Format(msgFormat, args), innerException) { }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MessageException<T> : MessageException where T : struct
    {
        /// <summary>
        /// 
        /// </summary>
        public T ErrorCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        public MessageException(T code)
            : base()
        {
            ErrorCode = code;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="msgFormat"></param>
        /// <param name="args"></param>
        public MessageException(T code, string msgFormat, params object[] args)
            : base(msgFormat, args)
        {
            ErrorCode = code;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="innerException"></param>
        /// <param name="msgFormat"></param>
        /// <param name="args"></param>
        public MessageException(T code, Exception innerException, string msgFormat, params object[] args)
            : base(innerException, msgFormat, args)
        {
            ErrorCode = code;
        }
    }
}
