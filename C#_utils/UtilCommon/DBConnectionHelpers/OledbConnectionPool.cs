using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;

namespace Util.Common.DBConnectionHelpers
{
    /// <summary>
    /// OleDb连接池
    /// </summary>
    public class OleDbConnectionPool : ConnectionPool<OleDbConnection>
    {
        /// <summary>
        /// 设置连接字符串
        /// </summary>
        /// <param name="path">file path</param>
        /// <param name="hasHeader">是否有头</param>
        public static void SetOleDbConnectionString(string path, bool? hasHeader)
        {
            string strExcelConn = string.Empty;

            if (!hasHeader.HasValue)
            {
                ConnectionString = string.Format("Provider = Microsoft.Jet.OLEDB.4.0; Data Source ={0};Extended Properties=Excel 8.0", path);
                return;
            }
            if (path.ToLower().EndsWith(".xls"))
            {
                ConnectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1};IMEX=1'"
                    , path
                    , hasHeader.Value ? "YES" : "NO");
            }
            else if (path.ToLower().EndsWith(".xlsx"))
            {
                ConnectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 12.0 Xml;HDR={1};IMEX=1'"
                    , path
                    , hasHeader.Value ? "YES" : "NO");
            }
            else
            {
                throw new NullReferenceException("Excel path is not legal!");
            }
        }
        /// <summary>
        /// ExcuteReader
        /// </summary>
        /// <param name="cmdType">指定如何解释命令字符串</param>
        /// <param name="cmdText">获取或设置要对数据源执行的 Transact-SQL 语句、表名或存储过程</param>
        /// <param name="parameter">参数</param>
        /// <returns>提供一种方法来读取一个或多个通过在数据源执行命令所获得的只进结果集流，这是由访问关系数据库的 .NET Framework 数据提供程序实现的</returns>
        public static OleDbDataReader ExecuteReader(CommandType cmdType, string cmdText, params OleDbParameter[] parameter)
        {
            OleDbCommand cmd = new OleDbCommand();
            PrepareCommand(cmd, cmdType, cmdText, parameter);
            OleDbDataReader reader = CurrentTransaction == null ? cmd.ExecuteReader(CommandBehavior.CloseConnection) : cmd.ExecuteReader();
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
        public static int ExecuteNonQuery(CommandType cmdType, string cmdText, params OleDbParameter[] parameter)
        {
            OleDbCommand cmd = new OleDbCommand();
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
        public static object ExecuteScalar(CommandType cmdType, string cmdText, params OleDbParameter[] parameter)
        {
            OleDbCommand cmd = new OleDbCommand();
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
        public static OleDbDataReader ExecuteReader(PageQuery<OleDbParameter> query)
        {
            //获取当前总记录数
            object total = ExecuteScalar(CommandType.Text, string.Format("SELECT COUNT(1) FROM {0} WHERE 1 = 1 {1}", query.TableName, query.Condiction), query.CmdParameters.ToArray());
            query.TotalCount = total == null ? 0 : Convert.ToInt32(total);
            return ExecuteReader(CommandType.Text, query.GetSql(), query.GetParameters().ToArray());
        }

        /// <summary>
        /// 执行查询，并返回查询所返回的结果集中第一行的第一列。忽略其他列或行
        /// </summary>
        /// <param name="cmdType">指定如何解释命令字符串</param>
        /// <param name="cmdText">获取或设置要对数据源执行的 Transact-SQL 语句、表名或存储过程</param>
        /// <param name="parameter">参数</param>
        /// <returns>DataSet</returns>
        public static DataTable ExecuteDataTable(CommandType cmdType,
            string cmdText, params OleDbParameter[] parameter)
        {
            OleDbCommand cmd = new OleDbCommand();
            PrepareCommand(cmd, cmdType, cmdText, parameter);
            OleDbDataAdapter oda = new OleDbDataAdapter(cmd);
            DataSet ds = new DataSet();
            oda.Fill(ds);
            return ds.Tables[0];
        }
        
        /// <summary>
        /// 执行查询，并返回查询所返回的结果集中第一行的第一列。忽略其他列或行
        /// </summary>
        /// <param name="cmdType">指定如何解释命令字符串</param>
        /// <param name="cmdText">获取或设置要对数据源执行的 Transact-SQL 语句、表名或存储过程</param>
        /// <param name="parameter">参数</param>
        /// <returns>DataSet</returns>
        public static string GetFirstSheetName()
        {
            using (OleDbConnection conn = new OleDbConnection(ConnectionString))
            {
                conn.Open();
                DataTable sheetsName = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "Table" }); //得到所有sheet的名字
                string firstSheetName = sheetsName.Rows[1][2].ToString(); //得到第一个sheet的名字

                /*
                string sql = string.Format("SELECT * FROM [{0}]", firstSheetName); //查询字符串
                //string sql = string.Format("SELECT * FROM [{0}] WHERE [日期] is not null", firstSheetName); //查询字符串

                OleDbDataAdapter ada = new OleDbDataAdapter(sql, connstring);
                DataSet set = new DataSet();
                ada.Fill(set);
                return set.Tables[0];
                 * */
                return firstSheetName;
            }

        }
    }
}
