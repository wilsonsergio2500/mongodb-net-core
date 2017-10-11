using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MC.Cache
{
    public class CacheProvider : ICacheProvider
    {
        private readonly IMemoryCache cache;
        public CacheProvider(IMemoryCache memoryCache)
        {
            cache = memoryCache;
        }

        public async Task<T> AddAsync<T>(string key, T item )
        {
            T element = await cache.GetOrCreateAsync<T>(key, entry =>
            {
                entry.SetSlidingExpiration(TimeSpan.FromMinutes(10)).SetAbsoluteExpiration(TimeSpan.FromHours(1));
                return Task.FromResult<T>(item);

            });

            return element;

        }

        public bool DoesKeyExist<T>(string key)
        {
            try
            {
                T item = cache.Get<T>(key);
                return item != null;
            } catch {
                return false;
            }
        }

        public T Get<T>(string Key) {
            T item = cache.Get<T>(Key);
            return item;
        }

        public void Set<T>(string key, T item) {
            MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions();
            cacheOptions.SetSlidingExpiration(TimeSpan.FromMinutes(10)).SetAbsoluteExpiration(TimeSpan.FromHours(1));

            cache.Set<T>(key, item, cacheOptions);

        }
    }
}
