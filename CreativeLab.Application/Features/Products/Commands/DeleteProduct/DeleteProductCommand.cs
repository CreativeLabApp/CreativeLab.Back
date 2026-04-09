using CreativeLab.Application.Interfaces;
using MediatR;

namespace CreativeLab.Application.Features.Products.Commands.DeleteProduct;

public class DeleteProductCommand : IRequest
{
    public Guid Id { get; set; }
}

public class DeleteProductCommandHandler(ICreativeLabDbContext dbContext)
    : IRequestHandler<DeleteProductCommand>
{
    public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await dbContext.Products
            .FindAsync([request.Id], cancellationToken)
            ?? throw new InvalidOperationException("Product with this Id does not exist");

        dbContext.Products.Remove(product);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
