using CreativeLab.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace CreativeLab.Persistence.EntityTypeConfigurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Description)
            .IsRequired()
            .HasColumnType("text");

        builder.Property(p => p.ShortDescription)
            .HasMaxLength(300);

        builder.Property(p => p.Price)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(p => p.DiscountPrice)
            .HasPrecision(18, 2);

        builder.Property(p => p.SKU)
            .HasMaxLength(100);

        builder.Property(p => p.StockQuantity)
            .IsRequired()
            .HasDefaultValue(1);

        builder.Property(p => p.IsAvailable)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(p => p.Weight)
            .HasPrecision(10, 3);

        builder.Property(p => p.Dimensions)
            .HasMaxLength(50);

        builder.Property(p => p.CreatedAt)
            .IsRequired();

        builder.Property(p => p.UpdatedAt);

        builder.Property(p => p.ThumbnailUrl)
            .HasMaxLength(500);

        // Преобразование массива ImageUrls в JSON
        builder.Property(p => p.ImageUrls)
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                v => JsonSerializer.Deserialize<string[]>(v, (JsonSerializerOptions)null) ?? Array.Empty<string>());

        // Индексы
        builder.HasIndex(p => p.SellerId);
        builder.HasIndex(p => p.CategoryId);
        builder.HasIndex(p => p.Price);
        builder.HasIndex(p => p.IsAvailable);
        builder.HasIndex(p => p.CreatedAt);

        // Внешние ключи
        builder.HasOne(p => p.Seller)
            .WithMany(u => u.CreatedProducts)
            .HasForeignKey(p => p.SellerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // Связь многие-ко-многим с Tags
        builder.HasMany(p => p.Tags)
            .WithMany(t => t.Products)
            .UsingEntity<Dictionary<string, object>>(
                "ProductTag",
                j => j
                    .HasOne<Tag>()
                    .WithMany()
                    .HasForeignKey("TagId")
                    .OnDelete(DeleteBehavior.Cascade),
                j => j
                    .HasOne<Product>()
                    .WithMany()
                    .HasForeignKey("ProductId")
                    .OnDelete(DeleteBehavior.Cascade),
                j =>
                {
                    j.HasKey("ProductId", "TagId");
                    j.ToTable("ProductTags");
                    j.HasIndex("TagId");
                });

        // Связь многие-ко-многим с Materials
        builder.HasMany(p => p.Materials)
            .WithMany(m => m.Products)
            .UsingEntity<Dictionary<string, object>>(
                "ProductMaterialLink",
                j => j
                    .HasOne<ProductMaterial>()
                    .WithMany()
                    .HasForeignKey("MaterialId")
                    .OnDelete(DeleteBehavior.Cascade),
                j => j
                    .HasOne<Product>()
                    .WithMany()
                    .HasForeignKey("ProductId")
                    .OnDelete(DeleteBehavior.Cascade),
                j =>
                {
                    j.HasKey("ProductId", "MaterialId");
                    j.ToTable("ProductMaterialLinks");
                    j.HasIndex("MaterialId");
                });
    }
}
