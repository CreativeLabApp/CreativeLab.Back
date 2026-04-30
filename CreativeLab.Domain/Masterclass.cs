namespace CreativeLab.Domain;

public class Masterclass
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ShortDescription { get; set; } // Для превью
    public Guid CategoryId { get; set; }
    public Guid AuthorId { get; set; }

    // Медиа
    public string[] ImageUrls { get; set; } = [];
    public string? ThumbnailUrl { get; set; } // Главное изображение

    // Статистика
    public int Views { get; set; }
    public decimal Rating { get; set; } // Изменил на decimal для точности
    public int RatingsCount { get; set; }

    // Метаданные
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsPublished { get; set; } = true;
    public DateTime? PublishedAt { get; set; }

    // Навигационные свойства
    public User Author { get; set; } = null!;
    public Category Category { get; set; } = null!;
    public ICollection<Tag> Tags { get; set; } = [];
    public ICollection<MasterclassMaterial> Materials { get; set; } = [];
    public ICollection<UserFavoriteMasterclass> FavoritedBy { get; set; } = []; // Обратная связь
}
