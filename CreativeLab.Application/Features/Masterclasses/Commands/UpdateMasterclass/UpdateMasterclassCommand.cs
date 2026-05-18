using CreativeLab.Domain;
using MediatR;

namespace CreativeLab.Application.Features.Masterclasses.Commands.UpdateMasterclass;

public class UpdateMasterclassCommand : IRequest<Masterclass>
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ShortDescription { get; set; }
    public Guid CategoryId { get; set; }
    public Guid AgeCategoryId { get; set; }
    public string[] ImageUrls { get; set; } = [];
    public string? ThumbnailUrl { get; set; }
    public string? VideoUrl { get; set; }
    public bool IsPublished { get; set; }
    public List<string> Materials { get; set; } = [];
}
