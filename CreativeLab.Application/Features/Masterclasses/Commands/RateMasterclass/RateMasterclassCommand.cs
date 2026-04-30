using CreativeLab.Application.Interfaces;
using CreativeLab.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CreativeLab.Application.Features.Masterclasses.Commands.RateMasterclass;

public class RateMasterclassCommand : IRequest<RateMasterclassResult>
{
    public Guid MasterclassId { get; set; }
    public Guid UserId { get; set; }
    public int Score { get; set; } // 1–5
    public string? Comment { get; set; }
}

public class RateMasterclassResult
{
    public decimal Rating { get; set; }
    public int RatingsCount { get; set; }
    public int UserScore { get; set; }
}

public class RateMasterclassCommandHandler(ICreativeLabDbContext dbContext)
    : IRequestHandler<RateMasterclassCommand, RateMasterclassResult>
{
    public async Task<RateMasterclassResult> Handle(RateMasterclassCommand request, CancellationToken cancellationToken)
    {
        if (request.Score < 1 || request.Score > 5)
            throw new ArgumentException("Score must be between 1 and 5");

        var masterclass = await dbContext.Masterclasses
            .FindAsync([request.MasterclassId], cancellationToken)
            ?? throw new InvalidOperationException("Masterclass not found");

        // Upsert оценки пользователя
        var existing = await dbContext.MasterclassRatings
            .FirstOrDefaultAsync(r => r.MasterclassId == request.MasterclassId && r.UserId == request.UserId, cancellationToken);

        if (existing is null)
        {
            await dbContext.MasterclassRatings.AddAsync(new MasterclassRating
            {
                Id = Guid.NewGuid(),
                MasterclassId = request.MasterclassId,
                UserId = request.UserId,
                Score = request.Score,
                Comment = request.Comment,
            }, cancellationToken);
        }
        else
        {
            existing.Score = request.Score;
            existing.Comment = request.Comment;
            existing.UpdatedAt = DateTime.UtcNow;
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        // Пересчёт среднего из таблицы
        var ratings = await dbContext.MasterclassRatings
            .Where(r => r.MasterclassId == request.MasterclassId)
            .ToListAsync(cancellationToken);

        masterclass.Rating = ratings.Count > 0 ? (decimal)ratings.Average(r => r.Score) : 0;
        masterclass.RatingsCount = ratings.Count;
        masterclass.UpdatedAt = DateTime.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);

        return new RateMasterclassResult
        {
            Rating = masterclass.Rating,
            RatingsCount = masterclass.RatingsCount,
            UserScore = request.Score,
        };
    }
}
