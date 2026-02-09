namespace CreativeLab.Domain;

public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Избранное
    public ICollection<UserFavoriteMasterclass> FavoriteMasterclasses { get; set; } = [];
    public ICollection<UserFavoriteProduct> FavoriteProducts { get; set; } = [];

    // Контент пользователя
    public ICollection<Masterclass> CreatedMasterclasses { get; set; } = [];
    public ICollection<Product> CreatedProducts { get; set; } = [];

    // Чаты
    public ICollection<ChatParticipant> ChatParticipants { get; set; } = [];
    public ICollection<Message> SentMessages { get; set; } = [];
}
