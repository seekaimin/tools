using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ServiceProcess;
using System.Threading;

namespace Util.ServiceEngine.ServiceForms
{
    /// <summary>
    /// 服务管理进度条
    /// </summary>
    internal partial class FormProgressBar : Form
    {
        internal bool IsInstall = false;
        /// <summary>
        /// 服务管理进度条
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        /// <param name="model">服务模式</param>
        /// <param name="args">参数</param>
        public FormProgressBar(string serviceName, ServiceControlModel model, params string[] args)
        {
            InitializeComponent();
            try
            {
                serviceController.ServiceName = serviceName;
#if DEBUG
                string service = serviceController.DisplayName;
                Console.WriteLine(service);
#endif
                bool temp = serviceController.CanShutdown;
                IsInstall = true;
                bkWork.RunWorkerAsync(new object[] { model, args });
            }
            catch
            {
                ServiceHelper.ShowMessage("system service", "service is not install!");
                killTimer.Start();
            }
        }

        private void killTimer_Tick(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
        private void bkWork_DoWork(object sender, DoWorkEventArgs e)
        {
            int proVal = 10;
            this.bkWork.ReportProgress(proVal);
            Thread.Sleep(100);
            object[] args = e.Argument as object[];
            ServiceControlModel controlModel = (ServiceControlModel)args[0];
            string[] parameters = args[1] as string[];
            ServiceControllerStatus serviceStatus = ServiceControllerStatus.Stopped;
            proVal = 20;
            this.bkWork.ReportProgress(proVal);
#if DEBUG
            Console.WriteLine("操作 [{0}]", controlModel);
#endif
            switch (controlModel)
            {
                case ServiceControlModel.Start:
                    serviceStatus = ServiceControllerStatus.Running;
#if DEBUG
                    Console.WriteLine("实际状态 [{0}]", serviceController.Status);
#endif
                    if (serviceStatus != serviceController.Status)
                    {
                        if (parameters != null)
                            serviceController.Start(parameters);
                        else
                            serviceController.Start();
                    }
                    break;
                case ServiceControlModel.Stop:
                    serviceStatus = ServiceControllerStatus.Stopped;
#if DEBUG
                    Console.WriteLine("实际状态 [{0}]", serviceController.Status);
#endif
                    if (serviceStatus != serviceController.Status)
                        serviceController.Stop();
                    break;
                case ServiceControlModel.ReStart:
#if DEBUG
                    Console.WriteLine("实际状态 [{0}]", serviceController.Status);
#endif
                    if (serviceStatus != serviceController.Status)
                        serviceController.Stop();
                    DoServiceStatus(ServiceControllerStatus.Stopped, proVal, 50);
                    proVal = 50;
                    if (parameters != null)
                        serviceController.Start(parameters);
                    else
                        serviceController.Start();
                    serviceStatus = ServiceControllerStatus.Running;
                    break;
            }
            DoServiceStatus(serviceStatus, proVal, 90);
            proVal = 100;
            this.bkWork.ReportProgress(proVal);
        }
        void DoServiceStatus(ServiceControllerStatus status, int startValue, int endvalue)
        {
            int val = startValue;
            int first = (startValue + endvalue) / 10;
            while (true)
            {
                if (val < (startValue + endvalue) / 2)
                {
                    val += first;
                }
                else if (val < endvalue)
                {
                    val++;
                }
                serviceController.Refresh();
                this.bkWork.ReportProgress(val);

                if (serviceController.Status == status)
                {
                    break;
                }
                else if (status == ServiceControllerStatus.Running && serviceController.Status == ServiceControllerStatus.Stopped)
                {
                    break;
                }
                Thread.Sleep(1000);
            }
            val = endvalue;
            this.bkWork.ReportProgress(val);
        }
        private void bkWork_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.valBar.Value = e.ProgressPercentage;
        }
        private void bkWork_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Thread.Sleep(500);
            killTimer.Start();
        }
    }
}
