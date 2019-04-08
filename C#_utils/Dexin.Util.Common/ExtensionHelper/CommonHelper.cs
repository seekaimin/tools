using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Dexin.Util.Common.ExtensionHelper
{
    /// <summary>
    /// 
    /// </summary>
    public static class CommonHelper
    {
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
