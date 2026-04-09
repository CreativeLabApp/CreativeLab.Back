using AutoMapper;
using CreativeLab.Application.Interfaces;
using MediatR;

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
            .FindAsync([request.Id], cancellationToken)
            ?? throw new InvalidOperationException("Product with this Id does not exist");

        return mapper.Map<ProductDetailsVm>(product);
    }
}
