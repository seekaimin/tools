using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace UtilToolTipHelper
{
    /// <summary>
    /// 自定义服务基类
    /// </summary>
    public abstract class MyServiceBase
    {
        //定义属性
        string _ToolTipTittle = "Service";
        /// <summary>
        /// 提示标题
        /// </summary>
        public string ToolTipTitle
        {
            get { return _ToolTipTittle; }
            set { _ToolTipTittle = value; }
        }
        ServiceRuningStates _RuningState = ServiceRuningStates.UnStarted;
        /// <summary>
        /// 服务运行状态
        /// </summary>
        public ServiceRuningStates RuningState
        {
            get { return _RuningState; }
            protected set { _RuningState = value; }
        }
        /// <summary>
        /// 退出服务事件
        /// </summary>
        public event ExitServiceHandeller ExitService;

        /// <summary>
        /// 线程池
        /// </summary>
        private List<Thread> ThreadPool = new List<Thread>();
        /// <summary>
        /// 事件池
        /// </summary>
        private List<Delegate> EventPool = new List<Delegate>();

        //方法
        /// <summary>
        /// 服务开启和停止锁定对象
        /// </summary>
        static object servicelockbject = new object();
        /// <summary>
        /// 启动服务
        /// </summary>
        public void Start()
        {
            lock (servicelockbject)
            {
                if (RuningState != ServiceRuningStates.UnStarted) return;

                try
                {
                    if (!Validate())
                    {
                        ExitService();
                    }
                    else
                    {
                        this.RuningState = ServiceRuningStates.Starting;
                        //清空所有线程 事件
                        ThreadPool.Clear();
                        EventPool.Clear();
                        if (ExitService != null)
                        {
                            AddEventToPool(ExitService);
                        }
                        Begin();
                    }
                }
                catch (Exception ex)
                {
                    DoException(ex);
                }
            }
        }
        /// <summary>
        /// 停止服务
        /// </summary>
        public void Stop()
        {
            lock (servicelockbject)
            {
                if (RuningState == ServiceRuningStates.Stoping || RuningState == ServiceRuningStates.Stoped) { return; }
                RuningState = ServiceRuningStates.Stoping;
                try { End(); }
                catch (Exception) { }
                try { Dispose(); }
                catch (Exception) { }
            }
        }

        /// <summary>
        /// 启动检核
        /// </summary>
        /// <returns></returns>
        protected virtual bool Validate()
        {
            return true;
        }
        /// <summary>
        /// 释放服务
        /// </summary>
        public void Dispose()
        {
            #region 清空线程池
            lock (ThreadPool)
            {
                ThreadPool.ForEach(delegate(Thread thread)
                {
                    try
                    {
                        thread.Abort();
                    }
                    catch (Exception) { }
                });
                ThreadPool.Clear();
            }
            #endregion
            #region 清空事件池
            lock (EventPool)
            {
                EventPool.ForEach(delegate(Delegate del)
                {
                    try
                    {
                        del = null;
                    }
                    catch (Exception) { }
                });
                EventPool.Clear();
            }
            #endregion
            //提示消息 服务已经停止
            RuningState = ServiceRuningStates.Stoped;
        }



        /// <summary>
        /// 服务内部处理异常
        /// </summary>
        /// <param name="ex">异常信息</param>
        protected virtual void DoException(Exception ex)
        {
            if (RuningState == ServiceRuningStates.Stoped || RuningState == ServiceRuningStates.UnStarted) return;
            if (ex is SettingException)
            {
                SettingException se = ex as SettingException;
                if (string.IsNullOrEmpty(se.Message))
                    ServiceHelpers.ShowMessage(ToolTipTitle, se.Message);
                else
                    ServiceHelpers.ShowMessage(ToolTipTitle, "Setting Error");
                DoExitService();
            }
            else if (ex is ServiceException)
            {
                ServiceException se = ex as ServiceException;
                if (string.IsNullOrEmpty(se.Message) == false)
                    ServiceHelpers.ShowMessage(ToolTipTitle, se.Message);
                if (se.IsExitService) DoExitService();
            }
            else
            {
                ServiceHelpers.ShowMessage(ToolTipTitle, "UnKnow Error");
                DoExitService();
            }
        }
        /// <summary>
        /// 服务内部处理异常
        /// </summary>
        /// <param name="message">异常信息</param>
        /// <param name="isExitService">是否退出服务</param>
        protected virtual void DoException(string message, bool isExitService = false)
        {
            DoException(new ServiceException(message, isExitService));
        }
        /// <summary>
        /// 退出服务
        /// </summary>
        protected void DoExitService()
        {
            if (ExitService != null)
            {
                ExitService();
            }
        }
        /// <summary>
        /// 添加线程到线程池
        /// </summary>
        /// <param name="thread">线程</param>
        protected void AddThreadToPool(Thread thread)
        {
            lock (ThreadPool)
            {
                ThreadPool.Add(thread);
            }
        }
        /// <summary>
        /// 添加事件到事件池
        /// </summary>
        /// <param name="del">事件</param>
        protected void AddEventToPool(Delegate del)
        {
            lock (EventPool)
            {
                EventPool.Add(del);
            }
        }

        //需要实现 的方法
        /// <summary>
        /// 自定义服务开启项目
        /// </summary>
        protected abstract void Begin();
        /// <summary>
        /// 自定义服务停止项目
        /// </summary>
        protected abstract void End();

    }
    /// <summary>
    /// 退出服务委托
    /// </summary>
    public delegate void ExitServiceHandeller();
    /// <summary>
    /// 自定义服务短异常信息
    /// </summary>
    public class ServiceException : Exception
    {
        private bool _IsExitService = false;
        /// <summary>
        /// 是否退出服务
        /// </summary>
        public virtual bool IsExitService
        {
            get { return _IsExitService; }
            set { _IsExitService = value; }
        }
        /// <summary>
        /// 服务端自定义异常
        /// </summary>
        /// <param name="message">提示消息</param>
        /// <param name="isExitService">是否退出服务</param>
        public ServiceException(string message, bool isExitService = false)
            : base(message)
        {
            this.IsExitService = isExitService;
        }
        /// <summary>
        /// 服务端自定义异常
        /// </summary>
        /// <param name="message">提示消息</param>
        /// <param name="ex">内部异常</param>
        /// <param name="isExitService">是否退出服务</param>
        public ServiceException(string message, Exception ex, bool isExitService = false)
            : base(message, ex)
        {
            this.IsExitService = isExitService;
        }
    }
    /// <summary>
    /// 自定义服务端配置异常
    /// 会将服务关闭
    /// </summary>
    public class SettingException : ServiceException
    {
        /// <summary>
        /// 服务端自定义异常
        /// 配置错误异常
        /// </summary>
        /// <param name="message">提示消息</param>
        public SettingException(string message)
            : base(message, true)
        {

        }
    }
    /// <summary>
    /// 服务运行状态
    /// </summary>
    public enum ServiceRuningStates
    {
        /// <summary>
        /// 未启动
        /// </summary>
        UnStarted,
        /// <summary>
        /// 正在启动
        /// </summary>
        Starting,
        /// <summary>
        /// 已启动
        /// </summary>
        Started,
        /// <summary>
        /// 挂起
        /// </summary>
        Suspend,
        /// <summary>
        /// 正在停止
        /// </summary>
        Stoping,
        /// <summary>
        /// 已经停止
        /// </summary>
        Stoped,
    }
}
