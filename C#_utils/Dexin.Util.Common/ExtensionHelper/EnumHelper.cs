using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dexin.Util.Common.ExtensionHelper
{
    /// <summary>
    /// 枚举扩展
    /// </summary>
    public static class EnumHelper
    {
        /// <summary>
        /// 将数字转化为相对应的枚举类型
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="obj">转化数值对象</param>
        /// <returns>相对应的枚举</returns>
        public static T ToEnum<T>(this Byte obj) where T : struct
        {
            return (T)Enum.ToObject(typeof(T), obj);
        }
        /// <summary>
        /// 将数字转化为相对应的枚举类型
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="obj">转化数值对象</param>
        /// <returns>相对应的枚举</returns>
        /// 
        public static T ToEnum<T>(this Int16 obj) where T : struct
        {
            return (T)Enum.ToObject(typeof(T), obj);
        }
        /// <summary>
        /// 将数字转化为相对应的枚举类型
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="obj">转化数值对象</param>
        /// <returns>相对应的枚举</returns>
        public static T ToEnum<T>(this Int32 obj) where T : struct
        {
            return (T)Enum.ToObject(typeof(T), obj);
        }
        /// <summary>
        /// 将数字转化为相对应的枚举类型
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="obj">转化数值对象</param>
        /// <returns>相对应的枚举</returns>
        public static T ToEnum<T>(this Int64 obj) where T : struct
        {
            return (T)Enum.ToObject(typeof(T), obj);
        }
        /// <summary>
        /// 将数字转化为相对应的枚举类型
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="obj">转化数值对象</param>
        /// <returns>相对应的枚举</returns>
        /// 
        public static T ToEnum<T>(this UInt16 obj) where T : struct
        {
            return (T)Enum.ToObject(typeof(T), obj);
        }
        /// <summary>
        /// 将数字转化为相对应的枚举类型
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="obj">转化数值对象</param>
        /// <returns>相对应的枚举</returns>
        public static T ToEnum<T>(this UInt32 obj) where T : struct
        {
            return (T)Enum.ToObject(typeof(T), obj);
        }
        /// <summary>
        /// 将数字转化为相对应的枚举类型
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="obj">转化数值对象</param>
        /// <returns>相对应的枚举</returns>
        public static T ToEnum<T>(this UInt64 obj) where T : struct
        {
            return (T)Enum.ToObject(typeof(T), obj);
        }

        /// <summary>
        /// 将字符串转化为相对应的枚举类型
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="obj">转化字符串对象</param>
        /// <param name="defaultValue">默认值</param>
        /// <param name="ignoreCase">忽略大小写</param>
        /// <returns>相对应的枚举</returns>
        public static T ToEnum<T>(this string obj, T defaultValue, bool ignoreCase = true) where T : struct
        {
            if (obj != null)
            {
                T result;
                if (Enum.TryParse(obj, ignoreCase, out result))
                {
                    return result;
                }
            }
            return defaultValue;
        }












        /// <summary>
        /// 转换枚举类成员为可遍历的集合对象
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <returns></returns>
        public static IEnumerable<T> EnumToList<T>() where T : struct
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }

        /// <summary>
        /// 将枚举转化为本地化后的KeyValuePair[string,  string]集合
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="localizeFun">本地化方法</param>
        /// <returns></returns>
        public static IList<KeyValuePair<string, string>> EnumToList<T>(Func<string, string> localizeFun) where T : struct
        {
            IList<KeyValuePair<string, string>> result = new List<KeyValuePair<string, string>>();
            foreach (T i in Enum.GetValues(typeof(T)))
            {
                result.Add(new KeyValuePair<string, string>(i.ToString(), localizeFun(i.ToString())));
            }
            return result;
        }

        /// <summary>
        /// 将枚举转化为本地化后的KeyValuePair集合
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <typeparam name="TKey">KeyValuePair.Key的类型</typeparam>
        /// <typeparam name="TValue">KeyValuePair.Value的类型</typeparam>
        /// <param name="funKey">KeyValuePair.Key的生成方法</param>
        /// <param name="funValue">KeyValuePair.Value的生成方法</param>
        /// <returns></returns>
        public static IList<KeyValuePair<TKey, TValue>> EnumToList<T, TKey, TValue>(Func<T, TKey> funKey, Func<T, TValue> funValue) where T : struct
        {
            IList<KeyValuePair<TKey, TValue>> result = new List<KeyValuePair<TKey, TValue>>();
            foreach (T i in EnumToList<T>())
            {
                result.Add(new KeyValuePair<TKey, TValue>(funKey(i), funValue(i)));
            }
            return result;
        }
    }
}
