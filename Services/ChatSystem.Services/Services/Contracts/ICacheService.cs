﻿namespace ChatSystem.Services.Services.Contracts
{
    public interface ICacheService
    {
        T Get<T>(string key);
        bool TryGet<T>(string key, out T value);
        void SetOrUpdate<T>(string key, T value);
        void RemoveFromCache(string key);
        T GetOrCreate<T>(string key, Func<T> createFunc);
        void UpdateCache<T>(string key, T value);
        IEnumerable<KeyValuePair<string, T>> GetAllCacheEntriesWithPrefix<T>(string prefix);
        void RemoveAllCacheEntriesWithPrefix(string prefix);
    }
}