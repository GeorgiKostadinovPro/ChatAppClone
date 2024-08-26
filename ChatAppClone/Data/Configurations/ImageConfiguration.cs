namespace ChatAppClone.Data.Configurations
{
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Microsoft.EntityFrameworkCore;
    using ChatAppClone.Data.Models;

    public class ImageConfiguration : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.HasKey(i => i.Id);

            builder
                .Property(i => i.Url)
                .IsRequired()
                .HasMaxLength(2048);

            builder
                .Property(i => i.PublicId)
                .IsRequired()
                .HasMaxLength(256);

            builder
                .HasOne(i => i.Chat)
                .WithMany(c => c.Images)
                .HasForeignKey(i => i.ChatId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(i => i.Message)
                .WithMany(c => c.Images)
                .HasForeignKey(i => i.MessageId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
