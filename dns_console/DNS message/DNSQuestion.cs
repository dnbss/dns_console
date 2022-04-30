using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dns_console.DNS_message
{
    /*           Question section format
     *           
      0  1  2  3  4  5  6  7  8  9  0  1  2  3  4  5
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    |                                               |
    /                     QNAME                     /
    /                                               /
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    |                     QTYPE                     |
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    |                     QCLASS                    |
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
     */

    internal class DNSQuestion
    {
        private string _qname;

        private List<byte> _qnameByte = new List<byte>();

        private ushort _qtype;

        private List<byte> _qtypeByte = new List<byte>();

        private ushort _qclass;

        private List<byte> _qclassByte = new List<byte>();

        public int Size => ToBytes().Length;
        public string QNAME
        {
            get { return _qname; }

            set
            {
                
                _qname = value;

                var splits = _qname.Split('.');

                List<byte> bytes = new List<byte>();

                for (int i = 0; i < splits.Length; i++)
                {
                    bytes.Add((byte)splits[i].Length);

                    for (int j = 0; j < splits[i].Length; j++)
                    {
                        bytes.Add((byte)splits[i][j]);
                    }
                }

                if (splits.Last() != "")
                {
                    bytes.Add(0);
                }

                _qnameByte = bytes;
            }
        }
        public ushort QTYPE
        {
            get { return _qtype; }

            set
            {
                
                _qtype = value;

                byte[] bytes = BitConverter.GetBytes(value).Reverse().ToArray();

                _qtypeByte = bytes.ToList();
            }
        }
        public ushort QCLASS
        {
            get { return _qclass; }

            set
            {
                _qclass = value;

                byte[] bytes = BitConverter.GetBytes(_qclass).Reverse().ToArray();

                _qclassByte = bytes.ToList();
            }
        }

        public DNSQuestion()
        {
            
        }

        public DNSQuestion(string qname, ushort qtype, ushort qclass)
        {
            QNAME = qname;
            QTYPE = qtype;
            QCLASS = qclass;
        }

        public byte[] ToBytes()
        {
            List<byte> result = new List<byte>();

            result.AddRange(_qnameByte);
            result.AddRange(_qtypeByte);
            result.AddRange(_qclassByte);

            return result.ToArray();
        }

        // TODO: rewrite this method
        public void FromBytes(byte[] b, int offset = 0)
        {
            byte[] bytes = new byte[b.Length - offset];

            Array.Copy(b, offset, bytes, 0, bytes.Length);


            var site = FromBytesToSite(bytes);

            QNAME = String.Join(".", site.ToArray());


            byte[] t = new byte[4];

            Array.Copy(bytes, _qnameByte.Count, t, 0, 4);

            for (int i = t.Length - 4; i < t.Length; i += 2)
            {
                var h = t[i];

                t[i] = t[i + 1];

                t[i + 1] = h;
            }
            
            QTYPE = BitConverter.ToUInt16(t, 0);
            QCLASS = BitConverter.ToUInt16(t, 2);
        }

        private List<string> FromBytesToSite(byte[] bytes)
        {
            int i = 0;

            List<string> site = new List<string>();

            while (bytes[i] != 0)
            {

                var b = bytes[i++];

                byte[] domen = new byte[b];

                Array.Copy(bytes, i, domen, 0, b);

                site.Add(Encoding.UTF8.GetString(domen));

                i += b;
            }

            return site;
        }
    }
}
