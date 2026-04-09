using CreativeLab.Domain;
using MediatR;

namespace CreativeLab.Application.Features.UserFavorites.Commands.AddFavoriteProduct;

public class AddFavoriteProductCommand : IRequest<UserFavoriteProduct>
{
    public Guid UserId { get; set; }
    public Guid ProductId { get; set; }
}
