using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using ChatSystem.Services.Constants;
using ChatSystem.Services.Services.Contracts;
using Microsoft.Extensions.Caching.Memory;

namespace ChatSystem.Common.Caching
{
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
            if (!string.IsNullOrEmpty(key) && this.memoryCache.TryGetValue<ConcurrentDictionary<string, object>>(key, out var collection))
            {
                if (collection.TryGetValue(key, out var value))
                {
                    return (T)value;
                }
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
            var collection = this.memoryCache.GetOrCreate<ConcurrentDictionary<string, object>>(key, cacheEntry =>
            {
                cacheEntry.AbsoluteExpirationRelativeToNow = CACHING_TIME;
                return new ConcurrentDictionary<string, object>();
            });

            collection.AddOrUpdate(key, value, (oldKey, oldValue) => value);
        }

        public bool RemoveFromCache<T>(string key)
        {
            var removed = false;

            if (!string.IsNullOrEmpty(key) && this.memoryCache.TryGetValue<ConcurrentDictionary<string, object>>(key, out var collection))
            {
                removed = collection.TryRemove(key, out _);
            }

            return removed;
        }

        public void AddToCollection<T>(string key, T value)
        {
            var collection = this.memoryCache.GetOrCreate<ConcurrentDictionary<string, List<T>>>(key, cacheEntry =>
            {
                cacheEntry.AbsoluteExpirationRelativeToNow = CACHING_TIME;
                return new ConcurrentDictionary<string, List<T>>();
            });

            if (collection.TryGetValue(key, out var list))
            {
                list.Add(value);
            }
            else
            {
                list = new List<T> { value };
                collection.TryAdd(key, list);
            }
        }
    }
}
