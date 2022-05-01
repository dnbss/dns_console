using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            FromBytesHeader(bytes);


            byte[] tail = new byte[bytes.Length - 12];

            Array.Copy(bytes, 12, tail, 0, tail.Length);


            int offset = 0;

            FromBytesQuestionList(tail, ref offset);

            FromBytesAnswerList(tail, ref offset);
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
    }
}
