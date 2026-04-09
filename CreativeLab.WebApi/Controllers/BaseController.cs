using CreativeLab.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CreativeLab.WebApi.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public abstract class BaseController : ControllerBase
{
    private IMediator _mediator = null!;

    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>()
        ?? throw new NullReferenceException("Mediator is null.");

    protected Guid UserId
    {
        get
        {
            if (!User.Identity?.IsAuthenticated ?? false)
                return Guid.Empty;

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)
                ?? User.FindFirst("sub")
                ?? User.FindFirst("uid");

            if (userIdClaim == null || string.IsNullOrWhiteSpace(userIdClaim.Value))
                return Guid.Empty;

            return Guid.TryParse(userIdClaim.Value, out var userId)
                ? userId
                : Guid.Empty;
        }
    }
}

