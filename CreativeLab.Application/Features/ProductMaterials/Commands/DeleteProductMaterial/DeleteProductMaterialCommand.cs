using CreativeLab.Application.Interfaces;
using MediatR;

namespace CreativeLab.Application.Features.ProductMaterials.Commands.DeleteProductMaterial;

public class DeleteProductMaterialCommand : IRequest
{
    public Guid Id { get; set; }
}

public class DeleteProductMaterialCommandHandler(ICreativeLabDbContext dbContext)
    : IRequestHandler<DeleteProductMaterialCommand>
{
    public async Task Handle(DeleteProductMaterialCommand request, CancellationToken cancellationToken)
    {
        var material = await dbContext.ProductMaterials
            .FindAsync([request.Id], cancellationToken)
            ?? throw new InvalidOperationException("ProductMaterial with this Id does not exist");

        dbContext.ProductMaterials.Remove(material);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
