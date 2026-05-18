using CreativeLab.Application.Interfaces;
using CreativeLab.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CreativeLab.Application.Features.Products.Commands.UpdateProduct;

public class UpdateProductCommandHandler(ICreativeLabDbContext dbContext)
    : IRequestHandler<UpdateProductCommand, Product>
{
    public async Task<Product> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await dbContext.Products
            .Include(p => p.Materials)
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken)
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

        // Обновляем материалы
        if (request.Materials != null)
        {
            // Очищаем текущие материалы
            product.Materials.Clear();

            // Обрабатываем новые материалы
            var materialNames = request.Materials
                .Where(m => !string.IsNullOrWhiteSpace(m))
                .Select(m => m.Trim())
                .Distinct()
                .ToList();

            if (materialNames.Count > 0)
            {
                // Получаем существующие материалы
                var existingMaterials = await dbContext.ProductMaterials
                    .Where(m => materialNames.Contains(m.Name))
                    .ToListAsync(cancellationToken);

                var existingMaterialNames = existingMaterials.Select(m => m.Name).ToHashSet();

                // Создаем новые материалы, которых нет в базе
                var newMaterialNames = materialNames.Except(existingMaterialNames).ToList();
                var newMaterials = newMaterialNames
                    .Select(name => new ProductMaterial
                    {
                        Id = Guid.NewGuid(),
                        Name = name,
                    })
                    .ToList();

                if (newMaterials.Count > 0)
                {
                    await dbContext.ProductMaterials.AddRangeAsync(newMaterials, cancellationToken);
                    await dbContext.SaveChangesAsync(cancellationToken);
                }

                // Объединяем существующие и новые материалы
                var allMaterials = existingMaterials.Concat(newMaterials).ToList();

                // Добавляем материалы в коллекцию
                foreach (var material in allMaterials)
                {
                    product.Materials.Add(material);
                }
            }
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        return product;
    }
}
