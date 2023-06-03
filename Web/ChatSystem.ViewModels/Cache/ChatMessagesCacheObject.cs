using ChatSystem.Data.Models;
using ChatSystem.ViewModels.ChatMessages;

namespace ChatSystem.ViewModels.Cache
{
    public class ChatMessagesCacheObject
    {
        public ChatMessagesCacheObject()
        {
            Messages = new List<ChatMessage>();
        }

        public ICollection<ChatMessage> Messages { get; set; }
    }

}
