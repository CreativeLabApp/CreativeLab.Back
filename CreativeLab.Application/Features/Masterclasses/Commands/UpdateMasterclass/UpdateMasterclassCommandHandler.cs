using CreativeLab.Application.Interfaces;
using CreativeLab.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CreativeLab.Application.Features.Masterclasses.Commands.UpdateMasterclass;

public class UpdateMasterclassCommandHandler(ICreativeLabDbContext dbContext)
    : IRequestHandler<UpdateMasterclassCommand, Masterclass>
{
    public async Task<Masterclass> Handle(UpdateMasterclassCommand request, CancellationToken cancellationToken)
    {
        var masterclass = await dbContext.Masterclasses
            .Include(m => m.Materials)
            .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken)
            ?? throw new InvalidOperationException("Masterclass with this Id does not exist");

        masterclass.Title = request.Title;
        masterclass.Description = request.Description;
        masterclass.ShortDescription = request.ShortDescription;
        masterclass.CategoryId = request.CategoryId;
        masterclass.AgeCategoryId = request.AgeCategoryId;
        masterclass.ImageUrls = request.ImageUrls;
        masterclass.ThumbnailUrl = request.ThumbnailUrl;
        masterclass.VideoUrl = request.VideoUrl;
        masterclass.IsPublished = request.IsPublished;
        masterclass.UpdatedAt = DateTime.UtcNow;

        if (request.IsPublished && masterclass.PublishedAt is null)
            masterclass.PublishedAt = DateTime.UtcNow;

        // Обновляем материалы
        if (request.Materials != null)
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

            // Очищаем и добавляем новые материалы
            masterclass.Materials.Clear();
            foreach (var material in allMaterials)
            {
                masterclass.Materials.Add(material);
            }
        }
        else
        {
            masterclass.Materials.Clear();
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        return masterclass;
    }
}
