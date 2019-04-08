using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Dexin.Util.Common.WinAPI
{
    /// <summary>
    /// 本地系统时间操作类
    /// </summary>
    public class LocalSystemDateTime
    {
        /// <summary>  
        /// 获取系统时间  
        /// </summary>  
        /// <param name="st"></param>  
        [DllImport("Kernel32.dll")]
        private static extern void GetLocalTime(SystemTime st);


        /// <summary>  
        /// 设置系统时间  
        /// </summary>  
        /// <param name="st"></param>  
        [DllImport("Kernel32.dll")]
        private static extern void SetLocalTime(SystemTime st);

        /// <summary>
        /// 设置系统时间  
        /// </summary>
        /// <param name="time"></param>
        public static void SetLocalTime(DateTime time)
        {
            SystemTime st = ConvertDateTimeToSystemTime(time);
            SetLocalTime(st);
        }


        /// <summary>
        /// 设置系统时间对象
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        protected class SystemTime
        {
            /// <summary>
            /// 
            /// </summary>
            public ushort wYear;
            /// <summary>
            /// 
            /// </summary>
            public ushort wMonth;
            /// <summary>
            /// 
            /// </summary>
            public ushort wDayOfWeek;
            /// <summary>
            /// 
            /// </summary>
            public ushort wDay;
            /// <summary>
            /// 
            /// </summary>
            public ushort Whour;
            /// <summary>
            /// 
            /// </summary>
            public ushort wMinute;
            /// <summary>
            /// 
            /// </summary>
            public ushort wSecond;
            /// <summary>
            /// 
            /// </summary>
            public ushort wMilliseconds;

        }

        /// <summary>
        /// 类型转换
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        protected static SystemTime ConvertDateTimeToSystemTime(DateTime time)
        {
            SystemTime st = new SystemTime();
            st.wYear = (ushort)time.Year;
            st.wMonth = (ushort)time.Month;
            st.wDay = (ushort)time.Day;
            st.Whour = (ushort)time.Hour;
            st.wMinute = (ushort)time.Minute;
            st.wSecond = (ushort)time.Second;
            return st;
        }

        /// <summary>
        /// 类型转换
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        protected static DateTime ConverSystemTimeToDateTime(SystemTime time)
        {
            return new DateTime(time.wYear, time.wMonth, time.wDay, time.Whour, time.wMinute, time.wSecond);
        }
    }
}
