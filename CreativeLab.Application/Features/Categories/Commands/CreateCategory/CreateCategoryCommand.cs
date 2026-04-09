using CreativeLab.Domain;
using MediatR;

namespace CreativeLab.Application.Features.Categories.Commands.CreateCategory;

public class CreateCategoryCommand : IRequest<Category>
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Order { get; set; }
}
