using AutoMapper;
using ChatSystem.Data;
using ChatSystem.Data.Models;
using ChatSystem.Services.Services.Contracts;
using ChatSystem.ViewModels.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ChatSystem.Services.Services
{
    public class UserService : IUserService
    {
        private readonly ChatDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(
           ChatDbContext dbContext,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<ChatUserViewModel>> GetAllUsersAsync()
        {
            var currentUserId = GetCurrentUserId();
            var users = await _dbContext.ChatUsers.ToListAsync();

            if (users != null && users.Any())
            {
                var filteredUsers = users.Where(u => u.Id != currentUserId);

                return _mapper.Map<IEnumerable<ChatUserViewModel>>(filteredUsers);
            }

            return Enumerable.Empty<ChatUserViewModel>();
        }

        public async Task<ChatUserViewModel> GetByUsername(string username)
        {
            var user = await _dbContext.ChatUsers.FirstOrDefaultAsync(x => x.UserName == username);

            if (user != null)
            {
                return _mapper.Map<ChatUserViewModel>(user);
            }

            return default(ChatUserViewModel);
        }

        public int GetCurrentUserId()
        {
            var userIdString = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (int.TryParse(userIdString, out int userId))
            {
                return userId;
            }

            return default(int);
        }

        public string GetCurrentUserUsername()
        {
            return _httpContextAccessor.HttpContext.User.Identity.Name;
        }
    }
}
