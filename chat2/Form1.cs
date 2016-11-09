using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace chat2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        bool serverok = false;
        Server s;

        private void button1_Click(object sender, EventArgs e)
        {
            if (!serverok)
            {
                serverok = true;
                button1.Text = "Выкл";
                IPAddress ip = IPAddress.Any;
                //IPAddress ip;
                //IPAddress.TryParse("127.0.0.1", out ip);
                s = new Server(ip,80);
                //s.Start();
            }
            else
            {
                serverok = false;
                button1.Text = "Вкл";
                s.Stop();
                s = null;
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        Client client;
        private void button2_Click(object sender, EventArgs e)
        {
            if (client!=null)
            client.DoYou(textBox1.Text);
            

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (client==null)
            {
                client = new Client(textBox2.Text, 80, textBox3.Text);
                if (client.clientok)
                    button3.Text = "Выкл клиент";
                else client = null;
            }
            else
            {
                client.Close();
                client = null;
                button3.Text = "Вкл клиент";
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (client != null)
            {
                client.Close();client = null;
            }
            if (s != null)
            {
                s.Stop();
                s = null;
            }
            

        }
    }
}
