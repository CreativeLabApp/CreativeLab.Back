using CreativeLab.Application.Interfaces;
using CreativeLab.Domain;
using Microsoft.EntityFrameworkCore;

namespace CreativeLab.Persistence;

public class CreativeLabDbContext(DbContextOptions<CreativeLabDbContext> options) 
    : DbContext(options), ICreativeLabDbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<UserFavoriteMasterclass> UserFavoriteMasterclasses { get; set; }
    public DbSet<UserFavoriteProduct> UserFavoriteProducts { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Masterclass> Masterclasses { get; set; }
    public DbSet<MasterclassMaterial> MasterclassMaterials { get; set; }
    public DbSet<MasterclassRating> MasterclassRatings { get; set; }
    public DbSet<ProductRating> ProductRatings { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductMaterial> ProductMaterials { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<Chat> Chats { get; set; }
    public DbSet<ChatParticipant> ChatParticipants { get; set; }
    public DbSet<Message> Messages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CreativeLabDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
