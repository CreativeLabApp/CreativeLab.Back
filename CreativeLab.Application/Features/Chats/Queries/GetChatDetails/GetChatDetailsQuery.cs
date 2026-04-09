using AutoMapper;
using CreativeLab.Application.Interfaces;
using MediatR;

namespace CreativeLab.Application.Features.Chats.Queries.GetChatDetails;

public class GetChatDetailsQuery : IRequest<ChatDetailsVm>
{
    public Guid Id { get; set; }
}

public class GetChatDetailsQueryHandler(ICreativeLabDbContext dbContext, IMapper mapper)
    : IRequestHandler<GetChatDetailsQuery, ChatDetailsVm>
{
    public async Task<ChatDetailsVm> Handle(GetChatDetailsQuery request, CancellationToken cancellationToken)
    {
        var chat = await dbContext.Chats
            .FindAsync([request.Id], cancellationToken)
            ?? throw new InvalidOperationException("Chat with this Id does not exist");

        return mapper.Map<ChatDetailsVm>(chat);
    }
}
