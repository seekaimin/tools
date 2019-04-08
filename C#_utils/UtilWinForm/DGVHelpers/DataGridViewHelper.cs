using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data;
using Util.Common.DBConnectionHelpers;
using System.Data.OleDb;

namespace Util.WinForm.DGVHelpers
{
    /// <summary>
    /// DGV 帮助类
    /// </summary>
    public static class DataGridViewHelper
    {
        /// <summary>
        /// 检核文件和选择的行
        /// </summary>
        /// <param name="dgv">dgv</param>
        /// <param name="filename">默认文件名</param>
        /// <param name="notexportcolumns">排除的列</param>
        /// <param name="selectedcolumns">返回选择的列</param>
        /// <param name="path">返回文件路径</param>
        /// <returns>检核成功或失败</returns>
        static bool CheckFileAndColumns(this DataGridView dgv, string filename, string[] notexportcolumns, Dictionary<string, string> selectedcolumns, out string path)
        {
            path = "";
            if (dgv == null)
            {
                throw new Exception("datagridview is not exists!");
            }
            List<KeyValuePair<string, string>> dgvcolumns = new List<KeyValuePair<string, string>>();
            foreach (DataGridViewColumn column in dgv.Columns)
            {
                //判断是否需要排除此列
                if (notexportcolumns.Contains(column.Name)) continue;
                dgvcolumns.Add(new KeyValuePair<string, string>(column.Name, column.HeaderText));
            }
            using (FormSelectColumns select = new FormSelectColumns(dgvcolumns))
            {
                if (select.ShowDialog() == DialogResult.OK)
                {
                    foreach (string key in select.SelectedColumns.Keys)
                    {
                        selectedcolumns.Add(key, select.SelectedColumns[key]);
                    }
                }
            }
            if (selectedcolumns.Count == 0) return false;
            SaveFileDialog save = new SaveFileDialog()
            {
                FileName = filename,
                Filter = ".csv|*.csv|.xls|*.xls",   //|.xlsx|*.xlsx",
            };
            if (save.ShowDialog() == DialogResult.OK)
            {
                path = save.FileName;
                return true;
            }
            return false;
        }


        /// <summary>
        /// 将DataGridView中的数据导出到文件
        /// </summary>
        /// <param name="dgv">DGV</param>
        /// <param name="filename">默认文件名</param>
        /// <param name="sheetname">表名</param>
        /// <param name="notexportcolumns">需要排除的列</param>
        public static void Export(this DataGridView dgv, string filename = "default.csv", string sheetname = "Sheet1", params string[] notexportcolumns)
        {
            if (notexportcolumns == null)
            {
                notexportcolumns = new string[0];
            }
            string path;
            Dictionary<string, string> selectedcolumns = new Dictionary<string, string>();
            if (dgv.CheckFileAndColumns(filename, notexportcolumns, selectedcolumns, out path))
            {
                FileFormats fileformat = path.ToUpper().EndsWith(".CSV") ? FileFormats.CSV : FileFormats.EXCEL;
                List<string> heads = new List<string>();
                List<List<string>> rows = new List<List<string>>();
                if (fileformat == FileFormats.CSV)
                {
                    #region create data
                    foreach (KeyValuePair<string, string> column in selectedcolumns)
                    {
                        heads.Add(column.Value);
                    }
                    foreach (DataGridViewRow deg_row in dgv.Rows)
                    {
                        if (deg_row.IsNewRow)
                        {
                            break;
                        }
                        List<string> row = new List<string>();
                        foreach (KeyValuePair<string, string> i in selectedcolumns)
                        {
                            row.Add(deg_row.Cells[i.Key].FormattedValue == null ? "" : deg_row.Cells[i.Key].FormattedValue.ToString());
                        }
                        rows.Add(row);
                    }
                    #endregion
                    DataToCSV(path, heads, rows);
                }
                else
                {
                    DataTable dt = new DataTable(sheetname);
                    var keys = selectedcolumns.Select(p => p.Key).ToList();
                    foreach (string name in keys)
                    {
                        DataGridViewColumn column = dgv.Columns[name];
                        if (keys.Contains(column.Name))
                        {
                            DataColumn temp = new DataColumn(column.HeaderText, typeof(string));
                            dt.Columns.Add(temp);
                        }
                    }
                    foreach (DataGridViewRow row in dgv.Rows)
                    {
                        DataRow dt_row = dt.NewRow();
                        for (int i = 0; i < keys.Count(); i++)
                        {
                            dt_row[i] = row.Cells[keys[i]].FormattedValue;
                        }
                        dt.Rows.Add(dt_row);
                    }
                    DataToExcel(path, dt);
                }
                MessageBox.Show("write completed!");
            }
        }
        /// <summary>
        /// 通过Oledb的方式导出数据到excel
        /// </summary>
        /// <param name="path">excel路径</param>
        /// <param name="dt">要导出的数据</param>
        public static void DataToExcel(string path, DataTable dt)
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
                StringBuilder createsql = new StringBuilder("CREATE TABLE [" + dt.TableName + "] (");
                foreach (DataColumn dc in dt.Columns)
                {
                    createsql.Append("[" + dc.ColumnName + "] " + OleDbType.Char.ToString() + " ,");
                }
                createsql.Remove(createsql.Length - 1, 1);
                createsql.Append(")");

