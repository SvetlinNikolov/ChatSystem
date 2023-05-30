using ChatSystem.Data;
using ChatSystem.Data.Models;
using ChatSystem.Services.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatSystem.Services.Services
{
    public class ConversationService : IConversationService
    {
        private readonly ChatDbContext _dbContext;
        private readonly IChatService _chatService;

        public ConversationService(ChatDbContext dbContext, IChatService chatService)
        {
            _dbContext = dbContext;
            _chatService = chatService;
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
