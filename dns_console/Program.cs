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
            DNSServer server = new DNSServer();

            Console.WriteLine("Enter your root dns server. By default, the Verisign, Inc. server is used (198.41.0.4):");
            string ip = Console.ReadLine();

            server.Start(ip == "" ? "198.41.0.4" : ip);
        }
    }
}
