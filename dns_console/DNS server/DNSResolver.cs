using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dns_console.DNS_cache;
using dns_console.Interfaces;
using dns_console.DNS_message;
using System.Net.Sockets;
using System.Net;
using dns_console.Enums;

namespace dns_console.DNS_server
{
    internal class DNSResolver
    {
        private ICache _cache;

        private Socket _socket;

        private IPEndPoint _endPoint;

        private EndPoint _epFrom;

        private Stack<DNSResourceRecord> _stack;

        private List<DNSResourceRecord> _answers;

        private DNSResourceRecord _root;

        public readonly int Port = 53;
        public DNSResolver(ICache cache, Socket socket)
        {
            _cache = cache;

            _socket = socket;

            _root = new DNSResourceRecord("", (ushort)DNSType.A, (ushort)DNSClass.IN
                , 0, 4, new byte[] { 198, 41, 0, 4 });

            _stack = new Stack<DNSResourceRecord>();

            _answers = new List<DNSResourceRecord>();

        }

        public DNSMessage Solve(DNSMessage m)
        {
            var question = m.Questions;

            byte[] buffer = new byte[512];

            DNSResourceRecord record = new DNSResourceRecord();

            DNSMessage mes = new DNSMessage();

            _stack.Clear();

            _stack.Push(_root);

            while (mes.Header.ANCOUNT == 0 && _stack.Count > 0 && _stack.Peek().NAME != question[0].QNAME)
            {
                record = _stack.Pop();

                if (record.TYPE != (ushort)DNSType.A)
                {
                    continue;
                }

                _endPoint = new IPEndPoint(IPAddress.Parse(record.DataToString()), Port);

                _socket.SendTo(m.ToBytes(), _endPoint);

                var length = _socket.Receive(buffer);

                byte[] cropBuffer = new byte[length];

                Array.Copy(buffer, cropBuffer, length);

                mes.FromBytes(cropBuffer);

                foreach (var item in mes.Additionals)
                {
                    _stack.Push(item);
                }
            }

            DNSMessage result = new DNSMessage();

            
            result.Questions = m.Questions;
            result.Answers = mes.Answers;
            result.Authoritys = mes.Authoritys;

            result.Header = m.Header;
            result.Header.QDCOUNT = (ushort)result.Questions.Length;
            result.Header.ANCOUNT = (ushort)result.Answers.Length;
            result.Header.NSCOUNT = (ushort)result.Authoritys.Length;

            return result;
        }
    }
}
