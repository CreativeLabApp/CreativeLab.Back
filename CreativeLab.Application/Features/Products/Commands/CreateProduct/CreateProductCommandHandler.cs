using CreativeLab.Application.Interfaces;
using CreativeLab.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CreativeLab.Application.Features.Products.Commands.CreateProduct;

public class CreateProductCommandHandler(ICreativeLabDbContext dbContext)
    : IRequestHandler<CreateProductCommand, Product>
{
    public async Task<Product> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var categoryId = request.CategoryId;

        // Если categoryId не передан — ищем по имени или создаём
        if (categoryId is null && !string.IsNullOrWhiteSpace(request.CategoryName))
        {
            var existing = await dbContext.Categories
                .FirstOrDefaultAsync(c => c.Name.ToLower() == request.CategoryName.ToLower(), cancellationToken);

            if (existing is not null)
            {
                categoryId = existing.Id;
            }
            else
            {
                var newCategory = new Category
                {
                    Id = Guid.NewGuid(),
                    Name = request.CategoryName.Trim(),
                    Order = 99,
                };
                await dbContext.Categories.AddAsync(newCategory, cancellationToken);
                await dbContext.SaveChangesAsync(cancellationToken);
                categoryId = newCategory.Id;
            }
        }

        if (categoryId is null)
            throw new InvalidOperationException("CategoryId or CategoryName is required");

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            ShortDescription = request.ShortDescription,
            SellerId = request.SellerId,
            Price = request.Price,
            DiscountPrice = request.DiscountPrice,
            SKU = request.SKU,
            StockQuantity = request.StockQuantity,
            IsAvailable = request.IsAvailable,
            CategoryId = categoryId.Value,
            ImageUrls = request.ImageUrls,
            ThumbnailUrl = request.ThumbnailUrl,
            Dimensions = request.Dimensions,
            Weight = request.Weight,
            CreatedAt = DateTime.UtcNow,
        };

        await dbContext.Products.AddAsync(product, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return product;
    }
}
