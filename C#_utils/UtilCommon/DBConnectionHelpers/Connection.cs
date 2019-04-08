using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;

namespace Util.Common.DBConnectionHelpers
{
    /// <summary>
    /// 数据库连接对象
    /// </summary>
    public class Connection : IDisposable
    {
        private Int32 _CurrentThreadID = 0;
        private bool _IsDisposed = false;
        DbConnection _connection;
        /// <summary>
        /// 构造 创建连接
        /// </summary>
        /// <param name="currentThreadID">线程ID</param>
        /// <param name="connection">连接字符串</param>
        internal Connection(int currentThreadID,DbConnection connection)
        {
            this.connection = connection;
            CurrentThreadID = 0;
            IsDisposed = false;
        }
        /// <summary>
        /// 连接
        /// </summary>
        internal DbConnection connection
        {
            get { return _connection; }
            private set { _connection = value; }
        }
        /// <summary>
        /// 是否被释放
        /// </summary>
        internal bool IsDisposed
        {
            get { return _IsDisposed; }
            set { _IsDisposed = value; }
        }
        /// <summary>
        /// 线程ID
        /// </summary>
        internal Int32 CurrentThreadID
        {
            get { return _CurrentThreadID; }
            set { _CurrentThreadID = value; }
        }
        /// <summary>
        /// 打开连接
        /// </summary>
        public void Open()
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
        }
        /// <summary>
        /// 关闭连接
        /// </summary>
        public void Close()
        {
            try
            {
                if (connection.State != ConnectionState.Closed)
                    connection.Close();
            }
            catch
            {
                this.Dispose();
            }
            finally
            {
                CurrentThreadID = 0;
            }
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (!IsDisposed)
            {
                IsDisposed = true;
                if (connection != null)
                {
                    try
                    {
                        this.connection.Dispose();
                    }
                    catch { }
                }
            }
        }
    }
}
