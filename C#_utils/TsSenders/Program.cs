using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace TsSenders
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
            Application.ThreadException += (s, e) =>
            {
                if (!string.IsNullOrEmpty(e.Exception.Message)) MessageBox.Show(e.Exception.Message);
            };
            Application.Run(new FormSender());
            //Application.Run(new Form2());
        }
    }
}
