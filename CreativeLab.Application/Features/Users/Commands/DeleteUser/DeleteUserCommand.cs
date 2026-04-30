using CreativeLab.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

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

        // Удаляем связанные мастер-классы
        var masterclasses = await dbContext.Masterclasses
            .Where(m => m.AuthorId == request.Id)
            .ToListAsync(cancellationToken);
        dbContext.Masterclasses.RemoveRange(masterclasses);

        // Удаляем связанные товары
        var products = await dbContext.Products
            .Where(p => p.SellerId == request.Id)
            .ToListAsync(cancellationToken);
        dbContext.Products.RemoveRange(products);

        // Удаляем refresh токены
        var tokens = await dbContext.RefreshTokens
            .Where(t => t.UserId == request.Id)
            .ToListAsync(cancellationToken);
        dbContext.RefreshTokens.RemoveRange(tokens);

        dbContext.Users.Remove(user);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
