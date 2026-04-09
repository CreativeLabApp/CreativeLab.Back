using CreativeLab.Domain;
using MediatR;

namespace CreativeLab.Application.Features.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommand : IRequest<Category>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Order { get; set; }
}
