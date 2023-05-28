using AutoMapper;
using ChatSystem.Data.Models;
using ChatSystem.ViewModels.Users;

namespace ShoppingList.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ChatUser, ChatUserViewModel>();
        }
    }
}