using AutoMapper;
using CreativeLab.Application.Features.Users.Commands.CreateUser;
using CreativeLab.Application.Features.Users.Queries.GetUserDetails;
using CreativeLab.Application.Features.Users.Queries.LoginUser;
using CreativeLab.Application.Interfaces;
using CreativeLab.WebApi.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CreativeLab.WebApi.Controllers;

public class AuthController(IMapper mapper, IJwtService jwtService, ICreativeLabDbContext dbContext) : BaseController
{
    private const string RefreshTokenCookie = "refreshToken";

    private void SetRefreshCookie(string token)
    {
        Response.Cookies.Append(RefreshTokenCookie, token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTimeOffset.UtcNow.AddDays(30),
        });
    }

    [HttpPost]
    public async Task<ActionResult<LoginUserVm>> Register([FromBody] RegisterDto dto)
    {
        var command = mapper.Map<CreateUserCommand>(dto);
        await Mediator.Send(command);
        return await LoginInternal(dto.Email, dto.Password);
    }

    [HttpPost]
    public async Task<ActionResult<LoginUserVm>> Login([FromBody] LoginDto dto)
    {
        return await LoginInternal(dto.Email, dto.Password);
    }

    [HttpPost]
    public async Task<ActionResult<RefreshResultDto>> Refresh()
    {
        var refreshTokenValue = Request.Cookies[RefreshTokenCookie];
        if (string.IsNullOrEmpty(refreshTokenValue))
            return Unauthorized("Refresh token missing");

        var storedToken = await dbContext.RefreshTokens
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.Token == refreshTokenValue);

        if (storedToken is null || storedToken.IsRevoked || storedToken.ExpiresAt < DateTime.UtcNow)
            return Unauthorized("Refresh token invalid or expired");

        // Ротация: отзываем старый, выдаём новый
        storedToken.IsRevoked = true;

        var newRefreshValue = jwtService.GenerateRefreshToken();
        var newRefresh = new Domain.RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = newRefreshValue,
            UserId = storedToken.UserId,
            ExpiresAt = DateTime.UtcNow.AddDays(30),
            CreatedAt = DateTime.UtcNow,
        };

        await dbContext.RefreshTokens.AddAsync(newRefresh);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        SetRefreshCookie(newRefreshValue);

        return Ok(new RefreshResultDto
        {
            AccessToken = jwtService.GenerateAccessToken(storedToken.User),
        });
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult> Logout()
    {
        var refreshTokenValue = Request.Cookies[RefreshTokenCookie];
        if (!string.IsNullOrEmpty(refreshTokenValue))
        {
            var token = await dbContext.RefreshTokens
                .FirstOrDefaultAsync(t => t.Token == refreshTokenValue);
            if (token is not null) token.IsRevoked = true;
            await dbContext.SaveChangesAsync(CancellationToken.None);
        }

        Response.Cookies.Delete(RefreshTokenCookie);
        return NoContent();
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<UserDetailsVm>> Me()
    {
        if (UserId == Guid.Empty) return Unauthorized();
        var user = await Mediator.Send(new GetUserDetailsQuery { Id = UserId });
        return Ok(user);
    }

    private async Task<ActionResult<LoginUserVm>> LoginInternal(string email, string password)
    {
        var result = await Mediator.Send(new LoginUserQuery { Email = email, Password = password });

        // Refresh token — в httpOnly cookie
        var refreshToken = await dbContext.RefreshTokens
            .Where(t => t.UserId == result.Id && !t.IsRevoked)
            .OrderByDescending(t => t.CreatedAt)
            .FirstOrDefaultAsync();

        if (refreshToken is not null)
            SetRefreshCookie(refreshToken.Token);

        return Ok(result);
    }
}

public class RefreshResultDto
{
    public string AccessToken { get; set; } = string.Empty;
}
