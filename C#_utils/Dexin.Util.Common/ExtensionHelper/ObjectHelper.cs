using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Net;

namespace Dexin.Util.Common.ExtensionHelper
{
    /// <summary>
    /// Object扩展方法
    /// </summary>
    public static class ObjectHelper
    {
        /// <summary>
        /// ToString  Null为""
        /// </summary>
        /// <param name="obj">转化对象</param>
        /// <param name="defaultValue">默认值 = ""</param>
        /// <returns></returns>
        public static string ToStr(this string obj, string defaultValue = "")
        {
            return obj == null ? defaultValue : obj;
        }
        /// <summary>
        /// ToString  Null为""
        /// </summary>
        /// <param name="obj">转化对象</param>
        /// <param name="defaultValue">默认值 = ""</param>
        /// <returns></returns>
        public static string ToStr(this object obj, string defaultValue = "")
        {
            return obj == null ? defaultValue : obj.ToString();
        }
        /// <summary>
        /// 字符串是否为空
        /// </summary>
        /// <param name="obj">字符串</param>
        /// <returns>比较结果</returns>
        public static bool IsNullOrEmpty(this string obj)
        {
            return string.IsNullOrEmpty(obj);
        }
        /// <summary>
        /// 转化为时间
        /// </summary>
        /// <param name="obj">转化对象</param>
        /// <returns>转化结果 转化失败返回当前时间</returns>
        public static DateTime ToDateTime(this string obj)
        {
            return DateTime.Parse(obj);
        }
        /// <summary>
        /// 转化为时间
        /// </summary>
        /// <param name="obj">转化对象</param>
        /// <returns>转化结果</returns>
        public static DateTime ToDateTime(this object obj)
        {
            return Convert.ToDateTime(obj);
        }

        /// <summary>
        /// 转化为时间
        /// </summary>
        /// <param name="obj">转化对象</param>
        /// <param name="defaultVal">失败后返回默认值</param>
        /// <returns>转化结果 转化失败返回当前时间</returns>
        public static DateTime ToDateTime(this string obj, DateTime defaultVal)
        {
            DateTime result = DateTime.Now;
            if (!DateTime.TryParse(obj, out result))
            {
                return defaultVal;
            }
            return result;
        }
        /// <summary>
        /// 转化为时间
        /// </summary>
        /// <param name="obj">转化对象</param>
        /// <param name="defaultVal">失败后返回默认值</param>
        /// <returns>转化结果</returns>
        public static DateTime ToDateTime(this object obj, DateTime defaultVal)
        {
            try
            {
                return Convert.ToDateTime(obj);
            }
            catch
            {
                return defaultVal;
            }
        }



