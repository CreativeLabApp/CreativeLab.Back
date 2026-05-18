using CreativeLab.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CreativeLab.Application.Features.Masterclasses.Queries.GetAllRatings;

public class AllRatingsDto
{
    public Guid Id { get; set; }
    public Guid MasterclassId { get; set; }
    public string MasterclassTitle { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public int Score { get; set; }
    public string? Comment { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class GetAllRatingsQuery : IRequest<List<AllRatingsDto>>
{
}

public class GetAllRatingsQueryHandler(ICreativeLabDbContext dbContext)
    : IRequestHandler<GetAllRatingsQuery, List<AllRatingsDto>>
{
    public async Task<List<AllRatingsDto>> Handle(GetAllRatingsQuery request, CancellationToken cancellationToken)
    {
        return await dbContext.MasterclassRatings
            .Include(r => r.User)
            .Include(r => r.Masterclass)
            .OrderByDescending(r => r.CreatedAt)
            .Select(r => new AllRatingsDto
            {
                Id = r.Id,
                MasterclassId = r.MasterclassId,
                MasterclassTitle = r.Masterclass.Title,
                UserId = r.UserId,
                UserName = r.User.Name + " " + r.User.Surname,
                Score = r.Score,
                Comment = r.Comment,
                CreatedAt = r.CreatedAt,
            })
            .ToListAsync(cancellationToken);
    }
}