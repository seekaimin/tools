using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Util.ServiceEngine.Core;
using System.Windows.Forms;
using System.Diagnostics;
using Util.ServiceEngine.ServiceForms;
using System.IO;

namespace Util.ServiceEngine
{
    /// <summary>
    /// 服务帮助类
    /// </summary>
    public static class ServiceHelper
    {
        private static Object lockObject = new object();
        /// <summary>
        /// 提示消息
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="message">消息</param>
        public static void ShowMessage(string title, string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }
            lock (lockObject)
            {
                ToolTopMessage ttm = new ToolTopMessage();
                ttm.Title = title;
                ttm.Content = message;
                StartShowMessageUI(ttm);
            }
        }

        /// <summary>
        /// 提示消息
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="message">消息</param>
        /// <param name="tootTipIcon">图标</param>
        public static void ShowMessage(string title, string message, IconType tootTipIcon)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }
            lock (lockObject)
            {
                ToolTopMessage ttm = new ToolTopMessage();
                ttm.Title = title;
                ttm.Content = message;
                ttm.BalloonType = (ToolTipIcon)((int)tootTipIcon);
                StartShowMessageUI(ttm);
            }
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
            if (string.IsNullOrEmpty(message))
            {
                return;
            }
            lock (lockObject)
            {
                ToolTopMessage ttm = new ToolTopMessage();
                ttm.Title = title;
                ttm.Content = message;
                ttm.Timeout = timeOut;
                ttm.BalloonType = (ToolTipIcon)((int)tootTipIcon);
                StartShowMessageUI(ttm);
            }
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
                    foreach (Process p in Process.GetProcessesByName("UtilServiceEngine"))
                    {
                        p.WaitForExit();
                    }

                    if (!Desktop.CreateProcess(Application.StartupPath + @"/UtilServiceEngine.exe",
                         Application.StartupPath,
                         String.Format("{0} {1} {2} {3}",
                         message.BalloonType.ToString(),
                         message.Title.Replace(' ', '^'),
                         message.Content.Replace(' ', '^'),
                         message.Timeout)
                         ))
                    {
                        Process.Start(Application.StartupPath + @"/UtilServiceEngine.exe", String.Format("{0} {1} {2} {3}",
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

        /// <summary>
        /// 通过进程名称杀掉进程
        /// </summary>
        /// <param name="progressName">进程名称</param>
        public static void KillProgress(string progressName)
        {
            lock (lockObject)
            {
                foreach (Process p in Process.GetProcessesByName(progressName))
                {
                    try
                    {
                        p.Kill();
                    }
                    catch { }
                }
            }
        }
    }
}
