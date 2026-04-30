using AutoMapper;
using AutoMapper.QueryableExtensions;
using CreativeLab.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CreativeLab.Application.Features.Products.Queries.GetProductList;

public class GetProductListQuery : IRequest<ProductListVm>
{
    public Guid? SellerId { get; set; }
    public bool OnlyAvailable { get; set; } = true;
}

public class ProductLookupDto
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
    public decimal Rating { get; set; }
    public int RatingsCount { get; set; }
}

public class ProductListVm
{
    public IList<ProductLookupDto> Products { get; set; } = [];
}

public class GetProductListQueryHandler(ICreativeLabDbContext dbContext, IMapper mapper)
    : IRequestHandler<GetProductListQuery, ProductListVm>
{
    public async Task<ProductListVm> Handle(GetProductListQuery request, CancellationToken cancellationToken)
    {
        var query = dbContext.Products.AsQueryable();

        if (request.OnlyAvailable)
            query = query.Where(p => p.IsAvailable);

        if (request.SellerId.HasValue)
            query = query.Where(p => p.SellerId == request.SellerId.Value);

        var products = await query
            .Include(p => p.Seller)
            .Include(p => p.Category)
            .Include(p => p.Materials)
            .OrderByDescending(p => p.CreatedAt)
            .Select(p => new ProductLookupDto
            {
                Id = p.Id,
                Title = p.Title,
                ShortDescription = p.ShortDescription,
                Description = p.Description,
                SellerId = p.SellerId,
                SellerName = p.Seller.Name + " " + p.Seller.Surname,
                CategoryId = p.CategoryId,
                CategoryName = p.Category.Name,
                Price = p.Price,
                DiscountPrice = p.DiscountPrice,
                ImageUrls = p.ImageUrls,
                ThumbnailUrl = p.ThumbnailUrl,
                IsAvailable = p.IsAvailable,
                StockQuantity = p.StockQuantity,
                Dimensions = p.Dimensions,
                Weight = p.Weight,
                CreatedAt = p.CreatedAt,
                Materials = p.Materials.Select(m => m.Name).ToList(),
                Rating = p.Rating,
                RatingsCount = p.RatingsCount,
            })
            .ToListAsync(cancellationToken);

        return new ProductListVm { Products = products };
    }
}
