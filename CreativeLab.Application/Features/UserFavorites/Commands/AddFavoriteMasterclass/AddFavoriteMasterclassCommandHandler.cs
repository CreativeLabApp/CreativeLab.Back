using CreativeLab.Application.Interfaces;
using CreativeLab.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CreativeLab.Application.Features.UserFavorites.Commands.AddFavoriteMasterclass;

public class AddFavoriteMasterclassCommandHandler(ICreativeLabDbContext dbContext)
    : IRequestHandler<AddFavoriteMasterclassCommand, UserFavoriteMasterclass>
{
    public async Task<UserFavoriteMasterclass> Handle(AddFavoriteMasterclassCommand request, CancellationToken cancellationToken)
    {
        var alreadyExists = await dbContext.UserFavoriteMasterclasses
            .AnyAsync(f => f.UserId == request.UserId && f.MasterclassId == request.MasterclassId, cancellationToken);

        if (alreadyExists)
            throw new InvalidOperationException("Masterclass is already in favorites");

        var favorite = new UserFavoriteMasterclass
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            MasterclassId = request.MasterclassId,
            AddedAt = DateTime.UtcNow
        };

        await dbContext.UserFavoriteMasterclasses.AddAsync(favorite, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return favorite;
    }
}
