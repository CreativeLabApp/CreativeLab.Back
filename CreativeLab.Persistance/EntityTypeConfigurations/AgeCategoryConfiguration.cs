using CreativeLab.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CreativeLab.Persistence.EntityTypeConfigurations;

public class AgeCategoryConfiguration : IEntityTypeConfiguration<AgeCategory>
{
    public void Configure(EntityTypeBuilder<AgeCategory> builder)
    {
        builder.ToTable("AgeCategories");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(a => a.MinAge)
            .IsRequired();

        builder.Property(a => a.MaxAge)
            .IsRequired();

        builder.Property(a => a.Description)
            .HasMaxLength(200);

        builder.HasIndex(a => a.Name).IsUnique();
    }
}