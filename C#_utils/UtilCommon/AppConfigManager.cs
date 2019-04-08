using System;
using System.Configuration;
using System.Linq;

namespace Util.Common
{
    /// <summary>
    /// 配置文件操作对象
    /// </summary>
    public class AppConfigManager
    {
        private Configuration _conManger;
        private AppSettingsSection _appManger;
        private ConnectionStringsSection _csManger;
        /// <summary>
        /// Configuration
        /// </summary>
        protected Configuration conManger
        {
            get { return _conManger; }
            set { _conManger = value; }
        }
        /// <summary>
        /// AppSettingsSection
        /// </summary>
        protected AppSettingsSection appManger
        {
            get { return _appManger; }
        }
        /// <summary>
        /// ConnectionStringsSection
        /// </summary>
        protected ConnectionStringsSection csManger
        {
            get { return _csManger; }
        }

        /// <summary>
        /// ConfigManager
        /// </summary>
        public AppConfigManager()
        {
            Refresh();
        }

        /// <summary>
        /// 保存数据库连接字符串
        /// 需调用Save方法才能保存到配置文件
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <param name="isThrow">当Key不存在时是否抛出异常</param>
        public void SetConnectionString(string key, string value, bool isThrow = false)
        {
            if (csManger.ConnectionStrings[key] == null)
            {
                if (isThrow)
                    throw new ArgumentNullException("Key");
                csManger.ConnectionStrings.Add(new ConnectionStringSettings(key, value));
            }
            else
            {
                csManger.ConnectionStrings[key].ConnectionString = value;
            }
        }
        /// <summary>
        /// 读取数据库连接字符串
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="isThrow">当Key不存在时是否抛出异常</param>
        /// <returns>数据库连接字符串</returns>
        public string GetConnectionString(string key, bool isThrow = false)
        {
            if (csManger.ConnectionStrings[key] == null)
            {
                if (isThrow)
                    throw new ArgumentNullException("Key");
                return string.Empty;
            }
            else
            {
                return csManger.ConnectionStrings[key].ConnectionString;
            }
        }
        /// <summary>
        /// 读取AppSetting[Key]值
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="isThrow">当Key不存在时是否抛出异常</param>
        /// <returns>字符串</returns>
        public string GetSetting(string key, bool isThrow = false)
        {
            if (appManger.Settings.AllKeys.Contains(key))
            {
                return appManger.Settings[key].Value;
            }
            else
            {
                if (isThrow)
                    throw new ArgumentNullException("Key");
                return string.Empty;
            }
        }
        /// <summary>
        /// 保存值
        /// 需调用Save方法才能保存到配置文件
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">value</param>
        /// <param name="isThrow">当Key不存在时是否抛出异常</param>
        /// <returns>字符串</returns>
        public void SetSetting(string key, string value, bool isThrow = false)
        {
            if (appManger.Settings.AllKeys.Contains(key))
            {
                appManger.Settings[key].Value = value;
            }
            else
            {
                if (isThrow)
                    throw new ArgumentNullException("Key");
                appManger.Settings.Add(new KeyValueConfigurationElement(key, value));
            }
        }
        /// <summary>
        /// 保存数据到配置文件
        /// </summary>
        public void Save()
        {
            conManger.Save();
            Refresh();
            //刷新节点，否则不能即时获得更新的数据
            //ConfigurationManager.RefreshSection("appSettings");
            //ConfigurationManager.RefreshSection("connectionStrings");
        }
        /// <summary>
        /// 刷新所有节点
        /// </summary>
        public void Refresh()
        {
            try
            {
                _conManger = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                _appManger = conManger.AppSettings;
                _csManger = conManger.ConnectionStrings;
            }
            catch
            {
            }
        }
    }
}
