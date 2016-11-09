using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace chat2
{
    class ClientAccept
    {
        TcpClient client;
        NetworkStream ns = null;
        bool ok = true;
        public ClientAccept(TcpClient tcp)
        {
            client = tcp;
        }

        public void DoIt()
        {
            try
            {
                ns = client.GetStream();
                byte[] data = new byte[64];
                while (ok)
                {
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
                    data = Encoding.Unicode.GetBytes(message);
                    ns.Write(data, 0, data.Length);
                }
            }
            catch
            {
                if (ns != null) ns.Close();
                if (client != null) client.Close();
                //MessageBox.Show("Не дуит");
            }

        }

        public void Close()
        {
            ok = false;
            client.Close();
        }
    }
}
