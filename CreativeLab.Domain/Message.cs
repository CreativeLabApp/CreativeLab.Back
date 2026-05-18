namespace CreativeLab.Domain;

public class Message
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ChatId { get; set; }
    public Guid SenderId { get; set; }
    public Guid ReceiverId { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsRead { get; set; }
    public bool IsDeleted { get; set; }
    public Guid? ReplyToMessageId { get; set; }

    // Навигационные свойства
    public Chat Chat { get; set; } = null!;
    public User Sender { get; set; } = null!;
    public User Receiver { get; set; } = null!;
    public Message? ReplyTo { get; set; }

    // Обратные связи
    public ICollection<Message> Replies { get; set; } = [];
}