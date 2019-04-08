using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Util.Common.AlgorithmVote
{
    /// <summary>
    /// 选举
    /// </summary>
    public class Vote
    {

        private long _ServiceId = 0;
        private long _ZxId = 0;
        private long _Epoch = 0;
        private States _ServiceState = States.STOPED;
        private int _VoteTimeOut = 1000 * 5;
        /// <summary>
        /// 服务状态值
        /// </summary>
        public enum States
        {
            /// <summary>
            /// 竞选状态。
            /// </summary>
            LOOKING,
            /// <summary>
            /// 随从状态，同步leader状态，参与投票。
            /// </summary>
            FOLLOWING,
            /// <summary>
            /// 观察状态, 同步leader状态，不参与投票。
            /// </summary>
            OBSERVING,
            /// <summary>
            /// 领导者状态。
            /// </summary>
            LEADING,
            /// <summary>
            /// 停止
            /// </summary>
            STOPED

        }
        /// <summary>
        /// 在配置server时，给定的服务器的标示id
        /// </summary>
        public long ServiceId { get => _ServiceId; set => _ServiceId = value; }
        /// <summary>
        /// 服务器在运行时产生的数据id，zxid越大，表示数据越新
        /// </summary>
        public long ZxId { get => _ZxId; set => _ZxId = value; }
        /// <summary>
        /// 选举的轮数，即逻辑时钟。随着选举的轮数++
        /// </summary>
        public long Epoch { get => _Epoch; set => _Epoch = value; }
        /// <summary>
        /// 服务状态
        /// </summary>
        public States ServiceState { get => _ServiceState; set => _ServiceState = value; }
        /// <summary>
        /// 选举超时时间
        /// </summary>
        public int VoteTimeOut { get => _VoteTimeOut; set => _VoteTimeOut = value; }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="serverId">在配置server时，给定的服务器的标示id</param>
        /// <param name="zxId">服务器在运行时产生的数据id，zxid越大，表示数据越新</param>
        /// <param name="epoch">选举的轮数，即逻辑时钟。随着选举的轮数++</param>
        public Vote(long serverId, long zxId, int epoch)
        {
            this.ServiceId = serverId;
            this.ZxId = zxId;
            this.Epoch = epoch;
        }
    }
}
