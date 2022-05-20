using System;
using System.Collections.Generic;
using System.Linq;


namespace dns_console.DNS_message
{
    /*           Format 
        +---------------------+
        |        Header       |
        +---------------------+
        |       Question      | the question for the name server
        +---------------------+
        |        Answer       | RRs answering the question
        +---------------------+
        |      Authority      | RRs pointing toward an authority
        +---------------------+
        |      Additional     | RRs holding additional information
        +---------------------+
     */

    internal class DNSMessage
    {
        private DNSHeader _header;

        private List<DNSQuestion> _questions;

        private List<DNSResourceRecord> _answers;

        private List<DNSResourceRecord> _authoritys;

        private List<DNSResourceRecord> _additionals;

        public DNSHeader Header
        {
            get { return _header; }
            set { _header = value; }
        }

        public DNSQuestion[] Questions
        {
            get { return _questions.ToArray(); }
            set { _questions = value.ToList(); }
        }

        public DNSResourceRecord[] Answers
        {
            get { return _answers.ToArray(); }
            set { _answers = value.ToList(); }
        }

        public DNSResourceRecord[] Authoritys
        {
            get { return _authoritys.ToArray(); }
            set { _authoritys = value.ToList(); }
        }

        public DNSResourceRecord[] Additionals
        {
            get { return _additionals.ToArray(); }
            set { _additionals = value.ToList(); }
        }

        public DNSMessage()
        {
            _header = new DNSHeader();
            _questions = new List<DNSQuestion>();
            _answers = new List<DNSResourceRecord>();
            _authoritys = new List<DNSResourceRecord>();
            _additionals = new List<DNSResourceRecord>();
        }

        public DNSMessage(DNSHeader header, List<DNSQuestion> questions
            , List<DNSResourceRecord> answers, List<DNSResourceRecord> authoritys, List<DNSResourceRecord> additionals)
        {
            _header = header;
            _questions = questions;
            _answers = answers;
            _authoritys = authoritys;
            _additionals = additionals;
        }


        public void Dump()
        {
            Console.WriteLine("Questions: ");
            _questions.ToList().ForEach(_ => Console.WriteLine($"QNAME: {_.QNAME}   QTYPE: {_.QTYPE}    QCLASS: {_.QCLASS}"));

            Console.WriteLine("Answers: ");
            _answers.ToList().ForEach(_ => Console.WriteLine($"CLASS: {_.CLASS}     TYPE: {_.TYPE}      DATA: {_.DataToString()}"));

            Console.WriteLine("Authoritys:");
            _authoritys.ToList().ForEach(_ => Console.WriteLine($"CLASS: {_.CLASS}     TYPE: {_.TYPE}      DATA: {_.DataToString()}"));

            Console.WriteLine("Additionals:");
            _additionals.ToList().ForEach(_ => Console.WriteLine($"CLASS: {_.CLASS}     TYPE: {_.TYPE}      DATA: {_.DataToString()}"));
        }

        public byte[] ToBytes()
        {
            List<byte> bytes = new List<byte>();

            bytes.AddRange(_header.ToBytes());

            _questions.ForEach(x => bytes.AddRange(x.ToBytes()));
            _answers.ForEach(x => bytes.AddRange(x.ToBytes()));
            _authoritys.ForEach(x => bytes.AddRange(x.ToBytes()));
            _additionals.ForEach(x => bytes.AddRange(x.ToBytes()));

            return bytes.ToArray();
        }

        public void FromBytes(byte[] bytes)
        {

            FromBytesHeader(bytes);


            /*byte[] tail = new byte[bytes.Length - 12];

            Array.Copy(bytes, 12, tail, 0, tail.Length);*/


            int offset = 12;

            FromBytesQuestionList(bytes, ref offset);

            FromBytesAnswerList(bytes, ref offset);

            FromBytesAuthorityList(bytes, ref offset);

            FromBytesAdditionalList(bytes, ref offset);
        }

        private void FromBytesHeader(byte[] bytes)
        {
            byte[] header = new byte[12];

            Array.Copy(bytes, 0, header, 0, 12);

            _header.FromBytes(header);
        }

        private void FromBytesQuestionList(byte[] bytes, ref int offset)
        {
            DNSQuestion[] questions = new DNSQuestion[_header.QDCOUNT];

            for (int i = 0; i < questions.Length; i++)
            {
                questions[i] = new DNSQuestion();

                questions[i].FromBytes(bytes, offset);

                offset += questions[i].Size;
            }

            _questions = questions.ToList();
        }

        private void FromBytesAnswerList(byte[] bytes, ref int offset)
        {
            DNSResourceRecord[] answers = new DNSResourceRecord[_header.ANCOUNT];

            for (int i = 0; i < answers.Length; i++)
            {
                answers[i] = new DNSResourceRecord();

                answers[i].FromBytes(bytes, offset);

                offset += answers[i].Size;
            }

            _answers = answers.ToList();
        }

        private void FromBytesAuthorityList(byte[] bytes, ref int offset)
        {
            DNSResourceRecord[] authoritys = new DNSResourceRecord[_header.NSCOUNT];

            for (int i = 0; i < authoritys.Length; i++)
            {
                authoritys[i] = new DNSResourceRecord();

                authoritys[i].FromBytes(bytes, offset);

                offset += authoritys[i].Size;
            }

            _authoritys = authoritys.ToList();
        }

        private void FromBytesAdditionalList(byte[] bytes, ref int offset)
        {
            DNSResourceRecord[] additionals = new DNSResourceRecord[_header.ARCOUNT];

            for (int i = 0; i < additionals.Length; i++)
            {
                additionals[i] = new DNSResourceRecord();

                additionals[i].FromBytes(bytes, offset);

                offset += additionals[i].Size;
            }

            _additionals = additionals.ToList();
        }
    }
}
