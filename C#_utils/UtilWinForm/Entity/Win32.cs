using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Util.WinForm.Entity
{
    public class Win32
    {
        public const int WM_NCHITTEST = 0x84;
        public const int HTCLIENT = 0x1;
        /// <summary>
        /// 常量
        /// </summary>
        public const int WM_SYSCOMMAND = 0x0112;
        /// <summary>
        /// 窗体移动
        /// </summary>
        public const int SC_MOVE = 0xF010;
        public const int HTCAPTION = 0x0002;
        /// <summary>
        /// 改变窗体大小
        /// </summary>
        public const int WMSZ_LEFT = 0xF001;
        public const int WMSZ_RIGHT = 0xF002;
        public const int WMSZ_TOP = 0xF003;
        public const int WMSZ_TOPLEFT = 0xF004;
        public const int WMSZ_TOPRIGHT = 0xF005;
        public const int WMSZ_BOTTOM = 0xF006;
        public const int WMSZ_BOTTOMLEFT = 0xF007;
        public const int WMSZ_BOTTOMRIGHT = 0xF008;
        /// <summary>
        /// 鼠标双击标题栏消息
        /// </summary>
        public const int WM_NCLBUTTONDBLCLK = 0xA3;

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern int CreateRoundRectRgn(int top, int buttom, int left, int right, int a, int b);
        [DllImport("user32.dll")]
        public static extern void SetWindowRgn(IntPtr hwnd, int a, bool flag);

    }
}
