using CreativeLab.Application.Interfaces;
using MediatR;

namespace CreativeLab.Application.Features.Masterclasses.Commands.DeleteMasterclass;

public class DeleteMasterclassCommand : IRequest
{
    public Guid Id { get; set; }
}

public class DeleteMasterclassCommandHandler(ICreativeLabDbContext dbContext)
    : IRequestHandler<DeleteMasterclassCommand>
{
    public async Task Handle(DeleteMasterclassCommand request, CancellationToken cancellationToken)
    {
        var masterclass = await dbContext.Masterclasses
            .FindAsync([request.Id], cancellationToken)
            ?? throw new InvalidOperationException("Masterclass with this Id does not exist");

        dbContext.Masterclasses.Remove(masterclass);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
