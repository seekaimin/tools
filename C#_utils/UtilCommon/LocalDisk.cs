using System.IO;
using System.Linq;
using System.Management;
using Util.Common.ExtensionHelper;
using System.Collections.Generic;

namespace Util.Common
{
    /// <summary>
    /// 提供磁盘管理的相关方法
    /// </summary>
    public static class DiskManager
    {
        /// <summary>
        /// 获取本机空闲最多的磁盘
        /// </summary>
        /// <returns></returns>
        public static DriveInfo GetMaxFreeDriver()
        {
            DriveInfo maxFree = DriveInfo.GetDrives().First();
            foreach (var driver in DriveInfo.GetDrives().Where(p => p.DriveType == DriveType.Fixed))
            {
                if (driver.AvailableFreeSpace > maxFree.AvailableFreeSpace)
                {
                    maxFree = driver;
                }
            }
            return maxFree;
        }

        /// <summary>
        /// 根据指定条件返回第一个符合条件的驱磁盘
        /// </summary>
        /// <param name="directorName">磁盘包含的文件夹名称</param>
        /// <returns></returns>
        public static DriveInfo GetDriver(string directorName)
        {
            foreach (var driver in DriveInfo.GetDrives())
            {
                if (Directory.Exists(driver.Name + directorName))
                {
                    return driver;
                }
            }
            return null;
        }

        /// <summary>
        /// 获取硬盘序号
        /// </summary>
        /// <returns></returns>
        public static List<string> GetDiskVolumeSerialNumber()
        {
            List<string> result = new List<string>();
            using (ManagementClass mc = new ManagementClass("win32_diskdrive"))
            {
                using (ManagementObjectCollection collection = mc.GetInstances())
                {
                    foreach (var a in collection)
                    {
                        result.Add(a.Properties["Model"].Value.ToStr());
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// 获取CPU序列号
        /// </summary>
        /// <returns></returns>
        public static List<string> GetCpuNumber()
        {
            List<string> result = new List<string>();
            using (ManagementClass mc = new ManagementClass("win32_processor"))
            using (ManagementObjectCollection collection = mc.GetInstances())
                foreach (var a in collection)
                {
                    result.Add(a.Properties["Processorid"].Value.ToStr());
                }
            return result;
        }
        /// <summary>
        /// 获取网卡硬件地址
        /// </summary>
        /// <returns></returns>
        public static List<string> GetNetworkNumber()
        {
            List<string> result = new List<string>();
            using (ManagementClass mc = new ManagementClass("win32_networkadapterconfiguration"))
            using (ManagementObjectCollection collection = mc.GetInstances())
            {
                foreach (var a in collection)
                {
                    bool IsIPEnabled = a.Properties["IPEnabled"].Value.ToBoolean();
                    if (IsIPEnabled)
                        result.Add(a.Properties["MacAddress"].Value.ToStr());
                }
            }
            return result;
        }

        /// <summary>
        /// 获取默认序列号
        /// </summary>
        /// <returns></returns>
        public static string GetDefaultNumber()
        {
            List<string> cpu = DiskManager.GetCpuNumber();
            //List<string> hdisk = DiskManager.GetDiskVolumeSerialNumber();
            //List<string> mac = DiskManager.GetNetworkNumber();
            string number = cpu.ArrayToString();// +hdisk.ArrayToString(); // +mac.ArrayToString();
            number = number.Replace(" ", "").Replace(":", "").ToUpper();
            return number;
        }


    }
}
