using CreativeLab.Application.Interfaces;
using CreativeLab.Domain;
using MediatR;

namespace CreativeLab.Application.Features.Products.Commands.UpdateProduct;

public class UpdateProductCommandHandler(ICreativeLabDbContext dbContext)
    : IRequestHandler<UpdateProductCommand, Product>
{
    public async Task<Product> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await dbContext.Products
            .FindAsync([request.Id], cancellationToken)
            ?? throw new InvalidOperationException("Product with this Id does not exist");

        product.Title = request.Title;
        product.Description = request.Description;
        product.ShortDescription = request.ShortDescription;
        product.Price = request.Price;
        product.DiscountPrice = request.DiscountPrice;
        product.SKU = request.SKU;
        product.StockQuantity = request.StockQuantity;
        product.IsAvailable = request.IsAvailable;
        product.CategoryId = request.CategoryId;
        product.ImageUrls = request.ImageUrls;
        product.ThumbnailUrl = request.ThumbnailUrl;
        product.Dimensions = request.Dimensions;
        product.Weight = request.Weight;
        product.UpdatedAt = DateTime.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);

        return product;
    }
}
