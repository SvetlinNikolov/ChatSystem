using ChatSystem.Data.Models;
using ChatSystem.Services.Repositories.Contracts;
using ChatSystem.Services.Services.Contracts;

namespace ChatSystem.Services.Services
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepository;

        public ChatService(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }

        public async Task<ChatMessage> GetChatMessageByIdAsync(int messageId)
        {
            return await _chatRepository.GetByIdAsync(messageId);
        }

        public async Task<IEnumerable<ChatMessage>> GetAllChatMessagesAsync()
        {
            return await _chatRepository.GetAllAsync();
        }

        public async Task AddChatMessageAsync(ChatMessage message)
        {
            await _chatRepository.AddAsync(message);
        }
    }
}
