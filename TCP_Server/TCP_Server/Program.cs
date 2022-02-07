using System;
using System.Text;
using System.Net;
using System.Linq;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace TCP_Server
{
    class Program
    {
        static List<Client> clientList = new List<Client>();
        public static void BroadcastMessage(string message)
        {
            var notConnectedList = new List<Client>();
            foreach(var client in clientList)
            {  
                if(client.Connected)
                    client.SendManage(message);
                else
                {
                    notConnectedList.Add(client);
                }
            }
            foreach (var temp in notConnectedList)
            {
                clientList.Remove(temp);
            }
        }
        static void Main(string[] args)
        {
            Socket tcpServer = new Socket(AddressFamily.InterNetwork , SocketType.Stream , ProtocolType.Tcp);
            tcpServer.Bind(new IPEndPoint(IPAddress.Parse("172.20.10.2"), 7878));
            tcpServer.Listen(100);
            Console.WriteLine("server running...");

            while(true)
            {
                Socket clientSocket = tcpServer.Accept();
                Console.WriteLine("a client is connected ! ");
                Client client = new Client(clientSocket);

                clientList.Add(client);
            }

        }
    }
}
