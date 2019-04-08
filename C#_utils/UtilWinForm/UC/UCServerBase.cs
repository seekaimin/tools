using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Util.WinForm.UC
{
    /// <summary>
    /// 带服务的控件基类
    /// </summary>
    public class UCServerBase : UserControl
    {
        /// <summary>
        /// 消息通知句柄
        /// </summary>
        public event Action<string> DoNotice;
        private RuningStates _RuningState = RuningStates.STOPED;

        /// <summary>
        /// 服务运行状态
        /// </summary>
        public RuningStates RuningState
        {
            get { return _RuningState; }
            protected set { _RuningState = value; }
        }
        /// <summary>
        /// 通知消息
        /// </summary>
        /// <param name="mesage">消息</param>
        public void Notice(string mesage)
        {
            if (DoNotice != null)
            {
                DoNotice(this.Name + ":" + mesage);
            }
        }
        /// <summary>
        /// 启动
        /// </summary>
        public void Start()
        {
            if (RuningState != RuningStates.STOPED)
            {
                return;
            }
            RuningState = RuningStates.STARTING;
            try
            {
                if (IsValidated() == false)
                {
                    return;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                this.Notice(e.Message);
                return;
            }
            Begin();
        }
        /// <summary>
        /// 验证
        /// </summary>
        /// <returns></returns>
        protected virtual bool IsValidated()
        {
            return true;
        }
        /// <summary>
        /// 启动
        /// </summary>
        protected virtual void Begin()
        {

        }
        /// <summary>
        /// 结束
        /// </summary>
        protected virtual void End()
        {

        }
        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            if (this.RuningState == RuningStates.STOPING || this.RuningState == RuningStates.STOPED)
            {
                return;
            }
            try
            {
                this.RuningState = RuningStates.STOPING;
                End();
            }
            catch (Exception e)
            {
            }
            finally
            {
                this.RuningState = RuningStates.STOPED;
            }
        }
        /// <summary>
        /// 重新启动
        /// </summary>
        public void Reset()
        {
            this.Stop();
            this.Start();
        }
        private void btn_setting_Click(object sender, EventArgs e)
        {

        }
        private void btn_start_Click(object sender, EventArgs e)
        {
            this.Start();
        }
        private void btn_stop_Click(object sender, EventArgs e)
        {
            this.Stop();
        }
        private void btn_reset_Click(object sender, EventArgs e)
        {
            this.Reset();
        }
        /// <summary>
        /// 服务运行状态
        /// </summary>
        public enum RuningStates
        {
            /// <summary>
            /// 启动中
            /// </summary>
            STARTING,
            /// <summary>
            /// 已启动
            /// </summary>
            STARTED,
            /// <summary>
            /// 停止中
            /// </summary>
            STOPING,
            /// <summary>
            /// 已停止
            /// </summary>
            STOPED,
        }
    }
}
