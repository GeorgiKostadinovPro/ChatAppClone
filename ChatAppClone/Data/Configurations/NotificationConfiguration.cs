namespace ChatAppClone.Data.Configurations
{
    using ChatAppClone.Common.Constants;
    using ChatAppClone.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.HasKey(n => n.Id);

            builder
                .HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder
                .Property(n => n.Content)
                .IsRequired()
                .HasMaxLength(NotificationConstants.ContentMaxLength);
        }
    }
}
