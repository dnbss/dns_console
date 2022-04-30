﻿using System;
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

            //DNSResourceRecord record = new DNSResourceRecord();

            byte[] header = new byte[]
            {
                0, 1, 2, 3, 0, 1, 0, 1, 0, 0, 0, 0
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
            mes.AddRange(record);

            DNSMessage message = new DNSMessage();
            message.FromBytes(mes.ToArray());
        }
    }
}
