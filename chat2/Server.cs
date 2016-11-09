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
    class Server
    {
        TcpListener tcpl;
        TcpClient client;
        ClientAccept[] clientA;
        int N = 0;
        int Max = 10;
        bool stopped = true;
        //int port;
        public Server(IPAddress ip, int port)
        {
            
            try
            {
                tcpl = new TcpListener(ip, port);
                tcpl.Start();
                stopped = false;
                Thread clientThread = new Thread(new ThreadStart(Start));
                clientThread.Start();
                clientA = new ClientAccept[Max];
                //Start();
            }
            catch
            {
                MessageBox.Show("Не стартанул( /n Не слышит(");   
            }
        }

        public void Start()
        {
            
            while (!stopped || N==10)
            {
                try
                {
                    client = tcpl.AcceptTcpClient();
                    clientA[N] = new ClientAccept(client);
                    Thread clientThread = new Thread(new ThreadStart(clientA[N++].DoIt));
                    clientThread.Start();
                    //clientA[N++].DoIt();
                }
                catch
                {
                    if (tcpl != null) tcpl.Stop();
                    MessageBox.Show("Клиент оторвался");
                }
            }
            
        }

        public void Stop()
        {
            stopped = true;
            if (tcpl != null)
                tcpl.Stop();
            for (int i=0;i<N;i++)
            clientA[i].Close();
        }

    }
}
