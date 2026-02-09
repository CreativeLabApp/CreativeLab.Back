using CreativeLab.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CreativeLab.Persistence.EntityTypeConfigurations;

public class UserFavoriteMasterclassConfiguration : IEntityTypeConfiguration<UserFavoriteMasterclass>
{
    public void Configure(EntityTypeBuilder<UserFavoriteMasterclass> builder)
    {
        builder.ToTable("UserFavoriteMasterclasses");

        builder.HasKey(f => f.Id);

        builder.Property(f => f.AddedAt)
            .IsRequired();

        // Составной уникальный индекс
        builder.HasIndex(f => new { f.UserId, f.MasterclassId })
            .IsUnique();

        // Внешние ключи
        builder.HasOne(f => f.User)
            .WithMany(u => u.FavoriteMasterclasses)
            .HasForeignKey(f => f.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(f => f.Masterclass)
            .WithMany(m => m.FavoritedBy)
            .HasForeignKey(f => f.MasterclassId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
