using CreativeLab.Domain;
using Microsoft.EntityFrameworkCore;

namespace CreativeLab.Application.Interfaces;

public interface ICreativeLabDbContext
{
    DbSet<User> Users { get; set; }
    DbSet<UserFavoriteMasterclass> UserFavoriteMasterclasses { get; set; }
    DbSet<UserFavoriteProduct> UserFavoriteProducts { get; set; }
    DbSet<RefreshToken> RefreshTokens { get; set; }
    DbSet<Category> Categories { get; set; }
    DbSet<Masterclass> Masterclasses { get; set; }
    DbSet<MasterclassMaterial> MasterclassMaterials { get; set; }
    DbSet<MasterclassRating> MasterclassRatings { get; set; }
    DbSet<ProductRating> ProductRatings { get; set; }
    DbSet<Product> Products { get; set; }
    DbSet<ProductMaterial> ProductMaterials { get; set; }
    DbSet<Tag> Tags { get; set; }
    DbSet<Chat> Chats { get; set; }
    DbSet<ChatParticipant> ChatParticipants { get; set; }
    DbSet<Message> Messages { get; set; }
    DbSet<UserPhoto> UserPhotos { get; set; }
    DbSet<AgeCategory> AgeCategories { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
