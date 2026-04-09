using CreativeLab.Domain;
using MediatR;

namespace CreativeLab.Application.Features.Users.Commands.CreateUser;

public class CreateUserCommand : IRequest<User>
{
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
