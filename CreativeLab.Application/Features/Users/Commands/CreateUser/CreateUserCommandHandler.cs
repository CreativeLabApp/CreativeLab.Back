using CreativeLab.Application.Interfaces;
using CreativeLab.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CreativeLab.Application.Features.Users.Commands.CreateUser;

public class CreateUserCommandHandler(ICreativeLabDbContext dbContext)
    : IRequestHandler<CreateUserCommand, User>
{
    public async Task<User> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await dbContext.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken: cancellationToken);

        if (existingUser is not null)
        {
            throw new InvalidOperationException("User with this email already exists");
        }

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var newUser = new User()
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Surname = request.Surname,
            Email = request.Email,
            PasswordHash = hashedPassword,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = null
        };

        await dbContext.Users.AddAsync(newUser, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return newUser;
    }
}
