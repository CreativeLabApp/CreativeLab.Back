using AutoMapper;
using CreativeLab.Application.Common.Mappings;
using CreativeLab.Domain;

namespace CreativeLab.Application.Features.Messages.Queries.GetMessageDetails;

public class MessageDetailsVm : IMapWith<Message>
{
    public Guid Id { get; set; }
    public Guid ChatId { get; set; }
    public Guid SenderId { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime SentAt { get; set; }
    public bool IsDeleted { get; set; }
    public Guid? ReplyToMessageId { get; set; }

    public void Mapping(Profile profile)
        => profile.CreateMap<Message, MessageDetailsVm>();
}
