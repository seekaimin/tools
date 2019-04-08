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
    public partial class s : Form
    {
        public s()
        {
            InitializeComponent();
        }

        private void btnSelete_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtPath.Text = ofd.FileName;
            }
        }
        private void btnSend_Click(object sender, EventArgs e)
        {
            SenderHelper.Ins.Dispose();
            decimal rate = 0m;
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
            if (rate <= 0)
            {
                txtRate.Focus();
                return;
            }
            string path = txtPath.Text;
            new Thread(() =>
            {
                List<byte> buffs = new List<byte>();
                int size = 1024;
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    byte[] temp = new byte[size];
                    do
                    {
                        if (fs.Length - fs.Position >= size)
                        {
                            fs.Read(temp, 0, size);
                            buffs.AddRange(temp);
                        }
                        else
                        {
                            size = (int)(fs.Length - fs.Position);
                            temp = new byte[size];
                            fs.Read(temp, 0, size);
                            buffs.AddRange(temp);
                            break;
                        }
                    } while (true);
                }
                //ss.Send(buffs.ToArray(), rate, txtIP.Text, Convert.ToInt32(txtPort.Text));
                //Send(path, rate, IPAddress.Parse("224.2.2.2"), 1009);
            }).Start();
        }
        bool flag = false;
        void Send(string path, decimal rate, IPAddress ip, int port)
        {
            flag = true;
            if (!File.Exists(path))
            {
                new FileNotFoundException();
            }
            //发送间隔
            int timespan = 100;
            //发送起始时间
            DateTime starttime = DateTime.Now;
            //单位时间发送字节数
            int ratebytimespan = (int)((rate * 1024m) * ((timespan / 1000m)));
            byte[] buffs = new byte[0];
            #region 读取文件保存到内存
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                buffs = new byte[fs.Length];
                fs.Read(buffs, 0, buffs.Length);
            }
            #endregion
            IPEndPoint endpoint = new IPEndPoint(ip, port);
            using (UdpClient client = new UdpClient())
            {
                starttime = DateTime.Now;
                do
                {
                    byte[] temp = new byte[ratebytimespan];
                    int size = ratebytimespan;
                    for (int i = 0; i < buffs.Length; )
                    {
                        if (!flag) break;
                        if (i + ratebytimespan > buffs.Length) { size = buffs.Length - i; }
                        else { size = ratebytimespan; }
                        i += size;
                        client.Send(temp, size, endpoint);

                        double timespans = (DateTime.Now - starttime).TotalMilliseconds;
                        double f = (i * 100) / timespans;
                        Console.WriteLine("current rate:{0}/{1}   {2}/{3}", f, ratebytimespan, i, buffs.Length);
                        if (f > ratebytimespan)
                        {
                            Thread.Sleep(timespan);
                        }
                    }
                    starttime = DateTime.Now;
                } while (flag);
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            SenderHelper.Ins.Dispose();
            flag = false;
        }

        private void btnSendWithRate_Click(object sender, EventArgs e)
        {
            decimal rate = Convert.ToDecimal(txtRate.Text);
            bool loop = cboIsCalcRate.Checked;
            string ip = txtIP.Text;
            string port = txtPort.Text;
            SenderHelper.Ins.Init(loop, SenderModels.IP);
            SenderHelper.Ins.Send(txtPath.Text, rate, new UdpClient(), new IPEndPoint(IPAddress.Parse(ip), int.Parse(port)));
        }
    }
}
