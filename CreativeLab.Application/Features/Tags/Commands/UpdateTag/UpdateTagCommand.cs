using CreativeLab.Domain;
using MediatR;

namespace CreativeLab.Application.Features.Tags.Commands.UpdateTag;

public class UpdateTagCommand : IRequest<Tag>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Color { get; set; }
}
