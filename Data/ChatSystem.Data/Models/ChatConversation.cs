using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatSystem.Data.Models
{
    public class ChatConversation
    {
        // This can be a chat room but requirement is only for 2 participants
        public int Id { get; set; }

        public string Name { get; set; }

        public ChatUser User1 { get; set; }

        public ChatUser User2 { get; set; }

        public ICollection<ChatMessage> Messages { get; set; }
    }
}
