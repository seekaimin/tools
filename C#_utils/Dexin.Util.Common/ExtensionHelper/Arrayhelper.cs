using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dexin.Util.Common.ExtensionHelper;

namespace Dexin.Util.Common.ExtensionHelper
{
    /// <summary>
    /// 数组帮助类
    /// </summary>
    public static class ArrayHelper
    {
        /// <summary>
        /// 比较两个字节是否相同
        /// </summary>
        /// <param name="source">字节1</param>
        /// <param name="comparison">字节2</param>
        /// <returns></returns>
        public static bool CompareBytes(byte[] source, byte[] comparison)
        {
            int count = source.Length;
            if (source.Length != comparison.Length)
                return false;
            for (int i = 0; i < count; i++)
                if (source[i] != comparison[i])
                    return false;
            return true;
        }



        /// <summary>
        /// 将字符串转换为buff
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="format">格式化类型  默认值16</param>
        /// <returns></returns>
        public static Byte[] StringToBytes(this string str, int format = 16)
        {
            IList<byte> result = new List<byte>();
            for (int i = 0; i < str.Length; )
            {
                result.Add(Convert.ToByte(str.Substring(i, 2), format));
                i += 2;
            }
            return result.ToArray();
        }

        /// <summary>
        /// 将buff按顺序显示为字符串
        /// </summary>
        /// <param name="buff">buff</param>
        /// <param name="splitChar">分隔符</param>
        /// <returns></returns>
        public static string BytesToString(this Byte[] buff, string splitChar = "")
        {
            return BitConverter.ToString(buff).Replace("-", splitChar);
        }
        /// <summary>
        /// 将buff按顺序显示为字符串
        /// </summary>
        /// <param name="buff">buff</param>
        /// <param name="splitChar">分隔符</param>
        /// <param name="format">X2   X4    X6</param>
        /// <returns></returns>
        public static string BytesToString(this Byte[] buff, string splitChar, string format)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte i in buff)
            {
                string temp = i.ToString(format);
                sb.Append(temp);
            }
            return BitConverter.ToString(buff).Replace("-", splitChar);
        }




        #region 数据拷贝
        /// <summary>
        /// 拷贝方法
        /// </summary>
        /// <param name="obj">目标数组</param>
        /// <param name="resource">源数据</param>
        /// <param name="index">带引用的下标</param>
        public static void Copy(this Byte[] obj, Byte resource, ref int index)
        {
            obj[index++] = resource;
        }
        /// <summary>
        /// 拷贝方法
        /// </summary>
        /// <param name="obj">目标数组</param>
        /// <param name="resource">源数据</param>
        /// <param name="index">带引用的下标</param>
        public static void Copy(this Byte[] obj, Int16 resource, ref int index)
        {
            Byte[] temp = BitConverter.GetBytes(resource.ToNetWorkNumber(NetWorkEnum.HostToNetWork));
            obj.CopyBytes(temp, ref index);
        }
        /// <summary>
        /// 拷贝方法
        /// </summary>
        /// <param name="obj">目标数组</param>
        /// <param name="resource">源数据</param>
        /// <param name="index">带引用的下标</param>
        public static void Copy(this Byte[] obj, UInt16 resource, ref int index)
        {
            Byte[] temp = BitConverter.GetBytes(resource.ToNetWorkNumber(NetWorkEnum.HostToNetWork));
            obj.CopyBytes(temp, ref index);
        }
        /// <summary>
        /// 拷贝方法
        /// </summary>
        /// <param name="obj">目标数组</param>
        /// <param name="resource">源数据</param>
        /// <param name="index">带引用的下标</param>
        public static void Copy(this Byte[] obj, Int32 resource, ref int index)
        {
            Byte[] temp = BitConverter.GetBytes(resource.ToNetWorkNumber(NetWorkEnum.HostToNetWork));
            obj.CopyBytes(temp, ref index);
        }
        /// <summary>
        /// 拷贝方法
        /// </summary>
        /// <param name="obj">目标数组</param>
        /// <param name="resource">源数据</param>
        /// <param name="index">带引用的下标</param>
        public static void Copy(this Byte[] obj, UInt32 resource, ref int index)
        {
            Byte[] temp = BitConverter.GetBytes(resource.ToNetWorkNumber(NetWorkEnum.HostToNetWork));
            obj.CopyBytes(temp, ref index);
        }
        /// <summary>
        /// 拷贝方法
        /// </summary>
        /// <param name="obj">目标数组</param>
        /// <param name="resource">源数据</param>
        /// <param name="index">带引用的下标</param>
        public static void Copy(this Byte[] obj, Int64 resource, ref int index)
        {
            Byte[] temp = BitConverter.GetBytes(resource.ToNetWorkNumber(NetWorkEnum.HostToNetWork));
            obj.CopyBytes(temp, ref index);
        }
        /// <summary>
        /// 拷贝方法
        /// </summary>
        /// <param name="obj">目标数组</param>
        /// <param name="resource">源数据</param>
        /// <param name="index">带引用的下标</param>
        public static void Copy(this Byte[] obj, UInt64 resource, ref int index)
        {
            Byte[] temp = BitConverter.GetBytes(resource.ToNetWorkNumber(NetWorkEnum.HostToNetWork));
            obj.CopyBytes(temp, ref index);
        }
        /// <summary>
        /// 拷贝数组
        /// </summary>
        /// <param name="obj">目标数组</param>
        /// <param name="resource">源</param>
        /// <param name="index">带引用的下标</param>
        public static void CopyBytes(this Byte[] obj, Byte[] resource, ref int index)
        {
            Array.Copy(resource, 0, obj, index, resource.Length);
            index += resource.Length;
        }
        /// <summary>
        /// 拷贝数组
        /// </summary>
        /// <param name="buff">目标数组</param>
        /// <param name="resource">源</param>
        /// <param name="length">数据长度</param>
        /// <param name="index">目标数组起始位置</param>
        public static void CopyBytes(this Byte[] buff, Byte[] resource, int length, ref int index)
        {
            Array.Copy(resource, 0, buff, index, length);
            index += length;
        }
        /// <summary>
        /// 拷贝数组
        /// </summary>
        /// <param name="buff">目标数组</param>
        /// <param name="sourceindex">数据源带引用的下标</param>
        /// <param name="length">数据长度</param>
        /// <param name="resource">源</param>
        /// <param name="index">目标数组起始下标</param>
        public static void CopyBytes(this Byte[] buff, Byte[] resource, int sourceindex, int length, ref int index)
        {
            Array.Copy(resource, sourceindex, buff, index, length);
            index += length;
        }
        /// <summary>
        /// 拷贝字符串 末尾截断为0
        /// </summary>
        /// <param name="obj">目标数组</param>
        /// <param name="resource">字符串</param>
        /// <param name="length">buff长度</param>
        /// <param name="encoding">编码方式</param>
        /// <param name="index">带引用的起始坐标</param>
        public static void CopyString(this Byte[] obj, string resource, int length, Encoding encoding, ref int index)
        {
            string temp = resource.ToStr();
            byte[] tempArray = new byte[length];
            while (true)
            {
                byte[] buff = encoding.GetBytes(temp);
                if (buff.Length > length - 1)
                {
                    temp = temp.Substring(0, temp.Length - 1);
                    continue;
                }
                tempArray.CopyBytes(buff, 0, buff.Length);
                break;
            }
            obj.CopyBytes(tempArray, ref index);
        }
        /// <summary>
        /// 拷贝字符串 末尾不截断
        /// </summary>
        /// <param name="obj">目标数组</param>
        /// <param name="resource">字符串</param>
        /// <param name="encoding">编码方式</param>
        /// <param name="index">带引用的起始坐标</param>
        public static void CopyString2(this Byte[] obj, string resource, Encoding encoding, ref int index)
        {
            string temp = resource.ToStr();
            byte[] tempArray = encoding.GetBytes(temp);
            obj.CopyBytes(tempArray, ref index);
        }

        /// <summary>
        /// 拷贝方法
        /// </summary>
        /// <param name="obj">目标数组</param>
        /// <param name="resource">源数据</param>
        /// <param name="index">下标</param>
        public static void Copy(this Byte[] obj, Byte resource, int index = 0)
        {
            obj.Copy(resource, ref index);
        }
        /// <summary>
        /// 拷贝方法
        /// </summary>
        /// <param name="obj">目标数组</param>
        /// <param name="resource">源数据</param>
        /// <param name="index">起始下标</param>
        public static void Copy(this Byte[] obj, Int16 resource, int index = 0)
        {
            obj.Copy(resource, ref index);
        }
        /// <summary>
        /// 拷贝方法
        /// </summary>
        /// <param name="obj">目标数组</param>
        /// <param name="resource">源数据</param>
        /// <param name="index">起始下标</param>
        public static void Copy(this Byte[] obj, UInt16 resource, int index = 0)
        {
            obj.Copy(resource, ref index);
        }
        /// <summary>
        /// 拷贝方法
        /// </summary>
        /// <param name="obj">目标数组</param>
        /// <param name="resource">源数据</param>
        /// <param name="index">起始下标</param>
        public static void Copy(this Byte[] obj, Int32 resource, int index = 0)
        {
            obj.Copy(resource, ref index);
        }
        /// <summary>
        /// 拷贝方法
        /// </summary>
        /// <param name="obj">目标数组</param>
        /// <param name="resource">源数据</param>
        /// <param name="index">起始下标</param>
        public static void Copy(this Byte[] obj, UInt32 resource, int index = 0)
        {
            obj.Copy(resource, ref index);
        }
        /// <summary>
        /// 拷贝方法
        /// </summary>
        /// <param name="obj">目标数组</param>
        /// <param name="resource">源数据</param>
        /// <param name="index">起始下标</param>
        public static void Copy(this Byte[] obj, Int64 resource, int index = 0)
        {
            obj.Copy(resource, ref index);
        }
        /// <summary>
        /// 拷贝方法
        /// </summary>
        /// <param name="obj">目标数组</param>
        /// <param name="resource">源数据</param>
        /// <param name="index">起始下标</param>
        public static void Copy(this Byte[] obj, UInt64 resource, int index = 0)
        {
            obj.Copy(resource, ref index);
        }
        /// <summary>
        /// 拷贝数组
        /// </summary>
        /// <param name="obj">目标数组</param>
        /// <param name="resource">源</param>
        /// <param name="index">目标数组存放下标</param>
        /// <param name="length">拷贝数据长度 默认-1:表示拷贝源数据长度</param>
        public static void CopyBytes(this Byte[] obj, Byte[] resource, int index = 0, int length = -1)
        {
            length = length < 0 ? resource.Length : length;
            Array.Copy(resource, 0, obj, index, length);
        }
        /// <summary>
        /// 拷贝字符串
        /// </summary>
        /// <param name="obj">目标数组</param>
        /// <param name="resource">字符串</param>
        /// <param name="length">buff长度</param>
        /// <param name="encoding">编码方式</param>
        /// <param name="index">起始坐标</param>
        public static void CopyString(this Byte[] obj, string resource, int length, Encoding encoding, int index)
        {
            obj.CopyString(resource, length, encoding, ref index);
        }
        /// <summary>
        /// 拷贝字符串 末尾不截断
        /// </summary>
        /// <param name="obj">目标数组</param>
        /// <param name="resource">字符串</param>
        /// <param name="encoding">编码方式</param>
        /// <param name="index">带引用的起始坐标</param>
        public static void CopyString2(this Byte[] obj, string resource, Encoding encoding, int index)
        {
            obj.CopyString2(resource, encoding, ref index);
        }
        #endregion


        #region 数据获取
        /// <summary>
        /// 从数组中取出部分数组
        /// </summary>
        /// <param name="obj">源数组</param>
        /// <param name="length">取值长度</param>
        /// <param name="index">起始下标</param>
        /// <returns>结果数组</returns>
        public static Byte[] GetBytes(this Byte[] obj, int length, ref int index)
        {
            Byte[] result = new byte[length];
            Array.Copy(obj, index, result, 0, length);
            index += length;
            return result;
        }
        /// <summary>
        /// 获取Int8
        /// </summary>
        /// <param name="obj">源数组</param>
        /// <param name="index">起始下标</param>
        /// <returns>结果数组</returns>
        public static Byte GetInt8(this Byte[] obj, ref int index)
        {
            return obj[index++];
        }
        /// <summary>
        /// 获取Int16
        /// </summary>
        /// <param name="obj">源数组</param>
        /// <param name="index">起始下标</param>
        /// <returns>结果数组</returns>
        public static Int16 GetInt16(this Byte[] obj, ref int index)
        {
            return BitConverter.ToInt16(obj.GetBytes(2, ref index), 0).ToNetWorkNumber(NetWorkEnum.NetWorkToHost);
        }
        /// <summary>
        /// 获取UInt16
        /// </summary>
        /// <param name="obj">源数组</param>
        /// <param name="index">起始下标</param>
        /// <returns>结果数组</returns>
        public static UInt16 GetUInt16(this Byte[] obj, ref int index)
        {
            return BitConverter.ToUInt16(obj.GetBytes(2, ref index), 0).ToNetWorkNumber(NetWorkEnum.NetWorkToHost);
        }
        /// <summary>
        /// 获取Int32
        /// </summary>
        /// <param name="obj">源数组</param>
        /// <param name="index">起始下标</param>
        /// <returns>结果数组</returns>
        public static Int32 GetInt32(this Byte[] obj, ref int index)
        {
            return BitConverter.ToInt32(obj.GetBytes(4, ref index), 0).ToNetWorkNumber(NetWorkEnum.NetWorkToHost);
        }
        /// <summary>
        /// 获取UInt16
        /// </summary>
        /// <param name="obj">源数组</param>
        /// <param name="index">起始下标</param>
        /// <returns>结果数组</returns>
        public static UInt32 GetUInt32(this Byte[] obj, ref int index)
        {
            return BitConverter.ToUInt32(obj.GetBytes(4, ref index), 0).ToNetWorkNumber(NetWorkEnum.NetWorkToHost);
        }
        /// <summary>
        /// 获取Int64
        /// </summary>
        /// <param name="obj">源数组</param>
        /// <param name="index">起始下标</param>
        /// <returns>结果数组</returns>
        public static Int64 GetInt64(this Byte[] obj, ref int index)
        {
            return BitConverter.ToInt64(obj.GetBytes(8, ref index), 0).ToNetWorkNumber(NetWorkEnum.NetWorkToHost);
        }
        /// <summary>
        /// 获取UInt16
        /// </summary>
        /// <param name="obj">源数组</param>
        /// <param name="index">起始下标</param>
        /// <returns>结果数组</returns>
        public static UInt64 GetUInt64(this Byte[] obj, ref int index)
        {
            return BitConverter.ToUInt64(obj.GetBytes(8, ref index), 0).ToNetWorkNumber(NetWorkEnum.NetWorkToHost);
        }
        /// <summary>
        /// 获取byte[]中的字符串
        /// </summary>
        /// <param name="obj">byte[] 源数组</param>
        /// <param name="length">buff长度</param>
        /// <param name="encoding">字符集</param>
        /// <param name="index">起始位置</param>
        /// <returns></returns>
        public static string GetString(this Byte[] obj, int length, Encoding encoding, ref int index)
        {
            return encoding.GetString(obj.GetBytes(length, ref index));
        }

        /// <summary>
        /// 获取byte[]
        /// </summary>
        /// <param name="obj">源</param>
        /// <param name="length">获取长度</param>
        /// <param name="index">起始位置</param>
        /// <returns></returns>
        public static Byte[] GetBytes(this Byte[] obj, int length, int index = 0)
        {
            return obj.GetBytes(length, ref index);
        }
        /// <summary>
        /// 获取Int16
        /// </summary>
        /// <param name="obj">源数组</param>
        /// <param name="index">起始下标</param>
        /// <returns>结果数组</returns>
        public static byte GetInt8(this Byte[] obj, int index = 0)
        {
            return obj.GetInt8(ref index);
        }
        /// <summary>
        /// 获取Int16
        /// </summary>
        /// <param name="obj">源数组</param>
        /// <param name="index">起始下标</param>
        /// <returns>结果数组</returns>
        public static Int16 GetInt16(this Byte[] obj, int index = 0)
        {
            return obj.GetInt16(ref index);
        }
        /// <summary>
        /// 获取UInt16
        /// </summary>
        /// <param name="obj">源数组</param>
        /// <param name="index">起始下标</param>
        /// <returns>结果数组</returns>
        public static UInt16 GetUInt16(this Byte[] obj, int index = 0)
        {
            return obj.GetUInt16(ref index);
        }
        /// <summary>
        /// 获取Int32
        /// </summary>
        /// <param name="obj">源数组</param>
        /// <param name="index">起始下标</param>
        /// <returns>结果数组</returns>
        public static Int32 GetInt32(this Byte[] obj, int index = 0)
        {
            return obj.GetInt32(ref index);
        }
        /// <summary>
        /// 获取UInt32
        /// </summary>
        /// <param name="obj">源数组</param>
        /// <param name="index">起始下标</param>
        /// <returns>结果数组</returns>
        public static UInt32 GetUInt32(this Byte[] obj, int index = 0)
        {
            return obj.GetUInt32(ref index);
        }
        /// <summary>
        /// 获取Int64
        /// </summary>
        /// <param name="obj">源数组</param>
        /// <param name="index">起始下标</param>
        /// <returns>结果数组</returns>
        public static Int64 GetInt64(this Byte[] obj, int index = 0)
        {
            return obj.GetInt64(ref index);
        }
        /// <summary>
        /// 获取UInt64
        /// </summary>
        /// <param name="obj">源数组</param>
        /// <param name="index">起始下标</param>
        /// <returns>结果数组</returns>
        public static UInt64 GetUInt64(this Byte[] obj, int index = 0)
        {
            return obj.GetUInt64(ref index);
        }
        /// <summary>
        /// 获取byte[]中的字符串
        /// </summary>
        /// <param name="obj">byte[] 源数组</param>
        /// <param name="length">buff长度</param>
        /// <param name="encoding">字符集</param>
        /// <param name="index">起始位置</param>
        /// <returns></returns>
        public static string GetString(this Byte[] obj, int length, Encoding encoding, int index = 0)
        {
            return obj.GetString(length, encoding, ref index);
        }
        #endregion
    }
}
