namespace ChatSystem.Data.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime Timestamp { get; set; }

        public ChatUser Sender { get; set; }

        public string SenderId { get; set; }

        public int ConversationId { get; set; }

        public ChatConversation Conversation { get; set; }
    }
}
