using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace TsSenders
{
    public partial class FormSender : Form
    {
        decimal rate = 0m;
        string path = "";
        IPAddress ip;
        int port = 0;
        bool loop = false;
        /// <summary>
        /// 播放模式
        /// </summary>
        SenderModels sendermodel
        {
            get { return rb_sendmodel_ip.Checked ? SenderModels.IP : SenderModels.Plyer_Box; }
        }

        public FormSender()
        {
            InitializeComponent();
            if (this.Owner != null)
            {
                this.Icon = this.Owner.Icon;
            }
            SenderHelper.Ins.SenderStpoedHandller += () =>
            {
                checkstate(false);
            };
        }
        private void FormSender_Load(object sender, EventArgs e)
        {
            List<string> addres = new List<string>();
            foreach (IPAddress ip in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (ip.IsIPv6LinkLocal || ip.IsIPv6Multicast || ip.IsIPv6SiteLocal || ip.IsIPv6Teredo)
                    continue;
                else
                {
                    addres.Add(ip.ToString());
                }
            }
            cmblocalip.DataSource = addres;
        }
        void check()
        {
            try
            {
                rate = Convert.ToDecimal(txtRate.Text);
            }
            catch (Exception ex)
            {
                txtRate.Text = "";
                txtRate.Focus();
                rate = 0;
            }
            if (rate <= 0m)
            {
                txtRate.Focus();
                throw new Exception("rate is error!");
            }
            path = txtPath.Text;
            if (!File.Exists(path))
            {
                txtPath.Focus();
                throw new FileNotFoundException();
            }
            if (sendermodel == SenderModels.IP)
            {
                ip = IPAddress.Parse(txtip.Text);
                port = Convert.ToInt32(txtport.Text);
                if (port < 1 || port > 65535)
                {
                    throw new Exception("port is error!");
                }
            }
            loop = cbo_isloop.Checked;
        }
        void checkstate(bool state)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    checkstates(state);
                }));
            }
            else
            {
                checkstates(state);
            }
        }
        void checkstates(bool state)
        {
            btnStop.Enabled = state;
            btnSend.Enabled = btnselectfile.Enabled = state == false;
            if (btnSend.Enabled)
            {
                txtRate.ReadOnly = false;
                cbo_isloop.Enabled = rb_sendmodel_plyer.Enabled = rb_sendmodel_ip.Enabled = true;
                txtip.ReadOnly = txtport.ReadOnly = rb_sendmodel_plyer.Checked;
                cmblocalip.Enabled = !rb_sendmodel_plyer.Checked;
            }
            else
            {
                txtRate.ReadOnly = true;
                cbo_isloop.Enabled = rb_sendmodel_plyer.Enabled = rb_sendmodel_ip.Enabled = false;
                txtip.ReadOnly = txtport.ReadOnly = true;
                cmblocalip.Enabled = false;
            }

        }
        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                check();
                checkstate(true);
                if (sendermodel == SenderModels.IP)
                {
                    UdpClient client = new UdpClient();
                    if (cmblocalip.SelectedItem != null)
                    {
                        client = new UdpClient(new IPEndPoint(IPAddress.Parse(cmblocalip.SelectedItem.ToString()), 0));
                    }
                    SenderHelper.Ins.Init(loop, SenderModels.IP);
                    SenderHelper.Ins.Send(path, rate, client, new IPEndPoint(ip, port));
                }
                else if (sendermodel == SenderModels.Plyer_Box)
                {
                    SenderHelper.CheckPlayer(rate);
                    SenderHelper.Ins.Init(loop, SenderModels.Plyer_Box);
                    SenderHelper.Ins.Send(path, rate);
                }
            }
            catch (Exception ex)
            {
                SenderHelper.Ins.Dispose();
                if (ex != null)
                    MessageBox.Show(ex.Message);
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            SenderHelper.Ins.Dispose();
            //checkstate(false);
        }
        private void rb_sendmodel_CheckedChanged(object sender, EventArgs e)
        {
            checkstate(btnSend.Enabled == false);
        }
        private void btnselectfile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtPath.Text = ofd.FileName;
            }
        }
    }
}
