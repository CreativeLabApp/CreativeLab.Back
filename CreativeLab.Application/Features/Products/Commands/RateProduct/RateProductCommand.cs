using CreativeLab.Application.Interfaces;
using MediatR;

namespace CreativeLab.Application.Features.Products.Commands.RateProduct;

public class RateProductCommand : IRequest<decimal>
{
    public Guid ProductId { get; set; }
    public int Score { get; set; } // 1–5
}

public class RateProductCommandHandler(ICreativeLabDbContext dbContext)
    : IRequestHandler<RateProductCommand, decimal>
{
    public async Task<decimal> Handle(RateProductCommand request, CancellationToken cancellationToken)
    {
        if (request.Score < 1 || request.Score > 5)
            throw new ArgumentException("Score must be between 1 and 5");

        var product = await dbContext.Products
            .FindAsync([request.ProductId], cancellationToken)
            ?? throw new InvalidOperationException("Product not found");

        var newCount = product.RatingsCount + 1;
        product.Rating = (product.Rating * product.RatingsCount + request.Score) / newCount;
        product.RatingsCount = newCount;
        product.UpdatedAt = DateTime.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);

        return product.Rating;
    }
}
