using ChatSystem.Data;
using ChatSystem.Data.Models;
using ChatSystem.Services.Constants;
using ChatSystem.Services.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace ChatSystem.Services.Services
{
    public class ChatService : IChatService
    {
        private readonly ChatDbContext _dbContext;
        private readonly ICacheService _cacheService;

        public ChatService(ChatDbContext dbContext, ICacheService cacheService)
        {
            _dbContext = dbContext;
            _cacheService = cacheService;
        }

        public async Task<ChatMessage> GetChatMessageByIdAsync(int messageId)
        {
            return await _dbContext.ChatMessages.FirstOrDefaultAsync(x => x.Id == messageId);
        }

        public async Task<IEnumerable<ChatMessage>> GetAllChatMessagesAsync()
        {
            return await _dbContext.ChatMessages.ToListAsync();
        }

        public async Task AddChatMessageAsync(int senderId, int conversationId, string messageContent)
        {
            if (senderId == default(int))
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

            _cacheService.AddToCollection(CacheConstants.MessageCacheKey, chatMessage);

            //await _dbContext.ChatMessages.AddAsync(chatMessage);
            //await _dbContext.SaveChangesAsync();
        }
    }
}
