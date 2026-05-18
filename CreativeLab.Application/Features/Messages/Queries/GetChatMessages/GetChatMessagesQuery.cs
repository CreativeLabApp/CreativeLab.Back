using AutoMapper;
using CreativeLab.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CreativeLab.Application.Features.Messages.Queries.GetChatMessages;

public class GetChatMessagesQuery : IRequest<List<MessageListItemVm>>
{
    public Guid ChatId { get; set; }
    public Guid UserId { get; set; }
}

public class MessageListItemVm
{
    public Guid Id { get; set; }
    public Guid SenderId { get; set; }
    public Guid ReceiverId { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool IsRead { get; set; }
}

public class GetChatMessagesQueryHandler(ICreativeLabDbContext dbContext, IMapper mapper)
    : IRequestHandler<GetChatMessagesQuery, List<MessageListItemVm>>
{
    public async Task<List<MessageListItemVm>> Handle(GetChatMessagesQuery request, CancellationToken cancellationToken)
    {
        var messages = await dbContext.Messages
            .Where(m => m.ChatId == request.ChatId)
            .OrderBy(m => m.CreatedAt)
            .ToListAsync(cancellationToken);

        return messages.Select(m => new MessageListItemVm
        {
            Id = m.Id,
            SenderId = m.SenderId,
            ReceiverId = m.ReceiverId,
            Content = m.Content,
            CreatedAt = m.CreatedAt,
            IsRead = m.IsRead
        }).ToList();
    }
}