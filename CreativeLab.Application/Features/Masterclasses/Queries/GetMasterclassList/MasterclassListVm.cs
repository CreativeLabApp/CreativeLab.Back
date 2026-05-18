using AutoMapper;
using CreativeLab.Application.Common.Mappings;
using CreativeLab.Domain;

namespace CreativeLab.Application.Features.Masterclasses.Queries.GetMasterclassList;

public class MasterclassLookupDto : IMapWith<Masterclass>
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? ShortDescription { get; set; }
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public Guid AgeCategoryId { get; set; }
    public string AgeCategoryName { get; set; } = string.Empty;
    public Guid AuthorId { get; set; }
    public string AuthorName { get; set; } = string.Empty;
    public string? ThumbnailUrl { get; set; }
    public string? VideoUrl { get; set; }
    public string[] ImageUrls { get; set; } = [];
    public IList<string> Materials { get; set; } = [];
    public int Views { get; set; }
    public decimal Rating { get; set; }
    public int RatingsCount { get; set; }
    public bool IsPublished { get; set; }
    public DateTime CreatedAt { get; set; }

    public void Mapping(Profile profile)
        => profile.CreateMap<Masterclass, MasterclassLookupDto>()
            .ForMember(d => d.CategoryName,     o => o.MapFrom(s => s.Category.Name))
            .ForMember(d => d.AgeCategoryName,  o => o.MapFrom(s => s.AgeCategory.Name))
            .ForMember(d => d.AuthorName,       o => o.MapFrom(s => $"{s.Author.Name} {s.Author.Surname}"))
            .ForMember(d => d.Materials,        o => o.MapFrom(s => s.Materials.Select(m => m.Name).ToList()));
}

public class MasterclassListVm
{
    public IList<MasterclassLookupDto> Masterclasses { get; set; } = [];
}
