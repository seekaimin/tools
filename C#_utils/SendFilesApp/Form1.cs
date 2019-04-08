using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Net.Sockets;
using System.Net;

namespace SendFilesApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnselectfile1_Click(object sender, EventArgs e)
        {
            OpenFileDialog f = new OpenFileDialog();
            if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtfile1.Text = f.FileName;
            }
        }
        private void btnselectfile2_Click(object sender, EventArgs e)
        {
            OpenFileDialog f = new OpenFileDialog();
            if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtfile2.Text = f.FileName;
            }
        }
        private void btnselectfile3_Click(object sender, EventArgs e)
        {
            OpenFileDialog f = new OpenFileDialog();
            if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtfile3.Text = f.FileName;
            }
        }

        bool issending = false;
        bool IsRuning
        {
            get { return issending && (this.Disposing == false) && (this.IsDisposed == false); }
        }
        bool isfile1 = false;
        bool isfile2 = false;
        bool isfile3 = false;
        IPEndPoint endpoint;
        decimal defaultrate = 0.5m;
        int maxsize = 188 * 2;
        private void btnstart_Click(object sender, EventArgs e)
        {
            string s = "abcdefgh";
            int size = 2;
            for (int i = 0; i < s.Length; i += size)
            {
                if (s.Length - i < size)
                {
                    size = s.Length - i;
                }
                string t = s.Substring(i, size);
                MessageBox.Show(t);
            }

            return;
            issending = true;
            string file1 = txtfile1.Text;
            string file2 = txtfile2.Text;
            string file3 = txtfile3.Text;
            bool loop = cboloop.Checked;
            endpoint = new IPEndPoint(IPAddress.Parse(txtip.Text), int.Parse(txtport.Text));
            isfile1 = File.Exists(file1);
            isfile2 = File.Exists(file2);
            isfile3 = File.Exists(file3);
            defaultrate = Convert.ToDecimal(txtdefaultrate.Text);
            new Thread(() =>
            {
                do
                {
                    Console.WriteLine("====================================");
                    Console.WriteLine("============send file start==========");
                    Console.WriteLine("====================================");
                    Thread.Sleep(1000);
                    using (UdpClient client = new UdpClient())
                    {
                        if (isfile1)
                        {
                            //发送文件1
                            Send(file1, client);
                            Console.WriteLine("============file1 end===============");
                        }
                        if (isfile2)
                        {
                            //发送文件3
                            Send(file2, client);
                            Console.WriteLine("============file2 end===============");
                        }
                        if (isfile2)
                        {
                            //发送文件3
                            Send(file3, client);
                            Console.WriteLine("============file3 end===============");
                        }
                    }
                    Console.WriteLine("====================================");
                    Console.WriteLine("==============all file end===========");
                    Console.WriteLine("=");
                    Console.WriteLine("=");
                    Console.WriteLine("=");
                    Console.WriteLine("=");
                    Console.WriteLine("=");
                    Console.WriteLine("=");
                    Console.WriteLine("=");
                    Console.WriteLine("=");
                    Console.WriteLine("=");
                    Console.WriteLine("=");
                } while (IsRuning && loop);
            }).Start();
        }
        private void btnstop_Click(object sender, EventArgs e)
        {
            issending = false;
        }

        public void Send(string path, UdpClient client)
        {
            DateTime dt = DateTime.Now;
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                byte[] buff = new byte[maxsize];
                byte[] temp = new byte[(int)fs.Length];
                fs.Read(temp, 0, temp.Length);
                int index = 0;
                while (IsRuning && index < temp.Length - 1)
                {
                    //Array.Clear(buff, 0, buff.Length);
                    int size = maxsize;
                    if (temp.Length - index < size)
                    {
                        size = (int)(temp.Length - index);
                    }
                    Array.Copy(temp, index, buff, 0, size);
                    //发送
                    client.Send(buff, size, endpoint);
                    index += size;
                    decimal timespan = (decimal)(DateTime.Now - dt).TotalSeconds;
                    if (timespan > 0)
                    {
                        decimal rate = (index / 1024m / 1024m / timespan);
                        decimal calcsize = defaultrate * timespan * 1024m * 1024;
                        Console.WriteLine("rate:{0}/{1}", rate, defaultrate);
                        if (rate > defaultrate)
                        {
                            int sleep = (int)((rate - defaultrate) * 1000m);
                            //Console.WriteLine("sleep:{0}", sleep);
                            if (sleep > 0)
                                Thread.Sleep(sleep);
                        }
                    }
                }
            }
        }

    }
}
