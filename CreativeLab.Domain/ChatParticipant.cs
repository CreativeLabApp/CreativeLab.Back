namespace CreativeLab.Domain;

public class ChatParticipant
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ChatId { get; set; }
    public Guid UserId { get; set; }
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastReadAt { get; set; }
    public bool IsMuted { get; set; }
    public bool IsPinned { get; set; }

    // Навигационные свойства
    public Chat Chat { get; set; } = null!;
    public User User { get; set; } = null!;
}
