using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.IO;
using Server.Properties;

namespace Server
{
    public partial class Server : Form
    {
     
        int begintcp = 1000;
        TcpListener mylister = new TcpListener(9000);
       
       // Dictionary<int,TcpListener> tcplisters =new Dictionary<int,TcpListener>();
        Dictionary<int, TcpClient> tcpClients = new Dictionary<int, TcpClient>();
        Dictionary<int, NetworkStream> tcpNetWorkStreams = new Dictionary<int, NetworkStream>();
              public Server() 
        {
            
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.AutoSize = false;
            
            Color FbackColor = (Color)Settings.Default["BackColor"];
            this.BackColor = FbackColor;
            AddTcpLister();
            //myServer.Start();
            //TcpClient newCustomer = myServer.AcceptTcpClient();

            //NetworkStream recevice = newCustomer.GetStream();
            //int length = recevice.Read(buffer, 0, buffer.Length);
            //textBox1.Text = Encoding.Unicode.GetString(buffer, 0, length);
                  }

        private void Server_FormClosing(object sender, FormClosingEventArgs e)
        {
         
        }
        
        public async void AddTcpLister()
        {
            mylister.Start();
            await AddTcpListerTask();

        }
        private  Task AddTcpListerTask()
        {
            Task AddlTask = Task.Run( () =>
            {
                do
                {
                                       
                    tcpClients.Add(begintcp, mylister.AcceptTcpClient());
                    textBox1.Text += "有客户进入，客户号：" + begintcp.ToString() + "\r\n"; 
                    ReceiveAsync(begintcp);
                    begintcp += 1;
                    
                } while (true); 
            });
            return AddlTask;
        }
        public async void ReceiveAsync(int dd)
        {
            await ReceiveTask(dd);
        }
        private Task ReceiveTask(int tcplister)
        {
            Task receiveTask = Task.Run(() =>
             {
                 byte[] mybuffer = new byte[8192];
                 tcpNetWorkStreams.Add(tcplister, tcpClients[tcplister].GetStream());
                 while (true)
                 {
                     int length = tcpNetWorkStreams[tcplister].Read(mybuffer, 0, mybuffer.Length);
                     if (length == 0)
                     {

                         tcpClients[tcplister].Close();
                         textBox1.Text += "客户退出：" + tcplister.ToString() + "\r\n";
                         
                         return;

                     }
                     string message = Encoding.Unicode.GetString(mybuffer, 0, length);
                     textBox1.Text += message + "\r\n";
                 }
                
             });
            return receiveTask;
             
        }
        /// <summary>
        /// 通过在线客户号，向客户回复消息。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void Reply_Click(object sender, EventArgs e)
        {
            //byte[] buffer = new byte[8192];
         byte[] WriteByte=   Encoding.Unicode.GetBytes(textBox3.Text);
            tcpNetWorkStreams[int.Parse(textBox2.Text)].Write(WriteByte, 0, WriteByte.Length);

        }

        private void 红色ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.BackColor = 红色ToolStripMenuItem.BackColor;
            
        }

        private void 蓝色ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.BackColor = 蓝色ToolStripMenuItem.BackColor;
        }

        private void 绿色ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.BackColor = 绿色ToolStripMenuItem.BackColor;
        }

        private void Server_FormClosed(object sender, FormClosedEventArgs e)
        {
            Settings.Default.BackColor = this.BackColor;
            Properties.Settings.Default.Save();


        }

        private void Server_ResizeBegin(object sender, EventArgs e)
        {

        }
    }
}
