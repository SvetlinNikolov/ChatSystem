using ChatSystem.Data.Models;
namespace ChatSystem.Services.Services.Contracts
{
    public interface IChatService
    {
        Task<ChatMessage> GetChatMessageByIdAsync(int messageId);
        Task<IEnumerable<ChatMessage>> GetAllChatMessagesAsync();
        Task AddChatMessageAsync(ChatMessage message);
    }
}
