using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TsSenders
{
    /// <summary>
    /// 播放盒发送
    /// </summary>
    public class PalyerSender : SenderBase
    {
        private double _Rate = 150f;
        /// <summary>
        /// 获取指定码率 public getter; private setter
        /// </summary>
        public double Rate
        {
            get { return _Rate; }
            private set { _Rate = value; }
        }
        /// <summary>
        /// 设置码率
        /// </summary>
        /// <param name="rate">码率</param>
        public void SetRate(double rate)
        {
            if (rate < 0)
                throw new Exception(string.Format("rate {0} error!", rate));
            Rate = rate;
        }
        /// <summary>
        /// 检核
        /// </summary>
        public void Check()
        {
            if (TsPlayer.IsOpen == false)
            {
                TsPlayer.OpenDevice(true);
                TsPlayer.SetRate(Rate);
            }
        }
    }
}
