using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Security.Principal;

namespace UtilToolTipHelper
{
    /// <summary>
    /// ToolTipHelper
    /// </summary>
    public class ServiceHelpers
    {
        private static Object lockObject = new object();
        /// <summary>
        /// 提示消息
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="message">消息</param>
        public static void ShowMessage(string title, string message)
        {
            ToolTopMessage ttm = new ToolTopMessage();
            ttm.Title = title;
            ttm.Content = message;
            StartShowMessageUI(ttm);
        }
        /// <summary>
        /// 提示消息
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="message">消息</param>
        /// <param name="timeOut">超时时间</param>
        /// <param name="tootTipIcon">图标</param>
        public static void ShowMessage(string title, string message, int timeOut, IconType tootTipIcon)
        {
            ToolTopMessage ttm = new ToolTopMessage();
            ttm.Title = title;
            ttm.Content = message;
            ttm.Timeout = timeOut;
            ttm.BalloonType = (ToolTipIcon)((int)tootTipIcon);
            StartShowMessageUI(ttm);
        }
        /// <summary>
        /// 显示提示消息到用户界面
        /// </summary>
        /// <param name="message">提示消息</param>
        /// <returns>用户窗体</returns>
        static void StartShowMessageUI(ToolTopMessage message)
        {
            lock (lockObject)
            {
                try
                {
                    foreach (Process p in Process.GetProcessesByName("UtilToolTipHelper"))
                    {
                        p.WaitForExit();
                    }

                    if (!Desktop.CreateProcess(Application.StartupPath + @"/UtilToolTipHelper.exe",
                         Application.StartupPath,
                         String.Format("{0} {1} {2} {3}",
                         message.BalloonType.ToString(),
                         message.Title.Replace(' ', '^'),
                         message.Content.Replace(' ', '^'),
                         message.Timeout)
                         ))
                    {
                        Process.Start(Application.StartupPath + @"/UtilToolTipHelper.exe", String.Format("{0} {1} {2} {3}",
                         message.BalloonType.ToString(),
                         message.Title.Replace(' ', '^'),
                         message.Content.Replace(' ', '^'),
                         message.Timeout));
                    }
                }
                catch { }
            }
        }
        /// <summary>
        /// 显示服务进度条
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        /// <param name="model">操作模式</param>
        /// <param name="args">参数</param>
        public static void ShowPogressBar(string serviceName, ServiceControlModel model, params string[] args)
        {
            FormProgressBar f = new FormProgressBar(serviceName, model, args);
            if (f.IsInstall)
                f.ShowDialog();
        }
    
    }
    /// <summary>
    /// 显示图标类型
    /// </summary>
    public enum IconType
    {
        /// <summary>
        /// 不是标准图标
        /// </summary>
        None = 0,
        /// <summary>
        /// 信息图标
        /// </summary>
        Info = 1,
        /// <summary>
        /// 警告图标
        /// </summary>
        Warning = 2,
        /// <summary>
        /// 错误图标
        /// </summary>
        Error = 3,
    }
    /// <summary>
    /// 服务控制模式
    /// </summary>
    public enum ServiceControlModel
    {
        /// <summary>
        /// 启动
        /// </summary>
        Start = 1,
        /// <summary>
        /// 停止
        /// </summary>
        Stop = 2,
        /// <summary>
        /// 重启
        /// </summary>
        ReStart = 3,
    }
}