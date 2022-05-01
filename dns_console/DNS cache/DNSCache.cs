using dns_console.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dns_console.DNS_cache
{
    internal class DNSCache : ICache
    {
        public byte[] Get(string key)
        {
            throw new NotImplementedException();
        }

        public void Set(byte[] bytes, string key, int ttl)
        {
            throw new NotImplementedException();
        }
    }
}
