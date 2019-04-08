using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Util.Common.ExtensionHelper;
using Util.Common;
using System.Xml.Linq;
using Util.WinForm;

namespace AppRegister
{
    public partial class FormRegister : Form
    {
        public FormRegister()
        {
            InitializeComponent();
            dtStart.Value = DateTime.Now.Date;
        }
        /// <summary>
        /// 运营商号
        /// </summary>
        string op_no = "";
        /// <summary>
        /// 软件名称
        /// </summary>
        string app_name = "";
        /// <summary>
        /// 序列号
        /// </summary>
        string sn = "";
        string PK = "";
        private void btnImport_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                FileName = "register_config.txt",
                Filter = "register_config.txt|*.txt"
            };
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //选择成功
                string info = "";
                using (StreamReader sr = new StreamReader(ofd.FileName, Encoding.UTF8))
                {
                    info = sr.ReadToEnd();
                }
                XElement root = XElement.Parse(info);
                XElement x_sn = root.Element(MyRegister.SN);
                XElement x_opno = root.Element(MyRegister.OP_NO);
                XElement x_appname = root.Element(MyRegister.APP_NAME);
                XElement x_pk = root.Element(MyRegister.PK);
                if (x_sn == null)
                    new Exception("config sn error");
                if (x_appname == null)
                    new Exception("config appname error");
                if (x_opno == null)
                    new Exception("config opno error");
                app_name = txtAppName.Text = x_appname.Value;
                op_no = txtop_no.Text = x_opno.Value;
                sn = txtSN.Text = x_sn.Value;
                PK = x_pk.Value;
            }
        }

        private void btnBuilderRegiste_Click(object sender, EventArgs e)
        {
            if (sn.IsNullOrEmpty())
            {
                new Exception("please select config file!");
            }
            DateTime start_date = dtStart.Value;
            int days = (int)numDays.Value;
            XElement x_config = MyRegister.BuilderConfig(app_name, op_no, sn, MyRegister.Config_State.Registed);
            x_config.AddChild(MyRegister.START_TIME, start_date.ToString("yyyy-MM-dd"));
            x_config.AddChild(MyRegister.DAYS, days);
            string config = x_config.ToString(SaveOptions.DisableFormatting);

            XElement root = new XElement("ROOT");


            string key = Encoding.UTF8.GetString(Convert.FromBase64String(PK));
            byte[] buff = Encoding.UTF8.GetBytes(config);

            byte[] miwen = CryptoHelper.RSAEncrypt(key, buff);

            root.AddChild(MyRegister.ROOTNAME, Convert.ToBase64String(miwen));
            root.AddChild(MyRegister.PK, key);

            string info = Convert.ToBase64String(Encoding.UTF8.GetBytes(root.ToString(SaveOptions.DisableFormatting)));
            string auto_string = "";
            Random random = new Random();
            int r_number = random.Next(0xF1, 0xFFFF);
            auto_string = r_number.ToString("X4");
            int count = r_number % 0xF;
            for (int i = 0; i < count; i++)
            {
                int temp = random.Next(33, 123);
                auto_string += (char)temp;
            }
            info = auto_string + info;


            //生成注册信息
            SaveFileDialog sfd = new SaveFileDialog()
            {
                FileName = "register_info.txt",
                Filter = "register_info.txt|*.txt"
            };
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //生成文本
                using (StreamWriter sw = new StreamWriter(sfd.FileName, false, Encoding.UTF8))
                {
                    sw.WriteLine(info);
                }
            }


        }
    }
}
