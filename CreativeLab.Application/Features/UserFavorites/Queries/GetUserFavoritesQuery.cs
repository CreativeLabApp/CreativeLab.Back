using CreativeLab.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CreativeLab.Application.Features.UserFavorites.Queries;

public class FavoriteMasterclassDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? ShortDescription { get; set; }
    public string? Description { get; set; }
    public Guid AuthorId { get; set; }
    public string AuthorName { get; set; } = string.Empty;
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string[] ImageUrls { get; set; } = [];
    public string? ThumbnailUrl { get; set; }
    public decimal Rating { get; set; }
    public int Views { get; set; }
    public bool IsPublished { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<string> Materials { get; set; } = [];
}

public class FavoriteProductDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? ShortDescription { get; set; }
    public string? Description { get; set; }
    public Guid SellerId { get; set; }
    public string SellerName { get; set; } = string.Empty;
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal? DiscountPrice { get; set; }
    public string[] ImageUrls { get; set; } = [];
    public string? ThumbnailUrl { get; set; }
    public bool IsAvailable { get; set; }
    public int StockQuantity { get; set; }
    public string? Dimensions { get; set; }
    public decimal? Weight { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<string> Materials { get; set; } = [];
}

public class UserFavoritesVm
{
    public List<FavoriteMasterclassDto> Masterclasses { get; set; } = [];
    public List<FavoriteProductDto> Products { get; set; } = [];
}

public class GetUserFavoritesQuery : IRequest<UserFavoritesVm>
{
    public Guid UserId { get; set; }
}

public class GetUserFavoritesQueryHandler(ICreativeLabDbContext dbContext)
    : IRequestHandler<GetUserFavoritesQuery, UserFavoritesVm>
{
    public async Task<UserFavoritesVm> Handle(GetUserFavoritesQuery request, CancellationToken cancellationToken)
    {
        var masterclasses = await dbContext.UserFavoriteMasterclasses
            .Where(f => f.UserId == request.UserId)
            .Include(f => f.Masterclass).ThenInclude(m => m.Author)
            .Include(f => f.Masterclass).ThenInclude(m => m.Category)
            .Include(f => f.Masterclass).ThenInclude(m => m.Materials)
            .OrderByDescending(f => f.AddedAt)
            .Select(f => new FavoriteMasterclassDto
            {
                Id               = f.Masterclass.Id,
                Title            = f.Masterclass.Title,
                ShortDescription = f.Masterclass.ShortDescription,
                Description      = f.Masterclass.Description,
                AuthorId         = f.Masterclass.AuthorId,
                AuthorName       = f.Masterclass.Author.Name + " " + f.Masterclass.Author.Surname,
                CategoryId       = f.Masterclass.CategoryId,
                CategoryName     = f.Masterclass.Category.Name,
                ImageUrls        = f.Masterclass.ImageUrls,
                ThumbnailUrl     = f.Masterclass.ThumbnailUrl,
                Rating           = f.Masterclass.Rating,
                Views            = f.Masterclass.Views,
                IsPublished      = f.Masterclass.IsPublished,
                CreatedAt        = f.Masterclass.CreatedAt,
                Materials        = f.Masterclass.Materials.Select(m => m.Name).ToList(),
            })
            .ToListAsync(cancellationToken);

        var products = await dbContext.UserFavoriteProducts
            .Where(f => f.UserId == request.UserId)
            .Include(f => f.Product).ThenInclude(p => p.Seller)
            .Include(f => f.Product).ThenInclude(p => p.Category)
            .Include(f => f.Product).ThenInclude(p => p.Materials)
            .OrderByDescending(f => f.AddedAt)
            .Select(f => new FavoriteProductDto
            {
                Id               = f.Product.Id,
                Title            = f.Product.Title,
                ShortDescription = f.Product.ShortDescription,
                Description      = f.Product.Description,
                SellerId         = f.Product.SellerId,
                SellerName       = f.Product.Seller.Name + " " + f.Product.Seller.Surname,
                CategoryId       = f.Product.CategoryId,
                CategoryName     = f.Product.Category.Name,
                Price            = f.Product.Price,
                DiscountPrice    = f.Product.DiscountPrice,
                ImageUrls        = f.Product.ImageUrls,
                ThumbnailUrl     = f.Product.ThumbnailUrl,
                IsAvailable      = f.Product.IsAvailable,
                StockQuantity    = f.Product.StockQuantity,
                Dimensions       = f.Product.Dimensions,
                Weight           = f.Product.Weight,
                CreatedAt        = f.Product.CreatedAt,
                Materials        = f.Product.Materials.Select(m => m.Name).ToList(),
            })
            .ToListAsync(cancellationToken);

        return new UserFavoritesVm { Masterclasses = masterclasses, Products = products };
    }
}
