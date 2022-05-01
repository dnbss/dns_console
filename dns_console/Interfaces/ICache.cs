using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dns_console.Interfaces
{
    internal interface ICache
    {
        public byte[] Get(string key);

        public void Set(byte[] bytes, string key, int ttl);


    }
}
