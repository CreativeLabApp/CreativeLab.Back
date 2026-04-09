using CreativeLab.Application.Interfaces;
using CreativeLab.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CreativeLab.Application.Features.UserFavorites.Commands.AddFavoriteProduct;

public class AddFavoriteProductCommandHandler(ICreativeLabDbContext dbContext)
    : IRequestHandler<AddFavoriteProductCommand, UserFavoriteProduct>
{
    public async Task<UserFavoriteProduct> Handle(AddFavoriteProductCommand request, CancellationToken cancellationToken)
    {
        var alreadyExists = await dbContext.UserFavoriteProducts
            .AnyAsync(f => f.UserId == request.UserId && f.ProductId == request.ProductId, cancellationToken);

        if (alreadyExists)
            throw new InvalidOperationException("Product is already in favorites");

        var favorite = new UserFavoriteProduct
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            ProductId = request.ProductId,
            AddedAt = DateTime.UtcNow
        };

        await dbContext.UserFavoriteProducts.AddAsync(favorite, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return favorite;
    }
}
