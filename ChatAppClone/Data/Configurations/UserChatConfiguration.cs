namespace ChatAppClone.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using ChatAppClone.Data.Models;

    public class UserChatConfiguration : IEntityTypeConfiguration<UserChat>
    {
        public void Configure(EntityTypeBuilder<UserChat> builder)
        {
            builder.HasKey(uc => uc.Id);

            builder
                .HasOne(uc => uc.User)
                .WithMany(u => u.UsersChats)
                .HasForeignKey(uc => uc.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(uc => uc.Chat)
                .WithMany(c => c.UsersChats)
                .HasForeignKey(uc => uc.ChatId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
