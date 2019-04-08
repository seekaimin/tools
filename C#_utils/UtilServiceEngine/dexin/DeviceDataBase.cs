using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Util.Common.ExtensionHelper;
using System.Net;

namespace Util.ServiceEngine.dexin
{
    /// <summary>
    /// dexin设备搜索数据结构
    /// </summary>
    public class DeviceDataBase
    {
        /// <summary>
        /// 10B 命令类型，固定为 19 49 19 94 20 05 20 12 20 17
        /// </summary>
        public byte[] CommandType = new byte[] { 0x19, 0x49, 0x19, 0x94, 0x20, 0x05, 0x20, 0x12, 0x20, 0x17 };
        /// <summary>
        /// 2B 协议版本，目前为 00 00
        /// </summary>
        public int ProtocolVersion { get; set; }
        /// <summary>
        /// 2B  命令头部长度，目前为 00 34 (52 字节)
        /// </summary>
        public int HeadLength { get; set; }
        /// <summary>
        ///  2B 描述循环部分总长度，没有则为 00 00
        /// </summary>
        public int DescriptionLoopLength { get; set; }
        /// <summary>
        /// 2 操作码： 0=探测;1=报告;2=修改
        /// </summary>
        public int OperationCode { get; set; }
        /// <summary>
        ///  6B 设备的 MAC 地址
        /// </summary>
        public byte[] MACAddress { get; set; }
        /// <summary>
        /// mac字符串
        /// </summary>
        public string MACAddressString
        {
            get
            {
                return this.MACAddress.ShowString();
            }
            set
            {
                this.MACAddress = value.StringToBytes();
            }
        }
        /// <summary>
        ///  4B 设备启动随机码，用于 IP 和 MAC 都一样的情况。
        /// </summary>
        public long BootRandCode { get; set; }
        /// <summary>
        ///  4B 设备的产品 ID，和 *.hw 文件的产品 ID 相同
        /// </summary>
        public long ProductID { get; set; }
        /// <summary>
        ///  4B 子 ID,保留用，比如定制版本等.默认为 00 00 00 00
        /// </summary>
        public long SubID { get; set; }
        /// <summary>
        ///  4B 设备 NMS 口的 IP 地址，网管使用。
        /// </summary>
        public string NMSIP = "";
        /// <summary>
        /// 4B 网管口掩码地址 默认为 FF FF FF 00
        /// </summary>
        public string NMSMask { get; set; }
        /// <summary>
        ///  4B 网管口网关 IP 地址
        /// </summary>
        public string NMSGateway { get; set; }
        /// <summary>
        ///  2B 网管端口. 2007=NMS app; 161=SNMP; 80=Web.
        /// </summary>
        public int NMSPort { get; set; }
        /// <summary>
        ///  2B 网管类型. 1=NMS; 2=SNMP; 4=Web; 6=SNMP_Web
        /// </summary>
        private int NMSType { get; set; }

        int size = 52;
        /// <summary>
        /// 解析成功失败标志
        /// </summary>
        public bool ParseFlag { get; set; }
        /// <summary>
        /// 数据解析
        /// </summary>
        /// <param name="data">需要解析的数据</param>
        public void Parse(byte[] data)
        {
            ParseFlag = false;
            if (data.Length < size)
            {
                return;
            }
            int index = 0;
            byte[] command_type = data.GetBytes(10, ref index);
            if (CommandType.Eq(command_type) == false)
            {
                return;
            }
            this.ProtocolVersion = data.GetUInt16(ref index);
            this.HeadLength = data.GetUInt16(ref index);
            if (data.Length != this.HeadLength)
            {
                throw new Exception("data length error!");
            }
            this.DescriptionLoopLength = data.GetUInt16(ref index);
            this.OperationCode = data.GetUInt16(ref index);
            this.MACAddress = data.GetBytes(6, ref index);
            this.BootRandCode = data.GetUInt32(ref index);
            this.ProductID = data.GetUInt32(ref index);
            this.SubID = data.GetUInt32(ref index);
            byte[] temp = data.GetBytes(4, ref index);
            this.NMSIP = new IPAddress(temp).ToString();
            temp = data.GetBytes(4, ref index);
            this.NMSMask = new IPAddress(temp).ToString();
            temp = data.GetBytes(4, ref index);
            this.NMSGateway = new IPAddress(temp).ToString();
            this.NMSPort = data.GetUInt16(ref index);
            this.NMSType = data.GetUInt16(ref index);
            ParseFlag = this.Parse(data, ref index);
        }
        /// <summary>
        /// 出去头部分的其他协议数据  自行解析
        /// </summary>
        /// <param name="data">总数据</param>
        /// <param name="index">解析下标</param>
        /// <returns></returns>
        protected virtual bool Parse(byte[] data, ref int index)
        {
            return true;
        }
    }
}

