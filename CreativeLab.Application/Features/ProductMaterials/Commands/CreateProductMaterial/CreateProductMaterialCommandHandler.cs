using CreativeLab.Application.Interfaces;
using CreativeLab.Domain;
using MediatR;

namespace CreativeLab.Application.Features.ProductMaterials.Commands.CreateProductMaterial;

public class CreateProductMaterialCommandHandler(ICreativeLabDbContext dbContext)
    : IRequestHandler<CreateProductMaterialCommand, ProductMaterial>
{
    public async Task<ProductMaterial> Handle(CreateProductMaterialCommand request, CancellationToken cancellationToken)
    {
        var material = new ProductMaterial
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description
        };

        await dbContext.ProductMaterials.AddAsync(material, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return material;
    }
}
