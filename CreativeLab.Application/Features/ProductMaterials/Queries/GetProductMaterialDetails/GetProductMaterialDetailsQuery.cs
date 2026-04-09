using AutoMapper;
using CreativeLab.Application.Interfaces;
using MediatR;

namespace CreativeLab.Application.Features.ProductMaterials.Queries.GetProductMaterialDetails;

public class GetProductMaterialDetailsQuery : IRequest<ProductMaterialDetailsVm>
{
    public Guid Id { get; set; }
}

public class GetProductMaterialDetailsQueryHandler(ICreativeLabDbContext dbContext, IMapper mapper)
    : IRequestHandler<GetProductMaterialDetailsQuery, ProductMaterialDetailsVm>
{
    public async Task<ProductMaterialDetailsVm> Handle(GetProductMaterialDetailsQuery request, CancellationToken cancellationToken)
    {
        var material = await dbContext.ProductMaterials
            .FindAsync([request.Id], cancellationToken)
            ?? throw new InvalidOperationException("ProductMaterial with this Id does not exist");

        return mapper.Map<ProductMaterialDetailsVm>(material);
    }
}
