using dns_console.DNS_message;
using dns_console.Enums;
using dns_console.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dns_console.DNS_cache
{
    internal class DNSCache : ICache<DNSQuestion, DNSMessage>
    {
        private MemoryCache _cache = new MemoryCache(new MemoryCacheOptions()); 

        public DNSMessage Get(DNSQuestion key)
        {
            DNSMessage entry;

            string strKey = key.QNAME + " " + ((DNSType)key.QTYPE).ToString() + " " + ((DNSClass)key.QCLASS).ToString();

            if (_cache.TryGetValue(strKey, out entry))
            {
                return entry;
            }

            return null;
        }

        public void Set(DNSQuestion key, DNSMessage value, uint ttl)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(DateTimeOffset.Now + TimeSpan.FromSeconds(ttl));

            string strKey = key.QNAME + " " + ((DNSType)key.QTYPE).ToString() + " " + ((DNSClass)key.QCLASS).ToString();

            _cache.Set(strKey, value, cacheEntryOptions);
        }
    }
}
