using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Util.Common.ExtensionHelper;

namespace Util.Common.DBConnectionHelpers
{
    /// <summary>
    /// 连接池
    /// </summary>
    public class SqlConnectionPool : ConnectionPool<SqlConnection>
    {
        /// <summary>
        /// 杀掉其他链接  是本连接独占数据库资源
        /// </summary>
        /// <param name="dbname">数据库名称</param>
        public static string getKillOtherConnectionString(string dbname)
        {
            string name = dbname;
            if (dbname.ToUpper().StartsWith("DBO."))
            {
                name = dbname.Substring(4, dbname.Length - 4);
            }
            string sql = @"
            DECLARE @sql NVARCHAR(max)
            DECLARE @SPID NVARCHAR(max)
            DECLARE Mycursor CURSOR
            FOR SELECT spid FROM master..sysprocesses  WHERE dbid=DB_ID('{0}') 
            OPEN Mycursor 
            FETCH NEXT FROM Mycursor INTO @SPID
            WHILE @@FETCH_STATUS = 0 
                BEGIN
                    SET @sql = ' kill ' + @SPID
                    EXEC(@sql)
                    FETCH NEXT FROM Mycursor INTO @SPID
                END
            CLOSE Mycursor
            DEALLOCATE Mycursor
            ".Fmt(name);
            return sql;
        }
        /// <summary>
        /// 检核数据库是否存在
        /// </summary>
        /// <param name="dbname">数据库名称</param>
        /// <returns></returns>
        public static bool CheckDBIsExist(string dbname)
        {
            string check_sql = "SELECT DB_ID('{0}') AS ID".Fmt(dbname);
            Object obj = ExecuteScalar(CommandType.Text, check_sql, null);
            long id = obj.ToInt64();
            return id > 0;
        }
        /// <summary>
        /// 创建数据库
        /// </summary>
        /// <param name="dbname">数据库名称</param>
        public static void CreateDataBase(string dbname)
        {
            string create_sql =getKillOtherConnectionString(dbname) + @" 
            
            IF ((SELECT DB_ID('{0}') AS ID)>0) 
            BEGIN
                DROP DATABASE {0}
            END
            CREATE DATABASE {0};
                                                "
                .Fmt(dbname);
            ExecuteNonQuery(CommandType.Text, create_sql, null);
        }
        /// <summary>
        /// 还原数据库
        /// </summary>
        /// <param name="dbname">数据库名称</param>
        /// <param name="fullpath">还原文件全路径</param>
        public static void Restore(string dbname, string fullpath)
        {
            string sql = getKillOtherConnectionString(dbname) + @" 
RESTORE DATABASE {0} FROM DISK = '{1}' WITH REPLACE ".Fmt(dbname, fullpath);
            ExecuteNonQuery(CommandType.Text, sql, null);
        }
        /// <summary>
        /// 备份数据库
        /// </summary>
        /// <param name="dbname">数据库名称</param>
        /// <param name="fullpath">备份全路径</param>
        public static void Backup(string dbname, string fullpath)
        {
            string sql = " BACKUP DATABASE {0} TO DISK = '{1}' ".Fmt(dbname, fullpath);
            ExecuteNonQuery(CommandType.Text, sql, null);
        }

        /// <summary>
        /// ExcuteReader
        /// </summary>
        /// <param name="cmdType">指定如何解释命令字符串</param>
        /// <param name="cmdText">获取或设置要对数据源执行的 Transact-SQL 语句、表名或存储过程</param>
        /// <param name="parameter">参数</param>
        /// <returns>提供一种方法来读取一个或多个通过在数据源执行命令所获得的只进结果集流，这是由访问关系数据库的 .NET Framework 数据提供程序实现的</returns>
        public static SqlDataReader ExecuteReader(CommandType cmdType, string cmdText, params SqlParameter[] parameter)
        {
            SqlCommand cmd = new SqlCommand();
            PrepareCommand(cmd, cmdType, cmdText, parameter);
            SqlDataReader reader = CurrentTransaction == null ? cmd.ExecuteReader(CommandBehavior.CloseConnection) : cmd.ExecuteReader();
            cmd.Parameters.Clear();
            return reader;
        }
        /// <summary>
        /// 对连接执行 Transact-SQL 语句并返回受影响的行数
        /// </summary>
        /// <param name="cmdType">指定如何解释命令字符串</param>
        /// <param name="cmdText">获取或设置要对数据源执行的 Transact-SQL 语句、表名或存储过程</param>
        /// <param name="parameter">参数</param>
        /// <returns>受影响的行数</returns>
        public static int ExecuteNonQuery(CommandType cmdType, string cmdText, params SqlParameter[] parameter)
        {
            SqlCommand cmd = new SqlCommand();
            PrepareCommand(cmd, cmdType, cmdText, parameter);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }
        /// <summary>
        /// 执行查询，并返回查询所返回的结果集中第一行的第一列。忽略其他列或行
        /// </summary>
        /// <param name="cmdType">指定如何解释命令字符串</param>
        /// <param name="cmdText">获取或设置要对数据源执行的 Transact-SQL 语句、表名或存储过程</param>
        /// <param name="parameter">参数</param>
        /// <returns>结果集中第一行的第一列；如果结果集为空，则为空引用（在 Visual Basic 中为 Nothing）。返回的最大字符数为 2033 个字符</returns>
        public static object ExecuteScalar(CommandType cmdType, string cmdText, params SqlParameter[] parameter)
        {
            SqlCommand cmd = new SqlCommand();
            PrepareCommand(cmd, cmdType, cmdText, parameter);
            object val = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return val;
        }
        ///<summary>
        /// 分页查询 不调用存储过程
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public static SqlDataReader ExecuteReader(PageQuery<SqlParameter> query)
        {
            //获取当前总记录数
            object total = ExecuteScalar(CommandType.Text, string.Format("SELECT COUNT(1) FROM {0} WHERE 1 = 1 {1}", query.TableName, query.Condiction), query.CmdParameters.ToArray());
            query.TotalCount = total == null ? 0 : Convert.ToInt32(total);
            return ExecuteReader(CommandType.Text, query.GetSql(), query.GetParameters().ToArray());
        }
    }
}
