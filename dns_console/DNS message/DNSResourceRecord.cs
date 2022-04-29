using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dns_console.DNS_message
{
    /*            Resource record format
       0  1  2  3  4  5  6  7  8  9  0  1  2  3  4  5
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    |                                               |
    /                                               /
    /                      NAME                     /
    |                                               |
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    |                      TYPE                     |
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    |                     CLASS                     |
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    |                      TTL                      |
    |                                               |
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    |                   RDLENGTH                    |
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--|
    /                     RDATA                     /
    /                                               /
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    */
    internal class DNSResourceRecord
    {
        private string _name;

        private List<byte> _nameByte = new List<byte>();

        private ushort _type;

        private List<byte> _typeByte = new List<byte>();

        private ushort _class;

        private List<byte> _classByte = new List<byte>();

        private uint _ttl;

        private List<byte> _ttlByte = new List<byte>();

        private ushort _rdlength;

        private List<byte> _rdlengthByte = new List<byte>();

        private byte[] _rdata;

        public string NAME
        {
            get { return _name; }

            set
            {
                _name = value;

                var splits = _name.Split('.');

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

                _nameByte = bytes;
            }
        }

        public ushort TYPE
        {
            get { return _type; }

            set
            {

                _type = value;

                byte[] bytes = BitConverter.GetBytes(value).Reverse().ToArray();

                _typeByte = bytes.ToList();
            }
        }

        public ushort CLASS
        {
            get { return _class; }

            set
            {
                _class = value;

                byte[] bytes = BitConverter.GetBytes(_class).Reverse().ToArray();

                _classByte = bytes.ToList();
            }
        }

        public uint TTL
        {
            get { return _ttl; }

            set
            {
                _ttl = value;

                byte[] bytes = BitConverter.GetBytes(_ttl).Reverse().ToArray();

                _ttlByte = bytes.ToList();
            }
        }

        public ushort RDLENGTH
        {
            get { return _rdlength; }

            set
            {
                _rdlength = value;

                byte[] bytes = BitConverter.GetBytes(_rdlength).Reverse().ToArray();

                _rdlengthByte = bytes.ToList();
            }
        }

        public byte[] RDATA
        {
            get { return _rdata; }

            set
            {
                _rdata = new byte[_rdlength];

                value.CopyTo(_rdata, 0);
            }
        }

        public DNSResourceRecord()
        {

        }

        public DNSResourceRecord(string name, ushort type, ushort Class
            , uint ttl, ushort rdlength, byte[] data)
        {
            NAME = name;
            TYPE = type;
            CLASS = Class;
            TTL = ttl;
            RDLENGTH = rdlength;
            RDATA = data;
        }

        public byte[] ToBytes()
        {
            List<byte> result = new List<byte>();

            result.AddRange(_nameByte);
            result.AddRange(_typeByte);
            result.AddRange(_classByte);
            result.AddRange(_ttlByte);
            result.AddRange(_rdlengthByte);
            result.AddRange(_rdata);

            return result.ToArray();
        }

        public void FromBytes(byte[] bytes)
        {
            var site = FromBytesToSite(bytes);

            NAME = String.Join(".", site.ToArray());


            byte[] t = new byte[10];

            Array.Copy(bytes, _nameByte.Count, t, 0, 10);

            for (int i = t.Length - 4; i < t.Length; i += 2)
            {
                var h = t[i];

                t[i] = t[i + 1];

                t[i + 1] = h;
            }


            TYPE = BitConverter.ToUInt16(t, 0);
            CLASS = BitConverter.ToUInt16(t, 2);
            TTL = BitConverter.ToUInt32(t, 4);
            RDLENGTH = BitConverter.ToUInt16(t, 8);
            bytes.CopyTo(RDATA, 10);
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
