using Findox.Domain.Entities;

namespace Findox.Infra.Authentication
{
    public interface ITokenManager
    {
        Task<string> GenerateJwtTokenAsync(int userId, IEnumerable<Role> userRoles);
        int? ValidateJwtToken(string token);
    }
}
