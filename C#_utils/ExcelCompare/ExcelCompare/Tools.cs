using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Util.Common;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace ExcelCompare
{
    public class Tools
    {
        public static string OLD_PATHNAME = "oldpath";
        public static string OLD_ENCLOSURE_PATHNAME = "oldenclosurepath";
        public static string NEW_PATHNAME = "newpath";
        public static string NEW_ENCLOSURE_PATHNAME = "newenclosurepath";



        public static string basepath = "";
        static Logger loger = null;
        static Configuration config = null;

        static Tools()
        {
            basepath = AppDomain.CurrentDomain.BaseDirectory;
            string configpath = Assembly.GetAssembly(typeof(Tools)).Location;
            //string configpath = Assembly.GetAssembly(typeof(Tools)).Location;//Path.Combine(basepath, "EPGXmlTransfer.exe");
            try
            {
                config = ConfigurationManager.OpenExeConfiguration(configpath);
                if (config.AppSettings.Settings.Count == 0)
                {
                    Tools.SetConfig(OLD_PATHNAME, "");
                    Tools.SetConfig(NEW_PATHNAME, "");

                    Tools.SetConfig(OLD_ENCLOSURE_PATHNAME, "");
                    Tools.SetConfig(NEW_ENCLOSURE_PATHNAME, "");

                    Tools.Save();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            loger = new Logger()
            {
                CurrentLevel = LogLevels.Debug,
            };
        }
        public static void d(string message)
        {
            loger.Write(LogLevels.Debug, message);
        }
        public static void d(string format, params object[] args)
        {
            loger.Write(LogLevels.Debug, format, args);
        }
        public static void SelectFolderPath(TextBox txt, Action<string> action = null)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                fbd.SelectedPath = txt.Text;
                if (fbd.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                {
                    return;
                }
                txt.Text = fbd.SelectedPath;
                if (action != null)
                {
                    action(txt.Text);
                }
            }
        }
        public static string GetConfig(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return "";
            }
            return config.AppSettings.Settings[key].Value;
        }
        public static void SetConfig(string key, string value)
        {
            config.AppSettings.Settings[key].Value = value;
        }
        public static void Save()
        {
            config.Save();
        }



        public static void SelectFile(TextBox txt)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    txt.Text = ofd.FileName;
                }
            }
        }
    }
}
