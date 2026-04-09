using CreativeLab.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CreativeLab.Application.Features.ChatParticipants.Commands.RemoveChatParticipant;

public class RemoveChatParticipantCommand : IRequest
{
    public Guid ChatId { get; set; }
    public Guid UserId { get; set; }
}

public class RemoveChatParticipantCommandHandler(ICreativeLabDbContext dbContext)
    : IRequestHandler<RemoveChatParticipantCommand>
{
    public async Task Handle(RemoveChatParticipantCommand request, CancellationToken cancellationToken)
    {
        var participant = await dbContext.ChatParticipants
            .FirstOrDefaultAsync(p => p.ChatId == request.ChatId && p.UserId == request.UserId, cancellationToken)
            ?? throw new InvalidOperationException("Participant not found in this chat");

        dbContext.ChatParticipants.Remove(participant);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
