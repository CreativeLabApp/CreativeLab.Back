namespace CreativeLab.Domain;

public class Chat
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastMessageAt { get; set; }
    public bool IsArchived { get; set; }

    // Навигационные свойства
    public ICollection<ChatParticipant> Participants { get; set; } = [];
    public ICollection<Message> Messages { get; set; } = [];
    public Message? LastMessage { get; set; }
    public Guid? LastMessageId { get; set; }
}
