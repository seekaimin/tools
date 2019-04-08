using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ExcelCompare
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
            Application.Run(new FormMain());
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            string msg = "出现异常";
            if (e.Exception != null)
            {
                msg = e.Exception.Message;
            }
            MessageBox.Show(msg);
        }
    }
}
