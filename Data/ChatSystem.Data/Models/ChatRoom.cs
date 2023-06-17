namespace ChatSystem.Data.Models
{
    public class ChatRoom
    {
        public int Id { get; set; }

        public ICollection<ChatMessage> Messages { get; set; }

        public ICollection<ChatUser> Users { get; set; }
    }
}
