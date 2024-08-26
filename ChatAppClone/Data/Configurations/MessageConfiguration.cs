namespace ChatAppClone.Data.Configurations
{
    using ChatAppClone.Data.Models;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Microsoft.EntityFrameworkCore;

    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.HasKey(m => m.Id);

            builder
                .Property(m => m.Content)
                .IsRequired()
                .HasMaxLength(4000);

            builder
                .Property(m => m.IsSeen)
                .IsRequired();

            builder
                .HasOne(m => m.Creator)
                .WithMany(u => u.Messages)
                .HasForeignKey(m => m.CreatorId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(m => m.Chat)
                .WithMany(c => c.Messages)
                .HasForeignKey(m => m.ChatId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