        /// <summary>
        /// 将字符串(true/false)转化为布尔
        /// </summary>
        /// <param name="obj">转化对象</param>
        /// <param name="defaultValue">默认值 = false</param>
        /// <returns>转化结果</returns>
        public static Boolean ToBoolean(this object obj, bool defaultValue = false)
        {
            Boolean result = defaultValue;
            try
            {
                result = Convert.ToBoolean(obj);
            }
            catch
            {
                result = defaultValue;
            }
            return result;
        }
        /// <summary>
        /// 转化为byte[]
        /// </summary>
        /// <param name="obj">转化对象</param>
        /// <returns>转化结果</returns>
        public static Byte[] ToBytes(this object obj)
        {
            return (Byte[])obj;
        }
        /// <summary>
        /// 转化为Int8
        /// </summary>
        /// <param name="obj">转化对象</param>
        /// <param name="defaultValue">默认值 = 0</param>
        /// <param name="numberStyle">样式</param>
        /// <returns>转化结果</returns>
        public static Byte ToInt8(this string obj, byte defaultValue = 0, NumberStyle numberStyle = NumberStyle.Dec)
        {
            Byte result = defaultValue;
            if (!obj.IsNullOrEmpty())
            {
                try
                {
                    result = Convert.ToByte(obj, (int)numberStyle);
                }
                catch
                {
                    result = defaultValue;
                }
            }
            return result;
        }
        /// <summary>
        /// 转化为Int16
        /// </summary>
        /// <param name="obj">转化对象</param>
        /// <param name="defaultValue">默认值=0</param>
        /// <param name="numberStyle">样式</param>
        /// <returns>转化结果</returns>
        public static Int16 ToInt16(this string obj, Int16 defaultValue = 0, NumberStyle numberStyle = NumberStyle.Dec)
        {
            Int16 result = defaultValue;
            if (!obj.IsNullOrEmpty())
            {
                try
                {
                    result = Convert.ToInt16(obj, (int)numberStyle);
                }
                catch
                {
                    result = defaultValue;
                }
            }
            return result;
        }
        /// <summary>
        /// 转化为Int16
        /// </summary>
        /// <param name="obj">转化对象</param>
        /// <param name="defaultValue">默认值=0</param>
        /// <returns>转化结果</returns>
        public static Int16 ToInt16(this object obj, Int16 defaultValue = 0)
        {
            Int16 result = defaultValue;
            if (obj != null)
            {
                try
                {
                    result = Convert.ToInt16(obj);
                }
                catch
                {
                    result = defaultValue;
                }
            }
            return result;
        }
        /// <summary>
        /// 转化为Int32
        /// </summary>
        /// <param name="obj">转化对象</param>
        /// <param name="defaultValue">默认为 = 0</param>
        /// <param name="numberStyle">样式</param>
        /// <returns>转化结果</returns>
        public static Int32 ToInt32(this string obj, Int32 defaultValue = 0, NumberStyle numberStyle = NumberStyle.Dec)
        {
            Int32 result = defaultValue;
            if (!obj.IsNullOrEmpty())
            {
                try
                {
                    result = Convert.ToInt32(obj, (int)numberStyle);
                }
                catch
                {
                    result = defaultValue;
                }
            }
            return result;
        }
        /// <summary>
        /// 转化为Int32
        /// </summary>
        /// <param name="obj">转化对象</param>
        /// <param name="defaultValue">默认为 = 0</param>
        /// <returns>转化结果</returns>
        public static Int32 ToInt32(this object obj, Int32 defaultValue = 0)
        {
            Int32 result = defaultValue;
            if (obj != null)
            {
                try
                {
                    result = Convert.ToInt32(obj);
                }
                catch
                {
                    result = defaultValue;
                }
            }
            return result;
        }
        /// <summary>
        /// 转化为Int64
        /// </summary>
        /// <param name="obj">转化对象</param>
        /// <param name="defaultValue">默认为 = 0</param>
        /// <param name="numberStyle">样式</param>
        /// <returns>转化结果</returns>
        public static Int64 ToInt64(this string obj, Int64 defaultValue = 0, NumberStyle numberStyle = NumberStyle.Dec)
        {
            Int64 result = defaultValue;
            if (!obj.IsNullOrEmpty())
            {
                try
                {
                    result = Convert.ToInt64(obj, (int)numberStyle);
                }
                catch
                {
                    result = defaultValue;
                }
            }
            return result;
        }
        /// <summary>
        /// 转化为Int32
        /// </summary>
        /// <param name="obj">转化对象</param>
        /// <param name="defaultValue">默认为 = 0</param>
        /// <returns>转化结果</returns>
        public static Int64 ToInt64(this object obj, Int64 defaultValue = 0)
        {
            Int64 result = defaultValue;
            if (obj != null)
            {
                try
                {
                    result = Convert.ToInt64(obj);
                }
                catch
                {
                    result = defaultValue;
                }
            }
            return result;
        }
        /// <summary>
        /// 转化为Decimal
        /// </summary>
        /// <param name="obj">转化对象</param>
        /// <param name="defaultValue">默认为 = 0</param>
        /// <returns>转化结果</returns>
        public static Decimal ToDecimal(this object obj, Int64 defaultValue = 0)
        {
            Decimal result = defaultValue;
            if (obj != null)
            {
                try
                {
                    result = Convert.ToDecimal(obj);
                }
                catch
                {
                    result = defaultValue;
                }
            }
            return result;
        }


        /// <summary>
        /// 内部转换
        /// </summary>
        /// <param name="str">需要转换的字符串</param>
        /// <returns></returns>
        public static UInt64 FormatToUInt64(this string str)
        {
            return (UInt64)FormatToInt64(str);
        }
        /// <summary>
        /// 内部转换
        /// </summary>
        /// <param name="str">需要转换的字符串</param>
        /// <returns></returns>
        public static Int64 FormatToInt64(this string str)
        {
            NumberStyle format = ExtensionHelper.NumberStyle.Dec;
            if (str.ToUpper().StartsWith("0X"))
            {
                format = NumberStyle.Hex;
            }
            return str.ToInt64(0, format);
        }


