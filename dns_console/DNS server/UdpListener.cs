using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using dns_console.DNS_message;
using dns_console.Interfaces;

namespace dns_console.DNS_server
{
    internal class UdpListener : IListener
    {
        private Socket _listener;

        private IPEndPoint _endPoint;

        private EndPoint _epFrom;

        private byte[] _buffer;

        public readonly IPAddress IPAddress;

        public readonly int Port;


        public UdpListener(string ipAddress = "127.0.0.1", int port = 53)
        {
            IPAddress = IPAddress.Parse(ipAddress);

            Port = port;

            _buffer = new byte[512];

            _endPoint = new IPEndPoint(IPAddress, Port);

            _epFrom = new IPEndPoint(IPAddress.Any, Port);

            _listener = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            _listener.Bind(_endPoint);

        }

        public void Start()
        {
            Console.WriteLine($"Server started on IP address: {IPAddress} on port: {Port}");

            DNSMessage message = new DNSMessage();

            while (true)
            {
                byte[] buffer = new byte[512];

                _listener.ReceiveFrom(buffer, ref _epFrom);

                message.FromBytes(buffer);

                _listener.SendTo(buffer, _epFrom);

                Console.WriteLine($"data sended to {_epFrom.ToString()}");
            }
        }

        public void Stop()
        {
            Console.WriteLine($"Server stoped");

            _listener.Close();
        }
    }
}
