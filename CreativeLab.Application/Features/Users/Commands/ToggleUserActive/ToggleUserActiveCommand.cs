using CreativeLab.Application.Interfaces;
using MediatR;

namespace CreativeLab.Application.Features.Users.Commands.ToggleUserActive;

public class ToggleUserActiveCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}

public class ToggleUserActiveCommandHandler(ICreativeLabDbContext dbContext)
    : IRequestHandler<ToggleUserActiveCommand, bool>
{
    public async Task<bool> Handle(ToggleUserActiveCommand request, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users
            .FindAsync([request.Id], cancellationToken)
            ?? throw new InvalidOperationException("User not found");

        user.IsActive = !user.IsActive;
        user.UpdatedAt = DateTime.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);

        return user.IsActive;
    }
}
