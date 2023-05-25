
using ChatSystem.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using System.Reflection.Metadata;

namespace ChatSystem.Data
{
    public class ChatDbContext : IdentityDbContext<ChatUser, IdentityRole, string>
    {
        public ChatDbContext(DbContextOptions<ChatDbContext> options)
          : base(options)
        {
        }
        public DbSet<ChatConversation> ChatConverstations { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }

        public DbSet<ChatUser> ChatUsers { get; set; }
    }
}
