using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dexin.Util.Common.ExtensionHelper
{
    /// <summary>
    /// IList扩展类
    /// </summary>
    public static class IListHelper
    {
        /// <summary>
        /// 从指定集合中移除指定的集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">源集合</param>
        /// <param name="removing">要移除的集合</param>
        public static void Remove<T>(this IList<T> source, IEnumerable<T> removing)
        {
            foreach (T ir in removing.ToList())
            {
                source.Remove(ir);
            }
        }
    }
}
