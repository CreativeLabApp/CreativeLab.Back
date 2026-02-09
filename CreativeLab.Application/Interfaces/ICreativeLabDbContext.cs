using CreativeLab.Domain;
using Microsoft.EntityFrameworkCore;

namespace CreativeLab.Application.Interfaces;

public interface ICreativeLabDbContext
{
    DbSet<User> Users { get; set; }
    DbSet<UserFavoriteMasterclass> UserFavoriteMasterclasses { get; set; }
    DbSet<UserFavoriteProduct> UserFavoriteProducts { get; set; }
    DbSet<Category> Categories { get; set; }
    DbSet<Masterclass> Masterclasses { get; set; }
    DbSet<Product> Products { get; set; }
    DbSet<ProductMaterial> ProductMaterials { get; set; }
    DbSet<Tag> Tags { get; set; }
    DbSet<Chat> Chats { get; set; }
    DbSet<ChatParticipant> ChatParticipants { get; set; }
    DbSet<Message> Messages { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
