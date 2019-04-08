using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace TsSenders
{
    /// <summary>
    /// 播放器接口
    /// </summary>
    public class ITsPlayer
    {
        #region =================Interface=================
        /// <summary>
        /// OpenDevice
        /// </summary>
        /// <param name="phDeviceHandle"></param>
        /// <param name="bDirect"></param>
        /// <returns></returns>
        [DllImport("StreamDevice.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool OpenDevice(ref Int64 phDeviceHandle, bool bDirect);
        /// <summary>
        /// CloseDevice
        /// </summary>
        /// <param name="phDeviceHandle"></param>
        /// <returns></returns>
        [DllImport("StreamDevice.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool CloseDevice(Int64 phDeviceHandle);
        /// <summary>
        /// GetState
        /// </summary>
        /// <returns></returns>
        [DllImport("StreamDevice.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool GetState();

        /// <summary>
        /// WriteData
        /// </summary>
        /// <param name="pBuffer"></param>
        /// <param name="bufLength"></param>
        /// <returns></returns>
        [DllImport("StreamDevice.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool WriteData(byte[] pBuffer, int bufLength);

        /// <summary>
        /// SetRate
        /// </summary>
        /// <param name="rate"></param>
        /// <returns></returns>
        [DllImport("StreamDevice.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool SetRate(double rate);
        #endregion
    }
    /// <summary>
    /// 码流播放器管理
    /// </summary>
    public class TsPlayer
    {
        /// <summary>
        ///  播放盒 最小设备号
        /// </summary>
        static Int64 MinDeviceHandle = -1;
        private static Int64 _DeviceHandle = -1;
        public static Int64 DeviceHandle
        {
            get { return _DeviceHandle; }
            private set { _DeviceHandle = value; }
        }
        public static bool IsOpen
        {
            get
            {
                return DeviceHandle != MinDeviceHandle;
            }
        }
        public static bool OpenDevice(bool isthrowexception = false)
        {
            long devicehandle = MinDeviceHandle;
            ITsPlayer.OpenDevice(ref devicehandle, true);
            DeviceHandle = devicehandle;
            if (IsOpen == false)
            {
                Console.WriteLine(string.Format("0x{0:X}", devicehandle));
                DeviceHandle = MinDeviceHandle;
                if (isthrowexception)
                {
                    throw new MessageException("open ts player error!{0}", devicehandle);
                }
            }
            return IsOpen;
        }
        public static bool CloseDevice()
        {
            bool result = false;
            if (IsOpen && ITsPlayer.CloseDevice(DeviceHandle))
            {
                result = true;
                DeviceHandle = 0;
            }
            return result;
        }
        public static bool GetState()
        {
            return ITsPlayer.GetState();
        }
        public static bool SetRate(double rate)
        {
            return ITsPlayer.SetRate(rate);
        }
        public static bool WriteData(byte[] buff)
        {
            lock (lockRePreparTSPlayer)
            {
                bool flag = false;
                if (IsOpen)
                {
                    flag = ITsPlayer.WriteData(buff, buff.Length);
                    if (flag == false)
                    {
                        CloseDevice();
                    }
                }
                return flag;
            }
        }
        private static Object lockRePreparTSPlayer = new Object();
    }
}
