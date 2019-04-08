using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Util.ServiceEngine
{
    /// <summary>
    /// 服务端异常信息
    /// </summary>
    public class ServiceException : Exception
    {
        private int _ErrorCode = -1;
        /// <summary>
        /// 错误代码
        /// </summary>
        public int ErrorCode
        {
            get { return _ErrorCode; }
            set { _ErrorCode = value; }
        }
        private bool _IsExitService = false;
        /// <summary>
        /// 是否退出服务
        /// </summary>
        public bool IsExitService
        {
            get { return _IsExitService; }
            set { _IsExitService = value; }
        }
        /// <summary>
        /// 服务端异常
        /// </summary>
        /// <param name="message">异常消息</param>
        /// <param name="isexitservice">是否退出</param>
        public ServiceException(string message, bool isexitservice = false)
            : base(message)
        {
            IsExitService = isexitservice;
        }
        /// <summary>
        /// 带内部异常的服务端异常
        /// </summary>
        /// <param name="message">提示消息</param>
        /// <param name="innerex">内部异常</param>
        /// <param name="isexitservice">是否退出</param>
        public ServiceException(string message, Exception innerex, bool isexitservice = false)
            : base(message, innerex)
        {
            IsExitService = isexitservice;
        }
    }
    /// <summary>
    /// 配置异常
    /// 会退出服务
    /// </summary>
    public class SettingException : ServiceException
    {
        /// <summary>
        /// 配置异常
        /// </summary>
        /// <param name="message">提示消息</param>
        public SettingException(string message) : base(message, true) { }
        /// <summary>
        /// 配置异常 带格式化
        /// </summary>
        /// <param name="formatmessage">需要格式化的消息</param>
        /// <param name="parameters">参数</param>
        public SettingException(string formatmessage, params object[] parameters) :
            base(string.Format(formatmessage, parameters), true)
        {

        }
    }
}
