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

        // Обрабатываем материалы - используем существующие или создаем новые
        if (request.Materials != null && request.Materials.Count > 0)
        {
            var materialNames = request.Materials
                .Where(m => !string.IsNullOrWhiteSpace(m))
                .Select(m => m.Trim())
                .Distinct()
                .ToList();

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

            // Загружаем продукт заново, чтобы получить доступ к коллекции Materials
            var loadedProduct = await dbContext.Products
                .Include(p => p.Materials)
                .FirstAsync(p => p.Id == product.Id, cancellationToken);

            // Добавляем материалы в коллекцию
            foreach (var material in allMaterials)
            {
                loadedProduct.Materials.Add(material);
            }

            await dbContext.SaveChangesAsync(cancellationToken);

            return loadedProduct;
        }

        return product;
    }
}
