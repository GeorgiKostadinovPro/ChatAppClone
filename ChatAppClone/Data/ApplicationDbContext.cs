namespace ChatAppClone.Data
{
    using ChatAppClone.Data.Models;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using System.Reflection;

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public Image Images { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<Chat> Chats { get; set; }

        public DbSet<UserChat> UsersChats { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            Assembly assembly = Assembly.GetAssembly(typeof(ApplicationDbContext))
                ?? Assembly.GetExecutingAssembly();

            builder.ApplyConfigurationsFromAssembly(assembly);

            base.OnModelCreating(builder);
        }
    }
}