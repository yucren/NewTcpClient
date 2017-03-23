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

namespace WindowsFormsApplication1
{
    public partial class Customer : Form
    {
        public Customer()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            
        }

        private void Myclient_ConnectedEvent(object sender, ConnectedEventArgs e)
        {
            Console.WriteLine(e.LocalEndPoint);
            Console.WriteLine(e.RemoteEndPoint);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IPAddress ip = IPAddress.Parse("192.168.0.188");

            NewTcpClient myclient = new NewTcpClient();
            try
            {
                
                myclient.ConnectedEvent += Myclient_ConnectedEvent;
                myclient.Connect(ip, 9000);

            }
            catch (Exception ex)
            {
                textBox1.Text += ex.Message + "\n";
                myclient.Close();
                myclient = null;
                return;
            }
            textBox1.Text += "连接成功\n";

        }
    }
    public class ConnectedEventArgs:EventArgs
    {
        public ConnectedEventArgs(EndPoint localEndPoint, EndPoint remoteEndPoint)
        {
            LocalEndPoint = localEndPoint;
            RemoteEndPoint = remoteEndPoint;

        }
        public  EndPoint LocalEndPoint
        {
            get ;
            private set;
        }
        public EndPoint RemoteEndPoint
        {
            get;
            private set;
        }


    }
    public  class NewTcpClient:TcpClient
    {

        public delegate void ConnectedEventHandle(object sender, ConnectedEventArgs e);
        public event ConnectedEventHandle ConnectedEvent;

        public new void Connect(IPAddress ip, int port)
        {
            try
            {
                base.Connect(ip, port);
                RaiseConnectedEvent();
            }
            catch (Exception)
            {

                  throw new Exception("无法连接服务器");
               
            }
           
            

        }
        protected virtual void RaiseConnectedEvent()
        {
            if (ConnectedEvent !=null)
            {
                ConnectedEvent(this, new ConnectedEventArgs(this.Client.LocalEndPoint, Client.RemoteEndPoint));
            }
        }


    }
}
