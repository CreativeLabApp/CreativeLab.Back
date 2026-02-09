using CreativeLab.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CreativeLab.Persistence.EntityTypeConfigurations;

public class ChatParticipantConfiguration : IEntityTypeConfiguration<ChatParticipant>
{
    public void Configure(EntityTypeBuilder<ChatParticipant> builder)
    {
        builder.ToTable("ChatParticipants");

        builder.HasKey(cp => cp.Id);

        builder.Property(cp => cp.JoinedAt)
            .IsRequired();

        builder.Property(cp => cp.LastReadAt);

        builder.Property(cp => cp.IsMuted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(cp => cp.IsPinned)
            .IsRequired()
            .HasDefaultValue(false);

        // Составной уникальный индекс
        builder.HasIndex(cp => new { cp.ChatId, cp.UserId })
            .IsUnique();

        builder.HasIndex(cp => cp.UserId);

        // Внешние ключи
        builder.HasOne(cp => cp.Chat)
            .WithMany(c => c.Participants)
            .HasForeignKey(cp => cp.ChatId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(cp => cp.User)
            .WithMany(u => u.ChatParticipants)
            .HasForeignKey(cp => cp.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
