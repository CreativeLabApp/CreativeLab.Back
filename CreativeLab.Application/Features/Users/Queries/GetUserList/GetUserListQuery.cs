using AutoMapper;
using CreativeLab.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CreativeLab.Application.Features.Users.Queries.GetUserList;

public class GetUserListQuery : IRequest<List<UserListItemVm>>
{
    public string? Search { get; set; }
    public Guid? ExcludeUserId { get; set; }
}

public class UserListItemVm
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public class GetUserListQueryHandler(ICreativeLabDbContext dbContext, IMapper mapper)
    : IRequestHandler<GetUserListQuery, List<UserListItemVm>>
{
    public async Task<List<UserListItemVm>> Handle(GetUserListQuery request, CancellationToken cancellationToken)
    {
        var query = dbContext.Users.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var searchLower = request.Search.ToLower();
            query = query.Where(u =>
                u.Name.ToLower().Contains(searchLower) ||
                u.Surname.ToLower().Contains(searchLower) ||
                u.Email.ToLower().Contains(searchLower));
        }

        if (request.ExcludeUserId.HasValue)
        {
            query = query.Where(u => u.Id != request.ExcludeUserId.Value);
        }

        var users = await query
            .OrderBy(u => u.Name)
            .Take(20)
            .ToListAsync(cancellationToken);

        return users.Select(u => new UserListItemVm
        {
            Id = u.Id,
            Name = u.Name,
            Surname = u.Surname,
            Email = u.Email
        }).ToList();
    }
}