using CreativeLab.Domain;
using MediatR;

namespace CreativeLab.Application.Features.Products.Commands.CreateProduct;

public class CreateProductCommand : IRequest<Product>
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ShortDescription { get; set; }
    public Guid SellerId { get; set; }
    public decimal Price { get; set; }
    public decimal? DiscountPrice { get; set; }
    public string? SKU { get; set; }
    public int StockQuantity { get; set; } = 1;
    public bool IsAvailable { get; set; } = true;
    public Guid? CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public string[] ImageUrls { get; set; } = [];
    public string? ThumbnailUrl { get; set; }
    public string? Dimensions { get; set; }
    public decimal? Weight { get; set; }
    public List<string> Materials { get; set; } = [];
}
