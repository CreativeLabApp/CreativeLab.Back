using AutoMapper;
using CreativeLab.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CreativeLab.Application.Features.Products.Queries.GetProductDetails;

public class GetProductDetailsQuery : IRequest<ProductDetailsVm>
{
    public Guid Id { get; set; }
}

public class GetProductDetailsQueryHandler(ICreativeLabDbContext dbContext, IMapper mapper)
    : IRequestHandler<GetProductDetailsQuery, ProductDetailsVm>
{
    public async Task<ProductDetailsVm> Handle(GetProductDetailsQuery request, CancellationToken cancellationToken)
    {
        var product = await dbContext.Products
            .Include(p => p.Seller)
            .Include(p => p.Category)
            .Include(p => p.Materials)
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken)
            ?? throw new InvalidOperationException("Product with this Id does not exist");

        var result = mapper.Map<ProductDetailsVm>(product);

        // Получаем мастер-классы того же автора
        var masterclasses = await dbContext.Masterclasses
            .Where(m => m.AuthorId == product.SellerId && m.IsPublished)
            .OrderByDescending(m => m.Rating)
            .Take(5)
            .ToListAsync(cancellationToken);

        result.Masterclasses = masterclasses.Select(m => new MasterclassVm
        {
            Id = m.Id,
            Title = m.Title,
            ThumbnailUrl = m.ThumbnailUrl,
            Rating = m.Rating,
            RatingsCount = m.RatingsCount
        }).ToList();

        return result;
    }
}
