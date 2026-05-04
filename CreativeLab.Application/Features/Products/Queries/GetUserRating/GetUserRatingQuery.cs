using CreativeLab.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CreativeLab.Application.Features.Products.Queries.GetUserRating;

public class UserProductRatingDto
{
    public int Score { get; set; }
    public string? Comment { get; set; }
}

public class GetUserRatingQuery : IRequest<UserProductRatingDto?>
{
    public Guid ProductId { get; set; }
    public Guid UserId { get; set; }
}

public class GetUserRatingQueryHandler(ICreativeLabDbContext dbContext)
    : IRequestHandler<GetUserRatingQuery, UserProductRatingDto?>
{
    public async Task<UserProductRatingDto?> Handle(GetUserRatingQuery request, CancellationToken cancellationToken)
    {
        var rating = await dbContext.ProductRatings
            .FirstOrDefaultAsync(r => r.ProductId == request.ProductId && r.UserId == request.UserId, cancellationToken);

        if (rating is null) 
            return new UserProductRatingDto { Score = 0, Comment = ""};

        return new UserProductRatingDto { Score = rating.Score, Comment = rating.Comment };
    }
}
