using System;
using System.Collections.Generic;
using dns_console.DNS_message;

namespace dns_console
{
    internal class Program
    {
        static void Main(string[] args)
        {
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

            DNSResourceRecord record = new DNSResourceRecord();

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
                0, 1,
                0, 0, 0, 200,
                0, 4,
                64, 233, 163, 100
            };

            record.FromBytes(data);

            var t = record.ToBytes();

            Console.WriteLine(record.DataToString());
        }
    }
}
