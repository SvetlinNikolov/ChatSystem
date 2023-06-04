﻿using AutoMapper;
using ChatSystem.Data;
using ChatSystem.Data.Models;
using ChatSystem.Services.Constants;
using ChatSystem.Services.Services.Contracts;
using ChatSystem.ViewModels.Cache;
using ChatSystem.ViewModels.ChatMessages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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

            await _dbContext.AddAsync(chatMessage);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<ChatMessageViewModel>> GetChatMessagesByUserIdsAsync(int userId, int skip, int take)
        {
            var currentUserId = _userService.GetCurrentUserId();

            var conversation = await _conversationService.GetConversationAsync(userId, currentUserId);

            if (conversation == null)
            {
                return Enumerable.Empty<ChatMessageViewModel>();
            }

            var messages = await _dbContext.ChatMessages
                .Where(x => x.ConversationId == conversation.Id)
                .OrderByDescending(m => m.Timestamp)
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            return _mapper.Map<IEnumerable<ChatMessageViewModel>>(messages);
        }

    }
}
