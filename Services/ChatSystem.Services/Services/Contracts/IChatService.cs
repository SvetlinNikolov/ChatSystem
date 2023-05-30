using ChatSystem.Data.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ChatSystem.Services.Services.Contracts
{
    public interface IChatService
    {
        Task<ChatMessage> GetChatMessageByIdAsync(int messageId);
        Task<IEnumerable<ChatMessage>> GetAllChatMessagesAsync();
        Task AddChatMessageAsync(int senderId, int conversationId, string messageContent);
    }
}
