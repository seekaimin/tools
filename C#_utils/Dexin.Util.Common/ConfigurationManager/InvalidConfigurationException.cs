using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dexin.Util.Common.ConfigurationManager
{
    /// <summary>
    /// 配置信息异常类
    /// </summary>
    public class InvalidConfigurationException : Exception
    {
        public InvalidConfigurationException() { }

        public InvalidConfigurationException(string message) : base(message) { }

        public InvalidConfigurationException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
