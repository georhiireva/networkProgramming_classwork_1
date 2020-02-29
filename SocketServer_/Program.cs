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

namespace SocketServer_
{
    class Program
    {
        private const int Port = 7081;
        static void Main(string[] args) => new Program().Run();

        private void Run()
        {
            Socket listening = new Socket(
                AddressFamily.InterNetwork, // простарнство адресов = интерент (v4)
                SocketType.Stream, // поточный сокет = TCP
                ProtocolType.IP); // протокол = Ip(v4)

            listening.Bind( // связывает сокет с ip  и портом
                new IPEndPoint(
                    IPAddress.Parse("192.168.1.92"), Port
                    ));

            listening.Listen(5); // начинаем слушать

            while (true)
            {
                Socket client = listening.Accept(); //принять входящее соединение
                Console.WriteLine($"Подключился клиент с айпи: {client.RemoteEndPoint}");
                DateTime now = DateTime.Now;
                //Преобразовать дату и время в массив байтов
                byte[] buffer = DateTimeToBytes(now);
               // client.Send(BitConverter.GetBytes(buffer.Length));
                client.Send(buffer);
                client.Shutdown(SocketShutdown.Both);
                client.Dispose();
            }
            // listening.Shutdown(SocketShutdown.Both);
            // listening.Dispose();
        }

        // В виде сериализации
        private byte[] DateTimeToBytes(DateTime dateTime)
        {
            IFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            formatter.Serialize(stream, dateTime);
            return stream.GetBuffer();
        }

        // Это более легковесный (?) способ в виде строки
        private byte[] DateTimeToString (DateTime dateTime)
        {
            string asString = $"{dateTime.ToShortDateString()} {dateTime.ToShortTimeString()}";
            byte[] bytes = Encoding.Unicode.GetBytes(asString);
            return bytes;
        }

    }
}
