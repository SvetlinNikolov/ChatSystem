﻿using ChatSystem.Data;
using ChatSystem.Data.Models;
using ChatSystem.Services.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace ChatSystem.Services.Services
{
    public class ConversationService : IConversationService
    {
        private readonly ChatDbContext _dbContext;

        public ConversationService(ChatDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> CreateConversationAsync(int firstUserId, int secondUserId)
        {
            var conversation = await GetConversationAsync(firstUserId, secondUserId);

            if (conversation == null)
            {
                var newConversation = new ChatConversation
                {
                    User1Id = firstUserId,
                    User2Id = secondUserId
                };

                await _dbContext.ChatConversations.AddAsync(newConversation);
                await _dbContext.SaveChangesAsync();

                return newConversation.Id;
            }

            return conversation.Id;
        }

        public async Task<ChatConversation> GetConversationAsync(int firstUserId, int secondUserId)
        {
            var conversation = await _dbContext.ChatConversations
                .FirstOrDefaultAsync(c => (c.User1.Id == firstUserId && c.User2.Id == secondUserId) ||
                          (c.User1.Id == secondUserId && c.User2.Id == firstUserId));

            return conversation;
        }
    }
}
