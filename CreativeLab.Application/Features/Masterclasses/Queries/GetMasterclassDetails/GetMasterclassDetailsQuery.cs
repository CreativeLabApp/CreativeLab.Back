using AutoMapper;
using CreativeLab.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CreativeLab.Application.Features.Masterclasses.Queries.GetMasterclassDetails;

public class GetMasterclassDetailsQuery : IRequest<MasterclassDetailsVm>
{
    public Guid Id { get; set; }
}

public class GetMasterclassDetailsQueryHandler(ICreativeLabDbContext dbContext, IMapper mapper)
    : IRequestHandler<GetMasterclassDetailsQuery, MasterclassDetailsVm>
{
    public async Task<MasterclassDetailsVm> Handle(GetMasterclassDetailsQuery request, CancellationToken cancellationToken)
    {
        var masterclass = await dbContext.Masterclasses
            .Include(m => m.Author)
            .Include(m => m.Category)
            .Include(m => m.Materials)
            .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken)
            ?? throw new InvalidOperationException("Masterclass with this Id does not exist");

        return mapper.Map<MasterclassDetailsVm>(masterclass);
    }
}
