using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Util.Common.ExtensionHelper;

namespace Util.Common
{
    /// <summary>
    /// DVB标准中用到的方法
    /// </summary>
    public static class DVBHelper
    {
        /// <summary>
        /// 构建BCD字符串
        /// </summary>
        /// <param name="leftLength">小数点左边的长度</param>
        /// <param name="rightLength">小数点右边的长度</param>
        /// <param name="str">需要转换的字符串</param>
        /// <returns>BCD字符串</returns>
        public static string ToBCDString(this string str, int leftLength, int rightLength)
        {
            string result = "";
            string left = "";
            string right = "";
            if (str.IndexOf('.') < 0)
            {
                left = str.PadLeft(leftLength, '0');
                result = left.Substring(left.Length - leftLength, leftLength);
                result = result.PadRight(leftLength + rightLength, '0');
            }
            else
            {
                left = str.Substring(0, str.IndexOf('.'));

                left = left.PadLeft(leftLength, '0');
                left = left.Substring(left.Length - leftLength, leftLength);

                right = str.Substring(str.IndexOf('.') + 1);

                right = right.PadRight(rightLength, '0');
                right = right.Substring(0, rightLength);
                result = left + right;
            }
            return result;
        }
        /// <summary>
        /// 将时间转换成MJD  5B
        /// </summary>
        /// <param name="datetime">时间</param>
        /// <returns>5个字节 byte数组</returns>
        public static Byte[] ToMJDBuff(this DateTime datetime)
        {
            byte[] result = new byte[5];
            int l = 0;

            int year, month, day, hour, minute, second, mjd = 0;


            year = datetime.Year - 1900;
            month = datetime.Month;
            day = datetime.Day;
            hour = datetime.Hour;
            minute = datetime.Minute;
            second = datetime.Second;
            if ((month == 1) || (month == 2))
                l = 1;
            else
                l = 0;
            mjd = 14956 + day + (int)((year - l) * 365.25) + (int)((month + 1 + l * 12) * 30.6001);

            result[0] = (byte)((mjd >> 8) & 0xff); // HIBYTE(mjd);
            result[1] = (byte)(mjd & 0xff);

            result[2] = (byte)(hour / 10 * 16 + hour % 10);
            result[3] = (byte)(minute / 10 * 16 + minute % 10);
            result[4] = (byte)(second / 10 * 16 + second % 10);
            return result;
        }
        /// <summary>
        /// MJD buff转时间
        /// </summary>
        /// <param name="buff">buff</param>
        /// <returns></returns>
        public static DateTime MJDToDateTime(this byte[] buff)
        {
            int k = 0;
            int year = 0, month = 0, day = 0, hour = 0, minute = 0, second = 0;
            byte[] temp_data_year = new byte[2];
            temp_data_year[0] = buff[0];
            temp_data_year[1] = buff[1];
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(temp_data_year);
            }
            UInt16 mjd_year = BitConverter.ToUInt16(temp_data_year, 0);
            year = (UInt16)((mjd_year - 15078.2) / 365.25);
            month = (int)((mjd_year - 14956.1 - (int)(year * 365.25)) / 30.6001);
            day = (int)(mjd_year - 14956 - (int)(year * 365.25) - (int)(month * 30.6001));
            byte[] temp = new byte[3];
            temp[0] = buff[2];
            temp[1] = buff[3];
            temp[2] = buff[4];

            string hexValues = BitConverter.ToString(temp);
            string[] hexValuesSplit = hexValues.Split('-');
            int i = 0;
            foreach (String hex in hexValuesSplit)
            {
                // Convert the number expressed in base-16 to an integer.
                int value = int.Parse(hex);
                temp[i] = (byte)value;
                i++;
            }
            hour = temp[0];
            minute = temp[1];
            second = temp[2];
            if (month == 14 || month == 15)
            {
                k = 1;
            }
            else
            {
                k = 0;
            }
            year += 1900 + k;
            month = month - 1 - k * 12;
            DateTime result = new DateTime(year, month, day, hour, minute, second);
            return result;

        }

        /// <summary>
        /// 拷贝MJD时间到数组
        /// </summary>
        /// <param name="data">数组</param>
        /// <param name="datetime">时间</param>
        /// <param name="index">起始下标</param>
        public static void CopyMJDDateTime(this byte[] data, DateTime datetime, ref int index)
        {
            byte[] temp = datetime.ToMJDBuff();
            data.CopyBytes(temp, ref index);
        }
        /// <summary>
        /// 获取MJD时间
        /// </summary>
        /// <param name="data">数组</param>
        /// <param name="index">起始位置</param>
        /// <returns>时间</returns>
        public static DateTime GetMJDDateTime(this byte[] data, ref int index)
        {
            byte[] temp = data.GetBytes(5, ref index);
            DateTime result = temp.MJDToDateTime();
            return result;
        }
    }
}
