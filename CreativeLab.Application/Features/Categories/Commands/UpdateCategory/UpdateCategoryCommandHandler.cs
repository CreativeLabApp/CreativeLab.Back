using CreativeLab.Application.Interfaces;
using CreativeLab.Domain;
using MediatR;

namespace CreativeLab.Application.Features.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommandHandler(ICreativeLabDbContext dbContext)
    : IRequestHandler<UpdateCategoryCommand, Category>
{
    public async Task<Category> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await dbContext.Categories
            .FindAsync([request.Id], cancellationToken)
            ?? throw new InvalidOperationException("Category with this Id does not exist");

        category.Name = request.Name;
        category.Description = request.Description;
        category.Order = request.Order;

        await dbContext.SaveChangesAsync(cancellationToken);

        return category;
    }
}
