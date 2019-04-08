using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Globalization;
using Util.Common.ExtensionHelper;

namespace Util.Common
{
    /// <summary>
    /// 线程壳 不可重复使用
    /// </summary>
    public sealed class ThreadShell : IDisposable
    {
        /// <summary>
        /// ID = 线程ID
        /// </summary>
        public int ID { get; private set; }
        /// <summary>
        /// 线程是否正常运行
        /// </summary>
        public bool Running { get; private set; }
        private bool _Sleeped = false;
        private Thread _Thread;
        private string _Name { get; set; }
        /// <summary>
        /// 构造
        /// </summary>
        private ThreadShell() { }
        /// <summary>
        /// 线程阻塞
        /// </summary>
        /// <param name="time">阻止时长  毫秒</param>
        /// <param name="unitTime">最小阻塞单元  毫秒   默认 1000毫秒</param>
        private void _Wait(int time, int unitTime = 1000)
        {
            if (_Thread == null || false == _Thread.IsAlive)
            {
                throw new NullReferenceException("{0} is stoped!".Fmt(this._Name));
            }
            if (false == this.Running)
            {
                return;
            }
            this._Sleeped = true;
            DateTime start = DateTime.Now;
            DateTime end;
            int calc = 0;
            while (_Thread != null && _Thread.IsAlive)
            {
                if (false == this.Running)
                {
                    break;
                }
                if (false == this._Sleeped)
                {
                    break;
                }
                end = DateTime.Now;
                if (time <= 0)
                {
                    Thread.Sleep(unitTime);
                }
                else if (time <= unitTime)
                {
                    Thread.Sleep(time);
                    break;
                }
                else
                {
                    calc = (int)((end - start).TotalMilliseconds);
                    if (calc >= time)
                    {
                        break;
                    }
                    Thread.Sleep(unitTime);
                }
            }
            this._Sleeped = false;
        }
        /// <summary>
        /// 线程唤醒
        /// </summary>
        public void WakeUp()
        {
            if (!this.Running)
            {
                throw new Exception("{0} is stoped!".Fmt(this._Name));
            }
            this._Sleeped = false;
        }
        /// <summary>
        /// 线程释放(非暴力释放)
        /// </summary>
        public void Dispose()
        {
            if (false == this.Running)
            {
                return;
            }
            this._Sleeped = false;
            this.Running = false;
            lock (pool)
            {
                if (pool.ContainsKey(this.ID))
                {
                    pool.Remove(this.ID);
                }
            }
        }
        private static Dictionary<int, ThreadShell> pool = new Dictionary<int, ThreadShell>();
        /// <summary>
        /// 启动
        /// </summary>
        /// <param name="start">启动方法</param>
        /// <param name="name">线程壳名称</param>
        /// <param name="exhandle">线程启动,异常处理句柄</param>
        public static ThreadShell Run(ThreadStart start, String name = null, Action<Exception> exhandle = null)
        {
            ThreadShell shell = new ThreadShell() { _Name = name };
            shell._Thread = new Thread(() =>
            {
                try
                {
                    start();
                }
                catch (Exception ex)
                {
                    if (exhandle != null) { exhandle(ex); }
                }
                finally
                {
                    //资源释放
                    shell.DoDispose();
                }
            });
            shell.ID = shell._Thread.ManagedThreadId;
            if (shell._Name.IsNullOrEmpty())
            {
                shell._Name = "ThreadShell-{0}".Fmt(shell.ID);
            }
            shell._Thread.Name = shell._Name;
            lock (pool)
            {
                pool[shell.ID] = shell;
                shell.Running = true;
            }
            shell._Thread.Start();
            return shell;
        }
        /// <summary>
        /// 获取当前线程壳   可能为空
        /// </summary>
        public static string Name
        {
            get
            {
                ThreadShell shell = ThreadShell.Current;
                return shell == null ? Thread.CurrentThread.Name : shell._Name;
            }
        }
        /// <summary>
        /// 获取当前线程壳   可能为空
        /// </summary>
        public static ThreadShell Current
        {
            get
            {
                lock (pool)
                {
                    int id = Thread.CurrentThread.ManagedThreadId;
                    return pool.ContainsKey(id) ? pool[id] : null;
                }
            }
        }
        /// <summary>
        /// 线程阻塞
        /// </summary>
        /// <param name="time">阻止时长  毫秒</param>
        /// <param name="unitTime">最小阻塞单元  毫秒   默认 1000毫秒</param>
        public static void Wait(int time, int unitTime = 1000)
        {
            ThreadShell shell = ThreadShell.Current;
            if (shell == null)
            {
                Thread.Sleep(time);
            }
            else
            {
                shell._Wait(time, unitTime);
            }
        }

        /// <summary>
        /// 释放所有资源
        /// </summary>
        public static void DisposeAll()
        {
            lock (pool)
            {
                while (pool.Count > 0)
                {
                    var item = pool.FirstOrDefault();
                    if (item.Value.Running)
                    {
                        item.Value.DoDispose();
                    }
                    else
                    {
                        pool.Remove(item.Key);
                    }
                }
            }
        }

    }









    /// <summary>
    /// 线程扩展类
    /// </summary>
    public static class ThreadHelpers
    {
        /// <summary>
        /// 创建一个线程
        /// </summary>
        /// <param name="threadName">线程名称</param>
        /// <param name="action">执行的方法</param>
        /// <param name="currentUICulture">UI区域属性</param>
        /// <param name="currentCulture">线程区域属性</param>
        /// <returns></returns>
        public static Thread CreateThread(ThreadStart action, string threadName, CultureInfo currentUICulture = null, CultureInfo currentCulture = null)
        {
            Thread thread = new Thread(action);
            thread.CurrentUICulture = currentUICulture == null ? Thread.CurrentThread.CurrentUICulture : currentUICulture;
            thread.CurrentCulture = currentCulture == null ? Thread.CurrentThread.CurrentCulture : currentCulture;
            if (!string.IsNullOrEmpty(threadName))
            {
                thread.Name = threadName;
            }
            return thread;
        }
        /// <summary>
        /// 创建一个线程 带参数
        /// </summary>
        /// <param name="threadName">线程名称</param>
        /// <param name="action">执行的方法</param>
        /// <param name="currentUICulture">UI区域属性</param>
        /// <param name="currentCulture">线程区域属性</param>
        /// <returns></returns>
        public static Thread CreateThread(ParameterizedThreadStart action, string threadName, CultureInfo currentUICulture = null, CultureInfo currentCulture = null)
        {
            Thread thread = new Thread(action);
            thread.CurrentUICulture = currentUICulture == null ? Thread.CurrentThread.CurrentUICulture : currentUICulture;
            thread.CurrentCulture = currentCulture == null ? Thread.CurrentThread.CurrentCulture : currentCulture;
            if (!string.IsNullOrEmpty(threadName))
            {
                thread.Name = threadName;
            }
            return thread;
        }
        /// <summary>
        /// 终止线程
        /// </summary>
        /// <param name="thread">需要终止的线程</param>
        /// <param name="action">发生异常 处理句柄</param>
        /// <returns></returns>
        public static void DoAbort(this Thread thread, ActionWithParametersHandler<Exception> action)
        {
            try
            {
                thread.Abort();
            }
            catch (Exception ex)
            {
                if (action != null)
                {
                    action(ex);
                }
            }
        }
        /// <summary>
        /// 终止线程
        /// </summary>
        /// <param name="thread">需要终止的线程</param>
        /// <returns></returns>
        public static void DoAbort(this Thread thread)
        {
            thread.DoAbort(null);
        }
    }
}
