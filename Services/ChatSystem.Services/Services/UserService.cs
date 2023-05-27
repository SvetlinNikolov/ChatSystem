using AutoMapper;
using ChatSystem.Services.Repositories.Contracts;
using ChatSystem.Services.Services.Contracts;
using ChatSystem.ViewModels.Users;

namespace ChatSystem.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(
            IUserRepository userRepository,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ChatUserViewModel>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<ChatUserViewModel>>(users);
        }
    }
}
