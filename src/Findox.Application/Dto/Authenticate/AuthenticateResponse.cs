namespace Findox.Application.Dto.User;

public class AuthenticateResponse
{
    public string? Username { get; set; }
    public string Token { get; set; }
    public string[]? Roles { get; set; }

    public AuthenticateResponse(Domain.Entities.Account user, string token)
    {
        Username = user.Username;
        Token = token;
        Roles = user.Roles.Select(x => x.Name).ToArray();
    }
}