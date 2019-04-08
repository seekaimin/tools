using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Dexin.Util.Common
{
    /// <summary>
    /// 提供对象属性管理的相关方法
    /// </summary>
    public static class ObjectPropertyMapper
    {
        /// <summary>
        /// 从一个对象拷贝数据到另外一个对象
        /// </summary>
        /// <typeparam name="T1">源数据类型</typeparam>
        /// <typeparam name="T2">目标对象类型</typeparam>
        /// <param name="entitySource">数据源</param>
        /// <param name="entityTarget">数据目标</param>
        /// <param name="excudeProperties">要排除的属性</param>
        public static void Map<T1, T2>(T1 entitySource, T2 entityTarget, params string[] excudeProperties)
        {
            foreach (PropertyInfo property in entityTarget.GetType().GetProperties())
            {
                if (excudeProperties.Contains(property.Name))
                    continue;

                property.SetValue(entityTarget
                    , entitySource.GetType().GetProperty(property.Name).GetValue(entitySource, null)
                    , null);
            }
        }

        /// <summary>
        /// 从一个对象拷贝数据到另外一个对象
        /// </summary>
        /// <typeparam name="T1">源数据类型</typeparam>
        /// <typeparam name="T2">目标对象类型</typeparam>
        /// <param name="entitySource">数据源</param>
        /// <param name="entityTarget">数据目标</param>
        /// <param name="ignoreError">是否忽略错误</param>
        /// <param name="excudeProperties">要排除的属性</param>
        public static void Map<T1, T2>(T1 entitySource, T2 entityTarget, bool ignoreError, params string[] excudeProperties)
        {
            foreach (PropertyInfo property in entityTarget.GetType().GetProperties())
            {
                try
                {
                    if (excudeProperties.Contains(property.Name))
                        continue;

                    property.SetValue(entityTarget
                        , entitySource.GetType().GetProperty(property.Name).GetValue(entitySource, null)
                        , null);
                }
                catch (Exception ex)
                {
                    if (!ignoreError)
                        throw ex;
                }
            }
        }
    }
}
