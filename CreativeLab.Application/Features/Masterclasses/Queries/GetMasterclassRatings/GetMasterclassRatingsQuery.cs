using CreativeLab.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CreativeLab.Application.Features.Masterclasses.Queries.GetMasterclassRatings;

public class MasterclassRatingDto
{
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public int Score { get; set; }
    public string? Comment { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class GetMasterclassRatingsQuery : IRequest<List<MasterclassRatingDto>>
{
    public Guid MasterclassId { get; set; }
}

public class GetMasterclassRatingsQueryHandler(ICreativeLabDbContext dbContext)
    : IRequestHandler<GetMasterclassRatingsQuery, List<MasterclassRatingDto>>
{
    public async Task<List<MasterclassRatingDto>> Handle(GetMasterclassRatingsQuery request, CancellationToken cancellationToken)
    {
        return await dbContext.MasterclassRatings
            .Where(r => r.MasterclassId == request.MasterclassId)
            .Include(r => r.User)
            .OrderByDescending(r => r.UpdatedAt ?? r.CreatedAt)
            .Select(r => new MasterclassRatingDto
            {
                UserId    = r.UserId,
                UserName  = r.User.Name + " " + r.User.Surname,
                Score     = r.Score,
                Comment   = r.Comment,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt,
            })
            .ToListAsync(cancellationToken);
    }
}
