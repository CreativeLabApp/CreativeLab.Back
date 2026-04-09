using AutoMapper;
using CreativeLab.Application.Features.Tags.Commands.CreateTag;
using CreativeLab.Application.Features.Tags.Commands.DeleteTag;
using CreativeLab.Application.Features.Tags.Commands.UpdateTag;
using CreativeLab.Application.Features.Tags.Queries.GetTagDetails;
using CreativeLab.WebApi.Dto;
using Microsoft.AspNetCore.Mvc;

namespace CreativeLab.WebApi.Controllers;

public class TagController(IMapper mapper) : BaseController
{
    [HttpGet]
    public async Task<ActionResult<TagDetailsVm>> Get([FromQuery] Guid id)
    {
        var vm = await Mediator.Send(new GetTagDetailsQuery { Id = id });
        return Ok(vm);
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateTagDto dto)
    {
        var command = mapper.Map<CreateTagCommand>(dto);
        var tag = await Mediator.Send(command);
        return Ok(tag);
    }

    [HttpPut]
    public async Task<ActionResult> Update([FromBody] UpdateTagDto dto)
    {
        var command = mapper.Map<UpdateTagCommand>(dto);
        var tag = await Mediator.Send(command);
        return Ok(tag);
    }

    [HttpDelete]
    public async Task<ActionResult> Delete([FromQuery] Guid id)
    {
        await Mediator.Send(new DeleteTagCommand { Id = id });
        return NoContent();
    }
}
