using AutoMapper;
using CreativeLab.Application.Interfaces;
using MediatR;

namespace CreativeLab.Application.Features.Messages.Queries.GetMessageDetails;

public class GetMessageDetailsQuery : IRequest<MessageDetailsVm>
{
    public Guid Id { get; set; }
}

public class GetMessageDetailsQueryHandler(ICreativeLabDbContext dbContext, IMapper mapper)
    : IRequestHandler<GetMessageDetailsQuery, MessageDetailsVm>
{
    public async Task<MessageDetailsVm> Handle(GetMessageDetailsQuery request, CancellationToken cancellationToken)
    {
        var message = await dbContext.Messages
            .FindAsync([request.Id], cancellationToken)
            ?? throw new InvalidOperationException("Message with this Id does not exist");

        return mapper.Map<MessageDetailsVm>(message);
    }
}
