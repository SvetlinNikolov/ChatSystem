using ChatSystem.Data;
using ChatSystem.Data.Models;
using ChatSystem.Services.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace ChatSystem.Services.Services
{
    public class ChatService : IChatService
    {
        private readonly ChatDbContext _dbContext;

        public ChatService(ChatDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ChatMessage> GetChatMessageByIdAsync(int messageId)
        {
            return await _dbContext.ChatMessages.FirstOrDefaultAsync(x => x.Id == messageId);
        }

        public async Task<IEnumerable<ChatMessage>> GetAllChatMessagesAsync()
        {
            return await _dbContext.ChatMessages.ToListAsync();
        }

        public async Task AddChatMessageAsync(string senderId, int conversationId, string messageContent)
        {
            if (string.IsNullOrEmpty(senderId))
            {
                throw new ArgumentException($"'{nameof(senderId)}' cannot be null or empty.", nameof(senderId));
            }

            if (conversationId <= 0)
            {
                throw new ArgumentException($"'{nameof(conversationId)}' cannot be null or empty.", nameof(conversationId));
            }

            var chatMessage = new ChatMessage
            {
                Content = messageContent,
                SenderId = senderId,
                ConversationId = conversationId,
                Timestamp = DateTime.UtcNow,
            };

            await _dbContext.ChatMessages.AddAsync(chatMessage);
            await _dbContext.SaveChangesAsync();
        }
    }
}
