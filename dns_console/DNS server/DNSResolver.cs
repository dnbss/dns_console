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
        private DNSCache _cache;

        private Socket _socket;

        private IPEndPoint _endPoint;

        private EndPoint _epFrom;

        private Stack<DNSResourceRecord> _stack;

        private List<DNSResourceRecord> _answers;

        private DNSResourceRecord _root;

        public readonly int Port = 53;
        public DNSResolver(DNSCache cache, Socket socket)
        {
            _cache = cache;

            _socket = socket;

            _root = new DNSResourceRecord("", (ushort)DNSType.A, (ushort)DNSClass.IN
                , 0, 4, new byte[] { 198, 41, 0, 4 });

            _stack = new Stack<DNSResourceRecord>();

            _answers = new List<DNSResourceRecord>();

        }

        private DNSMessage GetFromCache(DNSMessage m) 
        {
            var ans = _cache.Get(m.Questions[0]);

            if (ans == null)
            {
                return null;
            }

            DNSMessage result = new DNSMessage();


            result.Questions = m.Questions;
            result.Answers = ans.Answers;
            result.Authoritys = ans.Authoritys;

            result.Header = m.Header;
            result.Header.QDCOUNT = (ushort)result.Questions.Length;
            result.Header.ANCOUNT = (ushort)result.Answers.Length;
            result.Header.NSCOUNT = (ushort)result.Authoritys.Length;

            return result;
        }

        private void SetToCache(DNSMessage m)
        {
            _cache.Set(m.Questions[0], m, m.Answers.ToList().Min(x => x.TTL));
        }

        private void SolveForNS(DNSMessage mes, DNSMessage m)
        {
            if (mes.Authoritys.Length != 0 && mes.Additionals.Length == 0)
            {
                var authority = mes.Authoritys[0];

                DNSQuestion q = new DNSQuestion();

                q.QCLASS = m.Questions[0].QCLASS;
                q.QTYPE = m.Questions[0].QTYPE;
                q.QNAME = String.Join(".", q.FromBytesToSite(authority.RDATA));

                DNSMessage me = new DNSMessage();
                me.Questions = new DNSQuestion[] { q };

                me.Header = m.Header;

                var ans = DFS(me);

                if (ans.Answers.Length != 0)
                {
                    _stack.Clear();

                    _stack.Push(new DNSResourceRecord("", ans.Answers[0].TYPE, (ushort)DNSClass.IN
                    , 0, ans.Answers[0].RDLENGTH, ans.Answers[0].RDATA));
                }
            }
        }

        private DNSMessage DFS(DNSMessage m) 
        {
            byte[] buffer = new byte[512];

            DNSResourceRecord record = new DNSResourceRecord();

            DNSMessage mes = new DNSMessage();

            var stack = new Stack<DNSResourceRecord>();

            stack.Push(_root);

            while (mes.Header.ANCOUNT == 0 && stack.Count > 0)
            {
                record = stack.Pop();

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

                Console.WriteLine($"++++++End Point {_endPoint.Address}+++++");
                mes.Dump();
                Console.WriteLine("++++++++++++++++++++");
                Console.WriteLine();

                foreach(var item in mes.Additionals)
                {
                    if (item.TYPE == (ushort)DNSType.A)
                    {
                        stack.Push(item);
                    }
                }

                if (mes.Authoritys.Length != 0 && mes.Additionals.Length == 0 && mes.Questions[0].QTYPE == (ushort)DNSType.A)
                {
                    var authority = mes.Authoritys[0];

                    DNSQuestion q = new DNSQuestion();

                    q.QCLASS = m.Questions[0].QCLASS;
                    q.QTYPE = m.Questions[0].QTYPE;
                    q.QNAME = String.Join(".", q.FromBytesToSite(authority.RDATA));

                    DNSMessage me = new DNSMessage();
                    me.Questions = new DNSQuestion[] { q };

                    me.Header = m.Header;

                    var ans = DFS(me);

                    if (ans.Answers.Length != 0)
                    {
                        stack.Clear();

                        stack.Push(new DNSResourceRecord("", ans.Answers[0].TYPE, (ushort)DNSClass.IN
                        , 0, ans.Answers[0].RDLENGTH, ans.Answers[0].RDATA));
                    }
                }

            }

            return mes;
        }

        public DNSMessage Solve(DNSMessage m)
        {
            /*var cachingData = GetFromCache(m);

            if (cachingData != null)
            {
                Console.WriteLine("******************\nData is taken from the cache\n*********************\n");

                return cachingData;
            }*/

            var question = m.Questions;

            if (!(question[0].QTYPE == (ushort)DNSType.A || question[0].QTYPE == (ushort)DNSType.AAAA))
            {
                return m;
            }

            var mes = DFS(m);

            DNSMessage result = new DNSMessage();
            
            result.Questions = m.Questions;
            result.Answers = mes.Answers;
            //result.Authoritys = mes.Authoritys;

            result.Header = m.Header;
            result.Header.QDCOUNT = (ushort)result.Questions.Length;
            result.Header.ANCOUNT = (ushort)result.Answers.Length;
            result.Header.NSCOUNT = (ushort)result.Authoritys.Length;

            //SetToCache(result);

            return result;
        }


    }
}