        /// <summary>
        /// 内部转换
        /// </summary>
        /// <param name="str">需要转换的字符串</param>
        /// <returns></returns>
        public static UInt32 FormatToUInt32(this string str)
        {
            return (UInt32)FormatToInt32(str);
        }
        /// <summary>
        /// 内部转换
        /// </summary>
        /// <param name="str">需要转换的字符串</param>
        /// <returns></returns>
        public static Int32 FormatToInt32(this string str)
        {
            NumberStyle format = ExtensionHelper.NumberStyle.Dec;
            if (str.ToUpper().StartsWith("0X"))
            {
                format = NumberStyle.Hex;
            }
            return str.ToInt32(0, format);
        }

        /// <summary>
        /// 内部转换
        /// </summary>
        /// <param name="str">需要转换的字符串</param>
        /// <returns></returns>
        public static UInt16 FormatToUInt16(this string str)
        {
            return (UInt16)FormatToInt64(str);
        }
        /// <summary>
        /// 内部转换
        /// </summary>
        /// <param name="str">需要转换的字符串</param>
        /// <returns></returns>
        public static Int16 FormatToInt16(this string str)
        {
            NumberStyle format = ExtensionHelper.NumberStyle.Dec;
            if (str.ToUpper().StartsWith("0X"))
            {
                format = NumberStyle.Hex;
            }
            return str.ToInt16(0, format);
        }


        /// <summary>
        /// 内部转换
        /// </summary>
        /// <param name="str">需要转换的字符串</param>
        /// <returns></returns>
        public static byte FormatToInt8(this string str)
        {
            NumberStyle format = ExtensionHelper.NumberStyle.Dec;
            if (str.ToUpper().StartsWith("0X"))
            {
                format = NumberStyle.Hex;
            }
            return str.ToInt8(0, format);
        }


        /// <summary>
        /// 转化为Int8
        /// </summary>
        /// <param name="obj">转化对象</param>
        /// <returns>转化结果</returns>
        public static Byte ToInt8(this object obj)
        {
            return Convert.ToByte(obj);
        }
        /// <summary>
        /// 转化为无符号 16
        /// </summary>
        /// <param name="obj">需要转化的对象</param>
        /// <returns></returns>
        public static UInt16 ToUInt16(this object obj)
        {
            return Convert.ToUInt16(obj);
        }
        /// <summary>
        /// 转化为无符号 32
        /// </summary>
        /// <param name="obj">需要转化的对象</param>
        /// <returns></returns>
        public static UInt32 ToUInt32(this object obj)
        {
            return Convert.ToUInt32(obj);
        }
        /// <summary>
        /// 转化为无符号 64
        /// </summary>
        /// <param name="obj">需要转化的对象</param>
        /// <returns></returns>
        public static UInt64 ToUInt64(this object obj)
        {
            return Convert.ToUInt64(obj);
        }

        /// <summary>
        /// 最佳字符串
        /// </summary>
        /// <typeparam name="T">追加数据类型</typeparam>
        /// <param name="sb">StringBuilder</param>
        /// <param name="obj">追加对象</param>
        /// <param name="count">追加数量</param>
        public static void AP<T>(this StringBuilder sb, T obj, int count = 1)
        {
            for (int i = 0; i < count; i++)
                sb.Append(obj);
        }


