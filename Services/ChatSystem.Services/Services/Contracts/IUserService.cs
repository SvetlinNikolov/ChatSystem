using ChatSystem.Data.Models;
using ChatSystem.ViewModels.Users;

namespace ChatSystem.Services.Services.Contracts
{
    public interface IUserService
    {
        Task<IEnumerable<ChatUserViewModel>> GetAllUsersAsync();

        Task<ChatUserViewModel> GetByUsername(string username);

        int GetCurrentUserId();

        string GetCurrentUserUsername();
    }
}
