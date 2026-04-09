using CreativeLab.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CreativeLab.Application.Features.UserFavorites.Commands.RemoveFavoriteMasterclass;

public class RemoveFavoriteMasterclassCommand : IRequest
{
    public Guid UserId { get; set; }
    public Guid MasterclassId { get; set; }
}

public class RemoveFavoriteMasterclassCommandHandler(ICreativeLabDbContext dbContext)
    : IRequestHandler<RemoveFavoriteMasterclassCommand>
{
    public async Task Handle(RemoveFavoriteMasterclassCommand request, CancellationToken cancellationToken)
    {
        var favorite = await dbContext.UserFavoriteMasterclasses
            .FirstOrDefaultAsync(f => f.UserId == request.UserId && f.MasterclassId == request.MasterclassId, cancellationToken)
            ?? throw new InvalidOperationException("Masterclass is not in favorites");

        dbContext.UserFavoriteMasterclasses.Remove(favorite);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
