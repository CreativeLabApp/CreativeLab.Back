using CreativeLab.Domain;
using MediatR;

namespace CreativeLab.Application.Features.ProductMaterials.Commands.CreateProductMaterial;

public class CreateProductMaterialCommand : IRequest<ProductMaterial>
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}
