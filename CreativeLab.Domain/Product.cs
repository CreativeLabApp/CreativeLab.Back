namespace CreativeLab.Domain;

public class Product
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ShortDescription { get; set; } // Для превью

    // Продажа
    public Guid SellerId { get; set; }
    public decimal Price { get; set; }
    public decimal? DiscountPrice { get; set; }
    public string? SKU { get; set; } // Артикул
    public int StockQuantity { get; set; } = 1;
    public bool IsAvailable { get; set; } = true;

    // Категория
    public Guid CategoryId { get; set; }

    // Медиа
    public string[] ImageUrls { get; set; } = [];
    public string? ThumbnailUrl { get; set; } // Главное изображение

    // Характеристики
    public string? Dimensions { get; set; }
    public decimal? Weight { get; set; } // Изменил на decimal

    // Метаданные
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Рейтинг
    public decimal Rating { get; set; }
    public int RatingsCount { get; set; }

    // Навигационные свойства
    public User Seller { get; set; } = null!;
    public Category Category { get; set; } = null!;
    public ICollection<Tag> Tags { get; set; } = [];
    public ICollection<ProductMaterial> Materials { get; set; } = [];
    public ICollection<UserFavoriteProduct> FavoritedBy { get; set; } = []; // Обратная связь
}
