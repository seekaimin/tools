using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.IO;

namespace Dexin.Util.Common.Excel
{
    /// <summary>
    /// 提供数据和excel的相关操作
    /// </summary>
    public class ExcelHelper
    {
        /// <summary>
        /// 生产excel数据连接字符串
        /// </summary>
        /// <param name="path">文件地址</param>
        /// <param name="hasHeader">是否含有表头文件</param>
        /// <returns></returns>
        private static string GenerateExcelConnectionString(string path, bool? hasHeader = null)
        {
            string strExcelConn = string.Empty;

            if (!hasHeader.HasValue)
            {
                return string.Format("Provider = Microsoft.Jet.OLEDB.4.0; Data Source ={0};Extended Properties=Excel 8.0", path);
            }

            if (path.ToLower().EndsWith(".xls"))
            {
                strExcelConn = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1};IMEX=1'"
                    , path
                    , hasHeader.Value ? "YES" : "NO");
            }
            else if (path.ToLower().EndsWith(".xlsx"))
            {
                strExcelConn = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 12.0 Xml;HDR={1};IMEX=1'"
                    , path
                    , hasHeader.Value ? "YES" : "NO");
            }
            else
            {
                throw new NullReferenceException("Excel path is not legal!");
            }
            return strExcelConn;
        }

        /// <summary>
        /// 从excel文件中获取数据
        /// </summary>
        /// <param name="path">excel文件路径</param>
        /// <param name="hasHeader">是否含有表头信息</param>
        /// <returns></returns>
        public static DataSet GetdataFromExcel(string path, bool hasHeader)
        {
            DataSet dsData = new DataSet();

            using (OleDbConnection conn = new OleDbConnection(GenerateExcelConnectionString(path, hasHeader)))
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                var tables = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                foreach (DataRow iRow in tables.Rows)
                {
                    string sheet = iRow["TABLE_NAME"].ToString();
                    if (!sheet.EndsWith("$"))
                        continue;
                    string strSql = string.Format("select * from [{0}]", sheet);
                    OleDbDataAdapter oda = new OleDbDataAdapter(strSql, conn);
                    oda.Fill(dsData);
                }
            }

            return dsData;
        }

        /// <summary>
        /// 通过Oledb的方式导出数据到excel
        /// </summary>
        /// <param name="path">excel路径</param>
        /// <param name="excelData">要导出的数据</param>
        public static void SavedataToExcelOledb(string path, DataTable excelData)
        {
            if (File.Exists(path))
                File.Delete(path);

            using (OleDbConnection conn = new OleDbConnection(GenerateExcelConnectionString(path)))
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                #region 创建表
                StringBuilder createsql = new StringBuilder("CREATE TABLE " + excelData.TableName + " (");
                foreach (DataColumn dc in excelData.Columns)
                {
                    createsql.Append("[" + dc.ColumnName + "] " + OleDbType.Char.ToString() + " ,");
                }
                createsql.Remove(createsql.Length - 1, 1);
                createsql.Append(")");

                new OleDbCommand(createsql.ToString(), conn).ExecuteNonQuery();
                #endregion
                OleDbDataAdapter oda = new OleDbDataAdapter("select * from " + excelData.TableName, conn);
                OleDbCommandBuilder ocb = new OleDbCommandBuilder(oda);
                ocb.QuotePrefix = "[";
                ocb.QuoteSuffix = "]";
                DataTable targetTable = new DataTable();
                oda.Fill(excelData);
                oda.Update(excelData);
            }
        }

        /// <summary>
        /// 通过直接写文件的方式导出数据到excel
        /// </summary>
        /// <param name="path">excel路径</param>
        /// <param name="excelData">要导出的数据</param>
        public static void SavedataToExcel(string path, DataTable excelData)
        {
            using (StreamWriter sw = new StreamWriter(path, false, Encoding.Default))
            {
                StringBuilder sb = new StringBuilder();
                foreach (DataColumn dc in excelData.Columns)
                {
                    sb.Append(dc.ColumnName + "\t");
                }
                sb.Append(Environment.NewLine);

                foreach (DataRow dr in excelData.Rows)
                {
                    foreach (DataColumn dc in excelData.Columns)
                    {
                        sb.Append("'" + dr[dc.ColumnName].ToString() + "\t");
                    }
                    sb.Append(Environment.NewLine);
                }

                sw.Write(sb.ToString());
                sw.Flush();
            }
        }
        #region 创建DataTable通过字段列表
        /// <summary>
        /// 创建DataTable
        /// </summary>
        /// <param name="columnNames">列名称</param>
        /// <returns>DataTable</returns>
        public static DataTable CreatDataTable(IList<string> columnNames)
        {
            if (columnNames == null)
            {
                throw new Exception("columnNames is not null");
            }
            DataTable dt = new DataTable();
            DataColumn dc = null;
            foreach (string str in columnNames)
            {
                dc = new DataColumn(str);
                dt.Columns.Add(dc);
            }
            return dt;
        }
        #endregion
    }
}
