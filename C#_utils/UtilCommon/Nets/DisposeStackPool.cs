using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Util.Common.Nets
{
    /// <summary>
    /// 定义后进先出队列池
    /// </summary>
    /// <typeparam name="T">可释放的对象</typeparam>
    public class DisposeStackPool<T> : IDisposable where T : IDisposable
    {
        /// <summary>
        /// 资源释放标识
        /// </summary>
        public bool IsDisposed { get; protected set; }
        private Stack<T> pool = new Stack<T>();
        /// <summary>
        /// 构造
        /// </summary>
        public DisposeStackPool()
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
                item.DoDispose();
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
            do
            {
                Monitor.PulseAll(pool);
                if (pool.Count == 0)
                {
                    break;
                }
                T t = pool.Pop();
                t.DoDispose();
                Monitor.PulseAll(pool);
            } while (true);
            Monitor.Exit(pool);
        }
    }
}
