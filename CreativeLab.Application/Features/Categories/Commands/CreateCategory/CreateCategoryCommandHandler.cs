using CreativeLab.Application.Interfaces;
using CreativeLab.Domain;
using MediatR;

namespace CreativeLab.Application.Features.Categories.Commands.CreateCategory;

public class CreateCategoryCommandHandler(ICreativeLabDbContext dbContext)
    : IRequestHandler<CreateCategoryCommand, Category>
{
    public async Task<Category> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = new Category
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            Order = request.Order
        };

        await dbContext.Categories.AddAsync(category, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return category;
    }
}
