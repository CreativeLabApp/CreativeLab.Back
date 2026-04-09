using CreativeLab.Application.Interfaces;
using MediatR;

namespace CreativeLab.Application.Features.Messages.Commands.DeleteMessage;

public class DeleteMessageCommand : IRequest
{
    public Guid Id { get; set; }
}

public class DeleteMessageCommandHandler(ICreativeLabDbContext dbContext)
    : IRequestHandler<DeleteMessageCommand>
{
    public async Task Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
    {
        var message = await dbContext.Messages
            .FindAsync([request.Id], cancellationToken)
            ?? throw new InvalidOperationException("Message with this Id does not exist");

        // Soft delete
        message.IsDeleted = true;
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
