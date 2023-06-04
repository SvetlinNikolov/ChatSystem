using System;
using Microsoft.Extensions.Caching.Memory;
using ChatSystem.Services.Constants;
using ChatSystem.Services.Services.Contracts;

public class CacheService : ICacheService
{
    private readonly IMemoryCache _memoryCache;
    private readonly TimeSpan _cachingTime;

    public CacheService(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
        _cachingTime = CacheConstants.GlobalCachingTime;
    }

    public T Get<T>(string key)
    {
        return _memoryCache.Get<T>(key);
    }

    public bool TryGet<T>(string key, out T value)
    {
        return _memoryCache.TryGetValue(key, out value);
    }

    public void SetOrUpdate<T>(string key, T value)
    {
        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(_cachingTime);

        _memoryCache.Set(key, value, cacheEntryOptions);
    }

    public void RemoveFromCache(string key)
    {
        _memoryCache.Remove(key);
    }

    public T GetOrCreate<T>(string key, Func<T> createFunc)
    {
        if (_memoryCache.TryGetValue(key, out T value))
        {
            return value;
        }

        var createdValue = createFunc();
        SetOrUpdate(key, createdValue);

        return createdValue;
    }

    public void UpdateCache<T>(string key, T value)
    {
        if (_memoryCache.TryGetValue(key, out T existingValue))
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(_cachingTime);

            _memoryCache.Set(key, value, cacheEntryOptions);
        }
    }
}
