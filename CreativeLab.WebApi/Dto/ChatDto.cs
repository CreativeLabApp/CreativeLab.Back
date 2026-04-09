namespace CreativeLab.WebApi.Dto;

public class CreateChatDto
{
    public string Title { get; set; } = string.Empty;
    public List<Guid> ParticipantUserIds { get; set; } = [];
}

public class AddChatParticipantDto
{
    public Guid ChatId { get; set; }
    public Guid UserId { get; set; }
}

public class RemoveChatParticipantDto
{
    public Guid ChatId { get; set; }
    public Guid UserId { get; set; }
}