        /// <summary>
        /// 转化为IP地址
        /// </summary>
        /// <param name="obj">转化对象</param>
        /// <param name="defaultValue">默认值 = 127.0.0.1</param>
        /// <returns>转化结果</returns>
        public static IPAddress ToIPAdress(this string obj, string defaultValue = "127.0.0.1")
        {
            IPAddress result = IPAddress.Parse(defaultValue);
            if (!obj.IsNullOrEmpty())
            {
                if (!IPAddress.TryParse(obj, out result))
                {
                    result = IPAddress.Parse(defaultValue);
                }
            }
            return result;
        }
        /// <summary>
        /// 将源数据指定属性拷贝到存储对象中
        /// </summary>
        /// <typeparam name="T">存储数据类型</typeparam>
        /// <typeparam name="TResource">源数据类型</typeparam>
        /// <param name="obj">存储对象</param>
        /// <param name="resource">源对象</param>
        /// <param name="names">需要拷贝的字段名称</param>
        public static void CopyWith<T, TResource>(this T obj, TResource resource, params string[] names)
        {
            if (resource == null)
                throw new ArgumentNullException("resource");
            if (obj == null)
                throw new ArgumentNullException("obj");
            if (names == null || names.Length == 0)
                throw new ArgumentNullException("obj");
            PropertyInfo[] pGoal = typeof(T).GetProperties();
            PropertyInfo[] pResource = typeof(TResource).GetProperties();
            foreach (string name in names)
            {
                PropertyInfo p1 = pGoal.FirstOrDefault(p => p.Name.Equals(name));
                if (p1 == null)
                    continue;
                PropertyInfo p2 = pResource.FirstOrDefault(p => p.Name.Equals(name));
                if (p2 == null)
                    continue;
                p1.SetValue(obj, p2.GetValue(resource, null), null);
            }
        }
        /// <summary>
        /// 将源数据拷贝到存储对象中(排除某些属性)
        /// </summary>
        /// <typeparam name="T">存储数据类型</typeparam>
        /// <typeparam name="TResource">源数据类型</typeparam>
        /// <param name="obj">存储对象</param>
        /// <param name="resource">源对象</param>
        /// <param name="names">被排除的某些属性</param>
        public static void CopyWithout<T, TResource>(this T obj, TResource resource, params string[] names)
        {
            if (resource == null)
                throw new ArgumentNullException("resource");
            if (obj == null)
                throw new ArgumentNullException("obj");
            PropertyInfo[] pResource = typeof(TResource).GetProperties();
            foreach (PropertyInfo p1 in typeof(T).GetProperties())
            {
                if (names == null || names.Length == 0 || names.Contains(p1.Name))
                    continue;
                PropertyInfo p2 = pResource.FirstOrDefault(p => p.Name.Equals(p1.Name));
                if (p2 == null)
                    continue;
                p1.SetValue(obj, p2.GetValue(resource, null), null);
            }
        }
        /// <summary>
        /// 克隆对象
        /// </summary>
        /// <typeparam name="T">需要克隆的类型</typeparam>
        /// <param name="t">对象实体</param>
        /// <returns></returns>
        public static T Clone<T>(T t) where T : class,new()
        {
            T result = new T();
            foreach (PropertyInfo property in typeof(T).GetProperties())
            {
                property.SetValue(result, property.GetValue(t, null), null);
            }
            return result;
        }


        /// <summary>
        /// 将数组转化为字符串
        /// </summary>
        /// <typeparam name="T">数组类型</typeparam>
        /// <param name="items">数字</param>
        /// <param name="splitChar">需要分割的字符串  默认不分割</param>
        /// <param name="fun">进行处理后的字符串</param>
        /// <returns></returns>
        public static string ArrayToString<T>(this IEnumerable<T> items, string splitChar = "", Func<T, string> fun = null)
        {
            StringBuilder result = new StringBuilder();
            int index = 0;
            int count = items.Count();
            foreach (T item in items)
            {
                if (fun == null)
                {
                    result.Append(item);
                }
                else
                {
                    result.Append(fun(item));
                }
                if (++index < count)
                {
                    result.Append(splitChar);
                }
            }
            return result.ToString();
        }
        /// <summary>
        /// byte数组转化为字符串  x2 16进制显示
        /// </summary>
        /// <param name="items">需要转化的数组</param>
        /// <param name="splitChar">分隔符</param>
        /// <returns></returns>
        public static string ShowString(this byte[] items, string splitChar = "")
        {
            StringBuilder result = new StringBuilder();
            int index = 0;
            int count = items.Count();
            foreach (byte item in items)
            {
                result.Append(item.ToString("X2"));
                if (++index < count)
                {
                    result.Append(splitChar);
                }
            }
            return result.ToString();
        }
        /// <summary>
        /// 格式化字符串 调用string.Formt
        /// </summary>
        /// <param name="formatString">需要格式化的字符串</param>
        /// <param name="args">参数</param>
        /// <returns></returns>
        public static string Fmt(this String formatString, params object[] args)
        {
            return string.Format(formatString, args);
        }

        /// <summary>
        /// 比较数组是否一样
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static bool Eq(this byte[] p1, byte[] p2)
        {
            if (p1 == null && p2 == null)
            {
                return true;
            }
            if (p1 == null || p2 == null)
            {
                return false;
            }
            if (p1.Length == 0 && p2.Length == 0)
            {
                return true;
            }
            if (p1.Length != p2.Length)
            {
                return false;
            }
            for (int i = 0; i < p1.Length; i++)
            {
                if (p1[i] != p2[i])
                {
                    return false;
                }
            }
            return true;
        }


    }
    /// <summary>
    /// 数字样式
    /// </summary>
    public enum NumberStyle
    {
        /// <summary>
        /// 二进制
        /// </summary>
        Bin = 2,
        /// <summary>
        /// 八进制
        /// </summary>
        Oct = 8,
        /// <summary>
        /// 十进制
        /// </summary>
        Dec = 10,
        /// <summary>
        /// 十六进制
        /// </summary>
        Hex = 16,
    }
}
