using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TsSenders
{
    /// <summary>
    /// 数据发送基类
    /// </summary>
    public abstract class SenderBase : IDisposable
    {
        /// <summary>
        /// 同步字节
        /// </summary>
        protected const byte syns_byte = 0x47;
        /// <summary>
        /// 数据包长度
        /// </summary>
        protected const int package_size = 188;
        private bool _Loop = false;

        /// <summary>
        /// 已发送的包个数
        /// </summary>
        protected decimal send_total_package_count { get; set; }
        /// <summary>
        /// 每毫秒发送的包个数
        /// </summary>
        protected decimal total_package_count_bymillisecond { get; set; }

        /// <summary>
        /// 开始发送时间
        /// </summary>
        public DateTime StartTime { get; protected set; }
        /// <summary>
        /// 已发送的时间
        /// </summary>
        protected decimal send_total_milliseconds
        {
            get { return (decimal)(DateTime.Now - StartTime).TotalMilliseconds; }
        }
        private SenderModels _SenderModel = SenderModels.IP;


        /// <summary>
        /// 放松方式  默认IP
        /// </summary>
        public SenderModels SenderModel
        {
            get { return _SenderModel; }
            set { _SenderModel = value; }
        }

        private RunningStates _RunningState = RunningStates.Stop;
        /// <summary>
        /// 循环
        /// </summary>
        public bool Loop
        {
            get { return _Loop; }
            set { _Loop = value; }
        }
        /// <summary>
        /// 0:停止;1:正在运行
        /// </summary>
        public RunningStates RunningState
        {
            get { return _RunningState; }
            set { _RunningState = value; }
        }
        /// <summary>
        /// 自定义释放资源
        /// </summary>
        public void MyDispose(){}
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            RunningState = RunningStates.Stop;
            MyDispose();
        }
    }
}
