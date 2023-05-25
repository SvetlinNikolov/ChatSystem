namespace ChatSystem.Data.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime Timestamp { get; set; }

        public int UserId { get; set; }

        public int ConversationId { get; set; }

        public ChatUser User { get; set; }

        public ChatConversation Conversation { get; set; }
    }
}
