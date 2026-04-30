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

        return mapper.Map<ProductDetailsVm>(product);
    }
}
