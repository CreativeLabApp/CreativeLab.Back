using AutoMapper;
using CreativeLab.Application.Common.Mappings;
using CreativeLab.Domain;

namespace CreativeLab.Application.Features.Masterclasses.Queries.GetMasterclassDetails;

public class MasterclassDetailsVm : IMapWith<Masterclass>
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ShortDescription { get; set; }
    public Guid CategoryId { get; set; }
    public Guid AuthorId { get; set; }
    public string[] ImageUrls { get; set; } = [];
    public string? ThumbnailUrl { get; set; }
    public int Views { get; set; }
    public decimal Rating { get; set; }
    public int RatingsCount { get; set; }
    public bool IsPublished { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? PublishedAt { get; set; }

    public void Mapping(Profile profile)
        => profile.CreateMap<Masterclass, MasterclassDetailsVm>();
}
