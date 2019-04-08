using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Util.ServiceEngine.Core;

namespace Util.ServiceEngine.ServiceForms
{
    /// <summary>
    /// delegate
    /// </summary>
    /// <param name="notifyForm">Form</param>
    internal delegate void FormCloseEventHandeller(Form notifyForm);
    /// <summary>
    /// ToolTipHelperForm
    /// </summary>
    internal partial class ToolTipHelperForm : Form
    {
        private Desktop m_Desktop = new Desktop();

        private ToolTopMessage balloonMessage;
        /// <summary>
        /// 获取或设置提示消息
        /// </summary>
        public ToolTopMessage BalloonMessage
        {
            get { return balloonMessage; }
            set { balloonMessage = value; }
        }
        /// <summary>
        /// ToolTipHelperForm
        /// </summary>
        public ToolTipHelperForm()
        {
            InitializeComponent();

            serviceNotify.BalloonTipClosed += delegate(object sender, EventArgs e)
            {
                this.Exit();
            };

            serviceNotify.BalloonTipClicked += delegate(object sender, EventArgs e)
            {
                this.Exit();
            };

            serviceNotify.BalloonTipShown += delegate(object sender, EventArgs e)
            {
                this.timerFormKiller.Start();
            };
        }

        /// <summary>
        /// 显示提示消息
        /// </summary>
        /// <param name="message">要显示的消息</param>
        public void ShowMessage(object message)
        {
            if (!m_Desktop.BeginInteraction())
            {
                this.Exit();
                return;
            }
            else
            {
                this.balloonMessage = message as ToolTopMessage;
                Application.Run(this);
            }
        }

        /// <summary>
        /// 关闭提示消息
        /// </summary>
        public void Exit()
        {
            this.Invoke(new FormCloseEventHandeller(delegate(Form notifyForm)
            {
                notifyForm.Close();
            }), this);
        }
        /// <summary>
        /// OnClosed
        /// </summary>
        /// <param name="e">EventArgs</param>
        protected override void OnClosed(EventArgs e)
        {
            serviceNotify.Visible = false;
            serviceNotify.Dispose();
            components.Dispose();
            this.Dispose();
        }

        private void timerFormKiller_Tick(object sender, EventArgs e)
        {
            this.timerFormKiller.Stop();
            this.Close();
        }

        private void timerDeadFormKiller_Tick(object sender, EventArgs e)
        {
            this.timerDeadFormKiller.Stop();
            this.Exit();
        }

        private void ToolTipHelperForm_Load(object sender, EventArgs e)
        {
            Icon icon;
            switch (balloonMessage.BalloonType)
            {
                case ToolTipIcon.Info:
                    icon = new Icon(SystemIcons.Information, 16, 16);
                    break;
                case ToolTipIcon.Warning:
                    icon = new Icon(SystemIcons.Warning, 16, 16);
                    break;
                case ToolTipIcon.Error:
                    icon = new Icon(SystemIcons.Error, 16, 16);
                    break;
                default:
                    icon = new Icon(SystemIcons.Application, 16, 16);
                    break;
            }


            serviceNotify.Icon = icon;
            this.Visible = false;
            serviceNotify.Visible = true;

            serviceNotify.ShowBalloonTip(balloonMessage.Timeout
                , balloonMessage.Title
                , balloonMessage.Content
                , balloonMessage.BalloonType
                );

            this.timerFormKiller.Interval = balloonMessage.Timeout;
            this.timerDeadFormKiller.Interval = balloonMessage.Timeout * 4;
            this.timerDeadFormKiller.Start();
        }
    }
}
