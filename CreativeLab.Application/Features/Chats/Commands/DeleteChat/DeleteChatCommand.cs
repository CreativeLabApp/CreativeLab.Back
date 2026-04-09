using CreativeLab.Application.Interfaces;
using MediatR;

namespace CreativeLab.Application.Features.Chats.Commands.DeleteChat;

public class DeleteChatCommand : IRequest
{
    public Guid Id { get; set; }
}

public class DeleteChatCommandHandler(ICreativeLabDbContext dbContext)
    : IRequestHandler<DeleteChatCommand>
{
    public async Task Handle(DeleteChatCommand request, CancellationToken cancellationToken)
    {
        var chat = await dbContext.Chats
            .FindAsync([request.Id], cancellationToken)
            ?? throw new InvalidOperationException("Chat with this Id does not exist");

        dbContext.Chats.Remove(chat);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
