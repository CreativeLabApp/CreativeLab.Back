using AutoMapper;
using CreativeLab.Application.Common.Mappings;
using CreativeLab.Domain;

namespace CreativeLab.Application.Features.Products.Queries.GetProductDetails;

public class ProductDetailsVm : IMapWith<Product>
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ShortDescription { get; set; }
    public Guid SellerId { get; set; }
    public string SellerName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal? DiscountPrice { get; set; }
    public int StockQuantity { get; set; }
    public bool IsAvailable { get; set; }
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string[] ImageUrls { get; set; } = [];
    public string? ThumbnailUrl { get; set; }
    public string? Dimensions { get; set; }
    public decimal? Weight { get; set; }
    public List<string> Materials { get; set; } = [];
    public DateTime CreatedAt { get; set; }
    public decimal Rating { get; set; }
    public int RatingsCount { get; set; }

    public void Mapping(Profile profile)
        => profile.CreateMap<Product, ProductDetailsVm>()
            .ForMember(d => d.SellerName, opt => opt.MapFrom(s => s.Seller.Name + " " + s.Seller.Surname))
            .ForMember(d => d.CategoryName, opt => opt.MapFrom(s => s.Category.Name))
            .ForMember(d => d.Materials, opt => opt.MapFrom(s => s.Materials.Select(m => m.Name).ToList()));
}
