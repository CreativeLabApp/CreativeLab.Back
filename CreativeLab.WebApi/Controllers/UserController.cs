using AutoMapper;
using CreativeLab.Application.Features.Users.Commands.DeleteUser;
using CreativeLab.Application.Features.Users.Commands.ToggleUserActive;
using CreativeLab.Application.Features.Users.Commands.UpdateUser;
using CreativeLab.Application.Features.Users.Queries.GetUserDetails;
using CreativeLab.Application.Features.Users.Queries.GetUserList;
using CreativeLab.WebApi.Dto;
using Microsoft.AspNetCore.Mvc;

namespace CreativeLab.WebApi.Controllers;

public class UserController(IMapper mapper) : BaseController
{
    [HttpGet]
    public async Task<ActionResult<List<UserLookupDto>>> GetAll()
    {
        var users = await Mediator.Send(new GetUserListQuery());
        return Ok(users);
    }

    [HttpGet]
    public async Task<ActionResult<UserDetailsVm>> Get([FromQuery] Guid UserId)
    {
        var user = await Mediator.Send(new GetUserDetailsQuery { Id = UserId });
        return Ok(user);
    }

    [HttpPut]
    public async Task<ActionResult> Update([FromBody] UpdateUserDto dto)
    {
        var command = mapper.Map<UpdateUserCommand>(dto);
        await Mediator.Send(command);
        return NoContent();
    }

    [HttpDelete]
    public async Task<ActionResult> Delete([FromQuery] Guid id)
    {
        await Mediator.Send(new DeleteUserCommand { Id = id });
        return NoContent();
    }

    [HttpPatch]
    public async Task<ActionResult<bool>> ToggleActive([FromQuery] Guid id)
    {
        var isActive = await Mediator.Send(new ToggleUserActiveCommand { Id = id });
        return Ok(new { isActive });
    }
}
