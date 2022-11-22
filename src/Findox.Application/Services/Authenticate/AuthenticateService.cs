using Findox.Application.Dto.User;
using Findox.Infra.Authentication;
using Findox.Infra.Data.Repositories.User;
using Findox.Shared.Exceptions;

namespace Findox.Application.Services.Authenticate;

public class AuthenticateService : IAuthenticateService
{
    private readonly ITokenManager _tokenManager;
    private readonly IAccountRepository _userRepository;

    public AuthenticateService(ITokenManager tokenManager,
        IAccountRepository userRepository)
    {
        _tokenManager = tokenManager ?? throw new ArgumentNullException(nameof(tokenManager));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    public async Task<AuthenticateResponse> AuthenticateAsync(AuthenticateRequest request)
    {
        var user = await _userRepository.GetByUsernameAsync(request.Username);

        if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new AppException("Username or password is incorrect");

        var jwtToken = await _tokenManager.GenerateJwtTokenAsync(user.UserId, user.Roles);

        return new AuthenticateResponse(user, jwtToken);
    }
}