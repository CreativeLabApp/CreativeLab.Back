using CreativeLab.Application.Interfaces;
using CreativeLab.Domain;
using MediatR;

namespace CreativeLab.Application.Features.Chats.Commands.CreateChat;

public class CreateChatCommandHandler(ICreativeLabDbContext dbContext)
    : IRequestHandler<CreateChatCommand, Chat>
{
    public async Task<Chat> Handle(CreateChatCommand request, CancellationToken cancellationToken)
    {
        var chat = new Chat
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            CreatedAt = DateTime.UtcNow,
            Participants = request.ParticipantUserIds.Select(userId => new ChatParticipant
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                JoinedAt = DateTime.UtcNow
            }).ToList()
        };

        await dbContext.Chats.AddAsync(chat, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return chat;
    }
}
