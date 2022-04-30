﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dns_console.DNS_message
{
    internal class DNSMessage
    {
        private DNSHeader _header;

        private List<DNSQuestion> _questions;

        private List<DNSResourceRecord> _answers;

        public DNSMessage()
        {
            _header = new DNSHeader();
            _questions = new List<DNSQuestion>();
            _answers = new List<DNSResourceRecord>();
        }

        public DNSMessage(DNSHeader header, List<DNSQuestion> questions, List<DNSResourceRecord> answers)
        {
            _header = header;
            _questions = questions;
            _answers = answers;
        }


        public byte[] ToBytes()
        {
            List<byte> bytes = new List<byte>();

            bytes.AddRange(_header.ToBytes());
            _questions.ForEach(x => bytes.AddRange(x.ToBytes()));
            _answers.ForEach(x => bytes.AddRange(x.ToBytes()));

            return bytes.ToArray();
        }

        public void FromBytes(byte[] bytes)
        {
            byte[] header = new byte[12];

            Array.Copy(bytes, 0, header, 0, 12);

            _header.FromBytes(header);


            byte[] tail = new byte[bytes.Length - 12];

            Array.Copy(bytes, 12, tail, 0, tail.Length);


            DNSQuestion[] questions = new DNSQuestion[_header.QDCOUNT];

            int offset = 0;

            for (int i = 0; i < questions.Length; i++)
            {
                questions[i] = new DNSQuestion();

                questions[i].FromBytes(tail, offset);

                offset += questions[i].Size;
            }

            DNSResourceRecord[] answers = new DNSResourceRecord[_header.ANCOUNT];

            for (int i = 0; i < answers.Length; i++)
            {
                answers[i] = new DNSResourceRecord();

                answers[i].FromBytes(tail, offset);

                offset += answers[i].Size;
            }

            _questions = questions.ToList();
            _answers = answers.ToList();
        }
    }
}
