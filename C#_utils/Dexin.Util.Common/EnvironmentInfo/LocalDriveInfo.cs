using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Dexin.Util.Common.EnvironmentInfo
{
    /// <summary>
    /// 提供磁盘管理的相关方法
    /// </summary>
    public static class LocalDriveInfo
    {
        /// <summary>
        /// 获取本机空闲最多的磁盘
        /// </summary>
        /// <returns></returns>
        public static DriveInfo GetMaxFreeDriver()
        {
            DriveInfo maxFree = DriveInfo.GetDrives().First(x => x.IsReady);
            foreach (var driver in DriveInfo.GetDrives().Where(x => x.IsReady))
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
            foreach (var driver in DriveInfo.GetDrives().Where(x => x.IsReady))
            {
                if (Directory.Exists(driver.Name + directorName))
                {
                    return driver;
                }
            }

            return null;
        }
    }
}