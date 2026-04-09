using CreativeLab.Domain;
using MediatR;

namespace CreativeLab.Application.Features.ProductMaterials.Commands.UpdateProductMaterial;

public class UpdateProductMaterialCommand : IRequest<ProductMaterial>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}
