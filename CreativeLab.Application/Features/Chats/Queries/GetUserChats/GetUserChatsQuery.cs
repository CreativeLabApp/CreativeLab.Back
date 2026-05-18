using AutoMapper;
using CreativeLab.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CreativeLab.Application.Features.Chats.Queries.GetUserChats;

public class GetUserChatsQuery : IRequest<List<ChatListItemVm>>
{
    public Guid UserId { get; set; }
}

public class ChatListItemVm
{
    public Guid Id { get; set; }
    public Guid ParticipantId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public string LastMessage { get; set; } = string.Empty;
    public DateTime? LastMessageTime { get; set; }
    public int UnreadCount { get; set; }
    public List<ParticipantInfo>? Participants { get; set; }
}

public class ParticipantInfo
{
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class GetUserChatsQueryHandler(ICreativeLabDbContext dbContext, IMapper mapper)
    : IRequestHandler<GetUserChatsQuery, List<ChatListItemVm>>
{
    public async Task<List<ChatListItemVm>> Handle(GetUserChatsQuery request, CancellationToken cancellationToken)
    {
        var userChats = await dbContext.ChatParticipants
            .Include(cp => cp.Chat)
                .ThenInclude(c => c.Messages.OrderByDescending(m => m.CreatedAt).Take(1))
            .Include(cp => cp.Chat)
                .ThenInclude(c => c.Participants)
                    .ThenInclude(p => p.User)
            .Where(cp => cp.UserId == request.UserId)
            .ToListAsync(cancellationToken);

        var result = new List<ChatListItemVm>();

        foreach (var chatParticipant in userChats)
        {
            var chat = chatParticipant.Chat;
            var otherParticipant = chat.Participants
                .FirstOrDefault(p => p.UserId != request.UserId);

            var lastMessage = chat.Messages.FirstOrDefault();
            var unreadCount = chat.Messages.Count(m => m.ReceiverId == request.UserId && !m.IsRead);

            result.Add(new ChatListItemVm
            {
                Id = chat.Id,
                ParticipantId = otherParticipant?.UserId ?? Guid.Empty,
                Name = otherParticipant?.User?.Name + " " + otherParticipant?.User?.Surname ?? "Unknown",
                AvatarUrl = null,
                LastMessage = lastMessage?.Content ?? "",
                LastMessageTime = lastMessage?.CreatedAt,
                UnreadCount = unreadCount,
                Participants = chat.Participants.Select(p => new ParticipantInfo
                {
                    UserId = p.UserId,
                    Name = p.User?.Name + " " + p.User?.Surname ?? "Unknown"
                }).ToList()
            });
        }

        return result.OrderByDescending(c => c.LastMessageTime).ToList();
    }
}