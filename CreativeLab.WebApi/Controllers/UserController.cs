using CreativeLab.Application.Features.Users.Queries.GetUserDetails;
using Microsoft.AspNetCore.Mvc;

namespace CreativeLab.WebApi.Controllers;

public class UserController : BaseController
{
    [HttpGet]
    public async Task<ActionResult<UserDetailsVm>> Get([FromQuery] Guid UserId)
    {
        var query = new GetUserDetailsQuery()
        {
            Id = UserId
        };

        var user = await Mediator.Send(query);

        return Ok(user);
    }
}
