using ChatSystem.Data;
using ChatSystem.Data.Models;
using ChatSystem.Services.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace ChatSystem.Services.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly ChatDbContext _dbContext;

        public ChatRepository(ChatDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ChatMessage> GetByIdAsync(int id)
        {
            return await _dbContext.ChatMessages.FindAsync(id);
        }

        public async Task<IEnumerable<ChatMessage>> GetAllAsync()
        {
            return await _dbContext.ChatMessages.ToListAsync();
        }

        public async Task AddAsync(ChatMessage message)
        {
            _dbContext.ChatMessages.Add(message);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(ChatMessage message)
        {
            _dbContext.ChatMessages.Update(message);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(ChatMessage message)
        {
            _dbContext.ChatMessages.Remove(message);
            await _dbContext.SaveChangesAsync();
        }
    }
}
