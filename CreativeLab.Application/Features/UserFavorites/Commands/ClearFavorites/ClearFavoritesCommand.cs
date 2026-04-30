using CreativeLab.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CreativeLab.Application.Features.UserFavorites.Commands.ClearFavorites;

public class ClearFavoritesCommand : IRequest
{
    public Guid UserId { get; set; }
}

public class ClearFavoritesCommandHandler(ICreativeLabDbContext dbContext)
    : IRequestHandler<ClearFavoritesCommand>
{
    public async Task Handle(ClearFavoritesCommand request, CancellationToken cancellationToken)
    {
        var masterclasses = await dbContext.UserFavoriteMasterclasses
            .Where(f => f.UserId == request.UserId)
            .ToListAsync(cancellationToken);

        var products = await dbContext.UserFavoriteProducts
            .Where(f => f.UserId == request.UserId)
            .ToListAsync(cancellationToken);

        dbContext.UserFavoriteMasterclasses.RemoveRange(masterclasses);
        dbContext.UserFavoriteProducts.RemoveRange(products);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
