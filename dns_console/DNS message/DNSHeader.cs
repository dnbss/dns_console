using System;
using System.Linq;


namespace dns_console.DNS_message
{

    /*            Header section format
      0  1  2  3  4  5  6  7  8  9  0  1  2  3  4  5
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    |                      ID                       |
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    |QR|   Opcode  |AA|TC|RD|RA|   Z    |   RCODE   |
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    |                    QDCOUNT                    |
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    |                    ANCOUNT                    |
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    |                    NSCOUNT                    |
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    |                    ARCOUNT                    |
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+ 
    */
    internal class DNSHeader
    {
        private byte[] _header = new byte[12];

        private ushort _id;

        private ushort _flags;

        private ushort _qdcount;

        private ushort _ancount;

        private ushort _nscount;

        private ushort _arcount;

        public ushort ID 
        {
            get { return _id; }

            set
            {
                _id = value;

                byte[] bytes = BitConverter.GetBytes(value).Reverse().ToArray();

                bytes.CopyTo(_header, 0);
            }
        }

        public ushort Flags
        {
            get { return _flags; }

            set
            {
                _flags = value;

                byte[] bytes = BitConverter.GetBytes(Flags).Reverse().ToArray();

                bytes.CopyTo(_header, 2);
            }
        }

        public bool QR
        {
            get { return (Flags & 0x8000) == 0x8000; }

            set
            {
                if (value)
                {
                    Flags |= 0x8000;
                }
                else
                {
                    Flags = (ushort)(Flags & ~0x8000);
                }
            }
        }

        public byte OPCODE
        {
            get
            {
                return (byte)((Flags & 0x7800) >> 11);
            } 

            set
            {
                Flags = (ushort)((Flags & ~0x7800) | (value << 11));    
            }
        }

        public bool AA
        {
            get { return (Flags & 0x0400) == 0x0400; }

            set
            {
                if (value)
                {
                    Flags |= 0x0400;
                }
                else
                {
                    Flags = (ushort)(Flags & ~0x0400);
                }
            }
        }

        public bool TC
        {
            get { return (Flags & 0x0200) == 0x0200; }

            set
            {
                if (value)
                {
                    Flags |= 0x0200;
                }
                else
                {
                    Flags = (ushort)(Flags & ~0x0200);
                }
            }
        }

        public bool RD
        {
            get { return (Flags & 0x0100) == 0x0100; }

            set
            {
                if (value)
                {
                    Flags |= 0x0100;
                }
                else
                {
                    Flags = (ushort)(Flags & ~0x0100);
                }
            }
        }

        public bool RA
        {
            get { return (Flags & 0x0080) == 0x0080; }

            set
            {
                if (value)
                {
                    Flags |= 0x0080;
                }
                else
                {
                    Flags = (ushort)(Flags & ~0x0080);
                }
            }
        }

        public byte Z
        {
            get
            {
                return (byte)((Flags & 0x0070) >> 4);
            }

            set
            {
                Flags = (ushort)((Flags & ~0x0070) | (value << 4));
            }
        }

        public byte RCODE
        {
            get
            {
                return (byte)(Flags & 0x000F);
            }

            set
            {
                Flags = (ushort)((Flags & ~0x000F) | value);
            }
        }

        public ushort QDCOUNT
        {
            get { return _qdcount; }

            set
            {
                _qdcount = value;

                byte[] bytes = BitConverter.GetBytes(value).Reverse().ToArray();

                bytes.CopyTo(_header, 4);
            }
        }
        
        public ushort ANCOUNT
        {
            get { return _ancount; }

            set
            {
                _ancount = value;

                byte[] bytes = BitConverter.GetBytes(value).Reverse().ToArray();

                bytes.CopyTo(_header, 6);
            }
        }

        public ushort NSCOUNT
        {
            get { return _nscount; }

            set
            {
                _nscount = value;

                byte[] bytes = BitConverter.GetBytes(value).Reverse().ToArray();

                bytes.CopyTo(_header, 8);
            }
        }

        public ushort ARCOUNT
        {
            get { return _arcount; }

            set
            {
                _arcount = value;

                byte[] bytes = BitConverter.GetBytes(value).Reverse().ToArray();

                bytes.CopyTo(_header, 10);
            }
        }

        public DNSHeader()
        {

        }

        public DNSHeader(ushort id, bool qr, byte opcode
            , bool aa, bool tc, bool rd
            , bool ra, byte z, byte rcode
            , byte qdcount, byte ancount, byte nscount, byte arcount)
        {
            ID = id;
            QR = qr;
            OPCODE = opcode;
            AA = aa;
            TC = tc;
            RD = rd;
            RA = ra;
            Z = z;
            RCODE = rcode;
            QDCOUNT = qdcount;
            ANCOUNT = ancount;
            NSCOUNT = nscount;
            ARCOUNT = arcount;
        }

        public DNSHeader(ushort id, ushort flags, ushort qdcount
            , ushort ancount, ushort nscount, ushort arcount)
        {
            ID = id;
            Flags = flags;
            QDCOUNT = qdcount;
            ANCOUNT = ancount;
            NSCOUNT = nscount;
            ARCOUNT = arcount;
        }

        public byte[] ToBytes()
        {
            /*byte[] t = new byte[12];

            _header.CopyTo(t, 0);

            for (int i = 0; i < t.Length; i += 2)
            {
                var h = t[i];

                t[i] = t[i + 1];

                t[i + 1] = h;
            }

            return t;*/

            return _header;
        }

        
        // TODO: rewrite this shit
        public void FromBytes(byte[] bytes, int offset = 0)
        {
            if (bytes.Length < 12 + offset) throw new Exception("Incorrect bytes length!");

            byte[] t = new byte[12];

            bytes.CopyTo(t, 0);

            for (int i = offset; i < t.Length + offset; i += 2)
            {
                var h = t[i];

                t[i] = t[i + 1];

                t[i + 1] = h;
            }

            Buffer.BlockCopy(bytes, 0, _header, 0, 12);

            ID = BitConverter.ToUInt16(t, 0);
            Flags = BitConverter.ToUInt16(t, 2);
            QDCOUNT = BitConverter.ToUInt16(t, 4);
            ANCOUNT = BitConverter.ToUInt16(t, 6);
            NSCOUNT = BitConverter.ToUInt16(t, 8);
            ARCOUNT = BitConverter.ToUInt16(t, 10);
        }
    }
}
