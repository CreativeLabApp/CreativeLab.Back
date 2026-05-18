using CreativeLab.Application.Interfaces;
using CreativeLab.Domain;
using MediatR;

namespace CreativeLab.Application.Features.Messages.Commands.CreateMessage;

public class CreateMessageCommandHandler(ICreativeLabDbContext dbContext)
    : IRequestHandler<CreateMessageCommand, Message>
{
    public async Task<Message> Handle(CreateMessageCommand request, CancellationToken cancellationToken)
    {
        var message = new Message
        {
            Id = Guid.NewGuid(),
            ChatId = request.ChatId,
            SenderId = request.SenderId,
            ReceiverId = request.ReceiverId,
            Content = request.Content,
            ReplyToMessageId = request.ReplyToMessageId,
            CreatedAt = DateTime.UtcNow
        };

        await dbContext.Messages.AddAsync(message, cancellationToken);

        var chat = await dbContext.Chats.FindAsync([request.ChatId], cancellationToken);
        if (chat is not null)
        {
            chat.LastMessageAt = message.CreatedAt;
            chat.LastMessageId = message.Id;
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        return message;
    }
}
