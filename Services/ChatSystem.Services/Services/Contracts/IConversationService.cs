using ChatSystem.Data.Models;

namespace ChatSystem.Services.Services.Contracts
{
    public interface IConversationService
    {
        Task<ChatConversation> GetConversationAsync(int firstUserId, int secondUserId);

        Task<int> CreateConversationAsync(int firstUserId, int secondUserId);
    }
}
