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


namespace WindowsFormsApplication1
{
    public partial class Customer : Form
    {
        byte[] buffe = new byte[8192];
        IPAddress ip = IPAddress.Parse("192.168.0.188");

        NewTcpClient myclient = new NewTcpClient();
        public Customer()
        {
            InitializeComponent();
        }

       

        private void Myclient_ConnectedEvent(object sender, ConnectedEventArgs e)
        {
            Console.WriteLine(e.LocalEndPoint);
            Console.WriteLine(e.RemoteEndPoint);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (myclient.Connected==false)
            {
                try
                {

                    myclient.ConnectedEvent += Myclient_ConnectedEvent;
                    myclient.Connect(ip, 9000);

                }
                catch (Exception ex)
                {
                    textBox1.Text += ex.Message + "\n";
                    myclient.Close();

                    return;
                }
                textBox1.Text += "连接成功\n";
                button1.Text = "关闭连接";
            }
            else
            {
                myclient.Close();
            }
           
           
            

        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            if (myclient.Connected)
            {
                NetworkStream sendstream = myclient.GetStream();
                if (sendstream.CanWrite)
                {

               byte[] writeBytes =  Encoding.Unicode.GetBytes(textBox3.Text);

                    sendstream.Write(writeBytes, 0, writeBytes.Length);
                   
                }
                sendstream.Close();

            }


        }

        private void button3_Click(object sender, EventArgs e)
        {
            
            if (myclient.Connected)
            {
                MemoryStream mstream = new MemoryStream(1024);
                NetworkStream receive = myclient.GetStream();
                if (receive.CanRead)
                {
                    do
                    {
                        int receivelength = receive.Read(buffe, 0, buffe.Length);
                        mstream.Read(buffe, 0, receivelength);
                    } while (receive.DataAvailable)
               string message = Encoding.Unicode.GetString(mstream.GetBuffer());
                    textBox2.Text += message;
                    receive.Close();
                    mstream.Close();

                }
               
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            myclient.Close();
            Application.Exit();
        }

        private void Customer_FormClosing(object sender, FormClosingEventArgs e)
        {
            myclient.Close();
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
