using AutoMapper;
using CreativeLab.Application.Features.Messages.Commands.CreateMessage;
using CreativeLab.Application.Features.Messages.Commands.DeleteMessage;
using CreativeLab.Application.Features.Messages.Queries.GetMessageDetails;
using CreativeLab.WebApi.Dto;
using Microsoft.AspNetCore.Mvc;

namespace CreativeLab.WebApi.Controllers;

public class MessageController(IMapper mapper) : BaseController
{
    [HttpGet]
    public async Task<ActionResult<MessageDetailsVm>> Get([FromQuery] Guid id)
    {
        var vm = await Mediator.Send(new GetMessageDetailsQuery { Id = id });
        return Ok(vm);
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateMessageDto dto)
    {
        var command = mapper.Map<CreateMessageCommand>(dto);
        var message = await Mediator.Send(command);
        return Ok(message);
    }

    [HttpDelete]
    public async Task<ActionResult> Delete([FromQuery] Guid id)
    {
        await Mediator.Send(new DeleteMessageCommand { Id = id });
        return NoContent();
    }
}
