using CreativeLab.Domain;
using MediatR;

namespace CreativeLab.Application.Features.Messages.Commands.CreateMessage;

public class CreateMessageCommand : IRequest<Message>
{
    public Guid ChatId { get; set; }
    public Guid SenderId { get; set; }
    public string Content { get; set; } = string.Empty;
    public Guid? ReplyToMessageId { get; set; }
}
