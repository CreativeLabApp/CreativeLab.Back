using CreativeLab.Domain;
using MediatR;

namespace CreativeLab.Application.Features.Users.Commands.UpdateUser;

public class UpdateUserCommand : IRequest<User>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
