using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using dns_console.DNS_message;
using dns_console.DNS_server;
using dns_console.Interfaces;

namespace dns_console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IListener listener = new UdpListener();

            listener.Start();


            /*DNSQuestion question = new DNSQuestion();

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
