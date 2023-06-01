namespace ChatSystem.Services.Services.Contracts
{
    public interface ICacheService
    {
        public T Get<T>(string key);

        public bool TryGet<T>(string key, out T value);

        public void SetOrUpdate<T>(string key, T value);

        public bool RemoveFromCache<T>(string key);

        public void AddToCollection<T>(string key, T value);
    }
}