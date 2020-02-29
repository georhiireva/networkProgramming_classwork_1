using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace SocketClient_
{
    class Program
    {
        private const int Port = 7081;
        static void Main(string[] args) => new Program().Run();

        private void Run()
        {
            while (true)
            {
                Socket server = new Socket(
                    AddressFamily.InterNetwork, // простарнство адресов = интерент (v4)
                    SocketType.Stream, // поточный сокет = TCP
                    ProtocolType.IP // протокол = Ip(v4)
                    );
                //Console.Write("Введите имя сервера: ");
                string serverDns = "192.168.1.204";//Console.ReadLine();
                server.Connect(
                    new IPEndPoint(
                        Dns.GetHostAddresses(serverDns).First(),
                        Port
                        ));
                byte[] bytes = new byte[1024];

                int receivedCount = server.Receive(bytes);


                DateTime serverTime = BytesToDateTime(bytes, receivedCount);
                Console.WriteLine($"Server time: {serverTime.ToShortDateString()} {serverTime.ToShortTimeString()}");
                server.Shutdown(SocketShutdown.Both);
                server.Dispose();
            }
            Console.ReadKey();
        }

        private DateTime BytesToDateTime(byte[] bytes, int receivedCount)
        {
            MemoryStream stream = new MemoryStream(bytes, 0, receivedCount);
            IFormatter formatter = new BinaryFormatter();
            return (DateTime)formatter.Deserialize(stream);
        }
    }
}
