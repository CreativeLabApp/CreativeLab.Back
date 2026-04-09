using AutoMapper;
using CreativeLab.Application.Common.Mappings;
using CreativeLab.Domain;

namespace CreativeLab.Application.Features.Categories.Queries.GetCategoryDetails;

public class CategoryDetailsVm : IMapWith<Category>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Order { get; set; }

    public void Mapping(Profile profile)
        => profile.CreateMap<Category, CategoryDetailsVm>();
}
