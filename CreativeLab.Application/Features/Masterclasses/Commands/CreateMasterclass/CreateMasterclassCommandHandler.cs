using CreativeLab.Application.Interfaces;
using CreativeLab.Domain;
using MediatR;

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
            AuthorId = request.AuthorId,
            ImageUrls = request.ImageUrls,
            ThumbnailUrl = request.ThumbnailUrl,
            IsPublished = request.IsPublished,
            PublishedAt = request.IsPublished ? DateTime.UtcNow : null,
            CreatedAt = DateTime.UtcNow
        };

        await dbContext.Masterclasses.AddAsync(masterclass, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return masterclass;
    }
}
