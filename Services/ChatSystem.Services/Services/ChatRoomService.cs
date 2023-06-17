using ChatSystem.Data;
using ChatSystem.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatSystem.Services.Services
{
    public class ChatRoomService
    {
        private readonly ChatDbContext _dbContext;

        public ChatRoomService(ChatDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> CreateChatRoomAsync(int chatRoomId)
        {
            var chatRoom = await GetChatRoomAsync(chatRoomId);

            if (chatRoom == null)
            {
                var newChatRoom = new ChatRoom();

                await _dbContext.ChatRooms.AddAsync(newChatRoom);
                await _dbContext.SaveChangesAsync();

                return newChatRoom.Id;
            }

            return chatRoom.Id;
        }

        public async Task<ChatRoom> GetChatRoomAsync(int chatRoomId)
        {
            var chatRoom = await _dbContext.ChatRooms
                .FirstOrDefaultAsync(x => x.Id == chatRoomId);

            return chatRoom;
        }
    }
}
