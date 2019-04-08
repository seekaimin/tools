using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;

namespace Util.Common
{
    /// <summary>
    /// windows 操作cmd
    /// </summary>
    public class WindowsCMD
    {
        /// <summary>
        /// 关机计算机
        /// </summary>
        const string cmdShutdown = "shutdown -s -t {0}";
        /// <summary>
        /// 重启计算机
        /// </summary>
        const string cmdReStart = "shutdown -r -t {0}";
        /// <summary>
        /// 取消关机
        /// </summary>
        const string cmdCancelShutdown = "shutdown -a";
        /// <summary>
        /// 运行cmd命令
        /// </summary>
        /// <param name="cmd">控制命令</param>
        /// <param name="output">输出</param>
        public static void Run(string cmd, Action<string> output = null)
        {
            using (Process p = new Process())
            {
                p.StartInfo.FileName = "cmd.exe";
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.RedirectStandardInput = true;
                p.StartInfo.RedirectStandardOutput = true;
                if (output != null)
                {
                    p.OutputDataReceived += new DataReceivedEventHandler((s, d) =>
                    {
                        output(d.Data);
                    });
                }

                p.Start();
                p.StandardInput.WriteLine(cmd);
                p.StandardInput.WriteLine("exit");
                p.BeginOutputReadLine();
            }
        }
        /// <summary>
        /// 关闭计算机
        /// </summary>
        /// <param name="second">等待时间(秒)  默认0</param>
        public static void Shutdown(int second = 0)
        {
            string cmd = string.Format(cmdShutdown, second);
            Run(cmd);
        }
        /// <summary>
        /// 重启计算机
        /// </summary>
        /// <param name="second">等待时间(s) 默认0</param>
        public static void ReStart(int second = 0)
        {
            string cmd = string.Format(cmdReStart, second);
            Run(cmd);
        }
        /// <summary>
        /// 取消关机
        /// </summary>
        public static void CancelShutdown()
        {
            string cmd = cmdCancelShutdown;
            Run(cmd);
        }
    }


    public static class PCVolume
    {
        private const int APPCOMMAND_VOLUME_MUTE = 0x80000;
        private const int APPCOMMAND_VOLUME_UP = 0x0a0000;
        private const int APPCOMMAND_VOLUME_DOWN = 0x090000;
        private const int WM_APPCOMMAND = 0x319;

        [DllImport("user32.dll")]
        static extern IntPtr SendMessageW(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);
        public static void VolumeUP(IntPtr ptr)
        {
            SendMessageW(ptr, WM_APPCOMMAND, ptr, (IntPtr)APPCOMMAND_VOLUME_UP);
        }

        public static void VolumeDown(IntPtr ptr)
        {
            SendMessageW(ptr, WM_APPCOMMAND, ptr, (IntPtr)APPCOMMAND_VOLUME_DOWN);
        }

        public static void VolumeMute(IntPtr ptr)
        {
            SendMessageW(ptr, WM_APPCOMMAND, ptr, (IntPtr)APPCOMMAND_VOLUME_MUTE);
        }
    }
}
