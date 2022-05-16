using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dns_console.Interfaces
{
    internal interface ICache<TKey, TValue>
    {
        public TValue Get(TKey key);

        public void Set(TKey key, TValue value, uint ttl);

    }
}
