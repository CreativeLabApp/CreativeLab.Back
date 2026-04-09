using CreativeLab.Application.Interfaces;
using MediatR;

namespace CreativeLab.Application.Features.Users.Commands.DeleteUser;

public class DeleteUserCommand : IRequest
{
    public Guid Id { get; set; }
}

public class DeleteUserCommandHandler(ICreativeLabDbContext dbContext)
    : IRequestHandler<DeleteUserCommand>
{
    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users
            .FindAsync([request.Id], cancellationToken)
            ?? throw new InvalidOperationException("User with this Id does not exists");

        dbContext.Users.Remove(user);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
