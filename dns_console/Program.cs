using System;
using dns_console.DNS_server;


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
