using CreativeLab.Domain;
using MediatR;

namespace CreativeLab.Application.Features.Tags.Commands.CreateTag;

public class CreateTagCommand : IRequest<Tag>
{
    public string Name { get; set; } = string.Empty;
    public string? Color { get; set; }
}
