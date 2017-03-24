using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;


namespace Server
{
    
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            
            bool CreatedNew;
            String ip = "192.168.0.188";
            bool iptf = false;
            IPAddress[] adds = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (var add in adds)
            {
                if (add.ToString()=="192.168.0.188")
                {
                    iptf = true;
                }             
            }
            Mutex mutex = new Mutex(true, "SingleWinAPPMutex", out CreatedNew);
            //if (!iptf)
            //{
            //    MessageBox.Show("请在服务器上启动");
            //    Application.Exit();
            //    return;
            //}
            if (!CreatedNew)
            {
                MessageBox.Show("只能启动一个实例");
                Application.Exit();
                return;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Server());
        }
    }
}
