namespace CreativeLab.Domain;

public class UserFavoriteMasterclass
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid MasterclassId { get; set; }
    public DateTime AddedAt { get; set; } = DateTime.UtcNow; // Важно для сортировки

    public Masterclass Masterclass { get; set; } = null!;
    public User User { get; set; } = null!;
}
