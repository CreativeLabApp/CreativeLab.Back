namespace CreativeLab.Domain;

public class MasterclassRating
{
    public Guid Id { get; set; }
    public Guid MasterclassId { get; set; }
    public Guid UserId { get; set; }
    public int Score { get; set; } // 1–5
    public string? Comment { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public Masterclass Masterclass { get; set; } = null!;
    public User User { get; set; } = null!;
}
