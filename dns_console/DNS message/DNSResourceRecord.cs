using dns_console.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


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

        public int Size => ToBytes().Length;
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

        public void FromBytes(byte[] bytes, int offset = 0)
        {

            FromBytesToSite(bytes, offset);

            byte[] type = new byte[2];
            byte[] Class = new byte[2];
            byte[] ttl = new byte[4];
            byte[] rdlength = new byte[2];

            var os = _nameByte.Count + offset;

            Array.Copy(bytes, os, type, 0, 2);
            Array.Copy(bytes, os + 2, Class, 0, 2);
            Array.Copy(bytes, os + 4, ttl, 0, 4);
            Array.Copy(bytes, os + 8, rdlength, 0, 2);

            TYPE = BitConverter.ToUInt16(type.Reverse().ToArray(), 0);
            CLASS = BitConverter.ToUInt16(Class.Reverse().ToArray(), 0);
            TTL = BitConverter.ToUInt32(ttl.Reverse().ToArray(), 0);
            RDLENGTH = BitConverter.ToUInt16(rdlength.Reverse().ToArray(), 0);

            RDATA = new byte[RDLENGTH];
            Array.Copy(bytes, os + 10, RDATA, 0, RDLENGTH);
        }

        public string DataToString()
        {
            if (TYPE == (ushort)DNSType.AAAA)
            {
                string hexStr = Convert.ToHexString(RDATA);

                for (int i = 4; i < hexStr.Length; i += 5)
                {
                    hexStr = hexStr.Insert(i, ":");
                }

                return hexStr;
            }

            return String.Join('.', RDATA);
        }

        private void FromBytesToSite(byte[] bytes, int offset)
        {
            int i = offset;

            List<string> site = new List<string>();

            List<byte> nameByte = new List<byte>(); 

            List<byte> nameByteCompressed = new List<byte>();

            while (true)
            {

                if ((bytes[i] & 192) == 192)
                {
                    byte[] arr = new byte[2];

                    Array.Copy(bytes, i, arr, 0, 2);

                    arr[0] = (byte)(bytes[i] & ~192);

                    var os = BitConverter.ToInt16(arr.Reverse().ToArray(), 0);

                    OffsetBytes(bytes, os, site);

                    byte[] b = new byte[2];

                    Array.Copy(bytes, i, b, 0, 2);

                    nameByteCompressed.AddRange(b);

                    break;
                }

                if (bytes[i] == 0)
                {
                    nameByte.Add(bytes[i]);

                    break;
                }

                var len = bytes[i++];

                nameByte.Add(len);

                byte[] domen = new byte[len];

                nameByte.AddRange(domen.ToList());

                Array.Copy(bytes, i, domen, 0, len);

                site.Add(Encoding.UTF8.GetString(domen));

                i += len;

            }

            NAME = String.Join(".", site.ToArray());

            _nameByte = new List<byte>();

            _nameByte.AddRange(nameByte);
            _nameByte.AddRange(nameByteCompressed);

        }

        private void OffsetBytes(byte[] bytes, int DNSOffset, List<string> site)
        {
            int i = DNSOffset;

            while (bytes[i] != 0)
            {

                if ((bytes[i] & 192) == 192)
                {
                    byte[] arr = new byte[2];

                    Array.Copy(bytes, i, arr, 0, 2);

                    arr[0] = (byte)(bytes[i] & ~192);

                    var os = BitConverter.ToInt16(arr.Reverse().ToArray(), 0);

                    OffsetBytes(bytes, os, site);

                    return;
                }

                var b = bytes[i++];

                byte[] domen = new byte[b];

                Array.Copy(bytes, i, domen, 0, b);

                site.Add(Encoding.UTF8.GetString(domen));

                i += b;
            }

        }
    }
}
