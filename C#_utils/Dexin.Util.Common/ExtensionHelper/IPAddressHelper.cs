using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Dexin.Util.Common.ExtensionHelper
{
    /// <summary>
    /// IP地址帮助类
    /// </summary>
    public static class IPAddressHelper
    {
        /// <summary>
        /// 获取本地所有IP地址
        /// </summary>
        /// <param name="ipType">IP地址类型</param>
        /// <returns></returns>
        public static List<IPAddress> Localips(IPTypeEnum ipType = IPTypeEnum.IPV4)
        {
            List<IPAddress> ips = new List<IPAddress>();
            foreach (IPAddress ip in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (ipType == IPTypeEnum.IPV4 && ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    ips.Add(ip);
                }
                else if (ipType == IPTypeEnum.IPV6 && ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                {
                    ips.Add(ip);
                }
                else if (ipType == IPTypeEnum.ALL &&
                    (
                          ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork
                         || ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6
                    ))
                {
                    ips.Add(ip);
                }
            }
            return ips;
        }


        /// <summary>
        /// 获取本地所有IP地址
        /// </summary>
        /// <param name="isaddlocalip">是否加载 127.0.0.1</param>
        /// <param name="ipType">IP地址类型</param>
        /// <returns></returns>
        public static List<KeyValuePair<string, string>> LocalKeyValuePairIps(bool isaddlocalip = false, IPTypeEnum ipType = IPTypeEnum.IPV4)
        {
            List<KeyValuePair<string, string>> result = new List<KeyValuePair<string, string>>();
            if (isaddlocalip)
            {
                result.Add(new KeyValuePair<string, string>("127.0.0.1", "127.0.0.1"));
            }
            foreach (IPAddress address in Localips(ipType))
            {
                result.Add(new KeyValuePair<string, string>(address.ToString(), address.ToString()));
            }
            return result;
        }


        /// <summary>
        /// 检查字符串是否为ip地址
        /// </summary>
        /// <param name="IP">传入的字符串</param>
        /// <returns></returns>
        public static bool IsIPAddress(this string IP)
        {
            if (string.IsNullOrWhiteSpace(IP))
                return false;
            string[] items = IP.Split('.');
            if (items.Length != 4)
                return false;
            for (int i = 0; i < items.Length; i++)
            {
                int num;
                if (!int.TryParse(items[i], out num))
                    return false;
                if ((num < 0) || (num > 255))
                    return false;
                if ((num == 0) && (i == 0))
                    return false;
            }
            return true;
        }
        /// <summary>
        /// 获取IP地址  的Address
        /// </summary>
        /// <param name="IP">需要获取的字符串</param>
        /// <returns></returns>
        public static long GetAddress(this string IP)
        {
            if (IP.IsIPAddress() == false)
                return 0;
            //IPAddress addr = IP.ToIPAdress();
            //byte[] data = addr.GetAddressBytes();
            return GetAddress(IP.ToIPAdress());
        }
        /// <summary>
        /// 获取IP地址  的Address
        /// </summary>
        /// <param name="IP">需要获取地址</param>
        /// <returns></returns>
        public static long GetAddress(this IPAddress IP)
        {
            //byte[] data = addr.GetAddressBytes();
            int addr = BitConverter.ToInt32(IP.GetAddressBytes(), 0);
            uint res = (uint)IPAddress.HostToNetworkOrder(addr);
            return res;
        }
        /// <summary>
        /// 检查字符串是否为ip地址
        /// </summary>
        /// <param name="IP">传入的字符串</param>
        /// <returns></returns>
        public static bool IsMulticaseAddress(this string IP)
        {
            if (IP.IsIPAddress() == false)
                return false;
            long address = IP.GetAddress();
            string min = "224.0.0.255";
            string max = "239.255.255.255";
            long min_val = min.GetAddress();
            long max_val = max.GetAddress();
            return address > min_val && address <= max_val;
        }

        /// <summary>
        /// 检查字符串是否为掩码
        /// </summary>
        /// <param name="msk">传入的子网掩码</param>
        /// <returns></returns>
        public static bool IsMask(this string msk)
        {
            if (string.IsNullOrWhiteSpace(msk))
                return false;
            string[] items = msk.Split('.');
            if (items.Length != 4)
                return false;

            bool vZero = false; // 出现0
            for (int j = 0; j < items.Length; j++)
            {
                int i;
                if (!int.TryParse(items[j], out i))
                    return false;
                if ((i < 0) || (i > 255))
                    return false;
                if (vZero)
                {
                    if (i != 0)
                        return false;
                }
                else
                {
                    for (int k = 7; k >= 0; k--)
                        if (((i >> k) & 1) == 0) // 出现0
                            vZero = true;
                        else if (vZero)
                            return false; // 不为0
                }
            }
            return true;
        }
        /// <summary>
        /// 字符串地址比较
        /// </summary>
        /// <param name="ip0">地址1</param>
        /// <param name="ip1">地址2</param>
        /// <returns></returns>
        public static bool Compare(string ip0, string ip1)
        {
            if (!ip0.IsIPAddress())
            {
                return false;
            }
            if (!ip1.IsIPAddress())
            {
                return false;
            }
            return IPAddress.Parse(ip0).Equals(IPAddress.Parse(ip1));
        }
        /// <summary>
        /// 地址比较
        /// </summary>
        /// <param name="ip0">地址1</param>
        /// <param name="ip1">地址2</param>
        /// <returns></returns>
        public static bool Compare(this IPAddress ip0, IPAddress ip1)
        {
            return ip0.Equals(ip1);
        }
        /// <summary>
        /// 地址比较
        /// </summary>
        /// <param name="ip0">地址1</param>
        /// <param name="ip1">地址2</param>
        /// <returns></returns>
        public static bool Compare(this IPAddress ip0, string ip1)
        {
            if (!ip1.IsIPAddress())
            {
                return false;
            }
            return ip0.Equals(IPAddress.Parse(ip1));
        }
        /// <summary>
        /// 将IP地址转化为4字节数组
        /// </summary>
        /// <param name="IP">ip地址</param>
        /// <returns>4字节数组</returns>
        public static byte[] IPAddressTo4(string IP)
        {
            byte[] result = new byte[4];
            string[] items = IP.Split('.');
            for (int i = 0; i < 4; i++)
            {
                result[i] = items[i].ToInt8();
            }
            return result;
        }
    }
    /// <summary>
    /// IP地址类型
    /// </summary>
    public enum IPTypeEnum
    {
        /// <summary>
        /// 所有类型
        /// </summary>
        ALL = 0,
        /// <summary>
        /// IPV4
        /// </summary>
        IPV4,
        /// <summary>
        /// IPV6
        /// </summary>
        IPV6,
    }
}
