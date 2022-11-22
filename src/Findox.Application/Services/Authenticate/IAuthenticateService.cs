using Findox.Application.Dto.User;

namespace Findox.Application.Services.Authenticate;

public interface IAuthenticateService
{
    Task<AuthenticateResponse> AuthenticateAsync(AuthenticateRequest request);
}