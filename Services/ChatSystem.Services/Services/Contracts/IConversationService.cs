using ChatSystem.Data.Models;

namespace ChatSystem.Services.Services.Contracts
{
    public interface IConversationService
    {
        Task<ChatConversation> GetConversationAsync(string firstUserId, string secondUserId);

        Task<int> CreateConversationAsync(string firstUserId, string secondUserId);
    }
}
