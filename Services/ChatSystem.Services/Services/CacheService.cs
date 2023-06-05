using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;
using ChatSystem.Services.Constants;
using ChatSystem.Services.Services.Contracts;

public class CacheService : ICacheService
{
    private readonly ConcurrentDictionary<string, object> _cacheEntries;
    private readonly TimeSpan _cachingTime;

    public CacheService()
    {
        _cacheEntries = new ConcurrentDictionary<string, object>();
        _cachingTime = CacheConstants.GlobalCachingTime;
    }

    public T Get<T>(string key)
    {
        if (_cacheEntries.TryGetValue(key, out var value) && value is T typedValue)
        {
            return typedValue;
        }

        return default(T);
    }

    public bool TryGet<T>(string key, out T value)
    {
        if (_cacheEntries.TryGetValue(key, out var cacheValue) && cacheValue is T typedValue)
        {
            value = typedValue;
            return true;
        }

        value = default(T);
        return false;
    }

    public void SetOrUpdate<T>(string key, T value)
    {
        _cacheEntries.AddOrUpdate(key, value, (_, __) => value);
    }

    public void RemoveFromCache(string key)
    {
        _cacheEntries.TryRemove(key, out _);
    }

    public T GetOrCreate<T>(string key, Func<T> createFunc)
    {
        return (T)_cacheEntries.GetOrAdd(key, _ => createFunc());
    }

    public void UpdateCache<T>(string key, T value)
    {
        _cacheEntries[key] = value;
    }

    public IEnumerable<KeyValuePair<string, T>> GetAllCacheEntriesWithPrefix<T>(string prefix)
    {
        return _cacheEntries
            .Where(kv => kv.Key.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            .Select(kv => new KeyValuePair<string, T>(kv.Key, (T)kv.Value));
    }

    public void RemoveAllCacheEntriesWithPrefix(string prefix)
    {
        var matchingKeys = _cacheEntries.Keys.Where(key => key.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)).ToList();

        foreach (var key in matchingKeys)
        {
            _cacheEntries.TryRemove(key, out _);
        }
    }

}
