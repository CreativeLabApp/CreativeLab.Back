using CreativeLab.Application.Interfaces;
using CreativeLab.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CreativeLab.Application.Features.Users.Queries.LoginUser;

public class LoginUserQuery : IRequest<LoginUserVm>
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class LoginUserQueryHandler(ICreativeLabDbContext dbContext, IJwtService jwtService)
    : IRequestHandler<LoginUserQuery, LoginUserVm>
{
    public async Task<LoginUserVm> Handle(LoginUserQuery request, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken)
            ?? throw new UnauthorizedAccessException("Invalid email or password");

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid email or password");

        if (!user.IsActive)
            throw new UnauthorizedAccessException("Account is deactivated");

        var accessToken = jwtService.GenerateAccessToken(user);
        var refreshTokenValue = jwtService.GenerateRefreshToken();

        // Отзываем старые refresh токены пользователя
        var oldTokens = await dbContext.RefreshTokens
            .Where(t => t.UserId == user.Id && !t.IsRevoked)
            .ToListAsync(cancellationToken);
        foreach (var t in oldTokens) t.IsRevoked = true;

        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = refreshTokenValue,
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(30),
            CreatedAt = DateTime.UtcNow,
        };

        await dbContext.RefreshTokens.AddAsync(refreshToken, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new LoginUserVm
        {
            AccessToken = accessToken,
            Id = user.Id,
            Name = user.Name,
            Surname = user.Surname,
            Email = user.Email,
        };
    }
}
