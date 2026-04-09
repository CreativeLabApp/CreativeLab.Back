using AutoMapper;
using CreativeLab.Application.Features.Masterclasses.Commands.CreateMasterclass;
using CreativeLab.Application.Features.Masterclasses.Commands.DeleteMasterclass;
using CreativeLab.Application.Features.Masterclasses.Commands.UpdateMasterclass;
using CreativeLab.Application.Features.Masterclasses.Queries.GetMasterclassDetails;
using CreativeLab.WebApi.Dto;
using Microsoft.AspNetCore.Mvc;

namespace CreativeLab.WebApi.Controllers;

public class MasterclassController(IMapper mapper) : BaseController
{
    [HttpGet]
    public async Task<ActionResult<MasterclassDetailsVm>> Get([FromQuery] Guid id)
    {
        var vm = await Mediator.Send(new GetMasterclassDetailsQuery { Id = id });
        return Ok(vm);
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateMasterclassDto dto)
    {
        var command = mapper.Map<CreateMasterclassCommand>(dto);
        var masterclass = await Mediator.Send(command);
        return Ok(masterclass);
    }

    [HttpPut]
    public async Task<ActionResult> Update([FromBody] UpdateMasterclassDto dto)
    {
        var command = mapper.Map<UpdateMasterclassCommand>(dto);
        var masterclass = await Mediator.Send(command);
        return Ok(masterclass);
    }

    [HttpDelete]
    public async Task<ActionResult> Delete([FromQuery] Guid id)
    {
        await Mediator.Send(new DeleteMasterclassCommand { Id = id });
        return NoContent();
    }
}
