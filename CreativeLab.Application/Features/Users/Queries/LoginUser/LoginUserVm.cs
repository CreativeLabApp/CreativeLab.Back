namespace CreativeLab.Application.Features.Users.Queries.LoginUser;

public class LoginUserVm
{
    public string AccessToken { get; set; } = string.Empty;
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
