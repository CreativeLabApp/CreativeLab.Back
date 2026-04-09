using CreativeLab.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CreativeLab.Application.Features.UserFavorites.Commands.RemoveFavoriteProduct;

public class RemoveFavoriteProductCommand : IRequest
{
    public Guid UserId { get; set; }
    public Guid ProductId { get; set; }
}

public class RemoveFavoriteProductCommandHandler(ICreativeLabDbContext dbContext)
    : IRequestHandler<RemoveFavoriteProductCommand>
{
    public async Task Handle(RemoveFavoriteProductCommand request, CancellationToken cancellationToken)
    {
        var favorite = await dbContext.UserFavoriteProducts
            .FirstOrDefaultAsync(f => f.UserId == request.UserId && f.ProductId == request.ProductId, cancellationToken)
            ?? throw new InvalidOperationException("Product is not in favorites");

        dbContext.UserFavoriteProducts.Remove(favorite);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
