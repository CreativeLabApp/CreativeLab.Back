using CreativeLab.Domain;
using MediatR;

namespace CreativeLab.Application.Features.Chats.Commands.CreateChat;

public class CreateChatCommand : IRequest<Chat>
{
    public string Title { get; set; } = string.Empty;
    public List<Guid> ParticipantUserIds { get; set; } = [];
}
