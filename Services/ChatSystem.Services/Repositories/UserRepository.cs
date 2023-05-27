using ChatSystem.Data;
using ChatSystem.Data.Models;
using ChatSystem.Services.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

public class UserRepository : IUserRepository
{
    private readonly ChatDbContext _dbContext;

    public UserRepository(ChatDbContext context)
    {
        _dbContext = context;
    }

    public async Task<IEnumerable<ChatUser>> GetAllAsync()
    {
        return await _dbContext.ChatUsers.ToListAsync();
    }
}