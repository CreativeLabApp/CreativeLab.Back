using CreativeLab.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CreativeLab.Application.Features.Masterclasses.Commands.DeleteRating;

public class DeleteRatingCommand : IRequest
{
    public Guid Id { get; set; }
}

public class DeleteRatingCommandHandler(ICreativeLabDbContext dbContext)
    : IRequestHandler<DeleteRatingCommand>
{
    public async Task Handle(DeleteRatingCommand request, CancellationToken cancellationToken)
    {
        var rating = await dbContext.MasterclassRatings
            .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

        if (rating == null)
            throw new Exception("Rating not found");

        var masterclassId = rating.MasterclassId;

        dbContext.MasterclassRatings.Remove(rating);
        await dbContext.SaveChangesAsync(cancellationToken);

        // Пересчёт среднего рейтинга мастер-класса
        var masterclass = await dbContext.Masterclasses
            .FindAsync([masterclassId], cancellationToken);

        if (masterclass != null)
        {
            var ratings = await dbContext.MasterclassRatings
                .Where(r => r.MasterclassId == masterclassId)
                .ToListAsync(cancellationToken);

            masterclass.Rating = ratings.Count > 0 ? (decimal)ratings.Average(r => r.Score) : 0;
            masterclass.RatingsCount = ratings.Count;
            masterclass.UpdatedAt = DateTime.UtcNow;

            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}