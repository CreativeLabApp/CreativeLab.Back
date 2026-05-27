using CreativeLab.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CreativeLab.Application.Features.Masterclasses.Commands.IncrementMasterclassViews;

public class IncrementMasterclassViewsCommandHandler(ICreativeLabDbContext dbContext)
    : IRequestHandler<IncrementMasterclassViewsCommand, int>
{
    public async Task<int> Handle(IncrementMasterclassViewsCommand request, CancellationToken cancellationToken)
    {
        var masterclass = await dbContext.Masterclasses
            .FirstOrDefaultAsync(m => m.Id == request.MasterclassId, cancellationToken)
            ?? throw new InvalidOperationException("Masterclass with this Id does not exist");

        masterclass.Views++;
        await dbContext.SaveChangesAsync(cancellationToken);

        return masterclass.Views;
    }
}
