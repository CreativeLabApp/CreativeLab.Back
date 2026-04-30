using AutoMapper;
using CreativeLab.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CreativeLab.Application.Features.Users.Queries.GetUserDetails;

public class GetUserDetailsQuery : IRequest<UserDetailsVm>
{
    public Guid Id { get; set; }
}

public class GetUserDetailsQueryHandler(ICreativeLabDbContext dbContext, IMapper mapper)
    : IRequestHandler<GetUserDetailsQuery, UserDetailsVm>
{
    public async Task<UserDetailsVm> Handle(GetUserDetailsQuery request, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users
            .Include(u => u.CreatedMasterclasses)
            .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken)
            ?? throw new InvalidOperationException("User with this Id does not exist");

        return mapper.Map<UserDetailsVm>(user);
    }
}
