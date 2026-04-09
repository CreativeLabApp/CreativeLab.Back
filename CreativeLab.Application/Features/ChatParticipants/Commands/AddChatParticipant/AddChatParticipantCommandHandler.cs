using CreativeLab.Application.Interfaces;
using CreativeLab.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CreativeLab.Application.Features.ChatParticipants.Commands.AddChatParticipant;

public class AddChatParticipantCommandHandler(ICreativeLabDbContext dbContext)
    : IRequestHandler<AddChatParticipantCommand, ChatParticipant>
{
    public async Task<ChatParticipant> Handle(AddChatParticipantCommand request, CancellationToken cancellationToken)
    {
        var alreadyExists = await dbContext.ChatParticipants
            .AnyAsync(p => p.ChatId == request.ChatId && p.UserId == request.UserId, cancellationToken);

        if (alreadyExists)
            throw new InvalidOperationException("User is already a participant of this chat");

        var participant = new ChatParticipant
        {
            Id = Guid.NewGuid(),
            ChatId = request.ChatId,
            UserId = request.UserId,
            JoinedAt = DateTime.UtcNow
        };

        await dbContext.ChatParticipants.AddAsync(participant, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return participant;
    }
}
