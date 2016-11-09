using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Threading;

namespace chat2
{
    class Client
    {
        //int port;
        string name;
        TcpClient tcp;
        NetworkStream ns = null;
        public bool clientok=false;
        public Client(string ip, int port, string Name)
        {
            try
            {
                tcp = new TcpClient(ip, port);
                ns = tcp.GetStream();
                name = Name;
                clientok = true;
            }
            catch
            {
                MessageBox.Show("Не подключился...");
            }
        }

        public void DoYou(string message)
        {
            try
            {
                if (clientok)
                {
                    message = name + ":" + message;
                    byte[] data = Encoding.Unicode.GetBytes(message);
                    ns.Write(data, 0, data.Length);
                    //ответ
                    Thread clientThread = new Thread(new ThreadStart(Listen));
                    clientThread.Start();
                }
            }

            catch
            {
                if (ns != null) ns.Close();
                MessageBox.Show("НеДую!");
            }
        }

        void Listen()
        {
            try
            {
                byte[] data = new byte[64];
                StringBuilder sb = new StringBuilder();
                int length = 0;
                do
                {
                    length = ns.Read(data, 0, data.Length);
                    sb.Append(Encoding.Unicode.GetString(data, 0, length));
                } while (ns.DataAvailable);
                string message = sb.ToString();

                //// отправить сообщение в интерфейс
                var item = new NotifyIcon();
                item.Visible = true;
                item.Icon = System.Drawing.SystemIcons.Information;
                string[] info = message.Split(':');
                item.ShowBalloonTip(10000, "Сообщение от " + info[0], info[1], ToolTipIcon.Info);
                ////
            }
            catch
            {
                MessageBox.Show("Не слушаю!");
            }
        }
        public void Close()
        {
            tcp.Close();
            clientok = false;
        }
    }
}
