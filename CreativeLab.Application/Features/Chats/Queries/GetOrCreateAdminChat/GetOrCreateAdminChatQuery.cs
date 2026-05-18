using AutoMapper;
using CreativeLab.Application.Features.Chats.Queries.GetUserChats;
using CreativeLab.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CreativeLab.Application.Features.Chats.Queries.GetOrCreateAdminChat;

public class GetOrCreateAdminChatQuery : IRequest<ChatListItemVm>
{
    public Guid UserId { get; set; }
}

public class GetOrCreateAdminChatQueryHandler(ICreativeLabDbContext dbContext, IMapper mapper)
    : IRequestHandler<GetOrCreateAdminChatQuery, ChatListItemVm>
{
    public async Task<ChatListItemVm> Handle(GetOrCreateAdminChatQuery request, CancellationToken cancellationToken)
    {
        // Ищем админа (первый пользователь с ролью admin или конкретный пользователь)
        var admin = await dbContext.Users
            .FirstOrDefaultAsync(u => u.IsActive, cancellationToken)
            ?? throw new InvalidOperationException("Admin user not found");

        // Ищем существующий чат между пользователем и админом
        var existingChat = await dbContext.ChatParticipants
            .Include(cp => cp.Chat)
                .ThenInclude(c => c.Messages.OrderByDescending(m => m.CreatedAt).Take(1))
            .Include(cp => cp.Chat)
                .ThenInclude(c => c.Participants)
                    .ThenInclude(p => p.User)
            .Where(cp => cp.UserId == request.UserId)
            .Select(cp => cp.Chat)
            .Where(c => c.Participants.Any(p => p.UserId == admin.Id))
            .FirstOrDefaultAsync(cancellationToken);

        if (existingChat != null)
        {
            var otherParticipant = existingChat.Participants
                .FirstOrDefault(p => p.UserId != request.UserId);

            var lastMessage = existingChat.Messages.FirstOrDefault();
            var unreadCount = existingChat.Messages.Count(m => m.ReceiverId == request.UserId && !m.IsRead);

            return new ChatListItemVm
            {
                Id = existingChat.Id,
                ParticipantId = otherParticipant?.UserId ?? admin.Id,
                Name = "Поддержка",
                AvatarUrl = null,
                LastMessage = lastMessage?.Content ?? "",
                LastMessageTime = lastMessage?.CreatedAt,
                UnreadCount = unreadCount,
                Participants = existingChat.Participants.Select(p => new ParticipantInfo
                {
                    UserId = p.UserId,
                    Name = p.User?.Name + " " + p.User?.Surname ?? "Unknown"
                }).ToList()
            };
        }

        // Создаем новый чат с админом
        var newChat = new Domain.Chat
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            Participants = new List<Domain.ChatParticipant>
            {
                new Domain.ChatParticipant
                {
                    Id = Guid.NewGuid(),
                    UserId = request.UserId,
                    JoinedAt = DateTime.UtcNow
                },
                new Domain.ChatParticipant
                {
                    Id = Guid.NewGuid(),
                    UserId = admin.Id,
                    JoinedAt = DateTime.UtcNow
                }
            }
        };

        await dbContext.Chats.AddAsync(newChat, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new ChatListItemVm
        {
            Id = newChat.Id,
            ParticipantId = admin.Id,
            Name = "Поддержка",
            AvatarUrl = null,
            LastMessage = "",
            LastMessageTime = null,
            UnreadCount = 0,
            Participants = new List<ParticipantInfo>
            {
                new ParticipantInfo { UserId = request.UserId, Name = "Пользователь" },
                new ParticipantInfo { UserId = admin.Id, Name = "Поддержка" }
            }
        };
    }
}