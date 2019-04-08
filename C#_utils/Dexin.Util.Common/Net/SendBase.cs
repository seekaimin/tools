using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dexin.Util.Common.Net
{
    /// <summary>
    /// SendBase
    /// </summary>
    public abstract class SendBase
    {
        /// <summary>
        /// 数据包长度
        /// </summary>
        const int PACKAGE_SIZE = 188;


        /// <summary>
        /// 是否循环
        /// </summary>
        public bool Loop { get; set; }
        /// <summary>
        /// 设置码率(单位：M)
        /// </summary>
        public decimal Rate { get; set; }

        private int send_packages_size = 7;
        /// <summary>
        /// 默认发送包数量
        /// </summary>
        public int Send_packages_size
        {
            get { return send_packages_size; }
            set { send_packages_size = value; }
        }

        private int _MaxBufferSize = PACKAGE_SIZE * 40;
        /// <summary>
        /// 最大缓冲区大小   默认40个包
        /// </summary>
        public int MaxBufferSize
        {
            get { return _MaxBufferSize; }
            set { _MaxBufferSize = value; }
        }

        /// <summary>
        /// 已发送的包个数
        /// </summary>
        protected decimal send_packages_count { get; private set; }
        /// <summary>
        /// 按设置的码率理论上每秒发送的包个数
        /// </summary>
        protected decimal packages_count_bysecond
        {
            get
            {
                return Rate * 1024 * 1024 / PACKAGE_SIZE;
            }
        }
        /// <summary>
        /// 开始发送时间
        /// </summary>
        protected DateTime start_time { get; private set; }
        /// <summary>
        /// 已发送的时间
        /// </summary>
        protected decimal send_total_seconds
        {
            get { return (decimal)(DateTime.Now - start_time).TotalSeconds; }
        }

        /// <summary>
        /// 当前缓冲区大小
        /// </summary>
        int BufferSize { get; set; }
        /// <summary>
        /// 已读取的当前缓冲区位置
        /// </summary>
        int Buffer_Position { get; set; }
        /// <summary>
        /// 缓冲区
        /// </summary>
        byte[] Buffers = new byte[0];


    }
}
