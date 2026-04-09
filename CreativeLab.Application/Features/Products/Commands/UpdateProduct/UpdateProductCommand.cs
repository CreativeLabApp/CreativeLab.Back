using CreativeLab.Domain;
using MediatR;

namespace CreativeLab.Application.Features.Products.Commands.UpdateProduct;

public class UpdateProductCommand : IRequest<Product>
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ShortDescription { get; set; }
    public decimal Price { get; set; }
    public decimal? DiscountPrice { get; set; }
    public string? SKU { get; set; }
    public int StockQuantity { get; set; }
    public bool IsAvailable { get; set; }
    public Guid CategoryId { get; set; }
    public string[] ImageUrls { get; set; } = [];
    public string? ThumbnailUrl { get; set; }
    public string? Dimensions { get; set; }
    public decimal? Weight { get; set; }
}
