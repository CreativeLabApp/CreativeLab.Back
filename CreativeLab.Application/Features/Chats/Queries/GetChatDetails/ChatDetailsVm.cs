using AutoMapper;
using CreativeLab.Application.Common.Mappings;
using CreativeLab.Domain;

namespace CreativeLab.Application.Features.Chats.Queries.GetChatDetails;

public class ChatDetailsVm : IMapWith<Chat>
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? LastMessageAt { get; set; }
    public bool IsArchived { get; set; }
    public Guid? LastMessageId { get; set; }

    public void Mapping(Profile profile)
        => profile.CreateMap<Chat, ChatDetailsVm>();
}
