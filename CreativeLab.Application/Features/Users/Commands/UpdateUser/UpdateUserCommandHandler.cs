using CreativeLab.Application.Interfaces;
using CreativeLab.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CreativeLab.Application.Features.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler(ICreativeLabDbContext dbContext)
    : IRequestHandler<UpdateUserCommand, User>
{
    public async Task<User> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {   
        var user = await dbContext.Users
            .FindAsync([request.Id], cancellationToken)
            ?? throw new InvalidOperationException("User with this Id does not exists");

        user.Name = request.Name;
        user.Surname = request.Surname;
        user.Email = request.Email;
        user.UpdatedAt = DateTime.UtcNow;

        if (!string.IsNullOrWhiteSpace(request.Password))
        {
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        }

        var emailExists = await dbContext.Users
            .AnyAsync(u => u.Email == request.Email && u.Id != request.Id, cancellationToken);

        if (emailExists)
        {
            throw new InvalidOperationException("User with this email already exists");
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        return user;
    }
}
