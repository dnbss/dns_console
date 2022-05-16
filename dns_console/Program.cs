using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using dns_console.DNS_message;
using dns_console.DNS_server;
using dns_console.Interfaces;
using dns_console.DNS_cache;
using dns_console.Enums;

namespace dns_console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            socket.Bind(new IPEndPoint(IPAddress.Parse("192.168.4.144"), 53));

            EndPoint from = new IPEndPoint(IPAddress.Any, 0);

            DNSMessage message = new DNSMessage();

            DNSCache cache = new DNSCache();

            DNSResolver resolver = new DNSResolver(cache, socket);

            while (true)
            {
                byte[] buffer = new byte[512];

                Console.WriteLine("===Waiting...===");
                socket.ReceiveFrom(buffer, ref from);

                message.FromBytes(buffer);

                if (message.Questions[0].QTYPE == (ushort)DNSType.PTR)
                {
                    socket.SendTo(message.ToBytes(), from);

                    continue;
                }

                if (message.Questions[0].QTYPE == (ushort)DNSType.AAAA)
                {

                }

                Console.WriteLine("====Received====");
                message.Dump();
                Console.WriteLine("================");
                Console.WriteLine();

                var ans = resolver.Solve(message);

                socket.SendTo(ans.ToBytes(), from);

                Console.WriteLine("====Sending=====");
                ans.Dump();
                Console.WriteLine("================");
                Console.WriteLine();

                //ans.Answers.ToList().ForEach(answer => Console.WriteLine(answer.DataToString()));


            }

            /*IListener listener = new UdpListener();

            listener.Start();*/

            /*DNSCache cache = new DNSCache();

            byte[] data = new byte[]
            {
                3,
                119, 119, 119,
                2,
                121, 97,
                2,
                114, 117,
                0,
                0, 1,
                0, 1
            };*/

            /*DNSQuestion question = new DNSQuestion();

            byte[] data = new byte[]
            {
                0,
                0, 1,
                0, 1
            };

            question.FromBytes(data);

            byte[] k = question.ToBytes();*/

            //DNSResourceRecord record = new DNSResourceRecord();

            /*byte[] header = new byte[]
            {
                0, 1, 2, 3, 0, 2, 0, 2, 0, 0, 0, 0
            };

            byte[] question = new byte[]
            {
                3,
                119, 119, 119,
                2,
                121, 97,
                2,
                114, 117,
                0,
                0, 1,
                0, 1
            };

            byte[] record = new byte[]
            {
                3,
                119, 119, 119,
                2,
                121, 97,
                2,
                114, 117,
                0,
                0, 1,
                0, 1,
                0, 0, 0, 200,
                0, 4,
                64, 233, 163, 100
            };

            List<byte> mes = new List<byte>();
            mes.AddRange(header);
            mes.AddRange(question);
            mes.AddRange(question);
            mes.AddRange(record);
            mes.AddRange(record);

            DNSMessage message = new DNSMessage();
            message.FromBytes(mes.ToArray());*/
            //var t = message.ToBytes();



        }
    }
}
