using CreativeLab.Application.Interfaces;
using CreativeLab.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CreativeLab.Application.Features.Products.Commands.RateProduct;

public class RateProductCommand : IRequest<decimal>
{
    public Guid ProductId { get; set; }
    public Guid UserId { get; set; }
    public int Score { get; set; } // 1–5
}

public class RateProductCommandHandler(ICreativeLabDbContext dbContext)
    : IRequestHandler<RateProductCommand, decimal>
{
    public async Task<decimal> Handle(RateProductCommand request, CancellationToken cancellationToken)
    {
        if (request.Score < 1 || request.Score > 5)
            throw new ArgumentException("Score must be between 1 and 5");

        if (request.UserId == Guid.Empty)
            throw new InvalidOperationException("UserId cannot be empty. User must be authenticated.");

        var product = await dbContext.Products
            .FindAsync([request.ProductId], cancellationToken)
            ?? throw new InvalidOperationException("Product not found");

        // Проверяем, существует ли уже рейтинг пользователя для этого продукта
        var existingRating = await dbContext.ProductRatings
            .FirstOrDefaultAsync(r => r.ProductId == request.ProductId && r.UserId == request.UserId, cancellationToken);

        if (existingRating is not null)
        {
            // Обновляем существующий рейтинг
            var oldScore = existingRating.Score;
            existingRating.Score = request.Score;
            existingRating.UpdatedAt = DateTime.UtcNow;

            // Пересчитываем рейтинг продукта
            product.Rating = (product.Rating * product.RatingsCount - oldScore + request.Score) / product.RatingsCount;
            product.UpdatedAt = DateTime.UtcNow;
        }
        else
        {
            // Проверяем, существует ли пользователь
            var user = await dbContext.Users.FindAsync([request.UserId], cancellationToken);
            if (user is null)
                throw new InvalidOperationException($"User with Id {request.UserId} not found");

            // Создаем новый рейтинг
            var newRating = new ProductRating
            {
                Id = Guid.NewGuid(),
                ProductId = request.ProductId,
                UserId = request.UserId,
                Score = request.Score,
                CreatedAt = DateTime.UtcNow
            };

            dbContext.ProductRatings.Add(newRating);
            product.RatingsCount++;
            product.Rating = (product.Rating * (product.RatingsCount - 1) + request.Score) / product.RatingsCount;
            product.UpdatedAt = DateTime.UtcNow;
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        return product.Rating;
    }
}
