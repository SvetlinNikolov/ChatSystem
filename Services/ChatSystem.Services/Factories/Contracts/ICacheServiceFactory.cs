using ChatSystem.Services.Services.Contracts;

namespace ChatSystem.Services.Factories.Contracts
{
    public interface ICacheServiceFactory
    {
        ICacheService Create(IServiceProvider serviceProvider);
    }
}
