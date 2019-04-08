using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Util.Common
{
    /// <summary>
    /// 定义后进先出队列
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    public class StackPool<T> : IDisposable
    {
        /// <summary>
        /// 资源释放标识
        /// </summary>
        public bool IsDisposed { get; protected set; }
        private Stack<T> pool = new Stack<T>();
        /// <summary>
        /// 构造
        /// </summary>
        public StackPool()
        {
            Reset();
        }
        /// <summary>
        /// 添加一个元素到队列
        /// </summary>
        /// <param name="item">添加对象</param>
        public void Set(T item)
        {
            if (this.IsDisposed)
            {
                return;
            }
            Monitor.Enter(pool);
            pool.Push(item);
            Monitor.PulseAll(pool);
            Monitor.Exit(pool);
        }
        /// <summary>
        /// 获取队列元素并将其移除
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
                t = pool.Pop();
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
            Monitor.PulseAll(pool);
            Monitor.Exit(pool);
        }
    }
}
