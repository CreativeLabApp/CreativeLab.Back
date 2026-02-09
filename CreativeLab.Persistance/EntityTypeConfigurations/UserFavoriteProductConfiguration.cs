using CreativeLab.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CreativeLab.Persistence.EntityTypeConfigurations;

public class UserFavoriteProductConfiguration : IEntityTypeConfiguration<UserFavoriteProduct>
{
    public void Configure(EntityTypeBuilder<UserFavoriteProduct> builder)
    {
        builder.ToTable("UserFavoriteProducts");

        builder.HasKey(f => f.Id);

        builder.Property(f => f.AddedAt)
            .IsRequired();

        // Составной уникальный индекс
        builder.HasIndex(f => new { f.UserId, f.ProductId })
            .IsUnique();

        // Внешние ключи
        builder.HasOne(f => f.User)
            .WithMany(u => u.FavoriteProducts)
            .HasForeignKey(f => f.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(f => f.Product)
            .WithMany(p => p.FavoritedBy)
            .HasForeignKey(f => f.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
