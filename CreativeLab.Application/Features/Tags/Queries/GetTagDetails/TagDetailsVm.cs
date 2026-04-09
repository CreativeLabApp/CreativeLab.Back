using AutoMapper;
using CreativeLab.Application.Common.Mappings;
using CreativeLab.Domain;

namespace CreativeLab.Application.Features.Tags.Queries.GetTagDetails;

public class TagDetailsVm : IMapWith<Tag>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Color { get; set; }

    public void Mapping(Profile profile)
        => profile.CreateMap<Tag, TagDetailsVm>();
}
