using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;

namespace Dexin.Util.Common.Text
{
    public class TextHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="dt">数据表</param>
        /// <param name="needTableHeader">表头</param>
        public static void SaveTableToTXT(string path, DataTable dt, bool needTableHeader = true)
        {

            FileInfo fi = new FileInfo(path);
            DirectoryInfo dir = fi.Directory;
            //AddDirectorySecurity(dir.FullName, "everyone", FileSystemRights.FullControl, AccessControlType.Allow);
            if (fi.Exists)
            {
                fi.Delete();
            }
            IList<string> lines = new List<string>();
            int dccnt = dt.Columns.Count;
            if (needTableHeader == true)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var dc in dt.Columns)
                {
                    sb.Append(dc.ToString() + ",");
                }
                lines.Add(sb.ToString().TrimEnd(','));
            }
            foreach (DataRow dr in dt.Rows)
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < dccnt; i++)
                {
                    sb.Append(dr[i].ToString() + ",");
                }
                lines.Add(sb.ToString().TrimEnd(','));
            }
            using (StreamWriter sw = fi.CreateText())
            {
                foreach (var item in lines)
                {
                    sw.WriteLine(item);
                }
            }
            //RemoveDirectorySecurity(dir.FullName, "everyone", FileSystemRights.FullControl, AccessControlType.Allow);
        }
        public static bool IsValidUrl(string url)
        {
            var strRegex = "^((http|rtp)?://)(([0-9]{1,3}.){3}[0-9]{1,3}:[0-9]{1,4})$";
            return Regex.IsMatch(url, strRegex);
        }
    }
}
