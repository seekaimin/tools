using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Util.Common.ExtensionHelper;

namespace Util.Common
{
    /// <summary>
    /// 自定义异常
    /// </summary>
    public class AMException : Exception
    {
        private Int32 _ErrorCode = 0;
        /// <summary>
        /// 错误编号
        /// </summary>
        public Int32 ErrorCode
        {
            get { return _ErrorCode; }
            protected set { _ErrorCode = value; }
        }
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="message">消息信息</param>
        public AMException(string message) : base(message) { }
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="message">消息信息</param>
        /// <param name="errorCode">错误代码</param>
        public AMException(string message, Int32 errorCode)
            : base(message)
        {
            ErrorCode = errorCode;
        }
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="formatMessage">带格式化的信息</param>
        /// <param name="args">参数</param>
        public AMException(string formatMessage, params object[] args)
            : base(formatMessage.Fmt(args)) { }
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="formatMessage">带格式化的信息</param>
        /// <param name="args">参数</param>
        /// <param name="errorCode">错误代码</param>
        public AMException(string formatMessage, int errorCode, object[] args)
            : base(formatMessage.Fmt(args))
        {
            ErrorCode = errorCode;
        }
        /// <summary>
        /// /// 构造
        /// </summary>
        /// <param name="ex">异常</param>
        public AMException(Exception ex)
            : base(ex.Message, ex)
        {

        }
        /// <summary>
        ///  构造
        /// </summary>
        /// <param name="ex">异常</param>
        /// <param name="errorCode">错误代码</param>
        public AMException(Exception ex, Int32 errorCode)
            : base(ex.Message, ex)
        {
            ErrorCode = errorCode;
        }
        /// <summary>
        /// /// 构造
        /// </summary>
        /// <param name="errorCode">错误代码</param>
        public AMException(ErrorCode errorCode)
            : base(errorCode.ToString())
        {
            ErrorCode = (Int32)errorCode;
        }
    }
    /// <summary>
    /// 是有定义
    /// ErrorCode 0-0xFFFF
    /// </summary>
    public enum ErrorCode
    {
        /// <summary>
        /// buff错误
        /// </summary>
        BuffError = 0x1,
        /// <summary>
        /// 标题错误
        /// </summary>
        TittleError = 0x2,
        /// <summary>
        /// 版本号错误
        /// </summary>
        VersionError = 0x3,
        /// <summary>
        /// 消息来源错误
        /// </summary>
        MessageFormType = 0x4,
        /// <summary>
        /// 数据错误
        /// </summary>
        DataError = 0x5,
    }
}
