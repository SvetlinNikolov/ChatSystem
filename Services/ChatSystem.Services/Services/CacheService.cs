namespace ShishaProject.Common.Caching
{
    using System;
    using System.Collections.Concurrent;
    using ChatSystem.Services.Constants;
    using ChatSystem.Services.Services.Contracts;
    using Microsoft.Extensions.Caching.Memory;

    public class CacheService : ICacheService
    {
        private readonly IMemoryCache memoryCache;
        private readonly TimeSpan CACHING_TIME = CacheConstants.GlobalCachingTime;

        public CacheService(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        public T Get<T>(string key)
        {
            if (!string.IsNullOrEmpty(key) && this.memoryCache.TryGetValue<ConcurrentDictionary<string, T>>(key, out var collection))
            {
                return collection[key];
            }

            return default(T);
        }

        public bool TryGet<T>(string key, out T value)
        {
            value = this.Get<T>(key);

            return value != null && !value.Equals(default(T));
        }

        public void SetOrUpdate<T>(string key, T value)
        {
            var collection = this.memoryCache.GetOrCreate(key, cacheEntry =>
            {
                cacheEntry.AbsoluteExpirationRelativeToNow = CACHING_TIME;
                return new ConcurrentDictionary<string, T>();
            });

            collection.AddOrUpdate(key, value, (oldkey, oldvalue) => value);
        }

        public bool RemoveFromCache<T>(string key)
        {
            var removed = false;

            if (!string.IsNullOrEmpty(key) && this.memoryCache.TryGetValue<ConcurrentDictionary<string, T>>(key, out var collection))
            {
                removed = collection.TryRemove(key, out _);
            }

            return removed;
        }

        public void AddToCollection<T>(string key, T value)
        {
            var collection = this.memoryCache.GetOrCreate(key, cacheEntry =>
            {
                cacheEntry.AbsoluteExpirationRelativeToNow = CACHING_TIME;
                return new ConcurrentDictionary<string, IEnumerable<T>>();
            });

            if (collection.TryGetValue(key, out var enumerable))
            {
                enumerable = enumerable.Append(value);
                collection[key] = enumerable;
            }
            else
            {
                enumerable = new List<T> { value };
                collection.TryAdd(key, enumerable);
            }
        }
    }
}
