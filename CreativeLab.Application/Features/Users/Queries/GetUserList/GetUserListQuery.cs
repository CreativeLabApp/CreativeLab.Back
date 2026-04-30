using CreativeLab.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CreativeLab.Application.Features.Users.Queries.GetUserList;

public class GetUserListQuery : IRequest<List<UserLookupDto>> { }

public class UserLookupDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public int MasterclassesCount { get; set; }
    public int ProductsCount { get; set; }
    public bool IsAdmin { get; set; }
    public bool IsActive { get; set; }
}

public class GetUserListQueryHandler(ICreativeLabDbContext dbContext)
    : IRequestHandler<GetUserListQuery, List<UserLookupDto>>
{
    public async Task<List<UserLookupDto>> Handle(GetUserListQuery request, CancellationToken cancellationToken)
    {
        return await dbContext.Users
            .OrderBy(u => u.CreatedAt)
            .Select(u => new UserLookupDto
            {
                Id = u.Id,
                Name = u.Name,
                Surname = u.Surname,
                Email = u.Email,
                CreatedAt = u.CreatedAt,
                MasterclassesCount = u.CreatedMasterclasses.Count,
                ProductsCount = u.CreatedProducts.Count,
                IsAdmin = u.Email == "admin@creativelab.com",
                IsActive = u.IsActive,
            })
            .ToListAsync(cancellationToken);
    }
}
