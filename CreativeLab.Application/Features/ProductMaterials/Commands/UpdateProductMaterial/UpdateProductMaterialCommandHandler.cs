using CreativeLab.Application.Interfaces;
using CreativeLab.Domain;
using MediatR;

namespace CreativeLab.Application.Features.ProductMaterials.Commands.UpdateProductMaterial;

public class UpdateProductMaterialCommandHandler(ICreativeLabDbContext dbContext)
    : IRequestHandler<UpdateProductMaterialCommand, ProductMaterial>
{
    public async Task<ProductMaterial> Handle(UpdateProductMaterialCommand request, CancellationToken cancellationToken)
    {
        var material = await dbContext.ProductMaterials
            .FindAsync([request.Id], cancellationToken)
            ?? throw new InvalidOperationException("ProductMaterial with this Id does not exist");

        material.Name = request.Name;
        material.Description = request.Description;

        await dbContext.SaveChangesAsync(cancellationToken);

        return material;
    }
}
