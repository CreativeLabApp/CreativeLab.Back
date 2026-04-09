using AutoMapper;
using CreativeLab.Application.Interfaces;
using MediatR;

namespace CreativeLab.Application.Features.Tags.Queries.GetTagDetails;

public class GetTagDetailsQuery : IRequest<TagDetailsVm>
{
    public Guid Id { get; set; }
}

public class GetTagDetailsQueryHandler(ICreativeLabDbContext dbContext, IMapper mapper)
    : IRequestHandler<GetTagDetailsQuery, TagDetailsVm>
{
    public async Task<TagDetailsVm> Handle(GetTagDetailsQuery request, CancellationToken cancellationToken)
    {
        var tag = await dbContext.Tags
            .FindAsync([request.Id], cancellationToken)
            ?? throw new InvalidOperationException("Tag with this Id does not exist");

        return mapper.Map<TagDetailsVm>(tag);
    }
}
