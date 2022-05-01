using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using dns_console.DNS_message;

namespace dns_console.DNS_server
{
    internal class DNSServer
    {
        private Socket _listener;

        private IPEndPoint _endPoint;

        private EndPoint _epFrom;

        private byte[] _buffer;

        public readonly IPAddress IPAddress;

        public readonly int Port;


        public DNSServer(string ipAddress = "127.0.0.1", int port = 53)
        {
            IPAddress = IPAddress.Parse(ipAddress);

            Port = port;

            _buffer = new byte[512];

            _endPoint = new IPEndPoint(IPAddress, Port);

            _listener = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            _listener.Bind(_endPoint);
        }

        public void Start()
        {
            Console.WriteLine($"Server started on IP address: {IPAddress} on port: {Port}");

            while (true)
            {
                byte[] buffer = new byte[512];

                _listener.Receive(buffer);

                Console.WriteLine(System.Text.Encoding.UTF8.GetString(buffer));

                DNSMessage message = new DNSMessage();

                message.FromBytes(buffer);
            }
        }

        private void Recieve()
        {
            _listener.ReceiveFrom(_buffer, ref _epFrom);
        }
    }
}
