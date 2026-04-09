using CreativeLab.Domain;
using MediatR;

namespace CreativeLab.Application.Features.ChatParticipants.Commands.AddChatParticipant;

public class AddChatParticipantCommand : IRequest<ChatParticipant>
{
    public Guid ChatId { get; set; }
    public Guid UserId { get; set; }
}
