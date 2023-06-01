using ChatSystem.Services.Factories.Contracts;
using ChatSystem.Services.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace ChatSystem.Services.Factories
{
    public class CacheServiceFactory : ICacheServiceFactory
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public CacheServiceFactory(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public ICacheService Create(IServiceProvider serviceProvider)
        {
            using var scope = _scopeFactory.CreateScope();
            return scope.ServiceProvider.GetRequiredService<ICacheService>();
        }
    }
}
