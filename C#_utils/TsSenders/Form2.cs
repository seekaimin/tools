using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Net.Sockets;
using System.Net;

namespace TsSenders
{
    public partial class Form2 : Form
    {
        bool isStop = false;
        public Form2()
        {
            InitializeComponent();
        }
        int package_size = 188;
        /// <summary>
        /// 3 K 
        /// </summary>
        decimal default_total_rate = 100m;

        decimal total_size = 0;
        decimal total_rate = 0;
        decimal active_rate = 0;
        UdpClient udp = new UdpClient(new System.Net.IPEndPoint(IPAddress.Parse("192.168.19.1"), 0));
        IPEndPoint remote_endpoin = new IPEndPoint(IPAddress.Parse("224.2.2.2"), 1002);
        Random random = new Random();
        private void btn_active_Click(object sender, EventArgs e)
        {
            total_size = 0;
            send_count = 0;
            default_total_rate = Convert.ToDecimal(txt_total_rate.Text);
            isStop = false;
            List<byte[]> packages = new List<byte[]>();
            for (int i = 0; i < 100; i++)
            {
                byte[] package = new byte[package_size];
                for (int j = 0; j < package_size; j++)
                {
                    package[j] = 0xFF;
                }
                packages.Add(package);
            }
            new Thread(() =>
            {
                int index = 0;
                decimal send_package_size = 0;
                DateTime dtnow = DateTime.Now;

                while (isStop == false)
                {
                    if (this.Disposing || this.IsDisposed)
                    {
                        break;
                    }
                    if (index == packages.Count)
                    {
                        index = 0;
                    }
                    if (index % 0xFFFFFF == 0)
                    {
                        send_package_size = 0;
                        dtnow = DateTime.Now;


                        total_size = 0;
                        send_start_time = dtnow;
                    }
                    send(packages[index++]);
                    send_package_size = send_package_size + package_size;
                    double send_milliseconds = (DateTime.Now - dtnow).TotalMilliseconds;

                    try
                    {
                        active_rate = send_package_size / (decimal)send_milliseconds;
                        label1.Invoke(new Action(() =>
                        {
                            label1.Text = active_rate.ToString();
                        }));
                    }
                    catch { }
                    int sleep = random.Next(1, 500);
                    Thread.Sleep(sleep);
                }
            })
            {
                Priority = ThreadPriority.Highest,
            }.Start();
        }
        private void btn_supply_Click(object sender, EventArgs e)
        {
            byte[] empty_package = new byte[package_size];
            for (int i = 0; i < empty_package.Length; i++)
            {
                empty_package[i] = 0xFF;
            }
            new Thread(() =>
            {
                while (isStop == false)
                {
                    if (this.Disposing || this.IsDisposed)
                    {
                        break;
                    }
                    double send_milliseconds = (DateTime.Now - send_start_time).TotalMilliseconds;

                    try
                    {
                        total_rate = total_size / (decimal)send_milliseconds;
                        decimal calc_rate = default_total_rate - total_rate;
                        Console.WriteLine("total_rate:{0:0.0000}   active_rate:{1:0.0000}", total_rate, active_rate);
                        //k
                        if (calc_rate > 0)
                        {
                            decimal supply_bytes = calc_rate * 1024m;
                            int supply_package_count = (int)((supply_bytes / package_size) + 1);
                            if (supply_package_count > 100)
                                supply_package_count = 100;
                            for (int i = 0; i < supply_package_count; i++)
                            {
                                send(empty_package);
                            }
                            send_milliseconds = (DateTime.Now - send_start_time).TotalMilliseconds;
                            total_rate = total_size / (decimal)send_milliseconds;
                        }
                        label2.Invoke(new Action(() =>
                        {
                            label2.Text = total_rate.ToString();
                        }));
                    }
                    catch { }
                    Thread.Sleep(1);
                }
            })
            {
                //Priority = ThreadPriority.AboveNormal,
            }.Start();
        }

        int send_count = 0;
        DateTime send_start_time = DateTime.Now;
        void send(byte[] package)
        {
            total_size = total_size + package_size;
            udp.Send(package, package_size, remote_endpoin);
        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            int i = -1;

            Console.WriteLine(i>>24);



            isStop = true;
        }
    }
}
