using AutoMapper;
using CreativeLab.Application.Features.ChatParticipants.Commands.AddChatParticipant;
using CreativeLab.Application.Features.ChatParticipants.Commands.RemoveChatParticipant;
using CreativeLab.Application.Features.Chats.Commands.CreateChat;
using CreativeLab.Application.Features.Chats.Commands.DeleteChat;
using CreativeLab.Application.Features.Chats.Queries.GetChatDetails;
using CreativeLab.WebApi.Dto;
using Microsoft.AspNetCore.Mvc;

namespace CreativeLab.WebApi.Controllers;

public class ChatController(IMapper mapper) : BaseController
{
    [HttpGet]
    public async Task<ActionResult<ChatDetailsVm>> Get([FromQuery] Guid id)
    {
        var vm = await Mediator.Send(new GetChatDetailsQuery { Id = id });
        return Ok(vm);
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateChatDto dto)
    {
        var command = mapper.Map<CreateChatCommand>(dto);
        var chat = await Mediator.Send(command);
        return Ok(chat);
    }

    [HttpDelete]
    public async Task<ActionResult> Delete([FromQuery] Guid id)
    {
        await Mediator.Send(new DeleteChatCommand { Id = id });
        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult> AddParticipant([FromBody] AddChatParticipantDto dto)
    {
        var command = mapper.Map<AddChatParticipantCommand>(dto);
        var participant = await Mediator.Send(command);
        return Ok(participant);
    }

    [HttpDelete]
    public async Task<ActionResult> RemoveParticipant([FromBody] RemoveChatParticipantDto dto)
    {
        var command = mapper.Map<RemoveChatParticipantCommand>(dto);
        await Mediator.Send(command);
        return NoContent();
    }
}
