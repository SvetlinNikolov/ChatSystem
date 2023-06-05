using AutoMapper;
using ChatSystem.Data;
using ChatSystem.Data.Models;
using ChatSystem.Services.Constants;
using ChatSystem.Services.Services.Contracts;
using ChatSystem.ViewModels.Cache;
using ChatSystem.ViewModels.ChatMessages;
using Microsoft.EntityFrameworkCore;

namespace ChatSystem.Services.Services
{
    public class ChatService : IChatService
    {
        private readonly ChatDbContext _dbContext;
        private readonly ICacheService _cacheService;
        private readonly IConversationService _conversationService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public ChatService(ChatDbContext dbContext, ICacheService cacheService, IConversationService conversationService, IUserService userService, IMapper mapper)
        {
            _dbContext = dbContext;
            _cacheService = cacheService;
            _conversationService = conversationService;
            _userService = userService;
            _mapper = mapper;
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

            var cacheKey = $"{CacheConstants.MessageCacheKey}_{conversationId}";

            if (!_cacheService.TryGet(cacheKey, out List<ChatMessage> cachedMessages))
            {
                // Collection doesn't exist in cache, create a new one
                cachedMessages = new List<ChatMessage>();
                _cacheService.SetOrUpdate(cacheKey, cachedMessages);
            }

            // Add the new chat message to the collection
            cachedMessages.Add(chatMessage);

            if (cachedMessages.Count >= CacheConstants.MaximumAllowedMessagesInCache)
            {
                var lastSavedMessageTimestamp = await GetLastSavedMessageTimestamp(conversationId);

                if (cachedMessages.Any(msg => msg.Timestamp > lastSavedMessageTimestamp))
                {
                    // Send a request to the database to save the collection
                    await _dbContext.AddRangeAsync(cachedMessages);
                    await _dbContext.SaveChangesAsync();

                    // Clear the cache since the messages are saved in the database
                    _cacheService.RemoveFromCache(cacheKey);

                    // Reset the chatMessages list
                    cachedMessages.Clear();
                }
            }
        }

        public async Task<IEnumerable<ChatMessageViewModel>> GetChatMessagesByUserIdsAsync(int userId, int skip, int take)
        {
            var currentUserId = _userService.GetCurrentUserId();

            var conversation = await _conversationService.GetConversationAsync(userId, currentUserId);

            if (conversation == null)
            {
                return Enumerable.Empty<ChatMessageViewModel>();
            }

            var cacheKey = $"{CacheConstants.MessageCacheKey}_{conversation.Id}";

            _cacheService.TryGet(cacheKey, out List<ChatMessage> cachedChatMessages);

            var chatMessagesQuery = _dbContext.ChatMessages
                .Where(x => x.ConversationId == conversation.Id)
                .OrderByDescending(m => m.Timestamp);

            if (cachedChatMessages != null && cachedChatMessages.Any())
            {
                var cachedMessageCount = cachedChatMessages.Count;

                if (skip < cachedMessageCount)
                {
                    var cachedMessagesToReturn = cachedChatMessages
                       .Skip(skip)
                       .Take(take)
                       .OrderByDescending(x => x.Timestamp)
                       .ToList();

                    if (cachedMessagesToReturn.Count == take)
                    {
                        return _mapper.Map<IEnumerable<ChatMessageViewModel>>(cachedMessagesToReturn);
                    }

                    skip = 0;
                    take -= cachedMessagesToReturn.Count;

                    var dbChatMessages = await chatMessagesQuery.Skip(skip).Take(take).ToListAsync();

                    var allMessages = cachedMessagesToReturn
                        .Concat(dbChatMessages)
                        .ToList();

                    return _mapper.Map<IEnumerable<ChatMessageViewModel>>(allMessages);
                }
                else
                {
                    skip -= cachedMessageCount;
                }
            }

            var dbMessages = await chatMessagesQuery.Skip(skip).Take(take).ToListAsync();
            return _mapper.Map<IEnumerable<ChatMessageViewModel>>(dbMessages);
        }

        private async Task<DateTime> GetLastSavedMessageTimestamp(int conversationId)
        {
            var lastSavedMessage = await _dbContext.ChatMessages
                .Where(cm => cm.ConversationId == conversationId)
                .OrderByDescending(cm => cm.Timestamp)
                .FirstOrDefaultAsync();

            return lastSavedMessage?.Timestamp ?? DateTime.MinValue;
        }
    }
}
