using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Util.Common;
using System.Xml.Linq;
using Util.Common.ExtensionHelper;
using System.IO;

namespace Util.WinForm
{
    /// <summary>
    /// 关于注册
    /// </summary>
    public class MyRegister
    {
        /// <summary>
        /// 根节点名称
        /// </summary>
        public const string ROOTNAME = "config";
        /// <summary>
        /// 标题
        /// </summary>
        public const string APP_NAME = "app_name";
        /// <summary>
        /// 运营商号
        /// </summary>
        public const string OP_NO = "op_no";
        /// <summary>
        /// 序列号
        /// </summary>
        public const string SN = "sn";
        /// <summary>
        /// 序列号
        /// </summary>
        public const string START_TIME = "starttime";
        /// <summary>
        /// 有效天数
        /// </summary>
        public const string DAYS = "days";
        /// <summary>
        /// 状态
        /// </summary>
        public const string STATE = "state";
        /// <summary>
        /// 密钥
        /// </summary>
        public const string PK = "PK";
        /// <summary>
        /// 注册信息状态
        /// </summary>
        public enum Config_State
        {
            /// <summary>
            /// 未注册
            /// </summary>
            UnRegiste = 0,
            /// <summary>
            /// 已注册
            /// </summary>
            Registed = 1,
        }
        const int min_char = 33;
        const int max_char = 122;
        /// <summary>
        /// 生成注册信息配置文件
        /// </summary>
        /// <param name="appname">软件名称</param>
        /// <param name="opno">运营商号</param>
        /// <param name="defaultname">配置文件名称</param>
        /// <returns>注册信息配置文件</returns>
        public static string BuilderRegiste(string appname, string opno, string defaultname = "register_config.txt")
        {
            string config = "";
            //生成注册信息
            SaveFileDialog sfd = new SaveFileDialog()
            {
                FileName = defaultname,
                Filter = "register_config.txt|*.txt"
            };
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string sn = DiskManager.GetDefaultNumber();
                XElement root = BuilderConfig(appname, opno, sn, Config_State.UnRegiste);

                //生成密钥
                string pk1 = "";
                string pk2 = "";
                CryptoHelper.BuilderKey(out pk1, out pk2);
                string pk = Convert.ToBase64String(Encoding.UTF8.GetBytes(pk2));
                root.AddChild(PK, pk);
                //生成文本
                config = root.ToString(SaveOptions.DisableFormatting);
                using (StreamWriter sw = new StreamWriter(sfd.FileName, false, Encoding.UTF8))
                {
                    sw.WriteLine(config);
                }
            }
            return config;
        }



        /// <summary>
        /// 生成配置信息XML
        /// </summary>
        /// <param name="appname">软件名称</param>
        /// <param name="opno">运营商号</param>
        /// <returns></returns>
        public static XElement BuilderConfig(string appname, string opno)
        {
            string sn = DiskManager.GetDefaultNumber();
            XElement root = BuilderConfig(appname, opno, sn, Config_State.UnRegiste);
            return root;
        }


        /// <summary>
        /// 生成配置信息XML
        /// </summary>
        /// <param name="appname">软件名称</param>
        /// <param name="opno">运营商号</param>
        /// <param name="sn">序列号</param>
        /// <param name="state">配置文件状态</param>
        /// <returns></returns>
        public static XElement BuilderConfig(string appname, string opno, string sn, Config_State state)
        {
            XElement config = new XElement(ROOTNAME);
            config.AddChild(APP_NAME, appname);
            config.AddChild(OP_NO, opno);
            config.AddChild(SN, sn);
            config.AddChild(STATE, state.ToInt32());
            return config;
        }


        /// <summary>
        /// 读取注册信息
        /// </summary>
        /// <param name="miwen">密文</param>
        /// <param name="register">返回注册信息</param>
        /// <returns>0:返回成功;1:序列号不一致;0xFFFF;未知异常</returns>
        public static int ReadRegiste_info(string miwen, ref RegisterInfos register)
        {
            try
            {
                uint t_head = (uint)miwen.Substring(0, 4).ToInt16(0, NumberStyle.Hex);
                int _a = (int)(t_head % 0xF);
                miwen = miwen.Substring(_a + 4);
                byte[] buff = Convert.FromBase64String(miwen);
                string info = Encoding.UTF8.GetString(buff);
                XElement root = XElement.Parse(info);
                XElement x_key = root.Element(PK);
                XElement x_config = root.Element(ROOTNAME);
                string key = x_key.Value;
                string config = x_config.Value;

                byte[] miwen_buff = CryptoHelper.RSADecrypt(key, Convert.FromBase64String(config));

                string config_node = Encoding.UTF8.GetString(miwen_buff);


                string sn = DiskManager.GetDefaultNumber();
                RegisterInfos temp = new RegisterInfos(config_node);
                if (temp.SN != sn)
                {
                    return 1;
                }
                register.Set(temp);
                return 0;
            }
            catch (Exception ex)
            {
                return 0xFFFF;
            }
        }
    }


    /// <summary>
    /// 注册信息
    /// </summary>
    [Serializable]
    public class RegisterInfos
    {
        /// <summary>
        /// 构造
        /// </summary>
        public RegisterInfos()
        {
        }
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="info">xml</param>
        public RegisterInfos(string info)
        {
            XElement root = XElement.Parse(info);
            XElement x_sn = root.Element(MyRegister.SN);
            XElement x_op_no = root.Element(MyRegister.OP_NO);
            XElement x_days = root.Element(MyRegister.DAYS);
            XElement x_starttime = root.Element(MyRegister.START_TIME);
            XElement x_state = root.Element(MyRegister.STATE);
            XElement x_app_name = root.Element(MyRegister.APP_NAME);

            SN = x_sn.Value;
            OP_NO = x_op_no.Value;
            START_TIME = x_starttime.Value.ToDateTime();
            DAYS = x_days.Value.ToInt32();
            State = x_state.Value == "1" ? MyRegister.Config_State.Registed : MyRegister.Config_State.UnRegiste;
            App_Name = x_app_name.Value;
        }
        /// <summary>
        /// 序列号
        /// </summary>
        public string SN { get; private set; }
        /// <summary>
        /// 运营商号
        /// </summary>
        public string OP_NO { get; private set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime START_TIME { get; private set; }
        /// <summary>
        /// 有效天数
        /// </summary>
        public int DAYS { get; private set; }
        /// <summary>
        /// 状态
        /// </summary>
        public MyRegister.Config_State State { get; private set; }
        /// <summary>
        /// 软件key
        /// </summary>
        public string App_Name { get; set; }
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="temp">注册信息对象</param>
        public void Set(RegisterInfos temp)
        {
            SN = temp.SN;
            OP_NO = temp.OP_NO;
            START_TIME = temp.START_TIME;
            DAYS = temp.DAYS;
            State = temp.State;
            App_Name = temp.App_Name;
        }

        public override string ToString()
        {
            return string.Format("sn:{0}    \r\nop_no:{1}    \r\nstart_time:{2}     \r\ndays:{3}", SN, OP_NO, START_TIME, DAYS);
        }

    }
}
