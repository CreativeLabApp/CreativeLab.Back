using AutoMapper;
using AutoMapper.QueryableExtensions;
using CreativeLab.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CreativeLab.Application.Features.Masterclasses.Queries.GetMasterclassList;

public class GetMasterclassListQuery : IRequest<MasterclassListVm>
{
    public bool OnlyPublished { get; set; } = true;
    public Guid? AuthorId { get; set; }
}

public class GetMasterclassListQueryHandler(ICreativeLabDbContext dbContext, IMapper mapper)
    : IRequestHandler<GetMasterclassListQuery, MasterclassListVm>
{
    public async Task<MasterclassListVm> Handle(GetMasterclassListQuery request, CancellationToken cancellationToken)
    {
        var query = dbContext.Masterclasses.AsQueryable();

        if (request.OnlyPublished)
            query = query.Where(m => m.IsPublished);

        if (request.AuthorId.HasValue)
            query = query.Where(m => m.AuthorId == request.AuthorId.Value);

        var masterclasses = await query
            .Include(m => m.Author)
            .Include(m => m.Category)
            .Include(m => m.Materials)
            .OrderByDescending(m => m.CreatedAt)
            .ProjectTo<MasterclassLookupDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new MasterclassListVm { Masterclasses = masterclasses };
    }
}
