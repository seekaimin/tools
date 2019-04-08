using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Dexin.Util.Common.Colors
{
    /// <summary>
    /// 颜色帮助类
    /// </summary>
    public static class ColorExtension
    {
        /// <summary>
        /// 对指定进行渐变到白色
        /// </summary>
        /// <param name="color">要渐变的原始颜色</param>
        /// <param name="level">渐变程度，取值0~1</param>
        /// <returns>渐变后的颜色</returns>
        public static Color Gradual(this Color color, double level)
        {
            int r = color.R;
            r += (int)((255 - r) * level);
            int g = color.G;
            g += (int)((255 - g) * level);
            int b = color.B;
            b += (int)((255 - b) * level);

            return Color.FromArgb(r, g, b);
        }
    }
}
