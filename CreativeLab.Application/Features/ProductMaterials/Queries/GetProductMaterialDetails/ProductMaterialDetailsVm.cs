using AutoMapper;
using CreativeLab.Application.Common.Mappings;
using CreativeLab.Domain;

namespace CreativeLab.Application.Features.ProductMaterials.Queries.GetProductMaterialDetails;

public class ProductMaterialDetailsVm : IMapWith<ProductMaterial>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public void Mapping(Profile profile)
        => profile.CreateMap<ProductMaterial, ProductMaterialDetailsVm>();
}
