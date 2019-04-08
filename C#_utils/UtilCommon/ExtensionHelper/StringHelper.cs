using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Util.Common.ExtensionHelper
{
    /// <summary>
    /// StringHelper
    /// </summary>
    public static class StringHelper
    {
        /// <summary>
        /// Regex r = new Regex("((25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]\\d|\\d)\\.){3}(25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]\\d|[1-9])", RegexOptions.None);
        /// </summary>
        /// <param name="infomation">需要匹配的信息</param>
        /// <param name="regex">匹配的正则表达式</param>
        /// <returns>匹配的结果字符串集合</returns>
        public static List<string> RegexMatch(this string infomation, Regex regex)
        {
            List<string> result = new List<string>();
            //匹配IP的正则表达式
            string tempinfo = infomation;
            do
            {
                Match mc = regex.Match(tempinfo);
                if (mc.Groups.Count > 0)
                {
                    string temp = mc.Groups[0].Value;
                    if (temp.IsNullOrEmpty())
                    {
                        break;
                    }
                    //Console.WriteLine(tempip);
                    result.Add(temp);
                    int index = tempinfo.IndexOf(temp);
                    int start = index + temp.Length;
                    tempinfo = tempinfo.Substring(start);
                }
                else
                {
                    break;
                }
            } while (true);
            return result;
        }

    }


}
