using AutoMapper;
using ChatSystem.Data.Models;
using ChatSystem.ViewModels.ChatMessages;
using ChatSystem.ViewModels.Users;

namespace ShoppingList.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ChatUser, ChatUserViewModel>();

            CreateMap<ChatMessage, ChatMessageViewModel>();
        }
    }
}