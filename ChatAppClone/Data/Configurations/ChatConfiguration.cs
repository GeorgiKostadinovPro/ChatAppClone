﻿namespace ChatAppClone.Data.Configurations
{
    using ChatAppClone.Data.Models;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Microsoft.EntityFrameworkCore;

    public class ChatConfiguration : IEntityTypeConfiguration<Chat>
    {
        public void Configure(EntityTypeBuilder<Chat> builder)
        {
            builder.HasKey(c => c.Id);

            builder
                .Property(c => c.Name)
                .HasMaxLength(256);

            builder
                .Property(c => c.IsGroupChat)
                .IsRequired();

            builder
                .Property(c => c.ImageUrl)
                .HasMaxLength(2048);

            builder
                .Property(c => c.LastMessage)
                .HasMaxLength(4000);

            builder
                .Property(c => c.LastActive)
                .IsRequired();
        }
    }
}
