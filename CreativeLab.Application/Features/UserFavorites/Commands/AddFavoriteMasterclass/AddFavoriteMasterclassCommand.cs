using CreativeLab.Domain;
using MediatR;

namespace CreativeLab.Application.Features.UserFavorites.Commands.AddFavoriteMasterclass;

public class AddFavoriteMasterclassCommand : IRequest<UserFavoriteMasterclass>
{
    public Guid UserId { get; set; }
    public Guid MasterclassId { get; set; }
}
