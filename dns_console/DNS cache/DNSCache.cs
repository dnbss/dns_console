using dns_console.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dns_console.DNS_cache
{
    internal class DNSCache : ICache
    {
        private MemoryCache _cache = new MemoryCache(new MemoryCacheOptions()); 

        private List<string> _keys = new List<string>();

        public string[] Get(string key)
        {
            string[] entry;

            if (_cache.TryGetValue(key, out entry))
            {
                return entry;
            }

            return null;
        }

        public void Set(string[] bytes, string key, int ttl)
        {
            _keys.Add(key);

            var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(DateTimeOffset.Now + TimeSpan.FromSeconds(ttl));

            _cache.Set(key, bytes, cacheEntryOptions);
        }
    }
}
