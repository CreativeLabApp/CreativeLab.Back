using CreativeLab.Application.Interfaces;
using MediatR;

namespace CreativeLab.Application.Features.Categories.Commands.DeleteCategory;

public class DeleteCategoryCommand : IRequest
{
    public Guid Id { get; set; }
}

public class DeleteCategoryCommandHandler(ICreativeLabDbContext dbContext)
    : IRequestHandler<DeleteCategoryCommand>
{
    public async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await dbContext.Categories
            .FindAsync([request.Id], cancellationToken)
            ?? throw new InvalidOperationException("Category with this Id does not exist");

        dbContext.Categories.Remove(category);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
