using AutoMapper;
using CreativeLab.Application.Common.Mappings;
using CreativeLab.Domain;

namespace CreativeLab.Application.Features.Users.Queries.GetUserDetails;

public class UserDetailsVm : IMapWith<User>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public ICollection<UserFavoriteMasterclass> FavoriteMasterclasses { get; set; } = [];
    public ICollection<UserFavoriteProduct> FavoriteProducts { get; set; } = [];
    public ICollection<Masterclass> CreatedMasterclasses { get; set; } = [];
    public ICollection<Product> CreatedProducts { get; set; } = [];

    public void Mapping(Profile profile)
        => profile.CreateMap<User, UserDetailsVm>();
}