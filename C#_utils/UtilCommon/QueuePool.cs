using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Util.Common
{
    /// <summary>
    /// 定义先进先出队列
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    public class QueuePool<T> : IDisposable
    {
        /// <summary>
        /// 释放标识
        /// </summary>
        public bool IsDisposed { get; protected set; }
        private Queue<T> pool = new Queue<T>();
        /// <summary>
        /// 构造
        /// </summary>
        public QueuePool()
        {
            Reset();
        }
        /// <summary>
        /// 添加一个元素到队列
        /// </summary>
        /// <param name="item">添加的元素</param>
        public void Set(T item)
        {
            if (this.IsDisposed)
            {
                return;
            }
            Monitor.Enter(pool);
            pool.Enqueue(item);
            Monitor.Pulse(pool);
            Monitor.Exit(pool);
        }
        /// <summary>
        /// 获取一个队列元素并将其移除
        /// </summary>
        /// <returns></returns>
        public T Get()
        {
            T t = default(T);
            try
            {
                Monitor.Enter(pool);
                while (this.IsDisposed == false)
                {
                    if (this.pool.Count == 0)
                    {
                        Monitor.Wait(pool);
                    }
                    else
                    {
                        break;
                    }
                }
                t = pool.Dequeue();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                Monitor.Exit(pool);
            }
            return t;
        }

        /// <summary>
        /// 重置释放标识
        /// </summary>
        public void Reset()
        {
            this.IsDisposed = false;
        }
        /// <summary>
        /// 资源释放
        /// </summary>
        public void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }
            IsDisposed = true;
            Monitor.Enter(pool);
            Monitor.PulseAll(pool);
            if (pool.Count > 0)
            {
                pool.Clear();
            }
            Monitor.Exit(pool);
        }
        /// <summary>
        /// 数量
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return pool.Count();
        }
    }
}
