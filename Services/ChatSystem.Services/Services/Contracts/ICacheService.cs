using System;

namespace ChatSystem.Services.Services.Contracts
{
    public interface ICacheService
    {
        T Get<T>(string key);
        bool TryGet<T>(string key, out T value);
        void SetOrUpdate<T>(string key, T value);
        bool RemoveFromCache<T>(string key);
        void AddToCollection<T>(string key, T value);
    }
}
