using CreativeLab.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace CreativeLab.Persistence.EntityTypeConfigurations;

public class MasterclassConfiguration : IEntityTypeConfiguration<Masterclass>
{
    public void Configure(EntityTypeBuilder<Masterclass> builder)
    {
        builder.ToTable("Masterclasses");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(m => m.Description)
            .IsRequired()
            .HasColumnType("text");

        builder.Property(m => m.ShortDescription)
            .HasMaxLength(300);

        builder.Property(m => m.Views)
            .IsRequired();

        builder.Property(m => m.Rating)
            .HasPrecision(3, 2)
            .HasDefaultValue(0);

        builder.Property(m => m.RatingsCount)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(m => m.CreatedAt)
            .IsRequired();

        builder.Property(m => m.UpdatedAt);

        builder.Property(m => m.IsPublished)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(m => m.PublishedAt);

        // Преобразование массива ImageUrls в JSON
        builder.Property(m => m.ImageUrls)
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                v => JsonSerializer.Deserialize<string[]>(v, (JsonSerializerOptions)null) ?? Array.Empty<string>());

        builder.Property(m => m.ThumbnailUrl)
            .HasMaxLength(500);

        builder.Property(m => m.VideoUrl)
            .HasMaxLength(500);

        // Индексы
        builder.HasIndex(m => m.AuthorId);
        builder.HasIndex(m => m.CategoryId);
        builder.HasIndex(m => m.Rating);
        builder.HasIndex(m => m.CreatedAt);
        builder.HasIndex(m => m.IsPublished);

        // Внешние ключи
        builder.HasOne(m => m.Author)
            .WithMany(u => u.CreatedMasterclasses)
            .HasForeignKey(m => m.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(m => m.Category)
            .WithMany(c => c.Masterclasses)
            .HasForeignKey(m => m.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(m => m.AgeCategory)
            .WithMany(a => a.Masterclasses)
            .HasForeignKey(m => m.AgeCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // Связь многие-ко-многим с Tags
        builder.HasMany(m => m.Tags)
            .WithMany(t => t.Masterclasses)
            .UsingEntity<Dictionary<string, object>>(
                "MasterclassTag",
                j => j
                    .HasOne<Tag>()
                    .WithMany()
                    .HasForeignKey("TagId")
                    .OnDelete(DeleteBehavior.Cascade),
                j => j
                    .HasOne<Masterclass>()
                    .WithMany()
                    .HasForeignKey("MasterclassId")
                    .OnDelete(DeleteBehavior.Cascade),
                j =>
                {
                    j.HasKey("MasterclassId", "TagId");
                    j.ToTable("MasterclassTags");
                    j.HasIndex("TagId");
                });

        // Связь многие-ко-многим с Materials
        builder.HasMany(m => m.Materials)
            .WithMany(mat => mat.Masterclasses)
            .UsingEntity<Dictionary<string, object>>(
                "MasterclassMaterialLink",
                j => j
                    .HasOne<MasterclassMaterial>()
                    .WithMany()
                    .HasForeignKey("MaterialId")
                    .OnDelete(DeleteBehavior.Cascade),
                j => j
                    .HasOne<Masterclass>()
                    .WithMany()
                    .HasForeignKey("MasterclassId")
                    .OnDelete(DeleteBehavior.Cascade),
                j =>
                {
                    j.HasKey("MasterclassId", "MaterialId");
                    j.ToTable("MasterclassMaterialLinks");
                    j.HasIndex("MaterialId");
                });
    }
}
