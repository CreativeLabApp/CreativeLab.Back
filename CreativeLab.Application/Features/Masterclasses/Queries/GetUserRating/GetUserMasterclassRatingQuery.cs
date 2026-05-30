using CreativeLab.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CreativeLab.Application.Features.Masterclasses.Queries.GetUserRating;

public class UserMasterclassRatingDto
{
    public Guid Id { get; set; }
    public int Score { get; set; }
    public string? Comment { get; set; }
}

public class GetUserMasterclassRatingQuery : IRequest<UserMasterclassRatingDto?>
{
    public Guid MasterclassId { get; set; }
    public Guid UserId { get; set; }
}

public class GetUserMasterclassRatingQueryHandler(ICreativeLabDbContext dbContext)
    : IRequestHandler<GetUserMasterclassRatingQuery, UserMasterclassRatingDto?>
{
    public async Task<UserMasterclassRatingDto?> Handle(GetUserMasterclassRatingQuery request, CancellationToken cancellationToken)
    {
        var rating = await dbContext.MasterclassRatings
            .FirstOrDefaultAsync(r => r.MasterclassId == request.MasterclassId && r.UserId == request.UserId, cancellationToken);

        if (rating is null) return null;

        return new UserMasterclassRatingDto { Id = rating.Id, Score = rating.Score, Comment = rating.Comment };
    }
}
