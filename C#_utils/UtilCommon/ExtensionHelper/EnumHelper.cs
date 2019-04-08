using System;
using System.Collections.Generic;

namespace Util.Common.ExtensionHelper
{
    /// <summary>
    /// 关于枚举的扩展方法
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
        /// 将枚举转化为Int-String集合
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="localFun">是否需要本地化</param>
        /// <returns></returns>
        public static IList<KeyValuePair<Int32, String>> ToIntStringList<T>(Func<T, string> localFun = null) where T : struct
        {
            IList<KeyValuePair<Int32, String>> result = new List<KeyValuePair<Int32, String>>();
            foreach (T i in Enum.GetValues(typeof(T)))
            {
                if (localFun == null)
                {
                    result.Add(new KeyValuePair<Int32, String>(i.ToInt32(), i.ToString()));
                }
                else
                {
                    result.Add(new KeyValuePair<Int32, String>(i.ToInt32(), localFun(i)));
                }
            }
            return result;
        }
        /// <summary>
        /// 将枚举转化为String-String集合
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="localFun">是否需要本地化</param>
        /// <returns></returns>
        public static IList<KeyValuePair<String, String>> ToStringStringList<T>(Func<string, string> localFun = null) where T : struct
        {
            IList<KeyValuePair<String, String>> result = new List<KeyValuePair<String, String>>();
            foreach (T i in Enum.GetValues(typeof(T)))
            {
                if (localFun == null)
                {
                    result.Add(new KeyValuePair<String, String>(i.ToString(), i.ToString()));
                }
                else
                {
                    result.Add(new KeyValuePair<String, String>(i.ToString(), localFun(i.ToString())));
                }
            }
            return result;
        }

        /// <summary>
        /// 将枚举转化为T-String集合
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="localFun">是否需要本地化</param>
        /// <returns></returns>
        public static IList<KeyValuePair<T, String>> ToTStringList<T>(Func<T, string> localFun = null) where T : struct
        {
            IList<KeyValuePair<T, String>> result = new List<KeyValuePair<T, String>>();
            foreach (T i in Enum.GetValues(typeof(T)))
            {
                if (localFun == null)
                {
                    result.Add(new KeyValuePair<T, String>(i, i.ToString()));
                }
                else
                {
                    result.Add(new KeyValuePair<T, String>(i, localFun(i)));
                }
            }
            return result;
        }

    }
}
