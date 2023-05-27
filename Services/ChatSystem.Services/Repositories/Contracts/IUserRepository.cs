using ChatSystem.Data.Models;

namespace ChatSystem.Services.Repositories.Contracts
{
    public interface IUserRepository
    {
        Task<IEnumerable<ChatUser>> GetAllAsync();
    }
}
