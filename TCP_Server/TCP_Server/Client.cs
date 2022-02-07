using System;
using System.Text;
using System.Net;
using System.Linq;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Threading;
using System.Collections;

namespace TCP_Server
{
    class Client
    {
        private Socket clientSocket;
        private Thread t;
        private byte[] data = new byte[1024];
        public Client(Socket s )
        {
            clientSocket = s;

            t = new Thread(ReceiveMessage);
            t.Start();
        }
        void ReceiveMessage()
        {
       
            while(true)
            {
                if (clientSocket.Poll(10, SelectMode.SelectRead))
                {
                    clientSocket.Close();
                    break;
                }
                int length = clientSocket.Receive(data);
                string message = Encoding.UTF8.GetString(data , 0 , length);
                Program.BroadcastMessage(message);
                Console.WriteLine("收到消息: " + message);
            }
        }

        public void SendManage(string message)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            clientSocket.Send(data);
        }
        public bool Connected
        {
            get { return clientSocket.Connected; }
        
        }
    }
}
