using CreativeLab.Application.Interfaces;
using MediatR;

namespace CreativeLab.Application.Features.Tags.Commands.DeleteTag;

public class DeleteTagCommand : IRequest
{
    public Guid Id { get; set; }
}

public class DeleteTagCommandHandler(ICreativeLabDbContext dbContext)
    : IRequestHandler<DeleteTagCommand>
{
    public async Task Handle(DeleteTagCommand request, CancellationToken cancellationToken)
    {
        var tag = await dbContext.Tags
            .FindAsync([request.Id], cancellationToken)
            ?? throw new InvalidOperationException("Tag with this Id does not exist");

        dbContext.Tags.Remove(tag);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
