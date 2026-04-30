using CreativeLab.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CreativeLab.Persistence.EntityTypeConfigurations;

public class MasterclassMaterialConfiguration : IEntityTypeConfiguration<MasterclassMaterial>
{
    public void Configure(EntityTypeBuilder<MasterclassMaterial> builder)
    {
        builder.ToTable("MasterclassMaterials");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(m => m.Description)
            .HasMaxLength(500);

        builder.HasIndex(m => m.Name)
            .IsUnique();
    }
}
