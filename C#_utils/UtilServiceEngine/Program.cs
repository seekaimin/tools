using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Util.ServiceEngine.ServiceForms;
using Util.ServiceEngine.Core;

namespace Util.ServiceEngine
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            try
            {
                ToolTipHelperForm notify = new ToolTipHelperForm();
                string[] args = Environment.GetCommandLineArgs();
                if (args.Length == 1)
                {
                    notify.BalloonMessage = new ToolTopMessage()
                    {
                        BalloonType = (ToolTipIcon)Enum.Parse(typeof(ToolTipIcon), args[0]),
                        Title = args[1].Replace('^', ' '),
                        Content = args[2].Replace('^', ' '),
                        Timeout = Convert.ToInt32(args[3])
                    };
                    Application.Run(notify);
                }
                else if (args.Length == 4)
                {
                    notify.BalloonMessage = new ToolTopMessage()
                    {
                        BalloonType = (ToolTipIcon)Enum.Parse(typeof(ToolTipIcon), args[0]),
                        Title = args[1].Replace('^', ' '),
                        Content = args[2].Replace('^', ' '),
                        Timeout = Convert.ToInt32(args[3])
                    };
                    Application.Run(notify);
                }
                else if (args.Length == 5)
                {
                    notify.BalloonMessage = new ToolTopMessage()
                    {
                        BalloonType = (ToolTipIcon)Enum.Parse(typeof(ToolTipIcon), args[1]),
                        Title = args[2].Replace('^', ' '),
                        Content = args[3].Replace('^', ' '),
                        Timeout = Convert.ToInt32(args[4])
                    };
                    Application.Run(notify);
                }
                else { Application.Exit(); }
            }
            catch { Application.Exit(); }
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Application.Exit();
        }
    }
}

