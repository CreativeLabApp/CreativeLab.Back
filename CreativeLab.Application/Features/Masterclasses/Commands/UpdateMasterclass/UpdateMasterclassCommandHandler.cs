using CreativeLab.Application.Interfaces;
using CreativeLab.Domain;
using MediatR;

namespace CreativeLab.Application.Features.Masterclasses.Commands.UpdateMasterclass;

public class UpdateMasterclassCommandHandler(ICreativeLabDbContext dbContext)
    : IRequestHandler<UpdateMasterclassCommand, Masterclass>
{
    public async Task<Masterclass> Handle(UpdateMasterclassCommand request, CancellationToken cancellationToken)
    {
        var masterclass = await dbContext.Masterclasses
            .FindAsync([request.Id], cancellationToken)
            ?? throw new InvalidOperationException("Masterclass with this Id does not exist");

        masterclass.Title = request.Title;
        masterclass.Description = request.Description;
        masterclass.ShortDescription = request.ShortDescription;
        masterclass.CategoryId = request.CategoryId;
        masterclass.ImageUrls = request.ImageUrls;
        masterclass.ThumbnailUrl = request.ThumbnailUrl;
        masterclass.IsPublished = request.IsPublished;
        masterclass.UpdatedAt = DateTime.UtcNow;

        if (request.IsPublished && masterclass.PublishedAt is null)
            masterclass.PublishedAt = DateTime.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);

        return masterclass;
    }
}
