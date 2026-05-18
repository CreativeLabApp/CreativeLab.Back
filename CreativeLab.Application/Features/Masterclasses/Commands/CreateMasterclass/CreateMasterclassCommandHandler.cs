using CreativeLab.Application.Interfaces;
using CreativeLab.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CreativeLab.Application.Features.Masterclasses.Commands.CreateMasterclass;

public class CreateMasterclassCommandHandler(ICreativeLabDbContext dbContext)
    : IRequestHandler<CreateMasterclassCommand, Masterclass>
{
    public async Task<Masterclass> Handle(CreateMasterclassCommand request, CancellationToken cancellationToken)
    {
        var masterclass = new Masterclass
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            ShortDescription = request.ShortDescription,
            CategoryId = request.CategoryId,
            AgeCategoryId = request.AgeCategoryId,
            AuthorId = request.AuthorId,
            ImageUrls = request.ImageUrls,
            ThumbnailUrl = request.ThumbnailUrl,
            VideoUrl = request.VideoUrl,
            IsPublished = request.IsPublished,
            PublishedAt = request.IsPublished ? DateTime.UtcNow : null,
            CreatedAt = DateTime.UtcNow
        };

        await dbContext.Masterclasses.AddAsync(masterclass, cancellationToken);
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
            var existingMaterials = await dbContext.MasterclassMaterials
                .Where(m => materialNames.Contains(m.Name))
                .ToListAsync(cancellationToken);

            var existingMaterialNames = existingMaterials.Select(m => m.Name).ToHashSet();

            // Создаем новые материалы, которых нет в базе
            var newMaterialNames = materialNames.Except(existingMaterialNames).ToList();
            var newMaterials = newMaterialNames
                .Select(name => new MasterclassMaterial
                {
                    Id = Guid.NewGuid(),
                    Name = name,
                })
                .ToList();

            if (newMaterials.Count > 0)
            {
                await dbContext.MasterclassMaterials.AddRangeAsync(newMaterials, cancellationToken);
                await dbContext.SaveChangesAsync(cancellationToken);
            }

            // Объединяем существующие и новые материалы
            var allMaterials = existingMaterials.Concat(newMaterials).ToList();

            // Загружаем мастер-класс заново, чтобы получить доступ к коллекции Materials
            var loadedMasterclass = await dbContext.Masterclasses
                .Include(m => m.Materials)
                .FirstAsync(m => m.Id == masterclass.Id, cancellationToken);

            // Добавляем материалы в коллекцию
            foreach (var material in allMaterials)
            {
                loadedMasterclass.Materials.Add(material);
            }

            await dbContext.SaveChangesAsync(cancellationToken);
        }

        return masterclass;
    }
}
