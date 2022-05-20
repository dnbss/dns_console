namespace dns_console.Interfaces
{
    internal interface ICache<TKey, TValue>
    {
        public TValue Get(TKey key);

        public void Set(TKey key, TValue value, uint ttl);

    }
}