                new OleDbCommand(createsql.ToString(), conn).ExecuteNonQuery();
                #endregion
                OleDbDataAdapter oda = new OleDbDataAdapter("select * from [" + dt.TableName + "]", conn);
                OleDbCommandBuilder ocb = new OleDbCommandBuilder(oda);
                ocb.QuotePrefix = "[";
                ocb.QuoteSuffix = "]";
                oda.Fill(dt);
                oda.Update(dt);
            }
        }
        /// <summary>
        /// 通过直接写文件的方式导出数据到CSV
        /// </summary>
        /// <param name="path">excel路径</param>
        /// <param name="heads">头数据</param>
        /// <param name="rows">行数据</param>
        public static void DataToCSV(string path, List<string> heads, List<List<string>> rows)
        {
            #region write
            using (StreamWriter sw = new StreamWriter(path, false, Encoding.GetEncoding(0)))
            {
                StringBuilder head_line = new StringBuilder();
                foreach (string head in heads)
                {
                    head_line.Append(string.Format("\"{0}\"{1}", head, ","));
                }
                sw.WriteLine(head_line);
                foreach (List<string> row in rows)
                {
                    StringBuilder context_line = new StringBuilder();
                    foreach (string cell in row)
                    {
                        context_line.Append(string.Format("\"{0}\"{1}", cell, ","));
                    }
                    sw.WriteLine(context_line);
                }
                sw.Flush();
            }
            #endregion
        }


        /// <summary>
        /// 生产excel数据连接字符串
        /// </summary>
        /// <param name="path">文件地址</param>
        /// <param name="hasHeader">是否含有表头文件</param>
        /// <returns></returns>
        public static string GenerateExcelConnectionString(string path, bool? hasHeader = null)
        {
            return string.Format("Provider = Microsoft.Jet.OLEDB.4.0; Data Source ={0};Extended Properties=Excel 8.0", path);
            string excel_connection = string.Empty;

            if (!hasHeader.HasValue)
            {
                if (path.ToLower().EndsWith(".xls"))
                {
                    return string.Format("Provider = Microsoft.Jet.OLEDB.4.0; Data Source ={0};Extended Properties=Excel 8.0", path);
                }
                else
                {
                    return string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 12.0;HDR=NO;IMEX=1'", path);
                }
            }

            if (path.ToLower().EndsWith(".xls"))
            {
                excel_connection = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1};IMEX=1'"
                    , path
                    , hasHeader.Value ? "YES" : "NO");
            }
            else if (path.ToLower().EndsWith(".xlsx"))
            {
                excel_connection = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 12.0 Xml;HDR={1};IMEX=1'"
                    , path
                    , hasHeader.Value ? "YES" : "NO");
            }
            else
            {
                throw new NullReferenceException("Excel path is not legal!");
            }
            return excel_connection;
        }

        /// <summary>
        /// 从excel文件中获取数据
        /// </summary>
        /// <param name="path">excel文件路径</param>
        /// <param name="hasHeader">是否含有表头信息</param>
        /// <returns></returns>
        public static DataSet ExcelToData(string path, bool hasHeader)
        {
            DataSet ds = new DataSet();

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
                    oda.Fill(ds);
                }
            }
            return ds;
        }


    }
    /// <summary>
    /// 文件格式
    /// </summary>
    public enum FileFormats
    {
        /// <summary>
        /// CSV
        /// </summary>
        CSV,
        /// <summary>
        /// EXCEL
        /// </summary>
        EXCEL,
    }
}
