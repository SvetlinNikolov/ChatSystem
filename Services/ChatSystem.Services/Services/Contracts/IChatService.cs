using ChatSystem.Data.Models;
using ChatSystem.ViewModels.ChatMessages;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ChatSystem.Services.Services.Contracts
{
    public interface IChatService
    {
        Task AddChatMessageAsync(int senderId, int conversationId, string messageContent);

        Task<IEnumerable<ChatMessageViewModel>> GetChatMessagesByUserIdsAsync(int userId, int skip, int take);
    }
}
