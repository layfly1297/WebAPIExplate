using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Owin.Hosting;

namespace WindowsFormsApplication1
{
    //web api 预览博客  ： https://www.cnblogs.com/landeanfen/p/5501490.html
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //非主线程亦可操作界面
            CheckForIllegalCrossThreadCalls = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //获取本机IP
            string IP = "";
            IPHostEntry here = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress _ip in here.AddressList)
            {
                if (_ip.AddressFamily.ToString().ToUpper() == "INTERNETWORK")
                {
                    IP = _ip.ToString();
                    break;
                }
            }
            string baseAddress = "http://" + IP + ":9000/";
            WebApp.Start<Startup>(url: baseAddress);
            AppendText("服务器IP及端口:" + baseAddress);
            AppendText("开始监听...");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //测试API(这里就没写测试的了,自己用Http工具自己试一下)
            //http://192.168.1.3:9000/api/log/Get?msg=234234
            //URL:http://192.168.1.3:9000/api/log/Post  请求体:msg=testMsg&log=testLog
            //URL:http://192.168.1.3:9000/api/log/log  请求体:msg=testMsg&log=testLog
        }


        public void AppendText(string msg)
        {
            textBox1.AppendText(msg + "\r\n");
        }

    }
}
