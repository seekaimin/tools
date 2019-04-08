using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Util.Common.ExtensionHelper
{
    /// <summary>
    /// 数字类型扩展方法
    /// 包含网络传输
    /// </summary>
    public static class NetWorkHelper
    {
        /*
         * 数据在网络传输中用到的方法
         * **/
        #region 网络字节序转换
        /// <summary>
        /// 字节序转化
        /// </summary>
        /// <param name="obj">转化对象</param>
        /// <param name="netWork">转化类型 默认不转化</param>
        /// <returns>转化结果</returns>
        public static Int16 ToNetWorkNumber(this Int16 obj, NetWorkEnum netWork = NetWorkEnum.Normal)
        {
            if (netWork == NetWorkEnum.HostToNetWork)
            {
                return IPAddress.HostToNetworkOrder(obj);
            }
            else if (netWork == NetWorkEnum.NetWorkToHost)
            {
                return IPAddress.NetworkToHostOrder(obj);
            }
            return obj;
        }
        /// <summary>
        /// 字节序转化
        /// </summary>
        /// <param name="obj">转化对象</param>
        /// <param name="netWork">转化类型 默认不转化</param>
        /// <returns>转化结果</returns>
        public static Int32 ToNetWorkNumber(this Int32 obj, NetWorkEnum netWork = NetWorkEnum.Normal)
        {
            if (netWork == NetWorkEnum.HostToNetWork)
            {
                return IPAddress.HostToNetworkOrder(obj);
            }
            else if (netWork == NetWorkEnum.NetWorkToHost)
            {
                return IPAddress.NetworkToHostOrder(obj);
            }
            return obj;
        }
        /// <summary>
        /// 字节序转化
        /// </summary>
        /// <param name="obj">转化对象</param>
        /// <param name="netWork">转化类型 默认不转化</param>
        /// <returns>转化结果</returns>
        public static Int64 ToNetWorkNumber(this Int64 obj, NetWorkEnum netWork = NetWorkEnum.Normal)
        {
            if (netWork == NetWorkEnum.HostToNetWork)
            {
                return IPAddress.HostToNetworkOrder(obj);
            }
            else if (netWork == NetWorkEnum.NetWorkToHost)
            {
                return IPAddress.NetworkToHostOrder(obj);
            }
            return obj;
        }

        /// <summary>
        /// 字节序转化
        /// </summary>
        /// <param name="obj">转化对象</param>
        /// <param name="netWork">转化类型 默认不转化</param>
        /// <returns>转化结果</returns>
        public static UInt16 ToNetWorkNumber(this UInt16 obj, NetWorkEnum netWork = NetWorkEnum.Normal)
        {
            if (netWork == NetWorkEnum.HostToNetWork)
            {
                return (UInt16)IPAddress.HostToNetworkOrder((Int16)obj);
            }
            else if (netWork == NetWorkEnum.NetWorkToHost)
            {
                return (UInt16)IPAddress.NetworkToHostOrder((Int16)obj);
            }
            return obj;
        }
        /// <summary>
        /// 字节序转化
        /// </summary>
        /// <param name="obj">转化对象</param>
        /// <param name="netWork">转化类型 默认不转化</param>
        /// <returns>转化结果</returns>
        public static UInt32 ToNetWorkNumber(this UInt32 obj, NetWorkEnum netWork = NetWorkEnum.Normal)
        {
            if (netWork == NetWorkEnum.HostToNetWork)
            {
                return (UInt32)IPAddress.HostToNetworkOrder((Int32)obj);
            }
            else if (netWork == NetWorkEnum.NetWorkToHost)
            {
                return (UInt32)IPAddress.NetworkToHostOrder((Int32)obj);
            }
            return obj;
        }
        /// <summary>
        /// 字节序转化
        /// </summary>
        /// <param name="obj">转化对象</param>
        /// <param name="netWork">转化类型 默认不转化</param>
        /// <returns>转化结果</returns>
        public static UInt64 ToNetWorkNumber(this UInt64 obj, NetWorkEnum netWork = NetWorkEnum.Normal)
        {
            if (netWork == NetWorkEnum.HostToNetWork)
            {
                return (UInt64)IPAddress.HostToNetworkOrder((Int64)obj);
            }
            else if (netWork == NetWorkEnum.NetWorkToHost)
            {
                return (UInt64)IPAddress.NetworkToHostOrder((Int64)obj);
            }
            return obj;
        }
        #endregion
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
        /// 拷贝DateTime   size = 7
        /// </summary>
        /// <param name="obj">目标数组</param>
        /// <param name="resource">源</param>
        /// <param name="index">开始下表</param>
        public static void CopyDateTime(this Byte[] obj, DateTime resource, ref int index)
        {
            obj.CopyBytes(resource.DateTimeToBytes(), ref index);
        }
        /// <summary>
        /// 时间转化为数组
        /// </summary>
        /// <param name="dt">时间</param>
        /// <returns></returns>
        public static byte[] DateTimeToBytes(this DateTime dt)
        {
            int tempIndex = 0;
            Byte[] temp = new byte[7];
            temp.Copy((UInt16)dt.Year, ref tempIndex);//2
            temp.Copy((Byte)dt.Month, ref tempIndex);//1
            temp.Copy((Byte)dt.Day, ref tempIndex);//1
            temp.Copy((Byte)dt.Hour, ref tempIndex);//1
            temp.Copy((Byte)dt.Minute, ref tempIndex);//1
            temp.Copy((Byte)dt.Second, ref tempIndex);//1
            return temp;
        }
        /// <summary>
        /// 获取HEX字符串
        /// </summary>
        /// <param name="obj">原数组</param>
        /// <param name="length">获取长度</param>
        /// <param name="index">起始下标</param>
        /// <returns></returns>
        public static string GetASCIIString(this byte[] obj, int length, ref int index)
        {
            byte[] temp = obj.GetBytes(length, ref index);
            return Encoding.ASCII.GetString(temp);
        }


        /// <summary>
        /// 字符串传输(非0开始)    编码方式:ASCII 补齐方式:左补齐
        /// </summary>
        /// <param name="obj">目的数组</param>
        /// <param name="resource">源字符串</param>
        /// <param name="length">需要长度</param>
        /// <param name="index">起始位置</param>
        /// <param name="pad">补齐字符</param>
        public static void CopyASCIIString(this byte[] obj, string resource, int length, ref int index, char pad = '0')
        {
            string temp = resource.PadLeft(length, pad);
            byte[] buffer = Encoding.ASCII.GetBytes(temp);
            obj.CopyBytes(buffer, ref index);
        }
        /// <summary>
        /// 十六进制字符串拷贝
        /// </summary>
        /// <param name="obj">目的数组</param>
        /// <param name="resource">源十六进制字符串</param>
        /// <param name="index">其实下标</param>
        public static void CopyHexString(this byte[] obj, string resource, ref int index)
        {
            byte[] buffer = resource.StringToBytes();
            obj.CopyBytes(buffer, ref index);
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
        /// 拷贝DateTime
        /// </summary>
        /// <param name="obj">目标数组</param>
        /// <param name="resource">源</param>
        /// <param name="index">开始下表</param>
        public static void CopyDateTime(this Byte[] obj, DateTime resource, int index = 0)
        {
            obj.CopyDateTime(resource, ref index);
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
        /// 获取时间
        /// </summary>
        /// <param name="obj">数组</param>
        /// <param name="index">下标</param>
        /// <returns>时间</returns>
        public static DateTime GetDateTime(this Byte[] obj, ref int index)
        {
            int tempindex = 0;
            byte[] buff = obj.GetBytes(7, ref index);
            int year = buff.GetUInt16(ref tempindex);
            int month = buff.GetInt8(ref tempindex);
            int day = buff.GetInt8(ref tempindex);
            int hour = buff.GetInt8(ref tempindex);
            int minute = buff.GetInt8(ref tempindex);
            int second = buff.GetInt8(ref tempindex);
            return new DateTime(year, month, day, hour, minute, second);
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
        /// <summary>
        /// 获取时间
        /// </summary>
        /// <param name="obj">数组</param>
        /// <param name="index">下标</param>
        /// <returns>时间</returns>
        public static DateTime GetDateTime(this Byte[] obj, int index = 0)
        {
            return obj.GetDateTime(ref index);
        }
        #endregion


        /// <summary>
        /// 将字符串转换为buff
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="format">格式化类型  默认值16</param>
        /// <returns></returns>
        public static Byte[] StringToBytes(this string str, int format = 16)
        {
            IList<byte> result = new List<byte>();
            for (int i = 0; i < str.Length;)
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
        public static string BytesToString(this IEnumerable<Byte> buff, string splitChar = "")
        {
            return BitConverter.ToString(buff.ToArray()).Replace("-", splitChar);
        }
        /// <summary>
        /// 将buff按顺序显示为字符串
        /// </summary>
        /// <param name="buff">buff</param>
        /// <param name="splitChar">分隔符</param>
        /// <param name="format">X2   X4    X6</param>
        /// <returns></returns>
        public static string BytesToString(this IEnumerable<Byte> buff, string splitChar, string format)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte i in buff)
            {
                string temp = i.ToString(format);
                sb.Append(temp);
                sb.Append(splitChar);
            }
            return sb.ToStr();
        }

        /// <summary>
        /// 数字转化为网络字节序 buff
        /// </summary>
        /// <param name="val">需要转化的数字</param>
        /// <returns>网络字节序 buff</returns>
        public static byte[] ToNetWorkBuff(this Int16 val)
        {
            return BitConverter.GetBytes(val.ToNetWorkNumber(NetWorkEnum.HostToNetWork));
        }
        /// <summary>
        /// 数字转化为网络字节序 buff
        /// </summary>
        /// <param name="val">需要转化的数字</param>
        /// <returns>网络字节序 buff</returns>
        public static byte[] ToNetWorkBuff(this Int32 val)
        {
            return BitConverter.GetBytes(val.ToNetWorkNumber(NetWorkEnum.HostToNetWork));
        }
        /// <summary>
        /// 数字转化为网络字节序 buff
        /// </summary>
        /// <param name="val">需要转化的数字</param>
        /// <returns>网络字节序 buff</returns>
        public static byte[] ToNetWorkBuff(this Int64 val)
        {
            return BitConverter.GetBytes(val.ToNetWorkNumber(NetWorkEnum.HostToNetWork));
        }

        /// <summary>
        /// 数字转化为网络字节序 buff
        /// </summary>
        /// <param name="val">需要转化的数字</param>
        /// <returns>网络字节序 buff</returns>
        public static byte[] ToNetWorkBuff(this UInt16 val)
        {
            return BitConverter.GetBytes(val.ToNetWorkNumber(NetWorkEnum.HostToNetWork));
        }
        /// <summary>
        /// 数字转化为网络字节序 buff
        /// </summary>
        /// <param name="val">需要转化的数字</param>
        /// <returns>网络字节序 buff</returns>
        public static byte[] ToNetWorkBuff(this UInt32 val)
        {
            return BitConverter.GetBytes(val.ToNetWorkNumber(NetWorkEnum.HostToNetWork));
        }
        /// <summary>
        /// 数字转化为网络字节序 buff
        /// </summary>
        /// <param name="val">需要转化的数字</param>
        /// <returns>网络字节序 buff</returns>
        public static byte[] ToNetWorkBuff(this UInt64 val)
        {
            return BitConverter.GetBytes(val.ToNetWorkNumber(NetWorkEnum.HostToNetWork));
        }
    }
    /// <summary>
    /// 网络字节序转化枚举
    /// </summary>
    public enum NetWorkEnum
    {
        /// <summary>
        /// 自然不用转化
        /// </summary>
        Normal = 0,
        /// <summary>
        /// 主机字节序->网络字节序
        /// </summary>
        HostToNetWork = 1,
        /// <summary>
        /// 网络字节序->主机字节序
        /// </summary>
        NetWorkToHost = 2,
    }
}
