using System;
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

        public DNSMessage(DNSHeader header, List<DNSQuestion> questions, List<DNSResourceRecord> answers)
        {
            _header = header;
            _questions = questions;
            _answers = answers;
        }

    }
}
