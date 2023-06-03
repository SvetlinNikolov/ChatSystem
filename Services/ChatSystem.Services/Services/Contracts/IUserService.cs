using ChatSystem.Data.Models;
using ChatSystem.ViewModels.Users;

namespace ChatSystem.Services.Services.Contracts
{
    public interface IUserService
    {
        Task<IEnumerable<ChatUserViewModel>> GetAllUsersAsync();

        Task<ChatUserViewModel> GetUserByUsername(string username);

        int GetCurrentUserId();

        string GetCurrentUserUsername();

        Task<string> GetUsernameByIdAsync(int userId);

        Task<ChatUserViewModel> GetUserByIdAsync(int userId);
    }
}
