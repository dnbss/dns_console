using dns_console.DNS_cache;
using dns_console.DNS_message;
using dns_console.Enums;
using dns_console.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace dns_console.DNS_server
{
    internal class DNSServer
    {
        private Socket _socket;

        private int _port = 53;

        private DNSResolver _resolver;

        private ICache<DNSQuestion, DNSMessage> _cache;

        public DNSServer()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            _socket.Bind(new IPEndPoint(IPAddress.Any, _port));

            _cache = new DNSCache();
        }

        public void Start(string s = "198.41.0.4")
        {
            var ss = Dns.GetHostEntry(Dns.GetHostName()).AddressList.Last();

            Console.WriteLine($"Server has started on IP: {ss}, port: 53");

            _resolver = new DNSResolver(_cache, _socket, IPAddress.Parse(s).GetAddressBytes());

            EndPoint from = new IPEndPoint(IPAddress.Any, 0);

            DNSMessage message = new DNSMessage();

            while (true)
            {

                byte[] buffer = new byte[512];

                Console.WriteLine("===Waiting...===");
                _socket.ReceiveFrom(buffer, ref from);

                message.FromBytes(buffer);

                if (message.Questions[0].QTYPE == (ushort)DNSType.PTR)
                {
                    _socket.SendTo(message.ToBytes(), from);

                    continue;
                }

                var ans = _resolver.Solve(message);

                _socket.SendTo(ans.ToBytes(), from);

                Console.WriteLine("====Received====");
                message.Dump();
                Console.WriteLine("================");
                Console.WriteLine();

                Console.WriteLine("====Sending=====");
                ans.Dump();
                Console.WriteLine("================");
                Console.WriteLine();
            }
        }
    }
}
