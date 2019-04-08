using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Util.Common.DBConnectionHelpers;
using Util.Common.ExtensionHelper;
using System.IO;
using System.Data.OleDb;

namespace ExcelCompare
{
    public class ExcelFileHelper
    {
        /// <summary>
        /// 按区位分析
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <param name="console">控制台回调</param>
        /// <returns></returns>
        public static List<Item> readFile(string filename, Action<string> console)
        {
            if (!File.Exists(filename))
            {
                throw new Exception("{0}文件不存在!".Fmt(filename));
            }
            console("开始分析文件:{0}".Fmt(filename));
            StringBuilder sb = new StringBuilder();
            //key : 
            List<Item> result = new List<Item>();
            try
            {
                List<string> items = new List<string>();
                OleDbConnectionPool.SetOleDbConnectionString(filename, true);
                DataTable dt = null;
                string sql = null;
                #region 格式一
                try
                {
                    string name = OleDbConnectionPool.GetFirstSheetName();
                    sql = string.Format("SELECT 子件编码	,子件名称,子件规格,基本用量,区位,备注 FROM [{0}]", name);
                    console("尝试按[{0}]格式分析文件".Fmt(sql));
                    dt = OleDbConnectionPool.ExecuteDataTable(CommandType.Text, sql, null);
                }
                catch (Exception ex)
                {
                    console("按[{0}]格式分析出现异常!".Fmt(sql));
                    console(ex.Message);
                }
                #endregion
                #region 格式二
                if (dt == null)
                {
                    try
                    {
                        //new OleDbDataAdapter("select * from [Sheet1$]", OleDbConnectionPool.ConnectionString).Fill(dsAdd, "A");
                        //int columnsint = dsAdd.Tables["A"].Columns.Count;
                        //int rowsint = dsAdd.Tables["A"].Rows.Count;
                        //Console.WriteLine(rowsint);
                        string name = OleDbConnectionPool.GetFirstSheetName();
                        sql = string.Format("SELECT 编号,名称,型号,数量,位置代号,备注 FROM [{0}]", name); //查询字符串
                        console("尝试按[{0}]格式分析文件".Fmt(sql));
                        dt = OleDbConnectionPool.ExecuteDataTable(CommandType.Text, sql, null);
                    }
                    catch (Exception ex)
                    {
                        console("按[{0}]格式分析出现异常!".Fmt(sql));
                        console(ex.Message);
                    }
                }
                #endregion


                if (dt == null)
                {
                    throw new Exception("文件[{0}]分析出现错误".Fmt(filename));
                }



                int cellsize = dt.Columns.Count;
                StringBuilder ssss = new StringBuilder();
                foreach (DataRow row in dt.Rows)
                {
                    ssss.Append("\r\n#######################################\r\n");
                    for (int i = 0; i < cellsize; i++)
                    {
                        ssss.Append(row[i]);
                        ssss.Append("-");
                    }
                    ssss.Append("\r\n#######################################\r\n");
                }
                console("##########################数据分析  开始##########################");
                console(ssss.ToString());
                console("##########################数据分析  结束##########################");


                foreach (DataRow row in dt.Rows)
                {
                    string code = row[0].ToStr().Trim();
                    //排除空行
                    if (code.Length <= 0)
                        continue;

                    if (code.Equals("1010020824"))
                    {
                        Console.WriteLine("1010020824");
                    }
                    string name = row[1].ToString();
                    string mode = row[2].ToString();
                    decimal count = row[3].ToString().ToDecimal();
                    string positions = Convert.ToString(row[4]);
                    string remark = row[5].ToString();
                    Item item = new Item()
                    {
                        Code = code,
                        Name = name,
                        Mode = mode,
                        Remark = remark,
                        Count = count,
                        Position = positions,
                    };
                    result.Add(item);
                    string tempstring = "{0}#{1}#{2}#{3}#{4}#{5}".Fmt(code, name, mode, count, positions, remark);
                    items.Add(tempstring);
                    sb.Append(tempstring);
                    sb.Append("\r\n");
                }
                OleDbConnectionPool.Completed(true);
            }
            catch (Exception ex)
            {
                OleDbConnectionPool.Completed(false);
                throw new Exception("BOM文件分析失败 \r\n {0} \r\n {1}".Fmt(filename, ex.Message));
            }
            finally
            {
                OleDbConnectionPool.DisposePool();
            }
            console("文件分析完成!");
            //Console.WriteLine(sb.ToStr());
            Tools.d("####################################开始[0]####################################", filename);
            Tools.d(sb.ToStr());
            Tools.d("####################################结束[0]####################################", filename);
            return result;
        }
    }
}
