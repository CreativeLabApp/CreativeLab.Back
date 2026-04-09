using AutoMapper;
using CreativeLab.Application.Interfaces;
using MediatR;

namespace CreativeLab.Application.Features.Categories.Queries.GetCategoryDetails;

public class GetCategoryDetailsQuery : IRequest<CategoryDetailsVm>
{
    public Guid Id { get; set; }
}

public class GetCategoryDetailsQueryHandler(ICreativeLabDbContext dbContext, IMapper mapper)
    : IRequestHandler<GetCategoryDetailsQuery, CategoryDetailsVm>
{
    public async Task<CategoryDetailsVm> Handle(GetCategoryDetailsQuery request, CancellationToken cancellationToken)
    {
        var category = await dbContext.Categories
            .FindAsync([request.Id], cancellationToken)
            ?? throw new InvalidOperationException("Category with this Id does not exist");

        return mapper.Map<CategoryDetailsVm>(category);
    }
}
