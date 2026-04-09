using CreativeLab.Application.Interfaces;
using CreativeLab.Domain;
using MediatR;

namespace CreativeLab.Application.Features.Products.Commands.CreateProduct;

public class CreateProductCommandHandler(ICreativeLabDbContext dbContext)
    : IRequestHandler<CreateProductCommand, Product>
{
    public async Task<Product> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
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
            CategoryId = request.CategoryId,
            ImageUrls = request.ImageUrls,
            ThumbnailUrl = request.ThumbnailUrl,
            Dimensions = request.Dimensions,
            Weight = request.Weight,
            CreatedAt = DateTime.UtcNow
        };

        await dbContext.Products.AddAsync(product, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return product;
    }
}
