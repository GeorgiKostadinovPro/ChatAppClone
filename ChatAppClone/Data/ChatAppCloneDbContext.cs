namespace ChatAppClone.Data
{
    using ChatAppClone.Data.Models;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using System.Reflection;

    public class ChatAppCloneDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ChatAppCloneDbContext(DbContextOptions<ChatAppCloneDbContext> options)
            : base(options)
        {
        }

        public DbSet<Image> Images { get; set; } = null!;

        public DbSet<Message> Messages { get; set; } = null!;

        public DbSet<Chat> Chats { get; set; } = null!;

        public DbSet<UserChat> UsersChats { get; set; } = null!;

        public DbSet<UserFollows> UserFollows {  get; set; } = null!; 

        public DbSet<Notification> Notifications { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            Assembly assembly = Assembly.GetAssembly(typeof(ChatAppCloneDbContext))
                ?? Assembly.GetExecutingAssembly();

            builder.ApplyConfigurationsFromAssembly(assembly);

            base.OnModelCreating(builder);
        }
    }
}
