using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dexin.Util.Common.EnvironmentInfo
{
    /// <summary>
    /// 操作系统类型
    /// </summary>
    public enum OSystemTypes
    {
        /// <summary>
        /// 未知
        /// </summary>
        Unkonwn,
        /// <summary>
        /// 
        /// </summary>
        Win2000,
        /// <summary>
        /// 
        /// </summary>
        WinXP,
        /// <summary>
        /// 
        /// </summary>
        Win2003,
        /// <summary>
        /// 
        /// </summary>
        WinVista,
        /// <summary>
        /// 
        /// </summary>
        Win7Or2008,
        /// <summary>
        /// 
        /// </summary>
        Win8Or10,
    }

    /// <summary>
    /// 操作系统信息类
    /// </summary>
    public class OSystemInfo
    {
        /// <summary>
        /// 获取操作系统类型
        /// </summary>
        /// <returns></returns>
        public OSystemTypes GetCurrentOSystemType()
        {
            Version ver = System.Environment.OSVersion.Version;
            if (ver.Major == 5)
            {
                if (ver.Minor == 0)
                    return OSystemTypes.Win2000;
                else if (ver.Minor == 1)
                    return OSystemTypes.WinXP;
                else if (ver.Minor == 2)
                    return OSystemTypes.Win2003;
            }
            else if (ver.Major == 6)
            {
                if (ver.Minor == 0)
                    return OSystemTypes.WinVista;
                else if (ver.Minor == 1)
                    return OSystemTypes.Win7Or2008;
                else if (ver.Minor == 2)
                    return OSystemTypes.Win8Or10;
            }

            return OSystemTypes.Unkonwn;
        }
    }
}
