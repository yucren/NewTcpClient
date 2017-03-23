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

namespace Server
{
    public partial class Server : Form
    {
        TcpListener myServer = new TcpListener(9000);
        byte[] buffer = new byte[8192];
        public Server()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            myServer.Start();
            TcpClient newCustomer = myServer.AcceptTcpClient();

            NetworkStream recevice = newCustomer.GetStream();
            int length = recevice.Read(buffer, 0, buffer.Length);
            textBox1.Text = Encoding.Unicode.GetString(buffer, 0, length);
                  }

        private void Server_FormClosing(object sender, FormClosingEventArgs e)
        {
            myServer.Stop();
            myServer = null;
        }
    }
}
