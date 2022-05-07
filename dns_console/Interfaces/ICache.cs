using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dns_console.Interfaces
{
    internal interface ICache
    {
        public string[] Get(string key);

        public void Set(string[] bytes, string key, int ttl);

    }
}
