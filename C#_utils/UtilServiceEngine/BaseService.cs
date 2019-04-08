using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;

namespace Util.ServiceEngine
{
    /// <summary>
    /// 服务基类
    /// </summary>
    public abstract class BaseService : IDisposable
    {
        #region 属性
        /// <summary>
        /// 服务锁定对象
        /// 当开启服务和停止服务时使用
        /// </summary>
        public static object servicelockbject = new object();
        private string _ServiceTitle = "Service";
        /// <summary>
        /// 服务标题
        /// </summary>
        public string ServiceTitle
        {
            get { return _ServiceTitle; }
            set { _ServiceTitle = value; }
        }
        private ServiceStates _ServiceState = ServiceStates.UnStarted;
        /// <summary>
        /// 线程池
        /// </summary>
        private List<Thread> ThreadPool = new List<Thread>();
        /// <summary>
        /// 事件池
        /// </summary>
        private List<Delegate> EventPool = new List<Delegate>();
        /// <summary>
        /// 服务运行状态
        /// </summary>
        public ServiceStates ServiceState
        {
            get { return _ServiceState; }
            protected set { _ServiceState = value; }
        }
        #endregion
        #region Event
        /// <summary>
        /// 退出服务
        /// </summary>
        public event Action ExitServiceHandle;

        #endregion

        /// <summary>
        /// 是否通过检核
        /// </summary>
        bool IsValidated
        {
            get
            {
                try
                {
                    return Validate();
                }
                catch (SettingException settingex)
                {
                    DoException(settingex);
                    return false;
                }
                catch (ServiceException serviceex)
                {
                    DoException(serviceex);
                    if (!serviceex.IsExitService)
                    {
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    DoException(ex);
                }
                return false;
            }
        }
        /// <summary>
        /// 启动服务
        /// </summary>
        public void Start()
        {
            lock (servicelockbject)
            {
                //判断服务是否运行中
                if (ServiceState == ServiceStates.UnStarted || ServiceState == ServiceStates.Stoped)
                {
                    ServiceState = ServiceStates.Starting;
                    if (IsValidated)
                    {
                        //清空所有线程 事件
                        ThreadPool.Clear();
                        EventPool.Clear();
                        if (ExitServiceHandle != null)
                        {
                            AddEvents(ExitServiceHandle);
                        }
                        Begin();
                        if (ServiceState == ServiceStates.Started)
                        {
                            ServiceHelper.ShowMessage(ServiceTitle, "service is stared");
                        }
                    }
                    else
                    {
                        DoExitService();
                    }
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
                if (ServiceState == ServiceStates.UnStarted || ServiceState == ServiceStates.Stoping || ServiceState == ServiceStates.Stoped) { return; }
                ServiceState = ServiceStates.Stoping;
                new Thread(() =>
                {
                    try { End(); }
                    catch (Exception) { }
                    try { Dispose(); }
                    catch (Exception) { }
                    ServiceHelper.ShowMessage(ServiceTitle, "service is stoped!");
                }).Start();
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
        /// 处理退出服务
        /// </summary>
        protected void DoExitService()
        {
            if (ExitServiceHandle != null)
            {
                ExitServiceHandle();
            }
            else
            {
                this.Stop();
            }
        }

        /// <summary>
        /// 处理异常
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <param name="action">未知异常处理</param>
        protected virtual void DoException(Exception ex, Action<Exception> action = null)
        {
            if (ex is SettingException)
            {
                DoException((SettingException)ex);
            }
            else if (ex is ServiceException)
            {
                DoException((ServiceException)ex);
            }
            else
            {
                if (action != null)
                {
                    action(ex);
                }
                else
                {
                    throw ex;
                }
            }
        }
        /// <summary>
        /// 处理服务端异常
        /// </summary>
        /// <param name="ex">服务端异常</param>
        void DoException(ServiceException ex)
        {
            if (ex != null)
            {
                ServiceHelper.ShowMessage(ServiceTitle, ex.Message);
                if (ex.IsExitService)
                {
                    DoExitService();
                }
            }
        }
        /// <summary>
        /// 处理配置异常
        /// </summary>
        /// <param name="ex">配置异常</param>
        void DoException(SettingException ex)
        {
            if (ex != null)
            {
                ServiceHelper.ShowMessage(ServiceTitle, ex.Message);
                DoExitService();
            }
        }
        /// <summary>
        /// 释放服务
        /// </summary>
        public void Dispose()
        {
            try
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
            }
            catch (Exception) { }
            //提示消息 服务已经停止
            ServiceState = ServiceStates.Stoped;
        }
        /// <summary>
        /// 添加服务端线程
        /// </summary>
        /// <param name="thread">服务端线程</param>
        protected void AddThreads(Thread thread)
        {
            lock (ThreadPool)
            {
                ThreadPool.Add(thread);
            }
        }
        /// <summary>
        /// 添加服务端事件
        /// </summary>
        /// <param name="del">服务端事件</param>
        protected void AddEvents(Delegate del)
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
}
