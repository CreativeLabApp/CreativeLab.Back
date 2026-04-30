using CreativeLab.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CreativeLab.Application.Features.Categories.Queries.GetCategoryList;

public class GetCategoryListQuery : IRequest<List<CategoryLookupDto>> { }

public class CategoryLookupDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Order { get; set; }
}

public class GetCategoryListQueryHandler(ICreativeLabDbContext dbContext)
    : IRequestHandler<GetCategoryListQuery, List<CategoryLookupDto>>
{
    public async Task<List<CategoryLookupDto>> Handle(GetCategoryListQuery request, CancellationToken cancellationToken)
    {
        return await dbContext.Categories
            .OrderBy(c => c.Order)
            .Select(c => new CategoryLookupDto { Id = c.Id, Name = c.Name, Order = c.Order })
            .ToListAsync(cancellationToken);
    }
}
