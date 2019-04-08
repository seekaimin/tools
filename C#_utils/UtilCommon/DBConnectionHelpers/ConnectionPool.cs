using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Threading;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;

namespace Util.Common.DBConnectionHelpers
{
    /// <summary>
    /// 连接池父类
    /// </summary>
    public abstract class ConnectionPool<T>
        where T : DbConnection, new()
    {
        static string _ConnectionString = @"server=.;database=master;uid=sa;pwd=123;";
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public static string ConnectionString
        {
            get { return _ConnectionString; }
            set { _ConnectionString = value; }
        }
        /// <summary>
        /// 连接池
        /// </summary>
        static List<Connection> Pool;
        /// <summary>
        /// 事物池
        /// </summary>
        static Dictionary<int, DbTransaction> PoolTransactions = new Dictionary<int, DbTransaction>();
        static ConnectionPool()
        {
            Pool = new List<Connection>();
        }
        /// <summary>
        /// 当前连接对象
        /// </summary>
        public static Connection CurrentConnection
        {
            get
            {
                lock (Pool)
                {
                    int id = Thread.CurrentThread.ManagedThreadId;
                    Connection conn = Pool.FirstOrDefault(p => p.CurrentThreadID == id);
                    if (conn == null)
                    {
                        conn = Pool.FirstOrDefault(p => !p.IsDisposed && p.CurrentThreadID == 0);
                        if (conn == null)
                        {
                            conn = BuildConnection(id);
                            Pool.Add(conn);
                        }
                        conn.CurrentThreadID = id;
                    }
                    return conn;
                }
            }
        }
        /// <summary>
        /// 准备DbCommand
        /// </summary>
        /// <param name="cmd">DbCommand</param>
        /// <param name="cmdType">CommandType</param>
        /// <param name="cmdText">cmdText</param>
        /// <param name="cmdParms">DbParameter</param>
        public static void PrepareCommand(DbCommand cmd, CommandType cmdType, string cmdText, DbParameter[] cmdParms)
        {
            //为命令对象设置命令文本
            cmd.CommandText = cmdText;
            //判断是否存在事务操作
            int key = Thread.CurrentThread.ManagedThreadId;
            Connection conn = CurrentConnection;
            DbTransaction tran = CurrentTransaction;
            conn.Open();
            if (tran != null)
            {
                cmd.Transaction = tran;
            }
            cmd.Connection = conn.connection;
            //为命令对象设置命令类型
            cmd.CommandType = cmdType;
            //如果参数集不为空
            if (cmdParms != null)
            {
                //循环填充所有的命令参数至命令对象
                foreach (DbParameter parm in cmdParms)
                {
                    cmd.Parameters.Add(parm);
                }
            }
        }
        /// <summary>
        /// 开启事物
        /// </summary>
        public static void OpenTransaction()
        {
            lock (PoolTransactions)
            {
                int id = Thread.CurrentThread.ManagedThreadId;
                DbTransaction tran = CurrentTransaction;
                if (tran == null)
                {
                    Connection conn = CurrentConnection;
                    conn.Open();
                    PoolTransactions[id] = conn.connection.BeginTransaction();
                }
            }
        }
        /// <summary>
        /// 当前事物 可能为空
        /// </summary>
        public static DbTransaction CurrentTransaction
        {
            get
            {
                lock (PoolTransactions)
                {
                    int key = Thread.CurrentThread.ManagedThreadId;
                    return PoolTransactions.Keys.Contains(key) ? PoolTransactions[key] : null;
                }
            }
        }
        /// <summary>
        /// 释放连接池
        /// </summary>
        public static void DisposePool()
        {
            lock (PoolTransactions)
            {
                foreach (int key in PoolTransactions.Keys)
                {
                    try
                    {
                        PoolTransactions[key].Rollback();
                    }
                    catch { }
                }
                PoolTransactions.Clear();
            }
            lock (Pool)
            {
                foreach (Connection conn in Pool)
                {
                    try
                    {
                        conn.Dispose();
                    }
                    catch { }
                }
                Pool.Clear();
            }
        }
        /// <summary>
        /// 完成提交
        /// </summary>
        /// <param name="flag">提交、回滚事物</param>
        public static void Completed(bool flag)
        {
            Connection conn = CurrentConnection;
            lock (PoolTransactions)
            {
                DbTransaction tran = CurrentTransaction;
                if (tran != null)
                {
                    if (flag)
                    {
                        tran.Commit();
                    }
                    else
                    {
                        tran.Rollback();
                    }
                    int id = Thread.CurrentThread.ManagedThreadId;
                    if (PoolTransactions.Keys.Contains(id))
                    {
                        PoolTransactions.Remove(id);
                    }
                }
            }
            if (conn != null)
            {
                conn.Close();
            }
        }

        /// <summary>
        /// 创建一个新的连接
        /// </summary>
        /// <returns></returns>
        static Connection BuildConnection(int threadid)
        {
            if (typeof(T) == typeof(SqlConnection))
            {
                return new Connection(threadid, new SqlConnection(ConnectionString));
            }
            else if (typeof(T) == typeof(OleDbConnection))
            {
                return new Connection(threadid, new OleDbConnection(ConnectionString));
            }
            return null;
        }
    }
}
