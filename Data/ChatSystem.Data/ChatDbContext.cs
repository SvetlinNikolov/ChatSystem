using ChatSystem.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChatSystem.Data
{
    public class ChatDbContext : IdentityDbContext<ChatUser, IdentityRole<int>, int>
    {
        public ChatDbContext(DbContextOptions<ChatDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ChatConversation>()
                .HasOne(c => c.User1)
                .WithMany()
                .HasForeignKey(c => c.User1Id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ChatConversation>()
                .HasOne(c => c.User2)
                .WithMany()
                .HasForeignKey(c => c.User2Id)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }

        public DbSet<ChatConversation> ChatConversations { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }

        public DbSet<ChatUser> ChatUsers { get; set; }
    }
}
