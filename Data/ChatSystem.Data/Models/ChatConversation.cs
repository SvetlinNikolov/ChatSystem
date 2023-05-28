namespace ChatSystem.Data.Models
{
    public class ChatConversation
    {
        // This can be a chat room but requirement is only for 2 participants
        public int Id { get; set; }

        public ChatUser User1 { get; set; }

        public string User1Id { get; set; }

        public ChatUser User2 { get; set; }

        public string User2Id { get; set; }

        public ICollection<ChatMessage> Messages { get; set; }
    }
}
